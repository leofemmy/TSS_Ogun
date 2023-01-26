using System;
using System.Data.SqlClient;

namespace TaxSmartSuite.Class
{
    class Transaction
    {
        private DBConnection connection = new DBConnection();

        public Transaction()
        {
            connection.ConnectionString();
        }

        public void Transacts()
        {
            SqlConnection db = new SqlConnection(connection.ConnectionString());

            SqlTransaction transaction;

            db.Open();

            transaction = db.BeginTransaction();


            try
            {
                int i = new SqlCommand("INSERT INTO TransactionDemo " +
                            "(Text) VALUES ('Row1');", db, transaction)
                            .ExecuteNonQuery();

                new SqlCommand("INSERT INTO TransactionDemo " +
                   "(Text) VALUES ('Row2');", db, transaction)
                   .ExecuteNonQuery();

                new SqlCommand("INSERT INTO CrashMeNow VALUES " +
                   "('Die', 'Die', 'Die');", db, transaction)
                   .ExecuteNonQuery();

                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                Common.setMessageBox(ex.StackTrace, "Testing ", 2);
            }

            db.Close();

            //


        }
    }
}
