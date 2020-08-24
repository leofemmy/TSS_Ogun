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
using System.Globalization;
using DevExpress.XtraGrid;


namespace Collection.Forms
{
    public partial class FrmPayDirect : Form
    {
        public static FrmPayDirect publicStreetGroup;

        BackUpDB _backupdb = new BackUpDB();

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

        string user, Cheuqe;

        string[] split;

        string strDate = null;

        public FrmPayDirect()
        {
            InitializeComponent();

            setImages();

            ToolStripEvent();

            publicStreetGroup = this;

            bttnBrowse.Click += Bttn_Click;

            bttnPreview.Click += Bttn_Click;

            bttnImport.Click += Bttn_Click;

            bttnReset.Click += bttnReset_Click;

            txtFiletoLoad.TextChanged += txtFiletoLoad_TextChanged;

            this.dateTimePicker1.Value = DateTime.Today.AddDays(-1);
            //label6.Text = dateTimePicker1.Value.Subtract(1);
            Cheuqe = CheuqeOption;

            if (Program.UserID == "" || Program.UserID == null)
            {
                user = "Femi";
            }
            else
            {
                user = Program.UserID;
            }

            gridView1.RowStyle += gridView1_RowStyle;

            radioGroup1.SelectedIndexChanged += radioGroup1_SelectedIndexChanged;
        }

        void radioGroup1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (radioGroup1.EditValue == null) return;

            if ((Int32)this.radioGroup1.EditValue == 1)
            {

                //this.label1.Text = "Receipts No.";
                this.radioGroup2.Visible = true;
            }
            else if ((Int32)this.radioGroup1.EditValue == 2)
            {
                //txtSearch.Clear();
                this.radioGroup2.Visible = false;
            }
        }

