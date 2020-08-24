using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Normalization.Forms;
using TaxSmartSuite.Class ;

namespace Normalization
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            TaxSmartSuite.Class.Methods extMethod = new Methods();

            stateCode = getStatecode();

            if (IsApplicationValid())
            {
                string retval;

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                // //check the application for the localize information
                //retval = extMethod.getQuery("state", "");

                //if (string.IsNullOrEmpty(retval.ToString()) || retval.ToString() == "0")
                //{
                //    //Application.Run(new FrmLocalize());
                //}
                //call the sign in form

                //Application.Run(new FrmLogIn());

                //if (FrmLogIn.IsLoggedIn == true)
                //{
                //    //run the menu form
                //    Application.Run(new MDIMain());
                //}

                Application.Run(new MDIMain());
            }
            else
            {
                DialogResult show = MessageBox.Show("Application Not Accessible");
            }
        }

        public static string ApplicationCode = "004";
        public static string stateCode = string.Empty;

        static bool IsApplicationValid()
        {
            TaxSmartSuite.Class.Methods methods = new Methods();

            bool bRes = false;

            string SQL = String.Format("SELECT COUNT(*) AS Count FROM dbo.tblApplicationSetUp WHERE (ApplicationCode = '{0}' and StateCode='{1}')", ApplicationCode, stateCode);

            string retval = methods.ExecuteReader_withValue(SQL);
            if (retval == "1")
            {
                bRes = true;
            }
            return bRes;
        }

        public static string getStatecode()
        {
            TaxSmartSuite.Class.Methods methods = new Methods();
            string query = "select StateCode from tblState where Flag=1";
            string retval = methods.ExecuteReader_withValue(query);

            if (retval != "-1")
            {
                stateCode = retval.ToString();
            }
            else
                stateCode = null;
            return stateCode;
        }


    }
}
