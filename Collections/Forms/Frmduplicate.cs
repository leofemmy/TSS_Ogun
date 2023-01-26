using Collection.Classess;
using Collection.Report;
using Collections;
using DevExpress.Utils;
using DevExpress.XtraGrid.Selection;
using DevExpress.XtraReports.UI;
using DevExpress.XtraSplashScreen;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using TaxSmartSuite.Class;

namespace Collection.Forms
{
    public partial class Frmduplicate : Form
    {
        private string criteria3, criteria2;

        private SqlCommand _command;

        public static Frmduplicate publicStreetGroup;

        bool isFirstGrid = true; private System.Data.DataSet ds;

        AmountToWords amounttowords = new AmountToWords();

        private string strCollectionReportID;

        SqlDataAdapter adp; bool IsShowDialog;

        DataTable dt;
        int iprofile, ireval;
        DateTime sdate1, sdate2;

        DataTable dtd = new DataTable();

        GridCheckMarksSelection selection; string values;

        DataTable temTable = new DataTable(); int ival;
        public Frmduplicate()
        {


            InitializeComponent();

            publicStreetGroup = this;


        }
        //System.NullReferenceException
        public Frmduplicate(DataTable Dts, int Receiptoption, bool IsShowDialog)
        {
            InitializeComponent();
            dtd = Dts;
            ival = Receiptoption;
            this.IsShowDialog = IsShowDialog;
            temTable.Columns.Add("EReceipt", typeof(string));
            temTable.Columns.Add("PaymentRefNumber", typeof(string));
            temTable.Columns.Add("UserId", typeof(string));
            if (!this.IsShowDialog)
                Init();
        }

        void Init()
        {
            SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

            setImages();

            ToolStripEvent();

            Load += OnFormLoad;

            bttnprints.Click += bttnprints_Click;

            bttnMain.Click += bttnMain_Click;

            OnFormLoad(null, null);


            SplashScreenManager.CloseForm(false);
        }

        private void setImages()
        {
            tsbNew.Image = MDIMain.publicMDIParent.i16x16.Images[5];
            tsbEdit.Image = MDIMain.publicMDIParent.i16x16.Images[6];
            tsbDelete.Image = MDIMain.publicMDIParent.i16x16.Images[7];
            tsbReload.Image = MDIMain.publicMDIParent.i16x16.Images[0];
            tsbClose.Image = MDIMain.publicMDIParent.i16x16.Images[11];

            ////bttnCancel.Image = MDIMain.publicMDIParent.i32x32.Images[9];
            bttnMain.Image = MDIMain.publicMDIParent.i32x32.Images[8];
            bttnprints.Image = MDIMain.publicMDIParent.i32x32.Images[29];
            //btnSearch.Image = MDIMain.publicMDIParent.i32x32.Images[2];
            //btnClear.Image = MDIMain.publicMDIParent.i32x32.Images[3];

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
                this.Close();
            }
            else if (sender == tsbNew)
            {

            }
            else if (sender == tsbEdit)
            {
            }
            else if (sender == tsbDelete)
            {

                tsbReload.PerformClick();
                //boolIsUpdate = false;
            }
            else if (sender == tsbReload)
            {
                //iTransType = TransactionTypeCode.Reload; setReload();
                //ShowForm();
            }

        }

        void OnFormLoad(object sender, EventArgs e)
        {



            setReload();


        }

        private void setReload()
        {
            gridControl1.DataSource = dtd;

            label10.Text = String.Format("Total Number of Transactions: {0}", dtd.Rows.Count);

            gridView1.Columns["Amount"].DisplayFormat.FormatType = FormatType.Numeric;
            gridView1.Columns["Amount"].DisplayFormat.FormatString = "n2";
            gridView1.Columns["PaymentDate"].DisplayFormat.FormatType = FormatType.DateTime;
            gridView1.Columns["PaymentDate"].DisplayFormat.FormatString = "dd/MM/yyyy";
            gridView1.Columns["EReceiptsDate"].DisplayFormat.FormatString = "dd/MM/yyyy";
            gridView1.Columns["ID"].Visible = false;
            gridView1.Columns["AgencyCode"].Visible = false;
            gridView1.Columns["Description"].Visible = false;
            gridView1.Columns["DepositSlipNumber"].Visible = false;
            gridView1.Columns["GeneratedBy"].Visible = false;
            gridView1.Columns["BankName"].Visible = false;
            gridView1.Columns["EReceiptsDate"].Visible = false;
            gridView1.Columns["BranchName"].Visible = false;
            gridView1.OptionsBehavior.Editable = false;
            gridView1.BestFitColumns();

            if (isFirstGrid)
            {
                selection = new GridCheckMarksSelection(gridView1, ref lblSelect);
                selection.CheckMarkColumn.VisibleIndex = 0;
                isFirstGrid = false;
            }
        }

