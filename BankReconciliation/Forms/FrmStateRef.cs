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

namespace BankReconciliation.Forms
{
    public partial class FrmStateRef : Form
    {
        public static FrmStateRef publicStreetGroup;

        protected TransactionTypeCode iTransType;

        protected bool boolIsUpdate;

        protected int ID;

        bool isFirst = true;

        public FrmStateRef()
        {
            InitializeComponent();

            publicStreetGroup = this;

            setImages();

            ToolStripEvent();

            iTransType = TransactionTypeCode.Null;

            Load += OnFormLoad;

            bttnUpdate.Click += bttnUpdate_Click;

            txtPercent.Leave += txtPercent_Leave;

            txtVAT.Leave += txtVAT_Leave;

            txtWTH.Leave += txtWTH_Leave;

            gridView1.DoubleClick += gridView1_DoubleClick;

            dtpBegin.Format = DateTimePickerFormat.Custom;

            dtpBegin.CustomFormat = "dd-MM-yyyy";

            OnFormLoad(null, null);

           
        }

        void gridView1_DoubleClick(object sender, EventArgs e)
        {
            tsbEdit.PerformClick();
        }

        void txtWTH_Leave(object sender, EventArgs e)
        {
            if (!Logic.IsNumber((string)txtWTH.Text))
            {
                Common.setMessageBox(" With Holding Tax can only be in number values", Program.ApplicationName, 2);

                txtWTH.Clear(); txtWTH.Focus(); return;
            }
        }

        void txtVAT_Leave(object sender, EventArgs e)
        {
            if (!Logic.IsNumber((string)txtVAT.Text))
            {
                Common.setMessageBox(" VAT Payable Percentage can only be in number values", Program.ApplicationName, 2);

                txtVAT.Clear(); txtVAT.Focus(); return;
            }
        }

        void txtPercent_Leave(object sender, EventArgs e)
        {
            if (!Logic.IsNumber((string)txtPercent.Text))
            {
                Common.setMessageBox(" Payable Percentage can only be in number values", Program.ApplicationName, 2);

                txtPercent.Clear(); txtPercent.Focus(); return;
            }
        }

