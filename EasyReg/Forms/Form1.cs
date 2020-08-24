using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TaxSmartSuite.Class;
using EasyReg.Forms;

namespace EasyReg
{

    public partial class Form1 : Form
    {
        EasyRegService.EasyRegService EasyServices = new EasyReg.EasyRegService.EasyRegService();

        #region style textbox
        private Font _Empty_Font = new Font("Tahoma", 14, FontStyle.Italic);
        private Font _Fill_Font_User_ID = new Font("Consolas", 14, FontStyle.Regular);
        private Font _Fill_Font_Password = new Font("Wingdings 2", 15, FontStyle.Bold);

        private void SetEmptyTextValues_ToUserID()
        {
            txtUser.BackColor = Color.White;
            txtUser.ForeColor = Color.FromArgb(224, 224, 224);
            txtUser.Font = _Empty_Font;
        }

        private void SetFillTextValues_ToUserID()
        {
            //TextBoxUserName.BackColor = Color.Bisque
            txtUser.ForeColor = Color.Red;
            txtUser.Font = _Fill_Font_User_ID;
        }

        private void SetEmptyTextValues_Password()
        {
            txtPassword.BackColor = Color.White;
            txtPassword.ForeColor = Color.FromArgb(224, 224, 224);
            txtPassword.Font = _Empty_Font;
            char ch;
            bool lol = Char.TryParse(string.Empty, out ch); //Convert.ToChar(" ");
            txtPassword.PasswordChar = ch;
        }

        private void SetFillTextValues_Password()
        {
            txtPassword.BackColor = Color.White;
            txtPassword.ForeColor = Color.Red;
            txtPassword.Font = _Fill_Font_Password;
            txtPassword.PasswordChar = '*';
        }

        void txtUser_LostFocus(object sender, EventArgs e)
        {
            if ((!string.IsNullOrEmpty(this.txtUser.Text.Trim())) & !(txtUser.Text.ToString().Trim() == "[Set User Name]".ToString().Trim()))
            {
                this.txtUser.Text = this.txtUser.Text.ToUpper();
            }
        }

        void txtUser_Enter(object sender, EventArgs e)
        {
            if (txtUser.Text.ToString().Trim().Length == 0 | txtUser.Text.ToString().Trim() == "[Set User Name]".ToString().Trim())
            {
                txtUser.Text = "";
            }
            else
            {
                txtUser.SelectAll();
            }
        }

        void txtUser_Leave(object sender, EventArgs e)
        {
            if (txtUser.Text.ToString().Trim().Length == 0 | txtUser.Text.ToString().Trim() == "[Set User Name]".ToString().Trim())
            {
                SetEmptyTextValues_ToUserID();
                txtUser.Text = "[Set User Name]";
            }
            else
            {
                SetFillTextValues_ToUserID();
            }
        }

        void txtUser_TextChanged(object sender, EventArgs e)
        {
            if (txtUser.Text.ToString().Trim() == "[Set User Name]".ToString().Trim() | txtUser.Text.ToString().Trim().Length == 0)
            {
                SetEmptyTextValues_ToUserID();
            }
            else
            {
                SetFillTextValues_ToUserID();
            }
        }

        void txtPassword_Enter(object sender, EventArgs e)
        {
            if (txtPassword.Text.ToString().Trim().Length == 0 | txtPassword.Text.ToString().Trim() == "[Set Password]".ToString().Trim())
            {
                txtPassword.Text = "";
            }
            else
            {
                txtPassword.SelectAll();
            }
        }

        void txtPassword_Leave(object sender, EventArgs e)
        {
            if (txtPassword.Text.ToString().Trim().Length == 0 | txtPassword.Text.ToString().Trim() == "[Set Password]".ToString().Trim())
            {
                SetEmptyTextValues_Password();
                txtPassword.Text = "[Set Password]";
            }
            else
            {
                SetFillTextValues_Password();
            }
        }

        void txtPassword_TextChanged(object sender, EventArgs e)
        {
            if (txtPassword.Text.ToString().Trim() == "[Set Password]".ToString().Trim() | txtPassword.Text.ToString().Trim().Length == 0)
            {
                SetEmptyTextValues_Password();
            }
            else
            {
                SetFillTextValues_Password();
            }
        }

        #endregion
        public Form1()
        {
            InitializeComponent();

            Activated += FrmLogin_Activated;
            Load += FrmLogin_Load;

            txtUser.LostFocus += txtUser_LostFocus;
            txtUser.TextChanged += txtUser_TextChanged;
            txtUser.Leave += txtUser_Leave;
            txtUser.Enter += txtUser_Enter;

            BttnOK.Click += OnButtonClicked;
            BttnCancel.Click += OnButtonClicked;

            setDBComboBoxMake();
        }

        public void setDBComboBoxMake()
        {
            DataTable Dt = EasyServices.doSelectLoginTypes();
            Common.setComboList(cboLogin, Dt, "LoginTypeID", "LoginTypeName");
            cboLogin.SelectedIndex = -1;
        }

        void FrmLogin_Load(object sender, EventArgs e)
        {
            txtUser_Leave(null, null);
            txtPassword_Leave(null, null);

        }

        void FrmLogin_Activated(object sender, EventArgs e)
        {
            txtUser.Focus();
            //BttnOK.Focus();
        }

        void OnButtonClicked(object sender, EventArgs e)
        {
            if (sender == BttnCancel)
            {
                Program.APP_CONNECTED = false;

                Close();
            }
            else if (sender == BttnOK)
            {
                if (ProcessLoginDetails())
                {
                    Program.APP_CONNECTED = true;
                    Close();
                }
            }
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
                DataTable Dt = EasyServices.doLogin(txtUser.Text.Trim(),txtPassword.Text.Trim(),Convert.ToInt32(cboLogin.SelectedValue));

                string retCode = Dt.Rows[0][0].ToString();  

                bResponse = false;

                switch (retCode)
                {
                    case "00":// successful login
                        switch(cboLogin.SelectedValue.ToString())
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
                        Program.UserType=cboLogin.Text.Trim();
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
                            Common.setMessageBox(String.Format("Invalid Username/Password. Please try again.\n\nWarning: You only have {0} attempt.", lblAttempt.Text), Program.ApplicationName,3);
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

        }
           
        }

   
}