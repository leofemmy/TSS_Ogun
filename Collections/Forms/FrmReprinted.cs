using Collection.Classes;
using Collection.Classess;
using Collection.ReceiptServices;
using Collection.Report;
using Collections;
using DevExpress.Utils;
using DevExpress.XtraGrid.Selection;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraReports.UI;
using DevExpress.XtraSplashScreen;
using LinqToExcel;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using TaxSmartSuite.Class;

namespace Collection.Forms
{
    public partial class FrmReprinted : Form
    {
        private DataTable dt;

        //private DataTable dtsed;

        public static FrmReprinted publicStreetGroup;

        AmountToWords amounttowords = new AmountToWords();

        protected TransactionTypeCode iTransType;

        public static FrmReprinted publicInstance;

        protected bool boolIsUpdate;

        private System.Data.DataSet ds;

        private SqlDataAdapter adp;

        private SqlCommand command;

        string criteria, strreprint;

        string BatchNumber, values, values1, values2, query, criteria3, criteria2;

        GridCheckMarksSelection selection;

        System.Data.DataSet dstretval = new System.Data.DataSet();

        object[] Split;
        object[] Split2;
        object[] Split3;

        bool isReprint = true;

        private SqlCommand _command; DataTable temTable = new DataTable();


        SqlDataAdapter ada;

        private string strCollectionReportID;

        DataTable dtsed = new DataTable();

        DataTable tableTrans = new DataTable();

        DataTable dts = new DataTable();

        System.Data.DataSet dsc = new System.Data.DataSet();

        System.Data.DataSet dsreturn = new System.Data.DataSet();

        System.Data.DataSet dsGetapp = new System.Data.DataSet();

        System.Data.DataSet dsGetappup = new System.Data.DataSet();

        int countrece, countpay, countcontrol; string filenamesopen = String.Empty;

        bool isFirstGrid = true; ExcelQueryFactory excel = null;

        string sheetName = "Sheet1";
        DataTable dtpay;

        public FrmReprinted()
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

                InitializeComponent();

                publicInstance = this;

                publicStreetGroup = this;

                iTransType = TransactionTypeCode.New;

                setImages();

                ToolStripEvent();

                Load += OnFormLoad;

                btnSearch.Click += btnSelect_Click;

                btnConfirm.Click += btnConfirm_Click;

                btnPrint.Click += btnPrint_Click;

                btnMain.Click += btnMain_Click;

                btnGet.Click += btnGet_Click; btnupload.Click += Btnupload_Click;

                btnDelete.Click += btnDelete_Click;

                //gridView3.CustomDrawCell += gridView3_CustomDrawCell;
                gridView3.CustomColumnDisplayText += gridView3_CustomColumnDisplayText;

                temTable.Columns.Add("PaymentRefNumber", typeof(string));

                tableTrans.Columns.Add("SN", typeof(Int32));
                tableTrans.Columns.Add("PaymentRef", typeof(string));
                tableTrans.Columns.Add("EReceiptNumber", typeof(string));
                tableTrans.Columns.Add("ReprintType", typeof(string));
                tableTrans.Columns.Add("NewRevenueCode", typeof(string));
                tableTrans.Columns.Add("NewRevenueName", typeof(string));
                tableTrans.Columns.Add("NewAgencyCode", typeof(string));
                tableTrans.Columns.Add("NewAgencyName", typeof(string));
                tableTrans.Columns.Add("NewPayerName", typeof(string));
                tableTrans.Columns.Add("NewPayerAddress", typeof(string));
                tableTrans.Columns.Add("Reasonforrequest", typeof(string));
                tableTrans.Columns.Add("RequestSentBY", typeof(string));

                dtsed.Columns.Add("SN", typeof(int));
                dtsed.Columns.Add("Payment Ref", typeof(string));
                dtsed.Columns.Add("Receipt Number", typeof(string));

                gridControl2.DataSource = dtsed;
                gridView3.Columns["SN"].OptionsColumn.AllowEdit = false;
                gridView3.Columns["SN"].Width = 20;
                gridView3.Columns["Payment Ref"].Width = 200;
                gridView3.Columns["Receipt Number"].Width = 100;
                //gridView3.BestFitColumns();
                gridControl2.ForceInitialize();

                OnFormLoad(null, null);
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

                dtpay = new DataTable();

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

                            int cg = 0;

                            //dtpay.Clear();                            //dtpay.Clear();
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
                                    dtpay.Clear();

                                    dtpay = getData.ToDataTable();
                                    //passs infor to gridview
                                    values = String.Empty; values1 = String.Empty;

                                    if (dtpay.Rows.Count > 0)
                                    {
                                        DataColumn col = dtpay.Columns["PaymentRefNumber"];

                                        foreach (DataRow row in dtpay.Rows)
                                        {
                                            //strJsonData = row[col].ToString();

                                            values += String.Format("{0}", String.Format("{0}", row[col]));

                                            if (cg + 1 < dtpay.Rows.Count)

                                                values += ","; values1 += " ";
                                            ++cg;

                                        }

                                        try
                                        {
                                            using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
                                            {
                                                connect.Open();
                                                _command = new SqlCommand("doReprintedReceipts", connect) { CommandType = CommandType.StoredProcedure };
                                                _command.Parameters.Add(new SqlParameter("@PaymentRefNumber", SqlDbType.VarChar)).Value = values;
                                                _command.Parameters.Add(new SqlParameter("@EReceipts", SqlDbType.VarChar)).Value = values1;
                                                _command.CommandTimeout = 0;
                                                using (System.Data.DataSet ds = new System.Data.DataSet())
                                                {
                                                    ds.Clear();
                                                    adp = new SqlDataAdapter(_command);
                                                    adp.Fill(ds);
                                                    connect.Close();

                                                    if (ds.Tables[0].Rows[0]["returnCode"].ToString() != "00")
                                                    {
                                                        Common.setMessageBox(ds.Tables[0].Rows[0]["returnMessage"].ToString(), Program.ApplicationName, 2);

                                                        //Common.setMessageBox("Either the record is not printed or not Uploaded. Do Upload pending printed receipts", groupBox1.Text.Trim().ToString(), 1);

                                                        return;
                                                    }
                                                    else
                                                    {
                                                        label10.Text = String.Format("{0} Record(s) Imported for Reprinting", ds.Tables[1].Rows.Count);

                                                        dt = ds.Tables[1];
                                                        gridControl1.DataSource = dt;

                                                        gridView2.BestFitColumns();
                                                        layoutView1.Columns["Amount"].DisplayFormat.FormatType = FormatType.Numeric;
                                                        //layoutView1.Columns["Amount"].
                                                        layoutView1.Columns["Amount"].DisplayFormat.FormatString = "n2";

                                                        gridControl3.DataSource = dt;
                                                        gridView5.Columns["Amount"].DisplayFormat.FormatType = FormatType.Numeric;
                                                        gridView5.Columns["Amount"].DisplayFormat.FormatString = "n2";

                                                        gridView5.Columns["PaymentDate"].Visible = false;
                                                        gridView5.Columns["AgencyName"].Visible = false;
                                                        gridView5.Columns["Description"].Visible = false;
                                                        gridView5.Columns["PrintedBY"].Visible = false;
                                                        gridView5.Columns["IsPrintedDate"].Visible = false;
                                                        gridView5.Columns["ControlNumber"].Visible = false;
                                                        gridView5.Columns["ControlNumberBy"].Visible = false;
                                                        gridView5.Columns["ControlNumberDate"].Visible = false;
                                                        gridView5.Columns["isPrinted"].Visible = false;
                                                        gridView5.Columns["BatchNumber"].Visible = false;
                                                        gridView5.BestFitColumns();
                                                    }

                                                }
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            Common.setMessageBox(ex.Message, Program.ApplicationName, 3);
                                            txtPay.Text = string.Empty; txtSearch.Text = string.Empty;
                                            return;
                                        }
                                    }


                                    //gridControl2.DataSource = dtpay;
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

        void gridView3_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            DevExpress.XtraGrid.Columns.GridColumn Col = e.Column;
            if (Col.Name == "colSN")
            { // without " before and after yourColName
                if (e.ListSourceRowIndex >= 0)
                {
                    e.DisplayText = (e.ListSourceRowIndex + 1).ToString();
                }
            }
        }

        void gridView3_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            DevExpress.XtraGrid.Columns.GridColumn Col = e.Column;
            if (Col.Name == "colSN")
            {
                e.DisplayText = e.RowHandle.ToString();
            }

            //e.DisplayText = e.RowHandle.ToString();
        }

        void btnDelete_Click(object sender, EventArgs e)
        {
            GridView view = gridView5;

            if (view == null || view.SelectedRowsCount == 0)
            {
                Common.setMessageBox("No Recorc Selecte to be Delete", "Search Record", 1); return;
            }
            else
            {
                view.DeleteSelectedRows();
                gridControl3.RefreshDataSource();
                //btnSelect_Click(nukll, null);
            }

            //var row = view.GetFocusedDataRow();




            //DataRow[] rows = new DataRow[view.SelectedRowsCount];

            //for (int i = 0; i < view.SelectedRowsCount; i++)

            //    rows[i] = view.GetDataRow(view.GetSelectedRows()[i]);

            //view.BeginSort();

            //try
            //{

            //    foreach (DataRow row in rows)

            //        row.Delete();

            //}

            //finally
            //{

            //    view.EndSort();

            //}
        }

        void btnGet_Click(object sender, EventArgs e)
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

                switch (Program.intCode)
                {
                    case 13://Akwa Ibom state
                        using (var receiptAka = new AkwaIbomReceiptServices.ReceiptService())
                        {
                            dsGetapp = receiptAka.GetReceiptReprintApproved(Program.stationCode);
                        }

                        break;
                    case 32://kogi state
                        break;

                    case 37://ogun state
                        using (var receiptsserv = new ReceiptService())
                        {
                            dsGetapp = receiptsserv.GetReceiptReprintApproved(Program.stationCode);
                        }
                        break;

                    case 40://oyo state
                        using (var receiptsserv = new OyoReceiptServices.ReceiptService())
                        {
                            dsGetapp = receiptsserv.GetReceiptReprintApproved(Program.stationCode);
                        }
                        break;

                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                Common.setMessageBox(ex.StackTrace, "Get Approval Record", 3); return;
            }
            finally
            {
                SplashScreenManager.CloseForm(false);
            }


