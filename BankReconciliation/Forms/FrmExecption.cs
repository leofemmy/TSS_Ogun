using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Data.SqlClient;
using BankReconciliation.Class;
using TaxSmartSuite.Class;
using DevExpress.Utils;
using BankReconciliation.Report;
using System.Globalization;
using System.Reflection;
using System.Collections;
using DevExpress.XtraGrid.Selection;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraSplashScreen;
using DevExpress.XtraReports.UI;
using DevExpress.XtraReports.Parameters;

namespace BankReconciliation.Forms
{
    public partial class FrmExecption : Form
    {
        private DataTable DtnotInBoth, DtnotInBothA, DtInBothAs, DtInBoth, DtnotInBothAs, dtEdit, dbmissing, dbmatched, dbmissingpay;

        DataTable Dt; DataTable dt;

        private SqlCommand command;

        private System.Data.DataSet DtSet, DtSet2, dtResult;

        private TransactionTypeCode iTransType;

        private string monthName;

        double deb;

        DataTable dtsreces;

        private bool isRecord = true;

        private bool isRecord2 = true;

        GridCheckMarksSelection selection;

        GridCheckMarksSelection selection2;

        bool isFirstGrid = true;

        bool isFirstGrid2 = true;

        private System.Data.OleDb.OleDbDataAdapter MyCommand;

        private OleDbConnection MyConnection;

        protected bool boolIsUpdate; protected string ID; bool isFirst = true;

        bool Isbank = true;

        bool boolIsUpdate2;

        private SqlCommand _command; private SqlDataAdapter adp; private DataTable Dts;

        private String[] split2;

        string filenamesopen, BatchNumber, AgencyName, AgencyCode, stationName, stationcode, selectedPage, ElemDescr, ElemType, BranchCode, branchname, selectedPages, strStatus, strpayerID, strpaymentRef, strAcct, strAgencyname, StrAgencyCode, strRevenuecode, strDescription, strBranch;

        double openingbal;

        public static FrmExecption publicStreetGroup; private String[] split; private object strmonth;

        string[] Splits;

        double openbal = 0.0;
        DataTable tableTrans = new DataTable();

        DateTime retdate;

        GridColumn colView = new GridColumn();
        GridColumn colView2 = new GridColumn();

        DataTable dtc = new DataTable();
        DataTable dtde = new DataTable();
        DataTable dbDebit = new DataTable();
        DataTable dbcredit = new DataTable();

        //private SqlCommand _command; private SqlDataAdapter adp; private DataTable Dts;

        RepositoryItemComboBox repCombobox = new RepositoryItemComboBox();
        RepositoryItemGridLookUpEdit repComboLookBox = new RepositoryItemGridLookUpEdit();
        RepositoryItemGridLookUpEdit repComboLookBoxCredit = new RepositoryItemGridLookUpEdit();

        //selectedPages = string.Empty;

        public FrmExecption()
        {
            SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

            InitializeComponent();

            publicStreetGroup = this;

            iTransType = TransactionTypeCode.New;

            Load += OnFormLoad;

            ToolStripEvent();

            setImages();

            //setDBComboBoxPay(); stDBComboBoxZonal(); setDBComboBoxRevenue();

            cboBank.SelectedIndexChanged += cboBank_SelectedIndexChanged;

            cboRevenue.SelectedIndexChanged += cboRevenue_SelectedIndexChanged;

            xtraTabControl1.SelectedPageChanged += xtraTabControl1_SelectedPageChanged;

            xtraTabControl2.SelectedPageChanged += xtraTabControl2_SelectedPageChanged;

            cboAcct.SelectedIndexChanged += cboAcct_SelectedIndexChanged;

            cboRevenuetype.SelectedIndexChanged += cboRevenuetype_SelectedIndexChanged;

            radioGroup2.SelectedIndexChanged += radioGroup2_SelectedIndexChanged;

            radioGroup3.SelectedIndexChanged += radioGroup3_SelectedIndexChanged;

            OnFormLoad(null, null);

            //create offline table
            tableTrans.Columns.Add("Date", typeof(string));
            tableTrans.Columns.Add("Transaction Description", typeof(string));
            tableTrans.Columns.Add("Dr", typeof(Decimal));
            tableTrans.Columns.Add("Cr", typeof(Decimal));

            selectedPages = "0002";

            SplashScreenManager.CloseForm(false);

        }

