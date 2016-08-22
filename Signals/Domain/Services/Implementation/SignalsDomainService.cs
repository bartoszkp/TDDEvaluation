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
                if(!checkIfTimestampsAreCorrectBasedOnGranualityOfSignal(signal.Granularity,datum.Timestamp))
                {
                    throw new ArgumentException("incorrect timestamp(s)");
                }
            }

            Datum<T>[] dataDomainOrderedList = dataDomain.OrderBy(d => d.Timestamp).ToArray();    
            
            for(int i = 0; i < dataDomain.Count(); ++i)
            {
                dataDomainOrderedList.ElementAt(i).Signal = signal;
            }

            this.signalsDataRepository.SetData<T>(dataDomainOrderedList);
        }

      

        public IEnumerable<Datum<T>> GetData<T>(Signal signal, DateTime fromIncludedUtc, DateTime toExcludedUtc)
        {
            var data = this.signalsDataRepository.GetData<T>(signal, fromIncludedUtc, toExcludedUtc);

            MissingValuePolicyBase policy = GetMissingValuePolicy(signal.Id.Value);

            return DataFilledWithMissingValues<T>(data, signal, policy ,fromIncludedUtc,toExcludedUtc);
        }

        private IEnumerable<Domain.Datum<T>> DataFilledWithMissingValues<T>(IEnumerable<Domain.Datum<T>> data, 
            Signal signal, MissingValuePolicyBase policy, DateTime fromIncludedUtc, DateTime toExcludedUtc)
        {
            List<Domain.Datum<T>> filledList = new List<Datum<T>>();
            var array = data.OrderBy(x => x.Timestamp).ToArray();
            int indexOfArray = 0;
            DateTime lastIterativeTime;

            if (fromIncludedUtc == toExcludedUtc)
            {
                for (; indexOfArray < array.Length; ++indexOfArray)
                {
                    if (array[indexOfArray].Timestamp == fromIncludedUtc)
                    {
                        filledList.Add(array[indexOfArray]);
                        return filledList;
                    }
                }
            }
            for (DateTime iterativeTime = fromIncludedUtc; iterativeTime<toExcludedUtc;)
            {
                if (indexOfArray < array.Length && array[indexOfArray].Timestamp >= iterativeTime)
                {
                    lastIterativeTime = iterativeTime;
                    AddTimeBasedOnGranulatity(signal.Granularity, ref iterativeTime);

                    if (array[indexOfArray].Timestamp < iterativeTime)
                    {
                        filledList.Add(array.ElementAt(indexOfArray));
                        ++indexOfArray;
                    }
                    else
                    {
                        filledList.Add(GetDatumFilledWithMissingValuePolicy<T>(policy, signal, lastIterativeTime));
                    }
                }
                else
                {
                    lastIterativeTime = iterativeTime;
                    AddTimeBasedOnGranulatity(signal.Granularity, ref iterativeTime);
                    filledList.Add(GetDatumFilledWithMissingValuePolicy<T>(policy, signal, lastIterativeTime));
                }
            }
            return filledList;
        }

        private Domain.Datum<T> GetDatumFilledWithMissingValuePolicy<T>(MissingValuePolicyBase policy,Signal signal, 
            DateTime timestamp)
        {
            if(policy is NoneQualityMissingValuePolicy<T>)
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
            else
            {
                throw new NotImplementedException();
            }
        }

        private void AddTimeBasedOnGranulatity(Granularity granularity,ref DateTime dateTime)
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

            foreach(var signal in array)
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
                    for(int i = 0; i < prefixCount + 1; ++i)
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
                    if (timestamp.Second != 0) return false;
                    return true;

                case Granularity.Hour:
                    if (timestamp.Minute != 0) return false;
                    return true;

                case Granularity.Day:
                    if (timestamp.TimeOfDay.Ticks != 0) return false;
                    return true;

                case Granularity.Week:
                    if (timestamp.TimeOfDay.Ticks != 0 || timestamp.DayOfWeek == DayOfWeek.Monday) return false;
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
    }
}
