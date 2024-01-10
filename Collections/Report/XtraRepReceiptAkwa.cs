namespace Collection.Report
{
    public partial class XtraRepReceiptAkwa : DevExpress.XtraReports.UI.XtraReport
    {
        public XtraRepReceiptAkwa()
        {
            InitializeComponent();
        }

        private void Detail_BeforePrint(object sender, System.ComponentModel.CancelEventArgs e)
        {
            xrLabel43.Text = string.Format("*{0}**{1}**{2}*", this.GetCurrentColumnValue("PayerName").ToString(), string.Format("{0:n}", this.GetCurrentColumnValue("Amount")), this.GetCurrentColumnValue("EReceipts").ToString());

            xrLabel21.Text = string.Format("*{0}**{1}**{2}*", this.GetCurrentColumnValue("PayerName").ToString(), string.Format("{0:n}", this.GetCurrentColumnValue("Amount")), this.GetCurrentColumnValue("EReceipts").ToString());
        }

    }
}
