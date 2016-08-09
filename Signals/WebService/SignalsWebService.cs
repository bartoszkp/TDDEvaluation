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

            result.OrderBy(d => d.Timestamp);

            if (getSignal.DataType == Domain.DataType.Double)
                return ExecuteMVPAndConvertToDto<double>(getSignal, result, toExcludedUtc);
            else if (getSignal.DataType == Domain.DataType.Integer)
                return ExecuteMVPAndConvertToDto<int>(getSignal, result, toExcludedUtc);
            else if (getSignal.DataType == Domain.DataType.Decimal)
                return ExecuteMVPAndConvertToDto<decimal>(getSignal, result, toExcludedUtc);
            else if (getSignal.DataType == Domain.DataType.String)
                return ExecuteMVPAndConvertToDto<string>(getSignal, result, toExcludedUtc);
            else if (getSignal.DataType == Domain.DataType.Boolean)
                return ExecuteMVPAndConvertToDto<bool>(getSignal, result, toExcludedUtc);
            else
                throw new NotImplementedException();
        }

        public void SetData(int signalId, IEnumerable<Datum> data)
        {
            var setDataSignal = this.signalsDomainService.GetById(signalId);

            var newDomainDatum = new List<Domain.Datum<object>>();

            foreach (Datum d in data)
            {
                var domainDatum = d.ToDomain<Domain.Datum<object>>();
                newDomainDatum.Add(new Domain.Datum<object> { Signal = setDataSignal, Quality = domainDatum.Quality, Timestamp = domainDatum.Timestamp, Value = domainDatum.Value });
            }
            this.signalsDomainService.SetData(newDomainDatum);
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

        private IEnumerable<Domain.Datum<T>> ExecuteMVP<T>(
            Domain.Signal signal, MissingValuePolicy mvp, IEnumerable<Domain.Datum<object>> datum, DateTime toExcludedDate)
        {
            if (!(mvp is NoneQualityMissingValuePolicy))
                throw new NotImplementedException();

            var domain_datum = datum.ToArray();            
            if (domain_datum.Length == 0)
                return datum as IEnumerable<Domain.Datum<T>>;

            int expected_number_of_dates =
                DateDifferenceByGranularity(domain_datum.First().Timestamp, toExcludedDate, signal.Granularity);            
            if (domain_datum.Length == expected_number_of_dates)
                return datum as IEnumerable<Domain.Datum<T>>;

            var filledDatum = new Domain.Datum<T>[expected_number_of_dates];
            filledDatum[0] = domain_datum.First().ToDomain<Domain.Datum<T>>();
            filledDatum[filledDatum.Length - 1] = 
                (domain_datum.Last().Timestamp == DatePlusValueByGranularity(toExcludedDate, signal.Granularity, -1)) ? 
                domain_datum.Last().ToDomain<Domain.Datum<T>>() : 
                Domain.Datum<T>.CreateNone(signal, DatePlusValueByGranularity(toExcludedDate, signal.Granularity, -1));

            if (domain_datum.Length == 1)
            {
                for(int i = 1; i < filledDatum.Length - 1; ++i)
                    filledDatum[i] = Domain.Datum<T>.CreateNone(signal, DatePlusValueByGranularity(filledDatum[0].Timestamp, signal.Granularity,i));

                return filledDatum;
            }

            int domain_datum_iterator = 1;
            for(int i = 1; i < filledDatum.Length - 1; ++i)
            {
                if (domain_datum_iterator < domain_datum.Length &&
                    DatePlusValueByGranularity(filledDatum[i - 1].Timestamp, signal.Granularity) == domain_datum[domain_datum_iterator].Timestamp)
                    filledDatum[i] = domain_datum[domain_datum_iterator++].ToDomain<Domain.Datum<T>>();
                else
                {
                    filledDatum[i] = Domain.Datum<T>.CreateNone(
                        signal,
                        DatePlusValueByGranularity(filledDatum[i - 1].Timestamp, signal.Granularity)
                        );
                }
            }
            
            return filledDatum;            
        }

        private IEnumerable<Datum> ExecuteMVPAndConvertToDto<T>(Domain.Signal signal, 
            IEnumerable<Domain.Datum<object>> object_datum, DateTime toExcludedDate)
        {
            var mvpDatum = ExecuteMVP<T>(signal, GetMissingValuePolicy((int)signal.Id), object_datum, toExcludedDate);

            var dtoDatum = new List<Dto.Datum>();
            foreach (Domain.Datum<T> d in mvpDatum)
            {
                var quality = d.Quality.ToDto<Dto.Quality>();
                dtoDatum.Add(new Datum() { Value = d.Value, Quality = quality, Timestamp = d.Timestamp });
            }

            return dtoDatum;
        }
                
        private int DateDifferenceByGranularity(DateTime earlier, DateTime later, Domain.Granularity granularity)
        {
            int i = 1;            

            switch(granularity)
            {
                case Domain.Granularity.Year:
                    while (earlier.AddYears(i) != later) ++i;
                    break;
                case Domain.Granularity.Month:
                    while (earlier.AddMonths(i) != later) ++i;
                    break;
                case Domain.Granularity.Week:
                    while (earlier.AddDays(i * 7) != later) ++i;
                    break;
                case Domain.Granularity.Day:
                    while (earlier.AddDays(i) != later) ++i;
                    break;                
                case Domain.Granularity.Hour:
                    while (earlier.AddHours(i) != later) ++i;
                    break;
                case Domain.Granularity.Minute:
                    while (earlier.AddMinutes(i) != later) ++i;
                    break;
                case Domain.Granularity.Second:
                    while (earlier.AddSeconds(i) != later) ++i;
                    break;
            }

            return i;
        }
        private DateTime DatePlusValueByGranularity(DateTime date, Domain.Granularity granularity, int value = 1)
        {
            switch (granularity)
            {
                case Domain.Granularity.Year:
                    return date.AddYears(value);
                case Domain.Granularity.Month:
                    return date.AddMonths(value);
                case Domain.Granularity.Week:
                    return date.AddDays(7*value);
                case Domain.Granularity.Day:
                    return date.AddDays(value);
                case Domain.Granularity.Hour:
                    return date.AddHours(value);
                case Domain.Granularity.Minute:
                    return date.AddMinutes(value);
                case Domain.Granularity.Second:
                    return date.AddSeconds(value);

                default: return date;
            }
        }
    }

}
