using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BankReconciliation.Class;
using DevExpress.Utils;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Selection;
using DevExpress.XtraScheduler.Native;
using DevExpress.XtraSplashScreen;
using TaxSmartSuite.Class;

namespace BankReconciliation.Forms
{
    public partial class FrmDelist : Form
    {
        public static FrmDelist publicStreetGroup;
        private bool Isbank = false; private bool isRecord = false; private SqlCommand _command; private SqlDataAdapter adp;

        private DataTable dtc;

        RepositoryItemGridLookUpEdit repComboLookBox = new RepositoryItemGridLookUpEdit();

        GridCheckMarksSelection selection; bool isFirstGrid = true; bool isSeconGrid = true; GridCheckMarksSelection selection2;

        GridColumn colView2 = new GridColumn();

        RepositoryItemGridLookUpEdit repComboLookBoxCredit = new RepositoryItemGridLookUpEdit();

        DataTable temTable = new DataTable();
        System.Data.DataSet dts;
        private System.Data.DataSet dts2;
        private DataSet dataSet;

        //private Dataset dt = new Dataset();
        public FrmDelist()
        {
            SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

            InitializeComponent();

            publicStreetGroup = this;

            setImages();

            ToolStripEvent();

            Load += OnFormLoad;

            cboBank.SelectedIndexChanged += cboBank_SelectedIndexChanged;

            btnSearch.Click += btnSearch_Click; btnGet.Click += btnGet_Click; btnUpdate.Click += btnUpdate_Click;

            cboBank.KeyPress += cboBank_KeyPress;

            OnFormLoad(null, null);

            temTable.Columns.Add("SN", typeof(int));
            temTable.Columns.Add("PaymentRefNumber", typeof(string));
            temTable.Columns.Add("ICMAPaymentRef", typeof(string));
            temTable.Columns.Add("UserId", typeof(string));

            SplashScreenManager.CloseForm(false);
        }

