using System;
using System.Windows.Forms;
using mar2026.Classes;

namespace mar2026
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Perform VB6-like startup initialisation
            LoadMain.Initialize();

            // Show main form (Mim in VB6, FormMim in .NET)
            Application.Run(new FormMim());
        }
    }
}
