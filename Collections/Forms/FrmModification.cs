using Collection.Classess;
using Collection.ReceiptServices;
using Collections;
using DevExpress.Utils;
using DevExpress.XtraSplashScreen;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using TaxSmartSuite.Class;

namespace Collection.Forms
{
    public partial class FrmModification : Form
    {
        public static FrmModification publicInstance;

        private DataTable dt = new DataTable();



        DataTable dtRev = new DataTable();

        System.Data.DataSet dsreturn = new System.Data.DataSet();

        private SqlDataAdapter adp;

        private SqlCommand command;

        string strReceipt, strControlnum;

        bool isFirst = true;

        System.Data.DataSet dstretval = new System.Data.DataSet();

        public FrmModification()
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

                InitializeComponent();

                publicInstance = this;

                Load += OnFormLoad;

                setImages();

                ToolStripEvent();

                OnFormLoad(null, null);

                dtRev.Columns.Add("SN", typeof(Int32));
                dtRev.Columns.Add("PaymentRef", typeof(string));
                dtRev.Columns.Add("EReceiptNumber", typeof(string));
                dtRev.Columns.Add("ReprintType", typeof(string));
                dtRev.Columns.Add("NewRevenueCode", typeof(string));
                dtRev.Columns.Add("NewRevenueName", typeof(string));
                dtRev.Columns.Add("NewAgencyCode", typeof(string));
                dtRev.Columns.Add("NewAgencyName", typeof(string));
                dtRev.Columns.Add("NewPayerName", typeof(string));
                dtRev.Columns.Add("NewPayerAddress", typeof(string));
                dtRev.Columns.Add("Reasonforrequest", typeof(string));
                dtRev.Columns.Add("RequestSentBY", typeof(string));
                //dtRev.Columns.Add("Amount", typeof(double));
            }
            finally
            {
                SplashScreenManager.CloseForm(false);
            }
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
            //btnlist.Image = MDIMain.publicMDIParent.i32x32.Images[6];

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

        public void RefreshForm()
        {
            NavBars.ToolStripEnableDisableControls(toolStrip, Tag as String);
        }

        void OnFormLoad(object sender, EventArgs e)
        {
            //ShowForm();
            isFirst = false;

            setDBComboBoxReveneu(); setDBComboBox();

            btnSearch.Click += btnSearch_Click;

            cboRevenue.KeyPress += cboRevenue_KeyPress;

            cboSelect.SelectedIndexChanged += CboSelect_SelectedIndexChanged;

            cboSelect.KeyPress += CboSelect_KeyPress;

            cboRevenue.SelectedIndexChanged += cboRevenue_SelectedIndexChanged;

            radioGroup3.SelectedIndexChanged += radioGroup3_SelectedIndexChanged;

            btnUpdate.Click += btnUpdate_Click;
        }

        private void CboSelect_KeyPress(object sender, KeyPressEventArgs e)
        {
            Methods.AutoComplete(cboSelect, e, true);
        }

