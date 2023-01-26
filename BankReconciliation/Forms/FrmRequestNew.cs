using BankReconciliation.Class;
using BankReconciliation.Report;
using DevExpress.XtraGrid.Selection;
using DevExpress.XtraSplashScreen;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;
using TaxSmartSuite.Class;

namespace BankReconciliation.Forms
{
    public partial class FrmRequestNew : Form
    {
        private SqlCommand _command;
        private SqlDataAdapter adp;
        public static FrmRequestNew publicStreetGroup;

        GridCheckMarksSelection selection;

        bool isFirstGrid = true; DateTime dt = new DateTime();

        string strtoken;

        string sValue = ""; string sValue1 = ""; string sValue2 = ""; int sValue3 = 0; string sValue4 = ""; int postrequested = 0;
        public FrmRequestNew()
        {
            SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

            InitializeComponent();

            publicStreetGroup = this;

            setImages();

            ToolStripEvent();

            Load += OnFormLoad;

            OnFormLoad(null, null);

            cboBankName.Focus();

            SplashScreenManager.CloseForm(false);
        }

        private void setImages()
        {
            tsbNew.Image = MDIMains.publicMDIParent.i16x16.Images[5];
            tsbEdit.Image = MDIMains.publicMDIParent.i16x16.Images[6];
            tsbDelete.Image = MDIMains.publicMDIParent.i16x16.Images[7];
            tsbReload.Image = MDIMains.publicMDIParent.i16x16.Images[0];
            tsbClose.Image = MDIMains.publicMDIParent.i16x16.Images[11];

            //bttnCancel.Image = MDIMains.publicMDIParent.i32x32.Images[9];
            ////bttnReset.Image = MDIMains.publicMDIParent.i32x32.Images[8];
            ////bttnUpdate.Image = MDIMain.publicMDIParent.i32x32.Images[7];
            //bttnUpdate.Image = MDIMains.publicMDIParent.i32x32.Images[7];
            //btnAllocate.Image = MDIMains.publicMDIParent.i32x32.Images[6];
            btnApprove.Image = MDIMains.publicMDIParent.i32x32.Images[6];
            btnDisapprove.Image = MDIMains.publicMDIParent.i32x32.Images[6];
            btnToken.Image = MDIMains.publicMDIParent.i32x32.Images[7];
            //bttncompare.Image = MDIMains.publicMDIParent.i32x32.Images[8];
            //bttnClose.Image = MDIMain.publicMDIParent.i32x32.Images[9];
            //bttnSave.Image = MDIMains.publicMDIParent.i32x32.Images[7];

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
                MDIMains.publicMDIParent.RemoveControls();

            }
            else if (sender == tsbNew)
            {
                //groupControl2.Text = "Add New Record";
                //iTransType = TransactionTypeCode.New;
                //ShowForm();
                //boolIsUpdate = false;
            }
            else if (sender == tsbEdit)
            {
                //groupControl2.Text = "Edit Record Mode";
                //iTransType = TransactionTypeCode.Edit;
                //if (EditRecordMode())
                //{
                //    ShowForm();
                //    boolIsUpdate = true;
                //}
            }
            else if (sender == tsbDelete)
            {
                //groupControl2.Text = "Disable Record Mode";
                //iTransType = TransactionTypeCode.Delete;
                //if (MosesClassLibrary.Utilities.Common.AskQuestion("Disable this record will delete attached record.\nDo you want to continue?", ""))
                //{
                //    if (string.IsNullOrEmpty(ID.ToString()))
                //    {
                //        Common.setMessageBox("No Record Selected for Disable", Program.ApplicationName, 3);
                //        return;
                //    }
                //    else
                //        //deleteRecord(ID);
                //}
                //else
                tsbReload.PerformClick();
                //boolIsUpdate = false;
            }
            else if (sender == tsbReload)
            {
                //iTransType = TransactionTypeCode.Reload; setReload();
                //ShowForm();
            }
            //bttnReset.PerformClick();
        }

