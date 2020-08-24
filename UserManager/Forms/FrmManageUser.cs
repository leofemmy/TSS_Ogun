using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.Utils;
using TaxSmartSuite;
using UserManager.Classess;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Base;
using MosesClassLibrary.Security;
using DevExpress.XtraGrid.Views.Grid;
using TaxSmartSuite.Class;

namespace UserManager.Forms
{
    public partial class FrmManageUser : Form
    {
        //FORM VARIABLES
        public static FrmManageUser publicUser;

        protected TransactionTypeCode iTransType;
        protected bool boolIsUpdate;
        protected object ID;
        protected bool IsFirst = true;

        RepositoryItemCheckedComboBoxEdit repositoryRight = new RepositoryItemCheckedComboBoxEdit();

        public FrmManageUser()
        {
            using (WaitDialogForm dlg = new WaitDialogForm("Please Wait", "Initialising Form Data"))
            {
                InitializeComponent();
                publicUser = this;
                Load += OnFormLoad;

                setImages();
                ToolStripEvent();
                iTransType = TransactionTypeCode.New;

                OnFormLoad(null, null);
                //cbApplication_SelectedIndexChanged(null, null);
            }
        }

        void OnFormLoad(object sender, EventArgs e)
        {
            setDBComboBox();
            OnToolStripItemsClicked(tsbReload, null);
            //ShowForm();

            cbApplication.SelectedIndexChanged += cbApplication_SelectedIndexChanged;
            checkedListBoxControl1.DrawItem += checkedListBoxControl1_DrawItem;
            checkedListBoxControl1.ItemCheck += checkedListBoxControl1_ItemCheck;
            gridView1.CellValueChanged += gridView1_CellValueChanged;
            gridView1.DoubleClick += gridView1_DoubleClick;
            gridControl1.RepositoryItems.Add(repositoryRight);
            gridControl1.DataSourceChanged += OnDataSourceChanged;
            //checkedListBoxControl1.CheckOnClick = true;

            bttnCancel.Click += Bttn_Click;
            bttnReset.Click += Bttn_Click;
            bttnUpdate.Click += Bttn_Click;
        }

        private void setImages()
        {
            tsbNew.Image = MDIMainForm.publicMDIParent.i16x16.Images[5];
            tsbEdit.Image = MDIMainForm.publicMDIParent.i16x16.Images[6];
            tsbDelete.Image = MDIMainForm.publicMDIParent.i16x16.Images[7];
            tsbReload.Image = MDIMainForm.publicMDIParent.i16x16.Images[0];
            tsbClose.Image = MDIMainForm.publicMDIParent.i16x16.Images[11];

            bttnCancel.Image = MDIMainForm.publicMDIParent.i32x32.Images[9];
            bttnReset.Image = MDIMainForm.publicMDIParent.i32x32.Images[8];
            bttnUpdate.Image = MDIMainForm.publicMDIParent.i32x32.Images[7];

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
                MDIMainForm.publicMDIParent.RemoveControls();
            }
            else if (sender == tsbNew)
            {
                groupControl1.Text = "Add New Record";
                cbApplication_SelectedIndexChanged(null, null);
                iTransType = TransactionTypeCode.New;
                ShowForm();
                boolIsUpdate = false;
                txtUsername.Enabled = true;
                cbApplication.SelectedIndex = -1;
                //setDBComboBox();
            }
            else if (sender == tsbEdit)
            {
                if (EditRecordMode())
                {
                    groupControl1.Text = "Edit Record Mode";
                    iTransType = TransactionTypeCode.Edit;
                    ShowForm();
                    boolIsUpdate = true;
                    txtUsername.Enabled = false;
                    SetIndex();
                    cbApplication_SelectedIndexChanged(null, null);
                    //setDBComboBox2();
                }
            }
            else if (sender == tsbDelete)
            {
                groupControl1.Text = "Delete Record Mode";
                iTransType = TransactionTypeCode.Delete;
                if (MosesClassLibrary.Utilities.Common.AskQuestion("Deleting this record will delete attached record.\nDo you want to continue?", Program.ApplicationName))
                {
                }
                tsbReload.PerformClick();
                boolIsUpdate = false;
            }
            else if (sender == tsbReload)
            {
                groupControl1.Text = Program.ApplicationName;
                iTransType = TransactionTypeCode.Reload;
                ShowForm();
                setReload();
            }
            bttnReset.PerformClick();
        }

