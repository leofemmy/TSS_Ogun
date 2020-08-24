using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Data.Odbc;
using System.Data.SqlClient;
using Collection.Classess;
using TaxSmartSuite;
using DevExpress.Utils;
using System.Data.OleDb;
using TaxSmartSuite.Class;

namespace Collection.Forms
{
    public partial class FrmPayDirect : Form
    {
        public static FrmPayDirect publicStreetGroup;

        string strFormat, strEncoding;

        DataTable Dt = new DataTable();

        DataTable dt = new DataTable();

        DataTable Dts = new DataTable();

        private string fileCSV;		//full file name

        private string dirCSV;

        private long rowCount = 0;

        OleDbConnection MyConnection = null;

        System.Data.DataSet DtSet = null;

        OleDbDataAdapter MyCommand = null;

        public static string CheuqeOption;

        private string dbowner, strtablename;

        private SqlDataAdapter adp;

        private SqlCommand command;

        string user;

        public FrmPayDirect()
        {
            InitializeComponent();

            setImages();

            ToolStripEvent();

            publicStreetGroup = this;

            bttnBrowse.Click += Bttn_Click;

            bttnPreview.Click += Bttn_Click;

            bttnImport.Click += Bttn_Click;

            txtFiletoLoad.TextChanged += txtFiletoLoad_TextChanged;

            //label6.Text = dateTimePicker1.Value.Subtract(1);

            if (Program.UserID == "" || Program.UserID == null)
            { 
                user="Femi";
            }
            else
            {
                user=Program.UserID;            
            }

        }

        private void setImages()
        {
            tsbNew.Image = MDIMain.publicMDIParent.i16x16.Images[5];
            tsbEdit.Image = MDIMain.publicMDIParent.i16x16.Images[6];
            tsbDelete.Image = MDIMain.publicMDIParent.i16x16.Images[7];
            tsbReload.Image = MDIMain.publicMDIParent.i16x16.Images[0];
            tsbClose.Image = MDIMain.publicMDIParent.i16x16.Images[11];

            bttnImport.Image = MDIMain.publicMDIParent.i32x32.Images[28];
            bttnPreview.Image = MDIMain.publicMDIParent.i32x32.Images[8];
            bttnBrowse.Image = MDIMain.publicMDIParent.i32x32.Images[7];

        }

        void ToolStripEvent()
        {
            tsbClose.Click += OnToolStripItemsClicked;
            tsbNew.Click += OnToolStripItemsClicked;
            tsbEdit.Click += OnToolStripItemsClicked;
            tsbDelete.Click += OnToolStripItemsClicked;
            tsbReload.Click += OnToolStripItemsClicked;
        }

        void OnToolStripItemsClicked(object sender, EventArgs e)
        {
            if (sender == tsbClose)
            {
                MDIMain.publicMDIParent.RemoveControls();
            }

        }

        void Bttn_Click(object sender, EventArgs e)
        {
            if (sender == bttnBrowse)
            {
                Browse();
            }
            else if (sender == bttnPreview)
            {
                loadPreview();

            }
            else if (sender == bttnImport)
            {
                //Import_Click();
                if (radioGroup1.SelectedIndex == 0)//Execel import
                {
                    ImportCsvFile();
                }
                else //csv/ text file improt
                {
                    ImportCsvFile();
                }
            }
        }

