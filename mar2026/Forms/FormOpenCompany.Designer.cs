namespace mar2026.Forms
{
    partial class FormOpenCompany
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
            this.ButtonCancel = new System.Windows.Forms.Button();
            this.ButtonOk = new System.Windows.Forms.Button();
            this.ButtonCompact = new System.Windows.Forms.Button();
            this.ButtonToggleEditLocation = new System.Windows.Forms.Button();
            this.ButtonOpenFolder = new System.Windows.Forms.Button();
            this.TextBoxLocation = new System.Windows.Forms.TextBox();
            this.RadioButtonServer = new System.Windows.Forms.RadioButton();
            this.RadioButtonLocal = new System.Windows.Forms.RadioButton();
            this.ListViewCompanies = new System.Windows.Forms.ListView();
            this.SuspendLayout();
            // 
            // ButtonCancel
            // 
            this.ButtonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.ButtonCancel.Location = new System.Drawing.Point(490, 272);
            this.ButtonCancel.Name = "ButtonCancel";
            this.ButtonCancel.Size = new System.Drawing.Size(72, 23);
            this.ButtonCancel.TabIndex = 17;
            this.ButtonCancel.Text = "Cancel";
            this.ButtonCancel.UseVisualStyleBackColor = true;
            this.ButtonCancel.Click += new System.EventHandler(this.ButtonCancel_Click);
            // 
            // ButtonOk
            // 
            this.ButtonOk.Location = new System.Drawing.Point(490, 233);
            this.ButtonOk.Name = "ButtonOk";
            this.ButtonOk.Size = new System.Drawing.Size(72, 23);
            this.ButtonOk.TabIndex = 16;
            this.ButtonOk.Text = "Ok";
            this.ButtonOk.UseVisualStyleBackColor = true;
            this.ButtonOk.Click += new System.EventHandler(this.ButtonnOk_Click);
            // 
            // ButtonCompact
            // 
            this.ButtonCompact.Location = new System.Drawing.Point(12, 233);
            this.ButtonCompact.Name = "ButtonCompact";
            this.ButtonCompact.Size = new System.Drawing.Size(145, 23);
            this.ButtonCompact.TabIndex = 15;
            this.ButtonCompact.Text = "Compact Database";
            this.ButtonCompact.UseVisualStyleBackColor = true;
            this.ButtonCompact.Click += new System.EventHandler(this.ButtonCompact_Click);
            // 
            // ButtonToggleEditLocation
            // 
            this.ButtonToggleEditLocation.Location = new System.Drawing.Point(450, 192);
            this.ButtonToggleEditLocation.Name = "ButtonToggleEditLocation";
            this.ButtonToggleEditLocation.Size = new System.Drawing.Size(30, 23);
            this.ButtonToggleEditLocation.TabIndex = 14;
            this.ButtonToggleEditLocation.Text = "...";
            this.ButtonToggleEditLocation.UseVisualStyleBackColor = true;
            this.ButtonToggleEditLocation.Click += new System.EventHandler(this.ButtonToggleEditLocation_Click);
            // 
            // ButtonOpenFolder
            // 
            this.ButtonOpenFolder.Location = new System.Drawing.Point(490, 194);
            this.ButtonOpenFolder.Name = "ButtonOpenFolder";
            this.ButtonOpenFolder.Size = new System.Drawing.Size(75, 23);
            this.ButtonOpenFolder.TabIndex = 13;
            this.ButtonOpenFolder.Text = "Open folder";
            this.ButtonOpenFolder.UseVisualStyleBackColor = true;
            this.ButtonOpenFolder.Click += new System.EventHandler(this.ButtonOpenFolder_Click);
            // 
            // TextBoxLocation
            // 
            this.TextBoxLocation.Enabled = false;
            this.TextBoxLocation.Location = new System.Drawing.Point(12, 194);
            this.TextBoxLocation.Name = "TextBoxLocation";
            this.TextBoxLocation.Size = new System.Drawing.Size(432, 20);
            this.TextBoxLocation.TabIndex = 12;
            // 
            // RadioButtonServer
            // 
            this.RadioButtonServer.AutoSize = true;
            this.RadioButtonServer.Location = new System.Drawing.Point(480, 12);
            this.RadioButtonServer.Name = "RadioButtonServer";
            this.RadioButtonServer.Size = new System.Drawing.Size(82, 17);
            this.RadioButtonServer.TabIndex = 11;
            this.RadioButtonServer.TabStop = true;
            this.RadioButtonServer.Text = "Data Server";
            this.RadioButtonServer.UseVisualStyleBackColor = true;
            this.RadioButtonServer.CheckedChanged += new System.EventHandler(this.RadioButtonLocation_CheckedChanged);
            // 
            // RadioButtonLocal
            // 
            this.RadioButtonLocal.AutoSize = true;
            this.RadioButtonLocal.Location = new System.Drawing.Point(12, 12);
            this.RadioButtonLocal.Name = "RadioButtonLocal";
            this.RadioButtonLocal.Size = new System.Drawing.Size(83, 17);
            this.RadioButtonLocal.TabIndex = 10;
            this.RadioButtonLocal.TabStop = true;
            this.RadioButtonLocal.Text = "Data Lokaal";
            this.RadioButtonLocal.UseVisualStyleBackColor = true;
            this.RadioButtonLocal.CheckedChanged += new System.EventHandler(this.RadioButtonLocation_CheckedChanged);
            // 
            // ListViewCompanies
            // 
            this.ListViewCompanies.HideSelection = false;
            this.ListViewCompanies.Location = new System.Drawing.Point(12, 35);
            this.ListViewCompanies.Name = "ListViewCompanies";
            this.ListViewCompanies.Size = new System.Drawing.Size(553, 153);
            this.ListViewCompanies.TabIndex = 9;
            this.ListViewCompanies.UseCompatibleStateImageBehavior = false;
            this.ListViewCompanies.DoubleClick += new System.EventHandler(this.ListViewCompanies_DoubleClick);
            this.ListViewCompanies.Enter += new System.EventHandler(this.ListViewCompanies_GotFocus);
            // 
            // FormOpenCompany
            // 
            this.AcceptButton = this.ButtonOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.ButtonCancel;
            this.ClientSize = new System.Drawing.Size(578, 304);
            this.Controls.Add(this.ButtonCancel);
            this.Controls.Add(this.ButtonOk);
            this.Controls.Add(this.ButtonCompact);
            this.Controls.Add(this.ButtonToggleEditLocation);
            this.Controls.Add(this.ButtonOpenFolder);
            this.Controls.Add(this.TextBoxLocation);
            this.Controls.Add(this.RadioButtonServer);
            this.Controls.Add(this.RadioButtonLocal);
            this.Controls.Add(this.ListViewCompanies);
            this.Name = "FormOpenCompany";
            this.Text = "FormOpenCompany";
            this.Load += new System.EventHandler(this.FormOpenCompany_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ButtonCancel;
        private System.Windows.Forms.Button ButtonOk;
        private System.Windows.Forms.Button ButtonCompact;
        private System.Windows.Forms.Button ButtonToggleEditLocation;
        private System.Windows.Forms.Button ButtonOpenFolder;
        private System.Windows.Forms.TextBox TextBoxLocation;
        private System.Windows.Forms.RadioButton RadioButtonServer;
        private System.Windows.Forms.RadioButton RadioButtonLocal;
        private System.Windows.Forms.ListView ListViewCompanies;
    }
}