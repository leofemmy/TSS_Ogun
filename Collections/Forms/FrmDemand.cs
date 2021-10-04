using Collection.Classess;
using Collection.ReceiptServices;
using Collection.Report;
using DevExpress.Utils;
using DevExpress.XtraGrid.Selection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TaxSmartSuite.Class;
using DevExpress.XtraReports.UI;
using DevExpress.XtraReports.Parameters;
using DevExpress.XtraSplashScreen;
using Collections;

namespace Collection.Forms
{
    public partial class FrmDemand : Form
    {
        private SqlCommand _command;

        AmountToWords amounttowords = new AmountToWords();

        public static FrmDemand publicStreetGroup;

        protected TransactionTypeCode iTransType;

        protected bool boolIsUpdate;

        System.Data.DataSet dsreturn = new System.Data.DataSet();

        System.Data.DataSet dsGetapp = new System.Data.DataSet();

        System.Data.DataSet dsGetappup = new System.Data.DataSet();

        System.Data.DataSet dsf = new System.Data.DataSet();

        System.Data.DataSet dsD = new System.Data.DataSet();

        GridCheckMarksSelection selection;

        string BatchNumber, values, values1, values2, query, criteria3, criteria2;

        System.Data.DataSet dstretval = new System.Data.DataSet();

        private SqlDataAdapter adp; DataTable dtsed = new DataTable();

        string criteria, strreprint, strCollectionReportID;

        DataTable temTable = new DataTable();

        private SqlCommand command;

        bool isFirstGrid = true;
        public FrmDemand()
        {
            InitializeComponent();

            publicStreetGroup = this;

            iTransType = TransactionTypeCode.New;

            setImages();

            ToolStripEvent();

            Load += OnFormLoad;

            temTable.Columns.Add("EReceipt", typeof(string));
            temTable.Columns.Add("PaymentRefNumber", typeof(string));
            temTable.Columns.Add("UserId", typeof(string));

            dtsed.Columns.Add("SN", typeof(int));
            dtsed.Columns.Add("PaymentRef", typeof(string));

            //gridControl2.DataSource = dtsed;
            //gridView3.Columns["SN"].OptionsColumn.AllowEdit = false;
            //gridView3.Columns["SN"].Width = 30;
            //gridView3.Columns["PaymentRef"].Width = 500;


            OnFormLoad(null, null);
        }

        private void setImages()
        {
            tsbNew.Image = MDIMain.publicMDIParent.i16x16.Images[5];
            tsbEdit.Image = MDIMain.publicMDIParent.i16x16.Images[6];
            tsbDelete.Image = MDIMain.publicMDIParent.i16x16.Images[7];
            tsbReload.Image = MDIMain.publicMDIParent.i16x16.Images[0];
            tsbClose.Image = MDIMain.publicMDIParent.i16x16.Images[11];

            //bttnCancel.Image = MDIMain.publicMDIParent.i32x32.Images[9];
            //btnPrint.Image = MDIMain.publicMDIParent.i32x32.Images[29];
            btnSend.Image = MDIMain.publicMDIParent.i32x32.Images[10];
            btnGet.Image = MDIMain.publicMDIParent.i32x32.Images[2];
            //btnPrint.Image = MDIMain.publicMDIParent.i32x32.Images[29];
            //btnMain.Image = MDIMain.publicMDIParent.i32x32.Images[8];


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
            //isReprint = false;

            ShowForm();

            setReload();

            btnSend.Click += btnSend_Click;

            btnGet.Click += btnGet_Click;

            btnPrint.Click += btnPrint_Click;

            btnMain.Click += btnMain_Click;

            //gridView3.CustomColumnDisplayText += GridView3_CustomColumnDisplayText;
        }

