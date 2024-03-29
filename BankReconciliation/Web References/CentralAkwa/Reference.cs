﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by Microsoft.VSDesigner, Version 4.0.30319.42000.
// 
#pragma warning disable 1591

namespace BankReconciliation.CentralAkwa {
    using System.Diagnostics;
    using System;
    using System.Xml.Serialization;
    using System.ComponentModel;
    using System.Web.Services.Protocols;
    using System.Web.Services;
    using System.Data;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.9032.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="CollectionManagerSoap", Namespace="http://microsoft.com/webservices/")]
    public partial class CollectionManager : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback UploadDataOperationCompleted;
        
        private System.Threading.SendOrPostCallback DelistFetchOperationCompleted;
        
        private System.Threading.SendOrPostCallback DelistUpdateOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public CollectionManager() {
            this.Url = global::BankReconciliation.Properties.Settings.Default.BankReconciliation_CentralAkwa_CollectionManager;
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
        public event UploadDataCompletedEventHandler UploadDataCompleted;
        
        /// <remarks/>
        public event DelistFetchCompletedEventHandler DelistFetchCompleted;
        
        /// <remarks/>
        public event DelistUpdateCompletedEventHandler DelistUpdateCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://microsoft.com/webservices/UploadData", RequestNamespace="http://microsoft.com/webservices/", ResponseNamespace="http://microsoft.com/webservices/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public System.Data.DataSet UploadData(System.Data.DataSet dataSet, string UCC, string PCC) {
            object[] results = this.Invoke("UploadData", new object[] {
                        dataSet,
                        UCC,
                        PCC});
            return ((System.Data.DataSet)(results[0]));
        }
        
        /// <remarks/>
        public void UploadDataAsync(System.Data.DataSet dataSet, string UCC, string PCC) {
            this.UploadDataAsync(dataSet, UCC, PCC, null);
        }
        
        /// <remarks/>
        public void UploadDataAsync(System.Data.DataSet dataSet, string UCC, string PCC, object userState) {
            if ((this.UploadDataOperationCompleted == null)) {
                this.UploadDataOperationCompleted = new System.Threading.SendOrPostCallback(this.OnUploadDataOperationCompleted);
            }
            this.InvokeAsync("UploadData", new object[] {
                        dataSet,
                        UCC,
                        PCC}, this.UploadDataOperationCompleted, userState);
        }
        
        private void OnUploadDataOperationCompleted(object arg) {
            if ((this.UploadDataCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.UploadDataCompleted(this, new UploadDataCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://microsoft.com/webservices/DelistFetch", RequestNamespace="http://microsoft.com/webservices/", ResponseNamespace="http://microsoft.com/webservices/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public System.Data.DataSet DelistFetch(string payref) {
            object[] results = this.Invoke("DelistFetch", new object[] {
                        payref});
            return ((System.Data.DataSet)(results[0]));
        }
        
        /// <remarks/>
        public void DelistFetchAsync(string payref) {
            this.DelistFetchAsync(payref, null);
        }
        
        /// <remarks/>
        public void DelistFetchAsync(string payref, object userState) {
            if ((this.DelistFetchOperationCompleted == null)) {
                this.DelistFetchOperationCompleted = new System.Threading.SendOrPostCallback(this.OnDelistFetchOperationCompleted);
            }
            this.InvokeAsync("DelistFetch", new object[] {
                        payref}, this.DelistFetchOperationCompleted, userState);
        }
        
        private void OnDelistFetchOperationCompleted(object arg) {
            if ((this.DelistFetchCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.DelistFetchCompleted(this, new DelistFetchCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://microsoft.com/webservices/DelistUpdate", RequestNamespace="http://microsoft.com/webservices/", ResponseNamespace="http://microsoft.com/webservices/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public System.Data.DataSet DelistUpdate(System.Data.DataSet ds) {
            object[] results = this.Invoke("DelistUpdate", new object[] {
                        ds});
            return ((System.Data.DataSet)(results[0]));
        }
        
        /// <remarks/>
        public void DelistUpdateAsync(System.Data.DataSet ds) {
            this.DelistUpdateAsync(ds, null);
        }
        
        /// <remarks/>
        public void DelistUpdateAsync(System.Data.DataSet ds, object userState) {
            if ((this.DelistUpdateOperationCompleted == null)) {
                this.DelistUpdateOperationCompleted = new System.Threading.SendOrPostCallback(this.OnDelistUpdateOperationCompleted);
            }
            this.InvokeAsync("DelistUpdate", new object[] {
                        ds}, this.DelistUpdateOperationCompleted, userState);
        }
        
        private void OnDelistUpdateOperationCompleted(object arg) {
            if ((this.DelistUpdateCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.DelistUpdateCompleted(this, new DelistUpdateCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.9032.0")]
    public delegate void UploadDataCompletedEventHandler(object sender, UploadDataCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.9032.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class UploadDataCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal UploadDataCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public System.Data.DataSet Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((System.Data.DataSet)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.9032.0")]
    public delegate void DelistFetchCompletedEventHandler(object sender, DelistFetchCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.9032.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class DelistFetchCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal DelistFetchCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public System.Data.DataSet Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((System.Data.DataSet)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.9032.0")]
    public delegate void DelistUpdateCompletedEventHandler(object sender, DelistUpdateCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.9032.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class DelistUpdateCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal DelistUpdateCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public System.Data.DataSet Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((System.Data.DataSet)(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591