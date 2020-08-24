using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data.SqlClient;
using Collection.Classess;
using System.Data;
using System.IO;
using System.Linq;

namespace Collection.Report
{
    public partial class XRepReceipt : DevExpress.XtraReports.UI.XtraReport
    {
        DataTable dt = new DataTable();

        public string logoPath;
        public XRepReceipt()
        {
            InitializeComponent();

            xrLabel49.BeforePrint += xrLabel49_BeforePrint;
        }

        void xrLabel49_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {

        }

        private void XRepReceipt_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            //SetTextWatermark((XRepReceipt)sender);
        }

        private void Detail_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            var fullPath = System.IO.Path.Combine(System.Windows.Forms.Application.StartupPath, logoPath);
            if (!string.IsNullOrWhiteSpace(fullPath) && System.IO.File.Exists(fullPath))
                xrPictureBox1.Image = Image.FromFile(fullPath);
            xrPictureBox2.Image = Image.FromFile(fullPath);

            string testrval = string.Format("<{0}><{1}><{2}>", this.GetCurrentColumnValue("PayerName").ToString(), string.Format("{0:n}", this.GetCurrentColumnValue("Amount")), this.GetCurrentColumnValue("EReceipts").ToString());

            xrLabel50.Text = testrval;

            xrLabel51.Text = testrval;

            string strrep = string.Format("REPRINTED");

            //if (Program.isCentralData || Program.IsReprint)
            //{
            //    xrLabel54.Visible = true;
            //    xrLabel55.Visible = true;
            //    xrLabel55.Text = strrep;
            //    xrLabel54.Text = strrep;
            //}


        }

        //void SetTextWatermark(XtraReport report)
        //{
        //    //report.Watermark.Text = "DUPLICATE";
        //    report.Watermark.TextDirection = DevExpress.XtraPrinting.Drawing.DirectionMode.ForwardDiagonal;
        //    report.Watermark.Font = new Font(report.Watermark.Font.FontFamily, 30);
        //    report.Watermark.TextTransparency = 25;
        //    report.Watermark.ShowBehind = true;
        //    report.Watermark.ForeColor = Color.DodgerBlue;
        //}
    }
}

