namespace BankReconciliation.Report
{
    partial class XtraRepCrossTab
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(XtraRepCrossTab));
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.xrPivotGrid1 = new DevExpress.XtraReports.UI.XRPivotGrid();
            this.viewIGRAccountDetailsTableAdapter = new BankReconciliation.Dataset.dsTransactionTableAdapters.ViewIGRAccountDetailsTableAdapter();
            this.dsTransaction1 = new BankReconciliation.Dataset.dsTransaction();
            this.fieldAmount = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.fieldBankName = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.fieldTransDescription = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.xrPictureBox2 = new DevExpress.XtraReports.UI.XRPictureBox();
            this.xrPictureBox1 = new DevExpress.XtraReports.UI.XRPictureBox();
            this.xrLabel2 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.viewTransactionPostCollectionBankTableAdapter = new BankReconciliation.Dataset.dsTransactionTableAdapters.ViewTransactionPostCollectionBankTableAdapter();
            this.paramPeriod = new DevExpress.XtraReports.Parameters.Parameter();
            ((System.ComponentModel.ISupportInitialize)(this.dsTransaction1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrPivotGrid1});
            this.Detail.HeightF = 70.83334F;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrPivotGrid1
            // 
            this.xrPivotGrid1.Appearance.Cell.Font = new System.Drawing.Font("Arial", 7F);
            this.xrPivotGrid1.Appearance.CustomTotalCell.Font = new System.Drawing.Font("Times New Roman", 8F);
            this.xrPivotGrid1.Appearance.FieldHeader.Font = new System.Drawing.Font("Times New Roman", 8F);
            this.xrPivotGrid1.Appearance.FieldValue.Font = new System.Drawing.Font("Times New Roman", 8F);
            this.xrPivotGrid1.DataAdapter = this.viewIGRAccountDetailsTableAdapter;
            this.xrPivotGrid1.DataMember = "ViewIGRAccountDetails";
            this.xrPivotGrid1.DataSource = this.dsTransaction1;
            this.xrPivotGrid1.Fields.AddRange(new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField[] {
            this.fieldAmount,
            this.fieldBankName,
            this.fieldTransDescription});
            this.xrPivotGrid1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrPivotGrid1.Name = "xrPivotGrid1";
            this.xrPivotGrid1.OptionsData.FilterByVisibleFieldsOnly = true;
            this.xrPivotGrid1.OptionsPrint.FilterSeparatorBarPadding = 3;
            this.xrPivotGrid1.OptionsView.ShowColumnGrandTotalHeader = false;
            this.xrPivotGrid1.OptionsView.ShowColumnGrandTotals = false;
            this.xrPivotGrid1.OptionsView.ShowColumnHeaders = false;
            this.xrPivotGrid1.OptionsView.ShowDataHeaders = false;
            this.xrPivotGrid1.SizeF = new System.Drawing.SizeF(550.4167F, 58.33334F);
            // 
            // viewIGRAccountDetailsTableAdapter
            // 
            this.viewIGRAccountDetailsTableAdapter.ClearBeforeFill = true;
            // 
            // dsTransaction1
            // 
            this.dsTransaction1.DataSetName = "dsTransaction";
            this.dsTransaction1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
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
            this.fieldAmount.Width = 80;
            // 
            // fieldBankName
            // 
            this.fieldBankName.Area = DevExpress.XtraPivotGrid.PivotArea.RowArea;
            this.fieldBankName.AreaIndex = 0;
            this.fieldBankName.FieldName = "BankName";
            this.fieldBankName.Name = "fieldBankName";
            this.fieldBankName.Width = 120;
            // 
            // fieldTransDescription
            // 
            this.fieldTransDescription.Area = DevExpress.XtraPivotGrid.PivotArea.ColumnArea;
            this.fieldTransDescription.AreaIndex = 0;
            this.fieldTransDescription.FieldName = "TransDescription";
            this.fieldTransDescription.Name = "fieldTransDescription";
            this.fieldTransDescription.Width = 110;
            // 
            // TopMargin
            // 
            this.TopMargin.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrPictureBox2,
            this.xrPictureBox1,
            this.xrLabel2,
            this.xrLabel1});
            this.TopMargin.HeightF = 121.875F;
            this.TopMargin.Name = "TopMargin";
            this.TopMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.TopMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrPictureBox2
            // 
            this.xrPictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("xrPictureBox2.Image")));
            this.xrPictureBox2.LocationFloat = new DevExpress.Utils.PointFloat(957.2917F, 24.29167F);
            this.xrPictureBox2.Name = "xrPictureBox2";
            this.xrPictureBox2.SizeF = new System.Drawing.SizeF(93.12506F, 54.54167F);
            this.xrPictureBox2.Sizing = DevExpress.XtraPrinting.ImageSizeMode.ZoomImage;
            // 
            // xrPictureBox1
            // 
            this.xrPictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("xrPictureBox1.Image")));
            this.xrPictureBox1.LocationFloat = new DevExpress.Utils.PointFloat(10.00001F, 24.29167F);
            this.xrPictureBox1.Name = "xrPictureBox1";
            this.xrPictureBox1.SizeF = new System.Drawing.SizeF(100F, 54.54167F);
            this.xrPictureBox1.Sizing = DevExpress.XtraPrinting.ImageSizeMode.ZoomImage;
            // 
            // xrLabel2
            // 
            this.xrLabel2.Font = new System.Drawing.Font("Trebuchet MS", 9F, System.Drawing.FontStyle.Bold);
            this.xrLabel2.LocationFloat = new DevExpress.Utils.PointFloat(47.91667F, 89.16668F);
            this.xrLabel2.Name = "xrLabel2";
            this.xrLabel2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel2.SizeF = new System.Drawing.SizeF(853.125F, 23F);
            this.xrLabel2.StylePriority.UseFont = false;
            this.xrLabel2.StylePriority.UseTextAlignment = false;
            this.xrLabel2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // xrLabel1
            // 
            this.xrLabel1.Font = new System.Drawing.Font("Trebuchet MS", 11F, System.Drawing.FontStyle.Bold);
            this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(303.125F, 55.83334F);
            this.xrLabel1.Name = "xrLabel1";
            this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel1.SizeF = new System.Drawing.SizeF(368.75F, 23F);
            this.xrLabel1.StylePriority.UseFont = false;
            this.xrLabel1.StylePriority.UseTextAlignment = false;
            this.xrLabel1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // BottomMargin
            // 
            this.BottomMargin.HeightF = 8.333333F;
            this.BottomMargin.Name = "BottomMargin";
            this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // viewTransactionPostCollectionBankTableAdapter
            // 
            this.viewTransactionPostCollectionBankTableAdapter.ClearBeforeFill = true;
            // 
            // paramPeriod
            // 
            this.paramPeriod.Name = "paramPeriod";
            this.paramPeriod.Visible = false;
            // 
            // XtraRepCrossTab
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin});
            this.Landscape = true;
            this.Margins = new System.Drawing.Printing.Margins(10, 10, 122, 8);
            this.PageHeight = 850;
            this.PageWidth = 1100;
            this.PaperKind = System.Drawing.Printing.PaperKind.Custom;
            this.Parameters.AddRange(new DevExpress.XtraReports.Parameters.Parameter[] {
            this.paramPeriod});
            this.Version = "14.2";
            this.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.XtraRepCrossTab_BeforePrint);
            ((System.ComponentModel.ISupportInitialize)(this.dsTransaction1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.DetailBand Detail;
        private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
        private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
        private Dataset.dsTransactionTableAdapters.ViewIGRAccountDetailsTableAdapter viewIGRAccountDetailsTableAdapter;
        private Dataset.dsTransaction dsTransaction1;
        private Dataset.dsTransactionTableAdapters.ViewTransactionPostCollectionBankTableAdapter viewTransactionPostCollectionBankTableAdapter;
        private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField fieldAmount;
        private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField fieldBankName;
        private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField fieldTransDescription;
        public DevExpress.XtraReports.Parameters.Parameter paramPeriod;
        public DevExpress.XtraReports.UI.XRLabel xrLabel1;
        public DevExpress.XtraReports.UI.XRLabel xrLabel2;
        public DevExpress.XtraReports.UI.XRPivotGrid xrPivotGrid1;
        private DevExpress.XtraReports.UI.XRPictureBox xrPictureBox1;
        private DevExpress.XtraReports.UI.XRPictureBox xrPictureBox2;
    }
}
