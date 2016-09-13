using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Exceptions;
using Domain.Infrastructure;
using Domain.MissingValuePolicy;
using Domain.Repositories;
using Mapster;

namespace Domain.Services.Implementation
{
    [UnityRegister]
    public class SignalsDomainService : ISignalsDomainService
    {
        private readonly ISignalsRepository signalsRepository;
        private readonly ISignalsDataRepository signalsDataRepository;
        private readonly IMissingValuePolicyRepository missingValuePolicyRepository;

        public SignalsDomainService(
            ISignalsRepository signalsRepository,
            ISignalsDataRepository signalsDataRepository,
            IMissingValuePolicyRepository missingValuePolicyRepository)
        {
            this.signalsRepository = signalsRepository;
            this.signalsDataRepository = signalsDataRepository;
            this.missingValuePolicyRepository = missingValuePolicyRepository;
        }

        public Signal Add(Signal newSignal)
        {
            if (newSignal.Id.HasValue)
                throw new Exceptions.IdNotNullException();

            var result = signalsRepository.Add(newSignal);
            switch(result.DataType)
            {
                case (DataType.Boolean):
                    missingValuePolicyRepository.Set(result, new NoneQualityMissingValuePolicy<bool>());
                    break;

                case (DataType.Decimal):
                    missingValuePolicyRepository.Set(result, new NoneQualityMissingValuePolicy<decimal>());
                    break;

                case (DataType.Double):
                    missingValuePolicyRepository.Set(result, new NoneQualityMissingValuePolicy<double>());
                    break;

                case (DataType.Integer):
                    missingValuePolicyRepository.Set(result, new NoneQualityMissingValuePolicy<int>());
                    break;

                case (DataType.String):
                    missingValuePolicyRepository.Set(result, new NoneQualityMissingValuePolicy<string>());
                    break;
            }
            return result;
        }

        public Signal GetById(int signalId)
        {
            return this.signalsRepository.Get(signalId);
        }

        public Signal GetByPath(Path signalPath)
        {
            if (signalPath == null)
                throw new ArgumentNullException("Attempted to get signal with null path");

            var result = this.signalsRepository.Get(signalPath);
            return result;
        }
        
        public MissingValuePolicyBase GetMissingValuePolicy(int signalId)
        {
            var signal = this.GetById(signalId);
            if (signal == null)
                throw new NoSuchSignalException("Cannot get missing value policy for not exisitng signal");

            var mvp = this.missingValuePolicyRepository.Get(signal);

            if (mvp == null)
                return null;

            return TypeAdapter.Adapt(mvp, mvp.GetType(), mvp.GetType().BaseType)
                as MissingValuePolicy.MissingValuePolicyBase;
        }

        public void SetMissingValuePolicy(int signalId, MissingValuePolicyBase policy)
        {
            var signal = GetById(signalId);
            if (signal == null)
                throw new NoSuchSignalException("Attempted to set missing value policy to a non exsisting signal");
            
            policy.CheckGranularityAndDataType(signal);

            this.missingValuePolicyRepository.Set(signal, policy);
        }
        
        public IEnumerable<Datum<T>> GetData<T>(Signal signal, DateTime fromIncludedUtc, DateTime toExcludedUtc)
        {
            if (!VeryfiTimeStamp(signal.Granularity, fromIncludedUtc))
                throw new QuerryAboutDateWithIncorrectFormatException();

            var result = signalsDataRepository.GetData<T>(signal, fromIncludedUtc, toExcludedUtc)?.ToArray();

            if (result == null)
                return null;

            if(fromIncludedUtc == toExcludedUtc)
                return SetResultForEqualTimestamps(result, fromIncludedUtc, toExcludedUtc);

            result = result.OrderBy(d => d.Timestamp).ToArray();

            var mvp = GetMissingValuePolicy(signal.Id.Value);
            if (mvp != null)
            {
                List<Datum<T>> datums = new List<Datum<T>>();
                var date = fromIncludedUtc;
               
                if (typeof(SpecificValueMissingValuePolicy<T>) == mvp.GetType() || typeof(NoneQualityMissingValuePolicy<T>) == mvp.GetType() || typeof(ZeroOrderMissingValuePolicy<T>) == mvp.GetType())
                {
                    SetDatum(mvp, ref datums, result, signal, date, toExcludedUtc);
                }
                else if (mvp.GetType() == typeof(FirstOrderMissingValuePolicy<T>))
                {
                    if (signal.DataType.GetNativeType().Name == "Boolean" || signal.DataType.GetNativeType().Name == "String")
                        throw new ArgumentException("Boolean and String types are not supported.");

                    SetDatumForFirstOrderMissingValuePolicy(result, ref datums, signal, date, toExcludedUtc);
                }
                else if (mvp.GetType() == typeof(ShadowMissingValuePolicy<T>))
                {
                    SetDatumForShadowMissingValuePolicy(mvp, result, ref datums, signal, date, fromIncludedUtc, toExcludedUtc);
                }
                return datums?.ToArray();
            }
            return result;
        }

