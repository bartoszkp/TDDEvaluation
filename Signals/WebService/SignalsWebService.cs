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
                Domain.Path path = pathDto.ToDomain<Domain.Path>();
                var result = this.signalsDomainService.Get(path);
                return result.ToDto<Dto.Signal>();
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
            var signal = this.signalsDomainService?.GetById(signalId);
            if (signal == null)
            {
                throw new KeyNotFoundException();
            }
            else if (signal.DataType.GetNativeType() == typeof(int))
            {
                return this.signalsDomainService?
                    .GetData<int>(signal, fromIncludedUtc, toExcludedUtc)?.
                    ToDto<IEnumerable<Dto.Datum>>();
            }
            else if (signal.DataType.GetNativeType() == typeof(double))
            {
                return this.signalsDomainService?
                    .GetData<double>(signal, fromIncludedUtc, toExcludedUtc)?.
                    ToDto<IEnumerable<Dto.Datum>>();
            }
            else if (signal.DataType.GetNativeType() == typeof(bool))
            {
                return this.signalsDomainService?
                    .GetData<bool>(signal, fromIncludedUtc, toExcludedUtc)?.
                    ToDto<IEnumerable<Dto.Datum>>();
            }
            else if (signal.DataType.GetNativeType() == typeof(decimal))
            {
                return this.signalsDomainService?
                    .GetData<decimal>(signal, fromIncludedUtc, toExcludedUtc)?.
                    ToDto<IEnumerable<Dto.Datum>>();
            }
            else if (signal.DataType.GetNativeType() == typeof(string))
            {
                return this.signalsDomainService?
                    .GetData<string>(signal, fromIncludedUtc, toExcludedUtc)?.
                    ToDto<IEnumerable<Dto.Datum>>();
            }
            return null;

        }

        public void SetData(int signalId, IEnumerable<Datum> data)
        {
            var signal = this.signalsDomainService.GetById(signalId);

            if (signal == null)
            {
                throw new KeyNotFoundException();
            }
            else if (signal.DataType.GetNativeType() == typeof(int))
            {
                this.signalsDomainService?.SetData(signal, data?.ToDomain<IEnumerable<Domain.Datum<int>>>().ToArray());
                return;
            }
            else if (signal.DataType.GetNativeType() == typeof(double))
            {
                this.signalsDomainService?.SetData(signal, data?.ToDomain<IEnumerable<Domain.Datum<double>>>().ToArray());
                return;
            }
            else if (signal.DataType.GetNativeType() == typeof(bool))
            {
                this.signalsDomainService?.SetData(signal, data?.ToDomain<IEnumerable<Domain.Datum<bool>>>().ToArray());
                return;
            }
            else if (signal.DataType.GetNativeType() == typeof(decimal))
            {
                this.signalsDomainService?.SetData(signal, data?.ToDomain<IEnumerable<Domain.Datum<decimal>>>().ToArray());
                return;
            }
            else if (signal.DataType.GetNativeType() == typeof(string))
            {
                this.signalsDomainService?.SetData(signal, data?.ToDomain<IEnumerable<Domain.Datum<string>>>().ToArray());
                return;
            }
        }

        public MissingValuePolicy GetMissingValuePolicy(int signalId)
        {
            var signalDomain = this.signalsDomainService.GetById(signalId);
           return this.signalsDomainService.GetMissingValuePolicy(signalDomain).ToDto<MissingValuePolicy>();
        }

        public void SetMissingValuePolicy(int signalId, MissingValuePolicy policy)
        {
            var domainPolicyBase = policy.ToDomain<Domain.MissingValuePolicy.MissingValuePolicyBase>();
            this.signalsDomainService.SetMissingValuePolicy(signalId, domainPolicyBase);
        }
    }
}
