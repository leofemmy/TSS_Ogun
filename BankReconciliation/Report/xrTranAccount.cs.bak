using DevExpress.XtraReports.UI;
using System;
using System.Drawing;

namespace BankReconciliation.Report
{
    public partial class xrTranAccount : DevExpress.XtraReports.UI.XtraReport
    {
        double debit;
        double credit;
        double closebal;
        double openbal;
        //double total;
        double reemAmt;
        double difAmt;

        public xrTranAccount()
        {
            InitializeComponent();

            xrLabel16.BeforePrint += xrLabel16_BeforePrint;
        }

        void xrLabel16_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            debit = Convert.ToDouble(xrSummaryDr.Summary.GetResult());
            closebal = Convert.ToDouble(xrCloseBal.Summary.GetResult());
            credit = Convert.ToDouble(xrSummaryCr.Summary.GetResult());
            openbal = Convert.ToDouble(xrOpenBal.Summary.GetResult());
            reemAmt = Convert.ToDouble(xrAmt.Summary.GetResult());
            //total = (debit + closebal) - (openbal + credit );
            //total = openbal + credit - debit - closebal
            paramCloseDb.Value = debit + closebal;

            double total = (debit + closebal) - (openbal + credit);
            difAmt = total - reemAmt;
            xrLabel16.Text = string.Format("# {0:n2}", total);
            xrDiffAmt.Text = string.Format("# {0:n2}", difAmt);

            xrDiffAmt.ForeColor = Color.DeepSkyBlue;
            xrLabel16.ForeColor = Color.Brown;

        }

        private void xrTranAccount_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            // Create a data binding object.
            XRBinding binding = new XRBinding("Text", null, "ViewTransactionPostCollectionBank.Dr");
            XRBinding bindings = new XRBinding("Text", null, "ViewTransactionPostCollectionBank.Cr");
            XRBinding bindincol = new XRBinding("Text", null, "ViewTransactionPostCollectionBank.CloseBal");

            XRBinding bindingAmt = new XRBinding("Text", null, "ViewTransactionPostCollectionBank.Amount");


            XRBinding bindingopen = new XRBinding("Text", null, "ViewTransactionPostCollectionBank.OpenBal");
            // Add the data binding to the label's collection of bindings.
            xrSummaryDr.DataBindings.Add(binding);

            xrSummaryCr.DataBindings.Add(bindings);

            xrCloseBal.DataBindings.Add(bindincol);

            xrAmt.DataBindings.Add(bindingAmt);

            xrOpenBal.DataBindings.Add(bindingopen);

            // Create an XRSummary object.
            XRSummary summdb = new XRSummary();

            XRSummary summcr = new XRSummary();

            XRSummary sumclose = new XRSummary();

            XRSummary sumAt = new XRSummary();

            XRSummary sumOpen = new XRSummary();

            summdb.Func = SummaryFunc.Sum;
            summdb.Running = SummaryRunning.Group;
            summdb.IgnoreNullValues = true;
            summdb.FormatString = "# {0:n2}";

            summcr.Func = SummaryFunc.Sum;
            summcr.Running = SummaryRunning.Group;
            summcr.IgnoreNullValues = true;
            summcr.FormatString = "# {0:n2}";

            sumclose.Func = SummaryFunc.DSum;
            sumclose.Running = SummaryRunning.Report;
            sumclose.FormatString = "# {0:n2}";

            sumOpen.Func = SummaryFunc.DSum;
            sumOpen.Running = SummaryRunning.Report;
            sumOpen.FormatString = "# {0:n2}";

            sumAt.Func = SummaryFunc.DSum;
            sumAt.Running = SummaryRunning.Report;
            sumAt.FormatString = "# {0:n2}";

            xrSummaryDr.Summary = summdb;
            xrSummaryDr.ForeColor = Color.Green;

            xrAmt.Summary = sumAt;
            xrAmt.ForeColor = Color.Red;

            xrSummaryCr.Summary = summcr;
            xrSummaryCr.ForeColor = Color.Green;

            xrCloseBal.Summary = sumclose;
            xrCloseBal.ForeColor = Color.DarkOliveGreen;

            //xrLabel12.ForeColor = Color.DarkOliveGreen;

            xrOpenBal.Summary = sumOpen;
            xrOpenBal.ForeColor = Color.DarkOliveGreen;


            SummaryFunc res = default(SummaryFunc);
            res = SummaryFunc.Sum;

            XRSummary totdb = default(XRSummary);
            totdb = xrSummaryDr.Summary;

            XRSummary totcd = default(XRSummary);
            totcd = xrSummaryCr.Summary;





        }

    }
}
