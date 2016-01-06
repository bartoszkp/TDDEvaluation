using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace Signals.WebService
{
    [ServiceContract]
    public interface ISignals
    {
        [OperationContract]
        global::Signals.Domain.Signal Get(global::Signals.Domain.Path path);

        [OperationContract]
        global::Signals.Domain.Signal Add(global::Signals.Domain.Path path, string dataType, global::Signals.Domain.Granularity granularity);

        [OperationContract]
        IEnumerable<global::Signals.Domain.Datum<object>> GetData(global::Signals.Domain.Signal signal, DateTime fromIncluded, DateTime toExcluded);

        [OperationContract]
        void SetData(global::Signals.Domain.Signal signal, DateTime fromIncluded, IEnumerable<global::Signals.Domain.Datum<object>> data);
    }
}
