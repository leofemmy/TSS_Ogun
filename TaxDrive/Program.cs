using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using TaxDrive.Class;

namespace TaxDrive
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {

            if (!CheckDatabaseSettings)//check if there 
            {
                FrmTaxStation drive = new FrmTaxStation();
                drive.ShowDialog();
            }

            stateCode = getStateCode();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }

        public static string stateCode = string.Empty;

        public static string stateName = string.Empty;

        public static int intCode = 0;

        public static bool CheckDatabaseSettings
        {
            get
            {
                bool bResponse = false;

                string conn = Logic.ConfigureSettings();

                OleDbConnection db_con = new OleDbConnection(conn);

                String query = String.Format(
                              "select * from [{0}]", "tblStation");
                DataSet ds = new DataSet();

                db_con.Open();

                OleDbDataAdapter da =
                         new OleDbDataAdapter(query, conn);

                //Fill the DataSet
                da.Fill(ds, "tblStation");
                db_con.Close();

                if (ds.Tables[0].Rows.Count >= 1)
                {
                    bResponse = true;
                }
                else
                {
                    bResponse = false;
                }
                return bResponse;
            }
        }

        static string getStateCode()
        {
            OleDbConnection db_con;
            //get tax drive name

            string conn = Logic.ConfigureSettings();

            db_con = new OleDbConnection(conn);

            String query = String.Format(
                          "select * from [{0}] where Flag=True", "tblState");
            DataSet ds = new DataSet();

            db_con.Open();

            OleDbDataAdapter da =
                     new OleDbDataAdapter(query, conn);

            //Fill the DataSet
            da.Fill(ds, "tblState");
            db_con.Close();

            if (ds.Tables[0].Rows.Count >= 1)
            {
                stateCode = String.Format("{0}", ds.Tables[0].Rows[0]["StateCode"]);
                stateName = String.Format("{0}", ds.Tables[0].Rows[0]["StateName"]);
                intCode = Convert.ToInt32(stateCode);
            }

            return stateCode;
        }

    }
}
