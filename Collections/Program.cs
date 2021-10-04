using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Collection.Forms;
using System.Data;
using Collection.Classess;
using System.Globalization;
using System.Threading;
using BondForm = BondLibrary.Forms;
using System.IO;
using MosesClassLibrary;

namespace Collection
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();

            Application.SetCompatibleTextRenderingDefault(false);

            //string path = tmpFolder;

            bool isAutoLogin = args != null && args.Count() > 1;

            // Sets the CurrentCulture property to U.S. English.
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");

            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException1;
            // Force all WinForms errors to go through handler
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);

            Application.ThreadException += Application_ThreadException1;

            //RpohR9gbCkfm68WwyTTPFA ==
            //RpohR9gCbkfm68WywTTPFA ==old
            string var = MosesClassLibrary.Security.Encryption.Decrypt("wAJGdHnaqS8HiltluSOXVQ==");


            string var2 = MosesClassLibrary.Security.Encryption.Decrypt("cowkqU3RAp5NnBQP5LAEFg==");

            string var3 = MosesClassLibrary.Security.Encryption.Decrypt("fUGCTv0mBudxLzAy/UpOnQ==");

            string var3s = MosesClassLibrary.Security.Encryption.Decrypt("XGTBpxxY8m7A26IoVgykqg==");

            string varg3 = MosesClassLibrary.Security.Encryption.Decrypt("zFinOJ+88wFMs2p5XphOxg==");

            string varg4 = MosesClassLibrary.Security.Encryption.Decrypt("wAJGdHnaqS8HiltluSOXVQ==");

            string varg5 = MosesClassLibrary.Security.Encryption.Decrypt("XGTBpxxY8m7A26IoVgykqg==");



            string var4 = MosesClassLibrary.Security.Encryption.Decrypt("Udn/YDBdmprYCzNIai3VLHXPXP+jepSe");

            var gte = MosesClassLibrary.Security.Encryption.Encrypt("password@1234");

            isCentralData = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["IsCentralData"]) == 1 ? true : false;
#if true
            SystemName = System.Environment.MachineName;
            //var configPath = TaxSmartConfiguration.ConfigManager.GetServerName;
            //bool isFullPath = !isCentralData;

            TaxSmartConfiguration.ConfigManager config = new TaxSmartConfiguration.ConfigManager(TaxSmartConfiguration.ConfigManager.GetServerName);

            if (!config.IsDefaultFolderExist)
            {
                //Load Database Settings
                TaxSmartConfiguration.Winform.FrmDatabaseSetup frmDatabaseSetup = new TaxSmartConfiguration.Winform.FrmDatabaseSetup(TaxSmartConfiguration.ConfigManager.GetServerName);
                frmDatabaseSetup.ShowDialog();
                if (!frmDatabaseSetup.Status)
                {
                    return;
                }
            }
            if (!Logic.LoadConfig())
                return;

            //MessageBox.Show(Logic.ConnectionString);
            //return;
            //if (Logic.LoadConfig2()) return;

#else
            if (!MosesClassLibrary.Configuration2.SetConfigurationFilePath(path))
            {
                TaxSmartSuite.Class.Common.setMessageBox("Error loading application information", ApplicationName, 3);

                return;
            }


            if (!CheckDatabaseSettings)//check if there 
            {
                using (BondForm.FrmConnections frmdbocnnect = new BondForm.FrmConnections(Program.ApplicationName, "TaxSmartSuite", path))
                {

                    frmdbocnnect.ShowDialog();

                    if (!frmdbocnnect.retValue)
                    {
                        TaxSmartSuite.Class.Common.setMessageBox("Application Could not Connect to the Sever", ApplicationName, 3);
                        return;
                    }
                }
            }

            Logic.ConfigureSettings();
