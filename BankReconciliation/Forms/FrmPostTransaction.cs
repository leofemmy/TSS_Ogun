using BankReconciliation.Class;
using BankReconciliation.Report;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraReports.UI;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Windows.Forms;
using TaxSmartSuite.Class;

namespace BankReconciliation.Forms
{
    public partial class FrmPostTransaction : Form
    {
        private SqlCommand _command;

        private SqlDataAdapter adp;
        private DataTable Dts;
        public static FrmPostTransaction publicStreetGroup;

        protected TransactionTypeCode iTransType;

        protected bool boolIsUpdate;

        protected int ID;

        bool isFirst = true;

        private string querys;

        string[] split;

        string[] Split2;

        double openbalAcct;

        string strmonth;
        string monthName;

        DataTable tableTrans = new DataTable();

        DataTable dt, dt1;

        DataTable dtEdit;

        System.Globalization.CultureInfo enGB = new System.Globalization.CultureInfo("en-GB");

        public FrmPostTransaction()
        {
            InitializeComponent();

            publicStreetGroup = this;

            setImages();

            ToolStripEvent();

            iTransType = TransactionTypeCode.Null;

            Load += OnFormLoad;

            bttnPost.Click += Bttn_Click;

            bttnUpdate.Click += Bttn_Click;

            bttnCancel.Click += Bttn_Click;

            bttnReport.Click += Bttn_Click;

            txtDateE.LostFocus += txtDateE_LostFocus;

            txtCBal.LostFocus += txtCBal_LostFocus;

            txtAmount.LostFocus += txtAmount_LostFocus;

            OnFormLoad(null, null);

            //create offline table
            tableTrans.Columns.Add("Date", typeof(string));
            tableTrans.Columns.Add("Transaction Description", typeof(string));
            tableTrans.Columns.Add("Dr", typeof(Decimal));
            tableTrans.Columns.Add("Cr", typeof(Decimal));

            gridControl1.DataSource = tableTrans;
            //this.Controls.Add(gridControl1);
            gridControl1.BringToFront();
            gridView1.OptionsBehavior.Editable = false;
            gridView1.BestFitColumns();

            //spECAmount.Properties.MinValue = decimal.Zero;
            //spECAmount.Properties.MaxValue = decimal.MaxValue;

            //spEAmount.Properties.MinValue = decimal.Zero;
            //spEAmount.Properties.MaxValue = decimal.MaxValue;
            //gridView1.Columns["Dr"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            //gridView1.Columns["Dr"].SummaryItem.DisplayFormat = "n2";

            gridView3.DoubleClick += gridView3_DoubleClick;
        }

        void gridView3_DoubleClick(object sender, EventArgs e)
        {
            tsbEdit.PerformClick();

        }

        void txtAmount_LostFocus(object sender, EventArgs e)
        {
            String Text = ((TextBox)sender).Text.Replace(",", "");

            int Num;

            if (int.TryParse(Text, out Num))
            {
                Text = String.Format("{0:N2}", Num);
                ((TextBox)sender).Text = Text;
            }
        }

        void txtCBal_LostFocus(object sender, EventArgs e)
        {

            String Text = ((TextBox)sender).Text.Replace(",", "");

            int Num;

            if (int.TryParse(Text, out Num))
            {
                Text = String.Format("{0:N2}", Num);
                ((TextBox)sender).Text = Text;
            }

            //txtCBal.Text = string.Format("{0:N2}",txtCBal.Text);
        }

        void txtDateE_LostFocus(object sender, EventArgs e)
        {
            if (txtPeriod.Text == null || txtPeriod.Text == "")
            {
                Common.setEmptyField("Transaction Period", Program.ApplicationName);
                txtPeriod.Focus(); return;
            }
            else
            {
                split = txtPeriod.Text.Trim().Split(new Char[] { '/' });

                Split2 = txtDateE.EditValue.ToString().Split(new Char[] { '-' });

                if (Convert.ToInt32(split[0]) != Convert.ToInt32(Split2[1]) || Convert.ToInt32(split[1]) != Convert.ToInt32(Split2[2]))
                {
                    Common.setMessageBox(" Date Not Withing the Transaction Period ", Program.ApplicationName, 1);
                    txtDateE.EditValue = ""; txtDateE.Focus();
                    return;
                }
            }

        }

