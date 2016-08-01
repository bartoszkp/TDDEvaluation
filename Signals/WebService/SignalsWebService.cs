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
            throw new NotImplementedException();
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
            Domain.Signal domainSignal = GetById(signalId)?.ToDomain<Domain.Signal>();

            if(domainSignal == null)
                throw new ArgumentException("A signal with the given Id does not exist");

            switch (domainSignal.DataType)
            {
                case Domain.DataType.Boolean:
                    return signalsDomainService.GetData<bool>(domainSignal, fromIncludedUtc, toExcludedUtc)
                        .Select(d => d.ToDto<Dto.Datum>());
                case Domain.DataType.Decimal:
                    return signalsDomainService.GetData<decimal>(domainSignal, fromIncludedUtc, toExcludedUtc)
                        .Select(d => d.ToDto<Dto.Datum>());
                case Domain.DataType.Double:
                    return signalsDomainService.GetData<double>(domainSignal, fromIncludedUtc, toExcludedUtc)
                        .Select(d => d.ToDto<Dto.Datum>());
                case Domain.DataType.Integer:
                    return signalsDomainService.GetData<int>(domainSignal, fromIncludedUtc, toExcludedUtc)
                        .Select(d => d.ToDto<Dto.Datum>());
                case Domain.DataType.String:
                    return signalsDomainService.GetData<string>(domainSignal, fromIncludedUtc, toExcludedUtc)
                        .Select(d => d.ToDto<Dto.Datum>());
            }

            return null;
        }

        private IEnumerable<Domain.Datum<T>> ConvertDtoDataToDomain<T>(IEnumerable<Dto.Datum> data, Dto.Signal signal)
        {
            return data.Select(d =>
            {
                var domainSignal = d.ToDomain<Domain.Datum<T>>();
                domainSignal.Signal = signal.ToDomain<Domain.Signal>();
                return domainSignal;
            });
        }

        public void SetData(int signalId, IEnumerable<Datum> data)
        {
            var signal = GetById(signalId);

            if (signal == null)
                throw new ArgumentException("A signal with the given Id does not exist");

            switch (signal.DataType)
            {
                case DataType.Boolean:
                    signalsDomainService.SetData(ConvertDtoDataToDomain<bool>(data,signal));
                    break;
                case DataType.Decimal:
                    signalsDomainService.SetData(ConvertDtoDataToDomain<decimal>(data, signal));
                    break;
                case DataType.Double:
                    signalsDomainService.SetData(ConvertDtoDataToDomain<double>(data, signal));
                    break;
                case DataType.Integer:
                    signalsDomainService.SetData(ConvertDtoDataToDomain<int>(data, signal));
                    break;
                case DataType.String:
                    signalsDomainService.SetData(ConvertDtoDataToDomain<string>(data, signal));
                    break;
            }
        }

        public MissingValuePolicy GetMissingValuePolicy(int signalId)
        {
            Domain.Signal signal = GetById(signalId)?.ToDomain<Domain.Signal>();
            if(signal == null)
                throw new ArgumentException("A signal with the given Id does not exist");

            var result = signalsDomainService.GetMissingValuePolicy(signal);

            var returnResult =  result.ToDto<MissingValuePolicy>();
            returnResult.DataType = result.NativeDataType.FromNativeType().ToDto<Dto.DataType>();

            return returnResult;
        }

        public void SetMissingValuePolicy(int signalId, MissingValuePolicy policy)
        {
            Domain.Signal signal = GetById(signalId)?.ToDomain<Domain.Signal>();
            if (signal == null)
                throw new ArgumentException("A signal with the given Id does not exist");
            var domainPolicy = policy.ToDomain<Domain.MissingValuePolicy.MissingValuePolicyBase>();

            signalsDomainService.SetMissingValuePolicy(signal, domainPolicy);
        }
    }
}
