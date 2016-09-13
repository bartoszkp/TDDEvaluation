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
            if (newSignal.Id != null)
                throw new IdNotNullException();

            var signal = this.signalsRepository.Add(newSignal);

            var typeOfSignal = signal.DataType;

            switch (typeOfSignal)
            {
                case DataType.Boolean:
                    missingValuePolicyRepository.Set(signal, new NoneQualityMissingValuePolicy<bool>());
                    break;
                case DataType.Integer:
                    missingValuePolicyRepository.Set(signal, new NoneQualityMissingValuePolicy<int>());
                    break;
                case DataType.Double:
                    missingValuePolicyRepository.Set(signal, new NoneQualityMissingValuePolicy<double>());
                    break;
                case DataType.Decimal:
                    missingValuePolicyRepository.Set(signal, new NoneQualityMissingValuePolicy<decimal>());
                    break;
                case DataType.String:
                    missingValuePolicyRepository.Set(signal, new NoneQualityMissingValuePolicy<string>());
                    break;
                default:
                    break;
            }

            return signal;
        }

        public Signal GetById(int signalId)
        {
            return this.signalsRepository.Get(signalId);
        }

        public Signal GetByPath(Path path)
        {
            return this.signalsRepository.Get(path);
        }

        public IEnumerable<Datum<T>> GetData<T>(int signalId, DateTime fromIncludedUtc, DateTime toExcludedUtc)
        {
            var signal = GetById(signalId);

            if (!IsTimestampRegular(signal.Granularity, fromIncludedUtc))
                throw new IncorrectTimestampException();

            var result = signalsDataRepository.GetData<T>(signal, fromIncludedUtc, toExcludedUtc);

            var resultArray = result.OrderBy(datum => datum.Timestamp).ToArray();

            var mvp = GetMissingValuePolicy(signalId);

            if (mvp != null)
                resultArray = FillArray(resultArray, signal, fromIncludedUtc, toExcludedUtc, mvp);

            return resultArray;
        }

        public void SetData<T>(int signalId, IEnumerable<Datum<T>> enumerable)
        {
            var signal = GetById(signalId);

            foreach(var datum in enumerable)
            {
                if (!IsTimestampRegular(signal.Granularity, datum.Timestamp))
                    throw new IncorrectTimestampException();
            }

            enumerable = enumerable.Select(datum => { datum.Signal = signal; return datum; });

            signalsDataRepository.SetData(enumerable.ToList());
        }

        public MissingValuePolicyBase GetMissingValuePolicy(int signalId)
        {
            var signal = signalsRepository.Get(signalId);

            var mvp = missingValuePolicyRepository.Get(signal);

            if (mvp == null) return null;

            else return TypeAdapter.Adapt(mvp, mvp.GetType(), mvp.GetType().BaseType)
                as MissingValuePolicy.MissingValuePolicyBase;
        }

        public void SetMissingValuePolicy<T>(int signalId, MissingValuePolicyBase missingValuePolicyBase)
        {
            var signal = signalsRepository.Get(signalId);

            if (missingValuePolicyBase is ShadowMissingValuePolicy<T>)
            {
                CheckShadowDataTypeAndGranularityonToEquality<T>(signal, missingValuePolicyBase);
                CheckShadowDependencyCycle<T>(signal, missingValuePolicyBase);
            }

            missingValuePolicyRepository.Set(signal, missingValuePolicyBase);
        }

        private void CheckShadowDependencyCycle<T>(Signal signal, MissingValuePolicyBase missingValuePolicy)
        {
            Signal shadow = null;
            while(missingValuePolicy is ShadowMissingValuePolicy<T>)
            {
                var shadowMVP = missingValuePolicy as ShadowMissingValuePolicy<T>;
                shadow = shadowMVP.ShadowSignal;
                if (signal.Id == shadow.Id)
                    throw new ShadowMissingValuePolicyCycleException();
                missingValuePolicy = GetMissingValuePolicy(shadow.Id.Value);
            }
        }

        private void CheckShadowDataTypeAndGranularityonToEquality<T>(Signal signal, MissingValuePolicyBase mvp)
        {
            var shadowMVP = mvp as ShadowMissingValuePolicy<T>;
            if (shadowMVP.ShadowSignal.DataType != signal.DataType) throw new ShadowMissingValuePolicyDataTypeException();
            if (shadowMVP.ShadowSignal.Granularity != signal.Granularity) throw new ShadowMissingValuePolicyGranularityException();
        }

        public PathEntry GetPathEntry(Path path)
        {
            var result = signalsRepository.GetAllWithPathPrefix(path);
            var level = path.Length + 1;

            var signals = result.Where(signal => signal.Path.Length == level);
            var subpaths = result.Where(signal => signal.Path.Length > level)
                .Select(signal => signal.Path)
                .Select(p => Path.FromString(Path.JoinComponents(p.Components.Take(level))))
                .Distinct();

            return new PathEntry(signals.ToList(), subpaths.ToList());
        }

        private Datum<T>[] FillArray<T>(Datum<T>[] array, Signal signal, DateTime fromIncludedUtc, DateTime toExcludedUtc, MissingValuePolicyBase policy)
        {
            var empty = array.Length < 1;

            DateTime tmp;
            bool timestampsFollowingExistingDates = false;
            var dateModifier = DateModifier(signal.Granularity);
            var filledArray = new Datum<T>[NumberOfPeriods(fromIncludedUtc, toExcludedUtc, signal.Granularity)];

            tmp = fromIncludedUtc;
            int j = 0;
            for (int i = 0; i < filledArray.Length; i++)
            {
                if (!empty && array[j].Timestamp == tmp)
                {
                    timestampsFollowingExistingDates = true;
                    filledArray[i] = array[j];
                    j = (j + 1) % array.Length;
                }
                else
                    ValueFromMVP(i, ref filledArray, policy, signal, tmp, timestampsFollowingExistingDates);
                tmp = dateModifier(tmp);
            }

            return filledArray;
        }

        private void ValueFromMVP<T>(int current_index, ref Datum<T>[] filledArray, MissingValuePolicyBase policy, 
            Signal signal, DateTime timestamp, bool timestampsFollowingExistingDates)
        {
            if (policy is SpecificValueMissingValuePolicy<T>)
            {
                var specificPolicy = policy as SpecificValueMissingValuePolicy<T>;
                filledArray[current_index] = new Datum<T>
                {
                    Quality = specificPolicy.Quality,
                    Value = specificPolicy.Value,
                    Signal = signal,
                    Timestamp = timestamp
                };
            }
            else if (policy is ZeroOrderMissingValuePolicy<T>)
            {
                if (!timestampsFollowingExistingDates)
                {
                    var olderDatum = signalsDataRepository.GetDataOlderThan<T>(signal, timestamp, 1).SingleOrDefault();

                    if (olderDatum == null) filledArray[current_index] = Datum<T>.CreateNone(signal, timestamp);

                    else
                    {
                        filledArray[current_index] = new Datum<T>
                        {
                            Quality = olderDatum.Quality,
                            Value = olderDatum.Value,
                            Signal = olderDatum.Signal,
                            Timestamp = timestamp
                        };
                    }
                    return;
                }

                var previousDatum = filledArray[current_index - 1];
                filledArray[current_index] = new Datum<T>()
                {
                    Quality = previousDatum.Quality,
                    Signal = previousDatum.Signal,
                    Value = previousDatum.Value,
                    Timestamp = timestamp
                };
            }

            else if (policy is FirstOrderMissingValuePolicy<T>)
            {
                var olderData = signalsDataRepository.GetDataOlderThan<T>(signal, timestamp, 1).SingleOrDefault();
                var newerData = signalsDataRepository.GetDataNewerThan<T>(signal, timestamp, 1).SingleOrDefault();

                if (olderData != null && olderData.Quality != Quality.None && newerData != null && newerData.Quality != Quality.None) // jesli znalazlem wczesniejsza probke
                {
                    var diffNumberOlder_Newer = NumberOfPeriods(olderData.Timestamp, newerData.Timestamp, signal.Granularity);
                    var diffNumberOlder_Actual = NumberOfPeriods(olderData.Timestamp, timestamp, signal.Granularity);
                    var stepValue = ValueStep<T>(signal, olderData.Value, newerData.Value, diffNumberOlder_Newer, diffNumberOlder_Actual);

                    filledArray[current_index] = new Datum<T>()
                    {
                        Quality = (olderData.Quality > newerData.Quality) ? olderData.Quality : newerData.Quality,
                        Signal = olderData.Signal,
                        Value = stepValue,
                        Timestamp = timestamp
                    };
                }

                else filledArray[current_index] = Datum<T>.CreateNone(signal, timestamp);
            }

            else if (policy is ShadowMissingValuePolicy<T>)
            {
                var shadowPolicy = policy as ShadowMissingValuePolicy<T>;

                var data = signalsDataRepository.GetData<T>(shadowPolicy.ShadowSignal, timestamp, timestamp).FirstOrDefault();
                if (data != null)
                {
                    filledArray[current_index] = new Datum<T>
                    {
                        Quality = data.Quality,
                        Value = data.Value,
                        Signal = signal,
                        Timestamp = timestamp
                    };
                }
                else filledArray[current_index] = Datum<T>.CreateNone(signal, timestamp);
            }

            else
                filledArray[current_index] = Datum<T>.CreateNone(signal, timestamp);

        }

        private T ValueStep<T>(Signal signal, dynamic older, dynamic newer, int diffOlder_Newer, int diffOlder_Actual)
        {
            try
            {
                dynamic result = default(T);
                result = older + (newer - older) * diffOlder_Actual / diffOlder_Newer;
                if (typeof(int) == typeof(T)) { return result; } else { return Math.Round(result, 5); }
            }
            catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderException) { throw new ArgumentException(); }
        }

        private Func<DateTime, DateTime> DateModifier(Granularity granularity)
        {
            switch (granularity)
            {
                case Granularity.Second:
                    return (date) => date.AddSeconds(1);
                case Granularity.Minute:
                    return (date) => date.AddMinutes(1);
                case Granularity.Hour:
                    return (date) => date.AddHours(1);
                case Granularity.Day:
                    return (date) => date.AddDays(1);
                case Granularity.Week:
                    return (date) => date.AddDays(7);
                case Granularity.Month:
                    return (date) => date.AddMonths(1);
                case Granularity.Year:
                    return (date) => date.AddYears(1);
                default:
                    return (date) => date;
            }
        }

        private int NumberOfPeriods(DateTime fromIncludedUtc, DateTime toExcludedUtc, Granularity granularity)
        {
            var timeSpan = toExcludedUtc - fromIncludedUtc;

            if (fromIncludedUtc == toExcludedUtc)
                return 1;
            if (toExcludedUtc < fromIncludedUtc)
                return 0;

            switch (granularity)
            {
                case Granularity.Second:
                    return (int)timeSpan.TotalSeconds;
                case Granularity.Minute:
                    return (int)timeSpan.TotalMinutes;
                case Granularity.Hour:
                    return (int)timeSpan.TotalHours;
                case Granularity.Day:
                    return (int)timeSpan.TotalDays;
                case Granularity.Week:
                    return (int)(timeSpan.TotalDays / 7);
                case Granularity.Month:
                    return (toExcludedUtc.Month - fromIncludedUtc.Month) + 12 * (toExcludedUtc.Year - fromIncludedUtc.Year);
                case Granularity.Year:
                    return toExcludedUtc.Year - fromIncludedUtc.Year;
                default:
                    return 0;
            }
        }

        private bool IsTimestampRegular(Granularity granularity, DateTime timestamp)
        {
            bool seconds = timestamp.Millisecond == 0;
            bool minutes = seconds && timestamp.Second == 0;
            bool hours = minutes && timestamp.Minute == 0;
            bool days = hours && timestamp.Hour == 0;
            bool weeks = days && timestamp.DayOfWeek == DayOfWeek.Monday;
            bool months = days && timestamp.Day == 1;
            bool years = months && timestamp.Month == 1;

            switch (granularity)
            {
                case Granularity.Second:                    
                    return seconds;
                case Granularity.Minute:
                    return minutes;
                case Granularity.Hour:
                    return hours;
                case Granularity.Day:
                    return days;
                case Granularity.Week:
                    return weeks;
                case Granularity.Month:
                    return months;
                case Granularity.Year:
                    return years;
                default:
                    return false;
            }
        }

        public void Delete(int signalId)
        {
            this.missingValuePolicyRepository.Set(GetById(signalId), null);
            SignalDataRemove(GetById(signalId));
            this.signalsRepository.Delete(GetById(signalId));
        }

        private void SignalDataRemove(Signal signal)
        {
            switch(signal.DataType)
            {
                case DataType.Boolean: signalsDataRepository.DeleteData<bool>(signal); break;
                case DataType.Decimal: signalsDataRepository.DeleteData<decimal>(signal); break;
                case DataType.Double: signalsDataRepository.DeleteData<double>(signal); break;
                case DataType.Integer: signalsDataRepository.DeleteData<int>(signal); break;
                case DataType.String: signalsDataRepository.DeleteData<string>(signal); break;
            }
        }

        public IEnumerable<Datum<T>> GetCoarseData<T>(int signalId, Granularity granularity, DateTime fromIncludedUtc, DateTime toExcludedUtc)
        {
            var signal = GetById(signalId);

            if (signal.Granularity >= granularity)
                throw new ArgumentException("The granularity for coarse data has to be smaller than signal's granularity.");

            if (!IsTimestampRegular(granularity, fromIncludedUtc) || !IsTimestampRegular(granularity, toExcludedUtc))
                throw new IncorrectTimestampException();

            return default(IEnumerable<Datum<T>>);
        }
    }
}