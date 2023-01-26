using DevExpress.Utils;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.DXErrorProvider;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraSplashScreen;
using LinqToExcel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using ErrorInfo = DevExpress.XtraEditors.DXErrorProvider.ErrorInfo;
using Formatting = Newtonsoft.Json.Formatting;

namespace BankStatementExcel.Forms
{
    public partial class FrmImport : Form
    {
        private List<BankImportClass> _classObject = new List<BankImportClass>();
        List<SearchDirectory> searchDirectory = new List<SearchDirectory>();
        private SearchDirectory currentDirectory;
        private string defaultFolder = @"c:\programdata\ICMA\recon_utilies\";
        private string defaultFilename = "DefaultDirectory.json";
        RepositoryItemSpinEdit _itemSpinEdit = new RepositoryItemSpinEdit();
        //private SqlCommand _command; private SqlDataAdapter adp;
        private string filenamesopens;

        BindingList<BankImportClass> bindingList = new BindingList<BankImportClass>();
        private string filenamesopen;
        ExcelQueryFactory excel = null;

        private string wrkbankCode = string.Empty;
        private DateTime _dtstart;
        private DateTime _dtend; int iCount = 0; int iCount2 = 0;
        private string strBank;

        public FrmImport()
        {
            SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
            InitializeComponent();
            Init();
            SplashScreenManager.CloseForm(false);
        }

        public FrmImport(string bankCode, DateTime dtsrat, DateTime dtend, string BankNames)
        {
            SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

            InitializeComponent();

            wrkbankCode = bankCode;

            _dtstart = dtsrat;

            _dtend = dtend; strBank = BankNames;

            Init();
            SplashScreenManager.CloseForm(false);

        }

        private void Init()
        {

            btnImport.Click += btnImport_Click;

            btnRead.Click += btnRead_Click; btnClose.Click += btnClose_Click;

            cboSheet.SelectedIndexChanged += cboSheet_SelectedIndexChanged;

            //gridView1.ValidatingEditor += gridView1_ValidatingEditor;

            gridView1.InvalidRowException += gridView1_InvalidRowException;

            gridView1.ValidateRow += gridView1_ValidateRow;

            label6.Text = _dtstart.ToString();

            label7.Text = _dtend.ToString();

            groupControl1.Text = string.Format("Bank Statement Import:- {0}  between {1:d} and {2:d}", strBank, _dtstart, _dtend);

            btnBack.Click += btnBack_Click;
            btnSave.Click += btnSave_Click; btnExport.Click += btnExport_Click;
        }

        void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();

        }

        void btnExport_Click(object sender, EventArgs e)
        {
            SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

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

            SplashScreenManager.CloseForm(false);
        }

