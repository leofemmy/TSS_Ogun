using DevExpress.XtraNavBar;
using System;
using System.Windows.Forms;
using TaxSmartSuite.Class;



namespace Collection.Forms
{
    public partial class MDIMain : Form
    {
        private string retval;

        private int childFormNumber;

        public static string stateCode;

        public static MDIMain publicMDIParent;

        Timer timer = new Timer();

        int lockTimerCounter;

        int offsetCounter = 0;

        protected string marqueeString = "Tax Smart Collection Manager --- Powered by ICMA Services";


        Methods extMethods = new Methods();

        public MDIMain()
        {
            InitializeComponent();


            //stateCode = extMethods.getQuery("statecode", "");

            publicMDIParent = this;

            Load += MDIMainForm_Load;

            Resize += MDIMainForm_Resize;

            FormClosing += MDIMainForm_FormClosing;

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

            //OnNavBarItemsClicked(navBarItem, null);
        }

        protected void MenuEvent()
        {
            navBarItemCollect.LinkClicked += OnNavBarItemsClicked;

            navBarItemExit.LinkClicked += OnNavBarItemsClicked;

            navBarItemLGName.LinkClicked += OnNavBarItemsClicked;

            navBarItemPrintReceipt.LinkClicked += OnNavBarItemsClicked;

            navBarItemCollExp.LinkClicked += OnNavBarItemsClicked;

            navBarItemTaxAgentExp.LinkClicked += OnNavBarItemsClicked;

            navBarItemTaxPayerExp.LinkClicked += OnNavBarItemsClicked;

            navBarItemRevenueTaxOffice.LinkClicked += OnNavBarItemsClicked;

            navBarItemCollImpt.LinkClicked += OnNavBarItemsClicked;

            navBarItemTaxAgentImpt.LinkClicked += OnNavBarItemsClicked;

            navBarItemTaxPayerImpt.LinkClicked += OnNavBarItemsClicked;

            navBarItemPlatform.LinkClicked += OnNavBarItemsClicked;

            navBarItemCashBank.LinkClicked += OnNavBarItemsClicked;

            //navBarItemBusinessSub.LinkClicked += OnNavBarItemsClicked;

            navBarItemSummary.LinkClicked += OnNavBarItemsClicked;

            navBarItemPrintReceipt.LinkClicked += OnNavBarItemsClicked;
        }

        void OnNavBarItemsClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            if (sender == navBarItemCollImpt)
            {
                FrmImports.tableType = 1;

                tableLayoutPanel2.Controls.Add((new FrmImports().panelContainer), 1, 0);

                FrmImports.publicStreetGroup.Tag = ((sender) as NavBarItem).Tag;

                FrmImports.publicStreetGroup.RefreshForm();
            }
            else if (sender == navBarItemTaxAgentImpt)
            {
                FrmImports.tableType = 2;

                tableLayoutPanel2.Controls.Add((new FrmImports().panelContainer), 1, 0);

                FrmImports.publicStreetGroup.Tag = ((sender) as NavBarItem).Tag;

                FrmImports.publicStreetGroup.RefreshForm();

            }
            else if (sender == navBarItemTaxPayerImpt)
            {
                FrmImports.tableType = 3;

                tableLayoutPanel2.Controls.Add((new FrmImports().panelContainer), 1, 0);

                FrmImports.publicStreetGroup.Tag = ((sender) as NavBarItem).Tag;

                FrmImports.publicStreetGroup.RefreshForm();
            }
            else if (sender == navBarItemCollExp)
            {
                FrmExport.tableType = 1;

                tableLayoutPanel2.Controls.Add((new FrmExport().panelContainer), 1, 0);

                FrmExport.publicStreetGroup.Tag = ((sender) as NavBarItem).Tag;

                FrmExport.publicStreetGroup.RefreshForm();
            }
            else if (sender == navBarItemTaxAgentExp)
            {
                FrmExport.tableType = 2;

                tableLayoutPanel2.Controls.Add((new FrmExport().panelContainer), 1, 0);

                FrmExport.publicStreetGroup.Tag = ((sender) as NavBarItem).Tag;

                FrmExport.publicStreetGroup.RefreshForm();
            }
            else if (sender == navBarItemTaxPayerExp)
            {
                FrmExport.tableType = 3;

                tableLayoutPanel2.Controls.Add((new FrmExport().panelContainer), 1, 0);

                FrmExport.publicStreetGroup.Tag = ((sender) as NavBarItem).Tag;

                FrmExport.publicStreetGroup.RefreshForm();
            }
            else if (sender == navBarItemCashBank)
            {
                tableLayoutPanel2.Controls.Add((new FrmCashiers().panelContainer), 1, 0);

                //FrmCashiers.publicInstance.Tag = ((sender) as NavBarItem).Tag;

                //FrmCashiers.publicInstance.RefreshForm();

            }
            else if (sender == navBarItemCollect)
            {
                //tableLayoutPanel2.Controls.Add((new FrmDownload().panelContainer), 1, 0);
                tableLayoutPanel2.Controls.Add((new FrmDownloadSources().panelContainer), 1, 0);
            }
            else if (sender == navBarItemSummary)
            {
                //FrmSummaryReport
                tableLayoutPanel2.Controls.Add((new FrmSummaryReport().panelContainer), 1, 0);

            }
            else if (sender == navBarItemPrintReceipt)
            {
                tableLayoutPanel2.Controls.Add((new FrmPrintOption().panelContainer), 1, 0);
                //tableLayoutPanel2.Controls.Add((new FrmReceipts().panelContainer), 1, 0);
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

    }
}
