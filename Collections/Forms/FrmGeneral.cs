using Collection.Classess;
using Collection.Report;
using DevExpress.XtraReports.UI;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;
using TaxSmartSuite.Class;

namespace Collection.Forms
{
    public partial class FrmGeneral : Form
    {
        public static FrmGeneral publicInstance;

        protected TransactionTypeCode iTransType;

        private SqlCommand _command;

        private System.Data.DataSet ds;

        SqlDataAdapter adp;

        public FrmGeneral()
        {
            InitializeComponent();

            publicInstance = this;

            Load += OnFormLoad;

            setImages();

            ToolStripEvent();

            iTransType = TransactionTypeCode.New;

            OnFormLoad(null, null);

        }

        void radioGroup3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (radioGroup3.EditValue.ToString() == "0")
            {
                label6.Visible = false; cboStation.Visible = false;
            }
            if (radioGroup3.EditValue.ToString() == "1")
            { label6.Visible = true; cboStation.Visible = true; }


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

            setDBComboBox();

            radioGroup3.SelectedIndexChanged += radioGroup3_SelectedIndexChanged;

            bttnUpdate.Click += bttnUpdate_Click;
        }

        void bttnUpdate_Click(object sender, EventArgs e)
        {
            if (radioGroup3.EditValue == null)
            {
                Common.setMessageBox("Selection Type not Selected....!", Program.ApplicationName, 2);
                return;
            }
            else
            {
                string qury = string.Empty;
                System.Data.DataSet response = new System.Data.DataSet();

                XRepGlobalManifest Global = new XRepGlobalManifest();
                XRepGlobalManifest Global2 = new XRepGlobalManifest();
                XtraRepPayment payment = new XtraRepPayment();
                XtraRepPayment Reprint = new XtraRepPayment();
                //XtraRepPayment Reversal = new XtraRepPayment();
                XtraRepReversal Reversal = new XtraRepReversal();

                if (radioGroup3.EditValue.ToString() == "0")
                {
                    if (cboStation.SelectedIndex == -1)
                    {
                        qury = "0000";
                    }
                    try
                    {
                        using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
                        {
                            connect.Open();
                            _command = new SqlCommand("doGlobalManifest", connect)
                            {
                                CommandType = CommandType.StoredProcedure
                            };
                            _command.Parameters.Add(new SqlParameter("@start", SqlDbType.DateTime)).Value = string.Format("{0:yyyy-MM-dd 00:00:00}", DTPDateselect.Value.Date);
                            _command.Parameters.Add(new SqlParameter("@end", SqlDbType.DateTime)).Value = string.Format("{0:yyyy-MM-dd 23:59:59}", DTPDateselect.Value.Date);
                            _command.Parameters.Add(new SqlParameter("@Stationcode", SqlDbType.VarChar)).Value =
                              qury;
                            _command.Parameters.Add(new SqlParameter("@Type", SqlDbType.Int)).Value =
                              radioGroup3.EditValue;
                            _command.CommandTimeout = 0;
                            adp = new SqlDataAdapter(_command);
                            adp.Fill(response, "CollectionReportTable");

                            connect.Close();

                            if (response.Tables[0].Rows[0]["returnCode"].ToString() == "00")
                            {
                                if (response.Tables[1] != null && response.Tables[1].Rows.Count > 0)//Main Manifesthfhhf
                                {
                                    Global.DataSource = response.Tables[1];
                                    Global.DataAdapter = adp;
                                    Global.DataMember = "CollectionReportTable";
                                    Global.xrLabel12.Text = "GLOBAL RECEIPT MANIFEST";
                                    Global.xrLabel9.Text = string.Format("{0} State Government ", Program.StateName.ToUpper());
                                    Global.logoPath = Logic.logopth;
                                    Global.CreateDocument();

                                    if (response.Tables[2] != null && response.Tables[2].Rows.Count > 0)//unreceipted receipt Manifest
                                    {
                                        Global2.DataSource = response.Tables[2];
                                        Global2.DataAdapter = adp;
                                        Global2.DataMember = "CollectionReportTable";
                                        Global2.xrLabel12.Text = "GLOBAL UNRECEIPTED MANIFEST";
                                        Global2.xrLabel9.Text = string.Format("{0} State Government ", Program.StateName.ToUpper());
                                        Global2.CreateDocument();
                                    }

                                    if (response.Tables[3] != null && response.Tables[3].Rows.Count > 0)//Reprinted Receipts
                                    {
                                        var listresult = (from DataRow row in response.Tables[3].Rows
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
                                        Reprint.xrLabel2.Text = string.Format("{0} STATE GOVERNMENT",
                                            Program.StateName.ToUpper());
                                        Reprint.DataSource = listresult;

                                        Reprint.CreateDocument();
                                    }

                                    if (response.Tables[4] != null && response.Tables[4].Rows.Count > 0)//Amendement Receipt
                                    {
                                        var listresult = (from DataRow row in response.Tables[4].Rows
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
                                        payment.xrLabel10.Text = "Amended Receipts";
                                        payment.xrLabel2.Text = string.Format("{0} STATE GOVERNMENT",
                                            Program.StateName.ToUpper());
                                        payment.DataSource = listresult;

                                        payment.CreateDocument();


                                    }

                                    if (response.Tables[5] != null && response.Tables[5].Rows.Count > 0)//Reversal receipt
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

                                        Reversal.xrLabel10.Text = "Reversal"; Reversal.xrLabel24.Text = Program.UserID;
                                        Reversal.xrLabel9.Text = string.Format("{0} STATE GOVERNMENT",
                                            Program.StateName.ToUpper());
                                        Reversal.DataSource = listresult;

                                        Reversal.CreateDocument();
                                    }

                                    if (response.Tables[5] != null && response.Tables[5].Rows.Count > 0)//Unreceipted manifest
                                    {
                                        Global.Pages.AddRange(Reversal.Pages);
                                    }
                                    if (response.Tables[4] != null && response.Tables[4].Rows.Count > 0)
                                    {
                                        Global.Pages.AddRange(payment.Pages);
                                    }
                                    if (response.Tables[3] != null && response.Tables[3].Rows.Count > 0)
                                    {
                                        Global.Pages.AddRange(Reprint.Pages);
                                    }
                                    if (response.Tables[2] != null && response.Tables[2].Rows.Count > 0)
                                    {
                                        Global.Pages.AddRange(Global2.Pages);
                                    }

                                    // Reset all page numbers in the resulting document.
                                    Global.PrintingSystem.ContinuousPageNumbering = true;

                                    // Show the Print Preview form.
                                    Global.ShowPreviewDialog();
                                }
                                else
                                {
                                    Common.setMessageBox("No Record Found for selected Period", "TCO Report", 2); return;
                                }


                            }
                            else
                            {
                                Common.setMessageBox("No Record Found for selected Period", "TCO Report", 2); return;
                            }



                        }
                    }
                    catch (Exception ex)
                    {
                        Tripous.Sys.ErrorBox(ex.Message); return;
                    }
                }
                else
                {
                    if (cboStation.SelectedIndex < -1)
                    {
                        Common.setEmptyField("Station Name", Program.ApplicationName);
                        return;
                    }
                    else
                    {
                        try
                        {
                            using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
                            {
                                connect.Open();
                                _command = new SqlCommand("doGlobalManifest", connect)
                                {
                                    CommandType = CommandType.StoredProcedure
                                };
                                _command.Parameters.Add(new SqlParameter("@start", SqlDbType.DateTime)).Value =
                                    string.Format("{0:yyyy-MM-dd 00:00:00}", DTPDateselect.Value.Date);
                                _command.Parameters.Add(new SqlParameter("@end", SqlDbType.DateTime)).Value = string.Format("{0:yyyy-MM-dd 23:59:59}", DTPDateselect.Value.Date);
                                _command.Parameters.Add(new SqlParameter("@Stationcode", SqlDbType.VarChar)).Value =
                                   cboStation.SelectedValue.ToString();
                                _command.Parameters.Add(new SqlParameter("@Type", SqlDbType.Int)).Value =
                              radioGroup3.EditValue;
                                _command.CommandTimeout = 0;
                                adp = new SqlDataAdapter(_command);
                                adp.Fill(response, "CollectionReportTable");

                                connect.Close();
                                if (response.Tables[0].Rows[0]["returnCode"].ToString() == "00")
                                {
                                    if (response.Tables[1] != null && response.Tables[1].Rows.Count > 0)//Main Manifest
                                    {
                                        //Report.XRepGlobalManifest Global = new Collection.Report.XRepGlobalManifest
                                        //{
                                        Global.DataSource = response.Tables[1];
                                        Global.DataAdapter = adp;
                                        Global.DataMember = "CollectionReportTable";
                                        Global.xrLabel12.Text = "GLOBAL RECEIPT MANIFEST";
                                        //};
                                        Global.xrLabel9.Text = string.Format("{0} State Government ", Program.StateName.ToUpper());
                                        Global.logoPath = Logic.logopth;
                                        Global.ShowPreviewDialog();
                                    }

                                    if (response.Tables[2] != null && response.Tables[2].Rows.Count > 0)//unreceipted receipt Manifest
                                    {
                                        Global2.DataSource = response.Tables[2];
                                        Global2.DataAdapter = adp;
                                        Global2.DataMember = "CollectionReportTable";
                                        Global2.xrLabel12.Text = "GLOBAL UNRECEIPTED MANIFEST";
                                        Global2.xrLabel9.Text = string.Format("{0} State Government ", Program.StateName.ToUpper());
                                        Global2.CreateDocument();
                                    }

                                    if (response.Tables[3] != null && response.Tables[3].Rows.Count > 0)//Reprinted Receipts
                                    {
                                        var listresult = (from DataRow row in response.Tables[3].Rows
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
                                        Reprint.xrLabel2.Text = string.Format("{0} STATE GOVERNMENT",
                                            Program.StateName.ToUpper());
                                        Reprint.DataSource = listresult;

                                        Reprint.CreateDocument();
                                    }

                                    if (response.Tables[4] != null && response.Tables[4].Rows.Count > 0)//Amendement Receipt
                                    {
                                        var listresult = (from DataRow row in response.Tables[4].Rows
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
                                        payment.xrLabel10.Text = "Amended Receipts";
                                        payment.xrLabel2.Text = string.Format("{0} STATE GOVERNMENT",
                                            Program.StateName.ToUpper());
                                        payment.DataSource = listresult;

                                        payment.CreateDocument();


                                    }

                                    if (response.Tables[5] != null && response.Tables[5].Rows.Count > 0)//Reversal receipt
                                    {
                                        //var listresult = (from DataRow row in response.Tables[4].Rows
                                        //                  select new DataSet.Main
                                        //                  {
                                        //                      PaymentRefNumber = row["PaymentRefNumber"] as string,
                                        //                      Receipt = row["EReceipts"] as string,
                                        //                      ApprovalDate = Convert.ToDateTime(row["ApprovalDate"]),
                                        //                      OldValue = row["OldRecord"] as string,
                                        //                      ApprovalBy = row["ApprovalBy"] as string,
                                        //                      NewValue = row["NewRecord"] as string,
                                        //                      Description = row["Description"] as string,
                                        //                      Type = row["Type"] as string,
                                        //                      Amount = Convert.ToDecimal(row["Amount"])


                                        //                  }).ToList();
                                        //XtraRepPayment payment = new XtraRepPayment();

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


                                        Reversal.xrLabel10.Text = "Reversal"; Reversal.xrLabel24.Text = Program.UserID;
                                        Reversal.xrLabel9.Text = string.Format("{0} STATE GOVERNMENT",
                                            Program.StateName.ToUpper());
                                        Reversal.DataSource = listresult;

                                        Reversal.CreateDocument();
                                    }
                                }

                                if (response.Tables[5] != null && response.Tables[5].Rows.Count > 0)//Unreceipted manifest
                                {
                                    Global.Pages.AddRange(Reversal.Pages);
                                }
                                if (response.Tables[4] != null && response.Tables[4].Rows.Count > 0)
                                {
                                    Global.Pages.AddRange(payment.Pages);
                                }
                                if (response.Tables[3] != null && response.Tables[3].Rows.Count > 0)
                                {
                                    Global.Pages.AddRange(Reprint.Pages);
                                }
                                if (response.Tables[2] != null && response.Tables[2].Rows.Count > 0)
                                {
                                    Global.Pages.AddRange(Global2.Pages);
                                }

                                // Reset all page numbers in the resulting document.
                                Global.PrintingSystem.ContinuousPageNumbering = true;
                                Global.logoPath = Logic.logopth;
                                // Show the Print Preview form.
                                Global.ShowPreviewDialog();

                            }
                        }
                        catch (Exception ex)
                        {
                            Tripous.Sys.ErrorBox(ex.Message); return;
                        }
                    }
                    //}
                }
                #region
                //if (radioGroup3.EditValue.ToString() == "0")
                //{
                //    qury = string.Format("SELECT  [ID] ,[Provider] , [Channel] ,Collection.tblCollectionReport.[PaymentRefNumber] ,[DepositSlipNumber] , CONVERT(VARCHAR,CONVERT(DATE,[PaymentDate]),103) AS PaymentDate,[PayerID] , UPPER([PayerName]) AS [PayerName],[Amount] ,[PaymentMethod] , [ChequeNumber] ,[ChequeValueDate] ,[ChequeStatus] , [DateChequeReturned] ,[TelephoneNumber] , [ReceiptNo] , [ReceiptDate] , [PayerAddress] ,  [User] ,[RevenueCode] ,[Description] , [ChequeBankCode] , [ChequeBankName] , [AgencyName] ,  [AgencyCode] , [BankCode] , [BankName] , [BranchCode] ,[BranchName] , [ZoneCode] , [ZoneName] , [Username] ,  [AmountWords] , Collection.tblCollectionReport.[EReceipts] ,Collection.tblCollectionReport.[EReceiptsDate] ,[GeneratedBy] ,Collection.tblCollectionReport.[StationCode] ,(SELECT TOP 1 StationName from Receipt.tblStation WHERE tblStation.StationCode = Collection.tblCollectionReport.StationCode) AS StationName FROM Collection.tblCollectionReport WHERE Collection.tblCollectionReport.EReceipts IS NOT NULL AND (Collection.tblCollectionReport.[EReceiptsDate] BETWEEN '{0} 00:00:00' AND '{0} 23:59:59')  ORDER BY Collection.tblCollectionReport.AgencyCode,Collection.tblCollectionReport.StationCode  ,Collection.tblCollectionReport.RevenueCode,Collection.tblCollectionReport.EReceipts", DTPDateselect.Value.Date.ToString("MM/dd/yyyy"));
                //}

                //if (radioGroup3.EditValue.ToString() == "1")
                //{
                //    if (cboStation.SelectedIndex < -1)
                //    {
                //        Common.setEmptyField("Station Name", Program.ApplicationName);
                //        return;
                //    }
                //    else
                //    {
                //        qury = string.Format(" SELECT  [ID] ,[Provider] , [Channel] ,Collection.tblCollectionReport.[PaymentRefNumber] ,[DepositSlipNumber] , CONVERT(VARCHAR,CONVERT(DATE,[PaymentDate]),103) AS PaymentDate,[PayerID] , UPPER([PayerName]) AS [PayerName],[Amount] ,[PaymentMethod] , [ChequeNumber] ,[ChequeValueDate] ,[ChequeStatus] , [DateChequeReturned] ,[TelephoneNumber] , [ReceiptNo] , [ReceiptDate] , [PayerAddress] ,  [User] ,[RevenueCode] ,[Description] , [ChequeBankCode] , [ChequeBankName] , [AgencyName] ,  [AgencyCode] , [BankCode] , [BankName] , [BranchCode] ,[BranchName] , [ZoneCode] , [ZoneName] , [Username] ,  [AmountWords] , Collection.tblCollectionReport.[EReceipts] ,Collection.tblCollectionReport.[EReceiptsDate] ,[GeneratedBy] ,Collection.tblCollectionReport.[StationCode] ,(SELECT TOP 1 StationName from Receipt.tblStation WHERE tblStation.StationCode = Collection.tblCollectionReport.StationCode) AS StationName FROM Collection.tblCollectionReport WHERE Collection.tblCollectionReport.EReceipts IS NOT NULL AND (Collection.tblCollectionReport.[EReceiptsDate] BETWEEN '{0} 00:00:00' AND '{0} 23:59:59') AND Collection.tblCollectionReport.StationCode='{1}' ORDER BY Collection.tblCollectionReport.AgencyCode,Collection.tblCollectionReport.StationCode  ,Collection.tblCollectionReport.RevenueCode,Collection.tblCollectionReport.EReceipts", DTPDateselect.Value.Date.ToString("MM/dd/yyyy"), cboStation.SelectedValue);
                //    }

                //}

                #endregion

                #region olds2

                //if (response.Tables[0].Rows[0]["returnCode"].ToString() == "00")
                //{

                //    if (response.Tables[1] != null && response.Tables[1].Rows.Count > 0)//Main Manifest
                //    {
                //        using (System.Data.DataSet dds = new System.Data.DataSet("DsCollectionReport"))
                //        {
                //            SqlDataAdapter ada = new SqlDataAdapter();
                //            //using (SqlDataAdapter ada = new SqlDataAdapter(qury, Logic.ConnectionString))
                //            //{
                //            ada.Fill(dds, "CollectionReportTable");
                //            //Report.XRepGlobalManifest Global = new Collection.Report.XRepGlobalManifest { DataSource = dds, DataAdapter = ada, DataMember = "CollectionReportTable", RequestParameters = true };
                //            Report.XRepGlobalManifest Global = new Collection.Report.XRepGlobalManifest { DataSource = response.Tables[1], DataAdapter = ada, DataMember = "CollectionReportTable" };
                //            Global.xrLabel9.Text = string.Format("{0} State Government ", Program.StateName.ToUpper());
                //            //Global.paramStartDate.Value = "EReceiptsDate";
                //            //Global.paramEndDate.Value = "EReceiptsDate";
                //            // Global.paramEndDate.Visible = false;
                //            Global.ShowPreviewDialog();
                //            //}
                //        }
                //    }
                //}
                #endregion

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

        public void setDBComboBox()
        {
            DataTable Dt;

            using (var ds = new System.Data.DataSet())
            {
                using (SqlDataAdapter ada = new SqlDataAdapter((string)"SELECT StationCode,StationName FROM Receipt.tblStation ORDER BY StationCode", Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }

                Dt = ds.Tables[0];

            }

            Common.setComboList(cboStation, Dt, "StationCode", "StationName");

            cboStation.SelectedIndex = -1;


        }

    }
}
