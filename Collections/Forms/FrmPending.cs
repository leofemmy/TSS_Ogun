using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TaxSmartSuite;
using TaxSmartSuite.Class;
using Collection.Classess;
using System.Data.SqlClient;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.Utils;
using Collection.ReceiptServices;
using DevExpress.XtraGrid.Selection;
using Collection.Report;
using DevExpress.XtraReports.UI;
using DevExpress.XtraReports.Parameters;

namespace Collection.Forms
{
    public partial class FrmPending : Form
    {
        private SqlCommand _command;
        private SqlDataAdapter ada;

        private SqlCommand command;

        public static FrmPending publicStreetGroup;

        protected TransactionTypeCode iTransType;

        protected bool boolIsUpdate; SqlDataAdapter adp;

        bool IsShowDialog; DataTable dtget = new DataTable();

        System.Data.DataSet dsreturn = new System.Data.DataSet();

        System.Data.DataSet dstretval = new System.Data.DataSet();

        System.Data.DataSet dsGetapp = new System.Data.DataSet();

        System.Data.DataSet dsGetappup = new System.Data.DataSet();

        GridCheckMarksSelection selection; bool isFirstGrid = true;

        private string strCollectionReportID;
        private string user;

        bool isEmpty ;

        AmountToWords amounttowords = new AmountToWords();

        string BatchNumber, values, values1, values2, query, criteria3, criteria2;

        public FrmPending()
        {
            InitializeComponent();

            publicStreetGroup = this;

            iTransType = TransactionTypeCode.New;

            setImages();

            ToolStripEvent();

            Load += OnFormLoad;

            OnFormLoad(null, null);

            this.IsShowDialog = false;
        }

        public FrmPending(bool IsShowDialog)
        {
            InitializeComponent();

            publicStreetGroup = this;

            iTransType = TransactionTypeCode.New;

            setImages();

            ToolStripEvent();

            Load += OnFormLoad;

            this.IsShowDialog = IsShowDialog;

            if (!this.IsShowDialog)

                OnFormLoad(null, null);
        }

        private void setImages()
        {
            tsbNew.Image = MDIMain.publicMDIParent.i16x16.Images[5];
            tsbEdit.Image = MDIMain.publicMDIParent.i16x16.Images[6];
            tsbDelete.Image = MDIMain.publicMDIParent.i16x16.Images[7];
            tsbReload.Image = MDIMain.publicMDIParent.i16x16.Images[0];
            tsbClose.Image = MDIMain.publicMDIParent.i16x16.Images[11];

            //bttnCancel.Image = MDIMain.publicMDIParent.i32x32.Images[9];
            //btnPrint.Image = MDIMain.publicMDIParent.i32x32.Images[29];
            btnConfirm.Image = MDIMain.publicMDIParent.i32x32.Images[10];
            btnGet.Image = MDIMain.publicMDIParent.i32x32.Images[2];
            btnPrint.Image = MDIMain.publicMDIParent.i32x32.Images[29];
            btnMain.Image = MDIMain.publicMDIParent.i32x32.Images[8];


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
                if (!IsShowDialog)
                { MDIMain.publicMDIParent.RemoveControls(); }
                else
                { this.Close(); }


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
            ShowForm(); SetReload();

            gridView1.ValidatingEditor += gridView1_ValidatingEditor;

            btnConfirm.Click += bttn;

            btnGet.Click += bttn;

            btnPrint.Click += bttn;

            btnMain.Click += bttn;

            if (Program.UserID == "" || Program.UserID == null)
            {
                user = "Femi";
            }
            else
            {
                user = Program.UserID;
            }

        }

        void gridView1_ValidatingEditor(object sender, DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventArgs e)
        {
            GridView view = sender as GridView;

            if (view != null)
            {
                if (view.FocusedColumn.FieldName == "Reasons")
                {
                    object obj = e.Value;

                    if (string.IsNullOrEmpty(obj.ToString()))
                    {
                        e.Valid = false;
                        e.ErrorText = " Reason Can't be Empty ";
                    }
                }
            }

        }

