using BankReconciliation.Class;
using DevExpress.XtraGrid.Selection;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using TaxSmartSuite.Class;

namespace BankReconciliation.Forms
{
    public partial class FrmTempRevenue : Form
    {
        public static FrmTempRevenue publicStreetGroup;

        protected TransactionTypeCode iTransType; protected bool boolIsUpdate;

        bool isFirstGrid = true; GridCheckMarksSelection selection;

        private SqlCommand _command; private SqlDataAdapter adp;

        string ID = string.Empty;

        public FrmTempRevenue()
        {
            InitializeComponent();

            publicStreetGroup = this;

            setImages();

            ToolStripEvent();

            iTransType = TransactionTypeCode.Null;

            Load += OnFormLoad;

            OnFormLoad(null, null);
        }

        private void setImages()
        {
            tsbNew.Image = MDIMains.publicMDIParent.i16x16.Images[5];
            tsbEdit.Image = MDIMains.publicMDIParent.i16x16.Images[6];
            tsbDelete.Image = MDIMains.publicMDIParent.i16x16.Images[7];
            tsbReload.Image = MDIMains.publicMDIParent.i16x16.Images[0];
            tsbClose.Image = MDIMains.publicMDIParent.i16x16.Images[11];

            //bttnCancel.Image = MDIMains.publicMDIParent.i32x32.Images[9];
            //bttnReset.Image = MDIMains.publicMDIParent.i32x32.Images[8];
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
                //groupControl2.Text = "Add New Record";
                iTransType = TransactionTypeCode.New;
                //ShowForm();
                boolIsUpdate = false;
            }
            else if (sender == tsbEdit)
            {
                //groupControl2.Text = "Edit Record Mode";
                iTransType = TransactionTypeCode.Edit;
                //if (EditRecordMode())
                //{
                //    ShowForm();
                //    boolIsUpdate = true;
                //}
            }
            else if (sender == tsbDelete)
            {
                //groupControl2.Text = "Disable Record Mode";

                //iTransType = TransactionTypeCode.Delete;

                //if (MosesClassLibrary.Utilities.Common.AskQuestion("Disable this record will Disable attached record.\nDo you want to continue?", ""))
                //{
                //    if (string.IsNullOrEmpty(txtCode.Text.Trim()))
                //    {
                //        Common.setMessageBox("No Record Selected for Deleting", Program.ApplicationName, 3);
                //        return;
                //    }
                //    else
                //        deleteRecord(ID, (string)cboNature.SelectedValue);
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
            //ShowForm();
            //setDBComboBox();

            //isFirst = false;

            setReload();

            bttnUpdate.Click += bttnUpdate_Click;
        }

        void bttnUpdate_Click(object sender, EventArgs e)
        {
            if (selection.SelectedCount > 1)
            {
                Common.setMessageBox("Only Revenue Temp can be Set up", "Default Revenue Set up", 1);
                return;
            }
            else
            {
                if (!boolIsUpdate)
                {
                    for (int i = 0; i < selection.SelectedCount; i++)
                    {
                        string values = string.Empty;
                        values += String.Format("{0}", (selection.GetSelectedRow(i) as DataRowView)["RevenueCode"]);

                        string query = string.Format("INSERT INTO [Reconciliation].[tblRevenueTemp]([RevenueCode]) VALUES ('{0}');", values);

                        using (SqlConnection db = new SqlConnection(Logic.ConnectionString))
                        {
                            SqlTransaction transaction;

                            db.Open();

                            transaction = db.BeginTransaction();

                            try
                            {
                                using (SqlCommand sqlCommand1 = new SqlCommand(query, db, transaction))
                                {
                                    sqlCommand1.ExecuteNonQuery();
                                }

                                transaction.Commit();
                            }
                            catch (SqlException sqlError)
                            {
                                transaction.Rollback(); Tripous.Sys.ErrorBox(sqlError);
                                return;
                            }
                            db.Close();
                        }
                    }
                    return;
                }
                for (int i = 0; i < selection.SelectedCount; i++)
                {
                    string values = string.Empty;
                    values += String.Format("{0}", (selection.GetSelectedRow(i) as DataRowView)["RevenueCode"]);

                    using (SqlConnection db = new SqlConnection(Logic.ConnectionString))
                    {
                        SqlTransaction transaction;

                        db.Open();

                        transaction = db.BeginTransaction();

                        try
                        {
                            string dry = String.Format(String.Format("UPDATE [Reconciliation].[tblRevenueTemp] SET [RevenueCode]='{{0}}' where  RevenueCode ='{0}'", ID), values);

                            using (SqlCommand sqlCommand1 = new SqlCommand(dry, db, transaction))
                            {
                                sqlCommand1.ExecuteNonQuery();
                            }

                            transaction.Commit();
                        }
                        catch (SqlException sqlError)
                        {
                            transaction.Rollback(); Tripous.Sys.ErrorBox(sqlError);
                            return;
                        }
                        db.Close();
                    }
                }
                Common.setMessageBox("Record Update Successfully", Program.ApplicationName, 1);
                return;
            }

        }

