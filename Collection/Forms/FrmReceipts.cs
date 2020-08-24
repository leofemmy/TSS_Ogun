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
using DevExpress.XtraReports.UI;
using DevExpress.XtraReports.Parameters;

namespace Collection.Forms
{
    public partial class FrmReceipts : Form
    {
        private DataTable Dt;
        private string PrintType;
        public static FrmReceipts publicInstance;

        int iCount = 0;

        GridCheckMarksSelection selection;

        AmountToWords amounttowords = new AmountToWords();

        string query,payerid,amount;

        bool isFirstGrid = true;

        private bool isFirst = true;

        SqlDataAdapter adp;

        string Url;

        //System.Data.DataSet ds = new System.Data.DataSet();
        DataTable dt = new DataTable();
                
        public static FrmReceipts publicStreetGroup;

        protected TransactionTypeCode iTransType;

        protected bool boolIsUpdate;

        private string user;

        public FrmReceipts()
        {
            InitializeComponent();

            publicInstance = this;

            publicStreetGroup = this;

            setImages();

            ToolStripEvent();

            iTransType = TransactionTypeCode.New;

            Load += OnFormLoad;

            OnFormLoad(null, null);

            //cboZone.Items.Add("All Zones");

            if (Program.UserID == "" || Program.UserID == null)
            {
                user = "Femi";
            }
            else
            {
                user = Program.UserID;
            }

           

        }

        void btnMain_Click(object sender, EventArgs e)
        {
            //MDIMain.publicMDIParent.RemoveControls();
            //MDIMain.publicMDIParent.tableLayoutPanel2.Controls.Add((new FrmMainFest.panelContainer), 1, 0);
            MDIMain.publicMDIParent.tableLayoutPanel2.Controls.Add((new FrmMainFest().panelContainer), 1, 0);
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
            }
            else if (sender == tsbNew)
            {
                //label11.Visible = false;

                //txtPaymentRef.Visible = false;

                //groupControl2.Text = "Add New Record";

                //iTransType = TransactionTypeCode.New;

                //ShowForm();

                //clear();

                //groupControl2.Enabled = true;

                boolIsUpdate = false;
            }
            else if (sender == tsbEdit)
            {
                //groupControl2.Text = "Edit Record Mode";

                iTransType = TransactionTypeCode.Edit;

                //ShowForm();

                boolIsUpdate = true;

            }
            //else if (sender == tsbDelete)
            //{
            //    groupControl2.Text = "Delete Record Mode";
            //    iTransType = TransactionTypeCode.Delete;
            //    if (MosesClassLibrary.Utilities.Common.AskQuestion("Deleting this record will delete attached record.\nDo you want to continue?", ""))
            //    {
            //    }
            //    else
            //        tsbReload.PerformClick();
            //    boolIsUpdate = false;
            //}
            //else if (sender == tsbReload)
            //{
            //    iTransType = TransactionTypeCode.Reload;
            //    ShowForm();
            //}
            //bttnReset.PerformClick();
        }

        void OnFormLoad(object sender, EventArgs e)
        {
            //ShowForm();

            setDBComboBox();

            setDBComboBoxAgency();

            setDBComboBoxBank();

            setDBComboBoxBranch();

            btnSearch.Click += btnSearch_Click;

            btnPrint.Click += btnPrint_Click;

            btnMain.Click += btnMain_Click;

             isFirst = false;

            //generate number
            string test = String.Format("{0:d9}", (DateTime.Now.Ticks / 10) % 1000000000);

            string number = GetUniqueKey();

            //isSecond = false;

            //isThird = false;

            //isFourth = false;

            //populate year box
            //Methods.PopulateYear(cboYear);

            //populate months
            //Methods.PopulateMonth(cboMonth);

            //get default currency

            //string query = String.Format("select CurrencyName  from tblCurrency where Flag =1");

            //DataTable dts = (new Logic()).getSqlStatement(query).Tables[0];

            //if (dts != null)
            //{
            //    txtCurrency.Text = dts.Rows[0]["CurrencyName"].ToString();
            //}


        }

