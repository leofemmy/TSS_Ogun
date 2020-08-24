using Collection.Classess;
using Collections;
using DevExpress.Utils;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Selection;
using DevExpress.XtraSplashScreen;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TaxSmartSuite.Class;

namespace Collection.Forms
{
    public partial class FrmArmsApproval : Form
    {
        public static FrmArmsApproval publicStreetGroup;

        private SqlCommand _command;

        private SqlDataAdapter adp;

        GridColumn colView2 = new GridColumn();

        RepositoryItemGridLookUpEdit repComboLookBoxCredit = new RepositoryItemGridLookUpEdit();

        GridColumn colView = new GridColumn();
        GridCheckMarksSelection selection;

        DataTable tableTrans = new DataTable();

        System.Data.DataSet dataSet = new System.Data.DataSet();

        System.Data.DataSet dts; string strtoken = string.Empty;

        System.Data.DataSet dsreturn = new System.Data.DataSet();
        public FrmArmsApproval()
        {
            SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

            InitializeComponent();

            publicStreetGroup = this;

            setImages();

            ToolStripEvent();

            Load += OnFormLoad;

            OnFormLoad(null, null);


            tableTrans.Columns.Add("PaymentRefNumber", typeof(string));

            tableTrans.Columns.Add("Userid", typeof(string));


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
            sbnUpdate.Image = MDIMain.publicMDIParent.i32x32.Images[6];
            sbnDisapprove.Image = MDIMain.publicMDIParent.i32x32.Images[6];
            btnToken.Image = MDIMain.publicMDIParent.i32x32.Images[7];
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
            //sbnDisapprove.Click += SbnDisapprove_Click;
            btnToken.Click += BtnToken_Click;
        }

