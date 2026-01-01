using DAO;
using Scripting;
using System;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using System.Windows.Forms;

namespace mar2026.Classes
{
    public static class ModLibs
    {
        // mar2026/Classes/ModLibs.cs, inside ModLibs
        public static FormBasisFiche CUSTOMERS_SHEET;
        public static FormBasisFiche SUPPLIERS_SHEET;
        public static FormBasisFiche LEDGERACCOUNTS_SHEET;

        // Mijn dokumenten, ApplicatieData
        public const int CSIDL_PERSONAL = 0x5;
        public const int CSIDL_APPDATA = 0x1A;
        public const int CSIDL_PROGRAM_FILES = 0x26;

        // VB6 sound flags kept for reference (not used yet)
        public const int SND_ASYNC = 0x1;
        public const int SND_NODEFAULT = 0x2;
        public const int SND_MEMORY = 0x4;
        public static string SoundBuffer;

        public const string TABLE_STR = "Tabel";
        public const string ATTACHED_STR = "Verbonden";
        public const string QUERY_STR = "Opzoeking";
        public const string FIELD_STR = "Kolom";
        public const string FIELDS_STR = "Kolommen";
        public const string INDEX_STR = "Index";
        public const string INDEXES_STR = "Indexen";
        public const string PROPERTY_STR = "Eigenschap";
        public const string PROPERTIES_STR = "Eigenschappen";

        public const string ADOJET_PROVIDER = "Provider=Microsoft.Jet.OLEDB.4.0;";

        // current database node in treeview
        public static TreeNode gnodDBNode;
        // backup of current database node
        public static TreeNode gnodDBNode2;

        // marNT constanten
        public const int NUMBER_TABLES = 9;
        public const int TABLE_VARIOUS = 0;
        public const int TABLE_CUSTOMERS = 1;
        public const int TABLE_SUPPLIERS = 2;
        public const int TABLE_LEDGERACCOUNTS = 3;
        public const int TABLE_PRODUCTS = 4;
        public const int TABLE_CONTRACTS = 5;
        public const int TABLE_INVOICES = 6;
        public const int TABLE_JOURNAL = 7;
        public const int TABLE_DUMMY = 8;
        public const int TABLE_COUNTERS = 9;

        public const int PERIODAS_TEXT = 0;
        public const int BOOKYEARAS_TEXT = 1;
        public const int PERIODAS_KEY = 2;
        public const int BOOKYEARAS_KEY = 3;
        public const string SISO = "001*002*002*003*004*005*006*007*008*009*010*011*030*032*038*046*053*054*055*060*061*063*064*091*600*";
        public const int MAX_TELEBIB = 150;
        public const bool READING = true;
        public const bool READING_LOCK = false;

        public const string MASK_EURX = "######0.0000";
        public const string MASK_EURBH = "########0.00";

        public const string MASK_BEF = "##########";
        public const string MASK_EUR = "######0.00";

        public const double EURO = 40.3399;

        public static string usrMailAdres;
        public static string usrPW;

        public static string FileNameQR;
        public static bool PeppolFlag;

        // Fixed-length VB strings have no direct C# equivalent; use normal strings.
        public static string A;   // length 16 in VB
        public static string aa;  // length 4 in VB
        public static string AAA; // length 30 in VB

        public static string[] MASK_SY = new string[9]; // 0..8
        public static string MASK_2002; // VB: Fixed-length 10
        public static bool VSF_PRO;