        private void setReload()
        {
            try
            {
                ClearGridControl(gridControl1);
                DataTable Dt = null;
                (new BusinessLogic()).LoadDataTable(ProgramLoadTableCode.AllUsers, Program.StateCode, out Dt);
                gridControl1.DataSource = Dt;
                gridControl1.ForceInitialize();
                gridView1.OptionsBehavior.Editable = false;
                gridView1.OptionsView.ColumnAutoWidth = true;
                gridView1.Columns["ID"].Visible = true;
                gridView1.Columns["ClassOfUser"].Visible = true;
                gridView1.BestFitColumns();
            }
            catch (Exception ex)
            {
                Tripous.Sys.ErrorBox(ex);
            }
        }

        private void setReload2()
        {
            try
            {
                DataTable Dt = null;
                ClearGridControl(gridControl1);

                //(new BusinessLogic()).LoadDataTable(ProgramLoadTableCode.AllUsers, Program.StateCode, out Dt);
                //gridControl1.DataSource = Dt.DefaultView;
                //gridView1.OptionsBehavior.Editable = false;
                //gridView1.OptionsView.ColumnAutoWidth = false;
                //gridView1.Columns["ID"].Visible = false;
                //gridView1.Columns["ClassOfUser"].Visible = false;
                //gridView1.BestFitColumns();
            }
            catch (Exception ex)
            {
                Tripous.Sys.ErrorBox(ex);
            }
        }

        protected void ShowForm()
        {
            switch (iTransType)
            {
                case TransactionTypeCode.Null:
                    splitContainerControl1.PanelVisibility = SplitPanelVisibility.Panel2;
                    setReload();
                    groupControl2.Text = "List of Registered Users";
                    break;
                case TransactionTypeCode.New:
                    splitContainerControl1.PanelVisibility = SplitPanelVisibility.Both;
                    setReload2();
                    groupControl2.Text = "Modules Access and Right";
                    break;
                case TransactionTypeCode.Edit:
                    splitContainerControl1.PanelVisibility = SplitPanelVisibility.Both;
                    setReload2();
                    groupControl2.Text = "Modules Access and Right";
                    break;
                case TransactionTypeCode.Delete:
                    splitContainerControl1.PanelVisibility = SplitPanelVisibility.Panel2;
                    setReload();
                    groupControl2.Text = "List of Registered Users";
                    break;
                case TransactionTypeCode.Reload:
                    splitContainerControl1.PanelVisibility = SplitPanelVisibility.Panel2;
                    setReload();
                    groupControl2.Text = "List of Registered Users";
                    break;
                default:
                    splitContainerControl1.PanelVisibility = SplitPanelVisibility.Both;
                    setReload();
                    groupControl2.Text = "List of Registered Users";
                    break;
            }
        }

        protected void setDBComboBox()
        {
            DataTable Dt;
            if ((new BusinessLogic()).LoadDataTable(ProgramLoadTableCode.ApplicationSetUp, Program.StateCode, out Dt))
                Common.setComboList(cbApplication, Dt, "ApplicationCode", "ApplicationName");
            //Dt = null;
            //if ((new BusinessLogic()).LoadDataTable(ProgramLoadTableCode.TaxOffice, Program.StateCode, out Dt))
            //    Common.setComboList(cbTaxOffice, Dt, "RevenueOfficeID", "OfficeName");
            Dt = null;
            if ((new BusinessLogic()).LoadDataTable(ProgramLoadTableCode.UserType, Program.StateCode, out Dt))
                Common.setComboList(cbUserType, Dt, "UserCode", "UserDescription");
        }

        void cbApplication_SelectedIndexChanged(object sender, EventArgs e)
        {
            checkedListBoxControl1.DataSource = null;
            ClearGridControl(gridControl1);
            if (cbApplication.DataSource == null || cbApplication.SelectedValue == null) return;
            DataTable Dt;
            if ((new BusinessLogic()).LoadDataTable(ProgramLoadTableCode.ApplicationModules, cbApplication.SelectedValue.ToString(), out Dt))
            {
                if (Dt != null & Dt.Rows.Count > 0)
                {
                    checkedListBoxControl1.DataSource = Dt;
                    if (checkedListBoxControl1.DataSource != null)
                    {
                        checkedListBoxControl1.DisplayMember = "ModulesName";
                        checkedListBoxControl1.ValueMember = "ModulesCode";
                        checkedListBoxControl1_ItemCheck(null, null);
                    }
                }
            }
        }

