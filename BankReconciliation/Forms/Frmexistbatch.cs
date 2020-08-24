using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using BankReconciliation.Class;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraSplashScreen;

namespace BankReconciliation.Forms
{
    public partial class Frmexistbatch : Form
    {
        private SqlCommand _command;
        private SqlDataAdapter adp;

        public string ReturnValue1, ReturnValue2;

        public Frmexistbatch()
        {
            SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

            InitializeComponent();
            
            setReloadGrid();
            
            gridView1.DoubleClick += gridView1_DoubleClick;

            SplashScreenManager.CloseForm(false);
        }

        void gridView1_DoubleClick(object sender, EventArgs e)
        {
            if (gridView1.IsGroupRow(gridView1.FocusedRowHandle))
            {
                Console.WriteLine("Yes");
                 GridView view = (GridView)gridControl1.FocusedView;

            if (view != null)
            {
                DataRow dr = view.GetDataRow(view.FocusedRowHandle);

                if (dr != null)
                {
                    ReturnValue1 = Convert.ToString(dr["BatchCode"]);
                    ReturnValue2 = dr["BatchName"].ToString();
                    DialogResult = System.Windows.Forms.DialogResult.OK;
                    this.Close();
                }
                //else
                //    return;
            }
                
            }
        }

        void setReloadGrid()
        {
            using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
            {
                connect.Open();

                _command = new SqlCommand("dogetBatch", connect) { CommandType = CommandType.StoredProcedure };
               
                using (System.Data.DataSet ds = new System.Data.DataSet())
                {
                    ds.Clear();
                    
                    adp = new SqlDataAdapter(_command);
                    adp.Fill(ds);
                    //Dts = ds.Tables[0];
                    connect.Close();

                    //dtResult = ds;

                    if (ds.Tables[0].Rows[0]["returnCode"].ToString() == "00")
                    {
                        groupBox1.Text = ds.Tables[0].Rows[0]["returnMessage"].ToString();
                        gridControl1.DataSource = ds.Tables[1];
                        gridView1.Columns["BatchName"].Group();
                        gridView1.Columns["BatchName"].OptionsColumn.AllowEdit = false;
                        gridView1.Columns["BankShortCode"].OptionsColumn.AllowEdit = false;
                        gridView1.Columns["OpeningBalance"].OptionsColumn.AllowEdit = false;
                        gridView1.Columns["ClosingBalance"].OptionsColumn.AllowEdit = false;
                        gridView1.Columns["Start Date"].OptionsColumn.AllowEdit = false;
                        gridView1.Columns["End Date"].OptionsColumn.AllowEdit = false;
                        gridView1.Columns["BatchCode"].Visible = false;
                    }
                   
                    gridView1.ExpandAllGroups();
                    gridView1.OptionsView.ColumnAutoWidth = false;
                    gridView1.BestFitColumns();
                }
            }
        }
    }
}
