using System;
using System.Windows.Forms;
using static mar2026.Classes.ModLibs;
using static mar2026.Classes.ModDatabase;
using static mar2026.Classes.ModBtrieve;
using static mar2026.Classes.MimTools;

namespace mar2026
{
    public partial class FormBasisFicheTemp : Form
    {
        private ComboBox cmbSortering;
        private Button[] btnKnop; // index 0..10 (we only create what is used)
        private MaskedTextBox txtTekstInfo0;

        private int _fl; // current table (Fl in VB)

        public FormBasisFicheTemp(int fl)
        {
            _fl = fl;
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.cmbSortering = new ComboBox();
            this.txtTekstInfo0 = new MaskedTextBox();
            this.btnKnop = new Button[11];

            // Form
            this.Text = "BasisFiche";
            this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            this.ControlBox = false;
            this.ShowInTaskbar = false;
            this.WindowState = FormWindowState.Minimized;
            this.StartPosition = FormStartPosition.Manual;
            this.MdiParent = Application.OpenForms["FormMim"];

            // txtTekstInfo0 (TekstInfo(0))
            this.txtTekstInfo0.Location = new System.Drawing.Point(114, 12);
            this.txtTekstInfo0.Width = 295;
            this.txtTekstInfo0.TabIndex = 0;
            this.txtTekstInfo0.TextChanged += TxtTekstInfo0_TextChanged;
            this.txtTekstInfo0.GotFocus += TxtTekstInfo0_GotFocus;
            this.txtTekstInfo0.KeyDown += TxtTekstInfo0_KeyDown;
            this.txtTekstInfo0.ToolTipText("Breng hier fiche UNIEKE ID-kode in !");

            // cmbSortering
            this.cmbSortering.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbSortering.Location = new System.Drawing.Point(114, 126);
            this.cmbSortering.Width = 295;
            this.cmbSortering.TabIndex = 1;
            this.cmbSortering.GotFocus += CmbSortering_GotFocus;

            // Buttons (Knop array)
            // Index mapping: 0=Top,2=Zoeken,3=Nieuwe,4=Relaties,5=Bewerken,
            // 6=Lager,7=Hoger,8=Minimaliseren,9=Bodem,10=Verwijderen

            btnKnop[0] = CreateButton("Top", 114, 54, 0, "Eerste fiche in rij");
            btnKnop[2] = CreateButton("Zoeken op...", 0, 126, 2, "GeSELECTeerd zoeken (ANSI-92 SQL)");
            btnKnop[3] = CreateButton("Nieuwe", 0, 0, 3, "Nieuwe fiche"); // not visible in VB, but we keep entry
            btnKnop[4] = CreateButton("Relaties", 300, 54, 4, "Dokumenten en journaaldetail");
            btnKnop[5] = CreateButton("Bewerken", 0, 12, 5, "Aktieve fiche wijzigen en/of in detail bekijken");
            btnKnop[6] = CreateButton("Lager", 192, 54, 6, "Vorige");
            btnKnop[7] = CreateButton("Hoger", 192, 90, 7, "Volgende");
            btnKnop[8] = CreateButton("Minimaliseren", 300, 90, 8, "Venster minimaliseren");
            btnKnop[9] = CreateButton("Bodem", 114, 90, 9, "Laatste fiche in rij");
            btnKnop[10] = CreateButton("Verwijderen", 0, 90, 10, "Aktieve fiche verwijderen");

            btnKnop[5].Enabled = false; // default same as VB
            btnKnop[5].TabStop = false;

            // Wire clicks
            for (int i = 0; i < btnKnop.Length; i++)
            {
                if (btnKnop[i] != null)
                {
                    int idx = i;
                    btnKnop[i].Click += (s, e) => Knop_Click(idx);
                }
            }

            // Layout
            this.ClientSize = new System.Drawing.Size(423, 174);
            this.Controls.Add(this.txtTekstInfo0);
            this.Controls.Add(this.cmbSortering);
            foreach (var b in btnKnop)
                if (b != null) this.Controls.Add(b);

            this.Load += FormBasisFiche_Load;
            this.Activated += FormBasisFiche_Activated;
        }

