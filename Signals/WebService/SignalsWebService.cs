using System;
using System.Collections.Generic;
using System.Reflection;
using System.ServiceModel;
using Domain.Infrastructure;
using Domain.Services;
using Dto.Conversions;
using Microsoft.Practices.Unity;
using WebService.Infrastructure;

namespace WebService
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, IncludeExceptionDetailInFaults = true)]
    public class SignalsWebService : ISignalsWebService
    {
        public IUnityContainer UnityContainer { get; private set; }

        private readonly ISignalsDomainService signalsDomainService;

        public SignalsWebService(IUnityContainer unityContainer, ISignalsDomainService signalsDomainService)
        {
            this.UnityContainer = unityContainer;
            this.signalsDomainService = signalsDomainService;
        }     

        [DatabaseTransaction]
        public Dto.Signal Get(Dto.Path pathDto)
        {
            var path = pathDto.ToDomain<Domain.Path>();

            return this.signalsDomainService.Get(path).ToDto<Dto.Signal>();
        }

        [DatabaseTransaction]
        public Dto.Signal Add(Dto.Signal signalDto)
        {
            var signal = signalDto.ToDomain<Domain.Signal>();

            return this.signalsDomainService.Add(signal).ToDto<Dto.Signal>();
        }

        [DatabaseTransaction]
        public IEnumerable<Dto.Datum> GetData(Dto.Signal signalDto, DateTime fromIncluded, DateTime toExcluded)
        {
            var signal = signalDto.ToDomain<Domain.Signal>();

            var getData = GetAppropriateGetDataMethod(signal.DataType);

            return getData.Invoke(this.signalsDomainService, new object[] { signal, fromIncluded, toExcluded })
                .ToDto<IEnumerable<Dto.Datum>>();
        }

        [DatabaseTransaction]
        public void SetData(Dto.Signal signalDto, DateTime fromIncluded, IEnumerable<Dto.Datum> data)
        {
            var signal = signalDto.ToDomain<Domain.Signal>();

            var genericDatum = typeof(Domain.Datum<object>).GetGenericTypeDefinition();
            var concreteDatum = genericDatum.MakeGenericType(signal.DataType.GetNativeType());
            var genericIEnumerable = typeof(IEnumerable<object>).GetGenericTypeDefinition();
            var concreteIEnumerable = genericIEnumerable.MakeGenericType(concreteDatum);

            var setData = GetAppropriateSetDataMethod(signal.DataType);

            setData.Invoke(this.signalsDomainService, new object[] { signal, fromIncluded, concreteIEnumerable });
        }

        private static MethodInfo GetAppropriateGetDataMethod(Domain.DataType dataType)
        {
            var methodInfo = Infrastructure.ReflectionUtils
                .GetMethodInfo<ISignalsDomainService>(x => x.GetData<object>(null, default(DateTime), default(DateTime)));

            return GetAppropriateMethod(methodInfo.GetGenericMethodDefinition(), dataType);
        }

        private static MethodInfo GetAppropriateSetDataMethod(Domain.DataType dataType)
        {
            var methodInfo = Infrastructure.ReflectionUtils
                .GetMethodInfo<ISignalsDomainService>(x => x.SetData<object>(null, default(DateTime), null));

            return GetAppropriateMethod(methodInfo.GetGenericMethodDefinition(), dataType);
        }

        private static MethodInfo GetAppropriateMethod(MethodInfo genericMethodInfo, Domain.DataType dataType)
        {
            return genericMethodInfo.MakeGenericMethod(dataType.GetNativeType());
        }
    }
}
