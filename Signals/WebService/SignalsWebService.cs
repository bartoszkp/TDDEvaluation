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
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        public void SetData(int signalId, IEnumerable<Datum> data)
        {

           
            if (signalsDomainService != null)
            {
                var result = signalsDomainService.GetById(signalId);


                
                switch (result.DataType)
                {
                    case Domain.DataType.Boolean:

                        signalsDomainService.SetData<Boolean>(data.Select(s => s.ToDomain<Domain.Datum<Boolean>>()).ToList());

                        break;
                    case Domain.DataType.Integer:

                        signalsDomainService.SetData<int>(data.Select(s => s.ToDomain<Domain.Datum<int>>()).ToList());
                        break;
                    case Domain.DataType.Double:

                        signalsDomainService.SetData<double>(data.Select(s => s.ToDomain<Domain.Datum<double>>()).ToList());
                        break;
                    case Domain.DataType.Decimal:
                        signalsDomainService.SetData<decimal>(data.Select(s => s.ToDomain<Domain.Datum<decimal>>()).ToList());
                        break;
                    case Domain.DataType.String:
                        signalsDomainService.SetData<string>(data.Select(s => s.ToDomain<Domain.Datum<string>>()).ToList());
                        break;
                    default:
                        break;
                }

           

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
