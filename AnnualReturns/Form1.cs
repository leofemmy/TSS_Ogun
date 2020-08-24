using DevExpress.XtraEditors.Controls;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraGrid.Views.Grid;
using AnnualReturns.Class;
//using BankReconciliation.Forms;
using DevExpress.Utils;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraPrinting;
//using TaxDrive.Class;


namespace AnnualReturns
{
    public partial class Form1 : Form
    {
        private DateTime timer = new DateTime();
        private int interval = 3; //Minutes;
        private List<ClassObject> _classObject = new List<ClassObject>();
        private List<SearchDirectory> searchDirectory = new List<SearchDirectory>();
        private SearchDirectory currentDirectory;
        private string defaultFolder = @"c:\programdata\ICMA\Annual_utilies\";
        private string defaultFilename = "DefaultDirectory.json";
        RepositoryItemSpinEdit _itemSpinEdit = new RepositoryItemSpinEdit();
        RepositoryItemSpinEdit _itemSpinEdit2 = new RepositoryItemSpinEdit();
        private string filenamesopen;
        public Form1()
        {
            InitializeComponent();
            spinEdit1.Value = DateTime.Now.Year - 1;
            gridView1.ValidatingEditor += gridView1_ValidatingEditor;
            gridView1.ValidateRow += gridView1_ValidateRow;


            gridView1.InvalidRowException += gridView1_InvalidRowException;

            gridView1.RowUpdated += gridView1_RowUpdated;

            gridControl1.Enabled = false;

            InitialiseSpinEdit(); btnImport.Click +=btnImport_Click;

            initialspinedit2();

            btnLoad.Click += btnLoad_Click;
            btnSave.Click += btnSave_Click;
            btnClear.Click += btnClear_Click;
            btnExport.Click += btnExport_Click;
            
        }

