using Collection.Classess;
using Collections;
using DevExpress.Utils;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Selection;
using DevExpress.XtraSplashScreen;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using TaxSmartSuite.Class;

namespace Collection.Forms
{
    public partial class FrmFirstApproval : Form
    {
        public static FrmFirstApproval publicStreetGroup; private SqlCommand _command; private SqlDataAdapter adp; GridColumn colView2 = new GridColumn(); RepositoryItemGridLookUpEdit repComboLookBoxCredit = new RepositoryItemGridLookUpEdit(); GridColumn colView = new GridColumn();
        GridCheckMarksSelection selection; DataTable tableTrans = new DataTable();
        System.Data.DataSet dataSet = new System.Data.DataSet();
        System.Data.DataSet dts; string strtoken = string.Empty;

        System.Data.DataSet dsreturn = new System.Data.DataSet();
        public FrmFirstApproval()
        {
            SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

            InitializeComponent();

            publicStreetGroup = this;

            setImages();

            ToolStripEvent();

            Load += OnFormLoad;

            OnFormLoad(null, null);

            tableTrans.Columns.Add("ID", typeof(int));

            tableTrans.Columns.Add("BSID", typeof(int));

            tableTrans.Columns.Add("TransID", typeof(int));


            SplashScreenManager.CloseForm(false);
        }

        public void RefreshForm()
        {
            NavBars.ToolStripEnableDisableControls(toolStrip, Tag as String);
        }

        private void setImages()
        {
            tsbNew.Image = MDIMain.publicMDIParent.i16x16.Images[5];
            tsbEdit.Image = MDIMain.publicMDIParent.i16x16.Images[6];
            tsbDelete.Image = MDIMain.publicMDIParent.i16x16.Images[7];
            tsbReload.Image = MDIMain.publicMDIParent.i16x16.Images[0];
            tsbClose.Image = MDIMain.publicMDIParent.i16x16.Images[11];

            //bttnCancel.Image = MDIMains.publicMDIParent.i32x32.Images[9];
            ////bttnReset.Image = MDIMains.publicMDIParent.i32x32.Images[8];
            ////bttnUpdate.Image = MDIMain.publicMDIParent.i32x32.Images[7];
            //bttnUpdate.Image = MDIMains.publicMDIParent.i32x32.Images[7];
            //btnAllocate.Image = MDIMains.publicMDIParent.i32x32.Images[6];
            //bttncompare.Image = MDIMains.publicMDIParent.i32x32.Images[8];
            ////bttnClose.Image = MDIMain.publicMDIParent.i32x32.Images[9];
            //bttnSave.Image = MDIMains.publicMDIParent.i32x32.Images[7];
            sbnUpdate.Image = MDIMain.publicMDIParent.i32x32.Images[2];
            sbnDisapprove.Image = MDIMain.publicMDIParent.i32x32.Images[10];
            //btnToken.Image = MDIMain.publicMDIParent.i32x32.Images[7];
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
            selection = new GridCheckMarksSelection(gridView1);
            sbnUpdate.Click += SbnUpdate_Click;
            sbnDisapprove.Click += SbnDisapprove_Click;
            //btnToken.Click += BtnToken_Click;
        }

        private void SbnUpdate_Click(object sender, EventArgs e)
        {
            dowork();
        }

