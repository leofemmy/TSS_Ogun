using BankReconciliation.Class;
using DevExpress.XtraGrid.Views.Grid;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using TaxSmartSuite.Class;

namespace BankReconciliation.Forms
{
    public partial class FrmZonalTemp : Form
    {
        public static FrmZonalTemp publicStreetGroup;

        protected TransactionTypeCode iTransType;

        protected bool boolIsUpdate;

        protected string ID;

        bool isFirst = true;

        public FrmZonalTemp()
        {
            InitializeComponent();

            publicStreetGroup = this;

            setImages();

            ToolStripEvent();

            iTransType = TransactionTypeCode.Null;

            Load += OnFormLoad;

            OnFormLoad(null, null);

            gridView1.DoubleClick += gridView1_DoubleClick;

            txtCode.Leave += txtCode_Leave;

            txtName.Leave += txtName_Leave;

            //cboNature.Leave += cboNature_Leave;

            bttnUpdate.Click += Bttn_Click;
        }

        void txtName_Leave(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txtName.Text))
            {
                Common.setEmptyField("Revenue Office Name ", Program.ApplicationName);
                txtName.Focus(); return;
            }
            else
            {
                txtName.Text = txtName.Text.Trim().ToUpper();
            }
        }

        void txtCode_Leave(object sender, EventArgs e)
        {
            string query = String.Format("SELECT count(OfficeCode) AS Count FROM tblRevenueOffice WHERE OfficeCode='{0}'", (string)txtCode.Text);

            if (String.IsNullOrEmpty(txtCode.Text))
            {
                Common.setEmptyField("Revenue Office Code ", Program.ApplicationName);
                txtCode.Focus(); return;
            }
            else if (!Logic.isAlphaNumeric((string)txtCode.Text))
            {
                Common.setMessageBox(" Revenue Office  can only be in AlphaNumeric values", Program.ApplicationName, 2);

                txtCode.Clear(); txtCode.Focus(); return;
            }
            else if (new Logic().IsRecordExist(query))
            {
                Common.setMessageBox("Revenue Office  Already Exit", Program.ApplicationName, 2);

                txtCode.Clear(); txtCode.Focus(); return;

            }
        }

        private void setImages()
        {
            tsbNew.Image = MDIMains.publicMDIParent.i16x16.Images[5];
            tsbEdit.Image = MDIMains.publicMDIParent.i16x16.Images[6];
            tsbDelete.Image = MDIMains.publicMDIParent.i16x16.Images[7];
            tsbReload.Image = MDIMains.publicMDIParent.i16x16.Images[0];
            tsbClose.Image = MDIMains.publicMDIParent.i16x16.Images[11];

            bttnCancel.Image = MDIMains.publicMDIParent.i32x32.Images[9];
            //bttnReset.Image = MDIMain.publicMDIParent.i32x32.Images[8];
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
                ShowForm();
                boolIsUpdate = false;
            }
            else if (sender == tsbEdit)
            {
                groupControl2.Text = "Edit Record Mode";
                iTransType = TransactionTypeCode.Edit;
                if (EditRecordMode())
                {
                    ShowForm();
                    boolIsUpdate = true;
                }
            }
            else if (sender == tsbDelete)
            {
                groupControl2.Text = "Disable Record Mode";

                iTransType = TransactionTypeCode.Delete;

                if (MosesClassLibrary.Utilities.Common.AskQuestion("Disable this record will Disable attached record.\nDo you want to continue?", ""))
                {
                    if (string.IsNullOrEmpty(ID))
                    {
                        Common.setMessageBox("No Record Selected for Deleting", Program.ApplicationName, 3);
                        return;
                    }
                    else
                        deleteRecord(ID);
                }
                else
                    tsbReload.PerformClick();
                boolIsUpdate = false;
            }
            else if (sender == tsbReload)
            {
                iTransType = TransactionTypeCode.Reload;
                ShowForm();
            }

        }

        void OnFormLoad(object sender, EventArgs e)
        {
            ShowForm();
            //setDBComboBox();

            isFirst = false; setReload();

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

        void gridView1_DoubleClick(object sender, EventArgs e)
        {
            tsbEdit.PerformClick();
        }

        private void setReload()
        {
            //connect.connect.Close();
            DataTable dt;

            using (var ds = new System.Data.DataSet())
            {

                using (SqlDataAdapter ada = new SqlDataAdapter("SELECT * FROM [tblZonalRevenueOfficetemp]", Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }

                dt = ds.Tables[0];
                gridControl1.DataSource = dt.DefaultView;
            }
            gridView1.OptionsBehavior.Editable = false;
            //gridView1.Columns["AgencyCode"].Visible = false;
            gridView1.BestFitColumns();
        }

        private bool FillField(string fieldid)
        {
            bool bResponse = false;

            DataTable dts = (new Logic()).getSqlStatement((String.Format("select * from [tblZonalRevenueOfficetemp] where OfficeCode ='{0}'", fieldid))).Tables[0];

            if (dts != null)
            {
                bResponse = true;

                txtCode.Text = dts.Rows[0]["OfficeCode"].ToString();
                txtCode.Enabled = false;
                txtName.Text = dts.Rows[0]["OfficeName"].ToString();
                txtOfficeAddress.Text = dts.Rows[0]["OfficeAddress"].ToString();
                //cboNature.SelectedValue = dts.Rows[0]["AgencyCode"].ToString();

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
                    ID = (string)dr["OfficeCode"];

                    bResponse = FillField((string)dr["OfficeCode"]);
                }
                else
                {
                    Common.setMessageBox("No Record is seleected", "", 1);

                    bResponse = false;
                }
            }
            return bResponse;
        }

        public void RefreshForm()
        {
            NavBars.ToolStripEnableDisableControls(toolStrip, Tag as String);
        }

        void Bttn_Click(object sender, EventArgs e)
        {
            if (sender == bttnUpdate)
            {
                UpdateRecord();
            }
            else if (sender == bttnCancel)
            {
                tsbReload.PerformClick();
            }
        }

        void UpdateRecord()
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
                        string query = String.Format("INSERT INTO [tblZonalRevenueOfficetemp]([OfficeCode],[OfficeName],[OfficeAddress],IsActive) VALUES ('{0}','{1}','{2}','{3}');", txtCode.Text.Trim(), txtName.Text.Trim(), txtOfficeAddress.Text.Trim(), 1);

                        using (SqlCommand sqlCommand1 = new SqlCommand(query, db, transaction))
                        {
                            sqlCommand1.ExecuteNonQuery();
                        }

                        transaction.Commit();
                    }
                    catch (SqlException sqlError)
                    {
                        transaction.Rollback(); Tripous.Sys.ErrorBox(sqlError); return;
                    }
                    db.Close();
                }
            }
            else
            {
                using (SqlConnection db = new SqlConnection(Logic.ConnectionString))
                {
                    SqlTransaction transaction;

                    db.Open();

                    transaction = db.BeginTransaction();
                    try
                    {
                        using (SqlCommand sqlCommand1 = new SqlCommand(String.Format(String.Format("UPDATE [tblZonalRevenueOfficetemp] SET [OfficeName]='{{0}}',[OfficeAddress]='{{1}}',IsActive='{{2}}' where  [OfficeCode] ='{0}'", ID), txtName.Text.Trim(), txtOfficeAddress.Text.Trim(), 1), db, transaction))
                        {
                            sqlCommand1.ExecuteNonQuery();
                        }

                        transaction.Commit();
                    }
                    catch (SqlException sqlError)
                    {
                        transaction.Rollback(); Tripous.Sys.ErrorBox(sqlError); return;
                    }
                    db.Close();
                }
            }
            Common.setMessageBox("Records Update Successfully", Program.ApplicationName, 1);

            //tsbReload.PerformClick();
            setReload(); Clear();
            iTransType = TransactionTypeCode.Reload;
            ShowForm();
        }

        void deleteRecord(string parameter2)
        {
            string query = String.Format("UPDATE tblZonalRevenueOfficetemp SET IsActive = 0 WHERE OfficeCode='{0}'", parameter2);

            using (SqlConnection db = new SqlConnection(Logic.ConnectionString))
            {
                SqlTransaction transaction;

                db.Open();

                transaction = db.BeginTransaction();

                try
                {
                    using (SqlCommand sqlCommand1 = new SqlCommand(query, db, transaction))
                    {
                        sqlCommand1.ExecuteNonQuery();
                    }

                    transaction.Commit();

                    Common.setMessageBox("Record Deleted Successfully ", Program.ApplicationName, 3);
                    setReload(); Clear();
                    iTransType = TransactionTypeCode.Reload;
                    ShowForm();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Tripous.Sys.ErrorBox(ex);
                }

                db.Close();
            }

        }

        private void Clear()
        {
            txtCode.Text = string.Empty; txtName.Text = string.Empty; txtOfficeAddress.Text = string.Empty;


        }

    }
}
