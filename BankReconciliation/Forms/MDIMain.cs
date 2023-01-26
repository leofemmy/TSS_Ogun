using BankReconciliation.Class;
using DevExpress.XtraNavBar;
using System;
using System.Data;
using System.Diagnostics;
using System.Windows.Forms;
using TaxSmartSuite.Class;


namespace BankReconciliation.Forms
{
    public partial class MDIMain : Form
    {
        private string retval;
        //FrmSign sign = new FrmSign();

        //Methods extMothds = new Methods();

        //DBConnection connects = new DBConnection();


        private int childFormNumber;

        //public static string stateCode;

        public string ReceiptOfficecode;

        public static MDIMain publicMDIParent;

        Timer timer = new Timer();

        int lockTimerCounter;

        int offsetCounter = 0;

        protected string marqueeString = " Bank Reconciliation Manager --- Powered by ICMA Services";

        Methods extMethods = new Methods();

        public MDIMain()
        {
            InitializeComponent();

            publicMDIParent = this;

            Load += MDIMainForm_Load;

            Resize += MDIMainForm_Resize;

            FormClosing += MDIMainForm_FormClosing;

            timer.Tick += OnTimerTick;

            Init();

            DataTable dts = (new Logic()).getSqlStatement("select CentreCode from [tblReceiptOffice]").Tables[0];

            if (dts != null && dts.Rows.Count > 0)
            {
                ReceiptOfficecode = dts.Rows[0]["CentreCode"].ToString();
            }


            //NavBars.ManageNavBarControls(navBarControl1, Program.ApplicationCode);

            ////NavBars.NavBarEnableDisableControls(navBarControl1, false);

            ////NavBars.ManageUserLoginNavBar(navBarControl1);
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

            navBarItemExit.LinkClicked += OnNavBarItemsClicked;

            navBarItemBankStatement.LinkClicked += OnNavBarItemsClicked;

            navBarItemPlateNumber.LinkClicked += OnNavBarItemsClicked;

            navBarItemPostExPayDirect.LinkClicked += OnNavBarItemsClicked;

            navBarItemIssued.LinkClicked += OnNavBarItemsClicked;

            navBarItemValidateExPayDirect.LinkClicked += OnNavBarItemsClicked;

            navBarItemBankAccount.LinkClicked += OnNavBarItemsClicked;

            navBarItemTransactionDefinition.LinkClicked += OnNavBarItemsClicked;

            navBarItemTransactionPosting.LinkClicked += OnNavBarItemsClicked;

            navBarItemReconciliationAccount.LinkClicked += OnNavBarItemsClicked;

            navBarItemBankReconciliation.LinkClicked += OnNavBarItemsClicked;

            navBarItemTransactionType.LinkClicked += OnNavBarItemsClicked;

            navBarItemFiscailPeriod.LinkClicked += OnNavBarItemsClicked;

            navBarItemAgencySetUp.LinkClicked += OnNavBarItemsClicked;

            navBarItemRevenueSetup.LinkClicked += OnNavBarItemsClicked;

            navBarItemZonalRevenue.LinkClicked += OnNavBarItemsClicked;

            navBarItemCashierBook.LinkClicked += OnNavBarItemsClicked;

            navBarItemOfficeRequest.LinkClicked += OnNavBarItemsClicked;

            navBarItemCheckUpdate.LinkClicked += OnNavBarItemsClicked;
        }

        void OnNavBarItemsClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            if ((sender != navBarItemExit))
            {
                RemoveControls();
            }

