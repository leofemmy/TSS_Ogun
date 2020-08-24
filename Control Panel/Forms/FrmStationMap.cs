using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraGrid.Selection;
using TaxSmartSuite.Class;
using System.Data.SqlClient;
using Control_Panel.Class;

namespace Control_Panel.Forms
{
    public partial class FrmStationMap : Form
    {
        private DataTable Dt;

        GridCheckMarksSelection selection;

        public static FrmStationMap publicStreetGroup;

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

        }

        void bttnUpdate_Click(object sender, EventArgs e)
        {
            using (SqlConnection db = new SqlConnection(Logic.ConnectionString))
            {
                SqlTransaction transaction;

                db.Open();

                transaction = db.BeginTransaction();

                try
                {

                    for (int i = 0; i < selection.SelectedCount; i++)
                    {
                        using (var ds = new System.Data.DataSet())
                        {//select record

                            string lol = ((selection.GetSelectedRow(i) as DataRowView)["OfficeCode"].ToString());
                            string lol2 = ((selection.GetSelectedRow(i) as DataRowView)["OfficeName"].ToString());

                            string query = String.Format("INSERT INTO [tblStationMap]([StationCode],[StationName],[RevenueOfficeCode],[RevenueOfficeName]) VALUES ('{0}','{1}','{2}','{3}')",cboStation.SelectedValue,cboStation.Text.ToString(),lol,lol2 );


                            using (SqlCommand sqlCommand1 = new SqlCommand(query, db, transaction))
                            {
                                sqlCommand1.ExecuteNonQuery();
                            }

                            query = String.Format("SELECT StationCode,StationName FROM tblCollectionReport WHERE [ZoneCode] = '{0}'",  lol);

                            using (SqlDataAdapter ada = new SqlDataAdapter(query, Logic.ConnectionString))
                            {
                                ada.Fill(ds, "table");
                            }

                            Dt = ds.Tables[0];

                            foreach (DataRow item in Dt.Rows)
                            {
                                if (item["StationCode"].ToString() == "0" && item["StationName"].ToString() == "Not Yet Mapped")
                                {
                                    using (SqlCommand sqlCommand = new SqlCommand(String.Format(String.Format("UPDATE [tblCollectionReport] SET [StationCode]='{{0}}',[StationName]='{{1}}' where  [ZoneCode] ='{0}' and [StationCode]='0' and StationName='Not Yet Mapped' ", lol), lol, lol2), db, transaction))
                                    {
                                        sqlCommand.ExecuteNonQuery();
                                    }
                                }
                            }

                            //get the station code and name base on the check revenue office

                        }
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
            }
        }

        internal GridCheckMarksSelection Selection
        {
            get { return selection; }
        }

        private void setImages()
        {
            tsbNew.Image = MDIMain.publicMDIParent.i16x16.Images[3];
            tsbEdit.Image = MDIMain.publicMDIParent.i16x16.Images[4];
            tsbDelete.Image = MDIMain.publicMDIParent.i16x16.Images[0];
            tsbReload.Image = MDIMain.publicMDIParent.i16x16.Images[2];
            tsbClose.Image = MDIMain.publicMDIParent.i16x16.Images[1];

            ////bttnCancel.Image = MDIMain.publicMDIParent.i32x32.Images[9];
            ////bttnReset.Image = MDIMain.publicMDIParent.i32x32.Images[8];
            bttnUpdate.Image = MDIMain.publicMDIParent.i32x32.Images[29];

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
                using (SqlDataAdapter ada = new SqlDataAdapter("SELECT officecode,OfficeName FROM tblRevenueOffice WHERE officecode NOT IN (SELECT RevenueOfficeCode FROM tblStationMap )", Logic.ConnectionString))
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

            if (isFirstGrid)
            {
                selection = new GridCheckMarksSelection(gridView4);
                selection.CheckMarkColumn.VisibleIndex = 0;
                isFirstGrid = false;
            }
        }


    }
}