        void btnSave_Click(object sender, EventArgs e)
        {
            if (gridView1.HasError())
            {
                MessageBox.Show("There are still some errors in the Data Imported! Do Correct it.", "Reconcilation Utilies",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            else
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

                var BankImptClass = gridControl1.DataSource as List<BankImportClass>;

                if (BankImptClass != null && BankImptClass.Any())
                {
                    //ProcessLoadDirectory();
                    ProcessNewEntry();
                    ProcessSaveEntry(BankImptClass);
                    ProcessSaveDirectory();

                    btnExport.Enabled = true;
                }
                SplashScreenManager.CloseForm(false);
            }

            //if (iCount >= 1 || iCount2 >= 1)
            //{
            //    MessageBox.Show("There are still some errors in the Data Imported! Do Correct it.", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    return;
            //}
            //else
            //{
            //    var BankImptClass = gridControl1.DataSource as List<BankImportClass>;

            //    if (BankImptClass != null && BankImptClass.Any())
            //    {
            //        //ProcessLoadDirectory();
            //        ProcessNewEntry();
            //        ProcessSaveEntry(BankImptClass);
            //        ProcessSaveDirectory();

            //        btnExport.Enabled = true;
            //    }
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

        void btnBack_Click(object sender, EventArgs e)
        {
            gridControl1.DataSource = null;
            cboSheet.SelectedIndex = -1;
            cboDate.SelectedIndex = -1;
            cboDebit.SelectedIndex = -1;
            CboBalance.SelectedIndex = -1;
            cboCredit.SelectedIndex = -1;
            this.Hide();
            Form1 forms = new Form1();
            forms.ShowDialog();

        }

        void gridView1_InvalidRowException(object sender, DevExpress.XtraGrid.Views.Base.InvalidRowExceptionEventArgs e)
        {
            e.ExceptionMode = ExceptionMode.NoAction;
        }

        void gridView1_ValidateRow(object sender, DevExpress.XtraGrid.Views.Base.ValidateRowEventArgs e)
        {
            ErrorInfo info = new ErrorInfo();
            (e.Row as BankImportClass).GetError(info);
            e.Valid = info.ErrorText == "";
        }

        void btnRead_Click(object sender, EventArgs e)
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

                GridView view = gridView1;
                //var excel = new ExcelQueryFactory(filenamesopen);
                excel.AddMapping<BankImportClass>(x => x.DATE, cboDate.Text.ToString().Trim());
                excel.AddMapping<BankImportClass>(x => x.CREDIT, cboCredit.Text.ToString().Trim());
                excel.AddMapping<BankImportClass>(x => x.DEBIT, cboDebit.Text.ToString().Trim());
                excel.AddMapping<BankImportClass>(x => x.BALANCE, CboBalance.Text.ToString().Trim());

                var bankImportList = (from c in excel.Worksheet<BankImportClass>(cboSheet.Text.ToString().Trim())
                                      select c).ToList();

                if (bankImportList.Any())
                {
                    foreach (var importClass in bankImportList)
                    {
                        importClass.startDate = _dtstart;
                        importClass.endtDate = _dtend;
                    }
                    gridControl1.DataSource = null;

                    gridControl1.DataSource = bankImportList;
                    gridView1.Columns["startDate"].Visible = false;
                    gridView1.Columns["endtDate"].Visible = false;
                    gridView1.Columns["DATE"].DisplayFormat.FormatType = FormatType.DateTime;
                    gridView1.Columns["DATE"].DisplayFormat.FormatString = "dd/MM/yyyy";

                    gridView1.Columns["CREDIT"].DisplayFormat.FormatType = FormatType.Numeric;
                    gridView1.Columns["CREDIT"].DisplayFormat.FormatString = "n2";

                    gridView1.Columns["DEBIT"].DisplayFormat.FormatType = FormatType.Numeric;
                    gridView1.Columns["DEBIT"].DisplayFormat.FormatString = "n2";
                    //DataTable dt = bankImportList;



                    //GridViewInfo viewInfo = gridView1.GetViewInfo() as GridViewInfo;

                    //foreach (GridRowInfo rowInfo in viewInfo.RowsInfo)
                    //{
                    //    int rowHandle = rowInfo.RowHandle;

                    //    if (view.FocusedColumn.FieldName == "DATE")
                    //    {
                    //        System.Data.DataRow row = gridView1.GetDataRow(gridView1.FocusedRowHandle);

                    //        //string cellValue = rowInfo["DATE"].ToString();
                    //        //string cellValue = (string)view.GetRowCellValue(e.RowHandle, view.Columns["DATE"]);

                    //        if (!IsDateTime(view.GetRowCellValue(rowHandle, view.Columns["DATE"]).ToString()))
                    //        {
                    //            view.SetColumnError(view.Columns["DATE"], " Date not in right format ", ErrorType.Critical);
                    //        }

                    //        if (!CheckDateRange(_dtstart, _dtend, Convert.ToDateTime(view.GetRowCellValue(rowHandle, view.Columns["DATE"]))))
                    //        {
                    //            //Increment the count
                    //            iCount++;
                    //            view.SetColumnError(view.Columns["DATE"], " Date not in selected date Range ", ErrorType.Critical);
                    //            view.SetRowCellValue(view.FocusedRowHandle, view.Columns["DATE"], Convert.ToDateTime(view.Columns["DATE"]).ToShortDateString());
                    //        }
                    //        else
                    //        {
                    //            view.SetRowCellValue(view.FocusedRowHandle, view.Columns["DATE"], Convert.ToDateTime(view.Columns["DATE"]).ToShortDateString());
                    //        }

                    //        if (!IsNumeric(view.GetRowCellValue(rowHandle, view.Columns["CREDT"]).ToString()))
                    //        {
                    //            view.SetColumnError(view.Columns["CREDIT"], " Credit is not in the right format ", ErrorType.Critical);
                    //        }
                    //        else if (Convert.ToInt32(view.GetRowCellValue(rowHandle, view.Columns["CREDT"]).ToString()) < 0)
                    //        {
                    //            iCount2++;
                    //            view.SetColumnError(view.Columns["CREDIT"], " Credit is negative ", ErrorType.Critical);
                    //        }

                    //        if (!IsNumeric(view.GetRowCellValue(rowHandle, view.Columns["DEBIT"]).ToString()))
                    //        {
                    //            view.SetColumnError(view.Columns["DEBIT"], " Debit is not in the right format ", ErrorType.Critical);
                    //        }

                    //        if (!IsNumeric(view.GetRowCellValue(rowHandle, view.Columns["BALANCE"]).ToString()))
                    //        {
                    //            view.SetColumnError(view.Columns["BALANCE"], " Balance is not in the right format ", ErrorType.Critical);
                    //        }

                    //    }

                    //}

                    //for (int i = 0; i < gridView1.RowCount; i++)
                    //{
                    //    //view.FocusedRowHandle = i;

                    //    var row = gridView1.GetRow(i) as BankImportClass;

                    //    if (row != null)
                    //    {
                    //        //gridView1.SetColumnError()    
                    //        //DataRow row = gridView1.GetDataRow(i);
                    //        //string name = row["ColumnName"].ToString();


                    //        if (!IsDateTime(row.DATE))
                    //        {
                    //            view.SetColumnError(view.Columns["DATE"], " Date not in right format ", ErrorType.Critical);
                    //        }
                    //        else if (!CheckDateRange(_dtstart, _dtend, Convert.ToDateTime(row.DATE)))
                    //        {
                    //            //Increment the count
                    //            iCount++;
                    //            view.SetColumnError(view.Columns["DATE"], " Date not in selected date Range ",
                    //                ErrorType.Critical);
                    //            view.SetRowCellValue(view.FocusedRowHandle, view.Columns["DATE"],
                    //                Convert.ToDateTime(row.DATE).ToShortDateString());
                    //        }
                    //        else
                    //        {
                    //            view.SetRowCellValue(view.FocusedRowHandle, view.Columns["DATE"], Convert.ToDateTime(row.DATE).ToShortDateString());
                    //        }

                    //        if (!IsNumeric(row.CREDIT))
                    //        {
                    //            view.SetColumnError(view.Columns["CREDIT"], " Credit is not in the right format ", ErrorType.Critical);
                    //        }
                    //        else if (Convert.ToInt32(row.CREDIT) < 0)
                    //        {
                    //            iCount2++;
                    //            view.SetColumnError(view.Columns["CREDIT"], " Credit is negative ", ErrorType.Critical);
                    //        }

                    //        if (!IsNumeric(row.DEBIT))
                    //        {
                    //            view.SetColumnError(view.Columns["DEBIT"], " Debit is not in the right format ", ErrorType.Critical);
                    //        }

                    //        if (!IsNumeric(row.BALANCE))
                    //        {
                    //            view.SetColumnError(view.Columns["BALANCE"], " Balance is not in the right format ", ErrorType.Critical);
                    //        }

                    //    }
                    //}
                    //if (iCount >= 1)
                    //{
                    //    MessageBox.Show(string.Format("There are {0} Columnus, which Date not in selected date Range.Please, correct them and try again.", iCount), "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //    //return;

                    //}
                    //if (iCount2 >= 1)
                    //{
                    //    MessageBox.Show(string.Format("There are {0} Columnus, which Credit Value is Negative. Please Correct it along with their corresponding Values.", iCount2), "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //    //return;
                    //}

                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                SplashScreenManager.CloseForm(false);
            }


        }

        void cboSheet_SelectedIndexChanged(object sender, EventArgs e)
        {
            //var excel = new ExcelQueryFactory(filenamesopen);
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

                List<string> columnNames = excel.GetColumnNames(cboSheet.Text.ToString().Trim()).ToList();

                setComboList(cboCredit, columnNames);
                setComboList(cboDate, columnNames);
                setComboList(cboDebit, columnNames); setComboList(CboBalance, columnNames);
            }
            catch (Exception)
            {

                throw;
            }
            finally
            { SplashScreenManager.CloseForm(false); }

        }

