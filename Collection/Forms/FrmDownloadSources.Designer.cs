namespace Collection.Forms
{
    partial class FrmDownloadSources
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
            this.panelContainer = new DevExpress.XtraEditors.PanelControl();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.label2 = new System.Windows.Forms.Label();
            this.bttnClose = new System.Windows.Forms.Button();
            this.label13 = new System.Windows.Forms.Label();
            this.bttnBrowse = new System.Windows.Forms.Button();
            this.cboSources = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.HelpProviderHG = new System.Windows.Forms.HelpProvider();
            ((System.ComponentModel.ISupportInitialize)(this.panelContainer)).BeginInit();
            this.panelContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelContainer
            // 
            this.panelContainer.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(100)))));
            this.panelContainer.Appearance.Options.UseBackColor = true;
            this.panelContainer.Controls.Add(this.groupControl1);
            this.panelContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelContainer.Location = new System.Drawing.Point(0, 0);
            this.panelContainer.LookAndFeel.UseDefaultLookAndFeel = false;
            this.panelContainer.LookAndFeel.UseWindowsXPTheme = true;
            this.panelContainer.Name = "panelContainer";
            this.panelContainer.Size = new System.Drawing.Size(370, 168);
            this.panelContainer.TabIndex = 0;
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.label2);
            this.groupControl1.Controls.Add(this.bttnClose);
            this.groupControl1.Controls.Add(this.label13);
            this.groupControl1.Controls.Add(this.bttnBrowse);
            this.groupControl1.Controls.Add(this.cboSources);
            this.groupControl1.Controls.Add(this.label1);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupControl1.Location = new System.Drawing.Point(4, 4);
            this.groupControl1.LookAndFeel.SkinName = "Money Twins";
            this.groupControl1.LookAndFeel.UseDefaultLookAndFeel = false;
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(362, 159);
            this.groupControl1.TabIndex = 0;
            this.groupControl1.Text = "Collection Download Sources";
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(263, 101);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 14);
            this.label2.TabIndex = 434;
            this.label2.Text = "&Close";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // bttnClose
            // 
            this.bttnClose.Cursor = System.Windows.Forms.Cursors.Hand;
            this.HelpProviderHG.SetHelpKeyword(this.bttnClose, "FrmDownloadSources.htm#bttnClose");
            this.HelpProviderHG.SetHelpNavigator(this.bttnClose, System.Windows.Forms.HelpNavigator.Topic);
            this.bttnClose.Location = new System.Drawing.Point(267, 61);
            this.bttnClose.Name = "bttnClose";
            this.HelpProviderHG.SetShowHelp(this.bttnClose, true);
            this.bttnClose.Size = new System.Drawing.Size(46, 37);
            this.bttnClose.TabIndex = 433;
            this.bttnClose.UseVisualStyleBackColor = true;
            // 
            // label13
            // 
            this.label13.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(212, 101);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(52, 14);
            this.label13.TabIndex = 432;
            this.label13.Text = "&Select";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // bttnBrowse
            // 
            this.bttnBrowse.Cursor = System.Windows.Forms.Cursors.Hand;
            this.HelpProviderHG.SetHelpKeyword(this.bttnBrowse, "FrmDownloadSources.htm#bttnBrowse");
            this.HelpProviderHG.SetHelpNavigator(this.bttnBrowse, System.Windows.Forms.HelpNavigator.Topic);
            this.bttnBrowse.Location = new System.Drawing.Point(215, 61);
            this.bttnBrowse.Name = "bttnBrowse";
            this.HelpProviderHG.SetShowHelp(this.bttnBrowse, true);
            this.bttnBrowse.Size = new System.Drawing.Size(46, 37);
            this.bttnBrowse.TabIndex = 431;
            this.bttnBrowse.UseVisualStyleBackColor = true;
            // 
            // cboSources
            // 
            this.cboSources.FormattingEnabled = true;
            this.HelpProviderHG.SetHelpKeyword(this.cboSources, "FrmDownloadSources.htm#cboSources");
            this.HelpProviderHG.SetHelpNavigator(this.cboSources, System.Windows.Forms.HelpNavigator.Topic);
            this.cboSources.Items.AddRange(new object[] {
            "PayDirectOnline",
            "ReemsOnline"});
            this.cboSources.Location = new System.Drawing.Point(152, 34);
            this.cboSources.Name = "cboSources";
            this.HelpProviderHG.SetShowHelp(this.cboSources, true);
            this.cboSources.Size = new System.Drawing.Size(161, 21);
            this.cboSources.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(127, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Select Download Sources";
            // 
            // HelpProviderHG
            // 
            this.HelpProviderHG.HelpNamespace = "Bank_Reconciliation.chm";
            // 
            // FrmDownloadSources
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(370, 168);
            this.Controls.Add(this.panelContainer);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.HelpProviderHG.SetHelpKeyword(this, "FrmDownloadSources.htm");
            this.HelpProviderHG.SetHelpNavigator(this, System.Windows.Forms.HelpNavigator.Topic);
            this.Name = "FrmDownloadSources";
            this.HelpProviderHG.SetShowHelp(this, true);
            this.Text = "FrmDownloadSources";
            ((System.ComponentModel.ISupportInitialize)(this.panelContainer)).EndInit();
            this.panelContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.groupControl1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        public DevExpress.XtraEditors.PanelControl panelContainer;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cboSources;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button bttnClose;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Button bttnBrowse;
        private System.Windows.Forms.HelpProvider HelpProviderHG;

    }
}