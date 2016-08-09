using System;
using System.Collections.Generic;
using Domain.MissingValuePolicy;

namespace Domain.Services
{
    public interface ISignalsDomainService
    {
        Signal Add<T>(Signal newSignal, NoneQualityMissingValuePolicy<T> nonePolicy);

        Signal GetById(int signalId);
        Signal Get(Path pathDomain);
        MissingValuePolicyBase GetMissingValuePolicyBase(int signalId);
        void SetMissingValuePolicyBase(int signalId, MissingValuePolicyBase policy);
        IEnumerable<Datum<T>> GetData<T>(int signalId, DateTime fromIncludedUtc, DateTime toExcludedUtc);
        void SetData<T>(int signalId, IEnumerable<Datum<T>> dataDomain);
    }
}
