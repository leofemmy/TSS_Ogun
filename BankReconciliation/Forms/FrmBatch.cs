using BankReconciliation.Class;
using DevExpress.XtraSplashScreen;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Windows.Forms;
using TaxSmartSuite.Class;

namespace BankReconciliation.Forms
{
    public partial class FrmBatch : Form
    {
        private SqlCommand _command; private SqlDataAdapter adp;

        public string ReturnValue1, ReturnValue2;

        DataTable dt = new DataTable();
        public FrmBatch()
        {
            SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

            InitializeComponent();

            spinEdit1.Value = DateTime.Today.Year;

            var dtf = CultureInfo.CurrentCulture.DateTimeFormat;

            int j = 12;

            ToolStripEvent();

            //dt.Columns.Add
            dt.Columns.Add("Months", typeof(String));
            dt.Columns.Add("MonthsVal", typeof(String));

            for (int i = 1; i <= j; i++)
            {
                string monthName = dtf.GetMonthName(i);

                dt.Rows.Add(new object[] { monthName, string.Format("{0:00}", i) });
            }

            Common.setComboList(cboMonths, dt, "MonthsVal", "Months");

            cboMonths.Text = dtf.GetMonthName(DateTime.Today.Month);

            cboMonths.KeyPress += cboMonths_KeyPress;

            bttnUpdate.Click += bttnUpdate_Click;

            cboMonths.Focus(); setReload();

            SplashScreenManager.CloseForm(false);
        }

        void bttnUpdate_Click(object sender, EventArgs e)
        {
            string strvalue = string.Format("{0},{1}", cboMonths.Text, spinEdit1.Value);

            string strCode = string.Format("{0}/{1}", cboMonths.SelectedValue, spinEdit1.Value);

            //check if batch already exis
            DataTable dts = (new Logic()).getSqlStatement((String.Format("select * from [Reconciliation].[tblBatch] where BatchCode  ='{0}'", strCode))).Tables[0];

            if (dts.Rows.Count > 0)
            {
                Common.setMessageBox("Batch Code Already Exit ", Program.ApplicationName, 2);
                DialogResult = System.Windows.Forms.DialogResult.Cancel;
                ReturnValue1 = null;
                ReturnValue2 = null;
                Close();
                return;
            }
            else
            {

                using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
                {
                    connect.Open();
                    _command = new SqlCommand("NewReconciliationBatch", connect) { CommandType = CommandType.StoredProcedure };
                    _command.Parameters.Add(new SqlParameter("@strcode", SqlDbType.Char)).Value = strCode;
                    _command.Parameters.Add(new SqlParameter("@strValue", SqlDbType.Char)).Value = strvalue;
                    using (System.Data.DataSet ds = new System.Data.DataSet())
                    {
                        ds.Clear();
                        adp = new SqlDataAdapter(_command);
                        adp.Fill(ds);
                        connect.Close();

                        if (ds.Tables[0].Rows[0]["returnCode"].ToString() == "00")
                        {

                            ReturnValue1 = ds.Tables[1].Rows[0]["batchValue"].ToString();
                            ReturnValue2 = ds.Tables[1].Rows[0]["batchCode"].ToString();
                            DialogResult = System.Windows.Forms.DialogResult.OK;
                            Close();
                            return;

                        }
                        else
                        {
                            Common.setMessageBox(ds.Tables[0].Rows[0]["returnMessage"].ToString(), Program.ApplicationName, 2);
                            DialogResult = System.Windows.Forms.DialogResult.Cancel;
                            return;
                        }

                    }
                }
            }
        }

        void cboMonths_KeyPress(object sender, KeyPressEventArgs e)
        {
            Methods.AutoComplete(cboMonths, e, true);
        }

        private void setReload()
        {

            DataTable dt;

            using (var ds = new System.Data.DataSet())
            {

                using (SqlDataAdapter ada = new SqlDataAdapter("SELECT * from [Reconciliation].[tblBatch] ", Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }

                dt = ds.Tables[0];

                gridControl1.DataSource = dt.DefaultView;
            }

            gridView1.OptionsBehavior.Editable = false;

            gridView1.Columns["BatchCode"].Visible = false;

            gridView1.BestFitColumns();
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
                ReturnValue1 = null;
                ReturnValue2 = null;
                Close();

            }
            else if (sender == tsbNew)
            {
            }
            else if (sender == tsbEdit)
            {

            }
            else if (sender == tsbDelete)
            {

            }
            else if (sender == tsbReload)
            {

            }
            //bttnReset.PerformClick();
        }


    }
}
