using BankReconciliation.Class;
using BankReconciliation.Report;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraReports.UI;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using TaxSmartSuite.Class;

namespace BankReconciliation.Forms
{
    public partial class FrmTransactionPost : Form
    {
        private SqlCommand _command; private SqlDataAdapter adp; private DataTable Dts;

        private bool isThird = false;

        private string payref, branchname, BranchCode, bankcode, AgencyName, AgencyCode, selectedCode, ElemDescr, ElemType, stationcode, stationName, status, uploading;

        public static FrmTransactionPost publicStreetGroup;

        protected TransactionTypeCode iTransType; private string receipts; string strmonth; string monthName; string BankCode; string Acctno;

        protected bool boolIsUpdate; protected string ID; bool isFirst = true;

        private string querys, stDate, endDate; string[] split; string[] split2; string[] Split2; double openbalAcct;

        DataTable tableTrans = new DataTable();

        DataTable dt, dt1, dat, dats;

        DataTable dtEdit;

        string BatchNumber, selectedPage;

        DevExpress.XtraGrid.GridColumnSummaryItem sdm;

        System.Globalization.CultureInfo enGB = new System.Globalization.CultureInfo("en-GB");

        double CrAmout;

        public FrmTransactionPost()
        {
            InitializeComponent();

            publicStreetGroup = this;

            setImages();

            ToolStripEvent();

            iTransType = TransactionTypeCode.Null;

            Load += OnFormLoad;

            OnFormLoad(null, null);

            //create offline table
            tableTrans.Columns.Add("Date", typeof(string));
            tableTrans.Columns.Add("Transaction Description", typeof(string));
            tableTrans.Columns.Add("Dr", typeof(Decimal));
            tableTrans.Columns.Add("Cr", typeof(Decimal));



            DataTable dts = (new Logic()).getSqlStatement((String.Format("select TaxCode from tblState where StateCode= '{0}' ", Program.stateCode))).Tables[0];

            if (dts != null && dts.Rows.Count > 0)
            {
                //statename = dts.Rows[0]["statename"].ToString();
                payref = dts.Rows[0]["TaxCode"].ToString();
                //cboAccount.Text = dts.Rows[0]["AccountNumber"].ToString();

            }

            cboAcct.SelectedIndexChanged += cboAcct_SelectedIndexChanged;

            cboPeriod.SelectedIndexChanged += cboPeriod_SelectedIndexChanged;

            cboBank.SelectedIndexChanged += cboBank_SelectedIndexChanged;

            //dtpDateTrans.ValueChanged += dtpDateTrans_ValueChanged;

            cboCategory.SelectedIndexChanged += cboCategory_SelectedIndexChanged;

            cboRevenue.SelectedIndexChanged += cboRevenue_SelectedIndexChanged;

            cboZonal.SelectedIndexChanged += cboZonal_SelectedIndexChanged;

            radioGroup1.SelectedIndexChanged += radioGroup1_SelectedIndexChanged;

            //radioGroup2.SelectedIndexChanged += radioGroup2_SelectedIndexChanged;

            txtDateE.LostFocus += txtDateE_LostFocus;

            txtDateds.LostFocus += txtDateds_LostFocus;

            txtAmount.LostFocus += txtAmount_LostFocus;

            txtCloseBal.LostFocus += txtCloseBal_LostFocus;

            txtPayerName.LostFocus += txtPayerName_LostFocus;

            txtopenBal.LostFocus += txtopenBal_LostFocus;

            txtopenBal.Leave += txtopenBal_Leave;

            txtCloseBal.Leave += txtCloseBal_Leave;

            txtDepositAmt.LostFocus += txtDepositAmt_LostFocus;

            bttnPost.Click += Bttn_Click;

            btnAgent.Click += Bttn_Click;

            btnPerson.Click += Bttn_Click;

            btnUpdate.Click += Bttn_Click;

            btnGenrate.Click += Bttn_Click;

            bttnUpdate.Click += Bttn_Click;

            bttnReport.Click += Bttn_Click;

            bttnClose.Click += Bttn_Click;

            btnRegister.Click += Bttn_Click;

            bttnDetails.Click += Bttn_Click;

            button1.Click += Bttn_Click;

            bttnCollection.Click += Bttn_Click;

            bttnPeriod.Click += Bttn_Click;

            lblCollect.LinkClicked += lblCollect_LinkClicked;

            lblpay.LinkClicked += lblpay_LinkClicked;

            label33.LinkClicked += label33_LinkClicked;

            label9.LinkClicked += label9_LinkClicked;

            luePayeridPerson.EditValueChanged += luePayeridPerson_EditValueChanged;

            lupeAgent.EditValueChanged += lupeAgent_EditValueChanged;

            cboPayMode.KeyPress += cboPaymode_KeyPress;

            cboPeriod.KeyPress += cboPeriod_KeyPress;

            cboAcct.KeyPress += cboAcct_KeyPress;

            cboBank.KeyPress += cboBank_KeyPress;

            cboPayMode.SelectedIndexChanged += cboPayMode_SelectedIndexChanged;

            gridView3.DoubleClick += gridView3_DoubleClick;

            xtraTabControl1.SelectedPageChanged += xtraTabControl1_SelectedPageChanged;


        }

        void cboBank_KeyPress(object sender, KeyPressEventArgs e)
        {
            Methods.AutoComplete(cboBank, e, true);
        }

        void cboAcct_KeyPress(object sender, KeyPressEventArgs e)
        {
            Methods.AutoComplete(cboAcct, e, true);
        }

        void cboBank_SelectedIndexChanged(object sender, EventArgs e)
        {
            setDBComboBox();
        }

