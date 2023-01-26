using BankReconciliation.Class;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraSplashScreen;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Windows.Forms;
using TaxSmartSuite.Class;

namespace BankReconciliation.Forms
{
    public partial class FrmPeriods : Form
    {
        private bool isFirst = false;

        private string monthName;

        public static FrmPeriods publicStreetGroup;

        protected TransactionTypeCode iTransType;

        private String[] split;

        private string strmonth;

        protected bool boolIsUpdate;

        DataTable tableTrans = new DataTable();

        DataTable dt = new DataTable();

        private int numdays; private int perodis;
        private int year;
        private int eyear;
        private int syear;

        public FrmPeriods()
        {
            SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

            InitializeComponent();

            publicStreetGroup = this;

            setImages();

            ToolStripEvent();

            iTransType = TransactionTypeCode.New;

            Load += OnFormLoad;

            OnFormLoad(null, null);

            spinEdit1.KeyPress += spinEdit1_KeyPress;

            spinEdit1.ValueChanged += spinEdit1_ValueChanged;

            bttnUpdate.Click += bttnUpdate_Click;

            btnup.Click += btnup_Click;

            gridView3.ValidatingEditor += gridView3_ValidatingEditor;

            ////create offline table
            tableTrans.Columns.Add("Periods", typeof(string));
            tableTrans.Columns.Add("Months", typeof(string));
            tableTrans.Columns.Add("StartDate", typeof(DateTime));
            tableTrans.Columns.Add("EndDate", typeof(DateTime));
            tableTrans.Columns.Add("DateInterval", typeof(string));

            boolIsUpdate = false;

            SplashScreenManager.CloseForm(false);
        }

        void btnup_Click(object sender, EventArgs e)
        {
            //DialogResult dlg = MessageBox.Show("Save changes made to the Fiscal Year ?", Program.ApplicationName, MessageBoxButtons.YesNo);

            //if (dlg == DialogResult.Yes)
            //{
            UpRecord();
            //}
        }

