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
         return signalsDomainService.Get(pathDto.ToDomain<Domain.Path>())?.ToDto<Dto.Signal>();
        }

        public Signal GetById(int signalId)
        {
            return signalsDomainService.GetById(signalId)?.ToDto<Dto.Signal>();
        }

        public Signal Add(Signal signalDto)
        {
            var signal = signalDto.ToDomain<Domain.Signal>();

            return signalsDomainService.Add(signal).ToDto<Dto.Signal>();
        }

        public void Delete(int signalId)
        {
            throw new NotImplementedException();
        }

        public PathEntry GetPathEntry(Path pathDto)
        {
            var result = signalsDomainService.GetPathEntry(pathDto.ToDomain<Domain.Path>());

            return result.ToDto<Dto.PathEntry>();
        }

        public IEnumerable<Datum> GetData(int signalId, DateTime fromIncludedUtc, DateTime toExcludedUtc)
        {
            if (signalsDomainService != null)
            {
                var sig = signalsDomainService.GetById(signalId);
                var policy = GetMissingValuePolicy(signalId);

                if (sig == null) throw new ArgumentException();
                var k = sig.DataType;
                switch (k)
                {
                    case Domain.DataType.Boolean:
                        return signalsDomainService.GetData<Boolean>(sig, fromIncludedUtc, toExcludedUtc).Select(s => s.ToDto<Dto.Datum>());
                        
                    case Domain.DataType.Integer:
                        return signalsDomainService.GetData<int>(sig, fromIncludedUtc, toExcludedUtc).Select(s => s.ToDto<Dto.Datum>());
                       
                    case Domain.DataType.Double:
                        return signalsDomainService.GetData<double>(sig, fromIncludedUtc, toExcludedUtc).Select(s => s.ToDto<Dto.Datum>());
                     
                    case Domain.DataType.Decimal:
                        return signalsDomainService.GetData<decimal>(sig, fromIncludedUtc, toExcludedUtc).Select(s => s.ToDto<Dto.Datum>());
                       
                    case Domain.DataType.String:
                        return signalsDomainService.GetData<string>(sig, fromIncludedUtc, toExcludedUtc).Select(s => s.ToDto<Dto.Datum>());
                      
                    default:
                        break;
                }

              
            }
            return new Datum[] { new Datum()};
        }

        private bool VerifyTimeStamp(Domain.Signal signal, DateTime timestamp)
        {
            if (signal.Granularity == Domain.Granularity.Year && timestamp != new DateTime(timestamp.Year, 1, 1))
                return false;

            if (signal.Granularity == Domain.Granularity.Month && timestamp != new DateTime(timestamp.Year, timestamp.Month, 1))
                return false;

            if (signal.Granularity == Domain.Granularity.Week 
                && (timestamp.DayOfWeek != DayOfWeek.Monday 
                    || timestamp.Millisecond != 0 
                    || timestamp.Second != 0 
                    || timestamp.Minute != 0
                    || timestamp.Hour != 0))
                return false;

            if (signal.Granularity == Domain.Granularity.Day && timestamp != new DateTime(timestamp.Year, timestamp.Month, timestamp.Day))
                return false;

            if (signal.Granularity == Domain.Granularity.Hour 
                && timestamp != new DateTime(timestamp.Year, timestamp.Month, timestamp.Day, timestamp.Hour, 0, 0))
                return false;

            if (signal.Granularity == Domain.Granularity.Minute 
                && timestamp != new DateTime(timestamp.Year, timestamp.Month, timestamp.Day, timestamp.Hour, timestamp.Minute, 0))
                return false;

            if (signal.Granularity == Domain.Granularity.Second
                && timestamp != new DateTime(timestamp.Year, timestamp.Month, timestamp.Day, timestamp.Hour, timestamp.Minute, timestamp.Second))
                return false;

            return true;
        }

        public void SetData(int signalId, IEnumerable<Datum> data)
        {
            if (data == null || data.Count() == 0) return;



            if (signalsDomainService != null)
            {
                var result = signalsDomainService.GetById(signalId);

                if (result == null) throw new ArgumentException();

                foreach (var d in data)
                {
                    if (!VerifyTimeStamp(result, d.Timestamp))
                        throw new ArgumentException();
                }

                var type = DataTypeUtils.GetNativeType(result.DataType);
                var firstValueType = data.ToList()[0].Value?.GetType();

                if (firstValueType != null && type != firstValueType) throw new ArgumentException();

                switch (result.DataType)
                {

                    case Domain.DataType.Boolean:
                        if (type != typeof(Boolean)) throw new ArgumentException();
                        var list = (data.Select(s => {Domain.Datum<Boolean> sr= s.ToDomain<Domain.Datum<Boolean>>(); sr.Signal=result; return sr; }).ToList());
                        signalsDomainService.SetData<Boolean>(list);
                        break;
                    case Domain.DataType.Integer:
                        if (type != typeof(int)) throw new ArgumentException();
                        var list2 = (data.Select(s => { Domain.Datum<int> sr = s.ToDomain<Domain.Datum<int>>(); sr.Signal = result; return sr; }).ToList());
                        signalsDomainService.SetData<int>(list2);
                        break;
                    case Domain.DataType.Double:
                        if (type != typeof(double)) throw new ArgumentException();
                        var list3 = (data.Select(s => { Domain.Datum<double> sr = s.ToDomain<Domain.Datum<double>>(); sr.Signal = result; return sr; }).ToList());
                        
                        signalsDomainService.SetData<double>(list3);
                        break;
                    case Domain.DataType.Decimal:
                        if (type != typeof(decimal)) throw new ArgumentException();
                        var list4 = (data.Select(s => { Domain.Datum<decimal> sr = s.ToDomain<Domain.Datum<decimal>>(); sr.Signal = result; return sr; }).ToList());
                        signalsDomainService.SetData<Decimal>(list4);
                        break;
                    case Domain.DataType.String:
                        if (type != typeof(string)) throw new ArgumentException();
                        var list5 = (data.Select(s => { Domain.Datum<string> sr = s.ToDomain<Domain.Datum<string>>(); sr.Signal = result; return sr; }).ToList());
                        signalsDomainService.SetData<String>(list5);
                        break;
                    default:
                        break;
                }

            }
           
        }

        public MissingValuePolicy GetMissingValuePolicy(int signalId)
        {
            var sig = signalsDomainService.GetById(signalId);
            if (sig == null) throw new ArgumentException();

             return    signalsDomainService.Get(sig.ToDomain<Domain.Signal>())?.ToDto<Dto.MissingValuePolicy.MissingValuePolicy>();
        }

        public void SetMissingValuePolicy(int signalId, MissingValuePolicy policy)
        {
            
            var sig= signalsDomainService.GetById(signalId);
            if (sig == null) throw new ArgumentException();

            policy.ToDomain<Domain.MissingValuePolicy.MissingValuePolicyBase>();

            signalsDomainService.Set(sig, policy.ToDomain<Domain.MissingValuePolicy.MissingValuePolicyBase>());

        }
    }
}
