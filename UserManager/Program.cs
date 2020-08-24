using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Tripous;
using UserManager.Classess;

namespace UserManager
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            using (im = new InstanceManager(AppUniqueId))
            {
                #region Check for multiple instances
                if (!im.IsSingleInstance)
                {
                    Sys.ErrorBox("This application is already running!");
                    Application.Exit();
                    return;
                }
                #endregion

                #region Load Application Settings
                //string path = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "TaxSmartSuite");
                string path = tmpFolder;
                if (!MosesClassLibrary.Configuration2.SetConfigurationFilePath(path))
                {
                    TaxSmartSuite.Common.setMessageBox("Error loading application information", ApplicationName, 3);
                    return;
                }
                BusinessLogic.ConfigureSettings();
                #endregion

                if (IsApplicationExistAndAllowed)
                {
                    SQL = "SELECT TOP (1) StateCode FROM dbo.ViewDefaultSetupRevenueOffice";
                    StateCode = (new BusinessLogic()).ExecuteScalar(SQL);
                    if (!String.IsNullOrEmpty(StateCode))
                    {
                        SQL = "SELECT TOP (1) StateName FROM dbo.ViewDefaultSetupRevenueOffice";
                        StateName = (new BusinessLogic()).ExecuteScalar(SQL);

                        Application.EnableVisualStyles();
                        Application.SetCompatibleTextRenderingDefault(false);
                        //Application.Run(new MDIMainForm());
                        do
                        {
                            APP_LOGOUT = false;
                            Application.Run(new Forms.FrmLogin());
                            if (APP_CONNECTED)
                                Application.Run(new MDIMainForm());
                        } while (APP_LOGOUT);
                    }
                }
                else
                {
                    TaxSmartSuite.Common.setMessageBox(String.Format("{0} is not localised.{1}Please contact HeadQuarter for support", ApplicationName, Environment.NewLine), ApplicationName, 3);
                }
            }
        }

        static private InstanceManager im;
        private const string AppUniqueId = "{ICMA Services :: Tax Smart User Manager}";
        public static string ApplicationCode = "005";
        public static string ApplicationName = "Tax Smart User Manager";
        public static Int32 RefNum = 20;

        public static string StateCode { get; set; }
        public static string StateName { get; set; }
        public static string ServerName { get; set; }
        public static string UserID { get; set; }
        public static bool APP_CONNECTED = false;
        public static bool APP_LOGOUT = false;
        public static string tmpFolder
        {
            get
            {
                //return System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "TaxSmartSuite");
                return TaxSmartSuite.ApplicationSettings.ApplicationFolder;
            }
        }

        static string SQL = "SELECT COUNT(*) AS Count FROM dbo.ViewDefaultSetupRevenueOffice";

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
                Sys.ErrorBox(e.ExceptionObject as Exception);
            }
        }

        static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            Sys.ErrorBox(e.Exception);
        }

        private static bool IsApplicationAllowed
        {
            get
            {
                return (new BusinessLogic()).IsRecordExist(SQL);
                //return bRes;
            }
        }

        private static bool IsApplicationExistAndAllowed
        {
            get
            {
                string sql = string.Format("SELECT [ApplicationCode] FROM [dbo].[tblApplicationSetUp] WHERE [ApplicationCode] = '{0}' AND [Flag] = {1}", ApplicationCode, true);
                //bool lol = (new BusinessLogic()).IsRecordExist(SQL);
                return (new BusinessLogic()).IsRecordExist(SQL);
            }
        }
    }
}
