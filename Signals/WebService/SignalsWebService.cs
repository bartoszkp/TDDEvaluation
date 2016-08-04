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
            throw new NotImplementedException();
        }

        public void SetData(int signalId, IEnumerable<Datum> dataDto)
        {
            Domain.Signal signal = signalsDomainService.GetById(signalId);
            Type type = signal.DataType.GetNativeType();
            if (type == typeof(bool))
            {
                var dataDomain = dataDto.ToDomain<IEnumerable<Domain.Datum<Boolean>>>();
                dataDomain = FillDatum<bool>(signal, dataDomain);
                this.signalsDomainService.SetData<Boolean>(dataDomain);
            }
            if (type == typeof(int))
            {
                var dataDomain = dataDto.ToDomain<IEnumerable<Domain.Datum<int>>>();
                this.signalsDomainService.SetData<int>(dataDomain);
            }
            if (type == typeof(double))
            {
                var dataDomain = dataDto.ToDomain<IEnumerable<Domain.Datum<double>>>();
                dataDomain = FillDatum<double>(signal, dataDomain);
                this.signalsDomainService.SetData<double>(dataDomain);
            }
            if (type == typeof(decimal))
            {
                var dataDomain = dataDto.ToDomain<IEnumerable<Domain.Datum<decimal>>>();
                this.signalsDomainService.SetData<decimal>(dataDomain);
            }
            if (type == typeof(string))
            {
                var dataDomain = dataDto.ToDomain<IEnumerable<Domain.Datum<string>>>();
                this.signalsDomainService.SetData<string>(dataDomain);
            }
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
    }
}
