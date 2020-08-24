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
using System.Xml;
using Control_Panel.Class;

namespace Control_Panel.Forms
{
    public partial class FrmBankBranch : Form
    {
        //DBConnection connect = new DBConnection();

        //Methods extMethods = new Methods();

        public static FrmBankBranch publicStreetGroup;

        protected TransactionTypeCode iTransType;

        protected bool boolIsUpdate;

        protected int ID;

        bool isFirst = true;

        public FrmBankBranch()
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
            setDBComboBoxTn();
            isFirst = false;
            setReload();
            cboBankName.KeyPress += cboBankName_KeyPress;
            cboTown.KeyPress += cboTown_KeyPress;

        }

        void gridView1_DoubleClick(object sender, EventArgs e)
        {
            tsbEdit.PerformClick();
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

        private void setReload()
        {
            //connect.connect.Close();
            DataTable dt;
            //System.Data.DataSet ds;
            using (var ds = new System.Data.DataSet())
            {
                //connect.connect.Open();
                using (SqlDataAdapter ada = new SqlDataAdapter("select * from ViewBankBranch", Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }
                //connect.connect.Close();
                dt = ds.Tables[0];
                gridControl1.DataSource = dt.DefaultView;
            }
            gridView1.OptionsBehavior.Editable = false;
            gridView1.Columns["TownName"].Visible = false;
            gridView1.Columns["Address"].Visible = false;
            gridView1.Columns["Telephone"].Visible = false;
            gridView1.Columns["Fax"].Visible = false;
            gridView1.Columns["Email"].Visible = false;
            gridView1.Columns["BranchID"].Visible = false;
            gridView1.Columns["PlatFormCode"].Visible = false; 
            gridView1.BestFitColumns();
        }

        private void Clear()
        {
            //txtStreetGroup.Clear();
            txtAddress.Clear();
            txtBranchCode.Clear();
            txtBranchName.Clear();
            txtEmail.Clear();
            txtFax.Clear();
            txtPerson.Clear();
            txtTelephone.Clear();
            txtCode.Clear();
            setDBComboBox();
            setDBComboBoxTn();

        }

        private bool FillField(int fieldid)
        {
            bool bResponse = false;
            //load data from the table into the forms for edit
            //DataTable dts = extMethods.LoadData(String.Format("select * from ViewBankBranch where BranchID ='{0}'", fieldid));

            DataTable dts = (new Logic()).getSqlStatement((String.Format("select * from ViewBankBranch where BranchID ='{0}'", fieldid))).Tables[0];


            if (dts != null)
            {
                bResponse = true;

                txtAddress.Text = dts.Rows[0]["Address"].ToString();
                txtBranchCode.Text = dts.Rows[0]["PlatFormCode"].ToString();
                txtBranchName.Text = dts.Rows[0]["BranchName"].ToString();
                txtEmail.Text = dts.Rows[0]["Email"].ToString();
                txtFax.Text = dts.Rows[0]["Fax"].ToString();
                txtPerson.Text = dts.Rows[0]["ContactPerson"].ToString();
                txtTelephone.Text = dts.Rows[0]["Telephone"].ToString();
                cboTown.Text = dts.Rows[0]["TownName"].ToString();
                cboBankName.Text = dts.Rows[0]["BankName"].ToString();
                txtCode.Text = dts.Rows[0]["BranchCode"].ToString();

            }
            else
                bResponse = false;

            return bResponse;
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

            Common.setComboList(cboBankName, Dt, "BankShortCode", "BankName");

            cboBankName.SelectedIndex = -1;

            //connect.connect.Close();
        }

        public void RefreshForm()
        {
            NavBars.ToolStripEnableDisableControls(toolStrip, Tag as String);
        }

        public void setDBComboBoxTn()
        {
            DataTable Dt;

            //connect.connect.Close();

            using (var ds = new System.Data.DataSet())
            {
                //connect.connect.Open();
                string query = "select *  from tblTown";
                using (SqlDataAdapter ada = new SqlDataAdapter(query, Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }

                Dt = ds.Tables[0];
            }

            Common.setComboList(cboTown, Dt, "TownCode", "TownName");

            cboTown.SelectedIndex = -1;

            //connect.connect.Close();
        }

        void cboBankName_KeyPress(object sender, KeyPressEventArgs e)
        {
            Methods.AutoComplete(cboBankName, e, true);
        }

        void cboTown_KeyPress(object sender, KeyPressEventArgs e)
        {
            Methods.AutoComplete(cboTown, e, true);
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
                    ID = Convert.ToInt32(dr["BranchID"]);
                    bResponse = FillField(Convert.ToInt32(dr["BranchID"]));
                }
                else
                {
                    Common.setMessageBox("No Record is seleected", "", 1);
                    bResponse = false;
                }
            }
            return bResponse;
        }

        private void UpdateRecord()
        {
            try
            {
                if (txtBranchName.Text == "")
                {
                    Common.setEmptyField("Branch Name", Program.ApplicationName);
                    txtBranchName.Focus(); return;
                }
                else if (cboBankName.Text == "")
                {
                    Common.setEmptyField("Bank Name", Program.ApplicationName);
                    cboBankName.Focus(); return;
                }
                else if (cboTown.Text == "")
                {
                    Common.setEmptyField("Town Name", Program.ApplicationName);
                    cboTown.Focus(); return;
                }
                else if (txtBranchCode.Text == "")
                {
                    Common.setEmptyField("PlatForm Code", Program.ApplicationName);
                    txtBranchCode.Focus(); return;
                }
                else if (txtCode.Text == "")
                {
                    Common.setEmptyField("Branch Code", Program.ApplicationName);
                    txtCode.Focus(); return;
                }
                else
                {
                    //string streetGroup = txtName.Text.Trim();

                    //int TownID = (string.IsNullOrEmpty(cbTown.SelectedValue.ToString())) ? 0 : Convert.ToInt32(cbTown.SelectedValue.ToString()) + 0;

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

                                using (SqlCommand sqlCommand1 = new SqlCommand(String.Format("INSERT INTO [tblBankBranch]([PlatFormCode],[BranchName],[Address],[ContactPerson],[Telephone],[Fax],[Email],[BankShortCode],[TownID],[BranchCode])VALUES ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}');", txtBranchCode.Text.Trim().ToUpperInvariant(), txtBranchName.Text.Trim().ToUpperInvariant(), txtAddress.Text.Trim(), txtPerson.Text.Trim().ToUpperInvariant(), txtTelephone.Text.Trim(), txtFax.Text.Trim(), txtEmail.Text.Trim(), cboBankName.SelectedValue.ToString(), Convert.ToInt32(cboTown.SelectedValue.ToString()), txtCode.Text.Trim()), db, transaction))
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
                            setReload(); Clear(); cboBankName.Focus();
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
                                using (SqlCommand sqlCommand1 = new SqlCommand(String.Format(String.Format("UPDATE [tblBankBranch] SET [PlatFormCode]='{{0}}',[BranchName]='{{1}}',[Address]='{{2}}' ,[ContactPerson] ='{{3}}',[Telephone]='{{4}}',[Fax]='{{5}}',[Email]='{{6}}',[BankShortCode]='{{7}}',[TownID]='{{8}}',[BranchCode]='{{9}}' where  BranchID ='{0}'", ID), txtBranchCode.Text.Trim().ToUpperInvariant(), txtBranchName.Text.Trim().ToUpperInvariant(), txtAddress.Text.Trim(), txtPerson.Text.Trim().ToUpperInvariant(), txtTelephone.Text.Trim(), txtFax.Text.Trim(), txtEmail.Text.Trim(), cboBankName.SelectedValue.ToString(), Convert.ToInt32(cboTown.SelectedValue.ToString()), txtCode.Text.Trim()), db, transaction))
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

    }
}
