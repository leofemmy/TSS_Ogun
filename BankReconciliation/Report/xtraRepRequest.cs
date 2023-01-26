namespace BankReconciliation.Report
{
    public partial class xtraRepRequest : DevExpress.XtraReports.UI.XtraReport
    {
        public xtraRepRequest()
        {
            InitializeComponent();
        }
        private void xrSubreport1_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            //((XtraReport1)((XRSubreport)sender).ReportSource).parameter1.Value = Convert.ToInt32(GetCurrentColumnValue("PostingRequestID"));
            //[Posting Request ID] 
            //    ((XRSubreport)sender).ReportSource.Parameters["Posting Request ID]"].Value =
            //Convert.ToInt32(GetCurrentColumnValue("PostingRequestID"));
        }

    }
}
