using System;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Base;


namespace Control_Panel.Forms
{
    public partial class FrmIncomeType : Form
    {
        public FrmIncomeType()
        {
            InitializeComponent();
        }

        private void FrmIncomeType_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'dsBank.tblIncomeClass' table. You can move, or remove it, as needed.
            tblIncomeClassTableAdapter.Fill(dsBank.tblIncomeClass);
            // TODO: This line of code loads data into the 'dsBank.tblIncomeSource' table. You can move, or remove it, as needed.
            tblIncomeSourceTableAdapter.Fill(dsBank.tblIncomeSource);
            // TODO: This line of code loads data into the 'dsBank.tblIncomeType' table. You can move, or remove it, as needed.
            tblIncomeTypeTableAdapter.Fill(dsBank.tblIncomeType);

        }

        private void gcIncomeTax_EmbeddedNavigator_ButtonClick(object sender, NavigatorButtonClickEventArgs e)
        {
            //Append Button is Click
            if (e.Button.ButtonType == NavigatorButtonType.Append)
            {
                try
                {
                    //Save the latest changes to the bound DataTable 
                    ColumnView View = (ColumnView)gcIncomeTax.FocusedView;
                    if (!(View.PostEditor() && View.UpdateCurrentRow()))
                        return;

                    tblIncomeTypeTableAdapter.Update(dsBank.tblIncomeType);

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
                    ColumnView ViewDel = (ColumnView)gcIncomeTax.FocusedView;
                    ViewDel.DeleteSelectedRows();

                    tblIncomeTypeTableAdapter.Update(dsBank.tblIncomeType);

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
                    ColumnView View1 = (ColumnView)gcIncomeTax.FocusedView;

                    if (!(View1.PostEditor() && View1.UpdateCurrentRow()))
                        return;

                    tblIncomeTypeTableAdapter.Update(dsBank.tblIncomeType);

                }
                catch (Exception ex)
                {
                    MessageBox.Show(String.Format("{0}  {1}", ex.Message, ex.StackTrace));

                    throw;
                }
            }
        }
    }
}
