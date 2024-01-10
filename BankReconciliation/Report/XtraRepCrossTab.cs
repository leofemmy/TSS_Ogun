namespace BankReconciliation.Report
{
    public partial class XtraRepCrossTab : DevExpress.XtraReports.UI.XtraReport
    {
        public XtraRepCrossTab()
        {
            InitializeComponent();

        }

        private void XtraRepCrossTab_BeforePrint(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //     fieldCategoryID.FilterValues.Clear()
            //fieldCategoryID.FilterValues.FilterType = DevExpress.Data.PivotGrid.PivotFilterType.Included
            //fieldCategoryID.FilterValues.Add(Me.parameter1.Value)

            //fieldPeriod.FilterValues.Clear();
            // fieldPeriod.FilterValues.FilterType = DevExpress.XtraPivotGrid.PivotFilterType.Included;
            // fieldPeriod.FilterValues.Add(paramPeriod.Value);
        }

    }
}
