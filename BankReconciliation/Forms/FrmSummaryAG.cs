using BankReconciliation.Class;
using DevExpress.Utils;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraSplashScreen;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TaxSmartSuite.Class;


namespace BankReconciliation.Forms
{
    public partial class FrmSummaryAG : Form
    {
        public static FrmSummaryAG publicStreetGroup;

        protected FrmSummaryAG iTransType;

        protected bool boolIsUpdate; private bool isRecord = false;

        protected int ID; private SqlCommand _command; private SqlDataAdapter adp;

        System.Data.DataSet DtSet;

        bool isFirst = true;

        DataTable dt = new DataTable();
        public FrmSummaryAG()
        {
            SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

            InitializeComponent();

            publicStreetGroup = this;

            setImages();

            ToolStripEvent();

            //iTransType = TransactionTypeCode.Null;

            Load += OnFormLoad;

            openForm();
            btnPost.Click += btnPost_Click;
            btnUp.Click += btnUp_Click;

            gridView1.DoubleClick += gridView1_DoubleClick;

            cboAgency.SelectedIndexChanged += cboAgency_SelectedIndexChanged;

            OnFormLoad(null, null);

            SplashScreenManager.CloseForm(false);

        }

        void btnPost_Click(object sender, EventArgs e)
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

                if (dt!= null && dt.Rows.Count>0 )
                {
                    using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
                    {
                        connect.Open();
                        _command = new SqlCommand("doSummaryAGPosting", connect)
                        {
                            CommandType = CommandType.StoredProcedure
                        };

                        _command.Parameters.Add(new SqlParameter("@pTransactiondb", SqlDbType.Structured)).Value = dt;
                        _command.Parameters.Add(new SqlParameter("@Userid", SqlDbType.VarChar)).Value = Program.UserID;
                        _command.CommandTimeout = 0;

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

                                //FrmReportPosting report = new FrmReportPosting(ds.Tables[1], ds.Tables[2]);
                                using (FrmReportPosting frmreport = new FrmReportPosting(ds))
                                {
                                    frmreport.ShowDialog();
                                }
                                //FrmRequest_Load(null, null);



                            }

                        }
                    }
                }

                
                
            }
            catch (Exception ex)
            {
                
            }
            finally
            {
                SplashScreenManager.CloseForm(false);
            }
        }

        void gridView1_DoubleClick(object sender, EventArgs e)
        {
            bool bResponse = false;
            GridView view = (GridView)gridControl1.FocusedView;
            if (view != null)
            {
                DataRow dr = view.GetDataRow(view.FocusedRowHandle);
                if (dr != null)
                {
                    ID = Convert.ToInt32(dr["SummaryID"]);
                    bResponse = FillField(Convert.ToInt32(dr["SummaryID"]));
                    if (bResponse)
                    {
                        boolIsUpdate = true;
                    }
                    else
                    {
                        boolIsUpdate = false;
                    }
                }
                else
                {
                    Common.setMessageBox("No Record is seleected", "", 1);
                    bResponse = false;
                }
            }
            //return bResponse;
        }

        void btnUp_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty((string) cboAccount.SelectedValue.ToString()))
            {
                Common.setEmptyField("Account Name",gridControl1.Text.ToString());
                return;
            }
            else if (string.IsNullOrEmpty((string)cboAgency.SelectedValue.ToString()))
            {
                Common.setEmptyField("Agency Name", gridControl1.Text.ToString());
                return;
            }
            else if (string.IsNullOrEmpty((string) cboRevenue.SelectedValue.ToString()))
            {
                Common.setEmptyField("Revenue Name", gridControl1.Text.ToString());
                return;
            }
            else if (txtAmount.EditValue == null || string.IsNullOrWhiteSpace(txtAmount.EditValue.ToString()))
            {
                Common.setEmptyField("Amount", this.Text); txtAmount.Focus(); return;
            }
            else
            {
                if (!boolIsUpdate)
                {
                    using (SqlConnection db = new SqlConnection(Logic.ConnectionString))
                    {
                        SqlTransaction transaction;

                        db.Open();

                        transaction = db.BeginTransaction();

                        try
                        {
                            string query = string.Format("INSERT INTO Reconciliation.tblSummaryAG ( BankAccountID , FinancialperiodID , Amount ,RevenueCode , AgencyCode) VALUES ('{0}','{1}','{2}','{3}','{4}');", Convert.ToInt32(cboAccount.SelectedValue), label22.Text.Trim(), Convert.ToDouble(txtAmount.EditValue), cboRevenue.SelectedValue, cboAgency.SelectedValue);

                            using (SqlCommand sqlCommand1 = new SqlCommand(query, db, transaction))
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
                    Common.setMessageBox("Record has been successfully added. Contact your Admin officers for Approval", Program.ApplicationName, 1);
 
                }
                else
                {
                    using (SqlConnection db = new SqlConnection(Logic.ConnectionString))
                    {
                        SqlTransaction transaction;

                        db.Open();

                        transaction = db.BeginTransaction();
                        try
                        {
                            //MessageBox.Show(MDIMain.stateCode);
                            //fieldid
                            string dry = String.Format(String.Format("UPDATE Reconciliation.tblSummaryAG SET BankAccountID='{{0}}',FinancialperiodID='{{1}}',Amount ='{{2}}',RevenueCode='{{3}}',AgencyCode='{{4}}' where  SummaryID ='{0}'", ID),cboAccount.SelectedValue,Convert.ToInt32(label22.Text),txtAmount.EditValue,cboRevenue.SelectedValue,cboAgency.SelectedValue);

                            using (SqlCommand sqlCommand1 = new SqlCommand(dry, db, transaction))
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
                    Common.setMessageBox("Record has been successfully added.Contact your Admin officers for Approval", Program.ApplicationName, 1);
                    setReload(); Clear();
                
                }

                if (MessageBox.Show("Do you want to add another record?", Program.ApplicationName, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
                {
                    setReload(); Clear();
                    return;

                }
                else
                {
                    //bttnReset.PerformClick();
                    setReload(); Clear();
                }

            }
        }

        void cboAgency_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboAgency.SelectedValue != null && !isRecord)
            {
                setDBComboRevenue();
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
            //bttnReset.Image = MDIMains.publicMDIParent.i32x32.Images[8];
            //bttnUpdate.Image = MDIMain.publicMDIParent.i32x32.Images[7];
            //bttnUpdate.Image = MDIMains.publicMDIParent.i32x32.Images[7];

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
                //    tsbReload.PerformClick();
                //boolIsUpdate = false;
            }
            else if (sender == tsbReload)
            {
                //iTransType = TransactionTypeCode.Reload;
                //ShowForm();
            }
            //bttnReset.PerformClick();
        }

        void OnFormLoad(object sender, EventArgs e)
        {
            setDBComboBoxAcct();

            setDBComboAgency(); setReload();

            isFirst = false;

        }

        public void RefreshForm()
        {
            NavBars.ToolStripEnableDisableControls(toolStrip, Tag as String);
        }
        void setDBComboBoxAcct()
        {
            try
            {
                using (var ds = new System.Data.DataSet())
                {
                    using (SqlDataAdapter ada = new SqlDataAdapter(String.Format("SELECT BankName + ' - ' + AccountNumber AS Description,BankAccountID FROM ViewCurrencyBankAccount INNER JOIN Collection.tblBank ON Collection.tblBank.BankShortCode = dbo.ViewCurrencyBankAccount.BankShortCode"), Logic.ConnectionString))
                    {
                        ada.Fill(ds, "table");
                    }

                    Common.setComboList(cboAccount, ds.Tables[0], "BankAccountID", "Description");

                }

                cboAccount.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                Tripous.Sys.ErrorBox(String.Format("{0}{1}", ex.Message, ex.StackTrace)); return;
            }
        }
        void setDBComboAgency()
        {
            try
            {
                using (var ds = new System.Data.DataSet())
                {
                    using (SqlDataAdapter ada = new SqlDataAdapter(String.Format("SELECT AgencyCode,AgencyName FROM Registration.tblAgency ORDER BY AgencyName"), Logic.ConnectionString))
                    {
                        ada.Fill(ds, "table");
                    }

                    Common.setComboList(cboAgency, ds.Tables[0], "AgencyCode", "AgencyName");

                }

                cboAgency.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                Tripous.Sys.ErrorBox(String.Format("{0}{1}", ex.Message, ex.StackTrace)); return;
            }
        }
        void setDBComboRevenue()
        {
            try
            {
                isRecord = true;

                using (var ds = new System.Data.DataSet())
                {
                    using (SqlDataAdapter ada = new SqlDataAdapter(String.Format("SELECT RevenueCode,Description FROM Collection.tblRevenueType WHERE AgencyCode='{0}'", cboAgency.SelectedValue.ToString()), Logic.ConnectionString))
                    {
                        ada.Fill(ds, "table");
                    }

                    Common.setComboList(cboRevenue, ds.Tables[0], "RevenueCode", "Description");

                }

                cboRevenue.SelectedIndex = -1; isRecord = false;
            }
            catch (Exception ex)
            {
                Tripous.Sys.ErrorBox(String.Format("{0}{1}", ex.Message, ex.StackTrace)); return;
            }
        }
        void openForm()
        {
            using (FrmFinanicalYear fyear = new FrmFinanicalYear("Summary"))
            {
                fyear.ShowDialog();
            }
        }
        private void setReload()
        {
            try
            {
                //DataTable dtc;
                //setReloadsExtracted();
                //System.Data.DataSet ds;
                using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
                {
                    connect.Open();
                    _command = new SqlCommand("doSummaryAG", connect) { CommandType = CommandType.StoredProcedure };
                    _command.Parameters.Add(new SqlParameter("@FinancialperiodID", SqlDbType.VarChar)).Value = Convert.ToInt32(label22.Text);
                   
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
                            dt.Clear();
                            dt = ds.Tables[1];
                            gridControl1.DataSource = ds.Tables[1];
                        }

                    }
                }



                gridView1.Columns["Amount"].DisplayFormat.FormatType = FormatType.Numeric;
                gridView1.Columns["Amount"].DisplayFormat.FormatString = "n2";

                gridView1.Columns["Amount"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                gridView1.Columns["Amount"].SummaryItem.FieldName = "Amount";
                gridView1.Columns["Amount"].SummaryItem.DisplayFormat = "Total = {0:n}";
                gridView1.Columns["Amount"].OptionsColumn.AllowEdit = false;
                //gridView1.Columns["Date"].OptionsColumn.AllowEdit = false;
                gridView1.Columns["FinancialperiodID"].Visible = false;
                gridView1.Columns["BankAccountID"].Visible = false;
                gridView1.Columns["SummaryID"].Visible = false;
                //gridView1.Columns["FinancialperiodID"].Visible = false;

                gridView1.OptionsView.ColumnAutoWidth = false;
                gridView1.OptionsView.ShowFooter = true;
                gridView1.BestFitColumns();
            }
            catch (Exception ex)
            {
                Tripous.Sys.ErrorBox(String.Format("{0}{1}", ex.Message, ex.StackTrace)); return;
            }
        }

        private bool FillField(int fieldid)
        {
            bool bResponse = false;


            DataTable dts = (new Logic()).getSqlStatement((String.Format("SELECT ViewFinancialperiod.DESCRIPTION, BankName + ' - ' + AccountNumber AS Description ,ViewCurrencyBankAccount.BankAccountID ,Reconciliation.tblSummaryAG.SummaryID ,Reconciliation.tblSummaryAG.FinancialperiodID ,Reconciliation.tblSummaryAG.AgencyCode ,Reconciliation.tblSummaryAG.RevenueCode ,Collection.tblRevenueType.Description,AgencyName,Amount FROM ViewCurrencyBankAccount INNER JOIN Collection.tblBank ON Collection.tblBank.BankShortCode = dbo.ViewCurrencyBankAccount.BankShortCode INNER JOIN Reconciliation.tblSummaryAG ON Reconciliation.tblSummaryAG.BankAccountID = dbo.ViewCurrencyBankAccount.BankAccountID INNER JOIN dbo.ViewFinancialperiod ON dbo.ViewFinancialperiod.FinancialperiodID = Reconciliation.tblSummaryAG.FinancialperiodID     INNER JOIN Collection.tblRevenueType ON Collection.tblRevenueType.RevenueCode = Reconciliation.tblSummaryAG.RevenueCode AND Collection.tblRevenueType.AgencyCode = Reconciliation.tblSummaryAG.AgencyCode AND Collection.tblRevenueType.AgencyCode = Reconciliation.tblSummaryAG.AgencyCode INNER JOIN Registration.tblAgency ON Registration.tblAgency.AgencyCode = Collection.tblRevenueType.AgencyCode WHERE Reconciliation.tblSummaryAG.SummaryID ='{0}'", fieldid))).Tables[0];

            if (dts != null)
            {
                bResponse = true;

                cboAccount.SelectedValue = dts.Rows[0]["BankAccountID"].ToString();
                cboAgency.SelectedValue = dts.Rows[0]["AgencyCode"].ToString();
                cboRevenue.SelectedValue = dts.Rows[0]["RevenueCode"].ToString();
                txtAmount.EditValue = String.Format("{0:N2}", dts.Rows[0]["Amount"]);
                //cboCurrency.Text = dts.Rows[0]["Description"].ToString();
                //cboBranch.Text
                //txtOpneing.Text = String.Format("{0:N2}", dts.Rows[0]["Amount"]);
                //chkActive.CheckState = dts.Rows[0]["Status"].ToString();


            }
            else
                bResponse = false;

            return bResponse;
        }

        private void Clear()
        {
            //txtStreetGroup.Clear();
            cboRevenue.SelectedValue = -1; cboAccount.SelectedValue = -1; cboAgency.SelectedValue = -1;
            txtAmount.EditValue=0;


        }


    }
}