        //private void GridView3_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        //{
        //    DevExpress.XtraGrid.Columns.GridColumn Col = e.Column;
        //    if (Col.Name == "colSN")
        //    { // without " before and after yourColName
        //        if (e.ListSourceRowIndex >= 0)
        //        {
        //            e.DisplayText = (e.ListSourceRowIndex + 1).ToString();
        //        }
        //    }
        //}

        void btnMain_Click(object sender, EventArgs e)
        {
            if (selection.SelectedCount == 0 || string.IsNullOrEmpty(values))
            {
                Common.setMessageBox("No Selection Made for Printing of Receipts", Program.ApplicationName, 3);
                return;
            }
            else
            {
                using (FrmMainFest frmMainFest = new FrmMainFest(strCollectionReportID, values, criteria2, false) { IDList = strCollectionReportID })
                {
                    frmMainFest.ShowDialog();
                }
            }

        }

        void btnPrint_Click(object sender, EventArgs e)
        {
            values = string.Empty;

            lblSelect.Text = string.Empty;

            int j = 0;

            if (selection.SelectedCount == 0)
            {
                Common.setMessageBox("No Selection Made for Printing of Receipts", Program.ApplicationName, 3);
                return;
            }
            else
            {
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
                    _command.Parameters.Add(new SqlParameter("@type", SqlDbType.VarChar)).Value = "New";
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

                                    using (WaitDialogForm form = new WaitDialogForm())
                                    {
                                        string strFormat = null;

                                        query = string.Format("SELECT [ID] ,PaymentPeriod, [Provider] , [Channel] ,[PaymentRefNumber] , [DepositSlipNumber] , CONVERT(VARCHAR,CONVERT(DATETime,[PaymentDate])) AS PaymentDate,[PayerID] , UPPER([PayerName]) AS [PayerName], [Amount] ,[PaymentMethod] ,[ChequeNumber] ,[ChequeValueDate] ,[ChequeStatus] ,[DateChequeReturned] ,[TelephoneNumber] ,[ReceiptNo] ,[ReceiptDate] ,UPPER([PayerAddress]) as [PayerAddress] ,[User] ,[RevenueCode] , tblCollectionReport.Description , [ChequeBankCode] ,[ChequeBankName] ,[AgencyName] ,[AgencyCode] ,[BankCode] ,[BankName] ,[BranchCode] ,[BranchName] ,[ZoneCode] ,[ZoneName] ,[Username] ,[AmountWords] ,[EReceipts] ,[EReceiptsDate] ,[GeneratedBy] ,[StationCode] ,(SELECT StationName FROM Receipt.tblStation  WHERE tblStation.StationCode = Collection.tblCollectionReport.[StationCode]) AS StationName, Symbol , Surfix , tblCurrency.Description AS prefix from Collection.tblCollectionReport  JOIN Reconciliation.tblCurrency ON tblCurrency.CurrencyCode = tblCollectionReport.CurrencyCode WHERE PaymentRefNumber IN ({0})  ORDER BY Collection.tblCollectionReport.StationCode , Collection.tblCollectionReport.AgencyCode ,Collection.tblCollectionReport.RevenueCode,Collection.tblCollectionReport.EReceipts", values);

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
                                        case 20://detla state

                                            XtraRepReceiptDelta delta = new XtraRepReceiptDelta { DataSource = dds.Tables[0] /*recportRec.DataAdapter = ada;*/, DataMember = "CollectionReportTable" };
                                            //XRepReceipto/*yo delta = new XRepReceiptoyo { DataSource = dds.Tables[0] /*recportRec.DataAdapter = ada;*/, DataMember = "CollectionReportTable" };*/
                                            ////delta.PrintingSystem.StartPrint += PrintingSystem_StartPrint;
                                            delta.logoPath = Logic.singaturepth;
                                            delta.ShowPreviewDialog();
                                            break;

                                        case 37://ogun state
                                            XRepReceipt recportRec = new XRepReceipt { DataSource = dds.Tables[0] /*recportRec.DataAdapter = ada;*/, DataMember = "CollectionReportTable" }; recportRec.logoPath = Logic.singaturepth;
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
                        //    //ask if the print was sucessfull
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
                                        string query1 = String.Format("DELETE FROM Receipt.tblCollectionReceipt WHERE PaymentRefNumber IN (SELECT PaymentRefNumber FROM Receipt.tblReceipt WHERE PaymentRefNumber in ({0})", values);

                                        using (SqlCommand sqlCommand = new SqlCommand(query1, db, transaction))
                                        {
                                            sqlCommand.ExecuteNonQuery();
                                        }

                                        string query = String.Format("DELETE  FROM Receipt.tblReceipt where PaymentRefNumber in ({0})", values);

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
                    else
                    {
                        if (String.Compare(response.Tables[0].Rows[0]["returnCode"].ToString(), "-1", false) == 0)
                        {
                            Common.setMessageBox(response.Tables[0].Rows[0]["returnMessage"].ToString(), "", 2);
                            //return;

                            if (response.Tables[0].Rows[0]["returnMessage"].ToString() != "Payment Ref. Number Already Exist")
                            {
                                //using (Frmcontrol frmcontrol = new Frmcontrol())
                                //{
                                //    frmcontrol.gridControl1.DataSource = response.Tables[1];
                                //    frmcontrol.gridView1.BestFitColumns();
                                //    frmcontrol.label1.Text = "Payment Ref. Number Already been used";
                                //    frmcontrol.Text = "Payment Ref. Number Already been used";
                                //    frmcontrol.ShowDialog();
                                //}
                                return;
                            }
                            else
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

                        }
                        else
                        {
                            Common.setMessageBox(response.Tables[0].Rows[0]["returnMessage"].ToString(), "", 2);
                            return;
                        }
                    }
                }
            }
        }

