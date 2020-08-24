using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
//using TaxSmartRegistration.Demography;
using MosesClassLibrary.Utilities;
//using TaxSmartRegistration.Transactions;
using DevExpress.XtraNavBar;
using UserManager.Forms;

namespace UserManager
{
    public partial class MDIMainForm : Form
    {
        //FORM VARIABLES
        public static MDIMainForm publicMDIParent;

        Timer timer = new Timer();
        int lockTimerCounter;
        int offsetCounter = 0;

        protected string marqueeString = String.Format("{1} --- Powered by ICMA Services --- For {0} State", Program.StateName, Program.ApplicationName);

        public MDIMainForm()
        {
            InitializeComponent();
            publicMDIParent = this;
            Load += MDIMainForm_Load;
            Resize += MDIMainForm_Resize;
            FormClosing += new FormClosingEventHandler(MDIMainForm_FormClosing);
            timer.Tick += OnTimerTick;
            Init();
            //GetNavBarLinked();
            //Classess.NavBars.ManageNavBarControls(this.navBarControl1, Program.ApplicationCode);
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

        void MDIMainForm_Resize(object sender, EventArgs e)
        {
            gaugeControl1.Left = (panelControl1.Width - gaugeControl1.Width) / 2;
        }

        void MDIMainForm_Load(object sender, EventArgs e)
        {
            Text = String.Format("ICMA Services :: {2} -->>  For {0} State [ Server Name == {1} ]" 
                , Program.StateName, Program.ServerName, Program.ApplicationName);
            MenuEvent();
            OnNavBarItemsClicked(navBarItemManageUser, null);
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

        public void ReloadMarquee(string str)
        {
            marqueeString = str;
            Init();
        }

         protected void MenuEvent()
        {
            navBarItemManageUser.LinkClicked += OnNavBarItemsClicked;
            navBarItemStreetName.LinkClicked += OnNavBarItemsClicked;
            navBarItemTaxAgent.LinkClicked += OnNavBarItemsClicked;
            navBarItemTaxPayer.LinkClicked += OnNavBarItemsClicked;
            navBarItemPhotograph.LinkClicked += OnNavBarItemsClicked;
            navBarItemExit.LinkClicked += OnNavBarItemsClicked;
        }

         void OnNavBarItemsClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
         {
             RemoveControls();
             if (sender == navBarItemManageUser)
             {
                 this.tableLayoutPanel2.Controls.Add((new FrmManageUser2()).panelContainer, 1, 0);
                 FrmManageUser2.publicUser.RefreshForm();
             }
             else if (sender == navBarItemStreetName)
             {
                 //this.tableLayoutPanel2.Controls.Add((new FrmStreetName()).panelContainer, 1, 0);
             }
             else if (sender == navBarItemTaxAgent)
             {
                 //this.tableLayoutPanel2.Controls.Add((new FrmTaxAgentRegistration()).panelContainer, 1, 0);
             }
             else if (sender == navBarItemTaxPayer)
             {
                 //this.tableLayoutPanel2.Controls.Add((new FrmTaxPayerRegistration()).panelContainer, 1, 0);
             }
             else if (sender == navBarItemPhotograph)
             {
                 //this.tableLayoutPanel2.Controls.Add((new FrmTaxPayerPhotograph()).panelContainer, 1, 0);
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
             if (Common.AskQuestion("Are you sure you want to Close this application?", "Tax Smart Registration"))
                 bRes = true;
             else
                 bRes = false;
             return bRes;
         }

         void MDIMainForm_FormClosing(object sender, FormClosingEventArgs e)
         {
             e.Cancel = !CloseForm();
         }

         void GetNavBarLinked()
         {
             string display = string.Empty;
             foreach (NavBarGroup GroupList in navBarControl1.Groups)
             {
                 int lol = navBarControl1.Groups.Count;
                 int lool = GroupList.VisibleItemLinks.Count;
                 foreach (NavBarItemLink ItemList in GroupList.VisibleItemLinks)
                 {
                     NavBarItem items = ItemList.Item;
                     display += String.Format("{0} -- {1} -- {2}", GroupList.Caption, ItemList.Caption, items.Tag) + Environment.NewLine;
                     //MessageBox.Show();
                 }
             }
             Tripous.Sys.CourierBox(display);
             //MessageBox.Show(display);
             //foreach (NavBarItem item in navBarControl1.Items)
             //{
             //    //MessageBox.Show(item.Caption);
             //}
         }
    }
}
