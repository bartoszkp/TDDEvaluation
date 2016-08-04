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
            var signal = GetById(signalId);

            if (signal == null)
                throw new Domain.Exceptions.SettingNotExistingSignalDataException();

            if(signal.DataType == Dto.DataType.Boolean)
            {
                List<Domain.Datum<bool>> domainDatumList = new List<Domain.Datum<bool>>();

                foreach (var item in data)
                {
                    Domain.Datum<bool> domainDatum = new Domain.Datum<bool>();

                    domainDatum.Quality = item.Quality.ToDomain<Domain.Quality>();
                    domainDatum.Timestamp = item.Timestamp;
                    domainDatum.Value = Convert.ToBoolean(item.Value);

                    domainDatumList.Add(domainDatum);
                }

                IEnumerable<Domain.Datum<bool>> domainData = domainDatumList.AsEnumerable<Domain.Datum<bool>>();

                signalsDomainService.SetData(signalId, domainData);
            }
            else if(signal.DataType == Dto.DataType.Decimal)
            {
                List<Domain.Datum<decimal>> domainDatumList = new List<Domain.Datum<decimal>>();

                foreach (var item in data)
                {
                    Domain.Datum<decimal> domainDatum = new Domain.Datum<decimal>();

                    domainDatum.Quality = item.Quality.ToDomain<Domain.Quality>();
                    domainDatum.Timestamp = item.Timestamp;
                    domainDatum.Value = Convert.ToDecimal(item.Value);

                    domainDatumList.Add(domainDatum);
                }

                IEnumerable<Domain.Datum<decimal>> domainData = domainDatumList.AsEnumerable<Domain.Datum<decimal>>();

                signalsDomainService.SetData(signalId, domainData);
            }
            else if(signal.DataType == Dto.DataType.Double)
            {
                List<Domain.Datum<double>> domainDatumList = new List<Domain.Datum<double>>();

                foreach (var item in data)
                {
                    Domain.Datum<double> domainDatum = new Domain.Datum<double>();

                    domainDatum.Quality = item.Quality.ToDomain<Domain.Quality>();
                    domainDatum.Timestamp = item.Timestamp;
                    domainDatum.Value = Convert.ToDouble(item.Value);

                    domainDatumList.Add(domainDatum);
                }

                IEnumerable<Domain.Datum<double>> domainData = domainDatumList.AsEnumerable<Domain.Datum<double>>();

                signalsDomainService.SetData(signalId, domainData);
            }
            else if (signal.DataType == Dto.DataType.Integer)
            {
                List<Domain.Datum<int>> domainDatumList = new List<Domain.Datum<int>>();

                foreach (var item in data)
                {
                    Domain.Datum<int> domainDatum = new Domain.Datum<int>();

                    domainDatum.Quality = item.Quality.ToDomain<Domain.Quality>();
                    domainDatum.Timestamp = item.Timestamp;
                    domainDatum.Value = Convert.ToInt32(item.Value);

                    domainDatumList.Add(domainDatum);
                }

                IEnumerable<Domain.Datum<int>> domainData = domainDatumList.AsEnumerable<Domain.Datum<int>>();

                signalsDomainService.SetData(signalId, domainData);
            }
            else if (signal.DataType == Dto.DataType.String)
            {
                List<Domain.Datum<string>> domainDatumList = new List<Domain.Datum<string>>();

                foreach (var item in data)
                {
                    Domain.Datum<string> domainDatum = new Domain.Datum<string>();

                    domainDatum.Quality = item.Quality.ToDomain<Domain.Quality>();
                    domainDatum.Timestamp = item.Timestamp;
                    domainDatum.Value = Convert.ToString(item.Value);

                    domainDatumList.Add(domainDatum);
                }

                IEnumerable<Domain.Datum<string>> domainData = domainDatumList.AsEnumerable<Domain.Datum<string>>();

                signalsDomainService.SetData(signalId, domainData);
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
