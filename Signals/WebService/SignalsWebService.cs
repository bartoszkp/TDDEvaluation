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
            var result = this.signalsDomainService.Get(path);
            return result.ToDto<Dto.Signal>();
        }

        public Signal GetById(int signalId)
        {
            return this.signalsDomainService
                .GetById(signalId)
                ?.ToDto<Dto.Signal>();
        }

        public Signal Add(Signal signalDto)
        {
            var signal = signalDto.ToDomain<Domain.Signal>();

            var result = this.signalsDomainService
                .Add(signal);

            return result.ToDto<Dto.Signal>();
        }

        public void Delete(int signalId)
        {
            signalsDomainService.Delete(signalId);
        }

        public PathEntry GetPathEntry(Path pathDto)
        {
            return this.signalsDomainService.GetPathEntry(pathDto.ToDomain<Domain.Path>()).ToDto<Dto.PathEntry>();
        }

        public IEnumerable<Datum> GetData(int signalId, DateTime fromIncludedUtc, DateTime toExcludedUtc)
        {
            var getSignal = this.signalsDomainService.GetById(signalId);
            if (getSignal == null) throw new InvalidSignalId();

            switch (getSignal.DataType)
            {
                case Domain.DataType.Double:
                    return this.signalsDomainService.GetData<double>(getSignal, fromIncludedUtc, toExcludedUtc).Select(d => d.ToDto<Dto.Datum>()).ToArray().OrderBy(t => t.Timestamp);
                case Domain.DataType.Decimal:
                    return this.signalsDomainService.GetData<decimal>(getSignal, fromIncludedUtc, toExcludedUtc).Select(d => d.ToDto<Dto.Datum>()).ToArray().OrderBy(t => t.Timestamp);
                case Domain.DataType.Integer:
                    return this.signalsDomainService.GetData<int>(getSignal, fromIncludedUtc, toExcludedUtc).Select(d => d.ToDto<Dto.Datum>()).ToArray().OrderBy(t => t.Timestamp);
                case Domain.DataType.Boolean:
                    return this.signalsDomainService.GetData<bool>(getSignal, fromIncludedUtc, toExcludedUtc).Select(d => d.ToDto<Dto.Datum>()).ToArray().OrderBy(t => t.Timestamp);
                case Domain.DataType.String:
                    return this.signalsDomainService.GetData<string>(getSignal, fromIncludedUtc, toExcludedUtc).Select(d => d.ToDto<Dto.Datum>()).ToArray().OrderBy(t => t.Timestamp);
                default: throw new NotImplementedException();
            }
        }

        public IEnumerable<Datum> GetCoarseData(int signalId, Granularity granularity, DateTime fromIncludedUtc, DateTime toExcludedUtc)
        {
            throw new NotImplementedException();
        }

        public void SetData(int signalId, IEnumerable<Datum> data)
        {
            var setDataSignal = this.signalsDomainService.GetById(signalId);
            if (signalId == 0 || setDataSignal == null) throw new InvalidSignalId();

            switch(setDataSignal.DataType)
            {
                case Domain.DataType.Double:
                    this.signalsDomainService.SetData(setDataSignal, data.Select(d => d.ToDomain<Domain.Datum<double>>()).ToArray());
                    break;
                case Domain.DataType.Decimal:
                    this.signalsDomainService.SetData(setDataSignal, data.Select(d => d.ToDomain<Domain.Datum<decimal>>()).ToArray());
                    break;
                case Domain.DataType.String:
                    this.signalsDomainService.SetData(setDataSignal, data.Select(d => d.ToDomain<Domain.Datum<string>>()).ToArray());
                    break;
                case Domain.DataType.Integer:
                    this.signalsDomainService.SetData(setDataSignal, data.Select(d => d.ToDomain<Domain.Datum<Int32>>()).ToArray());
                    break;
                case Domain.DataType.Boolean:
                    this.signalsDomainService.SetData(setDataSignal, data.Select(d => d.ToDomain<Domain.Datum<bool>>()).ToArray());
                    break;
                default: throw new NotImplementedException();
            }
        }

        public MissingValuePolicy GetMissingValuePolicy(int signalId)
        {
            var domainSignal = this.signalsDomainService.GetById(signalId).ToDomain<Domain.Signal>();
            var result = this.signalsDomainService.GetMVP(domainSignal);
            return result.ToDto<Dto.MissingValuePolicy.MissingValuePolicy>();
        }

        public void SetMissingValuePolicy(int signalId, MissingValuePolicy policy)
        {
            var domainSetMVPSignal = this.GetById(signalId).ToDomain<Domain.Signal>();
            var domainPolicyBase = policy.ToDomain<Domain.MissingValuePolicy.MissingValuePolicyBase>();
            this.signalsDomainService.SetMVP(domainSetMVPSignal, domainPolicyBase);
        }
    }
}
