//#define HOME
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MosesClassLibrary.DataAccessLayer;
using MosesClassLibrary;
using System.Data.SqlClient;

namespace UserManager.Classess
{
    public class BusinessLogic
    {
        #region Declarations
        DataProvider DbProvider = DataProvider.SqlServer;
        /// <summary>
        /// store last error log
        /// </summary>
        private string _lastError = string.Empty;
        #endregion

        #region Settings and Common Methods
        /// <summary>
        /// Load Application Settings
        /// </summary>
        public static void ConfigureSettings()
        {
#if HOME
            if (Configuration2.getSetting("DBServerName") == string.Empty) Configuration2.writeSetting("DBServerName", @"MOSES-ICMA\SQLEXPRESS2008", false);
            if (Configuration2.getSetting("DBName") == string.Empty) Configuration2.writeSetting("DBName", "TaxSmartSuiteRevised", false);
            if (Configuration2.getSetting("DBUsername") == string.Empty) Configuration2.writeSetting("DBUsername", "sa", false);
            if (Configuration2.getSetting("DBPassword") == string.Empty) Configuration2.writeSetting("DBPassword", "moses", true);
            if (Configuration2.getSetting("EncryptionKey") == string.Empty) Configuration2.writeSetting("EncryptionKey", "ReemsonlineKeyPass7676777", false);

#else
            if (Configuration2.getSetting("DBServerName") == string.Empty) Configuration2.writeSetting("DBServerName", @"Server", false);
            if (Configuration2.getSetting("DBName") == string.Empty) Configuration2.writeSetting("DBName", "TaxSmartSuiteRevised", false);
            if (Configuration2.getSetting("DBUsername") == string.Empty) Configuration2.writeSetting("DBUsername", "sa", false);
            if (Configuration2.getSetting("DBPassword") == string.Empty) Configuration2.writeSetting("DBPassword", "icma", true);
            if (Configuration2.getSetting("EncryptionKey") == string.Empty) Configuration2.writeSetting("EncryptionKey", "ReemsonlineKeyPass7676777", false);
#endif
            Program.ServerName = Configuration2.getSetting("DBServerName");
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

        /// <summary>
        /// Return last generated sql error, if any otherwise null
        /// </summary>
        public string GetLastErrorMessage
        {
            get
            {
                return _lastError;
            }
        }
        /// <summary>
        /// Display Exceptional Error messaage in an Error Box
        /// </summary>
        /// <param name="ex">Exceptional object</param>
        private static void DisplayExceptionBox(Exception ex)
        {
            Tripous.Sys.ErrorBox(ex);
        }
        #endregion

        public bool LoadDataTable(string TableCode, string FilterCriteria, out DataTable Dt)
        {
            Dt = new DataTable("Details");
            bool bRet = false;
            IDBManager dbManager = new DBManager(DbProvider) { ConnectionString = ConnectionString };
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, "@Type", TableCode);
                dbManager.AddParameters(1, "@ID", FilterCriteria);
                DataSet Ds = dbManager.ExecuteDataSet(CommandType.StoredProcedure, "spGetDetails");
                Dt = Ds.Tables[0];
                if (Dt.Rows.Count > 0)
                    bRet = true;
            }
            catch (Exception ex)
            {
                //Usual Code
                bRet = false;
                _lastError = ex.Message;
                DisplayExceptionBox(ex);
            }
            finally
            {
                dbManager.Dispose();
            }

            return bRet;

        }

        public bool IsRecordExist(string SQL)
        {
            bool bRet = false;

            IDBManager dbManager = new DBManager(DbProvider) { ConnectionString = ConnectionString };
            try
            {
                dbManager.Open();
                int count = (int)dbManager.ExecuteScalar(CommandType.Text, SQL);
                bRet = (count > 0) ? true : false;
            }
            catch (Exception ce)
            {
                //Usual code
                bRet = false;
                _lastError = ce.Message;
                DisplayExceptionBox(ce);
            }
            finally
            {
                dbManager.Dispose();
            }

            return bRet;

        }

