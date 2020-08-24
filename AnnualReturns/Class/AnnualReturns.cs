using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Data = System.Data;

namespace AnnualReturns.Class
{
    class AnnualReturns
    {
    }

    public class Logic
    {
        private string _lastError = string.Empty;

        /// <summary>
        /// Build SQL Connection string 
        /// </summary>
        /// <summary>
        /// Return generated Sql ConnectionString
        /// </summary>



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

        public static Boolean IsNumeric(System.Object Expression)
        {
            if (Expression == null || Expression is DateTime)
                return false;

            if (Expression is Int16 || Expression is Int32 || Expression is Int64 || Expression is Decimal || Expression is Single || Expression is Double || Expression is Boolean)
                return true;

            try
            {
                if (Expression is string)
                    Double.Parse(Expression as string);
                else
                    Double.Parse(Expression.ToString());
                return true;
            }
            catch
            { } // just dismiss errors but return false
            return false;
        }

        public static bool IsDateTime(string txtDate)
        {
            DateTime tempDate;

            return DateTime.TryParse(txtDate, out tempDate) ? true : false;
        }

    }

}