        private void setReload()
        {
            try
            {
                string strquery = "SELECT Collection.tblCollectionReport.PaymentRefNumber , PayerName , Amount , PaymentDate , AgencyName ,tblCollectionReport.Description , CASE WHEN tblCollectionReport.EReceipts IS NULL  THEN ReceiptNo ELSE tblCollectionReport.EReceipts END AS EReceipts,ControlNumber,tblReceiptTransaction.Description AS Comment FROM    Receipt.tblReprintedReceipts INNER JOIN Receipt.tblReceiptTransaction ON tblReceiptTransaction.TransType = tblReprintedReceipts.TransType INNER JOIN Collection.tblCollectionReport ON tblCollectionReport.PaymentRefNumber = tblReprintedReceipts.PaymentRefNumber WHERE FirstApprovalStatus = 0";

                using (var ds = new System.Data.DataSet())
                {
                    using (SqlDataAdapter ada = new SqlDataAdapter(strquery, Logic.ConnectionString))
                    {
                        ada.SelectCommand.CommandTimeout = 0;

                        ada.Fill(ds, "table");
                    }

                    if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 1)
                    {
                        gridControl1.DataSource = ds.Tables[0];
                        gridView1.OptionsBehavior.Editable = false;

                        gridView1.Columns["Amount"].DisplayFormat.FormatType = FormatType.Numeric;
                        gridView1.Columns["Amount"].DisplayFormat.FormatString = "n2";
                        gridView1.Columns["PaymentDate"].DisplayFormat.FormatType = FormatType.DateTime;
                        gridView1.Columns["PaymentDate"].DisplayFormat.FormatString = "dd/MM/yyyy";
                    }
                    else
                        Common.setMessageBox("No Record to Approve", Program.ApplicationName, 2);
                    return;

                }

                //using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
                //{
                //    connect.Open();
                //    _command = new SqlCommand("ReloadReclassified", connect) { CommandType = CommandType.StoredProcedure };
                //    using (System.Data.DataSet ds = new System.Data.DataSet())
                //    {
                //        ds.Clear();
                //        adp = new SqlDataAdapter(_command);
                //        adp.Fill(ds);
                //        connect.Close();

                //        if (ds.Tables[0].Rows[0]["returnCode"].ToString() != "00")
                //        {

                //            return;
                //        }
                //        else
                //        {

                //            

                //            AddCombCredit();



                //            gridView1.Columns["ReclassifiedDate"].DisplayFormat.FormatType = FormatType.DateTime;
                //            gridView1.Columns["ReclassifiedDate"].DisplayFormat.FormatString = "dd/MM/yyyy";

                //            //gridView1.Columns["ReclassifiedTime"].DisplayFormat.FormatType = FormatType.DateTime;
                //            //gridView1.Columns["ReclassifiedTime"].DisplayFormat.FormatString = "HHMMSS";

                //            gridView1.Columns["OldTransID"].ColumnEdit = repComboLookBoxCredit;
                //            gridView1.Columns["OldTransID"].Caption = "From";

                //            gridView1.Columns["TransID"].ColumnEdit = repComboLookBoxCredit;
                //            gridView1.Columns["TransID"].Caption = "To";


                //            gridView1.Columns["Amount"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                //            gridView1.Columns["Amount"].SummaryItem.FieldName = "Amount";
                //            gridView1.Columns["Amount"].SummaryItem.DisplayFormat = "Total = {0:n}";
                //            gridView1.Columns["BSID"].Visible = false;
                //            gridView1.Columns["ID"].Visible = false;

                //            gridView1.OptionsView.ColumnAutoWidth = false;
                //            gridView1.OptionsView.ShowFooter = true;

                //            gridView1.BestFitColumns();
                //        }

                //    }
                //}
            }
            catch (Exception ex)
            {
                Tripous.Sys.ErrorBox(String.Format("{0}{1}", ex.Message, ex.StackTrace)); return;
            }
        }

        void dowork()
        {

            string value = string.Empty;

            strtoken = string.Empty;


            Token.dotoken();

            if (DialogResults.InputBox(@"OTP", string.Format("Kindly enter the token to Authorize this transaction.", $"********{Program.Userphone.Substring(7)}"), ref value) == DialogResult.OK)
            {
                if (Token.tokenInsertValidation(Program.UserID, Program.ApplicationCode, value.ToString(), true, "Arms Approval"))
                //if (validatetoken(value.ToString()))
                {
                    Processwork();
                    sbnUpdate.Enabled = true;
                }
                else
                {

                    sbnUpdate.Enabled = true;
                    //BtnToken_Click(null, null);

                }
            }

        }

