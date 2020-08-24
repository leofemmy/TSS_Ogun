//#define HOME
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using TaxSmartSuite.Class;
using MosesClassLibrary;
using System.Data.SqlClient;
using System.Data;
using Data = System.Data;
using DevExpress.XtraNavBar;
using MosesClassLibrary.DataAccessLayer;
using System.Windows.Forms;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Configuration;
using System.Net.NetworkInformation;
using System.IO;
using Collection.Classes;

namespace Collection.Classess
{
    public class NavBars
    {


        //SqlConnection 
        public static ArrayList ArrUserRight = new ArrayList();

        public static void ManageNavBarControls(NavBarControl NavBar, string AppCode)
        {
            var Blogic = new Logic();

            foreach (NavBarGroup GroupList in NavBar.Groups)
            {
                if (Blogic.InsertNavModules(GroupList.Tag as string, GroupList.Caption, AppCode))
                {
                    foreach (NavBarItemLink ItemList in GroupList.ItemLinks)
                    {
                        NavBarItem items = ItemList.Item;

                        if (!Blogic.InsertNavModulesAccess(items.Tag as string, items.Caption, GroupList.Tag as string))
                        {
                            Common.setMessageBox("Error Updating Application Modules Access", Program.ApplicationName, 3);

                            break;
                        }
                    }
                }
                else
                {
                    //if (String.IsNullOrEmpty(Blogic.GetLastErrorMessage))
                    //{
                    //    Common.setMessageBox("Error Updating Application Modules", Program.ApplicationName, 3);
                    //    break;
                    //}
                }
            }
        }

        public static void NavBarEnableDisableControls(NavBarControl NavBar, bool status)
        {
            if (NavBar == null) return;

            foreach (NavBarGroup GroupList in NavBar.Groups)
            {

                if (object.Equals(GroupList.Tag, "003-005"))

                    GroupList.Visible = true;

                else
                    GroupList.Visible = status;

                foreach (NavBarItemLink ItemList in GroupList.ItemLinks)
                {

                    //navBarItemCheckUpdate
                    //navBarItemReceiptGenerated
                    //|| (ItemList.ItemName == "navBarItCheck")
                    //|| (ItemList.ItemName == "navBarItemStationData")navBarItemReprintReceipt,navBarItemReceiptCancel,|| (ItemList.ItemName == "navBarItemReceiptCancel")
                    if ((ItemList.ItemName == "navBarItemExit") || (ItemList.ItemName == "navBarItemLogout") || (ItemList.ItemName == "navBarItemCentreData") || (ItemList.ItemName == "navBarItemStationData") || (ItemList.ItemName == "navBarItCheck") || (ItemList.ItemName == "navBarItemReprintReceipt") || (ItemList.ItemName == "navBarItemCheckUpdate") || (ItemList.ItemName == "navBarItemTransactionPending")) continue;

                    ItemList.Visible = status;
                }
            }
        }

        public static void NavBarEnableDisableControls(NavBarControl NavBar, bool status, string Code, bool IsFirstLevel)
        {
            if (NavBar == null) return;

            foreach (NavBarGroup GroupList in NavBar.Groups)
            {
                if (IsFirstLevel)
                {
                    if (GroupList.Tag == "002-004" as object) continue;

                    if (object.Equals(GroupList.Tag, Code))

                        GroupList.Visible = status;
                }
                else
                {
                    foreach (NavBarItemLink ItemList in GroupList.ItemLinks)
                    {
                        if (ItemList.ItemName == "navBarItemExit") continue;

                        NavBarItem items = ItemList.Item;

                        if (object.Equals(items.Tag, Code))

                            ItemList.Visible = status;
                    }
                }
            }
        }