        public static string[] SYS_VAR = new string[7]; // 0..6
        public static int[] FILE_NR = new int[NUMBER_TABLES + 1]; // 0..9
        public static string[] TLB_RECORD = new string[NUMBER_TABLES + 1];
        public static string[] KEY_BUF = new string[NUMBER_TABLES + 1];
        public static string[] TABLEDEF_ONT = new string[NUMBER_TABLES + 1];
        public static int[] KEY_INDEX = new int[NUMBER_TABLES + 1];
        public static int[] INSERT_FLAG = new int[NUMBER_TABLES + 1];
        public static int[] FL_NUMBEROFINDEXEN = new int[11]; // 0..10
        public static string[,] JETTABLEUSE_INDEX = new string[NUMBER_TABLES + 1, 11];
        public static int[,] FLINDEX_LEN = new int[NUMBER_TABLES + 1, 11];
        public static string[,] FLINDEX_CAPTION = new string[NUMBER_TABLES + 1, 11];
        public static string[,] FVT = new string[NUMBER_TABLES + 1, 11];
        public static string[] SQL_MSG = new string[NUMBER_TABLES + 1];

        public static string PROGRAM_LOCATION;
        public static int TELEBIB_LAST;
        public static string[] TABLEDEF_ONT_;
        public static string MSG;
        public static decimal DKTRL_CUMUL;
        public static decimal DKTRL_BEF;
        public static decimal DKTRL_EUR;

        public static int[] DAYS_IN_MONTH = new int[13]; // 1..12 used
        public static string[] MONTH_AS_TEXT = new string[13]; // 1..12 used

        public static string[] REPORT_FIELD = new string[24]; // 0..23
        public static int[] REPORT_TAB = new int[24];

        public static string[] TELEBIB_CODE = new string[MAX_TELEBIB + 2];      // -1..150 mapped to 0..151
        public static string[] TELEBIB_TEXT = new string[MAX_TELEBIB + 1];
        public static string[] TELEBIB_TYPE = new string[MAX_TELEBIB + 1];
        public static int[] TELEBIB_LENGHT = new int[MAX_TELEBIB + 1];
        public static int[] TELEBIB_POS = new int[MAX_TELEBIB + 1];
        
        public static int FL99;
        public static string FL99_RECORD;
        public static int PRINTER_CURRENT_Y;
        public static int PAGE_COUNTER;
        public static readonly string FULL_LINE = new string('-', 128);

        public static string MAR_VERSION;
        public static string LOG_PRINT;
        public static bool BL_LOGGING;
                
        public static int B_MODUS;
        public static int COUNT_TO;

        public static string PERIOD_FROMTO;  // length 16 in VB
        public static string BOOKYEAR_FROMTO; // length 16 in VB
        public static int ACTIVE_BOOKYEAR;
        public static string MIM_GLOBAL_DATE; // length 10 in VB
        public static bool VAT_BOBTHEBUILDERS;
        public static string DIRECTSELL_STRING;

        public static string LOCATION_COMPANYDATA;
        public static string LOCATION_NETDATA;
        public static string LOCATION_;
        public static string LOCATION_ASWEB;
        public static string LOCATION_MYDOCUMENTS;
        public static string SYSTEM_MYPERSONALDOCUMENTS;

        public static string ProducentNummer; // length 8 in VB
        public static string Eigenaar;        // length 8 in VB
        public static int FL;
        public static int SHARED_FL;
        public static int SHAREDSCAN_FL;
        public static int KTRL;
        public static int KTRL_LONG;
        public static int A_INDEX;
        public static int SHARED_INDEX;
        public static int ACTIVE_SHEET;

        public static bool BL_ENVIRONMENT;
        public static string ENVIRONMENT_GRIDTEXT;
        public static string GRIDTEXT;
        public static string GRIDTEXT_IS;
        public static object GRIDTEXT_POLICY;
        public static string GRIDTEXT_9;
        public static int GRID_ROWS;
        public static string XLOG_KEY;

        public static string XLOG_CASHREGISTER;

        public static double DCTRL_CUMUL;
        public static int SETUP_FIELDS;
        public static string COMPANY_CHOISE;
        public static double D_CURRENCY;        
        public static int CTRL_BOX;
        public static string SQL_COMMAND;
        public static int DOEVENTS_STATUS;
        public static int VSOFT_LOG;
        public static string PROGRAM_VERSION;
        public static int LOCK_HOLD;

