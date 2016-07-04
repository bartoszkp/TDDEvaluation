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
            var path = pathDto.ToDomain<Domain.Path>();

            return this.signalsDomainService.Get(path).ToDto<Signal>();
        }

        public Signal GetById(int signalId)
        {
            return this.signalsDomainService.Get(signalId).ToDto<Signal>();
        }

        public Signal Add(Signal signalDto)
        {
            var signal = signalDto.ToDomain<Domain.Signal>();

            var result = this.signalsDomainService.Add(signal).ToDto<Signal>();

            return result;
        }

        public void Delete(int signalId)
        {
            this.signalsDomainService.Delete(signalId);
        }

        public PathEntry GetPathEntry(Path pathDto)
        {
            var path = pathDto.ToDomain<Domain.Path>();

            return this.signalsDomainService.GetPathEntry(path).ToDto<PathEntry>();
        }

        public IEnumerable<Datum> GetData(int signalId, DateTime fromIncludedUtc, DateTime toExcludedUtc)
        {
            var signal = this.signalsDomainService.Get(signalId);

            var getData = GetAppropriateGetDataMethod(signal.DataType);

            var result = getData
                .Invoke(this.signalsDomainService, new object[] { signal, fromIncludedUtc, toExcludedUtc })
                as IEnumerable;

            return result
                .Cast<object>()
                .Select(d => d.ToDto<Datum>())
                .ToArray();
        }

        public void SetData(int signalId, IEnumerable<Datum> data)
        {
            var signal = this.signalsDomainService.Get(signalId);

            var genericDatum = typeof(Domain.Datum<object>).GetGenericTypeDefinition();
            var concreteDatum = genericDatum.MakeGenericType(signal.DataType.GetNativeType());

            var dataArray = data.ToArray();
            var concreteData = Array.CreateInstance(concreteDatum, dataArray.Length);

            for (int i = 0;i < dataArray.Length;++i)
            {
                concreteData.SetValue(dataArray[i].ToDomain(concreteDatum), i);
            }

            var setData = GetAppropriateSetDataMethod(signal.DataType);

            setData.Invoke(this.signalsDomainService, new object[] { signal, concreteData });
        }

        private static MethodInfo GetAppropriateGetDataMethod(Domain.DataType dataType)
        {
            var methodInfo = ReflectionUtils
                .GetMethodInfo<ISignalsDomainService>(x => x.GetData<object>(null, default(DateTime), default(DateTime)));

            return GetAppropriateMethod(methodInfo.GetGenericMethodDefinition(), dataType);
        }

        private static MethodInfo GetAppropriateSetDataMethod(Domain.DataType dataType)
        {
            var methodInfo = ReflectionUtils
                .GetMethodInfo<ISignalsDomainService>(x => x.SetData<object>(null, null));

            return GetAppropriateMethod(methodInfo.GetGenericMethodDefinition(), dataType);
        }

        private static MethodInfo GetAppropriateMethod(MethodInfo genericMethodInfo, Domain.DataType dataType)
        {
            return genericMethodInfo.MakeGenericMethod(dataType.GetNativeType());
        }

        public MissingValuePolicy GetMissingValuePolicy(int signalId)
        {
            var signal = this.signalsDomainService.Get(signalId);

            return this.signalsDomainService.GetMissingValuePolicy(signal)?
                .ToDto<MissingValuePolicy>();
        }

        public void SetMissingValuePolicy(int signalId, MissingValuePolicy missingValuePolicy)
        {
            var mvp = missingValuePolicy.ToDomain<Domain.MissingValuePolicy.MissingValuePolicyBase>();

            this.signalsDomainService.SetMissingValuePolicy(
                this.signalsDomainService.Get(signalId),
                mvp);
        }
    }
}
