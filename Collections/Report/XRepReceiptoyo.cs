namespace Collection.Report
{
    public partial class XRepReceiptoyo : DevExpress.XtraReports.UI.XtraReport
    {
        public XRepReceiptoyo()
        {
            InitializeComponent();
        }

        private void Detail_BeforePrint(object sender, System.ComponentModel.CancelEventArgs e)
        {
            string testrval = string.Format(" <{0}><{1}><{2}> ", this.GetCurrentColumnValue("PayerName").ToString(), string.Format("{0:n}", this.GetCurrentColumnValue("Amount")), this.GetCurrentColumnValue("EReceipts"));

            xrLabel43.Text = testrval;

            xrLabel44.Text = testrval;
        }

        private void XRepReceiptoyo_BeforePrint(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

    }
}
