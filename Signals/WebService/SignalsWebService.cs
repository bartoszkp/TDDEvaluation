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
            Domain.Path pathDomain = pathDto.ToDomain<Domain.Path>();
            return this.signalsDomainService.Get(pathDomain)?.ToDto<Dto.Signal>();
        }

        public Signal GetById(int signalId)
        {
            return this.signalsDomainService.GetById(signalId)
                ?.ToDto<Dto.Signal>();
        }

        public Signal Add(Signal signalDto)
        {
            var signal = signalDto.ToDomain<Domain.Signal>();

            var result = this.signalsDomainService.Add(signal).ToDto<Dto.Signal>();

            NoneQualityMissingValuePolicy mvp = new NoneQualityMissingValuePolicy()
            {
                DataType = result.DataType,
                Signal = result
            };
            
            this.SetMissingValuePolicy(mvp);

            return result;
        }

        public void Delete(int signalId)
        {
            throw new NotImplementedException();
        }

        public PathEntry GetPathEntry(Path pathDto)
        {
            Domain.Path path = pathDto.ToDomain<Domain.Path>();

            var result = signalsDomainService.GetPathEntry(path);

            return result.ToDto<Dto.PathEntry>();
        }

        public IEnumerable<Datum> GetData(int signalId, DateTime fromIncludedUtc, DateTime toExcludedUtc)
        {
            Domain.Signal signal = this.GetById(signalId)?.ToDomain<Domain.Signal>();

            Type signalType = signal.DataType.GetNativeType();

            if (signalType == typeof(bool))
            { return GetDataWithType<bool>(signal, fromIncludedUtc, toExcludedUtc); }
            else
            if (signalType == typeof(int))
            { return GetDataWithType<int>(signal, fromIncludedUtc, toExcludedUtc); }
            else
            if (signalType == typeof(double))
            { return GetDataWithType<double>(signal, fromIncludedUtc, toExcludedUtc); }
            else
            if (signalType == typeof(decimal))
            { return GetDataWithType<decimal>(signal, fromIncludedUtc, toExcludedUtc); }
            else
            if (signalType == typeof(string))
            { return GetDataWithType<string>(signal, fromIncludedUtc, toExcludedUtc); }
            else return null;
            
        }
        private IEnumerable<Dto.Datum> GetDataWithType<T>(Domain.Signal signal, DateTime fromIncludedUtc, DateTime toExcludedUtc)
        {
            IEnumerable<Domain.Datum<T>> result = signalsDomainService.GetData<T>(signal, fromIncludedUtc, toExcludedUtc).ToArray();

            var policy = this.signalsDomainService.GetMissingValuePolicy(signal.Id.Value) as Domain.MissingValuePolicy.MissingValuePolicy<T>;

            if(policy != null)
                result = policy.SetMissingValue(signal, result, fromIncludedUtc, toExcludedUtc);

            result = result.OrderBy(dat => dat.Timestamp).ToArray();

            return result.ToDto<IEnumerable<Dto.Datum>>();
        }
        
        

        public void SetData(int signalId, IEnumerable<Datum> dataDto)
        {
            Domain.Signal signal = this.GetById(signalId)?.ToDomain<Domain.Signal>();

            if (signal == null) { throw new ArgumentException(); }

            Type signalType = signal.DataType.GetNativeType();
            if (signalType == typeof(bool))   { SetDataWithType<bool>   (signal, dataDto);}
            if (signalType == typeof(int))    { SetDataWithType<int>    (signal, dataDto);}
            if (signalType == typeof(double)) { SetDataWithType<double> (signal, dataDto);}
            if (signalType == typeof(decimal)){ SetDataWithType<decimal>(signal, dataDto);}
            if (signalType == typeof(string)) { SetDataWithType<string> (signal, dataDto);}
        }
        private void SetDataWithType<T>(Domain.Signal signal, IEnumerable<Datum> dataDto)
        {
            IEnumerable<Domain.Datum<T>> dataDomain = dataDto.ToDomain<IEnumerable<Domain.Datum<T>>>();
            dataDomain = FillDatum<T>(signal, dataDomain);
            this.signalsDomainService.SetData<T>(dataDomain);
        }
        private IEnumerable<Domain.Datum<T>> FillDatum<T>(Domain.Signal signal, IEnumerable<Domain.Datum<T>> dataDomain)
        {
            List<Domain.Datum<T>> result = new List<Domain.Datum<T>>();
            foreach (var d in dataDomain)
            {
                var temp = d;
                temp.Signal = signal;
                result.Add(temp);
            }
            return result;
        }

        public MissingValuePolicy GetMissingValuePolicy(int signalId)
        {
            var result = this.signalsDomainService.GetMissingValuePolicy(signalId);
            return result?.ToDto<Dto.MissingValuePolicy.MissingValuePolicy>();
        }

        public void SetMissingValuePolicy(int signalId, MissingValuePolicy policy)
        {
            this.signalsDomainService.SetMissingValuePolicy(signalId, policy.ToDomain<Domain.MissingValuePolicy.MissingValuePolicyBase>());
        }

        public void SetMissingValuePolicy(MissingValuePolicy policy)
        {
            this.signalsDomainService.SetMissingValuePolicy(policy.ToDomain<Domain.MissingValuePolicy.MissingValuePolicyBase>());
        }
    }
}
