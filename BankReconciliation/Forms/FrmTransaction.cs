using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraSplashScreen;
using BankReconciliation.Class;
using System.Data.SqlClient;
using TaxSmartSuite.Class;
using System.Data.OleDb;
using DevExpress.Utils;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Selection;
using DevExpress.XtraGrid.Views.Grid;
using BankReconciliation.Report;
using DevExpress.XtraReports.UI;
using DevExpress.XtraReports.Parameters;
/* To work eith EPPlus library */
using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using System.IO;
using DevExpress.XtraEditors.Controls;
using System.Globalization;


namespace BankReconciliation.Forms
{
    public partial class FrmTransaction : Form
    {
        private SqlCommand _command; private SqlDataAdapter adp; private string BatchNumber; private SqlCommand _command1; private SqlCommand _command2;
        private DataTable dbcredit;
        private DataTable dbDebit;
        private DataTable dbmatched = null;
        DataTable dbCrDebit = null;
        private DataTable dbmissing;
        private DataTable dbmissingpay;
        private DataTable dtc;
        private DataTable dtde = null;
        private string ElemDescr;
        private string ElemType;
        private bool Isbank = false;
        private bool isFirstGrid = true;
        private bool isFirstGrid2 = true;
        private bool isRecord = false;
        private bool isRecord2 = false; bool IsResetBankStmt = false;
        bool isComplete = false;
        public static FrmTransaction publicStreetGroup;

        private System.Data.OleDb.OleDbDataAdapter MyCommand;

        private OleDbConnection MyConnection;

        string filenamesopen, acctnumber; bool boolIsUpdate2; bool isClosed;

        public static DataTable dtMatchedManual;

        DataTable Dt;
        DataTable Dts;
        System.Data.DataSet DtSet;
        DataTable dtcollnot = new DataTable(); DataTable dtBanNotMatch = new DataTable();
        GridColumn colView = new GridColumn(); GridColumn colView2 = new GridColumn();
        GridColumn colRevenue = new GridColumn();
        RepositoryItemComboBox repCombobox = new RepositoryItemComboBox();
        //RepositoryItemGridLookUpEdit repComboboxRevenue = new RepositoryItemGridLookUpEdit();
        RepositoryItemSearchLookUpEdit repComboboxRevenue = new RepositoryItemSearchLookUpEdit();
        RepositoryItemGridLookUpEdit repComboLookBox = new RepositoryItemGridLookUpEdit();
        RepositoryItemGridLookUpEdit repComboLookBoxCredit = new RepositoryItemGridLookUpEdit();
        GridCheckMarksSelection selection; GridCheckMarksSelection selection2;
        SqlTransaction transaction;

        private string strAcct;
        private string strBranch;
        private string strpayerID;
        private string strpaymentRef; string selectedPages;
        bool isExcelAltered = false;

        public FrmTransaction()
        {
            SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);


            InitializeComponent();

            publicStreetGroup = this;

            setImages();

            ToolStripEvent();

            Load += OnFormLoad;

            OnFormLoad(null, null);

            openForm();

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
            btnAllocate.Image = MDIMains.publicMDIParent.i32x32.Images[6];
            bttncompare.Image = MDIMains.publicMDIParent.i32x32.Images[8];
            //bttnClose.Image = MDIMain.publicMDIParent.i32x32.Images[9];
            bttnSave.Image = MDIMains.publicMDIParent.i32x32.Images[7];

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
                try
                {
                    MDIMains.publicMDIParent.RemoveControls();
                    //if (string.IsNullOrEmpty(label22.Text))
                    //{
                    //    MDIMains.publicMDIParent.RemoveControls();
                    //}
                    //else
                    //{
                    //    using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
                    //    {
                    //        connect.Open();

                    //        _command = new SqlCommand("doCheckBatch", connect) { CommandType = CommandType.StoredProcedure };
                    //        _command.Parameters.Add(new SqlParameter("@batch", SqlDbType.Int)).Value = label22.Text;

                    //        using (System.Data.DataSet ds = new System.Data.DataSet())
                    //        {
                    //            ds.Clear();
                    //            adp = new SqlDataAdapter(_command);
                    //            adp.Fill(ds);
                    //            //Dts = ds.Tables[0];
                    //            connect.Close();

                    //            //dtResult = ds;

                    //            if (ds.Tables[0].Rows[0]["returnCode"].ToString() == "00")
                    //            {
                    //                DialogResult result = MessageBox.Show(string.Format("There are no Banks Associated with this Batch ID {0} . Closing this means recreating it next time. ", label21.Text), Program.ApplicationName, MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

                    //                if (result == DialogResult.OK)
                    //                {
                    //                    //if (deletebatch())
                    //                    MDIMains.publicMDIParent.RemoveControls();
                    //                    //else return;
                    //                }
                    //                else
                    //                {
                    //                    return;
                    //                }

                    //            }
                    //            else
                    //                MDIMains.publicMDIParent.RemoveControls();
                    //        }
                    //    }
                    //}


                }
                catch (Exception ex)
                {
                    Tripous.Sys.ErrorBox(ex.StackTrace + ex.Message);
                    return;
                }
                finally
                {

                }

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
        //using (FrmViewAllocation frmreport = new FrmViewAllocation(dtde, dtc))
        //       {
        //           frmreport.ShowDialog();
        //       }
        void OnFormLoad(object sender, EventArgs e)
        {
            isClosed = false;

            isRecord2 = false;

            setDBComboBox(); setDBComboBoxPeriod();

            cboRecPeriod.SelectedIndexChanged += CboRecPeriod_SelectedIndexChanged;

            dtpStart.ValueChanged += dtpStart_ValueChanged;
            dtpEnd.ValueChanged += dtpEnd_ValueChanged;
            dtpStart.Focus();

            bttnImport.Click += bttnImport_Click;
            bttnUpdateExcel.Click += bttnUpdateExcel_Click;

            btnEdit.Click += btnEdit_Click; btnAddNew.Click += btnAddNew_Click;

            bttnBackAllocate.Click += bttnBackAllocate_Click;
            bttnBackPosting.Click += bttnBackPosting_Click;
            btnAllocate.Click += btnAllocate_Click;
            radioGroup3.SelectedIndexChanged += radioGroup3_SelectedIndexChanged;
            bttncompare.Click += bttncompare_Click;
            bttnMatch.Click += bttnMatch_Click;
            cboBank.KeyPress += cboBank_KeyPress;
            cboBank.SelectedIndexChanged += cboBank_SelectedIndexChanged;
            bttnSave.Click += bttnSave_Click;
            xtraTabControl1.SelectedPageChanged += xtraTabControl1_SelectedPageChanged;
            gridView10.DoubleClick += gridView10_DoubleClick;

            cboRevenuetype.KeyPress += cboRevenuetype_KeyPress;
            cboRevenuetype.SelectedIndexChanged += cboRevenuetype_SelectedIndexChanged;

            cboRevenuetype_SelectedIndexChanged(null, null);
            bttnPostingrec.Click += bttnPostingrec_Click;

            selectedPages = "0002";
            //cboAcct.SelectedIndexChanged += cboAcct_SelectedIndexChanged;
            //cboAcct_SelectedIndexChanged(null, null);

            cboAccount.SelectedIndexChanged += cboAccount_SelectedIndexChanged;

            linkLabel1.Click += LinksClicked;
            bttnClose.Click += bttnClose_Click;
            bttnreset.Click += bttnreset_Click;
            bttnPreview.Click += bttnPreview_Click;

            bttnReimport.Click += bttnReimport_Click;

            bttnMatched.Click += BttnMatched_Click;

            radioGroup2.SelectedIndexChanged += radioGroup2_SelectedIndexChanged;
            //chkUncleared.Click += chkUncleared_Click;
            checkBox1.Click += checkBox1_Click;
            txtClosing.LostFocus += txtClosing_LostFocus;
            txtOpening.LostFocus += txtOpening_LostFocus;

            bttnCancel.Click += bttnCancel_Click;
            bttnNextImp.Click += bttnNextImp_Click;
            bttncancelbs.Click += bttncancelbs_Click;
            bttnNext.Click += bttnNext_Click;
            bttnNextAllocate.Click += bttnNextAllocate_Click;
            bttnBack.Click += bttnBack_Click;
            bttnclosedper.Click += bttnclosedper_Click;

            btnPreviewA.Click += BtnPreviewA_Click;

            bttnSave.Enabled = false;

        }

        private void BttnMatched_Click(object sender, EventArgs e)
        {
            //using (FrmMatched frmreport = new FrmMatched(dtBanNotMatch, dtcollnot))
            //{
            //    frmreport.ShowDialog();
            //}

            //var listres = (from DataRow row in ds.Tables[1].Rows
            //               select new SetsData.TrendDA
            //               {
            //                   Income = Convert.ToDecimal(row["Income"]),
            //                   OfficeName = row["OfficeName"] as string,
            //                   RevenueOfficeID = Convert.ToInt32(row["RevenueOfficeID"]),
            //                   Tax = Convert.ToDecimal(row["Tax"]),
            //                   UTIN = row["UTIN"] as string,
            //                   PayerName = row["PayerName"] as string,
            //                   YEARs = row["YEARs"] as string
            //               }
            //                       ).ToList();


            var listest = (from DataRow row in dtBanNotMatch.Rows
                           select new Dataset.NtmBank
                           {
                               Amount = Convert.ToDecimal(row["Amount"]),
                               BalDate = Convert.ToDateTime(row["Date"].ToString()),
                               BSid = Convert.ToInt32(row["Bsid"])
                           }).ToList();

            var listTest2 = (from DataRow rows in dtcollnot.Rows
                             select new Dataset.NTCollection
                             {
                                 Amount = Convert.ToDecimal(rows["Amount"]),
                                 PaymentRef = rows["PaymentRef"] as string,
                                 CollDate = Convert.ToDateTime(rows["Date"].ToString())
                             }).ToList();

            DataTable dt = ConvertToDataTable(listest);

            DataTable dts2 = ConvertToDataTable(listTest2);

            using (Form1 usefg = new Form1(dt, dts2))
            {
                usefg.ShowDialog();
            }

            if (dtMatchedManual != null && dtMatchedManual.Rows.Count > 0)
            {
                reCalMatched();
            }
        }

        private void CboRecPeriod_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataRowView oDataRowView = cboRecPeriod.SelectedItem as DataRowView;

            if (oDataRowView != null)
            {
                cboBank.SelectedValue = oDataRowView.Row["BankShortCode"] as string;

                label22.Text = string.Format("{0}", oDataRowView.Row["FinancialperiodID"]);

                dtpStart.Value = Convert.ToDateTime(oDataRowView.Row["StartDate"]);

                dtpEnd.Value = Convert.ToDateTime(oDataRowView.Row["EndDate"]);

                label25.Text = string.Format("{0}", oDataRowView.Row["PeriodID"]);

                label21.Text = oDataRowView.Row["Period"] as string;

                cboAccount.SelectedValue = Convert.ToInt32(oDataRowView.Row["BankAccountID"]);

                cboAcct.Enabled = false; cboBank.Enabled = false; cboAccount.Enabled = false; dtpEnd.Enabled = false;
            }
        }

        private void BtnPreviewA_Click(object sender, EventArgs e)
        {
            if (!boolIsUpdate2)
            {

                using (FrmViewAllocation frmreport = new FrmViewAllocation(dtde, dtc))
                {
                    frmreport.ShowDialog();
                }
                //FrmRequest_Load(null, null);
            }
            else
            {
                //dbcredit //credit transaction
                //dbDebit //debit transaction

                using (FrmViewAllocation frmreport = new FrmViewAllocation(dbDebit, dbcredit))
                {
                    frmreport.ShowDialog();
                }
                //FrmRequest_Load(null, null);
            }
        }

        void btnAddNew_Click(object sender, EventArgs e)
        {
            gridView1.OptionsBehavior.Editable = true;

            GridView view = gridView1;

            //The row will provide group column values for a new row 
            int rowHandle = view.GetDataRowHandleByGroupRowHandle(view.FocusedRowHandle);

            //Store group column values 
            object[] groupValues = null;
            int groupColumnCount = view.GroupedColumns.Count;
            if (groupColumnCount > 0)
            {
                groupValues = new object[groupColumnCount];
                for (int i = 0; i < groupColumnCount; i++)
                {
                    groupValues[i] = view.GetRowCellValue(rowHandle, view.GroupedColumns[i]);
                }
            }

            //Add a new row 
            view.AddNewRow();
            //Get the handle of the new row 
            int newRowHandle = view.FocusedRowHandle;
            object newRow = view.GetRow(newRowHandle);
            //Set cell values corresponding to group columns 
            if (groupColumnCount > 0)
            {
                for (int i = 0; i < groupColumnCount; i++)
                {
                    view.SetRowCellValue(newRowHandle, view.GroupedColumns[i], groupValues[i]);
                }
            }
            //Accept the new row 
            //The row moves to a new position according to the current group settings 
            view.UpdateCurrentRow();
            //Locate the new row 
            for (int n = 0; n < view.DataRowCount; n++)
            {
                if (view.GetRow(n).Equals(newRow))
                {
                    view.FocusedRowHandle = n;
                    break;
                }
            }

        }

        void btnEdit_Click(object sender, EventArgs e)
        {
            gridView1.OptionsBehavior.Editable = true;

            gridView1.Columns["DEBIT"].OptionsColumn.AllowEdit = true;
            gridView1.Columns["CREDIT"].OptionsColumn.AllowEdit = true;
            gridView1.Columns["BALANCE"].OptionsColumn.AllowEdit = false;
            gridView1.Columns["DATE"].OptionsColumn.AllowEdit = false;
            gridView1.Columns["REVENUECODE"].OptionsColumn.AllowEdit = true;
            gridView1.Columns["PAYERNAME"].OptionsColumn.AllowEdit = true;

        }

