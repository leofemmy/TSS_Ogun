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
    public partial class FrmReceipts : Form
    {
        public static FrmReceipts publicStreetGroup;

        string modulesAccess, modulesAccess1;

        protected TransactionTypeCode iTransType;

        protected bool boolIsUpdate;

        protected int ID;

        private String[] split2;

        public FrmReceipts()
        {
            InitializeComponent();

            publicStreetGroup = this;

            setImages();

            ToolStripEvent();

            iTransType = TransactionTypeCode.Null;

            Load += OnFormLoad;

            gridView1.DoubleClick += gridView1_DoubleClick;

            bttnCancel.Click += Bttn_Click;

            //bttnReset.Click += Bttn_Click;

            bttnUpdate.Click += Bttn_Click;

            cboBranch.EditValueChanged += cboBranch_EditValueChanged;

            OnFormLoad(null, null);

        }

        void cboBranch_EditValueChanged(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            string values = string.Empty;

            object val = cboBranch.EditValue;

            object[] lol = val.ToString().Split(',');

            int i = 0;

            foreach (object obj in lol)
            {
                values += String.Format("{0}", obj.ToString().Trim());

                if (i + 1 < lol.Count())

                    values += ",";

                ++i;
            }

            modulesAccess1 = values.ToString();
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
            //else if (sender == tsbDelete)
            //{
            //    groupControl2.Text = "Delete Record Mode";
            //    iTransType = TransactionTypeCode.Delete;
            //    if (MosesClassLibrary.Utilities.Common.AskQuestion("Deleting this record will delete attached record.\nDo you want to continue?", ""))
            //    {
            //    }
            //    else
            //        tsbReload.PerformClick();
            //    boolIsUpdate = false;
            //}
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
            setReload();
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
            //        FillField(IDs);
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

        public void setDBComboBox()
        {
            DataTable Dt;

            using (var ds = new System.Data.DataSet())
            {

                string query = "select BankName + ' - '+ BranchName as description, BranchID from ViewBankBranch";

                using (SqlDataAdapter ada = new SqlDataAdapter(query, Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }

                Dt = ds.Tables[0];
            }

            Common.setCheckEdit(cboBranch, Dt, "BranchID", "description");



        }

        public void setDBComboBoxBranch(string parameter)
        {
            DataTable Dt;

            if (parameter == null || parameter == "")
            {
                return;
            }
            else
            {
                using (var ds = new System.Data.DataSet())
                {

                    string query = String.Format("select BranchID, BranchName from tblBankBranch where BankShortCode in (select * from [dbo].[fnConvertCSVToVarchar]('{0}'))", parameter);

                    using (SqlDataAdapter ada = new SqlDataAdapter(query, Logic.ConnectionString))
                    {
                        ada.Fill(ds, "table");
                    }

                    Dt = ds.Tables[0];
                }

                Common.setCheckEdit(cboBranch, Dt, "BranchID", "BranchName");
            }


        }

        public void RefreshForm()
        {
            NavBars.ToolStripEnableDisableControls(toolStrip, Tag as String);
        }

        private void UpdateRecord()
        {
            try
            {
                if (txtCentreName.Text == "")
                {
                    Common.setEmptyField("Centre Name ", Program.ApplicationName);
                    txtCentreName.Focus();
                }
                else if (cboBranch.EditValue.ToString() == null || cboBranch.EditValue.ToString() == "")
                {
                    Common.setEmptyField("Bank Branch ", Program.ApplicationName);
                    cboBranch.Focus();
                }
                else if (txtCPerson.Text == "" || txtCPerson.Text == null)
                {
                    Common.setEmptyField("Contact Person", Program.ApplicationName);
                    txtCPerson.Focus();
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
                                ////insert records into centre table table
                                int recs = Convert.ToInt32(new SqlCommand(String.Format("INSERT INTO [tblReceiptCentre]([CentreName],[CAddress],[CPhone],[CPerson],[ContPhone]) VALUES ('{0}','{1}','{2}','{3}','{4}');SELECT @@IDENTITY", txtCentreName.Text.Trim().ToUpperInvariant(), txtCAddress.Text.Trim(), txtCPhone.Text.Trim(), txtCPerson.Text.Trim(), txtContPhone.Text.Trim()), db, transaction).ExecuteScalar());

                                ////splite the bank branch code
                                split2 = cboBranch.EditValue.ToString().Split(',');

                                //count d number of split
                                for (int j = 0; j < split2.Count(); j++)
                                {
                                    //insert revenue code into table with revenue office id
                                    using (SqlCommand sqlCommand = new SqlCommand(String.Format("INSERT INTO [tblReceiptBankBranch]([BranchID],[CentreCode]) VALUES ('{0}', '{1}');", Convert.ToString(split2[j]), recs), db, transaction))
                                    {
                                        sqlCommand.ExecuteNonQuery();
                                    }
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
                            //bttnCancel.PerformClick();
                            tsbReload.PerformClick();
                        }
                        else
                        {
                            //bttnReset.PerformClick();
                            Clear(); txtCentreName.Focus();
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
                                //[tblReceiptCentre]([CentreName])
                                using (SqlCommand sqlCommand1 = new SqlCommand(String.Format(String.Format("UPDATE [tblReceiptCentre] SET [CentreName]='{{0}}',[CAddress]='{{1}}',[CPhone]='{{2}}',[CPerson]='{{3}}',[ContPhone]='{{4}}' where  ReceiptsId ='{0}'", ID), txtCentreName.Text.Trim().ToUpperInvariant(), txtCAddress.Text.Trim(), txtCPhone.Text.Trim(), txtCPerson.Text.Trim(), txtContPhone.Text.Trim()), db, transaction))
                                {
                                    sqlCommand1.ExecuteNonQuery();
                                }

                                ////splite the bank branch code
                                split2 = cboBranch.EditValue.ToString().Split(',');

                                ////update table TownLGARelations after Modify records
                                ////count d number of split
                                for (int j = 0; j < split2.Count(); j++)
                                {
                                    //[tblReceiptBankBranch]([BranchID],[CentreCode])
                                    using (SqlCommand sqlCommand = new SqlCommand(String.Format(String.Format("UPDATE [tblReceiptBankBranch] SET [BranchID]='{{0}}',[CentreCode]='{{1}}' where  CentreCode ='{0}'", ID), Convert.ToString(split2[j]), ID), db, transaction))
                                    {
                                        sqlCommand.ExecuteNonQuery();
                                    }
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

                        Common.setMessageBox("Changes in record has been successfully saved.", Program.ApplicationName, 1);

                        //bttnCancel.PerformClick();
                        tsbReload.PerformClick();

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
            txtCentreName.Clear(); txtCAddress.Clear(); txtCPhone.Clear(); txtCPerson.Clear(); txtContPhone.Clear();
            setDBComboBox();

        }

        private void setReload()
        {

            DataTable dt;
            //System.Data.DataSet ds;
            using (var ds = new System.Data.DataSet())
            {

                using (SqlDataAdapter ada = new SqlDataAdapter("SELECT ReceiptsId,CentreCode,CentreName,CAddress,CPerson,ContPhone,CPhone  from tblReceiptCentre ", Logic.ConnectionString))
                //using (SqlDataAdapter ada = new SqlDataAdapter("select * from tblTown ", connect.ConnectionString()))
                {
                    ada.Fill(ds, "table");
                }

                dt = ds.Tables[0];
                this.gridControl1.DataSource = dt.DefaultView;
            }
            this.gridView1.OptionsBehavior.Editable = false;
            gridView1.Columns["ReceiptsId"].Visible = false;
            gridView1.Columns["CPhone"].Visible = false;
            //gridView1.Columns["ContPhone"].GroupIndex = 0;
            //gridView1.Columns["CAddress"].GroupIndex = 1;
            ////gridView1.Columns["OfficeName"].GroupIndex = 2;
            gridView1.Columns["ContPhone"].Visible = false;
            gridView1.Columns["CPhone"].Visible = false;
            this.gridView1.BestFitColumns();
        }

        void gridView1_DoubleClick(object sender, EventArgs e)
        {
            tsbEdit.PerformClick();
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
                    ID = Convert.ToInt32(dr["ReceiptsId"]);
                    txtCentreName.Text = dr["CentreName"].ToString();
                    txtCAddress.Text = dr["CAddress"].ToString();
                    txtCPhone.Text = dr["CPhone"].ToString();
                    txtCPerson.Text = dr["CPerson"].ToString();
                    txtContPhone.Text = dr["ContPhone"].ToString();
                    bResponse = FillField(dr["ReceiptsId"].ToString());
                    //bResponse = true;
                }
                else
                {
                    Common.setMessageBox("No Record is seleected", "", 1);
                    bResponse = false;
                }
            }
            return bResponse;
        }

        private bool FillField(String fieldid)
        {
            bool bResponse = false;

            //load data from the table into the forms for edit
            string query = String.Format("select description, BranchID from ViewReceipCentreBranch where centrecode ='{0}'", fieldid);
            //DataTable dts = extMethods.LoadData(String.Format("select * from tblTown where TownCode ='{0}'", fieldid));

            DataTable dts = (new Logic()).getSqlStatement(query).Tables[0];

            if (dts != null)
            {
                bResponse = true;
                //txtTown.Text = dts.Rows[0]["TownName"].ToString();
                cboBranch.EditValue = dts.Rows[0]["BranchID"] + ", " + dts.Rows[1]["BranchID"];
                cboBranch.Refresh();

                Refresh();
                groupControl2.Refresh();

            }
            else
                bResponse = false;

            return bResponse;
        }



    }
}
