namespace TaxDrive
{
    partial class FrmTaxStation
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmTaxStation));
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.txtDrivename = new DevExpress.XtraEditors.TextEdit();
            this.btnUpdate = new DevExpress.XtraEditors.SimpleButton();
            this.HelpProviderHG = new System.Windows.Forms.HelpProvider();
            ((System.ComponentModel.ISupportInitialize)(this.txtDrivename.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(23, 15);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(113, 13);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "Tax Drive Station Name";
            // 
            // txtDrivename
            // 
            this.HelpProviderHG.SetHelpKeyword(this.txtDrivename, "FrmTaxStation.htm#txtDrivename");
            this.HelpProviderHG.SetHelpNavigator(this.txtDrivename, System.Windows.Forms.HelpNavigator.Topic);
            this.txtDrivename.Location = new System.Drawing.Point(153, 12);
            this.txtDrivename.Name = "txtDrivename";
            this.txtDrivename.Properties.Mask.EditMask = "\\p{Lu}+";
            this.txtDrivename.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
            this.HelpProviderHG.SetShowHelp(this.txtDrivename, true);
            this.txtDrivename.Size = new System.Drawing.Size(140, 20);
            this.txtDrivename.TabIndex = 0;
            // 
            // btnUpdate
            // 
            this.HelpProviderHG.SetHelpKeyword(this.btnUpdate, "FrmTaxStation.htm#btnUpdate");
            this.HelpProviderHG.SetHelpNavigator(this.btnUpdate, System.Windows.Forms.HelpNavigator.Topic);
            this.btnUpdate.Image = ((System.Drawing.Image)(resources.GetObject("btnUpdate.Image")));
            this.btnUpdate.Location = new System.Drawing.Point(226, 38);
            this.btnUpdate.Name = "btnUpdate";
            this.HelpProviderHG.SetShowHelp(this.btnUpdate, true);
            this.btnUpdate.Size = new System.Drawing.Size(97, 30);
            this.btnUpdate.TabIndex = 1;
            this.btnUpdate.Text = "&Update";
            // 
            // HelpProviderHG
            // 
            this.HelpProviderHG.HelpNamespace = "Bank_Reconciliation.chm";
            // 
            // FrmTaxStation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(337, 85);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.txtDrivename);
            this.Controls.Add(this.labelControl1);
            this.HelpProviderHG.SetHelpKeyword(this, "FrmTaxStation.htm");
            this.HelpProviderHG.SetHelpNavigator(this, System.Windows.Forms.HelpNavigator.Topic);
            this.Name = "FrmTaxStation";
            this.HelpProviderHG.SetShowHelp(this, true);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Tax Drive Station Setup";
            ((System.ComponentModel.ISupportInitialize)(this.txtDrivename.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.TextEdit txtDrivename;
        private DevExpress.XtraEditors.SimpleButton btnUpdate;
        private System.Windows.Forms.HelpProvider HelpProviderHG;
    }
}