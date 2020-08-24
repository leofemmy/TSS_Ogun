using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TaxSmartSuite.Class;
using System.Data.SqlClient;
using Control_Panel.Class;

namespace Control_Panel.Forms
{
    public partial class FrmSubBusiness : Form
    {
        Methods extMethods = new Methods();

        DBConnection connect = new DBConnection();

        public FrmSubBusiness()
        {
            InitializeComponent();
            //call the constring here
            //connect.ConnectionString();
        }

        private void CloseStripButton1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void LoadBusiness()
        {
            //connect.connect.Close();

            using (var ds = new System.Data.DataSet())
            {
                connect.connect.Open();
                string query = "select * from tblBusinessClass ";
                using (SqlDataAdapter ada = new SqlDataAdapter(query, Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }
                //fill the combo box
                cboBusiness.DataSource = ds.Tables[0];
            }


            cboBusiness.DisplayMember = "BusinessName";
            cboBusiness.ValueMember = "BusinessID";

            connect.connect.Close();
        }

        private void LoadGridView()
        {
            //DataTable dt = extMethods.LoadData("select * from ViewBusinessSubClass ");

            DataTable dt = (new Logic()).getSqlStatement("select * from ViewBusinessSubClass").Tables[0];

            gridControl1.DataSource = dt.DefaultView;
            gridView1.OptionsBehavior.Editable = false;
            gridView1.BestFitColumns();
        }
        private void clearfield()
        {
            cboBusiness.Text = string.Empty;
            txtSubName.Text = string.Empty;
        }
        private void FrmSubBusiness_Load(object sender, EventArgs e)
        {
            clearfield();
            LoadBusiness();
            LoadGridView();
           
        }

        private void SaveToolStripButton_Click(object sender, EventArgs e)
        {
            if (txtSubName.Text == "")
            {
                MessageBox.Show("Invalid Entry Can't Be Empty", "Error");
                txtSubName.Focus();
                return;
            }
            else if (cboBusiness.Text == "")
            {
                MessageBox.Show("Invalid Entry Can't Be Empty", "Error");
                cboBusiness.Focus();
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
                        using (SqlCommand sqlCommand = new SqlCommand(String.Format("INSERT INTO [tblBusinessSubClass]([BusinessSubName],[BusinessID]) VALUES ('{0}', '{1}');", txtSubName.Text.Trim().ToUpperInvariant(), cboBusiness.SelectedValue.ToString()), db, transaction))
                        {
                            sqlCommand.ExecuteNonQuery();
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
            clearfield();
            LoadGridView();
        }

        private void cboBusiness_KeyPress(object sender, KeyPressEventArgs e)
        {
            Methods.AutoComplete(cboBusiness, e, true);

        }

    }
}