#endif


            stateCode = getStatecode();

            //passwords = getUserPassword();

            if (Program.isCentralData)
            {
                stationName = "Central Station";

                stationCode = "0009";
            }
            else
            {
                stationName = getStationName();

                stationCode = getStationCode();
            }



            //check station code
            //if (string.IsNullOrEmpty(stationCode))
            //{ }

            //TaxSmartSuite.Class.Common.UpdateConfigurationFile(Logic.ConnectionString, "Collection.Properties.Settings.TaxSmartSuiteConnectionString");

            //TaxSmartSuite.Class.Common.UpdateConfigurationFile(Logic.ConnectionString, "Collection.Properties.Settings.TaxSmartSuiteRevisedConnectionString");

            //Guid g = Guid.NewGuid();
            //customerID = string.Format("{0}{1}{2}", DateTime.Now.ToString("yyyyMMdd"), "-", custIDIncrementer);


            if (IsApplicationValid())
            {

                var frm = new MDIMain();

                NavBars.ManageNavBarControls(frm.navBarControl1, Program.ApplicationCode);

                SQL = "SELECT  TOP (1) StateName FROM dbo.ViewDefaultSetupRevenueOffice2";

                StateName = (new Logic()).ExecuteScalar(SQL);

                //Application.EnableVisualStyles();
                //Application.SetCompatibleTextRenderingDefault(false);

                //check whether another instance of the program, then kill it

                //if (System.Diagnostics.Process.GetProcessesByName(System.IO.Path.GetFileNameWithoutExtension(System.Reflection.Assembly.GetEntryAssembly().Location)).Count() > 1) System.Diagnostics.Process.GetCurrentProcess().Kill();


                var login = new FrmLogin(isAutoLogin);

                if (isAutoLogin)
                {
                    login.Username = args[0];
                    login.Password = args[1];

                    UserID = args[0];
                    //passwords = args[1];
                }


                //run the menu form
                Application.Run(login);
                UserName = getUsername();
                Userphone = getuserphone();
                if (APP_CONNECTED)
                    Application.Run(new MDIMain());


            }
            else
            {
                MessageBox.Show("Application Not Accessible");
            }

            //}
            //else
            //{

            //}


        }

        private static void Application_ThreadException1(object sender, ThreadExceptionEventArgs e)
        {
            try
            {
                MessageBox.Show("Unhandled exception catched.\n Application is going to close now.");
            }
            catch
            {
                try
                {
                    MessageBox.Show("Fatal exception happend inside UIThreadException handler",
                        "Fatal Windows Forms Error", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Stop);
                }
                finally
                {
                    Application.Exit();
                }
            }

            // Here we can decide if we want to end our application or do something else
            Application.Exit();
        }

        private static void CurrentDomain_UnhandledException1(object sender, UnhandledExceptionEventArgs e)
        {
            //MessageBox.Show((e.ExceptionObject as Exception).Message, "Unhandled domain Exception");
            //Application.Exit();

            try
            {
                Exception ex = (Exception)e.ExceptionObject;
                MessageBox.Show("Unhadled domain exception:\n\n" + ex.Message);
            }
            catch (Exception exc)
            {
                try
                {
                    MessageBox.Show("Fatal exception happend inside UnhadledExceptionHandler: \n\n"
                        + exc.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
                finally
                {
                    Application.Exit();
                }
            }

        }

        public static string ApplicationCode = "003";

        private static string SQL;

        public static bool isCentralData;

        public static string stateCode = string.Empty;

        public static string stationName = string.Empty;

        public static string stationCode = string.Empty;

        public static string ApplicationName = "Collection";

        public static string Userphone = string.Empty;

        public static string SystemName = string.Empty;

        public static string UserID { get; set; }

        public static bool IsSplitRecord = false;
        public static string ServerName { get; set; }

        public static string StateName { get; set; }

        public static int intCode = 0;

        public static string UserName { get; set; }

        public static bool APP_CONNECTED = false;

        public static bool App_LOGOUT = false;

        public static bool IsReprint = false;

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

            string sql = String.Format("SELECT COUNT(*) AS Count FROM Login.tblApplicationSetUp  WHERE (ApplicationCode = '{0}')", ApplicationCode);
            //string sql = String.Format("SELECT COUNT(*) AS Count FROM dbo.tblApplicationSetUp WHERE (ApplicationCode = '{0}')", ApplicationCode);

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
            DataTable Dt = (new Logic()).getSqlStatement("select StateCode from Registration.tblState where Flag=1").Tables[0];
            //DataTable Dt = (new Logic()).getSqlStatement("select StateCode from tblState where Flag=1").Tables[0];

            if (Dt != null && Dt.Rows.Count > 0)
            {
                stateCode = String.Format("{0}", Dt.Rows[0]["StateCode"]);
                intCode = Convert.ToInt32(stateCode);
            }


            return stateCode;
        }

        public static string getStationName()
        {
            if (Program.isCentralData)
            {
                stationName = "Central Station";
            }
            else
            {
                //DataTable Dt = (new Logic()).getSqlStatement("select stationname from tblstation2").Tables[0];
                DataTable Dt = (new Logic()).getSqlStatement("select stationname from Receipt.tblStation").Tables[0];

                if (Dt != null && Dt.Rows.Count > 0)
                {
                    stationName = String.Format("{0}", Dt.Rows[0]["stationname"]);
                }

            }
            return stationName;
        }

        public static string getStationCode()
        {
            if (Program.isCentralData)
            {
                stationCode = System.Configuration.ConfigurationManager.AppSettings["CentralCode"].ToString();
            }
            else
            {
                DataTable Dt = (new Logic()).getSqlStatement("select StationCode from Receipt.tblStation").Tables[0];

                if (Dt != null && Dt.Rows.Count > 0)
                {
                    stationCode = String.Format("{0}", Dt.Rows[0]["StationCode"]);
                }
                else
                {
                    //FrmStations station = new FrmStations();
                    //station.ShowDialog();
                    FrmStationname station = new FrmStationname();
                    station.ShowDialog();
                }
            }

            return stationCode;
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

        public static bool CheckDatabaseSettings
        {
            get
            {
                bool bResponse = false;
                //if (!string.IsNullOrEmpty(TSS_BusinessLogic.ConnectionString))
                //    bResponse = true;
                string path = Path.Combine(tmpFolder, "Configuration.xml");

                if (File.Exists(path))
                {
                    string dbserver = Configuration2.getSetting("ServerName");
                    string dbname = Configuration2.getSetting("DatabaseName");
                    string dbuser = Configuration2.getSetting("Username");
                    string dbpass = Configuration2.getSetting("Password");
                    string dbuther = Configuration2.getSetting("useWindowsAuth");

                    if (
                        Configuration2.getSetting("ServerName") == string.Empty
                        || Configuration2.getSetting("DatabaseName") == string.Empty
                        || Configuration2.getSetting("Username") == string.Empty
                        || Configuration2.getSetting("Password") == string.Empty
                        || Configuration2.getSetting("useWindowsAuth") == string.Empty
                        )
                    {
                        bResponse = false;
                    }
                    else
                        bResponse = true;
                }
                else
                    bResponse = false;

                return bResponse;
            }
        }

        public static string getUsername()
        {
            string grt = string.Format("SELECT * FROM [dbo].[ViewUserApplication] WHERE [UserID] = '{0}'", Program.UserID);

            DataTable Dt = (new Logic()).getSqlStatement(grt).Tables[0];

            if (Dt != null && Dt.Rows.Count > 0)
            {
                UserName = String.Format("{0} - {1}", Dt.Rows[0]["Surname"], Dt.Rows[0]["Othernames"]);
            }

            return UserName;
        }

        static string getuserphone()
        {
            string quey = string.Format("SELECT Telephone FROM Login.tblUsers WHERE UserID='{0}'", Program.UserID);
            DataTable Dt = (new Logic()).getSqlStatement(quey).Tables[0];

            if (Dt != null && Dt.Rows.Count > 0)
            {
                Userphone = String.Format("{0}", Dt.Rows[0]["Telephone"]);

                //intCode = Convert.ToInt32(stateCode);

                //StateName = String.Format("{0}", Dt.Rows[0]["StateName"]);
            }

            return Userphone;
        }

    }
}
