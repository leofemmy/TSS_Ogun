using DevExpress.Utils;
using DevExpress.XtraSplashScreen;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BankReconciliation.Forms
{
    public partial class FrmReclassifiedResult : Form
    {
        System.Data.DataSet dts; 

          System.Data.DataSet dts2 = new System.Data.DataSet();
        public FrmReclassifiedResult()
        {
            InitializeComponent();
            Init();
        }

        public FrmReclassifiedResult(System.Data.DataSet ds)
        {
            InitializeComponent();
          
            dts2.Clear();
            dts2 = ds;
            Init();
        }

        void Init()
        {
            SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

            Load += OnFormLoad;

            OnFormLoad(null, null);

            SplashScreenManager.CloseForm(false);
        }

        void OnFormLoad(object sender, EventArgs e)
        {

            if (dts2.Tables[1] != null && dts2.Tables[1].Rows.Count > 0)
            {
                gridControl1.DataSource = dts2.Tables[1];
                gridView1.OptionsBehavior.Editable = false;
                gridView1.Columns["Amount"].DisplayFormat.FormatType = FormatType.Numeric;
                gridView1.Columns["Amount"].DisplayFormat.FormatString = "n2";
            }
        }

    }
}
