using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using BankReconciliation.Class;
using TaxSmartSuite.Class;
using System.Globalization;
using BankReconciliation.Report;

namespace BankReconciliation.Forms
{
    public partial class FrmAccountView : Form
    {
        private bool isFirst = false;

        private string monthName;

        public static FrmAccountView publicStreetGroup;

        protected TransactionTypeCode iTransType;

        private String[] split;

        private string strmonth;

        public FrmAccountView()
        {
            InitializeComponent();

            publicStreetGroup = this;

            setImages();

            ToolStripEvent();

            iTransType = TransactionTypeCode.New;

            Load += OnFormLoad;

            OnFormLoad(null, null);

            bttnUpdate.Click += bttnUpdate_Click;

        }

        void bttnUpdate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(cboAcct.Text))
            {
                Common.setEmptyField("Account Number",Program.ApplicationName);
                cboAcct.Focus(); return;
            }
            else if (string.IsNullOrEmpty(cboPeriods.Text))
            {
                Common.setEmptyField("Transaction Period",Program.ApplicationName);
                cboPeriods.Focus(); return;
            }
            else
            {
                strmonth = cboPeriods.Text.Substring(0, 2);
                //string fullmonth = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(strmonth); ;
                split = cboPeriods.Text.Trim().Split(new Char[] { '/' });
                var dtf = CultureInfo.CurrentCulture.DateTimeFormat;
                //string monthName = dtf.GetMonthName(strmonth);
                var month = Convert.ToInt32(cboPeriods.Text.Substring(0, 2));
                var fyear = cboPeriods.Text.Length;
                monthName = dtf.GetMonthName(month);
                xrTranAccount report = new xrTranAccount();

                string fulltext = String.Format("Collection Reconciliation for : [{0} {1}]", monthName, Convert.ToInt32(split[1]));

                report.paramAccount.Value = cboAcct.Text;

                report.paramPeriod.Value = cboPeriods.Text;

                report.xrLabel1.Text = fulltext;
                report.xrLabel18.Text = String.Format("{0}  -  {1}", label4.Text, label5.Text);

                report.ShowPreviewDialog();
            }
        }

        public void setDBComboBox()
        {
            DataTable Dt;

            //connect.connect.Close();

            using (var ds = new System.Data.DataSet())
            {
                //connect.connect.Open();
                string query = "select *  from tblBankAccount";
                using (SqlDataAdapter ada = new SqlDataAdapter(query, Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }

                Dt = ds.Tables[0];
            }

            Common.setComboList(cboAcct, Dt, "BankAccountID", "AccountNumber");

            cboAcct.SelectedIndex = -1;


        }

        private void setImages()
        {
            tsbNew.Image = MDIMains.publicMDIParent.i16x16.Images[5];
            tsbEdit.Image = MDIMains.publicMDIParent.i16x16.Images[6];
            tsbDelete.Image = MDIMains.publicMDIParent.i16x16.Images[7];
            tsbReload.Image = MDIMains.publicMDIParent.i16x16.Images[0];
            tsbClose.Image = MDIMains.publicMDIParent.i16x16.Images[11];

            //bttnCancel.Image = MDIMains.publicMDIParent.i32x32.Images[9];
            bttnUpdate.Image = MDIMains.publicMDIParent.i32x32.Images[7];
            ////bttnReset.Image = MDIMains.publicMDIParent.i32x32.Images[8];
            //bttnPost.Image = MDIMains.publicMDIParent.i32x32.Images[34];

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
                //groupControl2.Text = "Add New Record";
                iTransType = TransactionTypeCode.New;
                //Clear();
                ShowForm();
                //boolIsUpdate = false;
            }
            else if (sender == tsbEdit)
            {
                //groupControl2.Text = "Edit Record Mode";
                iTransType = TransactionTypeCode.Edit;
                //if (EditRecordMode())
                //{
                ShowForm();
                //    boolIsUpdate = true;
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

                //boolIsUpdate = false;
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

            setDBComboBox();

            setDBComboBoxPeriod();

            //isFirst = false;

            //setReload();
            //cboBank.KeyPress += cboBank_KeyPress;
            //cboBranch.KeyPress += cboBranch_KeyPress;
            //cboBank.SelectedIndexChanged += cboBank_SelectedIndexChanged;
            //radioGroup1.SelectedIndexChanged += radioGroup1_SelectedIndexChanged;

            cboAcct.SelectedIndexChanged += cboAcct_SelectedIndexChanged;
            //cboBank_SelectedIndexChanged(null, null);

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

        public void setDBComboBoxPeriod()
        {
            DataTable Dt;

            //connect.connect.Close();

            using (var ds = new System.Data.DataSet())
            {
                //connect.connect.Open();
                string query = "select distinct period from ViewTransactionPostCollectionBank";
                using (SqlDataAdapter ada = new SqlDataAdapter(query, Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }

                Dt = ds.Tables[0];
            }

            Common.setComboList(cboPeriods, Dt, "period", "period");

            cboPeriods.SelectedIndex = -1;


        }

        void cboAcct_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboAcct.SelectedValue != null && !isFirst)
            {
                GetAcctInfor(cboAcct.SelectedValue.ToString());
            }
        }

        void GetAcctInfor(string parameter)
        {
            DataTable Dt;

            //connect.connect.Close();

            using (var ds = new System.Data.DataSet())
            {
                //connect.connect.Open();
                string query = String.Format("select BankName,AccountName,BranchName,BankAccountID,OpenBal from ViewBankBranchAccount where BankAccountID = '{0}'", parameter);

                using (SqlDataAdapter ada = new SqlDataAdapter(query, Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }

                Dt = ds.Tables[0];
            }

            if (Dt != null && Dt.Rows.Count > 0)
            {
                label3.Text = string.Format("Account Description: {0} ", (string)Dt.Rows[0]["AccountName"]);

                label4.Text = string.Format("Bank Name: {0} ", (string)Dt.Rows[0]["BankName"]) ;

                label5.Text = string.Format("Branch Name: {0} ", (string)Dt.Rows[0]["BranchName"]); 

                //openbalAcct = Convert.ToDouble(Dt.Rows[0]["OpenBal"]);
            }
        }

        public void RefreshForm()
        {
            NavBars.ToolStripEnableDisableControls(toolStrip, Tag as String);
        }

    }
}
