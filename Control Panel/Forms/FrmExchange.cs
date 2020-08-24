using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid.Views.Base;

namespace Control_Panel.Forms
{
    public partial class FrmExchange : Form
    {
        public FrmExchange()
        {
            InitializeComponent();
        }

        private void FrmExchange_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'dsBank.tblCurrency1' table. You can move, or remove it, as needed.
            tblCurrency1TableAdapter.Fill(dsBank.tblCurrency1);
            // TODO: This line of code loads data into the 'dsBank.tblCurrency' table. You can move, or remove it, as needed.
            this.tblCurrencyTableAdapter.Fill(dsBank.tblCurrency);
            // TODO: This line of code loads data into the 'dsBank.tblExchangeRate' table. You can move, or remove it, as needed.
            tblExchangeRateTableAdapter.Fill(this.dsBank.tblExchangeRate);

        }

        private void gcExchange_EmbeddedNavigator_ButtonClick(object sender, NavigatorButtonClickEventArgs e)
        {
            //Append Button is Click
            if (e.Button.ButtonType == NavigatorButtonType.Append)
            {
                try
                {
                    //Save the latest changes to the bound DataTable 
                    ColumnView View = (ColumnView)gcExchange.FocusedView;
                    if (!(View.PostEditor() && View.UpdateCurrentRow()))
                        return;

                    this.tblExchangeRateTableAdapter.Update(this.dsBank.tblExchangeRate);


                }
                catch (Exception ex)
                {
                    MessageBox.Show(String.Format("{0}  {1}", ex.Message, ex.StackTrace));

                    throw;
                }

            }
            //Remove cell from Grid view
            if (e.Button.ButtonType == NavigatorButtonType.Remove)
            {
                if (MessageBox.Show("Do You Want To Delete This Record ? ", "Confirm Delete", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == DialogResult.Yes)

                    e.Handled = true;
                try
                {
                    //Delete Selected Rows that is  bound DataTable 
                    ColumnView ViewDel = (ColumnView)gcExchange.FocusedView;
                    ViewDel.DeleteSelectedRows();

                    this.tblExchangeRateTableAdapter.Update(this.dsBank.tblExchangeRate);

                }
                catch (Exception ex)
                {
                    MessageBox.Show(String.Format("{0}  {1}", ex.Message, ex.StackTrace));

                    throw;
                }

            }
            //Update Change made to the Grid
            if (e.Button.ButtonType == NavigatorButtonType.EndEdit)
            {
                try
                {
                    //Update the latest changes to the bound DataTable 
                    ColumnView View1 = (ColumnView)gcExchange.FocusedView;

                    if (!(View1.PostEditor() && View1.UpdateCurrentRow()))
                        return;

                    tblExchangeRateTableAdapter.Update(dsBank.tblExchangeRate);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + "  " + ex.StackTrace);

                    throw;
                }
            }
        }
    }
}
