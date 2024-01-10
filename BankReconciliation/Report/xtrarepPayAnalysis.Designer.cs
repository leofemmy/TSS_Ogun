namespace BankReconciliation.Report
{
    partial class xtrarepPayAnalysis
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
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.tblTransactionPostingRequestViewPostBankStatment1 = new BankReconciliation.tblTransactionPostingRequestViewPostBankStatment();
            this.viewPostingRequestBankTableAdapter = new BankReconciliation.tblTransactionPostingRequestViewPostBankStatmentTableAdapters.ViewPostingRequestBankTableAdapter();
            this.viewPayAnalysisTableAdapter1 = new BankReconciliation.tblTransactionPostingRequestViewPostBankStatmentTableAdapters.ViewPayAnalysisTableAdapter();
            this.calJan = new DevExpress.XtraReports.UI.CalculatedField();
            this.calculFeb = new DevExpress.XtraReports.UI.CalculatedField();
            this.calculMar = new DevExpress.XtraReports.UI.CalculatedField();
            this.calApril = new DevExpress.XtraReports.UI.CalculatedField();
            this.calculMay = new DevExpress.XtraReports.UI.CalculatedField();
            this.calJune = new DevExpress.XtraReports.UI.CalculatedField();
            this.calJunly = new DevExpress.XtraReports.UI.CalculatedField();
            this.calAug = new DevExpress.XtraReports.UI.CalculatedField();
            this.calSep = new DevExpress.XtraReports.UI.CalculatedField();
            this.calOct = new DevExpress.XtraReports.UI.CalculatedField();
            this.calNov = new DevExpress.XtraReports.UI.CalculatedField();
            this.calDec = new DevExpress.XtraReports.UI.CalculatedField();
            this.xrPivotGrid1 = new DevExpress.XtraReports.UI.XRPivotGrid();
            this.fieldMONTH1 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.fieldyear1 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.fieldPayer1 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.fieldMonthAmt1 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            ((System.ComponentModel.ISupportInitialize)(this.tblTransactionPostingRequestViewPostBankStatment1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrPivotGrid1});
            this.Detail.HeightF = 107.375F;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
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
            // tblTransactionPostingRequestViewPostBankStatment1
            // 
            this.tblTransactionPostingRequestViewPostBankStatment1.DataSetName = "tblTransactionPostingRequestViewPostBankStatment";
            this.tblTransactionPostingRequestViewPostBankStatment1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // viewPostingRequestBankTableAdapter
            // 
            this.viewPostingRequestBankTableAdapter.ClearBeforeFill = true;
            // 
            // viewPayAnalysisTableAdapter1
            // 
            this.viewPayAnalysisTableAdapter1.ClearBeforeFill = true;
            // 
            // calJan
            // 
            this.calJan.DataMember = "ViewPayAnalysis";
            this.calJan.Expression = "Iif([MONTH]=\'01\',[MonthAmt] ,0)";
            this.calJan.Name = "calJan";
            // 
            // calculFeb
            // 
            this.calculFeb.DataMember = "ViewPayAnalysis";
            this.calculFeb.Expression = "Iif([MONTH]=\'02\',[MonthAmt] ,0)";
            this.calculFeb.Name = "calculFeb";
            // 
            // calculMar
            // 
            this.calculMar.DataMember = "ViewPayAnalysis";
            this.calculMar.Expression = "Iif([MONTH]=\'03\',[MonthAmt] ,0)";
            this.calculMar.Name = "calculMar";
            // 
            // calApril
            // 
            this.calApril.DataMember = "ViewPayAnalysis";
            this.calApril.Expression = "Iif([MONTH]=\'04\',[MonthAmt] ,0)";
            this.calApril.Name = "calApril";
            // 
            // calculMay
            // 
            this.calculMay.DataMember = "ViewPayAnalysis";
            this.calculMay.Expression = "Iif([MONTH]=\'05\',[MonthAmt] ,0)";
            this.calculMay.Name = "calculMay";
            // 
            // calJune
            // 
            this.calJune.DataMember = "ViewPayAnalysis";
            this.calJune.Expression = "Iif([MONTH]=\'06\',[MonthAmt] ,0)";
            this.calJune.Name = "calJune";
            // 
            // calJunly
            // 
            this.calJunly.DataMember = "ViewPayAnalysis";
            this.calJunly.Expression = "Iif([MONTH]=\'07\',[MonthAmt] ,0)";
            this.calJunly.Name = "calJunly";
            // 
            // calAug
            // 
            this.calAug.DataMember = "ViewPayAnalysis";
            this.calAug.Expression = "Iif([MONTH]=\'08\',[MonthAmt] ,0)";
            this.calAug.Name = "calAug";
            // 
            // calSep
            // 
            this.calSep.DataMember = "ViewPayAnalysis";
            this.calSep.Expression = "Iif([MONTH]=\'09\',[MonthAmt] ,0)";
            this.calSep.Name = "calSep";
            // 
            // calOct
            // 
            this.calOct.DataMember = "ViewPayAnalysis";
            this.calOct.Expression = "Iif([MONTH]=\'10\',[MonthAmt] ,0)";
            this.calOct.Name = "calOct";
            // 
            // calNov
            // 
            this.calNov.DataMember = "ViewPayAnalysis";
            this.calNov.Expression = "Iif([MONTH]=\'11\',[MonthAmt] ,0)";
            this.calNov.Name = "calNov";
            // 
            // calDec
            // 
            this.calDec.DataMember = "ViewPayAnalysis";
            this.calDec.Expression = "Iif([MONTH]=\'12\',[MonthAmt] ,0)";
            this.calDec.Name = "calDec";
            // 
            // xrPivotGrid1
            // 
            this.xrPivotGrid1.Appearance.Cell.Font = new DevExpress.Drawing.DXFont("Times New Roman", 9.75F);
            this.xrPivotGrid1.Appearance.CustomTotalCell.Font = new DevExpress.Drawing.DXFont("Times New Roman", 9.75F);
            this.xrPivotGrid1.Appearance.FieldHeader.Font = new DevExpress.Drawing.DXFont("Times New Roman", 9.75F);
            this.xrPivotGrid1.Appearance.FieldValue.Font = new DevExpress.Drawing.DXFont("Times New Roman", 9.75F);
            this.xrPivotGrid1.Appearance.FieldValueGrandTotal.Font = new DevExpress.Drawing.DXFont("Times New Roman", 9.75F);
            this.xrPivotGrid1.Appearance.FieldValueTotal.Font = new DevExpress.Drawing.DXFont("Times New Roman", 9.75F);
            this.xrPivotGrid1.Appearance.GrandTotalCell.Font = new DevExpress.Drawing.DXFont("Times New Roman", 9.75F);
            this.xrPivotGrid1.Appearance.Lines.Font = new DevExpress.Drawing.DXFont("Times New Roman", 9.75F);
            this.xrPivotGrid1.Appearance.TotalCell.Font = new DevExpress.Drawing.DXFont("Times New Roman", 9.75F);
            this.xrPivotGrid1.DataAdapter = this.viewPayAnalysisTableAdapter1;
            this.xrPivotGrid1.DataMember = "ViewPayAnalysis";
            this.xrPivotGrid1.Fields.AddRange(new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField[] {
            this.fieldMONTH1,
            this.fieldyear1,
            this.fieldPayer1,
            this.fieldMonthAmt1});
            this.xrPivotGrid1.LocationFloat = new DevExpress.Utils.PointFloat(10.00001F, 0F);
            this.xrPivotGrid1.Name = "xrPivotGrid1";
            this.xrPivotGrid1.OptionsPrint.FilterSeparatorBarPadding = 3;
            this.xrPivotGrid1.SizeF = new System.Drawing.SizeF(968.9911F, 104.1667F);
            // 
            // fieldMONTH1
            // 
            this.fieldMONTH1.Appearance.Cell.Font = new DevExpress.Drawing.DXFont("Times New Roman", 9.75F);
            this.fieldMONTH1.Appearance.CustomTotalCell.Font = new DevExpress.Drawing.DXFont("Times New Roman", 9.75F);
            this.fieldMONTH1.Appearance.FieldHeader.Font = new DevExpress.Drawing.DXFont("Times New Roman", 9.75F);
            this.fieldMONTH1.Appearance.FieldValueGrandTotal.Font = new DevExpress.Drawing.DXFont("Times New Roman", 9.75F);
            this.fieldMONTH1.Appearance.FieldValueTotal.Font = new DevExpress.Drawing.DXFont("Times New Roman", 9.75F);
            this.fieldMONTH1.Appearance.GrandTotalCell.Font = new DevExpress.Drawing.DXFont("Times New Roman", 9.75F);
            this.fieldMONTH1.Appearance.TotalCell.Font = new DevExpress.Drawing.DXFont("Times New Roman", 9.75F);
            this.fieldMONTH1.Area = DevExpress.XtraPivotGrid.PivotArea.ColumnArea;
            this.fieldMONTH1.AreaIndex = 0;
            this.fieldMONTH1.Caption = "MONTH";
            this.fieldMONTH1.FieldName = "MONTH";
            this.fieldMONTH1.Name = "fieldMONTH1";
            // 
            // fieldyear1
            // 
            this.fieldyear1.Appearance.Cell.Font = new DevExpress.Drawing.DXFont("Times New Roman", 9.75F);
            this.fieldyear1.Appearance.CustomTotalCell.Font = new DevExpress.Drawing.DXFont("Times New Roman", 9.75F);
            this.fieldyear1.Appearance.FieldHeader.Font = new DevExpress.Drawing.DXFont("Times New Roman", 9.75F);
            this.fieldyear1.Appearance.FieldValueGrandTotal.Font = new DevExpress.Drawing.DXFont("Times New Roman", 9.75F);
            this.fieldyear1.Appearance.FieldValueTotal.Font = new DevExpress.Drawing.DXFont("Times New Roman", 9.75F);
            this.fieldyear1.Appearance.GrandTotalCell.Font = new DevExpress.Drawing.DXFont("Times New Roman", 9.75F);
            this.fieldyear1.Appearance.TotalCell.Font = new DevExpress.Drawing.DXFont("Times New Roman", 9.75F);
            this.fieldyear1.Area = DevExpress.XtraPivotGrid.PivotArea.ColumnArea;
            this.fieldyear1.AreaIndex = 1;
            this.fieldyear1.Caption = "year";
            this.fieldyear1.FieldName = "year";
            this.fieldyear1.Name = "fieldyear1";
            // 
            // fieldPayer1
            // 
            this.fieldPayer1.Appearance.Cell.Font = new DevExpress.Drawing.DXFont("Times New Roman", 9.75F);
            this.fieldPayer1.Appearance.CustomTotalCell.Font = new DevExpress.Drawing.DXFont("Times New Roman", 9.75F);
            this.fieldPayer1.Appearance.FieldHeader.Font = new DevExpress.Drawing.DXFont("Times New Roman", 9.75F);
            this.fieldPayer1.Appearance.FieldValueGrandTotal.Font = new DevExpress.Drawing.DXFont("Times New Roman", 9.75F);
            this.fieldPayer1.Appearance.FieldValueTotal.Font = new DevExpress.Drawing.DXFont("Times New Roman", 9.75F);
            this.fieldPayer1.Appearance.GrandTotalCell.Font = new DevExpress.Drawing.DXFont("Times New Roman", 9.75F);
            this.fieldPayer1.Appearance.TotalCell.Font = new DevExpress.Drawing.DXFont("Times New Roman", 9.75F);
            this.fieldPayer1.Area = DevExpress.XtraPivotGrid.PivotArea.RowArea;
            this.fieldPayer1.AreaIndex = 0;
            this.fieldPayer1.Caption = "Payer";
            this.fieldPayer1.FieldName = "Payer";
            this.fieldPayer1.Name = "fieldPayer1";
            // 
            // fieldMonthAmt1
            // 
            this.fieldMonthAmt1.Appearance.Cell.Font = new DevExpress.Drawing.DXFont("Times New Roman", 9.75F);
            this.fieldMonthAmt1.Appearance.CustomTotalCell.Font = new DevExpress.Drawing.DXFont("Times New Roman", 9.75F);
            this.fieldMonthAmt1.Appearance.FieldHeader.Font = new DevExpress.Drawing.DXFont("Times New Roman", 9.75F);
            this.fieldMonthAmt1.Appearance.FieldValueGrandTotal.Font = new DevExpress.Drawing.DXFont("Times New Roman", 9.75F);
            this.fieldMonthAmt1.Appearance.FieldValueTotal.Font = new DevExpress.Drawing.DXFont("Times New Roman", 9.75F);
            this.fieldMonthAmt1.Appearance.GrandTotalCell.Font = new DevExpress.Drawing.DXFont("Times New Roman", 9.75F);
            this.fieldMonthAmt1.Appearance.TotalCell.Font = new DevExpress.Drawing.DXFont("Times New Roman", 9.75F);
            this.fieldMonthAmt1.Area = DevExpress.XtraPivotGrid.PivotArea.DataArea;
            this.fieldMonthAmt1.AreaIndex = 0;
            this.fieldMonthAmt1.Caption = "Month Amt";
            this.fieldMonthAmt1.FieldName = "MonthAmt";
            this.fieldMonthAmt1.Name = "fieldMonthAmt1";
            // 
            // xtrarepPayAnalysis
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin});
            this.CalculatedFields.AddRange(new DevExpress.XtraReports.UI.CalculatedField[] {
            this.calJan,
            this.calculFeb,
            this.calculMar,
            this.calApril,
            this.calculMay,
            this.calJune,
            this.calJunly,
            this.calAug,
            this.calSep,
            this.calOct,
            this.calNov,
            this.calDec});
            this.DataAdapter = this.viewPayAnalysisTableAdapter1;
            this.DataMember = "ViewPayAnalysis";
            this.DataSource = this.tblTransactionPostingRequestViewPostBankStatment1;
            this.Landscape = true;
            this.Margins = new DevExpress.Drawing.DXMargins(100, 9, 100, 100);
            this.PageHeight = 850;
            this.PageWidth = 1100;
            this.Version = "13.2";
            ((System.ComponentModel.ISupportInitialize)(this.tblTransactionPostingRequestViewPostBankStatment1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.DetailBand Detail;
        private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
        private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
        private tblTransactionPostingRequestViewPostBankStatment tblTransactionPostingRequestViewPostBankStatment1;
        private tblTransactionPostingRequestViewPostBankStatmentTableAdapters.ViewPostingRequestBankTableAdapter viewPostingRequestBankTableAdapter;
        private tblTransactionPostingRequestViewPostBankStatmentTableAdapters.ViewPayAnalysisTableAdapter viewPayAnalysisTableAdapter1;
        private DevExpress.XtraReports.UI.CalculatedField calJan;
        private DevExpress.XtraReports.UI.CalculatedField calculFeb;
        private DevExpress.XtraReports.UI.CalculatedField calculMar;
        private DevExpress.XtraReports.UI.CalculatedField calApril;
        private DevExpress.XtraReports.UI.CalculatedField calculMay;
        private DevExpress.XtraReports.UI.CalculatedField calJune;
        private DevExpress.XtraReports.UI.CalculatedField calJunly;
        private DevExpress.XtraReports.UI.CalculatedField calAug;
        private DevExpress.XtraReports.UI.CalculatedField calSep;
        private DevExpress.XtraReports.UI.CalculatedField calOct;
        private DevExpress.XtraReports.UI.CalculatedField calNov;
        private DevExpress.XtraReports.UI.CalculatedField calDec;
        private DevExpress.XtraReports.UI.XRPivotGrid xrPivotGrid1;
        private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField fieldMONTH1;
        private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField fieldyear1;
        private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField fieldPayer1;
        private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField fieldMonthAmt1;
    }
}
