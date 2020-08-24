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
using AgencyControl.Class;

namespace AgencyControl.Forms
{
    public partial class FrmZone : Form
    {
        //DBConnection connect = new DBConnection();

        //Methods extMethods = new Methods();

        public static FrmZone publicStreetGroup;

        protected TransactionTypeCode iTransType;

        protected bool boolIsUpdate;
        
        protected int ID;
        
        bool isFirst = true;

        public FrmZone()
        {
            InitializeComponent();
          
            
            publicStreetGroup = this;
            
            setImages();

            ToolStripEvent();

            iTransType = TransactionTypeCode.New ;


            Load += OnFormLoad;
            gridView1.DoubleClick += gridView1_DoubleClick;
            bttnCancel.Click += Bttn_Click;
            bttnReset.Click += Bttn_Click;
            bttnUpdate.Click += Bttn_Click;

            OnFormLoad(null, null);
        }

        private void setImages()
        {
            tsbNew.Image = MDIMains.publicMDIParent.i16x16.Images[5];
            tsbEdit.Image = MDIMains.publicMDIParent.i16x16.Images[6];
            tsbDelete.Image = MDIMains.publicMDIParent.i16x16.Images[7];
            tsbReload.Image = MDIMains.publicMDIParent.i16x16.Images[0];
            tsbClose.Image = MDIMains.publicMDIParent.i16x16.Images[11];

            bttnCancel.Image = MDIMains.publicMDIParent.i32x32.Images[9];
            bttnReset.Image = MDIMains.publicMDIParent.i32x32.Images[8];
            bttnUpdate.Image = MDIMains.publicMDIParent.i32x32.Images[7];

        }

        void OnFormLoad(object sender, EventArgs e)
        {
            ShowForm();
            setDBComboBox();
            isFirst = false;
            //setReload();
            cboAgency.SelectedIndexChanged += cboAgency_SelectedIndexChanged;
            cboAgency_SelectedIndexChanged(null, null);
            
        }

        protected void ShowForm()
        {
            switch (iTransType)
            {
                case TransactionTypeCode.Null:
                    splitContainer1.Panel1Collapsed = false;
                    splitContainer1.Panel2Collapsed = true;
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
                string query = "select *  from tblAgency";

                using (SqlDataAdapter ada = new SqlDataAdapter(query,Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }

                Dt = ds.Tables[0];
            }

            Common.setComboList(cboAgency, Dt, "AgencyCode", "AgencyName");
           
            //connect.connect.Close();
        }

        private void setReload(int parameter)
        {
            //connect.connect.Close();
            DataTable dt;
            //System.Data.DataSet ds;
            using (var ds = new System.Data.DataSet())
            {
                //connect.connect.Open();
                //using (SqlDataAdapter ada = new SqlDataAdapter("select * from ViewAgencyZone", connect.ConnectionString()))

                using (SqlDataAdapter ada = new SqlDataAdapter(String.Format("select * from ViewAgencyZone where AgencyCode= '{0}'", parameter),Logic.ConnectionString ))
                {
                    ada.Fill(ds, "table");
                }
                //connect.connect.Close();
                dt = ds.Tables[0];
                gridControl1.DataSource = dt.DefaultView;
            }
            gridView1.OptionsBehavior.Editable = false;
            gridView1.Columns["Address"].Visible = false;
            gridView1.Columns["Phone"].Visible = false;
            gridView1.Columns["Fax"].Visible = false;
            gridView1.Columns["Email"].Visible = false;
            gridView1.Columns["StationHead"].Visible = false;
            gridView1.Columns["AgencyName"].Visible = false;
            gridView1.Columns["ZoneID"].Visible = false;
            gridView1.Columns["AgencyCode"].Visible = false;
            gridView1.BestFitColumns();
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
                //setReload();
            }
            else if (sender == bttnUpdate)
            {
                UpdateRecord();
            }
        }

        private void Clear()
        {
            //txtStreetGroup.Clear();
            
            txtHead.Clear();
            txtPhone.Clear();
            txtEmail.Clear();
            txtFax.Clear();
            txtzone.Clear();
            txtPhone.Clear();
            txtaddress.Clear();
            //setDBComboBox();


        }

        private bool FillField(int fieldid)
        {
            bool bResponse = false;
            //load data from the table into the forms for edit
            ////DataTable dts = extMethods.LoadData(String.Format("select * from ViewAgencyZone where ZoneID ='{0}'", fieldid));

            DataTable dts = (new Logic()).getSqlStatement((String.Format("select * from ViewAgencyZone where ZoneID ='{0}'", fieldid))).Tables[0];

            if (dts != null)
            {
                bResponse = true;
                txtzone.Text = dts.Rows[0]["ZoneName"].ToString();
                txtaddress.Text = dts.Rows[0]["Address"].ToString();
                txtEmail.Text = dts.Rows[0]["Email"].ToString();
                txtFax.Text = dts.Rows[0]["Fax"].ToString();
                txtHead.Text = dts.Rows[0]["StationHead"].ToString();
                txtPhone.Text = dts.Rows[0]["Phone"].ToString();
                cboAgency.Text = dts.Rows[0]["AgencyName"].ToString();
            }
            else
                bResponse = false;

            return bResponse;
        }