        void OnFormLoad(object sender, EventArgs e)
        {
            setDBComboBoxRev();

            //tblTransactionPostingRequestViewPostBankStatmentTableAdapters.ViewPostingRequestBankTableAdapter;
            //viewPostingRequestBankTableAdapter.Connection.ConnectionString = Logic.ConnectionString;
            //tblTransactionPostingRequestViewPostBankStatment.con

            cboBankName.KeyPress += comboBox1_KeyPress;

            cboBankName.SelectedIndexChanged += cboBankName_SelectedIndexChanged;

            btnApprove.Click += btnApprove_Click; btnDisapprove.Click += btnDisapprove_Click;

            btnToken.Click += BtnToken_Click;
        }

        private void BtnToken_Click(object sender, EventArgs e)
        {
            //    if (dotoken())
            //    {
            //        btnApprove.Enabled = true;
            //    }
            //    else
            //    {
            //        btnApprove.Enabled = false; BtnToken_Click(null, null);
            //    }

        }

        void btnDisapprove_Click(object sender, EventArgs e)
        {
            string value = string.Empty;

            if (DialogResults.InputBox(@"Comments for Disapproving ", "Transscation Disapproval", ref value) == DialogResult.OK)
            {
                //value = String.Format("{0:N2}", Convert.ToDecimal(value));

                if (!string.IsNullOrWhiteSpace(value))
                {
                    using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
                    {
                        connect.Open();
                        _command = new SqlCommand("doTransactionDisapproval", connect) { CommandType = CommandType.StoredProcedure };
                        _command.Parameters.Add(new SqlParameter("@userid", SqlDbType.VarChar)).Value = Program.UserID;
                        _command.Parameters.Add(new SqlParameter("@UserComment", SqlDbType.VarChar)).Value = value;
                        _command.Parameters.Add(new SqlParameter("@PostingRequestID", SqlDbType.Int)).Value = Convert.ToInt32(postrequested);

                        //@Years
                        using (System.Data.DataSet ds = new System.Data.DataSet())
                        {
                            ds.Clear();
                            adp = new SqlDataAdapter(_command);
                            adp.Fill(ds);
                            connect.Close();

                            if (ds.Tables[0].Rows[0]["returnCode"].ToString() != "00")
                            {
                                Common.setMessageBox(ds.Tables[0].Rows[0]["returnMessage"].ToString(), Program.ApplicationName, 2);

                                return;

                            }
                            else
                            {
                                Common.setMessageBox(ds.Tables[0].Rows[0]["returnMessage"].ToString(), Program.ApplicationName, 2);
                                //return;

                            }

                        }
                    }
                }
                else
                {
                    Common.setMessageBox("Disapproval Comment is Empty", "Transscation Disapproval", 3);

                    return;
                }
            }

            setDBComboBoxRev(); documentViewer1.DocumentSource = null;
            //    using (SqlConnection db = new SqlConnection(Logic.ConnectionString))
            //{
            //    SqlTransaction transaction;

            //    db.Open();

            //    transaction = db.BeginTransaction();

            //    try
            //    {
            //        using (SqlCommand sqlCommand1 = new SqlCommand(string.Format("update Reconciliation.tblTransactionPostingRequest set IsApproved=0 where BankShortCode='{0}' and BatchCode='{1}'", sValue.Trim(), sValue1.Trim()), db, transaction))
            //        {
            //            sqlCommand1.ExecuteNonQuery();
            //        }

            //        transaction.Commit();
            //    }
            //    catch (Exception ex)
            //    {
            //        transaction.Rollback();
            //        Tripous.Sys.ErrorBox(ex);
            //    }

            //    db.Close();



            //    return;
            //}
        }

