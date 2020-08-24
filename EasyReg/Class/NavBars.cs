#define HOME
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.XtraNavBar;
using System.Data.SqlClient;
using System.Data;
using MosesClassLibrary;

namespace EasyReg.Class
{
    public class NavBars
    {
        //public static void ManageNavBarControls(NavBarControl NavBar, string AppCode)
        //{
        //    var Blogic = new Logic();

        //    foreach (NavBarGroup GroupList in NavBar.Groups)
        //    {
        //        if (Blogic.InsertNavModules(GroupList.Tag as string, GroupList.Caption, AppCode))
        //        {
        //            foreach (NavBarItemLink ItemList in GroupList.ItemLinks)
        //            {
        //                NavBarItem items = ItemList.Item;

        //                if (!Blogic.InsertNavModulesAccess(items.Tag as string, items.Caption, GroupList.Tag as string))
        //                {
        //                    Common.setMessageBox("Error Updating Application Modules Access", Program.ApplicationName, 3);

        //                    break;
        //                }
        //            }
        //        }
        //        else
        //        {
        //            //if (String.IsNullOrEmpty(Blogic.GetLastErrorMessage))
        //            //{
        //            //    Common.setMessageBox("Error Updating Application Modules", Program.ApplicationName, 3);
        //            //    break;
        //            //}
        //        }
        //    }
        //}

        public static void NavBarEnableDisableControls(NavBarControl NavBar, bool status)
        {
            if (NavBar == null) return;

            foreach (NavBarGroup GroupList in NavBar.Groups)
            {
                if (object.Equals(GroupList.Tag, "001-003"))

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

    }
    public class Logic
    {
        public static void ConfigureSettings()
        {
#if HOME
            //
            //FEMI-PC\SQLEXPRESS
            if (Configuration2.getSetting("DBServerName") == string.Empty) Configuration2.writeSetting("DBServerName", @"OLUWAFEMI-PC\SQLEXPRESS", false);
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

        public System.Data.DataSet getSqlStatement(string SQL)
        {
            //bool bResponse = false;
            System.Data.DataSet Ds = new System.Data.DataSet();

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

    }

}
