﻿using System;
using System.Collections.Generic;

namespace Domain.Services
{
    public interface ISignalsDomainService
    {
        Signal GetById(int signalId);

        Signal Add(Signal newSignal);
        Signal Get(Path pathDto);

        void SetData<T>(IEnumerable<Datum<T>> data);

        IEnumerable<Datum<T>> GetData<T>(Signal signal, DateTime fromIncludedUtc, DateTime toExcludedUtc);
    }
}
