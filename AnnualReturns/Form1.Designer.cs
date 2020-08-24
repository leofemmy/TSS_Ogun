namespace AnnualReturns
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtTin = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.spinEdit1 = new DevExpress.XtraEditors.SpinEdit();
            this.label1 = new System.Windows.Forms.Label();
            this.btnClear = new DevExpress.XtraEditors.SimpleButton();
            this.btnExport = new DevExpress.XtraEditors.SimpleButton();
            this.btnSave = new DevExpress.XtraEditors.SimpleButton();
            this.btnLoad = new DevExpress.XtraEditors.SimpleButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.classObjectBindingSource = new System.Windows.Forms.BindingSource();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colTIN = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colMonths = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colStaff_No = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colSurname = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colOthernames = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colTotal_Income = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colTotal_Relief = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colTotal_Deduction = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colTax_Deducted = new DevExpress.XtraGrid.Columns.GridColumn();
            this.btnImport = new DevExpress.XtraEditors.SimpleButton();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.spinEdit1.Properties)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.classObjectBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnImport);
            this.groupBox1.Controls.Add(this.txtTin);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.spinEdit1);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.btnClear);
            this.groupBox1.Controls.Add(this.btnExport);
            this.groupBox1.Controls.Add(this.btnSave);
            this.groupBox1.Controls.Add(this.btnLoad);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(880, 116);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            // 
            // txtTin
            // 
            this.txtTin.Location = new System.Drawing.Point(97, 26);
            this.txtTin.Name = "txtTin";
            this.txtTin.Size = new System.Drawing.Size(192, 20);
            this.txtTin.TabIndex = 449;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(20, 29);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 13);
            this.label2.TabIndex = 447;
            this.label2.Text = "Agent Tin";
            // 
            // spinEdit1
            // 
            this.spinEdit1.EditValue = new decimal(new int[] {
            2011,
            0,
            0,
            0});
            this.spinEdit1.Location = new System.Drawing.Point(97, 59);
            this.spinEdit1.Name = "spinEdit1";
            this.spinEdit1.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.spinEdit1.Properties.DisplayFormat.FormatString = "d";
            this.spinEdit1.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.spinEdit1.Properties.EditFormat.FormatString = "d";
            this.spinEdit1.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.spinEdit1.Properties.Mask.EditMask = "d";
            this.spinEdit1.Properties.MaxLength = 4;
            this.spinEdit1.Properties.MaxValue = new decimal(new int[] {
            2120,
            0,
            0,
            0});
            this.spinEdit1.Properties.MinValue = new decimal(new int[] {
            2001,
            0,
            0,
            0});
            this.spinEdit1.Size = new System.Drawing.Size(65, 20);
            this.spinEdit1.TabIndex = 446;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 62);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Entering Year";
            // 
            // btnClear
            // 
            this.btnClear.Image = ((System.Drawing.Image)(resources.GetObject("btnClear.Image")));
            this.btnClear.Location = new System.Drawing.Point(620, 29);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(75, 36);
            this.btnClear.TabIndex = 3;
            this.btnClear.Text = "&Clear";
            // 
            // btnExport
            // 
            this.btnExport.Image = ((System.Drawing.Image)(resources.GetObject("btnExport.Image")));
            this.btnExport.Location = new System.Drawing.Point(479, 29);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(122, 36);
            this.btnExport.TabIndex = 2;
            this.btnExport.Text = "&Export to Excel";
            // 
            // btnSave
            // 
            this.btnSave.Image = ((System.Drawing.Image)(resources.GetObject("btnSave.Image")));
            this.btnSave.Location = new System.Drawing.Point(398, 29);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 36);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "&Save";
            // 
            // btnLoad
            // 
            this.btnLoad.Image = ((System.Drawing.Image)(resources.GetObject("btnLoad.Image")));
            this.btnLoad.Location = new System.Drawing.Point(317, 29);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(75, 36);
            this.btnLoad.TabIndex = 0;
            this.btnLoad.Text = "&Load";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.gridControl1);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox2.Location = new System.Drawing.Point(0, 134);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(1002, 375);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            // 
            // gridControl1
            // 
            this.gridControl1.DataSource = this.classObjectBindingSource;
            this.gridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl1.Location = new System.Drawing.Point(3, 16);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.Size = new System.Drawing.Size(996, 356);
            this.gridControl1.TabIndex = 0;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // classObjectBindingSource
            // 
            this.classObjectBindingSource.DataSource = typeof(ClassObject);
            // 
            // gridView1
            // 
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colTIN,
            this.colMonths,
            this.colStaff_No,
            this.colSurname,
            this.colOthernames,
            this.colTotal_Income,
            this.colTotal_Relief,
            this.colTotal_Deduction,
            this.colTax_Deducted});
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.True;
            this.gridView1.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.Bottom;
            this.gridView1.OptionsView.ShowGroupPanel = false;
            // 
            // colTIN
            // 
            this.colTIN.FieldName = "TIN";
            this.colTIN.Name = "colTIN";
            this.colTIN.Visible = true;
            this.colTIN.VisibleIndex = 0;
            // 
            // colMonths
            // 
            this.colMonths.FieldName = "Months";
            this.colMonths.Name = "colMonths";
            this.colMonths.Visible = true;
            this.colMonths.VisibleIndex = 1;
            // 
            // colStaff_No
            // 
            this.colStaff_No.FieldName = "Staff_No";
            this.colStaff_No.Name = "colStaff_No";
            this.colStaff_No.Visible = true;
            this.colStaff_No.VisibleIndex = 2;
            // 
            // colSurname
            // 
            this.colSurname.FieldName = "Surname";
            this.colSurname.Name = "colSurname";
            this.colSurname.Visible = true;
            this.colSurname.VisibleIndex = 3;
            // 
            // colOthernames
            // 
            this.colOthernames.FieldName = "Othernames";
            this.colOthernames.Name = "colOthernames";
            this.colOthernames.Visible = true;
            this.colOthernames.VisibleIndex = 4;
            // 
            // colTotal_Income
            // 
            this.colTotal_Income.FieldName = "Total_Income";
            this.colTotal_Income.Name = "colTotal_Income";
            this.colTotal_Income.Visible = true;
            this.colTotal_Income.VisibleIndex = 5;
            // 
            // colTotal_Relief
            // 
            this.colTotal_Relief.FieldName = "Total_Relief";
            this.colTotal_Relief.Name = "colTotal_Relief";
            this.colTotal_Relief.Visible = true;
            this.colTotal_Relief.VisibleIndex = 6;
            // 
            // colTotal_Deduction
            // 
            this.colTotal_Deduction.FieldName = "Total_Deduction";
            this.colTotal_Deduction.Name = "colTotal_Deduction";
            this.colTotal_Deduction.Visible = true;
            this.colTotal_Deduction.VisibleIndex = 7;
            // 
            // colTax_Deducted
            // 
            this.colTax_Deducted.FieldName = "Tax_Deducted";
            this.colTax_Deducted.Name = "colTax_Deducted";
            this.colTax_Deducted.Visible = true;
            this.colTax_Deducted.VisibleIndex = 8;
            // 
            // btnImport
            // 
            this.btnImport.Image = ((System.Drawing.Image)(resources.GetObject("btnImport.Image")));
            this.btnImport.Location = new System.Drawing.Point(701, 29);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(108, 36);
            this.btnImport.TabIndex = 477;
            this.btnImport.Text = "&Import";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1002, 509);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "Form1";
            this.Text = "Annual Return ";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.spinEdit1.Properties)).EndInit();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.classObjectBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private DevExpress.XtraEditors.SimpleButton btnClear;
        private DevExpress.XtraEditors.SimpleButton btnExport;
        private DevExpress.XtraEditors.SimpleButton btnSave;
        private DevExpress.XtraEditors.SimpleButton btnLoad;
        private System.Windows.Forms.GroupBox groupBox2;
        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private System.Windows.Forms.BindingSource classObjectBindingSource;
        private DevExpress.XtraGrid.Columns.GridColumn colTIN;
        private DevExpress.XtraGrid.Columns.GridColumn colMonths;
        private DevExpress.XtraGrid.Columns.GridColumn colStaff_No;
        private DevExpress.XtraGrid.Columns.GridColumn colSurname;
        private DevExpress.XtraGrid.Columns.GridColumn colOthernames;
        private DevExpress.XtraGrid.Columns.GridColumn colTotal_Income;
        private DevExpress.XtraGrid.Columns.GridColumn colTotal_Relief;
        private DevExpress.XtraGrid.Columns.GridColumn colTotal_Deduction;
        private DevExpress.XtraGrid.Columns.GridColumn colTax_Deducted;
        private System.Windows.Forms.Label label1;
        private DevExpress.XtraEditors.SpinEdit spinEdit1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtTin;
        private DevExpress.XtraEditors.SimpleButton btnImport;
    }
}

