using mar2026.Classes;
using System;
using System.IO;
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
            // DatumVerwerking.Value = DateTime.Now;
            DatumVerwerking.Value = SharedGlobals.Rdt;
        }

        private void FormBJPERDAT_Activated(object sender, EventArgs e)
        {
         
            if (SharedGlobals.MimLoadingNewCompany)
            {
                // 1. Load last used financial year and period
                string[] LastUsedFinancial = GetLastFinancial();

                // 2a. Insert available accounting years            
                CmbBoekjaar.Items.Clear();
                FillYearsCombo(this);
                // 2b. Set the last used accounting year
                int LastUsedYear = int.Parse(LastUsedFinancial[0]);
                CmbBoekjaar.SelectedIndex = LastUsedYear;

                // 3a. Insert available accounting periods
                CmbPeriodeBoekjaar.Items.Clear();
                FillPeriodsCombo(this, LastUsedYear);
                // 3b. Set the last used accounting period
                int LastUsedPeriod = int.Parse(LastUsedFinancial[2]);
                CmbPeriodeBoekjaar.SelectedIndex = LastUsedPeriod - 1;
                SharedGlobals.MimLoadingNewCompany = false;
                SharedGlobals.MimActiveBookYearText = CmbBoekjaar.Text;
                SharedGlobals.MimActiveBookPeriodText = CmbPeriodeBoekjaar.Text;
                SetAccountingPeriodRange(CmbBoekjaar.SelectedIndex, CmbPeriodeBoekjaar.SelectedIndex);

                // TODO Globals.MimActiveBookPeriod = AccountingPeriods.Text without the / and - characters

            }
        }        

        private void DatumVerwerking_ValueChanged(object sender, EventArgs e)
        {
            ModLibs.MIM_GLOBAL_DATE = DatumVerwerking.Value.ToString("dd/MM/yyyy");

            if (Application.OpenForms["FormMim"] is FormMim mim)
            {
                mim.toolStripBookingDateNow.Text = ModLibs.MIM_GLOBAL_DATE;
            }
        }

        private void CmbBoekjaar_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Keep existing behavior
            ModLibs.ACTIVE_BOOKYEAR = (int)CmbBoekjaar.SelectedIndex;

            if (SharedGlobals.MimLoadingNewCompany)
                return;

            if (CmbBoekjaar.Text != SharedGlobals.MimActiveBookYearText)
            {
                SharedGlobals.MimActiveBookYearText = CmbBoekjaar.Text;
                CmbPeriodeBoekjaar.Items.Clear();
                FillPeriodsCombo(this, CmbBoekjaar.Items.IndexOf(CmbBoekjaar.Text));
                CmbPeriodeBoekjaar.SelectedIndex = 0;
            }
        }

        private void CmbPeriodeBoekjaar_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SharedGlobals.MimLoadingNewCompany)
                return;

            if (CmbPeriodeBoekjaar.Text != SharedGlobals.MimActiveBookPeriodText)
            {
                SharedGlobals.MimActiveBookPeriodText = CmbPeriodeBoekjaar.Text;
                SetAccountingPeriodRange(CmbBoekjaar.SelectedIndex, CmbPeriodeBoekjaar.SelectedIndex);
                Text = "(" + CmbBoekjaar.Text + ") (" + CmbPeriodeBoekjaar.Text + ") BoekPeriode";
            }
            Save9999();
            
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

        private static string[] GetLastFinancial()
        {            
            string line = "";
            string FullPath;

            FullPath = Path.Combine(SharedGlobals.MimDataLocation ?? string.Empty, "9999.OXT");
            // Load last used financial year and period
            if (File.Exists(FullPath))
            {
                try
                {
                    var stream = new StreamReader(FullPath);
                    if (stream.Peek() > -1)
                    {
                        line = stream.ReadLine();
                    }
                    stream.Close();
                }
                catch (IOException ex)
                {
                    MessageBox.Show(FullPath + " could not be read");
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                MessageBox.Show(FullPath + " not found");
                MessageBox.Show("Creating a new 9999.OXT with content: 0,2025,1");
                line = "0,2025,1";
                File.WriteAllText(FullPath, line);
                MessageBox.Show("Done!");
            }
            return line.Split(',');
        }

        private static void FillYearsCombo(FormBJPERDAT formBJ)
        {
            string line = "";
            string FullPath;

            if (SharedGlobals.MimLoadingNewCompany)
            {
                formBJ.CmbBoekjaar.Items.Clear();
            }

            // Insert available accounting years
            for (int i = 9; i >= 0; i--)
            {
                FullPath = Path.Combine(SharedGlobals.MimDataLocation ?? string.Empty, "DEF" + i.ToString("00") + ".OXT");
                if (File.Exists(FullPath))
                {
                    try
                    {
                        var stream = new StreamReader(FullPath);
                        if (stream.Peek() > -1)
                        {
                            line = stream.ReadLine();
                            // MessageBox.Show(line);
                        }
                        stream.Close();
                    }
                    catch (IOException ex)
                    {
                        MessageBox.Show(FullPath + " could not be read");
                        MessageBox.Show(ex.Message);
                    }
                }
                else
                {
                    MessageBox.Show(FullPath + " not found");
                }
                if (!string.IsNullOrEmpty(line) && line.Length >= 4)
                {
                    formBJ.CmbBoekjaar.Items.Insert(0, line.Substring(0, 4));
                }
            }
        }

        private static void FillPeriodsCombo(FormBJPERDAT formBYPERDAT, int lastUsedYear)
        {
            string line = "";
            string FullPath;

            FullPath = Path.Combine(SharedGlobals.MimDataLocation ?? string.Empty, "DEF" + lastUsedYear.ToString("00") + ".OXT");
            // MessageBox.Show(FullPath);
            if (File.Exists(FullPath))
            {
                try
                {
                    var stream = new StreamReader(FullPath);
                    if (stream.Peek() > -1)
                    {
                        line = stream.ReadLine();
                        // MessageBox.Show(line);
                    }
                    stream.Close();
                }
                catch (IOException ex)
                {
                    MessageBox.Show(FullPath + " could not be read");
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                MessageBox.Show(FullPath + " not found");
            }
            string[] PeriodsLastUsedYear = line.Split(',');
            int UpperBound = PeriodsLastUsedYear.Length;

            string A;
            string PeriodFromFormatted;
            string PeriodToFormatted;

            for (int T = 0; T < UpperBound; T++)
            {
                A = PeriodsLastUsedYear[T];
                if (!string.IsNullOrEmpty(A) && A.Length >= 16)
                {
                    PeriodFromFormatted = A.Substring(6, 2) + "/" + A.Substring(4, 2) + "/" + A.Substring(0, 4);
                    PeriodToFormatted = A.Substring(14) + "/" + A.Substring(12, 2) + "/" + A.Substring(8, 4);
                    formBYPERDAT.CmbPeriodeBoekjaar.Items.Add(PeriodFromFormatted + " - " + PeriodToFormatted);
                }
            }

        }

        private static void SetAccountingPeriodRange(int SelectedYearIndex, int SelectedPeriodIndex)
        {
            string line = "";
            string FullPath;

            FullPath = Path.Combine(SharedGlobals.MimDataLocation ?? string.Empty, "DEF" + SelectedYearIndex.ToString("00") + ".OXT");
            // MessageBox.Show(FullPath);
            if (File.Exists(FullPath))
            {
                try
                {
                    var stream = new StreamReader(FullPath);
                    if (stream.Peek() > -1)
                    {
                        line = stream.ReadLine();
                        // MessageBox.Show(line);
                    }
                    stream.Close();
                }
                catch (IOException ex)
                {
                    MessageBox.Show(FullPath + " could not be read");
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                MessageBox.Show(FullPath + " not found");
            }
            string[] Periods = line.Split(',');
            SharedGlobals.SetMimActiveBookPeriod(Periods[SelectedPeriodIndex]);
        }

        private void Save9999()
        {
            string FullPath = Path.Combine(SharedGlobals.MimDataLocation ?? string.Empty, "9999.OXT");
            string line = CmbBoekjaar.SelectedIndex.ToString() + "," + CmbBoekjaar.Text + "," + (CmbPeriodeBoekjaar.SelectedIndex + 1).ToString();
            File.WriteAllText(FullPath, line);
        }
    }
}
