using BankReconciliation.Class;
using DevExpress.Utils;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraSplashScreen;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TaxSmartSuite.Class;

namespace BankReconciliation.Forms
{
    public partial class FrmReconciliationperid : Form
    {
        public static FrmReconciliationperid publicStreetGroup;

        protected TransactionTypeCode iTransType; private bool Isbank = false; bool Ispyear = false;

        protected bool boolIsUpdate; private bool isRecord = false; bool isMonth = false;

        protected int ID; private SqlCommand _command; private SqlDataAdapter adp; int fperiod; DateTime dtStart; DateTime dtEnd;

        bool isFresh = false;

        public FrmReconciliationperid()
        {
            SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

            InitializeComponent();

            publicStreetGroup = this;

            setImages();

            ToolStripEvent();

            iTransType = TransactionTypeCode.Null;

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

            bttnCancel.Image = MDIMains.publicMDIParent.i32x32.Images[9];
            //bttnReset.Image = MDIMains.publicMDIParent.i32x32.Images[8];
            //bttnUpdate.Image = MDIMain.publicMDIParent.i32x32.Images[7];
            bttnUpdate.Image = MDIMains.publicMDIParent.i32x32.Images[7];

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
                groupControl2.Text = "Add New Record";
                iTransType = TransactionTypeCode.New;
                Clear();
                ShowForm();
                boolIsUpdate = false;
            }
            else if (sender == tsbEdit)
            {
                groupControl2.Text = "Edit Record Mode";
                iTransType = TransactionTypeCode.Edit;
                Clear();
                if (EditRecordMode())
                {
                    ShowForm();
                    boolIsUpdate = true;
                }
            }
            else if (sender == tsbDelete)
            {
                groupControl2.Text = "Delete Record Mode";

                iTransType = TransactionTypeCode.Delete;

                var row = gridView1.GetFocusedDataRow();
                if (row == null)
                {
                    Common.setMessageBox("No Record Selected for Delete", Program.ApplicationName, 3);
                    return;
                }
                string msg = string.Format("Delete {0} for period {1:dd/MM/yyyy} to {2:dd/MM/yyyy}.\nDo you want to continue?", row["Description"], row["StartDate"], row["EndDate"]);
                if (MosesClassLibrary.Utilities.Common.AskQuestion(msg, ""))
                {
                    //if ((ID.ToString() == "0"))
                    //{
                    //    Common.setMessageBox("No Record Selected for Delete", Program.ApplicationName, 3);
                    //    return;
                    //}
                    //else
                    deleteRecord(row["PeriodID"].ToString());
                    tsbReload.PerformClick(); boolIsUpdate = false;
                }
                else
                    tsbReload.PerformClick();
                boolIsUpdate = false;
            }
            else if (sender == tsbReload)
            {
                iTransType = TransactionTypeCode.Reload;
                setReload();
                ShowForm();
            }
            //bttnReset.PerformClick();
        }

        void OnFormLoad(object sender, EventArgs e)
        {
            ShowForm();
            setDBComboBox(); setDBComboBoxYear(); //setDBComboBoxMonth();

            setReload();


            cboBank.SelectedIndexChanged += CboBank_SelectedIndexChanged;

            cboBank.KeyPress += CboBank_KeyPress;

            dtpStart.ValueChanged += DtpStart_ValueChanged;

            dtpEnd.ValueChanged += DtpEnd_ValueChanged;

            cboYear.SelectedIndexChanged += CboYear_SelectedIndexChanged;

            cboMonth.SelectedIndexChanged += CboMonth_SelectedIndexChanged;

            cboAccount.SelectedIndexChanged += CboAccount_SelectedIndexChanged;

            gridView1.DoubleClick += GridView1_DoubleClick;

            bttnUpdate.Click += BttnUpdate_Click;
            //setDBComboBoxMonth();

        }

