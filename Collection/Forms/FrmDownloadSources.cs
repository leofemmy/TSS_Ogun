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

        public FrmDownloadSources()
        {
            InitializeComponent();
            setImages();
            setDBComboBox();

            bttnClose.Click += Bttn_Click;
            bttnBrowse.Click += Bttn_Click;
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
                    MDIMain.publicMDIParent.RemoveControls();
                    MDIMain.publicMDIParent.tableLayoutPanel2.Controls.Add((new FrmPayDirect().panelContainer), 1, 0);
                    FrmPayDirect.CheuqeOption = "0001";
                }
                else if (cboSources.SelectedValue.ToString() == "0002")//reemsonline
                {
                    MDIMain.publicMDIParent.RemoveControls();
                    MDIMain.publicMDIParent.tableLayoutPanel2.Controls.Add((new FrmDownload().panelContainer), 1, 0);
                }
                else if (cboSources.SelectedValue.ToString() == "0003")//update cheque state
                {
                    MDIMain.publicMDIParent.RemoveControls();
                    MDIMain.publicMDIParent.tableLayoutPanel2.Controls.Add((new FrmPayDirect().panelContainer), 1, 0);
                    FrmPayDirect.CheuqeOption = "0003";
                }
            }
        }

    }
}
