using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using TaxSmartSuite.Class;
using System.Data.SqlClient;
using DevExpress.XtraGrid.Views.Grid;
using AgencyControl.Class;

namespace AgencyControl.Forms
{
    public partial class FrmRevenueTaxOffice : Form
    {
        //DBConnection connect = new DBConnection();

        //Methods extMethods = new Methods();

        public static FrmRevenueTaxOffice publicStreetGroup;

        protected TransactionTypeCode iTransType;

        protected bool boolIsUpdate;

        string modulesAccess;

        string[] split2;

        protected int ID;

        bool isFirst = true;

        public FrmRevenueTaxOffice()
        {
            InitializeComponent();

            //connect.ConnectionString();

            publicStreetGroup = this;

            setImages();
            
            ToolStripEvent();

            iTransType = TransactionTypeCode.New;

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
                //iTransType = TransactionTypeCode.New;
                ShowForm();
                
                Clear();
                
                Lockfield();
            }
            bttnReset.PerformClick();
        }

        void OnFormLoad(object sender, EventArgs e)
        {
            ShowForm();
            
            setDBComboBox();
            
            setDBComboBox2();
            //setReload(0);
            isFirst = false;
            //cboAgency_SelectedIndexChanged_2(null, null);
            cboAgency.SelectedIndexChanged += cboAgency_SelectedIndexChanged;
            
            cboAgency_SelectedIndexChanged(null, null);
            
            cboNature.SelectedIndex = -1;

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
                using (SqlDataAdapter ada = new SqlDataAdapter(query, Logic.ConnectionString ))
                {
                    ada.Fill(ds, "table");
                }

                Dt = ds.Tables[0];
            }

