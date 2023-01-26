using BankReconciliation.Class;
using DevExpress.XtraGrid.Selection;
using DevExpress.XtraSplashScreen;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using TaxSmartSuite.Class;

namespace BankReconciliation.Forms
{
    public partial class FrmRequest : Form
    {
        public static FrmRequest publicStreetGroup; private SqlCommand _command; private SqlDataAdapter adp;

        DataTable tableTrans = new DataTable();

        GridCheckMarksSelection selection;
        public FrmRequest()
        {
            SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

            InitializeComponent();

            publicStreetGroup = this;

            setImages();

            ToolStripEvent();

            Load += OnFormLoad;

            OnFormLoad(null, null);


            //selection.ClearSelection();
            selection = new GridCheckMarksSelection(gridView1);
            //selection.CheckMarkColumn.VisibleIndex = 0;


            //create offline table
            tableTrans.Columns.Add("BankShortCode", typeof(string));

            tableTrans.Columns.Add("FinancialperiodID", typeof(int));

            tableTrans.Columns.Add("StartDate", typeof(DateTime));

            tableTrans.Columns.Add("EndDate", typeof(DateTime));

            tableTrans.Columns.Add("BankAccountID", typeof(int));

            tableTrans.Columns.Add("PostingRequestID", typeof(int));

            tableTrans.Columns.Add("ReconID", typeof(int));



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

            FrmRequest_Load(null, null);

            btnAllocate.Click += btnAllocate_Click;

        }

        void btnAllocate_Click(object sender, EventArgs e)
        {
            if (gridView1.SelectedRowsCount == 0)
            {
                Common.setMessageBox("Select Record to Close", "Close Period", 1); return;
            }
            else
            {
                //Common.setMessageBox("Sorry TransactionPosting Suspend for now!", "Close Period", 1);
                //return;
                try
                {
                    SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

                    tableTrans.Clear();

                    if (string.IsNullOrEmpty(selection.SelectedCount.ToString())) return;

                    for (int i = 0; i < selection.SelectedCount; i++)
                    {
                        bool chkUpdate = (bool)(selection.GetSelectedRow(i) as DataRowView)["IsApproved"];
                        //BankAccountID,PostingRequestID
                        if (chkUpdate)
                        {
                            tableTrans.Rows.Add(new object[] { (selection.GetSelectedRow(i) as DataRowView)["BankShortCode"], (selection.GetSelectedRow(i) as DataRowView)["FinancialperiodID"], (selection.GetSelectedRow(i) as DataRowView)["StartDate"], (selection.GetSelectedRow(i) as DataRowView)["EndDate"], (selection.GetSelectedRow(i) as DataRowView)["BankAccountID"], (selection.GetSelectedRow(i) as DataRowView)["PostingRequestID"], (selection.GetSelectedRow(i) as DataRowView)["ReconID"] });
                        }
                    }


                    if (tableTrans.Rows.Count <= 0)
                    {
                        Common.setMessageBox("No approved record is selected", "Close Period", 3);
                        return;
                    }

                    selection.ClearSelection();

                    using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
                    {
                        connect.Open();
                        _command = new SqlCommand("PostingTransactionApprove", connect)
                        {
                            CommandType = CommandType.StoredProcedure
                        };

                        _command.Parameters.Add(new SqlParameter("@pTransactiondb", SqlDbType.Structured)).Value = tableTrans;
                        _command.Parameters.Add(new SqlParameter("@Userid", SqlDbType.VarChar)).Value = Program.UserID;
                        _command.CommandTimeout = 0;
                        //_command.Parameters.Add(new SqlParameter("@enddate", SqlDbType.VarChar)).Value = string.Format("{0:yyyy/MM/dd}", dtpEnd.Value);
                        //_command.Parameters.Add(new SqlParameter("@pType", SqlDbType.Bit)).Value = boolIsUpdate2;
                        //_command.Parameters.Add(new SqlParameter("@Batchcode", SqlDbType.Char)).Value = label22.Text;
                        //@Years
                        using (System.Data.DataSet ds = new System.Data.DataSet())
                        {
                            ds.Clear();
                            adp = new SqlDataAdapter(_command);
                            adp.Fill(ds);
                            connect.Close();

                            //&& ds.Tables[0].Rows[0]["returnCode"].ToString() != "01"
                            if (ds.Tables[0].Rows[0]["returnCode"].ToString() != "00" && ds.Tables[0].Rows[0]["returnCode"].ToString() != "01")
                            {
                                if (ds.Tables[0].Rows[0]["returnCode"].ToString() == "-1")
                                {
                                    Common.setMessageBox(ds.Tables[0].Rows[0]["returnMessage"].ToString(), Program.ApplicationName, 2);

                                    return;
                                }
                                if (ds.Tables[0].Rows[0]["returnCode"].ToString() == "-2")
                                {
                                    Common.setMessageBox(ds.Tables[0].Rows[0]["returnMessage"].ToString(), Program.ApplicationName, 2);

                                    using (FrmReportPosting frmreport = new FrmReportPosting(ds))
                                    {
                                        frmreport.ShowDialog();
                                    }
                                    FrmRequest_Load(null, null);

                                    //return;
                                }
                            }
                            else
                            {
                                Common.setMessageBox(ds.Tables[0].Rows[0]["returnMessage"].ToString(), Program.ApplicationName, 2);

                                if (ds.Tables.Count > 1)
                                {                                 //FrmReportPosting report = new FrmReportPosting(ds.Tables[1], ds.Tables[2]);
                                    using (FrmReportPosting frmreport = new FrmReportPosting(ds))
                                    {
                                        frmreport.ShowDialog();
                                    }
                                }
                                FrmRequest_Load(null, null);



                            }

                        }
                    }
                }
                catch (Exception ex)
                {
                    Tripous.Sys.ErrorBox(ex.Message); return;
                }
                finally
                {
                    SplashScreenManager.CloseForm(false);
                }
            }

        }

