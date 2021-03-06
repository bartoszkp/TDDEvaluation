﻿using System;
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

            return this.signalsDomainService.Get(path)?.ToDto<Signal>();
        }

        public Signal GetById(int signalId)
        {
            return this.signalsDomainService.Get(signalId)?.ToDto<Signal>();
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

            IEnumerable result;

            try
            {
                result = getData
                    .Invoke(this.signalsDomainService, new object[] { signal, fromIncludedUtc, toExcludedUtc })
                    as IEnumerable;
            }
            catch (TargetInvocationException te)
            {
                throw te.InnerException;
            }

            if (result == null)
                return null;

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

            for (int i = 0; i < dataArray.Length; ++i)
            {
                var dataType = dataArray[i].Value?.GetType();
                if (dataType != null && !dataType.Equals(signal.DataType.GetNativeType()))
                {
                    throw new Domain.Exceptions.IncorrectDataType(dataType.Name, signal.DataType.ToString());
                }

                concreteData.SetValue(dataArray[i].ToDomain(concreteDatum), i);
            }

            var setData = GetAppropriateSetDataMethod(signal.DataType);

            try
            {
                setData.Invoke(this.signalsDomainService, new object[] { signal, concreteData });
            }
            catch (TargetInvocationException te)
            {
                throw te.InnerException;
            }
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

            if (signal == null)
            {
                throw new KeyNotFoundException();
            }

            return this.signalsDomainService.GetMissingValuePolicy(signal)?
                .ToDto<MissingValuePolicy>();
        }

        public void SetMissingValuePolicy(int signalId, MissingValuePolicy policy)
        {
            var mvp = policy.ToDomain<Domain.MissingValuePolicy.MissingValuePolicyBase>();

            var signal = this.signalsDomainService.Get(signalId);

            if (signal == null)
            {
                throw new KeyNotFoundException();
            }

            this.signalsDomainService.SetMissingValuePolicy(
                signal,
                mvp);
        }

        public IEnumerable<Datum> GetCoarseData(int signalId, Granularity granularity, DateTime fromIncludedUtc, DateTime toExcludedUtc)
        {
            var signal = this.signalsDomainService.Get(signalId);

            var getCoarseData = GetAppropriateGetCoarseDataMethod(signal.DataType);

            IEnumerable result;

            try
            {
                result = getCoarseData
                    .Invoke(this.signalsDomainService, new object[] { signal, granularity.ToDomain<Domain.Granularity>(), fromIncludedUtc, toExcludedUtc })
                    as IEnumerable;
            }
            catch (TargetInvocationException te)
            {
                throw te.InnerException;
            }

            if (result == null)
                return null;

            return result
                .Cast<object>()
                .Select(d => d.ToDto<Datum>())
                .ToArray();
        }

        private static MethodInfo GetAppropriateGetCoarseDataMethod(Domain.DataType dataType)
        {
            var methodInfo = ReflectionUtils
                .GetMethodInfo<ISignalsDomainService>(x => x.GetCoarseData<object>(null, default(Domain.Granularity), default(DateTime), default(DateTime)));

            return GetAppropriateMethod(methodInfo.GetGenericMethodDefinition(), dataType);
        }
    }
}
