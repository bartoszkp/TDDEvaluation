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
            Domain.Path domainPath = pathDto.ToDomain<Domain.Path>();
            return signalsDomainService.GetByPath(domainPath).ToDto<Dto.Signal>();
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
            throw new NotImplementedException();
        }

        public PathEntry GetPathEntry(Path pathDto)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Datum> GetData(int signalId, DateTime fromIncludedUtc, DateTime toExcludedUtc)
        {
            List<Dto.Datum> dtoDatum = new List<Dto.Datum>();

            IEnumerable<Domain.Datum<object>> domainDatum = signalsDomainService.GetData(signalId, fromIncludedUtc, toExcludedUtc);

            foreach (Domain.Datum<object> d in domainDatum)
            {
                dtoDatum.Add(d.ToDto<Dto.Datum>());
            }

            return dtoDatum;
        }

        public void SetData(int signalId, IEnumerable<Dto.Datum> data)
        {
            List<Domain.Datum<object>> domainData = new List<Domain.Datum<object>>();
            
            foreach (Dto.Datum dtoDatum in data)
            {
                domainData.Add(dtoDatum.ToDomain<Domain.Datum<object>>());
            }

            signalsDomainService.SetData(signalId, domainData);
        }

        public MissingValuePolicy GetMissingValuePolicy(int signalId)
        {
            return signalsDomainService.GetMissingValuePolicy(signalId)?.ToDto<Dto.MissingValuePolicy.MissingValuePolicy>();
        }

        public void SetMissingValuePolicy(int signalId, MissingValuePolicy policy)
        {
            var domainMvp = policy.ToDomain<Domain.MissingValuePolicy.SpecificValueMissingValuePolicy<object>>();

            signalsDomainService.SetMissingValuePolicy(signalId, domainMvp);
        }
    }
}
