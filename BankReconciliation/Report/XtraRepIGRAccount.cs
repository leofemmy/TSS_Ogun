using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace BankReconciliation.Report
{
    public partial class XtraRepIGRAccount : DevExpress.XtraReports.UI.XtraReport
    {
        public XtraRepIGRAccount()
        {
            InitializeComponent();
        }

        private void XtraRepIGRAccount_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            //XRLabel lblcr = new XRLabel();
            //XRBinding binding = new XRBinding("Text", null, "ViewIGRAccountDetails.Dr");
            //XRBinding bindings = new XRBinding("Text", null, "ViewIGRAccountDetails.Cr");

            //XRSummary summdb = new XRSummary();

            //summdb.Func = SummaryFunc.Sum;
            //summdb.Running = SummaryRunning.None;
            //summdb.IgnoreNullValues = true;
            //summdb.FormatString = "{0:n2}";

            //if (bindings== null)
            //{
            //    //lblcr.DataBindings.Add(binding);
            //    xrLabel6.DataBindings.Add(binding);
            //    xrLabel6.Summary = summdb;
            //    //xrLabel6.FormattingRules=
            //}
            //else if (binding == null)
            //{
            //    //lblcr.DataBindings.Add(bindings);
            //    xrLabel6.DataBindings.Add(bindings);
            //    xrLabel6.Summary = summdb;

            //}
        }

    }
}
