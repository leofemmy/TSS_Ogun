using BankReconciliation.Class;
using DevExpress.XtraReports.UI;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace BankReconciliation.Report
{
    public partial class XtraRepSummrayAgency : DevExpress.XtraReports.UI.XtraReport
    {
        public XtraRepSummrayAgency()
        {
            InitializeComponent();

            xrLabel2.BeforePrint += xrLabel2_BeforePrint;

            xrLabel2.PreviewMouseMove += xrLabel2_PreviewMouseMove;

            xrLabel2.PreviewClick += xrLabel2_PreviewClick;
        }

        void xrLabel2_PreviewMouseMove(object sender, PreviewMouseEventArgs e)
        {
            Cursor.Current = Cursors.Hand;
        }

        void xrLabel2_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            xrLabel2.ForeColor = Color.Navy;

            ((XRLabel)sender).Tag = GetCurrentRow();
        }

        void xrLabel2_PreviewClick(object sender, PreviewMouseEventArgs e)
        {
            using (System.Data.DataSet dds = new System.Data.DataSet("DsCollectionReport"))
            {
                SqlDataAdapter ada;

                DataTable Dt = dds.Tables.Add("dsTransaction1");

                string query = string.Format("SELECT PayerName, PaymentDate, PaymentRefNumber, Amount, BankName, BankCode, AgencyCode, AgencyName, RevenueCode, Description, BatchCode FROM ViewReconciliationDetailsReport WHERE AgencyCode ='{0}' and BatchCode ='{1}' ", ((DataRowView)e.Brick.Value).Row["Agencycode"], ((DataRowView)e.Brick.Value).Row["batchcode"]);

                ada = new SqlDataAdapter(query, Logic.ConnectionString);
                ada.Fill(dds, "dsTransaction1");

                XtraRepReconDetailsReport reportst = new XtraRepReconDetailsReport() { DataSource = dds, DataMember = "ViewReconciliationDetailsReport" };

                reportst.xrLabel14.Text = xrLabel32.Text; reportst.xrLabel15.Text = string.Format("Detail of Collections by {0} for {1} ", ((DataRowView)e.Brick.Value).Row["AgencyName"], ((DataRowView)e.Brick.Value).Row["BatchName"]);

                reportst.ShowPreviewDialog();
            }



        }

    }
}