        public static bool ManageUserLoginNavBar(NavBarControl NavBar)
        {
            if (NavBar == null) return false;

            string UserID = Program.UserID;

            string AppCode = Program.ApplicationCode;

            if (String.IsNullOrEmpty(UserID)) return false;

            bool bResponse = false;

            /* •———————————————————————————————•
               | Load User Application Modules |
               •———————————————————————————————• */
            DataTable Dt;

            string SQL = String.Format(@"SELECT [ModulesCode],[ModulesName] FROM [dbo].[ViewUserApplicationModules] 
                    WHERE [UserID] = '{0}' AND [ApplicationCode] = '{1}'", UserID, AppCode);

            Dt = (new Logic()).getSqlStatement(SQL).Tables[0];

            if (Dt != null && Dt.Rows.Count > 0)
            {
                foreach (DataRow Dr in Dt.Rows)
                {
                    // Make visible the Menu group
                    NavBarEnableDisableControls(NavBar, true, Dr["ModulesCode"] as string, true);
                    /* •——————————————————————————————————————•
                       | Load User Application Modules Access |
                       •——————————————————————————————————————• */
                    SQL = String.Format(@"SELECT [ModuleAccessCode],[ModuleAccessName] 
                            FROM [dbo].[ViewUserApplicationModulesAccess] 
                            WHERE [UserID] = '{0}' AND [ModulesCode] = '{1}' AND [ApplicationCode] = '{2}'"
                        , UserID, Dr["ModulesCode"], AppCode);

                    DataTable Dtt = (new Logic()).getSqlStatement(SQL).Tables[0];

                    if (Dtt != null && Dtt.Rows.Count > 0)
                    {
                        foreach (DataRow Drr in Dtt.Rows)
                        {
                            // Make Visible the Menu Group Items
                            NavBarEnableDisableControls(NavBar, true, Drr["ModuleAccessCode"] as string, false);

                            LoadUserApplicationRight(UserID, Dr["ModulesCode"] as string, Drr["ModuleAccessCode"] as string, AppCode);
                        }
                    }
                }
                bResponse = true;
            }
            return bResponse;
        }

        protected static void LoadUserApplicationRight(string UserID, string ModulesCode
            , string ModuleAccessCode, string AppCode)
        {
            if (String.IsNullOrEmpty(UserID) || String.IsNullOrEmpty(ModulesCode) || String.IsNullOrEmpty(ModuleAccessCode) || String.IsNullOrEmpty(AppCode))

                return;

            string SQL = String.Format(@"SELECT [ApplicationRghtID],[ApplicationRightName],[ModulesCode],[ModuleAccessCode] FROM [dbo].[ViewUserApplicationRight]  WHERE [UserID] = '{0}' AND [ModulesCode] = '{1}' AND [ModuleAccessCode] = '{2}' AND [ApplicationCode] = '{3}'", UserID, ModulesCode, ModuleAccessCode, AppCode);

            DataTable Dt = (new Logic()).getSqlStatement(SQL).Tables[0];

            if (Dt != null && Dt.Rows.Count > 0)
            {
                foreach (DataRow Dr in Dt.Rows)
                {
                    if (Dr != null)

                        ArrUserRight.Add(Dr);
                }
            }
        }

        public static void ToolStripEnableDisableControls(ToolStrip toolStrip, string ModulesAccessCode)
        {
            if (toolStrip == null) return;

            if (String.IsNullOrEmpty(ModulesAccessCode)) return;

            ToolStripEnableDisable(toolStrip, null, false);

            if (ArrUserRight != null && ArrUserRight.Count > 0)
            {
                foreach (DataRow Dr in ArrUserRight)
                {
                    if (Dr != null)
                    {
                        if (String.Equals(Dr["ModuleAccessCode"] as string, ModulesAccessCode))
                        {
                            int right = Convert.ToInt32(Dr["ApplicationRghtID"]);

                            string Code = string.Empty;

                            switch (right)
                            {
                                case 1:
                                    Code = ApplicationRightCode.Add;

                                    break;

                                case 2:
                                    Code = ApplicationRightCode.Edit;

                                    break;

                                case 3:
                                    Code = ApplicationRightCode.Delete;

                                    break;

                                default:
                                    Code = String.Empty;

                                    break;
                            }
                            if (!String.IsNullOrEmpty(Code))

                                ToolStripEnableDisable(toolStrip, Code, true);
                        }
                    }
                }
            }
        }

        public static void ToolStripEnableDisable(ToolStrip toolStrip, string Code, bool Status)
        {
            if (toolStrip == null) return;

            foreach (ToolStripItem item in toolStrip.Items)
            {
                if (item.GetType().ToString() == typeof(ToolStripButton).ToString())
                {
                    ToolStripButton toolStripButton = item as ToolStripButton;

                    if (String.IsNullOrEmpty(Code))
                    {
                        if (String.Equals(toolStripButton.Tag, ApplicationRightCode.Close)) continue;

                        item.Enabled = Status;

                        continue;
                    }
                    else if (String.Equals(toolStripButton.Tag, Code))
                    {
                        toolStripButton.Enabled = Status;
                    }
                }
            }
        }

        public static DataColumn AddColumn(DataTable data, string name, Type type, bool ro, int pos)
        {
            DataColumn col = data.Columns.Add(name, type);
            col.Caption = name;
            col.ReadOnly = ro;
            col.SetOrdinal(pos);
            return col;
        }
    }

    public class Token
    {
        static string strtoken;

        static private SqlCommand _command;

        static private SqlDataAdapter adp;

        static DateTime dt = new DateTime();

        public static string GenerateToken()
        {
            //otp = Convert.ToString(rand.Next(10000, 1000000));
            Random RNG = new Random();

            string token = Convert.ToString(RNG.Next(10000, 1000000));

            return token;
        }

        public static void dotoken()
        {
            strtoken = Token.GenerateToken();

            if (CheckTokenperday())
            {
                TaxSmartSuite.Class.Common.setMessageBox("Please use your current token received early on to authorized", "Token Request", 1);
                //return ;
            }
            else
            {

                if (tokenInsertValidation(Program.UserID, Program.ApplicationCode, strtoken, false, "New"))
                {
                    try
                    {
                        var procesSms = new processSms.ProcessSms();

                        string strprocessSme = procesSms.SendSms(Program.Userphone, "Token", strtoken);

                        if (strprocessSme.Contains("Failed"))
                        {
                            Tripous.Sys.ErrorBox(strprocessSme.ToString());

                            TaxSmartSuite.Class.Common.setMessageBox(strprocessSme.ToString(), "Get Token", 1);

                            //return false;
                        }
                        else
                        {
                            TaxSmartSuite.Class.Common.setMessageBox(string.Format("Token Request sent to your registered number {0}.", $"********{Program.Userphone.Substring(7)}"), "Token Request", 1);

                            dt = DateTime.Now;

                            //return true;
                        }

                    }
                    catch (Exception ex)
                    {
                        Tripous.Sys.ErrorBox(ex.Message);

                        //return false;
                    }


                }
                //else
                //return false;
            }
        }

