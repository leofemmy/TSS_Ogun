using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BankReconciliation.ReceiptCollection;
using TaxSmartSuite.Class;
using System.Data.SqlClient;
using DevExpress.XtraGrid.Views.Grid;
using BankReconciliation.Class;
using DevExpress.Utils;
using System.Globalization;

namespace BankReconciliation.Forms
{
    public partial class FrmCashiers : Form
    {
        private string BatchNumber;

        public static FrmCashiers publicInstance;

        int iCount = 0;

        //DBConnection connect = new DBConnection();

        //Methods extMethods = new Methods();

        public static FrmCashiers publicStreetGroup;

        protected TransactionTypeCode iTransType;

        protected bool boolIsUpdate;

        protected string ID;

        //private SqlDataAdapter adp;

        private SqlCommand command;

        string firstday, lastday, receipts, statename;

        bool isFirst = true;

        bool isSecond = true;

        bool isThird = true;

        bool isFourth = true;

        bool isFifth = true;

        bool isSeven = true;

        DataTable Dts, DtSR, DtSrec;

        private string status;

        string retval, query, types, statecode, strMonth, payref, officecode;

        private SqlCommand _command; private SqlDataAdapter adp; private DataTable Dtsg;

        private string uploading, PayerID, stationname, staioncodes;

        string UCC = "INT*78";
        string PCC = "INT*00";

        //ReceiptCollection.SERVERREEMS recep = new ReceiptCollection.SERVERREEMS();
        //private OyoReceiptServiceOnline.OyoReceiptService _oyoService = new OyoReceiptServiceOnline.OyoReceiptService();//online one

        private ReceiptCollection.SERVERREEMS _recept = new SERVERREEMS();


        public FrmCashiers()
        {
            InitializeComponent();

            publicInstance = this;

            publicStreetGroup = this;

            statecode = Program.stateCode;

            string query = String.Format("select statename from tblState where StateCode= '{0}' ", Program.stateCode);

            //statename = extMethods.getQuery("bankcode", query);

            //payref = extMethods.getQuery("TAXCODE", statecode);

            //officecode = extMethods.getQuery("OFFICE", statecode);


            DataTable dts = (new Logic()).getSqlStatement((String.Format("select statename,TaxCode from tblState where StateCode= '{0}' ", Program.stateCode))).Tables[0];

            if (dts != null && dts.Rows.Count > 0)
            {
                statename = dts.Rows[0]["statename"].ToString();
                payref = dts.Rows[0]["TaxCode"].ToString();
                //cboAccount.Text = dts.Rows[0]["AccountNumber"].ToString();

            }

            setImages();

            ToolStripEvent();

            iTransType = TransactionTypeCode.Null;

            Load += OnFormLoad;

            gridView1.DoubleClick += gridView1_DoubleClick;

            btnAgent.Click += Bttn_Click;

            btnPerson.Click += Bttn_Click;

            btnGenrate.Click += Bttn_Click;

            btnUpdate.Click += Bttn_Click;

            btnSearch.Click += Bttn_Click;

            btnUpload.Click += Bttn_Click;

            OnFormLoad(null, null);

            groupControl2.Enabled = false;

            dtpDate.ValueChanged += dtpDate_ValueChanged;

            cboBank.SelectedIndexChanged += cboBank_SelectedIndexChanged;

            cboPaidBranch.SelectedIndexChanged += cboPaidBranch_SelectedIndexChanged;

            cboOffice.SelectedIndexChanged += cboPayMode_SelectedIndexChanged;

            cboAgency.SelectedIndexChanged += cboAgency_SelectedIndexChanged;

            cboOffice.KeyPress += cboPaymode_KeyPress;

            cboAgency.KeyPress += cboAgency_KeyPress;

            //cboBank.KeyPress += cboBank_KeyPres;
            cboBank.KeyPress += cboBank_KeyPress;

            //cboRevenue.KeyPress +=new KeyPressEventHandler(cboRevenue_KeyPress);

            cboCheque.SelectedIndexChanged += cboCheque_SelectedIndexChanged;

            cboOffice.SelectedIndexChanged += cboOffice_SelectedIndexChanged;

            cboPaidBranch.KeyPress += cboPaidBranch_KeyPress;

            luePayeridPerson.EditValueChanged += luePayeridPerson_EditValueChanged;

            lupeAgent.EditValueChanged += lupeAgent_EditValueChanged;

            cedDepositAmt.Leave += cedDepositAmt_Leave;

            txtDepName.Leave += txtDepName_Leave;

            txtAddress.Leave += txtAddress_Leave;

            timer1.Tick += timer1_Tick;
            //cedDepositAmt.Properties.Mask.UseMaskAsDisplayFormat = True;

            //NavBars.ToolStripEnableDisable(toolStrip, null, false);

            //UploadData();
        }

        void cedDepositAmt_Leave(object sender, EventArgs e)
        {

            cedDepositAmt.Properties.DisplayFormat.FormatType = FormatType.Custom;
            cedDepositAmt.Properties.DisplayFormat.FormatString = "n2";
        }

        void timer1_Tick(object sender, EventArgs e)
        {
            UploadData();
        }

        void txtAddress_Leave(object sender, EventArgs e)
        {
            txtAddress.Text = txtAddress.Text.Trim().ToUpper();
        }