        void checkedListBoxControl1_DrawItem(object sender, ListBoxDrawItemEventArgs e)
        {
            if (checkedListBoxControl1.GetItemChecked(e.Index))
            {
                e.Appearance.Font = new Font(e.Appearance.Font, FontStyle.Bold);
                e.Appearance.ForeColor = Color.Green;
                return;
            }
            e.Appearance.Font = new Font(e.Appearance.Font, FontStyle.Strikeout);
        }

        private ColumnView ColumnView
        {
            get { return gridControl1.MainView as ColumnView; }
        }

        void checkedListBoxControl1_ItemCheck(object sender, DevExpress.XtraEditors.Controls.ItemCheckEventArgs e)
        {
            //MessageBox.Show(checkedListBoxControl1.GetItemValue(e.Index).ToString());
            //MessageBox.Show(GetAllCheckedItems());
            if (e != null)
                if (e.State == CheckState.Checked)
                {
                    PopulateGrid(e.Index);
                }
                else if (e.State == CheckState.Unchecked)
                {
                    RemoveDataRowsFromDataTable(e.Index);
                }
            //PopulateGrid();
        }

        string GetAllCheckedItems()
        {
            string sResponse = string.Empty;
            foreach (var item in checkedListBoxControl1.CheckedItems)
            {
                DataRowView row = item as DataRowView;
                sResponse += String.Format("{1}{0}", row["ModulesCode"], String.IsNullOrEmpty(sResponse) ? "" : ",");
            }
            //int i = 0;
            //while (checkedListBoxControl1.GetItem(i) != null)
            //{
            //    i++;
            //    //checkedListBoxControl1.SetItemCheckState(i, (true.Equals(checkedListBoxControl1.GetItemValue(i)) ? CheckState.Unchecked : CheckState.Checked));
            //    if (checkedListBoxControl1.GetItemCheckState(i) == CheckState.Checked)
            //    {
            //        checkedListBoxControl1.GetItem(i);
            //    }
            //}
            return sResponse;
        }

        void PopulateGrid()
        {
            gridControl1.DataSource = null;
            string ID = GetAllCheckedItems();
            if (!string.IsNullOrEmpty(ID))
            {
                DataTable Dt;
                (new BusinessLogic()).LoadDataTable(ProgramLoadTableCode.ApplicationAccess, ID, out Dt);
                Dt.Columns.Add(new DataColumn("AccessRight"));
                gridControl1.DataSource = Dt;
                gridView1.OptionsBehavior.Editable = true;
                gridView1.Columns["ModulesCode"].Visible = false;
                gridView1.Columns["ModuleAccessCode"].Visible = false;
                PopulateRepositoryItems(gridView1.Columns["AccessRight"]);
                gridView1.Columns["ModulesName"].OptionsColumn.AllowEdit = false;
                gridView1.Columns["ModuleAccessName"].OptionsColumn.AllowEdit = false;
                gridView1.Columns["AccessRight"].OptionsColumn.AllowEdit = true;
                gridView1.OptionsView.ColumnAutoWidth = false;
                gridView1.BestFitColumns();
            }
        }

        void PopulateGrid(int index)
        {
            object lol = checkedListBoxControl1.GetItemValue(index);
            //gridControl1.DataSource = null;
            string ID = lol as String;
            if (!string.IsNullOrEmpty(ID))
            {
                DataTable Dt;
                (new BusinessLogic()).LoadDataTable(ProgramLoadTableCode.ApplicationAccess, ID, out Dt);
                DataTable DsTable = gridControl1.DataSource as DataTable;
                AddDataRowToDataTable(ref DsTable, Dt);
                //if (DsTable.Columns["AccessRight"] == null)
                //{
                //    DsTable.Columns.Add(new DataColumn("AccessRight"));
                //}
                if (boolIsUpdate) EditApplicationRight(ref DsTable);
                gridControl1.DataSource = DsTable;
                gridView1.OptionsBehavior.Editable = true;
                gridView1.Columns["ModulesCode"].Visible = false;
                gridView1.Columns["ModuleAccessCode"].Visible = false;
                PopulateRepositoryItems(gridView1.Columns["AccessRight"]);
                gridView1.Columns["ModulesName"].OptionsColumn.AllowEdit = false;
                gridView1.Columns["ModuleAccessName"].OptionsColumn.AllowEdit = false;
                gridView1.Columns["AccessRight"].OptionsColumn.AllowEdit = true;
                gridView1.OptionsView.ColumnAutoWidth = false;
                gridView1.BestFitColumns();
            }
        }

