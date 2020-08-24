using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TaxSmartSuite.Class;
using BankReconciliation.Forms;
using System.Data.SqlClient;
using BankReconciliation.Class;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Columns;

namespace BankReconciliation
{
    public partial class Form1 : Form
    {
        DataTable Dtt = new DataTable("ImageTable");


        public static Form1 publicStreetGroup;

        protected TransactionTypeCode iTransType;

        protected bool boolIsUpdate;

        protected int ID;

        bool isFirst = true;

        bool isSecond = true;

        public Form1()
        {
            InitializeComponent();

            publicStreetGroup = this;

            setImages();

            ToolStripEvent();

            iTransType = TransactionTypeCode.New;

            Load += OnFormLoad;

            //create table 
            //Dtt.BeginInit();
            //Dtt.Columns.Add("Date", typeof(DateTime));
            //Dtt.Columns.Add("Bank Debit", typeof(decimal));
            //Dtt.Columns.Add("Bank Credit", typeof(decimal));
            //Dtt.Columns.Add("Remarks", typeof(string));
            //Dtt.EndInit();
            ////assign the datatable to gridview control

            //gridControl1.DataSource = Dtt.DefaultView;
            ////gridControl1.DataMember = "ImageTable";
            //gridView1.OptionsBehavior.Editable = true;
            ////gridView1.Columns["ApplicantID"].Visible = false;
            //gridView1.BestFitColumns();
            //
            DataTable table = new DataTable();
            table.Columns.Add("Date", typeof(DateTime));
            table.Columns.Add("Bank Debit", typeof(Decimal));
            table.Columns.Add("Bank Credit", typeof(Decimal));
            table.Columns.Add("Remarks", typeof(String));

            gridControl1.DataSource = table;
            //this.Controls.Add(gridControl1);
            gridControl1.BringToFront();
            gridView1.OptionsBehavior.Editable = true;
            gridView1.BestFitColumns();

            OnFormLoad(null, null);

            groupControl2.Enabled = false;


        }

        private void setImages()
        {
            tsbNew.Image = MDIMains.publicMDIParent.i16x16.Images[5];
            tsbEdit.Image = MDIMains.publicMDIParent.i16x16.Images[6];
            tsbDelete.Image = MDIMains.publicMDIParent.i16x16.Images[7];
            tsbReload.Image = MDIMains.publicMDIParent.i16x16.Images[0];
            tsbClose.Image = MDIMains.publicMDIParent.i16x16.Images[11];

            //bttnCancel.Image = MDIMains.publicMDIParent.i32x32.Images[9];
            bttnLoad.Image = MDIMains.publicMDIParent.i32x32.Images[39];
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
                this.groupControl2.Enabled = true;
                label7.Visible = false; bttnLoad.Visible = false;
                iTransType = TransactionTypeCode.New;
                //ShowForm();
                //Unlockfield();
                boolIsUpdate = false;
            }
            else if (sender == tsbEdit)
            {
                groupControl2.Text = "Edit Record Mode";
                this.groupControl2.Enabled = true;
                label7.Visible = true; bttnLoad.Visible = true;
                iTransType = TransactionTypeCode.Edit;
                //if (EditRecordMode())
                //{
                //ShowForm();
                //    Unlockfield();
                //    boolIsUpdate = true;
                //}
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
                //ShowForm();
            }
            //bttnReset.PerformClick();
        }

        void OnFormLoad(object sender, EventArgs e)
        {
            ShowForm();

            setDBComboBox();

            isFirst = false;

            isSecond = false;

            cboBank.KeyPress += cboBank_KeyPress;

            cboBranch.KeyPress += cboBranch_KeyPress;

            cboBank.SelectedIndexChanged += cboBank_SelectedIndexChanged;
            //setReload();

            cboBranch.SelectedIndexChanged += cboBranch_SelectedIndexChanged;

            cboBank_SelectedIndexChanged(null, null);

            cboBranch_SelectedIndexChanged(null, null);

            gridView1.ValidateRow += gridView1_ValidateRow;

            gridView1.CellValueChanging += new CellValueChangedEventHandler(gridView1_CellValueChanging);
        }

