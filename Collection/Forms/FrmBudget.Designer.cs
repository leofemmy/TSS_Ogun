namespace Collection.Forms
{
    partial class FrmBudget
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
            this.gcBudget = new DevExpress.XtraGrid.GridControl();
            this.tblBudgetBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.dsBudget = new Collection.DataSet.dsBudget();
            this.gvBudget = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.repRevenue = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.tblRevenueTypeBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.dsRevenueEditor = new Collection.DataSet.dsRevenueEditor();
            this.tblBudgetTableAdapter = new Collection.DataSet.dsBudgetTableAdapters.tblBudgetTableAdapter();
            this.tblRevenueTypeTableAdapter = new Collection.DataSet.dsRevenueEditorTableAdapters.tblRevenueTypeTableAdapter();
            this.colRevenueCode = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colStateCode = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colAmount = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colYear = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)(this.gcBudget)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tblBudgetBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsBudget)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvBudget)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repRevenue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tblRevenueTypeBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsRevenueEditor)).BeginInit();
            this.SuspendLayout();
            // 
            // gcBudget
            // 
            this.gcBudget.DataSource = this.tblBudgetBindingSource;
            this.gcBudget.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcBudget.EmbeddedNavigator.ButtonClick += new DevExpress.XtraEditors.NavigatorButtonClickEventHandler(this.gcBudget_EmbeddedNavigator_ButtonClick);
            this.gcBudget.Location = new System.Drawing.Point(0, 0);
            this.gcBudget.MainView = this.gvBudget;
            this.gcBudget.Name = "gcBudget";
            this.gcBudget.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repRevenue});
            this.gcBudget.Size = new System.Drawing.Size(524, 342);
            this.gcBudget.TabIndex = 0;
            this.gcBudget.UseEmbeddedNavigator = true;
            this.gcBudget.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvBudget});
            // 
            // tblBudgetBindingSource
            // 
            this.tblBudgetBindingSource.DataMember = "tblBudget";
            this.tblBudgetBindingSource.DataSource = this.dsBudget;
            // 
            // dsBudget
            // 
            this.dsBudget.DataSetName = "dsBudget";
            this.dsBudget.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // gvBudget
            // 
            this.gvBudget.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colRevenueCode,
            this.colStateCode,
            this.colAmount,
            this.colYear});
            this.gvBudget.GridControl = this.gcBudget;
            this.gvBudget.Name = "gvBudget";
            this.gvBudget.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.True;
            this.gvBudget.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.True;
            this.gvBudget.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.Top;
            this.gvBudget.OptionsView.ShowGroupPanel = false;
            this.gvBudget.InitNewRow += new DevExpress.XtraGrid.Views.Grid.InitNewRowEventHandler(this.gvBudget_InitNewRow);
            // 
            // repRevenue
            // 
            this.repRevenue.AutoHeight = false;
            this.repRevenue.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repRevenue.DataSource = this.tblRevenueTypeBindingSource;
            this.repRevenue.DisplayMember = "RevenueName";
            this.repRevenue.Name = "repRevenue";
            this.repRevenue.ValueMember = "RevenueCode";
            // 
            // tblRevenueTypeBindingSource
            // 
            this.tblRevenueTypeBindingSource.DataMember = "tblRevenueType";
            this.tblRevenueTypeBindingSource.DataSource = this.dsRevenueEditor;
            // 
            // dsRevenueEditor
            // 
            this.dsRevenueEditor.DataSetName = "dsRevenueEditor";
            this.dsRevenueEditor.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // tblBudgetTableAdapter
            // 
            this.tblBudgetTableAdapter.ClearBeforeFill = true;
            // 
            // tblRevenueTypeTableAdapter
            // 
            this.tblRevenueTypeTableAdapter.ClearBeforeFill = true;
            // 
            // colRevenueCode
            // 
            this.colRevenueCode.ColumnEdit = this.repRevenue;
            this.colRevenueCode.FieldName = "RevenueCode";
            this.colRevenueCode.Name = "colRevenueCode";
            this.colRevenueCode.Visible = true;
            this.colRevenueCode.VisibleIndex = 0;
            // 
            // colStateCode
            // 
            this.colStateCode.FieldName = "StateCode";
            this.colStateCode.Name = "colStateCode";
            this.colStateCode.Visible = true;
            this.colStateCode.VisibleIndex = 2;
            // 
            // colAmount
            // 
            this.colAmount.FieldName = "Amount";
            this.colAmount.Name = "colAmount";
            this.colAmount.Visible = true;
            this.colAmount.VisibleIndex = 1;
            // 
            // colYear
            // 
            this.colYear.FieldName = "Year";
            this.colYear.Name = "colYear";
            this.colYear.Visible = true;
            this.colYear.VisibleIndex = 3;
            // 
            // FrmBudget
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(524, 342);
            this.Controls.Add(this.gcBudget);
            this.Name = "FrmBudget";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Annual Revenue Budget";
            this.Load += new System.EventHandler(this.FrmBudget_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gcBudget)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tblBudgetBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsBudget)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvBudget)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repRevenue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tblRevenueTypeBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsRevenueEditor)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.GridControl gcBudget;
        private DevExpress.XtraGrid.Views.Grid.GridView gvBudget;
        private Collection.DataSet.dsBudget dsBudget;
        private System.Windows.Forms.BindingSource tblBudgetBindingSource;
        private Collection.DataSet.dsBudgetTableAdapters.tblBudgetTableAdapter tblBudgetTableAdapter;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit repRevenue;
        private Collection.DataSet.dsRevenueEditor dsRevenueEditor;
        private System.Windows.Forms.BindingSource tblRevenueTypeBindingSource;
        private Collection.DataSet.dsRevenueEditorTableAdapters.tblRevenueTypeTableAdapter tblRevenueTypeTableAdapter;
        private DevExpress.XtraGrid.Columns.GridColumn colRevenueCode;
        private DevExpress.XtraGrid.Columns.GridColumn colStateCode;
        private DevExpress.XtraGrid.Columns.GridColumn colAmount;
        private DevExpress.XtraGrid.Columns.GridColumn colYear;
    }
}