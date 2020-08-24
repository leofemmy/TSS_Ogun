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
using TaxSmartUtility.Classes;

namespace TaxSmartUtility.Forms
{
    public partial class FrmApproval : Form
    {
        public static FrmApproval publicStreetGroup;

        private SqlCommand _command; private SqlDataAdapter adp;

        GridCheckMarksSelection selection; DataTable tableTrans = new DataTable(); DataSet dataSet = new DataSet();

        string strtoken = string.Empty; System.Data.DataSet dts;

        public FrmApproval()
        {
            SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

            InitializeComponent();

            publicStreetGroup = this;

            setImages();

            ToolStripEvent();

            Load += OnFormLoad;

            //btnToken.Click += BtnToken_Click;

            sbnUpdate.Click += SbnUpdate_Click;

            sbnDisapprove.Click += SbnDisapprove_Click;

            tableTrans.Columns.Add("ID", typeof(int));

            tableTrans.Columns.Add("UTIN", typeof(string));

            tableTrans.Columns.Add("isPrimary", typeof(Boolean));


            OnFormLoad(null, null);

            selection = new GridCheckMarksSelection(gridView1);

            SplashScreenManager.CloseForm(false);
        }

        private void SbnDisapprove_Click(object sender, EventArgs e)
        {
            string value = string.Empty;

            if (DialogResults.InputBox(@"Comments for Disapproving ", "Reclassification", ref value) == DialogResult.OK)
            {
                //value = String.Format("{0:N2}", Convert.ToDecimal(value));

                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

                if (!string.IsNullOrWhiteSpace(value))
                {
                    if (string.IsNullOrEmpty(selection.SelectedCount.ToString()) || selection.SelectedCount == 0)
                    {
                        Common.setMessageBox("No Record selected", "Reclassification", 1); return;
                    }
                    else
                    {
                        for (int i = 0; i < selection.SelectedCount; i++)
                        {
                            tableTrans.Rows.Add(new object[] { (selection.GetSelectedRow(i) as DataRowView)["MergeTinID"], (selection.GetSelectedRow(i) as DataRowView)["UTIN"], (selection.GetSelectedRow(i) as DataRowView)["IsprimaryTin"] });
                        }

                        if (tableTrans.Columns.Contains("ID")) tableTrans.Columns.Remove("ID");

                        using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
                        {
                            connect.Open();
                            _command = new SqlCommand("TaxAgentMergeTinDisapproved", connect) { CommandType = CommandType.StoredProcedure };
                            _command.Parameters.Add(new SqlParameter("@pTransaction", SqlDbType.Structured)).Value = tableTrans;
                            _command.Parameters.Add(new SqlParameter("@user", SqlDbType.VarChar)).Value = Program.UserID;
                            _command.CommandTimeout = 0;

                            using (System.Data.DataSet dts = new System.Data.DataSet())
                            {
                                dts.Clear();
                                adp = new SqlDataAdapter(_command);
                                adp.Fill(dts);
                                connect.Close();

                                if (dts.Tables[0].Rows[0]["returnCode"].ToString() != "00")
                                {
                                    Common.setMessageBox(dts.Tables[0].Rows[0]["returnMessage"].ToString(), Program.ApplicationName, 2);
                                    return;
                                }
                                else
                                {
                                    Common.setMessageBox(dts.Tables[0].Rows[0]["returnMessage"].ToString(), Program.ApplicationName, 2);
                                    return;
                                }


                            }
                        }


                    }
                }
                SplashScreenManager.CloseForm(false);
            }
           
        }

        private void SbnUpdate_Click(object sender, EventArgs e)
        {
            dowork();
        }

        //private void BtnToken_Click(object sender, EventArgs e)
        //{
        //    if (dotoken())
        //    {
        //        sbnUpdate.Enabled = true;
        //    }
        //    else
        //    {
        //        sbnUpdate.Enabled = false; BtnToken_Click(null, null);
        //    }
        //}

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
            sbnUpdate.Image = MDIMains.publicMDIParent.i32x32.Images[6];
            sbnDisapprove.Image = MDIMains.publicMDIParent.i32x32.Images[6];
            btnToken.Image = MDIMains.publicMDIParent.i32x32.Images[7];
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

            //sbnUpdate.Click += SbnUpdate_Click;
            //sbnDisapprove.Click += SbnDisapprove_Click; btnToken.Click += BtnToken_Click;
        }