        void Processwork()
        {
            if (string.IsNullOrEmpty(selection.SelectedCount.ToString()) || selection.SelectedCount == 0)
            {
                Common.setMessageBox("No Record selected", "Receipt Reprint Approval", 1); return;
            }
            string str = string.Format("Do you really want to Approve this ({0}) number(s) of Reclassified Transaction", selection.SelectedCount);

            DialogResult results = MessageBox.Show(str, Program.ApplicationName, MessageBoxButtons.YesNo);

            if (results == DialogResult.Yes)
            {
                for (int i = 0; i < selection.SelectedCount; i++)
                {
                    //tableTrans.Rows.Add(new object[] { (selection.GetSelectedRow(i) as DataRowView)["ID"], (selection.GetSelectedRow(i) as DataRowView)["BSID"], (selection.GetSelectedRow(i) as DataRowView)["TransID"] });

                    switch (Program.intCode)
                    {
                        //case 13://Akwa Ibom state
                        //    using (var receiptAka = new AkwaIbomReceiptServices.ReceiptService())
                        //    {
                        //        dsreturn = receiptAka.LogReceiptsReprintRequest(String.Format("{0}", (selection.GetSelectedRow(i) as DataRowView)["EReceipts"]), String.Format("{0}", (selection.GetSelectedRow(i) as DataRowView)["PaymentRefNumber"]), null, Program.UserID, String.Format("{0}", (selection.GetSelectedRow(i) as DataRowView)["Comment"]), "NONE", null, null, null, null, null, null, Program.stationCode);
                        //    }
                        //    break;
                        case 20://Delta state
                            using (var receiptDelta = new DeltaBir.ReceiptService())
                            {
                                dsreturn = receiptDelta.LogReceiptsReprintRequest(String.Format("{0}", (selection.GetSelectedRow(i) as DataRowView)["EReceipts"]), String.Format("{0}", (selection.GetSelectedRow(i) as DataRowView)["PaymentRefNumber"]), null, Program.UserID, String.Format("{0}", (selection.GetSelectedRow(i) as DataRowView)["Comment"]), "NONE", null, null, null, null, null, null, Program.stationCode);
                            }
                            break;
                        case 32://kogi state
                            break;

                        case 37://ogun state
                                //using (var receiptsserv = new ReceiptService())
                                //{
                                //    dsreturn = receiptsserv.LogReceiptsReprintRequest(String.Format("{0}", row["EReceipts"]), String.Format("{0}", row["PaymentRefNumber"]), null, Program.UserID, txtReason.Text, "NONE", null, null, null, null, null, null, Program.stationCode);
                                //}
                                //break;
                        case 40://oyo state
                                //using (var receiptsServices = new OyoReceiptServices.ReceiptService())
                                //{
                                //    dsreturn = receiptsServices.LogReceiptsReprintRequest(String.Format("{0}", row["EReceipts"]), String.Format("{0}", row["PaymentRefNumber"]), null, Program.UserID, txtReason.Text, "NONE", null, null, null, null, null, null, Program.stationCode);
                                //}
                                //break;

                        default:
                            break;
                    }
                    if (dsreturn.Tables[0].Rows[0]["returnCode"].ToString() == "-1")
                    {
                        Common.setMessageBox(dsreturn.Tables[0].Rows[0]["returnmessage"].ToString(), Program.ApplicationName, 1);
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
                                string query1 = String.Format("UPDATE Receipt.tblReprintedReceipts SET FirstApprovalStatus = '{0}',FirstApprovalBy = '{1}', FirstApprovalDate = '{2}' WHERE PaymentRefNumber IN('{3}')", 1, Program.UserName, string.Format("{0:yyyy/MM/dd hh:mm:ss}", DateTime.Now), String.Format("{0}", (selection.GetSelectedRow(i) as DataRowView)["PaymentRefNumber"]));

                                using (SqlCommand sqlCommand = new SqlCommand(query1, db, transaction))
                                {
                                    sqlCommand.ExecuteNonQuery();
                                }

                            }
                            catch (Exception ex)
                            {
                                transaction.Rollback();
                                Tripous.Sys.ErrorBox(ex);
                                return;
                            }
                            transaction.Commit();

                            db.Close();
                        }
                    }
                }
                ///
                Common.setMessageBox("Request Sent for Approval,Please Click on Get Approval Record, if your Admin has approve the record....", Program.ApplicationName, 1);
                return;
            }
            else
                return;
        }

        private void SbnDisapprove_Click(object sender, EventArgs e)
        {
            string value = string.Empty;

            if (DialogResults.InputBox(@"Comments for Disapproving ", "Reclassification", ref value) == DialogResult.OK)
            {
                //value = String.Format("{0:N2}", Convert.ToDecimal(value));

                if (!string.IsNullOrWhiteSpace(value))
                {
                    if (string.IsNullOrEmpty(selection.SelectedCount.ToString()) || selection.SelectedCount == 0)
                    {
                        Common.setMessageBox("No Record selected", "Receipt Reprint Approval", 1); return;
                    }
                    else
                    {
                        for (int i = 0; i < selection.SelectedCount; i++)
                        {
                            //tableTrans.Rows.Add(new object[] { (selection.GetSelectedRow(i) as DataRowView)["ID"], (selection.GetSelectedRow(i) as DataRowView)["BSID"], (selection.GetSelectedRow(i) as DataRowView)["TransID"] });

                            //DELETE FROM Receipt.tblReprintedReceipts WHERE PaymentRefNumber
                            string query1 = string.Format("DELETE FROM Receipt.tblReprintedReceipts WHERE PaymentRefNumber='{0}'", String.Format("{0}", (selection.GetSelectedRow(i) as DataRowView)["PaymentRefNumber"]));

                            using (SqlConnection db = new SqlConnection(Logic.ConnectionString))
                            {
                                SqlTransaction transaction;

                                db.Open();

                                transaction = db.BeginTransaction();
                                try
                                {
                                    //string query1 = String.Format("UPDATE Receipt.tblReprintedReceipts SET FirstApprovalStatus=1,FirstApprovalBy = '{0}', FirstApprovalDate = '{1}' WHERE PaymentRefNumber IN('{2}')", Program.UserName, string.Format("{ 0:yyyy / MM / dd hh: mm: ss}", DateTime.Now), String.Format("{0}", (selection.GetSelectedRow(i) as DataRowView)["PaymentRefNumber"]));

                                    using (SqlCommand sqlCommand = new SqlCommand(query1, db, transaction))
                                    {
                                        sqlCommand.ExecuteNonQuery();
                                    }

                                }
                                catch (Exception ex)
                                {
                                    transaction.Rollback();
                                    Tripous.Sys.ErrorBox(ex);
                                    return;
                                }
                                transaction.Commit();

                                db.Close();
                            }
                        }
                        Common.setMessageBox("Receipt Reprint Approval disapprove ", "Receipt Reprint Approval", 3);
                        setReload(); return;
                    }

                }
                else
                {
                    Common.setMessageBox("Disapproval Comment is Empty", "Receipt Reprint Approval", 3);
                    setReload();
                    return;
                }
            }
        }
    }
}
