using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TaxSmartSuite.Class;

namespace Control_Panel.Forms
{
    public partial class FrmDefualtState : Form
    {

        MDIMain md = new MDIMain();
        Methods extMethods = new Methods();

        public static string statecode;
  

        public FrmDefualtState()
        {
            InitializeComponent();
        }

        private void FrmDefualtState_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'dsBank.tblState1' table. You can move, or remove it, as needed.
            this.tblState1TableAdapter.Fill(this.dsBank.tblState1);

        }

        private void btnok_Click(object sender, EventArgs e)
        {
            MDIMain.stateCode   = this.cboState.SelectedValue.ToString();
            this.Close();
        }

        private void cboState_KeyPress(object sender, KeyPressEventArgs e)
        {
            Methods.AutoComplete(cboState, e, true);
        }
    }
}
