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
    public partial class FrmBussSubClass : Form
    {
        public static FrmBussSubClass publicStreetGroup;

        protected TransactionTypeCode iTransType;

        protected bool boolIsUpdate;

        protected int ID;

        bool isFirst = true;

        public FrmBussSubClass()
        {
            InitializeComponent();

            //connect.ConnectionString();

            publicStreetGroup = this;
            
            setImages();

            ToolStripEvent();

            iTransType = TransactionTypeCode.New ;
                     
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

        void gridView1_DoubleClick(object sender, EventArgs e)
        {
            tsbEdit.PerformClick();
        }

        void OnFormLoad(object sender, EventArgs e)
        {
            ShowForm();
            setDBComboBox();
            isFirst = false;
            //setReload();
            cboBusiness.SelectedIndexChanged += cboBusiness_SelectedIndexChanged;
            cboBusiness_SelectedIndexChanged(null, null);

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
                string query = "select *  from tblBusinessClass";
                using (SqlDataAdapter ada = new SqlDataAdapter(query, Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }

                Dt = ds.Tables[0];
            }

            Common.setComboList(cboBusiness, Dt, "BusinessID", "BusinessName");

            cboBusiness.SelectedIndex = -1;

            //connect.connect.Close();
        }

        private void Clear()
        {
            txtClass.Clear();
            //setDBComboBox();

        }

        private bool FillField(int fieldid)
        {
            bool bResponse = false;
            ////load data from the table into the forms for edit
            //DataTable dts = extMethods.LoadData(String.Format("select * from ViewBusinessSubClass where BusinessSubClassID ='{0}'", fieldid));
            DataTable dts = (new Logic()).getSqlStatement((String.Format("select * from ViewBusinessSubClass where BusinessSubClassID ='{0}'", fieldid))).Tables[0];


            if (dts != null)
            {
                bResponse = true;
                txtClass.Text = dts.Rows[0]["BusinessSubName"].ToString();
                cboBusiness.Text = dts.Rows[0]["BusinessName"].ToString();
            }
            else
                bResponse = false;

            return bResponse;
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
                //Unlockfield();
                boolIsUpdate = false;
            }
            else if (sender == tsbEdit)
            {
                groupControl2.Text = "Edit Record Mode";
                iTransType = TransactionTypeCode.Edit;
                if (EditRecordMode())
                {
                    ShowForm();
                    //Unlockfield();
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

        protected bool EditRecordMode()
        {
            bool bResponse = false;
            GridView view = (GridView)gridControl1.FocusedView;
            if (view != null)
            {
                DataRow dr = view.GetDataRow(view.FocusedRowHandle);
                if (dr != null)
                {
                    ID = Convert.ToInt32(dr["BusinessSubClassID"]);
                    bResponse = FillField(Convert.ToInt32(dr["BusinessSubClassID"]));
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
                if (txtClass.Text == "")
                {
                    Common.setEmptyField("Busines Class Description ", Program.ApplicationName);
                    txtClass.Focus();
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

                                using (SqlCommand sqlCommand1 = new SqlCommand(String.Format("INSERT INTO [tblBusinessSubClass]([BusinessSubName],[BusinessID]) VALUES ('{0}','{1}');",txtClass.Text.Trim().ToUpperInvariant(), Convert.ToInt32(cboBusiness.SelectedValue.ToString())), db, transaction))
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

                        setReload(Convert.ToInt32(cboBusiness.SelectedValue.ToString()));

                        Common.setMessageBox("Record has been successfully added", Program.ApplicationName, 1);

                        if (MessageBox.Show("Do you want to add another record?", Program.ApplicationName, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
                        {
                            bttnCancel.PerformClick();
                        }
                        else
                        {
                            //bttnReset.PerformClick();
                            Clear(); cboBusiness.Focus();
                           // setReload(Convert.ToInt32(cboBusiness.SelectedValue.ToString()));
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
                                using (SqlCommand sqlCommand1 = new SqlCommand(String.Format(String.Format("UPDATE [tblBusinessSubClass] SET [BusinessSubName]='{{0}}',[BusinessID] ='{{1}}' where  BusinessSubClassID ='{0}'", ID), txtClass.Text.Trim().ToUpperInvariant(), Convert.ToInt32(cboBusiness.SelectedValue.ToString())), db, transaction))
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
                        setReload(Convert.ToInt32(cboBusiness.SelectedValue.ToString()));

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

        public void RefreshForm()
        {
            NavBars.ToolStripEnableDisableControls(toolStrip, Tag as String);
        }

        void cboBusiness_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboBusiness.SelectedValue != null && !isFirst)
            {
                setReload(Convert.ToInt32(cboBusiness.SelectedValue.ToString()));
                //setDBComboBox1(Convert.ToInt32(cboAgency.SelectedValue.ToString()));
                //cboZone.SelectedIndex = -1;
                //setDBComboBox3(Convert.ToInt32(cboAgency.SelectedValue.ToString()));

            }
        }

        private void setReload(int parameter)
        {
            //connect.connect.Close();
            DataTable dt;
            //System.Data.DataSet ds;
            using (var ds = new System.Data.DataSet())
            {
                //connect.connect.Open();
                using (SqlDataAdapter ada = new SqlDataAdapter(String.Format("select * from tblBusinessSubClass where BusinessID ='{0}", parameter) + "' ", Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }
                //connect.connect.Close();
                dt = ds.Tables[0];
                gridControl1.DataSource = dt.DefaultView;
            }
            gridView1.OptionsBehavior.Editable = false;
            gridView1.Columns["BusinessID"].Visible = false;
            gridView1.Columns["BusinessSubClassID"].Visible = false;
            gridView1.BestFitColumns();
        }

    }
}
