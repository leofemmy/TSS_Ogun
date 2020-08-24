using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using TaxSmartSuite;
using System.Windows.Forms;
using TaxSmartSuite.Class;
using Collection.Classess;
using DevExpress.XtraReports.UI;
using DevExpress.XtraReports.Parameters;
using DevExpress.XtraSplashScreen;
using Collections;
using System.Data.SqlClient;

namespace Collection.Forms
{
    public partial class FrmDetailsReceipts : Form
    {
        public static FrmDetailsReceipts publicStreetGroup;

        protected TransactionTypeCode iTransType;

        public static FrmDetailsReceipts publicInstance;

        protected bool boolIsUpdate; private SqlCommand _command;

        SqlDataAdapter ada; SqlDataAdapter adp;

        string BatchNumber;

        string SQL,SQL1,SQL2,SQL3,SQL4;
        

         DateTime dt;

        
        public FrmDetailsReceipts()
         {

             try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

                InitializeComponent();

                publicInstance = this;

                publicStreetGroup = this;

                setImages();

                ToolStripEvent();

                Load += OnFormLoad;

                //btnSelect.Click += Bttn_Click;
                btnSelect.Click += btnSelect_Click;

                btnPrint.Click += btnPrint_Click;

                OnFormLoad(null, null);
            }
            finally
            {
                SplashScreenManager.CloseForm(false);
            }
        }

        void btnPrint_Click(object sender, EventArgs e)
        {
            Report.XtRepDetails rep = new Collection.Report.XtRepDetails();

            rep.xrLabel4.Text = lblCash.Text;
            rep.xrLabel5.Text = lblOwn.Text;
            rep.xrLabel6.Text = lblother.Text;
            rep.xrLabel8.Text = lblTotal.Text;
            rep.xrLabel14.Text = lblInter.Text;
            rep.xrLabel17.Text = lblException.Text;
            rep.xrLabel12.Text = String.Format("Printed By {0} on {1} ; Station Name : {2}", Program.UserID, DateTime.Now,Program.stationName);

            rep.xrLabel15.Text = string.Format("Transaction Date: {0}", dateTimePicker1.Value.ToShortDateString());
             
            rep.ShowPreviewDialog();
        }

        void btnSelect_Click(object sender, EventArgs e)
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

                DateTime dt = dateTimePicker1.Value.AddDays(1);

                using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
                {
                    connect.Open();
                    _command = new SqlCommand("doDetailsReceipts", connect) { CommandType = CommandType.StoredProcedure };
                    
                    _command.Parameters.Add(new SqlParameter("@date1", SqlDbType.VarChar)).Value =
                        string.Format("{0:yyyy/MM/dd 00:00:00}", dateTimePicker1.Value.ToString("MM/dd/yyyy"));
                    _command.Parameters.Add(new SqlParameter("@date2", SqlDbType.VarChar)).Value =
                        string.Format("{0:yyyy/MM/dd 00:00:00}", dateTimePicker1.Value.AddDays(1).ToString("MM/dd/yyyy"));
                    

                    _command.CommandTimeout = 0;

                    System.Data.DataSet response = new System.Data.DataSet();

                    adp = new SqlDataAdapter(_command);
                    adp.Fill(response);

                    connect.Close();
                    if (response.Tables[0].Rows[0]["returnCode"].ToString() != "00")
                    {
                        Common.setMessageBox(response.Tables[0].Rows[0]["returnMessage"].ToString(),
                            "Receipts", 2);
                        return;
                    }
                    else
                    {
                        //dtd.Clear();
                        //dtd = response.Tables[1];
                        //gridControl1.DataSource = dtd.DefaultView;
                        lblCash.Text = response.Tables[1].Rows[0]["cash"].ToString();
                        lblOwn.Text = response.Tables[2].Rows[0]["own"].ToString();
                        lblother.Text = response.Tables[3].Rows[0]["others"].ToString();
                        lblInter.Text = response.Tables[4].Rows[0]["internal"].ToString();
                        lblException.Text = response.Tables[5].Rows[0]["Exception"].ToString();

                        //if (response.Tables[1] != null && response.Tables[0].Rows.Count > 0)
                        //{
                        //    //using (
                        //    //    Frmduplicate duplicatefrm =
                        //    //        new Frmduplicate(response.Tables[1], Convert.ToInt32(radioGroup1.EditValue),
                        //    //        false))
                        //    //{
                        //    //    duplicatefrm.FormBorderStyle =
                        //    //        System.Windows.Forms.FormBorderStyle.FixedDialog;
                        //    //    duplicatefrm.ShowDialog();
                        //    //}
                        //}
                        //else
                        //{
                        //    Common.setMessageBox("Previson Receipt Record Not Found", "Receipt", 2); return;
                        //}
                    }
                }

                //CashCollection();

                //OtherCollection();

                //OwnerCollection();

                //InterCollection();