        void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                cboRevenuetype.SelectedIndex = -1;
            }
        }

        void bttnreset_Click(object sender, EventArgs e)
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

                //check if period already closed or not
                string qy = string.Format("SELECT IsClosed FROM dbo.tblPeriods WHERE BankCode='{0}' AND CONVERT(VARCHAR(10),[Start Date],103)='{1}' AND CONVERT(VARCHAR(10),[End Date],103)='{2}'", cboBank.SelectedValue, string.Format("{0:dd/MM/yyy}", dtpStart.Value), string.Format("{0:dd/MM/yyy}", dtpEnd.Value));

                using (var ds = new System.Data.DataSet())
                {
                    using (SqlDataAdapter ada = new SqlDataAdapter(qy, Logic.ConnectionString))
                    {
                        ada.Fill(ds, "table");
                    }

                    Dt = ds.Tables[0];
                }
                string qwy;
                if (Dt != null && Dt.Rows.Count > 0)
                {
                    Common.setMessageBox("Sorry, Period Already Closed, so it Can't be Rest", "Reset Period", 2);
                    return;
                }
                else
                {
                    //using (WaitDialogForm form = new WaitDialogForm("Please Wait...", "Resetting Transactions "))
                    {

                        string quytest = string.Format("SELECT AccountNumber,BranchCode FROM ViewBankBranchAccount where BankShortCode = '{0}'", cboBank.SelectedValue);

                        DataTable dtes = (new Logic()).getSqlStatement((quytest)).Tables[0];

                        if (dtes != null && dtes.Rows.Count > 0)
                        {
                            strAcct = (string)dtes.Rows[0]["AccountNumber"];
                            strBranch = (string)dtes.Rows[0]["BranchCode"];
                        }

                        //qwy = string.Format("DELETE FROM tblbankstatement WHERE BankCode='{0}' AND CONVERT(VARCHAR(10),StartDate,103)='{1}' AND CONVERT(VARCHAR(10),EndDate,103)='{2}'", cboBank.SelectedValue, string.Format("{0:dd/MM/yyy}", dtpStart.Value), string.Format("{0:dd/MM/yyy}", dtpEnd.Value));

                        //deleteRecord(qwy);

                        qwy = string.Format("DELETE FROM tblAllocateCredit WHERE BankCode='{0}' AND CONVERT(VARCHAR(10),StartDate,103)='{1}' AND CONVERT(VARCHAR(10),EndDate,103)='{2}'", cboBank.SelectedValue, string.Format("{0:dd/MM/yyy}", dtpStart.Value), string.Format("{0:dd/MM/yyy}", dtpEnd.Value));

                        deleteRecord(qwy);

                        qwy = string.Format("DELETE FROM tblAllocateDebit WHERE BankCode='{0}' AND CONVERT(VARCHAR(10),StartDate,103)='{1}' AND CONVERT(VARCHAR(10),EndDate,103)='{2}'", cboBank.SelectedValue, string.Format("{0:dd/MM/yyy}", dtpStart.Value), string.Format("{0:dd/MM/yyy}", dtpEnd.Value));

                        deleteRecord(qwy);

                        qwy = string.Format("DELETE FROM tblBankPayDirect WHERE BankCode='{0}' AND CONVERT(VARCHAR(10),StartDate,103)='{1}' AND CONVERT(VARCHAR(10),EndDate,103)='{2}'", cboBank.SelectedValue, string.Format("{0:dd/MM/yyy}", dtpStart.Value), string.Format("{0:dd/MM/yyy}", dtpEnd.Value));

                        deleteRecord(qwy);

                        qwy = string.Format("DELETE FROM tblBothBankPayDirect WHERE BankCode='{0}' AND CONVERT(VARCHAR(10),StartDate,103)='{1}' AND CONVERT(VARCHAR(10),EndDate,103)='{2}'", cboBank.SelectedValue, string.Format("{0:dd/MM/yyy}", dtpStart.Value), string.Format("{0:dd/MM/yyy}", dtpEnd.Value));

                        deleteRecord(qwy);

                        qwy = string.Format("DELETE FROM tblPayDirectBank WHERE BankCode='{0}' AND CONVERT(VARCHAR(10),StartDate,103)='{1}' AND CONVERT(VARCHAR(10),EndDate,103)='{2}'", cboBank.SelectedValue, string.Format("{0:dd/MM/yyy}", dtpStart.Value), string.Format("{0:dd/MM/yyy}", dtpEnd.Value));

                        deleteRecord(qwy);

                        qwy = string.Format("DELETE FROM tblTransactionPosting WHERE AccountNo='{0}' AND CONVERT(VARCHAR(10),StartDate,103)='{1}' AND CONVERT(VARCHAR(10),EndDate,103)='{2}'", strAcct, string.Format("{0:dd/MM/yyy}", dtpStart.Value), string.Format("{0:dd/MM/yyy}", dtpEnd.Value));

                        deleteRecord(qwy);
                    }
                    cboBank.SelectedIndex = -1;
                    Common.setMessageBox("Reset Successfull", "Reset Transaction", 1); return;
                }
            }
            catch (Exception ex)
            {
                Common.setMessageBox(ex.Message + ex.StackTrace, "Error", 1); return;
            }
            finally
            {
                SplashScreenManager.CloseForm(false);

            }
        }

        void bttnNext2_Click(object sender, EventArgs e)
        {
            xtraTabControl1.SelectedTabPage = xtraTabPage3;
        }

        void bttnPostingrec_Click(object sender, EventArgs e)
        {
            if (cbopaymethod.SelectedIndex == -1)
            {
                Common.setEmptyField("Payment Method", "Record Posting");
                cbopaymethod.Focus(); return;
            }
            else if (cbobranch.SelectedIndex == -1)
            {
                Common.setEmptyField("Paid Bank Branch", "Record Posting");
                cbobranch.Focus(); return;
            }
            else
            {

                using (WaitDialogForm form = new WaitDialogForm("Please Wait...", "Posting Transactions "))
                {
                    DataTable dts = (new Logic()).getSqlStatement((String.Format("SELECT Description,Type FROM dbo.tblTransDefinition	WHERE ElementCatCode = '{0}' ", (string)selectedPages))).Tables[0];

                    if (dts != null && dts.Rows.Count > 0)
                    {
                        ElemDescr = (string)dts.Rows[0]["Description"];
                        ElemType = (string)dts.Rows[0]["Type"];
                    }

                    string quytest = string.Format("SELECT AccountNumber,BranchCode FROM ViewBankBranchAccount where BankShortCode like '%{0}%'", cboBank.SelectedValue);

                    DataTable dtes = (new Logic()).getSqlStatement((quytest)).Tables[0];

                    if (dtes != null && dtes.Rows.Count > 0)
                    {
                        strAcct = (string)dtes.Rows[0]["AccountNumber"];
                        strBranch = (string)dtes.Rows[0]["BranchCode"];
                    }

                    BatchNumber = String.Format("{0:d5}", (DateTime.Now.Ticks / 10) % 100000);


                    strpayerID = string.Format("{0}|{1}|{2:yyyMMddhhmmss}", cboBank.SelectedValue, BatchNumber, DateTime.Now);
                    strpaymentRef = String.Format("{0}|OGPRC|{1}|{2:dd-MM-yyyy}|{3}", cboBank.SelectedValue, strBranch, DateTime.Now, string.Format("{0}{1}{2}{3}", DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second, DateTime.Now.Millisecond));

                    string[] Splits1;

                    Splits1 = txtpaymentdate.Text.Split(new Char[] { '/' });

                    string strDate = String.Format("{0}/{1}/{2}", Splits1[1], Splits1[0], Splits1[2]);

                    string query3;
                    //insert record
                    using (SqlConnection db = new SqlConnection(Logic.ConnectionString))
                    {
                        SqlTransaction transaction;

                        db.Open();

                        transaction = db.BeginTransaction();

                        try
                        {
                            if (checkBox1.Checked == true)
                            {
                                query3 = String.Format("INSERT INTO tblCollectionReport([Provider],[Channel],[PaymentRefNumber],[PaymentDate],[PayerID],[Amount],[PaymentMethod],[BankCode],[BankName],[GeneratedBy],[UploadStatus],[ChequeStatus],[IsPayDirect],[IsRecordExit],[ChequeValueDate],[AgencyCode],[AgencyName],[RevenueCode],DESCRIPTION,BranchCode,BranchName,PayerAddress,PayerName) VALUES ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}','{19}','{20}','{21}','{22}');", "ICMA", "Bank", strpaymentRef, strDate, strpayerID, Convert.ToDecimal(txtpstamount.Text), cbopaymethod.Text, cboBank.SelectedValue, cboBank.Text, Program.UserID, "Waiting", "Cleared", false, true, strDate, null, null, null, null, cbobranch.SelectedValue, cbobranch.Text, txtpsaddress.Text, txtpsname.Text);
                            }
                            else
                            {
                                query3 = String.Format("INSERT INTO tblCollectionReport([Provider],[Channel],[PaymentRefNumber],[PaymentDate],[PayerID],[Amount],[PaymentMethod],[BankCode],[BankName],[GeneratedBy],[UploadStatus],[ChequeStatus],[IsPayDirect],[IsRecordExit],[ChequeValueDate],[AgencyCode],[AgencyName],[RevenueCode],DESCRIPTION,BranchCode,BranchName,PayerAddress,PayerName) VALUES ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}','{19}','{20}','{21}','{22}');", "ICMA", "Bank", strpaymentRef, strDate, strpayerID, Convert.ToDecimal(txtpstamount.Text), cbopaymethod.Text, cboBank.SelectedValue, cboBank.Text, Program.UserID, "Waiting", "Cleared", false, true, strDate, label56.Text, label55.Text, cboRevenuetype.SelectedValue, cboRevenuetype.Text, cbobranch.SelectedValue, cbobranch.Text, txtpsaddress.Text, txtpsname.Text);
                            }
                            //insert into collection table


                            using (SqlCommand sqlCommand1 = new SqlCommand(query3, db, transaction))
                            {
                                sqlCommand1.ExecuteNonQuery();
                            }

                            //insert receord into the tbltransactionposting
                            string query4 = string.Format("INSERT INTO tblTransactionPosting (AccountNo,Type,TransDate,TransDescription,Dr,Cr,StartDate,EndDate) VALUES ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}');", strAcct, ElemType, strDate, ElemDescr, 0, Convert.ToDecimal(txtpstamount.Text), string.Format("{0:yyyy/MM/dd}", dtpStart.Value), string.Format("{0:yyyy/MM/dd}", dtpEnd.Value));

                            using (SqlCommand sqlCommand = new SqlCommand(query4, db, transaction))
                            {
                                sqlCommand.ExecuteNonQuery();
                            }

                            transaction.Commit();
                        }
                        catch (SqlException sqlError)
                        {
                            transaction.Rollback(); Tripous.Sys.ErrorBox(sqlError);
                            return;
                        }
                        db.Close();
                    }
                }
                Common.setMessageBox("Transaction Posted Successfully..", Program.ApplicationName, 1);
                clearRec();
                return;
            }
        }

        void gridView10_DoubleClick(object sender, EventArgs e)
        {
            GetRecord();
        }

        void cboRevenuetype_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboRevenuetype.SelectedValue != null && !isRecord2)
            {
                getAgency(cboRevenuetype.SelectedValue.ToString());
            }
        }

        void getAgency(string parameter)
        {
            DataTable Dts = new DataTable();

            System.Data.DataSet dataSet3 = new System.Data.DataSet();

            dataSet3.Clear(); Dts.Clear();
            using (SqlDataAdapter ada = new SqlDataAdapter((string)string.Format("SELECT tblAgency.AgencyCode, tblAgency.AgencyName FROM tblAgency INNER JOIN tblRevenueType ON tblAgency.AgencyCode = tblRevenueType.AgencyCode WHERE  (tblRevenueType.RevenueCode = '{0}')", parameter), Logic.ConnectionString))
            {
                ada.Fill(dataSet3, "table");
            }

            Dts = dataSet3.Tables[0];

            if (Dts != null && Dts.Rows.Count > 0)
            {
                label55.Text = String.Format("{0}", Dts.Rows[0]["AgencyName"]);
                label56.Text = String.Format("{0}", Dts.Rows[0]["AgencyCode"]);


            }

        }

        void cboRevenuetype_KeyPress(object sender, KeyPressEventArgs e)
        {
            Methods.AutoComplete(cboRevenuetype, e, true);
        }

        void gridView10_Click(object sender, EventArgs e)
        {

        }

        void GetRecord()
        {
            GridView view = (GridView)gridControl8.FocusedView;

            if (view != null)
            {
                DataRow dr = view.GetDataRow(view.FocusedRowHandle);
                if (dr != null)
                {

                    txtpaymentdate.Text = (string)dr["DATE"];
                    //txtpstamount.Text = dr["CREDIT"].ToString();
                    txtpstamount.Text = String.Format("{0:N2}", dr["CREDIT"]);

                    cboRevenuetype.Focus();
                }
            }
        }

        void clearRec()
        {
            txtpaymentdate.Text = string.Empty; txtpstamount.Text = string.Empty; cboRevenuetype.SelectedIndex = -1; txtpsname.Text = string.Empty; txtpsaddress.Text = string.Empty; txtslip.Text = string.Empty; cbobranch.SelectedIndex = -1; cbopaymethod.SelectedIndex = -1; label55.Text = string.Empty; label56.Text = string.Empty;
        }

        void bttnNext_Click(object sender, EventArgs e)
        {
            xtraTabControl1.SelectedTabPage = xtraTabPage7;
        }

        void bttnMatch_Click(object sender, EventArgs e)
        {
            string values = string.Empty; string values2 = string.Empty;
            string values3 = string.Empty; string values4 = string.Empty; lblSelect.Text = string.Empty; lblSelect2.Text = string.Empty;

            if (selection.SelectedCount == 1 && selection2.SelectedCount == 1)
            {
                for (int i = 0; i < selection.SelectedCount; i++)
                {
                    values = String.Format("{0}", (selection.GetSelectedRow(i) as DataRowView)["DATE"]);
                    values2 = String.Format("{0}", (selection.GetSelectedRow(i) as DataRowView)["CREDIT"]);
                }

                for (int i = 0; i < selection2.SelectedCount; i++)
                {
                    values3 = String.Format("{0}", (selection2.GetSelectedRow(i) as DataRowView)["DATE"]);
                    values4 = String.Format("{0}", (selection2.GetSelectedRow(i) as DataRowView)["CREDIT"]);
                }
                //checking both value
                if (values2 == values4)
                {
                    //adding value from bank statment to both 
                    dbmatched.Rows.Add(values, values2);
                    dbmatched.AcceptChanges();
                }
                else
                {
                    Common.setMessageBox("Selected Record for Matching Not Equall in Value", "Match Record", 2); return;
                }

                DialogResult result = MessageBox.Show(string.Format("You are Moving this record Date {0} and Amount {1:n} with Date {2} and Amount {3:n}. Is this Correct 'Yes / No'", values, values2, values3, values4), Program.ApplicationName, MessageBoxButtons.YesNo);

                if (result == DialogResult.Yes)
                {

                    //deleteing record from in bank statement
                    for (int h = 0; h < dbmissing.Rows.Count; h++)
                    {
                        if (dbmissing.Rows[h]["DATE"].ToString() == values && dbmissing.Rows[h]["CREDIT"].ToString() == values2)
                        {
                            dbmissing.Rows[h].Delete();
                        }
                    }

                    //deleteing record from in paydirect not in bank statemeng
                    for (int h = 0; h < dbmissingpay.Rows.Count; h++)
                    {
                        if (dbmissingpay.Rows[h]["DATE"].ToString() == values3 && dbmissingpay.Rows[h]["CREDIT"].ToString() == values4)
                        {
                            dbmissingpay.Rows[h].Delete();
                        }
                    }

                    dbmissing.AcceptChanges(); dbmissingpay.AcceptChanges();

                    Common.setMessageBox("Record Match Successfully", "Match Record", 1);
                    //reload the gridview of both table

                    if (dbmissing != null && dbmissing.Rows.Count > 0)
                    {
                        gridControl2.DataSource = dbmissing;
                        gridView3.OptionsBehavior.Editable = false;
                        gridView3.Columns["CREDIT"].DisplayFormat.FormatType = FormatType.Numeric;
                        gridView3.Columns["CREDIT"].DisplayFormat.FormatString = "n2";
                        gridView3.Columns["CREDIT"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                        gridView3.Columns["DATE"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
                        gridView3.Columns["DATE"].DisplayFormat.FormatString = "dd/MM/yyyy";
                        gridView3.Columns["CREDIT"].SummaryItem.FieldName = "CREDIT";
                        gridView3.Columns["CREDIT"].SummaryItem.DisplayFormat = "Total = {0:n}";
                        //gridView3.OptionsView.ColumnAutoWidth = false;
                        gridView3.OptionsView.ShowFooter = true;
                        gridView3.BestFitColumns();
                    }

                    if (dbmissingpay != null && dbmissingpay.Rows.Count > 0)
                    {
                        gridControl4.DataSource = dbmissingpay;
                        gridView5.OptionsBehavior.Editable = false;
                        gridView5.Columns["CREDIT"].DisplayFormat.FormatType = FormatType.Numeric;
                        gridView5.Columns["CREDIT"].DisplayFormat.FormatString = "n2";
                        gridView5.Columns["CREDIT"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                        gridView5.Columns["DATE"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
                        gridView5.Columns["DATE"].DisplayFormat.FormatString = "dd/MM/yyyy";
                        gridView5.Columns["CREDIT"].SummaryItem.FieldName = "CREDIT";
                        gridView5.Columns["CREDIT"].SummaryItem.DisplayFormat = "Total = {0:n}";
                        //gridView5.OptionsView.ColumnAutoWidth = false;
                        gridView5.OptionsView.ShowFooter = true;

                        gridView5.BestFitColumns();
                    }

                    if (dbmatched != null && dbmatched.Rows.Count > 0)
                    {
                        gridControl3.DataSource = dbmatched;
                        gridView4.OptionsBehavior.Editable = false;
                        gridView4.Columns["CREDIT"].DisplayFormat.FormatType = FormatType.Numeric;
                        gridView4.Columns["CREDIT"].DisplayFormat.FormatString = "n2";
                        gridView4.Columns["CREDIT"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                        gridView4.Columns["DATE"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
                        gridView4.Columns["DATE"].DisplayFormat.FormatString = "dd/MM/yyyy";
                        gridView4.Columns["CREDIT"].SummaryItem.FieldName = "CREDIT";
                        gridView4.Columns["CREDIT"].SummaryItem.DisplayFormat = "Total = {0:n}";
                        //gridView4.OptionsView.ColumnAutoWidth = false;
                        gridView4.OptionsView.ShowFooter = true;

                        gridView4.BestFitColumns();
                    }

                    if (isFirstGrid)
                    {
                        selection = new GridCheckMarksSelection(gridView3, ref lblSelect);
                        selection.CheckMarkColumn.VisibleIndex = 0;
                        isFirstGrid = false;
                    }
                    if (isFirstGrid2)
                    {
                        selection2 = new GridCheckMarksSelection(gridView5, ref lblSelect2);
                        selection2.CheckMarkColumn.VisibleIndex = 0;
                        isFirstGrid2 = false;
                    }
                    //isFirstGrid = true; isFirstGrid2 = true;
                    selection.ClearSelection(); selection2.ClearSelection();
                    CalClose();
                }
                else
                    return;

            }
            else
            {
                Common.setMessageBox("Only One Can Be Match at a time", "Match Record", 1); return;
            }
        }

        void dtpEnd_ValueChanged(object sender, EventArgs e)
        {
            label43.Text = dtpEnd.Value.ToLongDateString();
        }

        void dtpStart_ValueChanged(object sender, EventArgs e)
        {
            label42.Text = dtpStart.Value.ToLongDateString();
        }

        public void setDBComboBoxRev()
        {
            DataTable Dt;

            //connect.connect.Close();

            using (var ds = new System.Data.DataSet())
            {
                //connect.connect.Open();
                using (SqlDataAdapter ada = new SqlDataAdapter((string)"SELECT RevenueCode,Description FROM tblRevenueType", Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }

                Dt = ds.Tables[0];
            }

            Common.setComboList(cboRevenuetype, Dt, "RevenueCode", "Description");

            cboRevenuetype.SelectedIndex = -1;


        }

        void setDBComboxPaymode()
        {
            DataTable Dt;

            //connect.connect.Close();

            using (var ds = new System.Data.DataSet())
            {
                //connect.connect.Open();
                using (SqlDataAdapter ada = new SqlDataAdapter((string)"SELECT PayID,Description FROM tblPayMode", Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }

                Dt = ds.Tables[0];
            }

            Common.setComboList(cbopaymethod, Dt, "PayID", "Description");

            cbopaymethod.SelectedIndex = -1;

        }

        void setDBComboxBranchPaid(string parameter)
        {

            DataTable Dt;

            //connect.connect.Close();

            using (var ds = new System.Data.DataSet())
            {
                //connect.connect.Open();
                string qry = string.Format("SELECT BranchName,BranchCode FROM dbo.tblBankBranch WHERE BankShortCode='{0}'", parameter);
                using (SqlDataAdapter ada = new SqlDataAdapter(qry, Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }

                Dt = ds.Tables[0];
            }

            Common.setComboList(cbobranch, Dt, "BranchCode", "BranchName");

            cbobranch.SelectedIndex = -1;

        }

        void dtpEnd_LostFocus(object sender, EventArgs e)
        {

        }

        void dtpStart_LostFocus(object sender, EventArgs e)
        {

        }

        void cboBank_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboBank.SelectedValue != null && !Isbank)
            {

                setDBComboxBranchPaid(cboBank.SelectedValue.ToString());

                DataTable dts = (new Logic()).getSqlStatement((String.Format("SELECT CONVERT(VARCHAR(10),[End Date],103) as [EndDate] FROM tblPeriods WHERE BankCode='{0}'", cboBank.SelectedValue))).Tables[0];

                if (dts != null && dts.Rows.Count > 1)
                {
                    dtpStart.Value = Convert.ToDateTime(dts.Rows[0]["EndDate"]);
                    dtpStart.Enabled = false;
                }
                else
                { }


            }

        }

        void radioGroup3_SelectedIndexChanged(object sender, EventArgs e)
        {
            colView.Name = "Description";
            colView.FieldName = "Description";

            colView2.Name = "Description";
            colView2.FieldName = "Description";

            if (string.IsNullOrEmpty((string)cboBank.SelectedValue))
            {
                Common.setEmptyField("Bank", Program.ApplicationName);
                cboBank.Focus(); return;

            }
            else
            {
                if (radioGroup3.EditValue == "New")
                {
                    //call check to check if already assign before
                    string query = string.Format("SELECT COUNT(*) FROM tblAllocateCredit AC INNER JOIN tblAllocateDebit AD ON AC.BankCode = AD.BankCode WHERE AC.BankCode='{0}' AND CONVERT(VARCHAR(10),AC.StartDate,103)='{1}' AND CONVERT(VARCHAR(10),AC.EndDate,103)='{2}'", cboBank.SelectedValue, string.Format("{0:dd/MM/yyy}", dtpStart.Value), string.Format("{0:dd/MM/yyy}", dtpEnd.Value));

                    try
                    {
                        if (new Logic().IsRecordExist(query))
                        {
                            Common.setMessageBox("Transaction Already been Allocated,Choose Edit Mode to Edit Record", Program.ApplicationName, 1);
                            radioGroup3.SelectedIndex = -1;
                            return;

                        }
                        else
                            setReloadDB(); setReloadCD();
                        boolIsUpdate2 = false;
                    }
                    catch (Exception ex)
                    {
                        Tripous.Sys.ErrorBox(ex); return;
                    }
                }
                if (radioGroup3.EditValue == "Edit")
                {
                    setReloadDebit(); setReloadCredit();
                    boolIsUpdate2 = true;
                }
            }



        }

        void btnAllocate_Click(object sender, EventArgs e)
        {

           
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

                if (!boolIsUpdate2)
                {
                    using (WaitDialogForm form = new WaitDialogForm("Please Wait...", "Allocating Transactions "))
                    {
                        //credit transaction
                        try
                        {
                            using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
                            {
                                connect.Open();
                                _command = new SqlCommand("AllocateTransaction", connect) { CommandType = CommandType.StoredProcedure };
                                _command.Parameters.Add(new SqlParameter("@BankCode", SqlDbType.VarChar)).Value = cboBank.SelectedValue;
                                _command.Parameters.Add(new SqlParameter("@startdate", SqlDbType.VarChar)).Value = string.Format("{0:yyyy/MM/dd}", dtpStart.Value);
                                _command.Parameters.Add(new SqlParameter("@enddate", SqlDbType.VarChar)).Value = string.Format("{0:yyyy/MM/dd}", dtpEnd.Value);
                                _command.Parameters.Add(new SqlParameter("@pType", SqlDbType.Bit)).Value = true;
                                _command.Parameters.Add(new SqlParameter("@pStatus", SqlDbType.Bit)).Value = boolIsUpdate2;
                                _command.Parameters.Add(new SqlParameter("@pTransactiondb", SqlDbType.Structured)).Value = dtc;

                                using (System.Data.DataSet ds = new System.Data.DataSet())
                                {
                                    adp = new SqlDataAdapter(_command);
                                    adp.Fill(ds);
                                    Dts = ds.Tables[0];
                                    connect.Close();
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Tripous.Sys.ErrorBox(ex);
                        }
                        //debit transaction
                        try
                        {
                            using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
                            {
                                connect.Open();
                                _command = new SqlCommand("AllocateTransaction", connect) { CommandType = CommandType.StoredProcedure };
                                _command.Parameters.Add(new SqlParameter("@BankCode", SqlDbType.VarChar)).Value = cboBank.SelectedValue;
                                _command.Parameters.Add(new SqlParameter("@startdate", SqlDbType.VarChar)).Value = string.Format("{0:yyyy/MM/dd}", dtpStart.Value);
                                _command.Parameters.Add(new SqlParameter("@enddate", SqlDbType.VarChar)).Value = string.Format("{0:yyyy/MM/dd}", dtpEnd.Value);
                                _command.Parameters.Add(new SqlParameter("@pType", SqlDbType.Bit)).Value = false;
                                _command.Parameters.Add(new SqlParameter("@pStatus", SqlDbType.Bit)).Value = boolIsUpdate2;
                                _command.Parameters.Add(new SqlParameter("@pTransactiondb", SqlDbType.Structured)).Value = dtde;

                                using (System.Data.DataSet ds = new System.Data.DataSet())
                                {
                                    adp = new SqlDataAdapter(_command);
                                    adp.Fill(ds);
                                    Dts = ds.Tables[0];
                                    connect.Close();

                                    if (ds.Tables[0].Rows[0]["returnCode"].ToString() != "00")
                                    {
                                        Common.setMessageBox(ds.Tables[0].Rows[0]["returnMessage"].ToString(), Program.ApplicationName, 3);
                                    }
                                    else
                                    {
                                        Common.setMessageBox(ds.Tables[0].Rows[0]["returnMessage"].ToString(), Program.ApplicationName, 1);
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Tripous.Sys.ErrorBox(ex);
                        }
                    }
                }
                else
                {
                    using (WaitDialogForm form = new WaitDialogForm("Please Wait...", "Allocating Transactions "))
                    {
                        //credit transaction
                        try
                        {
                            using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
                            {
                                connect.Open();
                                _command = new SqlCommand("AllocateTransaction", connect) { CommandType = CommandType.StoredProcedure };
                                _command.Parameters.Add(new SqlParameter("@BankCode", SqlDbType.VarChar)).Value = cboBank.SelectedValue;
                                _command.Parameters.Add(new SqlParameter("@startdate", SqlDbType.VarChar)).Value = string.Format("{0:yyyy/MM/dd}", dtpStart.Value);
                                _command.Parameters.Add(new SqlParameter("@enddate", SqlDbType.VarChar)).Value = string.Format("{0:yyyy/MM/dd}", dtpEnd.Value);
                                _command.Parameters.Add(new SqlParameter("@pType", SqlDbType.Bit)).Value = true;
                                _command.Parameters.Add(new SqlParameter("@pStatus", SqlDbType.Bit)).Value = boolIsUpdate2;
                                _command.Parameters.Add(new SqlParameter("@pTransactiondb", SqlDbType.Structured)).Value = dbcredit;

                                using (System.Data.DataSet ds = new System.Data.DataSet())
                                {
                                    adp = new SqlDataAdapter(_command);
                                    adp.Fill(ds);
                                    Dts = ds.Tables[0];
                                    connect.Close();
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Tripous.Sys.ErrorBox(ex);
                        }
                        //debit transaction
                        try
                        {
                            using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
                            {
                                connect.Open();
                                _command = new SqlCommand("AllocateTransaction", connect) { CommandType = CommandType.StoredProcedure };
                                _command.Parameters.Add(new SqlParameter("@BankCode", SqlDbType.VarChar)).Value = cboBank.SelectedValue;
                                _command.Parameters.Add(new SqlParameter("@startdate", SqlDbType.VarChar)).Value = string.Format("{0:yyyy/MM/dd}", dtpStart.Value);
                                _command.Parameters.Add(new SqlParameter("@enddate", SqlDbType.VarChar)).Value = string.Format("{0:yyyy/MM/dd}", dtpEnd.Value);
                                _command.Parameters.Add(new SqlParameter("@pType", SqlDbType.Bit)).Value = false;
                                _command.Parameters.Add(new SqlParameter("@pStatus", SqlDbType.Bit)).Value = boolIsUpdate2;
                                _command.Parameters.Add(new SqlParameter("@pTransactiondb", SqlDbType.Structured)).Value = dbDebit;

                                using (System.Data.DataSet ds = new System.Data.DataSet())
                                {
                                    adp = new SqlDataAdapter(_command);
                                    adp.Fill(ds);
                                    Dts = ds.Tables[0];
                                    connect.Close();

                                    if (ds.Tables[0].Rows[0]["returnCode"].ToString() != "00")
                                    {
                                        Common.setMessageBox(ds.Tables[0].Rows[0]["returnMessage"].ToString(), Program.ApplicationName, 3);
                                    }
                                    else
                                    {
                                        Common.setMessageBox(ds.Tables[0].Rows[0]["returnMessage"].ToString(), Program.ApplicationName, 1);
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Tripous.Sys.ErrorBox(ex);
                        }
                    }
                }
            }
            finally
            {
                SplashScreenManager.CloseForm(false);

            }
            //}
        }

        void btnSearch_Click(object sender, EventArgs e)
        {
            colView.Name = "Description";
            colView.FieldName = "Description";

            colView2.Name = "Description";
            colView2.FieldName = "Description";

            if (radioGroup3.EditValue == "New")
            {
                setReloadDB(); setReloadCD();
                boolIsUpdate2 = false;
            }
            if (radioGroup3.EditValue == "Edit")
            {
                setReloadDebit(); setReloadCredit();
                boolIsUpdate2 = true;
            }
        }

        void AddCombDebit()
        {
            DataTable dtsed = new DataTable();

            repComboLookBox.DataSource = null;
            dtsed.Clear();
            //System.Data.DataSet ds;
            using (var ds = new System.Data.DataSet())
            {
                ds.Clear();
                using (SqlDataAdapter ada = new SqlDataAdapter("SELECT Description FROM dbo.tblTransDefinition WHERE Type='dr' AND IsActive=1", Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }

                repComboLookBox.NullText = "select";

                dtsed = ds.Tables[0];

                if (dtsed != null && dtsed.Rows.Count > 0)
                {
                    repComboLookBox.DataSource = dtsed;
                    repComboLookBox.DisplayMember = "Description";
                    repComboLookBox.ValueMember = "Description";
                    repComboLookBox.AllowNullInput = DefaultBoolean.True;

                    repComboLookBox.AutoComplete = true;
                }

            }
        }

        void AddCombCredit()
        {
            DataTable dtse = new DataTable();

            repComboLookBoxCredit.DataSource = null;

            dtse.Clear();
            //System.Data.DataSet ds;
            using (var dsed = new System.Data.DataSet())
            {
                dsed.Clear();

                using (SqlDataAdapter adas = new SqlDataAdapter("SELECT Description  FROM dbo.tblTransDefinition WHERE Type='cr' AND IsActive=1", Logic.ConnectionString))
                {
                    adas.Fill(dsed, "table");

                }

                repComboLookBoxCredit.NullText = "select";

                dtse = dsed.Tables[0];

                if (dtse != null && dtse.Rows.Count > 0)
                {
                    repComboLookBoxCredit.DataSource = dtse;
                    repComboLookBoxCredit.DisplayMember = "Description";
                    repComboLookBoxCredit.ValueMember = "Description";
                    repComboLookBoxCredit.AllowNullInput = DefaultBoolean.True;
                    //Autocomplete on all values
                    repComboLookBoxCredit.AutoComplete = true;
                }

            }
        }

        private void setReloadDB()
        {
            //connect.connect.Close();

            //System.Data.DataSet ds;
            using (var ds = new System.Data.DataSet())
            {
                ds.Clear();
                //connect.connect.Open();
                string qury = string.Format("SELECT CONVERT(VARCHAR, BSDate, 103)  AS Date, Debit AS Amount  FROM dbo.tblbankstatement WHERE Debit IS NOT NULL AND  Debit>0 AND BankCode='{0}' AND CONVERT(VARCHAR(10),StartDate,103)='{1}' AND CONVERT(VARCHAR(10),EndDate,103)='{2}'", cboBank.SelectedValue, string.Format("{0:dd/MM/yyy}", dtpStart.Value), string.Format("{0:dd/MM/yyy}", dtpEnd.Value));

                using (SqlDataAdapter ada = new SqlDataAdapter(qury, Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }
                dtde.Clear();
                //connect.connect.Close();
                dtde = ds.Tables[0];

                dtde.Columns.Add("Description", typeof(String));

                gridControl6.DataSource = dtde;
            }

            AddCombDebit();

            gridView8.Columns["Amount"].DisplayFormat.FormatType = FormatType.Numeric;
            gridView8.Columns["Amount"].DisplayFormat.FormatString = "n2";
            gridView8.Columns["Date"].DisplayFormat.FormatType = FormatType.DateTime;
            gridView8.Columns["Date"].DisplayFormat.FormatString = "dd/MM/yyyy";

            //var col= gridView1.Columns.Add();
            colView = gridView8.Columns["Description"];
            colView.ColumnEdit = repComboLookBox;

            colView.Visible = true;
            //OptionsColumn.AllowEdit = false

            gridView8.Columns["Amount"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;

            gridView8.Columns["Amount"].SummaryItem.FieldName = "Amount";
            gridView8.Columns["Amount"].SummaryItem.DisplayFormat = "Total = {0:n}";

            gridView8.Columns["Amount"].OptionsColumn.AllowEdit = false;
            gridView8.Columns["Date"].OptionsColumn.AllowEdit = false;
            gridView8.OptionsView.ColumnAutoWidth = false;
            gridView8.OptionsView.ShowFooter = true;

            gridView8.BestFitColumns();
        }

        private void setReloadCD()
        {

            //DataTable dtc;
            //setReloadsExtracted();
            //System.Data.DataSet ds;
            using (var dsc = new System.Data.DataSet())
            {
                dsc.Clear();
                //connect.connect.Open();
                string qury = string.Format("SELECT CONVERT(VARCHAR, BSDate, 103)  AS Date, Credit AS Amount  FROM dbo.tblbankstatement WHERE Credit IS NOT NULL AND Credit>0 AND BankCode='{0}' AND CONVERT(VARCHAR(10),StartDate,103)='{1}' AND CONVERT(VARCHAR(10),EndDate,103)='{2}'", cboBank.SelectedValue, string.Format("{0:dd/MM/yyy}", dtpStart.Value), string.Format("{0:dd/MM/yyy}", dtpEnd.Value));

                using (SqlDataAdapter ada = new SqlDataAdapter(qury, Logic.ConnectionString))
                {
                    ada.Fill(dsc, "table");
                }
                //connect.connect.Close();
                dtc.Clear();
                dtc = dsc.Tables[0];

                dtc.Columns.Add("Description", typeof(String));

                gridControl7.DataSource = dtc;
            }

            AddCombCredit();

            gridView9.Columns["Amount"].DisplayFormat.FormatType = FormatType.Numeric;
            gridView9.Columns["Amount"].DisplayFormat.FormatString = "n2";
            gridView9.Columns["Date"].DisplayFormat.FormatType = FormatType.DateTime;
            gridView9.Columns["Date"].DisplayFormat.FormatString = "dd/MM/yyyy";

            //var col= gridView1.Columns.Add();
            colView2 = gridView9.Columns["Description"];
            colView2.ColumnEdit = repComboLookBoxCredit;

            colView2.Visible = true;
            //OptionsColumn.AllowEdit = false

            gridView9.Columns["Amount"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            gridView9.Columns["Amount"].SummaryItem.FieldName = "Amount";
            gridView9.Columns["Amount"].SummaryItem.DisplayFormat = "Total = {0:n}";
            gridView9.Columns["Amount"].OptionsColumn.AllowEdit = false;
            gridView9.Columns["Date"].OptionsColumn.AllowEdit = false;
            gridView9.OptionsView.ColumnAutoWidth = false;
            gridView9.OptionsView.ShowFooter = true;
            gridView9.BestFitColumns();
        }

        void setReloadDebit()
        {
            using (var dsc = new System.Data.DataSet())
            {
                //connect.connect.Open();
                string qury = string.Format("SELECT CONVERT(VARCHAR, TransDate, 103)  AS Date ,TransAmount AS Amount,TransDescription AS Description FROM tblAllocateDebit WHERE TransAmount>0 AND BankCode='{0}' AND CONVERT(VARCHAR(10),StartDate,103)='{1}' AND CONVERT(VARCHAR(10),EndDate,103)='{2}'", cboBank.SelectedValue, string.Format("{0:dd/MM/yyy}", dtpStart.Value), string.Format("{0:dd/MM/yyy}", dtpEnd.Value));

                using (SqlDataAdapter ada = new SqlDataAdapter(qury, Logic.ConnectionString))
                {
                    ada.Fill(dsc, "table");
                }
                //connect.connect.Close();
                dbDebit = dsc.Tables[0];

                //dtc.Columns.Add("Description", typeof(String));

                gridControl6.DataSource = dbDebit;
            }

            AddCombDebit();

            gridView8.Columns["Amount"].DisplayFormat.FormatType = FormatType.Numeric;
            gridView8.Columns["Amount"].DisplayFormat.FormatString = "n2";
            gridView8.Columns["Date"].DisplayFormat.FormatType = FormatType.DateTime;
            gridView8.Columns["Date"].DisplayFormat.FormatString = "dd/MM/yyyy";

            //var col= gridView1.Columns.Add();
            colView = gridView8.Columns["Description"];
            colView.ColumnEdit = repComboLookBox;

            colView.Visible = true;
            //OptionsColumn.AllowEdit = false

            gridView8.Columns["Amount"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;

            gridView8.Columns["Amount"].SummaryItem.FieldName = "Amount";
            gridView8.Columns["Amount"].SummaryItem.DisplayFormat = "Total = {0:n}";
            gridView8.Columns["Amount"].OptionsColumn.AllowEdit = false;
            gridView8.Columns["Date"].OptionsColumn.AllowEdit = false;
            gridView8.OptionsView.ColumnAutoWidth = false;
            gridView8.OptionsView.ShowFooter = true;
            gridView8.BestFitColumns();
        }

        void setReloadCredit()
        {
            using (var dsc = new System.Data.DataSet())
            {
                //connect.connect.Open();
                string qury = string.Format("SELECT CONVERT(VARCHAR, TransDate, 103)  AS Date ,TransAmount AS Amount,TransDescription AS Description FROM tblAllocateCredit WHERE TransAmount>0 AND BankCode='{0}' AND CONVERT(VARCHAR(10),StartDate,103)='{1}' AND CONVERT(VARCHAR(10),EndDate,103)='{2}'", cboBank.SelectedValue, string.Format("{0:dd/MM/yyy}", dtpStart.Value), string.Format("{0:dd/MM/yyy}", dtpEnd.Value));

                using (SqlDataAdapter ada = new SqlDataAdapter(qury, Logic.ConnectionString))
                {
                    ada.Fill(dsc, "table");
                }
                //connect.connect.Close();
                dbcredit = dsc.Tables[0];

                //dtc.Columns.Add("Description", typeof(String));

                gridControl7.DataSource = dbcredit;
            }

            AddCombCredit();

            gridView9.Columns["Amount"].DisplayFormat.FormatType = FormatType.Numeric;
            gridView9.Columns["Amount"].DisplayFormat.FormatString = "n2";
            gridView9.Columns["Date"].DisplayFormat.FormatType = FormatType.DateTime;
            gridView9.Columns["Date"].DisplayFormat.FormatString = "dd/MM/yyyy";

            //var col= gridView1.Columns.Add();
            colView2 = gridView9.Columns["Description"];
            colView2.ColumnEdit = repComboLookBoxCredit;

            colView2.Visible = true;
            //OptionsColumn.AllowEdit = false
            gridView9.Columns["Amount"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            gridView9.Columns["Amount"].SummaryItem.FieldName = "Amount";
            gridView9.Columns["Amount"].SummaryItem.DisplayFormat = "Total = {0:n}";

            gridView9.Columns["Amount"].OptionsColumn.AllowEdit = false;
            gridView9.Columns["Date"].OptionsColumn.AllowEdit = false;
            gridView9.OptionsView.ColumnAutoWidth = false;
            gridView9.OptionsView.ShowFooter = true;
            gridView9.BestFitColumns();
        }

        void bttnPosting_Click(object sender, EventArgs e)
        {
            if (selection.SelectedCount == 0 && dbmissing.Rows.Count > 0)
            {
                Common.setMessageBox("No Selection Made for Record Posting", Program.ApplicationName, 3);

                return;
            }
            else
            {
                AddRecords();
            }

        }

        void linkLabel2_Click(object sender, EventArgs e)
        {
            if (Convert.ToDouble(linkLabel2.Text) != 0)
            {
                DataSet dataSet3 = new DataSet();

                using (SqlDataAdapter ada = new SqlDataAdapter((string)string.Format("SELECT Bankcode, Period, Date, Amount, Years, BankName, AccountNumber, BranchName, BranchCode FROM ViewPayDirectBank WHERE Period='{0}' and BankCode='{1}' and years='{2}'", cboPeriod.SelectedValue, cboBank.SelectedValue, cboYears.SelectedValue), Logic.ConnectionString))
                {
                    ada.Fill(dataSet3, "table");
                }

                //if (dataSet3 !=null && dataSet3)
                XtraRepPayDirectBank rep = new XtraRepPayDirectBank();
                rep.xrLabel10.Text = "List of Transactions in PayDirect but Not in Bank Statement ";

                rep.xrLabel12.Text = string.Format("{0}, {1}", cboPeriod.Text.ToUpper(), cboYears.SelectedValue);


                rep.DataSource = dataSet3;

                rep.DataMember = "table";

                rep.ShowPreviewDialog();
            }
        }

        void LinksClicked(object sender, EventArgs e)
        {
            DataSet dataSet3 = new DataSet();

            string lbltext = string.Empty;

            if (sender == linkLabel2)
            {
                using (SqlDataAdapter ada = new SqlDataAdapter((string)string.Format("SELECT Bankcode, Period, Date, Amount, Years, BankName, AccountNumber, BranchName, BranchCode FROM ViewPayDirectBank WHERE Period='{0}' and BankCode='{1}' and years='{2}'", cboPeriod.SelectedValue, cboBank.SelectedValue, cboYears.SelectedValue), Logic.ConnectionString))
                {
                    ada.Fill(dataSet3, "table");
                }

                lbltext = "List of Transactions in PayDirect but Not in Bank Statement ";
            }
            else if (sender == linkLabel3)
            {
                using (SqlDataAdapter ada = new SqlDataAdapter((string)string.Format("SELECT Bankcode, Period, Date, Amount, Years, BankName, AccountNumber, BranchName, BranchCode FROM ViewBankStatment WHERE Period='{0}' and BankCode='{1}' and years='{2}'", cboPeriod.SelectedValue, cboBank.SelectedValue, cboYears.SelectedValue), Logic.ConnectionString))
                {
                    ada.Fill(dataSet3, "table");
                }

                lbltext = "List of Transactions in Bank Statement ";
            }
            else if (sender == linkLabel4)
            {
                using (SqlDataAdapter ada = new SqlDataAdapter((string)string.Format("SELECT Bankcode, Period, Date, Amount, Years, BankName, AccountNumber, BranchName, BranchCode FROM ViewBankPayDirect WHERE Period='{0}' and BankCode='{1}' and years='{2}'", cboPeriod.SelectedValue, cboBank.SelectedValue, cboYears.SelectedValue), Logic.ConnectionString))
                {
                    ada.Fill(dataSet3, "table");
                }

                lbltext = "List of Transactions in Bank Statement but not in PayDirect ";
            }

            if (dataSet3.Tables[0].Rows.Count > 0)
            {
                XtraRepPayDirectBank rep = new XtraRepPayDirectBank();
                rep.xrLabel10.Text = lbltext;

                rep.xrLabel12.Text = string.Format("{0}, {1}", cboPeriod.Text.ToUpper(), cboYears.SelectedValue);


                rep.DataSource = dataSet3;

                rep.DataMember = "table";

                rep.ShowPreviewDialog();
            }
            else
                Common.setMessageBox("No Records to Preview", Program.ApplicationName, 1);
            return;


        }

        void txtUncleared_LostFocus(object sender, EventArgs e)
        {

            String Text = ((TextBox)sender).Text.Replace(",", "");

            double Num;

            if (double.TryParse(Text, out Num))
            {
                Text = String.Format("{0:N2}", Num);
                ((TextBox)sender).Text = Text;
            }
        }

        void txtClear_LostFocus(object sender, EventArgs e)
        {

            String Text = ((TextBox)sender).Text.Replace(",", "");

            double Num;

            if (double.TryParse(Text, out Num))
            {
                Text = String.Format("{0:N2}", Num);
                ((TextBox)sender).Text = Text;
            }
        }

        void linkLabel1_Click(object sender, EventArgs e)
        {
            DataTable Dts; string varGet;

            //using (WaitDialogForm form = new WaitDialogForm(" Please Wait...", " Loading Collections "))
            //{
            using (var ds = new System.Data.DataSet())
            {
                using (SqlDataAdapter ada = new SqlDataAdapter((string)String.Format("SELECT [Date Interval] FROM dbo.tblPeriods where Periods = '{0}'AND Year= '{1}'", cboPeriod.SelectedValue, cboYears.SelectedValue), Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }
                Dts = ds.Tables[0];

                if (Dts != null && Dts.Rows.Count > 0)
                {
                    varGet = (string)Dts.Rows[0]["Date Interval"];

                    split2 = varGet.Split(new Char[] { '-' });
                }
            }

            XtraRepReconDetails repRec = new XtraRepReconDetails();



            repRec.paramBankCode.Value = cboBank.SelectedValue;
            repRec.paramPeriod.Value = cboYears.SelectedValue;
            repRec.paramMonths.Value = cboPeriod.SelectedValue;
            repRec.paramRecord.Value = true;
            repRec.paramPayDirect.Value = true;

            repRec.xrLabel3.Text = String.Format(" PayDirect Collections between {0} and {1}", split2[0], split2[1]);
            repRec.xrLabel5.Text = String.Format("Bank Name:{0}", cboBank.Text);

            repRec.ShowPreviewDialog();
            //}


        }

        void bttnPeriod_Click(object sender, EventArgs e)
        {
            DataSet dstposted = new DataSet();

            string[] Splits1;
            string[] Splits2;
            //Splits1 = txtDateds.Text.Trim().Split(new Char[] { '/' });

            //string strDate = String.Format("{0}/{1}/{2}", Splits1[1], Splits1[0], Splits1[2]);

            //Splits2 = retdate.ToString().Split(new Char[] { '/' });

            //string strDate2 = String.Format("{0}/{1}/{2}", Splits2[1], Splits2[0], Splits2[2]);

            //string querys = String.Format("SELECT [Date Interval] FROM dbo.tblPeriods WHERE Year='{0}'  AND Periods='{1}' ", cboYears.SelectedValue, cboPeriod.SelectedValue);

            //DataTable dtsPeriod = (new Logic()).getSqlStatement(querys).Tables[0];

            //if (dtsPeriod != null && dtsPeriod.Rows.Count > 0)
            //{
            //    //Splits=dtsPeriod.Tables[0].Rows[0]["[Date Interval]"]
            //    string getvalue = dtsPeriod.Rows[0]["Date Interval"].ToString();

            //    Splits = getvalue.Split(new Char[] { '-' });

            //}
            //Convert.ToDateTime(Splits[0]);

            //Convert.ToDateTime(Splits[1]);

            //string[] Splits12;
            //string[] Splits21;
            //Splits12 = Splits[0].Split(new Char[] { '/' });

            //string strDated = String.Format("{0}/{1}/{2}", Splits12[1].Trim(), Splits12[0].Trim(), Splits12[2].Trim());

            //Splits21 = Splits[1].Split(new Char[] { '/' });

            //string strDateds = String.Format("{0}/{1}/{2}", Splits21[1].Trim(), Splits21[0].Trim(), Splits21[2].Trim());


            string quey = string.Format("SELECT  ID , Provider , Channel ,PaymentRefNumber ,   DepositSlipNumber ,PaymentDate , PayerID ,PayerName , Amount ,PaymentMethod ,ChequeNumber ,ChequeValueDate ,ChequeStatus ,DateChequeReturned ,TelephoneNumber , ReceiptNo , ReceiptDate ,PayerAddress , Status ,[User] ,RevenueCode , Description ,ChequeBankCode , ChequeBankName , AgencyName , AgencyCode , BankCode ,BankName ,BranchCode ,BranchName ,ZoneCode ,ZoneName , Username ,  State , AmountWords , URL , EReceipts ,EReceiptsDate ,GeneratedBy ,DateValidatedAgainst ,DateDiff , UploadStatus ,PrintedBY ,ControlNumber ,BatchNumber ,StationCode ,StationName ,isPrinted ,IsPrintedDate ,CentreCode ,CentreName ,IsNormalize ,NormalizeBy , NormalizeDate ,IsRecordExit , IsPayDirect FROM    tblCollectionReport WHERE ( Provider = 'icma' ) AND ( UploadStatus = 'Waiting' ) AND (CONVERT(VARCHAR,PaymentDate,103) BETWEEN '{0}' AND '{1}' or (CONVERT(VARCHAR, ChequeValueDate, 103) BETWEEN '{0}'  AND '{1}'))", string.Format("{0:dd/MM/yyy}", dtpStart.Value), string.Format("{0:dd/MM/yyy}", dtpEnd.Value));

            using (SqlDataAdapter ada = new SqlDataAdapter((string)quey, Logic.ConnectionString))
            {
                ada.Fill(dstposted, "postedtrans");
            }

            XtraRepPosted posted = new XtraRepPosted() { DataSource = dstposted, DataMember = "postedtrans" };

            posted.xrLabel12.Text = String.Format("List of Posted Transctions for the period of {0}, {1}", cboPeriod.Text, cboYears.SelectedValue);

            posted.xrLabel13.Text = " Bank Name: " + cboBank.Text.Trim();

            posted.ShowPreviewDialog();
        }

        void xtraTabControl2_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            selectedPages = string.Empty;

            if (e.Page.Name == "xtraTabPage4")
            {
                selectedPages = (string)e.Page.Tag;
            }
            else
            {
                selectedPages = (string)e.Page.Tag;
            }
        }

        void bttnClose_Click(object sender, EventArgs e)
        {
            ClosePeriod();
        }

        void txtPayerName_LostFocus(object sender, EventArgs e)
        {
            txtPayerName.Text = txtPayerName.Text.Trim().ToUpper();
            cboRevenue.Focus();
        }

        void txtDateds_LostFocus(object sender, EventArgs e)
        {
            if ((cboPeriod.SelectedValue == null || cboPeriod.SelectedValue == "") && (cboYears.SelectedValue == null || cboYears.SelectedValue == ""))
            {
                Common.setEmptyField(" Transaction Period ", Program.ApplicationName);
                cboPeriod.Focus(); return;
            }
            else
            {
                string querys = String.Format("SELECT [Date Interval] FROM dbo.tblPeriods WHERE Year='{0}'  AND Periods='{1}' ", cboYears.SelectedValue, cboPeriod.SelectedValue);

                DataTable dtsPeriod = (new Logic()).getSqlStatement(querys).Tables[0];

                if (dtsPeriod != null && dtsPeriod.Rows.Count > 0)
                {

                    string getvalue = dtsPeriod.Rows[0]["[Date Interval]"].ToString();

                    Splits = getvalue.Split(new Char[] { '-' });

                }

                if (Convert.ToDateTime(txtDateds.EditValue) >= Convert.ToDateTime(Splits[0]) && Convert.ToDateTime(txtDateds.EditValue) <= Convert.ToDateTime(Splits[1]))
                {
                    strStatus = "Cleared";
                    return;
                }
                else
                {
                    //get transaction date by deducting five day from the day
                    retdate = Convert.ToDateTime(txtDateds.EditValue).AddDays(-5);

                    if (Convert.ToDateTime(retdate.ToString("dd/MM/yyyy")) >= Convert.ToDateTime(Splits[1]) && Convert.ToDateTime(retdate.ToString("dd/MM/yyyy")) <= Convert.ToDateTime(Splits[0]))
                    {
                        Common.setMessageBox(" Date Not Withing the Transaction Period ", Program.ApplicationName, 1);
                        return;
                    }

                    strStatus = "Ucleared";

                    //txtDateds.EditValue = ""; txtDateds.Focus();
                    return;
                }
            }
        }

        void cboZonal_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dts = (new Logic()).getSqlStatement((String.Format("SELECT StationCode,StationName FROM dbo.tblStationMap WHERE RevenueOfficeCode = '{0}' ", cboZonal.SelectedValue))).Tables[0];

            if (dts != null && dts.Rows.Count > 0)
            {
                stationName = dts.Rows[0]["StationName"].ToString();
                stationcode = dts.Rows[0]["StationCode"].ToString();
            }
        }

        void cboRevenue_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dts = (new Logic()).getSqlStatement((String.Format("SELECT AgencyCode,AgencyName FROM ViewAgencyRevenueTemp WHERE RevenueCode = '{0}' ", cboRevenue.SelectedValue))).Tables[0];

            if (dts != null && dts.Rows.Count > 0)
            {
                AgencyName = dts.Rows[0]["AgencyName"].ToString();
                AgencyCode = dts.Rows[0]["AgencyCode"].ToString();

            }
        }

        void btnUpdateEx_Click(object sender, EventArgs e)
        {
            //string[] Splits1;
            //string[] Splits2;

            ////split = cboPeriod.Text.Trim().Split(new Char[] { '/' });

            //Splits1 = txtDateds.Text.Trim().Split(new Char[] { '/' });

            //string strDate = String.Format("{0}/{1}/{2}", Splits1[1], Splits1[0], Splits1[2]);

            //Splits2 = retdate.ToString().Split(new Char[] { '/' });

            //string strDate2 = String.Format("{0}/{1}/{2}", Splits2[1], Splits2[0], Splits2[2]);


            if (string.IsNullOrEmpty(txtDepositAmt.Text))
            {
                Common.setEmptyField("Deposit Amount", Program.ApplicationName);
                txtDepositAmt.Focus(); return;
            }
            else if (string.IsNullOrEmpty(cboAcct.SelectedValue.ToString()))
            {
                Common.setEmptyField("Account No.", Program.ApplicationName);
                cboAcct.Focus(); return;
            }
            //00/0000
            else if (string.IsNullOrEmpty(cboPeriod.SelectedValue.ToString()))
            {
                Common.setEmptyField("Transaction Period", Program.ApplicationName);
                cboPeriod.Focus(); return;
            }
            else if (string.IsNullOrEmpty(txtClosing.Text))
            {
                Common.setEmptyField("Closing Balance for Transaction Period", Program.ApplicationName);
                txtClosing.Focus(); return;
            }
            else if (string.IsNullOrEmpty(txtOpening.Text))
            {
                Common.setEmptyField("Opening Balance for Transaction Period", Program.ApplicationName);
                txtOpening.Focus(); return;

            }
            else
            {
                //get selected element category

                DataTable dts = (new Logic()).getSqlStatement((String.Format("SELECT Description,Type FROM dbo.tblTransDefinition	WHERE ElementCatCode = '{0}' ", (string)selectedPages))).Tables[0];

                if (dts != null && dts.Rows.Count > 0)
                {
                    ElemDescr = (string)dts.Rows[0]["Description"];
                    ElemType = (string)dts.Rows[0]["Type"];
                }
                #region

                //selectedPages = "0002";
                //ChequeStatus

                ////insert into collcetion report table
                //using (SqlConnection db = new SqlConnection(Logic.ConnectionString))
                //{
                //    SqlTransaction transaction;

                //    db.Open();

                //    transaction = db.BeginTransaction();

                //    try
                //    {
                //        string query3 = String.Format("INSERT INTO [tblCollectionReport]([Provider],[Channel],[PaymentRefNumber],[PaymentDate],[PayerName],[PayerID],[RevenueCode],[Description],[Amount],[PaymentMethod],[BankCode],[BankName],[BranchCode],[BranchName],[AgencyName],[AgencyCode],[ZoneCode],[ZoneName],[State],[GeneratedBy],[UploadStatus],[StationCode],[StationName],[ChequeStatus],[IsPayDirect],[IsRecordExit],[ChequeValueDate]) VALUES ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}','{19}','{20}','{21}','{22}','{23}','{24}','{25}','{26}');", "ICM", "Bank", txtPaymentRef.Text.Trim(), strDate, txtPayerName.Text.Trim(), txtPayerID.Text.Trim(), cboRevenue.SelectedValue, cboRevenue.Text.Trim(), txtDepositAmt.Text, cboPayMode.Text, cboBank.SelectedValue, cboBank.Text, BranchCode, branchname, AgencyName, AgencyCode, (string)cboZonal.SelectedValue, (string)cboZonal.Text, Program.StateName, Program.UserID, "Waiting", stationcode, stationName, strStatus, false, true, strDate2);
                //        //Program.UserIDm
                //        using (SqlCommand sqlCommand1 = new SqlCommand(query3, db, transaction))
                //        {
                //            sqlCommand1.ExecuteNonQuery();
                //        }

                //        transaction.Commit();
                //    }
                //    catch (SqlException sqlError)
                //    {
                //        transaction.Rollback(); Tripous.Sys.ErrorBox(sqlError);
                //    }
                //    db.Close();
                //}

                //insert into tblTransactionPost and tblTransactionPostRelation
                #endregion

                if (!boolIsUpdate)
                {

                    tableTrans.Rows.Add(new object[] { txtDateds.Text, ElemDescr, 0, txtDepositAmt.Text });
                    gridControl5.DataSource = tableTrans;
                }
                else
                {
                    double Cr = Convert.ToDouble(txtDepositAmt.Text);
                    dtEdit.Rows.Add(new object[] { txtDateds.Text, ElemDescr, selectedPages, 0, Cr });
                    gridControl5.DataSource = dtEdit;
                }

                Common.setMessageBox(" Transaction Posted Successfully ", Program.ApplicationName, 1);

                clear(); txtDateds.Focus();

            }
        }

        private void clear()
        {
            txtPaymentRef.Clear();
            txtDepositAmt.Text = string.Empty;
            cboRevenue.SelectedIndex = -1;
            cboZonal.SelectedIndex = -1;
            cboPayMode.SelectedIndex = -1;
            //cboCheque.SelectedIndex = -1;
            //txtSlipNo.Clear();
            txtDepName.Clear();
            txtPayerID.Clear();
            txtPayerName.Clear();
            //txtReceiptsNo.Clear(); txtChequeNo.Clear();
            txtDateds.Text = string.Empty;
            //cedAmount.Text = string.Empty;

        }

        void bttnPost_Click(object sender, EventArgs e)
        {
            UpdateRecord();
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

        void txtDepositAmt_LostFocus(object sender, EventArgs e)
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

        void txtAmount_LostFocus(object sender, EventArgs e)
        {
            String Text = ((TextBox)sender).Text.Replace(",", "");

            double Num;

            if (double.TryParse(Text, out Num))
            {
                Text = String.Format("{0:N2}", Num);
                ((TextBox)sender).Text = Text;
            }
        }

        void txtDateE_LostFocus(object sender, EventArgs e)
        {
            if ((cboPeriod.SelectedValue == null || cboPeriod.SelectedValue == "") && (cboYears.SelectedValue == null || cboYears.SelectedValue == ""))
            {
                Common.setEmptyField("Transaction Period", Program.ApplicationName);
                cboPeriod.Focus(); return;
            }
            else
            {
                string querys = String.Format("SELECT [Date Interval] FROM dbo.tblPeriods WHERE Year='{0}'  AND Periods='{1}' ", cboYears.SelectedValue, cboPeriod.SelectedValue);

                DataTable dtsPeriod = (new Logic()).getSqlStatement(querys).Tables[0];

                if (dtsPeriod != null && dtsPeriod.Rows.Count > 0)
                {

                    string getvalue = dtsPeriod.Rows[0]["[Date Interval]"].ToString();

                    Splits = getvalue.Split(new Char[] { '-' });

                }

                if (Convert.ToDateTime(txtDateE.EditValue) >= Convert.ToDateTime(Splits[0]) && Convert.ToDateTime(txtDateE.EditValue) <= Convert.ToDateTime(Splits[1]))
                {
                    return;
                }
                else
                {
                    Common.setMessageBox(" Date Not Withing the Transaction Period ", Program.ApplicationName, 1);
                    txtDateE.EditValue = ""; txtDateE.Focus();
                    return;
                }
            }
        }

        void btnRegister_Click(object sender, EventArgs e)
        {
            BatchNumber = String.Format("{0:d5}", (DateTime.Now.Ticks / 10) % 100000);
            txtPayerID.Visible = true; txtPayerName.Visible = true;
            txtPayerID.Enabled = true; txtPayerName.Enabled = true;
            txtPayerID.Text = string.Format("{0}|{1}|{2:yyyMMddhhmmss}", cboBank.SelectedValue, BatchNumber, DateTime.Now); txtPayerName.Focus();
            txtPaymentRef.Text = String.Format("ICM|{0}|{1:dd-MM-yyyy}|{2}", cboBank.SelectedValue, DateTime.Now, BatchNumber);
        }

        void bttnUpdate_Click(object sender, EventArgs e)
        {
                if (string.IsNullOrEmpty(cboAcct.SelectedValue.ToString()))
                {
                    Common.setEmptyField("Account No.", Program.ApplicationName);
                    cboAcct.Focus(); return;
                }
                else if (string.IsNullOrEmpty(txtClosing.Text))
                {
                    Common.setEmptyField("Closing Balance for Transaction Period", Program.ApplicationName);
                    txtClosing.Focus(); return;
                }
                else if (string.IsNullOrEmpty(txtOpening.Text))
                {
                    Common.setEmptyField("Opening Balance for Transaction Period", Program.ApplicationName);
                    txtOpening.Focus(); return;

                }
                else
                {
                   
                    if (isPeriodClose(cboBank.SelectedValue.ToString(), dtpStart.Value, dtpEnd.Value))
                    {
                        Common.setMessageBox("Sorry, Can't Saved,Transaction Period already Closed?", Program.ApplicationName, 3); return;
                    }
                    else
                    {

                        if (tableTrans.Rows.Count == 0 && tableTrans != null)
                        {
                            ProcessClose();
                        }
                        else
                        {

                            if (!boolIsUpdate)
                            {

                                using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
                                {
                                    connect.Open();
                                    _command = new SqlCommand("InsertUpdatePostRelationTransaction", connect) { CommandType = CommandType.StoredProcedure };
                                    _command.Parameters.Add(new SqlParameter("@AccountNo", SqlDbType.VarChar)).Value = cboAcct.Text.Trim();
                                    _command.Parameters.Add(new SqlParameter("@startdate", SqlDbType.VarChar)).Value = string.Format("{0:yyyy/MM/dd}", dtpStart.Value);
                                    _command.Parameters.Add(new SqlParameter("@enddate", SqlDbType.VarChar)).Value = string.Format("{0:yyyy/MM/dd}", dtpEnd.Value);
                                    _command.Parameters.Add(new SqlParameter("@closeBal", SqlDbType.Money)).Value = Convert.ToDouble(txtClosing.Text.Trim());
                                    _command.Parameters.Add(new SqlParameter("@OpenBal", SqlDbType.Money)).Value = Convert.ToDouble(txtOpening.Text);
                                    _command.Parameters.Add(new SqlParameter("@Isbool", SqlDbType.Bit)).Value = boolIsUpdate;
                                    _command.Parameters.Add(new SqlParameter("@pTransaction", SqlDbType.Structured)).Value = tableTrans;


                                    using (System.Data.DataSet ds = new System.Data.DataSet())
                                    {
                                        adp = new SqlDataAdapter(_command);
                                        adp.Fill(ds);
                                        Dts = ds.Tables[0];
                                        connect.Close();

                                        if (ds.Tables[0].Rows[0]["returnCode"].ToString() != "00")
                                        {
                                            Common.setMessageBox(ds.Tables[0].Rows[0]["returnMessage"].ToString(), Program.ApplicationName, 3);
                                        }
                                        else
                                        {
                                            Common.setMessageBox(ds.Tables[0].Rows[0]["returnMessage"].ToString(), Program.ApplicationName, 3);
                                        }

                                    }
                                }
                            }
                            else
                            {

                                using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
                                {
                                    connect.Open();
                                    _command = new SqlCommand("InsertUpdatePostRelationTransaction", connect) { CommandType = CommandType.StoredProcedure };
                                    _command.Parameters.Add(new SqlParameter("@AccountNo", SqlDbType.VarChar)).Value = cboAcct.Text.Trim();
                                    _command.Parameters.Add(new SqlParameter("@startdate", SqlDbType.VarChar)).Value = string.Format("{0:yyyy/MM/dd}", dtpStart.Value);
                                    _command.Parameters.Add(new SqlParameter("@enddate", SqlDbType.VarChar)).Value = string.Format("{0:yyyy/MM/dd}", dtpEnd.Value);
                                    _command.Parameters.Add(new SqlParameter("@closeBal", SqlDbType.Money)).Value = Convert.ToDouble(txtClosing.Text.Trim());
                                    _command.Parameters.Add(new SqlParameter("@OpenBal", SqlDbType.Money)).Value = Convert.ToDouble(txtOpening.Text.Trim());
                                    _command.Parameters.Add(new SqlParameter("@Isbool", SqlDbType.Bit)).Value = boolIsUpdate;
                                    _command.Parameters.Add(new SqlParameter("@Years", SqlDbType.Int)).Value = cboYears.SelectedValue;
                                    _command.Parameters.Add(new SqlParameter("@pTransaction", SqlDbType.Structured)).Value = dtEdit;
                                    
                                    using (System.Data.DataSet ds = new System.Data.DataSet())
                                    {
                                        adp = new SqlDataAdapter(_command);
                                        adp.Fill(ds);
                                        Dts = ds.Tables[0];
                                        connect.Close();

                                        if (ds.Tables[0].Rows[0]["returnCode"].ToString() != "00")
                                        {
                                            Common.setMessageBox(ds.Tables[0].Rows[0]["returnMessage"].ToString(), Program.ApplicationName, 1);

                                        }
                                        else
                                        {
                                            Common.setMessageBox(ds.Tables[0].Rows[0]["returnMessage"].ToString(), Program.ApplicationName, 1);
                                        }

                                    }
                                }
                            }
                        }
                        ProcessClose();
                    }
                    //ProcessClose();
                    Common.setMessageBox("Transaction Posted Successfully", Program.ApplicationName, 1); return;
                }

            
        }

        void radioGroup2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (radioGroup2.EditValue == null)
            {
                return;
            }
            else
            {
                //groupBox2.Enabled = true;

                setDBComboBoxTrans(radioGroup2.EditValue.ToString());
            }

        }

        public void setDBComboBoxTrans(string parameter)
        {
            DataTable Dt;

            //connect.connect.Close();

            using (var ds = new System.Data.DataSet())
            {
                //connect.connect.Open();
                string query = String.Format("select * from tblTransDefinition where type = '{0}' and IsActive='1' and ElementCatCode<>'0002'", parameter);

                using (SqlDataAdapter ada = new SqlDataAdapter(query, Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }

                Dt = ds.Tables[0];
            }

            Common.setComboList(cboTransType, Dt, "transid", "DESCRIPTION");

            cboTransType.SelectedIndex = -1;


        }

        void bttnCollection_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty((string)(cboBank.SelectedValue)))
            {
                Common.setEmptyField("BanK Name", Program.ApplicationName);
                cboBank.Focus();
                return;
            }
            else if (cboPeriod.SelectedValue == null || cboPeriod.SelectedValue == "")
            {
                Common.setEmptyField("Transaction Period", Program.ApplicationName);
                cboPeriod.Focus();
                return;
            }
            else if (cboYears.SelectedValue == null || cboYears.SelectedValue == "")
            {
                Common.setEmptyField("Transaction Year", Program.ApplicationName);
                cboYears.Focus(); return;
            }
            else
            {
                string strper = String.Format("{0}/{1}", cboPeriod.SelectedValue, cboYears.SelectedValue);

                using (FrmCollectView collectview = new FrmCollectView(cboBank.SelectedValue.ToString(), strper))
                {
                    collectview.ShowDialog();

                    double collectb = Convert.ToDouble(linkLabel1.Text);

                    string dub = string.Format("{0:n2}", collectview.getTotal);

                    //linkLabel1.Text = string.Format("{0:n2}", collectview.getTotal);

                    //label9.Text = string.Format("{0:n2}", (collectb - Convert.ToDouble(dub)));

                }

                GetPeriodCollection(cboBank.SelectedValue.ToString());
            }


        }

        void cboAcct_SelectedIndexChanged(object sender, EventArgs e)
        {


            if (cboAcct.SelectedValue != null && !isRecord)
            {
                string strper = String.Format("{0}/{1}", cboPeriod.SelectedValue, cboYears.SelectedValue);

                GetPeriodCollection(cboBank.SelectedValue.ToString());
                GetAcctInfor((string)cboAcct.Text);
                GetPayDirect(); GetPeriodRec();
                GetPerviousBalance();
            }
            else
            {
                txtOpening.Text = string.Empty;
                txtClosing.Text = string.Empty;
                //txtUncleared.Text = string.Empty;
                //txtClear.Text = string.Empty;
                gridControl5.DataSource = null;
                label34.Text = string.Empty;
                linkLabel1.Text = string.Empty;
                linkLabel2.Text = string.Empty;
                linkLabel3.Text = string.Empty;
                linkLabel4.Text = string.Empty;
            }
        }

        void xtraTabControl1_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {

            selectedPage = string.Empty;

            if (e.Page.Name == "xtraTabPage3")
            {
                selectedPage = (string)e.Page.Tag;

                if (string.IsNullOrEmpty((string)(cboBank.SelectedValue)))
                {
                    Common.setEmptyField("BanK Name", Program.ApplicationName);
                    cboBank.Focus();
                    return;
                }
                else
                {
                    setDBComboBox();

                }
            }

            if (e.Page.Name == "xtraTabPage7")
            {
                if (dbmissing != null && dbmissing.Rows.Count > 0)
                {
                    gridControl8.DataSource = dbmissing;
                    gridView10.OptionsBehavior.Editable = false;
                    gridView10.Columns["CREDIT"].DisplayFormat.FormatType = FormatType.Numeric;
                    gridView10.Columns["CREDIT"].DisplayFormat.FormatString = "n2";
                    gridView10.Columns["CREDIT"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                    gridView10.Columns["DATE"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
                    gridView10.Columns["DATE"].DisplayFormat.FormatString = "dd/MM/yyyy";
                    gridView10.Columns["CREDIT"].SummaryItem.FieldName = "CREDIT";
                    gridView10.Columns["CREDIT"].SummaryItem.DisplayFormat = "Total = {0:n}";
                    //gridView3.OptionsView.ColumnAutoWidth = false;
                    gridView10.OptionsView.ShowFooter = true;
                    gridView10.BestFitColumns();
                }
                else
                {

                }

            }
        }

        public void setDBComboBox()
        {
            DataTable Dt;

            //connect.connect.Close();

            if (string.IsNullOrEmpty(cboBank.SelectedValue.ToString()))
            {
                Common.setEmptyField("Bank Name", "Loading Bank Info");
                return;
            }
            else
            {

                using (var ds = new System.Data.DataSet())
                {
                    //connect.connect.Open();
                    string query = string.Format("SELECT BankAccountID,AccountNumber FROM ViewBankBranchAccount where BankShortCode = '{0}' and Status=1", cboBank.SelectedValue);
                    using (SqlDataAdapter ada = new SqlDataAdapter(query, Logic.ConnectionString))
                    {
                        ada.Fill(ds, "table");
                    }

                    Dt = ds.Tables[0];
                }

                Common.setComboList(cboAcct, Dt, "BankAccountID", "AccountNumber");

                cboAcct.SelectedIndex = -1;
            }

        }

        void xtraTabControl1_Click(object sender, EventArgs e)
        {
            //if (xtraTabControl1.TabPages.Name == xtraTabPage3)
            //{
            //    if (string.IsNullOrEmpty((string)(cboBank.SelectedValue)))
            //    {
            //        Common.setEmptyField("BanK Name", Program.ApplicationName);
            //        cboBank.Focus();
            //        return;
            //    }
            //    else if (cboPeriod.SelectedValue == null || cboPeriod.SelectedValue == "")
            //    {
            //        Common.setEmptyField("Transaction Period", Program.ApplicationName);
            //        cboPeriod.Focus();
            //        return;
            //    }
            //    else if (cboYears.SelectedValue == null || cboYears.SelectedValue == "")
            //    {
            //        Common.setEmptyField("Transaction Year", Program.ApplicationName);
            //        cboYears.Focus(); return;
            //    }
            //    else
            //    {
            //        DataTable dts = (new Logic()).getSqlStatement((String.Format("SELECT OpenBal FROM tblTransactionPosting WHERE Period = '{0}' AND AccountNo= '{1}' ", (string)cboPeriod.Text, (string)cboAcct.Text.Trim()))).Tables[0];

            //        if (dts != null && dts.Rows.Count > 0)
            //        {

            //            //if (result == DialogResult.Yes)
            //            //{
            //            txtOpening.Text = String.Format("{0:N2}", dts.Rows[0]["OpenBal"]); return;

            //            //}


            //        }
            //    }
            //}

        }

        void bttnPreview_Click(object sender, EventArgs e)
        {
            var dtf = CultureInfo.CurrentCulture.DateTimeFormat;

            if (radioGroup1.EditValue == null) return;

            if (Convert.ToInt32(radioGroup1.EditValue) == 0)
            {
                DataSet dataSet1 = new DataSet();

                string query = string.Format("SELECT BankCode, CONVERT(VARCHAR(10),Date,103) as Date, Amount, AccountNumber, BankName, BranchName, BranchCode FROM ViewBankPayDirect WHERE CONVERT(VARCHAR(10),StartDate,103)='{0}' AND CONVERT(VARCHAR(10),EndDate,103)='{1}'  and BankCode='{2}' ORDER BY CONVERT(VARCHAR(10),Date,103)", string.Format("{0:dd/MM/yyy}", dtpStart.Value), string.Format("{0:dd/MM/yyy}", dtpEnd.Value), cboBank.SelectedValue);

                using (SqlDataAdapter ada = new SqlDataAdapter((string)query, Logic.ConnectionString))
                {
                    ada.Fill(dataSet1, "table");
                }

                if (dataSet1.Tables[0].Rows.Count == 0)
                {
                    Common.setMessageBox("No Record For Not in PayDirect", Program.ApplicationName, 1);
                    return;
                }
                else
                {
                    XtraRepBankPayDirect report = new XtraRepBankPayDirect();

                    report.xrLabel10.Text = "List of Transactions in Bank Statment but not in PayDirect ";

                    report.xrLabel12.Text = string.Format("{0}, {1}", dtf.GetMonthName(dtpStart.Value.Month), dtpStart.Value.Year);

                    report.DataSource = dataSet1;

                    report.DataMember = "table";

                    report.ShowPreviewDialog();
                }



            }
            else if (Convert.ToInt32(radioGroup1.EditValue) == 1)
            {
                DataSet dataSet2 = new DataSet();

                using (SqlDataAdapter ada = new SqlDataAdapter((string)string.Format("SELECT ID, Bankcode,  CONVERT(VARCHAR(10),Date,103) as Date, Amount, BankName, BranchName, AccountNumber, BranchCode FROM ViewBothBankPayDirect WHERE CONVERT(VARCHAR(10),StartDate,103)='{0}' AND CONVERT(VARCHAR(10),EndDate,103)='{1}' and BankCode='{2}' ORDER BY CONVERT(VARCHAR(10),Date,103)", string.Format("{0:dd/MM/yyy}", dtpStart.Value), string.Format("{0:dd/MM/yyy}", dtpEnd.Value), cboBank.SelectedValue), Logic.ConnectionString))
                {
                    ada.Fill(dataSet2, "table");
                }

                if (dataSet2.Tables[0].Rows.Count == 0)
                {
                    Common.setMessageBox("No Record For In Both", Program.ApplicationName, 1);
                    return;
                }
                else
                {
                    XtraRepBothBankPaydirect reports = new XtraRepBothBankPaydirect();

                    reports.xrLabel11.Text = "List of Transactions in Both PayDirect and Bank Statement ";

                    reports.xrLabel12.Text = string.Format("{0}, {1}", dtf.GetMonthName(dtpStart.Value.Month), dtpStart.Value.Year);

                    reports.DataSource = dataSet2;

                    reports.DataMember = "table";

                    reports.ShowPreviewDialog();
                }
            }
            else if (Convert.ToInt32(radioGroup1.EditValue) == 2)
            {
                DataSet dataSet3 = new DataSet();

                using (SqlDataAdapter ada = new SqlDataAdapter((string)string.Format("SELECT Bankcode, CONVERT(VARCHAR(10),Date,103) as Date, Amount, BankName, AccountNumber, BranchName, BranchCode FROM ViewPayDirectBank WHERE CONVERT(VARCHAR(10),StartDate,103)='{0}' AND CONVERT(VARCHAR(10),EndDate,103)='{1}' and BankCode='{2}'  ORDER BY CONVERT(VARCHAR(10),Date,103)", string.Format("{0:dd/MM/yyy}", dtpStart.Value), string.Format("{0:dd/MM/yyy}", dtpEnd.Value), cboBank.SelectedValue), Logic.ConnectionString))
                {
                    ada.Fill(dataSet3, "table");
                }

                if (dataSet3.Tables[0].Rows.Count == 0)
                {
                    Common.setMessageBox("No Record For In Not in Bank", Program.ApplicationName, 1);
                    return;
                }
                else
                {
                    //if (dataSet3 !=null && dataSet3)
                    XtraRepPayDirectBank rep = new XtraRepPayDirectBank();
                    rep.xrLabel10.Text = "List of Transactions in PayDirect but Not in Bank Statement ";

                    rep.xrLabel12.Text = string.Format("{0}, {1}", dtf.GetMonthName(dtpStart.Value.Month), dtpStart.Value.Year);


                    rep.DataSource = dataSet3;

                    rep.DataMember = "table";

                    rep.ShowPreviewDialog();
                }

            }
        }

        void bttnSave_Click(object sender, EventArgs e)
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

                if (string.IsNullOrEmpty((string)(cboBank.SelectedValue)))
                {
                    Common.setEmptyField("BanK Name", Program.ApplicationName);
                    cboBank.Focus();
                    return;
                }
                else if (dbmissing == null || dbmatched == null || dbmissingpay == null)
                {
                    Common.setEmptyField("No Comparing Data Result Yet...", Program.ApplicationName);
                    return;
                }
                else
                {

                    //insert into paydirect table
                    if (dbmissingpay != null && dbmissingpay.Rows.Count > 0)
                    {
                        using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
                        {
                            connect.Open();
                            _command = new SqlCommand("InsertPayDirectBank", connect) { CommandType = CommandType.StoredProcedure };
                            _command.Parameters.Add(new SqlParameter("@bankcode", SqlDbType.VarChar)).Value = cboBank.SelectedValue;
                            _command.Parameters.Add(new SqlParameter("@startdate", SqlDbType.VarChar)).Value = string.Format("{0:yyyy/MM/dd}", dtpStart.Value);
                            _command.Parameters.Add(new SqlParameter("@enddate", SqlDbType.VarChar)).Value = string.Format("{0:yyyy/MM/dd}", dtpEnd.Value);
                            _command.Parameters.Add(new SqlParameter("@pTransaction", SqlDbType.Structured)).Value = dbmissingpay;

                            using (System.Data.DataSet ds = new System.Data.DataSet())
                            {
                                adp = new SqlDataAdapter(_command);
                                adp.Fill(ds);
                                Dts = ds.Tables[0];
                                connect.Close();

                            }
                        }
                    }
                    //insert into both table
                    if (dbmatched != null && dbmatched.Rows.Count > 0)
                    {
                        using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
                        {
                            connect.Open();
                            _command = new SqlCommand("InsertBothBankPayDirect", connect) { CommandType = CommandType.StoredProcedure };
                            _command.Parameters.Add(new SqlParameter("@bankcode", SqlDbType.VarChar)).Value = cboBank.SelectedValue;
                            _command.Parameters.Add(new SqlParameter("@startdate", SqlDbType.VarChar)).Value = string.Format("{0:yyyy/MM/dd}", dtpStart.Value);
                            _command.Parameters.Add(new SqlParameter("@enddate", SqlDbType.VarChar)).Value = string.Format("{0:yyyy/MM/dd}", dtpEnd.Value);
                            _command.Parameters.Add(new SqlParameter("@pTransaction", SqlDbType.Structured)).Value = dbmatched;

                            using (System.Data.DataSet ds = new System.Data.DataSet())
                            {
                                adp = new SqlDataAdapter(_command);
                                adp.Fill(ds);
                                Dts = ds.Tables[0];
                                connect.Close();

                                if (String.Compare(ds.Tables[0].Rows[0]["returnCode"].ToString(), "00", false) != 0)
                                {
                                    Common.setMessageBox(ds.Tables[0].Rows[0]["returnMessage"].ToString(), Program.ApplicationName, 2);
                                    //isError = true;
                                    //goto Map;
                                }
                                else
                                {
                                    Common.setMessageBox(ds.Tables[0].Rows[0]["returnMessage"].ToString(), Program.ApplicationName, 2);
                                }

                            }
                        }
                    }
                    //bank statement
                    if (dbmissing != null && dbmissing.Rows.Count > 0)
                    {
                        using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
                        {
                            connect.Open();
                            _command = new SqlCommand("InsertBankPayDirect", connect) { CommandType = CommandType.StoredProcedure };
                            _command.Parameters.Add(new SqlParameter("@bankcode", SqlDbType.VarChar)).Value = cboBank.SelectedValue;
                            _command.Parameters.Add(new SqlParameter("@startdate", SqlDbType.VarChar)).Value = string.Format("{0:yyyy/MM/dd}", dtpStart.Value);
                            _command.Parameters.Add(new SqlParameter("@enddate", SqlDbType.VarChar)).Value = string.Format("{0:yyyy/MM/dd}", dtpEnd.Value);
                            _command.Parameters.Add(new SqlParameter("@pTransaction", SqlDbType.Structured)).Value = dbmissing;

                            using (System.Data.DataSet ds = new System.Data.DataSet())
                            {
                                adp = new SqlDataAdapter(_command);
                                adp.Fill(ds);
                                Dts = ds.Tables[0];
                                connect.Close();

                                if (ds.Tables[0].Rows[0]["returnCode"].ToString() != "00")
                                {
                                    Common.setMessageBox(ds.Tables[0].Rows[0]["returnMessage"].ToString(), Program.ApplicationName, 2);
                                    //isError = true;
                                    //goto Map;
                                }
                                else
                                {
                                    Common.setMessageBox(ds.Tables[0].Rows[0]["returnMessage"].ToString(), Program.ApplicationName, 2);
                                }

                            }
                        }
                    }
                    groupBox4.Enabled = true;

                    //bttnPosting.Enabled = true;
                    bttnNext.Enabled = true;
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                SplashScreenManager.CloseForm(false);

            }
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
                //ShowForm();
                //boolIsUpdate = false;
            }
            else if (sender == tsbEdit)
            {
                //groupControl2.Text = "Edit Record Mode";
                iTransType = TransactionTypeCode.Edit;
                //if (EditRecordMode())
                //{
                //ShowForm();
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

                //ShowForm();
            }
            //bttnReset.PerformClick();
        }

        private void setImages()
        {

            tsbNew.Image = MDIMains.publicMDIParent.i16x16.Images[5];
            tsbEdit.Image = MDIMains.publicMDIParent.i16x16.Images[6];
            tsbDelete.Image = MDIMains.publicMDIParent.i16x16.Images[7];
            tsbReload.Image = MDIMains.publicMDIParent.i16x16.Images[0];
            tsbClose.Image = MDIMains.publicMDIParent.i16x16.Images[11];

            bttnSave.Image = MDIMains.publicMDIParent.i32x32.Images[28];
            btncompare.Image = MDIMains.publicMDIParent.i32x32.Images[25];
            //bttnCancel.Image = MDIMains.publicMDIParent.i32x32.Images[9];
            bttnUpdate.Image = MDIMains.publicMDIParent.i32x32.Images[7];
            btnUpdateEx.Image = MDIMains.publicMDIParent.i32x32.Images[7];
            ////bttnReset.Image = MDIMains.publicMDIParent.i32x32.Images[8];
            bttnPost.Image = MDIMains.publicMDIParent.i32x32.Images[34];
            bttnClose.Image = MDIMains.publicMDIParent.i16x16.Images[9];
            btnAllocate.Image = MDIMains.publicMDIParent.i32x32.Images[7];
            bttnPostingrec.Image = MDIMains.publicMDIParent.i32x32.Images[7];
            bttnPosting.Image = MDIMains.publicMDIParent.i32x32.Images[7];
            bttnNext.Image = MDIMains.publicMDIParent.i32x32.Images[47];
            bttnNext2.Image = MDIMains.publicMDIParent.i32x32.Images[47];

        }

        void btnUpdate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty((string)(cboBank.SelectedValue)))
            {
                Common.setEmptyField("BanK Name", Program.ApplicationName);
                cboBank.Focus();
                return;
            }
            else if (Dt == null)
            {
                Common.setMessageBox("Please Upload Bank Statement", Program.ApplicationName, 2); return;
            }
            else
            {
                try
                {
                    SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

                    //dtpEnd
                    //check if receord exits before
                    DataTable dts = (new Logic()).getSqlStatement((String.Format("SELECT BSDate AS DATE, ISNULL(Debit,0) AS DEBIT, ISNULL(Credit,0) AS CREDIT,Balance AS BALANCE  from tblbankstatement WHERE BankCode = '{0}' AND CONVERT(VARCHAR(10),StartDate,103)='{1}' AND CONVERT(VARCHAR(10),EndDate,103)='{2}' ", cboBank.SelectedValue, string.Format("{0:dd/MM/yyy}", dtpStart.Value), string.Format("{0:dd/MM/yyy}", dtpEnd.Value)))).Tables[0];

                    if (dts != null && dts.Rows.Count > 0)
                    {
                        //calling frmcopmare

                        using (FrmCompareValue compfrm = new FrmCompareValue(dts, Dt))
                        {
                            var res = compfrm.ShowDialog();
                            if (res == System.Windows.Forms.DialogResult.OK)
                            {
                                UpdateBankstatement((string)cboBank.SelectedValue, (string)cboPeriod.SelectedValue, (string)cboYears.SelectedValue, compfrm.dtEqual);
                            }
                        }
                    }
                    else
                    {
                        UpdateBankstatement((string)cboBank.SelectedValue, (string)cboPeriod.SelectedValue, (string)cboYears.SelectedValue, Dt);
                    }

                }
                catch (Exception ex)
                {

                    Common.setMessageBox(ex.StackTrace + ex.Message, "Error", 2);
                    return;
                }
                finally
                {
                    SplashScreenManager.CloseForm(false);

                }
            }

        }

        void cboPeriod_KeyPress(object sender, KeyPressEventArgs e)
        {
            Methods.AutoComplete(cboPeriod, e, true);
        }

        void cboBank_KeyPress(object sender, KeyPressEventArgs e)
        {
            Methods.AutoComplete(cboBank, e, true);
        }

        void button1_Click(object sender, EventArgs e)
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);


                string strPeriod = String.Format("{0}/{1}", cboPeriod.SelectedValue, cboYears.SelectedValue);

                if (string.IsNullOrEmpty((string)(cboBank.SelectedValue)))
                {
                    Common.setEmptyField("BanK Name", Program.ApplicationName);
                    cboBank.Focus();
                    return;
                }

                else
                {
                    //check for transaction compare before
                    string query = string.Format("SELECT COUNT(*) FROM tblBankPayDirect BP INNER JOIN tblBothBankPayDirect B ON BP.BankCode = B.Bankcode INNER JOIN tblPayDirectBank PB ON PB.Bankcode=B.Bankcode WHERE PB.Bankcode='{0}' AND CONVERT(VARCHAR(10),PB.StartDate,103) ='{1}' AND CONVERT(VARCHAR(10),PB.EndDate,103)='{2}'", cboBank.SelectedValue, string.Format("{0:dd/MM/yyy}", dtpStart.Value), string.Format("{0:dd/MM/yyy}", dtpEnd.Value));

                    if (new Logic().IsRecordExist(query))
                    {
                        if (MessageBox.Show("Transaction Already Compare Before?..,Do you want to continue?", Program.ApplicationName, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
                        {
                            //bttnCancel.PerformClick();
                            string quer1y = string.Format("DELETE FROM tblBankPayDirect WHERE BankCode='{0}' AND CONVERT(VARCHAR(10),StartDate,103) ='{1}' AND CONVERT(VARCHAR(10),EndDate,103)='{2}'", cboBank.SelectedValue, string.Format("{0:dd/MM/yyy}", dtpStart.Value), string.Format("{0:dd/MM/yyy}", dtpEnd.Value));

                            deleteRecord(quer1y);

                            string query2 = string.Format("DELETE FROM tblBothBankPayDirect WHERE BankCode='{0}'AND CONVERT(VARCHAR(10),StartDate,103) ='{1}' AND CONVERT(VARCHAR(10),EndDate,103)='{2}'", cboBank.SelectedValue, string.Format("{0:dd/MM/yyy}", dtpStart.Value), string.Format("{0:dd/MM/yyy}", dtpEnd.Value));

                            deleteRecord(query2);

                            string query3 = string.Format("DELETE FROM tblPayDirectBank WHERE BankCode='{0}' AND CONVERT(VARCHAR(10),StartDate,103) ='{1}' AND CONVERT(VARCHAR(10),EndDate,103)='{2}'", cboBank.SelectedValue, string.Format("{0:dd/MM/yyy}", dtpStart.Value), string.Format("{0:dd/MM/yyy}", dtpEnd.Value));

                            deleteRecord(query3);
                        }
                        else
                            return;

                    }
                    string test1 = string.Format("{0:yyyy/MM/dd 00:00:00}", dtpStart.Value);
                    string test2 = string.Format("{0:yyyy/MM/dd 23:59:59}", dtpEnd.Value);
                    using (WaitDialogForm form = new WaitDialogForm(" Please Wait...", " Comparing Statement "))
                    {
                        //using stored procedure to do the compare between the two {bank statement and collection }
                        using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
                        {
                            connect.Open();

                            _command = new SqlCommand("CompareBankStatementCollection", connect) { CommandType = CommandType.StoredProcedure };
                            _command.Parameters.Add(new SqlParameter("@bankcode", SqlDbType.VarChar)).Value = cboBank.SelectedValue;
                            _command.Parameters.Add(new SqlParameter("@startdate", SqlDbType.VarChar)).Value = string.Format("{0:yyyy/MM/dd 00:00:00}", dtpStart.Value);
                            _command.Parameters.Add(new SqlParameter("@enddate", SqlDbType.VarChar)).Value = string.Format("{0:yyyy/MM/dd 23:59:59}", dtpEnd.Value);

                            using (System.Data.DataSet ds = new System.Data.DataSet())
                            {//clear dataset and databtable
                                Dts = new DataTable();
                                Dts.Clear();
                                ds.Clear();
                                dbmatched = new DataTable(); dbmissing = new DataTable();
                                dbmissingpay = new DataTable();
                                dbmatched.Clear(); dbmissing.Clear(); dbmissingpay.Clear();

                                adp = new SqlDataAdapter(_command);
                                adp.Fill(ds);
                                Dts = ds.Tables[0];
                                connect.Close();

                                dtResult = ds;

                                if (ds.Tables[0].Rows[0]["returnCode"].ToString() == "00")
                                {
                                    dbmatched = ds.Tables[1];

                                    dbmissing = ds.Tables[2];

                                    dbmissingpay = ds.Tables[3];
                                }
                            }
                        }

                        if (dbmissing != null && dbmissing.Rows.Count > 0)
                        {
                            gridControl2.DataSource = dbmissing;
                            gridView3.OptionsBehavior.Editable = false;
                            gridView3.Columns["CREDIT"].DisplayFormat.FormatType = FormatType.Numeric;
                            gridView3.Columns["CREDIT"].DisplayFormat.FormatString = "n2";
                            gridView3.Columns["CREDIT"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                            gridView3.Columns["DATE"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
                            gridView3.Columns["DATE"].DisplayFormat.FormatString = "dd/MM/yyyy";
                            gridView3.Columns["CREDIT"].SummaryItem.FieldName = "CREDIT";
                            gridView3.Columns["CREDIT"].SummaryItem.DisplayFormat = "Total = {0:n}";
                            //gridView3.OptionsView.ColumnAutoWidth = false;
                            gridView3.OptionsView.ShowFooter = true;
                            gridView3.BestFitColumns();
                        }

                        if (dbmatched != null && dbmatched.Rows.Count > 0)
                        {
                            gridControl3.DataSource = dbmatched;
                            gridView4.OptionsBehavior.Editable = false;
                            gridView4.Columns["CREDIT"].DisplayFormat.FormatType = FormatType.Numeric;
                            gridView4.Columns["CREDIT"].DisplayFormat.FormatString = "n2";
                            gridView4.Columns["CREDIT"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                            gridView4.Columns["DATE"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
                            gridView4.Columns["DATE"].DisplayFormat.FormatString = "dd/MM/yyyy";
                            gridView4.Columns["CREDIT"].SummaryItem.FieldName = "CREDIT";
                            gridView4.Columns["CREDIT"].SummaryItem.DisplayFormat = "Total = {0:n}";
                            //gridView4.OptionsView.ColumnAutoWidth = false;
                            gridView4.OptionsView.ShowFooter = true;

                            gridView4.BestFitColumns();
                        }

                        if (dbmissingpay != null && dbmissingpay.Rows.Count > 0)
                        {
                            gridControl4.DataSource = dbmissingpay;
                            gridView5.OptionsBehavior.Editable = false;
                            gridView5.Columns["CREDIT"].DisplayFormat.FormatType = FormatType.Numeric;
                            gridView5.Columns["CREDIT"].DisplayFormat.FormatString = "n2";
                            gridView5.Columns["CREDIT"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                            gridView5.Columns["DATE"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
                            gridView5.Columns["DATE"].DisplayFormat.FormatString = "dd/MM/yyyy";
                            gridView5.Columns["CREDIT"].SummaryItem.FieldName = "CREDIT";
                            gridView5.Columns["CREDIT"].SummaryItem.DisplayFormat = "Total = {0:n}";
                            //gridView5.OptionsView.ColumnAutoWidth = false;
                            gridView5.OptionsView.ShowFooter = true;

                            gridView5.BestFitColumns();
                        }
                    }

                    if (isFirstGrid)
                    {
                        selection = new GridCheckMarksSelection(gridView3, ref lblSelect);
                        selection.CheckMarkColumn.VisibleIndex = 0;
                        isFirstGrid = false;
                    }
                    if (isFirstGrid2)
                    {
                        selection2 = new GridCheckMarksSelection(gridView5, ref lblSelect2);
                        selection2.CheckMarkColumn.VisibleIndex = 0;
                        isFirstGrid2 = false;
                    }
                    bttnSave.Enabled = true;
                    CalClose();
                }


            }
            catch (Exception ex)
            {

            }
            finally
            {
                SplashScreenManager.CloseForm(false);

            }

        }

        void btnUpload_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty((string)(cboBank.SelectedValue)))
            {
                Common.setEmptyField("BanK Name", Program.ApplicationName);
                cboBank.Focus();
                return;
            }
            else if (radioGroup4.SelectedIndex == -1)
            {
                Common.setEmptyField("Excel Type", Program.ApplicationName); return;
            }
            else
            {

                if (this.radioGroup4.SelectedIndex == 0)//open excel file 2003
                {
                    //exportToExcel();

                    try
                    {
                        SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

                        gridControl1.DataSource = null;

                        using (OpenFileDialog openFileDialogCSV = new OpenFileDialog() { InitialDirectory = Application.ExecutablePath, Filter = "Excel files (*.xls)|*.xls|All files (*.*)|*.*", FilterIndex = 1, RestoreDirectory = true })
                        {

                            //openFileDialogCSV.ShowDialog();
                            if (openFileDialogCSV.ShowDialog() == DialogResult.OK)

                                if (openFileDialogCSV.FileName.Length > 0)
                                {
                                    filenamesopen = openFileDialogCSV.FileName;
                                }

                            try
                            {
                                Dt = new DataTable();
                                Dt.Clear();
                                Dt.BeginInit();
                                Dt.Columns.Add("DATE", typeof(DateTime));
                                Dt.Columns.Add("DEBIT", typeof(decimal));
                                Dt.Columns.Add("CREDIT", typeof(decimal));
                                Dt.Columns.Add("BALANCE", typeof(decimal));
                                Dt.EndInit();

                                MyConnection = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" +
                                               filenamesopen + ";Extended Properties=\"Excel 8.0;HDR=YES;\"");

                                MyCommand = new System.Data.OleDb.OleDbDataAdapter("select * from [Sheet1$]", MyConnection);
                                DtSet = new System.Data.DataSet();
                                DtSet.Clear();
                                MyCommand.Fill(DtSet, "[Sheet1$]");
                                //MyCommand.Fill(Dt);

                                foreach (DataRow row in DtSet.Tables[0].Rows)
                                {
                                    //DataRow rw = new DataRow();
                                    if (!(row["DATE"] is DBNull) && (row["DATE"] != ""))
                                    {
                                        var rw = Dt.NewRow();
                                        rw["DATE"] = row["DATE"];
                                        rw["DEBIT"] = row["DEBIT"] is DBNull ? 0m : row["DEBIT"] == string.Empty ? 0m : Convert.ToDecimal(row["DEBIT"]);
                                        rw["CREDIT"] = row["CREDIT"] is DBNull ? 0m : Convert.ToDecimal(row["CREDIT"]);
                                        rw["BALANCE"] = row["BALANCE"] is DBNull ? 0m : Convert.ToDecimal(row["BALANCE"]);
                                        //if (rw["DATE"] != string.Empty)
                                        Dt.Rows.Add(rw);
                                    }
                                }


                                for (int h = 0; h < Dt.Rows.Count; h++)
                                {
                                    if (Dt.Rows[h].IsNull(0) == true)
                                    {
                                        Dt.Rows[h].Delete();
                                    }
                                }

                                Dt.AcceptChanges();
                                //SearchPrevisonRecord();

                                gridControl1.DataSource = Dt;
                                gridView1.OptionsBehavior.Editable = false;
                                MyConnection.Close();
                            }
                            catch (Exception ex)
                            {
                                Common.setMessageBox(ex.Message, Program.ApplicationName, 2);
                                gridControl1.DataSource = null;
                                return;
                            }
                            //ChangeValue(Dt);


                            //gridView1.BestFitColumns();
                            gridView1.Columns["DEBIT"].DisplayFormat.FormatType = FormatType.Numeric;
                            gridView1.Columns["DEBIT"].DisplayFormat.FormatString = "n2";
                            gridView1.Columns["CREDIT"].DisplayFormat.FormatType = FormatType.Numeric;
                            gridView1.Columns["CREDIT"].DisplayFormat.FormatString = "n2";
                            gridView1.Columns["BALANCE"].DisplayFormat.FormatType = FormatType.Numeric;
                            gridView1.Columns["BALANCE"].DisplayFormat.FormatString = "n2";

                            gridView1.Columns["CREDIT"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                            gridView1.Columns["DEBIT"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                            gridView1.Columns["BALANCE"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;

                            gridView1.Columns["BALANCE"].SummaryItem.FieldName = "CREDIT";
                            gridView1.Columns["BALANCE"].SummaryItem.DisplayFormat = "Total = {0:n}";

                            gridView1.Columns["CREDIT"].SummaryItem.FieldName = "CREDIT";
                            gridView1.Columns["CREDIT"].SummaryItem.DisplayFormat = "Total = {0:n}";

                            gridView1.Columns["DEBIT"].SummaryItem.FieldName = "DEBIT";
                            gridView1.Columns["DEBIT"].SummaryItem.DisplayFormat = "Total = {0:n}";

                            //gridView1.OptionsView.ColumnAutoWidth = false;
                            gridView1.OptionsView.ShowFooter = true;

                            gridView1.BestFitColumns();

                            label3.Text = Dt.Rows.Count + " Rows of Records ";


                        }
                    }
                    catch (Exception ex)
                    {
                        Common.setMessageBox(ex.StackTrace + ex.Message, "Error During Import", 2); return;
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

                        //exportToExcel();

                        gridControl1.DataSource = null;

                        using (OpenFileDialog openFileDialogCSV = new OpenFileDialog() { InitialDirectory = Application.ExecutablePath, Filter = "Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*", FilterIndex = 1, RestoreDirectory = true })
                        {

                            //openFileDialogCSV.ShowDialog();
                            if (openFileDialogCSV.ShowDialog() == DialogResult.OK)

                                if (openFileDialogCSV.FileName.Length > 0)
                                {
                                    filenamesopen = openFileDialogCSV.FileName;
                                }

                            try
                            {
                                Dt = new DataTable();
                                Dt.BeginInit();
                                Dt.Columns.Add("DATE", typeof(DateTime));
                                Dt.Columns.Add("DEBIT", typeof(decimal));
                                Dt.Columns.Add("CREDIT", typeof(decimal));
                                Dt.Columns.Add("BALANCE", typeof(decimal));
                                Dt.EndInit();

                                Dt.Clear();
                                MyConnection = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" +
                                               filenamesopen + ";Extended Properties=\"Excel 8.0;HDR=YES;\"");

                                MyCommand = new System.Data.OleDb.OleDbDataAdapter("select * from [Sheet1$]", MyConnection);
                                DtSet = new System.Data.DataSet(); DtSet.Clear();
                                MyCommand.Fill(DtSet, "[Sheet1$]");
                                //MyCommand.Fill(Dt);

                                foreach (DataRow row in DtSet.Tables[0].Rows)
                                {
                                    //DataRow rw = new DataRow();
                                    if (!(row["DATE"] is DBNull) && (row["DATE"] != ""))
                                    {
                                        var rw = Dt.NewRow();
                                        rw["DATE"] = row["DATE"];
                                        rw["DEBIT"] = row["DEBIT"] is DBNull ? 0m : row["DEBIT"] == string.Empty ? 0m : Convert.ToDecimal(row["DEBIT"]);
                                        rw["CREDIT"] = row["CREDIT"] is DBNull ? 0m : Convert.ToDecimal(row["CREDIT"]);
                                        rw["BALANCE"] = row["BALANCE"] is DBNull ? 0m : Convert.ToDecimal(row["BALANCE"]);
                                        //if (rw["DATE"] != string.Empty)
                                        Dt.Rows.Add(rw);
                                    }
                                }


                                for (int h = 0; h < Dt.Rows.Count; h++)
                                {
                                    if (Dt.Rows[h].IsNull(0) == true)
                                    {
                                        Dt.Rows[h].Delete();
                                    }
                                }

                                Dt.AcceptChanges();
                                //SearchPrevisonRecord();

                                gridControl1.DataSource = Dt;
                                gridView1.OptionsBehavior.Editable = false;
                                MyConnection.Close();
                            }
                            catch (Exception ex)
                            {
                                Common.setMessageBox(ex.Message, Program.ApplicationName, 2);
                                gridControl1.DataSource = null;
                                return;
                            }
                            //ChangeValue(Dt);



                            gridView1.Columns["DEBIT"].DisplayFormat.FormatType = FormatType.Numeric;
                            gridView1.Columns["DEBIT"].DisplayFormat.FormatString = "n2";
                            gridView1.Columns["CREDIT"].DisplayFormat.FormatType = FormatType.Numeric;
                            gridView1.Columns["CREDIT"].DisplayFormat.FormatString = "n2";
                            gridView1.Columns["BALANCE"].DisplayFormat.FormatType = FormatType.Numeric;
                            gridView1.Columns["BALANCE"].DisplayFormat.FormatString = "n2";
                            gridView1.Columns["BALANCE"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;

                            gridView1.Columns["BALANCE"].SummaryItem.FieldName = "CREDIT";
                            gridView1.Columns["BALANCE"].SummaryItem.DisplayFormat = "Total = {0:n}";

                            gridView1.Columns["CREDIT"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                            gridView1.Columns["DEBIT"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;

                            gridView1.Columns["CREDIT"].SummaryItem.FieldName = "CREDIT";
                            gridView1.Columns["CREDIT"].SummaryItem.DisplayFormat = "Total = {0:n}";

                            gridView1.Columns["DEBIT"].SummaryItem.FieldName = "DEBIT";
                            gridView1.Columns["DEBIT"].SummaryItem.DisplayFormat = "Total = {0:n}";

                            //gridView1.OptionsView.ColumnAutoWidth = false;
                            gridView1.OptionsView.ShowFooter = true;

                            gridView1.BestFitColumns();
                            label3.Text = Dt.Rows.Count + " Rows of Records ";


                        }
                    }
                    catch (Exception ex)
                    {
                        Common.setMessageBox(ex.StackTrace + ex.Message, "Error Loading", 2); return;
                    }
                    finally
                    {
                        SplashScreenManager.CloseForm(false);
                    }


                }

                #region


                //gridControl1.DataSource = null;

                //using (OpenFileDialog openFileDialogCSV = new OpenFileDialog() { InitialDirectory = Application.ExecutablePath, Filter = "Excel files (*.xls)|*.xls|All files (*.*)|*.*", FilterIndex = 1, RestoreDirectory = true })
                //{

                //    //openFileDialogCSV.ShowDialog();
                //    if (openFileDialogCSV.ShowDialog() == DialogResult.OK)

                //        if (openFileDialogCSV.FileName.Length > 0)
                //        {
                //            filenamesopen = openFileDialogCSV.FileName;
                //        }

                //    try
                //    {
                //        Dt = new DataTable();
                //        Dt.BeginInit();
                //        Dt.Columns.Add("DATE", typeof(DateTime));
                //        Dt.Columns.Add("DEBIT", typeof(decimal));
                //        Dt.Columns.Add("CREDIT", typeof(decimal));
                //        Dt.Columns.Add("BALANCE", typeof(decimal));
                //        Dt.EndInit();

                //        MyConnection = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" +
                //                       filenamesopen + ";Extended Properties=\"Excel 8.0;HDR=YES;\"");

                //        MyCommand = new System.Data.OleDb.OleDbDataAdapter("select * from [Sheet1$]", MyConnection);
                //        DtSet = new System.Data.DataSet();
                //        MyCommand.Fill(DtSet, "[Sheet1$]");
                //        //MyCommand.Fill(Dt);

                //        foreach (DataRow row in DtSet.Tables[0].Rows)
                //        {
                //            //DataRow rw = new DataRow();
                //            if (!(row["DATE"] is DBNull) && (row["DATE"] != ""))
                //            {
                //                var rw = Dt.NewRow();
                //                rw["DATE"] = row["DATE"];
                //                rw["DEBIT"] = row["DEBIT"] is DBNull ? 0m : row["DEBIT"] == string.Empty ? 0m : Convert.ToDecimal(row["DEBIT"]);
                //                rw["CREDIT"] = row["CREDIT"] is DBNull ? 0m : Convert.ToDecimal(row["CREDIT"]);
                //                rw["BALANCE"] = row["BALANCE"] is DBNull ? 0m : Convert.ToDecimal(row["BALANCE"]);
                //                //if (rw["DATE"] != string.Empty)
                //                Dt.Rows.Add(rw);
                //            }
                //        }

                //        #region old
                //        //Dt = DtSet.Tables.Add("[Sheet1$]");
                //        //Dt.BeginInit();
                //        //Dt.Columns.Add("DATE", typeof(DateTime));
                //        //Dt.Columns.Add("DEBIT", typeof(decimal));
                //        //Dt.Columns.Add("CREDIT", typeof(decimal));
                //        ////Dt.Columns.Add("VALUE DATE", typeof(DateTime));
                //        //Dt.Columns.Add("BALANCE", typeof(decimal));
                //        //Dt.EndInit();


                //        //Bind all excel data in to data set
                //        //MyCommand.Fill(DtSet, "[Sheet1$]");
                //        //Dt = DtSet.Tables[0];
                //        //Dt.BeginInit();
                //        //Dt.Columns.Add("DATE", typeof(DateTime));
                //        //Dt.Columns.Add("DEBIT", typeof(decimal));
                //        //Dt.Columns.Add("CREDIT", typeof(decimal));
                //        //Dt.Columns.Add("BALANCE", typeof(decimal));
                //        //Dt.EndInit();
                //        //Delete empty rows in datatable
                //        #endregion

                //        for (int h = 0; h < Dt.Rows.Count; h++)
                //        {
                //            if (Dt.Rows[h].IsNull(0) == true)
                //            {
                //                Dt.Rows[h].Delete();
                //            }
                //        }

                //        Dt.AcceptChanges();
                //        SearchPrevisonRecord();

                //        gridControl1.DataSource = Dt;
                //        gridView1.OptionsBehavior.Editable = false;
                //        MyConnection.Close();
                //    }
                //    catch (Exception ex)
                //    {
                //        Common.setMessageBox(ex.Message, Program.ApplicationName, 2);
                //        gridControl1.DataSource = null;
                //        return;
                //    }
                //    //ChangeValue(Dt);


                //    gridView1.BestFitColumns();
                //    gridView1.Columns["DEBIT"].DisplayFormat.FormatType = FormatType.Numeric;
                //    gridView1.Columns["DEBIT"].DisplayFormat.FormatString = "n2";
                //    gridView1.Columns["CREDIT"].DisplayFormat.FormatType = FormatType.Numeric;
                //    gridView1.Columns["CREDIT"].DisplayFormat.FormatString = "n2";
                //    gridView1.Columns["BALANCE"].DisplayFormat.FormatType = FormatType.Numeric;
                //    gridView1.Columns["BALANCE"].DisplayFormat.FormatString = "n2";

                //    label3.Text = Dt.Rows.Count + " Rows of Records ";


                //}
                #endregion
            }
        }

        void ChangeValue(DataTable Dt)
        {
            double dr, cr, bal;

            try
            {
                if (Dt != null && Dt.Rows.Count > 0)
                {
                    Dt.BeginInit();

                    foreach (DataRow row in Dt.Rows)
                    {
                        if (row == null || string.IsNullOrEmpty(row["DEBIT"].ToString())) continue;
                        dr = Convert.ToDouble(row["DEBIT"]);

                        row["DEBIT"] = Convert.ToDouble(dr);

                        if (row == null || string.IsNullOrEmpty(row["CREDIT"].ToString())) continue;
                        cr = Convert.ToDouble(row["CREDIT"]);

                        row["CREDIT"] = Convert.ToDouble(cr);

                        if (row == null || string.IsNullOrEmpty(row["BALANCE"].ToString())) continue;
                        bal = Convert.ToDouble(row["BALANCE"]);

                        row["BALANCE"] = Convert.ToDouble(bal);
                    }

                }
            }
            catch (Exception ex)
            {
                Tripous.Sys.ErrorBox(ex);
            }
            Dt.AcceptChanges();

        }

        private void CompareTwoDataTable(DataTable Dt1, DataTable Dt2, ref DataTable DtInBoth, ref DataTable DtNotInBoth)
        {

            DtInBoth = new DataTable();
            DtInBoth = Dt1.Clone();
            DtNotInBoth = new DataTable();
            DtNotInBoth = Dt1.Clone();
            Object[] vals = new Object[1];
            DataTable dtTableCompared = new DataTable();
            DataTable dtTableSearched = new DataTable();
            Boolean match = false;


            if (Dt1.Rows.Count >= Dt2.Rows.Count)
            {
                dtTableCompared = Dt1.Copy();
                dtTableSearched = Dt2.Copy();
            }
            else if (Dt1.Rows.Count < Dt2.Rows.Count)
            {
                dtTableCompared = Dt2.Copy();
                dtTableSearched = Dt1.Copy();
            }

            double dbcompare, dbsearched;


            foreach (DataRow row in dtTableCompared.Rows)
            {
                dbcompare = Convert.ToDouble(row["CREDIT"]);
                //DataRow newRows = DtnotInBoth.NewRow();
                int index = 0;

                foreach (DataRow item in dtTableSearched.Rows)
                {
                    DataRow newRow = DtInBoth.NewRow();

                    DataRow newRows = DtNotInBoth.NewRow();

                    dbsearched = Convert.ToDouble(item["CREDIT"]);

                    if (dbcompare == dbsearched)
                    {
                        //DtInBoth.Rows.Add(row["DATE"], row["CREDIT"]);

                        newRow.ItemArray = row.ItemArray;
                        DtInBoth.Rows.Add(newRow);
                        //break;
                        //match = true;
                        //break;
                    }
                    else
                    {
                        newRows.ItemArray = row.ItemArray;
                        DtNotInBoth.Rows.Add(newRows);
                    }


                }


            }



            //foreach (DataRow row in dtTableCompared.Rows)
            //{
            //    dbcompare = Convert.ToDouble(row["CREDIT"]);

            //    foreach (DataRow item in dtTableSearched.Rows)
            //    {
            //        DataRow newRow = DtNotInBoth.NewRow();

            //        dbsearched= Convert.ToDouble(item["CREDIT"]);

            //        if (dbcompare != dbsearched)
            //        {
            //            newRow.ItemArray = row.ItemArray;
            //            DtNotInBoth.Rows.Add(newRow);
            //            //break;
            //            //match = true;
            //            //break;
            //        }

            //    }
            //}




            //foreach (DataRow row in dtTableSearched.Rows)
            //{
            //    DataRow newRows = DtNotInBoth.NewRow();

            //    foreach (DataRow item in dtTableCompared.Rows)
            //    {
            //        if (dtTableCompared != null && dtTableCompared.Rows.Count >= 0)
            //        {
            //            if (Convert.ToString(row["DATE"]) != Convert.ToString(item["DATE"]) && Convert.ToDouble(row["CREDIT"]) != Convert.ToDouble(item["CREDIT"]))
            //            {
            //                //DtInBoth.Rows.Add(row["DATE"], row["CREDIT"]);

            //                newRows.ItemArray = row.ItemArray;
            //                match = false;
            //                break;

            //            }
            //            else
            //            {
            //                //DtNotInBoth.Rows.Add(row["DATE"], row["CREDIT"]);
            //                match = true;
            //                continue;
            //            }
            //        }
            //    }
            //    if (match)
            //    {
            //        DtNotInBoth.Rows.Add(newRows);
            //    }

            //vals[0] = row["DATE"];
            //if (dtTableCompared != null && dtTableCompared.Rows.Count >= 0)
            //{
            //    dtTableCompared.DefaultView.Sort = "DATE";
            //    int intRowFound = dtTableCompared.DefaultView.Find(vals[0]);
            //    if (intRowFound <= -1)
            //    {
            //        DataRow newRow = DtNotInBoth.NewRow();
            //        newRow.ItemArray = row.ItemArray;
            //        DtNotInBoth.Rows.Add(newRow);
            //    }
            //    else
            //    {
            //        DataRow newRow = DtInBoth.NewRow();
            //        newRow.ItemArray = row.ItemArray;
            //        DtInBoth.Rows.Add(newRow);
            //    }
            //}
        }

        void setDBComboBoxPeriod()
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

            Common.setComboList(cboPeriod, Dt, "Periods", "months");

            //cboPeriod.SelectedIndex = -1;
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

            Common.setComboList(cboYears, Dt, "YEAR", "YEAR");

            //cboYears.SelectedIndex = -1;
        }

        public void setDBComboBoxBank()
        {
            DataTable Dt;

            //connect.connect.Close();

            using (var ds = new System.Data.DataSet())
            {
                //connect.connect.Open();
                using (SqlDataAdapter ada = new SqlDataAdapter((string)"select *  from tblBank ORDER BY BankName,BankShortCode", Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }

                Dt = ds.Tables[0];
            }

            Common.setComboList(cboBank, Dt, "BankShortCode", "BankName");

            cboBank.SelectedIndex = -1;


        }

        void OnFormLoad(object sender, EventArgs e)
        {
            isRecord = false; isRecord2 = false;

            Isbank = false;


            setDBComboBoxRev();

            setDBComboxPaymode();

            setDBComboBoxBank();

            cboAcct_SelectedIndexChanged(null, null);

            boolIsUpdate2 = false;

            dtpStart.CustomFormat = "dd/MM/yyyy";

            dtpEnd.CustomFormat = "dd/MM/yyyy";

            btnUpload.Click += btnUpload_Click;

            btncompare.Click += button1_Click;

            btnUpdate.Click += btnUpdate_Click;

            bttnPreview.Click += bttnPreview_Click;

            bttnSave.Click += bttnSave_Click;

            //btnSearch.Click += btnSearch_Click;

            btnAllocate.Click += btnAllocate_Click;

            //bttnCollection.Click += bttnCollection_Click;

            bttnUpdate.Click += bttnUpdate_Click;

            btnRegister.Click += btnRegister_Click;

            bttnPost.Click += bttnPost_Click;

            btnUpdateEx.Click += btnUpdateEx_Click;

            bttnPeriod.Click += bttnPeriod_Click;

            linkLabel1.Click += linkLabel1_Click;

            //linkLabel2.Click += linkLabel2_Click;

            linkLabel2.Click += LinksClicked;

            linkLabel3.Click += LinksClicked;

            linkLabel4.Click += LinksClicked;

            cboBank.KeyPress += cboBank_KeyPress;

            //cboPeriod.KeyPress += cboPeriod_KeyPress;

            cboRevenuetype.KeyPress += cboRevenuetype_KeyPress;

            txtDateE.LostFocus += txtDateE_LostFocus;

            txtDateds.LostFocus += txtDateds_LostFocus;

            txtPayerName.LostFocus += txtPayerName_LostFocus;

            txtDepositAmt.LostFocus += txtDepositAmt_LostFocus;

            bttnClose.Click += bttnClose_Click;

            txtAmount.LostFocus += txtAmount_LostFocus;

            txtClosing.LostFocus += txtClosing_LostFocus;

            txtOpening.LostFocus += txtOpening_LostFocus;



            //cboZonal.SelectedIndexChanged += cboZonal_SelectedIndexChanged;



            dtpStart.ValueChanged += dtpStart_ValueChanged;

            dtpEnd.ValueChanged += dtpEnd_ValueChanged;

            bttnPosting.Click += bttnPosting_Click;

            bttnMatch.Click += bttnMatch_Click;

            bttnNext.Click += bttnNext_Click;

            bttnNext2.Click += bttnNext2_Click;

            bttnPostingrec.Click += bttnPostingrec_Click;

            bttnreset.Click += bttnreset_Click;

            checkBox1.CheckedChanged += checkBox1_CheckedChanged;

            //gridView10.Click += gridView10_Click;

            gridView10.DoubleClick += gridView10_DoubleClick;
        }

        public void RefreshForm()
        {
            NavBars.ToolStripEnableDisableControls(toolStrip, Tag as String);
        }

        /// <summary>
        /// Compares the two data tables.
        /// </summary>
        /// <param name="Dt1">The DT1.</param>
        /// <param name="Dt2">The DT2.</param>
        /// <param name="DtInBothAs">The dt in both as.</param>
        /// <param name="DtNotInBothAs">The dt not in both as.</param>
        private void CompareTwoDataTables(DataTable Dt1, DataTable Dt2, ref DataTable DtInBothAs, ref DataTable DtNotInBothAs)
        {


            DtInBothAs = new DataTable();
            DtInBothAs = Dt1.Clone();
            DtNotInBothAs = new DataTable();
            DtNotInBothAs = Dt1.Clone();
            Object[] vals = new Object[1];
            DataTable dtTableCompared = new DataTable();
            DataTable dtTableSearched = new DataTable();

            #region

            //if (Dt1.Rows.Count >= Dt2.Rows.Count)
            //{
            //    dtTableCompared = Dt1.Copy();
            //    dtTableSearched = Dt2.Copy();
            //}
            //else if (Dt1.Rows.Count < Dt2.Rows.Count)
            //{
            //    dtTableCompared = Dt2.Copy();
            //    dtTableSearched = Dt1.Copy();
            //}
            #endregion

            dtTableCompared = Dt1.Copy();
            dtTableSearched = Dt2.Copy();

            foreach (DataRow row in dtTableCompared.Rows)
            {
                vals[0] = row["DATE"];
                if (dtTableSearched != null && dtTableSearched.Rows.Count >= 0)
                {
                    dtTableSearched.DefaultView.Sort = "DATE";
                    int intRowFound = dtTableSearched.DefaultView.Find(vals[0]);
                    if (intRowFound <= -1)
                    {
                        DataRow newRow = DtNotInBothAs.NewRow();
                        newRow.ItemArray = row.ItemArray;
                        DtNotInBothAs.Rows.Add(newRow);

                    }
                    else
                    {
                        DataRow newRows = DtInBothAs.NewRow();
                        newRows.ItemArray = row.ItemArray;
                        DtInBothAs.Rows.Add(newRows);
                    }
                }
            }

            #region

            //foreach (DataRow row in dtTableSearched.Rows)
            //{
            //    vals[0] = row["DATE"];
            //    if (dtTableCompared != null && dtTableCompared.Rows.Count >= 0)
            //    {
            //        dtTableCompared.DefaultView.Sort = "DATE";
            //        int intRowFound = dtTableCompared.DefaultView.Find(vals[0]);
            //        if (intRowFound <= -1)
            //        {
            //            DataRow newRow = DtNotInBothA.NewRow();
            //            newRow.ItemArray = row.ItemArray;
            //            DtNotInBothA.Rows.Add(newRow);
            //        }
            //        else
            //        {
            //            DataRow newRow = DtInBothA.NewRow();
            //            newRow.ItemArray = row.ItemArray;
            //            DtInBothA.Rows.Add(newRow);
            //        }
            //    }
            //}
            #endregion
        }

        void SearchPrevisonRecord()
        {
            int months, years; string strperiod;

            if (Convert.ToInt32(cboPeriod.SelectedValue) == 1)
            {
                years = Convert.ToInt32(cboYears.SelectedValue) - 1;

                months = 12;
            }
            else
            {
                years = Convert.ToInt32(cboYears.SelectedValue);
                months = Convert.ToInt32(cboPeriod.SelectedValue) - 1;
            }


            string query = String.Format("SELECT * from tblbankstatement where BankCode ='{0}'  AND Period='{1}' AND years='{2}' AND Credit IS NOT NULL ORDER BY BSDate ", cboBank.SelectedValue, Convert.ToString(months).PadLeft(2, '0'), years);

            DataTable dtsbankSt = (new Logic()).getSqlStatement(query).Tables[0];


            string querys = String.Format("SELECT [Date Interval] FROM dbo.tblPeriods WHERE Year='{0}'  AND Periods='{1}' ", cboYears.SelectedValue, cboPeriod.SelectedValue);

            DataTable dtsPeriod = (new Logic()).getSqlStatement(querys).Tables[0];

            if (dtsPeriod != null && dtsPeriod.Rows.Count > 0)
            {
                //Splits=dtsPeriod.Tables[0].Rows[0]["[Date Interval]"]
                string getvalue = dtsPeriod.Rows[0]["Date Interval"].ToString();

                Splits = getvalue.Split(new Char[] { '-' });

            }

            if (dtsbankSt != null && dtsPeriod != null)
            {

                foreach (DataRow dr in dtsbankSt.Rows)
                {

                    //foreach (DataRow drs in dtsPeriod.Rows)
                    //{
                    if (Convert.ToDateTime(dr["BSDate"]) >= Convert.ToDateTime(Splits[0]) && Convert.ToDateTime(dr["BSDate"]) <= Convert.ToDateTime(Splits[1]))
                    {

                        string test = dr["BSDate"].ToString();
                        string test2 = dr["Credit"].ToString();
                        if (dr["Debit"].ToString() == null)
                        {
                            deb = 0;
                        }

                        Dt.Rows.Add(new object[] { dr["BSDate"].ToString(), deb, dr["Credit"].ToString(), dr["Balance"].ToString() });

                    }

                }
            }

        }

        public void GetPeriodCollection(string paramBankCode)
        {
            DataTable Dt = null;

            //label32.Text = string.Empty;

            linkLabel1.Text = string.Empty;

            using (var ds = new System.Data.DataSet())
            {
                //connect.connect.Open();
                string query = String.Format("select sum(Amount) as Amount from ViewCollectionAmountBankCode where  [BankCode] LIKE '%{0}%' AND CONVERT(VARCHAR, CONVERT(DATE, PaymentDate), 102) BETWEEN'{1}' and '{2}' and IsRecordExit=1 and IsPayDirect=1 ", paramBankCode, string.Format("{0:yyyy.MM.dd}", dtpStart.Value), string.Format("{0:yyyy.MM.dd}", dtpEnd.Value));

                using (SqlDataAdapter ada = new SqlDataAdapter(query, Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }

                Dt = ds.Tables[0];
            }

            decimal totalAmount = 0m;

            if (Dt != null && Dt.Rows.Count > 0)
            {
                linkLabel1.Text = String.Format("{0:N2}", Dt.Rows[0]["Amount"]);

                linkLabel1.Text = String.Format("{0:N2}", Dt.Rows[0]["Amount"]);

            }
            //return totalAmount;
        }

        void UpdateRecord()
        {

            if (radioGroup2.EditValue == "DR")
            {
                if (!boolIsUpdate)
                {

                    tableTrans.Rows.Add(new object[] { txtDateE.Text, cboTransType.Text, txtAmount.Text, 0 });
                    gridControl5.DataSource = tableTrans;
                }
                else
                {
                    double Dr = Convert.ToDouble(txtAmount.Text);
                    dtEdit.Rows.Add(new object[] { txtDateE.Text, cboTransType.Text, selectedPages, Dr, 0 });
                    gridControl5.DataSource = dtEdit;

                }

            }

            if (radioGroup2.EditValue == "CR")
            {
                if (!boolIsUpdate)
                {

                    tableTrans.Rows.Add(new object[] { txtDateE.Text, cboTransType.Text, 0, txtAmount.Text });
                    gridControl5.DataSource = tableTrans;
                }
                else
                {
                    double Cr = Convert.ToDouble(txtAmount.Text);
                    dtEdit.Rows.Add(new object[] { txtDateE.Text, cboTransType.Text, selectedPages, 0, Cr });
                    gridControl5.DataSource = dtEdit;
                }

            }

            //gridControl1.BringToFront();
            gridView6.Columns["Dr"].BestFit();
            gridView6.Columns["Cr"].BestFit();
            gridView6.OptionsBehavior.Editable = false;
            gridView6.BestFitColumns();
            Clear();
            txtDateE.Focus();
        }

        void Clear()
        {
            txtDateE.Text = ""; txtAmount.Clear();

            cboTransType.SelectedIndex = -1;
        }

        private static bool isPeriodClose(string strBankcode, DateTime dtv, DateTime dtv2)
        {
            bool isClose = false;

            //string query = String.Format("SELECT COUNT(Amount) AS Count FROM tblPeriods WHERE BankCode='{0}' and AccountNo='{1}' and Years ='{2}'", (string)strPeriod, (string)strAcctnum, (string)strYears);

            string query = string.Format("SELECT COUNT(Amount) FROM dbo.tblPeriods WHERE BankCode='{0}' AND CONVERT(VARCHAR(10),[Start Date],103)='{1}' AND CONVERT(VARCHAR(10),[End Date],103)='{2}' AND IsClosed=1", strBankcode, string.Format("{0:dd/MM/yyy}", dtv), string.Format("{0:dd/MM/yyy}", dtv2));

            if (new Logic().IsRecordExist(query))
            {
                isClose = true;
            }
            else
            {
                isClose = false;
            }

            return isClose;
        }

        private void bttnCollection_Click_1(object sender, EventArgs e)
        {

        }

        public void setDBComboBoxPay()
        {
            DataTable Dt;

            using (var ds = new System.Data.DataSet())
            {
                using (SqlDataAdapter ada = new SqlDataAdapter((string)"SELECT * FROM dbo.tblPayMode WHERE Description LIKE '%Cash%'", Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }

                Dt = ds.Tables[0];
            }

            Common.setComboList(cboPayMode, Dt, "PayID", "description");

            cboPayMode.SelectedIndex = -1;

        }

        public void stDBComboBoxZonal()
        {
            DataTable Dt;

            using (var ds = new System.Data.DataSet())
            {

                using (SqlDataAdapter ada = new SqlDataAdapter((string)"SELECT OfficeCode,OfficeName FROM tblZonalRevenueOfficetemp", Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }

                Dt = ds.Tables[0];
            }


            Common.setComboList(cboZonal, Dt, "OfficeCode", "OfficeName");

            cboZonal.SelectedIndex = -1;
        }

        public void setDBComboBoxRevenue()
        {
            DataTable Dt;

            using (var ds = new System.Data.DataSet())
            {


                using (SqlDataAdapter ada = new SqlDataAdapter((string)"select *  from tblRevenueTemp", Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }

                Dt = ds.Tables[0];
            }

            Common.setComboList(cboRevenue, Dt, "RevenueCode", "Description");

            cboRevenue.SelectedIndex = -1;


        }

        void GetAcctInfor(string parameter)
        {
            DataTable Dt;

            //connect.connect.Close();

            using (var ds = new System.Data.DataSet())
            {
                //connect.connect.Open();
                string query = String.Format("select BankName,AccountName,BranchName,BankAccountID,OpenBal,BankShortCode,BranchCode from ViewBankBranchAccount where AccountNumber = '{0}'", parameter);

                using (SqlDataAdapter ada = new SqlDataAdapter(query, Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }

                Dt = ds.Tables[0];
            }

            if (Dt != null && Dt.Rows.Count > 0)
            {
                BranchCode = (string)Dt.Rows[0]["BranchCode"];

                branchname = (string)Dt.Rows[0]["BranchName"];

                openingbal = Convert.ToDouble(Dt.Rows[0]["OpenBal"]);

            }

        }

        void ProcessClose()
        {
            if (string.IsNullOrEmpty(txtOpening.Text) || string.IsNullOrEmpty(txtClosing.Text))
            {
                return;
            }
            else
            {
                double sumDr = 0.0;
                double sumCr = 0.0;


                if (tableTrans.Rows.Count > 0 && tableTrans != null)
                {
                    //get the summary of dr and cr
                    sumDr = Convert.ToDouble(gridView6.Columns["Dr"].SummaryItem.SummaryValue);
                    sumCr = Convert.ToDouble(gridView6.Columns["Cr"].SummaryItem.SummaryValue);
                }

                double totCollection; double coll;
                //get total collection
                //double totCollection = Convert.ToDouble(linkLabel1.Text) + sumCr - Convert.ToDouble(linkLabel2.Text);
                if (string.IsNullOrEmpty(linkLabel3.Text))
                {
                    totCollection = 0;
                }
                else
                {
                    totCollection = Convert.ToDouble(linkLabel3.Text);//bank collection
                    //get open & closing balance
                }
                if (string.IsNullOrEmpty(linkLabel1.Text))
                {
                    coll = sumCr;//+ Convert.ToDouble(txtUncleared.Text);
                }
                else
                {
                    coll = Convert.ToDouble(linkLabel1.Text) + sumCr;//+ Convert.ToDouble(txtUncleared.Text);
                }
                //double SumClose = (Convert.ToDouble(txtOpening.Text) - sumDr + totCollection);
                double SumClose = (Convert.ToDouble(txtOpening.Text) - sumDr + totCollection);
                //+ Convert.ToDouble(txtUncleared.Text);
                double SumOpen = (sumCr - sumDr) - Convert.ToDouble(txtClosing.Text);


                label34.Text = String.Format("{0:N2}", SumClose);
            }
        }

        void ClosePeriod()
        {

            //check if transaction period is closed before or not

            string query = String.Format("SELECT COUNT(Amount) AS Count FROM tblPeriods WHERE BankCode='{0}' and CONVERT(VARCHAR(10),[Start Date],103)='{1}' and CONVERT(VARCHAR(10),[End Date],103)='{2}' AND IsClosed=1", (string)cboBank.SelectedValue, string.Format("{0:dd/MM/yyy}", dtpStart.Value), string.Format("{0:dd/MM/yyy}", dtpEnd.Value));



            if (new Logic().IsRecordExist(query))
            {
                Common.setMessageBox("Period Already Close", Program.ApplicationName, 2);
            }
            else
            {
                //now compare the close and open before insert record
                if (Convert.ToDouble(label34.Text) != Convert.ToDouble(txtClosing.Text))
                {
                    Common.setMessageBox("Closing Balance for the Transaction Period not Correct", Program.ApplicationName, 3); return;
                }
                else
                {
                    //commit close period into tbale per period and account

                    using (SqlConnection db = new SqlConnection(Logic.ConnectionString))
                    {
                        SqlTransaction transaction;

                        db.Open();
                        //string.Format("{0}{1}{2}{3}{4}",Program.UserID, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second, DateTime.Now.Millisecond);
                        transaction = db.BeginTransaction();
                        try
                        {

                            using (SqlCommand sqlCommand = new SqlCommand(String.Format("INSERT INTO tblPeriods( [Start Date] ,[End Date] ,BankCode , Amount , ReconciliationID , IsClosed ) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}');", string.Format("{0:yyyy/MM/dd}", dtpStart.Value), string.Format("{0:yyyy/MM/dd}", dtpEnd.Value), cboBank.SelectedValue, Convert.ToDouble(txtClosing.Text), string.Format("{0}{1}{2}{3}{4}", Program.UserID, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second, DateTime.Now.Millisecond), 1), db, transaction))
                            {
                                sqlCommand.ExecuteNonQuery();
                            }


                            transaction.Commit();

                        }
                        catch (SqlException sqlError)
                        {
                            transaction.Rollback();
                            Tripous.Sys.ErrorBox(sqlError);
                        }
                        db.Close();
                    }

                    Common.setMessageBox("Transaction Period Closed", Program.ApplicationName, 1); return;

                }

            }

        }

        void CalClose()
        {
            double debit = 0.0;
            double credit = 0.0;
            double collection = 0.0; double dbank = 0.0; double sumDr = 0.0;

            string query = String.Format("SELECT SUM(dr) AS debit,SUM(cr)AS credit FROM tblTransactionPosting WHERE CONVERT(VARCHAR(10),StartDate,103)='{0}' AND CONVERT(VARCHAR(10),EndDate,103)='{1}' AND AccountNo='{2}' ", string.Format("{0:dd/MM/yyy}", dtpStart.Value), string.Format("{0:dd/MM/yyy}", dtpEnd.Value), cboAcct.Text);

            DataTable dtsrec = (new Logic()).getSqlStatement(query).Tables[0];

            if (dtsrec != null && dtsrec.Rows.Count > 0)
            {
                if ((dtsrec.Rows[0]["debit"] == DBNull.Value) || string.IsNullOrEmpty(dtsrec.Rows[0]["debit"].ToString()))
                { debit = 0.0; }
                else
                {
                    debit = Convert.ToDouble(string.Format("{0:n2}", dtsrec.Rows[0]["debit"]));
                }

                if ((dtsrec.Rows[0]["credit"] == DBNull.Value) || string.IsNullOrEmpty(dtsrec.Rows[0]["credit"].ToString()))
                { credit = 0.0; }
                else
                {
                    credit = Convert.ToDouble(string.Format("{0:n2}", dtsrec.Rows[0]["credit"]));
                }

                //credit = Convert.ToDouble(string.Format("{0:n2}", dtsrec.Rows[0]["credit"]));
            }

            DataTable Dt = null;

            GetPerviousBalance();
            //label32.Text = string.Empty;
            if (dbmissing != null && dbmissing.Rows.Count > 0)
            {
                sumDr = Convert.ToDouble(gridView3.Columns["CREDIT"].SummaryItem.SummaryValue);
            }
            linkLabel1.Text = string.Empty;

            using (var ds = new System.Data.DataSet())
            {
                //connect.connect.Open();
                string quey = String.Format("select sum(Amount) as Amount from ViewCollectionAmountBankCode where  [BankCode] = '{0}' AND CONVERT(VARCHAR, CONVERT(DATE, PaymentDate), 102) BETWEEN'{1}' and '{2}' and IsRecordExit=1 and IsPayDirect=1 ", cboBank.SelectedValue, string.Format("{0:yyyy.MM.dd}", dtpStart.Value), string.Format("{0:yyyy.MM.dd}", dtpEnd.Value));

                using (SqlDataAdapter ada = new SqlDataAdapter(quey, Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }

                Dt = ds.Tables[0];
            }

            decimal totalAmount = 0m;

            if (Dt != null && Dt.Rows.Count > 0)
            {
                collection = Convert.ToDouble(String.Format("{0:N2}", Dt.Rows[0]["Amount"]));

            }

            DataTable dst2;
            //get bank statment collection
            System.Data.DataSet dataSet2 = new System.Data.DataSet();

            using (SqlDataAdapter ada = new SqlDataAdapter((string)string.Format("SELECT  SUM(Amount)  AS Amount FROM ViewBankStatment WHERE CONVERT(VARCHAR(10),StartDate,103)='{0}' and BankCode='{1}' and CONVERT(VARCHAR(10),EndDate,103)='{2}'", string.Format("{0:dd/MM/yyy}", dtpStart.Value), cboBank.SelectedValue, string.Format("{0:dd/MM/yyy}", dtpEnd.Value)), Logic.ConnectionString))
            {
                ada.Fill(dataSet2, "table");
            }

            dst2 = dataSet2.Tables[0];

            if (dst2 != null && dst2.Rows.Count > 0)
            {
                dbank = Convert.ToDouble(String.Format("{0:N2}", dst2.Rows[0]["Amount"]));
            }

            totalAmount = Convert.ToDecimal(openbal + (credit - debit) + sumDr);

            label60.Text = string.Format("Closing Balance Generated  {0:N2} ", totalAmount);
        }

        void GetPeriodRec()
        {
            if (isPeriodClose(cboBank.SelectedValue.ToString(), dtpStart.Value, dtpEnd.Value))
            {
                Common.setMessageBox("Transaction Period Already Closed", Program.ApplicationName, 1);

                setReload(); ProcessClose();
                return;
            }
            else
            {
                string query = String.Format("SELECT OpenBal,CloseBal,Cleared,Uncleared  FROM tblTransactionPosting WHERE CONVERT(VARCHAR(10),StartDate,103)='{0}' AND CONVERT(VARCHAR(10),EndDate,103)='{1}' AND AccountNo='{2}' ", string.Format("{0:dd/MM/yyy}", dtpStart.Value), string.Format("{0:dd/MM/yyy}", dtpEnd.Value), cboAcct.Text);

                DataTable dtsrec = (new Logic()).getSqlStatement(query).Tables[0];

                if (dtsrec != null && dtsrec.Rows.Count > 0)
                {
                    //txtOpening.Text = Convert.ToString(string.Format("{0:n2}", dtsrec.Rows[0]["OpenBal"]));
                    txtClosing.Text = Convert.ToString(string.Format("{0:n2}", dtsrec.Rows[0]["CloseBal"]));
                    //txtClear.Text = Convert.ToString(string.Format("{0:n2}", dtsrec.Rows[0]["Cleared"]));
                    //txtUncleared.Text = Convert.ToString(string.Format("{0:n2}", dtsrec.Rows[0]["Uncleared"]));
                    if (string.IsNullOrEmpty(txtClosing.Text))
                    {
                        txtClosing.Enabled = true;
                        //txtOpening.Enabled = true;
                        //txtClear.Enabled = false; txtUncleared.Enabled = false; 
                    }
                    else
                    {
                        txtClosing.Enabled = false;
                        //txtOpening.Enabled = false;
                        //txtClear.Enabled = false; txtUncleared.Enabled = false;
                    }



                    string queryes = String.Format("SELECT CONVERT(VARCHAR,TransDate,103) AS  Date,TransDescription as [Transaction Description],Dr,Cr  FROM tblTransactionPosting WHERE  CONVERT(VARCHAR(10),StartDate,103)='{0}' AND CONVERT(VARCHAR(10),EndDate,103)='{1}' AND AccountNo='{2}' ", string.Format("{0:dd/MM/yyy}", dtpStart.Value), string.Format("{0:dd/MM/yyy}", dtpEnd.Value), cboAcct.Text);

                    dtsreces = (new Logic()).getSqlStatement(queryes).Tables[0];

                    if (dtsreces != null && dtsreces.Rows.Count > 0)
                    {
                        tableTrans = dtsreces;
                        gridControl5.DataSource = dtsreces;
                        gridView6.Columns["Cr"].BestFit();
                        gridView6.Columns["Dr"].BestFit();
                        gridView6.OptionsBehavior.Editable = false;
                        gridView6.BestFitColumns();

                        ProcessClose();
                    }


                }

            }


        }

        void GetPerviousBalance()
        {
            double dbperiod, dbyear;

            string strperiod, stryear;


            //if (Convert.ToDouble(cboPeriod.SelectedValue) == 12)
            //{
            //    dbperiod = 01; dbyear = Convert.ToDouble(Convert.ToInt32(cboYears.SelectedValue) - 1);

            //}
            //else
            //{
            //    dbperiod = Convert.ToDouble(cboPeriod.SelectedValue) - 1; dbyear = Convert.ToDouble(cboYears.SelectedValue);
            //}

            //strperiod = dbperiod.ToString().PadLeft(cboPeriod.SelectedValue.ToString().Length, '0');

            //stryear = dbyear.ToString().PadLeft(cboYears.SelectedValue.ToString().Length, '0');

            DataTable Dts = null;

            System.Data.DataSet dataSet3 = new System.Data.DataSet();

            using (SqlDataAdapter ada = new SqlDataAdapter((string)string.Format("SELECT Amount FROM tblPeriods WHERE BankCode='{0}' AND CONVERT(VARCHAR(10),[End Date],103)='{1}' AND IsClosed=1", cboBank.SelectedValue, string.Format("{0:dd/MM/yyy}", dtpEnd.Value)), Logic.ConnectionString))
            {
                ada.Fill(dataSet3, "table");
            }

            Dts = dataSet3.Tables[0];

            if (Dts != null && Dts.Rows.Count > 0)
            {
                txtOpening.Text = String.Format("{0:N2}", Dts.Rows[0]["Amount"]); txtOpening.Enabled = false;
                openbal = Convert.ToDouble(String.Format("{0:N2}", Dts.Rows[0]["Amount"]));
            }
            else
            {
                txtOpening.Text = String.Format("{0:N2}", openingbal);

                //openbal = Convert.ToDouble(String.Format("{0:N2}", Dts.Rows[0]["Amount"]));
                openbal = Convert.ToDouble(String.Format("{0:N2}", openingbal));
                //GetPeriodRec();
            }


        }

        void GetPayDirect()
        {
            DataTable Dts = null;
            DataTable dst2 = null;
            DataTable dst3 = null;

            //get paydirect exception
            System.Data.DataSet dataSet3 = new System.Data.DataSet();

            using (SqlDataAdapter ada = new SqlDataAdapter((string)string.Format("SELECT  SUM(Amount)  AS Amount FROM ViewPayDirectBank WHERE CONVERT(VARCHAR(10),StartDate,103)='{0}' and BankCode='{1}' and CONVERT(VARCHAR(10),EndDate,103)='{2}'", string.Format("{0:dd/MM/yyy}", dtpStart.Value), cboBank.SelectedValue, string.Format("{0:dd/MM/yyy}", dtpEnd.Value)), Logic.ConnectionString))
            {
                ada.Fill(dataSet3, "table");
            }

            Dts = dataSet3.Tables[0];

            if (Dts != null && Dts.Rows.Count > 0)
            {
                linkLabel2.Text = String.Format("{0:N2}", Dts.Rows[0]["Amount"]);
            }


            if (string.IsNullOrEmpty(linkLabel2.Text))
            {
                linkLabel2.Text = "0.0";
            }

            //get bank exception
            System.Data.DataSet dataSet1 = new System.Data.DataSet();

            using (SqlDataAdapter ada = new SqlDataAdapter((string)string.Format("SELECT  SUM(Amount)  AS Amount FROM ViewBankPayDirect WHERE CONVERT(VARCHAR(10),StartDate,103)='{0}' and BankCode='{1}' and CONVERT(VARCHAR(10),EndDate,103)='{2}'", string.Format("{0:dd/MM/yyy}", dtpStart.Value), cboBank.SelectedValue, string.Format("{0:dd/MM/yyy}", dtpEnd.Value)), Logic.ConnectionString))
            {
                ada.Fill(dataSet1, "table");
            }

            dst3 = dataSet1.Tables[0];

            if (dst3 != null && dst3.Rows.Count > 0)
            {
                linkLabel4.Text = String.Format("{0:N2}", dst3.Rows[0]["Amount"]);
            }


            if (string.IsNullOrEmpty(linkLabel4.Text))
            {
                linkLabel4.Text = "0.0";
            }

            //get bank statment collection
            System.Data.DataSet dataSet2 = new System.Data.DataSet();

            using (SqlDataAdapter ada = new SqlDataAdapter((string)string.Format("SELECT  SUM(Amount)  AS Amount FROM ViewBankStatment WHERE CONVERT(VARCHAR(10),StartDate,103)='{0}' and BankCode='{1}' and CONVERT(VARCHAR(10),EndDate,103)='{2}'", string.Format("{0:dd/MM/yyy}", dtpStart.Value), cboBank.SelectedValue, string.Format("{0:dd/MM/yyy}", dtpEnd.Value)), Logic.ConnectionString))
            {
                ada.Fill(dataSet2, "table");
            }

            dst2 = dataSet2.Tables[0];

            if (dst2 != null && dst2.Rows.Count > 0)
            {
                linkLabel3.Text = String.Format("{0:N2}", dst2.Rows[0]["Amount"]);
            }


            if (string.IsNullOrEmpty(linkLabel3.Text))
            {
                linkLabel3.Text = "0.0";
            }

        }

        private void ComparePayDirectBank(DataTable dt1, DataTable dt2, ref DataTable dbmissingpay)
        {
            dbmissingpay = new DataTable();

            //If you have primary keys:
            var results = (from table1 in dt1.AsEnumerable()
                           join table2 in dt2.AsEnumerable() on table1.Field<string>("DATE") equals table2.Field<string>("DATE")
                           where table1.Field<decimal>("CREDIT") != table2.Field<decimal>("CREDIT")
                           select table1).ToList();

            var matched = (from table1 in dt1.AsEnumerable()
                           join table2 in dt2.AsEnumerable() on table1.Field<decimal>("CREDIT") equals table2.Field<decimal>("CREDIT")
                           where table1.Field<decimal>("CREDIT") == table2.Field<decimal>("CREDIT")
                           select table1).Distinct().ToList();

            var missing = (from table1 in dt1.AsEnumerable()
                           where !matched.Contains(table1)
                           select table1).ToList();

            var newRecspay = (from u in dt1.AsEnumerable()
                              where !(from o in dt2.AsEnumerable() select o.Field<decimal>("CREDIT")).Contains(u.Field<decimal>("CREDIT"))
                              select new
                              {
                                  DATE = u.Field<string>("DATE"),
                                  CREDIT = u.Field<decimal>("CREDIT")
                              }).ToList();



            dbmissingpay = LINQToDataTable(newRecspay); dbmissingpay.AcceptChanges();

        }

        private void CompareBankPayDirect(DataTable dt1, DataTable dt2, ref DataTable dbmissing, ref DataTable dbmatched)
        {


            //If you have primary keys: valuedate
            var results = (from table1 in dt1.AsEnumerable()
                           join table2 in dt2.AsEnumerable() on table1.Field<string>("DATE") equals table2.Field<string>("DATE")
                           where table1.Field<decimal>("CREDIT") != table2.Field<decimal>("CREDIT")
                           select table1).ToList();
            //This will give you the rows in dt1 which do not match the rows in dt2.  You will need to expand the where clause to include all your columns.

            //var test= from table1 in dt1.AsEnumerable() join table2 in dt2.AsEnumerable() on table1.Field<DateTime>


            //If you do not have primarry keys then you will need to match up each column and then find the missing.
            //old one
            //var matched = (from table1 in dt1.AsEnumerable()
            //               join table2 in dt2.AsEnumerable() on table1.Field<string>("DATE") equals table2.Field<string>("DATE")
            //               where table1.Field<decimal>("CREDIT") == table2.Field<decimal>("CREDIT")
            //               select table1).Distinct().ToList();

            var matched = (from table1 in dt1.AsEnumerable()
                           join table2 in dt2.AsEnumerable() on table1.Field<decimal>("CREDIT") equals table2.Field<decimal>("CREDIT")
                           where table1.Field<decimal>("CREDIT") == table2.Field<decimal>("CREDIT")
                           select table1).Distinct().ToList();


            var missing = (from table1 in dt1.AsEnumerable()
                           where !matched.Contains(table1)
                           select table1).Distinct().ToList();

            var rightRecord = (from table1 in dt1.AsEnumerable()
                               where matched.Contains(table1)
                               select table1).Distinct().ToList();

            var newRec = (from u in dt1.AsEnumerable()
                          where (from o in dt2.AsEnumerable() select o.Field<decimal>("CREDIT")).Contains(u.Field<decimal>("CREDIT"))
                          select new
                          {
                              DATE = u.Field<string>("DATE"),
                              CREDIT = u.Field<decimal>("CREDIT")
                          }).ToList();

            var newRecs = (from u in dt1.AsEnumerable()
                           where !(from o in dt2.AsEnumerable() select o.Field<decimal>("CREDIT")).Contains(u.Field<decimal>("CREDIT"))
                           select new
                           {
                               DATE = u.Field<string>("DATE"),
                               CREDIT = u.Field<decimal>("CREDIT")
                           }).ToList();

            var newRecs3 = (from u in dt2.AsEnumerable()
                            where !(from o in dt1.AsEnumerable() select o.Field<decimal>("CREDIT")).Contains(u.Field<decimal>("CREDIT"))
                            select new
                            {
                                DATE = u.Field<string>("DATE"),
                                CREDIT = u.Field<decimal>("CREDIT")
                            }).ToList();

            var updRec = from u in dt1.AsEnumerable()
                         join o in dt2.AsEnumerable() on u.Field<decimal>("CREDIT") equals o.Field<decimal>("CREDIT")
                         where (u.Field<decimal>("CREDIT") == o.Field<decimal>("CREDIT"))
                         select new
                         {
                             DATE = u.Field<string>("DATE"),
                             CREDIT = u.Field<decimal>("CREDIT")
                         };

            var dtData3 = dt2.AsEnumerable().Except(dt1.AsEnumerable(), DataRowComparer.Default);


            dbmissing = new DataTable();

            dbmatched = new DataTable();

            //dbmatched = ConvertListToDataTable(rightRecord); dbmatched.AcceptChanges();

            dbmatched = LINQToDataTable(newRec); dbmatched.AcceptChanges();
            //dbmatched = dtData3.CopyToDataTable();

            //dbmissing = ConvertListToDataTable(missing); dbmissing.AcceptChanges();

            dbmissing = LINQToDataTable(newRecs); dbmissing.AcceptChanges();

            //This should give you the rows which do not have a match.  You will need to expand the where clause to include all your columns.
        }

        public static DataTable LINQToDataTable<T>(IEnumerable<T> varlist)
        {
            DataTable dtReturn = new DataTable();

            // column names 
            PropertyInfo[] oProps = null;
            if (varlist == null) return dtReturn;

            foreach (T rec in varlist)
            {
                // Use reflection to get property names, to create table, Only first time, others will follow 
                if (oProps == null)
                {
                    oProps = ((Type)rec.GetType()).GetProperties();
                    foreach (PropertyInfo pi in oProps)
                    {
                        Type colType = pi.PropertyType;

                        if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition()
                        == typeof(Nullable<>)))
                        {
                            colType = colType.GetGenericArguments()[0];
                        }

                        dtReturn.Columns.Add(new DataColumn(pi.Name, colType));
                    }
                }

                DataRow dr = dtReturn.NewRow();

                foreach (PropertyInfo pi in oProps)
                {
                    //dr[pi.Name] = pi.GetValue(rec, null) == null ? DBNull.Value : pi.GetValue
                    //(rec, null);
                    dr[pi.Name] = pi.GetValue(rec, null) ?? DBNull.Value;
                }

                dtReturn.Rows.Add(dr);
            }
            return dtReturn;
        }

        public static DataTable ConvertListToDataTable(List<DataRow> varList)
        {
            DataTable dtReturn = null;
            foreach (var item in varList)
            {
                if (dtReturn == null)
                    dtReturn = item.Table.Clone();
                dtReturn.ImportRow(item);
            }
            return dtReturn;
        }

        public DataTable getDifferentRecords(DataTable FirstDataTable, DataTable SecondDataTable)
        {
            //Create Empty Table  
            DataTable ResultDataTable = new DataTable("ResultDataTable");

            //use a Dataset to make use of a DataRelation object  
            using (DataSet dstest = new DataSet())
            {
                //Add tables  
                dstest.Tables.AddRange(new DataTable[] { FirstDataTable.Copy(), SecondDataTable.Copy() });

                //Get Columns for DataRelation  
                DataColumn[] firstColumns = new DataColumn[dstest.Tables[0].Columns.Count];
                for (int i = 0; i < firstColumns.Length; i++)
                {
                    firstColumns[i] = dstest.Tables[0].Columns[i];
                }

                DataColumn[] secondColumns = new DataColumn[dstest.Tables[1].Columns.Count];
                for (int i = 0; i < secondColumns.Length; i++)
                {
                    secondColumns[i] = dstest.Tables[1].Columns[i];
                }

                //Create DataRelation  
                DataRelation r1 = new DataRelation(string.Empty, firstColumns, secondColumns, false);
                dstest.Relations.Add(r1);

                DataRelation r2 = new DataRelation(string.Empty, secondColumns, firstColumns, false);
                dstest.Relations.Add(r2);

                //Create columns for return table  
                for (int i = 0; i < FirstDataTable.Columns.Count; i++)
                {
                    ResultDataTable.Columns.Add(FirstDataTable.Columns[i].ColumnName, FirstDataTable.Columns[i].DataType);
                }

                //If FirstDataTable Row not in SecondDataTable, Add to ResultDataTable.  
                ResultDataTable.BeginLoadData();
                foreach (DataRow parentrow in dstest.Tables[0].Rows)
                {
                    DataRow[] childrows = parentrow.GetChildRows(r1);
                    if (childrows == null || childrows.Length == 0)
                        ResultDataTable.LoadDataRow(parentrow.ItemArray, true);
                }

                //If SecondDataTable Row not in FirstDataTable, Add to ResultDataTable.  
                foreach (DataRow parentrow in dstest.Tables[1].Rows)
                {
                    DataRow[] childrows = parentrow.GetChildRows(r2);
                    if (childrows == null || childrows.Length == 0)
                        ResultDataTable.LoadDataRow(parentrow.ItemArray, true);
                }
                ResultDataTable.EndLoadData();
            }

            return ResultDataTable;
        }
        //#endregion  

        DataTable CompareTwoDataTables(DataTable master, DataTable changer)
        {
            DataTable dtResult = new DataTable();
            dtResult = master.Copy();
            DataColumn col_new = new DataColumn("NewRow");
            DataColumn col_change = new DataColumn("ChangeRow");
            dtResult.Columns.Add(col_new);
            dtResult.Columns.Add(col_change);
            string dtFORM_clo = "";
            for (int j = 0; j < changer.Columns.Count; j++)
            {
                dtFORM_clo += changer.Columns[j].ToString() + ",";
            }
            string[] FORM_clos = dtFORM_clo.Split(',');
            for (int i = 0; i < master.Rows.Count; i++)
            {
                dtResult.Rows[i]["ChangeRow"] = "No";
                dtResult.Rows[i]["NewRow"] = "No";
                for (int xm = 0; xm < changer.Rows.Count; xm++)
                {
                    if (master.Rows[i]["Date"].ToString() == changer.Rows[xm]["Date"].ToString())//ID is the first column(primary key),So we know the name of column.
                    {
                        for (int col = 0; col < FORM_clos.Length - 1; col++)
                        {
                            if (master.Rows[i][FORM_clos[col]].ToString() != changer.Rows[xm][FORM_clos[col]].ToString())
                            {
                                changer.Rows.Remove(changer.Rows[xm]);
                                DataRow addrow = changer.NewRow();
                                for (int newcol = 0; newcol < FORM_clos.Length - 1; newcol++)
                                {
                                    addrow[FORM_clos[newcol]] = master.Rows[i][FORM_clos[newcol]].ToString();
                                }
                                changer.Rows.Add(addrow);
                                dtResult.Rows[i]["ChangeRow"] = "yes";
                            }
                        }
                    }
                    if (changer.Select("Date=" + master.Rows[i]["Date"].ToString()).Length == 0)
                    {
                        dtResult.Rows[i]["NewRow"] = "yes";
                    }
                }
            }
            return dtResult;
        }

        public DataTable CombineTwoDataTables(DataTable dt1, DataTable dt2)
        {
            //dt3 is the combined dt1 and dt2
            //add all rows of dt2 to dt3 then add only rows of dt1 that don't match
            DataTable dt3 = dt2.Copy();
            Boolean addrow = false;
            Boolean rowMatched = false;
            //check for for rows in dt1 not matching dt2
            for (int i = dt1.Rows.Count - 1; i >= 0; i--)
            {
                addrow = true;
                //compare every row in dt2 with row i in dt1
                for (int j = dt2.Rows.Count - 1; j >= 0; j--)
                {
                    rowMatched = true;
                    for (int k = dt1.Columns.Count - 1; k >= 0; k--)
                    {
                        if (dt1.Columns[k].ToString() != dt2.Columns[k].ToString())
                        {
                            rowMatched = false;
                            break;
                        }
                    }
                    if (rowMatched == true)
                    {
                        //don't add dt1 row if it already exists in dt2
                        addrow = false;
                        break;
                    }
                }
                if (addrow == true)
                {
                    dt3.Rows.Add(dt1.Rows[i].ItemArray);
                }
            }
            return dt3;
        }

        void setReload()
        {
            using (var ds = new System.Data.DataSet())
            {
                //connect.connect.Open();
                using (SqlDataAdapter ada = new SqlDataAdapter((String.Format("select CONVERT(VARCHAR, TransDate,103) AS Date ,TransDescription AS [Transaction Description],Dr,Cr  from tblTransactionPosting where Period ='{0}' and AccountNo ='{1}' and Years='{2}'", cboPeriod.SelectedValue, cboAcct.Text, cboYears.SelectedValue)), Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }
                //connect.connect.Close();
                dtEdit = ds.Tables[0];
                //gridControl1.DataSource = dt.DefaultView;

                //tableTrans = ds.Tables[0];

                gridControl5.DataSource = dtEdit.DefaultView;
            }

            gridView6.Columns["Cr"].BestFit();
            gridView6.Columns["Dr"].BestFit();
            gridView6.OptionsBehavior.Editable = false;
            gridView6.BestFitColumns();
        }

        void AddRecords()
        {

            if (selection.SelectedCount == 0 && dbmissing.Rows.Count > 0)
            {
                Common.setMessageBox("No Selection Made for Posting", Program.ApplicationName, 3);

                return;

            }
            else
            {
                using (WaitDialogForm form = new WaitDialogForm("Please Wait...", "Posting Transactions "))
                {
                    lblSelect.Text = string.Empty;

                    DataTable dts = (new Logic()).getSqlStatement((String.Format("SELECT Description,Type FROM dbo.tblTransDefinition	WHERE ElementCatCode = '{0}' ", (string)selectedPages))).Tables[0];

                    if (dts != null && dts.Rows.Count > 0)
                    {
                        ElemDescr = (string)dts.Rows[0]["Description"];
                        ElemType = (string)dts.Rows[0]["Type"];
                    }

                    string quytest = string.Format("SELECT AccountNumber,BranchCode FROM ViewBankBranchAccount where BankShortCode like '%{0}%'", cboBank.SelectedValue);

                    DataTable dtes = (new Logic()).getSqlStatement((quytest)).Tables[0];

                    if (dtes != null && dtes.Rows.Count > 0)
                    {
                        strAcct = (string)dtes.Rows[0]["AccountNumber"];
                        strBranch = (string)dtes.Rows[0]["BranchCode"];
                    }

                    //check if the account is empty
                    if (string.IsNullOrEmpty(strAcct))
                    {
                        Common.setMessageBox("Please set up Bank Account Detail", Program.ApplicationName, 1);
                        return;
                    }
                    //check if transaction record have been posted for the period before
                    string quy = string.Format("SELECT count(AccountNo) FROM tblTransactionPosting WHERE AccountNo='{0}' AND CONVERT(VARCHAR(10),StartDate,103)='{1}' AND CONVERT(VARCHAR(10),EndDate,103)='{2}'", strAcct, string.Format("{0:dd/MM/yyy}", dtpStart.Value), string.Format("{0:yyyy/MM/dd}", dtpEnd.Value));

                    if (new Logic().IsRecordExist(quy))
                    {
                        Common.setMessageBox("Period Transaction", Program.ApplicationName, 1);
                        return;
                    }
                    else
                    {
                        //strAgencyname,StrAgencyCode,strRevenueName, strDescription
                        //get Agency and Revenue
                        string qury = string.Format("SELECT top 1 tblAgencyTemp.AgencyCode, tblAgencyTemp.AgencyName, tblRevenueTemp.RevenueCode, tblRevenueTemp.Description FROM tblAgencyTemp INNER JOIN tblRevenueTemp ON tblAgencyTemp.AgencyCode = tblRevenueTemp.AgencyCode where tblAgencyTemp.StateCode ='{0}'", Program.stateCode);

                        DataTable dte = (new Logic()).getSqlStatement((qury)).Tables[0];

                        if (dte != null && dte.Rows.Count > 0)
                        {
                            strAgencyname = (string)dte.Rows[0]["AgencyName"];
                            StrAgencyCode = (string)dte.Rows[0]["AgencyCode"];
                            strRevenuecode = (string)dte.Rows[0]["RevenueCode"];
                            strDescription = (string)dte.Rows[0]["Description"];
                        }

                        int j = 0;

                        //checking selected record

                        for (int i = 0; i < selection.SelectedCount; i++)
                        {
                            //var src = DateTime.Now;
                            //var hm = new DateTime(src.Hour, src.Minute, src.Second);

                            string values = string.Empty; string values1 = string.Empty;
                            string[] Splits1; string[] Splits2;

                            strpaymentRef = string.Empty; strpayerID = string.Empty; BatchNumber = string.Empty;

                            values += String.Format("{0}", (selection.GetSelectedRow(i) as DataRowView)["Date"]);
                            values1 += String.Format("{0}", (selection.GetSelectedRow(i) as DataRowView)["Credit"]);

                            BatchNumber = String.Format("{0:d5}", (DateTime.Now.Ticks / 10) % 100000);

                            //BatchNumber = string.Format("{0}{1}{2}", DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

                            strpayerID = string.Format("{0}|{1}|{2:yyyMMddhhmmss}", cboBank.SelectedValue, BatchNumber, DateTime.Now);
                            strpaymentRef = String.Format("{0}|OGPRC|{1}|{2:dd-MM-yyyy}|{3}", cboBank.SelectedValue, strBranch, DateTime.Now, string.Format("{0}{1}{2}{3}", DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second, DateTime.Now.Millisecond));

                            //slipt the date, then recreate it

                            Splits1 = values.Split(new Char[] { '/' });

                            string strDate = String.Format("{0}/{1}/{2}", Splits1[1], Splits1[0], Splits1[2]);

                            //insert record
                            using (SqlConnection db = new SqlConnection(Logic.ConnectionString))
                            {
                                SqlTransaction transaction;

                                db.Open();

                                transaction = db.BeginTransaction();

                                try
                                {
                                    //insert into collection table
                                    string query3 = String.Format("INSERT INTO tblCollectionReport([Provider],[Channel],[PaymentRefNumber],[PaymentDate],[PayerID],[Amount],[PaymentMethod],[BankCode],[BankName],[GeneratedBy],[UploadStatus],[ChequeStatus],[IsPayDirect],[IsRecordExit],[ChequeValueDate],[AgencyCode],[AgencyName],[RevenueCode],DESCRIPTION) VALUES ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}');", "ICMA", "Bank", strpaymentRef, strDate, strpayerID, Convert.ToDecimal(values1), "Cash", cboBank.SelectedValue, cboBank.Text, Program.UserID, "Waiting", "Cleared", false, true, strDate, StrAgencyCode, strAgencyname, strRevenuecode, strDescription);

                                    using (SqlCommand sqlCommand1 = new SqlCommand(query3, db, transaction))
                                    {
                                        sqlCommand1.ExecuteNonQuery();
                                    }

                                    //insert receord into the tbltransactionposting
                                    string query4 = string.Format("INSERT INTO tblTransactionPosting (AccountNo,Type,TransDate,TransDescription,Dr,Cr,StartDate,EndDate) VALUES ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}');", strAcct, ElemType, strDate, ElemDescr, 0, Convert.ToDecimal(values1), string.Format("{0:yyyy/MM/dd}", dtpStart.Value), string.Format("{0:yyyy/MM/dd}", dtpEnd.Value));

                                    using (SqlCommand sqlCommand = new SqlCommand(query4, db, transaction))
                                    {
                                        sqlCommand.ExecuteNonQuery();
                                    }

                                    transaction.Commit();
                                }
                                catch (SqlException sqlError)
                                {
                                    transaction.Rollback(); Tripous.Sys.ErrorBox(sqlError);
                                    return;
                                }
                                db.Close();
                            }
                        }

                        DebitCredidtPst();

                        Common.setMessageBox("Transaction Posted Successfully..", Program.ApplicationName, 1);
                        return;
                    }

                    #region


                    //for (int i = 0; i < selection.SelectedCount; i++)
                    //{

                    //    string values = string.Empty; string values1 = string.Empty;
                    //    string[] Splits1; string[] Splits2;

                    //    values += String.Format("{0}", (selection.GetSelectedRow(i) as DataRowView)["Date"]);
                    //    values1 += String.Format("{0}", (selection.GetSelectedRow(i) as DataRowView)["Credit"]);

                    //    BatchNumber = String.Format("{0:d5}", (DateTime.Now.Ticks / 10) % 100000);

                    //    strpayerID = string.Format("{0}|{1}|{2:yyyMMddhhmmss}", cboBank.SelectedValue, BatchNumber, DateTime.Now);
                    //    strpaymentRef = String.Format("FIC|{0}|{1:dd-MM-yyyy}|{2}", cboBank.SelectedValue, DateTime.Now, BatchNumber);

                    //    //slipt the date, then recreate it

                    //    Splits1 = values.Split(new Char[] { '/' });

                    //    string strDate = String.Format("{0}/{1}/{2}", Splits1[1], Splits1[0], Splits1[2]);

                    //    //check record if transaction exits
                    //    string gqry = string.Format("SELECT * FROM tblCollectionReport WHERE PaymentDate='{0}' AND Amount='{1}'", strDate, values1);

                    //    DataTable dtsgqry = (new Logic()).getSqlStatement(gqry).Tables[0];

                    //    if (dtsgqry != null && dtsgqry.Rows.Count > 0)
                    //    {
                    //        //update record
                    //        //string query = String.Format(String.Format("UPDATE [tblPeriods] SET [Periods]='{{0}}',[Date Interval]='{{1}}',[Months]='{{2}}' ,[Start Date] ='{{3}}',[End Date]='{{4}}' where  [Year] ='{0}' and [Periods]='{1}'", spinEdit1.EditValue, (string)dt.Rows[i]["Periods"]), (string)dt.Rows[i]["Periods"], (string)dt.Rows[i]["Date Interval"], (string)dt.Rows[i]["Months"], Convert.ToDateTime(dt.Rows[i]["Start Date"]).ToString("yyyy/MM/dd"), Convert.ToDateTime(dt.Rows[i]["End Date"]).ToString("yyyy/MM/dd"));

                    //        //using (SqlCommand sqlCommand1 = new SqlCommand(query, db, transaction))
                    //        //{
                    //        //    sqlCommand1.ExecuteNonQuery();
                    //        //}
                    //        //using (SqlConnection db = new SqlConnection(Logic.ConnectionString))
                    //        //{
                    //        //    SqlTransaction transaction;

                    //        //    db.Open();

                    //        //    transaction = db.BeginTransaction();

                    //        //    string query = String.Format(String.Format("UPDATE [tblCollectionReport] SET [Periods]='{{0}}',[Date Interval]='{{1}}',[Months]='{{2}}' ,[Start Date] ='{{3}}',[End Date]='{{4}}' where  [Year] ='{0}' and [Periods]='{1}'", spinEdit1.EditValue, (string)dt.Rows[i]["Periods"]), (string)dt.Rows[i]["Periods"], (string)dt.Rows[i]["Date Interval"], (string)dt.Rows[i]["Months"], Convert.ToDateTime(dt.Rows[i]["Start Date"]).ToString("yyyy/MM/dd"), Convert.ToDateTime(dt.Rows[i]["End Date"]).ToString("yyyy/MM/dd"));

                    //        //    using (SqlCommand sqlCommand1 = new SqlCommand(query, db, transaction))
                    //        //    {
                    //        //        sqlCommand1.ExecuteNonQuery();
                    //        //    }

                    //        //}

                    //        return;
                    //    }
                    //    else
                    //    {

                    //    }


                    //    //insert into collcetion report table
                    //    //    using (SqlConnection db = new SqlConnection(Logic.ConnectionString))
                    //    //    {
                    //    //        SqlTransaction transaction;

                    //    //        db.Open();

                    //    //        transaction = db.BeginTransaction();

                    //    //        try
                    //    //        {
                    //    //            //insert into collection table
                    //    //            string query3 = String.Format("INSERT INTO [tblCollectionReport]([Provider],[Channel],[PaymentRefNumber],[PaymentDate],[PayerID],[Amount],[PaymentMethod],[BankCode],[BankName],[State],[GeneratedBy],[UploadStatus],[ChequeStatus],[IsPayDirect],[IsRecordExit],[ChequeValueDate],AgencyCode,AgencyName,RevenueCode,Description) VALUES ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}','{19}');", "ICM", "Bank", strpaymentRef, strDate, strpayerID, Convert.ToDecimal(values1), "Cash", cboBank.SelectedValue, cboBank.Text, Program.StateName, Program.UserID, "Waiting", "Cleared", false, true, strDate, StrAgencyCode, strAgencyname, strRevenuecode, strDescription);

                    //    //            using (SqlCommand sqlCommand1 = new SqlCommand(query3, db, transaction))
                    //    //            {
                    //    //                sqlCommand1.ExecuteNonQuery();
                    //    //            }

                    //    //            //insert receord into the tbltransactionposting
                    //    //            string query4 = string.Format("INSERT INTO tblTransactionPosting     (Period,AccountNo,Type,TransDate,TransDescription,Dr,Cr,Years) VALUES ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}');", cboPeriod.SelectedValue, strAcct, ElemType, strDate, ElemDescr, 0, Convert.ToDecimal(values1), cboYears.SelectedValue);

                    //    //            using (SqlCommand sqlCommand = new SqlCommand(query4, db, transaction))
                    //    //            {
                    //    //                sqlCommand.ExecuteNonQuery();
                    //    //            }

                    //    //            transaction.Commit();
                    //    //        }
                    //    //        catch (SqlException sqlError)
                    //    //        {
                    //    //            transaction.Rollback(); Tripous.Sys.ErrorBox(sqlError);
                    //    //            return;
                    //    //        }
                    //    //        db.Close();
                    //    //    }



                    //    //    ////insert receord into the gridview for posting
                    //    //    //double Cr = Convert.ToDouble(values1.Trim());
                    //    //    //tableTrans.Rows.Add(new object[] { values, ElemDescr, selectedPages, 0, Cr });
                    //    //    //gridControl5.DataSource = tableTrans;

                    //    //}
                    //}
                    #endregion


                }
            }
        }

        void DebitCredidtPst()
        {
            using (WaitDialogForm form = new WaitDialogForm("Please Wait...", "Posting Transactions "))
            {

                string quytest = string.Format("SELECT AccountNumber FROM ViewBankBranchAccount where BankShortCode like '%{0}%'", cboBank.SelectedValue);

                DataTable dtes = (new Logic()).getSqlStatement((quytest)).Tables[0];

                if (dtes != null && dtes.Rows.Count > 0)
                {
                    strAcct = (string)dtes.Rows[0]["AccountNumber"];
                }


                //debit side posting

                DataTable dts = (new Logic()).getSqlStatement((String.Format("SELECT TransDate as DATE,TransAmount,TransDescription FROM tblAllocateDebit WHERE BankCode = '{0}' AND CONVERT(VARCHAR(10),StartDate,103) = '{1}' AND CONVERT(VARCHAR(10),EndDate,103) = '{2}' AND ((TransDescription IS NOT NULL) or  (TransDescription IS NULL))", (string)cboBank.SelectedValue, string.Format("{0:dd/MM/yyy}", dtpStart.Value), string.Format("{0:dd/MM/yyy}", dtpEnd.Value)))).Tables[0];


                if (dts != null && dts.Rows.Count > 0)
                {
                    //looping the datatable thorugh
                    foreach (DataRow item in dts.Rows)
                    {
                        if (item != null)
                        {//insert into collcetion report table
                            using (SqlConnection db = new SqlConnection(Logic.ConnectionString))
                            {
                                SqlTransaction transaction;

                                db.Open();

                                transaction = db.BeginTransaction();

                                try
                                {
                                    //insert receord into the tbltransactionposting
                                    string query4 = string.Format("INSERT INTO tblTransactionPosting (AccountNo,Type,TransDate,TransDescription,Dr,Cr,StartDate,EndDate) VALUES ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}');", strAcct, "DR", item["DATE"], item["TransDescription"], Convert.ToDecimal(item["TransAmount"]), 0, string.Format("{0:yyyy/MM/dd}", dtpStart.Value), string.Format("{0:yyyy/MM/dd}", dtpEnd.Value));

                                    using (SqlCommand sqlCommand = new SqlCommand(query4, db, transaction))
                                    {
                                        sqlCommand.ExecuteNonQuery();
                                    }

                                    transaction.Commit();
                                }
                                catch (SqlException sqlError)
                                {
                                    transaction.Rollback(); Tripous.Sys.ErrorBox(sqlError);
                                }
                                db.Close();
                            }
                        }

                    }

                }

                //credit side posting

                DataTable dtse = (new Logic()).getSqlStatement((String.Format("SELECT TransDate AS DATE,TransAmount,TransDescription FROM tblAllocateCredit WHERE BankCode = '{0}' AND CONVERT(VARCHAR(10),StartDate,103) = '{1}' AND CONVERT(VARCHAR(10),EndDate,103) = '{2}' AND TransDescription IS NOT NULL ", (string)cboBank.SelectedValue, string.Format("{0:dd/MM/yyy}", dtpStart.Value), string.Format("{0:dd/MM/yyy}", dtpEnd.Value)))).Tables[0];

                if (dtse != null && dtse.Rows.Count > 0)
                {
                    foreach (DataRow item in dtse.Rows)
                    {
                        //insert into collcetion report table
                        using (SqlConnection db = new SqlConnection(Logic.ConnectionString))
                        {
                            SqlTransaction transaction;

                            db.Open();

                            transaction = db.BeginTransaction();

                            try
                            {
                                //insert receord into the tbltransactionposting
                                string query4 = string.Format("INSERT INTO tblTransactionPosting     (AccountNo,Type,TransDate,TransDescription,Dr,Cr,StartDate,EndDate) VALUES ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}');", strAcct, "CR", Convert.ToDateTime(item["DATE"]), item["TransDescription"], 0, Convert.ToDecimal(item["TransAmount"]), string.Format("{0:yyyy/MM/dd}", dtpStart.Value), string.Format("{0:yyyy/MM/dd}", dtpEnd.Value));

                                using (SqlCommand sqlCommand = new SqlCommand(query4, db, transaction))
                                {
                                    sqlCommand.ExecuteNonQuery();
                                }

                                transaction.Commit();
                            }
                            catch (SqlException sqlError)
                            {
                                transaction.Rollback(); Tripous.Sys.ErrorBox(sqlError);
                            }
                            db.Close();
                        }
                    }
                }
            }

        }

        static bool IsRecordExits(string strPeriod, string StrYear, DataTable dbData)
        {
            bool isRecord = false;

            String[] Splits3;
            string getvalue;

            string querys = String.Format("SELECT CONVERT(VARCHAR,[Start Date],103) AS [Start Date],CONVERT(VARCHAR,[End Date],103) AS [End Date] FROM tblPeriods WHERE Year='{0}'  AND Periods='{1}' ", StrYear, strPeriod);

            DataTable dtsPeriod = (new Logic()).getSqlStatement(querys).Tables[0];

            if (dtsPeriod != null && dtsPeriod.Rows.Count > 0)
            {
                //Splits=dtsPeriod.Tables[0].Rows[0]["[Date Interval]"]
                //getvalue = dtsPeriod.Rows[0]["Date Interval"].ToString();
                string strStart = dtsPeriod.Rows[0]["Start Date"].ToString();
                string strEnd = dtsPeriod.Rows[0]["End Date"].ToString();
                //Splits3 = getvalue.Split(new Char[] { '-' });

                if (dbData != null && dtsPeriod != null)
                {

                    foreach (DataRow dr in dbData.Rows)
                    {
                        //if (!System.DBNull(dr["DATE"]))
                        //{
                        //if (Convert.ToDateTime(dr["DATE"]) >= Convert.ToDateTime(Splits3[0]) && Convert.ToDateTime(dr["DATE"]) <= Convert.ToDateTime(Splits3[1]))
                        if (Convert.ToDateTime(dr["DATE"]) >= Convert.ToDateTime(strStart) && Convert.ToDateTime(dr["DATE"]) <= Convert.ToDateTime(strEnd))
                        {
                            isRecord = true;
                        }
                        else
                        {
                            isRecord = false;
                            return isRecord;
                        }
                        //}

                    }
                }

            }
            return isRecord;
        }

        void UpdateBankstatement(string strbankcode, string strperiod, string stryear, DataTable dbData)
        {

            using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
            {
                connect.Open();
                _command = new SqlCommand("InsertBankExcellStatment", connect) { CommandType = CommandType.StoredProcedure };
                _command.Parameters.Add(new SqlParameter("@bankcode", SqlDbType.VarChar)).Value = strbankcode;
                _command.Parameters.Add(new SqlParameter("@startdate", SqlDbType.VarChar)).Value = string.Format("{0:yyyy/MM/dd}", dtpStart.Value);
                _command.Parameters.Add(new SqlParameter("@enddate", SqlDbType.VarChar)).Value = string.Format("{0:yyyy/MM/dd}", dtpEnd.Value);
                _command.Parameters.Add(new SqlParameter("@pTransaction", SqlDbType.Structured)).Value = dbData;
                //@Years
                using (System.Data.DataSet ds = new System.Data.DataSet())
                {
                    ds.Clear(); Dts = new DataTable();
                    Dts.Clear();
                    adp = new SqlDataAdapter(_command);
                    adp.Fill(ds);
                    Dts = ds.Tables[0];
                    connect.Close();

                    if (ds.Tables[0].Rows[0]["returnCode"].ToString() != "00")
                    {
                        Common.setMessageBox(ds.Tables[0].Rows[0]["returnMessage"].ToString(), Program.ApplicationName, 2);

                        return;

                    }
                    else
                    {
                        Common.setMessageBox(ds.Tables[0].Rows[0]["returnMessage"].ToString(), Program.ApplicationName, 2);
                    }

                }
            }
            //}
        }

        private void radioGroup1_Properties_Click(object sender, EventArgs e)
        {

        }

        void deleteRecord(string parameter2)
        {
            //string query = String.Format("DELETE  FROM tblBankAccount WHERE BankAccountID='{0}'", parameter2);

            using (SqlConnection db = new SqlConnection(Logic.ConnectionString))
            {
                SqlTransaction transaction;

                db.Open();

                transaction = db.BeginTransaction();

                try
                {
                    using (SqlCommand sqlCommand1 = new SqlCommand(parameter2, db, transaction))
                    {
                        sqlCommand1.ExecuteNonQuery();
                    }

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Tripous.Sys.ErrorBox(ex);
                }

                db.Close();
            }
            //Common.setMessageBox("Record Deleted Successfully ", Program.ApplicationName, 3);
        }

    }
}
