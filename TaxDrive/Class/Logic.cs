using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxDrive.Class
{
    public class Logic
    {

        public static string ConfigureSettings()
        {
            return System.Configuration.ConfigurationManager.ConnectionStrings["connectionString"].ToString();
        }
    }
}
