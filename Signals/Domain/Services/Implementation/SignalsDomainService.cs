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

        private MissingValuePolicyBase ReturnMissingValuePolicy(Signal signal)
        {
            MissingValuePolicyBase policy;

            switch (signal.DataType)
            {
                case Domain.DataType.Boolean:
                        policy = new NoneQualityMissingValuePolicy<Boolean>();
                    break;
                case Domain.DataType.Decimal:
                        policy = new NoneQualityMissingValuePolicy<Decimal>();
                    break;
                case Domain.DataType.Double:
                         policy = new NoneQualityMissingValuePolicy<Double>();
                    break;
                case Domain.DataType.Integer:
                        policy = new NoneQualityMissingValuePolicy<Int32>();
                    break;
                case Domain.DataType.String:
                        policy = new NoneQualityMissingValuePolicy<String>();
                    break;
                default: return null;
            }

            return policy;
        }

        public Signal Add(Signal newSignal)
        {
            if (newSignal.Id.HasValue)
            {
                throw new IdNotNullException();
            }

            var signal = this.signalsRepository.Add(newSignal);

            var policy = ReturnMissingValuePolicy(signal);
            this.missingValuePolicyRepository.Set(signal, policy);

            return signal;
        }

        public Signal GetByPath(Path domainPath)
        {
            return signalsRepository.Get(domainPath);
        }

        public Signal GetById(int signalId)
        {
            return this.signalsRepository.Get(signalId);
        }

        public void SetData<T>(Signal foundSignal, IEnumerable<Datum<T>> data)
        {
            List<Datum<T>> dataList = data.ToList();
            for (int i = 0; i < dataList.Count; ++i)
            {
                CheckTimestamp(dataList[i].Timestamp, foundSignal.Granularity);
                dataList[i].Signal = foundSignal;
            }

            this.signalsDataRepository.SetData(dataList);
        }

        private void AddToDateTime(ref DateTime dateTime, Granularity granularity)
        {
            switch (granularity)
            {
                case Granularity.Second:
                    dateTime = dateTime.AddSeconds(1);
                    break;
                case Granularity.Minute:
                    dateTime = dateTime.AddMinutes(1);
                    break;
                case Granularity.Hour:
                    dateTime = dateTime.AddHours(1);
                    break;
                case Granularity.Day:
                    dateTime = dateTime.AddDays(1);
                    break;
                case Granularity.Week:
                    dateTime = dateTime.AddDays(7);
                    break;
                case Granularity.Month:
                    dateTime = dateTime.AddMonths(1);
                    break;
                case Granularity.Year:
                    dateTime = dateTime.AddYears(1);
                    break;
            }
        }
        
        public IEnumerable<Datum<T>> GetData<T>(Signal foundSignal, DateTime fromIncludedUtc, DateTime toExcludedUtc)
        {
            CheckTimestamp(fromIncludedUtc, foundSignal.Granularity);
            CheckTimestamp(toExcludedUtc, foundSignal.Granularity);
            
            var sortedData = signalsDataRepository.GetData<T>(foundSignal, fromIncludedUtc, toExcludedUtc)
                .OrderBy(d => d.Timestamp)
                .ToList();

            var policy = GetMissingValuePolicy(foundSignal.Id.Value) as MissingValuePolicy<T>;
            var index = 0;

            if (fromIncludedUtc == toExcludedUtc) toExcludedUtc = toExcludedUtc.AddTicks(1);

            for (DateTime dt = fromIncludedUtc; dt < toExcludedUtc; AddToDateTime(ref dt, foundSignal.Granularity), index++)
                if (sortedData.FirstOrDefault(d => d.Timestamp == dt) == null)
                    sortedData.Insert(index, policy.GetMissingDatum(sortedData, dt));
            
            return sortedData;
        }

        public void SetMissingValuePolicy(int signalId, MissingValuePolicyBase domainMvp)
        {
            Signal foundSignal = GetById(signalId);
            if (foundSignal == null)
                throw new SignalNotExistException();
                   
            missingValuePolicyRepository.Set(foundSignal, domainMvp);
        }

        public MissingValuePolicyBase GetMissingValuePolicy(int signalId)
        {
            Signal foundSignal = GetById(signalId);
            if (foundSignal == null)
                throw new SignalNotExistException();

            var mvp = missingValuePolicyRepository.Get(foundSignal);

            if (mvp == null)
                return null;

            return TypeAdapter.Adapt(mvp, mvp.GetType(), mvp.GetType().BaseType)
                as MissingValuePolicyBase;
        }

        public PathEntry GetAllWithPathPrefix(Path prefix)
        {
            var signals = signalsRepository.GetAllWithPathPrefix(prefix);
            var signalList = new List<Signal>();
            var subPathList = new List<Path>();
            var prefixCnt = prefix.Components.Count();

            foreach(var signal in signals)
            {
                var signalCnt = signal.Path.Components.Count();

                if (signalCnt == prefixCnt + 1)
                    signalList.Add(signal);
                else if (signalCnt > prefixCnt)
                    subPathList.Add(CreatePath(signal.Path.Components.Take(prefixCnt + 1)));
            }

            return new PathEntry(signalList, subPathList.Distinct()); 
        }

        private Path CreatePath(IEnumerable<string> components)
        {
            return Path.FromString(Path.JoinComponents(components));
        }

        private void CheckTimestamp(DateTime dt, Granularity granularity)
        {
            if(IsInvalid(dt, granularity))
                throw new DatetimeIsInvalidException(dt, granularity);
        }

        private bool IsInvalid(DateTime dt, Granularity granularity)
        {
            if ((int)granularity >= 0 && dt.Millisecond != 0) return true;
            if ((int)granularity >= 1 && dt.Second != 0) return true;
            if ((int)granularity >= 2 && dt.Minute != 0) return true;
            if ((int)granularity >= 3 && dt.Hour != 0) return true;
            if ((int)granularity == 4 && dt.DayOfWeek != DayOfWeek.Monday) return true;
            if ((int)granularity >= 5 && dt.Day != 1) return true;
            if ((int)granularity >= 6 && dt.Month != 1) return true;

            return false;
        }
    }
}
