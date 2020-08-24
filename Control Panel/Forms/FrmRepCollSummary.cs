using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TaxSmartSuite.Class;
using Control_Panel.Reports;
using Control_Panel.Class;
using DevExpress.XtraReports.UI;

namespace Control_Panel.Forms
{
    public partial class FrmRepCollSummary : Form
    {
        public static FrmRepCollSummary publicStreetGroup;

        protected bool boolIsUpdate;

        protected string ID;

        DataTable Dts, DtSR;

        bool isFirst = true;

        protected TransactionTypeCode iTransType;

        public FrmRepCollSummary()
        {
            InitializeComponent();

            setImages();

            publicStreetGroup = this;

            ToolStripEvent();

            btnPrint.Click += Bttn_Click;

            OnFormLoad(null, null);

        }

        private void setImages()
        {
            tsbNew.Image = MDIMain.publicMDIParent.i16x16.Images[5];

            tsbEdit.Image = MDIMain.publicMDIParent.i16x16.Images[6];

            tsbDelete.Image = MDIMain.publicMDIParent.i16x16.Images[7];

            tsbReload.Image = MDIMain.publicMDIParent.i16x16.Images[0];

            tsbClose.Image = MDIMain.publicMDIParent.i16x16.Images[11];

            btnPrint.Image = MDIMain.publicMDIParent.i32x32.Images[5];

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
            //else if (sender == tsbNew)
            //{
            //    //groupControl2.Text = "Add New Record";

            //    iTransType = TransactionTypeCode.New;

            //    ShowForm();

            //    //clear();

            //    //groupControl2.Enabled = true;

            //    boolIsUpdate = false;
            //}
            //else if (sender == tsbEdit)
            //{
            //    //groupControl2.Text = "Edit Record Mode";

            //    iTransType = TransactionTypeCode.Edit;

            //    //groupControl2.Enabled = true;

            //    ShowForm();

            //    boolIsUpdate = true;

            //}
            ////else if (sender == tsbDelete)
            ////{
            ////    groupControl2.Text = "Delete Record Mode";
            ////    iTransType = TransactionTypeCode.Delete;
            ////    if (MosesClassLibrary.Utilities.Common.AskQuestion("Deleting this record will delete attached record.\nDo you want to continue?", ""))
            ////    {
            ////    }
            ////    else
            ////        tsbReload.PerformClick();
            ////    boolIsUpdate = false;
            ////}
            else if (sender == tsbReload)
            {
                iTransType = TransactionTypeCode.Reload;
                //ShowForm();
                rdbAgency.Checked = false;
                rdbBank.Checked = false;
                rdbRevOffices.Checked = false;
                rdbRevType.Checked = false;
               
            }
            ////bttnReset.PerformClick();
        }

        void OnFormLoad(object sender, EventArgs e)
        {
            isFirst = false;

        }

        void Bttn_Click(object sender, EventArgs e)
        {

            if (sender == btnPrint)
            {
                printRecords();
            }
        }

        private void printRecords()
        {
            if (!rdbBank.Checked && !rdbAgency.Checked && !rdbRevType.Checked && !rdbRevOffices.Checked )
            {
                Common.setEmptyField("Cash Book Reports Option ", Program.ApplicationName);

                return;
            }
            else
            {
                if (rdbBank.Checked)
                {
                    XRepBankMaster report = new XRepBankMaster();

                    report.paramBDate.Value = dtpBegin.Value.Date.ToString("yyyy-MM-dd");

                    report.paramEDate.Value = dtpEnd.Value.Date.ToString("yyyy-MM-dd");

                    report.ShowPreviewDialog();
                }

                if (rdbAgency.Checked)
                {

                    XRepAgencyMaster reports = new XRepAgencyMaster();

                    reports.paramBDate.Value = dtpBegin.Value.Date.ToString("yyyy-MM-dd");

                    reports.paramEDate.Value = dtpEnd.Value.Date.ToString("yyyy-MM-dd");

                    reports.ShowPreviewDialog();

                }
                if (rdbRevType.Checked)
                {
                    XRepRevTypeMaster reportRev = new XRepRevTypeMaster();

                    reportRev.paramBDate.Value = dtpBegin.Value.Date.ToString("yyyy-MM-dd");

                    reportRev.paramEDate.Value = dtpEnd.Value.Date.ToString("yyyy-MM-dd");

                    reportRev.ShowPreviewDialog();

                }
                if (rdbRevOffices.Checked)
                {
                    XRepRevenueOfficeMaster office = new XRepRevenueOfficeMaster();

                    office.paramBegDate.Value = dtpBegin.Value.Date.ToString("yyyy-MM-dd");

                    office.paramEnDate.Value = dtpEnd.Value.Date.ToString("yyyy-MM-dd");

                    office.ShowPreviewDialog();


                }
            }

        }

        public void RefreshForm()
        {
            NavBars.ToolStripEnableDisableControls(toolStrip, Tag as String);
        }


    }
}
