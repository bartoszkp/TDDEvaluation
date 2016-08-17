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
using Domain.Exceptions;

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
            var pathDtoComponents = pathDto.Components;
            var pathString = Domain.Path.JoinComponents(pathDtoComponents);
            var pathDomain = Domain.Path.FromString(pathString);

            return this.signalsDomainService.Get(pathDomain)?.ToDto<Dto.Signal>();
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
            return this.signalsDomainService.GetByPrefixPath(pathDto.ToDomain<Domain.Path>()).ToDto<Dto.PathEntry>();
        }

        public IEnumerable<Datum> GetData(int signalId, DateTime fromIncludedUtc, DateTime toExcludedUtc)
        {
            var signal = this.signalsDomainService.GetById(signalId);

            if (signal == null)
                throw new CouldntGetASignalException();

            var dataDto = new List<Datum>();

            switch (signal.DataType)
            {
                case Domain.DataType.Boolean:
                    return this.signalsDomainService.GetData<bool>(signal, fromIncludedUtc, toExcludedUtc)
                        .ToArray().ToDto<IEnumerable<Dto.Datum>>();
                case Domain.DataType.Integer:
                    return this.signalsDomainService.GetData<int>(signal, fromIncludedUtc, toExcludedUtc)
                        .ToArray().ToDto<IEnumerable<Dto.Datum>>();
                case Domain.DataType.Double:
                    return this.signalsDomainService.GetData<double>(signal, fromIncludedUtc, toExcludedUtc)
                        .ToArray().ToDto<IEnumerable<Dto.Datum>>();
                case Domain.DataType.Decimal:
                    return this.signalsDomainService.GetData<decimal>(signal, fromIncludedUtc, toExcludedUtc)
                        .ToArray().ToDto<IEnumerable<Dto.Datum>>();
                case Domain.DataType.String:
                    return this.signalsDomainService.GetData<string>(signal, fromIncludedUtc, toExcludedUtc)
                        .ToArray().ToDto<IEnumerable<Dto.Datum>>();
                default: throw new TypeUnloadedException();
            }
        }

        public void SetData(int signalId, IEnumerable<Datum> data)
        {
            var signal = this.GetById(signalId);

            if (signal == null)
                throw new ArgumentException("Signal with given Id not found.");

            if (signal.DataType == DataType.Integer)
            {
                var dataDomain = new List<Domain.Datum<int>>();

                for (int i = 0; i < data.Count(); i++)
                {
                    dataDomain.Add(data.ElementAt(i).ToDomain<Domain.Datum<int>>());
                    dataDomain.ElementAt(i).Signal = signal.ToDomain<Domain.Signal>();
                }

                this.signalsDomainService.SetData(dataDomain);
            }
            else if (signal.DataType == DataType.Double)
            {
                var dataDomain = new List<Domain.Datum<double>>();

                for (int i = 0; i < data.Count(); i++)
                {
                    dataDomain.Add(data.ElementAt(i).ToDomain<Domain.Datum<double>>());
                    dataDomain.ElementAt(i).Signal = signal.ToDomain<Domain.Signal>();
                }

                this.signalsDomainService.SetData(dataDomain);
            }
            else if (signal.DataType == DataType.Decimal)
            {
                var dataDomain = new List<Domain.Datum<decimal>>();

                for (int i = 0; i < data.Count(); i++)
                {
                    dataDomain.Add(data.ElementAt(i).ToDomain<Domain.Datum<decimal>>());
                    dataDomain.ElementAt(i).Signal = signal.ToDomain<Domain.Signal>();
                }

                this.signalsDomainService.SetData(dataDomain);
            }
            else if (signal.DataType == DataType.Boolean)
            {
                var dataDomain = new List<Domain.Datum<bool>>();

                for (int i = 0; i < data.Count(); i++)
                {
                    dataDomain.Add(data.ElementAt(i).ToDomain<Domain.Datum<bool>>());
                    dataDomain.ElementAt(i).Signal = signal.ToDomain<Domain.Signal>();
                }

                this.signalsDomainService.SetData(dataDomain);
            }
            else if (signal.DataType == DataType.String)
            {
                var dataDomain = new List<Domain.Datum<string>>();

                for (int i = 0; i < data.Count(); i++)
                {
                    dataDomain.Add(data.ElementAt(i).ToDomain<Domain.Datum<string>>());
                    dataDomain.ElementAt(i).Signal = signal.ToDomain<Domain.Signal>();
                }

                this.signalsDomainService.SetData(dataDomain);
            }
        }

        public MissingValuePolicy GetMissingValuePolicy(int signalId)
        {
            return this.signalsDomainService.GetMissingValuePolicy(signalId).ToDto<Dto.MissingValuePolicy.MissingValuePolicy>();
        }

        public void SetMissingValuePolicy(int signalId, MissingValuePolicy policy)
        {
            var policyDomain = policy.ToDomain<Domain.MissingValuePolicy.MissingValuePolicyBase>();
            this.signalsDomainService.SetMissingValuePolicy(signalId, policyDomain);
        }
    }
}
