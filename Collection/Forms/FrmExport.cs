using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.IO;
using System.Windows.Forms;
using TaxSmartSuite;
using TaxSmartSuite.Class;
using Collection.Classess;

namespace Collection.Forms
{
    public partial class FrmExport : Form
    {
        DBConnection connects = new DBConnection();

        SqlDataAdapter da;

        SqlConnection cn;

        public static int tableType = 0;

        public static FrmExport publicStreetGroup;


        public FrmExport()
        {
            InitializeComponent();

            //connects.ConnectionString();

            publicStreetGroup = this;

            loadTables();

            setImages();

            ToolStripEvent();

            loadtype();

            //bttnCancel.Click += Bttn_Click;
           btnExport.Click += Bttn_Click;
           btnExport.Click += Bttn_Click;
            //bttnImport.Click += Bttn_Click;

           NavBars.ToolStripEnableDisable(toolStrip, null, false);
        }

        private void loadtype()
        {
            if (tableType == 1)
            {
                groupControl1.Text = " Collection Data Export";
            }
            else if (tableType == 2)
            {
                groupControl1.Text = " Tax Agent Data Export";
            }
            else if (tableType == 3)
            {
                groupControl1.Text = " Tax Payer Data Export";
            }
        }

        private void setImages()
        {
            tsbNew.Image = MDIMain.publicMDIParent.i16x16.Images[5];

            tsbEdit.Image = MDIMain.publicMDIParent.i16x16.Images[6];

            tsbDelete.Image = MDIMain.publicMDIParent.i16x16.Images[7];

            tsbReload.Image = MDIMain.publicMDIParent.i16x16.Images[0];

            tsbClose.Image = MDIMain.publicMDIParent.i16x16.Images[11];

            //bttnCancel.Image = MDIMain.publicMDIParent.i32x32.Images[9];

          btnExport.Image = MDIMain.publicMDIParent.i32x32.Images[8];

          btnRefresh.Image = MDIMain.publicMDIParent.i16x16.Images[0];

        }

