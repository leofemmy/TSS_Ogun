using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TaxSmartSuite.Class;
using BankReconciliation.Class;
using System.Data.SqlClient;
using DevExpress.Utils;
using BankReconciliation.Report;
using DevExpress.XtraGrid.Views.Grid;
using System.Globalization;
using DevExpress.XtraReports.UI;
using DevExpress.XtraReports.Parameters;
using DevExpress.XtraSplashScreen;

namespace BankReconciliation.Forms
{
    public partial class FrmCollectionControl : Form
    {
        private bool isFirst = false;

        private string monthName;

        private SqlCommand _command; private SqlDataAdapter adp; private DataTable Dts;


        //private SqlDataAdapter adp;

        private SqlCommand command;

        public static FrmCollectionControl publicStreetGroup;

        protected TransactionTypeCode iTransType; private bool Isbank = false;

        private bool Isbank2 = false;

        private String[] split; private bool isRecord = false;

        protected string ID;

        protected bool boolIsUpdate; double dubRate;

        //double dbCollect;

        double dbCollect, dbvat, dbwth, dbcollectionValue, dblpremises;

        double dbpay;

        string strRef; DateTime dtb; double dbPayable; double dbvatTax; double dbwthTax;

        public FrmCollectionControl()
        {
            SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

            InitializeComponent();

            publicStreetGroup = this;

            setImages();

            ToolStripEvent();

            iTransType = TransactionTypeCode.New;

            Load += OnFormLoad;

            openForm();

            gridView1.DoubleClick += gridView1_DoubleClick;

            cboBank.KeyPress += cboBank_KeyPress;

            txtClosing.Leave += txtClosing_Leave;

            //txtOpening.LostFocus += txtOpening_LostFocus;
            txtOpening.Leave += txtOpening_Leave;

            //txtTransfer.LostFocus += txtTransfer_LostFocus;
            txtTransfer.Leave += txtTransfer_Leave;

            txtTrans.EditValueChanged += txtTrans_EditValueChanged;

            txtTrans.Leave += txtTrans_Leave;

            bttnUpdate.Click += bttnUpdate_Click;

            bttnReport.Click += bttnReport_Click;

            bttnPay.Click += bttnPay_Click;

            bttnIGR.Click += bttnIGR_Click;

            //dtpStart.ValueChanged += dtpStart_ValueChanged;

            //dtpEnd.ValueChanged += dtpEnd_ValueChanged;

            cboAccount.SelectedIndexChanged += cboAccount_SelectedIndexChanged;

            cboBank.SelectedIndexChanged += cboBank_SelectedIndexChanged;

            txtClose.Leave += txtClose_Leave;

            txtopen.Leave += txtopen_Leave;

            txtAdvance.Leave += txtAdvance_Leave;

            txtCharges.Leave += TxtCharges_Leave;

            OnFormLoad(null, null);

            SplashScreenManager.CloseForm(false);
        }

        private void TxtCharges_Leave(object sender, EventArgs e)
        {

            double amt1 = double.Parse(txtCharges.EditValue.ToString(), NumberStyles.Number, CultureInfo.InvariantCulture);

            txtCharges.EditValue = string.Format("{0:N2}", amt1 * dubRate);
        }

