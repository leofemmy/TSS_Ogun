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

namespace BankReconciliation.ReconciliationUpload {
    using System;
    using System.Web.Services;
    using System.Diagnostics;
    using System.Web.Services.Protocols;
    using System.Xml.Serialization;
    using System.ComponentModel;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1038.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="REEMSSoap", Namespace="http://microsoft.com/webservices/")]
    public partial class REEMS : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback CollectReportsOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public REEMS() {
            this.Url = global::BankReconciliation.Properties.Settings.Default.BankReconciliation_ReconciliationUpload_REEMS;
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
        public event CollectReportsCompletedEventHandler CollectReportsCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://microsoft.com/webservices/CollectReports", RequestNamespace="http://microsoft.com/webservices/", ResponseNamespace="http://microsoft.com/webservices/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string CollectReports(
                    string pay_ref_num, 
                    string dep_slip_num, 
                    string pay_date, 
                    string payer_id, 
                    string payer_name, 
                    string tel, 
                    string rev_code, 
                    string description, 
                    decimal amount, 
                    string pay_method, 
                    string chq_num, 
                    string chq_val_date, 
                    string chq_bnk_code, 
                    string chq_bnk_name, 
                    string chq_status, 
                    string chq_rtn_date, 
                    string agency_name, 
                    string agency_code, 
                    string bank_code, 
                    string bank_name, 
                    string branch_code, 
                    string branch_name, 
                    string zone_code, 
                    string zone_name, 
                    string tco_code, 
                    string tco_name, 
                    string receipt_number, 
                    string payer_address, 
                    string user, 
                    string UCC, 
                    string PCC) {
            object[] results = this.Invoke("CollectReports", new object[] {
                        pay_ref_num,
                        dep_slip_num,
                        pay_date,
                        payer_id,
                        payer_name,
                        tel,
                        rev_code,
                        description,
                        amount,
                        pay_method,
                        chq_num,
                        chq_val_date,
                        chq_bnk_code,
                        chq_bnk_name,
                        chq_status,
                        chq_rtn_date,
                        agency_name,
                        agency_code,
                        bank_code,
                        bank_name,
                        branch_code,
                        branch_name,
                        zone_code,
                        zone_name,
                        tco_code,
                        tco_name,
                        receipt_number,
                        payer_address,
                        user,
                        UCC,
                        PCC});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void CollectReportsAsync(
                    string pay_ref_num, 
                    string dep_slip_num, 
                    string pay_date, 
                    string payer_id, 
                    string payer_name, 
                    string tel, 
                    string rev_code, 
                    string description, 
                    decimal amount, 
                    string pay_method, 
                    string chq_num, 
                    string chq_val_date, 
                    string chq_bnk_code, 
                    string chq_bnk_name, 
                    string chq_status, 
                    string chq_rtn_date, 
                    string agency_name, 
                    string agency_code, 
                    string bank_code, 
                    string bank_name, 
                    string branch_code, 
                    string branch_name, 
                    string zone_code, 
                    string zone_name, 
                    string tco_code, 
                    string tco_name, 
                    string receipt_number, 
                    string payer_address, 
                    string user, 
                    string UCC, 
                    string PCC) {
            this.CollectReportsAsync(pay_ref_num, dep_slip_num, pay_date, payer_id, payer_name, tel, rev_code, description, amount, pay_method, chq_num, chq_val_date, chq_bnk_code, chq_bnk_name, chq_status, chq_rtn_date, agency_name, agency_code, bank_code, bank_name, branch_code, branch_name, zone_code, zone_name, tco_code, tco_name, receipt_number, payer_address, user, UCC, PCC, null);
        }
        
        /// <remarks/>
        public void CollectReportsAsync(
                    string pay_ref_num, 
                    string dep_slip_num, 
                    string pay_date, 
                    string payer_id, 
                    string payer_name, 
                    string tel, 
                    string rev_code, 
                    string description, 
                    decimal amount, 
                    string pay_method, 
                    string chq_num, 
                    string chq_val_date, 
                    string chq_bnk_code, 
                    string chq_bnk_name, 
                    string chq_status, 
                    string chq_rtn_date, 
                    string agency_name, 
                    string agency_code, 
                    string bank_code, 
                    string bank_name, 
                    string branch_code, 
                    string branch_name, 
                    string zone_code, 
                    string zone_name, 
                    string tco_code, 
                    string tco_name, 
                    string receipt_number, 
                    string payer_address, 
                    string user, 
                    string UCC, 
                    string PCC, 
                    object userState) {
            if ((this.CollectReportsOperationCompleted == null)) {
                this.CollectReportsOperationCompleted = new System.Threading.SendOrPostCallback(this.OnCollectReportsOperationCompleted);
            }
            this.InvokeAsync("CollectReports", new object[] {
                        pay_ref_num,
                        dep_slip_num,
                        pay_date,
                        payer_id,
                        payer_name,
                        tel,
                        rev_code,
                        description,
                        amount,
                        pay_method,
                        chq_num,
                        chq_val_date,
                        chq_bnk_code,
                        chq_bnk_name,
                        chq_status,
                        chq_rtn_date,
                        agency_name,
                        agency_code,
                        bank_code,
                        bank_name,
                        branch_code,
                        branch_name,
                        zone_code,
                        zone_name,
                        tco_code,
                        tco_name,
                        receipt_number,
                        payer_address,
                        user,
                        UCC,
                        PCC}, this.CollectReportsOperationCompleted, userState);
        }
        
        private void OnCollectReportsOperationCompleted(object arg) {
            if ((this.CollectReportsCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.CollectReportsCompleted(this, new CollectReportsCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1038.0")]
    public delegate void CollectReportsCompletedEventHandler(object sender, CollectReportsCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1038.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class CollectReportsCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal CollectReportsCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
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