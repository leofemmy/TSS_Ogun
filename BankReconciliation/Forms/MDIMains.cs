using System;
using System.Data.SqlClient;
using System.Windows.Forms;
using BankReconciliation.Forms;
using TaxSmartSuite.Class;
using MosesClassLibrary.Utilities;
using DevExpress.XtraNavBar;
using BankReconciliation.Class;
using System.Diagnostics;
using System.Data;
using System.Collections.Generic;
using TaxSmartSuite.AutoUpdate;
using DevExpress.XtraBars.Alerter;


namespace BankReconciliation.Forms
{
    public partial class MDIMains : Form
    {
        private string retval;

        private int childFormNumber;

        public static string stateCode;

        public static MDIMains publicMDIParent;

        public string ReceiptOfficecode;

        Timer timer = new Timer();

        int lockTimerCounter;

        int offsetCounter = 0;



        //protected string marqueeString = " Bank Reconciliation Manager --- Powered by ICMA Services";

        protected string marqueeString = System.Configuration.ConfigurationManager.AppSettings["marquee"];

        //Methods extMethods = new Methods();

        public MDIMains()
        {
            InitializeComponent();
            //connects.ConnectionString();

            //   stateCode = extMethods.getQuery("statecode", "");

            publicMDIParent = this;

            Load += MDIMainForm_Load;

            Resize += MDIMainForm_Resize;

            FormClosing += MDIMainForm_FormClosing;

            timer.Tick += OnTimerTick;

            Init();


            //DataTable dts = (new Logic()).getSqlStatement("select CentreCode from [tblReceiptOffice]").Tables[0];

            //if (dts != null && dts.Rows.Count > 0)
            //{
            //    ReceiptOfficecode = dts.Rows[0]["CentreCode"].ToString();
            //}

            //NavBars.ManageNavBarControls(navBarControl1, Program.ApplicationCode);

            NavBars.NavBarEnableDisableControls(navBarControl1, false);

            NavBars.ManageUserLoginNavBar(navBarControl1);

            //if (NavBars.ManagerUserApproval(navBarControl1))
            //{
            //    Token.dotoken();
            //}
        }

        private void ShowNewForm(EventArgs e)
        {
            Form childForm = new Form { MdiParent = this, Text = "Window " + childFormNumber++ };
            childForm.Show();
        }

        void MDIMainForm_Resize(object sender, EventArgs e)
        {
            gaugeControl1.Left = (panelControl1.Width - gaugeControl1.Width) / 2;
        }

        void OnTimerTick(object sender, EventArgs e)
        {
            if (lockTimerCounter == 0)
            {
                lockTimerCounter++;

                UpdateText();

                lockTimerCounter--;
            }
        }

        void MDIMainForm_FormClosing(object sender, FormClosingEventArgs e)
        {

            e.Cancel = !CloseForm();
        }

        protected void UpdateText()
        {
            string fullTextToShow = marqueeString;

            char[] textToShow = new char[digitalGauge1.DigitCount];

            for (int i = 0; i < digitalGauge1.DigitCount; i++)
            {
                if (i + offsetCounter >= 0 && i + offsetCounter < fullTextToShow.Length)
                {
                    textToShow[i] = fullTextToShow[i + offsetCounter];
                }
                else textToShow[i] = ' ';
            }
            offsetCounter++;

            if (offsetCounter > digitalGauge1.DigitCount + fullTextToShow.Length) offsetCounter = -digitalGauge1.DigitCount;

            digitalGauge1.Text = new string(textToShow);
        }

        protected void Init()
        {
            timer.Stop();

            offsetCounter = -digitalGauge1.DigitCount;

            timer.Interval = 500 / 3;

            timer.Start();
        }

        void MDIMainForm_Load(object sender, EventArgs e)
        {
            Text = String.Format("ICMA Services :: {2} -->>  For {0} State [ Server Name == {1} ]-->> User ID:: {3}", Program.StateName, Program.ServerName, Program.ApplicationName, Program.UserID);

            MenuEvent(); AutoCheckUpdate();
        }

