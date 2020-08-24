using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Data;
using Inventory.Class;
using Inventory.Forms;


namespace Inventory
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            string path = tmpFolder;

            if (!MosesClassLibrary.Configuration2.SetConfigurationFilePath(path))
            {
                TaxSmartSuite.Class.Common.setMessageBox("Error loading application information", ApplicationName, 3);

                return;
            }


            Logic.ConfigureSettings();


            stateCode = getStatecode();


            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form1());

            Application.Run(new MDIMain());
        }

        public static string ApplicationCode = "010";

        public static string stateCode = string.Empty;

        public static string ApplicationName = "Inventory";

        public static string UserID { get; set; }

        public static bool APP_CONNECTED = false;

        public static string tmpFolder
        {
            get
            {
                return TaxSmartSuite.Class.ApplicationSettings.ApplicationFolder;

            }
        }

        public static string getStatecode()
        {
            DataTable Dt = (new Logic()).getSqlStatement("select StateCode from tblState where Flag=1").Tables[0];

            if (Dt != null && Dt.Rows.Count > 0)
            {
                stateCode = String.Format("{0}", Dt.Rows[0]["StateCode"]);
            }


            return stateCode;
        }

        static bool IsApplicationValid()
        {

            bool bRes;

            string SQL = String.Format("SELECT COUNT(*) AS Count FROM dbo.tblApplicationSetUp WHERE (ApplicationCode = '{0}')", ApplicationCode);


            if (new Logic().IsRecordExist(SQL))
                bRes = true;
            else
                bRes = false;

            return bRes;
        }

        static bool IsLocalizedApplicationValid()
        {
            bool bRes;

            if (new Logic().IsRecordExist("SELECT COUNT(*) AS Count FROM tblReceiptOffice "))
                bRes = true;
            else
                bRes = false;

            return bRes;
        }

    }
}
