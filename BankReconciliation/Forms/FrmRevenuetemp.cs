using BankReconciliation.Class;
using DevExpress.XtraGrid.Views.Grid;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using TaxSmartSuite.Class;

namespace BankReconciliation.Forms
{
    public partial class FrmRevenuetemp : Form
    {
        public static FrmRevenuetemp publicStreetGroup;

        protected TransactionTypeCode iTransType;

        protected bool boolIsUpdate;

        protected string ID;

        bool isFirst = true;

        public FrmRevenuetemp()
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

            txtCode.LostFocus += txtCode_LostFocus;

            txtName.Leave += txtName_Leave;

            txtName.LostFocus += txtName_LostFocus;

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
            string query = String.Format("SELECT count(RevenueCode) AS Count FROM tblRevenueType WHERE RevenueCode='{0}'", (string)txtCode.Text);

            if (String.IsNullOrEmpty(txtCode.Text))
            {
                Common.setEmptyField("Revenue Code ", Program.ApplicationName);
                txtCode.Focus(); return;
            }
            else if (!Logic.isAlphaNumeric((string)txtCode.Text))
            {
                Common.setMessageBox(" Revenue Code can only be in AlphaNumeric values", Program.ApplicationName, 2);

                txtCode.Clear(); txtCode.Focus(); return;
            }
            else if (new Logic().IsRecordExist(query))
            {
                Common.setMessageBox("Revenue Code Already Exit", Program.ApplicationName, 2); txtCode.Clear(); txtCode.Focus(); return;

            }
            else
                txtCode.Text = txtCode.Text.Trim().ToUpper();
        }

        void txtCode_LostFocus(object sender, EventArgs e)
        {
            string query = String.Format("SELECT count(RevenueCode) AS Count FROM tblRevenueType WHERE RevenueCode='{0}'", (string)txtCode.Text);

            if (String.IsNullOrEmpty(txtCode.Text))
            {
                Common.setEmptyField("Revenue Code ", Program.ApplicationName);
                txtCode.Focus(); return;
            }
            else if (!Logic.isAlphaNumeric((string)txtCode.Text))
            {
                Common.setMessageBox(" Revenue Code can only be in AlphaNumeric values", Program.ApplicationName, 2);

                txtCode.Clear(); txtCode.Focus(); return;
            }
            else if (new Logic().IsRecordExist(query))
            {
                Common.setMessageBox("Revenue Code Already Exit", Program.ApplicationName, 2); txtCode.Clear(); txtCode.Focus(); return;

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
                groupControl2.Text = "Disable Record Mode";

                iTransType = TransactionTypeCode.Delete;

                if (MosesClassLibrary.Utilities.Common.AskQuestion("Disable this record will Disable attached record.\nDo you want to continue?", ""))
                {
                    if (string.IsNullOrEmpty(txtCode.Text.Trim()))
                    {
                        Common.setMessageBox("No Record Selected for Deleting", Program.ApplicationName, 3);
                        return;
                    }
                    else
                        deleteRecord(ID, (string)cboNature.SelectedValue);
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

                using (SqlDataAdapter ada = new SqlDataAdapter("SELECT * FROM tblRevenueTemp", Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }

                dt = ds.Tables[0];
                gridControl1.DataSource = dt.DefaultView;
            }
            gridView1.OptionsBehavior.Editable = false;
            gridView1.Columns["AgencyCode"].Visible = false;
            gridView1.BestFitColumns();
        }

        private bool FillField(string fieldid)
        {
            bool bResponse = false;

            DataTable dts = (new Logic()).getSqlStatement((String.Format("select * from tblRevenueTemp where RevenueCode ='{0}'", fieldid))).Tables[0];

            if (dts != null)
            {
                bResponse = true;

                txtCode.Text = dts.Rows[0]["RevenueCode"].ToString();
                txtCode.Enabled = false;
                txtName.Text = dts.Rows[0]["Description"].ToString();
                cboNature.SelectedValue = dts.Rows[0]["AgencyCode"].ToString();

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
                    ID = (string)dr["RevenueCode"];

                    bResponse = FillField((string)dr["RevenueCode"]);
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
                using (SqlDataAdapter ada = new SqlDataAdapter((string)"SELECT  AgencyCode,AgencyName FROM tblAgencyTemp WHERE IsActive=1", Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }

                Dt = ds.Tables[0];
            }

            Common.setComboList(cboNature, Dt, "AgencyCode", "AgencyName");

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

                        string query = String.Format("INSERT INTO [tblRevenueTemp]([AgencyCode],[RevenueCode],[Description],IsActive) VALUES ('{0}','{1}','{2}','{3}');", cboNature.SelectedValue, txtCode.Text.Trim(), txtName.Text.Trim(), 1);

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
                //setReload();

                //Common.setMessageBox("Record added successfully ", Program.ApplicationName, 1);
                //tsbReload.PerformClick(); return;

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
                        //MessageBox.Show(MDIMains.stateCode);
                        //fieldid
                        using (SqlCommand sqlCommand1 = new SqlCommand(String.Format(String.Format("UPDATE [tblRevenueTemp] SET [Description]='{{0}}',[AgencyCode]='{{1}}',IsActive='{{2}}' where  [RevenueCode] ='{0}'", ID), txtName.Text.Trim(), cboNature.SelectedValue, 1), db, transaction))
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
            setReload(); Clear();
            Common.setMessageBox("Records Successfully saved.", Program.ApplicationName, 1);
            //bttnReset.PerformClick();
            tsbReload.PerformClick(); iTransType = TransactionTypeCode.Reload;
            ShowForm();

        }

        private void Clear()
        {
            txtCode.Text = string.Empty; txtName.Text = string.Empty; cboNature.SelectedIndex = -1; txtCode.Enabled = true;

        }

        void deleteRecord(string parmMeter, string parameter2)
        {
            string query = String.Format("UPDATE tblRevenueTemp SET IsActive=0 WHERE agencycode='{0}' AND RevenueCode='{1}'", parameter2, parmMeter);

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
                    setReload(); Clear();
                    Common.setMessageBox("Record Disable Successfully ", Program.ApplicationName, 3);
                    tsbReload.PerformClick(); iTransType = TransactionTypeCode.Reload;
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

    }
}
