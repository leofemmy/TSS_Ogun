using BankReconciliation.Class;
using DevExpress.Utils;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraSplashScreen;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;
using TaxSmartSuite.Class;

namespace BankReconciliation.Forms
{
    public partial class FrmReclassified : Form
    {
        public static FrmReclassified publicStreetGroup; private bool Isbank = false; private bool isRecord = false; private SqlCommand _command; private SqlDataAdapter adp; int periodid; DataTable dtcold; DataTable table; DataTable Dts;
        GridColumn colView2 = new GridColumn(); RepositoryItemGridLookUpEdit repComboLookBoxCredit = new RepositoryItemGridLookUpEdit(); GridColumn colView = new GridColumn();
        public FrmReclassified()
        {
            SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

            InitializeComponent();

            publicStreetGroup = this;

            setImages();

            ToolStripEvent();

            Load += OnFormLoad;

            OnFormLoad(null, null);

            //openForm();

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
            //bttncompare.Image = MDIMains.publicMDIParent.i32x32.Images[8];
            ////bttnClose.Image = MDIMain.publicMDIParent.i32x32.Images[9];
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
            //isRecord2 = false;

            setDBComboBox();

            setDBComboBoxPeriod();

            //setDBComboBoxyr();

            cboBank.SelectedIndexChanged += CboBank_SelectedIndexChanged;

            cboRecPeriod.SelectedIndexChanged += CboRecPeriod_SelectedIndexChanged;

            //cboYear.SelectedIndexChanged += CboYear_SelectedIndexChanged;

            sbnSearch.Click += SbnSearch_Click;

            sbnReset.Click += SbnReset_Click;

            sbnPreview.Click += SbnPreview_Click;

            sbnUpdate.Click += SbnUpdate_Click;


        }

        private void CboRecPeriod_SelectedIndexChanged(object sender, EventArgs e)
        {
            setReloadCD();
        }

        private void SbnUpdate_Click(object sender, EventArgs e)
        {
            if (table == null || table.Rows.Count <= 0)
            {
                Common.setMessageBox("No Record Selected for Reclassification", Program.ApplicationName, 3);
                return;
            }

            try
            {
                using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
                {
                    if (table.Columns.Contains("Date")) table.Columns.Remove("Date");
                    if (table.Columns.Contains("Amount")) table.Columns.Remove("Amount");
                    if (table.Columns.Contains("PaymentRefNumber")) table.Columns.Remove("PaymentRefNumber");


                    connect.Open();
                    _command = new SqlCommand("ReclassifiedTransaction", connect) { CommandType = CommandType.StoredProcedure };
                    _command.Parameters.Add(new SqlParameter("@userid", SqlDbType.VarChar)).Value = Program.UserID;
                    _command.Parameters.Add(new SqlParameter("@pTransaction", SqlDbType.Structured)).Value = table;


                    using (System.Data.DataSet ds = new System.Data.DataSet())
                    {
                        ds.Clear();
                        adp = new SqlDataAdapter(_command);
                        adp.Fill(ds);
                        //Dts = ds.Tables[0];
                        connect.Close();

                        if (ds.Tables[0].Rows[0]["returnCode"].ToString() != "00")
                        {
                            Common.setMessageBox(ds.Tables[0].Rows[0]["returnMessage"].ToString(), Program.ApplicationName, 3);
                            return;
                        }
                        else
                        {
                            Common.setMessageBox(ds.Tables[0].Rows[0]["returnMessage"].ToString(), Program.ApplicationName, 1);
                            return;
                        }
                    }


                }
            }
            catch (Exception ex)
            {
                Tripous.Sys.ErrorBox(String.Format("{0}{1}", ex.Message, ex.StackTrace));
                table.Clear(); return;
            }
        }