        void cboPeriod_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboPeriod.SelectedValue != null && !isFirst)
            {

                if (string.IsNullOrEmpty(cboAcct.Text))
                {
                    GetAcctInfor((string)Acctno);
                }
                else
                { GetAcctInfor((string)cboAcct.Text); }

                if (string.IsNullOrEmpty(Acctno))
                {

                }
                else
                {
                    cboAcct.Text = Acctno;
                }
                //
                GetBalance(); OpenBal();

                if (string.IsNullOrEmpty(cboPeriod.Text))
                {
                    GetPeriodCollection((string)cboBank.SelectedValue, ID);
                    GetAmountNotBank((string)cboBank.SelectedValue, ID);
                    GetNotPayCirectcollect((string)cboBank.SelectedValue, ID);

                    if (isPeriodClose(ID, cboAcct.Text))
                    {
                        Common.setMessageBox("Transaction Period Already Closed", Program.ApplicationName, 1);
                        setReload1(ID, cboAcct.Text);
                        ProcessClose();
                        //xtraTabControl1.Enabled = false;
                        return;
                    }
                }
                else
                {
                    GetPeriodCollection((string)cboBank.SelectedValue, cboPeriod.Text);
                    GetAmountNotBank((string)cboBank.SelectedValue, cboPeriod.Text);
                    GetNotPayCirectcollect((string)cboBank.SelectedValue, cboPeriod.Text);

                    if (isPeriodClose(cboPeriod.Text, cboAcct.Text))
                    {
                        Common.setMessageBox("Transaction Period Already Closed", Program.ApplicationName, 1);
                        setReload1(cboPeriod.Text, cboAcct.Text);
                        ProcessClose();
                        //xtraTabControl1.Enabled = false; 
                        return;
                    }
                }



            }
        }

        void label33_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            XtraRepPeriodSummary periodSummary = new XtraRepPeriodSummary();

            periodSummary.xrLabel6.Text = string.Format("{0:n2}", txtopenBal.Text);

            periodSummary.xrLabel7.Text = string.Format("{0:n2}", lblCollect.Text);

            periodSummary.xrLabel8.Text = string.Format("{0:n2}", label9.Text);

            periodSummary.xrLabel9.Text = string.Format("{0:n2}", lblpay.Text);

            periodSummary.xrLabel10.Text = string.Format("{0:n2}", gridView1.Columns["Dr"].SummaryItem.SummaryValue);


            periodSummary.ShowPreviewDialog();
        }

        void label9_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            DataTable Dts; string varGet;
            split = cboPeriod.Text.Trim().Split(new Char[] { '/' });
            using (var ds = new System.Data.DataSet())
            {
                using (SqlDataAdapter ada = new SqlDataAdapter((string)String.Format("SELECT [Date Interval] FROM dbo.tblPeriods where Periods = '{0}'AND Year= '{1}'", split[0], split[1]), Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }
                Dts = ds.Tables[0];

                if (Dts != null && Dts.Rows.Count > 0)
                {
                    varGet = (string)Dts.Rows[0]["[Date Interval]"];

                    split2 = varGet.Split(new Char[] { '-' });
                }
            }

            XtraRepReconDetails repRec = new XtraRepReconDetails();

            stDate = (string)split2[0];
            endDate = (string)split2[1];

            repRec.paramBankCode.Value = BankCode;
            repRec.paramPeriod.Value = cboPeriod.Text;
            repRec.paramRecord.Value = false;
            repRec.paramPayDirect.Value = true;

            repRec.xrLabel3.Text = String.Format("Payment not in Bank Statement from {0} to {1}", stDate, endDate);
            repRec.xrLabel5.Text = String.Format("{0} - {1}", cboBank.Text, branchname);

            repRec.ShowPreviewDialog();
        }

        void lblpay_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            DataTable Dts; string varGet;
            split = cboPeriod.Text.Trim().Split(new Char[] { '/' });
            using (var ds = new System.Data.DataSet())
            {
                using (SqlDataAdapter ada = new SqlDataAdapter((string)String.Format("SELECT [Date Interval] FROM dbo.tblPeriods where Periods = '{0}'AND Year= '{1}'", split[0], split[1]), Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }
                Dts = ds.Tables[0];

                if (Dts != null && Dts.Rows.Count > 0)
                {
                    varGet = (string)Dts.Rows[0]["[Date Interval]"];

                    split2 = varGet.Split(new Char[] { '-' });
                }
            }

            XtraRepReconDetails repRec = new XtraRepReconDetails();

            stDate = (string)split2[0];
            endDate = (string)split2[1];

            repRec.paramBankCode.Value = BankCode;
            repRec.paramPeriod.Value = cboPeriod.Text;
            repRec.paramRecord.Value = true;
            repRec.paramPayDirect.Value = false;

            repRec.xrLabel3.Text = String.Format("Collections not in PayDirect from {0} to {1}", stDate, endDate);
            repRec.xrLabel5.Text = String.Format("{0} - {1}", cboBank.Text, branchname);

            repRec.ShowPreviewDialog();
        }

        void lblCollect_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            DataTable Dts; string varGet;
            split = cboPeriod.Text.Trim().Split(new Char[] { '/' });
            using (var ds = new System.Data.DataSet())
            {
                using (SqlDataAdapter ada = new SqlDataAdapter((string)String.Format("SELECT [Date Interval] FROM dbo.tblPeriods where Periods = '{0}'AND Year= '{1}'", split[0], split[1]), Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }
                Dts = ds.Tables[0];

                if (Dts != null && Dts.Rows.Count > 0)
                {
                    varGet = (string)Dts.Rows[0]["[Date Interval]"];

                    split2 = varGet.Split(new Char[] { '-' });
                }
            }

            XtraRepReconDetails repRec = new XtraRepReconDetails();

            stDate = (string)split2[0];
            endDate = (string)split2[1];

            repRec.paramBankCode.Value = BankCode;
            repRec.paramPeriod.Value = cboPeriod.Text;
            repRec.paramRecord.Value = true;
            repRec.paramPayDirect.Value = true;

            repRec.xrLabel3.Text = String.Format("PayDirect Collections from {0} to {1}", stDate, endDate);
            repRec.xrLabel5.Text = String.Format("{0} - {1}", cboBank.Text, branchname);

            repRec.ShowPreviewDialog();
        }

        void txtPayerName_LostFocus(object sender, EventArgs e)
        {
            txtPayerName.Text = txtPayerName.Text.Trim().ToUpper();
            cboRevenue.Focus();
        }

        void xtraTabControl1_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            selectedPage = string.Empty;

            if (e.Page.Name == "xtraTabPage1")
            {
                selectedPage = (string)e.Page.Tag;
                if (!boolIsUpdate)
                {

                    //tableTrans.Rows.Add(new object[] { txtDateds.Text, ElemDescr, 0, txtDepositAmt.Text });
                    gridControl1.DataSource = tableTrans;
                }
                else
                {
                    //double Cr = Convert.ToDouble(txtDepositAmt.Text);
                    //dtEdit.Rows.Add(new object[] { txtDateds.Text, ElemDescr, 0, Cr });
                    gridControl1.DataSource = dtEdit;
                }
                gridView1.OptionsBehavior.Editable = false;

                gridView1.Columns["Dr"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                gridView1.Columns["Dr"].DisplayFormat.FormatString = "n2";
                gridView1.Columns["Cr"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                gridView1.Columns["Cr"].DisplayFormat.FormatString = "n2";
                gridView1.BestFitColumns();
                radioGroup1.Focus();
            }
            else if (e.Page.Name == "xtraTabPage2")
            {
                selectedPage = (string)e.Page.Tag;
                if (!string.IsNullOrEmpty(cboPeriod.Text))
                {
                    txtDateds.Focus();
                }
                return;

            }
        }

        void txtDateds_LostFocus(object sender, EventArgs e)
        {
            if (cboPeriod.Text == null || cboPeriod.Text == "")
            {
                Common.setEmptyField("Transaction Period", Program.ApplicationName);
                cboPeriod.Focus(); return;
            }
            else
            {
                split = cboPeriod.Text.Trim().Split(new Char[] { '/' });

                Split2 = txtDateds.EditValue.ToString().Split(new Char[] { '/' });

                if (Convert.ToInt32(split[0]) != Convert.ToInt32(Split2[1]) || Convert.ToInt32(split[1]) != Convert.ToInt32(Split2[2]))
                {
                    Common.setMessageBox(" Date Not Withing the Transaction Period ", Program.ApplicationName, 1);
                    txtDateds.EditValue = ""; txtDateds.Focus();
                    return;
                }
            }

        }

        void gridView3_DoubleClick(object sender, EventArgs e)
        {
            tsbEdit.PerformClick();
        }

        //void radioGroup2_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    //radioGroup1.EditValue.ToString()

        //    if (radioGroup2.EditValue == null)
        //    {
        //        return;
        //    }
        //    else
        //    {
        //        if (radioGroup2.EditValue.ToString() == "0001")
        //        {
        //            xtraTabControl1.Enabled = true;
        //            xtraTabControl1.SelectedTabPage = xtraTabPage1;
        //            xtraTabPage1.PageEnabled = true;
        //            xtraTabPage1.Text = cboCategory.Text.ToUpper() + "Internal Element";
        //            xtraTabPage2.PageEnabled = false;
        //            setReload1(cboPeriod.Text, cboAcct.Text);
        //        }
        //        if (radioGroup2.EditValue.ToString() == "0002")
        //        {
        //            xtraTabControl1.Enabled = true;
        //            xtraTabControl1.SelectedTabPage = xtraTabPage2;
        //            xtraTabPage2.PageEnabled = true;
        //            xtraTabPage1.PageEnabled = false;
        //            xtraTabPage2.Text = cboCategory.Text.ToUpper() + "External Element";
        //        }

        //        selectedCode = (string)cboCategory.SelectedValue;
        //    }
        //}

        void cboPeriod_KeyPress(object sender, KeyPressEventArgs e)
        {
            Methods.AutoComplete(cboPayMode, e, true);
        }

        void txtCloseBal_Leave(object sender, EventArgs e)
        {

            DataTable dts = (new Logic()).getSqlStatement((String.Format("SELECT CloseBal FROM tblTransactionPosting WHERE Period = '{0}' AND AccountNo= '{1}' ", (string)cboPeriod.Text, (string)cboAcct.Text.Trim()))).Tables[0];
            if (dts != null && dts.Rows.Count > 0)
            {
                ////ask if the print was sucessfull
                //DialogResult result = MessageBox.Show(" Closeing Balance Already Exist for this Period, Do you wish to maintain it ", Program.ApplicationName, MessageBoxButtons.YesNo);

                //if (result == DialogResult.Yes)
                //{
                txtCloseBal.Text = String.Format("{0:N2}", dts.Rows[0]["CloseBal"]); return;
                //}


            }
        }

        void txtopenBal_Leave(object sender, EventArgs e)
        {
            DataTable dts = (new Logic()).getSqlStatement((String.Format("SELECT OpenBal FROM tblTransactionPosting WHERE Period = '{0}' AND AccountNo= '{1}' ", (string)cboPeriod.Text, (string)cboAcct.Text.Trim()))).Tables[0];

            if (dts != null && dts.Rows.Count > 0)
            {
                //ask if the print was sucessfull
                //DialogResult result = MessageBox.Show(" Open Balance Already Exist for this Period, Do you wish to maintain it ", Program.ApplicationName, MessageBoxButtons.YesNo);

                //if (result == DialogResult.Yes)
                //{
                txtopenBal.Text = String.Format("{0:N2}", dts.Rows[0]["OpenBal"]); return;

                //}


            }


        }

        void cboZonal_SelectedIndexChanged(object sender, EventArgs e)
        {
            //stationcode, stationName

            DataTable dts = (new Logic()).getSqlStatement((String.Format("SELECT StationCode,StationName FROM dbo.tblStationMap WHERE RevenueOfficeCode = '{0}' ", cboZonal.SelectedValue))).Tables[0];

            if (dts != null && dts.Rows.Count > 0)
            {
                stationName = dts.Rows[0]["StationName"].ToString();
                stationcode = dts.Rows[0]["StationCode"].ToString();
            }
        }

        void cboRevenue_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dts = (new Logic()).getSqlStatement((String.Format("SELECT AgencyCode,AgencyName FROM ViewAgencyRevenueTemp WHERE RevenueCode = '{0}' ", cboRevenue.SelectedValue))).Tables[0];

            if (dts != null && dts.Rows.Count > 0)
            {
                //statename = dts.Rows[0]["statename"].ToString();
                //payref = dts.Rows[0]["TaxCode"].ToString();
                AgencyName = dts.Rows[0]["AgencyName"].ToString();
                AgencyCode = dts.Rows[0]["AgencyCode"].ToString();
                //cboAccount.Text = dts.Rows[0]["AccountNumber"].ToString();

            }
        }

        void txtDepositAmt_LostFocus(object sender, EventArgs e)
        {
            String Text = ((TextBox)sender).Text.Replace(",", "");

            double Num;

            if (double.TryParse(Text, out Num))
            {
                Text = String.Format("{0:N2}", Num);
                ((TextBox)sender).Text = Text;
            }
        }

        void txtopenBal_LostFocus(object sender, EventArgs e)
        {
            String Text = ((TextBox)sender).Text.Replace(",", "");

            double Num;

            if (double.TryParse(Text, out Num))
            {
                Text = String.Format("{0:N2}", Num);
                ((TextBox)sender).Text = Text;
            }

        }

        void txtCloseBal_LostFocus(object sender, EventArgs e)
        {
            String Text = ((TextBox)sender).Text.Replace(",", "");

            double Num;

            if (double.TryParse(Text, out Num))
            {
                Text = String.Format("{0:N2}", Num);
                ((TextBox)sender).Text = Text;
            }
        }

        void cboCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboCategory.SelectedValue.ToString() == "0001")
            {
                xtraTabControl1.Enabled = true;
                xtraTabControl1.SelectedTabPage = xtraTabPage1;
                xtraTabPage1.PageEnabled = true;
                xtraTabPage1.Text = cboCategory.Text.ToUpper() + " Transaction Element";
                xtraTabPage2.PageEnabled = false;
            }
            if (cboCategory.SelectedValue.ToString() == "0002")
            {
                xtraTabControl1.Enabled = true;
                xtraTabControl1.SelectedTabPage = xtraTabPage2;
                xtraTabPage2.PageEnabled = true;
                xtraTabPage1.PageEnabled = false;
                xtraTabPage2.Text = cboCategory.Text.ToUpper() + " Transaction Element";
            }

            selectedCode = (string)cboCategory.SelectedValue;

        }

        void dtpDateTrans_ValueChanged(object sender, EventArgs e)
        {
            //txtPeriod.Text = dtpDateTrans.Value.ToString("MM/yyyy");
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
                //dtpDateTrans.Focus();
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
                groupControl2.Text = "Delete Record Mode"; iTransType = TransactionTypeCode.Delete;

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
                setReload();
                ShowForm();
            }
            //bttnReset.PerformClick();
        }

        void OnFormLoad(object sender, EventArgs e)
        {
            ShowForm(); stDBComboBoxZonal(); setDBComboBoxBank();

            SetDBComboBoxCategory(); setDBComboBoxRevenue();

            setDBComboBoxPerson(); setDBComboBoxAgent(); setDBComboBoxPay();

            setReload(); setDBComboBoxPeriod(); setDBComboBoxBanks();

            isFirst = false;

            bttnDetails.Visible = false;

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
                    splitContainer1.Panel2Collapsed = true;
                    break;
                case TransactionTypeCode.Edit:
                    splitContainer1.Panel1Collapsed = false;
                    splitContainer1.Panel2Collapsed = true;
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
                string query = string.Format("SELECT BankAccountID,AccountNumber FROM ViewBankBranchAccount where BankShortCode like '%{0}%'", cboBank.SelectedValue);
                using (SqlDataAdapter ada = new SqlDataAdapter(query, Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }

                Dt = ds.Tables[0];
            }

            Common.setComboList(cboAcct, Dt, "BankAccountID", "AccountNumber");

            cboAcct.SelectedIndex = -1;


        }

        public void setDBComboBoxBank()
        {
            DataTable Dt;

            //connect.connect.Close();

            using (var ds = new System.Data.DataSet())
            {
                //connect.connect.Open();
                using (SqlDataAdapter ada = new SqlDataAdapter((string)"select *  from tblBank", Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }

                Dt = ds.Tables[0];
            }

            Common.setComboList(cboBank, Dt, "BankShortCode", "BankName");

            cboBank.SelectedIndex = -1;


        }

        public void SetDBComboBoxCategory()
        {
            DataTable Dt;

            //connect.connect.Close();

            using (var ds = new System.Data.DataSet())
            {
                //connect.connect.Open();
                using (SqlDataAdapter ada = new SqlDataAdapter((string)"SELECT Description,ElementCatCode FROM tblElementCategory", Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }

                Dt = ds.Tables[0];
            }

            Common.setComboList(cboCategory, Dt, "ElementCatCode", "Description");

            cboCategory.SelectedIndex = -1;
        }

        private void setImages()
        {
            tsbNew.Image = MDIMains.publicMDIParent.i16x16.Images[5];
            tsbEdit.Image = MDIMains.publicMDIParent.i16x16.Images[6];
            tsbDelete.Image = MDIMains.publicMDIParent.i16x16.Images[7];
            tsbReload.Image = MDIMains.publicMDIParent.i16x16.Images[0];
            tsbClose.Image = MDIMains.publicMDIParent.i16x16.Images[11];

            //bttnCancel.Image = MDIMains.publicMDIParent.i32x32.Images[9];
            bttnUpdate.Image = MDIMains.publicMDIParent.i32x32.Images[7];
            btnUpdate.Image = MDIMains.publicMDIParent.i32x32.Images[7];
            //////bttnReset.Image = MDIMains.publicMDIParent.i32x32.Images[8];
            bttnPost.Image = MDIMains.publicMDIParent.i32x32.Images[34];
            bttnReport.Image = MDIMains.publicMDIParent.i32x32.Images[5];
            bttnClose.Image = MDIMains.publicMDIParent.i16x16.Images[9];
            bttnCollection.Image = MDIMains.publicMDIParent.i16x16.Images[10];
            bttnDetails.Image = MDIMains.publicMDIParent.i32x32.Images[41];
            bttnPeriod.Image = MDIMains.publicMDIParent.i32x32.Images[42];

        }

        void GetAcctInfor(string parameter)
        {
            DataTable Dt;

            //connect.connect.Close();

            using (var ds = new System.Data.DataSet())
            {
                //connect.connect.Open();
                string query = String.Format("select BankName,AccountName,BranchName,BankAccountID,OpenBal,BankShortCode,BranchCode from ViewBankBranchAccount where AccountNumber = '{0}'", parameter);

                using (SqlDataAdapter ada = new SqlDataAdapter(query, Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }

                Dt = ds.Tables[0];
            }

            if (Dt != null && Dt.Rows.Count > 0)
            {
                //lblActDesc.Text = (string)Dt.Rows[0]["AccountName"];

                //cboBank.Text = (string)Dt.Rows[0]["BankName"];

                BankCode = (string)Dt.Rows[0]["BankShortCode"];

                BranchCode = (string)Dt.Rows[0]["BranchCode"];

                branchname = (string)Dt.Rows[0]["BranchName"];


            }

        }

        void cboAcct_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (cboAcct.SelectedValue != null && !isFirst)
            //{
            //    GetAcctInfor(cboAcct.SelectedValue.ToString());

            //    GetBalance(); OpenBal();

            //    if (string.IsNullOrEmpty(cboPeriod.Text))
            //    {
            //        GetPeriodCollection(BankCode, ID);
            //        GetAmountNotBank(BankCode, ID);
            //        GetNotPayCirectcollect(BankCode, ID);
            //    }
            //    else
            //    {
            //        GetPeriodCollection(BankCode, cboPeriod.Text);
            //        GetAmountNotBank(BankCode, cboPeriod.Text);
            //        GetNotPayCirectcollect(BankCode, cboPeriod.Text);
            //    }

            //}
        }

        void radioGroup1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (radioGroup1.EditValue == null)
            {
                return;
            }
            else
            {
                //groupBox2.Enabled = true;

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
                string query = String.Format("select * from tblTransDefinition where type = '{0}' and IsActive='1' and ElementCatCode<>'0002'", parameter);

                using (SqlDataAdapter ada = new SqlDataAdapter(query, Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }

                Dt = ds.Tables[0];
            }

            Common.setComboList(cboTransType, Dt, "transid", "DESCRIPTION");

            cboTransType.SelectedIndex = -1;


        }

        void txtDateE_LostFocus(object sender, EventArgs e)
        {
            if (cboPeriod.Text == null || cboPeriod.Text == "")
            {
                Common.setEmptyField("Transaction Period", Program.ApplicationName);
                cboPeriod.Focus(); return;
            }
            else
            {
                split = cboPeriod.Text.Trim().Split(new Char[] { '/' });

                Split2 = txtDateE.EditValue.ToString().Split(new Char[] { '/' });

                if (Convert.ToInt32(split[0]) != Convert.ToInt32(Split2[1]) || Convert.ToInt32(split[1]) != Convert.ToInt32(Split2[2]))
                {
                    Common.setMessageBox(" Date Not Withing the Transaction Period ", Program.ApplicationName, 1);
                    txtDateE.EditValue = ""; txtDateE.Focus();
                    return;
                }
            }

        }

        void txtAmount_LostFocus(object sender, EventArgs e)
        {
            String Text = ((TextBox)sender).Text.Replace(",", "");

            double Num;

            if (double.TryParse(Text, out Num))
            {
                Text = String.Format("{0:N2}", Num);
                ((TextBox)sender).Text = Text;
            }
        }

        void Bttn_Click(object sender, EventArgs e)
        {
            //XtraRepReconDetails
            if (sender == bttnPost)
            {
                UpdateRecord();
            }
            else if (sender == btnPerson)
            {
                //tsbReload.PerformClick();
                txtPayerID.Visible = false;
                lupeAgent.Visible = false;
                luePayeridPerson.Visible = true;
            }
            else if (sender == btnUpdate)
            {
                UpdateRecordInDir();
            }
            else if (sender == btnGenrate)
            {
                Generate();
            }
            else if (sender == btnAgent)
            {
                txtPayerID.Visible = false;
                luePayeridPerson.Visible = false;
                lupeAgent.Visible = true;
            }
            else if (sender == bttnClose)
            {
                ClosePeriod();
            }
            else if (sender == bttnUpdate)
            {
                InsertUpdate();
            }
            else if (sender == bttnPeriod)
            {
                XtraRepPeriodSummary periodSummary = new XtraRepPeriodSummary();

                periodSummary.xrLabel6.Text = string.Format("{0:n2}", txtopenBal.Text);

                periodSummary.xrLabel7.Text = string.Format("{0:n2}", lblCollect.Text);

                periodSummary.xrLabel8.Text = string.Format("{0:n2}", label9.Text);

                periodSummary.xrLabel9.Text = string.Format("{0:n2}", lblpay.Text);

                periodSummary.xrLabel10.Text = string.Format("{0:n2}", gridView1.Columns["Dr"].SummaryItem.SummaryValue);


                periodSummary.ShowPreviewDialog();

            }
            else if (sender == bttnCollection)
            {
                if (string.IsNullOrEmpty(BankCode))
                {
                    BankCode = (string)cboBank.SelectedValue;
                }

                if (!string.IsNullOrEmpty(BankCode) && !string.IsNullOrEmpty(cboPeriod.Text))
                {
                    using (FrmCollectView collectview = new FrmCollectView(BankCode, cboPeriod.Text))
                    {
                        collectview.ShowDialog();

                        double collectb = Convert.ToDouble(lblCollect.Text);

                        string dub = string.Format("{0:n2}", collectview.getTotal);

                        //lblCollect.Text = string.Format("{0:n2}", collectview.getTotal);

                        //label9.Text = string.Format("{0:n2}", (collectb - Convert.ToDouble(dub)));

                    }

                    GetPeriodCollection(BankCode, cboPeriod.Text);
                    GetAmountNotBank(BankCode, cboPeriod.Text);
                }
                return;

            }
            else if (sender == bttnDetails)
            {

            }
            else if (sender == btnRegister)
            {
                BatchNumber = String.Format("{0:d9}", (DateTime.Now.Ticks / 10) % 10000000);
                txtPayerID.Visible = true; txtPayerName.Visible = true;
                txtPayerID.Enabled = true; txtPayerName.Enabled = true;
                txtPayerID.Text = string.Format("{0}|{1}|{2:yyyMMddhhmmss}", BankCode, BatchNumber, DateTime.Now); txtPayerName.Focus();
            }
            else if (sender == bttnReport)
            {
                strmonth = cboPeriod.Text.Substring(0, 2);

                split = cboPeriod.Text.Trim().Split(new Char[] { '/' });
                var dtf = CultureInfo.CurrentCulture.DateTimeFormat;
                var month = Convert.ToInt32(cboPeriod.Text.Substring(0, 2));
                var fyear = cboPeriod.Text.Length;
                monthName = dtf.GetMonthName(month);

                xrTranAccount report = new xrTranAccount();

                string fulltext = String.Format("Collection Reconciliation for : [{0} {1}]", monthName, Convert.ToInt32(split[1]));

                report.paramAccount.Value = cboAcct.Text;

                report.paramPeriod.Value = cboPeriod.Text;

                //report.paramPeriod.Value = true;

                //report.paramPayDirect.Value = true;

                report.xrLabel1.Text = fulltext;
                report.xrLabel18.Text = String.Format("{0} - {1}", cboBank.Text, branchname);

                report.ShowPreviewDialog();
            }
        }

        void UpdateRecordInDir()
        {
            BatchNumber = String.Format("{0:d9}", (DateTime.Now.Ticks / 10) % 1000000000);

            txtPaymentRef.Text = String.Format("{0}|ICM|OYPD|{1}|{2:dd-MM-yyyy}|{3}", BankCode, payref, DateTime.Now, BatchNumber);

            if (string.IsNullOrEmpty(txtDepositAmt.Text))
            {
                Common.setEmptyField("Deposit Amount", Program.ApplicationName);
                txtDepositAmt.Focus(); return;
            }
            else if (string.IsNullOrEmpty(cboAcct.SelectedValue.ToString()))
            {
                Common.setEmptyField("Account No.", Program.ApplicationName);
                cboAcct.Focus(); return;
            }
            //00/0000
            else if (string.IsNullOrEmpty(cboPeriod.Text))
            {
                Common.setEmptyField("Transaction Period", Program.ApplicationName);
                cboPeriod.Focus(); return;
            }
            else if (string.IsNullOrEmpty(txtCloseBal.Text))
            {
                Common.setEmptyField("Closing Balance for Transaction Period", Program.ApplicationName);
                txtCloseBal.Focus(); return;
            }
            else if (string.IsNullOrEmpty(txtopenBal.Text))
            {
                Common.setEmptyField("Opening Balance for Transaction Period", Program.ApplicationName);
                txtopenBal.Focus(); return;

            }
            else
            {
                //get selected element category

                DataTable dts = (new Logic()).getSqlStatement((String.Format("SELECT Description,Type FROM dbo.tblTransDefinition	WHERE ElementCatCode = '{0}' ", (string)selectedPage))).Tables[0];

                if (dts != null && dts.Rows.Count > 0)
                {
                    ElemDescr = (string)dts.Rows[0]["Description"];
                    ElemType = (string)dts.Rows[0]["Type"];
                }

                //ChequeStatus

                //insert into collcetion report table
                using (SqlConnection db = new SqlConnection(Logic.ConnectionString))
                {
                    SqlTransaction transaction;

                    db.Open();

                    transaction = db.BeginTransaction();

                    try
                    {
                        //Program.UserID
                        using (SqlCommand sqlCommand1 = new SqlCommand(String.Format("INSERT INTO [tblCollectionReport]([Provider],[Channel],[PaymentRefNumber],[PaymentDate],[PayerName],[PayerID],[RevenueCode],[Description],[Amount],[PaymentMethod],[BankCode],[BankName],[BranchCode],[BranchName],[AgencyName],[AgencyCode],[ZoneCode],[ZoneName],[State],[ChequeNumber],[ChequeValueDate],[ChequeBankCode],[ChequeBankName],[EReceipts],[EReceiptsDate],[GeneratedBy],[UploadStatus],[StationCode],[StationName],[ChequeStatus],[IsPayDirect],[IsRecordExit]) VALUES ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}','{19}','{20}','{21}','{22}','{23}','{24}','{25}','{26}','{27}','{28}','{29}','{30}','{31}');", "ICM", "Bank", txtPaymentRef.Text.Trim(), Convert.ToDateTime(txtDateds.Text).ToString("MM/dd/yyyy"), txtPayerName.Text.Trim(), txtPayerID.Text.Trim(), cboRevenue.SelectedValue, cboRevenue.Text.Trim(), txtDepositAmt.Text, cboPayMode.Text, BankCode, cboBank.Text, BranchCode, branchname, AgencyName, AgencyCode, (string)cboZonal.SelectedValue, (string)cboZonal.Text, Program.StateName, txtChequeNo.Text.Trim(), dtpCheque.Value.Date.ToString("yyyy/MM/dd"), (string)cboCheque.Text, (string)cboCheque.Text, txtReceiptsNo.Text, DateTime.Now.ToString("yyyy/MM/dd"), Program.UserID, uploading, stationcode, stationName, status, false, true), db, transaction))
                        {
                            sqlCommand1.ExecuteNonQuery();
                        }

                        transaction.Commit();
                    }
                    catch (SqlException sqlError)
                    {
                        transaction.Rollback(); Tripous.Sys.ErrorBox(sqlError);
                    }
                    db.Close();
                }

                //insert into tblTransactionPost and tblTransactionPostRelation

                if (!boolIsUpdate)
                {

                    tableTrans.Rows.Add(new object[] { txtDateds.Text, ElemDescr, 0, txtDepositAmt.Text });
                    gridControl1.DataSource = tableTrans;
                }
                else
                {
                    double Cr = Convert.ToDouble(txtDepositAmt.Text);
                    dtEdit.Rows.Add(new object[] { txtDateds.Text, ElemDescr, 0, Cr });
                    gridControl1.DataSource = dtEdit;
                }


                #region

                //using (SqlConnection db = new SqlConnection(Logic.ConnectionString))
                //{
                //    SqlTransaction transaction;

                //    db.Open();

                //    transaction = db.BeginTransaction();
                //    try
                //    {

                //        using (SqlCommand sqlCommand = new SqlCommand(String.Format("INSERT INTO [tblTransactionPosting]([Period],[AccountNo],[ElementCat],[OpenBal],[CloseBal],[TransDate],[TransDescription],[Dr],[Cr]) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}');", cboPeriod.Text.Trim(), cboAcct.Text.Trim(), radioGroup2.EditValue, Convert.ToDouble(txtopenBal.Text), Convert.ToDouble(txtCloseBal.Text), Convert.ToDateTime(txtDateds.Text).ToString("MM/dd/yyyy"), ElemDescr, 0, Convert.ToDouble(txtDepositAmt.Text)), db, transaction))
                //        {
                //            sqlCommand.ExecuteNonQuery();
                //        }


                //        transaction.Commit();

                //        #region

                //        //int recs = Convert.ToInt32(new SqlCommand(String.Format("INSERT INTO tblTransactionPost( AccountNo ,Period , CloseBal , OpenBal) VALUES ('{0}','{1}','{2}','{3}');SELECT @@IDENTITY", (string)cboAcct.Text, (string)cboPeriod.Text, Convert.ToDouble(txtCloseBal.Text), Convert.ToDouble(txtopenBal.Text)), db, transaction).ExecuteScalar());


                //        ////insert records into tblTransactionPostRelation
                //        //using (SqlCommand sqlCommand = new SqlCommand(String.Format("INSERT INTO dbo.tblTransactionPostRelation( Date , Description , Dr , Cr , PostID) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}');", dtpDate.Value.Date.ToString("yyyy/MM/dd"), ElemDescr, 0, Convert.ToDouble(txtDepositAmt.Text), recs), db, transaction))
                //        //    {
                //        //        sqlCommand.ExecuteNonQuery();
                //        //    }


                //        //transaction.Commit();
                //        #endregion
                //    }
                //    catch (SqlException sqlError)
                //    {
                //        transaction.Rollback();     Tripous.Sys.ErrorBox(sqlError);
                //    }
                //    db.Close();
                //}
                #endregion

                Common.setMessageBox(" Transaction Post Successfully ", Program.ApplicationName, 1);



                clear(); txtDateds.Focus();
            }

        }

        private void clear()
        {
            txtPaymentRef.Clear();
            txtDepositAmt.Text = string.Empty;
            cboRevenue.SelectedIndex = -1;
            cboZonal.SelectedIndex = -1;
            cboPayMode.SelectedIndex = -1;
            cboCheque.SelectedIndex = -1;
            //txtSlipNo.Clear();
            txtDepName.Clear();
            txtPayerID.Clear();
            txtPayerName.Clear();
            txtReceiptsNo.Clear(); txtChequeNo.Clear();
            txtDateds.Text = string.Empty;
            //cedAmount.Text = string.Empty;

        }

        private void Generate()
        {
            //string rec;

            if (cboRevenue.Text.Length < 1)
            {
                Common.setMessageBox("Please Select Revenue Type", Program.ApplicationName, 1);

                cboRevenue.Focus(); return;
            }
            else if (string.IsNullOrEmpty(txtPayerID.Text))
            {
                Common.setEmptyField("Payer ID", Program.ApplicationName);
                btnPerson.Focus(); return;
            }
            else if (string.IsNullOrEmpty(cboPayMode.Text))
            {
                Common.setEmptyField(" Pay Mode ", Program.ApplicationName);
                cboPayMode.Focus(); return;
            }
            else

                if (Convert.ToInt32(cboPayMode.SelectedValue.ToString()) == 1)
            {
                txtReceiptsNo.Text = string.Empty; status = string.Empty; uploading = string.Empty;
            }
            else
            {
                status = "Cleared"; uploading = "Waiting";

                BatchNumber = String.Format("{0:d9}", (DateTime.Now.Ticks / 10) % 1000000000);
                //using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
                //{
                //    connect.Open();
                //    _command = new SqlCommand("doGeneratedReceipNumber", connect) { CommandType = CommandType.StoredProcedure };

                //    using (System.Data.DataSet ds = new System.Data.DataSet())
                //    {
                //        adp = new SqlDataAdapter(_command);
                //        adp.Fill(ds);
                //        Dts = ds.Tables[0];
                //        connect.Close();

                //        txtReceiptsNo.Text = String.Format("{0}", (string)ds.Tables[0].Rows[0]["ReceipitNo"]);

                //    }
                //}
            }



            receipts = Methods.generateRandomString(6);

            //string bankcode = retval.ToString();
            txtPaymentRef.Text = String.Format("ICM|{0}|{1}|{2:dd-MM-yyyy}|{3}", BankCode, payref, DateTime.Now, receipts);

            btnGenrate.Enabled = false; btnUpdate.Enabled = true;

        }

        void UpdateRecord()
        {
            if (radioGroup1.EditValue == "DR")
            {
                if (!boolIsUpdate)
                {

                    tableTrans.Rows.Add(new object[] { txtDateE.Text, cboTransType.Text, txtAmount.Text, 0 });
                    gridControl1.DataSource = tableTrans;
                }
                else
                {
                    double Dr = Convert.ToDouble(txtAmount.Text);
                    dtEdit.Rows.Add(new object[] { txtDateE.Text, cboTransType.Text, Dr, 0 });
                    gridControl1.DataSource = dtEdit;

                }

            }

            if (radioGroup1.EditValue == "CR")
            {
                if (!boolIsUpdate)
                {

                    tableTrans.Rows.Add(new object[] { txtDateE.Text, cboTransType.Text, 0, txtAmount.Text });
                    gridControl1.DataSource = tableTrans;
                }
                else
                {
                    double Cr = Convert.ToDouble(txtAmount.Text);
                    dtEdit.Rows.Add(new object[] { txtDateE.Text, cboTransType.Text, 0, Cr });
                    gridControl1.DataSource = dtEdit;
                }


            }



            //gridControl1.BringToFront();
            gridView1.Columns["Dr"].BestFit();
            gridView1.Columns["Cr"].BestFit();
            gridView1.OptionsBehavior.Editable = false;
            gridView1.BestFitColumns();
            Clear();
            txtDateE.Focus();
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
            else if (string.IsNullOrEmpty(cboPeriod.Text))
            {
                Common.setEmptyField("Transaction Period", Program.ApplicationName);
                cboPeriod.Focus(); return;
            }
            else if (string.IsNullOrEmpty(txtCloseBal.Text))
            {
                Common.setEmptyField("Closing Balance for Transaction Period", Program.ApplicationName);
                txtCloseBal.Focus(); return;
            }
            else if (string.IsNullOrEmpty(txtopenBal.Text))
            {
                Common.setEmptyField("Opening Balance for Transaction Period", Program.ApplicationName);
                txtopenBal.Focus(); return;

            }
            else
            {
                if (isPeriodClose(cboPeriod.Text, cboAcct.Text))
                {
                    Common.setMessageBox("Sorry, Can't Saved Closed Period", Program.ApplicationName, 1); return;
                }
                else
                {
                    if (!boolIsUpdate)
                    {

                        using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
                        {
                            connect.Open();
                            _command = new SqlCommand("InsertUpdatePostRelationTransaction", connect) { CommandType = CommandType.StoredProcedure };
                            _command.Parameters.Add(new SqlParameter("@AccountNo", SqlDbType.VarChar)).Value = cboAcct.Text.Trim();
                            _command.Parameters.Add(new SqlParameter("@Period", SqlDbType.VarChar)).Value = cboPeriod.Text;
                            _command.Parameters.Add(new SqlParameter("@closeBal", SqlDbType.Money)).Value = Convert.ToDouble(txtCloseBal.Text.Trim());
                            _command.Parameters.Add(new SqlParameter("@OpenBal", SqlDbType.Money)).Value = Convert.ToDouble(txtopenBal.Text);
                            _command.Parameters.Add(new SqlParameter("@Isbool", SqlDbType.Bit)).Value = boolIsUpdate;
                            //_command.Parameters.Add(new SqlParameter("@recEdit", SqlDbType.Int)).Value = 0;
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
                        #region

                        ////delete perivison record from the table after edit or modify

                        //    using (SqlConnection db = new SqlConnection(Logic.ConnectionString))
                        //    {
                        //        SqlTransaction transaction;

                        //        db.Open();

                        //        transaction = db.BeginTransaction();

                        //        try
                        //        {

                        //            string query = String.Format("DELETE FROM dbo.tblTransactionPosting WHERE Period = '{0}' AND AccountNo= '{1}' ", (string)cboPeriod.Text, (string)cboAcct.Text.Trim());

                        //            using (SqlCommand sqlCommand = new SqlCommand(query, db, transaction))
                        //            {
                        //                sqlCommand.ExecuteNonQuery();
                        //            }

                        //            transaction.Commit();
                        //        }
                        //        catch (Exception ex)
                        //        {
                        //            transaction.Rollback();
                        //            Tripous.Sys.ErrorBox(ex);
                        //        }

                        //        db.Close(); 
                        //    }

                        //    //insert new receord

                        //    using (SqlConnection db = new SqlConnection(Logic.ConnectionString))
                        //    {
                        //        SqlTransaction transaction;

                        //        db.Open();

                        //        transaction = db.BeginTransaction();
                        //        //looping receord from the element posted
                        //        try
                        //        {

                        //            for (int i = 0; i < dtEdit.Rows.Count; i++)
                        //            {
                        //                //insert records into tblTransactionPostRelation
                        //                string query = String.Format("INSERT INTO tblTransactionPosting(Period,AccountNo,ElementCat,OpenBal,CloseBal,TransDate,TransDescription,Dr,Cr) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}');", cboPeriod.Text.Trim(), cboAcct.Text.Trim(), radioGroup2.EditValue, Convert.ToDouble(txtopenBal.Text.Trim()), Convert.ToDouble(txtCloseBal.Text.Trim()), Convert.ToDateTime(dtEdit.Rows[i]["Date"]).ToString("yyyy/MM/dd"), dtEdit.Rows[i]["Transaction Description"], dtEdit.Rows[i]["Dr"], dtEdit.Rows[i]["Cr"]);

                        //                using (SqlCommand sqlCommand = new SqlCommand(query, db, transaction))
                        //                {
                        //                    sqlCommand.ExecuteNonQuery();
                        //                }
                        //            }



                        //            transaction.Commit();
                        //        }
                        //        catch (Exception ex)
                        //        {
                        //            transaction.Rollback();
                        //            Tripous.Sys.ErrorBox(ex);
                        //        }

                        //        db.Close();
                        //    }
                        #endregion

                        using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
                        {
                            connect.Open();
                            _command = new SqlCommand("InsertUpdatePostRelationTransaction", connect) { CommandType = CommandType.StoredProcedure };
                            _command.Parameters.Add(new SqlParameter("@AccountNo", SqlDbType.VarChar)).Value = cboAcct.Text.Trim();
                            _command.Parameters.Add(new SqlParameter("@Period", SqlDbType.VarChar)).Value = cboPeriod.Text;
                            _command.Parameters.Add(new SqlParameter("@closeBal", SqlDbType.Money)).Value = Convert.ToDouble(txtCloseBal.Text.Trim());
                            _command.Parameters.Add(new SqlParameter("@OpenBal", SqlDbType.Money)).Value = Convert.ToDouble(txtopenBal.Text.Trim());
                            _command.Parameters.Add(new SqlParameter("@Isbool", SqlDbType.Bit)).Value = boolIsUpdate;
                            //_command.Parameters.Add(new SqlParameter("@recEdit", SqlDbType.Int)).Value = ID;
                            _command.Parameters.Add(new SqlParameter("@pTransaction", SqlDbType.Structured)).Value = dtEdit;

                            using (System.Data.DataSet ds = new System.Data.DataSet())
                            {
                                adp = new SqlDataAdapter(_command);
                                adp.Fill(ds);
                                Dts = ds.Tables[0];
                                connect.Close();

                                if (ds.Tables[0].Rows[0]["returnCode"].ToString() != "00")
                                {
                                    Common.setMessageBox(ds.Tables[0].Rows[0]["returnMessage"].ToString(), Program.ApplicationName, 1);
                                    //isError = true;
                                    //goto Map;
                                }
                                else
                                {
                                    Common.setMessageBox(ds.Tables[0].Rows[0]["returnMessage"].ToString(), Program.ApplicationName, 1);
                                }

                            }
                        }
                    }
                }
                ProcessClose();
                //Common.setMessageBox("Transaction Posted Successfully", Program.ApplicationName, 1); return;
            }
            bttnReport.Enabled = true;
            //setReload();
        }

        public void setDBComboBoxRevenue()
        {
            DataTable Dt;

            using (var ds = new System.Data.DataSet())
            {


                using (SqlDataAdapter ada = new SqlDataAdapter((string)"select *  from tblRevenueTemp", Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }

                Dt = ds.Tables[0];
            }

            Common.setComboList(cboRevenue, Dt, "RevenueCode", "Description");

            cboRevenue.SelectedIndex = -1;


        }

        public void setDBComboBoxPay()
        {
            DataTable Dt;

            using (var ds = new System.Data.DataSet())
            {

                using (SqlDataAdapter ada = new SqlDataAdapter((string)"select *  from tblPayMode", Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }

                Dt = ds.Tables[0];
            }


            Common.setComboList(cboPayMode, Dt, "PayID", "description");

            cboPayMode.SelectedIndex = -1;


        }

        public void stDBComboBoxZonal()
        {
            DataTable Dt;

            using (var ds = new System.Data.DataSet())
            {

                using (SqlDataAdapter ada = new SqlDataAdapter((string)"SELECT OfficeCode,OfficeName FROM tblZonalRevenueOfficetemp", Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }

                Dt = ds.Tables[0];
            }


            Common.setComboList(cboZonal, Dt, "OfficeCode", "OfficeName");

            cboZonal.SelectedIndex = -1;
        }

        public void setDBComboBoxAgent()
        {
            DataTable Dt;

            using (var ds = new System.Data.DataSet())
            {


                string query = "select TaxAgentReferenceNumber as [Agent ID], OrganizationName from tblTaxAgent";

                using (SqlDataAdapter ada = new SqlDataAdapter(query, Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }

                Dt = ds.Tables[0];
            }

            //Common.setCheckEdit(luePayeridPerson, Dt, "BankShortCode", "BankName");

            //Common.setLookUpEdit(lupeAgent, Dt, "Agent ID", "OrganizationName");

            //cboBank.SelectedIndex = -1;


        }

        public void setDBComboBoxPerson()
        {
            DataTable Dt;


            using (var ds = new System.Data.DataSet())
            {

                string query = "select TaxPayerReferenceNumber as [Payer ID],Surname + ' '  + Othernames  as fullname from tblTaxPayer";

                using (SqlDataAdapter ada = new SqlDataAdapter(query, Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }

                Dt = ds.Tables[0];
            }

            //Common.setLookUpEdit(luePayeridPerson, Dt, "Payer ID", "fullname");

            //cboBank.SelectedIndex = -1;


        }

        void setDBComboBoxBanks()
        {
            DataTable Dt;

            using (var ds = new System.Data.DataSet())
            {
                using (SqlDataAdapter ada = new SqlDataAdapter((string)"select *  from tblBank", Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }

                Dt = ds.Tables[0];
            }

            Common.setComboList(cboCheque, Dt, "BankShortCode", "BankName");

            cboCheque.SelectedIndex = -1;

        }

        void setDBComboBoxPeriod()
        {
            DataTable Dt;

            using (var ds = new System.Data.DataSet())
            {
                using (SqlDataAdapter ada = new SqlDataAdapter((string)"SELECT Periods + '/'+ Year AS Periods,PeriodId FROM tblPeriods", Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }

                Dt = ds.Tables[0];
            }

            Common.setComboList(cboPeriod, Dt, "PeriodId", "Periods");

            cboPeriod.SelectedIndex = -1;
        }

        void luePayeridPerson_EditValueChanged(object sender, EventArgs e)
        {
            string values = string.Empty;

            object val = luePayeridPerson.EditValue;

            object[] lol = val.ToString().Split(',');

            int i = 0;

            foreach (object obj in lol)
            {
                values += String.Format("{0}", obj.ToString().Trim());

                if (i + 1 < lol.Count())

                    values += ",";

                ++i;

            }

            //txtPayerName.Text =(string) values;

            //txtPayerID.Text = luePayeridPerson.Text;

            txtPayerID.Text = (string)values;

            txtPayerName.Text = luePayeridPerson.Text;

            txtPayerID.Visible = true;

            luePayeridPerson.Visible = false;
        }

        void lupeAgent_EditValueChanged(object sender, EventArgs e)
        {
            string values = string.Empty;

            object val = lupeAgent.EditValue;

            object[] lol = val.ToString().Split(',');

            int i = 0;

            foreach (object obj in lol)
            {
                values += String.Format("{0}", obj.ToString().Trim());

                if (i + 1 < lol.Count())

                    values += ",";

                ++i;

            }

            txtPayerID.Text = (string)values;

            lupeAgent.Visible = false;

            txtPayerID.Visible = true;

            txtPayerName.Text = lupeAgent.Text;
        }

        void cboPaymode_KeyPress(object sender, KeyPressEventArgs e)
        {

            Methods.AutoComplete(cboPayMode, e, true);
        }

        void cboPayMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboPayMode.SelectedValue != null && !isThird)
            {
                //getPayMode(Convert.ToInt32(cboPayMode.SelectedValue.ToString()));
                if (Convert.ToInt32(cboPayMode.SelectedValue.ToString()) == 1)
                {
                    label25.Visible = true;
                    lblchequedate.Visible = true;
                    lblChqueBank.Visible = true;
                    txtChequeNo.Visible = true;
                    cboCheque.Visible = true;
                    dtpCheque.Visible = true;
                    label25.Visible = true;
                }
                else
                {
                    //label20.Visible = false;
                    lblchequedate.Visible = false;
                    lblChqueBank.Visible = false;
                    txtChequeNo.Visible = false;
                    cboCheque.Visible = false;
                    dtpCheque.Visible = false;
                    label25.Visible = false;
                }
            }


        }

        public void setDBChequeBankComboBox()
        {
            DataTable Dt;

            using (var ds = new System.Data.DataSet())
            {

                using (SqlDataAdapter ada = new SqlDataAdapter((string)"select *  from tblBank", Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }

                Dt = ds.Tables[0];
            }

            Common.setComboList(cboCheque, Dt, "BankShortCode", "BankName");

            cboCheque.SelectedIndex = -1;


        }

        private void setReload()
        {

            DataTable dt;

            using (var ds = new System.Data.DataSet())
            {
                using (SqlDataAdapter ada = new SqlDataAdapter("SELECT DISTINCT  Period,AccountNo,CloseBal,OpenBal FROM tblTransactionPosting", Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }

                dt = ds.Tables[0];

                gridControl2.DataSource = dt.DefaultView;
            }

            gridView3.OptionsBehavior.Editable = false;
            //gridView3.Columns["PostID"].Visible = false;
            gridView3.Columns["CloseBal"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            gridView3.Columns["CloseBal"].DisplayFormat.FormatString = "n2";
            gridView3.Columns["OpenBal"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            gridView3.Columns["OpenBal"].DisplayFormat.FormatString = "n2";
            //gridView2.Columns["PostID"].Visible = false;
            //gridView2.Columns["OpenBal"].Visible = false;
            gridView3.BestFitColumns();
        }

        private void getBankCode(string AcctNumber)
        {
            DataTable Dt = null;


            using (var ds = new System.Data.DataSet())
            {
                //connect.connect.Open();
                string query = String.Format(" SELECT  * FROM dbo.ViewBankBranchAccount WHERE AccountNumber = '{0}' ", AcctNumber);

                using (SqlDataAdapter ada = new SqlDataAdapter(query, Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }

                Dt = ds.Tables[0];
            }

            if (Dt != null && Dt.Rows.Count > 0)
            {
                bankcode = (string)Dt.Rows[0]["BankShortCode"];
                cboBank.SelectedValue = (string)Dt.Rows[0]["BankShortCode"];
            }

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
                    ID = (string)dr["Period"];

                    Acctno = (string)dr["AccountNo"];

                    bResponse = FillField((string)dr["Period"], (string)dr["AccountNo"]);

                    //GetPeriodCollection(BankCode, ID);
                    getBankCode((string)dr["AccountNo"]);

                    setReload1((string)dr["Period"], (string)dr["AccountNo"]);

                    cboPeriod.Text = (string)dr["Period"];
                }
                else
                {
                    Common.setMessageBox("No Record is seleected", "", 1);
                    bResponse = false;
                }
            }
            return bResponse;
        }

        private bool FillField(string fieldid, string pmAcct)
        {
            bool bResponse = false;
            //load data from the table into the forms for edit

            DataTable dts = (new Logic()).getSqlStatement((String.Format("select * from tblTransactionPosting where Period ='{0}' and AccountNo ='{1}'", fieldid, pmAcct))).Tables[0];

            if (dts != null)
            {
                bResponse = true;

                cboAcct.Text = dts.Rows[0]["AccountNo"].ToString();

                txtCloseBal.Text = string.Format("{0:n2}", dts.Rows[0]["CloseBal"]);
                txtopenBal.Text = string.Format("{0:n2}", dts.Rows[0]["OpenBal"]);
                txtCloseBal.Enabled = false; txtopenBal.Enabled = false; cboPeriod.Enabled = false; cboAcct.Enabled = false;


            }
            else
                bResponse = false;

            return bResponse;
        }

        private void setReload1(string fieldid, string pmAcct)
        {
            //connect.connect.Close();
            //System.Data.DataSet ds;
            using (var ds = new System.Data.DataSet())
            {
                //connect.connect.Open();
                using (SqlDataAdapter ada = new SqlDataAdapter((String.Format("select CONVERT(VARCHAR, TransDate,103) AS Date ,TransDescription AS [Transaction Description],Dr,Cr  from tblTransactionPosting where Period ='{0}' and AccountNo ='{1}'", fieldid, pmAcct)), Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }
                //connect.connect.Close();
                dtEdit = ds.Tables[0];
                //gridControl1.DataSource = dt.DefaultView;

                //tableTrans = ds.Tables[0];

                gridControl1.DataSource = dtEdit.DefaultView;
            }
            gridView1.OptionsBehavior.Editable = false;

            gridView1.Columns["Dr"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            gridView1.Columns["Dr"].DisplayFormat.FormatString = "n2";
            gridView1.Columns["Cr"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            gridView1.Columns["Cr"].DisplayFormat.FormatString = "n2";
            gridView1.BestFitColumns();
        }

        void GetBalance()
        {
            DataTable dts = (new Logic()).getSqlStatement((String.Format("SELECT CloseBal,OpenBal FROM tblTransactionPosting WHERE Period = '{0}' AND AccountNo= '{1}' ", (string)cboPeriod.Text, (string)cboAcct.Text.Trim()))).Tables[0];

            if (dts != null && dts.Rows.Count > 0)
            {
                txtopenBal.Text = String.Format("{0:N2}", dts.Rows[0]["OpenBal"]);
                txtCloseBal.Text = String.Format("{0:N2}", dts.Rows[0]["CloseBal"]);
                txtCloseBal.Enabled = false; txtopenBal.Enabled = false;
                return;

            }
            else
                txtCloseBal.Text = string.Empty; txtopenBal.Text = string.Empty;
            txtCloseBal.Enabled = true; txtopenBal.Enabled = true;

        }

        private static bool isPeriodClose(string strPeriod, string strAcctnum)
        {
            bool isClose = false;

            string query = String.Format("SELECT COUNT(Period) AS Count FROM tblClosePeriods WHERE Period='{0}' and AccountNo='{1}'", (string)strPeriod, (string)strAcctnum);

            if (new Logic().IsRecordExist(query))
            {
                isClose = true;
            }
            else
            {
                isClose = false;
            }

            return isClose;
        }

        void ClosePeriod()
        {

            //check if transaction period is closed before or not

            string query = String.Format("SELECT COUNT(Period) AS Count FROM tblClosePeriods WHERE Period='{0}' and AccountNo='{1}' ", (string)cboPeriod.Text, (string)cboAcct.Text);

            if (new Logic().IsRecordExist(query))
            {
                Common.setMessageBox("Period Already Close", Program.ApplicationName, 2);
            }
            else
            {
                //now compare the close and open before insert record
                if (Convert.ToDouble(label33.Text) != Convert.ToDouble(txtCloseBal.Text))
                {
                    Common.setMessageBox("Closing Balance for the Transaction Period not Correct", Program.ApplicationName, 3); return;
                }
                else
                {
                    //commit close period into tbale per period and account

                    using (SqlConnection db = new SqlConnection(Logic.ConnectionString))
                    {
                        SqlTransaction transaction;

                        db.Open();

                        transaction = db.BeginTransaction();
                        try
                        {

                            using (SqlCommand sqlCommand = new SqlCommand(String.Format("INSERT INTO [tblClosePeriods]([Period],[AccountNo],[OpenBal] ,[CloseBal],[IsClosed],[Years]) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}');", cboPeriod.Text.Trim(), cboAcct.Text.Trim(), Convert.ToDouble(txtopenBal.Text), Convert.ToDouble(txtCloseBal.Text), 1), db, transaction))
                            {
                                sqlCommand.ExecuteNonQuery();
                            }


                            transaction.Commit();

                        }
                        catch (SqlException sqlError)
                        {
                            transaction.Rollback();
                            Tripous.Sys.ErrorBox(sqlError);
                        }
                        db.Close();
                    }

                    Common.setMessageBox("Transaction Period is Closed", Program.ApplicationName, 1); return;

                }

            }

        }

        void OpenBal()
        {
            DataTable dts = (new Logic()).getSqlStatement((String.Format("SELECT MAX(Period) as Period FROM dbo.tblClosePeriods WHERE AccountNo= '{0}' ", (string)cboAcct.Text.Trim()))).Tables[0];

            if (dts != null)
            {
                if (dts.Rows.Count > 0)
                {
                    //check if records exit for the period
                    object value = dts.Rows[0]["Period"];

                    if (value != DBNull.Value)
                    {

                        //string dbPeriod = (string)dts.Rows[0]["Period"];
                        string[] Split3 = value.ToString().Split(new char[] { '/' });

                        if (string.IsNullOrEmpty(cboPeriod.Text))
                        {
                            //ID
                            split = ID.Split(new Char[] { '/' });
                        }
                        else
                        { split = cboPeriod.Text.Trim().Split(new Char[] { '/' }); }


                        if (Convert.ToDouble(Split3[0]) != Convert.ToDouble(split[0]))
                        {
                            //check if the difference in period months is one and 
                            if ((Convert.ToInt32(Convert.ToDouble(split[0]) - Convert.ToDouble(Split3[0])) == 1) && (Convert.ToInt32(split[1]) == Convert.ToInt32(Split3[1])))
                            {
                                //check get the close balance for perision period

                                DataTable dpers = (new Logic()).getSqlStatement((String.Format("SELECT CloseBal FROM dbo.tblClosePeriods WHERE Period= '{0}' and AccountNo= '{1}' ", (string)value, (string)cboAcct.Text.Trim()))).Tables[0];

                                if (dpers != null && dpers.Rows.Count > 0)
                                {
                                    txtopenBal.Text = String.Format("{0:n2}", dpers.Rows[0]["CloseBal"]);
                                    txtopenBal.Enabled = false; txtCloseBal.Focus();
                                }
                                else
                                { txtopenBal.Enabled = true; txtopenBal.Focus(); }

                            }
                        }
                    }

                }

            }
        }

        public void GetPeriodCollection(string paramBankCode, string paramPeriod)
        {
            DataTable Dt = null;

            label32.Text = string.Empty;

            lblCollect.Text = string.Empty;

            using (var ds = new System.Data.DataSet())
            {
                //connect.connect.Open();
                string query = String.Format("select sum(Amount) as Amount from ViewCollectionAmountBankCode where BankCode LIKE '%{0}%'AND Period= '{1}' and IsRecordExit=1 and IsPayDirect=1 ", paramBankCode, paramPeriod);

                using (SqlDataAdapter ada = new SqlDataAdapter(query, Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }

                Dt = ds.Tables[0];
            }

            decimal totalAmount = 0m;
            if (Dt != null && Dt.Rows.Count > 0)
            {
                label32.Text = String.Format("PayDirect Collection for {0} Period:", paramPeriod);
                //Total Collection Per Period:
                lblCollect.Text = String.Format("{0:N2}", Dt.Rows[0]["Amount"]);

                lblCollect.Text = String.Format("{0:N2}", Dt.Rows[0]["Amount"]);

                bttnDetails.Visible = true; bttnDetails.Enabled = true;
                totalAmount = Convert.ToDecimal(Dt.Rows[0]["Amount"]);
            }
            //return totalAmount;
        }

        void GetAmountNotBank(string paramBankCode, string paramPeriod)
        {
            DataTable Dt = null;


            //lblCollect.Text = string.Empty;

            using (var ds = new System.Data.DataSet())
            {
                //connect.connect.Open();
                string query = String.Format("select sum(Amount) as Amount from ViewCollectionAmountBankCode where BankCode LIKE '%{0}%'AND Period= '{1}' and IsRecordExit=0 and IsPayDirect=1", paramBankCode, paramPeriod);

                using (SqlDataAdapter ada = new SqlDataAdapter(query, Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }

                Dt = ds.Tables[0];
            }

            decimal totalAmount = 0m;
            if (Dt != null && Dt.Rows.Count > 0)
                label9.Text = String.Format("{0:N2}", Dt.Rows[0]["Amount"]);
            else
                label9.Text = string.Empty;
        }

        void GetNotPayCirectcollect(string paramBankCode, string paramPeriod)
        {

            DataTable Dt = null;


            using (var ds = new System.Data.DataSet())
            {
                //connect.connect.Open();
                string query = String.Format("select sum(Amount) as Amount from ViewCollectionAmountBankCode where BankCode LIKE '%{0}%'AND Period= '{1}' and IsRecordExit=1 and IsPayDirect=0", paramBankCode, paramPeriod);

                using (SqlDataAdapter ada = new SqlDataAdapter(query, Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }

                Dt = ds.Tables[0];
            }

            if (Dt != null && Dt.Rows.Count > 0)
                lblpay.Text = String.Format("{0:N2}", Dt.Rows[0]["Amount"]);
            else
                lblpay.Text = string.Empty;

        }

        void ProcessClose()
        {
            //get the summary of dr and cr
            double sumDr = Convert.ToDouble(gridView1.Columns["Dr"].SummaryItem.SummaryValue);
            double sumCr = Convert.ToDouble(gridView1.Columns["Cr"].SummaryItem.SummaryValue);

            //get open & closing balance
            double coll = Convert.ToDouble(lblCollect.Text) + sumCr;
            double SumClose = (coll - sumDr) + Convert.ToDouble(txtopenBal.Text);
            double SumOpen = (sumCr - sumDr) - Convert.ToDouble(txtCloseBal.Text);


            label33.Text = String.Format("{0:N2}", SumClose);
        }

        public void RefreshForm()
        {
            NavBars.ToolStripEnableDisableControls(toolStrip, Tag as String);
        }

    }
}
