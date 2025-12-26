namespace Mar2026
{
    partial class FormCloudSetting
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
            this.ButtonDefaultResetForOneDrive = new System.Windows.Forms.Button();
            this.ButtonDefaultResetForMapMarnt = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.TextBoxUrlLocal = new System.Windows.Forms.TextBox();
            this.TextBoxCloudMarnt = new System.Windows.Forms.TextBox();
            this.TextBoxCloudMario = new System.Windows.Forms.TextBox();
            this.TextBoxCloudArchive = new System.Windows.Forms.TextBox();
            this.ButtonSaveAndClose = new System.Windows.Forms.Button();
            this.ButtonClose = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ButtonDefaultResetForOneDrive
            // 
            this.ButtonDefaultResetForOneDrive.Location = new System.Drawing.Point(15, 155);
            this.ButtonDefaultResetForOneDrive.Name = "ButtonDefaultResetForOneDrive";
            this.ButtonDefaultResetForOneDrive.Size = new System.Drawing.Size(158, 23);
            this.ButtonDefaultResetForOneDrive.TabIndex = 0;
            this.ButtonDefaultResetForOneDrive.Text = "AutoDefault voor OneDrive";
            this.ButtonDefaultResetForOneDrive.UseVisualStyleBackColor = true;
            this.ButtonDefaultResetForOneDrive.Click += new System.EventHandler(this.ButtonDefaultResetForOneDrive_Click);
            // 
            // ButtonDefaultResetForMapMarnt
            // 
            this.ButtonDefaultResetForMapMarnt.Location = new System.Drawing.Point(15, 184);
            this.ButtonDefaultResetForMapMarnt.Name = "ButtonDefaultResetForMapMarnt";
            this.ButtonDefaultResetForMapMarnt.Size = new System.Drawing.Size(158, 23);
            this.ButtonDefaultResetForMapMarnt.TabIndex = 1;
            this.ButtonDefaultResetForMapMarnt.Text = "AutoDefault Map Marnt";
            this.ButtonDefaultResetForMapMarnt.UseVisualStyleBackColor = true;
            this.ButtonDefaultResetForMapMarnt.Click += new System.EventHandler(this.ButtonDefaultResetForMapMarnt_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(114, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "URL HOOFDBEDRIJF";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 54);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(86, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "CLOUD MARNT";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 89);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(82, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "CLOUD MARIO";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 124);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(93, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "CLOUD ARCHIEF";
            // 
            // TextBoxUrlLocal
            // 
            this.TextBoxUrlLocal.Location = new System.Drawing.Point(132, 22);
            this.TextBoxUrlLocal.Name = "TextBoxUrlLocal";
            this.TextBoxUrlLocal.Size = new System.Drawing.Size(360, 20);
            this.TextBoxUrlLocal.TabIndex = 6;
            // 
            // TextBoxCloudMarnt
            // 
            this.TextBoxCloudMarnt.Location = new System.Drawing.Point(132, 54);
            this.TextBoxCloudMarnt.Name = "TextBoxCloudMarnt";
            this.TextBoxCloudMarnt.Size = new System.Drawing.Size(360, 20);
            this.TextBoxCloudMarnt.TabIndex = 7;
            // 
            // TextBoxCloudMario
            // 
            this.TextBoxCloudMario.Location = new System.Drawing.Point(132, 86);
            this.TextBoxCloudMario.Name = "TextBoxCloudMario";
            this.TextBoxCloudMario.Size = new System.Drawing.Size(360, 20);
            this.TextBoxCloudMario.TabIndex = 8;
            // 
            // TextBoxCloudArchive
            // 
            this.TextBoxCloudArchive.Location = new System.Drawing.Point(132, 117);
            this.TextBoxCloudArchive.Name = "TextBoxCloudArchive";
            this.TextBoxCloudArchive.Size = new System.Drawing.Size(360, 20);
            this.TextBoxCloudArchive.TabIndex = 9;
            // 
            // ButtonSaveAndClose
            // 
            this.ButtonSaveAndClose.Location = new System.Drawing.Point(312, 155);
            this.ButtonSaveAndClose.Name = "ButtonSaveAndClose";
            this.ButtonSaveAndClose.Size = new System.Drawing.Size(180, 23);
            this.ButtonSaveAndClose.TabIndex = 10;
            this.ButtonSaveAndClose.Text = "Bewaren en Sluiten";
            this.ButtonSaveAndClose.UseVisualStyleBackColor = true;
            this.ButtonSaveAndClose.Click += new System.EventHandler(this.ButtonSaveAndClose_Click);
            // 
            // ButtonClose
            // 
            this.ButtonClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.ButtonClose.Location = new System.Drawing.Point(312, 184);
            this.ButtonClose.Name = "ButtonClose";
            this.ButtonClose.Size = new System.Drawing.Size(180, 23);
            this.ButtonClose.TabIndex = 11;
            this.ButtonClose.Text = "Sluiten";
            this.ButtonClose.UseVisualStyleBackColor = true;
            this.ButtonClose.Click += new System.EventHandler(this.ButtonClose_Click);
            // 
            // FormCloudSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.ButtonClose;
            this.ClientSize = new System.Drawing.Size(506, 224);
            this.Controls.Add(this.ButtonClose);
            this.Controls.Add(this.ButtonSaveAndClose);
            this.Controls.Add(this.TextBoxCloudArchive);
            this.Controls.Add(this.TextBoxCloudMario);
            this.Controls.Add(this.TextBoxCloudMarnt);
            this.Controls.Add(this.TextBoxUrlLocal);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ButtonDefaultResetForMapMarnt);
            this.Controls.Add(this.ButtonDefaultResetForOneDrive);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormCloudSetting";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "FormCloudSetting";
            this.Load += new System.EventHandler(this.FormCloudSetting_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ButtonDefaultResetForOneDrive;
        private System.Windows.Forms.Button ButtonDefaultResetForMapMarnt;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox TextBoxUrlLocal;
        private System.Windows.Forms.TextBox TextBoxCloudMarnt;
        private System.Windows.Forms.TextBox TextBoxCloudMario;
        private System.Windows.Forms.TextBox TextBoxCloudArchive;
        private System.Windows.Forms.Button ButtonSaveAndClose;
        private System.Windows.Forms.Button ButtonClose;
    }
}