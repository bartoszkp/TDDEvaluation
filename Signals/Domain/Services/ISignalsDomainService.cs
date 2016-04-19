﻿using System;
using System.Collections.Generic;

namespace Domain.Services
{
    public interface ISignalsDomainService
    {
        Signal Get(Path path);

        Signal Get(int signalId);

        Signal Add(Signal signal);

        PathEntry GetPathEntry(Path path);

        IEnumerable<Datum<T>> GetData<T>(Signal signal, DateTime fromIncluded, DateTime toExcluded);

        void SetData<T>(Signal signal, IEnumerable<Datum<T>> data);

        void SetMissingValuePolicyConfig(Signal signal, MissingValuePolicyConfig config);
    }
}