        private void setReload()
        {
            try
            {
                using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
                {
                    connect.Open();
                    _command = new SqlCommand("loadTaxAgentMergeTin", connect) { CommandType = CommandType.StoredProcedure };
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

                            //AddCombCredit();

                            gridView1.OptionsBehavior.Editable = false;

                            //gridView1.Columns["Amount"].DisplayFormat.FormatType = FormatType.Numeric;
                            //gridView1.Columns["Amount"].DisplayFormat.FormatString = "n2";
                            //gridView1.Columns["Date"].DisplayFormat.FormatType = FormatType.DateTime;
                            //gridView1.Columns["Date"].DisplayFormat.FormatString = "dd/MM/yyyy";

                            //gridView1.Columns["ReclassifiedDate"].DisplayFormat.FormatType = FormatType.DateTime;
                            //gridView1.Columns["ReclassifiedDate"].DisplayFormat.FormatString = "dd/MM/yyyy";

                            ////gridView1.Columns["ReclassifiedTime"].DisplayFormat.FormatType = FormatType.DateTime;
                            ////gridView1.Columns["ReclassifiedTime"].DisplayFormat.FormatString = "HHMMSS";

                            //gridView1.Columns["OldTransID"].ColumnEdit = repComboLookBoxCredit;
                            //gridView1.Columns["OldTransID"].Caption = "From";

                            //gridView1.Columns["TransID"].ColumnEdit = repComboLookBoxCredit;
                            //gridView1.Columns["TransID"].Caption = "To";


                            //gridView1.Columns["Amount"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                            //gridView1.Columns["Amount"].SummaryItem.FieldName = "Amount";
                            //gridView1.Columns["Amount"].SummaryItem.DisplayFormat = "Total = {0:n}";
                            //gridView1.Columns["BSID"].Visible = false;
                            gridView1.Columns["MergeTinID"].Visible = false;

                            //gridView1.OptionsView.ColumnAutoWidth = false;
                            //gridView1.OptionsView.ShowFooter = true;

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

        bool dotoken()
        {
            strtoken = Logic.GenerateToken();

            if (tokenInsertValidation(Program.UserID, Program.ApplicationCode, strtoken, false))
            {
                try
                {
                    var procesSms = new ProcessSms.ProcessSms();

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

        void dowork()
        {
            string value = string.Empty;

            strtoken = string.Empty;

            Token.dotoken();

            if (DialogResults.InputBox(@"OTP", string.Format("Kindly enter the token to Authorize this transaction.", $"********{Program.Userphone.Substring(7)}"), ref value) == DialogResult.OK)
            {
                if (Token.tokenInsertValidation(Program.UserID, Program.ApplicationCode, value.ToString(), true, string.Format("{0}", this.groupControl1.Text.Trim())))
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

        void procesfinalmerge(DataSet dstf)
        {

            if (dstf.Tables[0].Columns.Contains("MergeTinID")) dstf.Tables[0].Columns.Remove("MergeTinID");

            using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
            {
                connect.Open();
                _command = new SqlCommand("TaxAgentMergeTinProcess", connect) { CommandType = CommandType.StoredProcedure };
                _command.Parameters.Add(new SqlParameter("@pTransaction", SqlDbType.Structured)).Value = dstf.Tables[0];
                _command.Parameters.Add(new SqlParameter("@user", SqlDbType.VarChar)).Value = Program.UserID;
                _command.CommandTimeout = 0;

                using (System.Data.DataSet ds = new System.Data.DataSet())
                {
                    ds.Clear();
                    adp = new SqlDataAdapter(_command);
                    adp.Fill(ds);
                    connect.Close();

                    if (ds.Tables[0].Rows[0]["returnCode"].ToString() == "0")
                    {
                        Common.setMessageBox(ds.Tables[0].Rows[0]["returnMessage"].ToString(), Program.ApplicationName, 2);
                       
                        return;
                    }
                    else
                    {
                        Common.setMessageBox(ds.Tables[0].Rows[0]["returnMessage"].ToString(), Program.ApplicationName, 2);
                        return;
                    }
                    
                }
                setReload();
            }
          
        }
        void Processwork()
        {
            DataSet dataSet = new DataSet();

            System.Data.DataSet dtsg = new System.Data.DataSet();

            dtsg.Clear();

            if (string.IsNullOrEmpty(selection.SelectedCount.ToString()) || selection.SelectedCount == 0)
            {
                Common.setMessageBox("No Record selected", "Approval", 1); return;
            }


            string str = string.Format("Do you really want to Approve this ({0}) number(s) of Tax Agent Merging", selection.SelectedCount);

            DialogResult results = MessageBox.Show(str, Program.ApplicationName, MessageBoxButtons.YesNo);

            if (results == DialogResult.Yes)
            {
                for (int i = 0; i < selection.SelectedCount; i++)
                {
                    tableTrans.Rows.Add(new object[] { (selection.GetSelectedRow(i) as DataRowView)["MergeTinID"], (selection.GetSelectedRow(i) as DataRowView)["UTIN"], (selection.GetSelectedRow(i) as DataRowView)["IsprimaryTin"] });
                }

                if (tableTrans.Columns.Contains("ID")) tableTrans.Columns.Remove("ID");

                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

                using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
                {
                    connect.Open();
                    _command = new SqlCommand("TaxAgentMergeTinApproval", connect) { CommandType = CommandType.StoredProcedure };
                    _command.Parameters.Add(new SqlParameter("@pTransaction", SqlDbType.Structured)).Value = tableTrans;
                    _command.Parameters.Add(new SqlParameter("@user", SqlDbType.VarChar)).Value = Program.UserID;
                    _command.CommandTimeout = 0;

                    using (System.Data.DataSet dts = new System.Data.DataSet())
                    {
                        dts.Clear();
                        adp = new SqlDataAdapter(_command);
                        adp.Fill(dts);
                        connect.Close();

                        if (dts != null || dts.Tables[0].Rows.Count > 0)
                        {
                            
                            switch (Program.intCode)
                            {
                                case 13://Akwa Ibom state
                                    break;
                                case 20://Delta state
                                    using (var service = new ServiceRefernce.CollectionManager())
                                    {
                                        dtsg = service.TaxAgentMerger(dts);
                                    }
                                    break;
                                case 32://kogi state
                                    break;
                                case 37://ogun state

                                    break;
                                case 40://oyo state
                                    break;
                                default:
                                    break;
                            }
                            if (dtsg.Tables[0].Rows[0]["returnCode"].ToString() != "00")
                            {
                                Common.setMessageBox(dtsg.Tables[0].Rows[0]["returnMessage"].ToString(), Program.ApplicationName, 2);
                                return;
                            }
                            else
                            {

                                procesfinalmerge(dts);
                            }
                        }
                        else
                        {
                            return;
                        }

                    }
                }
                SplashScreenManager.CloseForm(false);


               
            }
        }

    }
}
