using Collection.Classess;
using DevExpress.XtraEditors;
using DevExpress.XtraReports.Parameters;
using DevExpress.XtraReports.UI;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace Collection.Report
{
    public partial class XtraRepDetails : DevExpress.XtraReports.UI.XtraReport
    {
        public XtraRepDetails()
        {
            InitializeComponent();

            xrLabel9.PreviewClick += xrLabel9_PreviewClick;

            xrLabel9.PreviewMouseMove += xrLabel9_PreviewMouseMove;

            xrLabel9.BeforePrint += xrLabel9_BeforePrint;


        }

        void xrLabel9_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            xrLabel9.ForeColor = Color.Navy;

            ((XRLabel)sender).Tag = GetCurrentRow();
        }

        void xrLabel9_PreviewMouseMove(object sender, PreviewMouseEventArgs e)
        {
            Cursor.Current = Cursors.Hand;
        }

        void xrLabel9_PreviewClick(object sender, PreviewMouseEventArgs e)
        {

            using (System.Data.DataSet dds = new System.Data.DataSet("DsCollectionReport"))
            {
                SqlDataAdapter ada;

                DataTable Dt = dds.Tables.Add("CollectionReportTable");
                ada = new SqlDataAdapter((string)"SELECT ID, Provider, Channel, PaymentRefNumber, DepositSlipNumber, PaymentDate, PayerID, PayerName, Amount, PaymentMethod, ChequeNumber, ChequeValueDate, ChequeStatus, DateChequeReturned, TelephoneNumber, ReceiptNo, ReceiptDate, PayerAddress, Status, [User], RevenueCode, Description, ChequeBankCode, ChequeBankName, AgencyName, AgencyCode, BankCode, BankName, BranchCode, BranchName, ZoneCode, ZoneName, Username, State, AmountWords, URL, EReceipts, EReceiptsDate, GeneratedBy, DateValidatedAgainst, DateDiff, UploadStatus, PrintedBY, DatePrinted, ControlNumber, BatchNumber, StationCode,(SELECT StationName FROM  tblStation WHERE(StationCode = dbo.tblCollectionReport.StationCode)) AS Stationname, isPrinted, CentreCode,CentreName, IsNormalize, NormalizeBy, NormalizeDate, IsPrintedDate, IsRecordExit,IsPayDirect FROM tblCollectionReport ORDER BY PaymentDate,Amount,BankCode , BranchCode ", Logic.ConnectionString);
                ada.Fill(dds, "CollectionReportTable");

                XtraRepDetailStation reportst = new XtraRepDetailStation() { DataSource = dds, DataMember = "CollectionReportTable" };
                reportst.paramStationCode.Value = ((DataRowView)e.Brick.Value).Row["StationCode"];

                reportst.paramDate.Value = this.paramDate.Value;

                reportst.xrLabel17.Text = string.Format("Date Receipts Created: {0:dd/MM/yyyy} ", this.paramDate.Value);

                reportst.ShowPreviewDialog();


            }
        }

        private void XtraRepDetails_ParametersRequestBeforeShow(object sender, DevExpress.XtraReports.Parameters.ParametersRequestEventArgs e)
        {
            foreach (ParameterInfo info in e.ParametersInformation)
            {
                if (info.Parameter.Name == paramDate.Name)
                {

                    var editor = info.Editor as DateEdit;
                    if (editor != null)
                    {

                        editor.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
                        editor.Properties.DisplayFormat.FormatString = "dd/MM/yyyy";
                    }

                }
            }
        }

        private void XtraRepDetails_ParametersRequestValueChanged(object sender, ParametersRequestValueChangedEventArgs e)
        {
            foreach (ParameterInfo info in e.ParametersInformation)
            {
                if (info.Parameter.Name == paramDate.Name)
                {

                    var editor = info.Editor as DateEdit;
                    if (editor != null)
                    {

                        editor.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
                        editor.Properties.DisplayFormat.FormatString = "dd/MM/yyyy";
                    }

                }
            }
        }

    }
}
