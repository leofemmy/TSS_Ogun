using Collection.Classess;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using TaxSmartSuite.Class;

namespace Collection.Forms
{
    public partial class FrmDownloadSources : Form
    {
        public static int tableType = 0;

        public static string CheuqeOption;

        string strFormat, strEncoding;

        DataTable Dt = new DataTable();

        Timer time = new Timer();

        public FrmDownloadSources()
        {
            InitializeComponent();
            setImages();
            setDBComboBox();
            LoadGrid();
            bttnClose.Click += Bttn_Click;
            bttnBrowse.Click += Bttn_Click;
            time.Tick += time_Tick;
            time.Interval = 1000;
            time.Start();
        }

        void time_Tick(object sender, EventArgs e)
        {
            LoadGrid();
        }

        private void setImages()
        {
            bttnClose.Image = MDIMain.publicMDIParent.i16x16.Images[11];
            bttnBrowse.Image = MDIMain.publicMDIParent.i32x32.Images[7];
        }

        void setDBComboBox()
        {

            DataTable Dt;
            using (var ds = new System.Data.DataSet())
            {
                string query = "SELECT code,description FROM tblDownloadSources";
                using (SqlDataAdapter ada = new SqlDataAdapter(query, Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }

                Dt = ds.Tables[0];
            }

            Common.setComboList(cboSources, Dt, "code", "description");

            cboSources.SelectedIndex = -1;

        }

        void Bttn_Click(object sender, EventArgs e)
        {
            if (sender == bttnBrowse)
            {
                Select_click();
            }
            else if (sender == bttnClose)
            {
                MDIMain.publicMDIParent.RemoveControls();
            }
        }

        void Select_click()
        {
            if (cboSources.Text == null || cboSources.Text == "")
            {
                Common.setEmptyField("Download Sources", Program.ApplicationName);
                cboSources.Focus(); return;
            }
            else
            {
                if (cboSources.SelectedValue.ToString() == "0001")//paydirect code
                {
                    FrmPayDirect.CheuqeOption = "0001";
                    using (FrmPayDirect frmPayDirect = new FrmPayDirect())
                    {
                        frmPayDirect.ShowDialog();
                    }
                }
                else if (cboSources.SelectedValue.ToString() == "0002")//reemsonline
                {
                    using (FrmDownload frmDownload = new FrmDownload())
                    {
                        frmDownload.ShowDialog();
                    }
                }
                else if (cboSources.SelectedValue.ToString() == "0003")//update cheque state
                {
                    FrmPayDirect.CheuqeOption = "0003";
                    using (FrmPayDirect frmPayDirect = new FrmPayDirect())
                    {
                        frmPayDirect.ShowDialog();
                    }
                }
            }
        }

        void LoadGrid()
        {
            using (var ds = new System.Data.DataSet())
            {
                string query = "SELECT CONVERT(VARCHAR,CONVERT(DATEtime,[PaymentDate]),103) AS PaymentDate,[COUNT],ChequeStatus,PaymentMethod FROM ViewUnclearedCheques";
                using (SqlDataAdapter ada = new SqlDataAdapter(query, Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }
                gridControl1.DataSource = ds.Tables[0];
                gridControl1.ForceInitialize();
                gridView1.BestFitColumns();
            }
        }

    }
}
