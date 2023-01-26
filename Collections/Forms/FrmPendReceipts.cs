using Collection.Classess;
using DevExpress.XtraGrid.Views.Grid;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;
using TaxSmartSuite.Class;

namespace Collection.Forms
{
    public partial class FrmPendReceipts : Form
    {
        private DataTable dt;

        private System.Data.DataSet ds;

        public static FrmPendReceipts publicStreetGroup;

        public static FrmPendReceipts publicInstance;

        SqlDataAdapter ada;

        string query, criteria;

        public FrmPendReceipts()
        {
            InitializeComponent();

            ToolStripEvent();

            publicStreetGroup = this;

            setImages();

            setReload();

            btnSearch.Click += btnSearch_Click;

            bttnUpdate.Click += bttnUpdate_Click;

            gridView1.ValidatingEditor += gridView1_ValidatingEditor;
        }

        void bttnUpdate_Click(object sender, EventArgs e)
        {
            //ds.AcceptChanges();
            ada.UpdateCommand = new SqlCommandBuilder(ada).GetUpdateCommand();
            ada.Update(ds);

            setReload();
        }

        void gridView1_ValidatingEditor(object sender, DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventArgs e)
        {
            GridView view = sender as GridView;

            if (view != null)
            {
                if (view.FocusedColumn.FieldName == "ControlNumber")
                {
                    object obj = e.Value;//get the value of the cell

                    if (!Logic.IsNumber((string)obj))
                    {
                        e.Valid = false;

                        e.ErrorText = " Control number can only be in number values ";
                    }
                    else
                    {
                        //if the false
                        if (Logic.CheckRnageValue((string)obj))
                        {
                            e.Valid = false;//set the error valid to false

                            //display error message
                            e.ErrorText = " Control Number Already Exit ";
                        }

                        if (!Logic.CheckRangeValue4mTable((string)obj))
                        {
                            e.Valid = false;//set the error valid to false

                            //display error message
                            e.ErrorText = " Control Number Not in The Issue Range ";
                        }

                    }

                }
            }
        }

        void btnSearch_Click(object sender, EventArgs e)
        {
            //GridView view;
            if (gridView1.RowCount == 0)
            {
                using (FrmReceiptDemand forms = new FrmReceiptDemand())
                {
                    forms.ShowDialog();
                }
            }
            else
            {
                Common.setMessageBox(" There are some Receipt Transaction to be completed " + " Please Complete them before continue ", Program.ApplicationName, 3);
                return;

            }

        }

        private void setImages()
        {
            tsbNew.Image = MDIMain.publicMDIParent.i16x16.Images[5];
            tsbEdit.Image = MDIMain.publicMDIParent.i16x16.Images[6];
            tsbDelete.Image = MDIMain.publicMDIParent.i16x16.Images[7];
            tsbReload.Image = MDIMain.publicMDIParent.i16x16.Images[0];
            tsbClose.Image = MDIMain.publicMDIParent.i16x16.Images[11];

            //bttnClose.Image = MDIMain.publicMDIParent.i16x16.Images[11];
            //bttnBrowse.Image = MDIMain.publicMDIParent.i32x32.Images[7];
            //btnPrint.Image = MDIMain.publicMDIParent.i32x32.Images[29];
            bttnUpdate.Image = MDIMain.publicMDIParent.i32x32.Images[7];
            btnSearch.Image = MDIMain.publicMDIParent.i32x32.Images[30];
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
                //Close();
            }

        }

        private void setReload()
        {
            //DataTable dt = ds.Tables[0];

            ds = new System.Data.DataSet();

            //connect.connect.Open();
            ada = new SqlDataAdapter("SELECT PaymentRefNumber,UPPER(PayerName) AS PayerName, Amount, EReceipts,ControlNumber,PrintedBY,DatePrinted FROM tblCollectionReport WHERE isPrinted=1 AND ControlNumber IS NULL", Logic.ConnectionString);

            //{
            ada.Fill(ds, "table");
            //}
            dt = ds.Tables[0];
            //connect.connect.Close();
            gridControl1.DataSource = ds;
            gridControl1.DataMember = "table";


            //ds.AcceptChanges();


            gridView1.BestFitColumns();
            gridView1.Columns["PaymentRefNumber"].OptionsColumn.AllowEdit = false;
            gridView1.Columns["PayerName"].OptionsColumn.AllowEdit = false;
            gridView1.Columns["Amount"].OptionsColumn.AllowEdit = false;
            gridView1.Columns["EReceipts"].OptionsColumn.AllowEdit = false;

            gridView1.Columns["ControlNumber"].OptionsColumn.AllowEdit = true;
            gridView1.Columns["PrintedBY"].OptionsColumn.AllowEdit = false;
            gridView1.Columns["DatePrinted"].OptionsColumn.AllowEdit = false;

            gridView1.Columns["Amount"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            gridView1.Columns["Amount"].DisplayFormat.FormatString = "n2";
            gridView1.Columns["DatePrinted"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            gridView1.Columns["DatePrinted"].DisplayFormat.FormatString = "dd MMM yyyy";
            AddUser(gridView1);
            gridControl1.ForceInitialize();


        }

        void testrange()
        {
            //if (Enumerable.Range(17251, 19250).Contains(range))
            //{ 
            //}

            if (Enumerable.Range(17251, 19250).Contains(17350))
            {
                MessageBox.Show(" Record Found ");
            }

        }

        void AddUser(GridView view)
        {
            if (view != null && view.RowCount > 0)
            {
                for (int i = 0; i < view.RowCount; i++)
                {
                    view.SetRowCellValue(i, view.Columns["PrintedBY"], Program.UserID);

                    view.SetRowCellValue(i, view.Columns["DatePrinted"], DateTime.Today);
                }
            }
        }

        public void RefreshForm()
        {
            NavBars.ToolStripEnableDisableControls(toolStrip, Tag as String);
        }


        //static bool CheckRnageValue(string ContNum)
        //{
        //    bool bRes;

        //    string sql = String.Format("SELECT COUNT(*) AS Count FROM tblCollectionReport WHERE (ControlNumber = '{0}')", ContNum);

        //    if (new Logic().IsRecordExist(sql))
        //        //if (retval == "1")
        //        bRes = true;
        //    else
        //        bRes = false;
        //    return bRes;
        //}

    }
}
