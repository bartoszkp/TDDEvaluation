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
            if (pathDto != null)
                return this.signalsDomainService.Get(pathDto.ToDomain<Domain.Path>())
                    ?.ToDto<Dto.Signal>();
            else
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
                throw new InvalidCastException("Signal dosen't exist");

            switch(signal.DataType)
            {
                case Domain.DataType.Boolean:
                    {
                        var item = signalsDomainService.GetData<bool>(signal, fromIncludedUtc, toExcludedUtc)
                            .ToList();
                        return item.ToDto<IEnumerable<Dto.Datum>>();
                    }
                case Domain.DataType.Decimal:
                    {
                        var item = signalsDomainService.GetData<decimal>(signal, fromIncludedUtc, toExcludedUtc)
                            .ToList();
                        return item.ToDto<IEnumerable<Dto.Datum>>();
                    }
                case Domain.DataType.Double:
                    {
                        var item = signalsDomainService.GetData<double>(signal, fromIncludedUtc, toExcludedUtc)
                            .ToList();
                        return item.ToDto<IEnumerable<Dto.Datum>>();
                    }
                case Domain.DataType.Integer:
                    {
                        var item = signalsDomainService.GetData<int>(signal, fromIncludedUtc, toExcludedUtc)
                            .ToList();
                        return item.ToDto<IEnumerable<Dto.Datum>>();
                    }
                case Domain.DataType.String:
                    {
                        var item = signalsDomainService.GetData<string>(signal, fromIncludedUtc, toExcludedUtc)
                            .ToList();
                        return item.ToDto<IEnumerable<Dto.Datum>>();
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

            switch (signal.DataType)
            {
                case Domain.DataType.Decimal:
                    {
                        var domianModel = data.ToDomain<IEnumerable<Domain.Datum<decimal>>>()
                            .ToList();
                        foreach (var item in domianModel)
                        {
                            item.Signal = signal;
                        }
                        signalsDomainService.SetData<decimal>(domianModel);
                        break;
                    }
                case Domain.DataType.Boolean:
                    {
                        var domianModel = data.ToDomain<IEnumerable<Domain.Datum<bool>>>()
                            .ToList();
                        foreach (var item in domianModel)
                        {
                            item.Signal = signal;
                        }
                        signalsDomainService.SetData<bool>(domianModel);
                        break;
                    }
                case Domain.DataType.Double:
                    {
                        var domianModel = data.ToDomain<IEnumerable<Domain.Datum<double>>>()
                            .ToList();
                        foreach (var item in domianModel)
                        {
                            item.Signal = signal;
                        }
                        signalsDomainService.SetData<double>(domianModel);
                        break;
                    }
                case Domain.DataType.Integer:
                    {
                        var domianModel = data.ToDomain<IEnumerable<Domain.Datum<int>>>()
                            .ToList();
                        foreach (var item in domianModel)
                        {
                            item.Signal = signal;
                        }
                        signalsDomainService.SetData<int>(domianModel);
                        break;
                    }
                case Domain.DataType.String:
                    {
                        var domianModel = data.ToDomain<IEnumerable<Domain.Datum<string>>>()
                            .ToList();
                        foreach (var item in domianModel)
                        {
                            item.Signal = signal;
                        }
                        signalsDomainService.SetData<string>(domianModel);
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
            var domianPolicy = policy?.ToDomain<Domain.MissingValuePolicy.MissingValuePolicyBase>();
            this.signalsDomainService.SetMissingValuePolicy(signalId, domianPolicy);
        }
    }
}
