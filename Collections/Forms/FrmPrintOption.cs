using Collection.Classess;
using Collection.Report;
using Collections;
using DevExpress.XtraReports.UI;
using DevExpress.XtraSplashScreen;
using System;
using System.Data.SqlClient;
using System.Windows.Forms;
using TaxSmartSuite.Class;

namespace Collection.Forms
{
    public partial class FrmPrintOption : Form
    {
        public static FrmPrintOption publicStreetGroup;

        protected TransactionTypeCode iTransType;

        public static FrmPrintOption publicInstance;

        protected bool boolIsUpdate;

        string BatchNumber;

        private string SQL;

        private bool Isbank = false;

        public FrmPrintOption()
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

                btnSelect.Click += Bttn_Click;

                //cboDate.KeyPress += cboDate_KeyPress;

                //cboDate.SelectedIndexChanged += cboDate_SelectedIndexChanged;
                dtpStart.ValueChanged += dtpStart_ValueChanged;

                dtpStart.Leave += dtpStart_Leave;
                OnFormLoad(null, null);
            }
            finally
            {

                SplashScreenManager.CloseForm(false);
            }

        }

        void dtpStart_Leave(object sender, EventArgs e)
        {
            getCheckedit();
        }

        void dtpStart_ValueChanged(object sender, EventArgs e)
        {

        }

        void cboDate_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (cboDate.SelectedValue != null && !Isbank) 
            //{
            //    getCheckedit();
            //}
        }


        //public void setDBComboBox()
        //{
        //    using (var ds = new System.Data.DataSet())
        //    {
        //        using (SqlDataAdapter ada = new SqlDataAdapter((string)string.Format("SELECT DISTINCT CONVERT(VARCHAR, CONVERT(DATE, IsPrintedDate), 103) as IsPrintedDate FROM Receipt.tblCollectionReceipt ORDER by CONVERT(VARCHAR, CONVERT(DATE, IsPrintedDate), 103) ASC "), Logic.ConnectionString))
        //        {
        //            ada.Fill(ds, "table");
        //        }
        //        Common.setComboList(cboDate, ds.Tables[0], "IsPrintedDate", "IsPrintedDate");

        //    }

        //    cboDate.SelectedIndex = -1;


        //}

        private void setImages()
        {
            tsbNew.Image = MDIMain.publicMDIParent.i16x16.Images[5];
            tsbEdit.Image = MDIMain.publicMDIParent.i16x16.Images[6];
            tsbDelete.Image = MDIMain.publicMDIParent.i16x16.Images[7];
            tsbReload.Image = MDIMain.publicMDIParent.i16x16.Images[0];
            tsbClose.Image = MDIMain.publicMDIParent.i16x16.Images[11];

            //bttnCancel.Image = MDIMain.publicMDIParent.i32x32.Images[9];
            //btnSelect.Image = MDIMain.publicMDIParent.i32x32.Images[8];
            btnSelect.Image = MDIMain.publicMDIParent.i32x32.Images[29];
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

        void Bttn_Click(object sender, EventArgs e)
        {
            if (sender == btnSelect)
            {
                //if (checkedComboBoxEdit1.EditValue.ToString() == null || checkedComboBoxEdit1.EditValue.ToString() == "")
                //{
                //    Common.setEmptyField("Printed Batches ", Program.ApplicationName);
                //    checkedComboBoxEdit1.Focus(); return;
                //}
                if (string.IsNullOrEmpty(txtSearch.Text.ToString()))
                {
                    Common.setMessageBox("Receipt Number", Program.ApplicationName, 3); return;
                }
                else
                {
                    try
                    {
                        SQL = String.Format("SELECT BatchNumber FROM Receipt.tblCollectionReceipt where EReceipts ='{0}'", txtSearch.Text.Trim()); ;

                        BatchNumber = (new Logic()).ExecuteScalar(SQL);
                    }
                    catch (Exception ex)
                    {
                        Tripous.Sys.ErrorBox(String.Format("Search Receipt for Mainfest with EReceipts number...{0}{1}", ex.Message, ex.StackTrace));
                        return;
                    }


                    if (!string.IsNullOrEmpty(BatchNumber))
                    {
                        var vquery = String.Format(@"SELECT [ID] ,
                                         [Provider] ,
                                         [Channel] ,
                                         Collection.tblCollectionReport.[PaymentRefNumber] ,
                                         [DepositSlipNumber] ,
                                         CONVERT(VARCHAR,CONVERT(DATE,[PaymentDate]),103) AS PaymentDate,
                                         [PayerID] ,
                                         UPPER([PayerName]) AS [PayerName],
                                         [Amount] ,
                                         [PaymentMethod] ,
                                         [ChequeNumber] ,
                                         [ChequeValueDate] ,
                                         [ChequeStatus] ,
                                         [DateChequeReturned] ,
                                         [TelephoneNumber] ,
                                         [ReceiptNo] ,
                                                                                  [PayerAddress] ,
                                          [User] ,
                                         [RevenueCode] ,
                                         [Description] ,
                                         [ChequeBankCode] ,
                                         [ChequeBankName] ,
                                         [AgencyName] ,
                                         [AgencyCode] ,
                                         [BankCode] ,
                                         [BankName] ,
                                         [BranchCode] ,
                                         [BranchName] ,                                      
                                         [ZoneCode] ,
                                         [ZoneName] ,
                                         [Username] ,
                                         [AmountWords] ,
                                         Collection.tblCollectionReport.[EReceipts] ,
                                         Collection.tblCollectionReport.[EReceiptsDate] ,
                                         [GeneratedBy] ,
                                         Receipt.tblCollectionReceipt.[UploadStatus] ,
                                         [PrintedBY] ,
                                         [ControlNumber] ,
                                         [BatchNumber] ,
                                         Collection.tblCollectionReport.[StationCode] ,(SELECT TOP 1 StationName FROM Receipt.tblStation WHERE StationCode=StationCode) AS  [StationName],IsPrintedDate AS ReceiptDate FROM Collection.tblCollectionReport  INNER JOIN Receipt.tblCollectionReceipt ON Collection.tblCollectionReport.PaymentRefNumber = Receipt.tblCollectionReceipt.PaymentRefNumber  WHERE Receipt.tblCollectionReceipt.BatchNumber in ('{0}') ORDER BY Collection.tblCollectionReport.AgencyCode , Collection.tblCollectionReport.RevenueCode, Collection.tblCollectionReport.EReceipts", BatchNumber);

                        try
                        {
                            using (System.Data.DataSet dds = new System.Data.DataSet("DsCollectionReport"))
                            {
                                using (SqlDataAdapter ada = new SqlDataAdapter(vquery, Logic.ConnectionString))
                                {
                                    ada.Fill(dds, "CollectionReportTable");
                                    XRepManifest repManifest = new XRepManifest
                                    {
                                        DataSource = dds,
                                        DataAdapter = ada,
                                        DataMember = "CollectionReportTable",
                                        RequestParameters = false
                                    };
                                    repManifest.xrLabel10.Text = Program.UserID;
                                    repManifest.logoPath = Logic.logopth;
                                    repManifest.xrLabel12.Text = "MANIFEST OF PRINTED RECEIPTS";
                                    repManifest.xrLabel9.Text = string.Format("{0} STATE GOVERNMENT",
                                                        Program.StateName.ToUpper());
                                    repManifest.ShowPreviewDialog();
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Tripous.Sys.ErrorBox(String.Format("Calling Report with Batch number...., {0}{1}", ex.Message, ex.StackTrace));
                            return;
                        }

                    }
                    else
                    {
                        Common.setMessageBox("Receipt Number does not Exist !", Program.ApplicationName, 3);
                    }
                }



            }



        }

        private void FrmPrintOption_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'reportDs1.tblCollectionReport' table. You can move, or remove it, as needed.
            //this.tblCollectionReportTableAdapter.Fill(this.reportDs1.tblCollectionReport);

        }

        public void RefreshForm()
        {
            NavBars.ToolStripEnableDisableControls(toolStrip, Tag as String);
        }

        void getCheckedit()
        {
            using (var ds = new System.Data.DataSet())
            {
                using (SqlDataAdapter ada = new SqlDataAdapter((string)string.Format("SELECT DISTINCT BatchNumber FROM Receipt.tblCollectionReceipt WHERE CONVERT(VARCHAR, CONVERT(DATE, IsPrintedDate), 103)='{0}'", string.Format("{0:dd/MM/yyyy}", dtpStart.Value)), Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }
                //Common.setComboList(checkedComboBoxEdit1, ds.Tables[0], "IsPrintedDate", "IsPrintedDate");

                Common.setCheckEdit(checkedComboBoxEdit1, ds.Tables[0], "BatchNumber", "BatchNumber");

            }
        }
    }
}
