using Collection.Classess;
using Collection.Report;
using DevExpress.XtraBars.Alerter;
using DevExpress.XtraNavBar;
//using Microsoft.SqlServer.Management.Smo;
//using Microsoft.SqlServer.Management.Common;
using DevExpress.XtraReports.UI;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
//using TaxSmartSuite.AutoUpdate;

namespace Collection.Forms
{
    public partial class MDIMain : Form
    {
        private SqlCommand command;

        private SqlDataAdapter adp;
        //navBarItemReceipt
        private string retval;

        private int childFormNumber;

        public static string stateCode;

        public static MDIMain publicMDIParent;

        private System.Data.DataSet ds;


        SqlDataAdapter ada;

        Timer timer = new Timer();

        int lockTimerCounter;

        int offsetCounter = 0;

        //protected string marqueeString = "Tax Smart Collection Manager --- Powered by ICMA Services";

        protected string marqueeString = string.Empty;
        public MDIMain()
        {
            InitializeComponent();

            publicMDIParent = this;

            Init2();

            Load += MDIMainForm_Load;

            Resize += MDIMainForm_Resize;

            FormClosing += MDIMainForm_FormClosing;

            timer.Tick += OnTimerTick;

            Init();

            //NavBars.ManageNavBarControls(navBarControl1, Program.ApplicationCode);

            NavBars.NavBarEnableDisableControls(navBarControl1, false);

            NavBars.ManageUserLoginNavBar(navBarControl1);

            //scripts();
        }

        void MDIMainForm_Load(object sender, EventArgs e)
        {

            if (Program.stateCode == "20")
            {
                marqueeString = System.Configuration.ConfigurationManager.AppSettings["marquee1"];

                Text = String.Format("ICMA Services :: Smart Receipt -->>  For {0} State [ Server Name == {1} ]-->> Station Name:: {2}  -->> User ID:: {3}", Program.StateName, Program.ServerName, Program.stationName, Program.UserID);

            }
            else
            {
                marqueeString = System.Configuration.ConfigurationManager.AppSettings["marquee"];

                Text = String.Format("ICMA Services :: {2} -->>  For {0} State [ Server Name == {1} ]-->> Station Name:: {3}  -->> User ID:: {4}", Program.StateName, Program.ServerName, Program.ApplicationName, Program.stationName, Program.UserID);
            }



            MenuEvent();

            //OnNavBarItemsClicked(navBarItem, null);

            scripts(); AutoCheckUpdate();
        }

        protected void Init()
        {
            timer.Stop();

            offsetCounter = -digitalGauge1.DigitCount;

            timer.Interval = 500 / 3;

            timer.Start();
        }

        protected void UpdateText()
        {
            string fullTextToShow = marqueeString;

            char[] textToShow = new char[digitalGauge1.DigitCount];

            for (int i = 0; i < digitalGauge1.DigitCount; i++)
            {
                if (i + offsetCounter >= 0 && i + offsetCounter < fullTextToShow.Length)
                {
                    textToShow[i] = fullTextToShow[i + offsetCounter];
                }
                else textToShow[i] = ' ';
            }
            offsetCounter++;

            if (offsetCounter > digitalGauge1.DigitCount + fullTextToShow.Length) offsetCounter = -digitalGauge1.DigitCount;

            digitalGauge1.Text = new string(textToShow);

        }

        void OnTimerTick(object sender, EventArgs e)
        {
            if (lockTimerCounter == 0)
            {
                lockTimerCounter++;

                UpdateText();

                lockTimerCounter--;
            }
        }

        void MDIMainForm_Resize(object sender, EventArgs e)
        {
            gaugeControl1.Left = (panelControl1.Width - gaugeControl1.Width) / 2;
        }

        void MDIMainForm_FormClosing(object sender, FormClosingEventArgs e)
        {

            e.Cancel = !CloseForm();
        }

