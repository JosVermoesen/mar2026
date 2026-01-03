using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static mar2026.Classes.AllFunctions;
using static mar2026.Classes.ModDatabase;
using static mar2026.Classes.ModLibs;
using static System.Net.Mime.MediaTypeNames;

namespace mar2026.SharedForms
{
    public partial class FormSQLSearch : Form
    {
        string sqlDummy;

        public FormSQLSearch()
        {
            InitializeComponent();
            ComboBoxSortOn.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void FormSQLSearch_Load(object sender, EventArgs e)
        {
            Text = Text + ": " + bstNaam[SHARED_FL];
            sqkResultListView.Clear();

            if (SHARED_FL == 1)
            sqlDummy = "SELECT " +
                "A110 AS [Nummer], " +
                "A100 AS [Naam1], " +
                "A101 AS [Voornaam], " +
                "A104 & ' ' & A105 & ' ' & A106 AS [Straat], " +
                "A108 AS [Plaats] " +
                "FROM Klanten " +
                "WHERE A100 Like 'van%' " +
                "ORDER BY A100 ASC";

            rtbSQLTekst.Text = sqlDummy;

            FillSortering();

            if (GRIDTEXT.IndexOf("@Beperk@", StringComparison.OrdinalIgnoreCase) >= 0 &&
                GRIDTEXT.Length >= 2)
            {
                TextBoxToSearch.Text = GRIDTEXT.Substring(0, 2) + "%";
                ButtonSearch_Click(sender, e);
            }
            else if (!string.IsNullOrEmpty(GRIDTEXT))
            {
                TextBoxToSearch.Text = GRIDTEXT + "%";
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
            var selectedItem = Convert.ToString(ComboBoxSortOn.SelectedItem ?? string.Empty);
            if (string.IsNullOrEmpty(selectedItem))
            {
                return;
            }

            SqlRefreshText(selectedItem);
        }
        
        private void ComboBoxSortOn_SelectedIndexChanged(object sender, EventArgs e)
        {            
            
        }

        private void TextBoxToSearch_TextChanged(object sender, EventArgs e)
        {
            // If length <= 1 and no '%' yet, append '%' and place caret before it
            if (TextBoxToSearch.Text.Length <= 1 &&
                TextBoxToSearch.Text.IndexOf('%') < 0)
            {
                // Prevent re-entrancy issues by caching the original text
                var original = TextBoxToSearch.Text;
                TextBoxToSearch.Text = original + "%";

                // Put caret just before the '%'
                TextBoxToSearch.SelectionStart = TextBoxToSearch.Text.Length - 1;
                TextBoxToSearch.SelectionLength = 0;
            }
        }

        private void TextBoxToSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            ButtonSearchLike.Text = "Zoeken";
        }

        private void TextBoxToSearch_Enter(object sender, EventArgs e)
        {
            ButtonSearchLike.Text = "Zoeken";
            TextBoxToSearch.SelectionStart = 0;
            TextBoxToSearch.SelectionLength = TextBoxToSearch.Text.Length;
        }

        private void TextBoxToSearch_KeyDown(object sender, KeyEventArgs e)
        {
            // VB6: On Error Resume Next – ignore focus errors
            try
            {
                // Arrow keys Up/Down (and also Right in original 38..40)
                if (e.KeyCode >= Keys.Up && e.KeyCode <= Keys.Down)
                {
                    if (sqkResultListView.Items.Count == 0)
                    {
                        return;
                    }

                    // Ensure some item is focused/selected before moving
                    int targetColumn = A_INDEX; // desired column (subitem) to focus 

                    sqkResultListView.Focus();

                    // Move to first row and desired column (subitem)
                    var firstItem = sqkResultListView.Items[0];
                    firstItem.Selected = true;
                    firstItem.Focused = true;

                    if (targetColumn >= 0 && targetColumn < firstItem.SubItems.Count)
                    {
                        // There is no direct "Col" property like VB6 grid,
                        // but having the correct item focused is generally enough.
                        // If needed, you could visually emphasize the subitem here.
                    }

                    e.Handled = true;
                }
            }
            catch
            {
                // Swallow any focus/navigation issues (On Error Resume Next behavior)
            }
        }

        private void RitchTextBoxSQLSelect_TextChanged(object sender, EventArgs e)
        {
            cmdBewaar.Enabled = true;
        }

        
        private void ListViewSqlResult_DoubleClick(object sender, EventArgs e)
        {
            // VB6: mfgLijst_DblClick -> cmdZoeken_Click
            ButtonSearch_Click(sender, e);
        }

        private void ListViewSqlResult_Enter(object sender, EventArgs e)
        {
            // VB6: mfgLijst_GotFocus
            //   cmdZoeken.Caption = "Ok"
            //   mfgLijst_Click

            ButtonSearchLike.Text = "Ok";
            ListViewSqlResult_Click(sender, e);
        }

        private void ListViewSqlResult_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            // VB6: mfgLijst_RowColChange
            // If mfgLijst.Rows <> 2 Then mfgLijst_Click
            //
            // Approximation: when there is more than 1 data row selected/visible,
            // trigger the same behaviour as clicking the grid.

            if (sqkResultListView.Items.Count != 2)
            {
                ListViewSqlResult_Click(sender, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Common handler approximating VB6 mfgLijst_Click:
        /// takes current row, updates XLOG_KEY/TextBoxToSearch and (optionally) performs search/OK logic.
        /// </summary>
        private void ListViewSqlResult_Click(object sender, EventArgs e)
        {
            if (sqkResultListView.FocusedItem == null ||
                sqkResultListView.FocusedItem.SubItems == null ||
                sqkResultListView.FocusedItem.SubItems.Count == 0)
            {
                return;
            }

            // Use first column as key (similar to existing SelectedIndexChanged)
            XLOG_KEY = sqkResultListView.FocusedItem.SubItems[0].Text;
            TextBoxToSearch.Text = XLOG_KEY;
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
        /// Populates Sortering with index definitions for SHARED_FL and
        /// selects the entry matching FLINDEX_CAPTION(SHARED_FL, A_INDEX).
        /// </summary>
        private void FillSortering()
        {
            // Fill list with all indexes for this table
            ComboBoxSortOn.Items.Clear();

            if (AD_NTDB == null || AD_NTDB.State != (int)ADODB.ObjectStateEnum.adStateOpen)
            {
                MessageBox.Show(
                    @"Databaseverbinding (AD_NTDB) is niet geopend.",
                    @"Indexen",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            ADODB.Recordset rstSchema = null;
            try
            {
                rstSchema = AD_NTDB.OpenSchema(ADODB.SchemaEnum.adSchemaIndexes);

                while (!rstSchema.EOF)
                {
                    var table = Convert.ToString(rstSchema.Fields["TABLE_NAME"].Value ?? string.Empty);
                    if (string.Equals(bstNaam[SHARED_FL], table, StringComparison.OrdinalIgnoreCase))
                    {
                        var columnName = Convert.ToString(rstSchema.Fields["COLUMN_NAME"].Value ?? string.Empty);
                        var indexName = Convert.ToString(rstSchema.Fields["INDEX_NAME"].Value ?? string.Empty);

                        // VB6 format: "+COLUMN; IndexName"
                        string item = "+" + columnName + "; " + indexName;
                        ComboBoxSortOn.Items.Add(item);
                    }

                    rstSchema.MoveNext();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    @"Fout bij ophalen van indexen: " + ex.Message,
                    @"Indexen",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                if (rstSchema != null && rstSchema.State == (int)ADODB.ObjectStateEnum.adStateOpen)
                {
                    rstSchema.Close();
                }
            }

            // Select the entry whose caption matches FLINDEX_CAPTION(SHARED_FL, A_INDEX)
            if (ComboBoxSortOn.Items.Count == 0)
            {
                return;
            }

            int indexNr = 0;
            string wantedCaption = FLINDEX_CAPTION[SHARED_FL, A_INDEX];

            if (!string.IsNullOrWhiteSpace(wantedCaption))
            {
                for (int t = 0; t < ComboBoxSortOn.Items.Count; t++)
                {
                    string item = Convert.ToString(ComboBoxSortOn.Items[t] ?? string.Empty);
                    int semiPos = item.IndexOf(";", StringComparison.Ordinal);
                    if (semiPos < 0 || semiPos + 2 >= item.Length)
                    {
                        continue;
                    }

                    string captionPart = item.Substring(semiPos + 2); // after "; "
                    if (string.Equals(captionPart, wantedCaption, StringComparison.Ordinal))
                    {
                        indexNr = t;
                        break;
                    }
                }
            }

            // If IndexNR Then ... ElseIf Sortering.ListCount Then ...
            ComboBoxSortOn.SelectedIndex = indexNr > 0 ? indexNr : 0;
        }


        /// <summary>
        /// C# translation of VB6 SQLVernieuwTekst.
        /// Builds the SQL string into rtbSQLTekst based on ComboTekst and txtTeZoeken.
        /// Relies on global state (SharedFl, JETTABLEUSE_INDEX, bstNaam, etc.).
        /// </summary>
        /// <param name="comboTekst">VB6 ComboTekst parameter (sort specification string).</param>
        public void SqlRefreshText(string comboTekst)
        {
            string sorteerIndex = "";
            string sorteerOrde = "";
            string sleuteltje;
            int telOrde = 0;

            try
            {
                // grdColWidth(0) = 0
                // GRD_COLWIDTH[0] = 0;

                sleuteltje =
                    "marEDB"
                    + SHARED_FL.ToString("00")
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
            if (ComboBoxSortOn != null && ComboBoxSortOn.Items.Count > 0)
            {
                for (int i = 0; i < ComboBoxSortOn.Items.Count; i++)
                {
                    string item = Convert.ToString(ComboBoxSortOn.Items[i] ?? string.Empty);
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
                        if (i == ComboBoxSortOn.Items.Count - 1)
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
            if (ComboBoxSortOn != null && ComboBoxSortOn.Items.Count > 0)
            {
                for (int i = 0; i < ComboBoxSortOn.Items.Count; i++)
                {
                    string item = Convert.ToString(ComboBoxSortOn.Items[i] ?? string.Empty);
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
                        ComboBoxSortOn.Items.Count != 1 &&
                        !(deLaatste && i == ComboBoxSortOn.Items.Count - 2) &&
                        i < ComboBoxSortOn.Items.Count - 1)
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