        private void SbnPreview_Click(object sender, EventArgs e)
        {

            var dtc = gridControl1.DataSource as DataTable;

            if (dtc == null || dtc.Rows.Count <= 0)
            {
                return;
            }

            table = dtc.Clone();
            table.Columns.Add("OldTransID", typeof(int));
            foreach (DataRow a in dtcold.Rows)
            {
                var c = (from x in dtc.AsEnumerable()
                         where x.Field<int>("BSID") == a.Field<int>("BSID")
                         select x).Single();
                if (c.Field<int>("TransID") != (a.Field<int>("TransID")))
                {
                    var row = new[] { c["BSID"], c["Date"], c["Amount"], c["TransID"], c["PaymentRefNumber"], a["TransID"] };
                    table.Rows.Add(row);

                }
            }
            gridControl2.DataSource = table;
            gridView2.OptionsBehavior.Editable = false;

            AddCombCredit();
            //var lol= 
            gridView2.Columns["Amount"].DisplayFormat.FormatType = FormatType.Numeric;
            gridView2.Columns["Amount"].DisplayFormat.FormatString = "n2";
            gridView2.Columns["Date"].DisplayFormat.FormatType = FormatType.DateTime;
            gridView2.Columns["Date"].DisplayFormat.FormatString = "dd/MM/yyyy";



            colView = gridView2.Columns["TransID"];
            colView.ColumnEdit = repComboLookBoxCredit;
            gridView2.Columns["OldTransID"].ColumnEdit = repComboLookBoxCredit;
            gridView2.Columns["OldTransID"].Caption = "From";
            gridView2.Columns["OldTransID"].VisibleIndex = 3;

            colView.Visible = true;
            colView.VisibleIndex = 4;
            //colView.Caption = "To";

            gridView2.Columns["Amount"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            gridView2.Columns["Amount"].SummaryItem.FieldName = "Amount";
            gridView2.Columns["Amount"].SummaryItem.DisplayFormat = "Total = {0:n}";
            gridView2.Columns["Amount"].OptionsColumn.AllowEdit = false;
            gridView2.Columns["Date"].OptionsColumn.AllowEdit = false;
            gridView2.Columns["PaymentRefNumber"].OptionsColumn.AllowEdit = false;
            gridView2.Columns["TransID"].OptionsColumn.AllowEdit = false;
            gridView2.Columns["BSID"].Visible = false;
            gridView2.Columns["TransID"].Caption = "To";
            gridView2.OptionsView.ColumnAutoWidth = false;
            gridView2.OptionsView.ShowFooter = true;
            gridView2.BestFitColumns();
        }

        private void SbnReset_Click(object sender, EventArgs e)
        {
            setReloadCD();
        }

        private void CboYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (cboYear.SelectedValue != null && !Isbank)
            //{
            //    setDBComboBoxMonth();

            //}
        }

        public void setDBComboBoxMonth()
        {
            //using (var ds = new System.Data.DataSet())
            //{
            //    string qry = string.Format("SELECT Months,Periods FROM Reconciliation.tblFinancialperiod WHERE Year='{0}' AND FinancialperiodID NOT IN (SELECT FinancialperiodID FROM Reconciliation.tblCloseFinanicalPeriod)", cboYear.SelectedValue);

            //    using (SqlDataAdapter ada = new SqlDataAdapter(qry, Logic.ConnectionString))
            //    {
            //        ada.Fill(ds, "table");
            //    }

            //    Common.setComboList(cboMonth, ds.Tables[0], "Periods", "Months");

            //}
            //cboMonth.SelectedIndex = -1;
        }

        private void SbnSearch_Click(object sender, EventArgs e)
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
            //else if (string.IsNullOrEmpty((string)(cboYear.SelectedValue.ToString())))
            //{
            //    Common.setEmptyField("Finanical Year", Program.ApplicationName);
            //    cboYear.Focus(); return;
            //}
            //else if (string.IsNullOrEmpty((string)(cboMonth.SelectedValue.ToString())))
            //{
            //    Common.setEmptyField("Finanical Month", Program.ApplicationName);
            //    cboMonth.Focus(); return;
            //}
            else
            {
                //setGetDate(cboMonth.SelectedValue.ToString(), cboYear.SelectedValue.ToString());
                setReloadCD();
            }
        }