        protected void MenuEvent()
        {
            navBarItem1.LinkClicked += OnNavBarItemsClicked;

            navBarItemExit.LinkClicked += OnNavBarItemsClicked;

            navBarItem2.LinkClicked += OnNavBarItemsClicked;

            navBarItemPrintReceipt.LinkClicked += OnNavBarItemsClicked;

            //navBarItemCollExp.LinkClicked += OnNavBarItemsClicked;

            //navBarItemTaxAgentExp.LinkClicked += OnNavBarItemsClicked;

            //navBarItemTaxPayerExp.LinkClicked += OnNavBarItemsClicked;

            //navBarItemRevenueTaxOffice.LinkClicked += OnNavBarItemsClicked;

            navBarItemReceiptCentre.LinkClicked += OnNavBarItemsClicked;

            //navBarItemTaxAgentImpt.LinkClicked += OnNavBarItemsClicked;

            //navBarItemTaxPayerImpt.LinkClicked += OnNavBarItemsClicked;

            //navBarItemPlatform.LinkClicked += OnNavBarItemsClicked;

            //navBarItemCakkshBank.LinkClicked += OnNavBarItemsClicked;

            //navBarItemStationMap.LinkClicked += OnNavBarItemsClicked;

            //navBarItemSummary.LinkClicked += OnNavBarItemsClicked;

            navBarItemLogout.LinkClicked += OnNavBarItemsClicked;

            navBarItemGeneral.LinkClicked += OnNavBarItemsClicked;

            navBarItemReprint.LinkClicked += OnNavBarItemsClicked;

            navBarItemPending.LinkClicked += OnNavBarItemsClicked;

            navBarItemReceipt.LinkClicked += OnNavBarItemsClicked;

            navBarItemSearchReceipts.LinkClicked += OnNavBarItemsClicked;

            navBarItem6.LinkClicked += OnNavBarItemsClicked;

            navBarItmArmsSearch.LinkClicked += OnNavBarItemsClicked;

            //navBarItemStations.LinkClicked += OnNavBarItemsClicked;

            navBarItemReceiptDemand.LinkClicked += OnNavBarItemsClicked;

            navBarItemReceiptReceive.LinkClicked += OnNavBarItemsClicked;

            navBarItemReceiptIssue.LinkClicked += OnNavBarItemsClicked;

            //navBarItemPrrintingCentreMap.LinkClicked += OnNavBarItemsClicked;

            navBarItemPriningRevenueCode.LinkClicked += OnNavBarItemsClicked;

            navBarItemReceiptReportDetails.LinkClicked += OnNavBarItemsClicked;

            navBarItemRevenueException.LinkClicked += OnNavBarItemsClicked;

            //navBarItemCheckUpdate.LinkClicked += OnNavBarItemsClicked;

            navBarItemReceiptGenerated.LinkClicked += OnNavBarItemsClicked;

            navBarItemStationData.LinkClicked += OnNavBarItemsClicked;

            navBarItemCentreData.LinkClicked += OnNavBarItemsClicked;

            navBarItemReprintReceipt.LinkClicked += OnNavBarItemsClicked;

            navBarItemReceiptIssuedReport.LinkClicked += OnNavBarItemsClicked;

            navBarStationCollection.LinkClicked += OnNavBarItemsClicked;

            navBarItemSplitTransaction.LinkClicked += OnNavBarItemsClicked;

            navBarItemReceiptModification.LinkClicked += OnNavBarItemsClicked;

            //navBarItemReceiptCancel.LinkClicked += OnNavBarItemsClicked;

            //navBarItemTransactionPending.LinkClicked += OnNavBarItemsClicked;

            navBarItemDemandReceipt.LinkClicked += OnNavBarItemsClicked;

            navBarItem3.LinkClicked += OnNavBarItemsClicked;

            navBarItem4.LinkClicked += OnNavBarItemsClicked;

            navBarItem5.LinkClicked += OnNavBarItemsClicked;

            navBarImtProfile.LinkClicked += OnNavBarItemsClicked;

            navBarItmTCO.LinkClicked += OnNavBarItemsClicked;

            navBarItCheck.LinkClicked += OnNavBarItemsClicked;

            navBarItem7.LinkClicked += OnNavBarItemsClicked;

            navBarItmArmsApproval.LinkClicked += OnNavBarItemsClicked;

            navBarItem8.LinkClicked += OnNavBarItemsClicked;
        }

        void OnNavBarItemsClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            if (sender != navBarItemExit)
                RemoveControls();

            //if (sender == navBarItemCollImpt)
            //{
            //    FrmImports.tableType = 1;

            //    tableLayoutPanel2.Controls.Add((new FrmImports().panelContainer), 1, 0);

            //    FrmImports.publicStreetGroup.Tag = ((sender) as NavBarItem).Tag;

            //    FrmImports.publicStreetGroup.RefreshForm();
            //}
            //else if (sender == navBarItemTaxAgentImpt)
            //{
            //    FrmImports.tableType = 2;

            //    tableLayoutPanel2.Controls.Add((new FrmImports().panelContainer), 1, 0);

            //    FrmImports.publicStreetGroup.Tag = ((sender) as NavBarItem).Tag;

            //    FrmImports.publicStreetGroup.RefreshForm();

            //}
            //else if (sender == navBarItemTaxPayerImpt)
            //{
            //    FrmImports.tableType = 3;

            //    tableLayoutPanel2.Controls.Add((new FrmImports().panelContainer), 1, 0);

            //    FrmImports.publicStreetGroup.Tag = ((sender) as NavBarItem).Tag;

            //    FrmImports.publicStreetGroup.RefreshForm();
            //}
            //if (sender == navBarItemCollExp)
            //{
            //    FrmExport.tableType = 1;

            //    tableLayoutPanel2.Controls.Add((new FrmExport().panelContainer), 1, 0);

            //    FrmExport.publicStreetGroup.Tag = ((sender) as NavBarItem).Tag;

            //    FrmExport.publicStreetGroup.RefreshForm();
            //}

