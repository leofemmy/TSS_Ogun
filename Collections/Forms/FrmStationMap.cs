using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TaxSmartSuite;
using DevExpress.XtraGrid.Selection;
using TaxSmartSuite.Class;
using System.Data.SqlClient;
//using Control_Panel.Class;
using Collection.Classess;
using Collection.Report;
using DevExpress.Utils;

namespace Collection.Forms
{
    public partial class FrmStationMap : Form
    {
        private DataTable Dt;

        GridCheckMarksSelection selection;

        public static FrmStationMap publicStreetGroup;

        public static FrmStationMap publicInstance;

        protected TransactionTypeCode iTransType;

        protected bool boolIsUpdate;

        protected int ID;

        bool isFirstGrid = true;

        bool isFirst = true;

        public FrmStationMap()
        {
            InitializeComponent();

            publicStreetGroup = this;

            setImages();

            ToolStripEvent();

            Load += OnFormLoad;

            iTransType = TransactionTypeCode.Null;

            OnFormLoad(null, null);

            bttnUpdate.Click += bttnUpdate_Click;

            btnReport.Click += btnReport_Click;

        }

        void btnReport_Click(object sender, EventArgs e)
        {
            using (SqlConnection db = new SqlConnection(Logic.ConnectionString))
            {
                SqlTransaction transaction;

                db.Open();

                transaction = db.BeginTransaction();

                try
                {
                    using (System.Data.DataSet dds = new System.Data.DataSet("DsCollectionReport"))
                    {
                        SqlDataAdapter ada;

                        using (WaitDialogForm form = new WaitDialogForm())
                        {
                            string strFormat = null;

                            DataTable Dt = dds.Tables.Add("Stationmap");
                            ada = new SqlDataAdapter((string)"SELECT stationcode,stationname,RevenueOfficeCode, RevenueOfficeName, BranchName, BankName FROM viewStationBankBranch", Logic.ConnectionString);
                            ada.Fill(dds, "Stationmap");
                           
                        }

                        //XtraRepMap repstation = new XtraRepMap { DataSource = dds, DataMember = "Stationmap" };
                        //repstation.ShowPreviewDialog();


                    }


                }
                catch (SqlException sqlError)
                {
                    //transaction.Rollback();

                    Tripous.Sys.ErrorBox(sqlError);
                }
                catch (Exception ex)
                {
                    Tripous.Sys.ErrorBox(ex);
                }
                db.Close();
            }
        }

        void bttnUpdate_Click(object sender, EventArgs e)
        {
            if (selection.SelectedCount < 1)
            {
                Common.setMessageBox("Please, select record to Map", Program.ApplicationName, 3);
            }
            else if (cboStation.SelectedIndex < 0)
            {
                Common.setMessageBox("Please, select Station to Map", Program.ApplicationName, 3);
            }
            else
            {
                
                //return;
                using (SqlConnection db = new SqlConnection(Logic.ConnectionString))
                {
                    SqlTransaction transaction;
                    SqlCommand sqlCommand1;

                    db.Open();

                    transaction = db.BeginTransaction();

                    try
                    {

                        for (int i = 0; i < selection.SelectedCount; i++)
                        {
                            //using (var ds = new System.Data.DataSet())
                            //{//select record
                            //BranchCode,BankShortCode

                            string lol = ((selection.GetSelectedRow(i) as DataRowView)["BranchCode"].ToString());
                            string lol2 = ((selection.GetSelectedRow(i) as DataRowView)["BankShortCode"].ToString());

                            string query = String.Format("INSERT INTO [tblStationMap]([StationCode],[StationName],[RevenueOfficeCode],[RevenueOfficeName]) VALUES ('{0}','{1}','{2}','{3}')", cboStation.SelectedValue.ToString(), cboStation.Text, lol, lol2);


                            sqlCommand1 = new SqlCommand(query, db, transaction);
                            sqlCommand1.ExecuteNonQuery();



                        }


                        //call report for Print
                        transaction.Commit();
                        //ReceiptCall();
                    }
                    catch (SqlException sqlError)
                    {
                        transaction.Rollback();
                    }
                    db.Close();

                    setReload();

                    cboStation.SelectedIndex = -1;

                    Common.setMessageBox("Record has been successfully added", Program.ApplicationName, 1);
                    selection.ClearSelection();
                }
            }
        }

        internal GridCheckMarksSelection Selection
        {
            get { return selection; }
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

        public void RefreshForm()
        {
            NavBars.ToolStripEnableDisableControls(toolStrip, Tag as String);
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
                //Close();
            }
            else if (sender == tsbNew)
            {
                //groupControl2.Text = "Add New Record";
                iTransType = TransactionTypeCode.New;
                //ShowForm();
                //cboNature.SelectedIndex = -1;
                boolIsUpdate = false;
            }
            else if (sender == tsbEdit)
            {
                //groupControl2.Text = "Edit Record Mode";
                iTransType = TransactionTypeCode.Edit;
                //if (EditRecordMode())
                //{
                //ShowForm();
                boolIsUpdate = true;
                //}
            }
            else if (sender == tsbDelete)
            {
                //groupControl2.Text = "Delete Record Mode";
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
            //ShowForm();
            setDBComboBox();
            isFirst = false;
            setReload();
            //cboNature.KeyPress += cboNature_KeyPress;

        }

        public void setDBComboBox()
        {
            DataTable Dt;

            using (var ds = new System.Data.DataSet())
            {
                string query = "SELECT StationCode,StationName FROM dbo.tblStation";

                using (SqlDataAdapter ada = new SqlDataAdapter(query, Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }

                Dt = ds.Tables[0];
            }

            Common.setComboList(cboStation, Dt, "StationCode", "StationName");

            cboStation.SelectedIndex = -1;

        }

        private void setReload()
        {
            DataTable dt;

            using (var ds = new System.Data.DataSet())
            {
                //connect.connect.Open();

                using (SqlDataAdapter ada = new SqlDataAdapter("SELECT BranchName,BranchCode,BankShortCode FROM ViewBranchBank WHERE (BranchCode NOT IN (SELECT RevenueOfficeCode FROM tblStationMap))  OR  ( BankShortCode NOT IN ( SELECT RevenueOfficeName FROM   tblStationMap ) )", Logic.ConnectionString))
                {
                    ada.SelectCommand.CommandTimeout = 0;

                    ada.Fill(ds, "table");
                }
                //connect.connect.Close();
                dt = ds.Tables[0];
                gridControl4.DataSource = dt.DefaultView;
            }
            gridView4.OptionsBehavior.Editable = false;
            gridView4.Columns["BranchCode"].Visible = false;
            gridView4.BestFitColumns();

            label4.Text = "Total Record:" + dt.Rows.Count;

            if (isFirstGrid)
            {
                selection = new GridCheckMarksSelection(gridView4,ref label3);
                selection.CheckMarkColumn.VisibleIndex = 0;
                isFirstGrid = false;
            }
        }


    }
}
