using BankReconciliation.Class;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using TaxSmartSuite.Class;

namespace BankReconciliation.Forms
{
    public partial class FrmUpload : Form
    {
        private SqlDataAdapter adp = null;
        private SqlCommand command;
        private DataTable Dts;
        public static FrmUpload publicStreetGroup;

        string UCC = "INT*78";
        string PCC = "INT*00";

        public FrmUpload()
        {
            InitializeComponent();

            setImages();

            ToolStripEvent();

            publicStreetGroup = this;

            Load += OnFormLoad;

            OnFormLoad(null, null);
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

        void OnFormLoad(object sender, EventArgs e)
        {
            DateTime result = DateTime.Today.Subtract(TimeSpan.FromDays(30));

            timer1.Tick += timer1_Tick;

            btnStart.Click += btnStart_Click; btnStop.Click += btnStop_Click;
        }

        void btnStop_Click(object sender, EventArgs e)
        {
            btnStop.Visible = false;

            btnStart.Visible = true;

            timer1.Enabled = false;
        }

        void btnStart_Click(object sender, EventArgs e)
        {
            btnStart.Visible = false;

            btnStop.Visible = true;

            timer1.Enabled = true;
        }

        void timer1_Tick(object sender, EventArgs e)
        {
            UploadData();
        }

        void UploadData()
        {

            try
            {
                DataTable dts = (new Logic()).getSqlStatement(("SELECT  TOP 1 Provider, Channel, PaymentRefNumber, DepositSlipNumber, (CONVERT(VARCHAR,PaymentDate,101) + ' ' + CONVERT(VARCHAR,PaymentDate,108)) AS PaymentDate, PayerID, PayerName, Amount, PaymentMethod,ChequeNumber,  ChequeValueDate, ChequeStatus, DateChequeReturned, TelephoneNumber, ReceiptNo, ReceiptDate, PayerAddress,  [User], RevenueCode, Description, ChequeBankCode, ChequeBankName, AgencyName, AgencyCode, BankCode, BankName, BranchCode, BranchName,NULL AS  ZoneCode, NULL AS ZoneName,NULL AS Username, NULL AS AmountWords, NULL AS EReceipts, NULL AS EReceiptsDate,NULL AS GeneratedBy,   NULL AS StationCode,NULL AS  StationName, ID FROM Reconciliation.tblCollectionReconciliation WHERE  Provider='ICMA' AND UploadStatus=0 ")).Tables[0];

                if (dts != null && dts.Rows.Count > 0)
                {
                    //var reconcilmethod = new Reems.REEMS();

                    //var reconcilmethod = new ReconciliationUpload.REEMS();

                    //string retvalue = reconcilmethod.CollectReports(dts.Rows[0]["PaymentRefNumber"].ToString(), dts.Rows[0]["DepositSlipNumber"].ToString(), dts.Rows[0]["PaymentDate"].ToString(), dts.Rows[0]["PayerID"].ToString(), dts.Rows[0]["PayerName"].ToString(), dts.Rows[0]["TelephoneNumber"].ToString(), dts.Rows[0]["RevenueCode"].ToString(), dts.Rows[0]["Description"].ToString(), Convert.ToDecimal(dts.Rows[0]["Amount"]), dts.Rows[0]["PaymentMethod"].ToString(), dts.Rows[0]["ChequeNumber"].ToString(), dts.Rows[0]["PaymentDate"].ToString(), dts.Rows[0]["ChequeBankCode"].ToString(), dts.Rows[0]["ChequeBankName"].ToString(), dts.Rows[0]["ChequeStatus"].ToString(), dts.Rows[0]["DateChequeReturned"].ToString(), dts.Rows[0]["AgencyName"].ToString(), dts.Rows[0]["AgencyCode"].ToString(), dts.Rows[0]["BankCode"].ToString(), dts.Rows[0]["BankName"].ToString(), dts.Rows[0]["BranchCode"].ToString(), dts.Rows[0]["BranchName"].ToString(), dts.Rows[0]["ZoneCode"].ToString(), dts.Rows[0]["ZoneName"].ToString(), dts.Rows[0]["ZoneCode"].ToString(), dts.Rows[0]["ZoneName"].ToString(), dts.Rows[0]["DepositSlipNumber"].ToString(), dts.Rows[0]["PayerAddress"].ToString(), Program.UserID, UCC, PCC);



                    //if (retvalue == "00")
                    //{
                    using (SqlConnection db = new SqlConnection(Logic.ConnectionString))
                    {
                        SqlTransaction transaction;

                        db.Open();

                        transaction = db.BeginTransaction();
                        try
                        {
                            //MessageBox.Show(MDIMain.stateCode);
                            //fieldid
                            //update record after 
                            using (SqlCommand sqlCommand1 = new SqlCommand(String.Format(String.Format("UPDATE Reconciliation.tblCollectionReconciliation SET UploadStatus='{{0}}', UploadBy='{{1}}', UploadDate='{{2}}' where  [PaymentRefNumber] ='{0}'", (string)dts.Rows[0]["PaymentRefNumber"]), true, Program.UserID, DateTime.Now), db, transaction))
                            {
                                sqlCommand1.ExecuteNonQuery();
                            }

                            //insert record to collection table
                            string query3 = String.Format("INSERT INTO Reconciliation.tblCollectionReconciliation( Provider , Channel ,PaymentRefNumber ,PaymentDate ,PayerID ,Amount ,PaymentMethod ,ChequeStatus ,RevenueCode ,Description , AgencyName , AgencyCode , BankCode ,BankName ) VALUES ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}');", (string)dts.Rows[0]["Provider"], (string)dts.Rows[0]["Channel"], (string)dts.Rows[0]["PaymentRefNumber"], (DateTime)dts.Rows[0]["PaymentRefNumber"], (string)dts.Rows[0]["PaymentRefNumber"], (string)dts.Rows[0]["PaymentRefNumber"], (string)dts.Rows[0]["PaymentRefNumber"], (string)dts.Rows[0]["PaymentRefNumber"], (string)dts.Rows[0]["PaymentRefNumber"], (string)dts.Rows[0]["PaymentRefNumber"], (string)dts.Rows[0]["PaymentRefNumber"], (string)dts.Rows[0]["PaymentRefNumber"], (string)dts.Rows[0]["PaymentRefNumber"], (string)dts.Rows[0]["PaymentRefNumber"]);


                            using (SqlCommand sqlCommand1 = new SqlCommand(query3, db, transaction))
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
                    //            //MessageBox.Show(MDIMain.stateCode);
                    //            //fieldid
                    //            using (SqlCommand sqlCommand1 = new SqlCommand(String.Format(String.Format("UPDATE [tblCollectionReport] SET UploadStatus='{{0}}' where  [PaymentRefNumber] ='{0}'", (string)dts.Rows[0]["PaymentRefNumber"]), (string)retvalue), db, transaction))

                    //            {
                    //                sqlCommand1.ExecuteNonQuery();
                    //            }

                    //            transaction.Commit();
                    //        }
                    //        catch (SqlException sqlError)
                    //        {
                    //            Tripous.Sys.ErrorBox(sqlError.Message, Program.ApplicationName, 2);
                    //            transaction.Rollback();
                    //            return;
                    //        }
                    //        db.Close();
                    //    }
                    //}

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

        void docount()
        {
            System.Data.DataSet ds = new System.Data.DataSet();
            ds.Clear();
            try
            {
                using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
                {
                    connect.Open();
                    Dts = new DataTable(); Dts.Clear();
                    command = new SqlCommand("doCountCashierRecord", connect) { CommandType = CommandType.StoredProcedure, CommandTimeout = 0 };


                    adp = new SqlDataAdapter(command);
                    adp.Fill(ds);
                    Dts = ds.Tables[0];
                    connect.Close();


                    if (ds.Tables[0].Rows[0]["returnCode"].ToString() == "00")
                    {
                        //label6.Text = String.Format("{0:N2}", ds.Tables[1].Rows[0]["ClosingBal"]);
                        lblAll.Text = ds.Tables[0].Rows[0]["All_Record"].ToString();
                        lblDownload.Text = ds.Tables[1].Rows[0]["Upload"].ToString();
                        lblRemain.Text = ds.Tables[2].Rows[0]["Remain"].ToString();
                        lblError.Text = ds.Tables[3].Rows[0]["Error"].ToString();
                    }
                    else
                    {
                        Common.setMessageBox(ds.Tables[0].Rows[0]["returnMessage"].ToString(), Program.ApplicationName, 2);
                    }


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


    }
}
