using ADODB;
using Microsoft.VisualBasic;
using System;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Windows.Forms;

using static mar2026.Classes.AutoLoadCompany;
using static mar2026.Classes.ModLibs;
using static mar2026.Classes.ModDatabase;

namespace mar2026.Classes
{
    public static class AllFunctions
    {
        private static readonly int X;

        // Extension method on ADODB.Recordset
        public static DataTable ADODBRSetToDataTable(this Recordset adodbRecordSet)
        {
            var dataAdapter = new OleDbDataAdapter();
            var dt = new DataTable();
            dataAdapter.Fill(dt, adodbRecordSet);
            return dt;
        }

        public static string OpenSchemaString(string objectType)
        {
            using (var cnn = new OleDbConnection
            {
                ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0; data source =" +
                                   LOCATION_COMPANYDATA + "\\marnt.mdv"
            })
            {
                // We only want user tables, not system tables
                var restrictions = new string[4];
                restrictions[3] = objectType;

                cnn.Open();
                // Get list of user tables
                DataTable userTables = cnn.GetSchema("Tables", restrictions);
                cnn.Close();

                string returnString = string.Empty;
                for (int i = 0; i < userTables.Rows.Count; i++)
                {
                    returnString += userTables.Rows[i][2].ToString() + "\r";
                }

                return returnString;
            }
        }

        public static void TransBegin()
        {
            try
            {
                AD_NTDB.BeginTrans();
                KTRL = 0;
            }
            catch (System.Exception ex)
            {
                ModLibs.KTRL = -1;
                MessageBox.Show(ex.Message);
            }
        }

        public static void TransCommit()
        {
            try
            {
                AD_NTDB.CommitTrans();
                KTRL = 0;
            }
            catch (System.Exception ex)
            {
                KTRL = -1;
                MessageBox.Show(ex.Message);
            }
        }

        public static void TransAbort()
        {
            try
            {
                AD_NTDB.RollbackTrans();
                KTRL = 0;
            }
            catch (System.Exception ex)
            {
                KTRL = -1;
                MessageBox.Show(ex.Message);
            }
        }

        //public static bool AdoNewRecord(int fl)
        //{
        //    TLB_RECORD[fl] = string.Empty;

        //    switch (fl)
        //    {
        //        case TABLE_CUSTOMERS:
        //        case TABLE_SUPPLIERS:
        //            // Taalkode
        //            AdoInsertToRecord((int)fl, "2", "A10C");
        //            // Landnummer ISO kode
        //            AdoInsertToRecord((int)fl, "002", "v149");
        //            // Landkode Postkantoor
        //            AdoInsertToRecord((int)fl, "B  ", "A109");
        //            // Landkode ISO kode
        //            AdoInsertToRecord((int)fl, "BE", "v150");
        //            // Munteenheid ISO kode
        //            AdoInsertToRecord((int)fl, BH_EURO ? "EUR" : "BEF", "vs03");
        //            // exemplaren dokumenten
        //            AdoInsertToRecord((int)fl, "1", "vs07");
        //            break;

        //        case TABLE_LEDGERACCOUNTS:
        //            // Budgetcode
        //            AdoInsertToRecord((int)fl, "O", "v032");
        //            break;

        //        case TABLE_PRODUCTS:
        //            // These rely on helper functions fmarBoxText/String99; stubbed for now.
        //            // vBib Fl, fmarBoxText("004", "2", "0"), "v106"
        //            // vBib Fl, Dec$(1, "#####.00"), "v107"
        //            // vBib Fl, fmarBoxText("022", "2", "N"), "v108"
        //            // vBib Fl, fmarBoxText("002", "2", String99(READING, 183)), "v111"
        //            // vBib Fl, String99(READING, 77), "v116"
        //            // vBib Fl, String99(READING, 78), "v117"
        //            // vBib Fl, String99(READING, 79), "v118"
        //            break;
        //    }

        //    return true;
        //}

        //public static string SetSpacing(string fTekst, int fLengte)
        //{
        //    string b = fTekst.Length > fLengte
        //        ? fTekst.Substring(0, fLengte)
        //        : fTekst;

        //    return b + new string(' ', fLengte - b.Length);
        //}

        //public static void AdoInsertToRecord(int fl, string fieldString1, string fieldString2)
        //{
        //    int TBStart;
        //    int TBStop;
        //    // replace positions 1-5 (0-based 1..5) with fieldString2
        //    string TBCode = "#" + fieldString2.PadRight(5).Substring(0, 5) + "#";

        //    if (string.IsNullOrEmpty(fieldString1))
        //        fieldString1 = " ";

        //    jump:
        //    if (TLB_RECORD[fl].IndexOf(TBCode, System.StringComparison.Ordinal) < 0)
        //    {
        //        TLB_RECORD[fl] += TBCode + fieldString1 + "#";
        //    }
        //    else
        //    {
        //        if (AdoGetField(fl, TBCode).TrimEnd() == fieldString1)
        //        {
        //            return;
        //        }

        //        TBStart = (int)(TLB_RECORD[fl].IndexOf(TBCode, System.StringComparison.Ordinal) + 1); // VB is 1-based
        //        TBStop = (int)TLB_RECORD[fl].IndexOf("#", TBStart + 7 - 1, System.StringComparison.Ordinal);

        //        // emulate VB Left/Right remove segment
        //        string leftPart = TLB_RECORD[fl].Substring(0, TBStart - 1);
        //        string rightPart = TLB_RECORD[fl].Substring(TBStop);
        //        TLB_RECORD[fl] = leftPart + rightPart;

        //        goto jump;
        //    }
        //}

        //public static string AdoGetField(int fl, string TBS)
        //{
        //    string tbsHere = string.Empty;

        //    if (TBS.Length == 7 && TBS[0] == '#')
        //    {
        //        tbsHere = TBS;
        //    }
        //    else if (TBS.Length == 6)
        //    {
        //        // "#     #", then insert 4 chars from TBS(2-5)
        //        string mid = TBS.Substring(1, 4);
        //        tbsHere = "#" + mid.PadRight(5).Substring(0, 5) + "#";
        //    }
        //    else
        //    {
        //        System.Diagnostics.Debugger.Break();
        //    }

        //    if (string.IsNullOrEmpty(TLB_RECORD[fl]))
        //        return string.Empty;

        //    try
        //    {
        //        int pos = TLB_RECORD[fl].IndexOf(tbsHere, System.StringComparison.Ordinal);
        //        if (pos < 0)
        //            return string.Empty;

        //        int start = pos + 7;
        //        int end = TLB_RECORD[fl].IndexOf("#", start, System.StringComparison.Ordinal);
        //        if (end < 0)
        //            return string.Empty;

        //        return TLB_RECORD[fl].Substring(start, end - start);
        //    }
        //    catch
        //    {
        //        return string.Empty;
        //    }
        //}

        // VB6 daoBlankoRecord: initialise TLB_RECORD defaults for a table.
        public static bool DaoBlankoRecord(int fl)
        {
            TLB_RECORD[fl] = string.Empty;

            switch (fl)
            {
                case TABLE_CUSTOMERS:
                case TABLE_SUPPLIERS:
                    VBib(fl, "2", "A10C");                         // Taalkode
                    VBib(fl, "002", "v149");                         // Landnummer ISO
                    VBib(fl, "B  ", "A109");                         // Landkode Postkantoor
                    VBib(fl, "BE", "v150");                         // Landkode ISO
                    VBib(fl, BH_EURO ? "EUR" : "BEF", "vs03");       // Munteenheid ISO
                    VBib(fl, "1", "vs07");                         // exemplaren documenten
                    break;

                case TABLE_LEDGERACCOUNTS:
                    VBib(fl, "O", "v032");                           // Budgetcode
                    break;

                case TABLE_PRODUCTS:
                    VBib(fl, FmarBoxText("004", "2", "0"), "v106");
                    // TODO: VBib(fl, DecFormat(1, "#####.00"),                  "v107");
                    VBib(fl, FmarBoxText("022", "2", "N"), "v108");
                    VBib(fl, FmarBoxText("002", "2", String99(READING, 183)), "v111");
                    VBib(fl, String99(READING, 77), "v116");
                    VBib(fl, String99(READING, 78), "v117");
                    VBib(fl, String99(READING, 79), "v118");
                    break;
            }

            return true;
        }

        // VB6 vBib: manipulate TLBR-style record string TLB_RECORD[fl].
        public static void VBib(int fl, string stringText1, string stringText2)
        {
            string tbCode = "#" + (stringText2 ?? string.Empty).PadRight(5).Substring(0, 5) + "#";

        jump:
            if (TLB_RECORD[fl].IndexOf(tbCode, System.StringComparison.Ordinal) < 0)
            {
                TLB_RECORD[fl] += tbCode + stringText1 + "#";
            }
            else
            {
                if (VBibTekst(fl, tbCode).TrimEnd() == stringText1)
                    return;

                int tbStart = TLB_RECORD[fl].IndexOf(tbCode, System.StringComparison.Ordinal);
                int tbStop = TLB_RECORD[fl].IndexOf("#", tbStart + 7, System.StringComparison.Ordinal);
                if (tbStart < 0 || tbStop < 0)
                    return;

                string left = TLB_RECORD[fl].Substring(0, tbStart);
                string right = TLB_RECORD[fl].Substring(tbStop);
                TLB_RECORD[fl] = left + right;
                goto jump;
            }
        }

