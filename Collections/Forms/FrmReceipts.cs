using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TaxSmartSuite.Class;
using System.Data.SqlClient;
using Collection.Classess;
using DevExpress.XtraGrid.Selection;
using System.Security.Cryptography;
using Collection.Report;
using System.Text.RegularExpressions;
using Collection.Classes;
using DevExpress.Utils;
using DevExpress.XtraPrinting;
using DevExpress.XtraReports.UI;
using DevExpress.XtraSplashScreen;
using Collections;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraPrinting.Drawing;
using LinqToExcel;

namespace Collection.Forms
{
    public partial class FrmReceipts : Form
    {
        private SqlCommand _command;

        private System.Data.DataSet DsPrintedView;

        private string PrintType;

        public static FrmReceipts publicInstance; ExcelQueryFactory excel = null;

        private System.Data.DataSet ds; string filenamesopen = String.Empty;

        SqlDataAdapter ada;

        int iCount = 0;

        private GridControl grid;

        private GridView view;

        GridCheckMarksSelection selection;

        AmountToWords amounttowords = new AmountToWords();

        string query, payerid, amount;

        bool isFirstGrid = true; bool isReprint = true;

        private bool isFirst = true;

        private bool IsAgency = false;

        private bool Isbank = false;

        SqlDataAdapter adp;

        private string SQL;

        string criteria, criteria2, criteria3;

        string Url;

        DataTable dt = new DataTable();

        DataTable dts = new DataTable();

        DataTable Dts = new DataTable();

        public static FrmReceipts publicStreetGroup;

        protected TransactionTypeCode iTransType;

        protected bool boolIsUpdate;

        private string user;

        DataTable temTable = new DataTable();
       
        private string strCollectionReportID;

        public FrmReceipts()
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

                InitializeComponent();

                publicInstance = this;

                publicStreetGroup = this;

                setImages();

                ToolStripEvent();

                iTransType = TransactionTypeCode.New;

                Load += OnFormLoad;

                btnSearch.Click += btnSearch_Click;

                btnPrint.Click += btnPrint_Click;

                btnMain.Click += btnMain_Click;

                btnupload.Click += Btnupload_Click;

                btnClear.Click += btnClear_Click;

                cboAgency.KeyPress += cboAgency_KeyPress;

                cboBank.KeyPress += cboBank_KeyPress;

                cboBranch.KeyPress += cboBranch_KeyPress;

                cboRevenue.KeyPress += cboRevenue_KeyPress;

                cboZone.KeyPress += cboZone_KeyPress;

                cboAgencyTest.EditValueChanged += cboAgencyTest_EditValueChanged;

                cboBankEdt.EditValueChanged += cboBankEdt_EditValueChanged;

                OnFormLoad(null, null);


                //if (Program.UserID == "" || Program.UserID == null)
                //{
                //    user = "Femi";
                //}
                //else
                //{
                user = Program.UserID;
                //}


                DateTime result = DateTime.Today.Subtract(TimeSpan.FromDays(1));
                dtpfrm.Value = result;
                dtpTo.Value = result;


