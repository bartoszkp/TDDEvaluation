using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using Domain.Infrastructure;
using Domain.Services;
using Dto.Conversions;

namespace WebService
{
    [ServiceBehavior(IncludeExceptionDetailInFaults = true)]
    public class SignalWebService : ISignalWebService
    {
        private readonly ISignalDomainService signalDomainService;

        public SignalWebService(ISignalDomainService signalDomainService)
        {
            this.signalDomainService = signalDomainService;
        }     

        public Dto.Signal Get(Dto.Path pathDto)
        {
            var path = pathDto.ToDomain<Domain.Path>();

            return this.signalDomainService.Get(path).ToDto<Dto.Signal>();
        }

        public Dto.Signal Add(Dto.Signal signalDto)
        {
            var signal = signalDto.ToDomain<Domain.Signal>();

            return this.signalDomainService.Add(signal).ToDto<Dto.Signal>();
        }

        public IEnumerable<Dto.Datum> GetData(Dto.Signal signalDto, DateTime fromIncluded, DateTime toExcluded)
        {
            var signal = signalDto.ToDomain<Domain.Signal>();

            var getData = GetAppropriateGetDataMethod(signal.DataType);

            var result = getData
                .Invoke(this.signalDomainService, new object[] { signal, fromIncluded, toExcluded })
                as IEnumerable;

            return result
                .Cast<object>()
                .Select(d => d.ToDto<Dto.Datum>())
                .ToArray();
        }

        public void SetData(Dto.Signal signalDto, IEnumerable<Dto.Datum> data)
        {
            var signal = signalDto.ToDomain<Domain.Signal>();

            var genericDatum = typeof(Domain.Datum<object>).GetGenericTypeDefinition();
            var concreteDatum = genericDatum.MakeGenericType(signal.DataType.GetNativeType());

            var dataArray = data.ToArray();
            var concreteData = Array.CreateInstance(concreteDatum, dataArray.Length);

            for (int i = 0;i < dataArray.Length;++i)
            {
                concreteData.SetValue(dataArray[i].ToDomain(concreteDatum), i);
            }

            var setData = GetAppropriateSetDataMethod(signal.DataType);

            setData.Invoke(this.signalDomainService, new object[] { signal, concreteData });
        }

        private static MethodInfo GetAppropriateGetDataMethod(Domain.DataType dataType)
        {
            var methodInfo = ReflectionUtils
                .GetMethodInfo<ISignalDomainService>(x => x.GetData<object>(null, default(DateTime), default(DateTime)));

            return GetAppropriateMethod(methodInfo.GetGenericMethodDefinition(), dataType);
        }

        private static MethodInfo GetAppropriateSetDataMethod(Domain.DataType dataType)
        {
            var methodInfo = ReflectionUtils
                .GetMethodInfo<ISignalDomainService>(x => x.SetData<object>(null, null));

            return GetAppropriateMethod(methodInfo.GetGenericMethodDefinition(), dataType);
        }

        private static MethodInfo GetAppropriateMethod(MethodInfo genericMethodInfo, Domain.DataType dataType)
        {
            return genericMethodInfo.MakeGenericMethod(dataType.GetNativeType());
        }
    }
}