            if (sender == navBarItemSplitTransaction)
            {
                tableLayoutPanel2.Controls.Add((new FrmSplit().panelContainer), 1, 0);

                FrmSplit.publicInstance.Tag = ((sender) as NavBarItem).Tag;

                FrmSplit.publicInstance.RefreshForm();
            }
            else if (sender == navBarItem8)//change password
            {
                using (var frm = new FrmChangePassword())
                {
                    frm.ShowDialog();
                }
            }
            else if (sender == navBarItCheck)
            {
                //string paths = AppDomain.CurrentDomain.BaseDirectory;
                CheckUpdate();
            }
            else if (sender == navBarItem1)
            {
                tableLayoutPanel2.Controls.Add((new FrmPayment().panelContainer), 1, 0);

                FrmPayment.publicInstance.Tag = ((sender) as NavBarItem).Tag;

                FrmPayment.publicInstance.RefreshForm();

                FrmPayment.boolIsUpdate = true;

            }
            else if (sender == navBarItem6)
            {
                tableLayoutPanel2.Controls.Add((new FrmCreatMainfest().panelContainer), 1, 0);

                FrmCreatMainfest.publicInstance.Tag = ((sender) as NavBarItem).Tag;

                FrmCreatMainfest.publicInstance.RefreshForm();
            }
            else if (sender == navBarItem2)
            {
                tableLayoutPanel2.Controls.Add((new FrmPayment().panelContainer), 1, 0);

                FrmPayment.publicInstance.Tag = ((sender) as NavBarItem).Tag;

                FrmPayment.publicInstance.RefreshForm();

                FrmPayment.boolIsUpdate = false;
            }
            else if (sender == navBarItmTCO)
            {
                tableLayoutPanel2.Controls.Add((new FrmTCO().panelContainer), 1, 0);

                FrmTCO.publicInstance.Tag = ((sender) as NavBarItem).Tag;

                FrmTCO.publicInstance.RefreshForm();
            }
            //else if (sender == navBarItemTaxPayerExp)
            //{
            //    FrmExport.tableType = 3;

            //    tableLayoutPanel2.Controls.Add((new FrmExport().panelContainer), 1, 0);

            //    FrmExport.publicStreetGroup.Tag = ((sender) as NavBarItem).Tag;

            //    FrmExport.publicStreetGroup.RefreshForm();
            //}
            //else if (sender == navBarItemTransactionPending)
            //{
            //    tableLayoutPanel2.Controls.Add((new FrmPending().panelContainer), 1, 0);

            //    FrmPending.publicStreetGroup.Tag = ((sender) as NavBarItem).Tag;

            //    FrmPending.publicStreetGroup.RefreshForm();
            //}
            //else if (sender == navBarItemCashBank)
            //{
            //    tableLayoutPanel2.Controls.Add((new FrmCashiers().panelContainer), 1, 0);

            //    FrmCashiers.publicInstance.Tag = ((sender) as NavBarItem).Tag;

            //    FrmCashiers.publicInstance.RefreshForm();

            //}
            else if (sender == navBarItmArmsSearch)
            {
                tableLayoutPanel2.Controls.Add((new FrmArmsSearch().panelContainer), 1, 0);
                FrmArmsSearch.publicStreetGroup.Tag = ((sender) as NavBarItem).Tag;
                FrmArmsSearch.publicStreetGroup.RefreshForm();
            }
            else if (sender == navBarItmArmsApproval)
            {
                tableLayoutPanel2.Controls.Add((new FrmArmsApproval().panelContainer), 1, 0);
                FrmArmsApproval.publicStreetGroup.Tag = ((sender) as NavBarItem).Tag;
                FrmArmsApproval.publicStreetGroup.RefreshForm();
            }
            else if (sender == navBarItemReceiptIssuedReport)
            {
                tableLayoutPanel2.Controls.Add((new FrmIssueReport().panelContainer), 1, 0);
                FrmIssueReport.publicStreetGroup.Tag = ((sender) as NavBarItem).Tag;
                FrmIssueReport.publicStreetGroup.RefreshForm();
            }
            else if (sender == navBarItemReceiptModification)
            {
                tableLayoutPanel2.Controls.Add((new FrmModification().panelContainer), 1, 0);
                FrmModification.publicInstance.Tag = ((sender) as NavBarItem).Tag;
                FrmModification.publicInstance.RefreshForm();

            }
            else if (sender == navBarItem5)
            {
                tableLayoutPanel2.Controls.Add((new FrmStations().panelContainer), 1, 0);
                //FrmStations.publicInstance.Tag = ((sender) as NavBarItem).Tag;
                FrmStations.publicStreetGroup.Tag = ((sender) as NavBarItem).Tag;
                FrmStations.publicStreetGroup.RefreshForm();
            }
            else if (sender == navBarItem4)
            {
                tableLayoutPanel2.Controls.Add((new FrmGetApproved().panelContainer), 1, 0);
                FrmGetApproved.publicInstance.Tag = ((sender) as NavBarItem).Tag;
                FrmGetApproved.publicInstance.RefreshForm();
            }
            else if (sender == navBarImtProfile)
            {
                tableLayoutPanel2.Controls.Add((new FrmProfile().panelContainer), 1, 0);
                FrmProfile.publicInstance.Tag = ((sender) as NavBarItem).Tag;
                FrmProfile.publicInstance.RefreshForm();
            }
            //else if (sender == navBarItemCollect)
            //{
            //    ////tableLayoutPanel2.Controls.Add((new FrmDownload().panelContainer), 1, 0);
            //    tableLayoutPanel2.Controls.Add((new FrmDownloadSources().panelContainer), 1, 0);

