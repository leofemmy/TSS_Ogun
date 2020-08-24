namespace Collection.Forms
{
    partial class FrmModification
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmModification));
            this.panelContainer = new DevExpress.XtraEditors.PanelControl();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.cboSelect = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtreason = new System.Windows.Forms.TextBox();
            this.label46 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.txtpayaddress = new System.Windows.Forms.TextBox();
            this.label45 = new System.Windows.Forms.Label();
            this.txtAgcode = new System.Windows.Forms.TextBox();
            this.txtpayername = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cboRevenue = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.layoutView1 = new DevExpress.XtraGrid.Views.Layout.LayoutView();
            this.layoutViewCard1 = new DevExpress.XtraGrid.Views.Layout.LayoutViewCard();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnSearch = new System.Windows.Forms.Button();
            this.txtsearchpay = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox13 = new System.Windows.Forms.GroupBox();
            this.radioGroup3 = new DevExpress.XtraEditors.RadioGroup();
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
            this.groupBox3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutViewCard1)).BeginInit();
            this.groupBox13.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radioGroup3.Properties)).BeginInit();
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
            this.panelContainer.Size = new System.Drawing.Size(856, 624);
            this.panelContainer.TabIndex = 1;
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.splitContainer1);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupControl1.Location = new System.Drawing.Point(4, 41);
            this.groupControl1.LookAndFeel.SkinName = "Money Twins";
            this.groupControl1.LookAndFeel.UseDefaultLookAndFeel = false;
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(825, 578);
            this.groupControl1.TabIndex = 8;
            this.groupControl1.Text = "Payment Modification";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.HelpProviderHG.SetHelpKeyword(this.splitContainer1, "FrmModification.htm#splitContainer1");
            this.HelpProviderHG.SetHelpNavigator(this.splitContainer1, System.Windows.Forms.HelpNavigator.Topic);
            this.splitContainer1.Location = new System.Drawing.Point(2, 20);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.cboSelect);
            this.splitContainer1.Panel1.Controls.Add(this.label5);
            this.splitContainer1.Panel1.Controls.Add(this.txtreason);
            this.splitContainer1.Panel1.Controls.Add(this.label46);
            this.splitContainer1.Panel1.Controls.Add(this.label9);
            this.splitContainer1.Panel1.Controls.Add(this.btnUpdate);
            this.splitContainer1.Panel1.Controls.Add(this.groupBox3);
            this.splitContainer1.Panel1.Controls.Add(this.groupBox1);
            this.splitContainer1.Panel1.Controls.Add(this.label7);
            this.splitContainer1.Panel1.Controls.Add(this.label8);
            this.splitContainer1.Panel1.Controls.Add(this.label3);
            this.splitContainer1.Panel1.Controls.Add(this.btnSearch);
            this.splitContainer1.Panel1.Controls.Add(this.txtsearchpay);
            this.splitContainer1.Panel1.Controls.Add(this.label6);
            this.splitContainer1.Panel1.Controls.Add(this.groupBox13);
            this.HelpProviderHG.SetShowHelp(this.splitContainer1, true);
            this.splitContainer1.Size = new System.Drawing.Size(821, 556);
            this.splitContainer1.SplitterDistance = 503;
            this.splitContainer1.TabIndex = 0;
            // 
            // cboSelect
            // 
            this.cboSelect.FormattingEnabled = true;
            this.cboSelect.Location = new System.Drawing.Point(561, 27);
            this.cboSelect.Name = "cboSelect";
            this.cboSelect.Size = new System.Drawing.Size(205, 21);
            this.cboSelect.TabIndex = 44;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(429, 25);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(123, 13);
            this.label5.TabIndex = 43;
            this.label5.Text = "Select Modification Type";
            // 
            // txtreason
            // 
            this.HelpProviderHG.SetHelpKeyword(this.txtreason, "FrmModification.htm#txtreason");
            this.HelpProviderHG.SetHelpNavigator(this.txtreason, System.Windows.Forms.HelpNavigator.Topic);
            this.txtreason.Location = new System.Drawing.Point(517, 313);
            this.txtreason.Multiline = true;
            this.txtreason.Name = "txtreason";
            this.HelpProviderHG.SetShowHelp(this.txtreason, true);
            this.txtreason.Size = new System.Drawing.Size(286, 36);
            this.txtreason.TabIndex = 42;
            // 
            // label46
            // 
            this.label46.AutoSize = true;
            this.label46.Location = new System.Drawing.Point(464, 316);
            this.label46.Name = "label46";
            this.label46.Size = new System.Drawing.Size(47, 13);
            this.label46.TabIndex = 41;
            this.label46.Text = "Reason:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(525, 392);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(42, 13);
            this.label9.TabIndex = 40;
            this.label9.Text = "&Update";
            // 
            // btnUpdate
            // 
            this.HelpProviderHG.SetHelpKeyword(this.btnUpdate, "FrmModification.htm#btnUpdate");
            this.HelpProviderHG.SetHelpNavigator(this.btnUpdate, System.Windows.Forms.HelpNavigator.Topic);
            this.btnUpdate.Location = new System.Drawing.Point(517, 356);
            this.btnUpdate.Name = "btnUpdate";
            this.HelpProviderHG.SetShowHelp(this.btnUpdate, true);
            this.btnUpdate.Size = new System.Drawing.Size(50, 33);
            this.btnUpdate.TabIndex = 39;
            this.btnUpdate.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.txtpayaddress);
            this.groupBox3.Controls.Add(this.label45);
            this.groupBox3.Controls.Add(this.txtAgcode);
            this.groupBox3.Controls.Add(this.txtpayername);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.cboRevenue);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Location = new System.Drawing.Point(464, 176);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(339, 133);
            this.groupBox3.TabIndex = 38;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Records Modifier";
            // 
            // txtpayaddress
            // 
            this.txtpayaddress.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.txtpayaddress.Enabled = false;
            this.txtpayaddress.ForeColor = System.Drawing.SystemColors.WindowText;
            this.HelpProviderHG.SetHelpKeyword(this.txtpayaddress, "FrmModification.htm#txtpayaddress");
            this.HelpProviderHG.SetHelpNavigator(this.txtpayaddress, System.Windows.Forms.HelpNavigator.Topic);
            this.txtpayaddress.Location = new System.Drawing.Point(97, 53);
            this.txtpayaddress.Multiline = true;
            this.txtpayaddress.Name = "txtpayaddress";
            this.HelpProviderHG.SetShowHelp(this.txtpayaddress, true);
            this.txtpayaddress.Size = new System.Drawing.Size(227, 44);
            this.txtpayaddress.TabIndex = 16;
            // 
            // label45
            // 
            this.label45.AutoSize = true;
            this.label45.Location = new System.Drawing.Point(9, 73);
            this.label45.Name = "label45";
            this.label45.Size = new System.Drawing.Size(81, 13);
            this.label45.TabIndex = 15;
            this.label45.Text = "Payer Address:";
            // 
            // txtAgcode
            // 
            this.HelpProviderHG.SetHelpKeyword(this.txtAgcode, "FrmModification.htm#txtAgcode");
            this.HelpProviderHG.SetHelpNavigator(this.txtAgcode, System.Windows.Forms.HelpNavigator.Topic);
            this.txtAgcode.Location = new System.Drawing.Point(97, 93);
            this.txtAgcode.Name = "txtAgcode";
            this.HelpProviderHG.SetShowHelp(this.txtAgcode, true);
            this.txtAgcode.Size = new System.Drawing.Size(180, 21);
            this.txtAgcode.TabIndex = 14;
            this.txtAgcode.Visible = false;
            // 
            // txtpayername
            // 
            this.txtpayername.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.txtpayername.Enabled = false;
            this.HelpProviderHG.SetHelpKeyword(this.txtpayername, "FrmModification.htm#txtpayername");
            this.HelpProviderHG.SetHelpNavigator(this.txtpayername, System.Windows.Forms.HelpNavigator.Topic);
            this.txtpayername.Location = new System.Drawing.Point(97, 30);
            this.txtpayername.Multiline = true;
            this.txtpayername.Name = "txtpayername";
            this.HelpProviderHG.SetShowHelp(this.txtpayername, true);
            this.txtpayername.Size = new System.Drawing.Size(227, 29);
            this.txtpayername.TabIndex = 13;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 100);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(75, 13);
            this.label4.TabIndex = 12;
            this.label4.Text = "Agency Code:";
            this.label4.Visible = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 49);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(69, 13);
            this.label2.TabIndex = 11;
            this.label2.Text = "Payer Name:";
            // 
            // cboRevenue
            // 
            this.cboRevenue.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.cboRevenue.Enabled = false;
            this.cboRevenue.FormattingEnabled = true;
            this.HelpProviderHG.SetHelpKeyword(this.cboRevenue, "FrmModification.htm#cboRevenue");
            this.HelpProviderHG.SetHelpNavigator(this.cboRevenue, System.Windows.Forms.HelpNavigator.Topic);
            this.cboRevenue.Location = new System.Drawing.Point(97, 12);
            this.cboRevenue.Name = "cboRevenue";
            this.HelpProviderHG.SetShowHelp(this.cboRevenue, true);
            this.cboRevenue.Size = new System.Drawing.Size(227, 21);
            this.cboRevenue.TabIndex = 10;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "Revenue Code:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.gridControl1);
            this.groupBox1.Location = new System.Drawing.Point(22, 76);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(425, 356);
            this.groupBox1.TabIndex = 37;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Transaction Details";
            // 
            // gridControl1
            // 
            this.gridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.HelpProviderHG.SetHelpKeyword(this.gridControl1, "FrmModification.htm#gridControl1");
            this.HelpProviderHG.SetHelpNavigator(this.gridControl1, System.Windows.Forms.HelpNavigator.Topic);
            this.gridControl1.Location = new System.Drawing.Point(3, 17);
            this.gridControl1.MainView = this.layoutView1;
            this.gridControl1.Name = "gridControl1";
            this.HelpProviderHG.SetShowHelp(this.gridControl1, true);
            this.gridControl1.Size = new System.Drawing.Size(419, 336);
            this.gridControl1.TabIndex = 43;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.layoutView1});
            // 
            // layoutView1
            // 
            this.layoutView1.Appearance.Card.Options.UseTextOptions = true;
            this.layoutView1.Appearance.Card.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.layoutView1.Appearance.FieldCaption.Options.UseTextOptions = true;
            this.layoutView1.Appearance.FieldCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.layoutView1.Appearance.FieldValue.Options.UseTextOptions = true;
            this.layoutView1.Appearance.FieldValue.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.layoutView1.CardMinSize = new System.Drawing.Size(400, 20);
            this.layoutView1.GridControl = this.gridControl1;
            this.layoutView1.Name = "layoutView1";
            this.layoutView1.OptionsBehavior.Editable = false;
            this.layoutView1.OptionsView.ShowHeaderPanel = false;
            this.layoutView1.TemplateCard = this.layoutViewCard1;
            // 
            // layoutViewCard1
            // 
            this.layoutViewCard1.HeaderButtonsLocation = DevExpress.Utils.GroupElementLocation.AfterText;
            this.layoutViewCard1.Name = "layoutViewCard1";
            // 
            // label7
            // 
            this.label7.AllowDrop = true;
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(464, 59);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(13, 14);
            this.label7.TabIndex = 36;
            this.label7.Text = "+";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(356, 60);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(40, 13);
            this.label8.TabIndex = 35;
            this.label8.Text = "&Search";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.Red;
            this.label3.Location = new System.Drawing.Point(142, 40);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(180, 13);
            this.label3.TabIndex = 34;
            this.label3.Text = "(e.g.ABP|OGPD|9-09-2013|324909)";
            // 
            // btnSearch
            // 
            this.HelpProviderHG.SetHelpKeyword(this.btnSearch, "FrmModification.htm#btnSearch");
            this.HelpProviderHG.SetHelpNavigator(this.btnSearch, System.Windows.Forms.HelpNavigator.Topic);
            this.btnSearch.Location = new System.Drawing.Point(347, 15);
            this.btnSearch.Name = "btnSearch";
            this.HelpProviderHG.SetShowHelp(this.btnSearch, true);
            this.btnSearch.Size = new System.Drawing.Size(59, 42);
            this.btnSearch.TabIndex = 33;
            this.btnSearch.UseVisualStyleBackColor = true;
            // 
            // txtsearchpay
            // 
            this.HelpProviderHG.SetHelpKeyword(this.txtsearchpay, "FrmModification.htm#txtsearchpay");
            this.HelpProviderHG.SetHelpNavigator(this.txtsearchpay, System.Windows.Forms.HelpNavigator.Topic);
            this.txtsearchpay.Location = new System.Drawing.Point(130, 17);
            this.txtsearchpay.Name = "txtsearchpay";
            this.HelpProviderHG.SetShowHelp(this.txtsearchpay, true);
            this.txtsearchpay.Size = new System.Drawing.Size(211, 21);
            this.txtsearchpay.TabIndex = 32;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(19, 17);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(106, 13);
            this.label6.TabIndex = 31;
            this.label6.Text = "Payment Reference:";
            // 
            // groupBox13
            // 
            this.groupBox13.Controls.Add(this.radioGroup3);
            this.groupBox13.Location = new System.Drawing.Point(561, 76);
            this.groupBox13.Name = "groupBox13";
            this.groupBox13.Size = new System.Drawing.Size(192, 94);
            this.groupBox13.TabIndex = 30;
            this.groupBox13.TabStop = false;
            this.groupBox13.Text = "Type";
            this.groupBox13.Visible = false;
            // 
            // radioGroup3
            // 
            this.HelpProviderHG.SetHelpKeyword(this.radioGroup3, "FrmModification.htm#radioGroup3");
            this.HelpProviderHG.SetHelpNavigator(this.radioGroup3, System.Windows.Forms.HelpNavigator.Topic);
            this.radioGroup3.Location = new System.Drawing.Point(11, 19);
            this.radioGroup3.Name = "radioGroup3";
            this.radioGroup3.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem(3, "Modify Payer Address"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(2, "Modify Payer Name"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(1, "Modify Revenue Type")});
            this.HelpProviderHG.SetShowHelp(this.radioGroup3, true);
            this.radioGroup3.Size = new System.Drawing.Size(160, 83);
            this.radioGroup3.TabIndex = 6;
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
            this.toolStrip.Size = new System.Drawing.Size(848, 37);
            this.toolStrip.TabIndex = 7;
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
            this.tsbNew.Visible = false;
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 37);
            this.toolStripSeparator1.Visible = false;
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
            this.tsbEdit.Visible = false;
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 37);
            this.toolStripSeparator2.Visible = false;
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
            this.tsbDelete.Visible = false;
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 37);
            this.toolStripSeparator3.Visible = false;
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
            this.tsbReload.Visible = false;
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 37);
            this.toolStripSeparator4.Visible = false;
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
            // FrmModification
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(856, 624);
            this.Controls.Add(this.panelContainer);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.HelpProviderHG.SetHelpKeyword(this, "FrmModification.htm");
            this.HelpProviderHG.SetHelpNavigator(this, System.Windows.Forms.HelpNavigator.Topic);
            this.Name = "FrmModification";
            this.HelpProviderHG.SetShowHelp(this, true);
            this.Text = "FrmModification";
            ((System.ComponentModel.ISupportInitialize)(this.panelContainer)).EndInit();
            this.panelContainer.ResumeLayout(false);
            this.panelContainer.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutViewCard1)).EndInit();
            this.groupBox13.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.radioGroup3.Properties)).EndInit();
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        public DevExpress.XtraEditors.PanelControl panelContainer;
        private DevExpress.XtraEditors.GroupControl groupControl1;
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
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.GroupBox groupBox13;
        private DevExpress.XtraEditors.RadioGroup radioGroup3;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.TextBox txtsearchpay;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtreason;
        private System.Windows.Forms.Label label46;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox txtpayaddress;
        private System.Windows.Forms.Label label45;
        private System.Windows.Forms.TextBox txtAgcode;
        private System.Windows.Forms.TextBox txtpayername;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cboRevenue;
        private System.Windows.Forms.Label label1;
        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Layout.LayoutView layoutView1;
        private DevExpress.XtraGrid.Views.Layout.LayoutViewCard layoutViewCard1;
        private System.Windows.Forms.HelpProvider HelpProviderHG;
        private System.Windows.Forms.ComboBox cboSelect;
        private System.Windows.Forms.Label label5;
    }
}