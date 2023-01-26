using Collection.Classess;
using System;
using System.Data.SqlClient;
using System.Windows.Forms;
using TaxSmartSuite.Class;

namespace Collection.Forms
{
    public partial class FrmStationname : Form
    {
        bool isFirst = true;

        protected int ID;

        protected bool boolIsUpdate;

        public FrmStationname()
        {
            InitializeComponent();

            bttnUpdate.Click += bttnUpdate_Click;
        }

        void bttnUpdate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtStation.Text))
            {
                Common.setEmptyField("Station Code", Program.ApplicationName);
                txtStation.Focus(); return;
            }
            else if (string.IsNullOrEmpty(txtstationName.Text))
            {
                Common.setEmptyField("Station Name", Program.ApplicationName); txtstationName.Focus();
                return;
            }
            else
            {
                //check form status either new or edit
                if (!boolIsUpdate)
                {
                    using (SqlConnection db = new SqlConnection(Logic.ConnectionString))
                    {
                        SqlTransaction transaction;

                        db.Open();

                        transaction = db.BeginTransaction();
                        try
                        {

                            using (SqlCommand sqlCommand1 = new SqlCommand(String.Format("INSERT INTO Receipt.tblStation(StationCode,[StationName],StateCode)VALUES ('{0}','{1}','{2}');", txtStation.Text.Trim(), txtstationName.Text.Trim(), Program.stateCode), db, transaction))
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
                    //setReload();
                    Common.setMessageBox("Record has been Successfully Added", Program.ApplicationName, 1);

                    if (Program.isCentralData)
                    {
                        if (MessageBox.Show("Do you want to add another record?", Program.ApplicationName, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
                        {
                            //bttnCancel.PerformClick();
                            //setReload(); 
                            Clear();
                            //tsbReload.PerformClick();
                        }
                        else
                        {
                            //bttnReset.PerformClick();

                        }
                    }
                    else
                    {
                        Clear();
                        //tsbReload.PerformClick();
                    }

                    //}
                }
                else
                {
                    //update the records

                    using (SqlConnection db = new SqlConnection(Logic.ConnectionString))
                    {
                        SqlTransaction transaction;

                        db.Open();

                        transaction = db.BeginTransaction();
                        try
                        {
                            //MessageBox.Show(MDIMain.stateCode);
                            //fieldid
                            string query = String.Format("UPDATE Receipt.tblStation SET [StationCode]='{0}',StationName='{1}',StateCode='{2}' where  StationID ='{1}'", txtStation.Text.Trim(), txtstationName.Text.Trim(), Program.stateCode, ID);

                            using (SqlCommand sqlCommand1 = new SqlCommand(query, db, transaction))
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

                    //setReload();
                    Common.setMessageBox("Changes in Record has been Successfully Saved.", Program.ApplicationName, 1);
                    //setReload();
                    //tsbReload.PerformClick();

                }
            }
            Application.Restart();
        }

        void Clear()
        {
            txtStation.Clear(); txtstationName.Clear();
        }

    }
}
