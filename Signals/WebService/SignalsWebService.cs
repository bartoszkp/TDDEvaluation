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
            throw new NotImplementedException();
        }

        public PathEntry GetPathEntry(Path pathDto)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Datum> GetData(int signalId, DateTime fromIncludedUtc, DateTime toExcludedUtc)
        {
            var getSignal = this.signalsDomainService.GetById(signalId);
            var result = this.signalsDomainService.GetData(getSignal, fromIncludedUtc, toExcludedUtc);
            var dtoDatum = new List<Dto.Datum>();
            foreach (Domain.Datum<double> d in result)
            {
                var quality = d.Quality.ToDto<Dto.Quality>();
                dtoDatum.Add(new Datum() { Value = d.Value, Quality = quality, Timestamp = d.Timestamp });
            }
            return dtoDatum;
        }

        public void SetData(int signalId, IEnumerable<Datum> data)
        {
            var setDataSignal = this.signalsDomainService.GetById(signalId);
            var newDomainDatum = new List<Domain.Datum<double>>();

            foreach (Datum d in data)
            {
                var domainDatum = d.ToDomain<Domain.Datum<double>>();
                newDomainDatum.Add(new Domain.Datum<double> { Signal = setDataSignal, Quality = domainDatum.Quality, Timestamp = domainDatum.Timestamp, Value = domainDatum.Value });
            }
            this.signalsDomainService.SetData(newDomainDatum);
        }

        public MissingValuePolicy GetMissingValuePolicy(int signalId)
        {
            var domainSignal = this.signalsDomainService.GetById(signalId).ToDomain<Domain.Signal>();
            var result = this.signalsDomainService.GetMVP(domainSignal);
            return result.ToDto<Dto.MissingValuePolicy.SpecificValueMissingValuePolicy>();
        }

        public void SetMissingValuePolicy(int signalId, MissingValuePolicy policy)
        {
            var domainSetMVPSignal = this.GetById(signalId).ToDomain<Domain.Signal>();
            var domainPolicyBase = policy.ToDomain<Domain.MissingValuePolicy.MissingValuePolicyBase>();
            this.signalsDomainService.SetMVP(domainSetMVPSignal, domainPolicyBase);
        }
    }
}
