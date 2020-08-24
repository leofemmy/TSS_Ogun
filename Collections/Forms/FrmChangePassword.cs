using Collection.Classes;
using Collection.Classess;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TaxSmartSuite.Class;

namespace Collection.Forms
{
    public partial class FrmChangePassword : Form
    {
        PasswordStrength pwdStrength = new PasswordStrength();

        public static string passwords { get; set; }

        public FrmChangePassword()
        {
            InitializeComponent();

            Load += OnFormLoad;

            txtNewpass.Leave += TxtNewpass_Leave;

            txtConfirm.Leave += TxtConfirm_Leave;

            bttnCancel.Click += OnBttnClicked;

            bttnUpdate.Click += OnBttnClicked;

            OnFormLoad(null, null);
        }

        private void TxtConfirm_Leave(object sender, EventArgs e)
        {
            pwdStrength.SetPassword(txtConfirm.Text);

            DataTable dt = pwdStrength.GetStrengthDetails();

            var pscore = pwdStrength.GetPasswordScore();

            var passStrength = pwdStrength.GetPasswordStrength();

            label6.Text = strengthChecker(pwdStrength.GetPasswordStrength());

            switch (label6.Text)
            {
                case "Very Weak":
                    label6.BackColor = Color.Red;
                    break;
                case "Weak":
                    label6.BackColor = Color.Red;
                    break;
                case "Good":
                    label6.BackColor = Color.Green;
                    break;
                case "Strong":
                    label6.BackColor = Color.Green;
                    break;
                case "Very Strong":
                    label6.BackColor = Color.Green;
                    break;
                default:
                    break;
            }

        }

        private void TxtNewpass_Leave(object sender, EventArgs e)
        {
            pwdStrength.SetPassword(txtNewpass.Text);

            DataTable dt = pwdStrength.GetStrengthDetails();

            var pscore = pwdStrength.GetPasswordScore();

            var passStrength = pwdStrength.GetPasswordStrength();

            label5.Text = strengthChecker(pwdStrength.GetPasswordStrength());

            switch (label5.Text)
            {
                case "Very Weak":
                    label5.BackColor = Color.Red;
                    break;
                case "Weak":
                    label5.BackColor = Color.Red;
                    break;
                case "Good":
                    label5.BackColor = Color.Green;
                    break;
                case "Strong":
                    label6.BackColor = Color.Green;
                    break;
                case "Very Strong":
                    label5.BackColor = Color.Green;
                    break;
                default:
                    break;
            }
        }

        void OnFormLoad(object sender, EventArgs e)
        {
            setImages();

            //var getpass = getUserPassword();

            txtUserID.Text = Program.UserID;

            txtoldPass.Text = MosesClassLibrary.Security.Encryption.Decrypt(getUserPassword());


        }

        private void setImages()
        {
            //tsbNew.Image = MDIMain.publicMDIParent.i16x16.Images[5];
            //tsbEdit.Image = MDIMain.publicMDIParent.i16x16.Images[6];
            //tsbDelete.Image = MDIMain.publicMDIParent.i16x16.Images[7];
            //tsbReload.Image = MDIMain.publicMDIParent.i16x16.Images[0];
            //tsbClose.Image = MDIMain.publicMDIParent.i16x16.Images[11];

            ////bttnCancel.Image = MDIMain.publicMDIParent.i32x32.Images[9];
            //btnMain.Image = MDIMain.publicMDIParent.i32x32.Images[8];
            //btnPrint.Image = MDIMain.publicMDIParent.i32x32.Images[29];
            //btnSearch.Image = MDIMain.publicMDIParent.i32x32.Images[2];
            //btnClear.Image = MDIMain.publicMDIParent.i32x32.Images[3];

        }

        void onLeave(object sender, EventArgs e)
        {
            if (sender == txtNewpass.Text)
            {
                pwdStrength.SetPassword(txtNewpass.Text);

                DataTable dt = pwdStrength.GetStrengthDetails();

                var pscore = pwdStrength.GetPasswordScore();

                var passStrength = pwdStrength.GetPasswordStrength();

                strengthChecker(pwdStrength.GetPasswordStrength());
            }
        }

        string strengthChecker(string pssChecker)
        {
            switch (pssChecker)
            {
                case "Very Weak":
                    break;
                case "Weak":
                    break;
                case "Good":
                    break;
                case "Strong":
                    break;
                case "Very Strong":
                    break;
                default:
                    break;
            }
            return pssChecker;
        }
        void OnBttnClicked(object sender, EventArgs e)
        {
            if (sender == bttnUpdate)
            {
                if (label5.Text == "Very Weak" || label6.Text == "Very Weak" || label5.Text == "Weak" || label6.Text == "Weak")
                {
                    Common.setMessageBox("Password Weak !Try again", Program.ApplicationName, 1);
                    return;
                }

                if (string.Equals(txtoldPass.Text, txtNewpass.Text))
                {
                    Common.setMessageBox("Old Password and new Password cannot be the same.", Program.ApplicationName, 2);
                    return;
                }
                else if (!String.Equals(txtNewpass.Text, txtConfirm.Text))
                {
                    Common.setMessageBox("New password not euqall", Program.ApplicationName, 2);
                    return;
                }
                else
                {
                    try
                    {
                        string strquery = string.Format("update Login.tblUsers set Password='{0}' WHERE UserID = '{1}' ", MosesClassLibrary.Security.Encryption.Encrypt(txtNewpass.Text.Trim()), Program.UserID);

                        //insert modife record
                        using (SqlConnection db = new SqlConnection(Logic.ConnectionString))
                        {
                            SqlTransaction transaction;

                            db.Open();

                            transaction = db.BeginTransaction();

                            using (SqlCommand sqlCommand2 = new SqlCommand(strquery, db, transaction))
                            {
                                sqlCommand2.ExecuteNonQuery();
                            }
                            transaction.Commit();

                            db.Close();
                        }
                        Common.setMessageBox("Password Change successfully.", Program.ApplicationName, 1);
                        DialogResult = DialogResult.OK;
                        Close();

                    }
                    catch (Exception ex)
                    {
                        Common.setMessageBox(string.Format("{0}----{1}..Error Occur While Changing Password ", ex.Message, ex.StackTrace), Program.ApplicationName, 3);
                        return;
                    }
                }
            }
            else if (sender == bttnCancel)
            {
                DialogResult = DialogResult.Cancel;
                Close();
            }
        }

        public static string getUserPassword()
        {
            string grt = string.Format("SELECT * FROM Login.tblUsers WHERE UserID = '{0}'", Program.UserID);

            DataTable Dt = (new Logic()).getSqlStatement(grt).Tables[0];

            if (Dt != null && Dt.Rows.Count > 0)
            {
                passwords = String.Format("{0}", Dt.Rows[0]["Password"]);
            }

            return passwords;
        }
    }
}
