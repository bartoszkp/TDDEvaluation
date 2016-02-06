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
        IEnumerable<Datum> GetData(Signal signal, DateTime fromIncluded, DateTime toExcluded);

        [OperationContract]
        void SetData(Signal signal, IEnumerable<Datum> data);

        [OperationContract]
        MissingValuePolicyConfig GetMissingValuePolicyConfig(Signal signal);
    }
}
