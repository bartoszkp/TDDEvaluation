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
            var signal = signalsDomainService.GetById(signalId);

            if (signal == null)
                throw new SignalNotFoundException(signalId);

            switch (signal.DataType)
            {
                case Domain.DataType.Boolean:
                    return signalsDomainService.GetData<bool>(signal, fromIncludedUtc, toExcludedUtc)
                        .Select(d => d.ToDto<Dto.Datum>()).ToList();
                case Domain.DataType.Integer:
                    return signalsDomainService.GetData<int>(signal, fromIncludedUtc, toExcludedUtc)
                        .Select(d => d.ToDto<Dto.Datum>()).ToList();
                case Domain.DataType.Double:
                    return signalsDomainService.GetData<double>(signal, fromIncludedUtc, toExcludedUtc)
                        .Select(d => d.ToDto<Dto.Datum>()).ToList();
                case Domain.DataType.Decimal:
                    return signalsDomainService.GetData<decimal>(signal, fromIncludedUtc, toExcludedUtc)
                        .Select(d => d.ToDto<Dto.Datum>()).ToList();
                case Domain.DataType.String:
                    return signalsDomainService.GetData<string>(signal, fromIncludedUtc, toExcludedUtc)
                        .Select(d => d.ToDto<Dto.Datum>()).ToList();
                default:
                    throw new NotImplementedException();
            }
        }

        public void SetData(int signalId, IEnumerable<Datum> data)
        {
            var signal = signalsDomainService.GetById(signalId);

            if (signal == null)
                throw new SignalNotFoundException(signalId);

            switch (signal.DataType)
            {
                case Domain.DataType.Boolean:
                    signalsDomainService.SetData(signal, data.Select(d => d.ToDomain<Domain.Datum<bool>>()));
                    break;
                case Domain.DataType.Integer:
                    signalsDomainService.SetData(signal, data.Select(d => d.ToDomain<Domain.Datum<int>>()));
                    break;
                case Domain.DataType.Double:
                    signalsDomainService.SetData(signal, data.Select(d => d.ToDomain<Domain.Datum<double>>()));
                    break;
                case Domain.DataType.Decimal:
                    signalsDomainService.SetData(signal, data.Select(d => d.ToDomain<Domain.Datum<decimal>>()));
                    break;
                case Domain.DataType.String:
                    signalsDomainService.SetData(signal, data.Select(d => d.ToDomain<Domain.Datum<string>>()));
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        public MissingValuePolicy GetMissingValuePolicy(int signalId)
        {
            var signal = signalsDomainService.GetById(signalId);

            if (signal == null)
                throw new SignalNotFoundException(signalId);

            return signalsDomainService.GetMissingValuePolicy(signal)
                ?.ToDto<Dto.MissingValuePolicy.MissingValuePolicy>();
        }

        public void SetMissingValuePolicy(int signalId, MissingValuePolicy policy)
        {
            var signal = signalsDomainService.GetById(signalId);

            if (signal == null)
                throw new SignalNotFoundException(signalId);

            var domainPolicy = policy.ToDomain<Domain.MissingValuePolicy.MissingValuePolicyBase>();

            signalsDomainService.SetMissingValuePolicy(signal, domainPolicy);
        }
    }
}
