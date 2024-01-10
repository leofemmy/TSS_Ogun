namespace BankReconciliation.Report
{
    partial class xrTranAccount
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
            DevExpress.XtraReports.UI.XRSummary xrSummary1 = new DevExpress.XtraReports.UI.XRSummary();
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.xrLabel6 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel5 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel4 = new DevExpress.XtraReports.UI.XRLabel();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.dsTransaction1 = new BankReconciliation.Dataset.dsTransaction();
            this.viewTransactionPostCollectionBankTableAdapter = new BankReconciliation.Dataset.dsTransactionTableAdapters.ViewTransactionPostCollectionBankTableAdapter();
            this.PageHeader = new DevExpress.XtraReports.UI.PageHeaderBand();
            this.xrLabel18 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
            this.GroupHeader1 = new DevExpress.XtraReports.UI.GroupHeaderBand();
            this.xrOpenBal = new DevExpress.XtraReports.UI.XRLabel();
            this.xrCloseBal = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLine1 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLabel15 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel14 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel13 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel11 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel8 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel7 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel2 = new DevExpress.XtraReports.UI.XRLabel();
            this.GroupFooter1 = new DevExpress.XtraReports.UI.GroupFooterBand();
            this.xrDiffAmt = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel17 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel10 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel9 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel3 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLine3 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLine2 = new DevExpress.XtraReports.UI.XRLine();
            this.xrAmt = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel16 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrSummaryCr = new DevExpress.XtraReports.UI.XRLabel();
            this.xrSummaryDr = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLine4 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLine5 = new DevExpress.XtraReports.UI.XRLine();
            this.calculatedField1 = new DevExpress.XtraReports.UI.CalculatedField();
            this.paramCloseDb = new DevExpress.XtraReports.Parameters.Parameter();
            this.calFdMonth = new DevExpress.XtraReports.UI.CalculatedField();
            this.paramAccount = new DevExpress.XtraReports.Parameters.Parameter();
            this.paramPeriod = new DevExpress.XtraReports.Parameters.Parameter();
            this.paramRecord = new DevExpress.XtraReports.Parameters.Parameter();
            this.paramPayDirect = new DevExpress.XtraReports.Parameters.Parameter();
            ((System.ComponentModel.ISupportInitialize)(this.dsTransaction1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel6,
            this.xrLabel5,
            this.xrLabel4});
            this.Detail.HeightF = 24F;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrLabel6
            // 
            this.xrLabel6.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "ViewTransactionPostCollectionBank.Cr", "# {0:n2}")});
            this.xrLabel6.LocationFloat = new DevExpress.Utils.PointFloat(451.0416F, 0F);
            this.xrLabel6.Name = "xrLabel6";
            this.xrLabel6.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel6.SizeF = new System.Drawing.SizeF(100F, 23F);
            xrSummary1.FormatString = "# {0:n2}";
            xrSummary1.IgnoreNullValues = true;
            this.xrLabel6.Summary = xrSummary1;
            this.xrLabel6.Text = "xrLabel6";
            // 
            // xrLabel5
            // 
            this.xrLabel5.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "ViewTransactionPostCollectionBank.Dr", "# {0:n2}")});
            this.xrLabel5.LocationFloat = new DevExpress.Utils.PointFloat(302.0833F, 0F);
            this.xrLabel5.Name = "xrLabel5";
            this.xrLabel5.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel5.SizeF = new System.Drawing.SizeF(100F, 23F);
            this.xrLabel5.Text = "xrLabel5";
            // 
            // xrLabel4
            // 
            this.xrLabel4.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "ViewTransactionPostCollectionBank.Description")});
            this.xrLabel4.LocationFloat = new DevExpress.Utils.PointFloat(10.00001F, 0F);
            this.xrLabel4.Name = "xrLabel4";
            this.xrLabel4.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel4.SizeF = new System.Drawing.SizeF(262.9167F, 22.99999F);
            this.xrLabel4.Text = "xrLabel4";
            // 
            // TopMargin
            // 
            this.TopMargin.HeightF = 50F;
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
            // PageHeader
            // 
            this.PageHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel18,
            this.xrLabel1});
            this.PageHeader.HeightF = 75.00002F;
            this.PageHeader.Name = "PageHeader";
            // 
            // xrLabel18
            // 
            this.xrLabel18.Font = new DevExpress.Drawing.DXFont("Times New Roman", 10F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrLabel18.LocationFloat = new DevExpress.Utils.PointFloat(172.9167F, 42.00004F);
            this.xrLabel18.Name = "xrLabel18";
            this.xrLabel18.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel18.SizeF = new System.Drawing.SizeF(329.1667F, 23F);
            this.xrLabel18.StylePriority.UseFont = false;
            this.xrLabel18.StylePriority.UseTextAlignment = false;
            this.xrLabel18.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // xrLabel1
            // 
            this.xrLabel1.Font = new DevExpress.Drawing.DXFont("Times New Roman", 11F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(64.5833F, 10.00001F);
            this.xrLabel1.Name = "xrLabel1";
            this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel1.SizeF = new System.Drawing.SizeF(543.75F, 24.04166F);
            this.xrLabel1.StylePriority.UseFont = false;
            this.xrLabel1.StylePriority.UseTextAlignment = false;
            this.xrLabel1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // GroupHeader1
            // 
            this.GroupHeader1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrOpenBal,
            this.xrCloseBal,
            this.xrLine1,
            this.xrLabel15,
            this.xrLabel14,
            this.xrLabel13,
            this.xrLabel11,
            this.xrLabel8,
            this.xrLabel7,
            this.xrLabel2});
            this.GroupHeader1.GroupFields.AddRange(new DevExpress.XtraReports.UI.GroupField[] {
            new DevExpress.XtraReports.UI.GroupField("AccountNo", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending)});
            this.GroupHeader1.HeightF = 99.08333F;
            this.GroupHeader1.Name = "GroupHeader1";
            // 
            // xrOpenBal
            // 
            this.xrOpenBal.LocationFloat = new DevExpress.Utils.PointFloat(150F, 39.24999F);
            this.xrOpenBal.Name = "xrOpenBal";
            this.xrOpenBal.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrOpenBal.SizeF = new System.Drawing.SizeF(129.1667F, 23F);
            this.xrOpenBal.Text = "xrOpenBal";
            // 
            // xrCloseBal
            // 
            this.xrCloseBal.LocationFloat = new DevExpress.Utils.PointFloat(491.6666F, 39.24999F);
            this.xrCloseBal.Name = "xrCloseBal";
            this.xrCloseBal.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrCloseBal.SizeF = new System.Drawing.SizeF(116.6667F, 23F);
            this.xrCloseBal.Text = "xrCloseBal";
            // 
            // xrLine1
            // 
            this.xrLine1.LineWidth = 2;
            this.xrLine1.LocationFloat = new DevExpress.Utils.PointFloat(4.124991F, 96.99999F);
            this.xrLine1.Name = "xrLine1";
            this.xrLine1.SizeF = new System.Drawing.SizeF(646.875F, 2.083336F);
            // 
            // xrLabel15
            // 
            this.xrLabel15.Font = new DevExpress.Drawing.DXFont("Times New Roman", 9.75F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrLabel15.LocationFloat = new DevExpress.Utils.PointFloat(461.4583F, 74.00001F);
            this.xrLabel15.Name = "xrLabel15";
            this.xrLabel15.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel15.SizeF = new System.Drawing.SizeF(100F, 23F);
            this.xrLabel15.StylePriority.UseFont = false;
            this.xrLabel15.Text = "Credit";
            // 
            // xrLabel14
            // 
            this.xrLabel14.Font = new DevExpress.Drawing.DXFont("Times New Roman", 9.75F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrLabel14.LocationFloat = new DevExpress.Utils.PointFloat(302.0833F, 74.00001F);
            this.xrLabel14.Name = "xrLabel14";
            this.xrLabel14.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel14.SizeF = new System.Drawing.SizeF(87.5F, 23F);
            this.xrLabel14.StylePriority.UseFont = false;
            this.xrLabel14.Text = "Debit";
            // 
            // xrLabel13
            // 
            this.xrLabel13.Font = new DevExpress.Drawing.DXFont("Times New Roman", 9.75F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrLabel13.LocationFloat = new DevExpress.Utils.PointFloat(25F, 74.00001F);
            this.xrLabel13.Name = "xrLabel13";
            this.xrLabel13.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel13.SizeF = new System.Drawing.SizeF(100F, 23F);
            this.xrLabel13.StylePriority.UseFont = false;
            this.xrLabel13.Text = "Description";
            // 
            // xrLabel11
            // 
            this.xrLabel11.Font = new DevExpress.Drawing.DXFont("Times New Roman", 9.75F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrLabel11.LocationFloat = new DevExpress.Utils.PointFloat(15.62494F, 39.24999F);
            this.xrLabel11.Name = "xrLabel11";
            this.xrLabel11.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel11.SizeF = new System.Drawing.SizeF(116.6667F, 23F);
            this.xrLabel11.StylePriority.UseFont = false;
            this.xrLabel11.Text = "Opening Balance:";
            // 
            // xrLabel8
            // 
            this.xrLabel8.Font = new DevExpress.Drawing.DXFont("Times New Roman", 9.75F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrLabel8.LocationFloat = new DevExpress.Utils.PointFloat(25F, 0F);
            this.xrLabel8.Name = "xrLabel8";
            this.xrLabel8.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel8.SizeF = new System.Drawing.SizeF(100F, 23F);
            this.xrLabel8.StylePriority.UseFont = false;
            this.xrLabel8.Text = "Account No:";
            // 
            // xrLabel7
            // 
            this.xrLabel7.Font = new DevExpress.Drawing.DXFont("Times New Roman", 9.75F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrLabel7.LocationFloat = new DevExpress.Utils.PointFloat(349.625F, 39.24999F);
            this.xrLabel7.Name = "xrLabel7";
            this.xrLabel7.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel7.SizeF = new System.Drawing.SizeF(121.875F, 23F);
            this.xrLabel7.StylePriority.UseFont = false;
            this.xrLabel7.Text = "Closing Balance:";
            // 
            // xrLabel2
            // 
            this.xrLabel2.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "ViewTransactionPostCollectionBank.AccountNo")});
            this.xrLabel2.LocationFloat = new DevExpress.Utils.PointFloat(150F, 0F);
            this.xrLabel2.Name = "xrLabel2";
            this.xrLabel2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel2.SizeF = new System.Drawing.SizeF(100F, 23F);
            this.xrLabel2.Text = "xrLabel2";
            // 
            // GroupFooter1
            // 
            this.GroupFooter1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrDiffAmt,
            this.xrLabel17,
            this.xrLabel10,
            this.xrLabel9,
            this.xrLabel3,
            this.xrLine3,
            this.xrLine2,
            this.xrAmt,
            this.xrLabel16,
            this.xrSummaryCr,
            this.xrSummaryDr,
            this.xrLine4,
            this.xrLine5});
            this.GroupFooter1.HeightF = 143.4583F;
            this.GroupFooter1.Name = "GroupFooter1";
            // 
            // xrDiffAmt
            // 
            this.xrDiffAmt.Font = new DevExpress.Drawing.DXFont("Times New Roman", 11F);
            this.xrDiffAmt.ForeColor = System.Drawing.Color.Red;
            this.xrDiffAmt.LocationFloat = new DevExpress.Utils.PointFloat(451.0416F, 110.125F);
            this.xrDiffAmt.Name = "xrDiffAmt";
            this.xrDiffAmt.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrDiffAmt.SizeF = new System.Drawing.SizeF(146.8751F, 23F);
            this.xrDiffAmt.StylePriority.UseFont = false;
            this.xrDiffAmt.StylePriority.UseForeColor = false;
            // 
            // xrLabel17
            // 
            this.xrLabel17.Font = new DevExpress.Drawing.DXFont("Times New Roman", 10F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrLabel17.LocationFloat = new DevExpress.Utils.PointFloat(4.124991F, 110.125F);
            this.xrLabel17.Name = "xrLabel17";
            this.xrLabel17.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel17.SizeF = new System.Drawing.SizeF(245.875F, 22.99998F);
            this.xrLabel17.StylePriority.UseFont = false;
            this.xrLabel17.Text = "Difference in Collections";
            // 
            // xrLabel10
            // 
            this.xrLabel10.Font = new DevExpress.Drawing.DXFont("Times New Roman", 10F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrLabel10.LocationFloat = new DevExpress.Utils.PointFloat(4.124991F, 54.12496F);
            this.xrLabel10.Name = "xrLabel10";
            this.xrLabel10.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel10.SizeF = new System.Drawing.SizeF(262.9166F, 23.00002F);
            this.xrLabel10.StylePriority.UseFont = false;
            this.xrLabel10.Text = "Total REEMS Collections";
            // 
            // xrLabel9
            // 
            this.xrLabel9.Font = new DevExpress.Drawing.DXFont("Times New Roman", 10F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrLabel9.LocationFloat = new DevExpress.Utils.PointFloat(4.124896F, 77.12498F);
            this.xrLabel9.Name = "xrLabel9";
            this.xrLabel9.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel9.SizeF = new System.Drawing.SizeF(262.9167F, 23.00002F);
            this.xrLabel9.StylePriority.UseFont = false;
            this.xrLabel9.Text = "Total Collections Per Bank";
            // 
            // xrLabel3
            // 
            this.xrLabel3.LocationFloat = new DevExpress.Utils.PointFloat(25F, 2.083336F);
            this.xrLabel3.Name = "xrLabel3";
            this.xrLabel3.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel3.SizeF = new System.Drawing.SizeF(100F, 23F);
            this.xrLabel3.Text = "Total";
            // 
            // xrLine3
            // 
            this.xrLine3.LineWidth = 2;
            this.xrLine3.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrLine3.Name = "xrLine3";
            this.xrLine3.SizeF = new System.Drawing.SizeF(646.875F, 2.083336F);
            // 
            // xrLine2
            // 
            this.xrLine2.LineWidth = 2;
            this.xrLine2.LocationFloat = new DevExpress.Utils.PointFloat(0F, 25.08334F);
            this.xrLine2.Name = "xrLine2";
            this.xrLine2.SizeF = new System.Drawing.SizeF(646.875F, 2.083336F);
            // 
            // xrAmt
            // 
            this.xrAmt.Font = new DevExpress.Drawing.DXFont("Times New Roman", 11F);
            this.xrAmt.LocationFloat = new DevExpress.Utils.PointFloat(451.0415F, 54.12499F);
            this.xrAmt.Name = "xrAmt";
            this.xrAmt.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrAmt.SizeF = new System.Drawing.SizeF(146.8751F, 23F);
            this.xrAmt.StylePriority.UseFont = false;
            // 
            // xrLabel16
            // 
            this.xrLabel16.Font = new DevExpress.Drawing.DXFont("Times New Roman", 11F);
            this.xrLabel16.ForeColor = System.Drawing.Color.Navy;
            this.xrLabel16.LocationFloat = new DevExpress.Utils.PointFloat(451.0416F, 77.12498F);
            this.xrLabel16.Name = "xrLabel16";
            this.xrLabel16.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel16.SizeF = new System.Drawing.SizeF(146.875F, 23F);
            this.xrLabel16.StylePriority.UseFont = false;
            this.xrLabel16.StylePriority.UseForeColor = false;
            // 
            // xrSummaryCr
            // 
            this.xrSummaryCr.LocationFloat = new DevExpress.Utils.PointFloat(451.0416F, 2.083336F);
            this.xrSummaryCr.Name = "xrSummaryCr";
            this.xrSummaryCr.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrSummaryCr.SizeF = new System.Drawing.SizeF(100F, 23F);
            this.xrSummaryCr.Text = "xrSummaryCr";
            // 
            // xrSummaryDr
            // 
            this.xrSummaryDr.LocationFloat = new DevExpress.Utils.PointFloat(302.0833F, 2.083336F);
            this.xrSummaryDr.Name = "xrSummaryDr";
            this.xrSummaryDr.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrSummaryDr.SizeF = new System.Drawing.SizeF(100F, 23F);
            this.xrSummaryDr.Text = "xrSummaryDr";
            // 
            // xrLine4
            // 
            this.xrLine4.LineStyle = DevExpress.Drawing.DXDashStyle.Dash;
            this.xrLine4.LocationFloat = new DevExpress.Utils.PointFloat(433.4583F, 100.125F);
            this.xrLine4.Name = "xrLine4";
            this.xrLine4.SizeF = new System.Drawing.SizeF(195.875F, 9.375F);
            // 
            // xrLine5
            // 
            this.xrLine5.LineStyle = DevExpress.Drawing.DXDashStyle.Dash;
            this.xrLine5.LocationFloat = new DevExpress.Utils.PointFloat(433.4583F, 134.0833F);
            this.xrLine5.Name = "xrLine5";
            this.xrLine5.SizeF = new System.Drawing.SizeF(195.875F, 9.375F);
            // 
            // calculatedField1
            // 
            this.calculatedField1.DataMember = "ViewTransactionPostCollectionBank";
            this.calculatedField1.Expression = "[CloseBal] +[Dr]";
            this.calculatedField1.Name = "calculatedField1";
            // 
            // paramCloseDb
            // 
            this.paramCloseDb.Name = "paramCloseDb";
            this.paramCloseDb.Type = typeof(decimal);
            this.paramCloseDb.Value = 0;
            this.paramCloseDb.Visible = false;
            // 
            // calFdMonth
            // 
            this.calFdMonth.DataMember = "ViewTransactionPostCollectionBank";
            this.calFdMonth.Expression = "GetDate([PaymentDate])";
            this.calFdMonth.Name = "calFdMonth";
            // 
            // paramAccount
            // 
            this.paramAccount.Name = "paramAccount";
            this.paramAccount.Value = "";
            this.paramAccount.Visible = false;
            // 
            // paramPeriod
            // 
            this.paramPeriod.Name = "paramPeriod";
            this.paramPeriod.Value = "";
            this.paramPeriod.Visible = false;
            // 
            // paramRecord
            // 
            this.paramRecord.Name = "paramRecord";
            this.paramRecord.Type = typeof(bool);
            this.paramRecord.Value = true;
            this.paramRecord.Visible = false;
            // 
            // paramPayDirect
            // 
            this.paramPayDirect.Name = "paramPayDirect";
            this.paramPayDirect.Type = typeof(bool);
            this.paramPayDirect.Value = true;
            this.paramPayDirect.Visible = false;
            // 
            // xrTranAccount
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.PageHeader,
            this.GroupHeader1,
            this.GroupFooter1});
            this.CalculatedFields.AddRange(new DevExpress.XtraReports.UI.CalculatedField[] {
            this.calculatedField1,
            this.calFdMonth});
            this.DataAdapter = this.viewTransactionPostCollectionBankTableAdapter;
            this.DataMember = "ViewTransactionPostCollectionBank";
            this.DataSource = this.dsTransaction1;
            this.FilterString = "[Period] Like Concat(\'%\', ?paramPeriod, \'%\') And [AccountNo] = ?paramAccount And " +
    "[IsRecordExit] = ?paramRecord And [IsPayDirect] = ?paramPayDirect";
            this.Margins = new DevExpress.Drawing.DXMargins(100, 99, 50, 100);
            this.Parameters.AddRange(new DevExpress.XtraReports.Parameters.Parameter[] {
            this.paramCloseDb,
            this.paramAccount,
            this.paramPeriod,
            this.paramRecord,
            this.paramPayDirect});
            this.Version = "11.2";
            this.BeforePrint += new DevExpress.XtraReports.UI.BeforePrintEventHandler(this.xrTranAccount_BeforePrint);
            ((System.ComponentModel.ISupportInitialize)(this.dsTransaction1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.DetailBand Detail;
        private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
        private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
        private BankReconciliation.Dataset.dsTransaction dsTransaction1;
        private BankReconciliation.Dataset.dsTransactionTableAdapters.ViewTransactionPostCollectionBankTableAdapter viewTransactionPostCollectionBankTableAdapter;
        private DevExpress.XtraReports.UI.PageHeaderBand PageHeader;
        private DevExpress.XtraReports.UI.GroupHeaderBand GroupHeader1;
        private DevExpress.XtraReports.UI.XRLabel xrLabel2;
        private DevExpress.XtraReports.UI.XRLabel xrLabel5;
        private DevExpress.XtraReports.UI.XRLabel xrLabel4;
        private DevExpress.XtraReports.UI.XRLabel xrLabel7;
        private DevExpress.XtraReports.UI.XRLabel xrLabel8;
        private DevExpress.XtraReports.UI.GroupFooterBand GroupFooter1;
        private DevExpress.XtraReports.UI.XRLabel xrLabel6;
        private DevExpress.XtraReports.UI.XRLabel xrLabel11;
        private DevExpress.XtraReports.UI.XRLabel xrLabel13;
        private DevExpress.XtraReports.UI.XRLabel xrLabel14;
        private DevExpress.XtraReports.UI.XRLabel xrLabel15;
        private DevExpress.XtraReports.UI.CalculatedField calculatedField1;
        private DevExpress.XtraReports.UI.XRLine xrLine1;
        private DevExpress.XtraReports.UI.XRLabel xrSummaryDr;
        private DevExpress.XtraReports.UI.XRLabel xrSummaryCr;
        private DevExpress.XtraReports.UI.XRLabel xrLabel16;
        private DevExpress.XtraReports.UI.XRLabel xrCloseBal;
        private DevExpress.XtraReports.Parameters.Parameter paramCloseDb;
        private DevExpress.XtraReports.UI.XRLabel xrAmt;
        private DevExpress.XtraReports.UI.XRLine xrLine3;
        private DevExpress.XtraReports.UI.XRLine xrLine2;
        private DevExpress.XtraReports.UI.XRLabel xrLabel3;
        private DevExpress.XtraReports.UI.XRLabel xrLabel9;
        private DevExpress.XtraReports.UI.XRLabel xrLabel10;
        private DevExpress.XtraReports.UI.XRLabel xrLabel17;
        private DevExpress.XtraReports.UI.XRLabel xrDiffAmt;
        private DevExpress.XtraReports.UI.CalculatedField calFdMonth;
        public DevExpress.XtraReports.UI.XRLabel xrLabel1;
        public DevExpress.XtraReports.UI.XRLabel xrLabel18;
        public DevExpress.XtraReports.Parameters.Parameter paramAccount;
        public DevExpress.XtraReports.Parameters.Parameter paramPeriod;
        private DevExpress.XtraReports.UI.XRLine xrLine5;
        private DevExpress.XtraReports.UI.XRLine xrLine4;
        private DevExpress.XtraReports.UI.XRLabel xrOpenBal;
        public DevExpress.XtraReports.Parameters.Parameter paramRecord;
        public DevExpress.XtraReports.Parameters.Parameter paramPayDirect;
    }
}