        private IEnumerable<Datum<T>> SetResultForEqualTimestamps<T>(Datum<T>[] result, DateTime fromIncludedUtc, DateTime toExcludedUtc)
        {
                if (result.Count() == 0)
                {
                    var resultList = result.ToList();
                    resultList.Add(new Datum<T>()
                    {
                        Timestamp = fromIncludedUtc,
                        Quality = Quality.None,
                        Value = default(T)
                    });
                    return resultList;
                }
                else
                    return result;
        }

        private void SetDatum<T>(MissingValuePolicyBase mvp, ref List<Datum<T>> datums, Datum<T>[] result, Signal signal, DateTime date, DateTime toExcludedUtc)
        {

            T value = default(T);
            Quality quality = default(Quality);

            var mvpSpec = mvp.Adapt(mvp, mvp.GetType(), mvp.GetType().BaseType)
                    as MissingValuePolicy.SpecificValueMissingValuePolicy<T>;
            if (date == toExcludedUtc)
            {
                if ((mvp.GetType() == typeof(SpecificValueMissingValuePolicy<T>)))
                    datums.Add(new Datum<T>() { Signal = signal, Quality = mvpSpec.GetQuality(), Timestamp = date, Value = mvpSpec.Value });
            }
            else
            {
                
                if (mvp.GetType() == typeof(ZeroOrderMissingValuePolicy<T>))
                {
                    if ((result.LastOrDefault(x => x == x).Timestamp) < date)
                    {
                        var dataOlder = result.Last(x => x == x);
                        quality = dataOlder.Quality;
                        value = dataOlder.Value;
                    }
                    while (date < toExcludedUtc)
                    {
                        Datum<T> elementOfList = result.FirstOrDefault(x => x.Timestamp == date);
                        if (elementOfList != null)
                        {
                            quality = elementOfList.Quality;
                            value = elementOfList.Value;

                        }
                        datums.Add(new Datum<T>() { Signal = signal, Quality = quality, Timestamp = date, Value = value });
                        date = AddingTimespanToDataTime(date, signal.Granularity);
                    }
                }
                else
                {
                    while (date < toExcludedUtc)
                    {
                        var actualDate = result.FirstOrDefault(d => d.Timestamp == date);

                        if (actualDate != null)
                        {
                            value = actualDate.Value;
                            quality = actualDate.Quality;
                            datums.Add(actualDate);
                        }
                        else if ((mvp.GetType() == typeof(NoneQualityMissingValuePolicy<T>)))
                        {
                            datums.Add(new Datum<T>() { Quality = Quality.None, Timestamp = date, Value = default(T) });
                        }
                        else
                        {
                            datums.Add(new Datum<T>() { Quality = mvpSpec.Quality, Timestamp = date, Value = mvpSpec.Value });
                        }
                        increaseDate(ref date, signal.Granularity);
                    }
                }
            }
            
           
        }
        