        public static bool CheckTokenperday()
        {
            bool respones = false;

            using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
            {
                connect.Open();
                _command = new SqlCommand("Reconciliation.Checktoken", connect) { CommandType = CommandType.StoredProcedure };
                _command.Parameters.Add(new SqlParameter("@userid", SqlDbType.VarChar)).Value = Program.UserID;
                //_command.Parameters.Add(new SqlParameter("@usertoken", SqlDbType.VarChar)).Value = strtoken;
                _command.Parameters.Add(new SqlParameter("@application", SqlDbType.VarChar)).Value = Program.ApplicationCode;
                _command.Parameters.Add(new SqlParameter("@CheckDatetime", SqlDbType.DateTime)).Value = DateTime.Today.ToString("yyyy/MM/dd hh:mm:ss");// DateTime.Now;
                using (System.Data.DataSet ds = new System.Data.DataSet())
                {
                    ds.Clear();
                    adp = new SqlDataAdapter(_command);
                    adp.Fill(ds);
                    connect.Close();

                    if (ds.Tables[0].Rows[0]["returnCode"].ToString() == "-1")
                    {
                        TaxSmartSuite.Class.Common.setMessageBox(ds.Tables[0].Rows[0]["returnMessage"].ToString(), Program.ApplicationName, 2);

                        respones = false;

                    }
                    else if (ds.Tables[0].Rows[0]["returnCode"].ToString() == "00")
                    {
                        respones = false;
                    }
                    else if (ds.Tables[0].Rows[0]["returnCode"].ToString() == "01")
                    {
                        respones = true;
                    }

                }
            }


            return respones;
        }

        public static bool tokenInsertValidation(string userid, string ApplicationCode, string strtoken, bool status, string Description)
        {
            using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
            {
                connect.Open();

                _command = new SqlCommand("Reconciliation.tokenInsertValidation", connect) { CommandType = CommandType.StoredProcedure };
                _command.Parameters.Add(new SqlParameter("@userid", SqlDbType.VarChar)).Value = userid;
                _command.Parameters.Add(new SqlParameter("@usertoken", SqlDbType.VarChar)).Value = strtoken;
                _command.Parameters.Add(new SqlParameter("@application", SqlDbType.VarChar)).Value = ApplicationCode;
                _command.Parameters.Add(new SqlParameter("@validDatetime", SqlDbType.DateTime)).Value = DateTime.Now;
                _command.Parameters.Add(new SqlParameter("@isValid", SqlDbType.Bit)).Value = status;
                _command.Parameters.Add(new SqlParameter("@Description", SqlDbType.VarChar)).Value = Description;

                using (System.Data.DataSet ds = new System.Data.DataSet())
                {
                    ds.Clear();
                    adp = new SqlDataAdapter(_command);
                    adp.Fill(ds);
                    connect.Close();

                    if (ds.Tables[0].Rows[0]["returnCode"].ToString() != "00")
                    {
                        TaxSmartSuite.Class.Common.setMessageBox(ds.Tables[0].Rows[0]["returnMessage"].ToString(), Program.ApplicationName, 2);

                        return false;

                    }
                    else
                    {
                        return true;
                    }

                }
            }
        }

        //public static bool IsPasswordStrengthCheck(string password, out PasswordScore passwordScore)
        //{
        //    var passwordStrengthScore = PasswordStrength.CheckStrength(password);
        //    passwordScore = passwordStrengthScore;
        //    bool bResponse;
        //    switch (passwordStrengthScore)
        //    {
        //        //case TaxSmartSuite.CommonLibrary.Security.Passwords.PasswordScore.Blank:
        //        //case TaxSmartSuite.CommonLibrary.Security.Passwords.PasswordScore.VeryWeak:
        //        //case TaxSmartSuite.CommonLibrary.Security.Passwords.PasswordScore.Weak:
        //        //    // Show an error message to the user
        //        //    bResponse = false;
        //        //    break;
        //        //case TaxSmartSuite.CommonLibrary.Security.Passwords.PasswordScore.Medium:
        //        case PasswordScore.Strong:
        //        case PasswordScore.VeryStrong:
        //            // Password deemed strong enough, allow user to be added to database etc
        //            bResponse = true;
        //            break;

        //        default:
        //            bResponse = false;
        //            break;
        //    }
        //    return bResponse;
        //}
    }

    /* •—————————————————————————————•
       | public enum ApplicationCode |
       | {                           |
       |     ControlPanel=1,         |
       |     Registration,           |
       |     Collection,             |
       |     Normalization           |
       | }                           |
       •—————————————————————————————• */


    public struct ApplicationRightCode
    {
        public const string Add = "001";

        public const string Edit = "002";

        public const string Delete = "003";

        public const string Reload = "004";

        public const string Close = "005";
    }

    public class Logic
    {
        private string _lastError = string.Empty;

        DataProvider DbProvider = DataProvider.SqlServer;

        static string _connString = null;
        static string _connString2 = null;

        AmountToWords amounttowords = new AmountToWords();
        public static bool LoadConfig()
        {
            bool response = false;
            try
            {
                response = TaxSmartConfiguration.ConfigManager.CheckDatabaseConnectionSettings(ref _connString, TaxSmartConfiguration.ConfigManager.GetServerName);
            }
            catch (Exception ex)
            {
                Tripous.Sys.ErrorBox(ex);
            }
            return response;
        }

