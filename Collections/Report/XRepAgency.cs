using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;
using System.Windows.Forms;

namespace Collection.Report
{
    public partial class XRepAgency : DevExpress.XtraReports.UI.XtraReport
    {
        public XRepAgency()
        {
            InitializeComponent();
        }

        private void xrLabel1_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            xrLabel1.ForeColor = Color.Blue;

            ((XRLabel)sender).Tag = GetCurrentRow();
        }

        private void xrLabel1_PreviewClick(object sender, PreviewMouseEventArgs e)
        {
            XRepAgencyDetail report = new XRepAgencyDetail();

            report.paramAgencyCode.Value = ((DataRowView)e.Brick.Value).Row["AgencyCode"];

            report.paramBeginDate.Value = this.paramBeginDate.Value;

            report.paramEndDate.Value = this.paramEndDate.Value;

            report.ShowPreviewDialog();
        }

        private void xrLabel1_PreviewMouseMove(object sender, PreviewMouseEventArgs e)
        {
            Cursor.Current = Cursors.Hand;
        }

    }
}
