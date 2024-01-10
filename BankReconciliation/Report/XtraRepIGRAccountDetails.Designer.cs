namespace BankReconciliation.Report
{
    partial class XtraRepIGRAccountDetails
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.xrPivotGrid1 = new DevExpress.XtraReports.UI.XRPivotGrid();
            this.viewIGRAccountDetailsTableAdapter = new BankReconciliation.Dataset.dsTransactionTableAdapters.ViewIGRAccountDetailsTableAdapter();
            this.fieldAmount = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.fieldBankName = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.fieldTransDescription = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.fieldPeriod = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.dsTransaction1 = new BankReconciliation.Dataset.dsTransaction();
            this.viewTransactionPostCollectionBankTableAdapter = new BankReconciliation.Dataset.dsTransactionTableAdapters.ViewTransactionPostCollectionBankTableAdapter();
            this.dsTransaction2 = new BankReconciliation.Dataset.dsTransaction();
            this.formattingRule1 = new DevExpress.XtraReports.UI.FormattingRule();
            ((System.ComponentModel.ISupportInitialize)(this.dsTransaction1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsTransaction2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrPivotGrid1});
            this.Detail.HeightF = 69.79166F;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrPivotGrid1
            // 
            this.xrPivotGrid1.Appearance.FieldHeader.Font = new DevExpress.Drawing.DXFont("Times New Roman", 7F, DevExpress.Drawing.DXFontStyle.Regular, DevExpress.Drawing.DXGraphicsUnit.Point, new DevExpress.Drawing.DXFontAdditionalProperty[] {new DevExpress.Drawing.DXFontAdditionalProperty("GdiCharSet", ((byte)(0)))});
            this.xrPivotGrid1.DataAdapter = this.viewIGRAccountDetailsTableAdapter;
            this.xrPivotGrid1.DataMember = "ViewIGRAccountDetails";
            this.xrPivotGrid1.Fields.AddRange(new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField[] {
            this.fieldAmount,
            this.fieldBankName,
            this.fieldTransDescription,
            this.fieldPeriod});
            this.xrPivotGrid1.LocationFloat = new DevExpress.Utils.PointFloat(10.00001F, 0F);
            this.xrPivotGrid1.Name = "xrPivotGrid1";
            this.xrPivotGrid1.OLAPConnectionString = "";
            this.xrPivotGrid1.OptionsChartDataSource.UpdateDelay = 300;
            this.xrPivotGrid1.OptionsView.ShowColumnHeaders = false;
            this.xrPivotGrid1.OptionsView.ShowDataHeaders = false;
            this.xrPivotGrid1.OptionsView.ShowRowHeaders = false;
            this.xrPivotGrid1.SizeF = new System.Drawing.SizeF(744.3751F, 51.04168F);
            this.xrPivotGrid1.BeforePrint += new DevExpress.XtraReports.UI.BeforePrintEventHandler(this.xrPivotGrid1_BeforePrint);
            // 
            // viewIGRAccountDetailsTableAdapter
            // 
            this.viewIGRAccountDetailsTableAdapter.ClearBeforeFill = true;
            // 
            // fieldAmount
            // 
            this.fieldAmount.Area = DevExpress.XtraPivotGrid.PivotArea.DataArea;
            this.fieldAmount.AreaIndex = 0;
            this.fieldAmount.CellFormat.FormatString = "{0:n2}";
            this.fieldAmount.CellFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.fieldAmount.FieldName = "Amount";
            this.fieldAmount.GrandTotalCellFormat.FormatString = "{0:n2}";
            this.fieldAmount.GrandTotalCellFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.fieldAmount.Name = "fieldAmount";
            this.fieldAmount.TotalCellFormat.FormatString = "{0:n2}";
            this.fieldAmount.TotalCellFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.fieldAmount.TotalValueFormat.FormatString = "{0:n2}";
            this.fieldAmount.TotalValueFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.fieldAmount.ValueFormat.FormatString = "{0:n2}";
            this.fieldAmount.ValueFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.fieldAmount.Width = 50;
            // 
            // fieldBankName
            // 
            this.fieldBankName.Area = DevExpress.XtraPivotGrid.PivotArea.RowArea;
            this.fieldBankName.AreaIndex = 0;
            this.fieldBankName.FieldName = "BankName";
            this.fieldBankName.Name = "fieldBankName";
            this.fieldBankName.Options.AllowRunTimeSummaryChange = true;
            this.fieldBankName.SortOrder = DevExpress.XtraPivotGrid.PivotSortOrder.Descending;
            // 
            // fieldTransDescription
            // 
            this.fieldTransDescription.Area = DevExpress.XtraPivotGrid.PivotArea.ColumnArea;
            this.fieldTransDescription.AreaIndex = 0;
            this.fieldTransDescription.FieldName = "TransDescription";
            this.fieldTransDescription.Name = "fieldTransDescription";
            this.fieldTransDescription.RowValueLineCount = 2;
            // 
            // fieldPeriod
            // 
            this.fieldPeriod.Area = DevExpress.XtraPivotGrid.PivotArea.ColumnArea;
            this.fieldPeriod.AreaIndex = 1;
            this.fieldPeriod.FieldName = "Period";
            this.fieldPeriod.Name = "fieldPeriod";
            this.fieldPeriod.Visible = false;
            // 
            // TopMargin
            // 
            this.TopMargin.Name = "TopMargin";
            this.TopMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.TopMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // BottomMargin
            // 
            this.BottomMargin.Name = "BottomMargin";
            this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // dsTransaction1
            // 
            this.dsTransaction1.DataSetName = "dsTransaction";
            this.dsTransaction1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // viewTransactionPostCollectionBankTableAdapter
            // 
            this.viewTransactionPostCollectionBankTableAdapter.ClearBeforeFill = true;
            // 
            // dsTransaction2
            // 
            this.dsTransaction2.DataSetName = "dsTransaction";
            this.dsTransaction2.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // formattingRule1
            // 
            this.formattingRule1.DataMember = "ViewIGRAccountDetails";
            this.formattingRule1.Name = "formattingRule1";
            // 
            // XtraRepIGRAccountDetails
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin});
            this.DataAdapter = this.viewIGRAccountDetailsTableAdapter;
            this.DataMember = "ViewIGRAccountDetails";
            this.DataSource = this.dsTransaction1;
            this.FormattingRuleSheet.AddRange(new DevExpress.XtraReports.UI.FormattingRule[] {
            this.formattingRule1});
            this.Landscape = true;
            this.Margins = new DevExpress.Drawing.DXMargins(15, 10, 100, 100);
            this.PageHeight = 850;
            this.PageWidth = 1100;
            this.Version = "11.2";
            this.ParametersRequestSubmit += new System.EventHandler<DevExpress.XtraReports.Parameters.ParametersRequestEventArgs>(this.XtraRepIGRAccountDetails_ParametersRequestSubmit);
            ((System.ComponentModel.ISupportInitialize)(this.dsTransaction1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsTransaction2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.DetailBand Detail;
        private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
        private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
        private Dataset.dsTransaction dsTransaction1;
        private Dataset.dsTransactionTableAdapters.ViewTransactionPostCollectionBankTableAdapter viewTransactionPostCollectionBankTableAdapter;
        private Dataset.dsTransactionTableAdapters.ViewIGRAccountDetailsTableAdapter viewIGRAccountDetailsTableAdapter;
        private DevExpress.XtraReports.UI.XRPivotGrid xrPivotGrid1;
        private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField fieldAmount;
        private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField fieldBankName;
        private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField fieldTransDescription;
        private Dataset.dsTransaction dsTransaction2;
        private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField fieldPeriod;
        private DevExpress.XtraReports.UI.FormattingRule formattingRule1;
    }
}
