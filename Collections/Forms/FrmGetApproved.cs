using Collection.Classess;
using Collection.ReceiptServices;
using Collection.Report;
using Collections;
using DevExpress.Utils;
using DevExpress.XtraGrid.Selection;
using DevExpress.XtraSplashScreen;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TaxSmartSuite.Class;
using DevExpress.XtraPrinting.Drawing;
using DevExpress.XtraReports.UI;




namespace Collection.Forms
{
    public partial class FrmGetApproved : Form
    {
        public static FrmGetApproved publicStreetGroup;

        AmountToWords amounttowords = new AmountToWords();

        protected FrmGetApproved iTransType;

        public static FrmGetApproved publicInstance;

        protected bool boolIsUpdate; private string strCollectionReportID;

        string criteria, strreprint;

        private System.Data.DataSet ds;

        private SqlDataAdapter adp;

        private SqlCommand command;

        System.Data.DataSet dsGetappup = new System.Data.DataSet();

        System.Data.DataSet dsc = new System.Data.DataSet();

        System.Data.DataSet dsreturn = new System.Data.DataSet();

        System.Data.DataSet dsGetapp = new System.Data.DataSet();

        System.Data.DataSet dstretval = new System.Data.DataSet();

        private SqlCommand _command; DataTable temTable = new DataTable();

        string BatchNumber, values, values1, values2, query, criteria3, criteria2;

        bool isFirstGrid = true; bool isReprint = true; DataTable dtn = new DataTable();

        GridCheckMarksSelection selection;
        public FrmGetApproved()
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

                InitializeComponent();

                publicInstance = this;

                publicStreetGroup = this;

                //iTransType = TransactionTypeCode.New;
                //setImages();

                ToolStripEvent();

                Load += OnFormLoad;

                btnGet.Click += btnGet_Click;

                btnPrint.Click += btnPrint_Click;

                btnMain.Click += btnMain_Click;

                temTable.Columns.Add("EReceipt", typeof(string));
                temTable.Columns.Add("PaymentRefNumber", typeof(string));
                temTable.Columns.Add("UserId", typeof(string));

                OnFormLoad(null, null);

