using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace Collection.Report
{
    public partial class XtraRepReceiptDelta : DevExpress.XtraReports.UI.XtraReport
    {
        public XtraRepReceiptDelta()
        {
            InitializeComponent();

            xrLabel43.BeforePrint += xrLabel43_BeforePrint;

        }

        void xrLabel43_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            string testrval = string.Format("<{0}><{1}><{2}>", this.GetCurrentColumnValue("PayerName").ToString(), string.Format("{0:n}", this.GetCurrentColumnValue("Amount")), this.GetCurrentColumnValue("EReceipts").ToString());

            xrLabel43.Text = testrval;
        }

        private void XtraRepReceiptDelta_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            SetTextWatermark((XtraRepReceiptDelta)sender);
        }

        void SetTextWatermark(XtraReport report)
        {
            report.Watermark.Text = "SAMPLE PRINTING";
            report.Watermark.TextDirection = DevExpress.XtraPrinting.Drawing.DirectionMode.ForwardDiagonal;
            report.Watermark.Font = new Font(report.Watermark.Font.FontFamily, 25);
            report.Watermark.TextTransparency = 150;
            report.Watermark.ShowBehind = false;
            report.Watermark.ForeColor = Color.Black;
        }

    }
}
