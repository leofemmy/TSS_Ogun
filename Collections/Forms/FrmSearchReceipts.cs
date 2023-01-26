using Collection.Classess;
using Collections;
using DevExpress.Utils;
using DevExpress.XtraSplashScreen;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using TaxSmartSuite.Class;

namespace Collection.Forms
{
    public partial class FrmSearchReceipts : Form
    {
        private DataTable dt;

        public static FrmSearchReceipts publicStreetGroup;

        protected TransactionTypeCode iTransType;

        public static FrmSearchReceipts publicInstance;

        protected bool boolIsUpdate;

        string criteria;

        string BatchNumber;

        public FrmSearchReceipts()
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

                InitializeComponent();

                publicInstance = this;

                publicStreetGroup = this;

                setImages();

                ToolStripEvent();

                Load += OnFormLoad;

                //radioGroup1.Properties.Click += Properties_Click;

                btnSelect.Click += btnSelect_Click;

                btnprint.Click += btnprint_Click;

                radioGroup1.SelectedIndexChanged += radioGroup1_SelectedIndexChanged;


                dtpDate.Format = DateTimePickerFormat.Custom;
                dtpDate.CustomFormat = "dd/MM/yyyy";
            }
            finally
            {
                SplashScreenManager.CloseForm(false);
            }
        }

        void btnprint_Click(object sender, EventArgs e)
        {
            if (dt != null && dt.Rows.Count > 0)
            {
                gridView2.OptionsPrint.UsePrintStyles = true;

                gridView2.OptionsPrint.EnableAppearanceEvenRow = true;
                // Set the background color for even rows. 
                gridView2.AppearancePrint.EvenRow.BackColor = Color.LightYellow;

                gridControl1.ShowPrintPreview();

            }
            else
                Common.setMessageBox("Sorry,there is No Record to be printed", Program.ApplicationName, 1); return;
        }

        void radioGroup1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (radioGroup1.EditValue == null) return;
            if ((Int32)this.radioGroup1.EditValue == 1)
            {
                this.label1.Text = "Receipts No.";
                label2.Visible = false; txtSearch.Clear();
                dtpDate.Enabled = true; dtpenddate.Enabled = true;
            }
            else if ((Int32)this.radioGroup1.EditValue == 2)
            {
                //txtSearch.Clear();
                this.label1.Text = "Payment Ref. No.";
                label2.Text = "(e.g LMFB|OGPDIPAY|0001|19-9-2013|143239 )";
                label2.Visible = true; txtSearch.Clear();
                dtpDate.Enabled = true; dtpenddate.Enabled = true;
            }
            else if ((Int32)this.radioGroup1.EditValue == 3)
            {
                //txtSearch.Clear();
                this.label1.Text = "Payer Name";
                label2.Visible = false; txtSearch.Clear();
                dtpDate.Enabled = true; dtpenddate.Enabled = true;
            }
            else if ((Int32)this.radioGroup1.EditValue == 4)
            {
                //this.label1.Text = "Receipts No.";
                this.label1.Text = "Deposit Slip Number";
                txtSearch.Enabled = true;
                dtpDate.Enabled = true; dtpenddate.Enabled = true;
            }
        }

        void btnSelect_Click(object sender, EventArgs e)
        {
            try
            {
                switch (Program.intCode)
                {
                    case 13://Akwa Ibom state
                        if ((Int32)this.radioGroup1.EditValue == 1)
                        {
                            //and ([EReceiptsDate] BETWEEN '{1} 00:00:00' AND '{1} 23:59:59')
                            criteria = String.Format("SELECT Collection.tblCollectionReport.PaymentRefNumber,PayerName,Amount,CONVERT(VARCHAR, CONVERT(DATE, PaymentDate), 103) AS PaymentDate,AgencyName,Description,Collection.tblCollectionReport.EReceipts,CONVERT(VARCHAR, CONVERT(DATE, Receipt.tblCollectionReceipt.EReceiptsDate), 103)  AS [Receipts Date],PrintedBY,CONVERT(VARCHAR, CONVERT(DATE, IsPrintedDate), 103)  AS DatePrinted,ControlNumber ,Collection.tblCollectionReport.StationCode, ( SELECT StationName FROM Receipt.tblStation WHERE  Collection.tblCollectionReport.StationCode = Receipt.tblStation.StationCode ) AS StationName FROM Collection.tblCollectionReport LEFT JOIN Receipt.tblCollectionReceipt ON Collection.tblCollectionReport.PaymentRefNumber = Receipt.tblCollectionReceipt.PaymentRefNumber WHERE Collection.tblCollectionReport.EReceipts LIKE '%{0}%'", txtSearch.Text.Trim());
                        }
                        else if ((Int32)this.radioGroup1.EditValue == 2)
                        {
                            criteria = String.Format("SELECT Collection.tblCollectionReport.PaymentRefNumber,PayerName,Amount,CONVERT(VARCHAR, CONVERT(DATE, PaymentDate), 103) AS PaymentDate,AgencyName,Description,Collection.tblCollectionReport.EReceipts,CONVERT(VARCHAR, CONVERT(DATE, Receipt.tblCollectionReceipt.EReceiptsDate), 103)  AS [Receipts Date],PrintedBY,CONVERT(VARCHAR, CONVERT(DATE, IsPrintedDate), 103)  AS DatePrinted,ControlNumber ,Collection.tblCollectionReport.StationCode, ( SELECT StationName FROM Receipt.tblStation WHERE  Collection.tblCollectionReport.StationCode = Receipt.tblStation.StationCode ) AS StationName FROM Collection.tblCollectionReport LEFT JOIN Receipt.tblCollectionReceipt ON Collection.tblCollectionReport.PaymentRefNumber = Receipt.tblCollectionReceipt.PaymentRefNumber WHERE Collection.tblCollectionReport.PaymentRefNumber LIKE '%{0}%'", txtSearch.Text.Trim());
                        }
                        else if ((Int32)this.radioGroup1.EditValue == 3)
                        {
                            //this.label1.Text = "Payer Name";
                            criteria = String.Format("SELECT Collection.tblCollectionReport.PaymentRefNumber,PayerName,Amount,CONVERT(VARCHAR, CONVERT(DATE, PaymentDate), 103) AS PaymentDate,AgencyName,Description,Receipt.tblCollectionReceipt.EReceipts,CONVERT(VARCHAR, CONVERT(DATE, Receipt.tblCollectionReceipt.EReceiptsDate), 103)  AS [Receipts Date],PrintedBY,CONVERT(VARCHAR, CONVERT(DATE, IsPrintedDate), 103)  AS DatePrinted,ControlNumber ,Collection.tblCollectionReport.StationCode, ( SELECT StationName FROM Receipt.tblStation WHERE  Collection.tblCollectionReport.StationCode = Receipt.tblStation.StationCode ) AS StationName FROM Collection.tblCollectionReport left JOIN Receipt.tblCollectionReceipt ON Collection.tblCollectionReport.PaymentRefNumber = Receipt.tblCollectionReceipt.PaymentRefNumber WHERE Collection.tblCollectionReport.PayerName Like '%{0}%'", txtSearch.Text.Trim());
                        }
                        else if ((Int32)this.radioGroup1.EditValue == 4)
                        {
                            criteria = String.Format("SELECT Collection.tblCollectionReport.PaymentRefNumber,PayerName,Amount,CONVERT(VARCHAR, CONVERT(DATE, PaymentDate), 103) AS PaymentDate,AgencyName,Description,Collection.tblCollectionReport.EReceipts,CONVERT(VARCHAR, CONVERT(DATE, Receipt.tblCollectionReceipt.EReceiptsDate), 103)  AS [Receipts Date],PrintedBY,CONVERT(VARCHAR, CONVERT(DATE, IsPrintedDate), 103)  AS DatePrinted,ControlNumber ,Collection.tblCollectionReport.StationCode, ( SELECT StationName FROM Receipt.tblStation WHERE  Collection.tblCollectionReport.StationCode = Receipt.tblStation.StationCode ) AS StationName FROM Collection.tblCollectionReport LEFT JOIN Receipt.tblCollectionReceipt ON Collection.tblCollectionReport.PaymentRefNumber = Receipt.tblCollectionReceipt.PaymentRefNumber WHERE DepositSlipNumber Like '%{0}%'", txtSearch.Text.Trim());
                        }
                        break;

                    case 20://detla state
                        if ((Int32)this.radioGroup1.EditValue == 1)
                        {
                            //and ([EReceiptsDate] BETWEEN '{1} 00:00:00' AND '{1} 23:59:59')
                            criteria = String.Format("SELECT Collection.tblCollectionReport.PaymentRefNumber,PayerName,Amount,CONVERT(VARCHAR, CONVERT(DATE, PaymentDate), 103) AS PaymentDate,AgencyName,Description,Collection.tblCollectionReport.EReceipts,CONVERT(VARCHAR, CONVERT(DATE, Receipt.tblCollectionReceipt.EReceiptsDate), 103)  AS [Receipts Date],PrintedBY,CONVERT(VARCHAR, CONVERT(DATE, IsPrintedDate), 103)  AS DatePrinted,Collection.tblCollectionReport.ControlNumber ,Collection.tblCollectionReport.StationCode, ( SELECT StationName FROM Receipt.tblStation WHERE  Collection.tblCollectionReport.StationCode = Receipt.tblStation.StationCode ) AS StationName FROM Collection.tblCollectionReport LEFT JOIN Receipt.tblCollectionReceipt ON Collection.tblCollectionReport.PaymentRefNumber = Receipt.tblCollectionReceipt.PaymentRefNumber WHERE Collection.tblCollectionReport.EReceipts LIKE '%{0}%' and Collection.tblCollectionReport.PaymentDate >= '{1}' And Collection.tblCollectionReport.PaymentDate <= '{2}'", txtSearch.Text.Trim(), string.Format("{0:MM/dd/yyy 00:00:00}", dtpDate.Value), string.Format("{0:MM/dd/yyy 23:59:59}", dtpenddate.Value));
                        }
                        else if ((Int32)this.radioGroup1.EditValue == 2)
                        {
                            criteria = String.Format("SELECT Collection.tblCollectionReport.PaymentRefNumber,PayerName,Amount,CONVERT(VARCHAR, CONVERT(DATE, PaymentDate), 103) AS PaymentDate,AgencyName,Description,Collection.tblCollectionReport.EReceipts,CONVERT(VARCHAR, CONVERT(DATE, Receipt.tblCollectionReceipt.EReceiptsDate), 103)  AS [Receipts Date],PrintedBY,CONVERT(VARCHAR, CONVERT(DATE, IsPrintedDate), 103)  AS DatePrinted,Collection.tblCollectionReport.ControlNumber ,Collection.tblCollectionReport.StationCode, ( SELECT StationName FROM Receipt.tblStation WHERE  Collection.tblCollectionReport.StationCode = Receipt.tblStation.StationCode ) AS StationName FROM Collection.tblCollectionReport LEFT JOIN Receipt.tblCollectionReceipt ON Collection.tblCollectionReport.PaymentRefNumber = Receipt.tblCollectionReceipt.PaymentRefNumber WHERE Collection.tblCollectionReport.PaymentRefNumber LIKE '%{0}%' And Collection.tblCollectionReport.PaymentDate >= '{1}' And Collection.tblCollectionReport.PaymentDate <= '{2}'", txtSearch.Text.Trim(), string.Format("{0:MM/dd/yyy 00:00:00}", dtpDate.Value), string.Format("{0:MM/dd/yyy 23:59:59}", dtpenddate.Value));
                        }
                        else if ((Int32)this.radioGroup1.EditValue == 3)
                        {
                            //this.label1.Text = "Payer Name";
                            criteria = String.Format("SELECT Collection.tblCollectionReport.PaymentRefNumber,PayerName,Amount,CONVERT(VARCHAR, CONVERT(DATE, PaymentDate), 103) AS PaymentDate,AgencyName,Description,Receipt.tblCollectionReceipt.EReceipts,CONVERT(VARCHAR, CONVERT(DATE, Receipt.tblCollectionReceipt.EReceiptsDate), 103)  AS [Receipts Date],PrintedBY,CONVERT(VARCHAR, CONVERT(DATE, IsPrintedDate), 103)  AS DatePrinted,Collection.tblCollectionReport.ControlNumber ,Collection.tblCollectionReport.StationCode, ( SELECT StationName FROM Receipt.tblStation WHERE  Collection.tblCollectionReport.StationCode = Receipt.tblStation.StationCode ) AS StationName FROM Collection.tblCollectionReport left JOIN Receipt.tblCollectionReceipt ON Collection.tblCollectionReport.PaymentRefNumber = Receipt.tblCollectionReceipt.PaymentRefNumber WHERE Collection.tblCollectionReport.PayerName Like '%{0}%' and Collection.tblCollectionReport.PaymentDate >= '{1}' And Collection.tblCollectionReport.PaymentDate <= '{2}' ", txtSearch.Text.Trim(), string.Format("{0:MM/dd/yyy 00:00:00}", dtpDate.Value), string.Format("{0:MM/dd/yyy 23:59:59}", dtpenddate.Value));
                        }
                        else if ((Int32)this.radioGroup1.EditValue == 4)
                        {
                            criteria = String.Format("SELECT Collection.tblCollectionReport.PaymentRefNumber,PayerName,Amount,CONVERT(VARCHAR, CONVERT(DATE, PaymentDate), 103) AS PaymentDate,AgencyName,Description,Collection.tblCollectionReport.EReceipts,CONVERT(VARCHAR, CONVERT(DATE, Receipt.tblCollectionReceipt.EReceiptsDate), 103)  AS [Receipts Date],PrintedBY,CONVERT(VARCHAR, CONVERT(DATE, IsPrintedDate), 103)  AS DatePrinted,Collection.tblCollectionReport.ControlNumber ,Collection.tblCollectionReport.StationCode, ( SELECT StationName FROM Receipt.tblStation WHERE  Collection.tblCollectionReport.StationCode = Receipt.tblStation.StationCode ) AS StationName FROM Collection.tblCollectionReport LEFT JOIN Receipt.tblCollectionReceipt ON Collection.tblCollectionReport.PaymentRefNumber = Receipt.tblCollectionReceipt.PaymentRefNumber WHERE  Collection.tblCollectionReport.PaymentDate >= '{0}' And Collection.tblCollectionReport.PaymentDate <= '{1}'   AND DepositSlipNumber Like '%{2}%'", string.Format("{0:MM/dd/yyy 00:00:00}", dtpDate.Value), string.Format("{0:MM/dd/yyy 23:59:59}", dtpenddate.Value), txtSearch.Text.Trim());
                        }
                        break;

                    case 37://ogun state
                        if ((Int32)this.radioGroup1.EditValue == 1)
                        {
                            //and ([EReceiptsDate] BETWEEN '{1} 00:00:00' AND '{1} 23:59:59')
                            criteria = String.Format("SELECT Collection.tblCollectionReport.PaymentRefNumber,PayerName,Amount,CONVERT(VARCHAR, CONVERT(DATE, PaymentDate), 103) AS PaymentDate,AgencyName,Description,Collection.tblCollectionReport.EReceipts,CONVERT(VARCHAR, CONVERT(DATE, Receipt.tblCollectionReceipt.EReceiptsDate), 103)  AS [Receipts Date],PrintedBY,CONVERT(VARCHAR, CONVERT(DATE, IsPrintedDate), 103)  AS DatePrinted,ControlNumber ,Collection.tblCollectionReport.StationCode, ( SELECT StationName FROM Receipt.tblStation WHERE  Collection.tblCollectionReport.StationCode = Receipt.tblStation.StationCode ) AS StationName FROM Collection.tblCollectionReport LEFT JOIN Receipt.tblCollectionReceipt ON Collection.tblCollectionReport.PaymentRefNumber = Receipt.tblCollectionReceipt.PaymentRefNumber WHERE Collection.tblCollectionReport.EReceipts LIKE '%{0}%' and Collection.tblCollectionReport.EReceiptsDate >= '{1}' And Collection.tblCollectionReport.EReceiptsDate <= '{2}'", txtSearch.Text.Trim(), string.Format("{0:MM/dd/yyy 00:00:00}", dtpDate.Value), string.Format("{0:MM/dd/yyy 23:59:59}", dtpenddate.Value));
                        }
                        else if ((Int32)this.radioGroup1.EditValue == 2)
                        {
                            criteria = String.Format("SELECT Collection.tblCollectionReport.PaymentRefNumber,PayerName,Amount,CONVERT(VARCHAR, CONVERT(DATE, PaymentDate), 103) AS PaymentDate,AgencyName,Description,Collection.tblCollectionReport.EReceipts,CONVERT(VARCHAR, CONVERT(DATE, Receipt.tblCollectionReceipt.EReceiptsDate), 103)  AS [Receipts Date],PrintedBY,CONVERT(VARCHAR, CONVERT(DATE, IsPrintedDate), 103)  AS DatePrinted,ControlNumber ,Collection.tblCollectionReport.StationCode, ( SELECT StationName FROM Receipt.tblStation WHERE  Collection.tblCollectionReport.StationCode = Receipt.tblStation.StationCode ) AS StationName FROM Collection.tblCollectionReport LEFT JOIN Receipt.tblCollectionReceipt ON Collection.tblCollectionReport.PaymentRefNumber = Receipt.tblCollectionReceipt.PaymentRefNumber WHERE Collection.tblCollectionReport.PaymentRefNumber LIKE '%{0}%' And Collection.tblCollectionReport.EReceiptsDate >= '{1}' And Collection.tblCollectionReport.EReceiptsDate <= '{2}'", txtSearch.Text.Trim(), string.Format("{0:MM/dd/yyy 00:00:00}", dtpDate.Value), string.Format("{0:MM/dd/yyy 23:59:59}", dtpenddate.Value));
                        }
                        else if ((Int32)this.radioGroup1.EditValue == 3)
                        {
                            //this.label1.Text = "Payer Name";
                            criteria = String.Format("SELECT Collection.tblCollectionReport.PaymentRefNumber,PayerName,Amount,CONVERT(VARCHAR, CONVERT(DATE, PaymentDate), 103) AS PaymentDate,AgencyName,Description,Receipt.tblCollectionReceipt.EReceipts,CONVERT(VARCHAR, CONVERT(DATE, Receipt.tblCollectionReceipt.EReceiptsDate), 103)  AS [Receipts Date],PrintedBY,CONVERT(VARCHAR, CONVERT(DATE, IsPrintedDate), 103)  AS DatePrinted,ControlNumber ,Collection.tblCollectionReport.StationCode, ( SELECT StationName FROM Receipt.tblStation WHERE  Collection.tblCollectionReport.StationCode = Receipt.tblStation.StationCode ) AS StationName FROM Collection.tblCollectionReport left JOIN Receipt.tblCollectionReceipt ON Collection.tblCollectionReport.PaymentRefNumber = Receipt.tblCollectionReceipt.PaymentRefNumber WHERE Collection.tblCollectionReport.PayerName Like '%{0}%' and Collection.tblCollectionReport.EReceiptsDate >= '{1}' And Collection.tblCollectionReport.EReceiptsDate <= '{2}' ", txtSearch.Text.Trim(), string.Format("{0:MM/dd/yyy 00:00:00}", dtpDate.Value), string.Format("{0:MM/dd/yyy 23:59:59}", dtpenddate.Value));
                        }
                        else if ((Int32)this.radioGroup1.EditValue == 4)
                        {
                            criteria = String.Format("SELECT Collection.tblCollectionReport.PaymentRefNumber,PayerName,Amount,CONVERT(VARCHAR, CONVERT(DATE, PaymentDate), 103) AS PaymentDate,AgencyName,Description,Collection.tblCollectionReport.EReceipts,CONVERT(VARCHAR, CONVERT(DATE, Receipt.tblCollectionReceipt.EReceiptsDate), 103)  AS [Receipts Date],PrintedBY,CONVERT(VARCHAR, CONVERT(DATE, IsPrintedDate), 103)  AS DatePrinted,ControlNumber ,Collection.tblCollectionReport.StationCode, ( SELECT StationName FROM Receipt.tblStation WHERE  Collection.tblCollectionReport.StationCode = Receipt.tblStation.StationCode ) AS StationName FROM Collection.tblCollectionReport LEFT JOIN Receipt.tblCollectionReceipt ON Collection.tblCollectionReport.PaymentRefNumber = Receipt.tblCollectionReceipt.PaymentRefNumber WHERE  Collection.tblCollectionReport.EReceiptsDate >= '{0}' And Collection.tblCollectionReport.EReceiptsDate <= '{1}'   AND DepositSlipNumber Like '%{2}%'", string.Format("{0:MM/dd/yyy 00:00:00}", dtpDate.Value), string.Format("{0:MM/dd/yyy 23:59:59}", dtpenddate.Value), txtSearch.Text.Trim());
                        }
                        break;

                    case 40://oyo state
                        if ((Int32)this.radioGroup1.EditValue == 1)
                        {
                            //and ([EReceiptsDate] BETWEEN '{1} 00:00:00' AND '{1} 23:59:59')
                            criteria = String.Format("SELECT Collection.tblCollectionReport.PaymentRefNumber,PayerName,Amount,CONVERT(VARCHAR, CONVERT(DATE, PaymentDate), 103) AS PaymentDate,AgencyName,Description,Collection.tblCollectionReport.EReceipts,CONVERT(VARCHAR, CONVERT(DATE, Receipt.tblCollectionReceipt.EReceiptsDate), 103)  AS [Receipts Date],PrintedBY,CONVERT(VARCHAR, CONVERT(DATE, IsPrintedDate), 103)  AS DatePrinted,ControlNumber ,Collection.tblCollectionReport.StationCode, ( SELECT StationName FROM Receipt.tblStation WHERE  Collection.tblCollectionReport.StationCode = Receipt.tblStation.StationCode ) AS StationName FROM Collection.tblCollectionReport LEFT JOIN Receipt.tblCollectionReceipt ON Collection.tblCollectionReport.PaymentRefNumber = Receipt.tblCollectionReceipt.PaymentRefNumber WHERE Collection.tblCollectionReport.EReceipts LIKE '%{0}%' and Collection.tblCollectionReport.EReceiptsDate >= '{1}' And Collection.tblCollectionReport.EReceiptsDate <= '{2}'", txtSearch.Text.Trim(), string.Format("{0:MM/dd/yyy 00:00:00}", dtpDate.Value), string.Format("{0:MM/dd/yyy 23:59:59}", dtpenddate.Value));
                        }
                        else if ((Int32)this.radioGroup1.EditValue == 2)
                        {
                            criteria = String.Format("SELECT Collection.tblCollectionReport.PaymentRefNumber,PayerName,Amount,CONVERT(VARCHAR, CONVERT(DATE, PaymentDate), 103) AS PaymentDate,AgencyName,Description,Collection.tblCollectionReport.EReceipts,CONVERT(VARCHAR, CONVERT(DATE, Receipt.tblCollectionReceipt.EReceiptsDate), 103)  AS [Receipts Date],PrintedBY,CONVERT(VARCHAR, CONVERT(DATE, IsPrintedDate), 103)  AS DatePrinted,ControlNumber ,Collection.tblCollectionReport.StationCode, ( SELECT StationName FROM Receipt.tblStation WHERE  Collection.tblCollectionReport.StationCode = Receipt.tblStation.StationCode ) AS StationName FROM Collection.tblCollectionReport LEFT JOIN Receipt.tblCollectionReceipt ON Collection.tblCollectionReport.PaymentRefNumber = Receipt.tblCollectionReceipt.PaymentRefNumber WHERE Collection.tblCollectionReport.PaymentRefNumber LIKE '%{0}%' And Collection.tblCollectionReport.EReceiptsDate >= '{1}' And Collection.tblCollectionReport.EReceiptsDate <= '{2}'", txtSearch.Text.Trim(), string.Format("{0:MM/dd/yyy 00:00:00}", dtpDate.Value), string.Format("{0:MM/dd/yyy 23:59:59}", dtpenddate.Value));
                        }
                        else if ((Int32)this.radioGroup1.EditValue == 3)
                        {
                            //this.label1.Text = "Payer Name";
                            criteria = String.Format("SELECT Collection.tblCollectionReport.PaymentRefNumber,PayerName,Amount,CONVERT(VARCHAR, CONVERT(DATE, PaymentDate), 103) AS PaymentDate,AgencyName,Description,Receipt.tblCollectionReceipt.EReceipts,CONVERT(VARCHAR, CONVERT(DATE, Receipt.tblCollectionReceipt.EReceiptsDate), 103)  AS [Receipts Date],PrintedBY,CONVERT(VARCHAR, CONVERT(DATE, IsPrintedDate), 103)  AS DatePrinted,ControlNumber ,Collection.tblCollectionReport.StationCode, ( SELECT StationName FROM Receipt.tblStation WHERE  Collection.tblCollectionReport.StationCode = Receipt.tblStation.StationCode ) AS StationName FROM Collection.tblCollectionReport left JOIN Receipt.tblCollectionReceipt ON Collection.tblCollectionReport.PaymentRefNumber = Receipt.tblCollectionReceipt.PaymentRefNumber WHERE Collection.tblCollectionReport.PayerName Like '%{0}%' and Collection.tblCollectionReport.EReceiptsDate >= '{1}' And Collection.tblCollectionReport.EReceiptsDate <= '{2}' ", txtSearch.Text.Trim(), string.Format("{0:MM/dd/yyy 00:00:00}", dtpDate.Value), string.Format("{0:MM/dd/yyy 23:59:59}", dtpenddate.Value));
                        }
                        else if ((Int32)this.radioGroup1.EditValue == 4)
                        {
                            criteria = String.Format("SELECT Collection.tblCollectionReport.PaymentRefNumber,PayerName,Amount,CONVERT(VARCHAR, CONVERT(DATE, PaymentDate), 103) AS PaymentDate,AgencyName,Description,Collection.tblCollectionReport.EReceipts,CONVERT(VARCHAR, CONVERT(DATE, Receipt.tblCollectionReceipt.EReceiptsDate), 103)  AS [Receipts Date],PrintedBY,CONVERT(VARCHAR, CONVERT(DATE, IsPrintedDate), 103)  AS DatePrinted,ControlNumber ,Collection.tblCollectionReport.StationCode, ( SELECT StationName FROM Receipt.tblStation WHERE  Collection.tblCollectionReport.StationCode = Receipt.tblStation.StationCode ) AS StationName FROM Collection.tblCollectionReport LEFT JOIN Receipt.tblCollectionReceipt ON Collection.tblCollectionReport.PaymentRefNumber = Receipt.tblCollectionReceipt.PaymentRefNumber WHERE  Collection.tblCollectionReport.EReceiptsDate >= '{0}' And Collection.tblCollectionReport.EReceiptsDate <= '{1}'   AND DepositSlipNumber Like '%{2}%'", string.Format("{0:MM/dd/yyy 00:00:00}", dtpDate.Value), string.Format("{0:MM/dd/yyy 23:59:59}", dtpenddate.Value), txtSearch.Text.Trim());
                        }
                        break;

                    case 32://kogi state
                        if ((Int32)this.radioGroup1.EditValue == 1)
                        {
                            //and ([EReceiptsDate] BETWEEN '{1} 00:00:00' AND '{1} 23:59:59')
                            criteria = String.Format("SELECT Collection.tblCollectionReport.PaymentRefNumber,PayerName,Amount,CONVERT(VARCHAR, CONVERT(DATE, PaymentDate), 103) AS PaymentDate,AgencyName,Description,Collection.tblCollectionReport.EReceipts,CONVERT(VARCHAR, CONVERT(DATE, Receipt.tblCollectionReceipt.EReceiptsDate), 103)  AS [Receipts Date],PrintedBY,CONVERT(VARCHAR, CONVERT(DATE, IsPrintedDate), 103)  AS DatePrinted,ControlNumber ,Collection.tblCollectionReport.StationCode, ( SELECT StationName FROM Receipt.tblStation WHERE  Collection.tblCollectionReport.StationCode = Receipt.tblStation.StationCode ) AS StationName FROM Collection.tblCollectionReport LEFT JOIN Receipt.tblCollectionReceipt ON Collection.tblCollectionReport.PaymentRefNumber = Receipt.tblCollectionReceipt.PaymentRefNumber WHERE Collection.tblCollectionReport.EReceipts LIKE '%{0}%' and Collection.tblCollectionReport.EReceiptsDate >= '{1}' And Collection.tblCollectionReport.EReceiptsDate <= '{2}'", txtSearch.Text.Trim(), string.Format("{0:MM/dd/yyy 00:00:00}", dtpDate.Value), string.Format("{0:MM/dd/yyy 23:59:59}", dtpenddate.Value));
                        }
                        else if ((Int32)this.radioGroup1.EditValue == 2)
                        {
                            criteria = String.Format("SELECT Collection.tblCollectionReport.PaymentRefNumber,PayerName,Amount,CONVERT(VARCHAR, CONVERT(DATE, PaymentDate), 103) AS PaymentDate,AgencyName,Description,Collection.tblCollectionReport.EReceipts,CONVERT(VARCHAR, CONVERT(DATE, Receipt.tblCollectionReceipt.EReceiptsDate), 103)  AS [Receipts Date],PrintedBY,CONVERT(VARCHAR, CONVERT(DATE, IsPrintedDate), 103)  AS DatePrinted,ControlNumber ,Collection.tblCollectionReport.StationCode, ( SELECT StationName FROM Receipt.tblStation WHERE  Collection.tblCollectionReport.StationCode = Receipt.tblStation.StationCode ) AS StationName FROM Collection.tblCollectionReport LEFT JOIN Receipt.tblCollectionReceipt ON Collection.tblCollectionReport.PaymentRefNumber = Receipt.tblCollectionReceipt.PaymentRefNumber WHERE Collection.tblCollectionReport.PaymentRefNumber LIKE '%{0}%' And Collection.tblCollectionReport.EReceiptsDate >= '{1}' And Collection.tblCollectionReport.EReceiptsDate <= '{2}'", txtSearch.Text.Trim(), string.Format("{0:MM/dd/yyy 00:00:00}", dtpDate.Value), string.Format("{0:MM/dd/yyy 23:59:59}", dtpenddate.Value));
                        }
                        else if ((Int32)this.radioGroup1.EditValue == 3)
                        {
                            //this.label1.Text = "Payer Name";
                            criteria = String.Format("SELECT Collection.tblCollectionReport.PaymentRefNumber,PayerName,Amount,CONVERT(VARCHAR, CONVERT(DATE, PaymentDate), 103) AS PaymentDate,AgencyName,Description,Receipt.tblCollectionReceipt.EReceipts,CONVERT(VARCHAR, CONVERT(DATE, Receipt.tblCollectionReceipt.EReceiptsDate), 103)  AS [Receipts Date],PrintedBY,CONVERT(VARCHAR, CONVERT(DATE, IsPrintedDate), 103)  AS DatePrinted,ControlNumber ,Collection.tblCollectionReport.StationCode, ( SELECT StationName FROM Receipt.tblStation WHERE  Collection.tblCollectionReport.StationCode = Receipt.tblStation.StationCode ) AS StationName FROM Collection.tblCollectionReport left JOIN Receipt.tblCollectionReceipt ON Collection.tblCollectionReport.PaymentRefNumber = Receipt.tblCollectionReceipt.PaymentRefNumber WHERE Collection.tblCollectionReport.PayerName Like '%{0}%' and Collection.tblCollectionReport.EReceiptsDate >= '{1}' And Collection.tblCollectionReport.EReceiptsDate <= '{2}' ", txtSearch.Text.Trim(), string.Format("{0:MM/dd/yyy 00:00:00}", dtpDate.Value), string.Format("{0:MM/dd/yyy 23:59:59}", dtpenddate.Value));
                        }
                        else if ((Int32)this.radioGroup1.EditValue == 4)
                        {
                            criteria = String.Format("SELECT Collection.tblCollectionReport.PaymentRefNumber,PayerName,Amount,CONVERT(VARCHAR, CONVERT(DATE, PaymentDate), 103) AS PaymentDate,AgencyName,Description,Collection.tblCollectionReport.EReceipts,CONVERT(VARCHAR, CONVERT(DATE, Receipt.tblCollectionReceipt.EReceiptsDate), 103)  AS [Receipts Date],PrintedBY,CONVERT(VARCHAR, CONVERT(DATE, IsPrintedDate), 103)  AS DatePrinted,ControlNumber ,Collection.tblCollectionReport.StationCode, ( SELECT StationName FROM Receipt.tblStation WHERE  Collection.tblCollectionReport.StationCode = Receipt.tblStation.StationCode ) AS StationName FROM Collection.tblCollectionReport LEFT JOIN Receipt.tblCollectionReceipt ON Collection.tblCollectionReport.PaymentRefNumber = Receipt.tblCollectionReceipt.PaymentRefNumber WHERE  Collection.tblCollectionReport.EReceiptsDate >= '{0}' And Collection.tblCollectionReport.EReceiptsDate <= '{1}'   AND DepositSlipNumber Like '%{2}%'", string.Format("{0:MM/dd/yyy 00:00:00}", dtpDate.Value), string.Format("{0:MM/dd/yyy 23:59:59}", dtpenddate.Value), txtSearch.Text.Trim());
                        }

                        break;

                    default:
                        break;
                }

                if (radioGroup1.EditValue == null)
                {
                    Common.setEmptyField("Search Criteria, can't be empty", "Search Receipt");
                    return;
                }
                else
                {
                    SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);





                    using (var ds = new System.Data.DataSet())
                    {
                        using (SqlDataAdapter ada = new SqlDataAdapter(criteria, Logic.ConnectionString))
                        {
                            ada.SelectCommand.CommandTimeout = 0;

                            ada.Fill(ds, "table");
                        }
                        dt = ds.Tables[0];

                    }

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        //gridControl4.DataSource = dt;
                        gridControl1.DataSource = dt;

                        gridView2.BestFitColumns();
                        layoutView1.Columns["Amount"].DisplayFormat.FormatType = FormatType.Numeric;
                        layoutView1.Columns["Amount"].DisplayFormat.FormatString = "n2";
                        //layoutView1.
                        //layoutView1.Columns["Amount"].DisplayFormat.Forma
                        layoutView1.Columns["PaymentDate"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
                        layoutView1.Columns["PaymentDate"].DisplayFormat.FormatString = "dd/MM/yyyy";
                        layoutView1.Columns["DatePrinted"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
                        layoutView1.Columns["DatePrinted"].DisplayFormat.FormatString = "dd/MM/yyyy";
                        //layoutView1.bes
                        //gridView2.Columns["Amount"].DisplayFormat.FormatType = FormatType.Numeric;
                        //gridView2.Columns["Amount"].DisplayFormat.FormatString = "n2";
                        //gridView4.Columns["ID"].Visible = false;
                    }
                    else
                    {
                        Common.setMessageBox("No Record Found", Program.ApplicationName, 1);
                        return;
                    }
                }
            }
            finally
            {
                SplashScreenManager.CloseForm(false);
            }

        }

        private void setImages()
        {
            tsbNew.Image = MDIMain.publicMDIParent.i16x16.Images[5];
            tsbEdit.Image = MDIMain.publicMDIParent.i16x16.Images[6];
            tsbDelete.Image = MDIMain.publicMDIParent.i16x16.Images[7];
            tsbReload.Image = MDIMain.publicMDIParent.i16x16.Images[0];
            tsbClose.Image = MDIMain.publicMDIParent.i16x16.Images[11];

            //bttnCancel.Image = MDIMain.publicMDIParent.i32x32.Images[9];
            //btnPrint.Image = MDIMain.publicMDIParent.i32x32.Images[29];
            btnSelect.Image = MDIMain.publicMDIParent.i32x32.Images[2];
            //btnSearch.Image = MDIMain.publicMDIParent.i32x32.Images[2];

        }

        void ToolStripEvent()
        {
            tsbClose.Click += OnToolStripItemsClicked;
            tsbNew.Click += OnToolStripItemsClicked;
            tsbEdit.Click += OnToolStripItemsClicked;
            tsbDelete.Click += OnToolStripItemsClicked;
            tsbReload.Click += OnToolStripItemsClicked;
        }

        void OnToolStripItemsClicked(object sender, EventArgs e)
        {
            if (sender == tsbClose)
            {
                MDIMain.publicMDIParent.RemoveControls();
            }
            else if (sender == tsbNew)
            {
                //label11.Visible = false;

                //txtPaymentRef.Visible = false;

                //groupControl2.Text = "Add New Record";

                //iTransType = TransactionTypeCode.New;

                //ShowForm();

                //clear();

                //groupControl2.Enabled = true;

                boolIsUpdate = false;
            }
            else if (sender == tsbEdit)
            {
                //groupControl2.Text = "Edit Record Mode";

                iTransType = TransactionTypeCode.Edit;

                //ShowForm();

                boolIsUpdate = true;

            }
            //else if (sender == tsbDelete)
            //{
            //    groupControl2.Text = "Delete Record Mode";
            //    iTransType = TransactionTypeCode.Delete;
            //    if (MosesClassLibrary.Utilities.Common.AskQuestion("Deleting this record will delete attached record.\nDo you want to continue?", ""))
            //    {
            //    }
            //    else
            //        tsbReload.PerformClick();
            //    boolIsUpdate = false;
            //}
            //else if (sender == tsbReload)
            //{
            //    iTransType = TransactionTypeCode.Reload;
            //    ShowForm();
            //}
            //bttnReset.PerformClick();
        }

        void OnFormLoad(object sender, EventArgs e)
        {
            //setDBComboBox();
        }

        public void RefreshForm()
        {
            NavBars.ToolStripEnableDisableControls(toolStrip, Tag as String);
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }


    }
}
