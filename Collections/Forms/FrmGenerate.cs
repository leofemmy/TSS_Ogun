using Collection.Classess;
using Collection.Report;
using DevExpress.XtraReports.UI;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using TaxSmartSuite.Class;

namespace Collection.Forms
{
    public partial class FrmGenerate : Form
    {
        public static FrmGenerate publicStreetGroup;

        protected TransactionTypeCode iTransType;

        public static FrmGenerate publicInstance;

        protected bool boolIsUpdate;

        private SqlCommand _command; private SqlDataAdapter adp; private string BatchNumber;


        //string BatchNumber;

        string SQL, SQL1, SQL2, SQL3, SQL4;

        DateTime dt;

        public FrmGenerate()
        {
            InitializeComponent();

            InitializeComponent();

            publicInstance = this;

            publicStreetGroup = this;

            setImages();

            ToolStripEvent();

            Load += OnFormLoad;

            btnSelect.Click += btnSelect_Click;

            OnFormLoad(null, null);
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

            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = "dd/MM/yyyy";
        }

        void btnSelect_Click(object sender, EventArgs e)
        {

            try
            {

                using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
                {
                    connect.Open();

                    _command = new SqlCommand("PrintedReceiptLocal", connect)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    _command.Parameters.Add(new SqlParameter("@date", SqlDbType.VarChar)).Value = string.Format("{0:yyyy/MM/dd 00:00:00}", dateTimePicker1.Value);

                    using (System.Data.DataSet ds = new System.Data.DataSet())
                    {
                        ds.Clear();
                        adp = new SqlDataAdapter(_command);
                        adp.Fill(ds);
                        //Dts = ds.Tables[0];
                        connect.Close();

                        if (ds.Tables[0].Rows[0]["returnCode"].ToString() == "00")
                        {
                            if (ds.Tables[1].Rows.Count > 0)
                            {
                                XRepGenerate recportRec = new XRepGenerate { DataSource = ds.Tables[1], DataMember = "CollectionReportTable" };

                                recportRec.xrLabel4.Text = string.Format("RECEIPT GENERATED FOR {0} ON  {1}", Program.stationName.ToUpper(), dateTimePicker1.Value.Date.ToString("dd/MM/yyyy"));

                                recportRec.xrLabel9.Text = string.Format("{0} STATE GOVERNMENT", Program.StateName.ToUpper());

                                recportRec.ShowPreviewDialog();
                            }
                            else
                            {
                                Common.setMessageBox("No Record Found for this Transaction Date", Program.ApplicationName, 1);
                                return;
                            }
                        }
                        else
                        {
                            Common.setMessageBox(string.Format("{0}...Generate Local Receipt Printed Summary", ds.Tables[0].Rows[0]["returnMessage"]), Program.ApplicationName, 3);
                        }
                    }

                }
            }
            catch (Exception exception)
            {
                Tripous.Sys.ErrorBox(String.Format("{0}.....{1}", exception.Message, exception.StackTrace));
            }

        }


    }
}
