using System;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Windows.Forms;
using ADODB;
using static mar2026.Classes.ModLibs;

namespace mar2026.Classes
{
    public static class ModDatabase
    {
        // This was 'Option Strict Off' in VB, so we keep many things as 'object' or dynamic-like.
        // You must provide all of these globals somewhere (they came from VB module scope).

        // Example of external/global dependencies you already have in VB:


        // Forms
        // public static FormBoxList FormBoxList;
        // public static FormBookingControl FormBookingControl;

        // --- VB helpers reimplemented ---

        private static string Left(string s, int len) =>
            s == null ? string.Empty : (s.Length <= len ? s : s.Substring(0, len));

        private static string Right(string s, int len) =>
            s == null ? string.Empty : (s.Length <= len ? s : s.Substring(s.Length - len));

        private static string Mid(string s, int start, int length)
        {
            if (s == null) return string.Empty;
            if (start < 1) start = 1;
            start--; // VB is 1-based
            if (start >= s.Length) return string.Empty;
            if (start + length > s.Length) length = s.Length - start;
            return s.Substring(start, length);
        }

        private static string Mid(string s, int start)
        {
            if (s == null) return string.Empty;
            if (start < 1) start = 1;
            start--;
            if (start >= s.Length) return string.Empty;
            return s.Substring(start);
        }

        private static void MidAssign(ref string s, int start, int length, string value)
        {
            if (s == null) s = string.Empty;
            if (start < 1) start = 1;
            start--; // 0-based
            if (start > s.Length) start = s.Length;
            if (start + length > s.Length) length = s.Length - start;
            s = s.Remove(start, length).Insert(start, value);
        }

        private static int InStr(int start, string s, string find)
        {
            if (s == null || find == null) return 0;
            if (start < 1) start = 1;
            int idx = s.IndexOf(find, start - 1, StringComparison.Ordinal);
            return idx < 0 ? 0 : idx + 1; // VB is 1-based
        }

        private static int InStr(string s, string find) => InStr(1, s, find);

        private static string Space(int n) => n <= 0 ? string.Empty : new string(' ', n);

        private static string RTrim(string s) =>
            s == null ? string.Empty : s.TrimEnd();

        private static int Asc(char c) => (int)c;

        // --- Extension: ADODB.Recordset -> DataTable ---

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
                // restrictions: 4-length array; index 3 is table type
                var restrictions = new string[4];
                restrictions[3] = objectType;

                cnn.Open();
                DataTable userTables = cnn.GetSchema("Tables", restrictions);
                cnn.Close();