        void gridView1_RowStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs e)
        {
            //if (e.RowHandle >= 0)
            //{
            //    int datediff = Convert.ToInt32(gridView1.GetRowCellValue(e.RowHandle, gridView1.Columns["DateDiff"]));
            //    if (datediff == 0)
            //        e.Appearance.BackColor = Color.LightGreen;
            //    else
            //        e.Appearance.BackColor = Color.DarkOrange;
            //}
        }

        void bttnReset_Click(object sender, EventArgs e)
        {
            txtFiletoLoad.Text = ""; gridControl1.DataSource = null;
            radioGroup1.SelectedIndex = -1;
            bttnBrowse.Enabled = true;
            bttnImport.Enabled = false;
            bttnPreview.Enabled = false;
            if (gridView1 != null) gridView1.Columns.Clear();
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
            bttnReset.Image = MDIMain.publicMDIParent.i16x16.Images[6];


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
                //MDIMain.publicMDIParent.RemoveControls();
                Close();
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
                        txtFiletoLoad.Text = openFileDialogCSV.FileName;

                    if (txtFiletoLoad.Text.Length > 0)
                    {
                        bttnPreview.Enabled = true;
                        bttnBrowse.Enabled = false;
                    }
                    else
                    {
                        bttnPreview.Enabled = false;
                        bttnBrowse.Enabled = true;
                    }
                }
            }
        }

        private void txtFiletoLoad_TextChanged(object sender, EventArgs e)
        {
            // full file name
            if (string.IsNullOrEmpty(txtFiletoLoad.Text))
            {
                return;
            }
            else
            {
                fileCSV = txtFiletoLoad.Text;

                // creates a System.IO.FileInfo object to retrive information from selected file.
                FileInfo fi = new FileInfo(fileCSV);
                // retrives directory
                dirCSV = fi.DirectoryName.ToString();
                // retrives file name with extension
                FileNevCSV = fi.Name.ToString();

                // database table name
                strtablename = fi.Name.Substring(0, fi.Name.Length - fi.Extension.Length).Replace(" ", "_");
            }

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
                    string s5;
                    s1 = String.Format("[{0}]", this.FileNevCSV);
                    s2 = "ColNameHeader=" + true;
                    // chkFirstRowColumnNames.Checked.ToString();
                    s3 = "Format=" + strFormat;
                    s5 = "CharacterSet=" + strEncoding;
                    srOutput.WriteLine(String.Format("{0}\r\n{1}\r\n{2}\r\n{3}\r\n{4}", s1, s2, s3, (string)"MaxScanRows=25", s5));
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
                    //ExcelExport (1)
                    //[Sheet1$]
                    MyCommand = new System.Data.OleDb.OleDbDataAdapter("select * from [Sheet1$]", MyConnection);
                    DtSet = new System.Data.DataSet();
                    //Bind all excel data in to data set
                    MyCommand.Fill(DtSet, "[Sheet1$]");
                    Dt = DtSet.Tables[0];
                    MyConnection.Close();
                    //Check datatable have records

                    //check if record id for paydirect or checque update

                    if (CheuqeOption.ToString() == "0001" && this.radioGroup1.SelectedIndex == 0)
                    {
                        //Dt = LoadCSV().Tables[0];

                        Dt.Columns.Add("Cheque_Status", typeof(System.String));

                        //Dt.Columns.Add("DateDiff", typeof(System.String));

                        //Dt.Columns.Add("DateValidateAgainst", typeof(System.DateTime));

                        Dt.Columns.Add("Cheque_ValueDate", typeof(System.String));

                        foreach (DataRow row in Dt.Rows)
                        {

                            #region 

                            //split = row["payment_ref_num"].ToString().Split(new char[] { '|' });

                            //string[] splitDate = split[2].Split(new char[] { '-' });

                            //string[] splitTime = row["payment_log_date"].ToString().Split(new char[] { ' ' });
                            //DateTime dtPaymentDate = new DateTime(Convert.ToInt32(splitDate[2]), Convert.ToInt32(splitDate[1]), Convert.ToInt32(splitDate[0]));
                            //row["payment_log_date"] = dtPaymentDate.ToString("MM/dd/yyyy");
                            //if (splitDate.Count() > 2 && splitTime.Count() > 0)
                            //{



                            //    //CultureInfo culture = new CultureInfo("en-US");
                            //    dtPaymentDate = Convert.ToDateTime(String.Format("{0} {1} {2}", dtPaymentDate.ToString("dd-MM-yyyy"), splitTime[1], splitTime[2]));
                            //    int dateDiff = dtPaymentDate.Subtract(dateTimePicker1.Value).Days;
                            //    row["payment_log_date"] = dtPaymentDate.ToString("MM/dd/yyyy");
                            //    row["DateDiff"] = Math.Abs(dateDiff);
                            //    row["DateValidateAgainst"] = dateTimePicker1.Value;
                            //}

                            #endregion

                            if (row["payment_method_name"].ToString() == "Other Bank Cheque" || row["payment_method_name"].ToString() == "Other Bank Cheque/Dr")
                            {
                                //Own Bank Cheque
                                row["Cheque_Status"] = "Pending";

                            }
                            else
                            {
                                row["Cheque_Status"] = "Cleared";
                                row["Cheque_ValueDate"] = row["payment_log_date"];
                            }
                        }


                        Dt.AcceptChanges();
                        //label4.Text = Dt.Rows.Count + "  Rows to be Imported ";

                        ReformatAmount2(Dt);

                        Dt.DefaultView.Sort = "revenue_code ASC";

                        gridControl1.DataSource = Dt;

                        gridView1.BestFitColumns();

                        gridView1.GroupFooterShowMode = DevExpress.XtraGrid.Views.Grid.GroupFooterShowMode.VisibleAlways;
                        //GridGroupSummaryItem item = new GridGroupSummaryItem();
                        //item.FieldName = "payment_ref_num";
                        //item.SummaryType = DevExpress.Data.SummaryItemType.Count;
                        //gridView1.GroupSummary.Add(item);

                        GridGroupSummaryItem item2 = new GridGroupSummaryItem() { FieldName = "Cheque_Status", SummaryType = DevExpress.Data.SummaryItemType.Count };
                        gridView1.GroupSummary.Add(item2);
                    }
                    else
                    {
                        if (Dt.Rows.Count > 0)
                        {
                            RemoveColumn(Dt, "F1");
                            RemoveColumn(Dt, "F14");
                            RemoveColumn(Dt, "F15");
                            ReformatAmount(Dt);
                            gridControl1.DataSource = Dt.DefaultView;
                            gridView1.OptionsBehavior.Editable = false;
                            gridView1.BestFitColumns();
                            gridView1.Columns["Amount"].DisplayFormat.FormatType = FormatType.Numeric;
                            gridView1.Columns["Amount"].DisplayFormat.FormatString = "n2";
                        }
                    }

                    //if (Dt.Rows.Count > 0)
                    //{
                    //    //RemoveColumn(Dt, "F1");
                    //    //ReformatAmount(Dt);
                    //    gridControl1.DataSource = Dt.DefaultView;
                    //    gridView1.OptionsBehavior.Editable = false;
                    //    gridView1.BestFitColumns();
                    //    //gridView1.Columns["Amount"].DisplayFormat.FormatType = FormatType.Numeric;
                    //    //gridView1.Columns["Amount"].DisplayFormat.FormatString = "n2";
                    //}
                    //Dt.AcceptChanges();
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

                        Dt.Columns.Add("Cheque_Status", typeof(System.String));

                        //Dt.Columns.Add("DateDiff", typeof(System.String));

                        //Dt.Columns.Add("DateValidateAgainst", typeof(System.DateTime));

                        Dt.Columns.Add("Cheque_ValueDate", typeof(System.String));

                        foreach (DataRow row in Dt.Rows)
                        {

                            //split = row["payment_ref_num"].ToString().Split(new char[] { '|' });

                            //string[] splitDate = split[2].ToString().Split(new char[] { '-' });

                            //string[] splitTime = row["payment_log_date"].ToString().Split(new char[] { ' ' });
                            //DateTime dtPaymentDate = new DateTime(Convert.ToInt32(splitDate[2]), Convert.ToInt32(splitDate[1]), Convert.ToInt32(splitDate[0]));
                            //row["payment_log_date"] = dtPaymentDate.ToString("MM/dd/yyyy");
                            //if (splitDate.Count() > 2 && splitTime.Count() > 0)
                            //{



                            //    //CultureInfo culture = new CultureInfo("en-US");
                            //    dtPaymentDate = Convert.ToDateTime(String.Format("{0} {1} {2}", dtPaymentDate.ToString("dd-MM-yyyy"), splitTime[1], splitTime[2]));
                            //    int dateDiff = dtPaymentDate.Subtract(dateTimePicker1.Value).Days;
                            //    row["payment_log_date"] = dtPaymentDate.ToString("MM/dd/yyyy");
                            //    row["DateDiff"] = Math.Abs(dateDiff);
                            //    row["DateValidateAgainst"] = dateTimePicker1.Value;
                            //}


                            if (row["payment_method_name"].ToString() == "Other Bank Cheque" || row["payment_method_name"].ToString() == "Other Bank Cheque/Dr")
                            {
                                //Own Bank Cheque
                                row["Cheque_Status"] = "Pending";

                            }
                            else
                            {
                                row["Cheque_Status"] = "Cleared";
                                row["Cheque_ValueDate"] = row["payment_log_date"];
                            }
                        }

                        Dt.AcceptChanges();

                        label4.Text = Dt.Rows.Count + "  Rows to be Imported ";

                        Dt.DefaultView.Sort = "revenue_code ASC";

                        gridControl1.DataSource = Dt;

                        gridView1.BestFitColumns();

                        gridView1.GroupFooterShowMode = DevExpress.XtraGrid.Views.Grid.GroupFooterShowMode.VisibleAlways;
                        //GridGroupSummaryItem item = new GridGroupSummaryItem();
                        //item.FieldName = "payment_ref_num";
                        //item.SummaryType = DevExpress.Data.SummaryItemType.Count;
                        //gridView1.GroupSummary.Add(item);

                        GridGroupSummaryItem item2 = new GridGroupSummaryItem() { FieldName = "Cheque_Status", SummaryType = DevExpress.Data.SummaryItemType.Count };
                        gridView1.GroupSummary.Add(item2);
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.Message, "Error - loadPreview", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

                if (Dt.Rows.Count > 0)
                {
                    bttnPreview.Enabled = false;
                    bttnImport.Enabled = true;
                    bttnBrowse.Enabled = false;
                }
                else
                {
                    bttnPreview.Enabled = true;
                    bttnImport.Enabled = false;
                    bttnBrowse.Enabled = false;
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
            //if (dateTimePicker1.Value >= DateTime.Now.Date)
            //{
            //    Common.setMessageBox("Transaction Date Must be Less than Today's Date", Program.ApplicationName, 3);
            //    return;
            //}
            //else           
            if (_backupdb.doBackUp("TaxSmartSuiteRevised") == "Done")
            {
                bool bResponse = false;
                bool isError = false;
                using (WaitDialogForm form = new WaitDialogForm())
                {
                    //strDate = GetValidateionDate;
                    //if (string.IsNullOrEmpty(strDate))
                    //    return;
                    //if (!ValidateDate()) return;
                    if (CheuqeOption == "0001")
                    {//import normal paydirect record
                        try
                        {
                            strDate = GetValidateionDate;
                            if (string.IsNullOrEmpty(strDate))
                                return;
                            if (!ValidateDate()) return;
                            using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
                            {
                                connect.Open();
                                command = new SqlCommand("insertCollectionReportPay", connect) { CommandType = CommandType.StoredProcedure };
                                command.Parameters.Add(new SqlParameter("@CollectionReport_Temp", SqlDbType.Structured)).Value = Dt;
                                command.Parameters.Add(new SqlParameter("@User", SqlDbType.VarChar)).Value = user;
                                command.Parameters.Add(new SqlParameter("@ValidateDate", SqlDbType.VarChar)).Value = strDate;
                                //command.Parameters.Add(new SqlParameter("@DateValidate", SqlDbType.DateTime)).Value = dateTimePicker1.Value;
                                command.CommandTimeout = 10000;
                                using (System.Data.DataSet ds = new System.Data.DataSet())
                                {
                                    adp = new SqlDataAdapter(command);
                                    adp.Fill(ds);
                                    Dts = ds.Tables[0];
                                    connect.Close();

                                    if (ds.Tables[0].Rows[0]["returnCode"].ToString() == "02")
                                    {
                                        Common.setMessageBox(ds.Tables[0].Rows[0]["returnMessage"].ToString(), Program.ApplicationName, 3);
                                        isError = true;
                                        goto Map;
                                    }
                                    bResponse = true;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            bResponse = false;
                            MessageBox.Show(ex.Message, " CSV Import ", MessageBoxButtons.OK, MessageBoxIcon.Error); return;
                        }
                    }
                    if (CheuqeOption.ToString() == "0003")
                    {
                        //update cheque bank status in the collection report table
                        try
                        {
                            strDate = GetValidateionDate;
                            if (string.IsNullOrEmpty(strDate))
                                return;
                            using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
                            {
                                connect.Open();
                                command = new SqlCommand("UpdateCollectionChequeStatus", connect) { CommandType = CommandType.StoredProcedure };
                                command.Parameters.Add(new SqlParameter("@CollectionDownloadCredentials_Temp", SqlDbType.Structured)).Value = Dt;
                                command.Parameters.Add(new SqlParameter("@User", SqlDbType.VarChar)).Value = user;
                                command.Parameters.Add(new SqlParameter("@OnlineCurrentDate", SqlDbType.VarChar)).Value = strDate;//dateTimePicker1.Value.ToString("MM/dd/yyyy");

                                command.CommandTimeout = 10000;
                                using (System.Data.DataSet ds = new System.Data.DataSet())
                                {
                                    adp = new SqlDataAdapter(command);
                                    adp.Fill(ds);
                                    //if (ds.Tables[0].Rows[0]["returnCode"].ToString() == "-1")
                                    //    MessageBox.Show(ds.Tables[0].Rows[0]["returnMessage"].ToString(), " CSV Import ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    Dts = ds.Tables[0];
                                    connect.Close();

                                    if (ds.Tables[0].Rows[0]["returnCode"].ToString() != "00")
                                    {
                                        if (ds.Tables[0].Rows[0]["returnCode"].ToString() == "04")
                                            Common.setMessageBox(ds.Tables[0].Rows[0]["returnMessage"].ToString().Replace("###", dateTimePicker1.Value.ToLongDateString()), Program.ApplicationName, 3);
                                        else
                                            Common.setMessageBox(ds.Tables[0].Rows[0]["returnMessage"].ToString(), Program.ApplicationName, 3);
                                        return;
                                    }

                                    bResponse = true;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            bResponse = false;
                            MessageBox.Show(ex.Message, " CSV Import ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }

                }
            Map:
                if (isError)
                {
                    using (FrmStationMap frmStationMap = new FrmStationMap())
                    {
                        frmStationMap.ShowDialog();
                    }
                }
                else
                {
                    if (bResponse && Dts.Rows[0][0].ToString() == "00")
                    {
                        MessageBox.Show(Dts.Rows[0]["returnMessage"].ToString(), " CSV Import ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    else
                    {
                        MessageBox.Show("Error...", " CSV Import ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

            }
            else
            {
                Common.setMessageBox("Could not create backup", Program.ApplicationName, 3);
                return;
            }
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

        void RemoveColumn(DataTable Dt, string fieldName)
        {
            if (string.IsNullOrEmpty(fieldName)) return;
            if (Dt != null && Dt.Columns.Count > 0)
            {
                DataColumn Dc1 = null;
                foreach (DataColumn Dc in Dt.Columns)
                {
                    if (Dc.ColumnName == fieldName)
                    {
                        Dc1 = Dc;
                        break;
                    }
                }
                if (Dc1 != null)
                {
                    Dt.Columns.Remove(Dc1);
                }
            }
        }

        string GetValidateionDate
        {
            get
            {
                string strDate = null;
                try
                {
                    var dtDate = new Collection.Reemsonline.GetNewDate();
                    strDate = dtDate.GetDate();
                    //strDate = "12/04/2011";
                }
                catch (Exception ex)
                {
                    //Tripous.Sys.ErrorBox(ex);
                    Common.setMessageBox(String.Format("Please check your internet connection.\n{0}", ex.Message), Program.ApplicationName, 3);
                    strDate = null;
                }
                return strDate;
            }
        }

        bool ValidateDate()
        {
            bool bResponse = false;
            if (string.IsNullOrEmpty(strDate)) return false;
            foreach (DataRow item in Dt.Rows)
            {
                //DateTimeStyles.)
                if (item == null) continue;
                //DateTime d1 = ConvertToDateTime(item["payment_log_date"].ToString());
                //DateTime d2 = ConvertToDateTime(strDate);
                ////int lol = MosesClassLibrary.Utilities.DateFormat.DateDiff(DateTime.Parse(item["payment_log_date"].ToString()), DateTime.Parse(strDate));
                //Console.WriteLine(d1);
                //Console.WriteLine(d2);
                //Console.WriteLine(d1.Subtract(d2).TotalDays);
                if (ConvertToDateTime(item["payment_log_date"].ToString()).Subtract(ConvertToDateTime(strDate)).TotalDays < 0)
                    bResponse = true;
                else
                {
                    Common.setMessageBox(String.Format("Transaction Payment Date must be less than {0}.", ConvertToDateTime(strDate).ToLongDateString()), Program.ApplicationName, 3);
                    bResponse = false;
                    break;
                }
            }
            return bResponse;
        }

        DateTime ConvertToDateTime(string strdate)
        {
            if (string.IsNullOrEmpty(strdate)) return new DateTime(1, 1, 1);
            DateTime dtReturn = new DateTime(1, 1, 1);
            string[] split = strdate.Split(new char[] { '/' });
            if (split.Count() > 0)
            {
                try
                {
                    dtReturn = new DateTime(Convert.ToInt32(split[2]), Convert.ToInt32(split[0]), Convert.ToInt32(split[1]));
                }
                catch
                {
                }
            }
            return dtReturn;
        }

        void ReformatAmount(DataTable Dt)
        {
            try
            {
                //Dt.BeginInit();
                //if (Dt.Columns["Amount"].DataType != typeof(System.Decimal))
                //    Dt.Columns["Amount"].DataType = typeof(System.Decimal);
                //Dt.EndInit();
                if (Dt != null && Dt.Rows.Count > 0)
                {
                    foreach (DataRow row in Dt.Rows)
                    {
                        if (row == null || string.IsNullOrEmpty(row["payment ref# number"].ToString())) continue;
                        //if (row["payment ref# number"].ToString() == null) continue;
                        var vAmount = row["Amount"].ToString();
                        var vPattern = @"[^0-9.]"; //[^0-9.]
                        var vNumber = System.Text.RegularExpressions.Regex.Replace(vAmount, vPattern, "");
                        //var number = Convert.ToInt32((currency.Replace(pattern,"")));
                        row["Amount"] = vNumber;
                    }
                }
            }
            catch (Exception ex)
            {
                Tripous.Sys.ErrorBox(ex);
            }
            Dt.AcceptChanges();
        }

        void ReformatAmount2(DataTable Dt)
        {
            try
            {
                //Dt.BeginInit();
                //if (Dt.Columns["Amount"].DataType != typeof(System.Decimal))
                //    Dt.Columns["Amount"].DataType = typeof(System.Decimal);
                //Dt.EndInit();
                if (Dt != null && Dt.Rows.Count > 0)
                {
                    foreach (DataRow row in Dt.Rows)
                    {
                        if (row == null || string.IsNullOrEmpty(row["payment_ref_num"].ToString())) continue;
                        //if (row["payment ref# number"].ToString() == null) continue;
                        var vAmount = row["payment_amount"].ToString();
                        var vPattern = @"[^0-9.]"; //[^0-9.]
                        var vNumber = System.Text.RegularExpressions.Regex.Replace(vAmount, vPattern, "");
                        //var number = Convert.ToInt32((currency.Replace(pattern,"")));
                        row["payment_amount"] = vNumber;
                    }
                }
            }
            catch (Exception ex)
            {
                Tripous.Sys.ErrorBox(ex);
            }
            Dt.AcceptChanges();
        }

    }
}