        void btnGet_Click(object sender, EventArgs e)
        {

            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

                switch (Program.intCode)
                {
                    case 32://kogi state
                        break;

                    case 13://Akwa Ibom state
                        using (var akwa = new AkwaIbomReceiptServices.ReceiptService())
                        {
                            dsGetapp = akwa.GetReceipt_OnDemand(Program.stationCode);
                        }
                        break;

                    case 37://ogun state
                        if (dsD.Tables[0] != null && dsD.Tables[0].Rows.Count >= 1)
                        {
                            using (var receiptsserv = new ReceiptService())
                            {
                                dsGetapp = receiptsserv.GetReceipt_OnDemand(dsD);
                            }
                        }
                        else
                        {
                            Common.setMessageBox("No record to Fetch from", "Demand Receipt", 3);
                            return;
                        }

                        break;

                    case 40://oyo state
                        using (var receiptsServices = new OyoReceiptServices.ReceiptService())
                        {
                            dsreturn = receiptsServices.GetReceipt_OnDemand(Program.stationCode);

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

            if (dsGetapp.Tables[0].Rows.Count < 1)
            {
                //Common.setMessageBox(dsGetapp.Tables[0].Rows[0]["returnmessage"].ToString(), Program.ApplicationName, 1);
                Common.setMessageBox("No Approved Record Found", Program.ApplicationName, 1);
                return;
            }
            else
            {
                dstretval = InsertData(dsGetapp);

                try
                {
                    SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);


                    switch (Program.intCode)
                    {
                        case 13://Akwa Ibom state
                            using (var akwa = new AkwaIbomReceiptServices.ReceiptService())
                            {
                                dsGetapp = akwa.GetReceiptUpdate_OnDemand(dstretval);
                            }
                            break;

                        case 32://kogi state
                            break;

                        case 37://ogun state
                            using (var receiptsserv = new ReceiptService())
                            {
                                dsGetappup = receiptsserv.GetReceiptUpdate_OnDemand(dstretval);
                            }

                            break;

                        case 40://oyo state
                            using (var receiptsServices = new OyoReceiptServices.ReceiptService())
                            {
                                dsreturn = receiptsServices.GetReceiptUpdate_OnDemand(dstretval);

                            }
                            break;

                        default:
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Common.setMessageBox(ex.StackTrace, "Error Update Online Records", 3); return;
                }
                finally
                {
                    SplashScreenManager.CloseForm(false);
                }

                if (dsGetappup.Tables[0].Rows[0]["returnCode"].ToString() == "00")
                {
                    iTransType = TransactionTypeCode.Edit;
                    btnPrint.Enabled = true;
                    voidgetRec(dstretval); setReload();
                    ShowForm();
                    return;
                }
                else
                {
                    Common.setMessageBox(string.Format("Error Updating Approval Record, {0}...", dsGetappup.Tables[0].Rows[0]["returnMessage"]), Program.ApplicationName, 3);
                    return;
                }
            }


        }
        //*929*1#
        //*229*1#
        void btnSend_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtPay.Text.ToString()))
            {
                Common.setMessageBox("No Record Entered", "Receipt Demand", 3);
                return;
            }
            else
            {
                //inssert into demanadreceipt table passing temp table

                try
                {
                    SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);



                    switch (Program.intCode)
                    {
                        case 13://Akwa Ibom state
                            using (var akwa = new AkwaIbomReceiptServices.ReceiptService())
                            {
                                dsGetapp = akwa.ReceiptRequestLog_OnDemand(txtPay.Text.Trim(), Program.stationCode, string.Format("{0} - {1}", Program.UserID, Program.UserName));
                            }
                            break;

                        case 32://kogi state
                            break;

                        case 37://ogun state
                            using (var receiptsserv = new ReceiptService())
                            {
                                dsreturn = receiptsserv.ReceiptRequestLog_OnDemand(txtPay.Text.Trim(), Program.stationCode, string.Format("{0} - {1}", Program.UserID, Program.UserName));
                            }

                            break;

                        case 40://oyo state
                            using (var receiptsServices = new OyoReceiptServices.ReceiptService())
                            {
                                dsreturn = receiptsServices.ReceiptRequestLog_OnDemand(txtPay.Text.Trim(), Program.stationCode, string.Format("{0} - {1}", Program.UserID, Program.UserName));

                            }
                            break;

                        default:
                            break;
                    }

                    if (dsreturn.Tables[0].Rows[0]["returncode"].ToString() == "-1")
                    {
                        Common.setMessageBox(dsreturn.Tables[0].Rows[0]["returnmessage"].ToString(), Program.ApplicationName, 1); txtPay.Text = string.Empty;
                        return;
                    }
                    else
                    {
                        using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
                        {
                            connect.Open();
                            _command = new SqlCommand("dboLogDemandReceipt", connect) { CommandType = CommandType.StoredProcedure };
                            _command.Parameters.Add(new SqlParameter("@userid", SqlDbType.VarChar)).Value = Program.UserID;
                            _command.Parameters.Add(new SqlParameter("@payer", SqlDbType.VarChar)).Value = txtPay.Text.ToString().Trim();
                            //_command.Parameters.Add(new SqlParameter("@pTransaction", SqlDbType.Structured)).Value = dtsed;

                            _command.CommandTimeout = 0;
                            //@Years
                            using (System.Data.DataSet ds = new System.Data.DataSet())
                            {
                                ds.Clear();
                                adp = new SqlDataAdapter(_command);
                                adp.Fill(ds);
                                connect.Close();

                                if (ds.Tables[0].Rows[0]["returnCode"].ToString() != "00")
                                {
                                    Common.setMessageBox(ds.Tables[0].Rows[0]["returnMessage"].ToString(), Program.ApplicationName, 2); txtPay.Text = string.Empty;
                                    return;
                                }
                                else
                                {


                                    Common.setMessageBox(dsreturn.Tables[0].Rows[0]["returnmessage"].ToString(), Program.ApplicationName, 1);

                                  
                                    txtPay.Text = string.Empty; setReload();
                                    return;

                                }


                            }
                        }
                    }
                }
                catch (Exception ex)
                {

                    Common.setMessageBox(string.Format("Please Connect to the Internet. Sending Request Failed"), Program.ApplicationName, 3);
                    return;
                }
                finally
                {
                    SplashScreenManager.CloseForm(false);
                }


            }

