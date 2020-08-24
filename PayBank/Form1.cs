using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PayBank
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            btnimportB.Click += BtnimportB_Click;
        }

        private void BtnimportB_Click(object sender, EventArgs e)
        {
            MessageBoxManager.Unregister();
            //MessageBoxManager.OK = "Excel 2003";
            MessageBoxManager.No = "Excel 2007";
            MessageBoxManager.Yes = "Excel 2003";
            MessageBoxManager.Register();

            DialogResult result = MessageBox.Show("Select Excel File Type", "Import Statement", MessageBoxButtons.YesNo, MessageBoxIcon.Question);


            if (result == DialogResult.Yes)//excele 2003
            {
                try
                {

                }
                catch (Exception ex)
                {
                    Tripous.Sys.ErrorBox(String.Format(" Modify the excel to contain this Column Header 'DATE','AOUNT'.... {0}{1} ...Import Data Error", ex.Message, ex.StackTrace));

                    return;

                }
            }
            if (result == DialogResult.No)//excele 2007
            {
            }

            MessageBoxManager.Unregister();
        }
    }
}
