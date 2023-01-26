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
    public partial class FrmStationDownload : Form
    {
        public static FrmStationDownload publicStreetGroup;

        private SqlDataAdapter adp;

        private SqlCommand command;

        DataTable Dts = new DataTable();

        BackgroundWorker m_oWorker;

        BackgroundWorker m2_Worker;

        Int32 recCount = 0; int recCount2 = 0;

        System.Data.DataSet dataSet = new System.Data.DataSet();

        System.Data.DataSet dataSet2 = new System.Data.DataSet();

        System.Data.DataSet dataSet3 = new System.Data.DataSet();

        bool isTimer1Call;

        public FrmStationDownload()
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

                isTimer1Call = false;

                InitializeComponent();

                setImages();

                ToolStripEvent();

                publicStreetGroup = this;

                btnStart.Click += btnStart_Click;

                timer1.Tick += timer1_Tick;

                btnStop.Click += btnStop_Click;

                btnStartup.Click += btnStartup_Click;

                btnStopup.Click += btnStopup_Click;

                timer2.Tick += timer2_Tick;

                //string val = Logic.GetMACAddress;

                //m_oWorker = new BackgroundWorker() { WorkerReportsProgress = true, WorkerSupportsCancellation = true };

                //m2_Worker = new BackgroundWorker() { WorkerReportsProgress = true, WorkerSupportsCancellation = true };

                //m_oWorker.DoWork += m_oWorker_DoWork;

                //m_oWorker.ProgressChanged += m_oWorker_ProgressChanged;

                //m_oWorker.RunWorkerCompleted += m_oWorker_RunWorkerCompleted;

                //m2_Worker.DoWork += m2_Worker_DoWork;

                //m2_Worker.RunWorkerCompleted += m2_Worker_RunWorkerCompleted;

                //m2_Worker.ProgressChanged += m2_Worker_ProgressChanged;

            }
            finally
            {
                SplashScreenManager.CloseForm(false);
            }


        }

        void m2_Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //Here you play with the main UI thread
            //progressBar1.Value = e.ProgressPercentage;

            //lblStatus2.Text = String.Format("Processing......{0}%", progressBar1.Value);
        }

        void m2_Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //If it was cancelled midway
            if (e.Cancelled)
            {
                //lblStatus2.Text = "Task Cancelled.";
                //Common.setMessageBox("Task Cancelled.", "Central Download", 3);
                return;
            }
            else if (e.Error != null)
            {
                //lblStatus2.Text = "Error while performing background operation.";
                //Common.setMessageBox("Error while performing background operation.", "Central Download", 3);
                return;
            }
            else
            {
                //lblStatus2.Text = "Task Completed...";
            }
            btnStartup.Enabled = true;
            btnStopup.Enabled = false;

        }

        void m2_Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            //NOTE : Never play with the UI thread here...

            //time consuming operation
            for (int i = 0; i < 100; i++)
            {
                Thread.Sleep(100);

                m2_Worker.ReportProgress(i);

                doUpload();

                //If cancel button was pressed while the execution is in progress
                //Change the state from cancellation ---> cancel'ed
                if (m2_Worker.CancellationPending)
                {
                    e.Cancel = true;

                    m2_Worker.ReportProgress(0);
                    return;
                }

            }

            //Report 100% completion on operation completed
            m2_Worker.ReportProgress(100);
        }

        void m_oWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //If it was cancelled midway
            if (e.Cancelled)
            {
                //lblStatus.Text = "Task Cancelled.";
                Common.setMessageBox("Task Cancelled.", "Central Download", 3);
                return;
            }
            else if (e.Error != null)
            {
                //lblStatus.Text = "Error while performing background operation.";
                Common.setMessageBox("Error while performing background operation.", "Central Download", 3);
                return;
            }
            else
            {
                //lblStatus.Text = "Task Completed...";
                Common.setMessageBox("Task Completed...", "Central Download", 3);
                return;
            }
            btnStart.Enabled = true;
            btnStop.Enabled = false;
            //btnStartAsyncOperation.Enabled = true;
            //btnCancel.Enabled = false;
        }

        void m_oWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //Here you play with the main UI thread
            //progressBar2.Value = e.ProgressPercentage;

            //lblStatus.Text = String.Format("Processing......{0}%", progressBar2.Value);
        }

        void m_oWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            //NOTE : Never play with the UI thread here...

            //time consuming operation
            for (int i = 0; i < 100; i++)
            {
                Thread.Sleep(100);
                m_oWorker.ReportProgress(i);

                DownloadIssueReceipt();
                doDowunload();

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
            //if (m2_Worker.IsBusy == false)
            //    m2_Worker.RunWorkerAsync();
            doUpload();
        }

        void btnStopup_Click(object sender, EventArgs e)
        {
            timer2.Enabled = false;

        }

        void btnStartup_Click(object sender, EventArgs e)
        {
            timer2.Enabled = true;
        }

        void btnStop_Click(object sender, EventArgs e)
        {
            btnStart.Enabled = true; btnStop.Enabled = false;

            btnStart.Visible = true;

            timer1.Enabled = false;

        }

        void btnStart_Click(object sender, EventArgs e)
        {
            btnStart.Enabled = false; btnStop.Enabled = true;

            btnStop.Visible = true;

            timer1.Enabled = true;
        }

        void timer1_Tick(object sender, EventArgs e)
        {
            DataTable Dt = (new Logic()).getSqlStatement("SELECT  COUNT(*) FROM Receipt.tblCollectionReceipt WHERE   UploadStatus = 0 AND ControlNumber IS NOT NULL AND PrintedBY IS NOT NULL").Tables[0];

            if (Dt != null && Dt.Rows.Count > 1)
            {
                timer1.Stop();
                lblStatus.Text = "Please Wait Upload in Progress, Download will Continue Shortly";
                lblStatus.Visible = true;
                timer2.Start();
                isTimer1Call = true;
                //doUpload();
            }
            else
            {
                timer2.Stop();
                DownloadIssueReceipt();
                doDowunload();
                isTimer1Call = false;
            }
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

        int doDowunload()
        {
            try
            {
                //    SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

                System.Data.DataSet dsonline = new System.Data.DataSet();

                using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
                {
                    connect.Open();

                    command = new SqlCommand("doGetStationInfo", connect) { CommandType = CommandType.StoredProcedure, CommandTimeout = 0 };

                    using (System.Data.DataSet ds = new System.Data.DataSet())
                    {
                        adp = new SqlDataAdapter(command);

                        adp.SelectCommand.CommandTimeout = 0;

                        adp.Fill(ds);

                        Dts = ds.Tables[0];

                        connect.Close();

                        if (ds.Tables[0].Rows.Count < 1)
                        {
                            label7.Text = "Not Configured for this station";
                            return 0;
                        }
                        else
                        {

                            label5.Text = string.Format("Receipt Data Download [ {0} ]", ds.Tables[0].Rows[0]["StationName"]);

                            switch (Program.intCode)
                            {
                                case 13://Akwa Ibom state
                                    using (var receiptAka = new AkwaIbomReceiptServices.ReceiptService())
                                    {
                                        dataSet = receiptAka.DownloadData(ds, Program.stateCode);
                                    }

                                    if (dataSet.Tables.Count == 0)
                                    {
                                        timer1.Stop(); timer1.Enabled = false;
                                        //btnStop.Visible = false; btnStart.Visible = true;
                                        return 0;
                                    }
                                    if (dataSet.Tables[0].Rows.Count < 1)
                                    {
                                        label7.Text = String.Format("No More Records for the Station {0}", Program.stationName);
                                        timer1.Stop(); timer1.Enabled = false;
                                        //btnStop.Visible = false; btnStart.Visible = true;
                                        return 0;
                                    }
                                    else
                                    {
                                        dataSet2 = InsertData(dataSet);

                                    }

                                    if (dataSet2.Tables[0].Rows[0]["returnCode"].ToString() == "-1")
                                    {
                                        timer1.Stop();
                                        Common.setMessageBox(string.Format("{0}...Error Occur During Data Insert After download.... Insert Download", dataSet2.Tables[0].Rows[0]["returnMessage"]), Program.ApplicationName, 3);
                                        timer1.Stop();
                                        timer1.Enabled = false;
                                        return 0;
                                    }
                                    else
                                    {
                                        dataSet3.Clear();
                                        using (var receiptAka = new AkwaIbomReceiptServices.ReceiptService())
                                        {
                                            dataSet3 = receiptAka.DownloadDataUpdate(dataSet2, Program.stationCode);
                                        }
                                    }
                                    break;

                                case 20://Delta State
                                    using (var receiptDelta = new DeltaBir.ReceiptService())
                                    {
                                        System.Data.DataSet dsnew = new System.Data.DataSet();
                                        //dsnew = Logic.GetMacAddress();
                                        //gridControl1.DataSource = dsnew.Tables[0];
                                        //gridControl1.DataSource = Logic.GetMacAddress();
                                        dataSet = receiptDelta.DownloadData(Logic.GetMacAddress(), Program.stationCode);
                                    }

                                    if (dataSet.Tables.Count == 0)
                                    {
                                        timer1.Stop(); timer1.Enabled = false;
                                        //btnStop.Visible = false; btnStart.Visible = true;
                                        return 0;
                                    }

                                    if (dataSet.Tables[0].Rows[0]["returnCode"].ToString() == "-1")
                                    {
                                        timer1.Stop();
                                        Common.setMessageBox(string.Format("{0}...Status Messages", dataSet.Tables[0].Rows[0]["returnMessage"]), Program.ApplicationName, 3);
                                        btnStart.Visible = true;
                                        timer1.Enabled = false;
                                        return 0;
                                    }
                                    if (dataSet.Tables[1] != null && dataSet.Tables[1].Rows.Count < 1)
                                    {
                                        label7.Text = String.Format("No More Records for the Station {0}", Program.stationName);
                                        timer1.Stop(); timer1.Enabled = false;
                                        btnStop.Visible = false; btnStart.Visible = true; btnStart.Enabled = true;
                                        return 0;
                                    }
                                    else
                                    {
                                        dataSet2 = InsertData(dataSet);

                                        if (dataSet2.Tables[0].Rows[0]["returnCode"].ToString() == "-1")
                                        {
                                            Common.setMessageBox(string.Format("{0}...Error Occur During Data Insert After download.... Insert Download", dataSet2.Tables[0].Rows[0]["returnMessage"]), Program.ApplicationName, 3);
                                            timer1.Stop();
                                            timer1.Enabled = false;
                                            return 0;
                                        }
                                        else
                                        {
                                            dataSet3.Clear();

                                            //dsonline = dataSet2.Tables[1];

                                            DataTable dtt = new DataTable();
                                            dtt = dataSet2.Tables[1].Copy();

                                            //dtsoff = ds.Tables[2].DataSet;
                                            dsonline.Tables.Add(dtt);

                                            using (var receiptDelta = new DeltaBir.ReceiptService())
                                            {
                                                dataSet3 = receiptDelta.DownloadDataUpdate(dsonline, Program.stationCode);
                                            }
                                        }
                                    }

                                    break;
                                case 32://kogi state
                                    break;

                                case 37://ogun state

                                    using (var receiptsserv = new ReceiptService())
                                    {
                                        System.Data.DataSet dsnew = new System.Data.DataSet();
                                        //dsnew = Logic.GetMacAddress();
                                        //gridControl1.DataSource = dsnew.Tables[0];
                                        dataSet = receiptsserv.DownloadData(Logic.GetMacAddress(), Program.stationCode);
                                    }

                                    if (dataSet.Tables[0].Rows[0]["returnCode"].ToString() == "-1")
                                    {
                                        timer1.Stop();
                                        Common.setMessageBox(string.Format("{0}...Status Messages", dataSet.Tables[0].Rows[0]["returnMessage"]), Program.ApplicationName, 3);
                                        btnStart.Visible = true;
                                        timer1.Enabled = false;
                                        return 0;
                                    }

                                    if (dataSet.Tables.Count == 0)
                                    {
                                        timer1.Stop(); timer1.Enabled = false;
                                        //btnStop.Visible = false; btnStart.Visible = true;
                                        return 0;
                                    }

                                    if (dataSet.Tables[1] != null && dataSet.Tables[1].Rows.Count < 1)
                                    {
                                        label7.Text = String.Format("No More Records for the Station {0}", Program.stationName);
                                        timer1.Stop(); timer1.Enabled = false;
                                        btnStop.Visible = false; btnStart.Visible = true; btnStart.Enabled = true;
                                        return 0;
                                    }
                                    else
                                    {
                                        dataSet2 = InsertData(dataSet);

                                        if (dataSet2.Tables[0].Rows[0]["returnCode"].ToString() == "-1")
                                        {
                                            Common.setMessageBox(string.Format("{0}...Error Occur During Data Insert After download.... Insert Download", dataSet2.Tables[0].Rows[0]["returnMessage"]), Program.ApplicationName, 3);
                                            timer1.Stop();
                                            timer1.Enabled = false;
                                            return 0;
                                        }
                                        else
                                        {
                                            dataSet3.Clear();

                                            using (var receiptsserv = new ReceiptService())
                                            {
                                                dataSet3 = receiptsserv.DownloadDataUpdate(dataSet2, Program.stationCode);
                                            }
                                        }
                                    }
                                    break;

                                case 40://oyo state

                                    using (var receiptsServices = new OyoReceiptServices.ReceiptService())
                                    {
                                        System.Data.DataSet dsnew = new System.Data.DataSet();
                                        dsnew = Logic.GetMacAddress();
                                        gridControl1.DataSource = dsnew.Tables[0];
                                        dataSet = receiptsServices.DownloadData(Logic.GetMacAddress(), Program.stationCode);
                                    }

                                    if (dataSet.Tables[0].Rows[0]["returnCode"].ToString() == "-1")
                                    {
                                        timer1.Stop();
                                        Common.setMessageBox(string.Format("{0}...Status Messages", dataSet.Tables[0].Rows[0]["returnMessage"]), Program.ApplicationName, 3);
                                        btnStart.Visible = true;
                                        timer1.Enabled = false;
                                        return 0;
                                    }

                                    if (dataSet.Tables.Count == 0)
                                    {
                                        timer1.Stop(); timer1.Enabled = false;
                                        //btnStop.Visible = false; btnStart.Visible = true;
                                        return 0;
                                    }

                                    if (dataSet.Tables[1] != null && dataSet.Tables[1].Rows.Count < 1)
                                    {
                                        label7.Text = String.Format("No More Records for the Station {0}", Program.stationName);
                                        timer1.Stop(); timer1.Enabled = false;
                                        btnStop.Visible = false; btnStart.Visible = true; btnStart.Enabled = true;
                                        return 0;
                                    }
                                    else
                                    {
                                        dataSet2 = InsertData(dataSet);

                                        if (dataSet2.Tables[0].Rows[0]["returnCode"].ToString() == "-1")
                                        {
                                            Common.setMessageBox(string.Format("{0}...Error Occur During Data Insert After download.... Insert Download", dataSet2.Tables[0].Rows[0]["returnMessage"]), Program.ApplicationName, 3);
                                            timer1.Stop();
                                            timer1.Enabled = false;
                                            return 0;
                                        }
                                        else
                                        {
                                            dataSet3.Clear();

                                            using (var receiptsServices = new OyoReceiptServices.ReceiptService())
                                            {
                                                dataSet3 = receiptsServices.DownloadDataUpdate(dataSet2, Program.stationCode);
                                            }
                                        }
                                    }
                                    break;

                                default:
                                    break;
                            }
                            if (dataSet3.Tables[0].Rows[0]["returnCode"].ToString() == "00")
                            {
                                lblAll.Text = dataSet3.Tables[1].Rows[0]["ALLRecords"].ToString();

                                lblDownload.Text = dataSet3.Tables[2].Rows[0]["DownloadedRecords"].ToString();

                                lblRemain.Text = dataSet3.Tables[3].Rows[0]["RemainRecords"].ToString();

                                lblError.Text = dataSet3.Tables[4].Rows[0]["ErrorRecords"].ToString();

                                return 1;

                            }
                            else
                            {

                                timer1.Stop();
                                Common.setMessageBox(string.Format("{0}...Download Data to Local Station...", dataSet3.Tables[0].Rows[0]["returnMessage"]), Program.ApplicationName, 3);
                                timer1.Enabled = false;
                                //btnStop.Visible = false;
                                //btnStart.Visible = true;
                                return 0;
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                timer1.Stop();
                Common.setMessageBox(String.Format("{0}----{1}...Do Download to Station,doDowunload", ex.Message, ex.StackTrace), Program.ApplicationName, 3); timer1.Stop(); timer1.Enabled = false;
                return 0;
            }
            finally
            {
                //SplashScreenManager.CloseForm(false);
            }
        }

        public System.Data.DataSet InsertData(System.Data.DataSet dataSet)
        {
            System.Data.DataSet ds = new System.Data.DataSet();

            try
            {
                recCount = dataSet.Tables[0].Rows.Count;

                switch (Program.intCode)
                {
                    //case 13://Akwa Ibom state


                    //    break;

                    //case 32://kogi state
                    //    break;
                    case 20://delta state
                        using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
                        {
                            connect.Open();

                            command = new SqlCommand("doInsertStationReceipt", connect) { CommandType = CommandType.StoredProcedure };
                            command.Parameters.Add(new SqlParameter("@tblCollectionReport_Temp", SqlDbType.Structured)).Value = dataSet.Tables[1];
                            command.CommandTimeout = 0;

                            //using (System.Data.DataSet dsf = new System.Data.DataSet())
                            //{
                            adp = new SqlDataAdapter(command);
                            adp.Fill(ds);


                            if (ds.Tables[0].Rows[0]["returnCode"].ToString() != "-1")
                            {
                                Dts = ds.Tables[1];
                            }

                            connect.Close();
                            //}
                        }
                        break;
                    case 37://ogun state
                        using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
                        {
                            connect.Open();

                            command = new SqlCommand("doInsertStationReceipt", connect) { CommandType = CommandType.StoredProcedure };
                            command.Parameters.Add(new SqlParameter("@tblCollectionReport_Temp", SqlDbType.Structured)).Value = dataSet.Tables[1];
                            command.CommandTimeout = 0;

                            //using (System.Data.DataSet dsf = new System.Data.DataSet())
                            //{
                            adp = new SqlDataAdapter(command);
                            adp.Fill(ds);
                            Dts = ds.Tables[0];
                            connect.Close();
                            //}
                        }
                        break;

                    case 40://oyo state
                        using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
                        {
                            connect.Open();

                            command = new SqlCommand("doInsertStationReceipt", connect) { CommandType = CommandType.StoredProcedure };
                            command.Parameters.Add(new SqlParameter("@tblCollectionReport_Temp", SqlDbType.Structured)).Value = dataSet.Tables[1];
                            command.CommandTimeout = 0;

                            //using (System.Data.DataSet dsf = new System.Data.DataSet())
                            //{
                            adp = new SqlDataAdapter(command);
                            adp.Fill(ds);
                            Dts = ds.Tables[0];
                            connect.Close();
                            //}
                        }
                        break;
                    default:
                        using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
                        {
                            connect.Open();

                            command = new SqlCommand("doInsertStationReceipt", connect) { CommandType = CommandType.StoredProcedure };
                            command.Parameters.Add(new SqlParameter("@tblCollectionReport_Temp", SqlDbType.Structured)).Value = dataSet.Tables[0];
                            command.CommandTimeout = 0;

                            //using (System.Data.DataSet dsf = new System.Data.DataSet())
                            //{
                            adp = new SqlDataAdapter(command);
                            adp.Fill(ds);
                            Dts = ds.Tables[0];
                            connect.Close();
                            //}
                        }
                        break;
                }




                return ds;
            }
            catch (Exception ex)
            {
                Common.setMessageBox(String.Format("{0}----{1}...Insert Data Record to Station", ex.StackTrace, ex.Message), Program.ApplicationName, 3);
                timer1.Enabled = false;
                btnStop.Visible = false;
                btnStart.Visible = true;
                return ds;
            }
            return ds;
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
                            //if (label8.InvokeRequired)
                            //{
                            //    label8.Invoke(new MethodInvoker(delegate { label7.Text = "No More Records to Upload"; }));
                            //}
                            //else
                            //{
                            label8.Text = "No More Records to Upload";
                            timer2.Stop(); timer2.Enabled = false;
                            //}
                            if (isTimer1Call)
                                lblStatus.Visible = false;
                            timer1.Enabled = true;
                            timer1.Start();

                            //label8.Text = "No More Records to Upload";

                            //m2_Worker.CancelAsync();
                            //btnStop.Visible = false; btnStartup.Visible = true;
                            return;
                        }
                        else
                        {
                            System.Data.DataSet dataSet = new System.Data.DataSet();

                            //gridControl2.BeginInvoke(new MethodInvoker(delegate
                            //{
                            //    gridControl2.DataSource = null;
                            //    gridControl2.DataSource = ds.Tables[0];
                            //    gridView2.BestFitColumns();
                            //}));

                            switch (Program.intCode)
                            {
                                case 13://Akwa Ibom state
                                    using (var receiptAka = new AkwaIbomReceiptServices.ReceiptService())
                                    {
                                        dataSet3 = receiptAka.UploadStationPrintedReceipts(ds, Program.stateCode);
                                    }

                                    break;
                                case 20:
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

                            //using (var recepservices = new ReceiptService())
                            //{
                            //dataSet = recepservices.UploadStationPrintedReceipts(ds, Program.stateCode);

                            if (String.Compare(dataSet.Tables[0].Rows[0]["rownumber"].ToString(), "-1", false) == 0)
                            {
                                timer2.Stop();
                                Common.setMessageBox((string)dataSet.Tables[0].Rows[0]["errorMessage"], Program.ApplicationName, 3);
                                timer2.Enabled = false;
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
                Common.setMessageBox(String.Format("{0}----{1}...Do upload from Station to Online", ex.StackTrace, ex.Message), Program.ApplicationName, 3); m2_Worker.CancelAsync();
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
                        timer2.Enabled = false;
                        m2_Worker.CancelAsync();
                        //btnStop.Visible = false;
                        //btnStartup.Visible = true;
                    }
                    else
                    {
                        //lblAllUp.Text = ds.Tables[0].Rows[0]["ALLRecords"].ToString();
                        //lblUpload.Text = ds.Tables[1].Rows[0]["UploadedRecords"].ToString();
                        //lblRemainUp.Text = ds.Tables[2].Rows[0]["RemainRecords"].ToString();
                        //lblErrup.Text = ds.Tables[3].Rows[0]["ErrorRecords"].ToString();

                        //if (lblAllUp.InvokeRequired)
                        //{
                        //    lblAllUp.Invoke(new MethodInvoker(delegate { label7.Text = ds.Tables[0].Rows[0]["ALLRecords"].ToString(); }));
                        //}
                        //else
                        //{
                        lblAllUp.Text = ds.Tables[0].Rows[0]["ALLRecords"].ToString();
                        //}

                        //if (lblUpload.InvokeRequired)
                        //{
                        //    lblUpload.Invoke(new MethodInvoker(delegate { label7.Text = ds.Tables[1].Rows[0]["UploadedRecords"].ToString(); }));
                        //}
                        //else
                        //{
                        lblUpload.Text = ds.Tables[1].Rows[0]["UploadedRecords"].ToString();
                        //}

                        //if (lblRemainUp.InvokeRequired)
                        //{
                        //    lblRemainUp.Invoke(new MethodInvoker(delegate { label7.Text = ds.Tables[2].Rows[0]["RemainRecords"].ToString(); }));
                        //}
                        //else
                        //{
                        lblRemainUp.Text = ds.Tables[2].Rows[0]["RemainRecords"].ToString();
                        //}

                        //if (lblErrup.InvokeRequired)
                        //{
                        //    lblErrup.Invoke(new MethodInvoker(delegate { label7.Text = ds.Tables[3].Rows[0]["ErrorRecords"].ToString(); }));
                        //}
                        //else
                        //{
                        lblErrup.Text = ds.Tables[3].Rows[0]["ErrorRecords"].ToString();
                        //}


                        //progressBar1.BeginInvoke((MethodInvoker)delegate()
                        //{
                        //    progressBar1.Minimum = Convert.ToInt32(lblRemainUp.Text);
                        //    progressBar1.Value = Convert.ToInt32(lblUpload.Text);
                        //    progressBar1.Refresh();
                        //    progressBar1.Increment(recCount2);
                        //    progressBar1.Maximum = Convert.ToInt32(lblAllUp.Text);
                        //    //txtLogDetails.Text = message; 

                        //});

                        //if (lblStatus2.InvokeRequired)
                        //{
                        //    lblStatus2.Invoke(new MethodInvoker(delegate
                        //    {
                        //        lblStatus2.Text = String.Format("Processing......{0}%", progressBar1.Value);
                        //    }));
                        //}
                        //else
                        //{
                        //    lblStatus2.Text = String.Format("Processing......{0}%", progressBar1.Value);
                        //}
                    }
                    //}
                }


                return dataSet;
            }
            catch (Exception ex)
            {
                Common.setMessageBox(String.Format("{0}....{1}..Do update uploadRecords On Station", ex.Message, ex.StackTrace), Program.ApplicationName, 3);
                timer2.Enabled = false;
                //btnStop.Visible = false;
                //btnStartup.Visible = true;
                //m2_Worker.CancelAsync();
                return dataSet;
            }
            return dataSet;
        }

        private int DownloadIssueReceipt()
        {
            try
            {
                using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
                {
                    connect.Open();

                    command = new SqlCommand("doGetStationInfo", connect) { CommandType = CommandType.StoredProcedure };

                    command.CommandTimeout = 0;

                    using (System.Data.DataSet ds = new System.Data.DataSet())
                    {
                        adp = new SqlDataAdapter(command);

                        adp.SelectCommand.CommandTimeout = 0;

                        adp.Fill(ds);

                        Dts = ds.Tables[0];

                        connect.Close();

                        if (ds.Tables.Count < 1 && ds.Tables[0].Rows.Count < 1)
                        {
                            return 0;
                        }
                        else
                        {
                            System.Data.DataSet dataSet = new System.Data.DataSet();

                            switch (Program.intCode)
                            {
                                case 13://Akwa Ibom state
                                    using (var receiptAka = new AkwaIbomReceiptServices.ReceiptService())
                                    {
                                        dataSet = receiptAka.DownloadDataIssueReceipt(ds, Program.stateCode);
                                    }
                                    break;
                                case 20:
                                    using (var receiptDelta = new DeltaBir.ReceiptService())
                                    {
                                        dataSet = receiptDelta.DownloadDataIssueReceipt(ds, Program.stateCode);
                                    }
                                    break;
                                case 32://kogi state
                                    break;

                                case 37://ogun state

                                    using (var receiptsserv = new ReceiptService())
                                    {
                                        dataSet = receiptsserv.DownloadDataIssueReceipt(ds, Program.stateCode);
                                    }

                                    break;

                                case 40://oyo state

                                    using (var receiptsServices = new OyoReceiptServices.ReceiptService())
                                    {
                                        dataSet = receiptsServices.DownloadDataIssueReceipt(ds, Program.stateCode);
                                    }


                                    break;

                                default:
                                    break;
                            }

                            if (dataSet.Tables.Count < 1 && dataSet.Tables[0].Rows.Count < 1)
                            {
                                return 0;
                            }
                            else
                            {
                                System.Data.DataSet dataSet2 = new System.Data.DataSet();

                                dataSet2 = InsertDataIssueReceipt(dataSet);

                                switch (Program.intCode)
                                {
                                    case 13://Akwa Ibom state
                                        using (var receiptAka = new AkwaIbomReceiptServices.ReceiptService())
                                        {
                                            dataSet2 = receiptAka.DownloadDataUpdateIssueReceipts(dataSet2, Program.stateCode);
                                        }
                                        break;
                                    case 20:
                                        using (var receiptDelta = new DeltaBir.ReceiptService())
                                        {
                                            dataSet = receiptDelta.DownloadDataUpdateIssueReceipts(dataSet2, Program.stateCode);
                                        }
                                        break;
                                    case 32://kogi state
                                        break;

                                    case 37://ogun state

                                        using (var receiptsserv = new ReceiptService())
                                        {
                                            dataSet2 = receiptsserv.DownloadDataUpdateIssueReceipts(dataSet2, Program.stateCode);
                                        }

                                        break;

                                    case 40://oyo state

                                        using (var receiptsServices = new OyoReceiptServices.ReceiptService())
                                        {
                                            dataSet2 = receiptsServices.DownloadDataUpdateIssueReceipts(dataSet2, Program.stateCode);
                                        }


                                        break;

                                    default:
                                        break;
                                }

                                return 1;
                            }


                        }
                    }
                }
            }
            catch (Exception ex)
            {
                timer1.Stop();

                Common.setMessageBox(String.Format("{0}....{1}..Download Issues Reeipts to Station", ex.Message, ex.StackTrace), Program.ApplicationName, 3);

                timer2.Stop();
                timer2.Enabled = false;
                btnStart.Visible = true; btnStart.Enabled = true;
                //btnStop.Visible = false;
                //btnStartup.Visible = true;
                //m_oWorker.CancelAsync();
                //bResponse=false;
                return 0;
            }
            //return bResponse;
        }

        System.Data.DataSet InsertDataIssueReceipt(System.Data.DataSet dataSet)
        {

            System.Data.DataSet ds = new System.Data.DataSet();

            try
            {
                using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
                {
                    connect.Open();

                    command = new SqlCommand("doInsertOyoReceiptIssueReceipt", connect) { CommandType = CommandType.StoredProcedure };
                    command.Parameters.Add(new SqlParameter("@tblIssueReceipt_Temp", SqlDbType.Structured)).Value = dataSet.Tables[0];
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
                Common.setMessageBox(String.Format("{0}....{1}..Download Insert Issues Reeipts to Station", ex.Message, ex.StackTrace), Program.ApplicationName, 3); timer2.Enabled = false;
                //btnStop.Visible = false;
                //btnStartup.Visible = true;
                //m_oWorker.CancelAsync();


                return ds;

            }
            return ds;
        }

    }
}