            //    //FrmDownloadSources

            //}
            else if (sender == navBarItemDemandReceipt)
            {
                tableLayoutPanel2.Controls.Add((new FrmDemand().panelContainer), 1, 0);

                FrmDemand.publicStreetGroup.Tag = ((sender) as NavBarItem).Tag;

                FrmDemand.publicStreetGroup.RefreshForm();
            }
            else if (sender == navBarItem7)
            {
                tableLayoutPanel2.Controls.Add((new FrmFirstApproval().panelContainer), 1, 0);

                FrmFirstApproval.publicStreetGroup.Tag = ((sender) as NavBarItem).Tag;

                FrmFirstApproval.publicStreetGroup.RefreshForm();
            }
            //else if (sender == navBarItemSummary)
            //{
            //    //FrmSummaryReport
            //    tableLayoutPanel2.Controls.Add((new FrmSummaryReport().panelContainer), 1, 0);

            //    //FrmSummaryReport.publicInstance.Tag = ((sender) as NavBarItem).Tag;

            //    //FrmSummaryReport.publicInstance.RefreshForm();

            //    FrmSummaryReport.publicStreetGroup.Tag = ((sender) as NavBarItem).Tag;
            //    FrmSummaryReport.publicStreetGroup.RefreshForm();

            //}
            else if (sender == navBarItemReceiptCentre)
            {
                tableLayoutPanel2.Controls.Add((new FrmReceiptsCentre().panelContainer), 1, 0);

                FrmReceiptsCentre.publicInstance.Tag = ((sender) as NavBarItem).Tag;

                FrmReceiptsCentre.publicInstance.RefreshForm();
            }
            //else if (sender == navBarItemPrrintingCentreMap)
            //{
            //    tableLayoutPanel2.Controls.Add((new FrmReceiptCentreMap().panelContainer), 1, 0);

            //    FrmReceiptCentreMap.publicInstance.Tag = ((sender) as NavBarItem).Tag;

            //    FrmReceiptCentreMap.publicInstance.RefreshForm();
            //}
            //else if (sender == navBarItemCheckUpdate)
            //{
            //    Process prupdate = Process.Start("wyUpdate.exe");

            //}
            else if (sender == navBarItemReceiptGenerated)
            {
                tableLayoutPanel2.Controls.Add((new FrmGenerate().panelContainer), 1, 0);

                FrmGenerate.publicInstance.Tag = ((sender) as NavBarItem).Tag;

                //FrmGenerate.publicInstance.RefreshForm();

            }
            else if (sender == navBarItem3)
            {
                tableLayoutPanel2.Controls.Add((new FrmAgecnyMainfest().panelContainer), 1, 0);

                FrmAgecnyMainfest.publicInstance.Tag = ((sender) as NavBarItem).Tag;
            }
            else if (sender == navBarItemReceiptDemand)
            {
                //tableLayoutPanel2.Controls.Add((new FrmReceiptDemand().panelContainer), 1, 0);
                tableLayoutPanel2.Controls.Add((new FrmPendReceipts().panelContainer), 1, 0);

                FrmPendReceipts.publicInstance.Tag = ((sender) as NavBarItem).Tag;

                FrmPendReceipts.publicInstance.RefreshForm();
            }
            else if (sender == navBarItemStationData)
            {
                tableLayoutPanel2.Controls.Add((new FrmStationDownload().panelContainer), 1, 0);

                FrmStationDownload.publicStreetGroup.Tag = ((sender) as NavBarItem).Tag;

                FrmStationDownload.publicStreetGroup.RefreshForm();
            }
            else if (sender == navBarItemCentreData)
            {
                tableLayoutPanel2.Controls.Add((new FrmCentralData().panelContainer), 1, 0);

                FrmCentralData.publicStreetGroup.Tag = ((sender) as NavBarItem).Tag;

                FrmCentralData.publicStreetGroup.RefreshForm();
            }
            //else if (sender == navBarItemStations)
            //{
            //    tableLayoutPanel2.Controls.Add((new FrmStations().panelContainer), 1, 0);

            //    //FrmStations.publicInstance.Tag = ((sender) as NavBarItem).Tag;

            //    FrmStations.publicStreetGroup.Tag = ((sender) as NavBarItem).Tag;

            //    FrmStations.publicStreetGroup.RefreshForm();

            //}
            //else if (sender == navBarItemReceiptCancel)
            //{
            //    tableLayoutPanel2.Controls.Add((new FrmReceiptCancel().panelContainer), 1, 0);

            //    FrmReceiptCancel.publicStreetGroup.Tag = ((sender) as NavBarItem).Tag;