        void gridView3_ValidatingEditor(object sender, DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventArgs e)
        {
            var dtf = CultureInfo.CurrentCulture.DateTimeFormat;

            GridView view = sender as GridView;

            var get = (object)e.Value;

            int rowHandle = view.FocusedRowHandle;

            var lol = view.GetRowCellValue(view.FocusedRowHandle, view.Columns["EndDate"]);

            var lol2 = view.GetRowCellValue(view.FocusedRowHandle, view.Columns["StartDate"]);

            var per = view.GetRowCellValue(view.FocusedRowHandle, view.Columns["Periods"]);

            DateTime dts = Convert.ToDateTime(lol2);

            DateTime dtget = Convert.ToDateTime(get);

            TimeSpan ts = dtget - dts;

            int dsy = ts.Days;

            if (dsy == 0 || dsy <= 0 || dsy == 30)
            {
                e.Valid = false;
                e.ErrorText = "Sorry, End Date Can't be same as Start Date or less than Start Date";
                return;
            }
            else
            {
                view.SetRowCellValue(view.FocusedRowHandle, view.Columns["EndDate"], String.Format("{0:dd/MM/yyy}", (object)e.Value));

                for (int i = rowHandle; i < view.RowCount; i++)
                {
                    //var lol = gridview1.getrowcellvalue(i, "");
                    int sh = i + 1;

                    if (sh > 13)
                    {
                        sh = i;
                    }


                    int numberOfDays = DateTime.DaysInMonth(Convert.ToInt32(spinEdit1.EditValue), sh);

                    var peri = view.GetRowCellValue(i, view.Columns["Periods"]);

                    var lend = view.GetRowCellValue(i, view.Columns["EndDate"]);

                    var lstart = view.GetRowCellValue(i, view.Columns["StartDate"]);

                    DateTime st, end;

                    if (i == 0 && Convert.ToInt32(per) == 1)
                    {
                        string vals = String.Format("{0:dd/MM/yyy} - {1:dd/MM/yyy}", lstart, lend);

                        view.SetRowCellValue(i, view.Columns["DateInterval"], vals);

                    }
                    else if (i == 1 && Convert.ToInt32(per) == 2)
                    {
                        string vals = String.Format("{0:dd/MM/yyy} - {1:dd/MM/yyy}", lstart, lend);

                        view.SetRowCellValue(i, view.Columns["DateInterval"], vals);
                    }
                    else if (i == 2 && Convert.ToInt32(per) == 3)
                    {
                        string vals = String.Format("{0:dd/MM/yyy} - {1:dd/MM/yyy}", lstart, lend);

                        view.SetRowCellValue(i, view.Columns["DateInterval"], vals);
                    }
                    else if (i == 3 && Convert.ToInt32(per) == 4)
                    {
                        string vals = String.Format("{0:dd/MM/yyy} - {1:dd/MM/yyy}", lstart, lend);

                        view.SetRowCellValue(i, view.Columns["DateInterval"], vals);
                    }
                    else if (i == 4 && Convert.ToInt32(per) == 5)
                    {
                        string vals = String.Format("{0:dd/MM/yyy} - {1:dd/MM/yyy}", lstart, lend);

                        view.SetRowCellValue(i, view.Columns["DateInterval"], vals);
                    }
                    else if (i == 5 && Convert.ToInt32(per) == 6)
                    {
                        string vals = String.Format("{0:dd/MM/yyy} - {1:dd/MM/yyy}", lstart, lend);

                        view.SetRowCellValue(i, view.Columns["DateInterval"], vals);
                    }
                    else if (i == 6 && Convert.ToInt32(per) == 8)
                    {
                        string vals = String.Format("{0:dd/MM/yyy} - {1:dd/MM/yyy}", lstart, lend);

                        view.SetRowCellValue(i, view.Columns["DateInterval"], vals);
                    }
                    else if (i == 7 && Convert.ToInt32(per) == 9)
                    {
                        string vals = String.Format("{0:dd/MM/yyy} - {1:dd/MM/yyy}", lstart, lend);

                        view.SetRowCellValue(i, view.Columns["DateInterval"], vals);
                    }
                    else if (i == 8 && Convert.ToInt32(per) == 10)
                    {
                        string vals = String.Format("{0:dd/MM/yyy} - {1:dd/MM/yyy}", lstart, lend);

                        view.SetRowCellValue(i, view.Columns["DateInterval"], vals);
                    }
                    else if (i == 9 && Convert.ToInt32(per) == 11)
                    {
                        string vals = String.Format("{0:dd/MM/yyy} - {1:dd/MM/yyy}", lstart, lend);

                        view.SetRowCellValue(i, view.Columns["DateInterval"], vals);
                    }
                    else if (i == 10 && Convert.ToInt32(per) == 12)
                    {
                        string vals = String.Format("{0:dd/MM/yyy} - {1:dd/MM/yyy}", lstart, lend);

                        view.SetRowCellValue(i, view.Columns["DateInterval"], vals);
                    }
                    else
                    {
                        end = Convert.ToDateTime(view.GetRowCellValue(i - 1, view.Columns["EndDate"]));

                        st = end.AddDays(1);

                        end = st.AddDays(numberOfDays);

                        view.SetRowCellValue(i, view.Columns["StartDate"], st);

                        view.SetRowCellValue(i, view.Columns["EndDate"], end);

                        string vals = String.Format("{0:dd/MM/yyy} - {1:dd/MM/yyy}", st, end);

                        view.SetRowCellValue(i, view.Columns["DateInterval"], vals);
                    }
                    boolIsUpdate = true;
                    //}
                    //view.FocusedRowHandle.
                }


            }

        }

        void spinEdit1_ValueChanged(object sender, EventArgs e)
        {
            setReload(spinEdit1.EditValue.ToString());
        }

