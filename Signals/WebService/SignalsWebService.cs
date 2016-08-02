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
                        var list1 = new List<Domain.Datum<Boolean>>();
                        foreach (var i in data)
                        {

                            list1.Add(i.ToDomain<Domain.Datum<Boolean>>());
                        }

                        signalsDomainService.SetData<Boolean>(list1);
                        break;
                    case Domain.DataType.Integer:
                        var list2 = new List<Domain.Datum<int>>();
                        foreach (var i in data)
                        {

                            list2.Add(i.ToDomain<Domain.Datum<int>>());
                        }

                        signalsDomainService.SetData<int>(list2);
                        break;
                    case Domain.DataType.Double:
                        var list3 = new List<Domain.Datum<double>>();
                        foreach (var i in data)
                        {

                            list3.Add(i.ToDomain<Domain.Datum<double>>());
                        }

                        signalsDomainService.SetData<double>(list3);
                        break;
                    case Domain.DataType.Decimal:
                        var list4 = new List<Domain.Datum<Decimal>>();
                        foreach (var i in data)
                        {

                            list4.Add(i.ToDomain<Domain.Datum<Decimal>>());
                        }

                        signalsDomainService.SetData<Decimal>(list4);
                        break;
                    case Domain.DataType.String:
                        var list5 = new List<Domain.Datum<String>>();
                        foreach (var i in data)
                        {

                            list5.Add(i.ToDomain<Domain.Datum<String>>());
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
