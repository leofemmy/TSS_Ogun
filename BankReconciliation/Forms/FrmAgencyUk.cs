using BankReconciliation.Class;
using DevExpress.XtraGrid.Views.Grid;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using TaxSmartSuite.Class;

namespace BankReconciliation.Forms
{
    public partial class FrmAgencyUk : Form
    {

        public static FrmAgencyUk publicStreetGroup;

        protected TransactionTypeCode iTransType;

        protected bool boolIsUpdate;

        protected string ID;

        bool isFirst = true;

        public FrmAgencyUk()
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

            txtName.LostFocus += txtName_LostFocus;

            txtCode.LostFocus += txtCode_LostFocus;

            cboNature.Leave += cboNature_Leave;

            bttnUpdate.Click += Bttn_Click;
        }

        void txtName_LostFocus(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txtName.Text))
            {
                Common.setEmptyField("Agency Name ", Program.ApplicationName);
                txtName.Focus(); return;
            }
            else
            {
                txtName.Text = txtName.Text.Trim().ToUpper();
            }
        }

        void cboNature_Leave(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(cboNature.Text))
            {
                Common.setEmptyField("Agency Nature ", Program.ApplicationName);
                cboNature.Focus(); return;
            }
        }

        void txtName_Leave(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txtName.Text))
            {
                Common.setEmptyField("Agency Name ", Program.ApplicationName);
                txtName.Focus(); return;
            }
            else
            {
                txtName.Text = txtName.Text.Trim().ToUpper();
            }
        }

        void txtCode_Leave(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txtName.Text))
            {
                Common.setEmptyField("Agency Name ", Program.ApplicationName);
                txtName.Focus(); return;
            }
            else
            {
                txtName.Text = txtName.Text.Trim().ToUpper();
            }
        }

        void txtCode_LostFocus(object sender, EventArgs e)
        {
            string query = String.Format("SELECT count(AgencyCode) AS Count FROM dbo.tblAgency WHERE AgencyCode='{0}'", (string)txtCode.Text);

            if (String.IsNullOrEmpty(txtCode.Text))
            {
                Common.setEmptyField("Agency Code ", Program.ApplicationName);
                txtCode.Focus(); return;
            }
            else if (!Logic.isAlphaNumeric((string)txtCode.Text))
            {
                Common.setMessageBox(" Agency Code can only be in AlphaNumeric values", Program.ApplicationName, 2);

                txtCode.Clear(); txtCode.Focus(); return;
            }
            else if (new Logic().IsRecordExist(query))
            {
                Common.setMessageBox("Agency Code Already Exit", Program.ApplicationName, 2);

                txtCode.Clear(); txtCode.Focus(); return;
            }
            else
                txtCode.Text = txtCode.Text.Trim().ToUpper();

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
                groupControl2.Text = "Delete Record Mode";
                iTransType = TransactionTypeCode.Delete;
                if (MosesClassLibrary.Utilities.Common.AskQuestion("Disable this record will Disable attached record.\nDo you want to continue?", ""))
                {
                    if (string.IsNullOrEmpty(ID.ToString()))
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
            //bttnReset.PerformClick();
        }

        void OnFormLoad(object sender, EventArgs e)
        {
            ShowForm(); setDBComboBox();

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

                using (SqlDataAdapter ada = new SqlDataAdapter("SELECT * FROM tblAgencyTemp", Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }

                dt = ds.Tables[0];
                gridControl1.DataSource = dt.DefaultView;
            }
            gridView1.OptionsBehavior.Editable = false;
            gridView1.Columns["NatureID"].Visible = false;
            gridView1.Columns["StateCode"].Visible = false;
            gridView1.BestFitColumns();
        }

        private bool FillField(string fieldid)
        {
            bool bResponse = false;
            //load data from the table into the forms for edit
            //DataTable dts = extMethods.LoadData(String.Format("select * from ViewBankBranchAccount where BankAccountID ='{0}'", fieldid));

            DataTable dts = (new Logic()).getSqlStatement((String.Format("select * from tblAgencyTemp where AgencyCode ='{0}'", fieldid))).Tables[0];

            if (dts != null)
            {
                bResponse = true;

                txtCode.Text = dts.Rows[0]["AgencyCode"].ToString();
                txtCode.Enabled = false;
                txtName.Text = dts.Rows[0]["AgencyName"].ToString();
                cboNature.SelectedValue = dts.Rows[0]["NatureID"].ToString();

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
                    ID = (string)dr["AgencyCode"];

                    bResponse = FillField((string)dr["AgencyCode"]);
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

        public void setDBComboBox()
        {
            DataTable Dt;

            //connect.connect.Close();

            using (var ds = new System.Data.DataSet())
            {
                //connect.connect.Open();
                using (SqlDataAdapter ada = new SqlDataAdapter((string)"SELECT NatureID,Description FROM dbo.tblAgencyNature", Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }

                Dt = ds.Tables[0];
            }

            Common.setComboList(cboNature, Dt, "NatureID", "Description");

            cboNature.SelectedIndex = -1;


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

                        string query = String.Format("INSERT INTO [tblAgencyTemp]([AgencyCode],[AgencyName],[StateCode],[NatureID],IsActive) VALUES ('{0}','{1}','{2}','{3}','{4}');", txtCode.Text.Trim(), txtName.Text.Trim(), Program.stateCode, cboNature.SelectedValue, 1);

                        using (SqlCommand sqlCommand1 = new SqlCommand(query, db, transaction))
                        {
                            sqlCommand1.ExecuteNonQuery();
                        }

                        transaction.Commit();
                    }
                    catch (SqlException sqlError)
                    {
                        transaction.Rollback();
                        Tripous.Sys.ErrorBox(sqlError);
                    }
                    db.Close();
                }

                setReload();

                Common.setMessageBox("Record has been successfully added", Program.ApplicationName, 1);
                Clear();
                //tsbReload.PerformClick(); 
                iTransType = TransactionTypeCode.Reload;
                ShowForm();
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
                        using (SqlCommand sqlCommand1 = new SqlCommand(String.Format(String.Format("UPDATE [tblAgencyTemp] SET [AgencyName]='{{0}}',[StateCode]='{{1}}',[NatureID]='{{2}}',IsActive='{{3}}' where  AgencyCode ='{0}'", ID), txtName.Text.Trim(), Program.stateCode, cboNature.SelectedValue, 1), db, transaction))
                        {
                            sqlCommand1.ExecuteNonQuery();
                        }

                        transaction.Commit();
                    }
                    catch (SqlException sqlError)
                    {
                        transaction.Rollback();
                        Tripous.Sys.ErrorBox(sqlError);
                    }
                    db.Close();
                }
                setReload();
                Common.setMessageBox("Records Successfully saved.", Program.ApplicationName, 1);

                Clear();
                tsbReload.PerformClick();
                iTransType = TransactionTypeCode.Reload;
                ShowForm();
            }

        }

        void deleteRecord(string parmMeter)
        {
            string query = String.Format("UPDATE tblAgencyTemp SET IsActive=0 WHERE agencycode='{0}'", parmMeter);

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

                    Common.setMessageBox("Record Disable Successfully ", Program.ApplicationName, 3);
                    Clear(); setReload();
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
            txtCode.Text = string.Empty; txtName.Text = string.Empty; cboNature.SelectedIndex = -1;
            txtCode.Enabled = true;
        }


    }
}
