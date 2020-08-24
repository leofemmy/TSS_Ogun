using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using UserManager.Model;
using DevExpress.Utils;
using TaxSmartSuite;
using TaxSmart.Data;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraEditors;
using MosesClassLibrary.Security;
using DevExpress.XtraGrid.Views.Grid;
using TSS = TaxSmartSuite.Controls;
using TaxSmartSuite.Class;

namespace UserManager.Forms
{
    public partial class FrmManageUser2 : Form
    {
        //FORM VARIABLES
        public static FrmManageUser2 publicUser;

        protected TransactionTypeCode iTransType;
        protected bool boolIsUpdate;
        protected object ID;
        protected bool IsFirst = true;

        readonly RepositoryItemCheckedComboBoxEdit repositoryRight = new RepositoryItemCheckedComboBoxEdit();
        TSS.GridCheckMarksSelection selection;

        public FrmManageUser2()
        {
            using (WaitDialogForm dlg = new WaitDialogForm("Please Wait", "Initialising Form Data"))
            {
                InitializeComponent();
                publicUser = this;

                setImages();
                OtherEvents();
                ToolStripEvent();

                Init();
            }
        }

        void Init()
        {
            tblApplicationSetUpBindingSource.DataSource = EntityModel.GetContext.TblApplicationSetUp
                .ByFlag(true).OrderBy(p => p.ApplicationName);
            tblUsersTypeBindingSource.DataSource = EntityModel.GetContext.TblUsersType;
            gridControl1.RepositoryItems.Add(repositoryRight);
            tsbReload.PerformClick();
            CheckBoxColumn();
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

        protected void OtherEvents()
        {
            bttnCancel.Click += Bttn_Click;
            bttnReset.Click += Bttn_Click;
            bttnUpdate.Click += Bttn_Click;
            bttnLoad.Click += Bttn_Click;

            //cbApplication.SelectedValueChanged += cbApplication_SelectedValueChanged;
            checkedListBoxControl1.ItemCheck += checkedListBoxControl1_ItemCheck;

            gridView1.DoubleClick += gridView1_DoubleClick;
            checkedComboBoxEdit1.EditValueChanged += checkedComboBoxEdit1_EditValueChanged;

            checkEdit1.CheckStateChanged += checkEdit1_CheckStateChanged;
        }

        void checkEdit1_CheckStateChanged(object sender, EventArgs e)
        {
            string value = null;
            for (int i = 0; i < gridView1.RowCount; i++)
            {
                if (checkEdit1.CheckState == CheckState.Checked)
                    value = "1, 2, 3";
                else
                    value = null;
                gridView1.SetRowCellValue(i, "AccessRight", value);
            }
        }

        void checkedComboBoxEdit1_EditValueChanged(object sender, EventArgs e)
        {
            checkedListBoxControl1.DataSource = null;
            Common.ClearGridControl(gridControl1);
            if (checkedComboBoxEdit1.EditValue == null) return;
            string value = checkedComboBoxEdit1.EditValue as String;
            string[] valueList = value.Split(',').Select(p => p.Trim()).ToArray();
            //var query = from p in EntityModel.GetContext.TblApplicationModules
            //                                    where valueList.Contains(p.ApplicationCode)
            //                                    select p;
            //checkedListBoxControl1.DataSource = query;
            viewApplicationModulesBindingSource.DataSource = from p in EntityModel.GetContext.ViewApplicationModules
                                                             where valueList.Contains(p.ApplicationCode)
                                                             select p;
            gridView2.BestFitColumns();
        }

        void gridView1_DoubleClick(object sender, EventArgs e)
        {
            if (iTransType == TransactionTypeCode.Reload)
                tsbEdit.PerformClick();
        }

        void cbApplication_SelectedValueChanged(object sender, EventArgs e)
        {
            //checkedListBoxControl1.DataSource = null;
            //Common.ClearGridControl(gridControl1);
            //if (cbApplication.DataSource == null || cbApplication.SelectedValue == null) return;
            //checkedListBoxControl1.DataSource = EntityModel.GetContext.TblApplicationModules.ByApplicationCode(Common.GetComboBoxValue(cbApplication, true)).ToList();
        }

        void checkedListBoxControl1_ItemCheck(object sender, DevExpress.XtraEditors.Controls.ItemCheckEventArgs e)
        {
            if (e.State == CheckState.Checked)
            {
                PopulateGrid(e.Index);
            }
            else if (e.State == CheckState.Unchecked)
            {
                RemoveDataRowsFromDataTable(e.Index);
            }
        }

        void PopulateGrid(int index)
        {
            /*
            string ID = checkedListBoxControl1.GetItemValue(index) as String;
            if (!string.IsNullOrEmpty(ID))
            {
                var appAccess = EntityModel.GetContext.ViewApplicationAccess.ByModulesCode(ID)
                    .Select(p => new AppAccessDisplay { ModulesName = p.ModulesName, ModuleAccessName = p.ModuleAccessName, Entity = p });
                List<AppAccessDisplay> DsTable = gridControl1.DataSource as List<AppAccessDisplay>;
                AddDataRowToDataTable(ref DsTable, appAccess.ToList());
                if (boolIsUpdate) EditApplicationRight(ref DsTable);
                gridControl1.DataSource = DsTable;
                gridView1.OptionsBehavior.Editable = true;
                gridView1.Columns["Entity"].Visible = false;
                //gridView1.Columns["ModuleAccessCode"].Visible = false;
                PopulateRepositoryItems(gridView1.Columns["AccessRight"]);
                gridView1.Columns["ModulesName"].OptionsColumn.AllowEdit = false;
                gridView1.Columns["ModuleAccessName"].OptionsColumn.AllowEdit = false;
                gridView1.Columns["AccessRight"].OptionsColumn.AllowEdit = true;
                gridView1.OptionsView.ColumnAutoWidth = false;
                gridView1.BestFitColumns();
                //SortGridView();
                gridView1.RefreshData();
            }
            */
        }

        void AddDataRowToDataTable(ref List<AppAccessDisplay> Dt, List<AppAccessDisplay> Dtt)
        {
            if (Dtt != null)
            {
                if (Dt == null)
                {
                    Dt = Dtt;
                }
                else
                {
                    Dt.AddRange(Dtt);
                }
            }
        }

        void EditApplicationRight(ref List<AppAccessDisplay> DsTable)
        {
            if (DsTable == null) return;
            foreach (var item in DsTable)
            {
                string ModulesCode = item.Entity.ModulesCode;
                string ModuleAccessCode = item.Entity.ModuleAccessCode;
                object AccessRight = item.AccessRight;
                var query = EntityModel.GetContext.ViewUserApplicationRight
                    .Where(p => p.UserID == ID && p.ModulesCode == ModulesCode && p.ModuleAccessCode == ModuleAccessCode)
                    .Select(p => p.ApplicationRghtID);
                if (query != null)
                {
                    AccessRight = TSS_Utils.Join(",", query, i => i.ToString());
                    item.AccessRight = AccessRight as String;
                }
            }
        }

        void PopulateRepositoryItems(GridColumn column)
        {
            var query = EntityModel.GetContext.TblApplicationRight;
            repositoryRight.DataSource = query;
            repositoryRight.DisplayMember = "ApplicationRightName";
            repositoryRight.ValueMember = "ApplicationRghtID";
            column.ColumnEdit = repositoryRight;
        }

        void RemoveDataRowsFromDataTable(int index)
        {
            string ID = checkedListBoxControl1.GetItemValue(index) as String;
            if (!string.IsNullOrEmpty(ID))
            {
                List<AppAccessDisplay> DsTable = gridControl1.DataSource as List<AppAccessDisplay>;
                if (DsTable != null)
                {
                    DsTable.RemoveAll(p => p.Entity.ModulesCode == ID);
                    gridView1.RefreshData();
                }
            }
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
            else if (sender == bttnLoad)
            {
                //LoadApplicationAccess();
                if (selection != null /*&& selection.SelectedCount > 0*/)
                {
                    LoadApplicationAccess();
                }
                else
                    Common.setMessageBox("No Application Module is Selected", Program.ApplicationName, 3);
            }
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
                //cbApplication_SelectedIndexChanged(null, null);
                iTransType = TransactionTypeCode.New;
                ShowForm();
                boolIsUpdate = false;
                txtUsername.Enabled = true;
                //cbApplication.SelectedIndex = -1;
                //checkedComboBoxEdit1.EditValue = null;
                bttnReset.PerformClick();
            }
            else if (sender == tsbEdit)
            {
                boolIsUpdate = true;
                if (EditRecordMode())
                {
                    groupControl1.Text = "Edit Record Mode";
                    iTransType = TransactionTypeCode.Edit;
                    ShowForm();
                    //boolIsUpdate = true;
                    txtUsername.Enabled = false;
                    //SetIndex();
                    //cbApplication_SelectedIndexChanged(null, null);
                }
            }
            else if (sender == tsbReload)
            {
                groupControl1.Text = Program.ApplicationName;
                iTransType = TransactionTypeCode.Reload;
                ShowForm();
                //setReload();
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
                    Common.ClearGridControl(gridControl1);
                    groupControl2.Text = "Modules Access and Right";
                    break;
                case TransactionTypeCode.Edit:
                    splitContainerControl1.PanelVisibility = SplitPanelVisibility.Both;
                    //Common.ClearGridControl(gridControl1);
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

        private void setReload()
        {
            try
            {
                Common.ClearGridControl(gridControl1);
                gridControl1.DataSource = EntityModel.GetContext.ViewUser;
                gridControl1.ForceInitialize();
                gridView1.OptionsBehavior.Editable = false;
                gridView1.OptionsView.ColumnAutoWidth = true;
                //gridView1.Columns["Id"].Visible = true;
                //gridView1.Columns["ClassOfUser"].Visible = true;
                gridView1.BestFitColumns();
            }
            catch (Exception ex)
            {
                Tripous.Sys.ErrorBox(ex);
            }
        }

        void SortGridView()
        {
            if (iTransType == TransactionTypeCode.New | iTransType == TransactionTypeCode.Edit)
            {
                try
                {
                    List<AppAccessDisplay> Dt = gridControl1.DataSource as List<AppAccessDisplay>;
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

        void ClearForm()
        {
            Common.ClearFields(groupBox1);
            //cbApplication.SelectedIndex = -1;
            checkedComboBoxEdit1.EditValue = null;
            checkedComboBoxEdit1.RefreshEditValue();
            viewApplicationModulesBindingSource.DataSource = null;
            Common.ClearGridControl(gridControl1);
        }

        bool FillField(object UserID)
        {
            if (String.IsNullOrEmpty(UserID as String))
                return false;
            bool bResponse = false;
            string ID = UserID as String;
            var query = EntityModel.GetContext.TblUsers.GetByKey(ID);
            var query2 = EntityModel.GetContext.ViewUserApplication.ByUserID(ID);
            if (query != null)
            {
                txtUsername.Text = query.UserID;
                txtSurname.Text = query.Surname;
                txtOthernames.Text = query.Othernames;
                txtPassword.Text = txtPassword2.Text = Encryption.Decrypt(query.Password);
                cbUserType.SelectedValue = query.UserCode;
                EditApplicationModules(ID);
                bResponse = true;
            }

            return bResponse;
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
                && !Common.IsNullOrEmpty(checkedComboBoxEdit1, "Application Type", Program.ApplicationName)
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
                ProcessData3();
            }
        }

        void ProcessData2()
        {
            string UserID = txtUsername.Text;
            string ApplicationCode = Common.GetComboBoxValue(cbApplication, true);
            using (WaitDialogForm dlg = new WaitDialogForm("Please Wait", "Performing Operation"))
            {
                using (TaxSmartDataContext context = new TaxSmartDataContext())
                {
                    TblUsers user = null;
                    TblUsersApplication userApp = null;
                    #region Process User Record and user Application
                    if (!boolIsUpdate)
                    {
                        user = new TblUsers();
                        user.UserID = UserID;
                        userApp = new TblUsersApplication();
                        userApp.ApplicationCode = ApplicationCode;
                        context.TblUsersApplication.InsertOnSubmit(userApp);
                        user.TblUsersApplicationList.Add(userApp);
                    }
                    else
                        user = context.TblUsers.GetByKey(UserID);
                    user.Password = Encryption.Encrypt(txtPassword.Text);
                    user.Surname = txtSurname.Text;
                    user.Othernames = txtOthernames.Text;
                    user.UserCode = Common.GetComboBoxValue(cbUserType, true);
                    #endregion

                    List<AppAccessDisplay> Dt = gridControl1.DataSource as List<AppAccessDisplay>;
                    if (Dt != null && Dt.Count > 0)
                    {
                        if (!boolIsUpdate)
                        {
                            #region Process user Application Modules Access
                            List<TblUserApplicationModulesAccess> userAppAccessList = new List<TblUserApplicationModulesAccess>();
                            foreach (var item in Dt)
                            {
                                string UserRightList = item.AccessRight;
                                if (!String.IsNullOrEmpty(UserRightList))
                                {
                                    TblUserApplicationModulesAccess userAppAccess = new TblUserApplicationModulesAccess();
                                    userAppAccess.ModulesCode = item.Entity.ModulesCode;
                                    userAppAccess.ModuleAccessCode = item.Entity.ModuleAccessCode;

                                    #region Process User Application Rights
                                    string[] UserRight = UserRightList.Split(',');
                                    if (UserRight != null && UserRight.Count() > 0 && !string.Equals(UserRight, null))
                                    {
                                        foreach (string obj in UserRight)
                                        {
                                            if (String.IsNullOrEmpty(obj)) continue;
                                            int UserRightID = Convert.ToInt32(obj);
                                            userAppAccess.TblUsersRightList
                                                .Add(new TblUsersRight { ApplicationRghtID = UserRightID });
                                        }
                                    }
                                    #endregion

                                    userAppAccessList.Add(userAppAccess);
                                }
                            }
                            userApp.TblUserApplicationModulesAccessList.AddRange(userAppAccessList);
                            #endregion
                        }
                    }
                    context.SubmitChanges();
                    Common.setMessageBox("User Access Right Successfully created", Program.ApplicationName, 1);
                    tsbReload.PerformClick();
                }
            }
        }

        void ProcessData3()
        {
            string UserID = txtUsername.Text;
            using (WaitDialogForm dlg = new WaitDialogForm("Please Wait", "Performing Operation"))
            {
                using (TaxSmartDataContext context = new TaxSmartDataContext())
                {
                    TblUsers user = null;
                    #region Process User Record
                    if (!boolIsUpdate)
                    {
                        user = new TblUsers();
                        user.UserID = UserID;
                    }
                    else
                        user = context.TblUsers.GetByKey(UserID);
                    user.Password = Encryption.Encrypt(txtPassword.Text);
                    user.Surname = txtSurname.Text;
                    user.Othernames = txtOthernames.Text;
                    user.UserCode = Common.GetComboBoxValue(cbUserType, true);
                    #endregion

                    // Get Selected Application Type
                    string value = checkedComboBoxEdit1.EditValue as String;
                    string[] valueList = value.Split(',').Select(p => p.Trim()).ToArray();


                    List<AppAccessDisplay> Dt = gridControl1.DataSource as List<AppAccessDisplay>;
                    if (Dt != null && Dt.Count > 0)
                    {
                        if (!boolIsUpdate)
                        {
                            #region Process user Application
                            List<TblUsersApplication> userAppList = new List<TblUsersApplication>();
                            foreach (var AppCode in valueList)
                            {
                                TblUsersApplication userApp = new TblUsersApplication();
                                userApp.ApplicationCode = AppCode;

                                //ProcessNewUserApplication(Dt, AppCode, userApp);
                                #region Process user Application Modules Access
                                List<TblUserApplicationModulesAccess> userAppAccessList = new List<TblUserApplicationModulesAccess>();
                                foreach (var item in Dt.Where(p => p.Entity.ApplicationCode == AppCode))
                                {
                                    string UserRightList = item.AccessRight;
                                    if (!String.IsNullOrEmpty(UserRightList))
                                    {
                                        TblUserApplicationModulesAccess userAppAccess = new TblUserApplicationModulesAccess();
                                        userAppAccess.ModulesCode = item.Entity.ModulesCode;
                                        userAppAccess.ModuleAccessCode = item.Entity.ModuleAccessCode;

                                        #region Process User Application Rights
                                        string[] UserRight = UserRightList.Split(',');
                                        if (UserRight != null && UserRight.Count() > 0 && !string.Equals(UserRight, null))
                                        {
                                            foreach (string obj in UserRight)
                                            {
                                                if (String.IsNullOrEmpty(obj)) continue;
                                                int UserRightID = Convert.ToInt32(obj);
                                                userAppAccess.TblUsersRightList
                                                    .Add(new TblUsersRight { ApplicationRghtID = UserRightID });
                                            }
                                        }
                                        #endregion

                                        userAppAccessList.Add(userAppAccess);
                                    }
                                }
                                userApp.TblUserApplicationModulesAccessList.AddRange(userAppAccessList);
                                #endregion

                                userAppList.Add(userApp);
                            }
                            user.TblUsersApplicationList.AddRange(userAppList);
                            #endregion
                        }
                        else
                        {
                            List<TblUsersApplication> userAppList = new List<TblUsersApplication>();
                            foreach (var AppCode in valueList)
                            {
                                bool isInsert = false;
                                var userApp = context.TblUsersApplication.ByApplicationCode(AppCode).FirstOrDefault();
                                if (userApp == null)   // Insert New Record
                                {
                                    userApp = new TblUsersApplication();
                                    userApp.ApplicationCode = AppCode;
                                    isInsert = true;
                                }
                            }
                        }
                    }
                    else
                    {
                        Common.setMessageBox("No Application Access Right is selected", Program.ApplicationName, 3);
                        return;
                    }
                    if (!boolIsUpdate)
                        context.TblUsers.InsertOnSubmit(user);

                    context.SubmitChanges();

                    if (!boolIsUpdate)
                        Common.setMessageBox("User Access Right Successfully created", Program.ApplicationName, 1);
                    else
                        Common.setMessageBox("User Access Right Successfully updated", Program.ApplicationName, 1);
                    tsbReload.PerformClick();
                }
            }
        }

        protected bool EditRecordMode()
        {
            bool bResponse = false;
            GridView view = (GridView)gridControl1.FocusedView;
            if (view != null)
            {
                ViewUser dr = view.GetRow(view.FocusedRowHandle) as ViewUser;
                if (dr != null)
                {
                    ID = dr.UserID;
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

        protected void EditApplicationModules(Object UserID)
        {
            if (String.IsNullOrEmpty(UserID as String))
                return;
            string ID = UserID as String;
            var context = EntityModel.GetContext;
            var userApp = context.ViewUserApplicationModules.ByUserID(ID);
            if (userApp != null && userApp.Count() > 0)
            {
                #region Update Selected Application Type
                var app = userApp.Select(p => p.ApplicationCode).Distinct();
                checkedComboBoxEdit1.EditValue = TSS_Utils.Join(",", app, i => i);
                checkedComboBoxEdit1.RefreshEditValue();
                #endregion
                var modules = userApp.Select(p => p.ModulesCode).Distinct();
                SetGridviewValue(modules.ToList());
                bttnLoad.PerformClick();
            }
            context.Dispose();
            context = null;
        }

        void SetGridviewValue(List<string> modulesCode)
        {
            GridView view = gridView2;
            for (int i = 0; i < view.RowCount; i++)
            {
                var item = view.GetRow(i) as ViewApplicationModules;
                if (item == null) continue;
                bool value = modulesCode.Any(p => p == item.ModulesCode);
                selection.SelectRow(i, value);
            }
            view.RefreshData();
            (view.GridControl).ForceInitialize();
        }

        void CheckBoxColumn()
        {
            if (selection != null) selection.Detach();
            selection = new TSS.GridCheckMarksSelection(gridView2);
            selection.CheckMarkColumn.VisibleIndex = 0;
        }

        void LoadApplicationAccess()
        {
            #region Get selected Records Modules Codes
            List<string> indices = new List<string>();
            for (int i = 0; i < selection.SelectedCount; i++)
            {
                ViewApplicationModules assess = selection.GetSelectedRow(i) as ViewApplicationModules;
                if (assess != null)
                {
                    indices.Add(assess.ModulesCode);
                }
            }
            #endregion

            #region Bind the Load Application Access to gridview
            var appAccess = from p in EntityModel.GetContext.ViewApplicationModulesAndAccess
                            where indices.Contains(p.ModulesCode)
                            select new AppAccessDisplay { AppicationName = p.ApplicationName, ModulesName = p.ModulesName, ModuleAccessName = p.ModuleAccessName, Entity = p };
            List<AppAccessDisplay> DsTable = gridControl1.DataSource as List<AppAccessDisplay>;
            // Remove Non selected Records
            if (DsTable != null)
                DsTable.RemoveAll(p => !indices.Contains(p.Entity.ModulesCode));
            //Add New Selected Records
            AddDataRowToDataTable2(ref DsTable, appAccess.ToList());

            if (boolIsUpdate) EditApplicationRight(ref DsTable);
            Common.ClearGridControl(gridControl1);
            gridControl1.DataSource = DsTable;
            gridView1.OptionsBehavior.Editable = true;
            gridView1.Columns["Entity"].Visible = false;
            PopulateRepositoryItems(gridView1.Columns["AccessRight"]);
            gridView1.Columns["AppicationName"].OptionsColumn.AllowEdit = false;
            gridView1.Columns["ModulesName"].OptionsColumn.AllowEdit = false;
            gridView1.Columns["ModuleAccessName"].OptionsColumn.AllowEdit = false;
            gridView1.Columns["AccessRight"].OptionsColumn.AllowEdit = true;
            gridView1.OptionsView.ColumnAutoWidth = false;
            gridView1.RefreshData();
            gridView1.BestFitColumns();
            #endregion
            //SortGridView();
        }

        void AddDataRowToDataTable2(ref List<AppAccessDisplay> Dt, List<AppAccessDisplay> Dtt)
        {
            if (Dtt != null)
            {
                if (Dt == null)
                {
                    Dt = Dtt;
                }
                else
                {
                    List<AppAccessDisplay> lol = Dt;
                    var str = lol.Select(q => q.Entity.ModulesCode);
                    Dt.AddRange(Dtt.Where(p => !str.Contains(p.Entity.ModulesCode)));
                    lol = null;
                }
            }
        }

        public void RefreshForm()
        {
            tsbReload.PerformClick();
        }

        List<TblTempEmployeeAnnualAssess> ProcessEmployeeArrayList(List<AppAccessDisplay> arr, TaxSmartDataContext context)
        {
            List<TblTempEmployeeAnnualAssess> empArrayList = new List<TblTempEmployeeAnnualAssess>();

            return empArrayList;
        }

        class AppAccessDisplay
        {
            public string AppicationName { get; set; }
            public string ModulesName { get; set; }
            public string ModuleAccessName { get; set; }
            public string AccessRight { get; set; }
            public ViewApplicationModulesAndAccess Entity { get; set; }
        }
    }
}
