﻿using System;
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
            return this.signalsDomainService.GetByPath(pathDto.ToDomain<Domain.Path>())
                ?.ToDto<Dto.Signal>();
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
            signalsDomainService.Delete(signalId);
        }

        public PathEntry GetPathEntry(Path pathDto)
        {
            Domain.Path pathDomain = pathDto.ToDomain<Domain.Path>();
            int lengthEntryPath = pathDomain.Length + 1;
            IEnumerable<Domain.Signal> signalsDomain = signalsDomainService.GetPathEntry(pathDomain).ToArray();
            IEnumerable<Dto.Signal> signalsDto = signalsDomain?.ToDto<IEnumerable<Dto.Signal>>();
            Dto.PathEntry pathEntry = new PathEntry()
            {
                Signals = signalsDto.Where<Dto.Signal>(s => s.Path.Components.Count() == lengthEntryPath).Select(signal => signal),
                SubPaths = signalsDto
                    .Where<Dto.Signal>(s => s.Path.Components.Count() > lengthEntryPath)
                    .Select(signal => new Path(){ Components = signal.Path.Components.Take(lengthEntryPath) })
                    .Distinct(new PathComparer())
            };
            return pathEntry;
        }

        public IEnumerable<Datum> GetData(int signalId, DateTime fromIncludedUtc, DateTime toExcludedUtc)
        {
            var signal = signalsDomainService.GetById(signalId);

            if (signal == null)
                throw new SignalNotFoundException(signalId);

            switch (signal.DataType)
            {
                case Domain.DataType.Boolean:
                    return signalsDomainService.GetData<bool>(signal, fromIncludedUtc, toExcludedUtc)
                        .Select(d => d.ToDto<Dto.Datum>()).ToList().OrderBy(t => t.Timestamp);
                case Domain.DataType.Integer:
                    return signalsDomainService.GetData<int>(signal, fromIncludedUtc, toExcludedUtc)
                        .Select(d => d.ToDto<Dto.Datum>()).ToList().OrderBy(t => t.Timestamp);
                case Domain.DataType.Double:
                    return signalsDomainService.GetData<double>(signal, fromIncludedUtc, toExcludedUtc)
                        .Select(d => d.ToDto<Dto.Datum>()).ToList().OrderBy(t => t.Timestamp);
                case Domain.DataType.Decimal:
                    return signalsDomainService.GetData<decimal>(signal, fromIncludedUtc, toExcludedUtc)
                        .Select(d => d.ToDto<Dto.Datum>()).ToList().OrderBy(t => t.Timestamp);
                case Domain.DataType.String:
                    return signalsDomainService.GetData<string>(signal, fromIncludedUtc, toExcludedUtc)
                        .Select(d => d.ToDto<Dto.Datum>()).ToList().OrderBy(t => t.Timestamp);
                default:
                    throw new NotImplementedException();
            }
        }

        public IEnumerable<Datum> GetCoarseData(int signalId, Granularity granularity, DateTime fromIncludedUtc, DateTime toExcludedUtc)
        {
            throw new NotImplementedException();
        }

        public void SetData(int signalId, IEnumerable<Datum> data)
        {
            var signal = signalsDomainService.GetById(signalId);

            if (signal == null)
                throw new SignalNotFoundException(signalId);

            switch (signal.DataType)
            {
                case Domain.DataType.Boolean:
                    signalsDomainService.SetData(signal, data.Select(d => d.ToDomain<Domain.Datum<bool>>()));
                    break;
                case Domain.DataType.Integer:
                    signalsDomainService.SetData(signal, data.Select(d => d.ToDomain<Domain.Datum<int>>()));
                    break;
                case Domain.DataType.Double:
                    signalsDomainService.SetData(signal, data.Select(d => d.ToDomain<Domain.Datum<double>>()));
                    break;
                case Domain.DataType.Decimal:
                    signalsDomainService.SetData(signal, data.Select(d => d.ToDomain<Domain.Datum<decimal>>()));
                    break;
                case Domain.DataType.String:
                    signalsDomainService.SetData(signal, data.Select(d => d.ToDomain<Domain.Datum<string>>()));
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        public MissingValuePolicy GetMissingValuePolicy(int signalId)
        {
            var signal = signalsDomainService.GetById(signalId);

            if (signal == null)
                throw new SignalNotFoundException(signalId);

            return signalsDomainService.GetMissingValuePolicy(signal)
                ?.ToDto<Dto.MissingValuePolicy.MissingValuePolicy>();
        }

        public void SetMissingValuePolicy(int signalId, MissingValuePolicy policy)
        {
            var signal = signalsDomainService.GetById(signalId);
            if (signal == null)
                throw new SignalNotFoundException(signalId);

            if(policy is ShadowMissingValuePolicy)
            {
                var mvp = policy as ShadowMissingValuePolicy;
                var shadow = mvp.ShadowSignal.ToDomain<Domain.Signal>();
                if(shadow.DataType != signal.DataType || shadow.Granularity != signal.Granularity)                
                    throw new IncompatibleShadowSignalException();                
            }
            var domainPolicy = policy?.ToDomain<Domain.MissingValuePolicy.MissingValuePolicyBase>();

            signalsDomainService.SetMissingValuePolicy(signal, domainPolicy);
        }
    }
}
