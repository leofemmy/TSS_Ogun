using Collection.Classess;
using Collection.Report;
using DevExpress.XtraReports.UI;
using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;
using TaxSmartSuite.Class;


namespace Collection.Forms
{
    public partial class FrmMainFest : Form
    {
        private bool bResponse;

        private DataTable Dts;

        string query;

        ArrayList myArrayList = new ArrayList();

        int arrycount, arrycount1;

        string[] split;
        //int test;
        int k, test, gbj;

        private SqlCommand command;

        private SqlDataAdapter adp;

        //private DataSet ds = new DataSet();

        System.Data.DataTable dt;

        System.Data.DataSet ds;

        string BatchNumber;

        private string user;

        public static FrmMainFest publicStreetGroup;

        protected TransactionTypeCode iTransType;

        protected bool boolIsUpdate;

        public static FrmMainFest publicInstance;


        public FrmMainFest()
        {
            InitializeComponent();

            publicInstance = this;

            publicStreetGroup = this;

            setImages();

            ToolStripEvent();

            iTransType = TransactionTypeCode.New;

            //Load += OnFormLoad;


            gbj = 0;
            SetReload();

            btnApply.Click += btnApply_Click;

            btnPrint.Click += btnPrint_Click;

            if (Program.UserID == "" || Program.UserID == null)
            {
                user = "Femi";
            }
            else
            {
                user = Program.UserID;
            }

            //generate number
            BatchNumber = String.Format("{0:d9}", (DateTime.Now.Ticks / 10) % 1000000000);


        }

        private void setImages()
        {
            tsbNew.Image = MDIMain.publicMDIParent.i16x16.Images[5];
            tsbEdit.Image = MDIMain.publicMDIParent.i16x16.Images[6];
            tsbDelete.Image = MDIMain.publicMDIParent.i16x16.Images[7];
            tsbReload.Image = MDIMain.publicMDIParent.i16x16.Images[0];
            tsbClose.Image = MDIMain.publicMDIParent.i16x16.Images[11];

            //bttnCancel.Image = MDIMain.publicMDIParent.i32x32.Images[9];
            btnApply.Image = MDIMain.publicMDIParent.i32x32.Images[8];
            btnPrint.Image = MDIMain.publicMDIParent.i32x32.Images[29];
            //btnSearch.Image = MDIMain.publicMDIParent.i32x32.Images[2];

        }

        void btnPrint_Click(object sender, EventArgs e)
        {
            XRepManifest repManifest = new XRepManifest();

            repManifest.ShowPreviewDialog();
        }

