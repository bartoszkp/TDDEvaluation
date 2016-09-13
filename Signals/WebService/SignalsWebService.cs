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
            var pathDomain = pathDto.ToDomain<Domain.Path>();
            Domain.Signal result = signalsDomainService.Get(pathDomain);
            return result?.ToDto<Dto.Signal>();
        }

        public Signal GetById(int signalId)
        {
            return this.signalsDomainService.GetById(signalId)
                ?.ToDto<Dto.Signal>();
        }

        public Signal Add(Signal signalDto)
        {
            var result = signalsDomainService.Add(signalDto.ToDomain<Domain.Signal>());
            return result.ToDto<Dto.Signal>();
        }

        public void Delete(int signalId)
        {
            this.signalsDomainService.Delete(signalId);
        }

        public PathEntry GetPathEntry(Path pathDto)
        {
            if (pathDto == null)
                throw new ArgumentNullException();
            if (pathDto.Components == null)
                return null;

            return this.signalsDomainService.GetPathEntry(pathDto.ToDomain<Domain.Path>())
                        ?.ToDto<Dto.PathEntry>();
        }

        public IEnumerable<Datum> GetData(int signalId, DateTime fromIncludedUtc, DateTime toExcludedUtc)
        {
            Signal signal = GetById(signalId);
            if (signal == null)
                throw new NoSuchSignalException();
            switch (signal.DataType)
            {
                case (DataType.Boolean):
                    return GetData<bool>(signalId, fromIncludedUtc, toExcludedUtc);

                case (DataType.Decimal):
                    return GetData<decimal>(signalId, fromIncludedUtc, toExcludedUtc);

                case (DataType.Double):
                    return GetData<double>(signalId, fromIncludedUtc, toExcludedUtc);

                case (DataType.Integer):
                    return GetData<int>(signalId, fromIncludedUtc, toExcludedUtc);

                case (DataType.String):
                    return GetData<string>(signalId, fromIncludedUtc, toExcludedUtc);
            }
            throw new NotSupportedException("Type is not supported");
        }

        public IEnumerable<Datum> GetCoarseData(int signalId, Granularity granularity, DateTime fromIncludedUtc, DateTime toExcludedUtc)
        {
            Signal signal = GetById(signalId);
            if (signal == null)
                throw new NoSuchSignalException();

            var domainSignal = signal.ToDomain<Domain.Signal>();
            var domainGranularity = granularity.ToDomain<Domain.Granularity>();
            switch (signal.DataType)
            {
                case (DataType.Decimal):
                    return signalsDomainService.GetCoarseData<decimal>(domainSignal, domainGranularity, fromIncludedUtc, toExcludedUtc).ToDto<IEnumerable<Datum>>();
                case (DataType.Double):
                    return signalsDomainService.GetCoarseData<double>(domainSignal, domainGranularity, fromIncludedUtc, toExcludedUtc).ToDto<IEnumerable<Datum>>();
                case (DataType.Integer):
                    return signalsDomainService.GetCoarseData<int>(domainSignal, domainGranularity, fromIncludedUtc, toExcludedUtc).ToDto<IEnumerable<Datum>>();
                case (DataType.String):
                    throw new ArgumentException();
                case (DataType.Boolean):
                    throw new ArgumentException();
            }
            return null;
        }

        public IEnumerable<Datum> GetData<T>(int signalId, DateTime fromIncludedUtc, DateTime toExcludedUtc)
        {
            var enumerable = signalsDomainService.GetData<T>(signalId, fromIncludedUtc, toExcludedUtc);

            var datums = new Datum[enumerable.Count()];
            int i = 0;
            foreach (var e in enumerable)
                datums[i++] = e.ToDto<Datum>();

            return datums;
        }

        public void SetData(int signalId, IEnumerable<Datum> data)
        {
            Signal signal = GetById(signalId);
            if (signal == null)
                throw new Domain.Exceptions.NoSuchSignalException();
            var switchDataType = new Dictionary<DataType, Action>()
            {
                {DataType.Boolean,()=> SetData<bool>(signalId, data) },
                {DataType.Decimal,()=> SetData<decimal>(signalId, data) },
                {DataType.Double,()=> SetData<double>(signalId, data) },
                {DataType.Integer,()=> SetData<int>(signalId, data) },
                {DataType.String,()=> SetData<string>(signalId, data) }
            };
            switchDataType[signal.DataType].Invoke();
        }

        public void SetData<T>(int signalId, IEnumerable<Datum> data)
        {
            var datums = new Domain.Datum<T>[data.Count()];
            int i = 0;
            foreach (var e in data)
            {
                datums[i++] = new Domain.Datum<T>()
                {
                    Quality = e.Quality.ToDomain<Domain.Quality>(),
                    Timestamp = e.Timestamp,
                    Value = e.Value.ToDomain<T>()
                };
            }
            signalsDomainService.SetData(signalId, datums);
        }

        public MissingValuePolicy GetMissingValuePolicy(int signalId)
        {
            var signal = signalsDomainService.GetById(signalId);
            return this.signalsDomainService.GetMissingValuePolicy(signal).ToDto<MissingValuePolicy>();
        }

        public void SetMissingValuePolicy(int signalId, MissingValuePolicy policy)
        {
            var policyDomain = policy.ToDomain<Domain.MissingValuePolicy.MissingValuePolicyBase>();
            signalsDomainService.SetMissingValuePolicyBase(signalId, policyDomain);
        }
    }
}
