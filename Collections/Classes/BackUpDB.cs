using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Data.SqlClient;
using Collection.Classess;

public class BackUpDB
{
    public BackUpDB()
    {

    }

    public string doBackUp(string db)
    {
        string retVal;
        try
        {
            Cursor.Current = Cursors.WaitCursor;
            if (Directory.Exists(@"c:\TaxSmartBackUp"))
            {
                SqlConnection connects;
                //string con = String.Format("Data Source = .\\\SQLEXPRESS; Initial Catalog={0} ;Integrated Security = True;", db);
                //string con = ; //String.Format("Data Source = .\\\SQLEXPRESS; Initial Catalog={0} ;Integrated Security = True;", db);
                connects = new SqlConnection(Logic.ConnectionString);
                connects.Open();
                //Execute SQL--------------- 
                SqlCommand command;
                command = new SqlCommand(
                    string.Format(
                        @"backup database {4} to disk ='c:\TaxSmartBackUp\{0}_{1}_{2}_{3}.bak' with init,stats=10",
                        DateTime.Now.ToLongDateString(), DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second,db), connects);
                command.ExecuteNonQuery();
                //------------------------------------------------------------------------------------------------------------------------------- 
                connects.Close();       
                retVal="Done";
                //MessageBox.Show(@"The database was successfully performed", @"Back", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                //create directory
                Directory.CreateDirectory(@"c:\TaxSmartBackUp");
                SqlConnection connects;
                //string con = "Data Source = .; Initial Catalog=REEMSONLINE ;Integrated Security = True;";
                //connect = new SqlConnection(con);
                connects = new SqlConnection(Logic.ConnectionString);
                connects.Open();
                //Execute SQL--------------- 
                SqlCommand command;
                command = new SqlCommand(
                     string.Format(
                         @"backup database {4} to disk ='c:\TaxSmartBackUp\{0}_{1}_{2}_{3}.bak' with init,stats=10",
                         DateTime.Now.ToLongDateString(), DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second, db), connects);
                command.ExecuteNonQuery();
                //------------------------------------------------------------------------------------------------------------------------------- 
                connects.Close();
                retVal = "Done";
                //MessageBox.Show(@"The database was successfully performed", @"Back", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        catch (Exception ex)
        {

           // MessageBox.Show(string.Format("{0}-----{1}", ex.Message, ex.StackTrace), @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            retVal = string.Format("{0}-----{1}", ex.Message, ex.StackTrace);
        }
        return retVal;
    }
}