//#define HOME
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using DevExpress.XtraNavBar;
using System.Data.SqlClient;
using System.Data;
using Data = System.Data;
using MosesClassLibrary;
using System.Windows.Forms;
using TaxSmartSuite.Class;
using MosesClassLibrary.DataAccessLayer;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Configuration;
using System.IO;

namespace BankReconciliation.Class
{
    public class NavBars
    {
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
                if (object.Equals(GroupList.Tag, "011-005"))

                    GroupList.Visible = true;

                else
                    GroupList.Visible = status;
                //if (GroupList.Tag == "002-004" as object) continue;
                //GroupList.Visible = status;|| (ItemList.ItemName == "navBarItemCheck")|| (ItemList.ItemName == "navBarItemDataUpload")
                foreach (NavBarItemLink ItemList in GroupList.ItemLinks)
                {
                    if ((ItemList.ItemName == "navBarItemExit") || (ItemList.ItemName == "navBarItemLogout") || (ItemList.ItemName == "navBarItemHelp") || (ItemList.ItemName == "navBarItemCheck")) continue;

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

        public static bool ManagerUserApproval(NavBarControl NavBar)
        {
            if (NavBar == null) return false;

            string UserID = Program.UserID;

            string AppCode = Program.ApplicationCode;

            if (String.IsNullOrEmpty(UserID)) return false;

            int g = 0;

            int counts;

            //for (int i = 0; i < selection.SelectedCount; i++)
            //{
            //    values += String.Format("'{0}'", (selection.GetSelectedRow(i) as DataRowView)["PaymentRefNumber"]);
            //    if (j + 1 < selection.SelectedCount)
            //        values += ",";
            //    ++j;

            //    temTable.Rows.Add(new object[] { String.Format("{0}", (selection.GetSelectedRow(i) as DataRowView)["EReceipts"]), String.Format("{0}", (selection.GetSelectedRow(i) as DataRowView)["PaymentRefNumber"]), Program.UserID });

            //}

            bool bResponse = false;

            /* •———————————————————————————————•
               | Load User Approval Application Modules |
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
                            if ((Drr["ModuleAccessCode"].ToString() == "011-001-020") || (Drr["ModuleAccessCode"].ToString() == "011-001-005") || (Drr["ModuleAccessCode"].ToString() == "011-001-022"))
                            {
                                ++g;
                            }

                        }
                    }
                }
                if (g >= 1)
                    bResponse = true;
            }
            return bResponse;


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
                        if (String.Equals(toolStripButton.Tag, ApplicationRightCode.Reload)) continue;

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

    }

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
        private string _lastError = string.Empty; static string _connString = null;

        DataProvider DbProvider = DataProvider.SqlServer;

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

        public static void ConfigureSettings()
        {
#if HOME
            //
            //FEMI-PC\SQLEXPRESS
            if (Configuration2.getSetting("DBServerName") == string.Empty) Configuration2.writeSetting("DBServerName", @"FEMMY-HP\SQLEXPRESS", false);
            if (Configuration2.getSetting("DBName") == string.Empty) Configuration2.writeSetting("DBName", "TaxSmartSuiteRevised", false);
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

        public static string EmailUrlAddress()
        {
            string EmailUrlAddress = string.Empty;

            string SQL = string.Format("SELECT Value FROM Registration.tblLocalization WHERE Name='Email Service Url'");

            DataTable Dts = (new Logic()).getSqlStatement(SQL).Tables[0];

            if (Dts != null)
            {
                EmailUrlAddress = Dts.Rows[0]["Value"].ToString();
            }
            return EmailUrlAddress;
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
                        //cmd.CommandTimeout = 15;
                        //CommandTimeout=500;
                        CommandType = CommandType.Text,
                        CommandTimeout = 0


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

        public object ExecuteScalar(string SQL)
        {

            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(SQL, conn)
                    {
                        //cmd.CommandTimeout = 15;
                        //CommandTimeout=500;
                        CommandType = CommandType.Text,
                        CommandTimeout = 0

                    };
                    object rec = cmd.ExecuteScalar();

                    return rec;
                }
            }
            catch (Exception ex)
            {


                Tripous.Sys.ErrorBox(ex);
            }
            return null;
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
                    CommandType = CommandType.StoredProcedure,
                    CommandTimeout = 0
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
                    CommandType = CommandType.StoredProcedure,
                    CommandTimeout = 0
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

        //public string ExecuteScalar(string SQL)
        //{
        //    string ResponseValue = "";
        //    IDBManager dbManager = new DBManager(DbProvider) { ConnectionString = ConnectionString };
        //    try
        //    {
        //        dbManager.Open();
        //        ResponseValue = Convert.ToString(dbManager.ExecuteScalar(CommandType.Text, SQL));
        //    }
        //    catch (Exception ex)
        //    {
        //        //Usual Code
        //        ResponseValue = "";
        //        _lastError = ex.Message;
        //        DisplayExceptionBox(ex);
        //    }
        //    finally
        //    {
        //        dbManager.Dispose();
        //    }

        //    return ResponseValue;
        //}

        private static void DisplayExceptionBox(Exception ex)
        {
            Tripous.Sys.ErrorBox(ex);
        }

        public static bool IsNumber(string text)
        {
            //^[\-\+\s]*[0-9\s]+$

            Regex regex = new Regex(@"-?0*((90(\.0*)?)|([1-8]?\d(\.\d*)?))");

            //Regex.IsMatch(text, @"^[\-\+]\s*\d+\s*$");

            int dumy;

            return int.TryParse(text, out dumy);
            //return regex.IsMatch(text);
        }

        public static bool IsAlphabte(string text)
        {
            //Regex regex = new Regex(@"^[a-zA-Z]");
            Regex regex = new Regex("[a-zA-Z][a-zA-Z ]+");
            return regex.IsMatch(text);
        }

        public static bool IsAlphaNum(string str)
        {
            if (string.IsNullOrEmpty(str))
                return false;

            for (int i = 0; i < str.Length; i++)
            {
                if (!(char.IsLetter(str[i])) && (!(char.IsNumber(str[i]))))
                    return false;
            }

            return true;
        }

        public static Boolean isAlphaNumeric(string strToCheck)
        {
            Regex rg = new Regex(@"^[a-zA-Z0-9\/s,]*$");
            return rg.IsMatch(strToCheck);
        }

        public static bool isDeceimalFormat(string input)
        {
            decimal dummy;

            return decimal.TryParse(input, NumberStyles.Number, CultureInfo.GetCultureInfo("en-US").NumberFormat, out dummy);
        }
        public static bool varDeceimal(string input)
        {
            var text = input;
            return Regex.IsMatch(text, @"\d{1,2}(\.\d{1,2})?$");
            //return;
        }


    }

    public class GetReceiptNumber
    {
        public static string ReceiptNo()
        {
            int countrec;

            string recep;

            DataTable Dt = (new Logic()).getSqlStatement(String.Format("SELECT COUNT(*) FROM dbo.tblCollectionReport WHERE YEAR(PaymentDate)=YEAR('{0}')", DateTime.Now.Year)).Tables[0];

            if (Dt != null && Dt.Rows.Count > 0)
                countrec = Convert.ToInt32(Dt.Rows[0][0]) + 1;
            else
                countrec = 0;

            //recep = "ICM" + "|" + bankCode + "|" + Program.StatePayCode
            //    + "|" + DateTime.Now.ToString("dd-MM-yyyy") +"|" +Convert.ToString(countrec).PadLeft(7,'0');
            recep = Convert.ToString(countrec);

            return recep;
        }

        public static DataTable GetAgencyReveue(string Paramter)
        {
            string qurty = String.Format("SELECT * FROM ViewAgencyRevenueType WHERE RevenueCode='{0}'", Paramter);
            DataTable Dt = (new Logic()).getSqlStatement(qurty).Tables[0];

            if (Dt != null && Dt.Rows.Count > 0)
            {
                return Dt;
            }
            Dt = null;
            //countrec = Convert.ToInt32(Dt.Rows[0][0]) + 1;

            return Dt;
        }
    }

}
