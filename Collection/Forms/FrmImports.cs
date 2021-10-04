using System;
using System.Data;
using System.Data.Odbc;
using System.Data.SqlClient;
using System.IO;
using System.Windows.Forms;
using TaxSmartSuite.Class;
using Collection.Classess;
using System.Data.OleDb;
using MosesClassLibrary.Security;

//using TaxSmartSuite.Dataset;

namespace Collection.Forms
{
    public partial class FrmImports : Form
    {
        public static int tableType = 0;

        string strFormat, strEncoding;

        DataTable Dt = new DataTable();

        DataTable dt = new DataTable();

        OleDbConnection MyConnection = null;
        System.Data.DataSet DtSet = null;
        OleDbDataAdapter MyCommand = null;
       
        private string fileCSV;		//full file name

        private string dirCSV;		//directory of file to import
//name (with extension) of file to import - field

        private long rowCount = 0;

        private string dbowner,strtablename;

        public static FrmImports publicStreetGroup;

        public FrmImports()
        {
            InitializeComponent();

            //connects.ConnectionString();
            
            dbowner = "dbo";
            
            setImages();

            ToolStripEvent();

            loadtype();

            publicStreetGroup = this;

             //bttnCancel.Click += Bttn_Click;
             bttnPreview.Click += Bttn_Click;

             bttnBrowse.Click += Bttn_Click;

             bttnImport.Click += Bttn_Click;

             NavBars.ToolStripEnableDisable(toolStrip, null, false);

             txtFiletoLoad.TextChanged += txtFiletoLoad_TextChanged;
        }

        private void loadtype()
        {
            if (tableType == 1)
            {
              groupControl1.Text = " Collection Data Import";
            }
            else if (tableType == 2)
            {
                groupControl1.Text = " Tax Agent Data Import";
            }
            else if (tableType == 3)
            {
                groupControl1.Text = " Tax Payer Data Import";
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

         private void loadPreview()
        {
           
            try
            {
                // select format, encoding, an write the schema file
                Format();
                Encoding();
                writeSchema();
                //LoadAndDecryptTable();
                // loads the first 500 rows from CSV file, and fills the
                // DataGridView control.
                Dt = LoadAndDecryptTable();
                //this.dataGridView_preView.DataSource = Dt;
                //this.gridControl1
                gridControl1.DataSource = Dt.DefaultView;
                gridView1.OptionsBehavior.Editable = false;
                gridView1.BestFitColumns();

                // dt = LoadCSV().Tables[0];
                // label4.Text = dt.Rows.Count + "Rows";
                //gridControl1.DataSource = dt;
                //gridControl1.DataMember = "csv";

                    //this.dataGridView_preView.DataSource = LoadCSV(500);
                    //this.dataGridView_preView.DataMember = "csv";

                //this.dataGridView_preView.DataMember = "csv";
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
                //strEncoding = "Unicode";
                strEncoding = "ANSI";
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
            //string fileContent = File.ReadAllText(txtFiletoLoad.Text);
            string outfilename = Path.Combine(Path.GetTempPath(), "test.csv");
            File.WriteAllText(outfilename, fileContent);
            DataTable Dt = LoadCSV(outfilename).Tables[0];
            File.Delete(outfilename);


            return Dt;
        }

        public System.Data.DataSet LoadCSV()
        {
           System.Data.DataSet ds = new  System.Data.DataSet();
            try
            {
                // Creates and opens an ODBC connection
                string strConnString = "Driver={Microsoft Text Driver (*.txt; *.csv)};Dbq=" + this.dirCSV.Trim() + ";Extensions=asc,csv,tab,txt;Persist Security Info=False";
                string sql_select;
                OdbcConnection conn;
                conn = new OdbcConnection(strConnString.Trim());
                conn.Open();

                //Creates the select command text
                //if (numberOfRows == -1)
                //{
                //    sql_select = "select * from [" + this.FileNevCSV.Trim() + "]";
                //}
                //else
                //{
                //    sql_select = "select top " + numberOfRows + " * from [" + this.FileNevCSV.Trim() + "]";
                //}
                sql_select = "select * from [" + this.FileNevCSV.Trim() + "]";

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

        public static System.Data.DataSet LoadCSV(String filename)
        {
            var ds = new System.Data.DataSet();

            try
            {
                string myPath = Path.GetDirectoryName(filename);
                // Creates and opens an ODBC connection
                string strConnString = String.Format("Driver={{Microsoft Text Driver (*.txt; *.csv;*)}};Dbq={0};Extensions=asc,csv,tab,txt;Persist Security Info=False", myPath);
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

        private void Import_Click()
        {
            // Copies all rows to the database from the dataset.
            //using (SqlBulkCopy bc = new SqlBulkCopy(connects.constring))
            using (SqlBulkCopy bc = new SqlBulkCopy(Classess.Logic.ConnectionString))
            
            {
                // Destination table with owner - this example doesn't
                // check the owner and table names!
                //bc.DestinationTableName = "[" + this.dbowner.ToString() + "].[" + this.strtablename + "]";
                if (tableType == 1)
                {
                    bc.DestinationTableName = "dbo.tblCollectionReport";// "[" + this.txtOwner.Text + "].[" + this.txtTableName.Text + "]";
                }
                else if (tableType == 2)
                {
                    bc.DestinationTableName = "dbo.tblTaxAgent";
                }
                else if (tableType == 3)
                {
                    bc.DestinationTableName = "dbo.tblTaxPayer";
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

        private void FrmImports_Load(object sender, EventArgs e)
        {

        }

        private void setImages()
        {
            tsbNew.Image = MDIMain.publicMDIParent.i16x16.Images[5];
            tsbEdit.Image = MDIMain.publicMDIParent.i16x16.Images[6];
            tsbDelete.Image = MDIMain.publicMDIParent.i16x16.Images[7];
            tsbReload.Image = MDIMain.publicMDIParent.i16x16.Images[0];
            tsbClose.Image = MDIMain.publicMDIParent.i16x16.Images[11];

            //bttnCancel.Image = MDIMain.publicMDIParent.i32x32.Images[9];
            bttnPreview.Image = MDIMain.publicMDIParent.i32x32.Images[8];
            bttnBrowse.Image = MDIMain.publicMDIParent.i32x32.Images[7];
            bttnImport.Image = MDIMain.publicMDIParent.i32x32.Images[28];

        }

        private void btnPreview_Click_1(object sender, EventArgs e)
        {

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
                //UpdateRecord();
                Import_Click();
            }
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

        public void RefreshForm()
        {
            NavBars.ToolStripEnableDisableControls(toolStrip, Tag as String);
        }

        private void bttnPreview_Click(object sender, EventArgs e)
        {

        }

    }
}