            //    FrmReceiptCancel.publicStreetGroup.RefreshForm();
            //}
            else if (sender == navBarItemPrintReceipt)
            {
                //tableLayoutPanel2.Controls.Add((new FrmPrintOption().panelContainer), 1, 0);
                var rec = new FrmReceipts();
                if (!rec.IsDisposed)
                    tableLayoutPanel2.Controls.Add((rec.panelContainer), 1, 0);

                FrmReceipts.publicInstance.Tag = ((sender) as NavBarItem).Tag;

                FrmReceipts.publicInstance.RefreshForm();
            }
            //else if (sender == navBarItemStationMap)
            //{
            //    tableLayoutPanel2.Controls.Add((new FrmStationMap().panelContainer), 1, 0);

            //    FrmStationMap.publicStreetGroup.Tag = ((sender) as NavBarItem).Tag;

            //    FrmStationMap.publicStreetGroup.RefreshForm();
            //}
            else if (sender == navBarItemReceipt)
            {
                tableLayoutPanel2.Controls.Add((new FrmDetailsReceipts().panelContainer), 1, 0);

                FrmDetailsReceipts.publicInstance.Tag = ((sender) as NavBarItem).Tag;

                FrmDetailsReceipts.publicInstance.RefreshForm();
            }
            else if (sender == navBarItemSearchReceipts)
            {
                tableLayoutPanel2.Controls.Add((new FrmSearchReceipts().panelContainer), 1, 0);

                FrmSearchReceipts.publicInstance.Tag = ((sender) as NavBarItem).Tag;

                FrmSearchReceipts.publicInstance.RefreshForm();
            }
            else if (sender == navBarItemPriningRevenueCode)
            {
                tableLayoutPanel2.Controls.Add((new FrmRevException().panelContainer), 1, 0);

                //FrmRevException.publicInstance.Tag = ((sender) as NavBarItem).Tag;

                FrmRevException.publicStreetGroup.Tag = ((sender) as NavBarItem).Tag;

                //FrmRevException.publicInstance.RefreshForm();
                FrmRevException.publicStreetGroup.RefreshForm();
            }
            else if (sender == navBarItemReceiptReportDetails)
            {
                using (System.Data.DataSet dds = new System.Data.DataSet("DsCollectionReport"))
                {
                    SqlDataAdapter ada;

                    DataTable Dt = dds.Tables.Add("CollectionReportTable");
                    ada = new SqlDataAdapter((string)"SELECT ID, Provider, Channel, Collection.tblCollectionReport.PaymentRefNumber, DepositSlipNumber, PaymentDate, PayerID, PayerName, Amount, PaymentMethod, ChequeNumber, ChequeValueDate, ChequeStatus, DateChequeReturned, TelephoneNumber, ReceiptNo, ReceiptDate, PayerAddress, [User], RevenueCode, Description, ChequeBankCode, ChequeBankName, AgencyName, AgencyCode, BankCode, BankName, BranchCode, BranchName, ZoneCode, ZoneName, Username,  AmountWords, Collection.tblCollectionReport.EReceipts, Collection.tblCollectionReport.EReceiptsDate, GeneratedBy,  Receipt.tblCollectionReceipt.UploadStatus, Receipt.tblCollectionReceipt.PrintedBY,Receipt.tblCollectionReceipt.ControlNumber, Receipt.tblCollectionReceipt.BatchNumber, Collection.tblCollectionReport.StationCode,(SELECT StationName FROM Receipt.tblStation WHERE StationCode = Collection.tblCollectionReport.StationCode) AS Stationname, Receipt.tblCollectionReceipt.isPrinted,Receipt.tblCollectionReceipt.IsPrintedDate FROM Collection.tblCollectionReport LEFT JOIN Receipt.tblCollectionReceipt ON Collection.tblCollectionReport.PaymentRefNumber = Receipt.tblCollectionReceipt.PaymentRefNumber", Logic.ConnectionString);
                    ada.Fill(dds, "CollectionReportTable");

                    Report.XRepReceiptReport receiptReport = new XRepReceiptReport { DataSource = dds /*recportRec.DataAdapter = ada;*/, DataMember = "CollectionReportTable" };

                    receiptReport.ShowPreviewDialog();


                }

            }
            else if (sender == navBarStationCollection)
            {
                using (System.Data.DataSet dds = new System.Data.DataSet("DsCollectionReport"))
                {
                    SqlDataAdapter ada;

                    string query = "SELECT ID, Provider, Channel, Collection.tblCollectionReport.PaymentRefNumber, DepositSlipNumber, PaymentDate, PayerID, PayerName, Amount, PaymentMethod, ChequeNumber, ChequeValueDate, ChequeStatus, DateChequeReturned, TelephoneNumber, ReceiptNo, ReceiptDate, PayerAddress, [User], RevenueCode, Description, ChequeBankCode, ChequeBankName, AgencyName, AgencyCode, BankCode, BankName, BranchCode, BranchName, ZoneCode, ZoneName, Username,  AmountWords, Collection.tblCollectionReport.EReceipts, Collection.tblCollectionReport.EReceiptsDate, GeneratedBy,  Receipt.tblCollectionReceipt.UploadStatus, Receipt.tblCollectionReceipt.PrintedBY,Receipt.tblCollectionReceipt.ControlNumber, Receipt.tblCollectionReceipt.BatchNumber, Collection.tblCollectionReport.StationCode,(SELECT StationName FROM Receipt.tblStation WHERE StationCode = Collection.tblCollectionReport.StationCode) AS Stationname, Receipt.tblCollectionReceipt.isPrinted,Receipt.tblCollectionReceipt.IsPrintedDate FROM Collection.tblCollectionReport LEFT JOIN Receipt.tblCollectionReceipt ON Collection.tblCollectionReport.PaymentRefNumber = Receipt.tblCollectionReceipt.PaymentRefNumber";

                    DataTable Dt = dds.Tables.Add("CollectionReportTable");
                    ada = new SqlDataAdapter(query, Logic.ConnectionString);
                    ada.Fill(dds, "CollectionReportTable");

                    //Report.XtraRepDetails repost = new XtraRepDetails();
                    //repost.ShowPreviewDialog();

                    Report.XtraRepDetails repost = new XtraRepDetails { DataSource = dds /*recportRec.DataAdapter = ada;*/, DataMember = "CollectionReportTable" };
                    repost.ShowPreviewDialog();


                }
            }
            else if (sender == navBarItemReprintReceipt)
            {
                tableLayoutPanel2.Controls.Add((new FrmReprinted().panelContainer), 1, 0);
            }
            else if (sender == navBarItemGeneral)
            {
                tableLayoutPanel2.Controls.Add((new FrmGeneral().panelContainer), 1, 0);

                FrmGeneral.publicInstance.Tag = ((sender) as NavBarItem).Tag;

                FrmGeneral.publicInstance.RefreshForm();


                //                string vquery;
                //                try
                //                {
                //                    //vquery = String.Format(@"SELECT * FROM dbo.tblCollectionReport WHERE ID IN (SELECT * FROM [fnConvertCSVToINT]('{0}'))", IDList);

                //                    #region SQL
                //                    vquery = @"SELECT  [ID] ,
                //                                         [Provider] ,
                //                                         [Channel] ,
                //                                         [PaymentRefNumber] ,
                //                                         [DepositSlipNumber] ,
                //                                         CONVERT(VARCHAR,CONVERT(DATE,[PaymentDate]),103) AS PaymentDate,
                //                                         [PayerID] ,
                //                                         UPPER([PayerName]) AS [PayerName],
                //                                         [Amount] ,
                //                                         [PaymentMethod] ,
                //                                         [ChequeNumber] ,
                //                                         [ChequeValueDate] ,
                //                                         [ChequeStatus] ,
                //                                         [DateChequeReturned] ,
                //                                         [TelephoneNumber] ,
                //                                         [ReceiptNo] ,
                //                                         [ReceiptDate] ,
                //                                         [PayerAddress] ,
                //                                         [Status] ,
                //                                         [User] ,
                //                                         [RevenueCode] ,
                //                                         [Description] ,
                //                                         [ChequeBankCode] ,
                //                                         [ChequeBankName] ,
                //                                         [AgencyName] ,
                //                                         [AgencyCode] ,
                //                                         [BankCode] ,
                //                                         [BankName] ,
                //                                         [BranchCode] ,
                //                                         [BranchName] ,                                        
                //                                         [ZoneCode] ,
                //                                         [ZoneName] ,
                //                                         [Username] ,
                //                                         [State] ,
                //                                         [AmountWords] ,
                //                                         [URL] ,
                //                                         [EReceipts] ,
                //                                         [EReceiptsDate] ,
                //                                         [GeneratedBy] ,
                //                                         [DateValidatedAgainst] ,
                //                                         [DateDiff] ,
                //                                         [UploadStatus] ,
                //                                         [PrintedBY] ,
                //                                         [DatePrinted] ,
                //                                         [ControlNumber] ,
                //                                         [BatchNumber] ,
                //                                         [StationCode] ,
                //--                                         [StationName]
                //--[StationName]
                //                                         (Select StationName from tblStation WHERE tblStation.StationCode = ViewCollectionReport.[StationCode]) AS StationName
                //                                 FROM    [dbo].[ViewCollectionReport]
                //                                  WHERE EReceipts IS NOT NULL ORDER BY ViewCollectionReport.AgencyCode,ViewCollectionReport.StationCode  ,ViewCollectionReport.RevenueCode,ViewCollectionReport.EReceipts";
                //                    #endregion
                //                    using (System.Data.DataSet dds = new System.Data.DataSet("DsCollectionReport"))
                //                    {
                //                        using (SqlDataAdapter ada = new SqlDataAdapter(vquery, Logic.ConnectionString))
                //                        {
                //                            ada.Fill(dds, "CollectionReportTable");
                //                            Report.XRepGlobalManifest Global = new Collection.Report.XRepGlobalManifest { DataSource = dds, DataAdapter = ada, DataMember = "CollectionReportTable", RequestParameters = true };
                //                            //Global.paramStartDate.Value = "EReceiptsDate";
                //                            //Global.paramEndDate.Value = "EReceiptsDate";
                //                            // Global.paramEndDate.Visible = false;
                //                            Global.ShowPreviewDialog();
                //                        }
                //                    }
                //                }
                //                catch
                //                {
                //                    vquery = @"SELECT [ID] ,
                //                                         [Provider] ,
                //                                         [Channel] ,
                //                                         [PaymentRefNumber] ,
                //                                         [DepositSlipNumber] ,
                //                                         [PaymentDate],
                //                                         [PayerID] ,
                //                                         UPPER([PayerName]) AS [PayerName],
                //                                         [Amount] ,
                //                                         [PaymentMethod] ,
                //                                         [ChequeNumber] ,
                //                                         [ChequeValueDate] ,
                //                                         [ChequeStatus] ,
                //                                         [DateChequeReturned] ,
                //                                         [TelephoneNumber] ,
                //                                         [ReceiptNo] ,
                //                                         [ReceiptDate] ,
                //                                         [PayerAddress] ,
                //                                         [Status] ,
                //                                         [User] ,
                //                                         [RevenueCode] ,
                //                                         [Description] ,
                //                                         [ChequeBankCode] ,
                //                                         [ChequeBankName] ,
                //                                         [AgencyName] ,
                //                                         [AgencyCode] ,
                //                                         [BankCode] ,
                //                                         [BankName] ,
                //                                         [BranchCode] ,
                //                                         [BranchName] ,
                //                                        [BankName]+' / '+ [BranchName] AS calcBankBranchCol,
                //                                         [ZoneCode] ,
                //                                         [ZoneName] ,
                //                                         [Username] ,
                //                                         [State] ,
                //                                         [AmountWords] ,
                //                                         [URL] ,
                //                                         [EReceipts] ,
                //                                         [EReceiptsDate] ,
                //                                         [GeneratedBy] ,
                //                                         [DateValidatedAgainst] ,
                //                                         [DateDiff] ,
                //                                         [UploadStatus] ,
                //                                         [PrintedBY] ,
                //                                         [DatePrinted] ,
                //                                         [ControlNumber] ,
                //                                         [BatchNumber] ,
                //                                         [StationCode] ,
                //                                     --[StationName]
                //--[StationName]
                //                                         (Select StationName from tblStation WHERE tblStation.StationCode = ViewCollectionReport.[StationCode]) AS StationName
                //                                 FROM    ViewCollectionReport WHERE EReceipts IS NOT NULL ORDER BY  ViewCollectionReport.AgencyCode ,ViewCollectionReport.StationCode ,ViewCollectionReport.RevenueCode,ViewCollectionReport.EReceipts";
                //                    using (System.Data.DataSet dds = new System.Data.DataSet("DsCollectionReport"))
                //                    {
                //                        using (SqlDataAdapter ada = new SqlDataAdapter(vquery, Logic.ConnectionString))
                //                        {
                //                            ada.Fill(dds, "CollectionReportTable");
                //                            Report.XRepGlobalManifest Global = new Collection.Report.XRepGlobalManifest { DataSource = dds, DataAdapter = ada, DataMember = "CollectionReportTable", RequestParameters = false };
                //                            Global.ShowPreviewDialog();
                //                        }
                //                    }
                //                }




                //Report.XRepGlobalManifest Global = new Collection.Report.XRepGlobalManifest();
                //Global.ShowPreviewDialog();
            }
            else if (sender == navBarItemPending)
            {
                Report.XRepPendTransaction pending = new Collection.Report.XRepPendTransaction();
                pending.ShowPreviewDialog();
            }
            else if (sender == navBarItemReprint)
            {
                tableLayoutPanel2.Controls.Add((new FrmPrintOption().panelContainer), 1, 0);

                FrmPrintOption.publicInstance.Tag = ((sender) as NavBarItem).Tag;

                FrmPrintOption.publicInstance.RefreshForm();
            }
            else if (sender == navBarItemReceiptReceive)
            {
                tableLayoutPanel2.Controls.Add((new FrmReceiveRec().panelContainer), 1, 0);

                FrmReceiveRec.publicInstance.Tag = ((sender) as NavBarItem).Tag;


                FrmReceiveRec.publicInstance.RefreshForm();

            }
            else if (sender == navBarItemReceiptIssue)
            {
                tableLayoutPanel2.Controls.Add((new FrmIssueReceipts().panelContainer), 1, 0);

                //FrmIssueReceipts.publicInstance.Tag = ((sender) as NavBarItem).Tag;

                FrmIssueReceipts.publicStreetGroup.Tag = ((sender) as NavBarItem).Tag;

                //FrmIssueReceipts.publicInstance.RefreshForm();

                FrmIssueReceipts.publicStreetGroup.RefreshForm();
            }
            else if (sender == navBarItemRevenueException)
            {
                tableLayoutPanel2.Controls.Add((new FrmRevException().panelContainer), 1, 0);

                //FrmRevException.publicInstance.Tag = ((sender) as NavBarItem).Tag;

                FrmRevException.publicStreetGroup.RefreshForm();

                FrmRevException.publicStreetGroup.Tag = ((sender) as NavBarItem).Tag;
            }
            else if (sender == navBarItemLogout)
            {
                Program.App_LOGOUT = true;
                Application.Restart();
                Close();
            }
            else if (sender == navBarItemExit)
            {
                Close();
            }
        }

