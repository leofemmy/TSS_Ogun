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
    public partial class FrmReprintHoc : Form
    {
        private SqlCommand _command; SqlDataAdapter adp;

        private string criteria3, criteria2;

        DataTable dtse = new DataTable();

        string values = string.Empty;

        bool isFirstGrid = true;

        GridCheckMarksSelection selection;

        bool IsShowDialog;

        private string strCollectionReportID; public static FrmReprintHoc publicStreetGroup;

        DataTable temTable = new DataTable();

        AmountToWords amounttowords = new AmountToWords(); DataTable dtata = new DataTable();
        public FrmReprintHoc()
        {
            //SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

            InitializeComponent();



            Init();


            //SplashScreenManager.CloseForm(false);
        }

        public FrmReprintHoc(DataTable dts, bool IsShowDialog)
        {
            InitializeComponent();
            dtse = dts;
            this.IsShowDialog = IsShowDialog;
            if (!this.IsShowDialog)
                Init();
        }

        void Init()
        {
            SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

            publicStreetGroup = this;

            setImages();

            ToolStripEvent();

            SetReload();

            bttnprints.Click += Bttnprints_Click;
            temTable.Columns.Add("EReceipt", typeof(string));
            temTable.Columns.Add("PaymentRefNumber", typeof(string));
            temTable.Columns.Add("UserId", typeof(string));

            bttnMain.Click += BttnMain_Click;
            SplashScreenManager.CloseForm(false);
        }

        private void BttnMain_Click(object sender, EventArgs e)
        {

            if (selection.SelectedCount == 0 || string.IsNullOrEmpty(values))
            {
                Common.setMessageBox("No Selection Made for Printing of Receipts", Program.ApplicationName, 3);
                return;

            }
            else
            {

                //if (Program.stateCode == "20")
                //{
                //    if (radioGroup1.EditValue == null)
                //    {
                //        Common.setMessageBox("Receipt Printing Options", Program.ApplicationName, 1);
                //        return;
                //    }
                //    else
                //    {
                //        using (
                //            FrmMainFest frmMainFest = new FrmMainFest(strCollectionReportID, values, criteria2, false, Convert.ToInt32(radioGroup1.EditValue))
                //            {
                //                IDList = strCollectionReportID
                //            })
                //        {
                //            frmMainFest.ShowDialog();
                //        }
                //    }
                //}
                //else
                //{
                using (
                        FrmMainFest frmMainFest = new FrmMainFest(strCollectionReportID, values, criteria2, false)
                        {
                            IDList = strCollectionReportID
                        })
                {
                    frmMainFest.ShowDialog();
                }
                //}




            }
        }

        private void Bttnprints_Click(object sender, EventArgs e)
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

        void SetReload()
        {
            DataTable dt;

            string values = string.Empty;

            if (dtse != null && dtse.Rows.Count > 0)
            {
                int i = 0;
                foreach (DataRow dr in dtse.Rows)
                {
                    values += string.Format("'{0}'", dr["paymentrefnumber"]);

                    if (i + 1 < dtse.Rows.Count)
                        values += ",";
                    ++i;
                }


                using (var ds = new System.Data.DataSet())
                {
                    //connect.connect.Open();
                    string query = string.Format("SELECT ID ,PaymentRefNumber , DepositSlipNumber ,CONVERT(VARCHAR, CONVERT(DATE, PaymentDate), 103) AS PaymentDate ,[PayerID] ,UPPER(PayerName) AS PayerName ,AgencyName ,AgencyCode , Description , RevenueCode , Amount ,BankName + '-' + BranchName AS Bank , EReceipts ,[GeneratedBy] , [BankName] , [BranchName] ,CONVERT(VARCHAR(10), EReceiptsDate, 103) AS EReceiptsDate  FROM Collection.tblCollectionReport WHERE PaymentRefNumber IN ({0})  ORDER BY Collection.tblCollectionReport.AgencyName ,Collection.tblCollectionReport.Description,Collection.tblCollectionReport.EReceipts", values);

                    using (SqlDataAdapter ada = new SqlDataAdapter(query, Logic.ConnectionString))
                    {
                        ada.Fill(ds, "table");
                    }
                    //connect.connect.Close();
                    dt = ds.Tables[0];
                    gridControl1.DataSource = dt;
                }


                label10.Text = String.Format("Total Number of Transactions: {0}", dt.Rows.Count);

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
        }

        void GetPayRef()
        {
            //string values = string.Empty;
            int intval = 0;

            lblSelect.Text = string.Empty;

            int j = 0;



            for (int i = 0; i < selection.SelectedCount; i++)
            {
                values += String.Format("'{0}'", (selection.GetSelectedRow(i) as DataRowView)["PaymentRefNumber"]);
                if (j + 1 < selection.SelectedCount)
                    values += ",";
                ++j;


                temTable.Rows.Add(new object[] { String.Format("{0}", (selection.GetSelectedRow(i) as DataRowView)["EReceipts"]), String.Format("{0}", (selection.GetSelectedRow(i) as DataRowView)["PaymentRefNumber"]), Program.UserID });

            }

            using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
            {
                connect.Open();
                _command = new SqlCommand("InserttblReceipt", connect) { CommandType = CommandType.StoredProcedure };
                _command.Parameters.Add(new SqlParameter("@tblCollectionReport_Temp", SqlDbType.Structured)).Value = temTable;
                _command.Parameters.Add(new SqlParameter("@type", SqlDbType.VarChar)).Value = "New";
                //_command.Parameters.Add(new SqlParameter("@intoption", SqlDbType.Int)).Value = intval;
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

                                using (WaitDialogForm form = new WaitDialogForm())
                                {
                                    string strFormat = null; string query = string.Empty;

                                    //if (Program.stateCode == "20")
                                    //{
                                    //    if (Convert.ToInt32(radioGroup1.EditValue) != 1)
                                    //    {
                                    //        query = string.Format("SELECT [ID] , [Provider] , [Channel] ,Collection.tblCollectionReport.PaymentRefNumber , [DepositSlipNumber] , CONVERT(VARCHAR,CONVERT(DATE,[PaymentDate])) AS PaymentDate,[PayerID] , UPPER([PayerName]) AS [PayerName], [Amount] ,[PaymentMethod] ,[ChequeNumber] ,[ChequeValueDate] ,[ChequeStatus] ,[DateChequeReturned] ,[TelephoneNumber] ,[ReceiptNo] ,[ReceiptDate] ,UPPER([PayerAddress]) as [PayerAddress] ,[User] ,[RevenueCode] ,[Description] , [ChequeBankCode] ,[ChequeBankName] ,[AgencyName] ,[AgencyCode] ,[BankCode] ,[BankName] ,[BranchCode] ,[BranchName] ,[ZoneCode] ,[ZoneName] ,[Username] ,[AmountWords] ,Collection.tblCollectionReport.[EReceipts] ,Collection.tblCollectionReport.[EReceiptsDate],[GeneratedBy] ,  Collection.tblCollectionReport.[StationCode] ,(SELECT TOP 1 StationName FROM Receipt.tblStation  WHERE tblStation.StationCode = Collection.tblCollectionReport.[StationCode]) AS StationName,ControlNumber , ControlNumberBy , ControlNumberDate ,IsPrintedDate , PrintedBY from Collection.tblCollectionReport  INNER JOIN Receipt.tblCollectionReceipt ON Receipt.tblCollectionReceipt.PaymentRefNumber = Collection.tblCollectionReport.PaymentRefNumber INNER JOIN Receipt.tblReceiptoption ON Receipt.tblReceiptoption.PaymentRefNumber = Receipt.tblCollectionReceipt.PaymentRefNumber WHERE Collection.tblCollectionReport.PaymentRefNumber  IN ({0})         AND Receipt.tblReceiptoption.IsPrinted = 1  ORDER BY Collection.tblCollectionReport.AgencyName ,Collection.tblCollectionReport.Description,Collection.tblCollectionReport.EReceipts", values);
                                    //    }
                                    //    else
                                    //    {
                                    //        query = string.Format("SELECT [ID] , [Provider] , [Channel] ,[PaymentRefNumber] , [DepositSlipNumber] , CONVERT(VARCHAR,CONVERT(DATE,[PaymentDate])) AS PaymentDate,[PayerID] , UPPER([PayerName]) AS [PayerName], [Amount] ,[PaymentMethod] ,[ChequeNumber] ,[ChequeValueDate] ,[ChequeStatus] ,[DateChequeReturned] ,[TelephoneNumber] ,[ReceiptNo] ,[ReceiptDate] ,UPPER([PayerAddress]) as [PayerAddress] ,[User] ,[RevenueCode] ,[Description] , [ChequeBankCode] ,[ChequeBankName] ,[AgencyName] ,[AgencyCode] ,[BankCode] ,[BankName] ,[BranchCode] ,[BranchName] ,[ZoneCode] ,[ZoneName] ,[Username] ,[AmountWords] ,[EReceipts] ,[EReceiptsDate] ,[GeneratedBy] ,[StationCode] ,(SELECT TOP 1 StationName FROM Receipt.tblStation  WHERE tblStation.StationCode = Collection.tblCollectionReport.[StationCode]) AS StationName from Collection.tblCollectionReport WHERE PaymentRefNumber IN ({0})  ORDER BY Collection.tblCollectionReport.AgencyName ,Collection.tblCollectionReport.Description,Collection.tblCollectionReport.EReceipts", values);
                                    //    }
                                    //}
                                    //else
                                    //{

                                    //}

                                    switch (Program.intCode)
                                    {
                                        case 20://detla state
                                            query = string.Format("SELECT [ID] , [Provider] , [Channel] ,[PaymentRefNumber] , [DepositSlipNumber] , CONVERT(VARCHAR,CONVERT(DATE,[PaymentDate])) AS PaymentDate,[PayerID] , UPPER([PayerName]) AS [PayerName], [Amount] ,[PaymentMethod] ,[ChequeNumber] ,[ChequeValueDate] ,[ChequeStatus] ,[DateChequeReturned] ,[TelephoneNumber] ,[ReceiptNo] ,[ReceiptDate] ,UPPER([PayerAddress]) as [PayerAddress] ,[User] ,[RevenueCode] ,tblCollectionReport.Description , [ChequeBankCode] ,[ChequeBankName] ,[AgencyName] ,[AgencyCode] ,[BankCode] ,[BankName] ,[BranchCode] ,[BranchName] ,[ZoneCode] ,[ZoneName] ,[Username] ,[AmountWords] ,[EReceipts] ,[EReceiptsDate] ,[GeneratedBy] ,[StationCode] ,(SELECT TOP 1 StationName FROM Receipt.tblStation  WHERE tblStation.StationCode = Collection.tblCollectionReport.[StationCode]) AS StationName,ControlNumber, Symbol , Surfix , tblCurrency.Description AS prefix from Collection.tblCollectionReport INNER JOIN Reconciliation.tblCurrency ON tblCurrency.CurrencyCode = tblCollectionReport.CurrencyCode WHERE PaymentRefNumber IN ({0})   ORDER BY Collection.tblCollectionReport.AgencyName ,Collection.tblCollectionReport.Description,Collection.tblCollectionReport.EReceipts", values);
                                            break;
                                        default:
                                            query = string.Format("SELECT [ID] , [Provider] , [Channel] ,[PaymentRefNumber] , [DepositSlipNumber] , CONVERT(VARCHAR,CONVERT(DATE,[PaymentDate])) AS PaymentDate,[PayerID] , UPPER([PayerName]) AS [PayerName], [Amount] ,[PaymentMethod] ,[ChequeNumber] ,[ChequeValueDate] ,[ChequeStatus] ,[DateChequeReturned] ,[TelephoneNumber] ,[ReceiptNo] ,[ReceiptDate] ,UPPER([PayerAddress]) as [PayerAddress] ,[User] ,[RevenueCode] ,tblCollectionReport.Description , [ChequeBankCode] ,[ChequeBankName] ,[AgencyName] ,[AgencyCode] ,[BankCode] ,[BankName] ,[BranchCode] ,[BranchName] ,[ZoneCode] ,[ZoneName] ,[Username] ,[AmountWords] ,[EReceipts] ,[EReceiptsDate] ,[GeneratedBy] ,[StationCode] ,(SELECT TOP 1 StationName FROM Receipt.tblStation  WHERE tblStation.StationCode = Collection.tblCollectionReport.[StationCode]) AS StationName, Symbol , Surfix , tblCurrency.Description AS prefix from Collection.tblCollectionReport INNER JOIN Reconciliation.tblCurrency ON tblCurrency.CurrencyCode = tblCollectionReport.CurrencyCode WHERE PaymentRefNumber IN ({0})   ORDER BY Collection.tblCollectionReport.AgencyName ,Collection.tblCollectionReport.Description,Collection.tblCollectionReport.EReceipts", values);
                                            break;
                                    }


                                    DataTable Dt = dds.Tables.Add("CollectionReportTable");
                                    ada = new SqlDataAdapter(query, Logic.ConnectionString);
                                    ada.Fill(dds, "CollectionReportTable");
                                    //Logic.ProcessDataTable(Dt);;
                                    Logic.ProcessDataTable(Dt);
                                    //strCollectionReportID = strFormat;
                                }

                                //XRepReceipt recportRec = new XRepReceipt { DataSource = dds.Tables[0] /*recportRec.DataAdapter = ada;*/, DataMember = "CollectionReportTable" };

                                //recportRec.ShowPreviewDialog();

                                ////selection.ClearSelection(); 
                                //dds.Clear();

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
                                        delta.ShowPreviewDialog();
                                        //delta.Print();
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

                                    //case 32://kogi state

                                    //    XRepReceiptkogi recportRecko = new XRepReceiptkogi { DataSource = dds.Tables[0] /*recportRec.DataAdapter = ada;*/, DataMember = "CollectionReportTable" };
                                    //    recportRecko.ShowPreviewDialog();

                                    //    break;

                                    default:

                                        break;
                                }
                            }

                            //dds.Clear();
                            DialogResult result = MessageBox.Show(" Is Receipt Printing Successful ?", Program.ApplicationName, MessageBoxButtons.YesNo);

                            if (result == DialogResult.Yes)
                            {
                                try
                                {
                                    SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

                                    //update the collection table by seting the isprinted to true
                                    using (SqlConnection dbs = new SqlConnection(Logic.ConnectionString))
                                    {
                                        SqlTransaction transactions;

                                        dbs.Open();

                                        //transaction = db.BeginTransaction();

                                        transactions = dbs.BeginTransaction();

                                        try
                                        {
                                            //string query1 = String.Format("UPDATE tblCollectionReport SET isPrinted=1,IsPrintedDate= '{0:MM/dd/yyyy HH:mm:ss tt}',PrintedBY='{1}' ,StationCode='{2}' WHERE PaymentRefNumber IN ({3})", DateTime.Now, Program.UserID, Program.stationCode, values);

                                            string query1 = String.Format("UPDATE Receipt.tblCollectionReceipt SET isPrinted=1,IsPrintedDate= '{0:MM/dd/yyyy HH:mm:ss tt}',PrintedBY='{1}',StationCode='{2}' WHERE PaymentRefNumber IN (SELECT PaymentRefNumber FROM Receipt.tblReceipt WHERE SentBy='{1}')", DateTime.Now, Program.UserID, Program.stationCode);

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
                                    bttnMain.Visible = true; label1.Visible = true;
                                    bttnMain.Enabled = true;
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
                        Tripous.Sys.ErrorBox(response.Tables[0].Rows[0]["returnMessage"].ToString());
                        return;
                    }
                    else
                    {
                        using (Frmcontrol frmcontrol = new Frmcontrol())
                        {
                            frmcontrol.gridControl1.DataSource = response.Tables[1];
                            frmcontrol.gridView1.BestFitColumns();
                            frmcontrol.label1.Text = "Payment Ref. Number Receipt printing in Process";
                            frmcontrol.Text = "Payment Ref. Number Receipt printing in Process";
                            frmcontrol.ShowDialog();
                        }

                        DialogResult result = MessageBox.Show("Do you wish to continue to Print the Receipts?", Program.ApplicationName, MessageBoxButtons.YesNo);

                        if (result == DialogResult.Yes)
                        {

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
                                            string strFormat = null; string query = string.Empty;


                                            switch (Program.intCode)
                                            {
                                                case 20://detla state
                                                    query = string.Format("SELECT [ID] , [Provider] , [Channel] ,[PaymentRefNumber] , [DepositSlipNumber] , CONVERT(VARCHAR,CONVERT(DATE,[PaymentDate])) AS PaymentDate,[PayerID] , UPPER([PayerName]) AS [PayerName], [Amount] ,[PaymentMethod] ,[ChequeNumber] ,[ChequeValueDate] ,[ChequeStatus] ,[DateChequeReturned] ,[TelephoneNumber] ,[ReceiptNo] AS [EReceipts] ,[ReceiptDate] ,UPPER([PayerAddress]) as [PayerAddress] ,[User] ,[RevenueCode] ,tblCollectionReport.Description , [ChequeBankCode] ,[ChequeBankName] ,[AgencyName] ,[AgencyCode] ,[BankCode] ,[BankName] ,[BranchCode] ,[BranchName] ,[ZoneCode] ,[ZoneName] ,[Username] ,[AmountWords] ,[EReceiptsDate] ,[GeneratedBy] ,[StationCode] ,(SELECT TOP 1 StationName FROM Receipt.tblStation  WHERE tblStation.StationCode = Collection.tblCollectionReport.[StationCode]) AS StationName, Symbol , Surfix , tblCurrency.Description AS prefix from Collection.tblCollectionReport INNER JOIN Reconciliation.tblCurrency ON tblCurrency.CurrencyCode = tblCollectionReport.CurrencyCode WHERE PaymentRefNumber IN ({0})  ORDER BY Collection.tblCollectionReport.AgencyName ,Collection.tblCollectionReport.Description,Collection.tblCollectionReport.ReceiptNo", values);
                                                    break;
                                                default:
                                                    query = string.Format("SELECT [ID] , [Provider] , [Channel] ,[PaymentRefNumber] , [DepositSlipNumber] , CONVERT(VARCHAR,CONVERT(DATE,[PaymentDate])) AS PaymentDate,[PayerID] , UPPER([PayerName]) AS [PayerName], [Amount] ,[PaymentMethod] ,[ChequeNumber] ,[ChequeValueDate] ,[ChequeStatus] ,[DateChequeReturned] ,[TelephoneNumber] ,[ReceiptNo] ,[ReceiptDate] ,UPPER([PayerAddress]) as [PayerAddress] ,[User] ,[RevenueCode] ,tblCollectionReport.Description , [ChequeBankCode] ,[ChequeBankName] ,[AgencyName] ,[AgencyCode] ,[BankCode] ,[BankName] ,[BranchCode] ,[BranchName] ,[ZoneCode] ,[ZoneName] ,[Username] ,[AmountWords] ,[EReceipts] ,[EReceiptsDate] ,[GeneratedBy] ,[StationCode] ,(SELECT TOP 1 StationName FROM Receipt.tblStation  WHERE tblStation.StationCode = Collection.tblCollectionReport.[StationCode]) AS StationName, Symbol , Surfix , tblCurrency.Description AS prefix from Collection.tblCollectionReport INNER JOIN Reconciliation.tblCurrency ON tblCurrency.CurrencyCode = tblCollectionReport.CurrencyCode WHERE PaymentRefNumber IN ({0})  ORDER BY Collection.tblCollectionReport.AgencyName ,Collection.tblCollectionReport.Description,Collection.tblCollectionReport.EReceipts", values);
                                                    break;
                                            }

                                            DataTable Dt = dds.Tables.Add("CollectionReportTable");
                                            ada = new SqlDataAdapter(query, Logic.ConnectionString);
                                            ada.Fill(dds, "CollectionReportTable");
                                            //Logic.ProcessDataTable(Dt);;
                                            Logic.ProcessDataTable(Dt); ;
                                            //strCollectionReportID = strFormat;
                                        }


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
                                                delta.logoPath = Logic.singaturepth;
                                                delta.ShowPreviewDialog();
                                                //delta.Print();
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

                                            //case 32://kogi state

                                            //    XRepReceiptkogi recportRecko = new XRepReceiptkogi { DataSource = dds.Tables[0] /*recportRec.DataAdapter = ada;*/, DataMember = "CollectionReportTable" };
                                            //    recportRecko.ShowPreviewDialog();

                                            //    break;

                                            default:
                                                break;
                                        }
                                        //selection.ClearSelection(); 
                                        dds.Clear();
                                    }

                                    DialogResult results = MessageBox.Show(" Is Receipt Printing Successful ?", Program.ApplicationName, MessageBoxButtons.YesNo);

                                    if (results == DialogResult.Yes)
                                    {
                                        try
                                        {
                                            SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

                                            //update the collection table by seting the isprinted to true
                                            using (SqlConnection dbs = new SqlConnection(Logic.ConnectionString))
                                            {
                                                SqlTransaction transactions;

                                                dbs.Open();

                                                //transaction = db.BeginTransaction();

                                                transactions = dbs.BeginTransaction();

                                                try
                                                {
                                                    string query1 = String.Format("UPDATE Receipt.tblCollectionReceipt SET isPrinted=1,IsPrintedDate= '{0:MM/dd/yyyy HH:mm:ss tt}',PrintedBY='{1}' ,StationCode='{2}' WHERE PaymentRefNumber IN ({3})", DateTime.Now, Program.UserID, Program.stationCode, values);

                                                    //string query1 = String.Format("UPDATE Receipt.tblCollectionReceipt SET isPrinted=1,IsPrintedDate= '{0:MM/dd/yyyy HH:mm:ss tt}',PrintedBY='{1}',StationCode='{2}' WHERE PaymentRefNumber IN (SELECT PaymentRefNumber FROM Receipt.tblReceipt WHERE SentBy='{1}')", DateTime.Now, Program.UserID, Program.stationCode);

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
                                            bttnMain.Visible = true; label1.Visible = true;
                                            bttnMain.Enabled = true;
                                        }
                                        finally
                                        {
                                            SplashScreenManager.CloseForm(false);
                                        }

                                        //using (FrmMainFest frmMainFest = new FrmMainFest(strCollectionReportID, values, criteria2, false) { IDList = strCollectionReportID })
                                        //{
                                        //    this.Close();
                                        //    frmMainFest.ShowDialog();
                                        //}


                                    }
                                    else
                                        return;

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
                            return;

                    }

                    //else
                    //{
                    //    Common.setMessageBox(response.Tables[0].Rows[0]["returnMessage"].ToString(), "", 2);
                    //    return;
                    //}




                }
            }
            //load the recport
            //using (SqlConnection db = new SqlConnection(Logic.ConnectionString))
            //{
            //    SqlTransaction transaction;

            //    db.Open();

            //    transaction = db.BeginTransaction();

            //    try
            //    {
            //        using (System.Data.DataSet dds = new System.Data.DataSet("DsCollectionReport"))
            //        {
            //            SqlDataAdapter ada;

            //            //using (WaitDialogForm form = new WaitDialogForm())
            //            //{
            //            string strFormat = null;

            //            string query = String.Format("SELECT [ID] , [Provider] , [Channel] ,[PaymentRefNumber] , [DepositSlipNumber] , CONVERT(VARCHAR,CONVERT(DATETime,[PaymentDate])) AS PaymentDate,[PayerID] , UPPER([PayerName]) AS [PayerName], [Amount] ,[PaymentMethod] ,[ChequeNumber] ,[ChequeValueDate] ,[ChequeStatus] ,[DateChequeReturned] ,[TelephoneNumber] ,[ReceiptNo] ,[ReceiptDate] ,UPPER([PayerAddress]) as [PayerAddress] ,[User] ,[RevenueCode] , Description , [ChequeBankCode] ,[ChequeBankName] , [AgencyName] ,[AgencyCode] ,[BankCode] ,[BankName] ,[BranchCode] ,[BranchName] ,[ZoneCode] ,[ZoneName] ,[Username] ,[AmountWords] ,[EReceipts] ,[EReceiptsDate] ,[GeneratedBy] ,[StationCode] ,(SELECT TOP 1 StationName from Receipt.tblStation WHERE tblStation.StationCode = Collection.tblCollectionReport.[StationCode]) AS StationName from Collection.tblCollectionReport WHERE PaymentRefNumber IN ({0}) ORDER BY Collection.tblCollectionReport.StationCode , Collection.tblCollectionReport.AgencyCode ,Collection.tblCollectionReport.RevenueCode,Collection.tblCollectionReport.EReceipts  ", values);


            //            DataTable Dt = dds.Tables.Add("CollectionReportTable");
            //            ada = new SqlDataAdapter(query, Logic.ConnectionString);
            //            ada.Fill(dds, "CollectionReportTable");
            //            Logic.ProcessDataTable(Dt);;
            //            strCollectionReportID = values;
            //            //}


            //            XRepReceipt recportRec = new XRepReceipt { DataSource = dds.Tables[0] /*recportRec.DataAdapter = ada;*/, DataMember = "CollectionReportTable" };

            //            recportRec.ShowPreviewDialog();

            //            //selection.ClearSelection();
            //            //dds.Clear();
            //        }
            //        db.Close();
            //    }
            //    catch (SqlException sqlError)
            //    {
            //        Tripous.Sys.ErrorBox(sqlError);
            //    }

            //}
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
                MDIMain.publicMDIParent.RemoveControls(); Close();
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


    }
}
