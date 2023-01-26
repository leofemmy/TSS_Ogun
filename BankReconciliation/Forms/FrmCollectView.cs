using BankReconciliation.Class;
using BankReconciliation.Report;
using DevExpress.XtraGrid.Selection;
using DevExpress.XtraReports.UI;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using TaxSmartSuite.Class;

namespace BankReconciliation.Forms
{
    public partial class FrmCollectView : Form
    {
        private DataTable Dt;

        private string endDate;
        GridCheckMarksSelection selection;

        public static FrmCollectView publicStreetGroup;

        protected TransactionTypeCode iTransType;

        protected bool boolIsUpdate;

        protected int ID;

        bool isFirstGrid = true;

        bool isFirst = true;

        private object[] split;

        private String[] split2;

        private string stDate, strBankName;

        string BankCode, strPeriods;

        DataTable dt = new DataTable(), dt2 = new DataTable();

        internal decimal sum;

        public FrmCollectView(string bankCode, string strPeriod)
        {
            InitializeComponent();

            setImages();

            Load += OnFormLoad;

            BankCode = bankCode;

            strPeriods = strPeriod;

            bttnProcess.Click += Button_Click;

            bttnClose.Click += Button_Click;

            bttnReport.Click += Button_Click;

            bttnUndo.Click += Button_Click;

            OnFormLoad(null, null);



        }

        void getTable()
        {
            //dt2 = new DataTable();
            //tableTrans.Columns.Add("Date", typeof(string));
            dt2.Columns.Add("PaymentRefNumber", typeof(string));
            dt2.Columns.Add("PaymentDate", typeof(string));
            dt2.Columns.Add("Amount", typeof(double));
        }

        internal GridCheckMarksSelection Selection
        {
            get { return selection; }
        }

        private void setImages()
        {
            //tsbNew.Image = MDIMains.publicMDIParent.i16x16.Images[5];
            //tsbEdit.Image = MDIMains.publicMDIParent.i16x16.Images[6];
            //tsbDelete.Image = MDIMains.publicMDIParent.i16x16.Images[7];
            //tsbReload.Image = MDIMains.publicMDIParent.i16x16.Images[0];
            //tsbClose.Image = MDIMains.publicMDIParent.i16x16.Images[11];

            ////bttnCancel.Image = MDIMains.publicMDIParent.i32x32.Images[9];
            ////bttnReset.Image = MDIMains.publicMDIParent.i32x32.Images[8];
            //bttnUpdate.Image = MDIMains.publicMDIParent.i32x32.Images[7];
            bttnReport.Image = MDIMains.publicMDIParent.i32x32.Images[5];
            bttnClose.Image = MDIMains.publicMDIParent.i16x16.Images[11];
            bttnProcess.Image = MDIMains.publicMDIParent.i32x32.Images[7];
            bttnUndo.Image = MDIMains.publicMDIParent.i32x32.Images[4];

        }

        void OnFormLoad(object sender, EventArgs e)
        {
            //ShowForm();
            getTable();
            isFirst = false;
            setReload();
            //cboNature.KeyPress += cboNature_KeyPress;

        }