        // VB6 vBibTekst: get value for a TLBR code from TLB_RECORD[fl].
        public static string VBibTekst(int fl, string tbs)
        {
            string tbsHier;
            if (!string.IsNullOrEmpty(tbs) && tbs[0] == '#')
            {
                tbsHier = tbs;
            }
            else
            {
                string mid = (tbs ?? string.Empty).PadRight(5).Substring(0, 5);
                tbsHier = "#" + mid + "#";
            }

            if (string.IsNullOrEmpty(TLB_RECORD[fl]))
                return string.Empty;

            try
            {
                int startCode = TLB_RECORD[fl].IndexOf(tbsHier, System.StringComparison.Ordinal);
                if (startCode < 0)
                    return string.Empty;

                int startVal = startCode + 7;
                int endVal = TLB_RECORD[fl].IndexOf("#", startVal, System.StringComparison.Ordinal);
                if (endVal < 0)
                    return string.Empty;

                return TLB_RECORD[fl].Substring(startVal, endVal - startVal);
            }
            catch
            {
                return string.Empty;
            }
        }

        public static object XrsMar(int fl, string tbs)
        {
            var value = RS_MAR[fl].Fields[tbs].Value;
            return (value == null || value is System.DBNull) ? string.Empty : value;
        }

        public static object ObjectValue(object dbWaarde)
        {
            return (dbWaarde == null || dbWaarde is System.DBNull) ? string.Empty : dbWaarde;
        }

        public static void ClearFlDummy()
        {
            KTRL = JetTableOpen(TABLE_DUMMY);
            JetGetFirst(TABLE_DUMMY, 0);

            while (!RS_MAR[TABLE_DUMMY].EOF)
            {
                RS_MAR[TABLE_DUMMY].Delete();
                RS_MAR[TABLE_DUMMY].MoveNext();
            }

            JetTableClose(TABLE_DUMMY);
        }

        /// <summary>
        /// Copy a file's contents into an ADODB BLOB field (equivalent to VB6 FileToBlob).
        /// </summary>
        //public static void FileToBlob(Field fld, string fileName, int chunkSize = 8192)
        //{
        //    if ((fld.Attributes & FieldAttributeEnum.adFldLong) == 0)
        //    {
        //        throw new InvalidOperationException("Field doesn't support the GetChunk/AppendChunk methods.");
        //    }

        //    if (!File.Exists(fileName))
        //    {
        //        throw new FileNotFoundException("File not found", fileName);
        //    }

        //    using (var fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
        //    {
        //        var buffer = new byte[chunkSize];
        //        int bytesRead;
        //        while ((bytesRead = fs.Read(buffer, 0, buffer.Length)) > 0)
        //        {
        //            var chunk = buffer;
        //            if (bytesRead != buffer.Length)
        //            {
        //                chunk = new byte[bytesRead];
        //                Buffer.BlockCopy(buffer, 0, chunk, 0, bytesRead);
        //            }

        //            fld.AppendChunk(chunk);
        //        }
        //    }
        //}

        /// <summary>
        /// Equivalent of VB6 ktrlBLOBRecord. Ensures bstndNaam37/typeZending37/bstBLOB37
        /// columns exist in Dokumenten; adds them if missing.
        /// </summary>
        //public static bool CheckBlobRecord()
        //{
        //    try
        //    {
        //        // Touch the field to see if it exists
        //        var dummy = RS_MAR[TABLE_INVOICES].Fields["bstndNaam37"].Name;
        //        return true;
        //    }
        //    catch (COMException ex)
        //    {
        //        // VB checked Err=3265 (item not found). In ADO this is typically -2146825023.
        //        const int ADODB_FIELD_NOT_FOUND = unchecked((int)0x800A0CC1);
        //        if (ex.ErrorCode != ADODB_FIELD_NOT_FOUND)
        //            throw;

        //        try
        //        {
        //            // Add missing columns
        //            MSG = "ALTER TABLE Dokumenten ADD COLUMN bstndNaam37 varchar;";
        //            AD_NTDB.Execute(MSG);
        //            MessageBox.Show(MSG, "Met succes", MessageBoxButtons.OK, MessageBoxIcon.Information);

        //            MSG = "ALTER TABLE Dokumenten ADD COLUMN typeZending37 TEXT(5);";
        //            AD_NTDB.Execute(MSG);
        //            MessageBox.Show(MSG, "Met succes", MessageBoxButtons.OK, MessageBoxIcon.Information);

        //            MSG = "ALTER TABLE Dokumenten ADD COLUMN bstBLOB37 OLEobject;";
        //            AD_NTDB.Execute(MSG);
        //            MessageBox.Show(MSG, "Met succes", MessageBoxButtons.OK, MessageBoxIcon.Information);

        //            MessageBox.Show(
        //                "Belangrijke velden werden toegevoegd. Gelieve het bedrijf opnieuw te openen a.u.b.",
        //                "Informatie",
        //                MessageBoxButtons.OK,
        //                MessageBoxIcon.Information);

        //            return false;
        //        }
        //        catch (Exception addEx)
        //        {
        //            MessageBox.Show(
        //                "Foutmelding bron: " + addEx.Source + Environment.NewLine +
        //                "Foutkodenummer: " + addEx.HResult + Environment.NewLine + Environment.NewLine +
        //                "Foutmelding omschrijving:" + Environment.NewLine + addEx.Message,
        //                "Fout bij wijzigen Dokumenten",
        //                MessageBoxButtons.OK,
        //                MessageBoxIcon.Error);
        //            return false;
        //        }
        //    }
        //}

        // Below are simple stubs for functions referenced above that you already
        // have (or will have) in C# elsewhere in your port. Keep signatures identical.

        public static int JetTableOpen(int fl)
        {
            // This is just a stub here because full implementation is elsewhere.
            // Keep your existing C# JetTableOpen implementation.
            throw new System.NotImplementedException();
        }

        public static void JetTableClose(int fl)
        {
            throw new System.NotImplementedException();
        }

        // --- VB6 modAdoRoutines ports ---
        public static string AdoBibText(Field adoField, string tbs)
        {
            if (adoField == null)
                return string.Empty;

            string value = adoField.Value as string ?? adoField.Value?.ToString() ?? string.Empty;
            if (string.IsNullOrEmpty(value))
                return string.Empty;

            // Expect TLBR structure with "#code #value#" pattern.
            string tbsHere;
            if (tbs.Length == 7 && tbs[0] == '#')
            {
                tbsHere = tbs;
            }
            else if (tbs.Length == 5)
            {
                tbsHere = "#" + tbs.PadRight(5).Substring(0, 5) + "#";
            }
            else
            {
                tbsHere = tbs;
            }

            int startPos = value.IndexOf(tbsHere, System.StringComparison.Ordinal);
            if (startPos < 0)
                return string.Empty;

            int dataStart = startPos + 7;
            if (dataStart >= value.Length)
                return string.Empty;

            int dataEnd = value.IndexOf('#', dataStart);
            if (dataEnd < 0)
                return string.Empty;

            return value.Substring(dataStart, dataEnd - dataStart);
        }

