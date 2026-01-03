using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using static mar2026.Classes.ModLibs;
using static mar2026.Classes.ModDatabase;
using static mar2026.Classes.AllFunctions;

namespace mar2026.SharedForms
{
    public partial class FormSQLSearch : Form
    {
        public int flHere;
        public int indexHere;
        public string StartKey;

        // Represents the list of sort definitions (VB6 Sortering.List)
        // Expected format per item:  "+v001;Omschrijving"  or "-v002;Andere omschrijving"
        public ListBox SorteringListBox { get; set; }

        public FormSQLSearch()
        {
            InitializeComponent();
        }

        private void FormSQLSearch_Load(object sender, EventArgs e)
        {
            // VB6:
            // Caption = Caption + ": " + bstNaam(SharedFl)
            Text = Text + ": " + bstNaam[SHARED_FL];

            // VB6: VulcmbSortering
            // -> here assumed to be a method that fills SorteringListBox from JETTABLEUSE_INDEX / FLINDEX_CAPTION
            FillSortering();

            // VB6:
            // If InStr(GridText, "@Beperk@") Then
            //     txtTeZoeken.Text = Left(GridText, 2) + "%"
            //     cmdZoeken_Click
            // ElseIf GridText <> "" Then
            //     txtTeZoeken.Text = GridText + "%"
            //     cmdZoeken_Click
            // Else
            //     txtTeZoeken.Text = "%"
            // End If

            var gridText = StartKey ?? string.Empty;

            if (gridText.IndexOf("@Beperk@", StringComparison.OrdinalIgnoreCase) >= 0 &&
                gridText.Length >= 2)
            {
                TextBoxToSearch.Text = gridText.Substring(0, 2) + "%";
                ButtonSearch_Click(sender, e);
            }
            else if (!string.IsNullOrEmpty(gridText))
            {
                TextBoxToSearch.Text = gridText + "%";
                ButtonSearch_Click(sender, e);
            }
            else
            {
                TextBoxToSearch.Text = "%";
            }
        }

        private void ButtonClose_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void ButtonSearch_Click(object sender, EventArgs e)
        {        
            if (SorteringListBox == null || SorteringListBox.Items.Count == 0)
            {
                return;
            }

            var selectedItem = Convert.ToString(SorteringListBox.SelectedItem ?? string.Empty);
            if (string.IsNullOrEmpty(selectedItem))
            {
                return;
            }

            SqlRefreshText(selectedItem);
        }

        private void ListViewSqlResult_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (sqkResultListView.FocusedItem == null ||
                sqkResultListView.FocusedItem.SubItems == null ||
                sqkResultListView.FocusedItem.SubItems.Count == 0)
            {
                return;
            }

            XLOG_KEY = sqkResultListView.FocusedItem.SubItems[0].Text;
            TextBoxToSearch.Text = XLOG_KEY;
        }
        

