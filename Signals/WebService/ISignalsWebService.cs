using Dto;
using System;
using System.Collections.Generic;
using System.ServiceModel;

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
        PathEntry GetPathEntry(Path path);

        [OperationContract]
        IEnumerable<Datum> GetData(int signalId, DateTime fromIncluded, DateTime toExcluded);

        [OperationContract]
        void SetData(int signalId, IEnumerable<Datum> data);

        [OperationContract]
        Dto.MissingValuePolicy.MissingValuePolicy GetMissingValuePolicy(int signalId);

        [OperationContract]
        void SetMissingValuePolicy(int signalId, Dto.MissingValuePolicy.MissingValuePolicy policy);
    }
}
