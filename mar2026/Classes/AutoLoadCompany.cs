using System;
using System.IO;
using System.Windows.Forms;
using ADODB;

using static mar2026.Classes.AllFunctions;
using static mar2026.Classes.ModLibs;

namespace mar2026.Classes
{
    /// <summary>
    /// Port of VB6 AutoLoadBedrijf: prepares active bookyear/period,
    /// opens databases (Jet/SQL Server), ensures schema upgrades, and
    /// initializes masks and Peppol/QR/etc. folders.
    /// This is still a partial port and should be completed step by step.
    /// </summary>
    public static class AutoLoadCompany
    {
        public static void Run(FormMim mim, FormBJPERDAT bjPerDat)
        {
            // NOTE: This is a skeleton. Only a small, safe subset of the VB6
            // logic is wired; the rest should be brought over gradually.

            // Reset some UI state similar to VB6.
            if (mim == null || bjPerDat == null)
                throw new ArgumentNullException("Main forms must not be null.");
                        
            XisEUROWasBEF = false;

            SharedGlobals.MimLoadingNewCompany = true;

            // Menus/toolbars: enable a minimal subset (adjust when you port more).
            // VB6 enabled MenuTitel(1..5), BasisB(1..4), Basis(11).
            // Here we assume corresponding menu items already exist on FormMim.

            // Sync processing date on forms
            bjPerDat.DatumVerwerking.Value = DateTime.ParseExact(
                MIM_GLOBAL_DATE ?? DateTime.Now.ToString("dd/MM/yyyy"),
                "dd/MM/yyyy",
                System.Globalization.CultureInfo.InvariantCulture);
            
            // mim.toolStripStatusLabelBookingsDate.Text =
            //         bjPerDat.DatumVerwerking.Value.ToString("dd/MM/yyyy");

            bjPerDat.CmbPeriodeBoekjaar.Items.Clear();

            // Optional: prepare .OXT DEF files like VB6 netVoorbereiden
            NetVoorbereiden();

            // Load active bookyears from 9999.OCT and DEFxx.OCT files.
            LoadBookyearsAndPeriods(bjPerDat);

            // Open database (Jet for now) and initialise index metadata.
            OpenJetDatabase(mim);

            // Initialise numeric masks (Cijfermaskers)
            Cijfermaskers();

            // TODO: add schema upgrade blocks, Peppol/QR/vpeSjbs/vat folders, etc.

            // Finally show the BJPERDAT form like VB6.
            //bjPerDat.Show(mim);
        }