        public string ExecuteScalar(string SQL)
        {
            string ResponseValue = "";
            IDBManager dbManager = new DBManager(DbProvider) { ConnectionString = ConnectionString };
            try
            {
                dbManager.Open();
                ResponseValue = dbManager.ExecuteScalar(CommandType.Text, SQL).ToString();
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

        public DataSet getSqlStatement(string SQL)
        {
            var Ds = new DataSet();

            IDBManager dbManager = new DBManager(DbProvider) { ConnectionString = ConnectionString };

            if (string.IsNullOrEmpty(SQL))
                return null;
            try
            {
                dbManager.Open();
                Ds = dbManager.ExecuteDataSet(CommandType.Text, SQL);
            }
            catch (Exception ce)
            {
                //Usual code
                _lastError = ce.Message;
                DisplayExceptionBox(ce);
                Ds = null;
            }
            finally
            {
                dbManager.Dispose();
            }

            return Ds;

        }

        public bool InsertUser(string Username, string Password, string Surname
            , string Othernames, string UserType, string AppCode, bool boolIsUpdate)
        {
            bool bResponse = false;
            _lastError = string.Empty;
            if (String.IsNullOrEmpty(Username)
                | String.IsNullOrEmpty(Password)
                | String.IsNullOrEmpty(Surname)
                | String.IsNullOrEmpty(Othernames)
                | String.IsNullOrEmpty(UserType)
                | String.IsNullOrEmpty(AppCode)
                )
                return false;
            IDBManager dbManager = new DBManager(DbProvider) { ConnectionString = ConnectionString };
            try
            {
                int iCount = 0;
                dbManager.Open();
                dbManager.CreateParameters(7);
                dbManager.AddParameters(iCount, "@Username", (object)Username);
                dbManager.AddParameters(++iCount, "@Password", (object)Password);
                dbManager.AddParameters(++iCount, "@Surname", (object)Surname);
                dbManager.AddParameters(++iCount, "@Othernames", (object)Othernames);
                dbManager.AddParameters(++iCount, "@UserType", (object)UserType);
                dbManager.AddParameters(++iCount, "@ApplicationCode", (object)AppCode);
                dbManager.AddParameters(++iCount, "@boolIsUpdate", (object)boolIsUpdate);

                int count = dbManager.ExecuteNonQuery(CommandType.StoredProcedure, "spInsertUser");
                bResponse = (count > 0) ? true : false;
            }
            catch (Exception ex)
            {
                //Usual code
                bResponse = false;
                _lastError = ex.Message;
                DisplayExceptionBox(ex);
            }
            finally
            {
                dbManager.Dispose();
            }
            return bResponse;
        }

        public string GetUserApplicationID(string UserID, string ApplicationCode)
        {
            if (String.IsNullOrEmpty(UserID) | String.IsNullOrEmpty(ApplicationCode))
                return null;
            string SQL = String.Format("SELECT [UserApplicationID] FROM [dbo].[tblUsersApplication] WHERE ([UserID] = '{0}' AND [ApplicationCode] = '{1}')"
                , UserID, ApplicationCode);
            return ExecuteScalar(SQL);
        }

        public bool InsertUserApplicationModulesAccess(int UserAppID, string ModulesCodes, string ModulesAccessCode)
        {
            bool bResponse = false;
            _lastError = string.Empty;
            object UserAccessID = null;
            if (String.IsNullOrEmpty(ModulesCodes)
                | String.IsNullOrEmpty(ModulesAccessCode)
                )
                return false;
            
            IDBManager dbManager = new DBManager(DbProvider) { ConnectionString = ConnectionString };
            try
            {
                int iCount = 0;
                dbManager.Open();
                dbManager.CreateParameters(4);
                dbManager.AddParameters(iCount, "@ModulesCodes", (object)ModulesCodes);
                dbManager.AddParameters(++iCount, "@ModulesAccessCode", (object)ModulesAccessCode);
                dbManager.AddParameters(++iCount, "@UserApplicationID", (object)UserAppID);
                dbManager.AddParameters(++iCount, "@UserAccessID", (object)UserAccessID);

                int count = dbManager.ExecuteNonQuery(CommandType.StoredProcedure, "spInsertUserModulesAccess");
                bResponse = (count > 0) ? true : false;
            }
            catch (Exception ex)
            {
                //Usual code
                bResponse = false;
                _lastError = ex.Message;
                DisplayExceptionBox(ex);
            }
            finally
            {
                dbManager.Dispose();
            }

            return bResponse;
        }

        public bool InsertUserApplicationModulesAccess2(int UserAppID, string ModulesCodes, string ModulesAccessCode, out int UserAccessID)
        {
            bool bResponse = false;
            _lastError = string.Empty;
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
                _lastError = ex.Message;
                DisplayExceptionBox(ex);
            }

            return bResponse;
        }

        //public bool InsertUserAccessRight(int UserAccessID, int AppRightID)
        //{
        //    bool bResponse = false;
        //    _lastError = string.Empty;
        //    using (SqlConnection conn = new SqlConnection(ConnectionString))
        //    {
        //        conn.Open();
        //        SqlCommand cmd = new SqlCommand("spInsertUserModulesAccess", conn)
        //        {
        //            CommandType = CommandType.StoredProcedure
        //        };
        //        SqlParameter pmUserAccessID = cmd.Parameters.Add("@UserAccessID", UserAccessID);
        //        SqlParameter pmAppRightID = cmd.Parameters.Add("@AppRightID", AppRightID);
        //    }

        //    return bResponse;
        //}

        public bool InsertUserAccessRight(int UserAccessID, int AppRightID)
        {
            bool bResponse = false;
            _lastError = string.Empty;
            IDBManager dbManager = new DBManager(DbProvider) { ConnectionString = ConnectionString };
            try
            {
                int iCount = 0;
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(iCount, "@UserAccessID", (object)UserAccessID);
                dbManager.AddParameters(++iCount, "@UserAppRightID", (object)AppRightID);

                int count = dbManager.ExecuteNonQuery(CommandType.StoredProcedure, "spInsertUserApplicationRight");
                bResponse = (count > 0) ? true : false;
            }
            catch (Exception ex)
            {
                //Usual code
                bResponse = false;
                _lastError = ex.Message;
                DisplayExceptionBox(ex);
            }
            finally
            {
                dbManager.Dispose();
            }
            return bResponse;
        }

    }
}
