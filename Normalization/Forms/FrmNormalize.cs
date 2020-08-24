using System;
using System.Data;
using System.Windows.Forms;
using TaxSmartSuite.Class;
using DevExpress.XtraGrid.Selection;
using System.Data.SqlClient;


namespace Normalization.Forms
{
    public partial class FrmNormalize : Form
    {
        TaxSmartSuite.Class.DBConnection connection = new DBConnection();

        TaxSmartSuite.Class.Methods extMethods = new Methods();

        System.Data.DataSet ds = new System.Data.DataSet();

        GridCheckMarksSelection selection;

        private string query, retval;

         SqlDataAdapter adp;

        private bool isFirst = true;


        public FrmNormalize()
        {
            InitializeComponent();
            connection.ConnectionString();
        }

        internal GridCheckMarksSelection Selection
        {
            get { return selection; }
        }
        
        private void btnSearch_Click(object sender, EventArgs e)
        {
            ds = new System.Data.DataSet();

            adp = new SqlDataAdapter();

            query = String.Format("select [Payment Ref. Number] ,[Payer name], Amount ,[Deposit Slip Number]from tblCollectionReport where [Payer Name] like '%{0}%'", txtfieldname.Text.Trim());

            //MessageBox.Show(query);

            adp = new SqlDataAdapter(query, connection.connect);

            adp.Fill(ds);
            gcNormalize.DataSource = null;
            gcNormalize.DataSource = ds.Tables[0];

            if (isFirst)
            {
                selection = new GridCheckMarksSelection(gvNormalize );
                selection.CheckMarkColumn.VisibleIndex = 0;
                isFirst = false;
            }
        }

        private void cboTaxAgent_KeyPress(object sender, KeyPressEventArgs e)
        {
            Methods.AutoComplete(cboTaxAgent, e, true);
        }

        private void btnNormalize_Click(object sender, EventArgs e)
        {
            string agentid = string.Empty;

            agentid = this.cboTaxAgent.SelectedValue.ToString();

            for (int i = 0; i < selection.SelectedCount; i++)
            {
                string lol = ((selection.GetSelectedRow(i) as DataRowView)["Payment Ref. Number"].ToString());
                //MessageBox.Show(lol);
                query = String.Format("UPDATE tblCollectionReport SET [Payer ID]='{0}' Where [Payment Ref. Number]='{1}'", agentid, lol);

                retval = extMethods.getQuery("INSERT", query);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