        private void setReload()
        {
            //connect.connect.Close();
            DataTable dt;

            using (var ds = new System.Data.DataSet())
            {

                using (SqlDataAdapter ada = new SqlDataAdapter("SELECT RevenueCode,Description,Registration.tblAgency.AgencyCode,AgencyName FROM Collection.tblRevenueType INNER JOIN Registration.tblAgency ON Collection.tblRevenueType.AgencyCode = Registration.tblAgency.AgencyCode WHERE RevenueCode='100011004112001'", Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }

                dt = ds.Tables[0];
                gridControl1.DataSource = ds.Tables[0];
            }


            gridView1.OptionsBehavior.Editable = false;
            gridView1.Columns["AgencyCode"].Visible = false;
            gridView1.Columns["RevenueCode"].Visible = false;
            gridView1.BestFitColumns();

            if (isFirstGrid)
            {
                Label lbltext = new Label();

                selection = new GridCheckMarksSelection(gridView1, ref lbltext);
                selection.CheckMarkColumn.VisibleIndex = 0;
                isFirstGrid = false;
                groupBox1.Text = lbltext.Text.Trim();
            }

            using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
            {
                connect.Open();
                _command = new SqlCommand("dogetRevenueTemp", connect) { CommandType = CommandType.StoredProcedure };
                using (System.Data.DataSet ds = new System.Data.DataSet())
                {
                    ds.Clear();
                    adp = new SqlDataAdapter(_command);
                    adp.Fill(ds);
                    connect.Close();

                    if (ds.Tables[0].Rows[0]["returnCode"].ToString() == "00")
                    {

                        if (ds.Tables[1].Rows.Count > 0)
                        {
                            //loope through the dataset to get
                            for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                            {
                                for (int h = 0; h < gridView1.DataRowCount; h++)
                                {
                                    int rowHandle = h;

                                    //object obj = gridView1.GetRowCellValue(rowHandle, "RevenueCode");

                                    //object bjt1 = ds.Tables[1].Rows[i]["RevenueCode"];

                                    if (gridView1.GetRowCellValue(rowHandle, "RevenueCode").Equals(ds.Tables[1].Rows[i]["RevenueCode"]))
                                    {
                                        selection.SelectRow(rowHandle, true);

                                        ID = ds.Tables[1].Rows[i]["RevenueCode"].ToString();

                                        boolIsUpdate = true;

                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                            }
                        }
                        else
                        {
                            boolIsUpdate = true;
                        }
                    }


                }
                //else
                //{
                //    gridControl1.DataSource = ds.Tables[1];
                //}

            }
        }


        public void RefreshForm()
        {
            NavBars.ToolStripEnableDisableControls(toolStrip, Tag as String);
        }


    }
}
