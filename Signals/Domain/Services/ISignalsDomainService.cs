﻿using System;
using System.Collections.Generic;
using Domain.MissingValuePolicy;

namespace Domain.Services
{
    public interface ISignalsDomainService
    {
        Signal Add(Signal newSignal);

        Signal GetById(int signalId);

        Signal Get(Path pathDto);

        void SetMissingValuePolicy(Domain.Signal signal, MissingValuePolicyBase missingValuePolicyBase);

        MissingValuePolicyBase GetMissingValuePolicy(Domain.Signal signal);

        void SetData(Signal signal, IEnumerable<Datum<double>> datum);
    }
}
