using BankReconciliation.Class;
using BankReconciliation.Report;
using DevExpress.Utils;
using DevExpress.XtraReports.UI;
using DevExpress.XtraSplashScreen;
using System;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;
using TaxSmartSuite.Class;

namespace BankReconciliation.Forms
{
    public partial class FrmYearly : Form
    {

        DataTable Dt;
        private DataTable dbmatched = null;
        DataTable dbCrDebit = null;
        private DataTable dbmissing = null;
        private DataTable dbmissingpay = null;
        System.Data.DataSet DtSet;

        public static FrmYearly publicStreetGroup;

        private SqlCommand _command; private SqlDataAdapter adp; private string BatchNumber;

        private System.Data.OleDb.OleDbDataAdapter MyCommand;

        private OleDbConnection MyConnection;

        string filenamesopen, acctnumber; bool boolIsUpdate2;

        private bool isFirstGrid = true;
        private bool isFirstGrid2 = true;
        private bool isRecord = false;
        private bool isRecord2 = false;
        bool isComplete = false; private bool Isbank = false;
        public FrmYearly()
        {
            SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

            InitializeComponent();

            publicStreetGroup = this;

            setImages();

            ToolStripEvent();

            Load += OnFormLoad;

            OnFormLoad(null, null);

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
                //try
                //{
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
                //                else
                //                    MDIMains.publicMDIParent.RemoveControls();
                //            }
                //        }
                //    }


                //}
                //catch (Exception ex)
                //{
                //    Tripous.Sys.ErrorBox(ex.StackTrace + ex.Message);
                //    return;
                //}
                //finally
                //{

                //}

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
            isRecord2 = false;

            setDBComboBox();

            bttnImport.Click += bttnImport_Click;

            bttnUpdateExcel.Click += bttnUpdateExcel_Click; txtOpen.Leave += txtOpen_Leave;

            cboBank.SelectedIndexChanged += cboBank_SelectedIndexChanged; bttnNextImp.Click += bttnNextImp_Click;

            cboBank.KeyPress += cboBank_KeyPress; bttncompare.Click += bttncompare_Click; bttnSave.Click += bttnSave_Click;

            bttnPreview.Click += bttnPreview_Click;
        }

