using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TaxDrive.Class;

namespace TaxDrive
{
    public partial class FrmTaxStation : Form
    {
         OleDbConnection db_con;
        public FrmTaxStation()
        {
            InitializeComponent();

            btnUpdate.Click += btnUpdate_Click;
        }

        void btnUpdate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtDrivename.EditValue.ToString()))
            {
                MessageBox.Show("Enter Station Name"); return;
            }
            else
            {
                try
                {
                    string conn = Logic.ConfigureSettings();

                    db_con = new OleDbConnection(conn);

                    db_con.Open();

                    String my_querry = String.Format("INSERT INTO tblStation(StationName)VALUES('{0}')", txtDrivename.EditValue.ToString());

                    OleDbCommand cmd = new OleDbCommand(my_querry, db_con);
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Data saved successfuly...!");

                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed due to" + ex.Message); return;
                }
                finally
                {
                    db_con.Close();
                }


            }
        }
    }
}
