namespace BankReconciliation.Report
{
    public partial class XtraRepIGRAccountDetails : DevExpress.XtraReports.UI.XtraReport
    {
        public XtraRepIGRAccountDetails()
        {
            InitializeComponent();
        }

        private void xrPivotGrid1_BeforePrint(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        private void XtraRepIGRAccountDetails_ParametersRequestSubmit(object sender, DevExpress.XtraReports.Parameters.ParametersRequestEventArgs e)
        {
            //this.xrPivotGrid1.Prefilter.CriteriaString = "[fieldPeriod]= e";
        }

    }
}
