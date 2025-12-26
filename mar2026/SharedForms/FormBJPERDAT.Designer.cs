namespace mar2026
{
    partial class FormBJPERDAT
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
            this.DatumVerwerking = new System.Windows.Forms.DateTimePicker();
            this.CmbPeriodeBoekjaar = new System.Windows.Forms.ComboBox();
            this.CmbBoekjaar = new System.Windows.Forms.ComboBox();
            this.BtnVerkleinen = new System.Windows.Forms.Button();
            this.lblDatumVandaag = new System.Windows.Forms.Label();
            this.lblActievePeriode = new System.Windows.Forms.Label();
            this.lblActiefBoekjaar = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // DatumVerwerking
            // 
            this.DatumVerwerking.Location = new System.Drawing.Point(96, 16);
            this.DatumVerwerking.Name = "DatumVerwerking";
            this.DatumVerwerking.Size = new System.Drawing.Size(200, 20);
            this.DatumVerwerking.TabIndex = 1;
            this.DatumVerwerking.ValueChanged += new System.EventHandler(this.DatumVerwerking_ValueChanged);
            // 
            // CmbPeriodeBoekjaar
            // 
            this.CmbPeriodeBoekjaar.FormattingEnabled = true;
            this.CmbPeriodeBoekjaar.Location = new System.Drawing.Point(96, 40);
            this.CmbPeriodeBoekjaar.Name = "CmbPeriodeBoekjaar";
            this.CmbPeriodeBoekjaar.Size = new System.Drawing.Size(200, 21);
            this.CmbPeriodeBoekjaar.TabIndex = 3;
            this.CmbPeriodeBoekjaar.SelectedIndexChanged += new System.EventHandler(this.CmbPeriodeBoekjaar_SelectedIndexChanged);
            // 
            // CmbBoekjaar
            // 
            this.CmbBoekjaar.FormattingEnabled = true;
            this.CmbBoekjaar.Location = new System.Drawing.Point(205, 71);
            this.CmbBoekjaar.Name = "CmbBoekjaar";
            this.CmbBoekjaar.Size = new System.Drawing.Size(91, 21);
            this.CmbBoekjaar.TabIndex = 5;
            this.CmbBoekjaar.SelectedIndexChanged += new System.EventHandler(this.CmbBoekjaar_SelectedIndexChanged);
            // 
            // BtnVerkleinen
            // 
            this.BtnVerkleinen.Location = new System.Drawing.Point(10, 69);
            this.BtnVerkleinen.Name = "BtnVerkleinen";
            this.BtnVerkleinen.Size = new System.Drawing.Size(80, 29);
            this.BtnVerkleinen.TabIndex = 3;
            this.BtnVerkleinen.TabStop = false;
            this.BtnVerkleinen.Text = "Minimaliseren";
            this.BtnVerkleinen.UseVisualStyleBackColor = true;
            this.BtnVerkleinen.Click += new System.EventHandler(this.BtnVerkleinen_Click);
            // 
            // lblDatumVandaag
            // 
            this.lblDatumVandaag.AutoSize = true;
            this.lblDatumVandaag.Location = new System.Drawing.Point(6, 22);
            this.lblDatumVandaag.Name = "lblDatumVandaag";
            this.lblDatumVandaag.Size = new System.Drawing.Size(83, 13);
            this.lblDatumVandaag.TabIndex = 0;
            this.lblDatumVandaag.Text = "&Datum vandaag";
            // 
            // lblActievePeriode
            // 
            this.lblActievePeriode.AutoSize = true;
            this.lblActievePeriode.Location = new System.Drawing.Point(7, 48);
            this.lblActievePeriode.Name = "lblActievePeriode";
            this.lblActievePeriode.Size = new System.Drawing.Size(82, 13);
            this.lblActievePeriode.TabIndex = 2;
            this.lblActievePeriode.Text = "Actieve &Periode";
            // 
            // lblActiefBoekjaar
            // 
            this.lblActiefBoekjaar.AutoSize = true;
            this.lblActiefBoekjaar.Location = new System.Drawing.Point(120, 74);
            this.lblActiefBoekjaar.Name = "lblActiefBoekjaar";
            this.lblActiefBoekjaar.Size = new System.Drawing.Size(79, 13);
            this.lblActiefBoekjaar.TabIndex = 4;
            this.lblActiefBoekjaar.Text = "Actief &Boekjaar";
            // 
            // FormBJPERDAT
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(307, 108);
            this.ControlBox = false;
            this.Controls.Add(this.lblActiefBoekjaar);
            this.Controls.Add(this.lblActievePeriode);
            this.Controls.Add(this.lblDatumVandaag);
            this.Controls.Add(this.BtnVerkleinen);
            this.Controls.Add(this.CmbBoekjaar);
            this.Controls.Add(this.CmbPeriodeBoekjaar);
            this.Controls.Add(this.DatumVerwerking);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormBJPERDAT";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Datum / Periode / Boekjaar (Ctrl+D)";
            this.Activated += new System.EventHandler(this.FormBJPERDAT_Activated);
            this.Load += new System.EventHandler(this.FormBJPERDAT_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.DateTimePicker DatumVerwerking;
        public System.Windows.Forms.ComboBox CmbPeriodeBoekjaar;
        public System.Windows.Forms.ComboBox CmbBoekjaar;
        private System.Windows.Forms.Button BtnVerkleinen;
        private System.Windows.Forms.Label lblDatumVandaag;
        private System.Windows.Forms.Label lblActievePeriode;
        private System.Windows.Forms.Label lblActiefBoekjaar;
    }
}