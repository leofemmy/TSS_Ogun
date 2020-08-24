//#define HOME
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data;
using Data = System.Data;
using System.Collections;
using DevExpress.XtraNavBar;
using MosesClassLibrary;
using TaxSmartSuite.Class;


namespace AgencyControl.Class
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
                if (object.Equals(GroupList.Tag, "001-005"))

                    GroupList.Visible = true;

                else
                    GroupList.Visible = status;
                //if (GroupList.Tag == "002-004" as object) continue;
                //GroupList.Visible = status;
                foreach (NavBarItemLink ItemList in GroupList.ItemLinks)
                {
                    if ((ItemList.ItemName == "navBarItemExit") || (ItemList.ItemName == "navBarItemLogout")) continue;

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

        //private SqlCommand command;

        //private string retVal;
        public static void ConfigureSettings()
        {
#if HOME
            if (Configuration2.getSetting("DBServerName") == string.Empty) Configuration2.writeSetting("DBServerName", @"FEMI-PC\SQLEXPRESS", false);
            if (Configuration2.getSetting("DBName") == string.Empty) Configuration2.writeSetting("DBName", "TaxSmartSuiteRevised", false);
            if (Configuration2.getSetting("DBUsername") == string.Empty) Configuration2.writeSetting("DBUsername", "sa", false);
            if (Configuration2.getSetting("DBPassword") == string.Empty) Configuration2.writeSetting("DBPassword", "bond", true);
            if (Configuration2.getSetting("EncryptionKey") == string.Empty) Configuration2.writeSetting("EncryptionKey", "ReemsonlineKeyPass7676777", false);

#else
            if (Configuration2.getSetting("DBServerName") == string.Empty) Configuration2.writeSetting("DBServerName", @"Server", false);
            if (Configuration2.getSetting("DBName") == string.Empty) Configuration2.writeSetting("DBName", "TaxSmartSuiteRevised", false);
            if (Configuration2.getSetting("DBUsername") == string.Empty) Configuration2.writeSetting("DBUsername", "sa", false);
            if (Configuration2.getSetting("DBPassword") == string.Empty) Configuration2.writeSetting("DBPassword", "icma", true);
            if (Configuration2.getSetting("EncryptionKey") == string.Empty) Configuration2.writeSetting("EncryptionKey", "ReemsonlineKeyPass7676777", false);
#endif
            //Program.ServerName = Configuration2.getSetting("DBServerName");
            //Configuration.writeSetting("ConStr", BuildSQLConnectionString, false);
            //Common.EncryptionKey = Configuration.getSetting("EncryptionKey");
        }

        /// <summary>
        /// Build SQL Connection string 
        /// </summary>
        private static string BuildSQLConnectionString
        {
            get
            {

                var builder = new SqlConnectionStringBuilder
                {
                    DataSource = Configuration2.getSetting("DBServerName")
                    ,
                    UserID = Configuration2.getSetting("DBUsername")
                    ,
                    Password = Configuration2.getSetting("DBPassword")
                    ,
                    InitialCatalog = Configuration2.getSetting("DBName")
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
                return BuildSQLConnectionString;
            }

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

        public DataTable LoadData(string querytype)
        {
            DataTable db;

            using (Data.DataSet Ds = new System.Data.DataSet())
            {
                
                    using (SqlDataAdapter da = new SqlDataAdapter(querytype, ConnectionString))
                    {
                        da.Fill(Ds, "table");
                    }
                    db = Ds.Tables[0];
                    return db;
            }

            //return db;

        }

        public DataTable LoadData(string querytype, string TblName)
        {
            
            DataTable db = new DataTable(TblName);
            using (Data.DataSet ds = new System.Data.DataSet())
            {
                
                using (SqlDataAdapter da = new SqlDataAdapter(querytype, ConnectionString))
                {
                    da.Fill(ds, TblName);
                }

                db = ds.Tables[0];

                return db;
            }
        }

        //public string ExecuteCommand(string query)
        //{
        //    try
        //    {

        //        //command = new SqlCommand(query, ConnectionString);

        //        using (SqlDataAdapter adp = new SqlDataAdapter(query, ConnectionString))
        //        {
        //        }

        //        command.ExecuteNonQuery();

        //        retVal = "00";
        //    }
        //    catch (Exception ex)
        //    {
        //        retVal = String.Format("{0}---{1}----ExecuteCommand", ex.Message, ex.StackTrace);
        //    }
          
        //    return retVal;
        //}

    }



}
