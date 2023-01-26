using BankReconciliation.Class;
using DevExpress.Utils;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Selection;
using DevExpress.XtraSplashScreen;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using TaxSmartSuite.Class;

namespace BankReconciliation.Forms
{
    public partial class FrmSwept : Form
    {
        public static FrmSwept publicStreetGroup; private bool Isbank = false; private bool isRecord = false; private SqlCommand _command; private SqlDataAdapter adp;

        private DataTable dtc;

        RepositoryItemGridLookUpEdit repComboLookBox = new RepositoryItemGridLookUpEdit();

        GridCheckMarksSelection selection; bool isFirstGrid = true;

        GridColumn colView2 = new GridColumn();

        RepositoryItemGridLookUpEdit repComboLookBoxCredit = new RepositoryItemGridLookUpEdit();

        DataTable temTable = new DataTable();

        public FrmSwept()
        {
            SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

            InitializeComponent();

            publicStreetGroup = this;

            setImages();

            ToolStripEvent();

            Load += OnFormLoad;

            cboBank.SelectedIndexChanged += cboBank_SelectedIndexChanged;

            cboBank.KeyPress += cboBank_KeyPress;

            btnSelect.Click += btnSelect_Click;

            btnUpdate.Click += btnUpdate_Click;

            OnFormLoad(null, null);

            temTable.Columns.Add("BsID", typeof(int));

            temTable.Columns.Add("DefinitionID", typeof(int));

            SplashScreenManager.CloseForm(false);
        }

