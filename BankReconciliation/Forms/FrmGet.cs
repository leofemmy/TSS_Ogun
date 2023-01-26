using BankReconciliation.Class;
using DevExpress.Utils;
using DevExpress.XtraSplashScreen;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using TaxSmartSuite.Class;

namespace BankReconciliation.Forms
{
    public partial class FrmGet : Form
    {
        public static FrmGet publicStreetGroup; private SqlCommand _command; private SqlDataAdapter adp; System.Data.DataSet dtsoff = new System.Data.DataSet(); System.Data.DataSet dts = new System.Data.DataSet(); DataTable dt = new DataTable(); DataTable tableTrans = new DataTable();
        public FrmGet()
        {
            SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

            InitializeComponent();

            publicStreetGroup = this;

            setImages();

            ToolStripEvent();

            Load += OnFormLoad;

            OnFormLoad(null, null);

            sbnUpdate.Click += SbnUpdate_Click;

            sbnContinue.Click += SbnContinue_Click;

            tableTrans.Columns.Add("OfflineUNiqueID", typeof(int));

            tableTrans.Columns.Add("ApprovedBy", typeof(string));

            tableTrans.Columns.Add("ApprovedDate", typeof(DateTime));

            SplashScreenManager.CloseForm(false);
        }

        private void SbnContinue_Click(object sender, EventArgs e)
        {
            //if (tableTrans != null && tableTrans.Rows.Count > 0)
            //{
            //    using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
            //    {
            //        connect.Open();
            //        _command = new SqlCommand("doReclassifiedProcess", connect) { CommandType = CommandType.StoredProcedure };
            //        _command.Parameters.Add(new SqlParameter("@pTransaction", SqlDbType.Structured)).Value = tableTrans;
            //        _command.Parameters.Add(new SqlParameter("@userid", SqlDbType.VarChar)).Value = Program.UserID;
            //        using (System.Data.DataSet ds = new System.Data.DataSet())
            //        {
            //            ds.Clear();
            //            adp = new SqlDataAdapter(_command);
            //            adp.Fill(ds);
            //            connect.Close();

            //            if (ds.Tables[0].Rows[0]["returnCode"].ToString() != "00")
            //            {
            //                Common.setMessageBox(ds.Tables[0].Rows[0]["returnMessage"].ToString(), Program.ApplicationName, 2);
            //                return;
            //            }
            //            else
            //            {

            //                dt = ds.Tables[1];


            //            }

            //        }
            //    }
            //}
            //else
            //{
            //    Common.setMessageBox("No Approved Record Found to Reclassified!", "Reclassification", 2); return;
            //}
        }

        private void SbnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

                switch (Program.intCode)
                {
                    case 13://Akwa Ibom state
                        break;
                    case 20://Delta state
                        using (var reconcilation = new CentralDetla.CollectionManager())
                        {
                            dts = reconcilation.ReconcilliationReclassificationCheckApproved(dtsoff);
                        }
                        break;
                    case 32://kogi state
                        break;
                    case 37://ogun state
                        using (var receiptsserv = new Centralogun.CollectionManager())
                        {
                            dts = receiptsserv.ReconcilliationReclassificationCheckApproved(dtsoff);
                        }
                        break;
                    case 40://oyo state
                        using (var reconciliationoyo = new Centraloyo.CollectionManager())
                        {
                            dts = reconciliationoyo.ReconcilliationReclassificationCheckApproved(dtsoff);
                        }
                        break;
                    default:
                        break;

                }