                Program.IsReprint = false;
            }
            finally
            {
                SplashScreenManager.CloseForm(false);
            }
        }

        void btnMain_Click(object sender, EventArgs e)
        {
            if (dtn == null)
            {
                Common.setMessageBox("No Records Printing of Receipts", Program.ApplicationName, 3);
                return;
            }
            else
            {
                using (FrmMainFest frmMainFest = new FrmMainFest(strCollectionReportID, values, criteria2, false) { IDList = strCollectionReportID })
                {
                    frmMainFest.ShowDialog();
                }

                gridControl4.Enabled = false;
            }
        }

        void btnPrint_Click(object sender, EventArgs e)
        {
            //if (dtn == null)
            //{
            //    Common.setMessageBox("No Records Printing of Receipts", Program.ApplicationName, 3);
            //    return;
            //}
            if (selection.SelectedCount == 0)
            {

                Common.setMessageBox("No Selection Made for Printing of Receipts", Program.ApplicationName, 3);
                return;

            }
            else
            {
                GetPayRef();


                ////ask if the print was sucessfull
                DialogResult result = MessageBox.Show("Is Receipt Printing Successful ?", Program.ApplicationName, MessageBoxButtons.YesNo);

                if (result == DialogResult.Yes)
                {
                    try
                    {
                        SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

                        //update the collection table by seting the isprinted to true
                        using (SqlConnection db = new SqlConnection(Logic.ConnectionString))
                        {
                            SqlTransaction transaction;

                            db.Open();

                            transaction = db.BeginTransaction();
                            try
                            {
                                string query1 = String.Format("UPDATE Receipt.tblCollectionReceipt SET isPrinted=1,IsPrintedDate= '{0:MM/dd/yyyy HH:mm:ss tt}',PrintedBY='{1}',StationCode='{2}' WHERE PaymentRefNumber IN ({3})", DateTime.Now, Program.UserID, Program.stationCode, values);

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
                    finally
                    {
                        SplashScreenManager.CloseForm(false);
                    }
                }
                else
                {
                    try
                    {
                        SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

                        using (SqlConnection db = new SqlConnection(Logic.ConnectionString))
                        {
                            SqlTransaction transaction;

                            db.Open();

                            transaction = db.BeginTransaction();
                            try
                            {
                                //string query1 = String.Format("DELETE FROM Receipt.tblCollectionReceipt WHERE PaymentRefNumber IN (SELECT PaymentRefNumber FROM Receipt.tblReceipt WHERE SentBy='{0}')", Program.UserID);

                                //using (SqlCommand sqlCommand = new SqlCommand(query1, db, transaction))
                                //{
                                //    sqlCommand.ExecuteNonQuery();
                                //}

                                string query = String.Format("DELETE  FROM Receipt.tblReceipt where SentBy='{0}')", Program.UserID);

                                using (SqlCommand sqlCommand1 = new SqlCommand(query, db, transaction))
                                {
                                    sqlCommand1.ExecuteNonQuery();
                                }

                                transaction.Commit();
                            }
                            catch (Exception ex)
                            {
                                transaction.Rollback();
                                Tripous.Sys.ErrorBox(ex);
                                return;
                            }


                            db.Close();
                        }
                    }
                    finally
                    {
                        SplashScreenManager.CloseForm(false);
                    }
                    return;
                }


            }
        }

        void btnGet_Click(object sender, EventArgs e)
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

                switch (Program.intCode)
                {
                    case 13://Akwa Ibom state
                        using (var receiptAka = new AkwaIbomReceiptServices.ReceiptService())
                        {
                            dsGetapp = receiptAka.GetReceiptReprintApproved(Program.stationCode);
                        }

                        break;
                    case 20://Delta state
                        using (var receiptDelta = new DeltaBir.ReceiptService())
                        {
                            dsGetapp = receiptDelta.GetReceiptReprintApproved(Program.stationCode);
                        }
                        break;
                    case 32://kogi state
                        break;

                    case 37://ogun state
                        using (var receiptsserv = new ReceiptService())
                        {
                            dsGetapp = receiptsserv.GetReceiptReprintApproved(Program.stationCode);
                        }
                        break;

                    case 40://oyo state
                        using (var receiptsserv = new OyoReceiptServices.ReceiptService())
                        {
                            dsGetapp = receiptsserv.GetReceiptReprintApproved(Program.stationCode);
                        }
                        break;

                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                Common.setMessageBox(ex.StackTrace, "Get Approval Record", 3); return;
            }
            finally
            {
                SplashScreenManager.CloseForm(false);
            }


            if (dsGetapp.Tables[0].Rows[0]["returncode"].ToString() == "-1")
            {
                Common.setMessageBox(dsGetapp.Tables[0].Rows[0]["returnmessage"].ToString(), Program.ApplicationName, 1);
                SetLoadData();
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
                    //insert record into collection
                    dstretval = InsertData(dsGetapp);


                    try
                    {
                        SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                        if (dstretval != null && dstretval.Tables[0].Rows.Count > 0)
                        {
                            switch (Program.intCode)
                            {
                                case 13://Akwa Ibom state
                                    using (var receiptAka = new AkwaIbomReceiptServices.ReceiptService())
                                    {
                                        dsGetappup = receiptAka.UpdateReceiptReprintApproved(dstretval);
                                    }

                                    break;
                                case 20://Delta state
                                    using (var receiptDelta = new DeltaBir.ReceiptService())
                                    {
                                        dsGetappup = receiptDelta.UpdateReceiptReprintApproved(dstretval);
                                    }
                                    break;
                                case 32://kogi state
                                    break;

                                case 37://ogun state
                                    using (var receiptsserv = new ReceiptService())
                                    {
                                        dsGetappup = receiptsserv.UpdateReceiptReprintApproved(dstretval);
                                    }
                                    break;

                                case 40://oyo state
                                    using (var receiptsserv = new OyoReceiptServices.ReceiptService())
                                    {
                                        dsGetappup = receiptsserv.UpdateReceiptReprintApproved(dstretval);
                                    }
                                    break;

                                default:
                                    break;
                            }
                        }
                        else
                            return;
                        //send return data online for update

                    }
                    catch (Exception ex)
                    {
                        Common.setMessageBox(ex.StackTrace, "Get Approval Record", 3); return;
                    }

                    finally
                    {
                        SplashScreenManager.CloseForm(false);
                    }

                    if (dsGetappup.Tables[0].Rows[0]["returnCode"].ToString() == "00")
                    {
                        //iTransType = TransactionTypeCode.Edit;
                        voidgetRec(dstretval);
                        OnFormLoad(null, null);
                        return;
                    }
                    else
                    {
                        Common.setMessageBox(string.Format("Error Updating Approval Record, {0}...", dsGetappup.Tables[0].Rows[0]["returnMessage"]), Program.ApplicationName, 3);
                        return;
                    }

                }
            }
        }

        public void RefreshForm()
        {
            NavBars.ToolStripEnableDisableControls(toolStrip, Tag as String);
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
                    command.CommandTimeout = 0;
                    adp = new SqlDataAdapter(command);
                    adp.Fill(ds);

                    connect.Close();
                    //}
                }


                return ds;
            }
            catch (Exception ex)
            {
                //Common.setMessageBox(String.Format("{0}----{1}...Insert Approved Records to Station", ex.StackTrace, ex.Message), Program.ApplicationName, 3);

                Tripous.Sys.ErrorBox(String.Format("{0}-...Error Insert Approved Records to local Station", ex.Message));

                return ds;
            }
            return ds;
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

                //iTransType = TransactionTypeCode.Edit;

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
            //isReprint = false;
            SetLoadData();
            //ShowForm();
            dtn = null;
        }

        void voidgetRec(System.Data.DataSet dt)
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

                dtn = new DataTable();
                values = string.Empty;
                DataTable dtf = dt.Tables[0];

                int j = 0;

                foreach (DataRow dr in dtf.Rows)
                {
                    //messagebox.show(dr["paymentrefnumber"].tostring());

                    values += string.Format("'{0}'", dr["paymentrefnumber"]);

                    if (j + 1 < dtf.Rows.Count)
                        values += ",";
                    ++j;
                }

                //values = string.Format("'{0}'", "ABMFB|OGPDIPAY|0001|21-7-2016|132335");

                string quy = String.Format("SELECT  ID ,PaymentPeriod, PaymentRefNumber,DepositSlipNumber,PaymentDate,UPPER(PayerName) as PayerName,Description,RevenueCode,Amount,BankName+ '-'+ BranchName AS Bank,EReceipts,EReceiptsDate from Collection.tblCollectionReport WHERE Collection.tblCollectionReport.PaymentRefNumber IN ({0}) ORDER BY AgencyCode ,RevenueCode,EReceipts", values);


                try
                {
                    using (var ds = new System.Data.DataSet())
                    {
                        ds.Clear();

                        using (SqlDataAdapter ada = new SqlDataAdapter(quy, Logic.ConnectionString))
                        {
                            ada.Fill(ds, "table");
                        }
                        //dtn.Clear();
                        dtn = ds.Tables[0];
                        gridControl4.DataSource = ds.Tables[0];
                        gridView4.BestFitColumns();
                        gridView4.Columns["Amount"].DisplayFormat.FormatType = FormatType.Numeric;
                        gridView4.Columns["Amount"].DisplayFormat.FormatString = "n2";
                        gridView4.Columns["PaymentDate"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
                        gridView4.Columns["PaymentDate"].DisplayFormat.FormatString = "dd/MM/yyyy";
                        gridView4.Columns["ID"].Visible = false;
                        gridView4.Columns["EReceiptsDate"].Visible = false;


                        //[GeneratedBy],[BankName],[BranchName]
                    }
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
            finally
            {
                SplashScreenManager.CloseForm(false);
            }


        }

        void GetPayRef()
        {
            string values = string.Empty;

            lblSelect.Text = string.Empty;

            int j = 0;

            temTable.Clear();

            for (int i = 0; i < selection.SelectedCount; i++)
            {
                temTable.Rows.Add(new object[] { String.Format("{0}", (selection.GetSelectedRow(i) as DataRowView)["EReceipts"]), String.Format("{0}", (selection.GetSelectedRow(i) as DataRowView)["PaymentRefNumber"]), Program.UserID });

                values += String.Format("'{0}'", (selection.GetSelectedRow(i) as DataRowView)["PaymentRefNumber"]);

                if (j + 1 < selection.SelectedCount)
                    values += ",";
                ++j;
            }

            Program.IsReprint = false;
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

                if (!isReprint)
                {
                    //using (WaitDialogForm form = new WaitDialogForm("Application Working...,Please Wait...", "Processing Your Request"))
                    //{

                    using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
                    {
                        connect.Open();
                        _command = new SqlCommand("InserttblReceipt", connect) { CommandType = CommandType.StoredProcedure };
                        _command.Parameters.Add(new SqlParameter("@tblCollectionReport_Temp", SqlDbType.Structured)).Value = temTable;
                        _command.Parameters.Add(new SqlParameter("@type", SqlDbType.VarChar)).Value = "Edit";
                        _command.Parameters.Add(new SqlParameter("@userid", SqlDbType.VarChar)).Value = Program.UserID;
                        System.Data.DataSet response = new System.Data.DataSet();
                        response.Clear();
                        adp = new SqlDataAdapter(_command);
                        adp.Fill(response);

                        connect.Close();
                        if (String.Compare(response.Tables[0].Rows[0]["returnCode"].ToString(), "00", false) == 0)
                        {
                            //do something load the report page
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

                                        string strFormat = null;

                                        query = string.Format("SELECT  [ID] , PaymentPeriod,[Provider] , [Channel] , tblCollectionReport.PaymentRefNumber , [DepositSlipNumber] ,CONVERT(VARCHAR, CONVERT(DATE, [PaymentDate])) AS PaymentDate , [PayerID] , UPPER([PayerName]) AS [PayerName] , [Amount] , [PaymentMethod] ,[ChequeNumber] , [ChequeValueDate] , [ChequeStatus] ,[DateChequeReturned] ,[TelephoneNumber] ,  [ReceiptNo] , [ReceiptDate] , UPPER([PayerAddress]) AS [PayerAddress] ,[User] ,   [RevenueCode] , tblCollectionReport.Description , [ChequeBankCode] ,[ChequeBankName] ,   [AgencyName] , [AgencyCode] , [BankCode] , [BankName] , [BranchCode] ,[BranchName] , [ZoneCode] , [ZoneName] ,  [Username] ,[AmountWords] , [EReceipts] , [EReceiptsDate] ,[GeneratedBy] ,[StationCode] ,ControlNumber ,ControlNumberDate , ( SELECT TOP 1 StationName  FROM  Receipt.tblStation  WHERE     tblStation.StationCode = Collection.tblCollectionReport.[StationCode]) AS StationName, Symbol , Surfix , tblCurrency.Description AS prefix FROM    Collection.tblCollectionReport LEFT JOIN Receipt.tblReprintedReceipts ON tblCollectionReport.PaymentRefNumber = tblReprintedReceipts.PaymentRefNumber inner JOIN Reconciliation.tblCurrency ON tblCurrency.CurrencyCode = tblCollectionReport.CurrencyCode WHERE  tblCollectionReport.PaymentRefNumber IN ({0})  ORDER BY Collection.tblCollectionReport.StationCode , Collection.tblCollectionReport.AgencyCode ,Collection.tblCollectionReport.RevenueCode,Collection.tblCollectionReport.EReceipts", values);

                                        DataTable Dt = dds.Tables.Add("CollectionReportTable");
                                        ada = new SqlDataAdapter(query, Logic.ConnectionString);
                                        ada.Fill(dds, "CollectionReportTable");
                                        Logic.ProcessDataTable(Dt); ;
                                        //strCollectionReportID = strFormat;



                                        //XRepReceipt recportRec = new XRepReceipt { DataSource = dds.Tables[0] /*recportRec.DataAdapter = ada;*/, DataMember = "CollectionReportTable" };

                                        //recportRec.ShowPreviewDialog();

                                        switch (Program.intCode)
                                        {
                                            case 13://Akwa Ibom state
                                                XtraRepReceiptAkwa akwa = new XtraRepReceiptAkwa { DataSource = dds.Tables[0] /*recportRec.DataAdapter = ada;*/, DataMember = "CollectionReportTable" };
                                                akwa.ShowPreviewDialog();
                                                break;
                                            case 20://detla state
                                                XtraRepReceiptDelta delta = new XtraRepReceiptDelta { DataSource = dds.Tables[0] /*recportRec.DataAdapter = ada;*/, DataMember = "CollectionReportTable" };
                                                //delta.PrintingSystem.StartPrint += PrintingSystem_StartPrint;
                                                //if (Program.stateCode == "20")
                                                //{
                                                //    if (Convert.ToInt32(radioGroup1.EditValue) != 1)
                                                //    {
                                                //        delta.xrLabel19.Visible = true;
                                                //        //delta.xrLabel13.Visible = true;
                                                //    }

                                                //}
                                                //delta.Watermark.Text = "DUPLICATE";
                                                ////delta.Watermark.TextDirection = DirectionMode.Clockwise;
                                                //delta.Watermark.Font = new Font(delta.Watermark.Font.FontFamily, 40);
                                                //delta.Watermark.ForeColor = Color.DodgerBlue;
                                                //delta.Watermark.TextTransparency = 150;
                                                //delta.Watermark.ShowBehind = false;

                                                delta.logoPath = Logic.singaturepth;
                                                delta.Imagepath = Logic.logopth;
                                                //report.Watermark.Text = "DUPLICATE";
                                                delta.xrLabel3.Visible = true;
                                                //delta.Imagepath = Logic.;
                                                delta.Watermark.Text = "DUPLICATE";
                                                delta.ShowPreviewDialog();
                                                //delta.Print();
                                                break;
                                            case 37://ogun state
                                                XRepReceipt recportRec = new XRepReceipt { DataSource = dds.Tables[0] /*recportRec.DataAdapter = ada;*/, DataMember = "CollectionReportTable" };
                                                //recportRec.logoPath = Logic.singaturepth;
                                                recportRec.logoPath = Logic.singaturepth;
                                                recportRec.ShowPreviewDialog();
                                                break;

                                            case 40://oyo state
                                                XRepReceiptoyo recportRecs = new XRepReceiptoyo { DataSource = dds.Tables[0] /*recportRec.DataAdapter = ada;*/, DataMember = "CollectionReportTable" };
                                                recportRecs.ShowPreviewDialog();
                                                break;

                                            //case 32://kogi state

                                            //    XRepReceiptkogi recportRecko = new XRepReceiptkogi { DataSource = dds.Tables[0] /*recportRec.DataAdapter = ada;*/, DataMember = "CollectionReportTable" };
                                            //    recportRecko.ShowPreviewDialog();

                                            //    break;

                                            default:
                                                break;
                                        }

                                        //selection.ClearSelection(); dds.Clear();
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

                        }
                        else
                        {
                            if (String.Compare(response.Tables[0].Rows[0]["returnCode"].ToString(), "-1", false) == 0)
                            {
                                using (Frmcontrol frmcontrol = new Frmcontrol())
                                {
                                    frmcontrol.gridControl1.DataSource = response.Tables[1];
                                    frmcontrol.gridView1.BestFitColumns();
                                    frmcontrol.label1.Text = "Payment Ref. Number Already been used";
                                    frmcontrol.Text = "Payment Ref. Number Already been used";
                                    frmcontrol.ShowDialog();
                                }
                            }
                            else
                            {
                                Common.setMessageBox(response.Tables[0].Rows[0]["returnMessage"].ToString(), "", 2);
                                return;
                            }
                        }
                    }
                }
                else
                {
                    using (SqlConnection db = new SqlConnection(Logic.ConnectionString))
                    {
                        SqlTransaction transaction;

                        db.Open();

                        transaction = db.BeginTransaction();

                        using (System.Data.DataSet dds = new System.Data.DataSet("DsCollectionReport"))
                        {
                            SqlDataAdapter ada;

                            using (WaitDialogForm form = new WaitDialogForm())
                            {
                                string strFormat = null;

                                query = string.Format("SELECT  [ID] , [Provider] , [Channel] , tblCollectionReport.PaymentRefNumber , [DepositSlipNumber] ,CONVERT(VARCHAR, CONVERT(DATE, [PaymentDate])) AS PaymentDate , [PayerID] , UPPER([PayerName]) AS [PayerName] , [Amount] , [PaymentMethod] ,[ChequeNumber] , [ChequeValueDate] , [ChequeStatus] ,[DateChequeReturned] ,[TelephoneNumber] ,  [ReceiptNo] , [ReceiptDate] , UPPER([PayerAddress]) AS [PayerAddress] ,[User] ,   [RevenueCode] , tblCollectionReport.Description , [ChequeBankCode] ,[ChequeBankName] ,   [AgencyName] , [AgencyCode] , [BankCode] , [BankName] , [BranchCode] ,[BranchName] , [ZoneCode] , [ZoneName] ,  [Username] ,[AmountWords] , [EReceipts] , [EReceiptsDate] ,[GeneratedBy] ,[StationCode] ,ControlNumber ,ControlNumberDate , ( SELECT TOP 1 StationName  FROM  Receipt.tblStation  WHERE     tblStation.StationCode = Collection.tblCollectionReport.[StationCode]) AS StationName, Symbol , Surfix , tblCurrency.Description AS prefix FROM    Collection.tblCollectionReport INNER JOIN Reconciliation.tblCurrency ON tblCurrency.CurrencyCode = tblCollectionReport.CurrencyCode LEFT JOIN Receipt.tblReprintedReceipts ON tblCollectionReport.PaymentRefNumber = tblReprintedReceipts.PaymentRefNumber WHERE  tblCollectionReport.PaymentRefNumber IN ({0}) ORDER BY Collection.tblCollectionReport.EReceipts", values);

                                DataTable Dt = dds.Tables.Add("CollectionReportTable");
                                ada = new SqlDataAdapter(query, Logic.ConnectionString);
                                ada.Fill(dds, "CollectionReportTable");
                                Logic.ProcessDataTable(Dt); ;
                                //strCollectionReportID = strFormat;
                            }


                            //XRepReceipt recportRec = new XRepReceipt { DataSource = dds.Tables[0] /*recportRec.DataAdapter = ada;*/, DataMember = "CollectionReportTable" };

                            //recportRec.ShowPreviewDialog();

                            //selection.ClearSelection(); dds.Clear();
                            switch (Program.intCode)
                            {
                                case 13://Akwa Ibom state
                                    XtraRepReceiptAkwa akwa = new XtraRepReceiptAkwa { DataSource = dds.Tables[0] /*recportRec.DataAdapter = ada;*/, DataMember = "CollectionReportTable" };
                                    akwa.ShowPreviewDialog();
                                    break;

                                case 37://ogun state
                                    XRepReceipt recportRec = new XRepReceipt { DataSource = dds.Tables[0] /*recportRec.DataAdapter = ada;*/, DataMember = "CollectionReportTable" };
                                    recportRec.logoPath = Logic.singaturepth;
                                    recportRec.ShowPreviewDialog();
                                    break;
                                case 20://detla state
                                    XtraRepReceiptDelta delta = new XtraRepReceiptDelta { DataSource = dds.Tables[0] /*recportRec.DataAdapter = ada;*/, DataMember = "CollectionReportTable" };
                                    //delta.PrintingSystem.StartPrint += PrintingSystem_StartPrint;
                                    //if (Program.stateCode == "20")
                                    //{
                                    //    if (Convert.ToInt32(radioGroup1.EditValue) != 1)
                                    //    {
                                    //        delta.xrLabel19.Visible = true;
                                    //        //delta.xrLabel13.Visible = true;
                                    //    }

                                    //}
                                    //delta.Watermark.Text = "DUPLICATE";
                                    ////delta.Watermark.TextDirection = DirectionMode.Clockwise;
                                    //delta.Watermark.Font = new Font(delta.Watermark.Font.FontFamily, 40);
                                    //delta.Watermark.ForeColor = Color.DodgerBlue;
                                    //delta.Watermark.TextTransparency = 150;
                                    //delta.Watermark.ShowBehind = false;

                                    delta.logoPath = Logic.singaturepth;
                                    delta.Imagepath = Logic.logopth;
                                    //report.Watermark.Text = "DUPLICATE";
                                    delta.xrLabel3.Visible = true; delta.logoPath = Logic.singaturepth;
                                    delta.Watermark.Text = "DUPLICATE";
                                    delta.ShowPreviewDialog();
                                    //delta.Print();
                                    break;
                                case 40://oyo state
                                    XRepReceiptoyo recportRecs = new XRepReceiptoyo { DataSource = dds.Tables[0] /*recportRec.DataAdapter = ada;*/, DataMember = "CollectionReportTable" };
                                    recportRecs.ShowPreviewDialog();
                                    break;

                                //case 32://kogi state

                                //    XRepReceiptkogi recportRecko = new XRepReceiptkogi { DataSource = dds.Tables[0] /*recportRec.DataAdapter = ada;*/, DataMember = "CollectionReportTable" };
                                //    recportRecko.ShowPreviewDialog();

                                //    break;

                                default:
                                    break;
                            }

                        }

                    }
                }
            }
            finally
            {
                SplashScreenManager.CloseForm(false);
            }

            //return values;
        }

        void ProcessDataTable(DataTable Dt)
        {
            if (Dt != null && Dt.Rows.Count > 0)
            {
                Dt.Columns.Add("URL", typeof(string));
                Dt.AcceptChanges();

                foreach (DataRow item in Dt.Rows)
                {
                    //if (item == null) continue;
                    //decimal amount = decimal.Parse(item["Amount"].ToString());
                    //try
                    //{
                    //    item["AmountWords"] = amounttowords.convertToWords(item["Amount"].ToString());

                    //    if (item["PayerID"].ToString().Length > 14)
                    //    {
                    //        item["PayerID"] = "Please approach the BIR for your unique Payer ID.";
                    //    }
                    //    else
                    //    {
                    //        item["PayerID"] = item["PayerID"].ToString();
                    //    }

                    //    item["URL"] = string.Format(@"Payment for {0} {1} << Paid at {2} - {3} , Deposit Slip Number {4} by {5}  >> ", item["Description"], item["RevenueCode"], item["BankName"], item["BranchName"], item["DepositSlipNumber"], item["PaymentMethod"]);

                    //    item["User"] = Program.UserID.ToUpper();

                    //    item["Username"] = string.Format(@"<< Reprint of  receipt with control number {0}  at {1} printing station >>", item["ControlNumber"], item["StationName"]);

                    //    strreprint = string.Format(" Reprint of {0} Receipt", item["ControlNumber"]);

                    //    item["PaymentDate"] = Convert.ToDateTime(item["PaymentDate"]).ToString("dd-MMM-yyyy");
                    //}
                    //catch
                    //{

                    //}
                    if (item == null) continue;
                    //decimal amount = decimal.Parse(item["Amount"].ToString());
                    try
                    {
                        item["AmountWords"] = amounttowords.convertToWords(item["Amount"].ToString(), item["prefix"].ToString(), item["Surfix"].ToString());
                        string stateCode = Program.stateCode;
                        if (!item["PayerID"].ToString().StartsWith(stateCode))
                        //if (item["PayerID"].ToString().Length > 14)
                        {
                            item["PayerID"] = "Approach the BIR for your Tax Identification Number.";
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



                        switch (Program.intCode)
                        {
                            case 13://Akwa Ibom state
                                item["URL"] = string.Format("Paid at {0} , {1}  Deposit Slip Number {2} in respect of {3} By {4}", item["BankName"], item["BranchName"], item["DepositSlipNumber"], item["Description"], item["PaymentMethod"]);
                                //item["AgencyCode"] = string.Format("{0}/{1}", item["AgencyCode"], item["RevenueCode"]);
                                item["AgencyCode"] = string.Format("{0}", item["RevenueCode"]);
                                break;

                            case 20://detla state
                                item["URL"] = string.Format("Payment for [{0}/{1}] paid at [{2}/{3}], Slip Number [{4}] by [{5}]", item["Description"], item["RevenueCode"], item["BankName"], item["BranchName"], item["DepositSlipNumber"], item["PaymentMethod"]);
                                break;


                            case 37://ogun state
                                item["URL"] = string.Format("Paid at {0} , {1}  Deposit Slip Number {2} in respect of {3} <{4}> By {5}", item["BankName"], item["BranchName"], item["DepositSlipNumber"], item["Description"], item["RevenueCode"], item["PaymentMethod"]);
                                break;

                            case 40://oyo state
                                item["URL"] = string.Format("Paid at {0} , {1}  Deposit Slip Number {2} in respect of {3} <{4}> By {5}", item["BankName"], item["BranchName"], item["DepositSlipNumber"], item["Description"], item["RevenueCode"], item["PaymentMethod"]);
                                break;

                            case 32://kogi state
                                item["URL"] = string.Format("Paid at {0} , {1}  Deposit Slip Number {2} in respect of {3} <{4}> By {5}", item["BankName"], item["BranchName"], item["DepositSlipNumber"], item["Description"], item["RevenueCode"], item["PaymentMethod"]);
                                break;

                            default:
                                break;
                        }

                        item["User"] = Program.UserID.ToUpper();

                        item["Username"] = string.Format(@"</Reprint of Receipt with control number {0} of {1}, Printed at {2} Zonal Office  by {3} on {4}/>", item["ControlNumber"], Convert.ToDateTime(item["ControlNumberDate"]).ToString("dd-MMM-yyyy"), item["StationName"], Program.UserID.ToUpper(), DateTime.Today.ToString("dd-MMM-yyyy"));

                        item["PaymentDate"] = Convert.ToDateTime(item["PaymentDate"]).ToString("dd-MMM-yyyy");
                    }
                    catch
                    {

                    }
                }
            }
        }

        void SetLoadData()
        {
            string quy = String.Format("SELECT ID , tblCollectionReport.PaymentRefNumber,DepositSlipNumber,PaymentDate,UPPER(PayerName) as PayerName,Description,RevenueCode,Amount,BankName+ '-'+ BranchName AS Bank,EReceipts,CASE WHEN EReceiptsDate IS NULL THEN PaymentDate ELSE EReceiptsDate END EReceiptsDate  FROM Receipt.tblReceipt JOIN Collection.tblCollectionReport ON tblCollectionReport.PaymentRefNumber = tblReceipt.PaymentRefNumber where Sentby='{0}' ORDER BY AgencyCode ,RevenueCode,EReceipts", Program.UserID);


            try
            {
                using (var ds = new System.Data.DataSet())
                {
                    ds.Clear();

                    using (SqlDataAdapter ada = new SqlDataAdapter(quy, Logic.ConnectionString))
                    {
                        ada.Fill(ds, "table");
                    }
                    //dtn.Clear();
                    dtn = ds.Tables[0];
                    if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                    {
                        gridControl4.DataSource = ds.Tables[0];
                        gridView4.BestFitColumns();
                        gridView4.Columns["Amount"].DisplayFormat.FormatType = FormatType.Numeric;
                        gridView4.Columns["Amount"].DisplayFormat.FormatString = "n2";
                        gridView4.Columns["PaymentDate"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
                        gridView4.Columns["PaymentDate"].DisplayFormat.FormatString = "dd/MM/yyyy";
                        gridView4.Columns["ID"].Visible = false;
                        gridView4.Columns["EReceiptsDate"].Visible = false;

                        if (isFirstGrid)
                        {
                            selection = new GridCheckMarksSelection(gridView4, ref lblSelect);
                            selection.CheckMarkColumn.VisibleIndex = 0;
                            isFirstGrid = false;
                        }
                    }
                    else
                    {
                        Common.setMessageBox("No Record Found", Program.ApplicationName, 3);
                        return;
                    }

                    //[GeneratedBy],[BankName],[BranchName]
                }
            }
            catch (Exception ex)
            {
                Common.setMessageBox(ex.Message, Program.ApplicationName, 3);
                return;
            }


        }

    }
}