        private void UpdateRecord()
        {
            try
            {
                if (txtzone.Text == "")
                {
                    Common.setEmptyField("Zone Name Group", Program.ApplicationName);
                    txtzone.Focus();
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

                                using (SqlCommand sqlCommand1 = new SqlCommand(String.Format("INSERT INTO [tblZone]([ZoneName],[Address],[Phone],[Fax],[Email],[StationHead],[AgencyCode]) VALUES ('{0}','{1}','{2}','{3}','{4}','{5}','{6}');", txtzone.Text.Trim().ToUpperInvariant(),txtaddress.Text.Trim().ToUpperInvariant(),txtPhone.Text.Trim(),txtFax.Text.Trim(),txtEmail.Text.Trim(),txtHead.Text.Trim().ToUpperInvariant() , Convert.ToInt32(cboAgency.SelectedValue.ToString())), db, transaction))
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
                        //setReload();
                        setReload(Convert.ToInt32(cboAgency.SelectedValue.ToString()));
                        Common.setMessageBox("Record has been successfully added", Program.ApplicationName, 1);

                        if (MessageBox.Show("Do you want to add another record?", Program.ApplicationName, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
                        {
                            bttnCancel.PerformClick();
                        }
                        else
                        {
                            bttnReset.PerformClick();
                            setReloadAgenct(Convert.ToInt32(cboAgency.SelectedValue.ToString()));
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
                                using (SqlCommand sqlCommand1 = new SqlCommand(String.Format(String.Format("UPDATE [tblZone] SET [ZoneName]='{{0}}',[Address]='{{1}}',[Phone]='{{2}}',[Fax]='{{3}}',[Email]='{{4}}',[StationHead]='{{5}}',[AgencyCode]='{{6}}' where  ZoneID ='{0}'", ID),txtzone.Text.Trim().ToUpperInvariant(),txtaddress.Text.Trim().ToUpperInvariant(),txtPhone.Text.Trim(),txtFax.Text.Trim(),txtEmail.Text.Trim(),txtHead.Text.Trim().ToUpperInvariant(), Convert.ToInt32(cboAgency.SelectedValue.ToString())), db, transaction))
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

                       ////setReload();
                        setReload(Convert.ToInt32(cboAgency.SelectedValue.ToString()));
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

        void gridView1_DoubleClick(object sender, EventArgs e)
        {
            tsbEdit.PerformClick();
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
            bttnReset.PerformClick();
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
                    ID = Convert.ToInt32(dr["ZoneID"]);
                    bResponse = FillField(Convert.ToInt32(dr["ZoneID"]));
                }
                else
                {
                    Common.setMessageBox("No Record is seleected", "", 1);
                    bResponse = false;
                }
            }
            return bResponse;
        }

        private void cboAgency_Leave(object sender, EventArgs e)
        {
            //MessageBox.Show(cboAgency.SelectedValue.ToString());
            setReloadAgenct(Convert.ToInt32(cboAgency.SelectedValue.ToString()));
        }

        private void setReloadAgenct(int id)
        {
            //connect.connect.Close();
            DataTable dt;
            //System.Data.DataSet ds;
            using (var ds = new System.Data.DataSet())
            {
                //connect.connect.Open();
                using (SqlDataAdapter ada = new SqlDataAdapter("select ZoneName from tblZone where AgencyCode ='" + id + "' ", Logic.ConnectionString ))
                {
                    ada.Fill(ds, "table");
                }
                //connect.connect.Close();
                dt = ds.Tables[0];
                gridControl1.DataSource = dt.DefaultView;
            }
            gridView1.OptionsBehavior.Editable = false;
            gridView1.BestFitColumns();
        }

        void cboAgency_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboAgency.SelectedValue != null && !isFirst)
            {
                setReload(Convert.ToInt32(cboAgency.SelectedValue.ToString()));
                //setDBComboBox1(Convert.ToInt32(cboAgency.SelectedValue.ToString()));
                //cboZone.SelectedIndex = -1;
                //setDBComboBox3(Convert.ToInt32(cboAgency.SelectedValue.ToString()));

            }
        }

        private void Lockfield()
        {
            txtaddress.Enabled = false;
            txtEmail.Enabled = false;
            txtFax.Enabled = false;
            txtHead.Enabled = false;
            txtPhone.Enabled = false;
            txtzone.Enabled = false;
        }

        private void Unlockfield()
        {
            txtaddress.Enabled = true;
            txtEmail.Enabled = true;
            txtFax.Enabled = true;
            txtHead.Enabled = true;
            txtPhone.Enabled = true;
            txtzone.Enabled = true;
        }

    }
}