        public static bool LoadConfig2()
        {
            bool response = false;

            string filePath = "C://TSS//TaxSmartSuiteNew//TaxSmartSuiteNew//Configuration.xml";

            try
            {
                //var config = new TaxSmartConfiguration.ConfigManager("C://TSS//TaxSmartSuiteNew", true);
                //config.CheckConnectionString();
                //var _connString2 = config.ConnectionString;
                ////var sqlBuilder = new SqlConnectionStringBuilder(_connString2 = config.ConnectionString);
                ////sqlBuilder.

                response = TaxSmartConfiguration.ConfigManager.CheckDatabaseConnectionSettings(ref _connString2, filePath, true);
            }
            catch (Exception ex)
            {
                Tripous.Sys.ErrorBox(ex);
            }
            return response;
        }

        public static string logopth
        {
            get
            {
                return "Logo//logo.png";
            }
        }

        public static string singaturepth
        {
            get
            {
                return "Logo//signature.png";
            }
        }

        public static void ConfigureSettings()
        {
            //TaxSmartSuiteRevised_Oyo
#if HOME
            if (Configuration2.getSetting("DBServerName") == string.Empty) Configuration2.writeSetting("DBServerName", @"LEOFEMMY-PC", false);
            if (Configuration2.getSetting("DBName") == string.Empty) Configuration2.writeSetting("DBName", "TaxSmartSuiteRevised_Oyo", false);
            if (Configuration2.getSetting("DBUsername") == string.Empty) Configuration2.writeSetting("DBUsername", "sa", false);
            if (Configuration2.getSetting("DBPassword") == string.Empty) Configuration2.writeSetting("DBPassword", "bond", true);
            if (Configuration2.getSetting("EncryptionKey") == string.Empty) Configuration2.writeSetting("EncryptionKey", "ReemsonlineKeyPass7676777", false);

#else
            if (Configuration2.getSetting("ServerName") == string.Empty) Configuration2.writeSetting("ServerName", @"Server", false);
            if (Configuration2.getSetting("DatabaseName") == string.Empty) Configuration2.writeSetting("DatabaseName", "TaxSmartSuiteRevised", false);
            if (Configuration2.getSetting("Username") == string.Empty) Configuration2.writeSetting("Username", "sa", false);
            var password = EncryptionConfiguration.EncryptString("1964", "moses");
            if (Configuration2.getSetting("Password") == string.Empty) Configuration2.writeSetting("Password", password, false);
            if (Configuration2.getSetting("EncryptionKey") == string.Empty) Configuration2.writeSetting("EncryptionKey", "ReemsonlineKeyPass7676777", false);
#endif
            Program.ServerName = Configuration2.getSetting("ServerName");

        }

        /// <summary>
        /// Build SQL Connection string 
        /// </summary>
        private static string BuildSQLConnectionString
        {
            get
            {

                var password = EncryptionConfiguration.DecryptString(Configuration2.getSetting("Password"), (string)new AppSettingsReader().GetValue("SecurityKey", typeof(String)));

                var builder = new SqlConnectionStringBuilder
                {
                    DataSource = Configuration2.getSetting("ServerName")
                    ,
                    UserID = Configuration2.getSetting("Username")
                    ,
                    Password = password// Configuration2.getSetting("DBPassword")
                    ,
                    InitialCatalog = Configuration2.getSetting("DatabaseName"),

                    ConnectTimeout = 0
                };

                String builderString = builder.ConnectionString;

                return builderString;
            }
        }

        /// <summary>
        /// Return generated Sql ConnectionString
        /// </summary>
        public static string ConnectionString
        {
            get
            {
                return _connString;
                //return BuildSQLConnectionString;
            }

        }

        public static string ConnectionString2
        {
            get
            {
                return _connString2;
                //return BuildSQLConnectionString;
            }

        }

        public static void LaunchApplication(string appPath, string args)
        {
            if (string.IsNullOrWhiteSpace(appPath)) return;
            if (!File.Exists(appPath))
            {
                Common.setMessageBox("Check Update Not availabale. Please contact administrator", Application.ProductName.ToString(), 2);
                return;
            }

            var proc = new System.Diagnostics.Process
            {
                StartInfo =
                {
                    FileName = appPath,
                    Verb = "runas",
                    UseShellExecute = true,
                    Arguments = args
                }
            };
            proc.Start();
        }

