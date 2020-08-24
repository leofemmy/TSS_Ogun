using DevExpress.Utils;
using DevExpress.XtraSplashScreen;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TaxDrive.Class;
using DevExpress.XtraReports.UI;
using DevExpress.XtraReports.Parameters;

namespace TaxDrive
{
    public partial class Form1 : Form
    {
        OleDbConnection db_con;

        string strname = string.Empty;

        DataSet dss = new DataSet();

        DataSet dsreturn = new DataSet();

        string strStatecode = string.Empty;

        string strTin = string.Empty;

        string criteria = string.Empty;
        public Form1()
        {
            SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

            InitializeComponent();

            radioGroup1.SelectedIndexChanged += radioGroup1_SelectedIndexChanged;

            getDriveName();

            sbnUpdate.Click += simpleButton1_Click;

            sbClose.Click += sbClose_Click; sbnReport.Click += sbnReport_Click;

            SplashScreenManager.CloseForm(false);
        }

        void sbnReport_Click(object sender, EventArgs e)
        {
            SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

            if ((Int32)this.radioGroup1.EditValue == 1)
            {
                criteria = String.Format("SELECT PayerName, PaymentDate, Amount, PaymentMethod, AgencyName, Description, BankName FROM tblCollectionReport WHERE PaymentRefNumber LIKE '%{0}%'", txtsearch.EditValue);

            }
            else if ((Int32)this.radioGroup1.EditValue == 2)
            {
                criteria = String.Format("SELECT PayerName, PaymentDate, Amount, PaymentMethod, AgencyName, Description, BankName FROM tblCollectionReport WHERE (((tblCollectionReport.PaymentDate)>=#{0}# And (tblCollectionReport.PaymentDate)<=#{1}# ))", string.Format("{0:M/dd/yyyy}", dtpDate.Value), string.Format("{0:M/dd/yyyy}", dtpDate2.Value));
            }
            else if ((Int32)this.radioGroup1.EditValue == 3)
            {
                criteria = String.Format("SELECT PayerName, PaymentDate, Amount, PaymentMethod, AgencyName, Description, BankName FROM tblCollectionReport WHERE PayerName LIKE '%{0}%'", txtsearch.EditValue);
            }

            string conn = Logic.ConfigureSettings();

            db_con = new OleDbConnection(conn);

            DataSet ds = new DataSet();

            db_con.Open();

            OleDbDataAdapter da =
                     new OleDbDataAdapter(criteria, conn);

            //Fill the DataSet

            da.Fill(ds, "tbluser");

            db_con.Close();

            if (ds.Tables[0].Rows.Count >= 1)
            {
                XtraReport1 taxReport = new XtraReport1();
                var replist = (from DataRow row in ds.Tables[0].Rows
                               select new Class.Report
                               {
                                   Amount = Convert.ToDecimal(row["Amount"]),
                                   PaymentDate = Convert.ToDateTime(row["PaymentDate"]),
                                   AgencyName = row["BankName"] as string,
                                   PayerName = row["BankName"] as string,
                                   PaymentMethod = row["PaymentMethod"] as string,
                                   Description = row["Description"] as string,
                                   BankName = row["BankName"] as string,
                               }).ToList();
                //var objectDataSource1 = (objectDataSource1)taxReport.DataSource;
                ////var binding = (BindingSource)taxReport.DataSource;

                //binding.Clear();

                //binding.DataSource = replist;
                taxReport.DataSource = replist;

                taxReport.ShowPreviewDialog();
            }
            SplashScreenManager.CloseForm(false);
        }

        void sbClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        void simpleButton1_Click(object sender, EventArgs e)
        {
            SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

            //if (txtsearch.EditValue == null) return;
            if ((Int32)this.radioGroup1.EditValue == 1)
            {
                criteria = String.Format("SELECT PayerName, PaymentDate, Amount, PaymentMethod, AgencyName, Description, BankName FROM tblCollectionReport WHERE PaymentRefNumber LIKE '%{0}%'", txtsearch.EditValue);

            }
            else if ((Int32)this.radioGroup1.EditValue == 2)
            {
                criteria = String.Format("SELECT PayerName, PaymentDate, Amount, PaymentMethod, AgencyName, Description, BankName FROM tblCollectionReport WHERE (((tblCollectionReport.PaymentDate)>=#{0}# And (tblCollectionReport.PaymentDate)<=#{1}# ))", string.Format("{0:M/dd/yyyy}", dtpDate.Value), string.Format("{0:M/dd/yyyy}", dtpDate2.Value));
            }
            else if ((Int32)this.radioGroup1.EditValue == 3)
            {
                criteria = String.Format("SELECT PayerName, PaymentDate, Amount, PaymentMethod, AgencyName, Description, BankName FROM tblCollectionReport WHERE PayerName LIKE '%{0}%'", txtsearch.EditValue);
            }

            string conn = Logic.ConfigureSettings();

            db_con = new OleDbConnection(conn);

            DataSet ds = new DataSet();

            db_con.Open();

            OleDbDataAdapter da =
                     new OleDbDataAdapter(criteria, conn);

            //Fill the DataSet

            da.Fill(ds, "tbluser");

            db_con.Close();

            if (ds.Tables[0].Rows.Count >= 1)
            {
                gridControl1.DataSource = ds.Tables[0];
                gridView1.Columns["PaymentDate"].DisplayFormat.FormatType = FormatType.DateTime;
                gridView1.Columns["PaymentDate"].DisplayFormat.FormatString = "dd/MM/yyyy";
                gridView1.Columns["Amount"].DisplayFormat.FormatType = FormatType.Numeric;
                gridView1.Columns["Amount"].DisplayFormat.FormatString = "n2";
                //gridView1.BestFitColumns();
            }

            SplashScreenManager.CloseForm(false);
        }