        void btnApply_Click(object sender, EventArgs e)
        {
            myArrayList.Clear();

            if (string.IsNullOrEmpty(txtStart.Text))
            {
                Common.setEmptyField("Please Enter Control Start Number", Program.ApplicationName);
                txtStart.Focus(); return;
            }
            else if (string.IsNullOrEmpty(txtEnd.Text))
            {
                Common.setEmptyField("Please Enter Control End Number", Program.ApplicationName);
                txtEnd.Focus(); return;
            }
            else
            {
                int range = Convert.ToInt32(txtEnd.Text) - Convert.ToInt32(txtStart.Text);
                int leng = txtStart.Text.Length;


                //generate control number base on enter range

                for (int j = 0; j <= range; j++)
                {
                    string contnum = Convert.ToString(Convert.ToInt32(txtStart.Text) + j);

                    contnum = contnum.PadLeft(leng, '0');

                    myArrayList.Add(contnum);

                }

                arrycount = myArrayList.Count;

                test = dt.Rows.Count;

                if (arrycount != test)
                {
                    Common.setMessageBox("Control Number Does not Till with Number of Records Printed", Program.ApplicationName, 1);

                    return;
                }

                if (!string.IsNullOrEmpty(txtExpec.Text))
                {
                    split = txtExpec.Text.ToString().Split(new Char[] { ',' });

                    //check and remove expection from the control list
                    for (int i = 0; i < split.Count(); i++)
                    {
                        //check with the list and remove

                        for (int j = 0; j < myArrayList.Count; j++)
                        {
                            if (split[i].ToString() == myArrayList[j].ToString())
                            {
                                myArrayList.Remove(split[i].ToString());
                            }
                        }
                    }
                }
                arrycount1 = myArrayList.Count;

                foreach (DataRow item in dt.Rows)
                {
                    string lol = item["PaymenRefNumber"].ToString();
                    //gbj = 0;
                    if (gbj == arrycount1)
                    {
                        break;
                    }
                    else
                    {
                        if (CheckRnageValue(myArrayList[gbj].ToString()))
                        {
                            Common.setMessageBox("Control Number Range already Exit", Program.ApplicationName, 2);
                            break;
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

                                    /////update bamkreceipts with control number
                                    string query = String.Format("UPDATE dbo.BankReceipts SET ControlNumber = '{0}',BatchNumber= '{1}',PrintedDate= '{2}',printedby= '{3}' WHERE PaymenRefNumber= '{4}'", myArrayList[gbj], BatchNumber, DateTime.Now.Date.ToString("yyyy-MM-dd hh:mm:ss"), user, lol);

                                    using (SqlCommand sqlCommand1 = new SqlCommand(query, db, transaction))
                                    {
                                        sqlCommand1.ExecuteNonQuery();
                                    }

                                    //update collection table when receipt is been printed
                                    string query1 = String.Format("UPDATE tblCollectionReport SET IsPrint = true,PrintedBY= '{0}',DatePrinted= '{1}',ControlNumber= '{2}',BatchNumber= '{3}' WHERE PaymenRefNumber= '{4}'", myArrayList[gbj], BatchNumber, DateTime.Now.Date.ToString("yyyy-MM-dd hh:mm:ss"), user, lol);

                                    using (SqlCommand sqlCommand = new SqlCommand(query, db, transaction))
                                    {
                                        sqlCommand.ExecuteNonQuery();
                                    }


                                }

                                catch (SqlException sqlError)
                                {
                                    transaction.Rollback();
                                }
                                transaction.Commit();

                                db.Close();
                            }
                        }



                    }
                    gbj = gbj + 1;

                }

                InsertReceiptHistory();

                SetReload();

                btnPrint.Enabled = true;

                btnApply.Enabled = false;


            }
        }

        void InsertReceiptHistory()
        {

            using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
            {
                connect.Open();
                command = new SqlCommand("InsertPrintReceiptHistory", connect) { CommandType = CommandType.StoredProcedure };

                using (System.Data.DataSet ds = new System.Data.DataSet())
                {
                    adp = new SqlDataAdapter(command);
                    adp.Fill(ds);
                    Dts = ds.Tables[0];
                    connect.Close();
                    bResponse = true;
                }
            }
        }

        void SetReload()
        {
            try
            {
                using (var ds = new System.Data.DataSet())
                {
                    query = "SELECT [PaymenRefNumber], [DepositSlipNumber], [PaymentDate], [PayerID], [PayerName], [Description], [Amount], [BankName], [BranchName], [ReceiptNumber], [Users], [ControlNumber],[BatchNumber],[printedby],[PrintedDate] FROM [BankReceipts] WHERE ([ControlNumber] IS NULL)";

                    using (SqlDataAdapter ada = new SqlDataAdapter(query, Logic.ConnectionString))
                    {
                        ada.Fill(ds, "table");
                    }



                    dt = ds.Tables[0];
                    //this.dataGridView1.DataSource = ds; //Logic.ConnectionString;                       
                    //dataGridView1.DataMember = "table";
                    gridControl1.DataSource = dt.DefaultView;
                    ////gridView1.Columns["BankReceiptsid"].Visible = false;
                    gridView1.BestFitColumns();
                    label5.Text = "Total Number of Records Printed: " + "   " + dt.Rows.Count;

                }


            }
            catch (Exception ex)
            {
                Common.setMessageBox(ex.Message, Program.ApplicationName, 3);
                return;
            }
        }

        static bool CheckRnageValue(string ContNum)
        {
            bool bRes = false;

            string sql = String.Format("SELECT COUNT(*) AS Count FROM dbo.tblPrintHistoryReceipt WHERE (ControlNumber = '{0}')", ContNum);

            if (new Logic().IsRecordExist(sql))

            //if (retval == "1")
            {
                bRes = true;
            }
            return bRes;
        }

        private void FrmMainFest_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'taxSmartSuiteRevisedDataSet.BankReceipts' table. You can move, or remove it, as needed.
            //this.bankReceiptsTableAdapter.Fill(this.taxSmartSuiteRevisedDataSet.BankReceipts);

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

    }
}
