using Collection.Classes;
using Collection.Classess;
using Collection.ReceiptServices;
using Collection.Report;
using Collections;
using DevExpress.Utils;
using DevExpress.XtraGrid.Selection;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraReports.UI;
using DevExpress.XtraSplashScreen;
using LinqToExcel;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using TaxSmartSuite.Class;


namespace Collection.Forms
{
    public partial class FrmSplit : Form
    {
        private SqlCommand _command;

        public static FrmSplit publicInstance;

        private DataTable dt = new DataTable();

        protected TransactionTypeCode iTransType;

        GridCheckMarksSelection selection;

        private SqlDataAdapter adp;

        private SqlCommand command;

        bool isFirst = true;

        bool isSecond = true;

        bool isFirstGrid = true;

        bool IsAgent = true;

        DataTable dts = new DataTable();

        DataTable dtRev = new DataTable();

        DataTable temTable = new DataTable();

        private string user; string sheetName = "Sheet1";

        private double dbAmout; string filenamesopen = String.Empty;

        string strPayRef, strUTIN, strAgencycode, strAgencyName, strerecipt;

        DateTime dtstart, dtend; ExcelQueryFactory excel = null;

        int jk = 0;

        string BatchNumber, values, values1, query, criteria3, criteria2;

        private string strCollectionReportID;

        AmountToWords amounttowords = new AmountToWords();

        System.Data.DataSet dataSet = new System.Data.DataSet();
        System.Data.DataSet dstretval = new System.Data.DataSet();

        System.Data.DataSet dsRev = new System.Data.DataSet();
        System.Data.DataSet dstg = new System.Data.DataSet();

        public FrmSplit()
        {
            InitializeComponent();

            publicInstance = this;

            Load += OnFormLoad;

            iTransType = TransactionTypeCode.New;

            btnSearch.Click += btnSearch_Click;

            setImages();

            ToolStripEvent();

            OnFormLoad(null, null);

            dtRev.Columns.Add("SN", typeof(string));
            dtRev.Columns.Add("UTIN", typeof(string));
            dtRev.Columns.Add("Payer Name", typeof(string));
            dtRev.Columns.Add("Revenue Code", typeof(string));
            dtRev.Columns.Add("Revenue Name", typeof(string));
            dtRev.Columns.Add("Agency Code", typeof(string));
            dtRev.Columns.Add("Agency Name", typeof(string));
            dtRev.Columns.Add("Amount", typeof(double));
            dtRev.Columns.Add("Address", typeof(string));

            temTable.Columns.Add("EReceipt", typeof(string));
            temTable.Columns.Add("PaymentRefNumber", typeof(string));
            temTable.Columns.Add("UserId", typeof(string));

            if (Program.UserID == "" || Program.UserID == null)
            {
                user = "Femi";
            }
            else
            {
                user = Program.UserID;
            }
            btnupload.Click += Btnupload_Click;

        }

        void cboAgent_SelectedIndexChanged(object sender, EventArgs e)
        {
            System.Data.DataSet dataSet = new System.Data.DataSet();

            if (cboAgent.SelectedValue != null && !isFirst)
            {
                if (cboAgent.SelectedIndex >= 0)
                {
                    try
                    {
                        SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

                        switch (Program.intCode)
                        {
                            case 13://Akwa Ibom state

                                using (var receiptAka = new AkwaIbomReceiptServices.ReceiptService())
                                {
                                    dataSet = receiptAka.getPayerInfo((string)cboAgent.SelectedValue, null, "ASSC");
                                }
                                break;

                            case 32://kogi state
                                break;
                            case 20:
                                using (var receiptsserv = new DeltaBir.ReceiptService())
                                {
                                    dataSet = receiptsserv.getPayerInfo((string)cboAgent.SelectedValue, null, "ASSC");
                                }
                                break;

                            case 37://ogun state

                                using (var receiptsserv = new ReceiptService())
                                {
                                    dataSet = receiptsserv.getPayerInfo((string)cboAgent.SelectedValue, null, "ASSC");
                                }

                                break;

                            case 40://oyo state

                                using (var receiptsServices = new OyoReceiptServices.ReceiptService())
                                {
                                    dataSet = receiptsServices.getPayerInfo((string)cboAgent.SelectedValue, null, "ASSC");
                                }


                                break;

                            default:
                                break;
                        }

                        if (dataSet.Tables[0].Rows[0]["returnCode"].ToString() == "00")
                        {
                            setReloadGridview(dataSet);
                        }
                        else if (dataSet.Tables[0].Rows[0]["returnCode"].ToString() == "-1")
                        {
                            Common.setMessageBox(string.Format("{0}...", dataSet.Tables[0].Rows[0]["returnMessage"]), Program.ApplicationName, 3);
                            return;
                        }
                        else
                        {
                            Common.setMessageBox(string.Format("{0}...Encounter Error During Record Search ", dataSet.Tables[0].Rows[0]["returnMessage"]), Program.ApplicationName, 3);
                            return;
                        }
                    }
                    catch (Exception ex)
                    {

                        Common.setMessageBox(string.Format("{0}...Encounter Error During Record Search ", String.Format("{0}...{1}", ex.StackTrace, ex.Message)), Program.ApplicationName, 3);
                        return;
                    }
                    finally
                    {
                        SplashScreenManager.CloseForm(false);
                    }
                }
            }

        }

        void cboRevenue_KeyPress(object sender, KeyPressEventArgs e)
        {
            Methods.AutoComplete(cboRevenue, e, true);
        }

