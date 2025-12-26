using ADODB;
using System;
using System.IO;
using System.Windows.Forms;

using mar2026.Classes;
using static mar2026.Classes.AllFunctions;
using static mar2026.Classes.ModLibs;

namespace mar2026
{
    public partial class FormBasisFiche : Form
    {
        public FormBasisFiche()
        {
            InitializeComponent();
            // this.MinimumSize = new System.Drawing.Size(327, 149);
            // this.MaximumSize = new System.Drawing.Size(327, 149);
        }

        private void ButtonMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
    }
}