        void bttnIGR_Click(object sender, EventArgs e)
        {
            System.Data.DataSet ds = new System.Data.DataSet();

            System.Data.DataSet dst = new System.Data.DataSet();

            System.Data.DataSet dsValue = new System.Data.DataSet();

            using (SqlDataAdapter ada = new SqlDataAdapter((string)"SELECT * FROM Reconciliation.tblStateRef", Logic.ConnectionString))
            {
                ada.Fill(dst, "table");
            }
            if (dst.Tables[0].Rows.Count != 0)
            {

                strRef = (string)dst.Tables[0].Rows[0]["RefNo"];
                dtb = Convert.ToDateTime(dst.Tables[0].Rows[0]["Bdate"]);
                dbPayable = Convert.ToDouble(dst.Tables[0].Rows[0]["Payable"]);
                dbvatTax = Convert.ToDouble(dst.Tables[0].Rows[0]["VAT"]);
                dbwthTax = Convert.ToDouble(dst.Tables[0].Rows[0]["WTH"]);
            }
            else
            {
                Common.setMessageBox("State Reference Letter Has Not Been Setup", Program.ApplicationName, 1);
                return;
            }

            string grq = string.Format("SELECT SUM(CollectionsBal) AS CollectAmount FROM [Reconciliation].[tblCollectionControl] WHERE FinancialperiodID={0}", Convert.ToInt32(label22.Text));

            using (SqlDataAdapter ada = new SqlDataAdapter(grq, Logic.ConnectionString))
            {
                ada.Fill(dsValue, "table");
            }

            if (dsValue.Tables[0].Rows.Count != 0)
            {
                dbcollectionValue = Convert.ToDouble(dsValue.Tables[0].Rows[0]["CollectAmount"]);
            }

            using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
            {
                connect.Open();
                _command = new SqlCommand("BusinessPremises", connect) { CommandType = CommandType.StoredProcedure };

                _command.Parameters.Add(new SqlParameter("@Financialperiod", SqlDbType.VarChar)).Value = Convert.ToInt32(label22.Text);

                _command.CommandTimeout = 0;

                using (System.Data.DataSet dstg = new System.Data.DataSet())
                {
                    adp = new SqlDataAdapter(_command);
                    adp.Fill(dstg);
                    Dts = dstg.Tables[0];
                    connect.Close();

                    if (dstg.Tables[1].Rows.Count != 0)
                    {
                        dblpremises = Convert.ToDouble(dstg.Tables[1].Rows[0]["Amount"]);
                    }
                }
            }

            switch (Program.intCode)
            {
                case 13://Akwa Ibom state
                    XtraRepAkwaIGR igr = new XtraRepAkwaIGR();

                    igr.xrLabel4.Text = string.Format("INTERNALLY GENERATED REVENUE (IGR)");

                    igr.xrLabel5.Text = string.Format("REPORT FOR THE MONTH OF {0}", label21.Text.ToString().ToUpper());

                    igr.xrLabel2.Text = string.Format("Please find attached the detailed IGR report for the month of {0} for your information and use.", label21.Text);

                    igr.xrLabel6.Text = strRef.ToString();

                    igr.xrLabel7.Text = dateTimePicker1.Value.ToString("dd MMMM,yyyy");

                    igr.ShowPreviewDialog();
                    break;
                case 20://delta state
                    XtraRepDeltaIGR delta = new XtraRepDeltaIGR();
                    delta.xrLabel4.Text = string.Format("Request for the payment of our {0:MMMM,yyyy} fees", dateTimePicker1.Value).ToString().ToUpper();

                    delta.xrLabel6.Text = dateTimePicker1.Value.ToString("dd MMMM,yyyy");
                    delta.xrRichText1.Text = string.Format("Please refer to your letter referenced {0} dated {1:dd MMMM yyyy}.\n We hereby request you to pay our fee for the month of {1:dd MMMM,yyyy} computed as follows:", strRef.ToUpper(), dateTimePicker1.Value);
                    delta.xrTableCell3.Text = string.Format("Gross Collection monitored for the month of {0:MMMM,yyyy}", dateTimePicker1.Value);

                    delta.xrTableCell4.Text = string.Format("{0:n2}", dbcollectionValue);
                    delta.xrTableCell6.Text = string.Format("{0:n2}", dblpremises);
                    delta.xrTableCell7.Text = string.Format("Total IGR monitored for the month of{0:MMMM,yyyy}", dateTimePicker1.Value).ToString().ToUpper();
                    double valmont = dbcollectionValue - dblpremises;
                    delta.xrTableCell8.Text = string.Format("{0:n2}", valmont);
                    double valten = valmont / dbPayable;
                    delta.xrTableCell10.Text = string.Format("{0:n2}", valten);
                    double valwat = valten / dbwthTax;
                    delta.xrTableCell14.Text = string.Format("{0:n2}", valwat);
                    double valamt = valten - valwat;
                    delta.xrTableCell12.Text = string.Format("{0:n2}", valamt);
                    double valvat = valamt / dbvatTax;
                    delta.xrTableCell16.Text = string.Format("{0:n2}", valwat);
                    double valTotal = valamt - valwat;
                    delta.xrTableCell18.Text = string.Format("{0:n2}", valTotal);
                    delta.ShowPreviewDialog();
                    break;
                case 32://kogi state
                    break;
                case 37://ogun state 
                    break;

                case 40://oyo state
                    break;
                default:
                    break;
            }

        }

        void txtAdvance_Leave(object sender, EventArgs e)
        {
            double amt1 = double.Parse(txtAdvance.EditValue.ToString(), NumberStyles.Number, CultureInfo.InvariantCulture);

            txtAdvance.EditValue = string.Format("{0:N2}", amt1 * dubRate);
        }

        void txtopen_Leave(object sender, EventArgs e)
        {
            double amt1 = double.Parse(txtopen.EditValue.ToString(), NumberStyles.Number, CultureInfo.InvariantCulture);

            txtopen.EditValue = string.Format("{0:N2}", amt1 * dubRate);
        }

        void txtClose_Leave(object sender, EventArgs e)
        {
            double amt1 = double.Parse(txtClose.EditValue.ToString(), NumberStyles.Number, CultureInfo.InvariantCulture);

            txtClose.EditValue = string.Format("{0:N2}", amt1 * dubRate);
        }

