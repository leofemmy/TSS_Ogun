using Collection.Classess;
using DevExpress.XtraGrid.Selection;
using DevExpress.XtraGrid.Views.Grid;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using TaxSmartSuite.Class;

namespace Collection.Forms
{
    public partial class FrmReceiptCentreMap : Form
    {
        private DataTable Dt;

        GridCheckMarksSelection selection;

        public static FrmReceiptCentreMap publicStreetGroup;

        public static FrmReceiptCentreMap publicInstance;

        protected TransactionTypeCode iTransType;

        protected bool boolIsUpdate;

        protected string ID, stationcode;

        bool isFirstGrid = true;

        bool isFirst = true;

        string querys = string.Empty;

        string lol = string.Empty;

        object objRevCode;

        public FrmReceiptCentreMap()
        {
            InitializeComponent();

            publicStreetGroup = this;

            setImages();

            ToolStripEvent();

            Load += OnFormLoad;

            iTransType = TransactionTypeCode.New;

            OnFormLoad(null, null);

            bttnUpdate.Click += bttnUpdate_Click;

            gridView1.FocusedRowChanged += gridView1_FocusedRowChanged;

            radioGroup1.SelectedIndexChanged += radioGroup1_SelectedIndexChanged;

        }

        void radioGroup1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (radioGroup1.EditValue == null) return;

            if ((Int32)this.radioGroup1.EditValue == 1)//Revenue Code
            {
                groupBox2.Text = "Check Revenue Code to Map With Printing Centre";

                querys = "SELECT RevenueCode,AgencyName FROM dbo.tblRevenueCodeAgency ORDER BY AgencyCode";
            }
            else if ((Int32)this.radioGroup1.EditValue == 2)//Bank Branch
            {

                groupBox2.Text = "Check Bank Branch to Map With Printing Centre";

                querys = "SELECT BranchCode,BranchName,BankName FROM dbo.ViewBankBranch ORDER BY PlatFormCode";
            }

            //call setreload

            isFirstGrid = true;

            setReload(querys);

        }

        void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {

            if (radioGroup1.SelectedIndex == -1)
            {
                Common.setMessageBox("Select Mapping Option", Program.ApplicationName, 2);
                return;
            }
            else
            {
                selection.ClearSelection();

                GridView view = (GridView)gridControl1.FocusedView;

                if (view != null)
                {
                    DataRow dr = view.GetDataRow(view.FocusedRowHandle);
                    if (dr != null)
                    {
                        ID = dr["CentreCode"].ToString();
                        stationcode = dr["StationCode"].ToString();
                        cboCentre.Text = dr["CentreName"].ToString();
                        cboStation.Text = dr["StationName"].ToString();

                        DataTable dts = (new Logic()).getSqlStatement((String.Format("SELECT RevenueCode FROM tblPrintingCenterMap where CentreCode ='{0}' and StationCode ='{1}'", ID, stationcode))).Tables[0];
                        if (dts != null)
                        {
                            BindData(dts);
                            boolIsUpdate = true;

                        }
                    }
                }
            }

        }

