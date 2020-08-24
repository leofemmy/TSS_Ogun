using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using Control_Panel.Class;
using TaxSmartSuite.Class;
using System.Globalization;
using DevExpress.XtraGrid.Views.Grid;

namespace Control_Panel.Forms
{
    public partial class FrmTreasury : Form
    {
        public static FrmTreasury publicStreetGroup;

        string modulesAccess, modulesAccess1;

        protected TransactionTypeCode iTransType;

        protected bool boolIsUpdate;

        protected string ID;

        bool isFirst = true;

        public FrmTreasury()
        {
            InitializeComponent();

            publicStreetGroup = this;

            setImages();

            ToolStripEvent();

            iTransType = TransactionTypeCode.Null;

            Load += OnFormLoad;

            bttnCancel.Click += Bttn_Click;

            bttnUpdate.Click += Bttn_Click;

            gridView1.DoubleClick += gridView1_DoubleClick;

            OnFormLoad(null, null);
        }

        void gridView1_DoubleClick(object sender, EventArgs e)
        {
            tsbEdit.PerformClick();
        }

        public void setDBComboBox()
        {
            DataTable Dt;
            using (var ds = new System.Data.DataSet())
            {
                using (SqlDataAdapter ada = new SqlDataAdapter((string)"SELECT BankName,BankShortCode FROM dbo.tblBank", Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }

                Dt = ds.Tables[0];
            }

            Common.setComboList(cboBank, Dt, "BankShortCode", "BankName");

            cboBank.SelectedIndex = -1;

        }

        private void setImages()
        {
            tsbNew.Image = MDIMain.publicMDIParent.i16x16.Images[5];
            tsbEdit.Image = MDIMain.publicMDIParent.i16x16.Images[6];
            tsbDelete.Image = MDIMain.publicMDIParent.i16x16.Images[7];
            tsbReload.Image = MDIMain.publicMDIParent.i16x16.Images[0];
            tsbClose.Image = MDIMain.publicMDIParent.i16x16.Images[11];

            bttnCancel.Image = MDIMain.publicMDIParent.i32x32.Images[9];
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
                if (MosesClassLibrary.Utilities.Common.AskQuestion("Deleting this record will delete attached record.\nDo you want to continue?", ""))
                {
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

        void setDBComboBoxAcct(string parameter)
        {
            DataTable Dt;

            using (var ds = new System.Data.DataSet())
            {

                string query = String.Format("select AccountNumber,BankAccountID  from ViewBankBranchAccount where BankName = '{0}'", parameter);

                using (SqlDataAdapter ada = new SqlDataAdapter(query, Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }

                Dt = ds.Tables[0];
            }

            Common.setComboList(cboAcct, Dt, "BankAccountID", "AccountNumber");

            cboAcct.SelectedIndex = -1;
        }

        void OnFormLoad(object sender, EventArgs e)
        {
            ShowForm();

            setDBComboBox();

            isFirst = false;

            setReload();

            cboBank.KeyPress += cboBank_KeyPress;

            cboBank.SelectedIndexChanged += cboBank_SelectedIndexChanged;

            cboBank_SelectedIndexChanged(null, null);

            cboBank.LostFocus += cboBank_LostFocus;

            txtCashOffice.LostFocus += txtCashOffice_LostFocus;
        }

        void txtCashOffice_LostFocus(object sender, EventArgs e)
        {
            txtCashOffice.Text = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(txtCashOffice.Text);
        }

        void cboBank_LostFocus(object sender, EventArgs e)
        {
            if (cboBank.SelectedValue != null && !isFirst)
            {
                setDBComboBoxAcct(cboBank.Text);
                cboAcct.SelectedIndex = -1;
            }
        }

        void cboBank_KeyPress(object sender, KeyPressEventArgs e)
        {
            Methods.AutoComplete(cboBank, e, true);
        }

        void cboBank_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (cboBank.SelectedValue != null && !isFirst)
            {
                setDBComboBoxAcct(cboBank.Text);
                cboAcct.SelectedIndex = -1;
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

        void Bttn_Click(object sender, EventArgs e)
        {
            if (sender == bttnCancel)
            {
                Clear();
                tsbReload.PerformClick();
            }
            else if (sender == bttnUpdate)
            {
                UpdateRecord();
            }
        }

        void Clear()
        {
            txtCashOffice.Clear(); cboBank.SelectedIndex = -1; cboAcct.SelectedIndex = -1;
        }

        private void setReload()
        {

            DataTable dt;

            using (var ds = new System.Data.DataSet())
            {
                //connect.connect.Open();
                using (SqlDataAdapter ada = new SqlDataAdapter("select * from ViewBankTreasury", Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }
                //connect.connect.Close();
                dt = ds.Tables[0];
                gridControl1.DataSource = dt.DefaultView;
            }
            gridView1.OptionsBehavior.Editable = false;
            gridView1.Columns["TreasuryCode"].Visible = false;
            //gridView1.Columns["BranchName"].Visible = false;
            gridView1.BestFitColumns();
        }

        private void UpdateRecord()
        {
            try
            {
                if (txtCashOffice.Text == "")
                {
                    Common.setEmptyField(" Treasury Cash Offices ", Program.ApplicationName);
                    txtCashOffice.Focus(); return;
                }
                else if (cboBank.Text == "")
                {
                    Common.setEmptyField("Bank Name", Program.ApplicationName);
                    cboBank.Focus(); return;
                }
                else if (cboAcct.Text == "")
                {
                    Common.setEmptyField("Account Number ", Program.ApplicationName);
                    cboAcct.Focus(); return;
                }
                else
                {

                    //check form status either new or edit
                    if (!boolIsUpdate)
                    {
                        using (SqlConnection db = new SqlConnection(Logic.ConnectionString))
                        {
                            SqlTransaction transaction;

                            db.Open();

                            transaction = db.BeginTransaction();
                            try
                            {

                                using (SqlCommand sqlCommand1 = new SqlCommand(String.Format("INSERT INTO [tblTreasury]([TreasuryName],[AcctNumber],[BankShortCode],[BankAccountID]) VALUES ('{0}','{1}','{2}','{3}');", txtCashOffice.Text.Trim(), cboAcct.Text.Trim(), cboBank.SelectedValue, cboAcct.SelectedValue), db, transaction))
                                {
                                    sqlCommand1.ExecuteNonQuery();
                                }

                                transaction.Commit();
                            }
                            catch (SqlException sqlError)
                            {
                                transaction.Rollback();
                                Tripous.Sys.ErrorBox(sqlError); return;
                            }
                            db.Close();
                        }

                        setReload();

                        Common.setMessageBox("Record has been successfully added", Program.ApplicationName, 1);
                        if (MessageBox.Show("Do you want to add another record?", Program.ApplicationName, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
                        {
                            //bttnCancel.PerformClick();
                            tsbReload.PerformClick();

                        }
                        else
                        {
                            setReload();
                            Clear(); txtCashOffice.Focus();
                        }
                        //}
                    }
                    else
                    {
                        //update the records

                        using (SqlConnection db = new SqlConnection(Logic.ConnectionString))
                        {
                            SqlTransaction transaction;

                            db.Open();

                            transaction = db.BeginTransaction();
                            try
                            {
                                using (SqlCommand sqlCommand1 = new SqlCommand(String.Format(String.Format("UPDATE [tblTreasury] SET [TreasuryName]='{{0}}',[AcctNumber]='{{1}}',[BankShortCode]='{{2}}',[BankAccountID]='{{3}}' where  TreasuryID ='{0}'", ID), txtCashOffice.Text.Trim(), cboAcct.Text.Trim(), cboBank.SelectedValue, cboAcct.SelectedValue), db, transaction))
                                {
                                    sqlCommand1.ExecuteNonQuery();
                                }

                                transaction.Commit();
                            }
                            catch (SqlException sqlError)
                            {
                                transaction.Rollback();
                                Tripous.Sys.ErrorBox(sqlError); return;
                            }
                            db.Close();
                        }

                        setReload();
                        Common.setMessageBox("Changes in record has been successfully Update.", Program.ApplicationName, 1);

                        Clear();

                        tsbReload.PerformClick();

                    }
                }
            }
            catch (Exception ex)
            {
                Tripous.Sys.ErrorBox(ex);
            }

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
                    ID = dr["TreasuryCode"].ToString();
                    bResponse = FillField(dr["TreasuryCode"].ToString());
                }
                else
                {
                    Common.setMessageBox("No Record is seleected", "", 1);
                    bResponse = false;
                }
            }
            return bResponse;
        }

        private bool FillField(string fieldid)
        {
            bool bResponse = false;
            //load data from the table into the forms for edit

            DataTable dts = (new Logic()).getSqlStatement((String.Format("select * from ViewBankTreasury where TreasuryCode ='{0}'", fieldid))).Tables[0];

            if (dts != null)
            {
                bResponse = true;

                txtCashOffice.Text = dts.Rows[0]["Cash Office"].ToString();
                cboBank.Text = dts.Rows[0]["Bank"].ToString();
                cboAcct.Text = dts.Rows[0]["Account Number"].ToString();
                //cboBranch.Text = dts.Rows[0]["BranchName"].ToString();

            }
            else
                bResponse = false;

            return bResponse;
        }

        public void RefreshForm()
        {
            NavBars.ToolStripEnableDisableControls(toolStrip, Tag as String);
        }


    }
}
