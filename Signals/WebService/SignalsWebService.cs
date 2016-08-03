﻿using System;
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
            return signalsDomainService.Get(pathDto.ToDomain<Domain.Path>()).ToDto<Dto.Signal>();
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
            var signal = GetById(signalId);
            if (signal == null)
                throw new SignalNotFoundException(signalId);

            return null;
        }

        public void SetData(int signalId, IEnumerable<Datum> data)
        {
            var signal = GetById(signalId);
            if (signal == null)
                throw new SignalNotFoundException(signalId);
                        
            if(data == null)
            {
                signalsDomainService.SetData<double>(null);
                return;
            }
            
            if (signal.DataType == DataType.Double)
            {
                var domain_data = ConvertCollectionDtoToDomainAndSetData<double>(data, signal);
                signalsDomainService.SetData(domain_data);
            }
            else if(signal.DataType == DataType.Boolean)
            {
                var domain_data = ConvertCollectionDtoToDomainAndSetData<bool>(data, signal);
                signalsDomainService.SetData(domain_data);
            }
            else if(signal.DataType == DataType.Integer)
            {
                var domain_data = ConvertCollectionDtoToDomainAndSetData<int>(data, signal);
                signalsDomainService.SetData(domain_data);
            }
            else if(signal.DataType == DataType.Decimal)
            {
                var domain_data = ConvertCollectionDtoToDomainAndSetData<decimal>(data, signal);
                signalsDomainService.SetData(domain_data);
            }
            else if(signal.DataType == DataType.String)
            {
                var domain_data = ConvertCollectionDtoToDomainAndSetData<string>(data, signal);
                signalsDomainService.SetData(domain_data);
            }
            else
                throw new NotImplementedException();
            
        }

        public MissingValuePolicy GetMissingValuePolicy(int signalId)
        {
            throw new NotImplementedException();
        }

        public void SetMissingValuePolicy(int signalId, MissingValuePolicy policy)
        {
            throw new NotImplementedException();
        }

        private IEnumerable<Domain.Datum<T>> ConvertCollectionDtoToDomainAndSetData<T>(IEnumerable<Datum> data, Signal signal)
        {
            int i = 0;
            var domain_data = new Domain.Datum<T>[data.Count()];
            foreach (var datum in data)
            {
                domain_data[i] = datum.ToDomain<Domain.Datum<T>>();
                domain_data[i].Signal = signal.ToDomain<Domain.Signal>();
                ++i;
            }

            return domain_data;     
        }
    }
}
