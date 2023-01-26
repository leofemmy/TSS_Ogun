using DevExpress.Utils;
using DevExpress.XtraSplashScreen;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using TaxSmartSuite.CommonLibrary.Controls;

namespace BankReconciliation.Forms
{
    public partial class Form1 : Form
    {
        public static Form1 publicStreetGroup;


        private GridRadioGroupColumnHelper _Helper;

        private GridRadioGroupColumnHelper _Helper2;

        private GridRadioGroupColumnHelper _Helper3;

        DataTable dtsh = new DataTable();

        DataTable dts2H = new DataTable();

        DataRow row; DataRow rowb; DataTable dt; DataRow row3;

        int rowHandleb; int rowHandle; int rowHandle3;

        public Form1()
        {
            InitializeComponent();
        }

        public Form1(DataTable dst, DataTable dst2)
        {
            InitializeComponent();

            spbAdd.Click += SpbAdd_Click;

            spbRemove.Click += SpbRemove_Click;

            FormClosing += Form1_FormClosing;

            dt = new DataTable();
            dt.Clear();
            dt.BeginInit();
            dt.Columns.Add("BSId", typeof(Int32));
            dt.Columns.Add("Date", typeof(DateTime));
            dt.Columns.Add("Amount", typeof(decimal));
            dt.Columns.Add("ColDate", typeof(DateTime));
            dt.Columns.Add("PaymentRef", typeof(string));
            dt.EndInit();


            if (dst != null && dst.Rows.Count > 1)
            {
                dts2H = dst2; dtsh = dst;


                var count = dts2H.Rows.Count;

                var count1 = dtsh.Rows.Count;
                for (int j = 0; j < dtsh.Rows.Count; j++)
                {
                    for (int i = 0; i < dts2H.Rows.Count; i++)
                    {

                        if (Convert.ToDateTime(dts2H.Rows[i]["CollDate"]) == Convert.ToDateTime(dtsh.Rows[j]["BalDate"]) && Convert.ToDecimal(dts2H.Rows[i]["Amount"]) == Convert.ToDecimal(dtsh.Rows[j]["Amount"]))
                        {
                            dt.Rows.Add(new object[] { Convert.ToInt32(dtsh.Rows[j]["BSid"]), Convert.ToDateTime(dtsh.Rows[j]["BalDate"]), Convert.ToDecimal(dtsh.Rows[j]["Amount"]), Convert.ToDateTime(dts2H.Rows[i]["CollDate"])
                               , dts2H.Rows[i]["PaymentRef"]
                            });

                            dtsh.Rows[j].Delete();

                            dts2H.Rows[i].Delete();
                        }
                    }
                }

                //dts2H.AcceptChanges();
                //dtsh.AcceptChanges();

                var countaf = dts2H.Rows.Count;

                var count1af = dtsh.Rows.Count;


                gridControl1.DataSource = dtsh;

                gridControl2.DataSource = dts2H;


                gridControl3.DataSource = dt;
                gridView3.Columns["PaymentRef"].Visible = false;

                _Helper = new GridRadioGroupColumnHelper(gridView1);

                _Helper.SelectedRowChanged += new EventHandler(_Helper_SelectedRowChanged);

                _Helper2 = new GridRadioGroupColumnHelper(gridView2);

                _Helper2.SelectedRowChanged += _Helper2_SelectedRowChanged;

                _Helper3 = new GridRadioGroupColumnHelper(gridView3);

                _Helper3.SelectedRowChanged += _Helper3_SelectedRowChanged;


            }


            Init();


            gridfill();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = !CloseForm();
        }

        private void _Helper3_SelectedRowChanged(object sender, EventArgs e)
        {
            rowHandle3 = _Helper3.SelectedDataSourceRowIndex;
        }

