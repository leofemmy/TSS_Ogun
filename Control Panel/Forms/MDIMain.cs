using System;
using System.Data.SqlClient;
using System.Windows.Forms;
using Control_Panel.Forms;
using TaxSmartSuite.Class;
using MosesClassLibrary.Utilities;
using DevExpress.XtraNavBar;
using Control_Panel.Class;
using MosesForms = ControlPanelIntegratedLibrary.Forms;
using MosesClasses = ControlPanelIntegratedLibrary.Classes;
using System.Diagnostics;


namespace Control_Panel.Forms
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

        protected string marqueeString = "Tax Smart Control Panel Manager --- Powered by ICMA Services";


        //Methods extMethods = new Methods();

        public MDIMain()
        {
            InitializeComponent();

            publicMDIParent = this;

            MosesInit();

            Load += MDIMainForm_Load;

            Resize += MDIMainForm_Resize;

            FormClosing += MDIMainForm_FormClosing;

            timer.Tick += OnTimerTick;

            Init();



            //NavBars.ManageNavBarControls(navBarControl1, Program.ApplicationCode);

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
            Text = String.Format("ICMA Services :: {2} -->>  For {0} State :: [ Server Name == {1} ]-->> User ID:: {3}", Program.StateName, Program.ServerName, Program.ApplicationName, Program.UserID);
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
            navBarItemReceiptCentre.LinkClicked += OnNavBarItemsClicked;
            navBarItemIncome.LinkClicked += OnNavBarItemsClicked;
            navBarItemStatutory.LinkClicked += OnNavBarItemsClicked;
            navBarItemRelief.LinkClicked += OnNavBarItemsClicked;
            navBarItemColSum.LinkClicked += OnNavBarItemsClicked;
            navBarItemTreasuryCash.LinkClicked += OnNavBarItemsClicked;
            navBarItemOption.LinkClicked += OnNavBarItemsClicked;
            navBarItemStations.LinkClicked += OnNavBarItemsClicked;
            navBarItemStationMap.LinkClicked += OnNavBarItemsClicked;
            navBarItemTypeRevenueMapping.LinkClicked += OnNavBarItemsClicked;

            navBarItemCheckUpdate.LinkClicked += OnNavBarItemsClicked;

        }

        void OnNavBarItemsClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            if (sender == navBarItemState)
            {
                if (string.IsNullOrEmpty(Program.stateCode))
                {
                    TaxSmartSuite.Class.Common.setMessageBox("Default State have not been Set", Program.ApplicationName, 1);

                    tableLayoutPanel2.Controls.Add((new FrmState().panelContainer), 1, 0);

                    FrmState.publicStreetGroup.Tag = ((sender) as NavBarItem).Tag;

                    FrmState.publicStreetGroup.RefreshForm();
                }
                else
                {
                    TaxSmartSuite.Class.Common.setMessageBox("Default State have been Set", Program.ApplicationName, 1);

                    if (MessageBox.Show("Do you want to change default state ?", Program.ApplicationName, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
                    {
                        return;

                    }
                    else
                    {
                        tableLayoutPanel2.Controls.Add((new FrmState().panelContainer), 1, 0);

                        FrmState.publicStreetGroup.Tag = ((sender) as NavBarItem).Tag;

                        FrmState.publicStreetGroup.RefreshForm();
                        //isFirst = false;
                    }
                }

            }
            else if (sender == navBarItemLGName)
            {
                tableLayoutPanel2.Controls.Add((new FrmLGA().panelContainer), 1, 0);

                FrmLGA.publicStreetGroup.Tag = ((sender) as NavBarItem).Tag;

                FrmLGA.publicStreetGroup.RefreshForm();
            }
            else if (sender == navBarItem1Town)
            {
                tableLayoutPanel2.Controls.Add((new FrmTown().panelContainer), 1, 0);

                FrmTown.publicStreetGroup.Tag = ((sender) as NavBarItem).Tag;

                FrmTown.publicStreetGroup.RefreshForm();
            }
            else if (sender == navBarItemCheckUpdate)
            {
                Process prupdate = Process.Start("wyUpdate.exe");
            }
            else if (sender == navBarItemAgency)
            {
                tableLayoutPanel2.Controls.Add((new FrmAgency().panelContainer), 1, 0);

                FrmAgency.publicStreetGroup.Tag = ((sender) as NavBarItem).Tag;

                FrmAgency.publicStreetGroup.RefreshForm();
            }
            else if (sender == navBarItemOption)
            {
                tableLayoutPanel2.Controls.Add((new FrmOptions().panelContainer), 1, 0);

                FrmOptions.publicStreetGroup.Tag = ((sender) as NavBarItem).Tag;

                FrmOptions.publicStreetGroup.RefreshForm();
            }
            else if (sender == navBarItemZone)
            {
                tableLayoutPanel2.Controls.Add((new FrmZone().panelContainer), 1, 0);

                FrmZone.publicStreetGroup.Tag = ((sender) as NavBarItem).Tag;

                FrmZone.publicStreetGroup.RefreshForm();
            }
            else if (sender == navBarItemRevenueType)
            {
                tableLayoutPanel2.Controls.Add((new FrmRevenueType().panelContainer), 1, 0);

                FrmRevenueType.publicStreetGroup.Tag = ((sender) as NavBarItem).Tag;

                FrmRevenueType.publicStreetGroup.RefreshForm();
            }
            else if (sender == navBarItemRevenueTaxOffice)
            {
                tableLayoutPanel2.Controls.Add((new FrmRevenueTaxOffice().panelContainer), 1, 0);

                FrmRevenueTaxOffice.publicStreetGroup.Tag = ((sender) as NavBarItem).Tag;

                FrmRevenueTaxOffice.publicStreetGroup.RefreshForm();
            }
            else if (sender == navBarItemReceiptCentre)
            {
                tableLayoutPanel2.Controls.Add((new FrmReceipts().panelContainer), 1, 0);

                FrmReceipts.publicStreetGroup.Tag = ((sender) as NavBarItem).Tag;

                FrmReceipts.publicStreetGroup.RefreshForm();
            }
            else if (sender == navBarItemBank)
            {
                tableLayoutPanel2.Controls.Add((new FrmBank().panelContainer), 1, 0);

                FrmBank.publicStreetGroup.Tag = ((sender) as NavBarItem).Tag;

                FrmBank.publicStreetGroup.RefreshForm();
            }
            else if (sender == navBarItemBranch)
            {
                tableLayoutPanel2.Controls.Add((new FrmBankBranch().panelContainer), 1, 0);

                FrmBankBranch.publicStreetGroup.Tag = ((sender) as NavBarItem).Tag;

                FrmBankBranch.publicStreetGroup.RefreshForm();
            }
            else if (sender == navBarItemBankAccount)
            {
                tableLayoutPanel2.Controls.Add((new FrmAccountBank().panelContainer), 1, 0);

                FrmAccountBank.publicStreetGroup.Tag = ((sender) as NavBarItem).Tag;

                FrmAccountBank.publicStreetGroup.RefreshForm();
            }
            else if (sender == navBarItemPlatform)
            {
                tableLayoutPanel2.Controls.Add((new FrmPlatForm().panelContainer), 1, 0);

                FrmPlatForm.publicStreetGroup.Tag = ((sender) as NavBarItem).Tag;

                FrmPlatForm.publicStreetGroup.RefreshForm();
            }
            else if (sender == navBarItemCurrency)
            {
                tableLayoutPanel2.Controls.Add((new FrmCurrency().panelContainer), 1, 0);

                FrmCurrency.publicStreetGroup.Tag = ((sender) as NavBarItem).Tag;

                FrmCurrency.publicStreetGroup.RefreshForm();
            }
            else if (sender == navBarItemBusniessClass)
            {
                tableLayoutPanel2.Controls.Add((new FrmBuss().panelContainer), 1, 0);

                FrmBuss.publicStreetGroup.Tag = ((sender) as NavBarItem).Tag;

                FrmBuss.publicStreetGroup.RefreshForm();
            }
            else if (sender == navBarItemStations)
            {
                tableLayoutPanel2.Controls.Add((new FrmStations().panelContainer), 1, 0);

                FrmStations.publicStreetGroup.Tag = ((sender) as NavBarItem).Tag;

                FrmStations.publicStreetGroup.RefreshForm();

            }
            else if (sender == navBarItemStationMap)
            {
                tableLayoutPanel2.Controls.Add((new FrmStationMap().panelContainer), 1, 0);

                FrmStationMap.publicStreetGroup.Tag = ((sender) as NavBarItem).Tag;

                FrmStationMap.publicStreetGroup.RefreshForm();
            }
            else if (sender == navBarItemBusinessSub)
            {
                tableLayoutPanel2.Controls.Add((new FrmBussSubClass().panelContainer), 1, 0);

                FrmBussSubClass.publicStreetGroup.Tag = ((sender) as NavBarItem).Tag;

                FrmBussSubClass.publicStreetGroup.RefreshForm();
            }
            else if (sender == navBarItemTypeRevenueMapping)
            {
                tableLayoutPanel2.Controls.Add((new FrmTaxPayerTypeRev().panelContainer), 1, 0);

                FrmTaxPayerTypeRev.publicStreetGroup.Tag = ((sender) as NavBarItem).Tag;

                FrmTaxPayerTypeRev.publicStreetGroup.RefreshForm();
            }
            else if (sender == navBarItemIncome)
            {
                //tableLayoutPanel2.Controls.Add((new MosesForms.FrmChargeableIncome()).panelContainer, 1, 0);
                //tableLayoutPanel2.Controls.Add((new MosesForms.FrmChargeableIncome()).panelContainer,1,0);
                //MosesForms.FrmChargeableIncome.publicInstance.Tag = ((sender) as NavBarItem).Tag;
                ////this.tableLayoutPanel2.Controls.Add()
                //tableLayoutPanel2.Controls.Add((new FrmIncome().panelContainer), 1, 0);

                //FrmIncome.publicStreetGroup.Tag = ((sender) as NavBarItem).Tag;

                //FrmIncome.publicStreetGroup.RefreshForm();
            }
            else if (sender == navBarItemStatutory)
            {
                //tableLayoutPanel2.Controls.Add((new FrmDeduction().panelContainer), 1, 0);
                //tableLayoutPanel2.Controls.Add((new MosesForms.FrmStatutoryDeduction()).panelContainer, 1, 0);
                //MosesForms.FrmStatutoryDeduction.publicInstance.Tag = ((sender) as NavBarItem).Tag;

            }
            else if (sender == navBarItemRelief)
            {
                //this.tableLayoutPanel2.Controls.Add((new MosesForms.FrmRelief()).panelContainer, 1, 0);
                //MosesForms.FrmRelief.publicInstance.Tag = ((sender) as NavBarItem).Tag;

            }
            else if (sender == navBarItemColSum)
            {
                this.tableLayoutPanel2.Controls.Add((new FrmRepCollSummary().panelContainer), 1, 0);

                FrmRepCollSummary.publicStreetGroup.Tag = ((sender) as NavBarItem).Tag;

                FrmRepCollSummary.publicStreetGroup.RefreshForm();

            }
            else if (sender == navBarItemTreasuryCash)
            {
                tableLayoutPanel2.Controls.Add((new FrmTreasury().panelContainer), 1, 0);

                FrmTreasury.publicStreetGroup.Tag = ((sender) as NavBarItem).Tag;

                FrmTreasury.publicStreetGroup.RefreshForm();
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

        private void MDIMain_Load(object sender, EventArgs e)
        {
            //this.toolStrip1.Renderer = new Office2007Renderer();
        }

        void MosesInit()
        {
            MosesClasses.Settings.menuImageList = i16x16;
            MosesClasses.Settings.BttnImageList = i32x32;
            MosesClasses.Settings.tableLayout = tableLayoutPanel2;
            //MosesClasses.Settings.PanelControl = panelContainer;
            MosesClasses.Settings.StateCode = Program.stateCode;
            MosesClasses.Settings.ConnectionString = Class.Logic.ConnectionString;
            MosesClasses.Settings.ApplicationName = Program.ApplicationName;
        }
    }
}