        private Button CreateButton(string text, int left, int top, int index, string toolTip)
        {
            var b = new Button();
            b.Text = text;
            b.Left = left;
            b.Top = top;
            b.Width = 75;
            b.Height = 30;
            b.TabStop = false;
            var tt = new ToolTip();
            tt.SetToolTip(b, toolTip);
            return b;
        }

        // --- VB: Form_Load equivalent is mostly handled by constructor/Init; we use Activated like VB ---

        private void FormBasisFiche_Load(object sender, EventArgs e)
        {
            // Nothing yet; behavior is in Activated like VB6
        }

        private void FormBasisFiche_Activated(object sender, EventArgs e)
        {
            // VB6: Fl = Val(Me.Tag)
            int fl = _fl;

            if (string.IsNullOrEmpty(LOCATION_COMPANYDATA))
                return;

            if (cmbSortering.Items.Count == 0)
            {
                for (int t = 0; t <= FL_NUMBEROFINDEXEN[fl]; t++)
                {
                    string item = t.ToString("00") + ":" + FLINDEX_CAPTION[fl, t] +
                                  " (" + (JETTABLEUSE_INDEX[fl, t]?.Trim() ?? "") + ")";
                    cmbSortering.Items.Add(item);
                }

                // Try to restore last sort choice if you have LoadText equivalent
                // string saved = MimTools.LoadText(this.Name, "ProduktSortering");
                // if (!string.IsNullOrEmpty(saved)) cmbSortering.Text = saved;
                if (cmbSortering.Items.Count > 1 && cmbSortering.SelectedIndex < 0)
                    cmbSortering.SelectedIndex = 1;
            }

            NieuweFiche(fl);
        }

        // --- Core VB methods port ---

        private void FicheNaarRecord(int fl)
        {
            // bGet fl, 0, vSet(TekstInfo(0).Text, FLINDEX_LEN(fl,0))
            ModDatabase.bGet(fl, 0, MimTools.VSet(txtTekstInfo0.Text, FLINDEX_LEN[fl, 0]));
            if (KTRL == 0)
                ModBtrieve.BUpdate(fl, 0);
            else
                ModBtrieve.BInsert(fl, 0);

            Knop_Click(3);
        }

