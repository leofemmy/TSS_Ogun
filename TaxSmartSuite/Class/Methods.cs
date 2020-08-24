using System;
using System.Data.SqlClient;
using System.Data;


namespace TaxSmartSuite.Class
{
    public class Methods
    {
        private SqlCommand command;

        private SqlDataAdapter adp;

        private SqlDataReader reader;

        private string retVal;

        DBConnection connects = new DBConnection();

        public Methods()
        {
            connects.ConnectionString();
            
        }

        //public string getQuery(string type, string query)
        //{
        //    switch (type.ToUpper())
        //    {
        //        case "STATE":
        //            query = "select COUNT(*) as reccount from tblTaxOfficeSetup";
        //            retVal = ExecuteReader_withValue(query);
        //            break;

        //        case "LOCAL":
        //            query = "select COUNT(*) from tblRevenueOfficeSetUp";
        //            retVal = ExecuteReader_withValue(query);
        //            break;

        //        case "OFFICE":
        //            query = "select * from tblRevenueOfficeSetUp";
        //            retVal = ExecuteReader_withValue(query);
        //            break;

        //        case "AGN":
        //            query = "select COUNT(*) from Agency";
        //            retVal = ExecuteReader_withValue(query);
        //            break;

        //        case "STATECODE":
        //            //query = "select StateCode  from ViewSetupTaxOffice";
        //            query = "select StateCode from tblState where Flag = 1";
        //            retVal = ExecuteReader_withValue(query);
        //            break;

        //        case "CURRENCY":
        //            query = "select CurrencyName  from tblCurrency where Flag =1";
        //            retVal = ExecuteReader_withValue(query);
        //            break;

        //        case "STATECOUNT":
        //            query = "select count(*) from tblState ";
        //            retVal = ExecuteReader_withValue(query);
        //            break;

        //        case "TAXCODE":
        //            query = String.Format("select TaxCode  from tblState where StateCode = '{0}'", query);
        //            retVal = ExecuteReader_withValue(query);
        //            break;

        //        case "BANKCODE":
        //            retVal = ExecuteReader_withValue(query);
        //            break;

        //        case "INSERT":
        //            retVal = ExecuteCommand(query);
        //            break;

        //        default:
        //            break;
        //    }
        //    return retVal;
        //}
         //AutoComplete
        public static void AutoComplete(System.Windows.Forms.ComboBox cb, System.Windows.Forms.KeyPressEventArgs e)
        {
            AutoComplete(cb, e, false);
        }

        //public DataTable LoadData(string querytype)
        //{
        //    connects.connect.Close();
        //    DataTable db;
        //    using (var ds = new DataSet())
        //    {
        //        connects.connect.Open();
        //        using (SqlDataAdapter da = new SqlDataAdapter(querytype, connects.ConnectionString()))
        //        {
        //            da.Fill(ds, "table");
        //        }
        //        db = ds.Tables[0];
        //        return db;
        //    }

        //}

        //public string GetFildNo(string query)
        //{
        //    double FieldNo;
        //    double lngfiled;
        //   double lngcount;
        //   string strZero;
        //    try
        //    {
        //        //connectionString.connect.Close();
        //        connects.connect.Close();
        //        command = new SqlCommand(query, connects.connect);
        //        adp = new SqlDataAdapter(command);
        //        connects.connect.Open();
        //        reader = command.ExecuteReader();

        //        if (reader.Read())
        //        {
        //            retVal = Convert.ToString(reader.GetValue(0));

        //           FieldNo = Convert.ToInt32(retVal.ToString()) + 1;

        //           if (retVal.Length <= 3)
        //           {
        //               lngcount = 3 - retVal.Length;

        //               if (lngcount == 2)
        //               {
        //                   strZero = "00";
        //               }
        //               else if (lngcount == 1)
        //               {
        //                   strZero = "0";
        //               }

        //           }


        //           //retVal = String.Format("{0}{1}", strZero, FieldNo);
        //        }
        //        //else
        //        //{
        //        //    retVal = "-1";
        //        //}
        //    }
        //    catch (Exception ex)
        //    {
        //        retVal = String.Format("{0}---{1}----ExecuteCommand_withValue", ex.Message, ex.StackTrace);
        //    }
        //    finally
        //    {
        //        connects.connect.Close();
        //    }
        //    return retVal;
        //}


        //public DataTable LoadData(string querytype, string TblName)
        //{
        //    connects.connect.Close();
        //    DataTable db = new DataTable(TblName);
        //    using (var ds = new DataSet())
        //    {
        //        connects.connect.Open();
        //        using (SqlDataAdapter da = new SqlDataAdapter(querytype, connects.ConnectionString()))
        //        {
        //            da.Fill(ds, TblName);
        //        }
        //        db = ds.Tables[0];
        //        return db;
        //    }

        //}

