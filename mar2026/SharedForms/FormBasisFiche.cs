using ADODB;
using mar2026.Classes;
using mar2026.SharedForms;
using System;
using System.IO;
using System.Windows.Forms;
using static mar2026.Classes.AllFunctions;
using static mar2026.Classes.ModDatabase;
using static mar2026.Classes.ModLibs;

namespace mar2026
{
    public partial class FormBasisFiche : Form
    {
        public int flHere;

        public FormBasisFiche()
        {
            InitializeComponent();

            ComboBoxSearchOn.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void FormBasisFiche_Load(object sender, EventArgs e)
        {
            if (ComboBoxSearchOn.Items.Count == 0)
            {
                for (int t = 0; t <= FL_NUMBEROFINDEXEN[flHere]; t++)
                {
                    string sortOmsString = t.ToString("00") + ":" + FLINDEX_CAPTION[flHere, t];
                    string sortveldString = JETTABLEUSE_INDEX[flHere, t]?.Trim();
                    ComboBoxSearchOn.Items.Add(sortOmsString + " (" + sortveldString + ")");
                }

                if (ComboBoxSearchOn.Items.Count > 0)
                {
                    if (ComboBoxSearchOn.Items.Count > 1)
                    {
                        ComboBoxSearchOn.SelectedIndex = 1;
                    }
                    else
                    {
                        ComboBoxSearchOn.SelectedIndex = 0;
                    }
                }
            }
        }

        private void ButtonMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void ButtonFirst_Click(object sender, EventArgs e)
        {
            JetGetFirst(flHere, 0);
            if (KTRL != 0)
            {
                System.Media.SystemSounds.Beep.Play();
                MasketEditBoxInfo.Enabled = false;
            }
            else
            {
                BasisRecordNaarFiche();
                MasketEditBoxInfo.Enabled = true;
                ButtonPrev.Visible = false;
                ButtonNext.Visible = true;
            }
        }

        private void ButtonLast_Click(object sender, EventArgs e)
        {
            JetGetLast(flHere, 0);
            if (KTRL != 0)
            {
                // If KTRL is non‑zero: VB If KTRL Then
                System.Media.SystemSounds.Beep.Play();
                MasketEditBoxInfo.Enabled = false;
            }
            else
            {
                BasisRecordNaarFiche();
                MasketEditBoxInfo.Enabled = true;
                ButtonPrev.Visible = true;
                ButtonNext.Visible = false;
            }

        }

        private void ButtonNext_Click(object sender, EventArgs e)
        {
            JetGetNext(flHere, 0);
            if (KTRL != 0)
            {
                // If KTRL is non‑zero: VB If KTRL Then
                System.Media.SystemSounds.Beep.Play();
                MasketEditBoxInfo.Enabled = false;
                ButtonNext.Visible = false;
            }
            else
            {
                BasisRecordNaarFiche();
                MasketEditBoxInfo.Enabled = true;
                ButtonPrev.Visible = true;
            }
        }

        private void ButtonPrev_Click(object sender, EventArgs e)
        {
            JetGetPrev(flHere, 0);
            if (KTRL != 0)
            {
                // If KTRL is non‑zero: VB If KTRL Then
                System.Media.SystemSounds.Beep.Play();
                MasketEditBoxInfo.Enabled = false;
                ButtonPrev.Visible = false;
            }
            else
            {
                BasisRecordNaarFiche();
                MasketEditBoxInfo.Enabled = true;
                ButtonNext.Visible = true;
            }
        }

        private void ButtonRelating_Click(object sender, EventArgs e)
        {

        }

        private void ButtonSearchOn_Click(object sender, EventArgs e)
        {
            SHARED_FL = flHere;
            GRIDTEXT = MasketEditBoxInfo.Text.TrimEnd();
            A_INDEX = ComboBoxSearchOn.SelectedIndex;

            using (FormSQLSearch fss = new FormSQLSearch())
            {
                var result = fss.ShowDialog(this);
                if (result != DialogResult.OK)
                {
                    // User cancelled or closed the dialog – do not change current record
                    return;
                }
            }

            if (KTRL == 0)
            {
                // The search found a record: show its key and load fiche
                MasketEditBoxInfo.Text = XLOG_KEY;
                JetGet(flHere, 0, SetSpacing(MasketEditBoxInfo.Text, FLINDEX_LEN[flHere, 0]));

                INSERT_FLAG[flHere] = 0;
                BasisRecordNaarFiche();
                ButtonEdit.Enabled = true;
            }
            else
            {
                // No record found / user chose a non‑existing key: prepare for insert
                ButtonEdit.Enabled = false;
                MasketEditBoxInfo.Text = string.Empty;
                INSERT_FLAG[flHere] = 1;
            }
            MasketEditBoxInfo.Enabled = INSERT_FLAG[flHere] == 0;
        }


        private void ButtonRemove_Click(object sender, EventArgs e)
        {

        }

        private void ButtonEdit_Click(object sender, EventArgs e)
        {

        }

        // VB: Private Sub RecordNaarFiche()
        private void BasisRecordNaarFiche()
        {
            // Do nothing if BOF or EOF
            if (RS_MAR[flHere].BOF || RS_MAR[flHere].EOF)
            {
                return;
            }

            // Reset buffer for this file
            TLB_RECORD[flHere] = "";

            if (KTRL != 0)
            {
                MessageBox.Show("stop");
            }
            else
            {
                // Copy current record into buffer
                RecordToField(flHere);
            }

            // Build lastKey from primary index and show in textbox            
            MasketEditBoxInfo.Text = VBibTekst(flHere, JETTABLEUSE_INDEX[flHere, 0].TrimEnd());
            MaskedTextBoxDescription.Text = VBibTekst(flHere, JETTABLEUSE_INDEX[flHere, 1].TrimEnd());

            // INSERT_FLAG(FL) = 0  -> keep same global behaviour
            INSERT_FLAG[flHere] = 0;
        }

        // VB: Private Sub FicheNaarRecord()
        private void FicheNaarRecord()
        {
            // Move to record with current key or decide to insert
            string key = SetSpacing(MasketEditBoxInfo.Text, FLINDEX_LEN[flHere, 0]);
            JetGet(flHere, 0, key);

            if (KTRL == 0)
            {
                JetUpdate(flHere, 0);
            }
            else
            {
                JetInsert(flHere, 0);
            }
        }
    }
}
