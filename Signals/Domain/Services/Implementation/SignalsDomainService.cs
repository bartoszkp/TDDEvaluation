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
            this.missingValuePolicyRepository.Set(signal, policy);
        }

        public IEnumerable<Datum<T>> GetData<T>(Signal signal, DateTime fromIncludedUtc, DateTime toExcludedUtc)
        {
            if (!VeryfiTimeStamp(signal.Granularity, fromIncludedUtc))
                throw new QuerryAboutDateWithIncorrectFormatException();

            var result = signalsDataRepository.GetData<T>(signal, fromIncludedUtc, toExcludedUtc)?.ToArray();

            if (result == null)
                return null;

            if (fromIncludedUtc == toExcludedUtc)
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

            for (int j = result.Length - 1; j > 0; --j)
                for (int i = 0; i < j; ++i)
                    if (result[i].Timestamp > result[i + 1].Timestamp)
                    {
                        var r = result[i];
                        result[i] = result[i + 1];
                        result[i + 1] = r;
                    }

            var mvp = GetMissingValuePolicy(signal.Id.Value);
            if (mvp != null)
            {
                List<Datum<T>> datums = new List<Datum<T>>();
                var date = fromIncludedUtc;
                while (date < fromIncludedUtc)
                    increaseDate(ref date, signal.Granularity);

                if (typeof(SpecificValueMissingValuePolicy<T>) == mvp.GetType() || typeof(NoneQualityMissingValuePolicy<T>) == mvp.GetType() || typeof(ZeroOrderMissingValuePolicy<T>) == mvp.GetType())
                {
                    T value = default(T);
                    Quality quality = default(Quality);

                    var mvpSpec = mvp.Adapt(mvp, mvp.GetType(), mvp.GetType().BaseType)
                            as MissingValuePolicy.SpecificValueMissingValuePolicy<T>;

                    for (int i = 0; date < toExcludedUtc && i < result.Length; increaseDate(ref date, signal.Granularity))
                    {
                        if (result[i].Timestamp == date)
                        {
                            value = result[i].Value;
                            quality = result[i].Quality;
                            datums.Add(result[i++]);
                        }
                        else
                        {
                            if (typeof(NoneQualityMissingValuePolicy<T>) == mvp.GetType())
                                datums.Add(new Datum<T>() { Quality = Quality.None, Timestamp = date, Value = default(T) });
                            else if (typeof(ZeroOrderMissingValuePolicy<T>) == mvp.GetType())
                            {
                                datums.Add(new Datum<T>() { Quality = quality, Timestamp = date, Value = value });
                            }
                            else
                                datums.Add(new Datum<T>() { Quality = mvpSpec.Quality, Timestamp = date, Value = mvpSpec.Value });
                        }
                    }

                    for (; date < toExcludedUtc; increaseDate(ref date, signal.Granularity))
                    {
                        if (typeof(NoneQualityMissingValuePolicy<T>) == mvp.GetType())
                             datums.Add(new Datum<T>() { Quality = Quality.None, Timestamp = date, Value = default(T) });
                        else if (typeof(ZeroOrderMissingValuePolicy<T>) == mvp.GetType())
                        {
                         datums.Add(new Datum<T>() { Quality = quality, Timestamp = date, Value = value });
                        }
                        else
                            datums.Add(new Datum<T>() { Quality = mvpSpec.Quality, Timestamp = date, Value = mvpSpec.Value });
                     }
                }
                else if(mvp.GetType() == typeof(FirstOrderMissingValuePolicy<T>))
                {
                    var datum = new Datum<T>();

                    T step = default(T);
                    Quality quality = default(Quality);
                    
                    while (date < toExcludedUtc)
                    {
                        var actualData = result.FirstOrDefault(x => x.Timestamp == date);
                        if (actualData != null)
                        {
                            datums.Add(actualData);
                            step = default(T);
                        }
                        else
                        {
                            var olderData = signalsDataRepository.GetDataOlderThan<T>(signal, date, 1);
                            var newerData = signalsDataRepository.GetDataNewerThan<T>(signal, date, 1);

                            if (olderData.Count() == 0)
                            {
                                datum = new Datum<T>()
                                {
                                    Quality = Quality.None,
                                    Value = default(T)
                                };
                            }
                            else if (newerData.Count() == 0)
                            {
                                datum = new Datum<T>()
                                {
                                    Quality = Quality.None,
                                    Value = default(T)
                                };
                            }
                            else
                            {
                                T value = olderData.First().Value;
                                var valueDifference = Convert.ChangeType((dynamic)newerData.First().Value - (dynamic)olderData.First().Value, typeof(T));
                                var granularity = signal.Granularity;

                                quality = SetOutQuality(olderData.First(), newerData.First());

                                int totalSteps = SetTotalSteps(granularity, olderData.First(), newerData.First());

                                value = SetOutValue(valueDifference, ref step, totalSteps, value);

                                datum = new Datum<T>()
                                {
                                    Quality = quality,
                                    Value = value
                                };
                            }
                            datums.Add(new Datum<T>() { Quality = datum.Quality, Value = datum.Value, Timestamp = date });
                        }
                        increaseDate(ref date, signal.Granularity);
                    }
                }

                return datums?.ToArray();
            }

            return result;
        }

        private int SetTotalSteps<T>(Granularity granularity, Datum<T> olderData, Datum<T> newerData)
        {
            var timespan = newerData.Timestamp.Subtract(olderData.Timestamp);

            switch (granularity)
            {
                case Granularity.Second:
                    return (int)timespan.TotalSeconds;
                case Granularity.Minute:
                    return (int)timespan.TotalMinutes;
                case Granularity.Hour:
                    return (int)timespan.TotalHours;
                case Granularity.Day:
                    return (int)timespan.TotalDays;
                case Granularity.Week:
                    return (int)timespan.TotalDays / 7;
                case Granularity.Month:
                    return (int)timespan.TotalDays / 30;
                case Granularity.Year:
                    return (int)timespan.TotalDays / 365;
                default:
                    return 0;
            }
        }

        private Quality SetOutQuality<T>(Datum<T> olderData, Datum<T> newerData)
        {
            if (olderData.Quality == newerData.Quality)
                return olderData.Quality;
            else if (olderData.Quality > newerData.Quality)
                return olderData.Quality;
            else
                return newerData.Quality;
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
    }
}
