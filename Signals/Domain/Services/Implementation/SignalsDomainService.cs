using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Exceptions;
using Domain.Infrastructure;
using Domain.MissingValuePolicy;
using Domain.Repositories;
using Mapster;
using DataAccess.DataFillHelpers;
using Domain.Services.DataFillHelpers;

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

        public Signal Add<T>(Signal newSignal, NoneQualityMissingValuePolicy<T> nonePolicy)
        {
            var toReturn = this.signalsRepository.Add(newSignal);

            missingValuePolicyRepository.Set(newSignal, nonePolicy);

            return toReturn;
        }

        public Signal GetById(int signalId)
        {
            var result = signalsRepository.Get(signalId);
            if (result == null)
            {
                return null;
            }
            return result;
        }

        public Signal Get(Path pathDomain)
        {
            var result = signalsRepository.Get(pathDomain);
            if (result == null)
            {
                return null;
            }
            return result;
        }

        public MissingValuePolicyBase GetMissingValuePolicyBase(int signalId)
        {
            var signal = signalsRepository.Get(signalId);
            if (signal == null)
            {
                throw new NoSuchSignalException();
            }
            var mvp = missingValuePolicyRepository.Get(signal);
            if (mvp == null)
                return null;

            return TypeAdapter.Adapt(mvp, mvp.GetType(), mvp.GetType().BaseType)
                as MissingValuePolicy.MissingValuePolicyBase;
        }

        public void SetMissingValuePolicyBase(int signalId, MissingValuePolicyBase policy)
        {
            var signal = signalsRepository.Get(signalId);
            if (signal == null)
            {
                throw new NoSuchSignalException();
            }
            missingValuePolicyRepository.Set(signal, policy);
        }

        public IEnumerable<Datum<T>> GetData<T>(int signalId, DateTime fromIncludedUtc, DateTime toExcludedUtc)
        {
            Signal signal = GetById(signalId);
            if (signal == null)
                throw new NoSuchSignalException();

            if (!VeryfiTimeStamp(signal.Granularity, fromIncludedUtc))
                throw new InvalidOperationException("Timestamp error :" + fromIncludedUtc + " dosen't match for " + signal.Granularity);

            var data = signalsDataRepository.GetData<T>(signal, fromIncludedUtc, toExcludedUtc);

            if (data.Count() == 0)
            {
                data = signalsDataRepository.GetDataOlderThan<T>(signal, fromIncludedUtc, 1);
            }

            var sortedDatums = data?.OrderBy(datum => datum.Timestamp).ToList();

            var mvp = GetMissingValuePolicyBase(signalId);

            if (mvp is SpecificValueMissingValuePolicy<T>)
            {
                var specificMvp = mvp as SpecificValueMissingValuePolicy<T>;
                SpecificDataFillHelper.FillMissingData(signal.Granularity, sortedDatums, specificMvp.Value, fromIncludedUtc, toExcludedUtc);
            }
            else if(mvp is ZeroOrderMissingValuePolicy<T>)
            {
                ZeroOrderFillHelper.FillMissingData<T>(signal.Granularity, sortedDatums, fromIncludedUtc, toExcludedUtc);
            }
            else if (mvp is NoneQualityMissingValuePolicy<T>)
            {
                NoneQualityDataFillHelper.FillMissingData(signal.Granularity, sortedDatums, fromIncludedUtc, toExcludedUtc);
            }

            return sortedDatums.OrderBy(datum => datum.Timestamp);
        }

        public void SetData<T>(int signalId, IEnumerable<Datum<T>> dataDomain)
        {
            Signal signal = GetById(signalId);
            if (signal == null)
                throw new NoSuchSignalException();

            foreach(var item in dataDomain)
            {
                if (!VeryfiTimeStamp(signal.Granularity, item.Timestamp))
                    throw new InvalidOperationException("Timestamp error :" + item.Timestamp + " dosen't match for " + signal.Granularity);
            }


            var datums = new Datum<T>[dataDomain.Count()];
            int i = 0;
            foreach (Datum<T> d in dataDomain)
            {
                datums[i++] = new Datum<T>
                {
                    Quality = d.Quality,
                    Timestamp = d.Timestamp,
                    Value = d.Value,
                    Signal = signal
                };
            }

            IEnumerable<Datum<T>> sortedDatums = datums.OrderBy(datum => datum.Timestamp);
            signalsDataRepository.SetData<T>(sortedDatums);
        }


        public PathEntry GetPathEntry(Path prefixPath)
        {
            var matchingSignals = this.signalsRepository.GetAllWithPathPrefix(prefixPath);

            if (matchingSignals == null)
                return null;

            var filteredMatchingSignals = matchingSignals.Where(s => s.Path.Length == prefixPath.Length + 1);

            var signalsInSubPaths = matchingSignals.Where(s => s.Path.Length > prefixPath.Length + 1);

            var subPaths = CreateSubPathsList(signalsInSubPaths, prefixPath);


            return new PathEntry(filteredMatchingSignals, subPaths);

        }

        private List<Path> CreateSubPathsList(IEnumerable<Signal> signalsInSubPaths, Path prefixPath)
        {
            var subPaths = new List<Path>();

            foreach (var signal in signalsInSubPaths)
            {
                var filteredComponents = new string[prefixPath.Length + 1];

                for (int i = 0; i < filteredComponents.Length; i++)
                {
                    filteredComponents[i] = signal.Path.Components.ElementAt(i);
                }

                Path subpath = Path.FromString(Path.JoinComponents(filteredComponents));
                if (subPaths.Find(s => s.Equals(subpath)) == null)
                    subPaths.Add(subpath);
            }

            return subPaths;
        }

        private bool VeryfiTimeStamp(Granularity granularity, DateTime timeStamp)
        {
            switch(granularity)
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
                        if(timeStamp.DayOfYear == 1 && timeStamp.Hour == 0 && timeStamp.Minute == 0
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

            missingValuePolicyRepository.Set(signal, null);

            this.signalsRepository.Delete(signal);
        }
    }
}