        private void CboBank_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboBank.SelectedValue != null && !Isbank)
            {
                setDBComboBoxAcct();
            }
        }

        void openForm()
        {

            using (FrmFinanicalYear fyear = new FrmFinanicalYear("Reclassified"))
            {
                fyear.ShowDialog();
            }
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

        private void setReloadCD()
        {

            gridControl1.DataSource = null;
            gridControl2.DataSource = null;

            DataRowView oDataRowView = cboRecPeriod.SelectedItem as DataRowView;

            if (oDataRowView != null)
            {
                //sValue = oDataRowView.Row["BankShortCode"] as string;

                //sValue1 = oDataRowView.Row["BatchCode"] as string;

                //sValue2 = oDataRowView.Row["BankName"] as string;

                //sValue4 = oDataRowView.Row["BankAccountID"] as string;

                using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
                {
                    connect.Open();
                    _command = new SqlCommand("doReclassified", connect) { CommandType = CommandType.StoredProcedure };
                    _command.Parameters.Add(new SqlParameter("@BankCode", SqlDbType.VarChar)).Value = oDataRowView.Row["BankShortCode"] as string; ;
                    _command.Parameters.Add(new SqlParameter("@AccountId", SqlDbType.Int)).Value = Convert.ToInt32(oDataRowView.Row["BankAccountID"]);
                    _command.Parameters.Add(new SqlParameter("@Periods", SqlDbType.Int)).Value = cboRecPeriod.SelectedValue;
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

                            dtcold = new DataTable();
                            dtcold.Clear();
                            dtcold = ds.Tables[1];
                            //dtcold.Columns.Add("OldTransIDk", typeof(int));
                            //dtc.Columns.Add("Description", typeof(String));


                        }

                    }
                }

                if (dtcold != null && dtcold.Rows.Count > 0)
                {
                    gridControl1.DataSource = dtcold.Copy();

                    sbnPreview.Enabled = true; sbnUpdate.Enabled = true;

                    AddCombCredit();

                    gridView1.Columns["Amount"].DisplayFormat.FormatType = FormatType.Numeric;
                    gridView1.Columns["Amount"].DisplayFormat.FormatString = "n2";
                    gridView1.Columns["Date"].DisplayFormat.FormatType = FormatType.DateTime;
                    gridView1.Columns["Date"].DisplayFormat.FormatString = "dd/MM/yyyy";



                    colView2 = gridView1.Columns["TransID"];
                    colView2.ColumnEdit = repComboLookBoxCredit;

                    //gridView1.Columns["OldTransID"].ColumnEdit = repComboLookBoxCredit;
                    //gridView1.Columns["OldTransID"].Caption = "To";
                    //gridView1.Columns["OldTransID"].VisibleIndex = 4;


                    colView2.Visible = true;

                    gridView1.Columns["Amount"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                    gridView1.Columns["Amount"].SummaryItem.FieldName = "Amount";
                    gridView1.Columns["Amount"].SummaryItem.DisplayFormat = "Total = {0:n}";
                    gridView1.Columns["Amount"].OptionsColumn.AllowEdit = false;
                    gridView1.Columns["Date"].OptionsColumn.AllowEdit = false;
                    gridView1.Columns["PaymentRefNumber"].OptionsColumn.AllowEdit = false;
                    gridView1.Columns["TransID"].OptionsColumn.AllowEdit = true;
                    //gridView1.Columns["OldTransID"].OptionsColumn.AllowEdit = true;
                    gridView1.Columns["BSID"].Visible = false;
                    gridView1.Columns["TransID"].Caption = "From";
                    colView.VisibleIndex = 3;
                    gridView1.OptionsView.ColumnAutoWidth = false;
                    gridView1.OptionsView.ShowFooter = true;
                    gridView1.BestFitColumns();
                }
                else
                {

                    gridControl1.DataSource = null;
                    gridControl2.DataSource = null;
                    Common.setMessageBox("No Allocation record found for selected Reconciliation Period", "Reclassification", 3); return;
                }

            }


        }

        public void setDBComboBoxyr()
        {
            //using (var ds = new System.Data.DataSet())
            //{
            //    using (SqlDataAdapter ada = new SqlDataAdapter((string)"SELECT DISTINCT Year FROM Reconciliation.tblFinancialperiod", Logic.ConnectionString))
            //    {
            //        ada.Fill(ds, "table");
            //    }

            //    Common.setComboList(cboYear, ds.Tables[0], "Year", "Year");

            //}
            //cboYear.SelectedIndex = -1;
        }

        void setGetDate(string months, string years)
        {

            DataTable dtsed = new DataTable();

            dtsed.Clear();

            using (var ds = new System.Data.DataSet())
            {
                ds.Clear();
                using (SqlDataAdapter ada = new SqlDataAdapter(string.Format("SELECT FinancialperiodID,StartDate,EndDate FROM Reconciliation.tblFinancialperiod WHERE Periods='{0}' AND Year='{1}'", months, years), Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }

                dtsed = ds.Tables[0];

                string[] formats = { "dd/MM/yyyy" };

                if (dtsed != null && dtsed.Rows.Count > 0)
                {
                    periodid = Convert.ToInt32(dtsed.Rows[0]["FinancialperiodID"].ToString());

                }

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

                    using (SqlDataAdapter adas = new SqlDataAdapter("SELECT Description,TransID  FROM Reconciliation.tblTransDefinition WHERE IsActive=1", Logic.ConnectionString))
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

        public DataTable CompareDataTables(DataTable first, DataTable second)
        {
            first.TableName = "FirstTable";
            second.TableName = "SecondTable";

            //Create Empty Table
            DataTable table = new DataTable("Difference");

            try
            {
                //Must use a Dataset to make use of a DataRelation object
                using (DataSet ds = new DataSet())
                {
                    //Add tables
                    ds.Tables.AddRange(new DataTable[] { first.Copy(), second.Copy() });

                    //Get Columns for DataRelation
                    DataColumn[] firstcolumns = new DataColumn[ds.Tables[0].Columns.Count];

                    for (int i = 0; i < firstcolumns.Length; i++)
                    {
                        firstcolumns[i] = ds.Tables[0].Columns[i];
                    }

                    DataColumn[] secondcolumns = new DataColumn[ds.Tables[1].Columns.Count];

                    for (int i = 0; i < secondcolumns.Length; i++)
                    {
                        secondcolumns[i] = ds.Tables[1].Columns[i];
                    }

                    //Create DataRelation
                    DataRelation r = new DataRelation(string.Empty, firstcolumns, secondcolumns, false);

                    ds.Relations.Add(r);

                    //Create columns for return table
                    for (int i = 0; i < first.Columns.Count; i++)
                    {
                        table.Columns.Add(first.Columns[i].ColumnName, first.Columns[i].DataType);
                    }

                    //If First Row not in Second, Add to return table.
                    table.BeginLoadData();

                    foreach (DataRow parentrow in ds.Tables[0].Rows)
                    {
                        DataRow[] childrows = parentrow.GetChildRows(r);
                        if (childrows == null || childrows.Length == 0)
                            table.LoadDataRow(parentrow.ItemArray, true);
                    }

                    table.EndLoadData();

                }
            }
            catch (Exception ex)
            {

            }
            return table;
        }

        private void panelContainer_Paint(object sender, PaintEventArgs e)
        {

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
        INNER JOIN Reconciliation.tblFinancialperiod ON tblFinancialperiod.FinancialperiodID = tblReconciliatioPeriod.FinancialperiodID WHERE (IsPeriodClosed=1) ORDER BY Description,Period
", Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }

                Dts = ds.Tables[0];
            }

            Common.setComboList(cboRecPeriod, Dts, "PeriodID", "Description");

            cboRecPeriod.SelectedIndex = -1;


        }
    }

}

