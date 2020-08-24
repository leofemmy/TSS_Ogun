using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using Collection.Classess;
using TaxSmartSuite;
using TaxSmartSuite.Class;
using System.Collections;
using DevExpress.XtraGrid.Views.Grid;
using Collection.Report;
using DevExpress.Utils;
using System.Linq;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.DXErrorProvider;
using DevExpress.XtraPrinting;
using DevExpress.XtraSplashScreen;
using Collections;
using DevExpress.XtraReports.UI;
using DevExpress.XtraReports.Parameters;



namespace Collection.Forms
{
    public partial class FrmMainFest : Form
    {
        private SqlCommand _command;
        private bool bResponse;

        private DataTable Dts;

        DataTable dtres = new DataTable();

        DataTable dtrep = new DataTable();

        DataTable dtpay = new DataTable();

        private System.Data.DataSet ds;

        SqlDataAdapter ada;

        string query;

        ArrayList myArrayList = new ArrayList();

        ArrayList myAL = new ArrayList();

        ArrayList myAL2 = new ArrayList();

        int arrycount, arrycount1;

        string[] split;
        //int test;
        int k, test, gbj;

        private SqlCommand command;

        private SqlDataAdapter adp;

        //private DataSet ds = new DataSet();

        System.Data.DataTable dt;
        System.Data.DataTable dt2;

        DataTable temTable = new DataTable();

        DataTable temControl = new DataTable();

        string BatchNumber;

        private string user;

        public static FrmMainFest publicStreetGroup;

        protected TransactionTypeCode iTransType;

        protected bool boolIsUpdate;

        string condition, condition2, condition3;

        public static FrmMainFest publicInstance;

        public string IDList;

        string[] splitS;

        string[] Split2;

        private int optionRece;

        int number = 0;

        int count;// definie count for template ussage

        string StrCltNumber = string.Empty;

        bool IsShowDialog;


        public FrmMainFest()
        {
            InitializeComponent();
            Init();

        }

        public FrmMainFest(String strSelectedList, string querys, string querys2, bool IsShowDialog)
        {
            InitializeComponent();
            if (String.IsNullOrEmpty(strSelectedList))
                DialogResult = DialogResult.Abort;
            IDList = strSelectedList;
            condition = querys;
            condition2 = querys2;
            this.IsShowDialog = IsShowDialog;
            if (!this.IsShowDialog)
                Init();


        }

        public FrmMainFest(String strSelectedList, string querys, string querys2, bool IsShowDialog, int ReceiptOption)
        {
            InitializeComponent();
            if (String.IsNullOrEmpty(strSelectedList))
                DialogResult = DialogResult.Abort;
            IDList = strSelectedList;
            condition = querys;
            condition2 = querys2;
            optionRece = ReceiptOption;
            this.IsShowDialog = IsShowDialog;
            if (!this.IsShowDialog)
                Init();


        }
        void Init()
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

                publicInstance = this;

                publicStreetGroup = this;

                setImages();

                ToolStripEvent();

                iTransType = TransactionTypeCode.New;

                //Load += OnFormLoad;

                //generate number
                BatchNumber = String.Format("{0:d9}", (DateTime.Now.Ticks / 10) % 1000000000);

                gbj = 0;

                SetReload();

                btnApply.Click += btnApply_Click;

                btnPrint.Click += btnPrint_Click;

                btnTelly.Click += btnTelly_Click;

                btnuse.Click += btnuse_Click;

                txtStart.LostFocus += txtStart_LostFocus;

                txtExpec.LostFocus += txtExpec_LostFocus;

                if (Program.UserID == "" || Program.UserID == null)
                {
                    user = "Femi";
                }
                else
                {
                    user = Program.UserID;
                }
                gridView1.ValidatingEditor += gridView1_ValidatingEditor;
                gridView1.HiddenEditor += gridView1_HiddenEditor;

                gridView1.ValidatingEditor += gridView1_ValidatingEditor;
                //gridView1.ValidateRow += gridView1_ValidateRow;
                //gridView1.InvalidRowException += gridView1_InvalidRowException;

                //temTable.Columns.Add("EReceipt", typeof(string));
                //temTable.Columns.Add("PaymentRefNumber", typeof(string));
                //temTable.Columns.Add("UserId", typeof(string));


                temControl.Columns.Add("EReceipt", typeof(string));
                temControl.Columns.Add("PaymentRefNumber", typeof(string));
                temControl.Columns.Add("UserId", typeof(string));

