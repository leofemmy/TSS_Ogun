using Collection.Classess;
using Collection.ReceiptServices;
using Collections;
using DevExpress.XtraSplashScreen;
using System;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Windows.Forms;
using TaxSmartSuite.Class;

namespace Collection.Forms
{
    public partial class FrmCentralData : Form
    {
        public static FrmCentralData publicStreetGroup;

        private SqlDataAdapter adp;

        private SqlCommand command;

        DataTable Dts = new DataTable();
        System.Data.DataSet dataSet = new System.Data.DataSet();
        System.Data.DataSet dataSet2 = new System.Data.DataSet();

        Int32 recCount = 0; int recCount2 = 0;

        System.Data.DataSet dataSet3 = new System.Data.DataSet();

        BackgroundWorker m_oWorker;

        public FrmCentralData()
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

                InitializeComponent();

                setImages();

                ToolStripEvent();

                publicStreetGroup = this;

                btnStart.Click += btnStart_Click;

                btnStop.Click += btnStop_Click;

                btnStartup.Click += btnStartup_Click;

                timer1.Tick += timer1_Tick; timer3.Tick += timer3_Tick;

                switch (Program.intCode)
                {
                    case 13://Akwa Ibom state

                        break;
                    case 32://kogi state
                        break;

                    case 37://ogun state
                        groupControl3.Visible = true;
                        break;

                    case 40://oyo state

                        break;

                    default:
                        break;
                }

                //m_oWorker = new BackgroundWorker() { WorkerReportsProgress = true, WorkerSupportsCancellation = true };

                ////m_oWorker.WorkerReportsProgress = true;

                ////m_oWorker.WorkerSupportsCancellation = true;

                //m_oWorker.DoWork += m_oWorker_DoWork;

                //m_oWorker.ProgressChanged += m_oWorker_ProgressChanged;

