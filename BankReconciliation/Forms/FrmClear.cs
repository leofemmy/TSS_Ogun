using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BankReconciliation.Class;
using DevExpress.Utils;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Selection;
using DevExpress.XtraSplashScreen;
using TaxSmartSuite.Class;

namespace BankReconciliation.Forms
{
    public partial class FrmClear : Form
    {
        public static FrmClear publicStreetGroup; private bool Isbank = false; private bool isRecord = false; private SqlCommand _command; private SqlDataAdapter adp;
        public FrmClear()
        {
            InitializeComponent();

            publicStreetGroup = this;

            setImages();

            ToolStripEvent();

            Load += OnFormLoad;

            OnFormLoad(null, null);
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
            setDbComboFiancial();

            cboBank.SelectedIndexChanged += cboBank_SelectedIndexChanged;
            btnSearch.Click += btnSearch_Click; btnClear.Click += btnClear_Click;

        }

        void btnClear_Click(object sender, EventArgs e)
        {
            string strv =
                string.Format(
                    "Transaction have been done on {0} for {1} financial period. Continue to clear this transaction will erase all that have been done",cboBank.Text.Trim().ToString(),cbofinancial.Text.Trim().ToString());

            DialogResult result = MessageBox.Show(strv, "Clear Transaction", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            //DialogResult resu = MessageBox.Show();

            if (result == DialogResult.Yes) //excele 2003
            {
                //Common.setMessageBox("Services Supsend for Now",Program.ApplicationName,1);
                //return;
                try
                {
                    SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                    using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
                    {
                        connect.Open();
                        _command = new SqlCommand("CleartTransaction", connect)
                        {
                            CommandType = CommandType.StoredProcedure
                        };
                        _command.Parameters.Add(new SqlParameter("@bankcode", SqlDbType.VarChar)).Value =
                            cboBank.SelectedValue;
                        _command.Parameters.Add(new SqlParameter("@Periods", SqlDbType.Int)).Value =
                            cbofinancial.SelectedValue;
                        _command.Parameters.Add(new SqlParameter("@AccountId", SqlDbType.Int)).Value =
                            Convert.ToInt32(cboAccount.SelectedValue);
                        _command.CommandTimeout = 0;
                        using (System.Data.DataSet ds = new System.Data.DataSet())
                        {
                            ds.Clear();
                            adp = new SqlDataAdapter(_command);
                            adp.Fill(ds);
                            //Dts = ds.Tables[0];
                            connect.Close();

                            if (ds.Tables[0].Rows[0]["returnCode"].ToString() == "00")
                            {
                                Common.setMessageBox(ds.Tables[0].Rows[0]["returnMessage"].ToString(),
                                    Program.ApplicationName, 2);
                                gridControl1.DataSource = null;
                                cbofinancial.SelectedIndex = -1;
                                cboBank.SelectedIndex = -1;
                                cboAccount.SelectedIndex = -1;
                            }
                            else
                            {
                                Common.setMessageBox(ds.Tables[0].Rows[0]["returnMessage"].ToString(),
                                     Program.ApplicationName, 2);
                            }
                        }
                    }

                }
                catch (Exception ex)
                {
                    Tripous.Sys.ErrorBox(String.Format("{0}{1}", ex.Message, ex.StackTrace));
                    return;
                }
                finally
                {
                    SplashScreenManager.CloseForm(false);
                }
            }
            else
            {
                return;
            }
        }

        void btnSearch_Click(object sender, EventArgs e)
        {
            gridControl1.DataSource = null;

            if (cboBank.SelectedIndex == -1)
            {
                Common.setEmptyField("Bank Name", Program.ApplicationName); return;

            }
            else if (cboAccount.SelectedIndex == -1)
            {
                Common.setEmptyField("bank Account", Program.ApplicationName); return;
            }
            else if (cbofinancial.SelectedIndex == -1)
            {
                Common.setEmptyField("Financial Period", Program.ApplicationName);
                return;
            }
            else
            {
                try
                {
                    SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

                    using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
                    {
                        connect.Open();
                        _command = new SqlCommand("getCleartTransaction", connect)
                        {
                            CommandType = CommandType.StoredProcedure
                        };
                        _command.Parameters.Add(new SqlParameter("@bankcode", SqlDbType.VarChar)).Value =
                            cboBank.SelectedValue;
                        _command.Parameters.Add(new SqlParameter("@Periods", SqlDbType.Int)).Value =
                            cbofinancial.SelectedValue;
                        _command.Parameters.Add(new SqlParameter("@AccountId", SqlDbType.Int)).Value =
                            Convert.ToInt32(cboAccount.SelectedValue);
                        _command.CommandTimeout = 0;
                        using (System.Data.DataSet ds = new System.Data.DataSet())
                        {
                            ds.Clear();
                            adp = new SqlDataAdapter(_command);
                            adp.Fill(ds);
                            //Dts = ds.Tables[0];
                            connect.Close();

                            if (ds.Tables[0].Rows[0]["returnCode"].ToString() == "00")
                            {
                                //Common.setMessageBox(ds.Tables[0].Rows[0]["returnMessage"].ToString(),
                                //    Program.ApplicationName, 2);

                                if (ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0)
                                {

                                    lblSelect.Text = string.Format("{0} Already Transact records found",
                                        ds.Tables[1].Rows.Count);

                                    gridControl1.DataSource = ds.Tables[1];

                                    gridView1.OptionsBehavior.Editable = false;
                                    gridView1.Columns["date"].DisplayFormat.FormatType = FormatType.DateTime;
                                    gridView1.Columns["date"].DisplayFormat.FormatString = "dd/MM/yyyy";
                                    gridView1.Columns["Debit"].DisplayFormat.FormatType = FormatType.Numeric;
                                    gridView1.Columns["Debit"].DisplayFormat.FormatString = "n2";

                                    gridView1.Columns["Credit"].DisplayFormat.FormatType = FormatType.Numeric;
                                    gridView1.Columns["Credit"].DisplayFormat.FormatString = "n2";

                                    gridView1.Columns["Balance"].DisplayFormat.FormatType = FormatType.Numeric;
                                    gridView1.Columns["Balance"].DisplayFormat.FormatString = "n2";
                                    gridView1.BestFitColumns();
                                }
                                else
                                {
                                    lblSelect.Text = string.Format("No Transact records found");
                                }
                            }
                            else
                            {
                                Common.setMessageBox(ds.Tables[0].Rows[0]["returnMessage"].ToString(),
                                    Program.ApplicationName, 2);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Tripous.Sys.ErrorBox(String.Format("{0}{1}", ex.Message, ex.StackTrace));
                    return;
                }
                finally
                {
                    SplashScreenManager.CloseForm(false);
                }

            }
        }

        void cboBank_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboBank.SelectedValue != null && !Isbank)
            {
                setDBComboBoxAcct();

            }
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

        private void setDbComboFiancial()
        {
            DataTable Dt;

            //connect.connect.Close();

            using (var ds = new System.Data.DataSet())
            {
                //connect.connect.Open();
                using (SqlDataAdapter ada = new SqlDataAdapter((string)"SELECT Months +','+Year AS Months,FinancialperiodID FROM Reconciliation.tblFinancialperiod ORDER BY Year", Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }

                Dt = ds.Tables[0];
            }

            Common.setComboList(cbofinancial, Dt, "FinancialperiodID", "Months");

            cbofinancial.SelectedIndex = -1;
        }

    }
}
