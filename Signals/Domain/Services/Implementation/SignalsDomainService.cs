using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Exceptions;
using Domain.Infrastructure;
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
                throw new ArgumentException("Signal already has an Id.");

            var result =  this.signalsRepository.Add(newSignal);

            Type mvpType = typeof(MissingValuePolicy.NoneQualityMissingValuePolicy<>);
            mvpType = mvpType.MakeGenericType(result.DataType.GetNativeType());
            MissingValuePolicy.MissingValuePolicyBase mvp  = Activator.CreateInstance(mvpType) as MissingValuePolicy.MissingValuePolicyBase;
            SetMissingValuePolicy(result, mvp);

            return result;
        }

        public Signal GetById(int signalId)
        {
            return this.signalsRepository.Get(signalId);
        }

        public Signal GetByPath(Path signalPath)
        {
            Signal signal = this.signalsRepository.Get(signalPath);

            return signal;
        }

        public void Delete<T>(int signalId)
        {
            Signal signalToDelete = GetById(signalId);
            SetMissingValuePolicy(signalToDelete, null);

            signalsDataRepository.DeleteData<T>(signalToDelete);

            signalsRepository.Delete(signalToDelete);
        }

        public void SetData<T>(Signal signal, IEnumerable<Datum<T>> data)
        {
            List<Datum<T>> dataList = data.ToList();
            for (int i =0;i < dataList.Count; ++i)
                dataList[i].Signal = signal;

            this.signalsDataRepository.SetData<T>(dataList);
        }

        public IEnumerable<Domain.Datum<T>> GetData<T>(int signalId, DateTime fromIncludedUtc, DateTime toExcludedUtc)
        {
            Signal signal = GetById(signalId);

            var data = this.signalsDataRepository
                .GetData<T>(signal, fromIncludedUtc, toExcludedUtc)
                .OrderBy(d => d.Timestamp).ToList();

            MissingValuePolicy.MissingValuePolicy<T> mvp = GetMissingValuePolicy(signal) as MissingValuePolicy.MissingValuePolicy<T>;
            DateTime current = fromIncludedUtc;

            int i = 0;
            Datum<T> before = new Datum<T>() {}; 
            while(current < toExcludedUtc)
            {

                if (i >= data.Count || data[i].Timestamp != current)
                {
                    before = GetMissingValue<T>(mvp, signal, current, before, toExcludedUtc);             
                    data.Add(before);
                }
                else
                {
                    before = data[i];   
                    i++;
                }
               
                current = GetNextDateFromGranularity(current, signal.Granularity);
            }

            return data.OrderBy(d => d.Timestamp);
        }

        private Datum<T> GetMissingValue<T>(MissingValuePolicy.MissingValuePolicyBase mvp, Signal signal, DateTime timeStamp, Datum<T> before, DateTime toExcludedUtc)
        {
            if (mvp is MissingValuePolicy.NoneQualityMissingValuePolicy<T>)
            {
                return Datum<T>.CreateNone(signal, timeStamp);
            }
            else if (mvp is MissingValuePolicy.SpecificValueMissingValuePolicy<T>)
            {
                var specificMVP = mvp as MissingValuePolicy.SpecificValueMissingValuePolicy<T>;
                return Datum<T>.CreateSpecific(signal, timeStamp, specificMVP.Quality, specificMVP.Value);
            }
            else if (mvp is MissingValuePolicy.ZeroOrderMissingValuePolicy<T>)
            {
                Datum<T> returnDatum = Datum<T>.CreateSpecific(signal, timeStamp, before.Quality, before.Value);
                //if datums value is null find older datum
                if (returnDatum.Value == null)
                {
                    returnDatum = this.signalsDataRepository.GetDataOlderThan<T>(signal, timeStamp, 1).FirstOrDefault();
                    //if any old datum does not exist, create new datum with default value
                    if (returnDatum == null)
                        returnDatum = Datum<T>.CreateNone(signal, timeStamp);
                    else
                        returnDatum.Timestamp = new DateTime(timeStamp.Ticks);
                }
                return returnDatum;
            }
            return new Datum<T>();
        }

        private DateTime GetNextDateFromGranularity(DateTime current, Granularity granularity)
        {
            switch (granularity)
            {
                case Granularity.Second:
                    return current.AddSeconds(1);
                case Granularity.Minute:
                    return current.AddMinutes(1);
                case Granularity.Hour:
                    return current.AddHours(1);
                case Granularity.Day:
                    return current.AddDays(1);
                case Granularity.Week:
                    return current.AddDays(7);
                case Granularity.Month:
                    return current.AddMonths(1);
                case Granularity.Year:
                    return current.AddYears(1);
            }

            return current;
        }

        public void SetMissingValuePolicy(Signal signal, Domain.MissingValuePolicy.MissingValuePolicyBase policy)
        {
            this.missingValuePolicyRepository.Set(signal,policy);
        }

        public MissingValuePolicy.MissingValuePolicyBase GetMissingValuePolicy(Signal signal)
        {
            var mvp = this.missingValuePolicyRepository.Get(signal);

            return TypeAdapter.Adapt(mvp, mvp.GetType(), mvp.GetType().BaseType)
               as MissingValuePolicy.MissingValuePolicyBase;
        }

        public PathEntry GetPathEntry(Path path)
        {
            var allSignals = this.signalsRepository.GetAllWithPathPrefix(path);
            var signalsInDir = new List<Signal>();
            var subPaths = new List<Path>();
            int pathDomainComponentsCount = path.Components.Count();

            foreach (var signal in allSignals)
            {
                int signalPathComponentsCount = signal.Path.Components.Count();

                if (signalPathComponentsCount - 1 == pathDomainComponentsCount)
                {
                    signalsInDir.Add(signal);
                }
                else if (signalPathComponentsCount - 1 > pathDomainComponentsCount)
                {
                    subPaths.Add(Path.FromString(Path.JoinComponents(signal.Path.Components.Take(pathDomainComponentsCount + 1))));
                }
            }

            return new PathEntry(signalsInDir, subPaths.Distinct());
        }
    }
}
