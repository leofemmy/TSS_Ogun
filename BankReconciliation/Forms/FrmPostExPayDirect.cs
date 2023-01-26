using BankReconciliation.Class;
using DevExpress.XtraGrid.Views.Grid;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Windows.Forms;
using TaxSmartSuite.Class;

namespace BankReconciliation.Forms
{
    public partial class FrmPostExPayDirect : Form
    {

        public static FrmPostExPayDirect publicStreetGroup;

        protected TransactionTypeCode iTransType;

        protected bool boolIsUpdate;

        protected string ID;

        string receiptno, payref;

        bool isFirst = true;

        bool isSecond = true;

        int Mth, Years;

        string[] split;

        public FrmPostExPayDirect()
        {
            InitializeComponent();

            setImages();

            ToolStripEvent();

            iTransType = TransactionTypeCode.Null;

            Load += OnFormLoad;

            OnFormLoad(null, null);

            groupControl2.Enabled = false;
        }

        private void setImages()
        {
            tsbNew.Image = MDIMains.publicMDIParent.i16x16.Images[5];
            tsbEdit.Image = MDIMains.publicMDIParent.i16x16.Images[6];
            tsbDelete.Image = MDIMains.publicMDIParent.i16x16.Images[7];
            tsbReload.Image = MDIMains.publicMDIParent.i16x16.Images[0];
            tsbClose.Image = MDIMains.publicMDIParent.i16x16.Images[11];

            bttnEdit.Image = MDIMains.publicMDIParent.i32x32.Images[39];
            //bttnLoad.Image = MDIMains.publicMDIParent.i32x32.Images[39];
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
                cboPosting.Text = "Non-REEMS Collections";
                iTransType = TransactionTypeCode.New;
                ShowForm();
                //boolIsUpdate = false;
                groupControl2.Enabled = true;
            }
            else if (sender == tsbEdit)
            {
                groupControl2.Text = "Edit Record Mode";

                iTransType = TransactionTypeCode.Edit;
                //if (EditRecordMode())
                //{
                ShowForm();
                //Unlockfield();
                //    boolIsUpdate = true;
                //}
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
                //ShowForm();
            }
            //bttnReset.PerformClick();
        }

        void OnFormLoad(object sender, EventArgs e)
        {
            ShowForm();

            setDBComboBox();

            isFirst = false;

            isSecond = false;

            setDBComboBoxRevenue();

            cboBank.KeyPress += cboBank_KeyPress;

            cboBranch.KeyPress += cboBranch_KeyPress;

            cboPosting.KeyPress += cboPosting_KeyPress;

            cboAcct.KeyPress += cboAcct_KeyPress;

            cboRevenue.KeyPress += cboRevenue_KeyPress;

            cboBank.SelectedIndexChanged += cboBank_SelectedIndexChanged;
            ////setReload();

            cboBranch.SelectedIndexChanged += cboBranch_SelectedIndexChanged;

            cboBank_SelectedIndexChanged(null, null);

            cboBranch_SelectedIndexChanged(null, null);

            txtAmount.LostFocus += txtAmount_LostFocus;

            txtDescription.Leave += txtDescription_Leave;

            bttnUpdate.Click += bttnUpdate_Click;

            bttnEdit.Click += bttnEdit_Click;

            gridView1.DoubleClick += new EventHandler(gridView1_DoubleClick);
        }

        void gridView1_DoubleClick(object sender, EventArgs e)
        {

            if (EditRecordMode())
            {
                iTransType = TransactionTypeCode.New;
                cboPosting.Text = "Non-REEMS Collections";
                ShowForm();
                groupControl2.Enabled = true;
                boolIsUpdate = true;
            }
        }