        void Bttn_Click(object sender, EventArgs e)
        {

            if (sender == bttnPost)
            {
                UpdateRecord();
            }
            else if (sender == bttnCancel)
            {
                tsbReload.PerformClick();
            }
            else if (sender == bttnUpdate)
            {
                InsertUpdate();
            }
            else if (sender == bttnReport)
            {
                strmonth = txtPeriod.Text.Substring(0, 2);
                //string fullmonth = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(strmonth); ;
                split = txtPeriod.Text.Trim().Split(new Char[] { '/' });
                var dtf = CultureInfo.CurrentCulture.DateTimeFormat;
                //string monthName = dtf.GetMonthName(strmonth);
                var month = Convert.ToInt32(txtPeriod.Text.Substring(0, 2));
                var fyear = txtPeriod.Text.Length;
                monthName = dtf.GetMonthName(month);
                xrTranAccount report = new xrTranAccount();

                string fulltext = String.Format("Collection Reconciliation for : [{0} {1}]", monthName, Convert.ToInt32(split[1]));

                report.paramAccount.Value = cboAcct.Text;

                report.paramPeriod.Value = txtPeriod.Text;

                report.xrLabel1.Text = fulltext;
                report.xrLabel18.Text = String.Format("{0} - {1}", lblBank.Text, lblbranch.Text);

                report.ShowPreviewDialog();
            }
        }

        void UpdateRecord()
        {
            if (radioGroup1.EditValue == "DR")
            {
                if (!boolIsUpdate)
                {

                    tableTrans.Rows.Add(new object[] { txtDateE.Text, cboTransType.Text, txtAmount.Text, 0 });
                }
                else
                {
                    dtEdit.Rows.Add(new object[] { txtDateE.Text, cboTransType.Text, txtAmount.Text, 0 });
                }

            }
            if (radioGroup1.EditValue == "CR")
            {
                if (!boolIsUpdate)
                {

                    tableTrans.Rows.Add(new object[] { txtDateE.Text, cboTransType.Text, 0, txtAmount.Text });
                }
                else
                {
                    dtEdit.Rows.Add(new object[] { txtDateE.Text, cboTransType.Text, 0, txtAmount.Text });
                }


            }
            Clear(); txtDateE.Focus();
        }

        private void setImages()
        {
            tsbNew.Image = MDIMains.publicMDIParent.i16x16.Images[5];
            tsbEdit.Image = MDIMains.publicMDIParent.i16x16.Images[6];
            tsbDelete.Image = MDIMains.publicMDIParent.i16x16.Images[7];
            tsbReload.Image = MDIMains.publicMDIParent.i16x16.Images[0];
            tsbClose.Image = MDIMains.publicMDIParent.i16x16.Images[11];

            bttnCancel.Image = MDIMains.publicMDIParent.i32x32.Images[9];
            bttnUpdate.Image = MDIMains.publicMDIParent.i32x32.Images[7];
            ////bttnReset.Image = MDIMains.publicMDIParent.i32x32.Images[8];
            bttnPost.Image = MDIMains.publicMDIParent.i32x32.Images[34];

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
                //Clear();
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
            //bttnReset.PerformClick();
        }

        void OnFormLoad(object sender, EventArgs e)
        {
            ShowForm();

            setDBComboBox();

            isFirst = false;

            setReload();
            //cboBank.KeyPress += cboBank_KeyPress;
            //cboBranch.KeyPress += cboBranch_KeyPress;
            //cboBank.SelectedIndexChanged += cboBank_SelectedIndexChanged;
            radioGroup1.SelectedIndexChanged += radioGroup1_SelectedIndexChanged;

            cboAcct.SelectedIndexChanged += cboAcct_SelectedIndexChanged;
            //cboBank_SelectedIndexChanged(null, null);

        }

