using Collection.Classess;
using DevExpress.XtraGrid.Views.Grid;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;
using TaxSmartSuite.Class;

namespace Collection.Forms
{
    public partial class FrmCashiers : Form
    {
        public static FrmCashiers publicInstance;

        int iCount = 0;

        //DBConnection connect = new DBConnection();

        Methods extMethods = new Methods();

        public static FrmCashiers publicStreetGroup;

        protected TransactionTypeCode iTransType;

        protected bool boolIsUpdate;

        protected string ID;

        string firstday, lastday, receipts, statename;

        bool isFirst = true;

        bool isSecond = true;

        bool isThird = true;

        bool isFourth = true;

        DataTable Dts, DtSR;

        string retval, query, types, statecode, strMonth, payref, officecode;

        public FrmCashiers()
        {
            InitializeComponent();

            publicInstance = this;

            //connect.ConnectionString();

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

            iTransType = TransactionTypeCode.New;

            Load += OnFormLoad;

            gridView1.DoubleClick += gridView1_DoubleClick;

            btnAgent.Click += Bttn_Click;

            btnPerson.Click += Bttn_Click;

            btnGenrate.Click += Bttn_Click;

            btnUpdate.Click += Bttn_Click;

            btnSearch.Click += Bttn_Click;

            OnFormLoad(null, null);

            groupControl2.Enabled = false;

            dtpDate.ValueChanged += dtpDate_ValueChanged;

            //NavBars.ToolStripEnableDisable(toolStrip, null, false);
        }

