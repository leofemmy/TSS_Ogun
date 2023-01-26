using DevExpress.XtraReports.UI;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace Collection.Report
{
    public partial class XRepRevenue : DevExpress.XtraReports.UI.XtraReport
    {
        public XRepRevenue()
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
            XRepRevnueDetails repRevDet = new XRepRevnueDetails();

            repRevDet.paramRevenueCode.Value = ((DataRowView)e.Brick.Value).Row["RevenueCode"];

            repRevDet.paramSDate.Value = this.paramStartDate.Value;

            repRevDet.paramEdate.Value = this.paramEndDate.Value;

            repRevDet.ShowPreviewDialog();
        }

        private void xrLabel1_PreviewMouseMove(object sender, PreviewMouseEventArgs e)
        {
            Cursor.Current = Cursors.Hand;
        }

    }
}