        void radioGroup3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtsearchpay.Text))
            {
                Common.setEmptyField("Payment Reference ", Program.ApplicationName); return;
            }
            else
            {
                if (radioGroup3.EditValue.ToString() == "1")
                {
                    groupBox3.Visible = false;
                    groupBox2.Visible = true;
                    groupBox2.Location = new Point(8, 147);
                    label18.Visible = false;
                    btnupload.Visible = false;
                    //txtsearchcriteria.Focus();
                    clearlist();
                    Logic.ClearFormControl(this);
                }
                if (radioGroup3.EditValue.ToString() == "2")
                {
                    groupBox3.Visible = true;
                    groupBox2.Visible = false;
                    label18.Visible = true;
                    btnupload.Visible = true;
                    groupBox3.Location = new Point(8, 147);
                    cboRevenue.Focus(); clearlist();
                    Logic.ClearFormControl(this);
                }
            }
        }

        void clearlist()
        {
            cboRevenue.SelectedIndex = -1; txtAmount.Text = string.Empty; txtpayername.Text = string.Empty;

            cboAgent.SelectedIndex = -1; txtaddress.Text = string.Empty;
        }

        void btnSearch_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtsearchpay.Text))
            {
                Common.setEmptyField("Payment Reference ", Program.ApplicationName); return;
            }
            else if ((Convert.ToInt32(Program.stateCode) == 20) || (Convert.ToInt32(Program.stateCode)) == 40)
            {

                using (var ds = new System.Data.DataSet())
                {

                    using (SqlDataAdapter ada = new SqlDataAdapter(String.Format("SELECT PaymentRefNumber FROM Receipt.tblSplitCollection WHERE PaymentRefNumber = '{0}'", txtsearchpay.Text), Logic.ConnectionString))
                    {
                        ada.SelectCommand.CommandTimeout = 0;

                        ada.Fill(ds, "table");
                    }
                    dt = ds.Tables[0];
                }

                if (dt != null && dt.Rows.Count > 0)
                {
                    Common.setMessageBox("Record has been Splited", Program.ApplicationName, 1);
                    txtsearchpay.Text = string.Empty;
                    return;
                }
                else
                {
                    using (var ds = new System.Data.DataSet())
                    {

                        using (SqlDataAdapter ada = new SqlDataAdapter(String.Format("SELECT Provider, AgencyName, AgencyCode, BankCode, BankName, BranchCode,BranchName, Channel, Collection.tblCollectionReport.PaymentRefNumber, DepositSlipNumber, PaymentDate, PayerID, PayerName, Amount, PaymentMethod, ChequeNumber,ChequeValueDate, ChequeStatus, DateChequeReturned, TelephoneNumber, ReceiptNo, ReceiptDate, PayerAddress, RevenueCode,  Description, ChequeBankCode, ChequeBankName, ZoneCode, ZoneName, Username, AmountWords,CASE WHEN EReceipts IS NULL THEN ReceiptNo ELSE  Collection.tblCollectionReport.EReceipts END EReceipts, CASE WHEN EReceiptsDate IS NULL THEN PaymentDate else Collection.tblCollectionReport.EReceiptsDate END EReceiptsDate,Collection.tblCollectionReport.StationCode FROM Collection.tblCollectionReport WHERE Collection.tblCollectionReport.PaymentRefNumber = '{0}'  ORDER BY Collection.tblCollectionReport.StationCode , Collection.tblCollectionReport.AgencyCode ,Collection.tblCollectionReport.RevenueCode,Collection.tblCollectionReport.EReceipts", txtsearchpay.Text), Logic.ConnectionString))
                        {
                            ada.SelectCommand.CommandTimeout = 0;

                            ada.Fill(ds, "table");
                        }
                        dt = ds.Tables[0];

                        //rw["BALANCE"] = row["BALANCE"] is DBNull ? 0m : Convert.ToDecimal(row["BALANCE"]);
                        //int? vasl = row["BSId"] == DBNull.Value ? (int?)null : Convert.ToInt32(row["BSId"]);

                        if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                        {
                            gridControl1.DataSource = ds.Tables[0];
                            dbAmout = Convert.ToDouble(dt.Rows[0]["Amount"]);
                            strPayRef = dt.Rows[0]["PaymentRefNumber"].ToString();
                            strerecipt = dt.Rows[0]["EReceipts"].ToString();
                            strUTIN = string.IsNullOrWhiteSpace(dt.Rows[0]["PayerID"].ToString())
                                ? "N/A" : dt.Rows[0]["PayerID"].ToString();
                            //? dt.Rows[0]["PaymentRefNumber"].ToString()
                            //: dt.Rows[0]["PayerID"].ToString();
                            dtstart = Convert.ToDateTime(dt.Rows[0]["EReceiptsDate"]);

                            gridView2.BestFitColumns();
                            layoutView1.Columns["Amount"].DisplayFormat.FormatType = FormatType.Numeric;
                            layoutView1.Columns["Amount"].DisplayFormat.FormatString = "n2";
                        }
                        else
                        {
                            Common.setMessageBox("Record not found ", Program.ApplicationName, 1);
                            txtsearchpay.Text = string.Empty;
                            return;
                        }

                    }


                }

            }
            else if (Convert.ToInt32(Program.stateCode) == 13)
            {
                using (var ds = new System.Data.DataSet())
                {

                    using (SqlDataAdapter ada = new SqlDataAdapter(String.Format("SELECT PaymentRefNumber FROM Receipt.tblSplitCollection WHERE PaymentRefNumber = '{0}'", txtsearchpay.Text), Logic.ConnectionString))
                    {
                        ada.SelectCommand.CommandTimeout = 0;

                        ada.Fill(ds, "table");
                    }
                    dt = ds.Tables[0];
                }
                if (dt != null && dt.Rows.Count > 0)
                {
                    Common.setMessageBox("Record has been Splited", Program.ApplicationName, 1);
                    txtsearchpay.Text = string.Empty;
                    return;
                }
                else
                {

                    using (var ds = new System.Data.DataSet())
                    {

                        using (SqlDataAdapter ada = new SqlDataAdapter(String.Format("SELECT Provider, AgencyName, AgencyCode, BankCode, BankName, BranchCode,BranchName, Channel, Collection.tblCollectionReport.PaymentRefNumber, DepositSlipNumber, PaymentDate, PayerID, PayerName, Amount, PaymentMethod, ChequeNumber,ChequeValueDate, ChequeStatus, DateChequeReturned, TelephoneNumber, ReceiptNo, ReceiptDate, PayerAddress, RevenueCode,  Description, ChequeBankCode, ChequeBankName, ZoneCode, ZoneName, Username, AmountWords, Collection.tblCollectionReport.EReceipts, Collection.tblCollectionReport.EReceiptsDate,   Receipt.tblCollectionReceipt.ControlNumber, Collection.tblCollectionReport.StationCode FROM Collection.tblCollectionReport LEFT JOIN Receipt.tblCollectionReceipt ON Collection.tblCollectionReport.PaymentRefNumber = Receipt.tblCollectionReceipt.PaymentRefNumber WHERE Collection.tblCollectionReport.PaymentRefNumber = '{0}' ORDER BY Collection.tblCollectionReport.StationCode , Collection.tblCollectionReport.AgencyCode ,Collection.tblCollectionReport.RevenueCode,Collection.tblCollectionReport.EReceipts", txtsearchpay.Text), Logic.ConnectionString))
                        {
                            ada.SelectCommand.CommandTimeout = 0;

                            ada.Fill(ds, "table");
                        }
                        dt = ds.Tables[0];

                        if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                        {
                            gridControl1.DataSource = ds.Tables[0];
                            dbAmout = Convert.ToDouble(dt.Rows[0]["Amount"]);
                            strPayRef = dt.Rows[0]["PaymentRefNumber"].ToString();
                            //strUTIN = dt.Rows[0]["PayerID"] == DBNull.Value
                            //    ? dt.Rows[0]["PaymentRefNumber"].ToString()
                            //    : dt.Rows[0]["PayerID"].ToString();
                            strUTIN = string.IsNullOrWhiteSpace(dt.Rows[0]["PayerID"].ToString())
                                ? "N/A" : dt.Rows[0]["PayerID"].ToString();
                            strerecipt = dt.Rows[0]["EReceipts"].ToString();
                            dtstart = Convert.ToDateTime(dt.Rows[0]["EReceiptsDate"]);

                            gridView2.BestFitColumns();
                            layoutView1.Columns["Amount"].DisplayFormat.FormatType = FormatType.Numeric;
                            layoutView1.Columns["Amount"].DisplayFormat.FormatString = "n2";
                        }
                        else
                        {
                            Common.setMessageBox("Record not found", Program.ApplicationName, 1);
                            txtsearchpay.Text = string.Empty;
                            return;
                        }
                    }
                }


            }
            else
            {
                using (var ds = new System.Data.DataSet())
                {

                    using (SqlDataAdapter ada = new SqlDataAdapter(String.Format("SELECT PaymentRefNumber FROM Receipt.tblSplitCollection WHERE PaymentRefNumber = '{0}'", txtsearchpay.Text), Logic.ConnectionString))
                    {
                        ada.SelectCommand.CommandTimeout = 0;

                        ada.Fill(ds, "table");
                    }
                    dt = ds.Tables[0];
                }
                if (dt != null && dt.Rows.Count > 0)
                {
                    Common.setMessageBox("Record has been Splited", Program.ApplicationName, 1);
                    txtsearchpay.Text = string.Empty;
                    return;
                }
                else
                {

                    using (var ds = new System.Data.DataSet())
                    {

                        using (SqlDataAdapter ada = new SqlDataAdapter(String.Format("SELECT   Provider, AgencyName, AgencyCode, BankCode, BankName, BranchCode,BranchName, Channel, Collection.tblCollectionReport.PaymentRefNumber, DepositSlipNumber, PaymentDate, PayerID, PayerName, Amount, PaymentMethod, ChequeNumber,ChequeValueDate, ChequeStatus, DateChequeReturned, TelephoneNumber, ReceiptNo, ReceiptDate, PayerAddress, RevenueCode,  Description, ChequeBankCode, ChequeBankName, ZoneCode, ZoneName, Username, AmountWords, Collection.tblCollectionReport.EReceipts, Collection.tblCollectionReport.EReceiptsDate,   Receipt.tblCollectionReceipt.ControlNumber, Collection.tblCollectionReport.StationCode FROM Collection.tblCollectionReport LEFT JOIN Receipt.tblCollectionReceipt ON Collection.tblCollectionReport.PaymentRefNumber = Receipt.tblCollectionReceipt.PaymentRefNumber WHERE Collection.tblCollectionReport.PaymentRefNumber = '{0}' AND Collection.tblCollectionReport.PaymentRefNumber IN (SELECT PaymentRefNumber FROM Receipt.tblCollectionReceipt) ORDER BY Collection.tblCollectionReport.StationCode , Collection.tblCollectionReport.AgencyCode ,Collection.tblCollectionReport.RevenueCode,Collection.tblCollectionReport.EReceipts", txtsearchpay.Text), Logic.ConnectionString))
                        {
                            ada.SelectCommand.CommandTimeout = 0;

                            ada.Fill(ds, "table");
                        }
                        dt = ds.Tables[0];

                        if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                        {
                            gridControl1.DataSource = ds.Tables[0];
                            dbAmout = Convert.ToDouble(dt.Rows[0]["Amount"]);
                            strPayRef = dt.Rows[0]["PaymentRefNumber"].ToString();
                            //strUTIN = dt.Rows[0]["PayerID"] == DBNull.Value
                            //    ? dt.Rows[0]["PaymentRefNumber"].ToString()
                            //    : dt.Rows[0]["PayerID"].ToString();
                            strUTIN = string.IsNullOrWhiteSpace(dt.Rows[0]["PayerID"].ToString())
                                ? "N/A" : dt.Rows[0]["PayerID"].ToString();
                            strerecipt = dt.Rows[0]["EReceipts"].ToString();
                            gridView2.BestFitColumns();
                            layoutView1.Columns["Amount"].DisplayFormat.FormatType = FormatType.Numeric;
                            layoutView1.Columns["Amount"].DisplayFormat.FormatString = "n2";
                        }
                        else
                        {
                            Common.setMessageBox("Record not found", Program.ApplicationName, 1);
                            txtsearchpay.Text = string.Empty;
                            return;
                        }
                    }
                }

            }

        }

        public void RefreshForm()
        {
            NavBars.ToolStripEnableDisableControls(toolStrip, Tag as String);
        }

        void setDBComboBoxReveneu()
        {
            DataTable Dt;

            using (var ds = new System.Data.DataSet())
            {
                using (SqlDataAdapter ada = new SqlDataAdapter(string.Format("SELECT LTRIM(RTRIM(RevenueCode)) AS RevenueCode, LTRIM(RTRIM(Description))  AS Description,AgencyCode FROM Collection.tblRevenueType WHERE AgencyCode ='{0}' ORDER BY  LTRIM(RTRIM(Description))", cboAgency.SelectedValue), Logic.ConnectionString))
                {
                    ada.SelectCommand.CommandTimeout = 0;

                    ada.Fill(ds, "table");
                }

                Dt = ds.Tables[0];
            }

            Common.setComboList(cboRevenue, Dt, "RevenueCode", "Description");

            cboRevenue.SelectedIndex = -1;
        }

        void setDBComboAgency()
        {
            DataTable Dt;

            using (var ds = new System.Data.DataSet())
            {
                using (SqlDataAdapter ada = new SqlDataAdapter((string)"SELECT AgencyName,AgencyCode FROM Registration.tblAgency WHERE LEN(AgencyName)>1 ORDER BY AgencyName", Logic.ConnectionString))
                {
                    ada.SelectCommand.CommandTimeout = 0;

                    ada.Fill(ds, "table");
                }

                Dt = ds.Tables[0];
            }

            Common.setComboList(cboAgency, Dt, "AgencyCode", "AgencyName");

            cboAgency.SelectedIndex = -1;
        }
        void OnFormLoad(object sender, EventArgs e)
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

                ShowForm();

                isFirst = false;

                isSecond = false;

                //setDBComboBoxReveneu();

                setDBComboAgency();

                radioGroup3.SelectedIndexChanged += radioGroup3_SelectedIndexChanged;

                cboRevenue.KeyPress += cboRevenue_KeyPress;

                cboAgent.SelectedIndexChanged += cboAgent_SelectedIndexChanged;

                cboRevenue.SelectedIndexChanged += cboRevenue_SelectedIndexChanged;

                cboAgency.SelectedIndexChanged += cboAgency_SelectedIndexChanged;

                gridView4.ValidatingEditor += gridView4_ValidatingEditor;

                gridView3.DoubleClick += gridView3_DoubleClick;

                cboAgent.KeyPress += cboAgent_KeyPress;

                btnlist.Click += btnlist_Click;

                btnUpdate.Click += btnUpdate_Click;

                btnPrint.Click += btnPrint_Click;

                btnMain.Click += btnMain_Click;

                txtpayername.Leave += txtpayername_Leave;

                txtaddress.Leave += Txtaddress_Leave;

                txtAmount.Leave += txtAmount_Leave;

                btnsearchgroup.Click += btnsearchgroup_Click;

                Checkrecord();
            }
            finally
            {
                SplashScreenManager.CloseForm(false);
            }

        }

        private void Txtaddress_Leave(object sender, EventArgs e)
        {
            txtaddress.Text = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToUpper(txtaddress.Text);
        }

        void cboAgency_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboAgency.SelectedValue != null && !isFirst)

            //if ((!string.IsNullOrEmpty(cboAgency.SelectedValue.ToString()) || cboAgency.SelectedValue.ToString() != null))
            {
                setDBComboBoxReveneu();
            }
        }

        void btnMain_Click(object sender, EventArgs e)
        {
            if (selection.SelectedCount == 0 || string.IsNullOrEmpty(values))
            {

                Common.setMessageBox("No Selection Made for Printing of Receipts", Program.ApplicationName, 3);
                return;

            }
            else
            {
                using (FrmMainFest frmMainFest = new FrmMainFest(strCollectionReportID, values, criteria2, false) { IDList = strCollectionReportID })
                {
                    frmMainFest.ShowDialog();
                }

            }
            //gridControl5.Enabled = false;
        }

        void btnPrint_Click(object sender, EventArgs e)
        {
            //if (selection.SelectedCount == 0)
            //{

            //    Common.setMessageBox("No Selection Made for Printing of Receipts", Program.ApplicationName, 3);
            //    return;

            //}
            //else
            //{
            GetPayRef();

            //}



        }

        void GetPayRef()
        {
            string strname = string.Empty;
            lblSelect.Text = string.Empty;
            strname = "composite receipt";
            int j = 0;

            temTable.Clear();

            if (selection.SelectedCount == 0)
            {

                Common.setMessageBox("No Selection Made for Printing of Receipts", Program.ApplicationName, 3);
                return;

            }
            else
            {

                for (int i = 0; i < selection.SelectedCount; i++)
                {
                    temTable.Rows.Add(new object[] { String.Format("{0}", (selection.GetSelectedRow(i) as DataRowView)["EReceipts"]), String.Format("{0}", (selection.GetSelectedRow(i) as DataRowView)["PaymentRefNumber"]), Program.UserID });

                    values += String.Format("'{0}'", (selection.GetSelectedRow(i) as DataRowView)["PaymentRefNumber"]);
                    if (j + 1 < selection.SelectedCount)
                        values += ",";
                    ++j;

                }

                using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
                {
                    connect.Open();
                    _command = new SqlCommand("InserttblReceipt", connect) { CommandType = CommandType.StoredProcedure };
                    _command.Parameters.Add(new SqlParameter("@tblCollectionReport_Temp", SqlDbType.Structured)).Value = temTable;
                    _command.Parameters.Add(new SqlParameter("@type", SqlDbType.VarChar)).Value = "New";
                    _command.Parameters.Add(new SqlParameter("@userid", SqlDbType.VarChar)).Value = Program.UserID;
                    _command.CommandTimeout = 0;
                    System.Data.DataSet response = new System.Data.DataSet();
                    response.Clear();
                    adp = new SqlDataAdapter(_command);
                    adp.Fill(response);

                    connect.Close();

                    if (String.Compare(response.Tables[0].Rows[0]["returnCode"].ToString(), "00", false) == 0)
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
                                        string strFormat = null;

                                        query = string.Format("SELECT [ID],[Provider] ,[Channel] ,[PaymentRefNumber] , [DepositSlipNumber] , CONVERT(VARCHAR,CONVERT(DATE,[PaymentDate])) AS PaymentDate,[PayerID] , UPPER([PayerName]) AS [PayerName], [Amount] ,[PaymentMethod] ,[ChequeNumber] ,[ChequeValueDate] ,[ChequeStatus] ,[DateChequeReturned] ,[TelephoneNumber] ,[ReceiptNo] ,[ReceiptDate] ,UPPER([PayerAddress]) as [PayerAddress] ,[User] ,[RevenueCode] ,tblCollectionReport.Description , [ChequeBankCode] ,[ChequeBankName] ,[AgencyName] ,[AgencyCode] ,[BankCode] ,[BankName] ,[BranchCode] ,[BranchName] ,[ZoneCode] ,[ZoneName] ,[Username] ,[AmountWords] ,[EReceipts] ,[EReceiptsDate] ,[GeneratedBy] ,[StationCode] ,(SELECT StationName FROM Receipt.tblStation  WHERE tblStation.StationCode = Collection.tblCollectionReport.[StationCode]) AS StationName , Symbol , Surfix , tblCurrency.Description AS prefix from Collection.tblCollectionReport  INNER JOIN Reconciliation.tblCurrency ON tblCurrency.CurrencyCode = tblCollectionReport.CurrencyCode WHERE PaymentRefNumber IN ({0})  ORDER BY Collection.tblCollectionReport.StationCode , Collection.tblCollectionReport.AgencyCode ,Collection.tblCollectionReport.RevenueCode,Collection.tblCollectionReport.EReceipts", values);

                                        DataTable Dt = dds.Tables.Add("CollectionReportTable");
                                        ada = new SqlDataAdapter(query, Logic.ConnectionString);
                                        ada.SelectCommand.CommandTimeout = 0;
                                        ada.Fill(dds, "CollectionReportTable");
                                        Logic.ProcessDataTable(Dt); ;
                                        //strCollectionReportID = strFormat;
                                    }

                                    //int Program.intCode = Convert.ToInt32(Program.stateCode);

                                    switch (Program.intCode)
                                    {
                                        case 13://Akwa Ibom state

                                            XtraRepReceiptAkwa akwa = new XtraRepReceiptAkwa { DataSource = dds.Tables[0] /*recportRec.DataAdapter = ada;*/, DataMember = "CollectionReportTable" };

                                            akwa.ShowPreviewDialog();
                                            break;

                                        case 20://detla state

                                            XtraRepReceiptDelta delta = new XtraRepReceiptDelta { DataSource = dds.Tables[0], /*recportRec.DataAdapter = ada;*/ DataMember = "CollectionReportTable", /*delta.PrintingSystem.StartPrint += PrintingSystem_StartPrint;*/ /*delta.xrLabel3.Text = strname.ToUpper();*/ /*delta.xrLabel3.Visible = true;*/logoPath = Logic.singaturepth, Imagepath = Logic.logopth };
                                            //report.Watermark.Text = "DUPLICATE";
                                            delta.xrLabel3.Text = "Mermorandum Receipt";
                                            delta.xrLabel2.Visible = false; delta.xrLabel20.Visible = false; delta.xrLabel21.Visible = false;

                                            //delta.xrLabel3.Text = "Memorandum ";
                                            delta.xrLabel3.Visible = true;
                                            delta.xrLabel13.Visible = true; delta.xrLabel13.Text = string.Format("{0} STATE GOVERNMENT", Program.StateName.ToUpper());

                                            delta.ShowPreviewDialog();
                                            //delta.Print();
                                            //XtraRepCustozime custozime = new XtraRepCustozime { DataSource = dds.Tables[0] /*recportRec.DataAdapter = ada;*/, DataMember = "CollectionReportTable" };
                                            //custozime.ShowPreviewDialog();

                                            break;
                                        //case 37://ogun state
                                        //    XRepReceipt recportRec = new XRepReceipt { DataSource = dds.Tables[0] /*recportRec.DataAdapter = ada;*/, DataMember = "CollectionReportTable" };

                                        //    recportRec.ShowPreviewDialog();
                                        //    break;

                                        case 37://ogun state
                                            XRepReceipt recportRec = new XRepReceipt { DataSource = dds.Tables[0] /*recportRec.DataAdapter = ada;*/, DataMember = "CollectionReportTable" };
                                            recportRec.xrLabel52.Text = strname.ToUpper();
                                            recportRec.xrLabel53.Text = strname.ToUpper();
                                            recportRec.xrLabel53.Visible = true;
                                            recportRec.xrLabel52.Visible = true; recportRec.logoPath = Logic.singaturepth;
                                            recportRec.ShowPreviewDialog();
                                            break;

                                        case 40://oyo state
                                            XRepReceiptoyo recportRecs = new XRepReceiptoyo { DataSource = dds.Tables[0] /*recportRec.DataAdapter = ada;*/, DataMember = "CollectionReportTable" };
                                            recportRecs.xrLabel46.Text = strname.ToUpper();
                                            recportRecs.xrLabel47.Text = strname.ToUpper();
                                            recportRecs.xrLabel47.Visible = true;
                                            recportRecs.xrLabel46.Visible = true;
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

                                //asking if receipt
                                //ask if the print was sucessfull
                                DialogResult result = MessageBox.Show(" Is Receipt Printing Successful ?", Program.ApplicationName, MessageBoxButtons.YesNo);

                                if (result == DialogResult.Yes)
                                {
                                    //update the collection table by seting the isprinted to true
                                    using (SqlConnection dbs = new SqlConnection(Logic.ConnectionString))
                                    {
                                        SqlTransaction transactions;

                                        dbs.Open();

                                        transactions = dbs.BeginTransaction();

                                        try
                                        {
                                            //string query1 = String.Format("UPDATE tblCollectionReport SET isPrinted=1,IsPrintedDate= '{0}' WHERE [PaymentRefNumber] IN ({1})", DateTime.Now, criteria3);

                                            string query1 = String.Format("UPDATE Receipt.tblCollectionReceipt SET isPrinted=1,IsPrintedDate= '{0:MM/dd/yyyy HH:mm:ss tt}',PrintedBY='{1}',StationCode='{2}' WHERE PaymentRefNumber IN ({3})", DateTime.Now, Program.UserID, Program.stationCode, values);

                                            using (SqlCommand sqlCommand = new SqlCommand(query1, dbs, transactions))
                                            {
                                                sqlCommand.ExecuteNonQuery();
                                            }

                                        }
                                        catch (Exception ex)
                                        {
                                            transactions.Rollback();
                                            Tripous.Sys.ErrorBox(ex);
                                            return;
                                        }
                                        transactions.Commit();

                                        dbs.Close();
                                    }
                                }
                                else
                                {
                                    return;
                                }

                            }
                            catch (SqlException sqlError)
                            {
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

                            DialogResult result = MessageBox.Show("Do you wish to continue to Print this Receipts?", Program.ApplicationName, MessageBoxButtons.YesNo);

                            if (result == DialogResult.Yes)
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

                                            query = string.Format("SELECT [ID] , [Provider] , [Channel] ,[PaymentRefNumber] , [DepositSlipNumber] , CONVERT(VARCHAR,CONVERT(DATE,[PaymentDate])) AS PaymentDate,[PayerID] , UPPER([PayerName]) AS [PayerName], [Amount] ,[PaymentMethod] ,[ChequeNumber] ,[ChequeValueDate] ,[ChequeStatus] ,[DateChequeReturned] ,[TelephoneNumber] ,[ReceiptNo] ,[ReceiptDate] ,UPPER([PayerAddress]) as [PayerAddress] ,[User] ,[RevenueCode] ,tblCollectionReport.Description , [ChequeBankCode] ,[ChequeBankName] ,[AgencyName] ,[AgencyCode] ,[BankCode] ,[BankName] ,[BranchCode] ,[BranchName] ,[ZoneCode] ,[ZoneName] ,[Username] ,[AmountWords] ,[EReceipts] ,[EReceiptsDate] ,[GeneratedBy] ,[StationCode] ,(SELECT StationName FROM Receipt.tblStation  WHERE tblStation.StationCode = Collection.tblCollectionReport.[StationCode]) AS StationName, Symbol , Surfix , tblCurrency.Description AS prefix from Collection.tblCollectionReport INNER JOIN Reconciliation.tblCurrency ON tblCurrency.CurrencyCode = tblCollectionReport.CurrencyCode WHERE PaymentRefNumber IN ({0})  ORDER BY Collection.tblCollectionReport.StationCode , Collection.tblCollectionReport.AgencyCode ,Collection.tblCollectionReport.RevenueCode,Collection.tblCollectionReport.EReceipts", values);

                                            DataTable Dt = dds.Tables.Add("CollectionReportTable");
                                            ada = new SqlDataAdapter(query, Logic.ConnectionString);
                                            ada.SelectCommand.CommandTimeout = 0;
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

                                                XtraRepReceiptDelta delta = new XtraRepReceiptDelta { DataSource = dds.Tables[0], /*recportRec.DataAdapter = ada;*/ DataMember = "CollectionReportTable", /*delta.PrintingSystem.StartPrint += PrintingSystem_StartPrint;*/ /*delta.xrLabel3.Text = strname.ToUpper();*/ /*delta.xrLabel3.Visible = true;*/logoPath = Logic.singaturepth, Imagepath = Logic.logopth };
                                                //report.Watermark.Text = "DUPLICATE";
                                                delta.xrLabel3.Text = "Mermorandum Receipt";
                                                delta.xrLabel2.Visible = false; delta.xrLabel20.Visible = false; delta.xrLabel21.Visible = false;

                                                //delta.xrLabel3.Text = "Memorandum ";
                                                delta.xrLabel3.Visible = true;
                                                delta.xrLabel13.Visible = true; delta.xrLabel13.Text = string.Format("{0} STATE GOVERNMENT", Program.StateName.ToUpper());

                                                delta.ShowPreviewDialog();
                                                //delta.Print();
                                                //XtraRepCustozime custozime = new XtraRepCustozime { DataSource = dds.Tables[0] /*recportRec.DataAdapter = ada;*/, DataMember = "CollectionReportTable" };
                                                //custozime.ShowPreviewDialog();

                                                break;
                                            //case 37://ogun state
                                            //    XRepReceipt recportRec = new XRepReceipt { DataSource = dds.Tables[0] /*recportRec.DataAdapter = ada;*/, DataMember = "CollectionReportTable" };

                                            //    recportRec.ShowPreviewDialog();
                                            //    break;

                                            case 37://ogun state
                                                XRepReceipt recportRec = new XRepReceipt { DataSource = dds.Tables[0] /*recportRec.DataAdapter = ada;*/, DataMember = "CollectionReportTable" };
                                                recportRec.xrLabel52.Text = strname.ToUpper();
                                                recportRec.xrLabel53.Text = strname.ToUpper();
                                                recportRec.xrLabel53.Visible = true;
                                                recportRec.xrLabel52.Visible = true; recportRec.logoPath = Logic.singaturepth;
                                                recportRec.ShowPreviewDialog();
                                                break;

                                            case 40://oyo state
                                                XRepReceiptoyo recportRecs = new XRepReceiptoyo { DataSource = dds.Tables[0] /*recportRec.DataAdapter = ada;*/, DataMember = "CollectionReportTable" };
                                                recportRecs.xrLabel46.Text = strname.ToUpper();
                                                recportRecs.xrLabel47.Text = strname.ToUpper();
                                                recportRecs.xrLabel47.Visible = true;
                                                recportRecs.xrLabel46.Visible = true;
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

                                    DialogResult results = MessageBox.Show(" Is Receipt Printing Successful ?", Program.ApplicationName, MessageBoxButtons.YesNo);

                                    if (results == DialogResult.Yes)
                                    {
                                        //update the collection table by seting the isprinted to true
                                        using (SqlConnection dbs = new SqlConnection(Logic.ConnectionString))
                                        {
                                            SqlTransaction transactions;

                                            dbs.Open();

                                            transactions = dbs.BeginTransaction();

                                            try
                                            {
                                                //string query1 = String.Format("UPDATE tblCollectionReport SET isPrinted=1,IsPrintedDate= '{0}' WHERE [PaymentRefNumber] IN ({1})", DateTime.Now, criteria3);

                                                string query1 = String.Format("UPDATE Receipt.tblCollectionReceipt SET isPrinted=1,IsPrintedDate= '{0:MM/dd/yyyy HH:mm:ss tt}',PrintedBY='{1}',StationCode='{2}' WHERE PaymentRefNumber IN ({3})", DateTime.Now, Program.UserID, Program.stationCode, values);

                                                using (SqlCommand sqlCommand = new SqlCommand(query1, dbs, transactions))
                                                {
                                                    sqlCommand.ExecuteNonQuery();
                                                }

                                            }
                                            catch (Exception ex)
                                            {
                                                transactions.Rollback();
                                                Tripous.Sys.ErrorBox(ex);
                                                return;
                                            }
                                            transactions.Commit();

                                            dbs.Close();
                                        }
                                    }
                                    else
                                    {
                                        return;
                                    }
                                }

                            }
                            else
                                return;
                        }
                        else
                        {
                            Common.setMessageBox(response.Tables[0].Rows[0]["returnMessage"].ToString(), "", 2);
                            return;
                        }
                    }
                }

            }
            //return values;
        }

        void btnsearchgroup_Click(object sender, EventArgs e)
        {
            System.Data.DataSet dataSet = new System.Data.DataSet();

            if (radioGroup1.EditValue == null)
            {
                Common.setMessageBox("Please select search type for the group....!", Program.ApplicationName, 2);
                return;
            }
            else if (string.IsNullOrEmpty(txtsearchcriteria.Text))
            {
                Common.setEmptyField("Search Criteria ", Program.ApplicationName);
                txtsearchcriteria.Focus();
                return;
            }
            else
            {
                cboAgent.SelectedIndex = -1;

                try
                {
                    SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

                    if (radioGroup1.EditValue.ToString() == "1")
                    {
                        try
                        {
                            switch (Program.intCode)
                            {
                                case 13://Akawa ibom state
                                    using (var akaw = new AkwaIbomReceiptServices.ReceiptService())
                                    {
                                        dataSet = akaw.getPayerInfo(txtsearchcriteria.Text, null, "ASSC");
                                    }
                                    break;

                                case 32://kogi state
                                    break;

                                case 37://ogun state
                                    using (var receiptsserv = new ReceiptService())
                                    {
                                        dataSet = receiptsserv.getPayerInfo(txtsearchcriteria.Text, null, "ASSC");

                                    }
                                    break;
                                case 20:
                                    using (var receiptsserv = new DeltaBir.ReceiptService())
                                    {
                                        dataSet = receiptsserv.getPayerInfo(txtsearchcriteria.Text, null, "ASSC");
                                    }
                                    break;

                                case 40://oyo state

                                    using (var receiptsServices = new OyoReceiptServices.ReceiptService())
                                    {
                                        dataSet = receiptsServices.getPayerInfo(txtsearchcriteria.Text, null, "ASSC");
                                    }

                                    break;
                                default:
                                    break;
                            }

                            //check return message
                            if (dataSet.Tables[0].Rows[0]["returnCode"].ToString() == "00")
                            {
                                cboAgent.Text = dataSet.Tables[1].Rows[0]["NAME"].ToString();
                                cboAgent.Enabled = false; isFirst = true;
                                setReloadGridview(dataSet); isFirst = false;
                            }
                            else if (dataSet.Tables[0].Rows[0]["returnCode"].ToString() == "-1")
                            {
                                Common.setMessageBox(string.Format("{0}", dataSet.Tables[0].Rows[0]["returnMessage"]), Program.ApplicationName, 3);
                                return;
                            }
                            else
                            {
                                Common.setMessageBox(string.Format("{0}...Encounter Error During Record Search ", dataSet.Tables[0].Rows[0]["returnMessage"]), Program.ApplicationName, 3);
                                return;
                            }

                        }
                        catch (Exception ex)
                        {
                            Common.setMessageBox(string.Format("{0}...Encounter Error During Record Search", ex.StackTrace), Program.ApplicationName, 3);
                            return;
                        }

                    }

                    if (radioGroup1.EditValue.ToString() == "2")
                    {
                        try
                        {
                            switch (Program.intCode)
                            {
                                case 13://Akawa ibom state
                                    using (var akaw = new AkwaIbomReceiptServices.ReceiptService())
                                    {
                                        dataSet = akaw.getPayerInfo(null, txtsearchcriteria.Text, "NAME");
                                    }
                                    break;

                                case 32://kogi state
                                    break;

                                case 20:
                                    using (var receiptsserv = new DeltaBir.ReceiptService())
                                    {
                                        dataSet = receiptsserv.getPayerInfo(null, txtsearchcriteria.Text, "NAME");
                                    }
                                    break;

                                case 37://ogun state

                                    using (var receiptsserv = new ReceiptService())
                                    {
                                        dataSet = receiptsserv.getPayerInfo(null, txtsearchcriteria.Text, "NAME");

                                    }
                                    break;

                                case 40://oyo state

                                    using (var receiptsServices = new OyoReceiptServices.ReceiptService())
                                    {
                                        dataSet = receiptsServices.getPayerInfo(null, txtsearchcriteria.Text, "NAME");
                                    }

                                    break;
                                default:
                                    break;
                            }


                            if (dataSet.Tables[0].Rows[0]["returnCode"].ToString() == "00")
                            {
                                isFirst = true; setDbComboBoxAgent(dataSet); isFirst = false;
                            }
                            else if (dataSet.Tables[0].Rows[0]["returnCode"].ToString() == "-1")
                            {
                                Common.setMessageBox(string.Format("{0}", dataSet.Tables[0].Rows[0]["returnMessage"]), Program.ApplicationName, 3);
                                return;
                            }
                            else
                            {
                                Common.setMessageBox(string.Format("{0}...Encounter Error During Record Search", dataSet.Tables[0].Rows[0]["returnMessage"]), Program.ApplicationName, 3);
                                return;
                            }
                            //}
                        }
                        catch (Exception ex)
                        {
                            Common.setMessageBox(string.Format("{0}...Encounter Error During Record Search", ex.StackTrace), Program.ApplicationName, 3);
                            return;
                        }

                    }
                }
                catch (Exception ex)
                {
                    Tripous.Sys.ErrorBox(ex.StackTrace + ex.Message);
                    return;
                }
                finally
                {
                    SplashScreenManager.CloseForm(false);
                }
            }
        }

        void gridView3_DoubleClick(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(" Are you sure you want to Remove this row ?", Program.ApplicationName, MessageBoxButtons.YesNo);

            if (result == DialogResult.Yes)
            {
                gridView3.DeleteRow(gridView3.FocusedRowHandle);
                var dt = gridControl2.DataSource as DataTable;
            }
            else
                return;
        }

        void gridView4_ValidatingEditor(object sender, DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventArgs e)
        {
            try
            {
                GridView view = sender as GridView;

                if (view != null)
                {
                    if (view.FocusedColumn.FieldName == "Amount")
                    {
                        object obj = e.Value;//get the value of the cell

                        if (obj == null) return;

                        if (!Logic.isDeceimalFormat((string)obj))
                        {
                            e.Valid = false;
                            e.ErrorText = "Amount can only be in number values";
                        }
                        else
                        {
                            //String Text = ((TextBox)sender).Text.Replace(",", "");

                            //double Num;

                            //if (double.TryParse(Text, out Num))
                            //{
                            //    Text = String.Format("{0:N2}", Num);
                            //    ((TextBox)sender).Text = Text;
                            //}
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                Tripous.Sys.ErrorBox(ex.StackTrace + ex.Message);
                return;
            }
        }

        void cboRevenue_SelectedIndexChanged(object sender, EventArgs e)
        {
            //GetAgencyCodeByRevenue
            if (cboRevenue.SelectedValue != null && !isSecond)
            {
                GetAgencyCodeByRevenue(cboRevenue.SelectedValue.ToString());
            }
        }

        void btnUpdate_Click(object sender, EventArgs e)
        {

            try
            {
                if (string.IsNullOrEmpty(txtsearchpay.Text))
                {
                    Common.setEmptyField("Payment Reference ", Program.ApplicationName); return;
                }

                if (radioGroup3.EditValue == null)
                {
                    Common.setMessageBox("Splitting Option/Type not Selected....!", Program.ApplicationName, 2);
                    return;
                }

                if (radioGroup3.EditValue.ToString() == "1")//group splitting
                {
                    //double sumAmt = Convert.ToDouble(gridView4.Columns["Amount"].SummaryItem.SummaryValue);

                    double sumAmt = Convert.ToDouble(string.Format("{0:0,0.00}", gridView4.Columns["Amount"].SummaryItem.SummaryValue));

                    //check amount
                    if (!Equals(dbAmout, sumAmt))
                    {
                        Common.setMessageBox("You have not achieved a balance on this payment Split..", Program.ApplicationName, 2);
                        return;
                    }
                    else
                    {
                        GetUtinpayer();

                        if (dtRev.DataSet == null)
                            dsRev.Tables.Add(dtRev);
                        else
                            dsRev = dtRev.DataSet;

                        if (dsRev.Tables[0].Rows.Count == 0 && dsRev.Tables[0] == null) return;

                        try
                        {
                            SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

                            //int Program.intCode = Convert.ToInt32(Program.stateCode);

                            switch (Program.intCode)
                            {
                                case 13://akwa ibom state
                                    using (var akwa = new AkwaIbomReceiptServices.ReceiptService())
                                    {
                                        dataSet = akwa.SplitCollection(dsRev, strPayRef, strUTIN, "ASC", Program.stationCode);
                                    }
                                    break;
                                case 20://delta state
                                    using (var delta = new DeltaBir.ReceiptService())
                                    {
                                        dataSet = delta.SplitCollection(dsRev, strPayRef, strUTIN, "ASC", Program.stationCode);
                                    }
                                    break;
                                case 32://kogi state
                                    break;

                                case 37://ogun state

                                    using (var receiptsserv = new ReceiptService())
                                    {
                                        dataSet = receiptsserv.SplitCollection(dsRev, strPayRef, strUTIN, "ASC", Program.stationCode);
                                    }

                                    break;

                                case 40://oyo state

                                    using (var receiptsServices = new OyoReceiptServices.ReceiptService())
                                    {
                                        dataSet = receiptsServices.SplitCollection(dsRev, strPayRef, strUTIN, "ASC", Program.stationCode);
                                    }

                                    break;
                                default:
                                    break;
                            }
                        }
                        catch (Exception ex)
                        {
                            Tripous.Sys.ErrorBox(ex.StackTrace + ex.Message);
                            return;
                        }
                        finally
                        {
                            SplashScreenManager.CloseForm(false);
                        }

                        //}
                    }
                }

                if (radioGroup3.EditValue.ToString() == "2")//Payment to revenue splitting
                {
                    double sumAmt = Convert.ToDouble(string.Format("{0:0,0.00}", gridView3.Columns["Amount"].SummaryItem.SummaryValue));

                    //if (dbAmout != sumAmt)
                    if (!Equals(dbAmout, sumAmt))
                    {
                        Common.setMessageBox("You have not achieved a balance on this payment Split..", Program.ApplicationName, 2);
                        return;
                    }
                    else
                    {
                        //using (var ds = new System.Data.DataSet())
                        //{
                        //    using (SqlDataAdapter ada = new SqlDataAdapter((string)string.Format("SELECT F1,NULL AS strUTIN, [NAME OF HOSPITAL],'4020128','Renewals of Specialist Hospitals, Clinics, Laboratories & Mutuaries','41800','MINISTRY OF HEALTH',[AMOUNT PAID], NULL AS Address FROM [dbo].[Sheet1$]"), Logic.ConnectionString))
                        //    {
                        //        ada.SelectCommand.CommandTimeout = 0;

                        //        ada.Fill(ds, "table");
                        //    }

                        //    dtRev = ds.Tables[0];
                        //}

                        //dsRev.Clear();
                        if (dtRev.DataSet == null)
                            dsRev.Tables.Add(dtRev);
                        else
                            dsRev = dtRev.DataSet;
                        //if (dsRev.Tables.Count == 0)
                        //dsRev.Tables.Add(new DataTable());
                        //dsRev.Tables[0] = dtRev;
                        //dsRev.Tables.Add(dtRev);

                        if (dsRev.Tables[0].Rows.Count == 0 && dsRev.Tables[0] == null) return;

                        try
                        {
                            SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

                            //int Program.intCode = Convert.ToInt32(Program.stateCode);

                            switch (Program.intCode)
                            {
                                case 13://Akwa ibom state
                                    using (var akwa = new AkwaIbomReceiptServices.ReceiptService())
                                    {
                                        dataSet = akwa.SplitCollection(dsRev, strPayRef, strUTIN, "REV", Program.stationCode);
                                    }
                                    break;

                                //  using (var receiptDelta = new DeltaBir.ReceiptService())
                                //{
                                //    dataSet = receiptDelta.DownloadData(Logic.GetMacAddress(), Program.stationCode);
                                //}
                                case 20://delta state

                                    using (var delta = new DeltaBir.ReceiptService())
                                    {
                                        dataSet = delta.SplitCollection(dsRev, strPayRef, strUTIN, "REV", Program.stationCode);
                                    }
                                    break;
                                case 32://kogi state
                                    break;
                                case 37://ogun state

                                    using (var receiptsserv = new ReceiptService())
                                    {
                                        dataSet = receiptsserv.SplitCollection(dsRev, strPayRef, strUTIN, "REV", Program.stationCode);
                                    }

                                    break;

                                case 40://oyo state

                                    using (var receiptsServices = new OyoReceiptServices.ReceiptService())
                                    {
                                        dataSet = receiptsServices.SplitCollection(dsRev, strPayRef, strUTIN, "REV", Program.stationCode);
                                    }
                                    //using (var receiptsservices = new TestReceiptServices.ReceiptService())
                                    //{
                                    //    dataSet = receiptsservices.SplitCollection(dsRev, strPayRef, strUTIN, "REV", Program.stationCode);
                                    //}
                                    break;
                                default:
                                    break;
                            }
                        }
                        catch (Exception ex)
                        {
                            Tripous.Sys.ErrorBox(ex.StackTrace + ex.Message);
                            return;
                        }
                        finally
                        {
                            SplashScreenManager.CloseForm(false);
                        }

                    }
                }

                if (dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
                {
                    //test retunr code
                    if (dataSet.Tables[0].Rows[0]["returnCode"].ToString() == "00")
                    {
                        //insert splitting record into tblsplitcollection
                        using (SqlConnection db = new SqlConnection(Logic.ConnectionString))
                        {
                            SqlTransaction transaction;

                            db.Open();
                            //strUTIN
                            transaction = db.BeginTransaction();
                            try
                            {
                                DataTable Dt, Dt2;


                                string sql2 = String.Format(@"SELECT PaymentRefNumber FROM Receipt.tblCollectionReceipt WHERE PaymentRefNumber = '{0}'", strPayRef);

                                Dt2 = (new Logic()).getSqlStatement(sql2).Tables[0];

                                if (Dt2 != null && Dt2.Rows.Count > 0)
                                { }
                                else
                                {
                                    string strquye2 = string.Format("INSERT INTO Receipt.tblCollectionReceipt ( PaymentRefNumber , EReceipts ,EReceiptsDate, StationCode )VALUES('{0}','{1}','{2}','{3}');", strPayRef, strerecipt, DateTime.Today.ToString("yyyy/MM/dd hh:mm:ss")
                                       //DateTime.Today.ToString("yyyy/MM/dd hh:mm:ss")
                                       , Program.stationCode);

                                    using (SqlCommand sqlCommand1 = new SqlCommand(strquye2, db, transaction))
                                    {
                                        sqlCommand1.ExecuteNonQuery();

                                    }

                                    //transaction.Commit();
                                }

                                string SQL = String.Format(@"SELECT PaymentRefNumber FROM Receipt.tblSplitCollection WHERE PaymentRefNumber = '{0}'", strPayRef);

                                Dt = (new Logic()).getSqlStatement(SQL).Tables[0];

                                if (Dt != null && Dt.Rows.Count > 0)
                                {

                                }
                                else
                                {

                                    string qry = String.Format("INSERT INTO Receipt.tblSplitCollection ( PaymentRefNumber , Amount , Utin ,MemberName ,SplitBy , SplitDate) VALUES ('{0}','{1}','{2}','{3}','{4}','{5}');", strPayRef, dbAmout, strUTIN, txtpayername.Text.Trim(), Program.UserID, string.Format("{0:yyyy/MM/dd hh:mm:ss}", DateTime.Now)
                                        );
                                    //DateTime.Today.ToString("yyyy/MM/dd hh:mm:ss")
                                    using (SqlCommand sqlCommand1 = new SqlCommand(qry, db, transaction))
                                    {
                                        sqlCommand1.ExecuteNonQuery();

                                    }

                                    transaction.Commit();
                                }



                            }
                            catch (SqlException sqlError)
                            {
                                Tripous.Sys.ErrorBox(sqlError);
                                transaction.Rollback();
                            }
                            db.Close();
                        }
                        //insert Record into collection table
                        dstretval = InsertData(dataSet);

                        //int Program.intCode = Convert.ToInt32(Program.stateCode);
                        //if (!Dt.Columns.Contains("REVENUECODE")) Dt.Columns.Add("REVENUECODE", typeof(string));
                        if (Program.stateCode.ToString() != "13")
                        {
                            if (!dstretval.Tables[0].Columns.Contains("Address")) dstretval.Tables[0].Columns.Add("Address", typeof(string));
                        }
                        else
                        {
                            dstretval = dstretval;
                        }

                        //if (!dstretval.Tables[0].Columns.Contains("Address")) dstretval.Tables[0].Columns.Add("Address", typeof(string));
                        //dstretval = dstretval;

                        switch (Program.intCode)
                        {
                            case 13://Akwa ibom state
                                using (var akwa = new AkwaIbomReceiptServices.ReceiptService())
                                {
                                    dstg = akwa.UpdateSplitCollection(dstretval);
                                }
                                break;
                            case 20://delta state
                                using (var delta = new DeltaBir.ReceiptService())
                                {
                                    dstg = delta.UpdateSplitCollection(dstretval);
                                }
                                break;
                            case 32://kogi state
                                break;
                            case 37://ogun state

                                using (var receiptsserv = new ReceiptService())
                                {
                                    dstg = receiptsserv.UpdateSplitCollection(dstretval);
                                }

                                break;

                            case 40://oyo state

                                using (var receiptsServices = new OyoReceiptServices.ReceiptService())
                                {
                                    dstg = receiptsServices.UpdateSplitCollection(dstretval);
                                }

                                break;
                            default:
                                break;
                        }


                        if (dstg.Tables[0].Rows[0]["returnCode"].ToString() == "00")
                        {
                            Common.setMessageBox(string.Format("Record Splitting {0},Please proceed to Receipt printing..", dstg.Tables[0].Rows[0]["returnMessage"]), Program.ApplicationName, 1);


                            iTransType = TransactionTypeCode.Edit;

                            //set record lo
                            dt = dataSet.Tables[1];
                            gridControl3.DataSource = dataSet.Tables[1];
                            gridView5.BestFitColumns();
                            gridView5.Columns["Amount"].DisplayFormat.FormatType = FormatType.Numeric;
                            gridView5.Columns["Amount"].DisplayFormat.FormatString = "n2";
                            gridView5.Columns["PaymentDate"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
                            gridView5.Columns["PaymentDate"].DisplayFormat.FormatString = "dd/MM/yyyy";
                            gridView5.Columns["RowNumber"].Visible = false;

                            if (isFirstGrid)
                            {
                                selection = new GridCheckMarksSelection(gridView5, ref lblSelect);
                                selection.CheckMarkColumn.VisibleIndex = 0;
                                isFirstGrid = false;
                            }

                            isFirst = true;
                            //load the form again
                            OnFormLoad(null, null);
                            //return;
                        }
                        else
                            Common.setMessageBox(string.Format("{0}...Records Encounter Error During Update Split Records", dstg.Tables[0].Rows[0]["returnMessage"]), Program.ApplicationName, 3);
                        return;

                    }
                    else
                    {
                        Common.setMessageBox(string.Format("{0}...Encounter Error During Splitting Records", dataSet.Tables[0].Rows[0]["returnMessage"]), Program.ApplicationName, 3);
                        return;
                    }
                }
                else
                {
                    Common.setMessageBox(string.Format("{0}...Error Occured", dataSet.Tables[0].Rows[0]["returnMessage"]), Program.ApplicationName, 3);
                    return;
                }

            }
            catch (Exception ex)
            {
                Tripous.Sys.ErrorBox(ex.StackTrace + ex.Message);
                return;
            }

            //ShowForm();
        }

        void txtAmount_Leave(object sender, EventArgs e)
        {
            if (!Logic.isDeceimalFormat((string)txtAmount.Text))
            {
                Common.setMessageBox("Amount can only be in number values", Program.ApplicationName, 2); txtAmount.Text = string.Empty;
                //txtNumber.Focus(); 
                return;
            }
            else
            {
                String Text = ((TextBox)sender).Text.Replace(",", "");

                double Num;

                if (double.TryParse(Text, out Num))
                {
                    Text = String.Format("{0:N2}", Num);
                    ((TextBox)sender).Text = Text;
                }
            }
        }

        void txtpayername_Leave(object sender, EventArgs e)
        {
            txtpayername.Text = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToUpper(txtpayername.Text);
        }

        void btnlist_Click(object sender, EventArgs e)
        {
            try
            {

                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

                jk = jk + 1;

                dtRev.Rows.Add(new object[] { Convert.ToString(jk), strUTIN, txtpayername.Text, cboRevenue.SelectedValue.ToString().Trim(), cboRevenue.Text.ToString().Trim(), strAgencycode, strAgencyName, Convert.ToDouble(txtAmount.Text), txtaddress.Text.ToString() });

                gridControl2.DataSource = dtRev;
                gridView3.OptionsBehavior.Editable = false;

                //gridView3.Columns["Amount"].OptionsColumn.AllowEdit = true;

                gridView3.Columns["Amount"].DisplayFormat.FormatType = FormatType.Numeric;
                gridView3.Columns["Amount"].DisplayFormat.FormatString = "n2";
                gridView3.Columns["Amount"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                gridView3.Columns["Amount"].SummaryItem.DisplayFormat = "Total = {0:n2}";
                gridView3.Columns["Revenue Code"].Visible = false;
                gridView3.Columns["Revenue Name"].Visible = false;
                gridView3.Columns["Agency Code"].Visible = false;
                gridView3.Columns["Agency Name"].Visible = false;
                gridView3.Columns["SN"].Visible = false;
                gridView3.Columns["UTIN"].Visible = false;
                gridView3.BestFitColumns();

                clearlist();
                cboRevenue.Focus();
            }
            catch (Exception ex)
            {
                Common.setMessageBox(String.Format("{0}....{1}", ex.Message, ex.StackTrace), "Payment Splitting", 3);
                return;
            }
            finally
            {
                SplashScreenManager.CloseForm(false);
            }
        }

        void cboAgent_KeyPress(object sender, KeyPressEventArgs e)
        {
            Methods.AutoComplete(cboAgent, e, true);
        }

        void setDbComboBoxAgent(System.Data.DataSet dst)
        {
            DataTable Dt;

            Dt = dst.Tables[1];

            Common.setComboList(cboAgent, Dt, "UTIN", "NAME");

            cboAgent.SelectedIndex = -1;
        }

        void setReloadGridview(System.Data.DataSet dtsec)
        {
            IsAgent = true;

            gridControl5.DataSource = null;

            gridView4.Columns.Clear();

            dts = dtsec.Tables[2];

            if (dts != null && dts.Rows.Count > 0)
            {
                //dt.Columns.Add("Money", typeof(double)));
                dts.Columns.Add("Amount", typeof(double));

                dts.AcceptChanges();

                //gridControl4.DataSource = dts;

                gridControl5.DataSource = dts;

                gridView4.OptionsBehavior.Editable = true;
                //gridView4.OptionsBehavior.Editable = true;
                //gridView4.OptionsBehavior.Editable = true;
                gridView4.Columns["UTIN"].OptionsColumn.AllowEdit = false;
                gridView4.Columns["UTIN"].Width = 220;
                gridView4.Columns["NAME"].OptionsColumn.AllowEdit = false;
                gridView4.Columns["Amount"].OptionsColumn.AllowEdit = true;
                gridView4.Columns["Amount"].DisplayFormat.FormatType = FormatType.Numeric;
                gridView4.Columns["Amount"].DisplayFormat.FormatString = "n2";
                gridView4.Columns["Amount"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                gridView4.Columns["Amount"].SummaryItem.DisplayFormat = "Total = {0:n2}";
                gridView4.Columns["UTIN"].Visible = false;
                gridView4.BestFitColumns();



            }

            if (IsAgent)
            {
                selection = new GridCheckMarksSelection(gridView4, ref lblSelect);
                selection.CheckMarkColumn.VisibleIndex = 0;
                IsAgent = false;
            }
        }

        void GetAgencyCodeByRevenue(string parameters)
        {
            DataTable Dt;

            using (var ds = new System.Data.DataSet())
            {
                using (SqlDataAdapter ada = new SqlDataAdapter((string)string.Format("SELECT Registration.tblAgency.AgencyCode,Registration.tblAgency.AgencyName FROM Collection.tblRevenueType INNER JOIN Registration.tblAgency ON Collection.tblRevenueType.AgencyCode = Registration.tblAgency.AgencyCode WHERE Collection.tblRevenueType.RevenueCode ='{0}'", parameters), Logic.ConnectionString))
                {
                    ada.SelectCommand.CommandTimeout = 0;

                    ada.Fill(ds, "table");
                }

                Dt = ds.Tables[0];
            }

            if (Dt != null && Dt.Rows.Count > 0)
            {
                strAgencycode = Dt.Rows[0]["AgencyCode"].ToString();
                strAgencyName = Dt.Rows[0]["AgencyName"].ToString();
            }
        }

        void GetUtinpayer()
        {
            dtRev.Clear();
            jk = 0;
            for (int i = 0; i < selection.SelectedCount; i++)
            {
                jk = jk + 1;
                string lol = ((selection.GetSelectedRow(i) as DataRowView)["UTIN"].ToString());
                string lol1 = ((selection.GetSelectedRow(i) as DataRowView)["Name"].ToString());
                string lol2 = ((selection.GetSelectedRow(i) as DataRowView)["Amount"].ToString());

                dtRev.Rows.Add(new object[] { Convert.ToString(jk), lol, lol1, null, null, null, null, Convert.ToDouble(lol2), });

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

                    command = new SqlCommand("doInsertSplitReceipt", connect) { CommandType = CommandType.StoredProcedure };
                    command.Parameters.Add(new SqlParameter("@tblCollectionReport_Temp", SqlDbType.Structured)).Value = dataSet.Tables[1];
                    command.CommandTimeout = 0;

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

        private void setImages()
        {
            tsbNew.Image = MDIMain.publicMDIParent.i16x16.Images[5];
            tsbEdit.Image = MDIMain.publicMDIParent.i16x16.Images[6];
            tsbDelete.Image = MDIMain.publicMDIParent.i16x16.Images[7];
            tsbReload.Image = MDIMain.publicMDIParent.i16x16.Images[0];
            tsbClose.Image = MDIMain.publicMDIParent.i16x16.Images[11];

            //bttnCancel.Image = MDIMain.publicMDIParent.i32x32.Images[9];
            //btnMain.Image = MDIMain.publicMDIParent.i32x32.Images[8];
            btnUpdate.Image = MDIMain.publicMDIParent.i32x32.Images[29];
            btnSearch.Image = MDIMain.publicMDIParent.i32x32.Images[7];
            btnlist.Image = MDIMain.publicMDIParent.i32x32.Images[6];


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
                Close();
            }
            else if (sender == tsbNew)
            {
                //boolIsUpdate = false;
            }
            else if (sender == tsbEdit)
            {
                //iTransType = TransactionTypeCode.Edit;

                //boolIsUpdate = true;

            }

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

        void ProcessDataTable(DataTable Dt)
        {
            //if (Dt != null && Dt.Rows.Count > 0)
            //{
            //    Dt.Columns.Add("URL", typeof(string));
            //    Dt.AcceptChanges();

            //    foreach (DataRow item in Dt.Rows)
            //    {
            //        if (item == null) continue;
            //        //decimal amount = decimal.Parse(item["Amount"].ToString());
            //        try
            //        {
            //            item["AmountWords"] = amounttowords.convertToWords(item["Amount"].ToString());

            //            if (item["PayerID"].ToString().Length > 14)
            //            {
            //                item["PayerID"] = "Please approach the BIR for your unique Payer ID.";
            //            }
            //            else
            //            {
            //                item["PayerID"] = item["PayerID"].ToString();
            //            }

            //            item["URL"] = string.Format(@"Payment for {0} {1} << Paid at {2} - {3} , Deposit Slip Number {4} by {5}  >> ", item["Description"], item["RevenueCode"], item["BankName"], item["BranchName"], item["DepositSlipNumber"], item["PaymentMethod"]);

            //            item["User"] = Program.UserID.ToUpper();

            //            item["Username"] = string.Format(@"<< Reprint of  receipt with control number {0}  at {1} printing station >>", item["ControlNumber"], item["StationName"]);

            //            //strreprint = string.Format(" Reprint of {0} Receipt", item["ControlNumber"]);

            //            item["PaymentDate"] = Convert.ToDateTime(item["PaymentDate"]).ToString("dd-MMM-yyyy");
            //        }
            //        catch
            //        {

            //        }
            //    }
            //}

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

                        if (item["PayerID"].ToString().Length > 14)
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

                        item["Username"] = string.Format(@"</Printed at {0} Zonal Office  by {1} on {2}/>", item["StationName"], user, DateTime.Today.ToString("dd-MMM-yyyy"));

                        item["PaymentDate"] = Convert.ToDateTime(item["PaymentDate"]).ToString("dd-MMM-yyyy");
                    }
                    catch
                    {

                    }
                }
            }
        }

        string GetRecordWaitMainfest(DataTable Dt)
        {
            string values = string.Empty;

            int j = 0;

            if (Dt != null && Dt.Rows.Count > 0)
            {
                foreach (DataRow item in Dt.Rows)
                {
                    if (item == null) continue;
                    item["PayerID"] = item["PayerID"].ToString();

                    values += String.Format("'{0}'", item["PaymentRefNumber"].ToString());
                    if (j + 1 < Dt.Rows.Count)
                        values += ",";
                    ++j;
                }
            }

            return values;
        }
        void Checkrecord()
        {
            DataTable dtg = new DataTable();

            string sql2 = String.Format(@"SELECT  PaymentRefNumber FROM  Receipt.tblCollectionReceipt WHERE  ( SUBSTRING(REVERSE(REPLACE(PaymentRefNumber, '', '')), 1, 1) = 'R' ) AND isPrinted IS NULL");

            dtg = (new Logic()).getSqlStatement(sql2).Tables[0];

            if (dtg != null && dtg.Rows.Count > 0)
            {
                DialogResult result = MessageBox.Show(string.Format("You have {0} record(s) yet to be completed. Click Yes, to Reprint to print them again if they were not printed successfully Or Click No, to apply Control Number, which means you already printed these records successfully ", dtg.Rows.Count), "Unfinished Job", MessageBoxButtons.YesNoCancel);


                if (result == DialogResult.Yes)
                {
                    //isReprint = true; GetPayRef();
                    //return;
                    using (FrmDisplay display = new FrmDisplay() { FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog })
                    {
                        Program.IsSplitRecord = true;
                        display.ShowDialog();
                    }
                }
                else if (result == DialogResult.No)
                {

                    criteria3 = GetRecordWaitMainfest(dts);

                    using (FrmMainFest frmMainFest = new FrmMainFest(strCollectionReportID, criteria3, criteria2, false) { IDList = strCollectionReportID })
                    {
                        //frmMainFest.ShowDialog();
                        frmMainFest.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
                        frmMainFest.ShowDialog();
                    }
                }
                else
                {
                    tsbClose.PerformClick();
                }
            }
            //else
            //{
            //    string strquye2 = string.Format("INSERT INTO Receipt.tblCollectionReceipt ( PaymentRefNumber , EReceipts ,EReceiptsDate, StationCode )VALUES('{0}','{1}','{2}','{3}');", strPayRef, strerecipt, dtstart.ToString("yyyy/MM/dd"), Program.stationCode);

            //    using (SqlCommand sqlCommand1 = new SqlCommand(strquye2, db, transaction))
            //    {
            //        sqlCommand1.ExecuteNonQuery();

            //    }

            //    //transaction.Commit();
            //}
        }

        public class Payments
        {
            public virtual string PayerName { get; set; }
            public virtual string Address { get; set; }
            public virtual decimal Amount { get; set; }
        }
        private void Btnupload_Click(object sender, EventArgs e)
        {
            try
            {
                txtpayername.Enabled = false;
                txtAmount.Enabled = false;
                txtaddress.Enabled = false;
                btnlist.Enabled = false;

                if (string.IsNullOrEmpty(txtsearchpay.Text))
                {
                    Common.setEmptyField("Payment Reference ", Program.ApplicationName); return;
                }
                else
                {
                    if (cboAgency.SelectedValue == null || cboRevenue.SelectedValue == null)
                    {
                        Common.setEmptyField("Payment Revue Description ", Program.ApplicationName); return;
                    }
                    else
                    {
                        DataTable dtup = new DataTable();

                        if (radioGroup3.EditValue.ToString() == "2")
                        {
                            SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                            //groupBox3.Visible = true;
                            //groupBox2.Visible = false;
                            //groupBox3.Location = new Point(8, 147);
                            //cboRevenue.Focus(); clearlist();
                            //Logic.ClearFormControl(this);

                            //OpenFileDialog of = new OpenFileDialog();
                            //of.Filter = "Excel Files|*.xls;*.xlsx;*.xlsm";
                            using (
                       OpenFileDialog _openFileDialogCSV = new OpenFileDialog()
                       {
                           InitialDirectory = Application.ExecutablePath,
                           //Filter = "Excel files (*.xls)|*.xls|All files (*.*)|*.*;*.xlsx;*.xlsm",
                           Filter = "Excel Files|*.xls;*.xlsx;*.xlsm",
                           FilterIndex = 1,
                           RestoreDirectory = true
                       })
                            {
                                if (_openFileDialogCSV.ShowDialog() == DialogResult.OK)
                                {

                                    if (_openFileDialogCSV.FileName.Length > 0)
                                    {
                                        filenamesopen = _openFileDialogCSV.FileName;


                                        excel = new ExcelQueryFactory(filenamesopen);

                                        var worksheetNames = excel.GetWorksheetNames();

                                        int cg = 0;

                                        //dtpay.Clear();                            //dtpay.Clear();
                                        var getData = (from a in excel.Worksheet(sheetName)
                                                       select new Payments
                                                       {
                                                           PayerName = a[0].Cast<string>(),
                                                           Address = a[1].Cast<string>(),
                                                           Amount = a[2].Cast<decimal>()
                                                       }).ToList();

                                        if (getData.Any())
                                        {


                                            //dtRev.Rows.Add(new object[] { Convert.ToString(jk), strUTIN, txtpayername.Text, cboRevenue.SelectedValue.ToString().Trim(), cboRevenue.Text.ToString().Trim(), strAgencycode, strAgencyName, Convert.ToDouble(txtAmount.Text), txtaddress.Text.ToString() });
                                            dtup = getData.ToDataTable();

                                            //dtRev.Columns.Add("SN", typeof(string));
                                            //dtRev.Columns.Add("UTIN", typeof(string));
                                            //dtRev.Columns.Add("Revenue Code", typeof(string));
                                            //dtRev.Columns.Add("Revenue Name", typeof(string));
                                            //dtRev.Columns.Add("Agency Code", typeof(string));
                                            //dtRev.Columns.Add("Agency Name", typeof(string));

                                            //dtRev.AcceptChanges();

                                            foreach (DataRow item in dtup.Rows)
                                            {
                                                cg = cg + 1;
                                                if (item == null) continue;

                                                //dtRev.Rows.Add(new object[]
                                                //  {
                                                //      cg, strUTIN, item["PayerName"].ToString(),
                                                //      cboRevenue.SelectedValue.ToString().Trim(),
                                                //      cboRevenue.Text.ToString().Trim(), strAgencycode, strAgencyName,
                                                //      strAgencyName, Convert.ToDouble( item["Amount"].ToString()),
                                                //      item["Address"].ToString()
                                                //  });
                                                dtRev.Rows.Add(new object[] { Convert.ToString(cg), strUTIN, item["PayerName"].ToString(), cboRevenue.SelectedValue.ToString().Trim(), cboRevenue.Text.ToString().Trim(), strAgencycode, strAgencyName, Convert.ToDouble(item["Amount"].ToString()), item["Address"].ToString() });
                                            }

                                            gridControl2.DataSource = dtRev;
                                            gridView3.OptionsBehavior.Editable = false;

                                            //gridView3.Columns["Amount"].OptionsColumn.AllowEdit = true;

                                            gridView3.Columns["Amount"].DisplayFormat.FormatType = FormatType.Numeric;
                                            gridView3.Columns["Amount"].DisplayFormat.FormatString = "n2";
                                            gridView3.Columns["Amount"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                                            gridView3.Columns["Amount"].SummaryItem.DisplayFormat = "Total = {0:n2}";
                                            gridView3.Columns["Revenue Code"].Visible = false;
                                            gridView3.Columns["Revenue Name"].Visible = false;
                                            gridView3.Columns["Agency Code"].Visible = false;
                                            gridView3.Columns["Agency Name"].Visible = false;
                                            gridView3.Columns["SN"].Visible = false;
                                            gridView3.Columns["UTIN"].Visible = false;
                                            gridView3.BestFitColumns();
                                        }


                                    }
                                }
                            }
                        }
                    }

                }


                //dtpay = new DataTable();


            }
            catch (Exception exception)
            {
                Common.setMessageBox(exception.Message, Program.ApplicationName, 3);
                return;
            }
            finally
            {
                SplashScreenManager.CloseForm(false);
            }
        }

    }
}