                if (dts != null)
                {
                    //tableTrans = new DataTable();

                    foreach (DataRow dr in dts.Tables[0].Rows)
                    {
                        for (int rowHandle = 0; rowHandle < gridView1.RowCount; rowHandle++)
                        {
                            var periodID = gridView1.GetRowCellValue(rowHandle, gridView1.Columns["ID"]);

                            if (Convert.ToInt32(dr["OfflineUniqueID"].ToString()) == Convert.ToInt32(periodID.ToString()))
                            {
                                bool? approvalStatus = null;
                                if (dr["isApproved"] != null && dr["isApproved"] != DBNull.Value)
                                    approvalStatus = Convert.ToBoolean(dr["isApproved"]);

                                if (approvalStatus == null)
                                {
                                    gridView1.SetRowCellValue(rowHandle, gridView1.Columns["Status"], "Awaiting Approval");
                                }
                                else if (approvalStatus == true)
                                {
                                    gridView1.SetRowCellValue(rowHandle, gridView1.Columns["Status"], "Approved");
                                    //get approved offlineid
                                    tableTrans.Rows.Add(new object[] { Convert.ToInt32(dr["OfflineUniqueID"].ToString()), dr["ApprovedBY"].ToString(), Convert.ToDateTime(dr["ApprovedDate"]) });
                                }
                                else if (approvalStatus == false)
                                {
                                    gridView1.SetRowCellValue(rowHandle, gridView1.Columns["Status"], "Disapproved");
                                }
                            }
                        }
                    }
                }