        void processApprove()
        {
            //DialogResult result = MessageBox.Show(string.Format("Has this bank reconciliation {0} for {1} period description, pass the approve stage.Can it now be Approve?", sValue2, sValue1), "Bank Reconciliation Approve Stage", MessageBoxButtons.YesNo);
            DialogResult result = MessageBox.Show(string.Format("Click Yes in agreement that the information on this approval page is genuine, else click No to cancel"), "Bank Reconciliation Approve Stage", MessageBoxButtons.YesNo);

            if (result == DialogResult.Yes)
            {
                using (SqlConnection db = new SqlConnection(Logic.ConnectionString))
                {
                    SqlTransaction transaction;

                    db.Open();

                    transaction = db.BeginTransaction();

                    try
                    {
                        using (SqlCommand sqlCommand1 = new SqlCommand(string.Format("update Reconciliation.tblTransactionPostingRequest set IsApproved=1,ApprovedBy='{0}',ApprovedDate='{1}' where BankShortCode='{2}' and FinancialperiodID='{3}'", Program.UserID, string.Format("{0:yyyy/MM/dd hh:mm:ss tt}", DateTime.Now), sValue.Trim(), sValue3), db, transaction))
                        {
                            sqlCommand1.ExecuteNonQuery();
                        }

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        Tripous.Sys.ErrorBox(ex);
                    }

                    db.Close();

                    Common.setMessageBox("Record Approved", "Bank Reconciliation Approve Stage", 1);

                    setDBComboBoxRev(); documentViewer1.DocumentSource = null;

                    return;
                }
            }
            else
                return;
        }

        void dowork()
        {
            string value = string.Empty;

            strtoken = string.Empty;

            if (!Token.dotoken())
            {
                if (Program.loginid == 0) return;

            }

            //if (Token.dotoken())
            //{
            string strtime = dt.ToString("mm");

            if (DialogResults.InputBox(@"OTP", string.Format("Kindly enter the token to Authorize this transaction.", $"********{Program.Userphone.Substring(7)}"), ref value) == DialogResult.OK)
            {
                if (Token.tokenInsertValidation(Program.UserID, Program.ApplicationCode, value.ToString(), true, string.Format("{0}..{1}..{2}...{3}..{4}", this.groupControl1.Text.Trim(), sValue2, sValue4, sValue3, postrequested)))
                //if (Token.validatetoken(value.ToString()))
                {
                    processApprove();
                    btnApprove.Enabled = true;
                }
                else
                {
                    ////false
                    //dowork();
                    btnApprove.Enabled = true;
                    //BtnToken_Click(null, null);

                }
            }
            //}
            //else
            //    dowork();

            //}

            //if (dotoken())
            //{


        }

        void btnApprove_Click(object sender, EventArgs e)
        {
            dowork();

        }

        void cboBankName_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataRowView oDataRowView = cboBankName.SelectedItem as DataRowView;



