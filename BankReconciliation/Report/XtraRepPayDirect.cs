using DevExpress.XtraReports.UI;
using System.Drawing;
using System.Windows.Forms;

namespace BankReconciliation.Report
{
    public partial class XtraRepPayDirect : DevExpress.XtraReports.UI.XtraReport
    {
        public XtraRepPayDirect()
        {
            InitializeComponent();

            xrLabel10.BeforePrint += xrLabel10_BeforePrint;

            xrLabel10.PreviewClick += xrLabel10_PreviewClick;

            xrLabel10.PreviewMouseMove += xrLabel10_PreviewMouseMove;
        }

        void xrLabel10_PreviewMouseMove(object sender, PreviewMouseEventArgs e)
        {
            Cursor.Current = Cursors.Hand;
        }

        void xrLabel10_PreviewClick(object sender, PreviewMouseEventArgs e)
        {

        }

        void xrLabel10_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            xrLabel10.ForeColor = Color.Blue;

            ((XRLabel)sender).Tag = GetCurrentRow();
        }

    }
}