                //m_oWorker.RunWorkerCompleted += m_oWorker_RunWorkerCompleted;
            }
            finally
            {
                SplashScreenManager.CloseForm(false);
            }

        }

        void timer3_Tick(object sender, EventArgs e)
        {
            //  doUpload();
        }

        void m_oWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //If it was cancelled midway
            if (e.Cancelled)
            {
                lblStatus.Text = "Task Cancelled.";
                //Common.setMessageBox("Task Cancelled.", "Central Download", 3);
                return;
            }
            else if (e.Error != null)
            {
                lblStatus.Text = "Error while performing background operation.";
                //Common.setMessageBox("Error while performing background operation.", "Central Download", 3);
                return;
            }
            else
            {
                lblStatus.Text = "Task Completed...";
            }
            btnStart.Enabled = true;
            btnStop.Enabled = false;
            //btnStartAsyncOperation.Enabled = true;
            //btnCancel.Enabled = false;
        }

        void m_oWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //Here you play with the main UI thread
            //progressBar1.Value = e.ProgressPercentage;
            ////lblStatus.Text = "Processing......" + progressBar1.Value.ToString() + "%";
            //lblStatus.Text = String.Format("Processing......{0}%", progressBar1.Value);
        }

        void m_oWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            //NOTE : Never play with the UI thread here...

            //time consuming operation
            for (int i = 0; i < 100; i++)
            {
                Thread.Sleep(100);
                m_oWorker.ReportProgress(i);
                uploadReceipts();
                doDownload();
                //If cancel button was pressed while the execution is in progress
                //Change the state from cancellation ---> cancel'ed
                if (m_oWorker.CancellationPending)
                {
                    e.Cancel = true;
                    m_oWorker.ReportProgress(0);
                    return;
                }

            }

            //Report 100% completion on operation completed
            m_oWorker.ReportProgress(100);
        }

        void timer2_Tick(object sender, EventArgs e)
        {
            uploadReceipts();
        }

        void btnStopup_Click(object sender, EventArgs e)
        {
            //btnStopup.Visible = false;

            //btnStartup.Visible = true;

            //timer2.Enabled = false;
        }

        void btnStartup_Click(object sender, EventArgs e)
        {
            timer3.Enabled = true;
        }

        void timer1_Tick(object sender, EventArgs e)
        {

           // uploadReceipts();
            doDownload();
            //if (m_oWorker.IsBusy == false)
            //    m_oWorker.RunWorkerAsync();
        }

        void btnStop_Click(object sender, EventArgs e)
        {
            btnStop.Visible = false;

            btnStart.Enabled = true;

            //if (m_oWorker.IsBusy)
            //{
            //    //Stop/Cancel the async operation here
            //    m_oWorker.CancelAsync();
            //}

            //btnStart.Visible = true;

            timer1.Enabled = false;

        }

        void btnStart_Click(object sender, EventArgs e)
        {
            label6.Text = "Starting....";
            btnStop.Visible = true;
            btnStart.Enabled = true;
            timer1.Enabled = true;

        }

        public void RefreshForm()
        {
            NavBars.ToolStripEnableDisableControls(toolStrip, Tag as String);
        }

        private void setImages()
        {
            tsbNew.Image = MDIMain.publicMDIParent.i16x16.Images[5];
            tsbEdit.Image = MDIMain.publicMDIParent.i16x16.Images[6];
            tsbDelete.Image = MDIMain.publicMDIParent.i16x16.Images[7];
            tsbReload.Image = MDIMain.publicMDIParent.i16x16.Images[0];
            tsbClose.Image = MDIMain.publicMDIParent.i16x16.Images[11];

            //bttnImport.Image = MDIMain.publicMDIParent.i32x32.Images[28];
            //bttnPreview.Image = MDIMain.publicMDIParent.i32x32.Images[8];
            //bttnBrowse.Image = MDIMain.publicMDIParent.i32x32.Images[7];
            //bttnReset.Image = MDIMain.publicMDIParent.i16x16.Images[6];


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
                //Close();
            }

        }

        void doDownload()
        {
            try
            {
                switch (Program.intCode)
                {
                    case 13://Akwa Ibom state
                        using (var receiptAka = new AkwaIbomReceiptServices.ReceiptService())
                        {
                            dataSet = receiptAka.DownloadDataCentral(Program.stateCode);
                        }

                        break;

                    case 20://Delta state
                        using (var receiptDelta = new DeltaBir.ReceiptService())
                        {
                            dataSet = receiptDelta.DownloadDataCentral(Program.stateCode);
                        }
                        break;

                    case 32://kogi state
                        using (var receiptservic = new Kogireceiptservice.ReceiptService())
                        {
                            dataSet = receiptservic.DownloadDataCentral(Program.stateCode);
                        }
                        break;

                    case 37://ogun state
                        using (var receiptsserv = new ReceiptService())
                        {
                            dataSet = receiptsserv.DownloadDataCentral(Program.stateCode);
                        }
                        break;
                    case 40://oyo state

                        using (var receiptsServices = new OyoReceiptServices.ReceiptService())
                        {
                            dataSet = receiptsServices.DownloadDataCentral(Program.stateCode);
                        }
                        break;

                    //http://www.ogunstaterevenue.com/OGS_IPS/ReceiptService.asmx
                    default:
                        break;
                }

                if (dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
                {
                    #region
                    ////insert records into local tabale

                    //if (gridControl1.InvokeRequired)
                    //{
                    //    gridControl1.Invoke(new MethodInvoker(UpdateMainUI));
                    //}

                    ////gridControl1.RefreshDataSource();
                    //if (gridControl1.InvokeRequired)
                    //{
                    //    gridControl1.BeginInvoke(new MethodInvoker(delegate
                    //    {
                    //gridControl1.DataSource = null;
                    //gridControl1.DataSource = dataSet.Tables[0];
                    //gridView1.Columns["Amount"].DisplayFormat.FormatType = FormatType.Numeric;
                    //gridView1.Columns["Amount"].DisplayFormat.FormatString = "n2";
                    //gridView1.Columns["PaymentDate"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
                    //gridView1.Columns["PaymentDate"].DisplayFormat.FormatString = "dd/MM/yyyy";
                    ////gridView1.Columns["Provider"].Visible = false;
                    ////gridView1.Columns["Channel"].Visible = false;
                    //gridView1.Columns["TelephoneNumber"].Visible = false;
                    //gridView1.Columns["ZoneCode"].Visible = false;
                    //gridView1.Columns["ZoneName"].Visible = false;
                    //gridView1.Columns["DateChequeReturned"].Visible = false;
                    //gridView1.Columns["DateValidatedAgainst"].Visible = false;
                    ////gridView1.Columns["PaymentRefNumber"].Visible = false;
                    //gridView1.Columns["DepositSlipNumber"].Visible = false;
                    //gridView1.Columns["PayerID"].Visible = false;
                    //gridView1.Columns["RevenueCode"].Visible = false;
                    //gridView1.Columns["Description"].Visible = false;
                    //gridView1.Columns["PaymentMethod"].Visible = false;
                    //gridView1.Columns["ChequeNumber"].Visible = false;
                    //gridView1.Columns["ChequeValueDate"].Visible = false;

                    //gridView1.Columns["ChequeBankCode"].Visible = false;
                    //gridView1.Columns["ChequeBankName"].Visible = false;
                    //gridView1.Columns["ChequeStatus"].Visible = false;
                    //gridView1.Columns["DateChequeReturned"].Visible = false;
                    //gridView1.Columns["AgencyName"].Visible = false;
                    //gridView1.Columns["AgencyCode"].Visible = false;
                    //gridView1.Columns["BankCode"].Visible = false;
                    //gridView1.Columns["BankName"].Visible = false;
                    //gridView1.Columns["BranchCode"].Visible = false;
                    //gridView1.Columns["BranchName"].Visible = false;
                    //gridView1.Columns["ReceiptNo"].Visible = false;
                    //gridView1.Columns["ReceiptDate"].Visible = false;
                    //gridView1.Columns["PayerAddress"].Visible = false;
                    ////gridView1.Columns["AmountWords"].Visible = false;
                    ////gridView1.Columns["GeneratedBy"].Visible = false;
                    //gridView1.Columns["DateValidatedAgainst"].Visible = false;
                    //gridView1.Columns["StationCode"].Visible = false;

                    //gridView1.BestFitColumns();
                    //    }));
                    //}
                    //gridControl1.RefreshDataSource();
                    #endregion

                    dataSet2 = InsertData(dataSet);

                    if (dataSet2.Tables.Count > 0 && dataSet2.Tables[0].Rows.Count > 0)
                    {


                        //if (dataSet2.Tables[0].Rows[0]["returnCode"].ToString() == "-1")
                        //{
                        //    Common.setMessageBox(string.Format("{0}...Error Occur During Data Insert After download.... Insert Download", dataSet2.Tables[0].Rows[0]["returnMessage"]), Program.ApplicationName, 3);

                        //    timer1.Stop();
                        //    timer1.Enabled = false;
                        //    btnStop.Enabled = false;
                        //    btnStart.Enabled = true;
                        //    //m_oWorker.CancelAsync();
                        //    return;
                        //}
                        //else
                        {
                            dataSet.Clear();
                            switch (Program.intCode)
                            {
                                case 13://Akwa Ibom state
                                    using (var receiptAka = new AkwaIbomReceiptServices.ReceiptService())
                                    {
                                        dataSet = receiptAka.DownloadDataUpdateCentral(dataSet2, Program.stateCode);
                                    }
                                    break;
                                case 20://Delta state
                                    using (var receiptDelta = new DeltaBir.ReceiptService())
                                    {
                                        dataSet = receiptDelta.DownloadDataUpdateCentral(dataSet2, Program.stateCode);
                                    }
                                    break;
                                case 32://kogi state
                                    break;

                                case 37://ogun state

                                    using (var receiptsserv = new ReceiptService())
                                    {
                                        dataSet = receiptsserv.DownloadDataUpdateCentral(dataSet2, Program.stateCode);
                                    }

                                    break;

                                case 40://oyo state

                                    using (var receiptsServices = new OyoReceiptServices.ReceiptService())
                                    {
                                        dataSet = receiptsServices.DownloadDataUpdateCentral(dataSet2, Program.stateCode);
                                    }


                                    break;

                                default:
                                    break;
                            }

                            if (dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
                            {
                                label6.Text = "Download In Progress";

                                if (dataSet.Tables[0].Rows[0]["returnCode"].ToString() == "00")
                                {

                                    lblAll.Text = dataSet.Tables[1].Rows[0]["ALLRecords"].ToString();

                                    lblDownload.Text = dataSet.Tables[2].Rows[0]["DownloadedRecords"].ToString();

                                    lblRemain.Text = dataSet.Tables[3].Rows[0]["RemainRecords"].ToString();

                                    lblError.Text = dataSet.Tables[4].Rows[0]["ErrorRecords"].ToString();

                                }
                                else
                                {
                                    timer1.Stop();
                                    Common.setMessageBox(string.Format("{0}...Download Data to Local Station,Update Data Download", dataSet.Tables[0].Rows[0]["returnMessage"]), Program.ApplicationName, 3);
                                    timer1.Enabled = false;

                                    return;
                                }
                            }

                        }
                    }

                }
                else
                {
                    label6.Text = "No More Records to Download";


                    timer1.Stop();
                    timer1.Enabled = false;

                    return;
                }


            }
            catch (Exception e)
            {
                timer1.Stop();
                Common.setMessageBox(string.Format("{0}----{1}..Do Down load to station", e.Message, e.StackTrace), Program.ApplicationName, 3); timer1.Enabled = false;

                return;
            }

        }

        void uploadReceipts()
        {
            using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
            {
                connect.Open();

                command = new SqlCommand("doGetReceiptIssue", connect) { CommandType = CommandType.StoredProcedure, CommandTimeout = 0 };

                try
                {
                    using (System.Data.DataSet ds = new System.Data.DataSet())
                    {
                        adp = new SqlDataAdapter(command);
                        adp.SelectCommand.CommandTimeout = 0;
                        adp.Fill(ds);
                        Dts = ds.Tables[0];
                        connect.Close();

                        //if (ds.Tables[0].Rows.Count > 0)
                        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            System.Data.DataSet dataSet = new System.Data.DataSet();

                            switch (Program.intCode)
                            {
                                case 13://Akwa Ibom state
                                    using (var receiptAka = new AkwaIbomReceiptServices.ReceiptService())
                                    {
                                        dataSet = receiptAka.uploadIssueReceipt(ds, Program.stateCode);
                                    }
                                    break;
                                case 20://Delta state
                                    using (var receiptDelta = new DeltaBir.ReceiptService())
                                    {
                                        dataSet = receiptDelta.uploadIssueReceipt(ds, Program.stateCode);
                                    }
                                    break;
                                case 32://kogi state
                                    break;

                                case 37://ogun state
                                    using (var receiptsserv = new ReceiptService())
                                    {
                                        dataSet = receiptsserv.uploadIssueReceipt(ds, Program.stateCode);
                                    }
                                    break;

                                case 40://oyo state
                                    using (var receiptsServices = new OyoReceiptServices.ReceiptService())
                                    {
                                        dataSet = receiptsServices.uploadIssueReceipt(ds, Program.stateCode);
                                    }
                                    break;

                                default:
                                    break;
                            }

                            UploadReceiptIssueUpdate(dataSet);

                        }
                        else
                        {
                            return;
                        }

                    }

                }
                catch (Exception e)
                {
                    timer1.Stop();
                    Common.setMessageBox(string.Format("{0}----{1} Error Occur Sending Record Online", e.Message, e.StackTrace), Program.ApplicationName, 3);
                    timer1.Enabled = false; btnStop.Visible = false; btnStart.Visible = true;
                    //m_oWorker.CancelAsync();
                    return;
                }
            }
        }

        public System.Data.DataSet InsertData(System.Data.DataSet dataSet)
        {
            System.Data.DataSet ds = new System.Data.DataSet();

            try
            {
                recCount = dataSet.Tables[0].Rows.Count;

                using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
                {
                    connect.Open();

                    command = new SqlCommand("doInsertCentralReceipt", connect) { CommandType = CommandType.StoredProcedure };
                    command.Parameters.Add(new SqlParameter("@tblCollectionReport_Temp", SqlDbType.Structured)).Value = dataSet.Tables[0];
                    command.CommandTimeout = 0;

                    adp = new SqlDataAdapter(command);
                    adp.SelectCommand.CommandTimeout = 0;
                    adp.Fill(ds);
                    Dts = ds.Tables[0];
                    connect.Close();

                }


                return ds;
            }
            catch (Exception ex)
            {
                Common.setMessageBox(string.Format("{0}----{1}", ex.Message, ex.StackTrace), Program.ApplicationName, 3);
                timer1.Enabled = false;
                btnStop.Visible = false;
                btnStart.Visible = true;
                //m_oWorker.CancelAsync();
                return ds;
            }
            return ds;
        }

        public System.Data.DataSet UploadReceiptIssueUpdate(System.Data.DataSet dataSet)
        {
            System.Data.DataSet ds = new System.Data.DataSet();

            try
            {
                using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
                {
                    connect.Open();

                    command = new SqlCommand("doGetReceiptIssue_Update", connect) { CommandType = CommandType.StoredProcedure };

                    command.Parameters.Add(new SqlParameter("@returnIssueTable_Temp", SqlDbType.Structured)).Value = dataSet.Tables[0];

                    command.CommandTimeout = 0;

                    adp = new SqlDataAdapter(command);

                    adp.SelectCommand.CommandTimeout = 0;

                    adp.Fill(ds);

                    Dts = ds.Tables[0];

                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        if (ds.Tables[0].Rows[0]["returnCode"].ToString() == "00")
                        {
                            connect.Close();
                        }
                        else
                        {
                            Common.setMessageBox(ds.Tables[0].Rows[0]["returnMessage"].ToString(), Program.ApplicationName, 3);
                            connect.Close();
                        }
                    }

                }


                return dataSet;
            }

            catch (Exception ex)
            {
                Common.setMessageBox(String.Format("{0}....{1}..Error Occur while Update Issues receipts Databae on {2} Server", ex.Message, ex.StackTrace, Program.ServerName), Program.ApplicationName, 3);
                timer1.Enabled = false;
                btnStop.Visible = false;
                //m_oWorker.CancelAsync();
                btnStart.Visible = true;

                return dataSet;
            }


            return dataSet;
        }

        void doUpload()
        {
            try
            {
                //SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

                using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
                {
                    connect.Open();

                    command = new SqlCommand("doGetStationPrintedReceipt", connect) { CommandType = CommandType.StoredProcedure };

                    command.CommandTimeout = 0;

                    using (System.Data.DataSet ds = new System.Data.DataSet())
                    {
                        adp = new SqlDataAdapter(command);

                        adp.SelectCommand.CommandTimeout = 0;

                        adp.Fill(ds);

                        Dts = ds.Tables[0];

                        connect.Close();

                        if (ds.Tables[0].Rows.Count < 1)
                        {

                            label8.Text = "No More Records to Upload";

                            timer3.Stop(); timer3.Enabled = false;

                            return;
                        }
                        else
                        {
                            System.Data.DataSet dataSet = new System.Data.DataSet();


                            switch (Program.intCode)
                            {
                                case 13://Akwa Ibom state
                                    using (var receiptAka = new AkwaIbomReceiptServices.ReceiptService())
                                    {
                                        dataSet3 = receiptAka.UploadStationPrintedReceipts(ds, Program.stateCode);
                                    }

                                    break;
                                case 20://Delta state
                                    using (var receiptDelta = new DeltaBir.ReceiptService())
                                    {
                                        dataSet = receiptDelta.UploadStationPrintedReceipts(ds, Program.stateCode);
                                    }
                                    break;
                                case 32://kogi state
                                    break;

                                case 37://ogun state
                                    using (var receiptsserv = new ReceiptService())
                                    {
                                        dataSet = receiptsserv.UploadStationPrintedReceipts(ds, Program.stateCode);
                                    }
                                    break;
                                case 40://oyo state
                                    using (var receiptsServices = new OyoReceiptServices.ReceiptService())
                                    {
                                        dataSet = receiptsServices.UploadStationPrintedReceipts(ds, Program.stateCode);
                                    }
                                    break;
                                default:
                                    break;
                            }


                            if (String.Compare(dataSet.Tables[0].Rows[0]["rownumber"].ToString(), "-1", false) == 0)
                            {
                                timer3.Stop();
                                Common.setMessageBox((string)dataSet.Tables[0].Rows[0]["errorMessage"], Program.ApplicationName, 3);
                                timer3.Enabled = false;
                                //btnStop.Visible = false; btnStartup.Visible = true;
                                //m2_Worker.CancelAsync();
                                return;
                            }
                            else
                            {
                                UploadRecordsUpdate(dataSet);
                            }
                            //}
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                timer2.Stop();
                Common.setMessageBox(String.Format("{0}----{1}...Do upload from Station to Online", ex.StackTrace, ex.Message), Program.ApplicationName, 3);// m2_Worker.CancelAsync();
                timer2.Enabled = false;
                //btnStop.Visible = false; btnStartup.Visible = true;
                return;
            }
            finally
            {
                //SplashScreenManager.CloseForm(false);
            }
        }

        private System.Data.DataSet UploadRecordsUpdate(System.Data.DataSet dataSet)
        {
            System.Data.DataSet ds = new System.Data.DataSet();

            try
            {
                using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
                {
                    connect.Open();

                    command = new SqlCommand("doGetStationPrintedReceipt_Update", connect) { CommandType = CommandType.StoredProcedure };

                    command.Parameters.Add(new SqlParameter("@returnTable_Temp", SqlDbType.Structured)).Value = dataSet.Tables[0];

                    command.Parameters.Add(new SqlParameter("@userid", SqlDbType.VarChar)).Value = Program.UserID;

                    command.CommandTimeout = 0;


                    adp = new SqlDataAdapter(command);

                    adp.SelectCommand.CommandTimeout = 0;

                    adp.Fill(ds);

                    Dts = ds.Tables[0];

                    connect.Close();

                    recCount2 = ds.Tables[0].Rows.Count;

                    if (ds.Tables.Count == 1)
                    {
                        Common.setMessageBox(ds.Tables[0].Rows[0]["returnMessage"].ToString(), Program.ApplicationName, 3);
                        timer3.Enabled = false;

                    }
                    else
                    {
                        lblAllUp.Text = ds.Tables[0].Rows[0]["ALLRecords"].ToString();

                        lblRemainUp.Text = ds.Tables[2].Rows[0]["RemainRecords"].ToString();

                        lblErrup.Text = ds.Tables[3].Rows[0]["ErrorRecords"].ToString();

                    }
                    //}
                }


                return dataSet;
            }
            catch (Exception ex)
            {
                Common.setMessageBox(String.Format("{0}....{1}..Do update uploadRecords On Station", ex.Message, ex.StackTrace), Program.ApplicationName, 3);
                timer3.Enabled = false;
                //btnStop.Visible = false;
                //btnStartup.Visible = true;
                //m2_Worker.CancelAsync();
                return dataSet;
            }
            return dataSet;
        }

    }
}