        void btnImport_Click(object sender, EventArgs e)
        {
            try
            {

                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

                using (
          OpenFileDialog _openFileDialogCSV = new OpenFileDialog()
          {
              InitialDirectory = Application.ExecutablePath,
              Filter = "Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*",
              FilterIndex = 1,
              RestoreDirectory = true
          })
                {
                    if (_openFileDialogCSV.ShowDialog() == DialogResult.OK)
                    {

                        if (_openFileDialogCSV.FileName.Length > 0)
                        {
                            filenamesopen = _openFileDialogCSV.FileName;

                            //var excelOM = new ExcelQueryFactory(filenamesopen);
                            //excelOM.DatabaseEngine = DatabaseEngine.Ace;

                            excel = new ExcelQueryFactory(filenamesopen);
                            var worksheetNames = excel.GetWorksheetNames();
                            cboSheet.Items.Clear();
                            foreach (var items in worksheetNames)
                            {
                                cboSheet.Items.Add(items);
                            }

                        }
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                SplashScreenManager.CloseForm(false);
            }

        }

        bool ProcessSaveDirectory()
        {
            if (searchDirectory == null || !searchDirectory.Any())
                return false;
            string filename = string.Format(@"{0}\{1}", defaultFolder, defaultFilename);
            return searchDirectory.SerializeToFile(filename);
        }

        void ProcessNewEntry()
        {
            currentDirectory =
                 searchDirectory.SingleOrDefault(
                     x =>
                         x.BankCode == wrkbankCode.ToString() &&
                         (x.StartDate >= _dtstart && x.EndDate <= _dtend));

            if (currentDirectory == null)
            {
                currentDirectory = new SearchDirectory();
                currentDirectory.ID = searchDirectory.Count + 1;
                currentDirectory.BankCode = wrkbankCode.ToString();
                currentDirectory.StartDate = _dtstart;
                currentDirectory.EndDate = _dtend;
                currentDirectory.Filename = Guid.NewGuid().ToString();
                _classObject = new List<BankImportClass>();
                searchDirectory.Add(currentDirectory);
            }
            else
            {
                _classObject = LoadClassObjects(currentDirectory.Filename);
            }
        }

        List<BankImportClass> LoadClassObjects(string filename)
        {
            string folderPath = Path.Combine(defaultFolder, "Others");
            if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);
            string filepath = string.Format(@"{0}\{1}.json", folderPath, filename);
            if (File.Exists(filepath))
                return filepath.DeserializeFromFile<BankImportClass>().ToList();
            else
                return new List<BankImportClass>();
        }

        bool ProcessSaveEntry(List<BankImportClass> clsObject)
        {
            if (clsObject == null || !clsObject.Any())
                return false;
            string folderPath = Path.Combine(defaultFolder, "Others");
            if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);
            string filename = string.Format(@"{0}\{1}.json", folderPath, currentDirectory.Filename);
            return clsObject.SerializeToFile(filename);

        }

