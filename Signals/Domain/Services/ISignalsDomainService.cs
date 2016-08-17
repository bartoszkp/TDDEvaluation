﻿using System;
using System.Collections.Generic;
using Domain.MissingValuePolicy;

namespace Domain.Services
{
    public interface ISignalsDomainService
    {
        Signal GetById(int signalId);

        Signal Add(Signal newSignal);

        Signal Get(Path pathDto);

        void SetMVP(Signal domainSetMVPSignal, MissingValuePolicyBase domainPolicyBase);

        MissingValuePolicy.MissingValuePolicyBase GetMVP(Signal domainSignal);

        IEnumerable<Datum<T>> GetData<T>(Signal getSignal, DateTime fromIncludedUtc, DateTime toExcludedUtc);

        void SetData<T>(Signal setDataSignal, IEnumerable<Datum<T>> enumerable);
    }
}