        private void Knop_Click(int index)
        {
            int fl = _fl;

            switch (index)
            {
                case 0: // Eerste
                    ModBtrieve.BFirst(fl, 0);
                    if (KTRL != 0)
                    {
                        System.Media.SystemSounds.Beep.Play();
                        btnKnop[5].Enabled = false;
                    }
                    else
                    {
                        INSERT_FLAG[fl] = 0;
                        RecordNaarFiche(fl);
                        btnKnop[5].Enabled = true;
                    }
                    break;

                case 2: // GeSELECTeerd zoeken
                    // Only wired minimal: TODO port SqlSearch, Venster etc.
                    // Here we just keep current logic stubbed.
                    break;

                case 3: // Nieuwe Fiche
                    NieuweFiche(fl);
                    break;

                case 4: // Relaties / Boekhouding etc. in VB; left for later
                    break;

                case 5: // Fiche EDITEREN
                    string teZoeken = txtTekstInfo0.Text.Trim();
                    if (string.IsNullOrEmpty(teZoeken))
                    {
                        System.Media.SystemSounds.Beep.Play();
                        return;
                    }

                    ModDatabase.bGet(fl, 0, teZoeken);
                    if (KTRL == 0)
                    {
                        INSERT_FLAG[fl] = 0;
                        RecordNaarFiche(fl);
                        btnKnop[5].Enabled = true;
                    }
                    else
                    {
                        NieuweFiche(fl);
                        txtTekstInfo0.Text = teZoeken;
                    }
                    break;

                case 6: // Fiche Lager (Prev)
                    ModBtrieve.BPrev(fl);
                    if (KTRL == 9)
                    {
                        ModBtrieve.BFirst(fl, 0);
                        if (KTRL != 0)
                        {
                            System.Media.SystemSounds.Beep.Play();
                            btnKnop[5].Enabled = false;
                        }
                    }
                    if (KTRL == 0)
                    {
                        INSERT_FLAG[fl] = 0;
                        RecordNaarFiche(fl);
                        btnKnop[5].Enabled = true;
                    }
                    break;

                case 7: // Fiche Hoger (Next)
                    ModBtrieve.BNext(fl);
                    if (KTRL == 9)
                    {
                        ModBtrieve.BLast(fl, 0);
                        if (KTRL != 0)
                        {
                            System.Media.SystemSounds.Beep.Play();
                            btnKnop[5].Enabled = false;
                        }
                    }
                    if (KTRL == 0)
                    {
                        INSERT_FLAG[fl] = 0;
                        RecordNaarFiche(fl);
                        btnKnop[5].Enabled = true;
                    }
                    break;

                case 8: // Minimaliseren
                    this.WindowState = FormWindowState.Minimized;
                    break;

                case 9: // Bodem (last)
                    ModBtrieve.BLast(fl, 0);
                    if (KTRL != 0)
                    {
                        System.Media.SystemSounds.Beep.Play();
                        btnKnop[5].Enabled = false;
                    }
                    else
                    {
                        INSERT_FLAG[fl] = 0;
                        RecordNaarFiche(fl);
                        btnKnop[5].Enabled = true;
                    }
                    break;

                case 10: // Verwijderen
                    if (INSERT_FLAG[fl] == 0)
                    {
                        string msg = "Bestaande fiche " + this.Text + " verwijderen.  Bent U zeker ?";
                        var res = MessageBox.Show(msg, "Verwijderen", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                        if (res == DialogResult.Yes)
                        {
                            ModBtrieve.BDelete(fl);
                            Knop_Click(3);
                        }
                    }
                    break;
            }
        }

        private void NieuweFiche(int fl)
        {
            // daoBlankoRecord equivalent:
            ModBtrieve.DaoBlankoRecord(fl);

            txtTekstInfo0.Text = string.Empty;
            INSERT_FLAG[fl] = 1;
            btnKnop[5].Enabled = false;
            btnKnop[5].Focus();
            txtTekstInfo0.Enabled = true;
            txtTekstInfo0.Focus();
        }

        private void RecordNaarFiche(int fl)
        {
            TLB_RECORD[fl] = string.Empty;
            if (KTRL == 0)
            {
                ModDatabase.RecordToVeld(fl);
            }

            txtTekstInfo0.Text = ModDatabase.VBibTekst(fl, "#" + JETTABLEUSE_INDEX[fl, 0] + "#");

            MSG = string.Empty;
            for (int t = 0; t <= 1; t++)
            {
                MSG += (FVT[fl, t]?.Trim() ?? string.Empty) + " ";
            }

            // SnelHelpPrint(MSG, BL_LOGGING) equivalent:
            LOG_PRINT += MSG + Environment.NewLine;
            INSERT_FLAG[fl] = 0;
        }

        // --- events matching VB behavior ---

        private void CmbSortering_GotFocus(object sender, EventArgs e)
        {
            // Knop(2).Default = True
            this.AcceptButton = btnKnop[2];
        }

        private void TxtTekstInfo0_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtTekstInfo0.Text))
            {
                INSERT_FLAG[_fl] = 1;
                btnKnop[5].Enabled = false;
            }
            else
            {
                btnKnop[5].Enabled = true;
            }
        }

        private void TxtTekstInfo0_GotFocus(object sender, EventArgs e)
        {
            // SnelHelpPrint TekstInfo(0).ToolTipText & ", " & "[Ctrl] voor geSELECTeerd zoeken !"
            LOG_PRINT += (txtTekstInfo0.ToolTipText + ", [Ctrl] voor geSELECTeerd zoeken !") + Environment.NewLine;
            this.AcceptButton = btnKnop[5];
            txtTekstInfo0.SelectAll();
        }

        private void TxtTekstInfo0_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ControlKey)
            {
                // Ctrl pressed: call SqlSearch equivalent when ported
                // aIndex = Val(Left(cmbSortering.Text,2))
                // SharedFl = Fl
                // GridText = TekstInfo(0).Text
                // SqlSearch.Show 1
                // If Ktrl = 0 Then ...
            }
        }
    }
}