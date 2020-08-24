namespace Control_Panel.Forms
{
    partial class FrmOptions
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmOptions));
            this.panelContainer = new DevExpress.XtraEditors.PanelControl();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.label11 = new System.Windows.Forms.Label();
            this.bttnCancel = new System.Windows.Forms.Button();
            this.label13 = new System.Windows.Forms.Label();
            this.bttnUpdate = new System.Windows.Forms.Button();
            this.xtraTabControl1 = new DevExpress.XtraTab.XtraTabControl();
            this.xtPage1 = new DevExpress.XtraTab.XtraTabPage();
            this.cboRevenue = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.xtPage2 = new DevExpress.XtraTab.XtraTabPage();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.txtName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.xtPage3 = new DevExpress.XtraTab.XtraTabPage();
            this.txtCode = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.xtPage4 = new DevExpress.XtraTab.XtraTabPage();
            this.chkRecepit = new System.Windows.Forms.CheckBox();
            this.txtConfirmation = new System.Windows.Forms.TextBox();
            this.txtWebSite = new System.Windows.Forms.TextBox();
            this.txtShortname = new System.Windows.Forms.TextBox();
            this.txtIssuing = new System.Windows.Forms.TextBox();
            this.txtReceipt = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
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
            this.HelpProviderHG = new System.Windows.Forms.HelpProvider();
            ((System.ComponentModel.ISupportInitialize)(this.panelContainer)).BeginInit();
            this.panelContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).BeginInit();
            this.xtraTabControl1.SuspendLayout();
            this.xtPage1.SuspendLayout();
            this.xtPage2.SuspendLayout();
            this.xtPage3.SuspendLayout();
            this.xtPage4.SuspendLayout();
            this.toolStrip.SuspendLayout();
            this.SuspendLayout();
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
            this.panelContainer.Size = new System.Drawing.Size(573, 437);
            this.panelContainer.TabIndex = 0;
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.splitContainer1);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupControl1.Location = new System.Drawing.Point(4, 41);
            this.groupControl1.LookAndFeel.SkinName = "Money Twins";
            this.groupControl1.LookAndFeel.UseDefaultLookAndFeel = false;
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(515, 391);
            this.groupControl1.TabIndex = 7;
            this.groupControl1.Text = "Collections Option Settings";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.HelpProviderHG.SetHelpKeyword(this.splitContainer1, "FrmOptions.htm#splitContainer1");
            this.HelpProviderHG.SetHelpNavigator(this.splitContainer1, System.Windows.Forms.HelpNavigator.Topic);
            this.splitContainer1.Location = new System.Drawing.Point(2, 21);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.label11);
            this.splitContainer1.Panel1.Controls.Add(this.bttnCancel);
            this.splitContainer1.Panel1.Controls.Add(this.label13);
            this.splitContainer1.Panel1.Controls.Add(this.bttnUpdate);
            this.splitContainer1.Panel1.Controls.Add(this.xtraTabControl1);
            this.HelpProviderHG.SetShowHelp(this.splitContainer1, true);
            this.splitContainer1.Size = new System.Drawing.Size(511, 368);
            this.splitContainer1.SplitterDistance = 318;
            this.splitContainer1.TabIndex = 0;
            // 
            // label11
            // 
            this.label11.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(437, 281);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(40, 14);
            this.label11.TabIndex = 451;
            this.label11.Text = "&Cancel";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // bttnCancel
            // 
            this.bttnCancel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.bttnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.HelpProviderHG.SetHelpKeyword(this.bttnCancel, "FrmOptions.htm#bttnCancel");
            this.HelpProviderHG.SetHelpNavigator(this.bttnCancel, System.Windows.Forms.HelpNavigator.Topic);
            this.bttnCancel.Location = new System.Drawing.Point(441, 241);
            this.bttnCancel.Name = "bttnCancel";
            this.HelpProviderHG.SetShowHelp(this.bttnCancel, true);
            this.bttnCancel.Size = new System.Drawing.Size(34, 37);
            this.bttnCancel.TabIndex = 450;
            this.bttnCancel.UseVisualStyleBackColor = true;
            // 
            // label13
            // 
            this.label13.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(382, 281);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(41, 14);
            this.label13.TabIndex = 449;
            this.label13.Text = "&Update";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // bttnUpdate
            // 
            this.bttnUpdate.Cursor = System.Windows.Forms.Cursors.Hand;
            this.HelpProviderHG.SetHelpKeyword(this.bttnUpdate, "FrmOptions.htm#bttnUpdate");
            this.HelpProviderHG.SetHelpNavigator(this.bttnUpdate, System.Windows.Forms.HelpNavigator.Topic);
            this.bttnUpdate.Location = new System.Drawing.Point(386, 241);
            this.bttnUpdate.Name = "bttnUpdate";
            this.HelpProviderHG.SetShowHelp(this.bttnUpdate, true);
            this.bttnUpdate.Size = new System.Drawing.Size(34, 37);
            this.bttnUpdate.TabIndex = 448;
            this.bttnUpdate.UseVisualStyleBackColor = true;
            // 
            // xtraTabControl1
            // 
            this.xtraTabControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.HelpProviderHG.SetHelpKeyword(this.xtraTabControl1, "FrmOptions.htm#xtraTabControl1");
            this.HelpProviderHG.SetHelpNavigator(this.xtraTabControl1, System.Windows.Forms.HelpNavigator.Topic);
            this.xtraTabControl1.Location = new System.Drawing.Point(0, 0);
            this.xtraTabControl1.LookAndFeel.SkinName = "Money Twins";
            this.xtraTabControl1.LookAndFeel.UseDefaultLookAndFeel = false;
            this.xtraTabControl1.Name = "xtraTabControl1";
            this.xtraTabControl1.SelectedTabPage = this.xtPage1;
            this.HelpProviderHG.SetShowHelp(this.xtraTabControl1, true);
            this.xtraTabControl1.Size = new System.Drawing.Size(511, 235);
            this.xtraTabControl1.TabIndex = 1;
            this.xtraTabControl1.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.xtPage1,
            this.xtPage2,
            this.xtPage3,
            this.xtPage4});
            // 
            // xtPage1
            // 
            this.xtPage1.Controls.Add(this.cboRevenue);
            this.xtPage1.Controls.Add(this.label1);
            this.xtPage1.Name = "xtPage1";
            this.xtPage1.Size = new System.Drawing.Size(505, 207);
            this.xtPage1.Text = "Revenue Suspense";
            this.xtPage1.Tooltip = "Revenue Suspense";
            // 
            // cboRevenue
            // 
            this.cboRevenue.FormattingEnabled = true;
            this.HelpProviderHG.SetHelpKeyword(this.cboRevenue, "FrmOptions.htm#cboRevenue");
            this.HelpProviderHG.SetHelpNavigator(this.cboRevenue, System.Windows.Forms.HelpNavigator.Topic);
            this.cboRevenue.Location = new System.Drawing.Point(142, 36);
            this.cboRevenue.Name = "cboRevenue";
            this.HelpProviderHG.SetShowHelp(this.cboRevenue, true);
            this.cboRevenue.Size = new System.Drawing.Size(295, 21);
            this.cboRevenue.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 36);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(99, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Revenue Suspense";
            // 
            // xtPage2
            // 
            this.xtPage2.Controls.Add(this.txtDescription);
            this.xtPage2.Controls.Add(this.txtName);
            this.xtPage2.Controls.Add(this.label3);
            this.xtPage2.Controls.Add(this.label2);
            this.xtPage2.Name = "xtPage2";
            this.xtPage2.Size = new System.Drawing.Size(505, 207);
            this.xtPage2.Text = "ETicket Headers";
            this.xtPage2.Tooltip = "ETicket Headers";
            // 
            // txtDescription
            // 
            this.txtDescription.Location = new System.Drawing.Point(166, 86);
            this.txtDescription.Multiline = true;
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(237, 108);
            this.txtDescription.TabIndex = 3;
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(166, 22);
            this.txtName.Multiline = true;
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(237, 58);
            this.txtName.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(28, 86);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(101, 32);
            this.label3.TabIndex = 1;
            this.label3.Text = "ETicket Payment Description";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(27, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(95, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Name of Authority";
            // 
            // xtPage3
            // 
            this.xtPage3.Controls.Add(this.txtCode);
            this.xtPage3.Controls.Add(this.label4);
            this.xtPage3.Name = "xtPage3";
            this.xtPage3.Size = new System.Drawing.Size(505, 207);
            this.xtPage3.Text = "State PayDirect Reference";
            this.xtPage3.Tooltip = "State PayDirect Reference";
            // 
            // txtCode
            // 
            this.txtCode.Location = new System.Drawing.Point(176, 30);
            this.txtCode.Name = "txtCode";
            this.txtCode.Size = new System.Drawing.Size(111, 21);
            this.txtCode.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(20, 30);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(88, 35);
            this.label4.TabIndex = 0;
            this.label4.Text = "State Pay Direct Reference Code";
            // 
            // xtPage4
            // 
            this.xtPage4.Controls.Add(this.chkRecepit);
            this.xtPage4.Controls.Add(this.txtConfirmation);
            this.xtPage4.Controls.Add(this.txtWebSite);
            this.xtPage4.Controls.Add(this.txtShortname);
            this.xtPage4.Controls.Add(this.txtIssuing);
            this.xtPage4.Controls.Add(this.txtReceipt);
            this.xtPage4.Controls.Add(this.label9);
            this.xtPage4.Controls.Add(this.label8);
            this.xtPage4.Controls.Add(this.label7);
            this.xtPage4.Controls.Add(this.label6);
            this.xtPage4.Controls.Add(this.label5);
            this.xtPage4.Name = "xtPage4";
            this.xtPage4.Size = new System.Drawing.Size(505, 207);
            this.xtPage4.Text = "Electronic Receipts";
            this.xtPage4.Tooltip = "Electronic Receipts";
            // 
            // chkRecepit
            // 
            this.chkRecepit.AutoSize = true;
            this.chkRecepit.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkRecepit.Location = new System.Drawing.Point(27, 192);
            this.chkRecepit.Name = "chkRecepit";
            this.chkRecepit.Size = new System.Drawing.Size(113, 17);
            this.chkRecepit.TabIndex = 11;
            this.chkRecepit.Text = "Import Receipt No";
            this.chkRecepit.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkRecepit.UseVisualStyleBackColor = true;
            // 
            // txtConfirmation
            // 
            this.txtConfirmation.Location = new System.Drawing.Point(166, 123);
            this.txtConfirmation.Multiline = true;
            this.txtConfirmation.Name = "txtConfirmation";
            this.txtConfirmation.Size = new System.Drawing.Size(269, 59);
            this.txtConfirmation.TabIndex = 10;
            // 
            // txtWebSite
            // 
            this.txtWebSite.Location = new System.Drawing.Point(166, 103);
            this.txtWebSite.Name = "txtWebSite";
            this.txtWebSite.Size = new System.Drawing.Size(269, 21);
            this.txtWebSite.TabIndex = 9;
            // 
            // txtShortname
            // 
            this.txtShortname.Location = new System.Drawing.Point(166, 83);
            this.txtShortname.Name = "txtShortname";
            this.txtShortname.Size = new System.Drawing.Size(136, 21);
            this.txtShortname.TabIndex = 8;
            // 
            // txtIssuing
            // 
            this.txtIssuing.Location = new System.Drawing.Point(166, 56);
            this.txtIssuing.Multiline = true;
            this.txtIssuing.Name = "txtIssuing";
            this.txtIssuing.Size = new System.Drawing.Size(269, 27);
            this.txtIssuing.TabIndex = 7;
            // 
            // txtReceipt
            // 
            this.txtReceipt.Location = new System.Drawing.Point(166, 21);
            this.txtReceipt.Multiline = true;
            this.txtReceipt.Name = "txtReceipt";
            this.txtReceipt.Size = new System.Drawing.Size(269, 35);
            this.txtReceipt.TabIndex = 6;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(27, 123);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(70, 13);
            this.label9.TabIndex = 4;
            this.label9.Text = "Comfirmation";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(27, 103);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(50, 13);
            this.label8.TabIndex = 3;
            this.label8.Text = "Web Site";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(27, 83);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(63, 13);
            this.label7.TabIndex = 2;
            this.label7.Text = "Short Name";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(27, 56);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(89, 13);
            this.label6.TabIndex = 1;
            this.label6.Text = "Issuing Authority";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(24, 21);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(99, 13);
            this.label5.TabIndex = 0;
            this.label5.Text = "Receipt Description";
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
            this.toolStrip.Size = new System.Drawing.Size(565, 37);
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
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 37);
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
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 37);
            // 
            // tsbDelete
            // 
            this.tsbDelete.Image = ((System.Drawing.Image)(resources.GetObject("tsbDelete.Image")));
            this.tsbDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbDelete.Name = "tsbDelete";
            this.tsbDelete.Size = new System.Drawing.Size(50, 34);
            this.tsbDelete.Tag = "003";
            this.tsbDelete.Text = "Delete";
            this.tsbDelete.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tsbDelete.ToolTipText = "Delete Selected Street Group";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 37);
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
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 37);
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
            // HelpProviderHG
            // 
            this.HelpProviderHG.HelpNamespace = "Bank_Reconciliation.chm";
            // 
            // FrmOptions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(573, 437);
            this.Controls.Add(this.panelContainer);
            this.HelpProviderHG.SetHelpKeyword(this, "FrmOptions.htm");
            this.HelpProviderHG.SetHelpNavigator(this, System.Windows.Forms.HelpNavigator.Topic);
            this.IsMdiContainer = true;
            this.Name = "FrmOptions";
            this.HelpProviderHG.SetShowHelp(this, true);
            this.Text = "FrmOptions";
            ((System.ComponentModel.ISupportInitialize)(this.panelContainer)).EndInit();
            this.panelContainer.ResumeLayout(false);
            this.panelContainer.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).EndInit();
            this.xtraTabControl1.ResumeLayout(false);
            this.xtPage1.ResumeLayout(false);
            this.xtPage1.PerformLayout();
            this.xtPage2.ResumeLayout(false);
            this.xtPage2.PerformLayout();
            this.xtPage3.ResumeLayout(false);
            this.xtPage3.PerformLayout();
            this.xtPage4.ResumeLayout(false);
            this.xtPage4.PerformLayout();
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.ResumeLayout(false);

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
        private System.Windows.Forms.SplitContainer splitContainer1;
        private DevExpress.XtraTab.XtraTabControl xtraTabControl1;
        private DevExpress.XtraTab.XtraTabPage xtPage1;
        private DevExpress.XtraTab.XtraTabPage xtPage2;
        private DevExpress.XtraTab.XtraTabPage xtPage3;
        private DevExpress.XtraTab.XtraTabPage xtPage4;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button bttnCancel;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Button bttnUpdate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cboRevenue;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtCode;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtShortname;
        private System.Windows.Forms.TextBox txtIssuing;
        private System.Windows.Forms.TextBox txtReceipt;
        private System.Windows.Forms.TextBox txtWebSite;
        private System.Windows.Forms.TextBox txtConfirmation;
        private System.Windows.Forms.CheckBox chkRecepit;
        private System.Windows.Forms.HelpProvider HelpProviderHG;

    }
}