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
        MissingValuePolicyConfig GetMissingValuePolicyConfig(int signalId);

        [OperationContract]
        void SetMissingValuePolicyConfig(int signalId, MissingValuePolicyConfig config);
    }
}