                string returnString = string.Empty;
                for (int i = 0; i < userTables.Rows.Count; i++)
                {
                    returnString += userTables.Rows[i][2].ToString() + Environment.NewLine;
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
            catch (Exception ex)
            {
                KTRL = -1;
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
            catch (Exception ex)
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
            catch (Exception ex)
            {
                KTRL = -1;
                MessageBox.Show(ex.Message);
            }
        }

        public static bool AdoNewRecord(int fl)
        {
            TLB_RECORD[fl] = string.Empty;
            switch (fl)
            {
                case TABLE_CUSTOMERS:
                case TABLE_SUPPLIERS:
                    AdoInsertToRecord((int)fl, "2", "A10C");   // Taalkode
                    AdoInsertToRecord((int)fl, "002", "v149"); // Landnummer ISO
                    AdoInsertToRecord((int)fl, "B  ", "A109"); // Landkode Postkantoor
                    AdoInsertToRecord((int)fl, "BE", "v150");  // Landkode ISO
                    AdoInsertToRecord((int)fl, "EUR", "vs03"); // Munteenheid ISO
                    AdoInsertToRecord((int)fl, "1", "vs07");   // exemplaren
                    break;
                case TABLE_LEDGERACCOUNTS:
                    AdoInsertToRecord((int)fl, "O", "v032");   // Budgetcode
                    break;
            }
            return true;
        }

        public static string SetSpacing(string fTekst, int fLengte)
        {
            string b = Left(fTekst ?? string.Empty, fLengte);
            return b + Space(fLengte - b.Length);
        }

        public static void AdoInsertToRecord(int fl, string fieldString1, string fieldString2)
        {
            int TBLen;
            int TBStart;
            int TBStop;
            string TBCode;

            TBCode = "#     #";
            MidAssign(ref TBCode, 2, 5, fieldString2);

            if (fieldString1 == string.Empty)
            {
                fieldString1 = " ";
            }

        jump:
            if (InStr(TLB_RECORD[fl], TBCode) == 0)
            {
                TLB_RECORD[fl] = TLB_RECORD[fl] + TBCode + fieldString1 + "#";
            }
            else
            {
                if (RTrim(AdoGetField(fl, TBCode)) == fieldString1)
                {
                    return;
                }
                TBLen = (int)TLB_RECORD[fl].Length;
                TBStart = (int)InStr(TLB_RECORD[fl], TBCode);
                TBStop = (int)InStr(TBStart + 7, TLB_RECORD[fl], "#");
                TLB_RECORD[fl] = Left(TLB_RECORD[fl], TBStart - 1) +
                                 Right(TLB_RECORD[fl], TBLen - TBStop);
                goto jump;
            }
        }

        public static string AdoGetField(int fl, string tbs)
        {
            if (RS_MAR[fl].RecordCount == 0)
            {
                return "";
            }

            try
            {

                string fieldValue = RS_MAR[fl].Fields[tbs].Value.ToString();

                if (!Convert.IsDBNull(RS_MAR[fl].Fields[tbs].Value))
                {
                    fieldValue = RS_MAR[fl].Fields[tbs].Value.ToString();
                }
                else
                {
                    fieldValue = "";
                }
                return fieldValue;
            }
            catch (Exception)
            {
                return "";
            }
        }

        public static object XrsMar(int fl, string tbs)
        {
            var value = RS_MAR[fl].Fields[tbs].Value;
            if (value == null || Convert.IsDBNull(value))
                return string.Empty;
            return value;
        }

        public static object ObjectValue(object dbWaarde)
        {
            if (dbWaarde == null || Convert.IsDBNull(dbWaarde))
                return string.Empty;
            return dbWaarde;
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

        public static int EditIsPossible(int fl)
        {
            // Original VB returned True immediately and never used the rest.
            return 1;
        }

        public static void JetTableClose(int fl)
        {
            if (fl == 99)
            {
                for (int i = 0; i <= NUMBER_TABLES; i++)
                {
                    TLB_RECORD[i] = string.Empty;
                    if (RS_MAR[i].State != (int)ObjectStateEnum.adStateClosed)
                    {
                        try
                        {
                            if (RS_MAR[i].EditMode != (int)EditModeEnum.adEditNone)
                                RS_MAR[i].CancelUpdate();
                            RS_MAR[i].Close();
                        }
                        catch (Exception ex)
                        {
                            if (ex is System.Runtime.InteropServices.COMException comEx &&
                                comEx.ErrorCode == unchecked((int)0x800A0D5C)) // 3420 equivalent
                            {
                                MessageBox.Show(ex.Message);
                            }
                        }
                    }
                }
            }
            else
            {
                if (RS_MAR[fl].State != (int)ObjectStateEnum.adStateClosed)
                {
                    try
                    {
                        if (RS_MAR[fl].EditMode != (int)EditModeEnum.adEditNone)
                            RS_MAR[fl].CancelUpdate();
                        RS_MAR[fl].Close();
                    }
                    catch (Exception ex)
                    {
                        if (ex is System.Runtime.InteropServices.COMException comEx &&
                            comEx.ErrorCode == unchecked((int)0x800A0D5C))
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                }
            }
        }

        public static void Bdelete(int fl)
        {
            KTRL = 0;

            try
            {
                RS_MAR[fl].Delete();
                KTRL = 0;
            }
            catch (Exception ex)
            {
                KTRL = -1;
                MessageBox.Show(ex.Message);
            }
        }

        public static void JetGetAll(int fl, int fIndex, bool moveFirstLast)
        {
            JetTableClose(fl);

            string testSQL = "SELECT * FROM " + JET_TABLENAME[fl] +
                          " ORDER BY " + JETTABLEUSE_INDEX[fl, fIndex] + " ASC";
            SQL_MSG[fl] = SQL_MSG[fl] = testSQL;

            KTRL = 0;
            JetTableOpen(fl);

            if (RS_MAR[fl].EOF || RS_MAR[fl].RecordCount == -1)
            {
                MessageBox.Show("Nothing found");
            }
            else if (RS_MAR[fl].RecordCount > 0)
            {
                KEY_INDEX[fl] = fIndex;
                if (moveFirstLast)
                {
                    RS_MAR[fl].MoveFirst();
                }
                else
                {
                    RS_MAR[fl].MoveLast();
                }
            }
        }

        public static void JetGetFirst(int fl, int fIndex)
        {
            try
            {
                int numberRecords = RS_MAR[fl].RecordCount;
                if (numberRecords > 0)
                {
                    RS_MAR[fl].MoveFirst();
                    KEY_INDEX[fl] = fIndex;
                }
            }
            catch (Exception)
            {
                try
                {
                    JetTableClose(fl);

                    string testSQL = "SELECT * FROM " + JET_TABLENAME[fl] +
                                  " ORDER BY " + JETTABLEUSE_INDEX[fl, fIndex] + " ASC";
                    SQL_MSG[fl] = SQL_MSG[fl] = testSQL;

                    KTRL = 0;
                    JetTableOpen(fl);

                    if (RS_MAR[fl].EOF || RS_MAR[fl].RecordCount == -1)
                    {
                        MessageBox.Show("Stop");
                    }
                    else
                    {
                        RS_MAR[fl].MoveFirst();
                        KEY_INDEX[fl] = fIndex;
                    }

                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        public static void JetGetLast(int fl, int fIndex)
        {
            try
            {
                int numberRecords = RS_MAR[fl].RecordCount;
                if (numberRecords > 0)
                {
                    RS_MAR[fl].MoveLast();
                    KEY_INDEX[fl] = fIndex;
                }
            }
            catch (Exception)
            {
                try
                {
                    JetTableClose(fl);

                    string testSQL = "SELECT * FROM " + JET_TABLENAME[fl] +
                                  " ORDER BY " + JETTABLEUSE_INDEX[fl, fIndex] + " ASC";
                    SQL_MSG[fl] = SQL_MSG[fl] = testSQL;

                    KTRL = 0;
                    JetTableOpen(fl);

                    if (RS_MAR[fl].EOF || RS_MAR[fl].RecordCount == -1)
                    {
                        MessageBox.Show("Stop");
                    }
                    else
                    {
                        RS_MAR[fl].MoveLast();
                        KEY_INDEX[fl] = fIndex;
                    }

                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        public static void JetGetNext(int fl, int fIndex)
        {
            try
            {
                int numberRecords = RS_MAR[fl].RecordCount;
                if (numberRecords > 0)
                {
                    if (!RS_MAR[fl].EOF)
                    {
                        RS_MAR[fl].MoveNext();
                        KEY_INDEX[fl] = fIndex;
                    }
                }
            }
            catch (Exception)
            {
                try
                {
                    JetTableClose(fl);

                    string testSQL = "SELECT * FROM " + JET_TABLENAME[fl] +
                                  " ORDER BY " + JETTABLEUSE_INDEX[fl, fIndex] + " ASC";
                    SQL_MSG[fl] = SQL_MSG[fl] = testSQL;

                    KTRL = 0;
                    JetTableOpen(fl);

                    if (RS_MAR[fl].EOF || RS_MAR[fl].RecordCount == -1)
                    {
                        MessageBox.Show("Stop");
                    }
                    else
                    {
                        RS_MAR[fl].MoveFirst();
                        KEY_INDEX[fl] = fIndex;
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        public static void JetGetPrev(int fl, int fIndex)
        {
            try
            {
                int numberRecords = RS_MAR[fl].RecordCount;
                if (numberRecords > 0)
                {
                    if (!RS_MAR[fl].BOF)
                    {
                        RS_MAR[fl].MovePrevious();
                        KEY_INDEX[fl] = fIndex;
                    }
                }
            }
            catch (Exception)
            {
                try
                {
                    JetTableClose(fl);

                    string testSQL = "SELECT * FROM " + JET_TABLENAME[fl] +
                                  " ORDER BY " + JETTABLEUSE_INDEX[fl, fIndex] + " ASC";
                    SQL_MSG[fl] = SQL_MSG[fl] = testSQL;

                    KTRL = 0;
                    JetTableOpen(fl);

                    if (RS_MAR[fl].EOF || RS_MAR[fl].RecordCount == -1)
                    {
                        MessageBox.Show("Stop");
                    }
                    else
                    {
                        RS_MAR[fl].MoveFirst();
                        KEY_INDEX[fl] = fIndex;
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        public static void JetGet(int fl, int fIndex, ref string fSleutel)
        {
            JetTableClose(fl);

            fSleutel = SetSpacing(fSleutel, FLINDEX_LEN[fl, fIndex]);
            SQL_MSG[fl] = "SELECT * FROM " + JET_TABLENAME[fl] +
                          " WHERE " + JETTABLEUSE_INDEX[fl, fIndex] +
                          "='" + fSleutel + "'";
            KTRL = 0;
            JetTableOpen(fl);

            if (RS_MAR[fl].EOF)
            {
                KTRL = 9;
            }
            else if (RS_MAR[fl].RecordCount == -1)
            {
                MessageBox.Show("Stop");
            }
            else if (RS_MAR[fl].RecordCount == 1)
            {
                KEY_BUF[fl] = SetSpacing(fSleutel, FLINDEX_LEN[fl, fIndex]);
                KEY_INDEX[fl] = fIndex;
            }
            else if (RS_MAR[fl].RecordCount > 1)
            {
                MessageBox.Show("Stop");
            }
        }

        public static void JetGetOrGreater(int fl, int fIndex, ref string fKey)
        {
        TryAgain:
            if (RS_MAR[fl].State == (int)ObjectStateEnum.adStateClosed)
            {
                KTRL = JetTableOpen(fl);
            }


            fKey = SetSpacing(fKey, FLINDEX_LEN[fl, fIndex]);

            try
            {
                if ((string)RS_MAR[fl].Index != FLINDEX_CAPTION[fl, fIndex])
                {
                    RS_MAR[fl].Index = FLINDEX_CAPTION[fl, fIndex];
                }
            }
            catch (System.Runtime.InteropServices.COMException comEx)
            {
                if (comEx.ErrorCode == unchecked((int)0x80040E21)) // -2147217883
                {
                    JetTableClose(fl);
                    Application.DoEvents();
                    goto TryAgain;
                }
            }

            RS_MAR[fl].Seek(fKey, SeekEnum.adSeekAfterEQ);

            KTRL = RS_MAR[fl].EOF ? (int)4 : (int)0;
            KEY_BUF[fl] = (string)RS_MAR[fl].Fields[RTrim(JETTABLEUSE_INDEX[fl, fIndex])].Value;
            KEY_INDEX[fl] = fIndex;
        }

        public static void JetInsert(int fl, int fIndex)
        {
            if (fl != TABLE_INVOICES)
            {
                if (RS_MAR[fl].State == (int)ObjectStateEnum.adStateClosed)
                {
                    KTRL = JetTableOpen(fl);
                }
                RS_MAR[fl].AddNew();
            }

            var xxxxx = FieldToRecord(fl);
            if (KTRL == 32000) return;

            KEY_INDEX[fl] = fIndex;
            KEY_BUF[fl] = FVT[fl, fIndex];

            if (fl == TABLE_JOURNAL)
            {
                DKTRL_CUMUL += Convert.ToDecimal(RS_MAR[TABLE_JOURNAL].Fields["v068"].Value);
                if (BH_EURO)
                {
                    DKTRL_BEF += Math.Round(Convert.ToDecimal(RS_MAR[TABLE_JOURNAL].Fields["v068"].Value) * (decimal)EURO, 0);
                    DKTRL_EUR += Math.Round(Convert.ToDecimal(RS_MAR[TABLE_JOURNAL].Fields["v068"].Value), 2);
                    try
                    {
                        RS_MAR[TABLE_JOURNAL].Fields["dece068"].Value =
                            Convert.ToDecimal(RS_MAR[TABLE_JOURNAL].Fields["v068"].Value);
                    }
                    catch
                    {
                        // ignore
                    }
                }
                else
                {
                    DKTRL_BEF += Math.Round(Convert.ToDecimal(RS_MAR[TABLE_JOURNAL].Fields["v068"].Value), 0);
                    DKTRL_EUR += Math.Round(Convert.ToDecimal(RS_MAR[TABLE_JOURNAL].Fields["v068"].Value) / (decimal)EURO, 2);
                }

                // Original VB built 'JustTry' and added to a grid; left out here.
                System.Diagnostics.Debugger.Break();
            }

            try
            {
                KTRL = 0;
                RS_MAR[fl].Update();
            }
            catch (System.Runtime.InteropServices.COMException comEx)
            {
                if (comEx.ErrorCode == unchecked((int)0x80040E21) && comEx.Message.Contains("3022"))
                {
                    MessageBox.Show("Unieke sleutel reeds aanwezig in bestand : " +
                                    JET_TABLENAME[fl] + Environment.NewLine + Environment.NewLine +
                                    "Mogelijke sleutel : " + FVT[fl, fIndex]);
                    KTRL = 3022;
                }
                else
                {
                    try
                    {
                        var fieldVal = RS_MAR[fl].Fields[fIndex].Value;
                        if (Convert.IsDBNull(fieldVal))
                        {
                            MessageBox.Show(comEx.Message + Environment.NewLine + Environment.NewLine +
                                            "TABLEDEF_ONT : " + JET_TABLENAME[fl] + Environment.NewLine + Environment.NewLine +
                                            "De sleutel heeft 'null' waarde");
                        }
                        else
                        {
                            MessageBox.Show(comEx.Message + Environment.NewLine + Environment.NewLine +
                                            "TABLEDEF_ONT : " + JET_TABLENAME[fl] + Environment.NewLine + Environment.NewLine +
                                            "Mogelijke sleutel : " + FVT[fl, fIndex]);

                        }
                    }
                    catch
                    {
                        MessageBox.Show(comEx.Message);
                    }
                    KTRL = -1;
                }
                return;
            }

            if (fl == TABLE_JOURNAL)
            {
                if (KTRL != 0)
                {
                    MessageBox.Show("JetInsert journaal stopkode " + KTRL);
                    return;
                }

                string unsafeKey = FVT[TABLE_JOURNAL, 0].Substring(0, 7);

                JetGet(TABLE_LEDGERACCOUNTS, 0, ref unsafeKey);
                if (KTRL != 0)
                {
                    MessageBox.Show("Rekening " + Left(FVT[TABLE_JOURNAL, 0], 7) +
                                    " niet te vinden." + Environment.NewLine +
                                    "Eerst SETUPrekening inbrengen a.u.b. !");
                    DKTRL_CUMUL += 99;
                    return;
                }

                if (ACTIVE_BOOKYEAR == 0)
                {
                    RecordToField(TABLE_LEDGERACCOUNTS);
                    AdoInsertToRecord(TABLE_LEDGERACCOUNTS,
                        (Convert.ToDecimal(AdoGetField(TABLE_LEDGERACCOUNTS, "#e023 #")) +
                         Convert.ToDecimal(AdoGetField(TABLE_JOURNAL, "#v068 #"))).ToString(),
                        "e023");
                    RS_MAR[TABLE_LEDGERACCOUNTS].Fields["dece023"].Value =
                        Convert.ToDecimal(RS_MAR[TABLE_LEDGERACCOUNTS].Fields["dece023"].Value) +
                        Convert.ToDecimal(RS_MAR[TABLE_JOURNAL].Fields["dece068"].Value);
                }
                else
                {
                    RecordToField(TABLE_LEDGERACCOUNTS);
                    AdoInsertToRecord(TABLE_LEDGERACCOUNTS,
                        (Convert.ToDecimal(AdoGetField(TABLE_LEDGERACCOUNTS, "#e022 #")) +
                         Convert.ToDecimal(AdoGetField(TABLE_JOURNAL, "#v068 #"))).ToString(),
                        "e022");
                    RS_MAR[TABLE_LEDGERACCOUNTS].Fields["dece022"].Value =
                        Convert.ToDecimal(RS_MAR[TABLE_LEDGERACCOUNTS].Fields["dece022"].Value) +
                        Convert.ToDecimal(RS_MAR[TABLE_JOURNAL].Fields["dece068"].Value);
                }
                JetUpdate(TABLE_LEDGERACCOUNTS, 0);
            }

            switch (KTRL)
            {
                case 0:
                    break;
                case 5:
                    string msg = "Dergelijke ID.Kode Bestaat reeds : " + KEY_BUF[fl] + " : " + fl;
                    MessageBox.Show(msg);
                    break;
                case 46:
                    string msg2 = "TABLEDEF_ONT werd geopend in READING-modus." + Environment.NewLine +
                                  "Schrijven is niet mogelijk...";
                    MessageBox.Show(msg2, "Database beveiliging");
                    break;
                default:
                    string msg3 = "Stopkode " + KTRL + " tijdens invoegen nieuwe record.";
                    MessageBox.Show(msg3);
                    break;
            }
        }

        private static string UnsafeLeft(string src, int len)
        {
            // Helper to pass by ref same variable as VB's Left(FVT(TABLE_JOURNAL, 0), 7)
            // Caller will ignore returned string except for search, so we keep it simple.
            return Left(src ?? string.Empty, len);
        }

        public static void JetLast(int fl, int fIndex)
        {
            JetTableClose(fl);

            SQL_MSG[fl] = "SELECT TOP 1 * FROM " + JET_TABLENAME[fl] +
                          " ORDER BY " + JETTABLEUSE_INDEX[fl, fIndex] + " DESC";
            KTRL = 0;
            JetTableOpen(fl);

            if (RS_MAR[fl].EOF || RS_MAR[fl].RecordCount == -1)
            {
                System.Diagnostics.Debugger.Break();
            }
            else if (RS_MAR[fl].RecordCount == 1)
            {
                KEY_INDEX[fl] = fIndex;
            }
            else if (RS_MAR[fl].RecordCount > 1)
            {
                System.Diagnostics.Debugger.Break();
            }
        }

        public static void JetNext(int fl, int fIndex, string keyBefore)
        {
            JetTableClose(fl);

            SQL_MSG[fl] = "SELECT TOP 1 * FROM " + JET_TABLENAME[fl] +
                          " WHERE " + JETTABLEUSE_INDEX[fl, fIndex] + " > '" + keyBefore + "'" +
                          " ORDER BY " + JETTABLEUSE_INDEX[fl, fIndex] + " ASC";
            KTRL = 0;
            JetTableOpen(fl);

            if (RS_MAR[fl].EOF || RS_MAR[fl].RecordCount == -1)
            {
                KTRL = 9;
            }
            else if (RS_MAR[fl].RecordCount == 1)
            {
                KEY_BUF[fl] = string.Empty;
                KEY_INDEX[fl] = fIndex;
            }
            else if (RS_MAR[fl].RecordCount > 1)
            {
                MessageBox.Show("Stop meer dan een record!");
            }
        }

        public static int JetTableOpen(int fl)
        {
            if (RS_MAR[fl].State != (int)ObjectStateEnum.adStateClosed)
                return 0;

            try
            {
                RS_MAR[fl].CursorLocation = CursorLocationEnum.adUseClient;

                if (fl == TABLE_COUNTERS)
                {
                    RS_MAR[fl].Open(SQL_MSG[fl], AD_NTDB);
                }
                else
                {
                    RS_MAR[fl].Open(SQL_MSG[fl], AD_NTDB,
                        CursorTypeEnum.adOpenKeyset,
                        LockTypeEnum.adLockOptimistic,
                        (int)CommandTypeEnum.adCmdTableDirect);
                }

                return 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                KTRL = -1;
                return -1;
            }
        }

        public static void JetPrev(int fl, int fIndex, string keyBefore)
        {
            JetTableClose(fl);

            SQL_MSG[fl] = "SELECT TOP 1 * FROM " + JET_TABLENAME[fl] +
                          " WHERE " + JETTABLEUSE_INDEX[fl, fIndex] + " < '" + keyBefore + "'" +
                          " ORDER BY " + JETTABLEUSE_INDEX[fl, fIndex] + " DESC";
            KTRL = 0;
            JetTableOpen(fl);

            if (RS_MAR[fl].EOF || RS_MAR[fl].RecordCount == -1)
            {
                KTRL = 9;
            }
            else if (RS_MAR[fl].RecordCount == 1)
            {
                KEY_BUF[fl] = string.Empty;
                KEY_INDEX[fl] = fIndex;
            }
            else if (RS_MAR[fl].RecordCount > 1)
            {
                MessageBox.Show("Stop meer dan een record!");
            }
        }

        public static void JetUpdate(int fl, int fIndex)
        {
            var xxxxx = FieldToRecord(fl);
            if (KTRL == 32000) return;

            KEY_BUF[fl] = FVT[fl, fIndex];
            KEY_INDEX[fl] = fIndex;
            if (fl != TABLE_DUMMY)
            {
                RS_MAR[fl].Fields["dnnsync"].Value = false;
            }

            try
            {
                RS_MAR[fl].Update();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public static int RTV(int fl)
        {
            // Reset record buffer for this file
            TLB_RECORD[fl] = string.Empty;
            int t = 0;

            // Walk all field codes for this table until we hit a zero-terminated entry
            while (!string.IsNullOrEmpty(VBC[fl, t]) && Asc(VBC[fl, t][0]) != 0)
            {
                // Copy trimmed field value from recordset into the TeleBib-style buffer
                AdoInsertToRecord(fl,
                    RTrim(Convert.ToString(RS_MAR[fl].Fields[VBC[fl, t]].Value)),
                    VBC[fl, t]);
                t++;
            }

            // Build index key values from the buffer for all defined indexes
            for (t = 0; t <= FL_NUMBEROFINDEXEN[fl]; t++)
            {
                FVT[fl, t] = AdoGetField(fl, "#" + JETTABLEUSE_INDEX[fl, t] + "#");
            }

            return 1; // True
        }

        public static void RecordToField(int fl)
        {
            TLB_RECORD[fl] = string.Empty;
            int t = 0;

            if (fl == TABLE_VARIOUS)
            {
                // Memo-only table: copy MEMO field directly into buffer
                TLB_RECORD[fl] = Convert.ToString(RS_MAR[fl].Fields["MEMO"].Value);
            }
            else if (fl == TABLE_DUMMY)
            {
                // Dummy table also uses MEMO field as whole record buffer
                TLB_RECORD[fl] = Convert.ToString(RS_MAR[fl].Fields["MEMO"].Value);
            }
            else
            {
                // Walk all defined field codes for this table
                while (!string.IsNullOrEmpty(VBC[fl, t]))
                {
                    AdoInsertToRecord(
                        fl,
                        Convert.ToString(RS_MAR[fl].Fields[VBC[fl, t]].Value),
                        VBC[fl, t] + " ");
                    t++;
                }
            }
        }

        public static void WriteLog(string btrieveAktie, int flNummer, int indexNummer, string indexSleutel)
        {
            string recordLijn = btrieveAktie + indexNummer.ToString();

            switch (flNummer)
            {
                case TABLE_VARIOUS:
                    recordLijn += "ALLERLEI";
                    break;
                case TABLE_CUSTOMERS:
                    recordLijn += "KLANTEN ";
                    break;
                case TABLE_SUPPLIERS:
                    recordLijn += "LEVERANC";
                    break;
                case TABLE_LEDGERACCOUNTS:
                    recordLijn += "REKENING";
                    break;
                case TABLE_PRODUCTS:
                    recordLijn += "PRODUKT ";
                    break;
                case TABLE_INVOICES:
                    recordLijn += "dokument";
                    break;
                case TABLE_JOURNAL:
                    recordLijn += "JOURNAAL";
                    break;
                case TABLE_CONTRACTS:
                    recordLijn += "POLISSEN";
                    break;
                case TABLE_COUNTERS:
                    recordLijn += "TELLERS ";
                    break;
            }

            switch (btrieveAktie)
            {
                case "INSERT":
                case "UPDATE":
                    recordLijn += TLB_RECORD[flNummer];
                    break;
                default:
                    recordLijn += indexSleutel;
                    break;
            }

            using (var sw = new StreamWriter(Path.Combine(PROGRAM_LOCATION, "NTIMPORT.LOG"), true))
            {
                sw.WriteLine(recordLijn);
            }
        }

        public static void SetFields(int fl, string vBibCode, string stringData)
        {
            if (string.IsNullOrEmpty(stringData))
                stringData = " ";

            string vBCode = RTrim(vBibCode);
            Console.WriteLine(vBCode);

            try
            {
                if (vBCode == "MEMO")
                {
                    RS_MAR[fl].Fields[vBCode].Value = TLB_RECORD[fl];
                }
                else
                {
                    int definedSize = RS_MAR[fl].Fields[vBCode].DefinedSize;
                    RS_MAR[fl].Fields[vBCode].Value =
                        Mid(stringData, 1, definedSize);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                System.Diagnostics.Debugger.Break();
            }
        }

        public static int FieldToRecord(int fl)
        {
            // Ensure table is open
            if (RS_MAR[fl].State == (int)ObjectStateEnum.adStateClosed)
            {
                KTRL = JetTableOpen(fl);
                // If later you re-enable EditIsPossible, keep 32000 logic here
            }

            // Special composite key handling for specific tables
            if (fl == TABLE_CONTRACTS)
            {
                AdoInsertToRecord(fl,
                    SetSpacing(AdoGetField(fl, "#v164 #"), 2) +
                    SetSpacing(AdoGetField(fl, "#A110 #"), 12) +
                    SetSpacing(AdoGetField(fl, "#A010 #"), 4) +
                    SetSpacing(AdoGetField(fl, "#A000 #"), 12),
                    "v167"); // MaandKlantMaatschappijPolis
            }
            else if (fl == TABLE_JOURNAL)
            {
                AdoInsertToRecord(fl,
                    SetSpacing(AdoGetField(fl, "#v019 #"), 7) +
                    AdoGetField(fl, "#v066 #"),
                    "v070");
            }

            // Build index values from the buffer
            for (int t = 0; t <= FL_NUMBEROFINDEXEN[fl]; t++)
            {
                FVT[fl, t] = SetSpacing(
                    AdoGetField(fl, "#" + JETTABLEUSE_INDEX[fl, t] + "#"),
                    FLINDEX_LEN[fl, t]);
            }

            // Primary index back into record buffer
            AdoInsertToRecord(fl, FVT[fl, 0], JETTABLEUSE_INDEX[fl, 0]);

            // Push buffer values back into fields
            int i = 0;
            while (!string.IsNullOrEmpty(VBC[fl, i]))
            {
                SetFields(fl, VBC[fl, i],
                    AdoGetField(fl, "#" + VBC[fl, i] + " #"));
                i++;
            }

            // Extra handling for TABLE_VARIOUS
            try
            {
                if (fl == TABLE_VARIOUS)
                {
                    RS_MAR[TABLE_VARIOUS].Fields["A000"].Value =
                        AdoGetField(TABLE_VARIOUS, "#A000 #");
                }
            }
            catch
            {
                // ignore to mimic On Error Resume Next
            }

            return 0;
        }

        public static string String99(int lockModus, int szNummer)
        {
            string tlString = "s" + szNummer.ToString("000");

            // Old: LOCK_HOLD = lockModus != READING;
            bool lockHoldLocal = lockModus != 0; // 0 is READING

            JetGet(TABLE_COUNTERS, 0, ref tlString);

            if (KTRL == 99)
            {
                // Not found, return empty
            }
            else if (KTRL != 0)
            {
                MessageBox.Show(
                    "Tellers Stopkode " + KTRL.ToString() +
                    ", voor setup-tellersleutel " + tlString + Environment.NewLine + Environment.NewLine +
                    "Overloop ALLE setup instellingen vooraleer énig boekjaar op te starten !" + Environment.NewLine +
                    "Wij staan tot uw beschikking om U hierbij te helpen.");
            }
            else
            {
                RecordToField(TABLE_COUNTERS);
                try
                {
                    return Convert.ToString(RS_MAR[TABLE_COUNTERS].Fields["v217"].Value);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

            JetTableClose(TABLE_COUNTERS);
            return string.Empty;
        }

        public static void TeleBibPage(int fl)
        {
            int flInput = 0;
            string ffDefinitie;
            string dummyString;
            string dummYtje;
            int ktrlAantal;

            decimal curBedrag;
            int t;

            int result = 0; // False

            try
            {
                string lokaalBestand;
                if (fl != TABLE_COUNTERS)
                {
                    lokaalBestand = Left(TABLEDEF_ONT[fl], 3);
                }
                else
                {
                    lokaalBestand = "00";
                }

                string programmaLokatieHier = Application.StartupPath + "\\";

                string defFolder = Path.Combine(programmaLokatieHier, "Def");
                string baseDefPath = Path.Combine(defFolder, lokaalBestand + ".Def");
                string userDefPath = Path.Combine(defFolder, lokaalBestand + "U.Def");
                string brokerDefPath = Path.Combine(defFolder, lokaalBestand + "M.Def");

                if (!File.Exists(baseDefPath))
                {
                    MessageBox.Show("Geen VsoftBib definitie " + baseDefPath);
                }

                // EerstEnVooral:
                if (!File.Exists(userDefPath))
                {
                    goto GeenUserVoorkeur;
                }
                else
                {
                    t = 0;
                    flInput = 1; // FreeFile() placeholder, not used with StreamReader

                    using (var sr = new StreamReader(userDefPath))
                    {
                        while (!sr.EndOfStream)
                        {
                            // VB Input reads 4 comma‑separated values:
                            // TELEBIB_CODE(T), TELEBIB_TEXT(T), TELEBIB_TYPE(T), TELEBIB_LENGHT(T)
                            string line = sr.ReadLine();
                            if (string.IsNullOrWhiteSpace(line))
                            {
                                continue;
                            }

                            var parts = line.Split(',');
                            if (parts.Length < 4)
                            {
                                continue;
                            }

                            TELEBIB_CODE[t] = parts[0].Trim().Trim('"');
                            TELEBIB_TEXT[t] = parts[1].Trim().Trim('"');
                            TELEBIB_TYPE[t] = parts[2].Trim().Trim('"');

                            if (!int.TryParse(parts[3].Trim().Trim('"'), out TELEBIB_LENGHT[t]))
                            {
                                TELEBIB_LENGHT[t] = 0;
                            }

                            VBC[fl, t] = Mid(TELEBIB_CODE[t], 5, 4);
                            t++;
                        }
                    }

                    VBC[fl, t] = string.Empty;
                    TELEBIB_CODE[t] = string.Empty;
                    TELEBIB_LAST = t - 1;
                    result = 1;
                }

            GeenUserVoorkeur:
                flInput = 1;

                using (var sr = new StreamReader(baseDefPath))
                {
                    t = 0;
                    while (!sr.EndOfStream)
                    {
                        string line = sr.ReadLine();
                        if (string.IsNullOrWhiteSpace(line))
                        {
                            continue;
                        }

                        var parts = line.Split(',');
                        if (parts.Length < 4)
                        {
                            continue;
                        }

                        TELEBIB_CODE[t] = parts[0].Trim().Trim('"');
                        TELEBIB_TEXT[t] = parts[1].Trim().Trim('"');
                        TELEBIB_TYPE[t] = parts[2].Trim().Trim('"');

                        if (!int.TryParse(parts[3].Trim().Trim('"'), out TELEBIB_LENGHT[t]))
                        {
                            TELEBIB_LENGHT[t] = 0;
                        }

                        VBC[fl, t] = Mid(TELEBIB_CODE[t], 5, 4);
                        t++;
                    }
                }

                VBC[fl, t] = string.Empty;
                TELEBIB_LAST = t - 1;
                TELEBIB_CODE[t] = string.Empty;
                result = 1;

            MakelaarIn:
                if (string.IsNullOrWhiteSpace(ProducentNummer))
                {
                    // nothing
                }
                else if (!File.Exists(brokerDefPath))
                {
                    // nothing
                }
                else
                {
                    flInput = 1;

                    using (var sr = new StreamReader(brokerDefPath))
                    {
                        while (!sr.EndOfStream)
                        {
                            string line = sr.ReadLine();
                            if (string.IsNullOrWhiteSpace(line))
                            {
                                continue;
                            }

                            var parts = line.Split(',');
                            if (parts.Length < 4)
                            {
                                continue;
                            }

                            TELEBIB_CODE[t] = parts[0].Trim().Trim('"');
                            TELEBIB_TEXT[t] = parts[1].Trim().Trim('"');
                            TELEBIB_TYPE[t] = parts[2].Trim().Trim('"');

                            if (!int.TryParse(parts[3].Trim().Trim('"'), out TELEBIB_LENGHT[t]))
                            {
                                TELEBIB_LENGHT[t] = 0;
                            }

                            VBC[fl, t] = Mid(TELEBIB_CODE[t], 5, 4);
                            t++;
                        }
                    }

                    VBC[fl, t] = string.Empty;
                    TELEBIB_LAST = t - 1;
                    TELEBIB_CODE[t] = string.Empty;
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show("Telebibinlaadfout " + ex.Message, "TeleBibPage");

            }
        }

        /// <summary>
        /// C# translation of VB6 ToonIndexen.
        /// Fills the Sortering list with index definitions for the given table name.
        /// </summary>
        /// <param name="tableName">Logical table name (e.g. &quot;Klanten&quot;).</param>
        /// <param name="target">Target list control to receive items (&quot;+COL; IndexName&quot;).</param>
        public static void ShowIndexes(string tableName, ListBox target)
        {
            if (target == null)
            {
                return;
            }

            target.Items.Clear();

            // Ensure AD_NTDB is open (ADODB.Connection from ModDatabase)
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
                // Equivalent of: Set rstSchema = adntDB.OpenSchema(adSchemaIndexes)
                rstSchema = AD_NTDB.OpenSchema(ADODB.SchemaEnum.adSchemaIndexes);

                while (!rstSchema.EOF)
                {
                    // TABLE_NAME and COLUMN_NAME come from the schema recordset
                    var table = Convert.ToString(rstSchema.Fields["TABLE_NAME"].Value ?? string.Empty);
                    if (string.Equals(tableName, table, StringComparison.OrdinalIgnoreCase))
                    {
                        var columnName = Convert.ToString(rstSchema.Fields["COLUMN_NAME"].Value ?? string.Empty);
                        var indexName = Convert.ToString(rstSchema.Fields["INDEX_NAME"].Value ?? string.Empty);

                        // VB6: "+" & rstSchema!COLUMN_NAME & "; " & rstSchema!INDEX_NAME
                        string item = "+" + columnName + "; " + indexName;
                        target.Items.Add(item);
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
        }
    }
}