using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Normalization.Forms
{
    public partial class FrmDownload : Form
    {
        public FrmDownload()
        {
            InitializeComponent();
        }

        private void FrmDownload_Load(object sender, EventArgs e)
        {
            webBrowser1.Navigate("http://localhost/DataManager/index.html");
        }
    }
}
