using Collection.Classess;
using DevExpress.Utils;
using DevExpress.XtraGrid.Views.Grid;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Windows.Forms;
using TaxSmartSuite.Class;

namespace Collection.Forms
{
    public partial class FrmReceiveRec : Form
    {
        public static FrmReceiveRec publicStreetGroup;

        public static FrmReceiveRec publicInstance;

        protected TransactionTypeCode iTransType;

        protected bool boolIsUpdate;

        protected int ID;

        bool isFirst = true;

        System.Globalization.CultureInfo enGB = new System.Globalization.CultureInfo("en-GB");

        public FrmReceiveRec()
        {
            InitializeComponent();

            publicStreetGroup = this;

            publicInstance = this;

            setImages();

            ToolStripEvent();

            iTransType = TransactionTypeCode.Null;

            Load += OnFormLoad;

            txtFrom.Leave += txtFrom_Leave;

            txtTo.Leave += txtTo_Leave;

            bttnUpdate.Click += bttnUpdate_Click;

            OnFormLoad(null, null);

            gridView1.DoubleClick += gridView1_DoubleClick;

        }

        void txtTo_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtTo.Text))
            {
                Common.setEmptyField(" Receive To ", Program.ApplicationName);
                txtTo.Focus(); return;
            }
            else if (!Logic.IsNumber((txtTo.Text)))
            {
                Common.setMessageBox("Received To can only be number values ", Program.ApplicationName, 2);
                txtTo.Clear(); txtTo.Focus(); return;
            }
            else if (txtFrom.Text.Length != txtTo.Text.Length)//check if numberr length match each other
            {
                Common.setMessageBox(" Control number range does not match ", Program.ApplicationName, 2);
                txtTo.Clear();
                return;
            }
            else
            {

                string retval = Right(txtFrom.Text, 1);

                //MessageBox.Show(retval);

                if (Convert.ToInt32(retval) == 1)
                {
                    txtQty.Text = Convert.ToString((Convert.ToInt32(txtTo.Text) - Convert.ToInt32(txtFrom.Text)) + 1);
                }
                else if (Convert.ToInt32(txtTo.Text) == Convert.ToInt32(txtFrom.Text))
                {
                    txtQty.Text = Convert.ToString((Convert.ToInt32(txtTo.Text) - Convert.ToInt32(txtFrom.Text)) + 1);
                }
                else if (Convert.ToInt32(retval) == 0)
                {
                    txtQty.Text = Convert.ToString(Convert.ToInt32(txtTo.Text) - Convert.ToInt32(txtFrom.Text));
                }
                else
                {
                    txtQty.Text = Convert.ToString((Convert.ToInt32(txtTo.Text) - Convert.ToInt32(txtFrom.Text)) + 1);
                    //txtQty.Text = Convert.ToString(Convert.ToInt32(txtTo.Text) - Convert.ToInt32(txtFrom.Text));
                }

            }
        }

        void txtFrom_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtFrom.Text))
            {
                Common.setEmptyField(" Received From ", Program.ApplicationName);
                txtFrom.Focus();
                return;
            }
            if (!Logic.IsNumber((txtFrom.Text)))
            {
                Common.setMessageBox("Received From can only be number values ", Program.ApplicationName, 2);
                txtFrom.Clear(); txtFrom.Focus(); return;
            }
        }

        void gridView1_DoubleClick(object sender, EventArgs e)
        {
            tsbEdit.PerformClick();
        }

        void bttnUpdate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtDate.Text))
            {
                Common.setEmptyField("Receive Date ", Program.ApplicationName);
                txtDate.Focus(); return;
            }
            else if (txtFrom.Text.Length != txtTo.Text.Length)//check if the number length match each other
            {
                Common.setMessageBox("Control number range does not match ", Program.ApplicationName, 2);
                return;
            }
            else
            {

                string DateString = string.Format("{0:dd/MM/yyyy}", txtDate.Text);

                DateTime temp = DateTime.ParseExact(DateString, "dd-MM-yyyy", CultureInfo.InvariantCulture);

                string str = temp.ToString("yyyy-MM-dd");

                //check form status either new or edit
                if (!boolIsUpdate)
                {
                    if (!CheckReceivecReprt(txtFrom.Text.Trim()))
                    {
                        using (SqlConnection db = new SqlConnection(Logic.ConnectionString))
                        {
                            SqlTransaction transaction;

                            db.Open();

                            transaction = db.BeginTransaction();
                            try
                            {

                                //using (SqlCommand sqlCommand1 = new SqlCommand(String.Format("INSERT INTO [tblReceiveReceipt]([Date],[ReceiveFrom],[ReceiveTo],[Qty],IssueQty,RemainQty)VALUES ('{0}','{1}','{2}','{3}','{4}','{5}');", Convert.ToDateTime(txtDate.Text, enGB), txtFrom.Text.Trim(), txtTo.Text.Trim(), txtQty.Text.Trim(), 0, 0), db, transaction))
                                using (SqlCommand sqlCommand1 = new SqlCommand(String.Format("INSERT INTO Receipt.tblReceiveReceipt([Date],[ReceiveFrom],[ReceiveTo],[Qty],IssueQty)VALUES ('{0}','{1}','{2}','{3}','{4}');", str, txtFrom.Text.Trim(), txtTo.Text.Trim(), txtQty.Text.Trim(), 0), db, transaction))
                                {
                                    sqlCommand1.ExecuteNonQuery();
                                }

                                transaction.Commit();
                            }
                            catch (SqlException sqlError)
                            {
                                Tripous.Sys.ErrorBox(sqlError);
                                transaction.Rollback();
                                return;
                            }
                            db.Close();
                        }
                        setReload();
                        Common.setMessageBox("Record has been Successfully Added", Program.ApplicationName, 1);

                        if (MessageBox.Show("Do you want to Add New record?", Program.ApplicationName, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
                        {
                            tsbReload.PerformClick();
                        }
                        else
                        {
                            setReload(); Clear(); txtDate.Focus();
                        }
                    }
                    else
                    {
                        Common.setMessageBox("Receipt Received Already Exist. Please Check it and Redo", Program.ApplicationName, 1);
                        setReload(); Clear(); txtDate.Focus();
                    }


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

                            using (SqlCommand sqlCommand1 = new SqlCommand(String.Format("UPDATE Receipt.tblReceiveReceipt SET [Date]='{0}',[ReceiveFrom]='{1}',[ReceiveTo]='{2}',[Qty]='{3}',IssueQty='0' where  ReceiveReceiptID ='{4}'", str, txtFrom.Text.Trim(), txtTo.Text.Trim(), txtQty.Text.Trim(), ID), db, transaction))
                            {
                                sqlCommand1.ExecuteNonQuery();
                            }

                            transaction.Commit();
                        }
                        catch (SqlException sqlError)
                        {
                            Tripous.Sys.ErrorBox(sqlError.Message);
                            transaction.Rollback();
                            return;
                        }
                        db.Close();
                    }

                    setReload();
                    Common.setMessageBox("Changes in Record has been Successfully Saved.", Program.ApplicationName, 1);
                    setReload();
                    tsbReload.PerformClick();
                }
            }
        }

        void Clear()
        {
            txtTo.Clear(); txtFrom.Clear(); txtDate.Text = null; txtQty.Clear();
        }

        void txtFrom_LostFocus(object sender, EventArgs e)
        {

        }

        void txtTo_LostFocus(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtTo.Text))
            {
                Common.setEmptyField(" Receive To ", Program.ApplicationName);
                txtTo.Focus(); return;
            }
            else if (!Logic.IsNumber((txtTo.Text)))
            {
                Common.setMessageBox("Received To can only be number values ", Program.ApplicationName, 2);
                txtTo.Clear(); txtTo.Focus(); return;
            }
            else if (txtFrom.Text.Length != txtTo.Text.Length)//check if numberr length match each other
            {
                Common.setMessageBox(" Control number range does not match ", Program.ApplicationName, 2);
                txtTo.Clear();
                return;
            }
            else
            {

                string retval = Right(txtFrom.Text, 1);

                //MessageBox.Show(retval);

                if (Convert.ToInt32(retval) == 1)
                {
                    txtQty.Text = Convert.ToString((Convert.ToInt32(txtTo.Text) - Convert.ToInt32(txtFrom.Text)) + 1);
                }
                else if (Convert.ToInt32(txtTo.Text) == Convert.ToInt32(txtFrom.Text))
                {
                    txtQty.Text = Convert.ToString((Convert.ToInt32(txtTo.Text) - Convert.ToInt32(txtFrom.Text)) + 1);
                }
                else if (Convert.ToInt32(retval) == 0)
                {
                    txtQty.Text = Convert.ToString(Convert.ToInt32(txtTo.Text) - Convert.ToInt32(txtFrom.Text));
                }
                else
                {
                    txtQty.Text = Convert.ToString((Convert.ToInt32(txtTo.Text) - Convert.ToInt32(txtFrom.Text)) + 1);
                    //txtQty.Text = Convert.ToString(Convert.ToInt32(txtTo.Text) - Convert.ToInt32(txtFrom.Text));
                }

            }
        }

        //private void textBox3_TextChanged(object sender, EventArgs e)
        //{

        //}

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

        public void RefreshForm()
        {
            NavBars.ToolStripEnableDisableControls(toolStrip, Tag as String);
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
                txtDate.Focus();
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

        public static string Left(string param, int length)
        {
            //we start at 0 since we want to get the characters starting from the
            //left and with the specified lenght and assign it to a variable
            string result = param.Substring(0, length);
            //return the result of the operation
            return result;
        }

        public static string Right(string param, int length)
        {
            //start at the index based on the lenght of the sting minus
            //the specified lenght and assign it a variable
            string result = param.Substring(param.Length - length, length);
            //return the result of the operation
            return result;
        }

        private void setReload()
        {

            DataTable dt;

            try
            {
                using (var ds = new System.Data.DataSet())
                {
                    //connect.connect.Open();

                    //using (SqlDataAdapter ada = new SqlDataAdapter("SELECT ReceiveReceiptID,CONVERT(VARCHAR(10), [Date], 103) AS Date,ReceiveFrom,ReceiveTo,Qty FROM tblReceiveReceipt", Logic.ConnectionString))
                    using (SqlDataAdapter ada = new SqlDataAdapter("SELECT ReceiveReceiptID,CONVERT(VARCHAR,CONVERT(DATEtime,[Date]),105)  AS Date,ReceiveFrom,ReceiveTo,Qty FROM Receipt.tblReceiveReceipt", Logic.ConnectionString))
                    {
                        ada.Fill(ds, "table");
                    }
                    dt = ds.Tables[0];
                    gridControl1.DataSource = dt.DefaultView;
                }
                gridView1.OptionsBehavior.Editable = false;
                gridView1.Columns["ReceiveReceiptID"].Visible = false;
                gridView1.Columns["Date"].DisplayFormat.FormatType = FormatType.DateTime;
                gridView1.Columns["Date"].DisplayFormat.FormatString = "dd-MM-yyyy";
                gridView1.BestFitColumns();
            }
            catch (Exception ex)
            {
                Tripous.Sys.ErrorBox(ex);
                return;
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
                    ID = Convert.ToInt32(dr["ReceiveReceiptID"]);
                    bResponse = FillField(Convert.ToInt32(dr["ReceiveReceiptID"]));
                }
                else
                {
                    Common.setMessageBox(" No Record is seleected ", "", 1);
                    bResponse = false;
                }
            }
            return bResponse;
        }

        private bool FillField(int fieldid)
        {
            bool bResponse = false;
            //load data from the table into the forms for edit

            DataTable dts = (new Logic()).getSqlStatement((String.Format("SELECT CONVERT(VARCHAR,CONVERT(DATEtime,[Date]),105) AS Date,ReceiveFrom,ReceiveTo,Qty FROM Receipt.tblReceiveReceipt where ReceiveReceiptID ='{0}'", fieldid))).Tables[0];

            if (dts != null)
            {
                bResponse = true;
                txtDate.Text = dts.Rows[0]["Date"].ToString();
                txtFrom.Text = dts.Rows[0]["ReceiveFrom"].ToString();
                txtTo.Text = dts.Rows[0]["ReceiveTo"].ToString();
                txtQty.Text = dts.Rows[0]["Qty"].ToString();

            }
            else
                bResponse = false;

            return bResponse;
        }

        //public static DateTime DateParse(string date)
        //{
        //    date = date.Trim();
        //    if (!string.IsNullOrEmpty(date))
        //        return DateTime.Parse(date, new System.Globalization.CultureInfo("fr-FR"));
        //    return new DateTime();
        //}

        bool CheckReceivecReprt(string ReceieveFrom)
        {
            bool bResponse = false;
            string SQL = string.Format("SELECT count(*) FROM   Receipt.tblReceiveReceipt WHERE(ReceiveFrom = '{0}')", ReceieveFrom);

            if (new Classess.Logic().IsRecordExist(SQL))
            {
                bResponse = true;
            }
            return bResponse;

        }

    }
}
