using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TaxSmartSuite.Class;
using BankReconciliation.Class;
using System.Data.SqlClient;
using DevExpress.Utils;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraEditors.Controls;
using DevExpress.Data;

namespace BankReconciliation.Forms
{
    public partial class FrmCompareValue : Form
    {
        public static FrmCompareValue publicStreetGroup;

        protected TransactionTypeCode iTransType;

        protected bool boolIsUpdate;

        protected int ID;

        bool isFirst = true;

        GridColumn colView = new GridColumn();
        GridColumn colView2 = new GridColumn();

        DataTable dtc = new DataTable();
        DataTable dt = new DataTable();
        DataTable dbDebit = new DataTable();
        DataTable dbcredit = new DataTable();

        private SqlCommand _command; private SqlDataAdapter adp; private DataTable Dts;

        RepositoryItemComboBox repCombobox = new RepositoryItemComboBox();
        RepositoryItemGridLookUpEdit repComboLookBox = new RepositoryItemGridLookUpEdit();
        RepositoryItemGridLookUpEdit repComboLookBoxCredit = new RepositoryItemGridLookUpEdit();

        public DataTable dtEqual = new DataTable();

        public FrmCompareValue(DataTable dts, DataTable dts2)
        {
            InitializeComponent();

            publicStreetGroup = this;

            //setImages();

            ToolStripEvent();

            iTransType = TransactionTypeCode.Null;

            Load += OnFormLoad;

            OnFormLoad(null, null);

            gridControl1.DataSource = dts;

            //load exir record into a gridview
            gridView1.Columns["DEBIT"].DisplayFormat.FormatType = FormatType.Numeric;
            gridView1.Columns["DEBIT"].DisplayFormat.FormatString = "n2";
            gridView1.Columns["DATE"].DisplayFormat.FormatType = FormatType.DateTime;
            gridView1.Columns["DATE"].DisplayFormat.FormatString = "dd/MM/yyyy";
            gridView1.Columns["CREDIT"].DisplayFormat.FormatType = FormatType.Numeric;
            gridView1.Columns["CREDIT"].DisplayFormat.FormatString = "n2";
            gridView1.Columns["BALANCE"].DisplayFormat.FormatType = FormatType.Numeric;
            gridView1.Columns["BALANCE"].DisplayFormat.FormatString = "n2";

            gridView1.Columns["DEBIT"].SortOrder = ColumnSortOrder.Ascending;
            //gridView1.Columns["StartDate"].Visible = false;
            //gridView1.Columns["EndDate"].Visible = false;
            gridView1.OptionsBehavior.Editable = false;
            gridView1.BestFitColumns();

            //load refrseh record
            gridControl2.DataSource = dts2;
            gridView2.Columns["DEBIT"].DisplayFormat.FormatType = FormatType.Numeric;
            gridView2.Columns["DEBIT"].DisplayFormat.FormatString = "n2";
            gridView2.Columns["DATE"].DisplayFormat.FormatType = FormatType.DateTime;
            gridView2.Columns["DATE"].DisplayFormat.FormatString = "dd/MM/yyyy";
            gridView2.Columns["CREDIT"].DisplayFormat.FormatType = FormatType.Numeric;
            gridView2.Columns["CREDIT"].DisplayFormat.FormatString = "n2";
            gridView2.Columns["BALANCE"].DisplayFormat.FormatType = FormatType.Numeric;
            gridView2.Columns["BALANCE"].DisplayFormat.FormatString = "n2";
            gridView2.Columns["DEBIT"].SortOrder = ColumnSortOrder.Ascending;
            gridView2.OptionsBehavior.Editable = false;
            gridView2.BestFitColumns();

            dtEqual = new DataTable();
            dtEqual.BeginInit();
            dtEqual.Columns.Add("DATE", typeof(DateTime));
            dtEqual.Columns.Add("DEBIT", typeof(decimal));
            dtEqual.Columns.Add("CREDIT", typeof(decimal));
            dtEqual.Columns.Add("BALANCE", typeof(decimal));
            dtEqual.Columns.Add("REVENUECODE", typeof(string));
            dtEqual.Columns.Add("PAYERNAME", typeof(string));
            dtEqual.EndInit();
            dtEqual.Rows.Clear();

            foreach (DataRow row in dts2.Rows)
            {
                var whereClause = string.Format("DATE = '{0}' AND DEBIT = {1} AND CREDIT = {2}", row["DATE"], AvoidNullInt(row["DEBIT"]), AvoidNullInt(row["CREDIT"]));
                var rows = dts.Select(whereClause);
                if (rows.Count() == 0)
                    dtEqual.ImportRow(row);
            }

            gridControl3.DataSource = dtEqual;
            gridView3.Columns["DEBIT"].DisplayFormat.FormatType = FormatType.Numeric;
            gridView3.Columns["DEBIT"].DisplayFormat.FormatString = "n2";
            gridView3.Columns["DATE"].DisplayFormat.FormatType = FormatType.DateTime;
            gridView3.Columns["DATE"].DisplayFormat.FormatString = "dd/MM/yyyy";
            gridView3.Columns["CREDIT"].DisplayFormat.FormatType = FormatType.Numeric;
            gridView3.Columns["CREDIT"].DisplayFormat.FormatString = "n2";
            gridView3.Columns["BALANCE"].DisplayFormat.FormatType = FormatType.Numeric;
            gridView3.Columns["BALANCE"].DisplayFormat.FormatString = "n2";
            gridView3.OptionsBehavior.Editable = false;
            gridView3.BestFitColumns();

            btnAllocate.Click += btnAllocate_Click;
            btnCancel.Click += btnCancel_Click;
            
        }

