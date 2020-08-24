using System;
using System.Data.SqlClient;
using System.Windows.Forms;
using MosesClassLibrary.Utilities;
using DevExpress.XtraNavBar;
using TaxSmartSuite.Class;
using System.Data;
using Inventory.Class;
using Inventory.Forms;



namespace Inventory.Forms
{
    public partial class MDIMain : Form
    {
        private string retval;
       
        private int childFormNumber;

        //public static string stateCode;

        public string ReceiptOfficecode;

        public static MDIMain publicMDIParent;

        Timer timer = new Timer();

        int lockTimerCounter;

        int offsetCounter = 0;

        protected string marqueeString = "Inventory Manager --- Powered by ICMA Services";

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

            //string query = "select CentreCode from [tblReceiptOffice]";

            //DataTable dts = (new Logic()).getSqlStatement(query).Tables[0];

            //if (dts != null)
            //{
            //    ReceiptOfficecode = dts.Rows[0]["CentreCode"].ToString();
            //}
            

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
             navBarItemExit.LinkClicked += OnNavBarItemsClicked;

             navBarItemPlate.LinkClicked += OnNavBarItemsClicked;

             navBarItemMLO.LinkClicked += OnNavBarItemsClicked;

             navBarItemReceived.LinkClicked += OnNavBarItemsClicked;

             navBarItemIssue.LinkClicked += OnNavBarItemsClicked;
        }

        void OnNavBarItemsClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            if (sender == navBarItemPlate)
            {
                //tableLayoutPanel2.Controls.Add((new FrmNormalize().panelContainer), 1, 0);

                //tableLayoutPanel2.Controls.Add((new FrmGeneral().panelContainer), 1, 0);

                //FrmImports.publicStreetGroup.Tag = ((sender) as NavBarItem).Tag;

                //FrmImports.publicStreetGroup.RefreshForm();

                tableLayoutPanel2.Controls.Add((new FrmPlate().panelContainer), 1, 0);
            }
            else if (sender == navBarItemMLO)
            {
                tableLayoutPanel2.Controls.Add((new FrmRegMLO().panelContainer), 1, 0);
            }
            else if (sender == navBarItemReceived)
            {
                tableLayoutPanel2.Controls.Add((new FrmFRSCReceive().panelContainer), 1, 0);

            }
            else if (sender == navBarItemIssue)
            {
                tableLayoutPanel2.Controls.Add((new FrmIssues().panelContainer), 1, 0);
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

        private void panelContainer_Paint(object sender, PaintEventArgs e)
        {

        }
 
    }
}