        void btnUpdate_Click(object sender, EventArgs e)
        {
            temTable.Clear();

            //tableTrans.Rows.Add(new object[] { txtDateds.Text, ElemDescr, 0, txtDepositAmt.Text });
            for (int i = 0; i < selection.SelectedCount; i++)
            {
                //temTable.Rows.Add(new object[] { String.Format("{0}", (selection.GetSelectedRow(i) as DataRowView)["BSID"]), String.Format("{0}", (selection.GetSelectedRow(i) as DataRowView)["DefinitionID"]) });

                temTable.Rows.Add(new object[] { String.Format("{0}", (selection.GetSelectedRow(i) as DataRowView)["BSID"]), String.Format("{0}", (selection.GetSelectedRow(i) as DataRowView)["Description"]) });
            }

            try
            {
                using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
                {
                    connect.Open();
                    _command = new SqlCommand("doSwepTransaction", connect) { CommandType = CommandType.StoredProcedure };
                    _command.Parameters.Add(new SqlParameter("@pTransactiondb", SqlDbType.Structured)).Value = temTable;


                    using (System.Data.DataSet ds = new System.Data.DataSet())
                    {
                        ds.Clear();
                        adp = new SqlDataAdapter(_command);
                        adp.Fill(ds);
                        //Dts = ds.Tables[0];
                        connect.Close();

                        if (ds.Tables[0].Rows[0]["returnCode"].ToString() != "00")
                        {
                            Common.setMessageBox(ds.Tables[0].Rows[0]["returnMessage"].ToString(), Program.ApplicationName, 3);
                        }
                        else
                        {
                            Common.setMessageBox(ds.Tables[0].Rows[0]["returnMessage"].ToString(), Program.ApplicationName, 1);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Tripous.Sys.ErrorBox(String.Format("{0}{1}Allocating", ex.StackTrace, ex.Message)); return;
            }
        }

        void btnSelect_Click(object sender, EventArgs e)
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

                if (string.IsNullOrEmpty((string)(cboBank.SelectedValue)))
                {
                    Common.setEmptyField("BanK Name", Program.ApplicationName);
                    cboBank.Focus();
                    return;
                }
                else if (string.IsNullOrEmpty((string)(cboAccount.SelectedValue.ToString())))
                {
                    Common.setEmptyField("Account Number", Program.ApplicationName); cboAccount.Focus(); return;
                }
                else
                {
                    using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
                    {
                        connect.Open();

                        _command = new SqlCommand("doUnsweptTransaction", connect) { CommandType = CommandType.StoredProcedure };
                        _command.Parameters.Add(new SqlParameter("@bankcode", SqlDbType.VarChar)).Value = cboBank.SelectedValue;
                        _command.Parameters.Add(new SqlParameter("@startdate", SqlDbType.VarChar)).Value = string.Format("{0:yyyy/MM/dd 00:00:00}", dtpStart.Value);
                        _command.Parameters.Add(new SqlParameter("@enddate", SqlDbType.VarChar)).Value = string.Format("{0:yyyy/MM/dd 23:59:59}", dtpEnd.Value);
                        _command.Parameters.Add(new SqlParameter("@AccountId", SqlDbType.Int)).Value = Convert.ToInt32(cboAccount.SelectedValue);

                        _command.CommandTimeout = 0;

                        using (System.Data.DataSet ds = new System.Data.DataSet())
                        {
                            adp = new SqlDataAdapter(_command);
                            adp.Fill(ds, "ViewUnsweptTransaction");
                            //Dts = ds.Tables[0];
                            connect.Close();

                            if (ds.Tables[0].Rows[0]["returnCode"].ToString() == "00")
                            {
                                dtc = new DataTable();
                                dtc.Clear();
                                dtc = ds.Tables[1];

                                dtc.Columns.Add("Description", typeof(String));

                                dtc.AcceptChanges();

                                gridControl1.DataSource = dtc;

                            }
                            else
                            {
                                Tripous.Sys.ErrorBox(String.Format("{0}{1}", ds.Tables[0].Rows[0]["returnCode"].ToString(), ds.Tables[0].Rows[0]["returnMessage"].ToString()));

                                return;
                            }

                            AddCombDefinition();

                            gridView1.Columns["Amount"].DisplayFormat.FormatType = FormatType.Numeric;
                            gridView1.Columns["Amount"].DisplayFormat.FormatString = "n2";
                            gridView1.Columns["BSDate"].DisplayFormat.FormatType = FormatType.DateTime;
                            gridView1.Columns["BSDate"].DisplayFormat.FormatString = "dd/MM/yyyy";
                            gridView1.Columns["BankAccountID"].Visible = false;
                            gridView1.Columns["BankName"].Visible = false;
                            gridView1.Columns["ExchangeRate"].Visible = false;
                            gridView1.Columns["CurrencyCode"].Visible = false;

                            colView2 = gridView1.Columns["Description"];
                            colView2.ColumnEdit = repComboLookBox;

                            colView2.Visible = true;

                            gridView1.Columns["BSID"].Visible = false;
                            gridView1.Columns["Credit"].Visible = false;
                            gridView1.Columns["EndDate"].Visible = false;
                            gridView1.Columns["FinancialperiodID"].Visible = false;
                            gridView1.Columns["Periods"].Visible = false;
                            gridView1.Columns["StartDate"].Visible = false;
                            gridView1.Columns["AccountNumber"].Visible = false;
                            gridView1.Columns["BankShortCode"].Visible = false;


                            if (isFirstGrid)
                            {
                                selection = new GridCheckMarksSelection(gridView1, ref lblSelect);
                                selection.CheckMarkColumn.VisibleIndex = 0;
                                isFirstGrid = false;
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

        void cboBank_KeyPress(object sender, KeyPressEventArgs e)
        {
            Methods.AutoComplete(cboBank, e, true);
        }

        void cboBank_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboBank.SelectedValue != null && !Isbank)
            {
                setDBComboBoxAcct();

            }
        }

        public void RefreshForm()
        {
            NavBars.ToolStripEnableDisableControls(toolStrip, Tag as String);
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

        public void setDBComboBox()
        {
            DataTable Dt;

            //connect.connect.Close();

            using (var ds = new System.Data.DataSet())
            {
                //connect.connect.Open();
                using (SqlDataAdapter ada = new SqlDataAdapter((string)"SELECT BankShortCode,BankName FROM Collection.tblBank", Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }

                Dt = ds.Tables[0];
            }

            Common.setComboList(cboBank, Dt, "BankShortCode", "BankName");

            cboBank.SelectedIndex = -1;


        }

        void setDBComboBoxAcct()
        {
            try
            {
                isRecord = true;

                using (var ds = new System.Data.DataSet())
                {
                    using (SqlDataAdapter ada = new SqlDataAdapter(String.Format("SELECT BankAccountID,AccountNumber FROM ViewCurrencyBankAccount WHERE BankShortCode='{0}'", cboBank.SelectedValue.ToString()), Logic.ConnectionString))
                    {
                        ada.Fill(ds, "table");
                    }

                    Common.setComboList(cboAccount, ds.Tables[0], "BankAccountID", "AccountNumber");

                }

                cboAccount.SelectedIndex = -1; isRecord = false;
            }
            catch (Exception ex)
            {
                Tripous.Sys.ErrorBox(String.Format("{0}{1}", ex.Message, ex.StackTrace)); return;
            }
        }

        void AddCombDefinition()
        {
            try
            {
                DataTable dtse = new DataTable();

                repComboLookBox.DataSource = null;

                dtse.Clear();
                //System.Data.DataSet ds;
                using (var dsed = new System.Data.DataSet())
                {
                    dsed.Clear();

                    using (SqlDataAdapter adas = new SqlDataAdapter("SELECT DefinitionID,Description FROM Reconciliation.tblSwepDefinition", Logic.ConnectionString))
                    {
                        adas.Fill(dsed, "table");

                    }

                    repComboLookBox.NullText = "Select Text";

                    dtse = dsed.Tables[0];

                    if (dtse != null && dtse.Rows.Count > 0)
                    {
                        repComboLookBox.DataSource = dtse;
                        repComboLookBox.DisplayMember = "Description";
                        repComboLookBox.ValueMember = "DefinitionID";
                        repComboLookBox.AllowNullInput = DefaultBoolean.True;
                        //Autocomplete on all values

                        repComboLookBox.PopulateViewColumns();
                        var view = repComboLookBox.View;
                        view.Columns["DefinitionID"].Visible = false;

                        repComboLookBox.AutoComplete = true;
                    }

                }
            }
            catch (Exception ex)
            {
                Tripous.Sys.ErrorBox(String.Format("{0}{1}", ex.Message, ex.StackTrace)); return;
            }
        }

    }
}
