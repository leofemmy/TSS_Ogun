using Collection.Classess;
using Collection.Report;
using Collections;
using DevExpress.XtraReports.UI;
using DevExpress.XtraSplashScreen;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using TaxSmartSuite.Class;

namespace Collection.Forms
{
    public partial class FrmPayment : Form
    {
        protected TransactionTypeCode iTransType;

        public static FrmPayment publicInstance;

        public static bool boolIsUpdate;
        public FrmPayment()
        {
            SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

            InitializeComponent();

            publicInstance = this;

            setImages();

            ToolStripEvent();

            Load += OnFormLoad;

            if (boolIsUpdate)
            {
                groupBox1.Text = "Receipt Exception Report";
            }
            else
            {
                groupBox1.Text = "Payment Report";
            }

            radioGroup1.SelectedIndexChanged += radioGroup1_SelectedIndexChanged;

            btnPreview.Click += btnPreview_Click;

            SplashScreenManager.CloseForm(false);
        }

        void btnPreview_Click(object sender, EventArgs e)
        {
            string criteria = string.Empty;

            if (string.IsNullOrEmpty(radioGroup1.EditValue.ToString()))
            {
                Common.setMessageBox("Search Criteria Option not Selected....!", Program.ApplicationName, 2);
                return;
            }
            else
            {
                try
                {
                    SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);


                    if (boolIsUpdate)
                    {
                        //exception report
                        if ((Int32)this.radioGroup1.EditValue == 0)
                        {
                            criteria = string.Format("SELECT Collection.tblCollectionReport.PaymentRefNumber , DepositSlipNumber ,  PaymentDate , [PayerID] , UPPER(PayerName) AS PayerName ,AgencyName ,  Description ,  Amount ,BankName + ',' + BranchName AS Bank , Collection.tblCollectionReport.EReceipts ,Collection.tblCollectionReport.StationCode ,( SELECT TOP 1 StationName FROM Receipt.tblStation WHERE tblStation.StationCode = Collection.tblCollectionReport.[StationCode]) AS StationName FROM Collection.tblCollectionReport WHERE  Collection.tblCollectionReport.EReceiptsDate >='{0}' And Collection.tblCollectionReport.EReceiptsDate  <= '{1}'  AND RevenueCode IN ( SELECT RevenueCode  FROM   Receipt.tblRevenueReceiptException ) ORDER BY Collection.tblCollectionReport.AgencyName ,        Collection.tblCollectionReport.Description , Collection.tblCollectionReport.EReceipts", string.Format("{0:MM/dd/yyyy 00:00:00}", dtpDate.Value), string.Format("{0:MM/dd/yyyy 23:59:59}", dtpenddate.Value));
                        }
                        else if ((Int32)this.radioGroup1.EditValue == 1)
                        {
                            criteria = string.Format("SELECT Collection.tblCollectionReport.PaymentRefNumber , DepositSlipNumber ,  PaymentDate , [PayerID] , UPPER(PayerName) AS PayerName ,AgencyName ,  Description ,  Amount ,BankName + ',' + BranchName AS Bank , Collection.tblCollectionReport.EReceipts ,Collection.tblCollectionReport.StationCode ,( SELECT TOP 1 StationName FROM Receipt.tblStation WHERE tblStation.StationCode = Collection.tblCollectionReport.[StationCode]) AS StationName FROM Collection.tblCollectionReport WHERE  Collection.tblCollectionReport.PaymentRefNumber Like '%{0}%'  AND RevenueCode IN ( SELECT RevenueCode  FROM   Receipt.tblRevenueReceiptException ) ORDER BY Collection.tblCollectionReport.AgencyName ,        Collection.tblCollectionReport.Description , Collection.tblCollectionReport.EReceipts", txtpayRef.Text.Trim());
                        }
                    }
                    else
                    {
                        //pyment report

                        //groupBox1.Text = "Payment Report";
                        if ((Int32)this.radioGroup1.EditValue == 0)
                        {
                            criteria = string.Format("SELECT  Collection.tblCollectionReport.PaymentRefNumber , DepositSlipNumber , PaymentDate , [PayerID] ,UPPER(PayerName) AS PayerName , AgencyName , Description ,  Amount , BankName + ',' + BranchName AS Bank , Collection.tblCollectionReport.EReceipts , ControlNumber , ControlNumberBy , ControlNumberDate , PrintedBY , Collection.tblCollectionReport.StationCode , ( SELECT TOP 1     StationName FROM  Receipt.tblStation  WHERE  tblStation.StationCode = Collection.tblCollectionReport.[StationCode]) AS StationName FROM Collection.tblCollectionReport LEFT JOIN Receipt.tblCollectionReceipt ON Receipt.tblCollectionReceipt.PaymentRefNumber = Collection.tblCollectionReport.PaymentRefNumber WHERE Collection.tblCollectionReport.EReceiptsDate >='{0}' And Collection.tblCollectionReport.EReceiptsDate  <= '{1}'  ORDER BY Collection.tblCollectionReport.AgencyName ,        Collection.tblCollectionReport.Description , Collection.tblCollectionReport.EReceipts", string.Format("{0:MM/dd/yyyy 00:00:00}", dtpDate.Value), string.Format("{0:MM/dd/yyyy 23:59:59}", dtpenddate.Value));
                        }
                        else if ((Int32)this.radioGroup1.EditValue == 1)
                        {
                            criteria = string.Format("SELECT  Collection.tblCollectionReport.PaymentRefNumber , DepositSlipNumber , PaymentDate , [PayerID] ,UPPER(PayerName) AS PayerName , AgencyName , Description ,  Amount , BankName + ',' + BranchName AS Bank , Collection.tblCollectionReport.EReceipts , ControlNumber , ControlNumberBy , ControlNumberDate , PrintedBY , Collection.tblCollectionReport.StationCode , ( SELECT TOP 1     StationName FROM  Receipt.tblStation  WHERE  tblStation.StationCode = Collection.tblCollectionReport.[StationCode]) AS StationName FROM Collection.tblCollectionReport LEFT JOIN Receipt.tblCollectionReceipt ON Receipt.tblCollectionReceipt.PaymentRefNumber = Collection.tblCollectionReport.PaymentRefNumber WHERE  Collection.tblCollectionReport.PaymentRefNumber Like '%{0}%'  ORDER BY Collection.tblCollectionReport.AgencyName ,        Collection.tblCollectionReport.Description , Collection.tblCollectionReport.EReceipts", txtpayRef.Text.Trim());
                        }
                    }

                    using (var ds = new System.Data.DataSet())
                    {
                        ds.Clear();

                        using (SqlDataAdapter ada = new SqlDataAdapter(criteria, Logic.ConnectionString))
                        {
                            ada.Fill(ds, "table");
                        }

                        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            if (boolIsUpdate)
                            {
                                if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                                {
                                    ////using object class
                                    var listexpection = (from DataRow row in ds.Tables[0].Rows
                                                         select new DataSet.Reports
                                                         {
                                                             PaymentRefNumber = row["PaymentRefNumber"] as string,
                                                             DepositSlipNumber = row["DepositSlipNumber"] as string,
                                                             PaymentDate = Convert.ToDateTime(row["PaymentDate"]),
                                                             PayerID = row["PayerID"] as string,
                                                             PayerName = row["PayerName"] as string,
                                                             AgencyName = row["AgencyName"] as string,
                                                             Description = row["Description"] as string,
                                                             Amount = Convert.ToDecimal(row["Amount"]),
                                                             Bank = row["Bank"] as string,
                                                             EReceipts = row["EReceipts"] as string,
                                                             StationCode = row["StationCode"] as string,
                                                             StationName = row["StationName"] as string
                                                         }
              ).ToList();

                                    XtraRepExecptionReport report = new XtraRepExecptionReport();
                                    var bindsoucre = (BindingSource)report.DataSource;
                                    bindsoucre.Clear();
                                    bindsoucre.DataSource = listexpection;
                                    report.xrLabel12.Text = string.Format("{0} State Government", Program.StateName.ToUpper());
                                    report.xrLabel13.Text = string.Format("List of Revenue Receipts Exception between {0} and {1}", string.Format("{0:dd/MM/yyyy}", dtpDate.Value), string.Format("{0:dd/MM/yyyy}", dtpenddate.Value));
                                    report.ShowPreviewDialog();

                                }
                                else
                                {
                                    Common.setMessageBox("No Record Found", Program.ApplicationName, 1); return;
                                }
                            }
                            else
                            {
                                if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                                {
                                    var list = (from DataRow rows in ds.Tables[0].Rows
                                                select new DataSet.Reports2
                                                {
                                                    PaymentRefNumber = rows["PaymentRefNumber"] as string
                                                    ,
                                                    DepositSlipNumber = rows["DepositSlipNumber"] as string,
                                                    PaymentDate = Convert.ToDateTime(rows["PaymentDate"]),
                                                    PayerID = rows["PayerID"] as string,
                                                    PayerName = rows["PayerName"] as string,
                                                    AgencyName = rows["AgencyName"] as string,
                                                    Description = rows["Description"] as string
                                                    ,
                                                    Amount = Convert.ToDecimal(rows["Amount"]),
                                                    Bank = rows["Bank"] as string,
                                                    EReceipts = rows["EReceipts"] as string,
                                                    StationCode = rows["StationCode"] as string,
                                                    StationName = rows["StationName"] as string,
                                                    ControlNumber = rows["ControlNumber"] as string,
                                                    //ControlNumberDate = Convert.ToDateTime(rows["ControlNumberDate"]),
                                                    PrintedBY = rows["PrintedBY"] as string
                                                }
                                        ).ToList();

                                    XtraRepPayments report = new XtraRepPayments();

                                    var bindsoucre = (BindingSource)report.DataSource;
                                    bindsoucre.Clear();
                                    bindsoucre.DataSource = list;
                                    report.xrLabel12.Text = string.Format("{0} State Government", Program.StateName.ToUpper());
                                    report.xrLabel13.Text = string.Format("List of Payment Collections Reports between {0} and {1}", string.Format("{0:dd/MM/yyyy}", dtpDate.Value), string.Format("{0:dd/MM/yyyy}", dtpenddate.Value));
                                    report.ShowPreviewDialog();

                                }
                                else
                                {
                                    Common.setMessageBox("No Record Found", Program.ApplicationName, 1); return;
                                }
                            }
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
        }

        void radioGroup1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (radioGroup1.EditValue == null) return;

            if ((Int32)this.radioGroup1.EditValue == 0)
            {
                label1.Visible = false; txtpayRef.Visible = false;
                label2.Visible = false;
                label3.Visible = true; label4.Visible = true;
                dtpDate.Visible = true; dtpenddate.Visible = true; dtpDate.Enabled = true; dtpenddate.Enabled = true;
            }
            else if ((Int32)this.radioGroup1.EditValue == 1)
            {

                label1.Visible = true; txtpayRef.Visible = true;
                label3.Visible = false; label4.Visible = false;
                dtpDate.Visible = false; dtpenddate.Visible = false; label2.Visible = true;
                label1.Location = new Point(9, 54);
                txtpayRef.Location = new Point(12, 70); label2.Location = new Point(9, 97);

            }
        }

        public void RefreshForm()
        {
            NavBars.ToolStripEnableDisableControls(toolStrip, Tag as String);
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
            btnPreview.Image = MDIMain.publicMDIParent.i32x32.Images[2];
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

    }
}