        void ProcessDataTable(DataTable Dt)
        {
            if (Dt != null && Dt.Rows.Count > 0)
            {
                Dt.Columns.Add("URL", typeof(string));
                Dt.AcceptChanges();

                foreach (DataRow item in Dt.Rows)
                {
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
                        item["ReceiptNo"] = item["ControlNumber"].ToString();
                        item["Description"] = item["Description"].ToString();
                        //item["URL"] = string.Format(@"Payment for {0} {1} << Paid at {2} - {3} , Deposit Slip Number {4} by {5}  >> ", item["Description"], item["RevenueCode"], item["BankName"], item["BranchName"], item["DepositSlipNumber"], item["PaymentMethod"]);

                        //item["URL"] = string.Format("Paid at {0} , {1}  Deposit Slip Number {2} in respect of {3} <{4}> By {5}", item["BankName"], item["BranchName"], item["DepositSlipNumber"], item["Description"], item["RevenueCode"], item["PaymentMethod"]);

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

                        //item["Username"] =
                        //    string.Format(
                        //        @"</Original control number {0} printed at {1} Zonal Office  by {2} on {3}/>",
                        //        item["ControlNumber"], item["StationName"], item["PrintedBY"], Program.UserID,
                        //        string.Format("{0:dd/MM/yyyy 23:59:59}", item["ControlNumberDate"]));
                        //     ;
                        //, item["IsPrintedDate"],{3:dd/MM/yyyy 23:59:59}
                        item["Username"] = string.Format(@"</Original control number {0} printed at {1} Office by {2} on {3} />", item["ControlNumber"], item["StationName"], item["PrintedBY"], Convert.ToDateTime(item["ControlNumberDate"]).ToString("dd-MMM-yyyy hh:mm:ss"));

                        item["PaymentDate"] = Convert.ToDateTime(item["PaymentDate"]).ToString("dd-MMM-yyyy");
                    }
                    catch
                    {

                    }
                }
            }
        }

        void GetPayRef()
        {
            //string values = string.Empty;

            lblSelect.Text = string.Empty;

            int j = 0;


            for (int i = 0; i < selection.SelectedCount; i++)
            {
                values += String.Format("'{0}'", (selection.GetSelectedRow(i) as DataRowView)["PaymentRefNumber"]);
                if (j + 1 < selection.SelectedCount)
                    values += ",";
                ++j;

                temTable.Rows.Add(new object[] { String.Format("{0}", (selection.GetSelectedRow(i) as DataRowView)["PaymentRefNumber"]), String.Format("{0}", (selection.GetSelectedRow(i) as DataRowView)["EReceipts"]), Program.UserID });

            }

            //new line here

            using (System.Data.DataSet dds = new System.Data.DataSet("DsCollectionReport"))
            {
                SqlDataAdapter ada;

                using (WaitDialogForm form = new WaitDialogForm())
                {
                    string strFormat = null; string query = string.Empty;


                    switch (Program.intCode)
                    {
                        case 20://detla state
                            query = string.Format("SELECT [ID] ,PaymentPeriod, [Provider] , [Channel] ,Collection.tblCollectionReport.[PaymentRefNumber] , [DepositSlipNumber] , CONVERT(VARCHAR,CONVERT(DATE,[PaymentDate])) AS PaymentDate,[PayerID] , UPPER([PayerName]) AS [PayerName], [Amount] ,[PaymentMethod] ,[ChequeNumber] ,[ChequeValueDate] ,[ChequeStatus] ,[DateChequeReturned] ,[TelephoneNumber] ,[ReceiptNo] as [EReceipts],Collection.tblCollectionReport.ReceiptDate ,UPPER([PayerAddress]) as [PayerAddress] ,[User] ,[RevenueCode] ,[Description] , [ChequeBankCode] ,[ChequeBankName] ,[AgencyName] ,[AgencyCode] ,[BankCode] ,[BankName] ,[BranchCode] ,[BranchName] ,[ZoneCode] ,[ZoneName] ,[Username] ,[AmountWords] ,Collection.tblCollectionReport.[EReceiptsDate] ,[GeneratedBy] ,Collection.tblCollectionReport.[StationCode] ,(SELECT TOP 1 StationName FROM Receipt.tblStation  WHERE tblStation.StationCode = Collection.tblCollectionReport.[StationCode]) AS StationName,ControlNumber,ControlNumberBy,ControlNumberDate,IsPrintedDate,PrintedBY from Collection.tblCollectionReport INNER JOIN Receipt.tblCollectionReceipt ON Receipt.tblCollectionReceipt.PaymentRefNumber = Collection.tblCollectionReport.PaymentRefNumber INNER JOIN Receipt.tblReceiptoption ON Receipt.tblReceiptoption.PaymentRefNumber = Receipt.tblCollectionReceipt.PaymentRefNumber WHERE Collection.tblCollectionReport.PaymentRefNumber IN ({0}) AND  Receipt.tblReceiptoption.IsPrinted=1 AND Receipt.tblReceiptoption.OptionVal={1}  ORDER BY Collection.tblCollectionReport.AgencyName ,Collection.tblCollectionReport.Description,Collection.tblCollectionReport.EReceipts", values, (ival - 1));
                            break;
                        default:
                            query = string.Format("SELECT [ID] ,PaymentPeriod, [Provider] , [Channel] ,Collection.tblCollectionReport.[PaymentRefNumber] , [DepositSlipNumber] , CONVERT(VARCHAR,CONVERT(DATE,[PaymentDate])) AS PaymentDate,[PayerID] , UPPER([PayerName]) AS [PayerName], [Amount] ,[PaymentMethod] ,[ChequeNumber] ,[ChequeValueDate] ,[ChequeStatus] ,[DateChequeReturned] ,[TelephoneNumber] ,[ReceiptNo] ,Collection.tblCollectionReport.ReceiptDate ,UPPER([PayerAddress]) as [PayerAddress] ,[User] ,[RevenueCode] ,tblCollectionReport.Description , [ChequeBankCode] ,[ChequeBankName] ,[AgencyName] ,[AgencyCode] ,[BankCode] ,[BankName] ,[BranchCode] ,[BranchName] ,[ZoneCode] ,[ZoneName] ,[Username] ,[AmountWords] ,Collection.tblCollectionReport.[EReceipts] ,Collection.tblCollectionReport.[EReceiptsDate] ,[GeneratedBy] ,Collection.tblCollectionReport.[StationCode] ,(SELECT TOP 1 StationName FROM Receipt.tblStation  WHERE tblStation.StationCode = Collection.tblCollectionReport.[StationCode]) AS StationName,ControlNumber,ControlNumberBy,ControlNumberDate,IsPrintedDate,PrintedBY, Symbol , Surfix , tblCurrency.Description AS prefix from Collection.tblCollectionReport JOIN Reconciliation.tblCurrency ON tblCurrency.CurrencyCode = tblCollectionReport.CurrencyCode                                INNER JOIN Receipt.tblCollectionReceipt ON Receipt.tblCollectionReceipt.PaymentRefNumber = Collection.tblCollectionReport.PaymentRefNumber INNER JOIN Receipt.tblReceiptoption ON Receipt.tblReceiptoption.PaymentRefNumber = Receipt.tblCollectionReceipt.PaymentRefNumber WHERE Collection.tblCollectionReport.PaymentRefNumber IN ({0}) AND  Receipt.tblReceiptoption.IsPrinted=1 AND Receipt.tblReceiptoption.OptionVal={1}  ORDER BY Collection.tblCollectionReport.AgencyName ,Collection.tblCollectionReport.Description,Collection.tblCollectionReport.EReceipts", values, (ival - 1));
                            break;
                    }


                    DataTable Dt = dds.Tables.Add("CollectionReportTable");
                    ada = new SqlDataAdapter(query, Logic.ConnectionString);
                    ada.Fill(dds, "CollectionReportTable");
                    Logic.ProcessDataTable(Dt); ;
                    //strCollectionReportID = strFormat;
                }

                switch (Program.intCode)
                {
                    case 13://Akwa Ibom state
                        XtraRepReceiptAkwa akwa = new XtraRepReceiptAkwa { DataSource = dds.Tables[0] /*recportRec.DataAdapter = ada;*/, DataMember = "CollectionReportTable" };
                        akwa.ShowPreviewDialog();
                        break;

                    case 20://detla state
                        XtraRepReceiptDelta delta = new XtraRepReceiptDelta { DataSource = dds.Tables[0] /*recportRec.DataAdapter = ada;*/, DataMember = "CollectionReportTable" };
                        delta.xrLabel19.Visible = true;
                        //delta.xrLabel13.Visible = true;
                        delta.logoPath = Logic.singaturepth;
                        delta.ShowPreviewDialog();
                        break;

                    case 37://ogun state
                        XRepReceipt recportRec = new XRepReceipt { DataSource = dds.Tables[0] /*recportRec.DataAdapter = ada;*/, DataMember = "CollectionReportTable" };
                        recportRec.logoPath = Logic.singaturepth;
                        recportRec.ShowPreviewDialog();
                        break;

                    case 40://oyo state
                        XRepReceiptoyo recportRecs = new XRepReceiptoyo { DataSource = dds.Tables[0] /*recportRec.DataAdapter = ada;*/, DataMember = "CollectionReportTable" };
                        recportRecs.ShowPreviewDialog();
                        break;

                    default:

                        break;
                }
            }


            DialogResult result = MessageBox.Show(" Is Receipt Printing Successful ?", Program.ApplicationName, MessageBoxButtons.YesNo);

            if (result == DialogResult.Yes)
            {
                try
                {
                    SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

                    bttnMain.Visible = true; label1.Visible = true;
                    bttnMain.Enabled = true;

                    using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
                    {
                        connect.Open();
                        _command = new SqlCommand("doReceiptOptionother", connect)
                        {
                            CommandType = CommandType.StoredProcedure
                        };
                        _command.Parameters.Add(new SqlParameter("@userid", SqlDbType.VarChar)).Value =
                            Program.UserID;
                        _command.Parameters.Add(new SqlParameter("@Receop", SqlDbType.VarChar)).Value =
                           ival;
                        _command.Parameters.Add(new SqlParameter("@tempTable",
                            SqlDbType.Structured)).Value = temTable;

                        System.Data.DataSet response = new System.Data.DataSet();

                        adp = new SqlDataAdapter(_command);
                        adp.Fill(response);

                        connect.Close();
                        if (response.Tables[0].Rows[0]["returnCode"].ToString() != "00")
                        {
                            Tripous.Sys.ErrorBox(response.Tables[0].Rows[0]["returnMessage"].ToString()); return;
                        }
                        else
                        {
                            Common.setMessageBox("Receipt Printed Successfully", Program.ApplicationName, 1);
                            //selection.ClearSelection();
                            this.Close();
                            return;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Tripous.Sys.ErrorBox(ex); return;
                }
                finally
                {
                    SplashScreenManager.CloseForm(false);
                }
                //using (FrmMainFest frmMainFest = new FrmMainFest(strCollectionReportID, values, criteria2, false) { IDList = strCollectionReportID })
                //{
                //    frmMainFest.ShowDialog();
                //}
            }
            else
                return;



            #region old

            //using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
            //{
            //    connect.Open();
            //    _command = new SqlCommand("InserttblReceipt", connect) { CommandType = CommandType.StoredProcedure };
            //    _command.Parameters.Add(new SqlParameter("@tblCollectionReport_Temp", SqlDbType.Structured)).Value = temTable;
            //    _command.Parameters.Add(new SqlParameter("@type", SqlDbType.VarChar)).Value = "New";
            //    _command.Parameters.Add(new SqlParameter("@userid", SqlDbType.VarChar)).Value = Program.UserID;
            //    System.Data.DataSet response = new System.Data.DataSet();
            //    response.Clear();
            //    adp = new SqlDataAdapter(_command);
            //    adp.Fill(response);

            //    connect.Close();
            //    if (String.Compare(response.Tables[0].Rows[0]["returnCode"].ToString(), "00", false) == 0)
            //    {
            //        //do something load the report page
            //        using (SqlConnection db = new SqlConnection(Logic.ConnectionString))
            //        {
            //            SqlTransaction transaction;

            //            db.Open();

            //            transaction = db.BeginTransaction();

            //            try
            //            {
            //                using (System.Data.DataSet dds = new System.Data.DataSet("DsCollectionReport"))
            //                {
            //                    SqlDataAdapter ada;

            //                    using (WaitDialogForm form = new WaitDialogForm())
            //                    {
            //                        string strFormat = null;

            //                        string query = string.Format("SELECT [ID] , [Provider] , [Channel] ,Collection.tblCollectionReport.[PaymentRefNumber] , [DepositSlipNumber] , CONVERT(VARCHAR,CONVERT(DATE,[PaymentDate])) AS PaymentDate,[PayerID] , UPPER([PayerName]) AS [PayerName], [Amount] ,[PaymentMethod] ,[ChequeNumber] ,[ChequeValueDate] ,[ChequeStatus] ,[DateChequeReturned] ,[TelephoneNumber] ,[ReceiptNo] ,[ReceiptDate] ,UPPER([PayerAddress]) as [PayerAddress] ,[User] ,[RevenueCode] ,[Description] , [ChequeBankCode] ,[ChequeBankName] ,[AgencyName] ,[AgencyCode] ,[BankCode] ,[BankName] ,[BranchCode] ,[BranchName] ,[ZoneCode] ,[ZoneName] ,[Username] ,[AmountWords] ,Collection.tblCollectionReport.[EReceipts] ,Collection.tblCollectionReport.[EReceiptsDate] ,[GeneratedBy] ,Collection.tblCollectionReport.[StationCode] ,(SELECT TOP 1 StationName FROM Receipt.tblStation  WHERE tblStation.StationCode = Collection.tblCollectionReport.[StationCode]) AS StationName,ControlNumber from Collection.tblCollectionReport INNER JOIN Receipt.tblCollectionReceipt ON Receipt.tblCollectionReceipt.PaymentRefNumber = Collection.tblCollectionReport.PaymentRefNumber WHERE PaymentRefNumber IN ({0})  ORDER BY Collection.tblCollectionReport.AgencyName ,Collection.tblCollectionReport.Description,Collection.tblCollectionReport.EReceipts", values);

            //                        DataTable Dt = dds.Tables.Add("CollectionReportTable");
            //                        ada = new SqlDataAdapter(query, Logic.ConnectionString);
            //                        ada.Fill(dds, "CollectionReportTable");
            //                        Logic.ProcessDataTable(Dt);;
            //                        //strCollectionReportID = strFormat;
            //                    }


            //                    //XRepReceipt recportRec = new XRepReceipt { DataSource = dds.Tables[0] /*recportRec.DataAdapter = ada;*/, DataMember = "CollectionReportTable" };

            //                    //recportRec.ShowPreviewDialog();

            //                    ////selection.ClearSelection(); 
            //                    //dds.Clear();

            //                    switch (Program.intCode)
            //                    {
            //                        case 13://Akwa Ibom state
            //                            XtraRepReceiptAkwa akwa = new XtraRepReceiptAkwa { DataSource = dds.Tables[0] /*recportRec.DataAdapter = ada;*/, DataMember = "CollectionReportTable" };
            //                            akwa.ShowPreviewDialog();
            //                            break;

            //                        case 20://detla state
            //                            XtraRepReceiptDelta delta = new XtraRepReceiptDelta { DataSource = dds.Tables[0] /*recportRec.DataAdapter = ada;*/, DataMember = "CollectionReportTable" };
            //                            delta.ShowPreviewDialog();
            //                            break;

            //                        case 37://ogun state
            //                            XRepReceipt recportRec = new XRepReceipt { DataSource = dds.Tables[0] /*recportRec.DataAdapter = ada;*/, DataMember = "CollectionReportTable" };

            //                            recportRec.ShowPreviewDialog();
            //                            break;

            //                        case 40://oyo state
            //                            XRepReceiptoyo recportRecs = new XRepReceiptoyo { DataSource = dds.Tables[0] /*recportRec.DataAdapter = ada;*/, DataMember = "CollectionReportTable" };
            //                            recportRecs.ShowPreviewDialog();
            //                            break;

            //                        //case 32://kogi state

            //                        //    XRepReceiptkogi recportRecko = new XRepReceiptkogi { DataSource = dds.Tables[0] /*recportRec.DataAdapter = ada;*/, DataMember = "CollectionReportTable" };
            //                        //    recportRecko.ShowPreviewDialog();

            //                        //    break;

            //                        default:

            //                            break;
            //                    }
            //                }

            //                //dds.Clear();
            //                DialogResult result = MessageBox.Show(" Is Receipt Printing Successful ?", Program.ApplicationName, MessageBoxButtons.YesNo);

            //                if (result == DialogResult.Yes)
            //                {
            //                    try
            //                    {
            //                        SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

            //                        //update the collection table by seting the isprinted to true
            //                        using (SqlConnection dbs = new SqlConnection(Logic.ConnectionString))
            //                        {
            //                            SqlTransaction transactions;

            //                            dbs.Open();

            //                            //transaction = db.BeginTransaction();

            //                            transactions = dbs.BeginTransaction();

            //                            try
            //                            {
            //                                string query1 = String.Format("UPDATE tblCollectionReport SET isPrinted=1,IsPrintedDate= '{0:MM/dd/yyyy HH:mm:ss tt}',PrintedBY='{1}' ,StationCode='{2}' WHERE PaymentRefNumber IN ({3})", DateTime.Now, Program.UserID, Program.stationCode, values);

            //                                //string query1 = String.Format("UPDATE Receipt.tblCollectionReceipt SET isPrinted=1,IsPrintedDate= '{0:MM/dd/yyyy HH:mm:ss tt}',PrintedBY='{1}',StationCode='{2}' WHERE PaymentRefNumber IN (SELECT PaymentRefNumber FROM Receipt.tblReceipt WHERE SentBy='{1}')", DateTime.Now, Program.UserID, Program.stationCode);

            //                                using (SqlCommand sqlCommand = new SqlCommand(query1, db, transaction))
            //                                {
            //                                    sqlCommand.ExecuteNonQuery();
            //                                }

            //                            }
            //                            catch (Exception ex)
            //                            {
            //                                transaction.Rollback();
            //                                Tripous.Sys.ErrorBox(ex);
            //                                return;
            //                            }
            //                            transaction.Commit();

            //                            db.Close();
            //                        }
            //                        bttnMain.Visible = true; label1.Visible = true;
            //                        bttnMain.Enabled = true;
            //                    }
            //                    finally
            //                    {
            //                        SplashScreenManager.CloseForm(false);
            //                    }
            //                    //using (FrmMainFest frmMainFest = new FrmMainFest(strCollectionReportID, values, criteria2, false) { IDList = strCollectionReportID })
            //                    //{
            //                    //    frmMainFest.ShowDialog();
            //                    //}
            //                }
            //                else
            //                    return;

            //            }
            //            catch (SqlException sqlError)
            //            {
            //                //transaction.Rollback();

            //                Tripous.Sys.ErrorBox(sqlError);
            //            }
            //            catch (Exception ex)
            //            {
            //                Tripous.Sys.ErrorBox(ex);
            //            }
            //            db.Close();
            //        }

            //    }
            //    else
            //    {
            //        if (String.Compare(response.Tables[0].Rows[0]["returnCode"].ToString(), "-1", false) == 0)
            //        {
            //            using (Frmcontrol frmcontrol = new Frmcontrol())
            //            {
            //                frmcontrol.gridControl1.DataSource = response.Tables[1];
            //                frmcontrol.gridView1.BestFitColumns();
            //                frmcontrol.label1.Text = "Payment Ref. Number Receipt printing in Process";
            //                frmcontrol.Text = "Payment Ref. Number Receipt printing in Process";
            //                frmcontrol.ShowDialog();
            //            }

            //            DialogResult result = MessageBox.Show("Do you wish to continue to Print the Receipts?", Program.ApplicationName, MessageBoxButtons.YesNo);

            //            if (result == DialogResult.Yes)
            //            {

            //                using (SqlConnection db = new SqlConnection(Logic.ConnectionString))
            //                {
            //                    SqlTransaction transaction;

            //                    db.Open();

            //                    transaction = db.BeginTransaction();

            //                    try
            //                    {
            //                        using (System.Data.DataSet dds = new System.Data.DataSet("DsCollectionReport"))
            //                        {
            //                            SqlDataAdapter ada;

            //                            using (WaitDialogForm form = new WaitDialogForm())
            //                            {
            //                                string strFormat = null;

            //                                string query = string.Format("SELECT [ID] , [Provider] , [Channel] ,[PaymentRefNumber] , [DepositSlipNumber] , CONVERT(VARCHAR,CONVERT(DATE,[PaymentDate])) AS PaymentDate,[PayerID] , UPPER([PayerName]) AS [PayerName], [Amount] ,[PaymentMethod] ,[ChequeNumber] ,[ChequeValueDate] ,[ChequeStatus] ,[DateChequeReturned] ,[TelephoneNumber] ,[ReceiptNo] ,[ReceiptDate] ,UPPER([PayerAddress]) as [PayerAddress] ,[User] ,[RevenueCode] ,[Description] , [ChequeBankCode] ,[ChequeBankName] ,[AgencyName] ,[AgencyCode] ,[BankCode] ,[BankName] ,[BranchCode] ,[BranchName] ,[ZoneCode] ,[ZoneName] ,[Username] ,[AmountWords] ,[EReceipts] ,[EReceiptsDate] ,[GeneratedBy] ,[StationCode] ,(SELECT TOP 1 StationName FROM Receipt.tblStation  WHERE tblStation.StationCode = Collection.tblCollectionReport.[StationCode]) AS StationName from Collection.tblCollectionReport WHERE PaymentRefNumber IN ({0}) ORDER BY Collection.tblCollectionReport.AgencyName ,Collection.tblCollectionReport.Description,Collection.tblCollectionReport.EReceipts", values);

            //                                DataTable Dt = dds.Tables.Add("CollectionReportTable");
            //                                ada = new SqlDataAdapter(query, Logic.ConnectionString);
            //                                ada.Fill(dds, "CollectionReportTable");
            //                                Logic.ProcessDataTable(Dt);;
            //                                //strCollectionReportID = strFormat;
            //                            }


            //                            //XRepReceipt recportRec = new XRepReceipt { DataSource = dds.Tables[0] /*recportRec.DataAdapter = ada;*/, DataMember = "CollectionReportTable" };

            //                            //recportRec.ShowPreviewDialog();

            //                            switch (Program.intCode)
            //                            {
            //                                case 13://Akwa Ibom state
            //                                    XtraRepReceiptAkwa akwa = new XtraRepReceiptAkwa { DataSource = dds.Tables[0] /*recportRec.DataAdapter = ada;*/, DataMember = "CollectionReportTable" };
            //                                    akwa.ShowPreviewDialog();
            //                                    break;

            //                                case 20://detla state
            //                                    XtraRepReceiptDelta delta = new XtraRepReceiptDelta { DataSource = dds.Tables[0] /*recportRec.DataAdapter = ada;*/, DataMember = "CollectionReportTable" };
            //                                    delta.ShowPreviewDialog();
            //                                    break;

            //                                case 37://ogun state
            //                                    XRepReceipt recportRec = new XRepReceipt { DataSource = dds.Tables[0] /*recportRec.DataAdapter = ada;*/, DataMember = "CollectionReportTable" };

            //                                    recportRec.ShowPreviewDialog();
            //                                    break;

            //                                case 40://oyo state
            //                                    XRepReceiptoyo recportRecs = new XRepReceiptoyo { DataSource = dds.Tables[0] /*recportRec.DataAdapter = ada;*/, DataMember = "CollectionReportTable" };
            //                                    recportRecs.ShowPreviewDialog();
            //                                    break;

            //                                //case 32://kogi state

            //                                //    XRepReceiptkogi recportRecko = new XRepReceiptkogi { DataSource = dds.Tables[0] /*recportRec.DataAdapter = ada;*/, DataMember = "CollectionReportTable" };
            //                                //    recportRecko.ShowPreviewDialog();

            //                                //    break;

            //                                default:
            //                                    break;
            //                            }
            //                            //selection.ClearSelection(); 
            //                            dds.Clear();
            //                        }

            //                        DialogResult results = MessageBox.Show(" Is Receipt Printing Successful ?", Program.ApplicationName, MessageBoxButtons.YesNo);

            //                        if (results == DialogResult.Yes)
            //                        {
            //                            try
            //                            {
            //                                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

            //                                //update the collection table by seting the isprinted to true
            //                                using (SqlConnection dbs = new SqlConnection(Logic.ConnectionString))
            //                                {
            //                                    SqlTransaction transactions;

            //                                    dbs.Open();

            //                                    //transaction = db.BeginTransaction();

            //                                    transactions = dbs.BeginTransaction();

            //                                    try
            //                                    {
            //                                        string query1 = String.Format("UPDATE Receipt.tblCollectionReceipt SET isPrinted=1,IsPrintedDate= '{0:MM/dd/yyyy HH:mm:ss tt}',PrintedBY='{1}' ,StationCode='{2}' WHERE PaymentRefNumber IN ({3})", DateTime.Now, Program.UserID, Program.stationCode, values);

            //                                        //string query1 = String.Format("UPDATE Receipt.tblCollectionReceipt SET isPrinted=1,IsPrintedDate= '{0:MM/dd/yyyy HH:mm:ss tt}',PrintedBY='{1}',StationCode='{2}' WHERE PaymentRefNumber IN (SELECT PaymentRefNumber FROM Receipt.tblReceipt WHERE SentBy='{1}')", DateTime.Now, Program.UserID, Program.stationCode);

            //                                        using (SqlCommand sqlCommand = new SqlCommand(query1, db, transaction))
            //                                        {
            //                                            sqlCommand.ExecuteNonQuery();
            //                                        }

            //                                    }
            //                                    catch (Exception ex)
            //                                    {
            //                                        transaction.Rollback();
            //                                        Tripous.Sys.ErrorBox(ex);
            //                                        return;
            //                                    }
            //                                    transaction.Commit();

            //                                    db.Close();
            //                                }
            //                                bttnMain.Visible = true; label1.Visible = true;
            //                                bttnMain.Enabled = true;
            //                            }
            //                            finally
            //                            {
            //                                SplashScreenManager.CloseForm(false);
            //                            }

            //                            //using (FrmMainFest frmMainFest = new FrmMainFest(strCollectionReportID, values, criteria2, false) { IDList = strCollectionReportID })
            //                            //{
            //                            //    this.Close();
            //                            //    frmMainFest.ShowDialog();
            //                            //}


            //                        }
            //                        else
            //                            return;

            //                    }
            //                    catch (SqlException sqlError)
            //                    {
            //                        //transaction.Rollback();

            //                        Tripous.Sys.ErrorBox(sqlError);
            //                    }
            //                    catch (Exception ex)
            //                    {
            //                        Tripous.Sys.ErrorBox(ex);
            //                    }
            //                    db.Close();
            //                }

            //            }
            //            else
            //                return;

            //        }
            //        else
            //        {
            //            Common.setMessageBox(response.Tables[0].Rows[0]["returnMessage"].ToString(), "", 2);
            //            return;
            //        }




            //    }
            //}
            #endregion

        }

        void bttnprints_Click(object sender, EventArgs e)
        {
            if (selection.SelectedCount == 0)
            {

                Common.setMessageBox("No Selection Made for Reprinting of Receipts", Program.ApplicationName, 3);
                return;

            }
            else
            {
                GetPayRef();
            }
        }

        void bttnMain_Click(object sender, EventArgs e)
        {

            if (selection.SelectedCount == 0 || string.IsNullOrEmpty(values))
            {
                Common.setMessageBox("No Selection Made for Printing of Receipts", Program.ApplicationName, 3);
                return;

            }
            else
            {

                if (Program.stateCode == "20")
                {
                    if (radioGroup1.EditValue == null)
                    {
                        Common.setMessageBox("Receipt Printing Options", Program.ApplicationName, 1);
                        return;
                    }
                    else
                    {
                        using (
                            FrmMainFest frmMainFest = new FrmMainFest(strCollectionReportID, values, criteria2, false, Convert.ToInt32(radioGroup1.EditValue))
                            {
                                IDList = strCollectionReportID
                            })
                        {
                            frmMainFest.ShowDialog();
                        }
                    }
                }
                else
                {
                    using (
                            FrmMainFest frmMainFest = new FrmMainFest(strCollectionReportID, values, criteria2, false)
                            {
                                IDList = strCollectionReportID
                            })
                    {
                        frmMainFest.ShowDialog();
                    }
                }




            }

            //using (FrmMainFest frmMainFest = new FrmMainFest(strCollectionReportID, values, criteria2, false) { IDList = strCollectionReportID })
            //{

            //    frmMainFest.ShowDialog();
            //}
        }

        void bttnprint_Click(object sender, EventArgs e)
        {


        }

    }
}
