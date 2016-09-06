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
                .GetByPath(pathDto.ToDomain<Domain.Path>())
                ?.ToDto<Dto.Signal>();
        }

        public Signal GetById(int signalId)
        {
            return signalsDomainService
                .GetById(signalId)
                ?.ToDto<Dto.Signal>();
        }

        public Signal Add(Signal signalDto)
        {
            var signal = signalDto.ToDomain<Domain.Signal>();

            var result = signalsDomainService.Add(signal);

            return result.ToDto<Dto.Signal>();
        }

        public void Delete(int signalId)
        {
            MethodInfo method = GetType()
                  .GetMethod("DeleteGeneric", BindingFlags.NonPublic | BindingFlags.Instance)
                  .MakeGenericMethod(new Type[] { GetSignalType(signalId) });

            object[] pararm = new object[] { (object)signalId };
            method.Invoke(this,pararm);
        }

        private void DeleteGeneric<T>(int signalId)
        {
            signalsDomainService.Delete<T>(signalId);
        }

        public PathEntry GetPathEntry(Path pathDto)
        {
            var result = signalsDomainService
                .GetAllWithPathPrefix(pathDto.ToDomain<Domain.Path>());

            return result.ToDto<PathEntry>();
        }

        public IEnumerable<Datum> GetData(int signalId, DateTime fromIncludedUtc, DateTime toExcludedUtc)
        {
            return (IEnumerable<Datum>)RunGenericMethod(GetSignalType(signalId), 
                "GetDataGeneric", signalId, fromIncludedUtc, toExcludedUtc);
        }

        public void SetData(int signalId, IEnumerable<Dto.Datum> data)
        {
            RunGenericMethod(GetSignalType(signalId), "SetDataGeneric", signalId, data);
        }

        public void SetMissingValuePolicy(int signalId, MissingValuePolicy policy)
        {
            RunGenericMethod(GetSignalType(signalId), "SetMissingValuePolicyGeneric", signalId, policy);
        }

        public MissingValuePolicy GetMissingValuePolicy(int signalId)
        {
            return signalsDomainService
                .GetMissingValuePolicy(signalId)
                ?.ToDto<Dto.MissingValuePolicy.MissingValuePolicy>();
        }

        public IEnumerable<Datum> GetDataGeneric<T>(int signalId, DateTime fromIncludedUtc, DateTime toExcludedUtc)
        {
            return signalsDomainService
                .GetData<T>(signalId, fromIncludedUtc, toExcludedUtc)
                .ToDto<IEnumerable<Datum>>();
        }

        public void SetDataGeneric<T>(int signalId, IEnumerable<Datum> data)
        {
            var cos = data.ToDomain<IEnumerable<Domain.Datum<T>>>();
            signalsDomainService
                .SetData(signalId, data.ToDomain<IEnumerable<Domain.Datum<T>>>());
        }

        public void SetMissingValuePolicyGeneric<T>(int signalId, MissingValuePolicy policy)
        {
            var domainMvp = policy.ToDomain<Domain.MissingValuePolicy.MissingValuePolicyBase>();

            signalsDomainService.SetMissingValuePolicy<T>(signalId, domainMvp);
        }

        private object RunGenericMethod(Type type, string methodName, params object[] param)
        {
            MethodInfo method = GetType()
                .GetMethod(methodName)
                .MakeGenericMethod(new Type[] { type });

            return method.Invoke(this, param.ToArray());
        }

        private Type GetSignalType(int signalId)
        {
            return signalsDomainService.GetSignalType(signalId);
        }
    }
}