            if (dsGetapp.Tables[0].Rows[0]["returncode"].ToString() == "-1")
            {
                Common.setMessageBox(dsGetapp.Tables[0].Rows[0]["returnmessage"].ToString(), Program.ApplicationName, 1);
                return;
            }
            else
            {
                if (dsGetapp.Tables.Count == 0 || dsGetapp.Tables[0].Rows.Count < 1)
                {
                    Common.setMessageBox("No records have been approval yet", "Get Approval Record", 1);
                    return;
                }
                else
                {
                    //insert record into collection
                    dstretval = InsertData(dsGetapp);

                    try
                    {
                        SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

                        //send return data online for update
                        switch (Program.intCode)
                        {
                            case 13://Akwa Ibom state
                                using (var receiptAka = new AkwaIbomReceiptServices.ReceiptService())
                                {
                                    dsGetappup = receiptAka.UpdateReceiptReprintApproved(dstretval);
                                }

                                break;
                            case 32://kogi state
                                break;

                            case 37://ogun state
                                using (var receiptsserv = new ReceiptService())
                                {
                                    dsGetappup = receiptsserv.UpdateReceiptReprintApproved(dstretval);
                                }
                                break;

                            case 40://oyo state
                                using (var receiptsserv = new OyoReceiptServices.ReceiptService())
                                {
                                    dsGetappup = receiptsserv.UpdateReceiptReprintApproved(dstretval);
                                }
                                break;

                            default:
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        Common.setMessageBox(ex.StackTrace, "Get Approval Record", 3); return;
                    }

                    finally
                    {
                        SplashScreenManager.CloseForm(false);
                    }

                    if (dsGetappup.Tables[0].Rows[0]["returnCode"].ToString() == "00")
                    {
                        iTransType = TransactionTypeCode.Edit;
                        voidgetRec(dstretval);
                        OnFormLoad(null, null); return;
                    }
                    else
                    {
                        Common.setMessageBox(string.Format("Error Updating Approval Record, {0}...", dsGetappup.Tables[0].Rows[0]["returnMessage"]), Program.ApplicationName, 3);
                        return;
                    }

                }
            }


            #region old

            //if (dsGetapp.Tables[0].Rows[0]["returnmessage"].ToString() == "-1")
            //{
            //    Common.setMessageBox(dsGetapp.Tables[0].Rows[0]["returnmessage1"].ToString(), Program.ApplicationName, 1);
            //    return;
            //}
            //else
            //{
            //    if (dsGetapp.Tables.Count == 0 || dsGetapp.Tables[0].Rows.Count < 1)
            //    {
            //        Common.setMessageBox("No records have been approval yet", "Get Approval Record", 1);
            //        return;
            //    }
            //    else
            //    {
            //        using (WaitDialogForm form = new WaitDialogForm("Application Working...,Please Wait...", "Getting Approval Records"))
            //        {
            //            //insert record into collection
            //            dstretval = InsertData(dsGetapp);

            //            //send return data online for update
            //            using (var receiptsserv = new ReceiptService())
            //            {
            //                dsGetappup = receiptsserv.UpdateReceiptReprintApproved(dstretval);
            //            }

            //            if (dsGetappup.Tables[0].Rows[0]["returnCode"].ToString() == "00")
            //            {
            //                iTransType = TransactionTypeCode.Edit;
            //                voidgetRec(dstretval);
            //                OnFormLoad(null, null); return;
            //            }
            //            else
            //            {
            //                Common.setMessageBox(string.Format("Error Updating Approval Record, {0}...", dsGetappup.Tables[0].Rows[0]["returnMessage"]), Program.ApplicationName, 3);
            //                return;
            //            }
            //        }
            //    }
            //}
            #endregion
        }

        void btnMain_Click(object sender, EventArgs e)
        {
            using (FrmMainFest frmMainFest = new FrmMainFest(strCollectionReportID, values, criteria2, false) { IDList = strCollectionReportID })
            {
                frmMainFest.ShowDialog();
            }

            gridControl4.Enabled = false;

        }

