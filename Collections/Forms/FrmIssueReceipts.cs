using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TaxSmartSuite;
using TaxSmartSuite.Class;
using System.Data.SqlClient;
using Collection.Classess;
using DevExpress.XtraGrid.Views.Grid;
using System.Globalization;
using DevExpress.XtraSplashScreen;
using Collections;

namespace Collection.Forms
{
    public partial class FrmIssueReceipts : Form
    {
        public static FrmIssueReceipts publicStreetGroup;

        public static FrmIssueReceipts publicInstance;

        protected TransactionTypeCode iTransType;

        protected bool boolIsUpdate;

        protected int ID;

        bool isFirst = true;

        string strQty = string.Empty;

        string strRemainQty = string.Empty;

        string strIssueqty = string.Empty;

        string query = string.Empty;

        Point LoadScreenLocation = Point.Empty;

        int LoaderWidth = 300;

        int LoaderHeight = 75;

        System.Globalization.CultureInfo enGB = new System.Globalization.CultureInfo("en-GB");

        public FrmIssueReceipts()
        {
            //SplashScreenManager.ShowForm(this, typeof(WaitForm1), tkrue, true, false);

            if (SplashScreenManager.Default != null) return;
            LoadScreenLocation = new Point(Cursor.Position.X - Convert.ToInt32(LoaderWidth / 3), Cursor.Position.Y - Convert.ToInt32(LoaderHeight / 2));
            SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, SplashFormStartPosition.Manual, LoadScreenLocation);


            InitializeComponent();

            publicStreetGroup = this;

            publicInstance = this;

            setImages();

            ToolStripEvent();

            iTransType = TransactionTypeCode.New;

            Load += OnFormLoad;

            OnFormLoad(null, null);

            txtQty.LostFocus += txtQty_LostFocus;

            //txtDate.LostFocus += txtDate_LostFocus;

            gridView1.DoubleClick += gridView1_DoubleClick;

            bttnUpdate.Click += bttnUpdate_Click;

            SplashScreenManager.CloseForm(false);
        }

        void gridView1_DoubleClick(object sender, EventArgs e)
        {
            tsbEdit.PerformClick();
        }

