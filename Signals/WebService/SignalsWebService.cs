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
            if (pathDto != null)
                return this.signalsDomainService.Get(pathDto.ToDomain<Domain.Path>())
                    ?.ToDto<Dto.Signal>();

            return null;
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
            var signal = signalsDomainService.GetById(signalId);

            if (signal == null)
                throw new NoSuchSignalException();

            switch (signal.DataType)
            {
                case Domain.DataType.Boolean:
                    {
                        var item = signalsDomainService.GetData<bool>(signal, fromIncludedUtc, toExcludedUtc)
                            .ToList();
                        return item?.ToDto<IEnumerable<Dto.Datum>>();
                    }
                case Domain.DataType.Decimal:
                    {
                        var item = signalsDomainService.GetData<decimal>(signal, fromIncludedUtc, toExcludedUtc)
                            .ToList();
                        return item?.ToDto<IEnumerable<Dto.Datum>>();
                    }
                case Domain.DataType.Double:
                    {
                        var item = signalsDomainService.GetData<double>(signal, fromIncludedUtc, toExcludedUtc)
                            .ToList();
                        return item?.ToDto<IEnumerable<Dto.Datum>>();
                    }
                case Domain.DataType.Integer:
                    {
                        var item = signalsDomainService.GetData<int>(signal, fromIncludedUtc, toExcludedUtc)
                            .ToList();
                        return item?.ToDto<IEnumerable<Dto.Datum>>();
                    }
                case Domain.DataType.String:
                    {
                        var item = signalsDomainService.GetData<string>(signal, fromIncludedUtc, toExcludedUtc)
                            .ToList();
                        return item?.ToDto<IEnumerable<Dto.Datum>>();
                    }
                default:
                    {
                        return null;
                    }
            }
        }

        public void SetData(int signalId, IEnumerable<Datum> data)
        {
            var signal = signalsDomainService.GetById(signalId);

            if (signal == null)
                throw new NoSuchSignalException();


            switch (signal.DataType)
            {
                case Domain.DataType.Decimal:
                    {
                        var domainModel = data.ToDomain<IEnumerable<Domain.Datum<decimal>>>()
                            .ToList();
                        setSignalToData(domainModel, signal);
                        signalsDomainService.SetData<decimal>(domainModel);
                        break;
                    }
                case Domain.DataType.Boolean:
                    {
                        var domainModel = data.ToDomain<IEnumerable<Domain.Datum<bool>>>()
                            .ToList();
                        setSignalToData(domainModel, signal);
                        signalsDomainService.SetData<bool>(domainModel);
                        break;
                    }
                case Domain.DataType.Double:
                    {
                        var domainModel = data.ToDomain<IEnumerable<Domain.Datum<double>>>()
                            .ToList();
                        setSignalToData(domainModel, signal);
                        signalsDomainService.SetData<double>(domainModel);
                        break;
                    }
                case Domain.DataType.Integer:
                    {
                        var domainModel = data.ToDomain<IEnumerable<Domain.Datum<int>>>()
                            .ToList();
                        setSignalToData(domainModel, signal);

                        signalsDomainService.SetData<int>(domainModel);
                        break;
                    }
                case Domain.DataType.String:
                    {
                        var domainModel = data.ToDomain<IEnumerable<Domain.Datum<string>>>()
                            .ToList();

                        setSignalToData(domainModel, signal);

                        signalsDomainService.SetData<string>(domainModel);
                        break;
                    }
            }
        }

        public MissingValuePolicy GetMissingValuePolicy(int signalId)
        {
            var item = signalsDomainService.GetMissingValuePolicy(signalId);
            return item?.ToDto<MissingValuePolicy>();
        }

        public void SetMissingValuePolicy(int signalId, MissingValuePolicy policy)
        {
            var domianPolicy = policy.ToDomain<Domain.MissingValuePolicy.MissingValuePolicyBase>();
            this.signalsDomainService.SetMissingValuePolicy(signalId, domianPolicy);
        }

        private void setSignalToData<T>(IEnumerable<Domain.Datum<T>> data, Domain.Signal signal)
        {
            foreach (var item in data)
            {
                item.Signal = signal;
            }
        }

    }
}
