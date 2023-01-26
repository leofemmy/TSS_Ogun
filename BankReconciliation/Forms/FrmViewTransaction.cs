using BankReconciliation.Class;
using BankReconciliation.Report;
using DevExpress.XtraReports.UI;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using TaxSmartSuite.Class;

namespace BankReconciliation.Forms
{
    public partial class FrmViewTransaction : Form
    {
        private bool isFirst = false;

        private string monthName;

        public static FrmViewTransaction publicStreetGroup;

        protected TransactionTypeCode iTransType;

        private String[] split;

        private string strmonth;

        public FrmViewTransaction()
        {
            InitializeComponent();

            publicStreetGroup = this;

            setImages();

            ToolStripEvent();

            iTransType = TransactionTypeCode.New;

            Load += OnFormLoad;

            OnFormLoad(null, null);

            radioGroup1.SelectedIndexChanged += radioGroup1_SelectedIndexChanged;

            radioGroup2.SelectedIndexChanged += radioGroup2_SelectedIndexChanged;

            bttnUpdate.Click += bttnUpdate_Click;
        }

        void bttnUpdate_Click(object sender, EventArgs e)
        {
            if (radioGroup1.EditValue.ToString() == null)
            {
                Common.setEmptyField("Transaction Type", Program.ApplicationName);
                return;
            }
            else if (radioGroup2.EditValue.ToString() == null)
            {
                Common.setEmptyField("Bank", Program.ApplicationName);
                return;
            }
            else if (string.IsNullOrEmpty(cboPeriods.Text))
            {
                Common.setEmptyField("Transaction Month", Program.ApplicationName); cboPeriods.Focus();
                return;
            }
            else if (string.IsNullOrEmpty(cboYears.Text))
            {
                Common.setEmptyField("Transaction Year", Program.ApplicationName); cboYears.Focus();
                return;
            }
            else
            {
                XtraRepTransaction reportTrans = new XtraRepTransaction();
                //determine the paramarter choice
                if (radioGroup1.EditValue.ToString().ToString() == "0" && radioGroup2.EditValue.ToString().ToString() == "0")
                {
                    reportTrans.FilterString = string.Format("[Period] Like '{0}' AND [Years] Like '{1}' ", cboPeriods.SelectedValue, cboYears.SelectedValue);
                }
                else if (radioGroup1.EditValue.ToString() == "0" && radioGroup2.EditValue.ToString() == "1")
                {
                    reportTrans.FilterString = string.Format("[Period] Like '{0}' AND [Years] Like '{1}'  AND [BankName]Like '{2}'", cboPeriods.SelectedValue, cboYears.SelectedValue, cboBank.Text);
                }
                else if (radioGroup1.EditValue.ToString() == "1" && radioGroup2.EditValue.ToString() == "0")
                {
                    reportTrans.FilterString = string.Format("[Period] Like '{0}'  AND [Years] Like '{1}'  AND [Description]Like '{2}'", cboPeriods.SelectedValue, cboYears.SelectedValue, cboType.Text);
                }
                else if (radioGroup1.EditValue.ToString() == "1" && radioGroup2.EditValue.ToString() == "1")
                {
                    reportTrans.FilterString = string.Format("[Period] Like '{0}'  AND [Years] Like '{1}'  AND [Description]Like '{2}' AND [BankName]Like '{3}'", cboPeriods.SelectedValue, cboYears.SelectedValue, cboType.Text, cboBank.Text);
                }
                reportTrans.ApplyFiltering();
                //reportTrans.ShowPrintStatusDialog();
                //reportTrans.ShowPreviewDialog();
                reportTrans.ShowPreviewDialog();
            }


        }

        void radioGroup2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (radioGroup2.EditValue.ToString() == null)
            {
                return;
            }
            else if (radioGroup2.EditValue.ToString() == "1")
            { label3.Visible = true; cboBank.Visible = true; }
            else
            { label3.Visible = false; cboBank.Visible = false; }
        }

