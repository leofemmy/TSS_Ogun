using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TaxSmartSuite.Class;
using DevExpress.XtraGrid.Views.Grid;
using System.Data.SqlClient;
using Control_Panel.Class;

namespace Control_Panel.Forms
{
    public partial class FrmPlatForm : Form
    {

        //DBConnection connect = new DBConnection();

        //Methods extMethods = new Methods();

        public static FrmPlatForm publicStreetGroup;

        protected TransactionTypeCode iTransType;

        protected bool boolIsUpdate;

        protected int ID;

        bool isFirst = true;

        public FrmPlatForm()
        {
            InitializeComponent();

            publicStreetGroup = this;

            setImages();

            ToolStripEvent();

            iTransType = TransactionTypeCode.Null;

            Load += OnFormLoad;

            gridView1.DoubleClick += gridView1_DoubleClick;

            bttnCancel.Click += Bttn_Click;

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

        void gridView1_DoubleClick(object sender, EventArgs e)
        {
            tsbEdit.PerformClick();
        }

        void OnFormLoad(object sender, EventArgs e)
        {
            ShowForm();
            setDBComboBox();
            isFirst = false;
            setReload();
            cboBank.KeyPress += cboBank_KeyPress;
            cboBranch.KeyPress += cboBranch_KeyPress;
            cboAccount.KeyPress += cboAccount_KeyPress;
            cboBank.SelectedIndexChanged += cboBank_SelectedIndexChanged;
            //cboBranch.SelectedIndexChanged += cboBranch_SelectedIndexChanged;
            cboBranch.Leave += cboBranch_Leave;
            cboBank_SelectedIndexChanged(null, null);

        }

        void Bttn_Click(object sender, EventArgs e)
        {
            if (sender == bttnCancel)
            {
                tsbReload.PerformClick();
            }
            //else if (sender == bttnReset)
            //{
            //    if (!boolIsUpdate)
            //        Clear();
            //    else
            //        FillField(ID);
            //    //setReload();
            //}
            else if (sender == bttnUpdate)
            {
                UpdateRecord();
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

        protected bool EditRecordMode()
        {
            bool bResponse = false;
            GridView view = (GridView)gridControl1.FocusedView;
            if (view != null)
            {
                DataRow dr = view.GetDataRow(view.FocusedRowHandle);
                if (dr != null)
                {
                    ID = Convert.ToInt32(dr["PlatformID"]);
                    bResponse = FillField(Convert.ToInt32(dr["PlatformID"]));
                }
                else
                {
                    Common.setMessageBox("No Record is seleected", "", 1);
                    bResponse = false;
                }
            }
            return bResponse;
        }

        public void setDBComboBox()
        {
            DataTable Dt;

            //connect.connect.Close();

            using (var ds = new System.Data.DataSet())
            {
                //connect.connect.Open();
                string query = "select bankshortcode,bankname  from ViewValidPlatformBank";
                using (SqlDataAdapter ada = new SqlDataAdapter(query, Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }

                Dt = ds.Tables[0];
            }

            Common.setComboList(cboBank, Dt, "bankshortcode", "bankname");

            cboBank.SelectedIndex = -1;

            
        }

        private void setReload()
        {
            
            DataTable dt;
            //System.Data.DataSet ds;
            using (var ds = new System.Data.DataSet())
            {
                
                using (SqlDataAdapter ada = new SqlDataAdapter("select * from ViewPlatBankAccount", Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }
                
                dt = ds.Tables[0];
                gridControl1.DataSource = dt.DefaultView;
            }
            gridView1.OptionsBehavior.Editable = false;
            gridView1.Columns["PlatformID"].Visible = false;
            //gridView1.Columns["Address"].Visible = false;
            //gridView1.Columns["Telephone"].Visible = false;
            //gridView1.Columns["Fax"].Visible = false;
            //gridView1.Columns["Email"].Visible = false;
            //gridView1.Columns["BranchID"].Visible = false;
            //gridView1.Columns["PlatFormCode"].Visible = false;
            gridView1.BestFitColumns();
        }

        private void Clear()
        {
            //txtStreetGroup.Clear();
            setDBComboBox();
            cboAccount.Text = string.Empty;
            cboBranch.Text = string.Empty;
            

        }

        private bool FillField(int fieldid)
        {
            bool bResponse = false;
            //load data from the table into the forms for edit
            //DataTable dts = extMethods.LoadData(String.Format("select * from ViewPlatBankAccount where PlatformID ='{0}'", fieldid));

            DataTable dts = (new Logic()).getSqlStatement((String.Format("select * from ViewPlatBankAccount where PlatformID ='{0}'", fieldid))).Tables[0];

            if (dts != null)
            {
                bResponse = true;

                cboBank.Text = dts.Rows[0]["BankName"].ToString();
                cboBranch.Text = dts.Rows[0]["BranchName"].ToString();
                cboAccount.Text = dts.Rows[0]["AccountNumber"].ToString();
               
            }
            else
                bResponse = false;

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
                if (cboBank.Text  == "")
                {
                    Common.setEmptyField("Bank Name", Program.ApplicationName);
                    cboBank.Focus(); return;
                }
                else if (cboBranch.Text  == "")
                {
                    Common.setEmptyField("Branch Name", Program.ApplicationName);
                    cboBranch.Focus(); return;
                }
                else if (cboAccount.Text == "")
                {
                    Common.setEmptyField("Account Name", Program.ApplicationName);
                    cboAccount.Focus(); return;
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

                                using (SqlCommand sqlCommand1 = new SqlCommand(String.Format("INSERT INTO [tblBankPlatformAccount]([BankShortCode],[BranchID],[BankAccountID]) VALUES ('{0}','{1}','{2}');",cboBank.SelectedValue.ToString(),Convert.ToInt32(cboBranch.SelectedValue.ToString()), Convert.ToInt32(cboAccount.SelectedValue.ToString())), db, transaction))
                                {
                                    sqlCommand1.ExecuteNonQuery();
                                }

                                transaction.Commit();
                            }
                            catch (SqlException sqlError)
                            {
                                transaction.Rollback();
                            }
                            db.Close();
                        }
                        setReload();
                        Common.setMessageBox("Record has been successfully added", Program.ApplicationName, 1);
                        if (MessageBox.Show("Do you want to add another record?", Program.ApplicationName, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
                        {
                            bttnCancel.PerformClick();
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
                                using (SqlCommand sqlCommand1 = new SqlCommand(String.Format(String.Format("UPDATE [tblBankPlatformAccount] SET [BankShortCode]='{{0}}',[BranchID] ='{{1}}',[BankAccountID]='{{2}}'  where  PlatformID ='{0}'", ID), cboBank.SelectedValue.ToString(), Convert.ToInt32(cboBranch.SelectedValue.ToString()), Convert.ToInt32(cboAccount.SelectedValue.ToString())), db, transaction))
                                {
                                    sqlCommand1.ExecuteNonQuery();
                                }

                                transaction.Commit();
                            }
                            catch (SqlException sqlError)
                            {
                                transaction.Rollback();
                            }
                            db.Close();
                        }

                        setReload();
                        Common.setMessageBox("Changes in record has been successfully saved.", Program.ApplicationName, 1);
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

        void cboAccount_KeyPress(object sender, KeyPressEventArgs e)
        {
            Methods.AutoComplete(cboAccount, e, true);
        }

        void cboBank_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (cboBank.SelectedValue != null && !isFirst)
            {
                setDBComboBoxs(cboBank.SelectedValue.ToString());

                //cboBranch_SelectedIndexChanged(sender, e);
               
            }
        }

        //void cboBranch_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (cboBranch.SelectedValue != null)
        //    {
        //        setDBComboBoxAcct(Convert.ToInt32(cboBranch.SelectedValue.ToString()));
        //    }
        //}

        public void setDBComboBoxs(string parameter)
        {
            DataTable Dt;

            //connect.connect.Close();

            using (var ds = new System.Data.DataSet())
            {
                //connect.connect.Open();
                string query = String.Format("select BranchID, BranchName from tblBankBranch where BankShortCode = '{0}'", parameter);
                using (SqlDataAdapter ada = new SqlDataAdapter(query, Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }

                Dt = ds.Tables[0];
            }

            Common.setComboList(cboBranch, Dt, "BranchID", "BranchName");

            cboBranch.SelectedIndex = -1;

            //connect.connect.Close();
        }

        public void setDBComboBoxAcct(int parameter)
        {
            DataTable Dt;

            //connect.connect.Close();

            using (var ds = new System.Data.DataSet())
            {
                //connect.connect.Open();
                string query = String.Format("select * from tblBankAccount where BranchID = '{0}'", parameter);
                using (SqlDataAdapter ada = new SqlDataAdapter(query, Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }

                Dt = ds.Tables[0];
            }

            Common.setComboList(cboAccount, Dt, "BankAccountID", "AccountNumber");

            cboAccount.SelectedIndex = -1;

            
        }

        void cboBranch_Leave(object sender, EventArgs e)
        {
            if (cboBranch.SelectedValue != null)
            {
                setDBComboBoxAcct(Convert.ToInt32(cboBranch.SelectedValue.ToString()));
            }
           
        }


    }
}