        void cboAccount_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboAccount.SelectedValue != null && !isRecord)
            {
                getopenBal();
            }
        }

        void bttnReimport_Click(object sender, EventArgs e)
        {
            string strquestion = string.Format("Do you want to Re-import {0},{1} Account Transaction between {2} and {3}", cboBank.Text, cboAccount.Text, string.Format("{0:dd/MM/yyyy}", dtpStart.Value), string.Format("{0:dd/MM/yyyy}", dtpEnd.Value));

            DialogResult result = MessageBox.Show(strquestion, "Import Statement", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)//excele 2003
            {
                isExcelAltered = IsResetBankStmt = true;
                bttnImport_Click(null, null);
            }

        }

        void ResetBankStatement()
        {
            using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
            {
                connect.Open();

                _command = new SqlCommand("doRestBankStatementImport", connect) { CommandType = CommandType.StoredProcedure };
                _command.Parameters.Add(new SqlParameter("@bankcode", SqlDbType.VarChar)).Value = cboBank.SelectedValue;
                _command.Parameters.Add(new SqlParameter("@startdate", SqlDbType.VarChar)).Value = string.Format("{0:yyyy/MM/dd 00:00:00}", dtpStart.Value);
                _command.Parameters.Add(new SqlParameter("@enddate", SqlDbType.VarChar)).Value = string.Format("{0:yyyy/MM/dd 23:59:59}", dtpEnd.Value);
                _command.Parameters.Add(new SqlParameter("@Periods", SqlDbType.Int)).Value = label22.Text.Trim();
                _command.Parameters.Add(new SqlParameter("@AccountId", SqlDbType.Int)).Value = cboAccount.SelectedValue;
                //_command.Parameters.Add(new SqlParameter("@UserId", SqlDbType.Char)).Value = Program.UserID;
                _command.CommandTimeout = 0;
                using (System.Data.DataSet ds = new System.Data.DataSet())
                {
                    ds.Clear();
                    adp = new SqlDataAdapter(_command);
                    adp.Fill(ds);
                    //Dts = ds.Tables[0];
                    connect.Close();

                    //dtResult = ds;

                    if (ds.Tables[0].Rows[0]["returnCode"].ToString() != "00")
                    {
                        Common.setMessageBox(ds.Tables[0].Rows[0]["returnMessage"].ToString(), "Reset Transaction Error", 1); return;
                    }

                }
            }
        }

        void bttnBackPosting_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Going Back will Cancel Transaction to be Post! Should I Continue ? ", "Transaction Posting", MessageBoxButtons.YesNo);

            if (result == DialogResult.Yes)
            {
                try
                {
                    SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);


                    xtraTabPage4.PageEnabled = true;
                    xtraTabPage1.PageEnabled = false; xtraTabPage3.PageEnabled = false; xtraTabPage2.PageEnabled = false;
                    xtraTabControl1.SelectedTabPage = xtraTabPage4;
                    return;


                }
                finally
                {
                    SplashScreenManager.CloseForm(false);
                }
            }
            else
            {
                return;
            }
        }

        void bttnBackAllocate_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Going Back will Cancel Allocate Transaction! Should I Continue ? ", "Allocate Transction", MessageBoxButtons.YesNo);

            if (result == DialogResult.Yes)
            {
                try
                {
                    SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

                    using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
                    {
                        connect.Open();

                        _command = new SqlCommand("ResetReconcilaition", connect) { CommandType = CommandType.StoredProcedure };
                        _command.Parameters.Add(new SqlParameter("@bankcode", SqlDbType.VarChar)).Value = cboBank.SelectedValue;
                        _command.Parameters.Add(new SqlParameter("@startdate", SqlDbType.VarChar)).Value = string.Format("{0:yyyy/MM/dd 00:00:00}", dtpStart.Value);
                        _command.Parameters.Add(new SqlParameter("@enddate", SqlDbType.VarChar)).Value = string.Format("{0:yyyy/MM/dd 23:59:59}", dtpEnd.Value);
                        _command.Parameters.Add(new SqlParameter("@BatchID", SqlDbType.Char)).Value = label22.Text.Trim();
                        _command.Parameters.Add(new SqlParameter("@Type", SqlDbType.Char)).Value = "Allocate";
                        _command.Parameters.Add(new SqlParameter("@UserId", SqlDbType.Char)).Value = Program.UserID;
                        _command.CommandTimeout = 0;
                        using (System.Data.DataSet ds = new System.Data.DataSet())
                        {
                            ds.Clear();
                            adp = new SqlDataAdapter(_command);
                            adp.Fill(ds);
                            //Dts = ds.Tables[0];
                            connect.Close();

                            //dtResult = ds;

                            if (ds.Tables[0].Rows[0]["returnCode"].ToString() == "00")
                            {
                                xtraTabPage3.PageEnabled = true;
                                xtraTabPage1.PageEnabled = false;
                                xtraTabControl1.SelectedTabPage = xtraTabPage3;
                                return;
                            }
                            else
                            {
                                //ds.Tables[1].Rows[0]["Acctnumber"].ToString()
                                Common.setMessageBox(ds.Tables[0].Rows[0]["returnMessage"].ToString(), "Reset Transaction Error", 1); return;
                            }
                        }
                    }


                }
                catch (Exception ex)
                {
                    Tripous.Sys.ErrorBox(ex.StackTrace + ex.Message);
                    return;
                }
                finally
                {
                    SplashScreenManager.CloseForm(false);
                }
            }
            else
            {
                return;
            }
        }

        void bttnclosedper_Click(object sender, EventArgs e)
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

                #region

                //string query = String.Format("SELECT COUNT(Amount) AS Count FROM tblPeriods WHERE BankCode='{0}' and CONVERT(VARCHAR(10),[Start Date],103)='{1}' and CONVERT(VARCHAR(10),[End Date],103)='{2}' AND IsClosed=1", (string)cboBank.SelectedValue, string.Format("{0:dd/MM/yyy}", dtpStart.Value), string.Format("{0:dd/MM/yyy}", dtpEnd.Value));

                //if (new Logic().IsRecordExist(query))
                //{
                //    Common.setMessageBox("Period Already Close", Program.ApplicationName, 2);
                //}
                //else
                //{
                ////commit close period into tbale per period and account
                //if (label6.ForeColor != Color.Green)
                //{
                //    Common.setMessageBox("Closing Balance not correct!", "Closed Period", 3); return;
                //}
                //    else
                //    {
                //        using (SqlConnection db = new SqlConnection(Logic.ConnectionString))
                //        {
                //            SqlTransaction transaction;

                //            db.Open();
                //            transaction = db.BeginTransaction();
                //            try
                //            {

                //                using (SqlCommand sqlCommand = new SqlCommand(String.Format("INSERT INTO tblPeriods( [Start Date] ,[End Date] ,BankCode , Amount , ReconciliationID , IsClosed,CloseBal,BatchID ) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}','{7}');", string.Format("{0:yyyy/MM/dd}", dtpStart.Value), string.Format("{0:yyyy/MM/dd}", dtpEnd.Value), cboBank.SelectedValue, Convert.ToDouble(label14.Text), string.Format("{0}{1}{2}{3}{4}", Program.UserID, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second, DateTime.Now.Millisecond), 1, Convert.ToDouble(label6.Text), Convert.ToInt32(label21.Text)), db, transaction))
                //                {
                //                    sqlCommand.ExecuteNonQuery();
                //                }


                //                transaction.Commit();

                //            }
                //            catch (SqlException sqlError)
                //            {
                //                transaction.Rollback();
                //                Tripous.Sys.ErrorBox(sqlError);
                //            }
                //            db.Close();
                //        }

                //        Common.setMessageBox("Transaction Period Closed", Program.ApplicationName, 1); return;

                //        DialogResult results = MessageBox.Show("Process Another Bank Account ?", "Bank Statement / PayDirect", MessageBoxButtons.YesNo);

                //        if (results == DialogResult.Yes)
                //        {
                //            ClearForm();
                //        }
                //        else
                //        {
                //            return;
                //        }
                //    }

                //}
                #endregion

                //commit close period into tbale per period and account
                if (label6.ForeColor != Color.Green)
                {
                    Common.setMessageBox("Closing Balance not correct!", "Closed Period", 3); return;
                }
                else
                {
                    using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
                    {
                        connect.Open();

                        _command = new SqlCommand("ClosePeriod", connect) { CommandType = CommandType.StoredProcedure };
                        _command.Parameters.Add(new SqlParameter("@bankcode", SqlDbType.VarChar)).Value = cboBank.SelectedValue;
                        _command.Parameters.Add(new SqlParameter("@startdate", SqlDbType.VarChar)).Value = string.Format("{0:yyyy/MM/dd 00:00:00}", dtpStart.Value);
                        _command.Parameters.Add(new SqlParameter("@enddate", SqlDbType.VarChar)).Value = string.Format("{0:yyyy/MM/dd 23:59:59}", dtpEnd.Value);
                        _command.Parameters.Add(new SqlParameter("@openAmount", SqlDbType.Decimal)).Value = Convert.ToDecimal(label14.Text);
                        _command.Parameters.Add(new SqlParameter("@CloseBal", SqlDbType.Decimal)).Value = Convert.ToDecimal(label6.Text);
                        _command.Parameters.Add(new SqlParameter("@BatchID", SqlDbType.Int)).Value = Convert.ToInt32(label21.Text);
                        _command.Parameters.Add(new SqlParameter("@userid", SqlDbType.VarChar)).Value = Program.UserID;

                        using (System.Data.DataSet ds = new System.Data.DataSet())
                        {
                            ds.Clear();
                            adp = new SqlDataAdapter(_command);
                            adp.Fill(ds);
                            //Dts = ds.Tables[0];
                            connect.Close();

                            //dtResult = ds;

                            if (ds.Tables[0].Rows[0]["returnCode"].ToString() == "00")
                            {
                                //string.Format(@"Period Closed for Batch ID {0} Bank Code {1} Date from {2}  Date to {3}", label21.Text, cboBank.SelectedValue, string.Format("{0:dd/MM/yyyy}", dtpStart.Value), string.Format("{0:dd/MM/yyyy}", dtpEnd.Value));
                                Common.setMessageBox(string.Format(@"Period Closed for Batch ID {0} Bank Code {1} Date from {2}  Date to {3}", label21.Text, cboBank.SelectedValue, string.Format("{0:dd/MM/yyyy}", dtpStart.Value), string.Format("{0:dd/MM/yyyy}", dtpEnd.Value)), Program.ApplicationName, 2);

                            }
                            else
                            {
                                Common.setMessageBox(ds.Tables[0].Rows[0]["returnMessage"].ToString(), Program.ApplicationName, 2);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Tripous.Sys.ErrorBox(ex.StackTrace + ex.Message);
                return;
            }
            finally
            {
                SplashScreenManager.CloseForm(false);
            }
        }

        void bttnBack_Click(object sender, EventArgs e)
        {
            xtraTabPage2.PageEnabled = false;
            xtraTabPage1.PageEnabled = true;
            xtraTabPage3.PageEnabled = false;
            xtraTabControl1.SelectedTabPage = xtraTabPage1;
            xtraTabControl1.Enabled = true;
            //bttncancelbs_Click(null, null);
            label6.Text = string.Empty;
        }

        void bttnNextAllocate_Click(object sender, EventArgs e)
        {
            if (!SaveBankAllocation()) return;
            xtraTabPage4.PageEnabled = true;
            xtraTabPage1.PageEnabled = false; xtraTabPage3.PageEnabled = false; xtraTabPage2.PageEnabled = false;
            xtraTabControl1.SelectedTabPage = xtraTabPage4;
        }

        void bttnNext_Click(object sender, EventArgs e)
        {
            if (!SaveCompareResultToDatabase()) return;
            xtraTabPage2.PageEnabled = true;
            xtraTabPage1.PageEnabled = false; xtraTabPage3.PageEnabled = false;
            xtraTabControl1.SelectedTabPage = xtraTabPage2;
        }

        void bttncancelbs_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you Sure to Cancel Bank Statement / PayDirect process", "Bank Statement / PayDirect", MessageBoxButtons.YesNo);

            if (result == DialogResult.Yes)
            {
                try
                {
                    SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

                    //using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
                    //{
                    //    connect.Open();

                    //    _command = new SqlCommand("ResetReconcilaition", connect) { CommandType = CommandType.StoredProcedure };
                    //    _command.Parameters.Add(new SqlParameter("@bankcode", SqlDbType.VarChar)).Value = cboBank.SelectedValue;
                    //    _command.Parameters.Add(new SqlParameter("@startdate", SqlDbType.VarChar)).Value = string.Format("{0:yyyy/MM/dd 00:00:00}", dtpStart.Value);
                    //    _command.Parameters.Add(new SqlParameter("@enddate", SqlDbType.VarChar)).Value = string.Format("{0:yyyy/MM/dd 23:59:59}", dtpEnd.Value);
                    //    _command.Parameters.Add(new SqlParameter("@BatchID", SqlDbType.Char)).Value = label22.Text.Trim();
                    //    _command.Parameters.Add(new SqlParameter("@Type", SqlDbType.Char)).Value = "Compare";
                    //    _command.Parameters.Add(new SqlParameter("@UserId", SqlDbType.Char)).Value = Program.UserID;
                    //    _command.CommandTimeout = 0;
                    //    using (System.Data.DataSet ds = new System.Data.DataSet())
                    //    {
                    //        ds.Clear();
                    //        dbmatched = new DataTable(); dbmissing = new DataTable();
                    //        dbmissingpay = new DataTable();
                    //        dbCrDebit = new DataTable(); dbCrDebit.Clear();
                    //        dbmatched.Clear(); dbmissing.Clear(); dbmissingpay.Clear();

                    //        adp = new SqlDataAdapter(_command);
                    //        adp.Fill(ds);
                    //        //Dts = ds.Tables[0];
                    //        connect.Close();

                    //        //dtResult = ds;

                    //        if (ds.Tables[0].Rows[0]["returnCode"].ToString() == "00")
                    //        {
                    //            return;
                    //        }
                    //        else
                    //        {
                    //            //ds.Tables[1].Rows[0]["Acctnumber"].ToString()
                    //            Common.setMessageBox(ds.Tables[0].Rows[0]["returnMessage"].ToString(), "Reset Transaction Error", 1); return;
                    //        }
                    //    }
                    //}


                }
                catch (Exception ex)
                {
                    Tripous.Sys.ErrorBox(ex.StackTrace + ex.Message);
                    return;
                }
                finally
                {
                    SplashScreenManager.CloseForm(false);
                }
            }
            else
            {
                return;
            }
        }

        void bttnNextImp_Click(object sender, EventArgs e)
        {
            //xtraTabControl1.TabPages = xtraTabPage3;
            if (isExcelAltered)
                if (!SaveExcel()) return;
            dtpEnd.Enabled = false; dtpStart.Enabled = false; cboBank.Enabled = false;
            xtraTabPage3.PageEnabled = true;
            xtraTabPage1.PageEnabled = false;
            xtraTabControl1.SelectedTabPage = xtraTabPage3;
        }

        void bttnCancel_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you Sure to Cancel Imported Statement", "Import Statement", MessageBoxButtons.YesNo);

            if (result == DialogResult.Yes)
            {
                try
                {
                    SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

                    string quer1y = string.Format("DELETE FROM Reconciliation.tblBankStatement WHERE BankShortCode='{0}' AND CONVERT(VARCHAR(10),StartDate,103) ='{1}' AND CONVERT(VARCHAR(10),EndDate,103)='{2}' AND FinancialperiodID={3} AND (BSID NOT IN(SELECT bsid FROM Reconciliation.tblBankStatementAllocation)) AND (BSID NOT IN(SELECT bsid FROM Reconciliation.tblBankStatementAllocation))", cboBank.SelectedValue, string.Format("{0:dd/MM/yyy}", dtpStart.Value), string.Format("{0:dd/MM/yyy}", dtpEnd.Value), label22.Text.Trim());

                    deleteRecord(quer1y);

                    gridControl1.DataSource = null; gridView1.Columns.Clear();
                }
                catch (Exception ex)
                {
                    Tripous.Sys.ErrorBox(ex.StackTrace + ex.Message);
                    return;
                }
                finally
                {
                    SplashScreenManager.CloseForm(false);
                }
            }
            else
            {
                return;
            }

        }

        void txtOpening_LostFocus(object sender, EventArgs e)
        {
            String Text = ((TextBox)sender).Text.Replace(",", "");

            double Num;

            if (double.TryParse(Text, out Num))
            {
                Text = String.Format("{0:N2}", Num);
                ((TextBox)sender).Text = Text;
            }
        }

        void txtClosing_LostFocus(object sender, EventArgs e)
        {
            String Text = ((TextBox)sender).Text.Replace(",", "");

            double Num;

            if (double.TryParse(Text, out Num))
            {
                Text = String.Format("{0:N2}", Num);
                ((TextBox)sender).Text = Text;
            }


        }

        void checkBox1_Click(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                cboRevenuetype.Enabled = false; cboRevenuetype.SelectedIndex = -1; label55.Text = string.Empty; label7.Text = string.Empty;

            }
            else
            {
                cboRevenuetype.Enabled = true; cboRevenuetype.SelectedIndex = -1;
            }
        }

        void chkUncleared_Click(object sender, EventArgs e)
        {
            if (chkUncleared.Checked == true)
            {
                groupBox5.Enabled = false;
                groupBox18.Enabled = false;
            }
            else
            {
                groupBox5.Enabled = true;
                groupBox18.Enabled = true;
            }
        }

        void radioGroup2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (radioGroup2.EditValue.ToString() == "Yes")
            {
                groupBox18.Enabled = false;
            }
            if (radioGroup2.EditValue.ToString() == "No")
            {
                groupBox18.Enabled = true;
            }
        }

        void bttnPreview_Click(object sender, EventArgs e)
        {
            if (radioGroup1.EditValue == null)
            {
                Common.setEmptyField("Report Option", "Bank Statement / PayDirect"); return;
            }
            else
            {
                try
                {
                    SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

                    if (Convert.ToInt32(radioGroup1.EditValue) == 0)
                    {
                        using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
                        {
                            connect.Open();
                            _command = new SqlCommand("GetPaymentRecord", connect) { CommandType = CommandType.StoredProcedure };
                            _command.Parameters.Add(new SqlParameter("@bankcode", SqlDbType.VarChar)).Value = cboBank.SelectedValue;
                            _command.Parameters.Add(new SqlParameter("@startDate", SqlDbType.DateTime)).Value = string.Format("{0:yyyy/MM/dd}", dtpStart.Value);
                            _command.Parameters.Add(new SqlParameter("@endDate", SqlDbType.DateTime)).Value = string.Format("{0:yyyy/MM/dd}", dtpEnd.Value);
                            _command.Parameters.Add(new SqlParameter("@PType", SqlDbType.VarChar)).Value = "Not in Paydirect";
                            _command.Parameters.Add(new SqlParameter("@BatchID", SqlDbType.Int)).Value = Convert.ToInt32(label22.Text);


                            using (System.Data.DataSet ds = new System.Data.DataSet())
                            {
                                ds.Clear();
                                adp = new SqlDataAdapter(_command);
                                adp.Fill(ds, "table");
                                //Dts = ds.Tables[0];
                                connect.Close();

                                if (ds.Tables[1].Rows[0]["returnCode"].ToString() == "00")
                                {
                                    if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                                    {
                                        XtraRepBankPayDirect report = new XtraRepBankPayDirect();

                                        report.xrLabel10.Text = "List of Transactions in Bank Statment not in REEMS ";

                                        report.xrLabel12.Text = string.Format("between {0} and {1}", string.Format("{0:dd/MM/yyyy}", dtpStart.Value), string.Format("{0:dd/MM/yyyy}", dtpEnd.Value));

                                        report.DataSource = ds.Tables[0];

                                        report.DataMember = "table";

                                        report.ShowPreviewDialog();

                                    }
                                    else
                                    {
                                        Common.setMessageBox("No Record Found", Program.ApplicationName, 1); return;
                                    }
                                }
                                else
                                {
                                    Common.setMessageBox(ds.Tables[1].Rows[0]["returnMessage"].ToString(), Program.ApplicationName, 2);
                                }

                            }
                        }
                    }
                    else if (Convert.ToInt32(radioGroup1.EditValue) == 1)
                    {
                        using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
                        {
                            connect.Open();
                            _command = new SqlCommand("GetPaymentRecord", connect) { CommandType = CommandType.StoredProcedure };
                            _command.Parameters.Add(new SqlParameter("@bankcode", SqlDbType.VarChar)).Value = cboBank.SelectedValue;
                            _command.Parameters.Add(new SqlParameter("@startDate", SqlDbType.DateTime)).Value = string.Format("{0:yyyy/MM/dd}", dtpStart.Value);
                            _command.Parameters.Add(new SqlParameter("@endDate", SqlDbType.DateTime)).Value = string.Format("{0:yyyy/MM/dd}", dtpEnd.Value);
                            _command.Parameters.Add(new SqlParameter("@PType", SqlDbType.VarChar)).Value = "In Bank";
                            _command.Parameters.Add(new SqlParameter("@BatchID", SqlDbType.Int)).Value = Convert.ToInt32(label22.Text);

                            using (System.Data.DataSet ds = new System.Data.DataSet())
                            {
                                ds.Clear();
                                adp = new SqlDataAdapter(_command);
                                adp.Fill(ds, "table");
                                //Dts = ds.Tables[0];
                                connect.Close();

                                if (ds.Tables[1].Rows[0]["returnCode"].ToString() == "00")
                                {
                                    //label6.Text = String.Format("{0:N2}", ds.Tables[1].Rows[0]["ClosingBal"]);

                                    if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                                    {
                                        XtraRepBothBankPaydirect reports = new XtraRepBothBankPaydirect();

                                        reports.xrLabel11.Text = "List of Transactions in Both REEMS and Bank Statement ";

                                        reports.xrLabel12.Text = string.Format("between {0} and {1}", string.Format("{0:dd/MM/yyyy}", dtpStart.Value), string.Format("{0:dd/MM/yyyy}", dtpEnd.Value));

                                        reports.DataSource = ds.Tables[0];

                                        reports.DataMember = "table";

                                        reports.ShowPreviewDialog();

                                    }
                                    else
                                    {
                                        Common.setMessageBox("No Record Found", Program.ApplicationName, 1); return;
                                    }
                                }
                                else
                                {
                                    Common.setMessageBox(ds.Tables[1].Rows[0]["returnMessage"].ToString(), Program.ApplicationName, 2);
                                }

                            }
                        }
                    }
                    else if (Convert.ToInt32(radioGroup1.EditValue) == 2)
                    {
                        using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
                        {
                            connect.Open();
                            _command = new SqlCommand("GetPaymentRecord", connect) { CommandType = CommandType.StoredProcedure };
                            _command.Parameters.Add(new SqlParameter("@bankcode", SqlDbType.VarChar)).Value = cboBank.SelectedValue;
                            _command.Parameters.Add(new SqlParameter("@startDate", SqlDbType.DateTime)).Value = string.Format("{0:yyyy/MM/dd}", dtpStart.Value);
                            _command.Parameters.Add(new SqlParameter("@endDate", SqlDbType.DateTime)).Value = string.Format("{0:yyyy/MM/dd}", dtpEnd.Value);
                            _command.Parameters.Add(new SqlParameter("@PType", SqlDbType.VarChar)).Value = "Not in Bank";
                            _command.Parameters.Add(new SqlParameter("@BatchID", SqlDbType.Int)).Value = Convert.ToInt32(label22.Text);

                            using (System.Data.DataSet ds = new System.Data.DataSet())
                            {
                                ds.Clear();
                                adp = new SqlDataAdapter(_command);
                                adp.Fill(ds, "table");
                                //Dts = ds.Tables[0];
                                connect.Close();

                                if (ds.Tables[1].Rows[0]["returnCode"].ToString() == "00")
                                {
                                    //label6.Text = String.Format("{0:N2}", ds.Tables[1].Rows[0]["ClosingBal"]);

                                    if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                                    {
                                        XtraRepPayDirectBank rep = new XtraRepPayDirectBank();
                                        rep.xrLabel10.Text = "List of Transactions in REEMS Not in Bank Statement ";

                                        rep.xrLabel12.Text = string.Format("between {0} and {1}", string.Format("{0:dd/MM/yyyy}", dtpStart.Value), string.Format("{0:dd/MM/yyyy}", dtpEnd.Value));


                                        rep.DataSource = ds.Tables[0];

                                        rep.DataMember = "table";

                                        rep.ShowPreviewDialog();

                                    }
                                    else
                                    {
                                        Common.setMessageBox("No Record Found", Program.ApplicationName, 1); return;
                                    }
                                }
                                else
                                {
                                    Common.setMessageBox(ds.Tables[1].Rows[0]["returnMessage"].ToString(), Program.ApplicationName, 2);
                                }

                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Tripous.Sys.ErrorBox(ex.StackTrace + ex.Message);
                    return;
                }
                finally
                {
                    SplashScreenManager.CloseForm(false);
                }
            }
        }

        void bttnreset_Click(object sender, EventArgs e)
        {
            //if (string.IsNullOrEmpty(cboBank.SelectedValue.ToString()))
            //{
            //    Common.setEmptyField("Bank Name", "Transcation"); return;
            //}
            //else
            //{
            //    //check if period already closed or not
            //    string qy = string.Format("SELECT IsClosed FROM  Reconciliation.tblPeriods WHERE BankShortCode='{0}' AND CONVERT(VARCHAR(10),[Start Date],103)='{1}' AND CONVERT(VARCHAR(10),[End Date],103)='{2}' AND BatchCode='{3}'", cboBank.SelectedValue, string.Format("{0:dd/MM/yyy}", dtpStart.Value), string.Format("{0:dd/MM/yyy}", dtpEnd.Value), label22.Text.Trim());

            //    using (var ds = new System.Data.DataSet())
            //    {
            //        using (SqlDataAdapter ada = new SqlDataAdapter(qy, Logic.ConnectionString))
            //        {
            //            ada.Fill(ds, "table");
            //        }

            //        Dt = ds.Tables[0];
            //    }

            //    //string qwy;

            //    if (Dt != null && Dt.Rows.Count > 0)
            //    {
            //        Common.setMessageBox("Sorry, Period Already Closed, so it Can't be Rest", "Reset Period", 2);
            //        return;
            //    }
            //    else
            //    {
            try
            {

                if (!MosesClassLibrary.Utilities.Common.AskQuestion("Resetting this period cannot be undone. Do you want to continue?", "Reset Period"))
                    return;

                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

                using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
                {
                    connect.Open();

                    _command = new SqlCommand("doResetPeriod", connect) { CommandType = CommandType.StoredProcedure };
                    _command.Parameters.Add(new SqlParameter("@periodid", SqlDbType.Int)).Value = Convert.ToInt32(label25.Text);
                    _command.Parameters.Add(new SqlParameter("@UserId", SqlDbType.Char)).Value = Program.UserID;
                    _command.CommandTimeout = 0;

                    using (System.Data.DataSet ds = new System.Data.DataSet())
                    {
                        ds.Clear();
                        adp = new SqlDataAdapter(_command);
                        adp.Fill(ds);
                        connect.Close();

                        if (ds.Tables[0].Rows[0]["returnCode"].ToString() == "00")
                        {
                            Common.setMessageBox(ds.Tables[0].Rows[0]["returnMessage"].ToString(), "Successful", 1);
                            tsbClose.PerformClick();
                            return;
                        }
                        else
                        {
                            //ds.Tables[1].Rows[0]["Acctnumber"].ToString()
                            Common.setMessageBox(ds.Tables[0].Rows[0]["returnMessage"].ToString(), "Reset Transaction Error", 3); return;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Tripous.Sys.ErrorBox(ex.StackTrace + ex.Message);
                return;
            }
            finally
            {
                SplashScreenManager.CloseForm(false);

            }
            //    }
            //}


        }

        void bttnClose_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtClosing.Text))
            {
                Common.setEmptyField("Closing Balance", "Close Period"); return;
            }
            else
            {
                try
                {
                    SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

                    string query = String.Format("SELECT COUNT(Amount) AS Count FROM tblPeriods WHERE BankCode='{0}' and CONVERT(VARCHAR(10),[Start Date],103)='{1}' and CONVERT(VARCHAR(10),[End Date],103)='{2}' AND IsClosed=1", (string)cboBank.SelectedValue, string.Format("{0:dd/MM/yyy}", dtpStart.Value), string.Format("{0:dd/MM/yyy}", dtpEnd.Value));

                    if (new Logic().IsRecordExist(query))
                    {
                        Common.setMessageBox("Period Already Close", Program.ApplicationName, 2);
                    }
                    else
                    {
                        //now compare the close and open before insert record
                        if (Convert.ToDouble(label6.Text) != Convert.ToDouble(txtClosing.Text))
                        {
                            Common.setMessageBox("Closing Balance for the Transaction Period not Correct", Program.ApplicationName, 3); return;
                        }
                        else
                        {
                            //commit close period into tbale per period and account

                            using (SqlConnection db = new SqlConnection(Logic.ConnectionString))
                            {
                                SqlTransaction transaction;

                                db.Open();
                                //string.Format("{0}{1}{2}{3}{4}",Program.UserID, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second, DateTime.Now.Millisecond);
                                transaction = db.BeginTransaction();
                                try
                                {

                                    using (SqlCommand sqlCommand = new SqlCommand(String.Format("INSERT INTO tblPeriods( [Start Date] ,[End Date] ,BankCode , Amount , ReconciliationID , IsClosed,CloseBal ) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}');", string.Format("{0:yyyy/MM/dd}", dtpStart.Value), string.Format("{0:yyyy/MM/dd}", dtpEnd.Value), cboBank.SelectedValue, Convert.ToDouble(txtOpening.Text), string.Format("{0}{1}{2}{3}{4}", Program.UserID, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second, DateTime.Now.Millisecond), 1, Convert.ToDouble(txtClosing.Text)), db, transaction))
                                    {
                                        sqlCommand.ExecuteNonQuery();
                                    }


                                    transaction.Commit();

                                }
                                catch (SqlException sqlError)
                                {
                                    transaction.Rollback();
                                    Tripous.Sys.ErrorBox(sqlError); return;
                                }
                                db.Close();
                            }

                            Common.setMessageBox("Transaction Period Closed", Program.ApplicationName, 1); return;

                        }
                    }
                }
                catch (Exception ex)
                {
                    Tripous.Sys.ErrorBox(ex.StackTrace + ex.Message);
                    return;
                }
                finally
                {
                    SplashScreenManager.CloseForm(false);
                }
            }
        }

        //void cboAcct_SelectedIndexChanged(object sender, EventArgs e)
        //{


        //    //if (cboAcct.SelectedValue != null && !isRecord)
        //    //{

        //    //    try
        //    //    {
        //    //        SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);



        //    //        using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
        //    //        {
        //    //            connect.Open();
        //    //            _command = new SqlCommand("CalcuteClosingBal", connect) { CommandType = CommandType.StoredProcedure };
        //    //            _command.Parameters.Add(new SqlParameter("@bankcode", SqlDbType.VarChar)).Value = cboBank.SelectedValue;
        //    //            _command.Parameters.Add(new SqlParameter("@startDate", SqlDbType.DateTime)).Value = string.Format("{0:yyyy/MM/dd}", dtpStart.Value);
        //    //            _command.Parameters.Add(new SqlParameter("@endDate", SqlDbType.DateTime)).Value = string.Format("{0:yyyy/MM/dd}", dtpEnd.Value);

        //    //            using (System.Data.DataSet ds = new System.Data.DataSet())
        //    //            {
        //    //                ds.Clear();
        //    //                adp = new SqlDataAdapter(_command);
        //    //                adp.Fill(ds);
        //    //                //Dts = ds.Tables[0];
        //    //                connect.Close();

        //    //                if (ds.Tables[0].Rows[0]["returnCode"].ToString() == "00")
        //    //                {

        //    //                    if (string.IsNullOrEmpty(ds.Tables[1].Rows[0]["OpeningBal"].ToString()))
        //    //                    {
        //    //                        txtOpening.Text = "0.0";
        //    //                    }
        //    //                    else
        //    //                    {
        //    //                        txtOpening.Text = String.Format("{0:N2}", ds.Tables[1].Rows[0]["OpeningBal"]);
        //    //                    }
        //    //                    if (string.IsNullOrEmpty(ds.Tables[1].Rows[0]["PayDirectBank"].ToString()))
        //    //                    {
        //    //                        linkLabel2.Text = "0.0";
        //    //                    }
        //    //                    else
        //    //                    {
        //    //                        linkLabel2.Text = String.Format("{0:N2}", ds.Tables[1].Rows[0]["PayDirectBank"]);
        //    //                    }
        //    //                    if (string.IsNullOrEmpty(ds.Tables[1].Rows[0]["BankPayDirect"].ToString()))
        //    //                    {
        //    //                        linkLabel4.Text = "0.0";
        //    //                    }
        //    //                    else
        //    //                    {
        //    //                        linkLabel4.Text = String.Format("{0:N2}", ds.Tables[1].Rows[0]["BankPayDirect"]);
        //    //                    }
        //    //                    if (string.IsNullOrEmpty(ds.Tables[1].Rows[0]["BankCollection"].ToString()))
        //    //                    {
        //    //                        linkLabel3.Text = "0.0";
        //    //                    }
        //    //                    else
        //    //                    {
        //    //                        linkLabel3.Text = String.Format("{0:N2}", ds.Tables[1].Rows[0]["BankCollection"]);
        //    //                    }
        //    //                    if (string.IsNullOrEmpty(ds.Tables[1].Rows[0]["ReemsCollection"].ToString()))
        //    //                    {
        //    //                        linkLabel1.Text = "0.0";
        //    //                    }
        //    //                    else
        //    //                    {
        //    //                        linkLabel1.Text = String.Format("{0:N2}", ds.Tables[1].Rows[0]["ReemsCollection"]);
        //    //                    }
        //    //                }
        //    //                else
        //    //                {
        //    //                    Common.setMessageBox(ds.Tables[0].Rows[0]["returnMessage"].ToString(), Program.ApplicationName, 2);
        //    //                }

        //    //            }
        //    //        }
        //    //    }
        //    //    catch (Exception ex)
        //    //    {
        //    //        Tripous.Sys.ErrorBox(ex.StackTrace + ex.Message);
        //    //        return;
        //    //    }
        //    //    finally
        //    //    {
        //    //        SplashScreenManager.CloseForm(false);
        //    //    }
        //    //}
        //    //else
        //    //{
        //    //    txtOpening.Text = string.Empty;
        //    //    txtClosing.Text = string.Empty;
        //    //    //txtUncleared.Text = string.Empty;
        //    //    //txtClear.Text = string.Empty;
        //    //    //gridControl5.DataSource = null;
        //    //    //label34.Text = string.Empty;
        //    //    linkLabel1.Text = string.Empty;
        //    //    linkLabel2.Text = string.Empty;
        //    //    linkLabel3.Text = string.Empty;
        //    //    linkLabel4.Text = string.Empty;
        //    //}
        //}

        void bttnPostingrec_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    SplashScreenManager.ShowForm(this, typeof (WaitForm1), true, true, false);

            if (isClosed)
            {
                Common.setMessageBox("Operation Can't be Perform! Period Closed", Program.ApplicationName, 1); return;
            }
            else
            {
                //check if record be sent for posting before
                string qurty =
                String.Format(
                    "SELECT COUNT(*) AS Count FROM   Reconciliation.tblTransactionPostingRequest WHERE  BankShortCode = '{0}' AND FinancialperiodID ='{1}' AND BankAccountID ='{2}'  AND StartDate = '{3}' AND EndDate = '{4}'",
                    cboBank.SelectedValue, label22.Text.ToString(), Convert.ToInt32(cboAccount.SelectedValue),
                    string.Format("{0:yyyy-MM-dd}", dtpStart.Value), string.Format("{0:yyyy-MM-dd}", dtpEnd.Value));

                if (new Logic().IsRecordExist(qurty))

                //if (retval == "1")
                {

                    Common.setMessageBox(
                        string.Format("Posting Request for this Bank: {0} transction has done", cboBank.Text),
                        Program.ApplicationName + "Posting Request", 1);
                    return;

                }
                else
                {
                    try
                    {
                        if (dbcredit.Columns.Contains("PayerName")) dbcredit.Columns.Remove("PayerName");
                        using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
                        {
                            connect.Open();
                            _command = new SqlCommand("PostingRequest", connect) { CommandType = CommandType.StoredProcedure };
                            //_command.Parameters.Add(new SqlParameter("@BankCode", SqlDbType.VarChar)).Value =
                            //    cboBank.SelectedValue;
                            //_command.Parameters.Add(new SqlParameter("@startdate", SqlDbType.VarChar)).Value =
                            //    string.Format("{0:yyyy/MM/dd}", dtpStart.Value);
                            //_command.Parameters.Add(new SqlParameter("@enddate", SqlDbType.VarChar)).Value =
                            //    string.Format("{0:yyyy/MM/dd}", dtpEnd.Value);
                            _command.Parameters.Add(new SqlParameter("@openingbal", SqlDbType.Money)).Value =
                                Convert.ToDecimal(label14.Text);
                            _command.Parameters.Add(new SqlParameter("@closebal", SqlDbType.Money)).Value =
                                Convert.ToDecimal(label6.Text);
                            _command.Parameters.Add(new SqlParameter("@ReconID", SqlDbType.Int)).Value = Convert.ToInt32(label25.Text);
                            //_command.Parameters.Add(new SqlParameter("@AccountId", SqlDbType.Int)).Value =
                            //Convert.ToInt32(cboAccount.SelectedValue);
                            _command.Parameters.Add(new SqlParameter("@Requestby", SqlDbType.VarChar)).Value =
                                Program.UserID;

                            _command.Parameters.Add(new SqlParameter("@pTransactiondb", SqlDbType.Structured)).Value =
                                dbcredit;
                            //@Years
                            using (System.Data.DataSet ds = new System.Data.DataSet())
                            {
                                ds.Clear();
                                adp = new SqlDataAdapter(_command);
                                adp.Fill(ds);
                                connect.Close();

                                if (ds.Tables[0].Rows[0]["returnCode"].ToString() != "00")
                                {
                                    Common.setMessageBox(ds.Tables[0].Rows[0]["returnMessage"].ToString(),
                                        Program.ApplicationName, 2);

                                    return;

                                }
                                else
                                {
                                    Common.setMessageBox(ds.Tables[0].Rows[0]["returnMessage"].ToString(),
                                        Program.ApplicationName, 2);

                                    //bttnclosedper.Enabled = true;
                                    bttnPostingrec.Enabled = false;
                                    return;

                                }

                            }
                        }
                    }
                    catch
                (Exception
                ex)
                    {
                        Tripous.Sys.ErrorBox(ex.StackTrace + ex.Message);
                        return;
                    }
                    finally
                    {
                        SplashScreenManager.CloseForm(false);
                    }
                }

            }



            #region

            //if (cbopaymethod.SelectedIndex == -1)
            //{
            //    Common.setEmptyField("Payment Method", "Record Posting");
            //    cbopaymethod.Focus(); return;
            //}
            //else if (cbobranch.SelectedIndex == -1)
            //{
            //    Common.setEmptyField("Paid Bank Branch", "Record Posting");
            //    cbobranch.Focus(); return;
            //}
            //else
            //{
            //    try
            //    {
            //        SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

            //        DataTable dts = (new Logic()).getSqlStatement((String.Format("SELECT Description,Type FROM dbo.tblTransDefinition	WHERE ElementCatCode = '{0}' ", (string)selectedPages))).Tables[0];

            //        if (dts != null && dts.Rows.Count > 0)
            //        {
            //            ElemDescr = (string)dts.Rows[0]["Description"];
            //            ElemType = (string)dts.Rows[0]["Type"];
            //        }

            //        string quytest = string.Format("SELECT AccountNumber,BranchCode FROM ViewBankBranchAccount where BankShortCode like '%{0}%'", cboBank.SelectedValue);

            //        DataTable dtes = (new Logic()).getSqlStatement((quytest)).Tables[0];

            //        if (dtes != null && dtes.Rows.Count > 0)
            //        {
            //            strAcct = (string)dtes.Rows[0]["AccountNumber"];
            //            strBranch = (string)dtes.Rows[0]["BranchCode"];
            //        }

            //        BatchNumber = String.Format("{0:d5}", (DateTime.Now.Ticks / 10) % 100000);


            //        strpayerID = string.Format("{0}|{1}|{2:yyyMMddhhmmss}", cboBank.SelectedValue, BatchNumber, DateTime.Now);
            //        strpaymentRef = String.Format("{0}|OGPRC|{1}|{2:dd-MM-yyyy}|{3}", cboBank.SelectedValue, strBranch, DateTime.Now, string.Format("{0}{1}{2}{3}", DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second, DateTime.Now.Millisecond));

            //        string[] Splits1 = txtpaymentdate.Text.Split(new Char[] { '/' });

            //        string strDate = String.Format("{0}/{1}/{2}", Splits1[1], Splits1[0], Splits1[2]);

            //        string query3;

            //        #region oldcode
            //        //if (radioGroup2.EditValue.ToString() == "Yes")
            //        //{//post record into transaction posting
            //        //    using (SqlConnection db = new SqlConnection(Logic.ConnectionString))
            //        //    {
            //        //        SqlTransaction transaction;

            //        //        db.Open();

            //        //        transaction = db.BeginTransaction();

            //        //        try
            //        //        {                               //insert receord into the tbltransactionposting
            //        //            string query4 = string.Format("INSERT INTO tblTransactionPosting (AccountNo,Type,TransDate,TransDescription,Dr,Cr,StartDate,EndDate,BatchID) VALUES ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}');", strAcct, ElemType, strDate, ElemDescr, 0, Convert.ToDecimal(txtpstamount.Text), string.Format("{0:yyyy/MM/dd}", dtpStart.Value), string.Format("{0:yyyy/MM/dd}", dtpEnd.Value), Convert.ToInt32(label21.Text));
            //        //            using (SqlCommand sqlCommand = new SqlCommand(query4, db, transaction))
            //        //            {
            //        //                sqlCommand.ExecuteNonQuery();
            //        //            }

            //        //            string dry = String.Format("UPDATE tblAllocateCredit SET IsPosted=1 where  BankCode ='{0}' and CONVERT(VARCHAR, TransDate, 103)='{1}' and TransAmount='{2}'", cboBank.SelectedValue, string.Format("{0:yyyy/MM/dd}", txtpaymentdate.Text), Convert.ToDecimal(txtpstamount.Text));

            //        //            using (SqlCommand sqlCommand2 = new SqlCommand(dry, db, transaction))
            //        //            {
            //        //                sqlCommand2.ExecuteNonQuery();
            //        //            }

            //        //            transaction.Commit();
            //        //        }
            //        //        catch (SqlException sqlError)
            //        //        {
            //        //            transaction.Rollback(); Tripous.Sys.ErrorBox(sqlError);
            //        //            return;
            //        //        }
            //        //        db.Close();
            //        //    }
            //        //}
            //        //else if (radioGroup2.EditValue.ToString() == "No")
            //        //{
            //        #endregion
            //        //insert record
            //        using (SqlConnection db = new SqlConnection(Logic.ConnectionString))
            //        {
            //            SqlTransaction transaction;

            //            db.Open();

            //            transaction = db.BeginTransaction();

            //            try
            //            {
            //                if (checkBox1.Checked == true)
            //                {
            //                    query3 = String.Format("INSERT INTO tblCollectionReport([Provider],[Channel],[PaymentRefNumber],[PaymentDate],[PayerID],[Amount],[PaymentMethod],[BankCode],[BankName],[GeneratedBy],[UploadStatus],[ChequeStatus],[IsPayDirect],[IsRecordExit],[ChequeValueDate],[AgencyCode],[AgencyName],[RevenueCode],DESCRIPTION,BranchCode,BranchName,PayerAddress,PayerName,BatchID) VALUES ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}','{19}','{20}','{21}','{22}','{23}');", "ICMA", "Bank", strpaymentRef, strDate, strpayerID, Convert.ToDecimal(txtpstamount.Text), cbopaymethod.Text, cboBank.SelectedValue, cboBank.Text, Program.UserID, "Waiting", "Cleared", false, true, strDate, null, null, null, null, cbobranch.SelectedValue, cbobranch.Text, txtpsaddress.Text, txtpsname.Text, Convert.ToInt32(label21.Text));
            //                }
            //                else
            //                {
            //                    //cleared item check
            //                    if (chkUncleared.Checked == true)
            //                    {
            //                        query3 = String.Format("INSERT INTO tblCollectionReport([Provider],[Channel],[PaymentRefNumber],[PaymentDate],[PayerID],[Amount],[PaymentMethod],[BankCode],[BankName],[GeneratedBy],[UploadStatus],[ChequeStatus],[IsPayDirect],[IsRecordExit],[ChequeValueDate],[AgencyCode],[AgencyName],[RevenueCode],DESCRIPTION,BranchCode,BranchName,PayerAddress,PayerName,BatchID) VALUES ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}','{19}','{20}','{21}','{22}','{23}');", "ICMA", "Bank", strpaymentRef, strDate, strpayerID, Convert.ToDecimal(txtpstamount.Text), cbopaymethod.Text, cboBank.SelectedValue, cboBank.Text, Program.UserID, "Waiting", "Uncleared", false, true, strDate, label7.Text, label55.Text, cboRevenuetype.SelectedValue, cboRevenuetype.Text, cbobranch.SelectedValue, cbobranch.Text, txtpsaddress.Text, txtpsname.Text, Convert.ToInt32(label21.Text));
            //                    }
            //                    else
            //                    {
            //                        query3 = String.Format("INSERT INTO tblCollectionReport([Provider],[Channel],[PaymentRefNumber],[PaymentDate],[PayerID],[Amount],[PaymentMethod],[BankCode],[BankName],[GeneratedBy],[UploadStatus],[ChequeStatus],[IsPayDirect],[IsRecordExit],[ChequeValueDate],[AgencyCode],[AgencyName],[RevenueCode],DESCRIPTION,BranchCode,BranchName,PayerAddress,PayerName,BatchID) VALUES ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}','{19}','{20}','{21}','{22}','{23}');", "ICMA", "Bank", strpaymentRef, strDate, strpayerID, Convert.ToDecimal(txtpstamount.Text), cbopaymethod.Text, cboBank.SelectedValue, cboBank.Text, Program.UserID, "Waiting", "Cleared", false, true, strDate, label7.Text, label55.Text, cboRevenuetype.SelectedValue, cboRevenuetype.Text, cbobranch.SelectedValue, cbobranch.Text, txtpsaddress.Text, txtpsname.Text, Convert.ToInt32(label21.Text));
            //                    }

            //                }
            //                //insert into collection table


            //                using (SqlCommand sqlCommand1 = new SqlCommand(query3, db, transaction))
            //                {
            //                    sqlCommand1.ExecuteNonQuery();
            //                }

            //                //insert receord into the tbltransactionposting
            //                string query4 = string.Format("INSERT INTO tblTransactionPosting (AccountNo,Type,TransDate,TransDescription,Dr,Cr,StartDate,EndDate,BatchID) VALUES ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}');", strAcct, ElemType, strDate, ElemDescr, 0, Convert.ToDecimal(txtpstamount.Text), string.Format("{0:yyyy/MM/dd}", dtpStart.Value), string.Format("{0:yyyy/MM/dd}", dtpEnd.Value), Convert.ToInt32(label21.Text));

            //                using (SqlCommand sqlCommand = new SqlCommand(query4, db, transaction))
            //                {
            //                    sqlCommand.ExecuteNonQuery();
            //                }

            //                string dry = String.Format("UPDATE tblAllocateCredit SET IsPosted=1 where  BankCode ='{0}' and CONVERT(VARCHAR, TransDate, 103)='{1}' and TransAmount='{2}'", cboBank.SelectedValue, string.Format("{0:yyyy/MM/dd}", txtpaymentdate.Text), Convert.ToDecimal(txtpstamount.Text));

            //                using (SqlCommand sqlCommand2 = new SqlCommand(dry, db, transaction))
            //                {
            //                    sqlCommand2.ExecuteNonQuery();
            //                }

            //                transaction.Commit();
            //            }
            //            catch (SqlException sqlError)
            //            {
            //                transaction.Rollback(); Tripous.Sys.ErrorBox(sqlError);
            //                return;
            //            }
            //            db.Close();
            //        }
            //        //}
            //        //deleteing record from in bank statement
            //        for (int h = 0; h < dbcredit.Rows.Count; h++)
            //        {
            //            if (dbcredit.Rows[h]["DATE"].ToString() == txtpaymentdate.Text && Convert.ToDouble(dbcredit.Rows[h]["Amount"]) == Convert.ToDouble(txtpstamount.Text))
            //            {
            //                dbcredit.Rows[h].Delete();
            //            }
            //        }
            //        dbcredit.AcceptChanges();

            //        if (dbcredit != null && dbcredit.Rows.Count > 0)
            //        {
            //            gridControl8.DataSource = dbcredit;
            //            gridView10.OptionsBehavior.Editable = false;
            //            gridView10.Columns["Amount"].DisplayFormat.FormatType = FormatType.Numeric;
            //            gridView10.Columns["Amount"].DisplayFormat.FormatString = "n2";
            //            gridView10.Columns["Amount"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            //            gridView10.Columns["Date"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            //            gridView10.Columns["Date"].DisplayFormat.FormatString = "dd/MM/yyyy";
            //            gridView10.Columns["Amount"].SummaryItem.FieldName = "Amount";
            //            gridView10.Columns["Amount"].SummaryItem.DisplayFormat = "Total = {0:n}";
            //            //gridView3.OptionsView.ColumnAutoWidth = false;
            //            gridView10.OptionsView.ShowFooter = true;
            //            gridView10.BestFitColumns();
            //        }

            //        ////saveing record back
            //        //if (dbcredit != null && dbcredit.Rows.Count > 0)
            //        //{
            //        //    using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
            //        //    {
            //        //        connect.Open();
            //        //        _command = new SqlCommand("InsertBankPayDirect", connect) { CommandType = CommandType.StoredProcedure };
            //        //        _command.Parameters.Add(new SqlParameter("@bankcode", SqlDbType.VarChar)).Value = cboBank.SelectedValue;
            //        //        _command.Parameters.Add(new SqlParameter("@startdate", SqlDbType.VarChar)).Value = string.Format("{0:yyyy/MM/dd}", dtpStart.Value);
            //        //        _command.Parameters.Add(new SqlParameter("@enddate", SqlDbType.VarChar)).Value = string.Format("{0:yyyy/MM/dd}", dtpEnd.Value);
            //        //        _command.Parameters.Add(new SqlParameter("@pTransaction", SqlDbType.Structured)).Value = dbcredit;

            //        //        using (System.Data.DataSet ds = new System.Data.DataSet())
            //        //        {
            //        //            ds.Clear();
            //        //            adp = new SqlDataAdapter(_command);
            //        //            adp.Fill(ds);
            //        //            //Dts = ds.Tables[0];
            //        //            connect.Close();

            //        //            if (ds.Tables[0].Rows[0]["returnCode"].ToString() != "00")
            //        //            {
            //        //                //Common.setMessageBox(ds.Tables[0].Rows[0]["returnMessage"].ToString(), Program.ApplicationName, 2);
            //        //                //CalClose();
            //        //            }
            //        //            //else
            //        //            //{
            //        //            //    Common.setMessageBox(ds.Tables[0].Rows[0]["returnMessage"].ToString(), Program.ApplicationName, 2);
            //        //            //}

            //        //        }
            //        //    }
            //        //}
            //        //groupBox4.Enabled = true;

            //        //CalClose();
            //    }
            //    finally
            //    {
            //        SplashScreenManager.CloseForm(false);
            //    }
            //    Common.setMessageBox("Transaction Posted Successfully..", Program.ApplicationName, 1);
            //    clearRec();
            //    return;
            //}
            #endregion
        }

        void gridView10_DoubleClick(object sender, EventArgs e)
        {
            //GetRecord();
        }

        void clearRec()
        {
            txtpaymentdate.Text = string.Empty; txtpstamount.Text = string.Empty; cboRevenuetype.SelectedIndex = -1; txtpsname.Text = string.Empty; txtpsaddress.Text = string.Empty; txtslip.Text = string.Empty; cbobranch.SelectedIndex = -1; cbopaymethod.SelectedIndex = -1; label55.Text = string.Empty; label7.Text = string.Empty; chkUncleared.Checked = false; checkBox1.Checked = false;
        }

        void cboRevenuetype_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboRevenuetype.SelectedValue != null && !isRecord2)
            {
                getAgency(cboRevenuetype.SelectedValue.ToString());
            }
            else
            {
                label55.Text = string.Empty;
                label7.Text = string.Empty;
            }
        }

        void cboRevenuetype_KeyPress(object sender, KeyPressEventArgs e)
        {
            Methods.AutoComplete(cboRevenuetype, e, true);
        }

        void xtraTabControl1_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            label6.Text = string.Format("{0:n2}", GetclosingBalance(Convert.ToInt32(cboAccount.SelectedValue), cboBank.SelectedValue.ToString(), Convert.ToInt32(label22.Text)));

            if (e.Page.Name == "xtraTabPage4")
            {
                try
                {
                    SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

                    setDBComboBoxRev(); setDbCombocBranch();
                    //setDBComboxPaymode();

                    setReloadCreditExpection(); bttnclosedper.Enabled = true;
                }
                finally
                {
                    SplashScreenManager.CloseForm(false);
                }
            }

            else if (e.Page.Name == "xtraTabPage5")
            {
                try
                {
                    SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

                    setDBComBoxAccount();
                }
                finally
                {
                    SplashScreenManager.CloseForm(false);
                }
            }
            else if (e.Page.Name == "xtraTabPage2")
            {
                colView.Name = "Description";
                colView.FieldName = "Description";

                colView2.Name = "Description";
                colView2.FieldName = "Description";



                try
                {
                    SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

                    //check allocate table for pervious data
                    string query = string.Format("SELECT COUNT(*) FROM Reconciliation.tblBankStatementRelation  INNER JOIN Reconciliation.tblBankStatement ON Reconciliation.tblBankStatementRelation.BSID= Reconciliation.tblBankStatement.BSID INNER JOIN Reconciliation.tblBankStatementAllocation ON Reconciliation.tblBankStatement.BSID = Reconciliation.tblBankStatementAllocation.BSID WHERE ReconCode='B' AND BankShortCode='{0}' AND FinancialperiodID ='{1}' AND CONVERT(VARCHAR(10),Reconciliation.tblBankStatement.StartDate,103)='{2}' AND CONVERT(VARCHAR(10),Reconciliation.tblBankStatement.EndDate,103)='{3}' and Reconciliation.tblBankStatement.BankAccountID='{4}'", cboBank.SelectedValue, label22.Text.Trim(), string.Format("{0:dd/MM/yyy}", dtpStart.Value), string.Format("{0:dd/MM/yyy}", dtpEnd.Value), Convert.ToInt32(cboAccount.SelectedValue));

                    try
                    {
                        if (new Logic().IsRecordExist(query))
                        {
                            boolIsUpdate2 = true;
                            setReloadDebit(); setReloadCredit();

                        }
                        else
                        {
                            setReloadDB(); setReloadCD();
                            boolIsUpdate2 = false;
                        }


                    }
                    catch (Exception ex)
                    {
                        Tripous.Sys.ErrorBox(ex); return;
                    }

                }
                finally
                {
                    SplashScreenManager.CloseForm(false);
                }

            }
        }

        void bttnSave_Click(object sender, EventArgs e)
        {
            //            if (string.IsNullOrEmpty((string)(cboBank.SelectedValue)))
            //            {
            //                Common.setEmptyField("BanK Name", Program.ApplicationName);
            //                cboBank.Focus();
            //                return;
            //            }
            //            else if (dbmissing == null || dbmatched == null || dbmissingpay == null || dbCrDebit == null)
            //            {
            //                Common.setEmptyField("No Comparing Data Result Yet...", Program.ApplicationName);
            //                return;
            //            }
            //            else
            //            {
            //                //DialogResult results = MessageBox.Show("Is Closing Balance Correct ?", "Bank Statement / PayDirect", MessageBoxButtons.YesNo);

            //                //if (results == DialogResult.Yes)
            //                //{
            //                string value = string.Empty;

            //                if (DialogResults.InputBox(@"Bank Statement / PayDirect", "Enter Actual Closing Balance on Bank Statement:", ref value) == DialogResult.OK)
            //                {
            //                    value = String.Format("{0:N2}", Convert.ToDecimal(value));

            //                    if (value == label6.Text)
            //                    {
            //                        if (isClosed)
            //                        {
            //                            Common.setMessageBox("Operation Can't be Perform! Period Closed", Program.ApplicationName, 1); return;
            //                        }
            //                        else
            //                        {
            //                            try
            //                            {
            //                                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
            //                                //insert into paydirect table
            //                                if (dbmissingpay != null && dbmissingpay.Rows.Count > 0)
            //                                {
            //                                    using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
            //                                    {
            //                                        connect.Open();
            //                                        _command = new SqlCommand("InsertPayDirectBank", connect) { CommandType = CommandType.StoredProcedure };
            //                                        _command.Parameters.Add(new SqlParameter("@bankcode", SqlDbType.VarChar)).Value = cboBank.SelectedValue;
            //                                        _command.Parameters.Add(new SqlParameter("@startdate", SqlDbType.VarChar)).Value = string.Format("{0:yyyy/MM/dd}", dtpStart.Value);
            //                                        _command.Parameters.Add(new SqlParameter("@enddate", SqlDbType.VarChar)).Value = string.Format("{0:yyyy/MM/dd}", dtpEnd.Value);
            //                                        _command.Parameters.Add(new SqlParameter("@pTransaction", SqlDbType.Structured)).Value = dbmissingpay;
            //                                        _command.Parameters.Add(new SqlParameter("@Periods", SqlDbType.Int)).Value = label22.Text;

            //                                        using (System.Data.DataSet ds = new System.Data.DataSet())
            //                                        {
            //                                            ds.Clear();
            //                                            adp = new SqlDataAdapter(_command);
            //                                            adp.Fill(ds);
            //                                            //Dts = ds.Tables[0];
            //                                            connect.Close();

            //                                        }
            //                                    }
            //                                }

            //                                //bank statement
            //                                if (dbmissing != null && dbmissing.Rows.Count > 0)
            //                                {
            //                                    using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
            //                                    {
            //                                        connect.Open();
            //                                        _command = new SqlCommand("InsertBankPayDirect", connect) { CommandType = CommandType.StoredProcedure };
            //                                        _command.Parameters.Add(new SqlParameter("@bankcode", SqlDbType.VarChar)).Value = cboBank.SelectedValue;
            //                                        _command.Parameters.Add(new SqlParameter("@startdate", SqlDbType.VarChar)).Value = string.Format("{0:yyyy/MM/dd}", dtpStart.Value);
            //                                        _command.Parameters.Add(new SqlParameter("@enddate", SqlDbType.VarChar)).Value = string.Format("{0:yyyy/MM/dd}", dtpEnd.Value);
            //                                        _command.Parameters.Add(new SqlParameter("@pTransaction", SqlDbType.Structured)).Value = dbmissing;
            //                                        _command.Parameters.Add(new SqlParameter("@Periods", SqlDbType.Int)).Value = label22.Text;

            //                                        using (System.Data.DataSet ds = new System.Data.DataSet())
            //                                        {
            //                                            ds.Clear();
            //                                            adp = new SqlDataAdapter(_command);
            //                                            adp.Fill(ds);
            //                                            //Dts = ds.Tables[0];
            //                                            connect.Close();


            //                                        }
            //                                    }
            //                                }
            //                                //insert into bancreditdebit
            //                                //if (dbmatched != null && dbmatched.Rows.Count > 0)
            //                                //{
            //                                //    using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
            //                                //    {
            //                                //        connect.Open();
            //                                //        _command = new SqlCommand("InsertBankCreditDebit", connect) { CommandType = CommandType.StoredProcedure };
            //                                //        _command.Parameters.Add(new SqlParameter("@bankcode", SqlDbType.VarChar)).Value = cboBank.SelectedValue;
            //                                //        _command.Parameters.Add(new SqlParameter("@startdate", SqlDbType.VarChar)).Value = string.Format("{0:yyyy/MM/dd}", dtpStart.Value);
            //                                //        _command.Parameters.Add(new SqlParameter("@enddate", SqlDbType.VarChar)).Value = string.Format("{0:yyyy/MM/dd}", dtpEnd.Value);
            //                                //        _command.Parameters.Add(new SqlParameter("@pTransaction", SqlDbType.Structured)).Value = dbmatched;
            //                                //        _command.Parameters.Add(new SqlParameter("@BatchID", SqlDbType.Int)).Value = label22.Text;
            //                                //        using (System.Data.DataSet ds = new System.Data.DataSet())
            //                                //        {
            //                                //            ds.Clear();
            //                                //            adp = new SqlDataAdapter(_command);
            //                                //            adp.Fill(ds);
            //                                //            //Dts = ds.Tables[0];
            //                                //            connect.Close();


            //                                //        }
            //                                //    }
            //                                //}
            //                                //insert into both table
            //                                if (dbmatched != null && dbmatched.Rows.Count > 0)
            //                                {
            //                                    using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
            //                                    {
            //                                        connect.Open();
            //                                        _command = new SqlCommand("InsertBothBankPayDirect", connect) { CommandType = CommandType.StoredProcedure };
            //                                        _command.Parameters.Add(new SqlParameter("@bankcode", SqlDbType.VarChar)).Value = cboBank.SelectedValue;
            //                                        _command.Parameters.Add(new SqlParameter("@startdate", SqlDbType.VarChar)).Value = string.Format("{0:yyyy/MM/dd}", dtpStart.Value);
            //                                        _command.Parameters.Add(new SqlParameter("@enddate", SqlDbType.VarChar)).Value = string.Format("{0:yyyy/MM/dd}", dtpEnd.Value);
            //                                        _command.Parameters.Add(new SqlParameter("@pTransaction", SqlDbType.Structured)).Value = dbmatched;
            //                                        _command.Parameters.Add(new SqlParameter("@Periods", SqlDbType.Int)).Value = label22.Text;
            //                                        using (System.Data.DataSet ds = new System.Data.DataSet())
            //                                        {
            //                                            ds.Clear();
            //                                            adp = new SqlDataAdapter(_command);
            //                                            adp.Fill(ds);
            //                                            //Dts = ds.Tables[0];
            //                                            connect.Close();


            //                                            if (ds.Tables[0].Rows[0]["returnCode"].ToString() != "00")
            //                                            {
            //                                                Common.setMessageBox(ds.Tables[0].Rows[0]["returnMessage"].ToString(), Program.ApplicationName, 2);
            //                                                //CalClose();
            //                                            }
            //                                            else
            //                                            {
            //                                                Common.setMessageBox(ds.Tables[0].Rows[0]["returnMessage"].ToString(), Program.ApplicationName, 1);
            //                                            }
            //                                        }
            //                                    }
            //                                }

            //                                groupBox4.Enabled = true;

            //                                bttnNext.Enabled = true;
            //                            }

            //                            finally
            //                            {
            //                                SplashScreenManager.CloseForm(false);
            //                            }
            //                        }


            //                        label6.ForeColor = Color.Green;

            //                    }
            //                    else
            //                    {
            //                        Common.setMessageBox(string.Format(@"Closing Balance not equal.
            //Calculated Balance:{0}
            //Closing Balance:{1}
            //Do Check again", label6.Text, String.Format("{0:N2}", Convert.ToDecimal(value))), "Bank Statement / PayDirect", 3); return;
            //                    }
            //                }
            //                //}
            //                //else
            //                //{
            //                //    bttncancelbs_Click(null, null);
            //                //    return;
            //                //}


            //            }
        }

        bool SaveCompareResultToDatabase()
        {
            if (string.IsNullOrEmpty((string)(cboBank.SelectedValue)))
            {
                Common.setEmptyField("BanK Name", Program.ApplicationName);
                cboBank.Focus();
                return false;
            }
            else if (dbmissing == null || dbmatched == null || dbmissingpay == null || dbCrDebit == null)
            {
                Common.setEmptyField("No Comparing Data Result Yet...", Program.ApplicationName);
                return false;
            }
            else
            {
                //DialogResult results = MessageBox.Show("Is Closing Balance Correct ?", "Bank Statement / PayDirect", MessageBoxButtons.YesNo);

                //if (results == DialogResult.Yes)
                //{
                string value = string.Empty;

                if (DialogResults.InputBox(@"Bank Statement / PayDirect", "Enter Actual Closing Balance on Bank Statement:", ref value) == DialogResult.OK)
                {
                    value = String.Format("{0:N2}", Convert.ToDecimal(value));

                    if (value == label6.Text)
                    {
                        if (isClosed)
                        {
                            Common.setMessageBox("Operation Can't be Perform! Period Closed", Program.ApplicationName, 1); return false;
                        }
                        else
                        {
                            try
                            {
                                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                                #region old

                                //System.Data.DataSet dss = new System.Data.DataSet();
                                //DataTable t1 = new DataTable();
                                //DataTable t2 = new DataTable();
                                //DataTable t3 = new DataTable();
                                ////insert into paydirect table
                                ////if ((dbmissingpay != null && dbmissingpay.Rows.Count > 0) || (dbmissing != null && dbmissing.Rows.Count > 0) || (dbmatched != null && dbmatched.Rows.Count > 0))
                                //{
                                //    using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
                                //    {
                                //        connect.Open();
                                //        transaction = connect.BeginTransaction();
                                //        //SqlDataAdapter da = new SqlDataAdapter();
                                //        try
                                //        {
                                //            if (dbmissingpay != null && dbmissingpay.Rows.Count > 0)
                                //            {
                                //                _command = new SqlCommand("InsertPayDirectBank", connect, transaction) { CommandType = CommandType.StoredProcedure };
                                //                _command.Parameters.Add(new SqlParameter("@bankcode", SqlDbType.VarChar)).Value = cboBank.SelectedValue;
                                //                _command.Parameters.Add(new SqlParameter("@startdate", SqlDbType.VarChar)).Value = string.Format("{0:yyyy/MM/dd}", dtpStart.Value);
                                //                _command.Parameters.Add(new SqlParameter("@enddate", SqlDbType.VarChar)).Value = string.Format("{0:yyyy/MM/dd}", dtpEnd.Value);
                                //                _command.Parameters.Add(new SqlParameter("@pTransaction", SqlDbType.Structured)).Value = dbmissingpay;
                                //                _command.Parameters.Add(new SqlParameter("@Periods", SqlDbType.Int)).Value = label22.Text;

                                //                var r1 = _command.ExecuteReader();
                                //                t1.Load(r1);
                                //            }

                                //            if (dbmissing != null && dbmissing.Rows.Count > 0)
                                //            {
                                //                _command1 = new SqlCommand("InsertBankPayDirect", connect, transaction) { CommandType = CommandType.StoredProcedure };
                                //                _command1.Parameters.Add(new SqlParameter("@bankcode", SqlDbType.VarChar)).Value = cboBank.SelectedValue;
                                //                _command1.Parameters.Add(new SqlParameter("@startdate", SqlDbType.VarChar)).Value = string.Format("{0:yyyy/MM/dd}", dtpStart.Value);
                                //                _command1.Parameters.Add(new SqlParameter("@enddate", SqlDbType.VarChar)).Value = string.Format("{0:yyyy/MM/dd}", dtpEnd.Value);
                                //                _command1.Parameters.Add(new SqlParameter("@pTransaction", SqlDbType.Structured)).Value = dbmissing;
                                //                _command1.Parameters.Add(new SqlParameter("@Periods", SqlDbType.Int)).Value = label22.Text;
                                //                var r2 = _command1.ExecuteReader();
                                //                t2.Load(r2);

                                //            }

                                //            if (dbmatched != null && dbmatched.Rows.Count > 0)
                                //            {
                                //                _command2 = new SqlCommand("InsertBothBankPayDirect", connect, transaction) { CommandType = CommandType.StoredProcedure };
                                //                _command2.Parameters.Add(new SqlParameter("@bankcode", SqlDbType.VarChar)).Value = cboBank.SelectedValue;
                                //                _command2.Parameters.Add(new SqlParameter("@startdate", SqlDbType.VarChar)).Value = string.Format("{0:yyyy/MM/dd}", dtpStart.Value);
                                //                _command2.Parameters.Add(new SqlParameter("@enddate", SqlDbType.VarChar)).Value = string.Format("{0:yyyy/MM/dd}", dtpEnd.Value);
                                //                _command2.Parameters.Add(new SqlParameter("@pTransaction", SqlDbType.Structured)).Value = dbmatched;
                                //                _command2.Parameters.Add(new SqlParameter("@Periods", SqlDbType.Int)).Value = label22.Text;
                                //                var r3 = _command2.ExecuteReader();
                                //                t3.Load(r3);
                                //            }
                                //            transaction.Commit();
                                //        }
                                //        catch (Exception sqlEx)
                                //        {
                                //            transaction.Rollback();
                                //            Tripous.Sys.ErrorBox(sqlEx.Message);
                                //            return false;
                                //        }
                                //        finally
                                //        {
                                //            connect.Close();
                                //            connect.Dispose();
                                //        }


                                //        //using (System.Data.DataSet ds = new System.Data.DataSet())
                                //        //{
                                //        //    ds.Clear();
                                //        //    adp = new SqlDataAdapter(_command);
                                //        //    adp.Fill(ds);
                                //        //    //Dts = ds.Tables[0];
                                //        //    connect.Close();

                                //        //}
                                //    }
                                //}
                                #endregion

                                if (!dbmissingpay.Columns.Contains("paymentref")) dbmissingpay.Columns.Add("paymentref");

                                using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
                                {
                                    connect.Open();
                                    //transaction = connect.BeginTransaction();

                                    _command = new SqlCommand("doBankPayDirectCompareResult", connect) { CommandType = CommandType.StoredProcedure };
                                    _command.Parameters.Add(new SqlParameter("@bankcode", SqlDbType.VarChar)).Value = cboBank.SelectedValue;
                                    _command.Parameters.Add(new SqlParameter("@startdate", SqlDbType.VarChar)).Value = string.Format("{0:yyyy/MM/dd}", dtpStart.Value);
                                    _command.Parameters.Add(new SqlParameter("@enddate", SqlDbType.VarChar)).Value = string.Format("{0:yyyy/MM/dd}", dtpEnd.Value);
                                    _command.Parameters.Add(new SqlParameter("@pTransaction", SqlDbType.Structured)).Value = dbmissing;//banks
                                    _command.Parameters.Add(new SqlParameter("@pTransactionReems", SqlDbType.Structured)).Value = dbmissingpay;//reems
                                    _command.Parameters.Add(new SqlParameter("@pTransactionBoth", SqlDbType.Structured)).Value = dbmatched;//both
                                    _command.Parameters.Add(new SqlParameter("@Periods", SqlDbType.Int)).Value = Convert.ToInt32(label22.Text);
                                    _command.Parameters.Add(new SqlParameter("@RecondID", SqlDbType.Int)).Value = Convert.ToInt32(label25.Text);
                                    _command.Parameters.Add(new SqlParameter("@CloseBal", SqlDbType.Decimal)).Value = Convert.ToDecimal(label6.Text);

                                    using (System.Data.DataSet ds = new System.Data.DataSet())
                                    {
                                        ds.Clear();
                                        adp = new SqlDataAdapter(_command);
                                        adp.Fill(ds);
                                        //Dts = ds.Tables[0];
                                        connect.Close();
                                        if (ds.Tables[0].Rows[0]["returnCode"].ToString() != "00")
                                        {
                                            Common.setMessageBox(ds.Tables[0].Rows[0]["returnMessage"].ToString(), Program.ApplicationName, 2); return false;
                                            //CalClose();
                                        }
                                        else
                                        {
                                            Common.setMessageBox(ds.Tables[0].Rows[0]["returnMessage"].ToString(), Program.ApplicationName, 1);
                                        }
                                    }
                                }

                                ////insert into paydirect table
                                //if ((dbmissingpay != null && dbmissingpay.Rows.Count > 0))
                                //{
                                //    using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
                                //    {
                                //        connect.Open();
                                //        //transaction = connect.BeginTransaction();

                                //        _command = new SqlCommand("InsertPayDirectBank", connect) { CommandType = CommandType.StoredProcedure };
                                //        _command.Parameters.Add(new SqlParameter("@bankcode", SqlDbType.VarChar)).Value = cboBank.SelectedValue;
                                //        _command.Parameters.Add(new SqlParameter("@startdate", SqlDbType.VarChar)).Value = string.Format("{0:yyyy/MM/dd}", dtpStart.Value);
                                //        _command.Parameters.Add(new SqlParameter("@enddate", SqlDbType.VarChar)).Value = string.Format("{0:yyyy/MM/dd}", dtpEnd.Value);
                                //        _command.Parameters.Add(new SqlParameter("@pTransaction", SqlDbType.Structured)).Value = dbmissingpay;
                                //        _command.Parameters.Add(new SqlParameter("@Periods", SqlDbType.Int)).Value = label22.Text;

                                //        using (System.Data.DataSet ds = new System.Data.DataSet())
                                //        {
                                //            ds.Clear();
                                //            adp = new SqlDataAdapter(_command);
                                //            adp.Fill(ds);
                                //            //Dts = ds.Tables[0];
                                //            connect.Close();
                                //            if (ds.Tables[0].Rows[0]["returnCode"].ToString() != "00")
                                //            {
                                //                Common.setMessageBox(ds.Tables[0].Rows[0]["returnMessage"].ToString(), Program.ApplicationName, 2); return false;
                                //                //CalClose();
                                //            }
                                //            //else
                                //            //{
                                //            //    Common.setMessageBox(ds.Tables[0].Rows[0]["returnMessage"].ToString(), Program.ApplicationName, 1);
                                //            //}
                                //        }
                                //    }
                                //}

                                //bank statement
                                //if (dbmissing != null && dbmissing.Rows.Count > 0)
                                //{
                                //    using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
                                //    {
                                //        connect.Open();
                                //        _command = new SqlCommand("InsertBankPayDirect", connect) { CommandType = CommandType.StoredProcedure };
                                //        _command.Parameters.Add(new SqlParameter("@bankcode", SqlDbType.VarChar)).Value = cboBank.SelectedValue;
                                //        _command.Parameters.Add(new SqlParameter("@startdate", SqlDbType.VarChar)).Value = string.Format("{0:yyyy/MM/dd}", dtpStart.Value);
                                //        _command.Parameters.Add(new SqlParameter("@enddate", SqlDbType.VarChar)).Value = string.Format("{0:yyyy/MM/dd}", dtpEnd.Value);
                                //        _command.Parameters.Add(new SqlParameter("@pTransaction", SqlDbType.Structured)).Value = dbmissing;
                                //        _command.Parameters.Add(new SqlParameter("@Periods", SqlDbType.Int)).Value = label22.Text;

                                //        using (System.Data.DataSet ds = new System.Data.DataSet())
                                //        {
                                //            ds.Clear();
                                //            adp = new SqlDataAdapter(_command);
                                //            adp.Fill(ds);
                                //            //Dts = ds.Tables[0];
                                //            connect.Close();
                                //            if (ds.Tables[0].Rows[0]["returnCode"].ToString() != "00")
                                //            {
                                //                Common.setMessageBox(ds.Tables[0].Rows[0]["returnMessage"].ToString(), Program.ApplicationName, 2); return false;
                                //                //CalClose();
                                //            }
                                //            //else
                                //            //{
                                //            //    Common.setMessageBox(ds.Tables[0].Rows[0]["returnMessage"].ToString(), Program.ApplicationName, 1);
                                //            //}

                                //        }
                                //    }
                                //}
                                //insert into bancreditdebit
                                //if (dbmatched != null && dbmatched.Rows.Count > 0)
                                //{
                                //    using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
                                //    {
                                //        connect.Open();
                                //        _command = new SqlCommand("InsertBankCreditDebit", connect) { CommandType = CommandType.StoredProcedure };
                                //        _command.Parameters.Add(new SqlParameter("@bankcode", SqlDbType.VarChar)).Value = cboBank.SelectedValue;
                                //        _command.Parameters.Add(new SqlParameter("@startdate", SqlDbType.VarChar)).Value = string.Format("{0:yyyy/MM/dd}", dtpStart.Value);
                                //        _command.Parameters.Add(new SqlParameter("@enddate", SqlDbType.VarChar)).Value = string.Format("{0:yyyy/MM/dd}", dtpEnd.Value);
                                //        _command.Parameters.Add(new SqlParameter("@pTransaction", SqlDbType.Structured)).Value = dbmatched;
                                //        _command.Parameters.Add(new SqlParameter("@BatchID", SqlDbType.Int)).Value = label22.Text;
                                //        using (System.Data.DataSet ds = new System.Data.DataSet())
                                //        {
                                //            ds.Clear();
                                //            adp = new SqlDataAdapter(_command);
                                //            adp.Fill(ds);
                                //            //Dts = ds.Tables[0];
                                //            connect.Close();


                                //        }
                                //    }
                                //}
                                //insert into both table
                                //if (dbmatched != null && dbmatched.Rows.Count > 0)
                                //{
                                //    using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
                                //    {
                                //        connect.Open();
                                //        _command = new SqlCommand("InsertBothBankPayDirect", connect) { CommandType = CommandType.StoredProcedure };
                                //        _command.Parameters.Add(new SqlParameter("@bankcode", SqlDbType.VarChar)).Value = cboBank.SelectedValue;
                                //        _command.Parameters.Add(new SqlParameter("@startdate", SqlDbType.VarChar)).Value = string.Format("{0:yyyy/MM/dd}", dtpStart.Value);
                                //        _command.Parameters.Add(new SqlParameter("@enddate", SqlDbType.VarChar)).Value = string.Format("{0:yyyy/MM/dd}", dtpEnd.Value);
                                //        _command.Parameters.Add(new SqlParameter("@pTransaction", SqlDbType.Structured)).Value = dbmatched;
                                //        _command.Parameters.Add(new SqlParameter("@Periods", SqlDbType.Int)).Value = label22.Text;
                                //        _command.Parameters.Add(new SqlParameter("@CloseBal", SqlDbType.Decimal)).Value = Convert.ToDecimal(label6.Text);

                                //        using (System.Data.DataSet ds = new System.Data.DataSet())
                                //        {
                                //            ds.Clear();
                                //            adp = new SqlDataAdapter(_command);
                                //            adp.Fill(ds);
                                //            //Dts = ds.Tables[0];
                                //            connect.Close();


                                //            if (ds.Tables[0].Rows[0]["returnCode"].ToString() != "00")
                                //            {
                                //                Common.setMessageBox(ds.Tables[0].Rows[0]["returnMessage"].ToString(), Program.ApplicationName, 2);
                                //                //CalClose();
                                //            }
                                //            else
                                //            {
                                //                Common.setMessageBox(ds.Tables[0].Rows[0]["returnMessage"].ToString(), Program.ApplicationName, 1);
                                //            }
                                //        }
                                //    }
                                //}


                                groupBox4.Enabled = true;

                                //bttnNext.Enabled = true;
                                return true;
                            }
                            catch (Exception ex)
                            {
                                Tripous.Sys.ErrorBox(String.Format("{0}{1}", ex.Message, ex.StackTrace)); return false;
                            }

                            finally
                            {
                                SplashScreenManager.CloseForm(false);
                            }
                        }


                        label6.ForeColor = Color.Green;

                    }
                    else
                    {
                        Common.setMessageBox(string.Format(@"Closing Balance not equal.
Calculated Balance:{0}
Closing Balance:{1}
Do Check again", label6.Text, String.Format("{0:N2}", Convert.ToDecimal(value))), "Bank Statement / PayDirect", 3); return false;
                    }
                }
                else
                    return false;
                //}
                //else
                //{
                //    bttncancelbs_Click(null, null);
                //    return;
                //}


            }
        }

        void cboBank_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (cboBank.SelectedValue != null && !Isbank)
            {
                //getopenBal();
                setDBComboBoxAcct();

                //xtraTabPage2.PageEnabled = false;
                //xtraTabPage1.PageEnabled = true; xtraTabPage3.PageEnabled = false;
                //xtraTabControl1.SelectedTabPage = xtraTabPage1;
                //xtraTabControl1.Enabled = true;
            }
        }

        void cboBank_KeyPress(object sender, KeyPressEventArgs e)
        {
            Methods.AutoComplete(cboBank, e, true);
        }

        void bttnMatch_Click(object sender, EventArgs e)
        {
            string values = string.Empty; double values2 = 0.0;
            string values3 = string.Empty; double values4 = 0.0;

            if (selection.SelectedCount == 1 && selection2.SelectedCount == 1)
            {
                for (int i = 0; i < selection.SelectedCount; i++)
                {
                    values = String.Format("{0}", (selection.GetSelectedRow(i) as DataRowView)["DATE"]);
                    values2 = Convert.ToDouble(String.Format("{0}", (selection.GetSelectedRow(i) as DataRowView)["CREDIT"]));
                }

                for (int i = 0; i < selection2.SelectedCount; i++)
                {
                    values3 = String.Format("{0}", (selection2.GetSelectedRow(i) as DataRowView)["DATE"]);
                    values4 = Convert.ToDouble(String.Format("{0}", (selection2.GetSelectedRow(i) as DataRowView)["CREDIT"]));
                }
                //checking both value
                if (values2 == values4)
                {
                    DialogResult result = MessageBox.Show(string.Format("You are Moving this record Date {0} and Amount {1} with Date {2} and Amount {3:}. Is this Correct ' Yes / No '", values, String.Format("{0:0.00}", values2), values3, String.Format("{0:0.00}", values4)), Program.ApplicationName, MessageBoxButtons.YesNo);

                    if (result == DialogResult.Yes)
                    {

                        //adding value from bank statment to both 
                        dbmatched.Rows.Add(values, values2);
                        dbmatched.AcceptChanges();

                        //deleteing record from in bank statement
                        for (int h = 0; h < dbmissing.Rows.Count; h++)
                        {
                            if (dbmissing.Rows[h]["DATE"].ToString() == values && Convert.ToDouble(dbmissing.Rows[h]["CREDIT"]) == values2)
                            {
                                dbmissing.Rows[h].Delete();
                            }
                        }

                        //deleteing record from in paydirect not in bank statemeng
                        for (int h = 0; h < dbmissingpay.Rows.Count; h++)
                        {
                            if (dbmissingpay.Rows[h]["DATE"].ToString() == values3 && Convert.ToDouble(dbmissingpay.Rows[h]["CREDIT"]) == values4)
                            {
                                dbmissingpay.Rows[h].Delete();
                            }
                        }

                        dbmissing.AcceptChanges(); dbmissingpay.AcceptChanges();

                        Common.setMessageBox("Record Match Successfully", "Match Record", 1);
                        //reload the gridview of both table

                        if (dbmissing != null && dbmissing.Rows.Count > 0)
                        {
                            gridControl2.DataSource = dbmissing;
                            gridView3.OptionsBehavior.Editable = false;
                            gridView3.Columns["CREDIT"].DisplayFormat.FormatType = FormatType.Numeric;
                            gridView3.Columns["CREDIT"].DisplayFormat.FormatString = "n2";
                            gridView3.Columns["CREDIT"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                            gridView3.Columns["DATE"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
                            gridView3.Columns["DATE"].DisplayFormat.FormatString = "dd/MM/yyyy";
                            gridView3.Columns["CREDIT"].SummaryItem.FieldName = "CREDIT";
                            gridView3.Columns["CREDIT"].SummaryItem.DisplayFormat = "Total = {0:n}";
                            //gridView3.OptionsView.ColumnAutoWidth = false;
                            gridView3.OptionsView.ShowFooter = true;
                            gridView3.BestFitColumns();
                        }

                        if (dbmissingpay != null && dbmissingpay.Rows.Count > 0)
                        {
                            gridControl4.DataSource = dbmissingpay;
                            gridView5.OptionsBehavior.Editable = false;
                            gridView5.Columns["CREDIT"].DisplayFormat.FormatType = FormatType.Numeric;
                            gridView5.Columns["CREDIT"].DisplayFormat.FormatString = "n2";
                            gridView5.Columns["CREDIT"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                            gridView5.Columns["DATE"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
                            gridView5.Columns["DATE"].DisplayFormat.FormatString = "dd/MM/yyyy";
                            gridView5.Columns["CREDIT"].SummaryItem.FieldName = "CREDIT";
                            gridView5.Columns["CREDIT"].SummaryItem.DisplayFormat = "Total = {0:n}";
                            //gridView5.OptionsView.ColumnAutoWidth = false;
                            gridView5.OptionsView.ShowFooter = true;

                            gridView5.BestFitColumns();
                        }

                        if (dbmatched != null && dbmatched.Rows.Count > 0)
                        {
                            gridControl3.DataSource = dbmatched;
                            gridView4.OptionsBehavior.Editable = false;
                            gridView4.Columns["CREDIT"].DisplayFormat.FormatType = FormatType.Numeric;
                            gridView4.Columns["CREDIT"].DisplayFormat.FormatString = "n2";
                            gridView4.Columns["CREDIT"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                            gridView4.Columns["DATE"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
                            gridView4.Columns["DATE"].DisplayFormat.FormatString = "dd/MM/yyyy";
                            gridView4.Columns["CREDIT"].SummaryItem.FieldName = "CREDIT";
                            gridView4.Columns["CREDIT"].SummaryItem.DisplayFormat = "Total = {0:n}";
                            //gridView4.OptionsView.ColumnAutoWidth = false;
                            gridView4.OptionsView.ShowFooter = true;

                            gridView4.BestFitColumns();
                        }

                        //if (isFirstGrid)
                        //{
                        //    selection = new GridCheckMarksSelection(gridView3, ref lblSelect);
                        //    selection.CheckMarkColumn.VisibleIndex = 0;
                        //    isFirstGrid = false;
                        //}
                        //if (isFirstGrid2)
                        //{
                        //    selection2 = new GridCheckMarksSelection(gridView5, ref lblSelect2);
                        //    selection2.CheckMarkColumn.VisibleIndex = 0;
                        //    isFirstGrid2 = false;
                        //}
                        ////isFirstGrid = true; isFirstGrid2 = true;
                        //selection.ClearSelection(); selection2.ClearSelection();

                    }
                    else
                        return;

                }
                else
                {
                    Common.setMessageBox("Selected Record for Matching Not Equall in Value", "Match Record", 2); return;
                }

            }
            else
            {
                Common.setMessageBox("Only One Can Be Match at a time", "Match Record", 1); return;
            }

        }

        void bttncompare_Click(object sender, EventArgs e)
        {
            gridControl2.DataSource = null; gridControl4.DataSource = null; gridControl3.DataSource = null;
            gridView3.Columns.Clear(); gridView4.Columns.Clear(); gridView5.Columns.Clear();

            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

                if (string.IsNullOrEmpty((string)(cboBank.SelectedValue)))
                {
                    Common.setEmptyField("BanK Name", Program.ApplicationName);
                    cboBank.Focus();
                    return;
                }
                else if (string.IsNullOrEmpty((string)(cboAccount.SelectedValue.ToString())))
                {
                    Common.setEmptyField("Account Number", Program.ApplicationName); cboAccount.Focus(); return;
                }
                else
                {
                    getCompare();

                }

            }
            finally
            {
                SplashScreenManager.CloseForm(false);

            }
        }

        void radioGroup3_SelectedIndexChanged(object sender, EventArgs e)
        {
            colView.Name = "Description";
            colView.FieldName = "Description";

            colView2.Name = "Description";
            colView2.FieldName = "Description";

            setReloadDB();

            if (string.IsNullOrEmpty((string)cboBank.SelectedValue))
            {
                Common.setEmptyField("Bank", Program.ApplicationName);
                cboBank.Focus(); return;

            }
            else
            {
                try
                {
                    SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

                    if (radioGroup3.EditValue.ToString() == "New")
                    {
                        //call check to check if already assign before
                        string query = string.Format("SELECT COUNT(*) FROM tblAllocateCredit AC INNER JOIN tblAllocateDebit AD ON AC.BankCode = AD.BankCode WHERE AC.BankCode='{0}' AND CONVERT(VARCHAR(10),AC.StartDate,103)='{1}' AND CONVERT(VARCHAR(10),AC.EndDate,103)='{2}'", cboBank.SelectedValue, string.Format("{0:dd/MM/yyy}", dtpStart.Value), string.Format("{0:dd/MM/yyy}", dtpEnd.Value));

                        try
                        {
                            if (new Logic().IsRecordExist(query))
                            {
                                Common.setMessageBox("Transaction Already been Allocated,Choose Edit Mode to Edit Record", Program.ApplicationName, 1);
                                setReloadDebit(); setReloadCredit();
                                boolIsUpdate2 = true;
                                //radioGroup3.SelectedIndex = -1;
                                return;
                            }
                            else
                                setReloadDB(); setReloadCD();
                            boolIsUpdate2 = false;
                        }
                        catch (Exception ex)
                        {
                            Tripous.Sys.ErrorBox(ex); return;
                        }
                    }
                    if (radioGroup3.EditValue.ToString() == "Edit")
                    {
                        setReloadDebit(); setReloadCredit();
                        boolIsUpdate2 = true;
                    }
                }
                finally
                {
                    SplashScreenManager.CloseForm(false);
                }
            }
        }

        void btnAllocate_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

            //    if (!boolIsUpdate2)
            //    {
            //        if (isClosed)
            //        {
            //            Common.setMessageBox("Operation Can't be Perform! Period Closed", Program.ApplicationName, 1); return;
            //        }
            //        else
            //        {
            //            try
            //            {
            //                using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
            //                {
            //                    dtc.Columns.Remove("PayerName");
            //                    connect.Open();
            //                    _command = new SqlCommand("AllocateTransaction", connect) { CommandType = CommandType.StoredProcedure };
            //                    _command.Parameters.Add(new SqlParameter("@BankCode", SqlDbType.VarChar)).Value = cboBank.SelectedValue;
            //                    _command.Parameters.Add(new SqlParameter("@startdate", SqlDbType.VarChar)).Value = string.Format("{0:yyyy/MM/dd}", dtpStart.Value);
            //                    _command.Parameters.Add(new SqlParameter("@enddate", SqlDbType.VarChar)).Value = string.Format("{0:yyyy/MM/dd}", dtpEnd.Value);
            //                    _command.Parameters.Add(new SqlParameter("@pType", SqlDbType.Bit)).Value = true;
            //                    _command.Parameters.Add(new SqlParameter("@UserID", SqlDbType.Bit)).Value = Program.UserID;
            //                    _command.Parameters.Add(new SqlParameter("@pTransactiondbCredit", SqlDbType.Structured)).Value = dtc;
            //                    _command.Parameters.Add(new SqlParameter("@pTransactiondbDebit", SqlDbType.Structured)).Value = dtde;
            //                    _command.Parameters.Add(new SqlParameter("@Periods", SqlDbType.Int)).Value = label22.Text;

            //                    using (System.Data.DataSet ds = new System.Data.DataSet())
            //                    {
            //                        ds.Clear();
            //                        adp = new SqlDataAdapter(_command);
            //                        adp.Fill(ds);
            //                        connect.Close();
            //                        //Dts = ds.Tables[0];

            //                        if (ds.Tables[0].Rows[0]["returnCode"].ToString() != "00")
            //                        {
            //                            Common.setMessageBox(ds.Tables[0].Rows[0]["returnMessage"].ToString(), Program.ApplicationName, 3);
            //                        }
            //                        else
            //                        {
            //                            Common.setMessageBox(ds.Tables[0].Rows[0]["returnMessage"].ToString(), Program.ApplicationName, 1);
            //                        }


            //                    }
            //                }
            //            }
            //            catch (Exception ex)
            //            {
            //                Tripous.Sys.ErrorBox(String.Format("{0}{1}Allocating", ex.StackTrace, ex.Message)); return;
            //            }

            //            #region Old Code
            //            ////credit transaction
            //            //try
            //            //{
            //            //    using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
            //            //    {
            //            //        dtc.Columns.Remove("PayerName");
            //            //        connect.Open();
            //            //        _command = new SqlCommand("AllocateTransaction", connect) { CommandType = CommandType.StoredProcedure };
            //            //        _command.Parameters.Add(new SqlParameter("@BankCode", SqlDbType.VarChar)).Value = cboBank.SelectedValue;
            //            //        _command.Parameters.Add(new SqlParameter("@startdate", SqlDbType.VarChar)).Value = string.Format("{0:yyyy/MM/dd}", dtpStart.Value);
            //            //        _command.Parameters.Add(new SqlParameter("@enddate", SqlDbType.VarChar)).Value = string.Format("{0:yyyy/MM/dd}", dtpEnd.Value);
            //            //        _command.Parameters.Add(new SqlParameter("@pType", SqlDbType.Bit)).Value = true;
            //            //        _command.Parameters.Add(new SqlParameter("@UserID", SqlDbType.Bit)).Value = Program.UserID;
            //            //        _command.Parameters.Add(new SqlParameter("@pTransactiondb", SqlDbType.Structured)).Value = dtc;
            //            //        _command.Parameters.Add(new SqlParameter("@Periods", SqlDbType.Int)).Value = label22.Text;

            //            //        using (System.Data.DataSet ds = new System.Data.DataSet())
            //            //        {
            //            //            ds.Clear();
            //            //            adp = new SqlDataAdapter(_command);
            //            //            adp.Fill(ds);
            //            //            //Dts = ds.Tables[0];
            //            //            connect.Close();
            //            //        }
            //            //    }
            //            //}
            //            //catch (Exception ex)
            //            //{
            //            //    Tripous.Sys.ErrorBox(String.Format("{0}{1}Allocating", ex.StackTrace, ex.Message)); return;
            //            //}
            //            ////debit transaction
            //            //try
            //            //{
            //            //    dtde.Columns.Remove("PayerName");
            //            //    using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
            //            //    {
            //            //        connect.Open();
            //            //        _command = new SqlCommand("AllocateTransaction", connect) { CommandType = CommandType.StoredProcedure };
            //            //        _command.Parameters.Add(new SqlParameter("@BankCode", SqlDbType.VarChar)).Value = cboBank.SelectedValue;
            //            //        _command.Parameters.Add(new SqlParameter("@startdate", SqlDbType.VarChar)).Value = string.Format("{0:yyyy/MM/dd}", dtpStart.Value);
            //            //        _command.Parameters.Add(new SqlParameter("@enddate", SqlDbType.VarChar)).Value = string.Format("{0:yyyy/MM/dd}", dtpEnd.Value);
            //            //        _command.Parameters.Add(new SqlParameter("@pType", SqlDbType.Bit)).Value = false;
            //            //        _command.Parameters.Add(new SqlParameter("@UserID", SqlDbType.Bit)).Value = Program.UserID;
            //            //        _command.Parameters.Add(new SqlParameter("@pTransactiondb", SqlDbType.Structured)).Value = dtde;
            //            //        _command.Parameters.Add(new SqlParameter("@Periods", SqlDbType.Int)).Value = label22.Text;

            //            //        using (System.Data.DataSet ds = new System.Data.DataSet())
            //            //        {
            //            //            ds.Clear();
            //            //            adp = new SqlDataAdapter(_command);
            //            //            adp.Fill(ds);
            //            //            //Dts = ds.Tables[0];
            //            //            connect.Close();

            //            //            if (ds.Tables[0].Rows[0]["returnCode"].ToString() != "00")
            //            //            {
            //            //                Common.setMessageBox(ds.Tables[0].Rows[0]["returnMessage"].ToString(), Program.ApplicationName, 3);
            //            //            }
            //            //            else
            //            //            {
            //            //                Common.setMessageBox(ds.Tables[0].Rows[0]["returnMessage"].ToString(), Program.ApplicationName, 1);
            //            //            }
            //            //        }
            //            //    }
            //            //}
            //            //catch (Exception ex)
            //            //{
            //            //    Tripous.Sys.ErrorBox(String.Format("{0}{1}Allocating", ex.StackTrace, ex.Message)); return;
            //            //} 
            //            #endregion
            //        }


            //    }
            //    else
            //    {
            //        Common.setMessageBox("Operation not allowed", Program.ApplicationName, 2); return;
            //        #region Old Code
            //        //if (isClosed)
            //        //{
            //        //    Common.setMessageBox("Operation Can't be Perform! Period Closed", Program.ApplicationName, 1); return;
            //        //}
            //        //else
            //        //{
            //        //    //credit transaction
            //        //    try
            //        //    {
            //        //        dbcredit.Columns.Remove("PayerName");
            //        //        using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
            //        //        {
            //        //            connect.Open();
            //        //            _command = new SqlCommand("AllocateTransaction", connect) { CommandType = CommandType.StoredProcedure };
            //        //            _command.Parameters.Add(new SqlParameter("@BankCode", SqlDbType.VarChar)).Value = cboBank.SelectedValue;
            //        //            _command.Parameters.Add(new SqlParameter("@startdate", SqlDbType.VarChar)).Value = string.Format("{0:yyyy/MM/dd}", dtpStart.Value);
            //        //            _command.Parameters.Add(new SqlParameter("@enddate", SqlDbType.VarChar)).Value = string.Format("{0:yyyy/MM/dd}", dtpEnd.Value);
            //        //            _command.Parameters.Add(new SqlParameter("@pType", SqlDbType.Bit)).Value = true;
            //        //            _command.Parameters.Add(new SqlParameter("@UserID", SqlDbType.Bit)).Value = Program.UserID;
            //        //            _command.Parameters.Add(new SqlParameter("@pTransactiondb", SqlDbType.Structured)).Value = dbcredit;
            //        //            _command.Parameters.Add(new SqlParameter("@Periods", SqlDbType.Int)).Value = label22.Text;

            //        //            using (System.Data.DataSet ds = new System.Data.DataSet())
            //        //            {
            //        //                adp = new SqlDataAdapter(_command);
            //        //                adp.Fill(ds);
            //        //                //Dts = ds.Tables[0];
            //        //                connect.Close();
            //        //            }
            //        //        }
            //        //    }
            //        //    catch (Exception ex)
            //        //    {
            //        //        Tripous.Sys.ErrorBox(String.Format("{0}{1}Allocating", ex.StackTrace, ex.Message)); return;
            //        //    }
            //        //    //debit transaction
            //        //    try
            //        //    {
            //        //        dbDebit.Columns.Remove("PayerName");
            //        //        using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
            //        //        {
            //        //            connect.Open();
            //        //            _command = new SqlCommand("AllocateTransaction", connect) { CommandType = CommandType.StoredProcedure };
            //        //            _command.Parameters.Add(new SqlParameter("@BankCode", SqlDbType.VarChar)).Value = cboBank.SelectedValue;
            //        //            _command.Parameters.Add(new SqlParameter("@startdate", SqlDbType.VarChar)).Value = string.Format("{0:yyyy/MM/dd}", dtpStart.Value);
            //        //            _command.Parameters.Add(new SqlParameter("@enddate", SqlDbType.VarChar)).Value = string.Format("{0:yyyy/MM/dd}", dtpEnd.Value);
            //        //            _command.Parameters.Add(new SqlParameter("@pType", SqlDbType.Bit)).Value = false;
            //        //            _command.Parameters.Add(new SqlParameter("@UserID", SqlDbType.Bit)).Value = Program.UserID;
            //        //            _command.Parameters.Add(new SqlParameter("@pTransactiondb", SqlDbType.Structured)).Value = dbDebit;
            //        //            _command.Parameters.Add(new SqlParameter("@Periods", SqlDbType.Int)).Value = label22.Text;

            //        //            using (System.Data.DataSet ds = new System.Data.DataSet())
            //        //            {
            //        //                adp = new SqlDataAdapter(_command);
            //        //                adp.Fill(ds);
            //        //                //Dts = ds.Tables[0];
            //        //                connect.Close();

            //        //                if (ds.Tables[0].Rows[0]["returnCode"].ToString() != "00")
            //        //                {
            //        //                    Common.setMessageBox(ds.Tables[0].Rows[0]["returnMessage"].ToString(), Program.ApplicationName, 3);
            //        //                }
            //        //                else
            //        //                {
            //        //                    Common.setMessageBox(ds.Tables[0].Rows[0]["returnMessage"].ToString(), Program.ApplicationName, 1);
            //        //                }
            //        //            }
            //        //        }
            //        //    }
            //        //    catch (Exception ex)
            //        //    {
            //        //        Tripous.Sys.ErrorBox(String.Format("{0}{1}Allocating", ex.StackTrace, ex.Message)); return;
            //        //    }

            //        //} 
            //        #endregion
            //    }
            //}
            //finally
            //{
            //    SplashScreenManager.CloseForm(false);

            //}
        }

        bool SaveBankAllocation()
        {
            if (isClosed)
            {
                Common.setMessageBox("Operation Can't be Perform! Period Closed", Program.ApplicationName, 1); return false;
            }
            else
            {
                try
                {
                    if (!CheckGridView(gridView8) || !CheckGridView(gridView9))
                    {
                        Common.setMessageBox("Some Transaction are not Allocated Yet", "Transaction Allocation", 1);
                        return false;
                    }

                    if (boolIsUpdate2)
                    {
                        dtc = dbcredit;
                        dtde = dbDebit;
                    }

                    using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
                    {
                        if (dtc.Columns.Contains("PayerName")) dtc.Columns.Remove("PayerName");
                        if (dtde.Columns.Contains("PayerName")) dtde.Columns.Remove("PayerName");
                        connect.Open();
                        _command = new SqlCommand("AllocateTransaction", connect) { CommandType = CommandType.StoredProcedure };
                        //_command.Parameters.Add(new SqlParameter("@BankCode", SqlDbType.VarChar)).Value = cboBank.SelectedValue;
                        //_command.Parameters.Add(new SqlParameter("@startdate", SqlDbType.VarChar)).Value = string.Format("{0:yyyy/MM/dd}", dtpStart.Value);
                        //_command.Parameters.Add(new SqlParameter("@enddate", SqlDbType.VarChar)).Value = string.Format("{0:yyyy/MM/dd}", dtpEnd.Value);
                        //_command.Parameters.Add(new SqlParameter("@pType", SqlDbType.Bit)).Value = true;
                        _command.Parameters.Add(new SqlParameter("@UserID", SqlDbType.VarChar)).Value = Program.UserID;
                        _command.Parameters.Add(new SqlParameter("@pTransactiondbCredit", SqlDbType.Structured)).Value = dtc;
                        _command.Parameters.Add(new SqlParameter("@pTransactiondbDebit", SqlDbType.Structured)).Value = dtde;
                        //_command.Parameters.Add(new SqlParameter("@Periods", SqlDbType.Int)).Value = Convert.ToInt32(label22.Text);
                        _command.Parameters.Add(new SqlParameter("@RecondID", SqlDbType.Int)).Value = Convert.ToInt32(label25.Text);

                        using (System.Data.DataSet ds = new System.Data.DataSet())
                        {
                            ds.Clear();
                            adp = new SqlDataAdapter(_command);
                            adp.Fill(ds);
                            connect.Close();
                            //Dts = ds.Tables[0];

                            if (ds.Tables[0].Rows[0]["returnCode"].ToString() != "00")
                            {
                                Common.setMessageBox(ds.Tables[0].Rows[0]["returnMessage"].ToString(), Program.ApplicationName, 3);
                                return false;
                            }
                            else
                            {
                                Common.setMessageBox(ds.Tables[0].Rows[0]["returnMessage"].ToString(), Program.ApplicationName, 1);
                                return true;
                            }

                        }
                    }
                }
                catch (Exception ex)
                {
                    Tripous.Sys.ErrorBox(String.Format("{0}{1}Allocating", ex.StackTrace, ex.Message)); return false;
                }
            }
        }

        bool CheckGridView(GridView view)
        {
#if false
            if (view == null || view.RowCount <= 0) return false;
            int errCount = 0;
            var col = view.Columns["Description"];
            for (int i = 0; i < view.RowCount; i++)
            {
                var value = view.GetRowCellValue(i, col);
                if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
                {
                    view.SetColumnError(col, "This is required", DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical);
                    ++errCount;
                }
            }
            return errCount == 0;
#else
            if (view == null || view.RowCount <= 0) return true;
            int errCount = 0;
            var dt = (view.GridControl).DataSource as DataTable;

            if (dt == null || dt.Rows.Count <= 0)
            {
                Common.setMessageBox("Error retrieving DataTable", Program.ApplicationName, 2);
                return false;
            }

            if (boolIsUpdate2)
            {
                var col = dt.Columns["TransID"];

                foreach (DataRow row in dt.Rows)
                {
                    row.ClearErrors();

                    var value = row["TransID"];

                    if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
                    {
                        row.SetColumnError(col, "This is required");
                        ++errCount;
                    }
                }
            }
            else
            {
                var col = dt.Columns["Description"];

                foreach (DataRow row in dt.Rows)
                {
                    row.ClearErrors();
                    var value = row["Description"];
                    if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
                    {
                        row.SetColumnError(col, "This is required");
                        ++errCount;
                    }
                }

            }

            return errCount == 0;
#endif
        }

        void bttnUpdateExcel_Click(object sender, EventArgs e)
        {
            //if (string.IsNullOrEmpty((string)(cboBank.SelectedValue)))
            //{
            //    Common.setEmptyField("BanK Name", Program.ApplicationName);
            //    cboBank.Focus();
            //    return;
            //}
            //else if (string.IsNullOrEmpty((string)(cboAccount.SelectedValue.ToString())))
            //{
            //    Common.setEmptyField("Account Number", Program.ApplicationName); cboAccount.Focus(); return;
            //}
            //else if (Dt == null)
            //{
            //    Common.setMessageBox("Please Upload Bank Statement", Program.ApplicationName, 2); return;
            //}
            //else
            //{
            //    DialogResult results = MessageBox.Show("Is Total Debit and Credit Import Correct ?", Program.ApplicationName, MessageBoxButtons.YesNo);

            //    if (results == DialogResult.Yes)
            //    {
            //        if (isClosed)
            //        {
            //            Common.setMessageBox("Operation Can't be Perform! Period Closed", Program.ApplicationName, 1); return;
            //        }
            //        else
            //        {
            //            try
            //            {
            //                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

            //                //string qry = (String.Format("SELECT BSDate AS DATE, ISNULL(Debit, 0) AS DEBIT,ISNULL(Credit, 0) AS CREDIT,Balance AS BALANCE ,RevenueCode ,PayerName  from Reconciliation.tblBankStatement WHERE BankShortCode = '{0}'  AND FinancialperiodID ='{1}' AND CONVERT(VARCHAR(10),StartDate,103)='{2}' AND CONVERT(VARCHAR(10),EndDate,103)='{3}' AND BankAccountID='{4}'", cboBank.SelectedValue, label22.Text.Trim(), string.Format("{0:dd/MM/yyy}", dtpStart.Value), string.Format("{0:dd/MM/yyy}", dtpEnd.Value), Convert.ToInt32(cboAccount.SelectedValue)));

            //                //DataTable dts = (new Logic()).getSqlStatement(qry).Tables[0];

            //                //if (dts != null && dts.Rows.Count > 0)
            //                //{
            //                //    //calling frmcopmare

            //                //    DialogResult result = MessageBox.Show(string.Format("Bank Transactions Already exist for this period {0} and {1}. Are you sure to continue.....", string.Format("{0:dd/MM/yyy}", dtpStart.Value), string.Format("{0:dd/MM/yyy}", dtpEnd.Value)), Program.ApplicationName, MessageBoxButtons.YesNo);

            //                //    if (result == DialogResult.Yes)
            //                //    {
            //                //        //DialogResult = System.Windows.Forms.DialogResult.OK;

            //                //        using (FrmCompareValue compfrm = new FrmCompareValue(dts, Dt))
            //                //        {
            //                //            var res = compfrm.ShowDialog();

            //                //            if (res == System.Windows.Forms.DialogResult.OK)
            //                //            {
            //                //                UpdateBankstatement((string)cboBank.SelectedValue, compfrm.dtEqual);
            //                //                bttnNextImp.Enabled = true;
            //                //            }
            //                //        }
            //                //    }
            //                //    else
            //                //        return;

            //                //}
            //                //else
            //                //{
            //                UpdateBankstatement((string)cboBank.SelectedValue, Dt);

            //                bttnNextImp.Enabled = true;
            //                //}

            //            }
            //            catch (Exception ex)
            //            {

            //                Common.setMessageBox(ex.StackTrace + ex.Message, "Error", 2);
            //                return;
            //            }
            //            finally
            //            {
            //                SplashScreenManager.CloseForm(false);

            //            }
            //        }

            //    }
            //    else
            //    {
            //        return;
            //    }

            //}
        }

        bool SaveExcel()
        {
            if (string.IsNullOrEmpty((string)(cboBank.SelectedValue)))
            {
                Common.setEmptyField("BanK Name", Program.ApplicationName);
                cboBank.Focus();
                return false;
            }
            else if (string.IsNullOrEmpty((string)(cboAccount.SelectedValue.ToString())))
            {
                Common.setEmptyField("Account Number", Program.ApplicationName); cboAccount.Focus(); return false;
            }
            else if (Dt == null)
            {
                Common.setMessageBox("Please Upload Bank Statement", Program.ApplicationName, 2); return false;
            }
            else
            {
                DialogResult results = MessageBox.Show("Is Total Debit and Credit Import Correct ?", Program.ApplicationName, MessageBoxButtons.YesNo);

                if (results == DialogResult.Yes)
                {
                    if (isClosed)
                    {
                        Common.setMessageBox("Operation Can't be Perform! Period Closed", Program.ApplicationName, 1); return false;
                    }
                    else
                    {
                        try
                        {
                            SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

                            UpdateBankstatement((string)cboBank.SelectedValue, Dt);

                            //bttnNextImp.Enabled = true;
                            return true;
                        }
                        catch (Exception ex)
                        {

                            Common.setMessageBox(ex.StackTrace + ex.Message, "Error", 2);
                            return false;
                        }
                        finally
                        {
                            SplashScreenManager.CloseForm(false);

                        }
                    }

                }
                else
                {
                    return false;
                }

            }
        }

        void bttnImport_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty((string)(cboBank.SelectedValue)))
            {
                Common.setEmptyField("BanK Name", Program.ApplicationName);
                cboBank.Focus();
                return;
            }
            else if (string.IsNullOrEmpty((string)(cboAccount.SelectedValue.ToString())))
            {
                Common.setEmptyField("Account Number", Program.ApplicationName);
                cboAccount.Focus(); return;
            }
            else
            {

                decimal openingBalance = 0m;
                //new import code here
                try
                {
                    using (OpenFileDialog openFileDialogCSV = new OpenFileDialog() { InitialDirectory = Application.ExecutablePath, Filter = "Excel Sheet(*.xlsx)|*.xlsx|Excel Sheet(*.xls)|*.xls|All Files(*.*)|*.*", FilterIndex = 1, RestoreDirectory = true })
                    {
                        //openFileDialogCSV.ShowDialog();
                        if (openFileDialogCSV.ShowDialog() == DialogResult.OK)
                        {

                            if (openFileDialogCSV.FileName.Length > 0)
                            {
                                filenamesopen = openFileDialogCSV.FileName;

                            }

                            try
                            {
                                Dt = new DataTable();
                                Dt.Clear();
                                Dt.BeginInit();
                                Dt.Columns.Add("DATE", typeof(DateTime));
                                Dt.Columns.Add("DEBIT", typeof(decimal));
                                Dt.Columns.Add("CREDIT", typeof(decimal));
                                Dt.Columns.Add("BALANCE", typeof(decimal));
                                Dt.Columns.Add("TELLERNO", typeof(Int32));
                                Dt.Columns.Add("REVENUECODE", typeof(string));
                                Dt.Columns.Add("PAYERNAME", typeof(string));
                                Dt.EndInit();


                                string connString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties='Excel 12.0;HDR=yes'", filenamesopen);

                                MyConnection = new OleDbConnection(connString);

                                MyCommand = new System.Data.OleDb.OleDbDataAdapter("select * from [Sheet1$]", MyConnection);
                                DtSet = new System.Data.DataSet();
                                DtSet.Clear();
                                MyCommand.Fill(DtSet, "[Sheet1$]");

                                var firstRow = DtSet.Tables[0].Rows[0];
                                var lastRow = DtSet.Tables[0].Rows[DtSet.Tables[0].Rows.Count - 1];
                                DateTime lastdate; DateTime firstdate;
                                if (string.IsNullOrEmpty(lastRow["DATE"].ToString()))
                                {
                                    Common.setMessageBox("Last row in excel import is empty", "Import Data Message", 1); ; return;
                                }
                                else
                                {
                                    lastdate = Convert.ToDateTime(lastRow["DATE"]);
                                    firstdate = Convert.ToDateTime(firstRow["DATE"]);
                                    openingBalance = Convert.ToDecimal(firstRow["BALANCE"]);
                                }
                                DtSet.Tables[0].Rows.Remove(firstRow);

                                //check date range
                                if (firstdate != dtpStart.Value && lastdate != dtpEnd.Value)
                                {
                                    Common.setMessageBox("The period date does not match the start date and end date of captured excel", "Import Data Message", 1);
                                    return;
                                }

                                int j = 0;

                                foreach (DataRow row in DtSet.Tables[0].Rows)
                                {
                                    j = j + 1;
                                    //DataRow rw = new DataRow();
                                    if (!(row["DATE"] is DBNull) && (row["DATE"] != ""))
                                    {
                                        var rw = Dt.NewRow();
                                        rw["DATE"] = row["DATE"];

                                        if (Convert.ToDateTime(row["DATE"]) >= dtpStart.Value.Date && Convert.ToDateTime(row["DATE"]) <= dtpEnd.Value.Date)
                                        {
                                            if (!(row["DEBIT"] is DBNull))
                                            {
                                                if (!Logic.isDeceimalFormat((string)row["DEBIT"].ToString()))
                                                {
                                                    Common.setMessageBox(string.Format("Incorrect Data format encountered. Correct {0} in Line No. {1} and try again.", row["DEBIT"], j), "Import Data Error", 3); return;
                                                }
                                                else
                                                {
                                                    rw["DEBIT"] = row["DEBIT"] is DBNull ? 0m : row["DEBIT"] == string.Empty ? 0m : Convert.ToDecimal(row["DEBIT"]);

                                                    rw["BALANCE"] = row["BALANCE"] is DBNull ? 0m : Convert.ToDecimal(row["BALANCE"]);
                                                    rw["CREDIT"] = row["CREDIT"] is DBNull ? 0m : Convert.ToDecimal(row["CREDIT"]);
                                                    rw["REVENUECODE"] = row["REVENUECODE"];

                                                    rw["PAYERNAME"] = row["PAYERNAME"]; rw["TELLERNO"] = row["TELLERNO"];
                                                }


                                            }
                                            else if (!(row["CREDIT"] is DBNull))
                                            {
                                                if (!Logic.isDeceimalFormat((string)row["CREDIT"].ToString()))
                                                {
                                                    Common.setMessageBox(string.Format("Incorrect Data format encountered. Correct {0} in Line No. {1} and try again.", row["CREDIT"], j), "Import Data Error", 3); return;
                                                }
                                                else
                                                {
                                                    rw["CREDIT"] = row["CREDIT"] is DBNull ? 0m : Convert.ToDecimal(row["CREDIT"]);
                                                    rw["DEBIT"] = row["DEBIT"] is DBNull ? 0m : row["DEBIT"] == string.Empty ? 0m : Convert.ToDecimal(row["DEBIT"]);

                                                    rw["BALANCE"] = row["BALANCE"] is DBNull ? 0m : Convert.ToDecimal(row["BALANCE"]);
                                                    rw["REVENUECODE"] = row["REVENUECODE"];

                                                    rw["PAYERNAME"] = row["PAYERNAME"]; rw["TELLERNO"] = row["TELLERNO"];
                                                }
                                            }
                                            else
                                            {
                                                rw["CREDIT"] = row["CREDIT"] is DBNull ? 0m : Convert.ToDecimal(row["CREDIT"]);
                                                rw["DEBIT"] = row["DEBIT"] is DBNull ? 0m : row["DEBIT"] == string.Empty ? 0m : Convert.ToDecimal(row["DEBIT"]);

                                                rw["BALANCE"] = row["BALANCE"] is DBNull ? 0m : Convert.ToDecimal(row["BALANCE"]);
                                                rw["REVENUECODE"] = row["REVENUECODE"];

                                                rw["PAYERNAME"] = row["PAYERNAME"]; rw["TELLERNO"] = row["TELLERNO"];
                                            }

                                            Dt.Rows.Add(rw);
                                        }
                                        else
                                        {
                                            Common.setMessageBox("Some transaction date are outside the specified date range", "Error During Import", 2); return;
                                        }

                                    }

                                }

                                for (int h = 0; h < Dt.Rows.Count; h++)
                                {
                                    if (Dt.Rows[h].IsNull(0) == true)
                                    {
                                        Dt.Rows[h].Delete();
                                    }
                                }

                                Dt.AcceptChanges();
                                //SearchPrevisonRecord();

                                gridControl1.DataSource = Dt;
                                gridView1.OptionsBehavior.Editable = true;
                            }
                            catch (Exception ex)
                            {
                                gridControl1.DataSource = null;
                                Tripous.Sys.ErrorBox(String.Format("{0}{1} ...Import Data Error", ex.Message, ex.StackTrace));

                                return;
                            }

                            //MyConnection.Close();

                            //ChangeValue(Dt);

                            AddComboboxRevenue();
                            //gridView1.BestFitColumns();
                            gridView1.Columns["DEBIT"].DisplayFormat.FormatType = FormatType.Numeric;
                            gridView1.Columns["DEBIT"].DisplayFormat.FormatString = "n2";
                            gridView1.Columns["CREDIT"].DisplayFormat.FormatType = FormatType.Numeric;
                            gridView1.Columns["CREDIT"].DisplayFormat.FormatString = "n2";
                            gridView1.Columns["BALANCE"].DisplayFormat.FormatType = FormatType.Numeric;
                            gridView1.Columns["BALANCE"].DisplayFormat.FormatString = "n2";

                            gridView1.Columns["CREDIT"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                            gridView1.Columns["DEBIT"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                            gridView1.Columns["BALANCE"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;

                            gridView1.Columns["BALANCE"].SummaryItem.FieldName = "BALANCE";
                            gridView1.Columns["BALANCE"].SummaryItem.DisplayFormat = "Total = {0:n}";

                            gridView1.Columns["CREDIT"].SummaryItem.FieldName = "CREDIT";
                            gridView1.Columns["CREDIT"].SummaryItem.DisplayFormat = "Total = {0:n}";

                            gridView1.Columns["DEBIT"].SummaryItem.FieldName = "DEBIT";
                            gridView1.Columns["DEBIT"].SummaryItem.DisplayFormat = "Total = {0:n}";

                            colRevenue = gridView1.Columns["REVENUECODE"];
                            colRevenue.ColumnEdit = repComboboxRevenue;

                            colRevenue.Visible = true;

                            //gridView1.OptionsView.ColumnAutoWidth = false;
                            gridView1.OptionsView.ShowFooter = true;
                            gridView1.Columns["REVENUECODE"].OptionsColumn.AllowEdit = true; gridView1.Columns["DEBIT"].OptionsColumn.AllowEdit = false;
                            gridView1.Columns["CREDIT"].OptionsColumn.AllowEdit = false; gridView1.Columns["BALANCE"].OptionsColumn.AllowEdit = false;
                            gridView1.Columns["PAYERNAME"].OptionsColumn.AllowEdit = false;
                            gridView1.Columns["REVENUECODE"].Caption = "REVENUENAME";
                            gridView1.BestFitColumns();

                            label2.Text = Dt.Rows.Count + " Rows of Records ";
                            isExcelAltered = true;
                        }
                        else
                        {
                            Common.setMessageBox("Operation Cancel", "Import Cancel", 3); return;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Common.setMessageBox(ex.StackTrace + ex.Message, "Error During Import", 2); return;
                }
                finally
                {
                    SplashScreenManager.CloseForm(false);
                }

                lblBankStatementOpeningBalance.Text = string.Format("{0:n2}", openingBalance);

                if (Math.Round(openingBalance, 2) != Math.Round(Convert.ToDecimal(label14.Text), 2))
                {
                    Common.setMessageBox("Bank Statement Opening Balance deos not tally with System Inherited Balance", "Import Error", 3);
                    gridControl1.Enabled = false;
                    bttnNextImp.Enabled = false;
                    return;
                }

                gridControl1.Enabled = true;
                bttnNextImp.Enabled = true;
                bttnUpdateExcel.Enabled = false;
            }

            //DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit xSubmit = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            //xSubmit.Buttons[0].Kind = ButtonPredefines.Glyph;
            //xSubmit.Buttons[0].Caption = "Submit";



            ////this.gridView1.Columns.Add(xSubmit);

            //cboBank.Enabled = false;
            //MessageBoxManager.Unregister();
            ////MessageBoxManager.OK = "Excel 2003";
            //MessageBoxManager.No = "Excel 2007";
            //MessageBoxManager.Yes = "Excel 2003";
            //MessageBoxManager.Register();

            //DialogResult result = MessageBox.Show("Select Excel File Type", "Import Statement", MessageBoxButtons.YesNo, MessageBoxIcon.Question);


            ////decimal openingBalance = 0m;

            //if (result == DialogResult.Yes)//excele 2003
            //{
            //    try
            //    {
            //        SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

            //        gridControl1.DataSource = null;

            //        using (OpenFileDialog openFileDialogCSV = new OpenFileDialog() { InitialDirectory = Application.ExecutablePath, Filter = "Excel files (*.xls)|*.xls|All files (*.*)|*.*", FilterIndex = 1, RestoreDirectory = true })
            //        {

            //            //openFileDialogCSV.ShowDialog();
            //            if (openFileDialogCSV.ShowDialog() == DialogResult.OK)
            //            {

            //                if (openFileDialogCSV.FileName.Length > 0)
            //                {
            //                    filenamesopen = openFileDialogCSV.FileName;
            //                }

            //                try
            //                {
            //                    Dt = new DataTable();
            //                    Dt.Clear();
            //                    Dt.BeginInit();
            //                    Dt.Columns.Add("DATE", typeof(DateTime));
            //                    Dt.Columns.Add("DEBIT", typeof(decimal));
            //                    Dt.Columns.Add("CREDIT", typeof(decimal));
            //                    Dt.Columns.Add("BALANCE", typeof(decimal));
            //                    Dt.Columns.Add("REVENUECODE", typeof(string));
            //                    Dt.Columns.Add("PAYERNAME", typeof(string));
            //                    Dt.EndInit();

            //                    //MyConnection = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" +
            //                    //filenamesopen + ";Extended Properties=\"Excel 8.0;HDR=YES;\"");

            //                    string connString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties='Excel 12.0;HDR=yes'", filenamesopen);

            //                    //MyConnection = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" +
            //                    //               filenamesopen + ";Extended Properties=\"Excel 8.0;HDR=YES;\"");
            //                    MyConnection = new OleDbConnection(connString);

            //                    MyCommand = new System.Data.OleDb.OleDbDataAdapter("select * from [Sheet1$]", MyConnection);
            //                    DtSet = new System.Data.DataSet();
            //                    DtSet.Clear();
            //                    MyCommand.Fill(DtSet, "[Sheet1$]");

            //                    var firstRow = DtSet.Tables[0].Rows[0];
            //                    openingBalance = Convert.ToDecimal(firstRow["BALANCE"]);
            //                    DtSet.Tables[0].Rows.Remove(firstRow);

            //                    int j = 0;
            //                    foreach (DataRow row in DtSet.Tables[0].Rows)
            //                    {
            //                        j = j + 1;
            //                        //DataRow rw = new DataRow();
            //                        if (!(row["DATE"] is DBNull) && (row["DATE"] != ""))
            //                        {
            //                            var rw = Dt.NewRow();
            //                            rw["DATE"] = row["DATE"];

            //                            if (Convert.ToDateTime(row["DATE"]) >= dtpStart.Value.Date && Convert.ToDateTime(row["DATE"]) <= dtpEnd.Value.Date)
            //                            {
            //                                if (!(row["DEBIT"] is DBNull))
            //                                {
            //                                    if (!Logic.isDeceimalFormat((string)row["DEBIT"].ToString()))
            //                                    {
            //                                        Common.setMessageBox(string.Format("Incorrect Data format encountered. Correct {0} in Line No. {1} and try again.", row["DEBIT"], j), "Import Data Error", 3); return;
            //                                    }
            //                                    else
            //                                    {
            //                                        rw["DEBIT"] = row["DEBIT"] is DBNull ? 0m : row["DEBIT"] == string.Empty ? 0m : Convert.ToDecimal(row["DEBIT"]);

            //                                        rw["BALANCE"] = row["BALANCE"] is DBNull ? 0m : Convert.ToDecimal(row["BALANCE"]);
            //                                        rw["CREDIT"] = row["CREDIT"] is DBNull ? 0m : Convert.ToDecimal(row["CREDIT"]);
            //                                        rw["REVENUECODE"] = row["REVENUECODE"];

            //                                        rw["PAYERNAME"] = row["PAYERNAME"];
            //                                    }


            //                                }
            //                                else if (!(row["CREDIT"] is DBNull))
            //                                {


            //                                    if (!Logic.isDeceimalFormat((string)row["CREDIT"].ToString()))
            //                                    {
            //                                        //var rw = Dt.NewRow();

            //                                        Common.setMessageBox(string.Format("Incorrect Data format encountered. Correct {0} in Line No. {1} and try again.", row["CREDIT"], j), "Import Data Error", 3); return;
            //                                    }
            //                                    else
            //                                    {
            //                                        rw["CREDIT"] = row["CREDIT"] is DBNull ? 0m : Convert.ToDecimal(row["CREDIT"]);
            //                                        rw["DEBIT"] = row["DEBIT"] is DBNull ? 0m : row["DEBIT"] == string.Empty ? 0m : Convert.ToDecimal(row["DEBIT"]);

            //                                        rw["BALANCE"] = row["BALANCE"] is DBNull ? 0m : Convert.ToDecimal(row["BALANCE"]);

            //                                    }
            //                                }
            //                                else
            //                                {
            //                                    rw["CREDIT"] = row["CREDIT"] is DBNull ? 0m : Convert.ToDecimal(row["CREDIT"]);
            //                                    rw["DEBIT"] = row["DEBIT"] is DBNull ? 0m : row["DEBIT"] == string.Empty ? 0m : Convert.ToDecimal(row["DEBIT"]);

            //                                    rw["BALANCE"] = row["BALANCE"] is DBNull ? 0m : Convert.ToDecimal(row["BALANCE"]);
            //                                    rw["REVENUECODE"] = row["REVENUECODE"];

            //                                    rw["PAYERNAME"] = row["PAYERNAME"];
            //                                }

            //                                Dt.Rows.Add(rw);
            //                            }
            //                            else
            //                            {
            //                                Common.setMessageBox("Some transaction date are outside the specified date range", "Error During Import", 2); return;
            //                            }

            //                        }

            //                    }


            //                    for (int h = 0; h < Dt.Rows.Count; h++)
            //                    {
            //                        if (Dt.Rows[h].IsNull(0) == true)
            //                        {
            //                            Dt.Rows[h].Delete();
            //                        }
            //                    }

            //                    Dt.AcceptChanges();
            //                    //SearchPrevisonRecord();

            //                    gridControl1.DataSource = Dt;
            //                    gridView1.OptionsBehavior.Editable = true;
            //                    //MyConnection.Close();
            //                }
            //                catch (Exception ex)
            //                {
            //                    //Common.setMessageBox("Modify the excel to contain this Column Header 'DATE','DEBIT','CREDIT','BALANCE','REVENUECODE','PAYERNAME',", "Import Data Error", 3);

            //                    gridControl1.DataSource = null;

            //                    Tripous.Sys.ErrorBox(String.Format(" Modify the excel to contain this Column Header 'DATE','DEBIT','CREDIT','BALANCE','REVENUECODE','PAYERNAME'.... {0}{1} ...Import Data Error", ex.Message, ex.StackTrace));

            //                    return;


            //                }
            //                //ChangeValue(Dt);

            //                AddComboboxRevenue();
            //                //gridView1.BestFitColumns();
            //                gridView1.Columns["DEBIT"].DisplayFormat.FormatType = FormatType.Numeric;
            //                gridView1.Columns["DEBIT"].DisplayFormat.FormatString = "n2";
            //                gridView1.Columns["CREDIT"].DisplayFormat.FormatType = FormatType.Numeric;
            //                gridView1.Columns["CREDIT"].DisplayFormat.FormatString = "n2";
            //                gridView1.Columns["BALANCE"].DisplayFormat.FormatType = FormatType.Numeric;
            //                gridView1.Columns["BALANCE"].DisplayFormat.FormatString = "n2";

            //                gridView1.Columns["CREDIT"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            //                gridView1.Columns["DEBIT"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            //                gridView1.Columns["BALANCE"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;

            //                gridView1.Columns["BALANCE"].SummaryItem.FieldName = "BALANCE";
            //                gridView1.Columns["BALANCE"].SummaryItem.DisplayFormat = "Total = {0:n}";

            //                gridView1.Columns["CREDIT"].SummaryItem.FieldName = "CREDIT";
            //                gridView1.Columns["CREDIT"].SummaryItem.DisplayFormat = "Total = {0:n}";

            //                gridView1.Columns["DEBIT"].SummaryItem.FieldName = "DEBIT";
            //                gridView1.Columns["DEBIT"].SummaryItem.DisplayFormat = "Total = {0:n}";

            //                colRevenue = gridView1.Columns["REVENUECODE"];
            //                colRevenue.ColumnEdit = repComboboxRevenue;

            //                colRevenue.Visible = true;

            //                //gridView1.OptionsView.ColumnAutoWidth = false;
            //                gridView1.OptionsView.ShowFooter = true;
            //                gridView1.Columns["REVENUECODE"].OptionsColumn.AllowEdit = true; gridView1.Columns["DEBIT"].OptionsColumn.AllowEdit = false;
            //                gridView1.Columns["CREDIT"].OptionsColumn.AllowEdit = false; gridView1.Columns["BALANCE"].OptionsColumn.AllowEdit = false;
            //                gridView1.Columns["PAYERNAME"].OptionsColumn.AllowEdit = false;
            //                gridView1.Columns["REVENUECODE"].Caption = "REVENUENAME";
            //                gridView1.BestFitColumns();

            //                label2.Text = Dt.Rows.Count + " Rows of Records ";
            //            }
            //            else
            //            {
            //                Common.setMessageBox("Operation Cancel", "Import Cancel", 3); return;
            //            }


            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        Common.setMessageBox(ex.StackTrace + ex.Message, "Error During Import", 2); return;
            //    }
            //    finally
            //    {
            //        SplashScreenManager.CloseForm(false);
            //    }
            //}

            //if (result == DialogResult.No)//excele 2007
            //{
            //    //MessageBox.Show("You clicked the yes button");
            //    //new OpenFileDialog().ShowDialog();

            //    try
            //    {
            //        SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

            //        //exportToExcel();

            //        gridControl1.DataSource = null;

            //        using (OpenFileDialog openFileDialogCSV = new OpenFileDialog() { InitialDirectory = Application.ExecutablePath, Filter = "Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*", FilterIndex = 1, RestoreDirectory = true })
            //        {

            //            //openFileDialogCSV.ShowDialog();
            //            if (openFileDialogCSV.ShowDialog() == DialogResult.OK)
            //            {
            //                if (openFileDialogCSV.FileName.Length > 0)
            //                {
            //                    filenamesopen = openFileDialogCSV.FileName;
            //                }
            //                //try
            //                //{
            //                Dt = new DataTable();
            //                Dt.BeginInit();
            //                Dt.Columns.Add("DATE", typeof(DateTime));
            //                Dt.Columns.Add("DEBIT", typeof(decimal));
            //                Dt.Columns.Add("CREDIT", typeof(decimal));
            //                Dt.Columns.Add("BALANCE", typeof(decimal));
            //                Dt.Columns.Add("REVENUECODE", typeof(string));
            //                Dt.Columns.Add("PAYERNAME", typeof(string));
            //                Dt.EndInit();

            //                Dt.Clear();
            //                string connString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties='Excel 12.0;HDR=yes'", filenamesopen);

            //                //MyConnection = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" +
            //                //               filenamesopen + ";Extended Properties=\"Excel 8.0;HDR=YES;\"");
            //                MyConnection = new OleDbConnection(connString);

            //                MyCommand = new System.Data.OleDb.OleDbDataAdapter("select * from [Sheet1$]", MyConnection);
            //                DtSet = new System.Data.DataSet(); DtSet.Clear();
            //                MyCommand.Fill(DtSet, "[Sheet1$]");

            //                var firstRow = DtSet.Tables[0].Rows[0];
            //                openingBalance = Convert.ToDecimal(firstRow["BALANCE"]);
            //                DtSet.Tables[0].Rows.Remove(firstRow);

            //                int j = 0;
            //                foreach (DataRow row in DtSet.Tables[0].Rows)
            //                {
            //                    j = j + 1;
            //                    //DataRow rw = new DataRow();
            //                    if (!(row["DATE"] is DBNull) && (row["DATE"] != ""))
            //                    {
            //                        var rw = Dt.NewRow();
            //                        rw["DATE"] = row["DATE"];

            //                        //          DateTime dates;

            //                        //          DateTime.TryParseExact(row["DATE"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture,
            //                        //DateTimeStyles.None, out dates);

            //                        //if (dates >= dtpStart.Value.Date && dates <= dtpEnd.Value.Date)
            //                        if (Convert.ToDateTime(row["DATE"]) >= dtpStart.Value.Date && Convert.ToDateTime(row["DATE"]) <= dtpEnd.Value.Date)
            //                        {
            //                            if (!(row["DEBIT"] is DBNull))
            //                            {
            //                                if (!Logic.isDeceimalFormat((string)row["DEBIT"].ToString()))
            //                                //if ((Int32)row["DEBIT"])
            //                                {
            //                                    //var rw = Dt.NewRow();

            //                                    Common.setMessageBox(string.Format("Incorrect Data format encountered. Correct {0} in Line No. {1} and try again.", row["DEBIT"], j), "Import Data Error", 3); return;
            //                                }
            //                                else
            //                                {
            //                                    rw["DEBIT"] = row["DEBIT"] is DBNull ? 0m : row["DEBIT"] == string.Empty ? 0m : Convert.ToDecimal(row["DEBIT"]);

            //                                    rw["BALANCE"] = row["BALANCE"] is DBNull ? 0m : Convert.ToDecimal(row["BALANCE"]);
            //                                    rw["CREDIT"] = row["CREDIT"] is DBNull ? 0m : Convert.ToDecimal(row["CREDIT"]);

            //                                    rw["REVENUECODE"] = row["REVENUECODE"];

            //                                    rw["PAYERNAME"] = row["PAYERNAME"];
            //                                }
            //                            }
            //                            else if (!(row["CREDIT"] is DBNull))
            //                            {
            //                                if (!Logic.isDeceimalFormat((string)row["CREDIT"].ToString()))
            //                                {
            //                                    //var rw = Dt.NewRow();

            //                                    Common.setMessageBox(string.Format("Incorrect Data format encountered. Correct {0} in Line No. {1} and try again.", row["CREDIT"], j), "Import Data Error", 3); return;
            //                                }
            //                                else
            //                                {
            //                                    rw["CREDIT"] = row["CREDIT"] is DBNull ? 0m : Convert.ToDecimal(row["CREDIT"]);
            //                                    rw["DEBIT"] = row["DEBIT"] is DBNull ? 0m : row["DEBIT"] == string.Empty ? 0m : Convert.ToDecimal(row["DEBIT"]);

            //                                    rw["BALANCE"] = row["BALANCE"] is DBNull ? 0m : Convert.ToDecimal(row["BALANCE"]);
            //                                    rw["REVENUECODE"] = row["REVENUECODE"];

            //                                    rw["PAYERNAME"] = row["PAYERNAME"];

            //                                }
            //                            }
            //                            else
            //                            {
            //                                rw["CREDIT"] = row["CREDIT"] is DBNull ? 0m : Convert.ToDecimal(row["CREDIT"]);
            //                                rw["DEBIT"] = row["DEBIT"] is DBNull ? 0m : row["DEBIT"] == string.Empty ? 0m : Convert.ToDecimal(row["DEBIT"]);

            //                                rw["BALANCE"] = row["BALANCE"] is DBNull ? 0m : Convert.ToDecimal(row["BALANCE"]);

            //                                rw["REVENUECODE"] = row["REVENUECODE"];

            //                                rw["PAYERNAME"] = row["PAYERNAME"];
            //                            }

            //                            Dt.Rows.Add(rw);
            //                        }
            //                        else
            //                        {
            //                            Common.setMessageBox("Some transaction date are outside the specified date range", "Error During Import", 2); return;
            //                        }
            //                    }

            //                }

            //                for (int h = 0; h < Dt.Rows.Count; h++)
            //                {
            //                    if (Dt.Rows[h].IsNull(0) == true)
            //                    {
            //                        Dt.Rows[h].Delete();
            //                    }
            //                }

            //                Dt.AcceptChanges();
            //                //SearchPrevisonRecord();

            //                gridControl1.DataSource = Dt;
            //                gridView1.OptionsBehavior.Editable = true;

            //                AddComboboxRevenue();

            //                MyConnection.Close();

            //                gridView1.Columns["DEBIT"].DisplayFormat.FormatType = FormatType.Numeric;
            //                gridView1.Columns["DEBIT"].DisplayFormat.FormatString = "n2";
            //                gridView1.Columns["CREDIT"].DisplayFormat.FormatType = FormatType.Numeric;
            //                gridView1.Columns["CREDIT"].DisplayFormat.FormatString = "n2";
            //                gridView1.Columns["BALANCE"].DisplayFormat.FormatType = FormatType.Numeric;
            //                gridView1.Columns["BALANCE"].DisplayFormat.FormatString = "n2";
            //                gridView1.Columns["BALANCE"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;

            //                gridView1.Columns["BALANCE"].SummaryItem.FieldName = "BALANCE";
            //                gridView1.Columns["BALANCE"].SummaryItem.DisplayFormat = "Total = {0:n}";

            //                gridView1.Columns["CREDIT"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            //                gridView1.Columns["DEBIT"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;

            //                gridView1.Columns["CREDIT"].SummaryItem.FieldName = "CREDIT";
            //                gridView1.Columns["CREDIT"].SummaryItem.DisplayFormat = "Total = {0:n}";

            //                gridView1.Columns["DEBIT"].SummaryItem.FieldName = "DEBIT";
            //                gridView1.Columns["DEBIT"].SummaryItem.DisplayFormat = "Total = {0:n}";

            //                colRevenue = gridView1.Columns["REVENUECODE"];
            //                colRevenue.ColumnEdit = repComboboxRevenue;

            //                colRevenue.Visible = true;

            //                //gridView1.OptionsView.ColumnAutoWidth = false;
            //                gridView1.OptionsView.ShowFooter = true;
            //                gridView1.Columns["REVENUECODE"].OptionsColumn.AllowEdit = true; gridView1.Columns["DEBIT"].OptionsColumn.AllowEdit = false;
            //                gridView1.Columns["CREDIT"].OptionsColumn.AllowEdit = false; gridView1.Columns["BALANCE"].OptionsColumn.AllowEdit = false;
            //                gridView1.Columns["PAYERNAME"].OptionsColumn.AllowEdit = false; gridView1.Columns["REVENUECODE"].Caption = "REVENUENAME";

            //                gridView1.BestFitColumns();
            //                label2.Text = Dt.Rows.Count + " Rows of Records ";
            //                isExcelAltered = true;
            //            }
            //            else
            //            {
            //                Common.setMessageBox("Operation Cancel", "Import Cancel", 3); return;
            //            }
            //        }

            //        ////gridView.Columns["Save"].ColumnEdit = _editButtonEdit;
            //        //gridView1.Columns["DEBIT"].ColumnEdit = _editButtonEdit;
            //        //_editButtonEdit.ButtonsStyle = BorderStyles.NoBorder;
            //        //_editButtonEdit.Buttons[0].Kind = ButtonPredefines.OK;
            //        //_editButtonEdit.Buttons[0].Appearance.BackColor = Color.Gray;
            //        //gridControl1.RepositoryItems.Add(_editButtonEdit);


            //    }
            //    catch (Exception ex)
            //    {

            //        gridControl1.DataSource = null;

            //        Tripous.Sys.ErrorBox(String.Format(" Modify the excel to contain this Column Header 'DATE','DEBIT','CREDIT','BALANCE','REVENUECODE','PAYERNAME'.... {0}{1} ...Import Data Error", ex.Message, ex.StackTrace));

            //        return;
            //    }
            //    finally
            //    {
            //        SplashScreenManager.CloseForm(false);
            //    }
            //}

            //lblBankStatementOpeningBalance.Text = string.Format("{0:n2}", openingBalance);
            //if (Math.Round(openingBalance, 2) != Math.Round(Convert.ToDecimal(label14.Text), 2))
            //{
            //    Common.setMessageBox("Bank Statement Opening Balance deos not tally with System Inherited Balance", "Import Error", 3);
            //    gridControl1.Enabled = false;
            //    bttnNextImp.Enabled = false;
            //    return;
            //}


            //MessageBoxManager.Unregister();

            #region

            //if (this.radioGroup4.SelectedIndex == 0)//open excel file 2003
            //{
            //    //exportToExcel();

            //    try
            //    {
            //        SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

            //        gridControl1.DataSource = null;

            //        using (OpenFileDialog openFileDialogCSV = new OpenFileDialog() { InitialDirectory = Application.ExecutablePath, Filter = "Excel files (*.xls)|*.xls|All files (*.*)|*.*", FilterIndex = 1, RestoreDirectory = true })
            //        {

            //            //openFileDialogCSV.ShowDialog();
            //            if (openFileDialogCSV.ShowDialog() == DialogResult.OK)

            //                if (openFileDialogCSV.FileName.Length > 0)
            //                {
            //                    filenamesopen = openFileDialogCSV.FileName;
            //                }

            //            try
            //            {
            //                Dt = new DataTable();
            //                Dt.Clear();
            //                Dt.BeginInit();
            //                Dt.Columns.Add("DATE", typeof(DateTime));
            //                Dt.Columns.Add("DEBIT", typeof(decimal));
            //                Dt.Columns.Add("CREDIT", typeof(decimal));
            //                Dt.Columns.Add("BALANCE", typeof(decimal));
            //                Dt.EndInit();

            //                MyConnection = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" +
            //                               filenamesopen + ";Extended Properties=\"Excel 8.0;HDR=YES;\"");

            //                MyCommand = new System.Data.OleDb.OleDbDataAdapter("select * from [Sheet1$]", MyConnection);
            //                DtSet = new System.Data.DataSet();
            //                DtSet.Clear();
            //                MyCommand.Fill(DtSet, "[Sheet1$]");
            //                //MyCommand.Fill(Dt);

            //                foreach (DataRow row in DtSet.Tables[0].Rows)
            //                {
            //                    //DataRow rw = new DataRow();
            //                    if (!(row["DATE"] is DBNull) && (row["DATE"] != ""))
            //                    {
            //                        var rw = Dt.NewRow();
            //                        rw["DATE"] = row["DATE"];
            //                        rw["DEBIT"] = row["DEBIT"] is DBNull ? 0m : row["DEBIT"] == string.Empty ? 0m : Convert.ToDecimal(row["DEBIT"]);
            //                        rw["CREDIT"] = row["CREDIT"] is DBNull ? 0m : Convert.ToDecimal(row["CREDIT"]);
            //                        rw["BALANCE"] = row["BALANCE"] is DBNull ? 0m : Convert.ToDecimal(row["BALANCE"]);
            //                        //if (rw["DATE"] != string.Empty)
            //                        Dt.Rows.Add(rw);
            //                    }
            //                }


            //                for (int h = 0; h < Dt.Rows.Count; h++)
            //                {
            //                    if (Dt.Rows[h].IsNull(0) == true)
            //                    {
            //                        Dt.Rows[h].Delete();
            //                    }
            //                }

            //                Dt.AcceptChanges();
            //                //SearchPrevisonRecord();

            //                gridControl1.DataSource = Dt;
            //                gridView1.OptionsBehavior.Editable = false;
            //                MyConnection.Close();
            //            }
            //            catch (Exception ex)
            //            {
            //                Common.setMessageBox(ex.Message, Program.ApplicationName, 2);
            //                gridControl1.DataSource = null;
            //                return;
            //            }
            //            //ChangeValue(Dt);


            //            //gridView1.BestFitColumns();
            //            gridView1.Columns["DEBIT"].DisplayFormat.FormatType = FormatType.Numeric;
            //            gridView1.Columns["DEBIT"].DisplayFormat.FormatString = "n2";
            //            gridView1.Columns["CREDIT"].DisplayFormat.FormatType = FormatType.Numeric;
            //            gridView1.Columns["CREDIT"].DisplayFormat.FormatString = "n2";
            //            gridView1.Columns["BALANCE"].DisplayFormat.FormatType = FormatType.Numeric;
            //            gridView1.Columns["BALANCE"].DisplayFormat.FormatString = "n2";

            //            gridView1.Columns["CREDIT"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            //            gridView1.Columns["DEBIT"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            //            gridView1.Columns["BALANCE"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;

            //            gridView1.Columns["BALANCE"].SummaryItem.FieldName = "CREDIT";
            //            gridView1.Columns["BALANCE"].SummaryItem.DisplayFormat = "Total = {0:n}";

            //            gridView1.Columns["CREDIT"].SummaryItem.FieldName = "CREDIT";
            //            gridView1.Columns["CREDIT"].SummaryItem.DisplayFormat = "Total = {0:n}";

            //            gridView1.Columns["DEBIT"].SummaryItem.FieldName = "DEBIT";
            //            gridView1.Columns["DEBIT"].SummaryItem.DisplayFormat = "Total = {0:n}";

            //            //gridView1.OptionsView.ColumnAutoWidth = false;
            //            gridView1.OptionsView.ShowFooter = true;

            //            gridView1.BestFitColumns();

            //            label2.Text = Dt.Rows.Count + " Rows of Records ";


            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        Common.setMessageBox(ex.StackTrace + ex.Message, "Error During Import", 2); return;
            //    }
            //    finally
            //    {
            //        SplashScreenManager.CloseForm(false);
            //    }
            //}
            //else
            //{
            //    try
            //    {
            //        SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

            //        //exportToExcel();

            //        gridControl1.DataSource = null;

            //        using (OpenFileDialog openFileDialogCSV = new OpenFileDialog() { InitialDirectory = Application.ExecutablePath, Filter = "Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*", FilterIndex = 1, RestoreDirectory = true })
            //        {

            //            //openFileDialogCSV.ShowDialog();
            //            if (openFileDialogCSV.ShowDialog() == DialogResult.OK)

            //                if (openFileDialogCSV.FileName.Length > 0)
            //                {
            //                    filenamesopen = openFileDialogCSV.FileName;
            //                }

            //            try
            //            {
            //                Dt = new DataTable();
            //                Dt.BeginInit();
            //                Dt.Columns.Add("DATE", typeof(DateTime));
            //                Dt.Columns.Add("DEBIT", typeof(decimal));
            //                Dt.Columns.Add("CREDIT", typeof(decimal));
            //                Dt.Columns.Add("BALANCE", typeof(decimal));
            //                Dt.EndInit();

            //                Dt.Clear();
            //                MyConnection = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" +
            //                               filenamesopen + ";Extended Properties=\"Excel 8.0;HDR=YES;\"");

            //                MyCommand = new System.Data.OleDb.OleDbDataAdapter("select * from [Sheet1$]", MyConnection);
            //                DtSet = new System.Data.DataSet(); DtSet.Clear();
            //                MyCommand.Fill(DtSet, "[Sheet1$]");
            //                //MyCommand.Fill(Dt);

            //                foreach (DataRow row in DtSet.Tables[0].Rows)
            //                {
            //                    //DataRow rw = new DataRow();
            //                    if (!(row["DATE"] is DBNull) && (row["DATE"] != ""))
            //                    {
            //                        var rw = Dt.NewRow();
            //                        rw["DATE"] = row["DATE"];
            //                        rw["DEBIT"] = row["DEBIT"] is DBNull ? 0m : row["DEBIT"] == string.Empty ? 0m : Convert.ToDecimal(row["DEBIT"]);
            //                        rw["CREDIT"] = row["CREDIT"] is DBNull ? 0m : Convert.ToDecimal(row["CREDIT"]);
            //                        rw["BALANCE"] = row["BALANCE"] is DBNull ? 0m : Convert.ToDecimal(row["BALANCE"]);
            //                        //if (rw["DATE"] != string.Empty)
            //                        Dt.Rows.Add(rw);
            //                    }
            //                }


            //                for (int h = 0; h < Dt.Rows.Count; h++)
            //                {
            //                    if (Dt.Rows[h].IsNull(0) == true)
            //                    {
            //                        Dt.Rows[h].Delete();
            //                    }
            //                }

            //                Dt.AcceptChanges();
            //                //SearchPrevisonRecord();

            //                gridControl1.DataSource = Dt;
            //                gridView1.OptionsBehavior.Editable = false;
            //                MyConnection.Close();
            //            }
            //            catch (Exception ex)
            //            {
            //                Common.setMessageBox(ex.Message, Program.ApplicationName, 2);
            //                gridControl1.DataSource = null;
            //                return;
            //            }
            //            //ChangeValue(Dt);



            //            gridView1.Columns["DEBIT"].DisplayFormat.FormatType = FormatType.Numeric;
            //            gridView1.Columns["DEBIT"].DisplayFormat.FormatString = "n2";
            //            gridView1.Columns["CREDIT"].DisplayFormat.FormatType = FormatType.Numeric;
            //            gridView1.Columns["CREDIT"].DisplayFormat.FormatString = "n2";
            //            gridView1.Columns["BALANCE"].DisplayFormat.FormatType = FormatType.Numeric;
            //            gridView1.Columns["BALANCE"].DisplayFormat.FormatString = "n2";
            //            gridView1.Columns["BALANCE"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;

            //            gridView1.Columns["BALANCE"].SummaryItem.FieldName = "CREDIT";
            //            gridView1.Columns["BALANCE"].SummaryItem.DisplayFormat = "Total = {0:n}";

            //            gridView1.Columns["CREDIT"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            //            gridView1.Columns["DEBIT"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;

            //            gridView1.Columns["CREDIT"].SummaryItem.FieldName = "CREDIT";
            //            gridView1.Columns["CREDIT"].SummaryItem.DisplayFormat = "Total = {0:n}";

            //            gridView1.Columns["DEBIT"].SummaryItem.FieldName = "DEBIT";
            //            gridView1.Columns["DEBIT"].SummaryItem.DisplayFormat = "Total = {0:n}";

            //            //gridView1.OptionsView.ColumnAutoWidth = false;
            //            gridView1.OptionsView.ShowFooter = true;

            //            gridView1.BestFitColumns();
            //            label2.Text = Dt.Rows.Count + " Rows of Records ";


            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        Common.setMessageBox(ex.StackTrace + ex.Message, "Error Loading", 2); return;
            //    }
            //    finally
            //    {
            //        SplashScreenManager.CloseForm(false);
            //    }
            //}
            #endregion
        }
        //}
        void dtpEnd_ValueChanged(object sender, EventArgs e)
        {
            label43.Text = dtpEnd.Value.ToLongDateString();

        }

        void dtpStart_ValueChanged(object sender, EventArgs e)
        {
            label42.Text = dtpStart.Value.ToLongDateString();
        }

        public void RefreshForm()
        {
            NavBars.ToolStripEnableDisableControls(toolStrip, Tag as String);
        }

        public void setDBComboBox()
        {
            try
            {
                using (var ds = new System.Data.DataSet())
                {
                    using (SqlDataAdapter ada = new SqlDataAdapter((string)"SELECT BankShortCode,BankName FROM Collection.tblBank ORDER BY BankName", Logic.ConnectionString))
                    {
                        ada.Fill(ds, "table");
                    }

                    Common.setComboList(cboBank, ds.Tables[0], "BankShortCode", "BankName");

                }


                cboBank.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                Tripous.Sys.ErrorBox(String.Format("{0}{1}", ex.Message, ex.StackTrace)); return;
            }


        }
        void setDBComboBoxAcct()
        {
            try
            {
                isRecord = true;

                using (var ds = new System.Data.DataSet())
                {
                    using (SqlDataAdapter ada = new SqlDataAdapter(String.Format("SELECT BankAccountID,AccountNumber FROM ViewCurrencyBankAccount WHERE BankShortCode='{0}'", cboBank.SelectedValue.ToString()), Logic.ConnectionString))
                    {
                        ada.Fill(ds, "table");
                    }

                    Common.setComboList(cboAccount, ds.Tables[0], "BankAccountID", "AccountNumber");

                }

                cboAccount.SelectedIndex = -1; isRecord = false;
            }
            catch (Exception ex)
            {
                Tripous.Sys.ErrorBox(String.Format("{0}{1}", ex.Message, ex.StackTrace)); return;
            }
        }
        void setDbCombocBranch()
        {
            try
            {
                using (var ds = new System.Data.DataSet())
                {
                    using (SqlDataAdapter ada = new SqlDataAdapter((string)string.Format("SELECT BranchName,BranchCode FROM Collection.tblBankBranch WHERE BankShortCode ='{0}'", cboBank.SelectedValue), Logic.ConnectionString))
                    {
                        ada.Fill(ds, "table");
                    }
                    Common.setComboList(cbobranch, ds.Tables[0], "BranchCode", "BranchName");

                }


                cbobranch.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                Tripous.Sys.ErrorBox(String.Format("{0}{1}", ex.Message, ex.StackTrace)); return;
            }
        }

        void setDBComboxPaymode()
        {
            try
            {
                using (var ds = new System.Data.DataSet())
                {
                    using (SqlDataAdapter ada = new SqlDataAdapter((string)"SELECT PayID,Description FROM dbo.tblPayMode", Logic.ConnectionString))
                    {
                        ada.Fill(ds, "table");
                    }
                    Common.setComboList(cbopaymethod, ds.Tables[0], "PayID", "Description");

                }


                cbopaymethod.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                Tripous.Sys.ErrorBox(String.Format("{0}{1}", ex.Message, ex.StackTrace)); return;
            }
        }

        void setDBComBoxAccount()
        {
            try
            {
                if (string.IsNullOrEmpty(cboBank.SelectedValue.ToString()))
                {
                    Common.setEmptyField("Bank Name", Program.ApplicationName);
                    return;
                }
                else
                {
                    DataTable Dt;

                    using (var ds = new System.Data.DataSet())
                    {
                        using (SqlDataAdapter ada = new SqlDataAdapter((string)string.Format("SELECT  AccountNumber,BankAccountID  FROM  Reconciliation.tblBankAccount WHERE IsActive=1 AND BankShortCode ='{0}'", cboBank.SelectedValue), Logic.ConnectionString))
                        {
                            ada.Fill(ds, "table");
                        }
                        Dt = ds.Tables[0];


                        if (Dt != null && Dt.Rows.Count > 0)
                        {

                            cboAcct.DataSource = Dt;
                            cboAcct.DisplayMember = "AccountNumber";

                            cboAcct.ValueMember = "BankAccountID";
                        }
                    }


                    cboAcct.SelectedIndex = -1;
                }
            }
            catch (Exception ex)
            {
                Tripous.Sys.ErrorBox(String.Format("{0}{1}", ex.Message, ex.StackTrace)); return;
            }
        }

        void UpdateBankstatement(string strbankcode, DataTable dbData)
        {

            try
            {
                using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
                {
                    connect.Open();
                    _command = new SqlCommand("InsertBankExcellStatment", connect) { CommandType = CommandType.StoredProcedure };
                    _command.Parameters.Add(new SqlParameter("@bankcode", SqlDbType.VarChar)).Value = strbankcode;
                    _command.Parameters.Add(new SqlParameter("@startdate", SqlDbType.VarChar)).Value = string.Format("{0:yyyy/MM/dd}", dtpStart.Value);
                    _command.Parameters.Add(new SqlParameter("@enddate", SqlDbType.VarChar)).Value = string.Format("{0:yyyy/MM/dd}", dtpEnd.Value);
                    _command.Parameters.Add(new SqlParameter("@pTransaction", SqlDbType.Structured)).Value = dbData;
                    _command.Parameters.Add(new SqlParameter("@Period", SqlDbType.Char)).Value = label22.Text;
                    _command.Parameters.Add(new SqlParameter("@openBal", SqlDbType.Decimal)).Value = Convert.ToDecimal(lblBankStatementOpeningBalance.Text);
                    _command.Parameters.Add(new SqlParameter("@AccountID", SqlDbType.Char)).Value = Convert.ToInt32(cboAccount.SelectedValue);
                    _command.Parameters.Add(new SqlParameter("@ResetBank", SqlDbType.Bit)).Value = IsResetBankStmt;
                    _command.Parameters.Add(new SqlParameter("@reconiD", SqlDbType.Int)).Value = Convert.ToInt32(label25.Text);

                    _command.CommandTimeout = 0;
                    //@Years
                    using (System.Data.DataSet ds = new System.Data.DataSet())
                    {
                        ds.Clear();
                        //Dts = new DataTable();
                        //Dts.Clear();
                        adp = new SqlDataAdapter(_command);
                        adp.Fill(ds);
                        //Dts = ds.Tables[0];
                        connect.Close();

                        if (ds.Tables[0].Rows[0]["returnCode"].ToString() != "00")
                        {
                            Common.setMessageBox(ds.Tables[0].Rows[0]["returnMessage"].ToString(), Program.ApplicationName, 2);

                            return;

                        }
                        else
                        {
                            Common.setMessageBox(ds.Tables[0].Rows[0]["returnMessage"].ToString(), Program.ApplicationName, 2);
                        }
                        IsResetBankStmt = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Tripous.Sys.ErrorBox(String.Format("{0}{1}", ex.Message, ex.StackTrace)); return;
            }

        }

        private void setReloadDB()
        {
            try
            {
                //connect.connect.Close();

                //System.Data.DataSet ds;

                using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
                {
                    connect.Open();
                    _command = new SqlCommand("getBankDebit", connect) { CommandType = CommandType.StoredProcedure };
                    _command.Parameters.Add(new SqlParameter("@bankcode", SqlDbType.VarChar)).Value = cboBank.SelectedValue;
                    _command.Parameters.Add(new SqlParameter("@startdate", SqlDbType.VarChar)).Value = string.Format("{0:yyyy/MM/dd}", dtpStart.Value);
                    _command.Parameters.Add(new SqlParameter("@enddate", SqlDbType.VarChar)).Value = string.Format("{0:yyyy/MM/dd}", dtpEnd.Value);
                    _command.Parameters.Add(new SqlParameter("@pType", SqlDbType.Bit)).Value = boolIsUpdate2;
                    _command.Parameters.Add(new SqlParameter("@AccountId", SqlDbType.Int)).Value = Convert.ToInt32(cboAccount.SelectedValue);
                    _command.Parameters.Add(new SqlParameter("@periods", SqlDbType.Int)).Value = label22.Text;
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
                            dtde = new DataTable();
                            dtde.Clear();

                            dtde = ds.Tables[1];

                            dtde.Columns.Add("Description", typeof(String));

                            gridControl6.DataSource = dtde;
                        }

                    }
                }


                AddCombDebit();

                gridView8.Columns["Amount"].DisplayFormat.FormatType = FormatType.Numeric;
                gridView8.Columns["Amount"].DisplayFormat.FormatString = "n2";
                gridView8.Columns["Date"].DisplayFormat.FormatType = FormatType.DateTime;
                gridView8.Columns["Date"].DisplayFormat.FormatString = "dd/MM/yyyy";

                //var col= gridView1.Columns.Add();
                colView = gridView8.Columns["Description"];
                colView.ColumnEdit = repComboLookBox;

                colView.Visible = true;
                //OptionsColumn.AllowEdit = false

                gridView8.Columns["Amount"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;

                gridView8.Columns["Amount"].SummaryItem.FieldName = "Amount";
                gridView8.Columns["Amount"].SummaryItem.DisplayFormat = "Total = {0:n}";

                gridView8.Columns["Amount"].OptionsColumn.AllowEdit = false;
                gridView8.Columns["Date"].OptionsColumn.AllowEdit = false;
                gridView8.Columns["BSID"].Visible = false;
                gridView8.Columns["PayerName"].Caption = "Narration";
                gridView8.Columns["PayerName"].Visible = true;
                gridView8.Columns["PayerName"].OptionsColumn.AllowEdit = false;

                gridView8.OptionsView.ColumnAutoWidth = false;
                gridView8.OptionsView.ShowFooter = true;

                gridView8.BestFitColumns();
            }
            catch (Exception ex)
            {
                Tripous.Sys.ErrorBox(String.Format("{0}{1}", ex.Message, ex.StackTrace)); return;
            }
        }

        private void setReloadCD()
        {

            try
            {
                //DataTable dtc;
                //setReloadsExtracted();
                //System.Data.DataSet ds;
                using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
                {
                    connect.Open();
                    _command = new SqlCommand("getBankCredit", connect) { CommandType = CommandType.StoredProcedure };
                    _command.Parameters.Add(new SqlParameter("@bankcode", SqlDbType.VarChar)).Value = cboBank.SelectedValue;
                    _command.Parameters.Add(new SqlParameter("@startdate", SqlDbType.VarChar)).Value = string.Format("{0:yyyy/MM/dd}", dtpStart.Value);
                    _command.Parameters.Add(new SqlParameter("@enddate", SqlDbType.VarChar)).Value = string.Format("{0:yyyy/MM/dd}", dtpEnd.Value);
                    _command.Parameters.Add(new SqlParameter("@pType", SqlDbType.Bit)).Value = boolIsUpdate2;
                    _command.Parameters.Add(new SqlParameter("@AccountId", SqlDbType.Int)).Value = Convert.ToInt32(cboAccount.SelectedValue);
                    _command.Parameters.Add(new SqlParameter("@periods", SqlDbType.Int)).Value = label22.Text;
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
                            //Common.setMessageBox(ds.Tables[0].Rows[0]["returnMessage"].ToString(), Program.ApplicationName, 2);

                            dtc = new DataTable();
                            dtc.Clear();
                            dtc = ds.Tables[1];

                            dtc.Columns.Add("Description", typeof(String));

                            gridControl7.DataSource = dtc;
                        }

                    }
                }


                AddCombCredit();

                gridView9.Columns["Amount"].DisplayFormat.FormatType = FormatType.Numeric;
                gridView9.Columns["Amount"].DisplayFormat.FormatString = "n2";
                gridView9.Columns["Date"].DisplayFormat.FormatType = FormatType.DateTime;
                gridView9.Columns["Date"].DisplayFormat.FormatString = "dd/MM/yyyy";

                //var col= gridView1.Columns.Add();
                colView2 = gridView9.Columns["Description"];
                colView2.ColumnEdit = repComboLookBoxCredit;

                colView2.Visible = true;
                //OptionsColumn.AllowEdit = false

                gridView9.Columns["Amount"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                gridView9.Columns["Amount"].SummaryItem.FieldName = "Amount";
                gridView9.Columns["Amount"].SummaryItem.DisplayFormat = "Total = {0:n}";
                gridView9.Columns["Amount"].OptionsColumn.AllowEdit = false;
                gridView9.Columns["Date"].OptionsColumn.AllowEdit = false;
                gridView9.Columns["PayerName"].OptionsColumn.AllowEdit = false;
                gridView9.Columns["BSID"].Visible = false;
                gridView9.OptionsView.ColumnAutoWidth = false;
                gridView9.OptionsView.ShowFooter = true;
                gridView9.BestFitColumns();
            }
            catch (Exception ex)
            {
                Tripous.Sys.ErrorBox(String.Format("{0}{1}", ex.Message, ex.StackTrace)); return;
            }
        }

        void AddComboboxRevenue()
        {
            DataTable dtsed = new DataTable();

            repComboboxRevenue.DataSource = null;

            using (var ds = new System.Data.DataSet())
            {
                ds.Clear();
                using (SqlDataAdapter ada = new SqlDataAdapter("SELECT RevenueCode ,Description AS RevenueName FROM Collection.tblRevenueType ORDER BY Description ASC", Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }

                //repComboLookBox.NullText = "select";

                dtsed = ds.Tables[0];

                if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {
                    repComboboxRevenue.DataSource = ds.Tables[0];

                    repComboboxRevenue.DisplayMember = "RevenueName";

                    repComboboxRevenue.ValueMember = "RevenueCode";

                    repComboboxRevenue.AllowNullInput = DefaultBoolean.True;

                    repComboboxRevenue.PopulateViewColumns();

                    var view = repComboboxRevenue.View;

                    //view.Columns["RevenueName"].Visible = false;

                    //repComboboxRevenue.AutoComplete = true;
                }
            }
        }

        void AddCombDebit()
        {
            try
            {
                DataTable dtsed = new DataTable();

                repComboLookBox.DataSource = null;
                dtsed.Clear();
                //System.Data.DataSet ds;
                using (var ds = new System.Data.DataSet())
                {
                    ds.Clear();
                    using (SqlDataAdapter ada = new SqlDataAdapter("SELECT Description,TransID FROM Reconciliation.tblTransDefinition WHERE Type='dr' AND IsActive=1", Logic.ConnectionString))
                    {
                        ada.Fill(ds, "table");
                    }

                    repComboLookBox.NullText = "select";

                    dtsed = ds.Tables[0];

                    if (dtsed != null && dtsed.Rows.Count > 0)
                    {
                        repComboLookBox.DataSource = dtsed;
                        repComboLookBox.DisplayMember = "Description";
                        repComboLookBox.ValueMember = "TransID";
                        repComboLookBox.AllowNullInput = DefaultBoolean.True;

                        repComboLookBox.PopulateViewColumns();
                        var view = repComboLookBox.View;
                        view.Columns["TransID"].Visible = false;

                        repComboLookBox.AutoComplete = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Tripous.Sys.ErrorBox(String.Format("{0}{1}", ex.Message, ex.StackTrace)); return;
            }
        }

        void AddCombCredit()
        {
            try
            {
                DataTable dtse = new DataTable();

                repComboLookBoxCredit.DataSource = null;

                dtse.Clear();
                //System.Data.DataSet ds;
                using (var dsed = new System.Data.DataSet())
                {
                    dsed.Clear();

                    using (SqlDataAdapter adas = new SqlDataAdapter("SELECT Description,TransID  FROM Reconciliation.tblTransDefinition WHERE Type='cr' AND IsActive=1", Logic.ConnectionString))
                    {
                        adas.Fill(dsed, "table");

                    }

                    repComboLookBoxCredit.NullText = "select";

                    dtse = dsed.Tables[0];

                    if (dtse != null && dtse.Rows.Count > 0)
                    {
                        repComboLookBoxCredit.DataSource = dtse;
                        repComboLookBoxCredit.DisplayMember = "Description";
                        repComboLookBoxCredit.ValueMember = "TransID";
                        repComboLookBoxCredit.AllowNullInput = DefaultBoolean.True;
                        //Autocomplete on all values

                        repComboLookBoxCredit.PopulateViewColumns();
                        var view = repComboLookBoxCredit.View;
                        view.Columns["TransID"].Visible = false;

                        repComboLookBoxCredit.AutoComplete = true;
                    }

                }
            }
            catch (Exception ex)
            {
                Tripous.Sys.ErrorBox(String.Format("{0}{1}", ex.Message, ex.StackTrace)); return;
            }
        }

        void setReloadDebit()
        {
            try
            {

                using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
                {
                    connect.Open();
                    _command = new SqlCommand("getBankDebit", connect) { CommandType = CommandType.StoredProcedure };
                    _command.Parameters.Add(new SqlParameter("@bankcode", SqlDbType.VarChar)).Value = cboBank.SelectedValue;
                    _command.Parameters.Add(new SqlParameter("@startdate", SqlDbType.VarChar)).Value = string.Format("{0:yyyy/MM/dd}", dtpStart.Value);
                    _command.Parameters.Add(new SqlParameter("@enddate", SqlDbType.VarChar)).Value = string.Format("{0:yyyy/MM/dd}", dtpEnd.Value);
                    _command.Parameters.Add(new SqlParameter("@pType", SqlDbType.Bit)).Value = boolIsUpdate2;
                    _command.Parameters.Add(new SqlParameter("@periods", SqlDbType.Int)).Value = label22.Text;
                    _command.Parameters.Add(new SqlParameter("@AccountId", SqlDbType.Int)).Value = Convert.ToInt32(cboAccount.SelectedValue);
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
                            //Common.setMessageBox(ds.Tables[0].Rows[0]["returnMessage"].ToString(), Program.ApplicationName, 2);
                            dbDebit = new DataTable(); dbDebit.Clear();

                            //dtde = new DataTable();
                            //dtde.Clear();

                            dbDebit = ds.Tables[1];

                            //dbDebit.Columns.Add("Description", typeof(String));

                            //dbDebit.AcceptChanges();

                            gridControl6.DataSource = dbDebit;
                        }

                    }
                }

                AddCombDebit();

                gridView8.Columns["Amount"].DisplayFormat.FormatType = FormatType.Numeric;
                gridView8.Columns["Amount"].DisplayFormat.FormatString = "n2";
                gridView8.Columns["Date"].DisplayFormat.FormatType = FormatType.DateTime;
                gridView8.Columns["Date"].DisplayFormat.FormatString = "dd/MM/yyyy";

                //var col= gridView1.Columns.Add();
                colView = gridView8.Columns["TransID"];
                colView.ColumnEdit = repComboLookBox;

                colView.Visible = true;
                //OptionsColumn.AllowEdit = false

                gridView8.Columns["Amount"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;

                gridView8.Columns["Amount"].SummaryItem.FieldName = "Amount";
                gridView8.Columns["Amount"].SummaryItem.DisplayFormat = "Total = {0:n}";
                gridView8.Columns["Amount"].OptionsColumn.AllowEdit = false;
                gridView8.Columns["Date"].OptionsColumn.AllowEdit = false;
                gridView8.Columns["BSID"].Visible = false;
                gridView8.Columns["PayerName"].Visible = true;
                gridView8.Columns["PayerName"].Caption = "Narration";
                gridView8.Columns["TransID"].Caption = "Description";
                gridView8.Columns["PayerName"].OptionsColumn.AllowEdit = false;

                gridView8.OptionsView.ColumnAutoWidth = false;
                gridView8.OptionsView.ShowFooter = true;
                gridView8.BestFitColumns();
            }
            catch (Exception ex)
            {
                Tripous.Sys.ErrorBox(String.Format("{0}{1}", ex.Message, ex.StackTrace)); return;
            }
        }

        void setReloadCredit()
        {

            try
            {
                using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
                {
                    connect.Open();
                    _command = new SqlCommand("getBankCredit", connect) { CommandType = CommandType.StoredProcedure };
                    _command.Parameters.Add(new SqlParameter("@bankcode", SqlDbType.VarChar)).Value = cboBank.SelectedValue;
                    _command.Parameters.Add(new SqlParameter("@startdate", SqlDbType.VarChar)).Value = string.Format("{0:yyyy/MM/dd}", dtpStart.Value);
                    _command.Parameters.Add(new SqlParameter("@enddate", SqlDbType.VarChar)).Value = string.Format("{0:yyyy/MM/dd}", dtpEnd.Value);
                    _command.Parameters.Add(new SqlParameter("@pType", SqlDbType.Bit)).Value = boolIsUpdate2;
                    _command.Parameters.Add(new SqlParameter("@periods", SqlDbType.Int)).Value = label22.Text;
                    _command.Parameters.Add(new SqlParameter("@AccountId", SqlDbType.Int)).Value = Convert.ToInt32(cboAccount.SelectedValue);
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
                            //Common.setMessageBox(ds.Tables[0].Rows[0]["returnMessage"].ToString(), Program.ApplicationName, 2);

                            //dtc = new DataTable();
                            //dtc.Clear();
                            dbcredit = new DataTable(); dbcredit.Clear();

                            dbcredit = ds.Tables[1];

                            //dbcredit.Columns.Add("Description", typeof(String));

                            //dbcredit.AcceptChanges();

                            gridControl7.DataSource = dbcredit;
                        }

                    }
                }


                AddCombCredit();

                gridView9.Columns["Amount"].DisplayFormat.FormatType = FormatType.Numeric;
                gridView9.Columns["Amount"].DisplayFormat.FormatString = "n2";
                gridView9.Columns["Date"].DisplayFormat.FormatType = FormatType.DateTime;
                gridView9.Columns["Date"].DisplayFormat.FormatString = "dd/MM/yyyy";

                ////var col= gridView1.Columns.Add();
                colView2 = gridView9.Columns["TransID"];
                colView2.ColumnEdit = repComboLookBoxCredit;

                //colView2.Visible = true;
                //OptionsColumn.AllowEdit = false
                gridView9.Columns["Amount"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                gridView9.Columns["Amount"].SummaryItem.FieldName = "Amount";
                gridView9.Columns["Amount"].SummaryItem.DisplayFormat = "Total = {0:n}";

                gridView9.Columns["Amount"].OptionsColumn.AllowEdit = false;
                gridView9.Columns["Date"].OptionsColumn.AllowEdit = false;
                gridView9.Columns["BSID"].Visible = false;
                //gridView9.Columns["TransID"].Visible = false;
                gridView9.Columns["PayerName"].OptionsColumn.AllowEdit = false;
                gridView9.Columns["PayerName"].Caption = "Narration";
                gridView9.Columns["TransID"].Caption = "Description";
                gridView9.OptionsView.ColumnAutoWidth = false;
                gridView9.OptionsView.ShowFooter = true;
                gridView9.BestFitColumns();
            }
            catch (Exception ex)
            {
                Tripous.Sys.ErrorBox(String.Format("{0}{1}", ex.Message, ex.StackTrace)); return;
            }
        }

        void setReloadCreditExpection()
        {
            //bttnPostingrec.Enabled = false;

            try
            {
                using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
                {
                    connect.Open();
                    _command = new SqlCommand("PostCredit", connect) { CommandType = CommandType.StoredProcedure };
                    _command.Parameters.Add(new SqlParameter("@bankcode", SqlDbType.VarChar)).Value = cboBank.SelectedValue;
                    _command.Parameters.Add(new SqlParameter("@startdate", SqlDbType.VarChar)).Value = string.Format("{0:yyyy/MM/dd}", dtpStart.Value);
                    _command.Parameters.Add(new SqlParameter("@enddate", SqlDbType.VarChar)).Value = string.Format("{0:yyyy/MM/dd}", dtpEnd.Value);
                    //_command.Parameters.Add(new SqlParameter("@pType", SqlDbType.Bit)).Value = boolIsUpdate2;
                    _command.Parameters.Add(new SqlParameter("@periods", SqlDbType.Int)).Value = label22.Text;
                    _command.Parameters.Add(new SqlParameter("@Accountid", SqlDbType.Int)).Value = Convert.ToInt32(cboAccount.SelectedValue);

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
                            dbcredit = new DataTable(); dbcredit.Clear();

                            dbcredit = ds.Tables[1];

                            if (ds.Tables[1].Rows.Count > 0 && ds.Tables[1] != null)
                            {
                                //isComplete = false;
                                bttnPostingrec.Enabled = true;
                            }
                            else
                            {
                                //label57.Text = "Completed";
                                //isComplete = true;
                            }
                            gridControl8.DataSource = ds.Tables[1];
                        }

                    }
                }
                gridView10.OptionsBehavior.Editable = false;
                gridView10.Columns["Amount"].DisplayFormat.FormatType = FormatType.Numeric;
                gridView10.Columns["Amount"].DisplayFormat.FormatString = "n2";
                gridView10.Columns["Date"].DisplayFormat.FormatType = FormatType.DateTime;
                gridView10.Columns["Date"].DisplayFormat.FormatString = "dd/MM/yyyy";


                gridView10.Columns["Amount"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                gridView10.Columns["Amount"].SummaryItem.FieldName = "Amount";
                gridView10.Columns["Amount"].SummaryItem.DisplayFormat = "Total = {0:n}";

                gridView10.Columns["Amount"].OptionsColumn.AllowEdit = false;
                gridView10.Columns["Date"].OptionsColumn.AllowEdit = false;
                gridView10.Columns["BSID"].Visible = false;
                gridView10.OptionsView.ColumnAutoWidth = false;
                gridView10.OptionsView.ShowFooter = true;
                gridView10.BestFitColumns();
            }
            catch (Exception ex)
            {
                Tripous.Sys.ErrorBox(String.Format("{0}{1}", ex.Message, ex.StackTrace)); return;
            }
        }

        void deleteRecord(string parameter2)
        {
            using (SqlConnection db = new SqlConnection(Logic.ConnectionString))
            {
                SqlTransaction transaction;

                db.Open();

                transaction = db.BeginTransaction();

                try
                {
                    using (SqlCommand sqlCommand1 = new SqlCommand(parameter2, db, transaction))
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
            }
        }

        void CalClose()
        {
            try
            {
                using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
                {
                    connect.Open();
                    _command = new SqlCommand("CalcuteClosingBal", connect) { CommandType = CommandType.StoredProcedure };
                    _command.Parameters.Add(new SqlParameter("@bankcode", SqlDbType.VarChar)).Value = cboBank.SelectedValue;
                    _command.Parameters.Add(new SqlParameter("@startDate", SqlDbType.DateTime)).Value = string.Format("{0:yyyy/MM/dd}", dtpStart.Value);
                    _command.Parameters.Add(new SqlParameter("@endDate", SqlDbType.DateTime)).Value = string.Format("{0:yyyy/MM/dd}", dtpEnd.Value);

                    using (System.Data.DataSet ds = new System.Data.DataSet())
                    {
                        ds.Clear();
                        adp = new SqlDataAdapter(_command);
                        adp.Fill(ds);
                        //Dts = ds.Tables[0];
                        connect.Close();

                        if (ds.Tables[0].Rows[0]["returnCode"].ToString() == "00")
                        {
                            label6.Text = String.Format("{0:N2}", ds.Tables[1].Rows[0]["ClosingBal"]);
                        }
                        else
                        {
                            Common.setMessageBox(ds.Tables[0].Rows[0]["returnMessage"].ToString(), Program.ApplicationName, 2);
                        }

                    }
                }

                //label6.Text = string.Format("{0:N2} ", totalAmount);
            }
            catch (Exception ex)
            {
                Tripous.Sys.ErrorBox(String.Format("{0}{1}", ex.Message, ex.StackTrace)); return;
            }
        }

        public void setDBComboBoxRev()
        {
            try
            {
                DataTable Dt;

                using (var ds = new System.Data.DataSet())
                {
                    //connect.connect.Open();
                    using (SqlDataAdapter ada = new SqlDataAdapter((string)"SELECT RevenueCode,Description FROM Collection.tblRevenueType", Logic.ConnectionString))
                    {
                        ada.Fill(ds, "table");
                    }

                    Dt = ds.Tables[0];
                }

                Common.setComboList(cboRevenuetype, Dt, "RevenueCode", "Description");

                cboRevenuetype.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                Tripous.Sys.ErrorBox(String.Format("{0}{1}", ex.Message, ex.StackTrace)); return;
            }


        }

        void getAgency(string parameter)
        {
            try
            {
                DataTable Dts = new DataTable();

                System.Data.DataSet dataSet3 = new System.Data.DataSet();

                dataSet3.Clear(); Dts.Clear();
                using (SqlDataAdapter ada = new SqlDataAdapter((string)string.Format("SELECT Registration.tblAgency.AgencyCode , AgencyName FROM Registration.tblAgency INNER JOIN Collection.tblRevenueType  ON Registration.tblAgency.AgencyCode = Collection.tblRevenueType.AgencyCode WHERE RevenueCode = '{0}'", parameter), Logic.ConnectionString))
                {
                    ada.Fill(dataSet3, "table");
                }

                Dts = dataSet3.Tables[0];

                if (Dts != null && Dts.Rows.Count > 0)
                {
                    label55.Text = String.Format("{0}", Dts.Rows[0]["AgencyName"]);
                    label7.Text = (string)Dts.Rows[0]["AgencyCode"];


                }
            }
            catch (Exception ex)
            {
                Tripous.Sys.ErrorBox(String.Format("{0}{1}", ex.Message, ex.StackTrace)); return;
            }

        }

        void GetRecord()
        {
            GridView view = (GridView)gridControl8.FocusedView;

            if (view != null)
            {
                DataRow dr = view.GetDataRow(view.FocusedRowHandle);

                if (dr != null)
                {

                    txtpaymentdate.Text = (string)dr["DATE"];
                    //txtpstamount.Text = dr["CREDIT"].ToString();
                    txtpstamount.Text = String.Format("{0:N2}", dr["Amount"]);

                    cboRevenuetype.Focus();
                }
                else
                {
                    Common.setEmptyField("Select Record...", "Get Record"); return;
                }
            }
        }

        void LinksClicked(object sender, EventArgs e)
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

                if (sender == linkLabel1)
                {
                    using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
                    {
                        connect.Open();
                        _command = new SqlCommand("GetPaymentRecord", connect) { CommandType = CommandType.StoredProcedure };
                        _command.Parameters.Add(new SqlParameter("@bankcode", SqlDbType.VarChar)).Value = cboBank.SelectedValue;
                        _command.Parameters.Add(new SqlParameter("@startDate", SqlDbType.DateTime)).Value = string.Format("{0:yyyy/MM/dd}", dtpStart.Value);
                        _command.Parameters.Add(new SqlParameter("@endDate", SqlDbType.DateTime)).Value = string.Format("{0:yyyy/MM/dd}", dtpEnd.Value);
                        _command.Parameters.Add(new SqlParameter("@PType", SqlDbType.VarChar)).Value = "Collections";

                        using (System.Data.DataSet ds = new System.Data.DataSet())
                        {
                            ds.Clear();
                            adp = new SqlDataAdapter(_command);
                            adp.Fill(ds, "table");
                            //Dts = ds.Tables[0];
                            connect.Close();

                            if (ds.Tables[1].Rows[0]["returnCode"].ToString() == "00")
                            {
                                label6.Text = String.Format("{0:N2}", ds.Tables[1].Rows[0]["ClosingBal"]);

                                if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                                {
                                    XtraRepReconDetails repRec = new XtraRepReconDetails() { DataSource = ds.Tables[0], DataMember = "table" };

                                    repRec.xrLabel3.Text = String.Format(" PayDirect Collections between {0} and {1}", string.Format("{0:dd/MM/yyyy}", dtpStart.Value), string.Format("{0:dd/MM/yyyy}", dtpEnd.Value));
                                    repRec.xrLabel5.Text = String.Format("Bank Name:{0}", cboBank.Text);

                                    repRec.ShowPreviewDialog();
                                }
                                else
                                {
                                    Common.setMessageBox("No Record Found", Program.ApplicationName, 1); return;
                                }
                            }
                            else
                            {
                                Common.setMessageBox(ds.Tables[0].Rows[0]["returnMessage"].ToString(), Program.ApplicationName, 2);
                            }

                        }
                    }
                }
                else if (sender == linkLabel2)
                {
                    using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
                    {
                        connect.Open();
                        _command = new SqlCommand("GetPaymentRecord", connect) { CommandType = CommandType.StoredProcedure };
                        _command.Parameters.Add(new SqlParameter("@bankcode", SqlDbType.VarChar)).Value = cboBank.SelectedValue;
                        _command.Parameters.Add(new SqlParameter("@startDate", SqlDbType.DateTime)).Value = string.Format("{0:yyyy/MM/dd}", dtpStart.Value);
                        _command.Parameters.Add(new SqlParameter("@endDate", SqlDbType.DateTime)).Value = string.Format("{0:yyyy/MM/dd}", dtpEnd.Value);
                        _command.Parameters.Add(new SqlParameter("@PType", SqlDbType.VarChar)).Value = "PayDirectBank";

                        using (System.Data.DataSet ds = new System.Data.DataSet())
                        {
                            ds.Clear();
                            adp = new SqlDataAdapter(_command);
                            adp.Fill(ds, "table");
                            //Dts = ds.Tables[0];
                            connect.Close();

                            if (ds.Tables[1].Rows[0]["returnCode"].ToString() == "00")
                            {
                                label6.Text = String.Format("{0:N2}", ds.Tables[1].Rows[0]["ClosingBal"]);

                                if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                                {
                                    XtraRepPayDirectBank rep = new XtraRepPayDirectBank() { DataSource = ds.Tables[0], DataMember = "table" };

                                    rep.xrLabel10.Text = "List of Transactions in PayDirect but Not in Bank Statement";

                                    rep.xrLabel12.Text = string.Format("Between {0} and {1}", string.Format("{0:dd/MM/yyyy}", dtpStart.Value), string.Format("{0:dd/MM/yyyy}", dtpEnd.Value));

                                    rep.ShowPreviewDialog();
                                }
                                else
                                {
                                    Common.setMessageBox("No Record Found", Program.ApplicationName, 1); return;
                                }
                            }
                            else
                            {
                                Common.setMessageBox(ds.Tables[0].Rows[0]["returnMessage"].ToString(), Program.ApplicationName, 2);
                            }

                        }
                    }
                }
                else if (sender == linkLabel3)
                {
                    using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
                    {
                        connect.Open();
                        _command = new SqlCommand("GetPaymentRecord", connect) { CommandType = CommandType.StoredProcedure };
                        _command.Parameters.Add(new SqlParameter("@bankcode", SqlDbType.VarChar)).Value = cboBank.SelectedValue;
                        _command.Parameters.Add(new SqlParameter("@startDate", SqlDbType.DateTime)).Value = string.Format("{0:yyyy/MM/dd}", dtpStart.Value);
                        _command.Parameters.Add(new SqlParameter("@endDate", SqlDbType.DateTime)).Value = string.Format("{0:yyyy/MM/dd}", dtpEnd.Value);
                        _command.Parameters.Add(new SqlParameter("@PType", SqlDbType.VarChar)).Value = "BankStatment";

                        using (System.Data.DataSet ds = new System.Data.DataSet())
                        {
                            ds.Clear();
                            adp = new SqlDataAdapter(_command);
                            adp.Fill(ds, "table");
                            //Dts = ds.Tables[0];
                            connect.Close();

                            if (ds.Tables[1].Rows[0]["returnCode"].ToString() == "00")
                            {
                                label6.Text = String.Format("{0:N2}", ds.Tables[1].Rows[0]["ClosingBal"]);

                                if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                                {
                                    XtraRepPayDirectBank rep = new XtraRepPayDirectBank() { DataSource = ds.Tables[0], DataMember = "table" };

                                    rep.xrLabel10.Text = "List of Transactions in Bank Statement ";

                                    rep.xrLabel12.Text = string.Format("Between {0} and {1}", string.Format("{0:dd/MM/yyyy}", dtpStart.Value), string.Format("{0:dd/MM/yyyy}", dtpEnd.Value));

                                    rep.ShowPreviewDialog();
                                }
                                else
                                {
                                    Common.setMessageBox("No Record Found", Program.ApplicationName, 1); return;
                                }
                            }
                            else
                            {
                                Common.setMessageBox(ds.Tables[0].Rows[0]["returnMessage"].ToString(), Program.ApplicationName, 2);
                            }

                        }
                    }
                }
                else if (sender == linkLabel4)
                {
                    using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
                    {
                        connect.Open();
                        _command = new SqlCommand("GetPaymentRecord", connect) { CommandType = CommandType.StoredProcedure };
                        _command.Parameters.Add(new SqlParameter("@bankcode", SqlDbType.VarChar)).Value = cboBank.SelectedValue;
                        _command.Parameters.Add(new SqlParameter("@startDate", SqlDbType.DateTime)).Value = string.Format("{0:yyyy/MM/dd}", dtpStart.Value);
                        _command.Parameters.Add(new SqlParameter("@endDate", SqlDbType.DateTime)).Value = string.Format("{0:yyyy/MM/dd}", dtpEnd.Value);
                        _command.Parameters.Add(new SqlParameter("@PType", SqlDbType.VarChar)).Value = "BankPayDirect";

                        using (System.Data.DataSet ds = new System.Data.DataSet())
                        {
                            ds.Clear();
                            adp = new SqlDataAdapter(_command);
                            adp.Fill(ds, "table");
                            //Dts = ds.Tables[0];
                            connect.Close();

                            if (ds.Tables[1].Rows[0]["returnCode"].ToString() == "00")
                            {
                                label6.Text = String.Format("{0:N2}", ds.Tables[1].Rows[0]["ClosingBal"]);

                                if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                                {
                                    XtraRepPayDirectBank rep = new XtraRepPayDirectBank() { DataSource = ds.Tables[0], DataMember = "table" };

                                    rep.xrLabel10.Text = "List of Transactions in Bank Statement but not in PayDirect ";

                                    rep.xrLabel12.Text = string.Format("Between {0} and {1}", string.Format("{0:dd/MM/yyyy}", dtpStart.Value), string.Format("{0:dd/MM/yyyy}", dtpEnd.Value));

                                    rep.ShowPreviewDialog();
                                }
                                else
                                {
                                    Common.setMessageBox("No Record Found", Program.ApplicationName, 1); return;
                                }
                            }
                            else
                            {
                                Common.setMessageBox(ds.Tables[0].Rows[0]["returnMessage"].ToString(), Program.ApplicationName, 2);
                            }

                        }
                    }
                }
            }
            finally
            {
                SplashScreenManager.CloseForm(false);
            }
        }

        void getopenBal()
        {
            if (string.IsNullOrEmpty(cboBank.SelectedValue.ToString()))
            {
                Common.setEmptyField("Bank Name", Program.ApplicationName);
                return;
            }
            else if (string.IsNullOrEmpty(cboAccount.SelectedValue.ToString()))
            {
                Common.setEmptyField("Account Number", Program.ApplicationName);
                return;
            }
            else
            {

                //if (stageID == 4)
                //{
                //    Common.setMessageBox("Sorry, this bank account has already been reconciled and is awaiting approval.\nPlease contact the approving officier.", Program.ApplicationName, 2);
                //    return;
                //}

                DataTable dtexit = new DataTable();
                label14.Text = string.Format("{0:n2}", GetOpeningBalance(Convert.ToInt32(cboAccount.SelectedValue), cboBank.SelectedValue.ToString(), Convert.ToInt32(label22.Text)));

                int stageID = GetBankStageID();
                bttnreset.Enabled = stageID >= 1 && stageID <= 4;
                switch (stageID)
                {
                    case 1:
                        xtraTabPage4.PageEnabled = false;
                        xtraTabPage2.PageEnabled = false;
                        xtraTabPage3.PageEnabled = true;
                        xtraTabPage1.PageEnabled = false;
                        xtraTabControl1.SelectedTabPage = xtraTabPage3;
                        xtraTabControl1.Enabled = true;
                        return;
                        break;

                    case 2:
                        xtraTabPage4.PageEnabled = false;
                        xtraTabPage2.PageEnabled = true;
                        xtraTabPage3.PageEnabled = false;
                        xtraTabPage1.PageEnabled = false;
                        xtraTabControl1.SelectedTabPage = xtraTabPage2;
                        xtraTabControl1.Enabled = true;
                        return;
                        break;

                    case 3:
                        xtraTabPage4.PageEnabled = true;
                        xtraTabPage2.PageEnabled = false;
                        xtraTabPage3.PageEnabled = false;
                        xtraTabPage1.PageEnabled = false;
                        xtraTabControl1.SelectedTabPage = xtraTabPage4;
                        xtraTabControl1.Enabled = true;
                        return;
                        break;

                    case 4:
                        Common.setMessageBox("Sorry, this bank account has already been reconciled and is awaiting approval.\nPlease contact the approving officier.", Program.ApplicationName, 2);
                        cboAcct.SelectedIndex = -1;
                        return;
                        break;

                    case 5:
                        Common.setMessageBox("This transaction period has been closed", Program.ApplicationName, 2);
                        cboAcct.SelectedIndex = -1;
                        return;
                }
                dtexit.Clear();
                try
                {
                    SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

                    using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
                    {
                        connect.Open();
                        _command = new SqlCommand("doGetOpeningAccount", connect) { CommandType = CommandType.StoredProcedure };
                        _command.Parameters.Add(new SqlParameter("@bankcode", SqlDbType.VarChar)).Value = cboBank.SelectedValue;
                        _command.Parameters.Add(new SqlParameter("@Periods", SqlDbType.Int)).Value = label22.Text;
                        _command.Parameters.Add(new SqlParameter("@AccountId", SqlDbType.Int)).Value = Convert.ToInt32(cboAccount.SelectedValue);
                        _command.Parameters.Add(new SqlParameter("@enddate", SqlDbType.VarChar)).Value = string.Format("{0:yyyy/MM/dd 23:59:59}", dtpEnd.Value);
                        _command.Parameters.Add(new SqlParameter("@startdate", SqlDbType.VarChar)).Value = string.Format("{0:yyyy/MM/dd 23:59:59}", dtpStart.Value);
                        _command.CommandTimeout = 0;
                        using (System.Data.DataSet ds = new System.Data.DataSet())
                        {
                            ds.Clear();
                            adp = new SqlDataAdapter(_command);
                            adp.Fill(ds);
                            //Dts = ds.Tables[0];
                            connect.Close();

                            if (ds.Tables[0].Rows[0]["returnCode"].ToString() == "00")
                            {
                                //Common.setkMessageBox(ds.Tables[0].Rows[0]["returnMessage"].ToString(), Program.ApplicationName, 2);

                                label14.Text = String.Format("{0:N2}", ds.Tables[1].Rows[0]["openingbal"]);

                                label17.Text = ds.Tables[1].Rows[0]["Acctnumber"].ToString();

                                //label6.Text = String.Format("{0:N2}", ds.Tables[1].Rows[0]["closebal"]);

                                isClosed = true;

                                if (ds.Tables[1].Rows[0]["EndDate"] != DBNull.Value)
                                {
                                    //dtpStart.Enabled = true; dtpEnd.Enabled = true;

                                    //dtpStart.Value = Convert.ToDateTime(ds.Tables[1].Rows[0]["EndDate"]);
                                }
                                else
                                {
                                    dtpStart.Value = dtpStart.Value;
                                    dtpStart.Enabled = false;
                                }


                                dtexit = ds.Tables[2];

                                if (dtexit != null && dtexit.Rows.Count > 0)
                                {
                                    Dt = dtexit;
                                    gridControl1.DataSource = dtexit;
                                    gridView1.OptionsBehavior.Editable = false;
                                    AddComboboxRevenue();
                                    gridView1.Columns["DEBIT"].DisplayFormat.FormatType = FormatType.Numeric;
                                    gridView1.Columns["DEBIT"].DisplayFormat.FormatString = "n2";
                                    gridView1.Columns["CREDIT"].DisplayFormat.FormatType = FormatType.Numeric;
                                    gridView1.Columns["CREDIT"].DisplayFormat.FormatString = "n2";
                                    gridView1.Columns["BALANCE"].DisplayFormat.FormatType = FormatType.Numeric;
                                    gridView1.Columns["BALANCE"].DisplayFormat.FormatString = "n2";

                                    gridView1.Columns["CREDIT"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                                    gridView1.Columns["DEBIT"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                                    gridView1.Columns["BALANCE"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;

                                    gridView1.Columns["BALANCE"].SummaryItem.FieldName = "CREDIT";
                                    gridView1.Columns["BALANCE"].SummaryItem.DisplayFormat = "Total = {0:n}";

                                    gridView1.Columns["CREDIT"].SummaryItem.FieldName = "CREDIT";
                                    gridView1.Columns["CREDIT"].SummaryItem.DisplayFormat = "Total = {0:n}";

                                    gridView1.Columns["DEBIT"].SummaryItem.FieldName = "DEBIT";
                                    gridView1.Columns["DEBIT"].SummaryItem.DisplayFormat = "Total = {0:n}";

                                    colRevenue = gridView1.Columns["REVENUECODE"];
                                    colRevenue.ColumnEdit = repComboboxRevenue;

                                    colRevenue.Visible = true;
                                    //gridView1.OptionsView.ColumnAutoWidth = false;
                                    gridView1.OptionsView.ShowFooter = true;

                                    gridView1.BestFitColumns();



                                    label2.Text = dtexit.Rows.Count + " Rows of Records ";

                                    bttnReimport.Enabled = true; bttnImport.Enabled = false; //bttnUpdateExcel.Enabled = true;
                                }
                                else
                                {
                                    bttnReimport.Enabled = false; bttnImport.Enabled = true; //bttnUpdateExcel.Enabled = true;
                                }

                                dtpEnd_ValueChanged(null, null);
                                dtpStart_ValueChanged(null, null);
                                xtraTabPage2.PageEnabled = false;
                                xtraTabPage1.PageEnabled = true; xtraTabPage3.PageEnabled = false;
                                xtraTabControl1.SelectedTabPage = xtraTabPage1;
                                xtraTabControl1.Enabled = true;
                                return;
                            }
                            else if (ds.Tables[0].Rows[0]["returnCode"].ToString() == "01")
                            {
                                //Common.setMessagekBox(ds.Tables[0].Rows[0]["returnMessage"].ToString(), Program.ApplicationName, 2);

                                label14.Text = String.Format("{0:N2}", ds.Tables[1].Rows[0]["openingbal"]);
                                label17.Text = ds.Tables[1].Rows[0]["Acctnumber"].ToString();

                                if (ds.Tables[1].Rows[0]["EndDate"] != DBNull.Value)
                                {
                                    //dtpStart.Enabled = true; dtpEnd.Enabled = true;

                                    //dtpStart.Value = Convert.ToDateTime(ds.Tables[1].Rows[0]["EndDate"]);
                                }
                                else
                                {
                                    dtpStart.Value = dtpStart.Value;
                                    dtpStart.Enabled = false;
                                }

                                dtexit = ds.Tables[2];

                                if (dtexit != null && dtexit.Rows.Count > 0)
                                {
                                    Dt = dtexit;
                                    gridControl1.DataSource = dtexit;
                                    gridView1.OptionsBehavior.Editable = false;
                                    AddComboboxRevenue();
                                    gridView1.Columns["DEBIT"].DisplayFormat.FormatType = FormatType.Numeric;
                                    gridView1.Columns["DEBIT"].DisplayFormat.FormatString = "n2";
                                    gridView1.Columns["CREDIT"].DisplayFormat.FormatType = FormatType.Numeric;
                                    gridView1.Columns["CREDIT"].DisplayFormat.FormatString = "n2";
                                    gridView1.Columns["BALANCE"].DisplayFormat.FormatType = FormatType.Numeric;
                                    gridView1.Columns["BALANCE"].DisplayFormat.FormatString = "n2";

                                    gridView1.Columns["CREDIT"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                                    gridView1.Columns["DEBIT"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                                    gridView1.Columns["BALANCE"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;

                                    gridView1.Columns["BALANCE"].SummaryItem.FieldName = "CREDIT";
                                    gridView1.Columns["BALANCE"].SummaryItem.DisplayFormat = "Total = {0:n}";

                                    gridView1.Columns["CREDIT"].SummaryItem.FieldName = "CREDIT";
                                    gridView1.Columns["CREDIT"].SummaryItem.DisplayFormat = "Total = {0:n}";

                                    gridView1.Columns["DEBIT"].SummaryItem.FieldName = "DEBIT";
                                    gridView1.Columns["DEBIT"].SummaryItem.DisplayFormat = "Total = {0:n}";

                                    colRevenue = gridView1.Columns["REVENUECODE"];
                                    colRevenue.ColumnEdit = repComboboxRevenue;

                                    colRevenue.Visible = true;

                                    //gridView1.OptionsView.ColumnAutoWidth = false;
                                    gridView1.OptionsView.ShowFooter = true;

                                    gridView1.BestFitColumns();

                                    label2.Text = dtexit.Rows.Count + " Rows of Records ";

                                    bttnReimport.Enabled = true; bttnImport.Enabled = false; //bttnUpdateExcel.Enabled = true;
                                }
                                else
                                {
                                    bttnReimport.Enabled = false; bttnImport.Enabled = true; //bttnUpdateExcel.Enabled = true;
                                }


                                dtpEnd_ValueChanged(null, null);
                                dtpStart_ValueChanged(null, null);
                                xtraTabPage2.PageEnabled = false;
                                xtraTabPage1.PageEnabled = true; xtraTabPage3.PageEnabled = false;
                                xtraTabControl1.SelectedTabPage = xtraTabPage1;
                                xtraTabControl1.Enabled = true;
                            }
                            else if (ds.Tables[0].Rows[0]["returnCode"].ToString() == "02")
                            {

                                //Common.setMessageBox(ds.Tables[0].Rows[0]["returnMessage"].ToString(), Program.ApplicationName, 2);

                                label14.Text = String.Format("{0:N2}", ds.Tables[1].Rows[0]["openingbal"]);
                                label17.Text = ds.Tables[1].Rows[0]["Acctnumber"].ToString();

                                dtexit = ds.Tables[2];


                                if (dtexit != null && dtexit.Rows.Count > 0)
                                {
                                    Dt = dtexit;
                                    gridControl1.DataSource = dtexit;
                                    gridView1.OptionsBehavior.Editable = false;
                                    AddComboboxRevenue();
                                    gridView1.Columns["DEBIT"].DisplayFormat.FormatType = FormatType.Numeric;
                                    gridView1.Columns["DEBIT"].DisplayFormat.FormatString = "n2";
                                    gridView1.Columns["CREDIT"].DisplayFormat.FormatType = FormatType.Numeric;
                                    gridView1.Columns["CREDIT"].DisplayFormat.FormatString = "n2";
                                    gridView1.Columns["BALANCE"].DisplayFormat.FormatType = FormatType.Numeric;
                                    gridView1.Columns["BALANCE"].DisplayFormat.FormatString = "n2";

                                    gridView1.Columns["CREDIT"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                                    gridView1.Columns["DEBIT"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                                    gridView1.Columns["BALANCE"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;

                                    gridView1.Columns["BALANCE"].SummaryItem.FieldName = "CREDIT";
                                    gridView1.Columns["BALANCE"].SummaryItem.DisplayFormat = "Total = {0:n}";

                                    gridView1.Columns["CREDIT"].SummaryItem.FieldName = "CREDIT";
                                    gridView1.Columns["CREDIT"].SummaryItem.DisplayFormat = "Total = {0:n}";

                                    gridView1.Columns["DEBIT"].SummaryItem.FieldName = "DEBIT";
                                    gridView1.Columns["DEBIT"].SummaryItem.DisplayFormat = "Total = {0:n}";

                                    colRevenue = gridView1.Columns["REVENUECODE"];
                                    colRevenue.ColumnEdit = repComboboxRevenue;

                                    colRevenue.Visible = true;

                                    //gridView1.OptionsView.ColumnAutoWidth = false;
                                    gridView1.OptionsView.ShowFooter = true;

                                    gridView1.BestFitColumns();

                                    label2.Text = dtexit.Rows.Count + " Rows of Records ";

                                    bttnReimport.Enabled = true; bttnImport.Enabled = false; //bttnUpdateExcel.Enabled = true;
                                }
                                else
                                {
                                    bttnReimport.Enabled = false; bttnImport.Enabled = true; //bttnUpdateExcel.Enabled = true;
                                }


                                //dtpStart.Enabled = false;
                                dtpEnd_ValueChanged(null, null);
                                dtpStart_ValueChanged(null, null);
                                xtraTabPage2.PageEnabled = false;
                                xtraTabPage1.PageEnabled = true; xtraTabPage3.PageEnabled = false;
                                xtraTabControl1.SelectedTabPage = xtraTabPage1;
                                xtraTabControl1.Enabled = true;


                            }
                            else if (ds.Tables[0].Rows[0]["returnCode"].ToString() == "00")
                            {
                                //Common.setMessageBox(ds.Tables[0].Rows[0]["returnMessage"].ToString(), Program.ApplicationName, 2);

                                label14.Text = String.Format("{0:N2}", ds.Tables[1].Rows[0]["openingbal"]);
                                label17.Text = ds.Tables[1].Rows[0]["Acctnumber"].ToString();

                                if (ds.Tables[1].Rows[0]["EndDate"] != DBNull.Value)
                                {
                                    //dtpStart.Enabled = true; dtpEnd.Enabled = true;

                                    //dtpStart.Value = Convert.ToDateTime(ds.Tables[1].Rows[0]["EndDate"]);
                                }
                                else
                                {
                                    dtpStart.Value = dtpStart.Value;
                                    dtpStart.Enabled = false;
                                }


                                dtexit = ds.Tables[2];

                                if (dtexit != null && dtexit.Rows.Count > 0)
                                {
                                    Dt = dtexit;
                                    gridControl1.DataSource = dtexit;
                                    gridView1.OptionsBehavior.Editable = false;
                                    AddComboboxRevenue();
                                    gridView1.Columns["DEBIT"].DisplayFormat.FormatType = FormatType.Numeric;
                                    gridView1.Columns["DEBIT"].DisplayFormat.FormatString = "n2";
                                    gridView1.Columns["CREDIT"].DisplayFormat.FormatType = FormatType.Numeric;
                                    gridView1.Columns["CREDIT"].DisplayFormat.FormatString = "n2";
                                    gridView1.Columns["BALANCE"].DisplayFormat.FormatType = FormatType.Numeric;
                                    gridView1.Columns["BALANCE"].DisplayFormat.FormatString = "n2";

                                    gridView1.Columns["CREDIT"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                                    gridView1.Columns["DEBIT"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                                    gridView1.Columns["BALANCE"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;

                                    gridView1.Columns["BALANCE"].SummaryItem.FieldName = "CREDIT";
                                    gridView1.Columns["BALANCE"].SummaryItem.DisplayFormat = "Total = {0:n}";

                                    gridView1.Columns["CREDIT"].SummaryItem.FieldName = "CREDIT";
                                    gridView1.Columns["CREDIT"].SummaryItem.DisplayFormat = "Total = {0:n}";

                                    gridView1.Columns["DEBIT"].SummaryItem.FieldName = "DEBIT";
                                    gridView1.Columns["DEBIT"].SummaryItem.DisplayFormat = "Total = {0:n}";

                                    colRevenue = gridView1.Columns["REVENUECODE"];
                                    colRevenue.ColumnEdit = repComboboxRevenue;

                                    colRevenue.Visible = true;

                                    //gridView1.OptionsView.ColumnAutoWidth = false;
                                    gridView1.OptionsView.ShowFooter = true;

                                    gridView1.BestFitColumns();

                                    label2.Text = dtexit.Rows.Count + " Rows of Records ";

                                    bttnReimport.Enabled = true; bttnImport.Enabled = false; //bttnUpdateExcel.Enabled = true;
                                }
                                else
                                {
                                    bttnReimport.Enabled = false; bttnImport.Enabled = true; //bttnUpdateExcel.Enabled = true;
                                }
                                dtpEnd_ValueChanged(null, null);
                                dtpStart_ValueChanged(null, null);
                                xtraTabPage2.PageEnabled = false;
                                xtraTabPage1.PageEnabled = true; xtraTabPage3.PageEnabled = false;
                                xtraTabControl1.SelectedTabPage = xtraTabPage1;
                                xtraTabControl1.Enabled = true;
                                return;
                            }
                            else if (ds.Tables[0].Rows[0]["returnCode"].ToString() == "04")
                            {


                                //Common.setMessageBox(string.Format("Previous Reconciliation Found for Batch ID {0} Date from {1} and {2} with {} as closing balance.Process next batch", ds.Tables[1].Rows[0]["BatchCode"].ToString(), Convert.ToDateTime(ds.Tables[1].Rows[0]["StartDate"]), Convert.ToDateTime(ds.Tables[1].Rows[0]["EndDate"]), Convert.ToDecimal(ds.Tables[1].Rows[0]["CloseBal"])), Program.ApplicationName, 1);

                                xtraTabPage2.PageEnabled = false;
                                xtraTabPage1.PageEnabled = false; xtraTabPage3.PageEnabled = false; xtraTabControl1.SelectedTabPage = xtraTabPage1;
                                xtraTabControl1.Enabled = false;

                                dtexit = ds.Tables[2];

                                if (dtexit != null && dtexit.Rows.Count > 0)
                                {
                                    Dt = dtexit;
                                    gridControl1.DataSource = dtexit;
                                    gridView1.OptionsBehavior.Editable = false;
                                    AddComboboxRevenue();
                                    gridView1.Columns["DEBIT"].DisplayFormat.FormatType = FormatType.Numeric;
                                    gridView1.Columns["DEBIT"].DisplayFormat.FormatString = "n2";
                                    gridView1.Columns["CREDIT"].DisplayFormat.FormatType = FormatType.Numeric;
                                    gridView1.Columns["CREDIT"].DisplayFormat.FormatString = "n2";
                                    gridView1.Columns["BALANCE"].DisplayFormat.FormatType = FormatType.Numeric;
                                    gridView1.Columns["BALANCE"].DisplayFormat.FormatString = "n2";

                                    gridView1.Columns["CREDIT"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                                    gridView1.Columns["DEBIT"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                                    gridView1.Columns["BALANCE"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;

                                    gridView1.Columns["BALANCE"].SummaryItem.FieldName = "CREDIT";
                                    gridView1.Columns["BALANCE"].SummaryItem.DisplayFormat = "Total = {0:n}";

                                    gridView1.Columns["CREDIT"].SummaryItem.FieldName = "CREDIT";
                                    gridView1.Columns["CREDIT"].SummaryItem.DisplayFormat = "Total = {0:n}";

                                    gridView1.Columns["DEBIT"].SummaryItem.FieldName = "DEBIT";
                                    gridView1.Columns["DEBIT"].SummaryItem.DisplayFormat = "Total = {0:n}";

                                    colRevenue = gridView1.Columns["REVENUECODE"];
                                    colRevenue.ColumnEdit = repComboboxRevenue;

                                    colRevenue.Visible = true;

                                    //gridView1.OptionsView.ColumnAutoWidth = false;
                                    gridView1.OptionsView.ShowFooter = true;

                                    gridView1.BestFitColumns();

                                    label2.Text = dtexit.Rows.Count + " Rows of Records ";

                                    bttnReimport.Enabled = true; bttnImport.Enabled = false; //bttnUpdateExcel.Enabled = true;
                                }
                                else
                                {
                                    bttnReimport.Enabled = false; bttnImport.Enabled = true; //bttnUpdateExcel.Enabled = true;
                                }

                            }
                            else if (ds.Tables[0].Rows[0]["returnCode"].ToString() == "03")
                            {
                                DateTime result = DateTime.Today.Subtract(TimeSpan.FromDays(30));

                                dtpStart.Value = dtpStart.Value;

                                dtpStart.CustomFormat = "dd/MM/yyyy";

                                dtpEnd.CustomFormat = "dd/MM/yyyy";
                                dtpEnd.Enabled = true; dtpStart.Enabled = false;

                                dtexit = ds.Tables[2];

                                if (dtexit != null && dtexit.Rows.Count > 0)
                                {
                                    Dt = dtexit;
                                    gridControl1.DataSource = dtexit;
                                    gridView1.OptionsBehavior.Editable = false;
                                    AddComboboxRevenue();
                                    gridView1.Columns["DEBIT"].DisplayFormat.FormatType = FormatType.Numeric;
                                    gridView1.Columns["DEBIT"].DisplayFormat.FormatString = "n2";
                                    gridView1.Columns["CREDIT"].DisplayFormat.FormatType = FormatType.Numeric;
                                    gridView1.Columns["CREDIT"].DisplayFormat.FormatString = "n2";
                                    gridView1.Columns["BALANCE"].DisplayFormat.FormatType = FormatType.Numeric;
                                    gridView1.Columns["BALANCE"].DisplayFormat.FormatString = "n2";

                                    gridView1.Columns["CREDIT"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                                    gridView1.Columns["DEBIT"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                                    gridView1.Columns["BALANCE"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;

                                    gridView1.Columns["BALANCE"].SummaryItem.FieldName = "CREDIT";
                                    gridView1.Columns["BALANCE"].SummaryItem.DisplayFormat = "Total = {0:n}";

                                    gridView1.Columns["CREDIT"].SummaryItem.FieldName = "CREDIT";
                                    gridView1.Columns["CREDIT"].SummaryItem.DisplayFormat = "Total = {0:n}";

                                    gridView1.Columns["DEBIT"].SummaryItem.FieldName = "DEBIT";
                                    gridView1.Columns["DEBIT"].SummaryItem.DisplayFormat = "Total = {0:n}";

                                    colRevenue = gridView1.Columns["REVENUECODE"];
                                    colRevenue.ColumnEdit = repComboboxRevenue;

                                    colRevenue.Visible = true;

                                    //gridView1.OptionsView.ColumnAutoWidth = false;
                                    gridView1.OptionsView.ShowFooter = true;

                                    gridView1.BestFitColumns();

                                    label2.Text = dtexit.Rows.Count + " Rows of Records ";

                                    bttnReimport.Enabled = true; bttnImport.Enabled = false; //bttnUpdateExcel.Enabled = true;
                                }
                                else
                                {
                                    bttnReimport.Enabled = false; bttnImport.Enabled = true; //bttnUpdateExcel.Enabled = true;
                                }


                                xtraTabPage2.PageEnabled = false;
                                xtraTabPage1.PageEnabled = true; xtraTabPage3.PageEnabled = false; xtraTabControl1.SelectedTabPage = xtraTabPage1;
                                xtraTabControl1.Enabled = true;
                                label14.Text = String.Format("{0:N2}", ds.Tables[1].Rows[0]["openingbal"]);
                                label17.Text = ds.Tables[1].Rows[0]["Acctnumber"].ToString();
                            }
                            else if (ds.Tables[0].Rows[0]["returnCode"].ToString() == "05")
                            {
                                label14.Text = String.Format("{0:N2}", ds.Tables[1].Rows[0]["openingbal"]);
                                label17.Text = ds.Tables[1].Rows[0]["Acctnumber"].ToString();

                                xtraTabPage2.PageEnabled = false;
                                xtraTabPage1.PageEnabled = true; xtraTabPage3.PageEnabled = false;
                                xtraTabControl1.SelectedTabPage = xtraTabPage1;
                                xtraTabControl1.Enabled = true;

                                return;
                            }
                            else
                            {
                                //Common.setMessageBox(ds.Tables[0].Rows[0]["returnMessage"].ToString(), Program.ApplicationName, 2);

                                label14.Text = String.Format("{0:N2}", ds.Tables[1].Rows[0]["openingbal"]);
                                label17.Text = ds.Tables[1].Rows[0]["Acctnumber"].ToString();

                                DateTime result = DateTime.Today.Subtract(TimeSpan.FromDays(30));

                                dtpStart.Value = result;

                                dtpStart.CustomFormat = "dd/MM/yyyy";

                                dtpEnd.CustomFormat = "dd/MM/yyyy";
                                dtpEnd.Enabled = true; dtpStart.Enabled = false;

                                dtexit = ds.Tables[2];

                                if (dtexit != null && dtexit.Rows.Count > 0)
                                {
                                    Dt = dtexit;
                                    gridControl1.DataSource = dtexit;
                                    gridView1.OptionsBehavior.Editable = false;
                                    AddComboboxRevenue();
                                    gridView1.Columns["DEBIT"].DisplayFormat.FormatType = FormatType.Numeric;
                                    gridView1.Columns["DEBIT"].DisplayFormat.FormatString = "n2";
                                    gridView1.Columns["CREDIT"].DisplayFormat.FormatType = FormatType.Numeric;
                                    gridView1.Columns["CREDIT"].DisplayFormat.FormatString = "n2";
                                    gridView1.Columns["BALANCE"].DisplayFormat.FormatType = FormatType.Numeric;
                                    gridView1.Columns["BALANCE"].DisplayFormat.FormatString = "n2";

                                    gridView1.Columns["CREDIT"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                                    gridView1.Columns["DEBIT"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                                    gridView1.Columns["BALANCE"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;

                                    gridView1.Columns["BALANCE"].SummaryItem.FieldName = "CREDIT";
                                    gridView1.Columns["BALANCE"].SummaryItem.DisplayFormat = "Total = {0:n}";

                                    gridView1.Columns["CREDIT"].SummaryItem.FieldName = "CREDIT";
                                    gridView1.Columns["CREDIT"].SummaryItem.DisplayFormat = "Total = {0:n}";

                                    gridView1.Columns["DEBIT"].SummaryItem.FieldName = "DEBIT";
                                    gridView1.Columns["DEBIT"].SummaryItem.DisplayFormat = "Total = {0:n}";

                                    colRevenue = gridView1.Columns["REVENUECODE"];
                                    colRevenue.ColumnEdit = repComboboxRevenue;

                                    colRevenue.Visible = true;

                                    //gridView1.OptionsView.ColumnAutoWidth = false;
                                    gridView1.OptionsView.ShowFooter = true;

                                    gridView1.BestFitColumns();

                                    label2.Text = dtexit.Rows.Count + " Rows of Records ";

                                    bttnReimport.Enabled = true; bttnImport.Enabled = false; //bttnUpdateExcel.Enabled = true;
                                }
                                else
                                {
                                    bttnReimport.Enabled = false; bttnImport.Enabled = true; //bttnUpdateExcel.Enabled = true;
                                }

                                xtraTabPage2.PageEnabled = false;
                                xtraTabPage1.PageEnabled = true; xtraTabPage3.PageEnabled = false;
                                xtraTabControl1.SelectedTabPage = xtraTabPage1;
                                xtraTabControl1.Enabled = true;
                            }


                        }
                    }



                }
                catch (Exception ex)
                {
                    Tripous.Sys.ErrorBox(String.Format("{0}{1}", ex.Message, ex.StackTrace)); return;
                }
                finally
                {
                    SplashScreenManager.CloseForm(false);
                }


            }
        }

        void getCompare()
        {
            try
            {


                using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
                {
                    connect.Open();

                    _command = new SqlCommand("CompareBankStatementCollection", connect) { CommandType = CommandType.StoredProcedure };
                    _command.Parameters.Add(new SqlParameter("@Periods", SqlDbType.Int)).Value = label25.Text.Trim();
                    _command.CommandTimeout = 0;
                    using (System.Data.DataSet ds = new System.Data.DataSet())
                    {
                        ds.Clear();
                        dbmatched = new DataTable(); dbmissing = new DataTable();
                        dbmissingpay = new DataTable();
                        dbCrDebit = new DataTable(); dbCrDebit.Clear();
                        dbmatched.Clear(); dbmissing.Clear(); dbmissingpay.Clear();

                        adp = new SqlDataAdapter(_command);
                        adp.Fill(ds);
                        //Dts = ds.Tables[0];
                        connect.Close();

                        //dtResult = ds;

                        if (ds.Tables[0].Rows[0]["returnCode"].ToString() == "00")
                        {
                            dbmatched = ds.Tables[1];

                            dbmissing = ds.Tables[2];

                            dbmissingpay = ds.Tables[3];


                            dtcollnot = ds.Tables[6]; dtBanNotMatch = ds.Tables[7];

                            if ((dtcollnot != null & dtcollnot.Rows.Count > 0) || (dtBanNotMatch != null & dtBanNotMatch.Rows.Count > 0))
                            {
                                bttnMatched.Enabled = true;
                            }

                            label6.Text = String.Format("{0:N2}", ds.Tables[4].Rows[0]["ClosingBal"]);

                            //dbCrDebit = ds.Tables[5];
                        }
                        else
                        {
                            Common.setMessageBox(ds.Tables[0].Rows[0]["returnMessage"].ToString(), Program.ApplicationName, 2);
                            return;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Tripous.Sys.ErrorBox(String.Format("{0}{1}", ex.StackTrace, ex.Message));
                return;
            }


            if (dbmissing != null && dbmissing.Rows.Count > 0)
            {
                gridControl2.DataSource = dbmissing;

            }

            if (dbmatched != null && dbmatched.Rows.Count > 0)
            {
                gridControl3.DataSource = dbmatched;
                //gridView4.OptionsBehavior.Editable = false;
                //gridView4.Columns["Amount"].DisplayFormat.FormatType = FormatType.Numeric;
                //gridView4.Columns["Amount"].DisplayFormat.FormatString = "###,###,###,##0.00##;(###,###,###,##0.00##)";
                //gridView4.Columns["Amount"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                //gridView4.Columns["DATE"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
                //gridView4.Columns["DATE"].DisplayFormat.FormatString = "dd/MM/yyyy";
                //gridView4.Columns["Amount"].SummaryItem.FieldName = "Amount";
                //gridView4.Columns["Amount"].SummaryItem.DisplayFormat = "Total = {0:###,###,###,##0.00##;(###,###,###,##0.00##)}";
                gridView4.Columns["paymentref"].Visible = false;
                //gridView4.OptionsView.ColumnAutoWidth = false;
                //gridView4.OptionsView.ShowFooter = true;

                //gridView4.BestFitColumns();
            }

            if (dbmissingpay != null && dbmissingpay.Rows.Count > 0)
            {
                gridControl4.DataSource = dbmissingpay;
                //gridView5.OptionsBehavior.Editable = false;
                gridView5.Columns["paymentref"].Visible = false;
                //gridView5.Columns["Amount"].DisplayFormat.FormatType = FormatType.Numeric;
                //gridView5.Columns["Amount"].DisplayFormat.FormatString = "###,###,###,##0.0##;(###,###,###,##0.0##)";
                //gridView5.Columns["Amount"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                //gridView5.Columns["DATE"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
                //gridView5.Columns["DATE"].DisplayFormat.FormatString = "dd/MM/yyyy";
                //gridView5.Columns["Amount"].SummaryItem.FieldName = "Amount";
                //gridView5.Columns["Amount"].SummaryItem.DisplayFormat = "Total = {0:###,###,###,##0.0##;(###,###,###,##0.0##)}";
                ////gridView5.Columns["bsid"].Visible = false;
                //gridView5.OptionsView.ColumnAutoWidth = false;
                //gridView5.OptionsView.ShowFooter = true;

                //gridView5.BestFitColumns();
            }
            else
            {
                gridControl4.DataSource = null;
            }

            refillgrid();
            //bttnSave.Enabled = true;
            bttncancelbs.Enabled = true;
        }

        void ClearForm()
        {
            dtpStart.Enabled = false; dtpEnd.Enabled = true; cboBank.Enabled = true; cboBank.SelectedIndex = -1; label42.Text = string.Empty; label43.Text = string.Empty; label14.Text = string.Empty; label6.Text = string.Empty; label17.Text = string.Empty; label42.Text = "+"; label43.Text = "+"; label14.Text = "+"; label6.Text = "+"; label17.Text = "+";
            gridControl1.DataSource = null; gridView1.Columns.Clear(); gridControl2.DataSource = null; gridView3.Columns.Clear(); gridControl3.DataSource = null; gridView4.Columns.Clear(); gridControl4.DataSource = null; gridView5.Columns.Clear(); gridControl6.DataSource = null; gridView8.Columns.Clear(); gridControl7.DataSource = null; gridView9.Columns.Clear(); gridControl8.DataSource = null; gridView10.Columns.Clear();

            clearRec(); xtraTabControl1.Enabled = false; xtraTabControl1.SelectedTabPage = xtraTabPage1;
        }
        void openForm()
        {

            //using (FrmFinanicalYear fyear = new FrmFinanicalYear("Transaction"))
            //{
            //    var result = fyear.ShowDialog();
            //    if (result != DialogResult.OK)
            //        //tsbClose.PerformClick();
            //        MDIMains.publicMDIParent.RemoveControls();
            //}
        }
        bool deletebatch()
        {
            bool bRespone = false;

            try
            {
                using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
                {
                    connect.Open();

                    _command = new SqlCommand("doDeleteBatch", connect) { CommandType = CommandType.StoredProcedure };
                    _command.Parameters.Add(new SqlParameter("@batch", SqlDbType.Char)).Value = label22.Text;
                    _command.CommandTimeout = 0;
                    using (System.Data.DataSet ds = new System.Data.DataSet())
                    {
                        ds.Clear();
                        adp = new SqlDataAdapter(_command);
                        adp.Fill(ds);
                        //Dts = ds.Tables[0];
                        connect.Close();


                        if (ds.Tables[0].Rows[0]["returnCode"].ToString() == "00")
                        {
                            bRespone = true;
                        }
                        else
                            bRespone = false;
                    }
                }

            }
            catch (Exception ex)
            {
                Tripous.Sys.ErrorBox(String.Format("{0}...{1}", ex.Message, ex.StackTrace));
            }

            return bRespone;
        }

        int GetBankStageID()
        {
            int stageID = 0;

            using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
            {
                connect.Open();
                _command = new SqlCommand("DoCheckStageID", connect) { CommandType = CommandType.StoredProcedure };
                _command.Parameters.Add(new SqlParameter("@periodid", SqlDbType.Int)).Value = Convert.ToInt32(label25.Text);

                using (System.Data.DataSet ds = new System.Data.DataSet())
                {
                    ds.Clear();
                    adp = new SqlDataAdapter(_command);
                    adp.Fill(ds);
                    connect.Close();

                    if (ds.Tables[0].Rows[0]["returnCode"].ToString() != "00")
                    {
                        Common.setMessageBox(ds.Tables[0].Rows[0]["returnMessage"].ToString(), Program.ApplicationName, 2);
                        return 0;
                    }
                    else
                    {
                        stageID = Convert.ToInt32(ds.Tables[1].Rows[0]["stageID"].ToString());
                    }

                }
            }
            return stageID;
        }

        decimal GetOpeningBalance(int accountID, string bankCode, int periodID)
        {
            decimal openbal = 0m;
            //string qury = string.Format("SELECT OpeningBal FROM Reconciliation.tblBankBalance WHERE BankAccountID={0} AND BankShortCode='{1}' AND FinancialperiodID={2}", accountID, bankCode, periodID);
            string qury = string.Format("SELECT OpeningBal FROM Reconciliation.tblBankBalance WHERE PeriodID={0}", label25.Text);
            var rec = (new Logic()).ExecuteScalar(qury);
            if (rec != null)
            {
                openbal = Convert.ToDecimal(rec);
            }
            return openbal;
        }

        decimal GetclosingBalance(int accountID, string bankCode, int periodID)
        {
            decimal closebal = 0m;
            string qury = string.Format("SELECT isnull(ClosingBalCal, 0) FROM Reconciliation.tblBankBalance WHERE PeriodID={0}", label25.Text);
            var rec = (new Logic()).ExecuteScalar(qury);
            if (rec != null)
            {
                closebal = Convert.ToDecimal(rec);
            }
            return closebal;
        }

        public void setDBComboBoxPeriod()
        {
            using (var ds = new System.Data.DataSet())
            {

                using (SqlDataAdapter ada = new SqlDataAdapter((string)@"
SELECT  PeriodID ,
        Description ,
        tblBankAccount.BankAccountID ,
        AccountNumber ,
        tblFinancialperiod.FinancialperiodID ,
        tblBank.BankShortCode ,
        BankName,Months+','+Year AS Period,tblReconciliatioPeriod.StartDate,tblReconciliatioPeriod.EndDate
FROM    Reconciliation.tblReconciliatioPeriod
        INNER JOIN Collection.tblBank ON tblBank.BankShortCode = tblReconciliatioPeriod.BankShortCode
        INNER JOIN Reconciliation.tblBankAccount ON tblBankAccount.BankAccountID = tblReconciliatioPeriod.BankAccountID
                                                    AND tblBankAccount.BankShortCode = tblBank.BankShortCode
        INNER JOIN Reconciliation.tblFinancialperiod ON tblFinancialperiod.FinancialperiodID = tblReconciliatioPeriod.FinancialperiodID WHERE (IsPeriodClosed IS NULL OR IsPeriodClosed=0)
", Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }

                Dts = ds.Tables[0];
            }

            Common.setComboList(cboRecPeriod, Dts, "PeriodID", "Description");

            cboRecPeriod.SelectedIndex = -1;


        }

        public DataTable ConvertToDataTable<T>(IList<T> data)
        {
            var properties = TypeDescriptor.GetProperties(typeof(T));
            var table = new DataTable();
            foreach (PropertyDescriptor prop in properties)
            {
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            }
            foreach (T item in data)
            {
                var row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                {
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                }
                table.Rows.Add(row);
            }
            return table;
        }

        void reCalMatched()
        {
            if ((dtMatchedManual != null && dtMatchedManual.Rows.Count > 0) || (dbmatched != null && dbmatched.Rows.Count > 0) || (dbmissing != null && dbmissing.Rows.Count > 0) || (dbmissingpay != null && dbmissingpay.Rows.Count > 0))
            {
                foreach (DataRow row in dtMatchedManual.Rows)
                {
                    int? vasl = row["BSId"] == DBNull.Value ? (int?)null : Convert.ToInt32(row["BSId"]);

                    string val = row["PaymentRef"] as string;

                    var revRow = dbmissing.Select($"bsid = {vasl}").SingleOrDefault();

                    dbmissing.Rows.Remove(revRow);

                    var Rowrev = dbmissingpay.Select($"paymentref = '{val}'").SingleOrDefault();
                    dbmissingpay.Rows.Remove(Rowrev);

                }

                foreach (DataRow row in dtMatchedManual.Rows)
                {
                    //int? vasl = row["BSId"] == DBNull.Value ? (int?)null : Convert.ToInt32(row["BSId"]);

                    //string val = row["PaymentRef"] as string;

                    //DateTime dtdate = Convert.ToDateTime(row["Date"]);

                    //decimal dbAmount = Convert.ToDecimal(row["Amount"]);

                    dbmatched.ImportRow(row);
                }

                //dtMatchedManual.AcceptChanges();


                gridControl2.DataSource = null;
                gridControl3.DataSource = null;
                gridControl4.DataSource = null;

                dbmatched.AcceptChanges();

                dbmissingpay.AcceptChanges();

                dbmissing.AcceptChanges();

                gridControl2.DataSource = dbmissing;

                gridControl3.DataSource = dbmatched;

                gridControl4.DataSource = dbmissingpay;

                refillgrid();
            }

        }

        void refillgrid()
        {

            if (dbmissing != null && dbmissing.Rows.Count > 0)
            {
                gridControl2.DataSource = dbmissing;
                gridView3.OptionsBehavior.Editable = false;
                gridView3.Columns["Amount"].DisplayFormat.FormatType = FormatType.Numeric;
                gridView3.Columns["Amount"].DisplayFormat.FormatString = "###,###,###,##0.00##;(###,###,###,##0.00##)";
                gridView3.Columns["Amount"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                gridView3.Columns["DATE"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
                gridView3.Columns["DATE"].DisplayFormat.FormatString = "dd/MM/yyyy";
                gridView3.Columns["Amount"].SummaryItem.FieldName = "Amount";
                gridView3.Columns["Amount"].SummaryItem.DisplayFormat = "Total = {0:###,###,###,##0.00##;(###,###,###,##0.00##)}";
                gridView3.Columns["bsid"].Visible = false;
                gridView3.OptionsView.ColumnAutoWidth = false;
                gridView3.OptionsView.ShowFooter = true;
                gridView3.BestFitColumns();

            }

            if (dbmatched != null && dbmatched.Rows.Count > 0)
            {
                gridControl3.DataSource = dbmatched;
                //gridControl3.DataSource = dbmatched;
                gridView4.OptionsBehavior.Editable = false;
                gridView4.Columns["Amount"].DisplayFormat.FormatType = FormatType.Numeric;
                gridView4.Columns["Amount"].DisplayFormat.FormatString = "###,###,###,##0.00##;(###,###,###,##0.00##)";
                gridView4.Columns["Amount"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                gridView4.Columns["DATE"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
                gridView4.Columns["DATE"].DisplayFormat.FormatString = "dd/MM/yyyy";
                gridView4.Columns["Amount"].SummaryItem.FieldName = "Amount";
                gridView4.Columns["Amount"].SummaryItem.DisplayFormat = "Total = {0:###,###,###,##0.00##;(###,###,###,##0.00##)}";
                gridView4.Columns["bsid"].Visible = false;
                gridView4.OptionsView.ColumnAutoWidth = false;
                gridView4.OptionsView.ShowFooter = true;

                gridView4.BestFitColumns();
            }

            if (dbmissingpay != null && dbmissingpay.Rows.Count > 0)
            {
                gridControl4.DataSource = dbmissingpay;
                gridView5.OptionsBehavior.Editable = false;
                //gridView5.Columns["paymentref"].Visible = false;
                gridView5.Columns["Amount"].DisplayFormat.FormatType = FormatType.Numeric;
                gridView5.Columns["Amount"].DisplayFormat.FormatString = "###,###,###,##0.0##;(###,###,###,##0.0##)";
                gridView5.Columns["Amount"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                gridView5.Columns["DATE"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
                gridView5.Columns["DATE"].DisplayFormat.FormatString = "dd/MM/yyyy";
                gridView5.Columns["Amount"].SummaryItem.FieldName = "Amount";
                gridView5.Columns["Amount"].SummaryItem.DisplayFormat = "Total = {0:###,###,###,##0.0##;(###,###,###,##0.0##)}";
                //gridView5.Columns["bsid"].Visible = false;
                gridView5.OptionsView.ColumnAutoWidth = false;
                gridView5.OptionsView.ShowFooter = true;

                gridView5.BestFitColumns();
            }
            else
            {
                gridControl4.DataSource = null;
            }

            
        }

    }

}