        /// <summary>
        /// C# equivalent of VB6 VulcmbSortering.
        /// Populates SorteringListBox with sort definitions for SHARED_FL.
        /// Expected item format: "+v001;Omschrijving" / "-v002;Andere omschrijving".
        /// </summary>
        private void FillSortering()
        {
            if (SorteringListBox == null)
            {
                return;
            }

            SorteringListBox.Items.Clear();

            // Build list from FLINDEX_CAPTION / JETTABLEUSE_INDEX
            // Default: ascending (+). Adapt if you have stored sort directions.
            for (int i = 0; i <= FL_NUMBEROFINDEXEN[SHARED_FL]; i++)
            {
                var veld = JETTABLEUSE_INDEX[SHARED_FL, i];
                var caption = FLINDEX_CAPTION[SHARED_FL, i];

                if (string.IsNullOrWhiteSpace(veld) || string.IsNullOrWhiteSpace(caption))
                {
                    continue;
                }

                // VB list format: "+v001;Omschrijving"
                string item = "+" + veld.Trim() + ";" + caption.Trim();
                SorteringListBox.Items.Add(item);
            }

            if (SorteringListBox.Items.Count > 0)
            {
                SorteringListBox.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// C# equivalent of VB6 cmdZoeken_Click.
        /// Invokes SqlVernieuwTekst based on current sort selection.
        /// </summary>
        

        /// <summary>
        /// C# translation of VB6 SQLVernieuwTekst.
        /// Builds the SQL string into rtbSQLTekst based on ComboTekst and txtTeZoeken.
        /// Relies on global state (SharedFl, JETTABLEUSE_INDEX, bstNaam, etc.).
        /// </summary>
        /// <param name="comboTekst">VB6 ComboTekst parameter (sort specification string).</param>
        public void SqlRefreshText(string comboTekst)
        {
            string sorteerIndex = string.Empty;
            string sorteerOrde = string.Empty;
            string sleuteltje;
            int telOrde = 0;

            try
            {
                // grdColWidth(0) = 0
                // GRD_COLWIDTH[0] = 0;
                                
                sleuteltje = 
                    "marEDB" 
                    + SHARED_FL.ToString ("00")
                    + comboTekst.Substring(0, comboTekst.IndexOf(";", StringComparison.Ordinal));
                

                while (true)
                {
                    int countTo = comboTekst.IndexOf(";", telOrde, StringComparison.Ordinal);
                    if (countTo < 0)
                    {
                        break;
                    }

                    // COUNT_TO in VB is position of ';' (1-based-1), so use countTo - 4..countTo-1 for the 4-char field name
                    if (countTo >= 4)
                    {
                        string veld = comboTekst.Substring(countTo - 4, 4);
                        char prefix = comboTekst[countTo - 5]; // '+' or '-'

                        if (telOrde == 0)
                        {
                            sorteerIndex = veld;
                            sorteerOrde = veld + (prefix == '+' ? " ASC" : " DESC");
                        }
                        else
                        {
                            sorteerIndex += "+" + veld;
                            sorteerOrde += ", " + veld + (prefix == '+' ? " ASC" : " DESC");
                        }
                    }

                    telOrde = countTo + 1;
                }

                // bGet TABLE_VARIOUS, 1, "29" + Sleuteltje
                string key = "29" + sleuteltje;
                BGet(TABLE_VARIOUS, 1, key);

                if (KTRL != 0)
                {
                    InitSql(comboTekst, sorteerIndex, sorteerOrde);
                    return;
                }

                // RecordToVeld TABLE_VARIOUS
                RecordToField(TABLE_VARIOUS);

                string msg = VBibTekst(TABLE_VARIOUS, "#v132 #");
                string upperMsg = msg.ToUpperInvariant();

                if (upperMsg.Contains("WHERE"))
                {
                    int wherePos = upperMsg.IndexOf(" WHERE ", StringComparison.Ordinal);
                    if (wherePos > 0)
                    {
                        msg = msg.Substring(0, wherePos);
                    }

                    msg += " WHERE " + sorteerIndex +
                           " Like " + "\"" + TextBoxToSearch.Text + "\"" +
                           " ORDER BY " + sorteerOrde;

                    rtbSQLTekst.Text = msg;

                    // Column widths part after "[Colwidth]"
                    string full = VBibTekst(TABLE_VARIOUS, "#v132 #");
                    int colPos = full.IndexOf("[Colwidth]", StringComparison.Ordinal);
                    if (colPos >= 0)
                    {
                        string widthsPart = full.Substring(colPos + "[Colwidth]".Length);
                        if (string.IsNullOrEmpty(widthsPart))
                        {
                            // GRD_COLWIDTH[0] = 0;
                        }
                        else
                        {
                            int countTo = 0;
                            while (!string.IsNullOrEmpty(widthsPart))
                            {
                                int tabPos = widthsPart.IndexOf('\t');
                                if (tabPos < 0)
                                {
                                    break;
                                }

                                string part = widthsPart.Substring(0, tabPos);
                                int val;
                                if (int.TryParse(part, out val))
                                {
                                    // GRD_COLWIDTH[countTo] = val;
                                }

                                widthsPart = widthsPart.Substring(tabPos + 1);
                                countTo++;
                            }

                            // GRD_COLWIDTH[countTo] = 0;
                        }
                    }
                    else
                    {
                        // GRD_COLWIDTH[0] = 0;
                    }
                }
                else
                {
                    InitSql(comboTekst, sorteerIndex, sorteerOrde);
                }
            }
            catch
            {
                MessageBox.Show(
                    @"Een fout tijdens opbouw van de SQL SELECT instructie.",
                    @"SQL fout",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// C# version of VB6 InitSQL GoSub block inside SQLVernieuwTekst.
        /// Builds a default SELECT statement into RtbSqlTekst based on Sortering list.
        /// </summary>
        private void InitSql(string comboTekst, string sorteerIndex, string sorteerOrde)
        {
            string msg = "SELECT";
            bool deLaatste = false;

            // eerst eerste index verzekeren !
            if (SorteringListBox != null && SorteringListBox.Items.Count > 0)
            {
                for (int i = 0; i < SorteringListBox.Items.Count; i++)
                {
                    string item = Convert.ToString(SorteringListBox.Items[i] ?? string.Empty);
                    int semiPos = item.IndexOf(";", StringComparison.Ordinal);
                    if (semiPos <= 0)
                    {
                        continue;
                    }

                    string veldNaam = item.Substring(1, semiPos - 1).Trim();
                    string hoofdIndex = JETTABLEUSE_INDEX[SHARED_FL, 0]?.Trim();

                    if (string.Equals(veldNaam, hoofdIndex, StringComparison.Ordinal))
                    {
                        string alias = item.Substring(semiPos + 1);
                        msg += " " + veldNaam + " AS [" + alias + "],";
                        if (i == SorteringListBox.Items.Count - 1)
                        {
                            deLaatste = true;
                        }

                        break;
                    }
                }
            }

            if (msg == "SELECT")
            {
                MessageBox.Show(
                    "Hoofdindex bestaat niet (meer)",
                    @"SQL fout",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }

            // dan de rest bijvoegen
            if (SorteringListBox != null && SorteringListBox.Items.Count > 0)
            {
                for (int i = 0; i < SorteringListBox.Items.Count; i++)
                {
                    string item = Convert.ToString(SorteringListBox.Items[i] ?? string.Empty);
                    int semiPos = item.IndexOf(";", StringComparison.Ordinal);
                    if (semiPos <= 0)
                    {
                        continue;
                    }

                    string veldNaam = item.Substring(1, semiPos - 1).Trim();
                    string hoofdIndex = JETTABLEUSE_INDEX[SHARED_FL, 0]?.Trim();

                    if (string.Equals(veldNaam, hoofdIndex, StringComparison.Ordinal))
                    {
                        continue;
                    }

                    string alias = item.Substring(semiPos + 1);
                    msg += " " + veldNaam + " AS [" + alias + "]";

                    if (!deLaatste ||
                        SorteringListBox.Items.Count != 1 &&
                        !(deLaatste && i == SorteringListBox.Items.Count - 2) &&
                        i < SorteringListBox.Items.Count - 1)
                    {
                        msg += ",";
                    }
                }
            }

            msg += " FROM " + bstNaam[SHARED_FL];
            msg += " WHERE " + sorteerIndex +
                   " Like " + "\"" + TextBoxToSearch.Text + "\"" +
                   " ORDER BY " + sorteerOrde;

            rtbSQLTekst.Text = msg;
        }                
    }
}
