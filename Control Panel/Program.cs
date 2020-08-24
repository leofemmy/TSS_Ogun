using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Control_Panel.Forms;
using ApplicationModules;
using MosesClassLibrary;
using TaxSmartSuite;
using TaxSmartSuite.Class;
using Control_Panel.Class;
using System.Data;

namespace Control_Panel
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]

        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            string path = tmpFolder;

            if (!MosesClassLibrary.Configuration2.SetConfigurationFilePath(path))
            {
                TaxSmartSuite.Class.Common.setMessageBox("Error loading application information", ApplicationName, 3);

                return;
            }

            Logic.ConfigureSettings();

            //stateCode = getStatecode();

            TaxSmartSuite.Class.Common.UpdateConfigurationFile(Logic.ConnectionString, "Control_Panel.Properties.Settings.TaxSmartSuiteConnectionString");

            //call state code

            if (!IsgetStatecode())
            {
              
                Common.setMessageBox("Please  Set the Default State of the Application", Program.ApplicationName, 2);
            }

            if (IsApplicationValid())
            {
                var frm = new MDIMain();

                NavBars.ManageNavBarControls(frm.navBarControl1, Program.ApplicationCode);


                //SQL = "SELECT  TOP (1) StateName FROM dbo.ViewDefaultSetupRevenueOffice2";
                SQL = "select StateName from tblState where Flag=1";
                StateName = (new Logic()).ExecuteScalar(SQL);

                //check whether another instance of the program, then kill it
                if (System.Diagnostics.Process.GetProcessesByName(System.IO.Path.GetFileNameWithoutExtension(System.Reflection.Assembly.GetEntryAssembly().Location)).Count() > 1) System.Diagnostics.Process.GetCurrentProcess().Kill();

                //run the menu form
                Application.Run(new FrmLogin());

                if (APP_CONNECTED)
                    Application.Run(new MDIMain());
                //    SingleApplication.Run(new MDIMain());
                ////SingleApplication.Run(frm);

                //}

            }
            else
            {
                MessageBox.Show("Application Not Accessible");
            }
        }

        public static string ApplicationCode = "001";

        public static string stateCode = string.Empty;

        private static string SQL;

        public static string ApplicationName = "Control Panel";

        public static string UserID { get; set; }

        public static string ServerName { get; set; }

        public static string StateName { get; set; }

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

            bool bRes;

            string SQL = String.Format("SELECT COUNT(*) AS Count FROM tblApplicationSetUp WHERE (ApplicationCode = '{0}')", ApplicationCode);

            if (new Logic().IsRecordExist(SQL))
                bRes = true;
            else
                bRes = false;

            return bRes;
        }

        static bool IsgetStatecode()
        {
            bool bRes;

            //TaxSmartSuite.Class.Methods methods = new Methods();
            DataTable Dt = (new Logic()).getSqlStatement("select StateCode from tblState where Flag=1").Tables[0];

            if (Dt != null && Dt.Rows.Count > 0)
            {
                stateCode = String.Format("{0}", Dt.Rows[0]["StateCode"]);
                bRes = true;
            }
            else
            {
                bRes = false;
            }


            return bRes;
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
