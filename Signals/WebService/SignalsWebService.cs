using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using DataAccess.Infrastructure;
using Domain.Exceptions;
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
            if (signalDto.Id != null)
                throw new IdNotNullException();

            var signal = signalDto.ToDomain<Domain.Signal>();

            var result = signalsDomainService.Add(signal);

            return result.ToDto<Dto.Signal>();
        }

        public void Delete(int signalId)
        {
            var signalDataType = signalsDomainService.GetById(signalId).DataType;
            switch(signalDataType)
            {
                case Domain.DataType.Boolean: signalsDomainService.Delete<bool>(signalId); break;
                case Domain.DataType.Decimal: signalsDomainService.Delete<decimal>(signalId); break;
                case Domain.DataType.Double: signalsDomainService.Delete<double>(signalId); break;
                case Domain.DataType.Integer: signalsDomainService.Delete<int>(signalId); break;
                case Domain.DataType.String: signalsDomainService.Delete<string>(signalId); break;
            }
            
        }

        public PathEntry GetPathEntry(Path pathDto)
        {
            return signalsDomainService.GetPathEntry(pathDto.ToDomain<Domain.Path>()).ToDto<Dto.PathEntry>();
        }

        public IEnumerable<Datum> GetData(int signalId, DateTime fromIncludedUtc, DateTime toExcludedUtc)
        {
            var signal = GetById(signalId);
            if (signal == null)
                throw new SignalNotFoundException(signalId);

            if (signal.DataType == DataType.Double)
            {
                var data = signalsDomainService.GetData<double>(signal.ToDomain<Domain.Signal>(), fromIncludedUtc, toExcludedUtc);
                return ConvertCollectionDomainToDto(data);
            }
            else if (signal.DataType == DataType.Boolean)
            {
                var data = signalsDomainService.GetData<bool>(signal.ToDomain<Domain.Signal>(), fromIncludedUtc, toExcludedUtc);
                return ConvertCollectionDomainToDto(data);
            }
            else if (signal.DataType == DataType.Integer)
            {
                var data = signalsDomainService.GetData<int>(signal.ToDomain<Domain.Signal>(), fromIncludedUtc, toExcludedUtc);
                return ConvertCollectionDomainToDto(data);
            }            
            else if (signal.DataType == DataType.Decimal)
            {
                var data = signalsDomainService.GetData<decimal>(signal.ToDomain<Domain.Signal>(), fromIncludedUtc, toExcludedUtc);
                return ConvertCollectionDomainToDto(data);
            }
            else if (signal.DataType == DataType.String)
            {
                var data = signalsDomainService.GetData<string>(signal.ToDomain<Domain.Signal>(), fromIncludedUtc, toExcludedUtc);
                return ConvertCollectionDomainToDto(data);
            }
            else throw new NotImplementedException();          
        }

        public IEnumerable<Datum> GetCoarseData(int signalId, Granularity granularity, DateTime fromIncludedUtc, DateTime toExcludedUtc)
        {
            var signal = GetById(signalId);
            if (signal == null)
                throw new SignalNotFoundException(signalId);
            
            var filledList = new List<Datum>();
            if (fromIncludedUtc > toExcludedUtc)
                return filledList;


            return filledList;
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
            var signal = GetById(signalId);
            if (signal == null)
                throw new SignalNotFoundException(signalId);

            var mvp = signalsDomainService.GetMissingValuePolicy(signal.ToDomain<Domain.Signal>());
            if (mvp == null)
                return null;
                    
            return mvp.ToDto<MissingValuePolicy>();
        }

        public void SetMissingValuePolicy(int signalId, MissingValuePolicy policy)
        {
            var signal = GetById(signalId);
            if (signal == null)
                throw new SignalNotFoundException(signalId);

            var domain_policy = policy.ToDomain<Domain.MissingValuePolicy.MissingValuePolicyBase>();

            signalsDomainService.SetMissingValuePolicy(signal.ToDomain<Domain.Signal>(), domain_policy);
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
        private IEnumerable<Dto.Datum> ConvertCollectionDomainToDto<T>(IEnumerable<Domain.Datum<T>> data)
        {
            int i = 0;
            if (data == null)
                return null;

            var dto_data = new Dto.Datum[data.Count()];
            foreach (var datum in data)
                dto_data[i++] = datum.ToDto<Dto.Datum>();

            return dto_data;
        }
    }
}
