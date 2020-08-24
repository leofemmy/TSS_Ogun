using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using TaxSmartSuite.Class;
using EasyReg.Class;

namespace EasyReg.Forms
{
    public partial class FrmLogIn : Form
    {
        private SqlDataAdapter adp;

        private SqlCommand command;
        
        private DataTable Dt;

        public FrmLogIn()
        {
            InitializeComponent();

            setDBComboBox();
            txtUser.Focus();
        }

        private void panelContainer_Paint(object sender, PaintEventArgs e)
        {

        }

        private void BttnCancel_Click(object sender, EventArgs e)
        {
            Program.APP_CONNECTED = false;
            Close();
        }

        void setDBComboBox()
        {
            //DataTable Dt = EasyServices.doSelectLoginTypes();
            //Common.setComboList(cboLogin, Dt, "LoginTypeID", "LoginTypeName");
            //cboLogin.SelectedIndex = -1;

            DataTable Dt;
            using (var ds = new System.Data.DataSet())
            {
                string query = "SELECT * FROM dbo.EasyReg_tblLoginType WHERE LoginTypeID > 1 ORDER BY LoginTypeName asc";
                using (SqlDataAdapter ada = new SqlDataAdapter(query, Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }

                Dt = ds.Tables[0];
            }

            Common.setComboList(cboLogin, Dt, "LoginTypeID", "LoginTypeName");

            cboLogin.SelectedIndex = -1;

        }

        bool ProcessLoginDetails()
        {

            bool bResponse = false;

            if (String.IsNullOrEmpty(txtUser.Text) || String.Equals(txtUser.Text, "[Set User Name]"))
            {
                Common.setEmptyField("Username", Program.ApplicationName);
                txtUser.Focus();
                return false;
            }
            else if (String.IsNullOrEmpty(txtPassword.Text) || String.Equals(txtPassword.Text, "[Set Password]"))
            {
                Common.setEmptyField("Password", Program.ApplicationName);
                txtPassword.Focus();
                return false;
            }
            else
            {
                //
                SqlConnection connect = new SqlConnection(Logic.ConnectionString);

                connect.Open();

                command = new SqlCommand("EasyReg_doLogin",connect) { CommandType = CommandType.StoredProcedure };
                command.Parameters.Add(new SqlParameter("@UserID", SqlDbType.VarChar)).Value = txtUser.Text.Trim();
                command.Parameters.Add(new SqlParameter("@Password", SqlDbType.VarChar)).Value = txtPassword.Text.Trim();
                command.Parameters.Add(new SqlParameter("@LoginTypeID", SqlDbType.Int)).Value = Convert.ToInt32(cboLogin.SelectedValue);

                using (DataSet ds = new DataSet())
                {
                    adp = new SqlDataAdapter(command);
                    adp.Fill(ds);
                    Dt = ds.Tables[0];

                    connect.Close();
                }


                //DataTable Dt = EasyServices.doLogin(txtUser.Text.Trim(), txtPassword.Text.Trim(), Convert.ToInt32(cboLogin.SelectedValue));

                string retCode = Dt.Rows[0][0].ToString();

                bResponse = false;

                switch (retCode)
                {
                    case "00":// successful login
                        switch (cboLogin.SelectedValue.ToString())
                        {
                            case "3":
                                break;
                            case "4":
                                break;
                            case "2":
                                break;
                        }

                        Program.AppicationUserCode = cboLogin.SelectedValue.ToString();
                        Program.AgentListId = Dt.Rows[0][3].ToString();
                        Program.UserName = Dt.Rows[0][2].ToString();
                        Program.UserType = cboLogin.Text.Trim();
                        Program.UserId = txtUser.Text.Trim();

                        bResponse = true;
                        break;
                    case "-1":
                        if (lblAttempt.Text == "1")
                        {
                            Common.setMessageBox("You already used all the attempts.\nThis will terminate the application.", Program.ApplicationName, 3);
                            this.Close();
                        }
                        else
                        {
                            int iAttempt;
                            iAttempt = Convert.ToInt32(lblAttempt.Text) - 1;
                            lblAttempt.Text = iAttempt.ToString();
                            Common.setMessageBox(String.Format("Invalid Username/Password. Please try again.\n\nWarning: You only have {0} attempt.", lblAttempt.Text), Program.ApplicationName, 3);
                        }
                        bResponse = false;
                        break;
                    default:
                        break;

                }

            }
            return bResponse;
        }

        private void BttnOK_Click(object sender, EventArgs e)
        {
            if (ProcessLoginDetails())
            {
                Program.APP_CONNECTED = true;
                Close();
            }
        }

    }
}