        private void setImages()
        {
            tsbNew.Image = MDIMain.publicMDIParent.i16x16.Images[5];
            tsbEdit.Image = MDIMain.publicMDIParent.i16x16.Images[6];
            tsbDelete.Image = MDIMain.publicMDIParent.i16x16.Images[7];
            tsbReload.Image = MDIMain.publicMDIParent.i16x16.Images[0];
            tsbClose.Image = MDIMain.publicMDIParent.i16x16.Images[11];

            //bttnCancel.Image = MDIMain.publicMDIParent.i32x32.Images[9];
            //bttnReset.Image = MDIMain.publicMDIParent.i32x32.Images[8];
            bttnUpdate.Image = MDIMain.publicMDIParent.i32x32.Images[7];

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
                ShowForm();
                //cboNature.SelectedIndex = -1;
                boolIsUpdate = false;
            }
            else if (sender == tsbEdit)
            {
                //groupControl2.Text = "Edit Record Mode";
                iTransType = TransactionTypeCode.Edit;
                //if (EditRecordMode())
                //{
                ShowForm();
                boolIsUpdate = true;
                //BindData();
                //}
            }
            else if (sender == tsbDelete)
            {
                ////groupControl2.Text = "Delete Record Mode";
                //iTransType = TransactionTypeCode.Delete;
                //if (MosesClassLibrary.Utilities.Common.AskQuestion("Deleting this record will delete attached record.\nDo you want to continue?", ""))
                //{
                //}
                //else
                //    tsbReload.PerformClick();
                //boolIsUpdate = false;
            }
            else if (sender == tsbReload)
            {
                iTransType = TransactionTypeCode.Reload;
                //ShowForm();
            }
            //bttnReset.PerformClick();
        }

        void OnFormLoad(object sender, EventArgs e)
        {
            ShowForm();

            setDBComboBox();

            //setReload();

            isFirst = false;

            cboStation.SelectedIndexChanged += cboStation_SelectedIndexChanged;

            cboStation_SelectedIndexChanged(null, null);

            setReload1();

            gridControl4.DataSource = null;


        }

        public void setDBComboBox()
        {
            DataTable Dt;

            using (var ds = new System.Data.DataSet())
            {
                string query = "SELECT StationCode,StationName FROM dbo.tblStation";

                using (SqlDataAdapter ada = new SqlDataAdapter(query, Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }

                Dt = ds.Tables[0];
            }

            Common.setComboList(cboStation, Dt, "StationCode", "StationName");

            cboStation.SelectedIndex = -1;

        }

        private void setReload(string querys)
        {
            DataTable dt;

            //gridControl4.DataSource = null;

            //gridView4.Columns.Clear();


            using (var ds = new System.Data.DataSet())
            {
                using (SqlDataAdapter ada = new SqlDataAdapter(querys, Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }
                dt = ds.Tables[0];

                gridControl4.DataSource = ds.Tables[0];
            }
            gridView4.OptionsBehavior.Editable = false;

            gridView4.BestFitColumns();

            if (isFirstGrid)
            {
                selection = new GridCheckMarksSelection(gridView4);
                selection.CheckMarkColumn.VisibleIndex = 0;
                isFirstGrid = false;
            }
        }

        void setDBComboBoxCentre(string parameter)
        {
            DataTable Dt;

            using (var ds = new System.Data.DataSet())
            {
                //string query = "SELECT CentreCode,CentreName FROM dbo.tblReceiptPrintingCentre WHERE StationCode";
                string query = String.Format("SELECT CentreCode,CentreName FROM tblReceiptPrintingCentre where StationCode= '{0}' ", parameter);
                using (SqlDataAdapter ada = new SqlDataAdapter(query, Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }

                Dt = ds.Tables[0];
            }

            Common.setComboList(cboCentre, Dt, "CentreCode", "CentreName");

            //cboCentre.SelectedIndex = -1;
        }

        void cboStation_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboStation.SelectedValue != null && !isFirst)
            {
                setDBComboBoxCentre(cboStation.SelectedValue.ToString());
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

        void bttnUpdate_Click(object sender, EventArgs e)
        {
            if (selection.SelectedCount < 1)
            {
                Common.setMessageBox("Please, select revenue code  to Map", Program.ApplicationName, 3); return;
            }
            else if (cboStation.SelectedIndex < 0)
            {
                Common.setMessageBox("Please, select Station to Map", Program.ApplicationName, 3); cboStation.Focus(); return;
            }
            else if (cboCentre.SelectedIndex < 0)
            {
                Common.setMessageBox("Please, select Printing Centre to Map", Program.ApplicationName, 3); cboCentre.Focus(); return;
            }
            else
            {

                if (!boolIsUpdate)
                {
                    using (SqlConnection db = new SqlConnection(Logic.ConnectionString))
                    {
                        SqlTransaction transaction;
                        SqlCommand sqlCommand1;

                        db.Open();

                        transaction = db.BeginTransaction();

                        try
                        {

                            for (int i = 0; i < selection.SelectedCount; i++)
                            {
                                if ((Int32)this.radioGroup1.EditValue == 1)//Revenue Code
                                {
                                    lol = ((selection.GetSelectedRow(i) as DataRowView)["RevenueCode"].ToString());
                                }
                                else if ((Int32)this.radioGroup1.EditValue == 2)//Bank Branch
                                {
                                    lol = ((selection.GetSelectedRow(i) as DataRowView)["BranchCode"].ToString());
                                }


                                string query = String.Format("INSERT INTO [tblPrintingCenterMap]([CentreCode],[CentreName],[StationName],[StationCode],[RevenueCode]) VALUES ('{0}','{1}','{2}','{3}','{4}')", cboCentre.SelectedValue.ToString(), cboCentre.Text.Trim(), cboStation.Text, cboStation.SelectedValue.ToString(), lol);


                                sqlCommand1 = new SqlCommand(query, db, transaction);
                                sqlCommand1.ExecuteNonQuery();

                            }


                            //call report for Print
                            transaction.Commit();
                            //ReceiptCall();
                        }
                        catch (SqlException sqlError)
                        {
                            transaction.Rollback();
                            Tripous.Sys.ErrorBox(sqlError);
                            return;
                        }
                        db.Close();

                    }
                }
                else
                {
                    using (SqlConnection db = new SqlConnection(Logic.ConnectionString))
                    {
                        SqlTransaction transaction;

                        SqlCommand sqlCommand1;

                        db.Open();

                        transaction = db.BeginTransaction();

                        try
                        {
                            string query = String.Format("DELETE FROM tblPrintingCenterMap where CentreCode ='{0}' and StationCode ='{1}'", ID, stationcode);

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
                    using (SqlConnection dbs = new SqlConnection(Logic.ConnectionString))
                    {
                        SqlTransaction transactions;
                        SqlCommand sqlCommand2;

                        dbs.Open();

                        transactions = dbs.BeginTransaction();

                        try
                        {

                            for (int i = 0; i < selection.SelectedCount; i++)
                            {
                                if ((Int32)this.radioGroup1.EditValue == 1)//Revenue Code
                                {
                                    lol = ((selection.GetSelectedRow(i) as DataRowView)["RevenueCode"].ToString());
                                }
                                else if ((Int32)this.radioGroup1.EditValue == 2)//Bank Branch
                                {
                                    lol = ((selection.GetSelectedRow(i) as DataRowView)["BranchCode"].ToString());
                                }
                                string query = String.Format("INSERT INTO [tblPrintingCenterMap]([CentreCode],[CentreName],[StationName],[StationCode],[RevenueCode]) VALUES ('{0}','{1}','{2}','{3}','{4}')", cboCentre.SelectedValue.ToString(), cboCentre.Text.Trim(), cboStation.Text, cboStation.SelectedValue.ToString(), lol);


                                sqlCommand2 = new SqlCommand(query, dbs, transactions);
                                sqlCommand2.ExecuteNonQuery();

                            }


                            //call report for Print
                            transactions.Commit();
                            //ReceiptCall();
                        }
                        catch (SqlException sqlError)
                        {
                            transactions.Rollback();
                            Tripous.Sys.ErrorBox(sqlError);
                            return;
                        }
                        dbs.Close();

                    }
                }
            }


            setReload1();
            setReload(querys);

            cboStation.SelectedIndex = -1;

            cboCentre.SelectedValue = -1;

            Common.setMessageBox(" Transaction Completed Successfully ", Program.ApplicationName, 1);
            selection.ClearSelection();

        }

        public void RefreshForm()
        {
            NavBars.ToolStripEnableDisableControls(toolStrip, Tag as String);
        }

        private void setReload1()
        {
            DataTable dt;

            using (var ds = new System.Data.DataSet())
            {
                //connect.connect.Open();
                using (SqlDataAdapter ada = new SqlDataAdapter("SELECT DISTINCT CentreCode,CentreName,StationCode,StationName FROM tblPrintingCenterMap ", Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }
                dt = ds.Tables[0];
                gridControl1.DataSource = dt.DefaultView;
            }
            gridView1.OptionsBehavior.Editable = false;
            //gridView1.Columns["ReceiveReceiptID"].Visible = false;
            gridView1.BestFitColumns();
        }

        void BindData(DataTable dt)
        {
            if (dt != null && dt.Rows.Count > 0 && gridView4 != null && gridView4.RowCount > 0)
            {
                for (int i = 0; i < gridView4.RowCount; i++)
                {
                    if ((Int32)this.radioGroup1.EditValue == 1)//Revenue Code
                    {
                        objRevCode = gridView4.GetRowCellValue(i, "RevenueCode");
                    }
                    else if ((Int32)this.radioGroup1.EditValue == 2)//Bank Branch
                    {
                        objRevCode = gridView4.GetRowCellValue(i, "BranchCode");
                    }

                    if (objRevCode != null)
                    {
                        for (int j = 0; j < dt.Rows.Count; j++)
                        {
                            string revCode = Convert.ToString(objRevCode);
                            string revCode1 = Convert.ToString(dt.Rows[j][0]);
                            if (revCode.Trim() == revCode1.Trim())
                            //if (object.Equals(objRevCode, dt.Rows[0]))
                            {
                                //gridView4.SelectRow(i);
                                selection.SelectRow(i, true);
                            }
                        }
                    }
                }
            }
        }

    }
}
