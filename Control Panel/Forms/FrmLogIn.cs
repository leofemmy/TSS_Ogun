using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TaxSmartSuite;
using MosesClassLibrary.Security;
using TaxSmartSuite.Class;
using Control_Panel.Class;

namespace Control_Panel.Forms
{
    public partial class FrmLogin : Form
    {
        #region style textbox
        private Font _Empty_Font = new Font("Tahoma", 14, FontStyle.Italic);
        private Font _Fill_Font_User_ID = new Font("Consolas", 14, FontStyle.Regular);
        private Font _Fill_Font_Password = new Font("Wingdings 2", 15, FontStyle.Bold);

        private void SetEmptyTextValues_ToUserID()
        {
            txtUsername.BackColor = Color.White;
            txtUsername.ForeColor = Color.FromArgb(224, 224, 224);
            txtUsername.Font = _Empty_Font;

        }

        private void SetFillTextValues_ToUserID()
        {
            //TextBoxUserName.BackColor = Color.Bisque
            txtUsername.ForeColor = Color.Red;
            txtUsername.Font = _Fill_Font_User_ID;

        }

        private void SetEmptyTextValues_Password()
        {
            txtPassword.BackColor = Color.White;
            txtPassword.ForeColor = Color.FromArgb(224, 224, 224);
            txtPassword.Font = _Empty_Font;
            char ch;
            bool lol  = Char.TryParse(string.Empty, out ch); //Convert.ToChar(" ");
            txtPassword.PasswordChar = ch;

        }

        private void SetFillTextValues_Password()
        {
            txtPassword.BackColor = Color.White;
            txtPassword.ForeColor = Color.Red;
            txtPassword.Font = _Fill_Font_Password;
            txtPassword.PasswordChar = '®';

        }

        void txtUsername_LostFocus(object sender, EventArgs e)
        {
            if ((!string.IsNullOrEmpty(this.txtUsername.Text.Trim())) & !(txtUsername.Text.ToString().Trim() == "[Set User Name]".ToString().Trim()))
            {
                this.txtUsername.Text = this.txtUsername.Text.ToUpper();
            }
        }

        void txtUsername_Enter(object sender, EventArgs e)
        {
            if (txtUsername.Text.ToString().Trim().Length == 0 | txtUsername.Text.ToString().Trim() == "[Set User Name]".ToString().Trim())
            {
                txtUsername.Text = "";
            }
            else
            {
                txtUsername.SelectAll();
            }
        }

        void txtUsername_Leave(object sender, EventArgs e)
        {
            if (txtUsername.Text.ToString().Trim().Length == 0 | txtUsername.Text.ToString().Trim() == "[Set User Name]".ToString().Trim())
            {
                SetEmptyTextValues_ToUserID();
                txtUsername.Text = "[Set User Name]";
            }
            else
            {
                SetFillTextValues_ToUserID();
            }
        }

        void txtUsername_TextChanged(object sender, EventArgs e)
        {
            if (txtUsername.Text.ToString().Trim() == "[Set User Name]".ToString().Trim() | txtUsername.Text.ToString().Trim().Length == 0)
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

        public FrmLogin()
        {
            InitializeComponent();

            Activated += FrmLogin_Activated;
            Load += FrmLogin_Load;

            L_Move.MouseDown += L_Move_MouseDown;

            BttnOK.Click += OnButtonClicked;
            BttnCancel.Click += OnButtonClicked;
            
            txtUsername.LostFocus += txtUsername_LostFocus;
            txtUsername.TextChanged += txtUsername_TextChanged;
            txtUsername.Leave += txtUsername_Leave;
            txtUsername.Enter += txtUsername_Enter;

            txtPassword.TextChanged += txtPassword_TextChanged;
            txtPassword.Leave += txtPassword_Leave;
            txtPassword.Enter += txtPassword_Enter;
        }

        void FrmLogin_Load(object sender, EventArgs e)
        {
            txtUsername_Leave(null, null);
            txtPassword_Leave(null, null);
            //BttnOK.PerformClick();
        }

        void FrmLogin_Activated(object sender, EventArgs e)
        {
            txtUsername.Focus();
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

        void L_Move_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                L_Move.Capture = false;
                const int L_DOWN = 0xa1;
                const int HTCAPTION = 2;
                Message msg = Message.Create(this.Handle, L_DOWN, new IntPtr(HTCAPTION), IntPtr.Zero);
                this.DefWndProc(ref msg);
            }
        }

        bool ProcessLoginDetails()
        {
            bool bResponse = false;

            if (String.IsNullOrEmpty(txtUsername.Text) || String.Equals(txtUsername.Text, "[Set User Name]"))
            {
                Common.setEmptyField("Username", Program.ApplicationName);
                txtUsername.Focus();
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
                string SQL = String.Format(@"SELECT COUNT(*) FROM [dbo].[ViewUserApplication] 
                                WHERE [UserID] = '{0}' AND [Password] = '{1}' AND [ApplicationCode] = '{2}' AND [Flag] = 1"
                    , txtUsername.Text, Encryption.Encrypt(txtPassword.Text), Program.ApplicationCode);

                if (new Logic().IsRecordExist(SQL))
                //if (true)
                {
                    Program.UserID = txtUsername.Text;
                    bResponse = true;
                }
                else
                {
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
                }
            }
            return bResponse;
        }
    }
}
