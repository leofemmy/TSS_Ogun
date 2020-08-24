//using Collection.Classess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Collection.Classes
{
   public static class Token
    {
        static string strtoken;

        static private SqlCommand _command;

        static private SqlDataAdapter adp;

        static DateTime dt = new DateTime();

        public static string GenerateToken()
        {
            //otp = Convert.ToString(rand.Next(10000, 1000000));
            Random RNG = new Random();

            //byte[] time = BitConverter.GetBytes(DateTime.UtcNow.ToBinary());
            //byte[] key = Guid.NewGuid().ToByteArray();
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
                    #region Oldsms

                    //SmsHelper sms = new SmsHelper("http://www.xpresspayonline.com/TestServers/IcmaSmsServer/", "446d8d01cd8d4d638767298273869e75",
                    //"gyIoXz+UlBZm8gkUCwR1yG/vpG38XqJ5ngNQZSN9Rb8=", Guid.NewGuid().ToString("N"));

                    //var response = sms.SendSms("XpressToken", Program.Userphone, strtoken);

                    //if (Convert.ToBoolean(response) == true)
                    //{
                    //    

                    //    return true;
                    //}
                    //else
                    //    return false;

                    //using (var receiptAka = new AkwaIbomReceiptServices.ReceiptService())
                    //{
                    //    dataSet = receiptAka.DownloadData(ds, Program.stateCode);
                    //}
                    #endregion

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

        public static bool tokenInsertValidation(string userid, string ApplicationCode, string strtoken, bool status, string Description)
        {
            using (SqlConnection connect = new SqlConnection(Classess.Logic.ConnectionString))
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

        public static bool CheckTokenperday()
        {
            bool respones = false;
            using (SqlConnection connect = new SqlConnection(Classess.Logic.ConnectionString))
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
    }
}