        void bttnUpdate_Click(object sender, EventArgs e)
        {

            const int month = 1;

            perodis = Convert.ToInt32(txtNo.Text);

            year = Convert.ToInt32(spinEdit1.EditValue);

            var dtf = CultureInfo.CurrentCulture.DateTimeFormat;

            syear = Convert.ToInt32(string.Format("{0:yyyy}", dtpStart.Value));

            eyear = Convert.ToInt32(string.Format("{0:yyyy}", dtpEnd.Value));

            if (year != syear && year != eyear)
            {
                Common.setMessageBox("Sorry, Selected date not withing the year range", Program.ApplicationName, 2);
                return;
            }
            else
            {

                TimeSpan ts = dtpEnd.Value - dtpStart.Value;

                numdays = ts.Days; // Will return the difference in Days

                if (numdays != 30)
                {
                    Common.setMessageBox(
                        string.Format(
                            "Sorry, Number of days selected is greater than a Month, {0} is the day selected. ", numdays),
                        Program.ApplicationName, 2);

                    //DialogResult results = MessageBox.Show("Do you wish to Continue", Program.ApplicationName,
                    //    MessageBoxButtons.YesNo);

                    //if (results == DialogResult.Yes)
                    //{
                    //    createfin();
                    //}
                    //else
                    //{
                    //    return;
                    //}
                    return;

                }
                else
                {
                    createfin();

                }
            }

        }

        void createfin()
        {
            gridControl2.DataSource = null;

            gridView3.Columns.Clear(); var dtf = CultureInfo.CurrentCulture.DateTimeFormat;

            if (numdays == 30)
            {
                DateTime firstday = dtpStart.Value;

                DateTime lastDay = dtpEnd.Value;

                tableTrans.Clear();

                for (int i = 1; i <= perodis; i++)
                {
                    //test the value of i
                    if (i > 1)
                    {
                        int numberOfDays = DateTime.DaysInMonth(year, i);
                        firstday = lastDay.AddDays(1);
                        //lastDay = firstday.AddDays(numberOfDays);
                        //lastDay = new DateTime(firstday.Year, i, numberOfDays);
                        ////lastDay= new DateTime(firstday.Year);
                        //TimeSpan dtg = firstday.Subtract(lastDay);
                        //int newd = dtg.Days;
                        //if (numberOfDays==29)
                        //{
                        //    numberOfDays = numberOfDays - 1;
                        //}

                        lastDay = firstday.AddDays(numberOfDays - 1);
                    }

                    string monthName = dtf.GetMonthName(i);

                    tableTrans.Rows.Add(new object[] { Convert.ToString(i).PadLeft(2, '0'), monthName, string.Format("{0:dd/MM/yyy}", firstday), string.Format("{0:dd/MM/yyy}", lastDay), String.Format("{0:dd/MM/yyy} - {1:dd/MM/yyy}", firstday, lastDay) });


                }
            }
            else
            {
                DateTime firstday = dtpStart.Value;

                DateTime lastDay = dtpEnd.Value;

                for (int i = 1; i <= perodis; i++)
                {
                    //test the value of i
                    if (i > 1)
                    {
                        int numberOfDays = DateTime.DaysInMonth(year, i);
                        firstday = lastDay.AddDays(1);
                        //lastDay = firstday.AddDays(numberOfDays);
                        lastDay = new DateTime(year, i, numberOfDays);
                    }


                    string monthName = dtf.GetMonthName(i);

                    tableTrans.Rows.Add(new object[] { Convert.ToString(i).PadLeft(2, '0'), monthName, string.Format("{0:dd/MM/yyy}", firstday), string.Format("{0:dd/MM/yyy}", lastDay), String.Format("{0:dd/MM/yyy} - {1:dd/MM/yyy}", firstday, lastDay) });


                }
            }

            gridControl2.DataSource = tableTrans;
            ////this.Controls.Add(gridControl1);
            //gridControl2.BringToFront();
            //gridView3.Columns["Date Interval"].Visible = false;
            gridView3.Columns["EndDate"].OptionsColumn.AllowEdit = true;
            gridView3.Columns["DateInterval"].OptionsColumn.AllowEdit = false;
            gridView3.Columns["Periods"].OptionsColumn.AllowEdit = false;
            gridView3.Columns["Months"].OptionsColumn.AllowEdit = false;
            gridView3.Columns["StartDate"].OptionsColumn.AllowEdit = false;
            gridView3.Columns["EndDate"].OptionsColumn.AllowEdit = true;
            //gridView3.OptionsBehavior.Editable = false;
            gridView3.BestFitColumns();

        }


        void spinEdit1_KeyPress(object sender, KeyPressEventArgs e)
        {
            setReload(spinEdit1.EditValue.ToString());
            e.Handled = true;
        }

        private void FrmPeriods_Load(object sender, EventArgs e)
        {

        }

