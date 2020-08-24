using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Download
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

            bool isAutoLogin = args != null && args.Count() > 1;

            //string var = MosesClassLibrary.Security.Encryption.Decrypt("pMjHyBh1atLUMAUBKFvR7w==");
            //check whether another instance of the program, then kill it
            if (System.Diagnostics.Process.GetProcessesByName(System.IO.Path.GetFileNameWithoutExtension(System.Reflection.Assembly.GetEntryAssembly().Location)).Count() > 1) System.Diagnostics.Process.GetCurrentProcess().Kill();

            var gte = TaxSmartConfiguration.ConfigManager.EncryptString("1964");

            TaxSmartConfiguration.ConfigManager config = new TaxSmartConfiguration.ConfigManager();
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

            stateCode = getStatecode();

            StateName = getStateName();

            
            Application.Run(new Form1());
        }

         private static string SQL;

        public static string stateCode = string.Empty; public static bool isCentralData;

        public static string StateName = string.Empty;

        public static string stationName = string.Empty;

        public static string StatePayCode = string.Empty;

        public static string stationCode = string.Empty;

        public static string UserName = string.Empty;

        public static string UserCondition = string.Empty;

        public static string ServerName { get; set; }

         public static string UserID { get; set; }

        public static bool APP_CONNECTED = false;

        public static int intCode = 0;

        //  public static string tmpFolder
        //{
        //    get
        //    {
        //        return Class.ApplicationSettings.ApplicationFolder;
        //    }
        //}

        //static bool IsApplicationValid()
        //{
        //    bool bRes;

        //    string SQL = String.Format("SELECT COUNT(*) AS Count FROM Login.tblApplicationSetUp WHERE (ApplicationCode = '{0}')", ApplicationCode);

        //    if (new Logic().IsRecordExist(SQL))
        //        bRes = true;
        //    else
        //        bRes = false;

        //    return bRes;
        //}

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

        //static void InserApplication()
        //{
        //    using (SqlConnection db = new SqlConnection(Logic.ConnectionString))
        //    {
        //        SqlTransaction transaction;

        //        db.Open();

        //        transaction = db.BeginTransaction();
        //        try
        //        {
        //            string query = string.Format("INSERT INTO Login.tblApplicationSetUp( ApplicationCode ,ApplicationName ,Flag ,Version , VersionNumber )VALUES ('{0}','{1}','{2}','{3}','{4}');", Program.ApplicationCode, Program.ApplicationName, '1', '1', '0');
        //            //string query = String.Format("INSERT INTO [tblBankAccount]([AccountName],[AccountNumber],[BranchID],[IsActive],[IsPlatform],[OpenBal]) VALUES ('{0}','{1}','{2}','{3}','{4}','{5}');", txtName.Text.Trim(), txtNumber.Text.Trim(), Convert.ToInt32(cboBranch.SelectedValue.ToString()), chkActive.Checked, chkPlatform.Checked, txtOpneing.Text);

        //            using (SqlCommand sqlCommand1 = new SqlCommand(query, db, transaction))
        //            {
        //                sqlCommand1.ExecuteNonQuery();
        //            }

        //            transaction.Commit();
        //        }
        //        catch (SqlException sqlError)
        //        {
        //            transaction.Rollback();
        //            Tripous.Sys.ErrorBox(sqlError);
        //            return;
        //        }
        //        db.Close();
        //    }
        //}

        //string path;
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

        //public static bool CheckDatabaseSettings
        //{
        //    get
        //    {
        //        bool bResponse = false;
        //        //if (!string.IsNullOrEmpty(TSS_BusinessLogic.ConnectionString))
        //        //    bResponse = true;
        //        string path = Path.Combine(tmpFolder, "Configuration.xml");

        //        if (File.Exists(path))
        //        {
        //            string dbserver = Configuration2.getSetting("ServerName");
        //            string dbname = Configuration2.getSetting("DatabaseName");
        //            string dbuser = Configuration2.getSetting("Username");
        //            string dbpass = Configuration2.getSetting("Password");
        //            string dbuther = Configuration2.getSetting("useWindowsAuth");

        //            if (
        //                Configuration2.getSetting("ServerName") == string.Empty
        //                || Configuration2.getSetting("DatabaseName") == string.Empty
        //                || Configuration2.getSetting("Username") == string.Empty
        //                || Configuration2.getSetting("Password") == string.Empty
        //                || Configuration2.getSetting("useWindowsAuth") == string.Empty
        //                )
        //            {
        //                bResponse = false;
        //            }
        //            else
        //                bResponse = true;
        //        }
        //        else
        //            bResponse = false;

        //        return bResponse;
        //    }
        //}

    }
}
