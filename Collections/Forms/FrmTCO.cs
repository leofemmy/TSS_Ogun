using Collection.Classess;
using Collection.Report;
using Collections;
using DevExpress.XtraReports.UI;
using DevExpress.XtraSplashScreen;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;
using TaxSmartSuite.Class;

namespace Collection.Forms
{
    public partial class FrmTCO : Form
    {
        public static FrmTCO publicInstance;

        protected TransactionTypeCode iTransType;

        private SqlCommand _command;

        private System.Data.DataSet ds;

        SqlDataAdapter adp;
        public FrmTCO()
        {
            InitializeComponent();

            publicInstance = this;

            Load += OnFormLoad;

            setImages();

            ToolStripEvent();

            iTransType = TransactionTypeCode.New;

            OnFormLoad(null, null);

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

            //bttnCancel.Image = MDIMain.publicMDIParent.i32x32.Images[9];
            //btnMain.Image = MDIMain.publicMDIParent.i32x32.Images[8];
            bttnUpdate.Image = MDIMain.publicMDIParent.i32x32.Images[29];
            //btnSearch.Image = MDIMain.publicMDIParent.i32x32.Images[7];
            //btnlist.Image = MDIMain.publicMDIParent.i32x32.Images[6];

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
                iTransType = TransactionTypeCode.New;
                ShowForm();
            }
            else if (sender == tsbEdit)
            {



                //boolIsUpdate = true;

            }

        }

        void OnFormLoad(object sender, EventArgs e)
        {
            ShowForm();

            bttnUpdate.Click += BttnUpdate_Click;

        }