        protected void MenuEvent()
        {

            navBarItemExit.LinkClicked += OnNavBarItemsClicked;
            navBarItemCheck.LinkClicked += OnNavBarItemsClicked;
            navBarItemIGR.LinkClicked += OnNavBarItemsClicked;
            navBarItemDefaultAgency.LinkClicked += OnNavBarItemsClicked;
            navBarItemDefaultRevenue.LinkClicked += OnNavBarItemsClicked;
            navBarItemBank.LinkClicked += OnNavBarItemsClicked;
            navBarItemBanks.LinkClicked += OnNavBarItemsClicked;
            //navBarItemCashier.LinkClicked += OnNavBarItemsClicked;
            navBarItemDefaultZonalRevenue.LinkClicked += OnNavBarItemsClicked;
            navBarItemFiscal.LinkClicked += OnNavBarItemsClicked;
            navBarItemRefernceletter.LinkClicked += OnNavBarItemsClicked;
            navBarItemTransaction.LinkClicked += OnNavBarItemsClicked;
            navBarItemTransactionType.LinkClicked += OnNavBarItemsClicked;
            navBarItemHelp.LinkClicked += OnNavBarItemsClicked;
            //navBarItemDataUpload.LinkClicked += OnNavBarItemsClicked;
            navBarItemCollectionControl.LinkClicked += OnNavBarItemsClicked;
            //navBarItemDataUpload.LinkClicked += OnNavBarItemsClicked;
            navBarItemSchedule.LinkClicked += OnNavBarItemsClicked;
            navBarItemPostingRequest.LinkClicked += OnNavBarItemsClicked;
            navBarItemClosePeriod.LinkClicked += OnNavBarItemsClicked;
            navBarItem1.LinkClicked += OnNavBarItemsClicked;
            navBarItem2.LinkClicked += OnNavBarItemsClicked;
            navBarItem3.LinkClicked += OnNavBarItemsClicked;
            navBarItem4.LinkClicked += OnNavBarItemsClicked;
            navBarItem5.LinkClicked += OnNavBarItemsClicked;
            //navBarItem6.LinkClicked += OnNavBarItemsClicked;
            navBarItem7.LinkClicked += OnNavBarItemsClicked;
            navBarList.LinkClicked += OnNavBarItemsClicked;
            navBarYear.LinkClicked += OnNavBarItemsClicked;
            navBarClearTrans.LinkClicked += OnNavBarItemsClicked;
            navBItmReclassified.LinkClicked += OnNavBarItemsClicked;
            navBaritmApprovalReclass.LinkClicked += OnNavBarItemsClicked;
            navBitmGet.LinkClicked += OnNavBarItemsClicked;
            navBITMReconciliationPeriod.LinkClicked += OnNavBarItemsClicked;
            navBITmMinisrty.LinkClicked += OnNavBarItemsClicked;
            navBarItem6.LinkClicked += OnNavBarItemsClicked;
            navBarItemToken.LinkClicked += OnNavBarItemsClicked;


        }

        void OnNavBarItemsClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            if (sender != navBarItemExit)
            {
                RemoveControls();
            }

