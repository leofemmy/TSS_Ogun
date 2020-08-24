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
using Inventory.Class;
using DevExpress.XtraGrid.Views.Grid;

namespace Inventory.Forms
{
    public partial class FrmRegMLO : Form
    {
        public static FrmRegMLO publicStreetGroup;

        protected TransactionTypeCode iTransType;

        protected bool boolIsUpdate;

        protected int ID;

        bool isFirst = true;

        public FrmRegMLO()
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

        void OnFormLoad(object sender, EventArgs e)
        {
            ShowForm();

            isFirst = false;
            
            setReload();

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
            bttnReset.PerformClick();
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
            }
            else if (sender == bttnUpdate)
            {
                UpdateRecord();
            }
        }

        private void Clear()
        {
            txtAddress.Clear(); txtName.Clear(); txtTelephone.Clear();
            txtLocation.Clear(); txtCTelephone.Clear(); txtCPerson.Clear();
        }

        private bool FillField(int fieldid)
        {
            bool bResponse = false;

            DataTable dts = (new Logic()).getSqlStatement((String.Format("select * from [tblRegMLO] where MLOID ='{0}'", fieldid))).Tables[0];

            if (dts != null)
            {
                bResponse = true;
                txtName.Text = dts.Rows[0]["Name"].ToString();
                txtTelephone.Text = dts.Rows[0]["Telephone"].ToString();
                txtLocation.Text = dts.Rows[0]["Location"].ToString();
                txtEmail.Text = dts.Rows[0]["Email"].ToString();
                txtCTelephone.Text = dts.Rows[0]["Cphone"].ToString();
                txtCPerson.Text = dts.Rows[0]["CPerson"].ToString();
                txtAddress.Text = dts.Rows[0]["Address"].ToString();
                    
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
                    ID = Convert.ToInt32(dr["MLOID"]);
                    bResponse = FillField(Convert.ToInt32(dr["MLOID"]));
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
                if (txtAddress.Text == "")
                {
                    Common.setEmptyField("MLO Address ", Program.ApplicationName);
                   txtAddress.Focus();
                    return;
                }
                else if (txtCPerson.Text == "")
                {
                    Common.setEmptyField("MLO Contact Person ", Program.ApplicationName);
                    txtCPerson.Focus(); return;

                }
                else if (txtCTelephone.Text == "")
                {
                    Common.setEmptyField("MLO Contact Person Phone ", Program.ApplicationName);
                    txtCTelephone.Focus(); return;

                }
                else if (txtLocation.Text == "")
                {
                    Common.setEmptyField("MLO Location ", Program.ApplicationName);
                    txtLocation.Focus(); return;

                }
                else if (txtName.Text == "")
                {
                    Common.setEmptyField("MLO Name ", Program.ApplicationName);
                    txtName.Focus(); return;

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

                                using (SqlCommand sqlCommand1 = new SqlCommand(String.Format("INSERT INTO [tblRegMLO]([Name],[Location],[Address],[Telephone],[Email],[CPerson],[Cphone])VALUES ('{0}','{1}','{2}','{3}','{4}','{5}','{6}');", txtName.Text.Trim().ToUpperInvariant(), txtLocation.Text.Trim(), txtAddress.Text.Trim(), txtTelephone.Text.Trim(), txtEmail.Text.Trim(), txtCPerson.Text.Trim(), txtCTelephone.Text.Trim()), db, transaction))
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
                            setReload();
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
                                using (SqlCommand sqlCommand1 = new SqlCommand(String.Format(String.Format("UPDATE [tblRegMLO] SET [Name]='{{0}}',[Location]='{{1}}',[Address]='{{2}}',[Telephone]='{{3}}',[Email]='{{4}}',[CPerson]='{{5}}',[Cphone]='{{6}}' where  MLOID ='{0}'", ID), txtName.Text.Trim().ToUpperInvariant(), txtLocation.Text.Trim(), txtAddress.Text.Trim(), txtTelephone.Text.Trim(), txtEmail.Text.Trim(), txtCPerson.Text.Trim(), txtCTelephone.Text.Trim()), db, transaction))

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
            DataTable dt;

            using (var ds = new System.Data.DataSet())
            {
                using (SqlDataAdapter ada = new SqlDataAdapter("select * from [tblRegMLO]", Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }
                dt = ds.Tables[0];
                gridControl1.DataSource = dt.DefaultView;
            }
            gridView1.OptionsBehavior.Editable = false;
            gridView1.Columns["MLOID"].Visible = false;
            gridView1.BestFitColumns();
        }

    }
}
