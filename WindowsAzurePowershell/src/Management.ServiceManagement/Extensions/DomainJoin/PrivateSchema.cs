﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18213
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by xsd, Version=4.0.30319.1.
// 
namespace Microsoft.WindowsAzure.Management.ServiceManagement.Extensions.DomainJoin {
    using System.Xml.Serialization;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="", IsNullable=false)]
    public partial class PrivateConfig {
        
        private string passwordField;
        
        private string localPasswordField;
        
        private string unjoinDomainPasswordField;
        
        /// <remarks/>
        public string Password {
            get {
                return this.passwordField;
            }
            set {
                this.passwordField = value;
            }
        }
        
        /// <remarks/>
        public string LocalPassword {
            get {
                return this.localPasswordField;
            }
            set {
                this.localPasswordField = value;
            }
        }
        
        /// <remarks/>
        public string UnjoinDomainPassword {
            get {
                return this.unjoinDomainPasswordField;
            }
            set {
                this.unjoinDomainPasswordField = value;
            }
        }
    }
}