            //    }
            //    //}

            //}

            //if (dtsed != null && dtsed.Rows.Count >= 1)
            //{
            //    if (dtsed.Columns.Contains("SN")) dtsed.Columns.Remove("SN");



            //}
            //else
            //{
            //    Common.setMessageBox("No Record Entered", "Receipt Demand", 3);
            //    return;

            //}
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

        public void RefreshForm()
        {
            NavBars.ToolStripEnableDisableControls(toolStrip, Tag as String);
        }

        public System.Data.DataSet InsertData(System.Data.DataSet dataSet)
        {
            System.Data.DataSet ds = new System.Data.DataSet();

            if (dataSet.Tables[0] != null && dataSet.Tables[0].Rows.Count > 0)
            {
                try
                {
                    using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
                    {
                        connect.Open();

                        command = new SqlCommand("doInsertStationReceipt", connect) { CommandType = CommandType.StoredProcedure };
                        command.Parameters.Add(new SqlParameter("@tblCollectionReport_Temp", SqlDbType.Structured)).Value = dataSet.Tables[0];

                        adp = new SqlDataAdapter(command);
                        adp.Fill(ds);
                        //Dts = ds.Tables[0];
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
            }
            return ds;
        }

        void voidgetRec(System.Data.DataSet dt)
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                gridControl4.DataSource = null;
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


                string quy = String.Format("SELECT ID,PaymentRefNumber,DepositSlipNumber,PaymentDate,UPPER(PayerName) as PayerName,Description,RevenueCode,Amount,BankName+ '-'+ BranchName AS Bank,EReceipts from Collection.tblCollectionReport WHERE Collection.tblCollectionReport.PaymentRefNumber IN ({0}) ORDER BY StationCode , AgencyCode ,RevenueCode,EReceipts", values);


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
                        gridControl4.DataSource = ds.Tables[0];
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
            catch (Exception)
            {

                throw;
            }
            finally
            {
                SplashScreenManager.CloseForm(false);
            }

        }

        void ProcessDataTable(DataTable Dt)
        {
            if (Dt != null && Dt.Rows.Count > 0)
            {
                Dt.Columns.Add("URL", typeof(string));
                Dt.AcceptChanges();

                foreach (DataRow item in Dt.Rows)
                {
                    //if (item == null) continue;
                    ////decimal amount = decimal.Parse(item["Amount"].ToString());
                    //try
                    //{
                    //    item["AmountWords"] = amounttowords.convertToWords(item["Amount"].ToString());

                    //    if (item["PayerID"].ToString().Length > 14)
                    //    {
                    //        item["PayerID"] = "None Yet Please approach the BIR for your Unique Payer ID.";
                    //    }
                    //    else
                    //    {
                    //        item["PayerID"] = item["PayerID"].ToString();
                    //        item["PayerID"] = string.Format("Your Payer ID which is <<{0}>> must be quoted in all transaction", item["PayerID"]);
                    //    }

                    //    item["ZoneCode"] = item["StationCode"].ToString();
                    //    item["ZoneName"] = item["StationName"].ToString();
                    //    //item["URL"] = string.Format(@"Payment for {0} {1} << Paid at {2} - {3} , Deposit Slip Number {4} by {5}  >> ", item["Description"], item["RevenueCode"], item["BankName"], item["BranchName"], item["DepositSlipNumber"], item["PaymentMethod"]);

                    //    item["URL"] = string.Format("Paid at {0} , {1}  Deposit Slip Number {2} in respect of {3} <{4}> By {5}", item["BankName"], item["BranchName"], item["DepositSlipNumber"], item["Description"], item["RevenueCode"], item["PaymentMethod"]);

                    //    item["User"] = Program.UserID.ToUpper();

                    //    item["Username"] = string.Format(@"</Printed at {0} Zonal Office  by {1} on {2}/>", item["StationName"], Program.UserID, DateTime.Today.ToString("dd-MMM-yyyy"));

                    //    item["PaymentDate"] = Convert.ToDateTime(item["PaymentDate"]).ToString("dd-MMM-yyyy");
                    //}
                    //catch
                    //{

                    //}
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
                                item["URL"] = string.Format("Being: Payment for [{0}/{1}] paid at [{2}/{3}], Slip Number [{4}] by [{5}]", item["Description"], item["RevenueCode"], item["BankName"], item["BranchName"], item["DepositSlipNumber"], item["PaymentMethod"]);
                                break;


                            case 37://ogun state
                                item["URL"] = string.Format("Paid at {0} , {1}  Deposit Slip Number {2} in respect of {3} <{4}> By {5}", item["BankName"], item["BranchName"], item["DepositSlipNumber"], item["Description"], item["RevenueCode"], item["PaymentMethod"]);
                                break;

                            case 40://oyo state
                                item["URL"] = string.Format("Payment for [{0}/{1}] paid at [{2}/{3}], Slip Number [{4}] by [{5}]", item["Description"], item["RevenueCode"], item["BankName"], item["BranchName"], item["DepositSlipNumber"], item["PaymentMethod"]);
                                break;

                            case 32://kogi state
                                item["URL"] = string.Format("Paid at {0} , {1}  Deposit Slip Number {2} in respect of {3} <{4}> By {5}", item["BankName"], item["BranchName"], item["DepositSlipNumber"], item["Description"], item["RevenueCode"], item["PaymentMethod"]);
                                break;

                            default:
                                break;
                        }

                        item["User"] = Program.UserID.ToUpper();

                        item["Username"] = string.Format(@"</Printed at {0} Zonal Office  by {1} on {2}/>", item["StationName"], Program.UserID.ToUpper(), DateTime.Today.ToString("dd-MMM-yyyy"));

                        item["PaymentDate"] = Convert.ToDateTime(item["PaymentDate"]).ToString("dd-MMM-yyyy");
                    }
                    catch
                    {

                    }
                }
            }
        }

        private void setReload()
        {
            try
            {
                dsD = new System.Data.DataSet();

                using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
                {
                    connect.Open();
                    _command = new SqlCommand("doReloadDemandReceiptRequest", connect) { CommandType = CommandType.StoredProcedure };
                    _command.CommandTimeout = 0;
                    using (System.Data.DataSet dss = new System.Data.DataSet())
                    {
                        dsD.Clear();
                        dss.Clear();
                        adp = new SqlDataAdapter(_command);
                        adp.Fill(dss);
                        connect.Close();


                        if (dss.Tables[0].Rows[0]["returnCode"].ToString() != "00")
                        {
                            Common.setMessageBox(dss.Tables[0].Rows[0]["returnMessage"].ToString(), Program.ApplicationName, 2);
                            return;
                        }
                        else
                        {
                            gridControl1.DataSource = dss.Tables[1];


                            DataTable dtts = dss.Tables[1].Clone();
                            foreach (DataRow row in dss.Tables[1].Rows)
                                dtts.ImportRow(row);

                            dsD.Tables.Add(dtts);

                            gridView2.OptionsBehavior.Editable = false;

                            gridView2.OptionsView.ColumnAutoWidth = false;
                            gridView2.OptionsView.ShowFooter = true;


                        }

                    }
                }
            }
            catch (Exception ex)
            {
                Tripous.Sys.ErrorBox(String.Format("{0}{1}", ex.Message, ex.StackTrace)); return;
            }
        }


    }
}
