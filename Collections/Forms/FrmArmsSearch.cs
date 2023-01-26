using Collection.Classess;
using Collections;
using DevExpress.XtraGrid.Selection;
using DevExpress.XtraSplashScreen;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using TaxSmartSuite.Class;

namespace Collection.Forms
{
    public partial class FrmArmsSearch : Form
    {
        public static FrmArmsSearch publicStreetGroup;

        protected FrmArmsSearch iTransType;

        public static FrmArmsSearch publicInstance;

        private SqlDataAdapter adp;

        private SqlCommand _command;

        protected bool boolIsUpdate; DataTable temTable = new DataTable();

        private DataTable dt; GridCheckMarksSelection selection;

        bool isFirstGrid = true;

        public FrmArmsSearch()
        {
            SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

            InitializeComponent();

            publicInstance = this;

            publicStreetGroup = this;

            setImages();

            Load += OnFormLoad;

            ToolStripEvent();

            OnFormLoad(null, null);

            temTable.Columns.Add("PaymentRefNumber", typeof(string));
            temTable.Columns.Add("UserId", typeof(string));

            SplashScreenManager.CloseForm(false);
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

                //iTransType = TransactionTypeCode.Edit;

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
            checkcontionstring();
            radioGroup1.SelectedIndexChanged += RadioGroup1_SelectedIndexChanged;

            btnSelect.Click += BtnSelect_Click;

            btnSend.Click += BtnSend_Click;
        }

        private void BtnSend_Click(object sender, EventArgs e)
        {
            if (selection.SelectedCount == 0)
            {
                Common.setMessageBox("No Selection Made for Printing of Receipts", Program.ApplicationName, 3);
                return;
            }
            else
            {
                GetPayRef();
            }
        }

