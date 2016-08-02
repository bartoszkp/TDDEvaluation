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
using Domain.MissingValuePolicy;
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
            if (pathDto == null)
                return null;

            return this.signalsDomainService
                .GetByPath(pathDto.ToDomain<Domain.Path>())
                .ToDto<Dto.Signal>();
        }

        public Signal GetById(int signalId)
        {
            return this.signalsDomainService
                .GetById(signalId)
                ?.ToDto<Dto.Signal>();
        }

        public Signal Add(Signal signalDto)
        {
            var signal = signalDto.ToDomain<Domain.Signal>();

            var result = this.signalsDomainService
                .Add(signal);

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
            return (IEnumerable<Datum>)RunGenericMethod("GetDataGeneric", signalId, fromIncludedUtc, toExcludedUtc);
        }

        public void SetData(int signalId, IEnumerable<Datum> data)
        {
            RunGenericMethod("SetDataGeneric", signalId, data);
        }

        private object RunGenericMethod(string functionName, int signalId, params object[] parameters)
        {
            var dataType = this.signalsDomainService.GetDataTypeById(signalId);

            MethodInfo method = GetType()
                .GetMethod(functionName)
                .MakeGenericMethod(new Type[] { dataType });
            
            return method.Invoke(this, new object[] { signalId }.Concat(parameters).ToArray());
        }

        public MissingValuePolicy GetMissingValuePolicy(int signalId)
        {
            return this.signalsDomainService.GetMissingValuePolicy(signalId)
                ?.ToDto<MissingValuePolicy>();
        }

        public void SetMissingValuePolicy(int signalId, MissingValuePolicy policy)
        {
            var domainPolicy = policy?.ToDomain<MissingValuePolicyBase>();

            this.signalsDomainService.SetMissingValuePolicy(signalId, domainPolicy);
        }

        public IEnumerable<Datum> GetDataGeneric<T>(int signalId, DateTime fromIncludedUtc, DateTime toExcludedUtc)
        {
            return this.signalsDomainService
                .GetData<T>(signalId, fromIncludedUtc, toExcludedUtc)
                .ToList()
                .ToDto<IEnumerable<Datum>>();
        }

        public void SetDataGeneric<T>(int signalId, IEnumerable<Datum> data)
        {
            var domainData = data.ToDomain<IEnumerable<Domain.Datum<T>>>();

            this.signalsDomainService.SetData(signalId, domainData);
        }
    }
}
