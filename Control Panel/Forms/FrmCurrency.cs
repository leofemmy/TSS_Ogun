using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TaxSmartSuite.Class;
using DevExpress.XtraGrid.Views.Grid;
using System.Data.SqlClient;
using Control_Panel.Class;

namespace Control_Panel.Forms
{
    public partial class FrmCurrency : Form
    {
        //DBConnection connect = new DBConnection();

        //Methods extMethods = new Methods();

        public static FrmCurrency publicStreetGroup;

        protected TransactionTypeCode iTransType;

        protected bool boolIsUpdate;

        protected int ID;

        bool isFirst = true;

        public FrmCurrency()
        {
            InitializeComponent();

            //connect.ConnectionString();

            publicStreetGroup = this;

            setImages();

            ToolStripEvent();

            iTransType = TransactionTypeCode.Null;

            Load += OnFormLoad;

            gridView3.DoubleClick += gridView1_DoubleClick;

            bttnCancel.Click += Bttn_Click;

            bttnReset.Click += Bttn_Click;

            bttnUpdate.Click += Bttn_Click;

            OnFormLoad(null, null);
        }

        private void setImages()
        {
            tsbNew.Image = MDIMain.publicMDIParent.i16x16.Images[5];
            tsbEdit.Image = MDIMain.publicMDIParent.i16x16.Images[6];
            tsbDelete.Image = MDIMain.publicMDIParent.i16x16.Images[7];
            tsbReload.Image = MDIMain.publicMDIParent.i16x16.Images[0];
            tsbClose.Image = MDIMain.publicMDIParent.i16x16.Images[11];

            bttnCancel.Image = MDIMain.publicMDIParent.i32x32.Images[9];
            bttnReset.Image = MDIMain.publicMDIParent.i32x32.Images[8];
            bttnUpdate.Image = MDIMain.publicMDIParent.i32x32.Images[7];

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
                MDIMain.publicMDIParent.RemoveControls();
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
                groupControl2.Text = "Delete Record Mode";
                iTransType = TransactionTypeCode.Delete;
                if (MosesClassLibrary.Utilities.Common.AskQuestion("Deleting this record will delete attached record.\nDo you want to continue?", ""))
                {
                }
                else
                    tsbReload.PerformClick();
                boolIsUpdate = false;
            }
            else if (sender == tsbReload)
            {
                iTransType = TransactionTypeCode.Reload;
                ShowForm();
            }
            bttnReset.PerformClick();
        }

        void gridView1_DoubleClick(object sender, EventArgs e)
        {
            tsbEdit.PerformClick();
        }