        public void RemoveControls()
        {
            try
            {
                tableLayoutPanel2.Controls.RemoveAt(1);
            }
            finally
            {
                tableLayoutPanel2.Controls.Add(panelContainer, 1, 0);
            }
        }

        private static bool CloseForm()
        {
            bool bRes = false;
#if true
            if (MessageBox.Show("Are you sure you want to Close this application?", Program.ApplicationName, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3) == DialogResult.Yes)
#else

            if (MosesClassLibrary.Utilities.Common.AskQuestion("Are you sure you want to Close this application?", Program.ApplicationName))
#endif
                bRes = true;
            else
                bRes = false;
            return bRes;
        }

        private void panelContainer_Paint(object sender, PaintEventArgs e)
        {

        }

        void Init2()
        {
            var frm = new TaxSmartSuite.Form1();
            i16x16 = frm.i16x16;
            i32x32 = frm.i32x32;
            navBarControl1.LargeImages = frm.i32x32;
            navBarControl1.SmallImages = frm.i16x16;
        }

        void scripts()
        {
            bool isCentralData = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["IsCentralData"]) == 1 ? true : false;

            if (isCentralData)
            {
                //navBarGroup8.Visible = true;

                navBarItemCentreData.Visible = true;

                navBarItemStationData.Visible = false;

                Program.stationCode = "0009";

                Program.stationName = "Central Station";

            }
            else
            {
                //navBarGroup8.Visible = true;

                navBarItemStationData.Visible = true;

                navBarItemCentreData.Visible = false;
            }