        void Bttn_Click(object sender, EventArgs e)
        {
            if (sender == btnRefresh)
            {
                loadTables();
            }
            else if (sender == btnExport)
            {
                Export();
              
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

        private void FrmExport_Load(object sender, EventArgs e)
        {
            loadTables();
        }

        private void loadTables()
        {
            // Conncets to database, and selects the table names.
            cn = new SqlConnection(Classess.Logic.ConnectionString);
            //select all table inside the database table
            //SqlDataAdapter da = new SqlDataAdapter("select name from dbo.sysobjects where xtype = 'U' and name <> 'dtproperties' order by name", cn);

            //determine which table to load from
            if (tableType == 1)
            {
                //select one table inside the database table
                da =
                    new SqlDataAdapter(
                        "select name from dbo.sysobjects where xtype = 'U' and name ='tblCollectionReport' order by name", cn);

            }
            else if (tableType == 2)
            {
                //select one table inside the database table
                da =
                    new SqlDataAdapter(
                        "select name from dbo.sysobjects where xtype = 'U' and name = 'tblTaxAgent' order by name", cn);

            }
            else if (tableType == 3)
            {
                //select one table inside the database table
                da =
                    new SqlDataAdapter(
                        "select name from dbo.sysobjects where xtype = 'U' and name ='tblTaxPayer' order by name", cn);

            }

            //select one table inside the database table
            //da =
            //    new SqlDataAdapter(
            //        "select name from dbo.sysobjects where xtype = 'U' and name <> 'dtproperties' order by name", cn);

            using (DataTable dt = new DataTable())
            {
                // Fills the list to an DataTable.
                da.Fill(dt);
                // Clears the ListBox
                lstTable.Items.Clear();
                // Fills the table names to the ListBox.
                // Notifies user if there is no user table in the database yet.
                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("There is no user table in the specified database. Import a CSV file first.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    lstTable.Items.Add("< no user table in database >");
                    btnExport.Enabled = false;
                }
                else
                {
                    btnExport.Enabled = true;
                    for (int i = 0; i < dt.Rows.Count; i++)
                        lstTable.Items.Add(dt.Rows[i][0].ToString());
                    lstTable.SelectedIndex = 0;
                }
            }

        }

        private void Refresh()
        {
            loadTables();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Export()
        {
            //exportToCSV();
            if(radioGroup1.SelectedIndex <0)
            {
                MessageBox.Show("Please Data Export Options", "Data Export", MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                return;
            }
            else
            {
                if(this.radioGroup1.SelectedIndex ==0)
                {
                    exportToExcel();
                }
                else
                {
                    exportToCSV();
                }
            }
        }

        private void exportToCSV()
        {
            //Asks the filename with a SaveFileDialog control.

            using (SaveFileDialog saveFileDialogCSV = new SaveFileDialog { InitialDirectory = Application.ExecutablePath.ToString(), Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*", FilterIndex = 1, RestoreDirectory = true })
            {
                if (saveFileDialogCSV.ShowDialog() == DialogResult.OK)
                {
                    // Runs the export operation if the given filenam is valid.
                    string filename = String.Format("{0}_{1}{2}", saveFileDialogCSV.FileName.Substring(0, saveFileDialogCSV.FileName.Length - 4), lstTable.SelectedItem, saveFileDialogCSV.FileName.Substring(saveFileDialogCSV.FileName.Length - 4));
                    exportToCSVfile(filename);
                }
            }
        }
      
        private void exportToExcel()
        {
            //Asks the filenam with a SaveFileDialog control.

            using (SaveFileDialog saveFileDialogCSV = new SaveFileDialog { InitialDirectory = Application.ExecutablePath.ToString(), Filter = "Excel files (*.xls)|*.xls|All files (*.*)|*.*", FilterIndex = 1, RestoreDirectory = true })
            {
                if (saveFileDialogCSV.ShowDialog() == DialogResult.OK)
                {
                    // Runs the export operation if the given filenam is valid.
                    string filename = String.Format("{0}_{1}{2}", saveFileDialogCSV.FileName.Substring(0, saveFileDialogCSV.FileName.Length - 4),lstTable.SelectedItem, saveFileDialogCSV.FileName.Substring(saveFileDialogCSV.FileName.Length - 4));
                    exportToCSVfile(filename);
                }
            }
        }

        private void exportToCSVfile(string fileOut)
        {
            // Connects to the database, and makes the select command.
            using (SqlConnection conn = new SqlConnection(Classess.Logic.ConnectionString))
            {
                string sqlQuery = "select * from " + lstTable.SelectedItem.ToString();
                SqlCommand command = new SqlCommand(sqlQuery, conn);
                conn.Open();
                // Creates a SqlDataReader instance to read data from the table.
                SqlDataReader dr = command.ExecuteReader();
                // Retrives the schema of the table.
                DataTable dtSchema = dr.GetSchemaTable();
                // Creates the CSV file as a stream, using the given encoding.
                using (StreamWriter sw = new StreamWriter(fileOut, false, encodingCSV))
                {
                    string strRow;
                   
                    string fileContent = columnNames(dtSchema, FrmExport.separator) + Environment.NewLine;
                    while (dr.Read())
                    {
                        strRow = "";
                        for (int i = 0; i < dr.FieldCount; i++)
                        {
                            // strRow += CryptorEngine.Encrypt(dr.GetString(i), true);
                            //dr.GetValue( )
                            strRow += dr.GetValue(i);
                            if (i < dr.FieldCount - 1)
                                strRow += separator;
                        }
                        //encrpytion goes head
                        //sw.WriteLine(strRow);
                        fileContent += strRow + Environment.NewLine;
                    }
                    sw.Write(CryptorEngine.Encrypt(fileContent, true));
                    // Closes the text stream and the database connenction.
                    sw.Close();
                }
                conn.Close();
            }

            // Notifies the user.
            MessageBox.Show("Export Data File Done");
        }

        private static string columnNames(DataTable dtSchemaTable, string delimiter)
        {
            string strOut = "";

            if (delimiter.ToLower() == "tab")
            {
                delimiter = "\t";
            }

            for (int i = 0; i < dtSchemaTable.Rows.Count; i++)
            {

                strOut += dtSchemaTable.Rows[i][0].ToString();


                if (i < dtSchemaTable.Rows.Count - 1)
                {
                    strOut += delimiter;
                }

            }
            return strOut;
        }

        private static string separator
        {
            get
            {
                return ",";
            }
        }

        private static Encoding encodingCSV
        {
            get
            {
                //if (rdbUnicode.Checked)
                //{
                //    return Encoding.Unicode;
                //}
                //else if (rdbASCII.Checked)
                //{
                //    return Encoding.ASCII;
                //}
                //else if (rdbUTF7.Checked)
                //{
                //    return Encoding.UTF7;
                //}
                //else if (rdbUTF8.Checked)
                //{
                //    return Encoding.UTF8;
                //}
                //else
                //{
                //    return Encoding.Unicode;
                //}
                return Encoding.Unicode;
                // You can add other options, for ex.:
                //return Encoding.GetEncoding("iso-8859-2");
                //return Encoding.Default;
            }
        }

        public void RefreshForm()
        {
            NavBars.ToolStripEnableDisableControls(toolStrip, Tag as String);
        }

      
    }
}