        public static void LoadBookyearsAndPeriods(FormBJPERDAT bjPerDat)
        {
            // Simplified, partial port of the 9999.OCT / DEFxx.OCT logic.
            // Assumes LOCATION_COMPANYDATA and PROGRAM_LOCATION are set.

            string oct9999 = Path.Combine(PROGRAM_LOCATION ?? string.Empty, "9999.OCT");
            if (!File.Exists(oct9999))
                return;

            try
            {
                // VB6: Open ... Len = 4, read 3 records (flags, active bookyear, active period)
                using (var fs = new FileStream(oct9999, FileMode.Open, FileAccess.Read))
                using (var br = new BinaryReader(fs))
                {
                    // rec 1: aa (ignored)
                    var aa1 = br.ReadBytes(4);

                    ACTIVE_BOOKYEAR = 0;

                    // rec 2: aa with active bookyear index (1 byte is enough, but we read 4)
                    var aa2 = br.ReadBytes(4);
                    // VB used ACTIVE_BOOKYEAR only later; we keep it as 0 for now.

                    // Fill combobox with available bookyears (DEFxx.OCT)
                    bjPerDat.CmbBoekjaar.Items.Clear();
                    for (int i = 9; i >= 0; i--)
                    {
                        string defPath = Path.Combine(
                            LOCATION_COMPANYDATA ?? string.Empty,
                            "DEF" + i.ToString("00") + ".OCT");

                        if (!File.Exists(defPath))
                            continue;

                        using (var fDef = new FileStream(defPath, FileMode.Open, FileAccess.Read))
                        using (var brDef = new BinaryReader(fDef))
                        {
                            // Each record: fixed 16 bytes; we only need first record to get the bookyear code.
                            byte[] rec = brDef.ReadBytes(16);
                            string a = System.Text.Encoding.Default.GetString(rec);
                            string xx = a.Substring(0, 4); // Left(A,4) in VB6
                            bjPerDat.CmbBoekjaar.Items.Insert(0, xx);
                        }
                    }

                    // rec 3: aa with active period
                    var aa3 = br.ReadBytes(4);
                    int actievePeriode;
                    int.TryParse(System.Text.Encoding.Default.GetString(aa3).Trim(), out actievePeriode);

                    // Select active bookyear index
                    if (bjPerDat.CmbBoekjaar.Items.Count > 0)
                        bjPerDat.CmbBoekjaar.SelectedIndex = ACTIVE_BOOKYEAR;

                    // Use selected text for TABLE_COUNTERS name
                    if (bjPerDat.CmbBoekjaar.SelectedItem is string jaarText)
                    {
                        bstNaam[TABLE_COUNTERS] = "jr" + jaarText;
                    }

                    // Load periods for active bookyear from DEFxx.OCT
                    if (bjPerDat.CmbBoekjaar.SelectedItem is string bjText)
                    {
                        string defActive = Path.Combine(
                            LOCATION_COMPANYDATA ?? string.Empty,
                            "DEF" + ACTIVE_BOOKYEAR.ToString("00") + ".OCT");

                        if (File.Exists(defActive))
                        {
                            LoadPeriodsFromDef(defActive, bjPerDat, actievePeriode);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "AutoLoadCompany.LoadBookyearsAndPeriods");
            }
        }

        private static void LoadPeriodsFromDef(string defPath, FormBJPERDAT bjPerDat, int actievePeriode)
        {
            bjPerDat.CmbPeriodeBoekjaar.Items.Clear();

            using (var fs = new FileStream(defPath, FileMode.Open, FileAccess.Read))
            using (var br = new BinaryReader(fs))
            {
                const int recordLen = 16;
                int t = 1;

                while (fs.Position + recordLen <= fs.Length && t <= 99)
                {
                    byte[] rec = br.ReadBytes(recordLen);
                    string a = System.Text.Encoding.Default.GetString(rec);

                    if (string.IsNullOrWhiteSpace(a) || a.TrimEnd() == new string(' ', 16))
                    {
                        if (bjPerDat.CmbPeriodeBoekjaar.Items.Count > 0)
                            bjPerDat.CmbPeriodeBoekjaar.SelectedIndex = 0;

                        string yy = bjPerDat.CmbPeriodeBoekjaar.Text;
                        if (yy.Length >= 24)
                        {
                            BOOKYEAR_FROMTO =
                                yy.Substring(6, 4) +
                                yy.Substring(3, 2) +
                                yy.Substring(0, 2) +
                                yy.Substring(19, 4) +
                                yy.Substring(16, 2) +
                                yy.Substring(13, 2);
                        }
                        break;
                    }
                    else
                    {
                        // XX = dd/MM/yyyy - dd/MM/yyyy (constructed from A)
                        string xx = a.Substring(6, 2) + "/" + a.Substring(4, 2) + "/" + a.Substring(0, 4) +
                                    " - " +
                                    a.Substring(14, 2) + "/" + a.Substring(12, 2) + "/" + a.Substring(8, 4);
                        bjPerDat.CmbPeriodeBoekjaar.Items.Add(xx);
                    }

                    t++;
                }

                if (actievePeriode - 1 > bjPerDat.CmbPeriodeBoekjaar.Items.Count)
                {
                    MessageBox.Show(
                        "Het hoogste boekjaar wordt automatisch ingeladen.  Laatste bewerking gebeurde in een boekjaar met meer periodes dan nu mogelijk.  De eerste periode van het hoogste boekjaar wordt hierna automatisch geaktiveerd");
                    actievePeriode = 1;
                }

                if (bjPerDat.CmbPeriodeBoekjaar.Items.Count > 0)
                {
                    bjPerDat.CmbPeriodeBoekjaar.SelectedIndex = Math.Max(0, actievePeriode - 1);
                    string at = bjPerDat.CmbPeriodeBoekjaar.Text;
                    if (at.Length >= 24)
                    {
                        PERIOD_FROMTO =
                            at.Substring(6, 4) +
                            at.Substring(3, 2) +
                            at.Substring(0, 2) +
                            at.Substring(at.Length - 4, 4) +
                            at.Substring(16, 2) +
                            at.Substring(13, 2);
                    }
                }
            }
        }

        private static void OpenJetDatabase(FormMim mim)
        {
            // Partial port of the Jet branch (non-SQL-Server) from VB6.
            // Focus: open ntDB, adntDB, run TabelKontrole and InitBestanden.

            string companyPath = LOCATION_COMPANYDATA ?? string.Empty;
            string mdvPath = Path.Combine(companyPath, "Marnt.MDV");

            if (!File.Exists(mdvPath))
            {
                LOCATION_NETDATA = "marNET\\App_Data\\";
                mdvPath = Path.Combine(companyPath, LOCATION_NETDATA, "Marnt.MDV");

                if (!File.Exists(mdvPath))
                {
                    MessageBox.Show("marnt.MDV niet gevonden in de datainhoudsopgave.", "AutoLoadCompany",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    // AutoUnloadBedrijf equivalent could go here.
                    return;
                }
            }
            else
            {
                LOCATION_NETDATA = string.Empty;
            }

            //// Ensure jrYYYY names via TabelKontrole
            //try
            //{
            //    TabelKontrole();
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message, "TabelKontrole", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}

            try
            {
                // ntDB = NTRuimte.OpenDatabase(...)
                // Here we use the ADO connection only (AD_NTDB).
                JET_CONNECT = ADOJET_PROVIDER +
                              "Data Source=" + companyPath + LOCATION_NETDATA + "\\marnt.mdv;" +
                              "Persist Security Info=False";

                AD_NTDB = new Connection();
                AD_NTDB.Open(JET_CONNECT);
                                
                BA_MODUS = 1;

                // Init index metadata
                InitBestanden();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Fout bij openen marnt.MDV: " + ex.Message,
                    "AutoLoadCompany",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private static void NetVoorbereiden()
        {
            // Simplified version of VB6 netVoorbereiden: ensure .OXT exists for each DEFxx.OCT
            for (int count = 9; count >= 0; count--)
            {
                string defOct = Path.Combine(LOCATION_COMPANYDATA ?? string.Empty, "DEF" + count.ToString("00") + ".OCT");
                string defOxt = Path.Combine(LOCATION_COMPANYDATA ?? string.Empty, "DEF" + count.ToString("00") + ".OXT");

                if (!File.Exists(defOxt) && File.Exists(defOct))
                {
                    using (var fs = new FileStream(defOct, FileMode.Open, FileAccess.Read))
                    using (var br = new BinaryReader(fs))
                    using (var sw = new StreamWriter(defOxt, false))
                    {
                        const int len = 16;
                        string dummyLine = string.Empty;
                        while (fs.Position + len <= fs.Length)
                        {
                            byte[] rec = br.ReadBytes(len);
                            string a = System.Text.Encoding.Default.GetString(rec);
                            if (string.IsNullOrWhiteSpace(a) || a.TrimEnd() == new string(' ', 16))
                                break;
                            dummyLine += "," + a;
                        }

                        if (dummyLine.StartsWith(","))
                            dummyLine = dummyLine.Substring(1);

                        sw.WriteLine(dummyLine);
                    }
                }
            }
        }
    }
}