        private void setImages()
        {
            tsbNew.Image = MDIMains.publicMDIParent.i16x16.Images[5];
            tsbEdit.Image = MDIMains.publicMDIParent.i16x16.Images[6];
            tsbDelete.Image = MDIMains.publicMDIParent.i16x16.Images[7];
            tsbReload.Image = MDIMains.publicMDIParent.i16x16.Images[0];
            tsbClose.Image = MDIMains.publicMDIParent.i16x16.Images[11];

            //bttnCancel.Image = MDIMains.publicMDIParent.i32x32.Images[9];
            bttnUpdate.Image = MDIMains.publicMDIParent.i32x32.Images[7];
            ////bttnReset.Image = MDIMains.publicMDIParent.i32x32.Images[8];
            //bttnPost.Image = MDIMains.publicMDIParent.i32x32.Images[34];
            btnup.Image = MDIMains.publicMDIParent.i32x32.Images[34];

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
                iTransType = TransactionTypeCode.New;
                //Clear();
                ShowForm();
                //boolIsUpdate = false;
            }
            else if (sender == tsbEdit)
            {
                //groupControl2.Text = "Edit Record Mode";
                iTransType = TransactionTypeCode.Edit;
                //if (EditRecordMode())
                //{
                ShowForm();
                //    boolIsUpdate = true;
                //}
            }
            else if (sender == tsbDelete)
            {
                //groupControl2.Text = "Delete Record Mode";
                iTransType = TransactionTypeCode.Delete;
                if (MosesClassLibrary.Utilities.Common.AskQuestion("Deleting this record will delete attached record.\nDo you want to continue?", ""))
                {
                }
                else
                    tsbReload.PerformClick();

                //boolIsUpdate = false;
            }
            else if (sender == tsbReload)
            {
                iTransType = TransactionTypeCode.Reload;

                ShowForm();
            }
            //bttnReset.PerformClick();
        }

        void OnFormLoad(object sender, EventArgs e)
        {
            ShowForm();
            //setReload(spinEdit1.EditValue.ToString());
        }

        protected void ShowForm()
        {
            switch (iTransType)
            {
                case TransactionTypeCode.Null:
                    splitContainer1.Panel1Collapsed = true;
                    splitContainer1.Panel2Collapsed = false;
                    break;
                case TransactionTypeCode.New:
                    splitContainer1.Panel1Collapsed = false;
                    splitContainer1.Panel2Collapsed = false;
                    break;
                case TransactionTypeCode.Edit:
                    splitContainer1.Panel1Collapsed = false;
                    splitContainer1.Panel2Collapsed = false;
                    break;
                case TransactionTypeCode.Delete:
                    splitContainer1.Panel1Collapsed = true;
                    splitContainer1.Panel2Collapsed = false;
                    break;
                case TransactionTypeCode.Reload:
                    splitContainer1.Panel1Collapsed = true;
                    splitContainer1.Panel2Collapsed = false;
                    break;
                default:
                    splitContainer1.Panel1Collapsed = true;
                    splitContainer1.Panel2Collapsed = false;
                    break;
            }
        }

        void UpRecord()
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

