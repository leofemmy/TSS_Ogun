namespace Control_Panel.Forms
{
    partial class FrmDefualtState
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
            this.label1 = new System.Windows.Forms.Label();
            this.cboState = new System.Windows.Forms.ComboBox();
            this.tblState1BindingSource = new System.Windows.Forms.BindingSource();
            this.dsBank = new Control_Panel.DataSet.dsBank();
            this.btnok = new System.Windows.Forms.Button();
            this.tblState1TableAdapter = new Control_Panel.DataSet.dsBankTableAdapters.tblState1TableAdapter();
            this.HelpProviderHG = new System.Windows.Forms.HelpProvider();
            ((System.ComponentModel.ISupportInitialize)(this.tblState1BindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsBank)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(24, 43);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(115, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Select Default State";
            // 
            // cboState
            // 
            this.cboState.DataSource = this.tblState1BindingSource;
            this.cboState.DisplayMember = "StateName";
            this.cboState.FormattingEnabled = true;
            this.HelpProviderHG.SetHelpKeyword(this.cboState, "FrmDefualtState.htm#label1");
            this.HelpProviderHG.SetHelpNavigator(this.cboState, System.Windows.Forms.HelpNavigator.Topic);
            this.cboState.Location = new System.Drawing.Point(145, 41);
            this.cboState.Name = "cboState";
            this.HelpProviderHG.SetShowHelp(this.cboState, true);
            this.cboState.Size = new System.Drawing.Size(208, 21);
            this.cboState.TabIndex = 1;
            this.cboState.ValueMember = "StateCode";
            this.cboState.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cboState_KeyPress);
            // 
            // tblState1BindingSource
            // 
            this.tblState1BindingSource.DataMember = "tblState1";
            this.tblState1BindingSource.DataSource = this.dsBank;
            // 
            // dsBank
            // 
            this.dsBank.DataSetName = "dsBank";
            this.dsBank.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // btnok
            // 
            this.HelpProviderHG.SetHelpKeyword(this.btnok, "FrmDefualtState.htm#btnok");
            this.HelpProviderHG.SetHelpNavigator(this.btnok, System.Windows.Forms.HelpNavigator.Topic);
            this.btnok.Location = new System.Drawing.Point(359, 41);
            this.btnok.Name = "btnok";
            this.HelpProviderHG.SetShowHelp(this.btnok, true);
            this.btnok.Size = new System.Drawing.Size(47, 23);
            this.btnok.TabIndex = 2;
            this.btnok.Text = "&Ok";
            this.btnok.UseVisualStyleBackColor = true;
            this.btnok.Click += new System.EventHandler(this.btnok_Click);
            // 
            // tblState1TableAdapter
            // 
            this.tblState1TableAdapter.ClearBeforeFill = true;
            // 
            // HelpProviderHG
            // 
            this.HelpProviderHG.HelpNamespace = "Bank_Reconciliation.chm";
            // 
            // FrmDefualtState
            // 
            this.AcceptButton = this.btnok;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(441, 122);
            this.Controls.Add(this.btnok);
            this.Controls.Add(this.cboState);
            this.Controls.Add(this.label1);
            this.HelpProviderHG.SetHelpKeyword(this, "FrmDefualtState.htm");
            this.HelpProviderHG.SetHelpNavigator(this, System.Windows.Forms.HelpNavigator.Topic);
            this.Name = "FrmDefualtState";
            this.HelpProviderHG.SetShowHelp(this, true);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Set Default State";
            this.Load += new System.EventHandler(this.FrmDefualtState_Load);
            ((System.ComponentModel.ISupportInitialize)(this.tblState1BindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsBank)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cboState;
        private System.Windows.Forms.Button btnok;
        private Control_Panel.DataSet.dsBank dsBank;
        private System.Windows.Forms.BindingSource tblState1BindingSource;
        private Control_Panel.DataSet.dsBankTableAdapters.tblState1TableAdapter tblState1TableAdapter;
        private System.Windows.Forms.HelpProvider HelpProviderHG;
    }
}