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
            throw new NotImplementedException();
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
            switch (signal.DataType)
            {
                case (DataType.Boolean):
                    SetData<bool>(signalId, data);
                    return;

                case (DataType.Decimal):
                    SetData<decimal>(signalId, data);
                    return;

                case (DataType.Double):
                    SetData<double>(signalId, data);
                    return;

                case (DataType.Integer):
                    SetData<int>(signalId, data);
                    return;

                case (DataType.String):
                    SetData<string>(signalId, data);
                    return;
            }
            throw new NotSupportedException("Type is not supported");
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
            var result = signalsDomainService.GetMissingValuePolicyBase(signalId);
            return result?.ToDto<MissingValuePolicy>();
        }

        public void SetMissingValuePolicy(int signalId, MissingValuePolicy policy)
        {
            var policyDomain = policy.ToDomain<Domain.MissingValuePolicy.MissingValuePolicyBase>();
            signalsDomainService.SetMissingValuePolicyBase(signalId, policyDomain);
        }

        
    }
}
