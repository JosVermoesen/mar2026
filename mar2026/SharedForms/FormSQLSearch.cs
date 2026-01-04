using ADODB;
using System;
using System.Windows.Forms;
using static mar2026.Classes.AllFunctions;
using static mar2026.Classes.ModDatabase;
using static mar2026.Classes.ModLibs;

namespace mar2026.SharedForms
{
    public partial class FormSQLSearch : Form
    {
        private readonly int[] _grdColWidth = new int[21];

        public Recordset SqlSearchRS { get; private set; }

        public FormSQLSearch()
        {
            InitializeComponent();
            ComboBoxSortOn.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void FormSQLSearch_Load(object sender, EventArgs e)
        {
            Text = Text + ": " + bstNaam[SHARED_FL];
            sqkResultListView.Clear();

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
            XLOG_KEY = string.Empty;
            Close();
        }

        private void ButtonSearch_Click(object sender, EventArgs e)
        {
            RefreshView();
        }

        private void ComboBoxSortOn_SelectedIndexChanged(object sender, EventArgs e)
        {
            SqlRefreshText(Convert.ToString(ComboBoxSortOn.Text));
            TextBoxToSearch.Text = "%";
            CleanUpForm();
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
            SqlRefreshText(Convert.ToString(ComboBoxSortOn.Text));
        }

        private void TextBoxToSearch_KeyPress(object sender, KeyPressEventArgs e)
        {


        }

        private void TextBoxToSearch_Enter(object sender, EventArgs e)
        {
            ButtonSearchLike.Enabled = true;
            ButtonSearchLike.Visible = true;
            AcceptButton = ButtonSearchLike;

            ButtonOk.Enabled = false;
            ButtonOk.Visible = false;

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
            XLOG_KEY = sqkResultListView.FocusedItem.SubItems[0].Text;
            ButtonOk.PerformClick();
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

        private void ListViewSqlResult_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            XLOG_KEY = sqkResultListView.FocusedItem.SubItems[0].Text;
            ButtonOk.PerformClick();
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
                        //if (ComboBoxSortOn.Items.Count == 2)
                        //{
                        //    break; // prevent excessive entries
                        //}
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
        /// Builds the SQL in rtbSQLTekst based on ComboTekst and TextBoxToSearch.
        /// Uses globals SHARED_FL, JETTABLEUSE_INDEX, bstNaam, etc.
        /// </summary>
        /// <param name="comboTekst">Sort specification string, e.g. &quot;+A100;Naam&quot;.</param>
        public void SqlRefreshText(string comboTekst)
        {
            string sorteerIndex = string.Empty;
            string sorteerOrde = string.Empty;
            string sleuteltje;
            int telOrde = 0;

            try
            {
                // grdColWidth(0) = 0
                _grdColWidth[0] = 0;


                sleuteltje = "marSQL" +
                    SHARED_FL.ToString("00") +
                    comboTekst.Substring(0, comboBoxSortOnIndexOf(comboTekst, ";"));

                // parse all sort parts
                while (true)
                {
                    int countTo = comboBoxSortOnIndexOf(comboTekst, ";", telOrde + 1) - 1;
                    if (countTo < 0)
                    {
                        break;
                    }

                    if (countTo >= 3)
                    {
                        // Mid(ComboTekst, COUNT_TO - 3, 4)
                        string veld = comboTekst.Substring(countTo - 3, 4);
                        char prefix = countTo - 4 >= 0 ? comboTekst[countTo - 4] : '+';

                        if (telOrde == 0)
                        {
                            sorteerIndex = veld;
                            sorteerOrde = veld;
                        }
                        else
                        {
                            sorteerIndex += "+" + veld;
                            sorteerOrde += ", " + veld;
                        }

                        sorteerOrde += prefix == '+' ? " ASC" : " DESC";
                    }

                    telOrde = countTo + 1;
                }

                // bGet TABLE_VARIOUS, 1, "29" + Sleuteltje
                string key = "29" + sleuteltje;
                // BGet(TABLE_VARIOUS, 1, key);
                JetGet(TABLE_VARIOUS, 1, key);


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
                    // strip existing WHERE .. tail
                    int wherePos = upperMsg.IndexOf(" WHERE ", StringComparison.Ordinal);
                    if (wherePos > 0)
                    {
                        msg = msg.Substring(0, wherePos);
                    }

                    msg += " WHERE " + sorteerIndex +
                           " Like " + "'" + TextBoxToSearch.Text + "'" +
                           " ORDER BY " + sorteerOrde;

                    RichTextBoxSQLSelect.Text = msg;

                    // parse [Colwidth] section
                    string full = VBibTekst(TABLE_VARIOUS, "#v132 #");
                    int colPos = full.IndexOf("[Colwidth]", StringComparison.Ordinal);
                    if (colPos >= 0)
                    {
                        string widthsPart = full.Substring(colPos + "[Colwidth]".Length);
                        if (string.IsNullOrEmpty(widthsPart))
                        {
                            _grdColWidth[0] = 0;
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
                                if (int.TryParse(part, out int val))
                                {
                                    _grdColWidth[countTo] = val;
                                }

                                widthsPart = widthsPart.Substring(tabPos + 1);
                                countTo++;
                            }

                            _grdColWidth[countTo] = 0;
                        }
                    }
                    else
                    {
                        _grdColWidth[0] = 0;
                    }
                }
                else
                {
                    // GoSub InitSQL
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
        /// Helper: safe IndexOf wrapper mimicking VB InStr behaviour used in the VB code.
        /// </summary>
        private static int comboBoxSortOnIndexOf(string text, string value, int start = 0)
        {
            if (string.IsNullOrEmpty(text))
            {
                return -1;
            }

            int pos = text.IndexOf(value, start, StringComparison.Ordinal);
            return pos < 0 ? -1 : pos;
        }

        /// <summary>
        /// C# version of the InitSQL GoSub inside SQLVernieuwTekst.
        /// Builds a default SELECT into rtbSQLTekst using the items in ComboBoxSortOn.
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
                        string alias = item.Substring(semiPos + 2);
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
                    @"Hoofdindex bestaat niet (meer)",
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

                    string alias = item.Substring(semiPos + 2);
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

            // Remove any trailing comma before adding FROM
            msg = msg.TrimEnd();
            if (msg.EndsWith(",", StringComparison.Ordinal))
            {
                msg = msg.Substring(0, msg.Length - 1);
            }

            msg += " FROM " + bstNaam[SHARED_FL];
            msg += " WHERE " + sorteerIndex +
                   " Like " + "\"" + TextBoxToSearch.Text + "\"" +
                   " ORDER BY " + sorteerOrde;

            RichTextBoxSQLSelect.Text = msg;
        }

        private void CleanUpForm()
        {
            // throw new NotImplementedException();
        }

        private void ButtonSQLToggle_Click(object sender, EventArgs e)
        {
            RichTextBoxSQLSelect.Enabled = !RichTextBoxSQLSelect.Enabled;
        }

        private void RefreshView()
        {
            // Build SQL text based on current sort selection
            var selectedItem = Convert.ToString(ComboBoxSortOn.SelectedItem ?? string.Empty);
            if (string.IsNullOrEmpty(selectedItem))
            {
                return;
            }

            SqlRefreshText(selectedItem);
            var sSQL = RichTextBoxSQLSelect.Text.Replace("\"", "'");

            Cursor = Cursors.WaitCursor;

            try
            {
                if (SqlSearchRS != null &&
                    SqlSearchRS.State == (int)ObjectStateEnum.adStateOpen)
                {
                    SqlSearchRS.Close();
                }
            }
            catch
            {
                // VB: ignored
            }

            try
            {
                SqlSearchRS = new Recordset
                {
                    CursorLocation = CursorLocationEnum.adUseClient
                };

                // In VB this used AD_NTDB; here you used a connection string.
                // Keep your existing behavior:
                string connectionString =
                    SharedGlobals.DbJetProvider +
                    SharedGlobals.MimDataLocation +
                    SharedGlobals.MarntMdvLocation +
                    "marnt.mdv";

                SqlSearchRS.Open(
                    sSQL,
                    connectionString,
                    CursorTypeEnum.adOpenForwardOnly,
                    LockTypeEnum.adLockReadOnly);

                // Clear and rebuild columns
                sqkResultListView.Clear();

                for (int i = 0; i < SqlSearchRS.Fields.Count; i++)
                {
                    var name = Convert.ToString(SqlSearchRS.Fields[i].Name ?? string.Empty);
                    sqkResultListView.Columns.Add(name, 100);
                }

                sqkResultListView.View = View.Details;

                if (SqlSearchRS.RecordCount > 0)
                {
                    SqlSearchRS.MoveFirst();
                    while (!SqlSearchRS.EOF)
                    {
                        string firstValue = Convert.ToString(
                            SqlSearchRS.Fields[0].Value ?? " ");

                        var item = new ListViewItem(firstValue);

                        for (int i = 1; i < SqlSearchRS.Fields.Count; i++)
                        {
                            object value = SqlSearchRS.Fields[i].Value;
                            string cell = value == null || value is DBNull
                                ? " "
                                : Convert.ToString(value);
                            item.SubItems.Add(cell);
                        }

                        sqkResultListView.Items.Add(item);
                        SqlSearchRS.MoveNext();
                    }
                }

                if (SqlSearchRS.RecordCount > 0)
                {
                    // You can wire a label similarly to VB's recordsLabel if you have one                    
                    sqkResultListView.FullRowSelect = true;

                    if (sqkResultListView.Items.Count > 0)
                    {
                        var firstItem = sqkResultListView.Items[0];
                        XLOG_KEY = firstItem.SubItems[0].Text;
                        TextBoxToSearch.Text = XLOG_KEY;
                        sqkResultListView.Enabled = true;
                        sqkResultListView.Focus();

                        ButtonOk.Enabled = true;
                        ButtonOk.Visible = true;
                        AcceptButton = ButtonOk;

                        ButtonSearchLike.Enabled = false;
                        ButtonSearchLike.Visible = false;
                    }
                }
                else
                {
                    ButtonOk.Enabled = false;
                    ButtonOk.Visible = false;
                    AcceptButton = ButtonSearchLike;

                    ButtonSearchLike.Enabled = true;
                    ButtonSearchLike.Visible = true;
                    TextBoxToSearch.Focus();
                }
            }
            catch
            {
                // Keep debugging behavior close to VB's Stop
                throw;
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void ButtonOk_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void sqkResultListView_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Enter key?
            if (e.KeyChar == (char)Keys.Return || e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;

                // Ensure there is a focused item
                if (sqkResultListView.FocusedItem != null &&
                    sqkResultListView.FocusedItem.SubItems != null &&
                    sqkResultListView.FocusedItem.SubItems.Count > 0)
                {
                    XLOG_KEY = sqkResultListView.FocusedItem.SubItems[0].Text;
                    TextBoxToSearch.Text = XLOG_KEY;
                }
                ButtonOk.PerformClick();
            }
        }
    }
}





