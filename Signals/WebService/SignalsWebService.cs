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
            Domain.Path path = pathDto.ToDomain<Domain.Path>();

            var result = signalsDomainService.Get(path);

            return result?.ToDto<Dto.Signal>();
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
            var sig = signalsDomainService.GetById(signalId);

            if (sig == null)
                throw new ArgumentException();


            switch (sig.DataType)
            {
                case Domain.DataType.Boolean:
                    signalsDomainService.DeleteData<Boolean>(sig);
                    break;
                case Domain.DataType.Decimal:
                    signalsDomainService.DeleteData<decimal>(sig);
                    break;
                case Domain.DataType.Double:
                    signalsDomainService.DeleteData<double>(sig);
                    break;
                case Domain.DataType.Integer:
                    signalsDomainService.DeleteData<int>(sig);
                    break;
                case Domain.DataType.String:
                    signalsDomainService.DeleteData<string>(sig);
                    break;
            }

            signalsDomainService.Delete(sig);

        }

        public PathEntry GetPathEntry(Path pathDto)
        {
            return signalsDomainService.GetPathEntry(pathDto.ToDomain<Domain.Path>())?.ToDto<Dto.PathEntry>();
        }

        public IEnumerable<Datum> GetData(int signalId, DateTime fromIncludedUtc, DateTime toExcludedUtc)
        {
            Domain.Signal domainSignal = GetById(signalId)?.ToDomain<Domain.Signal>();

            if (domainSignal == null)
                throw new CouldntGetASignalException();

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
                throw new CouldntGetASignalException();

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
            if (signal == null)
                throw new CouldntGetASignalException();

            var result = signalsDomainService.GetMissingValuePolicy(signal);

            if (result == null)
                return null;

            var returnResult =  result.ToDto<MissingValuePolicy>();
            returnResult.DataType = result.NativeDataType.FromNativeType().ToDto<Dto.DataType>();

            return returnResult;
        }

        public void SetMissingValuePolicy(int signalId, MissingValuePolicy policy)
        {
            Domain.Signal signal = GetById(signalId)?.ToDomain<Domain.Signal>();
            if (signal == null)
                throw new CouldntGetASignalException();
            var domainPolicy = policy.ToDomain<Domain.MissingValuePolicy.MissingValuePolicyBase>();

            signalsDomainService.SetMissingValuePolicy(signal, domainPolicy);
        }
    }
}