        private void CboSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(cboSelect.SelectedValue.ToString())) return;

            //if (cboSelect.SelectedValue.ToString() == "3" || cboSelect.SelectedValue.ToString() == "5")
            //{
            //    Common.setMessageBox("This Operation Can't be Peformed Here......", Program.ApplicationName, 1);
            //    //cboSelect.SelectedIndex = -1;
            //    return;
            //}
            if (cboSelect.SelectedValue.ToString() == "1")//Payer Address Change
            {
                cboRevenue.Enabled = false;
                txtpayaddress.Enabled = true;
                txtpayername.Enabled = false;
                txtpayaddress.Focus();
            }
            else if (cboSelect.SelectedValue.ToString() == "2")//Payer Name Change
            {
                cboRevenue.Enabled = false;
                txtpayaddress.Enabled = false;
                txtpayername.Enabled = true;
                txtpayername.Focus();
            }
            else if (cboSelect.SelectedValue.ToString() == "4")//Revenue Change
            {
                cboRevenue.Enabled = true;
                txtpayaddress.Enabled = false;
                txtpayername.Enabled = false;
                cboRevenue.Focus();
            }


        }

        void btnUpdate_Click(object sender, EventArgs e)
        {
            int jk = 0;

            dtRev = null;

            if (string.IsNullOrEmpty(txtreason.Text))
            {
                Common.setEmptyField("Paymet Ref. Search", Program.ApplicationName); cboSelect.Focus();
                txtsearchpay.Focus(); return;
            }
            else if (string.IsNullOrEmpty(txtreason.Text))
            {
                Common.setMessageBox("Reason for Modification can't be empty...", Program.ApplicationName, 1);
                txtreason.Focus(); return;
            }
            else if (string.IsNullOrEmpty(cboSelect.SelectedValue.ToString()))
            {
                Common.setEmptyField("Modification Type...", Program.ApplicationName); cboSelect.Focus();
                return;
            }
            else if (string.IsNullOrWhiteSpace(dt.Rows[0]["EReceipts"].ToString()))
            {
                Common.setEmptyField("Receipt Number is Empty...", Program.ApplicationName);
                return;
            }
            else
            {
                try
                {
                    SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

                    if (!Logic.CheckReceiptPrinted(dt.Rows[0]["PaymentRefNumber"].ToString()))
                    {
                        insertcollectionrecepits(dt);
                        doprocessing(dt);
                    }
                    else
                    {
                        doprocessing(dt);

                    }
                }
                catch (Exception ex)
                {
                    Common.setMessageBox(string.Format("{0}----{1}..Error Occur While Sending Request Reprint of Receipt ", ex.Message, ex.StackTrace), "Reprint of Receipt ", 3);
                    return;
                }
                finally
                {
                    SplashScreenManager.CloseForm(false);
                }

            }
        }

        void radioGroup3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (radioGroup3.EditValue.ToString() == "1")//change Revenue
            {
                cboRevenue.Enabled = true;
                txtpayaddress.Enabled = false;
                txtpayername.Enabled = false;
                cboRevenue.Focus();
            }
            if (radioGroup3.EditValue.ToString() == "2")//change payer name
            {
                cboRevenue.Enabled = false;
                txtpayaddress.Enabled = false;
                txtpayername.Enabled = true;
                txtpayername.Focus();
            }
            if (radioGroup3.EditValue.ToString() == "3")//change payer Address
            {
                cboRevenue.Enabled = false;
                txtpayaddress.Enabled = true;
                txtpayername.Enabled = false;
                txtpayaddress.Focus();
            }
            if (radioGroup3.EditValue.ToString() == "4")//Change All Records
            {
                cboRevenue.Enabled = true;
                txtpayaddress.Enabled = true;
                txtpayername.Enabled = true;
                cboRevenue.Focus();
            }
        }

        void cboRevenue_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboRevenue.SelectedValue != null && !isFirst)
            {
                GetAgencyCodeByRevenue(cboRevenue.SelectedValue.ToString());
            }
        }

        void cboRevenue_KeyPress(object sender, KeyPressEventArgs e)
        {
            Methods.AutoComplete(cboRevenue, e, true);
        }

        void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

                string query = string.Empty;

                query = String.Format("SELECT   Provider, Channel, Collection.tblCollectionReport.PaymentRefNumber, DepositSlipNumber, PaymentDate, PayerID, PayerName, Amount, PaymentMethod, ChequeNumber,ChequeValueDate, ChequeStatus, DateChequeReturned, TelephoneNumber, ReceiptNo, ReceiptDate, PayerAddress,  RevenueCode,  Description, ChequeBankCode, ChequeBankName, AgencyName, AgencyCode, BankCode, BankName, BranchCode,BranchName, Receipt.tblCollectionReceipt.StationCode AS ZoneCode, ZoneName, AmountWords,tblCollectionReport.EReceipts,tblCollectionReport.EReceiptsDate, Receipt.tblCollectionReceipt.PrintedBY,Receipt.tblCollectionReceipt.IsPrintedDate,  Receipt.tblCollectionReceipt.ControlNumber, Receipt.tblCollectionReceipt.BatchNumber, Receipt.tblCollectionReceipt.isPrinted,Collection.tblCollectionReport.StationCode,ControlNumberBy,ControlNumberDate,BatchNumber FROM Collection.tblCollectionReport LEFT JOIN  Receipt.tblCollectionReceipt ON Collection.tblCollectionReport.PaymentRefNumber = Receipt.tblCollectionReceipt.PaymentRefNumber WHERE Collection.tblCollectionReport.PaymentRefNumber = '{0}' ORDER BY Collection.tblCollectionReport.StationCode , Collection.tblCollectionReport.AgencyCode ,Collection.tblCollectionReport.RevenueCode,Collection.tblCollectionReport.EReceipts", txtsearchpay.Text);


                using (var ds = new System.Data.DataSet())
                {
                    using (SqlDataAdapter ada = new SqlDataAdapter(query, Logic.ConnectionString))
                    {
                        ada.SelectCommand.CommandTimeout = 0;

                        ada.Fill(ds, "table");
                    }
                    dt = ds.Tables[0];
                }


                if (dt != null && dt.Rows.Count > 0)
                {
                    gridControl1.DataSource = dt;

                    layoutView1.Columns["Amount"].DisplayFormat.FormatType = FormatType.Numeric;
                    layoutView1.Columns["Amount"].DisplayFormat.FormatString = "n2";

                    cboRevenue.SelectedValue = dt.Rows[0]["RevenueCode"].ToString();
                    cboRevenue.Text = dt.Rows[0]["Description"].ToString();
                    txtpayername.Text = dt.Rows[0]["PayerName"].ToString();
                    txtpayaddress.Text = dt.Rows[0]["PayerAddress"].ToString();
                    strReceipt = dt.Rows[0]["EReceipts"].ToString();
                    strControlnum = dt.Rows[0]["ControlNumber"].ToString();

                }
                else
                    Common.setMessageBox("Record found or not Uploaded After Receipt Print", Program.ApplicationName, 1);
                return;
            }
            catch (Exception ex)
            {
                Tripous.Sys.ErrorBox(ex);
                return;
            }
            finally
            {
                SplashScreenManager.CloseForm(false);
            }

        }

        void setDBComboBoxReveneu()
        {
            DataTable Dt;

            using (var ds = new System.Data.DataSet())
            {
                using (SqlDataAdapter ada = new SqlDataAdapter((string)"SELECT RevenueCode,Description FROM Collection.tblRevenueType", Logic.ConnectionString))
                {
                    ada.SelectCommand.CommandTimeout = 0;

                    ada.Fill(ds, "table");
                }

                Dt = ds.Tables[0];
            }

            Common.setComboList(cboRevenue, Dt, "RevenueCode", "Description");

            cboRevenue.SelectedIndex = -1;
        }

        void GetAgencyCodeByRevenue(string parameters)
        {
            DataTable Dt;

            using (var ds = new System.Data.DataSet())
            {
                using (SqlDataAdapter ada = new SqlDataAdapter((string)string.Format("SELECT tblAgency.AgencyCode,tblAgency.AgencyName FROM Collection.tblRevenueType INNER JOIN Registration.tblAgency ON tblRevenueType.AgencyCode = tblAgency.AgencyCode WHERE Collection.tblRevenueType.RevenueCode ='{0}'", parameters), Logic.ConnectionString))
                {
                    ada.SelectCommand.CommandTimeout = 0;

                    ada.Fill(ds, "table");
                }

                Dt = ds.Tables[0];
            }

            if (Dt != null && Dt.Rows.Count > 0)
            {
                txtAgcode.Text = Dt.Rows[0]["AgencyCode"].ToString();
                label7.Text = Dt.Rows[0]["AgencyName"].ToString();
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
                    command.Parameters.Add(new SqlParameter("@tblCollectionReport_Temp", SqlDbType.Structured)).Value = dataSet.Tables[2];
                    command.CommandTimeout = 0;

                    adp = new SqlDataAdapter(command);
                    adp.Fill(ds);
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

        public void setDBComboBox()
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

                DataTable Dt;

                using (var ds = new System.Data.DataSet())
                {
                    using (SqlDataAdapter ada = new SqlDataAdapter((string)"SELECT Description,TransType FROM Receipt.tblReceiptTransaction WHERE FlagID=1 ORDER BY Description", Logic.ConnectionString))
                    {
                        ada.SelectCommand.CommandTimeout = 0;

                        ada.Fill(ds, "table");
                    }

                    Dt = ds.Tables[0];
                }

                Common.setComboList(cboSelect, Dt, "TransType", "Description");

                cboSelect.SelectedIndex = -1;
            }
            finally
            {
                SplashScreenManager.CloseForm(false);
            }
        }

        void insertcollectionrecepits(DataTable dts)
        {
            using (SqlConnection db = new SqlConnection(Logic.ConnectionString))
            {
                SqlTransaction transaction;

                db.Open();

                transaction = db.BeginTransaction();

                try
                {
                    string querys = string.Empty;


                    querys = string.Format(
                        "INSERT  INTO Receipt.tblCollectionReceipt   (PaymentRefNumber,EReceipts,EReceiptsDate,StationCode,isPrinted,PrintedBY) VALUES('{0}','{1}','{2}','{3}','{4}','{5}');",
                        dts.Rows[0]["PaymentRefNumber"], dts.Rows[0]["EReceipts"], string.Format("{0:yyyy/MM/dd hh:mm:ss}", dts.Rows[0]["EReceiptsDate"]), Program.stationCode, 1, Program.UserID);

                    using (SqlCommand sqlCommand2 = new SqlCommand(querys, db, transaction))
                    {
                        sqlCommand2.ExecuteNonQuery();
                    }
                    transaction.Commit();

                    db.Close();
                }
                catch (Exception ex)
                {
                    Tripous.Sys.ErrorBox(ex + "Inserting record to reprinted table");
                    transaction.Rollback();

                }
            }
        }

        void doprocessing(DataTable dt)
        {
            if (cboSelect.SelectedValue.ToString() == "1") //Payer Address Change
            {
                try
                {
                    switch (Program.intCode)
                    {
                        case 13: //Akwa Ibom state
                            using (var receiptAka = new AkwaIbomReceiptServices.ReceiptService())
                            {
                                dsreturn = receiptAka.LogReceiptsReprintRequest(strReceipt,
                                    dt.Rows[0]["PaymentRefNumber"].ToString(), strControlnum,
                                    Program.UserID, txtreason.Text, "ADD", null, null, null, null, null,
                                    txtpayaddress.Text.Trim().ToString(), Program.stationCode);
                            }
                            break;
                        case 20: //Delta state
                            using (var receiptDelta = new DeltaBir.ReceiptService())
                            {
                                dsreturn = receiptDelta.LogReceiptsReprintRequest(strReceipt,
                                    dt.Rows[0]["PaymentRefNumber"].ToString(), strControlnum,
                                    Program.UserID, txtreason.Text, "ADD", null, null, null, null, null,
                                    txtpayaddress.Text.Trim().ToString(), Program.stationCode);
                            }
                            break;
                        case 32: //kogi state
                            break;

                        case 37: //ogun state
                            using (var receiptsserv = new ReceiptService())
                            {
                                dsreturn = receiptsserv.LogReceiptsReprintRequest(strReceipt,
                                    dt.Rows[0]["PaymentRefNumber"].ToString(), strControlnum,
                                    Program.UserID, txtreason.Text, "ADD", null, null, null, null, null,
                                    txtpayaddress.Text.Trim().ToString(), Program.stationCode);
                            }
                            break;

                        case 40: //oyo state

                            using (var receiptsserv = new OyoReceiptServices.ReceiptService())
                            {
                                dsreturn = receiptsserv.LogReceiptsReprintRequest(strReceipt,
                                    dt.Rows[0]["PaymentRefNumber"].ToString(), strControlnum,
                                    Program.UserID, txtreason.Text, "ADD", null, null, null, null, null,
                                    txtpayaddress.Text.Trim().ToString(), Program.stationCode);
                            }

                            break;

                        default:
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Common.setMessageBox(
                        string.Format("{0}----{1}..Sending Request Reprint of Receipt Failed", ex.Message,
                            ex.StackTrace), Program.ApplicationName, 3);
                    return;
                }
            }
            else if (cboSelect.SelectedValue.ToString() == "2") //Payer Name Change
            {
                try
                {
                    switch (Program.intCode)
                    {
                        case 13: //Akwa Ibom state
                            using (var receiptAka = new AkwaIbomReceiptServices.ReceiptService())
                            {
                                dsreturn = receiptAka.LogReceiptsReprintRequest(strReceipt,
                                    dt.Rows[0]["PaymentRefNumber"].ToString(), strControlnum,
                                    Program.UserID, txtreason.Text, "NAME", null, null, null, null,
                                    txtpayername.Text, null, Program.stationCode);
                            }
                            break;
                        case 20: //Delta state
                            using (var receiptDelta = new DeltaBir.ReceiptService())
                            {
                                dsreturn = receiptDelta.LogReceiptsReprintRequest(strReceipt,
                                    dt.Rows[0]["PaymentRefNumber"].ToString(), strControlnum,
                                    Program.UserID, txtreason.Text, "NAME", null, null, null, null,
                                    txtpayername.Text, null, Program.stationCode);
                            }
                            break;
                        case 32: //kogi state
                            break;

                        case 37: //ogun state

                            using (var receiptsserv = new ReceiptService())
                            {
                                dsreturn = receiptsserv.LogReceiptsReprintRequest(strReceipt,
                                    dt.Rows[0]["PaymentRefNumber"].ToString(), strControlnum,
                                    Program.UserID, txtreason.Text, "NAME", null, null, null, null,
                                    txtpayername.Text, null, Program.stationCode);
                            }
                            break;

                        case 40: //oyo state

                            using (var receiptsserv = new OyoReceiptServices.ReceiptService())
                            {
                                dsreturn = receiptsserv.LogReceiptsReprintRequest(strReceipt,
                                    dt.Rows[0]["PaymentRefNumber"].ToString(), strControlnum,
                                    Program.UserID, txtreason.Text, "NAME", null, null, null, null,
                                    txtpayername.Text, null, Program.stationCode);
                            }

                            break;

                        default:
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Common.setMessageBox(
                        string.Format("{0}----{1}..Sending Request Reprint of Receipt Failed", ex.Message,
                            ex.StackTrace), Program.ApplicationName, 3);
                    return;
                }
            }
            else if (cboSelect.SelectedValue.ToString() == "4") //Revenue Change
            {
                try
                {
                    switch (Program.intCode)
                    {
                        case 13: //Akwa Ibom state
                            using (var receiptAka = new AkwaIbomReceiptServices.ReceiptService())
                            {
                                dsreturn = receiptAka.LogReceiptsReprintRequest(strReceipt,
                                    dt.Rows[0]["PaymentRefNumber"].ToString(), strControlnum,
                                    Program.UserID, txtreason.Text, "REV",
                                    cboRevenue.SelectedValue.ToString(), cboRevenue.Text, txtAgcode.Text,
                                    label7.Text, null, null, Program.stationCode);
                            }
                            break;
                        case 20: //Delta state
                            using (var receiptDelta = new DeltaBir.ReceiptService())
                            {
                                dsreturn = receiptDelta.LogReceiptsReprintRequest(strReceipt,
                                    dt.Rows[0]["PaymentRefNumber"].ToString(), strControlnum,
                                    Program.UserID, txtreason.Text, "REV",
                                    cboRevenue.SelectedValue.ToString(), cboRevenue.Text, txtAgcode.Text,
                                    label7.Text, null, null, Program.stationCode);
                            }
                            break;
                        case 32: //kogi state
                            break;

                        case 37: //ogun state
                            using (var receiptsserv = new ReceiptService())
                            {
                                dsreturn = receiptsserv.LogReceiptsReprintRequest(strReceipt,
                                    dt.Rows[0]["PaymentRefNumber"].ToString(), strControlnum,
                                    Program.UserID, txtreason.Text, "REV",
                                    cboRevenue.SelectedValue.ToString(), cboRevenue.Text, txtAgcode.Text,
                                    label7.Text, null, null, Program.stationCode);
                            }
                            break;

                        case 40: //oyo state
                            using (var receiptsserv = new OyoReceiptServices.ReceiptService())
                            {
                                dsreturn = receiptsserv.LogReceiptsReprintRequest(strReceipt,
                                    dt.Rows[0]["PaymentRefNumber"].ToString(), strControlnum,
                                    Program.UserID, txtreason.Text, "REV",
                                    cboRevenue.SelectedValue.ToString(), cboRevenue.Text, txtAgcode.Text,
                                    label7.Text, null, null, Program.stationCode);
                            }
                            break;

                        default:
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Common.setMessageBox(
                        string.Format("{0}----{1}..Sending Request Reprint of Receipt Failed", ex.Message,
                            ex.StackTrace), Program.ApplicationName, 3);
                    return;
                }
            }


            //return code from onliner base on the type sent
            if (dsreturn.Tables[0].Rows[0]["returnCode"].ToString() == "00")
            {
                //insert modife record
                using (SqlConnection db = new SqlConnection(Logic.ConnectionString))
                {
                    SqlTransaction transaction;

                    db.Open();

                    transaction = db.BeginTransaction();

                    try
                    {
                        string querys = string.Empty;

                        if (cboSelect.SelectedValue.ToString() == "1") //Payer Address Change
                        {
                            querys = string.Format(
                                "INSERT  INTO Receipt.tblReprintedReceipts    (PaymentRefNumber ,isPrinted ,IsPrintedDate ,PrintedBy ,ControlNumber ,ControlNumberBy ,ControlNumberDate ,BatchNumber ,RequestDate ,Description ,OldRecord ,NewRecord,TransType ) VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}',{12});",
                                dt.Rows[0]["PaymentRefNumber"], dt.Rows[0]["isPrinted"],
                                string.Format("{0:yyyy/MM/dd hh:mm:ss}", dt.Rows[0]["IsPrintedDate"]),
                                dt.Rows[0]["PrintedBY"], dt.Rows[0]["ControlNumber"],
                                dt.Rows[0]["ControlNumberBy"],
                                string.Format("{0:yyyy/MM/dd hh:mm:ss}", dt.Rows[0]["ControlNumberDate"]),
                                dt.Rows[0]["BatchNumber"],
                                string.Format("{0:yyyy/MM/dd hh:mm:ss}", DateTime.Now),
                                txtreason.Text.Trim(), dt.Rows[0]["PayerAddress"],
                                txtpayaddress.Text.Trim().ToString(), cboSelect.SelectedValue);
                        }
                        else if (cboSelect.SelectedValue.ToString() == "2") //Payer Name Change
                        {
                            querys = string.Format(
                                "INSERT  INTO Receipt.tblReprintedReceipts    (PaymentRefNumber ,isPrinted ,IsPrintedDate ,PrintedBy ,ControlNumber ,ControlNumberBy ,ControlNumberDate ,BatchNumber ,RequestDate ,Description ,OldRecord ,NewRecord,TransType ) VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}',{12});",
                                dt.Rows[0]["PaymentRefNumber"], dt.Rows[0]["isPrinted"],
                                string.Format("{0:yyyy/MM/dd hh:mm:ss}", dt.Rows[0]["IsPrintedDate"]),
                                dt.Rows[0]["PrintedBY"], dt.Rows[0]["ControlNumber"],
                                dt.Rows[0]["ControlNumberBy"],
                                string.Format("{0:yyyy/MM/dd hh:mm:ss}", dt.Rows[0]["ControlNumberDate"]),
                                dt.Rows[0]["BatchNumber"],
                                string.Format("{0:yyyy/MM/dd hh:mm:ss}", DateTime.Now),
                                txtreason.Text.Trim(), dt.Rows[0]["PayerName"],
                                txtpayername.Text.Trim().ToString(), cboSelect.SelectedValue);
                        }
                        else if (cboSelect.SelectedValue.ToString() == "4") //Revenue Change
                        {
                            querys = string.Format(
                                "INSERT  INTO Receipt.tblReprintedReceipts    (PaymentRefNumber ,isPrinted ,IsPrintedDate ,PrintedBy ,ControlNumber ,ControlNumberBy ,ControlNumberDate ,BatchNumber ,RequestDate ,Description ,OldRecord ,NewRecord,TransType ) VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}',{12});",
                                dt.Rows[0]["PaymentRefNumber"], dt.Rows[0]["isPrinted"],
                                string.Format("{0:yyyy/MM/dd hh:mm:ss}", dt.Rows[0]["IsPrintedDate"]),
                                dt.Rows[0]["PrintedBY"], dt.Rows[0]["ControlNumber"],
                                dt.Rows[0]["ControlNumberBy"],
                                string.Format("{0:yyyy/MM/dd hh:mm:ss}", dt.Rows[0]["ControlNumberDate"]),
                                dt.Rows[0]["BatchNumber"],
                                string.Format("{0:yyyy/MM/dd hh:mm:ss}", DateTime.Now),
                                txtreason.Text.Trim(), dt.Rows[0]["RevenueCode"], cboRevenue.SelectedValue,
                                cboSelect.SelectedValue);
                        }


                        using (SqlCommand sqlCommand2 = new SqlCommand(querys, db, transaction))
                        {
                            sqlCommand2.ExecuteNonQuery();
                        }
                        transaction.Commit();

                        db.Close();
                    }
                    catch (Exception ex)
                    {
                        Tripous.Sys.ErrorBox(ex + "Inserting record to reprinted table");
                        transaction.Rollback();
                        return;
                    }
                }

                Common.setMessageBox(
                    "Request Sent for Approval. Click on Get Approved for records approved by Admin personal..",
                    Program.ApplicationName, 1);
                return;
            }
            else
            {
                Common.setMessageBox(dsreturn.Tables[0].Rows[0]["returnMessage"].ToString(),
                    Program.ApplicationName, 1);
                return;
            }
        }
    }
}
