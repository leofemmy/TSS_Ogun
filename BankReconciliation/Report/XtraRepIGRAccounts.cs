using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace BankReconciliation.Report
{
    public partial class XtraRepIGRAccounts : DevExpress.XtraReports.UI.XtraReport
    {
        double dbtotal,dbBank,dbcheque,dbcoll;

        public XtraRepIGRAccounts()
        {
            InitializeComponent();

            //xrLabel19.BeforePrint += xrLabel19_BeforePrint;
        }

        void xrLabel19_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            //dbBank = calBank.GetValue();
            //dbcheque = Convert.ToDouble(calCheque.GetType());
            //dbcoll = Convert.ToDouble(calCollection.GetType());
            //dbtotal = (dbBank + dbcheque + dbcoll);
            //xrLabel16.Text = string.Format("{0:n2}", dbtotal);
        }

    }
}