        void bttnUpdate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(cboStation.Text))
            {
                Common.setEmptyField(" Station Name ", Program.ApplicationName);
                cboStation.Focus();
            }
            else if (string.IsNullOrEmpty(txtDate.Text))
            {
                Common.setEmptyField(" Issue Date ", Program.ApplicationName);
                txtDate.Focus(); return;
            }
            else if (DateTime.Compare(Convert.ToDateTime(txtDate.Text, enGB), DateTime.Now.Date) > 0)
            {
                Common.setMessageBox(" Issue Date can't be greater than Current Date ", Program.ApplicationName, 1);
                txtDate.Text = null; txtDate.Focus(); return;
            }
            else if (string.IsNullOrEmpty(cboFrom.Text))
            {
                Common.setEmptyField(" Issue From ", Program.ApplicationName);
                cboFrom.Focus(); return;
            }
            else if (string.IsNullOrEmpty(txtQty.Text))
            {
                Common.setEmptyField(" Issue Quantity ", Program.ApplicationName);
                txtQty.Focus(); return;
            }
            else
            {
                string DateString = string.Format("{0:dd/MM/yyyy}", txtDate.Text);

                DateTime temp = DateTime.ParseExact(DateString, "dd-MM-yyyy", CultureInfo.InvariantCulture);

                string str = temp.ToString("yyyy-MM-dd");

                //check if the issue receipt have been used or not
                string sql = String.Format("SELECT COUNT(IssueFrom) AS Count FROM Receipt.tblIssueReceipt WHERE IssueFrom = '{0}' AND IssueTo = '{1}' AND StationCode = '{2}' ", cboFrom.Text, txtIssueTo.Text, cboStation.SelectedValue);

                if (new Logic().IsRecordExist(sql))
                {
                    Common.setMessageBox("Receipt have been Issued before", Program.ApplicationName, 1);
                    return;
                }
                else
                {
                    //check form status either new or editm
                    if (!boolIsUpdate)
                    {
                        using (SqlConnection db = new SqlConnection(Logic.ConnectionString))
                        {
                            SqlTransaction transaction;

                            db.Open();

                            transaction = db.BeginTransaction();
                            //check if the receive qty not equall to issues qty
                            if (strQty.ToString() != txtQty.Text.ToString())
                            {
                                try
                                {
                                    if (Convert.ToInt32(strRemainQty) == Convert.ToInt32(txtQty.Text))
                                    {
                                        query = String.Format("UPDATE Receipt.tblReceiveReceipt SET IsIssue='{0}',IssueQty='{1}' where  ReceiveReceiptID ='{2}'", true, Convert.ToInt32(strRemainQty) + Convert.ToInt32(txtQty.Text), cboFrom.SelectedValue);
                                    }
                                    else
                                    {
                                        query = String.Format("UPDATE Receipt.tblReceiveReceipt SET IssueQty='{0}' where ReceiveReceiptID ='{1}'", txtQty.Text.Trim(), cboFrom.SelectedValue);
                                    }



                                    using (SqlCommand sqlCommand1 = new SqlCommand(query, db, transaction))
                                    {
                                        sqlCommand1.ExecuteNonQuery();
                                    }

                                    transaction.Commit();
                                }
                                catch (Exception ex)
                                {
                                    Tripous.Sys.ErrorBox(ex);
                                    transaction.Rollback();
                                    return;
                                }
                            }

                            if (strQty.ToString() == txtQty.Text.ToString())
                            {
                                //Convert.ToInt32(strIssueqty)+Convert.ToInt32(txtQty.Text);
                                try
                                {
                                    string query = String.Format("UPDATE Receipt.tblReceiveReceipt SET IsIssue='{0}',IssueQty='{1}' where  ReceiveReceiptID ='{2}'", true, txtQty.Text, cboFrom.SelectedValue);

                                    using (SqlCommand sqlCommand1 = new SqlCommand(query, db, transaction))
                                    {
                                        sqlCommand1.ExecuteNonQuery();
                                    }

                                    transaction.Commit();
                                }
                                catch (Exception ex)
                                {
                                    Tripous.Sys.ErrorBox(ex);
                                    transaction.Rollback();
                                    return;
                                }
                            }
                            db.Close();
                        }

                        using (SqlConnection db = new SqlConnection(Logic.ConnectionString))
                        {
                            SqlTransaction transaction;

                            db.Open();

                            transaction = db.BeginTransaction();

                            try
                            {
                                string querys =
                                    String.Format(
                                        "INSERT INTO Receipt.tblIssueReceipt([IssueDate],[IssueFrom],[IssueTo],[StationCode],[IssueQty],[UsedQty],[UploadStatus]) VALUES ('{0}','{1}','{2}','{3}','{4}','{5}','{6}');", str, cboFrom.Text.Trim(), txtIssueTo.Text.Trim(), cboStation.SelectedValue, txtQty.Text.Trim(), 0, 0);

                                using (SqlCommand sqlCommand2 = new SqlCommand(querys, db, transaction))
                                {
                                    sqlCommand2.ExecuteNonQuery();
                                }

                                transaction.Commit();
                            }
                            catch (Exception ex)
                            {
                                Tripous.Sys.ErrorBox(ex);
                                transaction.Rollback();
                                return;
                            }
                            db.Close();
                        }
                        setReload(cboStation.SelectedValue.ToString());
                        //

                        Common.setMessageBox("Record has been Successfully Added", Program.ApplicationName, 1);

                        if (MessageBox.Show("Do you want to Add New record?", Program.ApplicationName, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
                        {
                            Clear(); tsbReload.PerformClick();
                        }
                        else
                        {

                            Clear(); txtDate.Focus();
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
                                string query = String.Format("UPDATE Receipt.tblIssueReceipt SET [IssueDate]='{0}',[IssueFrom]='{1}',[IssueTo]='{2}',[IssueQty]='{3}',[UploadStatus]=0 where  [StationCode] ='{4}' and IssueReceiptID ='{5}' ", str, cboFrom.Text.Trim(), txtIssueTo.Text.Trim(), txtQty.Text.Trim(), cboStation.SelectedValue, ID);

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
                            }
                            db.Close();
                        }
                        setReload(cboStation.SelectedValue.ToString());
                        Common.setMessageBox("Changes in Record has been Successfully Saved.", Program.ApplicationName, 1);
                        //setDBComboBoxCentre(cboStation.SelectedValue.ToString());
                        Clear();
                        tsbReload.PerformClick();
                    }
                }
            }
        }

        void Clear()
        {
            txtIssueTo.Clear(); txtQty.Clear(); txtDate.Text = null;
            setDBComboxFrom();
            //setDBComboBoxCentre(cboStation.SelectedValue.ToString());
            setDBComboBox();
        }

        void txtQty_LostFocus(object sender, EventArgs e)
        {
            int receive;

            if (string.IsNullOrEmpty(cboFrom.Text)) return;

            if (!Logic.IsNumber((txtQty.Text)))
            {
                Common.setMessageBox("Issue Quantity can only be in number values ", Program.ApplicationName, 2);
                txtQty.Clear(); txtQty.Focus(); return;
            }
            else
            {
                string query = String.Format("SELECT Qty,RemainQty,ReceiveTo,IssueQty FROM Receipt.tblReceiveReceipt WHERE [ReceiveReceiptID]='{0}'", cboFrom.SelectedValue);

                DataTable Dt = (new Logic()).getSqlStatement(query).Tables[0];

                if (Dt != null && Dt.Rows.Count > 0)
                {
                    strQty = String.Format("{0}", Dt.Rows[0]["Qty"]);

                    strRemainQty = String.Format("{0}", Dt.Rows[0]["RemainQty"]);

                    strIssueqty = String.Format("{0}", Dt.Rows[0]["IssueQty"]);

                    int leng = cboFrom.Text.Length;
                    //check if the issueqty and receiveqty
                    if (Convert.ToInt32(txtQty.Text) > Convert.ToInt32(strQty))
                    {
                        Common.setMessageBox(" Issue Quantity is greater than Stock available of " + strQty, Program.ApplicationName, 2);
                        txtQty.Clear();
                        txtQty.Focus();
                        return;
                    }
                    else if (Convert.ToInt32(txtQty.Text) == Convert.ToInt32(strQty))
                    {
                        int issueT = Convert.ToInt32(cboFrom.Text);
                        int issueTn = Convert.ToInt32(issueT) + Convert.ToInt32(txtQty.Text) - 1;
                        txtIssueTo.Text = Convert.ToString(issueTn.ToString().PadLeft(leng, '0'));
                    }
                    else if (Convert.ToInt32(txtQty.Text) <= Convert.ToInt32(strQty))
                    {
                        //chech the remain balance of the receipt

                        if (Convert.ToInt32(strRemainQty) == 0)
                        {
                            int issueT = Convert.ToInt32(cboFrom.Text) + Convert.ToInt32(txtQty.Text) - 1;

                            if (Convert.ToInt32(Dt.Rows[0]["ReceiveTo"]) != issueT)
                            {
                                int issueTs = Convert.ToInt32(Dt.Rows[0]["ReceiveTo"]) - issueT;

                                int issueTn = Convert.ToInt32(cboFrom.Text) + Convert.ToInt32(txtQty.Text) - 1 + issueTs;
                                txtIssueTo.Text = Convert.ToString(issueTn.ToString().PadLeft(leng, '0'));
                            }
                            else
                                txtIssueTo.Text = Convert.ToString(issueT.ToString().PadLeft(leng, '0'));
                        }
                        else if (Convert.ToInt32(strRemainQty) != 0)
                        {
                            if (boolIsUpdate)
                            {
                                int issueT = Convert.ToInt32(cboFrom.Text) + Convert.ToInt32(txtQty.Text) - 1;
                                txtIssueTo.Text = Convert.ToString(issueT.ToString().PadLeft(leng, '0'));
                            }
                            else
                            {
                                if (Convert.ToInt32(txtQty.Text) > Convert.ToInt32(strRemainQty))
                                {
                                    Common.setMessageBox(" Remaining quantity in this batch is " + strRemainQty, Program.ApplicationName, 2);
                                    txtQty.Clear();
                                    txtQty.Focus();
                                    return;
                                }
                                else if (Convert.ToInt32(txtQty.Text) < Convert.ToInt32(strRemainQty))
                                {
                                    int issueT = Convert.ToInt32(cboFrom.Text) + Convert.ToInt32(txtQty.Text) - 1;
                                    txtIssueTo.Text = Convert.ToString(issueT.ToString().PadLeft(leng, '0'));
                                }
                                else
                                {
                                    int intFromto = Convert.ToInt32(cboFrom.Text) + Convert.ToInt32(Dt.Rows[0]["IssueQty"]) ;
                                    cboFrom.Text = Convert.ToString(intFromto.ToString().PadLeft(leng, '0'));
                                    int issueT = Convert.ToInt32(cboFrom.Text) + Convert.ToInt32(txtQty.Text) - 1;
                                    txtIssueTo.Text = Convert.ToString(issueT.ToString().PadLeft(leng, '0'));
                                }
                            }

                        }


                    }

                }
            }


        }

        void OnFormLoad(object sender, EventArgs e)
        {
            ShowForm();

            setDBComboBox();

            setDBComboxFrom();

            isFirst = false;

            cboStation.SelectedIndexChanged += cboStation_SelectedIndexChanged;

            cboStation_SelectedIndexChanged(null, null);
        }

        void cboStation_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboStation.SelectedValue != null && !isFirst)
            {
                setReload(cboStation.SelectedValue.ToString());

                //setDBComboBoxCentre(cboStation.SelectedValue.ToString());
            }
        }

        void setDBComboBox()
        {

            DataTable Dt;

            using (var ds = new System.Data.DataSet())
            {
                //using (SqlDataAdapter ada = new SqlDataAdapter((string)"SELECT StationCode,StationName FROM tblStation", Logic.ConnectionString))
                using (SqlDataAdapter ada = new SqlDataAdapter((string)"SELECT StationCode,StationName FROM Receipt.tblStation", Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }

                Dt = ds.Tables[0];
            }

            Common.setComboList(cboStation, Dt, "StationCode", "StationName");

            cboStation.SelectedIndex = -1;

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
                    splitContainer1.Panel2Collapsed = false;
                    //Lockfield();
                    break;
                case TransactionTypeCode.Edit:
                    splitContainer1.Panel1Collapsed = false;
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

        private void setImages()
        {
            tsbNew.Image = MDIMain.publicMDIParent.i16x16.Images[5];
            tsbEdit.Image = MDIMain.publicMDIParent.i16x16.Images[6];
            tsbDelete.Image = MDIMain.publicMDIParent.i16x16.Images[7];
            tsbReload.Image = MDIMain.publicMDIParent.i16x16.Images[0];
            tsbClose.Image = MDIMain.publicMDIParent.i16x16.Images[11];

            //bttnCancel.Image = MDIMain.publicMDIParent.i32x32.Images[9];
            //bttnReset.Image = MDIMain.publicMDIParent.i32x32.Images[8];
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
                ShowForm(); cboStation.Focus(); boolIsUpdate = false;
            }
            else if (sender == tsbEdit)
            {
                groupControl2.Text = "Edit Record Mmode";
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
                    if (string.IsNullOrEmpty(ID.ToString()))
                    {
                        Common.setMessageBox("No Record Selected for Delete", "Delete Record", 1);
                        return;
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
                                using (SqlCommand sqlCommand2 = new SqlCommand(string.Format("DELETE FROM tblIssueReceipt WHERE IssueReceiptID='{0}'", ID), db, transaction))
                                {
                                    sqlCommand2.ExecuteNonQuery();
                                }

                                transaction.Commit();
                            }
                            catch (Exception ex)
                            {
                                transaction.Rollback();
                                Tripous.Sys.ErrorBox(ex);
                            }
                        }
                    }

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

        void setDBComboxFrom()
        {

            DataTable Dt;

            using (var ds = new System.Data.DataSet())
            {
                using (SqlDataAdapter ada = new SqlDataAdapter("SELECT ReceiveReceiptID,ReceiveFrom FROM Receipt.tblReceiveReceipt WHERE IsIssue=0 AND ReceiveFrom NOT IN (SELECT IssueFrom FROM Receipt.tblIssueReceipt)", Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }

                Dt = ds.Tables[0];
            }

            Common.setComboList(cboFrom, Dt, "ReceiveReceiptID", "ReceiveFrom");

            cboFrom.SelectedIndex = -1;
        }

        private void setReload(string parameter)
        {
            DataTable dt;

            using (var ds = new System.Data.DataSet())
            {
                //CONVERT(VARCHAR,CONVERT(DATEtime,[PaymentDate]),103) AS PaymentDate

                //string qury = String.Format("SELECT IssueReceiptID,CentreCode,CONVERT(VARCHAR(10),IssueDate, 103) AS IssueDate,IssueFrom,IssueTo,IssueQty,UsedQty,StationCode,(SELECT StationName FROM tblStation WHERE StationCode=tblIssueReceipt.StationCode) AS StationName from tblIssueReceipt where StationCode = '{0}' AND UploadStatus IS NULL OR UploadStatus='Pending' ", parameter);

                string qury = String.Format("SELECT IssueReceiptID,CONVERT(VARCHAR(10),IssueDate, 105) AS IssueDate,IssueFrom,IssueTo,IssueQty,UsedQty,StationCode,(SELECT StationName FROM Receipt.tblStation WHERE StationCode=Receipt.tblIssueReceipt.StationCode) AS StationName from Receipt.tblIssueReceipt where StationCode = '{0}' AND UploadStatus=0 ", parameter);

                using (SqlDataAdapter ada = new SqlDataAdapter(qury, Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }

                dt = ds.Tables[0];

                gridControl1.DataSource = dt.DefaultView;
            }

            gridView1.OptionsBehavior.Editable = false;

            //gridView1.Columns["CentreCode"].Visible = false;

            gridView1.Columns["StationCode"].Visible = false;

            gridView1.Columns["IssueReceiptID"].Visible = false;

            gridView1.Columns["UsedQty"].Visible = false;

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
                    ID = Convert.ToInt32(dr["IssueReceiptID"]);
                    bResponse = FillField(Convert.ToInt32(dr["IssueReceiptID"]));
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

            DataTable dts = (new Logic()).getSqlStatement((String.Format(" SELECT CONVERT(VARCHAR,CONVERT(DATEtime,[IssueDate]),105) AS IssueDate,IssueFrom,IssueTo,IssueQty,StationCode,StationName,ReceiveReceiptID from ViewReceiptStation where IssueReceiptID ='{0}'", fieldid))).Tables[0];

            if (dts != null && dts.Rows.Count > 0)
            {
                bResponse = true;
                txtDate.Text = dts.Rows[0]["IssueDate"].ToString();
                cboStation.Text = dts.Rows[0]["StationName"].ToString();
                cboStation.SelectedValue = dts.Rows[0]["StationCode"].ToString();
                txtQty.Text = dts.Rows[0]["IssueQty"].ToString();
                txtIssueTo.Text = dts.Rows[0]["IssueTo"].ToString();
                cboFrom.Text = dts.Rows[0]["IssueFrom"].ToString();
                cboFrom.SelectedValue = dts.Rows[0]["ReceiveReceiptID"].ToString();
                //cboCentre.SelectedValue = dts.Rows[0]["CentreCode"].ToString();
                //cboCentre.Text = dts.Rows[0]["CentreName"].ToString();
            }
            else
                bResponse = false;

            return bResponse;
        }

        void setDBComboBoxCentre(string parameter)
        {
            DataTable Dt;

            using (var ds = new System.Data.DataSet())
            {
                //string query = "SELECT CentreCode,CentreName FROM dbo.tblReceiptPrintingCentre WHERE StationCode";
                string query = String.Format("SELECT CentreCode,CentreName FROM dbo.tblReceiptPrintingCentre where StationCode= '{0}' ", parameter);
                using (SqlDataAdapter ada = new SqlDataAdapter(query, Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }

                Dt = ds.Tables[0];
            }

            Common.setComboList(cboCentre, Dt, "CentreCode", "CentreName");

            cboCentre.SelectedIndex = -1;
        }

        public void RefreshForm()
        {
            NavBars.ToolStripEnableDisableControls(toolStrip, Tag as String);
        }


    }
}
