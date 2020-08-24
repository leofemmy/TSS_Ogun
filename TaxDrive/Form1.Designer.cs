namespace TaxDrive
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
            this.sbnReport = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.dtpDate2 = new System.Windows.Forms.DateTimePicker();
            this.dtpDate = new System.Windows.Forms.DateTimePicker();
            this.sbnUpdate = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.txtsearch = new DevExpress.XtraEditors.TextEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.radioGroup1 = new DevExpress.XtraEditors.RadioGroup();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.sbClose = new DevExpress.XtraEditors.SimpleButton();
            this.sbUpdate = new DevExpress.XtraEditors.SimpleButton();
            this.HelpProviderHG = new System.Windows.Forms.HelpProvider();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtsearch.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radioGroup1.Properties)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.sbnReport);
            this.groupBox1.Controls.Add(this.labelControl3);
            this.groupBox1.Controls.Add(this.dtpDate2);
            this.groupBox1.Controls.Add(this.dtpDate);
            this.groupBox1.Controls.Add(this.sbnUpdate);
            this.groupBox1.Controls.Add(this.labelControl2);
            this.groupBox1.Controls.Add(this.txtsearch);
            this.groupBox1.Controls.Add(this.labelControl1);
            this.groupBox1.Controls.Add(this.radioGroup1);
            this.groupBox1.Location = new System.Drawing.Point(12, 64);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(573, 101);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Search Records";
            // 
            // sbnReport
            // 
            this.HelpProviderHG.SetHelpKeyword(this.sbnReport, "Form1_2.htm#sbnReport");
            this.HelpProviderHG.SetHelpNavigator(this.sbnReport, System.Windows.Forms.HelpNavigator.Topic);
            this.sbnReport.Image = ((System.Drawing.Image)(resources.GetObject("sbnReport.Image")));
            this.sbnReport.Location = new System.Drawing.Point(488, 51);
            this.sbnReport.Name = "sbnReport";
            this.HelpProviderHG.SetShowHelp(this.sbnReport, true);
            this.sbnReport.Size = new System.Drawing.Size(79, 26);
            this.sbnReport.TabIndex = 9;
            this.sbnReport.Text = "&Report";
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(250, 55);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(16, 13);
            this.labelControl3.TabIndex = 8;
            this.labelControl3.Text = "++";
            this.labelControl3.Visible = false;
            // 
            // dtpDate2
            // 
            this.dtpDate2.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.HelpProviderHG.SetHelpKeyword(this.dtpDate2, "Form1_2.htm#dtpDate2");
            this.HelpProviderHG.SetHelpNavigator(this.dtpDate2, System.Windows.Forms.HelpNavigator.Topic);
            this.dtpDate2.Location = new System.Drawing.Point(286, 49);
            this.dtpDate2.Name = "dtpDate2";
            this.HelpProviderHG.SetShowHelp(this.dtpDate2, true);
            this.dtpDate2.Size = new System.Drawing.Size(100, 20);
            this.dtpDate2.TabIndex = 7;
            this.dtpDate2.Visible = false;
            // 
            // dtpDate
            // 
            this.dtpDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.HelpProviderHG.SetHelpKeyword(this.dtpDate, "Form1_2.htm#dtpDate");
            this.HelpProviderHG.SetHelpNavigator(this.dtpDate, System.Windows.Forms.HelpNavigator.Topic);
            this.dtpDate.Location = new System.Drawing.Point(135, 49);
            this.dtpDate.Name = "dtpDate";
            this.HelpProviderHG.SetShowHelp(this.dtpDate, true);
            this.dtpDate.Size = new System.Drawing.Size(100, 20);
            this.dtpDate.TabIndex = 6;
            this.dtpDate.Visible = false;
            // 
            // sbnUpdate
            // 
            this.HelpProviderHG.SetHelpKeyword(this.sbnUpdate, "Form1_2.htm#sbnUpdate");
            this.HelpProviderHG.SetHelpNavigator(this.sbnUpdate, System.Windows.Forms.HelpNavigator.Topic);
            this.sbnUpdate.Image = ((System.Drawing.Image)(resources.GetObject("sbnUpdate.Image")));
            this.sbnUpdate.Location = new System.Drawing.Point(403, 51);
            this.sbnUpdate.Name = "sbnUpdate";
            this.HelpProviderHG.SetShowHelp(this.sbnUpdate, true);
            this.sbnUpdate.Size = new System.Drawing.Size(79, 26);
            this.sbnUpdate.TabIndex = 5;
            this.sbnUpdate.Text = "&Search";
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(168, 82);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(16, 13);
            this.labelControl2.TabIndex = 4;
            this.labelControl2.Text = "++";
            this.labelControl2.Visible = false;
            // 
            // txtsearch
            // 
            this.HelpProviderHG.SetHelpKeyword(this.txtsearch, "Form1_2.htm#txtsearch");
            this.HelpProviderHG.SetHelpNavigator(this.txtsearch, System.Windows.Forms.HelpNavigator.Topic);
            this.txtsearch.Location = new System.Drawing.Point(135, 49);
            this.txtsearch.Name = "txtsearch";
            this.HelpProviderHG.SetShowHelp(this.txtsearch, true);
            this.txtsearch.Size = new System.Drawing.Size(240, 20);
            this.txtsearch.TabIndex = 2;
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(6, 51);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(16, 13);
            this.labelControl1.TabIndex = 1;
            this.labelControl1.Text = "++";
            // 
            // radioGroup1
            // 
            this.HelpProviderHG.SetHelpKeyword(this.radioGroup1, "Form1_2.htm#radioGroup1");
            this.HelpProviderHG.SetHelpNavigator(this.radioGroup1, System.Windows.Forms.HelpNavigator.Topic);
            this.radioGroup1.Location = new System.Drawing.Point(6, 15);
            this.radioGroup1.Name = "radioGroup1";
            this.radioGroup1.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem(1, "Payment Ref. Number"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(2, "Payment Date"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(3, "Payer Name")});
            this.HelpProviderHG.SetShowHelp(this.radioGroup1, true);
            this.radioGroup1.Size = new System.Drawing.Size(453, 30);
            this.radioGroup1.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.gridControl1);
            this.groupBox2.Location = new System.Drawing.Point(12, 186);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(582, 350);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            // 
            // gridControl1
            // 
            this.gridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.HelpProviderHG.SetHelpKeyword(this.gridControl1, "Form1_2.htm#gridControl1");
            this.HelpProviderHG.SetHelpNavigator(this.gridControl1, System.Windows.Forms.HelpNavigator.Topic);
            this.gridControl1.Location = new System.Drawing.Point(3, 16);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.Name = "gridControl1";
            this.HelpProviderHG.SetShowHelp(this.gridControl1, true);
            this.gridControl1.Size = new System.Drawing.Size(576, 331);
            this.gridControl1.TabIndex = 1;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsView.ShowGroupPanel = false;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.sbClose);
            this.groupBox3.Controls.Add(this.sbUpdate);
            this.groupBox3.Location = new System.Drawing.Point(12, 3);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(579, 55);
            this.groupBox3.TabIndex = 4;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Download Record and Exit Application";
            // 
            // sbClose
            // 
            this.HelpProviderHG.SetHelpKeyword(this.sbClose, "Form1_2.htm#sbClose");
            this.HelpProviderHG.SetHelpNavigator(this.sbClose, System.Windows.Forms.HelpNavigator.Topic);
            this.sbClose.Image = ((System.Drawing.Image)(resources.GetObject("sbClose.Image")));
            this.sbClose.Location = new System.Drawing.Point(423, 19);
            this.sbClose.Name = "sbClose";
            this.HelpProviderHG.SetShowHelp(this.sbClose, true);
            this.sbClose.Size = new System.Drawing.Size(99, 30);
            this.sbClose.TabIndex = 3;
            this.sbClose.Text = "&Close";
            // 
            // sbUpdate
            // 
            this.HelpProviderHG.SetHelpKeyword(this.sbUpdate, "Form1_2.htm#sbUpdate");
            this.HelpProviderHG.SetHelpNavigator(this.sbUpdate, System.Windows.Forms.HelpNavigator.Topic);
            this.sbUpdate.Image = ((System.Drawing.Image)(resources.GetObject("sbUpdate.Image")));
            this.sbUpdate.Location = new System.Drawing.Point(63, 19);
            this.sbUpdate.Name = "sbUpdate";
            this.HelpProviderHG.SetShowHelp(this.sbUpdate, true);
            this.sbUpdate.Size = new System.Drawing.Size(99, 30);
            this.sbUpdate.TabIndex = 2;
            this.sbUpdate.Text = "&Update";
            // 
            // HelpProviderHG
            // 
            this.HelpProviderHG.HelpNamespace = "Bank_Reconciliation.chm";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(606, 548);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.HelpProviderHG.SetHelpKeyword(this, "Form1_2.htm");
            this.HelpProviderHG.SetHelpNavigator(this, System.Windows.Forms.HelpNavigator.Topic);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.HelpProviderHG.SetShowHelp(this, true);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtsearch.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radioGroup1.Properties)).EndInit();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private DevExpress.XtraEditors.RadioGroup radioGroup1;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.TextEdit txtsearch;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.SimpleButton sbnUpdate;
        private System.Windows.Forms.DateTimePicker dtpDate;
        private System.Windows.Forms.DateTimePicker dtpDate2;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.SimpleButton sbnReport;
        private System.Windows.Forms.GroupBox groupBox2;
        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private System.Windows.Forms.GroupBox groupBox3;
        private DevExpress.XtraEditors.SimpleButton sbUpdate;
        private DevExpress.XtraEditors.SimpleButton sbClose;
        private System.Windows.Forms.HelpProvider HelpProviderHG;
    }
}

