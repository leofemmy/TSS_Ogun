using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;
using System.Windows.Forms;

namespace Control_Panel.Reports
{
    public partial class XRepRevenueOfficeMaster : DevExpress.XtraReports.UI.XtraReport
    {
        public XRepRevenueOfficeMaster()
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
            XRepRevenueOfficeDetails office = new XRepRevenueOfficeDetails();

            office.paramBDate.Value = this.paramBegDate.Value;

            office.paramEDate.Value = this.paramEnDate.Value;

            office.paramOfficeCode.Value = ((DataRowView)e.Brick.Value).Row["RevenueOfficeID"];

            office.ShowPreviewDialog();
        }

        private void xrLabel2_PreviewMouseMove(object sender, PreviewMouseEventArgs e)
        {
            Cursor.Current = Cursors.Hand;
        }

    }
}
