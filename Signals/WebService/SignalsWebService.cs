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
            return signalsDomainService
                ?.Get(pathDto.ToDomain<Domain.Path>())
                .ToDto<Dto.Signal>();
        }

        public Signal GetById(int signalId)
        {
            return signalsDomainService.GetById(signalId).ToDto<Dto.Signal>();
        }

        public Signal Add(Signal signalDto)
        {
            var signal = signalDto.ToDomain<Domain.Signal>();

            var result = signalsDomainService.Add(signal);

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
            throw new NotImplementedException();
        }

        public void SetData(int signalId, IEnumerable<Datum> data)
        {
            var signal = this.signalsDomainService?.GetById(signalId);
            
            if(signal == null)
            {
                throw new KeyNotFoundException();
            }

            this.signalsDomainService?.SetData(signal, data?.ToDomain<IEnumerable<Domain.Datum<double>>>());
        }

        public MissingValuePolicy GetMissingValuePolicy(int signalId)
        {
            var signal = signalsDomainService?.GetById(signalId);

            if(signal == null)
            {
                throw new KeyNotFoundException();
            }

            return this.signalsDomainService
                ?.GetMissingValuePolicy(signal)
                ?.ToDto<MissingValuePolicy>();
        }

        public void SetMissingValuePolicy(int signalId, MissingValuePolicy policy)
        {
            var mvp = policy?.ToDomain<Domain.MissingValuePolicy.MissingValuePolicyBase>();

            var signal = signalsDomainService?.GetById(signalId);
            
            if(signal == null)
            {
                throw new KeyNotFoundException();
            }

            this.signalsDomainService?.SetMissingValuePolicy(
                signal,
                mvp);
        }
    }
}
