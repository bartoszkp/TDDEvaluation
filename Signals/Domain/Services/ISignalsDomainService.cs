﻿using System;
using System.Collections.Generic;

namespace Domain.Services
{
    public interface ISignalsDomainService
    {
        Signal Add(Signal newSignal);

        Signal GetById(int signalId);

        Signal GetByPath(Path signalPath);
        
        void Delete<T>(int signalId);

        void SetData<T>(Signal signalId, IEnumerable<Datum<T>> data);

        IEnumerable<Domain.Datum<T>> GetData<T>(int signalId, DateTime fromIncludedUtc, DateTime toExcludedUtc);

        void SetMissingValuePolicy(Signal signal, Domain.MissingValuePolicy.MissingValuePolicyBase policy);

        MissingValuePolicy.MissingValuePolicyBase GetMissingValuePolicy(Signal signal);

        PathEntry GetPathEntry(Path path);

        IEnumerable<Datum<T>> GetCoarseData<T>(Signal signal, Granularity granularity, DateTime fromIncludedUtc, DateTime toExcludedUtc);
    }
}
