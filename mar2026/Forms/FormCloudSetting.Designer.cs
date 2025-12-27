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
            this.GroupBoxCloud = new System.Windows.Forms.GroupBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioButtonShowAlwaysBookingsInfo = new System.Windows.Forms.RadioButton();
            this.radioButtonShowSomeBookingsInfo = new System.Windows.Forms.RadioButton();
            this.radioButtonShowNoBookingsInfo = new System.Windows.Forms.RadioButton();
            this.GroupBoxCloud.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ButtonDefaultResetForOneDrive
            // 
            this.ButtonDefaultResetForOneDrive.Location = new System.Drawing.Point(17, 138);
            this.ButtonDefaultResetForOneDrive.Name = "ButtonDefaultResetForOneDrive";
            this.ButtonDefaultResetForOneDrive.Size = new System.Drawing.Size(158, 30);
            this.ButtonDefaultResetForOneDrive.TabIndex = 0;
            this.ButtonDefaultResetForOneDrive.Text = "AutoDefault voor OneDrive";
            this.ButtonDefaultResetForOneDrive.UseVisualStyleBackColor = true;
            this.ButtonDefaultResetForOneDrive.Click += new System.EventHandler(this.ButtonDefaultResetForOneDrive_Click);
            // 
            // ButtonDefaultResetForMapMarnt
            // 
            this.ButtonDefaultResetForMapMarnt.Location = new System.Drawing.Point(181, 138);
            this.ButtonDefaultResetForMapMarnt.Name = "ButtonDefaultResetForMapMarnt";
            this.ButtonDefaultResetForMapMarnt.Size = new System.Drawing.Size(158, 30);
            this.ButtonDefaultResetForMapMarnt.TabIndex = 1;
            this.ButtonDefaultResetForMapMarnt.Text = "AutoDefault Map Marnt";
            this.ButtonDefaultResetForMapMarnt.UseVisualStyleBackColor = true;
            this.ButtonDefaultResetForMapMarnt.Click += new System.EventHandler(this.ButtonDefaultResetForMapMarnt_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(76, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "URL BEDRIJF";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 51);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(46, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "MARNT";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(14, 80);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(42, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "MARIO";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(14, 110);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "ARCHIEF";
            // 
            // TextBoxUrlLocal
            // 
            this.TextBoxUrlLocal.Location = new System.Drawing.Point(93, 25);
            this.TextBoxUrlLocal.Name = "TextBoxUrlLocal";
            this.TextBoxUrlLocal.Size = new System.Drawing.Size(360, 20);
            this.TextBoxUrlLocal.TabIndex = 6;
            // 
            // TextBoxCloudMarnt
            // 
            this.TextBoxCloudMarnt.Location = new System.Drawing.Point(93, 51);
            this.TextBoxCloudMarnt.Name = "TextBoxCloudMarnt";
            this.TextBoxCloudMarnt.Size = new System.Drawing.Size(360, 20);
            this.TextBoxCloudMarnt.TabIndex = 7;
            // 
            // TextBoxCloudMario
            // 
            this.TextBoxCloudMario.Location = new System.Drawing.Point(93, 77);
            this.TextBoxCloudMario.Name = "TextBoxCloudMario";
            this.TextBoxCloudMario.Size = new System.Drawing.Size(360, 20);
            this.TextBoxCloudMario.TabIndex = 8;
            // 
            // TextBoxCloudArchive
            // 
            this.TextBoxCloudArchive.Location = new System.Drawing.Point(93, 103);
            this.TextBoxCloudArchive.Name = "TextBoxCloudArchive";
            this.TextBoxCloudArchive.Size = new System.Drawing.Size(360, 20);
            this.TextBoxCloudArchive.TabIndex = 9;
            // 
            // ButtonSaveAndClose
            // 
            this.ButtonSaveAndClose.Location = new System.Drawing.Point(340, 138);
            this.ButtonSaveAndClose.Name = "ButtonSaveAndClose";
            this.ButtonSaveAndClose.Size = new System.Drawing.Size(113, 30);
            this.ButtonSaveAndClose.TabIndex = 10;
            this.ButtonSaveAndClose.Text = "Bewaren en Sluiten";
            this.ButtonSaveAndClose.UseVisualStyleBackColor = true;
            this.ButtonSaveAndClose.Click += new System.EventHandler(this.ButtonSaveAndClose_Click);
            // 
            // ButtonClose
            // 
            this.ButtonClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.ButtonClose.Location = new System.Drawing.Point(558, 168);
            this.ButtonClose.Name = "ButtonClose";
            this.ButtonClose.Size = new System.Drawing.Size(158, 23);
            this.ButtonClose.TabIndex = 11;
            this.ButtonClose.Text = "Sluiten";
            this.ButtonClose.UseVisualStyleBackColor = true;
            this.ButtonClose.Click += new System.EventHandler(this.ButtonClose_Click);
            // 
            // GroupBoxCloud
            // 
            this.GroupBoxCloud.Controls.Add(this.ButtonDefaultResetForOneDrive);
            this.GroupBoxCloud.Controls.Add(this.ButtonDefaultResetForMapMarnt);
            this.GroupBoxCloud.Controls.Add(this.label1);
            this.GroupBoxCloud.Controls.Add(this.ButtonSaveAndClose);
            this.GroupBoxCloud.Controls.Add(this.TextBoxCloudArchive);
            this.GroupBoxCloud.Controls.Add(this.label2);
            this.GroupBoxCloud.Controls.Add(this.TextBoxCloudMario);
            this.GroupBoxCloud.Controls.Add(this.label3);
            this.GroupBoxCloud.Controls.Add(this.TextBoxCloudMarnt);
            this.GroupBoxCloud.Controls.Add(this.label4);
            this.GroupBoxCloud.Controls.Add(this.TextBoxUrlLocal);
            this.GroupBoxCloud.Location = new System.Drawing.Point(12, 12);
            this.GroupBoxCloud.Name = "GroupBoxCloud";
            this.GroupBoxCloud.Size = new System.Drawing.Size(469, 179);
            this.GroupBoxCloud.TabIndex = 12;
            this.GroupBoxCloud.TabStop = false;
            this.GroupBoxCloud.Text = "Cloud";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioButtonShowAlwaysBookingsInfo);
            this.groupBox1.Controls.Add(this.radioButtonShowSomeBookingsInfo);
            this.groupBox1.Controls.Add(this.radioButtonShowNoBookingsInfo);
            this.groupBox1.Location = new System.Drawing.Point(487, 19);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(230, 90);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Journaal post";
            // 
            // radioButtonShowAlwaysBookingsInfo
            // 
            this.radioButtonShowAlwaysBookingsInfo.AutoSize = true;
            this.radioButtonShowAlwaysBookingsInfo.Location = new System.Drawing.Point(17, 62);
            this.radioButtonShowAlwaysBookingsInfo.Name = "radioButtonShowAlwaysBookingsInfo";
            this.radioButtonShowAlwaysBookingsInfo.Size = new System.Drawing.Size(146, 17);
            this.radioButtonShowAlwaysBookingsInfo.TabIndex = 2;
            this.radioButtonShowAlwaysBookingsInfo.TabStop = true;
            this.radioButtonShowAlwaysBookingsInfo.Text = "Altijd BoekingsInfo Tonen";
            this.radioButtonShowAlwaysBookingsInfo.UseVisualStyleBackColor = true;
            this.radioButtonShowAlwaysBookingsInfo.Click += new System.EventHandler(this.radioButtonShowAlwaysBookingsInfo_Click);
            // 
            // radioButtonShowSomeBookingsInfo
            // 
            this.radioButtonShowSomeBookingsInfo.AutoSize = true;
            this.radioButtonShowSomeBookingsInfo.Location = new System.Drawing.Point(17, 39);
            this.radioButtonShowSomeBookingsInfo.Name = "radioButtonShowSomeBookingsInfo";
            this.radioButtonShowSomeBookingsInfo.Size = new System.Drawing.Size(203, 17);
            this.radioButtonShowSomeBookingsInfo.TabIndex = 1;
            this.radioButtonShowSomeBookingsInfo.TabStop = true;
            this.radioButtonShowSomeBookingsInfo.Text = "BoekingsInfo bij EUR <> BEF verschil";
            this.radioButtonShowSomeBookingsInfo.UseVisualStyleBackColor = true;
            this.radioButtonShowSomeBookingsInfo.Click += new System.EventHandler(this.radioButtonShowSomeBookingsInfo_Click);
            // 
            // radioButtonShowNoBookingsInfo
            // 
            this.radioButtonShowNoBookingsInfo.AutoSize = true;
            this.radioButtonShowNoBookingsInfo.Location = new System.Drawing.Point(17, 16);
            this.radioButtonShowNoBookingsInfo.Name = "radioButtonShowNoBookingsInfo";
            this.radioButtonShowNoBookingsInfo.Size = new System.Drawing.Size(150, 17);
            this.radioButtonShowNoBookingsInfo.TabIndex = 0;
            this.radioButtonShowNoBookingsInfo.TabStop = true;
            this.radioButtonShowNoBookingsInfo.Text = "Geen BoekingsInfo Tonen";
            this.radioButtonShowNoBookingsInfo.UseVisualStyleBackColor = true;            
            this.radioButtonShowNoBookingsInfo.Click += new System.EventHandler(this.radioButtonShowNoBookingsInfo_Click);
            // 
            // FormCloudSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.ButtonClose;
            this.ClientSize = new System.Drawing.Size(728, 200);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.GroupBoxCloud);
            this.Controls.Add(this.ButtonClose);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormCloudSetting";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Instellingen";
            this.Load += new System.EventHandler(this.FormCloudSetting_Load);
            this.GroupBoxCloud.ResumeLayout(false);
            this.GroupBoxCloud.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

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
        private System.Windows.Forms.GroupBox GroupBoxCloud;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radioButtonShowAlwaysBookingsInfo;
        private System.Windows.Forms.RadioButton radioButtonShowSomeBookingsInfo;
        private System.Windows.Forms.RadioButton radioButtonShowNoBookingsInfo;
    }
}