        void bttnUpdate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtLetter.Text))
            {
                Common.setEmptyField("State Reference Letter", Program.ApplicationName);
                txtLetter.Focus(); return;
            }
            else if (string.IsNullOrEmpty(txtPercent.Text))
            {
                Common.setEmptyField("Payable Percentage", Program.ApplicationName);
                txtPercent.Focus(); return;
            }
            else if (string.IsNullOrEmpty(txtVAT.Text))
            {
                Common.setEmptyField("VAT Payable", Program.ApplicationName);
                txtVAT.Focus(); return;
            }
            else if (string.IsNullOrEmpty(txtWTH.Text))
            {
                Common.setEmptyField("With Holding Tax", Program.ApplicationName);
                txtWTH.Focus(); return;
            }
            else
            {
                if (!boolIsUpdate)
                {
                    using (SqlConnection db = new SqlConnection(Logic.ConnectionString))
                    {
                        SqlTransaction transaction;

                        db.Open();

                        transaction = db.BeginTransaction();
                        try
                        {

                            string query = String.Format("INSERT INTO Reconciliation.tblStateRef([RefNo],[Bdate] ,[Payable],[VAT],[WTH]) VALUES ('{0}','{1}','{2}','{3}','{4}');", txtLetter.Text.ToUpper(), string.Format("{0:yyyy/MM/dd 23:59:59}", dtpBegin.Value), Convert.ToDouble(txtPercent.Text), Convert.ToDouble(txtVAT.Text), Convert.ToDouble(txtWTH.Text));

                            using (SqlCommand sqlCommand1 = new SqlCommand(query, db, transaction))
                            {
                                sqlCommand1.ExecuteNonQuery();
                            }

                            transaction.Commit();
                        }
                        catch (SqlException sqlError)
                        {
                            transaction.Rollback();
                            Tripous.Sys.ErrorBox(sqlError); return;
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
                            //MessageBox.Show(MDIMain.stateCode);
                            //fieldid
                            string query = String.Format(String.Format("UPDATE Reconciliation.tblStateRef SET [RefNo]='{{0}}',[Payable]='{{1}}',[VAT]='{{2}}',[WTH]='{{3}}',[Bdate]='{{4}}',[Edate]='{{5}}' where  StateRefID ='{0}'", ID), txtLetter.Text.ToUpper(), Convert.ToInt32(txtPercent.Text), Convert.ToInt32(txtVAT.Text), Convert.ToInt32(txtWTH.Text), dtpBegin.Value.ToString("MM/dd/yyyy"), dtpEnding.Value.ToString("MM/dd/yyyy"));

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
                }
            }
            setReload();
            Common.setMessageBox("Records Successfully saved.", Program.ApplicationName, 1);
            //bttnReset.PerformClick();
            tsbReload.PerformClick();
        }

        private void setImages()
        {
            tsbNew.Image = MDIMains.publicMDIParent.i16x16.Images[5];
            tsbEdit.Image = MDIMains.publicMDIParent.i16x16.Images[6];
            tsbDelete.Image = MDIMains.publicMDIParent.i16x16.Images[7];
            tsbReload.Image = MDIMains.publicMDIParent.i16x16.Images[0];
            tsbClose.Image = MDIMains.publicMDIParent.i16x16.Images[11];

            //bttnCancel.Image = MDIMains.publicMDIParent.i32x32.Images[9];
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
                    if (string.IsNullOrEmpty(ID.ToString()))
                    {
                        Common.setMessageBox("No Record Selected for Deleting", Program.ApplicationName, 3);
                        return;
                    }
                    else
                        deleteRecord(ID);
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
            //setDBComboBox();

            isFirst = false;
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
                    splitContainer1.Panel2Collapsed = true;
                    break;
                case TransactionTypeCode.Edit:
                    splitContainer1.Panel1Collapsed = false;
                    splitContainer1.Panel2Collapsed = true;
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

            using (var ds = new System.Data.DataSet())
            {

                using (SqlDataAdapter ada = new SqlDataAdapter("SELECT StateRefID,RefNo AS [Reference Number],Bdate AS [Beginning Date],Edate AS [Ending Date] FROM Reconciliation.tblStateRef", Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }

                dt = ds.Tables[0];
                gridControl1.DataSource = dt.DefaultView;
            }
            //\\FILE-SERVER
            gridView1.OptionsBehavior.Editable = false;
            //gridView1.Columns["OpenBal"].DisplayFormat.FormatType = FormatType.Numeric;
            //gridView1.Columns["NatureID"].DisplayFormat.FormatString = "n2";
            gridView1.Columns["StateRefID"].Visible = false;
            gridView1.Columns["Ending Date"].Visible = false;
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
                    ID = Convert.ToInt32(dr["StateRefID"]);

                    bResponse = FillField(Convert.ToInt32(dr["StateRefID"]));
                }
                else
                {
                    Common.setMessageBox("No Record is seleected", "", 1);

                    bResponse = false;
                }
            }
            return bResponse;
        }

        private bool FillField(Int32 fieldid)
        {
            bool bResponse = false;
            //load data from the table into the forms for edit
            //DataTable dts = extMethods.LoadData(String.Format("select * from ViewBankBranchAccount where BankAccountID ='{0}'", fieldid));

            DataTable dts = (new Logic()).getSqlStatement((String.Format("select * from Reconciliation.tblStateRef where StateRefID ='{0}'", fieldid))).Tables[0];

            if (dts != null)
            {
                bResponse = true;

                txtLetter.Text = dts.Rows[0]["RefNo"].ToString();
                //txtPercent.Enabled = false;
                txtPercent.Text = dts.Rows[0]["Payable"].ToString();
                txtVAT.Text = dts.Rows[0]["VAT"].ToString();
                txtWTH.Text = dts.Rows[0]["WTH"].ToString();
                dtpBegin.Value = Convert.ToDateTime(dts.Rows[0]["Bdate"]);
                //dtpEnding.Value = Convert.ToDateTime(dts.Rows[0]["Edate"]);
                //cboNature.SelectedValue = dts.Rows[0]["NatureID"].ToString();

            }
            else
                bResponse = false;

            return bResponse;
        }

        public void RefreshForm()
        {
            NavBars.ToolStripEnableDisableControls(toolStrip, Tag as String);
        }

        void deleteRecord(int parameter2)
        {
            string query = String.Format("DELETE  FROM Reconciliation.tblStateRef WHERE StateRefID='{0}'", parameter2);

            using (SqlConnection db = new SqlConnection(Logic.ConnectionString))
            {
                SqlTransaction transaction;

                db.Open();

                transaction = db.BeginTransaction();

                try
                {
                    using (SqlCommand sqlCommand1 = new SqlCommand(query, db, transaction))
                    {
                        sqlCommand1.ExecuteNonQuery();
                    }

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Tripous.Sys.ErrorBox(ex);
                }

                db.Close();
            }
            Common.setMessageBox("Record Deleted Successfully ", Program.ApplicationName, 3);
        }

    }
}