        void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

        void btnAllocate_Click(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                DialogResult result = MessageBox.Show("Are you sure to continue.....", Program.ApplicationName, MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    DialogResult = System.Windows.Forms.DialogResult.OK;
                }
                else
                    DialogResult = System.Windows.Forms.DialogResult.Cancel;
            }
            else
                Common.setMessageBox("Please click on the Check box",Program.ApplicationName,1);
            return;

        }

        //void cboYears_KeyPress(object sender, KeyPressEventArgs e)
        //{ Methods.AutoComplete(cboYears, e, true); }

        //void cboPeriod_KeyPress(object sender, KeyPressEventArgs e)
        //{ Methods.AutoComplete(cboPeriod, e, true); }

        //void cboBank_KeyPress(object sender, KeyPressEventArgs e)
        //{ Methods.AutoComplete(cboBank, e, true); }

        //private void setImages()
        //{
        //    tsbNew.Image = MDIMains.publicMDIParent.i16x16.Images[5];
        //    tsbEdit.Image = MDIMains.publicMDIParent.i16x16.Images[6];
        //    tsbDelete.Image = MDIMains.publicMDIParent.i16x16.Images[7];
        //    tsbReload.Image = MDIMains.publicMDIParent.i16x16.Images[0];
        //    tsbClose.Image = MDIMains.publicMDIParent.i16x16.Images[11];


        //    btnAllocate.Image = MDIMains.publicMDIParent.i32x32.Images[7];
        //    //btnSearch.Image = MDIMains.publicMDIParent.i32x32.Images[40];

        //}

        void ToolStripEvent()
        {
            //tsbClose.Click += OnToolStripItemsClicked;
            //tsbNew.Click += OnToolStripItemsClicked;
            //tsbEdit.Click += OnToolStripItemsClicked;
            //tsbDelete.Click += OnToolStripItemsClicked;
            //tsbReload.Click += OnToolStripItemsClicked;
        }

