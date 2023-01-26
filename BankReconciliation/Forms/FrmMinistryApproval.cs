using BankReconciliation.Class;
using DevExpress.Utils;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Selection;
using DevExpress.XtraSplashScreen;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using TaxSmartSuite.Class;

namespace BankReconciliation.Forms
{
    public partial class FrmMinistryApproval : Form
    {
        public static FrmMinistryApproval publicStreetGroup; private SqlCommand _command; private SqlDataAdapter adp; GridColumn colView2 = new GridColumn(); RepositoryItemGridLookUpEdit repComboLookBoxCredit = new RepositoryItemGridLookUpEdit(); GridColumn colView = new GridColumn();
        GridCheckMarksSelection selection; DataTable tableTrans = new DataTable(); DataSet dataSet = new DataSet();
        System.Data.DataSet dts; string strtoken;
        public FrmMinistryApproval()
        {
            SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

            InitializeComponent();

            publicStreetGroup = this;

            setImages();

            ToolStripEvent();

            Load += OnFormLoad;

            OnFormLoad(null, null);

            tableTrans.Columns.Add("pPeriod", typeof(string));

            tableTrans.Columns.Add("pNarriation", typeof(string));

            tableTrans.Columns.Add("pBankAccountID", typeof(int));

            tableTrans.Columns.Add("pSummaryID", typeof(int));

            tableTrans.Columns.Add("pFinancialperiodID", typeof(int));

            tableTrans.Columns.Add("pAgencyCode", typeof(string));

            tableTrans.Columns.Add("pRevenueCode", typeof(string));

            tableTrans.Columns.Add("pDESCRIPTION", typeof(string));

            tableTrans.Columns.Add("pAgencyName", typeof(string));

            tableTrans.Columns.Add("pAmount", typeof(decimal));


            SplashScreenManager.CloseForm(false);
        }

        public void RefreshForm()
        {
            NavBars.ToolStripEnableDisableControls(toolStrip, Tag as String);
        }

        private void setImages()
        {
            tsbNew.Image = MDIMains.publicMDIParent.i16x16.Images[5];
            tsbEdit.Image = MDIMains.publicMDIParent.i16x16.Images[6];
            tsbDelete.Image = MDIMains.publicMDIParent.i16x16.Images[7];
            tsbReload.Image = MDIMains.publicMDIParent.i16x16.Images[0];
            tsbClose.Image = MDIMains.publicMDIParent.i16x16.Images[11];
            btnToken.Image = MDIMains.publicMDIParent.i32x32.Images[7];

            //bttnCancel.Image = MDIMains.publicMDIParent.i32x32.Images[9];
            ////bttnReset.Image = MDIMains.publicMDIParent.i32x32.Images[8];
            ////bttnUpdate.Image = MDIMain.publicMDIParent.i32x32.Images[7];
            //bttnUpdate.Image = MDIMains.publicMDIParent.i32x32.Images[7];
            //btnAllocate.Image = MDIMains.publicMDIParent.i32x32.Images[6];
            //bttncompare.Image = MDIMains.publicMDIParent.i32x32.Images[8];
            ////bttnClose.Image = MDIMain.publicMDIParent.i32x32.Images[9];
            //bttnSave.Image = MDIMains.publicMDIParent.i32x32.Images[7];
            sbnUpdate.Image = MDIMains.publicMDIParent.i32x32.Images[6];
            sbnDisapprove.Image = MDIMains.publicMDIParent.i32x32.Images[6];

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
            setReload(); selection = new GridCheckMarksSelection(gridView1);

            sbnUpdate.Click += SbnUpdate_Click;

            btnToken.Click += BtnToken_Click;

            sbnDisapprove.Click += SbnDisapprove_Click;
        }

