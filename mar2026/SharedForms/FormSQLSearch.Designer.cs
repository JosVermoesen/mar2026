namespace mar2026.SharedForms
{
    partial class FormSQLSearch
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
            this.LblText1 = new System.Windows.Forms.Label();
            this.LblText2 = new System.Windows.Forms.Label();
            this.chkExterneDatabase = new System.Windows.Forms.CheckBox();
            this.cmdBewaar = new System.Windows.Forms.Button();
            this.ButtonClose = new System.Windows.Forms.Button();
            this.RichTextBoxSQLSelect = new System.Windows.Forms.RichTextBox();
            this.TextBoxToSearch = new System.Windows.Forms.TextBox();
            this.sqkResultListView = new System.Windows.Forms.ListView();
            this.ComboBoxSortOn = new System.Windows.Forms.ComboBox();
            this.ButtonSearchLike = new System.Windows.Forms.Button();
            this.ButtonSQLToggle = new System.Windows.Forms.Button();
            this.ButtonOk = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // LblText1
            // 
            this.LblText1.AutoSize = true;
            this.LblText1.Location = new System.Drawing.Point(12, 22);
            this.LblText1.Name = "LblText1";
            this.LblText1.Size = new System.Drawing.Size(59, 13);
            this.LblText1.TabIndex = 0;
            this.LblText1.Text = "&Zoek zoals";
            // 
            // LblText2
            // 
            this.LblText2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.LblText2.Location = new System.Drawing.Point(470, 47);
            this.LblText2.Name = "LblText2";
            this.LblText2.Size = new System.Drawing.Size(63, 17);
            this.LblText2.TabIndex = 1;
            this.LblText2.Text = "0";
            this.LblText2.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // chkExterneDatabase
            // 
            this.chkExterneDatabase.AutoSize = true;
            this.chkExterneDatabase.Location = new System.Drawing.Point(338, 47);
            this.chkExterneDatabase.Name = "chkExterneDatabase";
            this.chkExterneDatabase.Size = new System.Drawing.Size(111, 17);
            this.chkExterneDatabase.TabIndex = 2;
            this.chkExterneDatabase.Text = "Externe Database";
            this.chkExterneDatabase.UseVisualStyleBackColor = true;
            this.chkExterneDatabase.Visible = false;
            // 
            // cmdBewaar
            // 
            this.cmdBewaar.Enabled = false;
            this.cmdBewaar.Location = new System.Drawing.Point(458, 275);
            this.cmdBewaar.Name = "cmdBewaar";
            this.cmdBewaar.Size = new System.Drawing.Size(72, 23);
            this.cmdBewaar.TabIndex = 4;
            this.cmdBewaar.Text = "Bewaren";
            this.cmdBewaar.UseVisualStyleBackColor = true;
            // 
            // ButtonClose
            // 
            this.ButtonClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.ButtonClose.Location = new System.Drawing.Point(458, 295);
            this.ButtonClose.Name = "ButtonClose";
            this.ButtonClose.Size = new System.Drawing.Size(72, 23);
            this.ButtonClose.TabIndex = 5;
            this.ButtonClose.Text = "Sluiten";
            this.ButtonClose.UseVisualStyleBackColor = true;
            this.ButtonClose.Click += new System.EventHandler(this.ButtonClose_Click);
            // 
            // RichTextBoxSQLSelect
            // 
            this.RichTextBoxSQLSelect.Enabled = false;
            this.RichTextBoxSQLSelect.Location = new System.Drawing.Point(12, 241);
            this.RichTextBoxSQLSelect.Name = "RichTextBoxSQLSelect";
            this.RichTextBoxSQLSelect.Size = new System.Drawing.Size(437, 77);
            this.RichTextBoxSQLSelect.TabIndex = 6;
            this.RichTextBoxSQLSelect.Text = "";
            this.RichTextBoxSQLSelect.TextChanged += new System.EventHandler(this.RitchTextBoxSQLSelect_TextChanged);
            // 
            // TextBoxToSearch
            // 
            this.TextBoxToSearch.Location = new System.Drawing.Point(77, 15);
            this.TextBoxToSearch.Name = "TextBoxToSearch";
            this.TextBoxToSearch.Size = new System.Drawing.Size(375, 20);
            this.TextBoxToSearch.TabIndex = 7;
            this.TextBoxToSearch.TextChanged += new System.EventHandler(this.TextBoxToSearch_TextChanged);
            this.TextBoxToSearch.Enter += new System.EventHandler(this.TextBoxToSearch_Enter);
            this.TextBoxToSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TextBoxToSearch_KeyDown);
            this.TextBoxToSearch.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TextBoxToSearch_KeyPress);
            // 
            // sqkResultListView
            // 
            this.sqkResultListView.HideSelection = false;
            this.sqkResultListView.Location = new System.Drawing.Point(15, 70);
            this.sqkResultListView.Name = "sqkResultListView";
            this.sqkResultListView.Size = new System.Drawing.Size(518, 165);
            this.sqkResultListView.TabIndex = 8;
            this.sqkResultListView.UseCompatibleStateImageBehavior = false;
            this.sqkResultListView.View = System.Windows.Forms.View.Details;
            this.sqkResultListView.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.ListViewSqlResult_ItemSelectionChanged);
            this.sqkResultListView.SelectedIndexChanged += new System.EventHandler(this.ListViewSqlResult_SelectedIndexChanged);
            this.sqkResultListView.Click += new System.EventHandler(this.ListViewSqlResult_Click);
            this.sqkResultListView.DoubleClick += new System.EventHandler(this.ListViewSqlResult_DoubleClick);
            this.sqkResultListView.Enter += new System.EventHandler(this.ListViewSqlResult_Enter);
            this.sqkResultListView.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.sqkResultListView_KeyPress);
            this.sqkResultListView.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.ListViewSqlResult_MouseDoubleClick);
            // 
            // ComboBoxSortOn
            // 
            this.ComboBoxSortOn.FormattingEnabled = true;
            this.ComboBoxSortOn.Location = new System.Drawing.Point(15, 43);
            this.ComboBoxSortOn.Name = "ComboBoxSortOn";
            this.ComboBoxSortOn.Size = new System.Drawing.Size(310, 21);
            this.ComboBoxSortOn.TabIndex = 9;
            this.ComboBoxSortOn.SelectedIndexChanged += new System.EventHandler(this.ComboBoxSortOn_SelectedIndexChanged);
            // 
            // ButtonSearchLike
            // 
            this.ButtonSearchLike.Location = new System.Drawing.Point(458, 15);
            this.ButtonSearchLike.Name = "ButtonSearchLike";
            this.ButtonSearchLike.Size = new System.Drawing.Size(75, 23);
            this.ButtonSearchLike.TabIndex = 10;
            this.ButtonSearchLike.Text = "Zoeken";
            this.ButtonSearchLike.UseVisualStyleBackColor = true;
            this.ButtonSearchLike.Click += new System.EventHandler(this.ButtonSearch_Click);
            // 
            // ButtonSQLToggle
            // 
            this.ButtonSQLToggle.Location = new System.Drawing.Point(455, 241);
            this.ButtonSQLToggle.Name = "ButtonSQLToggle";
            this.ButtonSQLToggle.Size = new System.Drawing.Size(72, 23);
            this.ButtonSQLToggle.TabIndex = 11;
            this.ButtonSQLToggle.Text = "Toggle";
            this.ButtonSQLToggle.UseVisualStyleBackColor = true;
            this.ButtonSQLToggle.Click += new System.EventHandler(this.ButtonSQLToggle_Click);
            // 
            // ButtonOk
            // 
            this.ButtonOk.Location = new System.Drawing.Point(458, 15);
            this.ButtonOk.Name = "ButtonOk";
            this.ButtonOk.Size = new System.Drawing.Size(75, 23);
            this.ButtonOk.TabIndex = 12;
            this.ButtonOk.Text = "Ok";
            this.ButtonOk.UseVisualStyleBackColor = true;
            this.ButtonOk.Click += new System.EventHandler(this.ButtonOk_Click);
            // 
            // FormSQLSearch
            // 
            this.AcceptButton = this.ButtonSearchLike;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.ButtonClose;
            this.ClientSize = new System.Drawing.Size(545, 330);
            this.Controls.Add(this.ButtonSQLToggle);
            this.Controls.Add(this.ComboBoxSortOn);
            this.Controls.Add(this.sqkResultListView);
            this.Controls.Add(this.TextBoxToSearch);
            this.Controls.Add(this.RichTextBoxSQLSelect);
            this.Controls.Add(this.ButtonClose);
            this.Controls.Add(this.cmdBewaar);
            this.Controls.Add(this.chkExterneDatabase);
            this.Controls.Add(this.LblText2);
            this.Controls.Add(this.LblText1);
            this.Controls.Add(this.ButtonSearchLike);
            this.Controls.Add(this.ButtonOk);
            this.Name = "FormSQLSearch";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "FormSQLSearch";
            this.Load += new System.EventHandler(this.FormSQLSearch_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label LblText1;
        private System.Windows.Forms.Label LblText2;
        private System.Windows.Forms.CheckBox chkExterneDatabase;
        private System.Windows.Forms.Button cmdBewaar;
        private System.Windows.Forms.Button ButtonClose;
        private System.Windows.Forms.RichTextBox RichTextBoxSQLSelect;
        private System.Windows.Forms.TextBox TextBoxToSearch;
        private System.Windows.Forms.ListView sqkResultListView;
        private System.Windows.Forms.ComboBox ComboBoxSortOn;
        private System.Windows.Forms.Button ButtonSearchLike;
        private System.Windows.Forms.Button ButtonSQLToggle;
        private System.Windows.Forms.Button ButtonOk;
    }
}