        void cboAcct_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboAcct.SelectedValue != null && !isFirst)
            {
                GetAcctInfor(cboAcct.SelectedValue.ToString());
            }
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

        public void setDBComboBox()
        {
            DataTable Dt;

            //connect.connect.Close();

            using (var ds = new System.Data.DataSet())
            {
                //connect.connect.Open();
                using (SqlDataAdapter ada = new SqlDataAdapter((string)"SELECT BankAccountID,AccountNumber FROM ViewBankBranchAccount", Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }

                Dt = ds.Tables[0];
            }

            Common.setComboList(cboAcct, Dt, "BankAccountID", "AccountNumber");

            cboAcct.SelectedIndex = -1;


        }

        void GetAcctInfor(string parameter)
        {
            DataTable Dt;

            //connect.connect.Close();

            using (var ds = new System.Data.DataSet())
            {
                //connect.connect.Open();
                string query = String.Format("select BankName,AccountName,BranchName,BankAccountID,OpenBal from ViewBankBranchAccount where BankAccountID = '{0}'", parameter);

                using (SqlDataAdapter ada = new SqlDataAdapter(query, Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }

                Dt = ds.Tables[0];
            }

            if (Dt != null && Dt.Rows.Count > 0)
            {
                lblActDesc.Text = (string)Dt.Rows[0]["AccountName"];

                lblBank.Text = (string)Dt.Rows[0]["BankName"];

                lblbranch.Text = (string)Dt.Rows[0]["BranchName"];

                openbalAcct = Convert.ToDouble(Dt.Rows[0]["OpenBal"]);
            }
        }

        void radioGroup1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (radioGroup1.EditValue == null)
            {
                return;
            }
            else
            {
                groupBox2.Enabled = true;

                setDBComboBoxTrans(radioGroup1.EditValue.ToString());
            }


        }

        public void setDBComboBoxTrans(string parameter)
        {
            DataTable Dt;

            //connect.connect.Close();

            using (var ds = new System.Data.DataSet())
            {
                //connect.connect.Open();
                string query = String.Format("select * from tblTransDefinition where type = '{0}' and ACTION='Active' ", parameter);

                using (SqlDataAdapter ada = new SqlDataAdapter(query, Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }

                Dt = ds.Tables[0];
            }

            Common.setComboList(cboTransType, Dt, "transid", "DESCRIPTION");

            cboTransType.SelectedIndex = -1;


        }

        void Clear()
        {
            txtDateE.Text = ""; txtAmount.Clear();

            cboTransType.SelectedIndex = -1;
        }

        void InsertUpdate()
        {
            if (string.IsNullOrEmpty(cboAcct.SelectedValue.ToString()))
            {
                Common.setEmptyField("Account No.", Program.ApplicationName);
                cboAcct.Focus(); return;
            }
            //00/0000
            else if (string.IsNullOrEmpty(txtPeriod.Text))
            {
                Common.setEmptyField("Transaction Period", Program.ApplicationName);
                txtPeriod.Focus(); return;
            }
            else if (string.IsNullOrEmpty(txtCBal.Text))
            {
                Common.setEmptyField("Closing Balance for Transaction Period", Program.ApplicationName);
                txtCBal.Focus(); return;
            }
            else
            {
                ////get pervison closing balance for new transaction period
                if (!boolIsUpdate)
                {
                    using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
                    {
                        connect.Open();
                        _command = new SqlCommand("InsertUpdatePostRelationTransaction", connect) { CommandType = CommandType.StoredProcedure };
                        _command.Parameters.Add(new SqlParameter("@AccountNo", SqlDbType.VarChar)).Value = cboAcct.Text.Trim();
                        _command.Parameters.Add(new SqlParameter("@Period", SqlDbType.VarChar)).Value = txtPeriod.Text.ToString();
                        _command.Parameters.Add(new SqlParameter("@closeBal", SqlDbType.Money)).Value = Convert.ToDouble(txtCBal.Text.Trim());
                        _command.Parameters.Add(new SqlParameter("@OpenBal", SqlDbType.Money)).Value = Convert.ToDouble(openbalAcct);
                        _command.Parameters.Add(new SqlParameter("@Isbool", SqlDbType.Bit)).Value = boolIsUpdate;
                        _command.Parameters.Add(new SqlParameter("@recEdit", SqlDbType.Int)).Value = 0;
                        _command.Parameters.Add(new SqlParameter("@pTransaction", SqlDbType.Structured)).Value = tableTrans;

                        using (System.Data.DataSet ds = new System.Data.DataSet())
                        {
                            adp = new SqlDataAdapter(_command);
                            adp.Fill(ds);
                            Dts = ds.Tables[0];
                            connect.Close();

                            if (ds.Tables[0].Rows[0]["returnCode"].ToString() != "00")
                            {
                                Common.setMessageBox(ds.Tables[0].Rows[0]["returnMessage"].ToString(), Program.ApplicationName, 3);
                                //isError = true;
                                //goto Map;
                            }
                            else
                            {
                                Common.setMessageBox(ds.Tables[0].Rows[0]["returnMessage"].ToString(), Program.ApplicationName, 3);
                            }

                        }
                    }
                }
                else
                {
                    using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
                    {
                        connect.Open();
                        _command = new SqlCommand("InsertUpdatePostRelationTransaction", connect) { CommandType = CommandType.StoredProcedure };
                        _command.Parameters.Add(new SqlParameter("@AccountNo", SqlDbType.VarChar)).Value = cboAcct.Text.Trim();
                        _command.Parameters.Add(new SqlParameter("@Period", SqlDbType.VarChar)).Value = txtPeriod.Text;
                        _command.Parameters.Add(new SqlParameter("@closeBal", SqlDbType.Money)).Value = Convert.ToDouble(txtCBal.Text.Trim());
                        _command.Parameters.Add(new SqlParameter("@OpenBal", SqlDbType.Money)).Value = Convert.ToDouble(openbalAcct);
                        _command.Parameters.Add(new SqlParameter("@Isbool", SqlDbType.Bit)).Value = boolIsUpdate;
                        _command.Parameters.Add(new SqlParameter("@recEdit", SqlDbType.Int)).Value = ID;
                        _command.Parameters.Add(new SqlParameter("@pTransaction", SqlDbType.Structured)).Value = dtEdit;

                        using (System.Data.DataSet ds = new System.Data.DataSet())
                        {
                            adp = new SqlDataAdapter(_command);
                            adp.Fill(ds);
                            Dts = ds.Tables[0];
                            connect.Close();

                            if (ds.Tables[0].Rows[0]["returnCode"].ToString() != "00")
                            {
                                Common.setMessageBox(ds.Tables[0].Rows[0]["returnMessage"].ToString(), Program.ApplicationName, 3);
                                //isError = true;
                                //goto Map;
                            }
                            else
                            {
                                Common.setMessageBox(ds.Tables[0].Rows[0]["returnMessage"].ToString(), Program.ApplicationName, 3);
                            }

                        }
                    }
                }
                #region oldcode

                //using (var ds = new System.Data.DataSet())
                //{
                //    //connect.connect.Open();

                //    string query = String.Format("select MAX(Period) as Period from tblTransactionPost where AccountNo = '{0}' ", cboAcct.Text);

                //    using (SqlDataAdapter ada = new SqlDataAdapter(query, Logic.ConnectionString))
                //    {
                //        ada.Fill(ds, "table");
                //    }
                //    //connect.connect.Close();
                //    dt = ds.Tables[0];
                //    //gridControl1.DataSource = dt.DefaultView;
                //}

                //if (dt != null && dt.Rows.Count > 0)
                //{
                //    //useing the period to get the close and open bal

                //    using (var dst = new System.Data.DataSet())
                //    {
                //        string query2 = String.Format("select CloseBal,OpenBal from tblTransactionPost where AccountNo = '{0}' and Period= '{1}'", cboAcct.Text, dt.Rows[0]["Period"]);
                //        using (SqlDataAdapter ada1 = new SqlDataAdapter(query2, Logic.ConnectionString))
                //        {
                //            ada1.Fill(dst, "table");
                //        }
                //        //connect.connect.Close();
                //        dt1 = dst.Tables[0];

                //    }
                //    if (dt1 != null && dt1.Rows.Count > 0)
                //    {
                //        if (Convert.ToDouble(dt1.Rows[0]["CloseBal"]) >= Convert.ToDouble(dt1.Rows[0]["OpenBal"]))
                //        {
                //            openbalAcct = Convert.ToDouble(dt1.Rows[0]["CloseBal"]);
                //        }
                //    }
                //}
                //else
                //    openbalAcct = 0.0;


                ////check form status either new or edit
                //if (!boolIsUpdate)
                //{
                //    //save record into transactionpost table
                //    using (SqlConnection db = new SqlConnection(Logic.ConnectionString))
                //    {
                //        SqlTransaction transaction;

                //        db.Open();

                //        transaction = db.BeginTransaction();

                //        string qury = String.Format("INSERT INTO [tblTransactionPost]([AccountNo],[Period],[CloseBal],[OpenBal]) VALUES ('{0}','{1}','{2}','{3}');SELECT @@IDENTITY", cboAcct.Text, txtPeriod.Text.ToString(), txtCBal.Text, openbalAcct);

                //        int recs = Convert.ToInt32(new SqlCommand(qury, db, transaction).ExecuteScalar());

                //        //save record into transaction post relation
                //        //first check the transaction post table if null
                //        if (tableTrans != null && tableTrans.Rows.Count > 0)
                //        {
                //            foreach (DataRow item in tableTrans.Rows)
                //            {
                //                string querys = String.Format("INSERT INTO [tblTransactionPostRelation]([Date],[Description],[Dr] ,[Cr],[PostID])VALUES ('{0}', '{1}', '{2}', '{3}', '{4}');", Convert.ToDateTime(item["Date"], enGB), (object)item["Transaction Description"], (object)item["Dr"], (object)item["Cr"], recs);

                //                using (SqlCommand sqlCommand = new SqlCommand(querys, db, transaction))
                //                {
                //                    sqlCommand.ExecuteNonQuery();
                //                }
                //            }
                //        }

                //        transaction.Commit();

                //        db.Close();

                //    }

                //    setReload();

                //    Common.setMessageBox("Transaction update has been successfully added", Program.ApplicationName, 1);

                //    if (MessageBox.Show("Do you want to add another Account transaction for this period ?", Program.ApplicationName, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
                //    {
                //        tsbReload.PerformClick();
                //    }
                //    else
                //    {

                //        Clear(); cboAcct.SelectedIndex = -1; txtCBal.Clear();

                //        lblActDesc.Text = ""; lblBank.Text = ""; lblbranch.Text = "";

                //        cboAcct.Focus();
                //    }
                //}
                //else//update record
                //{
                //    using (SqlConnection db = new SqlConnection(Logic.ConnectionString))
                //    {
                //        SqlTransaction transaction;

                //        db.Open();

                //        transaction = db.BeginTransaction();

                //        using (SqlCommand sqlCommand1 = new SqlCommand(String.Format(String.Format("UPDATE [tblTransactionPost] SET [CloseBal]='{{0}}'  where  PostID ='{0}'", ID),  txtCBal.Text), db, transaction))
                //        {
                //            sqlCommand1.ExecuteNonQuery();
                //        }

                //        using (SqlCommand sqlCommand2 = new SqlCommand(String.Format("DELETE FROM dbo.tblTransactionPostRelation WHERE PostID='{0}'", ID), db, transaction))
                //        {
                //            sqlCommand2.ExecuteNonQuery();
                //        }

                //        if (dtEdit != null && dtEdit.Rows.Count > 0)
                //        {
                //            foreach (DataRow item in dtEdit.Rows)
                //            {
                //                string querys = String.Format("INSERT INTO [tblTransactionPostRelation]([Date],[Description],[Dr] ,[Cr],[PostID])VALUES ('{0}', '{1}', '{2}', '{3}', '{4}');", Convert.ToDateTime(item["Date"], enGB), (object)item["Transaction Description"], (object)item["Dr"], (object)item["Cr"], ID);

                //                using (SqlCommand sqlCommand = new SqlCommand(querys, db, transaction))
                //                {
                //                    sqlCommand.ExecuteNonQuery();
                //                }
                //            }
                //        }

                //        transaction.Commit();

                //        db.Close();

                //        setReload();
                //        Common.setMessageBox("Changes in record has been successfully saved.", Program.ApplicationName, 1);
                //        //bttnReset.PerformClick();
                //        tsbReload.PerformClick();


                //    }
                //}
                #endregion



            }

            //setReload();
        }

        private void setReload()
        {
            //connect.connect.Close();
            DataTable dt;
            //System.Data.DataSet ds;
            using (var ds = new System.Data.DataSet())
            {
                //connect.connect.Open();
                using (SqlDataAdapter ada = new SqlDataAdapter("SELECT PostID,AccountNo,Period,CloseBal,OpenBal   FROM tblTransactionPost", Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }
                //connect.connect.Close();
                dt = ds.Tables[0];
                //gridControl1.DataSource = dt.DefaultView;

                gridControl2.DataSource = dt.DefaultView;
            }
            //gridView2.OptionsBehavior.Editable = false;
            gridView3.OptionsBehavior.Editable = false;
            gridView3.Columns["PostID"].Visible = false;
            gridView3.Columns["CloseBal"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            gridView3.Columns["CloseBal"].DisplayFormat.FormatString = "n2";
            gridView3.Columns["OpenBal"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            gridView3.Columns["OpenBal"].DisplayFormat.FormatString = "n2";
            //gridView2.Columns["PostID"].Visible = false;
            //gridView2.Columns["OpenBal"].Visible = false;
            gridView3.BestFitColumns();
        }

        protected bool EditRecordMode()
        {
            bool bResponse = false;
            GridView view = (GridView)gridControl2.FocusedView;
            if (view != null)
            {
                DataRow dr = view.GetDataRow(view.FocusedRowHandle);
                if (dr != null)
                {
                    ID = Convert.ToInt32(dr["PostID"]);
                    bResponse = FillField(Convert.ToInt32(dr["PostID"]));
                    setReload1(Convert.ToInt32(dr["PostID"]));
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
            //load data from the table into the forms for edit


            DataTable dts = (new Logic()).getSqlStatement((String.Format("select * from tblTransactionPost where PostID ='{0}'", fieldid))).Tables[0];

            if (dts != null)
            {
                bResponse = true;

                cboAcct.Text = dts.Rows[0]["AccountNo"].ToString();
                cboAcct.Enabled = false;
                txtPeriod.Text = dts.Rows[0]["Period"].ToString();
                txtPeriod.Enabled = false;
                txtCBal.Text = string.Format("{0:n2}", dts.Rows[0]["CloseBal"]);
                //txtCBal.Enabled = false;
                //cboBranch.Text = dts.Rows[0]["BranchName"].ToString();
                //chkActive.CheckState = dts.Rows[0]["Status"].ToString();


            }
            else
                bResponse = false;

            return bResponse;
        }

        private void setReload1(int fieldid)
        {
            //connect.connect.Close();
            //System.Data.DataSet ds;
            using (var ds = new System.Data.DataSet())
            {
                //connect.connect.Open();
                using (SqlDataAdapter ada = new SqlDataAdapter((String.Format("select CONVERT(VARCHAR, Date,103) AS Date ,Description AS [Transaction Description],Dr,Cr  from tblTransactionPostRelation where PostID ='{0}'", fieldid)), Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }
                //connect.connect.Close();
                dtEdit = ds.Tables[0];
                //gridControl1.DataSource = dt.DefaultView;

                gridControl1.DataSource = dtEdit.DefaultView;
            }
            gridView1.OptionsBehavior.Editable = false;

            gridView1.Columns["Dr"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            gridView1.Columns["Dr"].DisplayFormat.FormatString = "n2";

            gridView1.Columns["Cr"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            gridView1.Columns["Cr"].DisplayFormat.FormatString = "n2";
            gridView1.BestFitColumns();
        }


    }
}
