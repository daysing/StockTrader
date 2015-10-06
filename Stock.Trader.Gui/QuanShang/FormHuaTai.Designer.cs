namespace Stock.Trader
{
    partial class FormHuaTai
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormHuaTai));
            this.txtTransPwd = new AxCSSWEBLOGINHTLib.AxCsswebLogin();
            this.label1 = new System.Windows.Forms.Label();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.txtServicePwd = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.txtTransPwd)).BeginInit();
            this.SuspendLayout();
            // 
            // txtTransPwd
            // 
            this.txtTransPwd.Enabled = true;
            this.txtTransPwd.Location = new System.Drawing.Point(95, 48);
            this.txtTransPwd.Name = "txtTransPwd";
            this.txtTransPwd.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("txtTransPwd.OcxState")));
            this.txtTransPwd.Size = new System.Drawing.Size(107, 23);
            this.txtTransPwd.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 48);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "label1";
            // 
            // txtUsername
            // 
            this.txtUsername.Location = new System.Drawing.Point(95, 21);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(107, 21);
            this.txtUsername.TabIndex = 2;
            // 
            // txtServicePwd
            // 
            this.txtServicePwd.Location = new System.Drawing.Point(95, 77);
            this.txtServicePwd.Name = "txtServicePwd";
            this.txtServicePwd.Size = new System.Drawing.Size(107, 21);
            this.txtServicePwd.TabIndex = 3;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(127, 117);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 4;
            this.btnSave.Text = "保存";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // FormHuaTai
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(218, 155);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.txtServicePwd);
            this.Controls.Add(this.txtUsername);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtTransPwd);
            this.Name = "FormHuaTai";
            this.Text = "FormHuaTai";
            ((System.ComponentModel.ISupportInitialize)(this.txtTransPwd)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private AxCSSWEBLOGINHTLib.AxCsswebLogin txtTransPwd;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.TextBox txtServicePwd;
        private System.Windows.Forms.Button btnSave;
    }
}