using DevExpress.XtraBars.Alerter;
using DevExpress.XtraNavBar;
using DevExpress.XtraReports.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TaxSmartSuite.AutoUpdate;
using TaxSmartSuite.Class;
using TaxSmartUtility.Classes;

namespace TaxSmartUtility.Forms
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

            NavBars.ManageNavBarControls(navBarControl1, Program.ApplicationCode);

            NavBars.NavBarEnableDisableControls(navBarControl1, false);

            NavBars.ManageUserLoginNavBar(navBarControl1);
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

            MenuEvent();
        }

        protected void MenuEvent()
        {

            navBarItemExit.LinkClicked += OnNavBarItemsClicked;
            navBarItemCheck.LinkClicked += OnNavBarItemsClicked;
            navBarItemMergeAgent.LinkClicked += OnNavBarItemsClicked;
            navBarItemMergeApproval.LinkClicked += OnNavBarItemsClicked;
            navBItmTaxAgent.LinkClicked += OnNavBarItemsClicked;

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
            //else if (sender == navBarItemHelp)
            //{
            //    //Help.ShowHelp(this, "C:\\Users\\Femmy\\Documents\\Visual Studio 2010\\Projects\\TSS_Oyo\\BankReconciliation\\reconciliation.chm");

            //}
            else if (sender == navBarItemMergeAgent)
            {
                tableLayoutPanel2.Controls.Add((new Form1().panelContainer), 1, 0);

                Form1.publicStreetGroup.Tag = ((sender) as NavBarItem).Tag;

                Form1.publicStreetGroup.RefreshForm();

            }
            else if (sender == navBarItemMergeApproval)
            {
                tableLayoutPanel2.Controls.Add((new FrmApproval().panelContainer), 1, 0);

                FrmApproval.publicStreetGroup.Tag = ((sender) as NavBarItem).Tag;

                FrmApproval.publicStreetGroup.RefreshForm();
            }
            else if (sender == navBItmTaxAgent)
            {
                string strquery = string.Format("SELECT UTINNew ,tblTaxAgent.OrganizationName ,Registration.tblTaxAgentMerge.UTIN ,Registration.tblTaxAgentMerge.OrganizationName FROM Registration.tblTaxAgentMerge JOIN Registration.tblTaxAgent ON UTINNew = tblTaxAgent.UTIN AND TaxAgentReferenceNumberNew = Registration.tblTaxAgent.TaxAgentReferenceNumber");

                using (var ds = new System.Data.DataSet())
                {

                    using (SqlDataAdapter ada = new SqlDataAdapter(strquery, Logic.ConnectionString))
                    {
                        ada.SelectCommand.CommandTimeout = 0;
                        ada.Fill(ds, "tables");
                    }

                    if (ds.Tables[0] != null && ds.Tables[0].Rows.Count >= 1)
                    {
                        var listret = (from DataRow row in ds.Tables[0].Rows
                                       select new ReportsMergi
                                       {

                                           OrganizationNameNew = row["OrganizationName"] as string,
                                           UTINNew = row["UTINNew"] as string,
                                           UTIN = row["UTIN"] as string,
                                           OrganizationName = row["OrganizationName"] as string

                                       }).ToList();

                        //XtraRepTaxAgentReturnAnnual Annual = new XtraRepTaxAgentReturnAnnual { DataSource = listret, logoPath = Logic.logopth };
                        //Annual.xrLabel11.Text = string.Format("{0} STATE GOVERNMENT", Program.StateName.ToUpper());
                        //Annual.xrLabel12.Text = string.Format("Tax Agent Annual returns report for {0} assessmenyt year", oDataRowViewsd.Row["TaxYear"]);
                        //Annual.ShowPreviewDialog();
                        XtraReport1 report = new XtraReport1 { DataSource = listret };
                        report.ShowPreviewDialog();

                    }
                    else
                    {
                        Common.setMessageBox("No Record Found for selected  Tax Agent Reconcilied", Program.ApplicationName, 1);
                        return;
                    }

                }
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
            Logic.LaunchApplication(programPath, string.Format(@"/p:{1} {0}", "http://www.icmaservices.com/generalfolder/SmartUtility/SmartUtilitySetup.txt", System.IO.Path.GetFileName(Application.ExecutablePath)));
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
            get { return "http://www.icmaservices.com/generalfolder/SmartUtility/SmartUtilitySetup.txt"; }

        }

    }
}
