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
            throw new NotImplementedException();
        }

        public IEnumerable<Datum> GetData(int signalId, DateTime fromIncludedUtc, DateTime toExcludedUtc)
        {
            Signal signal = GetById(signalId);
            if (signal == null)
                throw new SignalIsNullException();

            return null;
        }

        public void SetData(int signalId, IEnumerable<Datum> data)
        {
            Signal signal = GetById(signalId);
            if (signal == null)
                throw new SignalIsNullException();

            switch (signal.DataType)
            {
                case Dto.DataType.Double:
                    this.signalsDomainService.SetData(signalId,
                        data.ToDomain<IEnumerable<Domain.Datum<Double>>>());
                    break;
                case Dto.DataType.Integer:
                    this.signalsDomainService.SetData(signalId,
                        data.ToDomain<IEnumerable<Domain.Datum<Int32>>>());
                    break;
                case Dto.DataType.Boolean:
                    this.signalsDomainService.SetData(signalId,
                        data.ToDomain<IEnumerable<Domain.Datum<Boolean>>>());
                    break;
                case Dto.DataType.Decimal:
                    this.signalsDomainService.SetData(signalId,
                        data.ToDomain<IEnumerable<Domain.Datum<Decimal>>>());
                    break;
                case Dto.DataType.String:
                    this.signalsDomainService.SetData(signalId,
                        data.ToDomain<IEnumerable<Domain.Datum<String>>>());
                    break;
            }
        }

        public MissingValuePolicy GetMissingValuePolicy(int signalId)
        {
            throw new NotImplementedException();
        }

        public void SetMissingValuePolicy(int signalId, MissingValuePolicy policy)
        {
            throw new NotImplementedException();
        }
    }
}