        void radioGroup1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (radioGroup1.EditValue.ToString() == null)
            {
                return;
            }
            else if (radioGroup1.EditValue.ToString() == "1")
            {
                label1.Visible = true; cboType.Visible = true;
            }
            else
            {
                label1.Visible = false; cboType.Visible = false;
            }
        }

        private void setImages()
        {

            tsbNew.Image = MDIMains.publicMDIParent.i16x16.Images[5];
            tsbEdit.Image = MDIMains.publicMDIParent.i16x16.Images[6];
            tsbDelete.Image = MDIMains.publicMDIParent.i16x16.Images[7];
            tsbReload.Image = MDIMains.publicMDIParent.i16x16.Images[0];
            tsbClose.Image = MDIMains.publicMDIParent.i16x16.Images[11];

            //bttnCancel.Image = MDIMain.publicMDIParent.i32x32.Images[9];
            bttnUpdate.Image = MDIMains.publicMDIParent.i32x32.Images[7];
            ////bttnReset.Image = MDIMain.publicMDIParent.i32x32.Images[8];
            //bttnPost.Image = MDIMain.publicMDIParent.i32x32.Images[34];

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

            SetDBComboBoxType();

            setDBComboBoxPeriods();

            setDBComboBoxPeriod();

            SetDBComboBoxBank();
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

        //public void setDBComboBoxPeriod()
        //{
        //    DataTable Dt;

        //    //connect.connect.Close();

        //    using (var ds = new System.Data.DataSet())
        //    {
        //        //connect.connect.Open();
        //        string query = "select distinct period from ViewTransactionPostCollectionBank";

        //        using (SqlDataAdapter ada = new SqlDataAdapter(query, Logic.ConnectionString))
        //        {
        //            ada.Fill(ds, "table");
        //        }

        //        Dt = ds.Tables[0];
        //    }

        //    Common.setComboList(cboPeriods, Dt, "period", "period");

        //    cboPeriods.SelectedIndex = -1;


        //}

        void SetDBComboBoxType()
        {
            DataTable Dt;

            cboType.Text = string.Empty;
            //connect.connect.Close();

            using (var ds = new System.Data.DataSet())
            {
                //connect.connect.Open();
                using (SqlDataAdapter ada = new SqlDataAdapter((string)"select Description,Type from tblTransDefinition", Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }

                Dt = ds.Tables[0];
            }

            Common.setComboList(cboType, Dt, "Type", "Description");

            cboType.SelectedIndex = -1;
        }

        void SetDBComboBoxBank()
        {
            DataTable Dt;

            cboBank.Text = string.Empty;
            //connect.connect.Close();

            using (var ds = new System.Data.DataSet())
            {
                //connect.connect.Open();
                using (SqlDataAdapter ada = new SqlDataAdapter((string)"select *  from tblBank", Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }

                Dt = ds.Tables[0];
            }

            Common.setComboList(cboBank, Dt, "BankShortCode", "BankName");

            cboBank.SelectedIndex = -1;
        }

        public void RefreshForm()
        {
            NavBars.ToolStripEnableDisableControls(toolStrip, Tag as String);
        }

        public void setDBComboBoxPeriod()
        {
            DataTable Dt;

            using (var ds = new System.Data.DataSet())
            {
                using (SqlDataAdapter ada = new SqlDataAdapter((string)"SELECT DISTINCT months,Periods FROM tblPeriods ORDER BY Periods", Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }

                Dt = ds.Tables[0];
            }

            Common.setComboList(cboPeriods, Dt, "Periods", "months");

            cboPeriods.SelectedIndex = -1;


        }

        void setDBComboBoxPeriods()
        {
            DataTable Dt;

            using (var ds = new System.Data.DataSet())
            {
                using (SqlDataAdapter ada = new SqlDataAdapter((string)"SELECT DISTINCT YEAR FROM tblPeriods", Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }

                Dt = ds.Tables[0];
            }

            Common.setComboList(cboYears, Dt, "YEAR", "YEAR");

            cboYears.SelectedIndex = -1;
        }

    }
}
