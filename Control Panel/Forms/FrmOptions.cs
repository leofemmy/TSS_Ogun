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


namespace Control_Panel.Forms
{
    public partial class FrmOptions : Form
    {

        public static FrmOptions publicStreetGroup;

        protected TransactionTypeCode iTransType;

        protected bool boolIsUpdate;

        protected int ID;

        int Cashid;

        bool isFirst = true;

        public FrmOptions()
        {
            InitializeComponent();

            publicStreetGroup = this;

            setImages();

            ToolStripEvent();

            iTransType = TransactionTypeCode.New;

            Load += OnFormLoad;
            
            OnFormLoad(null, null);

            bttnCancel.Click += Bttn_Click;

            bttnUpdate.Click += Bttn_Click;
        }

        private void setImages()
        {
            tsbNew.Image = MDIMain.publicMDIParent.i16x16.Images[5];
            tsbEdit.Image = MDIMain.publicMDIParent.i16x16.Images[6];
            tsbDelete.Image = MDIMain.publicMDIParent.i16x16.Images[7];
            tsbReload.Image = MDIMain.publicMDIParent.i16x16.Images[0];
            tsbClose.Image = MDIMain.publicMDIParent.i16x16.Images[11];

            bttnCancel.Image = MDIMain.publicMDIParent.i32x32.Images[9];
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
                //groupControl2.Text = "Add New Record";
                iTransType = TransactionTypeCode.New;
                ShowForm();
                boolIsUpdate = false;
            }
            else if (sender == tsbEdit)
            {
                //groupControl2.Text = "Edit Record Mode";
                iTransType = TransactionTypeCode.New;
                //if (EditRecordMode())
                //{
                ShowForm();
                boolIsUpdate = false;
                //}
            }
            else if (sender == tsbDelete)
            {
                //groupControl2.Text = "Delete Record Mode";
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
            
            isFirst = false;
            
            setReload();

            setDBComboBox();

            CheckData();

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

            DataTable dt;
            //System.Data.DataSet ds;
            using (var ds = new System.Data.DataSet())
            {

                using (SqlDataAdapter ada = new SqlDataAdapter("SELECT * FROM ViewCashOptionRevenue", Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }

                dt = ds.Tables[0];
                //gridControl1.DataSource = dt.DefaultView;
            }
         

            if (dt != null && dt.Rows.Count > 0)
            {
                cboRevenue.Text = dt.Rows[0][0].ToString();
                txtName.Text = dt.Rows[0][1].ToString();
                txtDescription.Text = dt.Rows[0][2].ToString();
                txtCode.Text = dt.Rows[0][3].ToString();
                txtReceipt.Text = dt.Rows[0][4].ToString();
                txtIssuing.Text = dt.Rows[0][5].ToString();
                txtShortname.Text = dt.Rows[0][6].ToString();
                txtWebSite.Text = dt.Rows[0][7].ToString();
                txtConfirmation.Text = dt.Rows[0][8].ToString();
                if (Convert.ToBoolean(dt.Rows[0][9]) ==true)
                {
                    chkRecepit.Checked = true;
                }
                else
                    chkRecepit.Checked = false;
            }
        }

        public void setDBComboBox()
        {
            DataTable Dt;

            //connect.connect.Close();

            using (var ds = new System.Data.DataSet())
            {
                //connect.connect.Open();
                using (SqlDataAdapter ada = new SqlDataAdapter("SELECT Description,RevenueCode FROM dbo.tblRevenueType", Logic.ConnectionString))

                {
                    ada.Fill(ds, "table");
                }

                Dt = ds.Tables[0];
            }

            Common.setComboList(cboRevenue, Dt, "RevenueCode", "Description");

            cboRevenue.SelectedIndex = -1;


        }

        void Bttn_Click(object sender, EventArgs e)
        {
            if (sender == bttnCancel)
            {
                tsbReload.PerformClick();
            }
            else if (sender == bttnUpdate)
            {
                UpdateRecord();
            }
        }

        void UpdateRecord()
        {
          
            if (this.xtraTabControl1.SelectedTabPage == xtPage1)//Revenue Suspense
            {
                //MessageBox.Show("Tab One Clicked");
                if (cboRevenue.Text == null || cboRevenue.Text == "")
                {
                    Common.setEmptyField("Revenue Suspense", Program.ApplicationName);
                    cboRevenue.Focus(); return;
                }
                else
                {
                    if (Cashid != null)
                    {
                        using (SqlConnection db = new SqlConnection(Logic.ConnectionString))
                        {
                            SqlTransaction transaction;

                            db.Open();

                            transaction = db.BeginTransaction();
                            try
                            {
                                using (SqlCommand sqlCommand1 = new SqlCommand(String.Format(String.Format("UPDATE [tblCashOptions] SET [RevSuspence]='{{0}}'  where  CashOptionsID ='{0}'", Cashid), cboRevenue.SelectedValue.ToString()), db, transaction))
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
                    }
                    else
                    {
                        using (SqlConnection db = new SqlConnection(Logic.ConnectionString))
                        {
                            SqlTransaction transaction;

                            db.Open();

                            transaction = db.BeginTransaction();
                            try
                            {

                                using (SqlCommand sqlCommand1 = new SqlCommand(String.Format("INSERT INTO [tblCashOptions]([RevSuspence]) VALUES ('{0}');", cboRevenue.SelectedValue.ToString()), db, transaction))
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
                    }
                }
            }
            else if (this.xtraTabControl1.SelectedTabPage == xtPage2)//eTicket Header
            {
                if (txtName.Text == null || txtName.Text == "")
                {
                    Common.setEmptyField("Authority Name ", Program.ApplicationName);
                    txtName.Focus(); return;
                }
                else if (txtDescription.Text == null || txtDescription.Text == "")
                {
                    Common.setEmptyField("Payment Description ", Program.ApplicationName);
                    txtDescription.Focus(); return;
                }
                else
                {
                    if (Cashid != null)
                    {
                        using (SqlConnection db = new SqlConnection(Logic.ConnectionString))
                        {
                            SqlTransaction transaction;

                            db.Open();

                            transaction = db.BeginTransaction();
                            try
                            {
                                using (SqlCommand sqlCommand1 = new SqlCommand(String.Format(String.Format("UPDATE [tblCashOptions] SET [Authority]='{{0}}',[eTicket]='{{1}}'  where  CashOptionsID ='{0}'", Cashid), txtName.Text.Trim(), txtDescription.Text.Trim()), db, transaction))
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
                    }
                    else
                    {
                        using (SqlConnection db = new SqlConnection(Logic.ConnectionString))
                        {
                            SqlTransaction transaction;

                            db.Open();

                            transaction = db.BeginTransaction();
                            try
                            {

                                using (SqlCommand sqlCommand1 = new SqlCommand(String.Format("INSERT INTO [tblCashOptions]([Authority],[eTicket]) VALUES ('{0}','{1}');", txtName.Text.Trim(), txtDescription.Text.Trim()), db, transaction))
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
                    }
                    
                }
            }
            else if (this.xtraTabControl1.SelectedTabPage == xtPage3)//State PayDirect Reference
            {
               if (txtCode.Text==null || txtCode.Text=="")
               {
                   Common.setEmptyField("State PayDirect Reference Code", Program.ApplicationName);
                   txtCode.Focus(); return;
               }
               else
               {
                   if (Cashid != null)
                   {
                       using (SqlConnection db = new SqlConnection(Logic.ConnectionString))
                       {
                           SqlTransaction transaction;

                           db.Open();

                           transaction = db.BeginTransaction();
                           try
                           {
                               using (SqlCommand sqlCommand1 = new SqlCommand(String.Format(String.Format("UPDATE [tblCashOptions] SET [PDReference]='{{0}}' where  CashOptionsID ='{0}'", Cashid),txtCode.Text.Trim()), db, transaction))
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
                   }
                   else
                   {
                       using (SqlConnection db = new SqlConnection(Logic.ConnectionString))
                       {
                           SqlTransaction transaction;

                           db.Open();

                           transaction = db.BeginTransaction();
                           try
                           {

                               using (SqlCommand sqlCommand1 = new SqlCommand(String.Format("INSERT INTO [tblCashOptions]([PDReference]) VALUES ('{0}');",txtCode.Text.Trim()), db, transaction))
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
                   }
               }
            }
            else if (this.xtraTabControl1.SelectedTabPage == xtPage4)//Electronic Receipts
            {
                if (txtReceipt.Text == null || txtReceipt.Text == "")
                {
                    Common.setEmptyField("Receipt Desrciption", Program.ApplicationName);
                    txtReceipt.Focus(); return;
                }
                else if (txtIssuing.Text == null || txtIssuing.Text == "")
                {
                    Common.setEmptyField("Issuing Authority", Program.ApplicationName);
                    txtIssuing.Focus(); return;
                }
                else if (txtShortname.Text == null || txtShortname.Text == "")
                {
                    Common.setEmptyField("Short Name", Program.ApplicationName);
                    txtShortname.Focus(); return;
                }
                else if (txtWebSite.Text == null || txtWebSite.Text == "")
                {
                    Common.setEmptyField("Web Site Name", Program.ApplicationName);
                    txtWebSite.Focus(); return;
                }
                else if (txtConfirmation.Text == null || txtConfirmation.Text == "")
                {
                    Common.setEmptyField("Confirmation", Program.ApplicationName);
                    txtConfirmation.Focus(); return;
                }
                else
                {
                    if (Cashid != null)
                    {
                        using (SqlConnection db = new SqlConnection(Logic.ConnectionString))
                        {
                            SqlTransaction transaction;

                            db.Open();

                            transaction = db.BeginTransaction();
                            try
                            {
                                using (SqlCommand sqlCommand1 = new SqlCommand(String.Format(String.Format("UPDATE [tblCashOptions] SET [eReceipt]='{{0}}',[IssueAuthor]='{{1}}',[Website]='{{2}}',[AuthoritySN]='{{3}}',[Confirmations]='{{4}}',[ReceiptNo]='{{5}}' where  CashOptionsID ='{0}'", Cashid), txtReceipt.Text.Trim(), txtIssuing.Text.Trim(), txtShortname.Text.Trim(), txtWebSite.Text.Trim(), txtConfirmation.Text.Trim(), chkRecepit.Checked), db, transaction))
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
                    }
                    else
                    {
                        using (SqlConnection db = new SqlConnection(Logic.ConnectionString))
                        {
                            SqlTransaction transaction;

                            db.Open();

                            transaction = db.BeginTransaction();
                            try
                            {

                                using (SqlCommand sqlCommand1 = new SqlCommand(String.Format("INSERT INTO [tblCashOptions]([eReceipt],[IssueAuthor],[Website],[AuthoritySN],[Confirmations],[ReceiptNo]) VALUES ('{0}','{1}','{2}','{3}','{4}','{5}');", txtReceipt.Text.Trim(), txtIssuing.Text.Trim(), txtShortname.Text.Trim(), txtWebSite.Text.Trim(), txtConfirmation.Text.Trim(), chkRecepit.Checked), db, transaction))
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
                    }
                }

                
            }

            Common.setMessageBox("Record has been successfully added", Program.ApplicationName, 1);
        }

        void CheckData()
        {
            DataTable dt;
            //System.Data.DataSet ds;
            using (var ds = new System.Data.DataSet())
            {

                using (SqlDataAdapter ada = new SqlDataAdapter("SELECT CashOptionsID FROM dbo.tblCashOptions", Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }

                dt = ds.Tables[0];

            }
        
            if (dt != null && dt.Rows.Count > 0)
            {
                Cashid =Convert.ToInt32(dt.Rows[0][0].ToString());
         
            }
        }

        public void RefreshForm()
        {
            NavBars.ToolStripEnableDisableControls(toolStrip, Tag as String);
        }

    }
}
