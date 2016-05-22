﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SignalsIntegrationTests.WS {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="WS.ISignalsWebService")]
    public interface ISignalsWebService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISignalsWebService/Get", ReplyAction="http://tempuri.org/ISignalsWebService/GetResponse")]
        Dto.Signal Get(Dto.Path path);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISignalsWebService/Get", ReplyAction="http://tempuri.org/ISignalsWebService/GetResponse")]
        System.Threading.Tasks.Task<Dto.Signal> GetAsync(Dto.Path path);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISignalsWebService/GetById", ReplyAction="http://tempuri.org/ISignalsWebService/GetByIdResponse")]
        Dto.Signal GetById(int signalId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISignalsWebService/GetById", ReplyAction="http://tempuri.org/ISignalsWebService/GetByIdResponse")]
        System.Threading.Tasks.Task<Dto.Signal> GetByIdAsync(int signalId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISignalsWebService/Add", ReplyAction="http://tempuri.org/ISignalsWebService/AddResponse")]
        Dto.Signal Add(Dto.Signal signal);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISignalsWebService/Add", ReplyAction="http://tempuri.org/ISignalsWebService/AddResponse")]
        System.Threading.Tasks.Task<Dto.Signal> AddAsync(Dto.Signal signal);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISignalsWebService/GetPathEntry", ReplyAction="http://tempuri.org/ISignalsWebService/GetPathEntryResponse")]
        Dto.PathEntry GetPathEntry(Dto.Path path);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISignalsWebService/GetPathEntry", ReplyAction="http://tempuri.org/ISignalsWebService/GetPathEntryResponse")]
        System.Threading.Tasks.Task<Dto.PathEntry> GetPathEntryAsync(Dto.Path path);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISignalsWebService/GetData", ReplyAction="http://tempuri.org/ISignalsWebService/GetDataResponse")]
        Dto.Datum[] GetData(int signalId, System.DateTime fromIncludedUtc, System.DateTime toExcludedUtc);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISignalsWebService/GetData", ReplyAction="http://tempuri.org/ISignalsWebService/GetDataResponse")]
        System.Threading.Tasks.Task<Dto.Datum[]> GetDataAsync(int signalId, System.DateTime fromIncludedUtc, System.DateTime toExcludedUtc);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISignalsWebService/SetData", ReplyAction="http://tempuri.org/ISignalsWebService/SetDataResponse")]
        void SetData(int signalId, Dto.Datum[] data);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISignalsWebService/SetData", ReplyAction="http://tempuri.org/ISignalsWebService/SetDataResponse")]
        System.Threading.Tasks.Task SetDataAsync(int signalId, Dto.Datum[] data);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISignalsWebService/GetMissingValuePolicy", ReplyAction="http://tempuri.org/ISignalsWebService/GetMissingValuePolicyResponse")]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(Dto.MissingValuePolicy.NoneQualityMissingValuePolicy))]
        Dto.MissingValuePolicy.MissingValuePolicy GetMissingValuePolicy(int signalId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISignalsWebService/GetMissingValuePolicy", ReplyAction="http://tempuri.org/ISignalsWebService/GetMissingValuePolicyResponse")]
        System.Threading.Tasks.Task<Dto.MissingValuePolicy.MissingValuePolicy> GetMissingValuePolicyAsync(int signalId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISignalsWebService/SetMissingValuePolicy", ReplyAction="http://tempuri.org/ISignalsWebService/SetMissingValuePolicyResponse")]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(Dto.MissingValuePolicy.NoneQualityMissingValuePolicy))]
        void SetMissingValuePolicy(int signalId, Dto.MissingValuePolicy.MissingValuePolicy missingValuePolicy);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISignalsWebService/SetMissingValuePolicy", ReplyAction="http://tempuri.org/ISignalsWebService/SetMissingValuePolicyResponse")]
        System.Threading.Tasks.Task SetMissingValuePolicyAsync(int signalId, Dto.MissingValuePolicy.MissingValuePolicy missingValuePolicy);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface ISignalsWebServiceChannel : SignalsIntegrationTests.WS.ISignalsWebService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class SignalsWebServiceClient : System.ServiceModel.ClientBase<SignalsIntegrationTests.WS.ISignalsWebService>, SignalsIntegrationTests.WS.ISignalsWebService {
        
        public SignalsWebServiceClient() {
        }
        
        public SignalsWebServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public SignalsWebServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public SignalsWebServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public SignalsWebServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public Dto.Signal Get(Dto.Path path) {
            return base.Channel.Get(path);
        }
        
        public System.Threading.Tasks.Task<Dto.Signal> GetAsync(Dto.Path path) {
            return base.Channel.GetAsync(path);
        }
        
        public Dto.Signal GetById(int signalId) {
            return base.Channel.GetById(signalId);
        }
        
        public System.Threading.Tasks.Task<Dto.Signal> GetByIdAsync(int signalId) {
            return base.Channel.GetByIdAsync(signalId);
        }
        
        public Dto.Signal Add(Dto.Signal signal) {
            return base.Channel.Add(signal);
        }
        
        public System.Threading.Tasks.Task<Dto.Signal> AddAsync(Dto.Signal signal) {
            return base.Channel.AddAsync(signal);
        }
        
        public Dto.PathEntry GetPathEntry(Dto.Path path) {
            return base.Channel.GetPathEntry(path);
        }
        
        public System.Threading.Tasks.Task<Dto.PathEntry> GetPathEntryAsync(Dto.Path path) {
            return base.Channel.GetPathEntryAsync(path);
        }
        
        public Dto.Datum[] GetData(int signalId, System.DateTime fromIncludedUtc, System.DateTime toExcludedUtc) {
            return base.Channel.GetData(signalId, fromIncludedUtc, toExcludedUtc);
        }
        
        public System.Threading.Tasks.Task<Dto.Datum[]> GetDataAsync(int signalId, System.DateTime fromIncludedUtc, System.DateTime toExcludedUtc) {
            return base.Channel.GetDataAsync(signalId, fromIncludedUtc, toExcludedUtc);
        }
        
        public void SetData(int signalId, Dto.Datum[] data) {
            base.Channel.SetData(signalId, data);
        }
        
        public System.Threading.Tasks.Task SetDataAsync(int signalId, Dto.Datum[] data) {
            return base.Channel.SetDataAsync(signalId, data);
        }
        
        public Dto.MissingValuePolicy.MissingValuePolicy GetMissingValuePolicy(int signalId) {
            return base.Channel.GetMissingValuePolicy(signalId);
        }
        
        public System.Threading.Tasks.Task<Dto.MissingValuePolicy.MissingValuePolicy> GetMissingValuePolicyAsync(int signalId) {
            return base.Channel.GetMissingValuePolicyAsync(signalId);
        }
        
        public void SetMissingValuePolicy(int signalId, Dto.MissingValuePolicy.MissingValuePolicy missingValuePolicy) {
            base.Channel.SetMissingValuePolicy(signalId, missingValuePolicy);
        }
        
        public System.Threading.Tasks.Task SetMissingValuePolicyAsync(int signalId, Dto.MissingValuePolicy.MissingValuePolicy missingValuePolicy) {
            return base.Channel.SetMissingValuePolicyAsync(signalId, missingValuePolicy);
        }
    }
}
