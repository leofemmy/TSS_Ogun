using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TaxSmartSuite.Class;
using System.Data.SqlClient;
using DevExpress.XtraGrid.Views.Grid;
using Control_Panel.Class;

namespace Control_Panel.Forms
{
    public partial class FrmAccountBank : Form
    {

        //DBConnection connect = new DBConnection();

        //Methods extMethods = new Methods();

        public static FrmAccountBank publicStreetGroup;

        protected TransactionTypeCode iTransType;

        protected bool boolIsUpdate;

        protected int ID;

        bool isFirst = true;

        public FrmAccountBank()
        {
            InitializeComponent();

            //connect.ConnectionString();

            publicStreetGroup = this;

            setImages();

            ToolStripEvent();

            iTransType = TransactionTypeCode.Null;

            Load += OnFormLoad;

            gridView1.DoubleClick += gridView1_DoubleClick;

            bttnCancel.Click += Bttn_Click;

            //bttnReset.Click += Bttn_Click;

            bttnUpdate.Click += Bttn_Click;

            OnFormLoad(null, null);
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

        void OnFormLoad(object sender, EventArgs e)
        {
            ShowForm();
            setDBComboBox();
            //setDBComboBoxTn();
            isFirst = false;
            setReload();
            cboBank.KeyPress += cboBank_KeyPress;
            cboBranch.KeyPress += cboBranch_KeyPress;
            cboBank.SelectedIndexChanged += cboBank_SelectedIndexChanged;
            cboBank_SelectedIndexChanged(null, null);

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
            //System.Data.DataSet ds;
            using (var ds = new System.Data.DataSet())
            {
                //connect.connect.Open();
                using (SqlDataAdapter ada = new SqlDataAdapter("select * from ViewBankBranchAccount", Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }
                //connect.connect.Close();
                dt = ds.Tables[0];
                gridControl1.DataSource = dt.DefaultView;
            }
            gridView1.OptionsBehavior.Editable = false;
            gridView1.Columns["BankAccountID"].Visible = false;
            gridView1.Columns["BranchName"].Visible = false;
            gridView1.BestFitColumns();
        }

        void Bttn_Click(object sender, EventArgs e)
        {
            //if (sender == bttnCancel)
            //{
            //    tsbReload.PerformClick();
            //}
            //else if (sender == bttnReset)
            //{
            //    if (!boolIsUpdate)
            //        Clear();
            //    else
            //        FillField(ID);
            //    //setReload();
            //}
            //else
            if (sender == bttnUpdate)
            {
                UpdateRecord();
            }
        }

        public void setDBComboBox()
        {
            DataTable Dt;

            //connect.connect.Close();

            using (var ds = new System.Data.DataSet())
            {
                //connect.connect.Open();
                string query = "select *  from tblBank";
                using (SqlDataAdapter ada = new SqlDataAdapter(query, Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }

                Dt = ds.Tables[0];
            }

            Common.setComboList(cboBank, Dt, "BankShortCode", "BankName");

            cboBank.SelectedIndex = -1;


        }

        private bool FillField(int fieldid)
        {
            bool bResponse = false;
            //load data from the table into the forms for edit
            //DataTable dts = extMethods.LoadData(String.Format("select * from ViewBankBranchAccount where BankAccountID ='{0}'", fieldid));

            DataTable dts = (new Logic()).getSqlStatement((String.Format("select * from ViewBankBranchAccount where BankAccountID ='{0}'", fieldid))).Tables[0];

            if (dts != null)
            {
                bResponse = true;

                txtName.Text = dts.Rows[0]["AccountName"].ToString();
                txtNumber.Text = dts.Rows[0]["AccountNumber"].ToString();
                cboBank.Text = dts.Rows[0]["BankName"].ToString();
                cboBranch.Text = dts.Rows[0]["BranchName"].ToString();
                //chkActive.CheckState = dts.Rows[0]["Status"].ToString();


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
                    ID = Convert.ToInt32(dr["BankAccountID"]);
                    bResponse = FillField(Convert.ToInt32(dr["BankAccountID"]));
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

        private void UpdateRecord()
        {
            try
            {
                if (txtName.Text == "")
                {
                    Common.setEmptyField("Account Name", Program.ApplicationName);
                    txtName.Focus(); return;
                }
                else if (cboBank.Text == "")
                {
                    Common.setEmptyField("Bank Name", Program.ApplicationName);
                    cboBank.Focus(); return;
                }
                else if (cboBranch.Text == "")
                {
                    Common.setEmptyField("Branch Name", Program.ApplicationName);
                    cboBranch.Focus(); return;
                }
                else if (txtNumber.Text == "")
                {
                    Common.setEmptyField("Account Number", Program.ApplicationName);
                    txtNumber.Focus(); return;
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

                                using (SqlCommand sqlCommand1 = new SqlCommand(String.Format("INSERT INTO [tblBankAccount]([AccountName],[AccountNumber],[BranchID],[IsActive]) VALUES ('{0}','{1}','{2}','{3}');", txtName.Text.Trim(), txtNumber.Text.Trim(), Convert.ToInt32(cboBranch.SelectedValue.ToString()), chkActive.Checked), db, transaction))
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

                        if (MessageBox.Show("Do you want to add another record?", Program.ApplicationName, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
                        {
                            //bttnCancel.PerformClick();
                            tsbReload.PerformClick();

                        }
                        else
                        {
                            //bttnReset.PerformClick();
                            setReload(); Clear(); cboBank.Focus();
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
                                //MessageBox.Show(MDIMain.stateCode);
                                //fieldid
                                using (SqlCommand sqlCommand1 = new SqlCommand(String.Format(String.Format("UPDATE [tblBankAccount] SET [AccountName]='{{0}}',[AccountNumber]='{{1}}',[BranchID]='{{2}}' ,[IsActive] ='{{3}}' where  BankAccountID ='{0}'", ID), txtName.Text.Trim(), txtNumber.Text.Trim(), Convert.ToInt32(cboBranch.SelectedValue.ToString()), chkActive.Checked), db, transaction))
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
                        Common.setMessageBox("Changes in record has been successfully saved.", Program.ApplicationName, 1);
                        //bttnReset.PerformClick();
                        tsbReload.PerformClick();

                    }
                }
            }
            catch (Exception ex)
            {
                Tripous.Sys.ErrorBox(ex);
            }

        }

        void cboBank_KeyPress(object sender, KeyPressEventArgs e)
        {
            Methods.AutoComplete(cboBank, e, true);
        }

        void cboBranch_KeyPress(object sender, KeyPressEventArgs e)
        {
            Methods.AutoComplete(cboBranch, e, true);
        }

        void cboBank_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboBank.SelectedValue != null && !isFirst)
            {
                //setReload(Convert.ToInt32(cboBank.SelectedValue.ToString()));
                setDBComboBoxBranch(cboBank.SelectedValue.ToString());
                cboBranch.SelectedIndex = -1;

            }
        }

        public void setDBComboBoxBranch(string parameter)
        {
            DataTable Dt;

            //connect.connect.Close();

            using (var ds = new System.Data.DataSet())
            {
                //connect.connect.Open();
                string query = String.Format("select *  from tblBankBranch where BankShortCode = '{0}'", parameter);

                using (SqlDataAdapter ada = new SqlDataAdapter(query, Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }

                Dt = ds.Tables[0];
            }

            Common.setComboList(cboBranch, Dt, "BranchID", "BranchName");

            cboBranch.SelectedIndex = -1;


        }

        private void Clear()
        {
            //txtStreetGroup.Clear();
            txtName.Clear();
            txtNumber.Clear();
            setDBComboBox();


        }


    }
}
