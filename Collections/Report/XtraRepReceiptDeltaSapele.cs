using DevExpress.XtraReports.UI;
using System.Drawing;

namespace Collection.Report
{
    public partial class XtraRepReceiptDelta : DevExpress.XtraReports.UI.XtraReport
    {
        public string logoPath;

        public string Imagepath;
        public XtraRepReceiptDelta()
        {
            InitializeComponent();

            this.BeforePrint += XtraRepReceiptDelta_BeforePrint1;

            xrLabel43.BeforePrint += xrLabel43_BeforePrint;
        }

        private void XtraRepReceiptDelta_BeforePrint1(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var fullPath = System.IO.Path.Combine(System.Windows.Forms.Application.StartupPath, logoPath);
            if (!string.IsNullOrWhiteSpace(fullPath) && System.IO.File.Exists(fullPath))
                xrPictureBox1.Image = Image.FromFile(fullPath);

            var fullpath2 = System.IO.Path.Combine(System.Windows.Forms.Application.StartupPath, Imagepath);
            if (!string.IsNullOrWhiteSpace(fullpath2) && System.IO.File.Exists(fullpath2))
                xrPictureBox2.Image = Image.FromFile(fullpath2);
            //xrPictureBox2.Image = Image.FromFile(fullPath);
        }

        void xrLabel43_BeforePrint(object sender, System.ComponentModel.CancelEventArgs e)
        {
            string testrval = string.Format("<{0}><{1}><{2}>", this.GetCurrentColumnValue("PayerName").ToString().Trim(), string.Format("{0:n}", this.GetCurrentColumnValue("Amount")), this.GetCurrentColumnValue("EReceipts").ToString());

            xrLabel43.Text = testrval.ToUpper();
        }

        private void XtraRepReceiptDelta_BeforePrint(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SetTextWatermark((XtraRepReceiptDelta)sender);
        }

        void SetTextWatermark(XtraReport report)
        {
            //report.Watermark.Text = "DUPLICATE";
            report.Watermark.TextDirection = DevExpress.XtraPrinting.Drawing.DirectionMode.ForwardDiagonal;
            //report.Watermark.Font = new Font(report.Watermark.Font.FontFamily, 20);
            report.Watermark.TextTransparency = 20;
            report.Watermark.ShowBehind = false;
            report.Watermark.ForeColor = Color.Gray;
        }

    }
}
