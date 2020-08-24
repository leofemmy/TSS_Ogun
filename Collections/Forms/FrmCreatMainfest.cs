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
using System.Collections;
using DevExpress.XtraGrid.Views.Grid;
using Collection.Report;
using DevExpress.Utils;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.DXErrorProvider;
using DevExpress.XtraPrinting;
using DevExpress.XtraSplashScreen;
using Collections;
using DevExpress.XtraReports.UI;
using DevExpress.XtraReports.Parameters;

namespace Collection.Forms
{
    public partial class FrmCreatMainfest : Form
    {
        public static FrmCreatMainfest publicInstance;

        protected TransactionTypeCode iTransType;

        private SqlCommand _command;

        protected bool boolIsUpdate; int newID; int count;

        string values;

        System.Data.DataSet ds;

        //SqlDataAdapter ada; 
        SqlDataAdapter ada;

        System.Data.DataTable dt; string query = string.Empty;
        public FrmCreatMainfest()
        {
            SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

            InitializeComponent();

            publicInstance = this;

            setImages();

            ToolStripEvent();

            Load += OnFormLoad;

            iTransType = TransactionTypeCode.New;

            OnFormLoad(null, null);

            SplashScreenManager.CloseForm(false);
        }

        private void setImages()
        {
            tsbNew.Image = MDIMain.publicMDIParent.i16x16.Images[5];
            tsbEdit.Image = MDIMain.publicMDIParent.i16x16.Images[6];
            tsbDelete.Image = MDIMain.publicMDIParent.i16x16.Images[7];
            tsbReload.Image = MDIMain.publicMDIParent.i16x16.Images[0];
            tsbClose.Image = MDIMain.publicMDIParent.i16x16.Images[11];

            //bttnCancel.Image = MDIMain.publicMDIParent.i32x32.Images[9];
            //btnApply.Image = MDIMain.publicMDIParent.i32x32.Images[8];
            //btnPrint.Image = MDIMain.publicMDIParent.i32x32.Images[29];
            //btnSearch.Image = MDIMain.publicMDIParent.i32x32.Images[2];

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
                Close();
            }
            else if (sender == tsbNew)
            {
                //label11.Visible = false;

                //txtPaymentRef.Visible = false;

                //groupControl2.Text = "Add New Record";

                //iTransType = TransactionTypeCode.New;

                //ShowForm();

                //clear();

                //groupControl2.Enabled = true;

                boolIsUpdate = false;
            }
            else if (sender == tsbEdit)
            {
                //groupControl2.Text = "Edit Record Mode";

                iTransType = TransactionTypeCode.Edit;

                //ShowForm();

                boolIsUpdate = true;

            }
            //else if (sender == tsbDelete)
            //{
            //    groupControl2.Text = "Delete Record Mode";
            //    iTransType = TransactionTypeCode.Delete;
            //    if (MosesClassLibrary.Utilities.Common.AskQuestion("Deleting this record will delete attached record.\nDo you want to continue?", ""))
            //    {
            //    }
            //    else
            //        tsbReload.PerformClick();
            //    boolIsUpdate = false;
            //}
            //else if (sender == tsbReload)
            //{
            //    iTransType = TransactionTypeCode.Reload;
            //    ShowForm();
            //}
            //bttnReset.PerformClick();
        }
        public void RefreshForm()
        {
            NavBars.ToolStripEnableDisableControls(toolStrip, Tag as String);
        }

