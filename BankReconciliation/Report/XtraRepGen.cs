using BankReconciliation.Class;
using DevExpress.XtraReports.UI;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace BankReconciliation.Report
{
    public partial class XtraRepGen : DevExpress.XtraReports.UI.XtraReport
    {
        public XtraRepGen()
        {
            InitializeComponent();

            xrLabel1.BeforePrint += xrLabel1_BeforePrint;

            xrLabel1.PreviewMouseMove += xrLabel1_PreviewMouseMove;

            xrLabel1.PreviewClick += xrLabel1_PreviewClick;
        }

        void xrLabel1_PreviewClick(object sender, PreviewMouseEventArgs e)
        {
            using (System.Data.DataSet dds = new System.Data.DataSet("DsCollectionReport"))
            {
                //ViewReportReconciliationRevenue
                SqlDataAdapter ada;

                DataTable Dt = dds.Tables.Add("dsTransaction1");

                string query = string.Format("SELECT PayerName, PaymentDate, PaymentRefNumber, Amount, BankName, BankCode, AgencyCode, AgencyName, RevenueCode, Description, BatchCode FROM ViewReconciliationDetailsReport WHERE BankCode ='{0}' and BatchCode ='{1}'", ((DataRowView)e.Brick.Value).Row["BankShortCode"], ((DataRowView)e.Brick.Value).Row["BatchCode"]);

                ada = new SqlDataAdapter(query, Logic.ConnectionString);
                ada.Fill(dds, "dsTransaction1");

                XtraRepReconDetailsReport reportst = new XtraRepReconDetailsReport() { DataSource = dds, DataMember = "ViewReconciliationDetailsReport" };
                //, ((DataRowView)e.Brick.Value).Row["BatchName"]
                reportst.xrLabel14.Text = xrLabel17.Text;
                reportst.xrLabel15.Text = string.Format("Detail of Collections by {0} for {1}", ((DataRowView)e.Brick.Value).Row["BankName"], ((DataRowView)e.Brick.Value).Row["BatchName"]);

                reportst.ShowPreviewDialog();
            }
        }

        void xrLabel1_PreviewMouseMove(object sender, PreviewMouseEventArgs e)
        {
            Cursor.Current = Cursors.Hand;
        }

        void xrLabel1_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            xrLabel1.ForeColor = Color.Navy;

            ((XRLabel)sender).Tag = GetCurrentRow();
        }

    }
}