        public void RefreshForm()
        {
            NavBars.ToolStripEnableDisableControls(toolStrip, Tag as String);
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
                    splitContainer1.Panel2Collapsed = true;
                    break;
                case TransactionTypeCode.Edit:
                    splitContainer1.Panel1Collapsed = true;
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

        void SetReload()
        {
            string qry = string.Format("SELECT PaymentRefNumber,ReceiptNo,UploadStatus as Status  FROM tblReceipt WHERE SentBy='{0}' ", Program.UserID);

            try
            {
                using (var ds = new System.Data.DataSet())
                {
                    ada = new SqlDataAdapter(qry, Logic.ConnectionString);

                    ada.Fill(ds, "table");

                    dtget = ds.Tables[0];
                }
            }
            catch (Exception ex)
            {
                Common.setMessageBox(ex.Message, Program.ApplicationName, 3);
                return;
            }

            if (dtget.Rows.Count > 0)
            {
                dtget.Columns.Add("Reasons", typeof(string));
                dtget.AcceptChanges();

                gridControl1.DataSource = dtget;
                gridView2.Columns["PaymentRefNumber"].OptionsColumn.AllowEdit = false;
                gridView2.Columns["ReceiptNo"].OptionsColumn.AllowEdit = false;
                gridView2.Columns["Reasons"].OptionsColumn.AllowEdit = true;
                gridView2.BestFitColumns();
                gridControl1.ForceInitialize();
            }
        }

        void bttn(object sender, EventArgs e)
        {
            if (sender == btnConfirm)
            {
                //loop the TableLayoutCellPaintEventArgs if reason is empty
                foreach (DataRow row in dtget.Rows)
                {
                    if ((row["Reasons"] is DBNull) || (row["Reasons"] == "") || (string.IsNullOrEmpty(Convert.ToString(row["Reasons"]))))
                    {
                        isEmpty = true;
                        Common.setMessageBox("Reason Can't be Empty", "Request Sending", 2); break;
                    }
                    else
                    {
                        
                        using (WaitDialogForm form = new WaitDialogForm("Application Working...,Please Wait...", "Sending Request"))
                        {
                            try
                            {
                                using (var receiptsserv = new ReceiptService())
                                {
                                    dsreturn = receiptsserv.LogReceiptsReprintRequest((string)row["ReceiptNo"], (string)row["PaymentRefNumber"], string.Empty, Program.UserID, (string)row["Reasons"], "NONE", string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, Program.stationCode);
                                }
                                isEmpty = false;
                            }
                            catch (Exception ex)
                            {
                                Common.setMessageBox(string.Format("{0}----{1}..Sending Request Reprint of Receipt Failed", ex.Message, ex.StackTrace), Program.ApplicationName, 3);
                                return;
                            }
                        }
                    }
                }

                if (!isEmpty)
                {
                    //get return message from online
                    if (dsreturn.Tables[0].Rows[0]["returnmessage"].ToString() == "-1")
                    {
                        Common.setMessageBox(dsreturn.Tables[0].Rows[0]["returnmessage1"].ToString(), Program.ApplicationName, 1);
                        return;
                    }
                    else
                    {

                        using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
                        {
                            connect.Open();
                            _command = new SqlCommand("UpdateRequestSent", connect) { CommandType = CommandType.StoredProcedure };
                            _command.Parameters.Add(new SqlParameter("@status", SqlDbType.VarChar)).Value = "Sent";
                            _command.Parameters.Add(new SqlParameter("@tblCollectionReport_Temp", SqlDbType.Structured)).Value = dtget;

                            System.Data.DataSet response = new System.Data.DataSet();

                            adp = new SqlDataAdapter(_command);
                            adp.Fill(response);

                            connect.Close();
                            if (response.Tables[0].Rows[0]["returnCode"].ToString() != "00")
                            {
                                Common.setMessageBox(response.Tables[0].Rows[0]["returnMessage"].ToString(), "Manifest", 2); return;
                            }
                            else
                            {
                                Common.setMessageBox("Request Sent for Approval..", Program.ApplicationName, 1);
                                Common.setMessageBox("Call your Admin Office for Record Aproval..,Click on Get Record Approval to get Record", Program.ApplicationName, 1);
                            }
                        }
                        
                        return;
                    }
                }
               

            }
            else if (sender == btnMain)
            {
                using (FrmMainFest frmMainFest = new FrmMainFest(strCollectionReportID, criteria3, criteria2,true) { IDList = strCollectionReportID })
                {
                    frmMainFest.ShowDialog();
                }
            }
            else if (sender == btnPrint)
            {

                if (selection.SelectedCount == 0)
                {

                    Common.setMessageBox("No Selection Made for Printing of Receipts", Program.ApplicationName, 3);
                    return;

                }
                else
                {
                    //criteria3 = GetPayRef();
                    using (SqlConnection db = new SqlConnection(Logic.ConnectionString))
                {
                    SqlTransaction transaction;

                    db.Open();

                    transaction = db.BeginTransaction();

                    try
                    {
                        using (System.Data.DataSet dds = new System.Data.DataSet("DsCollectionReport"))
                        {
                            SqlDataAdapter ada;

                            using (WaitDialogForm form = new WaitDialogForm())
                            {
                                string strFormat = null;

                                query = string.Format("SELECT [ID] , [Provider] , [Channel] ,[PaymentRefNumber] , [DepositSlipNumber] , CONVERT(VARCHAR,CONVERT(DATETime,[PaymentDate])) AS PaymentDate,[PayerID] , UPPER([PayerName]) AS [PayerName], [Amount] ,[PaymentMethod] ,[ChequeNumber] ,[ChequeValueDate] ,[ChequeStatus] ,[DateChequeReturned] ,[TelephoneNumber] ,[ReceiptNo] ,[ReceiptDate] ,UPPER([PayerAddress]) as [PayerAddress] ,[Status] ,[User] ,[RevenueCode] ,[Description] , [ChequeBankCode] ,[ChequeBankName] ,[AgencyName] ,[AgencyCode] ,[BankCode] ,[BankName] ,[BranchCode] ,[BranchName] ,[ZoneCode] ,[ZoneName] ,[Username] ,[State] ,[AmountWords] ,[URL] ,[EReceipts] ,[EReceiptsDate] ,[GeneratedBy] ,[DateValidatedAgainst] ,[DateDiff] ,[UploadStatus] ,[PrintedBY] ,[DatePrinted] ,[ControlNumber] ,[BatchNumber] ,[StationCode] ,(Select StationName from tblStation2 WHERE tblStation2.StationCode = tblCollectionReport.[StationCode]) AS StationName from tblCollectionReport WHERE RevenueCode NOT IN (SELECT RevenueCode FROM [tblPrintingRevenueCode]) AND PaymentRefNumber IN (SELECT PaymentRefNumber FROM  [tblReceipt] where sentby ='{0}' AND Isprinted=0) AND EReceipts IN (SELECT ReceiptNo FROM  [tblReceipt]) ORDER BY tblCollectionReport.StationCode , tblCollectionReport.AgencyCode ,tblCollectionReport.RevenueCode,tblCollectionReport.EReceipts", Program.UserID);

                                DataTable Dt = dds.Tables.Add("CollectionReportTable");
                                ada = new SqlDataAdapter(query, Logic.ConnectionString);
                                ada.Fill(dds, "CollectionReportTable");
                                Logic.ProcessDataTable(Dt);;
                                strCollectionReportID = strFormat;
                            }
                            

                            XRepReceipt recportRec = new XRepReceipt { DataSource = dds /*recportRec.DataAdapter = ada;*/, DataMember = "CollectionReportTable" };
                                recportRec.logoPath= Logic.singaturepth;
                                recportRec.ShowPreviewDialog();


                        }


                    }
                    catch (SqlException sqlError)
                    {
                        //transaction.Rollback();

                        Tripous.Sys.ErrorBox(sqlError);
                    }
                    catch (Exception ex)
                    {
                        Tripous.Sys.ErrorBox(ex);
                    }
                    db.Close();
                }


                btnMain.Enabled = true;

                btnPrint.Enabled = false;

                //btnSearch.Enabled = false;

                gridControl4.Enabled = false;

                //Common.setMessageBox(convert.tostring(DateTime.Now), Program.ApplicationName, 1);



                //ask if the print was sucessfull
                DialogResult result = MessageBox.Show(" Is Receipt Printing Successful ?", Program.ApplicationName, MessageBoxButtons.YesNo);

                if (result == DialogResult.Yes)
                {
                    //update the collection table by seting the isprinted to true
                    using (SqlConnection db = new SqlConnection(Logic.ConnectionString))
                    {
                        SqlTransaction transaction;

                        db.Open();

                        transaction = db.BeginTransaction();


                        try
                        {
                            string query1 = String.Format("UPDATE tblCollectionReport SET isPrinted=1,IsPrintedDate= '{0:MM/dd/yyyy HH:mm:ss tt}',PrintedBY='{1}' WHERE PaymentRefNumber IN (SELECT PaymentRefNumber FROM  [tblReceipt] where SentBy='{1}')", DateTime.Now, Program.UserID);

                            using (SqlCommand sqlCommand = new SqlCommand(query1, db, transaction))
                            {
                                sqlCommand.ExecuteNonQuery();
                            }

                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            Tripous.Sys.ErrorBox(ex);
                            return;
                        }
                        transaction.Commit();

                        db.Close();
                    }
                }

                

                
                //    //}
                //    //else
                //    //{
                //    //    return;
                //    //}


                }
                //else
                //    return;
            }
            else if (sender == btnGet)
            {
                try
                {
                    using (var receiptsserv = new ReceiptService())
                    {
                        dsGetapp = receiptsserv.GetReceiptReprintApproved(Program.stationCode);
                    }
                }
                catch (Exception ex)
                {
                    Common.setMessageBox(ex.StackTrace + ex.Message.ToString(), "Error", 3); return;
                }

                if (dsGetapp.Tables[0].Rows[0]["returnmessage"].ToString() == "-1")
                {
                    Common.setMessageBox(dsGetapp.Tables[0].Rows[0]["returnmessage1"].ToString(), Program.ApplicationName, 1);
                    return;
                }
                else
                {
                    if (dsGetapp.Tables.Count == 0 || dsGetapp.Tables[0].Rows.Count < 1)
                    {
                        Common.setMessageBox("No records have been approval yet", "Get Approval Record", 1);
                        return;
                    }
                    else
                    {
                        using (WaitDialogForm form = new WaitDialogForm("Application Working...,Please Wait...", "Getting Approval Records"))
                        {
                            //insert record into collection
                            dstretval = InsertData(dsGetapp);

                            //send return data online for update
                            try
                            {
                                using (var receiptsserv = new ReceiptService())
                                {
                                    dsGetappup = receiptsserv.UpdateReceiptReprintApproved(dstretval);
                                }

                            }
                            catch (Exception ex)
                            {
                                
                            }
                            if (dsGetappup.Tables[0].Rows[0]["returnCode"].ToString() == "00")
                            {
                                iTransType = TransactionTypeCode.Edit;
                                voidgetRec(dstretval);
                                OnFormLoad(null, null); return;
                            }
                            else
                            {
                                Common.setMessageBox(string.Format("Error Updating Approval Record, {0}...", dsGetappup.Tables[0].Rows[0]["returnMessage"]), Program.ApplicationName, 3);
                                return;
                            }
                        }
                    }
                }
            }

        }

        public System.Data.DataSet InsertData(System.Data.DataSet dataSet)
        {
            System.Data.DataSet ds = new System.Data.DataSet();

            try
            {
                using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
                {
                    connect.Open();

                    command = new SqlCommand("doInsertModifyReceipt", connect) { CommandType = CommandType.StoredProcedure };
                    command.Parameters.Add(new SqlParameter("@Userid", SqlDbType.VarChar)).Value = Program.UserID;
                    command.Parameters.Add(new SqlParameter("@tblCollectionReport_Temp", SqlDbType.Structured)).Value = dataSet.Tables[1];
                    command.CommandTimeout = 10000;

                    //using (System.Data.DataSet dsf = new System.Data.DataSet())
                    //{
                    adp = new SqlDataAdapter(command);
                    adp.Fill(ds);
                    //Dts = ds.Tables[0];
                    connect.Close();
                    //}
                }


                return ds;
            }
            catch (Exception ex)
            {
                Common.setMessageBox(String.Format("{0}----{1}...Insert Data Record to Station", ex.StackTrace, ex.Message), Program.ApplicationName, 3);

                return ds;
            }
            return ds;
        }

        void voidgetRec(System.Data.DataSet dt)
        {
            DataTable dtn = new DataTable();
            //values = string.Empty;
            //DataTable dtf = dt.Tables[0];

            //int j = 0;


            //foreach (DataRow dr in dtf.Rows)
            //{
            //    //MessageBox.Show(dr["paymentRefNumber"].ToString());

            //    values += String.Format("'{0}'", dr["paymentRefNumber"]);

            //    if (j + 1 < dtf.Rows.Count)
            //        values += ",";
            //    ++j;
            //}


            string quy = String.Format("SELECT  ID , PaymentRefNumber,DepositSlipNumber,CONVERT(VARCHAR, CONVERT(DATE, PaymentDate), 103) as PaymentDate,[PayerID],UPPER(PayerName) as PayerName,AgencyName,AgencyCode,Description,RevenueCode,Amount,BankName+ '-'+ BranchName AS Bank,EReceipts,[GeneratedBy],[BankName],[BranchName],StationCode from tblCollectionReport WHERE RevenueCode NOT IN (SELECT RevenueCode FROM [tblPrintingRevenueCode]) AND RevenueCode NOT IN ( SELECT RevenueCode FROM   tblRevenueReceiptException ) AND PaymentRefNumber IN (SELECT PaymentRefNumber FROM  [tblReceipt] where SentBy='{0}') ORDER BY tblCollectionReport.StationCode , tblCollectionReport.AgencyCode ,tblCollectionReport.RevenueCode,tblCollectionReport.EReceipts", Program.UserID);

            try
            {
                using (var dsds = new System.Data.DataSet())
                {
                    using (SqlDataAdapter ada = new SqlDataAdapter(quy, Logic.ConnectionString))
                    {
                        ada.Fill(dsds);
                    }
                    dtn = dsds.Tables[0];
                    gridControl4.DataSource = dtn;
                    gridView4.BestFitColumns();
                    gridView4.Columns["Amount"].DisplayFormat.FormatType = FormatType.Numeric;
                    gridView4.Columns["Amount"].DisplayFormat.FormatString = "n2";
                    gridView4.Columns["PaymentDate"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
                    gridView4.Columns["PaymentDate"].DisplayFormat.FormatString = "dd/MM/yyyy";
                    gridView4.Columns["ID"].Visible = false;
                }

                //label10.Text = String.Format("Total Number ot Payments: {0}", dt.Rows.Count);

            }
            catch (Exception ex)
            {
                Common.setMessageBox(ex.Message, Program.ApplicationName, 3);
                return;
            }

            if (isFirstGrid)
            {
                selection = new GridCheckMarksSelection(gridView4, ref lblSelect);
                selection.CheckMarkColumn.VisibleIndex = 0;
                isFirstGrid = false;
            }
        }

        internal GridCheckMarksSelection Selection
        {
            get { return selection; }
        }

        string GetPayRef()
        {
            string values = string.Empty;

            lblSelect.Text = string.Empty;

            int j = 0;

            using (WaitDialogForm form = new WaitDialogForm("Application Working...,Please Wait...", "Processing Your Request"))
            {

                for (int i = 0; i < selection.SelectedCount; i++)
                {
                    //string lol = ((selection.GetSelectedRow(i) as DataRowView)["PaymentRefNumber"].ToString());

                    values += String.Format("'{0}'", (selection.GetSelectedRow(i) as DataRowView)["PaymentRefNumber"]);
                    if (j + 1 < selection.SelectedCount)
                        values += ",";
                    ++j;

                    //insert record into tblreceipt for processing
                    using (SqlConnection db = new SqlConnection(Logic.ConnectionString))
                    {
                        SqlTransaction transaction;

                        db.Open();

                        transaction = db.BeginTransaction();

                        try
                        {
                            string querys =
                                String.Format(
                                    "INSERT INTO [tblReceipt]([PaymentRefNumber],[ReceiptNo],[DateSent],[SentBy],[AgencyCode] ,[AgencyName],[RevenueCode],[Description],[DepositSlipNumber],[PaymentDate],[PayerID],[PayerName],[Amount],GeneratedBy,[BankName],[BranchName],StationCode) VALUES ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}');", String.Format("{0}", (selection.GetSelectedRow(i) as DataRowView)["PaymentRefNumber"]), String.Format("{0}", (selection.GetSelectedRow(i) as DataRowView)["EReceipts"]), DateTime.Now, Program.UserID, String.Format("{0}", (selection.GetSelectedRow(i) as DataRowView)["AgencyCode"]), String.Format("{0}", (selection.GetSelectedRow(i) as DataRowView)["AgencyName"]), String.Format("{0}", (selection.GetSelectedRow(i) as DataRowView)["RevenueCode"]), String.Format("{0}", (selection.GetSelectedRow(i) as DataRowView)["Description"]), String.Format("{0}", (selection.GetSelectedRow(i) as DataRowView)["DepositSlipNumber"]), String.Format("{0}", (selection.GetSelectedRow(i) as DataRowView)["PaymentDate"]), String.Format("{0}", (selection.GetSelectedRow(i) as DataRowView)["PayerID"]), String.Format("{0}", (selection.GetSelectedRow(i) as DataRowView)["PayerName"]), String.Format("{0}", (selection.GetSelectedRow(i) as DataRowView)["Amount"]), String.Format("{0}", (selection.GetSelectedRow(i) as DataRowView)["GeneratedBy"]), String.Format("{0}", (selection.GetSelectedRow(i) as DataRowView)["BankName"]), String.Format("{0}", (selection.GetSelectedRow(i) as DataRowView)["BranchName"]), Program.stationCode);

                            using (SqlCommand sqlCommand2 = new SqlCommand(querys, db, transaction))
                            {
                                sqlCommand2.ExecuteNonQuery();
                            }

                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            Tripous.Sys.ErrorBox(ex);
                            transaction.Rollback();
                            //return;
                        }
                        db.Close();

                    }
                }

            }



            return values;
        }

        void ProcessDataTable(DataTable Dt)
        {
            if (Dt != null && Dt.Rows.Count > 0)
            {
                foreach (DataRow item in Dt.Rows)
                {
                    if (item == null) continue;
                    //decimal amount = decimal.Parse(item["Amount"].ToString());
                    try
                    {
                        item["AmountWords"] = amounttowords.convertToWords(item["Amount"].ToString(), item["prefix"].ToString(), item["Surfix"].ToString());

                        if (item["PayerID"].ToString().Length > 14)
                        {
                            item["PayerID"] = "None Yet Please approach the BIR for your Unique Payer ID.";
                        }
                        else
                        {
                            item["PayerID"] = item["PayerID"].ToString();
                            item["PayerID"] = string.Format("Your Payer ID which is <<{0}>> must be quoted in all transaction", item["PayerID"]);
                        }

                        item["ZoneCode"] = item["StationCode"].ToString();
                        item["ZoneName"] = item["StationName"].ToString();
                        item["Description"] = item["Description"].ToString();
                        //item["URL"] = string.Format(@"Payment for {0} {1} << Paid at {2} - {3} , Deposit Slip Number {4} by {5}  >> ", item["Description"], item["RevenueCode"], item["BankName"], item["BranchName"], item["DepositSlipNumber"], item["PaymentMethod"]);

                        item["URL"] = string.Format("Paid at {0} , {1}  Deposit Slip Number {2} in respect of {3} <{4}> By {5}", item["BankName"], item["BranchName"], item["DepositSlipNumber"], item["Description"], item["RevenueCode"], item["PaymentMethod"]);

                        item["User"] = Program.UserID.ToUpper();

                        item["Username"] = string.Format(@"</Printed at {0} Zonal Office  by {1} on {2}/>", item["StationName"], user, DateTime.Today.ToString("dd-MMM-yyyy"));

                        item["PaymentDate"] = Convert.ToDateTime(item["PaymentDate"]).ToString("dd-MMM-yyyy");
                    }
                    catch
                    {

                    }
                }
            }
        }

    }
}