        void OnToolStripItemsClicked(object sender, EventArgs e)
        {
            //    if (sender == tsbClose)
            //    {
            //        MDIMains.publicMDIParent.RemoveControls();
            //    }
            //    else if (sender == tsbNew)
            //    {
            //        //groupControl2.Text = "Add New Record";
            //        iTransType = TransactionTypeCode.New;
            //        ShowForm();
            //        boolIsUpdate = false;
            //    }
            //    else if (sender == tsbEdit)
            //    {
            //        //groupControl2.Text = "Edit Record Mode";
            //        iTransType = TransactionTypeCode.Edit;
            //        //if (EditRecordMode())
            //        //{
            //        ShowForm();
            //        boolIsUpdate = true;
            //        //}
            //    }
            //    else if (sender == tsbDelete)
            //    {
            //        //groupControl2.Text = "Delete Record Mode";
            //        iTransType = TransactionTypeCode.Delete;
            //        if (MosesClassLibrary.Utilities.Common.AskQuestion("Deleting this record will delete attached record.\nDo you want to continue?", ""))
            //        {
            //        }
            //        else
            //            tsbReload.PerformClick();

            //        boolIsUpdate = false;
            //    }
            //    else if (sender == tsbReload)
            //    {
            //        iTransType = TransactionTypeCode.Reload;
            //        ShowForm();
            //    }
            //bttnReset.PerformClick();
        }

        void OnFormLoad(object sender, EventArgs e)
        {
            //ShowForm();
            //setDBComboBoxBank();
            //setDBComboBoxPeriod();
            //setDBComboBoxPeriods();
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
            //NavBars.ToolStripEnableDisableControls(toolStrip, Tag as String);
        }

        //public void setDBComboBoxBank()
        //{
        //    DataTable Dt;

        //    using (var ds = new System.Data.DataSet())
        //    {
        //        using (SqlDataAdapter ada = new SqlDataAdapter((string)"select *  from tblBank", Logic.ConnectionString))
        //        {
        //            ada.Fill(ds, "table");
        //        }

        //        Dt = ds.Tables[0];
        //    }

        //    Common.setComboList(cboBank, Dt, "BankShortCode", "BankName");

        //    cboBank.SelectedIndex = -1;


        //}

        //void setDBComboBoxPeriod()
        //{
        //    DataTable Dt;

        //    using (var ds = new System.Data.DataSet())
        //    {
        //        using (SqlDataAdapter ada = new SqlDataAdapter((string)"SELECT DISTINCT months,Periods FROM tblPeriods ORDER BY Periods", Logic.ConnectionString))
        //        {
        //            ada.Fill(ds, "table");
        //        }

        //        Dt = ds.Tables[0];
        //    }

        //    Common.setComboList(cboPeriod, Dt, "Periods", "months");

        //}

        //void setDBComboBoxPeriods()
        //{
        //    DataTable Dt;

        //    using (var ds = new System.Data.DataSet())
        //    {
        //        using (SqlDataAdapter ada = new SqlDataAdapter((string)"SELECT DISTINCT YEAR FROM tblPeriods", Logic.ConnectionString))
        //        {
        //            ada.Fill(ds, "table");
        //        }

        //        Dt = ds.Tables[0];
        //    }

        //    Common.setComboList(cboYears, Dt, "YEAR", "YEAR");

        //}

        void Bttn_Click(object sender, EventArgs e)
        {
            //if (sender == btnSearch)
            //{
            //    colView.Name = "Description";
            //    colView.FieldName = "Description";

            //    colView2.Name = "Description";
            //    colView2.FieldName = "Description";

            //    if (!boolIsUpdate)
            //    {
            //        setReload();

            //        setReloads();
            //    }
            //    else
            //    {
            //        setReloadCredit();
            //        setReloadDebit();
            //    }
            //}
            //else 
            if (sender == btnAllocate)
            {
                updateRecord();
            }
        }

