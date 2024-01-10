using DevExpress.XtraReports.UI;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace Collection.Report
{
    public partial class XRepSummaryMaster : DevExpress.XtraReports.UI.XtraReport
    {


        public XRepSummaryMaster()
        {
            InitializeComponent();
        }

        private void xrLabel3_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            xrLabel3.ForeColor = Color.Blue;

            ((XRLabel)sender).Tag = GetCurrentRow();
        }

        private void xrLabel3_PreviewClick(object sender, PreviewMouseEventArgs e)
        {
            XRepSDetails report = new XRepSDetails();

            report.paramBankCode.Value = ((DataRowView)e.Brick.Value).Row["BankCode"];

            report.paramEndDate.Value = this.paramEndDate.Value;

            report.paramStartDate.Value = this.paramStartDate.Value;

            report.ShowPreviewDialog();
        }

        private void xrLabel3_PreviewMouseMove(object sender, PreviewMouseEventArgs e)
        {
            Cursor.Current = Cursors.Hand;
        }

    }
}
