﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SignalsIntegrationTests.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "14.0.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("..\\..\\..\\..\\Signals\\Hosting\\bin\\Debug\\Hosting.exe")]
        public string SignalsHostExecutablePath {
            get {
                return ((string)(this["SignalsHostExecutablePath"]));
            }
            set {
                this["SignalsHostExecutablePath"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("..\\..\\..\\..\\Signals\\DatabaseMaintenanceTool\\bin\\Debug\\DatabaseMaintenanceTool.exe" +
            "")]
        public string DatabaseMaintenanceToolExecutablePath {
            get {
                return ((string)(this["DatabaseMaintenanceToolExecutablePath"]));
            }
            set {
                this["DatabaseMaintenanceToolExecutablePath"] = value;
            }
        }
    }
}
