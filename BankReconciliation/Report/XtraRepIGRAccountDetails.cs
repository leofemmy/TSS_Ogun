﻿using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace BankReconciliation.Report
{
    public partial class XtraRepIGRAccountDetails : DevExpress.XtraReports.UI.XtraReport
    {
        public XtraRepIGRAccountDetails()
        {
            InitializeComponent();
        }

        private void xrPivotGrid1_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
          
        }

        private void XtraRepIGRAccountDetails_ParametersRequestSubmit(object sender, DevExpress.XtraReports.Parameters.ParametersRequestEventArgs e)
        {
            //this.xrPivotGrid1.Prefilter.CriteriaString = "[fieldPeriod]= e";
        }

    }
}
