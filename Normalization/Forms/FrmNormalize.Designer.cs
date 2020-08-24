namespace Normalization.Forms
{
    partial class FrmNormalize
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.gcNormalize = new DevExpress.XtraGrid.GridControl();
            this.gvNormalize = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.btnNormalize = new System.Windows.Forms.Button();
            this.cboTaxAgent = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnSearch = new System.Windows.Forms.Button();
            this.txtfieldname = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcNormalize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvNormalize)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnCancel);
            this.groupBox1.Controls.Add(this.gcNormalize);
            this.groupBox1.Controls.Add(this.btnNormalize);
            this.groupBox1.Controls.Add(this.cboTaxAgent);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.btnSearch);
            this.groupBox1.Controls.Add(this.txtfieldname);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(1, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(603, 367);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // gcNormalize
            // 
            this.gcNormalize.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.gcNormalize.Location = new System.Drawing.Point(3, 83);
            this.gcNormalize.MainView = this.gvNormalize;
            this.gcNormalize.Name = "gcNormalize";
            this.gcNormalize.Size = new System.Drawing.Size(597, 281);
            this.gcNormalize.TabIndex = 13;
            this.gcNormalize.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvNormalize});
            // 
            // gvNormalize
            // 
            this.gvNormalize.Appearance.ColumnFilterButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(244)))), ((int)(((byte)(250)))));
            this.gvNormalize.Appearance.ColumnFilterButton.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(154)))), ((int)(((byte)(153)))), ((int)(((byte)(182)))));
            this.gvNormalize.Appearance.ColumnFilterButton.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(244)))), ((int)(((byte)(250)))));
            this.gvNormalize.Appearance.ColumnFilterButton.ForeColor = System.Drawing.Color.Black;
            this.gvNormalize.Appearance.ColumnFilterButton.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.gvNormalize.Appearance.ColumnFilterButton.Options.UseBackColor = true;
            this.gvNormalize.Appearance.ColumnFilterButton.Options.UseBorderColor = true;
            this.gvNormalize.Appearance.ColumnFilterButton.Options.UseForeColor = true;
            this.gvNormalize.Appearance.ColumnFilterButtonActive.BackColor = System.Drawing.Color.White;
            this.gvNormalize.Appearance.ColumnFilterButtonActive.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(198)))), ((int)(((byte)(215)))));
            this.gvNormalize.Appearance.ColumnFilterButtonActive.BorderColor = System.Drawing.Color.White;
            this.gvNormalize.Appearance.ColumnFilterButtonActive.ForeColor = System.Drawing.Color.Black;
            this.gvNormalize.Appearance.ColumnFilterButtonActive.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.gvNormalize.Appearance.ColumnFilterButtonActive.Options.UseBackColor = true;
            this.gvNormalize.Appearance.ColumnFilterButtonActive.Options.UseBorderColor = true;
            this.gvNormalize.Appearance.ColumnFilterButtonActive.Options.UseForeColor = true;
            this.gvNormalize.Appearance.Empty.BackColor = System.Drawing.Color.White;
            this.gvNormalize.Appearance.Empty.Options.UseBackColor = true;
            this.gvNormalize.Appearance.EvenRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(244)))), ((int)(((byte)(250)))));
            this.gvNormalize.Appearance.EvenRow.ForeColor = System.Drawing.Color.Black;
            this.gvNormalize.Appearance.EvenRow.Options.UseBackColor = true;
            this.gvNormalize.Appearance.EvenRow.Options.UseForeColor = true;
            this.gvNormalize.Appearance.FilterCloseButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(244)))), ((int)(((byte)(250)))));
            this.gvNormalize.Appearance.FilterCloseButton.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(154)))), ((int)(((byte)(153)))), ((int)(((byte)(182)))));
            this.gvNormalize.Appearance.FilterCloseButton.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(244)))), ((int)(((byte)(250)))));
            this.gvNormalize.Appearance.FilterCloseButton.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.gvNormalize.Appearance.FilterCloseButton.Options.UseBackColor = true;
            this.gvNormalize.Appearance.FilterCloseButton.Options.UseBorderColor = true;
            this.gvNormalize.Appearance.FilterPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(133)))), ((int)(((byte)(131)))), ((int)(((byte)(161)))));
            this.gvNormalize.Appearance.FilterPanel.ForeColor = System.Drawing.Color.White;
            this.gvNormalize.Appearance.FilterPanel.Options.UseBackColor = true;
            this.gvNormalize.Appearance.FilterPanel.Options.UseForeColor = true;
            this.gvNormalize.Appearance.FixedLine.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(124)))), ((int)(((byte)(148)))));
            this.gvNormalize.Appearance.FixedLine.Options.UseBackColor = true;
            this.gvNormalize.Appearance.FocusedRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(178)))), ((int)(((byte)(180)))), ((int)(((byte)(191)))));
            this.gvNormalize.Appearance.FocusedRow.ForeColor = System.Drawing.Color.Black;
            this.gvNormalize.Appearance.FocusedRow.Options.UseBackColor = true;
            this.gvNormalize.Appearance.FocusedRow.Options.UseForeColor = true;
            this.gvNormalize.Appearance.FooterPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(244)))), ((int)(((byte)(250)))));
            this.gvNormalize.Appearance.FooterPanel.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(154)))), ((int)(((byte)(153)))), ((int)(((byte)(182)))));
            this.gvNormalize.Appearance.FooterPanel.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(244)))), ((int)(((byte)(250)))));
            this.gvNormalize.Appearance.FooterPanel.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.gvNormalize.Appearance.FooterPanel.Options.UseBackColor = true;
            this.gvNormalize.Appearance.FooterPanel.Options.UseBorderColor = true;
            this.gvNormalize.Appearance.GroupButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(215)))), ((int)(((byte)(215)))), ((int)(((byte)(229)))));
            this.gvNormalize.Appearance.GroupButton.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(215)))), ((int)(((byte)(215)))), ((int)(((byte)(229)))));
            this.gvNormalize.Appearance.GroupButton.ForeColor = System.Drawing.Color.Black;
            this.gvNormalize.Appearance.GroupButton.Options.UseBackColor = true;
            this.gvNormalize.Appearance.GroupButton.Options.UseBorderColor = true;
            this.gvNormalize.Appearance.GroupButton.Options.UseForeColor = true;
            this.gvNormalize.Appearance.GroupFooter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(215)))), ((int)(((byte)(215)))), ((int)(((byte)(229)))));
            this.gvNormalize.Appearance.GroupFooter.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(215)))), ((int)(((byte)(215)))), ((int)(((byte)(229)))));
            this.gvNormalize.Appearance.GroupFooter.ForeColor = System.Drawing.Color.Black;
            this.gvNormalize.Appearance.GroupFooter.Options.UseBackColor = true;
            this.gvNormalize.Appearance.GroupFooter.Options.UseBorderColor = true;
            this.gvNormalize.Appearance.GroupFooter.Options.UseForeColor = true;
            this.gvNormalize.Appearance.GroupPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(133)))), ((int)(((byte)(131)))), ((int)(((byte)(161)))));
            this.gvNormalize.Appearance.GroupPanel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(244)))), ((int)(((byte)(250)))));
            this.gvNormalize.Appearance.GroupPanel.Options.UseBackColor = true;
            this.gvNormalize.Appearance.GroupPanel.Options.UseForeColor = true;
            this.gvNormalize.Appearance.GroupRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(215)))), ((int)(((byte)(215)))), ((int)(((byte)(229)))));
            this.gvNormalize.Appearance.GroupRow.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(215)))), ((int)(((byte)(215)))), ((int)(((byte)(229)))));
            this.gvNormalize.Appearance.GroupRow.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.gvNormalize.Appearance.GroupRow.ForeColor = System.Drawing.Color.Black;
            this.gvNormalize.Appearance.GroupRow.Options.UseBackColor = true;
            this.gvNormalize.Appearance.GroupRow.Options.UseBorderColor = true;
            this.gvNormalize.Appearance.GroupRow.Options.UseFont = true;
            this.gvNormalize.Appearance.GroupRow.Options.UseForeColor = true;
            this.gvNormalize.Appearance.HeaderPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(244)))), ((int)(((byte)(250)))));
            this.gvNormalize.Appearance.HeaderPanel.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(154)))), ((int)(((byte)(153)))), ((int)(((byte)(182)))));
            this.gvNormalize.Appearance.HeaderPanel.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(244)))), ((int)(((byte)(250)))));
            this.gvNormalize.Appearance.HeaderPanel.ForeColor = System.Drawing.Color.Black;
            this.gvNormalize.Appearance.HeaderPanel.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.gvNormalize.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.gvNormalize.Appearance.HeaderPanel.Options.UseBorderColor = true;
            this.gvNormalize.Appearance.HeaderPanel.Options.UseForeColor = true;
            this.gvNormalize.Appearance.HideSelectionRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(218)))), ((int)(((byte)(219)))), ((int)(((byte)(226)))));
            this.gvNormalize.Appearance.HideSelectionRow.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(133)))), ((int)(((byte)(131)))), ((int)(((byte)(161)))));
            this.gvNormalize.Appearance.HideSelectionRow.Options.UseBackColor = true;
            this.gvNormalize.Appearance.HideSelectionRow.Options.UseForeColor = true;
            this.gvNormalize.Appearance.HorzLine.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(164)))), ((int)(((byte)(164)))), ((int)(((byte)(188)))));
            this.gvNormalize.Appearance.HorzLine.Options.UseBackColor = true;
            this.gvNormalize.Appearance.OddRow.BackColor = System.Drawing.Color.White;
            this.gvNormalize.Appearance.OddRow.ForeColor = System.Drawing.Color.Black;
            this.gvNormalize.Appearance.OddRow.Options.UseBackColor = true;
            this.gvNormalize.Appearance.OddRow.Options.UseForeColor = true;
            this.gvNormalize.Appearance.Preview.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(253)))));
            this.gvNormalize.Appearance.Preview.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(163)))), ((int)(((byte)(165)))), ((int)(((byte)(177)))));
            this.gvNormalize.Appearance.Preview.Options.UseBackColor = true;
            this.gvNormalize.Appearance.Preview.Options.UseForeColor = true;
            this.gvNormalize.Appearance.Row.BackColor = System.Drawing.Color.White;
            this.gvNormalize.Appearance.Row.ForeColor = System.Drawing.Color.Black;
            this.gvNormalize.Appearance.Row.Options.UseBackColor = true;
            this.gvNormalize.Appearance.Row.Options.UseForeColor = true;
            this.gvNormalize.Appearance.RowSeparator.BackColor = System.Drawing.Color.White;
            this.gvNormalize.Appearance.RowSeparator.Options.UseBackColor = true;
            this.gvNormalize.Appearance.SelectedRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(195)))), ((int)(((byte)(197)))), ((int)(((byte)(205)))));
            this.gvNormalize.Appearance.SelectedRow.ForeColor = System.Drawing.Color.Black;
            this.gvNormalize.Appearance.SelectedRow.Options.UseBackColor = true;
            this.gvNormalize.Appearance.SelectedRow.Options.UseForeColor = true;
            this.gvNormalize.Appearance.VertLine.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(164)))), ((int)(((byte)(164)))), ((int)(((byte)(188)))));
            this.gvNormalize.Appearance.VertLine.Options.UseBackColor = true;
            this.gvNormalize.GridControl = this.gcNormalize;
            this.gvNormalize.Name = "gvNormalize";
            this.gvNormalize.OptionsBehavior.Editable = false;
            this.gvNormalize.OptionsView.EnableAppearanceEvenRow = true;
            this.gvNormalize.OptionsView.EnableAppearanceOddRow = true;
            this.gvNormalize.OptionsView.ShowGroupPanel = false;
            // 
            // btnNormalize
            // 
            this.btnNormalize.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Bold);
            this.btnNormalize.Location = new System.Drawing.Point(419, 54);
            this.btnNormalize.Name = "btnNormalize";
            this.btnNormalize.Size = new System.Drawing.Size(71, 23);
            this.btnNormalize.TabIndex = 12;
            this.btnNormalize.Text = "Normalize";
            this.btnNormalize.UseVisualStyleBackColor = true;
            this.btnNormalize.Click += new System.EventHandler(this.btnNormalize_Click);
            // 
            // cboTaxAgent
            // 
            this.cboTaxAgent.DisplayMember = "Payer Name";
            this.cboTaxAgent.FormattingEnabled = true;
            this.cboTaxAgent.Location = new System.Drawing.Point(192, 54);
            this.cboTaxAgent.Name = "cboTaxAgent";
            this.cboTaxAgent.Size = new System.Drawing.Size(186, 21);
            this.cboTaxAgent.TabIndex = 11;
            this.cboTaxAgent.ValueMember = "UTIN";
            this.cboTaxAgent.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cboTaxAgent_KeyPress);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Bold);
            this.label2.Location = new System.Drawing.Point(5, 54);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(162, 15);
            this.label2.TabIndex = 10;
            this.label2.Text = "Tax Agents for Normalization";
            // 
            // btnSearch
            // 
            this.btnSearch.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Bold);
            this.btnSearch.Location = new System.Drawing.Point(419, 19);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(57, 23);
            this.btnSearch.TabIndex = 9;
            this.btnSearch.Text = "&Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // txtfieldname
            // 
            this.txtfieldname.Location = new System.Drawing.Point(192, 19);
            this.txtfieldname.Name = "txtfieldname";
            this.txtfieldname.Size = new System.Drawing.Size(190, 20);
            this.txtfieldname.TabIndex = 8;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(5, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(132, 15);
            this.label1.TabIndex = 7;
            this.label1.Text = "Search By Payer Name:";
            // 
            // btnCancel
            // 
            this.btnCancel.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Bold);
            this.btnCancel.Location = new System.Drawing.Point(507, 19);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(57, 23);
            this.btnCancel.TabIndex = 14;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // FrmNormalize
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(605, 391);
            this.Controls.Add(this.groupBox1);
            this.Name = "FrmNormalize";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Normalize Payer Indentity";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcNormalize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvNormalize)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox cboTaxAgent;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.TextBox txtfieldname;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnNormalize;
        private DevExpress.XtraGrid.GridControl gcNormalize;
        private DevExpress.XtraGrid.Views.Grid.GridView gvNormalize;
        private System.Windows.Forms.Button btnCancel;
    }
}