using BankReconciliation.Report;
using DevExpress.Utils;
using DevExpress.XtraSplashScreen;
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

namespace BankReconciliation.Forms
{
    public partial class FrmReportPosting : Form
    {
        public static FrmReportPosting publicStreetGroup;

        DataSet dts; DataSet dts2;
        public FrmReportPosting()
        {
            InitializeComponent();

            Init();

        }

        public FrmReportPosting(DataSet dst1)
        {
            InitializeComponent();

            dts = new DataSet(); dts.Clear();
            dts = dst1;

            Init();
        }

        void Init()
        {
            SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);


            publicStreetGroup = this;

            setImages();

            ToolStripEvent();

            Load += OnFormLoad;

            OnFormLoad(null, null);

            btnPrint.Click += btnPrint_Click;

            SplashScreenManager.CloseForm(false);
        }

        void btnPrint_Click(object sender, EventArgs e)
        {
            //gridControl1.ShowPrintPreview();
            if (dts.Tables[0].Rows[0]["returnCode"].ToString() == "00")
            {
                if (dts.Tables[1] != null && dts.Tables[1].Rows.Count > 0)
                {
                    //using object class
                    var list = (from DataRow row in dts.Tables[1].Rows
                                select new Dataset.PostedList
                                {
                                    Amount = Convert.ToDecimal(row["Amount"]),
                                    PaymentDate = Convert.ToDateTime(row["Date"]),
                                    PaymentRefNumber = row["PaymentRefNumber"] as string,

                                    //Description = row["Description"] as string,

                                    PayerID = row["payerid"] as string,
                                    //Transdate = Convert.ToDateTime(row["BSDate"]),
                                    //AgecnyName = row["AgencyName"] as string,
                                    BankName = row["BankName"] as string,

                                }
                                    ).ToList();
                    //string state=Program.StateName;
                    XtraRepPostedlist posted = new XtraRepPostedlist();
                  
                    posted.xrLabel6.Text = string.Format("{0} STATE GOVERNMENT OF NIGERIA", Program.StateName.ToUpper());

                    posted.xrLabel7.Text = string.Format("List of Collections Posted for the month ");

                    var binding = (BindingSource)posted.DataSource;

                    binding.Clear();

                    binding.DataSource = list;

                    posted.ShowPreviewDialog();

                }
                else
                {
                    Common.setMessageBox("No Record Found", Program.ApplicationName, 1); return;
                }
            }
        }
        private void setImages()
        {
            tsbNew.Image = MDIMains.publicMDIParent.i16x16.Images[5];
            tsbEdit.Image = MDIMains.publicMDIParent.i16x16.Images[6];
            tsbDelete.Image = MDIMains.publicMDIParent.i16x16.Images[7];
            tsbReload.Image = MDIMains.publicMDIParent.i16x16.Images[0];
            tsbClose.Image = MDIMains.publicMDIParent.i16x16.Images[11];

            //bttnCancel.Image = MDIMains.publicMDIParent.i32x32.Images[9];
            ////bttnReset.Image = MDIMains.publicMDIParent.i32x32.Images[8];
            ////bttnUpdate.Image = MDIMain.publicMDIParent.i32x32.Images[7];
            //bttnUpdate.Image = MDIMains.publicMDIParent.i32x32.Images[7];
            //btnAllocate.Image = MDIMains.publicMDIParent.i32x32.Images[6];
            //bttncompare.Image = MDIMains.publicMDIParent.i32x32.Images[8];
            //bttnClose.Image = MDIMain.publicMDIParent.i32x32.Images[9];
            //bttnSave.Image = MDIMains.publicMDIParent.i32x32.Images[7];

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
                //MDIMains.publicMDIParent.RemoveControls();
                this.Close();

            }
            else if (sender == tsbNew)
            {
                //groupControl2.Text = "Add New Record";
                //iTransType = TransactionTypeCode.New;
                //ShowForm();
                //boolIsUpdate = false;
            }
            else if (sender == tsbEdit)
            {
                //groupControl2.Text = "Edit Record Mode";
                //iTransType = TransactionTypeCode.Edit;
                //if (EditRecordMode())
                //{
                //    ShowForm();
                //    boolIsUpdate = true;
                //}
            }
            else if (sender == tsbDelete)
            {
                //groupControl2.Text = "Disable Record Mode";
                //iTransType = TransactionTypeCode.Delete;
                //if (MosesClassLibrary.Utilities.Common.AskQuestion("Disable this record will delete attached record.\nDo you want to continue?", ""))
                //{
                //    if (string.IsNullOrEmpty(ID.ToString()))
                //    {
                //        Common.setMessageBox("No Record Selected for Disable", Program.ApplicationName, 3);
                //        return;
                //    }
                //    else
                //        //deleteRecord(ID);
                //}
                //else
                tsbReload.PerformClick();
                //boolIsUpdate = false;
            }
            else if (sender == tsbReload)
            {
                //iTransType = TransactionTypeCode.Reload; setReload();
                //ShowForm();
            }
            //bttnReset.PerformClick();
        }

        void OnFormLoad(object sender, EventArgs e)
        {
            gridControl1.DataSource = dts.Tables[1];
            gridView1.Columns["STATUS"].Visible = false;
            gridView1.Columns["Amount"].DisplayFormat.FormatType = FormatType.Numeric;
            gridView1.Columns["Amount"].DisplayFormat.FormatString = "n2";
            gridView1.Columns["Date"].DisplayFormat.FormatType = FormatType.DateTime;
            gridView1.Columns["Date"].DisplayFormat.FormatString = "dd/MM/yyyy";
            gridView1.OptionsView.ColumnAutoWidth = false;
            gridView1.BestFitColumns();

            //gridControl2.DataSource = dts.Tables[2];
            //gridView2.Columns["STATUS"].Visible = false;
            //gridView2.Columns["psAmount"].DisplayFormat.FormatType = FormatType.Numeric;
            //gridView2.Columns["psAmount"].DisplayFormat.FormatString = "n2";
            //gridView2.Columns["psDate"].DisplayFormat.FormatType = FormatType.DateTime;
            //gridView2.Columns["psDate"].DisplayFormat.FormatString = "dd/MM/yyyy";
            //gridView2.OptionsView.ColumnAutoWidth = false;
            //gridView2.BestFitColumns();
        }

    }
}
