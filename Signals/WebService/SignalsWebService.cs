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
            return signalsDomainService.Get(pathDto.ToDomain<Domain.Path>())?.ToDto<Dto.Signal>();
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

            var result = signalsDomainService.GetPathEntry(pathDomain);

            return result.ToDto<Dto.PathEntry>();
        }

        public IEnumerable<Datum> GetData(int signalId, DateTime fromIncludedUtc, DateTime toExcludedUtc)
        {
            var signal = GetById(signalId).ToDomain<Domain.Signal>();
            if (signal == null)
            {
                throw new CouldntGetASignalException();
            }
            try
            {
                return typeof(Domain.Services.Implementation.SignalsDomainService)
                .GetMethod("GetData")
                .MakeGenericMethod(Domain.Infrastructure.DataTypeUtils.GetNativeType(signal.DataType))
                .Invoke(signalsDomainService, new object[] { signal, fromIncludedUtc, toExcludedUtc })
                ?.ToDto<IEnumerable<Dto.Datum>>();
            }
            catch(TargetInvocationException e)
            {
                throw e.InnerException;
            }
        }

        public IEnumerable<Datum> GetCoarseData(int signalId, Granularity granularity, DateTime fromIncludedUtc, DateTime toExcludedUtc)
        {
            var signal = signalsDomainService.GetById(signalId);
            if (signal == null)
            {
                throw new CouldntGetASignalException();
            }
            
            object data;
            try
            {
                data = typeof(Domain.Services.Implementation.SignalsDomainService).
                GetMethod("GetCoarseData").
                MakeGenericMethod(DataTypeUtils.GetNativeType(signal.DataType)).
                Invoke(signalsDomainService, new object[] { signal, granularity.ToDomain<Domain.Granularity>(), fromIncludedUtc, toExcludedUtc });
            }
            catch(System.Reflection.TargetInvocationException e)
            {
                throw e.InnerException;
            }

            return data.ToDto<IEnumerable<Dto.Datum>>();
        }

        public void SetData(int signalId, IEnumerable<Datum> data)
        {
            var signal = GetById(signalId).ToDomain<Domain.Signal>();
            if (signal == null)
            {
                throw new CouldntGetASignalException();
            }

            switch(signal.DataType) {
                case Domain.DataType.Integer:
                    signalsDomainService.SetData<int>(signal, data.ToDomain<IEnumerable<DataAccess.GenericInstantiations.DatumInteger>>().ToArray());
                    return;
                case Domain.DataType.Double:
                    signalsDomainService.SetData<double>(signal, data.ToDomain<IEnumerable<DataAccess.GenericInstantiations.DatumDouble>>().ToArray());
                    return;
                case Domain.DataType.Decimal:
                    signalsDomainService.SetData<decimal>(signal, data.ToDomain<IEnumerable<DataAccess.GenericInstantiations.DatumDecimal>>().ToArray());
                    return;
                case Domain.DataType.Boolean:
                    signalsDomainService.SetData<bool>(signal, data.ToDomain<IEnumerable<DataAccess.GenericInstantiations.DatumBoolean>>().ToArray());
                    return;
                case Domain.DataType.String:
                    signalsDomainService.SetData<string>(signal, data.ToDomain<IEnumerable<DataAccess.GenericInstantiations.DatumString>>().ToArray());
                    return;
                default:
                    throw new InvalidDataTypeException();
            }
        }

        public MissingValuePolicy GetMissingValuePolicy(int signalId)
        {
            var signal = GetById(signalId).ToDomain<Domain.Signal>();
            var result = signalsDomainService.GetMissingValuePolicy(signal);
            return result?.ToDto<Dto.MissingValuePolicy.MissingValuePolicy>();
        }

        public void SetMissingValuePolicy(int signalId, MissingValuePolicy policy)
        {
            signalsDomainService.SetMissingValuePolicy(GetById(signalId)?.ToDomain<Domain.Signal>(), policy?.ToDomain<Domain.MissingValuePolicy.MissingValuePolicyBase>());
        }
    }
}
