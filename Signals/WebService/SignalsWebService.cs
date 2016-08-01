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
using Domain.MissingValuePolicy;
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
            if (pathDto == null)
                return null;

            return this.signalsDomainService
                .GetByPath(pathDto.ToDomain<Domain.Path>())
                .ToDto<Dto.Signal>();
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
            var dataType = this.signalsDomainService.GetById(signalId)?.DataType;

            if (dataType == null)
                return null;

            if (dataType == Domain.DataType.Boolean)
                return GetData<Boolean>(signalId, fromIncludedUtc, toExcludedUtc);
            else if (dataType == Domain.DataType.Integer)
                return GetData<int>(signalId, fromIncludedUtc, toExcludedUtc);
            else if (dataType == Domain.DataType.Double)
                return GetData<Double>(signalId, fromIncludedUtc, toExcludedUtc);
            else if (dataType == Domain.DataType.Decimal)
                return GetData<Decimal>(signalId, fromIncludedUtc, toExcludedUtc);
            else if (dataType == Domain.DataType.String)
                return GetData<String>(signalId, fromIncludedUtc, toExcludedUtc);

            return null;
        }

        public void SetData(int signalId, IEnumerable<Datum> data)
        {
            var dataType = this.signalsDomainService.GetById(signalId)?.DataType;

            if (dataType == null)
                return;

            if (dataType == Domain.DataType.Boolean)
                SetData<Boolean>(signalId, data);
            else if (dataType == Domain.DataType.Integer)
                SetData<int>(signalId, data);
            else if (dataType == Domain.DataType.Double)
                SetData<Double>(signalId, data);
            else if (dataType == Domain.DataType.Decimal)
                SetData<Decimal>(signalId, data);
            else if (dataType == Domain.DataType.String)
                SetData<String>(signalId, data);
        }

        public MissingValuePolicy GetMissingValuePolicy(int signalId)
        {
            return this.signalsDomainService.GetMissingValuePolicy(signalId)
                ?.ToDto<MissingValuePolicy>();
        }

        public void SetMissingValuePolicy(int signalId, MissingValuePolicy policy)
        {
            var domainPolicy = policy?.ToDomain<MissingValuePolicyBase>();

            this.signalsDomainService.SetMissingValuePolicy(signalId, domainPolicy);
        }

        private IEnumerable<Datum> GetData<T>(int signalId, DateTime fromIncludedUtc, DateTime toExcludedUtc)
        {
            var data = this.signalsDomainService
                .GetData<T>(signalId, fromIncludedUtc, toExcludedUtc)
                .ToList();

            return data.ToDto<IEnumerable<Datum>>();
        }

        private void SetData<T>(int signalId, IEnumerable<Datum> data)
        {
            var domainData = data.ToDomain<IEnumerable<Domain.Datum<T>>>();

            this.signalsDomainService.SetData<T>(signalId, domainData);
        }
    }
}