                //this.IsShowDialog = IsShowDialog;


            }
            finally
            {
                SplashScreenManager.CloseForm(false);
            }
        }

        void btnuse_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtst.Text))
            {
                Common.setEmptyField("Control Number Start", "Mainfest"); return; txtst.Focus();
            }
            else if (string.IsNullOrEmpty(txtend.Text))
            {
                Common.setEmptyField("Control Number End", "Mainfest"); return; txtend.Focus();
            }
            else
            {

                try
                {
                    //SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

                    //IncrementColumn(0, txtst.Text, "ControlNumber", txtend.Text);
                    using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
                    {
                        connect.Open();
                        _command = new SqlCommand("ControlnumberCkecker", connect) { CommandType = CommandType.StoredProcedure };
                        _command.Parameters.Add(new SqlParameter("@startrange", SqlDbType.VarChar)).Value = txtst.Text;
                        _command.Parameters.Add(new SqlParameter("@endrange", SqlDbType.VarChar)).Value = txtend.Text;

                        //_command.CommandTimeout = 1200;
                        _command.CommandTimeout = 0;

                        System.Data.DataSet response = new System.Data.DataSet();

                        adp = new SqlDataAdapter(_command);
                        adp.Fill(response);

                        connect.Close();
                        if (response.Tables[0].Rows[0]["returnCode"].ToString() != "00")
                        {
                            Common.setMessageBox(response.Tables[0].Rows[0]["returnMessage"].ToString(), "Manifest", 2); return;
                        }
                        else
                        {
                            if (response.Tables[2].Rows.Count > 0)
                            {
                                Frmcontrol frmcontrol = new Frmcontrol();
                                frmcontrol.gridControl1.DataSource = response.Tables[2];
                                frmcontrol.gridView1.BestFitColumns();
                                frmcontrol.label1.Text = "The control number(s) listed below have been used already.";
                                frmcontrol.Text = "Used Control Number / Payment Ref. Number";
                                frmcontrol.ShowDialog();
                            }
                            //if (dtget.Rows.Count > 0)dts.Tables[1].Rows.Count > 0
                            if (response.Tables[1].Rows.Count > 0)
                            {
                                Gettable(response);
                            }
                            btnApply.Enabled = true;

                        }
                    }
                }
                finally
                {
                    //SplashScreenManager.CloseForm(false);
                }
            }
        }

        void btnTelly_Click(object sender, EventArgs e)
        {

            if (dt != null && dt.Rows.Count > 0)
            {
                //using object class
                var list = (from DataRow row in dt.Rows
                            select new DataSet.ReportManifestClass
                            {
                                Amount = Convert.ToDecimal(row["Amount"]),
                                ControlNumber = row["ControlNumber"] as string,
                                EReceiptNo = row["EReceipts"] as string,
                                Status = row["Mark x or ✓"] as string
                            }
                                ).ToList();

                XtraRepTelly rep = new XtraRepTelly();
                rep.xrLabel6.Text = Program.UserID;
                rep.xrLabel8.Text = string.Format("{0:dd/MM/yyy  hh:mm:ss}", DateTime.Now);
                var binding = (BindingSource)rep.DataSource;
                binding.DataSource = list;

                rep.ShowPreviewDialog();
            }




        }

        void link_CreateReportHeaderArea(object sender, CreateAreaEventArgs e)
        {
            //string reportHeader = "Report Header";
            //e.Graph.StringFormat = new BrickStringFormat(StringAlignment.Center);
            //e.Graph.Font = new Font("Tahoma", 14, FontStyle.Bold);
            //e.Graph.DrawString(reportHeader, Color.Black, rec, BorderSide.None);

        }

        void ValidateData()
        {
            GridView view = gridView1;

            int rowHandle = view.FocusedRowHandle;


            if (view != null)
            {
                if (view.FocusedColumn.FieldName == "ControlNumber")
                {
                    //object obj = view. e.Value;//get the value of the cell
                    //object obj=view.get
                    System.Data.DataRow row = gridView1.GetDataRow(gridView1.FocusedRowHandle);

                    string cellValue = row["ControlNumber"].ToString();

                    if (string.IsNullOrEmpty(cellValue))
                    {
                        return;
                    }
                    else
                    {

                        if (!Logic.IsNumber((string)cellValue))
                        {
                            view.SetColumnError(view.Columns["ControlNumber"], " Control number can only be in number values ", ErrorType.Critical);

                            return;
                        }
                        else
                        {
                            if (Logic.CheckRnageValue((string)cellValue))
                            {
                                view.SetColumnError(view.Columns["ControlNumber"], " Control Number Already Exit ", ErrorType.Critical); return;

                            }
                            else if (!Logic.CheckRangeValue4mTable(cellValue))
                            {
                                view.SetColumnError(view.Columns["ControlNumber"], " Control Number Not in The Issue Range ", ErrorType.Critical); return;
                            }
                            else
                            {
                                view.SetColumnError(view.Columns["ControlNumber"], string.Empty, ErrorType.None);
                            }


                        }
                    }
                    //cellValue = string.Empty;
                }
            }

        }

        void gridView1_InvalidRowException(object sender, DevExpress.XtraGrid.Views.Base.InvalidRowExceptionEventArgs e)
        {
            e.ExceptionMode = ExceptionMode.NoAction;

        }

        void gridView1_ValidateRow(object sender, DevExpress.XtraGrid.Views.Base.ValidateRowEventArgs e)
        {
            ValidateData();
            #region

            //ColumnView view = sender as ColumnView;

            //GridColumn column1 = view.Columns["ControlNumber"];

            //if (column1 != null)
            //{
            //    object obj = view.GetRowCellValue(e.RowHandle, column1);

            //    if (!Logic.IsNumber((string)obj))
            //    {
            //        e.Valid = false;
            //        //e.ErrorText = "Control number can only be in number values";
            //        view.SetColumnError(column1, "Control number can only be in number values");
            //    }
            //    else
            //    {
            //        //if the false
            //        if (Logic.CheckRnageValue((string)obj))
            //        {
            //            e.Valid = false;//set the error valid to false

            //            //display error message
            //            //e.ErrorText = " Control Number Already Exit ";
            //            view.SetColumnError(column1, "Control Number Already Exit");
            //        }

            //        if (!Logic.CheckRangeValue4mTable(Convert.ToInt32(obj)))
            //        {
            //            e.Valid = false;//set the error valid to false

            //            //display error message
            //            //e.ErrorText = " Control Number Not in The Issue Range";
            //            view.SetColumnError(column1, "Control Number Not in The Issue Range");
            //        }
            //    }


            //    }

            //GridView view = sender as GridView;

            //if (view != null)
            //{
            //    if (view.FocusedColumn.FieldName == "ControlNumber")
            //    {
            //        object obj = e.Value;//get the value of the cell

            //if (!Logic.IsNumber((string)obj))
            //{
            //    e.Valid = false;
            //    e.ErrorText = "Control number can only be in number values";
            //}
            //else
            //{
            //    //if the false
            //    if (Logic.CheckRnageValue((string)obj))
            //    {
            //        e.Valid = false;//set the error valid to false

            //        //display error message
            //        e.ErrorText = " Control Number Already Exit ";
            //    }

            //    if (!Logic.CheckRangeValue4mTable(Convert.ToInt32(obj)))
            //    {
            //        e.Valid = false;//set the error valid to false

            //        //display error message
            //        e.ErrorText = " Control Number Not in The Issue Range";
            //    }

            //}

            //    }
            //}

            #endregion

        }

        void gridView1_HiddenEditor(object sender, EventArgs e)
        {
            GridView view = sender as GridView;

            if (view != null)
            {
                if (view.FocusedColumn.FieldName == "ControlNumber")
                {
                    int rowHandle = view.FocusedRowHandle;

                    object obj = view.GetRowCellValue(rowHandle, "ControlNumber");


                    if (string.IsNullOrEmpty(obj.ToString()))
                    {
                        return;
                    }
                    else
                    {
                        if (obj != System.DBNull.Value)
                        {
                            string ctrlNumber = (string)view.GetRowCellValue(rowHandle, "ControlNumber");
                            //IncrementColumn(rowHandle, ctrlNumber, "ControlNumber");
                        }
                        else
                        {
                            return;
                        }
                    }
                }
            }
        }

        void txtExpec_LostFocus(object sender, EventArgs e)
        {
            myAL2.Clear();

            if (string.IsNullOrEmpty(txtExpec.Text)) return;
            number = 0;

            splitS = txtStart.Text.ToString().Split(new Char[] { ',' });

            //check and remove expection from the control list
            for (int i = 0; i < splitS.Count(); i++)// count number of split
            {
                //check with the list and remove
                string gh = splitS[i];
                Split2 = splitS[i].Split(new char[] { '-' });//get the firt split
                //get the length
                //chech the length of each range if equall

                if (Split2[0].Length != Split2[1].Length)
                {
                    MessageBox.Show("Length of the control  number range not equall, please re-enter");
                    txtStart.Clear(); txtStart.Focus(); txtTotal2.Clear();
                    myAL2.Clear();
                    return;
                }
                else
                {
                    int range = Convert.ToInt32(Split2[1]) - Convert.ToInt32(Split2[0]);


                    //create the number range
                    for (int g = 0; g < range; g++)
                    {
                        string contnumber = Convert.ToString(Convert.ToInt32(Split2[0]) + g);

                        contnumber = contnumber.PadLeft(Split2[0].Length, '0');

                        myAL2.Add(contnumber);
                    }

                    number = range + number;

                }
                txtTotal2.Text = Convert.ToString(number);

                if (!string.IsNullOrEmpty(txtTotal1.Text))
                {
                    txttot.Text = Convert.ToString(Convert.ToInt32(txtTotal2.Text) + Convert.ToInt32(txtTotal1.Text));
                }
            }
        }

        void txtStart_LostFocus(object sender, EventArgs e)
        {
            myAL.Clear();
            if (string.IsNullOrEmpty(txtStart.Text)) return;
            splitS = txtStart.Text.Split(new Char[] { ',' });

            //check and remove expection from the control list
            for (int i = 0; i < splitS.Count(); i++)// count number of split
            {
                //check with the list and remove
                string gh = splitS[i];
                Split2 = splitS[i].Split(new char[] { '-' });//get the firt split
                //get the length
                //chech the length of each range if equall

                if (Split2[0].Length != Split2[1].Length)
                {
                    MessageBox.Show("Length of the control  number range not equall, please re-enter");
                    txtStart.Clear(); txtStart.Focus(); txtTotal1.Clear();
                    myAL.Clear();
                    return;
                }
                else
                {
                    int range = Convert.ToInt32(Split2[1]) - Convert.ToInt32(Split2[0]);


                    //create the number range
                    for (int g = 0; g < range; g++)
                    {
                        string contnumber = Convert.ToString(Convert.ToInt32(Split2[0]) + g);

                        contnumber = contnumber.PadLeft(Split2[0].Length, '0');

                        myAL.Add(contnumber);
                    }

                    number = range + number;

                }
                txtTotal1.Text = Convert.ToString(number);

                if (!string.IsNullOrEmpty(txtTotal2.Text))
                {
                    txttot.Text = Convert.ToString(Convert.ToInt32(txtTotal2.Text) + Convert.ToInt32(txtTotal1.Text));
                }
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
            btnApply.Image = MDIMain.publicMDIParent.i32x32.Images[8];
            btnPrint.Image = MDIMain.publicMDIParent.i32x32.Images[29];
            //btnSearch.Image = MDIMain.publicMDIParent.i32x32.Images[2];

        }

        void btnPrint_Click(object sender, EventArgs e)
        {
            string vquery;
            //check if the control number is apply to printed receipts
            //if (gridView1.RowCount == 0)
            //if (ds.Tables[0].Rows.Count == 0)
            //{
            try
            {

                query = string.Format(@"SELECT  AgencyCode ,
        AgencyName ,
        PaymentRefNumber ,
        [DepositSlipNumber] ,
        [PaymentDate] ,
        [PayerID] ,
        UPPER([PayerName]) AS PayerName ,
        [Description] ,
        [Amount] ,
        [BankName] ,
        [BranchName] ,
         ReceiptNo AS [EReceipts] ,
        [GeneratedBy] ,
        [ControlNumber] ,
        [BatchNumber] ,
        [Printedby] ,
        --[EReceiptsDate] ,
        DatePrinted ,
        [UploadStatus],StationCode,(Select StationName from tblStation2 WHERE tblStation2.StationCode = tblReceipt.[StationCode]) AS StationName
FROM   tblCollectionReport
WHERE  PaymentRefNumber IN (SELECT PaymentRefNumber FROM  [tblReceipt] ) AND Isprinted=1 AND PrintedBY='{0}' AND LEN(ControlNumber)<>0 ", Program.UserID);

                using (System.Data.DataSet dds = new System.Data.DataSet("DsCollectionReport"))
                {
                    using (SqlDataAdapter ada = new SqlDataAdapter(query, Logic.ConnectionString))
                    {
                        ada.Fill(dds, "CollectionReportTable");
                        XRepManifest repManifest = new XRepManifest { DataSource = dds, DataAdapter = ada, DataMember = "CollectionReportTable", RequestParameters = false };
                        repManifest.xrLabel10.Text = Program.UserID;
                        repManifest.ShowPreviewDialog();
                    }
                }

            }
            catch
            {

                query = string.Format(@"SELECT  AgencyCode ,
        AgencyName ,
        PaymentRefNumber ,
        [DepositSlipNumber] ,
        [PaymentDate] ,
        [PayerID] ,
        UPPER([PayerName]) AS PayerName ,
        [Description] ,
        [Amount] ,
        [BankName] ,
        [BranchName] ,
         ReceiptNo AS [EReceipts] ,
        [GeneratedBy] ,
        [ControlNumber] ,
        [BatchNumber] ,
        [Printedby] ,
        --[EReceiptsDate] ,
        DatePrinted ,
        [UploadStatus],StationCode,(Select StationName from tblStation2 WHERE tblStation2.StationCode = tblReceipt.[StationCode]) AS StationName
FROM   tblCollectionReport
WHERE  PaymentRefNumber IN (SELECT PaymentRefNumber FROM  [tblReceipt] ) AND Isprinted=1 AND PrintedBY='{0}' AND LEN(ControlNumber)<>0", Program.UserID);


                using (System.Data.DataSet dds = new System.Data.DataSet("DsCollectionReport"))
                {
                    using (SqlDataAdapter ada = new SqlDataAdapter(query, Logic.ConnectionString))
                    {
                        ada.Fill(dds, "CollectionReportTable");
                        XRepManifest repManifest = new XRepManifest { DataSource = dds, DataAdapter = ada, DataMember = "CollectionReportTable", RequestParameters = false };
                        repManifest.ShowPreviewDialog();
                    }
                }
            }

            using (WaitDialogForm form = new WaitDialogForm("Application Working...,Please Wait...", "Processing"))
            {

                using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
                {
                    connect.Open();
                    _command = new SqlCommand("DeletetblReceipt", connect) { CommandType = CommandType.StoredProcedure };
                    _command.Parameters.Add(new SqlParameter("@tblCollectionReport_Temp", SqlDbType.Structured)).Value = temTable;

                    System.Data.DataSet response = new System.Data.DataSet();
                    adp = new SqlDataAdapter(_command);
                    adp.Fill(response);

                    connect.Close();
                    if (response.Tables[0].Rows[0]["returnCode"].ToString() == "00")
                    {
                    }
                    else
                    {
                        Common.setMessageBox(response.Tables[0].Rows[0]["returnMessage"].ToString(), "", 2); return;
                    }


                }
            }
        }

        string GetPaymentRef()
        {
            string values = string.Empty;

            int j = 0;

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                values += String.Format("'{0}'", ds.Tables[0].Rows[i]["PaymentRefNumber"].ToString().Trim());

                if (j + 1 < ds.Tables[0].Rows.Count)

                    values += ",";

                ++j;

            }

            return values;
        }

        void btnApply_Click(object sender, EventArgs e)
        {

            DialogResult result = MessageBox.Show(" By clicking on 'YES', you are accepting that the combination is correct and should be held responsible for any error ?", Program.ApplicationName, MessageBoxButtons.YesNo);

            if (result == DialogResult.Yes)
            {
                //if(ds.Tables[0].Rows[0]["ROWNAME"] == DbNull.value) {do stuff}

                if (ds.Tables[0].Rows[0]["ControlNumber"] == DBNull.Value)
                {
                    Common.setMessageBox(" Control Number Not Entred !", Program.ApplicationName, 1);
                    return;
                }
                else
                {
                    ////loop gridview

                    XtraRepPayment payment = new XtraRepPayment(); XtraRepPayment Reprint = new XtraRepPayment(); XtraRepPayment Reversal = new XtraRepPayment();
                    XRepManifest repManifest = new XRepManifest(); XRepManifest UnReceipt = new XRepManifest();

                    //calling store procedure
                    try
                    {
                        //SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

                        //if (Program.stateCode == "20")
                        //{
                        //    try
                        //    {
                        //        //SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

                        //        using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
                        //        {
                        //            connect.Open();
                        //            _command = new SqlCommand("ApplyControlNumberDelta", connect)
                        //            {
                        //                CommandType = CommandType.StoredProcedure
                        //            };
                        //            _command.Parameters.Add(new SqlParameter("@userid", SqlDbType.VarChar)).Value =
                        //                Program.UserID;
                        //            _command.Parameters.Add(new SqlParameter("@Receop", SqlDbType.VarChar)).Value =
                        //               optionRece;
                        //            _command.Parameters.Add(new SqlParameter("@tblCollectionReport_Temp",
                        //                SqlDbType.Structured)).Value = ds.Tables[0];

                        //            System.Data.DataSet response = new System.Data.DataSet();

                        //            adp = new SqlDataAdapter(_command);
                        //            adp.Fill(response);

                        //            connect.Close();
                        //            if (response.Tables[0].Rows[0]["returnCode"].ToString() != "00")
                        //            {

                        //                Frmcontrol frmcontrol = new Frmcontrol();
                        //                frmcontrol.gridControl1.DataSource = response.Tables[1];
                        //                frmcontrol.gridView1.BestFitColumns();
                        //                frmcontrol.label1.Text = "Duplicate Control Number";
                        //                frmcontrol.Text = "Duplicate Used Control Number";
                        //                frmcontrol.ShowDialog();

                        //            }
                        //            else
                        //            {
                        //                UpdateIssueTemp(ds.Tables[0].Rows[0]["ControlNumber"].ToString(),
                        //                    ds.Tables[0].Rows.Count);

                        //                if (response.Tables[1] != null && response.Tables[1].Rows.Count > 0)//Main Manifest
                        //                {
                        //                    using (System.Data.DataSet dds = new System.Data.DataSet("DsCollectionReport"))
                        //                    {
                        //                        using (SqlDataAdapter ada = new SqlDataAdapter(query, Logic.ConnectionString))
                        //                        {
                        //                            ada.Fill(dds, "CollectionReportTable");
                        //                            //XRepManifest repManifest = new XRepManifest
                        //                            //{
                        //                            repManifest.DataSource = response.Tables[1];
                        //                            repManifest.DataAdapter = ada;
                        //                            UnReceipt.DataAdapter = ada;
                        //                            repManifest.DataMember = "CollectionReportTable";
                        //                            repManifest.RequestParameters = false;
                        //                            //};
                        //                            repManifest.xrLabel10.Text = Program.UserID;
                        //                            repManifest.xrLabel9.Text = string.Format("{0} STATE GOVERNMENT",
                        //                                Program.StateName.ToUpper());
                        //                            //repManifest.ShowPreviewDialog();
                        //                            repManifest.CreateDocument();
                        //                        }
                        //                    }
                        //                }


                        //                // Reset all page numbers in the resulting document.
                        //                repManifest.PrintingSystem.ContinuousPageNumbering = true;
                        //                repManifest.logoPath = Logic.logopth;
                        //                // Show the Print Preview form.
                        //                repManifest.ShowPreviewDialog();

                        //                SetReload();
                        //                txtend.Text = string.Empty;
                        //                txtst.Text = string.Empty;

                        //                return;
                        //            }
                        //        }
                        //    }
                        //    catch (Exception ex)
                        //    {
                        //        Tripous.Sys.ErrorBox(ex.StackTrace.ToString() + ex.Message.ToString()); return;
                        //    }
                        //    finally
                        //    {
                        //        //SplashScreenManager.CloseForm(false);
                        //    }
                        //}
                        //else
                        //{
                        try
                        {
                            //SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                            if (ds.Tables[0].Columns.Contains("CurrencyCode")) ds.Tables[0].Columns.Remove("CurrencyCode");
                            ds.Tables[0].AcceptChanges();

                            using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
                            {
                                connect.Open();
                                _command = new SqlCommand("ApplyControlNumber", connect)
                                {
                                    CommandType = CommandType.StoredProcedure
                                };
                                _command.Parameters.Add(new SqlParameter("@userid", SqlDbType.VarChar)).Value =
                                    Program.UserID;
                                _command.Parameters.Add(new SqlParameter("@tblCollectionReport_Temp",
                                    SqlDbType.Structured)).Value = ds.Tables[0];
                                _command.CommandTimeout = 0;

                                System.Data.DataSet response = new System.Data.DataSet();

                                adp = new SqlDataAdapter(_command);
                                adp.Fill(response);

                                connect.Close();
                                if (response.Tables[0].Rows[0]["returnCode"].ToString() != "00")
                                {

                                    Frmcontrol frmcontrol = new Frmcontrol();
                                    frmcontrol.gridControl1.DataSource = response.Tables[1];
                                    frmcontrol.gridView1.BestFitColumns();
                                    frmcontrol.label1.Text = "Duplicate Control Number";
                                    frmcontrol.Text = "Duplicate Used Control Number";
                                    frmcontrol.ShowDialog();

                                }
                                else
                                {
                                    UpdateIssueTemp(ds.Tables[0].Rows[0]["ControlNumber"].ToString(),
                                        ds.Tables[0].Rows.Count);

                                    if (response.Tables[1] != null && response.Tables[1].Rows.Count > 0)//Main Manifest
                                    {
                                        using (System.Data.DataSet dds = new System.Data.DataSet("DsCollectionReport"))
                                        {
                                            using (SqlDataAdapter ada = new SqlDataAdapter(query, Logic.ConnectionString))
                                            {
                                                ada.Fill(dds, "CollectionReportTable");

                                                repManifest.DataSource = response.Tables[1];
                                                repManifest.DataAdapter = ada;
                                                repManifest.DataMember = "CollectionReportTable";
                                                repManifest.RequestParameters = false;
                                                UnReceipt.DataAdapter = ada;
                                                repManifest.xrLabel10.Text = Program.UserID;
                                                repManifest.xrLabel12.Text = "MANIFEST OF PRINTED RECEIPTS";
                                                repManifest.xrLabel9.Text = string.Format("{0} STATE GOVERNMENT",
                                                    Program.StateName.ToUpper());
                                                repManifest.logoPath = Logic.logopth;
                                                //repManifest.ShowPreviewDialog();

                                                repManifest.CreateDocument();

                                            }
                                        }
                                    }



                                    // Reset all page numbers in the resulting document.
                                    repManifest.PrintingSystem.ContinuousPageNumbering = true;

                                    // Show the Print Preview form.
                                    repManifest.ShowPreviewDialog();

                                    SetReload();
                                    txtend.Text = string.Empty;
                                    txtst.Text = string.Empty;

                                    return;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Tripous.Sys.ErrorBox(ex.StackTrace.ToString() + ex.Message.ToString()); return;
                        }
                        finally
                        {
                            //SplashScreenManager.CloseForm(false);
                        }
                        ///payment change report
                        //}
                    }
                    catch (Exception ex)
                    {
                        Tripous.Sys.ErrorBox(ex.StackTrace.ToString() + ex.Message.ToString()); return;
                    }
                    finally
                    {
                        //SplashScreenManager.CloseForm(false);
                    }


                }

            }
            else
            {
                return;
            }

        }

        void InsertReceiptHistory()
        {

            using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
            {
                connect.Open();
                command = new SqlCommand("InsertPrintReceiptHistory", connect) { CommandType = CommandType.StoredProcedure };

                using (System.Data.DataSet ds = new System.Data.DataSet())
                {
                    adp = new SqlDataAdapter(command);
                    adp.Fill(ds);
                    Dts = ds.Tables[0];
                    connect.Close();
                    bResponse = true;
                }
            }
        }

        void SetReload()
        {
            try
            {
                //if (Program.stateCode == "20")
                //{
                //    //intval = Convert.ToInt32(radioGroup1.EditValue);
                //    if (string.IsNullOrEmpty(condition))
                //    {

                //        query = string.Format("SELECT AgencyCode , AgencyName , ViewReceiptMainFest.PaymentRefNumber , RevenueCode,[DepositSlipNumber] ,PaymentDate, [PayerID] , UPPER([PayerName]) AS PayerName ,[Description] ,[Amount] ,[BankName] ,[BranchName] ,GeneratedBy, EReceipts,StationCode,[ControlNumber] ,[BatchNumber],PrintedBY FROM ViewReceiptMainFest INNER JOIN Receipt.tblReceipt ON ViewReceiptMainFest.PaymentRefNumber = tblReceipt.PaymentRefNumber WHERE PrintedBY='{0}' AND ControlNumber IS NULL AND ReceiptOptInt=1 ORDER BY AgencyName ,Description,EReceipts", Program.UserID);
                //    }
                //    else
                //    {
                //        query = string.Format("SELECT AgencyCode , AgencyName , ViewReceiptMainFest.PaymentRefNumber , RevenueCode,[DepositSlipNumber] ,PaymentDate, [PayerID] , UPPER([PayerName]) AS PayerName ,[Description] ,[Amount] ,[BankName] ,[BranchName] ,GeneratedBy, EReceipts,StationCode,[ControlNumber] ,[BatchNumber],PrintedBY FROM ViewReceiptMainFest INNER JOIN Receipt.tblReceipt ON ViewReceiptMainFest.PaymentRefNumber = tblReceipt.PaymentRefNumber WHERE  ViewReceiptMainFest.PaymentRefNumber  in ({0}) AND ReceiptOptInt=1 ORDER BY AgencyName ,Description,EReceipts", condition);
                //    }
                //}
                //else
                //{

                switch (Program.intCode)
                {
                    case 20://detla state
                        if (string.IsNullOrEmpty(condition))
                        {

                            query = string.Format("SELECT AgencyCode , AgencyName ,PaymentRefNumber, RevenueCode,[DepositSlipNumber] ,PaymentDate, [PayerID] , UPPER([PayerName]) AS PayerName ,[Description] ,CurrencyCode,[Amount] ,[BankName] ,[BranchName] ,GeneratedBy, [EReceipts],StationCode,[ControlNumber] ,[BatchNumber],PrintedBY FROM ViewReceiptMainFest WHERE PrintedBY='{0}' AND ControlNumber IS NULL ORDER BY AgencyName ,Description,EReceipts", Program.UserID);
                        }
                        else
                        {
                            query = string.Format("SELECT AgencyCode , AgencyName ,PaymentRefNumber, RevenueCode,[DepositSlipNumber] ,PaymentDate, [PayerID] , UPPER([PayerName]) AS PayerName ,[Description],CurrencyCode ,[Amount] ,[BankName] ,[BranchName] ,GeneratedBy, [EReceipts],StationCode,[ControlNumber] ,[BatchNumber],PrintedBY FROM ViewReceiptMainFest WHERE PaymentRefNumber in({0}) ORDER BY AgencyName ,Description,EReceipts", condition);
                        }
                        break;
                    default:
                        if (string.IsNullOrEmpty(condition))
                        {

                            query = string.Format("SELECT AgencyCode , AgencyName ,PaymentRefNumber, RevenueCode,[DepositSlipNumber] ,PaymentDate, [PayerID] , UPPER([PayerName]) AS PayerName ,[Description],CurrencyCode ,[Amount] ,[BankName] ,[BranchName] ,GeneratedBy, EReceipts,StationCode,[ControlNumber] ,[BatchNumber],PrintedBY FROM ViewReceiptMainFest WHERE PrintedBY='{0}' AND ControlNumber IS NULL ORDER BY AgencyName ,Description,EReceipts", Program.UserID);
                        }
                        else
                        {
                            query = string.Format("SELECT AgencyCode , AgencyName ,PaymentRefNumber, RevenueCode,[DepositSlipNumber] ,PaymentDate, [PayerID] , UPPER([PayerName]) AS PayerName ,[Description],CurrencyCode ,[Amount] ,[BankName] ,[BranchName] ,GeneratedBy, EReceipts,StationCode,[ControlNumber] ,[BatchNumber],PrintedBY FROM ViewReceiptMainFest WHERE PaymentRefNumber in({0}) ORDER BY AgencyName ,Description,EReceipts", condition);
                        }
                        break;


                }

                //}
                dt = new DataTable();
                dt.Clear();
                ds = new System.Data.DataSet();
                ds.Clear();


                ada = new SqlDataAdapter(query, Logic.ConnectionString);

                ada.SelectCommand.CommandTimeout = 0;

                ada.Fill(ds, "table");



                dt = ds.Tables[0];

                if (dt != null && dt.Rows.Count > 0)
                {
                    dt.Columns.Add("Mark x or ✓", typeof(string));
                    dt.AcceptChanges();

                    gridControl1.DataSource = dt;
                    //gridControl1.DataMember = "table";
                    gridView1.BestFitColumns();
                    //gridView1.BestFitColumns();
                    gridView1.Columns["PaymentRefNumber"].OptionsColumn.AllowEdit = false;
                    gridView1.Columns["DepositSlipNumber"].OptionsColumn.AllowEdit = false;
                    gridView1.Columns["DepositSlipNumber"].Visible = false;
                    gridView1.Columns["PaymentDate"].OptionsColumn.AllowEdit = false;
                    gridView1.Columns["PaymentDate"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
                    gridView1.Columns["PaymentDate"].DisplayFormat.FormatString = "dd/MM/yyyy";

                    gridView1.Columns["PayerID"].OptionsColumn.AllowEdit = false;
                    gridView1.Columns["PayerName"].OptionsColumn.AllowEdit = false;
                    gridView1.Columns["Description"].OptionsColumn.AllowEdit = false;
                    //gridView1.Columns["Description"].Visible = false;
                    gridView1.Columns["Amount"].OptionsColumn.AllowEdit = false;
                    gridView1.Columns["Amount"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                    gridView1.Columns["Amount"].DisplayFormat.FormatString = "n2";
                    gridView1.Columns["BankName"].OptionsColumn.AllowEdit = false;
                    gridView1.Columns["BankName"].Visible = false;
                    gridView1.Columns["BranchName"].OptionsColumn.AllowEdit = false;
                    gridView1.Columns["BranchName"].Visible = false;
                    gridView1.Columns["EReceipts"].OptionsColumn.AllowEdit = false;
                    gridView1.Columns["GeneratedBy"].OptionsColumn.AllowEdit = false;
                    gridView1.Columns["GeneratedBy"].Visible = false;
                    gridView1.Columns["ControlNumber"].OptionsColumn.AllowEdit = true;
                    gridView1.Columns["BatchNumber"].OptionsColumn.AllowEdit = false;
                    gridView1.Columns["PrintedBY"].OptionsColumn.AllowEdit = false;
                    gridView1.Columns["PrintedBY"].Visible = false;
                    //gridView1.Columns["EReceiptsDate"].OptionsColumn.AllowEdit = false;
                    //gridView1.Columns["EReceiptsDate"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
                    //gridView1.Columns["EReceiptsDate"].DisplayFormat.FormatString = "dd MMM yyyy";
                    //gridView1.Columns["EReceiptsDate"].Visible = false;
                    //gridView1.Columns["DatePrinted"].OptionsColumn.AllowEdit = false;
                    //gridView1.Columns["DatePrinted"].Visible = false;
                    gridView1.Columns["AgencyCode"].Visible = false;
                    gridView1.Columns["BatchNumber"].Visible = false;
                    gridView1.Columns["RevenueCode"].OptionsColumn.AllowEdit = false;
                    gridView1.Columns["RevenueCode"].Visible = false;
                    gridView1.Columns["StationCode"].Visible = false;
                    //gridView1.Columns["ReceiptID"].Visible = false;
                    gridView1.Columns["Mark x or ✓"].Visible = false;



                    AddUserBatchNumber(gridView1);
                    gridControl1.ForceInitialize();

                }
                else
                {
                    gridView1.Columns.Clear();
                }

                label5.Text = String.Format(" Total Number of Printed receipts :  {0} waiting for control number to be applied ", dt.Rows.Count);

                //}


            }
            catch (Exception ex)
            {
                Common.setMessageBox(ex.Message, Program.ApplicationName, 3);
                return;
            }
        }

        static bool CheckRnageValue(string ContNum)
        {
            bool bRes;

            string sql = String.Format("SELECT COUNT(*) AS Count FROM dbo.tblCollectionReport WHERE (ControlNumber = '{0}')", ContNum);

            if (new Logic().IsRecordExist(sql))
                //if (retval == "1")
                bRes = true;
            else
                bRes = false;
            return bRes;
        }

        private void FrmMainFest_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'taxSmartSuiteRevisedDataSet.BankReceipts' table. You can move, or remove it, as needed.
            //this.bankReceiptsTableAdapter.Fill(this.taxSmartSuiteRevisedDataSet.BankReceipts);
            //Init();
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
            else if (sender == tsbNew)
            {
                //label11.Visible = false;

                //txtPaymentRef.Visible = false;

                //groupControl2.Text = "Add New Record";

                //iTransType = TransactionTypeCode.New;

                //ShowForm();

                //clear();

                //groupControl2.Enabled = true;

                boolIsUpdate = false;
            }
            else if (sender == tsbEdit)
            {
                //groupControl2.Text = "Edit Record Mode";

                iTransType = TransactionTypeCode.Edit;

                //ShowForm();

                boolIsUpdate = true;

            }
            //else if (sender == tsbDelete)
            //{
            //    groupControl2.Text = "Delete Record Mode";
            //    iTransType = TransactionTypeCode.Delete;
            //    if (MosesClassLibrary.Utilities.Common.AskQuestion("Deleting this record will delete attached record.\nDo you want to continue?", ""))
            //    {
            //    }
            //    else
            //        tsbReload.PerformClick();
            //    boolIsUpdate = false;
            //}
            //else if (sender == tsbReload)
            //{
            //    iTransType = TransactionTypeCode.Reload;
            //    ShowForm();
            //}
            //bttnReset.PerformClick();
        }

        //void ProcessDataTable(DataTable Dt)
        //{
        //    if (Dt != null && Dt.Rows.Count > 0)
        //    {
        //        foreach (DataRow item in Dt.Rows)
        //        {
        //            if (item == null) continue;

        //        }
        //    }
        //}

        void AddUserBatchNumber(GridView view)
        {
            if (view != null && view.RowCount > 0)
            {
                for (int i = 0; i < view.RowCount; i++)
                {
                    //view.SetRowCellValue(i, view.Columns["Printedby"], Program.UserID);
                    view.SetRowCellValue(i, view.Columns["BatchNumber"], BatchNumber);
                    //view.SetRowCellValue(i, view.Columns["DatePrinted"], string.Format("{0:yyyy/MM/dd HH:mm:ss tt}", DateTime.Now));
                    //view.SetRowCellValue(i, view.Columns["UploadStatus"], (string)"Pending");
                    //gridView1.Columns["DatePrinted"].OptionsColumn.AllowEdit = false;
                    //gridView1.Columns["UploadStatus"].OptionsColumn.AllowEdit = false;
                }
            }
        }

        void gridView1_ValidatingEditor(object sender, DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventArgs e)
        {
            GridView view = sender as GridView;

            int rowHandle = view.FocusedRowHandle;

            if (view != null)
            {
                if (view.FocusedColumn.FieldName == "ControlNumber")
                {
                    object obj = e.Value;//get the value of the cell

                    if (string.IsNullOrEmpty(obj.ToString()))
                    {
                        //e.Value = string.Empty;
                        //view.SetRowCellValue(rowHandle, view.Columns["ControlNumber"], string.Empty);
                        return;
                    }
                    else
                    {
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
                            else if (!Logic.CheckRangeValue4mTable((string)obj))
                            {
                                e.Valid = false;//set the error valid to false

                                //display error message
                                e.ErrorText = " Control Number Not in The Issue Range ";
                            }
                            else
                            {
                                e.Valid = true;
                                //e.Value = (string)obj;
                            }

                        }
                    }
                    //e.Value = string.Empty;
                }
            }
        }

        void IncrementColumn(int startIndex, string startValue, string fieldName)
        {
            if (startIndex > -1)
            {
                GridView view = gridView1; count = 0;

                long start = long.Parse(startValue);

                StrCltNumber = startValue;

                //view.ClearColumnErrors();

                for (int i = startIndex; i < view.RowCount; i++)
                {
                    //chechk if the number exit in the issue control range
                    view.SetRowCellValue(i, fieldName, string.Empty);//clear cell

                    string newcellvalue = start++.ToString().PadLeft(startValue.Length, '0');

                    if (!Logic.CheckRangeValue4mTable((string)newcellvalue))
                    {
                        //breake if the control number not in the list of issue range
                        view.SetRowCellValue(i, fieldName, newcellvalue);
                        view.SetColumnError(view.Columns["ControlNumber"], " Control Number Not in The Issue Range  ", ErrorType.Critical);
                        break;

                    }
                    else
                    {
                        view.SetRowCellValue(i, fieldName, newcellvalue);
                        //check if the value equal end value
                        //if (Logic.CheckRnageValue((string)newcellvalue))
                        //{
                        //    view.SetRowCellValue(i, fieldName, newcellvalue);
                        //    view.SetColumnError(view.Columns["ControlNumber"], " Control Number Already Exit ", ErrorType.Critical);
                        //    break;
                        //}
                        //else 
                        //if (Convert.ToDouble(newcellvalue) <= Convert.ToDouble(endvalue))
                        //{
                        //    view.SetRowCellValue(i, fieldName, newcellvalue);

                        //    count = count + 1;

                        //    view.FocusedColumn = view.Columns[fieldName];
                        //    var lol = new BaseContainerValidateEditorEventArgs(newcellvalue);
                        //    gridView1_ValidatingEditor(gridView1, lol);
                        //    if (!lol.Valid)
                        //    {
                        //        //view.SetRowCellValue(i, fieldName, newcellvalue);
                        //        //view.SetColumnError(view.Columns["ControlNumber"], " Control Number Already Exit ", ErrorType.Critical);
                        //        break;
                        //    }
                        //}
                        //else
                        //    break;
                    }
                }

            }
        }

        void UpdateIssueTemp(string controlNum, int recordnumber)
        {
            using (System.Data.DataSet ds = new System.Data.DataSet())
            {

                using (SqlDataAdapter ada = new SqlDataAdapter(String.Format("SELECT IssueFrom,IssueTo,IssueQty,UsedQty FROM Receipt.tblIssueReceipt  WHERE (StationCode = '{0}')", Program.stationCode), Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }

                dt = ds.Tables[0];

                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow Dr in dt.Rows)
                    {
                        if (Enumerable.Range(Convert.ToInt32(Dr["IssueFrom"].ToString()), Convert.ToInt32(Dr["IssueQty"].ToString())).Contains(Convert.ToInt32(controlNum)))
                        {
                            string issfrom = Dr["IssueFrom"].ToString();
                            string issTo = Dr["IssueTo"].ToString();
                            int issueQty = Convert.ToInt32(Dr["IssueQty"].ToString());
                            int start = Convert.ToInt32(Dr["UsedQty"].ToString());
                            int used = start + recordnumber;

                            //bRes = true;

                            if (used > issueQty)
                            {
                                used = issueQty;
                            }
                            else
                            {
                                used = used;
                            }
                            //update the table
                            using (SqlConnection db = new SqlConnection(Logic.ConnectionString))
                            {
                                SqlTransaction transaction;

                                db.Open();

                                transaction = db.BeginTransaction();
                                try
                                {
                                    //MessageBox.Show(MDIMain.stateCode);
                                    //fieldid
                                    using (SqlCommand sqlCommand1 = new SqlCommand(String.Format(String.Format("UPDATE Receipt.tblIssueReceipt SET UsedQty='{{0}}' where  StationCode ='{0}' and IssueFrom='{1}' and IssueTo='{2}' ", Program.stationCode, issfrom, issTo), used), db, transaction))
                                    {
                                        sqlCommand1.ExecuteNonQuery();
                                    }

                                    transaction.Commit();
                                }
                                catch (SqlException sqlError)
                                {
                                    transaction.Rollback();
                                    Tripous.Sys.ErrorBox(sqlError);
                                    return;
                                }
                                db.Close();
                            }
                            break;
                        }
                    }
                }

            }
        }

        void ProcessDataTable(DataTable Dt)
        {
            if (Dt != null && Dt.Rows.Count > 0)
            {
                foreach (DataRow item in Dt.Rows)
                {
                    if (item == null) continue;
                    //decimal amount = decimal.Parse(item["Amount"].ToString());
                    try
                    {
                        item["Mark x or ✓"] = "";

                    }
                    catch
                    {

                    }
                }
                //Dt.AcceptChange();
            }
            Dt.AcceptChanges();
        }

        void Loadgrid()
        {
            if (dt != null && dt.Rows.Count > 0)
            {
                //dt.Columns.Add("Mark x or ✓", typeof(string));
                //dt.AcceptChanges();

                gridControl1.DataSource = dt;
                //gridControl1.DataMember = "table";
                gridView1.BestFitColumns();
                //gridView1.BestFitColumns();
                gridView1.Columns["PaymentRefNumber"].OptionsColumn.AllowEdit = false;
                gridView1.Columns["DepositSlipNumber"].OptionsColumn.AllowEdit = false;
                gridView1.Columns["DepositSlipNumber"].Visible = false;
                gridView1.Columns["PaymentDate"].OptionsColumn.AllowEdit = false;
                gridView1.Columns["PaymentDate"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
                gridView1.Columns["PaymentDate"].DisplayFormat.FormatString = "dd/MM/yyyy";

                gridView1.Columns["PayerID"].OptionsColumn.AllowEdit = false;
                gridView1.Columns["PayerName"].OptionsColumn.AllowEdit = false;
                gridView1.Columns["Description"].OptionsColumn.AllowEdit = false;
                gridView1.Columns["Description"].Visible = false;
                gridView1.Columns["Amount"].OptionsColumn.AllowEdit = false;
                gridView1.Columns["Amount"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                gridView1.Columns["Amount"].DisplayFormat.FormatString = "n2";
                gridView1.Columns["BankName"].OptionsColumn.AllowEdit = false;
                gridView1.Columns["BankName"].Visible = false;
                gridView1.Columns["BranchName"].OptionsColumn.AllowEdit = false;
                gridView1.Columns["BranchName"].Visible = false;
                gridView1.Columns["EReceipts"].OptionsColumn.AllowEdit = false;
                gridView1.Columns["GeneratedBy"].OptionsColumn.AllowEdit = false;
                gridView1.Columns["GeneratedBy"].Visible = false;
                gridView1.Columns["ControlNumber"].OptionsColumn.AllowEdit = true;
                gridView1.Columns["BatchNumber"].OptionsColumn.AllowEdit = false;
                gridView1.Columns["Printedby"].OptionsColumn.AllowEdit = false;
                gridView1.Columns["Printedby"].Visible = false;
                gridView1.Columns["EReceiptsDate"].OptionsColumn.AllowEdit = false;
                gridView1.Columns["EReceiptsDate"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
                gridView1.Columns["EReceiptsDate"].DisplayFormat.FormatString = "dd MMM yyyy";
                gridView1.Columns["EReceiptsDate"].Visible = false;
                gridView1.Columns["DatePrinted"].OptionsColumn.AllowEdit = false;
                gridView1.Columns["DatePrinted"].Visible = false;
                gridView1.Columns["AgencyCode"].Visible = false;
                gridView1.Columns["BatchNumber"].Visible = false;
                gridView1.Columns["UploadStatus"].OptionsColumn.AllowEdit = false;
                gridView1.Columns["UploadStatus"].Visible = false;
                gridView1.Columns["Mark x or ✓"].Visible = false;



                //AddUserBatchNumber(gridView1);
                //gridControl1.ForceInitialize();

            }
        }

        void Gettable(System.Data.DataSet dts)
        {
            GridView view = gridView1; count = 0;

            if (dts.Tables[1].Rows != null && dts.Tables[1].Rows.Count > 0)
            {
                for (int i = 0; i < dts.Tables[1].Rows.Count; i++)
                {
                    view.SetRowCellValue(i, "ControlNumber", dts.Tables[1].Rows[i]["controlnumber"]);
                }
            }
        }

        //void IncrementColumn(int startIndex, string startValue, string fieldName)
        //{
        //    if (startIndex > -1)
        //    {
        //        GridView view = gridView1;

        //        long start = long.Parse(startValue);

        //        for (int i = startIndex; i < view.RowCount; i++)
        //        {
        //            //chechk if the number exit in the issue control range

        //            string newcellvalue = start++.ToString().PadLeft(startValue.Length, '0');

        //            if (!Logic.CheckRangeValue4mTable(Convert.ToInt32(newcellvalue)))
        //            {
        //                //breake if the control number not in the list of issue range
        //                break;

        //            }
        //            else
        //            {
        //                view.SetRowCellValue(i, fieldName, newcellvalue);
        //            }
        //        }
        //    }
        //}

    }
}