        private void BttnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);


                System.Data.DataSet response = new System.Data.DataSet();

                using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
                {
                    connect.Open();
                    _command = new SqlCommand("doTCOReportManifest", connect)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    _command.Parameters.Add(new SqlParameter("@start", SqlDbType.DateTime)).Value = string.Format("{0:yyyy-MM-dd 00:00:00}", DTPDateselect.Value.Date);
                    _command.Parameters.Add(new SqlParameter("@enddate", SqlDbType.DateTime)).Value = string.Format("{0:yyyy-MM-dd 23:59:59}", DTPDateselect.Value.Date);

                    adp = new SqlDataAdapter(_command);

                    adp.Fill(response, "CollectionReportTable");

                    connect.Close();

                    if (response.Tables[0].Rows[0]["returnCode"].ToString() == "00")
                    {
                        XRepManifest repManifest = new XRepManifest();
                        XtraRepPayment payment = new XtraRepPayment(); XtraRepPayment Reprint = new XtraRepPayment();
                        XtraRepReversal Reversal = new XtraRepReversal();
                        XRepManifest UnReceipt = new XRepManifest(); XtraRepUnreceipted Unreceipted = new XtraRepUnreceipted();

                        if (response.Tables[1] != null && response.Tables[1].Rows.Count > 0)//Main Manifest
                        {
                            repManifest.DataSource = response.Tables[1];
                            repManifest.DataAdapter = adp;

                            repManifest.DataMember = "CollectionReportTable";
                            repManifest.RequestParameters = false;
                            //};
                            repManifest.xrLabel10.Text = Program.UserID;
                            repManifest.xrLabel9.Text = string.Format("{0} STATE GOVERNMENT",
                                Program.StateName.ToUpper());
                            //repManifest.ShowPreviewDialog();
                            repManifest.CreateDocument();

                            if (response.Tables[2] != null && response.Tables[2].Rows.Count > 0)//unreceipted receipt Manifest
                            {
                                //UnReceipt.DataSource = response.Tables[2];
                                //UnReceipt.DataAdapter = adp;
                                //UnReceipt.DataMember = "CollectionReportTable";
                                //UnReceipt.RequestParameters = false;
                                //UnReceipt.xrLabel10.Text = Program.UserID;
                                //UnReceipt.xrLabel12.Text = "MANIFEST OF UNRECEIPTED ";
                                //UnReceipt.xrLabel9.Text = string.Format("{0} STATE GOVERNMENT",
                                //    Program.StateName.ToUpper());

                                //UnReceipt.CreateDocument();

                                Unreceipted.DataSource = response.Tables[2];
                                Unreceipted.DataAdapter = adp;
                                Unreceipted.DataMember = "CollectionReportTable";
                                Unreceipted.RequestParameters = false;
                                Unreceipted.xrLabel10.Text = Program.UserID;
                                Unreceipted.xrLabel2.Text = "(BANK BRANCH)";
                                Unreceipted.xrLabel12.Text = "MANIFEST OF UNRECEIPTED PAYMENT";
                                Unreceipted.xrLabel9.Text = string.Format("{0} STATE GOVERNMENT",
                                    Program.StateName.ToUpper());
                                //Unreceipted.ShowPreviewDialog();
                                //return;
                                Unreceipted.CreateDocument();
                            }
                            if (checkEdit1.Checked)///payment
                            {
                                if (response.Tables[3] != null && response.Tables[3].Rows.Count > 0)//Payment Change Manifest
                                {
                                    var listresult = (from DataRow row in response.Tables[3].Rows
                                                      select new DataSet.Main
                                                      {
                                                          PaymentRefNumber = row["PaymentRefNumber"] as string,
                                                          PayerName = row["PayerName"] as string,
                                                          Receipt = row["EReceipts"] as string,
                                                          ApprovalDate = Convert.ToDateTime(row["ApprovalDate"]),
                                                          OldValue = row["OldRecord"] as string,
                                                          ApprovalBy = row["ApprovalBy"] as string,
                                                          NewValue = row["NewRecord"] as string,
                                                          Description = row["Description"] as string,
                                                          Type = row["Type"] as string,
                                                          Amount = Convert.ToDecimal(row["Amount"])


                                                      }).ToList();
                                    //XtraRepPayment payment = new XtraRepPayment();
                                    payment.xrLabel10.Text = "Amended Receipts"; payment.xrLabel24.Text = Program.UserID;
                                    payment.xrLabel2.Text = string.Format("{0} STATE GOVERNMENT",
                                        Program.StateName.ToUpper());
                                    payment.DataSource = listresult;

                                    payment.CreateDocument();
                                }
                            }
                            if (checkEdit2.Checked)//Reprint
                            {
                                if (response.Tables[4] != null && response.Tables[4].Rows.Count > 0)//Reprint Manifest
                                {
                                    var listresult = (from DataRow row in response.Tables[4].Rows
                                                      select new DataSet.Main
                                                      {
                                                          PaymentRefNumber = row["PaymentRefNumber"] as string,
                                                          Receipt = row["EReceipts"] as string,
                                                          ApprovalDate = Convert.ToDateTime(row["ApprovalDate"]),
                                                          OldValue = row["OldRecord"] as string,
                                                          ApprovalBy = row["ApprovalBy"] as string,
                                                          NewValue = row["NewRecord"] as string,
                                                          Description = row["Description"] as string,
                                                          Type = row["Type"] as string,
                                                          Amount = Convert.ToDecimal(row["Amount"])

                                                      }).ToList();
                                    //XtraRepPayment payment = new XtraRepPayment();

                                    Reprint.xrLabel10.Text = "Reprinted Receipts";
                                    Reprint.xrLabel24.Text = Program.UserID;
                                    Reprint.xrLabel2.Text = string.Format("{0} STATE GOVERNMENT",
                                        Program.StateName.ToUpper());
                                    Reprint.DataSource = listresult;

                                    Reprint.CreateDocument();
                                }
                            }
                            if (checkEdit3.Checked)//Reseveral
                            {
                                if (response.Tables[5] != null && response.Tables[5].Rows.Count > 0)//Reversal receipt Manifest
                                {

                                    var listresult = (from DataRow row in response.Tables[5].Rows
                                                      select new DataSet.Reversal
                                                      {
                                                          PaymentRefNumber = row["PaymentRefNumber"] as string,
                                                          Amount = Convert.ToDecimal(row["Amount"]),
                                                          Receipt = row["EReceipts"] as string,
                                                          Type = row["Type"] as string,
                                                          Description = row["Description"] as string,
                                                          PayerName = row["PayerName"] as string,
                                                          PaymentDate = Convert.ToDateTime(row["PaymentDate"]),
                                                          AgencyName = row["AgencyName"] as string,
                                                          User = row["User"] as string,
                                                          Bankname = row["Bankname"] as string,
                                                          DepositSlipNumber = row["DepositSlipNumber"] as string
                                                      }
                                    );

                                    Reversal.xrLabel10.Text = "Reversal";
                                    Reversal.xrLabel24.Text = Program.UserID;
                                    Reversal.xrLabel9.Text = string.Format("{0} STATE GOVERNMENT", Program.StateName.ToUpper());
                                    Reversal.DataSource = listresult;

                                    Reversal.CreateDocument();
                                }
                            }

                            if (response.Tables[2] != null && response.Tables[2].Rows.Count > 0)
                            {
                                repManifest.Pages.AddRange(Unreceipted.Pages);

                            }

                            if (checkEdit1.Checked)///payment
                            {
                                if (response.Tables[3] != null && response.Tables[3].Rows.Count > 0)
                                {
                                    repManifest.Pages.AddRange(payment.Pages);
                                }
                            }
                            if (checkEdit2.Checked)//Reprint
                            {
                                if (response.Tables[4] != null && response.Tables[4].Rows.Count > 0)
                                {
                                    repManifest.Pages.AddRange(Reprint.Pages);
                                }
                            }
                            if (checkEdit3.Checked)//Reseveral
                            {
                                if (response.Tables[5] != null && response.Tables[5].Rows.Count > 0)//Unreceipted manifest
                                {
                                    repManifest.Pages.AddRange(Reversal.Pages);
                                }
                            }


                            // Reset all page numbers in the resulting document.
                            repManifest.PrintingSystem.ContinuousPageNumbering = true;

                            // Show the Print Preview form.
                            repManifest.ShowPreviewDialog();
                        }
                        else
                        {
                            Common.setMessageBox("No Record Found for selected Period", "TCO Report", 2); return;
                        }



                    }
                    else
                    {
                        Tripous.Sys.ErrorBox(response.Tables[0].Rows[0]["returnMessage"].ToString());
                        return;
                    }

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

    }
}