        public void RefreshForm()
        {
            NavBars.ToolStripEnableDisableControls(toolStrip, Tag as String);
        }

        private void FrmRequest_Load(object sender, EventArgs e)
        {
            try
            {
                //myDataTable.Constraints.Clear
                viewPostbankStatmentTableAdapter1.Connection.ConnectionString = Logic.ConnectionString;
                viewPostingRequestBankTableAdapter.Connection.ConnectionString = Logic.ConnectionString;

                // TODO: This line of code loads data into the 'tblTransactionPostingRequestViewPostBankStatment.ViewPostingRequestBank' table. You can move, or remove it, as needed.
                this.viewPostingRequestBankTableAdapter.Fill(this.tblTransactionPostingRequestViewPostBankStatment.ViewPostingRequestBank);
                this.viewPostbankStatmentTableAdapter1.Fill(this.tblTransactionPostingRequestViewPostBankStatment.ViewPostbankStatment);

                gridView1.OptionsView.ColumnAutoWidth = false;
                gridView1.BestFitColumns();
                gridView2.OptionsView.ColumnAutoWidth = false;
                gridView2.BestFitColumns();
                gridView3.OptionsView.ColumnAutoWidth = false;
                gridView3.BestFitColumns();
            }
            catch (Exception ex)
            {

                Tripous.Sys.ErrorBox(ex.Message); return;
            }

        }

        private void postingRecordsListsBindingSource_CurrentChanged(object sender, EventArgs e)
        {

        }

        private void fillByToolStripButton_Click(object sender, EventArgs e)
        {
            try
            {
                this.viewPostingRequestBankTableAdapter.FillBy(this.tblTransactionPostingRequestViewPostBankStatment.ViewPostingRequestBank);
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }

        }

        //        bool CheckGridView(GridView view)
        //        {
        //#if false
        //            if (view == null || view.RowCount <= 0) return false;
        //            int errCount = 0;
        //            var col = view.Columns["BankShortCode"];
        //            for (int i = 0; i < view.RowCount; i++)
        //            {
        //                var value = view.GetRowCellValue(i, col);
        //                if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
        //                {
        //                    view.SetColumnError(col, "This is required", DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical);
        //                    ++errCount;
        //                }
        //            }
        //            return errCount == 0;
        //#else
        //            if (view == null || view.RowCount <= 0) return true;
        //            int errCount = 0;
        //            var dt = (view.GridControl).DataSource as DataTable;
        //            if (dt == null || dt.Rows.Count <= 0)
        //            {
        //                Common.setMessageBox("Error retrieving DataTable", Program.ApplicationName, 2);
        //                return false;
        //            }
        //            var col = dt.Columns["BankShortCode"];
        //            foreach (DataRow row in dt.Rows)
        //            {
        //                row.ClearErrors();
        //                var value = row["BankShortCode"];
        //                if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
        //                {
        //                    row.SetColumnError(col, "This is required");
        //                    ++errCount;
        //                }
        //            }
        //            return errCount == 0;
        //#endif
        //        }

    }
}