        private void SpbRemove_Click(object sender, EventArgs e)
        {
            row3 = dt.Rows[rowHandle3];

            gridControl1.DataSource = null;

            gridControl2.DataSource = null;

            gridControl3.DataSource = null;


            if (row3 != null)
            {
                int? vasl = row3["BSId"] == DBNull.Value ? (int?)null : Convert.ToInt32(row3["BSId"]);

                var revRow = dt.Select($"BSId = {vasl}").Single();

                //dtsh.ImportRow(row3);
                //dts2H.ImportRow(row3);

                dtsh.Rows.Add(row3["BSId"], row3["Amount"], row3["Date"]);

                dts2H.Rows.Add(row3["PaymentRef"], row3["Amount"], row3["ColDate"]);
                dt.Rows.Remove(revRow);

                //string val = row3["PaymentRef"] as string;

                //var Rowrev = dts2H.Select($"PaymentRef = '{val}'").Single();

                //dts2H.Rows.Add( row3["PaymentRef"], row3["Amount"],row3["CollDate"]);

                //dt.Rows.Remove(Rowrev);




                //foreach (DataRow row in dtsh.Rows)
                //{
                //    int? vasl = row3["BSId"] == DBNull.Value ? (int?)null : Convert.ToInt32(row3["BSId"]);

                //    var revRow = dt.Select($"BSId = {vasl}").Single();

                //    dtsh.Rows.Remove(revRow);
                //}

                //foreach (DataRow rows in dts2H.Rows)
                //{
                //    string val = rows["PaymentRef"] as string;

                //    var Rowrev = dts2H.Select($"PaymentRef = '{val}'").Single();

                //    dts2H.Rows.Remove(Rowrev);
                //}

                DeleteSelectedRows(gridView3, rowHandle3);

                dt.AcceptChanges();

                dtsh.AcceptChanges();

                dts2H.AcceptChanges();

                gridControl1.DataSource = dtsh;

                gridControl2.DataSource = dts2H;

                gridControl3.DataSource = dt;

                gridView3.Columns["Amount"].DisplayFormat.FormatType = FormatType.Numeric;
                gridView3.Columns["Amount"].DisplayFormat.FormatString = "n2";
                gridView3.Columns["Amount"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                gridView3.Columns["Amount"].SummaryItem.FieldName = "Amount";
                gridView3.Columns["Amount"].SummaryItem.DisplayFormat = "Total Amount = {0:n2}";
                gridView3.Columns["Amount"].OptionsColumn.AllowEdit = false;
                gridView3.Columns["Date"].OptionsColumn.AllowEdit = false;
                gridView3.Columns["ColDate"].OptionsColumn.AllowEdit = false;
                gridView3.Columns["Date"].Caption = "Date";
                gridView3.Columns["Date"].VisibleIndex = 1;
                gridView3.Columns["Amount"].VisibleIndex = 2;
                gridView3.Columns["BSId"].VisibleIndex = 3;
                gridView2.Columns["PaymentRef"].VisibleIndex = 4;
                gridView3.Columns["BSId"].Visible = false;
                gridView3.OptionsView.ShowFooter = true;
                gridView3.BestFitColumns();

                gridfill();
            }
        }

        private void SpbAdd_Click(object sender, EventArgs e)
        {
            row = dts2H.Rows[rowHandle];

            rowb = dtsh.Rows[rowHandleb];

            //check row are empty
            if (row != null && rowb != null)
            {
                if (Convert.ToDecimal(row["Amount"]) == Convert.ToDecimal(rowb["Amount"]))
                {
                    dt.Rows.Add(new object[] { Convert.ToInt32(rowb["BSid"]), Convert.ToDateTime(rowb["BalDate"]), Convert.ToDecimal(row["Amount"]), Convert.ToDateTime(row["CollDate"]), row["PaymentRef"] });
                }

                DeleteSelectedRows(gridView1, rowHandleb);

                DeleteSelectedRows(gridView2, rowHandle);

                gridControl3.DataSource = dt;

                gridfill();

                gridView3.Columns["Amount"].DisplayFormat.FormatType = FormatType.Numeric;
                gridView3.Columns["Amount"].DisplayFormat.FormatString = "n2";
                gridView3.Columns["Amount"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                gridView3.Columns["Amount"].SummaryItem.FieldName = "Amount";
                gridView3.Columns["Amount"].SummaryItem.DisplayFormat = "Total Amount = {0:n2}";
                gridView3.Columns["Amount"].OptionsColumn.AllowEdit = false;
                gridView3.Columns["Date"].OptionsColumn.AllowEdit = false;
                gridView3.Columns["ColDate"].OptionsColumn.AllowEdit = false;
                gridView3.Columns["Date"].Caption = "Date";
                gridView3.Columns["Date"].VisibleIndex = 1;
                gridView3.Columns["Amount"].VisibleIndex = 2;
                gridView3.Columns["BSId"].VisibleIndex = 3;
                gridView2.Columns["PaymentRef"].VisibleIndex = 4;
                gridView3.Columns["BSId"].Visible = false;
                gridView3.Columns["PaymentRef"].Visible = false;
                gridView3.OptionsView.ShowFooter = true;
                gridView3.BestFitColumns();
            }


        }

        private void _Helper2_SelectedRowChanged(object sender, EventArgs e)
        {
            rowHandle = _Helper2.SelectedDataSourceRowIndex;

        }

        void _Helper_SelectedRowChanged(object sender, EventArgs e)
        {
            rowHandleb = _Helper.SelectedDataSourceRowIndex;

        }

        private void DeleteSelectedRows(DevExpress.XtraGrid.Views.Grid.GridView view, int rowhandle)
        {
            if (view == null) return;

            view.DeleteRow(view.GetRowHandle(rowhandle));

            ////int[] rows = gridView1.GetSelectedRows();

            //int[] g = view.GetSelectedRows();

            //DataRow[] rows = new DataRow[view.SelectedRowsCount];

            //for (int i = 0; i < view.SelectedRowsCount; i++)

            //    rows[i] = view.GetDataRow(view.GetSelectedRows()[i]);



            //view.BeginSort();

            //try
            //{

            //    foreach (DataRow row in rows)

            //        row.Delete();

            //}

            //finally
            //{

            //    view.EndSort();

            //}
            view.RefreshData();

        }

        void gridfill()
        {
            gridView1.Columns["Amount"].DisplayFormat.FormatType = FormatType.Numeric;
            gridView1.Columns["Amount"].DisplayFormat.FormatString = "n2";
            gridView1.Columns["Amount"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            gridView1.Columns["Amount"].SummaryItem.FieldName = "Amount";
            gridView1.Columns["Amount"].SummaryItem.DisplayFormat = "Total= {0:n2}";
            gridView1.Columns["Amount"].OptionsColumn.AllowEdit = false;
            gridView1.Columns["BalDate"].OptionsColumn.AllowEdit = false;
            gridView1.Columns["BalDate"].Caption = "Date";
            gridView1.Columns["BalDate"].VisibleIndex = 1;
            gridView1.Columns["Amount"].VisibleIndex = 2;
            gridView1.Columns["BSid"].VisibleIndex = 3;
            gridView1.Columns["BSid"].Visible = false;
            //BSid
            gridView1.OptionsView.ShowFooter = true;
            gridView1.BestFitColumns();

            gridView2.Columns["Amount"].DisplayFormat.FormatType = FormatType.Numeric;
            gridView2.Columns["Amount"].DisplayFormat.FormatString = "n2";
            gridView2.Columns["Amount"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            gridView2.Columns["Amount"].SummaryItem.FieldName = "Amount";
            gridView2.Columns["Amount"].SummaryItem.DisplayFormat = "Total= {0:n2}";
            gridView2.Columns["Amount"].OptionsColumn.AllowEdit = false;
            gridView2.Columns["CollDate"].OptionsColumn.AllowEdit = false;
            gridView2.Columns["CollDate"].Caption = "Date";
            gridView2.Columns["CollDate"].VisibleIndex = 1;
            gridView2.Columns["Amount"].VisibleIndex = 2;
            gridView2.Columns["PaymentRef"].Visible = false;
            //gridView2.Columns["PaymentRef"].VisibleIndex = 3;
            gridView2.OptionsView.ShowFooter = true;
            gridView2.BestFitColumns();
        }

        void Init()
        {
            SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

            publicStreetGroup = this;

            setImages();

            ToolStripEvent();

            Load += OnFormLoad;

            OnFormLoad(null, null);

            //RepositoryItemCheck.EndInit();

            //RepositoryItemCheckColl.EndInit();

            //gridView1.CellValueChanged += GridView1_CellValueChanged;

            SplashScreenManager.CloseForm(false);
        }

        private void setImages()
        {
            tsbNew.Image = MDIMains.publicMDIParent.i16x16.Images[5];
            tsbEdit.Image = MDIMains.publicMDIParent.i16x16.Images[6];
            tsbDelete.Image = MDIMains.publicMDIParent.i16x16.Images[7];
            tsbReload.Image = MDIMains.publicMDIParent.i16x16.Images[0];
            tsbClose.Image = MDIMains.publicMDIParent.i16x16.Images[11];

            //bttnCancel.Image = MDIMains.publicMDIParent.i32x32.Images[9];
            ////bttnReset.Image = MDIMains.publicMDIParent.i32x32.Images[8];
            ////bttnUpdate.Image = MDIMain.publicMDIParent.i32x32.Images[7];
            //bttnUpdate.Image = MDIMains.publicMDIParent.i32x32.Images[7];
            //btnAllocate.Image = MDIMains.publicMDIParent.i32x32.Images[6];
            //bttncompare.Image = MDIMains.publicMDIParent.i32x32.Images[8];
            //bttnClose.Image = MDIMain.publicMDIParent.i32x32.Images[9];
            //bttnSave.Image = MDIMains.publicMDIParent.i32x32.Images[7];

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
                //MDIMains.publicMDIParent.RemoveControls();

                //FrmTransaction transact = new FrmTransaction();
                FrmTransaction.dtMatchedManual = dt;


                this.Close();

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
                tsbReload.PerformClick();
                //boolIsUpdate = false;
            }
            else if (sender == tsbReload)
            {
                //iTransType = TransactionTypeCode.Reload; setReload();
                //ShowForm();
            }
            //bttnReset.PerformClick();
        }

        void OnFormLoad(object sender, EventArgs e)
        {
            gridfill();

        }

        private static bool CloseForm()
        {
            bool bRes = false;


            if (MosesClassLibrary.Utilities.Common.AskQuestion("Are you sure you want to Close this application?", Program.ApplicationName))

                bRes = true;

            else
                bRes = false;

            return bRes;
        }

        public DataTable ConvertToDataTable<T>(IList<T> data)
        {
            var properties = TypeDescriptor.GetProperties(typeof(T));
            var table = new DataTable();
            foreach (PropertyDescriptor prop in properties)
            {
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            }
            foreach (T item in data)
            {
                var row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                {
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                }
                table.Rows.Add(row);
            }
            return table;
        }


    }
}
