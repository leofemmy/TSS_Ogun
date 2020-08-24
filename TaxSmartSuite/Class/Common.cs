using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraTab;
using System.Data;
using System.IO;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Configuration;

namespace TaxSmartSuite.Class
{
    public class Common
    {
        public static XtraTabControl TabCtrl;
        /// <summary>
        /// Display Message Dialog box 
        /// </summary>
        /// <param name="sMessage">Message to be display</param>
        /// <param name="Title">title of the dialog box</param>
        /// <param name="iWhich">1 = Information, 2 = Exclamation, 3 = Error</param>
        public static void setMessageBox(string sMessage, string Title, int iWhich)
        {
            switch (iWhich)
            {
                case 1:
                    MessageBox.Show(sMessage, Title, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
                case 2:
                    MessageBox.Show(sMessage, Title, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    break;
                case 3:
                    MessageBox.Show(sMessage, Title, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                default:
                    break;
            }
        }

        public static void setComboList(ComboBox sComboBox, DataTable Dt, string sFieldValue, string sFieldName)
        {
            try
            {
                sComboBox.DataSource = null;

                sComboBox.BeginUpdate();

                //bool lol = Dt.HasErrors;
                sComboBox.DisplayMember = sFieldName;

                sComboBox.ValueMember = sFieldValue;
                if (Dt != null && Dt.Rows.Count > 0)
                {
                    sComboBox.DataSource = Dt.DefaultView;

                    sComboBox.DisplayMember = sFieldName;

                    sComboBox.ValueMember = sFieldValue;
                }
                sComboBox.EndUpdate();
            }
            catch { sComboBox.DataSource = null; }
        }

        public static void setCheckEdit(DevExpress.XtraEditors.CheckedComboBoxEdit sCheckEdit, DataTable Dt, string sFieldValue, string sFieldName)
        {
            try
            {
                sCheckEdit.Properties.DataSource = null;

                sCheckEdit.Properties.Items.Clear();

                if (Dt != null && Dt.Rows.Count > 0)
                {
                    sCheckEdit.Properties.DataSource = Dt.DefaultView;

                    sCheckEdit.Properties.DisplayMember = sFieldName;

                    sCheckEdit.Properties.ValueMember = sFieldValue;
                }
            }
            catch { sCheckEdit.Properties.DataSource = null; }
        }

        public static void setLookUpEdit(DevExpress.XtraEditors.LookUpEdit sLookupEdit, DataTable Dt, string sFieldValue, string sFieldName)
        {
            try
            {
                sLookupEdit.Properties.DataSource = null;


                //sCheckEdit.Properties.Items.Clear();

                if (Dt != null && Dt.Rows.Count > 0)
                {
                    sLookupEdit.Properties.DataSource = Dt.DefaultView;

                    sLookupEdit.Properties.DisplayMember = sFieldName;

                    sLookupEdit.Properties.ValueMember = sFieldValue;
                }
            }
            catch { sLookupEdit.Properties.DataSource = null; }
        }

        public static void setEmptyField(string sField, string Title)
        {
            MessageBox.Show(sField + " is empty. Please check it!", Title, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        public static byte[] setPicture(PictureBox sPictureBox, string sLocation)
        {
            long sImageFileLength = 0;
            byte[] sBarrImg;
            try
            {
                sPictureBox.Image = Image.FromFile(sLocation);
                FileInfo fiImage = new FileInfo(sLocation);
                sImageFileLength = fiImage.Length;
                using (FileStream fs = new FileStream(sLocation, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    sBarrImg = new byte[Convert.ToInt32(sImageFileLength)];
                    fs.Read(sBarrImg, 0, Convert.ToInt32(sImageFileLength));
                    fs.Close();
                }
            }
            catch (Exception ex)
            {
                Tripous.Sys.ErrorBox(ex);
                sBarrImg = null;
            }
            return sBarrImg;
        }

        public static byte[] setBrowsePicture(PictureBox sPictureBox, Label sLabel, string imgPath)
        {
            byte[] sImg = null;
            using (OpenFileDialog openIMG = new OpenFileDialog())
            {
                try
                {
                    openIMG.Filter = "Known graphics format (*.bmp,*.jpg,*.gif,*.png)|*.bmp;*.jpg;*.gif;*.jpeg;*.png";
                    if (openIMG.ShowDialog() == DialogResult.OK && !String.IsNullOrEmpty(openIMG.FileName))
                    {
                        imgPath = openIMG.FileName;
                        sImg = setPicture(sPictureBox, imgPath);
                        sLabel.Visible = false;
                    }
                }
                catch (Exception ex)
                {
                    Tripous.Sys.ErrorBox(ex);
                    sImg = null;
                }
            }
            return sImg;
        }

        public static void setRemovePic(PictureBox sPictureBox)
        {
            try
            {
                sPictureBox.Image.Dispose();
                sPictureBox.Image = null;
            }
            catch (Exception ex)
            {
                Tripous.Sys.ErrorBox(ex);
            }
        }

        public static void setRemoveFile(string sFile, string sLocation)
        {
            try { if (File.Exists(sLocation + sFile) == true) { File.Delete(sLocation + sFile); } }
            catch (Exception ex)
            {
                Tripous.Sys.ErrorBox(ex);
            }
        }

        public static void setRemovePic(PictureBox sPictureBox, string sFile, string sFolder)
        {
            try
            {
                setRemovePic(sPictureBox);
                setRemoveFile(sFile, Path.GetTempPath().ToString() + sFolder);
            }
            catch (Exception ex)
            {
                Tripous.Sys.ErrorBox(ex); 
            }
        }

        public static bool IsNullOrEmpty(TextBox Txt, string Display, string Title)
        {
            bool bResponse = true;
            if (string.IsNullOrEmpty(Txt.Text))
            {
                setEmptyField(Display, Title);
                Txt.Focus();
                bResponse = true;
            }
            else
                bResponse = false;
            return bResponse;
        }

        public static bool IsNullOrEmpty(TextBox Txt, string Display, string Title, XtraTabPage TabPg)
        {
            bool bResponse = IsNullOrEmpty(Txt, Display, Title);
            if (bResponse)
                if (TabCtrl != null)
                {
                    TabCtrl.SelectedTabPage = TabPg;
                    Txt.Focus();
                }
            return bResponse;
        }

        public static bool IsNullOrEmpty(ComboBox Cb, string Display, string Title)
        {
            bool bResponse = true;
            if (Cb == null || Cb.SelectedIndex < 0)
            {
                setEmptyField(Display, Title);
                Cb.Focus();
                bResponse = true;
            }
            else
                bResponse = false;
            return bResponse;
        }

        public static bool IsNullOrEmpty(ComboBox Cb, string Display, string Title, XtraTabPage TabPg)
        {
            bool bResponse = IsNullOrEmpty(Cb, Display, Title);
            if (bResponse)
                if (TabCtrl != null)
                {
                    TabCtrl.SelectedTabPage = TabPg;
                    Cb.Focus();
                }
            return bResponse;

        }

        public static int GetComboBoxValue(ComboBox Cb)
        {
            return (string.IsNullOrEmpty(Cb.SelectedValue.ToString())) ? 0 : Convert.ToInt32(Cb.SelectedValue.ToString()) + 0;
        }

        public static void ClearFields(Control ctrl)
        {
            foreach (Control ctl in ctrl.Controls)
            {
                if (ctl is TextBox) { ctl.Text = string.Empty; }
                //else if (ctl is ComboBox) { ((ComboBox)ctl).SelectedIndex = -1; }
                else if (ctl is DateTimePicker) { ((DateTimePicker)ctl).Value = DateTime.Today; }
                //else if (ctl is DateTimePicker){((DateTimePicker)ctl).Checked  = false;}
                //else if (ctl is PictureBox) { ((PictureBox)ctl).Image = null; }
                else if (ctl is CheckBox) { ((CheckBox)ctl).Checked = false; }
                else if (ctl is ListView) { ((ListView)ctl).Items.Clear(); }
            }
        }

        public static string GenerateNumber(int length)
        {
            //Initiate objects & vars    
            Random random = new Random();
            String randomString = "";
            int randNumber;

            do
            {
                //Loop ‘length’ times to generate a random number or character
                for (int i = 0; i < length; i++)
                {
                    randNumber = random.Next(48, 58); //int {0-9}

                    //append random char or digit to random string
                    randomString = randomString + (char)randNumber;
                }
            } while (randomString.Length != length);

            //return the random string
            return randomString;
        }

        public static bool IsValidUrl(string url)
        {
            bool bResponse = false;
            System.Globalization.CompareInfo cmpUrl = System.Globalization.CultureInfo.InvariantCulture.CompareInfo;
            if (cmpUrl.IsPrefix(url, @"http://", System.Globalization.CompareOptions.IgnoreCase) == false)
            {
                url = @"http://" + url;
            }
            Regex RgxUrl = new Regex(@"^(http|https|ftp)\://[a-zA-Z0-9\-\.]+\.[a-zA-Z]{2,3}(:[a-zA-Z0-9]*)?/?([a-zA-Z0-9\-\._\?\,\'/\\\+&amp;%\$#\=~])*[^\.\,\)\(\s]$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            if (RgxUrl.IsMatch(url))
            {
                bResponse = true;
            }
            else
            {
                bResponse = false;
            }
            return bResponse;
        }

        public static void UpdateConfigurationFile(string connectionString, string ConnStringName)
        {
            if (String.IsNullOrEmpty(connectionString))
                throw new ArgumentException("connectionString is null or empty.", "connectionString");
            if (String.IsNullOrEmpty(ConnStringName))
                throw new ArgumentException("ConnStringName is null or empty.", "ConnStringName");
            try
            {
                var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                config.ConnectionStrings.ConnectionStrings[ConnStringName].ConnectionString = connectionString;
                config.Save(ConfigurationSaveMode.Modified, true);
                ConfigurationManager.RefreshSection("connectionStrings");
            }
            catch (Exception ex) { Tripous.Sys.ErrorBox(ex); }
        }

        public static bool IsValidEmail(string email)
        {
            //regular expression pattern for valid email
            //addresses, allows for the following domains:
            //com,edu,info,gov,int,mil,net,org,biz,name,museum,coop,aero,pro,tv
            //Regular expression object
            Regex check = new Regex(@"^[-a-zA-Z0-9][-.a-zA-Z0-9]*@[-.a-zA-Z0-9]+(\.[-.a-zA-Z0-9]+)*\.
    (com|edu|info|gov|int|mil|net|org|biz|name|museum|coop|aero|pro|tv|[a-zA-Z]{2})$", RegexOptions.IgnorePatternWhitespace);
            //boolean variable to return to calling method
            bool valid = false;

            //make sure an email address was provided
            if (string.IsNullOrEmpty(email))
            {
                valid = false;
            }
            else
            {
                //use IsMatch to validate the address
                valid = check.IsMatch(email);
            }
            //return the value to the calling method
            return valid;
        }

    }
}
