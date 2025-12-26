using mar2026.Classes;
using System;
using System.Windows.Forms;

namespace mar2026
{
    public partial class FormBJPERDAT : Form
    {
        public FormBJPERDAT()
        {
            InitializeComponent();
            // Optional: prevent automatic resizing
            this.AutoSize = false;
            this.AutoSizeMode = AutoSizeMode.GrowOnly;
            this.FormBorderStyle = FormBorderStyle.FixedDialog; // or FixedSingle/Fixed3D
            this.MaximizeBox = false;

            // Optional: force your preferred size
            // 327; 149
            this.Size = new System.Drawing.Size(327, 149); // pick your design size
        }
        

        private void BtnVerkleinen_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void FormBJPERDAT_Load(object sender, EventArgs e)
        {
            DatumVerwerking.Value = DateTime.Now;
            //TODO: DatumVerwerking_ValueChanged(this, EventArgs.Empty);
        }

        private void FormBJPERDAT_Activated(object sender, EventArgs e)
        {
            DatumVerwerking.Focus();
            // TODO: VB6 Form_Activate logic
            //if (this.datumVerwerking.Value.Date != DateTime.Now.Date)
            //{
            //    this.datumVerwerking.Value = DateTime.Now.Date;
            //    DatumVerwerking_ValueChanged(this, EventArgs.Empty);
            //}
        }

        private void DatumVerwerking_ValueChanged(object sender, EventArgs e)
        {
            ModLibs.MIM_GLOBAL_DATE = DatumVerwerking.Value.ToString("dd/MM/yyyy");

            if (Application.OpenForms["FormMim"] is FormMim mim)
            {
                mim.toolStripStatusBookingsDate.Text = ModLibs.MIM_GLOBAL_DATE;
            }
        }

        private void CmbBoekjaar_SelectedIndexChanged(object sender, EventArgs e)
        {
            ModLibs.ACTIVE_BOOKYEAR = (short)CmbBoekjaar.SelectedIndex;

        }

        private void CmbPeriodeBoekjaar_SelectedIndexChanged(object sender, EventArgs e)
        {
            // PeriodeBoekjaar_Click: compute PERIOD_FROMTO and caption.
            string a = this.CmbPeriodeBoekjaar.Text;
            if (a.Length < 21)
            {
                return;
            }

            string periodFromTo = a.Substring(6, 4) + a.Substring(3, 2) + a.Substring(0, 2)
                                  + a.Substring(a.Length - 4, 4) + a.Substring(16, 2) + a.Substring(13, 2);
            ModLibs.PERIOD_FROMTO = periodFromTo;

            this.Text = "(" + (this.CmbBoekjaar.Text ?? string.Empty) + ") (" + a.Substring(0, 10) + ") BoekPeriode";
        }
    }
}
