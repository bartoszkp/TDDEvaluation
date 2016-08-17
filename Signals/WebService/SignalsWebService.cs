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
using Domain.MissingValuePolicy;
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
            var pathDomain = pathDto.ToDomain<Domain.Path>();

            var result = this.signalsDomainService.Get(pathDomain);

            return result.ToDto<Signal>();
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
            var signal = this.signalsDomainService.GetById(signalId);

            if (signal == null)
                throw new InvalidOperationException("Signal dosen't exist");

            switch (signal.DataType)
            {
                case Domain.DataType.Boolean:
                    {
                        var item = signalsDomainService.GetData<bool>(signalId, fromIncludedUtc, toExcludedUtc)
                            .OrderBy(x => x.Timestamp).ToList();

                        return item?.ToDto<IEnumerable<Dto.Datum>>();
                    }
                case Domain.DataType.Decimal:
                    {
                        var item = signalsDomainService.GetData<decimal>(signalId, fromIncludedUtc, toExcludedUtc)
                            .OrderBy(x => x.Timestamp).ToList();

                        return item?.ToDto<IEnumerable<Dto.Datum>>();
                    }
                case Domain.DataType.Double:
                    {
                        var item = signalsDomainService.GetData<double>(signalId, fromIncludedUtc, toExcludedUtc)
                            .OrderBy(x => x.Timestamp).ToList();

                        return item?.ToDto<IEnumerable<Dto.Datum>>();
                    }
                case Domain.DataType.Integer:
                    {
                        var item = signalsDomainService.GetData<int>(signalId, fromIncludedUtc, toExcludedUtc)
                            .ToList().OrderBy(x => x.Timestamp);

                        return item?.ToDto<IEnumerable<Dto.Datum>>();
                    }
                case Domain.DataType.String:
                    {
                        var item = signalsDomainService.GetData<string>(signalId, fromIncludedUtc, toExcludedUtc)
                            .OrderBy(x => x.Timestamp).ToList();

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
                throw new IdNotNullException();

            if (data.First().Value.GetType() == typeof(int))
                this.signalsDomainService.SetData(signal, data.ToDomain<IEnumerable<Domain.Datum<int>>>());
            else if (data.First().Value.GetType() == typeof(double))
                this.signalsDomainService.SetData(signal, data.ToDomain<IEnumerable<Domain.Datum<double>>>());
            else if (data.First().Value.GetType() == typeof(bool))
                this.signalsDomainService.SetData(signal, data.ToDomain<IEnumerable<Domain.Datum<bool>>>());
            else if (data.First().Value.GetType() == typeof(string))
                this.signalsDomainService.SetData(signal, data.ToDomain<IEnumerable<Domain.Datum<string>>>());
            else if (data.First().Value.GetType() == typeof(decimal))
                this.signalsDomainService.SetData(signal, data.ToDomain<IEnumerable<Domain.Datum<decimal>>>());
                
        }

        public MissingValuePolicy GetMissingValuePolicy(int signalId)
        {
            var signal = this.signalsDomainService.GetById(signalId);
            if (signal == null)
                throw new SignalIsNullException();

            return this.signalsDomainService.GetMissingValuePolicy(signal).ToDto<MissingValuePolicy>();
        }

        public void SetMissingValuePolicy(int signalId, MissingValuePolicy mvp)
        {
            var missingValuePolicy = mvp.ToDomain<Domain.MissingValuePolicy.MissingValuePolicyBase>();
            var signal = signalsDomainService.GetById(signalId);
            if(signal == null)
            {
                throw new SignalIsNullException();
            }
            this.signalsDomainService?.SetMissingValuePolicy(signal, missingValuePolicy);
        }
    }
}