                //Exception();
                ////*344*0#
                ////    *244*2#
                lblTotal.Text = Convert.ToString((Convert.ToInt32(lblother.Text) + Convert.ToInt32(lblCash.Text) + Convert.ToInt32(lblOwn.Text) + Convert.ToInt32(lblInter.Text)) - Convert.ToInt32(lblException.Text));

            }
            finally
            {
                SplashScreenManager.CloseForm(false);
            }
        }

        private void setImages()
        {
            tsbNew.Image = MDIMain.publicMDIParent.i16x16.Images[5];
            tsbEdit.Image = MDIMain.publicMDIParent.i16x16.Images[6];
            tsbDelete.Image = MDIMain.publicMDIParent.i16x16.Images[7];
            tsbReload.Image = MDIMain.publicMDIParent.i16x16.Images[0];
            tsbClose.Image = MDIMain.publicMDIParent.i16x16.Images[11];

            //bttnCancel.Image = MDIMain.publicMDIParent.i32x32.Images[9];
            btnPrint.Image = MDIMain.publicMDIParent.i32x32.Images[29];
            btnSelect.Image = MDIMain.publicMDIParent.i32x32.Images[29];
            //btnSearch.Image = MDIMain.publicMDIParent.i32x32.Images[2];

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
                //label11.Visible = false;

                //txtPaymentRef.Visible = false;

                //groupControl2.Text = "Add New Record";

                //iTransType = TransactionTypeCode.New;

                //ShowForm();

                //clear();

                //groupControl2.Enabled = true;

                boolIsUpdate = false;
            }
            else if (sender == tsbEdit)
            {
                //groupControl2.Text = "Edit Record Mode";

                iTransType = TransactionTypeCode.Edit;

                //ShowForm();

                boolIsUpdate = true;

            }
            //else if (sender == tsbDelete)
            //{
            //    groupControl2.Text = "Delete Record Mode";
            //    iTransType = TransactionTypeCode.Delete;
            //    if (MosesClassLibrary.Utilities.Common.AskQuestion("Deleting this record will delete attached record.\nDo you want to continue?", ""))
            //    {
            //    }
            //    else
            //        tsbReload.PerformClick();
            //    boolIsUpdate = false;
            //}
            //else if (sender == tsbReload)
            //{
            //    iTransType = TransactionTypeCode.Reload;
            //    ShowForm();
            //}
            //bttnReset.PerformClick();
        }

        void OnFormLoad(object sender, EventArgs e)
        {
            //setDBComboBox();
        }

        void CashCollection()
        {


            SQL = string.Format("SELECT COUNT(*) FROM Collection.tblCollectionReport WHERE ([EReceiptsDate] BETWEEN '{0} 00:00:00' AND '{0} 23:59:59') AND PaymentMethod LIKE '%cash%'", dateTimePicker1.Value.ToString("MM/dd/yyyy"), dateTimePicker1.Value.AddDays(1).ToString("MM/dd/yyyy"));

             lblCash.Text = (new Logic()).ExecuteScalar(SQL);
            
        }

        void OwnerCollection()
        {

            SQL1 = string.Format("SELECT COUNT(*) FROM Collection.tblCollectionReport WHERE ([EReceiptsDate] BETWEEN '{0} 00:00:00' AND '{0} 23:59:59') AND PaymentMethod LIKE '%Own%'", dateTimePicker1.Value.ToString("MM/dd/yyyy"), dateTimePicker1.Value.AddDays(1).ToString("MM/dd/yyyy"));
            lblOwn.Text=(new Logic()).ExecuteScalar(SQL1);


        }

        void OtherCollection()
        {
            SQL2 = string.Format("SELECT COUNT(*) FROM Collection.tblCollectionReport WHERE ([EReceiptsDate] BETWEEN '{0} 00:00:00' AND '{0} 23:59:59') AND PaymentMethod LIKE '%Othe%'", dateTimePicker1.Value.ToString("MM/dd/yyyy"), dateTimePicker1.Value.AddDays(1).ToString("MM/dd/yyyy"));

            lblother.Text = (new Logic()).ExecuteScalar(SQL2);
        }

        void InterCollection()
        {
            SQL3 = string.Format("SELECT COUNT(*) FROM Collection.tblCollectionReport WHERE ([EReceiptsDate] BETWEEN '{0} 00:00:00' AND '{0} 23:59:59') AND PaymentMethod LIKE '%inter%'", dateTimePicker1.Value.ToString("MM/dd/yyyy"), dateTimePicker1.Value.AddDays(1).ToString("MM/dd/yyyy"));

            lblInter.Text = (new Logic()).ExecuteScalar(SQL3);
        }

        public void RefreshForm()
        {
            NavBars.ToolStripEnableDisableControls(toolStrip, Tag as String);
        }

        void Exception()
        {
            SQL4 = string.Format("SELECT COUNT(*) FROM Collection.tblCollectionReport WHERE ([EReceiptsDate] BETWEEN '{0} 00:00:00' AND '{0} 23:59:59') AND  RevenueCode IN (SELECT RevenueCode FROM  Receipt.tblRevenueReceiptException )", dateTimePicker1.Value.ToString("MM/dd/yyyy"), dateTimePicker1.Value.AddDays(1).ToString("MM/dd/yyyy"));

            lblException.Text = (new Logic()).ExecuteScalar(SQL4);
        
        }

    }
}