        void btnPrint_Click(object sender, EventArgs e)
        {
            if (selection.SelectedCount == 0)
            {

                Common.setMessageBox("No Selection Made for Printing of Receipts", Program.ApplicationName, 3);
                return;

            }
            else
            {
                GetPayRef();


                ////ask if the print was sucessfull
                DialogResult result = MessageBox.Show("Is Receipt Printing Successful ?", Program.ApplicationName, MessageBoxButtons.YesNo);

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
                                string query1 = String.Format("UPDATE Receipt.tblCollectionReceipt SET isPrinted=1,IsPrintedDate= '{0:MM/dd/yyyy HH:mm:ss tt}',PrintedBY='{1}',StationCode='{2}' WHERE PaymentRefNumber IN ({3})", DateTime.Now, Program.UserID, Program.stationCode, values);

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

                                string query = String.Format("DELETE  FROM Receipt.tblReceipt where SentBy='{0}')", Program.UserID);

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

        //string GetPayRef()
        //{
        //    string values = string.Empty;

        //    lblSelect.Text = string.Empty;

        //    int j = 0;


        //    for (int i = 0; i < selection.SelectedCount; i++)
        //    {
        //        //string lol = ((selection.GetSelectedRow(i) as DataRowView)["PaymentRefNumber"].ToString());

        //        values += String.Format("'{0}'", (selection.GetSelectedRow(i) as DataRowView)["PaymentRefNumber"]);
        //        if (j + 1 < selection.SelectedCount)
        //            values += ",";
        //        ++j;

        //    }



        //    return values;
        //}

        void GetPayRef()
        {
            string values = string.Empty;

            lblSelect.Text = string.Empty;

            temTable.Columns.Add("EReceipt", typeof(string));
            temTable.Columns.Add("PaymentRefNumber", typeof(string));
            temTable.Columns.Add("UserId", typeof(string));

            int j = 0;

            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

                if (!isReprint)
                {
                    //using (WaitDialogForm form = new WaitDialogForm("Application Working...,Please Wait...", "Processing Your Request"))
                    //{
                    temTable.Clear();

                    for (int i = 0; i < selection.SelectedCount; i++)
                    {
                        temTable.Rows.Add(new object[] { String.Format("{0}", (selection.GetSelectedRow(i) as DataRowView)["EReceipts"]), String.Format("{0}", (selection.GetSelectedRow(i) as DataRowView)["PaymentRefNumber"]), Program.UserID });

                        values += String.Format("'{0}'", (selection.GetSelectedRow(i) as DataRowView)["PaymentRefNumber"]);

                        if (j + 1 < selection.SelectedCount)
                            values += ",";
                        ++j;
                    }

                    using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
                    {
                        connect.Open();
                        _command = new SqlCommand("InserttblReceipt", connect) { CommandType = CommandType.StoredProcedure };
                        _command.Parameters.Add(new SqlParameter("@tblCollectionReport_Temp", SqlDbType.Structured)).Value = temTable;
                        _command.Parameters.Add(new SqlParameter("@type", SqlDbType.VarChar)).Value = "Edit";
                        _command.Parameters.Add(new SqlParameter("@userid", SqlDbType.VarChar)).Value = Program.UserID;
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

                                        string strFormat = null;

                                        query = string.Format("SELECT [ID] , [Provider] , [Channel] ,[PaymentRefNumber] , [DepositSlipNumber] , CONVERT(VARCHAR,CONVERT(DATE,[PaymentDate])) AS PaymentDate,[PayerID] , UPPER([PayerName]) AS [PayerName], [Amount] ,[PaymentMethod] ,[ChequeNumber] ,[ChequeValueDate] ,[ChequeStatus] ,[DateChequeReturned] ,[TelephoneNumber] ,[ReceiptNo] ,[ReceiptDate] ,UPPER([PayerAddress]) as [PayerAddress] ,[User] ,[RevenueCode] , tblCollectionReport.Description , [ChequeBankCode] ,[ChequeBankName] ,[AgencyName] ,[AgencyCode] ,[BankCode] ,[BankName] ,[BranchCode] ,[BranchName] ,[ZoneCode] ,[ZoneName] ,[Username] ,[AmountWords] ,[EReceipts] ,[EReceiptsDate] ,[GeneratedBy] ,[StationCode] ,(SELECT StationName FROM Receipt.tblStation  WHERE tblStation.StationCode = Collection.tblCollectionReport.[StationCode]) AS StationName, Symbol , Surfix , tblCurrency.Description AS prefix from Collection.tblCollectionReport INNER JOIN Reconciliation.tblCurrency ON tblCurrency.CurrencyCode = tblCollectionReport.CurrencyCode WHERE PaymentRefNumber IN ({0})  ORDER BY Collection.tblCollectionReport.StationCode , Collection.tblCollectionReport.AgencyCode ,Collection.tblCollectionReport.RevenueCode,Collection.tblCollectionReport.EReceipts", values);

                                        DataTable Dt = dds.Tables.Add("CollectionReportTable");
                                        ada = new SqlDataAdapter(query, Logic.ConnectionString);
                                        ada.Fill(dds, "CollectionReportTable");
                                        Logic.ProcessDataTable(Dt); ;
                                        //strCollectionReportID = strFormat;



                                        //XRepReceipt recportRec = new XRepReceipt { DataSource = dds.Tables[0] /*recportRec.DataAdapter = ada;*/, DataMember = "CollectionReportTable" };

                                        //recportRec.ShowPreviewDialog();

                                        switch (Program.intCode)
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

                                query = string.Format("SELECT [ID] , [Provider] , [Channel] ,[PaymentRefNumber] , [DepositSlipNumber] ,  CONVERT(VARCHAR,CONVERT(DATE,[PaymentDate])) AS PaymentDate,[PayerID] , UPPER([PayerName]) AS [PayerName], [Amount] ,[PaymentMethod] ,[ChequeNumber] ,[ChequeValueDate] ,[ChequeStatus] ,[DateChequeReturned] ,[TelephoneNumber] ,[ReceiptNo] ,[ReceiptDate] ,UPPER([PayerAddress]) as [PayerAddress] ,[User] ,[RevenueCode] , tblCollectionReport.Description , [ChequeBankCode] ,[ChequeBankName] ,[AgencyName] ,[AgencyCode] ,[BankCode] ,[BankName] ,[BranchCode] ,[BranchName] ,[ZoneCode] ,[ZoneName] ,[Username] ,[AmountWords] ,[EReceipts] ,[EReceiptsDate] ,[GeneratedBy] ,[StationCode] ,(SELECT TOP 1 StationName FROM Receipt.tblStation  WHERE tblStation.StationCode = Collection.tblCollectionReport.[StationCode]) AS StationName, Symbol , Surfix , tblCurrency.Description AS prefix from Collection.tblCollectionReport INNER JOIN Reconciliation.tblCurrency ON tblCurrency.CurrencyCode = tblCollectionReport.CurrencyCode WHERE PaymentRefNumber IN ({0})ORDER BY Collection.tblCollectionReport.EReceipts", values);

                                DataTable Dt = dds.Tables.Add("CollectionReportTable");
                                ada = new SqlDataAdapter(query, Logic.ConnectionString);
                                ada.Fill(dds, "CollectionReportTable");
                                Logic.ProcessDataTable(Dt); ;
                                //strCollectionReportID = strFormat;
                            }


                            //XRepReceipt recportRec = new XRepReceipt { DataSource = dds.Tables[0] /*recportRec.DataAdapter = ada;*/, DataMember = "CollectionReportTable" };

                            //recportRec.ShowPreviewDialog();

                            //selection.ClearSelection(); dds.Clear();
                            switch (Program.intCode)
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

                                //case 32://kogi state

                                //    XRepReceiptkogi recportRecko = new XRepReceiptkogi { DataSource = dds.Tables[0] /*recportRec.DataAdapter = ada;*/, DataMember = "CollectionReportTable" };
                                //    recportRecko.ShowPreviewDialog();

                                //    break;

                                default:
                                    break;
                            }

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

                        if (item["PayerID"].ToString().Length > 14)
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

                        item["Username"] = string.Format(@"<< Reprint of  receipt with control number {0}  at {1} printing station >>", item["ControlNumber"], item["StationName"]);

                        strreprint = string.Format(" Reprint of {0} Receipt", item["ControlNumber"]);

                        item["PaymentDate"] = Convert.ToDateTime(item["PaymentDate"]).ToString("dd-MMM-yyyy");
                    }
                    catch
                    {

                    }
                }
            }
        }

        void btnConfirm_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txtReason.Text))
            {
                Common.setEmptyField("Reason", Program.ApplicationName);
                txtReason.Focus(); return;
            }
            //else if (string.IsNullOrEmpty(txtPay.Text))
            //{
            //    Common.setEmptyField("Payment Ref. Number", Program.ApplicationName);
            //    txtPay.Focus(); return;
            //}
            //else if (countrece != countpay)
            //{
            //    Common.setMessageBox("Number of Receipt specified is different from number of Payment Ref", Program.ApplicationName, 2);
            //    return;
            //}
            else
            {
                if (Program.stateCode == "20")
                {
                    try
                    {
                        SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

                        for (int i = 0; i < gridView5.RowCount; i++)
                        {
                            //values += String.Format("{0}", obj.ToString().Trim());
                            var row = gridView5.GetDataRow(i);

                            if (row != null)
                            {
                                temTable.Rows.Add(new object[] { row["PaymentRefNumber"] });
                            }
                        }

                        //DataTable Dt; DataTable dtscol;
                        ////do check on collectionreceip first befor reprint table is checkec
                        //string SQL1 = String.Format(@"SELECT PaymentRefNumber FROM Receipt.tblCollectionReceipt WHERE PaymentRefNumber = '{0}'", row["PaymentRefNumber"]);

                        //dtscol = (new Logic()).getSqlStatement(SQL1).Tables[0];

                        //if (dtscol == null && dtscol.Rows.Count == 0)
                        //{
                        //    string strqry2 = string.Format("INSERT INTO Receipt.tblCollectionReceipt( PaymentRefNumber ,EReceipts ,EReceiptsDate ,StationCode , isPrinted , IsPrintedDate , PrintedBY ,ControlNumber , ControlNumberBy ,ControlNumberDate )VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}');", row["PaymentRefNumber"], row["EReceipts"], string.Format("{0:yyyy/MM/dd hh:mm:ss}", DateTime.Now), Program.stationCode, 1, string.Format("{0:yyyy/MM/dd hh:mm:ss}", DateTime.Now), Program.UserName, row["ControlNumber"], Program.UserName, string.Format("{0:yyyy/MM/dd hh:mm:ss}", DateTime.Now));

                        //    using (SqlConnection db = new SqlConnection(Logic.ConnectionString))
                        //    {
                        //        SqlTransaction transaction;

                        //        db.Open();

                        //        transaction = db.BeginTransaction();
                        //        try
                        //        {
                        //            using (SqlCommand sqlCommand = new SqlCommand(strqry2, db, transaction))
                        //            {
                        //                sqlCommand.ExecuteNonQuery();
                        //            }

                        //        }
                        //        catch (Exception ex)
                        //        {
                        //            transaction.Rollback();
                        //            Tripous.Sys.ErrorBox(ex);
                        //            return;
                        //        }
                        //        transaction.Commit();

                        //        db.Close();
                        //    }
                        //}


                        //string SQL = String.Format(@"SELECT PaymentRefNumber FROM Receipt.tblReprintedReceipts WHERE PaymentRefNumber = '{0}'", row["PaymentRefNumber"]);

                        //Dt = (new Logic()).getSqlStatement(SQL).Tables[0];

                        //if (Dt != null && Dt.Rows.Count > 0)
                        //{
                        //    using (SqlConnection db = new SqlConnection(Logic.ConnectionString))
                        //    {
                        //        SqlTransaction transaction;

                        //        db.Open();

                        //        transaction = db.BeginTransaction();
                        //        try
                        //        {
                        //            string query1 = String.Format("UPDATE Receipt.tblReprintedReceipts SET isPrinted = '{0}', IsPrintedDate = '{1}', PrintedBy = '{2}', ControlNumber = '{3}', ControlNumberBy = '{4}', ControlNumberDate = '{5}', BatchNumber = '{6}', RequestDate = '{7}', Description = '{8}',FirstApprovalStatus = '{9}' WHERE PaymentRefNumber IN('{9}')", row["isPrinted"], string.Format("{ 0:yyyy / MM / dd hh: mm: ss}", row["IsPrintedDate"]), row["PrintedBY"], row["ControlNumber"], row["ControlNumberBy"], string.Format("{ 0:yyyy / MM / dd hh: mm: ss}", row["ControlNumberDate"]), row["BatchNumber"], string.Format("{ 0:yyyy / MM / dd hh: mm: ss}", DateTime.Now), txtReason.Text.Trim(), row["PaymentRefNumber"], 0);

                        //            using (SqlCommand sqlCommand = new SqlCommand(query1, db, transaction))
                        //            {
                        //                sqlCommand.ExecuteNonQuery();
                        //            }

                        //        }
                        //        catch (Exception ex)
                        //        {
                        //            transaction.Rollback();
                        //            Tripous.Sys.ErrorBox(ex);
                        //            return;
                        //        }
                        //        transaction.Commit();

                        //        db.Close();
                        //    }
                        //}
                        //else
                        //{
                        //    //dtsed = ds.Tables[1].rows;
                        //    using (SqlConnection db = new SqlConnection(Logic.ConnectionString))
                        //    {
                        //        SqlTransaction transaction;

                        //        db.Open();

                        //        transaction = db.BeginTransaction();
                        //        try
                        //        {


                        //            string querys = string.Format("INSERT INTO Receipt.tblReprintedReceipts( PaymentRefNumber ,isPrinted ,IsPrintedDate , PrintedBy ,ControlNumber ,ControlNumberBy ,ControlNumberDate ,BatchNumber ,RequestDate ,Description ,OldRecord ,NewRecord,TransType,FirstApprovalStatus) VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}',{12},{13});", row["PaymentRefNumber"], 1, string.Format("{0:yyyy/MM/dd hh:mm:ss}", DateTime.Now), Program.UserName, row["ControlNumber"], Program.UserName, string.Format("{0:yyyy/MM/dd hh:mm:ss}", row["ControlNumberDate"]), row["BatchNumber"], string.Format("{0:yyyy/MM/dd hh:mm:ss}", DateTime.Now), txtReason.Text.Trim(), null, null, cboSelect.SelectedValue, 0);

                        //            using (SqlCommand sqlCommand2 = new SqlCommand(querys, db, transaction))
                        //            {
                        //                sqlCommand2.ExecuteNonQuery();
                        //            }
                        //            transaction.Commit();

                        //            db.Close();
                        //        }
                        //        catch (Exception ex)
                        //        {
                        //            Tripous.Sys.ErrorBox(ex + "Inserting record to reprinted table");
                        //            transaction.Rollback();
                        //            return;
                        //        }
                        //    }
                        //}
                        ////}

                        ////    }
                        ////}

                        //    }
                        //}

                        using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
                        {
                            connect.Open();
                            _command = new SqlCommand("doCheckInsertReprintedReceipy", connect) { CommandType = CommandType.StoredProcedure };
                            _command.Parameters.Add(new SqlParameter("@Username", SqlDbType.VarChar)).Value = Program.UserName;
                            _command.Parameters.Add(new SqlParameter("@description", SqlDbType.VarChar)).Value = txtReason.Text.Trim();
                            _command.Parameters.Add(new SqlParameter("@type", SqlDbType.VarChar)).Value = cboSelect.SelectedValue;
                            _command.Parameters.Add(new SqlParameter("@pTransaction", SqlDbType.Structured)).Value = temTable;

                            using (System.Data.DataSet ds = new System.Data.DataSet())
                            {
                                ds.Clear();
                                adp = new SqlDataAdapter(_command);
                                adp.Fill(ds);
                                connect.Close();

                                if (ds.Tables[0].Rows[0]["returnCode"].ToString() != "00")
                                {
                                    Common.setMessageBox(ds.Tables[0].Rows[0]["returnMessage"].ToString(), groupBox1.Text.Trim().ToString(), 1);
                                    return;

                                }
                                else
                                {
                                    Common.setMessageBox("Contact you Local Admin for Approval before sending Online for AG Final Approval.", Program.ApplicationName, 1);
                                    return;
                                }
                            }

                        }

                    }
                    finally
                    {
                        SplashScreenManager.CloseForm(false);
                    }
                }
                else
                {
                    //if (Program.stationCode == "0008" && Program.intCode == 37)
                    //{
                    //    //ProcessHOC
                    //    DataTable dt = new DataTable();
                    //    foreach (GridColumn column in gridView5.VisibleColumns)
                    //    {
                    //        dt.Columns.Add(column.FieldName, column.ColumnType);
                    //    }
                    //    for (int i = 0; i < gridView5.DataRowCount; i++)
                    //    {
                    //        DataRow row = dt.NewRow();
                    //        foreach (GridColumn column in gridView5.VisibleColumns)
                    //        {
                    //            row[column.FieldName] = gridView5.GetRowCellValue(i, column);
                    //        }
                    //        dt.Rows.Add(row);
                    //    }
                    //    if (dt!= null && dt.Rows.Count>.0)
                    //    {
                    //        using (FrmReprintHoc display = new FrmReprintHoc(dt, false) { FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog })
                    //        {
                    //            display.ShowDialog();
                    //        }
                    //    }
                    //    else
                    //    {
                    //        Common.setMessageBox("No Record for Reprinted", Program.ApplicationName, 1);
                    //        return;
                    //    }


                    //}
                    //else
                    //{
                    try
                    {
                        SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);


                        for (int i = 0; i < gridView5.RowCount; i++)
                        {
                            //values += String.Format("{0}", obj.ToString().Trim());
                            var row = gridView5.GetDataRow(i);
                            if (row != null)
                            {
                                //values = values + String.Format("{0}", row["Payment Ref"]);
                                //values1 = values1 + String.Format("{0}", row["Receipt Number"]);
                                switch (Program.intCode)
                                {
                                    case 13://Akwa Ibom state
                                        using (var receiptAka = new AkwaIbomReceiptServices.ReceiptService())
                                        {
                                            dsreturn = receiptAka.LogReceiptsReprintRequest(String.Format("{0}", row["EReceipts"]), String.Format("{0}", row["PaymentRefNumber"]), null, Program.UserID, txtReason.Text, "NONE", null, null, null, null, null, null, Program.stationCode);
                                        }
                                        break;
                                    case 20://Delta state
                                        using (var receiptDelta = new DeltaBir.ReceiptService())
                                        {
                                            dsreturn = receiptDelta.LogReceiptsReprintRequest(String.Format("{0}", row["EReceipts"]), String.Format("{0}", row["PaymentRefNumber"]), null, Program.UserID, txtReason.Text, "NONE", null, null, null, null, null, null, Program.stationCode);
                                        }
                                        break;
                                    case 32://kogi state
                                        break;

                                    case 37://ogun state


                                        using (var receiptsserv = new ReceiptService())
                                        {
                                            dsreturn = receiptsserv.LogReceiptsReprintRequest(String.Format("{0}", row["EReceipts"]), String.Format("{0}", row["PaymentRefNumber"]), null, Program.UserID, txtReason.Text, "NONE", null, null, null, null, null, null, Program.stationCode);
                                        }

                                        break;
                                    case 40://oyo state
                                        using (var receiptsServices = new OyoReceiptServices.ReceiptService())
                                        {
                                            dsreturn = receiptsServices.LogReceiptsReprintRequest(String.Format("{0}", row["EReceipts"]), String.Format("{0}", row["PaymentRefNumber"]), null, Program.UserID, txtReason.Text, "NONE", null, null, null, null, null, null, Program.stationCode);
                                        }
                                        break;

                                    default:
                                        break;
                                }
                            }
                            //get return message from online
                            if (dsreturn.Tables[0].Rows[0]["returnCode"].ToString() == "-1")
                            {
                                Common.setMessageBox(dsreturn.Tables[0].Rows[0]["returnmessage"].ToString(), Program.ApplicationName, 1);
                                return;
                            }
                            else
                            {
                                //InsertData 

                                using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
                                {
                                    connect.Open();
                                    _command = new SqlCommand("doReprintedReceipts", connect) { CommandType = CommandType.StoredProcedure };
                                    _command.Parameters.Add(new SqlParameter("@PaymentRefNumber", SqlDbType.VarChar)).Value = String.Format("{0}", row["PaymentRefNumber"]);
                                    _command.Parameters.Add(new SqlParameter("@EReceipts", SqlDbType.VarChar)).Value = String.Format("{0}", row["EReceipts"]);

                                    using (System.Data.DataSet ds = new System.Data.DataSet())
                                    {
                                        ds.Clear();
                                        adp = new SqlDataAdapter(_command);
                                        adp.Fill(ds);
                                        connect.Close();


                                        if (ds.Tables[0].Rows[0]["returnCode"].ToString() != "00")
                                        {
                                            //Common.setMessageBox("Either the record is not printed or not Uploaded. Do Upload pending printed receipts", groupBox1.Text.Trim().ToString(), 1);
                                            Common.setMessageBox(ds.Tables[0].Rows[0]["returnMessage"].ToString(), groupBox1.Text.Trim().ToString(), 1);
                                            return;

                                        }
                                        else
                                        {
                                            DataTable Dt;

                                            string SQL = String.Format(@"SELECT PaymentRefNumber FROM Receipt.tblReprintedReceipts WHERE PaymentRefNumber = '{0}'", ds.Tables[1].Rows[0]["PaymentRefNumber"]);

                                            Dt = (new Logic()).getSqlStatement(SQL).Tables[0];

                                            if (Dt != null && Dt.Rows.Count > 0)
                                            {
                                                using (SqlConnection db = new SqlConnection(Logic.ConnectionString))
                                                {
                                                    SqlTransaction transaction;

                                                    db.Open();

                                                    transaction = db.BeginTransaction();
                                                    try
                                                    {
                                                        string query1 = String.Format("UPDATE Receipt.tblReprintedReceipts SET isPrinted = '{0}', IsPrintedDate = '{1}', PrintedBy = '{2}', ControlNumber = '{3}', ControlNumberBy = '{4}', ControlNumberDate = '{5}', BatchNumber = '{6}', RequestDate = '{7}', Description = '{8}' WHERE PaymentRefNumber IN('{9}')", ds.Tables[1].Rows[0]["isPrinted"], string.Format("{ 0:yyyy / MM / dd hh: mm: ss}", ds.Tables[1].Rows[0]["IsPrintedDate"]), ds.Tables[1].Rows[0]["PrintedBY"], ds.Tables[1].Rows[0]["ControlNumber"], ds.Tables[1].Rows[0]["ControlNumberBy"], string.Format("{ 0:yyyy / MM / dd hh: mm: ss}", ds.Tables[1].Rows[0]["ControlNumberDate"]), ds.Tables[1].Rows[0]["BatchNumber"], string.Format("{ 0:yyyy / MM / dd hh: mm: ss}", DateTime.Now), txtReason.Text.Trim(), ds.Tables[1].Rows[0]["PaymentRefNumber"]);

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
                                            else
                                            {
                                                //dtsed = ds.Tables[1].rows;
                                                using (SqlConnection db = new SqlConnection(Logic.ConnectionString))
                                                {
                                                    SqlTransaction transaction;

                                                    db.Open();

                                                    transaction = db.BeginTransaction();
                                                    try
                                                    {
                                                        string querys = string.Format("INSERT INTO Receipt.tblReprintedReceipts( PaymentRefNumber ,isPrinted ,IsPrintedDate , PrintedBy ,ControlNumber ,ControlNumberBy ,ControlNumberDate ,BatchNumber ,RequestDate ,Description ,OldRecord ,NewRecord,TransType) VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}',{12});", ds.Tables[1].Rows[0]["PaymentRefNumber"], ds.Tables[1].Rows[0]["isPrinted"], string.Format("{0:yyyy/MM/dd hh:mm:ss}", ds.Tables[1].Rows[0]["IsPrintedDate"]), ds.Tables[1].Rows[0]["PrintedBY"], Program.stationCode, Program.UserID, string.Format("{0:yyyy/MM/dd hh:mm:ss}", ds.Tables[1].Rows[0]["ControlNumberDate"]), ds.Tables[1].Rows[0]["BatchNumber"], string.Format("{0:yyyy/MM/dd hh:mm:ss}", DateTime.Now), txtReason.Text.Trim(), null, null, cboSelect.SelectedValue);

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
                                            }
                                        }

                                    }
                                }
                            }

                        }
                        Common.setMessageBox("Request Sent for Approval,Please Click on Get Approval Record, if your Admin has approve the record....", Program.ApplicationName, 1);
                        //long split record and send request
                        //for (int o = 0; o < Split.Count(); o++)
                        //{

                        //    try
                        //    {
                        //        switch (Program.intCode)
                        //        {
                        //            case 13://Akwa Ibom state
                        //                using (var receiptAka = new AkwaIbomReceiptServices.ReceiptService())
                        //                {
                        //                    dsreturn = receiptAka.LogReceiptsReprintRequest((string)Split[o], (string)Split2[o], (string)Split3[o], Program.UserID, txtReason.Text, "NONE", null, null, null, null, null, null, Program.stationCode);
                        //                }

                        //                break;

                        //            case 32://kogi state
                        //                break;

                        //            case 37://ogun state

                        //                using (var receiptsserv = new ReceiptService())
                        //                {
                        //                    dsreturn = receiptsserv.LogReceiptsReprintRequest((string)Split[o], (string)Split2[o], (string)Split3[o], Program.UserID, txtReason.Text, "NONE", null, null, null, null, null, null, Program.stationCode);
                        //                }
                        //                break;

                        //            case 40://oyo state
                        //                break;

                        //            default:
                        //                break;
                        //        }
                        //    }
                        //    catch (Exception ex)
                        //    {
                        //        Common.setMessageBox(string.Format("{0}----{1}..Sending Request Reprint of Receipt Failed", ex.Message, ex.StackTrace), Program.ApplicationName, 3);
                        //        return;
                        //    }
                        //}
                        //get return message from online
                        //if (dsreturn.Tables[0].Rows[0]["returnmessage"].ToString() == "-1")
                        //{
                        //    Common.setMessageBox(dsreturn.Tables[0].Rows[0]["returnmessage1"].ToString(), Program.ApplicationName, 1);
                        //    return;
                        //}
                        //else
                        //{
                        //    using (SqlConnection db = new SqlConnection(Logic.ConnectionString))
                        //    {
                        //        SqlTransaction transaction;

                        //        db.Open();

                        //        transaction = db.BeginTransaction();
                        //        try
                        //        {
                        //            string querys = string.Format("INSERT INTO Receipt.tblReprintedReceipts( PaymentRefNumber ,isPrinted ,IsPrintedDate , PrintedBy ,ControlNumber ,ControlNumberBy ,ControlNumberDate ,BatchNumber ,RequestDate ,Description ,OldRecord ,NewRecord) VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}');", dt.Rows[0]["PaymentRefNumber"], dt.Rows[0]["isPrinted"], string.Format("{0:yyyy/MM/dd hh:mm:ss}", dt.Rows[0]["IsPrintedDate"]), dt.Rows[0]["PrintedBY"], dt.Rows[0]["ControlNumber"], dt.Rows[0]["ControlNumberBy"], string.Format("{0:yyyy/MM/dd hh:mm:ss}", dt.Rows[0]["ControlNumberDate"]), dt.Rows[0]["BatchNumber"], string.Format("{0:yyyy/MM/dd hh:mm:ss}", DateTime.Now), txtReason.Text.Trim(), null, null);

                        //            using (SqlCommand sqlCommand2 = new SqlCommand(querys, db, transaction))
                        //            {
                        //                sqlCommand2.ExecuteNonQuery();
                        //            }
                        //            transaction.Commit();

                        //            db.Close();
                        //        }
                        //        catch (Exception ex)
                        //        {
                        //            Tripous.Sys.ErrorBox(ex + "Inserting record to reprinted table");
                        //            transaction.Rollback();
                        //            return;
                        //        }
                        //    }

                        //    Common.setMessageBox("Request Sent for Approval,Please Click on Get Approval Record, if your Admin has approve the record....", Program.ApplicationName, 1);
                        //    //Common.setMessageBox("Please Click on Get Approval Record..", Program.ApplicationName, 1);
                        //    return;
                        //}
                    }
                    finally
                    {
                        SplashScreenManager.CloseForm(false);

                    }
                    //}
                }
            }
        }

        void btnSelect_Click(object sender, EventArgs e)
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

                values = string.Empty; values1 = string.Empty; values2 = string.Empty;

                int k = 0;
                int l = 0;

                for (int i = 0; i < gridView3.RowCount - 1; i++)
                {
                    //values += String.Format("{0}", obj.ToString().Trim());
                    var row = gridView3.GetDataRow(i);

                    if (row != null)
                    {
                        if (!string.IsNullOrEmpty(row["Payment Ref"].ToString()))
                        {
                            values += String.Format("{0}", String.Format("{0}", row["Payment Ref"]));


                            if (k + 1 < gridView3.RowCount - 1)

                                values += ",";
                            ++k;

                        }
                        if (!string.IsNullOrEmpty(row["Receipt Number"].ToString()))
                        {
                            values1 += String.Format("{0}", String.Format("{0}", row["Receipt Number"]));

                            if (l + 1 < gridView3.RowCount - 1)
                                values1 += ",";
                            ++l;
                        }



                    }

                }

                try
                {
                    using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
                    {
                        connect.Open();
                        _command = new SqlCommand("doReprintedReceipts", connect) { CommandType = CommandType.StoredProcedure };
                        _command.Parameters.Add(new SqlParameter("@PaymentRefNumber", SqlDbType.VarChar)).Value = values;
                        _command.Parameters.Add(new SqlParameter("@EReceipts", SqlDbType.VarChar)).Value = values1;
                        _command.CommandTimeout = 0;
                        using (System.Data.DataSet ds = new System.Data.DataSet())
                        {
                            ds.Clear();
                            adp = new SqlDataAdapter(_command);
                            adp.Fill(ds);
                            connect.Close();


                            if (ds.Tables[0].Rows[0]["returnCode"].ToString() != "00")
                            {
                                Common.setMessageBox(ds.Tables[0].Rows[0]["returnMessage"].ToString(), Program.ApplicationName, 2);

                                //Common.setMessageBox("Either the record is not printed or not Uploaded. Do Upload pending printed receipts", groupBox1.Text.Trim().ToString(), 1);

                                return;


                            }
                            else
                            {
                                dt = ds.Tables[1];
                                gridControl1.DataSource = dt;

                                gridView2.BestFitColumns();
                                layoutView1.Columns["Amount"].DisplayFormat.FormatType = FormatType.Numeric;
                                //layoutView1.Columns["Amount"].
                                layoutView1.Columns["Amount"].DisplayFormat.FormatString = "n2";

                                gridControl3.DataSource = dt;
                                gridView5.Columns["Amount"].DisplayFormat.FormatType = FormatType.Numeric;
                                gridView5.Columns["Amount"].DisplayFormat.FormatString = "n2";

                                gridView5.Columns["PaymentDate"].Visible = false;
                                gridView5.Columns["AgencyName"].Visible = false;
                                gridView5.Columns["Description"].Visible = false;
                                gridView5.Columns["PrintedBY"].Visible = false;
                                gridView5.Columns["IsPrintedDate"].Visible = false;
                                gridView5.Columns["ControlNumber"].Visible = false;
                                gridView5.Columns["ControlNumberBy"].Visible = false;
                                gridView5.Columns["ControlNumberDate"].Visible = false;
                                gridView5.Columns["isPrinted"].Visible = false;
                                gridView5.Columns["BatchNumber"].Visible = false;
                                gridView5.BestFitColumns();
                            }

                        }
                    }
                }
                catch (Exception ex)
                {
                    Common.setMessageBox(ex.Message, Program.ApplicationName, 3);
                    txtPay.Text = string.Empty; txtSearch.Text = string.Empty;
                    return;
                }

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

            btnDelete.Image = MDIMain.publicMDIParent.i32x32.Images[9];
            //btnPrint.Image = MDIMain.publicMDIParent.i32x32.Images[29];
            btnConfirm.Image = MDIMain.publicMDIParent.i32x32.Images[10];
            btnSearch.Image = MDIMain.publicMDIParent.i32x32.Images[2];
            btnPrint.Image = MDIMain.publicMDIParent.i32x32.Images[29];
            btnMain.Image = MDIMain.publicMDIParent.i32x32.Images[8];


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
            isReprint = false; setDBComboBox();

            ShowForm();
        }

        public void RefreshForm()
        {
            NavBars.ToolStripEnableDisableControls(toolStrip, Tag as String);
        }

        internal GridCheckMarksSelection Selection
        {
            get { return selection; }
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

        public System.Data.DataSet InsertData(System.Data.DataSet dataSet)
        {
            System.Data.DataSet ds = new System.Data.DataSet();

            try
            {
                using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
                {
                    connect.Open();

                    command = new SqlCommand("doInsertModifyReceipt", connect) { CommandType = CommandType.StoredProcedure };
                    command.Parameters.Add(new SqlParameter("@Userid", SqlDbType.VarChar)).Value = Program.UserID;
                    command.Parameters.Add(new SqlParameter("@tblCollectionReport_Temp", SqlDbType.Structured)).Value = dataSet.Tables[1];
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

        void voidgetRec(System.Data.DataSet dt)
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

                DataTable dtn = new DataTable();
                values = string.Empty;
                DataTable dtf = dt.Tables[0];

                int j = 0;

                foreach (DataRow dr in dtf.Rows)
                {
                    //messagebox.show(dr["paymentrefnumber"].tostring());

                    values += string.Format("'{0}'", dr["paymentrefnumber"]);

                    if (j + 1 < dtf.Rows.Count)
                        values += ",";
                    ++j;
                }


                string quy = String.Format("SELECT  ID , PaymentRefNumber,DepositSlipNumber,PaymentDate,UPPER(PayerName) as PayerName,Description,RevenueCode,Amount,BankName+ '-'+ BranchName AS Bank,EReceipts from Collection.tblCollectionReport WHERE Collection.tblCollectionReport.PaymentRefNumber IN ({0}) ORDER BY StationCode , AgencyCode ,RevenueCode,EReceipts", values);


                try
                {
                    using (var ds = new System.Data.DataSet())
                    {
                        ds.Clear();

                        using (SqlDataAdapter ada = new SqlDataAdapter(quy, Logic.ConnectionString))
                        {
                            ada.Fill(ds, "table");
                        }
                        dtn.Clear();
                        dtn = ds.Tables[0];
                        gridControl4.DataSource = dtn;
                        gridView4.BestFitColumns();
                        gridView4.Columns["Amount"].DisplayFormat.FormatType = FormatType.Numeric;
                        gridView4.Columns["Amount"].DisplayFormat.FormatString = "n2";
                        gridView4.Columns["PaymentDate"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
                        gridView4.Columns["PaymentDate"].DisplayFormat.FormatString = "dd/MM/yyyy";
                        gridView4.Columns["ID"].Visible = false;


                        //[GeneratedBy],[BankName],[BranchName]
                    }
                }
                catch (Exception ex)
                {
                    Common.setMessageBox(ex.Message, Program.ApplicationName, 3);
                    return;
                }

                if (isFirstGrid)
                {
                    selection = new GridCheckMarksSelection(gridView4, ref lblSelect);
                    selection.CheckMarkColumn.VisibleIndex = 0;
                    isFirstGrid = false;
                }

            }
            finally
            {
                SplashScreenManager.CloseForm(false);
            }


        }

        public void setDBComboBox()
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

                DataTable Dt;

                using (var ds = new System.Data.DataSet())
                {
                    using (SqlDataAdapter ada = new SqlDataAdapter((string)"SELECT Description,TransType FROM Receipt.tblReceiptTransaction WHERE FlagID=2 ORDER BY Description", Logic.ConnectionString))
                    {
                        ada.SelectCommand.CommandTimeout = 0;

                        ada.Fill(ds, "table");
                    }

                    Dt = ds.Tables[0];
                }

                Common.setComboList(cboSelect, Dt, "TransType", "Description");

                //cboSelect.SelectedIndex = 3;
            }
            finally
            {
                SplashScreenManager.CloseForm(false);
            }
        }

        //void ProcessHOC()
        //{
        //    //string values = string.Empty;
        //    int intval = 0;

        //    lblSelect.Text = string.Empty;

        //    int j = 0;

        //    if (Program.stateCode == "20")
        //    {
        //        intval = Convert.ToInt32(radioGroup1.EditValue);
        //    }

        //    for (int i = 0; i < selection.SelectedCount; i++)
        //    {
        //        values += String.Format("'{0}'", (selection.GetSelectedRow(i) as DataRowView)["PaymentRefNumber"]);
        //        if (j + 1 < selection.SelectedCount)
        //            values += ",";
        //        ++j;

        //        temTable.Rows.Add(new object[] { String.Format("{0}", (selection.GetSelectedRow(i) as DataRowView)["EReceipts"]), String.Format("{0}", (selection.GetSelectedRow(i) as DataRowView)["PaymentRefNumber"]), Program.UserID });

        //    }

        //    using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
        //    {
        //        connect.Open();
        //        _command = new SqlCommand("InserttblReceipt", connect) { CommandType = CommandType.StoredProcedure };
        //        _command.Parameters.Add(new SqlParameter("@tblCollectionReport_Temp", SqlDbType.Structured)).Value = temTable;
        //        _command.Parameters.Add(new SqlParameter("@type", SqlDbType.VarChar)).Value = "New";
        //        //_command.Parameters.Add(new SqlParameter("@intoption", SqlDbType.Int)).Value = intval;
        //        _command.Parameters.Add(new SqlParameter("@userid", SqlDbType.VarChar)).Value = Program.UserID;
        //        System.Data.DataSet response = new System.Data.DataSet();
        //        response.Clear();
        //        adp = new SqlDataAdapter(_command);
        //        adp.Fill(response);

        //        connect.Close();
        //        if (String.Compare(response.Tables[0].Rows[0]["returnCode"].ToString(), "00", false) == 0)
        //        {
        //            //do something load the report page
        //            using (SqlConnection db = new SqlConnection(Logic.ConnectionString))
        //            {
        //                SqlTransaction transaction;

        //                db.Open();

        //                transaction = db.BeginTransaction();

        //                try
        //                {
        //                    using (System.Data.DataSet dds = new System.Data.DataSet("DsCollectionReport"))
        //                    {
        //                        SqlDataAdapter ada;

        //                        using (WaitDialogForm form = new WaitDialogForm())
        //                        {
        //                            string strFormat = null; string query = string.Empty;

        //                            //if (Program.stateCode == "20")
        //                            //{
        //                            //    if (Convert.ToInt32(radioGroup1.EditValue) != 1)
        //                            //    {
        //                            //        query = string.Format("SELECT [ID] , [Provider] , [Channel] ,Collection.tblCollectionReport.PaymentRefNumber , [DepositSlipNumber] , CONVERT(VARCHAR,CONVERT(DATE,[PaymentDate])) AS PaymentDate,[PayerID] , UPPER([PayerName]) AS [PayerName], [Amount] ,[PaymentMethod] ,[ChequeNumber] ,[ChequeValueDate] ,[ChequeStatus] ,[DateChequeReturned] ,[TelephoneNumber] ,[ReceiptNo] ,[ReceiptDate] ,UPPER([PayerAddress]) as [PayerAddress] ,[User] ,[RevenueCode] ,[Description] , [ChequeBankCode] ,[ChequeBankName] ,[AgencyName] ,[AgencyCode] ,[BankCode] ,[BankName] ,[BranchCode] ,[BranchName] ,[ZoneCode] ,[ZoneName] ,[Username] ,[AmountWords] ,Collection.tblCollectionReport.[EReceipts] ,Collection.tblCollectionReport.[EReceiptsDate],[GeneratedBy] ,  Collection.tblCollectionReport.[StationCode] ,(SELECT TOP 1 StationName FROM Receipt.tblStation  WHERE tblStation.StationCode = Collection.tblCollectionReport.[StationCode]) AS StationName,ControlNumber , ControlNumberBy , ControlNumberDate ,IsPrintedDate , PrintedBY from Collection.tblCollectionReport  INNER JOIN Receipt.tblCollectionReceipt ON Receipt.tblCollectionReceipt.PaymentRefNumber = Collection.tblCollectionReport.PaymentRefNumber INNER JOIN Receipt.tblReceiptoption ON Receipt.tblReceiptoption.PaymentRefNumber = Receipt.tblCollectionReceipt.PaymentRefNumber WHERE Collection.tblCollectionReport.PaymentRefNumber  IN ({0})         AND Receipt.tblReceiptoption.IsPrinted = 1  ORDER BY Collection.tblCollectionReport.AgencyName ,Collection.tblCollectionReport.Description,Collection.tblCollectionReport.EReceipts", values);
        //                            //    }
        //                            //    else
        //                            //    {
        //                            //        query = string.Format("SELECT [ID] , [Provider] , [Channel] ,[PaymentRefNumber] , [DepositSlipNumber] , CONVERT(VARCHAR,CONVERT(DATE,[PaymentDate])) AS PaymentDate,[PayerID] , UPPER([PayerName]) AS [PayerName], [Amount] ,[PaymentMethod] ,[ChequeNumber] ,[ChequeValueDate] ,[ChequeStatus] ,[DateChequeReturned] ,[TelephoneNumber] ,[ReceiptNo] ,[ReceiptDate] ,UPPER([PayerAddress]) as [PayerAddress] ,[User] ,[RevenueCode] ,[Description] , [ChequeBankCode] ,[ChequeBankName] ,[AgencyName] ,[AgencyCode] ,[BankCode] ,[BankName] ,[BranchCode] ,[BranchName] ,[ZoneCode] ,[ZoneName] ,[Username] ,[AmountWords] ,[EReceipts] ,[EReceiptsDate] ,[GeneratedBy] ,[StationCode] ,(SELECT TOP 1 StationName FROM Receipt.tblStation  WHERE tblStation.StationCode = Collection.tblCollectionReport.[StationCode]) AS StationName from Collection.tblCollectionReport WHERE PaymentRefNumber IN ({0})  ORDER BY Collection.tblCollectionReport.AgencyName ,Collection.tblCollectionReport.Description,Collection.tblCollectionReport.EReceipts", values);
        //                            //    }
        //                            //}
        //                            //else
        //                            //{

        //                            //}

        //                            switch (Program.intCode)
        //                            {
        //                                case 20://detla state
        //                                    query = string.Format("SELECT [ID] , [Provider] , [Channel] ,[PaymentRefNumber] , [DepositSlipNumber] , CONVERT(VARCHAR,CONVERT(DATE,[PaymentDate])) AS PaymentDate,[PayerID] , UPPER([PayerName]) AS [PayerName], [Amount] ,[PaymentMethod] ,[ChequeNumber] ,[ChequeValueDate] ,[ChequeStatus] ,[DateChequeReturned] ,[TelephoneNumber] ,[ReceiptNo] ,[ReceiptDate] ,UPPER([PayerAddress]) as [PayerAddress] ,[User] ,[RevenueCode] ,[Description] , [ChequeBankCode] ,[ChequeBankName] ,[AgencyName] ,[AgencyCode] ,[BankCode] ,[BankName] ,[BranchCode] ,[BranchName] ,[ZoneCode] ,[ZoneName] ,[Username] ,[AmountWords] ,[EReceipts] ,[EReceiptsDate] ,[GeneratedBy] ,[StationCode] ,(SELECT TOP 1 StationName FROM Receipt.tblStation  WHERE tblStation.StationCode = Collection.tblCollectionReport.[StationCode]) AS StationName,ControlNumber from Collection.tblCollectionReport WHERE PaymentRefNumber IN (SELECT PaymentRefNumber FROM Receipt.tblReceipt where SentBy='{0}')   ORDER BY Collection.tblCollectionReport.AgencyName ,Collection.tblCollectionReport.Description,Collection.tblCollectionReport.EReceipts", Program.UserID);
        //                                    break;
        //                                default:
        //                                    query = string.Format("SELECT [ID] , [Provider] , [Channel] ,[PaymentRefNumber] , [DepositSlipNumber] , CONVERT(VARCHAR,CONVERT(DATE,[PaymentDate])) AS PaymentDate,[PayerID] , UPPER([PayerName]) AS [PayerName], [Amount] ,[PaymentMethod] ,[ChequeNumber] ,[ChequeValueDate] ,[ChequeStatus] ,[DateChequeReturned] ,[TelephoneNumber] ,[ReceiptNo] ,[ReceiptDate] ,UPPER([PayerAddress]) as [PayerAddress] ,[User] ,[RevenueCode] ,[Description] , [ChequeBankCode] ,[ChequeBankName] ,[AgencyName] ,[AgencyCode] ,[BankCode] ,[BankName] ,[BranchCode] ,[BranchName] ,[ZoneCode] ,[ZoneName] ,[Username] ,[AmountWords] ,[EReceipts] ,[EReceiptsDate] ,[GeneratedBy] ,[StationCode] ,(SELECT TOP 1 StationName FROM Receipt.tblStation  WHERE tblStation.StationCode = Collection.tblCollectionReport.[StationCode]) AS StationName from Collection.tblCollectionReport WHERE PaymentRefNumber IN (SELECT PaymentRefNumber FROM Receipt.tblReceipt where SentBy='{0}')   ORDER BY Collection.tblCollectionReport.AgencyName ,Collection.tblCollectionReport.Description,Collection.tblCollectionReport.EReceipts", Program.UserID);
        //                                    break;
        //                            }


        //                            DataTable Dt = dds.Tables.Add("CollectionReportTable");
        //                            ada = new SqlDataAdapter(query, Logic.ConnectionString);
        //                            ada.Fill(dds, "CollectionReportTable");
        //                            //Logic.ProcessDataTable(Dt);;
        //                            Logic.ProcessDataTable(Dt);
        //                            //strCollectionReportID = strFormat;
        //                        }

        //                        //XRepReceipt recportRec = new XRepReceipt { DataSource = dds.Tables[0] /*recportRec.DataAdapter = ada;*/, DataMember = "CollectionReportTable" };

        //                        //recportRec.ShowPreviewDialog();

        //                        ////selection.ClearSelection(); 
        //                        //dds.Clear();

        //                        switch (Program.intCode)
        //                        {
        //                            case 13://Akwa Ibom state
        //                                XtraRepReceiptAkwa akwa = new XtraRepReceiptAkwa { DataSource = dds.Tables[0] /*recportRec.DataAdapter = ada;*/, DataMember = "CollectionReportTable" };
        //                                akwa.ShowPreviewDialog();
        //                                break;

        //                            case 20://detla state
        //                                XtraRepReceiptDelta delta = new XtraRepReceiptDelta { DataSource = dds.Tables[0] /*recportRec.DataAdapter = ada;*/, DataMember = "CollectionReportTable" };
        //                                //delta.PrintingSystem.StartPrint += PrintingSystem_StartPrint;
        //                                //if (Program.stateCode == "20")
        //                                //{
        //                                //    if (Convert.ToInt32(radioGroup1.EditValue) != 1)
        //                                //    {
        //                                //        delta.xrLabel19.Visible = true;
        //                                //        //delta.xrLabel13.Visible = true;
        //                                //    }

        //                                //}
        //                                //delta.Watermark.Text = "DUPLICATE";
        //                                ////delta.Watermark.TextDirection = DirectionMode.Clockwise;
        //                                //delta.Watermark.Font = new Font(delta.Watermark.Font.FontFamily, 40);
        //                                //delta.Watermark.ForeColor = Color.DodgerBlue;
        //                                //delta.Watermark.TextTransparency = 150;
        //                                //delta.Watermark.ShowBehind = false;

        //                                delta.logoPath = Logic.singaturepth;
        //                                delta.ShowPreviewDialog();
        //                                //delta.Print();
        //                                break;

        //                            case 37://ogun state
        //                                XRepReceipt recportRec = new XRepReceipt { DataSource = dds.Tables[0] /*recportRec.DataAdapter = ada;*/, DataMember = "CollectionReportTable" };
        //                                recportRec.logoPath = Logic.singaturepth;
        //                                recportRec.ShowPreviewDialog();
        //                                break;

        //                            case 40://oyo state
        //                                XRepReceiptoyo recportRecs = new XRepReceiptoyo { DataSource = dds.Tables[0] /*recportRec.DataAdapter = ada;*/, DataMember = "CollectionReportTable" };
        //                                recportRecs.ShowPreviewDialog();
        //                                break;

        //                            //case 32://kogi state

        //                            //    XRepReceiptkogi recportRecko = new XRepReceiptkogi { DataSource = dds.Tables[0] /*recportRec.DataAdapter = ada;*/, DataMember = "CollectionReportTable" };
        //                            //    recportRecko.ShowPreviewDialog();

        //                            //    break;

        //                            default:

        //                                break;
        //                        }
        //                    }

        //                    //dds.Clear();
        //                    DialogResult result = MessageBox.Show(" Is Receipt Printing Successful ?", Program.ApplicationName, MessageBoxButtons.YesNo);

        //                    if (result == DialogResult.Yes)
        //                    {
        //                        try
        //                        {
        //                            SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

        //                            //update the collection table by seting the isprinted to true
        //                            using (SqlConnection dbs = new SqlConnection(Logic.ConnectionString))
        //                            {
        //                                SqlTransaction transactions;

        //                                dbs.Open();

        //                                //transaction = db.BeginTransaction();

        //                                transactions = dbs.BeginTransaction();

        //                                try
        //                                {
        //                                    string query1 = String.Format("UPDATE tblCollectionReport SET isPrinted=1,IsPrintedDate= '{0:MM/dd/yyyy HH:mm:ss tt}',PrintedBY='{1}' ,StationCode='{2}' WHERE PaymentRefNumber IN ({3})", DateTime.Now, Program.UserID, Program.stationCode, values);

        //                                    //string query1 = String.Format("UPDATE Receipt.tblCollectionReceipt SET isPrinted=1,IsPrintedDate= '{0:MM/dd/yyyy HH:mm:ss tt}',PrintedBY='{1}',StationCode='{2}' WHERE PaymentRefNumber IN (SELECT PaymentRefNumber FROM Receipt.tblReceipt WHERE SentBy='{1}')", DateTime.Now, Program.UserID, Program.stationCode);

        //                                    using (SqlCommand sqlCommand = new SqlCommand(query1, db, transaction))
        //                                    {
        //                                        sqlCommand.ExecuteNonQuery();
        //                                    }

        //                                }
        //                                catch (Exception ex)
        //                                {
        //                                    transaction.Rollback();
        //                                    Tripous.Sys.ErrorBox(ex);
        //                                    return;
        //                                }
        //                                transaction.Commit();

        //                                db.Close();
        //                            }
        //                            bttnMain.Visible = true; label1.Visible = true;
        //                            bttnMain.Enabled = true;
        //                        }
        //                        finally
        //                        {
        //                            SplashScreenManager.CloseForm(false);
        //                        }
        //                        //using (FrmMainFest frmMainFest = new FrmMainFest(strCollectionReportID, values, criteria2, false) { IDList = strCollectionReportID })
        //                        //{
        //                        //    frmMainFest.ShowDialog();
        //                        //}
        //                    }
        //                    else
        //                        return;

        //                }
        //                catch (SqlException sqlError)
        //                {
        //                    //transaction.Rollback();

        //                    Tripous.Sys.ErrorBox(sqlError);
        //                }
        //                catch (Exception ex)
        //                {
        //                    Tripous.Sys.ErrorBox(ex);
        //                }
        //                db.Close();
        //            }

        //        }
        //        else
        //        {
        //            if (String.Compare(response.Tables[0].Rows[0]["returnCode"].ToString(), "-1", false) == 0)
        //            {
        //                using (Frmcontrol frmcontrol = new Frmcontrol())
        //                {
        //                    frmcontrol.gridControl1.DataSource = response.Tables[1];
        //                    frmcontrol.gridView1.BestFitColumns();
        //                    frmcontrol.label1.Text = "Payment Ref. Number Receipt printing in Process";
        //                    frmcontrol.Text = "Payment Ref. Number Receipt printing in Process";
        //                    frmcontrol.ShowDialog();
        //                }

        //                DialogResult result = MessageBox.Show("Do you wish to continue to Print the Receipts?", Program.ApplicationName, MessageBoxButtons.YesNo);

        //                if (result == DialogResult.Yes)
        //                {

        //                    using (SqlConnection db = new SqlConnection(Logic.ConnectionString))
        //                    {
        //                        SqlTransaction transaction;

        //                        db.Open();

        //                        transaction = db.BeginTransaction();

        //                        try
        //                        {
        //                            using (System.Data.DataSet dds = new System.Data.DataSet("DsCollectionReport"))
        //                            {
        //                                SqlDataAdapter ada;

        //                                using (WaitDialogForm form = new WaitDialogForm())
        //                                {
        //                                    string strFormat = null; string query = string.Empty;


        //                                    switch (Program.intCode)
        //                                    {
        //                                        case 20://detla state
        //                                            query = string.Format("SELECT [ID] , [Provider] , [Channel] ,[PaymentRefNumber] , [DepositSlipNumber] , CONVERT(VARCHAR,CONVERT(DATE,[PaymentDate])) AS PaymentDate,[PayerID] , UPPER([PayerName]) AS [PayerName], [Amount] ,[PaymentMethod] ,[ChequeNumber] ,[ChequeValueDate] ,[ChequeStatus] ,[DateChequeReturned] ,[TelephoneNumber] ,[ReceiptNo] AS [EReceipts] ,[ReceiptDate] ,UPPER([PayerAddress]) as [PayerAddress] ,[User] ,[RevenueCode] ,[Description] , [ChequeBankCode] ,[ChequeBankName] ,[AgencyName] ,[AgencyCode] ,[BankCode] ,[BankName] ,[BranchCode] ,[BranchName] ,[ZoneCode] ,[ZoneName] ,[Username] ,[AmountWords] ,[EReceiptsDate] ,[GeneratedBy] ,[StationCode] ,(SELECT TOP 1 StationName FROM Receipt.tblStation  WHERE tblStation.StationCode = Collection.tblCollectionReport.[StationCode]) AS StationName from Collection.tblCollectionReport WHERE PaymentRefNumber IN ({0})  ORDER BY Collection.tblCollectionReport.AgencyName ,Collection.tblCollectionReport.Description,Collection.tblCollectionReport.ReceiptNo", values);
        //                                            break;
        //                                        default:
        //                                            query = string.Format("SELECT [ID] , [Provider] , [Channel] ,[PaymentRefNumber] , [DepositSlipNumber] , CONVERT(VARCHAR,CONVERT(DATE,[PaymentDate])) AS PaymentDate,[PayerID] , UPPER([PayerName]) AS [PayerName], [Amount] ,[PaymentMethod] ,[ChequeNumber] ,[ChequeValueDate] ,[ChequeStatus] ,[DateChequeReturned] ,[TelephoneNumber] ,[ReceiptNo] ,[ReceiptDate] ,UPPER([PayerAddress]) as [PayerAddress] ,[User] ,[RevenueCode] ,[Description] , [ChequeBankCode] ,[ChequeBankName] ,[AgencyName] ,[AgencyCode] ,[BankCode] ,[BankName] ,[BranchCode] ,[BranchName] ,[ZoneCode] ,[ZoneName] ,[Username] ,[AmountWords] ,[EReceipts] ,[EReceiptsDate] ,[GeneratedBy] ,[StationCode] ,(SELECT TOP 1 StationName FROM Receipt.tblStation  WHERE tblStation.StationCode = Collection.tblCollectionReport.[StationCode]) AS StationName from Collection.tblCollectionReport WHERE PaymentRefNumber IN ({0})  ORDER BY Collection.tblCollectionReport.AgencyName ,Collection.tblCollectionReport.Description,Collection.tblCollectionReport.EReceipts", values);
        //                                            break;
        //                                    }

        //                                    DataTable Dt = dds.Tables.Add("CollectionReportTable");
        //                                    ada = new SqlDataAdapter(query, Logic.ConnectionString);
        //                                    ada.Fill(dds, "CollectionReportTable");
        //                                    //Logic.ProcessDataTable(Dt);;
        //                                    Logic.ProcessDataTable(Dt); ;
        //                                    //strCollectionReportID = strFormat;
        //                                }


        //                                //XRepReceipt recportRec = new XRepReceipt { DataSource = dds.Tables[0] /*recportRec.DataAdapter = ada;*/, DataMember = "CollectionReportTable" };

        //                                //recportRec.ShowPreviewDialog();

        //                                switch (Program.intCode)
        //                                {
        //                                    case 13://Akwa Ibom state
        //                                        XtraRepReceiptAkwa akwa = new XtraRepReceiptAkwa { DataSource = dds.Tables[0] /*recportRec.DataAdapter = ada;*/, DataMember = "CollectionReportTable" };
        //                                        akwa.ShowPreviewDialog();
        //                                        break;

        //                                    case 20://detla state
        //                                        XtraRepReceiptDelta delta = new XtraRepReceiptDelta { DataSource = dds.Tables[0] /*recportRec.DataAdapter = ada;*/, DataMember = "CollectionReportTable" };
        //                                        //delta.PrintingSystem.StartPrint += PrintingSystem_StartPrint;
        //                                        delta.logoPath = Logic.singaturepth;
        //                                        delta.ShowPreviewDialog();
        //                                        //delta.Print();
        //                                        break;

        //                                    case 37://ogun state
        //                                        XRepReceipt recportRec = new XRepReceipt { DataSource = dds.Tables[0] /*recportRec.DataAdapter = ada;*/, DataMember = "CollectionReportTable" };
        //                                        recportRec.logoPath = Logic.singaturepth;
        //                                        recportRec.ShowPreviewDialog();
        //                                        break;

        //                                    case 40://oyo state
        //                                        XRepReceiptoyo recportRecs = new XRepReceiptoyo { DataSource = dds.Tables[0] /*recportRec.DataAdapter = ada;*/, DataMember = "CollectionReportTable" };
        //                                        recportRecs.ShowPreviewDialog();
        //                                        break;

        //                                    //case 32://kogi state

        //                                    //    XRepReceiptkogi recportRecko = new XRepReceiptkogi { DataSource = dds.Tables[0] /*recportRec.DataAdapter = ada;*/, DataMember = "CollectionReportTable" };
        //                                    //    recportRecko.ShowPreviewDialog();

        //                                    //    break;

        //                                    default:
        //                                        break;
        //                                }
        //                                //selection.ClearSelection(); 
        //                                dds.Clear();
        //                            }

        //                            DialogResult results = MessageBox.Show(" Is Receipt Printing Successful ?", Program.ApplicationName, MessageBoxButtons.YesNo);

        //                            if (results == DialogResult.Yes)
        //                            {
        //                                try
        //                                {
        //                                    SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

        //                                    //update the collection table by seting the isprinted to true
        //                                    using (SqlConnection dbs = new SqlConnection(Logic.ConnectionString))
        //                                    {
        //                                        SqlTransaction transactions;

        //                                        dbs.Open();

        //                                        //transaction = db.BeginTransaction();

        //                                        transactions = dbs.BeginTransaction();

        //                                        try
        //                                        {
        //                                            string query1 = String.Format("UPDATE Receipt.tblCollectionReceipt SET isPrinted=1,IsPrintedDate= '{0:MM/dd/yyyy HH:mm:ss tt}',PrintedBY='{1}' ,StationCode='{2}' WHERE PaymentRefNumber IN ({3})", DateTime.Now, Program.UserID, Program.stationCode, values);

        //                                            //string query1 = String.Format("UPDATE Receipt.tblCollectionReceipt SET isPrinted=1,IsPrintedDate= '{0:MM/dd/yyyy HH:mm:ss tt}',PrintedBY='{1}',StationCode='{2}' WHERE PaymentRefNumber IN (SELECT PaymentRefNumber FROM Receipt.tblReceipt WHERE SentBy='{1}')", DateTime.Now, Program.UserID, Program.stationCode);

        //                                            using (SqlCommand sqlCommand = new SqlCommand(query1, db, transaction))
        //                                            {
        //                                                sqlCommand.ExecuteNonQuery();
        //                                            }

        //                                        }
        //                                        catch (Exception ex)
        //                                        {
        //                                            transaction.Rollback();
        //                                            Tripous.Sys.ErrorBox(ex);
        //                                            return;
        //                                        }
        //                                        transaction.Commit();

        //                                        db.Close();
        //                                    }
        //                                    bttnMain.Visible = true; label1.Visible = true;
        //                                    bttnMain.Enabled = true;
        //                                }
        //                                finally
        //                                {
        //                                    SplashScreenManager.CloseForm(false);
        //                                }

        //                                //using (FrmMainFest frmMainFest = new FrmMainFest(strCollectionReportID, values, criteria2, false) { IDList = strCollectionReportID })
        //                                //{
        //                                //    this.Close();
        //                                //    frmMainFest.ShowDialog();
        //                                //}


        //                            }
        //                            else
        //                                return;

        //                        }
        //                        catch (SqlException sqlError)
        //                        {
        //                            //transaction.Rollback();

        //                            Tripous.Sys.ErrorBox(sqlError);
        //                        }
        //                        catch (Exception ex)
        //                        {
        //                            Tripous.Sys.ErrorBox(ex);
        //                        }
        //                        db.Close();
        //                    }

        //                }
        //                else
        //                    return;

        //            }
        //            else
        //            {
        //                Common.setMessageBox(response.Tables[0].Rows[0]["returnMessage"].ToString(), "", 2);
        //                return;
        //            }




        //        }
        //    }
        //    //load the recport
        //    //using (SqlConnection db = new SqlConnection(Logic.ConnectionString))
        //    //{
        //    //    SqlTransaction transaction;

        //    //    db.Open();

        //    //    transaction = db.BeginTransaction();

        //    //    try
        //    //    {
        //    //        using (System.Data.DataSet dds = new System.Data.DataSet("DsCollectionReport"))
        //    //        {
        //    //            SqlDataAdapter ada;

        //    //            //using (WaitDialogForm form = new WaitDialogForm())
        //    //            //{
        //    //            string strFormat = null;

        //    //            string query = String.Format("SELECT [ID] , [Provider] , [Channel] ,[PaymentRefNumber] , [DepositSlipNumber] , CONVERT(VARCHAR,CONVERT(DATETime,[PaymentDate])) AS PaymentDate,[PayerID] , UPPER([PayerName]) AS [PayerName], [Amount] ,[PaymentMethod] ,[ChequeNumber] ,[ChequeValueDate] ,[ChequeStatus] ,[DateChequeReturned] ,[TelephoneNumber] ,[ReceiptNo] ,[ReceiptDate] ,UPPER([PayerAddress]) as [PayerAddress] ,[User] ,[RevenueCode] , Description , [ChequeBankCode] ,[ChequeBankName] , [AgencyName] ,[AgencyCode] ,[BankCode] ,[BankName] ,[BranchCode] ,[BranchName] ,[ZoneCode] ,[ZoneName] ,[Username] ,[AmountWords] ,[EReceipts] ,[EReceiptsDate] ,[GeneratedBy] ,[StationCode] ,(SELECT TOP 1 StationName from Receipt.tblStation WHERE tblStation.StationCode = Collection.tblCollectionReport.[StationCode]) AS StationName from Collection.tblCollectionReport WHERE PaymentRefNumber IN ({0}) ORDER BY Collection.tblCollectionReport.StationCode , Collection.tblCollectionReport.AgencyCode ,Collection.tblCollectionReport.RevenueCode,Collection.tblCollectionReport.EReceipts  ", values);


        //    //            DataTable Dt = dds.Tables.Add("CollectionReportTable");
        //    //            ada = new SqlDataAdapter(query, Logic.ConnectionString);
        //    //            ada.Fill(dds, "CollectionReportTable");
        //    //            Logic.ProcessDataTable(Dt);;
        //    //            strCollectionReportID = values;
        //    //            //}


        //    //            XRepReceipt recportRec = new XRepReceipt { DataSource = dds.Tables[0] /*recportRec.DataAdapter = ada;*/, DataMember = "CollectionReportTable" };

        //    //            recportRec.ShowPreviewDialog();

        //    //            //selection.ClearSelection();
        //    //            //dds.Clear();
        //    //        }
        //    //        db.Close();
        //    //    }
        //    //    catch (SqlException sqlError)
        //    //    {
        //    //        Tripous.Sys.ErrorBox(sqlError);
        //    //    }

        //    //}
        //}



    }
}