        void btnPrint_Click(object sender, EventArgs e)
        {
            PrintType = "Original"; 

            if (Program.stateCode == "20")
            {
                Url = "www.deltabir.com";
            }
            else if (Program.stateCode == "13")
            { Url = "www.Akwaibomrevenue.com"; }
            else if (Program.stateCode == "37")
            { Url = "www.ogunbir.com"; }
            else if (Program.stateCode == "40")
            {
                Url = "www.oyobir.com"; 
            }
            if (selection.SelectedCount == 0)
            {
                Common.setMessageBox("Please Check Records to Print", Program.ApplicationName, 1);
                return;
            }
            else
            {
                EmptyBankReceipts();

                using (SqlConnection db = new SqlConnection(Logic.ConnectionString))
                {
                    SqlTransaction transaction;

                    db.Open();

                    transaction = db.BeginTransaction();

                    try 
                    {

                        for (int i = 0; i < selection.SelectedCount; i++)
                        {
                            using (var ds = new System.Data.DataSet())
                            {//select record

                                string lol = ((selection.GetSelectedRow(i) as DataRowView)["PaymentRefNumber"].ToString());

                                query = String.Format("SELECT [PaymentRefNumber],[DepositSlipNumber],[PaymentDate],[PayerID],[PayerName] ,[RevenueCode] ,[Description] ,[Amount] ,[PaymentMethod] ,[AgencyName] ,	[BankName] ,[BranchName] ,[EReceipts] ,[PayerAddress],StationName,ZoneName FROM dbo.tblCollectionReport WHERE ([PaymentDate] >= '{0}' AND [PaymentDate] <= '{1}') and EReceipts IS NOT NULL AND [PaymentRefNumber]= '{2}'", dtpfrom.Value.Date.ToString("yyyy-MM-dd"), dtpTo.Value.Date.ToString("yyyy-MM-dd"), lol);

                                using (SqlDataAdapter ada = new SqlDataAdapter(query, Logic.ConnectionString))
                                {
                                    ada.Fill(ds, "table");
                                }

                                Dt = ds.Tables[0];

                            }

                            //insert record into bankreceipt table

                            foreach (DataRow item in Dt.Rows)
                            {
                                if (item["PayerID"].ToString().Length > 14)
                                {
                                    payerid = "Please approach the BIR for your unique Payer ID.";
                                }
                                else
                                {
                                    payerid = item["PayerID"].ToString();
                                }

                                amount = amounttowords.convertToWords(item["Amount"].ToString());
                                //if (item.Rows[0]["PlateCode"].length)
                                //payerid
                                //amount
                                //try
                                //{
                                    string desc = Regex.Replace(item["Description"].ToString(), @"[']", "");

                                    string query2 = String.Format("INSERT INTO [BankReceipts]([PaymenRefNumber],[DepositSlipNumber],[PaymentDate],[PayerID],[PayerName] ,[RevenueCode] ,[Description] ,[Amount] ,[PaymentMethod] ,[AgencyName] ,[BankName] ,[BranchName] ,[ReceiptNumber] ,[PayerAddress],[PrintType],[URL],[AmountWords],[Users],StationName,TaxOffice) VALUES ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}', '{14}','{15}','{16}','{17}' ,'{18}','{19}')", item["PaymentRefNumber"].ToString(), item["DepositSlipNumber"].ToString(), item["PaymentDate"].ToString(), payerid, item["PayerName"].ToString(), item["RevenueCode"].ToString(), desc, item["Amount"].ToString(), item["PaymentMethod"].ToString(), item["AgencyName"].ToString(), item["BankName"].ToString(), item["BranchName"].ToString(), item["EReceipts"].ToString(), item["PayerAddress"].ToString(), PrintType, Url, amount, user, item["StationName"].ToString(), item["ZoneName"].ToString());


                                    using (SqlCommand sqlCommand1 = new SqlCommand(query2, db, transaction))
                                    {
                                        sqlCommand1.ExecuteNonQuery();
                                    }
                                   
                                //}
                                //catch (Exception ex)
                                //{
                                //    Common.setMessageBox(ex.StackTrace.ToString(), Program.ApplicationName, 3);
                                //    return;
                                //}
                            }

                        }
                        transaction.Commit();

                        //call report for Print

                        ReceiptCall();

                        btnMain.Enabled = true;
                        btnPrint.Enabled = false;
                    }
                    catch (SqlException sqlError)
                    {
                        transaction.Rollback();
                    }
                    db.Close();

                }

                
            }

            
        }

