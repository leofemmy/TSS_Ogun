using Collection.Classess;
using DevExpress.XtraGrid.Views.Grid;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using TaxSmartSuite.Class;

namespace Collection.Forms
{
    public partial class FrmReceiptsCentre : Form
    {
        public static FrmReceiptsCentre publicStreetGroup;

        public static FrmReceiptsCentre publicInstance;

        protected TransactionTypeCode iTransType;

        protected bool boolIsUpdate;

        protected int ID;

        bool isFirst = true;

        public FrmReceiptsCentre()
        {
            InitializeComponent();

            publicStreetGroup = this;

            publicInstance = this;

            setImages();

            ToolStripEvent();

            iTransType = TransactionTypeCode.New;

            Load += OnFormLoad;

            OnFormLoad(null, null);

            bttnUpdate.Click += bttnUpdate_Click;

            gridView1.DoubleClick += gridView1_DoubleClick;
        }

        void gridView1_DoubleClick(object sender, EventArgs e)
        {
            tsbEdit.PerformClick();
        }

        void bttnUpdate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtCentreName.Text))
            {
                Common.setEmptyField(" Centre Name ", Program.ApplicationName);
                txtCentreName.Focus(); return;
            }
            else if (Logic.IsNumber(txtCentreName.Text))
            {
                Common.setMessageBox("Centre name can only contain character", Program.ApplicationName, 1);
                txtCentreName.Clear(); txtCentreName.Focus(); return;
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
                            using (SqlCommand sqlCommand1 = new SqlCommand(String.Format("INSERT INTO [tblReceiptPrintingCentre]([CentreName],[StationCode]) VALUES ('{0}','{1}');", txtCentreName.Text.ToUpper(), cboStation.SelectedValue), db, transaction))
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
                        db.Close();
                    }

                    setReload(cboStation.SelectedValue.ToString());

                    Common.setMessageBox("Record has been Successfully Added", Program.ApplicationName, 1);

                    if (MessageBox.Show("Do you want to Add New record?", Program.ApplicationName, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
                    {
                        txtCentreName.Clear();
                        tsbReload.PerformClick();
                    }
                    else
                    {
                        txtCentreName.Clear(); setDBComboBox(); txtCentreName.Focus();
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
                            string query = String.Format("UPDATE [tblReceiptPrintingCentre] SET [CentreName]='{0}' where  [StationCode] ='{1}' and ReceiptCentreID ='{2}' ", txtCentreName.Text.ToUpper(), cboStation.SelectedValue, ID);

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
                    //setReload();
                    txtCentreName.Clear(); txtCentreName.Focus();
                    tsbReload.PerformClick();
                }
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

        void OnFormLoad(object sender, EventArgs e)
        {
            ShowForm();

            setDBComboBox();

            isFirst = false;

            cboStation.SelectedIndexChanged += cboStation_SelectedIndexChanged;

            cboStation_SelectedIndexChanged(null, null);
        }

        void setDBComboBox()
        {

            DataTable Dt;

            using (var ds = new System.Data.DataSet())
            {
                string query = "SELECT StationCode,StationName FROM tblStation";

                using (SqlDataAdapter ada = new SqlDataAdapter(query, Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }

                Dt = ds.Tables[0];
            }

            Common.setComboList(cboStation, Dt, "StationCode", "StationName");

            //cboStation.SelectedIndex = -1;

        }

        void cboStation_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboStation.SelectedValue != null && !isFirst)
            {
                txtCentreName.Clear();
                setReload(cboStation.SelectedValue.ToString());
            }
        }

        private void setReload(string parameter)
        {
            DataTable dt;

            using (var ds = new System.Data.DataSet())
            {
                //CONVERT(VARCHAR,CONVERT(DATEtime,[PaymentDate]),103) AS PaymentDate
                using (SqlDataAdapter ada = new SqlDataAdapter(String.Format("SELECT  ReceiptCentreID,CentreName,CentreCode,StationCode,StationName FROM ViewStationReceiptCentre where StationCode= '{0}' ", parameter), Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }

                dt = ds.Tables[0];

                gridControl1.DataSource = dt.DefaultView;
            }

            gridView1.OptionsBehavior.Editable = false;

            gridView1.Columns["ReceiptCentreID"].Visible = false;

            //gridView1.Columns["UsedQty"].Visible = false;

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
                    ID = Convert.ToInt32(dr["ReceiptCentreID"]);

                    bResponse = FillField(Convert.ToInt32(dr["ReceiptCentreID"]));
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
            ////DataTable dts = extMethods.LoadData(String.Format("select * from ViewAgencyZone where ZoneID ='{0}'", fieldid));

            DataTable dts = (new Logic()).getSqlStatement((String.Format(" SELECT CentreName,CentreCode,StationCode,StationName FROM ViewStationReceiptCentre where ReceiptCentreID ='{0}'", fieldid))).Tables[0];

            if (dts != null)
            {
                bResponse = true;
                //txtDate.Text = dts.Rows[0]["IssueDate"].ToString();
                cboStation.Text = dts.Rows[0]["StationName"].ToString();
                cboStation.SelectedValue = dts.Rows[0]["StationCode"].ToString();
                txtCentreName.Text = dts.Rows[0]["CentreName"].ToString();
                //txtIssueTo.Text = dts.Rows[0]["IssueTo"].ToString();
                //cboFrom.Text = dts.Rows[0]["IssueFrom"].ToString();
                //cboFrom.SelectedValue = dts.Rows[0]["ReceiveReceiptID"].ToString();
            }
            else
                bResponse = false;

            return bResponse;
        }

        private void radioGroup1_Properties_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        public void RefreshForm()
        {
            NavBars.ToolStripEnableDisableControls(toolStrip, Tag as String);
        }



    }
}
