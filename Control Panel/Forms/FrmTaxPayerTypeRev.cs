using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraGrid.Selection;
using TaxSmartSuite.Class;
using System.Data.SqlClient;
using Control_Panel.Class;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;

namespace Control_Panel.Forms
{
    public partial class FrmTaxPayerTypeRev : Form
    {
        private DataTable Dt;

        GridCheckMarksSelection selection;

        public static FrmTaxPayerTypeRev publicStreetGroup;

        protected TransactionTypeCode iTransType;

        protected bool boolIsUpdate;

        protected string ID;

        bool isFirstGrid = true;

        bool isFirst = true;

        public FrmTaxPayerTypeRev()
        {
            InitializeComponent();

            publicStreetGroup = this;

            setImages();

            ToolStripEvent();

            Load += OnFormLoad;

            iTransType = TransactionTypeCode.Null;

            OnFormLoad(null, null);

            bttnUpdate.Click += bttnUpdate_Click;
        }

        void bttnUpdate_Click(object sender, EventArgs e)
        {
            if (selection.SelectedCount == 0)
            {
                Common.setMessageBox("Please Select Revenue Code to Map with Tax Payer Type", Program.ApplicationName, 1);
                return;
            }
            else
            {
                if (!boolIsUpdate)
                {
                    using (SqlConnection db = new SqlConnection(Logic.ConnectionString))
                    {
                        SqlTransaction transaction;

                        db.Open();

                        transaction = db.BeginTransaction();

                        try
                        {
                            for (int i = 0; i < selection.SelectedCount; i++)
                            {

                                using (var ds = new System.Data.DataSet())
                                {//select record

                                    //get the selected value from the  gridview
                                    string lol = ((selection.GetSelectedRow(i) as DataRowView)["RevenueCode"].ToString());

                                    //insert the selected record into the table
                                    string query = String.Format("INSERT INTO [tblRevenueCodeTaxPayerTypeRelation]([TaxPayerTypeCode],[RevenueCode],ModifyBy) VALUES ('{0}','{1}','{2}')", cboStation.SelectedValue, lol, Program.UserID);

                                    using (SqlCommand sqlCommand1 = new SqlCommand(query, db, transaction))
                                    {
                                        sqlCommand1.ExecuteNonQuery();
                                    }

                                }
                            }

                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            Tripous.Sys.ErrorBox(ex); return;
                        }

                        db.Close(); selection.ClearSelection(); setReload(); cboStation.SelectedIndex = -1;

                    }
                }
                else
                {
                    //delete exiting records
                    using (SqlConnection db = new SqlConnection(Logic.ConnectionString))
                    {
                        SqlTransaction transaction;

                        SqlCommand sqlCommand1;

                        db.Open();

                        transaction = db.BeginTransaction();

                        try
                        {
                            string query = String.Format("DELETE FROM tblRevenueCodeTaxPayerTypeRelation where TaxPayerTypeCode ='{0}' ", ID);

                            sqlCommand1 = new SqlCommand(query, db, transaction);
                            sqlCommand1.ExecuteNonQuery();

                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            Tripous.Sys.ErrorBox(ex);
                            return;
                        }
                        db.Close();
                    }

                    //insert new records after modification
                    using (SqlConnection db = new SqlConnection(Logic.ConnectionString))
                    {
                        SqlTransaction transaction;

                        db.Open();

                        transaction = db.BeginTransaction();

                        try
                        {
                            for (int i = 0; i < selection.SelectedCount; i++)
                            {

                                using (var ds = new System.Data.DataSet())
                                {//select record

                                    //get the selected value from the  gridview
                                    string lol = ((selection.GetSelectedRow(i) as DataRowView)["RevenueCode"].ToString());

                                    //insert the selected record into the table
                                    using (SqlCommand sqlCommand1 = new SqlCommand((string)String.Format("INSERT INTO [tblRevenueCodeTaxPayerTypeRelation]([TaxPayerTypeCode],[RevenueCode],ModifyBy) VALUES ('{0}','{1}','{2}')", cboStation.SelectedValue, lol, Program.UserID), db, transaction))
                                    {
                                        sqlCommand1.ExecuteNonQuery();
                                    }

                                }
                            }

                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            Tripous.Sys.ErrorBox(ex);
                            return;
                        }

                        db.Close(); selection.ClearSelection(); cboStation.SelectedIndex = -1;


                    }

                }

                Common.setMessageBox("Record has been successfully Mapped", Program.ApplicationName, 1);

                tsbReload.PerformClick();


            }
        }