            if (sender == navBarItemCheck)
            {
                //Process prupdate = Process.Start("wyUpdate.exe");
                CheckUpdate();
            }
            else if (sender == navBarItemHelp)
            {
                //Help.ShowHelp(this, "C:\\Users\\Femmy\\Documents\\Visual Studio 2010\\Projects\\TSS_Oyo\\BankReconciliation\\reconciliation.chm");

            }
            else if (sender == navBarItem1)
            {
                tableLayoutPanel2.Controls.Add((new FrmPeriods().panelContainer), 1, 0);

                FrmPeriods.publicStreetGroup.Tag = ((sender) as NavBarItem).Tag;

                FrmPeriods.publicStreetGroup.RefreshForm();

            }
            else if (sender == navBarItem6)
            {
                using (var frm = new FrmChangePassword())
                {
                    frm.ShowDialog();
                }
            }
            else if (sender == navBITmMinisrty)
            {
                tableLayoutPanel2.Controls.Add((new FrmMinistryApproval().panelContainer), 1, 0);

                FrmMinistryApproval.publicStreetGroup.Tag = ((sender) as NavBarItem).Tag;

                FrmMinistryApproval.publicStreetGroup.RefreshForm();
            }
            else if (sender == navBaritmApprovalReclass)
            {
                tableLayoutPanel2.Controls.Add((new FrmFirstApproval().panelContainer), 1, 0);

                FrmFirstApproval.publicStreetGroup.Tag = ((sender) as NavBarItem).Tag;

                FrmFirstApproval.publicStreetGroup.RefreshForm();

            }
            else if (sender == navBarItem2)
            {
                tableLayoutPanel2.Controls.Add((new FrmCloseFinanical().panelContainer), 1, 0);

                FrmCloseFinanical.publicStreetGroup.Tag = ((sender) as NavBarItem).Tag;

                FrmCloseFinanical.publicStreetGroup.RefreshForm();
            }
            else if (sender == navBarItemIGR)
            {
                tableLayoutPanel2.Controls.Add((new FrmReportIGR().panelContainer), 1, 0);

                FrmReportIGR.publicStreetGroup.Tag = ((sender) as NavBarItem).Tag;

                FrmReportIGR.publicStreetGroup.RefreshForm();
            }
            else if (sender == navBarItem5)
            {
                tableLayoutPanel2.Controls.Add((new FrmExchangeRate().panelContainer), 1, 0);

                FrmExchangeRate.publicStreetGroup.Tag = ((sender) as NavBarItem).Tag;

                FrmExchangeRate.publicStreetGroup.RefreshForm();
            }
            else if (sender == navBITMReconciliationPeriod)
            {
                tableLayoutPanel2.Controls.Add((new FrmReconciliationperid().panelContainer), 1, 0);

                FrmReconciliationperid.publicStreetGroup.Tag = ((sender) as NavBarItem).Tag;

                FrmReconciliationperid.publicStreetGroup.RefreshForm();
            }
            else if (sender == navBarItem3)
            {
                tableLayoutPanel2.Controls.Add((new Frmunswept().panelContainer), 1, 0);

                Frmunswept.publicStreetGroup.Tag = ((sender) as NavBarItem).Tag;

                Frmunswept.publicStreetGroup.RefreshForm();
            }
            else if (sender == navBarList)
            {
                tableLayoutPanel2.Controls.Add((new FrmDelist().panelContainer), 1, 0);

                FrmDelist.publicStreetGroup.Tag = ((sender) as NavBarItem).Tag;

                FrmDelist.publicStreetGroup.RefreshForm();
            }
            else if (sender == navBarYear)
            {
                tableLayoutPanel2.Controls.Add((new FrmYearly().panelContainer), 1, 0);

                FrmYearly.publicStreetGroup.Tag = ((sender) as NavBarItem).Tag;

                FrmYearly.publicStreetGroup.RefreshForm();
            }
            else if (sender == navBarItem4)
            {
                tableLayoutPanel2.Controls.Add((new FrmSwept().panelContainer), 1, 0);

                FrmSwept.publicStreetGroup.Tag = ((sender) as NavBarItem).Tag;

                FrmSwept.publicStreetGroup.RefreshForm();
            }
            else if (sender == navBarItemToken)
            {
                tableLayoutPanel2.Controls.Add((new frmtoken().panelContainer), 1, 0);

                frmtoken.publicStreetGroup.Tag = ((sender) as NavBarItem).Tag;

                frmtoken.publicStreetGroup.RefreshForm();
            }
            //else if (sender == navBarItem6)
            //{
            //    tableLayoutPanel2.Controls.Add((new FrmAnalysis().panelContainer), 1, 0);

            //    FrmAnalysis.publicStreetGroup.Tag = ((sender) as NavBarItem).Tag;

            //    FrmAnalysis.publicStreetGroup.RefreshForm();
            //}
            else if (sender == navBarItem7)
            {
                tableLayoutPanel2.Controls.Add((new FrmSummaryAG().panelContainer), 1, 0);

                FrmSummaryAG.publicStreetGroup.Tag = ((sender) as NavBarItem).Tag;

                FrmSummaryAG.publicStreetGroup.RefreshForm();
            }
            //else if (sender == navBarItemDataUpload)
            //{
            //    //tableLayoutPanel2.Controls.Add((new FrmUpload().panelContainer), 1, 0);

            //    //FrmUpload.publicStreetGroup.Tag = ((sender) as NavBarItem).Tag;

