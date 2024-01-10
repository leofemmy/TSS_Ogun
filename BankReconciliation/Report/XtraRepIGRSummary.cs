using BankReconciliation.Class;
using DevExpress.XtraReports.UI;
using System.Data;
using System.Data.SqlClient;

namespace BankReconciliation.Report
{
    public partial class XtraRepIGRSummary : DevExpress.XtraReports.UI.XtraReport
    {
        private SqlCommand _command; private SqlDataAdapter adp;
        public XtraRepIGRSummary()
        {
            InitializeComponent();
        }

        private void xrSubreport1_BeforePrint_2(object sender, System.ComponentModel.CancelEventArgs e)
        {
            using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
            {
                connect.Open();

                _command = new SqlCommand("doIGRReport", connect) { CommandType = CommandType.StoredProcedure };

                _command.Parameters.Add(new SqlParameter("@Type", SqlDbType.VarChar)).Value = "Summary";

                _command.Parameters.Add(new SqlParameter("@BatchCode", SqlDbType.VarChar)).Value = GetCurrentColumnValue("BatchCode");

                using (System.Data.DataSet ds = new System.Data.DataSet())
                {
                    ds.Clear();
                    adp = new SqlDataAdapter(_command);
                    adp.Fill(ds);
                    connect.Close();

                    if (ds.Tables[0].Rows[0]["returnCode"].ToString() == "00")
                    {
                        XRSubreport xrSubReport = (XRSubreport)sender;

                        XtraRepSubSummary subrep = xrSubReport.ReportSource as XtraRepSubSummary;

                        subrep.DataSource = ds.Tables[1];

                        subrep.DataMember = "table";

                        subrep.Parameters["parameter1"].Value = GetCurrentColumnValue("BatchCode");

                    }
                    else
                    {
                        return;
                    }

                }
            }
        }
    }
}
