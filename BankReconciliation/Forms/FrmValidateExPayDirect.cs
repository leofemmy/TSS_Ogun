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
using BankReconciliation.Class;

namespace BankReconciliation.Forms
{
    public partial class FrmValidateExPayDirect : Form
    {
        public static FrmValidateExPayDirect publicStreetGroup;

        protected TransactionTypeCode iTransType;

        protected bool boolIsUpdate;

        protected string ID;

        string receiptno, payref;

        bool isFirst = true;

        bool isSecond = true;

        int Mth, Years;

        string[] split;

        public FrmValidateExPayDirect()
        {
            InitializeComponent();

            setImages();

            ToolStripEvent();

            iTransType = TransactionTypeCode.Null;

            Load += OnFormLoad;

            OnFormLoad(null, null);
        }

        private void setImages()
        {
            tsbNew.Image = MDIMains.publicMDIParent.i16x16.Images[5];
            tsbEdit.Image = MDIMains.publicMDIParent.i16x16.Images[6];
            tsbDelete.Image = MDIMains.publicMDIParent.i16x16.Images[7];
            tsbReload.Image = MDIMains.publicMDIParent.i16x16.Images[0];
            tsbClose.Image = MDIMains.publicMDIParent.i16x16.Images[11];

            //bttnEdit.Image = MDIMains.publicMDIParent.i32x32.Images[39];
            bttnLoad.Image = MDIMains.publicMDIParent.i32x32.Images[39];
            bttnExcel.Image = MDIMains.publicMDIParent.i32x32.Images[27];

            bttnFind.Image = MDIMains.publicMDIParent.i32x32.Images[40];
            //bttnUpdate.Image = MDIMains.publicMDIParent.i32x32.Images[7];

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
                //cboPosting.Text = "Non-REEMS Collections";
                iTransType = TransactionTypeCode.New;
                ShowForm();
                //boolIsUpdate = false;
                groupControl2.Enabled = true;
            }
            else if (sender == tsbEdit)
            {
                groupControl2.Text = "Edit Record Mode";

                iTransType = TransactionTypeCode.Edit;
                //if (EditRecordMode())
                //{
                ShowForm();
                //Unlockfield();
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

            //setDBComboBoxRevenue();

            cboBank.KeyPress += cboBank_KeyPress;

            //cboBranch.KeyPress += cboBranch_KeyPress;

            //cboPosting.KeyPress += cboPosting_KeyPress;

            //cboAcct.KeyPress += cboAcct_KeyPress;

            //cboRevenue.KeyPress += cboRevenue_KeyPress;

            cboBank.SelectedIndexChanged += cboBank_SelectedIndexChanged;
            //////setReload();

            //cboBranch.SelectedIndexChanged += cboBranch_SelectedIndexChanged;

            //cboBank_SelectedIndexChanged(null, null);

            //cboBranch_SelectedIndexChanged(null, null);

            //txtAmount.LostFocus += txtAmount_LostFocus;

            //txtDescription.Leave += txtDescription_Leave;

            //bttnUpdate.Click += bttnUpdate_Click;

            //bttnEdit.Click += bttnEdit_Click;

            //gridView1.DoubleClick += new EventHandler(gridView1_DoubleClick);
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
                    splitContainer1.Panel2Collapsed = true;
                    //Lockfield();
                    break;
                case TransactionTypeCode.Edit:
                    splitContainer1.Panel1Collapsed = true;
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

        void cboBank_KeyPress(object sender, KeyPressEventArgs e)
        {
            Methods.AutoComplete(cboBank, e, true);
        }

        void cboBank_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboBank.SelectedValue != null && !isFirst)
            {
                setDBComboBoxBranch(cboBank.SelectedValue.ToString());
            }
        }

        void setDBComboBoxBranch(string Parameter)
        {
            DataTable Dt;

            using (var ds = new System.Data.DataSet())
            {
                string query = String.Format("SELECT AccountNumber FROM dbo.ViewBankBranchAccount WHERE BankShortCode ='{0}'", Parameter);

                using (SqlDataAdapter ada = new SqlDataAdapter(query, Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }

                Dt = ds.Tables[0];
            }

            if (Dt != null && Dt.Rows.Count > 0)
            {
                label8.Text = Dt.Rows[0][0].ToString();
            }
        
        }

        public void RefreshForm()
        {
            NavBars.ToolStripEnableDisableControls(toolStrip, Tag as String);
        }

    }
}