        private void SetDatumForFirstOrderMissingValuePolicy<T>(Datum<T>[] result, ref List<Datum<T>> datums, Signal signal, DateTime fromIncludedUtc, DateTime toExcludedUtc)
        {
            var datumsFirst = new List<Datum<T>>();

            while (fromIncludedUtc < toExcludedUtc)
            {
                var dataOlder = signalsDataRepository.GetDataOlderThan<T>(signal, fromIncludedUtc, 1);
                var dataNewer = signalsDataRepository.GetDataNewerThan<T>(signal, fromIncludedUtc, 1);
                if ((dataOlder == null) || (dataOlder.Count() == 0))
                {
                    if (result.FirstOrDefault(x => x.Timestamp == fromIncludedUtc) == null)
                    {
                        datumsFirst.Add(new Datum<T>()
                        {
                            Quality = Quality.None,
                            Value = default(T),
                            Timestamp = fromIncludedUtc
                        });
                    }
                    else
                    {
                        datumsFirst.Add(result.First(x => x.Timestamp == fromIncludedUtc));
                    }
                    fromIncludedUtc = AddingTimespanToDataTime(fromIncludedUtc, signal.Granularity);
                }
                else
                if ((dataNewer == null) || (dataNewer.Count() == 0))
                {
                    if (result.FirstOrDefault(x => x.Timestamp == fromIncludedUtc) == null)
                    {
                        datumsFirst.Add(new Datum<T>()
                        {
                            Quality = Quality.None,
                            Value = default(T),
                            Timestamp = fromIncludedUtc
                        });
                    }
                    fromIncludedUtc = AddingTimespanToDataTime(fromIncludedUtc, signal.Granularity);
                }
                else
                {
                    switch (signal.DataType)
                    {
                        case DataType.Boolean:
                            {
                                break;
                            }
                        case DataType.Decimal:
                            {
                                var value = Convert.ToDecimal(dataOlder.ElementAt(0).Value);
                                Quality quality = ChoiseQuality(dataNewer.ElementAt(0).Quality, dataOlder.ElementAt(0).Quality);
                                var substractTime = SetTotalSteps(dataNewer.ElementAt(0).Timestamp, dataOlder.ElementAt(0).Timestamp, signal.Granularity);
                                var substractValue = Convert.ToDecimal(dataNewer.ElementAt(0).Value) - Convert.ToDecimal(dataOlder.ElementAt(0).Value);
                                if (Convert.ToInt16(substractTime) > SetTotalSteps(toExcludedUtc, fromIncludedUtc, signal.Granularity))
                                {
                                    int valueIteration = Convert.ToInt16(SetTotalSteps(toExcludedUtc, fromIncludedUtc, signal.Granularity));
                                    for (int i = 0; i < valueIteration; i++)
                                    {
                                        if (quality == Quality.None)
                                            value = 0;
                                        else
                                            value = value + substractValue / Convert.ToDecimal(substractTime);
                                        datumsFirst.Add(new Datum<T>()
                                        {
                                            Quality = quality,
                                            Value = (T)(value).Adapt(value.GetType(), typeof(decimal)),
                                            Timestamp = fromIncludedUtc
                                        });
                                        fromIncludedUtc = AddingTimespanToDataTime(fromIncludedUtc, signal.Granularity);
                                    }
                                    break;
                                }
                                else
                                {
                                    for (int i = 0; i < Convert.ToInt16(substractTime); i++)
                                    {
                                        
                                        if (quality == Quality.None)
                                            value = 0;
                                        else
                                            value = value + substractValue / Convert.ToDecimal(substractTime);
                                        datumsFirst.Add(new Datum<T>()
                                        {
                                            Quality = quality,
                                            Value = (T)(value).Adapt(value.GetType(), typeof(decimal)),
                                            Timestamp = fromIncludedUtc
                                        });
                                        fromIncludedUtc = AddingTimespanToDataTime(fromIncludedUtc, signal.Granularity);
                                    }
                                }
                                break;
                            }
                        case DataType.Double:
                            {
                                var value = Convert.ToDouble(dataOlder.ElementAt(0).Value);
                                Quality quality = ChoiseQuality(dataNewer.ElementAt(0).Quality, dataOlder.ElementAt(0).Quality);
                                var substractTime = SetTotalSteps(dataNewer.ElementAt(0).Timestamp, dataOlder.ElementAt(0).Timestamp, signal.Granularity);
                                var substractValue = Convert.ToDouble(dataNewer.ElementAt(0).Value) - Convert.ToDouble(dataOlder.ElementAt(0).Value);
                                if (Convert.ToInt16(substractTime) > SetTotalSteps(toExcludedUtc, fromIncludedUtc, signal.Granularity))
                                {
                                    int valueIteration = Convert.ToInt16(SetTotalSteps(toExcludedUtc, fromIncludedUtc, signal.Granularity));
                                    for (int i = 0; i < valueIteration; i++)
                                    {
                                        if (quality == Quality.None)
                                            value = 0;
                                        else
                                            value = value + substractValue / Convert.ToDouble(substractTime);
                                        datumsFirst.Add(new Datum<T>()
                                        {
                                            Quality = quality,
                                            Value = (T)(value).Adapt(value.GetType(), typeof(double)),
                                            Timestamp = fromIncludedUtc
                                        });
                                        fromIncludedUtc = AddingTimespanToDataTime(fromIncludedUtc, signal.Granularity);
                                    }
                                    break;
                                }
                                else
                                {
                                    for (int i = 0; i < Convert.ToInt16(substractTime); i++)
                                    {
                                        if (quality == Quality.None)
                                            value = 0;
                                        else
                                            value = value + substractValue / Convert.ToDouble(substractTime);
                                        datumsFirst.Add(new Datum<T>()
                                        {
                                            Quality = quality,
                                            Value = (T)(value).Adapt(value.GetType(), typeof(double)),
                                            Timestamp = fromIncludedUtc
                                        });
                                        fromIncludedUtc = AddingTimespanToDataTime(fromIncludedUtc, signal.Granularity);
                                    }
                                }
                                break;
                            }
                        case DataType.Integer:
                            {
                                var value = Convert.ToInt32(dataOlder.ElementAt(0).Value);
                                var substractTime = SetTotalSteps(dataNewer.ElementAt(0).Timestamp, dataOlder.ElementAt(0).Timestamp, signal.Granularity);
                                var substractValue = Convert.ToInt16(dataNewer.ElementAt(0).Value) - Convert.ToInt16(dataOlder.ElementAt(0).Value);
                                if (Convert.ToInt16(substractTime) > SetTotalSteps(toExcludedUtc, fromIncludedUtc, signal.Granularity))
                                {
                                    int valueIteration = Convert.ToInt16(SetTotalSteps(toExcludedUtc, fromIncludedUtc, signal.Granularity));
                                    for (int i = 0; i < valueIteration; i++)
                                    {
                                        Quality quality = ChoiseQuality(dataNewer.ElementAt(0).Quality, dataOlder.ElementAt(0).Quality);

                                        if (quality == Quality.None)
                                            value = 0;
                                        else
                                            value = value + substractValue / Convert.ToInt16(substractTime);
                                        datumsFirst.Add(new Datum<T>()
                                        {
                                            Quality = quality,
                                            Value = (T)(value).Adapt(value.GetType(), typeof(int)),
                                            Timestamp = fromIncludedUtc
                                        });
                                        fromIncludedUtc = AddingTimespanToDataTime(fromIncludedUtc, signal.Granularity);
                                    }
                                    datumsFirst.Add(dataNewer.ElementAt(0));
                                    fromIncludedUtc = AddingTimespanToDataTime(fromIncludedUtc, signal.Granularity);
                                    break;
                                }
                                else
                                    for (int i = 0; i < Convert.ToInt16(substractTime)-1; i++)
                                    {
                                        Quality quality = ChoiseQuality(dataNewer.ElementAt(0).Quality, dataOlder.ElementAt(0).Quality);

                                        if (quality == Quality.None)
                                            value = 0;
                                        else
                                            value = value + substractValue / Convert.ToInt16(substractTime);
                                        datumsFirst.Add(new Datum<T>()
                                        {
                                            Quality = quality,
                                            Value = (T)(value).Adapt(value.GetType(), typeof(int)),
                                            Timestamp = fromIncludedUtc
                                        });
                                        fromIncludedUtc = AddingTimespanToDataTime(fromIncludedUtc, signal.Granularity);
                                    }
                                datumsFirst.Add(dataNewer.ElementAt(0));
                                fromIncludedUtc = AddingTimespanToDataTime(fromIncludedUtc, signal.Granularity);
                                break;
                            }
                        case DataType.String:
                            {
                                break;
                            }
                    }
                }
            }
            RemoveElementsFromDatumAndAddingNewElements<T>(datums, datumsFirst);

        }