        void OnFormLoad(object sender, EventArgs e)
        {
            SetReload();

            btnCreate.Click += BtnCreate_Click;

            btnUpdate.Click += BtnUpdate_Click;

        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            if (ds.Tables[0].Rows[0]["ManifestID"] == DBNull.Value )
            {
                Common.setMessageBox(" Manifest Not created yet !", Program.ApplicationName, 1);
                return;
            }
            else
            {
                try
                {
                    SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

                    using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
                    {
                        connect.Open();
                        _command = new SqlCommand("ApplyManifest", connect)
                        {
                            CommandType = CommandType.StoredProcedure
                        };
                        //_command.Parameters.Add(new SqlParameter("@userid", SqlDbType.VarChar)).Value =
                        //    Program.UserID;
                        _command.Parameters.Add(new SqlParameter("@tblCollectionReport_Manifest",
                            SqlDbType.Structured)).Value = ds.Tables[0];
                        _command.CommandTimeout = 0;
                        System.Data.DataSet response = new System.Data.DataSet();

                        //adp = new SqlDataAdapter(_command);
                        //adp.Fill(response);

                        ada = new SqlDataAdapter(_command);

                        ada.Fill(response);

                        connect.Close();

                        if (response.Tables[0].Rows[0]["returnCode"].ToString() == "00")
                        {
                            using (System.Data.DataSet dds = new System.Data.DataSet("DsCollectionReport"))
                            {
                                //using (SqlDataAdapter ada = new SqlDataAdapter(query, Logic.ConnectionString))
                                //{
                                //ada.Fill(dds, "CollectionReportTable");
                                XRepManifest repManifest = new XRepManifest
                                {
                                    DataSource = response.Tables[1],
                                    DataAdapter = ada,
                                    DataMember = "CollectionReportTable",
                                    RequestParameters = false
                                };
                                repManifest.xrLabel10.Text = Program.UserID;
                                repManifest.logoPath = Logic.logopth;
                                repManifest.xrLabel9.Text = string.Format("{0} STATE GOVERNMENT",
                                    Program.StateName.ToUpper());
                                repManifest.ShowPreviewDialog();
                                //}
                            }
                        }
                        else
                        { }
                    }
                }
                catch (Exception ex)
                {
                    
                    Tripous.Sys.ErrorBox(ex.Message + ex.StackTrace.ToString());
                    return;
                }
                finally
                {
                    SplashScreenManager.CloseForm(false);
                }
            }
        }

        private void BtnCreate_Click(object sender, EventArgs e)
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

                using (SqlConnection db = new SqlConnection(Logic.ConnectionString))
                {
                    SqlTransaction transaction;

                    db.Open();

                    transaction = db.BeginTransaction();
                    try
                    {


                        using (SqlCommand sqlCommand1 = new SqlCommand(String.Format("INSERT INTO Receipt.tblManifest( ManifestBy, ManifestDate ) VALUES ('{0}','{1}');SELECT CAST(scope_identity() AS int)", Program.UserID.ToString().Trim(), DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss")), db, transaction))
                        {
                            newID = (int)sqlCommand1.ExecuteScalar();
                        }

                        transaction.Commit();
                    }
                    catch (SqlException sqlError)
                    {
                        transaction.Rollback();
                        Tripous.Sys.ErrorBox(sqlError);
                        return;
                    }
                    finally
                    {
                        ApplyMainifestgrid(newID);
                        db.Close();
                    }

                }
            }
            catch (Exception ex)
            {
                Tripous.Sys.Error(ex.StackTrace + ex.Message);
            }
            finally
            {
                SplashScreenManager.CloseForm(false);
            }
        }

        void SetReload()
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

                //query = string.Format("SELECT PaymentRefNumber,EReceipts AS Receipts,PrintedBY,ManifestID,IsPrintedDate AS PrintedDate,ControlNumber FROM Receipt.tblCollectionReceipt WHERE PrintedBY IS NOT NULL AND ControlNumber IS NOT NULL");

                dt = new DataTable();
                dt.Clear();
                ds = new System.Data.DataSet();
                ds.Clear();


                ada = new SqlDataAdapter(string.Format("SELECT TOP 400 PaymentRefNumber,EReceipts AS Receipts,PrintedBY,ManifestID,IsPrintedDate AS PrintedDate,ControlNumber FROM Receipt.tblCollectionReceipt WHERE PrintedBY IS NOT NULL AND ControlNumber IS NOT NULL AND ManifestID IS NULL"), Logic.ConnectionString);

                ada.Fill(ds, "table");



                dt = ds.Tables[0];

                if (dt != null && dt.Rows.Count > 0)
                {
                    //dt.Columns.Add("Mark x or ✓", typeof(string));
                    //dt.AcceptChanges();

                    gridControl1.DataSource = dt;
                    gridView1.Columns["PrintedDate"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
                    gridView1.Columns["PrintedDate"].DisplayFormat.FormatString = "dd/MM/yyyy";


                    gridControl1.ForceInitialize();

                }
                else
                {
                    gridView1.Columns.Clear();
                }

                //label5.Text = String.Format(" Total Number of Printed receipts :  {0} waiting for control number to be applied ", dt.Rows.Count);

                //}


            }
            catch (Exception ex)
            {
                Common.setMessageBox(ex.Message, Program.ApplicationName, 3);
                return;
            }
            finally
            {
                SplashScreenManager.CloseForm(false);
            }
        }

        void ApplyMainifestgrid(int inmain)
        {
            GridView view = gridView1; count = 0;

            if (!string.IsNullOrEmpty(inmain.ToString()))
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    view.SetRowCellValue(i, "ManifestID", inmain);
                }
            }
        }
    }
}