        // Public KBTable As DAO.Recordset
        public static Database NT_DB;
        public static ADODB.Recordset[] NT_RS = new ADODB.Recordset[10]; // 0..9
        public static Workspace NT_SPACE;

        public static ADODB.Connection AD_KBDB;
        public static ADODB.Recordset AD_KBTable;

        public static ADODB.Connection AD_NTDB;
        public static ADODB.Connection AD_NTDB_SQLS;

        public static ADODB.Connection AD_TBIB;
        public static ADODB.Recordset RS_VALUES;
        public static ADODB.Recordset RS_JOURNAL;
        public static ADODB.Recordset[] RS_MAR = new ADODB.Recordset[10]; // 0..9
        public static string JET_CONNECT;
        public static string SQL_CONNECT;

        public static int XDO_EVENTS;
        public static string[] JET_TABLENAME = new string[10]; // 0..9
        public static int[] ADDNEW_STATUS = new int[10];

        public static string[,] VBC = new string[10, 201]; // [0..9,0..200]
        public static int BA_MODUS;

        public static bool TEST_EUROMODUS;
        public static bool BH_EURO;
        public static bool XisEUROWasBEF;

        public static DateTime TIMER_TIME;
        public static object RETURN_VALUE;
        public static object FIGURE1;
        public static object FIGURE2;

        public static int LISTPRINTER_NUMBER;
        public static int DOCUMENTPRINTER_NUMBER;
        public static int CASHPRINTER_NUMBER;

        public static object JUMP_FORM;

        public static FileSystemObject FS;

        public static decimal CashRegisterTicketTotal;
        public static decimal CashRegisterTotal;
        public static decimal CashRegisterPayingBEF;
        public static decimal CashRegisterPayingEUR;
        public static decimal CashRegisterBackEUR;

        public static decimal CashRegisterTotalBEF;
        public static decimal CashRegisterTotalEUR;

        public static bool DECIMAL_CTRL;

        // marIntegraal.NET
        public static string USER_LICENSEINFO;
        public static bool JOURNAL_LOCKED;
        public static string USER_MAILADDRESS;
        public static string USER_PASSWORD;

        public static double PDF_VSOFT_FROM;
        public static double PDF_VSOFT_TO;
        public static double PDF_ADDRESS_XPOS;
        public static double PDF_ADDRESS_YPOS;
        public static double PDF_ADDRESS_XPOS2;
        public static double PDF_ADDRESS_YPOS2;

        public static string STR_TELEBIBIO;

        public static string UITWISSELING_OMS;
        public static string UITWISSELING_DATA;
        public static string DOCUMENTLINES_OMS;
        public static string DOCUMENTLINES_DATA;

        public static string[] UITWISSELING_OMS_ARRAY;
        public static string[] UITWISSELING_DATA_ARRAY;
        public static string[] DOCUMENTLINES_OMS_ARRAY;
        public static string[] DOCUMENTLINES_DATA_ARRAY;
                

        // ADO.NET equivalents
        public static OleDbConnection adKBDB;
        public static OleDbConnection adTBIB;
        public static DataTable adKBTable;

        /// <summary>
        /// Simple helper similar to VB ShellExecuteWithFallback using Process.Start.
        /// </summary>
        public static bool ShellExecuteWithFallback(string target)
        {
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = target,
                    UseShellExecute = true
                });
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("ShellExecuteWithFallback error: " + ex.Message);
                return false;
            }
        }

        public static string[] bstNaam = new string[10]; // 0..9 table names (jrYYYY etc.)
        public static string String99(bool lockModus, int szNummer)
        {
            string tlString = "s" + szNummer.ToString("000");
            LOCK_HOLD = (int)(lockModus != READING ? 1 : 0);
            return tlString;
        }

        public static FormBasisFiche FormReference;
        public static FormBasisFiche[] BasisB = new FormBasisFiche[4]; // 1..3 used
    }
}