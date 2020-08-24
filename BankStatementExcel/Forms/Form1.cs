using DevExpress.Utils;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.DXErrorProvider;
using DevExpress.XtraGrid.Views.Grid;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using BankStatementExcel.Forms;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraExport;
using DevExpress.XtraPrinting;
using DevExpress.XtraSplashScreen;
using MosesClassLibrary.Utilities;
using Formatting = Newtonsoft.Json.Formatting;

namespace BankStatementExcel
{
    public partial class Form1 : Form
    {
        DataTable Dt;
        string query = string.Empty;
        string query2 = string.Empty;
        DataTable dt = new DataTable();
        //System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();
        DateTime timer = new DateTime();
        int interval = 1; //Minutes;
        private List<ClassObject> _classObject = new List<ClassObject>();
        List<SearchDirectory> searchDirectory = new List<SearchDirectory>();
        private SearchDirectory currentDirectory;
        private string defaultFolder = @"c:\programdata\ICMA\recon_utilies\";
        private string defaultFilename = "DefaultDirectory.json";
        RepositoryItemSpinEdit _itemSpinEdit = new RepositoryItemSpinEdit();
        private SqlCommand _command; private SqlDataAdapter adp;
        private string filenamesopen;
        public Form1()
        {
            InitializeComponent();

            //setDBComboBox();

            gridView1.CellValueChanged += gridView1_CellValueChanged;

            btnload.Click += btnload_Click;

            btnSave.Click += btnSave_Click;

            btnExport.Click += btnExport_Click;

            btnClear.Click += btnClear_Click;

            gridView1.ValidatingEditor += gridView1_ValidatingEditor;

            gridView1.InvalidRowException += gridView1_InvalidRowException;

            gridView1.RowUpdated += gridView1_RowUpdated; gridView1.ValidateRow += gridView1_ValidateRow;

            btnImport.Click += btnImport_Click;

            load();

            InitialiseSpinEdit(); btnClose.Click += btnClose_Click;

            gridControl1.Enabled = false;

        }