        void PopulateRepositoryItems(GridColumn column)
        {
            DataTable Dt;
            (new BusinessLogic()).LoadDataTable(ProgramLoadTableCode.ApplicationRight, Program.StateCode, out Dt);
            repositoryRight.DataSource = Dt;
            repositoryRight.DisplayMember = "ApplicationRightName";
            repositoryRight.ValueMember = "ApplicationRghtID";
            column.ColumnEdit = repositoryRight;

            //{
            //    //Common.setComboList(cbApplication, Dt, "ApplicationRghtID", "ApplicationRightName");
            //    repositoryRight.DataSource = Dt;
            //    repositoryRight.DisplayMember = "ApplicationRightName";
            //    repositoryRight.ValueMember = "ApplicationRghtID";
            //}
            //else
            //    repositoryRight.DataSource = null;
        }

        void ClearGridControl(GridControl grid)
        {
            grid.DataSource = null;
            (grid.MainView as ColumnView).Columns.Clear();
        }

        void gridView1_CellValueChanged(object sender, CellValueChangedEventArgs e)
        {
            gridView1.BestFitColumns();
        }

        void AddDataRowToDataTable(ref DataTable Dt, DataTable Dtt)
        {
            if (Dtt != null)
            {
                if (Dt == null)
                {
                    Dt = Dtt.Copy();
                    Dt.Columns.Add(new DataColumn("AccessRight"));
                }
                else
                {
                    foreach (DataRow Dr in Dtt.Rows)
                    {
                        //Dt.Rows.Add(Dr);
                        Dt.ImportRow(Dr);
                    }
                }
            }
        }

        void RemoveDataRowsFromDataTable(int index)
        {
            object lol = checkedListBoxControl1.GetItemValue(index);
            string ID = lol as String;
            if (!string.IsNullOrEmpty(ID))
            {
                DataTable DsTable = gridControl1.DataSource as DataTable;
                if (DsTable != null)
                {
                    DataRow[] Dr = DsTable.Select(String.Format("ModulesCode = '{0}'", ID));
                    if (Dr != null && Dr.Count() > 0)
                    {
                        foreach (DataRow item in Dr)
                        {
                            DsTable.Rows.Remove(item);
                        }
                        DsTable.AcceptChanges();
                    }
                }
            }
        }

