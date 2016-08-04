using System;
using System.Collections.Generic;
using Domain.MissingValuePolicy;

namespace Domain.Services
{
    public interface ISignalsDomainService
    {
        Signal GetById(int signalId);

        Signal Add(Signal newSignal);

        Signal Get(Path pathDto);

        void SetData(IEnumerable<Datum<object>> newDomainDatum);

        IEnumerable<Datum<object>> GetData(Signal getSignal, DateTime fromIncludedUtc, DateTime toExcludedUtc);

        void SetMVP(Signal domainSetMVPSignal, MissingValuePolicyBase domainPolicyBase);

        MissingValuePolicy.MissingValuePolicyBase GetMVP(Signal domainSignal);
    }
}
