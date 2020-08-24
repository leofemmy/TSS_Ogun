namespace Collection.Forms
{
    partial class FrmLogin
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmLogin));
            this.BttnOK = new DevExpress.XtraEditors.SimpleButton();
            this.BttnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.L_Move = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.lblAttempt = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.HelpProviderHG = new System.Windows.Forms.HelpProvider();
            this.SuspendLayout();
            // 
            // BttnOK
            // 
            this.HelpProviderHG.SetHelpKeyword(this.BttnOK, "FrmLogin_2.htm#BttnOK");
            this.HelpProviderHG.SetHelpNavigator(this.BttnOK, System.Windows.Forms.HelpNavigator.Topic);
            this.BttnOK.Location = new System.Drawing.Point(282, 218);
            this.BttnOK.LookAndFeel.SkinName = "Blue";
            this.BttnOK.LookAndFeel.UseDefaultLookAndFeel = false;
            this.BttnOK.Name = "BttnOK";
            this.HelpProviderHG.SetShowHelp(this.BttnOK, true);
            this.BttnOK.Size = new System.Drawing.Size(75, 23);
            this.BttnOK.TabIndex = 0;
            this.BttnOK.Text = "&OK";
            // 
            // BttnCancel
            // 
            this.BttnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.HelpProviderHG.SetHelpKeyword(this.BttnCancel, "FrmLogin_2.htm#BttnCancel");
            this.HelpProviderHG.SetHelpNavigator(this.BttnCancel, System.Windows.Forms.HelpNavigator.Topic);
            this.BttnCancel.Location = new System.Drawing.Point(372, 218);
            this.BttnCancel.LookAndFeel.SkinName = "Blue";
            this.BttnCancel.LookAndFeel.UseDefaultLookAndFeel = false;
            this.BttnCancel.Name = "BttnCancel";
            this.HelpProviderHG.SetShowHelp(this.BttnCancel, true);
            this.BttnCancel.Size = new System.Drawing.Size(75, 23);
            this.BttnCancel.TabIndex = 1;
            this.BttnCancel.Text = "&Cancel";
            // 
            // txtUsername
            // 
            this.txtUsername.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtUsername.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.HelpProviderHG.SetHelpKeyword(this.txtUsername, "FrmLogin_2.htm#L_Move");
            this.HelpProviderHG.SetHelpNavigator(this.txtUsername, System.Windows.Forms.HelpNavigator.Topic);
            this.txtUsername.Location = new System.Drawing.Point(284, 112);
            this.txtUsername.Margin = new System.Windows.Forms.Padding(4, 7, 4, 7);
            this.txtUsername.Name = "txtUsername";
            this.HelpProviderHG.SetShowHelp(this.txtUsername, true);
            this.txtUsername.Size = new System.Drawing.Size(155, 24);
            this.txtUsername.TabIndex = 2;
            // 
            // L_Move
            // 
            this.L_Move.BackColor = System.Drawing.Color.Transparent;
            this.L_Move.Cursor = System.Windows.Forms.Cursors.Hand;
            this.L_Move.Dock = System.Windows.Forms.DockStyle.Top;
            this.L_Move.Location = new System.Drawing.Point(0, 0);
            this.L_Move.Name = "L_Move";
            this.L_Move.Size = new System.Drawing.Size(481, 54);
            this.L_Move.TabIndex = 20;
            // 
            // txtPassword
            // 
            this.txtPassword.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtPassword.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.HelpProviderHG.SetHelpKeyword(this.txtPassword, "FrmLogin_2.htm#txtPassword");
            this.HelpProviderHG.SetHelpNavigator(this.txtPassword, System.Windows.Forms.HelpNavigator.Topic);
            this.txtPassword.Location = new System.Drawing.Point(284, 161);
            this.txtPassword.Margin = new System.Windows.Forms.Padding(4, 7, 4, 7);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.HelpProviderHG.SetShowHelp(this.txtPassword, true);
            this.txtPassword.Size = new System.Drawing.Size(155, 24);
            this.txtPassword.TabIndex = 21;
            // 
            // lblAttempt
            // 
            this.lblAttempt.AutoSize = true;
            this.lblAttempt.BackColor = System.Drawing.Color.Transparent;
            this.lblAttempt.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAttempt.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lblAttempt.Location = new System.Drawing.Point(102, 237);
            this.lblAttempt.Name = "lblAttempt";
            this.lblAttempt.Size = new System.Drawing.Size(15, 16);
            this.lblAttempt.TabIndex = 23;
            this.lblAttempt.Text = "3";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.Black;
            this.label6.Location = new System.Drawing.Point(20, 236);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(92, 16);
            this.label6.TabIndex = 22;
            this.label6.Text = "User Attempt: ";
            // 
            // HelpProviderHG
            // 
            this.HelpProviderHG.HelpNamespace = "Bank_Reconciliation.chm";
            // 
            // FrmLogin
            // 
            this.AcceptButton = this.BttnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::Collection.Properties.Resources.LogIn2;
            this.CancelButton = this.BttnCancel;
            this.ClientSize = new System.Drawing.Size(481, 267);
            this.Controls.Add(this.lblAttempt);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.L_Move);
            this.Controls.Add(this.txtUsername);
            this.Controls.Add(this.BttnCancel);
            this.Controls.Add(this.BttnOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.HelpProviderHG.SetHelpKeyword(this, "FrmLogin_2.htm");
            this.HelpProviderHG.SetHelpNavigator(this, System.Windows.Forms.HelpNavigator.Topic);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmLogin";
            this.HelpProviderHG.SetShowHelp(this, true);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FrmLogin";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton BttnOK;
        private DevExpress.XtraEditors.SimpleButton BttnCancel;
        private System.Windows.Forms.TextBox txtUsername;
        internal System.Windows.Forms.Label L_Move;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label lblAttempt;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.HelpProvider HelpProviderHG;
    }
}