                if (!isRecordExit())
                {
                    if (tableTrans != null && tableTrans.Rows.Count > 0)
                    {
                        using (SqlConnection db = new SqlConnection(Logic.ConnectionString))
                        {
                            SqlTransaction transaction;

                            db.Open();

                            transaction = db.BeginTransaction();

                            try
                            {

                                for (int i = 0; i < tableTrans.Rows.Count; i++)
                                {
                                    using (SqlCommand sqlCommand1 = new SqlCommand(String.Format("INSERT INTO Reconciliation.tblFinancialperiod ( Year,Periods , DateInterval,Months , StartDate , EndDate  ) VALUES ('{0}','{1}','{2}','{3}','{4}','{5}');", spinEdit1.EditValue, (string)tableTrans.Rows[i]["Periods"], (string)tableTrans.Rows[i]["DateInterval"], (string)tableTrans.Rows[i]["Months"], Convert.ToDateTime(tableTrans.Rows[i]["StartDate"]).ToString("yyyy/MM/dd"), Convert.ToDateTime(tableTrans.Rows[i]["EndDate"]).ToString("yyyy/MM/dd")), db, transaction))
                                    {
                                        sqlCommand1.ExecuteNonQuery();
                                    }
                                }



                                transaction.Commit();
                            }
                            catch (Exception ex)
                            {
                                transaction.Rollback();
                                Tripous.Sys.ErrorBox(ex);

                            }

                            db.Close();
                        }
                        Common.setMessageBox("Fiscal Periods Record Saved", Program.ApplicationName, 1);
                        return;
                    }
                    else
                    {
                        Common.setMessageBox("Database is Empty....", Program.ApplicationName, 1);
                        return;
                    }
                }
                else
                {
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        //delete record using the year
                        using (SqlConnection db = new SqlConnection(Logic.ConnectionString))
                        {
                            SqlTransaction transaction;

                            db.Open();

                            transaction = db.BeginTransaction();

                            string qry = string.Format("DELETE FROM Reconciliation.tblFinancialperiod WHERE Year='{0}'", spinEdit1.EditValue);

                            using (SqlCommand sqlCommand1 = new SqlCommand(qry, db, transaction))
                            {
                                sqlCommand1.ExecuteNonQuery();
                            }

                            transaction.Commit();

                        }

                        //insert Refresh record
                        using (SqlConnection db = new SqlConnection(Logic.ConnectionString))
                        {
                            SqlTransaction transaction;

                            db.Open();

                            transaction = db.BeginTransaction();

                            try
                            {
                                for (int i = 0; i < dt.Rows.Count; i++)
                                {
                                    using (SqlCommand sqlCommand1 = new SqlCommand(String.Format("INSERT INTO Reconciliation.tblFinancialperiod ( Year,Periods , DateInterval,Months , StartDate , EndDate  ) VALUES ('{0}','{1}','{2}','{3}','{4}','{5}');", spinEdit1.EditValue, (string)dt.Rows[i]["Periods"], (string)dt.Rows[i]["DateInterval"], (string)dt.Rows[i]["Months"], Convert.ToDateTime(dt.Rows[i]["StartDate"]).ToString("yyyy/MM/dd"), Convert.ToDateTime(dt.Rows[i]["EndDate"]).ToString("yyyy/MM/dd")), db, transaction))
                                    {
                                        sqlCommand1.ExecuteNonQuery();
                                    }

                                    //string query = String.Format(String.Format("UPDATE [tblPeriods] SET [Periods]='{{0}}',[Date Interval]='{{1}}',[Months]='{{2}}' ,[Start Date] ='{{3}}',[End Date]='{{4}}' where  [Year] ='{0}' and [Periods]='{1}'", spinEdit1.EditValue, (string)dt.Rows[i]["Periods"]), (string)dt.Rows[i]["Periods"], (string)dt.Rows[i]["Date Interval"], (string)dt.Rows[i]["Months"], Convert.ToDateTime(dt.Rows[i]["Start Date"]).ToString("yyyy/MM/dd"), Convert.ToDateTime(dt.Rows[i]["End Date"]).ToString("yyyy/MM/dd"));

                                    //using (SqlCommand sqlCommand1 = new SqlCommand(query, db, transaction))
                                    //{
                                    //    sqlCommand1.ExecuteNonQuery();
                                    //}
                                }

                                transaction.Commit();
                            }
                            catch (Exception ex)
                            {
                                transaction.Rollback();
                                Tripous.Sys.ErrorBox(ex);
                            }

                            db.Close();
                        }
                        Common.setMessageBox("Fiscal Periods Record Update", Program.ApplicationName, 1);
                        return;
                    }
                    else
                    {
                        Common.setMessageBox("Database is Empty....", Program.ApplicationName, 1);
                        return;
                    }
                }
            }
            finally
            {
                SplashScreenManager.CloseForm(false);
            }



            #region



            //if ((tableTrans != null && tableTrans.Rows.Count > 0) || (dt != null && dt.Rows.Count > 0))
            //{

            //    //call method to check if the period already exit

            //    if (!isRecordExit())
            //    {
            //using (SqlConnection db = new SqlConnection(Logic.ConnectionString))
            //{
            //    SqlTransaction transaction;

            //    db.Open();

            //    transaction = db.BeginTransaction();

            //    try
            //    {

            //        for (int i = 0; i < tableTrans.Rows.Count; i++)
            //        {
            //            using (SqlCommand sqlCommand1 = new SqlCommand(String.Format("INSERT INTO dbo.tblPeriods( Year, Periods, [Date Interval],Months,[Start Date],[End Date]) VALUES ('{0}','{1}','{2}','{3}','{4}','{5}');", spinEdit1.EditValue, (string)tableTrans.Rows[i]["Periods"], (string)tableTrans.Rows[i]["Date Interval"], (string)tableTrans.Rows[i]["Months"], Convert.ToDateTime(tableTrans.Rows[i]["Start Date"]).ToString("yyyy/MM/dd"), Convert.ToDateTime(tableTrans.Rows[i]["End Date"]).ToString("yyyy/MM/dd")), db, transaction))
            //            {
            //                sqlCommand1.ExecuteNonQuery();
            //            }
            //        }



            //        transaction.Commit();
            //    }
            //    catch (Exception ex)
            //    {
            //        transaction.Rollback();
            //        Tripous.Sys.ErrorBox(ex);

            //    }

            //    db.Close();
            //}
            //    }
            //    else
            //    {//delete receord from periods
            //using (SqlConnection db = new SqlConnection(Logic.ConnectionString))
            //{
            //    SqlTransaction transaction;

            //    db.Open();

            //    transaction = db.BeginTransaction();

            //    string qry = string.Format("DELETE FROM tblPeriods WHERE Year='{0}'", spinEdit1.EditValue);

            //    using (SqlCommand sqlCommand1 = new SqlCommand(qry, db, transaction))
            //    {
            //        sqlCommand1.ExecuteNonQuery();
            //    }

            //    transaction.Commit();

            //}

            //        //insert new record into periods table

            ////#region 

            ////using (SqlConnection db = new SqlConnection(Logic.ConnectionString))
            ////{
            ////    SqlTransaction transaction;

            ////    db.Open();

            ////    transaction = db.BeginTransaction();

            ////    try
            ////    {
            ////        for (int i = 0; i < dt.Rows.Count; i++)
            ////        {
            ////            string query = String.Format(String.Format("UPDATE [tblPeriods] SET [Periods]='{{0}}',[Date Interval]='{{1}}',[Months]='{{2}}' ,[Start Date] ='{{3}}',[End Date]='{{4}}' where  [Year] ='{0}' and [Periods]='{1}'", spinEdit1.EditValue, (string)dt.Rows[i]["Periods"]), (string)dt.Rows[i]["Periods"], (string)dt.Rows[i]["Date Interval"], (string)dt.Rows[i]["Months"], Convert.ToDateTime(dt.Rows[i]["Start Date"]).ToString("yyyy/MM/dd"), Convert.ToDateTime(dt.Rows[i]["End Date"]).ToString("yyyy/MM/dd"));

            ////            using (SqlCommand sqlCommand1 = new SqlCommand(query, db, transaction))
            ////            {
            ////                sqlCommand1.ExecuteNonQuery();
            ////            }
            ////        }

            ////        transaction.Commit();
            ////    }
            ////    catch (Exception ex)
            ////    {
            ////        transaction.Rollback();
            ////        Tripous.Sys.ErrorBox(ex);
            ////    }

            ////    db.Close();
            ////}
            //        #endregion

            //        using (SqlConnection db = new SqlConnection(Logic.ConnectionString))
            //        {
            //            SqlTransaction transaction;

            //            db.Open();

            //            transaction = db.BeginTransaction();

            //            try
            //            {

            //                for (int i = 0; i < dt.Rows.Count; i++)
            //                {
            //                    using (SqlCommand sqlCommand1 = new SqlCommand(String.Format("INSERT INTO dbo.tblPeriods( Year, Periods, [Date Interval],Months,[Start Date],[End Date]) VALUES ('{0}','{1}','{2}','{3}','{4}','{5}');", spinEdit1.EditValue, (string)dt.Rows[i]["Periods"], (string)dt.Rows[i]["Date Interval"], (string)dt.Rows[i]["Months"], Convert.ToDateTime(dt.Rows[i]["Start Date"]).ToString("yyyy/MM/dd"), Convert.ToDateTime(dt.Rows[i]["End Date"]).ToString("yyyy/MM/dd")), db, transaction))
            //                    {
            //                        sqlCommand1.ExecuteNonQuery();
            //                    }
            //                }



            //                transaction.Commit();
            //            }
            //            catch (Exception ex)
            //            {
            //                transaction.Rollback();
            //                Tripous.Sys.ErrorBox(ex);

            //            }

            //            db.Close();
            //        }
            //    }

            //}
            #endregion

        }

        private void setReload(string strYear)
        {

            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

                gridControl2.DataSource = null; gridView3.Columns.Clear();

                //string query = string.Format("SELECT Periods,Months,[Start Date],[End Date],[Date Interval] FROM tblPeriods WHERE Year='{0}'", strYear);

                using (var ds = new System.Data.DataSet())
                {
                    using (SqlDataAdapter ada = new SqlDataAdapter(string.Format("SELECT Periods,Months,StartDate,EndDate,DateInterval FROM Reconciliation.tblFinancialperiod WHERE Year='{0}'", strYear), Logic.ConnectionString))
                    {
                        ada.Fill(ds, "table");
                    }

                    dt = ds.Tables[0];

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        gridControl2.DataSource = dt.DefaultView;

                        //gridView3.OptionsBehavior.Editable = false;
                        gridView3.Columns["EndDate"].OptionsColumn.AllowEdit = true;
                        //gridView3.Columns["Date Interval"].Visible = false;
                        gridView3.Columns["Periods"].OptionsColumn.AllowEdit = false;
                        gridView3.Columns["Months"].OptionsColumn.AllowEdit = false;
                        gridView3.Columns["StartDate"].OptionsColumn.AllowEdit = false;
                        gridView3.Columns["DateInterval"].OptionsColumn.AllowEdit = false;
                        gridView3.BestFitColumns();

                        dtpStart.Enabled = false; dtpEnd.Enabled = false;
                        int numberOfDays = DateTime.DaysInMonth(Convert.ToInt32(spinEdit1.EditValue), 1);
                        DateTime fsday = new DateTime(Convert.ToInt32(spinEdit1.EditValue), 1, 1);
                        DateTime laDay = new DateTime(Convert.ToInt32(spinEdit1.EditValue), 1, numberOfDays);

                        dtpStart.Value = fsday;
                        dtpEnd.Value = laDay;
                        dtpStart.CustomFormat = "dd/MM/yyyy";
                        dtpEnd.CustomFormat = "dd/MM/yyyy";

                    }
                    else
                    {
                        gridControl2.DataSource = null;
                        dtpStart.Enabled = true; dtpEnd.Enabled = true;
                        int numberOfDays = DateTime.DaysInMonth(Convert.ToInt32(spinEdit1.EditValue), 1);
                        DateTime fsday = new DateTime(Convert.ToInt32(spinEdit1.EditValue), 1, 1);
                        DateTime laDay = new DateTime(Convert.ToInt32(spinEdit1.EditValue), 1, numberOfDays);

                        dtpStart.Value = fsday;
                        dtpEnd.Value = laDay;
                        dtpStart.CustomFormat = "dd/MM/yyyy";
                        dtpEnd.CustomFormat = "dd/MM/yyyy";
                    }


                }


            }
            finally
            {
                SplashScreenManager.CloseForm(false);
            }

        }

        bool isRecordExit()
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

                bool bRes;

                string SQL = String.Format("SELECT COUNT(*) AS Count FROM Reconciliation.tblFinancialperiod WHERE (Year = '{0}')", spinEdit1.EditValue);

                if (new Logic().IsRecordExist(SQL))
                    bRes = true;
                else
                    bRes = false;

                return bRes;
            }
            finally
            {
                SplashScreenManager.CloseForm(false);
            }
        }

        public void RefreshForm()
        {
            NavBars.ToolStripEnableDisableControls(toolStrip, Tag as String);
        }

        static bool isPeriodExit(string strPeriod, string strYear)
        {
            bool bRes;

            string SQL = String.Format("SELECT COUNT(*) AS Count FROM tblClosePeriods WHERE (Years = '{0}' and Period='{1}')", strYear, strPeriod);

            if (new Logic().IsRecordExist(SQL))
                bRes = true;
            else
                bRes = false;

            return bRes;
        }

    }
}