            //    //FrmUpload.publicStreetGroup.RefreshForm();
            //}
            else if (sender == navBarItemDefaultAgency)
            {
                tableLayoutPanel2.Controls.Add((new FrmAgencyUk().panelContainer), 1, 0);

                FrmAgencyUk.publicStreetGroup.Tag = ((sender) as NavBarItem).Tag;

                FrmAgencyUk.publicStreetGroup.RefreshForm();
            }
            else if (sender == navBarItemDefaultRevenue)
            {
                tableLayoutPanel2.Controls.Add((new FrmTempRevenue().panelContainer), 1, 0);
                FrmTempRevenue.publicStreetGroup.Tag = ((sender) as NavBarItem).Tag;
                FrmTempRevenue.publicStreetGroup.RefreshForm();
                //FrmRevenuetemp.publicStreetGroup.Tag = ((sender) as NavBarItem).Tag;

                //FrmRevenuetemp.publicStreetGroup.RefreshForm();
            }
            else if (sender == navBarItemBank)
            {
                tableLayoutPanel2.Controls.Add((new FrmAccountBank().panelContainer), 1, 0);

                FrmAccountBank.publicStreetGroup.Tag = ((sender) as NavBarItem).Tag;

                FrmAccountBank.publicStreetGroup.RefreshForm();
            }
            else if (sender == navBarClearTrans)
            {

                tableLayoutPanel2.Controls.Add((new FrmClear().panelContainer), 1, 0);

                FrmClear.publicStreetGroup.Tag = ((sender) as NavBarItem).Tag;

                FrmClear.publicStreetGroup.RefreshForm();
            }
            else if (sender == navBarItemPostingRequest)
            {
                tableLayoutPanel2.Controls.Add((new FrmRequestNew().panelContainer), 1, 0);

                FrmRequestNew.publicStreetGroup.Tag = ((sender) as NavBarItem).Tag;

                FrmRequestNew.publicStreetGroup.RefreshForm();
            }
            else if (sender == navBarItemClosePeriod)
            {
                tableLayoutPanel2.Controls.Add((new FrmRequest().panelContainer), 1, 0);

                FrmRequest.publicStreetGroup.Tag = ((sender) as NavBarItem).Tag;

                FrmRequest.publicStreetGroup.RefreshForm();
            }
            else if (sender == navBarItemBanks)
            {
                //tableLayoutPanel2.Controls.Add((new FrmExecption().panelControl1), 1, 0);

                //FrmExecption.publicStreetGroup.Tag = ((sender) as NavBarItem).Tag;

                //FrmExecption.publicStreetGroup.RefreshForm();

                tableLayoutPanel2.Controls.Add((new FrmTransaction().panelContainer), 1, 0);

                FrmTransaction.publicStreetGroup.Tag = ((sender) as NavBarItem).Tag;

                FrmTransaction.publicStreetGroup.RefreshForm();
            }
            else if (sender == navBItmReclassified)
            {
                tableLayoutPanel2.Controls.Add((new FrmReclassified().panelContainer), 1, 0);

                FrmReclassified.publicStreetGroup.Tag = ((sender) as NavBarItem).Tag;

                FrmReclassified.publicStreetGroup.RefreshForm();
            }
            else if (sender == navBitmGet)
            {
                tableLayoutPanel2.Controls.Add((new FrmGet().panelContainer), 1, 0);

                FrmGet.publicStreetGroup.Tag = ((sender) as NavBarItem).Tag;

                FrmGet.publicStreetGroup.RefreshForm();

            }
            else if (sender == navBarItemSchedule)
            {
                tableLayoutPanel2.Controls.Add((new FrmSchedule().panelContainer), 1, 0);

                FrmSchedule.publicStreetGroup.Tag = ((sender) as NavBarItem).Tag;

                FrmSchedule.publicStreetGroup.RefreshForm();
            }
            else if (sender == navBarItemDefaultZonalRevenue)
            {
                tableLayoutPanel2.Controls.Add((new FrmZonalTemp().panelContainer), 1, 0);

                FrmZonalTemp.publicStreetGroup.Tag = ((sender) as NavBarItem).Tag;

                FrmZonalTemp.publicStreetGroup.RefreshForm();
            }
            else if (sender == navBarItemFiscal)
            {
                tableLayoutPanel2.Controls.Add((new FrmPeriods().panelContainer), 1, 0);

                FrmPeriods.publicStreetGroup.Tag = ((sender) as NavBarItem).Tag;

                FrmPeriods.publicStreetGroup.RefreshForm();
            }
            else if (sender == navBarItemRefernceletter)
            {
                tableLayoutPanel2.Controls.Add((new FrmStateRef().panelContainer), 1, 0);

                FrmStateRef.publicStreetGroup.Tag = ((sender) as NavBarItem).Tag;

                FrmStateRef.publicStreetGroup.RefreshForm();
            }
            else if (sender == navBarItemTransaction)
            {
                tableLayoutPanel2.Controls.Add((new FrmTransactionSetup().panelContainer), 1, 0);

                FrmTransactionSetup.publicStreetGroup.Tag = ((sender) as NavBarItem).Tag;

                FrmTransactionSetup.publicStreetGroup.RefreshForm();
            }
            else if (sender == navBarItemTransactionType)
            {
                tableLayoutPanel2.Controls.Add((new FrmViewTransaction().panelContainer), 1, 0);

                FrmViewTransaction.publicStreetGroup.Tag = ((sender) as NavBarItem).Tag;

                FrmViewTransaction.publicStreetGroup.RefreshForm();
            }
            else if (sender == navBarItemCollectionControl)
            {
                tableLayoutPanel2.Controls.Add((new FrmCollectionControl().panelContainer), 1, 0);

                FrmCollectionControl.publicStreetGroup.Tag = ((sender) as NavBarItem).Tag;

                FrmCollectionControl.publicStreetGroup.RefreshForm();
            }
            else if (sender == navBarItemExit)
            {
                Close();
            }
            //else if (sender == navBarItemDataUpload)
            //{
            //    tableLayoutPanel2.Controls.Add((new FrmTransaction().panelContainer), 1, 0);

