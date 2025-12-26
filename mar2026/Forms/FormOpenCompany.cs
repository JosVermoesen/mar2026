using ADODB;
using System;
using System.IO;
using System.Windows.Forms;

using mar2026.Classes;
using static mar2026.Classes.AllFunctions;
using static mar2026.Classes.ModLibs;

namespace mar2026.Forms
{
    public partial class FormOpenCompany : Form
    {
        private string _strDataLocatie;

        public FormOpenCompany()
        {
            InitializeComponent();
        }

        private void FormOpenCompany_Load(object sender, EventArgs e)
        {
            Top = 0;
            Left = 0;

            // ListView setup
            ListViewCompanies.View = View.Details;
            ListViewCompanies.FullRowSelect = true;
            ListViewCompanies.HideSelection = false;
            ListViewCompanies.Columns.Clear();
            ListViewCompanies.Columns.Add("Benaming", 420);
            ListViewCompanies.Columns.Add("Map", 80);

            // Remember last choice: lokaal/server
            _strDataLocatie = LoadText("BedrijfOpenen", "DataDefault");
            if (string.IsNullOrWhiteSpace(_strDataLocatie))
                _strDataLocatie = "lokaal";

            if (string.Equals(_strDataLocatie, "server", StringComparison.OrdinalIgnoreCase))
            {
                RadioButtonServer.Checked = true;
                // Disable basis(4) equivalent if you have one on main form
                // if (Application.OpenForms["FormMim"] is FormMim mim)
                    //TODO? mim.SetBasis4Enabled(false);
            }
            else
            {
                RadioButtonLocal.Checked = true;
                // if (Application.OpenForms["FormMim"] is FormMim mim)
                    // mim.SetBasis4Enabled(true);
            }

            VSF_PRO = false;

            try
            {
                // Initial path from location text
                if (string.IsNullOrWhiteSpace(TextBoxLocation.Text))
                {
                    TextBoxLocation.Text = LOCATION_;
                }
                LOCATION_ = TextBoxLocation.Text.TrimEnd('\\') + "\\";
                FillCompanyList();
            }
            catch
            {
                MessageBox.Show("LOCATION_ bedrijven onvindbaar.  Kontroleer manueel a.u.b.", "Fout", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FillCompanyList()
        {
            ListViewCompanies.Items.Clear();

            string myPath = TextBoxLocation.Text.TrimEnd('\\') + "\\";
            if (!Directory.Exists(myPath))
                return;

            try
            {
                foreach (var dir in Directory.GetDirectories(myPath))
                {
                    string folderName = Path.GetFileName(dir);
                    string marntTxt = Path.Combine(dir, "marnt.txt");
                    if (!File.Exists(marntTxt))
                        continue;

                    string naamDetail;
                    using (var sr = new StreamReader(marntTxt))
                    {
                        naamDetail = sr.ReadLine() ?? string.Empty;
                    }

                    var item = new ListViewItem(naamDetail);
                    item.SubItems.Add(folderName);
                    ListViewCompanies.Items.Add(item);
                }
            }
            catch
            {
                // ignore directory errors, like VB6 On Error Resume Next
            }
        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ButtonOpenFolder_Click(object sender, EventArgs e)
        {
            try
            {
                string path = TextBoxLocation.Text;
                if (Directory.Exists(path))
                {
                    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                    {
                        FileName = path,
                        UseShellExecute = true
                    });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Open folder", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ButtonToggleEditLocation_Click(object sender, EventArgs e)
        {
            TextBoxLocation.Enabled = !TextBoxLocation.Enabled;
            if (TextBoxLocation.Enabled)
            {
                TextBoxLocation.Focus();
                TextBoxLocation.SelectAll();
                return;
            }

            // Save new default location depending on radio selection
            if (RadioButtonLocal.Checked)
            {
                string old = LoadText(Application.ProductName, "Bedrijfsinhoudsopgave2025");
                if (!string.Equals(TextBoxLocation.Text, old, StringComparison.OrdinalIgnoreCase))
                {
                    var res = MessageBox.Show(
                        TextBoxLocation.Text + "\n\n" +
                        "Wordt dit de nieuwe 'lokale' opstartinhoudsopgave?",
                        "Bevestig",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question,
                        MessageBoxDefaultButton.Button2);

                    if (res == DialogResult.Yes)
                    {
                        SaveText(Application.ProductName, "Bedrijfsinhoudsopgave2025", TextBoxLocation.Text);
                        MessageBox.Show("Hierna wordt er afgesloten.  Start het programma opnieuw op.");
                        Application.OpenForms["FormMim"]?.Close();
                    }
                    else
                    {
                        TextBoxLocation.Text = old;
                    }
                }
            }
            else
            {
                string old = LoadText(Application.ProductName, "ServerBedrijfsinhoudsopgave");
                if (!string.Equals(TextBoxLocation.Text, old, StringComparison.OrdinalIgnoreCase))
                {
                    var res = MessageBox.Show(
                        TextBoxLocation.Text + "\n\n" +
                        "Wordt dit de nieuwe 'server' opstartinhoudsopgave ?",
                        "Bevestig",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question,
                        MessageBoxDefaultButton.Button2);

                    if (res == DialogResult.Yes)
                    {
                        SaveText(Application.ProductName, "ServerBedrijfsinhoudsopgave", TextBoxLocation.Text);
                        MessageBox.Show("Hierna wordt er afgesloten.  Start het programma opnieuw op.");
                        Application.OpenForms["FormMim"]?.Close();
                    }
                    else
                    {
                        TextBoxLocation.Text = old;
                    }
                }
            }
        }

        private void RadioButtonLocation_CheckedChanged(object sender, EventArgs e)
        {
            if (!RadioButtonLocal.Checked && !RadioButtonServer.Checked)
                return;

            try
            {
                if (RadioButtonServer.Checked)
                {
                    TextBoxLocation.Text = LoadText(Application.ProductName, "ServerBedrijfsinhoudsopgave");
                    SaveText("BedrijfOpenen", "DataDefault", "server");
                    ButtonCompact.Enabled = false;
                    // if (Application.OpenForms["FormMim"] is FormMim mim)
                        // mim.SetBasis4Enabled(false);
                }
                else
                {
                    TextBoxLocation.Text = LoadText(Application.ProductName, "Bedrijfsinhoudsopgave2025");
                    SaveText("BedrijfOpenen", "DataDefault", "lokaal");
                    ButtonCompact.Enabled = true;
                    // if (Application.OpenForms["FormMim"] is FormMim mim)
                        // mim.SetBasis4Enabled(true);
                }
                LOCATION_ = TextBoxLocation.Text.TrimEnd('\\') + "\\";
                FillCompanyList();
            }
            catch
            {

            }
        }

        private void ListViewCompanies_DoubleClick(object sender, EventArgs e)
        {
            DoOpenCompany();
        }

        private void ListViewCompanies_GotFocus(object sender, EventArgs e)
        {

        }

        private void ButtonnOk_Click(object sender, EventArgs e)
        {
            DoOpenCompany();
        }

        private void DoOpenCompany()
        {
            if (ListViewCompanies.Items.Count == 0 || ListViewCompanies.SelectedItems.Count == 0)
                return;

            var item = ListViewCompanies.SelectedItems[0];
            string folder = item.SubItems[1].Text;

            LOCATION_COMPANYDATA = LOCATION_ + folder + "\\";
            if (Application.OpenForms["FormMim"] is FormMim mim)
            {
                mim.Text = Application.ProductName + " - [" + item.Text.Trim() + "]";
            }

            for (int i = 1; i <= 3; i++)
            { 
                // Example: activate BasisB[1] (Fiche Klanten)
                var fiche = ModLibs.BasisB[i];
                fiche.Enabled = true;
                fiche.WindowState = FormWindowState.Normal;
                fiche.BringToFront();
                fiche.Activate();
            }

            // AutoLoadBedrijf equivalent: use AutoLoadCompany.Run
            // if (Application.OpenForms["FormBJPERDAT"] is FormBJPERDAT bj)
            // {
            //     AutoLoadCompany.Run((FormMim)Application.OpenForms["FormMim"], bj);
            // }

            // Close();
        }

        private void ButtonCompact_Click(object sender, EventArgs e)
        {
            if (ListViewCompanies.Items.Count == 0 || ListViewCompanies.SelectedItems.Count == 0)
                return;

            var item = ListViewCompanies.SelectedItems[0];
            string folder = item.SubItems[1].Text;
            string companyPath = LOCATION_ + folder + "\\";
            string mdvPath = Path.Combine(companyPath, "marnt.mdv");

            if (!File.Exists(mdvPath))
                return;

            try
            {
                // Just test open like VB did
                string connString = ADOJET_PROVIDER +
                                    "Data Source=" + mdvPath + ";" +
                                    "Persist Security Info=False";

                Connection cnn = null;
                try
                {
                    cnn = new Connection();
                    cnn.Open(connString);
                    MSG = "Huidige database in JetVersie 4.x vernieuwen\n" + 
                          "Microsoft ADO Versie " + cnn.Version + "\n\n" +
                          "LOCATION_ : " + mdvPath;
                }
                finally
                {
                    cnn?.Close();
                }

                // TODO: JRO compact logic can be ported if you need it; for now we show info
                MessageBox.Show(MSG, "Database vernieuwen", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "CompactDatabase", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
