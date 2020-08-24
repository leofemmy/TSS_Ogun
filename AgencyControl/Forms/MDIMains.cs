using System;
using System.Data.SqlClient;
using System.Windows.Forms;
using AgencyControl.Forms;
using TaxSmartSuite.Class;
using MosesClassLibrary.Utilities;
using DevExpress.XtraNavBar;
using AgencyControl.Class;




namespace AgencyControl.Forms
{
    public partial class MDIMains : Form
    {
        private string retval;
        
            private int childFormNumber;

        public static string stateCode;

        public static MDIMains publicMDIParent;

        Timer timer = new Timer();

        int lockTimerCounter;

        int offsetCounter = 0;

        protected string marqueeString = "Tax Smart Agency Control Panel Manager --- Powered by ICMA Services";


        //Methods extMethods = new Methods();

        public MDIMains()
        {
            InitializeComponent();
            //connects.ConnectionString();

         //   stateCode = extMethods.getQuery("statecode", "");

            publicMDIParent = this;

            Load += MDIMainForm_Load;
            
            Resize += MDIMainForm_Resize;
            
            FormClosing += new FormClosingEventHandler(MDIMainForm_FormClosing);
            
            timer.Tick += OnTimerTick;
            
            Init();

            //NavBars.ManageNavBarControls(navBarControl1, Program.ApplicationCode);

            //NavBars.NavBarEnableDisableControls(navBarControl1, false);

            //NavBars.ManageUserLoginNavBar(navBarControl1);
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
            MenuEvent();
            //OnNavBarItemsClicked(navBarItemState, null);
        }

        protected void MenuEvent()
        {
            navBarItemState.LinkClicked += OnNavBarItemsClicked;
            navBarItemExit.LinkClicked += OnNavBarItemsClicked;
            navBarItemLGName.LinkClicked += OnNavBarItemsClicked;
            navBarItem1Town.LinkClicked += OnNavBarItemsClicked;
            navBarItemAgency.LinkClicked += OnNavBarItemsClicked;
            navBarItemZone.LinkClicked += OnNavBarItemsClicked;
            navBarItemRevenueType.LinkClicked += OnNavBarItemsClicked;
            navBarItemRevenueTaxOffice.LinkClicked += OnNavBarItemsClicked;
            navBarItemBank.LinkClicked += OnNavBarItemsClicked;
            navBarItemBranch.LinkClicked += OnNavBarItemsClicked;
            navBarItemBankAccount.LinkClicked += OnNavBarItemsClicked;
            navBarItemPlatform.LinkClicked += OnNavBarItemsClicked;
            navBarItemCurrency.LinkClicked += OnNavBarItemsClicked;
            navBarItemBusniessClass.LinkClicked += OnNavBarItemsClicked;
            navBarItemBusinessSub.LinkClicked += OnNavBarItemsClicked;
        }

        void OnNavBarItemsClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            
            if (sender == navBarItemZone)
            {
                tableLayoutPanel2.Controls.Add((new FrmZone().panelContainer), 1, 0);
            }
            else if (sender == navBarItemRevenueTaxOffice)
            {
                tableLayoutPanel2.Controls.Add((new FrmRevenueTaxOffice().panelContainer), 1, 0);
            }
         
            else if (sender == navBarItemExit)
            {
                Close();
            }
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

            
            if (MosesClassLibrary.Utilities.Common.AskQuestion("Are you sure you want to Close this application?", "Tax Smart Registration"))

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

    }
}