        void bttnPreview_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(radioGroup1.EditValue) == 0)
            {
                using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
                {
                    connect.Open();
                    _command = new SqlCommand("getReportYear", connect) { CommandType = CommandType.StoredProcedure };
                    _command.Parameters.Add(new SqlParameter("@bankcode", SqlDbType.VarChar)).Value = cboBank.SelectedValue;
                    _command.Parameters.Add(new SqlParameter("@startdate", SqlDbType.VarChar)).Value = string.Format("{0:yyyy/MM/dd 00:00:00}", dtpStart.Value);
                    _command.Parameters.Add(new SqlParameter("@enddate", SqlDbType.VarChar)).Value = string.Format("{0:yyyy/MM/dd 23:59:59}", dtpEnd.Value);
                    _command.Parameters.Add(new SqlParameter("@ReportType", SqlDbType.VarChar)).Value = "Bank";
                    _command.CommandTimeout = 0;

                    using (System.Data.DataSet ds = new System.Data.DataSet())
                    {
                        ds.Clear();
                        adp = new SqlDataAdapter(_command);
                        adp.Fill(ds, "table");
                        //Dts = ds.Tables[0];
                        connect.Close();

                        if (ds.Tables[0].Rows[0]["returnCode"].ToString() == "00")
                        {
                            if (ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0)
                            {
                                xtraRepyearly repyear = new xtraRepyearly();

                                var replist = (from DataRow row in ds.Tables[1].Rows
                                               select new Dataset.Reportyear
                                               {
                                                   Date = Convert.ToDateTime(row["Date"]),
                                                   Amount = Convert.ToDecimal(row["Credit"]),
                                                   PaymentRef = row["RevenueCode"] as string
                                               }
                                 ).ToList();

                                repyear.xrLabel11.Text = string.Format("Bank Name: {0}", cboBank.Text.Trim());
                                repyear.xrLabel12.Text =
                                    string.Format("List of Transactions in Bank Statement not in Reems between {0:dd/MM/yyyy} and {1:dd/MM/yyyy}", dtpStart.Value, dtpEnd.Value);
                                repyear.DataSource = replist;

                                repyear.ShowPreviewDialog();

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
                    _command = new SqlCommand("getReportYear", connect) { CommandType = CommandType.StoredProcedure };
                    _command.Parameters.Add(new SqlParameter("@bankcode", SqlDbType.VarChar)).Value = cboBank.SelectedValue;
                    _command.Parameters.Add(new SqlParameter("@startdate", SqlDbType.VarChar)).Value = string.Format("{0:yyyy/MM/dd 00:00:00}", dtpStart.Value);
                    _command.Parameters.Add(new SqlParameter("@enddate", SqlDbType.VarChar)).Value = string.Format("{0:yyyy/MM/dd 23:59:59}", dtpEnd.Value);
                    _command.Parameters.Add(new SqlParameter("@ReportType", SqlDbType.VarChar)).Value = "Both";
                    _command.CommandTimeout = 0;

                    using (System.Data.DataSet ds = new System.Data.DataSet())
                    {
                        ds.Clear();
                        adp = new SqlDataAdapter(_command);
                        adp.Fill(ds, "table");
                        //Dts = ds.Tables[0];
                        connect.Close();

                        if (ds.Tables[0].Rows[0]["returnCode"].ToString() == "00")
                        {
                            if (ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0)
                            {
                                xtraRepyearly repyear = new xtraRepyearly();

                                var replist = (from DataRow row in ds.Tables[1].Rows
                                               select new Dataset.Reportyear
                                               {
                                                   Date = Convert.ToDateTime(row["Date"]),
                                                   Amount = Convert.ToDecimal(row["Credit"]),
                                                   PaymentRef = row["RevenueCode"] as string
                                               }
                                 ).ToList();

                                repyear.xrLabel11.Text = string.Format("Bank Name: {0}", cboBank.Text.Trim());
                                repyear.xrLabel12.Text =
                                    string.Format("List of Transactions in both Bank Statement and Reems between {0:dd/MM/yyyy} and {1:dd/MM/yyyy}", dtpStart.Value, dtpEnd.Value);
                                repyear.DataSource = replist;

                                repyear.ShowPreviewDialog();

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
                    _command = new SqlCommand("getReportYear", connect) { CommandType = CommandType.StoredProcedure };
                    _command.Parameters.Add(new SqlParameter("@bankcode", SqlDbType.VarChar)).Value = cboBank.SelectedValue;
                    _command.Parameters.Add(new SqlParameter("@startdate", SqlDbType.VarChar)).Value = string.Format("{0:yyyy/MM/dd 00:00:00}", dtpStart.Value);
                    _command.Parameters.Add(new SqlParameter("@enddate", SqlDbType.VarChar)).Value = string.Format("{0:yyyy/MM/dd 23:59:59}", dtpEnd.Value);
                    _command.Parameters.Add(new SqlParameter("@ReportType", SqlDbType.VarChar)).Value = "Reems";
                    _command.CommandTimeout = 0;

                    using (System.Data.DataSet ds = new System.Data.DataSet())
                    {
                        ds.Clear();
                        adp = new SqlDataAdapter(_command);
                        adp.Fill(ds, "table");
                        //Dts = ds.Tables[0];
                        connect.Close();

                        if (ds.Tables[0].Rows[0]["returnCode"].ToString() == "00")
                        {
                            if (ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0)
                            {
                                xtraRepyearly repyear = new xtraRepyearly();

                                var replist = (from DataRow row in ds.Tables[1].Rows
                                               select new Dataset.Reportyear
                                               {
                                                   Date = Convert.ToDateTime(row["PaymentDate"]),
                                                   Amount = Convert.ToDecimal(row["Amount"]),
                                                   PaymentRef = row["PaymentRefNumber"] as string
                                               }
                                 ).ToList();

                                repyear.xrLabel11.Text = string.Format("Bank Name: {0}", cboBank.Text.Trim());
                                repyear.xrLabel12.Text =
                                    string.Format("List of Transactions in Reems not in Bank Statements between {0:dd/MM/yyyy} and {1:dd/MM/yyyy}", dtpStart.Value, dtpEnd.Value);
                                repyear.DataSource = replist;

                                repyear.ShowPreviewDialog();

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

        void bttnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty((string)(cboBank.SelectedValue)))
            {
                Common.setEmptyField("BanK Name", Program.ApplicationName);
                cboBank.Focus();
                return;
            }
            else if (dbmissing == null || dbmatched == null || dbmissingpay == null || dbCrDebit == null)
            {
                Common.setEmptyField("No Comparing Data Result Yet...", Program.ApplicationName);
                return;
            }
            else if (string.IsNullOrEmpty((string)(txtOpen.EditValue.ToString())))
            {
                Common.setEmptyField("Opening Balance", Program.ApplicationName);
                txtOpen.Focus(); return;
            }
            else
            {
                try
                {
                    SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

                    //insert into paydirect table
                    using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
                    {
                        connect.Open();
                        _command = new SqlCommand("InsertPayDirectBankYear", connect) { CommandType = CommandType.StoredProcedure };
                        _command.Parameters.Add(new SqlParameter("@bankcode", SqlDbType.VarChar)).Value = cboBank.SelectedValue;
                        _command.Parameters.Add(new SqlParameter("@startdate", SqlDbType.VarChar)).Value = string.Format("{0:yyyy/MM/dd}", dtpStart.Value);
                        _command.Parameters.Add(new SqlParameter("@enddate", SqlDbType.VarChar)).Value = string.Format("{0:yyyy/MM/dd}", dtpEnd.Value);
                        _command.Parameters.Add(new SqlParameter("@pTransaction", SqlDbType.Structured)).Value = dbmissingpay;
                        //_command.Parameters.Add(new SqlParameter("@Periods", SqlDbType.Int)).Value = label22.Text;
                        _command.CommandTimeout = 0;
                        using (System.Data.DataSet ds = new System.Data.DataSet())
                        {
                            ds.Clear();
                            adp = new SqlDataAdapter(_command);
                            adp.Fill(ds);
                            //Dts = ds.Tables[0];
                            connect.Close();

                        }
                    }

                    //bank statement
                    using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
                    {
                        connect.Open();
                        _command = new SqlCommand("InsertBankPayDirectyear", connect) { CommandType = CommandType.StoredProcedure };
                        _command.Parameters.Add(new SqlParameter("@bankcode", SqlDbType.VarChar)).Value = cboBank.SelectedValue;
                        _command.Parameters.Add(new SqlParameter("@startdate", SqlDbType.VarChar)).Value = string.Format("{0:yyyy/MM/dd}", dtpStart.Value);
                        _command.Parameters.Add(new SqlParameter("@enddate", SqlDbType.VarChar)).Value = string.Format("{0:yyyy/MM/dd}", dtpEnd.Value);
                        _command.Parameters.Add(new SqlParameter("@pTransaction", SqlDbType.Structured)).Value = dbmissing;
                        //_command.Parameters.Add(new SqlParameter("@Periods", SqlDbType.Int)).Value = label22.Text;
                        _command.CommandTimeout = 0;
                        using (System.Data.DataSet ds = new System.Data.DataSet())
                        {
                            ds.Clear();
                            adp = new SqlDataAdapter(_command);
                            adp.Fill(ds);
                            //Dts = ds.Tables[0];
                            connect.Close();


                        }


                    }

                    //insert into both table(bank & paydirect)
                    using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
                    {
                        connect.Open();
                        _command = new SqlCommand("InsertBothBankPayDirectyear", connect) { CommandType = CommandType.StoredProcedure };
                        _command.Parameters.Add(new SqlParameter("@bankcode", SqlDbType.VarChar)).Value = cboBank.SelectedValue;
                        _command.Parameters.Add(new SqlParameter("@startdate", SqlDbType.VarChar)).Value = string.Format("{0:yyyy/MM/dd}", dtpStart.Value);
                        _command.Parameters.Add(new SqlParameter("@enddate", SqlDbType.VarChar)).Value = string.Format("{0:yyyy/MM/dd}", dtpEnd.Value);
                        _command.Parameters.Add(new SqlParameter("@pTransaction", SqlDbType.Structured)).Value = dbmatched;
                        //_command.Parameters.Add(new SqlParameter("@Periods", SqlDbType.Int)).Value = label22.Text;
                        _command.CommandTimeout = 0;
                        using (System.Data.DataSet ds = new System.Data.DataSet())
                        {
                            ds.Clear();
                            adp = new SqlDataAdapter(_command);
                            adp.Fill(ds);
                            //Dts = ds.Tables[0];
                            connect.Close();

                            if (ds.Tables[0].Rows[0]["returnCode"].ToString() != "00")
                            {
                                Common.setMessageBox(ds.Tables[0].Rows[0]["returnMessage"].ToString(), Program.ApplicationName, 2);
                                //CalClose();
                            }
                            else
                            {
                                Common.setMessageBox(ds.Tables[0].Rows[0]["returnMessage"].ToString(), Program.ApplicationName, 1);
                            }


                        }
                    }
                    groupBox4.Enabled = true;

                }
                catch (Exception ex)
                {
                    Tripous.Sys.ErrorBox(String.Format("{0}{1}", ex.StackTrace, ex.Message));
                    return;
                }
                finally
                {
                    SplashScreenManager.CloseForm(false);
                }
            }
        }

        void bttncompare_Click(object sender, EventArgs e)
        {
            gridControl2.DataSource = null; gridControl4.DataSource = null; gridControl3.DataSource = null;
            gridView3.Columns.Clear(); gridView4.Columns.Clear(); gridView5.Columns.Clear();

            SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

            if (string.IsNullOrEmpty((string)(cboBank.SelectedValue)))
            {
                Common.setEmptyField("BanK Name", Program.ApplicationName);
                cboBank.Focus();
                return;
            }
            else if (string.IsNullOrEmpty((string)(cboAccount.SelectedValue.ToString())))
            {
                Common.setEmptyField("Account Number", Program.ApplicationName);
                cboAccount.Focus();
                return;
            }
            else if (string.IsNullOrEmpty((string)(txtOpen.EditValue.ToString())))
            {
                Common.setEmptyField("Opening Balance", Program.ApplicationName);
                txtOpen.Focus(); return;
            }
            else
            {
                try
                {
                    using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
                    {
                        connect.Open();

                        _command = new SqlCommand("CompareBankStatementCollectionyear", connect) { CommandType = CommandType.StoredProcedure };
                        _command.Parameters.Add(new SqlParameter("@bankcode", SqlDbType.VarChar)).Value = cboBank.SelectedValue;
                        _command.Parameters.Add(new SqlParameter("@startdate", SqlDbType.VarChar)).Value = string.Format("{0:yyyy/MM/dd 00:00:00}", dtpStart.Value);
                        _command.Parameters.Add(new SqlParameter("@enddate", SqlDbType.VarChar)).Value = string.Format("{0:yyyy/MM/dd 23:59:59}", dtpEnd.Value);
                        _command.Parameters.Add(new SqlParameter("@AccountId", SqlDbType.Int)).Value = Convert.ToInt32(cboAccount.SelectedValue);
                        _command.Parameters.Add(new SqlParameter("@openinBal", SqlDbType.Money)).Value = txtOpen.EditValue;
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

                                label6.Text = String.Format("{0:N2}", ds.Tables[4].Rows[0]["ClosingBal"]);

                                //dbCrDebit = ds.Tables[5];

                                if (ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0)
                                {
                                    gridControl3.DataSource = ds.Tables[1];
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

                                if (ds.Tables[2] != null && ds.Tables[2].Rows.Count > 0)
                                {
                                    gridControl2.DataSource = ds.Tables[2];
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

                                {
                                }

                                if (ds.Tables[3] != null && ds.Tables[3].Rows.Count > 0)
                                {
                                    gridControl4.DataSource = ds.Tables[3];
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
                            }
                        }
                    }
                    bttnSave.Enabled = true;
                }
                catch (Exception ex)
                {
                    Tripous.Sys.ErrorBox(String.Format("{0}{1}", ex.StackTrace, ex.Message));
                    return;
                }

            }
        }

        void bttnNextImp_Click(object sender, EventArgs e)
        {
            //xtraTabControl1.TabPages = xtraTabPage3;
            dtpEnd.Enabled = false; dtpStart.Enabled = false; cboBank.Enabled = false;
            xtraTabPage3.PageEnabled = true;
            xtraTabPage1.PageEnabled = false;
            xtraTabControl1.SelectedTabPage = xtraTabPage3;
        }

        void txtOpen_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtOpen.Text))
            {
                return;
            }
            else
            {
                using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
                {
                    connect.Open();
                    _command = new SqlCommand("doGetOpeningAccountyear", connect)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    _command.Parameters.Add(new SqlParameter("@bankcode", SqlDbType.VarChar)).Value =
                        cboBank.SelectedValue;
                    _command.Parameters.Add(new SqlParameter("@OpeningBal", SqlDbType.Int)).Value = txtOpen.EditValue;
                    _command.Parameters.Add(new SqlParameter("@AccountId", SqlDbType.Int)).Value =
                        Convert.ToInt32(cboAccount.SelectedValue);
                    _command.Parameters.Add(new SqlParameter("@enddate", SqlDbType.VarChar)).Value =
                        string.Format("{0:yyyy/MM/dd 23:59:59}", dtpEnd.Value);
                    _command.Parameters.Add(new SqlParameter("@startdate", SqlDbType.VarChar)).Value =
                        string.Format("{0:yyyy/MM/dd 23:59:59}", dtpStart.Value);
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
                            if (ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0)
                            {
                                gridControl1.DataSource = ds.Tables[1];
                                gridView1.OptionsBehavior.Editable = false;

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

                                //gridView1.OptionsView.ColumnAutoWidth = false;
                                gridView1.OptionsView.ShowFooter = true;

                                gridView1.BestFitColumns();



                                label2.Text = string.Format("{0} Rows of Records ", ds.Tables[1].Rows.Count);
                            }
                        }
                    }
                }

            }
        }

        void bttnUpdateExcel_Click(object sender, EventArgs e)
        {
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
            else if (Dt == null)
            {
                Common.setMessageBox("Please Upload Bank Statement", Program.ApplicationName, 2); return;
            }
            else if (string.IsNullOrEmpty((string)(txtOpen.EditValue.ToString())))
            {
                Common.setMessageBox("Opening Balance", Program.ApplicationName, 2); return;
            }
            else
            {
                DialogResult results = MessageBox.Show("Is Total Debit and Credit Import Correct ?", Program.ApplicationName, MessageBoxButtons.YesNo);

                if (results == DialogResult.Yes)
                {
                    try
                    {
                        SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

                        //string qry = (String.Format("SELECT BSDate AS DATE, ISNULL(Debit, 0) AS DEBIT,ISNULL(Credit, 0) AS CREDIT,Balance AS BALANCE ,RevenueCode ,PayerName  from Reconciliation.tblBankStatement WHERE BankShortCode = '{0}'  AND FinancialperiodID ='{1}' AND CONVERT(VARCHAR(10),StartDate,103)='{2}' AND CONVERT(VARCHAR(10),EndDate,103)='{3}' AND BankAccountID='{4}'", cboBank.SelectedValue, label22.Text.Trim(), string.Format("{0:dd/MM/yyy}", dtpStart.Value), string.Format("{0:dd/MM/yyy}", dtpEnd.Value), Convert.ToInt32(cboAccount.SelectedValue)));

                        //DataTable dts = (new Logic()).getSqlStatement(qry).Tables[0];

                        //if (dts != null && dts.Rows.Count > 0)
                        //{
                        //    //calling frmcopmare

                        //    DialogResult result = MessageBox.Show(string.Format("Bank Transactions Already exist for this period {0} and {1}. Are you sure to continue.....", string.Format("{0:dd/MM/yyy}", dtpStart.Value), string.Format("{0:dd/MM/yyy}", dtpEnd.Value)), Program.ApplicationName, MessageBoxButtons.YesNo);

                        //    if (result == DialogResult.Yes)
                        //    {
                        //        //DialogResult = System.Windows.Forms.DialogResult.OK;

                        //        using (FrmCompareValue compfrm = new FrmCompareValue(dts, Dt))
                        //        {
                        //            var res = compfrm.ShowDialog();

                        //            if (res == System.Windows.Forms.DialogResult.OK)
                        //            {
                        //                UpdateBankstatement((string)cboBank.SelectedValue, compfrm.dtEqual);
                        //                bttnNextImp.Enabled = true;
                        //            }
                        //        }
                        //    }
                        //    else
                        //        return;

                        //}
                        //else
                        //{
                        UpdateBankstatement((string)cboBank.SelectedValue, Dt);

                        bttnNextImp.Enabled = true;
                        //}

                    }
                    catch (Exception ex)
                    {

                        Common.setMessageBox(ex.StackTrace + ex.Message, "Error", 2);
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

                cboBank.Enabled = false;
                MessageBoxManager.Unregister();
                //MessageBoxManager.OK = "Excel 2003";
                MessageBoxManager.No = "Excel 2007";
                MessageBoxManager.Yes = "Excel 2003";
                MessageBoxManager.Register();

                DialogResult result = MessageBox.Show("Select Excel File Type", "Import Statement", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)//excele 2003
                {
                    try
                    {
                        SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

                        gridControl1.DataSource = null;

                        using (OpenFileDialog openFileDialogCSV = new OpenFileDialog() { InitialDirectory = Application.ExecutablePath, Filter = "Excel files (*.xls)|*.xls|All files (*.*)|*.*", FilterIndex = 1, RestoreDirectory = true })
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
                                    Dt.Columns.Add("REVENUECODE", typeof(string));
                                    Dt.Columns.Add("PAYERNAME", typeof(string));
                                    Dt.EndInit();

                                    MyConnection = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" +
                                                   filenamesopen + ";Extended Properties=\"Excel 8.0;HDR=YES;\"");

                                    MyCommand = new System.Data.OleDb.OleDbDataAdapter("select * from [Sheet1$]", MyConnection);
                                    DtSet = new System.Data.DataSet();
                                    DtSet.Clear();
                                    MyCommand.Fill(DtSet, "[Sheet1$]");

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

                                                        rw["PAYERNAME"] = row["PAYERNAME"];
                                                    }


                                                }
                                                else if (!(row["CREDIT"] is DBNull))
                                                {


                                                    if (!Logic.isDeceimalFormat((string)row["CREDIT"].ToString()))
                                                    {
                                                        //var rw = Dt.NewRow();

                                                        Common.setMessageBox(string.Format("Incorrect Data format encountered. Correct {0} in Line No. {1} and try again.", row["CREDIT"], j), "Import Data Error", 3); return;
                                                    }
                                                    else
                                                    {
                                                        rw["CREDIT"] = row["CREDIT"] is DBNull ? 0m : Convert.ToDecimal(row["CREDIT"]);
                                                        rw["DEBIT"] = row["DEBIT"] is DBNull ? 0m : row["DEBIT"] == string.Empty ? 0m : Convert.ToDecimal(row["DEBIT"]);

                                                        rw["BALANCE"] = row["BALANCE"] is DBNull ? 0m : Convert.ToDecimal(row["BALANCE"]);

                                                    }
                                                }
                                                else
                                                {
                                                    rw["CREDIT"] = row["CREDIT"] is DBNull ? 0m : Convert.ToDecimal(row["CREDIT"]);
                                                    rw["DEBIT"] = row["DEBIT"] is DBNull ? 0m : row["DEBIT"] == string.Empty ? 0m : Convert.ToDecimal(row["DEBIT"]);

                                                    rw["BALANCE"] = row["BALANCE"] is DBNull ? 0m : Convert.ToDecimal(row["BALANCE"]);
                                                    rw["REVENUECODE"] = row["REVENUECODE"];

                                                    rw["PAYERNAME"] = row["PAYERNAME"];
                                                }

                                                Dt.Rows.Add(rw);
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
                                    gridView1.OptionsBehavior.Editable = false;
                                    //MyConnection.Close();
                                }
                                catch (Exception ex)
                                {
                                    //Common.setMessageBox("Modify the excel to contain this Column Header 'DATE','DEBIT','CREDIT','BALANCE','REVENUECODE','PAYERNAME',", "Import Data Error", 3);

                                    gridControl1.DataSource = null;

                                    Tripous.Sys.ErrorBox(String.Format(" Modify the excel to contain this Column Header 'DATE','DEBIT','CREDIT','BALANCE','REVENUECODE','PAYERNAME'.... {0}{1} ...Import Data Error", ex.Message, ex.StackTrace));

                                    return;


                                }
                                //ChangeValue(Dt);


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

                                //gridView1.OptionsView.ColumnAutoWidth = false;
                                gridView1.OptionsView.ShowFooter = true;

                                gridView1.BestFitColumns();

                                label2.Text = Dt.Rows.Count + " Rows of Records ";
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
                }

                if (result == DialogResult.No)//excele 2007
                {
                    //MessageBox.Show("You clicked the yes button");
                    //new OpenFileDialog().ShowDialog();

                    try
                    {
                        SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

                        //exportToExcel();

                        gridControl1.DataSource = null;

                        using (OpenFileDialog openFileDialogCSV = new OpenFileDialog() { InitialDirectory = Application.ExecutablePath, Filter = "Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*", FilterIndex = 1, RestoreDirectory = true })
                        {

                            //openFileDialogCSV.ShowDialog();
                            if (openFileDialogCSV.ShowDialog() == DialogResult.OK)
                            {
                                if (openFileDialogCSV.FileName.Length > 0)
                                {
                                    filenamesopen = openFileDialogCSV.FileName;
                                }
                                //try
                                //{
                                Dt = new DataTable();
                                Dt.BeginInit();
                                Dt.Columns.Add("DATE", typeof(DateTime));
                                Dt.Columns.Add("DEBIT", typeof(decimal));
                                Dt.Columns.Add("CREDIT", typeof(decimal));
                                Dt.Columns.Add("BALANCE", typeof(decimal));
                                Dt.Columns.Add("REVENUECODE", typeof(string));
                                Dt.Columns.Add("PAYERNAME", typeof(string));
                                Dt.EndInit();

                                Dt.Clear();
                                MyConnection = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" +
                                               filenamesopen + ";Extended Properties=\"Excel 8.0;HDR=YES;\"");

                                MyCommand = new System.Data.OleDb.OleDbDataAdapter("select * from [Sheet1$]", MyConnection);
                                DtSet = new System.Data.DataSet(); DtSet.Clear();
                                MyCommand.Fill(DtSet, "[Sheet1$]");

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
                                                //if ((Int32)row["DEBIT"])
                                                {
                                                    //var rw = Dt.NewRow();

                                                    Common.setMessageBox(string.Format("Incorrect Data format encountered. Correct {0} in Line No. {1} and try again.", row["DEBIT"], j), "Import Data Error", 3); return;
                                                }
                                                else
                                                {
                                                    rw["DEBIT"] = row["DEBIT"] is DBNull ? 0m : row["DEBIT"] == string.Empty ? 0m : Convert.ToDecimal(row["DEBIT"]);

                                                    rw["BALANCE"] = row["BALANCE"] is DBNull ? 0m : Convert.ToDecimal(row["BALANCE"]);
                                                    rw["CREDIT"] = row["CREDIT"] is DBNull ? 0m : Convert.ToDecimal(row["CREDIT"]);

                                                    rw["REVENUECODE"] = row["REVENUECODE"];

                                                    rw["PAYERNAME"] = row["PAYERNAME"];
                                                }
                                            }
                                            else if (!(row["CREDIT"] is DBNull))
                                            {
                                                if (!Logic.isDeceimalFormat((string)row["CREDIT"].ToString()))
                                                {
                                                    //var rw = Dt.NewRow();

                                                    Common.setMessageBox(string.Format("Incorrect Data format encountered. Correct {0} in Line No. {1} and try again.", row["CREDIT"], j), "Import Data Error", 3); return;
                                                }
                                                else
                                                {
                                                    rw["CREDIT"] = row["CREDIT"] is DBNull ? 0m : Convert.ToDecimal(row["CREDIT"]);
                                                    rw["DEBIT"] = row["DEBIT"] is DBNull ? 0m : row["DEBIT"] == string.Empty ? 0m : Convert.ToDecimal(row["DEBIT"]);

                                                    rw["BALANCE"] = row["BALANCE"] is DBNull ? 0m : Convert.ToDecimal(row["BALANCE"]);
                                                    rw["REVENUECODE"] = row["REVENUECODE"];

                                                    rw["PAYERNAME"] = row["PAYERNAME"];

                                                }
                                            }
                                            else
                                            {
                                                rw["CREDIT"] = row["CREDIT"] is DBNull ? 0m : Convert.ToDecimal(row["CREDIT"]);
                                                rw["DEBIT"] = row["DEBIT"] is DBNull ? 0m : row["DEBIT"] == string.Empty ? 0m : Convert.ToDecimal(row["DEBIT"]);

                                                rw["BALANCE"] = row["BALANCE"] is DBNull ? 0m : Convert.ToDecimal(row["BALANCE"]);

                                                rw["REVENUECODE"] = row["REVENUECODE"];

                                                rw["PAYERNAME"] = row["PAYERNAME"];
                                            }

                                            Dt.Rows.Add(rw);
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
                                gridView1.OptionsBehavior.Editable = false;
                                MyConnection.Close();

                                gridView1.Columns["DEBIT"].DisplayFormat.FormatType = FormatType.Numeric;
                                gridView1.Columns["DEBIT"].DisplayFormat.FormatString = "n2";
                                gridView1.Columns["CREDIT"].DisplayFormat.FormatType = FormatType.Numeric;
                                gridView1.Columns["CREDIT"].DisplayFormat.FormatString = "n2";
                                gridView1.Columns["BALANCE"].DisplayFormat.FormatType = FormatType.Numeric;
                                gridView1.Columns["BALANCE"].DisplayFormat.FormatString = "n2";
                                gridView1.Columns["BALANCE"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;

                                gridView1.Columns["BALANCE"].SummaryItem.FieldName = "BALANCE";
                                gridView1.Columns["BALANCE"].SummaryItem.DisplayFormat = "Total = {0:n}";

                                gridView1.Columns["CREDIT"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                                gridView1.Columns["DEBIT"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;

                                gridView1.Columns["CREDIT"].SummaryItem.FieldName = "CREDIT";
                                gridView1.Columns["CREDIT"].SummaryItem.DisplayFormat = "Total = {0:n}";

                                gridView1.Columns["DEBIT"].SummaryItem.FieldName = "DEBIT";
                                gridView1.Columns["DEBIT"].SummaryItem.DisplayFormat = "Total = {0:n}";

                                //gridView1.OptionsView.ColumnAutoWidth = false;
                                gridView1.OptionsView.ShowFooter = true;

                                gridView1.BestFitColumns();
                                label2.Text = Dt.Rows.Count + " Rows of Records ";
                            }
                            else
                            {
                                Common.setMessageBox("Operation Cancel", "Import Cancel", 3); return;
                            }
                        }

                    }
                    catch (Exception ex)
                    {

                        gridControl1.DataSource = null;

                        Tripous.Sys.ErrorBox(String.Format(" Modify the excel to contain this Column Header 'DATE','DEBIT','CREDIT','BALANCE','REVENUECODE','PAYERNAME'.... {0}{1} ...Import Data Error", ex.Message, ex.StackTrace));

                        return;
                    }
                    finally
                    {
                        SplashScreenManager.CloseForm(false);
                    }
                }

                bttnNextImp.Enabled = true; bttnUpdateExcel.Enabled = true;
                MessageBoxManager.Unregister();

            }
        }

        void cboBank_KeyPress(object sender, KeyPressEventArgs e)
        {
            Methods.AutoComplete(cboBank, e, true);
        }

        void cboBank_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboBank.SelectedValue != null && !Isbank)
            {
                setDBComboBoxAcct();
            }
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

        public void RefreshForm()
        {
            NavBars.ToolStripEnableDisableControls(toolStrip, Tag as String);
        }

        void UpdateBankstatement(string strbankcode, DataTable dbData)
        {

            try
            {
                using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
                {
                    connect.Open();
                    _command = new SqlCommand("InsertBankExcellStatmentyearly", connect) { CommandType = CommandType.StoredProcedure };
                    _command.Parameters.Add(new SqlParameter("@bankcode", SqlDbType.VarChar)).Value = strbankcode;
                    _command.Parameters.Add(new SqlParameter("@startdate", SqlDbType.VarChar)).Value = string.Format("{0:yyyy/MM/dd}", dtpStart.Value);
                    _command.Parameters.Add(new SqlParameter("@enddate", SqlDbType.VarChar)).Value = string.Format("{0:yyyy/MM/dd}", dtpEnd.Value);
                    _command.Parameters.Add(new SqlParameter("@pTransaction", SqlDbType.Structured)).Value = dbData;
                    _command.Parameters.Add(new SqlParameter("@opening", SqlDbType.Money)).Value = txtOpen.EditValue;
                    _command.Parameters.Add(new SqlParameter("@AccountID", SqlDbType.Char)).Value = Convert.ToInt32(cboAccount.SelectedValue);

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

                    }
                }
            }
            catch (Exception ex)
            {
                Tripous.Sys.ErrorBox(String.Format("{0}{1}", ex.Message, ex.StackTrace)); return;
            }

        }

    }
}
