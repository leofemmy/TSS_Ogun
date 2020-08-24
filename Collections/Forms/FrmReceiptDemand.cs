using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using TaxSmartSuite;
using Collection.Classess;
using TaxSmartSuite.Class;
using Collection.Report;
using DevExpress.Utils;
using Microsoft.VisualBasic;
using System.Text.RegularExpressions;
using Collection.Classess;
using WindowsApplication1;
using DevExpress.XtraReports.UI;
using DevExpress.XtraReports.Parameters;


namespace Collection.Forms
{
    public partial class FrmReceiptDemand : Form
    {
        private DataTable dt;

        public static FrmReceiptDemand publicStreetGroup;

        string query, criteria;

        int j = 0;

        AmountToWords amounttowords = new AmountToWords();

        public FrmReceiptDemand()
        {
            InitializeComponent();

            ToolStripEvent();

            publicStreetGroup = this;

            setImages();

            setDBComboBox();

            btnSearch.Click += btnSearch_Click;

            btnPrint.Click += btnPrint_Click;
        }

        void btnPrint_Click(object sender, EventArgs e)
        {


            using (System.Data.DataSet dds = new System.Data.DataSet("DsCollectionReport"))
            {
                SqlDataAdapter ada;

                using (WaitDialogForm form = new WaitDialogForm("Please Wait...", "Initialising Receipt Printing"))
                {
                    //string strFormat = null;
                    query = String.Format("SELECT [Provider] , [Channel] ,[PaymentRefNumber] , [DepositSlipNumber] , CONVERT(VARCHAR,CONVERT(DATEtime,[PaymentDate]),103) AS PaymentDate,[PayerID] , UPPER([PayerName]) AS [PayerName], [Amount] ,[PaymentMethod] ,[ChequeNumber] ,[ChequeValueDate] ,[ChequeStatus] ,[DateChequeReturned] ,[TelephoneNumber] ,[ReceiptNo] ,[ReceiptDate] ,UPPER([PayerAddress]) as [PayerAddress] ,[Status] ,[User] ,[RevenueCode] ,tblCollectionReport.Description , [ChequeBankCode] ,[ChequeBankName] ,[AgencyName] ,[AgencyCode] ,[BankCode] ,[BankName] ,[BranchCode] ,[BranchName] ,[ZoneCode] ,[ZoneName] ,[Username] ,[State] ,[AmountWords] ,[URL] ,[EReceipts] ,[EReceiptsDate] ,[GeneratedBy] ,[DateValidatedAgainst] ,[DateDiff] ,[UploadStatus] ,[PrintedBY] ,[DatePrinted] ,[ControlNumber] ,[BatchNumber] ,[StationCode] ,(Select StationName from tblStation2 WHERE tblStation2.StationCode=tblCollectionReport.[StationCode]) AS StationName, Symbol , Surfix , tblCurrency.Description AS prefix from tblCollectionReport  INNER JOIN Reconciliation.tblCurrency ON tblCurrency.CurrencyCode = tblCollectionReport.CurrencyCode WHERE RevenueCode IN (SELECT RevenueCode FROM [tblPrintingRevenueCode]) AND PaymentRefNumber Like '%{0}%' ORDER BY tblCollectionReport.StationCode , tblCollectionReport.AgencyCode ,tblCollectionReport.RevenueCode,tblCollectionReport.EReceipts ", criteria);


                    DataTable Dt = dds.Tables.Add("CollectionReportTable");
                    ada = new SqlDataAdapter(query, Logic.ConnectionString);
                    ada.Fill(dds, "CollectionReportTable");
                    Logic.ProcessDataTable(Dt); ;
                    //strCollectionReportID = strFormat;
                }
                using (XRepReceipt recportRec = new XRepReceipt { DataSource = dds /*recportRec.DataAdapter = ada;*/, DataMember = "CollectionReportTable" })
                {
                    j = j + 1;
                    //open report in dialog view
                    recportRec.xrLabel43.Text = String.Format(" {0} Printing ", j);
                    //recportRec.ShowPreviewDialog();
                    recportRec.logoPath = Logic.singaturepth;
                    //send the receipt to the printer directly
                    recportRec.Print();

                }

                //update the collection table by seting the isprinted to true
                using (SqlConnection db = new SqlConnection(Logic.ConnectionString))
                {
                    SqlTransaction transaction;

                    db.Open();

                    transaction = db.BeginTransaction();

                    try
                    {
                        string query1 = String.Format("UPDATE tblCollectionReport SET isPrinted=1 WHERE [PaymentRefNumber]= '{0}'", criteria);

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

            //ask if the print was sucessfull
            DialogResult result = MessageBox.Show(" Receipt Printing Successful ?", " Print Receipts On Demand ", MessageBoxButtons.YesNo);

            if (result == DialogResult.Yes)
            {

            Restart:
                InputBoxResult value = InputBox.Show(" Enter Receipt Control Number ", "Print Receipts On Demand", "Default", 100, 50);

                //check the return value
                if (value.ReturnCode == DialogResult.OK)
                {
                    //check if the value is null
                    if (string.IsNullOrEmpty(value.Text))
                    {
                        Common.setEmptyField(" Control Number ", Program.ApplicationName);
                        goto Restart;
                    }
                    else if (!Logic.IsNumber((value.Text)))//check if the return value contain string
                    {
                        Common.setMessageBox(" Control Number can only be in number values (e.g 0124556) ", Program.ApplicationName, 3);
                        goto Restart;
                    }
                    else if (Logic.CheckRnageValue(value.Text))
                    {
                        Common.setMessageBox(" Control Number Range already Exit ", Program.ApplicationName, 2);
                        goto Restart;
                    }
                    else if (!Logic.CheckRangeValue4mTable((string)value.Text))
                    {
                        Common.setMessageBox(" Control Number Not in The Issue Range ", Program.ApplicationName, 2);
                        goto Restart;
                    }
                    else
                    {
                        //    ////////update the collection table by seting the isprinted to true
                        using (SqlConnection db = new SqlConnection(Logic.ConnectionString))
                        {
                            SqlTransaction transaction;

                            db.Open();

                            transaction = db.BeginTransaction();

                            try
                            {
                                string query1 = String.Format("UPDATE tblCollectionReport SET PrintedBY= '{0}',DatePrinted= '{1}',ControlNumber= '{2}',[UploadStatus]='Pending' WHERE [PaymentRefNumber]= '{3}'", Program.UserID, DateTime.Now.Date.ToString("yyyy-MM-dd hh:mm:ss"), value.Text, criteria);

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
                }

                Common.setMessageBox("Receipt Printing Successfull", Program.ApplicationName, 1);

            }
            else
            {
                ////go back to the search method
                btnSearch.Enabled = false;
                cboBank.Enabled = false;
                txtDate.Enabled = false;
                txtPayment.Enabled = false;

            }

        }

        void btnSearch_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(txtDate.Text))
            {
                Common.setEmptyField("Payment Date", Program.ApplicationName);
                txtDate2.Focus(); return;
            }
            else if (string.IsNullOrEmpty(txtDate.Text))
            {
                Common.setEmptyField("Payment Ref. Number", Program.ApplicationName);
                txtPayment.Focus(); return;
            }
            else
            {
                criteria = String.Format("{0}|OYPD|{1}|{2}", cboBank.SelectedValue, txtDate.Text.Trim(), txtPayment.Text.Trim());

                //MessageBox.Show(criteria);

                query = String.Format("SELECT PaymentRefNumber,DepositSlipNumber,PaymentDate,UPPER(PayerName) as PayerName,Description,Amount,BankName+ '-'+ BranchName AS Bank,EReceipts from tblCollectionReport WHERE PaymentRefNumber Like '%{0}%' AND (isPrinted=0 OR isPrinted IS NULL) AND EReceipts IS NOT NULL AND ControlNumber IS NULL ", criteria);

                using (var ds = new System.Data.DataSet())
                {
                    using (SqlDataAdapter ada = new SqlDataAdapter(query, Logic.ConnectionString))
                    {
                        ada.Fill(ds, "table");
                    }
                    dt = ds.Tables[0];

                    gridControl1.DataSource = dt;

                }

                if (dt.Rows.Count > 0)
                {
                    label6.Visible = true;
                    label6.Text = String.Format("This Payment Ref. Number  {0}  is ready for printing", criteria);
                    btnSearch.Enabled = false;
                    btnPrint.Enabled = true;
                }
                else
                {
                    label6.Visible = true;
                    label6.Text = " Record Not Found";
                    btnSearch.Enabled = true;
                    btnPrint.Enabled = false;
                }
            }
        }

        void setDBComboBox()
        {

            DataTable Dt;

            using (var ds = new System.Data.DataSet())
            {
                string query = "SELECT BankShortCode,BankName FROM tblBank ORDER BY BankShortCode";
                using (SqlDataAdapter ada = new SqlDataAdapter(query, Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }

                Dt = ds.Tables[0];
            }

            Common.setComboList(cboBank, Dt, "BankShortCode", "BankName");

            //cboBank.SelectedIndex = -1;

        }

        private void setImages()
        {
            tsbNew.Image = MDIMain.publicMDIParent.i16x16.Images[5];
            tsbEdit.Image = MDIMain.publicMDIParent.i16x16.Images[6];
            tsbDelete.Image = MDIMain.publicMDIParent.i16x16.Images[7];
            tsbReload.Image = MDIMain.publicMDIParent.i16x16.Images[0];
            tsbClose.Image = MDIMain.publicMDIParent.i16x16.Images[11];

            //bttnClose.Image = MDIMain.publicMDIParent.i16x16.Images[11];
            //bttnBrowse.Image = MDIMain.publicMDIParent.i32x32.Images[7];
            btnPrint.Image = MDIMain.publicMDIParent.i32x32.Images[29];
            btnSearch.Image = MDIMain.publicMDIParent.i32x32.Images[2];
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
                //MDIMain.publicMDIParent.RemoveControls();
                Close();
            }

        }

        void ProcessDataTable(DataTable Dt)
        {
            if (Dt != null && Dt.Rows.Count > 0)
            {
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
                            item["PayerID"] = "Please approach the BIR for your unique Payer ID.";
                        }
                        else
                        {
                            item["PayerID"] = item["PayerID"].ToString();
                        }
                        item["Description"] = item["Description"].ToString();
                        item["URL"] = string.Format(@"Payment for {0} {1} << Paid at {2} - {3} , Deposit Slip Number {4} by {5}  >> ", item["Description"], item["RevenueCode"], item["BankName"], item["BranchName"], item["DepositSlipNumber"], item["PaymentMethod"]);
                        item["User"] = Program.UserID.ToUpper();
                        item["Username"] = string.Format(@"</Printed at {0} Zonal Office  by {1} on {2}/>", item["StationName"], Program.UserID, DateTime.Today.ToString("dd-MMM-yyyy"));
                        item["PaymentDate"] = Convert.ToDateTime(item["PaymentDate"]).ToString("dd-MMM-yyyy");
                    }
                    catch
                    {

                    }
                }
            }
        }

        //bool IsNumber(string text)
        //{
        //    Regex regex = new Regex(@"^[-+]?[0-9]*\.?[0-9]+$");
        //    return regex.IsMatch(text);
        //}

        public static bool CheckRnageValue(string ContNum)
        {
            bool bRes;

            string sql = String.Format("SELECT COUNT(*) AS Count FROM tblCollectionReport WHERE (ControlNumber = '{0}')", ContNum);

            if (new Logic().IsRecordExist(sql))
                //if (retval == "1")
                bRes = true;
            else
                bRes = false;
            return bRes;
        }

        //Enumerable
    }
}
