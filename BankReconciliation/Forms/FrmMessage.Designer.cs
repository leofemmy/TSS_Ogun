namespace BankReconciliation.Forms
{
    partial class FrmMessage
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
            this.groupBox13 = new System.Windows.Forms.GroupBox();
            this.radioGroup3 = new DevExpress.XtraEditors.RadioGroup();
            this.HelpProviderHG = new System.Windows.Forms.HelpProvider();
            this.groupBox13.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radioGroup3.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox13
            // 
            this.groupBox13.Controls.Add(this.radioGroup3);
            this.groupBox13.Location = new System.Drawing.Point(12, 12);
            this.groupBox13.Name = "groupBox13";
            this.groupBox13.Size = new System.Drawing.Size(232, 79);
            this.groupBox13.TabIndex = 450;
            this.groupBox13.TabStop = false;
            this.groupBox13.Text = "Excel Type";
            // 
            // radioGroup3
            // 
            this.HelpProviderHG.SetHelpKeyword(this.radioGroup3, "FrmMessage.htm#radioGroup3");
            this.HelpProviderHG.SetHelpNavigator(this.radioGroup3, System.Windows.Forms.HelpNavigator.Topic);
            this.radioGroup3.Location = new System.Drawing.Point(11, 19);
            this.radioGroup3.Name = "radioGroup3";
            this.radioGroup3.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem(1, "Excel 2003"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(2, "Excel 2007")});
            this.HelpProviderHG.SetShowHelp(this.radioGroup3, true);
            this.radioGroup3.Size = new System.Drawing.Size(194, 21);
            this.radioGroup3.TabIndex = 6;
            // 
            // HelpProviderHG
            // 
            this.HelpProviderHG.HelpNamespace = "Bank_Reconciliation.chm";
            // 
            // FrmMessage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(278, 121);
            this.Controls.Add(this.groupBox13);
            this.HelpProviderHG.SetHelpKeyword(this, "FrmMessage.htm");
            this.HelpProviderHG.SetHelpNavigator(this, System.Windows.Forms.HelpNavigator.Topic);
            this.Name = "FrmMessage";
            this.HelpProviderHG.SetShowHelp(this, true);
            this.Text = "FrmMessage";
            this.groupBox13.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.radioGroup3.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox13;
        private DevExpress.XtraEditors.RadioGroup radioGroup3;
        private System.Windows.Forms.HelpProvider HelpProviderHG;
    }
}