        private void setReload()
        {
            gridControl1.DataSource = string.Empty;

            dt = new DataTable();

            using (var ds = new System.Data.DataSet())
            {
                //connect.connect.Open();

                string query = String.Format("SELECT PaymentRefNumber,PaymentDate,Amount from ViewReconciliationCollectionReport where BankCode LIKE '%{0}%'AND Period= '{1}' and IsRecordExit=1 and IsPayDirect=1 ORDER BY PaymentDate", BankCode, strPeriods);

                using (SqlDataAdapter ada = new SqlDataAdapter(query, Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }

                dt = ds.Tables[0];
                //gridControl4.DataSource = dt.DefaultView;

                gridControl1.DataSource = dt.DefaultView;
            }

            gridView1.OptionsBehavior.Editable = false;
            gridView1.Columns["Amount"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            gridView1.Columns["Amount"].DisplayFormat.FormatString = "n2";
            gridView1.Columns["PaymentDate"].BestFit();
            gridView1.Columns["Amount"].BestFit();
            gridView1.Columns["PaymentRefNumber"].Visible = false;
            gridView1.BestFitColumns();


            gridView1.Columns["Amount"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            gridView1.Columns["Amount"].SummaryItem.DisplayFormat = "Total Amount = {0:n}";

            gridView1.OptionsView.ShowFooter = true;

            label5.Text = dt.Rows.Count + "  Rows of Records. ";

            if (isFirstGrid)
            {
                selection = new GridCheckMarksSelection(gridView1, ref label4);
                selection.CheckMarkColumn.VisibleIndex = 0;
                isFirstGrid = false;
            }
        }

        void Button_Click(object sender, EventArgs e)
        {

            if (sender == bttnProcess)
            {
                if (selection.SelectedCount < 1)
                {
                    Common.setMessageBox("Please, Select Record..", Program.ApplicationName, 3);
                    return;
                }
                else
                {
                    strProcess();

                    #region 

                    //using (SqlConnection db = new SqlConnection(Logic.ConnectionString))
                    //{
                    //    SqlTransaction transaction;

                    //    SqlCommand sqlCommand1;

                    //    db.Open();

                    //    transaction = db.BeginTransaction();

                    //    try
                    //    {
                    //        for (int i = 0; i < selection.SelectedCount; i++)
                    //        {
                    //            string lol = ((selection.GetSelectedRow(i) as DataRowView)["PaymentRefNumber"].ToString());

                    //            string query = String.Format("UPDATE dbo.tblCollectionReport SET IsRecordExit =0 WHERE PaymentRefNumber= '{0}'", lol);

                    //            sqlCommand1 = new SqlCommand(query, db, transaction);

                    //            sqlCommand1.ExecuteNonQuery();
                    //        }
                    //        transaction.Commit();
                    //    }
                    //    catch (Exception ex)
                    //    {
                    //        transaction.Rollback();
                    //        Tripous.Sys.ErrorBox(ex);
                    //    }

                    //    db.Close();
                    //}

                    //setReload();
                    #endregion
                }
            }
            else if (sender == bttnUndo)
            {
                gridControl1.DataSource = string.Empty;

                if (dt != null)
                {

                    gridControl1.DataSource = dt.DefaultView;
                    gridView1.OptionsBehavior.Editable = false;
                    gridView1.Columns["Amount"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                    gridView1.Columns["Amount"].DisplayFormat.FormatString = "n2";

                    gridView1.Columns["PaymentRefNumber"].Visible = false;
                    gridView1.BestFitColumns();



                    gridView1.Columns["Amount"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                    gridView1.Columns["Amount"].SummaryItem.FieldName = "Amount";
                    gridView1.Columns["Amount"].SummaryItem.DisplayFormat = "Total Amount = {0:n}";

                    gridView1.OptionsView.ShowFooter = true;

                    dt2.Clear();
                }
            }
            else if (sender == bttnReport)
            {
                DataTable Dts; string varGet;

                GetAcctInfor(BankCode);

                split = strPeriods.Split(new Char[] { '/' });

                using (var ds = new System.Data.DataSet())
                {
                    string query = String.Format("SELECT [Date Interval] FROM dbo.tblPeriods where Periods = '{0}'AND Year= '{1}'", split[0], split[1]);

                    using (SqlDataAdapter ada = new SqlDataAdapter(query, Logic.ConnectionString))
                    {
                        ada.Fill(ds, "table");
                    }
                    Dts = ds.Tables[0];

                    if (Dts != null && Dts.Rows.Count > 0)
                    {
                        varGet = (string)Dts.Rows[0]["[Date Interval]"];

                        split2 = varGet.Split(new Char[] { '-' });
                    }
                }

                stDate = (string)split2[0]; endDate = (string)split2[1];
                //check if the datasource dt2 is either epmty or null
                if (dt2 != null && dt2.Rows.Count > 0)
                {
                    //XtraRepReconDetails repRec = new XtraRepReconDetails();
                    XtraRepPayDirect repRec = new XtraRepPayDirect() { DataSource = dt2, DataMember = "ReportTable" };


                    repRec.xrLabel4.Text = String.Format("PayDirect Collections Not found in the Bank Statement from {0} to {1}", stDate, endDate);
                    repRec.xrLabel5.Text = String.Format("Bank Name: {0}", strBankName, strBankName);

                    repRec.ShowPreviewDialog();
                }
                else
                {

                    XtraRepPayDirect repRec = new XtraRepPayDirect()
                    {
                        FilterString = "StartsWith([BankCode], ?paramBankCode) And [Period] = ?paramPeriod And [IsRecordExit] = ?paramRecord"
                        //String.Format("StartsWith([BankCode], {0}) And [Period] = {1} And [IsRecordExit] = {2}", BankCode, strPeriods, false)
                    };
                    repRec.paramBankCode.Value = BankCode;
                    repRec.paramRecord.Value = false;
                    repRec.paramPeriod.Value = strPeriods;
                    repRec.xrLabel4.Text = String.Format("PayDirect Collections Not found in the Bank Statement from {0} to {1}", stDate, endDate);
                    repRec.xrLabel5.Text = String.Format("Bank Name: {0}", strBankName, strBankName);

                    repRec.ShowPreviewDialog();
                }

            }
            else if (sender == bttnClose)
            {

                processData();

                sum = Convert.ToDecimal(gridView1.Columns["Amount"].SummaryItem.SummaryValue);

                this.Close();

            }
        }

        public decimal getTotal
        {
            get
            {
                return sum;
            }
        }

        void GetAcctInfor(string parameter)
        {
            DataTable Dt;

            using (var ds = new System.Data.DataSet())
            {

                string query = String.Format("select BankName,AccountName,BranchName,BankAccountID,OpenBal,BankShortCode,BranchCode from ViewBankBranchAccount where BankShortCode = '{0}'", parameter);

                using (SqlDataAdapter ada = new SqlDataAdapter(query, Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }

                Dt = ds.Tables[0];
            }

            if (Dt != null && Dt.Rows.Count > 0)
            {

                strBankName = (string)Dt.Rows[0]["BankName"];

            }

            //return strBankName;

        }

        void strProcess()
        {
            gridControl1.DataSource = string.Empty;

            //clear dt2 tabale
            dt2.Clear();


            for (int i = 0; i < selection.SelectedCount; i++)
            {
                dt2.Rows.Add(new object[] { (string)(selection.GetSelectedRow(i) as DataRowView)["PaymentRefNumber"], (string)(selection.GetSelectedRow(i) as DataRowView)["PaymentDate"], Convert.ToDouble((selection.GetSelectedRow(i) as DataRowView)["Amount"]) });
            }

            DataTable dtChange = getChanges(dt, dt2);

            if (dtChange != null)
            {
                gridControl1.DataSource = dtChange.DefaultView;
                gridView1.OptionsBehavior.Editable = false;
                gridView1.Columns["Amount"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                gridView1.Columns["Amount"].DisplayFormat.FormatString = "n2";

                gridView1.Columns["PaymentRefNumber"].Visible = false;
                gridView1.BestFitColumns();

                gridView1.Columns["Amount"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                gridView1.Columns["Amount"].SummaryItem.FieldName = "Amount";
                gridView1.Columns["Amount"].SummaryItem.DisplayFormat = "Total Amount = {0:n}";

                gridView1.OptionsView.ShowFooter = true;
            }

        }

        static DataTable CompareTwoDataTable(DataTable dt1, DataTable dt2)
        {

            DataTable dt3 = dt2.GetChanges();

            return dt3;
        }

        static DataTable getChanges(DataTable dt1, DataTable dt2)
        {
            DataTable dt3 = null;

            if (dt1 != null && dt2 != null)
            {
                string query = null;
                foreach (DataRow dr in dt2.Rows)
                {
                    query += string.Format("'{0}',", dr.ItemArray[0]);
                }
                if (!string.IsNullOrEmpty(query))
                    query = query.Remove(query.Length - 1, 1);
                var rows = dt1.Select(string.Format("{0} NOT IN ({1})", "PaymentRefNumber", query));
                dt3 = rows.CopyToDataTable();
            }
            return dt3;
        }

        void processData()
        { //process data that are selected and stored in dt2

            if (dt2 != null && dt2.Rows.Count > 1)
            {
                using (SqlConnection db = new SqlConnection(Logic.ConnectionString))
                {
                    SqlTransaction transaction;

                    SqlCommand sqlCommand1;

                    db.Open();

                    transaction = db.BeginTransaction();

                    try
                    {
                        foreach (DataRow dr in dt2.Rows)
                        {
                            string part = (string)dr["PaymentRefNumber"];

                            sqlCommand1 = new SqlCommand((string)String.Format("UPDATE dbo.tblCollectionReport SET IsRecordExit =0 WHERE PaymentRefNumber= '{0}'", part), db, transaction);

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
            }



        }


    }
}