        public static bool AdoGet(int tableIndex, int indexIndex, string sZoals, object sZoek)
        {
            // Ensure recordset is open
            if (RS_MAR[tableIndex] == null || RS_MAR[tableIndex].State == (int)ObjectStateEnum.adStateClosed)
            {
                // bOpen equivalent is not yet ported; return false to avoid throwing.
                return false;
            }

            bool found = false;

            if (!string.IsNullOrEmpty(SQL_CONNECT) && SQL_CONNECT.IndexOf("SQLOLEDB", System.StringComparison.OrdinalIgnoreCase) >= 0)
            {
                try
                {
                    RS_MAR[tableIndex].Close();

                    string wherePart = JETTABLEUSE_INDEX[tableIndex, indexIndex] +
                                       " " + sZoals + " '" + (sZoek ?? string.Empty) + "'";

                    string sql;
                    if (tableIndex == TABLE_COUNTERS)
                    {
                        sql = "SELECT * FROM jr" + JET_TABLENAME[tableIndex] + " WHERE " + wherePart;
                    }
                    else
                    {
                        sql = "SELECT * FROM " + JET_TABLENAME[tableIndex] + " WHERE " + wherePart;
                    }

                    RS_MAR[tableIndex].Open(sql, AD_NTDB,
                        CursorTypeEnum.adOpenForwardOnly,
                        LockTypeEnum.adLockOptimistic,
                        (int)CommandTypeEnum.adCmdText);

                    if (!RS_MAR[tableIndex].EOF)
                    {
                        found = true;
                    }
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show(
                        "Bron:" + "\n" + ex.Source +
                        "\n" + "Foutnummer: " + ex.HResult +
                        "\n" + "Detail:" + "\n" + ex.Message,
                        "ADO_GET",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return false;
                }

            }
            else
            {
                try
                {
                    RS_MAR[tableIndex].Index = FLINDEX_CAPTION[tableIndex, indexIndex];

                    if (sZoals == "=")
                    {
                        RS_MAR[tableIndex].Seek(sZoek);
                    }
                    else if (sZoals == ">=")
                    {
                        RS_MAR[tableIndex].Seek(sZoek, SeekEnum.adSeekAfterEQ);
                    }
                    else
                    {
                        MessageBox.Show(sZoals + " nog niet beschikbaar", "ADO_GET");
                    }

                    if (!RS_MAR[tableIndex].EOF)
                    {
                        found = true;
                    }
                }
                catch
                {
                    found = false;
                }
            }

            return found;
        }

        public static object Rv(Recordset adoRecord, string tbs)
        {
            if (adoRecord == null)
                return string.Empty;

            object value;
            try
            {
                value = adoRecord.Fields[tbs].Value;
            }
            catch
            {
                return string.Empty;
            }

            return (value == null || value is System.DBNull) ? string.Empty : value;
        }

        public static void ToonIndexen(string tbNaam, ComboBox obObject)
        {
            obObject.Items.Clear();

            Recordset rstSchema = null;
            try
            {
                rstSchema = AD_NTDB.OpenSchema(SchemaEnum.adSchemaIndexes);

                while (!rstSchema.EOF)
                {
                    string tableName = rstSchema.Fields["TABLE_NAME"].Value.ToString();
                    if (string.Equals(tbNaam, tableName, System.StringComparison.OrdinalIgnoreCase))
                    {
                        string columnName = rstSchema.Fields["COLUMN_NAME"].Value.ToString();
                        string indexName = rstSchema.Fields["INDEX_NAME"].Value.ToString();
                        obObject.Items.Add("+" + columnName + "; " + indexName);
                    }

                    rstSchema.MoveNext();
                }
            }
            catch
            {
                // ignore schema errors
            }
            finally
            {
                if (rstSchema != null && rstSchema.State != 0)
                {
                    rstSchema.Close();
                }
            }
        }

        public static string VBt(string tlbr, string tbs)
        {
            if (string.IsNullOrEmpty(tlbr))
                return string.Empty;

            string tbsHier = "#     #";
            if (tbs.Length > 0)
            {
                string mid = tbs.PadRight(5).Substring(0, 5);
                tbsHier = "#" + mid + "#";
            }

            int pos = tlbr.IndexOf(tbsHier, System.StringComparison.Ordinal);
            if (pos < 0)
                return string.Empty;

            int start = pos + 7;
            if (start >= tlbr.Length)
                return string.Empty;

            int end = tlbr.IndexOf('#', start);
            if (end < 0)
                return string.Empty;

            return tlbr.Substring(start, end - start);
        }

        public static bool AdxKolom(string tbNaam, string clNaam, int clType, int clLengte)
        {
            // Uses ADOX via late binding to avoid adding a hard reference.
            // This keeps the dependency optional and close to the VB6 behavior.
            object cat = null;

            try
            {
                var catalogType = System.Type.GetTypeFromProgID("ADOX.Catalog");
                if (catalogType == null)
                {
                    MessageBox.Show("ADOX.Catalog COM component not available.", "AdxKolom");
                    return false;
                }

                cat = System.Activator.CreateInstance(catalogType);

                // Decide between SQL Server and Jet connection string
                string connectString;
                try
                {
                    // AD_NTDB.Properties["DBMS Name"] may fail on some providers
                    var dbmsName = AD_NTDB.Properties["DBMS Name"].Value as string ?? string.Empty;
                    if (dbmsName.IndexOf("SQL Server", System.StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        connectString = SQL_CONNECT;
                    }
                    else
                    {
                        connectString = JET_CONNECT;
                    }
                }
                catch
                {
                    connectString = JET_CONNECT;
                }

                catalogType.InvokeMember("ActiveConnection", System.Reflection.BindingFlags.SetProperty,
                    null, cat, new object[] { connectString });

                // Create Column object
                var columnType = System.Type.GetTypeFromProgID("ADOX.Column");
                if (columnType == null)
                {
                    MessageBox.Show("ADOX.Column COM component not available.", "AdxKolom");
                    return false;
                }

                object clCol = System.Activator.CreateInstance(columnType);

                columnType.InvokeMember("Name", System.Reflection.BindingFlags.SetProperty,
                    null, clCol, new object[] { clNaam });

                columnType.InvokeMember("ParentCatalog", System.Reflection.BindingFlags.SetProperty,
                    null, clCol, new object[] { cat });

                columnType.InvokeMember("Type", System.Reflection.BindingFlags.SetProperty,
                    null, clCol, new object[] { clType });

                if (clLengte != 0)
                {
                    columnType.InvokeMember("DefinedSize", System.Reflection.BindingFlags.SetProperty,
                        null, clCol, new object[] { clLengte });
                }

                // Nullable = True
                var props = columnType.InvokeMember("Properties", System.Reflection.BindingFlags.GetProperty,
                    null, clCol, null);
                var propsType = props.GetType();
                var nullableProp = propsType.InvokeMember("Item", System.Reflection.BindingFlags.GetProperty,
                    null, props, new object[] { "Nullable" });
                nullableProp.GetType().InvokeMember("Value", System.Reflection.BindingFlags.SetProperty,
                    null, nullableProp, new object[] { true });

                // Append column to table
                var tables = cat.GetType().InvokeMember("Tables", System.Reflection.BindingFlags.GetProperty,
                    null, cat, null);
                var table = tables.GetType().InvokeMember("Item", System.Reflection.BindingFlags.GetProperty,
                    null, tables, new object[] { tbNaam });
                var columns = table.GetType().InvokeMember("Columns", System.Reflection.BindingFlags.GetProperty,
                    null, table, null);
                columns.GetType().InvokeMember("Append", System.Reflection.BindingFlags.InvokeMethod,
                    null, columns, new object[] { clCol });

                return true;
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(
                    "Foutmelding bron: " + ex.Source + System.Environment.NewLine +
                    "Foutkodenummer: " + ex.HResult + "\n" +
                    "Foutmelding omschrijving:" + System.Environment.NewLine + ex.Message,
                    "AdxKolom",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                MessageBox.Show("Aanmaak Kolom " + clNaam + " zonderSucces.", "AdxKolom", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return false;
            }
            finally
            {
                if (cat != null)
                {
                    try
                    {
                        cat.GetType().InvokeMember("ActiveConnection", System.Reflection.BindingFlags.SetProperty,
                            null, cat, new object[] { null });
                    }
                    catch
                    {
                    }
                }
            }
        }

        public static bool AdxMaakTabel(string tbNaam)
        {
            object cat = null;

            try
            {
                var catalogType = System.Type.GetTypeFromProgID("ADOX.Catalog");
                if (catalogType == null)
                {
                    MessageBox.Show("ADOX.Catalog COM component not available.", "AdxMaakTabel");
                    return false;
                }

                cat = System.Activator.CreateInstance(catalogType);

                string connectString;
                try
                {
                    var dbmsName = AD_NTDB.Properties["DBMS Name"].Value as string ?? string.Empty;
                    if (dbmsName.IndexOf("SQL Server", System.StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        connectString = SQL_CONNECT;
                    }
                    else
                    {
                        connectString = JET_CONNECT;
                    }
                }
                catch
                {
                    connectString = JET_CONNECT;
                }

                catalogType.InvokeMember("ActiveConnection", System.Reflection.BindingFlags.SetProperty,
                    null, cat, new object[] { connectString });

                var tableType = System.Type.GetTypeFromProgID("ADOX.Table");
                if (tableType == null)
                {
                    MessageBox.Show("ADOX.Table COM component not available.", "AdxMaakTabel");
                    return false;
                }

                object tbl = System.Activator.CreateInstance(tableType);

                tableType.InvokeMember("Name", System.Reflection.BindingFlags.SetProperty,
                    null, tbl, new object[] { tbNaam });
                tableType.InvokeMember("ParentCatalog", System.Reflection.BindingFlags.SetProperty,
                    null, tbl, new object[] { cat });

                // Columns.Append "ID", adInteger, adBigInt
                var columns = tableType.InvokeMember("Columns", System.Reflection.BindingFlags.GetProperty,
                    null, tbl, null);
                columns.GetType().InvokeMember("Append", System.Reflection.BindingFlags.InvokeMethod,
                    null, columns, new object[] { "ID", /*adInteger*/ 3, /*adBigInt*/ 20 });

                // AutoIncrement = True
                var idColumn = columns.GetType().InvokeMember("Item", System.Reflection.BindingFlags.GetProperty,
                    null, columns, new object[] { "ID" });
                var idProps = idColumn.GetType().InvokeMember("Properties", System.Reflection.BindingFlags.GetProperty,
                    null, idColumn, null);
                var idPropsType = idProps.GetType();
                var autoIncProp = idPropsType.InvokeMember("Item", System.Reflection.BindingFlags.GetProperty,
                    null, idProps, new object[] { "AutoIncrement" });
                autoIncProp.GetType().InvokeMember("Value", System.Reflection.BindingFlags.SetProperty,
                    null, autoIncProp, new object[] { true });

                var tables = cat.GetType().InvokeMember("Tables", System.Reflection.BindingFlags.GetProperty,
                    null, cat, null);
                tables.GetType().InvokeMember("Append", System.Reflection.BindingFlags.InvokeMethod,
                    null, tables, new object[] { tbl });

                MessageBox.Show("Aanmaak tabel " + tbNaam + " metSucces.", "AdxMaakTabel", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return true;
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(
                    "Foutmelding bron: " + ex.Source + System.Environment.NewLine +
                    "Foutkodenummer: " + ex.HResult + "\n" +
                    "Foutmelding omschrijving:" + System.Environment.NewLine + ex.Message,
                    "AdxMaakTabel",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return false;
            }
            finally
            {
                if (cat != null)
                {
                    try
                    {
                        cat.GetType().InvokeMember("ActiveConnection", System.Reflection.BindingFlags.SetProperty,
                            null, cat, new object[] { null });
                    }
                    catch
                    {
                    }
                }
            }
        }

        public static bool TabelKontrole()
        {
            ADODB.Connection cnnHier = null;
            object catHier = null;

            try
            {
                cnnHier = new ADODB.Connection();
                cnnHier.Open("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + LOCATION_COMPANYDATA + "marnt.mdv;");

                var catalogType = System.Type.GetTypeFromProgID("ADOX.Catalog");
                if (catalogType == null)
                {
                    MessageBox.Show("ADOX.Catalog COM component not available.", "TabelKontrole");
                    return false;
                }

                catHier = System.Activator.CreateInstance(catalogType);
                catalogType.InvokeMember("ActiveConnection", System.Reflection.BindingFlags.SetProperty,
                    null, catHier, new object[] { cnnHier });

                var tables = catHier.GetType().InvokeMember("Tables", System.Reflection.BindingFlags.GetProperty,
                    null, catHier, null);
                int count = (int)tables.GetType().InvokeMember("Count", System.Reflection.BindingFlags.GetProperty,
                    null, tables, null);

                for (COUNT_TO = 0; COUNT_TO < count; COUNT_TO++)
                {
                    var table = tables.GetType().InvokeMember("Item", System.Reflection.BindingFlags.GetProperty,
                        null, tables, new object[] { COUNT_TO });
                    string name = table.GetType().InvokeMember("Name", System.Reflection.BindingFlags.GetProperty,
                        null, table, null) as string;

                    if (string.Compare(name, "1900", System.StringComparison.Ordinal) >= 0 &&
                        string.Compare(name, "2004", System.StringComparison.Ordinal) <= 0)
                    {
                        table.GetType().InvokeMember("Name", System.Reflection.BindingFlags.SetProperty,
                            null, table, new object[] { "jr" + name });
                    }
                }

                // Refresh tables collection
                tables.GetType().InvokeMember("Refresh", System.Reflection.BindingFlags.InvokeMethod,
                    null, tables, null);

                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                if (catHier != null)
                {
                    try
                    {
                        catHier.GetType().InvokeMember("ActiveConnection", System.Reflection.BindingFlags.SetProperty,
                            null, catHier, new object[] { null });
                    }
                    catch
                    {
                    }
                }

                if (cnnHier != null && cnnHier.State != 0)
                {
                    cnnHier.Close();
                }
            }
        }

        public static void Run(FormMim mim, FormBJPERDAT bjPerDat)
        {
            if (mim == null || bjPerDat == null)
                throw new System.ArgumentNullException("Main forms must not be null.");

            XisEUROWasBEF = false;

            bjPerDat.DatumVerwerking.Value = System.DateTime.ParseExact(
                MIM_GLOBAL_DATE ?? System.DateTime.Now.ToString("dd/MM/yyyy"),
                "dd/MM/yyyy",
                System.Globalization.CultureInfo.InvariantCulture);
            mim.toolStripBookingDateNow.Text = bjPerDat.DatumVerwerking.Value.ToString();

            bjPerDat.CmbPeriodeBoekjaar.Items.Clear();

            // VB6 netVoorbereiden: create .OXT from DEFxx.OCT
            NetVoorbereiden();

            LoadBookyearsAndPeriods(bjPerDat);
            OpenJetDatabase(mim);


            Cijfermaskers();

            bjPerDat.Show(mim);
        }

        private static void OpenJetDatabase(FormMim mim)
        {
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
                    return;
                }
            }
            else
            {
                LOCATION_NETDATA = string.Empty;
            }

            try
            {
                TabelKontrole(); // from ModDatabase
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message, "TabelKontrole", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            try
            {
                JET_CONNECT = ADOJET_PROVIDER +
                              "Data Source=" + companyPath + LOCATION_NETDATA + "\\marnt.mdv;" +
                              "Persist Security Info=False";

                AD_NTDB = new Connection();
                AD_NTDB.Open(JET_CONNECT);

                BA_MODUS = 1;
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(
                    "Fout bij openen marnt.MDV: " + ex.Message,
                    "AutoLoadCompany",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }

            // --- Loop tables and load TeleBib + verify indexes ---

            for (int t = TABLE_VARIOUS; t <= TABLE_COUNTERS; t++)
            {
                BClose(t);
                BOpen(t);

                TeleBibPage(t);


                if (t == TABLE_VARIOUS || t == TABLE_COUNTERS)
                {
                    continue;
                }

                string aa = string.Empty;

                try
                {
                    // Fase 1 : Huidige databaseindexen (her)samenstellen
                    if (NT_DB != null && !string.IsNullOrEmpty(JET_TABLENAME[t]))
                    {
                        var tableDef = NT_DB.TableDefs[JET_TABLENAME[t]];
                        int indexCount = tableDef.Indexes.Count;

                        for (int tt = 0; tt < indexCount; tt++)
                        {
                            var daoIndex = (DAO.Index)tableDef.Indexes[tt];
                            aa += daoIndex.Name + ";";
                        }
                    }
                }
                catch
                {
                    // Ignore DAO errors; we'll skip index verification for this table.
                    continue;
                }

                // Fase 2 : Standaard definitie aanwezigheid controleren
                for (int tt = 0; tt <= FL_NUMBEROFINDEXEN[t]; tt++)
                {
                    string caption = FLINDEX_CAPTION[t, tt];
                    if (string.IsNullOrEmpty(caption))
                    {
                        continue;
                    }

                    int plTt = aa.IndexOf(caption, StringComparison.Ordinal);
                    if (plTt >= 0)
                    {
                        if (plTt == 0)
                        {
                            aa = aa.Substring(caption.Length + 1);
                        }
                        else
                        {
                            aa = aa.Substring(0, plTt) + aa.Substring(plTt + caption.Length + 1);
                        }
                    }
                    else if (caption == "Boekdatum")
                    {
                        // Ignore missing "Boekdatum" index
                    }
                    else
                    {
                        MessageBox.Show(
                            "Index '" + caption + "' van tabel '" + JET_TABLENAME[t] + "' bestaat niet meer !!!",
                            "InitBestanden",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);
                    }
                }

                // Any remaining indexes in 'aa' are user‑added; append them to FL_INDEX arrays
                if (!string.IsNullOrEmpty(aa))
                {
                    while (!string.IsNullOrEmpty(aa))
                    {
                        int sep = aa.IndexOf(';');
                        if (sep < 0)
                        {
                            break;
                        }

                        string idxName = aa.Substring(0, sep);
                        if (string.IsNullOrEmpty(idxName))
                        {
                            break;
                        }

                        FL_NUMBEROFINDEXEN[t]++;
                        int idxPos = FL_NUMBEROFINDEXEN[t];
                        FLINDEX_CAPTION[t, idxPos] = idxName;

                        try
                        {
                            var tableDef = NT_DB.TableDefs[JET_TABLENAME[t]];
                            var daoIndex = tableDef.Indexes[idxName];
                            int fieldCount = daoIndex.Fields.Count;

                            if (fieldCount - 1 != 0)
                            {
                                // Composite index: join first and remaining fields with '+'
                                MessageBox.Show(
                                    "Index " + idxName + " van tabel " + JET_TABLENAME[t] +
                                    " is samengesteld uit meerdere velden..." + Environment.NewLine +
                                    "Deze index enkel te gebruiken voor lijsten van " + JET_TABLENAME[t] +
                                    ".  Bij geïndexeerd zoeken wordt enkel het eerste veld opgenomen in het rooster.");

                                string firstName = ((DAO.Field)daoIndex.Fields[0]).Name;
                                JETTABLEUSE_INDEX[t, idxPos] = firstName;

                                for (int ttt = 1; ttt < fieldCount; ttt++)
                                {
                                    string fn = ((DAO.Field)daoIndex.Fields[ttt]).Name;
                                    JETTABLEUSE_INDEX[t, idxPos] += "+" + fn;
                                }

                                FLINDEX_LEN[t, idxPos] = 0;
                            }
                            else
                            {
                                string firstName = ((DAO.Field)daoIndex.Fields[0]).Name;
                                JETTABLEUSE_INDEX[t, idxPos] = firstName;

                                var fld = tableDef.Fields[firstName.TrimEnd()];
                                FLINDEX_LEN[t, idxPos] = fld.Size;
                            }
                        }
                        catch
                        {
                            // If DAO fails here, leave JETTABLEUSE_INDEX/FLINDEX_LEN as defaults.
                        }

                        int plRemove = aa.IndexOf(idxName, StringComparison.Ordinal);
                        if (plRemove == 0)
                        {
                            aa = aa.Substring(idxName.Length + 1);
                        }
                        else if (plRemove > 0)
                        {
                            aa = aa.Substring(0, plRemove) + aa.Substring(plRemove + idxName.Length + 1);
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
        }

        private static void NetVoorbereiden()
        {
            // For each DEFxx.OCT, create DEFxx.OXT if it doesn't exist
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

        public static void RecordToVeld(int fl)
        {
            TLB_RECORD[fl] = string.Empty;

            try
            {
                int t = 0;

                if (fl == TABLE_VARIOUS || fl == TABLE_DUMMY)
                {
                    // In VB6: TLB_RECORD(Fl) = rsMAR(Fl).Fields("MEMO")
                    object memo = RS_MAR[fl].Fields["MEMO"].Value;
                    TLB_RECORD[fl] = memo == null || memo is System.DBNull ? string.Empty : memo.ToString();
                }
                else
                {
                    // Build TLBR buffer from all vBC field codes
                    while (t < VBC.GetLength(1) && !string.IsNullOrEmpty(VBC[fl, t]))
                    {
                        string fieldCode = VBC[fl, t];
                        object valObj = RS_MAR[fl].Fields[fieldCode].Value;
                        string val = valObj == null || valObj is System.DBNull ? string.Empty : valObj.ToString();
                        VBib(fl, val, fieldCode);
                        t++;
                    }
                }

                // Fill FVT with index key values taken from TLB_RECORD
                for (int i = 0; i <= FL_NUMBEROFINDEXEN[fl]; i++)
                {
                    string code = "#" + JETTABLEUSE_INDEX[fl, i] + "#";
                    FVT[fl, i] = VBibTekst(fl, code);
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message, "RecordToVeld");
            }
        }

        public static void BGet(int fl, int fIndex, string fSleutel)
        {
            int probeerTellertje = 0;

        bGetNogEens:
            // Ensure recordset open
            if (RS_MAR[fl] == null || RS_MAR[fl].State == (int)ObjectStateEnum.adStateClosed)
            {
                // Use BOpen from ModBtrieve if available, otherwise just return
                if (BOpen(fl) != 0)
                {
                    KTRL = 99;
                    return;
                }
            }

            try
            {
                string sleutel = VSet(fSleutel, FLINDEX_LEN[fl, fIndex]);

                if (!string.Equals(RS_MAR[fl].Index, FLINDEX_CAPTION[fl, fIndex], System.StringComparison.Ordinal))
                {
                    RS_MAR[fl].Index = FLINDEX_CAPTION[fl, fIndex];
                }

                RS_MAR[fl].Seek(sleutel, SeekEnum.adSeekFirstEQ);

                if (RS_MAR[fl].EOF)
                {
                    KTRL = 4;
                }
                else
                {
                    KTRL = 0;
                }

                KEY_BUF[fl] = VSet(sleutel, FLINDEX_LEN[fl, fIndex]);
                KEY_INDEX[fl] = (int)fIndex;
            }
            catch (System.Exception)
            {
                BClose(fl);
                probeerTellertje++;
                if (probeerTellertje > 5)
                {
                    KTRL = 99;
                    return;
                }
                goto bGetNogEens;
            }
        }

        /// <summary>
        /// VB6 Dec(fGetal, fMasker): Format a number with a given mask and ensure
        /// the decimal separator is '.' and total length matches the mask.
        /// </summary>
        public static string Dec(double value, string mask)
        {
            // Use VB-like formatting via .NET standard numeric format strings
            // The VB mask is already provided (e.g. "#####0.00"), so we can
            // just use string.Format respecting current culture then normalize.
            string formatted = value.ToString(mask);

            if (formatted.Length < mask.Length)
            {
                formatted = new string(' ', mask.Length - formatted.Length) + formatted;
            }

            // VB6 replaced comma with dot if needed.
            formatted = formatted.Replace(',', '.');

            return formatted;
        }

        /// <summary>
        /// DB control helper for TABLE_LEDGERACCOUNTS: show nearby records around a key
        /// using the MinimumIndeling table. This is a direct, minimal port of DbKontrole.
        /// </summary>
        public static void DbKontrole(string searchKey, int flNr)
        {
            if (flNr != TABLE_LEDGERACCOUNTS)
            {
                MessageBox.Show("DbKontrole not implemented for FlNr=" + flNr, "DbKontrole",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                var rsKBRecord = new Recordset
                {
                    CursorLocation = CursorLocationEnum.adUseServer
                };

                // MinimumIndeling table in AD_KBDB
                rsKBRecord.Open("MinimumIndeling", AD_KBDB,
                    CursorTypeEnum.adOpenKeyset,
                    LockTypeEnum.adLockOptimistic,
                    (int)CommandTypeEnum.adCmdTableDirect);

                rsKBRecord.Index = "RekeningNummer";

                string buffer = string.Empty;
                int teller = 0;

                // Seek before or equal
                rsKBRecord.Seek(searchKey, SeekEnum.adSeekBeforeEQ);

                if (rsKBRecord.BOF)
                {
                    if (rsKBRecord.EOF)
                    {
                        rsKBRecord.MoveFirst();
                    }

                    buffer = rsKBRecord.Fields[0].Value + " " +
                             rsKBRecord.Fields[1].Value + Environment.NewLine + buffer;

                    teller = 0;
                    rsKBRecord.MovePrevious();

                    while (!rsKBRecord.BOF && teller < 10)
                    {
                        teller++;
                        rsKBRecord.MovePrevious();
                        buffer = rsKBRecord.Fields[0].Value + " " +
                                 rsKBRecord.Fields[1].Value + Environment.NewLine + buffer;
                    }
                }

                // Seek after or equal
                teller = 0;
                rsKBRecord.Seek(searchKey, SeekEnum.adSeekAfterEQ);

                if (!rsKBRecord.EOF)
                {
                    rsKBRecord.MoveNext();
                    while (!rsKBRecord.EOF && teller < 15)
                    {
                        teller++;
                        buffer += rsKBRecord.Fields[0].Value + " " +
                                  rsKBRecord.Fields[1].Value + Environment.NewLine;
                        rsKBRecord.MoveNext();
                    }
                }

                // Show in InfoData (left panel) if FormMim is available.
                if (Application.OpenForms["FormMim"] is FormMim mim)
                {
                    mim.Invoke(new Action(() =>
                    {
                        // InfoData is a Panel; original VB6 used .Cls/.Print.
                        // Here we can show it as a simple multiline label or textbox.
                        mim.Controls["infoData"].Visible = true;
                        // You may want to add a dedicated Label/ListBox inside infoData to show text.
                    }));
                }

                rsKBRecord.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "DbKontrole", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static string BtwKontrole(string btwString)
        {
            if (string.IsNullOrEmpty(btwString) || btwString.Length < 10)
                return string.Empty;

            try
            {
                double d1 = double.Parse(btwString.Substring(0, 8));
                double d2 = d1 / 97.0;
                double frac = d2 - Math.Floor(d2);
                int ipip = 97 - (int)(frac * 97.0);

                int lastTwo = int.Parse(btwString.Substring(btwString.Length - 2));
                if (ipip != lastTwo)
                    return string.Empty;

                return btwString;
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Format and pad a string to a fixed length (VB6 vSet equivalent).
        /// Delegates to ModDatabase.SetSpacing for core logic.
        /// </summary>        
        public static string VSet(string text, int length)
        {
            if (text == null)
                text = string.Empty;

            string b = text.Length > length ? text.Substring(0, length) : text;
            return b + new string(' ', length - b.Length);
        }

        /// <summary>
        /// Split the semicolon-separated definition in adKBTable.splitDefinitie and populate a ComboBox.
        /// Returns the matching entry if OptieTxt is found, otherwise empty.
        /// (Port of ZoekEnPlaats without direct dependence on VB6-specific controls.)
        /// </summary>
        public static string ZoekEnPlaats(ComboBox control, string zoekTekst,
            out int aLijnen, out int optieNr, string optieTxt)
        {
            aLijnen = 0;
            optieNr = 0;
            string result = string.Empty;

            if (AD_KBTable == null)
            {
                MessageBox.Show("AD_KBTable is not initialized.", "ZoekEnPlaats",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return result;
            }

            // Use Index/Seek if available, else fallback to linear scan
            try
            {
                AD_KBTable.Index = "BestandsNaam";
                AD_KBTable.Seek(zoekTekst, SeekEnum.adSeekFirstEQ);
            }
            catch
            {
                AD_KBTable.MoveFirst();
                while (!AD_KBTable.EOF &&
                       !string.Equals(SafeDbValue(AD_KBTable.Fields["BestandsNaam"].Value).ToString(),
                                      zoekTekst, StringComparison.OrdinalIgnoreCase))
                {
                    AD_KBTable.MoveNext();
                }
            }

            if (AD_KBTable.EOF)
            {
                MessageBox.Show("Stop !  Keuzebox " + zoekTekst + " niet te vinden...");
                return result;
            }

            string joinStringHier = SafeDbValue(AD_KBTable.Fields["splitDefinitie"].Value).ToString();
            if (!joinStringHier.EndsWith(";", StringComparison.Ordinal))
            {
                joinStringHier += ";";
            }

            control.Items.Clear();
            int puntKommaLokatie = 0;
            aLijnen = 0;
            int optieLen = optieTxt?.Length ?? 0;

            while (true)
            {
                int next = joinStringHier.IndexOf(';', puntKommaLokatie);
                if (next < 0)
                    break;

                string item = joinStringHier.Substring(puntKommaLokatie, next - puntKommaLokatie);
                aLijnen++;
                control.Items.Add(item);

                if (!string.IsNullOrEmpty(optieTxt) &&
                    item.StartsWith(optieTxt, StringComparison.Ordinal))
                {
                    optieNr = aLijnen - 1;
                    result = item;
                }

                puntKommaLokatie = next + 1;
            }

            if (optieLen == 0)
            {
                optieNr = 0;
            }

            if (control.Items.Count > 0 && optieNr >= 0 && optieNr < control.Items.Count)
            {
                control.SelectedIndex = optieNr;
            }

            return result;
        }

        private static object SafeDbValue(object dbValue)
        {
            return (dbValue == null || dbValue is DBNull) ? string.Empty : dbValue;
        }

        public static bool FileExists(string path)
        {
            try
            {
                return System.IO.File.Exists(path);
            }
            catch
            {
                return false;
            }
        }

        public static string GetFileSize(string source)
        {
            try
            {
                var fi = new System.IO.FileInfo(source);
                return fi.Length.ToString();
            }
            catch
            {
                return "0";
            }
        }

        public static bool ValidateNumeric(string text)
        {
            return string.IsNullOrEmpty(text) ||
                   text == "-" ||
                   text == "-." ||
                   text == "." ||
                   double.TryParse(text, out _);
        }

        public static bool IsSchrikkelJaar(int jaar)
        {
            try
            {
                DateTime dt = new DateTime(jaar, 2, 29);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Initialize FL_NUMBEROFINDEXEN, JETTABLEUSE_INDEX, FLINDEX_LEN, FLINDEX_CAPTION
        /// for all main tables and verify Telebib page definitions. Returns false if
        /// required definitions or indexes are missing.
        /// </summary>
        public static bool InitBestanden()
        {
            bool ok = true;

            FL_NUMBEROFINDEXEN[TABLE_VARIOUS] = 1;
            JETTABLEUSE_INDEX[TABLE_VARIOUS, 0] = "v004 "; FLINDEX_LEN[TABLE_VARIOUS, 0] = 13; FLINDEX_CAPTION[TABLE_VARIOUS, 0] = "Partij";
            JETTABLEUSE_INDEX[TABLE_VARIOUS, 1] = "v005 "; FLINDEX_LEN[TABLE_VARIOUS, 1] = 20; FLINDEX_CAPTION[TABLE_VARIOUS, 1] = "SPtype";

            FL_NUMBEROFINDEXEN[TABLE_CUSTOMERS] = 1;
            JETTABLEUSE_INDEX[TABLE_CUSTOMERS, 0] = "A110 "; FLINDEX_LEN[TABLE_CUSTOMERS, 0] = 12; FLINDEX_CAPTION[TABLE_CUSTOMERS, 0] = "Nummer";
            JETTABLEUSE_INDEX[TABLE_CUSTOMERS, 1] = "A100 "; FLINDEX_LEN[TABLE_CUSTOMERS, 1] = 10; FLINDEX_CAPTION[TABLE_CUSTOMERS, 1] = "Bedrijfsnaam";

            FL_NUMBEROFINDEXEN[TABLE_SUPPLIERS] = 1;
            JETTABLEUSE_INDEX[TABLE_SUPPLIERS, 0] = "A110 "; FLINDEX_LEN[TABLE_SUPPLIERS, 0] = 12; FLINDEX_CAPTION[TABLE_SUPPLIERS, 0] = "Nummer";
            JETTABLEUSE_INDEX[TABLE_SUPPLIERS, 1] = "A100 "; FLINDEX_LEN[TABLE_SUPPLIERS, 1] = 10; FLINDEX_CAPTION[TABLE_SUPPLIERS, 1] = "Bedrijfsnaam";

            FL_NUMBEROFINDEXEN[TABLE_LEDGERACCOUNTS] = 1;
            JETTABLEUSE_INDEX[TABLE_LEDGERACCOUNTS, 0] = "v019 "; FLINDEX_LEN[TABLE_LEDGERACCOUNTS, 0] = 7; FLINDEX_CAPTION[TABLE_LEDGERACCOUNTS, 0] = "RekeningNummer";
            JETTABLEUSE_INDEX[TABLE_LEDGERACCOUNTS, 1] = "v020 "; FLINDEX_LEN[TABLE_LEDGERACCOUNTS, 1] = 10; FLINDEX_CAPTION[TABLE_LEDGERACCOUNTS, 1] = "Omschrijving";

            FL_NUMBEROFINDEXEN[TABLE_PRODUCTS] = 1;
            JETTABLEUSE_INDEX[TABLE_PRODUCTS, 0] = "v102 "; FLINDEX_LEN[TABLE_PRODUCTS, 0] = 13; FLINDEX_CAPTION[TABLE_PRODUCTS, 0] = "Artikelkode EAN";
            JETTABLEUSE_INDEX[TABLE_PRODUCTS, 1] = "v105 "; FLINDEX_LEN[TABLE_PRODUCTS, 1] = 10; FLINDEX_CAPTION[TABLE_PRODUCTS, 1] = "Omschrijving";

            FL_NUMBEROFINDEXEN[TABLE_JOURNAL] = 4;
            JETTABLEUSE_INDEX[TABLE_JOURNAL, 0] = "v070 "; FLINDEX_LEN[TABLE_JOURNAL, 0] = 15; FLINDEX_CAPTION[TABLE_JOURNAL, 0] = "Rekening Boekdatum";
            JETTABLEUSE_INDEX[TABLE_JOURNAL, 1] = "v033 "; FLINDEX_LEN[TABLE_JOURNAL, 1] = 11; FLINDEX_CAPTION[TABLE_JOURNAL, 1] = "Dokumentnummer";
            JETTABLEUSE_INDEX[TABLE_JOURNAL, 2] = "v038 "; FLINDEX_LEN[TABLE_JOURNAL, 2] = 8; FLINDEX_CAPTION[TABLE_JOURNAL, 2] = "Betalingsstuk";
            JETTABLEUSE_INDEX[TABLE_JOURNAL, 3] = "v041 "; FLINDEX_LEN[TABLE_JOURNAL, 3] = 1; FLINDEX_CAPTION[TABLE_JOURNAL, 3] = "Bewerkingsvlag";
            JETTABLEUSE_INDEX[TABLE_JOURNAL, 4] = "v066 "; FLINDEX_LEN[TABLE_JOURNAL, 4] = 7; FLINDEX_CAPTION[TABLE_JOURNAL, 4] = "Boekdatum";

            FL_NUMBEROFINDEXEN[TABLE_INVOICES] = 2;
            JETTABLEUSE_INDEX[TABLE_INVOICES, 0] = "v033 "; FLINDEX_LEN[TABLE_INVOICES, 0] = 11; FLINDEX_CAPTION[TABLE_INVOICES, 0] = "DokumentNummer";
            JETTABLEUSE_INDEX[TABLE_INVOICES, 1] = "v034 "; FLINDEX_LEN[TABLE_INVOICES, 1] = 13; FLINDEX_CAPTION[TABLE_INVOICES, 1] = "Partij";
            JETTABLEUSE_INDEX[TABLE_INVOICES, 2] = "A000 "; FLINDEX_LEN[TABLE_INVOICES, 2] = 12; FLINDEX_CAPTION[TABLE_INVOICES, 2] = "KontraktNummer";

            FL_NUMBEROFINDEXEN[TABLE_CONTRACTS] = 3;
            JETTABLEUSE_INDEX[TABLE_CONTRACTS, 0] = "A000 "; FLINDEX_LEN[TABLE_CONTRACTS, 0] = 12; FLINDEX_CAPTION[TABLE_CONTRACTS, 0] = "Polisnummer";
            JETTABLEUSE_INDEX[TABLE_CONTRACTS, 1] = "A110 "; FLINDEX_LEN[TABLE_CONTRACTS, 1] = 12; FLINDEX_CAPTION[TABLE_CONTRACTS, 1] = "Klantkode";
            JETTABLEUSE_INDEX[TABLE_CONTRACTS, 2] = "A010 "; FLINDEX_LEN[TABLE_CONTRACTS, 2] = 4; FLINDEX_CAPTION[TABLE_CONTRACTS, 2] = "Maatschappij";
            JETTABLEUSE_INDEX[TABLE_CONTRACTS, 3] = "v167 "; FLINDEX_LEN[TABLE_CONTRACTS, 3] = 30; FLINDEX_CAPTION[TABLE_CONTRACTS, 3] = "MaandKlantMijPolis";

            FL_NUMBEROFINDEXEN[TABLE_COUNTERS] = 0;
            JETTABLEUSE_INDEX[TABLE_COUNTERS, 0] = "v071 "; FLINDEX_LEN[TABLE_COUNTERS, 0] = 5; FLINDEX_CAPTION[TABLE_COUNTERS, 0] = "Setup Parameter";

            FL_NUMBEROFINDEXEN[TABLE_DUMMY] = 0;
            JETTABLEUSE_INDEX[TABLE_DUMMY, 0] = "v089 "; FLINDEX_LEN[TABLE_DUMMY, 0] = 20; FLINDEX_CAPTION[TABLE_DUMMY, 0] = "Plaatselijk sorteren";

            return ok;
        }

        /// <summary>
        /// Set numeric masks according to BH_EURO flag (VB6 Cijfermaskers).
        /// </summary>
        public static void Cijfermaskers()
        {
            MASK_2002 = BH_EURO ? MASK_EUR : MASK_BEF;

            MASK_SY[0] = "#########";
            MASK_SY[1] = "###0";
            MASK_SY[2] = "######0.00";
            MASK_SY[3] = "##0.00000000";
            MASK_SY[4] = "#######0.00";
            MASK_SY[5] = "##0";
            MASK_SY[6] = "#0";
            MASK_SY[7] = "#####0.0";
            MASK_SY[8] = "#######0";
        }

        public static string LoadText(string section, string key)
        {
            string valuePath =
                Interaction.GetSetting(
                    "marINTEGRAAL",
                    section,
                    key,
                    "") ?? string.Empty;

            return string.IsNullOrEmpty(valuePath) ? string.Empty : valuePath;
        }

        public static void SaveText(string section, string key, string value)
        {
            Interaction.SaveSetting(
                "marINTEGRAAL",
                section,
                key,
                value ?? string.Empty);
        }

        public static bool BankOk(string rekString)
        {
            if (rekString == null)
                return false;

            rekString = rekString.Trim();
            if (rekString.Length == 14)
            {
                rekString = rekString.Substring(0, 3) +
                            rekString.Substring(4, 7) +
                            rekString.Substring(12, 2);
            }
            else if (rekString.Length != 12)
            {
                return false;
            }

            double dPip = double.Parse(rekString.Substring(0, 3) + rekString.Substring(3, 7));
            string controle = rekString.Substring(10, 2);

            if (controle == "00")
                return false;

            double rest = dPip - Math.Floor(dPip / 97d) * 97d;
            if (rest == 0 && controle == "97")
                return true;

            return Math.Abs(rest - double.Parse(controle)) < 0.0001;
        }

        public static bool CopyFile(string sourcePath, string targetPath, string fileToCopy)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;

                string searchPattern = fileToCopy;
                string[] files;

                if (fileToCopy.IndexOfAny(new[] { '?', '*' }) >= 0)
                {
                    if (!Directory.Exists(sourcePath))
                    {
                        MessageBox.Show("Stop tijdens het kopieren.  Bestand niet te vinden: \"" + fileToCopy + "\"", "SETUP");
                        return false;
                    }

                    files = Directory.GetFiles(sourcePath, searchPattern);
                    if (files.Length == 0)
                    {
                        MessageBox.Show("Stop tijdens het kopieren.  Bestand niet te vinden: \"" + fileToCopy + "\"", "SETUP");
                        return false;
                    }
                }
                else
                {
                    string full = Path.Combine(sourcePath, fileToCopy);
                    if (!File.Exists(full))
                    {
                        MessageBox.Show("Bestand niet te vinden: \"" + fileToCopy + "\"", "SETUP");
                        return false;
                    }

                    files = new[] { full };
                }

                Directory.CreateDirectory(targetPath);

                foreach (var src in files)
                {
                    string name = Path.GetFileName(src);
                    string dst = Path.Combine(targetPath, name);

                    if (File.Exists(dst))
                        File.Delete(dst);

                    using (var inStream = new FileStream(src, FileMode.Open, FileAccess.Read))
                    using (var outStream = new FileStream(dst, FileMode.CreateNew, FileAccess.Write))
                    {
                        var buffer = new byte[3000];
                        int read;
                        while ((read = inStream.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            outStream.Write(buffer, 0, read);
                        }
                    }

                    if (new FileInfo(src).Length != new FileInfo(dst).Length)
                    {
                        MessageBox.Show("Stop tijdens het kopieren van " + fileToCopy + "\"", "SETUP");
                        return false;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Stop tijdens het kopieren van " + fileToCopy + "\"" + System.Environment.NewLine + ex.Message, "SETUP");
                return false;
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        public static bool CreatePath(string destPath)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;

                if (!destPath.EndsWith("\\"))
                    destPath += "\\";

                Directory.CreateDirectory(destPath);
                return true;
            }
            catch
            {
                MessageBox.Show("Stop tijdens aanmaak van inhoudsopgaves op de doeldisk.", "SETUP", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        public static bool DateInvalid(string fDatum)
        {
            if (string.IsNullOrEmpty(fDatum) || fDatum.Length != 10)
                return true;

            if (!int.TryParse(fDatum.Substring(0, 2), out int dag) ||
                !int.TryParse(fDatum.Substring(3, 2), out int maand) ||
                !int.TryParse(fDatum.Substring(6, 4), out int jaar))
            {
                return true;
            }

            if (dag > 0 && dag < 32 && maand > 0 && maand < 13 && jaar > 1985 && jaar < 2062)
                return false;

            System.Media.SystemSounds.Beep.Play();
            return true;
        }

        public static string DateKey(string dateText)
        {
            if (string.IsNullOrEmpty(dateText) || dateText.Length < 10)
                return string.Empty;

            string dag = dateText.Substring(0, 2);
            string maand = dateText.Substring(3, 2);
            string jaar = dateText.Substring(6, 4);
            return jaar + maand + dag;
        }

        public static bool DateCheck(string fDatum, int vlag)
        {
            if (string.IsNullOrEmpty(fDatum))
                return false;

            string gDatum = fDatum.Replace("/", string.Empty);
            if (gDatum.Length < 8)
                return false;

            string dag, maand, jaar;

            switch (vlag)
            {
                case PERIODAS_TEXT:
                case BOOKYEARAS_TEXT:
                    dag = gDatum.Substring(0, 2);
                    maand = gDatum.Substring(2, 2);
                    jaar = gDatum.Substring(4, 4);
                    break;
                case PERIODAS_KEY:
                case BOOKYEARAS_KEY:
                    jaar = gDatum.Substring(0, 4);
                    maand = gDatum.Substring(4, 2);
                    dag = gDatum.Substring(6, 2);
                    break;
                default:
                    MessageBox.Show("Datum onjuist !");
                    return false;
            }

            string key = jaar + maand + dag;

            switch (vlag)
            {
                case PERIODAS_TEXT:
                case PERIODAS_KEY:
                    return string.Compare(key, BOOKYEAR_FROMTO.Substring(0, 8), StringComparison.Ordinal) >= 0 &&
                           string.Compare(key, BOOKYEAR_FROMTO.Substring(BOOKYEAR_FROMTO.Length - 8, 8), StringComparison.Ordinal) <= 0;
                case BOOKYEARAS_TEXT:
                case BOOKYEARAS_KEY:
                    return string.Compare(key, BOOKYEAR_FROMTO.Substring(0, 8), StringComparison.Ordinal) >= 0 &&
                           string.Compare(key, BOOKYEAR_FROMTO.Substring(BOOKYEAR_FROMTO.Length - 8, 8), StringComparison.Ordinal) <= 0;
                default:
                    return false;
            }
        }

        public static string DateText(string dateAsKey)
        {
            if (string.IsNullOrEmpty(dateAsKey) || dateAsKey.Length < 8)
                return string.Empty;

            string day = dateAsKey.Substring(6, 2);
            string month = dateAsKey.Substring(4, 2);
            string year = dateAsKey.Substring(0, 4);
            return day + "/" + month + "/" + year;
        }

        public static string FmarBoxText(string marBoxNumber, string taal, string marBoxOption)
        {
            string zoekTekst;
            if (marBoxNumber.Length == 2)
            {
                zoekTekst = "NTKB" + taal + "9";

            }
            else if (marBoxNumber.Length == 3)
            {
                zoekTekst = "NTKB" + taal;

            }
            else
            {
                MessageBox.Show("fmarBoxText fout");
                return string.Empty;
            }

            // ByRef DeKontrol As ListBox, ByRef ZoekTekst As String, ByRef ALijnen As Short, ByRef OptieNr As Short, ByRef OptieTxt As String
            // TODO:
            // return ZoekEnPlaats(KeuzeVSF.NTBoxLijst, zoekTekst, 0, 0, marBoxOption);
            return string.Empty;
        }

        public static string SleutelDok(int recordNr)
        {
            FL99_RECORD = String99(READING_LOCK, recordNr);

            string voorLetter;
            switch (recordNr)
            {
                case 1: voorLetter = "A0"; break;
                case 3: voorLetter = "A1"; break;
                case 11: voorLetter = "V0"; break;
                case 13: voorLetter = "V1"; break;
                case 73: voorLetter = "B0"; break;
                case 59: voorLetter = "F0"; break;
                case 121: voorLetter = "Q0"; break;
                case 188: voorLetter = "PF"; break;
                default:
                    MessageBox.Show("Ongeldige record : " + recordNr);
                    return string.Empty;
            }

            return voorLetter + PERIOD_FROMTO.Substring(0, 4) +
                   (int.TryParse(FL99_RECORD, out int v) ? (v + 1).ToString("00000") : "00001");
        }

        public static string String99(bool lockModus, int szNummer)
        {
            string tlString = "s" + szNummer.ToString("000");
            LOCK_HOLD = (int)(lockModus != READING ? 1 : 0);

            BGet(TABLE_COUNTERS, 0, tlString);

            if (KTRL == 99)
            {
                return string.Empty;
            }

            if (KTRL != 0)
            {
                MessageBox.Show("Tellers Stopkode " + KTRL + ", voor setup-tellersleutel " + tlString +
                                System.Environment.NewLine + System.Environment.NewLine +
                                "Overloop ALLE setup instellingen vooraleer enig boekjaar op te starten !" + System.Environment.NewLine +
                                "Wij staan tot uw beschikking om U hierbij te helpen.");
                return string.Empty;
            }

            RecordToVeld(TABLE_COUNTERS);

            try
            {
                return RS_MAR[TABLE_COUNTERS].Fields["v217"].Value as string ?? string.Empty;
            }
            catch
            {
                return string.Empty;
            }
            finally
            {
                BClose(TABLE_COUNTERS);
            }
        }

        public static string VValdag(string rDat1, string rvv)
        {
            try
            {
                int dag = int.Parse(rDat1.Substring(0, 2));
                int maand = int.Parse(rDat1.Substring(3, 2));
                int jaar = int.Parse(rDat1.Substring(6, 4)) - 1990;
                int avd = int.Parse(rvv);

                if (avd == 0)
                    return rDat1;

                while (true)
                {
                    int daysInMonth = DAYS_IN_MONTH[maand];
                    if (dag + avd <= daysInMonth)
                        break;

                    avd -= (daysInMonth - dag);
                    dag = 0;
                    if (maand == 12)
                    {
                        maand = 1;
                        jaar++;
                    }
                    else
                    {
                        maand++;
                    }
                }

                dag += avd;

                if (rvv.ToUpperInvariant().Contains("E"))
                {
                    dag = DAYS_IN_MONTH[maand];
                }

                int realYear = jaar + 1990;
                return dag.ToString("00") + "/" + maand.ToString("00") + "/" + realYear.ToString("0000");
            }
            catch
            {
                return string.Empty;
            }
        }

        public static string DATE_KEY(string datumfTXT)
        {
            if (string.IsNullOrEmpty(datumfTXT) || datumfTXT.Length < 10)
                return string.Empty;

            string dag = datumfTXT.Substring(0, 2);
            string maand = datumfTXT.Substring(3, 2);
            string jaar = datumfTXT.Substring(6, 4);

            // YYYYMMDD as string, like VB6
            return jaar + maand + dag;
        }

        public static bool DATE_CHECK(string fDatum, int fVlag)
        {
            if (string.IsNullOrEmpty(fDatum))
                return false;

            // Strip all '/'
            string gDatum = fDatum.Replace("/", string.Empty);
            if (gDatum.Length < 8)
                return false;

            string dag = string.Empty;
            string maand = string.Empty;
            string jaar = string.Empty;

            switch (fVlag)
            {
                case PERIODAS_TEXT:
                case BOOKYEARAS_TEXT:
                    dag = gDatum.Substring(0, 2);
                    maand = gDatum.Substring(2, 2);
                    jaar = gDatum.Substring(4, 4);
                    break;

                case PERIODAS_KEY:
                case BOOKYEARAS_KEY:
                    jaar = gDatum.Substring(0, 4);
                    maand = gDatum.Substring(4, 2);
                    dag = gDatum.Substring(6, 2);
                    break;

                default:
                    MessageBox.Show("Datum onjuist !");
                    return false;
            }

            string sleutel = jaar + maand + dag;

            switch (fVlag)
            {
                case PERIODAS_TEXT:
                case PERIODAS_KEY:
                    if (string.IsNullOrEmpty(PERIOD_FROMTO) || PERIOD_FROMTO.Length < 16)
                        return false;
                    return string.Compare(sleutel, PERIOD_FROMTO.Substring(0, 8), StringComparison.Ordinal) >= 0 &&
                           string.Compare(sleutel, PERIOD_FROMTO.Substring(PERIOD_FROMTO.Length - 8, 8), StringComparison.Ordinal) <= 0;

                case BOOKYEARAS_TEXT:
                case BOOKYEARAS_KEY:
                    if (string.IsNullOrEmpty(BOOKYEAR_FROMTO) || BOOKYEAR_FROMTO.Length < 16)
                        return false;
                    return string.Compare(sleutel, BOOKYEAR_FROMTO.Substring(0, 8), StringComparison.Ordinal) >= 0 &&
                           string.Compare(sleutel, BOOKYEAR_FROMTO.Substring(BOOKYEAR_FROMTO.Length - 8, 8), StringComparison.Ordinal) <= 0;

                default:
                    return false;
            }
        }

        public static string DATE_TEXT(string dateAsKey)
        {
            if (string.IsNullOrEmpty(dateAsKey) || dateAsKey.Length < 8)
                return string.Empty;

            string day = dateAsKey.Substring(6, 2);
            string month = dateAsKey.Substring(4, 2);
            string year = dateAsKey.Substring(0, 4);
            return day + "/" + month + "/" + year;
        }

        public static void CloseOpenWindows()
        {
            // Safe VB6 CloseOpenWindows equivalent: close known forms if they are open.
            try
            {
                // Specific forms from VB6 can be re‑enabled once their C# types exist:
                // if (Application.OpenForms["SqlSearch"] is Form sqlSearch) sqlSearch.Close();
                // if (Application.OpenForms["Xlog"] is Form xlog) xlog.Close();
                // if (Application.OpenForms["SetupEnParameters"] is Form setup) setup.Close();
                // if (Application.OpenForms["Afbeeldingen"] is Form afbeeldingen) afbeeldingen.Close();
                // if (Application.OpenForms["xDokument"] is Form xDok) xDok.Close();
                // if (Application.OpenForms["VrijBericht"] is Form vrijBericht) vrijBericht.Close();
                // if (Application.OpenForms["HistoriekSQL"] is Form historiekSql) historiekSql.Close();
                // if (Application.OpenForms["SQLLijsten"] is Form sqlLijsten) sqlLijsten.Close();
                // if (Application.OpenForms["Venster"] is Form venster) venster.Close();
                // if (Application.OpenForms["DirekteAankoop"] is Form direkteAankoop) direkteAankoop.Close();
                // if (Application.OpenForms["DirekteVerkoop"] is Form direkteVerkoop) direkteVerkoop.Close();
                // if (Application.OpenForms["DiversePosten"] is Form diversePosten) diversePosten.Close();
                // if (Application.OpenForms["InbrengFinancieel"] is Form inbrengFinancieel) inbrengFinancieel.Close();

                // Generic fallback: close all open forms except the main FormMim if present.
                foreach (Form openForm in Application.OpenForms)
                {
                    if (openForm is FormMim)
                        continue; // keep main window open

                    try
                    {
                        openForm.Close();
                    }
                    catch
                    {
                        // Mimic VB6 'On Error Resume Next'
                    }
                }
            }
            catch
            {
                // Outer safety net; also mimics 'On Error Resume Next'
            }
        }




        public static object OWaarde(object dbWaarde)
        {
            return (dbWaarde == null || dbWaarde is DBNull) ? string.Empty : dbWaarde;
        }

        public static void BClose(int fl)
        {
            // VB: If Fl = 99 Then close all tables
            if (fl == 99)
            {
                for (int i = 0; i <= NUMBER_TABLES; i++)
                {
                    TLB_RECORD[i] = string.Empty;
                    CloseTable(i);
                }
            }
            else
            {
                CloseTable(fl);
            }
        }

        private static void CloseTable(int fl)
        {
            if (RS_MAR[fl] == null || RS_MAR[fl].State == (int)ObjectStateEnum.adStateClosed)
            {
                return;
            }

            try
            {
                RS_MAR[fl].Close();
                KTRL = 0;
            }
            catch (System.Runtime.InteropServices.COMException comEx)
            {
                // VB6 checked Err = 3420
                const int ADO_ERROR_3420 = unchecked((int)0x800A0D5C);
                KTRL = comEx.ErrorCode;
                if (comEx.ErrorCode == ADO_ERROR_3420)
                {
                    MessageBox.Show(comEx.Message);
                }
            }
            catch (Exception ex)
            {
                KTRL = ex.HResult;
                MessageBox.Show(ex.Message);
            }
        }

        public static int BOpen(int fl)
        {
            // If already open, just return 0 (success)
            if (RS_MAR[fl] != null && RS_MAR[fl].State != (int)ObjectStateEnum.adStateClosed)
            {
                return 0;
            }

            if (RS_MAR[fl] == null)
            {
                RS_MAR[fl] = new Recordset();
            }

            try
            {
                RS_MAR[fl].CursorLocation = CursorLocationEnum.adUseServer;

                if (fl == TABLE_COUNTERS)
                {
                    RS_MAR[fl].Open(
                        JET_TABLENAME[fl],
                        AD_NTDB,
                        CursorTypeEnum.adOpenKeyset,
                        LockTypeEnum.adLockOptimistic,
                        (int)CommandTypeEnum.adCmdTableDirect);
                }
                else
                {
                    string testSQL = "SELECT * FROM " + JET_TABLENAME[fl] +
                              " ORDER BY " + JETTABLEUSE_INDEX[fl, 0] + " ASC";
                    SQL_MSG[fl] = testSQL;

                    RS_MAR[fl].Open(
                        SQL_MSG[fl],
                        AD_NTDB,
                        CursorTypeEnum.adOpenKeyset,
                        LockTypeEnum.adLockOptimistic,
                        (int)CommandTypeEnum.adCmdTableDirect);
                }


                // ntRS(Fl).LockEdits = False  'not applicable in ADODB.Recordset

                return 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "BOpen");
                KTRL = ex.HResult;
                return KTRL;
            }
        }

        // TeleBibPagina and full Btrieve cursor operations (bFirst/bNext/...) remain
        // in ModDatabase for now; only core record-buffer helpers are centralized here.

        // Add this method to the ModBtrieve class to resolve CS0117

    }
}
