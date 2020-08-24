using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using TaxSmartSuite;
using TaxSmartSuite.Class;
using Collection.Forms;
using Collection.Classess;
using System.Data;

namespace Collection
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //TaxSmartSuite.Class.Methods extMethod = new Methods();

            string path = tmpFolder;

            if (!MosesClassLibrary.Configuration2.SetConfigurationFilePath(path))
            {
                TaxSmartSuite.Class.Common.setMessageBox("Error loading application information", ApplicationName, 3);

                return;
            }

            Classess.Logic.ConfigureSettings();


            stateCode = getStatecode();

            TaxSmartSuite.Class.Common.UpdateConfigurationFile(Logic.ConnectionString, "Collection.Properties.Settings.TaxSmartSuiteConnectionString");

            if (IsApplicationValid())
            {
                string retval;

                Application.EnableVisualStyles();

                Application.SetCompatibleTextRenderingDefault(false);

                //////run the menu form
                //Application.Run(new FrmLogin());

                //if (APP_CONNECTED)
                    Application.Run(new MDIMain());


            }
            else
            {
                MessageBox.Show("Application Not Accessible");
            }
        }

        public static string ApplicationCode = "010";

        public static string stateCode = string.Empty;

        public static string ApplicationName = "Collection Manager";

        public static string UserID { get; set; }

        public static bool APP_CONNECTED = false;

        public static string tmpFolder
        {
            get
            {
                return TaxSmartSuite.Class.ApplicationSettings.ApplicationFolder;
            }
        }

        static bool IsApplicationValid()
        {

            bool bRes = false;

            string sql = String.Format("SELECT COUNT(*) AS Count FROM dbo.tblApplicationSetUp WHERE (ApplicationCode = '{0}')", ApplicationCode);

            if (new Logic().IsRecordExist(sql))

            //if (retval == "1")
            {
                bRes = true;
            }
            return bRes;
        }

        public static string getStatecode()
        {
            // TaxSmartSuite.Class.Methods methods = new Methods();
            DataTable Dt = (new Logic()).getSqlStatement("select StateCode from tblState where Flag=1").Tables[0];

            if (Dt != null && Dt.Rows.Count > 0)
            {
                stateCode = String.Format("{0}", Dt.Rows[0]["StateCode"]);
            }


            return stateCode;
        }


        static Program()
        {
            Application.ThreadException += Application_ThreadException;
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if ((e.ExceptionObject is Exception) && !e.IsTerminating)
            {
                Tripous.Sys.ErrorBox(e.ExceptionObject as Exception);
            }
        }

        static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            Tripous.Sys.ErrorBox(e.Exception);
        }


    }


}
