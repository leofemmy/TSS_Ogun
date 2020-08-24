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
using Control_Panel.Class;
using DevExpress.XtraGrid.Views.Grid;

namespace Control_Panel.Forms
{
    public partial class FrmIncome : Form
    {
        public static FrmIncome publicStreetGroup;

        protected TransactionTypeCode iTransType;

        protected bool boolIsUpdate;

        protected string ID;

        bool isFirst = true;

        public FrmIncome()
        {
            InitializeComponent();

            publicStreetGroup = this;

            setImages();

            ToolStripEvent();

            iTransType = TransactionTypeCode.Null;

            Load += OnFormLoad;

            gridView1.DoubleClick += gridView1_DoubleClick;

            bttnCancel.Click += Bttn_Click;

            bttnReset.Click += Bttn_Click;

            bttnUpdate.Click += Bttn_Click;

            OnFormLoad(null, null);
        }

        private void Clear()
        {

            txtName.Clear();

            setDBComboBox();

            setDBComboBox1();


        }

        private void setImages()
        {
            tsbNew.Image = MDIMain.publicMDIParent.i16x16.Images[5];
            tsbEdit.Image = MDIMain.publicMDIParent.i16x16.Images[6];
            tsbDelete.Image = MDIMain.publicMDIParent.i16x16.Images[7];
            tsbReload.Image = MDIMain.publicMDIParent.i16x16.Images[0];
            tsbClose.Image = MDIMain.publicMDIParent.i16x16.Images[11];

            bttnCancel.Image = MDIMain.publicMDIParent.i32x32.Images[9];
            bttnReset.Image = MDIMain.publicMDIParent.i32x32.Images[8];
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
                //    //Unlockfield();
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
            bttnReset.PerformClick();
        }

        void OnFormLoad(object sender, EventArgs e)
        {
            ShowForm();

            setDBComboBox();

            isFirst = false;

            setDBComboBox1();

            setReload();


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
                    //Lockfield();
                    break;
                case TransactionTypeCode.Edit:
                    splitContainer1.Panel1Collapsed = false;
                    splitContainer1.Panel2Collapsed = false;
                    //Lockfield();
                    break;
                case TransactionTypeCode.Delete:
                    splitContainer1.Panel1Collapsed = true;
                    splitContainer1.Panel2Collapsed = false;
                    break;
                case TransactionTypeCode.Reload:
                    splitContainer1.Panel1Collapsed = false;
                    splitContainer1.Panel2Collapsed = false;
                    break;
                default:
                    splitContainer1.Panel1Collapsed = true;
                    splitContainer1.Panel2Collapsed = false;
                    break;
            }
        }

        public void setDBComboBox()
        {
            DataTable Dt;

            //connect.connect.Close();

            using (var ds = new System.Data.DataSet())
            {
                //connect.connect.Open();
                string query = "select IncomeSourcesId,Description from tblIncomeSources";

                using (SqlDataAdapter ada = new SqlDataAdapter(query, Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }

                Dt = ds.Tables[0];
            }

            Common.setComboList(cboSources, Dt, "IncomeSourcesId", "Description");

            cboSources.SelectedIndex = -1;

            //connect.connect.Close();
        }

        public void setDBComboBox1()
        {
            DataTable Dt;

            //connect.connect.Close();

            using (var ds = new System.Data.DataSet())
            {
                //connect.connect.Open();
                string query = "select IncomeClass, Description from tblIncomeClass";

                using (SqlDataAdapter ada = new SqlDataAdapter(query, Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }

                Dt = ds.Tables[0];
            }

            Common.setComboList(cboClass, Dt, "IncomeClass", "Description");

            cboClass.SelectedIndex = -1;

            //connect.connect.Close();
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
            else if (sender == bttnReset)
            {
                if (!boolIsUpdate)
                    Clear();
                else
                    FillField(ID);
                setReload();
            }
            else if (sender == bttnUpdate)
            {
                UpdateRecord();
            }
        }

        private void UpdateRecord()
        {
            try
            {
                if (txtName.Text == "")
                {
                    Common.setEmptyField(" Income Type Name ", Program.ApplicationName);
                    txtName.Focus();
                }
                else if (cboClass.SelectedValue.ToString() == null || cboClass.SelectedValue.ToString() == "")
                {
                    Common.setEmptyField(" Income Class ", Program.ApplicationName);
                    cboClass.Focus();
                }
                else if (cboSources.SelectedValue.ToString() == null || cboSources.SelectedValue.ToString() == "")
                {
                    Common.setEmptyField(" Income Sources ", Program.ApplicationName);
                    cboSources.Focus();

                }
                else
                {
                    //check form status either new or edit
                    //insert new record

                    if (!boolIsUpdate)
                    {
                        using (SqlConnection db = new SqlConnection(Logic.ConnectionString))
                        {
                            SqlTransaction transaction;

                            db.Open();

                            transaction = db.BeginTransaction();
                            try
                            {

                                using (SqlCommand sqlCommand1 = new SqlCommand(String.Format("INSERT INTO [tblIncomeType]([TypeName],[IncomeClass],[IncomeSourcesId]) VALUES ('{0}','{1}','{2}');", txtName.Text.Trim().ToUpperInvariant(), Convert.ToInt32(cboClass.SelectedValue.ToString()), Convert.ToInt32(cboSources.SelectedValue.ToString())), db, transaction))
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
                            bttnReset.PerformClick();
                        }
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
                                using (SqlCommand sqlCommand1 = new SqlCommand(String.Format(String.Format("UPDATE [tblIncomeType] SET [TypeName]='{{0}}',[IncomeClass] ='{{1}}',[IncomeSourcesId]='{{2}}' where  IncomeTypeCode ='{0}'", ID), txtName.Text.Trim().ToUpperInvariant(), Convert.ToInt32(cboClass.SelectedValue.ToString()), Convert.ToInt32(cboSources.SelectedValue.ToString())), db, transaction))
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

                        bttnReset.PerformClick();

                    }
                }
            }
            catch (Exception ex)
            {
                Tripous.Sys.ErrorBox(ex);
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
                string query = "select * from ViewIncomeType";

                using (SqlDataAdapter ada = new SqlDataAdapter(query, Logic.ConnectionString))

                //using (SqlDataAdapter ada = new SqlDataAdapter(String.Format("select * from ViewIncomeType )", Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }
                //connect.connect.Close();
                dt = ds.Tables[0];

                gridControl1.DataSource = dt.DefaultView;
                //}
                //gridView1.OptionsBehavior.Editable = false;
                //gridView1.Columns["BusinessID"].Visible = false;
                //gridView1.Columns["BusinessSubClassID"].Visible = false;
                gridView1.BestFitColumns();
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
                    ID = dr["Code"].ToString();

                    bResponse = FillField(dr["Code"].ToString());
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
            ////load data from the table into the forms for edit

            DataTable dts = (new Logic()).getSqlStatement((String.Format("select * from ViewIncomeType where Code ='{0}'", fieldid))).Tables[0];

            if (dts != null)
            {
                bResponse = true;

                txtName.Text = dts.Rows[0]["Name"].ToString();

                cboClass.Text = dts.Rows[0]["Class"].ToString();

                cboSources.Text = dts.Rows[0]["Type"].ToString();
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