        private void BtnSelect_Click(object sender, EventArgs e)
        {
            string criteria = string.Empty;

            try
            {

                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

                if (radioGroup1.EditValue == null)
                {
                    Common.setEmptyField("Search Criteria, can't be empty", "Arms Payment Receipt Search");
                    return;
                }
                else if ((Int32)this.radioGroup1.EditValue == 1)
                {
                    //and ([EReceiptsDate] BETWEEN '{1} 00:00:00' AND '{1} 23:59:59')
                    criteria = String.Format("SELECT  [Payment Ref. Number] , [Payer Name] , Amount , CONVERT(VARCHAR, CONVERT(DATE, [Payment Date]), 103) AS [Payment Date], [Agency Name] , Description  FROM CollectionReport WHERE Provider='ICM' AND  ReceiptNo LIKE '%{0}%' AND  ([Payment Ref. Number] LIKE 'ogrc%' OR [Payment Ref. Number] LIKE '%icm%' OR [Payment Ref. Number] LIKE '%ogpd%') ", txtSearch.Text.Trim());
                }
                else if ((Int32)this.radioGroup1.EditValue == 2)
                {
                    criteria = String.Format("SELECT  [Payment Ref. Number] , [Payer Name] , Amount , CONVERT(VARCHAR, CONVERT(DATE, [Payment Date]), 103) AS [Payment Date],[Agency Name] , Description  FROM CollectionReport WHERE Provider='ICM' AND  [Payment Ref. Number] = '{0}' AND ([Payment Ref. Number] LIKE '%ogrc%' OR [Payment Ref. Number] LIKE '%icm%' OR [Payment Ref. Number] LIKE '%ogpd%')", txtSearch.Text.Trim());
                }
                else if ((Int32)this.radioGroup1.EditValue == 3)
                {
                    //this.label1.Text = "Payer Name";
                    criteria = String.Format("SELECT  [Payment Ref. Number] , [Payer Name] , Amount , CONVERT(VARCHAR, CONVERT(DATE, [Payment Date]), 103) AS [Payment Date],[Agency Name] , Description  FROM CollectionReport WHERE Provider='ICM'AND   [Payer Name] Like '%{0}%' AND ([Payment Ref. Number] LIKE '%ogrc%' OR [Payment Ref. Number] LIKE '%icm%' OR [Payment Ref. Number] LIKE '%ogpd%')", txtSearch.Text.Trim());
                }
                else if ((Int32)this.radioGroup1.EditValue == 4)
                {
                    criteria = String.Format("SELECT  [Payment Ref. Number] , [Payer Name] , Amount , CONVERT(VARCHAR, CONVERT(DATE, [Payment Date]), 103) AS [Payment Date],[Agency Name] , Description  FROM CollectionReport WHERE Provider='ICM' AND  [Deposit Slip Number] Like '%{0}%' AND ([Payment Ref. Number] LIKE '%ogrc%' OR [Payment Ref. Number] LIKE '%icm%' OR [Payment Ref. Number] LIKE '%ogpd%')", txtSearch.Text.Trim());
                }

                using (var ds = new System.Data.DataSet())
                {
                    using (SqlDataAdapter ada = new SqlDataAdapter(criteria, Logic.ConnectionString2))
                    {
                        ada.SelectCommand.CommandTimeout = 0;

                        ada.Fill(ds, "table");
                    }
                    dt = ds.Tables[0];

                }



                if (dt != null && dt.Rows.Count > 0)
                {
                    gridControl1.DataSource = dt;
                    gridView1.BestFitColumns();
                    gridView1.OptionsBehavior.Editable = false;
                    gridView1.Columns["Payment Date"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
                    gridView1.Columns["Payment Date"].DisplayFormat.FormatString = "dd/MM/yyyy";
                    gridView1.Columns["Amount"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                    gridView1.Columns["Amount"].DisplayFormat.FormatString = "n2";



                    if (isFirstGrid)
                    {
                        selection = new GridCheckMarksSelection(gridView1, ref lblSelect);
                        selection.CheckMarkColumn.VisibleIndex = 0;
                        isFirstGrid = false;
                    }
                }
                else
                    Common.setMessageBox("No Record found", "Arms Payment Receipt Search", 1); return;
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                SplashScreenManager.CloseForm(false);
            }

        }

        private void RadioGroup1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (radioGroup1.EditValue == null) return;
            if ((Int32)this.radioGroup1.EditValue == 1)
            {
                this.label3.Text = "Receipts No.";
                label2.Visible = false; txtSearch.Clear();

            }
            else if ((Int32)this.radioGroup1.EditValue == 2)
            {
                //txtSearch.Clear();
                this.label3.Text = "Payment Ref. No.";
                label2.Text = "(e.g LMFB|OGPDIPAY|0001|19-9-2013|143239 )";
                label2.Visible = true; txtSearch.Clear();

            }
            else if ((Int32)this.radioGroup1.EditValue == 3)
            {
                //txtSearch.Clear();
                this.label3.Text = "Payer Name";
                label2.Visible = false; txtSearch.Clear();

            }
            else if ((Int32)this.radioGroup1.EditValue == 4)
            {
                //this.label1.Text = "Receipts No.";
                this.label3.Text = "Deposit Slip Number";
                txtSearch.Enabled = true;

            }
        }

        public void RefreshForm()
        {
            NavBars.ToolStripEnableDisableControls(toolStrip, Tag as String);
        }

        void checkcontionstring()
        {
            string name = System.Environment.MachineName;

            string filePath = "C://TSS//TaxSmartSuiteNew//TaxSmartSuiteNew//Configuration.xml";
            //var config = new TaxSmartConfiguration.ConfigManager("C://TSS//TaxSmartSuiteNew", true);
            TaxSmartConfiguration.ConfigManager config2 = new TaxSmartConfiguration.ConfigManager(filePath, true);
            if (!config2.IsDefaultFolderExist)
            {
                //Load Database Settings
                TaxSmartConfiguration.Winform.FrmDatabaseSetup frmDatabaseSetup = new TaxSmartConfiguration.Winform.FrmDatabaseSetup(filePath, true);
                frmDatabaseSetup.ShowDialog();
                if (!frmDatabaseSetup.Status)
                {
                    return;
                }
            }
            if (!Logic.LoadConfig2())
                return;

        }

        string LoadConfig(string filePath)
        {
            string connString = null;
            var status = TaxSmartConfiguration.ConfigManager.CheckDatabaseConnectionSettings(ref connString, filePath, true);
            return connString;
        }

        void GetPayRef()
        {
            string values = string.Empty;

            lblSelect.Text = string.Empty;

            int j = 0;

            temTable.Clear();

            for (int i = 0; i < selection.SelectedCount; i++)
            {
                temTable.Rows.Add(new object[] { String.Format("{0}", (selection.GetSelectedRow(i) as DataRowView)["Payment Ref. Number"]), Program.UserID });

            }

            using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
            {
                connect.Open();
                _command = new SqlCommand("ArmsReceiptsMoved", connect) { CommandType = CommandType.StoredProcedure };

                _command.Parameters.Add(new SqlParameter("@pTransaction", SqlDbType.Structured)).Value = temTable;

                _command.CommandTimeout = 0;

                System.Data.DataSet response = new System.Data.DataSet();

                response.Clear();

                adp = new SqlDataAdapter(_command);
                adp.Fill(response);

                connect.Close();

                if (String.Compare(response.Tables[0].Rows[0]["returnCode"].ToString(), "00", false) == 0)
                {
                    Common.setMessageBox(response.Tables[0].Rows[0]["returnmessage"].ToString(), Program.ApplicationName, 1);

                    if (response.Tables[1] != null || response.Tables[1].Rows.Count > 0)
                    {
                        using (FrmArmsDisplay display = new FrmArmsDisplay(response, true) { FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog })
                        {
                            display.ShowDialog();
                        }
                    }


                }
                else
                {
                    Common.setMessageBox(response.Tables[0].Rows[0]["returnmessage"].ToString(), Program.ApplicationName, 1);
                    return;
                }

            }

        }


    }
}
