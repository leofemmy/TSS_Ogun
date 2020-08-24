using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;
using System.Windows.Forms;

namespace Control_Panel.Reports
{
    public partial class XRepAgencyMaster : DevExpress.XtraReports.UI.XtraReport
    {
        public XRepAgencyMaster()
        {
            InitializeComponent();
        }

        private void xrLabel2_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            xrLabel2.ForeColor = Color.Blue;

            ((XRLabel)sender).Tag = GetCurrentRow();
        }

        private void xrLabel2_PreviewClick(object sender, PreviewMouseEventArgs e)
        {

            XRepAgencyDetails report = new XRepAgencyDetails();

            report.paramBDate.Value = this.paramBDate.Value;

            report.paramEDate.Value = this.paramEDate.Value;

            report.paramCode.Value = ((DataRowView)e.Brick.Value).Row["AgencyCode"];

            report.ShowPreviewDialog();

                
        }

        private void xrLabel2_PreviewMouseMove(object sender, PreviewMouseEventArgs e)
        {
            Cursor.Current = Cursors.Hand;
        }

    }
}
