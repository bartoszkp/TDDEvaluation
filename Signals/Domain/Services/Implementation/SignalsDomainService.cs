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
            if(newSignal.Id.HasValue)
            {
                throw new IdNotNullException();
            }

            var signal = this.signalsRepository.Add(newSignal);
            if(missingValuePolicyRepository == null)
            {
                return signal;
            }

            string typeName = signal.DataType.GetNativeType().Name;

            switch (typeName)
            {
                case "Int32":
                    SetDefaultMissingValuePolicyToNoneQualityMissingValuePolicy<int>(signal);
                    break;
                case "Double":
                    SetDefaultMissingValuePolicyToNoneQualityMissingValuePolicy<double>(signal);
                    break;
                case "Decimal":
                    SetDefaultMissingValuePolicyToNoneQualityMissingValuePolicy<decimal>(signal);
                    break;
                case "Boolean":
                    SetDefaultMissingValuePolicyToNoneQualityMissingValuePolicy<bool>(signal);
                    break;
                case "String":
                    SetDefaultMissingValuePolicyToNoneQualityMissingValuePolicy<string>(signal);
                    break;
            }
            return signal;
        }


        private void SetDefaultMissingValuePolicyToNoneQualityMissingValuePolicy<T>(Signal signal)
        {
            this.missingValuePolicyRepository.Set(signal, new NoneQualityMissingValuePolicy<T>());
        }
        public Signal GetById(int signalId)
        {
            return this.signalsRepository.Get(signalId);
        }

        public void SetData<T>(int signalId, IEnumerable<Datum<T>> data)
        {
            var signal = GetById(signalId);
            
            foreach(var item in data)
            {
                item.Signal = signal;
            }

            signalsDataRepository.SetData(data);
        }
        
        public IEnumerable<Datum<T>> GetData<T>(int signalId, DateTime fromIncludedUtc, DateTime toExcludedUtc)
        {
            var signal = GetById(signalId);

            var data = this.signalsDataRepository
                .GetData<T>(signal, fromIncludedUtc, toExcludedUtc);
            foreach (var item in data)
            {
                switch (signal.Granularity)
                {
                    case Granularity.Second:
                        {
                            if(item.Timestamp.Millisecond != 0)
                            {
                                throw new TimestampHaveWrongFormatException();
                            }
                            break;
                        }
                    case Granularity.Minute:
                        {
                            if((item.Timestamp.Millisecond!=0)||(item.Timestamp.Second!=0))
                            {
                                throw new TimestampHaveWrongFormatException();
                            }
                            break;
                        }
                    case Granularity.Hour:
                        {
                            if ((item.Timestamp.Millisecond != 0) || (item.Timestamp.Second != 0) || (item.Timestamp.Minute != 0))
                            {
                                throw new TimestampHaveWrongFormatException();
                            }
                            break;
                        }
                    case Granularity.Day:
                        {
                            if ((item.Timestamp.Millisecond != 0) || (item.Timestamp.Second != 0) || (item.Timestamp.Minute != 0) || (item.Timestamp.Hour!=0))
                            {
                                throw new TimestampHaveWrongFormatException();
                            }
                            break;
                        }
                        
                }
            }
            if (fromIncludedUtc==toExcludedUtc)
            {
                List<Datum<T>> returnElement = new List<Datum<T>>();
                int indeks = 0;
                foreach(var x in data)
                {
                    if (fromIncludedUtc == x.Timestamp)
                    {
                        returnElement.Add(x);
                        return returnElement;
                    }
                    indeks++;    
                }
            }
            if(missingValuePolicyRepository == null)
            {
                return data;
            }
            var mvp = GetMissingValuePolicy(signal) as MissingValuePolicy.MissingValuePolicy<T>;

            return mvp.FillData(signal, data, fromIncludedUtc, toExcludedUtc).ToArray();
        }

        public Signal GetByPath(Path path)
        {
            var result = signalsRepository.Get(path);

            if (result == null)
                return null;

            return result;
        }

        public void SetMissingValuePolicy(Signal exampleSignal, MissingValuePolicyBase policy)
        {
            this.missingValuePolicyRepository.Set(exampleSignal, policy);
        }

        public MissingValuePolicyBase GetMissingValuePolicy(Signal signal)
        {
            var result = this.missingValuePolicyRepository.Get(signal);
            return TypeAdapter.Adapt(result, result.GetType(), result.GetType().BaseType) as MissingValuePolicy.MissingValuePolicyBase;
        }

        public PathEntry GetPathEntry(Path pathDomain)
        {
            var signals = signalsRepository.GetAllWithPathPrefix(pathDomain);

            if (signals == null)
                return null;

            var directPathSignals = signals
                .Where(s => s.Path.Length == pathDomain.Length + 1)
                .Select(s => s)
                .ToArray();
            
            var subPaths = signals
                .Where(s => s.Path.Length > pathDomain.Length + 1)
                .Select(s => s.Path.GetPrefix(pathDomain.Length + 1))
                .Distinct()
                .ToArray();
            
            return new PathEntry(directPathSignals, subPaths.AsEnumerable<Path>().ToArray());
        }
    }
}
