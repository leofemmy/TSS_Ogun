using BankReconciliation.Class;
using BankReconciliation.Report;
using DevExpress.Utils;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraReports.UI;
using DevExpress.XtraSplashScreen;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;
using TaxSmartSuite.Class;

namespace BankReconciliation.Forms
{
    public partial class FrmSchedule : Form
    {
        private SqlCommand _command;
        private SqlDataAdapter adp;
        public static FrmSchedule publicStreetGroup;

        public FrmSchedule()
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

            }
            else if (sender == tsbEdit)
            {
            }
            else if (sender == tsbDelete)
            {

                tsbReload.PerformClick();
                //boolIsUpdate = false;
            }
            else if (sender == tsbReload)
            {
                //iTransType = TransactionTypeCode.Reload; setReload();
                //ShowForm();
            }

        }
        void OnFormLoad(object sender, EventArgs e)
        {
            setReload();

            gridView1.DoubleClick += gridView1_DoubleClick;
        }

        void gridView1_DoubleClick(object sender, EventArgs e)
        {
            GridView view = (GridView)gridControl1.FocusedView;

            if (radioGroup3.SelectedIndex == -1)
            {
                Common.setEmptyField("Report Type", groupControl1.Text); return;
            }
            else
            {

                if (view != null)
                {
                    DataRow dr = view.GetDataRow(view.FocusedRowHandle);

                    if (dr != null)
                    {
                        if (radioGroup3.EditValue.ToString() == "Bank")
                        {
                            try
                            {
                                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

                                using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
                                {
                                    connect.Open();
                                    _command = new SqlCommand("BankTransaction", connect) { CommandType = CommandType.StoredProcedure };
                                    _command.Parameters.Add(new SqlParameter("@bankcode", SqlDbType.VarChar)).Value = dr["BankShortCode"];
                                    _command.Parameters.Add(new SqlParameter("@Periods", SqlDbType.Int)).Value = Convert.ToInt32(dr["FinancialperiodID"]);
                                    _command.Parameters.Add(new SqlParameter("@BankAccountID", SqlDbType.Int)).Value = Convert.ToInt32(dr["BankAccountID"]);
                                    _command.Parameters.Add(new SqlParameter("@startDate", SqlDbType.DateTime)).Value = string.Format("{0:yyyy/MM/dd}", dr["StartDate"]);
                                    _command.Parameters.Add(new SqlParameter("@endDate", SqlDbType.DateTime)).Value = string.Format("{0:yyyy/MM/dd}", dr["EndDate"]);
                                    _command.Parameters.Add(new SqlParameter("@PType", SqlDbType.VarChar)).Value = "Bank";
                                    //
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

                                                //using object class
                                                var list = (from DataRow row in ds.Tables[1].Rows
                                                            select new Dataset.ReportSchedule
                                                            {
                                                                Date = Convert.ToDateTime(row["Date"]),
                                                                Debit = Convert.ToDecimal(row["Debit"]),
                                                                Credit = Convert.ToDecimal(row["Credit"]),
                                                                Comments = row["Comments"] as string,
                                                                BankName = row["BankName"] as string,
                                                                Acctnumber = row["Acctnumber"] as string,
                                                                Period = row["period"] as string,
                                                                Branchname = row["Branchname"] as string
                                                            }
                                                                ).ToList();

                                                XtraRepSchedule repSchedule = new XtraRepSchedule();

                                                var bindingsed = (BindingSource)repSchedule.DataSource;
                                                bindingsed.Clear();
                                                bindingsed.DataSource = list;
                                                repSchedule.xrLabel2.Text = "Schedule of Unexplained Bank Transactions";
                                                repSchedule.ShowPreviewDialog();


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
                            finally
                            {
                                SplashScreenManager.CloseForm(false);
                            }
                        }
                        else if (radioGroup3.EditValue.ToString() == "Allocate")
                        {


                            string strquery = string.Format("SELECT  BankName ,AccountName , AccountNumber ,BSDate , Debit ,Credit ,Description , ISNULL(AllocateBy, 'No Name') AS AllocateBy  ,  ISNULL(DateAllocate, GETDATE()) AS DateAllocate  ,Months + ',' + Year AS Period ,ISNULL(ClosingBalance, 0) AS ClosingBalance ,  ISNULL(OpeningBalance, 0) AS OpeningBalance  ,ISNULL(TotalCredit, 0) AS TotalCredit , ISNULL(TotalDebit, 0) AS TotalDebit FROM    Reconciliation.tblBankStatementAllocation JOIN Reconciliation.tblBankStatement ON tblBankStatement.BSID = tblBankStatementAllocation.BSID JOIN Reconciliation.tblTransDefinition ON tblTransDefinition.TransID = tblBankStatementAllocation.TransID JOIN Reconciliation.tblBankAccount ON tblBankAccount.BankAccountID = tblBankStatement.BankAccountID AND tblBankAccount.BankShortCode = tblBankStatement.BankShortCode JOIN Collection.tblBank ON tblBank.BankShortCode = tblBankStatement.BankShortCode JOIN Reconciliation.tblFinancialperiod ON tblFinancialperiod.FinancialperiodID = tblBankStatement.FinancialperiodID  JOIN Reconciliation.tblTransactionPostingRequest ON tblTransactionPostingRequest.BankAccountID = tblBankAccount.BankAccountID AND tblTransactionPostingRequest.BankShortCode = tblBank.BankShortCode AND tblTransactionPostingRequest.FinancialperiodID = tblFinancialperiod.FinancialperiodID  WHERE tblBankStatement.BankShortCode ='{0}' AND tblBankStatement.BankAccountID='{1}' AND tblBankStatement.FinancialperiodID='{2}'", dr["BankShortCode"], dr["BankAccountID"], dr["FinancialperiodID"]);

                            using (var ds = new System.Data.DataSet())
                            {
                                //connect.connect.Open();
                                using (SqlDataAdapter ada = new SqlDataAdapter(strquery, Logic.ConnectionString))
                                {
                                    ada.Fill(ds, "table");
                                }

                                //var Dt = ds.Tables[0];
                                if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                                {
                                    var listAllocate = (from DataRow row in ds.Tables[0].Rows
                                                        select new Dataset.Allocation
                                                        {
                                                            BankName = row["BankName"] as string,
                                                            AccountName = row["AccountName"] as string,
                                                            AccountNumber = row["AccountNumber"] as string,
                                                            BSDate = Convert.ToDateTime(row["BSDate"]),
                                                            Debit = Convert.ToDecimal(row["Debit"]),
                                                            Credit = Convert.ToDecimal(row["Credit"]),
                                                            Description = row["Description"] as string,
                                                            AllocateBy = row["AllocateBy"] as string,
                                                            DateAllocate = Convert.ToDateTime(row["DateAllocate"]),
                                                            Period = row["Period"] as string,
                                                            ClosingBalance = Convert.ToDecimal(row["ClosingBalance"]),
                                                            OpeningBalance = Convert.ToDecimal(row["OpeningBalance"]),
                                                            TotalCredit = Convert.ToDecimal(row["TotalCredit"]),
                                                            TotalDebit = Convert.ToDecimal(row["TotalDebit"])
                                                        }).ToList();

                                    XtraRepAllocatereport allocate = new XtraRepAllocatereport() { DataSource = listAllocate };
                                    allocate.ShowPreviewDialog();
                                    //var binding = (BindingSource)allocate.DataSource;
                                    //binding.Clear();
                                    //binding.DataSource = listAllocate;
                                    ////allocate.xrLabel2.Text = "Bank not in Reems";
                                    //allocate.ShowPreviewDialog();

                                    ////repyear.xrLabel11.Text = string.Format("Bank Name: {0}", cboBank.Text.Trim());
                                    ////repyear.xrLabel12.Text =
                                    ////    string.Format("List of Transactions in Bank Statement not in Reems between {0:dd/MM/yyyy} and {1:dd/MM/yyyy}", dtpStart.Value, dtpEnd.Value);
                                    ////repyear.DataSource = replist;

                                    ////repyear.ShowPreviewDialog();

                                }
                                else
                                {
                                    Common.setMessageBox("No Record Found", Program.ApplicationName, 1); return;
                                }
                            }



                        }
                        //BReems
                        else if (radioGroup3.EditValue.ToString() == "BReems")
                        {
                            try
                            {
                                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

                                using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
                                {
                                    connect.Open();
                                    _command = new SqlCommand("BankTransaction", connect) { CommandType = CommandType.StoredProcedure };
                                    _command.Parameters.Add(new SqlParameter("@bankcode", SqlDbType.VarChar)).Value = dr["BankShortCode"];
                                    _command.Parameters.Add(new SqlParameter("@Periods", SqlDbType.Int)).Value = Convert.ToInt32(dr["FinancialperiodID"]);
                                    _command.Parameters.Add(new SqlParameter("@startDate", SqlDbType.DateTime)).Value = string.Format("{0:yyyy/MM/dd}", dr["StartDate"]);
                                    _command.Parameters.Add(new SqlParameter("@endDate", SqlDbType.DateTime)).Value = string.Format("{0:yyyy/MM/dd}", dr["EndDate"]);
                                    _command.Parameters.Add(new SqlParameter("@BankAccountID", SqlDbType.Int)).Value = Convert.ToInt32(dr["BankAccountID"]);
                                    _command.Parameters.Add(new SqlParameter("@PType", SqlDbType.VarChar)).Value = "BReems";
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

                                                //using object class
                                                var list = (from DataRow row in ds.Tables[1].Rows
                                                            select new Dataset.ReportSchedule
                                                            {
                                                                Date = Convert.ToDateTime(row["Date"]),
                                                                //Debit = Convert.ToDecimal(row["Debit"]),
                                                                Credit = Convert.ToDecimal(row["Credit"]),
                                                                Comments = row["Description"] as string,
                                                                BankName = row["BankName"] as string,
                                                                Acctnumber = row["Acctnumber"] as string,
                                                                Period = row["peroids"] as string,
                                                                Branchname = row["Branchname"] as string
                                                            }
                                                                ).ToList();

                                                XtraRepSchedule repSchedule = new XtraRepSchedule();

                                                var binding = (BindingSource)repSchedule.DataSource;
                                                binding.Clear();
                                                binding.DataSource = list;
                                                repSchedule.xrLabel2.Text = "Bank not in Reems";
                                                repSchedule.ShowPreviewDialog();


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
                            finally
                            {
                                SplashScreenManager.CloseForm(false);
                            }
                        }
                        //posted
                        else if (radioGroup3.EditValue.ToString() == "Posted")
                        {
                            try
                            {
                                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

                                using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
                                {
                                    connect.Open();
                                    _command = new SqlCommand("BankTransaction", connect) { CommandType = CommandType.StoredProcedure };
                                    _command.Parameters.Add(new SqlParameter("@bankcode", SqlDbType.VarChar)).Value = dr["BankShortCode"];
                                    _command.Parameters.Add(new SqlParameter("@Periods", SqlDbType.Int)).Value = Convert.ToInt32(dr["FinancialperiodID"]);
                                    _command.Parameters.Add(new SqlParameter("@startDate", SqlDbType.DateTime)).Value = string.Format("{0:yyyy/MM/dd}", dr["StartDate"]);
                                    _command.Parameters.Add(new SqlParameter("@endDate", SqlDbType.DateTime)).Value = string.Format("{0:yyyy/MM/dd}", dr["EndDate"]);
                                    _command.Parameters.Add(new SqlParameter("@BankAccountID", SqlDbType.Int)).Value = Convert.ToInt32(dr["BankAccountID"]);
                                    _command.Parameters.Add(new SqlParameter("@PType", SqlDbType.VarChar)).Value = "Posted";
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
                                                //using object class
                                                var list = (from DataRow row in ds.Tables[1].Rows
                                                            select new Dataset.Payment
                                                            {
                                                                Amount = Convert.ToDecimal(row["Amount"]),
                                                                PaymentDate = Convert.ToDateTime(row["PaymentDate"]),
                                                                PaymentRefNumber = row["PaymentRefNumber"] as string,

                                                                Description = row["Description"] as string,

                                                                PayerName = row["PayerID"] as string,
                                                                Transdate = Convert.ToDateTime(row["BSDate"]),
                                                                AgecnyName = row["AgencyName"] as string,
                                                                BankName = row["BankName"] as string,

                                                                //Branchname = row["Branchname"] as string
                                                            }
                                                                ).ToList();
                                                //string state=Program.StateName;
                                                XtraRepPostedTrans payment = new XtraRepPostedTrans();
                                                //XtraRepPayment payment = new XtraRepPayment();
                                                //payment.xrLabel1.Text = "OGUN STATE GOVERNMENT OF NIGERIA";

                                                payment.xrLabel6.Text = string.Format("{0} STATE GOVERNMENT OF NIGERIA", Program.StateName.ToUpper());

                                                payment.xrLabel7.Text = string.Format("List of Collections Posted for the month of {0} ", string.Format("{0:MMMM yyyy}", dr["Descriptions"]));

                                                var binding = (BindingSource)payment.DataSource;

                                                binding.Clear();

                                                binding.DataSource = list;

                                                payment.ShowPreviewDialog();

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
                            catch (Exception ex)
                            {

                                Common.setMessageBox(String.Format("{0}..{1}", ex.Message, ex.StackTrace), "Report", 2);
                                return;
                            }
                            finally
                            {
                                SplashScreenManager.CloseForm(false);
                            }
                        }
                        //Payment
                        else if (radioGroup3.EditValue.ToString() == "Payment")
                        {
                            try
                            {
                                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

                                using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
                                {
                                    connect.Open();
                                    _command = new SqlCommand("BankTransaction", connect) { CommandType = CommandType.StoredProcedure };
                                    _command.Parameters.Add(new SqlParameter("@bankcode", SqlDbType.VarChar)).Value = dr["BankShortCode"];
                                    _command.Parameters.Add(new SqlParameter("@Periods", SqlDbType.Int)).Value = Convert.ToInt32(dr["FinancialperiodID"]);
                                    _command.Parameters.Add(new SqlParameter("@startDate", SqlDbType.DateTime)).Value = string.Format("{0:yyyy/MM/dd}", dr["StartDate"]);
                                    _command.Parameters.Add(new SqlParameter("@endDate", SqlDbType.DateTime)).Value = string.Format("{0:yyyy/MM/dd}", dr["EndDate"]);
                                    _command.Parameters.Add(new SqlParameter("@BankAccountID", SqlDbType.Int)).Value = Convert.ToInt32(dr["BankAccountID"]);
                                    _command.Parameters.Add(new SqlParameter("@PType", SqlDbType.VarChar)).Value = "Payment";
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
                                                //using object class
                                                var list = (from DataRow row in ds.Tables[1].Rows
                                                            select new Dataset.Payment
                                                            {
                                                                Amount = Convert.ToDecimal(row["Amount"]),
                                                                PaymentDate = Convert.ToDateTime(row["PaymentDate"]),
                                                                PaymentRefNumber = row["PaymentRefNumber"] as string,

                                                                Description = row["Description"] as string,

                                                                PayerName = row["PayerName"] as string,

                                                                //Branchname = row["Branchname"] as string
                                                            }
                                                                ).ToList();
                                                //string state=Program.StateName;
                                                XtraRepPayment payment = new XtraRepPayment();
                                                //payment.xrLabel1.Text = "OGUN STATE GOVERNMENT OF NIGERIA";

                                                payment.xrLabel1.Text = string.Format("{0} STATE GOVERNMENT OF NIGERIA", Program.StateName.ToUpper());

                                                payment.xrLabel2.Text = string.Format("Schedule of REEMS Platform Payments Not Posted into Bank Statement for month of {0} ", string.Format("{0:MMMM yyyy}", dr["EndDate"]));
                                                var binding = (BindingSource)payment.DataSource;
                                                binding.Clear();
                                                binding.DataSource = list;
                                                payment.ShowPreviewDialog();

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
                            finally
                            {
                                SplashScreenManager.CloseForm(false);
                            }
                        }
                        //reems
                        else if (radioGroup3.EditValue.ToString() == "Reems")
                        {
                            try
                            {
                                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

                                using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
                                {
                                    connect.Open();
                                    _command = new SqlCommand("BankTransaction", connect) { CommandType = CommandType.StoredProcedure };
                                    _command.Parameters.Add(new SqlParameter("@bankcode", SqlDbType.VarChar)).Value = dr["BankShortCode"];
                                    _command.Parameters.Add(new SqlParameter("@Periods", SqlDbType.Int)).Value = Convert.ToInt32(dr["FinancialperiodID"]);
                                    _command.Parameters.Add(new SqlParameter("@startDate", SqlDbType.DateTime)).Value = string.Format("{0:yyyy/MM/dd}", dr["StartDate"]);
                                    _command.Parameters.Add(new SqlParameter("@endDate", SqlDbType.DateTime)).Value = string.Format("{0:yyyy/MM/dd}", dr["EndDate"]);
                                    _command.Parameters.Add(new SqlParameter("@BankAccountID", SqlDbType.Int)).Value = Convert.ToInt32(dr["BankAccountID"]);
                                    _command.Parameters.Add(new SqlParameter("@PType", SqlDbType.VarChar)).Value = "Reems";
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

                                                //using object class
                                                var list = (from DataRow row in ds.Tables[1].Rows
                                                            select new Dataset.Reems
                                                            {
                                                                Openingbal = Convert.ToDecimal(row["openingbalance"]),
                                                                closingbal = Convert.ToDecimal(row["closebalance"]),
                                                                ReemsCollection = Convert.ToDecimal(row["both"]),
                                                                Bankexcpec = Convert.ToDecimal(row["BankExcpection"]),
                                                                Bankcredit = Convert.ToDecimal(row["BankCredit"]),
                                                                BankDebit = Convert.ToDecimal(row["BankDebit"]),
                                                                bankcharge = Convert.ToDecimal(row["bankcharge"]),
                                                                Transferto = Convert.ToDecimal(row["Transferto"]),
                                                                PrevcreditRev = Convert.ToDecimal(row["PrevcreditRev"]),
                                                                ReturnCheque = Convert.ToDecimal(row["ReturnCheque"]),
                                                                CurrentReversalDr = Convert.ToDecimal(row["CurrentReversalDr"]),
                                                                CurrentReversalCr = Convert.ToDecimal(row["CurrentReversalCr"]),
                                                                PayDirectBank = Convert.ToDecimal(row["PayDirectBank"]),
                                                                CreditInterest = Convert.ToDecimal(row["CreditInterest"]),
                                                                TransferFromGovtAcct = Convert.ToDecimal(row["TransferFromGovtAcct"]),
                                                                PrevDebitReversed = Convert.ToDecimal(row["PrevDebitReversed"]),
                                                                BankName = row["BankName"] as string,

                                                                Acctnumber = row["Acctnumber"] as string,

                                                                Period = row["period"] as string,

                                                                Branchname = row["Branchname"] as string
                                                            }
                                                                ).ToList();

                                                XtraRepBankReems bankreem = new XtraRepBankReems
();
                                                //XtraRepSchedule repSchedule = new XtraRepSchedule();

                                                var binding = (BindingSource)bankreem.DataSource;
                                                binding.DataSource = list;
                                                //repSchedule.ShowPreviewDialog();
                                                bankreem.ShowPreviewDialog();

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
                            finally
                            {
                                SplashScreenManager.CloseForm(false);
                            }
                        }


                    }
                    else
                    {
                        Common.setEmptyField("Select Record...", "Get Record"); return;
                    }
                }
            }
        }

        public void setDBComboBox()
        {
            using (var ds = new System.Data.DataSet())
            {
                using (SqlDataAdapter ada = new SqlDataAdapter((string)"SELECT BankShortCode,BankName FROM Collection.tblBank ORDER BY BankName", Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }

                //Dt = ds.Tables[0];

                //Common.setComboList(cboBank, ds.Tables[0], "BankShortCode", "BankName");

            }


            //cboBank.SelectedIndex = -1;


        }

        public void RefreshForm()
        {
            NavBars.ToolStripEnableDisableControls(toolStrip, Tag as String);
        }

        private void setReload()
        {
            using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
            {
                connect.Open();
                _command = new SqlCommand("dogetReconcilie", connect) { CommandType = CommandType.StoredProcedure };

                using (System.Data.DataSet ds = new System.Data.DataSet())
                {
                    ds.Clear();
                    adp = new SqlDataAdapter(_command);
                    adp.Fill(ds);
                    connect.Close();

                    if (ds.Tables[0].Rows[0]["returnCode"].ToString() == "00")
                    {
                        gridControl1.DataSource = ds.Tables[1];
                    }
                    else
                    {
                        return;
                    }
                }
            }

            gridView1.OptionsBehavior.Editable = false;
            gridView1.Columns["Closing Balance"].DisplayFormat.FormatType = FormatType.Numeric;
            gridView1.Columns["Closing Balance"].DisplayFormat.FormatString = "n2";
            gridView1.Columns["Opening Balance"].DisplayFormat.FormatType = FormatType.Numeric;
            gridView1.Columns["Opening Balance"].DisplayFormat.FormatString = "n2";
            gridView1.Columns["BankShortCode"].Visible = false;
            gridView1.Columns["FinancialperiodID"].Visible = false;
            gridView1.Columns["Periods"].Visible = false;
            gridView1.Columns["StartDate"].Visible = false;
            gridView1.Columns["EndDate"].Visible = false;
            gridView1.Columns["BankAccountID"].Visible = false;
            //Status
            gridView1.BestFitColumns();
        }


    }
}
