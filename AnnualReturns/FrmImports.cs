using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors.DXErrorProvider;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraSplashScreen;
using LinqToExcel;
using LinqToExcel.Domain;
using ErrorInfo = DevExpress.XtraEditors.DXErrorProvider.ErrorInfo;
using Formatting = Newtonsoft.Json.Formatting;

namespace AnnualReturns
{
    public partial class FrmImports : Form
    {
        //private string filenamesopen;
        ExcelQueryFactory excel = null;
        private List<ClassObject> _classObject = new List<ClassObject>();
        private List<SearchDirectory> searchDirectory = new List<SearchDirectory>();
        private SearchDirectory currentDirectory;
        private string defaultFolder = @"c:\programdata\ICMA\Annual_utilies\";
        private string defaultFilename = "DefaultDirectory.json";
        RepositoryItemSpinEdit _itemSpinEdit = new RepositoryItemSpinEdit();
        RepositoryItemSpinEdit _itemSpinEdit2 = new RepositoryItemSpinEdit();
        private string filenamesopen;
        private string agent = string.Empty;
        private string intyearw = string.Empty;
        public FrmImports()
        {
            InitializeComponent();
            Init();
        }

        public FrmImports(string agentTin, string intyear)
        {
            InitializeComponent();

            intyearw = intyear;

            agent = agentTin;

            Init();
        }