        private void SetDatumForShadowMissingValuePolicy<T>(MissingValuePolicyBase mvp, Datum<T>[] result, ref List<Datum<T>> datums, Signal signal, DateTime date, DateTime fromIncludedUtc, DateTime toExcludedUtc)
        {
            while (date < toExcludedUtc)
            {
                var shadowSignal = mvp.GetShadowSignal();
                var shadowDatum = signalsDataRepository.GetData<T>(shadowSignal, fromIncludedUtc, toExcludedUtc);

                var actualData = result.FirstOrDefault(d => d.Timestamp == date);
                var shadowData = shadowDatum.FirstOrDefault(d => d.Timestamp == date);

                if (actualData != null)
                {
                    datums.Add(actualData);
                }
                else if (shadowData != null)
                {
                    datums.Add(shadowData);
                }
                else
                {
                    datums.Add(new Datum<T>() { Quality = Quality.None, Timestamp = date, Value = default(T) });
                }

                increaseDate(ref date, signal.Granularity);
            }
        }


        private DateTime AddingTimespanToDataTime(DateTime addedData, Granularity granularitySignal)
        {
            var checkGranularity = new Dictionary<Granularity, Action>()
            {
                {Granularity.Day, ()=> addedData=addedData.AddDays(1) },
                {Granularity.Hour, ()=> addedData=addedData.AddHours(1) },
                {Granularity.Minute, ()=> addedData=addedData.AddMinutes(1) },
                {Granularity.Month, ()=> addedData=addedData.AddMonths(1) },
                {Granularity.Second, ()=> addedData=addedData.AddSeconds(1) },
                {Granularity.Week, ()=> addedData=addedData.AddDays(7) },
                {Granularity.Year, ()=> addedData=addedData.AddYears(1) }
            };
            checkGranularity[granularitySignal].Invoke();
            return addedData;
        }

