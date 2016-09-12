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
            {
                throw new IdNotNullException();
            }

            var result = this.signalsRepository.Add(newSignal);
            var dataTypeSwitch = new Dictionary<DataType, Action>
            {
                { DataType.Boolean,()=>missingValuePolicyRepository.Set(result, new NoneQualityMissingValuePolicy<bool>()) },
                { DataType.Decimal, () =>missingValuePolicyRepository.Set(result, new NoneQualityMissingValuePolicy<decimal>())},
                { DataType.Double,() =>missingValuePolicyRepository.Set(result, new NoneQualityMissingValuePolicy<double>())},
                { DataType.Integer,()=>missingValuePolicyRepository.Set(result, new NoneQualityMissingValuePolicy<int>())},
                { DataType.String, ()=>missingValuePolicyRepository.Set(result, new NoneQualityMissingValuePolicy<string>())}
            };
            dataTypeSwitch[newSignal.DataType].Invoke();
            return result;
        }

        public Signal GetById(int signalId)
        {
            return this.signalsRepository.Get(signalId);
        }

        public Signal Get(Path newPath)
        {
            if (newPath.Components.Equals(""))
            {
                throw new PathIsEmptyException();
            }

            return this.signalsRepository.Get(newPath);
        }

        public void SetMissingValuePolicy(Domain.Signal signal, MissingValuePolicyBase mvpDomain)
        {
            shadowPolicySignalCase(signal, mvpDomain);

            this.missingValuePolicyRepository.Set(signal, mvpDomain);
        }

        public MissingValuePolicyBase GetMissingValuePolicy(Signal signal)
        {
            var missingValuePolicy = this.missingValuePolicyRepository.Get(signal);
            if (missingValuePolicy == null)
                return null;
            return TypeAdapter.Adapt(missingValuePolicy, missingValuePolicy.GetType(), missingValuePolicy.GetType().BaseType)
                    as MissingValuePolicy.MissingValuePolicyBase;

        }

        public IEnumerable<Datum<T>> GetData<T>(Signal signal, DateTime fromIncludedUtc, DateTime toExcludedUtc)
        {
            if (!dateIsValid(signal.Granularity, fromIncludedUtc))
                throw new ArgumentException("Date: "+fromIncludedUtc.ToString()+ "is invalid");
            
            var datum = new Datum<T>();
            var mvp = GetMissingValuePolicy(signal);

            return FillDatum<T>(fromIncludedUtc, toExcludedUtc, signal, datum, mvp);
        }

        private IEnumerable<Datum<T>> FillDatum<T>(DateTime fromIncludedUtc, DateTime toExcludedUtc, Signal signal, Datum<T> datum, MissingValuePolicyBase mvp)
        {
            var gettingList = this.signalsDataRepository?.GetData<T>(signal, fromIncludedUtc, toExcludedUtc)?.ToArray();

            Datum<T> xx = null;
            var returnList = new List<Datum<T>>();

            T step = default(T);
            Quality quality = 0;

            if (fromIncludedUtc == toExcludedUtc)
            {
                if (gettingList.Count() > 0)
                    return gettingList;

                if (mvp.GetType() == typeof(SpecificValueMissingValuePolicy<T>))
                {
                    datum = SetDatumForSpecificOrderMissingValuePolicy<T>((MissingValuePolicy.SpecificValueMissingValuePolicy<T>)mvp);
                }
                else if (mvp.GetType() == typeof(ZeroOrderMissingValuePolicy<T>))
                {
                    datum = SetDatumForZeroOrderMissingValuePolicy<T>(signal, fromIncludedUtc);
                }
                else if (mvp.GetType() == typeof(FirstOrderMissingValuePolicy<T>))
                {
                    datum = SetDatumForFirstOrderMissingValuePolicy(signal, datum, fromIncludedUtc, quality, ref step);
                }
                else if (mvp.GetType() == typeof(ShadowMissingValuePolicy<T>))
                {
                    datum = SetDatumForShadowMissingValuePolicy<T>((MissingValuePolicy.ShadowMissingValuePolicy<T>)mvp, signal, fromIncludedUtc);
                }
                else if (mvp.GetType() == typeof(NoneQualityMissingValuePolicy<T>))
                {
                    datum = new Datum<T>() { Quality = Quality.None, Signal = signal, Timestamp = fromIncludedUtc, Value = default(T) };
                }
                returnList.Add(new Datum<T>() { Quality = datum.Quality, Timestamp = fromIncludedUtc, Value = datum.Value, Signal = signal });
            }

            while (fromIncludedUtc < toExcludedUtc)
            {
                if (gettingList != null)
                    xx = gettingList.FirstOrDefault(x => x.Timestamp == fromIncludedUtc);
                if (xx == null)
                {
                    if (signalsDataRepository.GetDataOlderThan<T>(signal, fromIncludedUtc, 1) != null && mvp.GetType() == typeof(SpecificValueMissingValuePolicy<T>))
                    {
                        datum = SetDatumForSpecificOrderMissingValuePolicy<T>((MissingValuePolicy.SpecificValueMissingValuePolicy<T>)mvp);
                    }
                    else if (mvp.GetType() == typeof(ZeroOrderMissingValuePolicy<T>))
                    {
                        datum = SetDatumForZeroOrderMissingValuePolicy<T>(signal, fromIncludedUtc);
                    }
                    else if (mvp.GetType() == typeof(FirstOrderMissingValuePolicy<T>))
                    {
                        datum = SetDatumForFirstOrderMissingValuePolicy(signal, datum, fromIncludedUtc, quality, ref step);
                    }
                    else if (mvp.GetType() == typeof(ShadowMissingValuePolicy<T>))
                    {
                        datum = SetDatumForShadowMissingValuePolicy<T>((MissingValuePolicy.ShadowMissingValuePolicy<T>)mvp, signal, fromIncludedUtc);
                    }
                    returnList.Add(new Datum<T>() { Quality = datum.Quality, Timestamp = fromIncludedUtc, Value = datum.Value, Signal = signal });
                }
                else
                {
                    returnList.Add(xx);
                    step = default(T);
                }
                fromIncludedUtc = AddToDateTime(fromIncludedUtc, signal);
            }

            return returnList.ToArray();
        }

        private Datum<T> SetDatumForSpecificOrderMissingValuePolicy<T>(MissingValuePolicy.SpecificValueMissingValuePolicy<T> mvp)
        {
            return new Datum<T>()
            {
                Quality = mvp.Quality,
                Value = mvp.Value
            };
        }

        private Datum<T> SetDatumForZeroOrderMissingValuePolicy<T>(Signal signal, DateTime fromIncludedUtc)
        {
            var lastData = signalsDataRepository.GetDataOlderThan<T>(signal, fromIncludedUtc, 1);
            if (lastData.Count() == 0)
            {
                return new Datum<T>()
                {
                    Quality = Quality.None,
                    Signal = signal,
                    Timestamp = SubstractDateTime(signal.Granularity, fromIncludedUtc),
                    Value = default(T),
                };
            }
            else
            {
                return new Datum<T>()
                {
                    Quality = lastData.ElementAt(0).Quality,
                    Signal = signal,
                    Timestamp = lastData.ElementAt(0).Timestamp,
                    Value = lastData.ElementAt(0).Value,
                };
            }
        }

        private Datum<T> SetDatumForFirstOrderMissingValuePolicy<T>(Signal signal, Datum<T> datum, DateTime fromIncludedUtc, Quality quality, ref T step)
        {
            if (signal.DataType == DataType.Boolean || signal.DataType == DataType.String)
                throw new ArgumentException("Boolean and String types are not supported.");



            var olderData = signalsDataRepository.GetDataOlderThan<T>(signal, fromIncludedUtc, 1);
            var newerData = signalsDataRepository.GetDataNewerThan<T>(signal, fromIncludedUtc, 1);

            Datum<T> olderDatum = null;
            Datum<T> newerDatum = null;
            
            var defaultDatum = new Datum<T>() {
                Quality = Quality.None,
                Value = default(T),
                Timestamp = fromIncludedUtc
            };

            if (olderData.Count() > 0)
                olderDatum = olderData.ElementAt(0);
            if (newerData.Count() > 0)
                newerDatum = newerData.ElementAt(0);

            else if (olderDatum.Quality == Quality.None || newerData.ElementAt(0).Quality == Quality.None)
                return defaultDatum;

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

            return datum;
        }

        private Datum<T> SetDatumForShadowMissingValuePolicy<T>(MissingValuePolicy.ShadowMissingValuePolicy<T> mvp, Signal signal, DateTime timestamp)
        {
            var signalData = signalsDataRepository.GetData<T>(signal, timestamp, timestamp);
            if (signalData.Count() == 0)
            {
                var shadowSignalData = signalsDataRepository.GetData<T>(mvp.ShadowSignal, timestamp, timestamp);
                if (shadowSignalData.Count() == 0)
                {
                    return new Datum<T>()
                    {
                        Quality = Quality.None,
                        Value = default(T),
                    };
                }
                else
                {
                    return shadowSignalData.ElementAt(0);
                }
            }
            else
            {
                return signalData.ElementAt(0);
            }
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
                    var monthTimeDifference = (newerData.Timestamp.Year - olderData.Timestamp.Year) * 12 +
                        (newerData.Timestamp.Month - olderData.Timestamp.Month);
                    return monthTimeDifference;
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
        
        public void SetData<T>(Signal signal, IEnumerable<Datum<T>> datum)
        {
            var datumWithSignal = new Datum<T>[datum.Count()];
            int i = 0;
            foreach (var d in datum)
            {
                if (!dateIsValid(signal.Granularity, d.Timestamp))
                    throw new ArgumentException("Date: " + d.Timestamp.ToString() + "is invalid");

                datumWithSignal[i++] = new Datum<T>()
                {
                    Quality = d.Quality,
                    Timestamp = d.Timestamp,
                    Value = d.Value,
                    Signal = signal
                };
            }
            this.signalsDataRepository.SetData<T>(datumWithSignal);
        }

        public PathEntry GetPathEntry(Path pathDomain)
        {
            List<Domain.Signal> signalList = signalsRepository.GetAllWithPathPrefix(pathDomain).ToList();

            var directPathSignals = signalList
                .Where(s => s.Path.Length == pathDomain.Length + 1)
                .ToArray();

            var subPaths = signalList
                .Where(s => s.Path.Length > pathDomain.Length + 1)
                .Select(s => s.Path.GetPrefix(pathDomain.Length + 1))
                .Distinct()
                .ToArray();

            return new PathEntry(directPathSignals, subPaths);
        }
        
        public DateTime AddToDateTime(DateTime date, Signal signal)
        {
            var addTimeSpan = new Dictionary<Granularity, Action>
                {
                    {Granularity.Day,() => date = date.AddDays(1)},
                    {Granularity.Hour,() => date = date.AddHours(1)},
                    {Granularity.Minute,() => date = date.AddMinutes(1)},
                    {Granularity.Month,() => date = date.AddMonths(1)},
                    {Granularity.Second,() => date = date.AddSeconds(1)},
                    {Granularity.Week,() => date = date.AddDays(7)},
                    {Granularity.Year,() => date = date.AddYears(1)}
                };
            addTimeSpan[signal.Granularity].Invoke();
            return date;
        }

        private bool dateIsValid (Granularity granularity, DateTime date)
        {
            DateTime correctDate = new DateTime();
            switch (granularity)
            {
                case (Granularity.Year):
                    correctDate = new DateTime(date.Year, 1, 1);
                    break;

                case (Granularity.Month):
                    correctDate = new DateTime(date.Year, date.Month, 1);
                    break;

                case (Granularity.Week):
                    if (date.DayOfWeek != DayOfWeek.Monday)
                        return false;
                    correctDate = new DateTime(date.Year, date.Month, date.Day);
                    break;

                case (Granularity.Day):
                    correctDate = new DateTime(date.Year, date.Month, date.Day);
                    break;

                case (Granularity.Hour):
                    correctDate = new DateTime(date.Year, date.Month, date.Day, date.Hour, 0, 0);
                    break;

                case (Granularity.Minute):
                    correctDate = new DateTime(date.Year, date.Month, date.Day, date.Hour, date.Minute, 0);
                    break;

                case (Granularity.Second):
                    correctDate = new DateTime(date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second);
                    break;

                default:
                    throw new NotSupportedException("Granularity: " + granularity.ToString() + " is not supported");
            }
            return date.Ticks == correctDate.Ticks;
        }

        public void Delete(int signalId)
        {
            var signal = GetById(signalId);

            if(signal == null)
            {
                throw new Domain.Exceptions.SignalDoesNotExist();
            }

            missingValuePolicyRepository.Set(signal, null);

            var typename = signal.DataType.GetNativeType().Name;

            switch (typename)
            {
                case "Int32":
                    DeleteSignalData<int>(signal);
                    break;
                case "Double":
                    DeleteSignalData<double>(signal);
                    break;
                case "Decimal":
                    DeleteSignalData<decimal>(signal);
                    break;
                case "Boolean":
                    DeleteSignalData<bool>(signal);
                    break;
                case "String":
                    DeleteSignalData<string>(signal);
                    break;
            }

            signalsRepository.Delete(signal);
        }

        private void DeleteSignalData<T>(Signal signal)
        {
            signalsDataRepository.DeleteData<T>(signal);
        }

        private DateTime SubstractDateTime(Granularity granularity, DateTime dateTime)
        {
            switch(granularity)
            {
                case Granularity.Second:
                    return dateTime.AddSeconds(-1);

                case Granularity.Minute:
                    return dateTime.AddMinutes(-1);

                case Granularity.Hour:
                    return dateTime.AddHours(-1);

                case Granularity.Day:
                    return dateTime.AddDays(-1);

                case Granularity.Week:
                    return dateTime.AddDays(-7);

                case Granularity.Month:
                    return dateTime.AddMonths(-1);

                case Granularity.Year:
                    return dateTime.AddYears(-1);
            }
            return new DateTime();
        }

        private void shadowPolicySignalCase(Signal signal, MissingValuePolicyBase mvpDomain)
        {
            if (mvpDomain.GetType() == typeof(ShadowMissingValuePolicy<bool>))
            {

                var shadowMvp = mvpDomain as ShadowMissingValuePolicy<bool>;
                var shadowSignal = shadowMvp.ShadowSignal;

                if (!NotAShadowDependencyCycle(shadowMvp, signal))
                    throw new Exception();


                if (signal.DataType != shadowSignal.DataType ||
                signal.Granularity != shadowSignal.Granularity)
                    throw new Exceptions.ShadowSignalDataTypeOrGranularityDoesntMatch();
            }
            else if (mvpDomain.GetType() == typeof(MissingValuePolicy.ShadowMissingValuePolicy<int>))
            {
                var shadowMvp = mvpDomain as ShadowMissingValuePolicy<int>;
                var shadowSignal = shadowMvp.ShadowSignal;

                if (!NotAShadowDependencyCycle(shadowMvp, signal))
                    throw new Exception();


                if (signal.DataType != shadowSignal.DataType ||
                signal.Granularity != shadowSignal.Granularity)
                    throw new Exceptions.ShadowSignalDataTypeOrGranularityDoesntMatch();
            }
            else if (mvpDomain.GetType() == typeof(MissingValuePolicy.ShadowMissingValuePolicy<double>))
            {
                var shadowMvp = mvpDomain as ShadowMissingValuePolicy<double>;
                var shadowSignal = shadowMvp.ShadowSignal;

                if (!NotAShadowDependencyCycle(shadowMvp, signal))
                    throw new Exception();


                if (signal.DataType != shadowSignal.DataType ||
                signal.Granularity != shadowSignal.Granularity)
                    throw new Exceptions.ShadowSignalDataTypeOrGranularityDoesntMatch();
            }
            else if (mvpDomain.GetType() == typeof(MissingValuePolicy.ShadowMissingValuePolicy<decimal>))
            {
                var shadowMvp = mvpDomain as ShadowMissingValuePolicy<decimal>;
                var shadowSignal = shadowMvp.ShadowSignal;

                if (!NotAShadowDependencyCycle(shadowMvp, signal))
                    throw new Exception();

                if (signal.DataType != shadowSignal.DataType ||
                signal.Granularity != shadowSignal.Granularity)
                    throw new Exceptions.ShadowSignalDataTypeOrGranularityDoesntMatch();
            }
            else if (mvpDomain.GetType() == typeof(MissingValuePolicy.ShadowMissingValuePolicy<string>))
            {
                var shadowMvp = mvpDomain as ShadowMissingValuePolicy<string>;
                var shadowSignal = shadowMvp.ShadowSignal;

                if (!NotAShadowDependencyCycle(shadowMvp, signal))
                    throw new Exception();

                if (signal.DataType != shadowSignal.DataType ||
                signal.Granularity != shadowSignal.Granularity)
                    throw new Exceptions.ShadowSignalDataTypeOrGranularityDoesntMatch();
            }
        }

        private bool NotAShadowDependencyCycle<T>(ShadowMissingValuePolicy<T> shadowMvp,Signal signal) 
        {
            var shadowSignal = shadowMvp.ShadowSignal;
            var signalMvp = GetMissingValuePolicy(shadowSignal);

            if (shadowSignal.Id == signal.Id)
                return false;

            while (true) 
            {

                if (signalMvp is ShadowMissingValuePolicy<T>) {
                    var shadowSignalMvp = signalMvp as ShadowMissingValuePolicy<T>;
                    shadowSignal = shadowSignalMvp.ShadowSignal;

                    if (shadowSignal.Id == signal.Id) {
                        return false;
                    }
                    signalMvp = GetMissingValuePolicy(shadowSignal);


                } else return true;


            }
        }

    }
}
