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
            Signal signal = GetById(signalId);
            if (signal == null)
                throw new CouldntGetASignalException();

            switch (signal.DataType)
            {
                case Dto.DataType.Double:
                    signalsDomainService.Delete<double>(signalId);
                    break;
                case Dto.DataType.Integer:
                    signalsDomainService.Delete<int>(signalId);
                    break;
                case Dto.DataType.Boolean:
                    signalsDomainService.Delete<bool>(signalId);
                    break;
                case Dto.DataType.Decimal:
                    signalsDomainService.Delete<decimal>(signalId);
                    break;
                case Dto.DataType.String:
                    signalsDomainService.Delete<string>(signalId);
                    break;
            }

            signalsDomainService.Delete<int>(signalId);
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

            checkDate(signal.Granularity, new Datum[] { new Datum() { Timestamp= fromIncludedUtc} , new Datum() { Timestamp = toExcludedUtc } });

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

        public IEnumerable<Datum> GetCoarseData(int signalId, Granularity granularity, DateTime fromIncludedUtc, DateTime toExcludedUtc)
        {
            throw new CouldntGetASignalException();
        }

        public void SetData(int signalId, IEnumerable<Datum> data)
        {
            Signal signal = GetById(signalId);
            if (signal == null)
                throw new CouldntGetASignalException();

         

            checkDate(signal.Granularity, data);
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

        private void checkDate(Granularity granularity, IEnumerable<Datum> data)
        {
            foreach (var i in data)
            {
                switch (granularity)
                {
                    case Granularity.Year:
                        checkSingularDate(i.Timestamp.Millisecond, i.Timestamp.Second, i.Timestamp.Minute, i.Timestamp.Hour, i.Timestamp.Day, i.Timestamp.DayOfYear);
                        break;
                    case Granularity.Month:
                        checkSingularDate(i.Timestamp.Millisecond, i.Timestamp.Second, i.Timestamp.Minute, i.Timestamp.Hour, i.Timestamp.Day);
                        break;
                    case Granularity.Week:
                        if (i.Timestamp.DayOfWeek != DayOfWeek.Monday) throw new ArgumentException();
                        checkSingularDate(i.Timestamp.Millisecond, i.Timestamp.Second, i.Timestamp.Minute, i.Timestamp.Hour);
                        break;
                    case Granularity.Day:
                        checkSingularDate(i.Timestamp.Millisecond, i.Timestamp.Second, i.Timestamp.Minute, i.Timestamp.Hour);
                        break;
                    case Granularity.Hour:
                        checkSingularDate(i.Timestamp.Millisecond, i.Timestamp.Second, i.Timestamp.Minute);
                        break;
                    case Granularity.Minute:
                        checkSingularDate(i.Timestamp.Millisecond, i.Timestamp.Second);
                        break;
                    case Granularity.Second:
                        checkSingularDate(i.Timestamp.Millisecond);
                        break;
                    default:
                        break;
                }

            }
        }
        private void checkSingularDate(int millisecond, int second = 0, int minute = 0, int hour = 0, int day = 1, int dayOfYear = 1)
        {
            if (dayOfYear != 1 || day != 1 || hour != 0 || minute != 0 || second != 0 || millisecond != 0) throw new ArgumentException();
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

            if(policy is Dto.MissingValuePolicy.ShadowMissingValuePolicy)
            {
                var shadowPolicy = policy as Dto.MissingValuePolicy.ShadowMissingValuePolicy;
                if((int)shadowPolicy.ShadowSignal.Granularity != (int)signal.Granularity || (int)shadowPolicy.ShadowSignal.DataType != (int)signal.DataType )
                {
                    throw new ArgumentException("Granuality and DataType of ShadowMissingValuePolicy and signal MUST match");
                }
            }

            this.signalsDomainService.SetMissingValuePolicy(
                signal.ToDomain<Domain.Signal>(),
                policy.ToDomain<Domain.MissingValuePolicy.MissingValuePolicyBase>());
        }
    }
}
