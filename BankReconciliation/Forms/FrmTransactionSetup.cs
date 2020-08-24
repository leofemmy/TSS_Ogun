using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TaxSmartSuite.Class;
using BankReconciliation.Class;
using System.Data.SqlClient;
using DevExpress.XtraGrid.Views.Grid;
using System.Globalization;

namespace BankReconciliation.Forms
{
    public partial class FrmTransactionSetup : Form
    {
        public static FrmTransactionSetup publicStreetGroup;

        protected TransactionTypeCode iTransType;

        protected bool boolIsUpdate;

        protected int ID;

        bool isFirst = true;

        public FrmTransactionSetup()
        {
            InitializeComponent();

            publicStreetGroup = this;

            setImages();

            ToolStripEvent();

            iTransType = TransactionTypeCode.Null;

            Load += OnFormLoad;

            bttnCancel.Click += Bttn_Click;

            bttnUpdate.Click += Bttn_Click;

            txtDescription.LostFocus += txtDescription_LostFocus;

            gridView1.DoubleClick += gridView1_DoubleClick;

            OnFormLoad(null, null);

        }

        void txtDescription_LostFocus(object sender, EventArgs e)
        {
            txtDescription.Text = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(txtDescription.Text);
        }

        private void setImages()
        {
            tsbNew.Image = MDIMains.publicMDIParent.i16x16.Images[5];
            tsbEdit.Image = MDIMains.publicMDIParent.i16x16.Images[6];
            tsbDelete.Image = MDIMains.publicMDIParent.i16x16.Images[7];
            tsbReload.Image = MDIMains.publicMDIParent.i16x16.Images[0];
            tsbClose.Image = MDIMains.publicMDIParent.i16x16.Images[11];

            bttnCancel.Image = MDIMains.publicMDIParent.i32x32.Images[9];
            //bttnReset.Image = MDIMains.publicMDIParent.i32x32.Images[8];
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
                Clear();
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
            ShowForm(); setReload(); setDBComboBox(); isFirst = false;
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
                    splitContainer1.Panel2Collapsed = true;
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
                using (SqlDataAdapter ada = new SqlDataAdapter("SELECT * FROM ViewElementCatwgoryTransDefinition ORDER BY Type, Description", Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }
                //connect.connect.Close();
                dt = ds.Tables[0];
                gridControl1.DataSource = dt.DefaultView;
            }
            gridView1.OptionsBehavior.Editable = false;
            gridView1.Columns["TransID"].Visible = false;
            gridView1.Columns["ElementCatCode"].Visible = false;
            gridView1.BestFitColumns();
        }

        void Bttn_Click(object sender, EventArgs e)
        {
            if (sender == bttnUpdate)
            {
                UpdateRecord();
            }
            else if (sender == bttnCancel)
            {
                tsbReload.PerformClick();
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
                if (String.Compare(txtDescription.Text, "", false) == 0)
                {
                    Common.setEmptyField("Transaction Description ", Program.ApplicationName);
                    txtDescription.Focus(); return;
                }
                else if (radioGroup1.SelectedIndex == -1)
                {
                    Common.setEmptyField("Transaction Type", Program.ApplicationName);
                    return;
                }
                else if (string.IsNullOrEmpty(cboCatgory.Text))
                {
                    Common.setEmptyField("Element category", Program.ApplicationName); cboCatgory.Focus(); return;
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

                                string query = String.Format("INSERT INTO Reconciliation.tblTransDefinition([Description],[Type],[ElementCatCode],IsActive) VALUES ('{0}','{1}','{2}','{3}');", txtDescription.Text.Trim(), radioGroup1.EditValue, cboCatgory.SelectedValue, 1);

                                using (SqlCommand sqlCommand1 = new SqlCommand(query, db, transaction))
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
                            setReload(); Clear();
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
                                using (SqlCommand sqlCommand1 = new SqlCommand(String.Format(String.Format("UPDATE Reconciliation.tblTransDefinition SET [Description]='{{0}}',[Type]='{{1}}',[ElementCatCode]='{{2}}',IsActive='{{3}}'  where  TransID ='{0}'", ID), txtDescription.Text.Trim(), radioGroup1.EditValue, cboCatgory.SelectedValue,1), db, transaction))
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

        private void Clear()
        {
            //txtStreetGroup.Clear();
            txtDescription.Clear();
            radioGroup1.SelectedIndex = -1;
            cboCatgory.SelectedIndex = -1; ;
            
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
                    ID = Convert.ToInt32(dr["TransID"]);
                    bResponse = FillField(Convert.ToInt32(dr["TransID"]));
                }
                else
                {
                    Common.setMessageBox("No Record is seleected", "", 1);
                    bResponse = false;
                }
            }
            return bResponse;
        }

        private bool FillField(int fieldid)
        {
            bool bResponse = false;
            //load data from the table into the forms for edit
            //DataTable dts = extMethods.LoadData(String.Format("select * from ViewBankBranchAccount where BankAccountID ='{0}'", fieldid));

            DataTable dts = (new Logic()).getSqlStatement((String.Format("select * from ViewElementCatwgoryTransDefinition where TransID ='{0}'", fieldid))).Tables[0];

            if (dts != null)
            {
                bResponse = true;

                txtDescription.Text = dts.Rows[0]["Description"].ToString();

                if (dts.Rows[0]["Type"].ToString() == "DR")
                {
                    radioGroup1.SelectedIndex = 0;
                }
                else
                {
                    radioGroup1.SelectedIndex = 1;
                }
                cboCatgory.Text = (string)dts.Rows[0]["ElementCategory"];
                cboCatgory.SelectedValue = (Int32)dts.Rows[0]["ElementCatCode"];
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
                using (SqlDataAdapter ada = new SqlDataAdapter((string)"SELECT ElementCatCode,Description FROM Reconciliation.tblElementCategory", Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }

                Dt = ds.Tables[0];
            }

            Common.setComboList(cboCatgory, Dt, "ElementCatCode", "Description");

            cboCatgory.SelectedIndex = -1;


        }


    }
}
