using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using TaxSmartSuite;
using System.Windows.Forms;
using System.Data.SqlClient;
using Collection.Classess;
using TaxSmartSuite.Class;
using DevExpress.XtraGrid.Selection;

namespace Collection.Forms
{
    public partial class FrmRevException : Form
    {
        private DataTable Dt;

        GridCheckMarksSelection selection;

        public static FrmRevException publicInstance;
        
        public static FrmRevException publicStreetGroup;

        protected TransactionTypeCode iTransType;

        protected bool boolIsUpdate;

        protected string ID, stationcode;

        bool isFirstGrid = true;

        bool isFirst = true;

        DataTable dt, dts;

        object objRevCode;


        public FrmRevException()
        {
            InitializeComponent();

            publicStreetGroup = this;

            setImages();

            ToolStripEvent();

            Load += OnFormLoad;

            iTransType = TransactionTypeCode.New;

            OnFormLoad(null, null);

            cboAgency.SelectedIndexChanged += cboAgency_SelectedIndexChanged;

            bttnUpdate.Click += bttnUpdate_Click;

        }

        void bttnUpdate_Click(object sender, EventArgs e)
        {
            //if (selection.SelectedCount < 1)
            //{
            //    Common.setMessageBox("Please, select Revenue Code ", Program.ApplicationName, 3); return;
            //}
            //else
            //{
            //DELETE record from table by agancycode
            using (SqlConnection db = new SqlConnection(Logic.ConnectionString))
            {
                SqlTransaction transaction;

                SqlCommand sqlCommand1;

                db.Open();

                transaction = db.BeginTransaction();

                string querys = String.Format("DELETE FROM Receipt.tblRevenueReceiptException WHERE AgencyCode='{0}'", cboAgency.SelectedValue);

                sqlCommand1 = new SqlCommand(querys, db, transaction);
                sqlCommand1.ExecuteNonQuery();


                try
                {
                    transaction.Commit();
                    db.Close();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Tripous.Sys.ErrorBox(ex);
                    return;
                }
            }
            //insert new record into the database
            using (SqlConnection db = new SqlConnection(Logic.ConnectionString))
            {
                SqlTransaction transaction;

                SqlCommand sqlCommand1;

                db.Open();

                transaction = db.BeginTransaction();

                for (int i = 0; i < selection.SelectedCount; i++)
                {
                    string lol = ((selection.GetSelectedRow(i) as DataRowView)["RevenueCode"].ToString());

                    string query = String.Format("INSERT INTO Receipt.tblRevenueReceiptException ( RevenueCode,AgencyCode ) VALUES ('{0}','{1}')", lol, cboAgency.SelectedValue);

                    sqlCommand1 = new SqlCommand(query, db, transaction); sqlCommand1.ExecuteNonQuery();

                }

                transaction.Commit();
            }
            Common.setMessageBox(" Transaction Completed Successfully ", Program.ApplicationName, 1);
            //}
        }

        void cboAgency_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboAgency.SelectedValue != null && !isFirst)
            {
                setReload(Convert.ToInt32(cboAgency.SelectedValue));
                MarkData(Convert.ToInt32(cboAgency.SelectedValue));

            }
        }

        public void setDBComboBox()
        {
            DataTable Dt;

            using (var ds = new System.Data.DataSet())
            {
                using (SqlDataAdapter ada = new SqlDataAdapter((string)"SELECT AgencyCode,AgencyName FROM Registration.tblAgency ORDER BY AgencyName", Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }

                Dt = ds.Tables[0];
            }

            Common.setComboList(cboAgency, Dt, "AgencyCode", "AgencyName");

            cboAgency.SelectedIndex = -1;

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
                //groupControl2.Text = "Add New Record";
                iTransType = TransactionTypeCode.New;
                ShowForm();
                //cboNature.SelectedIndex = -1;
                boolIsUpdate = false;
            }
            else if (sender == tsbEdit)
            {
                //groupControl2.Text = "Edit Record Mode";
                iTransType = TransactionTypeCode.Edit;
                //if (EditRecordMode())
                //{
                ShowForm();
                boolIsUpdate = true;
                //BindData();
                //}
            }
            else if (sender == tsbDelete)
            {
                ////groupControl2.Text = "Delete Record Mode";
                //iTransType = TransactionTypeCode.Delete;
                //if (MosesClassLibrary.Utilities.Common.AskQuestion("Deleting this record will delete attached record.\nDo you want to continue?", ""))
                //{
                //}
                //else
                //    tsbReload.PerformClick();
                //boolIsUpdate = false;
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

            isFirst = false;

            setDBComboBox();

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

        private void setReload(int strAgency)
        {

            using (var ds = new System.Data.DataSet())
            {
                using (SqlDataAdapter ada = new SqlDataAdapter(string.Format("SELECT RevenueCode,Description FROM Collection.tblRevenueType WHERE AgencyCode='{0}'", (strAgency)), Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }
                //connect.connect.Close();
                dt = ds.Tables[0];
                gridControl4.DataSource = dt.DefaultView;
            }
            gridView4.OptionsBehavior.Editable = false;
            //gridView4.Columns["BankID"].Visible = false;
            gridView4.BestFitColumns();

            label5.Text = dt.Rows.Count + "  Rows of Records. ";

            if (isFirstGrid)
            {
                //selection = new GridCheckMarksSelection(gridView1, ref label4);
                selection = new GridCheckMarksSelection(gridView4, ref label4);
                selection.CheckMarkColumn.VisibleIndex = 0;
                isFirstGrid = false;
            }
        }

        private void MarkData(int InAgencyCode)
        {

            using (var ds = new System.Data.DataSet())
            {
                using (SqlDataAdapter ada = new SqlDataAdapter(string.Format("SELECT RevenueCode FROM Receipt.tblRevenueReceiptException WHERE AgencyCode='{0}'", InAgencyCode), Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }

                dts = ds.Tables[0];
            }

            if (dts != null && dts.Rows.Count > 0 && gridView4 != null && gridView4.RowCount > 0)
            {
                for (int i = 0; i < gridView4.RowCount; i++)
                {
                    objRevCode = gridView4.GetRowCellValue(i, "RevenueCode");

                    if (objRevCode != null)
                    {
                        for (int j = 0; j < dts.Rows.Count; j++)
                        {
                            string revCode = Convert.ToString(objRevCode);
                            string revCode1 = Convert.ToString(dts.Rows[j][0]);
                            if (revCode.Trim() == revCode1.Trim())
                            //if (object.Equals(objRevCode, dt.Rows[0]))
                            {
                                //gridView4.SelectRow(i);
                                //selection.SelectRow(i, true);
                                selection.SelectRow(i, true);
                            }
                        }
                    }
                }
            }
        }

        public void RefreshForm()
        {
            NavBars.ToolStripEnableDisableControls(toolStrip, Tag as String);
        }

    }
}