        void gridView1_CellValueChanging(object sender, CellValueChangedEventArgs e)
        {
            //Dim intID As Integer = (gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "IntegrationEntityID"));
 

            //decimal credit;

          object  credit = (gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "Bank Debit"));
            if (e.Column.FieldName == "Bank Debit")
            {
                credit = Convert.ToDecimal(gridView1.GetRowCellValue(e.RowHandle, "Bank Debit"));
            }
        }

        void gridView1_ValidateRow(object sender, DevExpress.XtraGrid.Views.Base.ValidateRowEventArgs e)
        {
            //string[] split;

            //if (txtEPeriod.EditValue == null)
            //{
            //    Common.setEmptyField("Transaction Period", Program.ApplicationName);
            //    txtEPeriod.Focus(); return;
            //}
            //else
            //{
            //    split =txtEPeriod.EditValue.ToString().Split(new Char[] { '/' });
            //}
            ////if (txtPeriod.Text == null)
            ////{
            ////    Common.setEmptyField("Transaction Period", Program.ApplicationName);
            ////    txtPeriod.Focus(); return;
            ////}
            ////else
            ////{
            ////    split = txtPeriod.Text.Trim().Split(new Char[] { '/' });

            ////}

            //ColumnView view = sender as ColumnView;

            ////split2[1].ToString();

            //GridColumn column1 = view.Columns["Date"];

            ////Get the value of the first column
            //DateTime time1 = (DateTime)view.GetRowCellValue(e.RowHandle, column1);

            //if (time1.Month != Convert.ToInt32(split[0]) || time1.Year != Convert.ToInt32(split[1]))
            //{
            //    Common.setMessageBox("Date Not Withing the Transaction Period", Program.ApplicationName, 1); return;

            //    e.Valid = false;
            //}

        }

        void cboBranch_KeyPress(object sender, KeyPressEventArgs e)
        {
            Methods.AutoComplete(cboBranch, e, true);
        }

        void cboBranch_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboBranch.SelectedValue != null && !isSecond)
            {
                setDBComboBoxAcct(Convert.ToInt32(cboBranch.SelectedValue));
                //cboBranch.SelectedIndex = -1;
            }
        }

        void cboBank_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboBank.SelectedValue != null && !isFirst)
            {
                setDBComboBoxBranch(cboBank.SelectedValue.ToString());
                //cboBank.SelectedIndex = -1;
            }
        }

        void cboBank_KeyPress(object sender, KeyPressEventArgs e)
        {
            Methods.AutoComplete(cboBank, e, true);
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

            using (var ds = new System.Data.DataSet())
            {
                string query = "SELECT BankName,BankShortCode FROM dbo.tblBank";
                using (SqlDataAdapter ada = new SqlDataAdapter(query, Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }

                Dt = ds.Tables[0];
            }

            Common.setComboList(cboBank, Dt, "BankShortCode", "BankName");

            cboBank.SelectedIndex = -1;

        }

        private void Lockfield()
        {
            //txtaddress.Enabled = false;
            //txtEmail.Enabled = false;
            //txtFax.Enabled = false;
            //txtHead.Enabled = false;
            //txtPhone.Enabled = false;
            //txtzone.Enabled = false;
        }

        private void Unlockfield()
        {
            //txtaddress.Enabled = true;
            //txtEmail.Enabled = true;
            //txtFax.Enabled = true;
            //txtHead.Enabled = true;
            //txtPhone.Enabled = true;
            //txtzone.Enabled = true;
        }

        void setDBComboBoxBranch(string Parameter)
        {
            DataTable Dt;

            using (var ds = new System.Data.DataSet())
            {
                string query = String.Format("SELECT BranchName,BranchID FROM dbo.tblBankBranch WHERE BankShortCode ='{0}'", Parameter);

                using (SqlDataAdapter ada = new SqlDataAdapter(query, Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }

                Dt = ds.Tables[0];
            }

            Common.setComboList(cboBranch, Dt, "BranchID", "BranchName");

            cboBranch.SelectedIndex = -1;
        }

        void setDBComboBoxAcct(int Param)
        {
            DataTable Dt;

            using (var ds = new System.Data.DataSet())
            {
                string query = String.Format("SELECT AccountNumber,BankAccountID FROM dbo.tblBankAccount WHERE BranchID ='{0}'", Param);

                using (SqlDataAdapter ada = new SqlDataAdapter(query, Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }

                Dt = ds.Tables[0];
            }

            Common.setComboList(cboAcct, Dt, "BankAccountID", "AccountNumber");

            cboAcct.SelectedIndex = -1;

        }

        //private void gridView1_CellValueChanging(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        //{
        //    decimal credit;
        //    //int handle = e.RowHandle;
        //    //int foced = gridView1.FocusedRowHandle;
        //    //if (e.Column.FieldName == "Bank Debit")
        //    //{
        //    //    credit = Convert.ToDecimal(gridView1.GetRowCellValue(0, "Bank Credit"));
        //    //    if (credit == decimal.Zero)
        //    //        gridView1.Columns["Bank Credit"].OptionsColumn.AllowEdit = false;
        //    //    else
        //    //        gridView1.Columns["Bank Credit"].OptionsColumn.AllowEdit = true;
        //    //}
        //    //else if (e.Column.FieldName == "Bank Credit")
        //    //{
        //    //    credit = Convert.ToDecimal(gridView1.GetRowCellValue(0, "Bank Debit"));
        //    //    if (credit == decimal.Zero)
        //    //        gridView1.Columns["bank debit"].OptionsColumn.AllowEdit = false;
        //    //    else
        //    //        gridView1.Columns["bank debit"].OptionsColumn.AllowEdit = true;
        //    //}

        //    if (e.Column.Caption == "Bank Debit")
        //    {
        //        credit =Convert.ToDecimal(gridView1.GetRowCellValue(e.RowHandle, "Bank Debit"));
        //    }
        //}
    }
}