        void ReceiptCall()
        {
            XRepReceipt recportRec = new XRepReceipt();

            recportRec.ShowPreviewDialog();
          
        }

        void btnSearch_Click(object sender, EventArgs e)
        {
            //ds = new System.Data.DataSet();

            adp = new SqlDataAdapter();

            if (cboAgency.SelectedIndex == -1 && cboBank.SelectedIndex == -1 && cboBranch.SelectedIndex == -1 && cboZone.SelectedIndex == -1)//All criteria
            {
                query = String.Format("SELECT PaymentRefNumber,DepositSlipNumber,PaymentDate,PayerName,Description,Amount,BankName+ '-'+ BranchName AS Bank,EReceipts from tblCollectionReport  WHERE ([PaymentDate] >= '{0}' AND [PaymentDate] <= '{1}') and EReceipts IS NOT NULL AND Amount >0 ", dtpfrom.Value.Date.ToString("yyyy-MM-dd"), dtpTo.Value.Date.ToString("yyyy-MM-dd"));
            }
            else if (cboAgency.SelectedIndex >= 1 && cboBank.SelectedIndex == -1 && cboBranch.SelectedIndex == -1 && cboZone.SelectedIndex == -1)//Agency Alone
            {
                query = String.Format("SELECT PaymentRefNumber,DepositSlipNumber,PaymentDate,PayerName,Description,Amount,BankName+ '-'+ BranchName AS Bank,EReceipts from tblCollectionReport  WHERE ([PaymentDate] >= '{0}' AND [PaymentDate] <= '{1}') and EReceipts IS NOT NULL AND AgencyCode= '{2}' AND Amount >0", dtpfrom.Value.Date.ToString("yyyy-MM-dd"), dtpTo.Value.Date.ToString("yyyy-MM-dd"), cboAgency.SelectedValue);
            }
            else if (cboAgency.SelectedIndex == -1 && cboBank.SelectedIndex >= 1 && cboBranch.SelectedIndex == -1 && cboZone.SelectedIndex == -1)//Bank Alone
            {
                query = String.Format("SELECT PaymentRefNumber,DepositSlipNumber,PaymentDate,PayerName,Description,Amount,BankName+ '-'+ BranchName AS Bank,EReceipts from tblCollectionReport  WHERE ([PaymentDate] >= '{0}' AND [PaymentDate] <= '{1}') and EReceipts IS NOT NULL AND BankCode= '{2}' AND Amount >0", dtpfrom.Value.Date.ToString("yyyy-MM-dd"), dtpTo.Value.Date.ToString("yyyy-MM-dd"), cboBank.SelectedValue);
            }
            else if (cboAgency.SelectedIndex == -1 && cboBank.SelectedIndex == -1 && cboBranch.SelectedIndex >= 1 && cboZone.SelectedIndex == -1)//All Bank Branch
            {
                query = String.Format("SELECT PaymentRefNumber,DepositSlipNumber,PaymentDate,PayerName,Description,Amount,BankName+ '-'+ BranchName AS Bank,EReceipts from tblCollectionReport  WHERE ([PaymentDate] >= '{0}' AND [PaymentDate] <= '{1}') and EReceipts IS NOT NULL AND BranchCode= '{2}' AND Amount >0", dtpfrom.Value.Date.ToString("yyyy-MM-dd"), dtpTo.Value.Date.ToString("yyyy-MM-dd"), cboBranch.SelectedValue);
            }
            else if (cboAgency.SelectedIndex == -1 && cboBank.SelectedIndex == -1 && cboBranch.SelectedIndex == -1 && cboZone.SelectedIndex >= 1) //All Zone
            {
                query = String.Format("SELECT PaymentRefNumber,DepositSlipNumber,PaymentDate,PayerName,Description,Amount,BankName+ '-'+ BranchName AS Bank,EReceipts from tblCollectionReport  WHERE ([PaymentDate] >= '{0}' AND [PaymentDate] <= '{1}') and EReceipts IS NOT NULL AND ZoneCode= '{2}' AND Amount >0 ", dtpfrom.Value.Date.ToString("yyyy-MM-dd"), dtpTo.Value.Date.ToString("yyyy-MM-dd"), cboZone.SelectedValue);
            }
            else if (cboAgency.SelectedIndex >= 1 && cboBank.SelectedIndex >= 1 && cboBranch.SelectedIndex == -1 && cboZone.SelectedIndex == -1)//Agency and Bank
            {
                query = String.Format("SELECT PaymentRefNumber,DepositSlipNumber,PaymentDate,PayerName,Description,Amount,BankName+ '-'+ BranchName AS Bank,EReceipts from tblCollectionReport  WHERE ([PaymentDate] >= '{0}' AND [PaymentDate] <= '{1}') and EReceipts IS NOT NULL AND AgencyCode= '{2}' AND BankCode= '{3}' AND Amount >0 ", dtpfrom.Value.Date.ToString("yyyy-MM-dd"), dtpTo.Value.Date.ToString("yyyy-MM-dd"), cboAgency.SelectedValue, cboBank.SelectedValue);
            }
            else if (cboAgency.SelectedIndex == -1 && cboBank.SelectedIndex == -1 && cboBranch.SelectedIndex >= 1 && cboZone.SelectedIndex >= 1) //bank branch and zone
            {
                query = String.Format("SELECT PaymentRefNumber,DepositSlipNumber,PaymentDate,PayerName,Description,Amount,BankName+ '-'+ BranchName AS Bank,EReceipts from tblCollectionReport  WHERE ([PaymentDate] >= '{0}' AND [PaymentDate] <= '{1}') and EReceipts IS NOT NULL AND ZoneCode= '{2}' AND BranchCode= '{3}' AND Amount >0 ", dtpfrom.Value.Date.ToString("yyyy-MM-dd"), dtpTo.Value.Date.ToString("yyyy-MM-dd"), cboZone.SelectedValue, cboBranch.SelectedValue);
            }
            else if (cboAgency.SelectedIndex >= 1 && cboBank.SelectedIndex == -1 && cboBranch.SelectedIndex >= 1 && cboZone.SelectedIndex == -1)//Agency and Bank branch
            {
                query = String.Format("SELECT PaymentRefNumber,DepositSlipNumber,PaymentDate,PayerName,Description,Amount,BankName+ '-'+ BranchName AS Bank,EReceipts from tblCollectionReport  WHERE ([PaymentDate] >= '{0}' AND [PaymentDate] <= '{1}') and EReceipts IS NOT NULL AND AgencyCode= '{2}' AND BranchCode= '{3}' AND Amount >0", dtpfrom.Value.Date.ToString("yyyy-MM-dd"), dtpTo.Value.Date.ToString("yyyy-MM-dd"), cboAgency.SelectedValue, cboBranch.SelectedValue);
            }
            else if (cboAgency.SelectedIndex == -1 && cboBank.SelectedIndex >= 1 && cboBranch.SelectedIndex == -1 && cboZone.SelectedIndex >= 1)//zone and bank
            {
                query = String.Format("SELECT PaymentRefNumber,DepositSlipNumber,PaymentDate,PayerName,Description,Amount,BankName+ '-'+ BranchName AS Bank,EReceipts from tblCollectionReport  WHERE ([PaymentDate] >= '{0}' AND [PaymentDate] <= '{1}') and EReceipts IS NOT NULL AND ZoneCode= '{2}' AND BankCode= '{3}' AND Amount >0", dtpfrom.Value.Date.ToString("yyyy-MM-dd"), dtpTo.Value.Date.ToString("yyyy-MM-dd"), cboZone.SelectedValue, cboBank.SelectedValue);
            }
            else if (cboAgency.SelectedIndex >= 1 && cboBank.SelectedIndex == -1 && cboBranch.SelectedIndex == -1 && cboZone.SelectedIndex >= 1)//Agency and zone
            {
                query = String.Format("SELECT PaymentRefNumber,DepositSlipNumber,PaymentDate,PayerName,Description,Amount,BankName+ '-'+ BranchName AS Bank,EReceipts from tblCollectionReport  WHERE ([PaymentDate] >= '{0}' AND [PaymentDate] <= '{1}') and EReceipts IS NOT NULL AND AgencyCode= '{2}' AND ZoneCode= '{3}' AND Amount >0", dtpfrom.Value.Date.ToString("yyyy-MM-dd"), dtpTo.Value.Date.ToString("yyyy-MM-dd"), cboAgency.SelectedValue, cboZone.SelectedValue);
            }
            else if (cboAgency.SelectedIndex == -1 && cboBank.SelectedIndex >= 1 && cboBranch.SelectedIndex >= 1 && cboZone.SelectedIndex == -1)//bank and branch
            {
                query = String.Format("SELECT PaymentRefNumber,DepositSlipNumber,PaymentDate,PayerName,Description,Amount,BankName+ '-'+ BranchName AS Bank,EReceipts from tblCollectionReport  WHERE ([PaymentDate] >= '{0}' AND [PaymentDate] <= '{1}') and EReceipts IS NOT NULL AND BranchCode= '{2}' AND BankCode= '{3}' AND Amount >0", dtpfrom.Value.Date.ToString("yyyy-MM-dd"), dtpTo.Value.Date.ToString("yyyy-MM-dd"), cboBranch.SelectedValue, cboBank.SelectedValue);
            }
            else if (cboAgency.SelectedIndex >= 1 && cboBank.SelectedIndex >= 1 && cboBranch.SelectedIndex >= 1 && cboZone.SelectedIndex == -1)//agency,bank and bankbranch
            {
                query = String.Format("SELECT PaymentRefNumber,DepositSlipNumber,PaymentDate,PayerName,Description,Amount,BankName+ '-'+ BranchName AS Bank,EReceipts from tblCollectionReport  WHERE ([PaymentDate] >= '{0}' AND [PaymentDate] <= '{1}') and EReceipts IS NOT NULL AND BranchCode= '{2}' AND BankCode= '{3}' AND AgencyCode= '{4}' AND Amount >0", dtpfrom.Value.Date.ToString("yyyy-MM-dd"), dtpTo.Value.Date.ToString("yyyy-MM-dd"), cboBranch.SelectedValue, cboBank.SelectedValue, cboAgency.SelectedValue);
            }
            else if (cboAgency.SelectedIndex >= 1 && cboBank.SelectedIndex >= 1 && cboBranch.SelectedIndex == -1 && cboZone.SelectedIndex >= 1)//agency,zone,bank
            {
                query = String.Format("SELECT PaymentRefNumber,DepositSlipNumber,PaymentDate,PayerName,Description,Amount,BankName+ '-'+ BranchName AS Bank,EReceipts from tblCollectionReport  WHERE ([PaymentDate] >= '{0}' AND [PaymentDate] <= '{1}') and EReceipts IS NOT NULL AND ZoneCode= '{2}' AND BankCode= '{3}' AND AgencyCode= '{4}' AND Amount >0", dtpfrom.Value.Date.ToString("yyyy-MM-dd"), dtpTo.Value.Date.ToString("yyyy-MM-dd"), cboZone.SelectedValue, cboBank.SelectedValue, cboAgency.SelectedValue);
            }
            else if (cboAgency.SelectedIndex == -1 && cboBank.SelectedIndex >= 1 && cboBranch.SelectedIndex >= 1 && cboZone.SelectedIndex >= 1)//zone,bank, and bank branch
            {
                query = String.Format("SELECT PaymentRefNumber,DepositSlipNumber,PaymentDate,PayerName,Description,Amount,BankName+ '-'+ BranchName AS Bank,EReceipts from tblCollectionReport  WHERE ([PaymentDate] >= '{0}' AND [PaymentDate] <= '{1}') and EReceipts IS NOT NULL AND ZoneCode= '{2}' AND BankCode= '{3}' AND BranchCode= '{4}' AND Amount >0 ", dtpfrom.Value.Date.ToString("yyyy-MM-dd"), dtpTo.Value.Date.ToString("yyyy-MM-dd"), cboZone.SelectedValue, cboBank.SelectedValue, cboBranch.SelectedValue);
            }
            else if (cboAgency.SelectedIndex >= 1 && cboBank.SelectedIndex >= 1 && cboBranch.SelectedIndex >= 1 && cboZone.SelectedIndex >= 1)//all the 4 condition
            {
                query = String.Format("SELECT PaymentRefNumber,DepositSlipNumber,PaymentDate,PayerName,Description,Amount,BankName+ '-'+ BranchName AS Bank,EReceipts from tblCollectionReport  WHERE ([PaymentDate] >= '{0}' AND [PaymentDate] <= '{1}') and EReceipts IS NOT NULL AND ZoneCode= '{2}' AND BankCode= '{3}' AND BranchCode= '{4}' AND AgencyCode= '{5}' AND Amount >0 ", dtpfrom.Value.Date.ToString("yyyy-MM-dd"), dtpTo.Value.Date.ToString("yyyy-MM-dd"), cboZone.SelectedValue, cboBank.SelectedValue, cboBranch.SelectedValue, cboAgency.SelectedValue);
            }
            
           
                try
                {
                    using (var ds = new System.Data.DataSet())
                {
                    using (SqlDataAdapter ada = new SqlDataAdapter(query, Logic.ConnectionString))
                    {
                        ada.Fill(ds, "table");
                    }


                    dt = ds.Tables[0];
                    gridControl4.DataSource = dt.DefaultView;
                    gridView4.BestFitColumns();

                }

                    label10.Text = String.Format("Total Number ot Payments: {0}", dt.Rows.Count);

                if (isFirstGrid)
                {
                    selection = new GridCheckMarksSelection(gridView4);
                    selection.CheckMarkColumn.VisibleIndex = 0;
                    isFirstGrid = false;
                }
                }
                catch (Exception ex)
                {
                 Common.setMessageBox(ex.Message,Program.ApplicationName,3);
                    return;
                }
            
        }

