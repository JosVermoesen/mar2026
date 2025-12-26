using mar2026.Classes;
using Mar2026;
using System;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Windows.Forms;
using static mar2026.Classes.AllFunctions;
using static mar2026.Classes.ModLibs;

namespace mar2026
{
    public partial class FormMim : Form
    {
        private string FULL_LINE;

        public FormMim()
        {
            InitializeComponent();            
        }

        private void FormMim_Load(object sender, EventArgs e)
        {
            if (!Properties.Settings.Default.IsUpgraded)
            {
                Properties.Settings.Default.Upgrade();
                Properties.Settings.Default.IsUpgraded = true;
                Properties.Settings.Default.Save();
            }

            if (Properties.Settings.Default.MainTop <= 0)
            {
                Width = 816;
                Height = 489;
                StartPosition = FormStartPosition.CenterScreen;
            }
            else
            {
                Top = Properties.Settings.Default.MainTop;
                Left = Properties.Settings.Default.MainLeft;
                Width = Properties.Settings.Default.MainWidth;
                Height = Properties.Settings.Default.MainHeight;
            }
                        
            toolStripStatusBookingsDate.Text = DateTime.Now.ToString("dd/MM/yyyy");

            // PROGRAM_LOCATION = App.Path + "\"
            ModLibs.PROGRAM_LOCATION = Application.StartupPath.TrimEnd(Path.DirectorySeparatorChar) + Path.DirectorySeparatorChar;

            // Build caption like: App.Title & " v." & App.Major & "." & App.Minor & "." & App.Revision
            // Here we use ProductName + ProductVersion
            // var version = new Version(Application.ProductVersion);
            // Text = Application.ProductName + " v." +
            //        version.Major + "." + version.Minor + "." + version.Build;

            // Demo defaults from VB6
            ModLibs.usrMailAdres = "demo@rv.be";
            ModLibs.usrPW = "9999";

            // Application Data (CSIDL_APPDATA equivalent) – if you still need it
            var appDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            // If you used PROGRAM_LOCATION or other globals depending on this, set them here.

            // Personal documents (CSIDL_PERSONAL)
            var personalFolder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            ModLibs.SYSTEM_MYPERSONALDOCUMENTS = personalFolder;
                        
            ModLibs.LOCATION_MYDOCUMENTS =
                LoadText(Application.ProductName, "Bedrijfsinhoudsopgave" + "2025");

            // BeWaarTekst "Programma", "LOCATION_", App.path
            SaveText("Mar2026", "LOCATION_", Application.StartupPath);

            // First-time / transition handling: ensure LOCATION_MYDOCUMENTS
            if (string.IsNullOrWhiteSpace(ModLibs.LOCATION_MYDOCUMENTS))
            {
                // Equivalent of VB6 GoSub MaakBewaarDataFolder
                // EnsureBewaarDataFolder(personalFolder);

                // If we successfully set LOCATION_MYDOCUMENTS, reload the stored value
                if (!string.IsNullOrWhiteSpace(ModLibs.LOCATION_MYDOCUMENTS))
                {
                    ModLibs.LOCATION_MYDOCUMENTS =
                        LoadText(Application.ProductName, "Bedrijfsinhoudsopgave" + "2025");
                }
            }

            Cursor = Cursors.WaitCursor;

            // MIM_GLOBAL_DATE = Format(Now, "dd/mm/yyyy")
            ModLibs.MIM_GLOBAL_DATE = DateTime.Now.ToString("dd/MM/yyyy");

            // Toolbar combo – you already had similar code but VB6 did not set SelectedIndex
            cmdWegBoekModus.Items.Clear();
            cmdWegBoekModus.Items.Add("0: Geen BoekingsInfo tonen (EUROTEST niet actief)");
            cmdWegBoekModus.Items.Add("1: Enkel BoekingsInfo tonen bij EUR<>BEF verschil");
            cmdWegBoekModus.Items.Add("2: Altijd BoekingsInfo tonen");
            cmdWegBoekModus.SelectedIndex = 2;

            // Load vsoft.ini settings
            // var iniPath = Path.Combine(ModLibs.PROGRAM_LOCATION, "vsoft.ini");
            //if (!File.Exists(iniPath))
            //{
            //    MessageBox.Show(
            //        "VSOFT.INI niet te vinden.  Installeer korrekt a.u.b.",
            //        "Fout",
            //        MessageBoxButtons.OK,
            //        MessageBoxIcon.Error);

            //    // Equivalent of VB6 End: exit application
            //    Application.Exit();
            //    return;
            //}

            //foreach (var line in File.ReadAllLines(iniPath))
            //{
            //    var trimmed = line ?? string.Empty;
            //    if (trimmed.Length < 9) continue;

            //    var key = trimmed.Substring(0, 9).ToLowerInvariant();

            //    // programma* is ignored in VB6
            //    if (key == "assurnet ")
            //    {
            //        // LOCATION_ASWEB = <value> & Format(Now, "MMSS")
            //        var value = GetIniValue(trimmed);
            //        ModLibs.LOCATION_ASWEB = value + DateTime.Now.ToString("mmss");
            //    }
            //    else if (key == "producent")
            //    {
            //        // ProducentNummer = <value>
            //        ModLibs.ProducentNummer = GetIniValue(trimmed);
            //    }
            //}

            ModLibs.LOCATION_ = (ModLibs.LOCATION_MYDOCUMENTS ?? string.Empty).TrimEnd('\\') + "\\";

            // PERIOD_FROMTO = ""
            ModLibs.PERIOD_FROMTO = string.Empty;
                        
            InitFirst();

            // ADO → ADO.NET portability: open Access databases
            // Provider=Microsoft.Jet.OLEDB.4.0; Data Source=PROGRAM_LOCATION + "Default2022.mdb";
            try
            {
                var dataDir = Path.Combine(Application.StartupPath, "MdX");

                // Debug / verification
                var kbPath = Path.Combine(dataDir, "Default2022.mdv");
                var tbPath = Path.Combine(dataDir, "Telebib2.mdv");

                if (!File.Exists(kbPath) || !File.Exists(tbPath))
                {
                    MessageBox.Show(
                        "MDB not found.\n\nStartupPath: " + Application.StartupPath +
                        "\n\nExpected:\n" + kbPath + "\n" + tbPath,
                        "Pad fout",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    Application.Exit();
                    return;
                }

                var kbConnStr = "Provider=Microsoft.Jet.OLEDB.4.0;" +
                                "Data Source=" + kbPath + ";";

                var tbConnStr = "Provider=Microsoft.Jet.OLEDB.4.0;" +
                                "Data Source=" + tbPath + ";";

                ModLibs.adKBDB = new OleDbConnection(kbConnStr);
                ModLibs.adKBDB.Open();

                ModLibs.adTBIB = new OleDbConnection(tbConnStr);
                ModLibs.adTBIB.Open();

                ModLibs.adKBTable = new DataTable("KeuzeBoxData");
                using (var da = new OleDbDataAdapter("SELECT * FROM KeuzeBoxData", ModLibs.adKBDB))
                {
                    da.Fill(ModLibs.adKBTable);
                }
            }
            catch (Exception ex)
            {
                Cursor = Cursors.Default;
                MessageBox.Show(
                    "Fout bij openen van Access databanken (Default2022.mdb / Telebib2.mdb):" +
                    Environment.NewLine + ex.Message,
                    "Databasefout",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                Application.Exit();
                return;
            }

            // MIM_GLOBAL_DATE = Format(Now, "dd/mm/yyyy")
            ModLibs.MIM_GLOBAL_DATE = DateTime.Now.ToString("dd/MM/yyyy");
            Cursor = Cursors.Default;
            cmdWegBoekModus.Items.Clear();
            cmdWegBoekModus.Items.Add("0: Geen BoekingsInfo tonen (EUROTEST niet actief)");
            cmdWegBoekModus.Items.Add("1: Enkel BoekingsInfo tonen bij EUR<>BEF verschil");
            cmdWegBoekModus.Items.Add("2: Altijd BoekingsInfo tonen");
            cmdWegBoekModus.SelectedIndex = 2;

            // Pre-initialize array slots, but do NOT create or show the forms here.
            // They will be created with fixed size in ShowBasisFiche when first used.
            for (int i = 1; i <= 3; i++)
            {
                if (ModLibs.BasisB[i] == null || ModLibs.BasisB[i].IsDisposed)
                {
                    ModLibs.BasisB[i] = new FormBasisFiche
                    {
                        MdiParent = this
                    };

                    // Per-index customization
                    switch (i)
                    {
                        case 1:
                            ModLibs.BasisB[1].Text = "Fiche Klanten";
                            ModLibs.BasisB[1].BackColor = System.Drawing.Color.Blue;
                            break;

                        case 2:
                            ModLibs.BasisB[2].Text = "Fiche Leveranciers";
                            ModLibs.BasisB[2].BackColor = System.Drawing.Color.Red;
                            break;

                        case 3:
                            ModLibs.BasisB[3].Text = "Rekening Fiche";
                            ModLibs.BasisB[3].BackColor = System.Drawing.Color.White;
                            break;
                    }
                    // Show as MDI child, minimized and disabled at startup
                    ModLibs.BasisB[i].Show();
                    ModLibs.BasisB[i].WindowState = FormWindowState.Minimized;
                    ModLibs.BasisB[i].Enabled = false;
                }
            }
            ModLibs.FormReference = ModLibs.BasisB[1];
            FormReference = null;
            Cursor = Cursors.Default;

            // Equivalent of BedrijfOpenen.Show
            var openCompany = new mar2026.Forms.FormOpenCompany
            {
                MdiParent = this
            };
            openCompany.Show();
        }

        private void FormMim_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.MainTop = Top;
            Properties.Settings.Default.MainLeft = Left;
            Properties.Settings.Default.MainWidth = Width;
            Properties.Settings.Default.MainHeight = Height;            
            Properties.Settings.Default.Save();
        }
                
        private void TbOpen_Click(object sender, EventArgs e)
        {
            // Toolbar Open delegates to Acties/Bedrijf Openen.
            MenuActiesBedrijfOpenen_Click(sender, e);
        }

        private void TbSqlZoek_Click(object sender, EventArgs e)
        {
            // Toolbar SQLZoek corresponds to Systeem / SQL bewerkingen (later).
        }

        private void TbVsoft_Click(object sender, EventArgs e)
        {
            // Toolbar Map Manueel corresponds to Cloud4MAR / Manuele Documenten (later).
        }

        private void TbServer2_Click(object sender, EventArgs e)
        {
            // Toolbar Archief corresponds to Cloud4MAR / Archief (later).
        }        

        private void DatumVerwerking_ValueChanged(object sender, EventArgs e)
        {
            // VB6 stored this into MIM_GLOBAL_DATE and copied to BJPERDAT.DatumVerwerking.
            ModLibs.MIM_GLOBAL_DATE = this.toolStripStatusBookingsDate.Text;
        }

        private void CmdWegBoekModus_SelectedIndexChanged(object sender, EventArgs e)
        {
            // VB6 saved this choice and then disabled the combo on Enter; here we only keep selection.
        }

        // === Acties menu handlers (port of Basis_Click branches) ===

        private void MenuActiesBedrijfOpenen_Click(object sender, EventArgs e)
        {
            // VB6: Basis_Click Index 0 – GaVerder, AutoUnloadBedrijf, show BedrijfOpenen.
            // Here we only mark the intent for now.
            MessageBox.Show("Bedrijf openen: nog te porteren logica.", "Acties", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void MenuActiesNieuwBedrijf_Click(object sender, EventArgs e)
        {
            // VB6: Basis_Click Index 1 – GaVerder, AutoUnloadBedrijf, NieuwBedrijf.Show 1
            MessageBox.Show("Nieuw bedrijf installeren: nog te porteren logica.", "Acties", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void MenuActiesBedrijfSluiten_Click(object sender, EventArgs e)
        {
            // VB6: Basis_Click Index 3 – AutoUnloadBedrijf, ntDB.Close, lock-file check
            MessageBox.Show("Bedrijf sluiten: nog te porteren logica.", "Acties", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void MenuActiesMarSync_Click(object sender, EventArgs e)
        {
            // VB6: Basis_Click Index 4 – AutoUnloadBedrijf, DetectClickOnceShortcut
            MessageBox.Show("MarSync starten: nog te porteren logica (DetectClickOnceShortcut).", "Acties", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void MenuActiesTaakbalkVergrendeld_Click(object sender, EventArgs e)
        {
            // VB6: toggled Basis(6).Checked and enabled/disabled toolbar widgets.
            bool locked = this.MenuActiesTaakbalkVergrendeld.Checked;

            this.cmdWegBoekModus.Enabled = !locked;

        }

        private void MenuActiesTaakbalkZichtbaar_Click(object sender, EventArgs e)
        {
            // VB6: toggled Basis(8).Checked and tbToolBar.Visible, persisted setting.
            // this.toolBar.Visible = this.MenuActiesTaakbalkZichtbaar.Checked;
            // Persist via your own settings mechanism when ready.
        }

        private void MenuActiesManager2009_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Normal;
            Size = new System.Drawing.Size(1152, 864);
            CenterToScreen();
        }

        private void MenuActiesManager2005_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Normal;
            Size = new System.Drawing.Size(1024, 768);
            CenterToScreen();
        }

        private void MenuActiesXmlRekenbladen_Click(object sender, EventArgs e)
        {
            // VB6: Basis_Click Index 12 – frmRekenBlad.Show
            MessageBox.Show("XML Rekenbladen: nog te porteren logica.", "Acties", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void MenuActiesAfsluiten_Click(object sender, EventArgs e)
        {
            Close();
        }
        
        private void MenuCloud4MarManueel_Click(object sender, EventArgs e)
        {
            // VB6: DNN(0) – open URL from LaadTekst("dnnInstellingen", "Mario")
            MessageBox.Show("Cloud4MAR Manuele Documenten: URL-launch logica nog te porteren.",
                "Cloud4MAR", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void MenuCloud4MarArchief_Click(object sender, EventArgs e)
        {
            // VB6: DNN(1) – LaadTekst("dnnInstellingen", "Archief")
            MessageBox.Show("Cloud4MAR Archief: URL-launch logica nog te porteren.",
                "Cloud4MAR", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void MenuCloud4MarInstellingen_Click(object sender, EventArgs e)
        {
            using (FormCloudSetting dlg = new FormCloudSetting())
            {
                dlg.ShowDialog();
            }
        }

        private void MenuCloud4MarWebsite_Click(object sender, EventArgs e)
        {
            ShellExecuteWithFallback("https://rv.be/accounting");
        }
        
        private void MenuInfoCommissie_Click(object sender, EventArgs e)
        {
            ShellExecuteWithFallback("https://www.cbn-cnc.be/nl");
        }

        private void MenuInfoLicentie_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Licentie toewijzing-scherm nog te porteren.",
                "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void MenuInfoCmd_Click(object sender, EventArgs e)
        {
            ShellExecuteWithFallback("cmd");
        }

        private void MenuPeppolValidator_Click(object sender, EventArgs e)
        {
            ShellExecuteWithFallback("https://peppol-tools.ademico-software.com/ui/document-validator");
        }

        private void MenuPeppolDocs_Click(object sender, EventArgs e)
        {
            ShellExecuteWithFallback("https://docs.peppol.eu/poacc/billing/3.0/");
        }

        private void MenuPeppolClickOnce_Click(object sender, EventArgs e)
        {
            ShellExecuteWithFallback("https://clickonce.vsoft.be/MarSync/publish.htm");
        }

                
        public static string VSet(string text, short length)
        {
            return SetSpacing(text ?? string.Empty, length);
        }

        private void MenuWindowCascade_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.Cascade);
        }

        private void MenuWindowTileVertical_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileVertical);
        }

        private void MenuWindowTileHorizontal_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileHorizontal);
        }

        private void MenuWindowArrangeIcons_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.ArrangeIcons);
        }
                
        private void MenuOpenFormsChild_Click(object sender, EventArgs e)
        {
            if (sender is ToolStripMenuItem menuItem && menuItem.Tag is Form form)
            {
                form.WindowState = FormWindowState.Normal;
                form.BringToFront();
                form.Activate();
            }
        }

        private void MenuActionsCloseApp_Click(object sender, EventArgs e)
        {
            foreach (Form child in MdiChildren)
            {
                child.Close();
            }
            Close();
        }

        private void MenuActionsOpenCompany_Click(object sender, EventArgs e)
        {
            // Show the company open dialog as an MDI child (like VB6 BedrijfOpenen)
            var dlg = new mar2026.Forms.FormOpenCompany
            {
                MdiParent = this
            };
            dlg.Show();
        }

        private void InitFirst()
        {
            TABLEDEF_ONT[TABLE_VARIOUS] = "0000000.ONT";   // 00
            TABLEDEF_ONT[TABLE_CUSTOMERS] = "0010000.ONT";   // 01
            TABLEDEF_ONT[TABLE_SUPPLIERS] = "0020000.ONT";   // 02
            TABLEDEF_ONT[TABLE_LEDGERACCOUNTS] = "0030000.ONT"; // 03
            TABLEDEF_ONT[TABLE_PRODUCTS] = "0040000.ONT";   // 04
            TABLEDEF_ONT[TABLE_JOURNAL] = "0600000.ONT";   // 05
            TABLEDEF_ONT[TABLE_INVOICES] = "0200000.ONT";   // 06
            TABLEDEF_ONT[TABLE_CONTRACTS] = "0700000.ONT";   // 07
            TABLEDEF_ONT[TABLE_DUMMY] = "90DUMMY.ONT";   // 08
            TABLEDEF_ONT[TABLE_COUNTERS] = "00.ONT";       // 09

            bstNaam[TABLE_VARIOUS] = "Allerlei";      // 00
            bstNaam[TABLE_CUSTOMERS] = "Klanten";       // 01
            bstNaam[TABLE_SUPPLIERS] = "Leveranciers";  // 02
            bstNaam[TABLE_LEDGERACCOUNTS] = "Rekeningen";    // 03
            bstNaam[TABLE_PRODUCTS] = "Produkten";     // 04
            bstNaam[TABLE_JOURNAL] = "Journalen";     // 05
            bstNaam[TABLE_INVOICES] = "dokumenten";    // 06
            bstNaam[TABLE_CONTRACTS] = "Polissen";      // 07
            bstNaam[TABLE_DUMMY] = "TmpBestand";    // 08
            bstNaam[TABLE_COUNTERS] = "Tell";          // 09

            DAYS_IN_MONTH[1] = 31;
            DAYS_IN_MONTH[2] = 29;
            DAYS_IN_MONTH[3] = 31;
            DAYS_IN_MONTH[4] = 30;
            DAYS_IN_MONTH[5] = 31;
            DAYS_IN_MONTH[6] = 30;
            DAYS_IN_MONTH[7] = 31;
            DAYS_IN_MONTH[8] = 31;
            DAYS_IN_MONTH[9] = 30;
            DAYS_IN_MONTH[10] = 31;
            DAYS_IN_MONTH[11] = 30;
            DAYS_IN_MONTH[12] = 31;

            MONTH_AS_TEXT[1] = "Januari  ";
            MONTH_AS_TEXT[2] = "Februari ";
            MONTH_AS_TEXT[3] = "Maart    ";
            MONTH_AS_TEXT[4] = "April    ";
            MONTH_AS_TEXT[5] = "Mei      ";
            MONTH_AS_TEXT[6] = "Juni     ";
            MONTH_AS_TEXT[7] = "Juli     ";
            MONTH_AS_TEXT[8] = "Augustus ";
            MONTH_AS_TEXT[9] = "September";
            MONTH_AS_TEXT[10] = "October  ";
            MONTH_AS_TEXT[11] = "November ";
            MONTH_AS_TEXT[12] = "December ";
        }



        //public static object XrsMar(short fl, string TBS)
        //{
        //    var value = RS_MAR[fl].Fields[TBS].Value;
        //    return (value == null || value is System.DBNull) ? string.Empty : value;
        //}

        
        private void ToolStripCustomers_Click(object sender, EventArgs e)
        {
            ShowBasisFiche(1);
        }

        private void ToolStripSuppliers_Click(object sender, EventArgs e)
        {
            ShowBasisFiche(2);
        }

        private void ToolStripLedgerAccounts_Click(object sender, EventArgs e)
        {
            ShowBasisFiche(3);
        }

        private void MenuListOpenForms_DropDownOpening(object sender, EventArgs e)
        {
            var menu = sender as ToolStripMenuItem;
            if (menu == null) return;

            menu.DropDownItems.Clear();
            foreach (Form child in MdiChildren)
            {
                var item = new ToolStripMenuItem
                {
                    Text = child.Text,
                    Tag = child
                };
                item.Click += MenuOpenFormsChild_Click;
                menu.DropDownItems.Add(item);
            }
        }

        private void ShowBasisFiche(int index)
        {
            if (index < 1 || index > 3)
                return;

            if (BasisB[index] == null || BasisB[index].IsDisposed)
            {
                BasisB[index] = new FormBasisFiche
                {
                    MdiParent = this
                };

                switch (index)
                {
                    case 1:
                        BasisB[1].Text = "Fiche Klanten";
                        BasisB[1].BackColor = System.Drawing.Color.Blue;
                        break;
                    case 2:
                        BasisB[2].Text = "Fiche Leveranciers";
                        BasisB[2].BackColor = System.Drawing.Color.Red;
                        break;
                    case 3:
                        BasisB[3].Text = "Rekening Fiche";
                        BasisB[3].BackColor = System.Drawing.Color.White;
                        break;
                }

                BasisB[index].Show();
            }

            var form = BasisB[index];

            // Force fixed size every time you show/activate
            form.MinimumSize = new System.Drawing.Size(327, 149);
            form.MaximumSize = new System.Drawing.Size(327, 149);
            form.Size = new System.Drawing.Size(327, 149);

            form.WindowState = FormWindowState.Normal;
            form.Enabled = true;
            form.BringToFront();
            form.Activate();

            FormReference = form;
        }
        public void ShowBJPERDAT()
        {
            foreach (Form child in this.MdiChildren)
            {
                if (child is FormBJPERDAT existing)
                {
                    existing.WindowState = FormWindowState.Normal;
                    existing.BringToFront();
                    return;
                }
            }

            var frm = new FormBJPERDAT
            {
                MdiParent = this
            };
            frm.Show();
        }
    }
}



