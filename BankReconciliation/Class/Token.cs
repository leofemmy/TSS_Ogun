using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TaxSmartSuite.Class;

namespace BankReconciliation.Class
{
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

            //byte[] time = BitConverter.GetBytes(DateTime.UtcNow.ToBinary());
            //byte[] key = Guid.NewGuid().ToByteArray();
            string token = Convert.ToString(RNG.Next(10000, 1000000));

            return token;
        }

        public static bool dotoken()
        {
            bool bRespon = false;

            strtoken = Token.GenerateToken();

            if (Program.loginStatus && Program.tokenStatus)
            //if (CheckTokenperday())
            {
                TaxSmartSuite.Class.Common.setMessageBox("Please use current token received early on to authorized", "Token Request", 1);
                Program.loginid = 1;
                bRespon = false;
            }
            else
            {

                //if (tokenInsertValidation(Program.UserID, Program.ApplicationCode, strtoken, false, "New"))
                //{
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
                    var procesSms = new ProcessSms.ProcessSms();

                    string strprocessSme = procesSms.SendSms(Program.Userphone, "Token", strtoken);

                    if (strprocessSme.Contains("Failed"))
                    {
                        Tripous.Sys.ErrorBox(strprocessSme.ToString());

                        TaxSmartSuite.Class.Common.setMessageBox(strprocessSme.ToString(), "Get Token", 1);
                        Program.loginid = 0;
                        bRespon = false;
                    }
                    else
                    {
                        SendEmailToken(strtoken, Program.UserEmail);

                        if (tokenInsertValidation(Program.UserID, Program.ApplicationCode, strtoken, false, "New"))
                        {
                            TaxSmartSuite.Class.Common.setMessageBox(string.Format("Token Request sent to your registered number {0} and Email Address{1}.", $"********{Program.Userphone.Substring(7)}", $"********{Program.UserEmail.Substring(10)}"), "Token Request", 1);

                            dt = DateTime.Now;
                            Program.tokenStatus = true;
                            bRespon = true;
                        }

                    }

                }
                catch (Exception ex)
                {
                    Tripous.Sys.ErrorBox(ex.Message);
                    Program.loginid = 0;
                    bRespon = false;
                }


                //}
                ////else
                ////return false;
            }
            return bRespon;
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

        //public static bool validatetoken(string valtoken)
        //{
        //    //bool respones;

        //    //if (tokenInsertValidation(Program.UserID, Program.ApplicationCode, valtoken, true))
        //    //{
        //    //    respones = true;
        //    //}
        //    //else
        //    //{
        //    //    respones = false;
        //    //}

        //    //return respones;
        //}

        public static bool InitiatePasswordResetEmail(EmailModel emailModel)
        {
            bool bResponse = false;

            ResetPasswordService passwordService = new ResetPasswordService(Logic.EmailUrlAddress());

            var responseMsg = passwordService.ResetPassword(emailModel);

            if (responseMsg == null)
            {
                Common.setMessageBox("Error retrieving response from Email Service. Please contact administrator", Program.ApplicationName, 3);
                return false;
            }

            if (!responseMsg.ResponseStatus)
            {
                Common.setMessageBox(responseMsg.ResponseMsg, Program.ApplicationName, 3);
                return false;
            }
            return bResponse;
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

        public static void SendEmailToken(string strToken, string stremail)
        {

            //< add key = "fromEmail" value = "your@icmaservices.com" />

            //    < add key = "SMTPHost" value = "smtp.1and1.com" />

            //    < add key = "EnableSsl" value = "true" />

            //    < add key = "SMTPName" value = "Eazy Collects System" />

            //    < add key = "SMTPAddress" value = "youremail@icmaservices.com" />

            //    < add key = "SMTPPassword" value = "Password1234" />

            //    < add key = "SMTPSecure" value = "True" />

            //    < add key = "SMTPPort" value = "587" />

            //    < add key = "SMTPTimeout" value = "9999999" />

            //    < add key = "vs:EnableBrowserLink" value = "false" />

            try
            {
                var fromAddress = new MailAddress(System.Configuration.ConfigurationManager.AppSettings["fromAddress"].ToString(), "Bank Reconciliation Application");
                //var fromAddress = new MailAddress("taxsmart@ogunstaterevenue.com", "From Name");
                var toAddress = new MailAddress(stremail);
                const string fromPassword = "Password*1234";
                //const string fromPassword = "taxsmart";
                const string subject = "Token Request";
                const string body = "21563410";

                var smtp = new SmtpClient
                {
                    Host = System.Configuration.ConfigurationManager.AppSettings["Host"].ToString(),// "smtp.gmail.com",
                    //Host = "smtp.1and1.com",
                    Port = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["Port"]),
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    Credentials = new NetworkCredential(fromAddress.Address, System.Configuration.ConfigurationManager.AppSettings["fromPassword"].ToString()),
                    Timeout = 20000
                };
                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = strToken.ToString()
                })
                {
                    smtp.Send(message);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }


        }
    }


}
