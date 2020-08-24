namespace BankReconciliation.Report
{
    partial class XtraRepCrossTabCon
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
            this.viewConsolidatedTableAdapter = new BankReconciliation.Dataset.dsTransactionTableAdapters.ViewConsolidatedTableAdapter();
            this.dsTransaction1 = new BankReconciliation.Dataset.dsTransaction();
            this.fieldOpenBal1 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.fieldCloseBal1 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.fieldtransdescription1 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.fieldAmount1 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.fieldBankName1 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.fieldCollectionAmt1 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.viewSummaryIGRAccountTableAdapter = new BankReconciliation.Dataset.dsTransactionTableAdapters.ViewSummaryIGRAccountTableAdapter();
            this.viewIGRAccountDetailsTableAdapter = new BankReconciliation.Dataset.dsTransactionTableAdapters.ViewIGRAccountDetailsTableAdapter();
            this.viewTransactionPostCollectionBankTableAdapter = new BankReconciliation.Dataset.dsTransactionTableAdapters.ViewTransactionPostCollectionBankTableAdapter();
            ((System.ComponentModel.ISupportInitialize)(this.dsTransaction1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrPivotGrid1});
            this.Detail.HeightF = 100F;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrPivotGrid1
            // 
            this.xrPivotGrid1.DataAdapter = this.viewConsolidatedTableAdapter;
            this.xrPivotGrid1.DataMember = "ViewConsolidated";
            this.xrPivotGrid1.DataSource = this.dsTransaction1;
            this.xrPivotGrid1.Fields.AddRange(new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField[] {
            this.fieldOpenBal1,
            this.fieldCloseBal1,
            this.fieldtransdescription1,
            this.fieldAmount1,
            this.fieldBankName1,
            this.fieldCollectionAmt1});
            this.xrPivotGrid1.LocationFloat = new DevExpress.Utils.PointFloat(10.00001F, 10.00001F);
            this.xrPivotGrid1.Name = "xrPivotGrid1";
            this.xrPivotGrid1.SizeF = new System.Drawing.SizeF(267.0833F, 79.99999F);
            // 
            // viewConsolidatedTableAdapter
            // 
            this.viewConsolidatedTableAdapter.ClearBeforeFill = true;
            // 
            // dsTransaction1
            // 
            this.dsTransaction1.DataSetName = "dsTransaction";
            this.dsTransaction1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // fieldOpenBal1
            // 
            this.fieldOpenBal1.Area = DevExpress.XtraPivotGrid.PivotArea.DataArea;
            this.fieldOpenBal1.AreaIndex = 3;
            this.fieldOpenBal1.Caption = "Open Bal";
            this.fieldOpenBal1.FieldName = "OpenBal";
            this.fieldOpenBal1.Name = "fieldOpenBal1";
            // 
            // fieldCloseBal1
            // 
            this.fieldCloseBal1.Area = DevExpress.XtraPivotGrid.PivotArea.DataArea;
            this.fieldCloseBal1.AreaIndex = 2;
            this.fieldCloseBal1.Caption = "Close Bal";
            this.fieldCloseBal1.FieldName = "CloseBal";
            this.fieldCloseBal1.Name = "fieldCloseBal1";
            // 
            // fieldtransdescription1
            // 
            this.fieldtransdescription1.Area = DevExpress.XtraPivotGrid.PivotArea.ColumnArea;
            this.fieldtransdescription1.AreaIndex = 0;
            this.fieldtransdescription1.Caption = "transdescription";
            this.fieldtransdescription1.FieldName = "transdescription";
            this.fieldtransdescription1.Name = "fieldtransdescription1";
            // 
            // fieldAmount1
            // 
            this.fieldAmount1.Area = DevExpress.XtraPivotGrid.PivotArea.DataArea;
            this.fieldAmount1.AreaIndex = 0;
            this.fieldAmount1.Caption = "Amount";
            this.fieldAmount1.FieldName = "Amount";
            this.fieldAmount1.Name = "fieldAmount1";
            // 
            // fieldBankName1
            // 
            this.fieldBankName1.Area = DevExpress.XtraPivotGrid.PivotArea.RowArea;
            this.fieldBankName1.AreaIndex = 0;
            this.fieldBankName1.Caption = "Bank Name";
            this.fieldBankName1.FieldName = "BankName";
            this.fieldBankName1.Name = "fieldBankName1";
            // 
            // fieldCollectionAmt1
            // 
            this.fieldCollectionAmt1.Area = DevExpress.XtraPivotGrid.PivotArea.DataArea;
            this.fieldCollectionAmt1.AreaIndex = 1;
            this.fieldCollectionAmt1.Caption = "Collection Amt";
            this.fieldCollectionAmt1.FieldName = "CollectionAmt";
            this.fieldCollectionAmt1.Name = "fieldCollectionAmt1";
            // 
            // TopMargin
            // 
            this.TopMargin.HeightF = 100F;
            this.TopMargin.Name = "TopMargin";
            this.TopMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.TopMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // BottomMargin
            // 
            this.BottomMargin.HeightF = 100F;
            this.BottomMargin.Name = "BottomMargin";
            this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // viewSummaryIGRAccountTableAdapter
            // 
            this.viewSummaryIGRAccountTableAdapter.ClearBeforeFill = true;
            // 
            // viewIGRAccountDetailsTableAdapter
            // 
            this.viewIGRAccountDetailsTableAdapter.ClearBeforeFill = true;
            // 
            // viewTransactionPostCollectionBankTableAdapter
            // 
            this.viewTransactionPostCollectionBankTableAdapter.ClearBeforeFill = true;
            // 
            // XtraRepCrossTabCon
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin});
            this.Version = "14.2";
            ((System.ComponentModel.ISupportInitialize)(this.dsTransaction1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.DetailBand Detail;
        private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
        private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
        private DevExpress.XtraReports.UI.XRPivotGrid xrPivotGrid1;
        private Dataset.dsTransactionTableAdapters.ViewSummaryIGRAccountTableAdapter viewSummaryIGRAccountTableAdapter;
        private Dataset.dsTransactionTableAdapters.ViewIGRAccountDetailsTableAdapter viewIGRAccountDetailsTableAdapter;
        private Dataset.dsTransactionTableAdapters.ViewConsolidatedTableAdapter viewConsolidatedTableAdapter;
        private Dataset.dsTransaction dsTransaction1;
        private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField fieldOpenBal1;
        private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField fieldCloseBal1;
        private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField fieldtransdescription1;
        private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField fieldAmount1;
        private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField fieldBankName1;
        private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField fieldCollectionAmt1;
        private Dataset.dsTransactionTableAdapters.ViewTransactionPostCollectionBankTableAdapter viewTransactionPostCollectionBankTableAdapter;
    }
}