            if (oDataRowView != null)
            {
                sValue = oDataRowView.Row["BankShortCode"] as string;

                sValue1 = oDataRowView.Row["BatchCode"] as string;

                sValue2 = oDataRowView.Row["BankName"] as string;

                sValue4 = oDataRowView.Row["BankAccountID"] as string;

                sValue3 = Convert.ToInt32(oDataRowView.Row["FinancialperiodID"]);

                postrequested = Convert.ToInt32(oDataRowView.Row["PostingRequestID"]);

                using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
                {
                    connect.Open();
                    _command = new SqlCommand("doGetRequestPostingDetails", connect) { CommandType = CommandType.StoredProcedure };
                    _command.Parameters.Add(new SqlParameter("@Bankcode", SqlDbType.VarChar)).Value = oDataRowView.Row["BankShortCode"] as string;
                    _command.Parameters.Add(new SqlParameter("@Periods", SqlDbType.Int)).Value = Convert.ToInt32(oDataRowView.Row["FinancialperiodID"]);
                    _command.Parameters.Add(new SqlParameter("@BankAccountID", SqlDbType.Int)).Value = Convert.ToInt32(oDataRowView.Row["BankAccountID"]);

                    //@Years
                    using (System.Data.DataSet ds = new System.Data.DataSet())
                    {
                        ds.Clear();
                        adp = new SqlDataAdapter(_command);
                        adp.Fill(ds);
                        connect.Close();

                        if (ds.Tables[0].Rows[0]["returnCode"].ToString() != "00")
                        {
                            Common.setMessageBox(ds.Tables[0].Rows[0]["returnMessage"].ToString(), Program.ApplicationName, 2);

                            return;

                        }
                        else
                        {
                            if (ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0)
                            {
                                var list = (from DataRow row in ds.Tables[1].Rows
                                            select new Dataset.PostingRequest
                                            {
                                                Openingbal = Convert.ToDecimal(row["OpeningBalance"]),
                                                closingbal = Convert.ToDecimal(row["ClosingBalance"]),
                                                Debit = Convert.ToDecimal(row["Debit"]),
                                                Credit = Convert.ToDecimal(row["Credit"]),
                                                BankCode = row["BankCode"] as string,
                                                BankName = row["BankName"] as string,
                                                BatchCode = row["BatchCode"] as string,
                                                BatchName = row["BatchName"] as string,
                                                Description = row["Description"] as string,
                                                Type = row["Type"] as string,
                                                TransID = Convert.ToInt16(row["TransID"]),
                                                Acctnumber = row["Acctnumber"] as string,
                                                ActDescription = row["ActDescription"] as string,
                                                startdate = Convert.ToDateTime(row["startdate"]),
                                                enddate = Convert.ToDateTime(row["enddate"]),
                                                PostingRequestID = Convert.ToInt32(row["PostingRequestID"])
                                            }
                                               ).ToList();

                                //var lists = (from DataRow rows in ds.Tables[2].Rows
                                //             select new Dataset.Bank
                                //             {
                                //                 Amount = Convert.ToDecimal(rows["Amount"]),
                                //                 BSDate = Convert.ToDateTime(rows["BSDate"]),
                                //                 PostingRequestID = Convert.ToInt32(rows["PostingRequestID"])
                                //             }).ToList();


                                xtraRepRequest requestreport = new xtraRepRequest();
                                var binding = (BindingSource)requestreport.DataSource;
                                binding.DataSource = list;
                                //var binding2 = (BindingSource)requestreport.DataSource;
                                //binding2.DataSource = lists;
                                documentViewer1.DocumentSource = requestreport;
                                //requestreport.ShowPreviewDialog();
                                requestreport.CreateDocument(true);
                                documentViewer1.Show();

                            }
                            else
                            {
                                Common.setMessageBox("No Details for the Selected Bank / Transaction Allocation Not done for selected Bank.", Program.ApplicationName, 1); return;
                            }
                        }

                    }
                }
            }




        }

        void comboBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            Methods.AutoComplete(cboBankName, e, true);
        }

        public void RefreshForm()
        {
            NavBars.ToolStripEnableDisableControls(toolStrip, Tag as String);
        }

        public void setDBComboBoxRev()
        {
            DataTable Dt;

            using (var ds = new System.Data.DataSet())
            {

                using (SqlDataAdapter ada = new SqlDataAdapter((string)@"
SELECT  PostingRequestID ,
        Reconciliation.tblTransactionPostingRequest.BankShortCode ,
        Reconciliation.tblFinancialperiod.Months + ','
        + Reconciliation.tblFinancialperiod.Year AS BatchName ,
        OpeningBalance ,
        ClosingBalance ,
        Reconciliation.tblTransactionPostingRequest.FinancialperiodID ,
        Reconciliation.tblFinancialperiod.Periods + '/'
        + Reconciliation.tblFinancialperiod.Year AS BatchCode ,
        Reconciliation.tblTransactionPostingRequest.BankAccountID ,
        dbo.ViewBankBranchAccount.BankName + ' - '
        + ViewBankBranchAccount.AccountNumber + ' - {'
        + ViewBankBranchAccount.Description + '}' AS BankName
FROM    Reconciliation.tblTransactionPostingRequest
        INNER JOIN Reconciliation.tblFinancialperiod ON Reconciliation.tblFinancialperiod.FinancialperiodID = Reconciliation.tblTransactionPostingRequest.FinancialperiodID
        INNER JOIN Collection.tblBank ON Collection.tblBank.BankShortCode = Reconciliation.tblTransactionPostingRequest.BankShortCode
        INNER JOIN dbo.ViewBankBranchAccount ON dbo.ViewBankBranchAccount.BankAccountID = Reconciliation.tblTransactionPostingRequest.BankAccountID
                                                AND dbo.ViewBankBranchAccount.BankShortCode = Reconciliation.tblTransactionPostingRequest.BankShortCode
WHERE   IsApproved IS NULL
        OR ( IsApproved = 0
             AND ApprovedDate IS NULL
           );
", Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }

                Dt = ds.Tables[0];
            }

            Common.setComboList(cboBankName, Dt, "BankShortCode", "BankName");

            cboBankName.SelectedIndex = -1;


        }


    }
}


