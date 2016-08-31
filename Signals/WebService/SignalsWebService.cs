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
using Domain.MissingValuePolicy;
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

            var result = this.signalsDomainService.Get(pathDomain);

            return result.ToDto<Signal>();
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
            this.signalsDomainService.Delete(signalId);
        }

        public PathEntry GetPathEntry(Path pathDto)
        {
            var pathDomain = pathDto.ToDomain<Domain.Path>();
            var result = this.signalsDomainService.GetPathEntry(pathDomain);
            return result.ToDto<Dto.PathEntry>();
        }

        public IEnumerable<Datum> GetData(int signalId, DateTime fromIncludedUtc, DateTime toExcludedUtc)
        {
            var signal = this.signalsDomainService.GetById(signalId);

            if (signal == null)
                throw new IdNotNullException();
            
            switch (signal.DataType)
            {
                case Domain.DataType.Boolean:
                    {
                        return signalsDomainService.GetData<bool>(signal, fromIncludedUtc, toExcludedUtc)
                        .Select(d => d.ToDto<Dto.Datum>()).ToList().OrderBy(t => t.Timestamp);
                    }
                case Domain.DataType.Decimal:
                    {
                        return signalsDomainService.GetData<decimal>(signal, fromIncludedUtc, toExcludedUtc)
                        .Select(d => d.ToDto<Dto.Datum>()).ToList().OrderBy(t => t.Timestamp);
                    }
                case Domain.DataType.Double:
                    {
                        return signalsDomainService.GetData<double>(signal, fromIncludedUtc, toExcludedUtc)
                        .Select(d => d.ToDto<Dto.Datum>()).ToList().OrderBy(t => t.Timestamp);
                    }
                case Domain.DataType.Integer:
                    {
                        return signalsDomainService.GetData<int>(signal, fromIncludedUtc, toExcludedUtc)
                        .Select(d => d.ToDto<Dto.Datum>()).ToList().OrderBy(t => t.Timestamp);
                    }
                case Domain.DataType.String:
                    {
                        return signalsDomainService.GetData<string>(signal, fromIncludedUtc, toExcludedUtc)
                        .Select(d => d.ToDto<Dto.Datum>()).ToList().OrderBy(t => t.Timestamp);
                    }
                default:
                    {
                        return null;
                    }
            }
        }

        public void SetData(int signalId, IEnumerable<Datum> data)
        {
            var signal = signalsDomainService.GetById(signalId);
            if (signal == null)
                throw new IdNotNullException();

            switch (signal.DataType)
            {
                case Domain.DataType.Integer:
                    this.signalsDomainService.SetData(signal, data.ToDomain<IEnumerable<Domain.Datum<int>>>());
                    return;

                case Domain.DataType.Double:
                    this.signalsDomainService.SetData(signal, data.ToDomain<IEnumerable<Domain.Datum<double>>>());
                    return;

                case Domain.DataType.Boolean:
                    this.signalsDomainService.SetData(signal, data.ToDomain<IEnumerable<Domain.Datum<bool>>>());
                    return;

                case Domain.DataType.String:
                    this.signalsDomainService.SetData(signal, data.ToDomain<IEnumerable<Domain.Datum<string>>>());
                    return;

                case Domain.DataType.Decimal:
                    this.signalsDomainService.SetData(signal, data.ToDomain<IEnumerable<Domain.Datum<decimal>>>());
                    return;
            }

        }

        public MissingValuePolicy GetMissingValuePolicy(int signalId)
        {
            var signal = this.signalsDomainService.GetById(signalId);
            if (signal == null)
                throw new SignalIsNullException();

            return this.signalsDomainService.GetMissingValuePolicy(signal).ToDto<MissingValuePolicy>();
        }

        public void SetMissingValuePolicy(int signalId, MissingValuePolicy mvp)
        {
            var missingValuePolicy = mvp.ToDomain<Domain.MissingValuePolicy.MissingValuePolicyBase>();
            var signal = signalsDomainService.GetById(signalId);
            if (signal == null)
            {
                throw new SignalIsNullException();
            }
            this.signalsDomainService?.SetMissingValuePolicy(signal, missingValuePolicy);
        }
    }
}