        void radioGroup1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (radioGroup1.EditValue == null) return;
            if ((Int32)this.radioGroup1.EditValue == 1)
            {
                dtpDate.Visible = false; txtsearch.Visible = true;
                labelControl2.Visible = true;
                this.labelControl1.Text = "Payment Ref. Number";
                labelControl2.Text = "(e.g LMFB|OGPDIPAY|0001|19-9-2013|143239 )";

            }
            else if ((Int32)this.radioGroup1.EditValue == 2)
            {
                dtpDate.Visible = true; dtpDate2.Visible = true;
                txtsearch.Visible = false;
                labelControl2.Visible = true; labelControl3.Visible = true;
                this.labelControl1.Text = "Payment Date between";
                labelControl3.Text = "and";
                labelControl2.Text = "(e.g MM/DD/YYYY )";
            }
            else if ((Int32)this.radioGroup1.EditValue == 3)
            {
                txtsearch.Visible = true;
                dtpDate.Visible = false;
                this.labelControl1.Text = "Payer Name";
                labelControl2.Visible = false;
            }
        }
        void sbUpdate_Click(object sender, EventArgs e)
        {


            SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

            //strname

            switch (Program.intCode)
            {
                case 13://Akwa Ibom state

                    break;

                case 32://kogi state
                    break;

                case 37://ogun state

                    using (var DriveCollection = new CollectionManager.CollectionManager())
                    {
                        dss = DriveCollection.CmuCollectionDownload(strTin);
                    }
                    break;

                case 40://oyo state

                    break;

                default:
                    break;
            }


            if (dss.Tables.Count == 0)
            {
                //looping through the return dataset

                foreach (DataRow dr in dss.Tables[0].Rows)
                {
                    //get reccord from the dataset

                    string payref = dr["PaymentRefNumber"].ToString();

                    //check it paymentref exit before in the table
                    string conn = Logic.ConfigureSettings();

                    db_con = new OleDbConnection(conn);

                    String query = String.Format("Select * from tblCollectionReport where PaymentRefNumber = '{0}' ", payref);

                    DataSet ds = new DataSet();

                    db_con.Open();

                    OleDbDataAdapter da =
                             new OleDbDataAdapter(query, conn);

                    //Fill the DataSet
                    da.Fill(ds, "tblCollectionReport");
                    db_con.Close();

                    if (ds.Tables[0].Rows.Count >= 1)
                    {
                        
                        //record exits den do update
                        string query1 = String.Format("UPDATE tblCollectionReport SET [Provider] = '{0}',[Channel] = '{1}',[DepositSlipNumber] = '{2}' ,[PaymentDate] ='{3:MM/dd/yyyy HH:mm:ss tt}',[PayerID] = '{4}',[PayerName] = '{5}',[Amount] = '{6}',[PaymentMethod] = '{7}',[ChequeNumber] ='{8}',[ChequeValueDate] = '{9:MM/dd/yyyy HH:mm:ss tt}',[ChequeStatus] = '{10}',[DateChequeReturned] = '{11:MM/dd/yyyy HH:mm:ss tt}',[TelephoneNumber] = '{12}',[ReceiptNo] = '{13}',[ReceiptDate] = '{14:MM/dd/yyyy HH:mm:ss tt}',[PayerAddress] = '{15}',[User] ='{16}',[RevenueCode] ='{17}',[Description] = '{18}',[ChequeBankCode] ='{19}',[ChequeBankName] = '{20}',[AgencyName] = '{21}',[AgencyCode] = '{22}',[BankCode] = '{23}',[BankName] = '{24}',[BranchCode] = '{25}',[BranchName] = '{26}',[ZoneCode] = '{27}',[ZoneName] = '{27}',[Username] = '{28}',[AmountWords] = '{30}',[EReceipts] = '{31}' ,[EReceiptsDate] = '{32:MM/dd/yyyy HH:mm:ss tt}',[GeneratedBy] = '{33}' ,[StationCode] = '{34}',[StationName] = '{35}'  WHERE PaymentRefNumber = '{36}'", dr["Provider"], dr["Channel"], dr["DepositSlipNumber"], dr["PaymentDate"], dr["PayerID"], dr["PayerName"], dr["Amount"], dr["PaymentMethod"], dr["ChequeNumber"], dr["ChequeValueDate"], dr["ChequeStatus"], dr["DateChequeReturned"], dr["TelephoneNumber"], dr["ReceiptNo"], dr["ReceiptDate"], dr["PayerAddress"], dr["User"], dr["RevenueCode"], dr["Description"], dr["ChequeBankCode"], dr["ChequeBankName"], dr["AgencyName"], dr["AgencyCode"], dr["BankCode"], dr["BankName"], dr["BranchCode"], dr["BranchName"], dr["ZoneCode"], dr["ZoneName"], dr["Username"], dr["AmountWords"], dr["EReceipts"], dr["EReceiptsDate"], dr["GeneratedBy"], dr["StationCode"], dr["StationName"], dr["PaymentRefNumber"]);

                        //string conn = Logic.ConfigureSettings();

                        db_con = new OleDbConnection(conn);

                        OleDbCommand cmd = new OleDbCommand(query1, db_con);
                        cmd.ExecuteNonQuery();

                    }
                    else
                    {
                        //insert update
                        string qury = string.Format("Insert into tblCollectionReport ([Provider],[Channel] ,[DepositSlipNumber]  ,[PaymentDate] ,[PayerID] ,[PayerName] ,[Amount] ,[PaymentMethod] ,[ChequeNumber],[ChequeValueDate] ,[ChequeStatus],[DateChequeReturned] ,[TelephoneNumber] ,[ReceiptNo],[ReceiptDate],[PayerAddress] ,[User] ,[RevenueCode],[Description] ,[ChequeBankCode],[ChequeBankName],[AgencyName],[AgencyCode],[BankCode],[BankName],[BranchCode] ,[BranchName] ,[ZoneCode],[ZoneName],[Username],[AmountWords],[EReceipts] ,[EReceiptsDate] ,[GeneratedBy] ,[StationCode],[StationName,PaymentRefNumber])VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}','{19}','{20}','{21}','{22}','{23}','{24}','{25}','{26}','{27}','{28}','{29}','{30}','{31}','{32}','{33}','{34}','{35}','{36}')", dr["Provider"], dr["Channel"], dr["DepositSlipNumber"], dr["PaymentDate"], dr["PayerID"], dr["PayerName"], dr["Amount"], dr["PaymentMethod"], dr["ChequeNumber"], dr["ChequeValueDate"], dr["ChequeStatus"], dr["DateChequeReturned"], dr["TelephoneNumber"], dr["ReceiptNo"], dr["ReceiptDate"], dr["PayerAddress"], dr["User"], dr["RevenueCode"], dr["Description"], dr["ChequeBankCode"], dr["ChequeBankName"], dr["AgencyName"], dr["AgencyCode"], dr["BankCode"], dr["BankName"], dr["BranchCode"], dr["BranchName"], dr["ZoneCode"], dr["ZoneName"], dr["Username"], dr["AmountWords"], dr["EReceipts"], dr["EReceiptsDate"], dr["GeneratedBy"], dr["StationCode"], dr["StationName"], dr["PaymentRefNumber"]);


                        db_con = new OleDbConnection(conn);

                        OleDbCommand cmd = new OleDbCommand(qury, db_con);
                        cmd.ExecuteNonQuery();

                    }


                }
                //update online table after download
                switch (Program.intCode)
                {
                    case 13://Akwa Ibom state

                        break;

                    case 32://kogi state
                        break;

                    case 37://ogun state

                        using (var DriveCollection = new CollectionManager.CollectionManager())
                        {
                            dsreturn = DriveCollection.CmuCollectionDownloadUpdate(dss, strTin);
                        }
                        break;

                    case 40://oyo state

                        break;

                    default:
                        break;
                }
            }
            else
            {
                MessageBox.Show(String.Format("No More Records for {0} Tax Drive Team", strname));
                return;
            }


            SplashScreenManager.CloseForm(false);
        }

        void getDriveName()
        {
            //get tax drive name

            string conn = Logic.ConfigureSettings();

            db_con = new OleDbConnection(conn);

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
                //passed tax drive name to the web services
                strname = ds.Tables[0].Rows[0]["StationName"].ToString();
                strTin = ds.Tables[0].Rows[0]["UTIN"].ToString();
                this.Text = String.Format("{0} Tax Drive Team for {1} State", strname, Program.stateName);
            }
            else
            {
                MessageBox.Show("Tax Drive Station Not Set");
                return;
            }
        }

    }
}
