using Collection.Classess;
using Collections;
using DevExpress.XtraSplashScreen;
using TaxSmartSuite.Class;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraGrid.Views.Layout;
using DevExpress.XtraGrid.Selection;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraEditors.Repository;

namespace Collection.Forms
{
    public partial class FrmCriteria : Form
    {
        public static FrmCriteria publicInstance;

        string values, values1, values2, values3;

        protected TransactionTypeCode iTransType; private SqlCommand _command;

        protected bool boolIsUpdate;

        GridCheckMarksSelection selection;

        GridCheckMarksSelection selectiongrid;

        protected int ID; bool isBankGrid = true; SqlDataAdapter adp;

        bool isRevenueGrid = true;

        DataTable temTable = new DataTable();

        DataTable temTableAgcy = new DataTable();
        public FrmCriteria()
        {
            SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

            InitializeComponent();

            iTransType = TransactionTypeCode.Null;

            publicInstance = this;

            setImages();

            ToolStripEvent();

            temTable.Columns.Add("BankCode", typeof(string));

            temTable.Columns.Add("BranchCode", typeof(string));

            temTableAgcy.Columns.Add("AgencyCode", typeof(string));

            temTableAgcy.Columns.Add("RevenueCode", typeof(string));

            cboAgencyTest.EditValueChanged += cboAgencyTest_EditValueChanged;

            cboBankEdt.EditValueChanged += cboBankEdt_EditValueChanged;

            gridView1.DoubleClick += GridView1_DoubleClick;

            bttnUpdate.Click += BttnUpdate_Click;

            Load += OnFormLoad;

            OnFormLoad(null, null);

            SplashScreenManager.CloseForm(false);
        }

        private void GridView1_DoubleClick(object sender, EventArgs e)
        {
            tsbEdit.PerformClick();
        }

        private void setImages()
        {
            tsbNew.Image = MDIMain.publicMDIParent.i16x16.Images[5];
            tsbEdit.Image = MDIMain.publicMDIParent.i16x16.Images[6];
            tsbDelete.Image = MDIMain.publicMDIParent.i16x16.Images[7];
            tsbReload.Image = MDIMain.publicMDIParent.i16x16.Images[0];
            tsbClose.Image = MDIMain.publicMDIParent.i16x16.Images[11];

            ////bttnCancel.Image = MDIMain.publicMDIParent.i32x32.Images[9];
            //bttnMain.Image = MDIMain.publicMDIParent.i32x32.Images[8];
            //bttnprints.Image = MDIMain.publicMDIParent.i32x32.Images[29];
            //btnSearch.Image = MDIMain.publicMDIParent.i32x32.Images[2];
            //btnClear.Image = MDIMain.publicMDIParent.i32x32.Images[3];

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
                FrmReceipts.publicInstance.setDBComboProfile();
                this.Close();
            }
            else if (sender == tsbNew)
            {
                iTransType = TransactionTypeCode.New;
                setClear();
                ShowForm();
                boolIsUpdate = false;
            }
            else if (sender == tsbEdit)
            {
                iTransType = TransactionTypeCode.Edit;
                if (EditRecordMode())
                {
                    ShowForm();
                    boolIsUpdate = true;
                }
            }
            else if (sender == tsbDelete)
            {

                tsbReload.PerformClick();
                boolIsUpdate = false;
            }
            else if (sender == tsbReload)
            {
                iTransType = TransactionTypeCode.Reload; setReload();
                boolIsUpdate = false;
                ShowForm();
            }

        }

        void OnFormLoad(object sender, EventArgs e)
        {

            setReload(); ShowForm();

            setDBComboBoxAgency();

            setDBComboBoxBank();


        }