        private void setReload()
        {
            //connect.connect.Close();

            ////System.Data.DataSet ds;
            //using (var ds = new System.Data.DataSet())
            //{
            //    //connect.connect.Open();
            //    string qury = string.Format("SELECT CONVERT(VARCHAR, BSDate, 103)  AS Date, Debit AS Amount  FROM dbo.tblbankstatement WHERE Debit IS NOT NULL AND BankCode='{0}' AND Period='{1}' AND Years='{2}'", cboBank.SelectedValue, cboPeriod.SelectedValue, cboYears.SelectedValue);
            //    using (SqlDataAdapter ada = new SqlDataAdapter(qury, Logic.ConnectionString))
            //    {
            //        ada.Fill(ds, "table");
            //    }
            //    //connect.connect.Close();
            //    dt = ds.Tables[0];

            //    dt.Columns.Add("Description", typeof(String));

            //    gridControl1.DataSource = dt;
            //}

            //AddCombDebit();

            //gridView1.Columns["Amount"].DisplayFormat.FormatType = FormatType.Numeric;
            //gridView1.Columns["Amount"].DisplayFormat.FormatString = "n2";
            //gridView1.Columns["Date"].DisplayFormat.FormatType = FormatType.DateTime;
            //gridView1.Columns["Date"].DisplayFormat.FormatString = "dd/MM/yyyy";

            ////var col= gridView1.Columns.Add();
            //colView = gridView1.Columns["Description"];
            //colView.ColumnEdit = repComboLookBox;

            //colView.Visible = true;
            ////OptionsColumn.AllowEdit = false
            //gridView1.Columns["Amount"].OptionsColumn.AllowEdit = false;
            //gridView1.Columns["Date"].OptionsColumn.AllowEdit = false;

            //gridView1.BestFitColumns();
        }

