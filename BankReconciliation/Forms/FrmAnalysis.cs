using BankReconciliation.Class;
using BankReconciliation.Report;
using DevExpress.Utils;
using DevExpress.XtraSplashScreen;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TaxSmartSuite.Class;
using DevExpress.XtraReports.UI.PivotGrid;
using DevExpress.XtraPivotGrid;
using DevExpress.XtraPrinting;
using DevExpress.XtraReports.UI;

namespace BankReconciliation.Forms
{
    public partial class FrmAnalysis : Form
    {
        public static FrmAnalysis publicStreetGroup;

        protected TransactionTypeCode iTransType;

        protected bool boolIsUpdate;

        protected int ID; private SqlCommand _command; private SqlDataAdapter adp;

        System.Data.DataSet DtSet;

        DataTable Dt;

        private System.Data.OleDb.OleDbDataAdapter MyCommand;

        private OleDbConnection MyConnection;

        string filenamesopen, acctnumber; bool boolIsUpdate2;

        bool isFirst = true;
        public FrmAnalysis()
        {
            SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

            InitializeComponent();

            publicStreetGroup = this;

            setImages();

            ToolStripEvent();

            iTransType = TransactionTypeCode.Null;

            bttnImport.Click += bttnImport_Click;

            bttnUpdateExcel.Click += bttnUpdateExcel_Click;

            bttnprint.Click += bttnprint_Click;

            Load += OnFormLoad;

            SplashScreenManager.CloseForm(false);
        }

        void bttnprint_Click(object sender, EventArgs e)
        {
           

            xtrarepPayAnalysis analysis = new xtrarepPayAnalysis();

            analysis.ShowPreviewDialog();
            
        }