        private double SetTotalSteps(DateTime dataNever, DateTime dataOlder, Granularity granularitySignal)
        {
            double returnValue = 0;
            var calulateSubstract = new Dictionary<Granularity, Action>()
            {
                {Granularity.Day, ()=> returnValue=(dataNever - dataOlder).TotalDays },
                {Granularity.Hour, ()=> returnValue=(dataNever - dataOlder).TotalHours },
                {Granularity.Minute, ()=> returnValue=(dataNever - dataOlder).TotalMinutes },
                {Granularity.Month, ()=> returnValue=((dataNever.Year - dataOlder.Year)*12)+dataNever.Month - dataOlder.Month },
                {Granularity.Second, ()=> returnValue=(dataNever - dataOlder).TotalSeconds },
                {Granularity.Week, ()=> returnValue=(dataNever - dataOlder).TotalDays/7 },
                {Granularity.Year, ()=> returnValue=(dataNever.Year - dataOlder.Year)}
            };
            calulateSubstract[granularitySignal].Invoke();

            return returnValue;
        }

        private void RemoveElementsFromDatumAndAddingNewElements<T>(List<Domain.Datum<T>> datums, List<Domain.Datum<T>> datumsFirst)
        {
            for (int i = 0; i < datums.Count; i++)
                datums.RemoveAt(i);
            for (int i = 0; i < datumsFirst.Count; i++)
                datums.Add(datumsFirst.ElementAt(i));
        }


        private Quality ChoiseQuality(Quality dataNeverQuality, Quality dataOlderQuality)
        {
            int dataNeverQualityValue = SetValueToQuality(dataNeverQuality);
            int dataOlderQualityValue = SetValueToQuality(dataOlderQuality);

            if (dataOlderQualityValue < dataNeverQualityValue)
                return dataOlderQuality;
            else
                return dataNeverQuality;
        }

