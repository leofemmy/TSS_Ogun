using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using TaxSmartSuite.Class;

namespace Collection.Forms
{
    public partial class FrmBudget : Form
    {
        string statecode;

        private TaxSmartSuite.Class.Methods extMethod = new Methods();

        public FrmBudget()
        {
            InitializeComponent();

            statecode = extMethod.getQuery("statecode", "");

        }

        private void FrmBudget_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'dsRevenueEditor.tblRevenueType' table. You can move, or remove it, as needed.
            this.tblRevenueTypeTableAdapter.Fill(this.dsRevenueEditor.tblRevenueType);
            // TODO: This line of code loads data into the 'dsBudget.tblBudget' table. You can move, or remove it, as needed.
            this.tblBudgetTableAdapter.Fill(this.dsBudget.tblBudget);

        }

        private void gvBudget_InitNewRow(object sender, DevExpress.XtraGrid.Views.Grid.InitNewRowEventArgs e)
        {
            GridView view = sender as GridView;
            view.SetRowCellValue(e.RowHandle, view.Columns["Year"], DateTime.Today.Year);
            GridView views = sender as GridView;
            views.SetRowCellValue(e.RowHandle, view.Columns["StateCode"], statecode);
        }

        private void gcBudget_EmbeddedNavigator_ButtonClick(object sender, DevExpress.XtraEditors.NavigatorButtonClickEventArgs e)
        {
            //Append Button is Click
            if (e.Button.ButtonType == NavigatorButtonType.Append)
            {
                try
                {
                    //Save the latest changes to the bound DataTable 
                    ColumnView View = (ColumnView)gcBudget.FocusedView;
                    if (!(View.PostEditor() && View.UpdateCurrentRow()))
                        return;

                    this.tblBudgetTableAdapter.Update(this.dsBudget.tblBudget);

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + "  " + ex.StackTrace);

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
                    ColumnView ViewDel = (ColumnView)gcBudget.FocusedView;
                    ViewDel.DeleteSelectedRows();

                    return;

                    this.tblBudgetTableAdapter.Update(this.dsBudget.tblBudget);

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + "  " + ex.StackTrace);

                    throw;
                }

            }
            //Update Change made to the Grid
            if (e.Button.ButtonType == NavigatorButtonType.EndEdit)
            {
                try
                {
                    //Update the latest changes to the bound DataTable 
                    ColumnView View1 = (ColumnView)gcBudget.FocusedView;

                    if (!(View1.PostEditor() && View1.UpdateCurrentRow()))
                        return;

                    this.tblBudgetTableAdapter.Update(this.dsBudget.tblBudget);

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
