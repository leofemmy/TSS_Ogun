﻿namespace Control_Panel.Forms
{
    partial class FrmDeduction
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmDeduction));
            this.panelContainer = new DevExpress.XtraEditors.PanelControl();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.groupControl2 = new DevExpress.XtraEditors.GroupControl();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.cboClass = new System.Windows.Forms.ComboBox();
            this.cboSources = new System.Windows.Forms.ComboBox();
            this.txtName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.bttnReset = new System.Windows.Forms.Button();
            this.bttnUpdate = new System.Windows.Forms.Button();
            this.bttnCancel = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridView2 = new DevExpress.XtraGrid.Views.Grid.GridView();
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
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).BeginInit();
            this.groupControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).BeginInit();
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
            this.panelContainer.Size = new System.Drawing.Size(528, 392);
            this.panelContainer.TabIndex = 0;
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.splitContainer1);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupControl1.Location = new System.Drawing.Point(4, 41);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(489, 346);
            this.groupControl1.TabIndex = 5;
            this.groupControl1.Text = "Statutory Deduction";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.HelpProviderHG.SetHelpKeyword(this.splitContainer1, "FrmDeduction.htm#splitContainer1");
            this.HelpProviderHG.SetHelpNavigator(this.splitContainer1, System.Windows.Forms.HelpNavigator.Topic);
            this.splitContainer1.Location = new System.Drawing.Point(2, 21);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.groupControl2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.gridControl1);
            this.HelpProviderHG.SetShowHelp(this.splitContainer1, true);
            this.splitContainer1.Size = new System.Drawing.Size(485, 323);
            this.splitContainer1.SplitterDistance = 165;
            this.splitContainer1.TabIndex = 0;
            // 
            // groupControl2
            // 
            this.groupControl2.Controls.Add(this.label4);
            this.groupControl2.Controls.Add(this.textBox1);
            this.groupControl2.Controls.Add(this.cboClass);
            this.groupControl2.Controls.Add(this.cboSources);
            this.groupControl2.Controls.Add(this.txtName);
            this.groupControl2.Controls.Add(this.label3);
            this.groupControl2.Controls.Add(this.label13);
            this.groupControl2.Controls.Add(this.label12);
            this.groupControl2.Controls.Add(this.label11);
            this.groupControl2.Controls.Add(this.bttnReset);
            this.groupControl2.Controls.Add(this.bttnUpdate);
            this.groupControl2.Controls.Add(this.bttnCancel);
            this.groupControl2.Controls.Add(this.label2);
            this.groupControl2.Controls.Add(this.label1);
            this.groupControl2.Location = new System.Drawing.Point(3, 12);
            this.groupControl2.LookAndFeel.SkinName = "Money Twins";
            this.groupControl2.LookAndFeel.UseDefaultLookAndFeel = false;
            this.groupControl2.Name = "groupControl2";
            this.groupControl2.Size = new System.Drawing.Size(463, 134);
            this.groupControl2.TabIndex = 2;
            this.groupControl2.Text = "Add New Record";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(275, 33);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(45, 13);
            this.label4.TabIndex = 436;
            this.label4.Text = "Persons";
            // 
            // textBox1
            // 
            this.HelpProviderHG.SetHelpKeyword(this.textBox1, "FrmDeduction.htm#textBox1");
            this.HelpProviderHG.SetHelpNavigator(this.textBox1, System.Windows.Forms.HelpNavigator.Topic);
            this.textBox1.Location = new System.Drawing.Point(265, 48);
            this.textBox1.Name = "textBox1";
            this.HelpProviderHG.SetShowHelp(this.textBox1, true);
            this.textBox1.Size = new System.Drawing.Size(76, 21);
            this.textBox1.TabIndex = 435;
            // 
            // cboClass
            // 
            this.cboClass.FormattingEnabled = true;
            this.HelpProviderHG.SetHelpKeyword(this.cboClass, "FrmDeduction.htm#cboClass");
            this.HelpProviderHG.SetHelpNavigator(this.cboClass, System.Windows.Forms.HelpNavigator.Topic);
            this.cboClass.Location = new System.Drawing.Point(341, 48);
            this.cboClass.Name = "cboClass";
            this.HelpProviderHG.SetShowHelp(this.cboClass, true);
            this.cboClass.Size = new System.Drawing.Size(109, 21);
            this.cboClass.TabIndex = 434;
            // 
            // cboSources
            // 
            this.cboSources.FormattingEnabled = true;
            this.HelpProviderHG.SetHelpKeyword(this.cboSources, "FrmDeduction.htm#cboSources");
            this.HelpProviderHG.SetHelpNavigator(this.cboSources, System.Windows.Forms.HelpNavigator.Topic);
            this.cboSources.Location = new System.Drawing.Point(144, 48);
            this.cboSources.Name = "cboSources";
            this.HelpProviderHG.SetShowHelp(this.cboSources, true);
            this.cboSources.Size = new System.Drawing.Size(121, 21);
            this.cboSources.TabIndex = 433;
            // 
            // txtName
            // 
            this.HelpProviderHG.SetHelpKeyword(this.txtName, "FrmDeduction.htm#txtName");
            this.HelpProviderHG.SetHelpNavigator(this.txtName, System.Windows.Forms.HelpNavigator.Topic);
            this.txtName.Location = new System.Drawing.Point(5, 48);
            this.txtName.Name = "txtName";
            this.HelpProviderHG.SetShowHelp(this.txtName, true);
            this.txtName.Size = new System.Drawing.Size(140, 21);
            this.txtName.TabIndex = 432;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(354, 33);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(52, 13);
            this.label3.TabIndex = 431;
            this.label3.Text = "Limitation";
            // 
            // label13
            // 
            this.label13.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(333, 114);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(41, 14);
            this.label13.TabIndex = 430;
            this.label13.Text = "&Update";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label12
            // 
            this.label12.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(375, 114);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(36, 14);
            this.label12.TabIndex = 429;
            this.label12.Text = "&Reset";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label11
            // 
            this.label11.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(412, 114);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(40, 14);
            this.label11.TabIndex = 428;
            this.label11.Text = "&Cancel";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // bttnReset
            // 
            this.bttnReset.Cursor = System.Windows.Forms.Cursors.Hand;
            this.HelpProviderHG.SetHelpKeyword(this.bttnReset, "FrmDeduction.htm#bttnReset");
            this.HelpProviderHG.SetHelpNavigator(this.bttnReset, System.Windows.Forms.HelpNavigator.Topic);
            this.bttnReset.Location = new System.Drawing.Point(377, 74);
            this.bttnReset.Name = "bttnReset";
            this.HelpProviderHG.SetShowHelp(this.bttnReset, true);
            this.bttnReset.Size = new System.Drawing.Size(34, 37);
            this.bttnReset.TabIndex = 426;
            this.bttnReset.UseVisualStyleBackColor = true;
            // 
            // bttnUpdate
            // 
            this.bttnUpdate.Cursor = System.Windows.Forms.Cursors.Hand;
            this.HelpProviderHG.SetHelpKeyword(this.bttnUpdate, "FrmDeduction.htm#bttnUpdate");
            this.HelpProviderHG.SetHelpNavigator(this.bttnUpdate, System.Windows.Forms.HelpNavigator.Topic);
            this.bttnUpdate.Location = new System.Drawing.Point(337, 74);
            this.bttnUpdate.Name = "bttnUpdate";
            this.HelpProviderHG.SetShowHelp(this.bttnUpdate, true);
            this.bttnUpdate.Size = new System.Drawing.Size(34, 37);
            this.bttnUpdate.TabIndex = 425;
            this.bttnUpdate.UseVisualStyleBackColor = true;
            // 
            // bttnCancel
            // 
            this.bttnCancel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.bttnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.HelpProviderHG.SetHelpKeyword(this.bttnCancel, "FrmDeduction.htm#bttnCancel");
            this.HelpProviderHG.SetHelpNavigator(this.bttnCancel, System.Windows.Forms.HelpNavigator.Topic);
            this.bttnCancel.Location = new System.Drawing.Point(416, 74);
            this.bttnCancel.Name = "bttnCancel";
            this.HelpProviderHG.SetShowHelp(this.bttnCancel, true);
            this.bttnCancel.Size = new System.Drawing.Size(34, 37);
            this.bttnCancel.TabIndex = 427;
            this.bttnCancel.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(148, 33);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Status";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(26, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(85, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Deduction Name";
            // 
            // gridControl1
            // 
            this.gridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.HelpProviderHG.SetHelpKeyword(this.gridControl1, "FrmDeduction.htm#gridControl1");
            this.HelpProviderHG.SetHelpNavigator(this.gridControl1, System.Windows.Forms.HelpNavigator.Topic);
            this.gridControl1.Location = new System.Drawing.Point(0, 0);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.Name = "gridControl1";
            this.HelpProviderHG.SetShowHelp(this.gridControl1, true);
            this.gridControl1.Size = new System.Drawing.Size(485, 154);
            this.gridControl1.TabIndex = 4;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1,
            this.gridView2});
            // 
            // gridView1
            // 
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsView.ShowGroupPanel = false;
            // 
            // gridView2
            // 
            this.gridView2.GridControl = this.gridControl1;
            this.gridView2.Name = "gridView2";
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
            this.toolStrip.Size = new System.Drawing.Size(520, 37);
            this.toolStrip.TabIndex = 4;
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
            this.tsbEdit.Tag = "001";
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
            // FrmDeduction
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(528, 392);
            this.Controls.Add(this.panelContainer);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.HelpProviderHG.SetHelpKeyword(this, "FrmDeduction.htm");
            this.HelpProviderHG.SetHelpNavigator(this, System.Windows.Forms.HelpNavigator.Topic);
            this.Name = "FrmDeduction";
            this.HelpProviderHG.SetShowHelp(this, true);
            this.Text = "FrmDeduction";
            ((System.ComponentModel.ISupportInitialize)(this.panelContainer)).EndInit();
            this.panelContainer.ResumeLayout(false);
            this.panelContainer.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).EndInit();
            this.groupControl2.ResumeLayout(false);
            this.groupControl2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).EndInit();
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

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
        private DevExpress.XtraEditors.GroupControl groupControl2;
        private System.Windows.Forms.ComboBox cboClass;
        private System.Windows.Forms.ComboBox cboSources;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button bttnReset;
        private System.Windows.Forms.Button bttnUpdate;
        private System.Windows.Forms.Button bttnCancel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBox1;
        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView2;
        public DevExpress.XtraEditors.PanelControl panelContainer;
        private System.Windows.Forms.HelpProvider HelpProviderHG;
    }
}