//using Control_Panel.Class;
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
    public partial class FrmAccountBank : Form
    {

        //DBConnection connect = new DBConnection();

        //Methods extMethods = new Methods();

        public static FrmAccountBank publicStreetGroup;

        protected TransactionTypeCode iTransType;

        protected bool boolIsUpdate;

        protected int ID;

        bool isFirst = true;

        public FrmAccountBank()
        {
            SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

            InitializeComponent();

            //connect.ConnectionString();

            publicStreetGroup = this;

            setImages();

            ToolStripEvent();

            iTransType = TransactionTypeCode.Null;

            Load += OnFormLoad;

            gridView1.DoubleClick += gridView1_DoubleClick;

            bttnCancel.Click += Bttn_Click;

            //bttnReset.Click += Bttn_Click;

            bttnUpdate.Click += Bttn_Click;

            OnFormLoad(null, null);

            //txtOpneing.LostFocus += txtOpneing_LostFocus;

            txtOpneing.Leave += txtOpneing_Leave;

            //txtNumber.LostFocus += txtNumber_LostFocus;

            txtNumber.Leave += txtNumber_Leave;

            //txtName.LostFocus += txtName_LostFocus;

            txtName.Leave += txtName_Leave;

            SplashScreenManager.CloseForm(false);
        }

        void txtOpneing_Leave(object sender, EventArgs e)
        {
            if (!Logic.isDeceimalFormat((string)txtOpneing.Text))
            {
                Common.setMessageBox("Opening Balance can only be in number values", Program.ApplicationName, 2); txtOpneing.Text = string.Empty;
                //txtNumber.Focus(); 
                return;
            }
            else
            {
                String Text = ((TextBox)sender).Text.Replace(",", "");

                double Num;

                if (double.TryParse(Text, out Num))
                {
                    Text = String.Format("{0:N2}", Num);
                    ((TextBox)sender).Text = Text;
                }
            }
        }

        void txtNumber_Leave(object sender, EventArgs e)
        {
            if (!Logic.isAlphaNumeric((string)txtNumber.Text))
            {
                Common.setMessageBox("Account Number can only be in number values", Program.ApplicationName, 2); txtNumber.Text = string.Empty;
                //txtNumber.Focus(); 
                return;
            }
        }

        void txtName_Leave(object sender, EventArgs e)
        {
            var cultureInfo = System.Threading.Thread.CurrentThread.CurrentCulture;
            //return cultureInfo.TextInfo.ToTitleCase(str.ToLower());

            if (!Logic.IsAlphabte((string)txtName.Text))
            {
                Common.setMessageBox("Account Name can only be Character", Program.ApplicationName, 2);
                txtName.Text = string.Empty;
                //txtName.Focus();
                return;
            }
            else
            { txtName.Text = cultureInfo.TextInfo.ToTitleCase(txtName.Text); }
        }

        void txtName_LostFocus(object sender, EventArgs e)
        {
            var cultureInfo = System.Threading.Thread.CurrentThread.CurrentCulture;
            //return cultureInfo.TextInfo.ToTitleCase(str.ToLower());

            if (!Logic.IsAlphabte((string)txtName.Text))
            {
                Common.setMessageBox("Account Name can only be Character", Program.ApplicationName, 2);
                txtName.Text = string.Empty;
                //txtName.Focus();
                return;
            }
            else
            { txtName.Text = cultureInfo.TextInfo.ToTitleCase(txtName.Text); }

        }

        void txtNumber_LostFocus(object sender, EventArgs e)
        {
            if (!Logic.IsNumber((string)txtNumber.Text))
            {
                Common.setMessageBox("Account Number can only be in number values", Program.ApplicationName, 2); txtNumber.Text = string.Empty;
                //txtNumber.Focus(); 
                return;
            }
        }

        void txtOpneing_LostFocus(object sender, EventArgs e)
        {

            if (!Logic.isDeceimalFormat((string)txtOpneing.Text))
            {
                Common.setMessageBox("Opening Balance can only be in number values", Program.ApplicationName, 2); txtOpneing.Text = string.Empty;
                //txtNumber.Focus(); 
                return;
            }
            else
            {
                String Text = ((TextBox)sender).Text.Replace(",", "");

                double Num;

                if (double.TryParse(Text, out Num))
                {
                    Text = String.Format("{0:N2}", Num);
                    ((TextBox)sender).Text = Text;
                }
            }
            //String Text = ((TextBox)sender).Text.Replace(",", "");

            //double Num;

            //if (double.TryParse(Text, out Num))
            //{
            //    Text = String.Format("{0:N2}", Num);
            //    ((TextBox)sender).Text = Text;
            //}
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
                groupControl2.Text = "Disable Record Mode";
                iTransType = TransactionTypeCode.Delete;
                if (MosesClassLibrary.Utilities.Common.AskQuestion("Disable this record will delete attached record.\nDo you want to continue?", ""))
                {
                    if (string.IsNullOrEmpty(ID.ToString()))
                    {
                        Common.setMessageBox("No Record Selected for Disable", Program.ApplicationName, 3);
                        return;
                    }
                    else
                        deleteRecord(ID);
                }
                else
                    tsbReload.PerformClick();
                boolIsUpdate = false;
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
            setDBComboBox(); setDbComboBoxCurrency();
            //setDBComboBoxTn();
            isFirst = false;
            setReload();
            cboBank.KeyPress += cboBank_KeyPress;
            cboBranch.KeyPress += cboBranch_KeyPress; cboCurrency.KeyPress += cboCurrency_KeyPress;
            cboBank.SelectedIndexChanged += cboBank_SelectedIndexChanged;
            cboBank_SelectedIndexChanged(null, null);
            Clear();
        }

        void cboCurrency_KeyPress(object sender, KeyPressEventArgs e)
        {
            Methods.AutoComplete(cboCurrency, e, true);
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

        void gridView1_DoubleClick(object sender, EventArgs e)
        {
            tsbEdit.PerformClick();
        }

        private void setReload()
        {
            //connect.connect.Close();
            DataTable dt;
            //System.Data.DataSet ds;
            Clear();

            using (var ds = new System.Data.DataSet())
            {
                //connect.connect.Open();
                using (SqlDataAdapter ada = new SqlDataAdapter("select * from ViewBankBranchAccount ORDER BY BankShortCode", Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }
                //connect.connect.Close();
                dt = ds.Tables[0];
                gridControl1.DataSource = dt.DefaultView;
            }
            gridView1.OptionsBehavior.Editable = false;
            gridView1.Columns["OpenBal"].DisplayFormat.FormatType = FormatType.Numeric;
            gridView1.Columns["OpenBal"].DisplayFormat.FormatString = "n2";
            gridView1.Columns["BankAccountID"].Visible = false;
            gridView1.Columns["AccountName"].Visible = false;
            gridView1.Columns["Status"].Visible = false;
            gridView1.Columns["BranchName"].Visible = false;
            gridView1.Columns["BranchCode"].Visible = false;
            gridView1.Columns["BankShortCode"].Visible = false;
            gridView1.Columns["CurrencyID"].Visible = false;
            //gridView1.Columns["Description"].Visible = false;

            //Status
            gridView1.BestFitColumns();
        }

        void Bttn_Click(object sender, EventArgs e)
        {
            if (sender == bttnUpdate)
            {
                UpdateRecord();
            }
            else if (sender == bttnCancel)
            {
                tsbReload.PerformClick();
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

        private bool FillField(int fieldid)
        {
            bool bResponse = false;
            //load data from the table into the forms for edit
            //DataTable dts = extMethods.LoadData(String.Format("select * from ViewBankBranchAccount where BankAccountID ='{0}'", fieldid));

            DataTable dts = (new Logic()).getSqlStatement((String.Format("select * from ViewBankBranchAccount where BankAccountID ='{0}'", fieldid))).Tables[0];

            if (dts != null)
            {
                bResponse = true;

                txtName.Text = dts.Rows[0]["AccountName"].ToString();
                txtNumber.Text = dts.Rows[0]["AccountNumber"].ToString();
                cboBank.Text = dts.Rows[0]["BankName"].ToString();
                cboBranch.Text = dts.Rows[0]["BranchName"].ToString();
                cboCurrency.Text = dts.Rows[0]["Description"].ToString();
                //cboBranch.Text
                txtOpneing.Text = String.Format("{0:N2}", dts.Rows[0]["OpenBal"]);
                //chklink.CheckState = Convert.ToBoolean(dts.Rows[0]["IsLink"]);


                if (dts.Rows[0]["IsLink"] is DBNull)
                {

                }
                else
                {
                    if (Convert.ToBoolean(dts.Rows[0]["IsLink"]))
                    {
                        chklink.Checked = true;
                    }
                    else
                    {
                        chklink.Checked = false;
                    }
                }

                if (dts.Rows[0]["IsCommercial"] is DBNull)
                { }
                else
                { radioGroup1.EditValue = Convert.ToBoolean(dts.Rows[0]["IsCommercial"]); }



                //chkActive.CheckState = dts.Rows[0]["Status"].ToString();


            }
            else
                bResponse = false;

            return bResponse;
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
                    ID = Convert.ToInt32(dr["BankAccountID"]);
                    bResponse = FillField(Convert.ToInt32(dr["BankAccountID"]));
                }
                else
                {
                    Common.setMessageBox("No Record is seleected", "", 1);
                    bResponse = false;
                }
            }
            return bResponse;
        }

        public void RefreshForm()
        {
            NavBars.ToolStripEnableDisableControls(toolStrip, Tag as String);
        }

        private void UpdateRecord()
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

                if (String.Compare(txtName.Text, "", false) == 0)
                {
                    Common.setEmptyField("Account Name", Program.ApplicationName);
                    txtName.Focus(); return;
                }
                else if (String.Compare(cboBank.Text, "", false) == 0)
                {
                    Common.setEmptyField("Bank Name", Program.ApplicationName);
                    cboBank.Focus(); return;
                }
                else if (String.Compare(cboCurrency.Text, "", false) == 0)
                {
                    Common.setEmptyField("Currency", Program.ApplicationName);
                    cboCurrency.Focus(); return;
                }
                else if (String.Compare(cboBranch.Text, "", false) == 0)
                {
                    Common.setEmptyField("Branch Name", Program.ApplicationName);
                    cboBranch.Focus(); return;
                }
                else if (String.Compare(txtNumber.Text, "", false) == 0)
                {
                    Common.setEmptyField("Account Number", Program.ApplicationName);
                    txtNumber.Focus(); return;
                }
                else if (string.Compare(txtOpneing.Text, "", false) == 0)
                {
                    Common.setEmptyField("Account Opening Balance", Program.ApplicationName);
                    txtOpneing.Focus(); return;
                }
                else if (radioGroup1.SelectedIndex == -1)
                {
                    Common.setEmptyField("Select Bank Operation Type", Program.ApplicationName);
                    return;
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
                                //radioGroup1.EditValue
                                string query = String.Format("INSERT INTO Reconciliation.tblBankAccount([AccountName],[AccountNumber],[BranchCode],[IsActive],[OpenBal],BankShortCode,CurrencyID,IsLink,IsCommercial) VALUES ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}');", txtName.Text.Trim(), txtNumber.Text.Trim(), cboBranch.SelectedValue.ToString(), chkActive.Checked, Convert.ToDouble(txtOpneing.Text), cboBank.SelectedValue.ToString(), Convert.ToInt32(cboCurrency.SelectedValue), chklink.Checked, radioGroup1.EditValue);

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
                            //bttnReset.PerformClick();
                            setReload(); Clear(); cboBank.Focus();
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
                                string dry = String.Format(String.Format("UPDATE Reconciliation.tblBankAccount SET [AccountName]='{{0}}',[AccountNumber]='{{1}}',[IsActive] ='{{2}}',OpenBal='{{3}}',CurrencyID='{{4}}',IsLink='{{5}}',IsCommercial='{{6}}' where  BankAccountID ='{0}'", ID), txtName.Text.Trim(), txtNumber.Text.Trim(), chkActive.Checked, Convert.ToDouble(txtOpneing.Text), Convert.ToInt32(cboCurrency.SelectedValue), chklink.Checked, radioGroup1.EditValue);

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
                        //bttnReset.PerformClick();
                        tsbReload.PerformClick();

                    }
                }
            }
            catch (Exception ex)
            {
                Tripous.Sys.ErrorBox(ex);
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

        void cboBranch_KeyPress(object sender, KeyPressEventArgs e)
        {
            Methods.AutoComplete(cboBranch, e, true);
        }

        void cboBank_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboBank.SelectedValue != null && !isFirst)
            {
                //setReload(Convert.ToInt32(cboBank.SelectedValue.ToString()));
                setDBComboBoxBranch(cboBank.SelectedValue.ToString());
                cboBranch.SelectedIndex = -1;

            }
        }

        public void setDBComboBoxBranch(string parameter)
        {
            DataTable Dt;

            //connect.connect.Close();

            using (var ds = new System.Data.DataSet())
            {
                using (SqlDataAdapter ada = new SqlDataAdapter((string)String.Format("SELECT  BranchCode,BranchName FROM Collection.tblBankBranch WHERE BankShortCode = '{0}'", parameter), Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }

                Dt = ds.Tables[0];
            }

            Common.setComboList(cboBranch, Dt, "BranchCode", "BranchName");

            cboBranch.SelectedIndex = -1;


        }

        private void Clear()
        {
            //txtStreetGroup.Clear();
            txtName.Clear();
            txtNumber.Clear();
            txtOpneing.Clear();
            setDBComboBox(); setDbComboBoxCurrency(); radioGroup1.SelectedIndex = -1;


        }

        void deleteRecord(int parameter2)
        {
            //try
            //{
            using (SqlConnection db = new SqlConnection(Logic.ConnectionString))
            {
                SqlTransaction transaction;

                db.Open();

                transaction = db.BeginTransaction();

                try
                {
                    using (SqlCommand sqlCommand1 = new SqlCommand(String.Format("update Reconciliation.tblBankAccount SET IsActive=0 WHERE BankAccountID='{0}'", parameter2), db, transaction))
                    {
                        sqlCommand1.ExecuteNonQuery();
                    }

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Tripous.Sys.ErrorBox(ex); return;
                }
                Common.setMessageBox("Record Disable Successfully ", Program.ApplicationName, 3);
                db.Close();
            }
        }

    }
}
