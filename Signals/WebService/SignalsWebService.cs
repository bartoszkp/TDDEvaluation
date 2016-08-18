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
            var result = signalsDomainService.GetByPath(pathDto.ToDomain<Domain.Path>());

            return result.ToDto<Dto.Signal>();
        }

        public Signal GetById(int signalId)
        {
            if (signalId == 0)
            {
                return null;
            }
            else return signalsDomainService.GetById(signalId).ToDto<Dto.Signal>();
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
            var pathDomain = pathDto.ToDomain<Domain.Path>();

            return this.signalsDomainService?.GetPathEntry(pathDomain).ToDto<Dto.PathEntry>();
        }

        public IEnumerable<Datum> GetData(int signalId, DateTime fromIncludedUtc, DateTime toExcludedUtc)
        {
            var signal = GetById(signalId);

            if (signal == null)
                throw new  Domain.Exceptions.GettingDataOfNotExistingSignal(); 

            if(signal.DataType == Dto.DataType.Boolean)
            {
                IEnumerable<Domain.Datum<bool>> toConvertToReturn = signalsDomainService.GetData<bool>(signalId, fromIncludedUtc, toExcludedUtc);

                List<Dto.Datum> dtoDatumList = new List<Dto.Datum>();

                foreach (var item in toConvertToReturn)
                {
                    Dto.Datum dtoDatum = new Dto.Datum();

                    dtoDatum.Quality = item.Quality.ToDto<Dto.Quality>();
                    dtoDatum.Timestamp = item.Timestamp;
                    dtoDatum.Value = Convert.ToBoolean(item.Value);

                    dtoDatumList.Add(dtoDatum);
                }

                return dtoDatumList.AsEnumerable<Dto.Datum>();
            }
            else if(signal.DataType == Dto.DataType.Decimal)
            {
                IEnumerable<Domain.Datum<decimal>> toConvertToReturn = signalsDomainService.GetData<decimal>(signalId, fromIncludedUtc, toExcludedUtc);

                List<Dto.Datum> dtoDatumList = new List<Dto.Datum>();

                foreach (var item in toConvertToReturn)
                {
                    Dto.Datum dtoDatum = new Dto.Datum();

                    dtoDatum.Quality = item.Quality.ToDto<Dto.Quality>();
                    dtoDatum.Timestamp = item.Timestamp;
                    dtoDatum.Value = Convert.ToDecimal(item.Value);

                    dtoDatumList.Add(dtoDatum);
                }

                return dtoDatumList.AsEnumerable<Dto.Datum>();
            }
            else if (signal.DataType == Dto.DataType.Double)
            {
                IEnumerable<Domain.Datum<double>> toConvertToReturn = signalsDomainService.GetData<double>(signalId, fromIncludedUtc, toExcludedUtc);

                List<Dto.Datum> dtoDatumList = new List<Dto.Datum>();

                foreach (var item in toConvertToReturn)
                {
                    Dto.Datum dtoDatum = new Dto.Datum();

                    dtoDatum.Quality = item.Quality.ToDto<Dto.Quality>();
                    dtoDatum.Timestamp = item.Timestamp;
                    dtoDatum.Value = Convert.ToDouble(item.Value);

                    dtoDatumList.Add(dtoDatum);
                }

                return dtoDatumList.AsEnumerable<Dto.Datum>();
            }
            else if (signal.DataType == Dto.DataType.Integer)
            {
                IEnumerable<Domain.Datum<int>> toConvertToReturn = signalsDomainService.GetData<int>(signalId, fromIncludedUtc, toExcludedUtc);

                List<Dto.Datum> dtoDatumList = new List<Dto.Datum>();

                foreach (var item in toConvertToReturn)
                {
                    Dto.Datum dtoDatum = new Dto.Datum();

                    dtoDatum.Quality = item.Quality.ToDto<Dto.Quality>();
                    dtoDatum.Timestamp = item.Timestamp;
                    dtoDatum.Value = Convert.ToInt32(item.Value);

                    dtoDatumList.Add(dtoDatum);
                }

                return dtoDatumList.AsEnumerable<Dto.Datum>();
            }
            else if (signal.DataType == Dto.DataType.String)
            {
                IEnumerable<Domain.Datum<string>> toConvertToReturn = signalsDomainService.GetData<string>(signalId, fromIncludedUtc, toExcludedUtc);

                List<Dto.Datum> dtoDatumList = new List<Dto.Datum>();

                foreach (var item in toConvertToReturn)
                {
                    Dto.Datum dtoDatum = new Dto.Datum();

                    dtoDatum.Quality = item.Quality.ToDto<Dto.Quality>();
                    dtoDatum.Timestamp = item.Timestamp;
                    dtoDatum.Value = Convert.ToString(item.Value);

                    dtoDatumList.Add(dtoDatum);
                }

                return dtoDatumList.AsEnumerable<Dto.Datum>();
            }

            return null;
        }

        public void SetData(int signalId, IEnumerable<Datum> data)
        {
            var signal = GetById(signalId).ToDomain<Domain.Signal>();

            if (signal == null)
                throw new Domain.Exceptions.SettingNotExistingSignalDataException();

            var sortedDomainData = data.OrderBy(d => d.Timestamp).ToArray();

            var typename = signal.DataType.GetNativeType().Name;

            switch (typename)
            {
                case "Int32":
                    GenericSetDataCall<int>(signalId, sortedDomainData);
                    break;
                case "Double":
                    GenericSetDataCall<double>(signalId, sortedDomainData);
                    break;
                case "Decimal":
                    GenericSetDataCall<decimal>(signalId, sortedDomainData);
                    break;
                case "Boolean":
                    GenericSetDataCall<bool>(signalId, sortedDomainData);
                    break;
                case "String":
                    GenericSetDataCall<string>(signalId, sortedDomainData);
                    break;
            }
        }

        private void GenericSetDataCall<T>(int signalId, Datum[] datum)
        {
            signalsDomainService.SetData<T>(signalId, datum.ToDomain<IEnumerable<Domain.Datum<T>>>().ToArray());
        }

        public MissingValuePolicy GetMissingValuePolicy(int signalId)
        {
            var signal = signalsDomainService.GetById(signalId);
            return this.signalsDomainService.GetMissingValuePolicy(signal).ToDto<MissingValuePolicy>();
        }

        public void SetMissingValuePolicy(int signalId, MissingValuePolicy policy)
        {
            var signal = GetById(signalId);

            if (signal == null)
                throw new Domain.Exceptions.SettingPolicyNotExistingSignalException();

            signalsDomainService.SetMissingValuePolicy(signal.ToDomain<Domain.Signal>(), policy.ToDomain<Domain.MissingValuePolicy.MissingValuePolicyBase>());
        }
    }
}