        public static void AutoComplete(System.Windows.Forms.ComboBox cb, System.Windows.Forms.KeyPressEventArgs e, bool blnLimitToList)
        {
            string strFindStr = "";

            if (e.KeyChar == (char)8)
            {
                if (cb.SelectionStart <= 1)
                {
                    cb.Text = "";
                    return;
                }

                if (cb.SelectionLength == 0)
                    strFindStr = cb.Text.Substring(0, cb.Text.Length - 1);
                else
                    strFindStr = cb.Text.Substring(0, cb.SelectionStart - 1);
            }
            else
            {
                if (cb.SelectionLength == 0)
                    strFindStr = cb.Text + e.KeyChar;
                else
                    strFindStr = cb.Text.Substring(0, cb.SelectionStart) + e.KeyChar;
            }

            int intIdx = -1;

            // Search the string in the ComboBox list.

            intIdx = cb.FindString(strFindStr);

            if (intIdx != -1)
            {
                cb.SelectedText = "";
                cb.SelectedIndex = intIdx;
                cb.SelectionStart = strFindStr.Length;
                cb.SelectionLength = cb.Text.Length;
                e.Handled = true;
            }
            else
            {
                e.Handled = blnLimitToList;
            }
        }


        //public string ExecuteCommand(string query)
        //{
        //    try
        //    {
        //        connects.connect.Close();
        //        command = new SqlCommand(query, connects.connect);
        //        adp = new SqlDataAdapter(command);
        //        connects.connect.Open();
        //        command.ExecuteNonQuery();
        //        retVal = "00";
        //    }
        //    catch (Exception ex)
        //    {
        //        retVal = String.Format("{0}---{1}----ExecuteCommand", ex.Message, ex.StackTrace);
        //    }
        //    finally
        //    {
        //        connects.connect.Close();
        //    }
        //    return retVal;
        //}

        public static void  PopulateYear(System.Windows.Forms.ComboBox cb)
        {
            cb.DataSource = null;
            var item = new System.Collections.ArrayList();
            for (int i = 0; i < 5; i++)
            {
                item.Add(DateTime.Today.Year - 2 + i);
            }
            cb.DataSource = item;
            cb.SelectedItem = DateTime.Today.Year;
        }


        //public string ExecuteReader_withValue(string query)////////////execute and select a record to upload..
        //{
        //    try
        //    {
        //        //connectionString.connect.Close();
        //        connects.connect.Close();
        //        command = new SqlCommand(query, connects.connect);
        //        adp = new SqlDataAdapter(command);
        //        connects.connect.Open();
        //        reader = command.ExecuteReader();

        //        if (reader.Read())
        //        {
        //            retVal = Convert.ToString(reader.GetValue(0));
        //        }
        //        else
        //        {
        //            retVal = "-1";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        retVal = String.Format("{0}---{1}----ExecuteCommand_withValue", ex.Message, ex.StackTrace);
        //    }
        //    finally
        //    {
        //        connects.connect.Close();
        //    }
        //    return retVal;
        //}

        public static String generateRandomString(int length)
        {
            Random rand = new Random();
            //Initiate objects & vars    Random random = new Random();
            String randomString = "";
            int randNumber;

            //Loop ‘length’ times to generate a random number or character
            for (int i = 0; i < length; i++)
            {
                 randNumber = rand.Next(48, 58);// random.Next(48, 58); //int {0-9}

                //append random char or digit to random string
                randomString = randomString + (char)randNumber;
            }
            //return the random string
            return randomString;
        }

        public static void PopulateMonth(System.Windows.Forms.ComboBox cb)
        {
            DataTable Dt = new DataTable("Month");
            Dt.Columns.Add(new DataColumn("Display", typeof(string)));
            Dt.Columns.Add(new DataColumn("Value", typeof(Int32)));
            cb.DataSource = null;
            for (int i = 1; i <= 12; i++)
            {
                Dt.Rows.Add(new object[] { new System.DateTime(2010, i, 1).ToString("MMMM"), i });
            }
            cb.DataSource = Dt;
            cb.DisplayMember = "Display";
            cb.ValueMember = "Value";
        }

        /// <summary>
        /// Get the first day of the month for a
        /// month passed by it's integer value
        /// </summary>
        /// <param name="iMonth"></param>
        /// <returns></returns>
        public DateTime GetFirstDayOfMonth(int iMonth,int iYear)
        {
            // set return value to the last day of the month
            // for any date passed in to the method


            // create a datetime variable set to the passed in date
            //DateTime dtFrom = new DateTime(DateTime.Now.Year, iMonth, 1);
            DateTime dtFrom = new DateTime(iYear, iMonth, 1);


            // remove all of the days in the month
            // except the first day and set the
            // variable to hold that date
            dtFrom = dtFrom.AddDays(-(dtFrom.Day - 1));

            // return the first day of the month
            return dtFrom;
        }
        
        /// <summary>
        /// Get the last day of a month expressed by it's
        /// integer value
        /// </summary>
        /// <param name="iMonth"></param>
        /// <returns></returns>
        public DateTime GetLastDayOfMonth(int iMonth, int iYear)
        {

            // set return value to the last day of the month
            // for any date passed in to the method

            // create a datetime variable set to the passed in date
            DateTime dtTo = new DateTime(iYear, iMonth, 1);

            // overshoot the date by a month
            dtTo = dtTo.AddMonths(1);

            // remove all of the days in the next month
            // to get bumped down to the last day of the 
            // previous month
            dtTo = dtTo.AddDays(-(dtTo.Day));

            // return the last day of the month
            return dtTo;

        }



    }
}