        void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        void btnImport_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(cboBank.SelectedValue.ToString()))
            {

                MessageBox.Show("Bank Name", Program.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            else if (string.IsNullOrEmpty(dtpStart.EditValue.ToString()) && string.IsNullOrEmpty(dtpEnd.EditValue.ToString()))
            {
                MessageBox.Show("Date Range is Empty", Program.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            else
            {
                try
                {
                    SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

                    this.Hide();
                    FrmImport import = new FrmImport(cboBank.SelectedValue.ToString(), dtpStart.DateTime,
                        dtpEnd.DateTime, cboBank.Text.Trim().ToString());
                    import.ShowDialog();
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message + ex.StackTrace, Program.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                finally
                {
                    SplashScreenManager.CloseForm(false);
                }
            }
        }

        void btnClear_Click(object sender, EventArgs e)
        {
            cboBank.SelectedIndex = -1;
            dtpStart.EditValue = string.Empty;
            dtpEnd.EditValue = string.Empty;
            classObjectBindingSource.DataSource = null;
            _classObject = null;
            searchDirectory = null;
            currentDirectory = null;
        }

        void btnExport_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.InitialDirectory = Convert.ToString(Environment.SpecialFolder.MyDocuments);
            saveFileDialog1.Filter = "Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 1;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                gridView1.OptionsPrint.UsePrintStyles = false;
                gridView1.ExportToXlsx(saveFileDialog1.FileName);

            }

            MessageBox.Show(string.Format("Export to Excel SuccessFull!, you can find the file in {0}", saveFileDialog1.FileName));
        }

        void InitialiseSpinEdit()
        {
            _itemSpinEdit = new RepositoryItemSpinEdit();
            _itemSpinEdit.EditMask = "n2";
            _itemSpinEdit.Mask.BeepOnError = true;
            _itemSpinEdit.MinValue = 0m;
            _itemSpinEdit.MaxValue = decimal.MaxValue;
            _itemSpinEdit.Buttons.Clear();
            _itemSpinEdit.BorderStyle = BorderStyles.NoBorder;
        }

        void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(cboBank.SelectedValue.ToString()))
            {
                //Common.setEmptyField("Bank Name", Program.ApplicationName);
                MessageBox.Show("Bank Name", Program.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            else
            {
                try
                {
                    SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

                    var clsObject = classObjectBindingSource.DataSource as List<ClassObject>;

                    if (clsObject != null && clsObject.Any())
                    {
                        ProcessSaveEntry(clsObject);
                        ProcessSaveDirectory();
                        timer = DateTime.Now;
                    }
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message + ex.StackTrace, Program.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                finally
                {
                    SplashScreenManager.CloseForm(false);
                }

            }
        }

        void gridView1_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            //GridView view = sender as GridView;

            ////if (view.FocusedColumn.FieldName != "BALANCE") return;

            //int rowHandle = view.FocusedRowHandle;

            //if (view.FocusedColumn.FieldName == "BALANCE")
            //{
            //    if (rowHandle == 0)
            //    {
            //        var lol3 = view.GetRowCellValue(0, view.Columns["BALANCE"]);

            //        view.SetRowCellValue(0, view.Columns["BALANCE"], string.Format("{0:n}", view.GetRowCellValue(0, view.Columns["BALANCE"])));
            //    }
            //    else
            //    {
            //        double get = Convert.ToDouble(view.GetRowCellValue(Convert.ToInt32(view.FocusedRowHandle) - 1, view.Columns["BALANCE"]));

            //        double vals = Convert.ToDouble(view.GetRowCellValue(Convert.ToInt32(view.FocusedRowHandle) - 1, view.Columns["BALANCE"])) - Convert.ToDouble(view.GetRowCellValue(view.FocusedRowHandle, view.Columns["DEBIT"])) + Convert.ToDouble(view.GetRowCellValue(view.FocusedRowHandle, view.Columns["CREDIT"]));

            //        view.SetRowCellValue(view.FocusedRowHandle, view.Columns["BALANCE"], vals);
            //    }
            //}

            //if (view.FocusedColumn.FieldName == "CREDIT")
            //{
            //    double get = Convert.ToDouble(view.GetRowCellValue(Convert.ToInt32(view.FocusedRowHandle) - 1, view.Columns["BALANCE"]));

            //    double vals = Convert.ToDouble(view.GetRowCellValue(Convert.ToInt32(view.FocusedRowHandle) - 1, view.Columns["BALANCE"])) + Convert.ToDouble(view.GetRowCellValue(view.FocusedRowHandle, view.Columns["CREDIT"])) + Convert.ToDouble(view.GetRowCellValue(view.FocusedRowHandle, view.Columns["DEBIT"]));

            //    view.SetRowCellValue(view.FocusedRowHandle, view.Columns["BALANCE"], vals);

            //}



        }

        void ProcessLoadDirectory()
        {
            if (!Directory.Exists(defaultFolder)) Directory.CreateDirectory(defaultFolder);

            string filename = string.Format(@"{0}\{1}", defaultFolder, defaultFilename);

            if (File.Exists(filename))
            {
                searchDirectory = filename.DeserializeFromFile<SearchDirectory>().ToList();
            }
            else
                searchDirectory = new List<SearchDirectory>();
        }

        bool ProcessSaveDirectory()
        {
            SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

            if (searchDirectory == null || !searchDirectory.Any())
                return false;
            string filename = string.Format(@"{0}\{1}", defaultFolder, defaultFilename);
            return searchDirectory.SerializeToFile(filename);

            SplashScreenManager.CloseForm(false);
        }

        List<ClassObject> LoadClassObjects(string filename)
        {
            string folderPath = Path.Combine(defaultFolder, "Others");
            if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);
            string filepath = string.Format(@"{0}\{1}.json", folderPath, filename);
            if (File.Exists(filepath))
                return filepath.DeserializeFromFile<ClassObject>().ToList();
            else
                return new List<ClassObject>();
        }

        void ProcessNewEntry()
        {
            currentDirectory =
                searchDirectory.SingleOrDefault(
                    x =>
                        x.BankCode == cboBank.SelectedValue.ToString() &&
                        (x.StartDate >= dtpStart.DateTime && x.EndDate <= dtpEnd.DateTime));
            if (currentDirectory == null)
            {
                currentDirectory = new SearchDirectory();
                currentDirectory.ID = searchDirectory.Count + 1;
                currentDirectory.BankCode = cboBank.SelectedValue.ToString();
                currentDirectory.StartDate = dtpStart.DateTime;
                currentDirectory.EndDate = dtpEnd.DateTime;
                currentDirectory.Filename = Guid.NewGuid().ToString();
                _classObject = new List<ClassObject>();
                searchDirectory.Add(currentDirectory);
            }
            else
            {
                _classObject = LoadClassObjects(currentDirectory.Filename);
            }

            //_classObject = new List<ClassObject>();
            timer = DateTime.Now;
            classObjectBindingSource.DataSource = _classObject;

            gridControl1.ForceInitialize();
            gridView1.Columns["DEBIT"].ColumnEdit = _itemSpinEdit;
            //gridView1.Columns["DEBIT"].DisplayFormat.FormatString = "n2";

            gridView1.Columns["CREDIT"].ColumnEdit = _itemSpinEdit;
            //gridView1.Columns["CREDIT"].DisplayFormat.FormatString = "n2";

            gridView1.Columns["BALANCE"].ColumnEdit = _itemSpinEdit;
            //gridView1.Columns["BALANCE"].DisplayFormat.FormatString = "n2";

            gridView1.Columns["CREDIT"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            gridView1.Columns["DEBIT"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            gridView1.Columns["BALANCE"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;

            gridView1.Columns["BALANCE"].SummaryItem.FieldName = "BALANCE";
            gridView1.Columns["BALANCE"].SummaryItem.DisplayFormat = "Total = {0:n}";

            gridView1.Columns["CREDIT"].SummaryItem.FieldName = "CREDIT";
            gridView1.Columns["CREDIT"].SummaryItem.DisplayFormat = "Total = {0:n}";

            gridView1.Columns["DEBIT"].SummaryItem.FieldName = "DEBIT";
            gridView1.Columns["DEBIT"].SummaryItem.DisplayFormat = "Total = {0:n}";

            gridView1.OptionsView.ShowFooter = true;

            gridView1.BestFitColumns();
        }

        void btnload_Click(object sender, EventArgs e)
        {
            ProcessLoadDirectory();
            ProcessNewEntry();
            gridControl1.Enabled = true;
#if false
            if (string.IsNullOrEmpty(cboBank.SelectedValue.ToString()))
            {
                Common.setEmptyField("Bank Name", Program.ApplicationName);
                return;
            }
            else
            {
                using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
                {
                    connect.Open();
                    _command = new SqlCommand("dogetTempExcel", connect) { CommandType = CommandType.StoredProcedure };
                    _command.Parameters.Add(new SqlParameter("@BankShortCode", SqlDbType.VarChar)).Value = cboBank.SelectedValue;
                    _command.Parameters.Add(new SqlParameter("@StartDate", SqlDbType.VarChar)).Value = string.Format("{0:yyyy/MM/dd}", dtpStart.Value);
                    _command.Parameters.Add(new SqlParameter("@Enddate", SqlDbType.VarChar)).Value = string.Format("{0:yyyy/MM/dd}", dtpEnd.Value);
                    _command.CommandTimeout = 0;

                    using (System.Data.DataSet ds = new System.Data.DataSet())
                    {
                        ds.Clear();
                        adp = new SqlDataAdapter(_command);
                        adp.Fill(ds);
                        dt = ds.Tables[0];
                        connect.Close();

                    }
                }

                timer = DateTime.Now;
                gridControl1.DataSource = dt;

            }



            gridView1.Columns["DEBIT"].DisplayFormat.FormatType = FormatType.Numeric;
            gridView1.Columns["DEBIT"].DisplayFormat.FormatString = "n2";

            gridView1.Columns["CREDIT"].DisplayFormat.FormatType = FormatType.Numeric;
            gridView1.Columns["CREDIT"].DisplayFormat.FormatString = "n2";

            gridView1.Columns["BALANCE"].DisplayFormat.FormatType = FormatType.Numeric;
            gridView1.Columns["BALANCE"].DisplayFormat.FormatString = "n2";

            gridView1.Columns["CREDIT"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            gridView1.Columns["DEBIT"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            gridView1.Columns["BALANCE"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;

            gridView1.Columns["BALANCE"].SummaryItem.FieldName = "BALANCE";
            gridView1.Columns["BALANCE"].SummaryItem.DisplayFormat = "Total = {0:n}";

            gridView1.Columns["CREDIT"].SummaryItem.FieldName = "CREDIT";
            gridView1.Columns["CREDIT"].SummaryItem.DisplayFormat = "Total = {0:n}";

            gridView1.Columns["DEBIT"].SummaryItem.FieldName = "DEBIT";
            gridView1.Columns["DEBIT"].SummaryItem.DisplayFormat = "Total = {0:n}";

            gridView1.OptionsView.ShowFooter = true;

            gridView1.BestFitColumns();
#endif
        }

        bool ProcessSaveEntry(List<ClassObject> clsObject)
        {

            SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

            if (clsObject == null || !clsObject.Any())
                return false;
            string folderPath = Path.Combine(defaultFolder, "Others");
            if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);
            string filename = string.Format(@"{0}\{1}.json", folderPath, currentDirectory.Filename);
            return clsObject.SerializeToFile(filename);
            //using (FileStream fs = File.Open(/*@"C:\test.json"*/filename, FileMode.OpenOrCreate))
            //{
            //    using (StreamWriter sw = new StreamWriter(fs))
            //    {
            //        using (JsonWriter jw = new JsonTextWriter(sw))
            //        {
            //            jw.Formatting = Formatting.Indented;
            //            JsonSerializer serializer = new JsonSerializer();
            //            serializer.Serialize(jw, clsObject);
            //        }
            //    }
            //}
            SplashScreenManager.CloseForm(false);


        }

        void gridView1_RowUpdated(object sender, DevExpress.XtraGrid.Views.Base.RowObjectEventArgs e)
        {
            if (e.Row != null)
            {
                //var clsObject = gridControl1.DataSource as List<ClassObject>;
                var clsObject = classObjectBindingSource.DataSource as List<ClassObject>;

                if (clsObject != null && clsObject.Any())
                {
                    TimeSpan timeDiff = DateTime.Now - timer;
                    if (Convert.ToInt32(timeDiff.TotalMinutes) < interval)
                        return;
                    ProcessSaveEntry(clsObject);
                    ProcessSaveDirectory();
                    timer = DateTime.Now;

                }
#if false
                var dt = gridControl1.DataSource as DataTable;
                if (dt != null && dt.Rows.Count > 0)
                {
                    TimeSpan timeDiff = DateTime.Now - timer;
                    if (Convert.ToInt32(timeDiff.TotalMinutes) > interval)
                    {
                        using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
                        {
                            connect.Open();
                            _command = new SqlCommand("TempleExcel", connect) { CommandType = CommandType.StoredProcedure };
                            _command.Parameters.Add(new SqlParameter("@pTransaction", SqlDbType.Structured)).Value = dt;
                            _command.Parameters.Add(new SqlParameter("@BankShortCode", SqlDbType.VarChar)).Value = cboBank.SelectedValue;
                            _command.Parameters.Add(new SqlParameter("@StartDate", SqlDbType.VarChar)).Value = string.Format("{0:yyyy/MM/dd}", dtpStart.Value);
                            _command.Parameters.Add(new SqlParameter("@Enddate", SqlDbType.VarChar)).Value = string.Format("{0:yyyy/MM/dd}", dtpEnd.Value);
                            _command.Parameters.Add(new SqlParameter("@Type", SqlDbType.VarChar)).Value = "Auto";
                            _command.CommandTimeout = 0;

                            using (System.Data.DataSet ds = new System.Data.DataSet())
                            {
                                ds.Clear();
                                adp = new SqlDataAdapter(_command);
                                adp.Fill(ds);
                                //Dts = ds.Tables[0];
                                connect.Close();

                            }
                        }
                        timer = DateTime.Now;
                    }
                }
#endif

            }
        }

        void btnNew_Click(object sender, EventArgs e)
        {


        }

        void gridView1_ValidatingEditor(object sender, BaseContainerValidateEditorEventArgs e)
        {
            GridView view = gridView1;

            int rowHandle = view.FocusedRowHandle;

            if (view != null)
            {
                if (view.FocusedColumn.FieldName == "DATE")
                {
                    System.Data.DataRow row = gridView1.GetDataRow(gridView1.FocusedRowHandle);

                    string cellValue = e.Value.ToString();

                    if (!IsDateTime(cellValue))
                    {
                        //view.SetColumnError(view.Columns["DATE"], " Wrong Date Format..... ", ErrorType.Critical);
                        e.ErrorText = "Wrong Date Format";
                        e.Valid = false;
                        return;
                    }
                    else if (!CheckDateRange(dtpStart.DateTime, dtpEnd.DateTime, Convert.ToDateTime(cellValue)))
                    {
                        //view.SetColumnError(view.Columns["DATE"], " Date not in Range ", ErrorType.Critical);
                        e.ErrorText = "Date not in Range....";
                        e.Valid = false;
                        return;
                    }
                }
                else if (view.FocusedColumn.FieldName == "CREDIT")
                {
                    System.Data.DataRow row = gridView1.GetDataRow(gridView1.FocusedRowHandle);

                    string cellValue = e.Value.ToString();

                    //if (string.Format(e.Value.ToString())) return;

                    if (string.IsNullOrEmpty(cellValue)) view.SetRowCellValue(view.FocusedRowHandle, view.Columns["CREDIT"], 0);

                    if (!IsNumeric(cellValue))
                    {
                        e.ErrorText = "CREDIT Should be Numeric or Double....";
                        return;
                    }

                }
                else if (view.FocusedColumn.FieldName == "DEBIT")
                {
                    System.Data.DataRow row = gridView1.GetDataRow(gridView1.FocusedRowHandle);

                    string cellValue = e.Value.ToString();

                    if (string.IsNullOrEmpty(cellValue)) view.SetRowCellValue(view.FocusedRowHandle, view.Columns["DEBIT"], 0);

                    if (!IsNumeric(cellValue))
                    {
                        e.ErrorText = "DEBIT Should be Numeric or Double....";
                        return;
                    }

                }
                else if (view.FocusedColumn.FieldName == "BALANCE")
                {
                    System.Data.DataRow row = gridView1.GetDataRow(gridView1.FocusedRowHandle);

                    string cellValue = e.Value.ToString();

                    if (string.IsNullOrEmpty(cellValue)) view.SetRowCellValue(view.FocusedRowHandle, view.Columns["BALANCE"], 0);

                    if (!IsNumeric(cellValue))
                    {
                        e.ErrorText = "BALANCE Should be Numeric or Double....";
                        return;
                    }

                }
            }
        }

        void gridView1_InvalidRowException(object sender, DevExpress.XtraGrid.Views.Base.InvalidRowExceptionEventArgs e)
        {
            e.ExceptionMode = ExceptionMode.DisplayError;
            e.WindowCaption = "Input Error";
            // Destroying the editor and discarding the changes made within the edited cell
            gridView1.HideEditor();
        }

        public static bool IsDateTime(string txtDate)
        {
            DateTime tempDate;

            return DateTime.TryParse(txtDate, out tempDate) ? true : false;
        }

        public static Boolean IsNumeric(System.Object Expression)
        {
            if (Expression == null || Expression is DateTime)
                return false;

            if (Expression is Int16 || Expression is Int32 || Expression is Int64 || Expression is Decimal || Expression is Single || Expression is Double || Expression is Boolean)
                return true;

            try
            {
                if (Expression is string)
                    Double.Parse(Expression as string);
                else
                    Double.Parse(Expression.ToString());
                return true;
            }
            catch
            { } // just dismiss errors but return false
            return false;
        }

        void gridView1_ValidateRow(object sender, DevExpress.XtraGrid.Views.Base.ValidateRowEventArgs e)
        {
            GridView view = gridView1;

            int rowHandle = view.FocusedRowHandle;

            if (view != null)
            {
                view.ClearColumnErrors();

                if (view.FocusedColumn.FieldName == "DATE")
                {
                    System.Data.DataRow row = gridView1.GetDataRow(gridView1.FocusedRowHandle);

                    string cellValue = string.Empty;
                    if (row != null)
                    {
                        if (row["DATE"] != null)
                        {
                            cellValue = Convert.ToString(row["DATE"]);
                        }

                        if (!IsDateTime(cellValue))
                        {
                            view.SetColumnError(view.Columns["DATE"], " Wrong Date Format..... ", ErrorType.Critical); return;
                        }
                        else if (Convert.ToDateTime(dtpStart.DateTime) >= Convert.ToDateTime(cellValue) && Convert.ToDateTime(dtpEnd.DateTime) <= Convert.ToDateTime(cellValue))
                        {
                            view.SetColumnError(view.Columns["DATE"], " Date not in Range ", ErrorType.Critical); return;
                        }
                    }


                    //string cellValue = Convert.ToString(row["DATE"]);


                }
                else if (view.FocusedColumn.FieldName == "CREDIT")
                {
                    System.Data.DataRow row = gridView1.GetDataRow(gridView1.FocusedRowHandle);

                    string cellValue = string.Empty;
                    if (row != null)
                    {
                        if (row["CREDIT"] != null)
                        {
                            cellValue = Convert.ToString(row["CREDIT"]);
                        }

                        if (!IsNumeric(cellValue))
                        {
                            view.SetColumnError(view.Columns["CREDIT"], "CREDIT Should be Numeric or Double.... ", ErrorType.Critical); return;
                        }
                    }

                    //string cellValue =Convert.ToString(row["CREDIT"]);


                }
                else if (view.FocusedColumn.FieldName == "DEBIT")
                {
                    System.Data.DataRow row = gridView1.GetDataRow(gridView1.FocusedRowHandle);

                    string cellValue = string.Empty;
                    if (row != null)
                    {
                        if (row["DEBIT"] != null)
                        {
                            cellValue = Convert.ToString(row["DEBIT"]);
                        }

                        if (!IsNumeric(cellValue))
                        {
                            view.SetColumnError(view.Columns["DEBIT"], "DEBIT Should be Numeric or Double.... ", ErrorType.Critical); return;
                        }
                    }

                    //string cellValue = Convert.ToString(row["DEBIT"]);


                }
                else if (view.FocusedColumn.FieldName == "BALANCE")
                {
                    System.Data.DataRow row = gridView1.GetDataRow(gridView1.FocusedRowHandle);

                    string cellValue = string.Empty;
                    if (row != null)
                    {
                        if (row["BALANCE"] != null)
                        {
                            cellValue = Convert.ToString(row["BALANCE"]);
                        }

                        if (!IsNumeric(cellValue))
                        {
                            view.SetColumnError(view.Columns["BALANCE"], "BALANCE Should be Numeric or Double.... ", ErrorType.Critical); return;
                        }
                    }

                }
            }
        }

        static bool CheckDateRange(DateTime strStart, DateTime strEnd, DateTime valecheck)
        {

            if (Convert.ToDateTime(strStart) <= Convert.ToDateTime(valecheck) && Convert.ToDateTime(strEnd) >= Convert.ToDateTime(valecheck))
                return true;
            else
                return false;



        }

        private void load()
        {

            StringReader s = new StringReader(BankStatementExcel.Properties.Resources.BankTest);
            XmlTextReader xmdatareader = new XmlTextReader(s);
            DataSet _objdataset = new DataSet();
            _objdataset.ReadXml(xmdatareader);
            cboBank.DataSource = _objdataset.Tables[0];
            cboBank.DisplayMember = "Name";

            cboBank.ValueMember = "Code";
            cboBank.SelectedIndex = -1;
        }

    }
}


public class SearchDirectory
{
    public int ID { get; set; }
    public String BankCode { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Filename { get; set; }
}

public class ClassObject
{
    public DateTime DATE { get; set; }

    public double DEBIT { get; set; }

    public double CREDIT { get; set; }

    public double BALANCE { get; set; }

    public string PAYERNAME { get; set; }
    public string REVENUECODE { get; set; }
}

public class BankDirectory
{
    [XmlElement("Bank")]
    public List<Bank> BankList = new List<Bank>();
}
public class Bank
{
    //public int HouseNo { get; set; }
    public string Name { get; set; }
    public string Code { get; set; }
}

public static class JsonHelpers
{
    private static T CreateFromJsonStream<T>(this Stream stream)
    {
        JsonSerializer serializer = new JsonSerializer();
        T data;
        using (StreamReader streamReader = new StreamReader(stream))
        {
            data = (T)serializer.Deserialize(streamReader, typeof(T));
        }
        return data;
    }

    public static T CreateFromJsonString<T>(this String json)
    {
        T data;
        using (MemoryStream stream = new MemoryStream(System.Text.Encoding.Default.GetBytes(json)))
        {
            data = CreateFromJsonStream<T>(stream);
        }
        return data;
    }

    public static T CreateFromJsonFile<T>(this String fileName)
    {
        T data;
        using (FileStream fileStream = new FileStream(fileName, FileMode.Open))
        {
            data = CreateFromJsonStream<T>(fileStream);
        }
        return data;
    }

    public static IEnumerable<T> DeserializeFromFile<T>(this String fileName)
    {
        IEnumerable<T> data;
        using (StreamReader file = File.OpenText(fileName))
        {
            JsonSerializer serializer = new JsonSerializer();
            data = (IEnumerable<T>)serializer.Deserialize(file, typeof(IEnumerable<T>));
        }
        return data;
    }

    public static bool SerializeToFile<T>(this IEnumerable<T> enumerable, String fileName)
    {
        using (StreamWriter file = File.CreateText(fileName))
        {
            JsonSerializer serializer = new JsonSerializer();
            serializer.Formatting = Formatting.Indented;
            serializer.Serialize(file, enumerable);
        }
        return true;
    }
}