        void bttnUpdateExcel_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
                {
                    connect.Open();
                    _command = new SqlCommand("dopayAnalysis", connect) { CommandType = CommandType.StoredProcedure };
                    //_command.Parameters.Add(new SqlParameter("@bankcode", SqlDbType.VarChar)).Value = strbankcode;
                    //_command.Parameters.Add(new SqlParameter("@startdate", SqlDbType.VarChar)).Value = string.Format("{0:yyyy/MM/dd}", dtpStart.Value);
                    //_command.Parameters.Add(new SqlParameter("@enddate", SqlDbType.VarChar)).Value = string.Format("{0:yyyy/MM/dd}", dtpEnd.Value);
                    _command.Parameters.Add(new SqlParameter("@pTransaction", SqlDbType.Structured)).Value = Dt;
                    //_command.Parameters.Add(new SqlParameter("@Period", SqlDbType.Char)).Value = label22.Text;
                    //_command.Parameters.Add(new SqlParameter("@AccountID", SqlDbType.Char)).Value = Convert.ToInt32(cboAccount.SelectedValue);
                    //@Years
                    _command.CommandTimeout = 0;
                    using (System.Data.DataSet ds = new System.Data.DataSet())
                    {
                        ds.Clear();
                        //Dts = new DataTable();
                        //Dts.Clear();
                        adp = new SqlDataAdapter(_command);
                        adp.Fill(ds);
                        //Dts = ds.Tables[0];
                        connect.Close();

                        if (ds.Tables[0].Rows[0]["returnCode"].ToString() != "00")
                        {
                            Common.setMessageBox(ds.Tables[0].Rows[0]["returnMessage"].ToString(), Program.ApplicationName, 2);

                            return;

                        }
                        else
                        {
                            Common.setMessageBox(ds.Tables[0].Rows[0]["returnMessage"].ToString(), Program.ApplicationName, 2);
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                Tripous.Sys.ErrorBox(String.Format("{0}{1}", ex.Message, ex.StackTrace)); return;
            }
        }

        void bttnImport_Click(object sender, EventArgs e)
        {
            MessageBoxManager.Unregister();
            //MessageBoxManager.OK = "Excel 2003";
            MessageBoxManager.No = "Excel 2007";
            MessageBoxManager.Yes = "Excel 2003";
            MessageBoxManager.Register();

            DialogResult result = MessageBox.Show("Select Excel File Type", "Import Statement", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)//excele 2003
            {
                try
                {
                    SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

                    gridControl1.DataSource = null;

                    using (OpenFileDialog openFileDialogCSV = new OpenFileDialog() { InitialDirectory = Application.ExecutablePath, Filter = "Excel files (*.xls)|*.xls|All files (*.*)|*.*", FilterIndex = 1, RestoreDirectory = true })
                    {

                        //openFileDialogCSV.ShowDialog();
                        if (openFileDialogCSV.ShowDialog() == DialogResult.OK)
                        {

                            if (openFileDialogCSV.FileName.Length > 0)
                            {
                                filenamesopen = openFileDialogCSV.FileName;
                            }

                            try
                            {
                                Dt = new DataTable();
                                Dt.Clear();
                                Dt.BeginInit();
                                Dt.Columns.Add("DATE", typeof(DateTime));
                                Dt.Columns.Add("PAYER", typeof(string));
                                Dt.Columns.Add("AMOUNT", typeof(decimal));
                                //Dt.Columns.Add("BALANCE", typeof(decimal));
                                Dt.EndInit();

                                MyConnection = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" +
                                               filenamesopen + ";Extended Properties=\"Excel 8.0;HDR=YES;\"");

                                MyCommand = new System.Data.OleDb.OleDbDataAdapter("select * from [Sheet1$]", MyConnection);
                                DtSet = new System.Data.DataSet();
                                DtSet.Clear();
                                MyCommand.Fill(DtSet, "[Sheet1$]");
                                //MyCommand.Fill(Dt);

                                int j = 0;
                                foreach (DataRow row in DtSet.Tables[0].Rows)
                                {
                                    j = j + 1;
                                    //DataRow rw = new DataRow();
                                    if (!(row["DATE"] is DBNull) && (row["DATE"] != ""))
                                    {
                                        var rw = Dt.NewRow();
                                        rw["DATE"] = row["DATE"];

                                        if (!(row["AMOUNT"] is DBNull))
                                        {
                                            if (!Logic.isDeceimalFormat((string)row["AMOUNT"].ToString()))
                                            {
                                                Common.setMessageBox(string.Format("Incorrect Data format encountered. Correct {0} in Line No. {1} and try again.", row["AMOUNT"], j), "Import Data Error", 3); return;
                                            }
                                            else
                                            {
                                                rw["PAYER"] = row["PAYER"];
                                                rw["AMOUNT"] = row["AMOUNT"] is DBNull ? 0m : row["AMOUNT"] == string.Empty ? 
                                                                                                  0m : 
                                                                                                  Convert.ToDecimal(row["AMOUNT"]);

                                                
                                                //rw["CREDIT"] = row["CREDIT"] is DBNull ? 0m : Convert.ToDecimal(row["CREDIT"]);
                                            }


                                        }
                                        //else if (!(row["CREDIT"] is DBNull))
                                        //{


                                        //    if (!Logic.isDeceimalFormat((string)row["CREDIT"].ToString()))
                                        //    {
                                        //        //var rw = Dt.NewRow();

                                        //        Common.setMessageBox(string.Format("Incorrect Data format encountered. Correct {0} in Line No. {1} and try again.", row["CREDIT"], j), "Import Data Error", 3); return;
                                        //    }
                                        //    else
                                        //    {
                                        //        rw["CREDIT"] = row["CREDIT"] is DBNull ? 0m : Convert.ToDecimal(row["CREDIT"]);
                                        //        rw["DEBIT"] = row["DEBIT"] is DBNull ? 0m : row["DEBIT"] == string.Empty ? 0m : Convert.ToDecimal(row["DEBIT"]);

                                        //        rw["BALANCE"] = row["BALANCE"] is DBNull ? 0m : Convert.ToDecimal(row["BALANCE"]);

                                        //    }
                                        //}
                                        else
                                        {
                                            //rw["CREDIT"] = row["CREDIT"] is DBNull ? 0m : Convert.ToDecimal(row["CREDIT"]);
                                            //rw["DEBIT"] = row["DEBIT"] is DBNull ? 0m : row["DEBIT"] == string.Empty ? 0m : Convert.ToDecimal(row["DEBIT"]);
                                            rw["PAYER"] = row["PAYER"];
                                            rw["AMOUNT"] = row["AMOUNT"] is DBNull ? 0m : Convert.ToDecimal(row["AMOUNT"]);
                                        }

                                        Dt.Rows.Add(rw);


                                    }

                                }


                                for (int h = 0; h < Dt.Rows.Count; h++)
                                {
                                    if (Dt.Rows[h].IsNull(0) == true)
                                    {
                                        Dt.Rows[h].Delete();
                                    }
                                }

                                Dt.AcceptChanges();
                                //SearchPrevisonRecord();

                                gridControl1.DataSource = Dt;
                                gridView1.OptionsBehavior.Editable = false;
                                MyConnection.Close();
                            }
                            catch (Exception ex)
                            {
                                Common.setMessageBox("Modify the excel to contaim Column Header 'DATE','PAYER','AMOUNT'", "Import Data Error", 3);
                                gridControl1.DataSource = null;
                                return;
                            }
                            //ChangeValue(Dt);


                            //gridView1.BestFitColumns();
                            gridView1.Columns["AMOUNT"].DisplayFormat.FormatType = FormatType.Numeric;
                            gridView1.Columns["AMOUNT"].DisplayFormat.FormatString = "n2";
                            //gridView1.Columns["CREDIT"].DisplayFormat.FormatType = FormatType.Numeric;
                            //gridView1.Columns["CREDIT"].DisplayFormat.FormatString = "n2";
                            //gridView1.Columns["BALANCE"].DisplayFormat.FormatType = FormatType.Numeric;
                            //gridView1.Columns["BALANCE"].DisplayFormat.FormatString = "n2";

                            gridView1.Columns["AMOUNT"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                            //gridView1.Columns["DEBIT"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                            //gridView1.Columns["BALANCE"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;

                            gridView1.Columns["AMOUNT"].SummaryItem.FieldName = "AMOUNT";
                            gridView1.Columns["AMOUNT"].SummaryItem.DisplayFormat = "Total = {0:n}";

                            //gridView1.Columns["CREDIT"].SummaryItem.FieldName = "CREDIT";
                            //gridView1.Columns["CREDIT"].SummaryItem.DisplayFormat = "Total = {0:n}";

                            //gridView1.Columns["DEBIT"].SummaryItem.FieldName = "DEBIT";
                            //gridView1.Columns["DEBIT"].SummaryItem.DisplayFormat = "Total = {0:n}";

                            //gridView1.OptionsView.ColumnAutoWidth = false;
                            gridView1.OptionsView.ShowFooter = true;

                            gridView1.BestFitColumns();

                            label2.Text = Dt.Rows.Count + " Rows of Records ";
                        }
                        else
                        {
                            Common.setMessageBox("Operation Cancel", "Import Cancel", 3); return;
                        }


                    }
                }
                catch (Exception ex)
                {
                    Common.setMessageBox(ex.StackTrace + ex.Message, "Error During Import", 2); return;
                }
                finally
                {
                    SplashScreenManager.CloseForm(false);
                }
            }

            if (result == DialogResult.No)//excele 2007
            {
                //MessageBox.Show("You clicked the yes button");
                //new OpenFileDialog().ShowDialog();

                try
                {
                    SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

                    //exportToExcel();

                    gridControl1.DataSource = null;

                    using (OpenFileDialog openFileDialogCSV = new OpenFileDialog() { InitialDirectory = Application.ExecutablePath, Filter = "Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*", FilterIndex = 1, RestoreDirectory = true })
                    {

                        //openFileDialogCSV.ShowDialog();
                        if (openFileDialogCSV.ShowDialog() == DialogResult.OK)
                        {
                            if (openFileDialogCSV.FileName.Length > 0)
                            {
                                filenamesopen = openFileDialogCSV.FileName;
                            }
                            //try
                            //{
                            Dt = new DataTable();
                            Dt.BeginInit();
                            Dt.Columns.Add("DATE", typeof(DateTime));
                            Dt.Columns.Add("PAYER", typeof(decimal));
                            Dt.Columns.Add("AMOUNT", typeof(decimal));
                            //Dt.Columns.Add("BALANCE", typeof(decimal));
                            Dt.EndInit();

                            Dt.Clear();
                            MyConnection = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" +
                                           filenamesopen + ";Extended Properties=\"Excel 8.0;HDR=YES;\"");

                            MyCommand = new System.Data.OleDb.OleDbDataAdapter("select * from [Sheet1$]", MyConnection);
                            DtSet = new System.Data.DataSet(); DtSet.Clear();
                            MyCommand.Fill(DtSet, "[Sheet1$]");

                            int j = 0;

                            foreach (DataRow row in DtSet.Tables[0].Rows)
                            {
                                j = j + 1;
                                //DataRow rw = new DataRow();
                                if (!(row["DATE"] is DBNull) && (row["DATE"] != ""))
                                {
                                    var rw = Dt.NewRow();
                                    rw["DATE"] = row["DATE"];

                                    if (!(row["AMOUNT"] is DBNull))
                                    {
                                        if (!Logic.isDeceimalFormat((string)row["AMOUNT"].ToString()))
                                        {
                                            Common.setMessageBox(string.Format("Incorrect Data format encountered. Correct {0} in Line No. {1} and try again.", row["AMOUNT"], j), "Import Data Error", 3); return;
                                        }
                                        else
                                        {
                                            rw["PAYER"] = row["PAYER"];
                                            rw["AMOUNT"] = row["AMOUNT"] is DBNull ? 0m : row["AMOUNT"] == string.Empty ?
                                                                                              0m :
                                                                                              Convert.ToDecimal(row["AMOUNT"]);


                                            //rw["CREDIT"] = row["CREDIT"] is DBNull ? 0m : Convert.ToDecimal(row["CREDIT"]);
                                        }


                                    }
                                    //else if (!(row["CREDIT"] is DBNull))
                                    //{


                                    //    if (!Logic.isDeceimalFormat((string)row["CREDIT"].ToString()))
                                    //    {
                                    //        //var rw = Dt.NewRow();

                                    //        Common.setMessageBox(string.Format("Incorrect Data format encountered. Correct {0} in Line No. {1} and try again.", row["CREDIT"], j), "Import Data Error", 3); return;
                                    //    }
                                    //    else
                                    //    {
                                    //        rw["CREDIT"] = row["CREDIT"] is DBNull ? 0m : Convert.ToDecimal(row["CREDIT"]);
                                    //        rw["DEBIT"] = row["DEBIT"] is DBNull ? 0m : row["DEBIT"] == string.Empty ? 0m : Convert.ToDecimal(row["DEBIT"]);

                                    //        rw["BALANCE"] = row["BALANCE"] is DBNull ? 0m : Convert.ToDecimal(row["BALANCE"]);

                                    //    }
                                    //}
                                    else
                                    {
                                        //rw["CREDIT"] = row["CREDIT"] is DBNull ? 0m : Convert.ToDecimal(row["CREDIT"]);
                                        //rw["DEBIT"] = row["DEBIT"] is DBNull ? 0m : row["DEBIT"] == string.Empty ? 0m : Convert.ToDecimal(row["DEBIT"]);
                                        rw["PAYER"] = row["PAYER"];
                                        rw["AMOUNT"] = row["AMOUNT"] is DBNull ? 0m : Convert.ToDecimal(row["AMOUNT"]);
                                    }

                                    Dt.Rows.Add(rw);


                                }

                            }


                            for (int h = 0; h < Dt.Rows.Count; h++)
                            {
                                if (Dt.Rows[h].IsNull(0) == true)
                                {
                                    Dt.Rows[h].Delete();
                                }
                            }

                            Dt.AcceptChanges();
                            //SearchPrevisonRecord();

                            gridControl1.DataSource = Dt;
                            gridView1.OptionsBehavior.Editable = false;
                            MyConnection.Close();
                            //}
                            //catch (Exception ex)
                            //{
                            //    //string.Format("Column name 'DEBIT' or 'CREDIT' is not in table [Sheet1$]. Correct the Excel Sheet Column name format.Try again");
                            //    Common.setMessageBox("Column name 'DEBIT' or 'CREDIT' is not in table [Sheet1$]. Correct the Excel Sheet Column name format.Try again", Program.ApplicationName, 2);
                            //    gridControl1.DataSource = null;
                            //    return;
                            //}
                            //ChangeValue(Dt);



                            gridView1.Columns["AMOUNT"].DisplayFormat.FormatType = FormatType.Numeric;
                            gridView1.Columns["AMOUNT"].DisplayFormat.FormatString = "n2";
                            //gridView1.Columns["CREDIT"].DisplayFormat.FormatType = FormatType.Numeric;
                            //gridView1.Columns["CREDIT"].DisplayFormat.FormatString = "n2";
                            //gridView1.Columns["BALANCE"].DisplayFormat.FormatType = FormatType.Numeric;
                            //gridView1.Columns["BALANCE"].DisplayFormat.FormatString = "n2";
                            //gridView1.Columns["BALANCE"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;

                            gridView1.Columns["AMOUNT"].SummaryItem.FieldName = "AMOUNT";
                            gridView1.Columns["AMOUNT"].SummaryItem.DisplayFormat = "Total = {0:n}";

                            gridView1.Columns["AMOUNT"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                            //gridView1.Columns["AMOUNT"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;

                            //gridView1.Columns["CREDIT"].SummaryItem.FieldName = "CREDIT";
                            //gridView1.Columns["CREDIT"].SummaryItem.DisplayFormat = "Total = {0:n}";

                            //gridView1.Columns["DEBIT"].SummaryItem.FieldName = "DEBIT";
                            //gridView1.Columns["DEBIT"].SummaryItem.DisplayFormat = "Total = {0:n}";

                            //gridView1.OptionsView.ColumnAutoWidth = false;
                            gridView1.OptionsView.ShowFooter = true;

                            gridView1.BestFitColumns();
                            label2.Text = Dt.Rows.Count + " Rows of Records ";
                        }
                        else
                        {
                            Common.setMessageBox("Operation Cancel", "Import Cancel", 3); return;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Common.setMessageBox("Modify the excel to contaim Column Header 'DATE','PAYER','AMOUNT'", "Import Data Error", 3);
                }
                finally
                {
                    SplashScreenManager.CloseForm(false);
                }
            }

            bttnUpdateExcel.Enabled = true;
            MessageBoxManager.Unregister();


        }

        private void setImages()
        {
            tsbNew.Image = MDIMains.publicMDIParent.i16x16.Images[5];
            tsbEdit.Image = MDIMains.publicMDIParent.i16x16.Images[6];
            tsbDelete.Image = MDIMains.publicMDIParent.i16x16.Images[7];
            tsbReload.Image = MDIMains.publicMDIParent.i16x16.Images[0];
            tsbClose.Image = MDIMains.publicMDIParent.i16x16.Images[11];

            //bttnCancel.Image = MDIMains.publicMDIParent.i32x32.Images[9];
            //bttnReset.Image = MDIMains.publicMDIParent.i32x32.Images[8];
            //bttnUpdate.Image = MDIMain.publicMDIParent.i32x32.Images[7];
            //bttnUpdate.Image = MDIMains.publicMDIParent.i32x32.Images[7];

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
                MDIMains.publicMDIParent.RemoveControls();
            }
            else if (sender == tsbNew)
            {
                //groupControl2.Text = "Add New Record";
                //iTransType = TransactionTypeCode.New;
                //ShowForm();
                //boolIsUpdate = false;
            }
            else if (sender == tsbEdit)
            {
                //groupControl2.Text = "Edit Record Mode";
                //iTransType = TransactionTypeCode.Edit;
                //if (EditRecordMode())
                //{
                //    ShowForm();
                //    boolIsUpdate = true;
                //}
            }
            else if (sender == tsbDelete)
            {
                //groupControl2.Text = "Disable Record Mode";
                //iTransType = TransactionTypeCode.Delete;
                //if (MosesClassLibrary.Utilities.Common.AskQuestion("Disable this record will delete attached record.\nDo you want to continue?", ""))
                //{
                //    if (string.IsNullOrEmpty(ID.ToString()))
                //    {
                //        Common.setMessageBox("No Record Selected for Disable", Program.ApplicationName, 3);
                //        return;
                //    }
                //    else
                //        //deleteRecord(ID);
                //}
                //else
                //    tsbReload.PerformClick();
                //boolIsUpdate = false;
            }
            else if (sender == tsbReload)
            {
                iTransType = TransactionTypeCode.Reload;
                //ShowForm();
            }
            //bttnReset.PerformClick();
        }

        void OnFormLoad(object sender, EventArgs e)
        {
            //ShowForm();

            //setDBComboBoxTn();
            isFirst = false;


        }

        public void RefreshForm()
        {
            NavBars.ToolStripEnableDisableControls(toolStrip, Tag as String);
        }


    }
}
