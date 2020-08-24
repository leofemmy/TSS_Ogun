using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;
using System.Windows.Forms;

namespace Control_Panel.Reports
{
    public partial class XRepRevTypeMaster : DevExpress.XtraReports.UI.XtraReport
    {
        public XRepRevTypeMaster()
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
            XRepRevTypeDetails reports = new XRepRevTypeDetails();

            reports.paramRevCode.Value = ((DataRowView)e.Brick.Value).Row["RevenueCode"];

            reports.paramBdate.Value = this.paramBDate.Value;

            reports.paramEDate.Value = this.paramEDate.Value;

            reports.ShowPreviewDialog();

         
        }

        private void xrLabel2_PreviewMouseMove(object sender, PreviewMouseEventArgs e)
        {
            Cursor.Current = Cursors.Hand;
        }

    }
}