            if (sender == navBarItemBankStatement)
            {
                //tableLayoutPanel2.Controls.Add((new FrmNormalize().panelContainer), 1, 0);

                //tableLayoutPanel2.Controls.Add((new FrmGeneral().panelContainer), 1, 0);

                //tableLayoutPanel2.Controls.Add((new Form1().panelContainer), 1, 0);
                tableLayoutPanel2.Controls.Add((new FrmExecption().panelControl1), 1, 0);

                FrmExecption.publicStreetGroup.Tag = ((sender) as NavBarItem).Tag;

                FrmExecption.publicStreetGroup.RefreshForm();


            }
            else if (sender == navBarItemOtherReceipt)
            {
                //tableLayoutPanel2.Controls.Add((new FrmPrintReceipt().panelContainer), 1, 0);

            }
            else if (sender == navBarItemCheckUpdate)
            {

                Process prupdate = Process.Start("wyUpdate.exe");
                //Process p = Process.Start("wyUpdate.exe");  
            }
            else if (sender == navBarItemPostExPayDirect)
            {
                //tableLayoutPanel2.Controls.Add((new FrmReceived().panelContainer), 1, 0);
                tableLayoutPanel2.Controls.Add((new FrmPostExPayDirect().panelContainer), 1, 0);

                FrmPostExPayDirect.publicStreetGroup.Tag = ((sender) as NavBarItem).Tag;

                FrmPostExPayDirect.publicStreetGroup.RefreshForm();
            }
            else if (sender == navBarItemOfficeRequest)
            {
                tableLayoutPanel2.Controls.Add((new FrmStateRef().panelContainer), 1, 0);

                FrmStateRef.publicStreetGroup.Tag = ((sender) as NavBarItem).Tag;

                FrmStateRef.publicStreetGroup.RefreshForm();
            }
            else if (sender == navBarItemValidateExPayDirect)
            {
                //tableLayoutPanel2.Controls.Add((new FrmRegistration().panelContainer), 1, 0);
                tableLayoutPanel2.Controls.Add((new FrmValidateExPayDirect().panelContainer), 1, 0);

                FrmValidateExPayDirect.publicStreetGroup.Tag = ((sender) as NavBarItem).Tag;

                FrmValidateExPayDirect.publicStreetGroup.RefreshForm();
            }
            else if (sender == navBarItemBankAccount)
            {
                tableLayoutPanel2.Controls.Add((new FrmAccountBank().panelContainer), 1, 0);

                FrmAccountBank.publicStreetGroup.Tag = ((sender) as NavBarItem).Tag;

                FrmAccountBank.publicStreetGroup.RefreshForm();
            }
            else if (sender == navBarItemTransactionDefinition)
            {
                tableLayoutPanel2.Controls.Add((new FrmTransactionSetup().panelContainer), 1, 0);

                FrmTransactionSetup.publicStreetGroup.Tag = ((sender) as NavBarItem).Tag;

                FrmTransactionSetup.publicStreetGroup.RefreshForm();

            }
            else if (sender == navBarItemFiscailPeriod)
            {
                tableLayoutPanel2.Controls.Add((new FrmPeriods().panelContainer), 1, 0);

                FrmPeriods.publicStreetGroup.Tag = ((sender) as NavBarItem).Tag;

                FrmPeriods.publicStreetGroup.RefreshForm();
            }
            else if (sender == navBarItemTransactionPosting)
            {
                //tableLayoutPanel2.Controls.Add((new FrmPostTransaction().panelContainer),1,0);
                tableLayoutPanel2.Controls.Add((new FrmTransactionPost().panelContainer), 1, 0);

                //FrmTransactionPost.publicStreetGroup.Tag = ((sender) as NavBarItem).Tag;

                //FrmTransactionPost.publicStreetGroup.RefreshForm();
            }
            else if (sender == navBarItemReconciliationAccount)
            {
                //tableLayoutPanel2.Controls.Add((new FrmAccountView().panelContainer), 1, 0);

                //FrmAccountView.publicStreetGroup.Tag = ((sender) as NavBarItem).Tag;

                //FrmAccountView.publicStreetGroup.RefreshForm();
            }
            else if (sender == navBarItemTransactionType)
            {
                tableLayoutPanel2.Controls.Add((new FrmViewTransaction().panelContainer), 1, 0);

                FrmViewTransaction.publicStreetGroup.Tag = ((sender) as NavBarItem).Tag;

                FrmViewTransaction.publicStreetGroup.RefreshForm();
            }
            else if (sender == navBarItemBankReconciliation)
            {
                tableLayoutPanel2.Controls.Add((new FrmIGR().panelContainer), 1, 0);

                FrmIGR.publicStreetGroup.Tag = ((sender) as NavBarItem).Tag;

                FrmIGR.publicStreetGroup.RefreshForm();
            }
            else if (sender == navBarItemAgencySetUp)
            {
                tableLayoutPanel2.Controls.Add((new FrmAgencyUk().panelContainer), 1, 0);

                FrmAgencyUk.publicStreetGroup.Tag = ((sender) as NavBarItem).Tag;

                FrmAgencyUk.publicStreetGroup.RefreshForm();
            }
            else if (sender == navBarItemZonalRevenue)
            {
                tableLayoutPanel2.Controls.Add((new FrmZonalTemp().panelContainer), 1, 0);

                FrmZonalTemp.publicStreetGroup.Tag = ((sender) as NavBarItem).Tag;

                FrmZonalTemp.publicStreetGroup.RefreshForm();
            }
            else if (sender == navBarItemRevenueSetup)
            {
                tableLayoutPanel2.Controls.Add((new FrmRevenuetemp().panelContainer), 1, 0);

                FrmRevenuetemp.publicStreetGroup.Tag = ((sender) as NavBarItem).Tag;

                FrmRevenuetemp.publicStreetGroup.RefreshForm();
            }
            else if (sender == navBarItemCashierBook)
            {
                tableLayoutPanel2.Controls.Add((new FrmCashiers().panelContainer), 1, 0);

                //FrmCashiers.publicStreetGroup.Tag = ((sender) as NavBarItem).Tag;

                //FrmCashiers.publicStreetGroup.RefreshForm();
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
            if (MosesClassLibrary.Utilities.Common.AskQuestion("Are you sure you want to Close this application?", "Road Taxes Manager"))
                bRes = true;
            else
                bRes = false;
            return bRes;
        }

    }
}