        private void CboAccount_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboAccount.SelectedIndex == -1)
                return;
            var respCode = CheckPeriodDate();
            if (string.IsNullOrWhiteSpace(respCode))
                return;
            //if (respCode == "01")
            //    isFresh = true;
            isFresh = respCode == "01";
        }

        private void GridView1_DoubleClick(object sender, EventArgs e)
        {
            tsbEdit.PerformClick();
        }

        private void BttnUpdate_Click(object sender, EventArgs e)
        {
            if (String.Compare(txtName.Text, "", false) == 0)
            {
                Common.setEmptyField("Period Name", Program.ApplicationName);
                txtName.Focus(); return;
            }
            else if (String.Compare(cboBank.Text, "", false) == 0)
            {
                Common.setEmptyField("Bank Name", Program.ApplicationName);
                cboBank.Focus(); return;
            }
            else if (String.Compare(cboAccount.Text, "", false) == 0)
            {
                Common.setEmptyField("Bank Account", Program.ApplicationName); cboAccount.Focus(); return;
            }
            else
            {
                //if (!CheckDateRange(dtStart, dtEnd, dtpStart.Value, dtpEnd.Value))
                //{
                //    Common.setMessageBox("Date not within Finanical Data Range", Program.ApplicationName, 2);
                //    return;
                //}
                //else
                //doinsert here
                if (!isFresh)
                    loaddates();
                using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
                {
                    connect.Open();
                    _command = new SqlCommand("doInsertReconciliationPeriod", connect) { CommandType = CommandType.StoredProcedure };
                    _command.Parameters.Add(new SqlParameter("@Description", SqlDbType.VarChar)).Value = txtName.Text.Trim();
                    _command.Parameters.Add(new SqlParameter("@BankAccountID", SqlDbType.Int)).Value = Convert.ToInt32(cboAccount.SelectedValue);
                    _command.Parameters.Add(new SqlParameter("@FinancialperiodID", SqlDbType.Int)).Value = fperiod;
                    _command.Parameters.Add(new SqlParameter("@BankShortCode", SqlDbType.VarChar)).Value = cboBank.SelectedValue;
                    _command.Parameters.Add(new SqlParameter("@StartDate", SqlDbType.Date)).Value = string.Format("{0:yyyy/MM/dd}", dtpStart.Value);
                    _command.Parameters.Add(new SqlParameter("@EndDate", SqlDbType.Date)).Value = string.Format("{0:yyyy/MM/dd}", dtpEnd.Value);
                    _command.Parameters.Add(new SqlParameter("@userid", SqlDbType.VarChar)).Value = Program.UserID;

                    _command.CommandTimeout = 0;

                    using (System.Data.DataSet ds = new System.Data.DataSet())
                    {
                        ds.Clear();
                        adp = new SqlDataAdapter(_command);
                        adp.Fill(ds);
                        connect.Close();

                        if (ds.Tables[0].Rows[0]["returnCode"].ToString() != "00")
                        {
                            Common.setMessageBox(ds.Tables[0].Rows[0]["returnMessage"].ToString(), Program.ApplicationName, 2); Clear();
                            return;
                        }
                        else
                        {
                            Common.setMessageBox(ds.Tables[0].Rows[0]["returnMessage"].ToString(), Program.ApplicationName, 2);

                            if (MessageBox.Show("Do you want to add another record?", Program.ApplicationName, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
                            {
                                bttnCancel.PerformClick(); tsbReload.PerformClick();

                            }
                            else
                            {
                                //bttnReset.PerformClick();

                                setReload(); Clear();
                            }

                        }

                    }
                }
            }
        }

        private void CboBank_KeyPress(object sender, KeyPressEventArgs e)
        {
            Methods.AutoComplete(cboBank, e, true);
        }

        private void CboMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (cboMonth.SelectedValue != null && !isMonth)
            //{
            //    CheckPeriodDate();
            //}
            if (isFresh)
            {
                loaddates();
            }
        }

        private void CboYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboYear.SelectedValue != null && cboYear.SelectedIndex > -1 && !Ispyear)
            {
                setDBComboBoxMonth();
            }
        }

        private void DtpEnd_ValueChanged(object sender, EventArgs e)
        {
            DateTime endDate = dtpEnd.Value;

            DateTime startDate = dtpStart.Value;

            int diffday = startDate.Subtract(endDate).Days;

            if (endDate <= startDate)
            {
                Common.setMessageBox("End Date must not be equally / less than Start  Date", Program.ApplicationName, 2);
            }

            getDateInfor(endDate);

            label43.Text = dtpEnd.Value.ToLongDateString();

        }

        private void DtpStart_ValueChanged(object sender, EventArgs e)
        {
            label42.Text = dtpStart.Value.ToLongDateString();
        }

        private void CboBank_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboBank.SelectedValue != null && !Isbank)
            {
                //getopenBal();
                setDBComboBoxAcct();
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

        public void setDBComboBox()
        {
            DataTable Dt;

            //connect.connect.Close();

            using (var ds = new System.Data.DataSet())
            {
                //connect.connect.Open();
                using (SqlDataAdapter ada = new SqlDataAdapter((string)"SELECT BankShortCode,BankName FROM Collection.tblBank", Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }

                Dt = ds.Tables[0];
            }

            Common.setComboList(cboBank, Dt, "BankShortCode", "BankName");

            cboBank.SelectedIndex = -1;


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

        public void setDBComboBoxMonth()
        {
            using (var ds = new System.Data.DataSet())
            {
                string qry = string.Format("SELECT Months,Periods FROM Reconciliation.tblFinancialperiod WHERE Year='{0}' AND FinancialperiodID NOT IN (SELECT FinancialperiodID FROM Reconciliation.tblCloseFinanicalPeriod)", cboYear.SelectedValue);

                using (SqlDataAdapter ada = new SqlDataAdapter(qry, Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }

                Common.setComboList(cboMonth, ds.Tables[0], "Periods", "Months");

            }
            cboMonth.SelectedIndex = -1;
        }

        public void setDBComboBoxYear()
        {
            using (var ds = new System.Data.DataSet())
            {
                using (SqlDataAdapter ada = new SqlDataAdapter((string)"SELECT DISTINCT Year FROM Reconciliation.tblFinancialperiod", Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }

                Common.setComboList(cboYear, ds.Tables[0], "Year", "Year");

            }
            cboYear.SelectedIndex = -1;
        }
        void getDateInfor(DateTime dtsdate)
        {
            string strqury = string.Format("SELECT  Year,Periods FROM Reconciliation.tblFinancialperiod WHERE EndDate='{0:yyyy/MM/dd}'", dtsdate);

            using (var ds = new System.Data.DataSet())
            {
                using (SqlDataAdapter ada = new SqlDataAdapter(strqury, Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }


                //ds.Tables[0].Rows[0]["returnCode"].ToString();

                if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {
                    int year = Convert.ToInt32(ds.Tables[0].Rows[0]["Year"].ToString());

                    string period = ds.Tables[0].Rows[0]["Periods"].ToString();

                    cboYear.SelectedValue = year; cboMonth.SelectedValue = period;
                }

            }
        }
        void loaddates()
        {
            DataTable dtsed = new DataTable();

            dtsed.Clear();

            using (var ds = new System.Data.DataSet())
            {
                ds.Clear();
                using (SqlDataAdapter ada = new SqlDataAdapter(string.Format("SELECT FinancialperiodID,StartDate,EndDate FROM Reconciliation.tblFinancialperiod WHERE Periods='{0}' AND Year='{1}'", cboMonth.SelectedValue, cboYear.SelectedValue), Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }

                dtsed = ds.Tables[0];

                string[] formats = { "dd/MM/yyyy" };

                if (dtsed != null && dtsed.Rows.Count > 0)
                {
                    //dtpStart.Enabled = true; dtpEnd.Enabled = true;

                    fperiod = Convert.ToInt32(dtsed.Rows[0]["FinancialperiodID"].ToString());

                    if (isFresh)
                    {
                        DateTime startDate = dtsed.Rows[0].Field<DateTime>("StartDate");
                        DateTime endDate = dtsed.Rows[0].Field<DateTime>("EndDate");

                        dtpStart.MaxDate = dtpEnd.MaxDate = new DateTime(2500, 1, 1);
                        dtpStart.MinDate = dtpEnd.MinDate = new DateTime(1999, 1, 1);
                        dtpStart.MaxDate = dtpEnd.MaxDate = endDate;
                        dtpStart.MinDate = dtpEnd.MinDate = startDate;
                        dtpEnd.Value = endDate;
                        dtpStart.Value = startDate;
                        //dtStart = Convert.ToDateTime(dtsed.Rows[0]["StartDate"].ToString());

                        //dtEnd = Convert.ToDateTime(dtsed.Rows[0]["EndDate"].ToString());

                        //dtpStart.Value = Convert.ToDateTime(dtsed.Rows[0]["StartDate"].ToString());

                        //dtpEnd.Value = Convert.ToDateTime(dtsed.Rows[0]["EndDate"].ToString());
                    }
                }
            }
        }

        private void setReload()
        {
            //connect.connect.Close();
            DataTable dt;
            //System.Data.DataSet ds;
            using (var ds = new System.Data.DataSet())
            {
                //connect.connect.Open();
                using (SqlDataAdapter ada = new SqlDataAdapter(@"SELECT PeriodID,tblBank.BankShortCode ,Description,
        BankName,
        tblReconciliatioPeriod.BankAccountID,
        AccountNumber,
        Months,
        Periods,
        Year,tblReconciliatioPeriod.StartDate,tblReconciliatioPeriod.EndDate
FROM    Reconciliation.tblReconciliatioPeriod
        INNER JOIN Collection.tblBank ON tblBank.BankShortCode = tblReconciliatioPeriod.BankShortCode
        INNER JOIN ViewCurrencyBankAccount ON ViewCurrencyBankAccount.BankAccountID = tblReconciliatioPeriod.BankAccountID
        INNER JOIN Reconciliation.tblFinancialperiod ON tblFinancialperiod.FinancialperiodID = tblReconciliatioPeriod.FinancialperiodID ORDER BY Year,Periods,tblBank.BankShortCode", Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }
                //connect.connect.Close();
                dt = ds.Tables[0];
                gridControl1.DataSource = dt.DefaultView;
            }
            gridView1.OptionsBehavior.Editable = false;

            gridView1.Columns["BankAccountID"].Visible = false;
            gridView1.Columns["PeriodID"].Visible = false;
            gridView1.Columns["BankShortCode"].Visible = false;


            //Status
            gridView1.BestFitColumns();
        }

        protected bool EditRecordMode()
        {
            bool bResponse = false;
            GridView view = (GridView)gridControl1.FocusedView;
            if (view != null)
            {
                DataRow dr = view.GetDataRow(view.FocusedRowHandle);
                if (dr != null)
                {
                    ID = Convert.ToInt32(dr["PeriodID"]);
                    bResponse = FillField(Convert.ToInt32(dr["PeriodID"]));
                }
                else
                {
                    Common.setMessageBox("No Record is seleected", "", 1);
                    bResponse = false;
                }
            }
            return bResponse;
        }

        private bool FillField(int fieldid)
        {
            bool bResponse = false;
            //load data from the table into the forms for edit

            DataTable dts = (new Logic()).getSqlStatement((String.Format("SELECT BankShortCode,BankAccountID,Description,Year,Months,tblReconciliatioPeriod.StartDate,tblReconciliatioPeriod.EndDate FROM Reconciliation.tblReconciliatioPeriod INNER JOIN Reconciliation.tblFinancialperiod ON tblFinancialperiod.FinancialperiodID = tblReconciliatioPeriod.FinancialperiodID WHERE PeriodID='{0}'", fieldid))).Tables[0];

            if (dts != null)
            {
                bResponse = true;
                cboBank.SelectedValue = dts.Rows[0]["BankShortCode"].ToString();
                cboAccount.SelectedValue = dts.Rows[0]["BankAccountID"].ToString();
                txtName.Text = dts.Rows[0]["Description"].ToString();
                cboYear.Text = dts.Rows[0]["Year"].ToString();
                cboMonth.Text = dts.Rows[0]["Months"].ToString();
                dtpStart.Value = Convert.ToDateTime(dts.Rows[0]["StartDate"]);
                dtpEnd.Value = Convert.ToDateTime(dts.Rows[0]["EndDate"]);
                dtpStart.Enabled = false;
            }
            else
                bResponse = false;

            return bResponse;
        }

        bool CheckDateRange(DateTime Startrange, DateTime EndRange, DateTime start, DateTime end)
        {
            if (start >= Startrange && end <= EndRange)
            {
                return true;
            }
            else
                return false;
        }

        private void Clear()
        {
            //txtStreetGroup.Clear();
            txtName.Clear();
            cboBank.SelectedValue = -1; cboAccount.SelectedValue = -1; cboYear.SelectedValue = -1; cboMonth.SelectedValue = -1;
            dtpStart.Enabled = false; //dtpEnd.Enabled = false;


        }

        void deleteRecord(string parameter2)
        {
            using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
            {
                connect.Open();
                _command = new SqlCommand("doDeleteReconciliationPeriod", connect) { CommandType = CommandType.StoredProcedure };
                _command.Parameters.Add(new SqlParameter("@periodID", SqlDbType.Int)).Value = Convert.ToInt32(parameter2);
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
                        return;
                    }

                }
            }
        }

        string CheckPeriodDate()
        {
            using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
            {
                connect.Open();
                _command = new SqlCommand("doCheckLoadReconciliationPeriodStartDate", connect) { CommandType = CommandType.StoredProcedure };
                _command.Parameters.Add(new SqlParameter("@Accountid", SqlDbType.Int)).Value = Convert.ToInt32(cboAccount.SelectedValue);
                _command.Parameters.Add(new SqlParameter("@bankcode", SqlDbType.VarChar)).Value = cboBank.SelectedValue.ToString();
                //_command.Parameters.Add(new SqlParameter("@financialPeriodID", SqlDbType.Int)).Value = Convert.ToInt32(cboMonth.SelectedValue);

                _command.CommandTimeout = 0;

                using (System.Data.DataSet ds = new System.Data.DataSet())
                {
                    ds.Clear();
                    adp = new SqlDataAdapter(_command);
                    adp.Fill(ds);
                    connect.Close();

                    string respCode = ds.Tables[0].Rows[0]["returnCode"].ToString();

                    if (respCode == "00" || respCode == "01")
                    {
                        //var objDate = ds.Tables[1].Rows[0]["startdate"];
                        //DateTime? dtStartDate= (objDate==null || objDate == DBNull.Value)? null: 
                        // dtpStart.MaxDate = new DateTime(2500, 1, 1);

                        dtpStart.MinDate = dtpEnd.MinDate = new DateTime(1999, 1, 1);

                        DateTime? dtStartDate = ds.Tables[1].Rows[0].Field<DateTime?>("startdate");

                        if (ds.Tables.Count >= 3 && ds.Tables[2].Rows.Count > 0)
                        {
                            var table = ds.Tables[2];

                            //int year = table.Rows[0].Field<int>("Year");
                            //int finPeriodID = table.Rows[0].Field<int>("FinancialperiodID");
                            string period = table.Rows[0].Field<string>("Periods");
                            //DateTime startDate = table.Rows[0].Field<DateTime>("StartDate");
                            DateTime endDate = table.Rows[0].Field<DateTime>("EndDate");

                            //cboYear.SelectedValue = year;
                            //  setDBComboBoxMonth();
                            // cboMonth.SelectedValue = period;

                            dtpStart.MaxDate = dtStartDate.Value;
                            dtpStart.MinDate = dtStartDate.Value;
                            //dtpEnd.Value = endDate;
                            dtpStart.Value = dtStartDate.Value;// Convert.ToDateTime(ds.Tables[1].Rows[0]["startdate"]);
                        }
                        else
                        {

                        }
                        dtpStart.Enabled = cboYear.Enabled = cboMonth.Enabled = respCode == "01";
                        return respCode;// Convert.ToDateTime(ds.Tables[1].Rows[0]["startdate"]);
                    }
                    else
                    {
                        //dtpStart.Value = Convert.ToDateTime(ds.Tables[1].Rows[0]["startdate"]);

                        //dtpStart.Enabled = false;
                        Common.setMessageBox(ds.Tables[0].Rows[0]["returnMessage"].ToString(), Program.ApplicationName, 2);
                        return null;
                    }

                }
            }
        }

        private void DeleteSelectedRows(DevExpress.XtraGrid.Views.Grid.GridView view)
        {

            if (view == null || view.SelectedRowsCount == 0) return;



            DataRow[] rows = new DataRow[view.SelectedRowsCount];

            for (int i = 0; i < view.SelectedRowsCount; i++)

                rows[i] = view.GetDataRow(view.GetSelectedRows()[i]);



            view.BeginSort();

            try
            {

                foreach (DataRow row in rows)

                    row.Delete();

            }

            finally
            {

                view.EndSort();

            }

        }

    }
}
