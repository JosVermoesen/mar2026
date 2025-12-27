using System;
using System.IO;
using System.Windows.Forms;

using mar2026;
using mar2026.Classes;
using static mar2026.Classes.ModLibs;
using static mar2026.Classes.AllFunctions;

namespace Mar2026
{
    public partial class FormCloudSetting : Form
    {
        public FormCloudSetting()
        {
            InitializeComponent();
        }

        private void FormCloudSetting_Load(object sender, EventArgs e)
        {            
            // Enable default buttons only when company data location is known
            if (!string.IsNullOrEmpty(LOCATION_COMPANYDATA))
            {
                ButtonDefaultResetForOneDrive.Enabled = true;
                ButtonDefaultResetForMapMarnt.Enabled = true;
            }
            else
            {
                ButtonDefaultResetForOneDrive.Enabled = false;
                ButtonDefaultResetForMapMarnt.Enabled = false;
            }


            string bookInfoMode = LoadText("Algemeen", "BoekInfoModus");
            if (bookInfoMode == "")
            {
                radioButtonShowAlwaysBookingsInfo.Checked = true;
            }
            else
            {
                // Map stored mode ("0","1","2") to the proper radio button
                switch (bookInfoMode.Substring(0, 1))
                {
                    case "0":
                        radioButtonShowNoBookingsInfo.Checked = true;
                        break;
                    case "1":
                        radioButtonShowSomeBookingsInfo.Checked = true;
                        break;
                    case "2":
                        radioButtonShowAlwaysBookingsInfo.Checked = true;
                        break;
                    default:
                        radioButtonShowAlwaysBookingsInfo.Checked = true;
                        break;
                }
            }

            // Load MimDataLocation from marIntegraal settings
            // Value must contains "\marnt\data"
            string valuePath = LoadText("marIntegraal", "Bedrijfsinhoudsopgave2025");
            bool containsPath = valuePath.ToLower().Contains(@"\marnt\data".ToLower());
            if (!containsPath)
            {
                MessageBox.Show("De locatie van de bedrijfsinhoudsopgave is niet correct ingesteld.\n\nDuidt in marIntegraal een correcte locatie aan a.u.b.", "Fout in locatie", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Close();
            }
            if (!Directory.Exists(valuePath))
            {
                MessageBox.Show($"marnt\\data niet gevonden:\n\n {valuePath}");
            }
            SharedGlobals.SetMimDataLocation(valuePath);

            // Load MarNT CloudLocation from settings and save to SharedGlobals
            valuePath = LoadText("dnnInstellingen", "Cloud");            
            if (!Directory.Exists(valuePath))
            {
                MessageBox.Show($"inhoudsopgave voor cloud niet gevonden:\n\n {valuePath}");
            }
            SharedGlobals.MarntCloudLocation = valuePath;

            // Load MarNT Archive CloudLocation from settings and save to SharedGlobals
            valuePath = LoadText("dnnInstellingen", "Archief");            
            if (!Directory.Exists(valuePath))
            {
                MessageBox.Show($"inhoudsopgave archief niet gevonden:\n\n {valuePath}");
            }
            SharedGlobals.MarntCLoudArchiveLocation = valuePath;

            // Load MarNT Mario CloudLocation from settings and save to SharedGlobals
            valuePath = LoadText("dnnInstellingen", "Mario");            
            if (!Directory.Exists(valuePath))
            {
                MessageBox.Show($"inhoudsopgave voor manueel niet gevonden:\n\n {valuePath}");
            }
            SharedGlobals.MarntCloudMarioLocation = valuePath;

            // Load existing settings or propose defaults
            if (string.IsNullOrEmpty(LoadText("dnnInstellingen", "Cloud")))
            {
                string bedrijfsLoc = LoadText(Application.ProductName, "Bedrijfsinhoudsopgave");
                MessageBox.Show(
                    "Nieuwe PC of nog geen instellingen voor Cloud.  Wijzig de volgende standaardwaarden a.u.b. voor uw bedrijf (zie aanbevelingen in onze voorbeeld nota!) of vraag onze gratis bijstand om dit in uw plaats in orde te brengen.",
                    "Cloud instellingen",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                                
                TextBoxUrlLocal.Text = "https://mijndomein.be";                
                TextBoxCloudMarnt.Text = System.IO.Path.Combine(bedrijfsLoc, "cloud");                
                TextBoxCloudMario.Text = System.IO.Path.Combine(bedrijfsLoc, "cloud", "mario");
                TextBoxCloudArchive.Text = System.IO.Path.Combine(bedrijfsLoc, "cloud", "archief");
            }
            else
            {
                TextBoxUrlLocal.Text = LoadText("dnnInstellingen", "URLwww");
                TextBoxCloudMarnt.Text = LoadText("dnnInstellingen", "Cloud");
                TextBoxCloudMario.Text = LoadText("dnnInstellingen", "Mario");
                TextBoxCloudArchive.Text = LoadText("dnnInstellingen", "Archief");                
            }
        }

        private void ButtonCloudArchive_Click(object sender, EventArgs e)
        {
            if (!ModLibs.ShellExecuteWithFallback(TextBoxUrlLocal.Text))
            {
                MessageBox.Show(
                    "Kon " + TextBoxCloudArchive.Text + " niet openen. Raadpleeg ShellHelper.log voor details.",
                    "Cloud archief",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);
            }
        }

        private void ButtonCloudMario_Click(object sender, EventArgs e)
        {
            if (!ModLibs.ShellExecuteWithFallback(TextBoxCloudMario.Text))
            {
                MessageBox.Show(
                    "Kon " + TextBoxCloudMario.Text + " niet openen. Raadpleeg ShellHelper.log voor details.",
                    "Cloud Mario",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);
            }
        }
        private void ButtonCloudMarnt_Click(object sender, EventArgs e)
        {
            if (!ModLibs.ShellExecuteWithFallback(TextBoxCloudMarnt.Text))
            {
                MessageBox.Show(
                    "Kon " + TextBoxCloudMarnt.Text + " niet openen. Raadpleeg ShellHelper.log voor details.",
                    "Cloud MarNT",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);
            }
        }

        private void ButtonDefaultResetForMapMarnt_Click(object sender, EventArgs e)
        {
            string marNTLocatie = (LoadText(Application.ProductName, "Bedrijfsinhoudsopgave" + "2025") ?? string.Empty).ToLowerInvariant();
            marNTLocatie = marNTLocatie.Replace("\\data", string.Empty);

            string serverMap = (LoadText(Application.ProductName, "ServerBedrijfsinhoudsopgave") ?? string.Empty).Trim().ToLowerInvariant();
            if (!string.IsNullOrEmpty(serverMap))
            {
                string msg = "Voor deze PC bestaat al een serverinhoudsopgave:\n" +
                             serverMap + "\n\n" +
                             "Verwijder indien nodig.\n\n" +
                             "Met serverinstellingen gedraagt deze PC zich als client.\n" +
                             "Voor marIntegraal draaiende op locatie (client) dient U\n" + 
                             "de instellingen manueel in te voeren a.d.h.v. uw server-link.";
                
                MessageBox.Show(msg, "Cloud instellingen", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string confirm = "Akkoord voor:\n" + 
                             "CLOUD   MARNT: " + marNTLocatie + " (dus dezelfde hoofdmap)\n" + 
                             "CLOUD   MARIO: " + System.IO.Path.Combine(marNTLocatie, "manueel\n") + 
                             "CLOUD ARCHIEF: " + System.IO.Path.Combine(marNTLocatie, "archief\n");

            if (MessageBox.Show(confirm, "Cloud instellingen", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                TextBoxCloudMarnt.Text = marNTLocatie;
                TextBoxCloudMario.Text = System.IO.Path.Combine(marNTLocatie, "manueel");
                TextBoxCloudArchive.Text = System.IO.Path.Combine(marNTLocatie, "archief");

                try
                {
                    FS.CreateFolder(TextBoxCloudMario.Text);
                }
                catch
                {
                    MessageBox.Show("Map bestaat reeds:\n\n"                        
                        + TextBoxCloudMario.Text,
                        "Cloud instellingen", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                try
                {
                    FS.CreateFolder(TextBoxCloudArchive.Text);
                }
                catch
                {
                    MessageBox.Show("Map bestaat reeds\n\n" + TextBoxCloudArchive.Text,
                        "Cloud instellingen", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                // SaveAndClose();
            }
        }

        private void ButtonDefaultResetForOneDrive_Click(object sender, EventArgs e)
        {
            string marNTLocatie = (LoadText(Application.ProductName, "Bedrijfsinhoudsopgave" + "2025") ?? string.Empty).ToLowerInvariant();
            marNTLocatie = marNTLocatie.Replace("\\data", string.Empty);

            string serverMap = (LoadText(Application.ProductName, "ServerBedrijfsinhoudsopgave") ?? string.Empty).Trim().ToLowerInvariant();
            if (!string.IsNullOrEmpty(serverMap))
            {
                string msg = "Voor deze PC bestaat al een serverinhoudsopgave:\n" + 
                             serverMap + "\n\n" +
                             "Verwijder indien nodig.\n\n" +
                             "Met serverinstellingen gedraagt deze PC zich als client.\n" + 
                             "Voor marIntegraal draaiende op locatie (client) dient U\n" + 
                             "de instellingen manueel in te voeren a.d.h.v. uw server-link.";
                MessageBox.Show(msg, "Cloud instellingen", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string systemPersonalDocs = (SYSTEM_MYPERSONALDOCUMENTS ?? string.Empty).ToLowerInvariant();
            if (systemPersonalDocs.Contains("onedrive"))
            {
                string msg = "Dit is een toestel met 'OneDrive' Map ideaal voor automatische\n" +
                             "archivering naar de CLOUD.\n\n" + 
                             "Akkoord voor:" + Environment.NewLine +
                             "CLOUD   MARNT: " + System.IO.Path.Combine(systemPersonalDocs, "marNT") + "\n" +
                             "CLOUD   MARIO: " + System.IO.Path.Combine(systemPersonalDocs, "marNT", "manueel") + "\n" +
                             "CLOUD ARCHIEF: " + System.IO.Path.Combine(systemPersonalDocs, "marNT", "archief");

                if (MessageBox.Show(msg, "Cloud instellingen", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    TextBoxCloudMarnt.Text = System.IO.Path.Combine(systemPersonalDocs, "marNT");
                    TextBoxCloudMario.Text = System.IO.Path.Combine(systemPersonalDocs, "marNT", "manueel");
                    TextBoxCloudArchive.Text = System.IO.Path.Combine(systemPersonalDocs, "marNT", "archief");

                    TryCreateFolder(TextBoxCloudMarnt.Text);
                    TryCreateFolder(TextBoxCloudMario.Text);
                    TryCreateFolder(TextBoxCloudArchive.Text);

                    // SaveAndClose();
                }
            }
        }

        private static void TryCreateFolder(string path)
        {
            try
            {
                FS.CreateFolder(path);
            }
            catch
            {
                MessageBox.Show("Map bestaat reeds\n\n" + path,
                    "Cloud instellingen", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
                
        private void ButtonClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void SaveAndClose()
        {         
            // CmdBewaar_Click logic
            SaveText("dnnInstellingen", "Archief", TextBoxCloudArchive.Text); // archief cloud
            SaveText("dnnInstellingen", "URLwww", TextBoxUrlLocal.Text);
            SaveText("dnnInstellingen", "Mario", TextBoxCloudMario.Text); // mario cloud
            SaveText("dnnInstellingen", "Cloud", TextBoxCloudMarnt.Text); // marnt cloud
            Close();
        }

        private void ButtonSaveAndClose_Click(object sender, EventArgs e)
        {
            SaveAndClose();
        }
                        
        private void radioButtonShowNoBookingsInfo_Click(object sender, EventArgs e)
        {
            if (radioButtonShowNoBookingsInfo.Checked)
            {
                SaveText("Algemeen", "BoekInfoModus", "0: " + radioButtonShowNoBookingsInfo.Text);
                // Update the status/toolbar label on the running FormMim
                if (Application.OpenForms["FormMim"] is FormMim mim)
                {
                    mim.toolStripJournalEntryNow.Text = radioButtonShowNoBookingsInfo.Text;
                }
            }

        }

        private void radioButtonShowSomeBookingsInfo_Click(object sender, EventArgs e)
        {
            if (radioButtonShowSomeBookingsInfo.Checked)
            {
                SaveText("Algemeen", "BoekInfoModus", "1: " + radioButtonShowSomeBookingsInfo.Text);
                // Update the status/toolbar label on the running FormMim
                if (Application.OpenForms["FormMim"] is FormMim mim)
                {
                    mim.toolStripJournalEntryNow.Text = radioButtonShowSomeBookingsInfo.Text;
                }
            }
        }

        private void radioButtonShowAlwaysBookingsInfo_Click(object sender, EventArgs e)
        {
            if (radioButtonShowAlwaysBookingsInfo.Checked)
            {
                SaveText("Algemeen", "BoekInfoModus", "2: " + radioButtonShowAlwaysBookingsInfo.Text);
                // Update the status/toolbar label on the running FormMim
                if (Application.OpenForms["FormMim"] is FormMim mim)
                {
                    mim.toolStripJournalEntryNow.Text = radioButtonShowAlwaysBookingsInfo.Text;
                }
            }
        }        
    }
}
