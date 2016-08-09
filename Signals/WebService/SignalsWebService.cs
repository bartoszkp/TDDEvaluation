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
            Domain.Path domainPath = pathDto.ToDomain<Domain.Path>();
            return signalsDomainService.GetByPath(domainPath).ToDto<Dto.Signal>();
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
            Signal foundSignal = GetById(signalId);
            if (foundSignal == null)
                throw new SignalNotExistException();

            Domain.Signal signal = foundSignal.ToDomain<Domain.Signal>();

            switch (foundSignal.DataType)
            {
                case Dto.DataType.Double:
                    return signalsDomainService.GetData<Double>(signal, fromIncludedUtc, toExcludedUtc)
                             .ToArray().ToDto<IEnumerable<Dto.Datum>>();
                case Dto.DataType.Integer:
                    return signalsDomainService.GetData<Int32>(signal, fromIncludedUtc, toExcludedUtc)
                             .ToArray().ToDto<IEnumerable<Dto.Datum>>();
                case Dto.DataType.Boolean:
                    return signalsDomainService.GetData<Boolean>(signal, fromIncludedUtc, toExcludedUtc)
                             .ToArray().ToDto<IEnumerable<Dto.Datum>>();
                case Dto.DataType.Decimal:
                    return signalsDomainService.GetData<Decimal>(signal, fromIncludedUtc, toExcludedUtc)
                             .ToArray().ToDto<IEnumerable<Dto.Datum>>();
                case Dto.DataType.String:
                    return signalsDomainService.GetData<String>(signal, fromIncludedUtc, toExcludedUtc)
                             .ToArray().ToDto<IEnumerable<Dto.Datum>>();
            }
            return null;
        }

        public void SetData(int signalId, IEnumerable<Dto.Datum> data)
        {
            Signal foundSignal = GetById(signalId);
            if (foundSignal == null)
                throw new SignalNotExistException();

            Domain.Signal domainSignal = foundSignal.ToDomain<Domain.Signal>();

            switch (foundSignal.DataType)
            {
                case Dto.DataType.Double:
                    signalsDomainService.SetData(domainSignal, data.ToDomain<IEnumerable<Domain.Datum<Double>>>());
                    break;
                case Dto.DataType.Boolean:
                    signalsDomainService.SetData(domainSignal, data.ToDomain<IEnumerable<Domain.Datum<Boolean>>>());
                    break;
                case Dto.DataType.Decimal:
                    signalsDomainService.SetData(domainSignal, data.ToDomain<IEnumerable<Domain.Datum<Decimal>>>());
                    break;
                case Dto.DataType.Integer:
                    signalsDomainService.SetData(domainSignal, data.ToDomain<IEnumerable<Domain.Datum<Int32>>>());
                    break;
                case Dto.DataType.String:
                    signalsDomainService.SetData(domainSignal, data.ToDomain<IEnumerable<Domain.Datum<String>>>());
                    break;
            }

        }

        public MissingValuePolicy GetMissingValuePolicy(int signalId)
        {
            return signalsDomainService.GetMissingValuePolicy(signalId)?.ToDto<Dto.MissingValuePolicy.MissingValuePolicy>();
        }

        public void SetMissingValuePolicy(int signalId, MissingValuePolicy policy)
        {
            var domainMvp = policy.ToDomain<Domain.MissingValuePolicy.SpecificValueMissingValuePolicy<object>>();

            signalsDomainService.SetMissingValuePolicy(signalId, domainMvp);
        }
    }
}