        private void setReload()
        {
            try
            {
                using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
                {
                    connect.Open();
                    _command = new SqlCommand("Receipt.ArmsLoadWaitingApprovalRecord", connect) { CommandType = CommandType.StoredProcedure };
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

                            gridControl1.DataSource = ds.Tables[1];


                            gridView1.OptionsBehavior.Editable = false;

                            gridView1.Columns["Amount"].DisplayFormat.FormatType = FormatType.Numeric;
                            gridView1.Columns["Amount"].DisplayFormat.FormatString = "n2";
                            gridView1.Columns["RequestDate"].DisplayFormat.FormatType = FormatType.DateTime;
                            gridView1.Columns["RequestDate"].DisplayFormat.FormatString = "dd/MM/yyyy";

                            //gridView1.Columns["ReclassifiedDate"].DisplayFormat.FormatType = FormatType.DateTime;
                            //gridView1.Columns["ReclassifiedDate"].DisplayFormat.FormatString = "dd/MM/yyyy";

                            ////gridView1.Columns["ReclassifiedTime"].DisplayFormat.FormatType = FormatType.DateTime;
                            ////gridView1.Columns["ReclassifiedTime"].DisplayFormat.FormatString = "HHMMSS";

                            //gridView1.Columns["OldTransID"].ColumnEdit = repComboLookBoxCredit;
                            //gridView1.Columns["OldTransID"].Caption = "From";

                            //gridView1.Columns["TransID"].ColumnEdit = repComboLookBoxCredit;
                            //gridView1.Columns["TransID"].Caption = "To";


                            gridView1.Columns["Amount"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                            gridView1.Columns["Amount"].SummaryItem.FieldName = "Amount";
                            gridView1.Columns["Amount"].SummaryItem.DisplayFormat = "Total = {0:n}";
                            //gridView1.Columns["BSID"].Visible = false;
                            //gridView1.Columns["ID"].Visible = false;

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

        private void BtnToken_Click(object sender, EventArgs e)
        {
            if (dotoken())
            {
                sbnUpdate.Enabled = true;
            }
            else
            {
                sbnUpdate.Enabled = false; BtnToken_Click(null, null);
            }
        }

        bool dotoken()
        {
            strtoken = Token.GenerateToken();

            if (tokenInsertValidation(Program.UserID, Program.ApplicationCode, strtoken, false))
            {
                try
                {
                    var procesSms = new processSms.ProcessSms();

                    string strprocessSme = procesSms.SendSms(Program.Userphone, "Token", strtoken);

                    if (strprocessSme.Contains("Failed"))
                    {
                        Tripous.Sys.ErrorBox(strprocessSme.ToString());

                        Common.setMessageBox(strprocessSme.ToString(), "Get Token", 1);

                        return false;
                    }
                    else
                    {
                        Common.setMessageBox(string.Format("Token Request sent to your registered number {0}.", $"********{Program.Userphone.Substring(7)}"), "Token Request", 1);

                        //dt = DateTime.Now;

                        return true;
                    }

                }
                catch (Exception ex)
                {
                    Tripous.Sys.ErrorBox(ex.Message);

                    return false;
                }


            }
            else
                return false;
        }

        bool validatetoken(string valtoken)
        {
            bool respones;

            if (tokenInsertValidation(Program.UserID, Program.ApplicationCode, valtoken, true))
            {
                respones = true;
            }
            else
            {
                respones = false;
            }

            return respones;
        }

        bool tokenInsertValidation(string userid, string ApplicationCode, string strtoken, bool status)
        {
            using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
            {
                connect.Open();

                _command = new SqlCommand("Reconciliation.tokenInsertValidation", connect) { CommandType = CommandType.StoredProcedure };
                _command.Parameters.Add(new SqlParameter("@userid", SqlDbType.VarChar)).Value = userid;
                _command.Parameters.Add(new SqlParameter("@usertoken", SqlDbType.VarChar)).Value = strtoken;
                _command.Parameters.Add(new SqlParameter("@application", SqlDbType.VarChar)).Value = ApplicationCode;
                _command.Parameters.Add(new SqlParameter("@validDatetime", SqlDbType.DateTime)).Value = DateTime.Now;
                _command.Parameters.Add(new SqlParameter("@isValid", SqlDbType.Bit)).Value = status;
                _command.Parameters.Add(new SqlParameter("@Description", SqlDbType.VarChar)).Value = "Arms Approval";

                using (System.Data.DataSet ds = new System.Data.DataSet())
                {
                    ds.Clear();
                    adp = new SqlDataAdapter(_command);
                    adp.Fill(ds);
                    connect.Close();

                    if (ds.Tables[0].Rows[0]["returnCode"].ToString() != "00")
                    {
                        Common.setMessageBox(ds.Tables[0].Rows[0]["returnMessage"].ToString(), Program.ApplicationName, 2);

                        return false;

                    }
                    else
                    {
                        return true;
                    }

                }
            }
        }

        private void SbnUpdate_Click(object sender, EventArgs e)
        {
            dowork();
        }

        //private void SbnDisapprove_Click(object sender, EventArgs e)
        //{
        //    string value = string.Empty;

        //    if (DialogResults.InputBox(@"Comments for Disapproving ", "Reclassification", ref value) == DialogResult.OK)
        //    {
        //        //value = String.Format("{0:N2}", Convert.ToDecimal(value));

        //        if (!string.IsNullOrWhiteSpace(value))
        //        {
        //            if (string.IsNullOrEmpty(selection.SelectedCount.ToString()) || selection.SelectedCount == 0)
        //            {
        //                Common.setMessageBox("No Record selected", "Reclassification", 1); return;
        //            }
        //            else
        //            {
        //                for (int i = 0; i < selection.SelectedCount; i++)
        //                {
        //    tableTrans.Rows.Add(new object[] { (selection.GetSelectedRow(i) as DataRowView)["ID"], (selection.GetSelectedRow(i) as DataRowView)["BSID"], (selection.GetSelectedRow(i) as DataRowView)["TransID"]
        //});
        //                }

        //                using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
        //                {
        //                    connect.Open();
        //                    _command = new SqlCommand("doReclassifiedRequestDisapproved", connect) { CommandType = CommandType.StoredProcedure };
        //                    _command.Parameters.Add(new SqlParameter("@pTransaction", SqlDbType.Structured)).Value = tableTrans;
        //                    _command.Parameters.Add(new SqlParameter("@userid", SqlDbType.VarChar)).Value = Program.UserID;
        //                    _command.Parameters.Add(new SqlParameter("@comment", SqlDbType.VarChar)).Value = value;

        //                    using (System.Data.DataSet ds = new System.Data.DataSet())
        //                    {
        //                        ds.Clear();
        //                        adp = new SqlDataAdapter(_command);
        //                        adp.Fill(ds);
        //                        connect.Close();

        //                        if (ds.Tables[0].Rows[0]["returnCode"].ToString() == "00")
        //                        {
        //                            Common.setMessageBox(ds.Tables[0].Rows[0]["returnMessage"].ToString(), Program.ApplicationName, 2);
        //                            setReload();
        //                            return;
        //                        }
        //                        else
        //                        {
        //                            Tripous.Sys.ErrorBox(String.Format("{0}", ds.Tables[0].Rows[0]["returnMessage"].ToString()));
        //                            return;
        //                        }
        //                    }

        //                }
        //            }

        //        }
        //        else
        //        {
        //            Common.setMessageBox("Disapproval Comment is Empty", "Reclassification", 3);

        //            return;
        //        }
        //    }
        //}

        void dowork()
        {
            string value = string.Empty;

            strtoken = string.Empty;

            if (DialogResults.InputBox(@"OTP", string.Format("Kindly enter the token to Authorize this transaction.", $"********{Program.Userphone.Substring(7)}"), ref value) == DialogResult.OK)
            {
                if (validatetoken(value.ToString()))
                {
                    Processwork();
                    sbnUpdate.Enabled = false;
                }
                else
                {
                    sbnUpdate.Enabled = false;
                    BtnToken_Click(null, null);

                }
            }

        }

        void Processwork()
        {
            System.Data.DataSet dataSet = new System.Data.DataSet();

            dts = new System.Data.DataSet();

            dts.Clear(); tableTrans.Clear();


            if (string.IsNullOrEmpty(selection.SelectedCount.ToString()) || selection.SelectedCount == 0)
            {
                Common.setMessageBox("No Record selected", "Arms Payments", 1); return;
            }
            else
            {
                string str = string.Format("Do you really want to Approve this ({0}) number(s) of Arms Payments", selection.SelectedCount);

                DialogResult results = MessageBox.Show(str, Program.ApplicationName, MessageBoxButtons.YesNo);

                if (results == DialogResult.Yes)
                {
                    for (int i = 0; i < selection.SelectedCount; i++)
                    {
                        tableTrans.Rows.Add(new object[] { (selection.GetSelectedRow(i) as DataRowView)["PaymentRefNumber"], Program.UserID, });
                    }
                }

                try
                {
                    using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
                    {
                        connect.Open();
                        _command = new SqlCommand("ArmsApproval", connect) { CommandType = CommandType.StoredProcedure };
                        _command.Parameters.Add(new SqlParameter("@pTransaction", SqlDbType.Structured)).Value = tableTrans;
                        //_command.Parameters.Add(new SqlParameter("@userid", SqlDbType.VarChar)).Value = Program.UserID;
                        _command.CommandTimeout = 0;
                        using (System.Data.DataSet ds = new System.Data.DataSet())
                        {
                            ds.Clear();
                            adp = new SqlDataAdapter(_command);
                            adp.Fill(ds);
                            connect.Close();

                            if (ds.Tables[0].Rows[0]["returnCode"].ToString() == "00")
                            {
                                Common.setMessageBox(ds.Tables[0].Rows[0]["returnMessage"].ToString(), Program.ApplicationName, 2);

                                doSending(ds);
                                ////using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
                                ////{
                                //if (ds.Tables[0].Columns.Contains("Narration")) ds.Tables[0].Columns.Remove("Narration");

                                //connect.Open();
                                //_command = new SqlCommand("ReclassifiedTransactionLogRequest", connect) { CommandType = CommandType.StoredProcedure };
                                //_command.Parameters.Add(new SqlParameter("@pTransaction", SqlDbType.Structured)).Value = ds.Tables[0];
                                //_command.Parameters.Add(new SqlParameter("@userid", SqlDbType.VarChar)).Value = Program.UserID;

                                //using (System.Data.DataSet ds1 = new System.Data.DataSet())
                                //{
                                //    ds1.Clear();
                                //    adp = new SqlDataAdapter(_command);
                                //    adp.Fill(ds1);
                                //    connect.Close();

                                //    if (ds1.Tables[0].Rows[0]["returnCode"].ToString() != "00")
                                //    {
                                //        Common.setMessageBox(ds1.Tables[0].Rows[0]["returnMessage"].ToString(), Program.ApplicationName, 2);
                                //        return;
                                //    }
                                //    else
                                //    {
                                //        Common.setMessageBox(ds1.Tables[0].Rows[0]["returnMessage"].ToString(), Program.ApplicationName, 2);
                                //        setReload();
                                //        return;
                                //    }

                                //}
                                ////}
                            }
                            else
                            {
                                Tripous.Sys.ErrorBox(String.Format("{0}", dts.Tables[0].Rows[0]["returnMessage"].ToString()));
                                return;
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

        void doSending(System.Data.DataSet dst)
        {
            foreach (DataRow row in dst.Tables[1].Rows)
            {
                //Console.WriteLine(row["ImagePath"]);
                if (row != null)
                {
                    switch (Program.intCode)
                    {
                        case 13://Akwa Ibom state
                            using (var receiptAka = new AkwaIbomReceiptServices.ReceiptService())
                            {
                                dsreturn = receiptAka.LogReceiptsReprintRequest(String.Format("{0}", row["EReceipts"]), String.Format("{0}", row["PaymentRefNumber"]), null, Program.UserID, "Arms Records", "NONE", null, null, null, null, null, null, Program.stationCode);
                            }
                            break;
                        case 20://Delta state
                            using (var receiptDelta = new DeltaBir.ReceiptService())
                            {
                                dsreturn = receiptDelta.LogReceiptsReprintRequest(String.Format("{0}", row["EReceipts"]), String.Format("{0}", row["PaymentRefNumber"]), null, Program.UserID, "Arms Records", "NONE", null, null, null, null, null, null, Program.stationCode);
                            }
                            break;
                        case 32://kogi state
                            break;

                        case 37://ogun state
                            using (var receiptsserv = new ReceiptServices.ReceiptService())
                            {
                                dsreturn = receiptsserv.LogReceiptsReprintRequest(String.Format("{0}", row["EReceipts"]), String.Format("{0}", row["PaymentRefNumber"]), null, Program.UserID, "Arms Records", "NONE", null, null, null, null, null, null, Program.stationCode);
                            }
                            break;
                        case 40://oyo state
                            using (var receiptsServices = new OyoReceiptServices.ReceiptService())
                            {
                                dsreturn = receiptsServices.LogReceiptsReprintRequest(String.Format("{0}", row["EReceipts"]), String.Format("{0}", row["PaymentRefNumber"]), null, Program.UserID, "Arms Records", "NONE", null, null, null, null, null, null, Program.stationCode);
                            }
                            break;

                        default:
                            break;
                    }
                }
            }
            //call procedure for insert

            using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
            {
                connect.Open();
                _command = new SqlCommand("ArmsReprintInsert", connect) { CommandType = CommandType.StoredProcedure };
                _command.Parameters.Add(new SqlParameter("@pTransaction", SqlDbType.Structured)).Value = dst.Tables[1];
                _command.CommandTimeout = 0;

                using (System.Data.DataSet ds = new System.Data.DataSet())
                {
                    ds.Clear();
                    adp = new SqlDataAdapter(_command);
                    adp.Fill(ds);
                    connect.Close();

                    if (ds.Tables[0].Rows[0]["returnCode"].ToString() == "00")
                    {
                        Common.setMessageBox(ds.Tables[0].Rows[0]["returnMessage"].ToString(), Program.ApplicationName, 2);
                        return;
                    }
                    else
                    {
                        Tripous.Sys.ErrorBox(String.Format("{0}", dts.Tables[0].Rows[0]["returnMessage"].ToString()));
                        return;
                    }
                }
            }
        }
    }
}