        void OnFormLoad(object sender, EventArgs e)
        {
            ShowForm();
            //setDBComboBox();
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

        protected bool EditRecordMode()
        {
            bool bResponse = false;
            GridView view = (GridView)gridControl1.FocusedView;
            if (view != null)
            {
                DataRow dr = view.GetDataRow(view.FocusedRowHandle);
                if (dr != null)
                {
                    ID = Convert.ToInt32(dr["CurrencyID"]);
                    bResponse = FillField(Convert.ToInt32(dr["CurrencyID"]));
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
            ////load data from the table into the forms for edit
            //DataTable dts = extMethods.LoadData(String.Format("select * from ViewCurrencyExchange where CurrencyID ='{0}'", fieldid));
            DataTable dts = (new Logic()).getSqlStatement((String.Format("select * from ViewCurrencyExchange where CurrencyID ='{0}'", fieldid))).Tables[0];

            if (dts != null)
            {
                bResponse = true;

                txtCode.Text = dts.Rows[0]["CurrencyCode"].ToString();
                txtName.Text = dts.Rows[0]["CurrencyName"].ToString();
                txtRate.Text = dts.Rows[0]["Exchange"].ToString();
                

            }
            else
                bResponse = false;

            return bResponse;
        }

        void Bttn_Click(object sender, EventArgs e)
        {
            if (sender == bttnCancel)
            {
                tsbReload.PerformClick();
            }
            else if (sender == bttnReset)
            {
                if (!boolIsUpdate)
                    Clear();
                else
                    FillField(ID);
                //setReload();
            }
            else if (sender == bttnUpdate)
            {
                UpdateRecord();
            }
        }

        private void Clear()
        {
            //txtStreetGroup.Clear();
            txtCode.Clear();
            txtName.Clear();
            txtRate.Clear();


        }

        public void RefreshForm()
        {
            NavBars.ToolStripEnableDisableControls(toolStrip, Tag as String);
        }

        private void UpdateRecord()
        {
            try
            {
                if (txtCode.Text == "")
                {
                    Common.setEmptyField("Currency Code", Program.ApplicationName);
                    txtCode.Focus(); return;
                }
                else if (txtName.Text == "")
                {
                    Common.setEmptyField("Currency Name", Program.ApplicationName);
                    txtName.Focus(); return;
                }
                else if (txtRate.Text == "")
                {
                    Common.setEmptyField("Exchange Rate", Program.ApplicationName);
                    txtRate.Focus(); return;
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

                                //insert into currency table
                                int recs = Convert.ToInt32(new SqlCommand(String.Format("INSERT INTO [tblCurrency]([CurrencyName],[CurrencyCode],[Flag]) VALUES ('{0}', '{1}', '{2}');SELECT @@IDENTITY", txtName.Text.Trim().ToUpperInvariant(), txtCode.Text.Trim().ToUpperInvariant(), chkCurrency.Checked), db, transaction).ExecuteScalar());


                                using (SqlCommand sqlCommand1 = new SqlCommand(String.Format("INSERT INTO [tblExchangeRate]([Rate],[CurrencyID]) VALUES ('{0}', '{1}');", Convert.ToDouble(txtRate.Text.Trim()), recs), db, transaction))
                                {
                                    sqlCommand1.ExecuteNonQuery();
                                }

                                transaction.Commit();
                            }
                            catch (SqlException sqlError)
                            {
                                transaction.Rollback();
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
                            bttnReset.PerformClick();
                            setReload();
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
                                //fieldid ,,
                                using (SqlCommand sqlCommand1 = new SqlCommand(String.Format(String.Format("UPDATE [tblCurrency] SET [CurrencyName]='{{0}}',[CurrencyCode] ='{{1}}',[Flag]='{{2}}'  where  CurrencyID ='{0}'", ID), txtName.Text.Trim().ToUpperInvariant(), txtCode.Text.Trim().ToUpperInvariant(), chkCurrency.Checked), db, transaction))
                                {
                                    sqlCommand1.ExecuteNonQuery();
                                }

                                //([Rate],[CurrencyID]
                                using (SqlCommand sqlCommand = new SqlCommand(String.Format(String.Format("UPDATE [tblExchangeRate] SET [Rate]='{{0}}' where  CurrencyID ='{0}'", ID), Convert.ToDouble(txtRate.Text.Trim())), db, transaction))
                                {
                                    sqlCommand.ExecuteNonQuery();
                                }



                                transaction.Commit();
                            }
                            catch (SqlException sqlError)
                            {
                                transaction.Rollback();
                            }
                            db.Close();
                        }

                        setReload();
                        Common.setMessageBox("Changes in record has been successfully saved.", Program.ApplicationName, 1);
                        bttnReset.PerformClick();

                    }
                }
            }
            catch (Exception ex)
            {
                Tripous.Sys.ErrorBox(ex);
            }

        }

        private void setReload()
        {
            //connect.connect.Close();
            DataTable dt;
            //System.Data.DataSet ds;
            using (var ds = new System.Data.DataSet())
            {
                //connect.connect.Open();
                using (SqlDataAdapter ada = new SqlDataAdapter("select CurrencyID, CurrencyName,CurrencyCode,Exchange  from ViewCurrencyExchange", Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }
                //connect.connect.Close();

                dt = ds.Tables[0];

                gridControl1.DataSource = dt.DefaultView;
            }
            gridView3.OptionsBehavior.Editable = false;
            gridView3.Columns["CurrencyID"].Visible = false;
            gridView3.BestFitColumns();
        }

    }
}
