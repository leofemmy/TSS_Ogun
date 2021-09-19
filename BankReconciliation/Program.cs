using BankReconciliation.Class;
using BankReconciliation.Forms;
using MosesClassLibrary;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Windows.Forms;


namespace BankReconciliation
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>m
        [STAThread]
        static void Main(string[] args)
        {

            Application.EnableVisualStyles();

            Application.SetCompatibleTextRenderingDefault(false);

            //string path = tmpFolder;

            bool isAutoLogin = args != null && args.Count() > 1;

            //check whether another instance of the program, then kill it
            if (System.Diagnostics.Process.GetProcessesByName(System.IO.Path.GetFileNameWithoutExtension(System.Reflection.Assembly.GetEntryAssembly().Location)).Count() > 1) System.Diagnostics.Process.GetCurrentProcess().Kill();

            string GH = MosesClassLibrary.Security.Encryption.Decrypt("XSP+kdUZvsyEuNRLXy+lWGxwzndM35Lj");
            string var = MosesClassLibrary.Security.Encryption.Decrypt("3V1el5Ftwt3JJqu+HasHtg==");
            string vars = MosesClassLibrary.Security.Encryption.Decrypt("TPa7tFbIUYRBs5LESu/2Hg==");
            string var2 = MosesClassLibrary.Security.Encryption.Decrypt("DCb5DBDO2BtbqsOLHpWJ7A==");
            string var4 = MosesClassLibrary.Security.Encryption.Decrypt("D30t9qV1aA8NKPPPz61kmjatzwmaDX2S");
            

            var gte = TaxSmartConfiguration.ConfigManager.EncryptString("james007");

#if true
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
#else
            if (!MosesClassLibrary.Configuration2.SetConfigurationFilePath(path))
            {
                TaxSmartSuite.Class.Common.setMessageBox("Error loading application information", ApplicationName, 3);
                //return;
            }



            if (!CheckDatabaseSettings)//check if there 
            {
                BondForm.FrmConnections dbconnect = new BondForm.FrmConnections(Program.ApplicationName, "TaxSmartSuite", path);

                dbconnect.ShowDialog();

                if (!dbconnect.retValue)
                {
                    TaxSmartSuite.Class.Common.setMessageBox("Application Could not Connect to the Sever", ApplicationName, 3);

                    return;
                }
            }

            Logic.ConfigureSettings();
#endif

            stateCode = getStatecode();

            //StatePayCode = getStatePayCode();

         

            //stationName = getStationName();

            //setting connection string for app.config
            //TaxSmartSuite.Class.Common.UpdateConfigurationFile(Logic.ConnectionString, "BankReconciliation.Properties.Settings.TaxSmartSuiteRevised_oyoConnectionString");


            if (IsApplicationValid())
            {
                string retval;

                //var frm = new MDIMain();
                var frm = new MDIMains();

                NavBars.ManageNavBarControls(frm.navBarControl1, Program.ApplicationCode);

                var strvale = MosesClassLibrary.Security.Encryption.Decrypt("xiSttfx7JNEqFPn7HB0W7g==");
                //SQL = "SELECT  TOP (1) StateName FROM dbo.ViewDefaultSetupRevenueOffice2";

                //StateName = (new Logic()).ExecuteScalar(SQL);


                //string testApplication = System.IO.Path.GetFileNameWithoutExtension(System.Reflection.Assembly.GetEntryAssembly().Location);



                var login = new FrmLogin(isAutoLogin);
                if (isAutoLogin)
                {
                    login.Username = args[0];
                    login.Password = args[1];
                }


                //run the menu form
                Application.Run(login);
                Userphone = getuserphone();
                StateName = getStateName();

                UserEmail = getUseremail();


                loginStatus = true;
                if (APP_CONNECTED)
                    Application.Run(new MDIMains());

             //run the main menu form
                //Application.Run(new MDIMain());

                //SingleApplication.Run(new MDIMain());

                //run the menu form
                //Application.Run(login);
                //UserName = getUsername();
                //if (APP_CONNECTED)
                //    Application.Run(new MDIMain());

            }
            else
            {
                InserApplication();

                MessageBox.Show("Application Not Accessible");
            }


        }

        public static string ApplicationCode = "011";

        private static string SQL;

        public static string stateCode = string.Empty;

        public static string StateName = string.Empty;

        public static string StatePayCode = string.Empty;

        public static string Userphone = string.Empty;

        public static string UserEmail = string.Empty;

        public static string ServerName { get; set; }

        public static string ApplicationName = "Reconciliation Manager";

        public static string UserID { get; set; }


        public static bool APP_CONNECTED = false;

        public static bool loginStatus = false;

        public static Int32 loginid = 0;

        public static bool tokenStatus = false;


        public static int intCode = 0;
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

            string SQL = String.Format("SELECT COUNT(*) AS Count FROM Login.tblApplicationSetUp WHERE (ApplicationCode = '{0}')", ApplicationCode);

            if (new Logic().IsRecordExist(SQL))
                bRes = true;
            else
                bRes = false;

            return bRes;
        }

        public static string getStatecode()
        {
            DataTable Dt = (new Logic()).getSqlStatement("SELECT StateCode,StateName FROM Registration.tblState where Flag=1").Tables[0];

            if (Dt != null && Dt.Rows.Count > 0)
            {
                stateCode = String.Format("{0}", Dt.Rows[0]["StateCode"]);

                intCode = Convert.ToInt32(stateCode);

                //StateName = String.Format("{0}", Dt.Rows[0]["StateName"]);
            }

            return stateCode;
        }

        public static string getStateName()
        {
            DataTable Dt = (new Logic()).getSqlStatement("SELECT StateCode,StateName FROM Registration.tblState where Flag=1").Tables[0];

            if (Dt != null && Dt.Rows.Count > 0)
            {
                //stateCode = String.Format("{0}", Dt.Rows[0]["StateCode"]);

                StateName = String.Format("{0}", Dt.Rows[0]["StateName"]);
            }

            return StateName;
        }

        //static bool IsLocalizedApplicationValid()
        //{
        //    bool bRes;

        //    if (new Logic().IsRecordExist("SELECT COUNT(*) AS Count FROM tblReceiptOffice "))
        //        bRes = true;
        //    else
        //        bRes = false;

        //    return bRes;
        //}

        //static string getStatePayCode()
        //{
        //    DataTable Dt = (new Logic()).getSqlStatement("SELECT  PDReference FROM dbo.tblCashOptions").Tables[0];

        //    if (Dt != null && Dt.Rows.Count > 0)
        //    {
        //        StatePayCode = String.Format("{0}", Dt.Rows[0]["PDReference"]);

        //    }

        //    return StatePayCode;

        //}

        static void InserApplication()
        {
            using (SqlConnection db = new SqlConnection(Logic.ConnectionString))
            {
                SqlTransaction transaction;

                db.Open();

                transaction = db.BeginTransaction();
                try
                {
                    string query = string.Format("INSERT INTO Login.tblApplicationSetUp( ApplicationCode ,ApplicationName ,Flag ,Version , VersionNumber )VALUES ('{0}','{1}','{2}','{3}','{4}');", Program.ApplicationCode, Program.ApplicationName, '1', '1', '0');
                    //string query = String.Format("INSERT INTO [tblBankAccount]([AccountName],[AccountNumber],[BranchID],[IsActive],[IsPlatform],[OpenBal]) VALUES ('{0}','{1}','{2}','{3}','{4}','{5}');", txtName.Text.Trim(), txtNumber.Text.Trim(), Convert.ToInt32(cboBranch.SelectedValue.ToString()), chkActive.Checked, chkPlatform.Checked, txtOpneing.Text);

                    using (SqlCommand sqlCommand1 = new SqlCommand(query, db, transaction))
                    {

                        sqlCommand1.ExecuteNonQuery();
                    }

                    transaction.Commit();
                }
                catch (SqlException sqlError)
                {
                    transaction.Rollback();
                    Tripous.Sys.ErrorBox(sqlError);
                    return;
                }
                db.Close();
            }
        }

        static string getuserphone()
        {
            string quey = string.Format("SELECT Telephone FROM Login.tblUsers WHERE UserID='{0}'", Program.UserID);
            DataTable Dt = (new Logic()).getSqlStatement(quey).Tables[0];

            if (Dt != null && Dt.Rows.Count > 0)
            {
                Userphone = String.Format("{0}", Dt.Rows[0]["Telephone"]);
            }

            return Userphone;
        }

        static string getUseremail()
        {
            string quey = string.Format("SELECT EmailAddress FROM Login.tblUsers WHERE UserID='{0}'", Program.UserID);
            DataTable Dt = (new Logic()).getSqlStatement(quey).Tables[0];

            if (Dt != null && Dt.Rows.Count > 0)
            {
                UserEmail = String.Format("{0}", Dt.Rows[0]["EmailAddress"]);
            }

            return UserEmail;
        }

        //string path;

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

    }
}
