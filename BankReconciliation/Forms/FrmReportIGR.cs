using BankReconciliation.Class;
using BankReconciliation.Report;
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
    public partial class FrmReportIGR : Form
    {
        public static FrmReportIGR publicStreetGroup;

        protected TransactionTypeCode iTransType;

        private SqlCommand _command; private SqlDataAdapter adp;

        public FrmReportIGR()
        {
            SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

            InitializeComponent();

            publicStreetGroup = this;

            setImages();

            ToolStripEvent();

            iTransType = TransactionTypeCode.New;

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
            bttnUpdate.Image = MDIMains.publicMDIParent.i32x32.Images[7];
            ////bttnReset.Image = MDIMains.publicMDIParent.i32x32.Images[8];
            //bttnPost.Image = MDIMains.publicMDIParent.i32x32.Images[34];

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
                iTransType = TransactionTypeCode.New;
                //Clear();
                ShowForm();
                //boolIsUpdate = false;
            }
            else if (sender == tsbEdit)
            {
                //groupControl2.Text = "Edit Record Mode";
                iTransType = TransactionTypeCode.Edit;
                //if (EditRecordMode())
                //{
                ShowForm();
                //    boolIsUpdate = true;
                //}
            }
            else if (sender == tsbDelete)
            {
                //groupControl2.Text = "Delete Record Mode";
                iTransType = TransactionTypeCode.Delete;
                if (MosesClassLibrary.Utilities.Common.AskQuestion("Deleting this record will delete attached record.\nDo you want to continue?", ""))
                {
                }
                else
                    tsbReload.PerformClick();

                //boolIsUpdate = false;
            }
            else if (sender == tsbReload)
            {
                iTransType = TransactionTypeCode.Reload;

                ShowForm();
            }
            //bttnReset.PerformClick();
        }

        void OnFormLoad(object sender, EventArgs e)
        {
            ShowForm();

            setDBComboBox();

            bttnUpdate.Click += bttnUpdate_Click;

        }

        void bttnUpdate_Click(object sender, EventArgs e)
        {
            DataRowView oDataRowView = cboBatch.SelectedItem as DataRowView;

            if (radioGroup1.SelectedIndex == -1)
            {
                Common.setEmptyField("IGR Report Options...", Program.ApplicationName);
                return;
            }
            else
            {
                try
                {
                    SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

                    if (oDataRowView != null)
                    {
                        if (radioGroup1.EditValue == "Bank")//Call Report for bank Reconciliation
                        {
                            using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
                            {
                                connect.Open();
                                _command = new SqlCommand("doIGRReport", connect) { CommandType = CommandType.StoredProcedure };
                                _command.Parameters.Add(new SqlParameter("@Type", SqlDbType.VarChar)).Value = "Bank";
                                _command.Parameters.Add(new SqlParameter("@BatchCode", SqlDbType.VarChar)).Value = oDataRowView.Row["BatchCode"];
                                _command.CommandTimeout = 0;
                                using (System.Data.DataSet ds = new System.Data.DataSet())
                                {
                                    ds.Clear();
                                    adp = new SqlDataAdapter(_command);
                                    adp.Fill(ds);
                                    connect.Close();

                                    if (ds.Tables[0].Rows[0]["returnCode"].ToString() == "00")
                                    {
                                        XtraRepGen report = new XtraRepGen();
                                        report.xrLabel18.Text = String.Format(" for the month of : [{0}]", oDataRowView.Row["BatchName"]);
                                        report.DataSource = ds.Tables[1];
                                        report.DataMember = "table";
                                        report.ShowPreviewDialog();

                                    }
                                    else
                                    {
                                        return;
                                    }

                                }
                            }
                        }
                        else if (radioGroup1.EditValue == "Agency")
                        {
                            using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
                            {
                                connect.Open();
                                _command = new SqlCommand("doIGRReport", connect) { CommandType = CommandType.StoredProcedure };
                                _command.Parameters.Add(new SqlParameter("@Type", SqlDbType.VarChar)).Value = "Agency";
                                _command.Parameters.Add(new SqlParameter("@BatchCode", SqlDbType.VarChar)).Value = oDataRowView.Row["BatchCode"];
                                _command.CommandTimeout = 0;
                                using (System.Data.DataSet ds = new System.Data.DataSet())
                                {
                                    ds.Clear();
                                    adp = new SqlDataAdapter(_command);
                                    adp.Fill(ds);
                                    connect.Close();

                                    if (ds.Tables[0].Rows[0]["returnCode"].ToString() == "00")
                                    {

                                        XtraRepSummrayAgency repAgency = new XtraRepSummrayAgency() { DataSource = ds, DataMember = "table" };
                                        repAgency.xrLabel32.Text = string.Format(" {0} STATE GOVERNMENT ", Program.StateName.ToUpper());

                                        repAgency.xrLabel33.Text = string.Format("Summary of Collection by Agencies for the Month of  {0}", oDataRowView.Row["BatchName"]);
                                        repAgency.DataSource = ds.Tables[1];
                                        repAgency.DataMember = "ViewReconciliationAgencyDetails";
                                        repAgency.ShowPreviewDialog();

                                    }
                                    else
                                    {
                                        return;
                                    }

                                }
                            }
                        }
                        else if (radioGroup1.EditValue == "Revenue")
                        {
                            using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
                            {
                                connect.Open();
                                _command = new SqlCommand("doIGRReport", connect) { CommandType = CommandType.StoredProcedure };
                                _command.Parameters.Add(new SqlParameter("@Type", SqlDbType.VarChar)).Value = "Revenue";
                                _command.Parameters.Add(new SqlParameter("@BatchCode", SqlDbType.VarChar)).Value = oDataRowView.Row["BatchCode"];
                                _command.CommandTimeout = 0;
                                using (System.Data.DataSet ds = new System.Data.DataSet())
                                {
                                    ds.Clear();
                                    adp = new SqlDataAdapter(_command);
                                    adp.Fill(ds);
                                    connect.Close();

                                    if (ds.Tables[0].Rows[0]["returnCode"].ToString() == "00")
                                    {

                                        XtraRepSummRevenue report = new XtraRepSummRevenue() { DataSource = ds.Tables[1], DataMember = "table" };
                                        //report.paramMonths.Value = dtpStart.Value.Month;
                                        //report.paramYear.Value = dtpStart.Value.Year;

                                        report.xrLabel32.Text = string.Format(" {0} STATE GOVERNMENT ", Program.StateName.ToUpper());

                                        report.xrLabel33.Text = string.Format("Summary of Collection by Revenue Type for the Month of  {0}", oDataRowView.Row["BatchName"]);

                                        report.ShowPreviewDialog();


                                    }
                                    else
                                    {
                                        return;
                                    }

                                }
                            }
                        }
                        else if (radioGroup1.EditValue == "Details")
                        {
                            using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
                            {
                                connect.Open();
                                _command = new SqlCommand("doIGRReport", connect) { CommandType = CommandType.StoredProcedure };
                                _command.Parameters.Add(new SqlParameter("@Type", SqlDbType.VarChar)).Value = "Details";
                                _command.Parameters.Add(new SqlParameter("@BatchCode", SqlDbType.VarChar)).Value = oDataRowView.Row["BatchCode"];
                                _command.CommandTimeout = 0;
                                using (System.Data.DataSet ds = new System.Data.DataSet())
                                {
                                    ds.Clear();
                                    adp = new SqlDataAdapter(_command);
                                    adp.Fill(ds);
                                    connect.Close();

                                    if (ds.Tables[0].Rows[0]["returnCode"].ToString() == "00")
                                    {

                                        XtraRepAgencyDetails report = new XtraRepAgencyDetails();

                                        report.xrLabel32.Text = string.Format(" {0} STATE GOVERNMENT ", Program.StateName.ToUpper());
                                        report.xrLabel33.Text = string.Format("Summary of Collection by Agencies Details for the Month of  {0}", oDataRowView.Row["BatchName"]);

                                        //report.paramMonths.Value = dtpStart.Value.Month;
                                        //report.paramYears.Value = dtpStart.Value.Year;
                                        report.DataSource = ds.Tables[1];
                                        report.DataMember = "ViewReconciliationAgencyDetails";
                                        //report.DataAdapter
                                        report.ShowPreviewDialog();


                                    }
                                    else
                                    {
                                        return;
                                    }

                                }
                            }
                        }
                        else if (radioGroup1.EditValue == "Account")
                        {
                            using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
                            {
                                connect.Open();
                                _command = new SqlCommand("doIGRReport", connect) { CommandType = CommandType.StoredProcedure };
                                _command.Parameters.Add(new SqlParameter("@Type", SqlDbType.VarChar)).Value = "Account";
                                _command.Parameters.Add(new SqlParameter("@BatchCode", SqlDbType.VarChar)).Value = oDataRowView.Row["BatchCode"];
                                _command.CommandTimeout = 0;
                                using (System.Data.DataSet ds = new System.Data.DataSet())
                                {
                                    ds.Clear();
                                    adp = new SqlDataAdapter(_command);
                                    adp.Fill(ds);
                                    connect.Close();

                                    if (ds.Tables[0].Rows[0]["returnCode"].ToString() == "00")
                                    {

                                        //XtraRepIGRSummary repsumary = new XtraRepIGRSummary();

                                        XtraRepIGRAccounts repacct = new XtraRepIGRAccounts();

                                        //repsumary.xrLabel13.Text = string.Format("{0} State Government", Program.StateName);

                                        //repsumary.xrLabel14.Text = string.Format("Summary of {0} State Government IGR Accounts for {1}", Program.StateName, oDataRowView.Row["BatchName"]);

                                        repacct.xrLabel27.Text = string.Format(" {0} STATE GOVERNMENT ", Program.StateName.ToUpper());

                                        repacct.xrLabel28.Text = string.Format(" DETAILS OF TRANSFER / CHARGES FOR {0}", oDataRowView.Row["BatchName"]);

                                        repacct.DataSource = ds.Tables[1];
                                        repacct.DataMember = "table";
                                        repacct.ShowPreviewDialog();

                                    }
                                    else
                                    {
                                        return;
                                    }

                                }
                            }
                        }
                        else if (radioGroup1.EditValue == "Consolidate")
                        {
                            using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
                            {
                                connect.Open();
                                _command = new SqlCommand("doIGRReport", connect) { CommandType = CommandType.StoredProcedure };
                                _command.Parameters.Add(new SqlParameter("@Type", SqlDbType.VarChar)).Value = "Consolidate";
                                _command.Parameters.Add(new SqlParameter("@BatchCode", SqlDbType.VarChar)).Value = oDataRowView.Row["BatchCode"];
                                _command.CommandTimeout = 0;
                                using (System.Data.DataSet ds = new System.Data.DataSet())
                                {
                                    ds.Clear();
                                    adp = new SqlDataAdapter(_command);
                                    adp.Fill(ds);
                                    connect.Close();

                                    if (ds.Tables[0].Rows[0]["returnCode"].ToString() == "00")
                                    {
                                        XtraRepSummary repsummary = new XtraRepSummary();
                                        repsummary.xrLabel32.Text = string.Format("{0} STATE GOVERNMENT ", Program.StateName.ToUpper());
                                        repsummary.xrLabel33.Text = string.Format("CONSOLIDATED SUMMARY ACCOUNT OF IGR FOR THE MONTH OF {0}", oDataRowView.Row["BatchName"]);

                                        repsummary.DataSource = ds.Tables[1];

                                        repsummary.DataMember = "ViewConsolidateDetailsTransactions";

                                        //repsummary.paramPeriod.Value = dtpStart.Value.Month;
                                        //repsummary.paramYear.Value = dtpStart.Value.Year;
                                        repsummary.ShowPreviewDialog();

                                    }
                                    else
                                    {
                                        return;
                                    }

                                }
                            }
                        }
                        else if (radioGroup1.EditValue == "Bank Details")
                        {
                            using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
                            {
                                connect.Open();
                                _command = new SqlCommand("doIGRReport", connect) { CommandType = CommandType.StoredProcedure };
                                _command.Parameters.Add(new SqlParameter("@Type", SqlDbType.VarChar)).Value = "Bank Details";
                                _command.Parameters.Add(new SqlParameter("@BatchCode", SqlDbType.VarChar)).Value = oDataRowView.Row["BatchCode"];
                                _command.CommandTimeout = 0;
                                using (System.Data.DataSet ds = new System.Data.DataSet())
                                {
                                    ds.Clear();
                                    adp = new SqlDataAdapter(_command);
                                    adp.Fill(ds);
                                    connect.Close();

                                    if (ds.Tables[0].Rows[0]["returnCode"].ToString() == "00")
                                    {
                                        XtraRepBankDetails bankdetails = new XtraRepBankDetails();

                                        var list = (from DataRow row in ds.Tables[1].Rows
                                                    select new Dataset.BankDetails
                                                    {
                                                        Amount = Convert.ToDecimal(row["Amount"]),
                                                        PaymentDate = Convert.ToDateTime(row["PaymentDate"]),
                                                        BankName = row["BankName"] as string,
                                                        BankCode = row["BankCode"] as string,
                                                        Description = row["Description"] as string,
                                                        PayerName = row["PayerName"] as string,
                                                        RevenueCode = row["RevenueCode"] as string,
                                                        AccountNumber = row["AccountNumber"] as string,

                                                        //Branchname = row["Branchname"] as string
                                                    }
                                                                 ).ToList();

                                        bankdetails.xrLabel17.Text = string.Format("{0} State Government", Program.StateName);

                                        bankdetails.xrLabel18.Text = string.Format("Bank Details of {0} State Government IGR Accounts for {1}", Program.StateName, oDataRowView.Row["BatchName"]);

                                        //bankdetails.DataSource = ds.Tables[1];
                                        //bankdetails.DataMember = "table";

                                        var binding = (BindingSource)bankdetails.DataSource;

                                        binding.Clear();

                                        binding.DataSource = list;

                                        bankdetails.ShowPreviewDialog();

                                    }
                                    else
                                    {
                                        return;
                                    }

                                }
                            }
                        }
                        else if (radioGroup1.EditValue == "Summary")
                        {
                            using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
                            {
                                connect.Open();
                                _command = new SqlCommand("doIGRReport", connect) { CommandType = CommandType.StoredProcedure };
                                _command.Parameters.Add(new SqlParameter("@Type", SqlDbType.VarChar)).Value = "Summary";
                                _command.Parameters.Add(new SqlParameter("@BatchCode", SqlDbType.VarChar)).Value = oDataRowView.Row["BatchCode"];
                                _command.CommandTimeout = 0;

                                using (System.Data.DataSet ds = new System.Data.DataSet())
                                {
                                    ds.Clear();
                                    adp = new SqlDataAdapter(_command);
                                    adp.Fill(ds);
                                    connect.Close();

                                    if (ds.Tables[0].Rows[0]["returnCode"].ToString() == "00")
                                    {

                                        XtraRepIGRSummary repsumary = new XtraRepIGRSummary();

                                        repsumary.xrLabel13.Text = string.Format("{0} State Government", Program.StateName);

                                        repsumary.xrLabel14.Text = string.Format("Summary of {0} State Government IGR Accounts for {1}", Program.StateName, oDataRowView.Row["BatchName"]);
                                        repsumary.DataSource = ds.Tables[1];
                                        repsumary.DataMember = "table";
                                        repsumary.ShowPreviewDialog();

                                    }
                                    else
                                    {
                                        return;
                                    }

                                }
                            }
                        }

                    }
                    else
                    { }
                }
                catch (Exception ex)
                {
                    Tripous.Sys.ErrorBox(ex);
                }
                finally
                {
                    SplashScreenManager.CloseForm(false);
                }

            }
        }

        protected void ShowForm()
        {
            switch (iTransType)
            {
                case TransactionTypeCode.Null:
                    splitContainer1.Panel1Collapsed = true;
                    splitContainer1.Panel2Collapsed = false;
                    break;
                case TransactionTypeCode.New:
                    splitContainer1.Panel1Collapsed = false;
                    splitContainer1.Panel2Collapsed = false;
                    break;
                case TransactionTypeCode.Edit:
                    splitContainer1.Panel1Collapsed = false;
                    splitContainer1.Panel2Collapsed = false;
                    break;
                case TransactionTypeCode.Delete:
                    splitContainer1.Panel1Collapsed = true;
                    splitContainer1.Panel2Collapsed = false;
                    break;
                case TransactionTypeCode.Reload:
                    splitContainer1.Panel1Collapsed = true;
                    splitContainer1.Panel2Collapsed = false;
                    break;
                default:
                    splitContainer1.Panel1Collapsed = true;
                    splitContainer1.Panel2Collapsed = false;
                    break;
            }
        }

        public void RefreshForm()
        {
            NavBars.ToolStripEnableDisableControls(toolStrip, Tag as String);
        }

        void setDBComboBox()
        {
            DataTable Dt;

            using (var ds = new System.Data.DataSet())
            {
                using (SqlDataAdapter ada = new SqlDataAdapter((string)"SELECT BatchCode,BatchName FROM Reconciliation.tblBatch WHERE BatchCode IN ( SELECT DISTINCT BatchCode FROM Reconciliation.tblPeriods WHERE IsClosed = 1)", Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }

                Dt = ds.Tables[0];
            }

            Common.setComboList(cboBatch, Dt, "BatchCode", "BatchName");

            cboBatch.SelectedIndex = -1;

        }

    }
}
