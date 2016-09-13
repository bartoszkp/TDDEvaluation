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
            var path = pathDto.ToDomain<Domain.Path>();

            var result = signalsDomainService.GetByPath(path);

            return result?.ToDto<Dto.Signal>();
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
            this.signalsDomainService.Delete(signalId);
        }

        public PathEntry GetPathEntry(Path pathDto)
        {
            return signalsDomainService.GetPathEntry(pathDto.ToDomain<Domain.Path>()).ToDto<PathEntry>();
        }

        public IEnumerable<Datum> GetData(int signalId, DateTime fromIncludedUtc, DateTime toExcludedUtc)
        {
            var signal = signalsDomainService.GetById(signalId);
            if (signal == null) throw new ArgumentException();

            else
            {
                var type = signal.DataType.ToString();

                if (type == "Boolean") return signalsDomainService.GetData<bool>(signalId, fromIncludedUtc, toExcludedUtc).ToDto<IEnumerable<Dto.Datum>>();
                else if (type == "Integer") return signalsDomainService.GetData<int>(signalId, fromIncludedUtc, toExcludedUtc).ToDto<IEnumerable<Dto.Datum>>();
                else if (type == "Double") return signalsDomainService.GetData<double>(signalId, fromIncludedUtc, toExcludedUtc).ToDto<IEnumerable<Dto.Datum>>();
                else if (type == "Decimal") return signalsDomainService.GetData<decimal>(signalId, fromIncludedUtc, toExcludedUtc).ToDto<IEnumerable<Dto.Datum>>();
                else if (type == "String") return signalsDomainService.GetData<string>(signalId, fromIncludedUtc, toExcludedUtc).ToDto<IEnumerable<Dto.Datum>>();
                else throw new ArgumentException("Type of the signal must be bool, int, double, decimal or string.");
            }
        }

        public IEnumerable<Datum> GetCoarseData(int signalId, Granularity granularity, DateTime fromIncludedUtc, DateTime toExcludedUtc)
        {
            var signal = signalsDomainService.GetById(signalId);
            if (signal.Granularity == granularity.ToDomain<Domain.Granularity>())
                throw new ArgumentException("The granularity for coarse data has to be smaller than signal's granularity.");
            else return default(IEnumerable<Datum>);
        }

        public void SetData(int signalId, IEnumerable<Datum> data)
        {
            var signal = signalsDomainService.GetById(signalId);
            if (signal == null)
                throw new ArgumentException();

            switch (signal.DataType)
            {
                case Domain.DataType.Boolean:
                    signalsDomainService.SetData(signalId, data.Select(d => d.ToDomain<Domain.Datum<bool>>()));
                    break;
                case Domain.DataType.Integer:
                    signalsDomainService.SetData(signalId, data.Select(d => d.ToDomain<Domain.Datum<int>>()));
                    break;
                case Domain.DataType.Double:
                    signalsDomainService.SetData(signalId, data.Select(d => d.ToDomain<Domain.Datum<double>>()));
                    break;
                case Domain.DataType.Decimal:
                    signalsDomainService.SetData(signalId, data.Select(d => d.ToDomain<Domain.Datum<decimal>>()));
                    break;
                case Domain.DataType.String:
                    signalsDomainService.SetData(signalId, data.Select(d => d.ToDomain<Domain.Datum<string>>()));
                    break;
            }
        }

        public MissingValuePolicy GetMissingValuePolicy(int signalId)
        {
            if (signalsDomainService.GetById(signalId) == null)
                throw new ArgumentException();

            return signalsDomainService.GetMissingValuePolicy(signalId)?.ToDto<MissingValuePolicy>();
        }

        public void SetMissingValuePolicy(int signalId, MissingValuePolicy policy)
        {
            var signal = signalsDomainService.GetById(signalId);
            
            if ( signal == null) throw new ArgumentException();
            else
            {
                var mvp = policy as ShadowMissingValuePolicy;
                var DomainMVP = policy.ToDomain<Domain.MissingValuePolicy.MissingValuePolicyBase>();
                if (mvp != null) switch(mvp.DataType)
                    {
                    case DataType.Boolean: { signalsDomainService.SetMissingValuePolicy<bool>(signalId, DomainMVP);    break; }
                    case DataType.Decimal: { signalsDomainService.SetMissingValuePolicy<decimal>(signalId, DomainMVP); break; }
                    case DataType.Double:  { signalsDomainService.SetMissingValuePolicy<double>(signalId, DomainMVP);  break; }
                    case DataType.Integer: { signalsDomainService.SetMissingValuePolicy<int>(signalId, DomainMVP);     break; }
                    case DataType.String:  { signalsDomainService.SetMissingValuePolicy<string>(signalId, DomainMVP);  break; }
                    }
                else signalsDomainService.SetMissingValuePolicy<Type>(signalId, DomainMVP);
            }
        }
    }
}