        void cboAccount_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboAccount.SelectedValue != null && !Isbank2)
            {
                //getopenBal();
                getRate();
            }
        }

        void gridView1_DoubleClick(object sender, EventArgs e)
        {
            tsbEdit.PerformClick();
        }
        void txtTrans_Leave(object sender, EventArgs e)
        {

            double amt1 = double.Parse(txtopen.EditValue.ToString(), NumberStyles.Number, CultureInfo.InvariantCulture);

            double amt2 = double.Parse(txtClose.EditValue.ToString(), NumberStyles.Number, CultureInfo.InvariantCulture);

            double amt3 = double.Parse(txtTrans.EditValue.ToString(), NumberStyles.Number, CultureInfo.InvariantCulture);

            double amt4 = double.Parse(txtAdvance.EditValue.ToString(), NumberStyles.Number, CultureInfo.InvariantCulture);

            double amt6 = double.Parse(txtCharges.EditValue.ToString(), NumberStyles.Number, CultureInfo.InvariantCulture);

            txtTrans.EditValue = string.Format("{0:N2}", amt3 * dubRate);

            double amt5 = double.Parse(txtTrans.EditValue.ToString(), NumberStyles.Number, CultureInfo.InvariantCulture);

            double sumt = Convert.ToDouble(amt2) + Convert.ToDouble(amt5) + Convert.ToDouble(amt6) - Convert.ToDouble(amt1) - Convert.ToDouble(amt4);


            //Convert.ToDouble
            txtCollections.Text = string.Format("{0:N2}", sumt);
        }

        void txtTrans_EditValueChanged(object sender, EventArgs e)
        {
            //txtCollections.Text = string.Format("{0:N2}", Convert.ToDouble(txtClosing.Text) - Convert.ToDouble(txtOpening.Text) + Convert.ToDouble(txtTransfer.Text));


        }

        void cboBank_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboBank.SelectedValue != null && !Isbank)
            {
                //getopenBal();
                setDBComboBoxAcct();
            }
        }

        void dtpEnd_ValueChanged(object sender, EventArgs e)
        {
            //label43.Text = dtpEnd.Value.ToLongDateString();
            //setReload(dtpStart.Value, dtpEnd.Value);
        }

        void dtpStart_ValueChanged(object sender, EventArgs e)
        {
            //label42.Text = dtpStart.Value.ToLongDateString();
        }

        void txtOpening_Leave(object sender, EventArgs e)
        {
            //            if (int.TryParse(textbox1.Text, out value)) 
            //{
            //// it's a valid integer => you could use the value variable here
            //}

            if (string.IsNullOrEmpty(txtOpening.Text))
            {
                Common.setMessageBox("Closing Balance empty Filed", Program.ApplicationName, 1); return;
            }
            else if (!Logic.IsAlphaNum((string)txtOpening.Text))
            {
                Common.setMessageBox("Opening Balance Can only contain number variable", Program.ApplicationName, 1);
                txtOpening.Text = string.Empty; return;
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

                //txtCollections.Text = string.Format("{0:N2}", Convert.ToDouble(txtClosing.Text) - Convert.ToDouble(txtOpening.Text) + Convert.ToDouble(txtTransfer.Text));
            }
        }

        void txtTransfer_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtClosing.Text) || string.IsNullOrEmpty(txtOpening.Text) || string.IsNullOrEmpty(txtTransfer.Text))
            {
                Common.setMessageBox("Empty Filed", Program.ApplicationName, 1); return;
            }
            else if (!Logic.IsNumber((string)txtTransfer.Text))
            {
                Common.setMessageBox("Transfer / Charges Can only contain number variable", Program.ApplicationName, 1);
                txtTransfer.Text = string.Empty; return;
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

                txtCollections.Text = string.Format("{0:N2}", (Convert.ToDouble(txtClosing.Text) + Convert.ToDouble(txtCharges.Text)) - (Convert.ToDouble(txtOpening.Text) + Convert.ToDouble(txtTransfer.Text)));
            }
        }

        void txtClosing_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtClosing.Text))
            {
                Common.setMessageBox("Closing Balance empty Filed", Program.ApplicationName, 1); return;
            }
            else if (!Logic.IsNumber((string)txtClosing.Text))
            {
                Common.setMessageBox("Closing Balance Can only contain number variable", Program.ApplicationName, 1);
                txtClosing.Text = string.Empty; return;
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

                //txtCollections.Text = string.Format("{0:N2}", Convert.ToDouble(txtClosing.Text) - Convert.ToDouble(txtOpening.Text) + Convert.ToDouble(txtTransfer.Text));
            }
        }

        void bttnPay_Click(object sender, EventArgs e)
        {
            //dataSet.Tables[1].Rows[0]["ALLRecords"].ToString()
            //get the record from the table
            var dtf = CultureInfo.CurrentCulture.DateTimeFormat;

            System.Data.DataSet ds = new System.Data.DataSet();

            System.Data.DataSet dst = new System.Data.DataSet();

            using (SqlDataAdapter ada = new SqlDataAdapter((string)"SELECT * FROM Reconciliation.tblStateRef", Logic.ConnectionString))
            {
                ada.Fill(dst, "table");
            }
            if (dst.Tables[0].Rows.Count != 0)
            {

                strRef = (string)dst.Tables[0].Rows[0]["RefNo"];
                dtb = Convert.ToDateTime(dst.Tables[0].Rows[0]["Bdate"]);
                dbPayable = Convert.ToDouble(dst.Tables[0].Rows[0]["Payable"]);
                dbvatTax = Convert.ToDouble(dst.Tables[0].Rows[0]["VAT"]);
                dbwthTax = Convert.ToDouble(dst.Tables[0].Rows[0]["WTH"]);
            }
            else
            {
                Common.setMessageBox("State Reference Letter Has Not Been Setup", Program.ApplicationName, 1);
                return;
            }
            string query = string.Format("Select sum(collectionsbal) as Collect from ViewCollectionControl where FinancialperiodID ='{0}'", label22.Text.Trim());

            //string quy = string.Format("select sum(collectionsbal) as Collect from viewcollectioncontrol where [month]='{0}' and [year]='{1}' ", cboPeriods.SelectedValue, cboYears.SelectedValue);

            using (SqlDataAdapter ada = new SqlDataAdapter((string)query, Logic.ConnectionString))
            {
                ada.Fill(ds, "table");
            }

            if (ds.Tables[0].Rows.Count != 0)
            {
                dbCollect = Convert.ToDouble(ds.Tables[0].Rows[0]["Collect"]);
            }

            switch (Program.intCode)
            {
                case 13://Akwa Ibom state
                    XtraRepAkwaPay Akwapay = new XtraRepAkwaPay();
                    Akwapay.xrLabel4.Text = string.Format("REQUEST FOR THE PAYMENT OF OUR COMMISSION FOR THE MONTH OF {0}", label21.Text.ToString().ToUpper());

                    Akwapay.xrLabel5.Text = string.Format("We hereby request for the payment of the commission due to us on the revenue generated for the month of {0}", label21.Text);

                    Akwapay.xrTableCell1.Text = string.Format("Gross Collection for the month of {0}", label21.Text);

                    Akwapay.xrTableCell2.Text = string.Format("{0:n2}", dbCollect);

                    Akwapay.xrLabel6.Text = strRef.ToString();

                    Akwapay.xrLabel7.Text = dateTimePicker1.Value.ToString("dd MMMM,yyyy");

                    double dbfee = (dbPayable / 100) * dbCollect;

                    double dbWth = (dbwthTax / 100) * dbfee;

                    double dbpay = dbfee - dbWth;

                    double dbvat = (dbvatTax / 100) * dbfee;

                    double dbamt = dbpay - dbvat;

                    Akwapay.xrTableCell17.Text = string.Format("{0:n2}", dbfee);

                    Akwapay.xrTableCell14.Text = string.Format("{0:n2}", dbWth);

                    Akwapay.xrTableCell11.Text = string.Format("{0:n2}", dbpay);

                    Akwapay.xrTableCell8.Text = string.Format("{0:n2}", dbvat);

                    Akwapay.xrTableCell5.Text = string.Format("{0:n2}", dbamt);

                    Akwapay.ShowPreviewDialog();
                    break;

                case 20://detla state

                    break;

                case 37://ogun state
                    break;

                case 40://oyo state
                    break;

                case 32://kogi state
                    break;

                default:
                    break;
            }

            //if (ds.Tables[0].Rows.Count != 0)
            //{
            //    dbCollect = Convert.ToDecimal(ds.Tables[0].Rows[0]["Collect"]);

            //    //strbdate, stredate;
            //    XtraRepIGRPay repigrpay = new XtraRepIGRPay();

            //    repigrpay.xrLabel24.Text = string.Format("{0:dd MMMM, yyyy}", DateTime.Now);


            //    //repigrpay.xrLabel5.Text = "Please refer to your letter referenced HCF. 293/T/120 dated 3rd January 2013.";
            //    repigrpay.xrLabel5.Text = string.Format("Please refer to your letter referenced {0} dated {1}", strRef, dtb.ToString("dd MMMM, yyyy"));
            //    repigrpay.xrLabel4.Text = string.Format("REQUEST FOR THE PAYMENT OF OUR {0}  FEE", dtf.GetMonthName(dtpStart.Value.Month));

            //    repigrpay.xrLabel6.Text = string.Format("We hereby request you to pay our fee for the month of {0}, {1} computed as follows:", dtf.GetMonthName(dtpStart.Value.Month), dtpStart.Value);

            //    repigrpay.xrTableCell4.Text = string.Format("Gross Collection monitored for the month of {0}, {1}", dtf.GetMonthName(dtpStart.Value.Month), dtpStart.Value);

            //    repigrpay.xrTableCell5.Text = string.Format("{0:n2}", dbCollect);

            //    //repigrpay.xrLabel25.Text = "Less Business Premises";

            //    const double lessben = 18000000;

            //    repigrpay.xrTableCell15.Text = "180,000,000.00";

            //    repigrpay.xrTableCell6.Text = string.Format("Incremental Collection for the month of {0}, {1}", dtf.GetMonthName(dtpStart.Value.Month), dtpStart.Value);

            //    decimal tot = Convert.ToDecimal(dbCollect) - Convert.ToDecimal(lessben);

            //    repigrpay.xrTableCell7.Text = string.Format("{0:n2}", tot);

            //    //dbpay = Convert.ToDecimal(10) / Convert.ToDecimal(100);

            //    dbpay = Convert.ToDecimal(dbPayable) / Convert.ToDecimal(100);

            //    //dbwth = Convert.ToDecimal(5) / Convert.ToDecimal(100);
            //    dbwth = Convert.ToDecimal(dbwthTax) / Convert.ToDecimal(100);

            //    //dbvat = Convert.ToDecimal(5) / Convert.ToDecimal(100);
            //    dbvat = Convert.ToDecimal(dbvatTax) / Convert.ToDecimal(100);
            //    decimal totfee = dbpay * tot;
            //    //string test = String.Format("{0:0.000}", 10 / 100);
            //    decimal totVat = totfee * dbvat;
            //    repigrpay.xrTableCell9.Text = string.Format("{0:n2}", totfee);
            //    repigrpay.xrTableCell11.Text = string.Format("{0:n2}", (totVat));
            //    //repigrpay.xrTableCell13.Text = string.Format("{0:n2}", ((dbCollect * dbpay) - ((dbCollect * dbpay * dbwth))));
            //    //repigrpay.xrTableCell17.Text = string.Format("{0:n2}", (((dbCollect * dbpay) - ((dbCollect * dbpay * dbwth)))) * (dbvat));
            //    decimal totToa = totfee + totVat;
            //    //repigrpay.xrTableCell19.Text = string.Format("{0:n2}", ((dbCollect * (dbpay)) - ((dbCollect * (dbpay)) * (dbwth))) - ((((dbCollect * (dbpay)) - ((dbCollect * (dbpay)) * (dbwth)))) * (dbvat)));
            //    repigrpay.xrTableCell19.Text = string.Format("{0:n2}", totToa);

            //    repigrpay.ShowPreviewDialog();
            //}

        }

        void bttnReport_Click(object sender, EventArgs e)
        {
            System.Data.DataSet ds = new System.Data.DataSet();

            //string quy = string.Format("select * from viewcollectioncontrol where [month]='{0}' and [year]='{1}' ORDER BY BankCode", cboPeriods.SelectedValue, cboYears.SelectedValue);
            var dtf = CultureInfo.CurrentCulture.DateTimeFormat;
            string query = string.Format("Select * from ViewCollectionControl where FinancialperiodID ='{0}'", label22.Text.Trim());

            //string query = string.Format("Select * from ViewCollectionControl where CONVERT(VARCHAR(10),StartDate,103)='{0}' and CONVERT(VARCHAR(10),EndDate,103)='{1}'", string.Format("{0:dd/MM/yyy}", dtpStart.Value), string.Format("{0:dd/MM/yyy}", dtpEnd.Value));

            using (SqlDataAdapter ada = new SqlDataAdapter((string)query, Logic.ConnectionString))
            {
                ada.Fill(ds, "table");
            }


            if (ds.Tables[0].Rows.Count == 0)
            {
                Common.setMessageBox("Sorry No Record to View", Program.ApplicationName, 1);
                return;
            }
            else
            {
                XtraRepReportControl report = new XtraRepReportControl() { DataSource = ds, DataMember = "table" };
                report.xrLabel1.Text = string.Format("{0} STATE GOVERNMENT", Program.StateName.ToUpper());
                report.xrLabel2.Text = string.Format("Bank Reconciliation Statement for the month of {0}", label21.Text);

                report.ShowPreviewDialog();
            }


        }
        //tsbEdit.PerformClick();

        void cboYears_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (cboYears.SelectedValue != null && !isFirst)
            //{
            //    setReload((string)cboPeriods.SelectedValue, (string)cboYears.SelectedValue);
            //}
        }

        void bttnUpdate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty((string)cboBank.SelectedValue))
            {
                Common.setEmptyField("Bank Name", Program.ApplicationName);
                cboBank.Focus(); return;
            }
            //else if (string.IsNullOrEmpty((string)cboYears.SelectedValue))
            //{ Common.setEmptyField("Transaction Year", Program.ApplicationName); cboYears.Focus(); return; }
            //else if (string.IsNullOrEmpty((string)cboPeriods.SelectedValue))
            //{ Common.setEmptyField("Transtion Month", Program.ApplicationName); cboPeriods.Focus(); return; }
            else if (txtAdvance.EditValue == null || string.IsNullOrWhiteSpace(txtAdvance.EditValue.ToString()))
            {
                Common.setEmptyField("Advance RTD/TRD, can't be empty", this.Text); txtAdvance.Focus(); return;
            }
            else if (txtopen.EditValue == null || string.IsNullOrWhiteSpace(txtopen.EditValue.ToString()))
            {
                Common.setEmptyField("Opening Balance, can't be empty", this.Text); txtopen.Focus(); return;
            }
            else if (txtClose.EditValue == null || string.IsNullOrWhiteSpace(txtClose.EditValue.ToString()))
            {
                Common.setEmptyField("Closing Balance, can't be empty", this.Text); txtClose.Focus(); return;
            }
            else if (txtTrans.EditValue == null || string.IsNullOrWhiteSpace(txtTrans.EditValue.ToString()))
            {
                Common.setEmptyField("Transfer, can't be empty", this.Text); txtTrans.Focus(); return;
            }
            else if (txtCharges.EditValue == null || string.IsNullOrWhiteSpace(txtTrans.EditValue.ToString()))
            {
                Common.setEmptyField("Charges, can't be empty", this.Text); txtTrans.Focus(); return;
            }
            //else if (string.IsNullOrEmpty(txtTransfer.Text))
            //{
            //    Common.setEmptyField("Transfer Balance", Program.ApplicationName); txtTransfer.Focus(); return;
            //}
            //else if (string.IsNullOrEmpty(txtOpening.Text))
            //{
            //    Common.setEmptyField("Opening Balance", Program.ApplicationName); txtOpening.Focus(); return;
            //}
            //else if (string.IsNullOrEmpty(txtClosing.Text))
            //{
            //    Common.setEmptyField("Closing Balance", Program.ApplicationName); txtClosing.Focus(); return;
            //}
            //else if (string.IsNullOrEmpty(txtCollections.Text))
            //{
            //    Common.setEmptyField("Closing Balance", Program.ApplicationName); txtCollections.Focus(); return;
            //}
            //else if (string.IsNullOrEmpty(txtCollections.Text))
            //{
            //    Common.setEmptyField("Closing Balance", Program.ApplicationName); txtCollections.Focus(); return;
            //}
            else
            {
                if (!boolIsUpdate)
                {
                    using (SqlConnection db = new SqlConnection(Logic.ConnectionString))
                    {
                        SqlTransaction transaction;

                        db.Open();

                        transaction = db.BeginTransaction();

                        try
                        {
                            string query = string.Format("INSERT INTO [Reconciliation].[tblCollectionControl] ([BankAccountID],[FinancialperiodID],[BankShortCode],[OpenBal],[CloseBal],[TransferBal],[CollectionsBal],[AdvanceBal],ChargesBal) VALUES ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}');", Convert.ToInt32(cboAccount.SelectedValue), label22.Text.Trim(), cboBank.SelectedValue, Convert.ToDouble(txtopen.EditValue), Convert.ToDouble(txtClose.EditValue), Convert.ToDouble(txtTrans.EditValue), Convert.ToDouble(txtCollections.Text), Convert.ToDouble(txtAdvance.EditValue), Convert.ToDouble(txtCharges.EditValue));

                            using (SqlCommand sqlCommand1 = new SqlCommand(query, db, transaction))
                            {
                                sqlCommand1.ExecuteNonQuery();
                            }

                            transaction.Commit();

                        }
                        catch (SqlException sqlError)
                        {

                            transaction.Rollback();
                            Tripous.Sys.ErrorBox(sqlError);
                            return;
                        }
                        db.Close();
                    }
                    //setReload((string)cboPeriods.SelectedValue, (string)cboYears.SelectedValue);
                    Clear();

                    Common.setMessageBox("Record has been successfully added", Program.ApplicationName, 1);

                    if (MessageBox.Show("Do you want to add another record?", Program.ApplicationName, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
                    {

                        return;

                    }
                    else
                    {
                        //bttnReset.PerformClick();
                        setReload();

                        //setReload(); 
                        Clear();
                        //cboBank.Focus();
                    }
                }
                else
                {
                    using (SqlConnection db = new SqlConnection(Logic.ConnectionString))
                    {
                        SqlTransaction transaction;

                        db.Open();

                        transaction = db.BeginTransaction();

                        try
                        {

                            string query = String.Format(String.Format("UPDATE [tblCollectionControl] SET [OpenBal]='{{0}}',[CloseBal]='{{1}}',[TransferBal]='{{2}}',[CollectionsBal]='{{3}}',[ChargesBal]='{{4}}' where  BankCode ='{0}'and CONVERT(VARCHAR(10),StartDate,103)='{1}'", ID, Convert.ToDouble(txtOpening.Text), Convert.ToDouble(txtClosing.Text), Convert.ToDouble(txtTransfer.Text), Convert.ToDouble(txtCollections.Text), Convert.ToDouble(txtCharges.Text)));

                            using (SqlCommand sqlCommand1 = new SqlCommand(query, db, transaction))
                            {
                                sqlCommand1.ExecuteNonQuery();
                            }

                            transaction.Commit();
                        }
                        catch (SqlException sqlError)
                        {

                            transaction.Rollback();
                            Tripous.Sys.ErrorBox(sqlError);
                            return;
                        }
                    }

                }
            }
        }

        void txtTransfer_LostFocus(object sender, EventArgs e)
        {

        }

        void txtOpening_LostFocus(object sender, EventArgs e)
        {
            String Text = ((TextBox)sender).Text.Replace(",", "");

            double Num;

            if (double.TryParse(Text, out Num))
            {
                Text = String.Format("{0:N2}", Num);
                ((TextBox)sender).Text = Text;
            }
        }

        void txtClosing_LostFocus(object sender, EventArgs e)
        {
            String Text = ((TextBox)sender).Text.Replace(",", "");

            double Num;

            if (double.TryParse(Text, out Num))
            {
                Text = String.Format("{0:N2}", Num);
                ((TextBox)sender).Text = Text;
            }
        }

        void cboYears_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Methods.AutoComplete(cboYears, e, true);
        }

        void cboPeriods_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Methods.AutoComplete(cboPeriods, e, true);
        }

        void cboBank_KeyPress(object sender, KeyPressEventArgs e)
        {
            Methods.AutoComplete(cboBank, e, true);
        }

        public void RefreshForm()
        {
            NavBars.ToolStripEnableDisableControls(toolStrip, Tag as String);
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
            bttnPay.Image = MDIMains.publicMDIParent.i32x32.Images[34];

            bttnReport.Image = MDIMains.publicMDIParent.i32x32.Images[15];
            bttnIGR.Image = MDIMains.publicMDIParent.i32x32.Images[34];

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
                if (EditRecordMode())
                {
                    ShowForm();
                    boolIsUpdate = true;
                }
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

            //dtpStart.CustomFormat = "dd/MM/yyyy";

            //dtpEnd.CustomFormat = "dd/MM/yyyy";
            dateTimePicker1.CustomFormat = "dd/MM/yyyy";

            setDBComboBoxBank();

            setReload();


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

        public void setDBComboBoxPeriod()
        {
            DataTable Dt;

            using (var ds = new System.Data.DataSet())
            {
                using (SqlDataAdapter ada = new SqlDataAdapter((string)"SELECT DISTINCT months,Periods FROM tblPeriods ORDER BY Periods", Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }

                Dt = ds.Tables[0];
            }

            //Common.setComboList(cboPeriods, Dt, "Periods", "months");



            //cboPeriods.SelectedIndex = -1;


        }

        void setDBComboBoxPeriods()
        {
            DataTable Dt;

            using (var ds = new System.Data.DataSet())
            {
                using (SqlDataAdapter ada = new SqlDataAdapter((string)"SELECT DISTINCT YEAR FROM tblPeriods", Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }

                Dt = ds.Tables[0];
            }

            //Common.setComboList(cboYears, Dt, "YEAR", "YEAR");

            //cboYears.SelectedIndex = -1;
        }

        public void setDBComboBoxBank()
        {
            DataTable Dt;

            //connect.connect.Close();

            using (var ds = new System.Data.DataSet())
            {
                //connect.connect.Open();
                using (SqlDataAdapter ada = new SqlDataAdapter((string)"SELECT * FROM Collection.tblBank", Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }

                Dt = ds.Tables[0];
            }

            Common.setComboList(cboBank, Dt, "BankShortCode", "BankName");

            cboBank.SelectedIndex = -1;


        }

        private void setReload()
        {
            //connect.connect.Close();
            DataTable dt;
            //System.Data.DataSet ds;
            using (var ds = new System.Data.DataSet())
            {
                //query
                string query = string.Format("Select * from ViewCollectionControl where FinancialperiodID ='{0}'", label22.Text.Trim());
                //connect.connect.Open();
                using (SqlDataAdapter ada = new SqlDataAdapter(query, Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }
                //connect.connect.Close();
                dt = ds.Tables[0];
                gridControl1.DataSource = dt.DefaultView;
            }
            gridView1.OptionsBehavior.Editable = false;
            gridView1.Columns["OpenBal"].DisplayFormat.FormatType = FormatType.Numeric;
            gridView1.Columns["OpenBal"].DisplayFormat.FormatString = "n2";
            gridView1.Columns["CloseBal"].DisplayFormat.FormatType = FormatType.Numeric;
            gridView1.Columns["CloseBal"].DisplayFormat.FormatString = "n2";
            //gridView1.Columns["TransferBal"].DisplayFormat.FormatType = FormatType.Numeric;
            //gridView1.Columns["TransferBal"].DisplayFormat.FormatString = "n2";
            gridView1.Columns["CollectionsBal"].DisplayFormat.FormatType = FormatType.Numeric;
            gridView1.Columns["CollectionsBal"].DisplayFormat.FormatString = "n2";


            gridView1.Columns["FinancialperiodID"].Visible = false;
            gridView1.Columns["AdvanceBal"].Visible = false;
            gridView1.Columns["Description"].Visible = false;
            gridView1.Columns["BankName"].Visible = false;
            gridView1.Columns["BankAccountID"].Visible = false;
            //AccountNumber
            //gridView1.Columns["EndDate"].Visible = false;
            gridView1.Columns["CollectionControlID"].Visible = false;
            gridView1.Columns["TransferBal"].Visible = false;
            gridView1.BestFitColumns();
        }

        private void Clear()
        {
            cboBank.SelectedIndex = -1; txtClosing.Text = string.Empty; txtCollections.Text = string.Empty; txtOpening.Text = string.Empty; txtTransfer.Text = string.Empty; cboAccount.SelectedIndex = -1;
            txtAdvance.EditValue = string.Empty; txtClose.EditValue = string.Empty; txtTrans.EditValue = string.Empty; txtopen.EditValue = string.Empty;

        }

        protected bool EditRecordMode()
        {
            bool bResponse = false;
            GridView view = (GridView)gridControl1.FocusedView;
            if (view != null)
            {
                DataRow dr = view.GetDataRow(view.FocusedRowHandle);
                if (dr != null)
                {
                    ID = dr["CollectionControlID"].ToString();
                    bResponse = FillField(dr["CollectionControlID"].ToString());
                }
                else
                {
                    Common.setMessageBox("No Record is seleected", "", 1);
                    bResponse = false;
                }
            }
            return bResponse;
        }

        private bool FillField(string fieldid)
        {
            bool bResponse = false;

            DataTable dts = (new Logic()).getSqlStatement((String.Format("select * from ViewCollectionControl where CollectionControlID ='{0}'", fieldid))).Tables[0];

            if (dts != null)
            {
                bResponse = true;

                cboBank.SelectedValue = dts.Rows[0]["BankName"].ToString();
                //cboBank.Text = dts.Rows[0]["BankName"].ToString();
                //txtClosing.Text = dts.Rows[0]["CloseBal"].ToString();
                //txtOpening.Text = dts.Rows[0]["OpenBal"].ToString();
                //txtTransfer.Text = dts.Rows[0]["TransferBal"].ToString();
                //cboBranch.Text = dts.Rows[0]["BranchName"].ToString();
                txtCollections.Text = String.Format("{0:N2}", dts.Rows[0]["CollectionsBal"]);
                //cboBank.Enabled = false;
                txtAdvance.EditValue = String.Format("{0:N2}", dts.Rows[0]["AdvanceBal"]);
                txtClose.EditValue = String.Format("{0:N2}", dts.Rows[0]["CloseBal"]);
                txtopen.EditValue = String.Format("{0:N2}", dts.Rows[0]["OpenBal"]);
                txtTrans.EditValue = String.Format("{0:N2}", dts.Rows[0]["TransferBal"]);
                //txtAdvance.EditValue = dts.Rows[0]["AdvanceBal"].ToString();
                //txtAdvance.EditValue = dts.Rows[0]["AdvanceBal"].ToString();
                //txtAdvance.EditValue = dts.Rows[0]["AdvanceBal"].ToString();


            }
            else
                bResponse = false;

            return bResponse;
        }

        void setDBComboBoxAcct()
        {
            try
            {
                isRecord = true;

                using (var ds = new System.Data.DataSet())
                {
                    using (SqlDataAdapter ada = new SqlDataAdapter(String.Format("SELECT BankAccountID,AccountNumber FROM ViewCurrencyBankAccount WHERE BankShortCode='{0}'", cboBank.SelectedValue.ToString()), Logic.ConnectionString))
                    {
                        ada.Fill(ds, "table");
                    }

                    Common.setComboList(cboAccount, ds.Tables[0], "BankAccountID", "AccountNumber");

                }

                cboAccount.SelectedIndex = -1; isRecord = false;
            }
            catch (Exception ex)
            {
                Tripous.Sys.ErrorBox(String.Format("{0}{1}", ex.Message, ex.StackTrace)); return;
            }
        }

        void openForm()
        {
            //FrmFinanicalYear fyear = new FrmFinanicalYear();
            //fyear.ShowDialog();

            using (FrmFinanicalYear fyear = new FrmFinanicalYear("Collection"))
            {
                fyear.ShowDialog();
            }
        }

        void getRate()
        {
            dubRate = 0.0;

            DataTable dts = (new Logic()).getSqlStatement((String.Format("SELECT Rate FROM Reconciliation.tblExchangeRate INNER JOIN Reconciliation.tblBankAccount ON Reconciliation.tblBankAccount.CurrencyID = Reconciliation.tblExchangeRate.CurrencyID WHERE BankAccountID='{0}'", cboAccount.SelectedValue))).Tables[0];

            if (dts != null && dts.Rows.Count > 0)
            {
                dubRate = Convert.ToDouble(String.Format("{0:N2}", dts.Rows[0]["Rate"]));
            }
            else
                dubRate = 1;
        }
    }
}