        public static bool IsDateTime(string txtDate)
        {
            DateTime tempDate;

            return DateTime.TryParse(txtDate, out tempDate) ? true : false;
        }

        public static bool CheckDateRange(DateTime strStart, DateTime strEnd, DateTime valecheck)
        {

            if ((Convert.ToDateTime(valecheck) >= Convert.ToDateTime(strStart)) && (Convert.ToDateTime(valecheck) <= Convert.ToDateTime(strEnd)))
                return true;
            else
                return false;

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
        private void setComboList(ComboBox sComboBox, List<string> columinsVar)
        {
            sComboBox.Items.Clear();

            if (columinsVar != null)
            {
                foreach (var items in columinsVar)
                {
                    sComboBox.Items.Add(items);
                }
            }


        }

        public static bool InRange(DateTime dateToCheck, DateTime startDate, DateTime endDate)
        {
            return dateToCheck >= startDate && dateToCheck < endDate;
        }

        public static DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }
    }

    public class BankImportClass : IDXDataErrorInfo
    {
        public DateTime DATE { get; set; }
        public decimal DEBIT { get; set; }
        public decimal CREDIT { get; set; }
        public decimal BALANCE { get; set; }
        public string PAYERNAME { get; set; }
        public string REVENUECODE { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endtDate { get; set; }

        public void GetPropertyError(string propertyName, ErrorInfo info)
        {
            //throw new NotImplementedException();
            if (propertyName == "DATE")
            {
                if (string.IsNullOrWhiteSpace(DATE.ToString()))
                    info.ErrorText = string.Format("The {0} field cannot be empty", propertyName);
                else if (!FrmImport.IsDateTime(DATE.ToString()))
                    info.ErrorText = "Invalid Date";
                else if (!FrmImport.CheckDateRange(startDate, endtDate, Convert.ToDateTime(DATE)))
                {
                    info.ErrorText = "Date not within the specified date range";
                }
            }

            if (propertyName == "CREDIT")
            {
                if (!FrmImport.IsNumeric(CREDIT))
                {
                    info.ErrorText = "Credit is not in the right format";
                }
                else if (CREDIT < 0)
                    info.ErrorText = "Credit is negative ";
            }

            //if (propertyName == "DEBIT")
            //{
            //    info.ErrorText = "Debit is not in the right format";
            //}

            //if (propertyName == "BALANCE")
            //{
            //    info.ErrorText = "Balance is not in the right format";
            //}
        }

        public void GetError(ErrorInfo info)
        {
            //throw new NotImplementedException();
            ErrorInfo propertyInfo = new ErrorInfo();
            GetPropertyError("DATE", propertyInfo);
            if (propertyInfo.ErrorText == "")
                GetPropertyError("CREDIT", propertyInfo);
            if (propertyInfo.ErrorText != "")
                info.ErrorText = "This object has errors";
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

    public static class GridControlExtension
    {
        public static bool HasError(this ColumnView view)
        {
            for (int i = 0; i < view.RowCount; i++)
            {
                //var row = view.GetRow(i);
                ErrorInfo info = new ErrorInfo();
                var row = view.GetRow(i) as BankImportClass;
                if (row != null)
                {
                    row.GetError(info);
                    if (!string.IsNullOrWhiteSpace(info.ErrorText))
                        return true;
                }
                else
                    throw new NullReferenceException("error during conversion");
            }
            return false;
        }
    }
}
