using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using AgencyControl.Forms;
using System.Data;
using AgencyControl.Class;

namespace AgencyControl
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

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form1());
            Application.Run(new MDIMains());
        }

        public static string ApplicationCode = "001";

        public static string stateCode = string.Empty;

        public static string ApplicationName = "Agency Control Panel";

        public static string UserID { get; set; }

        public static bool APP_CONNECTED = false;

        public static string getStatecode()
        {
            string query = "select StateCode from tblState where Flag=1";

            DataTable Dt = (new Logic()).getSqlStatement(query).Tables[0];

            if (Dt != null && Dt.Rows.Count > 0)
            {
                stateCode = String.Format("{0}", Dt.Rows[0]["StateCode"]);
            }


            return stateCode;
        }

        public static string tmpFolder
        {
            get
            {
                return System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "TaxSmartSuite");
            }
        }


    }
}
