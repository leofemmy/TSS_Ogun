namespace Control_Panel.Forms
{
    partial class FrmIncomeType
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
            this.gcIncomeTax = new DevExpress.XtraGrid.GridControl();
            this.tblIncomeTypeBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.dsBank = new Control_Panel.DataSet.dsBank();
            this.gvIncomeTax = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colDescription = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colDateCreated = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colIncomeSourceID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repSources = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.tblIncomeSourceBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.colIncomeClassID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repClass = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.tblIncomeClassBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.tblIncomeTypeTableAdapter = new Control_Panel.DataSet.dsBankTableAdapters.tblIncomeTypeTableAdapter();
            this.tblIncomeSourceTableAdapter = new Control_Panel.DataSet.dsBankTableAdapters.tblIncomeSourceTableAdapter();
            this.tblIncomeClassTableAdapter = new Control_Panel.DataSet.dsBankTableAdapters.tblIncomeClassTableAdapter();
            ((System.ComponentModel.ISupportInitialize)(this.gcIncomeTax)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tblIncomeTypeBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsBank)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvIncomeTax)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repSources)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tblIncomeSourceBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repClass)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tblIncomeClassBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // gcIncomeTax
            // 
            this.gcIncomeTax.DataSource = this.tblIncomeTypeBindingSource;
            this.gcIncomeTax.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcIncomeTax.EmbeddedNavigator.ButtonClick += new DevExpress.XtraEditors.NavigatorButtonClickEventHandler(this.gcIncomeTax_EmbeddedNavigator_ButtonClick);
            this.gcIncomeTax.Location = new System.Drawing.Point(0, 0);
            this.gcIncomeTax.MainView = this.gvIncomeTax;
            this.gcIncomeTax.Name = "gcIncomeTax";
            this.gcIncomeTax.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repSources,
            this.repClass});
            this.gcIncomeTax.Size = new System.Drawing.Size(510, 351);
            this.gcIncomeTax.TabIndex = 0;
            this.gcIncomeTax.UseEmbeddedNavigator = true;
            this.gcIncomeTax.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvIncomeTax});
            // 
            // tblIncomeTypeBindingSource
            // 
            this.tblIncomeTypeBindingSource.DataMember = "tblIncomeType";
            this.tblIncomeTypeBindingSource.DataSource = this.dsBank;
            // 
            // dsBank
            // 
            this.dsBank.DataSetName = "dsBank";
            this.dsBank.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // gvIncomeTax
            // 
            this.gvIncomeTax.Appearance.ColumnFilterButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(131)))), ((int)(((byte)(153)))), ((int)(((byte)(177)))));
            this.gvIncomeTax.Appearance.ColumnFilterButton.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(173)))), ((int)(((byte)(197)))));
            this.gvIncomeTax.Appearance.ColumnFilterButton.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(131)))), ((int)(((byte)(153)))), ((int)(((byte)(177)))));
            this.gvIncomeTax.Appearance.ColumnFilterButton.ForeColor = System.Drawing.Color.Gray;
            this.gvIncomeTax.Appearance.ColumnFilterButton.Options.UseBackColor = true;
            this.gvIncomeTax.Appearance.ColumnFilterButton.Options.UseBorderColor = true;
            this.gvIncomeTax.Appearance.ColumnFilterButton.Options.UseForeColor = true;
            this.gvIncomeTax.Appearance.ColumnFilterButtonActive.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(173)))), ((int)(((byte)(197)))));
            this.gvIncomeTax.Appearance.ColumnFilterButtonActive.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(177)))), ((int)(((byte)(193)))), ((int)(((byte)(211)))));
            this.gvIncomeTax.Appearance.ColumnFilterButtonActive.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(173)))), ((int)(((byte)(197)))));
            this.gvIncomeTax.Appearance.ColumnFilterButtonActive.ForeColor = System.Drawing.Color.Blue;
            this.gvIncomeTax.Appearance.ColumnFilterButtonActive.Options.UseBackColor = true;
            this.gvIncomeTax.Appearance.ColumnFilterButtonActive.Options.UseBorderColor = true;
            this.gvIncomeTax.Appearance.ColumnFilterButtonActive.Options.UseForeColor = true;
            this.gvIncomeTax.Appearance.Empty.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(173)))), ((int)(((byte)(197)))));
            this.gvIncomeTax.Appearance.Empty.Options.UseBackColor = true;
            this.gvIncomeTax.Appearance.EvenRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(131)))), ((int)(((byte)(153)))), ((int)(((byte)(177)))));
            this.gvIncomeTax.Appearance.EvenRow.BackColor2 = System.Drawing.Color.GhostWhite;
            this.gvIncomeTax.Appearance.EvenRow.ForeColor = System.Drawing.Color.Black;
            this.gvIncomeTax.Appearance.EvenRow.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal;
            this.gvIncomeTax.Appearance.EvenRow.Options.UseBackColor = true;
            this.gvIncomeTax.Appearance.EvenRow.Options.UseForeColor = true;
            this.gvIncomeTax.Appearance.FilterCloseButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(212)))), ((int)(((byte)(208)))), ((int)(((byte)(200)))));
            this.gvIncomeTax.Appearance.FilterCloseButton.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.gvIncomeTax.Appearance.FilterCloseButton.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(212)))), ((int)(((byte)(208)))), ((int)(((byte)(200)))));
            this.gvIncomeTax.Appearance.FilterCloseButton.ForeColor = System.Drawing.Color.Black;
            this.gvIncomeTax.Appearance.FilterCloseButton.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal;
            this.gvIncomeTax.Appearance.FilterCloseButton.Options.UseBackColor = true;
            this.gvIncomeTax.Appearance.FilterCloseButton.Options.UseBorderColor = true;
            this.gvIncomeTax.Appearance.FilterCloseButton.Options.UseForeColor = true;
            this.gvIncomeTax.Appearance.FilterPanel.BackColor = System.Drawing.Color.Black;
            this.gvIncomeTax.Appearance.FilterPanel.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(212)))), ((int)(((byte)(208)))), ((int)(((byte)(200)))));
            this.gvIncomeTax.Appearance.FilterPanel.ForeColor = System.Drawing.Color.White;
            this.gvIncomeTax.Appearance.FilterPanel.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal;
            this.gvIncomeTax.Appearance.FilterPanel.Options.UseBackColor = true;
            this.gvIncomeTax.Appearance.FilterPanel.Options.UseForeColor = true;
            this.gvIncomeTax.Appearance.FixedLine.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(31)))), ((int)(((byte)(55)))));
            this.gvIncomeTax.Appearance.FixedLine.Options.UseBackColor = true;
            this.gvIncomeTax.Appearance.FocusedRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(79)))), ((int)(((byte)(101)))), ((int)(((byte)(125)))));
            this.gvIncomeTax.Appearance.FocusedRow.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(129)))), ((int)(((byte)(151)))), ((int)(((byte)(175)))));
            this.gvIncomeTax.Appearance.FocusedRow.ForeColor = System.Drawing.Color.White;
            this.gvIncomeTax.Appearance.FocusedRow.Options.UseBackColor = true;
            this.gvIncomeTax.Appearance.FocusedRow.Options.UseForeColor = true;
            this.gvIncomeTax.Appearance.FooterPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(131)))), ((int)(((byte)(153)))), ((int)(((byte)(177)))));
            this.gvIncomeTax.Appearance.FooterPanel.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(131)))), ((int)(((byte)(153)))), ((int)(((byte)(177)))));
            this.gvIncomeTax.Appearance.FooterPanel.ForeColor = System.Drawing.Color.Black;
            this.gvIncomeTax.Appearance.FooterPanel.Options.UseBackColor = true;
            this.gvIncomeTax.Appearance.FooterPanel.Options.UseBorderColor = true;
            this.gvIncomeTax.Appearance.FooterPanel.Options.UseForeColor = true;
            this.gvIncomeTax.Appearance.GroupButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(131)))), ((int)(((byte)(153)))), ((int)(((byte)(177)))));
            this.gvIncomeTax.Appearance.GroupButton.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(131)))), ((int)(((byte)(153)))), ((int)(((byte)(177)))));
            this.gvIncomeTax.Appearance.GroupButton.ForeColor = System.Drawing.Color.Black;
            this.gvIncomeTax.Appearance.GroupButton.Options.UseBackColor = true;
            this.gvIncomeTax.Appearance.GroupButton.Options.UseBorderColor = true;
            this.gvIncomeTax.Appearance.GroupButton.Options.UseForeColor = true;
            this.gvIncomeTax.Appearance.GroupFooter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(163)))), ((int)(((byte)(187)))));
            this.gvIncomeTax.Appearance.GroupFooter.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(163)))), ((int)(((byte)(187)))));
            this.gvIncomeTax.Appearance.GroupFooter.ForeColor = System.Drawing.Color.Black;
            this.gvIncomeTax.Appearance.GroupFooter.Options.UseBackColor = true;
            this.gvIncomeTax.Appearance.GroupFooter.Options.UseBorderColor = true;
            this.gvIncomeTax.Appearance.GroupFooter.Options.UseForeColor = true;
            this.gvIncomeTax.Appearance.GroupPanel.BackColor = System.Drawing.Color.Black;
            this.gvIncomeTax.Appearance.GroupPanel.BackColor2 = System.Drawing.Color.White;
            this.gvIncomeTax.Appearance.GroupPanel.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.gvIncomeTax.Appearance.GroupPanel.ForeColor = System.Drawing.Color.White;
            this.gvIncomeTax.Appearance.GroupPanel.Options.UseBackColor = true;
            this.gvIncomeTax.Appearance.GroupPanel.Options.UseFont = true;
            this.gvIncomeTax.Appearance.GroupPanel.Options.UseForeColor = true;
            this.gvIncomeTax.Appearance.GroupRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(79)))), ((int)(((byte)(101)))), ((int)(((byte)(125)))));
            this.gvIncomeTax.Appearance.GroupRow.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(193)))), ((int)(((byte)(204)))), ((int)(((byte)(217)))));
            this.gvIncomeTax.Appearance.GroupRow.Options.UseBackColor = true;
            this.gvIncomeTax.Appearance.GroupRow.Options.UseForeColor = true;
            this.gvIncomeTax.Appearance.HeaderPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(131)))), ((int)(((byte)(153)))), ((int)(((byte)(177)))));
            this.gvIncomeTax.Appearance.HeaderPanel.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(131)))), ((int)(((byte)(153)))), ((int)(((byte)(177)))));
            this.gvIncomeTax.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.gvIncomeTax.Appearance.HeaderPanel.ForeColor = System.Drawing.Color.Black;
            this.gvIncomeTax.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.gvIncomeTax.Appearance.HeaderPanel.Options.UseBorderColor = true;
            this.gvIncomeTax.Appearance.HeaderPanel.Options.UseFont = true;
            this.gvIncomeTax.Appearance.HeaderPanel.Options.UseForeColor = true;
            this.gvIncomeTax.Appearance.HideSelectionRow.BackColor = System.Drawing.Color.Gray;
            this.gvIncomeTax.Appearance.HideSelectionRow.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(212)))), ((int)(((byte)(208)))), ((int)(((byte)(200)))));
            this.gvIncomeTax.Appearance.HideSelectionRow.Options.UseBackColor = true;
            this.gvIncomeTax.Appearance.HideSelectionRow.Options.UseForeColor = true;
            this.gvIncomeTax.Appearance.HorzLine.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(131)))), ((int)(((byte)(153)))), ((int)(((byte)(177)))));
            this.gvIncomeTax.Appearance.HorzLine.Options.UseBackColor = true;
            this.gvIncomeTax.Appearance.OddRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(193)))), ((int)(((byte)(204)))), ((int)(((byte)(217)))));
            this.gvIncomeTax.Appearance.OddRow.BackColor2 = System.Drawing.Color.White;
            this.gvIncomeTax.Appearance.OddRow.ForeColor = System.Drawing.Color.Black;
            this.gvIncomeTax.Appearance.OddRow.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.BackwardDiagonal;
            this.gvIncomeTax.Appearance.OddRow.Options.UseBackColor = true;
            this.gvIncomeTax.Appearance.OddRow.Options.UseForeColor = true;
            this.gvIncomeTax.Appearance.Preview.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(213)))), ((int)(((byte)(237)))));
            this.gvIncomeTax.Appearance.Preview.BackColor2 = System.Drawing.Color.White;
            this.gvIncomeTax.Appearance.Preview.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(79)))), ((int)(((byte)(101)))), ((int)(((byte)(125)))));
            this.gvIncomeTax.Appearance.Preview.Options.UseBackColor = true;
            this.gvIncomeTax.Appearance.Preview.Options.UseForeColor = true;
            this.gvIncomeTax.Appearance.Row.BackColor = System.Drawing.Color.White;
            this.gvIncomeTax.Appearance.Row.ForeColor = System.Drawing.Color.Black;
            this.gvIncomeTax.Appearance.Row.Options.UseBackColor = true;
            this.gvIncomeTax.Appearance.Row.Options.UseForeColor = true;
            this.gvIncomeTax.Appearance.RowSeparator.BackColor = System.Drawing.Color.White;
            this.gvIncomeTax.Appearance.RowSeparator.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(173)))), ((int)(((byte)(197)))));
            this.gvIncomeTax.Appearance.RowSeparator.Options.UseBackColor = true;
            this.gvIncomeTax.Appearance.SelectedRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(89)))), ((int)(((byte)(111)))), ((int)(((byte)(135)))));
            this.gvIncomeTax.Appearance.SelectedRow.ForeColor = System.Drawing.Color.White;
            this.gvIncomeTax.Appearance.SelectedRow.Options.UseBackColor = true;
            this.gvIncomeTax.Appearance.SelectedRow.Options.UseForeColor = true;
            this.gvIncomeTax.Appearance.VertLine.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(131)))), ((int)(((byte)(153)))), ((int)(((byte)(177)))));
            this.gvIncomeTax.Appearance.VertLine.Options.UseBackColor = true;
            this.gvIncomeTax.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colDescription,
            this.colDateCreated,
            this.colIncomeSourceID,
            this.colIncomeClassID});
            this.gvIncomeTax.GridControl = this.gcIncomeTax;
            this.gvIncomeTax.Name = "gvIncomeTax";
            this.gvIncomeTax.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.True;
            this.gvIncomeTax.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.True;
            this.gvIncomeTax.OptionsView.EnableAppearanceEvenRow = true;
            this.gvIncomeTax.OptionsView.EnableAppearanceOddRow = true;
            this.gvIncomeTax.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.Bottom;
            this.gvIncomeTax.OptionsView.ShowGroupPanel = false;
            this.gvIncomeTax.PaintStyleName = "WindowsXP";
            // 
            // colDescription
            // 
            this.colDescription.Caption = "Income Name";
            this.colDescription.FieldName = "Description";
            this.colDescription.Name = "colDescription";
            this.colDescription.Visible = true;
            this.colDescription.VisibleIndex = 0;
            // 
            // colDateCreated
            // 
            this.colDateCreated.FieldName = "DateCreated";
            this.colDateCreated.Name = "colDateCreated";
            // 
            // colIncomeSourceID
            // 
            this.colIncomeSourceID.Caption = "Income Source";
            this.colIncomeSourceID.ColumnEdit = this.repSources;
            this.colIncomeSourceID.FieldName = "IncomeSourceID";
            this.colIncomeSourceID.Name = "colIncomeSourceID";
            this.colIncomeSourceID.Visible = true;
            this.colIncomeSourceID.VisibleIndex = 1;
            // 
            // repSources
            // 
            this.repSources.AutoHeight = false;
            this.repSources.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repSources.DataSource = this.tblIncomeSourceBindingSource;
            this.repSources.DisplayMember = "Description";
            this.repSources.Name = "repSources";
            this.repSources.ValueMember = "IncomeSourceID";
            // 
            // tblIncomeSourceBindingSource
            // 
            this.tblIncomeSourceBindingSource.DataMember = "tblIncomeSource";
            this.tblIncomeSourceBindingSource.DataSource = this.dsBank;
            // 
            // colIncomeClassID
            // 
            this.colIncomeClassID.Caption = "Income Class";
            this.colIncomeClassID.ColumnEdit = this.repClass;
            this.colIncomeClassID.FieldName = "IncomeClassID";
            this.colIncomeClassID.Name = "colIncomeClassID";
            this.colIncomeClassID.Visible = true;
            this.colIncomeClassID.VisibleIndex = 2;
            // 
            // repClass
            // 
            this.repClass.AutoHeight = false;
            this.repClass.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repClass.DataSource = this.tblIncomeClassBindingSource;
            this.repClass.DisplayMember = "Description";
            this.repClass.Name = "repClass";
            this.repClass.ValueMember = "IncomeClassID";
            // 
            // tblIncomeClassBindingSource
            // 
            this.tblIncomeClassBindingSource.DataMember = "tblIncomeClass";
            this.tblIncomeClassBindingSource.DataSource = this.dsBank;
            // 
            // tblIncomeTypeTableAdapter
            // 
            this.tblIncomeTypeTableAdapter.ClearBeforeFill = true;
            // 
            // tblIncomeSourceTableAdapter
            // 
            this.tblIncomeSourceTableAdapter.ClearBeforeFill = true;
            // 
            // tblIncomeClassTableAdapter
            // 
            this.tblIncomeClassTableAdapter.ClearBeforeFill = true;
            // 
            // FrmIncomeType
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(510, 351);
            this.Controls.Add(this.gcIncomeTax);
            this.Name = "FrmIncomeType";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Chargeable Income Type Definitation";
            this.Load += new System.EventHandler(this.FrmIncomeType_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gcIncomeTax)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tblIncomeTypeBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsBank)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvIncomeTax)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repSources)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tblIncomeSourceBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repClass)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tblIncomeClassBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.GridControl gcIncomeTax;
        private DevExpress.XtraGrid.Views.Grid.GridView gvIncomeTax;
        private Control_Panel.DataSet.dsBank dsBank;
        private System.Windows.Forms.BindingSource tblIncomeTypeBindingSource;
        private Control_Panel.DataSet.dsBankTableAdapters.tblIncomeTypeTableAdapter tblIncomeTypeTableAdapter;
        private DevExpress.XtraGrid.Columns.GridColumn colDescription;
        private DevExpress.XtraGrid.Columns.GridColumn colDateCreated;
        private DevExpress.XtraGrid.Columns.GridColumn colIncomeSourceID;
        private DevExpress.XtraGrid.Columns.GridColumn colIncomeClassID;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit repSources;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit repClass;
        private System.Windows.Forms.BindingSource tblIncomeSourceBindingSource;
        private Control_Panel.DataSet.dsBankTableAdapters.tblIncomeSourceTableAdapter tblIncomeSourceTableAdapter;
        private System.Windows.Forms.BindingSource tblIncomeClassBindingSource;
        private Control_Panel.DataSet.dsBankTableAdapters.tblIncomeClassTableAdapter tblIncomeClassTableAdapter;
    }
}