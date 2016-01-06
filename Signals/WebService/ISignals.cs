using System;
using System.Collections.Generic;
using System.ServiceModel;
using Signals.Dto;

namespace Signals.WebService
{
    [ServiceContract]
    public interface ISignals
    {
        [OperationContract]
        Signal Get(Path path);

        [OperationContract]
        Signal Add(Signal signal);

        [OperationContract]
        IEnumerable<Datum> GetData(Signal signal, DateTime fromIncluded, DateTime toExcluded);

        [OperationContract]
        void SetData(Signal signal, DateTime fromIncluded, IEnumerable<Datum> data);
    }
}
