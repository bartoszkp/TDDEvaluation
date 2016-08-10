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

            if (result == null) return null;

            else return result.ToDto<Dto.Signal>();
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
            if (signalsDomainService.GetById(signalId) == null) throw new ArgumentException();

            else
            {
                var signal = signalsDomainService.GetById(signalId);

                var type = signal.DataType.ToString();

                if (type == "Boolean") return signalsDomainService.GetData<bool>(signalId, fromIncludedUtc, toExcludedUtc).ToDto<IEnumerable<Dto.Datum>>();
                else if (type == "Integer") return signalsDomainService.GetData<int>(signalId, fromIncludedUtc, toExcludedUtc).ToDto<IEnumerable<Dto.Datum>>();
                else if (type == "Double") return signalsDomainService.GetData<double>(signalId, fromIncludedUtc, toExcludedUtc).ToDto<IEnumerable<Dto.Datum>>();
                else if (type == "Decimal") return signalsDomainService.GetData<decimal>(signalId, fromIncludedUtc, toExcludedUtc).ToDto<IEnumerable<Dto.Datum>>();
                else if (type == "String") return signalsDomainService.GetData<string>(signalId, fromIncludedUtc, toExcludedUtc).ToDto<IEnumerable<Dto.Datum>>();
                else throw new ArgumentException("Type of the signal must be bool, int, double, decimal or string.");
            }
        }

        public void SetData(int signalId, IEnumerable<Datum> data)
        {
            if (signalsDomainService.GetById(signalId) == null) throw new ArgumentException();

            else
            {
                Type type = null;

                foreach (var item in data)
                {
                    type = item.Value.GetType();
                }
                
                if (type == typeof(bool)) signalsDomainService.SetData(signalId, data.ToDomain<IEnumerable<Domain.Datum<bool>>>().ToArray());
                else if (type == typeof(int)) signalsDomainService.SetData(signalId, data.ToDomain<IEnumerable<Domain.Datum<int>>>().ToArray());
                else if (type == typeof(double)) signalsDomainService.SetData(signalId, data.ToDomain<IEnumerable<Domain.Datum<double>>>().ToArray());
                else if (type == typeof(decimal)) signalsDomainService.SetData(signalId, data.ToDomain<IEnumerable<Domain.Datum<decimal>>>().ToArray());
                else if (type == typeof(string)) signalsDomainService.SetData(signalId, data.ToDomain<IEnumerable<Domain.Datum<string>>>().ToArray());
                else throw new ArgumentException("Type of the 'data' parameter's signals must be bool, int, double, decimal or string.");

            }
        }

        public MissingValuePolicy GetMissingValuePolicy(int signalId)
        {
            if (signalsDomainService.GetById(signalId) == null) throw new ArgumentException();

            else
            {
                if (signalsDomainService.GetMissingValuePolicy(signalId) == null) return null;

                else return signalsDomainService.GetMissingValuePolicy(signalId).ToDto<Dto.MissingValuePolicy.MissingValuePolicy>();
            }
        }

        public void SetMissingValuePolicy(int signalId, MissingValuePolicy policy)
        {
            throw new ArgumentException();
        }

    }
}