        void AddCombDebit()
        {
            DataTable dtsed = new DataTable();

            repComboLookBox.DataSource = null;

            //System.Data.DataSet ds;
            using (var ds = new System.Data.DataSet())
            {
                using (SqlDataAdapter ada = new SqlDataAdapter("SELECT Description FROM Reconciliation.tblTransDefinition WHERE Type='dr' AND IsActive=1", Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }

                repComboLookBox.NullText = "select";

                dtsed = ds.Tables[0];

                if (dtsed != null && dtsed.Rows.Count > 0)
                {
                    repComboLookBox.DataSource = dtsed;
                    repComboLookBox.DisplayMember = "Description";
                    repComboLookBox.ValueMember = "Description";

                    //repComboLookBox.TextEditStyle = TextEditStyles.Standard;
                    //repComboLookBox.View.OptionsBehavior.Editable = true;
                    //repComboLookBox.TextEditStyle = TextEditStyles.Standard;
                    ////No visual indication
                    //repComboLookBox.ShowDropDown = ShowDropDown.Never;
                    //repComboLookBox.Buttons[0].Visible = false;
                    //Autocomplete on all values
                    repComboLookBox.AllowNullInput = DefaultBoolean.True;
                    repComboLookBox.AutoComplete = true;
                }

            }
        }

        private void setReloads()
        {

            //DataTable dtc;
            //setReloadsExtracted();
            ////System.Data.DataSet ds;
            //using (var dsc = new System.Data.DataSet())
            //{
            //    //connect.connect.Open();
            //    string qury = string.Format("SELECT CONVERT(VARCHAR, BSDate, 103)  AS Date, Credit AS Amount  FROM dbo.tblbankstatement WHERE Credit IS NOT NULL AND BankCode='{0}' AND Period='{1}' AND Years='{2}'", cboBank.SelectedValue, cboPeriod.SelectedValue, cboYears.SelectedValue);

            //    using (SqlDataAdapter ada = new SqlDataAdapter(qury, Logic.ConnectionString))
            //    {
            //        ada.Fill(dsc, "table");
            //    }
            //    //connect.connect.Close();
            //    dtc = dsc.Tables[0];

            //    dtc.Columns.Add("Description", typeof(String));

            //    gridControl2.DataSource = dtc;
            //}

            //AddCombCredit();

            //gridView2.Columns["Amount"].DisplayFormat.FormatType = FormatType.Numeric;
            //gridView2.Columns["Amount"].DisplayFormat.FormatString = "n2";
            //gridView2.Columns["Date"].DisplayFormat.FormatType = FormatType.DateTime;
            //gridView2.Columns["Date"].DisplayFormat.FormatString = "dd/MM/yyyy";

            ////var col= gridView1.Columns.Add();
            //colView2 = gridView2.Columns["Description"];
            //colView2.ColumnEdit = repComboLookBoxCredit;

            //colView2.Visible = true;
            ////OptionsColumn.AllowEdit = false
            //gridView2.Columns["Amount"].OptionsColumn.AllowEdit = false;
            //gridView2.Columns["Date"].OptionsColumn.AllowEdit = false;

            //gridView2.BestFitColumns();
        }

        void AddCombCredit()
        {
            DataTable dtse = new DataTable();

            repComboLookBoxCredit.DataSource = null;

            //System.Data.DataSet ds;
            using (var dsed = new System.Data.DataSet())
            {
                using (SqlDataAdapter adas = new SqlDataAdapter("SELECT Description FROM Reconciliation.tblTransDefinition WHERE Type='cr' AND IsActive=1", Logic.ConnectionString))
                {
                    adas.Fill(dsed, "table");
                }

                repComboLookBoxCredit.NullText = "select";

                dtse = dsed.Tables[0];

                if (dtse != null && dtse.Rows.Count > 0)
                {
                    repComboLookBoxCredit.DataSource = dtse;
                    repComboLookBoxCredit.DisplayMember = "Description";
                    repComboLookBoxCredit.ValueMember = "Description";


                    //repComboLookBoxCredit.TextEditStyle = TextEditStyles.Standard;
                    //repComboLookBoxCredit.View.OptionsBehavior.Editable = true;
                    //repComboLookBoxCredit.TextEditStyle = TextEditStyles.Standard;
                    ////No visual indication
                    //repComboLookBoxCredit.ShowDropDown = ShowDropDown.Never;
                    //repComboLookBoxCredit.Buttons[0].Visible = false;
                    repComboLookBoxCredit.AllowNullInput = DefaultBoolean.True;
                    //Autocomplete on all values
                    repComboLookBoxCredit.AutoComplete = true;
                }

            }
        }

        void updateRecord()
        {
            //if (!boolIsUpdate)
            //{
            //    //credit transaction
            //    using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
            //    {
            //        connect.Open();
            //        _command = new SqlCommand("AllocateTransaction", connect) { CommandType = CommandType.StoredProcedure };
            //        _command.Parameters.Add(new SqlParameter("@BankCode", SqlDbType.VarChar)).Value = cboBank.SelectedValue;
            //        _command.Parameters.Add(new SqlParameter("@Months", SqlDbType.VarChar)).Value = cboPeriod.SelectedValue;
            //        _command.Parameters.Add(new SqlParameter("@Years", SqlDbType.VarChar)).Value = cboYears.SelectedValue;
            //        _command.Parameters.Add(new SqlParameter("@pType", SqlDbType.Bit)).Value = true;
            //        _command.Parameters.Add(new SqlParameter("@pStatus", SqlDbType.Bit)).Value = boolIsUpdate;
            //        _command.Parameters.Add(new SqlParameter("@pTransactiondb", SqlDbType.Structured)).Value = dtc;

            //        using (System.Data.DataSet ds = new System.Data.DataSet())
            //        {
            //            adp = new SqlDataAdapter(_command);
            //            adp.Fill(ds);
            //            Dts = ds.Tables[0];
            //            connect.Close();
            //        }
            //    }
            //    //debit transaction
            //    using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
            //    {
            //        connect.Open();
            //        _command = new SqlCommand("AllocateTransaction", connect) { CommandType = CommandType.StoredProcedure };
            //        _command.Parameters.Add(new SqlParameter("@BankCode", SqlDbType.VarChar)).Value = cboBank.SelectedValue;
            //        _command.Parameters.Add(new SqlParameter("@Months", SqlDbType.VarChar)).Value = cboPeriod.SelectedValue;
            //        _command.Parameters.Add(new SqlParameter("@Years", SqlDbType.VarChar)).Value = cboYears.SelectedValue;
            //        _command.Parameters.Add(new SqlParameter("@pType", SqlDbType.Bit)).Value = false;
            //        _command.Parameters.Add(new SqlParameter("@pStatus", SqlDbType.Bit)).Value = boolIsUpdate;
            //        _command.Parameters.Add(new SqlParameter("@pTransactiondb", SqlDbType.Structured)).Value = dt;

            //        using (System.Data.DataSet ds = new System.Data.DataSet())
            //        {
            //            adp = new SqlDataAdapter(_command);
            //            adp.Fill(ds);
            //            Dts = ds.Tables[0];
            //            connect.Close();

            //            if (ds.Tables[0].Rows[0]["returnCode"].ToString() != "00")
            //            {
            //                Common.setMessageBox(ds.Tables[0].Rows[0]["returnMessage"].ToString(), Program.ApplicationName, 3);
            //            }
            //            else
            //            {
            //                Common.setMessageBox(ds.Tables[0].Rows[0]["returnMessage"].ToString(), Program.ApplicationName, 1);
            //            }
            //        }
            //    }
            //}
            //else
            //{
            //    //credit transaction
            //    using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
            //    {
            //        connect.Open();
            //        _command = new SqlCommand("AllocateTransaction", connect) { CommandType = CommandType.StoredProcedure };
            //        _command.Parameters.Add(new SqlParameter("@BankCode", SqlDbType.VarChar)).Value = cboBank.SelectedValue;
            //        _command.Parameters.Add(new SqlParameter("@Months", SqlDbType.VarChar)).Value = cboPeriod.SelectedValue;
            //        _command.Parameters.Add(new SqlParameter("@Years", SqlDbType.VarChar)).Value = cboYears.SelectedValue;
            //        _command.Parameters.Add(new SqlParameter("@pType", SqlDbType.Bit)).Value = true;
            //        _command.Parameters.Add(new SqlParameter("@pStatus", SqlDbType.Bit)).Value = boolIsUpdate;
            //        _command.Parameters.Add(new SqlParameter("@pTransactiondb", SqlDbType.Structured)).Value = dbcredit;

            //        using (System.Data.DataSet ds = new System.Data.DataSet())
            //        {
            //            adp = new SqlDataAdapter(_command);
            //            adp.Fill(ds);
            //            Dts = ds.Tables[0];
            //            connect.Close();
            //        }
            //    }
            //    //debit transaction
            //    using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
            //    {
            //        connect.Open();
            //        _command = new SqlCommand("AllocateTransaction", connect) { CommandType = CommandType.StoredProcedure };
            //        _command.Parameters.Add(new SqlParameter("@BankCode", SqlDbType.VarChar)).Value = cboBank.SelectedValue;
            //        _command.Parameters.Add(new SqlParameter("@Months", SqlDbType.VarChar)).Value = cboPeriod.SelectedValue;
            //        _command.Parameters.Add(new SqlParameter("@Years", SqlDbType.VarChar)).Value = cboYears.SelectedValue;
            //        _command.Parameters.Add(new SqlParameter("@pType", SqlDbType.Bit)).Value = false;
            //        _command.Parameters.Add(new SqlParameter("@pStatus", SqlDbType.Bit)).Value = boolIsUpdate;
            //        _command.Parameters.Add(new SqlParameter("@pTransactiondb", SqlDbType.Structured)).Value = dbDebit;

            //        using (System.Data.DataSet ds = new System.Data.DataSet())
            //        {
            //            adp = new SqlDataAdapter(_command);
            //            adp.Fill(ds);
            //            Dts = ds.Tables[0];
            //            connect.Close();

            //            if (ds.Tables[0].Rows[0]["returnCode"].ToString() != "00")
            //            {
            //                Common.setMessageBox(ds.Tables[0].Rows[0]["returnMessage"].ToString(), Program.ApplicationName, 3);
            //            }
            //            else
            //            {
            //                Common.setMessageBox(ds.Tables[0].Rows[0]["returnMessage"].ToString(), Program.ApplicationName, 1);
            //            }
            //        }
            //    }
            //}

        }

        void setReloadDebit()
        {
            //using (var dsc = new System.Data.DataSet())
            //{
            //    //connect.connect.Open();
            //    string qury = string.Format("SELECT CONVERT(VARCHAR, TransDate, 103)  AS Date ,TransAmount AS Amount,TransDescription AS Description FROM tblAllocateDebit WHERE BankCode='{0}' AND Months='{1}' AND Years='{2}'", cboBank.SelectedValue, cboPeriod.SelectedValue, cboYears.SelectedValue);

            //    using (SqlDataAdapter ada = new SqlDataAdapter(qury, Logic.ConnectionString))
            //    {
            //        ada.Fill(dsc, "table");
            //    }
            //    //connect.connect.Close();
            //    dbDebit = dsc.Tables[0];

            //    //dtc.Columns.Add("Description", typeof(String));

            //    gridControl1.DataSource = dbDebit;
            //}

            //AddCombDebit();

            //gridView1.Columns["Amount"].DisplayFormat.FormatType = FormatType.Numeric;
            //gridView1.Columns["Amount"].DisplayFormat.FormatString = "n2";
            //gridView1.Columns["Date"].DisplayFormat.FormatType = FormatType.DateTime;
            //gridView1.Columns["Date"].DisplayFormat.FormatString = "dd/MM/yyyy";

            ////var col= gridView1.Columns.Add();
            //colView = gridView1.Columns["Description"];
            //colView.ColumnEdit = repComboLookBox;

            //colView.Visible = true;
            ////OptionsColumn.AllowEdit = false
            //gridView1.Columns["Amount"].OptionsColumn.AllowEdit = false;
            //gridView1.Columns["Date"].OptionsColumn.AllowEdit = false;

            //gridView1.BestFitColumns();
        }

        void setReloadCredit()
        {
            //    using (var dsc = new System.Data.DataSet())
            //    {
            //        //connect.connect.Open();
            //    //    string qury = string.Format("SELECT CONVERT(VARCHAR, TransDate, 103)  AS Date ,TransAmount AS Amount,TransDescription AS Description FROM tblAllocateCredit WHERE BankCode='{0}' AND Months='{1}' AND Years='{2}'", cboBank.SelectedValue, cboPeriod.SelectedValue, cboYears.SelectedValue);

            //    //    using (SqlDataAdapter ada = new SqlDataAdapter(qury, Logic.ConnectionString))
            //    //    {
            //    //        ada.Fill(dsc, "table");
            //    //    }
            //    //    //connect.connect.Close();
            //    //    dbcredit = dsc.Tables[0];

            //    //    //dtc.Columns.Add("Description", typeof(String));

            //    //    gridControl2.DataSource = dbcredit;
            //    //}

            //    //AddCombCredit();

            //    //gridView2.Columns["Amount"].DisplayFormat.FormatType = FormatType.Numeric;
            //    //gridView2.Columns["Amount"].DisplayFormat.FormatString = "n2";
            //    //gridView2.Columns["Date"].DisplayFormat.FormatType = FormatType.DateTime;
            //    //gridView2.Columns["Date"].DisplayFormat.FormatString = "dd/MM/yyyy";

            //    ////var col= gridView1.Columns.Add();
            //    //colView2 = gridView2.Columns["Description"];
            //    //colView2.ColumnEdit = repComboLookBoxCredit;

            //    //colView2.Visible = true;
            //    ////OptionsColumn.AllowEdit = false
            //    //gridView2.Columns["Amount"].OptionsColumn.AllowEdit = false;
            //    //gridView2.Columns["Date"].OptionsColumn.AllowEdit = false;

            //    //gridView2.BestFitColumns();
            //}
        }

        decimal AvoidNullInt(object value)
        {
            decimal iRet = 0m;


            if (value is DBNull)
                iRet = 0;
            else if (value is decimal)
                iRet = Convert.ToDecimal(value);
            return iRet;
        }
    }
}