                //check for approved record before processing
                if (tableTrans != null && tableTrans.Rows.Count > 0)
                {
                    using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
                    {
                        connect.Open();
                        _command = new SqlCommand("doReclassifiedProcess", connect) { CommandType = CommandType.StoredProcedure };
                        _command.Parameters.Add(new SqlParameter("@pTransaction", SqlDbType.Structured)).Value = tableTrans;
                        _command.Parameters.Add(new SqlParameter("@userid", SqlDbType.VarChar)).Value = Program.UserID;

                        _command.CommandTimeout = 0;

                        using (System.Data.DataSet ds = new System.Data.DataSet())
                        {
                            ds.Clear();
                            adp = new SqlDataAdapter(_command);
                            adp.Fill(ds);
                            connect.Close();

                            if (ds.Tables[0].Rows[0]["returnCode"].ToString() != "00")
                            {
                                Common.setMessageBox(ds.Tables[0].Rows[0]["returnMessage"].ToString(), Program.ApplicationName, 2);
                                return;
                            }
                            else
                            {
                                Common.setMessageBox(ds.Tables[0].Rows[0]["returnMessage"].ToString(), Program.ApplicationName, 2);

                                using (FrmReclassifiedResult frmreport = new FrmReclassifiedResult(ds))
                                {
                                    frmreport.ShowDialog();
                                }
                            }

                        }
                    }
                }
                else
                {
                    Common.setMessageBox("No Approved Record Found to Reclassified!", "Reclassification", 2); return;
                }
            }
            catch (Exception ex)
            {
                Tripous.Sys.ErrorBox(ex.Message); return;
            }
            finally
            {
                SplashScreenManager.CloseForm(false);
            }

        }

        public void RefreshForm()
        {
            NavBars.ToolStripEnableDisableControls(toolStrip, Tag as String);
        }

        private void setImages()
        {
            tsbNew.Image = MDIMains.publicMDIParent.i16x16.Images[5];
            tsbEdit.Image = MDIMains.publicMDIParent.i16x16.Images[6];
            tsbDelete.Image = MDIMains.publicMDIParent.i16x16.Images[7];
            tsbReload.Image = MDIMains.publicMDIParent.i16x16.Images[0];
            tsbClose.Image = MDIMains.publicMDIParent.i16x16.Images[11];

            //bttnCancel.Image = MDIMains.publicMDIParent.i32x32.Images[9];
            ////bttnReset.Image = MDIMains.publicMDIParent.i32x32.Images[8];
            ////bttnUpdate.Image = MDIMain.publicMDIParent.i32x32.Images[7];
            //bttnUpdate.Image = MDIMains.publicMDIParent.i32x32.Images[7];
            //btnAllocate.Image = MDIMains.publicMDIParent.i32x32.Images[6];
            //bttncompare.Image = MDIMains.publicMDIParent.i32x32.Images[8];
            ////bttnClose.Image = MDIMain.publicMDIParent.i32x32.Images[9];
            //bttnSave.Image = MDIMains.publicMDIParent.i32x32.Images[7];

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
                //iTransType = TransactionTypeCode.New;
                //ShowForm();
                //boolIsUpdate = false;
            }
            else if (sender == tsbEdit)
            {
                //groupControl2.Text = "Edit Record Mode";
                //iTransType = TransactionTypeCode.Edit;
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
                //if (MosesClassLibrary.Utilities.Common.AskQuestion("Disable this record will delete attached record.\nDo you want to continue?", ""))
                //{
                //    if (string.IsNullOrEmpty(ID.ToString()))
                //    {
                //        Common.setMessageBox("No Record Selected for Disable", Program.ApplicationName, 3);
                //        return;
                //    }
                //    else
                //        //deleteRecord(ID);
                //}
                //else
                tsbReload.PerformClick();
                //boolIsUpdate = false;
            }
            else if (sender == tsbReload)
            {
                //iTransType = TransactionTypeCode.Reload; setReload();
                //ShowForm();
            }
            //bttnReset.PerformClick();
        }

        void OnFormLoad(object sender, EventArgs e)
        {
            setReload();

        }

        private void setReload()
        {
            try
            {
                using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
                {
                    connect.Open();
                    _command = new SqlCommand("ReloadReclassifiedLogRequest", connect) { CommandType = CommandType.StoredProcedure };
                    using (System.Data.DataSet ds = new System.Data.DataSet())
                    {
                        ds.Clear();
                        adp = new SqlDataAdapter(_command);
                        adp.Fill(ds);
                        connect.Close();

                        if (ds.Tables[0].Rows[0]["returnCode"].ToString() != "00")
                        {
                            Common.setMessageBox(ds.Tables[0].Rows[0]["returnMessage"].ToString(), Program.ApplicationName, 2);
                            return;
                        }
                        else
                        {


                            dt = ds.Tables[1];

                            dt.Columns.Add("Status", typeof(String));

                            gridControl1.DataSource = dt;
                            DataTable dtt = new DataTable();
                            dtt = ds.Tables[2].Copy();

                            //dtsoff = ds.Tables[2].DataSet;
                            dtsoff.Tables.Add(dtt);

                            //AddCombCredit();

                            gridView1.OptionsBehavior.Editable = false;

                            gridView1.Columns["Amount"].DisplayFormat.FormatType = FormatType.Numeric;
                            gridView1.Columns["Amount"].DisplayFormat.FormatString = "n2";
                            gridView1.Columns["Date"].DisplayFormat.FormatType = FormatType.DateTime;
                            gridView1.Columns["Date"].DisplayFormat.FormatString = "dd/MM/yyyy";

                            gridView1.Columns["ReclassifiedDate"].DisplayFormat.FormatType = FormatType.DateTime;
                            gridView1.Columns["ReclassifiedDate"].DisplayFormat.FormatString = "dd/MM/yyyy";

                            gridView1.Columns["FirstApprovedDate"].DisplayFormat.FormatType = FormatType.DateTime;
                            gridView1.Columns["FirstApprovedDate"].DisplayFormat.FormatString = "dd/MM/yyyy";

                            gridView1.Columns["Amount"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                            gridView1.Columns["Amount"].SummaryItem.FieldName = "Amount";
                            gridView1.Columns["Amount"].SummaryItem.DisplayFormat = "Total = {0:n}";
                            gridView1.Columns["BSID"].Visible = false;
                            gridView1.Columns["ID"].Visible = false;

                            gridView1.OptionsView.ColumnAutoWidth = false;
                            gridView1.OptionsView.ShowFooter = true;

                            gridView1.BestFitColumns();
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                Tripous.Sys.ErrorBox(String.Format("{0}{1}", ex.Message, ex.StackTrace)); return;
            }
        }

    }
}