        private void BttnUpdate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtName.Text.Trim().ToString()) || txtName.Text == null)
            {
                Common.setEmptyField("Profile Name", "Profile Setup");
                return;
            }
            else
            {
                temTableAgcy.Clear(); temTable.Clear();

                if (selection.SelectedCount == 0)
                {
                    Common.setMessageBox("Bank Branch", "Profile Setup", 2); return;
                }
                else
                {
                    for (int i = 0; i < selection.SelectedCount; i++)
                    {
                        temTable.Rows.Add(new object[] { String.Format("{0}", (selection.GetSelectedRow(i) as DataRowView)["BankShortCode"]), String.Format("{0}", (selection.GetSelectedRow(i) as DataRowView)["BranchCode"]) });
                    }

                }

                if (selectiongrid.SelectedCount == 0)
                {
                    Common.setMessageBox("Revenue Description", "Profile Setup", 2); return;
                }
                else
                {
                    for (int i = 0; i < selectiongrid.SelectedCount; i++)
                    {
                        temTableAgcy.Rows.Add(new object[] { String.Format("{0}", (selectiongrid.GetSelectedRow(i) as DataRowView)["AgencyCode"]), String.Format("{0}", (selectiongrid.GetSelectedRow(i) as DataRowView)["RevenueCode"]) });
                    }
                }

                using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
                {
                    connect.Open();
                    _command = new SqlCommand("doProfileCriteria", connect) { CommandType = CommandType.StoredProcedure };
                    _command.Parameters.Add(new SqlParameter("@profilename", SqlDbType.VarChar)).Value = txtName.Text.Trim();
                    _command.Parameters.Add(new SqlParameter("@pAgencyTable", SqlDbType.Structured)).Value = temTableAgcy;
                    _command.Parameters.Add(new SqlParameter("@pBankTable", SqlDbType.Structured)).Value = temTable;
                    _command.Parameters.Add(new SqlParameter("@type", SqlDbType.Bit)).Value = boolIsUpdate;
                    _command.Parameters.Add(new SqlParameter("@userid", SqlDbType.VarChar)).Value = Program.UserID;
                    _command.Parameters.Add(new SqlParameter("@ids", SqlDbType.Int)).Value = ID;
                    _command.CommandTimeout = 0;
                    System.Data.DataSet response = new System.Data.DataSet();
                    response.Clear();
                    adp = new SqlDataAdapter(_command);
                    adp.Fill(response);

                    connect.Close();

                    if (String.Compare(response.Tables[0].Rows[0]["returnCode"].ToString(), "00", false) == 0)
                    {
                        Common.setMessageBox(response.Tables[0].Rows[0]["returnMessage"].ToString(), "", 2);

                        setClear(); setReload(); tsbReload.PerformClick();

                        return;
                    }
                    else
                    {
                        Common.setMessageBox(response.Tables[0].Rows[0]["returnMessage"].ToString(), "", 2);

                        setClear(); setReload(); tsbReload.PerformClick();

                        return;
                    }

                }


            }
        }

        public void setDBComboBoxBank()
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

                DataTable Dt;

                using (var ds = new System.Data.DataSet())
                {
                    using (SqlDataAdapter ada = new SqlDataAdapter("SELECT BankShortCode,BankName FROM Collection.tblBank WHERE BankShortCode IN (SELECT BankShortCode FROM Collection.tblBankBranch) AND LEN(BankShortCode)<=7 ORDER BY BankName", Logic.ConnectionString))
                    {
                        ada.SelectCommand.CommandTimeout = 0;

                        ada.Fill(ds, "table");
                    }

                    Dt = ds.Tables[0];
                }

                Common.setCheckEdit(cboBankEdt, Dt, "BankShortCode", "BankName");

                //cboBankEdt.EditValue = -1;
            }
            finally
            {
                SplashScreenManager.CloseForm(false);
            }

        }

        public void setDBComboBoxAgency()
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

                DataTable Dt;

                using (var ds = new System.Data.DataSet())
                {
                    using (SqlDataAdapter ada = new SqlDataAdapter((string)"SELECT AgencyCode,AgencyName FROM Registration.tblAgency ORDER BY AgencyName", Logic.ConnectionString))
                    {
                        ada.SelectCommand.CommandTimeout = 0;

                        ada.Fill(ds, "table");
                    }

                    Dt = ds.Tables[0];
                }

                Common.setCheckEdit(cboAgencyTest, Dt, "AgencyCode", "AgencyName");


            }
            finally
            {
                SplashScreenManager.CloseForm(false);
            }

        }

        private void setReload()
        {

            try
            {
                DataTable dt;

                using (var ds = new System.Data.DataSet())
                {

                    using (SqlDataAdapter ada = new SqlDataAdapter("SELECT Name,ProfileID FROM Receipt.tblProfile ", Logic.ConnectionString))
                    {
                        ada.Fill(ds, "Profile");
                    }
                    using (SqlDataAdapter ada1 = new SqlDataAdapter("SELECT ProfileID,AgencyName,BranchName,Description,Bank,Revenue,Agency,BankBranch FROM dbo.ViewProflieCriteria", Logic.ConnectionString))
                    {
                        ada1.Fill(ds, "Criteria");
                    }

                    DataColumn keyColumn = ds.Tables["Profile"].Columns["ProfileID"];
                    DataColumn foreignKeyColumn = ds.Tables["Criteria"].Columns["ProfileID"];
                    ds.Relations.Add("Profile1", keyColumn, foreignKeyColumn);

                    dt = ds.Tables[0];

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        //gridControl3.DataSource = ds.Tables["office"];
                        //gridControl3.ForceInitialize();

                        gridControl1.DataSource = ds.Tables["Profile"];
                        gridControl1.ForceInitialize();

                        LayoutView lView = new LayoutView(gridControl1);
                        gridControl1.LevelTree.Nodes.Add("Name", lView);
                        lView.ViewCaption = "Proflie Criteria";

                        gridView1.OptionsBehavior.Editable = false;
                        gridView1.Columns["ProfileID"].Visible = false;
                        //gridView1.Columns["ProfileID"].Visible = false;
                        gridView1.BestFitColumns();
                    }



                }
            }
            catch (Exception ex)
            {
                Tripous.Sys.ErrorBox(ex.Message.ToString()); return;
            }

        }

        void cboBankEdt_EditValueChanged(object sender, EventArgs e)
        {
            if (cboBankEdt.EditValue != null && !string.IsNullOrEmpty(cboBankEdt.EditValue.ToString()))
            {
                checkBankgridview();

                setDBComboBoxBranch();

                loadBankGridview();
            }
        }

        public void setDBComboBoxBranch()
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

                DataTable Dt;

                string values = string.Empty;

                object[] lol = cboBankEdt.EditValue.ToString().Split(',');

                int i = 0;

                foreach (object obj in lol)
                {
                    values += string.Format("'{0}'", obj.ToString().Trim());

                    if (i + 1 < lol.Count())
                        values += ",";
                    ++i;
                }

                using (var ds = new System.Data.DataSet())
                {
                    string query = string.Format("SELECT BranchCode, BankName +','+BranchName AS BranchName ,Collection.tblBank.BankShortCode FROM Collection.tblBankBranch INNER JOIN Collection.tblBank ON Collection.tblBank.BankShortCode = Collection.tblBankBranch.BankShortCode WHERE Collection.tblBank.BankShortCode in ({0}) ORDER BY BranchName", values);

                    using (SqlDataAdapter ada = new SqlDataAdapter(query, Logic.ConnectionString))
                    {
                        ada.SelectCommand.CommandTimeout = 0;

                        ada.Fill(ds, "table");
                    }

                    Dt = ds.Tables[0];

                }
                if (Dt != null && Dt.Rows.Count > 0)
                {
                    gridControl2.DataSource = Dt.DefaultView;
                    gridView3.OptionsBehavior.Editable = false;
                    gridView3.Columns["BranchCode"].Visible = false;
                    gridView3.Columns["BankShortCode"].Visible = false;

                    gridView3.BestFitColumns();

                }


                if (isBankGrid)
                {
                    selection = new GridCheckMarksSelection(gridView3, ref label7);
                    selection.CheckMarkColumn.VisibleIndex = 0;
                    isBankGrid = false;
                }



            }
            catch (Exception ex)
            {
                Tripous.Sys.ErrorBox(ex.Message.ToString()); return;
            }

            finally
            {
                SplashScreenManager.CloseForm(false);
            }

        }

        void cboAgencyTest_EditValueChanged(object sender, EventArgs e)
        {
            if (cboAgencyTest.EditValue != null && !string.IsNullOrEmpty(cboAgencyTest.EditValue.ToString()))
            {
                checkAgencygridview();

                setDBComboBoxReveneu();

                loadAgencyGridView();
            }
        }

        void setDBComboBoxReveneu()
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

                DataTable DtRev;

                string values = string.Empty;

                object[] lol = cboAgencyTest.EditValue.ToString().Split(',');

                int i = 0;

                foreach (object obj in lol)
                {
                    values += string.Format("'{0}'", obj.ToString().Trim());

                    if (i + 1 < lol.Count())
                        values += ",";
                    ++i;
                }

                using (var ds = new System.Data.DataSet())
                {
                    string query = string.Format("SELECT AgencyCode,RevenueCode,Description FROM Collection.tblRevenueType WHERE AgencyCode IN ({0})  ORDER BY Description", values);

                    using (SqlDataAdapter ada = new SqlDataAdapter(query, Logic.ConnectionString))
                    {
                        ada.SelectCommand.CommandTimeout = 0;

                        ada.Fill(ds, "table");
                    }

                    DtRev = ds.Tables[0];
                }

                if (DtRev != null && DtRev.Rows.Count > 0)
                {
                    gridControl3.DataSource = DtRev;
                    gridView5.OptionsBehavior.Editable = false;
                    gridView5.Columns["RevenueCode"].Visible = false;
                    gridView5.Columns["AgencyCode"].Visible = false;

                    gridView5.BestFitColumns();
                }

                if (isRevenueGrid)
                {
                    selectiongrid = new GridCheckMarksSelection(gridView5, ref label6);
                    selectiongrid.CheckMarkColumn.VisibleIndex = 0;
                    isRevenueGrid = false;
                }


            }
            catch (Exception ex)
            {
                Tripous.Sys.ErrorBox(ex.Message.ToString()); return;
            }

            finally
            {
                SplashScreenManager.CloseForm(false);
            }
        }

        void setClear()
        {
            txtName.Text = String.Empty;
            cboBankEdt.EditValue = null; //cboBankEdt.RefreshEditValue();
            cboAgencyTest.EditValue = null; //cboAgencyTest.RefreshEditValue();

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
                    ID = Convert.ToInt32(dr["ProfileID"]);
                    bResponse = FillField(Convert.ToInt32(dr["ProfileID"]));
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
            try
            {
                bool bResponse = false;
                //load data from the table into the forms for edit
                DataTable dtBanks = new DataTable(); DataTable dtBranch = new DataTable(); DataTable dtAgency = new DataTable(); DataTable dtRevenue = new DataTable();

                DataTable dts = (new Logic()).getSqlStatement((String.Format("SELECT Name FROM Receipt.tblProfile WHERE ProfileID  ='{0}'", fieldid))).Tables[0];

                dtBanks = (new Logic()).getSqlStatement((String.Format("SELECT DISTINCT Bank as BankShortCode  FROM Receipt.tblProfileCriteria WHERE ProfileID  ='{0}'", fieldid))).Tables[0];

                dtBranch = (new Logic()).getSqlStatement((String.Format("SELECT DISTINCT Bank as BankShortCode ,BankBranch as BranchCode FROM Receipt.tblProfileCriteria WHERE BankBranch IS NOT NULL AND  ProfileID  ='{0}'", fieldid))).Tables[0];

                dtAgency = (new Logic()).getSqlStatement((String.Format("SELECT DISTINCT Agency as AgencyCode FROM Receipt.tblProfileCriteria WHERE ProfileID  ='{0}'", fieldid))).Tables[0];

                dtRevenue = (new Logic()).getSqlStatement((String.Format("SELECT DISTINCT Revenue as RevenueCode,AgencyCode as AgencyCode FROM Receipt.tblProfileCriteria JOIN Collection.tblRevenueType ON Revenue=RevenueCode WHERE Revenue IS NOT NULL AND ProfileID  ='{0}'", fieldid))).Tables[0];

                int k = 0; int l = 0;

                if (dts != null)
                {
                    bResponse = true;

                    txtName.Text = dts.Rows[0]["Name"].ToString();

                    txtName.Enabled = false;

                    if (dtBanks != null && dtBanks.Rows.Count >= 1)
                    {
                        values = string.Empty;

                        if (dtBanks.Rows[0]["BankShortCode"] != DBNull.Value)
                        {
                            foreach (DataRow item in dtBanks.Rows)
                            {
                                values += string.Format("{0}", item["BankShortCode"].ToString().Trim());

                                if (l + 1 < dtBanks.Rows.Count)
                                    values += ",";
                                ++l;
                            }

                            cboBankEdt.SetEditValue(values.ToString());

                            //check bank branch
                            if (dtBranch != null && dtBranch.Rows.Count >= 1)
                            {
                                if (dtBranch.Rows[0]["BranchCode"] != DBNull.Value)
                                {
                                    for (int i = 0; i < gridView3.RowCount; i++)
                                    {
                                        object objbankCode = gridView5.GetRowCellValue(i, "BankShortCode");

                                        object objBranchCode = gridView5.GetRowCellValue(i, "BranchCode");

                                        for (int z = 0; z < dtBranch.Rows.Count; z++)
                                        {
                                            string Bankcode = Convert.ToString(objbankCode);
                                            string Branchcode = Convert.ToString(objBranchCode);
                                            string branchcode1 = Convert.ToString(dtBranch.Rows[z][1]);
                                            string BankCode1 = Convert.ToString(dtBranch.Rows[z][0]);

                                            if ((Bankcode.Trim() == BankCode1.Trim()) && (Branchcode.Trim() == branchcode1.Trim()))
                                            {
                                                selection.SelectRow(z, true);
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    for (int i = 0; i < gridView3.RowCount; i++)
                                    {
                                        selection.SelectRow(i, true);
                                    }
                                }
                            }
                            else
                            {
                                for (int i = 0; i < gridView3.RowCount; i++)
                                {
                                    selection.SelectRow(i, true);
                                }
                            }
                        }



                        //for (int i = 0; i < gridView4.RowCount; i++)
                        //{
                        //}
                        //if (ds.Tables[0].Rows[0]["ControlNumber"] == DBNulesl.Value)
                    }

                    if (dtAgency != null && dtAgency.Rows.Count >= 1)
                    {
                        if (dtAgency.Rows[0]["AgencyCode"] != DBNull.Value)
                        {
                            values = string.Empty;

                            foreach (DataRow item in dtAgency.Rows)
                            {
                                values += string.Format("{0}", item["AgencyCode"].ToString().Trim());

                                if (k + 1 < dtAgency.Rows.Count)
                                    values += ",";
                                ++k;
                            }

                            cboAgencyTest.SetEditValue(values);

                            for (int i = 0; i < gridView5.RowCount; i++)
                            {
                                selectiongrid.SelectRow(i, true);
                            }
                        }
                        else
                        {
                            if (dtRevenue != null && dtRevenue.Rows.Count >= 1)
                            {
                                if (dtRevenue.Rows[0]["AgencyCode"] != DBNull.Value)
                                {
                                    DataTable dtg = dtRevenue.DefaultView.ToTable(true, "AgencyCode");

                                    cboAgencyTest.SetEditValue(dtg.Rows[0]["AgencyCode"].ToString());
                                }

                                for (int i = 0; i < gridView5.RowCount; i++)
                                {
                                    object objAgeCode = gridView5.GetRowCellValue(i, "AgencyCode");

                                    object objRevCode = gridView5.GetRowCellValue(i, "RevenueCode");

                                    for (int j = 0; j < dtRevenue.Rows.Count; j++)
                                    {
                                        string revCode = Convert.ToString(objRevCode);
                                        string Agcode = Convert.ToString(objAgeCode);
                                        string Agecode1 = Convert.ToString(dtRevenue.Rows[j][1]);
                                        string revCode1 = Convert.ToString(dtRevenue.Rows[j][0]);

                                        if ((Agcode.Trim() == Agecode1.Trim()) && (revCode.Trim() == revCode1.Trim()))
                                        {
                                            //selection.SelectRow(i, true);
                                            selectiongrid.SelectRow(j, true);
                                        }

                                    }
                                }
                            }
                        }
                    }
                    //if (!string.IsNullOrEmpty(dts.Rows[0]["Agency"].ToString()))
                    //{
                    //    //cboAgencyTest.SetEditValue(dts.Rows[0]["Agency"].ToString());
                    //    removeCombinedFlags(cboAgencyTest.Properties);
                    //}

                    //if (!string.IsNullOrEmpty(dts.Rows[0]["Revenue"].ToString()))
                    //{
                    //    cboRevenueEdt.SetEditValue(dts.Rows[0]["Revenue"].ToString());
                    //}

                    //if (!string.IsNullOrEmpty(dts.Rows[0]["Bank"].ToString()))
                    //{
                    //    cboBankEdt.SetEditValue(dts.Rows[0]["Bank"].ToString());
                    //}

                    //if (!string.IsNullOrEmpty(dts.Rows[0]["BankBranch"].ToString()))
                    //{
                    //    cboBranchesEdt.SetEditValue(dts.Rows[0]["BankBranch"].ToString());
                    //}
                }
                else
                    bResponse = false;

                return bResponse;
            }
            catch (Exception ex)
            {

                Tripous.Sys.ErrorBox(ex.Message.ToString());

                return false;
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

        private void removeCombinedFlags(RepositoryItemCheckedComboBoxEdit ri)
        {
            for (int i = ri.Items.Count - 1; i > 0; i--)
            {
                Enum val1 = ri.Items[i].Value as Enum;
                for (int j = i - 1; j >= 0; j--)
                {
                    Enum val2 = ri.Items[j].Value as Enum;
                    if (val1.HasFlag(val2))
                    {
                        ri.Items.RemoveAt(i);
                        break;
                    }
                }
            }
        }

        void checkAgencygridview()
        {
            if (gridView5.RowCount == 0) return;

            temTableAgcy.Clear();

            if (gridView5.RowCount != 0)
            {
                if (selectiongrid.SelectedCount != 0)
                {
                    for (int i = 0; i < selectiongrid.SelectedCount; i++)
                    {
                        temTableAgcy.Rows.Add(new object[] { String.Format("{0}", (selectiongrid.GetSelectedRow(i) as DataRowView)["AgencyCode"]), String.Format("{0}", (selectiongrid.GetSelectedRow(i) as DataRowView)["RevenueCode"]) });
                    }
                }
            }
        }

        void checkBankgridview()
        {
            if (gridView3.RowCount == 0) return;

            temTable.Clear();

            if ((gridView3.RowCount != 0))
            {
                if (selection.SelectedCount != 0)
                {
                    for (int i = 0; i < selection.SelectedCount; i++)
                    {
                        temTable.Rows.Add(new object[] { String.Format("{0}", (selection.GetSelectedRow(i) as DataRowView)["BankShortCode"]), String.Format("{0}", (selection.GetSelectedRow(i) as DataRowView)["BranchCode"]) });
                    }

                }
            }

        }

        void loadBankGridview()
        {
            if (temTable == null && temTable.Rows.Count >= 1) return;

            if (temTable != null && temTable.Rows.Count >= 1)
            {
                for (int i = 0; i < temTable.Rows.Count; i++)
                {
                    object obj = temTable.Rows[i][0];

                    Object obj2 = temTable.Rows[i][1];

                    for (int j = 0; j < gridView3.RowCount; j++)
                    {
                        object bank = gridView3.GetRowCellValue(j, "BankShortCode");

                        object branch = gridView3.GetRowCellValue(j, "BranchCode");
                        //string Agecode1 = Convert.ToString(dtRevenue.Rows[j][1]);
                        if ((Convert.ToString(obj) == Convert.ToString(bank)) && (Convert.ToString(obj2) == Convert.ToString(branch)))
                        {
                            selection.SelectRow(j, true);
                        }
                    }
                }
            }
        }

        void loadAgencyGridView()
        {
            if (temTableAgcy == null && temTableAgcy.Rows.Count >= 1) return;

            if (temTableAgcy != null && temTableAgcy.Rows.Count >= 1)
            {
                for (int i = 0; i < temTableAgcy.Rows.Count; i++)
                {
                    object obj = temTableAgcy.Rows[i][0];

                    object obj2 = temTableAgcy.Rows[i][1];

                    for (int j = 0; j < gridView5.RowCount; j++)
                    {
                        object Agency = gridView5.GetRowCellValue(j, "AgencyCode");

                        object Revenue = gridView5.GetRowCellValue(j, "RevenueCode");

                        if ((Convert.ToString(obj) == Convert.ToString(Agency)) && Convert.ToString(obj2) == Convert.ToString(Revenue))
                        {
                            selectiongrid.SelectRow(j, true);
                        }

                    }
                }
            }
        }
    }
}
