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

                        var list=(data.Select(s => s.ToDomain<Domain.Datum<Boolean>>()).ToList());
                        for (int i= 0;i < list.Count;i++ )
                        {
                            list[i].Signal = result;
                        }
                        signalsDomainService.SetData<Boolean>(list);
                        break;
                    case Domain.DataType.Integer:

                        var list2 = (data.Select(s => s.ToDomain<Domain.Datum<int>>()).ToList());
                        for (int i = 0; i < list2.Count; i++)
                        {
                            list2[i].Signal = result;
                        }
                        signalsDomainService.SetData<int>(list2);
                        break;
                    case Domain.DataType.Double:
                        var list3 = (data.Select(s => s.ToDomain<Domain.Datum<double>>()).ToList());
                        for (int i = 0; i < list3.Count; i++)
                        {
                            list3[i].Signal = result;
                        }
                        signalsDomainService.SetData<double>(list3);
                        break;
                    case Domain.DataType.Decimal:
                        var list4 = (data.Select(s => s.ToDomain<Domain.Datum<Decimal>>()).ToList());
                        for (int i = 0; i < list4.Count; i++)
                        {
                            list4[i].Signal = result;
                        }
                        signalsDomainService.SetData<Decimal>(list4);
                        break;
                    case Domain.DataType.String:
                        var list5 = (data.Select(s => s.ToDomain<Domain.Datum<String>>()).ToList());
                        for (int i = 0; i < list5.Count; i++)
                        {
                            list5[i].Signal = result;
                        }
                        signalsDomainService.SetData<String>(list5);
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
