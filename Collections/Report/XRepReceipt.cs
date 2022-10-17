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
using System.Collections.Generic;
using Collection.Classes;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net;
using System.Web.Script.Serialization;
using System.Text;

namespace Collection.Report
{
    public partial class XRepReceipt : DevExpress.XtraReports.UI.XtraReport
    {
        DataTable dt = new DataTable();

        BarCode barcode = new BarCode();

        public string logoPath;

        //const string serviceMethod = "/api/v1/QrCoder/encode-string";

        public XRepReceipt()
        {
            InitializeComponent();

            xrLabel49.BeforePrint += xrLabel49_BeforePrint;

            //var BarcodeServiceUrl = System.Configuration.ConfigurationManager.AppSettings["BaseURL"];
        }

        private string PopulatebarCode(string stringraw)
        {
            string apiUrl = System.Configuration.ConfigurationManager.AppSettings["BaseURL"];

            //string apiUrl = "http://services.oyostatebir.com/AssessmentRepositoryService";

            var sources = string.Empty;

            string inputJson = string.Empty; string json = string.Empty;

            WebClient client = new WebClient();

            client.Headers["Content-type"] = "application/json";
            client.Encoding = Encoding.UTF8;

            var paymentRefNo = stringraw;//"OYPDMS226QQ272";  //GTB|BRH|OGUNPYD|16-03-2021|489879
            var encodedPaymentRefNo = AppWebExtension.Base64UrlEncode(paymentRefNo.TrimEnd().ToString());
            var serviceBaseUrl = System.Configuration.ConfigurationManager.AppSettings["serviceBaseUrl"]; //"http://services.oyostatebir.com/AssessmentRepositoryService";
            var serviceMethod = "/api/v1/PaymentCollections/int/generate-online-receipt?&P0dTUM4S=";
            var receiptPathToEncode = $"{serviceBaseUrl}{serviceMethod}{encodedPaymentRefNo}";

            object inputs = new
            {
                rawString = receiptPathToEncode,// "P0dTUM4S=T1lQRE1TMjI2UVEyNzI",// stringraw.Trim(),
            };
            inputJson = (new JavaScriptSerializer()).Serialize(inputs);
            json = client.UploadString(apiUrl + "/api/v1/QrCoder/encode-string", inputJson);
            BarcodeResponse myDeserializedClass = JsonConvert.DeserializeObject<BarcodeResponse>(json);
            sources = myDeserializedClass.data.qrImageUrl;


            return sources;

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

            string testrval = string.Format("<{0}><{1}><{2}><{3}>", this.GetCurrentColumnValue("PaymentRefNumber").ToString(), this.GetCurrentColumnValue("PayerName").ToString(), string.Format("{0:n}", this.GetCurrentColumnValue("Amount")), this.GetCurrentColumnValue("EReceipts").ToString());

            xrLabel50.Text = testrval;

            string payval = this.GetCurrentColumnValue("PaymentPeriod").ToString();

            var bvcode = PopulatebarCode(this.GetCurrentColumnValue("PaymentRefNumber").ToString());// note i will pass payment ref number here


            // var bvcode = PopulatebarCode("GTB|BRH|OGUNPYD|16-03-2021|489879");

            xrPictureBox3.ImageUrl = bvcode;

            xrPictureBox4.ImageUrl = bvcode;


            //var gh = BarCode.GenerateConfirmDocBarcode1(payval);
            //var gh = GetStringData(testrval); // barcode.GenerateConfirmDocBarcode(testrval).GetAwaiter().GetResult();

            xrLabel51.Text = testrval;

            if (!string.IsNullOrWhiteSpace(payval))
            {
                xrLabel58.Visible = true;
                xrLabel59.Visible = true;
                xrLabel4.Visible = true;
                xrLabel21.Visible = true;
            }
            else
            {
                xrLabel58.Visible = false;
                xrLabel4.Visible = false;
                xrLabel59.Visible = false; xrLabel21.Visible = false;
            }


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

