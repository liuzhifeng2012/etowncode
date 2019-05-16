﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.1022
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

// 
// 此源代码是由 Microsoft.VSDesigner 4.0.30319.1022 版自动生成。
// 
#pragma warning disable 1591

namespace ETS2.VAS.Service.WebReference1 {
    using System;
    using System.Web.Services;
    using System.Diagnostics;
    using System.Web.Services.Protocols;
    using System.ComponentModel;
    using System.Xml.Serialization;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="ITicketServiceHttpBinding", Namespace="http://service.ts.bjlyw.com")]
    public partial class ITicketService : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback getProductInfoOperationCompleted;
        
        private System.Threading.SendOrPostCallback getEleInterfaceOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public ITicketService() {
            this.Url = global::ETS2.VAS.Service.Properties.Settings.Default.ETS2_VAS_Service_WebReference1_ITicketService;
            if ((this.IsLocalFileSystemWebService(this.Url) == true)) {
                this.UseDefaultCredentials = true;
                this.useDefaultCredentialsSetExplicitly = false;
            }
            else {
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        public new string Url {
            get {
                return base.Url;
            }
            set {
                if ((((this.IsLocalFileSystemWebService(base.Url) == true) 
                            && (this.useDefaultCredentialsSetExplicitly == false)) 
                            && (this.IsLocalFileSystemWebService(value) == false))) {
                    base.UseDefaultCredentials = false;
                }
                base.Url = value;
            }
        }
        
        public new bool UseDefaultCredentials {
            get {
                return base.UseDefaultCredentials;
            }
            set {
                base.UseDefaultCredentials = value;
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        /// <remarks/>
        public event getProductInfoCompletedEventHandler getProductInfoCompleted;
        
        /// <remarks/>
        public event getEleInterfaceCompletedEventHandler getEleInterfaceCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("", RequestNamespace="http://service.ts.bjlyw.com", ResponseNamespace="http://service.ts.bjlyw.com", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("out", IsNullable=true)]
        public string getProductInfo([System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] string organization, [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] string xml) {
            object[] results = this.Invoke("getProductInfo", new object[] {
                        organization,
                        xml});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void getProductInfoAsync(string organization, string xml) {
            this.getProductInfoAsync(organization, xml, null);
        }
        
        /// <remarks/>
        public void getProductInfoAsync(string organization, string xml, object userState) {
            if ((this.getProductInfoOperationCompleted == null)) {
                this.getProductInfoOperationCompleted = new System.Threading.SendOrPostCallback(this.OngetProductInfoOperationCompleted);
            }
            this.InvokeAsync("getProductInfo", new object[] {
                        organization,
                        xml}, this.getProductInfoOperationCompleted, userState);
        }
        
        private void OngetProductInfoOperationCompleted(object arg) {
            if ((this.getProductInfoCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.getProductInfoCompleted(this, new getProductInfoCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("", RequestNamespace="http://service.ts.bjlyw.com", ResponseNamespace="http://service.ts.bjlyw.com", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("out", IsNullable=true)]
        public string getEleInterface([System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] string organization, [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] string xml) {
            object[] results = this.Invoke("getEleInterface", new object[] {
                        organization,
                        xml});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void getEleInterfaceAsync(string organization, string xml) {
            this.getEleInterfaceAsync(organization, xml, null);
        }
        
        /// <remarks/>
        public void getEleInterfaceAsync(string organization, string xml, object userState) {
            if ((this.getEleInterfaceOperationCompleted == null)) {
                this.getEleInterfaceOperationCompleted = new System.Threading.SendOrPostCallback(this.OngetEleInterfaceOperationCompleted);
            }
            this.InvokeAsync("getEleInterface", new object[] {
                        organization,
                        xml}, this.getEleInterfaceOperationCompleted, userState);
        }
        
        private void OngetEleInterfaceOperationCompleted(object arg) {
            if ((this.getEleInterfaceCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.getEleInterfaceCompleted(this, new getEleInterfaceCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        public new void CancelAsync(object userState) {
            base.CancelAsync(userState);
        }
        
        private bool IsLocalFileSystemWebService(string url) {
            if (((url == null) 
                        || (url == string.Empty))) {
                return false;
            }
            System.Uri wsUri = new System.Uri(url);
            if (((wsUri.Port >= 1024) 
                        && (string.Compare(wsUri.Host, "localHost", System.StringComparison.OrdinalIgnoreCase) == 0))) {
                return true;
            }
            return false;
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    public delegate void getProductInfoCompletedEventHandler(object sender, getProductInfoCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class getProductInfoCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal getProductInfoCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    public delegate void getEleInterfaceCompletedEventHandler(object sender, getEleInterfaceCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class getEleInterfaceCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal getEleInterfaceCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591