            Common.setComboList(cboAgency, Dt, "AgencyCode", "AgencyName");
            
        }

        public void setDBComboBox1(int fId)
        {
            DataTable Dt;

            using (var ds = new System.Data.DataSet())
            {
            
                string query = String.Format("select *  from tblZone where AgencyCode = '{0}' ", fId);

                using (SqlDataAdapter ada = new SqlDataAdapter(query, Logic.ConnectionString ))
                {
                    ada.Fill(ds, "table");
                }

                Dt = ds.Tables[0];
            }

            Common.setComboList(cboZone, Dt, "ZoneID", "ZoneName");
            
        }

        public void setDBComboBox2()
        {
            DataTable Dt;

            using (var ds = new System.Data.DataSet())
            {
               string query = "select *  from tblOperationModes";

                using (SqlDataAdapter ada = new SqlDataAdapter(query, Logic.ConnectionString ))
                {
                    ada.Fill(ds, "table");
                }

                Dt = ds.Tables[0];
            }

            Common.setComboList(cboNature, Dt, "OperationID", "Description");
        }

        public void setDBComboBox3(int fId)
        {
            DataTable Dt;

           using (var ds = new System.Data.DataSet())
            {
        
                string query = String.Format("select *  from tblRevenueType where AgencyCode = '{0}' ", fId);

                using (SqlDataAdapter ada = new SqlDataAdapter(query, Logic.ConnectionString ))
                {
                    ada.Fill(ds, "table");
                }

                Dt = ds.Tables[0];
            }

            //populate checkeditcombobox
            Common.setCheckEdit(chkEditRevenue, Dt, "RevenueCode", "Description");
            
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
                ////setReload();
            }
            else if (sender == bttnUpdate)
            {
                UpdateRecord();
            }
        }

        private void Lockfield()
        {
            cboNature.Enabled = false;
            cboZone.Enabled = false;
            txtAddress.Enabled = false;
            txtEmail.Enabled = false;
            txtFaxno.Enabled = false;
            txtHead.Enabled = false;
            txtPhone.Enabled = false;
            txtRevOffice.Enabled = false;
            chkEditRevenue.Enabled = false;
        }

        private void Unlockfield()
        {
            cboNature.Enabled = true;
            cboZone.Enabled = true;
            txtAddress.Enabled = true;
            txtEmail.Enabled = true;
            txtFaxno.Enabled = true;
            txtHead.Enabled = true;
            txtPhone.Enabled = true;
            txtRevOffice.Enabled = true;
            chkEditRevenue.Enabled = true;
        }

        private void UpdateRecord()
        {
            try
            {
                if (txtRevOffice.Text == "")
                {
                    Common.setEmptyField("Rev. Office Name ", Program.ApplicationName);
                    txtRevOffice.Focus(); return;
                }
                else if (txtAddress.Text == "")
                {
                    Common.setEmptyField("Rev. Office Address ", Program.ApplicationName);
                    txtAddress.Focus(); return;
                }
                else if (cboAgency.SelectedValue.ToString() == null || cboAgency.SelectedValue.ToString() == "")
                {
                    Common.setEmptyField("Parent Agency Field ", Program.ApplicationName);
                    cboAgency.Focus(); return;
                }
                else if (cboNature.SelectedValue.ToString() == null || cboNature.SelectedValue.ToString() == "")
                {
                    Common.setEmptyField("Nature of Operation Field ", Program.ApplicationName);
                    cboNature.Focus(); return;
                }
                else if (cboZone.SelectedValue.ToString() == null || cboZone.SelectedValue.ToString() == "")
                {
                    Common.setEmptyField("Zone Field ", Program.ApplicationName);
                    cboZone.Focus(); return;
                }
                else if (chkEditRevenue.EditValue.ToString() == null || chkEditRevenue.EditValue.ToString() == "")
                {
                    Common.setEmptyField("Revenue Operation Field ", Program.ApplicationName);
                    chkEditRevenue.Focus(); return;
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

                                //new insert method here
                                int recs = Convert.ToInt32(new SqlCommand(String.Format("INSERT INTO [tblRevenueOffice]([AgencyCode],[ZoneID],[OperationID],[OfficeName],[Address],[Telephone],[FaxNumber],[EmailAddress],[HeadOfStation]) VALUES ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}');SELECT @@IDENTITY", Convert.ToInt32(cboAgency.SelectedValue.ToString()), Convert.ToInt32(cboZone.SelectedValue.ToString()), Convert.ToInt32(cboNature.SelectedValue.ToString()), txtRevOffice.Text.Trim().ToUpperInvariant(), txtAddress.Text.Trim(), txtPhone.Text.Trim(), txtFaxno.Text.Trim(), txtEmail.Text.Trim(), txtHead.Text.Trim().ToUpperInvariant()), db, transaction).ExecuteScalar());

                                //splite the Revenue code
                                split2 = modulesAccess.Split(',');

                                //count d number of split
                                for (int j = 0; j < split2.Count(); j++)
                                {
                                    //insert revenue code into table with revenue office id
                                    using (SqlCommand sqlCommand = new SqlCommand(String.Format("INSERT INTO [tblRevenueOfficeRevenueTypeRelation]([RevenueCode],[RevenueOfficeID]) VALUES ('{0}', '{1}');", Convert.ToString(split2[j]), recs), db, transaction))
                                    {
                                        sqlCommand.ExecuteNonQuery();
                                    }
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
                                
                                //update revenue office table
                                using (SqlCommand sqlCommand1 = new SqlCommand(String.Format(String.Format(" UPDATE [tblRevenueOffice] SET [AgencyCode] = '{{0}}',[ZoneID] = '{{1}}',[OperationID] = '{{2}}',[OfficeName] = '{{3}}' ,[Address] = '{{4}}',[Telephone] = '{{5}}',[FaxNumber] = '{{6}}',[EmailAddress] = '{{7}}',[HeadOfStation] = '{{8}}' where RevenueOfficeID ='{0}'", ID), Convert.ToInt32(cboAgency.SelectedValue.ToString()), Convert.ToInt32(cboZone.SelectedValue.ToString()), Convert.ToInt32(cboNature.SelectedValue.ToString()), txtRevOffice.Text.Trim().ToUpperInvariant(), txtAddress.Text.Trim(), txtPhone.Text.Trim(), txtFaxno.Text.Trim(), txtEmail.Text.Trim(), txtHead.Text.Trim().ToUpperInvariant()), db, transaction))

                                {
                                    sqlCommand1.ExecuteNonQuery();
                                }

                                //splite the Revenue code
                                split2 = modulesAccess.Split(',');

                                //count d number of split
                                for (int j = 0; j < split2.Count(); j++)
                                {
                                     //update revenue office code table

                                    using (SqlCommand sqlCommand = new SqlCommand(String.Format(String.Format(" UPDATE [tblRevenueOfficeRevenueTypeRelation] SET [RevenueCode] = '{{0}}' where RevenueOfficeID ='{0}'", ID), Convert.ToString(split2[j])), db, transaction))
                                    {
                                        sqlCommand.ExecuteNonQuery();
                                    }

                                }

                               transaction.Commit();
                            }
                            catch (SqlException e)
                            {
                                try
                                {
                                    transaction.Rollback();
                                }
                                catch (SqlException ex)
                                {
                                    if (transaction.Connection != null)
                                    {
                                        Tripous.Sys.ErrorBox(ex);
                                    }
                                }


                            }
                            finally
                            {
                                db.Close();
                            }

                        }

                       //setReload();
                        setReload(Convert.ToInt32(cboAgency.SelectedValue.ToString()));

                        Common.setMessageBox("Changes in record has been successfully saved.", Program.ApplicationName, 1);
                        bttnReset.PerformClick();
                        //}
                    }
                }
            }
            catch (Exception ex)
            {
                Tripous.Sys.ErrorBox(ex);
            }

        }

        private void Clear()
        {
            txtAddress.Clear();
            txtEmail.Clear();
            txtFaxno.Clear();
            txtHead.Clear();
            txtPhone.Clear();
            txtRevOffice.Clear();

        }

        void gridView1_DoubleClick(object sender, EventArgs e)
        {
            tsbEdit.PerformClick();
        }

        private bool FillField(int fieldid)
        {
            bool bResponse = false;
            //load data from the table into the forms for edit

            DataTable dts = (new Logic()).getSqlStatement((String.Format("select * from ViewRevenueOffice where RevenueOfficeID ='{0}'", fieldid))).Tables[0];

            if (dts != null)
            {
                bResponse = true;
                cboAgency.Text = dts.Rows[0]["AgencyName"].ToString();
                cboZone.Text = dts.Rows[0]["ZoneName"].ToString();
                txtAddress.Text = dts.Rows[0]["Address"].ToString();
                txtEmail.Text = dts.Rows[0]["EmailAddress"].ToString();
                txtFaxno.Text = dts.Rows[0]["FaxNumber"].ToString();
                txtHead.Text = dts.Rows[0]["HeadOfStation"].ToString();
                txtRevOffice.Text = dts.Rows[0]["OfficeName"].ToString();
                txtPhone.Text = dts.Rows[0]["Telephone"].ToString();
                cboNature.Text = dts.Rows[0]["Description"].ToString();
            }
            else
                bResponse = false;

            return bResponse;
        }

        private void setReload(int paramid)
        {
            DataTable dt;

            using (var ds = new System.Data.DataSet())
            {
                using (SqlDataAdapter ada = new SqlDataAdapter(String.Format("select * from ViewRevenueOffice where AgencyCode= '{0}'", paramid), Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }

                dt = ds.Tables[0];
                gridControl1.DataSource = dt.DefaultView;
            }
            gridView1.OptionsBehavior.Editable = false;
            gridView1.Columns["AgencyCode"].Visible = false;
            gridView1.Columns["AgencyName"].Visible = false;
            gridView1.Columns["RevenueOfficeID"].Visible = false;
            gridView1.Columns["ZoneID"].Visible = false;
            gridView1.Columns["OperationID"].Visible = false;
            gridView1.Columns["FaxNumber"].Visible = false;
            gridView1.Columns["EmailAddress"].Visible = false;
            gridView1.Columns["Telephone"].Visible = false;

            gridView1.BestFitColumns();
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
                    ID = Convert.ToInt32(dr["RevenueOfficeID"]);
                
                    bResponse = FillField(Convert.ToInt32(dr["RevenueOfficeID"]));
                }
                else
                {
                    Common.setMessageBox("No Record is seleected", "", 1);
                   
                    bResponse = false;
                }
            }
            return bResponse;
        }

        private void cboAgency_KeyPress(object sender, KeyPressEventArgs e)
        {
            Methods.AutoComplete(cboAgency, e, true);
        }

        private void cboZone_KeyPress(object sender, KeyPressEventArgs e)
        {
            Methods.AutoComplete(cboZone, e, true);
        }

        private void cboNature_KeyPress(object sender, KeyPressEventArgs e)
        {
            Methods.AutoComplete(cboNature, e, true);

        }

        private void cboAgency_Leave(object sender, EventArgs e)
        {
            //setDBComboBox1(Convert.ToInt32(cboAgency.SelectedValue.ToString()));
            //setDBComboBox3(Convert.ToInt32(cboAgency.SelectedValue.ToString()));
            //setReload(Convert.ToInt32(cboAgency.SelectedValue.ToString()));
            //splitContainer1.Panel2Collapsed = false;
        }

        private void chkEditRevenue_EditValueChanged(object sender, EventArgs e)
        {
            string values = string.Empty;

            object val = chkEditRevenue.EditValue;

            object[] lol = val.ToString().Split(',');

            int i = 0;
           
            foreach (object obj in lol)
            {
            
                values += String.Format("{0}", obj.ToString().Trim());
               
                if (i + 1 < lol.Count())
                
                    values += ",";
               
                ++i;
            }

            modulesAccess = values.ToString();
        }

        void cboAgency_SelectedIndexChanged(object sender, EventArgs e)
          {
              if (cboAgency.SelectedValue != null && !isFirst)
              {
                 
                  setReload(Convert.ToInt32(cboAgency.SelectedValue.ToString()));
                 
                  setDBComboBox1(Convert.ToInt32(cboAgency.SelectedValue.ToString()));
                 
                  cboZone.SelectedIndex = -1;
                 
                  setDBComboBox3(Convert.ToInt32(cboAgency.SelectedValue.ToString()));
                  
              }
          }
    }
}
