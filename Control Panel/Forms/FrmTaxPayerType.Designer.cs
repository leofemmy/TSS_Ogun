namespace Control_Panel.Forms
{
    partial class FrmTaxPayerType
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
            this.gcTaxPayer = new DevExpress.XtraGrid.GridControl();
            this.tblTaxPayerTypeBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.dsBank = new Control_Panel.DataSet.dsBank();
            this.gvTaxPayer = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colTaxPayerTypeID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colTaxPayerCode = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colTaxPayerName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colStateCode = new DevExpress.XtraGrid.Columns.GridColumn();
            this.tblTaxPayerTypeTableAdapter = new Control_Panel.DataSet.dsBankTableAdapters.tblTaxPayerTypeTableAdapter();
            ((System.ComponentModel.ISupportInitialize)(this.gcTaxPayer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tblTaxPayerTypeBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsBank)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvTaxPayer)).BeginInit();
            this.SuspendLayout();
            // 
            // gcTaxPayer
            // 
            this.gcTaxPayer.DataSource = this.tblTaxPayerTypeBindingSource;
            this.gcTaxPayer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcTaxPayer.EmbeddedNavigator.ButtonClick += new DevExpress.XtraEditors.NavigatorButtonClickEventHandler(this.gcTaxPayer_EmbeddedNavigator_ButtonClick);
            this.gcTaxPayer.Location = new System.Drawing.Point(0, 0);
            this.gcTaxPayer.MainView = this.gvTaxPayer;
            this.gcTaxPayer.Name = "gcTaxPayer";
            this.gcTaxPayer.Size = new System.Drawing.Size(405, 291);
            this.gcTaxPayer.TabIndex = 0;
            this.gcTaxPayer.UseEmbeddedNavigator = true;
            this.gcTaxPayer.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvTaxPayer});
            // 
            // tblTaxPayerTypeBindingSource
            // 
            this.tblTaxPayerTypeBindingSource.DataMember = "tblTaxPayerType";
            this.tblTaxPayerTypeBindingSource.DataSource = this.dsBank;
            // 
            // dsBank
            // 
            this.dsBank.DataSetName = "dsBank";
            this.dsBank.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // gvTaxPayer
            // 
            this.gvTaxPayer.Appearance.ColumnFilterButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(131)))), ((int)(((byte)(153)))), ((int)(((byte)(177)))));
            this.gvTaxPayer.Appearance.ColumnFilterButton.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(173)))), ((int)(((byte)(197)))));
            this.gvTaxPayer.Appearance.ColumnFilterButton.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(131)))), ((int)(((byte)(153)))), ((int)(((byte)(177)))));
            this.gvTaxPayer.Appearance.ColumnFilterButton.ForeColor = System.Drawing.Color.Gray;
            this.gvTaxPayer.Appearance.ColumnFilterButton.Options.UseBackColor = true;
            this.gvTaxPayer.Appearance.ColumnFilterButton.Options.UseBorderColor = true;
            this.gvTaxPayer.Appearance.ColumnFilterButton.Options.UseForeColor = true;
            this.gvTaxPayer.Appearance.ColumnFilterButtonActive.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(173)))), ((int)(((byte)(197)))));
            this.gvTaxPayer.Appearance.ColumnFilterButtonActive.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(177)))), ((int)(((byte)(193)))), ((int)(((byte)(211)))));
            this.gvTaxPayer.Appearance.ColumnFilterButtonActive.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(173)))), ((int)(((byte)(197)))));
            this.gvTaxPayer.Appearance.ColumnFilterButtonActive.ForeColor = System.Drawing.Color.Blue;
            this.gvTaxPayer.Appearance.ColumnFilterButtonActive.Options.UseBackColor = true;
            this.gvTaxPayer.Appearance.ColumnFilterButtonActive.Options.UseBorderColor = true;
            this.gvTaxPayer.Appearance.ColumnFilterButtonActive.Options.UseForeColor = true;
            this.gvTaxPayer.Appearance.Empty.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(173)))), ((int)(((byte)(197)))));
            this.gvTaxPayer.Appearance.Empty.Options.UseBackColor = true;
            this.gvTaxPayer.Appearance.EvenRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(131)))), ((int)(((byte)(153)))), ((int)(((byte)(177)))));
            this.gvTaxPayer.Appearance.EvenRow.BackColor2 = System.Drawing.Color.GhostWhite;
            this.gvTaxPayer.Appearance.EvenRow.ForeColor = System.Drawing.Color.Black;
            this.gvTaxPayer.Appearance.EvenRow.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal;
            this.gvTaxPayer.Appearance.EvenRow.Options.UseBackColor = true;
            this.gvTaxPayer.Appearance.EvenRow.Options.UseForeColor = true;
            this.gvTaxPayer.Appearance.FilterCloseButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(212)))), ((int)(((byte)(208)))), ((int)(((byte)(200)))));
            this.gvTaxPayer.Appearance.FilterCloseButton.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.gvTaxPayer.Appearance.FilterCloseButton.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(212)))), ((int)(((byte)(208)))), ((int)(((byte)(200)))));
            this.gvTaxPayer.Appearance.FilterCloseButton.ForeColor = System.Drawing.Color.Black;
            this.gvTaxPayer.Appearance.FilterCloseButton.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal;
            this.gvTaxPayer.Appearance.FilterCloseButton.Options.UseBackColor = true;
            this.gvTaxPayer.Appearance.FilterCloseButton.Options.UseBorderColor = true;
            this.gvTaxPayer.Appearance.FilterCloseButton.Options.UseForeColor = true;
            this.gvTaxPayer.Appearance.FilterPanel.BackColor = System.Drawing.Color.Black;
            this.gvTaxPayer.Appearance.FilterPanel.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(212)))), ((int)(((byte)(208)))), ((int)(((byte)(200)))));
            this.gvTaxPayer.Appearance.FilterPanel.ForeColor = System.Drawing.Color.White;
            this.gvTaxPayer.Appearance.FilterPanel.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal;
            this.gvTaxPayer.Appearance.FilterPanel.Options.UseBackColor = true;
            this.gvTaxPayer.Appearance.FilterPanel.Options.UseForeColor = true;
            this.gvTaxPayer.Appearance.FixedLine.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(31)))), ((int)(((byte)(55)))));
            this.gvTaxPayer.Appearance.FixedLine.Options.UseBackColor = true;
            this.gvTaxPayer.Appearance.FocusedRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(79)))), ((int)(((byte)(101)))), ((int)(((byte)(125)))));
            this.gvTaxPayer.Appearance.FocusedRow.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(129)))), ((int)(((byte)(151)))), ((int)(((byte)(175)))));
            this.gvTaxPayer.Appearance.FocusedRow.ForeColor = System.Drawing.Color.White;
            this.gvTaxPayer.Appearance.FocusedRow.Options.UseBackColor = true;
            this.gvTaxPayer.Appearance.FocusedRow.Options.UseForeColor = true;
            this.gvTaxPayer.Appearance.FooterPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(131)))), ((int)(((byte)(153)))), ((int)(((byte)(177)))));
            this.gvTaxPayer.Appearance.FooterPanel.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(131)))), ((int)(((byte)(153)))), ((int)(((byte)(177)))));
            this.gvTaxPayer.Appearance.FooterPanel.ForeColor = System.Drawing.Color.Black;
            this.gvTaxPayer.Appearance.FooterPanel.Options.UseBackColor = true;
            this.gvTaxPayer.Appearance.FooterPanel.Options.UseBorderColor = true;
            this.gvTaxPayer.Appearance.FooterPanel.Options.UseForeColor = true;
            this.gvTaxPayer.Appearance.GroupButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(131)))), ((int)(((byte)(153)))), ((int)(((byte)(177)))));
            this.gvTaxPayer.Appearance.GroupButton.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(131)))), ((int)(((byte)(153)))), ((int)(((byte)(177)))));
            this.gvTaxPayer.Appearance.GroupButton.ForeColor = System.Drawing.Color.Black;
            this.gvTaxPayer.Appearance.GroupButton.Options.UseBackColor = true;
            this.gvTaxPayer.Appearance.GroupButton.Options.UseBorderColor = true;
            this.gvTaxPayer.Appearance.GroupButton.Options.UseForeColor = true;
            this.gvTaxPayer.Appearance.GroupFooter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(163)))), ((int)(((byte)(187)))));
            this.gvTaxPayer.Appearance.GroupFooter.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(163)))), ((int)(((byte)(187)))));
            this.gvTaxPayer.Appearance.GroupFooter.ForeColor = System.Drawing.Color.Black;
            this.gvTaxPayer.Appearance.GroupFooter.Options.UseBackColor = true;
            this.gvTaxPayer.Appearance.GroupFooter.Options.UseBorderColor = true;
            this.gvTaxPayer.Appearance.GroupFooter.Options.UseForeColor = true;
            this.gvTaxPayer.Appearance.GroupPanel.BackColor = System.Drawing.Color.Black;
            this.gvTaxPayer.Appearance.GroupPanel.BackColor2 = System.Drawing.Color.White;
            this.gvTaxPayer.Appearance.GroupPanel.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.gvTaxPayer.Appearance.GroupPanel.ForeColor = System.Drawing.Color.White;
            this.gvTaxPayer.Appearance.GroupPanel.Options.UseBackColor = true;
            this.gvTaxPayer.Appearance.GroupPanel.Options.UseFont = true;
            this.gvTaxPayer.Appearance.GroupPanel.Options.UseForeColor = true;
            this.gvTaxPayer.Appearance.GroupRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(79)))), ((int)(((byte)(101)))), ((int)(((byte)(125)))));
            this.gvTaxPayer.Appearance.GroupRow.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(193)))), ((int)(((byte)(204)))), ((int)(((byte)(217)))));
            this.gvTaxPayer.Appearance.GroupRow.Options.UseBackColor = true;
            this.gvTaxPayer.Appearance.GroupRow.Options.UseForeColor = true;
            this.gvTaxPayer.Appearance.HeaderPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(131)))), ((int)(((byte)(153)))), ((int)(((byte)(177)))));
            this.gvTaxPayer.Appearance.HeaderPanel.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(131)))), ((int)(((byte)(153)))), ((int)(((byte)(177)))));
            this.gvTaxPayer.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.gvTaxPayer.Appearance.HeaderPanel.ForeColor = System.Drawing.Color.Black;
            this.gvTaxPayer.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.gvTaxPayer.Appearance.HeaderPanel.Options.UseBorderColor = true;
            this.gvTaxPayer.Appearance.HeaderPanel.Options.UseFont = true;
            this.gvTaxPayer.Appearance.HeaderPanel.Options.UseForeColor = true;
            this.gvTaxPayer.Appearance.HideSelectionRow.BackColor = System.Drawing.Color.Gray;
            this.gvTaxPayer.Appearance.HideSelectionRow.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(212)))), ((int)(((byte)(208)))), ((int)(((byte)(200)))));
            this.gvTaxPayer.Appearance.HideSelectionRow.Options.UseBackColor = true;
            this.gvTaxPayer.Appearance.HideSelectionRow.Options.UseForeColor = true;
            this.gvTaxPayer.Appearance.HorzLine.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(131)))), ((int)(((byte)(153)))), ((int)(((byte)(177)))));
            this.gvTaxPayer.Appearance.HorzLine.Options.UseBackColor = true;
            this.gvTaxPayer.Appearance.OddRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(193)))), ((int)(((byte)(204)))), ((int)(((byte)(217)))));
            this.gvTaxPayer.Appearance.OddRow.BackColor2 = System.Drawing.Color.White;
            this.gvTaxPayer.Appearance.OddRow.ForeColor = System.Drawing.Color.Black;
            this.gvTaxPayer.Appearance.OddRow.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.BackwardDiagonal;
            this.gvTaxPayer.Appearance.OddRow.Options.UseBackColor = true;
            this.gvTaxPayer.Appearance.OddRow.Options.UseForeColor = true;
            this.gvTaxPayer.Appearance.Preview.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(213)))), ((int)(((byte)(237)))));
            this.gvTaxPayer.Appearance.Preview.BackColor2 = System.Drawing.Color.White;
            this.gvTaxPayer.Appearance.Preview.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(79)))), ((int)(((byte)(101)))), ((int)(((byte)(125)))));
            this.gvTaxPayer.Appearance.Preview.Options.UseBackColor = true;
            this.gvTaxPayer.Appearance.Preview.Options.UseForeColor = true;
            this.gvTaxPayer.Appearance.Row.BackColor = System.Drawing.Color.White;
            this.gvTaxPayer.Appearance.Row.ForeColor = System.Drawing.Color.Black;
            this.gvTaxPayer.Appearance.Row.Options.UseBackColor = true;
            this.gvTaxPayer.Appearance.Row.Options.UseForeColor = true;
            this.gvTaxPayer.Appearance.RowSeparator.BackColor = System.Drawing.Color.White;
            this.gvTaxPayer.Appearance.RowSeparator.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(173)))), ((int)(((byte)(197)))));
            this.gvTaxPayer.Appearance.RowSeparator.Options.UseBackColor = true;
            this.gvTaxPayer.Appearance.SelectedRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(89)))), ((int)(((byte)(111)))), ((int)(((byte)(135)))));
            this.gvTaxPayer.Appearance.SelectedRow.ForeColor = System.Drawing.Color.White;
            this.gvTaxPayer.Appearance.SelectedRow.Options.UseBackColor = true;
            this.gvTaxPayer.Appearance.SelectedRow.Options.UseForeColor = true;
            this.gvTaxPayer.Appearance.VertLine.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(131)))), ((int)(((byte)(153)))), ((int)(((byte)(177)))));
            this.gvTaxPayer.Appearance.VertLine.Options.UseBackColor = true;
            this.gvTaxPayer.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colTaxPayerTypeID,
            this.colTaxPayerCode,
            this.colTaxPayerName,
            this.colStateCode});
            this.gvTaxPayer.GridControl = this.gcTaxPayer;
            this.gvTaxPayer.Name = "gvTaxPayer";
            this.gvTaxPayer.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.True;
            this.gvTaxPayer.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.True;
            this.gvTaxPayer.OptionsView.EnableAppearanceEvenRow = true;
            this.gvTaxPayer.OptionsView.EnableAppearanceOddRow = true;
            this.gvTaxPayer.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.Bottom;
            this.gvTaxPayer.OptionsView.ShowGroupPanel = false;
            this.gvTaxPayer.PaintStyleName = "WindowsXP";
            this.gvTaxPayer.InitNewRow += new DevExpress.XtraGrid.Views.Grid.InitNewRowEventHandler(this.gvTaxPayer_InitNewRow);
            // 
            // colTaxPayerTypeID
            // 
            this.colTaxPayerTypeID.FieldName = "TaxPayerTypeID";
            this.colTaxPayerTypeID.Name = "colTaxPayerTypeID";
            this.colTaxPayerTypeID.OptionsColumn.ReadOnly = true;
            // 
            // colTaxPayerCode
            // 
            this.colTaxPayerCode.FieldName = "TaxPayerCode";
            this.colTaxPayerCode.Name = "colTaxPayerCode";
            this.colTaxPayerCode.Visible = true;
            this.colTaxPayerCode.VisibleIndex = 0;
            // 
            // colTaxPayerName
            // 
            this.colTaxPayerName.FieldName = "TaxPayerName";
            this.colTaxPayerName.Name = "colTaxPayerName";
            this.colTaxPayerName.Visible = true;
            this.colTaxPayerName.VisibleIndex = 1;
            // 
            // colStateCode
            // 
            this.colStateCode.FieldName = "StateCode";
            this.colStateCode.Name = "colStateCode";
            // 
            // tblTaxPayerTypeTableAdapter
            // 
            this.tblTaxPayerTypeTableAdapter.ClearBeforeFill = true;
            // 
            // FrmTaxPayerType
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(405, 291);
            this.Controls.Add(this.gcTaxPayer);
            this.Name = "FrmTaxPayerType";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Default State Tax Payer Type Set Up";
            this.Load += new System.EventHandler(this.FrmTaxPayerType_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gcTaxPayer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tblTaxPayerTypeBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsBank)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvTaxPayer)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.GridControl gcTaxPayer;
        private DevExpress.XtraGrid.Views.Grid.GridView gvTaxPayer;
        private Control_Panel.DataSet.dsBank dsBank;
        private System.Windows.Forms.BindingSource tblTaxPayerTypeBindingSource;
        private Control_Panel.DataSet.dsBankTableAdapters.tblTaxPayerTypeTableAdapter tblTaxPayerTypeTableAdapter;
        private DevExpress.XtraGrid.Columns.GridColumn colTaxPayerTypeID;
        private DevExpress.XtraGrid.Columns.GridColumn colTaxPayerCode;
        private DevExpress.XtraGrid.Columns.GridColumn colTaxPayerName;
        private DevExpress.XtraGrid.Columns.GridColumn colStateCode;
    }
}