        void bttnEdit_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtEPeriod2.Text))
            {
                Common.setEmptyField("Transaction Period", Program.ApplicationName);
                txtEPeriod2.Focus(); return;
            }
            else
            {

                split = txtEPeriod2.EditValue.ToString().Split(new Char[] { '/' });
                Mth = Convert.ToInt32(split[0]);
                Years = Convert.ToInt32(split[1]);

                setReload(Mth, Years);
            }
        }

        void bttnUpdate_Click(object sender, EventArgs e)
        {
            upDateRecord();
        }

        void txtDescription_Leave(object sender, EventArgs e)
        {
            txtDescription.Text = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(txtDescription.Text);
        }

        void txtAmount_LostFocus(object sender, EventArgs e)
        {
            txtAmount.Text = Convert.ToDecimal(txtAmount.Text).ToString("###,###,###.00");
        }

        void cboRevenue_KeyPress(object sender, KeyPressEventArgs e)
        {
            Methods.AutoComplete(cboRevenue, e, true);
        }

        void cboAcct_KeyPress(object sender, KeyPressEventArgs e)
        {
            Methods.AutoComplete(cboAcct, e, true);
        }

        void cboBranch_KeyPress(object sender, KeyPressEventArgs e)
        {
            Methods.AutoComplete(cboBranch, e, true);
        }

        void cboBank_KeyPress(object sender, KeyPressEventArgs e)
        {
            Methods.AutoComplete(cboBank, e, true);
        }

        void cboPosting_KeyPress(object sender, KeyPressEventArgs e)
        {
            Methods.AutoComplete(cboPosting, e, true);
        }

        protected void ShowForm()
        {
            switch (iTransType)
            {
                case TransactionTypeCode.Null:
                    splitContainer1.Panel1Collapsed = false;
                    splitContainer1.Panel2Collapsed = true;
                    break;
                case TransactionTypeCode.New:
                    splitContainer1.Panel1Collapsed = false;
                    splitContainer1.Panel2Collapsed = true;
                    //Lockfield();
                    break;
                case TransactionTypeCode.Edit:
                    splitContainer1.Panel1Collapsed = true;
                    splitContainer1.Panel2Collapsed = false;
                    //Lockfield();
                    break;
                case TransactionTypeCode.Delete:
                    splitContainer1.Panel1Collapsed = true;
                    splitContainer1.Panel2Collapsed = false;
                    break;
                case TransactionTypeCode.Reload:
                    splitContainer1.Panel1Collapsed = false;
                    splitContainer1.Panel2Collapsed = false;
                    break;
                default:
                    splitContainer1.Panel1Collapsed = true;
                    splitContainer1.Panel2Collapsed = false;
                    break;
            }
        }

        public void setDBComboBox()
        {
            DataTable Dt;

            using (var ds = new System.Data.DataSet())
            {
                string query = "SELECT BankName,BankShortCode FROM dbo.tblBank";
                using (SqlDataAdapter ada = new SqlDataAdapter(query, Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }

                Dt = ds.Tables[0];
            }

            Common.setComboList(cboBank, Dt, "BankShortCode", "BankName");

            cboBank.SelectedIndex = -1;

        }

        void setDBComboBoxBranch(string Parameter)
        {
            DataTable Dt;

            using (var ds = new System.Data.DataSet())
            {
                string query = String.Format("SELECT BranchName,BranchID FROM dbo.tblBankBranch WHERE BankShortCode ='{0}'", Parameter);

                using (SqlDataAdapter ada = new SqlDataAdapter(query, Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }

                Dt = ds.Tables[0];
            }

            Common.setComboList(cboBranch, Dt, "BranchID", "BranchName");

            cboBranch.SelectedIndex = -1;
        }

        void setDBComboBoxAcct(int Param)
        {
            DataTable Dt;

            using (var ds = new System.Data.DataSet())
            {
                string query = String.Format("SELECT AccountNumber,BankAccountID FROM dbo.tblBankAccount WHERE BranchID ='{0}'", Param);

                using (SqlDataAdapter ada = new SqlDataAdapter(query, Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }

                Dt = ds.Tables[0];
            }

            Common.setComboList(cboAcct, Dt, "BankAccountID", "AccountNumber");

            cboAcct.SelectedIndex = -1;

        }

        void cboBranch_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboBranch.SelectedValue != null && !isSecond)
            {
                setDBComboBoxAcct(Convert.ToInt32(cboBranch.SelectedValue));
                //cboBranch.SelectedIndex = -1;
            }
        }

        void cboBank_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboBank.SelectedValue != null && !isFirst)
            {
                setDBComboBoxBranch(cboBank.SelectedValue.ToString());
                //cboBank.SelectedIndex = -1;
            }
        }

        void setDBComboBoxRevenue()
        {
            DataTable Dt;

            using (var ds = new System.Data.DataSet())
            {
                string query = "SELECT Description,RevenueCode FROM dbo.tblRevenueType";
                using (SqlDataAdapter ada = new SqlDataAdapter(query, Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }

                Dt = ds.Tables[0];
            }

            Common.setComboList(cboRevenue, Dt, "RevenueCode", "Description");

            cboRevenue.SelectedIndex = -1;
        }

        void upDateRecord()
        {
            //    string count;
            //    count.PadLeft(7, '0');
            if (txtEPeriod.EditValue == null || txtEPeriod.EditValue == "")
            {
                Common.setEmptyField("Transaction Period", Program.ApplicationName);
                txtEPeriod.Focus(); return;
            }
            else
            {
                split = txtEPeriod.EditValue.ToString().Split(new Char[] { '/' });

                if (dtpDate.Value.Month != Convert.ToInt32(split[0]) || dtpDate.Value.Year != Convert.ToInt32(split[1]))
                {
                    Common.setMessageBox("Invalid Date, Date not in the Range of Transaction Period", Program.ApplicationName, 3);
                    return;
                }
                else if (String.IsNullOrEmpty(cboBank.Text))
                {
                    Common.setEmptyField("Bank Name", Program.ApplicationName);
                    cboBank.Focus(); return;
                }
                else if (String.IsNullOrEmpty(cboBranch.Text))
                {
                    Common.setEmptyField("Bank Branch Name", Program.ApplicationName);
                    cboBranch.Focus(); return;
                }
                else if (string.IsNullOrEmpty(cboAcct.Text))
                {
                    Common.setEmptyField("Account Number", Program.ApplicationName);
                    cboAcct.Focus(); return;
                }
                else if (string.IsNullOrEmpty(txtDescription.Text))
                {
                    Common.setEmptyField("Description", Program.ApplicationName);
                    txtDescription.Focus(); return;
                }
                else if (string.IsNullOrEmpty(txtAmount.Text))
                {
                    Common.setEmptyField("Amount", Program.ApplicationName);
                    txtAmount.Focus(); return;
                }
                else if (string.IsNullOrEmpty(cboRevenue.Text))
                {
                    Common.setEmptyField("Revenue Type", Program.ApplicationName);
                    cboRevenue.Focus(); return;
                }
                else
                {
                    //get Agency name and code using revenue code
                    DataTable dst = GetReceiptNumber.GetAgencyReveue(cboRevenue.SelectedValue.ToString());

                    //check record mode
                    if (!boolIsUpdate)
                    {

                        //Get Receipt Number
                        receiptno = GetReceiptNumber.ReceiptNo();

                        //payment Ref. number
                        payref = String.Format("ICM|{0}|{1}|{2:dd-MM-yyyy}|{3}", cboBank.SelectedValue, Program.StatePayCode, DateTime.Now, Convert.ToString(receiptno).PadLeft(7, '0'));

                        using (SqlConnection db = new SqlConnection(Logic.ConnectionString))
                        {
                            SqlTransaction transaction;

                            db.Open();

                            transaction = db.BeginTransaction();
                            try
                            {

                                using (SqlCommand sqlCommand1 = new SqlCommand(String.Format("INSERT INTO [tblCollectionReport]([Provider],[Channel],[PaymentRefNumber],[PaymentDate],[PayerName],[RevenueCode],[Description],[Amount],[AgencyName],[AgencyCode],[BankCode],[BankName],[BranchCode],[BranchName],[ReceiptNo],[State]) VALUES ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}');", "ICM", "Bank", payref, DateTime.Now.ToString("yyyy-MM-dd"), txtDescription.Text.Trim(), cboRevenue.SelectedValue.ToString(), cboRevenue.Text.Trim(), txtAmount.Text.Trim(), dst.Rows[0][0].ToString(), dst.Rows[0][1].ToString(), cboBank.SelectedValue.ToString(), cboBank.Text.Trim(), cboBranch.SelectedValue.ToString(), cboBranch.Text.Trim(), receiptno, Program.StateName), db, transaction))
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
                                using (SqlCommand sqlCommand1 = new SqlCommand(String.Format(String.Format("UPDATE [tblCollectionReport] SET [Provider]='{{0}}',[Channel]='{{1}}',[PaymentRefNumber]='{{2}}',[PaymentDate]='{{3}}',[PayerName]='{{4}}',[RevenueCode]='{{5}}',[Description]='{{6}}',[Amount]='{{7}}',[AgencyName]='{{8}}',[AgencyCode]='{{9}}',[BankCode]='{{10}}',[BankName]='{{11}}',[BranchCode]='{{12}}',[BranchName]='{{13}}',[ReceiptNo]='{{14}}',[State]='{{15}}' where  [PaymentRefNumber] ='{0}'", ID), "ICM", "Bank", payref, DateTime.Now.ToString("yyyy-MM-dd"), txtDescription.Text.Trim(), cboRevenue.SelectedValue.ToString(), cboRevenue.Text.Trim(), txtAmount.Text.Trim(), dst.Rows[0][0].ToString(), dst.Rows[0][1].ToString(), cboBank.SelectedValue.ToString(), cboBank.Text.Trim(), cboBranch.SelectedValue.ToString(), cboBranch.Text.Trim(), receiptno, Program.StateName), db, transaction))
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
                    }






                    //dst.Rows[0][0].ToString();


                    Common.setMessageBox("Record Update", Program.ApplicationName, 1);
                    //       recep = "ICM" + "|" + bankCode + "|" + Program.StatePayCode
                    //+ "|" + DateTime.Now.ToString("dd-MM-yyyy") + "|" + Convert.ToString(countrec).PadLeft(7, '0');
                }
            }
            //else
            //{ 
            //string counts ="9867";
            //    MessageBox.Show(counts.PadLeft(7,'0'),Program.ApplicationName);

            //}
        }

        private void setReload(int Months, int years)
        {
            DataTable dt;

            using (var ds = new System.Data.DataSet())
            {
                string query = String.Format("SELECT paymentdate as Date,Paymentrefnumber,Amount,Bankname,Branchname ,Bankcode,Branchcode FROM tblcollectionreport WHERE PROVIDER='ICM'AND YEAR(paymentdate)='{0}' AND MONTH(paymentdate) ='{1}' ", years, Months);

                using (SqlDataAdapter ada = new SqlDataAdapter(query, Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }

                dt = ds.Tables[0];
                gridControl1.DataSource = dt.DefaultView;
            }
            gridView1.OptionsBehavior.Editable = false;
            gridView1.Columns["Branchcode"].Visible = false;
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
                    ID = dr["Paymentrefnumber"].ToString();

                    bResponse = FillField(dr["Paymentrefnumber"].ToString());
                }
                else
                {
                    Common.setMessageBox("No Record is seleected", "", 1);
                    bResponse = false;
                }
            }
            return bResponse;
        }

        private bool FillField(string fieldid)
        {
            bool bResponse = false;

            DataTable dts = (new Logic()).getSqlStatement((String.Format("SELECT * from tblcollectionreport where paymentrefnumber ='{0}'", fieldid))).Tables[0];

            if (dts != null && dts.Rows.Count > 0)
            {
                bResponse = true;

                cboBank.Text = dts.Rows[0]["BankName"].ToString();
                cboBranch.Text = dts.Rows[0]["branchname"].ToString();
                txtDescription.Text = dts.Rows[0]["PayerName"].ToString();
                txtAmount.Text = Convert.ToDecimal(dts.Rows[0]["amount"]).ToString("###,###,###.00");
                cboRevenue.Text = dts.Rows[0]["Description"].ToString();
                txtEPeriod.Text = txtEPeriod2.Text.Trim();
                dtpDate.Value = Convert.ToDateTime(dts.Rows[0]["PaymentDate"]);

                receiptno = dts.Rows[0]["ReceiptNo"].ToString();
                payref = fieldid;
            }
            else
                bResponse = false;

            return bResponse;
        }

        public void RefreshForm()
        {
            NavBars.ToolStripEnableDisableControls(toolStrip, Tag as String);
        }


    }
}