        void OnDataSourceChanged(object sender, EventArgs e)
        {
            if (iTransType == TransactionTypeCode.New | iTransType == TransactionTypeCode.Edit)
            {
                try
                {
                    DataTable Dt = (sender as GridControl).DataSource as DataTable;
                    if (Dt != null)
                    {
                        gridView1.Columns["ModulesName"].SortIndex = 0;
                        gridView1.Columns["ModulesName"].SortOrder = DevExpress.Data.ColumnSortOrder.Ascending;

                        gridView1.Columns["ModuleAccessName"].SortIndex = 1;
                        gridView1.Columns["ModuleAccessName"].SortOrder = DevExpress.Data.ColumnSortOrder.Ascending;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        void gridView1_DoubleClick(object sender, EventArgs e)
        {
            tsbEdit.PerformClick();
        }

        void Bttn_Click(object sender, EventArgs e)
        {
            if (sender == bttnCancel)
            {
                if (MosesClassLibrary.Utilities.Common.AskQuestion("Closing this form will result to data lost.\nAre you sure you want to continue?", Program.ApplicationName))
                    tsbReload.PerformClick();
            }
            else if (sender == bttnReset)
            {
                if (!boolIsUpdate)
                    ClearForm();
                else
                    FillField(ID);
            }
            else if (sender == bttnUpdate)
            {
                ProcessData();
            }
        }

        protected void ProcessData()
        {
            if (
                !Common.IsNullOrEmpty(txtUsername, "Username", Program.ApplicationName)
                && !Common.IsNullOrEmpty(txtPassword, "Password", Program.ApplicationName)
                && !Common.IsNullOrEmpty(txtPassword2, "Confirm Password", Program.ApplicationName)
                //&& !Common.IsNullOrEmpty(cbTaxOffice, "Tax Office", Program.ApplicationName)
                && !Common.IsNullOrEmpty(cbUserType, "Class of User", Program.ApplicationName)
                && !Common.IsNullOrEmpty(txtSurname, "Surname", Program.ApplicationName)
                && !Common.IsNullOrEmpty(txtOthernames, "Othernames", Program.ApplicationName)
                && !Common.IsNullOrEmpty(cbApplication, "Application Type", Program.ApplicationName)
                )
            {
                if (!MosesClassLibrary.Utilities.DataFormat.IsAlphaNumericOnly(txtPassword.Text))
                {
                    Common.setMessageBox("Password support only AlphaNumeric", Program.ApplicationName, 3);
                    txtPassword.Focus();
                    return;
                }
                if (!MosesClassLibrary.Utilities.DataFormat.IsAlphaNumericOnly(txtPassword2.Text))
                {
                    Common.setMessageBox("Password support only AlphaNumeric", Program.ApplicationName, 3);
                    txtPassword2.Focus();
                    return;
                }
                if (!String.Equals(txtPassword.Text, txtPassword2.Text))
                {
                    Common.setMessageBox("Password does not match", Program.ApplicationName, 3);
                    txtPassword.Focus();
                    return;
                }
                string UserID = txtUsername.Text;
                string ApplicationCode = Common.GetComboBoxValue(cbApplication, true);
                BusinessLogic Blogic = new BusinessLogic();
                if (Blogic.InsertUser(UserID, Encryption.Encrypt(txtPassword.Text), txtSurname.Text
                    , txtOthernames.Text, Common.GetComboBoxValue(cbUserType, true), ApplicationCode, boolIsUpdate))
                {
                    string userAppID = Blogic.GetUserApplicationID(UserID, ApplicationCode);
                    if (!String.IsNullOrEmpty(userAppID))
                        if (ProcessDataTable(Convert.ToInt32(userAppID), ref Blogic))
                        {
                            Common.setMessageBox("User Access Right Successfully created", Program.ApplicationName, 1);
                            tsbReload.PerformClick();
                        }
                }
            }
        }

        bool ProcessDataTable(int UserAppID, ref BusinessLogic BLogic)
        {
            bool bResponse = false;
            DataTable DsTable = gridControl1.DataSource as DataTable;
            if (DsTable != null && DsTable.Rows.Count > 0)
            {
                bResponse = false;
                bool bRes = false;
                foreach (DataRow Dr in DsTable.Rows)
                {
                    int UserAccessID = 0;
                    //bRes = BLogic.InsertUserApplicationModulesAccess2(UserAppID, Dr["ModulesCode"].ToString(), Dr["ModuleAccessCode"].ToString()
                    //    , out UserAccessID);
                    bRes = true;
                    if (bRes)
                    {
                        string UserRightList = Dr["AccessRight"] as string;
                        if (!String.IsNullOrEmpty(UserRightList))
                        {
                            bRes = BLogic.InsertUserApplicationModulesAccess2(UserAppID, Dr["ModulesCode"].ToString(), Dr["ModuleAccessCode"].ToString(), out UserAccessID);
                            if (bRes)
                            {
                                string[] UserRight = UserRightList.Split(',');
                                if (UserRight != null && UserRight.Count() > 0 && !string.Equals(UserRight, null))
                                {
                                    foreach (string obj in UserRight)
                                    {
                                        if (String.IsNullOrEmpty(obj)) continue;
                                        //bRes = BLogic.InsertUserApplicationModulesAccess2(UserAppID, Dr["ModulesCode"].ToString(), Dr["ModuleAccessCode"].ToString(), out UserAccessID);
                                        int UserRightID = Convert.ToInt32(obj);
                                        bRes = BLogic.InsertUserAccessRight(UserAccessID, UserRightID);
                                        if (bRes)
                                            bResponse = true;
                                    }
                                }
                            }
                        }
                    }
                    else
                        break;
                }
            }
            return bResponse;
        }

        void ClearForm()
        {
            Common.ClearFields(groupBox1);
            cbApplication.SelectedIndex = -1;
        }

        bool FillField(object UserID)
        {
            if (String.IsNullOrEmpty(UserID.ToString()))
                return false;
            bool bResponse = false;
            string SQL = String.Format(@"SELECT * FROM [dbo].[tblUsers] WHERE [UserID] = '{0}';
                    SELECT [ApplicationCode] FROM [dbo].[ViewUserApplication] WHERE [UserID] = '{0}'", ID);
            DataSet Ds = (new BusinessLogic()).getSqlStatement(SQL);
            DataTable Dt = Ds.Tables[0];
            DataTable Dtt = Ds.Tables[1];
            if (Dt != null && Dt.Rows.Count > 0)
            {
                DataRow Dr = Dt.Rows[0];
                if (Dr != null)
                {
                    txtUsername.Text = Dr["UserID"].ToString();
                    txtPassword.Text = txtPassword2.Text = Encryption.Decrypt(Dr["Password"].ToString());
                    txtSurname.Text = Dr["Surname"].ToString();
                    txtOthernames.Text = Dr["Othernames"].ToString();
                    cbUserType.SelectedValue = Dr["UserCode"];
                    cbApplication.SelectedValue = Dtt.Rows[0]["ApplicationCode"];
                    EditApplicationModules();
                    bResponse = true;
                }
            }
            else
                bResponse = false;
            return bResponse;
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
                    ID = dr["UserID"];
                    bResponse = FillField(ID);
                }
                else
                {
                    Common.setMessageBox("No Record is seleected", Program.ApplicationName, 1);
                    bResponse = false;
                }
            }
            return bResponse;
        }

        protected void setDBComboBox2()
        {
            DataTable Dt;
            if ((new BusinessLogic()).LoadDataTable(ProgramLoadTableCode.UserApplication, ID.ToString(), out Dt))
                Common.setComboList(cbApplication, Dt, "ApplicationCode", "ApplicationName");
        }

        protected void EditApplicationModules()
        {
            SetIndex();
            DataTable Dt;
            if ((new BusinessLogic()).LoadDataTable(ProgramLoadTableCode.UserApplicationModules, Common.GetComboBoxValue(cbApplication, true), out Dt))
            {
                foreach (DataRow Dr in Dt.Rows)
                {
                    int i = 0;
                    while (checkedListBoxControl1.GetItem(i) != null)
                    {
                        if (object.Equals(checkedListBoxControl1.GetItemValue(i), Dr["ModulesCode"]))
                        {
                            checkedListBoxControl1.SetItemChecked(i, true);
                        }
                        i++;
                    }
                }
            }
        }

        protected void EditApplicationRight(ref DataTable DsTable)
        {
            //GridView view = gridView1;
            //if (view == null) return;
            //for (int i = 0; i < view.RowCount; i++)
            //{
            //    DataRow Dr = view.GetDataRow(i);

            //}
            if (DsTable == null) return;
            foreach (DataRow Dr in DsTable.Rows)
            {
                string ModulesCode = Dr["ModulesCode"] as string;
                string ModuleAccessCode = Dr["ModuleAccessCode"] as string;
                object AccessRight = Dr["AccessRight"];
                string SQL = String.Format(@"SELECT [ApplicationRghtID] FROM [dbo].[ViewUserApplicationRight] 
                                WHERE [UserID] = '{0}' AND [ModulesCode] ='{1}' AND [ModuleAccessCode] = '{2}'"
                    , ID, ModulesCode, ModuleAccessCode);
                DataTable Dt = (new BusinessLogic()).getSqlStatement(SQL).Tables[0];
                if (Dt != null && Dt.Rows.Count > 0)
                {
                    AccessRight = ConvertDataTableToStringDelimiter(ref Dt, ",", "ApplicationRghtID");
                    Dr["AccessRight"] = AccessRight;
                }
            }
        }

        void SetIndex()
        {
            DataView dv = cbApplication.DataSource as DataView;
            DataTable Dt = dv.Table;
            if (Dt != null && Dt.Rows.Count > 0)
                if (cbApplication.SelectedIndex == -1)
                    cbApplication.SelectedIndex = 0;
            dv = null;
        }

        string ConvertDataTableToStringDelimiter(ref DataTable Dt, string delimiter, string columnName)
        {
            if (Dt == null) return null;
            string strResponse = string.Empty;
            foreach (DataRow Dr in Dt.Rows)
            {
                strResponse += String.Format("{1} {0}", Dr[columnName], String.IsNullOrEmpty(strResponse) ? "" : ",");
            }
            return strResponse;
        }
    }
}