        public bool InsertUserApplicationModulesAccess2(int UserAppID, string ModulesCodes, string ModulesAccessCode, out int UserAccessID)
        {
            bool bResponse = false;

            //_lastError = string.Empty;
            //object UserAccessID = null;
            UserAccessID = 0;

            if (String.IsNullOrEmpty(ModulesCodes)
                | String.IsNullOrEmpty(ModulesAccessCode)
                )
                return false;

            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("spInsertUserModulesAccess", conn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    // input ModulesCodes
                    SqlParameter pmModulesCodes = cmd.Parameters.Add("@ModulesCodes", SqlDbType.VarChar, 50);
                    pmModulesCodes.Value = ModulesCodes;
                    // input ModulesAccessCode
                    SqlParameter pmModulesAccessCode = cmd.Parameters.Add("@ModulesAccessCode", SqlDbType.VarChar, 50);
                    pmModulesAccessCode.Value = ModulesAccessCode;
                    // input ModulesCodes
                    SqlParameter pmUserApplicationID = cmd.Parameters.Add("@UserApplicationID", SqlDbType.Int);
                    pmUserApplicationID.Value = UserAppID;
                    // output UserAccessID
                    SqlParameter pmUserAccessID = cmd.Parameters.Add("@UserAccessID", SqlDbType.Int);
                    pmUserAccessID.Direction = ParameterDirection.Output;

                    //// return value
                    //SqlParameter rowCount = cmd.Parameters.Add("@rowCount", SqlDbType.Int);
                    //rowCount.Direction = ParameterDirection.ReturnValue;
                    int count = cmd.ExecuteNonQuery();

                    if (count > 0)
                    {
                        bResponse = true;

                        UserAccessID = Convert.ToInt32(pmUserAccessID.Value);
                    }
                    else
                        bResponse = false;

                    bResponse = (count > 0) ? true : false;
                }
            }
            catch (Exception ex)
            {
                //Usual code
                bResponse = false;
                //_lastError = ex.Message;
                //DisplayExceptionBox(ex);
            }

            return bResponse;
        }

