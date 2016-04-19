using System;
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
        Signal Add(Signal signal);

        [OperationContract]
        PathEntry GetPathEntry(Path path);

        [OperationContract]
        IEnumerable<Datum> GetData(int signalId, DateTime fromIncluded, DateTime toExcluded);

        [OperationContract]
        void SetData(int signalId, IEnumerable<Datum> data);

        [OperationContract]
        MissingValuePolicy GetMissingValuePolicy(int signalId);

        [OperationContract]
        void SetMissingValuePolicy(int signalId, MissingValuePolicy config);
    }
}
