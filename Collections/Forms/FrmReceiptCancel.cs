using Collection.Classess;
//using System.Data.SqlClient;
using DevExpress.Utils;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using TaxSmartSuite.Class;

namespace Collection.Forms
{
    public partial class FrmReceiptCancel : Form
    {

        public static FrmReceiptCancel publicStreetGroup;

        protected TransactionTypeCode iTransType;

        public static FrmReceiptCancel publicInstance;

        protected bool boolIsUpdate;

        private DataTable dt;

        public FrmReceiptCancel()
        {
            InitializeComponent();

            publicInstance = this;

            publicStreetGroup = this;

            setImages();

            ToolStripEvent();

            Load += OnFormLoad;

            OnFormLoad(null, null);
        }

        public void RefreshForm()
        {
            NavBars.ToolStripEnableDisableControls(toolStrip, Tag as String);
        }

        private void setImages()
        {
            tsbNew.Image = MDIMain.publicMDIParent.i16x16.Images[5];
            tsbEdit.Image = MDIMain.publicMDIParent.i16x16.Images[6];
            tsbDelete.Image = MDIMain.publicMDIParent.i16x16.Images[7];
            tsbReload.Image = MDIMain.publicMDIParent.i16x16.Images[0];
            tsbClose.Image = MDIMain.publicMDIParent.i16x16.Images[11];
            btnCancel.Image = MDIMain.publicMDIParent.i32x32.Images[2];

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
                boolIsUpdate = false;
            }
            else if (sender == tsbEdit)
            {
                iTransType = TransactionTypeCode.Edit;

                //ShowForm();

                boolIsUpdate = true;

            }

        }

        void OnFormLoad(object sender, EventArgs e)
        {
            //setDBComboBox();

            btnCancel.Click += btnCancel_Click;

            //createTable();
        }

        void btnCancel_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtreceipt.Text))
            {
                Common.setEmptyField("Receipt Number", Program.ApplicationName);
                txtreceipt.Focus(); return;
            }
            else if (string.IsNullOrEmpty(txtreason.Text))
            {
                Common.setEmptyField("Reason for Cancel", Program.ApplicationName);
                txtreason.Focus(); return;
            }
            else
            {
                string criteria = String.Format("SELECT PaymentRefNumber,PayerName,Amount,PaymentDate,AgencyName,Description,EReceipts,PrintedBY,DatePrinted,ControlNumber ,StationCode, ( SELECT StationName FROM dbo.tblStation WHERE tblCollectionReport.StationCode =tblStation.StationCode ) AS StationName FROM dbo.tblCollectionReport WHERE EReceipts='{0}'", txtreceipt.Text.Trim());

                using (var ds = new System.Data.DataSet())
                {
                    using (SqlDataAdapter ada = new SqlDataAdapter(criteria, Logic.ConnectionString))
                    {
                        ada.Fill(ds, "table");
                    }
                    dt = ds.Tables[0];

                }

                if (dt != null && dt.Rows.Count > 0)
                {
                    insertRecord(dt);
                }
                else
                {
                    Common.setMessageBox("Sorry No Record Found to Cancel", "Receipt Cancel", 1);
                    return;
                }
            }
        }

        void insertRecord(DataTable dt)
        {
            using (WaitDialogForm form = new WaitDialogForm("Application Working...,Please Wait...", "Processing Request "))
            {

                //reading data content

                foreach (DataRow item in dt.Rows)
                {
                    if (item == null) continue;

                    item["Amount"].ToString();

                    using (SqlConnection db = new SqlConnection(Logic.ConnectionString))
                    {
                        SqlTransaction transaction;

                        db.Open();

                        transaction = db.BeginTransaction();
                        try
                        {
                            using (SqlCommand sqlCommand1 = new SqlCommand(String.Format("INSERT INTO tblReceiptCancel( ReceiptNo ,PaymentRefNo , Reason ,Userid,CancelDate)VALUES ('{0}','{1}','{2}','{3}','{4}');", (string)item["EReceipts"], (string)item["PaymentRefNumber"], txtreason.Text.Trim(), Program.UserID, string.Format("{0:yyyy/MM/dd}", DateTime.Now)), db, transaction))
                            {
                                sqlCommand1.ExecuteNonQuery();
                            }

                            string query1 = String.Format("UPDATE tblCollectionReport SET ControlNumber=NULL,isPrinted=0,IsPrintedDate=NULL,BatchNumber=NULL,DatePrinted=NULL,PrintedBY=NULL WHERE EReceipts='{0}'", (string)item["EReceipts"]);

                            using (SqlCommand sqlCommand = new SqlCommand(query1, db, transaction))
                            {
                                sqlCommand.ExecuteNonQuery();
                            }

                            transaction.Commit();

                            Common.setMessageBox("Request Sucessfull", "Receipt Cancel", 1);
                        }
                        catch (SqlException sqlError)
                        {
                            Tripous.Sys.ErrorBox(sqlError);
                            transaction.Rollback();
                        }
                        db.Close();
                    }

                    //record to the cancel table

                }
            }
        }

        void createTable()
        {
            //SQLiteConnection sqlite_conn;
            //SQLiteCommand sqlite_cmd;
            //SQLiteDataReader sqlite_datareader;

            SqlConnection sqlite_conn;
            SqlCommand sqlite_cmd;
            SqlDataReader sqlite_datareader;

            // create a new database connection:
            sqlite_conn = new SqlConnection(Logic.ConnectionString);

            sqlite_conn.Open();

            sqlite_cmd = sqlite_conn.CreateCommand();

            //sqlite_cmd.CommandText = "create table if not exists CREATE TABLE [tblReceiptCancel]([ReceiptCancelID] [int] IDENTITY(1,1) NOT NULL,	[ReceiptNo] [varchar](50) NULL,	[PaymentRefNo] [varchar](50) NULL,	[Reason] [varchar](1000) NULL,	[Userid] [varchar](50) NULL,	[CancelDate] [datetime] NULL);";

            sqlite_cmd.CommandText = "IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tblReceiptCancel]') AND type in (N'U')) BEGIN CREATE TABLE    create table if not exists CREATE TABLE [tblReceiptCancel]([ReceiptCancelID] [int] IDENTITY(1,1) NOT NULL,	[ReceiptNo] [varchar](50) NULL,	[PaymentRefNo] [varchar](50) NULL,	[Reason] [varchar](1000) NULL,	[Userid] [varchar](50) NULL,	[CancelDate] [datetime] NULL) END";

            int row = sqlite_cmd.ExecuteNonQuery();
        }
    }
}
