namespace Control_Panel.Forms
{
    partial class FrmExchange
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
            this.gcExchange = new DevExpress.XtraGrid.GridControl();
            this.tblExchangeRateBindingSource = new System.Windows.Forms.BindingSource();
            this.dsBank = new Control_Panel.DataSet.dsBank();
            this.gvExchange = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colRate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colCurrencyID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repCurrency = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.tblCurrency1BindingSource = new System.Windows.Forms.BindingSource();
            this.tblCurrencyBindingSource = new System.Windows.Forms.BindingSource();
            this.tblExchangeRateTableAdapter = new Control_Panel.DataSet.dsBankTableAdapters.tblExchangeRateTableAdapter();
            this.tblCurrencyTableAdapter = new Control_Panel.DataSet.dsBankTableAdapters.tblCurrencyTableAdapter();
            this.tblCurrency1TableAdapter = new Control_Panel.DataSet.dsBankTableAdapters.tblCurrency1TableAdapter();
            this.HelpProviderHG = new System.Windows.Forms.HelpProvider();
            ((System.ComponentModel.ISupportInitialize)(this.gcExchange)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tblExchangeRateBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsBank)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvExchange)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repCurrency)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tblCurrency1BindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tblCurrencyBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // gcExchange
            // 
            this.gcExchange.DataSource = this.tblExchangeRateBindingSource;
            this.gcExchange.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcExchange.EmbeddedNavigator.ButtonClick += new DevExpress.XtraEditors.NavigatorButtonClickEventHandler(this.gcExchange_EmbeddedNavigator_ButtonClick);
            this.HelpProviderHG.SetHelpKeyword(this.gcExchange, "FrmExchange.htm#gcExchange");
            this.HelpProviderHG.SetHelpNavigator(this.gcExchange, System.Windows.Forms.HelpNavigator.Topic);
            this.gcExchange.Location = new System.Drawing.Point(0, 0);
            this.gcExchange.MainView = this.gvExchange;
            this.gcExchange.Name = "gcExchange";
            this.gcExchange.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repCurrency});
            this.HelpProviderHG.SetShowHelp(this.gcExchange, true);
            this.gcExchange.Size = new System.Drawing.Size(333, 295);
            this.gcExchange.TabIndex = 0;
            this.gcExchange.UseEmbeddedNavigator = true;
            this.gcExchange.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvExchange});
            // 
            // tblExchangeRateBindingSource
            // 
            this.tblExchangeRateBindingSource.DataMember = "tblExchangeRate";
            this.tblExchangeRateBindingSource.DataSource = this.dsBank;
            // 
            // dsBank
            // 
            this.dsBank.DataSetName = "dsBank";
            this.dsBank.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // gvExchange
            // 
            this.gvExchange.Appearance.ColumnFilterButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(131)))), ((int)(((byte)(153)))), ((int)(((byte)(177)))));
            this.gvExchange.Appearance.ColumnFilterButton.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(173)))), ((int)(((byte)(197)))));
            this.gvExchange.Appearance.ColumnFilterButton.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(131)))), ((int)(((byte)(153)))), ((int)(((byte)(177)))));
            this.gvExchange.Appearance.ColumnFilterButton.ForeColor = System.Drawing.Color.Gray;
            this.gvExchange.Appearance.ColumnFilterButton.Options.UseBackColor = true;
            this.gvExchange.Appearance.ColumnFilterButton.Options.UseBorderColor = true;
            this.gvExchange.Appearance.ColumnFilterButton.Options.UseForeColor = true;
            this.gvExchange.Appearance.ColumnFilterButtonActive.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(173)))), ((int)(((byte)(197)))));
            this.gvExchange.Appearance.ColumnFilterButtonActive.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(177)))), ((int)(((byte)(193)))), ((int)(((byte)(211)))));
            this.gvExchange.Appearance.ColumnFilterButtonActive.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(173)))), ((int)(((byte)(197)))));
            this.gvExchange.Appearance.ColumnFilterButtonActive.ForeColor = System.Drawing.Color.Blue;
            this.gvExchange.Appearance.ColumnFilterButtonActive.Options.UseBackColor = true;
            this.gvExchange.Appearance.ColumnFilterButtonActive.Options.UseBorderColor = true;
            this.gvExchange.Appearance.ColumnFilterButtonActive.Options.UseForeColor = true;
            this.gvExchange.Appearance.Empty.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(173)))), ((int)(((byte)(197)))));
            this.gvExchange.Appearance.Empty.Options.UseBackColor = true;
            this.gvExchange.Appearance.EvenRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(131)))), ((int)(((byte)(153)))), ((int)(((byte)(177)))));
            this.gvExchange.Appearance.EvenRow.BackColor2 = System.Drawing.Color.GhostWhite;
            this.gvExchange.Appearance.EvenRow.ForeColor = System.Drawing.Color.Black;
            this.gvExchange.Appearance.EvenRow.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal;
            this.gvExchange.Appearance.EvenRow.Options.UseBackColor = true;
            this.gvExchange.Appearance.EvenRow.Options.UseForeColor = true;
            this.gvExchange.Appearance.FilterCloseButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(212)))), ((int)(((byte)(208)))), ((int)(((byte)(200)))));
            this.gvExchange.Appearance.FilterCloseButton.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.gvExchange.Appearance.FilterCloseButton.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(212)))), ((int)(((byte)(208)))), ((int)(((byte)(200)))));
            this.gvExchange.Appearance.FilterCloseButton.ForeColor = System.Drawing.Color.Black;
            this.gvExchange.Appearance.FilterCloseButton.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal;
            this.gvExchange.Appearance.FilterCloseButton.Options.UseBackColor = true;
            this.gvExchange.Appearance.FilterCloseButton.Options.UseBorderColor = true;
            this.gvExchange.Appearance.FilterCloseButton.Options.UseForeColor = true;
            this.gvExchange.Appearance.FilterPanel.BackColor = System.Drawing.Color.Black;
            this.gvExchange.Appearance.FilterPanel.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(212)))), ((int)(((byte)(208)))), ((int)(((byte)(200)))));
            this.gvExchange.Appearance.FilterPanel.ForeColor = System.Drawing.Color.White;
            this.gvExchange.Appearance.FilterPanel.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal;
            this.gvExchange.Appearance.FilterPanel.Options.UseBackColor = true;
            this.gvExchange.Appearance.FilterPanel.Options.UseForeColor = true;
            this.gvExchange.Appearance.FixedLine.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(31)))), ((int)(((byte)(55)))));
            this.gvExchange.Appearance.FixedLine.Options.UseBackColor = true;
            this.gvExchange.Appearance.FocusedRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(79)))), ((int)(((byte)(101)))), ((int)(((byte)(125)))));
            this.gvExchange.Appearance.FocusedRow.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(129)))), ((int)(((byte)(151)))), ((int)(((byte)(175)))));
            this.gvExchange.Appearance.FocusedRow.ForeColor = System.Drawing.Color.White;
            this.gvExchange.Appearance.FocusedRow.Options.UseBackColor = true;
            this.gvExchange.Appearance.FocusedRow.Options.UseForeColor = true;
            this.gvExchange.Appearance.FooterPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(131)))), ((int)(((byte)(153)))), ((int)(((byte)(177)))));
            this.gvExchange.Appearance.FooterPanel.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(131)))), ((int)(((byte)(153)))), ((int)(((byte)(177)))));
            this.gvExchange.Appearance.FooterPanel.ForeColor = System.Drawing.Color.Black;
            this.gvExchange.Appearance.FooterPanel.Options.UseBackColor = true;
            this.gvExchange.Appearance.FooterPanel.Options.UseBorderColor = true;
            this.gvExchange.Appearance.FooterPanel.Options.UseForeColor = true;
            this.gvExchange.Appearance.GroupButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(131)))), ((int)(((byte)(153)))), ((int)(((byte)(177)))));
            this.gvExchange.Appearance.GroupButton.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(131)))), ((int)(((byte)(153)))), ((int)(((byte)(177)))));
            this.gvExchange.Appearance.GroupButton.ForeColor = System.Drawing.Color.Black;
            this.gvExchange.Appearance.GroupButton.Options.UseBackColor = true;
            this.gvExchange.Appearance.GroupButton.Options.UseBorderColor = true;
            this.gvExchange.Appearance.GroupButton.Options.UseForeColor = true;
            this.gvExchange.Appearance.GroupFooter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(163)))), ((int)(((byte)(187)))));
            this.gvExchange.Appearance.GroupFooter.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(163)))), ((int)(((byte)(187)))));
            this.gvExchange.Appearance.GroupFooter.ForeColor = System.Drawing.Color.Black;
            this.gvExchange.Appearance.GroupFooter.Options.UseBackColor = true;
            this.gvExchange.Appearance.GroupFooter.Options.UseBorderColor = true;
            this.gvExchange.Appearance.GroupFooter.Options.UseForeColor = true;
            this.gvExchange.Appearance.GroupPanel.BackColor = System.Drawing.Color.Black;
            this.gvExchange.Appearance.GroupPanel.BackColor2 = System.Drawing.Color.White;
            this.gvExchange.Appearance.GroupPanel.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.gvExchange.Appearance.GroupPanel.ForeColor = System.Drawing.Color.White;
            this.gvExchange.Appearance.GroupPanel.Options.UseBackColor = true;
            this.gvExchange.Appearance.GroupPanel.Options.UseFont = true;
            this.gvExchange.Appearance.GroupPanel.Options.UseForeColor = true;
            this.gvExchange.Appearance.GroupRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(79)))), ((int)(((byte)(101)))), ((int)(((byte)(125)))));
            this.gvExchange.Appearance.GroupRow.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(193)))), ((int)(((byte)(204)))), ((int)(((byte)(217)))));
            this.gvExchange.Appearance.GroupRow.Options.UseBackColor = true;
            this.gvExchange.Appearance.GroupRow.Options.UseForeColor = true;
            this.gvExchange.Appearance.HeaderPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(131)))), ((int)(((byte)(153)))), ((int)(((byte)(177)))));
            this.gvExchange.Appearance.HeaderPanel.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(131)))), ((int)(((byte)(153)))), ((int)(((byte)(177)))));
            this.gvExchange.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.gvExchange.Appearance.HeaderPanel.ForeColor = System.Drawing.Color.Black;
            this.gvExchange.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.gvExchange.Appearance.HeaderPanel.Options.UseBorderColor = true;
            this.gvExchange.Appearance.HeaderPanel.Options.UseFont = true;
            this.gvExchange.Appearance.HeaderPanel.Options.UseForeColor = true;
            this.gvExchange.Appearance.HideSelectionRow.BackColor = System.Drawing.Color.Gray;
            this.gvExchange.Appearance.HideSelectionRow.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(212)))), ((int)(((byte)(208)))), ((int)(((byte)(200)))));
            this.gvExchange.Appearance.HideSelectionRow.Options.UseBackColor = true;
            this.gvExchange.Appearance.HideSelectionRow.Options.UseForeColor = true;
            this.gvExchange.Appearance.HorzLine.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(131)))), ((int)(((byte)(153)))), ((int)(((byte)(177)))));
            this.gvExchange.Appearance.HorzLine.Options.UseBackColor = true;
            this.gvExchange.Appearance.OddRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(193)))), ((int)(((byte)(204)))), ((int)(((byte)(217)))));
            this.gvExchange.Appearance.OddRow.BackColor2 = System.Drawing.Color.White;
            this.gvExchange.Appearance.OddRow.ForeColor = System.Drawing.Color.Black;
            this.gvExchange.Appearance.OddRow.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.BackwardDiagonal;
            this.gvExchange.Appearance.OddRow.Options.UseBackColor = true;
            this.gvExchange.Appearance.OddRow.Options.UseForeColor = true;
            this.gvExchange.Appearance.Preview.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(213)))), ((int)(((byte)(237)))));
            this.gvExchange.Appearance.Preview.BackColor2 = System.Drawing.Color.White;
            this.gvExchange.Appearance.Preview.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(79)))), ((int)(((byte)(101)))), ((int)(((byte)(125)))));
            this.gvExchange.Appearance.Preview.Options.UseBackColor = true;
            this.gvExchange.Appearance.Preview.Options.UseForeColor = true;
            this.gvExchange.Appearance.Row.BackColor = System.Drawing.Color.White;
            this.gvExchange.Appearance.Row.ForeColor = System.Drawing.Color.Black;
            this.gvExchange.Appearance.Row.Options.UseBackColor = true;
            this.gvExchange.Appearance.Row.Options.UseForeColor = true;
            this.gvExchange.Appearance.RowSeparator.BackColor = System.Drawing.Color.White;
            this.gvExchange.Appearance.RowSeparator.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(173)))), ((int)(((byte)(197)))));
            this.gvExchange.Appearance.RowSeparator.Options.UseBackColor = true;
            this.gvExchange.Appearance.SelectedRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(89)))), ((int)(((byte)(111)))), ((int)(((byte)(135)))));
            this.gvExchange.Appearance.SelectedRow.ForeColor = System.Drawing.Color.White;
            this.gvExchange.Appearance.SelectedRow.Options.UseBackColor = true;
            this.gvExchange.Appearance.SelectedRow.Options.UseForeColor = true;
            this.gvExchange.Appearance.VertLine.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(131)))), ((int)(((byte)(153)))), ((int)(((byte)(177)))));
            this.gvExchange.Appearance.VertLine.Options.UseBackColor = true;
            this.gvExchange.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colRate,
            this.colCurrencyID});
            this.gvExchange.GridControl = this.gcExchange;
            this.gvExchange.Name = "gvExchange";
            this.gvExchange.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.True;
            this.gvExchange.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.True;
            this.gvExchange.OptionsView.EnableAppearanceEvenRow = true;
            this.gvExchange.OptionsView.EnableAppearanceOddRow = true;
            this.gvExchange.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.Top;
            this.gvExchange.OptionsView.ShowGroupPanel = false;
            this.gvExchange.PaintStyleName = "WindowsXP";
            // 
            // colRate
            // 
            this.colRate.Caption = "EXchange Rate";
            this.colRate.FieldName = "Rate";
            this.colRate.Name = "colRate";
            this.colRate.Visible = true;
            this.colRate.VisibleIndex = 1;
            // 
            // colCurrencyID
            // 
            this.colCurrencyID.Caption = "Currency Name";
            this.colCurrencyID.ColumnEdit = this.repCurrency;
            this.colCurrencyID.FieldName = "CurrencyID";
            this.colCurrencyID.Name = "colCurrencyID";
            this.colCurrencyID.Visible = true;
            this.colCurrencyID.VisibleIndex = 0;
            // 
            // repCurrency
            // 
            this.repCurrency.AutoHeight = false;
            this.repCurrency.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repCurrency.DataSource = this.tblCurrency1BindingSource;
            this.repCurrency.DisplayMember = "CurrencyName";
            this.repCurrency.Name = "repCurrency";
            this.repCurrency.ValueMember = "CurrencyID";
            // 
            // tblCurrency1BindingSource
            // 
            this.tblCurrency1BindingSource.DataMember = "tblCurrency1";
            this.tblCurrency1BindingSource.DataSource = this.dsBank;
            // 
            // tblCurrencyBindingSource
            // 
            this.tblCurrencyBindingSource.DataMember = "tblCurrency";
            this.tblCurrencyBindingSource.DataSource = this.dsBank;
            // 
            // tblExchangeRateTableAdapter
            // 
            this.tblExchangeRateTableAdapter.ClearBeforeFill = true;
            // 
            // tblCurrencyTableAdapter
            // 
            this.tblCurrencyTableAdapter.ClearBeforeFill = true;
            // 
            // tblCurrency1TableAdapter
            // 
            this.tblCurrency1TableAdapter.ClearBeforeFill = true;
            // 
            // HelpProviderHG
            // 
            this.HelpProviderHG.HelpNamespace = "Bank_Reconciliation.chm";
            // 
            // FrmExchange
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(333, 295);
            this.Controls.Add(this.gcExchange);
            this.HelpProviderHG.SetHelpKeyword(this, "FrmExchange.htm");
            this.HelpProviderHG.SetHelpNavigator(this, System.Windows.Forms.HelpNavigator.Topic);
            this.Name = "FrmExchange";
            this.HelpProviderHG.SetShowHelp(this, true);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Exchange Rate Setting";
            this.Load += new System.EventHandler(this.FrmExchange_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gcExchange)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tblExchangeRateBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsBank)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvExchange)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repCurrency)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tblCurrency1BindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tblCurrencyBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.GridControl gcExchange;
        private DevExpress.XtraGrid.Views.Grid.GridView gvExchange;
        private Control_Panel.DataSet.dsBank dsBank;
        private System.Windows.Forms.BindingSource tblExchangeRateBindingSource;
        private Control_Panel.DataSet.dsBankTableAdapters.tblExchangeRateTableAdapter tblExchangeRateTableAdapter;
        private DevExpress.XtraGrid.Columns.GridColumn colRate;
        private DevExpress.XtraGrid.Columns.GridColumn colCurrencyID;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit repCurrency;
        private System.Windows.Forms.BindingSource tblCurrencyBindingSource;
        private Control_Panel.DataSet.dsBankTableAdapters.tblCurrencyTableAdapter tblCurrencyTableAdapter;
        private System.Windows.Forms.BindingSource tblCurrency1BindingSource;
        private Control_Panel.DataSet.dsBankTableAdapters.tblCurrency1TableAdapter tblCurrency1TableAdapter;
        private System.Windows.Forms.HelpProvider HelpProviderHG;
    }
}