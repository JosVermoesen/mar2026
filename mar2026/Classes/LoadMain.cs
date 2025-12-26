using System;
using System.IO;
using System.Windows.Forms;
using ADODB;
using Scripting;

using static mar2026.Classes.ModLibs;

namespace mar2026.Classes
{
    /// <summary>
    /// Startup helper converted from VB6 aLoadMain.Main.
    /// Handles version info, basic licensing stub, and initial form show.
    /// </summary>
    public static class LoadMain
    {
        public static string StrConnect;
        public static string StrLogFile;

        /// <summary>
        /// Perform VB6-like startup initialisation. This is not an entry point;
        /// Program.Main calls this helper.
        /// </summary>
        public static void Initialize()
        {
            // Version info (no direct App.Major/Minor/Revision in .NET)
            var v = typeof(LoadMain).Assembly.GetName().Version;
            MAR_VERSION = v != null ? v.ToString() : string.Empty;

            // Persist version like BeWaarTekst "marIntegraal", "Version", MAR_VERSION
            try
            {
                var key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(@"Software\\marIntegraal");
                key?.SetValue("Version", MAR_VERSION ?? string.Empty);
            }
            catch
            {
                // ignore registry failures
            }

            PeppolFlag = false;
            DECIMAL_CTRL = false;   // VB6 DecimalKTRL
            BL_LOGGING = false;

            for (int i = 0; i < RS_MAR.Length; i++)
            {
                RS_MAR[i] = new Recordset();
            }

            // Working directory like VB6 ChDir App.Path
            try
            {
                var exePath = Application.StartupPath;
                Directory.SetCurrentDirectory(exePath);

                // FS = New FileSystemObject
                FS = new FileSystemObject();

                // Simple license marker us103.lic in exe folder (DEMO handling omitted for now)
                string licPath = Path.Combine(exePath, "us103.lic");
                if (!System.IO.File.Exists(licPath))
                {
                    System.IO.File.WriteAllText(licPath, DateTime.Now.ToString("s"));
                }

                StrLogFile = Path.Combine(exePath, Path.GetFileNameWithoutExtension(Application.ExecutablePath) + ".log");
            }
            catch
            {
                // ignore IO/FS issues; app can still continue
            }
        }
    }
}
