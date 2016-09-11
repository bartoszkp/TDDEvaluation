﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ExampleSignalClient.Signals {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="Path", Namespace="http://schemas.datacontract.org/2004/07/Dto")]
    [System.SerializableAttribute()]
    public partial class Path : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string[] ComponentsField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string[] Components {
            get {
                return this.ComponentsField;
            }
            set {
                if ((object.ReferenceEquals(this.ComponentsField, value) != true)) {
                    this.ComponentsField = value;
                    this.RaisePropertyChanged("Components");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="Signal", Namespace="http://schemas.datacontract.org/2004/07/Dto")]
    [System.SerializableAttribute()]
    public partial class Signal : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private ExampleSignalClient.Signals.DataType DataTypeField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private ExampleSignalClient.Signals.Granularity GranularityField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Nullable<int> IdField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private ExampleSignalClient.Signals.Path PathField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public ExampleSignalClient.Signals.DataType DataType {
            get {
                return this.DataTypeField;
            }
            set {
                if ((this.DataTypeField.Equals(value) != true)) {
                    this.DataTypeField = value;
                    this.RaisePropertyChanged("DataType");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public ExampleSignalClient.Signals.Granularity Granularity {
            get {
                return this.GranularityField;
            }
            set {
                if ((this.GranularityField.Equals(value) != true)) {
                    this.GranularityField = value;
                    this.RaisePropertyChanged("Granularity");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Nullable<int> Id {
            get {
                return this.IdField;
            }
            set {
                if ((this.IdField.Equals(value) != true)) {
                    this.IdField = value;
                    this.RaisePropertyChanged("Id");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public ExampleSignalClient.Signals.Path Path {
            get {
                return this.PathField;
            }
            set {
                if ((object.ReferenceEquals(this.PathField, value) != true)) {
                    this.PathField = value;
                    this.RaisePropertyChanged("Path");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="DataType", Namespace="http://schemas.datacontract.org/2004/07/Dto")]
    public enum DataType : int {
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Boolean = 0,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Integer = 1,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Double = 2,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Decimal = 3,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        String = 4,
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="Granularity", Namespace="http://schemas.datacontract.org/2004/07/Dto")]
    public enum Granularity : int {
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Second = 0,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Minute = 1,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Hour = 2,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Day = 3,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Week = 4,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Month = 5,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Year = 6,
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="PathEntry", Namespace="http://schemas.datacontract.org/2004/07/Dto")]
    [System.SerializableAttribute()]
    public partial class PathEntry : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private ExampleSignalClient.Signals.Signal[] SignalsField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private ExampleSignalClient.Signals.Path[] SubPathsField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public ExampleSignalClient.Signals.Signal[] Signals {
            get {
                return this.SignalsField;
            }
            set {
                if ((object.ReferenceEquals(this.SignalsField, value) != true)) {
                    this.SignalsField = value;
                    this.RaisePropertyChanged("Signals");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public ExampleSignalClient.Signals.Path[] SubPaths {
            get {
                return this.SubPathsField;
            }
            set {
                if ((object.ReferenceEquals(this.SubPathsField, value) != true)) {
                    this.SubPathsField = value;
                    this.RaisePropertyChanged("SubPaths");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="Datum", Namespace="http://schemas.datacontract.org/2004/07/Dto")]
    [System.SerializableAttribute()]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(ExampleSignalClient.Signals.Path))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(ExampleSignalClient.Signals.Signal))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(ExampleSignalClient.Signals.DataType))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(ExampleSignalClient.Signals.Granularity))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(ExampleSignalClient.Signals.PathEntry))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(ExampleSignalClient.Signals.Signal[]))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(ExampleSignalClient.Signals.Path[]))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(ExampleSignalClient.Signals.Datum[]))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(ExampleSignalClient.Signals.Quality))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(ExampleSignalClient.Signals.MissingValuePolicy))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(ExampleSignalClient.Signals.NoneQualityMissingValuePolicy))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(ExampleSignalClient.Signals.SpecificValueMissingValuePolicy))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(ExampleSignalClient.Signals.ZeroOrderMissingValuePolicy))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(ExampleSignalClient.Signals.FirstOrderMissingValuePolicy))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(ExampleSignalClient.Signals.ShadowMissingValuePolicy))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(string[]))]
    public partial class Datum : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private ExampleSignalClient.Signals.Quality QualityField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.DateTime TimestampField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private object ValueField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public ExampleSignalClient.Signals.Quality Quality {
            get {
                return this.QualityField;
            }
            set {
                if ((this.QualityField.Equals(value) != true)) {
                    this.QualityField = value;
                    this.RaisePropertyChanged("Quality");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.DateTime Timestamp {
            get {
                return this.TimestampField;
            }
            set {
                if ((this.TimestampField.Equals(value) != true)) {
                    this.TimestampField = value;
                    this.RaisePropertyChanged("Timestamp");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public object Value {
            get {
                return this.ValueField;
            }
            set {
                if ((object.ReferenceEquals(this.ValueField, value) != true)) {
                    this.ValueField = value;
                    this.RaisePropertyChanged("Value");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="Quality", Namespace="http://schemas.datacontract.org/2004/07/Dto")]
    public enum Quality : int {
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        None = 0,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Good = 1,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Fair = 2,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Poor = 3,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Bad = 4,
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="MissingValuePolicy", Namespace="http://schemas.datacontract.org/2004/07/Dto.MissingValuePolicy")]
    [System.SerializableAttribute()]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(ExampleSignalClient.Signals.NoneQualityMissingValuePolicy))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(ExampleSignalClient.Signals.SpecificValueMissingValuePolicy))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(ExampleSignalClient.Signals.ZeroOrderMissingValuePolicy))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(ExampleSignalClient.Signals.FirstOrderMissingValuePolicy))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(ExampleSignalClient.Signals.ShadowMissingValuePolicy))]
    public partial class MissingValuePolicy : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private ExampleSignalClient.Signals.DataType DataTypeField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Nullable<int> IdField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private ExampleSignalClient.Signals.Signal SignalField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public ExampleSignalClient.Signals.DataType DataType {
            get {
                return this.DataTypeField;
            }
            set {
                if ((this.DataTypeField.Equals(value) != true)) {
                    this.DataTypeField = value;
                    this.RaisePropertyChanged("DataType");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Nullable<int> Id {
            get {
                return this.IdField;
            }
            set {
                if ((this.IdField.Equals(value) != true)) {
                    this.IdField = value;
                    this.RaisePropertyChanged("Id");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public ExampleSignalClient.Signals.Signal Signal {
            get {
                return this.SignalField;
            }
            set {
                if ((object.ReferenceEquals(this.SignalField, value) != true)) {
                    this.SignalField = value;
                    this.RaisePropertyChanged("Signal");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="NoneQualityMissingValuePolicy", Namespace="http://schemas.datacontract.org/2004/07/Dto.MissingValuePolicy")]
    [System.SerializableAttribute()]
    public partial class NoneQualityMissingValuePolicy : ExampleSignalClient.Signals.MissingValuePolicy {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="SpecificValueMissingValuePolicy", Namespace="http://schemas.datacontract.org/2004/07/Dto.MissingValuePolicy")]
    [System.SerializableAttribute()]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(ExampleSignalClient.Signals.Path))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(ExampleSignalClient.Signals.Signal))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(ExampleSignalClient.Signals.DataType))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(ExampleSignalClient.Signals.Granularity))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(ExampleSignalClient.Signals.PathEntry))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(ExampleSignalClient.Signals.Signal[]))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(ExampleSignalClient.Signals.Path[]))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(ExampleSignalClient.Signals.Datum[]))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(ExampleSignalClient.Signals.Datum))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(ExampleSignalClient.Signals.Quality))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(ExampleSignalClient.Signals.MissingValuePolicy))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(ExampleSignalClient.Signals.NoneQualityMissingValuePolicy))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(ExampleSignalClient.Signals.ZeroOrderMissingValuePolicy))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(ExampleSignalClient.Signals.FirstOrderMissingValuePolicy))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(ExampleSignalClient.Signals.ShadowMissingValuePolicy))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(string[]))]
    public partial class SpecificValueMissingValuePolicy : ExampleSignalClient.Signals.MissingValuePolicy {
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private ExampleSignalClient.Signals.Quality QualityField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private object ValueField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public ExampleSignalClient.Signals.Quality Quality {
            get {
                return this.QualityField;
            }
            set {
                if ((this.QualityField.Equals(value) != true)) {
                    this.QualityField = value;
                    this.RaisePropertyChanged("Quality");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public object Value {
            get {
                return this.ValueField;
            }
            set {
                if ((object.ReferenceEquals(this.ValueField, value) != true)) {
                    this.ValueField = value;
                    this.RaisePropertyChanged("Value");
                }
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="ZeroOrderMissingValuePolicy", Namespace="http://schemas.datacontract.org/2004/07/Dto.MissingValuePolicy")]
    [System.SerializableAttribute()]
    public partial class ZeroOrderMissingValuePolicy : ExampleSignalClient.Signals.MissingValuePolicy {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="FirstOrderMissingValuePolicy", Namespace="http://schemas.datacontract.org/2004/07/Dto.MissingValuePolicy")]
    [System.SerializableAttribute()]
    public partial class FirstOrderMissingValuePolicy : ExampleSignalClient.Signals.MissingValuePolicy {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="ShadowMissingValuePolicy", Namespace="http://schemas.datacontract.org/2004/07/Dto.MissingValuePolicy")]
    [System.SerializableAttribute()]
    public partial class ShadowMissingValuePolicy : ExampleSignalClient.Signals.MissingValuePolicy {
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private ExampleSignalClient.Signals.Signal ShadowSignalField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public ExampleSignalClient.Signals.Signal ShadowSignal {
            get {
                return this.ShadowSignalField;
            }
            set {
                if ((object.ReferenceEquals(this.ShadowSignalField, value) != true)) {
                    this.ShadowSignalField = value;
                    this.RaisePropertyChanged("ShadowSignal");
                }
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="Signals.ISignalsWebService")]
    public interface ISignalsWebService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISignalsWebService/Get", ReplyAction="http://tempuri.org/ISignalsWebService/GetResponse")]
        ExampleSignalClient.Signals.Signal Get(ExampleSignalClient.Signals.Path path);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISignalsWebService/Get", ReplyAction="http://tempuri.org/ISignalsWebService/GetResponse")]
        System.Threading.Tasks.Task<ExampleSignalClient.Signals.Signal> GetAsync(ExampleSignalClient.Signals.Path path);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISignalsWebService/GetById", ReplyAction="http://tempuri.org/ISignalsWebService/GetByIdResponse")]
        ExampleSignalClient.Signals.Signal GetById(int signalId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISignalsWebService/GetById", ReplyAction="http://tempuri.org/ISignalsWebService/GetByIdResponse")]
        System.Threading.Tasks.Task<ExampleSignalClient.Signals.Signal> GetByIdAsync(int signalId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISignalsWebService/Add", ReplyAction="http://tempuri.org/ISignalsWebService/AddResponse")]
        ExampleSignalClient.Signals.Signal Add(ExampleSignalClient.Signals.Signal signal);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISignalsWebService/Add", ReplyAction="http://tempuri.org/ISignalsWebService/AddResponse")]
        System.Threading.Tasks.Task<ExampleSignalClient.Signals.Signal> AddAsync(ExampleSignalClient.Signals.Signal signal);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISignalsWebService/Delete", ReplyAction="http://tempuri.org/ISignalsWebService/DeleteResponse")]
        void Delete(int signalId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISignalsWebService/Delete", ReplyAction="http://tempuri.org/ISignalsWebService/DeleteResponse")]
        System.Threading.Tasks.Task DeleteAsync(int signalId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISignalsWebService/GetPathEntry", ReplyAction="http://tempuri.org/ISignalsWebService/GetPathEntryResponse")]
        ExampleSignalClient.Signals.PathEntry GetPathEntry(ExampleSignalClient.Signals.Path path);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISignalsWebService/GetPathEntry", ReplyAction="http://tempuri.org/ISignalsWebService/GetPathEntryResponse")]
        System.Threading.Tasks.Task<ExampleSignalClient.Signals.PathEntry> GetPathEntryAsync(ExampleSignalClient.Signals.Path path);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISignalsWebService/GetData", ReplyAction="http://tempuri.org/ISignalsWebService/GetDataResponse")]
        ExampleSignalClient.Signals.Datum[] GetData(int signalId, System.DateTime fromIncludedUtc, System.DateTime toExcludedUtc);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISignalsWebService/GetData", ReplyAction="http://tempuri.org/ISignalsWebService/GetDataResponse")]
        System.Threading.Tasks.Task<ExampleSignalClient.Signals.Datum[]> GetDataAsync(int signalId, System.DateTime fromIncludedUtc, System.DateTime toExcludedUtc);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISignalsWebService/GetCoarseData", ReplyAction="http://tempuri.org/ISignalsWebService/GetCoarseDataResponse")]
        ExampleSignalClient.Signals.Datum[] GetCoarseData(int signalId, ExampleSignalClient.Signals.Granularity granularity, System.DateTime fromIncludedUtc, System.DateTime toExcludedUtc);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISignalsWebService/GetCoarseData", ReplyAction="http://tempuri.org/ISignalsWebService/GetCoarseDataResponse")]
        System.Threading.Tasks.Task<ExampleSignalClient.Signals.Datum[]> GetCoarseDataAsync(int signalId, ExampleSignalClient.Signals.Granularity granularity, System.DateTime fromIncludedUtc, System.DateTime toExcludedUtc);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISignalsWebService/SetData", ReplyAction="http://tempuri.org/ISignalsWebService/SetDataResponse")]
        void SetData(int signalId, ExampleSignalClient.Signals.Datum[] data);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISignalsWebService/SetData", ReplyAction="http://tempuri.org/ISignalsWebService/SetDataResponse")]
        System.Threading.Tasks.Task SetDataAsync(int signalId, ExampleSignalClient.Signals.Datum[] data);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISignalsWebService/GetMissingValuePolicy", ReplyAction="http://tempuri.org/ISignalsWebService/GetMissingValuePolicyResponse")]
        ExampleSignalClient.Signals.MissingValuePolicy GetMissingValuePolicy(int signalId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISignalsWebService/GetMissingValuePolicy", ReplyAction="http://tempuri.org/ISignalsWebService/GetMissingValuePolicyResponse")]
        System.Threading.Tasks.Task<ExampleSignalClient.Signals.MissingValuePolicy> GetMissingValuePolicyAsync(int signalId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISignalsWebService/SetMissingValuePolicy", ReplyAction="http://tempuri.org/ISignalsWebService/SetMissingValuePolicyResponse")]
        void SetMissingValuePolicy(int signalId, ExampleSignalClient.Signals.MissingValuePolicy policy);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISignalsWebService/SetMissingValuePolicy", ReplyAction="http://tempuri.org/ISignalsWebService/SetMissingValuePolicyResponse")]
        System.Threading.Tasks.Task SetMissingValuePolicyAsync(int signalId, ExampleSignalClient.Signals.MissingValuePolicy policy);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface ISignalsWebServiceChannel : ExampleSignalClient.Signals.ISignalsWebService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class SignalsWebServiceClient : System.ServiceModel.ClientBase<ExampleSignalClient.Signals.ISignalsWebService>, ExampleSignalClient.Signals.ISignalsWebService {
        
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
        
        public ExampleSignalClient.Signals.Signal Get(ExampleSignalClient.Signals.Path path) {
            return base.Channel.Get(path);
        }
        
        public System.Threading.Tasks.Task<ExampleSignalClient.Signals.Signal> GetAsync(ExampleSignalClient.Signals.Path path) {
            return base.Channel.GetAsync(path);
        }
        
        public ExampleSignalClient.Signals.Signal GetById(int signalId) {
            return base.Channel.GetById(signalId);
        }
        
        public System.Threading.Tasks.Task<ExampleSignalClient.Signals.Signal> GetByIdAsync(int signalId) {
            return base.Channel.GetByIdAsync(signalId);
        }
        
        public ExampleSignalClient.Signals.Signal Add(ExampleSignalClient.Signals.Signal signal) {
            return base.Channel.Add(signal);
        }
        
        public System.Threading.Tasks.Task<ExampleSignalClient.Signals.Signal> AddAsync(ExampleSignalClient.Signals.Signal signal) {
            return base.Channel.AddAsync(signal);
        }
        
        public void Delete(int signalId) {
            base.Channel.Delete(signalId);
        }
        
        public System.Threading.Tasks.Task DeleteAsync(int signalId) {
            return base.Channel.DeleteAsync(signalId);
        }
        
        public ExampleSignalClient.Signals.PathEntry GetPathEntry(ExampleSignalClient.Signals.Path path) {
            return base.Channel.GetPathEntry(path);
        }
        
        public System.Threading.Tasks.Task<ExampleSignalClient.Signals.PathEntry> GetPathEntryAsync(ExampleSignalClient.Signals.Path path) {
            return base.Channel.GetPathEntryAsync(path);
        }
        
        public ExampleSignalClient.Signals.Datum[] GetData(int signalId, System.DateTime fromIncludedUtc, System.DateTime toExcludedUtc) {
            return base.Channel.GetData(signalId, fromIncludedUtc, toExcludedUtc);
        }
        
        public System.Threading.Tasks.Task<ExampleSignalClient.Signals.Datum[]> GetDataAsync(int signalId, System.DateTime fromIncludedUtc, System.DateTime toExcludedUtc) {
            return base.Channel.GetDataAsync(signalId, fromIncludedUtc, toExcludedUtc);
        }
        
        public ExampleSignalClient.Signals.Datum[] GetCoarseData(int signalId, ExampleSignalClient.Signals.Granularity granularity, System.DateTime fromIncludedUtc, System.DateTime toExcludedUtc) {
            return base.Channel.GetCoarseData(signalId, granularity, fromIncludedUtc, toExcludedUtc);
        }
        
        public System.Threading.Tasks.Task<ExampleSignalClient.Signals.Datum[]> GetCoarseDataAsync(int signalId, ExampleSignalClient.Signals.Granularity granularity, System.DateTime fromIncludedUtc, System.DateTime toExcludedUtc) {
            return base.Channel.GetCoarseDataAsync(signalId, granularity, fromIncludedUtc, toExcludedUtc);
        }
        
        public void SetData(int signalId, ExampleSignalClient.Signals.Datum[] data) {
            base.Channel.SetData(signalId, data);
        }
        
        public System.Threading.Tasks.Task SetDataAsync(int signalId, ExampleSignalClient.Signals.Datum[] data) {
            return base.Channel.SetDataAsync(signalId, data);
        }
        
        public ExampleSignalClient.Signals.MissingValuePolicy GetMissingValuePolicy(int signalId) {
            return base.Channel.GetMissingValuePolicy(signalId);
        }
        
        public System.Threading.Tasks.Task<ExampleSignalClient.Signals.MissingValuePolicy> GetMissingValuePolicyAsync(int signalId) {
            return base.Channel.GetMissingValuePolicyAsync(signalId);
        }
        
        public void SetMissingValuePolicy(int signalId, ExampleSignalClient.Signals.MissingValuePolicy policy) {
            base.Channel.SetMissingValuePolicy(signalId, policy);
        }
        
        public System.Threading.Tasks.Task SetMissingValuePolicyAsync(int signalId, ExampleSignalClient.Signals.MissingValuePolicy policy) {
            return base.Channel.SetMissingValuePolicyAsync(signalId, policy);
        }
    }
}