        void btnImport_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtTin.Text.Trim()))
            {
                MessageBox.Show("Agent TIN", "Annual Utilies", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            else
            {
                FrmImports import = new FrmImports(txtTin.Text.Trim(), spinEdit1.EditValue.ToString());
                this.Hide();
                import.ShowDialog();
            }
           

        }

        void gridView1_ValidateRow(object sender, DevExpress.XtraGrid.Views.Base.ValidateRowEventArgs e)
        {
            GridView view = sender as GridView;

            int rowHandle = view.FocusedRowHandle;

            if (view != null)
            {
                view.ClearColumnErrors();
                string cellValue = (string)view.GetRowCellValue(e.RowHandle, view.Columns["Staff_No"]);

                if (string.IsNullOrEmpty(cellValue) || cellValue.ToString() == "0")
                {
                    e.Valid = false;

                    view.SetColumnError(view.Columns["Staff_No"], "Staff Number is Empty");

                    return;

                }


                double celldeud = (double)view.GetRowCellValue(e.RowHandle, view.Columns["Tax_Deducted"]);
                if (string.IsNullOrEmpty(celldeud.ToString()) || celldeud.ToString() == "0")
                {
                    e.Valid = false;

                    view.SetColumnError(view.Columns["Tax_Deducted"], "Tax Deducted is Empty or Zero");
                    return;
                }

                double cellincome = (double)view.GetRowCellValue(e.RowHandle, view.Columns["Total_Income"]);
                if (string.IsNullOrEmpty(cellincome.ToString()) || cellincome.ToString() == "0")
                {
                    e.Valid = false;

                    view.SetColumnError(view.Columns["Total_Income"], "Total Income is Empty or Zero"); return;

                }


                int cellMonths = (int)view.GetRowCellValue(e.RowHandle, view.Columns["Months"]);
                if (string.IsNullOrEmpty(cellMonths.ToString()) || cellMonths.ToString() == "0")
                {
                    e.Valid = false;

                    view.SetColumnError(view.Columns["Months"], "Months is Empty"); return;

                }


                string cellname = (string)view.GetRowCellValue(e.RowHandle, view.Columns["Surname"]);
                if (string.IsNullOrEmpty(cellname) || cellname.ToString() == "0")
                {
                    e.Valid = false;

                    view.SetColumnError(view.Columns["Surname"], "Surname is Empty"); return;

                }


                string cellother = (string)view.GetRowCellValue(e.RowHandle, view.Columns["Othernames"]);

                if (string.IsNullOrEmpty(cellother) || cellother.ToString() == "0")
                {
                    e.Valid = false;

                    view.SetColumnError(view.Columns["Othernames"], "Other names is Empty");
                    return;

                }

            }
        }

        void InitialiseSpinEdit()
        {
            _itemSpinEdit = new RepositoryItemSpinEdit();
            _itemSpinEdit.EditMask = "n";
            _itemSpinEdit.Mask.BeepOnError = true;
            _itemSpinEdit.MinValue = 0m;
            _itemSpinEdit.MaxValue = decimal.MaxValue;
            _itemSpinEdit.Buttons.Clear();
            _itemSpinEdit.BorderStyle = BorderStyles.NoBorder;
        }

        private void initialspinedit2()
        {
            CultureInfo ci = new CultureInfo(CultureInfo.CurrentCulture.Name);
            ci.NumberFormat.NumberDecimalSeparator = ".";


            _itemSpinEdit2 = new RepositoryItemSpinEdit();
            _itemSpinEdit2.Mask.Culture = ci;
            _itemSpinEdit2.EditMask = "n0";
            _itemSpinEdit2.Mask.BeepOnError = true;
            _itemSpinEdit2.MinValue = 0;
            _itemSpinEdit2.MaxValue = int.MaxValue;
            //decimal.MaxValue;
            _itemSpinEdit2.Buttons.Clear();
            _itemSpinEdit2.BorderStyle = BorderStyles.NoBorder;
        }

        void btnExport_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtTin.Text.ToString()))
            {
                MessageBox.Show("Agent TIN not Avaliable", "Annual Return", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }
            else
            {
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.InitialDirectory = Convert.ToString(Environment.SpecialFolder.MyDocuments);
                saveFileDialog1.Filter = "Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*";
                saveFileDialog1.FilterIndex = 1;

                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    gridView1.ExportToXlsx(saveFileDialog1.FileName);

                }

                MessageBox.Show(string.Format("Export to Excel SuccessFull!, you can find the file in {0}",
                    saveFileDialog1.FileName));
            }
        }

        void btnClear_Click(object sender, EventArgs e)
        {
            classObjectBindingSource.DataSource = null;
            _classObject = null;
            searchDirectory = null;
            currentDirectory = null;
        }

        void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtTin.Text.ToString()))
            {
                MessageBox.Show("Agent TIN not Avaliable", "Annual Return", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }
            else
            {
                var clsObject = classObjectBindingSource.DataSource as List<ClassObject>;

                if (clsObject != null && clsObject.Any())
                {
                    ProcessSaveEntry(clsObject);
                    ProcessSaveDirectory();
                    timer = DateTime.Now;
                }
            }

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
            }
        }

        void btnLoad_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtTin.Text.ToString()) || txtTin.Text.ToString() == "")
            {
                MessageBox.Show("Agent TIN not Avaliable", "Annual Return", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }
            else
            {
                ProcessLoadDirectory();
                ProcessNewEntry();
            }

        }

        private void gridView1_InvalidRowException(object sender,
            DevExpress.XtraGrid.Views.Base.InvalidRowExceptionEventArgs e)
        {
            e.ExceptionMode = ExceptionMode.NoAction;
            //e.WindowCaption = "Input Error";

            gridView1.HideEditor();
        }

        public static Boolean IsNumeric(System.Object Expression)
        {
            if (Expression == null || Expression is DateTime)
                return false;

            if (Expression is Int16 || Expression is Int32 || Expression is Int64 || Expression is Decimal ||
                Expression is Single || Expression is Double || Expression is Boolean)
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
            {
            } // just dismiss errors but return false
            return false;
        }

        public static bool IsDateTime(string txtDate)
        {
            DateTime tempDate;

            return DateTime.TryParse(txtDate, out tempDate) ? true : false;
        }

        public static Boolean isAlphaNumeric(string strToCheck)
        {
            Regex rg = new Regex(@"^[a-zA-Z0-9\/s,]*$");
            return rg.IsMatch(strToCheck);
        }

        private void gridView1_ValidatingEditor(object sender,
            DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventArgs e)
        {
            GridView view = gridView1;

            int rowHandle = view.FocusedRowHandle;

            if (view != null)
            {

                if (view.FocusedColumn.FieldName == "TIN")
                {
                    System.Data.DataRow row = gridView1.GetDataRow(gridView1.FocusedRowHandle);

                    string cellValue = e.Value.ToString();

                    if (string.IsNullOrEmpty(cellValue))
                        view.SetRowCellValue(view.FocusedRowHandle, view.Columns["TIN"], 0);

                    if (!isAlphaNumeric(cellValue))
                    {
                        e.ErrorText = "TIN Should be AlphaNumeric...."; e.Valid = false;
                        return;
                    }
                }
                else if (view.FocusedColumn.FieldName == "Total_Income")
                {
                    System.Data.DataRow row = gridView1.GetDataRow(gridView1.FocusedRowHandle);

                    string cellValue = e.Value.ToString();

                    if (!IsNumeric(cellValue))
                    {
                        e.ErrorText = "Total Income Should be Decmial or Double...."; e.Valid = false;
                        return;
                    }


                }
                else if (view.FocusedColumn.FieldName == "Total_Relief")
                {
                    System.Data.DataRow row = gridView1.GetDataRow(gridView1.FocusedRowHandle);

                    string cellValue = e.Value.ToString();

                    if (string.IsNullOrEmpty(cellValue))
                        view.SetRowCellValue(view.FocusedRowHandle, view.Columns["Total_Relief"], 0);

                    if (!IsNumeric(cellValue))
                    {
                        e.ErrorText = "Total Relief Should be Decmial or Double...."; e.Valid = false;
                        return;
                    }

                }
                else if (view.FocusedColumn.FieldName == "Total_Deduction")
                {
                    System.Data.DataRow row = gridView1.GetDataRow(gridView1.FocusedRowHandle);

                    string cellValue = e.Value.ToString();

                    if (string.IsNullOrEmpty(cellValue))
                        view.SetRowCellValue(view.FocusedRowHandle, view.Columns["Total_Deduction"], 0);

                    if (!IsNumeric(cellValue))
                    {
                        e.ErrorText = "Total Deduction Should be Decmial or Double...."; e.Valid = false;
                        return;
                    }

                }
                else if (view.FocusedColumn.FieldName == "Tax_Deducted")
                {
                    System.Data.DataRow row = gridView1.GetDataRow(gridView1.FocusedRowHandle);

                    string cellValue = e.Value.ToString();

                    if (string.IsNullOrEmpty(cellValue))
                        view.SetRowCellValue(view.FocusedRowHandle, view.Columns["Tax_Deducted"], 0);

                    if (!IsNumeric(cellValue))
                    {
                        e.ErrorText = "Tax Deducted Should be Decmial or Double...."; e.Valid = false;
                        return;
                    }

                }
                else if (view.FocusedColumn.FieldName == "Surname")
                {

                    System.Data.DataRow row = gridView1.GetDataRow(gridView1.FocusedRowHandle);

                    string cellValue = e.Value.ToString();

                    if (string.IsNullOrEmpty(cellValue))
                        view.SetRowCellValue(view.FocusedRowHandle, view.Columns["Surname"], 0);

                    if (!isAlphaNumeric(cellValue))
                    {
                        e.ErrorText = "Surname Should be AlphaNumeric...."; e.Valid = false;
                        return;
                    }

                }
                else if (view.FocusedColumn.FieldName == "Months")
                {
                    System.Data.DataRow rows = gridView1.GetDataRow(gridView1.FocusedRowHandle);

                    string cellValue = e.Value.ToString();

                    if (string.IsNullOrEmpty(cellValue) || Convert.ToInt16(cellValue) == 0)
                        view.SetRowCellValue(view.FocusedRowHandle, view.Columns["Months"], 1);

                    if (!IsNumeric(cellValue))
                    {
                        e.ErrorText = "Months Should be Numeric...."; e.Valid = false;
                        return;
                    }
                    else if (Convert.ToInt16(cellValue) <= 0)
                    {
                        e.ErrorText = "Months Should Not be Negative....";
                        e.Valid = false;
                        return;
                    }
                    else if (Convert.ToInt16(cellValue) >= 13)
                    {
                        e.ErrorText = "Months Should Not be Greater Than 12 Months....";
                        e.Valid = false;
                        return;
                    }

                }
                else if (view.FocusedColumn.FieldName == "Othernames")
                {

                    System.Data.DataRow row = gridView1.GetDataRow(gridView1.FocusedRowHandle);

                    string cellValue = e.Value.ToString();

                    if (string.IsNullOrEmpty(cellValue))
                        view.SetRowCellValue(view.FocusedRowHandle, view.Columns["Othernames"], 0);

                    if (!isAlphaNumeric(cellValue))
                    {
                        e.ErrorText = "Othernames Should be AlphaNumeric...."; e.Valid = false;
                        return;
                    }

                }

            }
        }

        bool ProcessSaveEntry(List<ClassObject> clsObject)
        {
            if (clsObject == null || !clsObject.Any())
                return false;
            string folderPath = Path.Combine(defaultFolder, "Others");
            if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);
            string filename = string.Format(@"{0}\{1}.json", folderPath, currentDirectory.Filename);
            return clsObject.SerializeToFile(filename);

        }

        void ProcessNewEntry()
        {
            currentDirectory =
                searchDirectory.SingleOrDefault(
                    x =>
                        x.Year == spinEdit1.EditValue.ToString() && x.Tin == txtTin.Text.ToString());
            if (currentDirectory == null)
            {
                currentDirectory = new SearchDirectory();
                currentDirectory.ID = searchDirectory.Count + 1;
                currentDirectory.Year = spinEdit1.EditValue.ToString();
                currentDirectory.Tin = txtTin.Text.ToString();
                //currentDirectory.StartDate = dtpStart.DateTime;
                //currentDirectory.EndDate = dtpEnd.DateTime;
                currentDirectory.Filename = Guid.NewGuid().ToString();
                _classObject = new List<ClassObject>();
                searchDirectory.Add(currentDirectory);
            }
            else
            {
                _classObject = LoadClassObjects(currentDirectory.Filename);
            }

            gridControl1.Enabled = true;
            //_classObject = new List<ClassObject>();
            timer = DateTime.Now;
            classObjectBindingSource.DataSource = _classObject;

            gridControl1.ForceInitialize();
            gridView1.Columns["Total_Income"].ColumnEdit = _itemSpinEdit;
            gridView1.Columns["Total_Relief"].ColumnEdit = _itemSpinEdit;
            gridView1.Columns["Total_Deduction"].ColumnEdit = _itemSpinEdit;
            gridView1.Columns["Tax_Deducted"].ColumnEdit = _itemSpinEdit;
            gridView1.Columns["Months"].ColumnEdit = _itemSpinEdit2;

            gridView1.BestFitColumns();
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
            if (searchDirectory == null || !searchDirectory.Any())
                return false;
            string filename = string.Format(@"{0}\{1}", defaultFolder, defaultFilename);
            return searchDirectory.SerializeToFile(filename);
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


    }
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

public class ClassObject
{
    public string TIN { get; set; }

    public int Months { get; set; }

    public string Staff_No { get; set; }

    public string Surname { get; set; }

    public string Othernames { get; set; }

    public double Total_Income { get; set; }

    public double Total_Relief { get; set; }

    public double Total_Deduction { get; set; }

    public double Tax_Deducted { get; set; }


}

public class SearchDirectory
{
    public int ID { get; set; }
    public String Year { get; set; }

    public string Tin { get; set; }

    public string Filename { get; set; }
}

