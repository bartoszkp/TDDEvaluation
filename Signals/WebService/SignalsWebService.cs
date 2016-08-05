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
            throw new NotImplementedException();
        }

        public PathEntry GetPathEntry(Path pathDto)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Datum> GetData(int signalId, DateTime fromIncludedUtc, DateTime toExcludedUtc)
        {
            throw new NotImplementedException();
        }

        public void SetData(int signalId, IEnumerable<Datum> data)
        {
            var signal = GetById(signalId).ToDomain<Domain.Signal>();
            if (signal == null) throw new ArgumentException("no signals with this id");

            if(signal.DataType == Domain.DataType.Integer)
            {
                var domainData= data.ToDomain<IEnumerable<DataAccess.GenericInstantiations.DatumInteger>>().ToArray();
                foreach (var x in domainData)
                {
                    x.Signal = signal;
                }
                signalsDomainService.SetData<int>(domainData);
            }

            if (signal.DataType == Domain.DataType.Double)
            {
                var domainData = data.ToDomain<IEnumerable<DataAccess.GenericInstantiations.DatumDouble>>().ToArray();
                foreach (var x in domainData)
                {
                    x.Signal = signal;
                }
                signalsDomainService.SetData<double>(domainData);
            }
            if (signal.DataType == Domain.DataType.Decimal)
            {
                var domainData = data.ToDomain<IEnumerable<DataAccess.GenericInstantiations.DatumDecimal>>().ToArray();
                foreach (var x in domainData)
                {
                    x.Signal = signal;
                }
                signalsDomainService.SetData<decimal>(domainData);
            }
            if (signal.DataType == Domain.DataType.Boolean)
            {
                var domainData = data.ToDomain<IEnumerable<DataAccess.GenericInstantiations.DatumBoolean>>().ToArray();
                foreach (var x in domainData)
                {
                    x.Signal = signal;
                }
                signalsDomainService.SetData<bool>(domainData);
            }
            if (signal.DataType == Domain.DataType.String)
            {
                var domainData = data.ToDomain<IEnumerable<DataAccess.GenericInstantiations.DatumString>>().ToArray();
                foreach (var x in domainData)
                {
                    x.Signal = signal;
                }
                signalsDomainService.SetData<string>(domainData);
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
