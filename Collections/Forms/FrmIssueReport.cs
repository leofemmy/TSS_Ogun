using Collection.Classess;
using Collection.Report;
using DevExpress.XtraReports.UI;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using TaxSmartSuite.Class;

namespace Collection.Forms
{
    public partial class FrmIssueReport : Form
    {
        public static FrmIssueReport publicStreetGroup;

        protected TransactionTypeCode iTransType;

        public FrmIssueReport()
        {
            InitializeComponent();

            publicStreetGroup = this;

            setImages();

            ToolStripEvent();

            iTransType = TransactionTypeCode.New;

            Load += OnFormLoad;

            bttnUpdate.Click += bttnUpdate_Click;

            OnFormLoad(null, null);

        }

        void bttnUpdate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(cboPeriods.Text))
            {
                Common.setEmptyField("Station Name", Program.ApplicationName);
                cboPeriods.Focus(); return;
            }
            else
            {
                System.Data.DataSet ds = new System.Data.DataSet();

                string quy = string.Format("SELECT  IssueDate, IssueFrom, IssueTo, UploadStatus, IssueQty, StationCode FROM Receipt.tblIssueReceipt where StationCode='{0}' AND CONVERT(VARCHAR, CONVERT(DATETIME, IssueDate), 101) BETWEEN '{1}' AND '{2}' ", cboPeriods.SelectedValue, dateTimePicker1.Value.Date.ToString("dd-MM-yyyy"), dateTimePicker2.Value.Date.ToString("dd-MM-yyyy"));

                using (SqlDataAdapter ada = new SqlDataAdapter((string)quy, Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }

                if (ds.Tables[0].Rows.Count > 0)
                {
                    XtraReport2 report = new XtraReport2() { DataSource = ds, DataMember = "table" };

                    report.xrLabel8.Text = cboPeriods.Text;
                    report.xrLabel18.Text = string.Format("{0} State Government", Program.StateName);
                    report.xrLabel19.Text = string.Format("Receipts Issued to {0}  between {1} and {2}", cboPeriods.Text, dateTimePicker1.Value.Date.ToString("dd/MM/yyyy"), dateTimePicker2.Value.Date.ToString("dd/MM/yyyy"));

                    report.ShowPreviewDialog();
                }
                else
                    Common.setMessageBox("No Records for this Station to Preview", "Station Issue Report ", 1); return;



            }
        }

        private void FrmIssueReport_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'dataSet11.ViewIssueReceipts' table. You can move, or remove it, as needed.
            //this.viewIssueReceiptsTableAdapter.Fill(this.dataSet11.ViewIssueReceipts);
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = "dd/MM/yyyy";

            dateTimePicker2.Format = DateTimePickerFormat.Custom;
            dateTimePicker2.CustomFormat = "dd/MM/yyyy";

        }

        private void setImages()
        {
            tsbNew.Image = MDIMain.publicMDIParent.i16x16.Images[5];
            tsbEdit.Image = MDIMain.publicMDIParent.i16x16.Images[6];
            tsbDelete.Image = MDIMain.publicMDIParent.i16x16.Images[7];
            tsbReload.Image = MDIMain.publicMDIParent.i16x16.Images[0];
            tsbClose.Image = MDIMain.publicMDIParent.i16x16.Images[11];

            //bttnCancel.Image = MDIMains.publicMDIParent.i32x32.Images[9];
            bttnUpdate.Image = MDIMain.publicMDIParent.i32x32.Images[7];
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
                MDIMain.publicMDIParent.RemoveControls();
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
                using (SqlDataAdapter ada = new SqlDataAdapter((string)"SELECT StationCode,StationName FROM Receipt.tblStation ORDER BY StationCode", Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }

                Dt = ds.Tables[0];


            }

            Common.setComboList(cboPeriods, Dt, "StationCode", "StationName");



            cboPeriods.SelectedIndex = -1;


        }

        public void RefreshForm()
        {
            NavBars.ToolStripEnableDisableControls(toolStrip, Tag as String);
        }


    }
}