        private int SetValueToQuality(Quality dataQuality)
        {
            int value = 0;
            var choiseQuality = new Dictionary<Quality, Action>()
            {
                {Quality.None,()=>value=0 },
                {Quality.Bad,()=>value=1 },
                {Quality.Poor,()=>value=2 },
                {Quality.Fair,()=>value=3 },
                {Quality.Good,()=>value=4 },

            };
            choiseQuality[dataQuality].Invoke();
            return value;
        }




        private T SetOutValue<T>(dynamic valueDifference, ref T step, int totalSteps, T value)
        {
            if (valueDifference > 0)
            {
                step += valueDifference / totalSteps;

                return (dynamic)value + step;
            }
            else if (valueDifference < 0)
            {
                step += -(valueDifference / totalSteps);
                return (dynamic)value + step;
            }
            return default(T);
        }

        public void SetData<T>(IEnumerable<Datum<T>> data, Signal signal)
        {
            if (data == null)
                throw new ArgumentNullException("Attempted to set null data for a signal");



            SetSignalForDatumCollection(data, signal);

            foreach(var item in data)
            {
                if (!VeryfiTimeStamp(signal.Granularity, item.Timestamp))
                    throw new BadDateFormatForSignalException();
            }

            signalsDataRepository.SetData(data);
        }

        private void SetSignalForDatumCollection<T>(IEnumerable<Domain.Datum<T>> data, Signal signal)
        {
            if (!data.Any() || signal == null)
                return;

            foreach (var datum in data)
            {
                datum.Signal = signal;
            }
        }

        private bool TimestampCorrectCheckerForYear<T>(IEnumerable<Datum<T>> existingDatum, Domain.Signal existingSignal)
        {
            var TimestampMonth = existingDatum.ToList().ElementAt(0).Timestamp.Month;
            var TimestampDay = existingDatum.ToList().ElementAt(0).Timestamp.Day;
            var TimestampHour = existingDatum.ToList().ElementAt(0).Timestamp.Hour;
            var TimestampMinute = existingDatum.ToList().ElementAt(0).Timestamp.Minute;
            var TimestampSecond = existingDatum.ToList().ElementAt(0).Timestamp.Second;

            if (existingSignal.Granularity == Granularity.Year && TimestampMonth != 1 && TimestampDay >= 1 && TimestampHour >= 0
                && TimestampMinute >= 0 && TimestampSecond >= 0 || TimestampMonth == 1 && TimestampDay != 1 && TimestampHour >= 0
                && TimestampMinute >= 0 && TimestampSecond >= 0 || TimestampMonth == 1 && TimestampDay == 1 && TimestampHour != 0
                && TimestampMinute >= 0 && TimestampSecond >= 0 || TimestampMonth == 1 && TimestampDay == 1 && TimestampHour == 0
                && TimestampMinute != 0 && TimestampSecond >= 0 || TimestampMonth == 1 && TimestampDay == 1 && TimestampHour == 0
                && TimestampMinute == 0 && TimestampSecond != 0)
            {
                return true;
            }
            else return false;
        }

        private bool TimestampCorrectCheckerForMonth<T>(IEnumerable<Datum<T>> existingDatum, Domain.Signal existingSignal)
        {
            var TimestampDay = existingDatum.ToList().ElementAt(0).Timestamp.Day;
            var TimestampHour = existingDatum.ToList().ElementAt(0).Timestamp.Hour;
            var TimestampMinute = existingDatum.ToList().ElementAt(0).Timestamp.Minute;
            var TimestampSecond = existingDatum.ToList().ElementAt(0).Timestamp.Second;

            if (existingSignal.Granularity == Granularity.Month &&  TimestampDay != 1 && TimestampHour >= 0
                && TimestampMinute >= 0 && TimestampSecond >= 0 || TimestampDay == 1 && TimestampHour != 0
                && TimestampMinute >= 0 && TimestampSecond >= 0 || TimestampDay == 1 && TimestampHour == 0
                && TimestampMinute != 0 && TimestampSecond >= 0 || TimestampDay == 1 && TimestampHour == 0
                && TimestampMinute == 0 && TimestampSecond != 0)
            {
                return true;
            }
            else return false;
        }

