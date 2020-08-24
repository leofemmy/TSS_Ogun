using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Collection.Forms
{
    public partial class FrmDownload : Form
    {
        public static FrmDownload publicStreetGroup;

        public FrmDownload()
        {
            InitializeComponent();

            publicStreetGroup = this;

            setImages();

            ToolStripEvent();

            Load += OnFormLoad;

            OnFormLoad(null, null);

            //NavBars.ToolStripEnableDisable(toolStrip, null, false);
        }

        private void FrmDownload_Load(object sender, EventArgs e)
        {
            //webBrowser1.Navigate("http://localhost/DataManager/index.html");
            
        }

        private void setImages()
        {
            tsbNew.Image = MDIMain.publicMDIParent.i16x16.Images[5];

            tsbEdit.Image = MDIMain.publicMDIParent.i16x16.Images[6];

            tsbDelete.Image = MDIMain.publicMDIParent.i16x16.Images[7];

            tsbReload.Image = MDIMain.publicMDIParent.i16x16.Images[0];

            tsbClose.Image = MDIMain.publicMDIParent.i16x16.Images[11];

          

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
            }
        }

        void OnFormLoad(object sender, EventArgs e)
        {
            webBrowser1.Navigate("http://localhost/DataManager/index.html");
        }



    }
}
