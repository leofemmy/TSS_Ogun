using System;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;

namespace Control_Panel.Forms
{
    public partial class FrmTaxPayerType : Form
    {
        MDIMain md = new MDIMain();

        public FrmTaxPayerType()
        {
            InitializeComponent();
        }

        private void FrmTaxPayerType_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'dsBank.tblTaxPayerType' table. You can move, or remove it, as needed.
            tblTaxPayerTypeTableAdapter.Fill(dsBank.tblTaxPayerType);

        }

        private void gvTaxPayer_InitNewRow(object sender, InitNewRowEventArgs e)
        {
            GridView view = sender as GridView;

            view.SetRowCellValue(e.RowHandle, view.Columns["StateCode"], MDIMain.stateCode);
        }

        private void gcTaxPayer_EmbeddedNavigator_ButtonClick(object sender, NavigatorButtonClickEventArgs e)
        {
            //Append Button is Click
            if (e.Button.ButtonType == NavigatorButtonType.Append)
            {
                try
                {
                    //Save the latest changes to the bound DataTable 
                    ColumnView View = (ColumnView)gcTaxPayer.FocusedView;
                    if (!(View.PostEditor() && View.UpdateCurrentRow()))
                        return;

                    tblTaxPayerTypeTableAdapter.Update (dsBank.tblTaxPayerType);

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
                    ColumnView ViewDel = (ColumnView)gcTaxPayer.FocusedView;
                    ViewDel.DeleteSelectedRows();

                    tblTaxPayerTypeTableAdapter.Update(dsBank.tblTaxPayerType);

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
                    ColumnView View1 = (ColumnView)gcTaxPayer.FocusedView;

                    if (!(View1.PostEditor() && View1.UpdateCurrentRow()))
                        return;

                    tblTaxPayerTypeTableAdapter.Update(dsBank.tblTaxPayerType);

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
