using BankReconciliation.Class;
using DevExpress.Utils;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraSplashScreen;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using TaxSmartSuite.Class;

namespace BankReconciliation.Forms
{
    public partial class FrmFinanicalYear : Form
    {
        private bool Isbank = false;

        private SqlCommand _command;

        private SqlDataAdapter adp;

        bool IsShowDialog;

        string AppNames;
        public FrmFinanicalYear()
        {
            InitializeComponent();

            Init();

        }

        public FrmFinanicalYear(string AppName)
        {
            InitializeComponent();
            AppNames = AppName;
            this.IsShowDialog = IsShowDialog;
            if (!this.IsShowDialog)
                Init();

        }
        void Init()
        {
            SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

            setDBComboBox();

            cboYear.SelectedIndexChanged += cboYear_SelectedIndexChanged;

            btnSelect.Click += btnSelect_Click;

            gridView1.DoubleClick += gridView1_DoubleClick;

            SplashScreenManager.CloseForm(false);
        }
        void gridView1_DoubleClick(object sender, EventArgs e)
        {
            string strDecD = null; string fince = null;

            GridView view = (GridView)gridControl1.FocusedView;
            if (view != null)
            {

                DataRow dr = view.GetDataRow(view.FocusedRowHandle);

                if (dr != null)
                {
                    strDecD = dr["DESCRIPTION"].ToString();
                    fince = dr["Periods"].ToString();
                }

                DataTable dtsed = new DataTable();

                dtsed.Clear();

                using (var ds = new System.Data.DataSet())
                {
                    ds.Clear();
                    using (SqlDataAdapter ada = new SqlDataAdapter(string.Format("SELECT * FROM Reconciliation.tblCloseFinanicalPeriod WHERE FinancialperiodID='{0}'", dr["FinancialperiodID"]), Logic.ConnectionString))
                    {
                        ada.Fill(ds, "table");
                    }

                    dtsed = ds.Tables[0];

                    if (dtsed != null && dtsed.Rows.Count > 0)
                    {
                        Common.setMessageBox(string.Format("Sorry, This Finanical Period {0} has been Closed.", strDecD), Program.ApplicationName, 1);
                        return;


                    }
                    else
                    {
                        //FrmTransaction.publicStreetGroup.label21.Text = strDecD;

                        //FrmTransaction.publicStreetGroup.label22.Text = dr["FinancialperiodID"].ToString();

                        if (AppNames == "Transaction")
                        {
                            FrmTransaction.publicStreetGroup.label21.Text = strDecD;

                            FrmTransaction.publicStreetGroup.label20.Text = "Period";

                            //setGetDate(cboMonth.SelectedValue.ToString(), cboYear.SelectedValue.ToString());
                        }
                        else if (AppNames == "Collection")
                        {
                            FrmCollectionControl.publicStreetGroup.label20.Text = "Period";

                            FrmCollectionControl.publicStreetGroup.label21.Text = strDecD;
                            FrmCollectionControl.publicStreetGroup.label22.Text = dr["FinancialperiodID"].ToString();

                            //setGetDate(cboMonth.SelectedValue.ToString(), cboYear.SelectedValue.ToString());
                        }
                        else if (AppNames == "Summary")
                        {
                            FrmSummaryAG.publicStreetGroup.label20.Text = "Period";

                            FrmSummaryAG.publicStreetGroup.label21.Text = strDecD;

                            //setGetDate(cboMonth.SelectedValue.ToString(), cboYear.SelectedValue.ToString());
                        }
                        else if (AppNames == "Reclassified")
                        {
                            FrmReclassified.publicStreetGroup.label20.Text = "Period";
                            FrmReclassified.publicStreetGroup.label21.Text = strDecD;
                            //setGetDate(cboMonth.SelectedValue.ToString(), cboYear.SelectedValue.ToString());
                        }


                        setGetDate(fince.ToString(), cboYear.SelectedValue.ToString());
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                }


                //DataTable Dt = (new Logic()).getSqlStatement(String.Format("SELECT COUNT(*) FROM Reconciliation.tblCloseFinanicalPeriod WHERE FinancialperiodID='{0}'", dr["FinancialperiodID"])).Tables[0];

                //if (Dt != null && Dt.Rows.Count > 0)
                //{
                //    //countrec = Convert.ToInt32(Dt.Rows[0][0]) + 1;
                //    Common.setMessageBox(string.Format("Sorry, This Finanical Period {0} has been Closed.", strDecD), Program.ApplicationName, 1);
                //    return;
                //}
                //else
                //{
                //    FrmTransaction.publicStreetGroup.label21.Text = strDecD;

                //    FrmTransaction.publicStreetGroup.label22.Text = dr["FinancialperiodID"].ToString();

                //    setGetDate(fince.ToString(), cboYear.SelectedValue.ToString());

                //    this.Close();
                //}
                //countrec = 0;
            }
        }

        void btnSelect_Click(object sender, EventArgs e)
        {
            if (cboYear.SelectedValue == null || string.IsNullOrEmpty(cboYear.SelectedValue.ToString()))
            {
                Common.setEmptyField("Financial Year", Program.ApplicationName);
                return;
            }
            else if (cboMonth.SelectedValue == null || string.IsNullOrEmpty(cboMonth.SelectedValue.ToString()))
            {
                Common.setEmptyField("Financial Month", Program.ApplicationName);
                return;
            }
            else
            {

                string gh = string.Format("{0},{1}", cboMonth.Text, cboYear.SelectedValue);

                if (AppNames == "Transaction")
                {
                    FrmTransaction.publicStreetGroup.label21.Text = gh;

                    FrmTransaction.publicStreetGroup.label20.Text = "Period";

                    setGetDate(cboMonth.SelectedValue.ToString(), cboYear.SelectedValue.ToString());
                }
                else if (AppNames == "Collection")
                {
                    FrmCollectionControl.publicStreetGroup.label20.Text = "Period";

                    FrmCollectionControl.publicStreetGroup.label21.Text = gh;

                    setGetDate(cboMonth.SelectedValue.ToString(), cboYear.SelectedValue.ToString());
                }
                else if (AppNames == "Summary")
                {
                    FrmSummaryAG.publicStreetGroup.label20.Text = "Period";

                    FrmSummaryAG.publicStreetGroup.label21.Text = gh;

                    setGetDate(cboMonth.SelectedValue.ToString(), cboYear.SelectedValue.ToString());
                }
                else if (AppNames == "Reclassified")
                {
                    FrmReclassified.publicStreetGroup.label20.Text = "Period";
                    FrmReclassified.publicStreetGroup.label21.Text = gh;
                    setGetDate(cboMonth.SelectedValue.ToString(), cboYear.SelectedValue.ToString());
                }


                this.Close();
            }
        }

        void cboYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboYear.SelectedValue != null && !Isbank)
            {
                setDBComboBoxMonth();
                setReloadGrid();

            }
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
            if (AppNames == "Collection")
            {
                try
                {
                    using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
                    {
                        connect.Open();

                        _command = new SqlCommand("doFinancialperiodresult", connect) { CommandType = CommandType.StoredProcedure };
                        _command.Parameters.Add(new SqlParameter("@pYear", SqlDbType.VarChar)).Value = cboYear.SelectedValue;
                        _command.Parameters.Add(new SqlParameter("@Type", SqlDbType.VarChar)).Value = AppNames;

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
                                gridControl1.DataSource = ds.Tables[1];
                                gridView1.Columns["AdvanceBal"].DisplayFormat.FormatType = FormatType.Numeric;
                                gridView1.Columns["AdvanceBal"].DisplayFormat.FormatString = "n2";
                                gridView1.Columns["CloseBal"].DisplayFormat.FormatType = FormatType.Numeric;
                                gridView1.Columns["CloseBal"].DisplayFormat.FormatString = "n2";
                                gridView1.Columns["CollectionsBal"].DisplayFormat.FormatType = FormatType.Numeric;
                                gridView1.Columns["CollectionsBal"].DisplayFormat.FormatString = "n2";
                                gridView1.Columns["OpenBal"].DisplayFormat.FormatType = FormatType.Numeric;
                                gridView1.Columns["OpenBal"].DisplayFormat.FormatString = "n2";
                                gridView1.Columns["TransferBal"].DisplayFormat.FormatType = FormatType.Numeric;
                                gridView1.Columns["TransferBal"].DisplayFormat.FormatString = "n2";
                                //gridView1.Columns["FinancialperiodID"].Visible = false;
                                //gridView1.Columns["YEAR"].Visible = false;
                                //gridView1.Columns["Periods"].Visible = false;
                                //gridView1.Columns["CloseBal"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                            }

                            gridView1.ExpandAllGroups();
                            gridView1.OptionsView.ColumnAutoWidth = false;
                            gridView1.BestFitColumns();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Tripous.Sys.ErrorBox(String.Format("{0},{1}", ex.Message, ex.StackTrace));
                    return;
                }
            }
            else if (AppNames == "Transaction")
            {
                try
                {
                    using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
                    {
                        connect.Open();

                        _command = new SqlCommand("doFinancialperiodresult", connect) { CommandType = CommandType.StoredProcedure };
                        _command.Parameters.Add(new SqlParameter("@pYear", SqlDbType.VarChar)).Value = cboYear.SelectedValue;
                        _command.Parameters.Add(new SqlParameter("@Type", SqlDbType.VarChar)).Value = AppNames;

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
                                gridControl1.DataSource = ds.Tables[1];
                                gridView1.Columns["ClosingBalances"].DisplayFormat.FormatType = FormatType.Numeric;
                                gridView1.Columns["ClosingBalances"].DisplayFormat.FormatString = "n2";
                                gridView1.Columns["FinancialperiodID"].Visible = false;
                                gridView1.Columns["YEAR"].Visible = false;
                                gridView1.Columns["Periods"].Visible = false;
                                //gridView1.Columns["CloseBal"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                            }

                            gridView1.ExpandAllGroups();
                            gridView1.OptionsView.ColumnAutoWidth = false;
                            gridView1.BestFitColumns();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Tripous.Sys.ErrorBox(String.Format("{0},{1}", ex.Message, ex.StackTrace));
                    return;
                }
            }
            else if (AppNames == "Summary")
            {
                try
                {
                    using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
                    {
                        connect.Open();

                        _command = new SqlCommand("doFinancialperiodresult", connect) { CommandType = CommandType.StoredProcedure };
                        _command.Parameters.Add(new SqlParameter("@pYear", SqlDbType.VarChar)).Value = cboYear.SelectedValue;
                        _command.Parameters.Add(new SqlParameter("@Type", SqlDbType.VarChar)).Value = AppNames;

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
                                gridControl1.DataSource = ds.Tables[1];
                                gridView1.Columns["Amount"].DisplayFormat.FormatType = FormatType.Numeric;
                                gridView1.Columns["Amount"].DisplayFormat.FormatString = "n2";
                                //gridView1.Columns["FinancialperiodID"].Visible = false;
                                //gridView1.Columns["YEAR"].Visible = false;
                                //gridView1.Columns["Periods"].Visible = false;
                                //gridView1.Columns["CloseBal"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                            }

                            gridView1.ExpandAllGroups();
                            gridView1.OptionsView.ColumnAutoWidth = false;
                            gridView1.BestFitColumns();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Tripous.Sys.ErrorBox(String.Format("{0},{1}", ex.Message, ex.StackTrace));
                    return;
                }
            }

        }

        void setGetDate(string months, string years)
        {

            DataTable dtsed = new DataTable();

            dtsed.Clear();

            using (var ds = new System.Data.DataSet())
            {
                ds.Clear();
                using (SqlDataAdapter ada = new SqlDataAdapter(string.Format("SELECT FinancialperiodID,StartDate,EndDate FROM Reconciliation.tblFinancialperiod WHERE Periods='{0}' AND Year='{1}'", months, years), Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }

                dtsed = ds.Tables[0];

                string[] formats = { "dd/MM/yyyy" };

                if (dtsed != null && dtsed.Rows.Count > 0)
                {
                    if (AppNames == "Transaction")
                    {
                        //lblAll.Text = dataSet3.Tables[1].Rows[0]["ALLRecords"].ToString();
                        FrmTransaction.publicStreetGroup.label22.Text = dtsed.Rows[0]["FinancialperiodID"].ToString();
                        FrmTransaction.publicStreetGroup.dtpStart.Value = Convert.ToDateTime(dtsed.Rows[0]["StartDate"].ToString());
                        FrmTransaction.publicStreetGroup.dtpEnd.Value = Convert.ToDateTime(dtsed.Rows[0]["EndDate"].ToString());

                        FrmTransaction.publicStreetGroup.label25.Text = string.Format("From {0} To {1}", Convert.ToDateTime(dtsed.Rows[0]["StartDate"]).ToString("dd/MM/yyyy"), Convert.ToDateTime(dtsed.Rows[0]["EndDate"]).ToString("dd/MM/yyyy"));
                    }
                    else if (AppNames == "Collection")
                    {
                        FrmCollectionControl.publicStreetGroup.label22.Text = dtsed.Rows[0]["FinancialperiodID"].ToString();
                        FrmCollectionControl.publicStreetGroup.label11.Text = Convert.ToDateTime(dtsed.Rows[0]["StartDate"]).ToString("dd/MM/yyyy");
                        FrmCollectionControl.publicStreetGroup.label12.Text = Convert.ToDateTime(dtsed.Rows[0]["EndDate"]).ToString("dd/MM/yyyy");


                    }
                    else if (AppNames == "Summary")
                    {
                        FrmSummaryAG.publicStreetGroup.label22.Text = dtsed.Rows[0]["FinancialperiodID"].ToString();
                    }
                    else if (AppNames == "Reclassified")
                    {
                        FrmReclassified.publicStreetGroup.label22.Text = dtsed.Rows[0]["FinancialperiodID"].ToString();

                    }


                }
            }
        }

    }
}
