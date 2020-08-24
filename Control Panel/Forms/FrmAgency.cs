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
    public partial class FrmAgency : Form
    {
        //DBConnection connect = new DBConnection();

        //Methods extMethods = new Methods();

        public static FrmAgency publicStreetGroup;

        protected TransactionTypeCode iTransType;

        protected bool boolIsUpdate;

        protected int ID;

        public FrmAgency()
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

        void OnFormLoad(object sender, EventArgs e)
        {
            ShowForm();
            setDBComboBox();
            setReload();
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
                txtCode.Visible = true;
                label1.Visible = true;
                iTransType = TransactionTypeCode.New;
                ShowForm();
                boolIsUpdate = false;
            }
            else if (sender == tsbEdit)
            {
                groupControl2.Text = "Edit Record Mode";
                txtCode.Visible = false;
                label1.Visible = false;
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

        public void setDBComboBox()
        {
            DataTable Dt;

            //connect.connect.Close();

            using (var ds = new System.Data.DataSet())
            {
                //connect.connect.Open();
                string query = "select *  from tblAgencyNature";
                using (SqlDataAdapter ada = new SqlDataAdapter(query, Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }

                Dt = ds.Tables[0];
            }

            Common.setComboList(cboNature, Dt, "NatureID", "Description");

        }

        private void setReload()
        {
            //connect.connect.Close();
            DataTable dt;
            //System.Data.DataSet ds;
            using (var ds = new System.Data.DataSet())
            {
                //connect.connect.Open();
                using (SqlDataAdapter ada = new SqlDataAdapter("select * from ViewAgencyNature ", Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }
                //connect.connect.Close();
                dt = ds.Tables[0];
                gridControl1.DataSource = dt.DefaultView;
            }
            gridView1.OptionsBehavior.Editable = false;
            gridView1.Columns["AgencyCode"].Visible = false;
            gridView1.Columns["Description"].Visible = false;
            gridView1.Columns["Phone"].Visible = false;
            gridView1.Columns["Fax"].Visible = false;
            gridView1.Columns["Email"].Visible = false;
            gridView1.Columns["ContactPhone"].Visible = false;
            gridView1.BestFitColumns();
        }

        private void Clear()
        {
            //txtStreetGroup.Clear();
            txtCode.Clear();
            txtAddress.Clear();
            txtCName.Clear();
            txtCPhone.Clear();
            txtEmail.Clear();
            txtFax.Clear();
            txtName.Clear();
            txtPhone.Clear();
            setDBComboBox();


        }

        private bool FillField(int fieldid)
        {
            bool bResponse = false;
            //load data from the table into the forms for edit
            //DataTable dts = extMethods.LoadData(String.Format("select * from ViewAgencyNature where AgencyCode ='{0}'", fieldid));

            DataTable dts = (new Logic()).getSqlStatement((String.Format("select * from ViewAgencyNature where AgencyCode ='{0}'", fieldid))).Tables[0];

            if (dts != null)
            {
                bResponse = true;
                txtName.Text = dts.Rows[0]["AgencyName"].ToString();
                txtAddress.Text = dts.Rows[0]["Address"].ToString();
                txtCName.Text = dts.Rows[0]["ContactName"].ToString();
                txtCPhone.Text = dts.Rows[0]["ContactPhone"].ToString();
                txtEmail.Text = dts.Rows[0]["Email"].ToString();
                txtFax.Text = dts.Rows[0]["Fax"].ToString();
                txtPhone.Text = dts.Rows[0]["Phone"].ToString();
                cboNature.Text = dts.Rows[0]["Description"].ToString();
            }
            else
                bResponse = false;

            return bResponse;
        }

        private void UpdateRecord()
        {
            try
            {
                if (txtName.Text == "")
                {
                    Common.setEmptyField("Agency Name Group", Program.ApplicationName);
                    txtName.Focus();
                }
                else if (!Logic.IsNumber((string)txtCode.Text))
                {
                    Common.setMessageBox("Agency Code can only be in number values", Program.ApplicationName, 1);
                    txtCode.Clear(); txtCode.Focus();
                    return;
                }
                else
                {
                    string streetGroup = txtName.Text.Trim();

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
                                string query = String.Format("INSERT INTO [tblAgency]([AgencyCode],[AgencyName],[StateCode],[Address],[Phone],[Fax],[ContactName],[ContactPhone],[Email],[NatureID]) VALUES ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}');", txtCode.Text.Trim(), txtName.Text.Trim().ToUpperInvariant(), Program.stateCode, txtAddress.Text.Trim().ToUpperInvariant(), txtPhone.Text.Trim(), txtFax.Text.Trim(), txtCName.Text.Trim().ToUpperInvariant(), txtCPhone.Text.Trim(), txtEmail.Text.Trim(), Convert.ToInt32(cboNature.SelectedValue.ToString()));

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

                        if (MessageBox.Show("Do you want to add another record?", Program.ApplicationName, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
                        {
                            bttnCancel.PerformClick();
                        }
                        else
                        {
                            //bttnReset.PerformClick();
                            setReload(); Clear(); txtCode.Focus();
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
                                using (SqlCommand sqlCommand1 = new SqlCommand(String.Format(String.Format("UPDATE [tblAgency] SET [AgencyName]='{{0}}',[StateCode]='{{1}}',[Address]='{{2}}',[Phone]='{{3}}',[Fax]='{{4}}',[ContactName]='{{5}}',[ContactPhone]='{{6}}',[Email]='{{7}}',[NatureID]='{{8}}' where  [AgencyCode] ='{0}'", ID), txtName.Text.Trim().ToUpperInvariant(), Program.stateCode, txtAddress.Text.Trim().ToUpperInvariant(), txtPhone.Text.Trim(), txtFax.Text.Trim(), txtCName.Text.Trim().ToUpperInvariant(), txtCPhone.Text.Trim(), txtEmail.Text.Trim(), Convert.ToInt32(cboNature.SelectedValue.ToString())), db, transaction))
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

                        //if ((new BusinessLogic()).UpdateStreetGroup(Common.GetComboBoxValue(cbTown), streetGroup, TownID))
                        //{
                        setReload();
                        Common.setMessageBox("Changes in record has been successfully saved.", Program.ApplicationName, 1);
                        //bttnReset.PerformClick();
                        tsbReload.PerformClick();
                        //}
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
                    ID = Convert.ToInt32(dr["AgencyCode"]);
                    bResponse = FillField(Convert.ToInt32(dr["AgencyCode"]));
                }
                else
                {
                    Common.setMessageBox("No Record is seleected", "", 1);
                    bResponse = false;
                }
            }
            return bResponse;
        }

        private void FrmAgency_Leave(object sender, EventArgs e)
        {

        }

        public void RefreshForm()
        {
            NavBars.ToolStripEnableDisableControls(toolStrip, Tag as String);
        }

    }
}
