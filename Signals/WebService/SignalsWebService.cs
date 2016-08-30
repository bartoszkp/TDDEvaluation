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
            return signalsDomainService
                ?.Get(pathDto.ToDomain<Domain.Path>())
                .ToDto<Dto.Signal>();
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
            signalsDomainService.Delete(signalId);
        }

        public PathEntry GetPathEntry(Path pathDto)
        {
            return signalsDomainService?.GetPathEntry(pathDto.ToDomain<Domain.Path>())?.ToDto<Dto.PathEntry>();
        }

        public IEnumerable<Datum> GetData(int signalId, DateTime fromIncludedUtc, DateTime toExcludedUtc)
        {
            var signal = this.signalsDomainService?.GetById(signalId);
            if(fromIncludedUtc == null)
            {
                throw new NotImplementedException();
            }
            if(signal == null)
            {
                throw new KeyNotFoundException();
            }
            switch (signal.DataType)
            {
                case Domain.DataType.Boolean:
                    return signalsDomainService.GetData<bool>(signal, fromIncludedUtc, toExcludedUtc)
                        .Select(d => d.ToDto<Dto.Datum>()).ToList().OrderBy(t => t.Timestamp);
                case Domain.DataType.Decimal:
                    return signalsDomainService.GetData<decimal>(signal, fromIncludedUtc, toExcludedUtc)
                        .Select(d => d.ToDto<Dto.Datum>()).ToList().OrderBy(t => t.Timestamp);
                case Domain.DataType.Double:
                    return signalsDomainService.GetData<double>(signal, fromIncludedUtc, toExcludedUtc)
                        .Select(d => d.ToDto<Dto.Datum>()).ToList().OrderBy(t => t.Timestamp);
                case Domain.DataType.Integer:
                    return signalsDomainService.GetData<int>(signal, fromIncludedUtc, toExcludedUtc)
                        .Select(d => d.ToDto<Dto.Datum>()).ToList().OrderBy(t => t.Timestamp);
                case Domain.DataType.String:
                    return signalsDomainService.GetData<string>(signal, fromIncludedUtc, toExcludedUtc)
                        .Select(d => d.ToDto<Dto.Datum>()).ToList().OrderBy(t => t.Timestamp);
                default:
                    throw new KeyNotFoundException();
            }   
        }

        public void SetData(int signalId, IEnumerable<Datum> data)
        {
            var signal = this.signalsDomainService?.GetById(signalId);
            
            if(signal == null)
            {
                throw new KeyNotFoundException();
            }
            if (data.Count() == 0)
                return;

            if (data.First().Value == null)
            {
                this.signalsDomainService?.SetData(signal, data?.ToDomain<IEnumerable<Domain.Datum<string>>>());
                return;
            }
            else if (data.First().Value.GetType() == typeof(int))
            {
                this.signalsDomainService?.SetData(signal, data?.ToDomain<IEnumerable<Domain.Datum<int>>>());
                return;
            }
            else if (data.First().Value.GetType() == typeof(double))
            {
                this.signalsDomainService?.SetData(signal, data?.ToDomain<IEnumerable<Domain.Datum<double>>>());
                return;
            }
            else if(data.First().Value.GetType() == typeof(bool))
            {
                this.signalsDomainService?.SetData(signal, data?.ToDomain<IEnumerable<Domain.Datum<bool>>>());
                return;
            }
            else if(data.First().Value.GetType() == typeof(string))
            {
                this.signalsDomainService?.SetData(signal, data?.ToDomain<IEnumerable<Domain.Datum<string>>>());
                return;
            }
            else if (data.First().Value.GetType() == typeof(decimal))
            {
                this.signalsDomainService?.SetData(signal, data?.ToDomain<IEnumerable<Domain.Datum<decimal>>>());
                return;
            }
        }

        public MissingValuePolicy GetMissingValuePolicy(int signalId)
        {
            var signal = signalsDomainService?.GetById(signalId);

            if(signal == null)
            {
                throw new KeyNotFoundException();
            }

            return this.signalsDomainService
                ?.GetMissingValuePolicy(signal)
                ?.ToDto<MissingValuePolicy>();
        }

        public void SetMissingValuePolicy(int signalId, MissingValuePolicy policy)
        {
            var mvp = policy?.ToDomain<Domain.MissingValuePolicy.MissingValuePolicyBase>();
            
            var signal = signalsDomainService?.GetById(signalId);
            
            if(signal == null)
            {
                throw new KeyNotFoundException();
            }

            this.signalsDomainService?.SetMissingValuePolicy(
                signal,
                mvp);
        }
    }
}