        private bool TimestampCorrectCheckerForWeek<T>(IEnumerable<Datum<T>> existingDatum, Domain.Signal existingSignal)
        {
            var TimestampDayOfTheWeek = existingDatum.ToList().ElementAt(0).Timestamp.DayOfWeek;
            var TimestampHour = existingDatum.ToList().ElementAt(0).Timestamp.Hour;
            var TimestampMinute = existingDatum.ToList().ElementAt(0).Timestamp.Minute;
            var TimestampSecond = existingDatum.ToList().ElementAt(0).Timestamp.Second;

            if (existingSignal.Granularity == Granularity.Week && TimestampDayOfTheWeek != System.DayOfWeek.Monday && TimestampHour >= 0
                && TimestampMinute >= 0 && TimestampSecond >= 0 || TimestampDayOfTheWeek == System.DayOfWeek.Monday && TimestampHour != 0
                && TimestampMinute >= 0 && TimestampSecond >= 0 || TimestampDayOfTheWeek != System.DayOfWeek.Monday && TimestampHour == 0
                && TimestampMinute != 0 && TimestampSecond >= 0 || TimestampDayOfTheWeek != System.DayOfWeek.Monday && TimestampHour == 0
                && TimestampMinute == 0 && TimestampSecond != 0)
            {
                return true;
            }
            else return false;
        }

        private bool TimestampCorrectCheckerForDay<T>(IEnumerable<Datum<T>> existingDatum, Domain.Signal existingSignal)
        {
            var TimestampHour = existingDatum.ToList().ElementAt(0).Timestamp.Hour;
            var TimestampMinute = existingDatum.ToList().ElementAt(0).Timestamp.Minute;
            var TimestampSecond = existingDatum.ToList().ElementAt(0).Timestamp.Second;

            if (existingSignal.Granularity == Granularity.Day &&  TimestampHour != 0 
                && TimestampMinute >= 0 && TimestampSecond >= 0 ||  TimestampHour == 0
                && TimestampMinute != 0 && TimestampSecond >= 0 ||  TimestampHour == 0
                && TimestampMinute == 0 && TimestampSecond != 0)
            {
                return true;
            }
            else return false;
        }

        private bool TimestampCorrectCheckerForHour<T>(IEnumerable<Datum<T>> existingDatum, Domain.Signal existingSignal)
        {
            var TimestampMinute = existingDatum.ToList().ElementAt(0).Timestamp.Minute;
            var TimestampSecond = existingDatum.ToList().ElementAt(0).Timestamp.Second;

            if (existingSignal.Granularity == Granularity.Hour && TimestampMinute != 0 && TimestampSecond >= 0 
                || TimestampMinute == 0 && TimestampSecond != 0)
            {
                return true;
            }
            else return false;
        }

        private bool TimestampCorrectCheckerForMinute<T>(IEnumerable<Datum<T>> existingDatum, Domain.Signal existingSignal)
        {
            var TimestampSecond = existingDatum.ToList().ElementAt(0).Timestamp.Second;
            var TimestampMilisecond = existingDatum.ToList().ElementAt(0).Timestamp.Millisecond;

            if (existingSignal.Granularity == Granularity.Minute && TimestampSecond != 0 && TimestampMilisecond >= 0
                || TimestampSecond == 0 && TimestampMilisecond != 0)
            {
                return true;
            }
            else return false;
        }

        private bool TimestampCorrectCheckerForSecond<T>(IEnumerable<Datum<T>> existingDatum, Domain.Signal existingSignal)
        {
            var TimestampMilisecond = existingDatum.ToList().ElementAt(0).Timestamp.Millisecond;

            if (existingSignal.Granularity == Granularity.Second && TimestampMilisecond != 0)
            {
                return true;
            }
            else return false;
        }