        private void Browse()
        {
            using (OpenFileDialog openFileDialogCSV = new OpenFileDialog())
            {
                if (this.radioGroup1.SelectedIndex < 0)
                {
                    MessageBox.Show(" Please Select Type of Data to Load ", " Data Import ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else
                {
                    if (this.radioGroup1.SelectedIndex == 0)
                    {
                        if (this.radioGroup2.SelectedIndex == 0)//open excel file 2003
                        {
                            //exportToExcel();
                            openFileDialogCSV.InitialDirectory = Application.ExecutablePath.ToString();
                            openFileDialogCSV.Filter = "Excel files (*.xls)|*.xls|All files (*.*)|*.*";
                            openFileDialogCSV.FilterIndex = 1;
                            openFileDialogCSV.RestoreDirectory = true;
                        }
                        else
                        {
                            //exportToExcel();
                            openFileDialogCSV.InitialDirectory = Application.ExecutablePath.ToString();
                            openFileDialogCSV.Filter = "Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*";
                            openFileDialogCSV.FilterIndex = 1;
                            openFileDialogCSV.RestoreDirectory = true;
                        }
                     
                    }
                    else
                    {
                        //exportToCSV();                 
                        openFileDialogCSV.InitialDirectory = Application.ExecutablePath.ToString();
                        openFileDialogCSV.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*";
                        openFileDialogCSV.FilterIndex = 1;
                        openFileDialogCSV.RestoreDirectory = true;
                    }
                    if (openFileDialogCSV.ShowDialog() == DialogResult.OK)
                        txtFiletoLoad.Text = openFileDialogCSV.FileName.ToString();
                }
            }
        }

        private void txtFiletoLoad_TextChanged(object sender, EventArgs e)
        {
            // full file name
            fileCSV = txtFiletoLoad.Text;

            // creates a System.IO.FileInfo object to retrive information from selected file.
            FileInfo fi = new FileInfo(fileCSV);
            // retrives directory
            this.dirCSV = fi.DirectoryName.ToString();
            // retrives file name with extension
            FileNevCSV = fi.Name.ToString();

            // database table name
            strtablename = fi.Name.Substring(0, fi.Name.Length - fi.Extension.Length).Replace(" ", "_");
        }

        public string //name (with extension) of file to import - property
      FileNevCSV { get; set; }

        private void Format()
        {
            try
            {
                strFormat = "Delimited(;)";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Format");
            }
        }

        private void Encoding()
        {
            try
            {
                //strEncoding = "Unicode";
                strEncoding = "ANSI";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Encoding");
            }
        }

        private void writeSchema()
        {
            try
            {
                using (FileStream fsOutput = new FileStream(this.dirCSV + "\\schema.ini", FileMode.Create, FileAccess.Write))
                {
                    StreamWriter srOutput = new StreamWriter(fsOutput);
                    string s1;
                    string s2;
                    string s3;
                    string s4;
                    string s5;
                    s1 = String.Format("[{0}]", this.FileNevCSV);
                    s2 = "ColNameHeader=" + true;
                    // chkFirstRowColumnNames.Checked.ToString();
                    s3 = "Format=" + strFormat;
                    s4 = "MaxScanRows=25";
                    s5 = "CharacterSet=" + strEncoding;
                    srOutput.WriteLine(String.Format("{0}\r\n{1}\r\n{2}\r\n{3}\r\n{4}", s1, s2, s3, s4, s5));
                    srOutput.Close();
                    fsOutput.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "writeSchema");
            }
            finally
            { }
        }

        private void loadPreview()
        {
            using (WaitDialogForm form = new WaitDialogForm())
            {
                //determine what to load either excle or csv
                if (this.radioGroup1.SelectedIndex == 0)//excle file
                {
                    if (this.radioGroup2.SelectedIndex == 0)//open excel file 2003
                    {
                        //Connection for MS Excel 2003 .xls format
                        MyConnection = new OleDbConnection(String.Format("provider=Microsoft.Jet.OLEDB.4.0; Data Source='{0}';Extended Properties=Excel 8.0;", txtFiletoLoad.Text));
                    }
                    else
                    {
                        //Connection for .xslx 2007 format
                        MyConnection = new OleDbConnection(String.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source='{0}';Extended Properties=Excel 12.0;", txtFiletoLoad.Text));
                    }
                                      

                    //Select your Excel file
                    MyCommand = new System.Data.OleDb.OleDbDataAdapter("select * from [Sheet1$]", MyConnection);
                    DtSet = new System.Data.DataSet();
                    //Bind all excel data in to data set
                    MyCommand.Fill(DtSet, "[Sheet1$]");
                    Dt = DtSet.Tables[0];
                    MyConnection.Close();
                    //Check datatable have records
                    if (Dt.Rows.Count > 0)
                    {

                        gridControl1.DataSource = Dt.DefaultView;
                        gridView1.OptionsBehavior.Editable = false;
                        gridView1.BestFitColumns();
                    }
                    label4.Text = Dt.Rows.Count + "  Rows of Records to be Imported ";

              
                }
                else//csv file
                {
                    try
                    {
                        // select format, encoding, an write the schema file
                        Format();
                        Encoding();
                        writeSchema();
                      
                        Dt = LoadCSV().Tables[0];
                        //dt.Columns.Add("MyRow", type(System.Int32));
                        Dt.Columns.Add("Cheque_Status", typeof(System.String));
                        foreach (DataRow row in Dt.Rows)
                        {

                            if (row["payment_method_name"].ToString() == "Cash")
                            {
                                row["Cheque_Status"] = "Cleared";
                            }
                            else
                            {
                                row["Cheque_Status"] = "Pending";
                            }
                      
                        }
                        Dt.AcceptChanges();
                        label4.Text = Dt.Rows.Count + "  Rows to be Imported ";
                        gridControl1.DataSource = Dt;
                        gridView1.BestFitColumns();
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.Message, "Error - loadPreview", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

               
            }

        }

        public System.Data.DataSet LoadCSV()
        {
            System.Data.DataSet ds = new System.Data.DataSet();
            try
            {
                // Creates and opens an ODBC connection
                //string strConnString = String.Format("Driver={{Microsoft Text Driver (*.txt; *.csv;)}};Dbq={0};Extensions=asc,csv,tab,txt;Persist Security Info=False", dirCSV.Trim());

                string strConnString = String.Format("Driver={{Microsoft Text Driver (*.txt; *.csv)}};Dbq={0};Extensions=asc,csv,tab,txt;Persist Security Info=False", this.dirCSV.Trim());

                string sql_select;
                OdbcConnection conn;
                conn = new OdbcConnection(strConnString.Trim());
                conn.Open();

               
                sql_select = String.Format("select * from [{0}]", FileNevCSV.Trim());

                //Creates the data adapter
                OdbcDataAdapter obj_oledb_da = new OdbcDataAdapter(sql_select, conn);

                //Fills dataset with the records from CSV file
                obj_oledb_da.Fill(ds, "csv");

                //closes the connection
                conn.Close();
            }
            catch (Exception e) //Error
            {
                MessageBox.Show(e.Message, "Error - LoadCSV", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return ds;
        }

        void ImportCsvFile()
        {
            if (dateTimePicker1.Value >= DateTime.Now.Date)
            {
                Common.setMessageBox("Transaction Date Must be Less than Today Date", Program.ApplicationName, 3);
                return;
            }
            else
            {
                bool bResponse = false;

                using (WaitDialogForm form = new WaitDialogForm())
                {
                    if (CheuqeOption.ToString() == "0001")
                    {//import normal paydirect record
                        try
                        {
                            using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
                            {
                                connect.Open();
                                command = new SqlCommand("insertCollectionReportPay", connect) { CommandType = CommandType.StoredProcedure };
                                command.Parameters.Add(new SqlParameter("@CollectionReport_Temp", SqlDbType.Structured)).Value = Dt;
                                command.Parameters.Add(new SqlParameter("@User", SqlDbType.VarChar)).Value = user;
                                command.Parameters.Add(new SqlParameter("@DateValidate", SqlDbType.DateTime)).Value = dateTimePicker1.Value;

                                using (System.Data.DataSet ds = new System.Data.DataSet())
                                {
                                    adp = new SqlDataAdapter(command);
                                    adp.Fill(ds);
                                    Dts = ds.Tables[0];
                                    connect.Close();
                                    bResponse = true;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            bResponse = false;
                            MessageBox.Show(ex.Message, " CSV Import ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    if (CheuqeOption.ToString() == "0003")
                    {
                        //update cheque bank status in the collection report table
                        try
                        {
                            using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
                            {
                                connect.Open();
                                command = new SqlCommand("UpdateCollectionChequeStatus", connect) { CommandType = CommandType.StoredProcedure };
                                command.Parameters.Add(new SqlParameter("@CollectionDownloadCredentials_Temp", SqlDbType.Structured)).Value = Dt;
                                command.Parameters.Add(new SqlParameter("@User", SqlDbType.VarChar)).Value = user;
                                using (System.Data.DataSet ds = new System.Data.DataSet())
                                {
                                    adp = new SqlDataAdapter(command);
                                    adp.Fill(ds);
                                    if (ds.Tables[0].Rows[0]["returnCode"].ToString() == "-1")
                                        MessageBox.Show(ds.Tables[0].Rows[0]["dateeee"].ToString(), " CSV Import ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    Dts = ds.Tables[0];
                                    connect.Close();
                                    bResponse = true;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            bResponse = false;
                            MessageBox.Show(ex.Message, " CSV Import ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }

                }

                if (bResponse && Dts.Rows[0][0].ToString() == "00")
                {
                    MessageBox.Show("Record Update successfully", " CSV Import ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Record update not successfully", " CSV Import ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

           
        }

        private void bttnImport_Click(object sender, EventArgs e)
        {
        }

        private void radioGroup1_Properties_Click(object sender, EventArgs e)
        {
            if (radioGroup1.SelectedIndex == 1)
            {
                radioGroup2.Visible = true;
            }
            else
                radioGroup2.Visible = false;
        }

        //string GenerateReceiptnumber()
        //{
        //    string strconnect = Logic.ConnectionString;

        //    SqlConnection connect = new SqlConnection(strconnect);

        //    connect.Open();


        //    SqlCommand command = new SqlCommand("doGenerateReceiptNumber", connect) { CommandType = CommandType.StoredProcedure };

        //    command.Parameters.Add(new SqlParameter("@LastIssueDate", SqlDbType.Date)).Value = dtpIssues.Value.Date.ToString("yyyy-MM-dd");
        //    command.Parameters.Add(new SqlParameter("@LastExpiryDate", SqlDbType.Date)).Value = dtpExpiry.Value.Date.ToString("yyyy-MM-dd");
        //    command.Parameters.Add(new SqlParameter("@VehiclePlateNumber", SqlDbType.VarChar)).Value = txtPlate.Text.Trim();
        //}

    }
}
