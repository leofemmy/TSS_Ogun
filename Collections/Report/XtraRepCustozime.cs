using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace Collection.Report
{
    public partial class XtraRepCustozime : DevExpress.XtraReports.UI.XtraReport
    {
        public XtraRepCustozime()
        {
            InitializeComponent();

            xrLabel43.BeforePrint += xrLabel43_BeforePrint;
        }

        void xrLabel43_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            string testrval = string.Format("<{0}><{1}><{2}>", this.GetCurrentColumnValue("PayerName").ToString(), string.Format("{0:n}", this.GetCurrentColumnValue("Amount")), this.GetCurrentColumnValue("EReceipts").ToString());

            xrLabel43.Text = testrval;
        }

    }
}
