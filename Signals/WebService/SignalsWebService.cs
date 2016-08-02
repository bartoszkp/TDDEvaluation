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
using Mapster;
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
            var domainPath = pathDto?.ToDomain<Domain.Path>();

            var result = signalsDomainService.GetByPath(domainPath);

            return result.ToDto<Dto.Signal>();
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

        public IEnumerable<Datum> GetSignalData(int signalId, DateTime fromIncludedUtc, DateTime toExcludedUtc)
        {
            var signal = GetById(signalId);

            if (signal == null)
                throw new NoSuchSignalException("Could not get data for not existing signal");

            var domainSignal = signal.ToDomain<Domain.Signal>();


            switch (signal.DataType)
            {
                case DataType.Boolean:
                    return signalsDomainService.GetData<bool>(domainSignal, fromIncludedUtc, toExcludedUtc)
                        ?.ToDto<IEnumerable<Dto.Datum>>();

                case DataType.Integer:
                    return signalsDomainService.GetData<int>(domainSignal, fromIncludedUtc, toExcludedUtc)
                        ?.ToDto<IEnumerable<Dto.Datum>>();

                case DataType.Double:
                    return signalsDomainService.GetData<double>(domainSignal, fromIncludedUtc, toExcludedUtc)
                        ?.ToDto<IEnumerable<Dto.Datum>>();

                case DataType.Decimal:
                    return signalsDomainService.GetData<decimal>(domainSignal, fromIncludedUtc, toExcludedUtc)
                        ?.ToDto<IEnumerable<Dto.Datum>>();

                case DataType.String:
                    return signalsDomainService.GetData<string>(domainSignal, fromIncludedUtc, toExcludedUtc)
                        ?.ToDto<IEnumerable<Dto.Datum>>();
                default:
                    return null;

            }

        }

        public void SetSignalData(int signalId, IEnumerable<Datum> data)
        {
            var signal = GetById(signalId);

            if (signal == null)
                throw new NoSuchSignalException("Could not get data for not existing signal");
        }

        public MissingValuePolicy GetMissingValuePolicy(int signalId)
        {
            var mvp = this.signalsDomainService.GetMissingValuePolicy(signalId);

            if (mvp == null) return null;


            return mvp.ToDto<Dto.MissingValuePolicy.SpecificValueMissingValuePolicy>();


        }

        public void SetMissingValuePolicy(int signalId, MissingValuePolicy policy)
        {
            if (policy == null)
                throw new ArgumentNullException();

            var domainPolicy = policy.ToDomain<Domain.MissingValuePolicy.MissingValuePolicyBase>();
            signalsDomainService.SetMissingValuePolicy(signalId, domainPolicy);
        }
    }
}
