namespace BankReconciliation.Forms
{
    partial class FrmRequest
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            DevExpress.XtraGrid.GridLevelNode gridLevelNode1 = new DevExpress.XtraGrid.GridLevelNode();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmRequest));
            this.gridView3 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colDate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ColPayerName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colAmount = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colBSIDs = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colBankS = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.viewPostingRequestBankBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.tblTransactionPostingRequestViewPostBankStatment = new BankReconciliation.tblTransactionPostingRequestViewPostBankStatment();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colBankName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colBankShortCode = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colBatchCode = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colBatchName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colOpeningBalance = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colClosingBalance = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colTotalCredit = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colTotalDebit = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colRequestedBy = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colRequestedDate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colDescription = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colIsApproved = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colApprovedBy = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colApprovedDate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridView2 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colBSDate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colCredit = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colBSID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colBankShort = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.panelContainer = new DevExpress.XtraEditors.PanelControl();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.label35 = new System.Windows.Forms.Label();
            this.btnAllocate = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.tsbNew = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbEdit = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbDelete = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbReload = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbClose = new System.Windows.Forms.ToolStripButton();
            this.viewPostingRequestBankTableAdapter = new BankReconciliation.tblTransactionPostingRequestViewPostBankStatmentTableAdapters.ViewPostingRequestBankTableAdapter();
            this.viewPostbankStatmentTableAdapter1 = new BankReconciliation.tblTransactionPostingRequestViewPostBankStatmentTableAdapters.ViewPostbankStatmentTableAdapter();
            this.postingRecordsListsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.viewPostingRequestBankViewPostbankStatmentBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.fillByToolStrip = new System.Windows.Forms.ToolStrip();
            this.fillByToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.HelpProviderHG = new System.Windows.Forms.HelpProvider();
            ((System.ComponentModel.ISupportInitialize)(this.gridView3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.viewPostingRequestBankBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tblTransactionPostingRequestViewPostBankStatment)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelContainer)).BeginInit();
            this.panelContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.toolStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.postingRecordsListsBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.viewPostingRequestBankViewPostbankStatmentBindingSource)).BeginInit();
            this.fillByToolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // gridView3
            // 
            this.gridView3.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colDate,
            this.ColPayerName,
            this.colAmount,
            this.colBSIDs,
            this.colBankS,
            this.gridColumn2});
            this.gridView3.GridControl = this.gridControl1;
            this.gridView3.Name = "gridView3";
            this.gridView3.OptionsBehavior.Editable = false;
            this.gridView3.OptionsView.ColumnAutoWidth = false;
            this.gridView3.OptionsView.ShowGroupPanel = false;
            // 
            // colDate
            // 
            this.colDate.Caption = "Date";
            this.colDate.DisplayFormat.FormatString = "d";
            this.colDate.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.colDate.FieldName = "BSDate";
            this.colDate.Name = "colDate";
            this.colDate.Visible = true;
            this.colDate.VisibleIndex = 0;
            // 
            // ColPayerName
            // 
            this.ColPayerName.Caption = "PayerName";
            this.ColPayerName.FieldName = "PayerName";
            this.ColPayerName.Name = "ColPayerName";
            this.ColPayerName.Visible = true;
            this.ColPayerName.VisibleIndex = 2;
            // 
            // colAmount
            // 
            this.colAmount.Caption = "Amount";
            this.colAmount.DisplayFormat.FormatString = "n2";
            this.colAmount.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colAmount.FieldName = "Credit";
            this.colAmount.Name = "colAmount";
            this.colAmount.Visible = true;
            this.colAmount.VisibleIndex = 1;
            // 
            // colBSIDs
            // 
            this.colBSIDs.Caption = "BSID";
            this.colBSIDs.FieldName = "BSID";
            this.colBSIDs.Name = "colBSIDs";
            // 
            // colBankS
            // 
            this.colBankS.Caption = "Bank Short Code";
            this.colBankS.FieldName = "BankShortCode";
            this.colBankS.Name = "colBankS";
            // 
            // gridColumn2
            // 
            this.gridColumn2.Caption = "FinancialperiodID";
            this.gridColumn2.FieldName = "FinancialperiodID";
            this.gridColumn2.Name = "gridColumn2";
            // 
            // gridControl1
            // 
            this.gridControl1.DataSource = this.viewPostingRequestBankBindingSource;
            this.gridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.HelpProviderHG.SetHelpKeyword(this.gridControl1, "FrmRequest.htm#gridControl1");
            this.HelpProviderHG.SetHelpNavigator(this.gridControl1, System.Windows.Forms.HelpNavigator.Topic);
            gridLevelNode1.LevelTemplate = this.gridView3;
            gridLevelNode1.RelationName = "ViewPostingRequestBank_ViewPostbankStatment";
            this.gridControl1.LevelTree.Nodes.AddRange(new DevExpress.XtraGrid.GridLevelNode[] {
            gridLevelNode1});
            this.gridControl1.Location = new System.Drawing.Point(3, 17);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.Name = "gridControl1";
            this.HelpProviderHG.SetShowHelp(this.gridControl1, true);
            this.gridControl1.Size = new System.Drawing.Size(826, 391);
            this.gridControl1.TabIndex = 0;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1,
            this.gridView2,
            this.gridView3});
            // 
            // viewPostingRequestBankBindingSource
            // 
            this.viewPostingRequestBankBindingSource.DataMember = "ViewPostingRequestBank";
            this.viewPostingRequestBankBindingSource.DataSource = this.tblTransactionPostingRequestViewPostBankStatment;
            // 
            // tblTransactionPostingRequestViewPostBankStatment
            // 
            this.tblTransactionPostingRequestViewPostBankStatment.DataSetName = "tblTransactionPostingRequestViewPostBankStatment";
            this.tblTransactionPostingRequestViewPostBankStatment.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // gridView1
            // 
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colBankName,
            this.colBankShortCode,
            this.colBatchCode,
            this.colBatchName,
            this.colOpeningBalance,
            this.colClosingBalance,
            this.colTotalCredit,
            this.colTotalDebit,
            this.colRequestedBy,
            this.colRequestedDate,
            this.colDescription,
            this.colIsApproved,
            this.colApprovedBy,
            this.colApprovedDate});
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsBehavior.Editable = false;
            this.gridView1.OptionsSelection.MultiSelect = true;
            this.gridView1.OptionsView.ShowGroupPanel = false;
            // 
            // colBankName
            // 
            this.colBankName.FieldName = "BankName";
            this.colBankName.Name = "colBankName";
            this.colBankName.OptionsColumn.AllowEdit = false;
            this.colBankName.Visible = true;
            this.colBankName.VisibleIndex = 0;
            // 
            // colBankShortCode
            // 
            this.colBankShortCode.FieldName = "BankShortCode";
            this.colBankShortCode.Name = "colBankShortCode";
            // 
            // colBatchCode
            // 
            this.colBatchCode.FieldName = "BatchCode";
            this.colBatchCode.Name = "colBatchCode";
            // 
            // colBatchName
            // 
            this.colBatchName.FieldName = "BatchName";
            this.colBatchName.Name = "colBatchName";
            this.colBatchName.Visible = true;
            this.colBatchName.VisibleIndex = 1;
            // 
            // colOpeningBalance
            // 
            this.colOpeningBalance.DisplayFormat.FormatString = "n2";
            this.colOpeningBalance.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colOpeningBalance.FieldName = "OpeningBalance";
            this.colOpeningBalance.Name = "colOpeningBalance";
            this.colOpeningBalance.OptionsColumn.AllowEdit = false;
            this.colOpeningBalance.Visible = true;
            this.colOpeningBalance.VisibleIndex = 3;
            // 
            // colClosingBalance
            // 
            this.colClosingBalance.DisplayFormat.FormatString = "n2";
            this.colClosingBalance.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colClosingBalance.FieldName = "ClosingBalance";
            this.colClosingBalance.Name = "colClosingBalance";
            this.colClosingBalance.OptionsColumn.AllowEdit = false;
            this.colClosingBalance.Visible = true;
            this.colClosingBalance.VisibleIndex = 4;
            // 
            // colTotalCredit
            // 
            this.colTotalCredit.DisplayFormat.FormatString = "n2";
            this.colTotalCredit.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colTotalCredit.FieldName = "TotalCredit";
            this.colTotalCredit.Name = "colTotalCredit";
            this.colTotalCredit.OptionsColumn.AllowEdit = false;
            this.colTotalCredit.Visible = true;
            this.colTotalCredit.VisibleIndex = 5;
            // 
            // colTotalDebit
            // 
            this.colTotalDebit.DisplayFormat.FormatString = "n2";
            this.colTotalDebit.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colTotalDebit.FieldName = "TotalDebit";
            this.colTotalDebit.Name = "colTotalDebit";
            this.colTotalDebit.OptionsColumn.AllowEdit = false;
            this.colTotalDebit.Visible = true;
            this.colTotalDebit.VisibleIndex = 6;
            // 
            // colRequestedBy
            // 
            this.colRequestedBy.FieldName = "RequestedBy";
            this.colRequestedBy.Name = "colRequestedBy";
            // 
            // colRequestedDate
            // 
            this.colRequestedDate.FieldName = "RequestedDate";
            this.colRequestedDate.Name = "colRequestedDate";
            // 
            // colDescription
            // 
            this.colDescription.FieldName = "Description";
            this.colDescription.Name = "colDescription";
            this.colDescription.Visible = true;
            this.colDescription.VisibleIndex = 2;
            // 
            // colIsApproved
            // 
            this.colIsApproved.Caption = "Approved";
            this.colIsApproved.FieldName = "IsApproved";
            this.colIsApproved.Name = "colIsApproved";
            this.colIsApproved.Visible = true;
            this.colIsApproved.VisibleIndex = 7;
            // 
            // colApprovedBy
            // 
            this.colApprovedBy.FieldName = "ApprovedBy";
            this.colApprovedBy.Name = "colApprovedBy";
            // 
            // colApprovedDate
            // 
            this.colApprovedDate.FieldName = "ApprovedDate";
            this.colApprovedDate.Name = "colApprovedDate";
            // 
            // gridView2
            // 
            this.gridView2.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colBSDate,
            this.colCredit,
            this.colBSID,
            this.colBankShort,
            this.gridColumn1});
            this.gridView2.GridControl = this.gridControl1;
            this.gridView2.Name = "gridView2";
            this.gridView2.OptionsBehavior.Editable = false;
            this.gridView2.OptionsView.ShowGroupPanel = false;
            // 
            // colBSDate
            // 
            this.colBSDate.Caption = "Date";
            this.colBSDate.DisplayFormat.FormatString = "d";
            this.colBSDate.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.colBSDate.FieldName = "BSDate";
            this.colBSDate.Name = "colBSDate";
            // 
            // colCredit
            // 
            this.colCredit.Caption = "Amunt";
            this.colCredit.DisplayFormat.FormatString = "n2";
            this.colCredit.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colCredit.FieldName = "Credit";
            this.colCredit.Name = "colCredit";
            // 
            // colBSID
            // 
            this.colBSID.Caption = "BSID";
            this.colBSID.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colBSID.FieldName = "BSID";
            this.colBSID.Name = "colBSID";
            // 
            // colBankShort
            // 
            this.colBankShort.Caption = "Bank Short Code";
            this.colBankShort.FieldName = "BankShortCode";
            this.colBankShort.Name = "colBankShort";
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "gridColumn1";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 0;
            // 
            // panelContainer
            // 
            this.panelContainer.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(100)))));
            this.panelContainer.Appearance.Options.UseBackColor = true;
            this.panelContainer.Controls.Add(this.groupControl1);
            this.panelContainer.Controls.Add(this.toolStrip);
            this.panelContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelContainer.Location = new System.Drawing.Point(0, 0);
            this.panelContainer.LookAndFeel.UseDefaultLookAndFeel = false;
            this.panelContainer.LookAndFeel.UseWindowsXPTheme = true;
            this.panelContainer.Name = "panelContainer";
            this.panelContainer.Size = new System.Drawing.Size(975, 610);
            this.panelContainer.TabIndex = 0;
            // 
            // groupControl1
            // 
            this.groupControl1.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.groupControl1.Appearance.Options.UseBackColor = true;
            this.groupControl1.Controls.Add(this.label35);
            this.groupControl1.Controls.Add(this.btnAllocate);
            this.groupControl1.Controls.Add(this.groupBox1);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupControl1.Location = new System.Drawing.Point(4, 41);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(947, 564);
            this.groupControl1.TabIndex = 7;
            this.groupControl1.Text = "Closing Bank Transaction Posting Request";
            // 
            // label35
            // 
            this.label35.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label35.Location = new System.Drawing.Point(860, 155);
            this.label35.Name = "label35";
            this.label35.Size = new System.Drawing.Size(81, 21);
            this.label35.TabIndex = 453;
            this.label35.Text = "&Close Period";
            this.label35.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnAllocate
            // 
            this.btnAllocate.Cursor = System.Windows.Forms.Cursors.Hand;
            this.HelpProviderHG.SetHelpKeyword(this.btnAllocate, "FrmRequest.htm#btnAllocate");
            this.HelpProviderHG.SetHelpNavigator(this.btnAllocate, System.Windows.Forms.HelpNavigator.Topic);
            this.btnAllocate.Location = new System.Drawing.Point(863, 115);
            this.btnAllocate.Name = "btnAllocate";
            this.HelpProviderHG.SetShowHelp(this.btnAllocate, true);
            this.btnAllocate.Size = new System.Drawing.Size(67, 37);
            this.btnAllocate.TabIndex = 452;
            this.btnAllocate.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.gridControl1);
            this.groupBox1.Location = new System.Drawing.Point(8, 72);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(832, 411);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // toolStrip
            // 
            this.toolStrip.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbNew,
            this.toolStripSeparator1,
            this.tsbEdit,
            this.toolStripSeparator2,
            this.tsbDelete,
            this.toolStripSeparator3,
            this.tsbReload,
            this.toolStripSeparator4,
            this.tsbClose});
            this.toolStrip.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.toolStrip.Location = new System.Drawing.Point(4, 4);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip.Size = new System.Drawing.Size(967, 37);
            this.toolStrip.TabIndex = 6;
            this.toolStrip.Text = "toolStrip1";
            // 
            // tsbNew
            // 
            this.tsbNew.Image = ((System.Drawing.Image)(resources.GetObject("tsbNew.Image")));
            this.tsbNew.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbNew.Name = "tsbNew";
            this.tsbNew.Size = new System.Drawing.Size(37, 34);
            this.tsbNew.Tag = "001";
            this.tsbNew.Text = "New";
            this.tsbNew.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tsbNew.ToolTipText = "Add New Street Group";
            this.tsbNew.Visible = false;
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 37);
            this.toolStripSeparator1.Visible = false;
            // 
            // tsbEdit
            // 
            this.tsbEdit.Image = ((System.Drawing.Image)(resources.GetObject("tsbEdit.Image")));
            this.tsbEdit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbEdit.Name = "tsbEdit";
            this.tsbEdit.Size = new System.Drawing.Size(35, 34);
            this.tsbEdit.Tag = "002";
            this.tsbEdit.Text = "Edit";
            this.tsbEdit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tsbEdit.ToolTipText = "Modify Selected Street Group";
            this.tsbEdit.Visible = false;
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 37);
            this.toolStripSeparator2.Visible = false;
            // 
            // tsbDelete
            // 
            this.tsbDelete.Image = ((System.Drawing.Image)(resources.GetObject("tsbDelete.Image")));
            this.tsbDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbDelete.Name = "tsbDelete";
            this.tsbDelete.Size = new System.Drawing.Size(54, 34);
            this.tsbDelete.Tag = "003";
            this.tsbDelete.Text = "Disable";
            this.tsbDelete.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tsbDelete.ToolTipText = "Disable";
            this.tsbDelete.Visible = false;
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 37);
            this.toolStripSeparator3.Visible = false;
            // 
            // tsbReload
            // 
            this.tsbReload.Image = ((System.Drawing.Image)(resources.GetObject("tsbReload.Image")));
            this.tsbReload.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbReload.Name = "tsbReload";
            this.tsbReload.Size = new System.Drawing.Size(53, 34);
            this.tsbReload.Tag = "004";
            this.tsbReload.Text = "Reload";
            this.tsbReload.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tsbReload.ToolTipText = "Reload / Refresh Data Grid View";
            this.tsbReload.Visible = false;
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 37);
            this.toolStripSeparator4.Visible = false;
            // 
            // tsbClose
            // 
            this.tsbClose.Image = ((System.Drawing.Image)(resources.GetObject("tsbClose.Image")));
            this.tsbClose.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbClose.Name = "tsbClose";
            this.tsbClose.Size = new System.Drawing.Size(43, 34);
            this.tsbClose.Tag = "005";
            this.tsbClose.Text = "Close";
            this.tsbClose.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tsbClose.ToolTipText = "Close Form";
            // 
            // viewPostingRequestBankTableAdapter
            // 
            this.viewPostingRequestBankTableAdapter.ClearBeforeFill = true;
            // 
            // viewPostbankStatmentTableAdapter1
            // 
            this.viewPostbankStatmentTableAdapter1.ClearBeforeFill = true;
            // 
            // postingRecordsListsBindingSource
            // 
            this.postingRecordsListsBindingSource.DataSource = this.viewPostingRequestBankViewPostbankStatmentBindingSource;
            this.postingRecordsListsBindingSource.CurrentChanged += new System.EventHandler(this.postingRecordsListsBindingSource_CurrentChanged);
            // 
            // viewPostingRequestBankViewPostbankStatmentBindingSource
            // 
            this.viewPostingRequestBankViewPostbankStatmentBindingSource.DataMember = "ViewPostingRequestBank_ViewPostbankStatment";
            this.viewPostingRequestBankViewPostbankStatmentBindingSource.DataSource = this.viewPostingRequestBankBindingSource;
            // 
            // fillByToolStrip
            // 
            this.fillByToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fillByToolStripButton});
            this.fillByToolStrip.Location = new System.Drawing.Point(0, 0);
            this.fillByToolStrip.Name = "fillByToolStrip";
            this.fillByToolStrip.Size = new System.Drawing.Size(975, 25);
            this.fillByToolStrip.TabIndex = 1;
            this.fillByToolStrip.Text = "fillByToolStrip";
            // 
            // fillByToolStripButton
            // 
            this.fillByToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.fillByToolStripButton.Name = "fillByToolStripButton";
            this.fillByToolStripButton.Size = new System.Drawing.Size(39, 22);
            this.fillByToolStripButton.Text = "FillBy";
            this.fillByToolStripButton.Click += new System.EventHandler(this.fillByToolStripButton_Click);
            // 
            // HelpProviderHG
            // 
            this.HelpProviderHG.HelpNamespace = "Bank_Reconciliation.chm";
            // 
            // FrmRequest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(975, 610);
            this.Controls.Add(this.fillByToolStrip);
            this.Controls.Add(this.panelContainer);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.HelpProviderHG.SetHelpKeyword(this, "FrmRequest.htm");
            this.HelpProviderHG.SetHelpNavigator(this, System.Windows.Forms.HelpNavigator.Topic);
            this.Name = "FrmRequest";
            this.HelpProviderHG.SetShowHelp(this, true);
            this.Text = "FrmRequest";
            this.Load += new System.EventHandler(this.FrmRequest_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gridView3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.viewPostingRequestBankBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tblTransactionPostingRequestViewPostBankStatment)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelContainer)).EndInit();
            this.panelContainer.ResumeLayout(false);
            this.panelContainer.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.postingRecordsListsBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.viewPostingRequestBankViewPostbankStatmentBindingSource)).EndInit();
            this.fillByToolStrip.ResumeLayout(false);
            this.fillByToolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public DevExpress.XtraEditors.PanelControl panelContainer;
        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripButton tsbNew;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton tsbEdit;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton tsbDelete;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton tsbReload;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripButton tsbClose;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private System.Windows.Forms.GroupBox groupBox1;
        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private tblTransactionPostingRequestViewPostBankStatment tblTransactionPostingRequestViewPostBankStatment;
        private System.Windows.Forms.BindingSource viewPostingRequestBankBindingSource;
        private tblTransactionPostingRequestViewPostBankStatmentTableAdapters.ViewPostingRequestBankTableAdapter viewPostingRequestBankTableAdapter;
        private DevExpress.XtraGrid.Columns.GridColumn colBankName;
        private DevExpress.XtraGrid.Columns.GridColumn colBankShortCode;
        private DevExpress.XtraGrid.Columns.GridColumn colBatchCode;
        private DevExpress.XtraGrid.Columns.GridColumn colBatchName;
        private DevExpress.XtraGrid.Columns.GridColumn colOpeningBalance;
        private DevExpress.XtraGrid.Columns.GridColumn colClosingBalance;
        private DevExpress.XtraGrid.Columns.GridColumn colTotalCredit;
        private DevExpress.XtraGrid.Columns.GridColumn colTotalDebit;
        private DevExpress.XtraGrid.Columns.GridColumn colRequestedBy;
        private DevExpress.XtraGrid.Columns.GridColumn colRequestedDate;
        private DevExpress.XtraGrid.Columns.GridColumn colIsApproved;
        private DevExpress.XtraGrid.Columns.GridColumn colApprovedBy;
        private DevExpress.XtraGrid.Columns.GridColumn colApprovedDate;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView2;
        private tblTransactionPostingRequestViewPostBankStatmentTableAdapters.ViewPostbankStatmentTableAdapter viewPostbankStatmentTableAdapter1;
        private System.Windows.Forms.Label label35;
        private System.Windows.Forms.Button btnAllocate;
        private System.Windows.Forms.BindingSource postingRecordsListsBindingSource;
        private System.Windows.Forms.BindingSource viewPostingRequestBankViewPostbankStatmentBindingSource;
        private System.Windows.Forms.ToolStrip fillByToolStrip;
        private System.Windows.Forms.ToolStripButton fillByToolStripButton;
        private DevExpress.XtraGrid.Columns.GridColumn colBSDate;
        private DevExpress.XtraGrid.Columns.GridColumn colCredit;
        private DevExpress.XtraGrid.Columns.GridColumn colBSID;
        private DevExpress.XtraGrid.Columns.GridColumn colBankShort;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView3;
        private DevExpress.XtraGrid.Columns.GridColumn colDate;
        private DevExpress.XtraGrid.Columns.GridColumn colAmount;
        private DevExpress.XtraGrid.Columns.GridColumn colBSIDs;
        private DevExpress.XtraGrid.Columns.GridColumn colBankS;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.Columns.GridColumn colDescription;
        private System.Windows.Forms.HelpProvider HelpProviderHG;
        private DevExpress.XtraGrid.Columns.GridColumn ColPayerName;
    }
}