using BankReconciliation.Class;
using DevExpress.Utils;
using DevExpress.XtraGrid.Selection;
using DevExpress.XtraSplashScreen;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using TaxSmartSuite.Class;

namespace BankReconciliation.Forms
{
    public partial class FrmCloseFinanical : Form
    {
        public static FrmCloseFinanical publicStreetGroup;

        GridCheckMarksSelection selection;

        private SqlCommand _command;

        private SqlDataAdapter adp;

        private bool Isbank = false;
        public FrmCloseFinanical()
        {
            SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);


            InitializeComponent();

            publicStreetGroup = this;

            setImages();

            ToolStripEvent();

            //selection = new GridCheckMarksSelection(gridView1);

            Load += OnFormLoad;

            OnFormLoad(null, null);

            cboYear.SelectedIndexChanged += cboYear_SelectedIndexChanged;

            btnSelect.Click += btnSelect_Click;

            btnClose.Click += btnClose_Click;

            SplashScreenManager.CloseForm(false);
        }

        void btnClose_Click(object sender, EventArgs e)
        {

            using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
            {
                connect.Open();
                _command = new SqlCommand("doCloseFinancialperiod", connect) { CommandType = CommandType.StoredProcedure };
                _command.Parameters.Add(new SqlParameter("@period", SqlDbType.VarChar)).Value = cboMonth.SelectedValue;
                _command.Parameters.Add(new SqlParameter("@years", SqlDbType.VarChar)).Value = cboYear.SelectedValue;

                using (System.Data.DataSet ds = new System.Data.DataSet())
                {
                    ds.Clear();
                    adp = new SqlDataAdapter(_command);
                    adp.Fill(ds);
                    connect.Close();

                    if (ds.Tables[0].Rows[0]["returnCode"].ToString() != "00")
                    {
                        Common.setMessageBox(ds.Tables[0].Rows[0]["returnMessage"].ToString(), Program.ApplicationName, 2);

                        return;

                    }
                    else
                    {
                        Common.setMessageBox(ds.Tables[0].Rows[0]["returnMessage"].ToString(), Program.ApplicationName, 2);

                        return;
                    }

                }
            }
        }

        void btnSelect_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(cboYear.SelectedValue.ToString()))
            {
                Common.setEmptyField("Financial Year", Program.ApplicationName);
                return;
            }
            else if (string.IsNullOrEmpty(cboMonth.SelectedValue.ToString()))
            {
                Common.setEmptyField("Financial Month", Program.ApplicationName);
                return;
            }
            else
            {
                using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
                {
                    connect.Open();
                    _command = new SqlCommand("dogetCloseFinancialperiod", connect) { CommandType = CommandType.StoredProcedure };
                    _command.Parameters.Add(new SqlParameter("@period", SqlDbType.VarChar)).Value = cboMonth.SelectedValue;
                    _command.Parameters.Add(new SqlParameter("@years", SqlDbType.VarChar)).Value = cboYear.SelectedValue;

                    using (System.Data.DataSet ds = new System.Data.DataSet())
                    {
                        ds.Clear();
                        adp = new SqlDataAdapter(_command);
                        adp.Fill(ds);
                        connect.Close();

                        if (ds.Tables[0].Rows[0]["returnCode"].ToString() != "00")
                        {
                            Common.setMessageBox(ds.Tables[0].Rows[0]["returnMessage"].ToString(), Program.ApplicationName, 2);

                            return;

                        }
                        else
                        {
                            gridControl1.DataSource = ds.Tables[1];
                            gridView1.Columns["ClosingBalance"].DisplayFormat.FormatType = FormatType.Numeric;
                            gridView1.Columns["ClosingBalance"].DisplayFormat.FormatString = "n2";
                            gridView1.Columns["OpeningBalance"].DisplayFormat.FormatType = FormatType.Numeric;
                            gridView1.Columns["OpeningBalance"].DisplayFormat.FormatString = "n2";
                            gridView1.Columns["FinancialperiodID"].Visible = false;
                            gridView1.Columns["Months"].Visible = false;
                            gridView1.Columns["ClosingBalance"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;

                            gridView1.Columns["ClosingBalance"].SummaryItem.FieldName = "ClosingBalance";
                            gridView1.Columns["ClosingBalance"].SummaryItem.DisplayFormat = "Total = {0:n}";

                            //gridView1.BestFitColumns();
                        }

                    }
                }
            }

        }

        void cboYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboYear.SelectedValue != null && !Isbank)
            {
                setDBComboBoxMonth();

            }
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
            setDBComboBox();

        }

        public void RefreshForm()
        {
            NavBars.ToolStripEnableDisableControls(toolStrip, Tag as String);
        }

        public void setDBComboBox()
        {
            using (var ds = new System.Data.DataSet())
            {
                using (SqlDataAdapter ada = new SqlDataAdapter((string)"SELECT DISTINCT Year FROM Reconciliation.tblFinancialperiod", Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }

                Common.setComboList(cboYear, ds.Tables[0], "Year", "Year");

            }
            cboYear.SelectedIndex = -1;
        }

        public void setDBComboBoxMonth()
        {
            using (var ds = new System.Data.DataSet())
            {
                string qry = string.Format("SELECT Months,Periods FROM Reconciliation.tblFinancialperiod WHERE Year='{0}' AND FinancialperiodID NOT IN (SELECT FinancialperiodID FROM Reconciliation.tblCloseFinanicalPeriod)", cboYear.SelectedValue);

                using (SqlDataAdapter ada = new SqlDataAdapter(qry, Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }

                Common.setComboList(cboMonth, ds.Tables[0], "Periods", "Months");

            }
            cboMonth.SelectedIndex = -1;
        }

        void setReloadGrid()
        {
            using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
            {
                connect.Open();

                _command = new SqlCommand("dogetCloseFinancialperiod", connect) { CommandType = CommandType.StoredProcedure };

                using (System.Data.DataSet ds = new System.Data.DataSet())
                {
                    ds.Clear();

                    adp = new SqlDataAdapter(_command);
                    adp.Fill(ds);
                    //Dts = ds.Tables[0];
                    connect.Close();

                    //dtResult = ds;

                    if (ds.Tables[0].Rows[0]["returnCode"].ToString() == "00")
                    {
                        //groupBox1.Text = ds.Tables[0].Rows[0]["returnMessage"].ToString();

                        gridControl1.DataSource = ds.Tables[1];
                        //gridView1.Columns["Months"].SortOrder;
                        gridView1.Columns["Months"].Group();
                        //gridView1.Columns["BatchName"].OptionsColumn.AllowEdit = false;
                        //gridView1.Columns["BankShortCode"].OptionsColumn.AllowEdit = false;
                        //gridView1.Columns["OpeningBalance"].OptionsColumn.AllowEdit = false;
                        //gridView1.Columns["CloseBal"].OptionsColumn.AllowEdit = false;
                        //gridView1.Columns["Start Date"].OptionsColumn.AllowEdit = false;
                        //gridView1.Columns["End Date"].OptionsColumn.AllowEdit = false;
                        gridView1.Columns["CloseBal"].DisplayFormat.FormatType = FormatType.Numeric;
                        gridView1.Columns["CloseBal"].DisplayFormat.FormatString = "n2";
                        gridView1.Columns["FinancialperiodID"].Visible = false;
                        //gridView1.Columns["CloseBal"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                    }

                    gridView1.ExpandAllGroups();
                    gridView1.OptionsView.ColumnAutoWidth = false;
                    gridView1.BestFitColumns();
                }
            }
        }

    }
}