        public void setDBComboBox()
        {
            DataTable Dt;



            using (var ds = new System.Data.DataSet())
            {


                string query = "SELECT DISTINCT ZoneCode,ZoneName FROM tblCollectionReport ORDER BY ZoneCode";

                using (SqlDataAdapter ada = new SqlDataAdapter(query, Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }

                Dt = ds.Tables[0];
            }

            Common.setComboList(cboZone, Dt, "ZoneCode", "ZoneName");


            cboZone.SelectedIndex = -1;


        }

        private void FrmReceipts_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'reportDs.tblCollectionReport' table. You can move, or remove it, as needed.
            //this.tblCollectionReportTableAdapter.Fill(this.reportDs.tblCollectionReport);

        }

        public void setDBComboBoxAgency()
        {
            DataTable Dt;



            using (var ds = new System.Data.DataSet())
            {


                string query = "SELECT DISTINCT AgencyCode,LTRIM(AgencyName) as AgencyName FROM tblCollectionReport ORDER BY AgencyCode";

                using (SqlDataAdapter ada = new SqlDataAdapter(query, Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }

                Dt = ds.Tables[0];
            }

            Common.setComboList(cboAgency, Dt, "AgencyCode", "AgencyName");


            cboAgency.SelectedIndex = -1;


        }

        public void setDBComboBoxBank()
        {
            DataTable Dt;



            using (var ds = new System.Data.DataSet())
            {


                string query = "SELECT DISTINCT BankCode,BankName FROM tblCollectionReport ORDER BY BankCode";

                using (SqlDataAdapter ada = new SqlDataAdapter(query, Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }

                Dt = ds.Tables[0];
            }

            Common.setComboList(cboBank, Dt, "BankCode", "BankName");


            cboBank.SelectedIndex = -1;


        }

        public void setDBComboBoxBranch()
        {
            DataTable Dt;



            using (var ds = new System.Data.DataSet())
            {


                string query = "SELECT DISTINCT BranchCode,BranchName FROM tblCollectionReport ORDER BY BranchCode";

                using (SqlDataAdapter ada = new SqlDataAdapter(query, Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }

                Dt = ds.Tables[0];
            }

            Common.setComboList(cboBranch, Dt, "BranchCode", "BranchName");


            cboBranch.SelectedIndex = -1;


        }

        private string GetUniqueKey()
            {
            int maxSize  = 8 ;
            int minSize = 5 ;
            char[] chars = new char[62];
            string a;
            a = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            chars = a.ToCharArray();
            int size  = maxSize ;
            byte[] data = new byte[1];
            RNGCryptoServiceProvider  crypto = new RNGCryptoServiceProvider();
            crypto.GetNonZeroBytes(data) ;
            size =  maxSize ;
            data = new byte[size];
            crypto.GetNonZeroBytes(data);
            StringBuilder result = new StringBuilder(size) ;
            foreach(byte b in data )
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
string querydelte="delete from BankReceipts";
                     using (SqlCommand sqlCommand1 = new SqlCommand(querydelte, db, transaction))
                                {
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
        

    }

}