        void btnUpdate_Click(object sender, EventArgs e)
        {
            if (selection.SelectedCount == 0)
            {
                Common.setMessageBox("No Selection Made for Printing of Receipts", Program.ApplicationName, 3);
                return;

            }
            else if (selection2.SelectedCount == 0)
            {
                Common.setMessageBox("No Selection Made for Printing of Receipts", Program.ApplicationName, 3);
                return;
            }
            else if (selection.SelectedCount != selection.SelectedCount)
            {
                Common.setMessageBox("Number of Delist Transaction Record not equall", Program.ApplicationName, 3);
                return;
            }
            else
            {
                for (int i = 0; i < selection.SelectedCount; i++)
                {
                    for (int j = 0; j < selection2.SelectedCount; j++)
                    {
                        string _values = String.Format("'{0}'", (selection.GetSelectedRow(i) as DataRowView)["Amount"]);
                        string _values2 = string.Format("'{0}'", (selection2.GetSelectedRow(j) as DataRowView)["Amount"]);

                        if (_values.CompareTo(_values2) == 0)
                        {
                            ///add code here for true
                            temTable.Rows.Add(new object[] { j, String.Format("{0}", (selection2.GetSelectedRow(j) as DataRowView)["PaymentRefNumber"]), String.Format("{0}", (selection.GetSelectedRow(i) as DataRowView)["PaymentRefNumber"]), Program.UserID });
                        }
                        else
                        {
                            Common.setMessageBox("Selected Amount Not Equall", Program.ApplicationName, 3);
                            return;
                        }
                    }
                }

                if (temTable != null && temTable.Rows.Count > 0)
                {
                    dataSet = new DataSet("table");

                    dataSet.Tables.Add(temTable);

                    switch (Program.intCode)
                    {
                        case 13://Akwa Ibom state
                            using (var receiptAka = new CentralAkwa.CollectionManager())
                            {
                                dts = receiptAka.DelistUpdate(dataSet);
                            }

                            using (var tempState = new StateAkwa.CollectionManager())
                            {
                                dts2 = tempState.DelistUpdate(dataSet);
                            }
                            break;

                        case 20://Delta state
                            using (var tempCentral = new CentralDetla.CollectionManager())
                            {
                                dts = tempCentral.DelistUpdate(dataSet);
                            }
                            using (var tempState = new StateDelta.CollectionManager())
                            {
                                dts2 = tempState.DelistUpdate(dataSet);
                            }
                            break;

                        case 32://kogi state
                        //using (var receiptservic = new Kogireceiptservice.ReceiptService())
                        //{
                        //    dataSet = receiptservic.DownloadDataCentral(Program.stateCode);
                        //}
                        //break;

                        case 37://ogun state
                            using (var receiptsserv = new Centralogun.CollectionManager())
                            {
                                dts = receiptsserv.DelistUpdate(dataSet);
                            }

                            using (var tempState = new StateOgun.CollectionManager())
                            {
                                dts2 = tempState.DelistUpdate(dataSet);
                            }
                            break;
                        case 40://oyo state

                            using (var receiptsServices = new Centraloyo.CollectionManager())
                            {
                                dts = receiptsServices.DelistUpdate(dataSet);
                            }

                            using (var tempState = new StateOyo.CollectionManager())
                            {
                                dts2 = tempState.DelistUpdate(dataSet);
                            }
                            break;

                        default:
                            break;
                    }
                }
                //get return
                //dataSet2.Tables[0].Rows[0]["returnCode"].ToString() == "-1"
                if (dts.Tables[0].Rows[0]["returnCode"].ToString() == "00" && dts2.Tables[0].Rows[0]["returnCode"].ToString() == "00")
                {
                    //insert record local after delist from online
               
                    using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
                    {
                        //gridControl1.DataSource = null;
                        connect.Open();

                        _command = new SqlCommand("InsertDelist", connect) { CommandType = CommandType.StoredProcedure };
                        _command.Parameters.Add(new SqlParameter("@DelistTemp", SqlDbType.Structured)).Value = temTable;
                        _command.CommandTimeout = 0;

                        using (System.Data.DataSet ds = new System.Data.DataSet())
                        {
                            adp = new SqlDataAdapter(_command);
                            adp.Fill(ds, "ViewUnsweptTransaction");
                            //Dts = ds.Tables[0];
                            connect.Close();

                            if (ds.Tables[0].Rows[0]["returnCode"].ToString() == "00")
                            {
                                Common.setMessageBox(string.Format("{0}", dts.Tables[0].Rows[0]["returnMessage"]), Program.ApplicationName, 1);
                                gridControl1.DataSource = null;
                                gridControl2.DataSource = null;
                            }
                            else
                            {
                                Tripous.Sys.ErrorBox(String.Format("{0}{1}", ds.Tables[0].Rows[0]["returnCode"].ToString(), ds.Tables[0].Rows[0]["returnMessage"].ToString()));

                                return;
                            }

                        }
                    }

                }
                else
                {
                    Common.setMessageBox(string.Format("{0}...Error Occur During Delist Transaction", dts.Tables[0].Rows[0]["returnMessage"]), Program.ApplicationName, 3);
                    return;
                }
            }
        }

        void btnGet_Click(object sender, EventArgs e)
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

                dts = new System.Data.DataSet();

                dts.Clear();