            //    FrmTransaction.publicStreetGroup.Tag = ((sender) as NavBarItem).Tag;

            //    FrmTransaction.publicStreetGroup.RefreshForm();

            //}
        }

        public void RemoveControls()
        {
            try
            {
                tableLayoutPanel2.Controls.RemoveAt(1);
            }
            finally
            {
                tableLayoutPanel2.Controls.Add(panelContainer, 1, 0);
            }
        }

        private static bool CloseForm()
        {
            bool bRes = false;


            if (MosesClassLibrary.Utilities.Common.AskQuestion("Are you sure you want to Close this application?", Program.ApplicationName))

                bRes = true;

            else
                bRes = false;

            return bRes;
        }

        //private void setImages()
        //{

        //    _001_001.Image = i32x32.Images[14];
        //    _001_001_001.Image = i32x32.Images[12];
        //    _001_002.Image = i32x32.Images[25];

        //    //tsbNew.Image = i32x32.Images[0];
        //    //tsbModify.Image = i32x32.Images[1];
        //    //tsbDelete.Image = i32x32.Images[3];
        //    //tsbReload.Image = i32x32.Images[4];
        //    //tsbClose.Image = i32x32.Images[6];
        //    //tsbCardDetails.Image = i32x32.Images[11];
        //    //tsbMakePayment.Image = i32x32.Images[12];
        //    //tsbAccountStatement.Image = i32x32.Images[5];

        //    //cmsItemNew.Image = MDIMainForm.publicMDIParent.i16x16.Images[5];
        //    //cmsItemModify.Image = MDIMainForm.publicMDIParent.i16x16.Images[6];
        //    //cmsItemDelete.Image = MDIMainForm.publicMDIParent.i16x16.Images[7];
        //    //cmsItemReload.Image = MDIMainForm.publicMDIParent.i16x16.Images[9];
        //    //cmsItemClose.Image = MDIMainForm.publicMDIParent.i16x16.Images[11];
        //    //cmsMakePayment.Image = MDIMainForm.publicMDIParent.i16x16.Images[12];
        //    //cmsAccountStatement.Image = MDIMainForm.publicMDIParent.i16x16.Images[10];

        //    //gridView1.Images = MDIMainForm.publicMDIParent.i24x24;

        //}


        //private void userModules()
        //{
        //    string query = String.Format("select modulescode from ViewUserApplicationModules where userid = '{0}'",FrmLogIn.Userid);


