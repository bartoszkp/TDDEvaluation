using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using DataAccess.Infrastructure;
using Domain.Infrastructure;
using Domain.Services;
using Dto;
using Dto.Conversions;
using Dto.MissingValuePolicy;
using Microsoft.Practices.Unity;
using Domain.Exceptions;

namespace WebService
{
    [UnityRegister(typeof(ContainerControlledLifetimeManager), typeof(DatabaseTransactionInterceptionInjectionMembersFactory))]
    [ServiceBehavior(IncludeExceptionDetailInFaults = true)]
    public class SignalsWebService : ISignalsWebService
    {
        private readonly ISignalsDomainService signalsDomainService;

        public SignalsWebService(ISignalsDomainService signalsDomainService)
        {
             this.signalsDomainService = signalsDomainService;
        }     

        public Signal Get(Path pathDto)
        {
                Domain.Path path = pathDto.ToDomain<Domain.Path>();
                var result = this.signalsDomainService.Get(path);
                return result.ToDto<Dto.Signal>();
        }

        public Signal GetById(int signalId)
        {
            return this.signalsDomainService.GetById(signalId)
                ?.ToDto<Dto.Signal>();
        }

        public Signal Add(Signal signalDto)
        {
            var signal = signalDto.ToDomain<Domain.Signal>();
            var result = this.signalsDomainService.Add(signal);
            return result.ToDto<Dto.Signal>();
        }

        public void Delete(int signalId)
        {
            var signal = this.signalsDomainService.GetById(signalId);
            if (signal == null)
                throw new NoSuchSignalException();

            this.signalsDomainService.Delete(signal);
        }

        public PathEntry GetPathEntry(Path pathDto)
        {
            var result = signalsDomainService.GetPathEntry(pathDto.ToDomain<Domain.Path>());

            return result.ToDto<Dto.PathEntry>();
        }

        public IEnumerable<Datum> GetData(int signalId, DateTime fromIncludedUtc, DateTime toExcludedUtc)
        {
            if (fromIncludedUtc > toExcludedUtc)
                return new List<Datum>();

            var signal = this.signalsDomainService?.GetById(signalId);
            if (signal == null)
            {
                throw new KeyNotFoundException();
            }

            string typeName = signal.DataType.GetNativeType().Name;

            switch (typeName)
            {
                case "Int32":
                    return GenericGetDataCall<int>(signal, fromIncludedUtc, toExcludedUtc);
                case "Double":
                    return GenericGetDataCall<double>(signal, fromIncludedUtc, toExcludedUtc);
                case "Decimal":
                    return GenericGetDataCall<decimal>(signal, fromIncludedUtc, toExcludedUtc);
                case "Boolean":
                    return GenericGetDataCall<bool>(signal, fromIncludedUtc, toExcludedUtc);
                case "String":
                    return GenericGetDataCall<string>(signal, fromIncludedUtc, toExcludedUtc);
            }
            
            return null;
        }

        private IEnumerable<Datum> GenericGetDataCall<T>(Domain.Signal signal, DateTime fromIncludedUtc, DateTime toExcludedUtc)
        {
            return this.signalsDomainService?
                    .GetData<T>(signal, fromIncludedUtc, toExcludedUtc)?
                    .ToDto<IEnumerable<Dto.Datum>>();
        }

        public void SetData(int signalId, IEnumerable<Datum> datum)
        {
            var signal = this.signalsDomainService.GetById(signalId);

            if (signal == null)
            {
                throw new KeyNotFoundException();
            }

                var data = datum?.OrderBy(dt => dt.Timestamp).ToArray();
                string typeName = signal.DataType.GetNativeType().Name;

                switch (typeName)
                {
                    case "Int32":
                    GenericSetDataCall<int>(signal, data);
                    break;
                    case "Double":
                        GenericSetDataCall<double>(signal, data);
                        break;
                    case "Decimal":
                        GenericSetDataCall<decimal>(signal, data);
                        break;
                    case "Boolean":
                        GenericSetDataCall<bool>(signal, data);
                        break;
                    case "String":
                        GenericSetDataCall<string>(signal, data);
                        break;
                }
            }
        

        private void GenericSetDataCall<T>(Domain.Signal signal, Datum[] data)
        {
            this.signalsDomainService?.SetData(signal, data?.ToDomain<IEnumerable<Domain.Datum<T>>>().ToArray());
        }

        public MissingValuePolicy GetMissingValuePolicy(int signalId)
        {
           var signalDomain = this.signalsDomainService.GetById(signalId);
           return this.signalsDomainService.GetMissingValuePolicy(signalDomain).ToDto<MissingValuePolicy>();
        }

        public void SetMissingValuePolicy(int signalId, MissingValuePolicy policy)
        {
            var domainPolicyBase = policy.ToDomain<Domain.MissingValuePolicy.MissingValuePolicyBase>();
            this.signalsDomainService.SetMissingValuePolicy(signalId, domainPolicyBase);
        }
    }
}
