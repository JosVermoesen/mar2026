using ADODB;
using System;
using System.IO;
using System.Windows.Forms;

using mar2026.Classes;
using static mar2026.Classes.AllFunctions;
using static mar2026.Classes.ModDatabase;

using static mar2026.Classes.ModLibs;

namespace mar2026
{
    public partial class FormBasisFiche : Form
    {
        public int flHere;
        private string lastKey; // holds last key like in VB

        public FormBasisFiche()
        {            
            InitializeComponent();
            // this.MinimumSize = new System.Drawing.Size(327, 149);
            // this.MaximumSize = new System.Drawing.Size(327, 149);
        }

        private void FormBasisFiche_Load(object sender, EventArgs e)
        {
            // VB: If sorteringComboBox.Items.Count Then ... Else ...
            if (ComboBoxSearchOn.Items.Count == 0)
            {
                for (int t = 0; t <= FL_NUMBEROFINDEXEN[flHere]; t++)
                {
                    // Dim sortOmsString As String = Format(T, "00") & ":" & FLINDEX_CAPTION(hierFl, T)
                    string sortOmsString = t.ToString("00") + ":" + FLINDEX_CAPTION[flHere, t];

                    // Dim sortveldString As String = Trim(JETTABLEUSE_INDEX(hierFl, T))
                    string sortveldString = JETTABLEUSE_INDEX[flHere, t]?.Trim();

                    // sorteringComboBox.Items.Add(sortOmsString & " (" & sortveldString & ")")
                    ComboBoxSearchOn.Items.Add(sortOmsString + " (" + sortveldString + ")");
                }

                if (ComboBoxSearchOn.Items.Count > 0)
                {
                    // VB uses 1-based, we keep index 1 if it exists
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
                // If KTRL is non‑zero: VB If KTRL Then
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
            //JetGetLast(flHere, 0);
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

        }

        private void ButtonPrev_Click(object sender, EventArgs e)
        {

        }

        private void ButtonRelating_Click(object sender, EventArgs e)
        {

        }

        private void ButtonSearchOn_Click(object sender, EventArgs e)
        {

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
            lastKey = VBibTekst(flHere, JETTABLEUSE_INDEX[flHere, 0].TrimEnd());
            MasketEditBoxInfo.Text = lastKey;

            // INSERT_FLAG(FL) = 0  -> keep same global behaviour
            INSERT_FLAG[flHere] = 0;
        }

        // VB: Private Sub FicheNaarRecord()
        private void FicheNaarRecord()
        {
            // Move to record with current key or decide to insert
            string key = SetSpacing(MasketEditBoxInfo.Text, FLINDEX_LEN[flHere, 0]);
            JetGet(flHere, 0, ref key);

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