        void Init()
        {
            btnClose.Click += btnClose_Click; btnBack.Click += btnBack_Click; btnImport.Click += btnImport_Click; cboSheet.SelectedIndexChanged += cboSheet_SelectedIndexChanged; btnRead.Click += btnRead_Click;
            btnSave.Click += btnSave_Click; btnExport.Click += btnExport_Click;
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

        void btnSave_Click(object sender, EventArgs e)
        {
            if (gridView1.HasError())
            {
                MessageBox.Show("There are still some errors in the Data Imported! Do Correct it.",
                    "Annual Return Utilies",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            else
            {
                var BankImptClass = gridControl1.DataSource as List<BankImportClass>;

                if (BankImptClass != null && BankImptClass.Any())
                {
                    //ProcessLoadDirectory();
                    ProcessNewEntry();
                    ProcessSaveEntry(BankImptClass);
                    ProcessSaveDirectory();

                    btnExport.Enabled = true;
                }
            }
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

        void ProcessNewEntry()
        {
            currentDirectory =
                searchDirectory.SingleOrDefault(
                    x =>
                        x.Year == intyearw && x.Tin == agent);
            if (currentDirectory == null)
            {
                currentDirectory = new SearchDirectory();
                currentDirectory.ID = searchDirectory.Count + 1;
                currentDirectory.Year = intyearw;
                currentDirectory.Tin = agent;
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

        }

        bool ProcessSaveDirectory()
        {
            if (searchDirectory == null || !searchDirectory.Any())
                return false;
            string filename = string.Format(@"{0}\{1}", defaultFolder, defaultFilename);
            return searchDirectory.SerializeToFile(filename);
        }

        void btnRead_Click(object sender, EventArgs e)
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

                GridView view = gridView1;

                excel.AddMapping<BankImportClass>(x => x.TIN, cboTin.Text.ToString().Trim());
                excel.AddMapping<BankImportClass>(x => x.Months, cboMonth.Text.ToString().Trim());
                excel.AddMapping<BankImportClass>(x => x.Staff_No, cboNo.Text.ToString().Trim());
                excel.AddMapping<BankImportClass>(x => x.Surname, CboSurname.Text.ToString().Trim());
                excel.AddMapping<BankImportClass>(x => x.Othernames, CboOther.Text.ToString().Trim());
                excel.AddMapping<BankImportClass>(x => x.Total_Income, CboIncome.Text.ToString().Trim());
                excel.AddMapping<BankImportClass>(x => x.Total_Relief, CboRelief.Text.ToString().Trim());
                excel.AddMapping<BankImportClass>(x => x.Total_Deduction, CboDeduction.Text.ToString().Trim());
                excel.AddMapping<BankImportClass>(x => x.Tax_Deducted, CboDeducted.Text.ToString().Trim());

                var bankImportList = (from c in excel.Worksheet<BankImportClass>(cboSheet.Text.ToString().Trim())
                                      select c).ToList();

                if (bankImportList.Any())
                {
                    gridControl1.DataSource = null;

                    gridControl1.DataSource = bankImportList;


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
            try
            {
                SplashScreenManager.ShowForm(this, typeof (WaitForm1), true, true, false);

                List<string> columnNames = excel.GetColumnNames(cboSheet.Text.ToString().Trim()).ToList();

                setComboList(cboTin, columnNames);
                setComboList(CboOther, columnNames);
                setComboList(CboRelief, columnNames);
                setComboList(CboDeducted, columnNames);
                setComboList(CboDeduction, columnNames);
                setComboList(cboMonth, columnNames);
                setComboList(CboIncome, columnNames);
                setComboList(cboNo, columnNames);
                setComboList(CboSurname, columnNames);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message + "..." + ex.StackTrace, "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;

            }
            finally
            {
                SplashScreenManager.CloseForm(false);
            }
        }

        void btnImport_Click(object sender, EventArgs e)
        {
            try
            {

                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

                using (
          OpenFileDialog openFileDialogCSV = new OpenFileDialog()
          {
              InitialDirectory = Application.ExecutablePath,
              Filter = "Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*",
              FilterIndex = 1,
              RestoreDirectory = true
          })
                {
                    if (openFileDialogCSV.ShowDialog() == DialogResult.OK)
                    {

                        if (openFileDialogCSV.FileName.Length > 0)
                        {
                            filenamesopen = openFileDialogCSV.FileName;

                            //var excelOM = new ExcelQueryFactory(filenamesopen);
                            //excelOM.DatabaseEngine = DatabaseEngine.Ace;

                            excel = new ExcelQueryFactory(filenamesopen);
                            var worksheetNames = excel.GetWorksheetNames();
                            //var worksheetNames = excelOM.Worksheet();
                            cboSheet.Items.Clear();
                            foreach (var items in worksheetNames)
                            {
                                cboSheet.Items.Add(items);
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message + ex.StackTrace, "Annual Returns Utilies", MessageBoxButtons.OK, MessageBoxIcon.Error); return;
                ;
            }
            finally
            {
                SplashScreenManager.CloseForm(false);
            }
        }

        void btnBack_Click(object sender, EventArgs e)
        {
            gridControl1.DataSource = null;

            this.Hide();
            //clear the combo box
            cboTin.SelectedIndex = -1;
            cboMonth.SelectedIndex = -1;
            cboNo.SelectedIndex = -1;
            CboSurname.SelectedIndex = -1;
            CboOther.SelectedIndex = -1;
            CboIncome.SelectedIndex = -1;
            CboRelief.SelectedIndex = -1;
            CboDeducted.SelectedIndex = -1;
            CboDeduction.SelectedIndex = -1;
            cboSheet.SelectedIndex = -1;

            Form1 fmback = new Form1();
            fmback.ShowDialog();
        }

        void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
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


        public class BankImportClass : IDXDataErrorInfo
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

            public void GetPropertyError(string propertyName, ErrorInfo info)
            {
                //throw new NotImplementedException();
                if (propertyName == "Total_Deduction")
                {
                    if (!FrmImports.IsNumeric(Total_Deduction))
                    {
                        info.ErrorText = "Total Deduction is not in the right format";
                    }
                    else if (Total_Deduction < 0)
                        info.ErrorText = "Total Deduction is negative ";
                }

                if (propertyName == "Tax_Deducted")
                {
                    if (!FrmImports.IsNumeric(Tax_Deducted))
                    {
                        info.ErrorText = "Total Deducted is not in the right format";
                    }
                    else if (Tax_Deducted < 0)
                        info.ErrorText = "Total Deducted is negative ";
                }

                if (propertyName == "Total_Income")
                {
                    if (!FrmImports.IsNumeric(Total_Income))
                    {
                        info.ErrorText = "Total Income is not in the right format";
                    }
                    else if (Total_Income < 0)
                        info.ErrorText = "Total Income is negative ";
                }

                if (propertyName == "Total_Relief")
                {
                    if (!FrmImports.IsNumeric(Total_Relief))
                    {
                        info.ErrorText = "Total Relief is not in the right format";
                    }
                    else if (Total_Relief < 0)
                        info.ErrorText = "Total Relief is negative ";
                }

                if (propertyName == "Months")
                {
                    if (!FrmImports.IsNumeric(Months))
                    {
                        info.ErrorText = "Months not in the right format";
                    }
                    else if (Months >= 13)
                        info.ErrorText = "Months Excced the Calander Months ";
                }

            }

            public void GetError(ErrorInfo info)
            {
                //throw new NotImplementedException();
                ErrorInfo propertyInfo = new ErrorInfo();
                GetPropertyError("Months", propertyInfo);
                if (propertyInfo.ErrorText == "")
                    GetPropertyError("Total_Income", propertyInfo);
                if (propertyInfo.ErrorText == "")
                    //    GetPropertyError("Total_Relief", propertyInfo);
                    //if (propertyInfo.ErrorText == "")
                    GetPropertyError("Tax_Deducted", propertyInfo);
                if (propertyInfo.ErrorText == "")
                    GetPropertyError("Total_Deduction", propertyInfo);
                if (propertyInfo.ErrorText == "")
                    GetPropertyError("Total_Relief", propertyInfo);
                if (propertyInfo.ErrorText != "")
                    info.ErrorText = "This object has errors";
            }

        }

        public class SearchDirectory
        {
            public int ID { get; set; }
            public String Year { get; set; }

            public string Tin { get; set; }

            public string Filename { get; set; }
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
                var row = view.GetRow(i) as FrmImports.BankImportClass;
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
