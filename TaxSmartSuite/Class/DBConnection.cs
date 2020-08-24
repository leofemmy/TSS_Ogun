using System;
using System.Data.SqlClient ;

namespace TaxSmartSuite.Class
{
  public  class DBConnection
    {
        public SqlConnection connect;
        public string constring;
        //string gStatename, gUTINCode;
        //private SqlCommand lobjCommand;
        //SqlDataReader reader;
        public string ConnectionString()
        {
            //local connection
            constring = "server=FEMMY; User ID=sa; Password=bond; database=TaxSmartSuiteRevised; Max Pool Size=100";
            //Remote connection
            //constring = "server=SERVER; User ID=sa; Password=icma; database=TaxSmartSuiteRevised; Max Pool Size=100";

            connect = new SqlConnection(constring);
            //connect.Open();
            try
            {
                connect.Open();
            }
            catch (Exception)
            {

                throw;
            }
            return constring;
        }
    }
     


}