            isCentralData = false;
        }

        protected void CheckUpdate()
        {
            var startupPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
            var programPath = System.IO.Path.Combine(startupPath, "twux.exe");
            Logic.LaunchApplication(programPath, string.Format(@"/p:{1} {0}", GetUpdateUrl, System.IO.Path.GetFileName(Application.ExecutablePath)));
            //TaxSmartSuite.CommonLibrary.Common.Utils.LaunchApplication(programPath,
            //string.Format(@"/p:{1} {0}", GetUpdateUrl, System.IO.Path.GetFileName(Application.ExecutablePath)));
        }

        private string GetUpdateUrl
        {
            get { return "http://www.icmaservices.com/generalfolder/Receipt/CollectionSetup.txt"; }
        }

        void alertControl_AlertClick(object sender, AlertClickEventArgs e)
        {
            CheckUpdate();
        }

        private void AutoCheckUpdate()
        {
            //AppAutoUpdate.GetUpdate(GetUpdateUrl).ContinueWith(task =>
            //{
            //    var updateReturnMsg = task.Result;
            //    if (updateReturnMsg.Status)
            //    {
            //        BeginInvoke(new MethodInvoker(delegate ()
            //        {
            //            var msg = string.Format("<b>{0}</b>\n<i>{1}</i>", updateReturnMsg.AppName,
            //                updateReturnMsg.Version);
            //            AlertInfo info = new AlertInfo("<size=14><b><u>Update Available</u></b></size>", msg);
            //            AlertControl alertControl = new AlertControl { AllowHtmlText = true, AllowHotTrack = true };
            //            alertControl.AlertClick += alertControl_AlertClick;
            //            alertControl.Show(this, info);
            //        }));
            //    }
            //});

            //AppAutoUpdate.GetUpdate(GetUpdateUrl).ContinueWith(task =>
            //{



            //    var updateReturnMsg = task.Result;
            //    if (updateReturnMsg.Status)
            //    {
            //        BeginInvoke(new MethodInvoker(delegate ()
            //        {
            //            var msg = string.Format("<b>{0}</b>\n<i>{1}</i>", updateReturnMsg.AppName,
            //                updateReturnMsg.Version);
            //            AlertInfo info = new AlertInfo("<size=14><b><u>Update Available</u></b></size>", msg);
            //            AlertControl alertControl = new AlertControl { AllowHtmlText = true, AllowHotTrack = true };
            //            alertControl.AlertClick += alertControl_AlertClick;
            //            alertControl.Show(this, info);
            //        }));
            //    }
            //});
        }

        //private void AutoCheckUpdate()
        //{
        //    AppAutoUpdate.GetUpdate(GetUpdateUrl).ContinueWith(task =>
        //    {
        //        var updateReturnMsg = task.Result;
        //        if (updateReturnMsg.Status)
        //        {
        //            BeginInvoke(new MethodInvoker(delegate ()
        //            {
        //                var msg = string.Format("<b>{0}</b>\n<i>{1}</i>", updateReturnMsg.AppName,
        //                    updateReturnMsg.Version);
        //                AlertInfo info = new AlertInfo("<size=14><b><u>Update Available</u></b></size>", msg);
        //                AlertControl alertControl = new AlertControl { AllowHtmlText = true, AllowHotTrack = true };
        //                alertControl.AlertClick += alertControl_AlertClick;
        //                alertControl.Show(this, info);
        //            }));
        //        }
        //    });
        //}
    }
}
