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
            this.splitContainerControl1 = new DevExpress.XtraEditors.SplitContainerControl();
            this.label2 = new System.Windows.Forms.Label();
            this.bttnClose = new System.Windows.Forms.Button();
            this.label13 = new System.Windows.Forms.Label();
            this.bttnBrowse = new System.Windows.Forms.Button();
            this.cboSources = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupControl2 = new DevExpress.XtraEditors.GroupControl();
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.HelpProviderHG = new System.Windows.Forms.HelpProvider();
            ((System.ComponentModel.ISupportInitialize)(this.panelContainer)).BeginInit();
            this.panelContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).BeginInit();
            this.splitContainerControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).BeginInit();
            this.groupControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
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
            this.panelContainer.Size = new System.Drawing.Size(642, 481);
            this.panelContainer.TabIndex = 0;
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.splitContainerControl1);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupControl1.Location = new System.Drawing.Point(4, 4);
            this.groupControl1.LookAndFeel.SkinName = "Money Twins";
            this.groupControl1.LookAndFeel.UseDefaultLookAndFeel = false;
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(401, 472);
            this.groupControl1.TabIndex = 0;
            this.groupControl1.Text = "Collection Download Sources";
            // 
            // splitContainerControl1
            // 
            this.splitContainerControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerControl1.Horizontal = false;
            this.splitContainerControl1.Location = new System.Drawing.Point(2, 21);
            this.splitContainerControl1.Name = "splitContainerControl1";
            this.splitContainerControl1.Panel1.Controls.Add(this.label2);
            this.splitContainerControl1.Panel1.Controls.Add(this.bttnClose);
            this.splitContainerControl1.Panel1.Controls.Add(this.label13);
            this.splitContainerControl1.Panel1.Controls.Add(this.bttnBrowse);
            this.splitContainerControl1.Panel1.Controls.Add(this.cboSources);
            this.splitContainerControl1.Panel1.Controls.Add(this.label1);
            this.splitContainerControl1.Panel1.Text = "Panel1";
            this.splitContainerControl1.Panel2.Controls.Add(this.groupControl2);
            this.splitContainerControl1.Panel2.Text = "Panel2";
            this.splitContainerControl1.Size = new System.Drawing.Size(397, 449);
            this.splitContainerControl1.SplitterPosition = 101;
            this.splitContainerControl1.TabIndex = 435;
            this.splitContainerControl1.Text = "splitContainerControl1";
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(277, 79);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 14);
            this.label2.TabIndex = 440;
            this.label2.Text = "&Close";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // bttnClose
            // 
            this.bttnClose.Cursor = System.Windows.Forms.Cursors.Hand;
            this.HelpProviderHG.SetHelpKeyword(this.bttnClose, "FrmDownloadSources_1.htm#bttnClose");
            this.HelpProviderHG.SetHelpNavigator(this.bttnClose, System.Windows.Forms.HelpNavigator.Topic);
            this.bttnClose.Location = new System.Drawing.Point(281, 39);
            this.bttnClose.Name = "bttnClose";
            this.HelpProviderHG.SetShowHelp(this.bttnClose, true);
            this.bttnClose.Size = new System.Drawing.Size(46, 37);
            this.bttnClose.TabIndex = 439;
            this.bttnClose.UseVisualStyleBackColor = true;
            // 
            // label13
            // 
            this.label13.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(226, 79);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(52, 14);
            this.label13.TabIndex = 438;
            this.label13.Text = "&Select";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // bttnBrowse
            // 
            this.bttnBrowse.Cursor = System.Windows.Forms.Cursors.Hand;
            this.HelpProviderHG.SetHelpKeyword(this.bttnBrowse, "FrmDownloadSources_1.htm#bttnBrowse");
            this.HelpProviderHG.SetHelpNavigator(this.bttnBrowse, System.Windows.Forms.HelpNavigator.Topic);
            this.bttnBrowse.Location = new System.Drawing.Point(229, 39);
            this.bttnBrowse.Name = "bttnBrowse";
            this.HelpProviderHG.SetShowHelp(this.bttnBrowse, true);
            this.bttnBrowse.Size = new System.Drawing.Size(46, 37);
            this.bttnBrowse.TabIndex = 437;
            this.bttnBrowse.UseVisualStyleBackColor = true;
            // 
            // cboSources
            // 
            this.cboSources.FormattingEnabled = true;
            this.HelpProviderHG.SetHelpKeyword(this.cboSources, "FrmDownloadSources_1.htm#cboSources");
            this.HelpProviderHG.SetHelpNavigator(this.cboSources, System.Windows.Forms.HelpNavigator.Topic);
            this.cboSources.Items.AddRange(new object[] {
            "PayDirectOnline",
            "ReemsOnline"});
            this.cboSources.Location = new System.Drawing.Point(166, 12);
            this.cboSources.Name = "cboSources";
            this.HelpProviderHG.SetShowHelp(this.cboSources, true);
            this.cboSources.Size = new System.Drawing.Size(161, 21);
            this.cboSources.TabIndex = 436;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(19, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(127, 13);
            this.label1.TabIndex = 435;
            this.label1.Text = "Select Download Sources";
            // 
            // groupControl2
            // 
            this.groupControl2.AppearanceCaption.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupControl2.AppearanceCaption.Options.UseFont = true;
            this.groupControl2.Controls.Add(this.gridControl1);
            this.groupControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupControl2.Location = new System.Drawing.Point(0, 0);
            this.groupControl2.Name = "groupControl2";
            this.groupControl2.Size = new System.Drawing.Size(397, 343);
            this.groupControl2.TabIndex = 0;
            this.groupControl2.Text = "Pending Cheques";
            // 
            // gridControl1
            // 
            this.gridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.HelpProviderHG.SetHelpKeyword(this.gridControl1, "FrmDownloadSources_1.htm#gridControl1");
            this.HelpProviderHG.SetHelpNavigator(this.gridControl1, System.Windows.Forms.HelpNavigator.Topic);
            this.gridControl1.Location = new System.Drawing.Point(2, 22);
            this.gridControl1.LookAndFeel.SkinName = "iMaginary";
            this.gridControl1.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.Name = "gridControl1";
            this.HelpProviderHG.SetShowHelp(this.gridControl1, true);
            this.gridControl1.Size = new System.Drawing.Size(393, 319);
            this.gridControl1.TabIndex = 0;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsBehavior.Editable = false;
            this.gridView1.OptionsView.ShowGroupPanel = false;
            // 
            // HelpProviderHG
            // 
            this.HelpProviderHG.HelpNamespace = "Bank_Reconciliation.chm";
            // 
            // FrmDownloadSources
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(642, 481);
            this.Controls.Add(this.panelContainer);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.HelpProviderHG.SetHelpKeyword(this, "FrmDownloadSources_1.htm");
            this.HelpProviderHG.SetHelpNavigator(this, System.Windows.Forms.HelpNavigator.Topic);
            this.Name = "FrmDownloadSources";
            this.HelpProviderHG.SetShowHelp(this, true);
            this.Text = "FrmDownloadSources";
            ((System.ComponentModel.ISupportInitialize)(this.panelContainer)).EndInit();
            this.panelContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).EndInit();
            this.splitContainerControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).EndInit();
            this.groupControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public DevExpress.XtraEditors.PanelControl panelContainer;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.SplitContainerControl splitContainerControl1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button bttnClose;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Button bttnBrowse;
        private System.Windows.Forms.ComboBox cboSources;
        private System.Windows.Forms.Label label1;
        private DevExpress.XtraEditors.GroupControl groupControl2;
        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private System.Windows.Forms.HelpProvider HelpProviderHG;

    }
}