                switch (Program.intCode)
                {
                    case 13://Akwa Ibom state
                        using (var receiptAka = new CentralAkwa.CollectionManager())
                        {
                            dts = receiptAka.DelistFetch(txtSearch.Text.Trim().ToString());
                        }

                        break;

                    case 20://Delta state
                        using (var receiptservic = new CentralDetla.CollectionManager())
                        {
                            dts = receiptservic.DelistFetch(txtSearch.Text.Trim().ToString());
                        }
                        break;

                    case 32://kogi state
                    //using (var receiptservic = new Kogireceiptservice.ReceiptService())
                    //{
                    //    dataSet = receiptservic.DownloadDataCentral(Program.stateCode);
                    //}
                    //break;

                    case 37://ogun state
                        using (var receiptsserv = new Centralogun.CollectionManager())
                        {
                            dts = receiptsserv.DelistFetch(txtSearch.Text.Trim().ToString());
                        }
                        break;
                    case 40://oyo state

                        using (var receiptsServices = new Centraloyo.CollectionManager())
                        {
                            dts = receiptsServices.DelistFetch(txtSearch.Text.Trim().ToString());
                        }
                        break;

                    default:
                        break;
                }
                if (dts.Tables[0].Rows[0]["returnCode"].ToString() == "00")
                {
                    if (dts.Tables[1] != null && dts.Tables[1].Rows.Count > 0)
                    {
                        gridControl2.DataSource = dts.Tables[1];

                        gridView2.Columns["Provider"].Visible = false; gridView2.Columns["Channel"].Visible = false;
                        gridView2.Columns["DepositSlipNumber"].Visible = false; gridView2.Columns["PaymentMethod"].Visible = false; gridView2.Columns["AgencyName"].Visible = false; gridView2.Columns["AgencyCode"].Visible = false; gridView2.Columns["BankCode"].Visible = false; gridView2.Columns["BankName"].Visible = false;
                        gridView2.Columns["BranchCode"].Visible = false; gridView2.Columns["BranchName"].Visible = false;
                        gridView2.Columns["ReceiptNo"].Visible = false; gridView2.Columns["ReceiptDate"].Visible = false; gridView2.Columns["TelephoneNumber"].Visible = false; gridView2.Columns["AmountWords"].Visible = false; gridView2.Columns["RevenueCode"].Visible = false; gridView2.Columns["RevenueName"].Visible = false; gridView2.Columns["ChequeNumber"].Visible = false; gridView2.Columns["ChequeValueDate"].Visible = false; gridView2.Columns["ChequeBankCodeListID"].Visible = false; gridView2.Columns["ChequeBankCodeName"].Visible = false;
                        gridView2.Columns["ChequeStatus"].Visible = false; gridView2.Columns["DateChequeReturned"].Visible = false; gridView2.Columns["TownName"].Visible = false; gridView2.Columns["PaymentMethod"].Visible = false; gridView2.Columns["PayerAddress"].Visible = false; gridView2.Columns["PayerName"].Visible = false; gridView2.Columns["PayerID"].Visible = false; gridView2.Columns["EReceipts"].Visible = false;
                        gridView2.Columns["EReceiptsDate"].Visible = false; gridView2.Columns["GeneratedBy"].Visible = false;
                        gridView2.Columns["ControlNumber"].Visible = false; gridView2.Columns["Status"].Visible = false;
                        gridView2.Columns["PrintedDate"].Visible = false; gridView2.Columns["PrintedBy"].Visible = false;
                        gridView2.Columns["StationCode"].Visible = false; gridView2.Columns["PayerAddress"].Visible = false;
                        gridView2.Columns["TownListID"].Visible = false; gridView2.Columns["ICMAeReceipts"].Visible = false;
                        gridView2.Columns["PostedBy"].Visible = false; gridView2.Columns["ReceiptDownloadStatus"].Visible = false;

                        gridView2.Columns["Amount"].DisplayFormat.FormatType = FormatType.Numeric;
                        gridView2.Columns["Amount"].DisplayFormat.FormatString = "n2";
                        gridView2.Columns["PaymentDate"].DisplayFormat.FormatType = FormatType.DateTime;
                        gridView2.Columns["PaymentDate"].DisplayFormat.FormatString = "dd/MM/yyyy";

                        if (isSeconGrid)
                        {
                            selection2 = new GridCheckMarksSelection(gridView2, ref lblSelect2);
                            selection2.CheckMarkColumn.VisibleIndex = 0;
                            isSeconGrid = false;
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Tripous.Sys.ErrorBox(ex.Message + ex.StackTrace.ToString(), Program.ApplicationName); return;
            }
            finally
            {
                SplashScreenManager.CloseForm(false);
            }

        }

        void btnSearch_Click(object sender, EventArgs e)
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
                else if (string.IsNullOrEmpty((string)(cboAccount.SelectedValue.ToString())))
                {
                    Common.setEmptyField("Account Number", Program.ApplicationName); cboAccount.Focus(); return;
                }
                else
                {
                    using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
                    {
                        gridControl1.DataSource = null;
                        connect.Open();

                        _command = new SqlCommand("DoDelistTrans", connect) { CommandType = CommandType.StoredProcedure };
                        _command.Parameters.Add(new SqlParameter("@bankcode", SqlDbType.VarChar)).Value = cboBank.SelectedValue;
                        _command.Parameters.Add(new SqlParameter("@startdate", SqlDbType.VarChar)).Value = string.Format("{0:yyyy/MM/dd 00:00:00}", dtpStart.Value);
                        _command.Parameters.Add(new SqlParameter("@enddate", SqlDbType.VarChar)).Value = string.Format("{0:yyyy/MM/dd 23:59:59}", dtpEnd.Value);
                        _command.Parameters.Add(new SqlParameter("@AccountId", SqlDbType.Int)).Value = Convert.ToInt32(cboAccount.SelectedValue);

                        _command.CommandTimeout = 0;

                        using (System.Data.DataSet ds = new System.Data.DataSet())
                        {
                            adp = new SqlDataAdapter(_command);
                            adp.Fill(ds, "ViewUnsweptTransaction");
                            //Dts = ds.Tables[0];
                            connect.Close();

                            if (ds.Tables[0].Rows[0]["returnCode"].ToString() == "00")
                            {
                                gridControl1.DataSource = ds.Tables[1];
                            }
                            else
                            {
                                Tripous.Sys.ErrorBox(String.Format("{0}{1}", ds.Tables[0].Rows[0]["returnCode"].ToString(), ds.Tables[0].Rows[0]["returnMessage"].ToString()));

                                return;
                            }

                            //AddCombDefinition();

                            gridView1.Columns["Amount"].DisplayFormat.FormatType = FormatType.Numeric;
                            gridView1.Columns["Amount"].DisplayFormat.FormatString = "n2";
                            gridView1.Columns["Date"].DisplayFormat.FormatType = FormatType.DateTime;
                            gridView1.Columns["Date"].DisplayFormat.FormatString = "dd/MM/yyyy";

                            if (isFirstGrid)
                            {
                                selection = new GridCheckMarksSelection(gridView1, ref lblSelect);
                                selection.CheckMarkColumn.VisibleIndex = 0;
                                isFirstGrid = false;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Tripous.Sys.ErrorBox(ex.Message + ex.StackTrace.ToString(), Program.ApplicationName); return;
            }
            finally
            {
                SplashScreenManager.CloseForm(false);
            }
        }

        void cboBank_KeyPress(object sender, KeyPressEventArgs e)
        {
            Methods.AutoComplete(cboBank, e, true);
        }

        void cboBank_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboBank.SelectedValue != null && !Isbank)
            {
                setDBComboBoxAcct();

            }
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
            ////bttnReset.Image = MDIMains.publicMDIParent.i32x32.Images[8];
            ////bttnUpdate.Image = MDIMain.publicMDIParent.i32x32.Images[7];
            //bttnUpdate.Image = MDIMains.publicMDIParent.i32x32.Images[7];
            //btnAllocate.Image = MDIMains.publicMDIParent.i32x32.Images[6];
            //bttncompare.Image = MDIMains.publicMDIParent.i32x32.Images[8];
            //bttnClose.Image = MDIMain.publicMDIParent.i32x32.Images[9];
            //bttnSave.Image = MDIMains.publicMDIParent.i32x32.Images[7];

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
                //iTransType = TransactionTypeCode.New;
                //ShowForm();
                //boolIsUpdate = false;
            }
            else if (sender == tsbEdit)
            {
                //groupControl2.Text = "Edit Record Mode";
                //iTransType = TransactionTypeCode.Edit;
                //if (EditRecordMode())
                //{
                //    ShowForm();
                //    boolIsUpdate = true;
                //}
            }
            else if (sender == tsbDelete)
            {
                //groupControl2.Text = "Disable Record Mode";
                //iTransType = TransactionTypeCode.Delete;
                //if (MosesClassLibrary.Utilities.Common.AskQuestion("Disable this record will delete attached record.\nDo you want to continue?", ""))
                //{
                //    if (string.IsNullOrEmpty(ID.ToString()))
                //    {
                //        Common.setMessageBox("No Record Selected for Disable", Program.ApplicationName, 3);
                //        return;
                //    }
                //    else
                //        //deleteRecord(ID);
                //}
                //else
                tsbReload.PerformClick();
                //boolIsUpdate = false;
            }
            else if (sender == tsbReload)
            {
                //iTransType = TransactionTypeCode.Reload; setReload();
                //ShowForm();
            }
            //bttnReset.PerformClick();
        }

        void OnFormLoad(object sender, EventArgs e)
        {
            setDBComboBox();

        }

        public void setDBComboBox()
        {
            DataTable Dt;

            //connect.connect.Close();

            using (var ds = new System.Data.DataSet())
            {
                //connect.connect.Open();
                using (SqlDataAdapter ada = new SqlDataAdapter((string)"SELECT BankShortCode,BankName FROM Collection.tblBank", Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }

                Dt = ds.Tables[0];
            }

            Common.setComboList(cboBank, Dt, "BankShortCode", "BankName");

            cboBank.SelectedIndex = -1;


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

    }
}
