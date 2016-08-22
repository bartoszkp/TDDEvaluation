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
            return signalsDomainService.GetByPath(pathDto.ToDomain<Domain.Path>()).ToDto<Dto.Signal>();
        }

        public Signal GetById(int signalId)
        {
            return signalsDomainService.GetById(signalId).ToDto<Dto.Signal>();
        }

        public Signal Add(Signal signalDto)
        {
            var signal = signalDto.ToDomain<Domain.Signal>();

            var result = signalsDomainService.Add(signal);

            return result.ToDto<Dto.Signal>();
        }

        public void Delete(int signalId)
        {
            throw new NotImplementedException();
        }

        public PathEntry GetPathEntry(Path pathDto)
        {
            return this.signalsDomainService.GetPathEntry(pathDto.ToDomain<Domain.Path>()).ToDto<Dto.PathEntry>();
        }

        public IEnumerable<Datum> GetData(int signalId, DateTime fromIncludedUtc, DateTime toExcludedUtc)
        {
            Signal signal = GetById(signalId);
            if (signal == null)
                throw new CouldntGetASignalException();

            switch (signal.DataType)
            {
                case Dto.DataType.Double:
                    return this.signalsDomainService.GetData<Double>(signalId, fromIncludedUtc, toExcludedUtc)
                        .ToArray().ToDto<IEnumerable<Dto.Datum>>();
                case Dto.DataType.Integer:
                    return this.signalsDomainService.GetData<Int32>(signalId, fromIncludedUtc, toExcludedUtc)
                        .ToArray().ToDto<IEnumerable<Dto.Datum>>();
                case Dto.DataType.Boolean:
                    return this.signalsDomainService.GetData<Boolean>(signalId, fromIncludedUtc, toExcludedUtc)
                        .ToArray().ToDto<IEnumerable<Dto.Datum>>();
                case Dto.DataType.Decimal:
                    return this.signalsDomainService.GetData<Decimal>(signalId, fromIncludedUtc, toExcludedUtc)
                        .ToArray().ToDto<IEnumerable<Dto.Datum>>();
                case Dto.DataType.String:
                    return this.signalsDomainService.GetData<String>(signalId, fromIncludedUtc, toExcludedUtc)
                        .ToArray().ToDto<IEnumerable<Dto.Datum>>();
            }

            return null;
        }

        public void SetData(int signalId, IEnumerable<Datum> data)
        {
            Signal signal = GetById(signalId);
            if (signal == null)
                throw new CouldntGetASignalException();

           if(signal.Granularity==Granularity.Month)
                foreach (var i in data)
                {
                    if (i.Timestamp.Day != 1||i.Timestamp.Hour!=0||i.Timestamp.Minute!=0||i.Timestamp.Millisecond!=0)
                        throw new ArgumentException();
                }
            if (signal.Granularity == Granularity.Day)
                foreach (var i in data)
                {
                    if ( i.Timestamp.Hour != 0 || i.Timestamp.Minute != 0 || i.Timestamp.Millisecond != 0)
                        throw new ArgumentException();
                }


            switch (signal.DataType)
            {
                case Dto.DataType.Double:
                    this.signalsDomainService.SetData(signal.ToDomain<Domain.Signal>(),
                        data.ToDomain<IEnumerable<Domain.Datum<Double>>>());
                    break;
                case Dto.DataType.Integer:
                    this.signalsDomainService.SetData(signal.ToDomain<Domain.Signal>(),
                        data.ToDomain<IEnumerable<Domain.Datum<Int32>>>());
                    break;
                case Dto.DataType.Boolean:
                    this.signalsDomainService.SetData(signal.ToDomain<Domain.Signal>(),
                        data.ToDomain<IEnumerable<Domain.Datum<Boolean>>>());
                    break;
                case Dto.DataType.Decimal:
                    this.signalsDomainService.SetData(signal.ToDomain<Domain.Signal>(),
                        data.ToDomain<IEnumerable<Domain.Datum<Decimal>>>());
                    break;
                case Dto.DataType.String:
                    this.signalsDomainService.SetData(signal.ToDomain<Domain.Signal>(),
                        data.ToDomain<IEnumerable<Domain.Datum<String>>>());
                    break;
            }
        }

        public MissingValuePolicy GetMissingValuePolicy(int signalId)
        {
            Domain.Signal signal = this.signalsDomainService.GetById(signalId);
            if (signal == null)
                throw new CouldntGetASignalException();

            return this.signalsDomainService.GetMissingValuePolicy(signal)
                .ToDto<Dto.MissingValuePolicy.MissingValuePolicy>();
        }

        public void SetMissingValuePolicy(int signalId, MissingValuePolicy policy)
        {
            Domain.Signal signal = this.signalsDomainService.GetById(signalId);
            if (signal == null)
                throw new CouldntGetASignalException();

            this.signalsDomainService.SetMissingValuePolicy(
                signal.ToDomain<Domain.Signal>(),
                policy.ToDomain<Domain.MissingValuePolicy.MissingValuePolicyBase>());
        }
    }
}