        public bool IsRecordExist(string SQL)
        {
            bool bResponse = false;
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(SQL, conn)
                    {
                        CommandType = CommandType.Text
                    };
                    int count = (int)cmd.ExecuteScalar();

                    bResponse = (count > 0) ? true : false;
                }
            }
            catch (Exception ex)
            {
                bResponse = false;

                Tripous.Sys.ErrorBox(ex);
            }
            return bResponse;
        }

        public Data.DataSet getSqlStatement(string SQL)
        {
            //bool bResponse = false;
            Data.DataSet Ds = new System.Data.DataSet();
            try
            {
                using (SqlDataAdapter da = new SqlDataAdapter(SQL, ConnectionString))
                {
                    da.Fill(Ds);
                }
            }
            catch (Exception ex)
            {
                //bResponse = false;
                Tripous.Sys.ErrorBox(ex);
            }
            return Ds;
        }

        public bool InsertNavModules(string ModulesCodes, string ModulesName, string ApplicationCode)
        {
            if (String.IsNullOrEmpty(ModulesCodes) || String.IsNullOrEmpty(ModulesName) || String.IsNullOrEmpty(ApplicationCode))
                return false;
            bool bResponse = false;
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("spApplicationModules", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                SqlParameter pmModulesCodes = cmd.Parameters.Add("@ModulesCodes", SqlDbType.VarChar, 50);
                pmModulesCodes.Value = ModulesCodes;

                SqlParameter pmModulesName = cmd.Parameters.Add("@ModulesName", SqlDbType.VarChar, 50);
                pmModulesName.Value = ModulesName;

                SqlParameter pmApplicationCode = cmd.Parameters.Add("@ApplicationCode", SqlDbType.VarChar, 50);
                pmApplicationCode.Value = ApplicationCode;

                int count = cmd.ExecuteNonQuery();
                bResponse = (count > 0) ? true : false;
            }

            return bResponse;
        }

        public bool InsertNavModulesAccess(string ModulesAccessCode, string ModulesAccessName, string ModulesCode)
        {
            if (String.IsNullOrEmpty(ModulesAccessCode) || String.IsNullOrEmpty(ModulesAccessName) || String.IsNullOrEmpty(ModulesCode))
                return false;

            bool bResponse = false;

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("spApplicationModulesAccess", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                SqlParameter pmModulesAccessCode = cmd.Parameters.Add("@ModulesAccessCode", SqlDbType.VarChar, 50);
                pmModulesAccessCode.Value = ModulesAccessCode;

                SqlParameter pmModulesAccessName = cmd.Parameters.Add("@ModulesAccessName", SqlDbType.VarChar, 50);
                pmModulesAccessName.Value = ModulesAccessName;

                SqlParameter pmModulesCode = cmd.Parameters.Add("@ModulesCode", SqlDbType.VarChar, 50);
                pmModulesCode.Value = ModulesCode;

                int count = cmd.ExecuteNonQuery();
                bResponse = (count > 0) ? true : false;
            }

            return bResponse;
        }

        public static bool IsNumber(string text)
        {
            Regex regex = new Regex(@"^[-+]?[0-9]*\.?[0-9]+$");
            return regex.IsMatch(text);
        }

        public static int PasswordExpiryPeriod()
        {
            int PasswordExpiryPeriods = 0;

            string SQL = string.Format("SELECT Value FROM Registration.tblLocalization WHERE Name='Password Expiry period'");

            DataTable Dts = (new Logic()).getSqlStatement(SQL).Tables[0];

            if (Dts != null)
            {
                PasswordExpiryPeriods = Convert.ToInt32(Dts.Rows[0]["Value"]);
            }
            return PasswordExpiryPeriods;
        }
        public static bool IsUserAccountExpired(string username)
        {
            bool Bresp = false;

            string SQL = string.Format("SELECT * FROM Login.tblUsers WHERE UserID='{0}'", username);

            DataTable Dtt = (new Logic()).getSqlStatement(SQL).Tables[0];

            DateTime LastResetDate;

            if (Dtt != null)
            {
                bool IsAccountExpired = Convert.ToBoolean(Dtt.Rows[0]["IsAccountExpired"]);

                if (Convert.IsDBNull(Dtt.Rows[0]["LastResetDate"]))
                {
                    Bresp = true;
                }
                else
                {
                    LastResetDate = Convert.ToDateTime(Dtt.Rows[0]["LastResetDate"]);

                    if (IsAccountExpired || LastResetDate == null)
                    {
                        Bresp = true;
                    }
                    else
                    {

                        int expiryPeriod = PasswordExpiryPeriod();

                        var timespan = DateTime.Today - LastResetDate;

                        var isExpired = timespan.Days > expiryPeriod;

                        Bresp = isExpired;
                    }
                }

            }

            return Bresp;

        }

        public static bool IsUserAccountSuspended(string username)
        {
            bool Bresp = false;


            string SQL = string.Format("SELECT IsAccountSuspended FROM Login.tblUsers WHERE UserID='{0}'", username);

            DataTable Dtt = (new Logic()).getSqlStatement(SQL).Tables[0];

            if (Dtt != null)
            {
                Bresp = Convert.ToBoolean(Dtt.Rows[0]["IsAccountSuspended"]);
            }

            return Bresp;
        }
        public static bool IsUserEmailValid(string username)
        {
            bool Bresp = false;

            string SQL = string.Format("SELECT EmailAddress FROM Login.tblUsers WHERE UserID='{0}'", username);

            DataTable Dtt = (new Logic()).getSqlStatement(SQL).Tables[0];

            if (Dtt != null)
            {
                //Bresp = Convert.ToBoolean(Dtt.Rows[0]["EmailAddress"]);
                Bresp = true;
            }

            return Bresp;

        }
        public static bool IsUserFirstLogin(string username)
        {
            bool Bresp = false;

            string SQL = string.Format("SELECT IsFirstLogin FROM Login.tblUsers WHERE UserID='{0}'", username);

            DataTable Dtt = (new Logic()).getSqlStatement(SQL).Tables[0];

            if (Dtt != null)
            {
                Bresp = Convert.ToBoolean(Dtt.Rows[0]["IsFirstLogin"]);
            }

            return Bresp;

            //return EntityModel.GetContext.TblUsers.Single(x => x.UserID.ToLower() == username).IsFirstLogin;
        }
        public string ExecuteScalar(string SQL)
        {
            string ResponseValue = "";
            IDBManager dbManager = new DBManager(DbProvider) { ConnectionString = ConnectionString };
            try
            {
                dbManager.Open();
                ResponseValue = Convert.ToString(dbManager.ExecuteScalar(CommandType.Text, SQL));
            }
            catch (Exception ex)
            {
                //Usual Code
                ResponseValue = "";
                _lastError = ex.Message;
                DisplayExceptionBox(ex);
            }
            finally
            {
                dbManager.Dispose();
            }

            return ResponseValue;
        }

        private static void DisplayExceptionBox(Exception ex)
        {
            Tripous.Sys.ErrorBox(ex);
        }

        public static bool CheckRnageValue(string ContNum)
        {
            bool bRes = false;

            string sql = String.Format("SELECT COUNT(ControlNumber) AS Count FROM Receipt.tblCollectionReceipt WHERE (ControlNumber = '{0}')", ContNum);

            if (new Logic().IsRecordExist(sql))
                //if (retval == "1")
                bRes = true;
            else
                bRes = false;
            return bRes;
        }

        public static bool CheckRangeValue4mTable(string numrange)
        {
            DataTable dt;

            bool bRes = false;

            using (System.Data.DataSet ds = new System.Data.DataSet())
            {
                string qry = string.Empty;

                if (Program.isCentralData)
                {
                    qry = string.Format("SELECT IssueFrom,IssueTo FROM Receipt.tblIssueReceipt WHERE IssueFrom <='{0}' AND IssueTo>='{0}'", numrange);
                }
                else
                {
                    qry = string.Format("SELECT IssueFrom,IssueTo FROM Receipt.tblIssueReceipt WHERE IssueFrom <='{0}' AND IssueTo>='{0}' and (StationCode = '{1}') ", numrange, Program.stationCode);
                }


                //using (SqlDataAdapter ada = new SqlDataAdapter(String.Format("SELECT IssueFrom,IssueTo,IssueQty FROM tblIssueReceipt WHERE (StationCode = '{0}')", Program.stationCode), Logic.ConnectionString))
                using (SqlDataAdapter ada = new SqlDataAdapter(qry, Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }


                dt = ds.Tables[0];

                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow Dr in dt.Rows)
                    {
                        if (string.IsNullOrEmpty(Dr["IssueFrom"].ToString()) && string.IsNullOrEmpty(Dr["IssueTo"].ToString()))
                        {
                            bRes = false;
                        }
                        else
                        {
                            bRes = true;
                        }
                    }

                    //foreach (DataRow Dr in dt.Rows)
                    //{
                    //    if (Enumerable.Range(Convert.ToInt32(Dr["IssueFrom"].ToString()), Convert.ToInt32(Dr["IssueQty"].ToString())).Contains(numrange))
                    //    {

                    //        bRes = true;
                    //        break;
                    //    }
                    //    else
                    //        bRes = false;

                    //}
                }

            }
            return bRes;

        }

        public static DialogResult InputBox(string title, string promptText, ref string value)
        {
            Form form = new Form();
            Label label = new Label();
            TextBox textBox = new TextBox();
            Button buttonOk = new Button();
            Button buttonCancel = new Button();

            form.Text = title;
            label.Text = promptText;
            textBox.Text = value;

            buttonOk.Text = "OK";
            buttonCancel.Text = "Cancel";
            buttonOk.DialogResult = DialogResult.OK;
            buttonCancel.DialogResult = DialogResult.Cancel;

            label.SetBounds(9, 20, 372, 13);
            textBox.SetBounds(12, 36, 372, 20);
            buttonOk.SetBounds(228, 72, 75, 23);
            buttonCancel.SetBounds(309, 72, 75, 23);

            label.AutoSize = true;
            textBox.Anchor = textBox.Anchor | AnchorStyles.Right;
            buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

            form.ClientSize = new Size(396, 107);
            form.Controls.AddRange(new Control[] { label, textBox, buttonOk, buttonCancel });
            form.ClientSize = new Size(Math.Max(300, label.Right + 10), form.ClientSize.Height);
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MinimizeBox = false;
            form.MaximizeBox = false;
            form.AcceptButton = buttonOk;
            form.CancelButton = buttonCancel;

            DialogResult dialogResult = form.ShowDialog();
            value = textBox.Text;
            return dialogResult;
        }

        public static bool isDeceimalFormat(string input)
        {
            Decimal dummy;

            return Decimal.TryParse(input, out dummy);
        }

        public static void ClearFormControl(Control control)
        {
            foreach (Control c in control.Controls)
            {
                if (c is TextBox)
                {
                    ((TextBox)c).Clear();
                }

                if (c.HasChildren)
                {
                    ClearFormControl(c);
                }


                if (c is CheckBox)
                {

                    ((CheckBox)c).Checked = false;
                }

                if (c is RadioButton)
                {
                    ((RadioButton)c).Checked = false;
                }

                if (c is ComboBox)
                {
                    ((ComboBox)c).SelectedIndex = -1;
                }
            }
        }

        //public static string GetMACAddress()
        //{
        //    NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();

        //    String sMacAddress = string.Empty;

        //    foreach (NetworkInterface adapter in nics)
        //    {
        //        if (sMacAddress == String.Empty)// only return MAC Address from first card  
        //        {
        //            IPInterfaceProperties properties = adapter.GetIPProperties();
        //            sMacAddress = adapter.GetPhysicalAddress().ToString();
        //        }
        //    }
        //    return sMacAddress;
        //}

        public static System.Data.DataSet GetMacAddress()
        {
            DataTable _dataTable = new DataTable("MAC");

            DataRow _dataRow;

            _dataTable.Columns.Add("MacAddress", typeof(string));

            //foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            //{
            //    if (nic.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
            //    {
            //        _dataRow = _dataTable.NewRow();
            //        _dataRow["MacAddress"] = nic.GetPhysicalAddress().ToString();
            //        _dataTable.Rows.Add(_dataRow);
            //    }
            //}
            //_dataRow = _dataTable.NewRow();
            //_dataRow["MacAddress"] = "F4EC38AE6DD3";
            //_dataTable.Rows.Add(_dataRow);
            //=====
            IPGlobalProperties computerProperties = IPGlobalProperties.GetIPGlobalProperties();
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();

            foreach (NetworkInterface adapter in nics)
            {
                //PhysicalAddress address = adapter.GetPhysicalAddress();

                _dataRow = _dataTable.NewRow();
                _dataRow["MacAddress"] = adapter.GetPhysicalAddress().ToString();
                _dataTable.Rows.Add(_dataRow);
            }
            //====
            //_dataRow = _dataTable.NewRow();
            //_dataRow["MacAddress"] = "F4EC38AE6DD3";//asaba mac address
            //_dataTable.Rows.Add(_dataRow);
            System.Data.DataSet ds = new System.Data.DataSet();
            ds.Tables.Add(_dataTable);
            return ds;
            //return _dataTable;
        }

        public static void ProcessDataTable(DataTable Dt)
        {
            AmountToWords amounttowords = new AmountToWords();

            if (Dt != null && Dt.Rows.Count > 0)
            {
                Dt.Columns.Add("URL", typeof(string));
                Dt.AcceptChanges();

                foreach (DataRow item in Dt.Rows)
                {
                    if (item == null) continue;
                    //decimal amount = decimal.Parse(item["Amount"].ToString());
                    try
                    {
                        item["AmountWords"] = string.Format("{0}{1:n2} - {2}", item["Symbol"].ToString(), Convert.ToDecimal(item["Amount"]), amounttowords.convertToWords(item["Amount"].ToString(), item["prefix"].ToString(), item["Surfix"].ToString()));

                        //item["AmountWords"] = amounttowords.convertToWords(item["Amount"].ToString(), item["prefix"].ToString(), item["Surfix"].ToString());

                        string stateCode = Program.stateCode;
                        if (!item["PayerID"].ToString().StartsWith(stateCode))
                        //if (item["PayerID"].ToString().Length > 14)
                        {
                            item["PayerID"] = "Approach the BIR for your Tax Identification Number.";
                        }
                        else
                        {item["PayerID"] = item["PayerID"].ToString();
                            item["PayerID"] = string.Format("Your Payer ID which is <<{0}>> must be quoted in all transaction", item["PayerID"]);
                        }

                        item["ZoneCode"] = item["StationCode"].ToString();

                        item["Description"] = item["Description"].ToString();
                        //item["URL"] = string.Format(@"Payment for {0} {1} << Paid at {2} - {3} , Deposit Slip Number {4} by {5}  >> ", item["Description"], item["RevenueCode"], item["BankName"], item["BranchName"], item["DepositSlipNumber"], item["PaymentMethod"]);

                        //item["URL"] = string.Format("Paid at {0} , {1}  Deposit Slip Number {2} in respect of {3} <{4}> By {5}", item["BankName"], item["BranchName"], item["DepositSlipNumber"], item["Description"], item["RevenueCode"], item["PaymentMethod"]);

                        switch (Program.intCode)
                        {
                            case 13://Akwa Ibom state
                                item["URL"] = string.Format("Paid at {0} , {1}  Deposit Slip Number {2} in respect of {3} By {4}", item["BankName"], item["BranchName"], item["DepositSlipNumber"], item["Description"], item["PaymentMethod"]);
                                item["ZoneName"] = item["StationName"].ToString();
                                //item["AgencyCode"] = string.Format("{0}/{1}", item["AgencyCode"], item["RevenueCode"]);
                                item["AgencyCode"] = string.Format("{0}", item["RevenueCode"]);

                                item["User"] = Program.UserID.ToUpper();

                                item["Username"] = string.Format(@"</Printed at {0} Zonal Office  by {1} on {2}/>", item["StationName"], Program.UserID, DateTime.Today.ToString("dd-MMM-yyyy"));

                                item["PaymentDate"] = Convert.ToDateTime(item["PaymentDate"]).ToString("dd-MM-yyyy");
                                break;

                            case 20://detla state
                                item["URL"] = string.Format("Payment for [{0}/{1}] paid at [{2}/{3}] Deposit Slip Number [{4}] by [{5}]", item["Description"], item["RevenueCode"], item["BankName"], item["BranchName"], item["DepositSlipNumber"], item["PaymentMethod"]);
                                item["ZoneName"] = item["ZoneName"].ToString();
                                //item["Channel"] = item["ControlNumber"].ToString();
                                //item["ControlNumber"] = item["ControlNumber"].ToString();

                                item["User"] = Program.UserID.ToUpper();

                                item["Username"] = string.Format(@"</Printed at BIR Headquarters on {0}/>", DateTime.Today.ToString("dd/MM/yyyy"));

                                item["PaymentDate"] = Convert.ToDateTime(item["PaymentDate"]).ToString("dd-MM-yyyy");
                                break;

                            case 37://ogun state
                                item["URL"] = string.Format("Paid at {0} , {1}  Deposit Slip Number {2} in respect of {3} <{4}> By {5}", item["BankName"], item["BranchName"], item["DepositSlipNumber"], item["Description"], item["RevenueCode"], item["PaymentMethod"]);
                                item["ZoneName"] = item["StationName"].ToString();

                                item["User"] = Program.UserID.ToUpper();

                                item["Username"] = string.Format(@"</Printed at {0} Zonal Office  by {1} on {2}/>", item["StationName"], Program.UserID, DateTime.Today.ToString("dd-MMM-yyyy"));

                                item["PaymentDate"] = Convert.ToDateTime(item["PaymentDate"]).ToString("dd-MM-yyyy");
                                break;

                            case 40://oyo state
                                item["URL"] = string.Format("Paid at {0} , {1}  Deposit Slip Number {2} in respect of {3} <{4}> By {5}", item["BankName"], item["BranchName"], item["DepositSlipNumber"], item["Description"], item["RevenueCode"], item["PaymentMethod"]);
                                item["ZoneName"] = item["StationName"].ToString();
                                break;

                            case 32://kogi state
                                item["URL"] = string.Format("Paid at {0} , {1}  Deposit Slip Number {2} in respect of {3} <{4}> By {5}", item["BankName"], item["BranchName"], item["DepositSlipNumber"], item["Description"], item["RevenueCode"], item["PaymentMethod"]);
                                item["ZoneName"] = item["StationName"].ToString();

                                item["User"] = Program.UserID.ToUpper();

                                item["Username"] = string.Format(@"</Printed at {0} Zonal Office  by {1} on {2}/>", item["StationName"], Program.UserID, DateTime.Today.ToString("dd-MMM-yyyy"));

                                item["PaymentDate"] = Convert.ToDateTime(item["PaymentDate"]).ToString("dd-MM-yyyy");
                                break;

                            default:
                                break;
                        }

                        //if (Program.stateCode == "20")
                        //{
                        //    if (Convert.ToInt32(radioGroup1.EditValue) != 1)
                        //    {
                        //        item["Username"] = string.Format(@"</Original control number {0} printed at {1} Office by {2} on {3} />", item["ControlNumber"], item["StationName"], item["PrintedBY"], Convert.ToDateTime(item["ControlNumberDate"]).ToString("dd-MMM-yyyy hh:mm:ss"));
                        //    }

                        //}

                    }
                    catch (Exception ex)
                    {
                        Tripous.Sys.ErrorBox(ex.Message); return;
                    }
                }
            }
        }

        public static bool CheckReceiptPrinted(string paymentrefnumber)
        {
            bool bRes = false;

            string sql = String.Format("SELECT COUNT(ControlNumber) AS Count FROM Receipt.tblCollectionReceipt WHERE (PaymentRefNumber = '{0}')", paymentrefnumber);

            if (new Logic().IsRecordExist(sql))
                //if (retval == "1")
                bRes = true;
            else
                bRes = false;
            return bRes;
        }

    }


}