        //    connects.connect.Close();
        //    using (SqlCommand command = new SqlCommand(query, connects.connect))
        //    {
        //        SqlDataAdapter adp = new SqlDataAdapter(command);
        //        connects.connect.Open();
        //        SqlDataReader reader = command.ExecuteReader();
        //        while (reader.Read())
        //        {
        //            foreach (ToolStripItem item in (toolStrip1.Items))
        //                if (item.GetType().ToString() == typeof(ToolStripDropDownButton).ToString())
        //                    if (reader.GetValue(0).ToString() == item.Name.Substring(1).Replace('_', '-'))
        //                        item.Enabled = true;
        //        }
        //    }

        //}


        //private void userModulesAccess()
        //{
        //    string query = String.Format("select ModuleAccessCode from ViewUserApplicationModulesAccess where userid = '{0}'", FrmLogIn.Userid);


        //    connects.connect.Close();
        //    using (SqlCommand command = new SqlCommand(query, connects.connect))
        //    {
        //        SqlDataAdapter adp = new SqlDataAdapter(command);
        //        connects.connect.Open();
        //        SqlDataReader reader = command.ExecuteReader();
        //        while (reader.Read())
        //        {
        //            foreach (ToolStripItem item in (toolStrip1.Items))
        //                if (item.GetType().ToString() == typeof(ToolStripDropDownButton).ToString())
        //                {
        //                    ToolStripDropDownButton lol = (ToolStripDropDownButton)item;
        //                    //lol.DropDownItems
        //                    foreach (ToolStripItem item2 in lol.DropDownItems)
        //                        if (item2.GetType().ToString() == typeof(ToolStripMenuItem).ToString())
        //                        {
        //                            //MessageBox.Show(item2.Name.ToString());
        //                            if (reader.GetValue(0).ToString() == item2.Name.Substring(1).Replace('_', '-'))
        //                            {
        //                                //ToolStripMenuItem lol2 = (ToolStripMenuItem)item2;

        //                                //foreach (ToolStripItem item3 in lol2.DropDownItems)
        //                                //{
        //                                //    item3.Enabled = true;
        //                                //}

        //                                item2.Enabled = true;
        //                            }
        //                        }

        //                }
        //        }
        //    }

        //}

        private void MDIMain_Load(object sender, EventArgs e)
        {
            //this.toolStrip1.Renderer = new Office2007Renderer();
        }

        protected void CheckUpdate()
        {
            var startupPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
            var programPath = System.IO.Path.Combine(startupPath, "twux.exe");
            Logic.LaunchApplication(programPath, string.Format(@"/p:{1} {0}", "http://www.icmaservices.com/generalfolder/Reconciliation/BankReconciliationSetup.txt", System.IO.Path.GetFileName(Application.ExecutablePath)));
            //TaxSmartSuite.CommonLibrary.Common.Utils.LaunchApplication(programPath,
            //string.Format(@"/p:{1} {0}", GetUpdateUrl, System.IO.Path.GetFileName(Application.ExecutablePath)));
        }

        private void AutoCheckUpdate()
        {
            AppAutoUpdate.GetUpdate(GetUpdateUrl).ContinueWith(task =>
            {
                var updateReturnMsg = task.Result;
                if (updateReturnMsg.Status)
                {
                    BeginInvoke(new MethodInvoker(delegate ()
                    {
                        var msg = string.Format("<b>{0}</b>\n<i>{1}</i>", updateReturnMsg.AppName,
                            updateReturnMsg.Version);
                        AlertInfo info = new AlertInfo("<size=14><b><u>Update Available</u></b></size>", msg);
                        AlertControl alertControl = new AlertControl { AllowHtmlText = true, AllowHotTrack = true };
                        alertControl.AlertClick += alertControl_AlertClick;
                        alertControl.Show(this, info);
                    }));
                }
            });
        }

        void alertControl_AlertClick(object sender, AlertClickEventArgs e)
        {
            CheckUpdate();
        }

        private string GetUpdateUrl
        {
            get { return "http://www.icmaservices.com/generalfolder/Reconciliation/BankReconciliationSetup.txt"; }

        }

        private void tableLayoutPanel2_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
