using BankReconciliation.Class;
using BankReconciliation.Report;
using DevExpress.XtraSplashScreen;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TaxSmartSuite.Class;
using DevExpress.XtraReports.UI;
using DevExpress.XtraReports.Parameters;
using DevExpress.XtraPrinting;
namespace BankReconciliation.Forms
{
    public partial class Frmunswept : Form
    {
        public static Frmunswept publicStreetGroup;

        private bool Isbank = false; private bool isRecord = false; private SqlCommand _command; private SqlDataAdapter adp;

        public Frmunswept()
        {
            SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

            InitializeComponent();

            publicStreetGroup = this;

            setImages();

            ToolStripEvent();

            Load += OnFormLoad;

            cboBank.SelectedIndexChanged += cboBank_SelectedIndexChanged;

            cboBank.KeyPress += cboBank_KeyPress;

            btnSelect.Click += btnSelect_Click;

            OnFormLoad(null, null);

            SplashScreenManager.CloseForm(false);

        }

        void btnSelect_Click(object sender, EventArgs e)
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

                if (string.IsNullOrEmpty((string)(cboBank.SelectedValue)))
                {
                    Common.setEmptyField("BanK Name", Program.ApplicationName);
                    cboBank.Focus();
                    return;
                }
                else if (string.IsNullOrEmpty((string)(cboAccount.SelectedValue.ToString())))
                {
                    Common.setEmptyField("Account Number", Program.ApplicationName); cboAccount.Focus(); return;
                }
                else
                {
                    if (radioGroup1.EditValue.ToString() == "1")//unswept transaction
                    {
                        using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
                        {
                            connect.Open();

                            _command = new SqlCommand("doUnsweptTransaction", connect) { CommandType = CommandType.StoredProcedure };
                            _command.Parameters.Add(new SqlParameter("@bankcode", SqlDbType.VarChar)).Value = cboBank.SelectedValue;
                            _command.Parameters.Add(new SqlParameter("@startdate", SqlDbType.VarChar)).Value = string.Format("{0:yyyy/MM/dd 00:00:00}", dtpStart.Value);
                            _command.Parameters.Add(new SqlParameter("@enddate", SqlDbType.VarChar)).Value = string.Format("{0:yyyy/MM/dd 23:59:59}", dtpEnd.Value);
                            _command.Parameters.Add(new SqlParameter("@AccountId", SqlDbType.Int)).Value = Convert.ToInt32(cboAccount.SelectedValue);

                            _command.CommandTimeout = 0;

                            using (System.Data.DataSet ds = new System.Data.DataSet())
                            {
                                adp = new SqlDataAdapter(_command);
                                adp.Fill(ds, "ViewUnsweptTransaction");
                                //Dts = ds.Tables[0];
                                connect.Close();

                                if (ds.Tables[0].Rows[0]["returnCode"].ToString() == "00")
                                {
                                    if (ds.Tables[1].Rows.Count == 0)
                                    {
                                        MessageBox.Show("No Record for Unswpt Transaction"); return;
                                    }
                                    else
                                    {
                                        XtraRepUnswept unswept = new XtraRepUnswept { DataSource = ds.Tables[1], DataAdapter = adp, DataMember = "ViewUnsweptTransaction", RequestParameters = false };
                                        unswept.xrLabel12.Text = string.Format("{0} STATE GOVERNMENT", Program.StateName.ToUpper());
                                        unswept.xrLabel13.Text = "PAYMENTS NOT POSTED THROUGH PAYDIRECT PLATFORM";
                                        unswept.ShowPreviewDialog();
                                    }

                                    //xtraRepunswept repManifest = new XRepManifest { DataSource = ds.Tables[1], DataAdapter = adp, DataMember = "CollectionReportTable", RequestParameters = false };
                                }
                                else
                                {
                                    Tripous.Sys.ErrorBox(String.Format("{0}{1}", ds.Tables[0].Rows[0]["returnCode"].ToString(), ds.Tables[0].Rows[0]["returnMessage"].ToString()));

                                    return;
                                }
                            }
                        }
                    }
                    else
                    {
                        using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
                        {
                            connect.Open();

                            _command = new SqlCommand("doSweptReport", connect) { CommandType = CommandType.StoredProcedure };
                            _command.Parameters.Add(new SqlParameter("@bankcode", SqlDbType.VarChar)).Value = cboBank.SelectedValue;
                            _command.Parameters.Add(new SqlParameter("@startdate", SqlDbType.VarChar)).Value = string.Format("{0:yyyy/MM/dd 00:00:00}", dtpStart.Value);
                            _command.Parameters.Add(new SqlParameter("@enddate", SqlDbType.VarChar)).Value = string.Format("{0:yyyy/MM/dd 23:59:59}", dtpEnd.Value);
                            _command.Parameters.Add(new SqlParameter("@AccountId", SqlDbType.Int)).Value = Convert.ToInt32(cboAccount.SelectedValue);

                            _command.CommandTimeout = 0;

                            using (System.Data.DataSet ds = new System.Data.DataSet())
                            {
                                adp = new SqlDataAdapter(_command);
                                adp.Fill(ds, "ViewsweptTransaction");
                                //Dts = ds.Tables[0];
                                connect.Close();

                                if (ds.Tables[0].Rows[0]["returnCode"].ToString() == "00")
                                {

                                    if (ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0)
                                    {
                                        XtraSwept swept = new XtraSwept { DataSource = ds.Tables[1], DataAdapter = adp, DataMember = "ViewUnsweptTransaction", RequestParameters = false };
                                        swept.xrLabel17.Text = string.Format("{0} STATE GOVERNMENT", Program.StateName.ToUpper());
                                        swept.xrLabel19.Text = "SWEPT TRANSACTION REPORT";
                                        swept.ShowPreviewDialog();
                                    }
                                    else
                                    { Common.setMessageBox("No Record Found for Swept Transaction", Program.ApplicationName, 1); return; }

                                    //xtraRepunswept repManifest = new XRepManifest { DataSource = ds.Tables[1], DataAdapter = adp, DataMember = "CollectionReportTable", RequestParameters = false };
                                }
                                else
                                {
                                    Tripous.Sys.ErrorBox(String.Format("{0}{1}", ds.Tables[0].Rows[0]["returnCode"].ToString(), ds.Tables[0].Rows[0]["returnMessage"].ToString()));

                                    return;
                                }
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                SplashScreenManager.CloseForm(false);
            }
        }

        void cboBank_KeyPress(object sender, KeyPressEventArgs e)
        {
            Methods.AutoComplete(cboBank, e, true);
        }

        void cboBank_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboBank.SelectedValue != null && !Isbank)
            {
                setDBComboBoxAcct();

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
            //bttnClose.Image = MDIMain.publicMDIParent.i32x32.Images[9];
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
            setDBComboBox();

        }

        public void setDBComboBox()
        {
            DataTable Dt;

            //connect.connect.Close();

            using (var ds = new System.Data.DataSet())
            {
                //connect.connect.Open();
                using (SqlDataAdapter ada = new SqlDataAdapter((string)"SELECT BankShortCode,BankName FROM Collection.tblBank", Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }

                Dt = ds.Tables[0];
            }

            Common.setComboList(cboBank, Dt, "BankShortCode", "BankName");

            cboBank.SelectedIndex = -1;


        }

        void setDBComboBoxAcct()
        {
            try
            {
                isRecord = true;

                using (var ds = new System.Data.DataSet())
                {
                    using (SqlDataAdapter ada = new SqlDataAdapter(String.Format("SELECT BankAccountID,AccountNumber FROM ViewCurrencyBankAccount WHERE BankShortCode='{0}'", cboBank.SelectedValue.ToString()), Logic.ConnectionString))
                    {
                        ada.Fill(ds, "table");
                    }

                    Common.setComboList(cboAccount, ds.Tables[0], "BankAccountID", "AccountNumber");

                }

                cboAccount.SelectedIndex = -1; isRecord = false;
            }
            catch (Exception ex)
            {
                Tripous.Sys.ErrorBox(String.Format("{0}{1}", ex.Message, ex.StackTrace)); return;
            }
        }


    }
}