        private void increaseDate(ref DateTime date, Granularity granularity)
        {
            switch (granularity)
            {
                case Granularity.Second:
                    date = date.AddSeconds(1);
                    return;

                case Granularity.Minute:
                    date = date.AddMinutes(1);
                    return;

                case Granularity.Hour:
                    date = date.AddHours(1);
                    return;

                case Granularity.Day:
                    date = date.AddDays(1);
                    return;

                case Granularity.Week:
                    date = date.AddDays(7);
                    return;

                case Granularity.Month:
                    date = date.AddMonths(1);
                    return;

                case Granularity.Year:
                    date = date.AddYears(1);
                    return;
            }
            throw new NotSupportedException("Granularity: " + granularity.ToString() + " is not supported");
        }

        public PathEntry GetPathEntry(Path path)
        {
            var signals = signalsRepository.GetAllWithPathPrefix(path);

            List<Signal> signalsToAdd = new List<Signal>();
            List<Path> subPaths = new List<Path>();

            foreach (var signal in signals)
            {
                Signal signalToAdd = new Signal();
                if (signal.Path.GetPrefix(path.Length).ToString() == path.ToString() &&
                    signal.Path.ToString().LastIndexOf('/') == (path.ToString().Length))
                {
                    signalToAdd = signal;
                    signalsToAdd.Add(signalToAdd);
                }

                Path pathToAdd = signal.Path.GetPrefix(path.Length + 1);
                if (!subPaths.Contains(pathToAdd) && pathToAdd.ToString() != signal.Path.ToString())
                {
                    subPaths.Add(pathToAdd);
                }
            }

            return new PathEntry(signalsToAdd, subPaths);
        }

        private bool VeryfiTimeStamp(Granularity granularity, DateTime timeStamp)
        {
            switch (granularity)
            {
                case Granularity.Day:
                    {
                        if (timeStamp.Hour == 0 && timeStamp.Minute == 0
                            && timeStamp.Second == 0 && timeStamp.Millisecond == 0)
                            return true;
                        break;
                    }
                case Granularity.Hour:
                    {
                        if (timeStamp.Minute == 0 && timeStamp.Second == 0
                            && timeStamp.Millisecond == 0)
                            return true;
                        break;
                    }
                case Granularity.Minute:
                    {
                        if (timeStamp.Second == 0 && timeStamp.Millisecond == 0)
                            return true;
                        break;
                    }
                case Granularity.Month:
                    {
                        if (timeStamp.Day == 1 && timeStamp.Hour == 0 && timeStamp.Minute == 0
                            && timeStamp.Second == 0 && timeStamp.Millisecond == 0)
                            return true;
                        break;
                    }
                case Granularity.Second:
                    {
                        if (timeStamp.Millisecond == 0)
                            return true;
                        break;
                    }
                case Granularity.Week:
                    {
                        if (timeStamp.DayOfWeek == DayOfWeek.Monday && timeStamp.Hour == 0 && timeStamp.Minute == 0
                             && timeStamp.Second == 0 && timeStamp.Millisecond == 0)
                            return true;
                        break;
                    }
                case Granularity.Year:
                    {
                        if (timeStamp.DayOfYear == 1 && timeStamp.Hour == 0 && timeStamp.Minute == 0
                             && timeStamp.Second == 0 && timeStamp.Millisecond == 0)
                            return true;
                        break;
                    }
                default:
                    {
                        throw new NotImplementedException();
                    }
            }

            return false;
        }

        public void Delete(int signalId)
        {
            var signal = GetById(signalId);
            if (signal == null)
                throw new NoSuchSignalException("Signal not found");
            switch (signal.DataType)
            {
                case DataType.Double:
                    signalsDataRepository.DeleteData<double>(signal);
                    break;
                case DataType.Boolean:
                    signalsDataRepository.DeleteData<bool>(signal);
                    break;
                case DataType.Decimal:
                    signalsDataRepository.DeleteData<decimal>(signal);
                    break;
                case DataType.String:
                    signalsDataRepository.DeleteData<string>(signal);
                    break;
                case DataType.Integer:
                    signalsDataRepository.DeleteData<int>(signal);
                    break;
            }

            missingValuePolicyRepository.Set(signal, null);
            signalsRepository.Delete(signal);
        }

        public IEnumerable<Datum<T>> GetCoarseData<T>(Signal signal, Granularity granularity, DateTime fromIncludedUtc, DateTime toExcludedUtc)
        {
            
            return null;
        }

        
    }
}
