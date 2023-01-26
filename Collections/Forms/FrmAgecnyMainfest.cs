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
    public partial class FrmAgecnyMainfest : Form
    {
        public static FrmAgecnyMainfest publicStreetGroup;

        protected TransactionTypeCode iTransType;

        public static FrmAgecnyMainfest publicInstance;

        protected bool boolIsUpdate;

        string criteria, criteria2, criteria3;

        private SqlCommand _command; private SqlDataAdapter adp; private string BatchNumber;


        //string BatchNumber;

        string SQL, SQL1, SQL2, SQL3, SQL4;

        DateTime dt;
        public FrmAgecnyMainfest()
        {
            InitializeComponent();

            publicInstance = this;

            publicStreetGroup = this;

            setImages();

            ToolStripEvent();

            Load += OnFormLoad;

            cboAgencyTest.EditValueChanged += cboAgencyTest_EditValueChanged;

            btnSelect.Click += btnSelect_Click;

            OnFormLoad(null, null);
        }

        void btnSelect_Click(object sender, EventArgs e)
        {
            string values = string.Empty;

            string values1 = string.Empty;

            //criteria = String.Format("AND [EReceiptsDate] >='{0}' AND [EReceiptsDate]<= '{1}' ", string.Format("{0:MM/dd/yyyy 00:00:00}", dtpfrm.Value), string.Format("{0:MM/dd/yyyy 23:59:59}", dtpTo.Value));

            if (cboAgencyTest.EditValue == null || cboAgencyTest.EditValue.ToString() != "")
            {
                object[] lol = cboAgencyTest.EditValue.ToString().Split(',');

                int i = 0;

                foreach (object obj in lol)
                {
                    values += string.Format("{0}", obj.ToString().Trim());

                    if (i + 1 < lol.Count())
                        values += ",";
                    ++i;
                }

                //criteria += String.Format(" AND AgencyCode in ({0})", values);
            }

            if (cboRevenueEdt.EditValue == null || cboRevenueEdt.EditValue.ToString() != "")
            {
                object[] lol = cboRevenueEdt.EditValue.ToString().Split(',');

                int i = 0;

                foreach (object obj in lol)
                {
                    values1 += string.Format("{0}", obj.ToString().Trim());

                    if (i + 1 < lol.Count())
                        values1 += ",";
                    ++i;
                }

                //criteria += String.Format(" AND RevenueCode in ({0})", values1);
            }

            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

                using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
                {
                    connect.Open();

                    _command = new SqlCommand("doReprintMainfest", connect) { CommandType = CommandType.StoredProcedure };
                    _command.Parameters.Add(new SqlParameter("@AgencyCode", SqlDbType.VarChar)).Value = values.ToString();
                    _command.Parameters.Add(new SqlParameter("@StartDate", SqlDbType.VarChar)).Value = string.Format("{0:yyyy/MM/dd 00:00:00}", dtpfrm.Value);
                    _command.Parameters.Add(new SqlParameter("@enddate", SqlDbType.VarChar)).Value = string.Format("{0:yyyy/MM/dd 23:59:59}", dtpTo.Value);
                    _command.Parameters.Add(new SqlParameter("@RevenueCode", SqlDbType.VarChar)).Value = values1.ToString();

                    _command.CommandTimeout = 0;
                    using (System.Data.DataSet ds = new System.Data.DataSet())
                    {
                        ds.Clear();

                        adp = new SqlDataAdapter(_command);
                        adp.Fill(ds, "CollectionReportTable");
                        //Dts = ds.Tables[0];
                        connect.Close();

                        //dtResult = ds;

                        if (ds.Tables[0].Rows[0]["returnCode"].ToString() == "00")
                        {

                            if (ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0)//Unreceipted manifest
                            {
                                //dbCrDebit = ds.Tables[5];
                                //using (System.Data.DataSet dds = new System.Data.DataSet("DsCollectionReport"))
                                //{
                                //    using (SqlDataAdapter ada = new SqlDataAdapter(query, Logic.ConnectionString))
                                //    {
                                //ada.Fill(dds, "CollectionReportTable");
                                XRepManifest repManifest = new XRepManifest { DataSource = ds.Tables[1], DataAdapter = adp, DataMember = "CollectionReportTable", RequestParameters = false };
                                repManifest.xrLabel10.Text = Program.UserID;
                                //OGUN STATE GOVERNMENTre
                                repManifest.logoPath = Logic.logopth;
                                repManifest.xrLabel9.Text = string.Format("{0} STATE GOVERNMENT", Program.StateName.ToUpper());
                                repManifest.ShowPreviewDialog();
                                //    }
                                //}
                            }
                            else
                            {
                                Common.setMessageBox("No Record Found!!!!!!", "Agency Mainfest", 2);
                                return;

                            }

                        }
                        else
                        {


                            Tripous.Sys.ErrorBox(String.Format("{0}{1}", ds.Tables[0].Rows[0]["returnCode"].ToString(), ds.Tables[0].Rows[0]["returnMessage"].ToString()));

                            return;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Tripous.Sys.ErrorBox(String.Format("{0}{1}", ex.StackTrace, ex.Message));
                return;
            }
            finally
            {
                SplashScreenManager.CloseForm(false);
            }
        }

        void cboAgencyTest_EditValueChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(cboAgencyTest.EditValue.ToString()) || cboAgencyTest.EditValue != null)
            {
                setDBComboBoxReveneu();
            }
        }

        private void setImages()
        {
            tsbNew.Image = MDIMain.publicMDIParent.i16x16.Images[5];
            tsbEdit.Image = MDIMain.publicMDIParent.i16x16.Images[6];
            tsbDelete.Image = MDIMain.publicMDIParent.i16x16.Images[7];
            tsbReload.Image = MDIMain.publicMDIParent.i16x16.Images[0];
            tsbClose.Image = MDIMain.publicMDIParent.i16x16.Images[11];

            //bttnCancel.Image = MDIMain.publicMDIParent.i32x32.Images[9];
            //btnPrint.Image = MDIMain.publicMDIParent.i32x32.Images[29];
            btnSelect.Image = MDIMain.publicMDIParent.i32x32.Images[29];
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

        void OnFormLoad(object sender, EventArgs e)
        {
            //setDBComboBox();

            //dateTimePicker1.Format = DateTimePickerFormat.Custom;
            //dateTimePicker1.CustomFormat = "dd/MM/yyyy";

            DateTime result = DateTime.Today.Subtract(TimeSpan.FromDays(10));

            DateTime result2 = DateTime.Today.Subtract(TimeSpan.FromDays(1));

            dtpfrm.Value = result;

            dtpTo.Value = result2;

            setDBComboBoxAgency();
        }

        void setDBComboBoxReveneu()
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

                DataTable Dt;

                string values = string.Empty;

                object[] lol = cboAgencyTest.EditValue.ToString().Split(',');

                int i = 0;

                foreach (object obj in lol)
                {
                    values += string.Format("'{0}'", obj.ToString().Trim());

                    if (i + 1 < lol.Count())
                        values += ",";
                    ++i;
                }

                using (var ds = new System.Data.DataSet())
                {
                    string query = string.Format("SELECT RevenueCode,Description FROM Collection.tblRevenueType WHERE AgencyCode IN ({0})  ORDER BY Description", values);

                    using (SqlDataAdapter ada = new SqlDataAdapter(query, Logic.ConnectionString))
                    {
                        ada.SelectCommand.CommandTimeout = 0;

                        ada.Fill(ds, "table");
                    }

                    Dt = ds.Tables[0];
                }

                //Common.setComboList(cboRevenue, Dt, "RevenueCode", "Description");

                Common.setCheckEdit(cboRevenueEdt, Dt, "RevenueCode", "Description");

                //cboRevenue.SelectedIndex = -1;
            }
            finally
            {
                SplashScreenManager.CloseForm(false);
            }
        }

        public void setDBComboBoxAgency()
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

                DataTable Dt;

                using (var ds = new System.Data.DataSet())
                {
                    using (SqlDataAdapter ada = new SqlDataAdapter((string)"SELECT AgencyCode,UPPER(AgencyName) as AgencyName FROM Registration.tblAgency ORDER BY AgencyName", Logic.ConnectionString))
                    {
                        ada.SelectCommand.CommandTimeout = 0;

                        ada.Fill(ds, "table");
                    }

                    Dt = ds.Tables[0];
                }

                Common.setCheckEdit(cboAgencyTest, Dt, "AgencyCode", "AgencyName");


            }
            finally
            {
                SplashScreenManager.CloseForm(false);
            }

        }


    }
}