        private void SbnDisapprove_Click(object sender, EventArgs e)
        {
            string value = string.Empty;

            if (DialogResults.InputBox(@"Comments for Disapproving ", "Ministry Disapproval", ref value) == DialogResult.OK)
            {

                if (!string.IsNullOrWhiteSpace(value))
                {
                    for (int i = 0; i < selection.SelectedCount; i++)
                    {

                        tableTrans.Rows.Add(new object[] { (selection.GetSelectedRow(i) as DataRowView)["Period"], (selection.GetSelectedRow(i) as DataRowView)["Narriation"], Convert.ToInt32((selection.GetSelectedRow(i) as DataRowView)["BankAccountID"]) , Convert.ToInt32((selection.GetSelectedRow(i) as DataRowView)["SummaryID"]), Convert.ToInt32((selection.GetSelectedRow(i) as DataRowView)["FinancialperiodID"]), (selection.GetSelectedRow(i) as DataRowView)["AgencyCode"],(selection.GetSelectedRow(i) as DataRowView)["RevenueCode"],
        (selection.GetSelectedRow(i) as DataRowView)["Description"],(selection.GetSelectedRow(i) as DataRowView)["AgencyName"],Convert.ToDecimal((selection.GetSelectedRow(i) as DataRowView)["Amount"]) });

                    }

                    using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
                    {
                        connect.Open();
                        _command = new SqlCommand("Reconciliation.DisapprovalMinistrySummary", connect)
                        {
                            CommandType = CommandType.StoredProcedure
                        };

                        _command.Parameters.Add(new SqlParameter("@pTransactiondb", SqlDbType.Structured)).Value = tableTrans;
                        _command.Parameters.Add(new SqlParameter("@Userid", SqlDbType.VarChar)).Value = Program.UserID;
                        _command.Parameters.Add(new SqlParameter("@comment", SqlDbType.VarChar)).Value = value;
                        _command.CommandTimeout = 0;

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

                                //FrmReportPosting report = new FrmReportPosting(ds.Tables[1], ds.Tables[2]);
                                //using (FrmReportPosting frmreport = new FrmReportPosting(ds))
                            }


                        }
                    }
                }
                else
                {
                    Common.setMessageBox("Disapproval Comment is Empty", "Ministry Disapproval", 3);

                    return;
                }
                setReload();
            }
        }

        private void BtnToken_Click(object sender, EventArgs e)
        {
            if (dotoken())
            {
                sbnUpdate.Enabled = true;
            }
            else
            {
                sbnUpdate.Enabled = false; BtnToken_Click(null, null);
            }
        }

        private void SbnUpdate_Click(object sender, EventArgs e)
        {
            dowork();
        }

        private void setReload()
        {
            try
            {
                using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
                {
                    connect.Open();
                    _command = new SqlCommand("Reconciliation.LoadMinistrySummary", connect) { CommandType = CommandType.StoredProcedure };
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

                            gridControl1.DataSource = ds.Tables[1];

                            gridView1.OptionsBehavior.Editable = false;

                            gridView1.Columns["Amount"].DisplayFormat.FormatType = FormatType.Numeric;
                            gridView1.Columns["Amount"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                            gridView1.Columns["Amount"].SummaryItem.FieldName = "Amount";
                            gridView1.Columns["Amount"].SummaryItem.DisplayFormat = "Total = {0:n}";
                            gridView1.Columns["SummaryID"].Visible = false;
                            gridView1.Columns["AgencyCode"].Visible = false;
                            gridView1.Columns["RevenueCode"].Visible = false;
                            gridView1.Columns["BankAccountID"].Visible = false;
                            gridView1.Columns["FinancialperiodID"].Visible = false;

                            gridView1.OptionsView.ColumnAutoWidth = false;
                            gridView1.OptionsView.ShowFooter = true;

                            gridView1.BestFitColumns();
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                Tripous.Sys.ErrorBox(String.Format("{0}{1}", ex.Message, ex.StackTrace)); return;
            }
        }

        bool dotoken()
        {
            strtoken = Token.GenerateToken();

            if (tokenInsertValidation(Program.UserID, Program.ApplicationCode, strtoken, false))
            {
                try
                {
                    var procesSms = new ProcessSms.ProcessSms();

                    string strprocessSme = procesSms.SendSms(Program.Userphone, "Token", strtoken);

                    if (strprocessSme.Contains("Failed"))
                    {
                        Tripous.Sys.ErrorBox(strprocessSme.ToString());

                        Common.setMessageBox(strprocessSme.ToString(), "Get Token", 1);

                        return false;
                    }
                    else
                    {
                        Common.setMessageBox(string.Format("Token Request sent to your registered number {0}.", $"********{Program.Userphone.Substring(7)}"), "Token Request", 1);

                        //dt = DateTime.Now;

                        return true;
                    }

                }
                catch (Exception ex)
                {
                    Tripous.Sys.ErrorBox(ex.Message);

                    return false;
                }


            }
            else
                return false;
        }

        bool validatetoken(string valtoken)
        {
            bool respones;

            if (tokenInsertValidation(Program.UserID, Program.ApplicationCode, valtoken, true))
            {
                respones = true;
            }
            else
            {
                respones = false;
            }

            return respones;
        }
        bool tokenInsertValidation(string userid, string ApplicationCode, string strtoken, bool status)
        {
            using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
            {
                connect.Open();

                _command = new SqlCommand("Reconciliation.tokenInsertValidation", connect) { CommandType = CommandType.StoredProcedure };
                _command.Parameters.Add(new SqlParameter("@userid", SqlDbType.VarChar)).Value = userid;
                _command.Parameters.Add(new SqlParameter("@usertoken", SqlDbType.VarChar)).Value = strtoken;
                _command.Parameters.Add(new SqlParameter("@application", SqlDbType.VarChar)).Value = ApplicationCode;
                _command.Parameters.Add(new SqlParameter("@validDatetime", SqlDbType.DateTime)).Value = DateTime.Now;
                _command.Parameters.Add(new SqlParameter("@isValid", SqlDbType.Bit)).Value = status;

                using (System.Data.DataSet ds = new System.Data.DataSet())
                {
                    ds.Clear();
                    adp = new SqlDataAdapter(_command);
                    adp.Fill(ds);
                    connect.Close();

                    if (ds.Tables[0].Rows[0]["returnCode"].ToString() != "00")
                    {
                        Common.setMessageBox(ds.Tables[0].Rows[0]["returnMessage"].ToString(), Program.ApplicationName, 2);

                        return false;

                    }
                    else
                    {
                        return true;
                    }

                }
            }
        }

        void dowork()
        {
            string value = string.Empty;

            strtoken = string.Empty;


            Token.dotoken();

            if (DialogResults.InputBox(@"OTP", string.Format("Kindly enter the token to Authorize this transaction.", $"********{Program.Userphone.Substring(7)}"), ref value) == DialogResult.OK)
            {
                if (Token.tokenInsertValidation(Program.UserID, Program.ApplicationCode, value.ToString(), true, string.Format("{0}", this.groupControl1.Text.Trim())))
                //if (validatetoken(value.ToString()))
                {
                    Processwork();
                    sbnUpdate.Enabled = true;
                }
                else
                {
                    ////false
                    ////dowork();
                    sbnUpdate.Enabled = true;
                    //BtnToken_Click(null, null);

                }
            }
            //}
            //else
            //    dowork();
        }

        void Processwork()
        {
            if (selection.SelectedCount == 0)
            {
                Common.setMessageBox("No Selection made", Program.ApplicationName, 3);
                return;

            }
            else
            {
                for (int i = 0; i < selection.SelectedCount; i++)
                {

                    tableTrans.Rows.Add(new object[] { (selection.GetSelectedRow(i) as DataRowView)["Period"], (selection.GetSelectedRow(i) as DataRowView)["Narriation"], Convert.ToInt32((selection.GetSelectedRow(i) as DataRowView)["BankAccountID"]) , Convert.ToInt32((selection.GetSelectedRow(i) as DataRowView)["SummaryID"]), Convert.ToInt32((selection.GetSelectedRow(i) as DataRowView)["FinancialperiodID"]), (selection.GetSelectedRow(i) as DataRowView)["AgencyCode"],(selection.GetSelectedRow(i) as DataRowView)["RevenueCode"],
        (selection.GetSelectedRow(i) as DataRowView)["Description"],(selection.GetSelectedRow(i) as DataRowView)["AgencyName"],Convert.ToDecimal((selection.GetSelectedRow(i) as DataRowView)["Amount"]) });

                }

                using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
                {
                    connect.Open();
                    _command = new SqlCommand("doSummaryAGPosting", connect)
                    {
                        CommandType = CommandType.StoredProcedure
                    };

                    _command.Parameters.Add(new SqlParameter("@pTransactiondb", SqlDbType.Structured)).Value = tableTrans;
                    _command.Parameters.Add(new SqlParameter("@Userid", SqlDbType.VarChar)).Value = Program.UserID;
                    _command.CommandTimeout = 0;

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

                            //FrmReportPosting report = new FrmReportPosting(ds.Tables[1], ds.Tables[2]);
                            using (FrmReportPosting frmreport = new FrmReportPosting(ds))
                            {
                                frmreport.ShowDialog();
                            }
                            //FrmRequest_Load(null, null);

                            setReload();

                        }

                    }
                }


            }

        }
    }
}
