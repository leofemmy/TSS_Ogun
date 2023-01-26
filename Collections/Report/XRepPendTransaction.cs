using System;

namespace Collection.Report
{
    public partial class XRepPendTransaction : DevExpress.XtraReports.UI.XtraReport
    {
        public XRepPendTransaction()
        {
            InitializeComponent();
        }

        private void XRepPendTransaction_ParametersRequestSubmit(object sender, DevExpress.XtraReports.Parameters.ParametersRequestEventArgs e)
        {
            string objDate = Convert.ToDateTime(paramDate.Value).Date.ToShortDateString() + " 00:00:00";
            DateTime dtDate = DateTime.Parse(objDate);
            paramStartDate.Value = (object)dtDate;

            objDate = Convert.ToDateTime(paramDate.Value).Date.ToShortDateString() + " 23:59:59";
            dtDate = DateTime.Parse(objDate);
            paramEndDate.Value = (object)dtDate;
        }

    }
}
