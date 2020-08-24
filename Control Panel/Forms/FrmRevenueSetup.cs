using System;
using System.Data;
using System.Windows.Forms;
using TaxSmartSuite.Class ;
using System.Data.SqlClient;
using Control_Panel.Class;

namespace Control_Panel.Forms
{
    public partial class FrmRevenueSetup : Form
    {
        //Methods extMethods = new Methods();

        //DBConnection connect = new DBConnection();

        public FrmRevenueSetup()
        {
            InitializeComponent();
            //call the constring here
            //connect.ConnectionString();
        }

        private void FrmRevenueSetup_Load(object sender, EventArgs e)
        {
            LoadGridView();
            LoadAgency();
        }

        private void LoadGridView()
        {
            string query = String.Format("select * from ViewAgencyRevenueType");

            DataTable dt = (new Logic()).getSqlStatement(query).Tables[0];
            
            //DataTable dt = extMethods.LoadData("select * from ViewAgencyRevenueType");
            gridControl1.DataSource = dt.DefaultView;
            gridView1.Columns["AgencyCode"].VisibleIndex = -1;
            gridView1.OptionsBehavior.Editable = false;
            gridView1.BestFitColumns();
        }

       private void CloseStripButton1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void LoadAgency()
        {
            

            using (var ds = new System.Data.DataSet())
            {
                
                string query = "select AgencyCode , AgencyName from tblAgency ";
                using (SqlDataAdapter ada = new SqlDataAdapter(query, Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }
                //fill the combo box
                cboAgency.DataSource = ds.Tables[0];
            }


            cboAgency.DisplayMember = "AgencyName";

            cboAgency.ValueMember = "AgencyCode";

           
        }

        private void cboAgency_KeyPress(object sender, KeyPressEventArgs e)
        {
            Methods.AutoComplete(cboAgency, e, true);
        }

        private void SaveToolStripButton_Click(object sender, EventArgs e)
        {
            if (txtRevCode.Text == "")
            {
                MessageBox.Show("Invalid Entry", "Error");
                return;
            }
            else if (txtRevName.Text == "")
            {
                MessageBox.Show("Invalid Entry", "Error");
                return;
            }
            else
            {
                using (SqlConnection db = new SqlConnection(Logic.ConnectionString))
                {
                    SqlTransaction transaction;

                    db.Open();

                    transaction = db.BeginTransaction();
                    try
                    {
                        //insert into town table
                      using (SqlCommand sqlCommand = new SqlCommand(String.Format("INSERT INTO [tblRevenueType]([RevenueCode],[RevenueName]) VALUES ('{0}', '{1}');", txtRevCode.Text.Trim().ToUpperInvariant(), txtRevName.Text.Trim().ToUpperInvariant()), db, transaction))
                        {
                            sqlCommand.ExecuteNonQuery();
                        }

                      using (SqlCommand sqlCommand1 = new SqlCommand(String.Format("INSERT INTO [tblAgencyRevenueRelation]([RevenueCode],[AgencyCode]) VALUES ('{0}', '{1}');", txtRevCode.Text.Trim().ToUpperInvariant(), cboAgency.SelectedValue.ToString()), db, transaction))
                      {
                          sqlCommand1.ExecuteNonQuery();
                      }


                        transaction.Commit();
                    }
                    catch (SqlException sqlError)
                    {
                        transaction.Rollback();
                    }
                    db.Close();
                }
            }
            LoadGridView();
            clearField();
        }

        private void clearField()
        {
            txtRevName.Text = string.Empty;
            txtRevCode.Text = string.Empty;
            cboAgency.Text = string.Empty;
        }

    }
}