        internal GridCheckMarksSelection Selection
        {
            get { return selection; }
        }

        private void setImages()
        {
            tsbNew.Image = MDIMain.publicMDIParent.i16x16.Images[3];
            tsbEdit.Image = MDIMain.publicMDIParent.i16x16.Images[4];
            tsbDelete.Image = MDIMain.publicMDIParent.i16x16.Images[0];
            tsbReload.Image = MDIMain.publicMDIParent.i16x16.Images[2];
            tsbClose.Image = MDIMain.publicMDIParent.i16x16.Images[1];

            ////bttnCancel.Image = MDIMain.publicMDIParent.i32x32.Images[9];
            ////bttnReset.Image = MDIMain.publicMDIParent.i32x32.Images[8];
            bttnUpdate.Image = MDIMain.publicMDIParent.i32x32.Images[29];

        }

        public void RefreshForm()
        {
            NavBars.ToolStripEnableDisableControls(toolStrip, Tag as String);
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
                MDIMain.publicMDIParent.RemoveControls();
            }
            else if (sender == tsbNew)
            {
                //groupControl2.Text = "Add New Record";
                iTransType = TransactionTypeCode.New;
                setReload();
                ShowForm();
                //cboNature.SelectedIndex = -1;
                boolIsUpdate = false;
            }
            else if (sender == tsbEdit)
            {
                //groupControl2.Text = "Edit Record Mode";
                iTransType = TransactionTypeCode.Edit;
                if (EditRecordMode())
                {
                    ShowForm();
                    boolIsUpdate = true;
                }
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
                boolIsUpdate = false;
            }
            else if (sender == tsbReload)
            {
                iTransType = TransactionTypeCode.Reload; setReloadgrc1(); ShowForm();
            }
            //bttnReset.PerformClick();
        }

        void OnFormLoad(object sender, EventArgs e)
        {
            ShowForm();
            setReloadgrc1();
            setDBComboBox();
            isFirst = false;

            //cboNature.KeyPress += cboNature_KeyPress;

        }