        private void setImages()
        {
            tsbNew.Image = MDIMain.publicMDIParent.i16x16.Images[5];
            tsbEdit.Image = MDIMain.publicMDIParent.i16x16.Images[6];
            tsbDelete.Image = MDIMain.publicMDIParent.i16x16.Images[7];
            tsbReload.Image = MDIMain.publicMDIParent.i16x16.Images[0];
            tsbClose.Image = MDIMain.publicMDIParent.i16x16.Images[11];

            //bttnCancel.Image = MDIMain.publicMDIParent.i32x32.Images[9];
            //bttnReset.Image = MDIMain.publicMDIParent.i32x32.Images[8];
            btnUpdate.Image = MDIMain.publicMDIParent.i32x32.Images[7];
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

        void gridView1_DoubleClick(object sender, EventArgs e)
        {
            //tsbEdit.PerformClick();
            EditRecordMode();
        }

        void OnToolStripItemsClicked(object sender, EventArgs e)
        {
            if (sender == tsbClose)
            {
                MDIMain.publicMDIParent.RemoveControls();
            }
            else if (sender == tsbNew)
            {
                label11.Visible = false;

                txtPaymentRef.Visible = false;

                groupControl2.Text = "Add New Record";

                iTransType = TransactionTypeCode.New;

                ShowForm();

                clear();

                groupControl2.Enabled = true;

                boolIsUpdate = false;
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
        }

        private void Generate()
        {
            if (cboRevenue.Text.Length < 1)
            {
                Common.setMessageBox("Please Select Revenue Type", Program.ApplicationName, 1);

                cboRevenue.Focus();
                return;
            }
            else

                receipts = Methods.generateRandomString(8);

            txtReceiptsNo.Text = String.Format("ICM|{0}", receipts);

            //string bankcode = retval.ToString();
            txtPaymentRef.Text = String.Format("ICM|{0}|{1}|{2:dd-MM-yyyy}|{3}", cboBank.SelectedValue, payref, DateTime.Now, receipts);

            //txtPaymentRef.Text = String.Format("{0}|{1}|{2}|{3}", cboBank.SelectedValue.ToString(), payref, DateTime.Now.ToString("dd-MM-yyyy"), txtReceiptsNo.Text.Trim());

        }

        private void searchRecord()
        {

            string querys;
            //determine search string/ parameter

            if (txtSeachString.Text == "")
            {
                querys = String.Format("SELECT PaymentRefNumber, PaymentDate, DepositSlipNumber AS [Slip No], Amount, BankName, BranchName FROM tblCollectionReport where PaymentDate between '{0:yyyy-MM-dd}' and '{1:yyyy-MM-dd}' ", dtpfrom.Value.Date, dtpTo.Value.Date);

            }
            else
            {
                querys = String.Format("SELECT PaymentRefNumber, PaymentDate, DepositSlipNumber AS [Slip No], Amount, BankName, BranchName FROM tblCollectionReport where PaymentRefNumber LIKE '%{0}%'", txtSeachString.Text.Trim());
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
                else if (cboPayMode.Text == "")
                {
                    Common.setEmptyField(" Pay Mode ", Program.ApplicationName);
                    cboPayMode.Focus();
                    return;
                }
                else if (txtPayerID.Text == "")
                {
                    Common.setEmptyField("Payer ID", Program.ApplicationName);

                    txtPayerID.Focus();
                    return;
                }
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

                                using (SqlCommand sqlCommand1 = new SqlCommand(String.Format("INSERT INTO [tblCollectionReport]([Provider],[Channel],[PaymentRefNumber],[DepositSlipNumber],[PaymentDate],[PayerName],[PayerID],[RevenueCode],[Description],[Amount],[PaymentMethod],[BankCode],[BankName],[BranchCode],[BranchName],[AgencyName],[AgencyCode],[ZoneCode],[ZoneName],[ReceiptNo],[ReceiptDate],[State],[TelephoneNumber],[ChequeNumber],[ChequeValueDate],[ChequeBankCode],[ChequeBankName]) VALUES ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}','{19}','{20}','{21}','{22}','{23}','{24}','{25}','{26}');", "ICM", "Bank", txtPaymentRef.Text.Trim(), txtSlipNo.Text.Trim(), dtpDate.Value.Date.ToString("yyyy-MM-dd"), txtPayerID.Text.Trim(), txtPayerName.Text.Trim(), cboRevenue.SelectedValue, cboRevenue.Text.Trim(), cedDepositAmt.Text, cboPayMode.SelectedValue, cboBank.SelectedValue, cboBank.Text, cboPaidBranch.Text, cboPaidBranch.Text, Dts.Rows[0]["AgencyName"].ToString(), Dts.Rows[0]["AgencyCode"].ToString(), Dts.Rows[0]["ZoneID"].ToString(), Dts.Rows[0]["ZoneName"].ToString(), txtReceiptsNo.Text.Trim(), txtReceipDate.Value.Date.ToString("yyyy-MM-dd"), statename.ToString(), txtTelephone.Text.Trim(), txtChequeNo.Text.Trim(), dtpCheque.Value.Date.ToString("yyyy-MM-dd"), cboCheque.SelectedValue.ToString(), cboCheque.SelectedText.ToString()), db, transaction))

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
                        //setReload();
                        //setReload(Convert.ToInt32(cboAgency.SelectedValue.ToString()));
                        Common.setMessageBox("Record has been successfully added", Program.ApplicationName, 1);
                        clear();
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
                                //MessageBox.Show(MDIMain.stateCode);
                                //fieldid
                                using (SqlCommand sqlCommand1 = new SqlCommand(String.Format(String.Format("UPDATE [tblCollectionReport] SET [Provider]='{{0}}',[Channel]='{{1}}',[PaymentRefNumber]='{{2}}',[DepositSlipNumber]='{{3}}',[PaymentDate]='{{4}}',[PayerID]='{{5}}',[PayerName]='{{6}}',[RevenueCode]='{{7}}',[Description]='{{8}}',[Amount]='{{9}}',[PaymentMethod]='{{10}}',[BankCode]='{{11}}',[BankName]='{{12}}',[BranchCode]='{{13}}',[BranchName]='{{14}}',[AgencyName]='{{15}}',[AgencyCode]='{{16}}',[ZoneCode]='{{17}}',[ZoneName]='{{18}}',[ReceiptNo]='{{19}}',[ReceiptDate]='{{20}}',[State]='{{21}}',[TelephoneNumber]='{{22}}' ,[ChequeNumber] = '{{23}}',[ChequeValueDate]='{{24}}',[ChequeBankCode]='{{25}}',[ChequeBankName]='{{26}}' where  [PaymentRefNumber] ='{0}'", ID), txtPaymentRef.Text.Trim(), txtSlipNo.Text.Trim(), dtpDate.Value.Date.ToString("yyyy-MM-dd"), txtPayerName.Text.Trim(), txtPayerID.Text.Trim(), cboRevenue.SelectedValue, cboRevenue.Text.Trim(), cedDepositAmt.Text, cboPayMode.SelectedValue, cboBank.SelectedValue, cboBank.Text, cboPaidBranch.Text, cboPaidBranch.Text, Dts.Rows[0]["AgencyName"].ToString(), Dts.Rows[0]["AgencyCode"].ToString(), Dts.Rows[0]["ZoneID"].ToString(), Dts.Rows[0]["ZoneName"].ToString(), txtReceiptsNo.Text.Trim(), txtReceipDate.Value.Date.ToString("yyyy-MM-dd"), statename.ToString(), txtTelephone.Text.Trim(), txtChequeNo.Text.Trim(), dtpCheque.Value.Date.ToString("yyyy-MM-dd"), cboCheque.SelectedValue.ToString(), cboCheque.SelectedText.ToString()), db, transaction))

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

                        //////setReload();
                        //setReload(Convert.ToInt32(cboAgency.SelectedValue.ToString()));
                        Common.setMessageBox("Changes in record has been successfully saved.", Program.ApplicationName, 1);
                        //bttnReset.PerformClick();
                        clear();
                        iTransType = TransactionTypeCode.New;
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

            //load data from the table into the forms for edit
            string query = String.Format(String.Format("select * from tblCollectionReport where PaymentRefNumber ='{0}'", fieldid));
            DataTable dts = (new Logic()).getSqlStatement(query).Tables[0];


            if (dts != null)
            {
                bResponse = true;

                label11.Visible = true;

                txtPaymentRef.Visible = true;

                txtPaymentRef.Text = dts.Rows[0]["PaymentRefNumber"].ToString();
                txtSlipNo.Text = dts.Rows[0]["DepositSlipNumber"].ToString();
                dtpDate.Text = dts.Rows[0]["PaymentDate"].ToString();
                txtPayerID.Text = dts.Rows[0]["PayerID"].ToString();
                txtPayerName.Text = dts.Rows[0]["PayerName"].ToString();
                txtDepName.Text = dts.Rows[0]["PayerName"].ToString();
                cboRevenue.Text = dts.Rows[0]["Description"].ToString();
                cedDepositAmt.Text = dts.Rows[0]["Amount"].ToString();
                //cedAmount.Text = dts.Rows[0]["Amount"].ToString();
                cboPayMode.Text = dts.Rows[0]["PaymentMethod"].ToString();
                cboBank.Text = dts.Rows[0]["BankName"].ToString();
                cboPaidBranch.Text = dts.Rows[0]["BranchName"].ToString();
                txtReceiptsNo.Text = dts.Rows[0]["ReceiptNo"].ToString();
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

            setDBChequeBankComboBox();

            setDBComboBoxPay();

            setDBComboBoxRevenue();

            setDBComboBoxAgent();

            setDBComboBoxPerson();

            setGetOffAgency();

            isFirst = false;

            isSecond = false;

            isThird = false;

            isFourth = false;

            //populate year box
            //Methods.PopulateYear(cboYear);

            //populate months
            //Methods.PopulateMonth(cboMonth);

            //get default currency

            string query = String.Format("select CurrencyName  from tblCurrency where Flag =1");

            DataTable dts = (new Logic()).getSqlStatement(query).Tables[0];

            if (dts != null)
            {
                txtCurrency.Text = dts.Rows[0]["CurrencyName"].ToString();
            }

            //cboMonth.SelectedIndexChanged += cboMonth_SelectedIndexChanged;

            cboBank.SelectedIndexChanged += cboBank_SelectedIndexChanged;

            cboPaidBranch.SelectedIndexChanged += cboPaidBranch_SelectedIndexChanged;

            cboPayMode.SelectedIndexChanged += cboPayMode_SelectedIndexChanged;

            cboPayMode.KeyPress += cboPaymode_KeyPress;

            cboCheque.SelectedIndexChanged += cboCheque_SelectedIndexChanged;


            cboPaidBranch.KeyPress += cboPaidBranch_KeyPress;

            luePayeridPerson.EditValueChanged += luePayeridPerson_EditValueChanged;

            lupeAgent.EditValueChanged += lupeAgent_EditValueChanged;


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


                string query = "select *  from tblBank";

                using (SqlDataAdapter ada = new SqlDataAdapter(query, Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }

                Dt = ds.Tables[0];
            }

            Common.setComboList(cboBank, Dt, "BankShortCode", "BankName");


            cboBank.SelectedIndex = -1;


        }

        public void setDBChequeBankComboBox()
        {
            DataTable Dt;



            using (var ds = new System.Data.DataSet())
            {


                string query = "select *  from tblBank";

                using (SqlDataAdapter ada = new SqlDataAdapter(query, Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }

                Dt = ds.Tables[0];
            }

            Common.setComboList(cboCheque, Dt, "BankShortCode", "BankName");


            cboBank.SelectedIndex = -1;


        }

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
            cboRevenue.Text = string.Empty;
            txtPayerID.Clear();
            txtPayerName.Clear();
            txtReceiptsNo.Clear();
            //cedAmount.Text = string.Empty;
            cboPayMode.Text = string.Empty;
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


            Common.setComboList(cboPayMode, Dt, "PayID", "description");

            cboPayMode.SelectedIndex = -1;


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

            Common.setLookUpEdit(lupeAgent, Dt, "TaxAgentReferenceNumber", "OrganizationName");

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

            Common.setLookUpEdit(luePayeridPerson, Dt, "TaxAgentReferenceNumber", "fullname");

            cboBank.SelectedIndex = -1;


        }

        public void setDBComboBoxRevenue()
        {
            DataTable Dt;

            using (var ds = new System.Data.DataSet())
            {


                string query = "select *  from tblRevenueType";

                using (SqlDataAdapter ada = new SqlDataAdapter(query, Logic.ConnectionString))
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

            Methods.AutoComplete(cboPayMode, e, true);
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


                string query = String.Format("select BranchID ,BranchName from tblBankBranch where BankShortCode = '{0}'", parameter);

                using (SqlDataAdapter ada = new SqlDataAdapter(query, Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }

                Dt = ds.Tables[0];
            }

            Common.setComboList(cboPaidBranch, Dt, "BranchID", "BranchName");


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
                setDBComboBoxBranchAcct(Convert.ToInt32(cboPaidBranch.SelectedValue.ToString()));

            }
        }

        void cboPayMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboPayMode.SelectedValue != null && !isThird)
            {
                //getPayMode(Convert.ToInt32(cboPayMode.SelectedValue.ToString()));
                if (Convert.ToInt32(cboPayMode.SelectedValue.ToString()) == 1)
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

        public void setDBComboBoxBranchAcct(int parameter)
        {
            DataTable Dt;

            using (var ds = new System.Data.DataSet())
            {
                string query = String.Format("select AccountNumber from tblBankAccount where BranchID = '{0}'", parameter);
                using (SqlDataAdapter ada = new SqlDataAdapter(query, Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }

                Dt = ds.Tables[0];
            }

            txtActNo.Text = String.Format("{0}", Dt.Rows[0]["AccountNumber"]);

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



    }
}
