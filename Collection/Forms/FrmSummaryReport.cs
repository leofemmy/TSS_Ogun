using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TaxSmartSuite.Class;
using DevExpress.XtraReports.UI;
using DevExpress.XtraReports.Parameters;
using Collection.Report;

namespace Collection.Forms
{
    public partial class FrmSummaryReport : Form
    {
        public static FrmSummaryReport publicStreetGroup;

        protected bool boolIsUpdate;

        protected string ID;

        DataTable Dts, DtSR;

        bool isFirst = true;

        protected TransactionTypeCode iTransType;

        public FrmSummaryReport()
        {
            InitializeComponent();

            setImages();

            publicStreetGroup = this;

            ToolStripEvent();

            btnPrint.Click += Bttn_Click;

            OnFormLoad(null, null);
        }

        void OnFormLoad(object sender, EventArgs e)
        {
            isFirst = false;

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
            ////else if (sender == tsbReload)
            ////{
            ////    iTransType = TransactionTypeCode.Reload;
            ////    ShowForm();
            ////}
            ////bttnReset.PerformClick();
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
            if (!rdbBank.Checked && !rdbAgency.Checked && !rdbRevOffice.Checked)
            {
                Common.setEmptyField("Cash Book Reports Option ", Program.ApplicationName);

                return;
            }
            else
            {
                if (rdbBank.Checked)
                {
                    XRepSummaryMaster report = new XRepSummaryMaster();

                    report.paramStartDate.Value = dtpBegin.Value.Date.ToString("yyyy-MM-dd");

                    report.paramEndDate.Value = dtpEnd.Value.Date.ToString("yyyy-MM-dd");

                    report.ShowPreviewDialog();
                }

                if (rdbAgency.Checked)
                {
                    XRepAgency reports = new XRepAgency();

                    reports.paramBeginDate.Value = dtpBegin.Value.Date.ToString("yyyy-MM-dd");

                    reports.paramEndDate.Value = dtpEnd.Value.Date.ToString("yyyy-MM-dd");

                    reports.ShowPreviewDialog();

                }
                if (rdbRevOffice.Checked)
                {
                    XRepRevenue repRev = new XRepRevenue();

                    repRev.paramStartDate.Value = dtpBegin.Value.Date.ToString("yyyy-MM-dd");

                    repRev.paramEndDate.Value = dtpEnd.Value.Date.ToString("yyyy-MM-dd");

                    repRev.ShowPreviewDialog();
                }
            }

        }
    }
    
}
