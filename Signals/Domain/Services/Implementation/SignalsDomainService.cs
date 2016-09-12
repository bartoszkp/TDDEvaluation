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
            {
                throw new IdNotNullException();
            }

            Signal addedSignal = this.signalsRepository.Add(newSignal);

            var policy = SetNoneQualityMissingValuePolicy(newSignal);
            this.missingValuePolicyRepository.Set(addedSignal, policy);

            return addedSignal;
        }

        public void Delete(int signalId)
        {
            var signal = signalsRepository.Get(signalId);

            if (signal == null) throw new CouldntGetASignalException();

            DeleteSignalData(signal);
            missingValuePolicyRepository.Set(signal, null);

            signalsRepository.Delete(signal);
        }

        public Signal GetById(int signalId)
        {
            return this.signalsRepository.Get(signalId);
        }

        public Signal Get(Path pathDomain)
        {
            var signal = this.signalsRepository.Get(pathDomain);

            return signal;
        }

        public void SetMissingValuePolicy(int signalId, MissingValuePolicyBase policy)
        {
            var signal = this.GetById(signalId);

            if (signal == null)
            {
                throw new CouldntGetASignalException();
            }

            this.missingValuePolicyRepository.Set(signal, policy);
        }

        public Domain.MissingValuePolicy.MissingValuePolicyBase GetMissingValuePolicy(int signalId)
        {
            var signal = this.GetById(signalId);

            if (signal == null)
            {
                throw new CouldntGetASignalException();
            }

            var mvp = this.missingValuePolicyRepository.Get(signal);

            if (mvp == null)
            {
                throw new CouldntGetMissingValuePolicyException();
            }

            return TypeAdapter.Adapt(mvp, mvp.GetType(), mvp.GetType().BaseType)
                as MissingValuePolicy.MissingValuePolicyBase;
        }

        public void SetData<T>(Signal signal, IEnumerable<Datum<T>> dataDomain)
        {
            foreach (var datum in dataDomain)
            {
                if (!checkIfTimestampsAreCorrectBasedOnGranualityOfSignal(signal.Granularity, datum.Timestamp))
                {
                    throw new ArgumentException("incorrect timestamp(s)");
                }
            }

            Datum<T>[] dataDomainOrderedList = dataDomain.OrderBy(d => d.Timestamp).ToArray();

            for (int i = 0; i < dataDomain.Count(); ++i)
            {
                dataDomainOrderedList.ElementAt(i).Signal = signal;
            }

            this.signalsDataRepository.SetData<T>(dataDomainOrderedList);
        }



        public IEnumerable<Datum<T>> GetData<T>(Signal signal, DateTime fromIncludedUtc, DateTime toExcludedUtc)
        {
            if (!checkIfTimestampsAreCorrectBasedOnGranualityOfSignal(signal.Granularity, fromIncludedUtc))
            {
                throw new ArgumentException("incorrect timestamp(s)");
            }

            var data = this.signalsDataRepository.GetData<T>(signal, fromIncludedUtc, toExcludedUtc);

            MissingValuePolicyBase policy = GetMissingValuePolicy(signal.Id.Value);

            return DataFilledWithMissingValues<T>(data, signal, policy, fromIncludedUtc, toExcludedUtc);
        }

        private IEnumerable<Domain.Datum<T>> DataFilledWithMissingValues<T>(IEnumerable<Domain.Datum<T>> data,
            Signal signal, MissingValuePolicyBase policy, DateTime fromIncludedUtc, DateTime toExcludedUtc)
        {
            var filledList = data.OrderBy(x => x.Timestamp).ToList();
            int index = 0;

            if (fromIncludedUtc == toExcludedUtc)
            {
                if (filledList.Count == 0)
                    filledList.Add(GetDatumFilledWithMissingValuePolicy<T>(filledList, data, policy, signal, fromIncludedUtc));
                return filledList;
            }

            for (DateTime iterativeTime = fromIncludedUtc; iterativeTime < toExcludedUtc; index++, AddTimeBasedOnGranulatity(signal.Granularity, ref iterativeTime))
            {
                if (filledList.Find(d => d.Timestamp == iterativeTime) != null) continue;

                filledList.Insert(index, GetDatumFilledWithMissingValuePolicy<T>(filledList, data, policy, signal, iterativeTime));
            }
            return filledList;
        }

        private Domain.Datum<T> GetDatumFilledWithMissingValuePolicy<T>(List<Domain.Datum<T>> filledList, IEnumerable<Domain.Datum<T>> data, MissingValuePolicyBase policy, Signal signal,
            DateTime timestamp)
        {
            if (policy is NoneQualityMissingValuePolicy<T>)
            {
                return Domain.Datum<T>.CreateNone(signal, timestamp);
            }
            else if (policy is SpecificValueMissingValuePolicy<T>)
            {
                var specificPolicy = policy as MissingValuePolicy.SpecificValueMissingValuePolicy<T>;
                return new Domain.Datum<T>()
                {
                    Quality = specificPolicy.Quality,
                    Timestamp = timestamp,
                    Value = specificPolicy.Value
                };
            }
            else if (policy is ZeroOrderMissingValuePolicy<T>)
            {
                var result = filledList.Where(d => d.Timestamp < timestamp).LastOrDefault();

                if (result == null)
                    result = signalsDataRepository.GetDataOlderThan<T>(signal, timestamp, 1).LastOrDefault();

                if (result == null)
                    return Domain.Datum<T>.CreateNone(signal, timestamp);

                return new Datum<T>()
                {
                    Quality = result.Quality,
                    Signal = result.Signal,
                    Value = result.Value,
                    Timestamp = timestamp
                };
            }
            else if (policy is FirstOrderMissingValuePolicy<T>)
            {
                if (signal.DataType == DataType.Boolean || signal.DataType == DataType.String)
                    throw new TypeUnsupportedException();

                var left = data.Where(d => d.Timestamp <= timestamp).LastOrDefault();
                var right = data.Where(d => d.Timestamp > timestamp).FirstOrDefault();

                if (left == null)
                    left = signalsDataRepository.GetDataOlderThan<T>(signal, timestamp, 1).LastOrDefault();
                if (right == null)
                    right = signalsDataRepository.GetDataNewerThan<T>(signal, timestamp, 1).FirstOrDefault();
                    
                if(left == null || right == null)
                    return Datum<T>.CreateNone(signal, timestamp);

                var value = GenerateLinearInterpolationValue(left, right, timestamp, signal.Granularity);
                var quality = GenerateLinearInterpolationQuality(left.Quality, right.Quality);

                return new Datum<T>()
                {
                    Quality = quality,
                    Signal = signal,
                    Value = value,
                    Timestamp = timestamp
                };
            }
            else if (policy is ShadowMissingValuePolicy<T>)
            {
                var pol = policy as ShadowMissingValuePolicy<T>;
                var result = signalsDataRepository.GetData<T>(pol.ShadowSignal, timestamp, timestamp);

                if (result.Count() == 1)
                {
                    return result.ToArray()[0];
                }

                return new Datum<T>()
                {
                    Quality = Quality.None,
                    Signal = signal,
                    Value = default(T),
                    Timestamp = timestamp
                };
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        private T GenerateLinearInterpolationValue<T>(Datum<T> left, Datum<T> right, DateTime timestamp, Granularity granularity)
        {
            var numMissingValues = 0;
            var numMissedValue = 0;

            for (DateTime dt = left.Timestamp; dt < right.Timestamp; numMissingValues++, AddTimeBasedOnGranulatity(granularity, ref dt))
                if (dt == timestamp) numMissedValue = numMissingValues;

            dynamic leftValue = left.Value, 
                    rightValue = right.Value;

            var increaseValue = (rightValue - leftValue) / numMissingValues;

            return leftValue + (increaseValue * numMissedValue);
        }

        private Quality GenerateLinearInterpolationQuality(Quality left, Quality right)
        {
            if (left == Quality.None || right == Quality.None)
                return Quality.None;

            return ((int)left > (int)right) ? left : right;
        }
        
        private void AddTimeBasedOnGranulatity(Granularity granularity, ref DateTime dateTime)
        {
            switch (granularity)
            {
                case Granularity.Second:
                    dateTime = dateTime.AddSeconds(1);
                    return;
                case Granularity.Minute:
                    dateTime = dateTime.AddMinutes(1);
                    return;
                case Granularity.Hour:
                    dateTime = dateTime.AddHours(1);
                    return;
                case Granularity.Day:
                    dateTime = dateTime.AddDays(1);
                    return;
                case Granularity.Week:
                    dateTime = dateTime.AddDays(7);
                    return;
                case Granularity.Month:
                    dateTime = dateTime.AddMonths(1);
                    return;
                case Granularity.Year:
                    dateTime = dateTime.AddYears(1);
                    return;

                default: throw new NotImplementedException();
            }
        }

        private MissingValuePolicyBase SetNoneQualityMissingValuePolicy(Signal signal)
        {
            switch (signal.DataType)
            {
                case DataType.Boolean:
                    return new NoneQualityMissingValuePolicy<bool>();
                case DataType.Decimal:
                    return new NoneQualityMissingValuePolicy<decimal>();
                case DataType.Double:
                    return new NoneQualityMissingValuePolicy<double>();
                case DataType.Integer:
                    return new NoneQualityMissingValuePolicy<int>();
                case DataType.String:
                    return new NoneQualityMissingValuePolicy<string>();
                default:
                    throw new TypeUnsupportedException();
            }
        }

        private MissingValuePolicyBase SetSpecificValueMissingValuePolicy(Signal signal)
        {
            switch (signal.DataType)
            {
                case DataType.Boolean:
                    return new SpecificValueMissingValuePolicy<bool>();
                case DataType.Decimal:
                    return new SpecificValueMissingValuePolicy<decimal>();
                case DataType.Double:
                    return new SpecificValueMissingValuePolicy<double>();
                case DataType.Integer:
                    return new SpecificValueMissingValuePolicy<int>();
                case DataType.String:
                    return new SpecificValueMissingValuePolicy<string>();
                default:
                    throw new TypeUnsupportedException();
            }
        }

        private void DeleteSignalData(Signal signal)
        {
            switch (signal.DataType)
            {
                case DataType.Boolean:
                    signalsDataRepository.DeleteData<bool>(signal);
                    break;
                case DataType.Decimal:
                    signalsDataRepository.DeleteData<decimal>(signal);
                    break;
                case DataType.Double:
                    signalsDataRepository.DeleteData<double>(signal);
                    break;
                case DataType.Integer:
                    signalsDataRepository.DeleteData<int>(signal);
                    break;
                case DataType.String:
                    signalsDataRepository.DeleteData<string>(signal);
                    break;
                default:
                    throw new TypeUnsupportedException();
            }
        }

        public PathEntry GetByPrefixPath(Path path)
        {
            var pathEntry = new PathEntry();

            var allSignals = this.signalsRepository.GetAllWithPathPrefix(path);

            return FillPathEntryLists(allSignals, path);
        }

        private PathEntry FillPathEntryLists(IEnumerable<Domain.Signal> signals, Path prefix)
        {
            List<Signal> listOfSignals = new List<Signal>();
            List<Path> listOfPaths = new List<Path>();

            int prefixCount = prefix.Components.Count();

            var array = signals.ToArray();

            foreach (var signal in array)
            {
                if (signal.Path.Components.Count() - 1 == prefixCount)
                {
                    if (PathEquals(signal.Path, prefix))
                    {
                        listOfSignals.Add(signal);
                    }
                }
                else
                {
                    Path path = signal.Path;
                    List<string> components = new List<string>();
                    for (int i = 0; i < prefixCount + 1; ++i)
                    {
                        components.Add(path.Components.ElementAt(i));
                    }
                    string stringComponents = Domain.Path.JoinComponents(components);
                    listOfPaths.Add(Domain.Path.FromString(stringComponents));
                }
            }

            return new Domain.PathEntry(listOfSignals, listOfPaths.Distinct());
        }

        private bool PathEquals(Path expected, Path actual)
        {
            int actualCount = actual.Components.Count();
            for (int i = 0; i < actualCount; ++i)
            {
                if (!expected.Components.ElementAt(i).Equals(actual.Components.ElementAt(i)))
                {
                    return false;
                }
            }
            return true;
        }

        private bool checkIfTimestampsAreCorrectBasedOnGranualityOfSignal(Granularity granularity, DateTime timestamp)
        {
            switch (granularity)
            {
                case Granularity.Second:
                    if (timestamp.Millisecond != 0) return false;
                    return true;

                case Granularity.Minute:
                    if (timestamp.Second != 0 || timestamp.Millisecond !=0) return false;
                    return true;

                case Granularity.Hour:
                    if (timestamp.Minute != 0 || timestamp.Second !=0 || timestamp.Millisecond != 0) return false;
                    return true;

                case Granularity.Day:
                    if (timestamp.TimeOfDay.Ticks != 0) return false;
                    return true;

                case Granularity.Week:
                    if (timestamp.TimeOfDay.Ticks != 0 || timestamp.DayOfWeek != DayOfWeek.Monday) return false;
                    return true;

                case Granularity.Month:
                    if (timestamp.Day != 1 || timestamp.TimeOfDay.Ticks != 0) return false;
                    return true;

                case Granularity.Year:
                    if (timestamp.Month != 1 || timestamp.Day != 1 || timestamp.TimeOfDay.Ticks != 0) return false;
                    return true;

                default:
                    return true;
            }
        }

        private Quality SelectWorstQualityFromRange(IEnumerable<Quality> range)
        {
            if (range.Contains(Quality.None))
                return Quality.None;

            return range.Max();
        }

        public IEnumerable<Datum<T>> GetCoarseData<T>(Signal signal, Granularity granularity, DateTime fromIncludedUtc, DateTime toExcludedUtc)
        {
            if (signal.Granularity >= granularity)
                throw new ArgumentException("The given granularity should be bigger than the signals granularity.");

            if(!checkIfTimestampsAreCorrectBasedOnGranualityOfSignal(signal.Granularity, toExcludedUtc))
                throw new ArgumentException("incorrect timestamp(s)");
            if (!checkIfTimestampsAreCorrectBasedOnGranualityOfSignal(granularity, toExcludedUtc))
                throw new ArgumentException("incorrect timestamp(s)");
            if (!checkIfTimestampsAreCorrectBasedOnGranualityOfSignal(signal.Granularity, fromIncludedUtc))
                throw new ArgumentException("incorrect timestamp(s)");
            if (!checkIfTimestampsAreCorrectBasedOnGranualityOfSignal(granularity, fromIncludedUtc))
                throw new ArgumentException("incorrect timestamp(s)");
            if (fromIncludedUtc > toExcludedUtc)
                return Enumerable.Empty<Datum<T>>();

            if (signal.DataType == DataType.Boolean || signal.DataType == DataType.String)
                throw new TypeUnsupportedException();

            if (fromIncludedUtc == toExcludedUtc)
            {
                AddTimeBasedOnGranulatity(granularity, ref toExcludedUtc);

                var data = GetData<T>(signal, fromIncludedUtc, toExcludedUtc);
                dynamic value = data.Average(d => (dynamic)d.Value);
                value = Convert.ChangeType(value, typeof(T));
                var quality = SelectWorstQualityFromRange(data.Select(d => d.Quality));

                return new List<Datum<T>>() { new Datum<T>() { Value = value, Quality = quality, Timestamp = fromIncludedUtc } };
            }

            var result = new List<Datum<T>>();
            var timeIterator = fromIncludedUtc;

            while(timeIterator < toExcludedUtc)
            {
                var beginRange = timeIterator;
                AddTimeBasedOnGranulatity(granularity, ref timeIterator);

                var data = GetData<T>(signal, beginRange, timeIterator);
                dynamic value = data.Average(d => (dynamic)d.Value);
                value = Convert.ChangeType(value, typeof(T));
                var quality = SelectWorstQualityFromRange(data.Select(d => d.Quality));

                result.Add(new Datum<T>() { Value = value, Quality = quality, Timestamp = beginRange });
            }

            return result;
        }
    }
}