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

        public Signal GetById(int signalId)
        {
            return this.signalsRepository.Get(signalId);
        }

        public Signal Add(Signal newSignal)
        {
            if (newSignal.Id.HasValue)
            {
                throw new IdNotNullException();
            }
            
            var result= this.signalsRepository.Add(newSignal);

            var dataType = result.DataType;       

            switch (dataType)
            {
                case Domain.DataType.Boolean:
                    missingValuePolicyRepository.Set(result, new NoneQualityMissingValuePolicy<Boolean>());
                    break;
                case Domain.DataType.Integer:
                    missingValuePolicyRepository.Set(result, new NoneQualityMissingValuePolicy<int>());
                    break;
                case Domain.DataType.Double:
                    missingValuePolicyRepository.Set(result, new NoneQualityMissingValuePolicy<double>());
                    break;
                case Domain.DataType.Decimal:
                    missingValuePolicyRepository.Set(result, new NoneQualityMissingValuePolicy<decimal>());
                    break;
                case Domain.DataType.String:
                    missingValuePolicyRepository.Set(result, new NoneQualityMissingValuePolicy<string>());
                    break;
                default:
                    break;
            }

            return result;
        }

        public Signal GetByPath(Path signalPath)
        {
            var result = this.signalsRepository.Get(signalPath);


            return result;
        }

        public void SetMissingValuePolicy(int signalId, MissingValuePolicyBase policy)
        {
            var signal = MissingValuePolicySignal(signalId);

            missingValuePolicyRepository.Set(signal, policy);
        }

        public MissingValuePolicyBase GetMissingValuePolicy(int signalId)
        {
            var signal = MissingValuePolicySignal(signalId);
            var mvp = this.missingValuePolicyRepository.Get(signal);

            if (mvp != null)
                mvp = TypeAdapter.Adapt(mvp, mvp.GetType(), mvp.GetType().BaseType)
                    as MissingValuePolicyBase;

            return mvp;
        }

        public void SetData<T>(int signalId, IEnumerable<Datum<T>> data)
        {
            var signal = this.signalsRepository.Get(signalId);
            var dataList = data.ToList();

            foreach (var d in dataList)
            {
                if (!VerifyTimestamp(d.Timestamp, signal.Granularity))
                    throw new InvalidTimestampException();
                d.Signal = signal;
            }

            this.signalsDataRepository.SetData(dataList);
        }

        public IEnumerable<Datum<T>> GetData<T>(int signalId, DateTime fromIncludedUtc, DateTime toExcludedUtc)
        {
            var signal = this.signalsRepository.Get(signalId);

            if (!VerifyTimestamp(fromIncludedUtc, signal.Granularity))
                throw new InvalidTimestampException();
                      
            var result = this.signalsDataRepository.GetData<T>(signal, fromIncludedUtc, toExcludedUtc);

            var sortedList = result.OrderBy(x => x.Timestamp).ToList();

            result = AddMissingDataDependOnMissingValuePolicy(sortedList, fromIncludedUtc, toExcludedUtc, signal);

            return result.OrderBy(s=>s.Timestamp);
        }

        public Type GetDataTypeById(int signalId)
        {
            var dataType = this.signalsRepository.Get(signalId)
                ?.DataType;

            if (dataType.HasValue == false)
                throw new SignalDoesntExistException();

            return DataTypeUtils.GetNativeType(dataType.Value);
        }

        public PathEntry GetPathEntry(Path path)
        {
            IEnumerable<Signal> allSignalsWithPathPrefix = signalsRepository.GetAllWithPathPrefix(path);

            int numberOfPathComponents = path.Components.Count();

            List<Signal> signalsFromSubdirectory = GetSignalsFromSubdirectory(allSignalsWithPathPrefix, numberOfPathComponents);
            List<Path> pathsFromSubdirectory = GetPathsFromSubdirectory(allSignalsWithPathPrefix, numberOfPathComponents);
            
            return new PathEntry(signalsFromSubdirectory, pathsFromSubdirectory);
        }


        private Signal MissingValuePolicySignal(int signalId)
        {
            var signal = this.signalsRepository.Get(signalId);

            if (signal == null)
                throw new SignalDoesntExistException();

            return signal;
        }

        private IEnumerable<Datum<T>> AddMissingDataDependOnMissingValuePolicy<T>
            (List<Datum<T>> datumsList, DateTime fromIncludedUtc, DateTime toExcludedUtc, Signal signal)
        {
            var mvp = GetMissingValuePolicy(signal.Id.Value);
            var time = fromIncludedUtc;

            if (mvp is NoneQualityMissingValuePolicy<T>)
                datumsList = AddNoneQualityMissingValuePolicy(datumsList, time, toExcludedUtc, signal);

            if (mvp is SpecificValueMissingValuePolicy<T>)
                datumsList = AddSpecificQualityMissingValuePolicy(datumsList, time, toExcludedUtc, signal, mvp);

            if (mvp is ZeroOrderMissingValuePolicy<T>)
                datumsList = AddZeroOrderMissingValuePolicy(datumsList, time, toExcludedUtc, signal);

            return datumsList;
        }

        private List<Datum<T>> AddZeroOrderMissingValuePolicy<T>
             (List<Datum<T>> datumsList, DateTime time, DateTime toExcludedUtc, Signal signal)
        {
            while (time < toExcludedUtc)
            {
                if (datumsList.FindIndex(x => x.Timestamp == time) < 0)
                {
                    var dataToAdd = signalsDataRepository.GetDataOlderThan<T>(signal, time, 1).SingleOrDefault();
                    if (dataToAdd != null)
                        datumsList.Add(new Datum<T>() { Quality = dataToAdd.Quality, Timestamp = time, Value = dataToAdd.Value });
                    else
                        datumsList.Add(new Datum<T>() { Quality = Quality.None, Timestamp = time, Value = default(T) });
                }
                time = ShiftTime(signal.Granularity, time);
            }

            return datumsList;
        }

        private List<Datum<T>> AddNoneQualityMissingValuePolicy<T>
             (List<Datum<T>> datumsList, DateTime time, DateTime toExcludedUtc, Signal signal)
        {
            while (time < toExcludedUtc)
            {
                if (datumsList.FindIndex(x => x.Timestamp == time) < 0)
                    datumsList.Add(new Datum<T>() { Quality = Quality.None, Timestamp = time, Value = default(T) });
                time = ShiftTime(signal.Granularity, time);
            }

            return datumsList;
        }

        private List<Datum<T>> AddSpecificQualityMissingValuePolicy<T>
            (List<Datum<T>> datumsList, DateTime time, DateTime toExcludedUtc, Signal signal, MissingValuePolicyBase mvp)
        {
            var specifiedMvp = mvp as SpecificValueMissingValuePolicy<T>;
            while (time < toExcludedUtc)
            {
                if (datumsList.FindIndex(x => x.Timestamp == time) < 0)
                {
                    datumsList.Add(new Datum<T>() { Quality = specifiedMvp.Quality, Timestamp = time, Value = specifiedMvp.Value });
                }
                time = ShiftTime(signal.Granularity, time);
            }

            return datumsList;
        }

        private DateTime ShiftTime(Granularity granularity, DateTime time, int shift = 1)
        {
            switch (granularity)
            {
                case Granularity.Second:
                    return time.AddSeconds(shift);
                case Granularity.Minute:
                    return time.AddMinutes(shift);
                case Granularity.Hour:
                    return time.AddHours(shift);
                case Granularity.Day:
                    return time.AddDays(shift);
                case Granularity.Week:
                    return time.AddDays(7 * shift);
                case Granularity.Month:
                    return time.AddMonths(shift);
                case Granularity.Year:
                    return time.AddYears(shift);
                default:
                    return new DateTime();
            }
        }

        private List<Signal> GetSignalsFromSubdirectory(IEnumerable<Signal> allSignals, int numberOfPathComponents)
        {
            List<Signal> returnSignals = new List<Signal>();

            List<Signal> signalsWithGivenPathPrefix = allSignals.ToList();

            for (int i = 0; i < signalsWithGivenPathPrefix.Count(); i++)
            {
                if (numberOfPathComponents - signalsWithGivenPathPrefix[i].Path.Components.Count() == -1)
                    returnSignals.Add(signalsWithGivenPathPrefix[i]);
            }

            return returnSignals;
        }

        private List<Path> GetPathsFromSubdirectory(IEnumerable<Signal> allSignals, int numberOfPathComponents)
        {
            List<Signal> returnSignalsPaths = new List<Signal>();

            List<Signal> signalsWithGivenPathPrefix = allSignals.ToList();

            for (int i = 0; i < signalsWithGivenPathPrefix.Count(); i++)
            {
                if (numberOfPathComponents - signalsWithGivenPathPrefix[i].Path.Components.Count() < -1)
                    returnSignalsPaths.Add(signalsWithGivenPathPrefix[i]);
            }

            var signalsPaths = returnSignalsPaths.Select(signal =>
               Path.FromString(signal.Path.Components.ElementAt(numberOfPathComponents - 1)) + signal.Path.Components.ElementAt(numberOfPathComponents))
                  .Distinct()
                  .Select(p => p);

            return signalsPaths.ToList();
        }

        private bool VerifyTimestamp(DateTime timestamp, Granularity granularity)
        {
            switch (granularity)
            {
                case Granularity.Second:
                    return timestamp.Millisecond == 0;
                case Granularity.Minute:
                    return timestamp.Second == 0 && timestamp.Millisecond == 0;
                case Granularity.Hour:
                    return timestamp.Minute == 0 && timestamp.Second == 0 && timestamp.Millisecond == 0;
                case Granularity.Day:
                    return timestamp.Hour == 0 && timestamp.Minute == 0 && timestamp.Second == 0 && timestamp.Millisecond == 0;
                case Granularity.Week:
                    return timestamp.DayOfWeek == DayOfWeek.Monday && timestamp.Hour == 0 && timestamp.Minute == 0 && timestamp.Second == 0 && timestamp.Millisecond == 0;
                case Granularity.Month:
                    return timestamp.Day == 1 && timestamp.Hour == 0 && timestamp.Minute == 0 && timestamp.Second == 0 && timestamp.Millisecond == 0;
                case Granularity.Year:
                    return timestamp.Month == 1 && timestamp.Day == 1 && timestamp.Hour == 0 && timestamp.Minute == 0 && timestamp.Second == 0 && timestamp.Millisecond == 0;
                default:
                    return false;
            }
        }
    } 
}
