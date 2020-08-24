using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TaxSmartSuite.Class;
using System.IO;
using System.Data.Odbc;
using System.Data.SqlClient;


namespace Normalization.Forms
{
    public partial class FrmImports : Form
    {
        TaxSmartSuite.Class.DBConnection connects = new DBConnection();

        public static int tableType = 0;

        string strFormat, strEncoding;

        DataTable Dt = new DataTable();

        private string fileCSV;		//full file name

        private string dirCSV;		//directory of file to import

        private long rowCount = 0;

        private string dbowner, strtablename;

        public FrmImports()
        {
            InitializeComponent();
            //connects.ConnectionString();
            dbowner = "dbo";
        }

        private void btnclose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
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
                        //exportToExcel();
                        openFileDialogCSV.InitialDirectory = Application.ExecutablePath.ToString();
                        openFileDialogCSV.Filter = "Excel files (*.xls)|*.xls|All files (*.*)|*.*";
                        openFileDialogCSV.FilterIndex = 1;
                        openFileDialogCSV.RestoreDirectory = true;
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

        private void btnPreview_Click(object sender, EventArgs e)
        {
            loadPreview();
        }

        private void loadPreview()
        {
            try
            {
                // select format, encoding, an write the schema file
                Format();
                Encoding();
                writeSchema();
               Dt = LoadAndDecryptTable();
                this.dataGridView_preView.DataSource = Dt;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error - loadPreview", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public string //name (with extension) of file to import - property
      FileNevCSV { get; set; }


        private bool fileCheck()
        {
            if ((fileCSV == "") || (fileCSV == null) || (dirCSV == "") || (dirCSV == null) || (FileNevCSV == "") || (FileNevCSV == null))
            {
                MessageBox.Show("Select a CSV file to load first!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else
            {
                return true;
            }
        }

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
            finally
            {
            }
        }

        private void Encoding()
        {
            try
            {
                strEncoding = "Unicode";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Encoding");
            }
            finally
            {
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

        private DataTable LoadAndDecryptTable()
        {
            string fileContent = CryptorEngine.Decrypt(File.ReadAllText(txtFiletoLoad.Text), true);
            string outfilename = Path.Combine(Path.GetTempPath(), "test.csv");
            File.WriteAllText(outfilename, fileContent);
            DataTable Dt = LoadCSV(outfilename).Tables[0];
            File.Delete(outfilename);


            return Dt;
        }

        public static System.Data.DataSet LoadCSV(String filename)
        {
            var ds = new System.Data.DataSet();

            try
            {
                string myPath = Path.GetDirectoryName(filename);
                // Creates and opens an ODBC connection
                string strConnString = String.Format("Driver={{Microsoft Text Driver (*.txt; *.csv)}};Dbq={0};Extensions=asc,csv,tab,txt;Persist Security Info=False", myPath);
                string sql_select;
                OdbcConnection conn;
                conn = new OdbcConnection(strConnString.Trim());
                conn.Open();


                sql_select = String.Format("select * from [{0}]", Path.GetFileName(filename));

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

        private void btnImport_Click(object sender, EventArgs e)
        {
            // Copies all rows to the database from the dataset.
            using (SqlBulkCopy bc = new SqlBulkCopy(connects.constring))
            {
                // Destination table with owner - this example doesn't
                // check the owner and table names!
                //bc.DestinationTableName = "[" + this.dbowner.ToString() + "].[" + this.strtablename + "]";
                if (tableType == 1)
                {
                    bc.DestinationTableName = "dbo.CollectionReport";// "[" + this.txtOwner.Text + "].[" + this.txtTableName.Text + "]";
                }
                else if (tableType == 2)
                {
                    bc.DestinationTableName = "dbo.[PayerInfo_organization]";
                }
                else if (tableType == 3)
                {
                    bc.DestinationTableName = "dbo.[PayerInfo_individual]";
                }

                // User notification with the SqlRowsCopied event
                bc.NotifyAfter = 100;
                bc.SqlRowsCopied += OnSqlRowsCopied;

                // Starts the bulk copy.
                bc.WriteToServer(Dt);

                // Closes the SqlBulkCopy instance
                bc.Close();
            }
        }

        private void OnSqlRowsCopied(object sender, SqlRowsCopiedEventArgs e)
        {
            lblProgress.Text = String.Format("Imported: {0}/{1} row(s)", e.RowsCopied, rowCount);
            lblProgress.Refresh();
        }
    }
}