        void cboOffice_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboOffice.SelectedValue != null && !isSeven)
            {
                //setDBComboBoxRevenue((string)cboAgency.SelectedValue);
                DataTable dts = (new Logic()).getSqlStatement((String.Format("SELECT StationCode,StationName FROM dbo.tblStationMap WHERE RevenueOfficeCode= '{0}' ", (string)cboOffice.SelectedValue))).Tables[0];

                if (dts != null && dts.Rows.Count > 0)
                {
                    stationname = dts.Rows[0]["StationName"].ToString();
                    staioncodes = dts.Rows[0]["StationCode"].ToString();
                    //payref = dts.Rows[0]["TaxCode"].ToString();
                    //cboAccount.Text = dts.Rows[0]["AccountNumber"].ToString();

                }
            }
        }

        void cboAgency_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (cboAgency.SelectedValue != null && !isFifth)
            {
                setDBComboBoxRevenue((string)cboAgency.SelectedValue);
            }
        }

        void txtDepName_Leave(object sender, EventArgs e)
        {
            txtDepName.Text = txtDepName.Text.Trim().ToUpper();
        }

        private void setImages()
        {
            tsbNew.Image = MDIMains.publicMDIParent.i16x16.Images[5];
            tsbEdit.Image = MDIMains.publicMDIParent.i16x16.Images[6];
            tsbDelete.Image = MDIMains.publicMDIParent.i16x16.Images[7];
            tsbReload.Image = MDIMains.publicMDIParent.i16x16.Images[0];
            tsbClose.Image = MDIMains.publicMDIParent.i16x16.Images[11];

            //bttnCancel.Image = MDIMains.publicMDIParent.i32x32.Images[9];
            //bttnReset.Image = MDIMains.publicMDIParent.i32x32.Images[8];
            btnUpdate.Image = MDIMains.publicMDIParent.i32x32.Images[7];
            btnSearch.Image = MDIMains.publicMDIParent.i32x32.Images[2];

        }

        void ToolStripEvent()
        {
            tsbClose.Click += OnToolStripItemsClicked;
            tsbNew.Click += OnToolStripItemsClicked;
            tsbEdit.Click += OnToolStripItemsClicked;
            tsbDelete.Click += OnToolStripItemsClicked;
            tsbReload.Click += OnToolStripItemsClicked;
        }

        void gridView1_DoubleClick(object sender, EventArgs e)
        {
            //tsbEdit.PerformClick();
            EditRecordMode();
        }

        void OnToolStripItemsClicked(object sender, EventArgs e)
        {
            if (sender == tsbClose)
            {
                MDIMains.publicMDIParent.RemoveControls();
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

                //boolIsUpdate = false;

                //make new button function like edit for the cash system

                groupControl2.Text = "Edit Record Mode";

                iTransType = TransactionTypeCode.Edit;

                ShowForm();

                boolIsUpdate = true;
            }
            else if (sender == tsbEdit)
            {
                groupControl2.Text = "Edit Record Mode";

                iTransType = TransactionTypeCode.Edit;

                ShowForm();

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

        protected bool EditRecordMode()
        {
            bool bResponse = false;

            GridView view = (GridView)gridControl1.FocusedView;

            if (view != null)
            {
                DataRow dr = view.GetDataRow(view.FocusedRowHandle);

                if (dr != null)
                {
                    ID = dr["PaymentRefNumber"].ToString();

                    bResponse = FillField(dr["PaymentRefNumber"].ToString());
                    boolIsUpdate = true;
                    cboBank.Enabled = false;
                }
                else
                {
                    Common.setMessageBox("No Record is seleected", "", 1);

                    bResponse = false;
                }
            }
            return bResponse;
        }

        void Bttn_Click(object sender, EventArgs e)
        {
            if (sender == btnPerson)
            {
                //tsbReload.PerformClick();
                lupeAgent.Visible = false;
                luePayeridPerson.Visible = true;
            }
            else if (sender == btnAgent)
            {
                luePayeridPerson.Visible = false;
                lupeAgent.Visible = true;
                //if (!boolIsUpdate)
                //    Clear();
                //else
                //    FillField(ID);
                //setReload();
            }
            else if (sender == btnSearch)
            {
                searchRecord();
            }
            else if (sender == btnGenrate)
            {
                Generate();
            }
            else if (sender == btnUpdate)
            {
                UpdateRecord();
            }
            else if (sender == btnUpload)
            {
                timer1.Enabled = true;

                timer1.Start();
            }
        }

        private void Generate()
        {
            if (cboAgency.Text.Length < 1)
            {
                Common.setMessageBox("Please Select Revenue Type", Program.ApplicationName, 1);

                cboAgency.Focus();
                return;
            }
            else
            {
                //generate number
                BatchNumber = String.Format("{0:d5}", (DateTime.Now.Ticks / 10) % 100000);

                #region


                //using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
                //{
                //    connect.Open();
                //    _command = new SqlCommand("doGeneratedReceipNumber", connect) { CommandType = CommandType.StoredProcedure };

                //    using (System.Data.DataSet ds = new System.Data.DataSet())
                //    {
                //        adp = new SqlDataAdapter(_command);
                //        adp.Fill(ds);
                //        Dtsg = ds.Tables[0];
                //        connect.Close();

                //        txtReceiptsNo.Text = String.Format("{0}", (string)ds.Tables[0].Rows[0]["ReceipitNo"]);
                //        status = "Cleared"; uploading = "Pending";
                //    }
                //}


                //receipts = Methods.generateRandomString(8);

                //txtReceiptsNo.Text =String.Format("ICM|{0}", receipts);

                //string bankcode = retval.ToString();
                //testif the bank selected is not null

                #endregion

                if (!boolIsUpdate)
                {
                    if (string.IsNullOrEmpty((string)cboBank.SelectedValue))
                    {
                        Common.setEmptyField("Paid Bank ", Program.ApplicationName);
                        return;
                    }
                    else
                    {
                        PayerID = String.Format("{0}|ICM|{1:dd-MM-yyyy}|{2}", cboBank.SelectedValue, DateTime.Now, BatchNumber);
                        txtPaymentRef.Text = String.Format("{0}|ICM|{1:dd-MM-yyyy}|{2}", cboBank.SelectedValue, DateTime.Now, BatchNumber);
                        label11.Visible = true; txtPaymentRef.Visible = true;
                    }
                }
                else
                { }

                //txtPaymentRef.Text = String.Format("{0}|{1}|{2}|{3}", cboBank.SelectedValue.ToString(), payref, DateTime.Now.ToString("dd-MM-yyyy"), txtReceiptsNo.Text.Trim());

            }

        }

        private void searchRecord()
        {

            string querys;
            //determine search string/ parameter

            if (txtSeachString.Text == "")
            {
                querys = String.Format("SELECT PaymentRefNumber ,CONVERT(VARCHAR, PaymentDate, 103)  as PaymentDate  , DepositSlipNumber AS [Slip No] ,  Amount , BankName , BranchName FROM ViewCollectionReport where CONVERT(VARCHAR, CONVERT(DATE, PaymentDate), 102) BETWEEN '{0}' AND '{1}' AND (UploadStatus='Waiting') AND Provider='icma' and IsPayDirect=0 and IsRecordExit=1 ", string.Format("{0:yyyy/MM/dd}", dtpfrom.Value), string.Format("{0:yyyy/MM/dd}", dtpTo.Value));

            }
            else
            {
                querys = String.Format("SELECT PaymentRefNumber, CONVERT(VARCHAR, PaymentDate, 103)  as PaymentDate , DepositSlipNumber AS [Slip No], Amount, BankName, BranchName FROM ViewCollectionReport where PaymentRefNumber LIKE '%{0}%' AND (UploadStatus='Waiting') AND Provider='icma'and IsPayDirect=0 and IsRecordExit=1 ", txtSeachString.Text.Trim());
            }
            using (var ds = new System.Data.DataSet())
            {


                using (SqlDataAdapter ada = new SqlDataAdapter(querys, Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }

                DtSR = ds.Tables[0];

                gridControl1.DataSource = DtSR.DefaultView;
            }

            gridView1.OptionsBehavior.Editable = false;
            gridView1.Columns["Amount"].DisplayFormat.FormatType = FormatType.Numeric;
            gridView1.Columns["Amount"].DisplayFormat.FormatString = "n2";
            gridView1.BestFitColumns();
        }

        private void UpdateRecord()
        {
            try
            {
                if (txtSlipNo.Text == "")
                {
                    Common.setEmptyField("Deposit Slip No", Program.ApplicationName);
                    txtSlipNo.Focus();
                    return;
                }
                else if (cboPaidBranch.Text == "")
                {
                    Common.setEmptyField(" Branch Paid", Program.ApplicationName);
                    cboPaidBranch.Focus();
                    return;
                }
                else if (cboBank.Text == "")
                {
                    Common.setEmptyField("Bank Name", Program.ApplicationName);
                    cboBank.Focus();
                    return;
                }
                else if (txtDepName.Text == "")
                {
                    Common.setEmptyField("Depositor Name", Program.ApplicationName);
                    txtDepName.Focus();
                    return;
                }
                else if (cedDepositAmt.Text == "")
                {
                    Common.setEmptyField("Deposit Amount  ", Program.ApplicationName);
                    cedDepositAmt.Focus();
                    return;
                }
                //else if (cboOffice.Text == "")
                //{
                //    Common.setEmptyField(" Pay Mode ", Program.ApplicationName);
                //    cboOffice.Focus();
                //    return;
                //}
                //else if (txtPayerID.Text == "")
                //{
                //    Common.setEmptyField("Payer ID", Program.ApplicationName);

                //    txtPayerID.Focus();
                //    return;
                //}
                else
                {
                    //check form status either new or edit
                    if (!boolIsUpdate)
                    {
                        using (SqlConnection db = new SqlConnection(Logic.ConnectionString))
                        {
                            SqlTransaction transaction;

                            db.Open();

                            transaction = db.BeginTransaction();

                            try
                            {
                                string query = String.Format("INSERT INTO [tblCollectionReport]([Provider],[Channel],[PaymentRefNumber],[DepositSlipNumber],[PaymentDate],[PayerName],[PayerID],[RevenueCode],[Description],[Amount],[PaymentMethod],[BankCode],[BankName],[BranchCode],[BranchName],[AgencyName],[AgencyCode],[ZoneCode],[ZoneName],[State],[TelephoneNumber],[ChequeNumber],[ChequeValueDate],[ChequeBankCode],[ChequeBankName],ChequeStatus, StationCode,StationName,UploadStatus,GeneratedBy,PayerAddress) VALUES ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}','{19}','{20}','{21}','{22}','{23}','{24}','{25}','{26}','{27}','{28}','{29}','{30}');", "ICM", "Bank", txtPaymentRef.Text.Trim(), txtSlipNo.Text.Trim(), dtpDate.Value.Date.ToString("MM/dd/yyyy"), txtDepName.Text.Trim(), (string)PayerID, cboRevenue.SelectedValue.ToString(), (string)cboRevenue.Text, cedDepositAmt.Text, txtPayMode.Text, cboBank.SelectedValue, cboBank.Text, cboPaidBranch.Text, cboPaidBranch.Text, cboAgency.SelectedValue, cboAgency.SelectedValue, cboOffice.SelectedValue, cboOffice.Text, (string)statename, txtTelephone.Text.Trim(), txtChequeNo.Text.Trim(), dtpCheque.Value.Date.ToString("MM/dd/yyyy"), cboCheque.SelectedValue, cboCheque.SelectedText, "Cleared", staioncodes, stationname, "Pending", Program.UserID, txtAddress.Text.Trim().ToUpper());

                                using (SqlCommand sqlCommand1 = new SqlCommand(query, db, transaction))
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
                        #region

                        //setReload();
                        //setReload(Convert.ToInt32(cboAgency.SelectedValue.ToString()));
                        //sending record to online


                        //var receiptcoll = new ReceiptCollection.SERVERREEMS();

                        //string testvalue = receiptcoll.CollectReports(txtPaymentRef.Text.Trim(), txtSlipNo.Text.Trim(), Convert.ToString(dtpDate.Value.Date), (string)PayerID, txtDepName.Text.Trim(), txtTelephone.Text.Trim(), cboRevenue.SelectedValue.ToString(), (string)cboRevenue.Text, Convert.ToDecimal(cedDepositAmt.Text), txtPayMode.Text, txtChequeNo.Text.Trim(), Convert.ToString(dtpCheque.Value.Date), cboCheque.SelectedValue.ToString(), cboCheque.SelectedText, "Cleared", "", cboAgency.SelectedValue.ToString(), cboAgency.SelectedValue.ToString(), cboBank.SelectedValue.ToString(), cboBank.Text, cboPaidBranch.Text, cboPaidBranch.Text, cboOffice.SelectedValue.ToString(), cboOffice.Text, null, null, null, txtAddress.Text.Trim().ToUpper(), Program.UserID, UCC, PCC);

                        //if (testvalue == "00")
                        //{
                        //    using (SqlConnection db = new SqlConnection(Logic.ConnectionString))
                        //    {
                        //        SqlTransaction transaction;

                        //        db.Open();

                        //        transaction = db.BeginTransaction();
                        //        try
                        //        {
                        //            //MessageBox.Show(MDIMain.stateCode);
                        //            //fieldid
                        //            using (SqlCommand sqlCommand1 = new SqlCommand(String.Format(String.Format("UPDATE [tblCollectionReport] SET UploadStatus='{{0}}' where  [PaymentRefNumber] ='{0}'", txtPaymentRef.Text.Trim()), "Uploaded"), db, transaction))
                        //            {
                        //                sqlCommand1.ExecuteNonQuery();
                        //            }

                        //            transaction.Commit();
                        //        }
                        //        catch (SqlException sqlError)
                        //        {
                        //            transaction.Rollback();
                        //        }
                        //        db.Close();
                        //    }
                        //}
                        //else
                        //{

                        //}

                        #endregion

                        Common.setMessageBox("Record has been successfully added", Program.ApplicationName, 1);
                        clear(); label11.Visible = false;
                        txtPaymentRef.Visible = false;
                        iTransType = TransactionTypeCode.New;
                        ShowForm();

                    }
                    else
                    {
                        //update the records

                        using (SqlConnection db = new SqlConnection(Logic.ConnectionString))
                        {
                            SqlTransaction transaction;

                            db.Open();

                            transaction = db.BeginTransaction();
                            try
                            {

                                using (SqlCommand sqlCommand1 = new SqlCommand(String.Format(String.Format("UPDATE [tblCollectionReport] SET [AgencyName]='{{0}}',[AgencyCode]='{{1}}',[RevenueCode]='{{2}}' ,[Description] ='{{3}}',[ZoneCode]='{{4}}',[ZoneName]='{{5}}',UploadStatus='{{6}}',[DepositSlipNumber]='{{7}}',[BranchCode]='{{8}}',[BranchName]='{{9}}',PayerName='{{10}}' where  [PaymentRefNumber] ='{0}'", ID), cboAgency.Text, cboAgency.SelectedValue, cboRevenue.SelectedValue, cboRevenue.Text, cboOffice.SelectedValue, cboOffice.Text, "Pending", txtSlipNo.Text.Trim(), cboPaidBranch.SelectedValue, cboPaidBranch.Text, txtDepName.Text), db, transaction))
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

                        searchRecord();
                        //setReload(Convert.ToInt32(cboAgency.SelectedValue.ToString()));
                        Common.setMessageBox("Changes in record has been successfully saved.", Program.ApplicationName, 1);
                        //bttnReset.PerformClick();
                        label11.Enabled = false;
                        label11.Visible = false;
                        txtPaymentRef.Visible = false;
                        clear();
                        iTransType = TransactionTypeCode.Null;
                        ShowForm();

                    }
                }
            }
            catch (Exception ex)
            {
                Tripous.Sys.ErrorBox(ex);
            }

        }

        private bool FillField(string fieldid)
        {
            bool bResponse = false;

            //string[] Splits1;

            //load data from the table into the forms for edit
            string query = String.Format("select  Provider, Channel, PaymentRefNumber, DepositSlipNumber, CONVERT(VARCHAR, PaymentDate, 103)  AS PaymentDate, PayerID, PayerName, Amount,PaymentMethod, ChequeNumber, ChequeValueDate, ChequeStatus, DateChequeReturned, TelephoneNumber, ReceiptNo, ReceiptDate, PayerAddress, Status, [User],RevenueCode, Description, ChequeBankCode, ChequeBankName, AgencyName, AgencyCode, BankCode, BankName, BranchCode, BranchName, ZoneCode,ZoneName, Username, State, AmountWords, URL, EReceipts, EReceiptsDate, GeneratedBy, DateValidatedAgainst, DateDiff, UploadStatus, PrintedBY, DatePrinted, ControlNumber, BatchNumber, StationCode, StationName, ID, isPrinted, IsNormalize, NormalizeBy, NormalizeDate, IsPrintedDate, CentreCode, CentreName, IsRecordExit, IsPayDirect from ViewCollectionReport where PaymentRefNumber ='{0}'", fieldid);

            DataTable dts = (new Logic()).getSqlStatement(query).Tables[0];


            if (dts != null)
            {
                bResponse = true;

                label11.Visible = true;

                txtPaymentRef.Visible = true;


                txtPaymentRef.Text = dts.Rows[0]["PaymentRefNumber"].ToString();
                txtSlipNo.Text = dts.Rows[0]["DepositSlipNumber"].ToString();

                dtpDate.Value = DateTime.Parse(dts.Rows[0]["PaymentDate"].ToString(), CultureInfo.CreateSpecificCulture("en-GB"));

                txtReceipDate.Value = DateTime.Parse(dts.Rows[0]["PaymentDate"].ToString(), CultureInfo.CreateSpecificCulture("en-GB"));
                txtPayerID.Text = dts.Rows[0]["PayerID"].ToString();
                txtPayerName.Text = dts.Rows[0]["PayerName"].ToString();
                txtDepName.Text = dts.Rows[0]["PayerName"].ToString();
                cboAgency.Text = dts.Rows[0]["AgencyName"].ToString();
                cboRevenue.Text = dts.Rows[0]["Description"].ToString();
                cedDepositAmt.Text = dts.Rows[0]["Amount"].ToString();
                //cedAmount.Text = dts.Rows[0]["Amount"].ToString();
                cboOffice.Text = dts.Rows[0]["ZoneName"].ToString();
                cboBank.Text = dts.Rows[0]["BankName"].ToString();
                cboBank.SelectedValue = dts.Rows[0]["BankCode"].ToString();
                cboPaidBranch.Text = dts.Rows[0]["BranchName"].ToString();
                //txtReceiptsNo.Text = dts.Rows[0]["ReceiptNo"].ToString();
                groupControl2.Enabled = true;
                splitContainer1.Panel1Collapsed = false;
                splitContainer1.Panel2Collapsed = true;
            }
            else
                bResponse = false;

            return bResponse;
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

        void OnFormLoad(object sender, EventArgs e)
        {
            ShowForm();

            setDBComboBox();

            //setDBChequeBankComboBox();

            setDBComboBoxPay();

            //setDBComboBoxRevenue();

            setDBComboBoxAgent();

            setDBComboBoxPerson();

            setGetOffAgency();

            setDBComboBoxAgency();

            setDBComboOffice();

            isFirst = false;

            isSecond = false;

            isThird = false;

            isFourth = false;

            isFifth = false;

            isSeven = false;

            //populate year box
            //Methods.PopulateYear(cboYear);

            //populate months
            //Methods.PopulateMonth(cboMonth);

            //get default currency

            DataTable dts = (new Logic()).getSqlStatement((string)"select CurrencyName  from tblCurrency where Flag =1").Tables[0];

            if (dts != null)
            {
                txtCurrency.Text = dts.Rows[0]["CurrencyName"].ToString();
            }

            //cboMonth.SelectedIndexChanged += cboMonth_SelectedIndexChanged;




        }

        void cboAgency_KeyPress(object sender, KeyPressEventArgs e)
        {
            Methods.AutoComplete(cboAgency, e, true);
        }

        void luePayeridPerson_EditValueChanged(object sender, EventArgs e)
        {
            string values = string.Empty;

            object val = luePayeridPerson.EditValue;

            object[] lol = val.ToString().Split(',');

            int i = 0;

            foreach (object obj in lol)
            {
                values += String.Format("{0}", obj.ToString().Trim());

                if (i + 1 < lol.Count())

                    values += ",";

                ++i;

            }

            txtPayerName.Text = values.ToString();

            txtPayerID.Text = luePayeridPerson.Text;

            luePayeridPerson.Visible = false;
        }

        void lupeAgent_EditValueChanged(object sender, EventArgs e)
        {
            string values = string.Empty;

            object val = lupeAgent.EditValue;

            object[] lol = val.ToString().Split(',');

            int i = 0;

            foreach (object obj in lol)
            {
                values += String.Format("{0}", obj.ToString().Trim());

                if (i + 1 < lol.Count())

                    values += ",";

                ++i;

            }

            txtPayerName.Text = values.ToString();
            txtPayerID.Text = lupeAgent.Text;
            lupeAgent.Visible = false;
        }

        public void setDBComboBox()
        {
            DataTable Dt;

            using (var ds = new System.Data.DataSet())
            {
                using (SqlDataAdapter ada = new SqlDataAdapter("select *  from tblBank", Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }

                Dt = ds.Tables[0];
            }

            Common.setComboList(cboBank, Dt, "BankShortCode", "BankName");


            cboBank.SelectedIndex = -1;


        }

        public void setDBComboBoxAgency()
        {
            DataTable Dt;

            using (var ds = new System.Data.DataSet())
            {

                using (SqlDataAdapter ada = new SqlDataAdapter((string)"SELECT AgencyCode,AgencyName FROM dbo.tblAgency", Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }

                Dt = ds.Tables[0];
            }

            Common.setComboList(cboAgency, Dt, "AgencyCode", "AgencyName");

            cboAgency.SelectedIndex = -1;

        }
        //cboOffice
        //public void setDBChequeBankComboBox()
        //{
        //    DataTable Dt;



        //    using (var ds = new System.Data.DataSet())
        //    {


        //        string query = "select *  from tblBank";

        //        using (SqlDataAdapter ada = new SqlDataAdapter(query, Logic.ConnectionString))
        //        {
        //            ada.Fill(ds, "table");
        //        }

        //        Dt = ds.Tables[0];
        //    }

        //    Common.setComboList(cboCheque, Dt, "BankShortCode", "BankName");


        //    cboBank.SelectedIndex = -1;


        //}

        void setDBComboOffice()
        {
            DataTable Dt;

            using (var ds = new System.Data.DataSet())
            {


                using (SqlDataAdapter ada = new SqlDataAdapter((string)"SELECT OfficeName FROM dbo.tblRevenueOffice", Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }

                Dt = ds.Tables[0];
            }

            Common.setComboList(cboOffice, Dt, "OfficeName", "OfficeName");


            cboOffice.SelectedIndex = -1;
        }

        // public void setDBChequeBankComboBox()
        //{
        //    DataTable Dt;

        //    using (var ds = new System.Data.DataSet())
        //    {


        //        using (SqlDataAdapter ada = new SqlDataAdapter((string)"SELECT RevenueOfficeCode,RevenueOfficeName FROM dbo.tblStationMap", Logic.ConnectionString))
        //        {
        //            ada.Fill(ds, "table");
        //        }

        //        Dt = ds.Tables[0];
        //    }

        //    Common.setComboList(cboOffice, Dt, "RevenueOfficeCode", "RevenueOfficeName");


        //    cboOffice.SelectedIndex = -1;


        //}


        private void clear()
        {
            txtPaymentRef.Clear();
            //cboMonth.Text = string.Empty;
            cboBank.Text = string.Empty;
            cboPaidBranch.Text = string.Empty;
            txtActNo.Clear();
            txtSlipNo.Clear();
            cedDepositAmt.Text = string.Empty;
            txtDepName.Clear();
            txtBatchNo.Clear();
            cboAgency.Text = string.Empty;
            txtPayerID.Clear();
            txtPayerName.Clear();
            txtReceiptsNo.Clear();
            txtAddress.Clear();
            cboRevenue.SelectedIndex = -1;
            //cedAmount.Text = string.Empty;
            cboOffice.Text = string.Empty;
            //txtCheque.Clear();
        }

        public void setGetOffAgency()
        {


            using (var ds = new System.Data.DataSet())
            {


                string query = String.Format("select AgencyCode ,AgencyName,ZoneID,ZoneName from ViewRevenueOffice where RevenueOfficeID = '{0}'", officecode);

                using (SqlDataAdapter ada = new SqlDataAdapter(query, Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }

                Dts = ds.Tables[0];
            }

        }

        public void setDBComboBoxPay()
        {
            DataTable Dt;



            using (var ds = new System.Data.DataSet())
            {


                string query = "select *  from tblPayMode";

                using (SqlDataAdapter ada = new SqlDataAdapter(query, Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }

                Dt = ds.Tables[0];
            }


            Common.setComboList(cboOffice, Dt, "PayID", "description");

            cboOffice.SelectedIndex = -1;


        }

        public void setDBComboBoxAgent()
        {
            DataTable Dt;

            using (var ds = new System.Data.DataSet())
            {


                string query = "select TaxAgentReferenceNumber as [Agent Name], OrganizationName from tblTaxAgent";

                using (SqlDataAdapter ada = new SqlDataAdapter(query, Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }

                Dt = ds.Tables[0];
            }

            //Common.setCheckEdit(luePayeridPerson, Dt, "BankShortCode", "BankName");

            //Common.setLookUpEdit(lupeAgent, Dt, "TaxAgentReferenceNumber", "OrganizationName");

            //Common.setLookUpEdit(lupeAgent, Dt, "TaxAgentReferenceNumber", "OrganizationName");

            cboBank.SelectedIndex = -1;


        }

        public void setDBComboBoxPerson()
        {
            DataTable Dt;


            using (var ds = new System.Data.DataSet())
            {

                string query = "select TaxPayerReferenceNumber as [Payer Name],Surname + '' + Othernames  as fullname from tblTaxPayer";

                using (SqlDataAdapter ada = new SqlDataAdapter(query, Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }

                Dt = ds.Tables[0];
            }

            //Common.setCheckEdit(luePayeridPerson, Dt, "BankShortCode", "BankName");

            //Common.setLookUpEdit(luePayeridPerson, Dt, "TaxAgentReferenceNumber", "fullname");

            cboBank.SelectedIndex = -1;


        }

        public void setDBComboBoxRevenue(string strAgencycode)
        {
            DataTable Dt;

            using (var ds = new System.Data.DataSet())
            {
                using (SqlDataAdapter ada = new SqlDataAdapter(string.Format("select RevenueCode,Description  from tblRevenueType WHERE AgencyCode ='{0}'", strAgencycode), Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }

                Dt = ds.Tables[0];
            }

            Common.setComboList(cboRevenue, Dt, "RevenueCode", "Description");

            cboRevenue.SelectedIndex = -1;


        }

        void cboPaymode_KeyPress(object sender, KeyPressEventArgs e)
        {

            Methods.AutoComplete(cboOffice, e, true);
        }

        //void cboMonth_SelectedIndexChanged(object sender, EventArgs e)
        //{

        //    string period = " Current Tranacstion Period ";

        //    string periodTo = " To ";

        //    string texts = " Cashiers Book ";

        //    groupControl1.Text = string.Empty;


        //    // set first day 
        //    firstday = extMethods.GetFirstDayOfMonth(Convert.ToInt32(cboMonth.SelectedValue),Convert.ToInt32( cboYear.SelectedValue)).ToShortDateString();

        //    // set last day 
        //    lastday = extMethods.GetLastDayOfMonth(Convert.ToInt32(cboMonth.SelectedValue), Convert.ToInt32(cboYear.SelectedValue)).ToShortDateString();


        //    groupControl1.Text = texts + '-' + period + firstday + periodTo + lastday;

        //}

        void cboBank_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboBank.SelectedValue != null && !isFirst)
            {
                setDBComboBoxBranch(cboBank.SelectedValue.ToString());

                cboPaidBranch.SelectedIndex = -1;
                txtActNo.Clear();

            }
        }

        public void setDBComboBoxBranch(string parameter)
        {
            DataTable Dt;

            using (var ds = new System.Data.DataSet())
            {


                string query = String.Format("select BranchCode ,BranchName from tblBankBranch where BankShortCode = '{0}'", parameter);

                using (SqlDataAdapter ada = new SqlDataAdapter(query, Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }

                Dt = ds.Tables[0];
            }

            Common.setComboList(cboPaidBranch, Dt, "BranchCode", "BranchName");


            if (Dt.Rows.Count > 0)
                cboPaidBranch.SelectedIndex = 0;
            else
                cboPaidBranch.SelectedIndex = -1;
        }

        ////private void getPayMode(int parameter)
        //{
        //    if (parameter == 1)
        //    {
        //        label20.Visible = true;
        //        txtCheque.Visible = true;
        //    }
        //    else
        //        label20.Visible = false;
        //    txtCheque.Visible = false;

        //}

        void cboPaidBranch_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboPaidBranch.SelectedValue != null && !isSecond)
            {
                //cboBranch.SelectedIndex = 0;
                setDBComboBoxBranchAcct(cboPaidBranch.SelectedValue.ToString());

            }
        }

        void cboPayMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboOffice.SelectedValue != null && !isThird)
            {
                //getPayMode(Convert.ToInt32(cboPayMode.SelectedValue.ToString()));
                if (String.IsNullOrEmpty(cboOffice.SelectedValue.ToString()))
                {
                    return;
                }
                else
                {
                    if (string.IsNullOrEmpty(cboOffice.SelectedValue.ToString()))
                    {
                        label20.Visible = true;
                        lblchequedate.Visible = true;
                        lblChqueBank.Visible = true;
                        txtChequeNo.Visible = true;
                        cboCheque.Visible = true;
                        dtpCheque.Visible = true;
                    }
                    else
                    {
                        label20.Visible = false;
                        lblchequedate.Visible = false;
                        lblChqueBank.Visible = false;
                        txtChequeNo.Visible = false;
                        cboCheque.Visible = false;
                        dtpCheque.Visible = false;
                    }
                }

            }


        }

        void cboCheque_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboCheque.SelectedValue != null && !isFourth)
            {
                //getPayMode(Convert.ToInt32(cboPayMode.SelectedValue.ToString()));
                if (cboCheque.SelectedValue.ToString() != cboBank.SelectedValue.ToString())
                {
                    DateTime dtc = dtpDate.Value;

                    dtpCheque.Value = dtc.AddDays(3);
                }
                else
                {
                    dtpCheque.Value = dtpDate.Value;
                }
            }


        }

        public void setDBComboBoxBranchAcct(string parameter)
        {
            DataTable Dt;

            using (var ds = new System.Data.DataSet())
            {
                string query = String.Format("select AccountNumber from tblBankAccount where BranchCode = '{0}' and BankCode='{1}'", parameter, cboBank.SelectedValue.ToString());
                using (SqlDataAdapter ada = new SqlDataAdapter(query, Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }

                Dt = ds.Tables[0];
            }

            if (Dt != null && Dt.Rows.Count > 0)
            { txtActNo.Text = String.Format("{0}", Dt.Rows[0]["AccountNumber"]); }


        }

        void cboBank_KeyPress(object sender, KeyPressEventArgs e)
        {
            Methods.AutoComplete(cboBank, e, true);
        }

        void cboPaidBranch_KeyPress(object sender, KeyPressEventArgs e)
        {
            Methods.AutoComplete(cboPaidBranch, e, true);
        }

        //void cboYear_KeyPress(object sender, KeyPressEventArgs e)
        //{
        //    Methods.AutoComplete(cboYear, e, true);
        //}

        //void cboMonth_KeyPress(object sender, KeyPressEventArgs e)
        //{
        //    Methods.AutoComplete(cboMonth, e, true);
        //}

        void dtpDate_ValueChanged(object sender, EventArgs e)
        {
            //if (iCount == 0)
            //{
            //    int getMonth = Convert.ToDateTime(dtpDate.Value).Month;

            //    int getYear = Convert.ToDateTime(dtpDate.Value).Year;

            //    if (Convert.ToInt32(cboMonth.SelectedValue) != getMonth && Convert.ToInt32(cboYear.SelectedValue) != getYear)
            //    {
            //        Common.setMessageBox("Date is Outside Current Entry Period", Program.ApplicationName, 2);
            //    }

            //    iCount++;
            //}
            //else
            //    iCount = 0;
        }

        public void RefreshForm()
        {
            NavBars.ToolStripEnableDisableControls(toolStrip, Tag as String);
        }

        void UpdateGenerate()
        { }

        void docount()
        {
            System.Data.DataSet ds = new System.Data.DataSet();

            try
            {
                using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
                {
                    connect.Open();

                    command = new SqlCommand("doCountCashierRecord", connect) { CommandType = CommandType.StoredProcedure, CommandTimeout = 0 };


                    adp = new SqlDataAdapter(command);
                    adp.Fill(ds);
                    Dts = ds.Tables[0];
                    connect.Close();
                    //if (ds.Tables.Count == 1)
                    //{
                    //    Common.setMessageBox(ds.Tables[0].Rows[0]["returnMessage"].ToString(), Program.ApplicationName, 3);
                    //    timer2.Enabled = false;
                    //    btnStop.Visible = false;
                    //    btnStartup.Visible = true;
                    //}
                    //else
                    //{
                    lblAll.Text = ds.Tables[0].Rows[0]["All_Record"].ToString();
                    lblDownload.Text = ds.Tables[1].Rows[0]["Upload"].ToString();
                    lblRemain.Text = ds.Tables[2].Rows[0]["Remain"].ToString();
                    lblError.Text = ds.Tables[3].Rows[0]["Error"].ToString();
                    //}
                    //}
                }


                //return dataSet;
            }
            catch (Exception ex)
            {
                Common.setMessageBox(String.Format("{0}....{1}..Do update uploadRecords On Station", ex.Message, ex.StackTrace), Program.ApplicationName, 3);
                //timer2.Enabled = false;
                //btnStop.Visible = false;
                //btnStartup.Visible = true;
                //return dataSet;
            }
            //return dataSet;
        }

        void UploadData()
        {
            #region

            //string querys = "SELECT   Provider, Channel, PaymentRefNumber, DepositSlipNumber, CONVERT(DATETIME, PaymentDate) AS PaymentDate, PayerID, PayerName, Amount, PaymentMethod,ChequeNumber, ChequeValueDate, ChequeStatus, DateChequeReturned, TelephoneNumber, ReceiptNo, ReceiptDate, PayerAddress, Status, [User], RevenueCode, Description, ChequeBankCode, ChequeBankName, AgencyName, AgencyCode, BankCode, BankName, BranchCode, BranchName, ZoneCode, ZoneName, Username,              State, AmountWords, URL, EReceipts, EReceiptsDate, GeneratedBy, DateValidatedAgainst, DateDiff, UploadStatus, PrintedBY, DatePrinted, ControlNumber, BatchNumber, StationCode, StationName, ID, isPrinted, IsNormalize, NormalizeBy, NormalizeDate, isPrintedDate, CentreCode, CentreName AND UploadStatus='pending' AND Provider='icm'"; 

            //determine search string/ parameter

            //if (txtSeachString.Text == "")
            //{
            //    querys = String.Format("SELECT PaymentRefNumber, PaymentDate, DepositSlipNumber AS [Slip No], Amount, BankName, BranchName FROM ViewCollectionReport where PaymentDate between '{0:yyyy-MM-dd}' and '{1:yyyy-MM-dd}' AND UploadStatus='pending' AND Provider='icm' ", dtpfrom.Value.Date, dtpTo.Value.Date);

            //}
            //else
            //{
            //    querys = String.Format("SELECT PaymentRefNumber, PaymentDate, DepositSlipNumber AS [Slip No], Amount, BankName, BranchName FROM ViewCollectionReport where PaymentRefNumber LIKE '%{0}%' AND UploadStatus='pending' AND Provider='icm'", txtSeachString.Text.Trim());
            //}
            //using (var ds = new System.Data.DataSet())
            //{
            //    using (SqlDataAdapter ada = new SqlDataAdapter(querys, Logic.ConnectionString))
            //    {
            //        ada.Fill(ds, "table");
            //    }

            //    DtSrec = ds.Tables[0];

            //}
            #endregion


            try
            {
                DataTable dts = (new Logic()).getSqlStatement(("SELECT TOP 1  Provider, Channel, PaymentRefNumber, DepositSlipNumber, (CONVERT(VARCHAR,PaymentDate,101) + ' ' + CONVERT(VARCHAR,PaymentDate,108)) AS PaymentDate, PayerID, PayerName, Amount, PaymentMethod,ChequeNumber,  ChequeValueDate, ChequeStatus, DateChequeReturned, TelephoneNumber, ReceiptNo, ReceiptDate, PayerAddress, Status, [User], RevenueCode, Description, ChequeBankCode, ChequeBankName, AgencyName, AgencyCode, BankCode, BankName, BranchCode, BranchName, ZoneCode, ZoneName, Username, State, AmountWords, URL, EReceipts, EReceiptsDate, GeneratedBy, DateValidatedAgainst, DateDiff, UploadStatus, PrintedBY, DatePrinted, ControlNumber, BatchNumber, StationCode, StationName, ID, isPrinted, IsNormalize, NormalizeBy, NormalizeDate, isPrintedDate, CentreCode, CentreName FROM dbo.ViewCollectionReport where UploadStatus='pending' AND Provider='FIC'")).Tables[0];

                if (dts != null && dts.Rows.Count > 0)
                {
                    var receiptcash = new ReceiptCollection.SERVERREEMS();

                    //Program.UserID, UCC, PCC

                    string retvalue = receiptcash.CollectReports(dts.Rows[0]["PaymentRefNumber"].ToString(), dts.Rows[0]["DepositSlipNumber"].ToString(), dts.Rows[0]["PaymentDate"].ToString(), dts.Rows[0]["PayerID"].ToString(), dts.Rows[0]["PayerName"].ToString(), dts.Rows[0]["TelephoneNumber"].ToString(), dts.Rows[0]["RevenueCode"].ToString(), dts.Rows[0]["Description"].ToString(), Convert.ToDecimal(dts.Rows[0]["Amount"]), dts.Rows[0]["PaymentMethod"].ToString(), dts.Rows[0]["ChequeNumber"].ToString(), dts.Rows[0]["PaymentDate"].ToString(), dts.Rows[0]["ChequeBankCode"].ToString(), dts.Rows[0]["ChequeBankName"].ToString(), dts.Rows[0]["ChequeStatus"].ToString(), dts.Rows[0]["DateChequeReturned"].ToString(), dts.Rows[0]["AgencyName"].ToString(), dts.Rows[0]["AgencyCode"].ToString(), dts.Rows[0]["BankCode"].ToString(), dts.Rows[0]["BankName"].ToString(), dts.Rows[0]["BranchCode"].ToString(), dts.Rows[0]["BranchName"].ToString(), dts.Rows[0]["ZoneCode"].ToString(), dts.Rows[0]["ZoneName"].ToString(), dts.Rows[0]["ZoneCode"].ToString(), dts.Rows[0]["ZoneName"].ToString(), dts.Rows[0]["DepositSlipNumber"].ToString(), dts.Rows[0]["PayerAddress"].ToString(), Program.UserID, UCC, PCC);

                    if (retvalue == "00")
                    {
                        using (SqlConnection db = new SqlConnection(Logic.ConnectionString))
                        {
                            SqlTransaction transaction;

                            db.Open();

                            transaction = db.BeginTransaction();
                            try
                            {
                                //MessageBox.Show(MDIMain.stateCode);
                                //fieldid
                                using (SqlCommand sqlCommand1 = new SqlCommand(String.Format(String.Format("UPDATE [tblCollectionReport] SET UploadStatus='{{0}}' where  [PaymentRefNumber] ='{0}'", (string)dts.Rows[0]["PaymentRefNumber"]), "Uploaded"), db, transaction))
                                {
                                    sqlCommand1.ExecuteNonQuery();
                                }

                                transaction.Commit();
                            }
                            catch (SqlException sqlError)
                            {
                                Tripous.Sys.ErrorBox(sqlError.Message, Program.ApplicationName, 2);

                                transaction.Rollback(); return;
                            }
                            db.Close();
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
                                //MessageBox.Show(MDIMain.stateCode);
                                //fieldid
                                using (SqlCommand sqlCommand1 = new SqlCommand(String.Format(String.Format("UPDATE [tblCollectionReport] SET UploadStatus='{{0}}' where  [PaymentRefNumber] ='{0}'", (string)dts.Rows[0]["PaymentRefNumber"]), (string)retvalue), db, transaction))
                                {
                                    sqlCommand1.ExecuteNonQuery();
                                }

                                transaction.Commit();
                            }
                            catch (SqlException sqlError)
                            {
                                Tripous.Sys.ErrorBox(sqlError.Message, Program.ApplicationName, 2);
                                transaction.Rollback();
                                return;
                            }
                            db.Close();
                        }
                    }

                    docount();

                }
                else
                {
                    Common.setMessageBox("No Payment Normalization record to be upload", Program.ApplicationName, 2);
                    timer1.Enabled = false;

                    timer1.Stop();

                    return;
                }
            }
            catch (Exception ex)
            {
                timer1.Stop();
                Tripous.Sys.ErrorBox(String.Format("{0}.......{1}", ex.Message, ex.StackTrace)); return;
            }

        }
    }
}
