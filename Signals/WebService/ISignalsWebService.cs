﻿using System;
using System.Collections.Generic;
using System.ServiceModel;
using Dto;

namespace WebService
{
    [ServiceContract]
    public interface ISignalsWebService
    {
        [OperationContract]
        Signal Get(Path path);

        [OperationContract]
        Signal GetById(int signalId);

        [OperationContract]
        Signal Add(Signal signal);

        [OperationContract]
        void Delete(int signalId);

        [OperationContract]
        PathEntry GetPathEntry(Path path);

        [OperationContract]
        IEnumerable<Datum> GetSignalData(int signalId, DateTime fromIncludedUtc, DateTime toExcludedUtc);

        [OperationContract]
        void SetSignalData(int signalId, IEnumerable<Datum> data);

        [OperationContract]
        Dto.MissingValuePolicy.MissingValuePolicy GetMissingValuePolicy(int signalId);

        [OperationContract]
        void SetMissingValuePolicy(int signalId, Dto.MissingValuePolicy.MissingValuePolicy policy);
    }
}