        public void setDBComboBox()
        {
            DataTable Dt;

            using (var ds = new System.Data.DataSet())
            {
                using (SqlDataAdapter ada = new SqlDataAdapter((string)"SELECT TaxPayerTypeCode,Name FROM dbo.tblTaxPayerType", Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }

                Dt = ds.Tables[0];
            }

            Common.setComboList(cboStation, Dt, "TaxPayerTypeCode", "Name");

            cboStation.SelectedIndex = -1;

        }

        private void setReload()
        {
            DataTable dt;

            using (var ds = new System.Data.DataSet())
            {
                //connect.connect.Open();
                using (SqlDataAdapter ada = new SqlDataAdapter("SELECT RevenueCode,Description FROM dbo.tblRevenueType WHERE RevenueCode NOT IN (SELECT RevenueCode FROM tblRevenueCodeTaxPayerTypeRelation) ORDER BY RevenueCode", Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }
                //connect.connect.Close();
                dt = ds.Tables[0];
                gridControl4.DataSource = dt.DefaultView;
            }
            gridView4.OptionsBehavior.Editable = false;
            //gridView4.Columns["BankID"].Visible = false;
            gridView4.BestFitColumns();

            if (isFirstGrid)
            {
                selection = new GridCheckMarksSelection(gridView4);
                selection.CheckMarkColumn.VisibleIndex = 0;
                isFirstGrid = false;
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
                    splitContainer1.Panel2Collapsed = true;
                    break;
                case TransactionTypeCode.Edit:
                    splitContainer1.Panel1Collapsed = false;
                    splitContainer1.Panel2Collapsed = true;
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

        private void setReloadgrc1()
        {
            //connect.connect.Close();
            DataTable dt;
            //System.Data.DataSet ds;
            using (var ds = new System.Data.DataSet())
            {
                //connect.connect.Open();
                using (SqlDataAdapter ada = new SqlDataAdapter("select TaxPayerTypeCode,DESCRIPTION as [TaxPayerType],RevenueName  from viewRevenueCodeTaxPayerTypeRelation ORDER BY TaxPayerTypeCode", Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }
                //connect.connect.Close();
                dt = ds.Tables[0];
                gridControl1.DataSource = dt.DefaultView;
            }
            gridView1.OptionsBehavior.Editable = false;
            //gridView1.gr
            gridView1.Columns["TaxPayerTypeCode"].Visible = false;

            gridView1.Columns["TaxPayerType"].GroupIndex = 0;
            gridView1.BestFitColumns();
            gridView1.GroupFooterShowMode = DevExpress.XtraGrid.Views.Grid.GroupFooterShowMode.VisibleAlways;
            GridGroupSummaryItem item2 = new GridGroupSummaryItem();
            item2.FieldName = "TaxPayerType";
            item2.SummaryType = DevExpress.Data.SummaryItemType.Count;
            gridView1.GroupSummary.Add(item2);
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
                    //ID = Convert.ToInt32(dr["TaxPayerTypeCode"]);
                    setReloadEdit();
                    cboStation.Text = (string)dr["TaxPayerType"];
                    cboStation.SelectedValue = (string)dr["TaxPayerTypeCode"];
                    ID = (string)dr["TaxPayerTypeCode"];
                    bResponse = FillField((string)dr["TaxPayerTypeCode"]);
                }
                else
                {
                    Common.setMessageBox("No Record is seleected", "", 1);
                    bResponse = false;
                }
            }
            return bResponse;
        }

        private void setReloadEdit()
        {
            DataTable dt;

            using (var ds = new System.Data.DataSet())
            {
                //connect.connect.Open();
                using (SqlDataAdapter ada = new SqlDataAdapter("SELECT RevenueCode,Description FROM dbo.tblRevenueType  ORDER BY RevenueCode", Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }
                //connect.connect.Close();
                dt = ds.Tables[0];
                gridControl4.DataSource = dt.DefaultView;
            }
            gridView4.OptionsBehavior.Editable = false;
            //gridView4.Columns["BankID"].Visible = false;
            gridView4.BestFitColumns();

            if (isFirstGrid)
            {
                selection = new GridCheckMarksSelection(gridView4);
                selection.CheckMarkColumn.VisibleIndex = 0;
                isFirstGrid = false;
            }
        }

        private bool FillField(string fieldid)
        {
            bool bResponse = false;
            //load data from the table into the forms for edit
            //DataTable dts = extMethods.LoadData(String.Format("select * from ViewBankBranch where BranchID ='{0}'", fieldid));

            DataTable dts = (new Logic()).getSqlStatement((String.Format("select RevenueCode,RevenueName  from viewRevenueCodeTaxPayerTypeRelation WHERE taxpayertypecode ='{0}'", fieldid))).Tables[0];

            object objRevCode;

            if (dts != null && dts.Rows.Count > 0 && gridView4 != null && gridView4.RowCount > 0)
            {
                bResponse = true;

                for (int i = 0; i < gridView4.RowCount; i++)
                {
                    objRevCode = gridView4.GetRowCellValue(i, "RevenueCode");

                    for (int j = 0; j < dts.Rows.Count; j++)
                    {
                        if ((string)objRevCode == (string)dts.Rows[j][0])
                        {
                            selection.SelectRow(i, true);
                        }
                    }
                }
                //txtAddress.Text = dts.Rows[0]["Address"].ToString();
                //txtBranchCode.Text = dts.Rows[0]["PlatFormCode"].ToString();
                //txtBranchName.Text = dts.Rows[0]["BranchName"].ToString();
                //txtEmail.Text = dts.Rows[0]["Email"].ToString();
                //txtFax.Text = dts.Rows[0]["Fax"].ToString();
                //txtPerson.Text = dts.Rows[0]["ContactPerson"].ToString();
                //txtTelephone.Text = dts.Rows[0]["Telephone"].ToString();
                //cboTown.Text = dts.Rows[0]["TownName"].ToString();
                //cboBankName.Text = dts.Rows[0]["BankName"].ToString();
                //txtCode.Text = dts.Rows[0]["BranchCode"].ToString();

            }
            else
                bResponse = false;

            return bResponse;
        }



    }
}
