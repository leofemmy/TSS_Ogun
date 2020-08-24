using Collection.Classess;
using Collections;
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
    public partial class FrmArmsDisplay : Form
    {
        System.Data.DataSet dstpass = new System.Data.DataSet(); bool IsShowDialog;

        DataTable temTable = new DataTable();

        private SqlDataAdapter adp;

        private SqlCommand _command;
        public FrmArmsDisplay()
        {
            InitializeComponent();
        }

        public FrmArmsDisplay(System.Data.DataSet dst, bool IsShowDialog)
        {
            SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

            InitializeComponent();

            dstpass.Clear();

            dstpass = dst;

            //temTable.Columns.Add("PaymentRefNumber", typeof(string));
            //temTable.Columns.Add("EReceipts", typeof(string));
            //temTable.Columns.Add("EReceiptsDate", typeof(DateTime));
            //temTable.Columns.Add("Amount", typeof(decimal));


            if (dstpass.Tables != null || dstpass.Tables[0].Rows.Count > 0)
            {
                gridControl1.DataSource = dstpass.Tables[1];
                gridView1.BestFitColumns();
            }

            if (!this.IsShowDialog)
                Init();

            SplashScreenManager.CloseForm(false);
        }

        void Init()
        {

            btnOk.Click += BtnOk_Click;
            //setImages();

            //ToolStripEvent();

            //Load += OnFormLoad;

            //bttnprints.Click += bttnprints_Click;

            //bttnMain.Click += bttnMain_Click;

            //OnFormLoad(null, null);



        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            System.Data.DataTable dts = dstpass.Tables[1];

            using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
            {
                connect.Open();
                _command = new SqlCommand("ArmsReceiptsMovedChecker", connect) { CommandType = CommandType.StoredProcedure };

                _command.Parameters.Add(new SqlParameter("@pTransaction", SqlDbType.Structured)).Value = dts;

                _command.CommandTimeout = 0;

                System.Data.DataSet response = new System.Data.DataSet();

                response.Clear();

                adp = new SqlDataAdapter(_command);

                adp.Fill(response);

                connect.Close();

                if (String.Compare(response.Tables[0].Rows[0]["returnCode"].ToString(), "00", false) == 0)
                {
                    Common.setMessageBox(response.Tables[0].Rows[0]["returnmessage"].ToString(), Program.ApplicationName, 1);

                }
                else
                {
                    Common.setMessageBox(response.Tables[0].Rows[0]["returnmessage"].ToString(), Program.ApplicationName, 1);
                    return;
                }

            }

            this.Close();
        }
    }
}
