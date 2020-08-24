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
    public partial class FrmExchangeRate : Form
    {
        public static FrmExchangeRate publicStreetGroup; protected int ID; bool isFirst = true;

        protected TransactionTypeCode iTransType; protected bool boolIsUpdate;

        public FrmExchangeRate()
        {
            SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

            InitializeComponent();

            publicStreetGroup = this;

            setImages();

            ToolStripEvent();

            iTransType = TransactionTypeCode.Null;

            Load += OnFormLoad;

            gridView1.DoubleClick += gridView1_DoubleClick;

            bttnUpdate.Click += bttnUpdate_Click; bttnCancel.Click += bttnCancel_Click;

            OnFormLoad(null, null);

            SplashScreenManager.CloseForm(false);


        }

        void bttnCancel_Click(object sender, EventArgs e)
        {
            tsbReload.PerformClick();
        }

        void bttnUpdate_Click(object sender, EventArgs e)
        {

            if (String.Compare(cboCurrency.Text, "", false) == 0)
            {
                Common.setEmptyField("Currency", Program.ApplicationName);
                cboCurrency.Focus(); return;
            }
            else if (txtRate.EditValue == null || string.IsNullOrWhiteSpace(txtRate.EditValue.ToString()))
            {
                Common.setEmptyField("Transfer / Charges, can't be empty", this.Text); txtRate.Focus(); return;
            }
            else
            {
                //check form status either new or edit
                if (!boolIsUpdate)
                {
                    using (SqlConnection db = new SqlConnection(Logic.ConnectionString))
                    {
                        SqlTransaction transaction;

                        db.Open();

                        transaction = db.BeginTransaction();
                        try
                        {
                            string query = String.Format("INSERT INTO [Reconciliation].[tblExchangeRate]([CurrencyID],[Rate]) VALUES ('{0}','{1}');", Convert.ToInt32(cboCurrency.SelectedValue),Convert.ToDouble(txtRate.EditValue));

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
                    setReload();
                    Common.setMessageBox("Record has been successfully added", Program.ApplicationName, 1);

                    if (MessageBox.Show("Do you want to add another record?", Program.ApplicationName, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
                    {
                        bttnCancel.PerformClick();

                    }
                    else
                    {

                        setReload(); txtRate.EditValue = string.Empty;
                        Clear();
                    }
                    //}
                }
                else
                {
                    //update the records

                    using (SqlConnection db = new SqlConnection(Logic.ConnectionString))
                    {
                        SqlTransaction transaction;

                        db.Open();

                        transaction = db.BeginTransaction();
                        try
                        {
                            //MessageBox.Show(MDIMain.stateCode);
                            //fieldid
                            string dry = String.Format(String.Format("UPDATE [Reconciliation].[tblExchangeRate] SET [CurrencyID]='{{0}}',[Rate]='{{1}}' where  ExchangeRateID ='{0}'", ID),Convert.ToInt32(cboCurrency.SelectedValue),Convert.ToDouble(txtRate.EditValue));

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

                    setReload();
                    Common.setMessageBox("Changes in record has been successfully saved.", Program.ApplicationName, 1);
                    Clear();
                    //bttnReset.PerformClick();
                    tsbReload.PerformClick();

                }
            }
        }

        void gridView1_DoubleClick(object sender, EventArgs e)
        {
            tsbEdit.PerformClick();
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

            bttnCancel.Image = MDIMains.publicMDIParent.i32x32.Images[9];
            //bttnReset.Image = MDIMains.publicMDIParent.i32x32.Images[8];
            //bttnUpdate.Image = MDIMain.publicMDIParent.i32x32.Images[7];
            bttnUpdate.Image = MDIMains.publicMDIParent.i32x32.Images[7];

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
                groupControl2.Text = "Add New Record";
                iTransType = TransactionTypeCode.New;
                ShowForm();
                boolIsUpdate = false;
            }
            else if (sender == tsbEdit)
            {
                groupControl2.Text = "Edit Record Mode";
                iTransType = TransactionTypeCode.Edit;
                if (EditRecordMode())
                {
                    ShowForm();
                    boolIsUpdate = true;
                }
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
                //        deleteRecord(ID);
                //}
                //else
                //    tsbReload.PerformClick();
                //boolIsUpdate = false;
            }
            else if (sender == tsbReload)
            {
                iTransType = TransactionTypeCode.Reload; setReload();
                ShowForm();
            }
            //bttnReset.PerformClick();
        }

        void OnFormLoad(object sender, EventArgs e)
        {
            ShowForm();

            setDbComboBoxCurrency();
            //setDBComboBoxTn();
            isFirst = false;
            setReload();


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

        void setDbComboBoxCurrency()
        {
            DataTable Dt;

            //connect.connect.Close();

            using (var ds = new System.Data.DataSet())
            {
                //connect.connect.Open();
                using (SqlDataAdapter ada = new SqlDataAdapter((string)"SELECT Description + ' (' + LTRIM(RTRIM(CurrencyCode)) +')' AS Description,CurrencyID FROM Reconciliation.tblCurrency", Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }

                Dt = ds.Tables[0];
            }

            Common.setComboList(cboCurrency, Dt, "CurrencyID", "Description");

            cboCurrency.SelectedIndex = -1;
        }

        private void setReload()
        {
            //connect.connect.Close();
            DataTable dt;
            //System.Data.DataSet ds;
            using (var ds = new System.Data.DataSet())
            {
                //connect.connect.Open();
                using (SqlDataAdapter ada = new SqlDataAdapter("select * from viewExchangeRate", Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }
                //connect.connect.Close();
                dt = ds.Tables[0];
                gridControl1.DataSource = dt.DefaultView;
            }
            gridView1.OptionsBehavior.Editable = false;
            gridView1.Columns["Rate"].DisplayFormat.FormatType = FormatType.Numeric;
            gridView1.Columns["Rate"].DisplayFormat.FormatString = "n2";
            gridView1.Columns["ExchangeRateID"].Visible = false;
            gridView1.Columns["CurrencyID"].Visible = false;

            //Status
            gridView1.BestFitColumns();
        }

        protected bool EditRecordMode()
        {
            bool bResponse = false;
            GridView view = (GridView)gridControl1.FocusedView;
            if (view != null)
            {
                DataRow dr = view.GetDataRow(view.FocusedRowHandle);
                if (dr != null)
                {
                    ID = Convert.ToInt32(dr["ExchangeRateID"]);
                    bResponse = FillField(Convert.ToInt32(dr["ExchangeRateID"]));
                }
                else
                {
                    Common.setMessageBox("No Record is seleected", "", 1);
                    bResponse = false;
                }
            }
            return bResponse;
        }

        private bool FillField(int fieldid)
        {
            bool bResponse = false;

            DataTable dts = (new Logic()).getSqlStatement((String.Format("select * from viewExchangeRate where ExchangeRateID ='{0}'", fieldid))).Tables[0];

            if (dts != null)
            {
                bResponse = true;

                cboCurrency.Text = dts.Rows[0]["Description"].ToString();
                txtRate.EditValue = String.Format("{0:N2}", dts.Rows[0]["Rate"]);
                
            }
            else
                bResponse = false;

            return bResponse;
        }

        void Clear()
        {
            cboCurrency.SelectedIndex = -1; txtRate.EditValue = string.Empty; cboCurrency.Focus();
        }
    }
}
