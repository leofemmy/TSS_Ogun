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
    public partial class FrmRevenueType : Form
    {
        //DBConnection connect = new DBConnection();

        //Methods extMethods = new Methods();

        public static FrmRevenueType publicStreetGroup;

        protected TransactionTypeCode iTransType;

        protected bool boolIsUpdate;

        protected int ID;

        bool isFirst = true;

        public FrmRevenueType()
        {
            InitializeComponent();


            publicStreetGroup = this;

            setImages(); ToolStripEvent();

            iTransType = TransactionTypeCode.New;

            Load += OnFormLoad;

            gridView1.DoubleClick += gridView1_DoubleClick;

            bttnCancel.Click += Bttn_Click;

            //bttnReset.Click += Bttn_Click;

            bttnUpdate.Click += Bttn_Click;

            OnFormLoad(null, null);

            txtHead.Leave += txtHead_Leave;

            txtSubHead.Leave += txtSubHead_Leave;

            //cboAgency.Text = string.Empty;
            cboCategory.SelectedIndex = -1;

            cboSep.SelectedIndex = -1;

            cboSubLedg.SelectedIndex = -1;
        }

        void txtSubHead_Leave(object sender, EventArgs e)
        {
            if (!Logic.IsNumber((string)txtSubHead.Text))
            {
                Common.setMessageBox("Revenue Subhead can only be in number values", Program.ApplicationName, 1);
                txtSubHead.Clear(); txtSubHead.Focus();
                return;
            }
            else
            {
                txtRevCode.Text = txtHead.Text.Trim() + cboSep.Text.Trim() + txtSubHead.Text.Trim();
                txtRevCode.Enabled = false;
            }
        }

        void txtHead_Leave(object sender, EventArgs e)
        {
            if (!Logic.IsNumber((string)txtHead.Text))
            {
                Common.setMessageBox("Revenue Head can only be in number values", Program.ApplicationName, 1);
                txtHead.Clear(); txtHead.Focus();
                return;
            }
        }

        void OnFormLoad(object sender, EventArgs e)
        {
            ShowForm();
            setDBComboBox();
            isFirst = false;
            setDBComboBox1();
            setDBComboBox2();
            setDBComboBox3();
            setReload();

            cboAgency.SelectedIndexChanged += cboAgency_SelectedIndexChanged;

            cboAgency.KeyPress += cboAgency_KeyPress;

            cboCategory.KeyPress += cboCategory_KeyPress;

            cboSubLedg.KeyPress += cboSubLedg_KeyPress;

            cboAgency_SelectedIndexChanged(null, null);

        }

        void cboAgency_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboAgency.SelectedValue != null && !isFirst)
            {
                setReloadByAgent(Convert.ToInt32(cboAgency.SelectedValue.ToString()));

            }
        }

        private void setReload()
        {

            DataTable dt;
            //System.Data.DataSet ds;
            using (var ds = new System.Data.DataSet())
            {

                using (SqlDataAdapter ada = new SqlDataAdapter("select * from ViewRevenueTypeCategory", Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }

                dt = ds.Tables[0];
                gridControl1.DataSource = dt.DefaultView;
            }
            gridView1.OptionsBehavior.Editable = false;
            gridView1.Columns["RevenueTypeID"].Visible = false;
            gridView1.Columns["AgencyCode"].Visible = false;
            gridView1.Columns["Seperator"].Visible = false;
            gridView1.BestFitColumns();
        }

        private void setReloadByAgent(int Ids)
        {

            DataTable dt;
            //System.Data.DataSet ds;
            using (var ds = new System.Data.DataSet())
            {

                using (SqlDataAdapter ada = new SqlDataAdapter(String.Format("select * from ViewRevenueTypeCategory WHERE AgencyCode = '{0}'", Ids), Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }

                dt = ds.Tables[0];
                gridControl1.DataSource = dt.DefaultView;
            }
            gridView1.OptionsBehavior.Editable = false;
            gridView1.Columns["RevenueTypeID"].Visible = false;
            gridView1.Columns["AgencyCode"].Visible = false;
            gridView1.Columns["Seperator"].Visible = false;
            gridView1.BestFitColumns();
        }

        private void UpdateRecord()
        {
            try
            {
                if (txtHead.Text == "")
                {
                    Common.setEmptyField("Revenue Head Code Can't Be Empty", Program.ApplicationName);
                    txtHead.Focus();
                }
                else if (txtSubHead.Text == "")
                {
                    Common.setEmptyField("Revenue Sub Head Code Can't Be Empty", Program.ApplicationName);
                    txtSubHead.Focus();
                }
                else if (txtRevenueDesc.Text == "")
                {
                    Common.setEmptyField("Revenue Description Can't Be Empty", Program.ApplicationName);
                    txtRevenueDesc.Focus();
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
                                string query = String.Format("INSERT INTO [tblRevenueType]([AgencyCode],[CategoryID],[AccountID],[SeperatorID],[Head],[SubHead],[Description],[RevenueCode]) VALUES ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}');", Convert.ToInt32(cboAgency.SelectedValue), Convert.ToInt32(cboCategory.SelectedValue), Convert.ToInt32(cboSubLedg.SelectedValue), Convert.ToInt32(cboSep.SelectedValue), txtHead.Text.Trim(), txtSubHead.Text.Trim(), txtRevenueDesc.Text.Trim(), txtRevCode.Text.Trim());

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

                        setReloadByAgent(Convert.ToInt32(cboAgency.SelectedValue.ToString()));

                        Common.setMessageBox("Record has been successfully added", Program.ApplicationName, 1);

                        if (MessageBox.Show("Do you want to add another record?", Program.ApplicationName, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
                        {
                            bttnCancel.PerformClick();
                        }
                        else
                        {

                            setReloadByAgent(Convert.ToInt32(cboAgency.SelectedValue.ToString()));
                            Clear(); cboAgency.Focus();
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
                                ////fieldid
                                string strupdate = String.Format(String.Format("UPDATE [tblRevenueType] SET [AgencyCode]='{{0}}',[CategoryID]='{{1}}',[AccountID]='{{2}}',[SeperatorID]='{{3}}',[Head]='{{4}}',[SubHead]='{{5}}',[Description]='{{6}}',[RevenueCode]='{{7}}' where RevenueTypeID ='{0}'", ID), Convert.ToInt32(cboAgency.SelectedValue), Convert.ToInt32(cboCategory.SelectedValue), Convert.ToInt32(cboSubLedg.SelectedValue), Convert.ToInt32(cboSep.SelectedValue), txtHead.Text.Trim(), txtSubHead.Text.Trim(), txtRevenueDesc.Text.Trim(), txtRevCode.Text.Trim());

                                using (SqlCommand sqlCommand1 = new SqlCommand(strupdate, db, transaction))
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

                        setReloadByAgent(Convert.ToInt32(cboAgency.SelectedValue));
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

            //}
            else if (sender == bttnUpdate)
            {
                UpdateRecord();
            }
        }

        private void Clear()
        {

            txtHead.Clear();
            txtRevCode.Clear();
            txtRevenueDesc.Clear();
            txtSubHead.Clear();
            setDBComboBox();
            setDBComboBox1();
            setDBComboBox2();
            setDBComboBox3();


        }

        private bool FillField(int fieldid)
        {
            bool bResponse = false;
            //load data from the table into the forms for edit
            //DataTable dts = extMethods.LoadData(String.Format("select * from ViewRevenueTypeCategory where RevenueTypeID ='{0}'", fieldid));

            DataTable dts = (new Logic()).getSqlStatement((String.Format("select * from ViewRevenueTypeCategory where RevenueTypeID ='{0}'", fieldid))).Tables[0];

            if (dts != null)
            {
                bResponse = true;
                txtHead.Text = dts.Rows[0]["Head"].ToString();
                txtRevCode.Text = dts.Rows[0]["RevenueCode"].ToString();
                txtRevenueDesc.Text = dts.Rows[0]["Description"].ToString();
                txtSubHead.Text = dts.Rows[0]["SubHead"].ToString();
                //txtHead.Text = dts.Rows[0]["StationHead"].ToString();
                cboAgency.Text = dts.Rows[0]["AgencyName"].ToString();
                cboSep.Text = dts.Rows[0]["Seperator"].ToString();
                cboCategory.Text = dts.Rows[0]["Category"].ToString();
                cboSubLedg.Text = dts.Rows[0]["SubLedger"].ToString();

            }
            else
                bResponse = false;

            return bResponse;
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

        public void setDBComboBox()
        {
            DataTable Dt;



            using (var ds = new System.Data.DataSet())
            {

                string query = "select *  from tblAgency";
                using (SqlDataAdapter ada = new SqlDataAdapter(query, Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }

                Dt = ds.Tables[0];
            }

            Common.setComboList(cboAgency, Dt, "AgencyCode", "AgencyName");

        }

        public void setDBComboBox1()
        {
            DataTable Dt;



            using (var ds = new System.Data.DataSet())
            {

                string query = "select *  from tblSeperator";
                using (SqlDataAdapter ada = new SqlDataAdapter(query, Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }

                Dt = ds.Tables[0];
            }

            Common.setComboList(cboSep, Dt, "SeperatorID", "Description");

        }

        public void RefreshForm()
        {
            NavBars.ToolStripEnableDisableControls(toolStrip, Tag as String);
        }

        public void setDBComboBox2()
        {
            DataTable Dt;



            using (var ds = new System.Data.DataSet())
            {

                string query = "select *  from tblRevenueCategory";
                using (SqlDataAdapter ada = new SqlDataAdapter(query, Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }

                Dt = ds.Tables[0];
            }

            Common.setComboList(cboCategory, Dt, "CategoryID", "Description");

        }

        public void setDBComboBox3()
        {
            DataTable Dt;



            using (var ds = new System.Data.DataSet())
            {

                string query = "select *  from tblSubLedgerAccount";
                using (SqlDataAdapter ada = new SqlDataAdapter(query, Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }

                Dt = ds.Tables[0];
            }

            Common.setComboList(cboSubLedg, Dt, "AccountID", "Description");

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
                Unlockfield();
                boolIsUpdate = false;
            }
            else if (sender == tsbEdit)
            {
                groupControl2.Text = "Edit Record Mode";
                iTransType = TransactionTypeCode.Edit;
                if (EditRecordMode())
                {
                    ShowForm();
                    Unlockfield();
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
                    Lockfield();
                    break;
                case TransactionTypeCode.Edit:
                    splitContainer1.Panel1Collapsed = false;
                    splitContainer1.Panel2Collapsed = false;
                    Lockfield();
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
                    ID = Convert.ToInt32(dr["RevenueTypeID"]);
                    bResponse = FillField(Convert.ToInt32(dr["RevenueTypeID"]));
                }
                else
                {
                    Common.setMessageBox("No Record is seleected", "", 1);
                    bResponse = false;
                }
            }
            return bResponse;
        }

        //private void txtSubHead_TextChanged(object sender, EventArgs e)
        //{
        //    //txtRevCode.Text = txtHead.Text.Trim() + cboSep.Text.Trim() + txtSubHead.Text.Trim();

        //}

        void cboAgency_KeyPress(object sender, KeyPressEventArgs e)
        {
            Methods.AutoComplete(cboAgency, e, true);
        }

        void cboCategory_KeyPress(object sender, KeyPressEventArgs e)
        {
            Methods.AutoComplete(cboCategory, e, true);
        }

        void cboSubLedg_KeyPress(object sender, KeyPressEventArgs e)
        {
            Methods.AutoComplete(cboSubLedg, e, true);

        }

        void gridView1_DoubleClick(object sender, EventArgs e)
        {
            tsbEdit.PerformClick();
        }

        private void Lockfield()
        {
            txtHead.Enabled = false;
            txtRevCode.Enabled = false;
            txtRevenueDesc.Enabled = false;
            txtSubHead.Enabled = false;
            cboSubLedg.Enabled = false;
            cboSep.Enabled = false;
            cboCategory.Enabled = false;
        }

        private void Unlockfield()
        {
            txtHead.Enabled = true;
            txtRevCode.Enabled = true;
            txtRevenueDesc.Enabled = true;
            txtSubHead.Enabled = true;
            cboSubLedg.Enabled = true;
            cboSep.Enabled = true;
            cboCategory.Enabled = true;
        }

    }
}