                temTable.Columns.Add("EReceipt", typeof(string));
                temTable.Columns.Add("PaymentRefNumber", typeof(string));
                temTable.Columns.Add("UserId", typeof(string));

            }
            finally
            {
                SplashScreenManager.CloseForm(false);
            }
        }

        private void Btnupload_Click(object sender, EventArgs e)
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

                gridControl4.DataSource = null;

                List<string> missing = null;

                List<string> boths = null;

                List<string> mn = null;

                List<string> getJk = null;

                List<string> repm = null;

                List<string> m = null;

                string sheetName = "Sheet1"; string values = String.Empty;

                int gh = 0; label10.Text = String.Empty;

                DataTable dtpay = new DataTable();

                using (
                    OpenFileDialog _openFileDialogCSV = new OpenFileDialog()
                    {
                        InitialDirectory = Application.ExecutablePath,
                        Filter = "Excel files (*.xls)|*.xls|All files (*.*)|*.*",
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


                            dtpay.Clear();
                            var getData = (from a in excel.Worksheet(sheetName)
                                           select new Payment { PaymentRefNumber = a[0].Cast<string>() }).ToList();

                            if (getData.Any())
                            {
                                var grp = (from gb in getData
                                           group gb by gb.PaymentRefNumber
                                    into gbs
                                           where gbs.Count() > 1
                                           select new { gbs.Key }).ToList();

                                if (grp.Any())
                                {
                                    Common.setMessageBox("Import Excell Contain Duplicate Transaction.Please Treat it",
                                        Program.ApplicationName, 3);

                                    var ghk = grp.Select(x => x.Key).ToList();

                                    using (FrmMissingExcel display = new FrmMissingExcel())
                                    {
                                        display.showData(ghk);
                                        display.FormBorderStyle =
                                            System.Windows.Forms.FormBorderStyle.FixedSingle;
                                        display.lblInfo.Text =
                                            String.Format("{0} Duplicate Record(s) exist.", ghk.Count());
                                        display.lblInfo.ForeColor = Color.Red;

                                        display.ShowDialog();
                                    }
                                }
                                else
                                {
                                    dtpay = getData.ToDataTable();

                                    int j = 0;

                                    foreach (DataRow dr in dtpay.Rows)
                                    {
                                        values += string.Format("'{0}'", dr["paymentrefnumber"]);

                                        if (j + 1 < dtpay.Rows.Count)
                                            values += ",";
                                        ++j;
                                    }

                                    string quy =
                                        String.Format(
                                            "SELECT  ID , PaymentRefNumber , DepositSlipNumber ,CONVERT(VARCHAR, CONVERT(DATE, PaymentDate), 103) AS PaymentDate ,[PayerID] , UPPER(PayerName) AS PayerName ,AgencyName ,AgencyCode ,Description ,RevenueCode , BranchCode ,Amount , BankName + '-' + BranchName AS Bank ,EReceipts ,[GeneratedBy] ,[BankName] ,[BranchName] ,EReceiptsDate,StationCode FROM    Collection.tblCollectionReport  WHERE   PaymentRefNumber NOT IN ( SELECT  PaymentRefNumber  FROM  Receipt.tblReceipt ) AND Collection.tblCollectionReport.PaymentRefNumber NOT IN (SELECT  PaymentRefNumber FROM    Receipt.tblCollectionReceipt ) AND Collection.tblCollectionReport.EReceipts IS NOT NULL  AND Collection.tblCollectionReport.EReceiptsDate IS NOT NULL AND Collection.tblCollectionReport.Amount>0 AND RevenueCode NOT IN (SELECT  RevenueCode FROM    Receipt.tblRevenueReceiptException ) AND Collection.tblCollectionReport.PaymentRefNumber IN({0}) ORDER BY Collection.tblCollectionReport.AgencyName ,  Collection.tblCollectionReport.Description ,Collection.tblCollectionReport.EReceipts ,Collection.tblCollectionReport.EReceiptsDate",
                                            values);

                                    using (var ds = new System.Data.DataSet())
                                    {
                                        ds.Clear();
                                        dt.Clear();

                                        using (SqlDataAdapter ada = new SqlDataAdapter(quy, Logic.ConnectionString))
                                        {
                                            ada.Fill(ds, "table");
                                        }

                                        if (dtpay.Rows.Count != ds.Tables[0].Rows.Count)
                                        {
                                            //get miss one from the two table

                                            var matched = (from dtp in dtpay.AsEnumerable()
                                                           join ds2 in ds.Tables[0].AsEnumerable() on dtp.Field<string>(
                                                               "paymentrefnumber") equals ds2.Field<string>("paymentrefnumber")
                                                           select dtp
                                            ).ToList();

                                            missing = (from ds2 in dtpay.AsEnumerable()
                                                       where !matched.Contains(ds2)
                                                       select ds2[0].ToString()).ToList();

                                            boths = (from ds2 in dtpay.AsEnumerable()
                                                     where matched.Contains(ds2)
                                                     select ds2[0].ToString()).ToList();

                                            if (missing.Any())
                                            {

                                                values = string.Empty;
                                                int k = 0;

                                                //find get missing record
                                                for (int i = 0; i < missing.Count; i++)
                                                {
                                                    values += string.Format("'{0}'", missing[i]);

                                                    if (k + 1 < missing.Count)
                                                        values += ",";
                                                    ++k;
                                                }
                                                //check if the receipts is already be printed

                                                string strq =
                                                    string.Format(
                                                        "SELECT PaymentRefNumber FROM Receipt.tblCollectionReceipt WHERE PaymentRefNumber IN({0})",
                                                        values);

                                                using (var dstm = new System.Data.DataSet())
                                                {
                                                    dstm.Clear();
                                                    using (SqlDataAdapter adas =
                                                        new SqlDataAdapter(strq, Logic.ConnectionString))
                                                    {
                                                        adas.Fill(dstm, "table");
                                                    }

                                                    if (dstm.Tables[0].Rows.Count > 0 && dstm.Tables[0] != null)
                                                    {
                                                        repm = dstm.Tables[0].AsEnumerable()
                                                            .Select(g => g.Field<string>("PaymentRefNumber")
                                                            ).ToList();

                                                        m = (from o in missing
                                                             join ism in repm on o equals ism into ab
                                                             from c in ab.DefaultIfEmpty()
                                                             where c == null
                                                             select o).ToList();
                                                    }
                                                }

                                                if (m != null && m.Count > 0)
                                                {
                                                    MessageBox.Show(
                                                        string.Format("{0} Record(s) can not be found", m.Count()),
                                                        Program.ApplicationName, MessageBoxButtons.OK,
                                                        MessageBoxIcon.Information);

                                                    mn = m;

                                                    using (FrmMissingExcel display = new FrmMissingExcel())
                                                    {
                                                        display.showData(m);
                                                        display.FormBorderStyle =
                                                            System.Windows.Forms.FormBorderStyle.FixedSingle;
                                                        display.lblInfo.Text =
                                                            String.Format("{0} Receipt(s) do not exist.", m.Count());
                                                        display.lblInfo.ForeColor = Color.Red;

                                                        display.ShowDialog();
                                                    }
                                                }
                                                else
                                                {
                                                    if (repm != null && repm.Count > 0)
                                                    {

                                                        //ask if the print was sucessfull
                                                        DialogResult result =
                                                            MessageBox.Show(
                                                                String.Format(
                                                                    "{0} Receipt(s) have been printed before !. Do you wish to continue (Yes/No)?",
                                                                    repm.Count), Program.ApplicationName,
                                                                MessageBoxButtons.YesNo);

                                                        if (result == DialogResult.Yes)
                                                        {
                                                            using (FrmMissingExcel display = new FrmMissingExcel())
                                                            {
                                                                display.showData(repm);
                                                                display.FormBorderStyle =
                                                                    System.Windows.Forms.FormBorderStyle.FixedSingle;
                                                                display.lblInfo.Text =
                                                                    String.Format(
                                                                        "{0} Receipt(s) have been printed before !",
                                                                        repm.Count);
                                                                //"This receipt have been printed before. Kindly Use Normaly Reprint process to reprint them";
                                                                display.lblInfo.ForeColor = Color.Red;

                                                                display.ShowDialog();
                                                            }

                                                            if (mn == null)
                                                            {
                                                                getJk = boths.ToList();
                                                            }
                                                            else
                                                            {

                                                                getJk = boths.Concat(mn).ToList();
                                                            }

                                                            values = string.Empty;

                                                            int op = 0;

                                                            //find get missing record
                                                            for (int i = 0; i < getJk.Count(); i++)
                                                            {
                                                                values += string.Format("'{0}'", getJk[i]);

                                                                if (op + 1 < getJk.Count())
                                                                    values += ",";
                                                                ++op;
                                                            }

                                                            string qury =
                                                                String.Format(
                                                                    "SELECT  ID , PaymentRefNumber , DepositSlipNumber ,CONVERT(VARCHAR, CONVERT(DATE, PaymentDate), 103) AS PaymentDate ,[PayerID] , UPPER(PayerName) AS PayerName ,AgencyName ,AgencyCode ,Description ,RevenueCode , BranchCode ,Amount , BankName + '-' + BranchName AS Bank ,EReceipts ,[GeneratedBy] ,[BankName] ,[BranchName] ,EReceiptsDate,StationCode FROM    Collection.tblCollectionReport  WHERE   PaymentRefNumber NOT IN ( SELECT  PaymentRefNumber  FROM  Receipt.tblReceipt ) AND Collection.tblCollectionReport.PaymentRefNumber NOT IN (SELECT  PaymentRefNumber FROM    Receipt.tblCollectionReceipt ) AND Collection.tblCollectionReport.EReceipts IS NOT NULL  AND Collection.tblCollectionReport.EReceiptsDate IS NOT NULL AND Collection.tblCollectionReport.Amount>0 AND RevenueCode NOT IN (SELECT  RevenueCode FROM    Receipt.tblRevenueReceiptException ) AND Collection.tblCollectionReport.PaymentRefNumber IN({0}) ORDER BY Collection.tblCollectionReport.AgencyName ,  Collection.tblCollectionReport.Description ,Collection.tblCollectionReport.EReceipts ,Collection.tblCollectionReport.EReceiptsDate",
                                                                    values);
                                                            using (var dst = new System.Data.DataSet())
                                                            {
                                                                dst.Clear();

                                                                using (SqlDataAdapter adat =
                                                                    new SqlDataAdapter(qury, Logic.ConnectionString))
                                                                {
                                                                    adat.Fill(dst, "table");
                                                                }

                                                                if (dst.Tables[0].Rows.Count > 0 && dst.Tables[0] != null)
                                                                {
                                                                    gridControl4.DataSource = null;

                                                                    dt = dst.Tables[0];

                                                                    gridControl4.DataSource = ds.Tables[0];
                                                                    //
                                                                    gridView4.Columns["ID"].Visible = false;
                                                                    gridView4.Columns["Amount"].DisplayFormat.FormatType =
                                                                        FormatType.Numeric;
                                                                    gridView4.Columns["Amount"].DisplayFormat.FormatString =
                                                                        "n2";
                                                                    gridView4.Columns["PaymentDate"].DisplayFormat
                                                                            .FormatType =
                                                                        DevExpress.Utils.FormatType.DateTime;
                                                                    gridView4.Columns["PaymentDate"].DisplayFormat
                                                                            .FormatString =
                                                                        "dd/MM/yyyy";
                                                                    gridView4.Columns["EReceiptsDate"].DisplayFormat
                                                                            .FormatString =
                                                                        "dd/MM/yyyy";

                                                                    gridView4.Columns["AgencyCode"].Visible = false;
                                                                    gridView4.Columns["GeneratedBy"].Visible = false;
                                                                    gridView4.Columns["BankName"].Visible = false;
                                                                    gridView4.Columns["BranchName"].Visible = false;

                                                                    label10.Text =
                                                                        String.Format("Total Number of Payments: {0}",
                                                                            dt.Rows.Count);



                                                                    if (isFirstGrid)
                                                                    {
                                                                        selection = new GridCheckMarksSelection(gridView4,
                                                                            ref lblSelect);
                                                                        selection.CheckMarkColumn.VisibleIndex = 0;
                                                                        isFirstGrid = false;
                                                                    }
                                                                }
                                                            }
                                                        }
                                                        else
                                                        {
                                                            MessageBox.Show("User Operation has been cancelled",
                                                                Program.ApplicationName, MessageBoxButtons.OK,
                                                                MessageBoxIcon.Information);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        values = string.Empty;

                                                        int op = 0;

                                                        //find get missing record
                                                        for (int i = 0; i < boths.Count(); i++)
                                                        {
                                                            values += string.Format("'{0}'", boths[i]);

                                                            if (op + 1 < boths.Count())
                                                                values += ",";
                                                            ++op;
                                                        }

                                                        string qury =
                                                            String.Format(
                                                                "SELECT  ID , PaymentRefNumber , DepositSlipNumber ,CONVERT(VARCHAR, CONVERT(DATE, PaymentDate), 103) AS PaymentDate ,[PayerID] , UPPER(PayerName) AS PayerName ,AgencyName ,AgencyCode ,Description ,RevenueCode , BranchCode ,Amount , BankName + '-' + BranchName AS Bank ,EReceipts ,[GeneratedBy] ,[BankName] ,[BranchName] ,EReceiptsDate,StationCode FROM    Collection.tblCollectionReport  WHERE   PaymentRefNumber NOT IN ( SELECT  PaymentRefNumber  FROM  Receipt.tblReceipt ) AND Collection.tblCollectionReport.PaymentRefNumber NOT IN (SELECT  PaymentRefNumber FROM    Receipt.tblCollectionReceipt ) AND Collection.tblCollectionReport.EReceipts IS NOT NULL  AND Collection.tblCollectionReport.EReceiptsDate IS NOT NULL AND Collection.tblCollectionReport.Amount>0 AND RevenueCode NOT IN (SELECT  RevenueCode FROM    Receipt.tblRevenueReceiptException ) AND Collection.tblCollectionReport.PaymentRefNumber IN({0}) ORDER BY Collection.tblCollectionReport.AgencyName ,  Collection.tblCollectionReport.Description ,Collection.tblCollectionReport.EReceipts ,Collection.tblCollectionReport.EReceiptsDate", values);
                                                        using (var dst = new System.Data.DataSet())
                                                        {
                                                            dst.Clear();

                                                            using (SqlDataAdapter adat =
                                                                new SqlDataAdapter(qury, Logic.ConnectionString))
                                                            {
                                                                adat.Fill(dst, "table");
                                                            }

                                                            if (dst.Tables[0].Rows.Count > 0 && dst.Tables[0] != null)
                                                            {
                                                                gridControl4.DataSource = null;

                                                                dt = dst.Tables[0];

                                                                gridControl4.DataSource = ds.Tables[0];
                                                                //
                                                                gridView4.Columns["ID"].Visible = false;
                                                                gridView4.Columns["Amount"].DisplayFormat.FormatType =
                                                                    FormatType.Numeric;
                                                                gridView4.Columns["Amount"].DisplayFormat.FormatString =
                                                                    "n2";
                                                                gridView4.Columns["PaymentDate"].DisplayFormat
                                                                        .FormatType =
                                                                    DevExpress.Utils.FormatType.DateTime;
                                                                gridView4.Columns["PaymentDate"].DisplayFormat
                                                                        .FormatString =
                                                                    "dd/MM/yyyy";
                                                                gridView4.Columns["EReceiptsDate"].DisplayFormat
                                                                        .FormatString =
                                                                    "dd/MM/yyyy";

                                                                gridView4.Columns["AgencyCode"].Visible = false;
                                                                gridView4.Columns["GeneratedBy"].Visible = false;
                                                                gridView4.Columns["BankName"].Visible = false;
                                                                gridView4.Columns["BranchName"].Visible = false;

                                                                label10.Text =
                                                                    String.Format("Total Number of Payments: {0}",
                                                                        dt.Rows.Count);



                                                                if (isFirstGrid)
                                                                {
                                                                    selection = new GridCheckMarksSelection(gridView4,
                                                                        ref lblSelect);
                                                                    selection.CheckMarkColumn.VisibleIndex = 0;
                                                                    isFirstGrid = false;
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                                                {
                                                    dt = ds.Tables[0];
                                                    gridControl4.DataSource = ds.Tables[0];
                                                    //
                                                    gridView4.Columns["ID"].Visible = false;
                                                    gridView4.Columns["Amount"].DisplayFormat.FormatType =
                                                        FormatType.Numeric;
                                                    gridView4.Columns["Amount"].DisplayFormat.FormatString = "n2";
                                                    gridView4.Columns["PaymentDate"].DisplayFormat.FormatType =
                                                        DevExpress.Utils.FormatType.DateTime;
                                                    gridView4.Columns["PaymentDate"].DisplayFormat.FormatString =
                                                        "dd/MM/yyyy";
                                                    gridView4.Columns["EReceiptsDate"].DisplayFormat.FormatString =
                                                        "dd/MM/yyyy";

                                                    gridView4.Columns["AgencyCode"].Visible = false;
                                                    gridView4.Columns["GeneratedBy"].Visible = false;
                                                    gridView4.Columns["BankName"].Visible = false;
                                                    gridView4.Columns["BranchName"].Visible = false;

                                                    label10.Text =
                                                        String.Format("Total Number of Payments: {0}", dt.Rows.Count);



                                                    if (isFirstGrid)
                                                    {
                                                        selection = new GridCheckMarksSelection(gridView4,
                                                            ref lblSelect);
                                                        selection.CheckMarkColumn.VisibleIndex = 0;
                                                        isFirstGrid = false;
                                                    }
                                                }
                                                else
                                                {
                                                    Common.setMessageBox("No Record Found for selected record search",
                                                        "Receipts", 2);
                                                    return;
                                                }
                                            }

                                        }
                                        else
                                        {
                                            if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                                            {
                                                dt = ds.Tables[0];
                                                gridControl4.DataSource = ds.Tables[0];
                                                //
                                                gridView4.Columns["ID"].Visible = false;
                                                gridView4.Columns["Amount"].DisplayFormat.FormatType =
                                                    FormatType.Numeric;
                                                gridView4.Columns["Amount"].DisplayFormat.FormatString = "n2";
                                                gridView4.Columns["PaymentDate"].DisplayFormat.FormatType =
                                                    DevExpress.Utils.FormatType.DateTime;
                                                gridView4.Columns["PaymentDate"].DisplayFormat.FormatString =
                                                    "dd/MM/yyyy";
                                                gridView4.Columns["EReceiptsDate"].DisplayFormat.FormatString =
                                                    "dd/MM/yyyy";

                                                gridView4.Columns["AgencyCode"].Visible = false;
                                                gridView4.Columns["GeneratedBy"].Visible = false;
                                                gridView4.Columns["BankName"].Visible = false;
                                                gridView4.Columns["BranchName"].Visible = false;

                                                label10.Text =
                                                    String.Format("Total Number of Payments: {0}", dt.Rows.Count);



                                                if (isFirstGrid)
                                                {
                                                    selection = new GridCheckMarksSelection(gridView4,
                                                        ref lblSelect);
                                                    selection.CheckMarkColumn.VisibleIndex = 0;
                                                    isFirstGrid = false;
                                                }
                                            }
                                            else
                                            {
                                                Common.setMessageBox("No Record Found for selected record search",
                                                    "Receipts", 2);
                                                return;
                                            }
                                        }

                                    }
                                }


                            }
                        }
                    }
                }
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

        void cboBankEdt_EditValueChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(cboBankEdt.EditValue.ToString()) || cboBankEdt.EditValue != null)
            {
                setDBComboBoxBranch();
            }
        }

        void cboAgencyTest_EditValueChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(cboAgencyTest.EditValue.ToString()) || cboAgencyTest.EditValue != null)
            {
                setDBComboBoxReveneu();
            }
        }

        void btnClear_Click(object sender, EventArgs e)
        {
            //cboAgency.SelectedIndex = -1; cboBank.SelectedIndex = -1; cboBranch.SelectedIndex = -1; cboRevenue.SelectedIndex = -1; cboZone.SelectedIndex = -1; txtPayref.Text = string.Empty;
            cboProfile.SelectedIndex = -1; gridControl4.DataSource = null; btnMain.Enabled = false; btnPrint.Enabled = false; label10.Text = string.Empty;
        }

        void cboZone_KeyPress(object sender, KeyPressEventArgs e)
        {
            Methods.AutoComplete(cboZone, e, true);
        }

        void cboRevenue_KeyPress(object sender, KeyPressEventArgs e)
        {
            Methods.AutoComplete(cboRevenue, e, true);
        }

        void cboBranch_KeyPress(object sender, KeyPressEventArgs e)
        {
            Methods.AutoComplete(cboBranch, e, true);
        }

        void cboBank_KeyPress(object sender, KeyPressEventArgs e)
        {
            Methods.AutoComplete(cboBank, e, true);
        }

        void cboAgency_KeyPress(object sender, KeyPressEventArgs e)
        {
            Methods.AutoComplete(cboAgency, e, true);
        }

        void btnMain_Click(object sender, EventArgs e)
        {
            btnPrint.Enabled = false;

            //criteria = String.Format("WHERE ([EReceiptsDate] BETWEEN '{0} 00:00:00' AND '{0} 23:59:59') AND EReceipts IS NOT NULL AND Amount > 0 AND BatchNumber IS NULL ", dtpfrom.Value.Date.ToString("MM/dd/yyyy"));
            //check for record waiting manifest
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

                if (dts.Rows.Count > 0)
                {
                    if (Program.stateCode == "20")
                    {
                        criteria3 = GetRecordWaitMainfest(dts);
                        using (
                           FrmMainFest frmMainFest = new FrmMainFest(strCollectionReportID, criteria3, criteria2, false, Convert.ToInt32(radioGroup1.EditValue))
                           {
                               IDList = strCollectionReportID
                           })
                        {
                            frmMainFest.ShowDialog();
                        }
                    }
                    else
                    {
                        criteria3 = GetRecordWaitMainfest(dts);

                        using (FrmMainFest frmMainFest = new FrmMainFest(strCollectionReportID, criteria3, criteria2, false) { IDList = strCollectionReportID })
                        {
                            frmMainFest.ShowDialog();
                        }
                    }

                }
                else
                {
                    if (Program.stateCode == "20")
                    {
                        using (
                             FrmMainFest frmMainFest = new FrmMainFest(strCollectionReportID, criteria3, criteria2, false, Convert.ToInt32(radioGroup1.EditValue))
                             {
                                 IDList = strCollectionReportID
                             })
                        {
                            frmMainFest.ShowDialog();
                        }
                    }
                    else
                    {
                        using (FrmMainFest frmMainFest = new FrmMainFest(strCollectionReportID, criteria3, criteria2, false) { IDList = strCollectionReportID })
                        {
                            frmMainFest.ShowDialog();
                        }
                    }


                }
            }
            finally
            {
                SplashScreenManager.CloseForm(false);
            }
            //CountMain();

        }

        internal GridCheckMarksSelection Selection
        {
            get { return selection; }
        }

        private void setImages()
        {
            tsbNew.Image = MDIMain.publicMDIParent.i16x16.Images[5];
            tsbEdit.Image = MDIMain.publicMDIParent.i16x16.Images[6];
            tsbDelete.Image = MDIMain.publicMDIParent.i16x16.Images[7];
            tsbReload.Image = MDIMain.publicMDIParent.i16x16.Images[0];
            tsbClose.Image = MDIMain.publicMDIParent.i16x16.Images[11];

            //bttnCancel.Image = MDIMain.publicMDIParent.i32x32.Images[9];
            btnMain.Image = MDIMain.publicMDIParent.i32x32.Images[8];
            btnPrint.Image = MDIMain.publicMDIParent.i32x32.Images[29];
            btnSearch.Image = MDIMain.publicMDIParent.i32x32.Images[2];
            btnClear.Image = MDIMain.publicMDIParent.i32x32.Images[3];

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
                boolIsUpdate = false;
            }
            else if (sender == tsbEdit)
            {
                iTransType = TransactionTypeCode.Edit;

                boolIsUpdate = true;

            }

        }

        void OnFormLoad(object sender, EventArgs e)
        {
            //ShowForm();
            //get state code
            //if (Program.stateCode == "20")
            //{
            //    groupBox4.Visible = true;
            //    //groupBox5.Visible = false;
            //    //groupBox6.Visible = true;
            //    radioGroup1.EditValueChanged += RadioGroup1_EditValueChanged;

            //}
            //else
            //{
            groupBox4.Visible = false;
            //    //groupBox5.Visible = true;
            //    //groupBox6.Visible = false;
            //}

            //setDBComboBox();

            setDBComboProfile();

            //setDBComoboxCurrency();

            //setDBComboBoxAgency();

            //setDBComboBoxBank();

            //cboAgency.SelectedIndexChanged += cboAgency_SelectedIndexChanged;

            //cboBank.SelectedIndexChanged += cboBank_SelectedIndexChanged;

            bttnCreate.Click += bttnCreate_Click;

            //

            //

            CountMain();

            //getReceipt();

            isFirst = false;

            isReprint = false;

            //generate number
            string test = String.Format("{0:d9}", (DateTime.Now.Ticks / 10) % 1000000000);

            string number = GetUniqueKey();

            dtpfrom.Format = DateTimePickerFormat.Custom; dtpfrom.CustomFormat = "dd/MM/yyyy";

            //cboAgency_SelectedIndexChanged(null, null); cboBank_SelectedIndexChanged(null, null);

        }

        private void RadioGroup1_EditValueChanged(object sender, EventArgs e)
        {
            if (Convert.ToInt32(radioGroup1.EditValue) != 1)
            {
                btnPrint.Enabled = false; btnMain.Enabled = false;
            }
            else
            {
                btnPrint.Enabled = true; btnMain.Enabled = true;
            }
        }

        void bttnCreate_Click(object sender, EventArgs e)
        {
            //using (FrmProfile Profile = new FrmProfile() { FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog })
            //{
            //    Profile.ShowDialog();
            //}

            using (FrmCriteria Profile = new FrmCriteria() { FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog })
            {
                Profile.ShowDialog();
            }


        }

        void cboBank_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                if (cboBank.SelectedValue != null && !Isbank)
                {
                    setDBComboBoxBranch();
                }
            }
            finally
            {
                SplashScreenManager.CloseForm(false);
            }
        }

        void cboAgency_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

                if (cboAgency.SelectedValue != null && !IsAgency)
                {
                    setDBComboBoxReveneu();
                }
            }
            finally
            {
                SplashScreenManager.CloseForm(false);
            }
        }

        void btnPrint_Click(object sender, EventArgs e)
        {
            //criteria = String.Format("WHERE ([EReceiptsDate] BETWEEN '{0} 00:00:00' AND '{0} 23:59:59') AND EReceipts IS NOT NULL AND Amount > 0 AND BatchNumber IS NULL ", dtpfrom.Value.Date.ToString("MM/dd/yyyy"));
            //check for record waiting manifest
            if (dts.Rows.Count > 0)
            {
                Common.setMessageBox("There are some Printed Receipt with out Control Number.! Click on Control Number  to Apply It", Program.ApplicationName, 2);
                btnMain.Visible = true;
                btnMain.Enabled = true;
                return;

            }
            else
            {
                if (selection == null)
                {
                    Common.setMessageBox("No Selection Made for Printing of Receipts", Program.ApplicationName, 3);
                    return;
                }


                if (selection.SelectedCount == 0)
                {
                    Common.setMessageBox("No Selection Made for Printing of Receipts", Program.ApplicationName, 3);
                    return;

                }
                else
                {
                    GetPayRef();
                }

                btnMain.Enabled = true;

                btnPrint.Enabled = true;

                //btnSearch.Enabled = false;

                gridControl4.Enabled = false;

                //Common.setMessageBox(convert.tostring(DateTime.Now), Program.ApplicationName, 1);



                //ask if the print was sucessfull
                DialogResult result = MessageBox.Show(" Is Receipt Printing Successful ?", Program.ApplicationName, MessageBoxButtons.YesNo);

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
                            btnMain.Enabled = true;
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
                                string query1 = String.Format("DELETE FROM Receipt.tblCollectionReceipt WHERE PaymentRefNumber IN (SELECT PaymentRefNumber FROM Receipt.tblReceipt WHERE SentBy='{0}')", Program.UserID);

                                using (SqlCommand sqlCommand = new SqlCommand(query1, db, transaction))
                                {
                                    sqlCommand.ExecuteNonQuery();
                                }

                                string query = String.Format("DELETE  FROM Receipt.tblReceipt where SentBy='{0}'", Program.UserID);

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

        void PrintingSystem_EndPrint(object sender, EventArgs e)
        {
            //Common.setMessageBox("Test Printing",Program.ApplicationName,1);
            //update the collection table by seting the isprinted to true
            using (SqlConnection db = new SqlConnection(Logic.ConnectionString))
            {
                SqlTransaction transaction;

                db.Open();

                transaction = db.BeginTransaction();

                try
                {
                    string query1 = String.Format("UPDATE tblCollectionReport SET isPrinted=1,IsPrintedDate= '{0}' WHERE [PaymentRefNumber] IN ({1})", DateTime.Now, criteria3);

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

        void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                gridControl4.DataSource = null;

                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

                if (dts.Rows.Count > 0)
                {
                    Common.setMessageBox(
                        "Sorry...,There some Printed Receipt waiting for Control Number.! Click on Mainfest Button to Apply It",
                        Program.ApplicationName, 2);
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
                    //        if (Convert.ToInt32(radioGroup1.EditValue) != 1)
                    //        {
                    //            using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
                    //            {
                    //                connect.Open();
                    //                _command = new SqlCommand("doProfieSearch2", connect) { CommandType = CommandType.StoredProcedure };
                    //                _command.Parameters.Add(new SqlParameter("@ProfileID", SqlDbType.Int)).Value =
                    //                    Convert.ToInt32(cboProfile.SelectedValue);
                    //                _command.Parameters.Add(new SqlParameter("@date1", SqlDbType.VarChar)).Value =
                    //                    string.Format("{0:yyyy/MM/dd 00:00:00}", dtpfrm.Value);
                    //                _command.Parameters.Add(new SqlParameter("@date2", SqlDbType.VarChar)).Value =
                    //                    string.Format("{0:yyyy/MM/dd 23:59:59}", dtpTo.Value);
                    //                _command.Parameters.Add(new SqlParameter("@reVal", SqlDbType.Int)).Value =
                    //                    Convert.ToInt32(radioGroup1.EditValue);

                    //                _command.CommandTimeout = 0;

                    //                System.Data.DataSet response = new System.Data.DataSet();

                    //                adp = new SqlDataAdapter(_command);
                    //                adp.Fill(response);

                    //                connect.Close();
                    //                if (response.Tables[0].Rows[0]["returnCode"].ToString() != "00")
                    //                {
                    //                    Common.setMessageBox(response.Tables[0].Rows[0]["returnMessage"].ToString(),
                    //                        "Receipts", 2);
                    //                    return;
                    //                }
                    //                else
                    //                {
                    //                    //dtd.Clear();
                    //                    //dtd = response.Tables[1];
                    //                    //gridControl1.DataSource = dtd.DefaultView;
                    //                    if (response.Tables[1] != null && response.Tables[0].Rows.Count > 0)
                    //                    {
                    //                        using (
                    //                            Frmduplicate duplicatefrm =
                    //                                new Frmduplicate(response.Tables[1], Convert.ToInt32(radioGroup1.EditValue),
                    //                                false))
                    //                        {
                    //                            duplicatefrm.FormBorderStyle =
                    //                                System.Windows.Forms.FormBorderStyle.FixedDialog;
                    //                            duplicatefrm.ShowDialog();
                    //                        }
                    //                    }
                    //                    else
                    //                    {
                    //                        Common.setMessageBox("Previson Receipt Record Not Found", "Receipt", 2); return;
                    //                    }
                    //                }
                    //            }


                    //        }
                    //        else
                    //        {
                    //            gridControl4.Enabled = true; adp = new SqlDataAdapter();

                    //            try
                    //            {
                    //                using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
                    //                {
                    //                    connect.Open();
                    //                    _command = new SqlCommand("doProfieSearch", connect)
                    //                    {
                    //                        CommandType = CommandType.StoredProcedure
                    //                    };
                    //                    _command.Parameters.Add(new SqlParameter("@ProfileID", SqlDbType.Int)).Value =
                    //                        Convert.ToInt32(cboProfile.SelectedValue);
                    //                    _command.Parameters.Add(new SqlParameter("@date1", SqlDbType.VarChar)).Value =
                    //                        string.Format("{0:yyyy/MM/dd 00:00:00}", dtpfrm.Value);
                    //                    //string.Format("{0:MM/dd/yyyy 00:00:00}", dtpfrm.Value);
                    //                    _command.Parameters.Add(new SqlParameter("@date2", SqlDbType.VarChar)).Value =
                    //                        string.Format("{0:yyyy/MM/dd 23:59:59}", dtpTo.Value);
                    //                    //string.Format("{0:MM/dd/yyyy 23:59:59}", dtpTo.Value);

                    //                    _command.CommandTimeout = 0;

                    //                    System.Data.DataSet response = new System.Data.DataSet();

                    //                    adp = new SqlDataAdapter(_command);
                    //                    adp.Fill(response);

                    //                    connect.Close();
                    //                    if (response.Tables[0].Rows[0]["returnCode"].ToString() != "00")
                    //                    {
                    //                        Common.setMessageBox(response.Tables[0].Rows[0]["returnMessage"].ToString(), "Receipts",
                    //                            2);
                    //                        gridControl4.DataSource = null;
                    //                        return;
                    //                    }
                    //                    else
                    //                    {
                    //                        if (response.Tables[1] != null && response.Tables[1].Rows.Count > 0)
                    //                        {
                    //                            dt.Clear();
                    //                            dt = response.Tables[1];
                    //                            gridControl4.DataSource  = dt;
                    //                            gridView4.BestFitColumns();
                    //                            gridView4.Columns["ID"].Visible = false;
                    //gridView4.Columns["Amount"].DisplayFormat.FormatType = FormatType.Numeric;
                    //gridView4.Columns["Amount"].DisplayFormat.FormatString = "n2";
                    //                            gridView4.Columns["PaymentDate"].DisplayFormat.FormatType =
                    //                                DevExpress.Utils.FormatType.DateTime;
                    //                            gridView4.Columns["PaymentDate"].DisplayFormat.FormatString = "dd/MM/yyyy";
                    //                            gridView4.Columns["EReceiptsDate"].DisplayFormat.FormatString = "dd/MM/yyyy";

                    //                            gridView4.Columns["AgencyCode"].Visible = false;
                    //                            gridView4.Columns["GeneratedBy"].Visible = false;
                    //                            gridView4.Columns["BankName"].Visible = false;
                    //                            gridView4.Columns["BranchName"].Visible = false;
                    //                        }
                    //                        else
                    //                        {
                    //                            Common.setMessageBox("No Record Found for selected record search", "Receipts", 2);
                    //                            return;
                    //                        }


                    //                    }
                    //                }

                    //                label10.Text = String.Format("Total Number of Payments: {0}", dt.Rows.Count);

                    //            }
                    //            catch (Exception ex)
                    //            {
                    //                Common.setMessageBox(ex.Message, Program.ApplicationName, 3);
                    //                return;
                    //            }
                    //            finally
                    //            {
                    //                SplashScreenManager.CloseForm(false);
                    //            }

                    //            if (isFirstGrid)
                    //            {
                    //                selection = new GridCheckMarksSelection(gridView4, ref lblSelect);
                    //                selection.CheckMarkColumn.VisibleIndex = 0;
                    //                isFirstGrid = false;
                    //            }
                    //        }
                    //    }
                    //}
                    //else
                    //{
                    gridControl4.Enabled = true; adp = new SqlDataAdapter();

                    try
                    {
                        using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
                        {
                            connect.Open();
                            _command = new SqlCommand("doProfieSearch", connect)
                            {
                                CommandType = CommandType.StoredProcedure
                            };
                            _command.Parameters.Add(new SqlParameter("@ProfileID", SqlDbType.Int)).Value =
                                Convert.ToInt32(cboProfile.SelectedValue);
                            _command.Parameters.Add(new SqlParameter("@date1", SqlDbType.VarChar)).Value =
                                string.Format("{0:yyyy/MM/dd 00:00:00}", dtpfrm.Value);
                            //string.Format("{0:MM/dd/yyyy 00:00:00}", dtpfrm.Value);
                            _command.Parameters.Add(new SqlParameter("@date2", SqlDbType.VarChar)).Value =
                                string.Format("{0:yyyy/MM/dd 23:59:59}", dtpTo.Value);
                            //string.Format("{0:MM/dd/yyyy 23:59:59}", dtpTo.Value);

                            _command.CommandTimeout = 0;

                            System.Data.DataSet response = new System.Data.DataSet();

                            adp = new SqlDataAdapter(_command);
                            adp.Fill(response);

                            connect.Close();
                            if (response.Tables[0].Rows[0]["returnCode"].ToString() != "00")
                            {
                                Common.setMessageBox(response.Tables[0].Rows[0]["returnMessage"].ToString(), "Receipts",
                                    2);
                                gridControl4.DataSource = null;
                                return;
                            }
                            else
                            {
                                if (response.Tables[1] != null && response.Tables[1].Rows.Count > 0)
                                {
                                    dt.Clear();
                                    dt = response.Tables[1];
                                    gridControl4.DataSource = response.Tables[1];
                                    //
                                    gridView4.Columns["ID"].Visible = false;
                                    gridView4.Columns["Amount"].DisplayFormat.FormatType = FormatType.Numeric;
                                    gridView4.Columns["Amount"].DisplayFormat.FormatString = "n2";
                                    gridView4.Columns["PaymentDate"].DisplayFormat.FormatType =
                                        DevExpress.Utils.FormatType.DateTime;
                                    gridView4.Columns["PaymentDate"].DisplayFormat.FormatString = "dd/MM/yyyy";
                                    gridView4.Columns["EReceiptsDate"].DisplayFormat.FormatString = "dd/MM/yyyy";

                                    gridView4.Columns["AgencyCode"].Visible = false;
                                    gridView4.Columns["GeneratedBy"].Visible = false;
                                    gridView4.Columns["BankName"].Visible = false;
                                    gridView4.Columns["BranchName"].Visible = false;
                                    //gridView4.BestFitColumns();
                                }
                                else
                                {
                                    Common.setMessageBox("No Record Found for selected record search", "Receipts", 2);
                                    return;
                                }


                            }
                        }

                        label10.Text = String.Format("Total Number of Payments: {0}", dt.Rows.Count);

                    }
                    catch (Exception ex)
                    {
                        Common.setMessageBox(ex.Message, Program.ApplicationName, 3);
                        return;
                    }
                    finally
                    {
                        SplashScreenManager.CloseForm(false);
                    }

                    if (isFirstGrid)
                    {
                        selection = new GridCheckMarksSelection(gridView4, ref lblSelect);
                        selection.CheckMarkColumn.VisibleIndex = 0;
                        isFirstGrid = false;
                    }

                    //if (dt.Rows.Count > 0)
                    //{
                    //    btnPrint.Enabled = true;

                    //    //btnMain.Enabled = true;
                    //}
                    //else
                    //{
                    //    btnPrint.Enabled = false;

                    //    //btnMain.Enabled = false;
                    //}


                    //}
                }


            }
            catch (Exception ex)
            {
                Common.setMessageBox(ex.Message, Program.ApplicationName, 3);
                return;
            }
            finally
            {
                SplashScreenManager.CloseForm(false);
            }

            #region old

            //gridView4.Columns.Clear();



            //criteria = String.Format("AND [EReceiptsDate] >='{0}' AND [EReceiptsDate]<= '{1}' ", string.Format("{0:MM/dd/yyyy 00:00:00}", dtpfrm.Value), string.Format("{0:MM/dd/yyyy 23:59:59}", dtpTo.Value));

            //criteria2 = String.Format("AND [EReceiptsDate] >='{0}' AND [EReceiptsDate]<= '{1}'", string.Format("{0:MM/dd/yyyy 00:00:00}", dtpfrm.Value), string.Format("{0:MM/dd/yyyy 23:59:59}", dtpTo.Value));


            //if (cboAgencyTest.EditValue == null || cboAgencyTest.EditValue.ToString() != "")
            //{
            //    string values = string.Empty;

            //    object[] lol = cboAgencyTest.EditValue.ToString().Split(',');

            //    int i = 0;

            //foreach (object obj in lol)
            //{
            //    values += string.Format("'{0}'", obj.ToString().Trim());

            //    if (i + 1 < lol.Count())
            //        values += ",";
            //    ++i;
            //}

            //    criteria += String.Format(" AND AgencyCode in ({0})", values);
            //}


            //if (cboRevenueEdt.EditValue == null || cboRevenueEdt.EditValue.ToString() != "")
            //{
            //    string values = string.Empty;

            //    object[] lol = cboRevenueEdt.EditValue.ToString().Split(',');

            //    int i = 0;

            //    foreach (object obj in lol)
            //    {
            //        values += string.Format("'{0}'", obj.ToString().Trim());

            //        if (i + 1 < lol.Count())
            //            values += ",";
            //        ++i;
            //    }

            //    criteria += String.Format(" AND RevenueCode in ({0})", values);
            //}

            //if (cboBankEdt.EditValue == null || cboBankEdt.EditValue.ToString() != "")
            //{
            //    string values = string.Empty;

            //    object[] lol = cboBankEdt.EditValue.ToString().Split(',');

            //    int i = 0;

            //    foreach (object obj in lol)
            //    {
            //        values += string.Format("'{0}'", obj.ToString().Trim());

            //        if (i + 1 < lol.Count())
            //            values += ",";
            //        ++i;
            //    }


            //    criteria += String.Format(" AND BankCode in ({0})", values);

            //}

            //if (cboBranchesEdt.EditValue == null || cboBranchesEdt.EditValue.ToString() != "")
            //{

            //    string values = string.Empty;

            //    object[] lol = cboBranchesEdt.EditValue.ToString().Split(',');

            //    int i = 0;

            //    foreach (object obj in lol)
            //    {
            //        values += string.Format("'{0}'", obj.ToString().Trim());

            //        if (i + 1 < lol.Count())
            //            values += ",";
            //        ++i;
            //    }


            //    criteria += String.Format(" AND BranchCode in ({0})", values);
            //}

            //if (cboZone.SelectedIndex > -1)
            //{
            //    criteria += String.Format(" AND ZoneName = '{0}'", cboZone.SelectedValue);
            //}

            //if (!string.IsNullOrEmpty(txtPayref.Text))
            //{
            //    criteria += String.Format(" AND PaymentRefNumber = '{0}'", txtPayref.Text);
            //}


            //switch (Program.intCode)
            //{
            //    case 13://Akwa Ibom state
            //        query = String.Format("SELECT  ID , PaymentRefNumber,DepositSlipNumber,CONVERT(VARCHAR, CONVERT(DATE, PaymentDate), 103) as PaymentDate,[PayerID],UPPER(PayerName) as PayerName,AgencyName,AgencyCode,Description,RevenueCode,Amount,BankName+ '-'+ BranchName AS Bank,EReceipts,[GeneratedBy],[BankName],[BranchName],EReceiptsDate from Collection.tblCollectionReport WHERE PaymentRefNumber NOT IN (SELECT PaymentRefNumber FROM Receipt.tblReceipt ) AND Collection.tblCollectionReport.PaymentRefNumber NOT IN (SELECT PaymentRefNumber FROM Receipt.tblCollectionReceipt) AND Collection.tblCollectionReport.EReceipts IS NOT NULL  {0} AND RevenueCode NOT IN(SELECT RevenueCode FROM Receipt.tblRevenueReceiptException)   ORDER BY Collection.tblCollectionReport.AgencyName ,Collection.tblCollectionReport.Description,Collection.tblCollectionReport.EReceipts", criteria);
            //        break;
            //    case 20:
            //        query = String.Format("SELECT  ID , PaymentRefNumber,DepositSlipNumber,CONVERT(VARCHAR, CONVERT(DATE, PaymentDate), 103) as PaymentDate,[PayerID],UPPER(PayerName) as PayerName,AgencyName,AgencyCode,Description,RevenueCode,Amount,BankName+ '-'+ BranchName AS Bank,EReceipts,[GeneratedBy],[BankName],[BranchName],EReceiptsDate from Collection.tblCollectionReport WHERE PaymentRefNumber NOT IN (SELECT PaymentRefNumber FROM Receipt.tblReceipt ) AND Collection.tblCollectionReport.PaymentRefNumber NOT IN (SELECT PaymentRefNumber FROM Receipt.tblCollectionReceipt) AND Collection.tblCollectionReport.EReceipts IS NOT NULL {0}   ORDER BY Collection.tblCollectionReport.AgencyName ,Collection.tblCollectionReport.Description,Collection.tblCollectionReport.EReceipts", criteria);
            //        break;
            //    case 37://ogun state
            //        query = String.Format("SELECT  ID , PaymentRefNumber,DepositSlipNumber,CONVERT(VARCHAR, CONVERT(DATE, PaymentDate), 103) as PaymentDate,[PayerID],UPPER(PayerName) as PayerName,AgencyName,AgencyCode,Description,RevenueCode,Amount,BankName+ '-'+ BranchName AS Bank,EReceipts,[GeneratedBy],[BankName],[BranchName],EReceiptsDate from Collection.tblCollectionReport WHERE PaymentRefNumber NOT IN (SELECT PaymentRefNumber FROM Receipt.tblReceipt ) AND Collection.tblCollectionReport.PaymentRefNumber NOT IN (SELECT PaymentRefNumber FROM Receipt.tblCollectionReceipt) AND Collection.tblCollectionReport.EReceipts IS NOT NULL  {0}   ORDER BY Collection.tblCollectionReport.AgencyName ,Collection.tblCollectionReport.Description,Collection.tblCollectionReport.EReceipts", criteria);
            //        break;

            //    case 40://oyo state
            //        query = String.Format("SELECT  ID , PaymentRefNumber,DepositSlipNumber,CONVERT(VARCHAR, CONVERT(DATE, PaymentDate), 103) as PaymentDate,[PayerID],UPPER(PayerName) as PayerName,AgencyName,AgencyCode,Description,RevenueCode,Amount,BankName+ '-'+ BranchName AS Bank,EReceipts,[GeneratedBy],[BankName],[BranchName],EReceiptsDate from Collection.tblCollectionReport WHERE PaymentRefNumber NOT IN (SELECT PaymentRefNumber FROM Receipt.tblReceipt ) AND Collection.tblCollectionReport.PaymentRefNumber NOT IN (SELECT PaymentRefNumber FROM Receipt.tblCollectionReceipt) AND Collection.tblCollectionReport.EReceipts IS NOT NULL {0} AND RevenueCode NOT IN(SELECT RevenueCode FROM Receipt.tblRevenueReceiptException)   ORDER BY Collection.tblCollectionReport.AgencyName ,Collection.tblCollectionReport.Description,Collection.tblCollectionReport.EReceipts", criteria);
            //        break;

            //    case 32://kogi state
            //        query = String.Format("SELECT  ID , PaymentRefNumber,DepositSlipNumber,CONVERT(VARCHAR, CONVERT(DATE, PaymentDate), 103) as PaymentDate,[PayerID],UPPER(PayerName) as PayerName,AgencyName,AgencyCode,Description,RevenueCode,Amount,BankName+ '-'+ BranchName AS Bank,EReceipts,[GeneratedBy],[BankName],[BranchName],EReceiptsDate from Collection.tblCollectionReport WHERE PaymentRefNumber NOT IN (SELECT PaymentRefNumber FROM Receipt.tblReceipt ) AND Collection.tblCollectionReport.PaymentRefNumber NOT IN (SELECT PaymentRefNumber FROM Receipt.tblCollectionReceipt) AND Collection.tblCollectionReport.EReceipts IS NOT NULL {0}   ORDER BY Collection.tblCollectionReport.AgencyName ,Collection.tblCollectionReport.Description,Collection.tblCollectionReport.EReceipts", criteria);
            //        break;

            //    default:
            //        break;
            //}

            #endregion


        }

        void GetPayRef()
        {
            string values = string.Empty;

            lblSelect.Text = string.Empty; int intval = 0;

            int j = 0;

            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

                //if (Program.stateCode == "20")
                //{
                //    intval = Convert.ToInt32(radioGroup1.EditValue);
                //}


                if (!isReprint)
                {
                    temTable.Clear();

                    for (int i = 0; i < selection.SelectedCount; i++)
                    {
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
                        _command.CommandTimeout = 0;
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
                                            string strFormat = null;

                                            switch (Program.intCode)
                                            {
                                                case 20://detla state
                                                    query = string.Format("SELECT [ID] , PaymentPeriod,[Provider] , [Channel] ,[PaymentRefNumber] , [DepositSlipNumber] , CONVERT(VARCHAR,CONVERT(DATE,[PaymentDate])) AS PaymentDate,[PayerID] , UPPER([PayerName]) AS [PayerName], [Amount] ,[PaymentMethod] ,[ChequeNumber] ,[ChequeValueDate] ,[ChequeStatus] ,[DateChequeReturned] ,[TelephoneNumber] ,[ReceiptNo] ,[ReceiptDate] ,UPPER([PayerAddress]) as [PayerAddress] ,[User] ,[RevenueCode] , tblCollectionReport.Description , [ChequeBankCode] ,[ChequeBankName] ,[AgencyName] ,[AgencyCode] ,[BankCode] ,[BankName] ,[BranchCode] ,[BranchName] ,[ZoneCode] ,[ZoneName] ,[Username] ,[AmountWords] ,[EReceipts] ,[EReceiptsDate] ,[GeneratedBy] ,[StationCode] ,(SELECT TOP 1 StationName FROM Receipt.tblStation  WHERE tblStation.StationCode COLLATE DATABASE_DEFAULT = Collection.tblCollectionReport.[StationCode]) AS StationName, Symbol , Surfix , tblCurrency.Description AS prefix  from Collection.tblCollectionReport  JOIN Reconciliation.tblCurrency ON tblCurrency.CurrencyCode = tblCollectionReport.CurrencyCode WHERE PaymentRefNumber COLLATE DATABASE_DEFAULT  IN (SELECT PaymentRefNumber FROM Receipt.tblReceipt where SentBy='{0}')   ORDER BY Collection.tblCollectionReport.AgencyName ,Collection.tblCollectionReport.Description,Collection.tblCollectionReport.EReceipts", Program.UserID);
                                                    break;
                                                default:
                                                    query = string.Format("SELECT [ID] ,PaymentPeriod, [Provider] , [Channel] ,[PaymentRefNumber] , [DepositSlipNumber] , CONVERT(VARCHAR,CONVERT(DATE,[PaymentDate])) AS PaymentDate,[PayerID] , UPPER([PayerName]) AS [PayerName], [Amount] ,[PaymentMethod] ,[ChequeNumber] ,[ChequeValueDate] ,[ChequeStatus] ,[DateChequeReturned] ,[TelephoneNumber] ,[ReceiptNo] ,[ReceiptDate] ,UPPER([PayerAddress]) as [PayerAddress] ,[User] ,[RevenueCode] , tblCollectionReport.Description  , [ChequeBankCode] ,[ChequeBankName] ,[AgencyName] ,[AgencyCode] ,[BankCode] ,[BankName] ,[BranchCode] ,[BranchName] ,[ZoneCode] ,[ZoneName] ,[Username] ,[AmountWords] ,[EReceipts] ,[EReceiptsDate] ,[GeneratedBy] ,[StationCode] ,(SELECT TOP 1 StationName FROM Receipt.tblStation  WHERE tblStation.StationCode COLLATE DATABASE_DEFAULT  = Collection.tblCollectionReport.[StationCode]) AS StationName,  Symbol , Surfix , tblCurrency.Description AS prefix  from Collection.tblCollectionReport  JOIN Reconciliation.tblCurrency ON tblCurrency.CurrencyCode = tblCollectionReport.CurrencyCode WHERE PaymentRefNumber COLLATE DATABASE_DEFAULT  IN (SELECT PaymentRefNumber FROM Receipt.tblReceipt where SentBy='{0}')   ORDER BY Collection.tblCollectionReport.AgencyName ,Collection.tblCollectionReport.Description,Collection.tblCollectionReport.EReceipts", Program.UserID);
                                                    break;
                                            }



                                            DataTable Dt = dds.Tables.Add("CollectionReportTable");
                                            ada = new SqlDataAdapter(query, Logic.ConnectionString);
                                            ada.Fill(dds, "CollectionReportTable");
                                            Logic.ProcessDataTable(Dt); ;
                                            //strCollectionReportID = strFormat;
                                        }

                                        //int stCode = Convert.ToInt32(Program.stateCode);

                                        switch (Program.intCode)
                                        {
                                            case 13://Akwa Ibom state
                                                XtraRepReceiptAkwa akwa = new XtraRepReceiptAkwa { DataSource = dds.Tables[0] /*recportRec.DataAdapter = ada;*/, DataMember = "CollectionReportTable" };
                                                akwa.ShowPreviewDialog();
                                                break;

                                            case 20://detla state

                                                XtraRepReceiptDelta delta = new XtraRepReceiptDelta { DataSource = dds.Tables[0] /*recportRec.DataAdapter = ada;*/, DataMember = "CollectionReportTable" };
                                                //XRepReceipto/*yo delta = new XRepReceiptoyo { DataSource = dds.Tables[0] /*recportRec.DataAdapter = ada;*/, DataMember = "CollectionReportTable" };*/
                                                ////delta.PrintingSystem.StartPrint += PrintingSystem_StartPrint;
                                                //delta.Watermark.Text = "DUPLICATE";
                                                ////delta.Watermark.TextDirection = DirectionMode.Clockwise;
                                                //delta.Watermark.Font = new Font(delta.Watermark.Font.FontFamily, 40);
                                                //delta.Watermark.ForeColor = Color.DodgerBlue;
                                                //delta.Watermark.TextTransparency = 150;
                                                //delta.Watermark.ShowBehind = false;
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

                                            //case 32://kogi state

                                            //    XRepReceiptkogi recportRecko = new XRepReceiptkogi { DataSource = dds.Tables[0] /*recportRec.DataAdapter = ada;*/, DataMember = "CollectionReportTable" };
                                            //    recportRecko.ShowPreviewDialog();

                                            //    break;

                                            default:
                                                break;
                                        }
                                        //nb 
                                        ////XRepReceipt recportRec = new XRepReceipt { DataSource = dds.Tables[0] /*recportRec.DataAdapter = ada;*/, DataMember = "CollectionReportTable" };

                                        ////recportRec.ShowPreviewDialog();

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

                                query = string.Format("SELECT [ID] , [Provider] , [Channel] ,[PaymentRefNumber] , [DepositSlipNumber] , CONVERT(VARCHAR,CONVERT(DATE,[PaymentDate])) AS PaymentDate,[PayerID] , UPPER([PayerName]) AS [PayerName], [Amount] ,[PaymentMethod] ,[ChequeNumber] ,[ChequeValueDate] ,[ChequeStatus] ,[DateChequeReturned] ,[TelephoneNumber] ,[ReceiptNo] ,[ReceiptDate] ,UPPER([PayerAddress]) as [PayerAddress] ,[User] ,[RevenueCode] , tblCollectionReport.Description  , [ChequeBankCode] ,[ChequeBankName] ,[AgencyName] ,[AgencyCode] ,[BankCode] ,[BankName] ,[BranchCode] ,[BranchName] ,[ZoneCode] ,[ZoneName] ,[Username] ,[AmountWords] ,[EReceipts] ,[EReceiptsDate] ,[GeneratedBy] ,[StationCode] ,(SELECT TOP 1 StationName FROM Receipt.tblStation  WHERE tblStation.StationCode COLLATE DATABASE_DEFAULT = Collection.tblCollectionReport.[StationCode]) AS StationName, Symbol , Surfix , tblCurrency.Description AS prefix from Collection.tblCollectionReport  INNER JOIN Reconciliation.tblCurrency ON tblCurrency.CurrencyCode = tblCollectionReport.CurrencyCode WHERE PaymentRefNumber COLLATE DATABASE_DEFAULT  IN (SELECT PaymentRefNumber FROM Receipt.tblReceipt where SentBy='{0}')  ORDER BY Collection.tblCollectionReport.AgencyName ,Collection.tblCollectionReport.Description,Collection.tblCollectionReport.EReceipts", Program.UserID);

                                DataTable Dt = dds.Tables.Add("CollectionReportTable");
                                ada = new SqlDataAdapter(query, Logic.ConnectionString);
                                ada.Fill(dds, "CollectionReportTable");
                                Logic.ProcessDataTable(Dt); ;
                                //strCollectionReportID = strFormat;
                            }


                            //XRepReceipt recportRec = new XRepReceipt { DataSource = dds.Tables[0] /*recportRec.DataAdapter = ada;*/, DataMember = "CollectionReportTable" };

                            //recportRec.ShowPreviewDialog();

                            int stCode = Convert.ToInt32(Program.stateCode);

                            switch (stCode)
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

                                case 40://oyo state
                                    XRepReceiptoyo recportRecs = new XRepReceiptoyo { DataSource = dds.Tables[0] /*recportRec.DataAdapter = ada;*/, DataMember = "CollectionReportTable" };
                                    recportRecs.ShowPreviewDialog();
                                    break;

                                case 20://detla state
                                    XtraRepReceiptDelta delta = new XtraRepReceiptDelta { DataSource = dds.Tables[0] /*recportRec.DataAdapter = ada;*/, DataMember = "CollectionReportTable" };
                                    //delta.PrintingSystem.StartPrint += PrintingSystem_StartPrint;
                                    delta.logoPath = Logic.singaturepth;
                                    delta.ShowPreviewDialog();
                                    delta.Print();
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
                }
            }
            finally
            {
                SplashScreenManager.CloseForm(false);
            }

            //return values;
        }

        void PrintingSystem_StartPrint(object sender, DevExpress.XtraPrinting.PrintDocumentEventArgs e)
        {
            //e.PrintDocument.PrinterSettings.Copies = 2;
        }
        public void setDBComboBox()
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

                DataTable Dt;

                using (var ds = new System.Data.DataSet())
                {
                    using (SqlDataAdapter ada = new SqlDataAdapter((string)"SELECT DISTINCT ZoneName  FROM Collection.tblCollectionReport WHERE ZoneName IS NOT NULL AND ZoneName <>'' ORDER BY ZoneName", Logic.ConnectionString))
                    {
                        ada.SelectCommand.CommandTimeout = 0;

                        ada.Fill(ds, "table");
                    }

                    Dt = ds.Tables[0];
                }

                Common.setComboList(cboZone, Dt, "ZoneName", "ZoneName");

                cboZone.SelectedIndex = -1;
            }
            finally
            {
                SplashScreenManager.CloseForm(false);
            }
        }

        public void setDBComboBoxAgency()
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

                DataTable Dt;

                using (var ds = new System.Data.DataSet())
                {
                    using (SqlDataAdapter ada = new SqlDataAdapter((string)"SELECT AgencyCode,AgencyName FROM Registration.tblAgency ORDER BY AgencyName", Logic.ConnectionString))
                    {
                        ada.SelectCommand.CommandTimeout = 0;

                        ada.Fill(ds, "table");
                    }

                    Dt = ds.Tables[0];
                }

                Common.setComboList(cboAgency, Dt, "AgencyCode", "AgencyName");

                Common.setCheckEdit(cboAgencyTest, Dt, "AgencyCode", "AgencyName");

                cboAgency.SelectedIndex = -1;
            }
            finally
            {
                SplashScreenManager.CloseForm(false);
            }

        }

        void setDBComboBoxReveneu()
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

                DataTable Dt;

                string values = string.Empty;

                object[] lol = cboAgencyTest.EditValue.ToString().Split(',');

                int i = 0;

                foreach (object obj in lol)
                {
                    values += string.Format("'{0}'", obj.ToString().Trim());

                    if (i + 1 < lol.Count())
                        values += ",";
                    ++i;
                }

                using (var ds = new System.Data.DataSet())
                {
                    string query = string.Format("SELECT RevenueCode,Description FROM Collection.tblRevenueType WHERE AgencyCode IN ({0})  ORDER BY Description", values);

                    using (SqlDataAdapter ada = new SqlDataAdapter(query, Logic.ConnectionString))
                    {
                        ada.SelectCommand.CommandTimeout = 0;

                        ada.Fill(ds, "table");
                    }

                    Dt = ds.Tables[0];
                }

                //Common.setComboList(cboRevenue, Dt, "RevenueCode", "Description");

                Common.setCheckEdit(cboRevenueEdt, Dt, "RevenueCode", "Description");

                //cboRevenue.SelectedIndex = -1;
            }
            finally
            {
                SplashScreenManager.CloseForm(false);
            }
        }

        public void setDBComboBoxBank()
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

                DataTable Dt;

                using (var ds = new System.Data.DataSet())
                {
                    using (SqlDataAdapter ada = new SqlDataAdapter("SELECT BankShortCode,BankName FROM Collection.tblBank ORDER BY BankName", Logic.ConnectionString))
                    {
                        ada.SelectCommand.CommandTimeout = 0;

                        ada.Fill(ds, "table");
                    }

                    Dt = ds.Tables[0];
                }

                Common.setComboList(cboBank, Dt, "BankShortCode", "BankName");

                Common.setCheckEdit(cboBankEdt, Dt, "BankShortCode", "BankName");

                cboBank.SelectedIndex = -1;
            }
            finally
            {
                SplashScreenManager.CloseForm(false);
            }

        }

        public void setDBComboBoxBranch()
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

                DataTable Dt;

                string values = string.Empty;

                object[] lol = cboBankEdt.EditValue.ToString().Split(',');

                int i = 0;

                foreach (object obj in lol)
                {
                    values += string.Format("'{0}'", obj.ToString().Trim());

                    if (i + 1 < lol.Count())
                        values += ",";
                    ++i;
                }

                using (var ds = new System.Data.DataSet())
                {
                    string query = string.Format("SELECT BranchCode, BankName +','+BranchName AS BranchName FROM Collection.tblBankBranch INNER JOIN Collection.tblBank ON Collection.tblBank.BankShortCode = Collection.tblBankBranch.BankShortCode WHERE Collection.tblBank.BankShortCode in ({0}) ORDER BY BranchName", values);

                    using (SqlDataAdapter ada = new SqlDataAdapter(query, Logic.ConnectionString))
                    {
                        ada.SelectCommand.CommandTimeout = 0;

                        ada.Fill(ds, "table");
                    }

                    Dt = ds.Tables[0];
                }

                Common.setComboList(cboBranch, Dt, "BranchCode", "BranchName");

                Common.setCheckEdit(cboBranchesEdt, Dt, "BranchCode", "BranchName");

                cboBranch.SelectedIndex = -1;
            }
            finally
            {
                SplashScreenManager.CloseForm(false);
            }

        }

        public void setDBComboProfile()
        {
            //cboProfile
            DataTable Dt;

            using (var ds = new System.Data.DataSet())
            {
                using (SqlDataAdapter ada = new SqlDataAdapter((string)"SELECT ProfileID,Name FROM Receipt.tblProfile ORDER BY Name", Logic.ConnectionString))
                {
                    ada.SelectCommand.CommandTimeout = 0;

                    ada.Fill(ds, "table");
                }

                Dt = ds.Tables[0];
            }

            Common.setComboList(cboProfile, Dt, "ProfileID", "Name");

            //Common.setCheckEdit(cboAgencyTest, Dt, "AgencyCode", "AgencyName");

            cboProfile.SelectedIndex = -1;
        }

        //void setDBComoboxCurrency()
        //{
        //    DataTable dts;

        //    using (var ds = new System.Data.DataSet())
        //    {
        //        using (SqlDataAdapter ada = new SqlDataAdapter((string)"SELECT  Description , CurrencyCode FROM Reconciliation.tblCurrency", Logic.ConnectionString))
        //        {
        //            ada.SelectCommand.CommandTimeout = 0;

        //            ada.Fill(ds, "table");
        //        }

        //        dts = ds.Tables[0];
        //    }
        //    Common.setComboList(cboCurrency, dts, "CurrencyCode", "Description");

        //    cboCurrency.SelectedIndex = -1;
        //}
        private string GetUniqueKey()
        {
            int maxSize = 8;
            int minSize = 5;
            char[] chars = new char[62];
            string a;
            a = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            chars = a.ToCharArray();
            int size = maxSize;
            byte[] data = new byte[1];
            RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider();
            crypto.GetNonZeroBytes(data);
            size = maxSize;
            data = new byte[size];
            crypto.GetNonZeroBytes(data);
            StringBuilder result = new StringBuilder(size);
            foreach (byte b in data)
            {
                result.Append(chars[b % (chars.Length - 1)]);
            }
            return result.ToString();
        }

        void EmptyBankReceipts()
        {
            using (SqlConnection db = new SqlConnection(Logic.ConnectionString))
            {
                SqlTransaction transaction;

                db.Open();

                transaction = db.BeginTransaction();

                try
                {
                    using (SqlCommand sqlCommand1 = new SqlCommand("delete from BankReceipts", db, transaction))
                    {
                        sqlCommand1.CommandTimeout = 0;

                        sqlCommand1.ExecuteNonQuery();
                    }

                    transaction.Commit();
                }
                catch (SqlException sqlError)
                {
                    transaction.Rollback();
                }
                db.Close();

            }

        }

        public DataSet.DataSet1 GetFilteredTypedDataSets(DataSet.DataSet1 dsp, string filter)
        {
            //filter the rows from a copy of the authors table
            DataRow[] foundRows = dsp.tblCollectionReport.Copy().Select(filter);


            //delete the authors from the typed dataset
            dsp.tblCollectionReport.Clear();


            //merge the filtered rows back to the typed dataset
            dsp.Merge(foundRows, false, MissingSchemaAction.Add);


            return dsp;
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
                                item["URL"] = string.Format("Paid at {0} , {1}  Deposit Slip Number {2}  in respect of {3} <{4}> By {5}", item["BankName"], item["BranchName"], item["DepositSlipNumber"], item["Description"], item["RevenueCode"], item["PaymentMethod"]);
                                break;

                            case 40://oyo state
                                item["URL"] = string.Format("Paid at {0} , {1}  Deposit Slip Number {2}  in respect of {3} <{4}> By {5}", item["BankName"], item["BranchName"], item["DepositSlipNumber"], item["Description"], item["RevenueCode"], item["PaymentMethod"]);
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

        public void RefreshForm()
        {
            NavBars.ToolStripEnableDisableControls(toolStrip, Tag as String);
        }

        void CountMain()
        {
            try
            {
                //using (var ds = new System.Data.DataSet())
                //{

                using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
                {
                    connect.Open();
                    _command = new SqlCommand("SearchReceipt", connect) { CommandType = CommandType.StoredProcedure };
                    _command.Parameters.Add(new SqlParameter("@Userid", SqlDbType.VarChar)).Value = Program.UserID;
                    _command.CommandTimeout = 0;

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
                        if (response.Tables[1] != null && response.Tables[1].Rows.Count > 0)
                        {
                            dts = response.Tables[1];

                            if (dts.Rows.Count > 0)
                            {
                                //gridControl1.DataSource = dt.DefaultView;
                                label7.Text = String.Format(" {0} Printed receipts Waiting for Control Number to be applied ", dts.Rows.Count);

                                //DialogResult result = MessageBox.Show(string.Format("The Last job comprising {0} records is not yet finished. Click Yes and select the remaining pages to complete printing Or No to apply Control Number, if all have printed successfully ", dts.Rows.Count), Program.ApplicationName, MessageBoxButtons.YesNoCancel);

                                DialogResult result = MessageBox.Show(string.Format("You have {0} record(s) yet to be completed. Click Yes, to Reprint to print them again if they were not printed successfully Or Click No, to apply Control Number, which means you already printed these records successfully ", dts.Rows.Count), "Unfinished Job", MessageBoxButtons.YesNoCancel);

                                if (result == DialogResult.Yes)
                                {
                                    //isReprint = true; GetPayRef();
                                    //return;
                                    using (FrmDisplay display = new FrmDisplay() { FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog })
                                    {
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
                            else
                            {
                                gridView1.Columns.Clear();
                            }
                        }
                        else
                        {
                            return;

                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Common.setMessageBox(ex.Message, Program.ApplicationName, 3);
                return;
            }
        }

        //void getReceipt()
        //{
        //    DataTable dtget = new DataTable();

        //    string qry = string.Format("SELECT PaymentRefNumber,ReceiptNo FROM tblReceipt WHERE Isprinted=1 AND LEN(ControlNumber)=0 AND PrintedBY='{0}'", Program.UserID);

        //    try
        //    {
        //        using (var ds = new System.Data.DataSet())
        //        {
        //            ada = new SqlDataAdapter(qry, Logic.ConnectionString);

        //            ada.SelectCommand.CommandTimeout = 0;

        //            ada.Fill(ds, "table");

        //            dtget = ds.Tables[0];
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Common.setMessageBox(ex.Message, Program.ApplicationName, 3);
        //        return;
        //    }

        //    if (dtget.Rows.Count > 0)
        //    {
        //        DialogResult result = MessageBox.Show("Some Receipt Printing Transaction Not Completed,Do you wish to send for Reprint request ?", Program.ApplicationName, MessageBoxButtons.YesNo);

        //        if (result == DialogResult.Yes)
        //        {
        //            using (FrmPending frmpending = new FrmPending(true))
        //            {
        //                frmpending.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
        //                frmpending.ShowDialog();
        //            }

        //            return;
        //        }

        //    }
        //}
    }

    public class Payment
    {
        public virtual string PaymentRefNumber { get; set; }
    }
}
