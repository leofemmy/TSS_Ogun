using BankReconciliation.Class;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using TaxSmartSuite.Class;

namespace BankReconciliation.Forms
{
    public partial class FrmIGR : Form
    {
        private bool isFirst = false;

        private string monthName;

        public static FrmIGR publicStreetGroup;

        protected TransactionTypeCode iTransType;

        private String[] split;

        private string strmonth;

        string strRef, strperiod;

        DateTime dtbdate, dtedate;

        string strbdate, stredate;

        double dbCollect, dbpay, dbvat, dbwth;

        public FrmIGR()
        {
            InitializeComponent();

            publicStreetGroup = this;

            setImages();

            ToolStripEvent();

            iTransType = TransactionTypeCode.New;

            Load += OnFormLoad;

            dtpStart.ValueChanged += dtpStart_ValueChanged;

            //bttnUpdate.Click += bttnUpdate_Click;

            OnFormLoad(null, null);
        }

        private void setImages()
        {
            tsbNew.Image = MDIMains.publicMDIParent.i16x16.Images[5];
            tsbEdit.Image = MDIMains.publicMDIParent.i16x16.Images[6];
            tsbDelete.Image = MDIMains.publicMDIParent.i16x16.Images[7];
            tsbReload.Image = MDIMains.publicMDIParent.i16x16.Images[0];
            tsbClose.Image = MDIMains.publicMDIParent.i16x16.Images[11];

            //bttnCancel.Image = MDIMains.publicMDIParent.i32x32.Images[9];
            bttnUpdate.Image = MDIMains.publicMDIParent.i32x32.Images[7];
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
                MDIMains.publicMDIParent.RemoveControls();
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

        public void RefreshForm()
        {
            NavBars.ToolStripEnableDisableControls(toolStrip, Tag as String);
        }

        void dtpStart_ValueChanged(object sender, EventArgs e)
        {
            label42.Text = dtpStart.Value.ToLongDateString();
        }

        void setDBComboBox()
        {
            DataTable Dt;

            using (var ds = new System.Data.DataSet())
            {
                using (SqlDataAdapter ada = new SqlDataAdapter((string)"SELECT BatchID FROM tblBatch", Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }

                Dt = ds.Tables[0];
            }

            Common.setComboList(cboBatch, Dt, "BatchID", "BatchID");

            cboBatch.SelectedIndex = -1;

        }

        void GetStateRefer()
        {
            string quy = String.Format("select CONVERT(DATE,bdate)AS bdate,CONVERT(DATE,Edate)AS Edate,RefNo,Payable,VAT,WTH FROM tblStateRef where statecode ='{0}'", Program.stateCode);

            DataTable dts = (new Logic()).getSqlStatement((quy)).Tables[0];


            //, if (dtes != null && dtes.Rows.Count > 0)
            if (dts != null && dts.Rows.Count > 0)
            {
                strRef = dts.Rows[0]["RefNo"].ToString();

                strbdate = dts.Rows[0]["Bdate"].ToString();
                dtbdate = Convert.ToDateTime(dts.Rows[0]["Bdate"]);

                stredate = dts.Rows[0]["Edate"].ToString();
                dtedate = Convert.ToDateTime(dts.Rows[0]["Edate"]);
                dbpay = Convert.ToDouble(dts.Rows[0]["Payable"]);
                dbvat = Convert.ToDouble(dts.Rows[0]["VAT"]);
                dbwth = Convert.ToDouble(dts.Rows[0]["WTH"]);



            }

        }


        //void bttnUpdate_Click(object sender, EventArgs e)
        //{
        //    var dtf = CultureInfo.CurrentCulture.DateTimeFormat;


        //    if (radioGroup1.SelectedIndex == -1)
        //    {
        //        Common.setEmptyField(" IGR Report Options...", Program.ApplicationName);
        //        return;
        //    }
        //    else
        //    {

        //        GetStateRefer();

        //        //strperiod = String.Format("{0}/{1}", cboPeriods.SelectedValue, cboYears.SelectedValue);

        //        //GetPeriodCollection(strperiod);

        //        #region

        //        //// get month string 
        //        //strmonth = cboPeriods.Text.Substring(0, 2);

        //        //split = cboPeriods.Text.Trim().Split(new Char[] { '/' });

        //        //var dtf = CultureInfo.CurrentCulture.DateTimeFormat;

        //        //var month = Convert.ToInt32(cboPeriods.Text.Substring(0, 2));

        //        //var fyear = cboPeriods.Text.Length;

        //        //monthName = dtf.GetMonthName(month);
        //        #endregion

        //        if (radioGroup1.EditValue == "Bank")//Call Report for bank Reconciliation
        //        {
        //            //string fulltext = String.Format(" for the month of : [{0} {1}]", cboPeriods.Text, cboYears.SelectedValue);

        //            //XtraRepGeneral reports = new XtraRepGeneral();

        //            //reports.xrLabel18.Text = fulltext;

        //            //reports.parmPeriod.Value = cboPeriods.SelectedValue;

        //            //reports.paramYear.Value = cboYears.SelectedValue;

        //            //reports.ShowPreviewDialog();
        //            DataSet dataSet1 = new DataSet();

        //            string query = string.Format("SELECT Description, BankName, AccountNo, OpenBal, CloseBal, Dr, Cr, AccountName, BranchName, BankShortCode, StartDate, EndDate FROM ViewTransactionBankAccount WHERE CONVERT(VARCHAR(10),StartDate,103)='{0}' AND CONVERT(VARCHAR(10),EndDate,103)='{1}'", string.Format("{0:dd/MM/yyy}", dtpStart.Value), string.Format("{0:dd/MM/yyy}", dtpEnd.Value));

        //            using (SqlDataAdapter ada = new SqlDataAdapter((string)query, Logic.ConnectionString))
        //            {
        //                ada.Fill(dataSet1, "table");
        //            }

        //            if (dataSet1.Tables[0].Rows.Count == 0)
        //            {
        //                Common.setMessageBox("No Record For This Report ", Program.ApplicationName, 1);
        //                return;
        //            }
        //            else
        //            {

        //                XtraRepGen report = new XtraRepGen();
        //                report.xrLabel18.Text = String.Format(" for the month of : [{0} {1}]", dtpStart.Value.Month, dtpStart.Value.Year);
        //                //report.paramPeriod.Value = dtpStart.Value.Month;
        //                //report.paramYear.Value = dtpStart.Value.Year;
        //                report.DataSource = dataSet1;
        //                report.DataMember = "table";
        //                report.ShowPreviewDialog();
        //            }
        //        }
        //        else if (radioGroup1.EditValue == "Payment")//Call Report for IGR Payment
        //        {
        //            //strbdate, stredate;
        //            XtraRepIGRPay repigrpay = new XtraRepIGRPay();

        //            repigrpay.xrLabel24.Text = string.Format("{0:dd MMMM, yyyy}", DateTime.Now);

        //            repigrpay.xrLabel5.Text = string.Format("Please refer to your letters referenced: {0} dated {1}", strRef, string.Format("{0:dd MMMM yyyy}", dtbdate));
        //            //, string.Format("{0:dd MMMM yyyy}", dtedate)
        //            repigrpay.xrLabel4.Text = string.Format("REQUEST FOR THE PAYMENT OF OUR {0} FEE", dtf.GetMonthName(dtpStart.Value.Month));

        //            repigrpay.xrLabel6.Text = string.Format("We hereby request you to pay our fee for the month of {0}, {1} computed as follows:", dtf.GetMonthName(dtpStart.Value.Month), dtpStart.Value.Year);

        //            repigrpay.xrTableCell4.Text = string.Format("Gross Collection monitored for the month of {0},{1}", dtf.GetMonthName(dtpStart.Value.Month), dtpStart.Value.Year);

        //            repigrpay.xrTableCell5.Text = string.Format("{0:n2}", dbCollect);

        //            repigrpay.xrTableCell15.Text = "180,000,000.00";

        //            repigrpay.xrTableCell6.Text = string.Format("Incremental Collection for the month of {0},{1}", dtf.GetMonthName(dtpStart.Value.Month), dtpStart.Value.Year);

        //            double dbt = Convert.ToDouble(dbCollect - 180000000); //incremental collection

        //            repigrpay.xrTableCell7.Text = string.Format("{0:n2}", dbt);

        //            double dbfeepay = Convert.ToDouble((15 / 100) * dbt);

        //            repigrpay.xrTableCell9.Text = string.Format("{0:n2}", dbfeepay);

        //            double dbvatadd = Convert.ToDouble((5 / 100) * dbfeepay);

        //            repigrpay.xrTableCell11.Text = string.Format("{0:n2}", dbvatadd);

        //            double dbgrossfee = Convert.ToDouble(dbvatadd + dbfeepay);

        //            repigrpay.xrTableCell19.Text = string.Format("{0:n2}", dbgrossfee);

        //            //repigrpay.xrLabel9.Text = string.Format("Total IGR monitored for the month of {0},{1} computed as follows:", dtpStart.Value.Month, dtpStart.Value.Year);

        //            //repigrpay.xrLabel10.Text = string.Format("{0:n2}", dbCollect);

        //            //repigrpay.xrLabel12.Text = string.Format("{0:n2}", dbCollect * (dbpay / 100));

        //            //repigrpay.xrLabel17.Text = string.Format("{0:n2}", (dbCollect * (dbpay / 100)) * (dbwth / 100));

        //            //repigrpay.xrLabel18.Text = string.Format("{0:n2}", ((dbCollect * (dbpay / 100)) - ((dbCollect * (dbpay / 100)) * (dbwth / 100))));

        //            //repigrpay.xrLabel19.Text = string.Format("{0:n2}", (((dbCollect * (dbpay / 100)) - ((dbCollect * (dbpay / 100)) * (dbwth / 100)))) * (dbvat / 100));

        //            //repigrpay.xrLabel20.Text = string.Format("{0:n2}", ((dbCollect * (dbpay / 100)) - ((dbCollect * (dbpay / 100)) * (dbwth / 100))) - ((((dbCollect * (dbpay / 100)) - ((dbCollect * (dbpay / 100)) * (dbwth / 100)))) * (dbvat / 100)));

        //            repigrpay.ShowPreviewDialog();

        //        }
        //        else if (radioGroup1.EditValue == "Summary")
        //        {
        //            DataSet dataSet1 = new DataSet();

        //            string query = string.Format("SELECT AccountNo, OpenBal, CloseBal, Dr, Cr, BankShortCode, BankName, StartDate, EndDate, Amount, ExAmount FROM ViewSummaryIGRAccount WHERE CONVERT(VARCHAR(10),StartDate,103)='{0}' AND CONVERT(VARCHAR(10),EndDate,103)='{1}'", string.Format("{0:dd/MM/yyy}", dtpStart.Value), string.Format("{0:dd/MM/yyy}", dtpEnd.Value));
        //            using (SqlDataAdapter ada = new SqlDataAdapter((string)query, Logic.ConnectionString))
        //            {
        //                ada.Fill(dataSet1, "table");
        //            }

        //            if (dataSet1.Tables[0].Rows.Count == 0)
        //            {
        //                Common.setMessageBox("No Record For This Report ", Program.ApplicationName, 1);
        //                return;
        //            }
        //            else
        //            {
        //                XtraRepIGRSummary repsumary = new XtraRepIGRSummary();

        //                repsumary.xrLabel13.Text = string.Format("{0} State Government", Program.StateName);

        //                repsumary.xrLabel14.Text = string.Format("Summary of {0} State Government IGR Accounts for {1}, {2}", Program.StateName, dtf.GetMonthName(dtpStart.Value.Month), dtpStart.Value.Year);

        //                //repsumary.paramPeriod.Value = dtpStart.Value.Month;

        //                //repsumary.paramYear.Value = dtpStart.Value.Year;
        //                repsumary.DataSource = dataSet1;

        //                repsumary.DataMember = "table";

        //                repsumary.ShowPreviewDialog();
        //            }

        //        }
        //        else if (radioGroup1.EditValue == "Consolidate")
        //        {
        //            //DataSet dataSet1 = new DataSet();
        //            DataSet dataSet1 = new DataSet();

        //            string query = string.Format("SELECT BankShortCode, BankName, OpenBal, CloseBal, Amount, [Returned Cheque], [Transfer To Govt. Acct], [Bank Charge], [Prev Credit Reversed], [Credit Interst], [Transfer From Govt. Acct], [Prev Debit Reversed], [Returned Cheque (Prev)], [Collection Logment], Expr1, StartDate, EndDate FROM  ViewConsolidateDetailsTransactions WHERE CONVERT(VARCHAR(10),StartDate,103)='{0}' AND CONVERT(VARCHAR(10),EndDate,103)='{1}'", string.Format("{0:dd/MM/yyy}", dtpStart.Value), string.Format("{0:dd/MM/yyy}", dtpEnd.Value));
        //            using (SqlDataAdapter ada = new SqlDataAdapter((string)query, Logic.ConnectionString))
        //            {
        //                ada.Fill(dataSet1, "table");
        //            }

        //            if (dataSet1.Tables[0].Rows.Count == 0)
        //            {
        //                Common.setMessageBox("No Record For This Report ", Program.ApplicationName, 1);
        //                return;
        //            }
        //            else
        //            {
        //XtraRepSummary repsummary = new XtraRepSummary();
        //repsummary.xrLabel32.Text = string.Format("{0} STATE GOVERNMENT ", Program.StateName.ToUpper());
        //repsummary.xrLabel33.Text = string.Format(" CONSOLIDATED SUMMARY ACCOUNT OF IGR FOR THE MONTH OF {0} , {1}", dtf.GetMonthName(dtpStart.Value.Month), dtpStart.Value.Year);

        //repsummary.DataSource = dataSet1;

        //repsummary.DataMember = "table";

        ////repsummary.paramPeriod.Value = dtpStart.Value.Month;
        ////repsummary.paramYear.Value = dtpStart.Value.Year;
        //repsummary.ShowPreviewDialog();
        //            }

        //        }
        //        else if (radioGroup1.EditValue == "Account")
        //        {
        //            #region

        //            ////// Create a cross-tab report.
        //            //XtraReport report = CreateReport();

        //            //XRLabel label1 = new XRLabel();


        //            ////call a igraccount detail report
        //            ////XtraRepIGRAccount report = new XtraRepIGRAccount();

        //            ////report.xrLabel13.Text = string.Format("{0} State Government", Program.StateName);

        //            ////report.xrLabel14.Text = string.Format("Transfer / Charges Account Details IGR for the month of {0}, {1}", cboPeriods.Text, cboYears.SelectedValue.ToString());

        //            ////report.paramPeriod.Value = cboPeriods.Text;
        //            //// Show its Print Preview.

        //            //report.Landscape = true;

        //            //report.PaperKind = System.Drawing.Printing.PaperKind.Custom;
        //            //report.PageHeight = 850;
        //            //report.PageWidth = 1100;
        //            //report.Margins = new System.Drawing.Printing.Margins(15, 15, 100, 100);
        //            //report.VerticalContentSplitting = DevExpress.XtraPrinting.VerticalContentSplitting.Smart;

        //            //report.ShowPreview();
        //            #endregion

        //            #region

        //            //DataSet dataSet1 = new DataSet();

        //            //string quy = string.Format("SELECT Period,AccountNo,OpenBal,CloseBal,TransDescription,BankName,BankShortCode,Amount,Years FROM ViewIGRAccountDetails WHERE Period='{0}' and years='{1}' ", cboPeriods.SelectedValue, cboYears.SelectedValue);

        //            //using (SqlDataAdapter ada = new SqlDataAdapter((string)quy, Logic.ConnectionString))
        //            //{
        //            //    ada.Fill(dataSet1, "table");
        //            //}

        //            //XtraRepCrossTab reportAct = new XtraRepCrossTab();

        //            //reportAct.xrLabel1.Text = string.Format(" {0} STATE GOVERNMENT ", Program.StateName.ToUpper());

        //            //reportAct.xrLabel2.Text = string.Format(" TRANSFER / CHARGES OF ACCOUNT DETAILS OF IGR FOR THE  {0} ,{1}", cboPeriods.Text, cboYears.SelectedValue);

        //            //reportAct.xrPivotGrid1.DataSource = dataSet1;

        //            //reportAct.xrPivotGrid1.DataMember = "table";

        //            //reportAct.ShowPreviewDialog();
        //            #endregion

        //            DataSet dataSet1 = new DataSet();

        //            //string query = string.Format("SELECT AccountNo, OpenBal, CloseBal, Amount, transdescription, BankShortCode, BankName, StartDate, EndDate FROM  ViewIGRAccountDetails WHERE CONVERT(VARCHAR(10),StartDate,103)='{0}' AND CONVERT(VARCHAR(10),EndDate,103)='{1}'", string.Format("{0:dd/MM/yyy}", dtpStart.Value), string.Format("{0:dd/MM/yyy}", dtpEnd.Value));
        //            using (SqlDataAdapter ada = new SqlDataAdapter((string)query, Logic.ConnectionString))
        //            {
        //                ada.Fill(dataSet1, "table");
        //            }

        //            if (dataSet1.Tables[0].Rows.Count == 0)
        //            {
        //                Common.setMessageBox("No Record For This Report ", Program.ApplicationName, 1);
        //                return;
        //            }
        //            else
        //            {
        //                XtraRepIGRAccounts repacct = new XtraRepIGRAccounts();

        //                repacct.xrLabel27.Text = string.Format(" {0} STATE GOVERNMENT ", Program.StateName.ToUpper());

        //                repacct.xrLabel28.Text = string.Format(" DETAILS OF TRANSFER / CHARGES FOR THE MONTH OF  {0} ,{1}", dtf.GetMonthName(dtpStart.Value.Month), dtpStart.Value.Year);

        //                //repacct.paramPeriod.Value = dtpStart.Value.Month;

        //                //repacct.paramYear.Value = dtpStart.Value.Year;
        //                repacct.DataSource = dataSet1;

        //                repacct.DataMember = "table";

        //                repacct.ShowPreviewDialog();
        //            }
        //        }
        //        else if (radioGroup1.EditValue == "Revenue")
        //        {
        //            System.Data.DataSet ds = new System.Data.DataSet();

        //            string quy = string.Format("SELECT Row_number() OVER ( ORDER BY ( SELECT   1 ) ) AS SN ,RevenueCode , Description ,SUM(Amount) AS Amount FROM ViewReportReconciliationCollection WHERE (CONVERT(VARCHAR, CONVERT(DATE, PaymentDate), 102) BETWEEN'{0}' and '{1}') GROUP BY RevenueCode ,Description", string.Format("{0:yyyy.MM.dd}", dtpStart.Value), string.Format("{0:yyyy.MM.dd}", dtpEnd.Value));

        //            using (SqlDataAdapter ada = new SqlDataAdapter((string)quy, Logic.ConnectionString))
        //            {
        //                ada.Fill(ds, "table");
        //            }

        //            if (ds.Tables[0].Rows.Count == 0)
        //            {
        //                Common.setMessageBox("No Record For This Report ", Program.ApplicationName, 1);
        //                return;
        //            }
        //            else
        //            {

        //                XtraRepSummRevenue report = new XtraRepSummRevenue() { DataSource = ds, DataMember = "table" };
        //                report.paramMonths.Value = dtpStart.Value.Month;
        //                report.paramYear.Value = dtpStart.Value.Year;

        //                report.xrLabel32.Text = string.Format(" {0} STATE GOVERNMENT ", Program.StateName.ToUpper());

        //                report.xrLabel33.Text = string.Format("Summary of Collection by Revenue Type for the Month of  {0} , {1}", dtf.GetMonthName(dtpStart.Value.Month), dtpStart.Value.Year);

        //                report.ShowPreviewDialog();
        //            }
        //            //[Months] 
        //            //FilterString = "StartsWith([BankCode], ?paramBankCode) And [Period] = ?paramPeriod And [IsRecordExit] = ?paramRecord"
        //        }
        //        else if (radioGroup1.EditValue == "Agency")
        //        {
        //            System.Data.DataSet ds = new System.Data.DataSet();

        //            string quy = string.Format("SELECT Row_number() OVER ( ORDER BY ( SELECT   1 ) ) AS SN ,AgencyCode ,AgencyName ,SUM(Amount)AS Amount  FROM ViewReportReconciliationagency WHERE (CONVERT(VARCHAR, CONVERT(DATE, PaymentDate), 102) BETWEEN'{0}' and '{1}') GROUP BY AgencyCode ,AgencyName", string.Format("{0:yyyy.MM.dd}", dtpStart.Value), string.Format("{0:yyyy.MM.dd}", dtpEnd.Value));

        //            using (SqlDataAdapter ada = new SqlDataAdapter((string)quy, Logic.ConnectionString))
        //            {
        //                ada.Fill(ds, "table");
        //            }

        //            if (ds.Tables[0].Rows.Count == 0)
        //            {
        //                Common.setMessageBox("No Record For This Report ", Program.ApplicationName, 1);
        //                return;
        //            }
        //            else
        //            {
        //                XtraRepSummrayAgency repAgency = new XtraRepSummrayAgency() { DataSource = ds, DataMember = "table" };
        //                repAgency.xrLabel32.Text = string.Format(" {0} STATE GOVERNMENT ", Program.StateName.ToUpper());

        //                repAgency.xrLabel33.Text = string.Format("Summary of Collection by Agencies for the Month of  {0} , {1}", dtf.GetMonthName(dtpStart.Value.Month), dtpStart.Value.Year);
        //                repAgency.ShowPreviewDialog();
        //            }
        //        }
        //        else if (radioGroup1.EditValue == "Details")
        //        {
        //            System.Data.DataSet ds = new System.Data.DataSet();

        //            string quy = string.Format("SELECT PaymentDate, PayerName, RevenueCode, ChequeValueDate, Description, Amount, Period, BankCode, years, PaymentRefNumber, Months,IsRecordExit, IsPayDirect, AgencyName, AgencyCode, BankName FROM ViewReconciliationAgencyDetails WHERE (CONVERT(VARCHAR, CONVERT(DATE, PaymentDate), 102) BETWEEN'{0}' and '{1}')", string.Format("{0:yyyy.MM.dd}", dtpStart.Value), string.Format("{0:yyyy.MM.dd}", dtpEnd.Value));

        //            using (SqlDataAdapter ada = new SqlDataAdapter((string)quy, Logic.ConnectionString))
        //            {
        //                ada.Fill(ds, "table");
        //            }

        //            if (ds.Tables[0].Rows.Count == 0)
        //            {
        //                Common.setMessageBox("No Record For This Report ", Program.ApplicationName, 1);
        //                return;
        //            }
        //            else
        //            {
        //                XtraRepAgencyDetails report = new XtraRepAgencyDetails();
        //                report.xrLabel32.Text = string.Format(" {0} STATE GOVERNMENT ", Program.StateName.ToUpper());
        //                report.xrLabel33.Text = string.Format("Summary of Collection by Agencies Details for the Month of  {0} , {1}", dtf.GetMonthName(dtpStart.Value.Month), dtpStart.Value.Year);

        //                report.paramMonths.Value = dtpStart.Value.Month;
        //                report.paramYears.Value = dtpStart.Value.Year;
        //                report.ShowPreviewDialog();
        //            }
        //        }


        //    }
        //}

    }
}
