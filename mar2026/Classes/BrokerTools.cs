using System;

namespace mar2026.Classes
{
    /// <summary>
    /// Helpers converted from VB6 modMakelaars (broker / Assurnet tooling).
    /// This class currently contains only the stateless string/number helpers.
    /// </summary>
    public static class BrokerTools
    {
        /// <summary>
        /// Extract an XEH/XET snippet for a given policy number from a UN/EDIFACT user area.
        /// This is a direct port of VB6 SnippetXEH.
        /// </summary>
        /// <param name="userArea">Raw EDIFACT string</param>
        /// <param name="polisNummer">Policy number to find (RFF+001:polis)</param>
        /// <param name="indent">If true, perform tb2-style indentation (not yet implemented)</param>
        /// <returns>Snippet string or empty if not found / wrong structure</returns>
        public static string SnippetXEH(string userArea, string polisNummer, bool indent)
        {
            if (string.IsNullOrEmpty(userArea) || string.IsNullOrEmpty(polisNummer))
                return string.Empty;

            string zoekXET;
            string zoekXEH;

            if (userArea.IndexOf("XET+03", StringComparison.Ordinal) >= 0)
            {
                // Termijnen en contanten via borderels
                zoekXEH = "XEH+03";
                zoekXET = "XET+03";
            }
            else if (userArea.IndexOf("XRT+2", StringComparison.Ordinal) >= 0)
            {
                // Commissies via rekeninguittreksels
                zoekXET = "XRT+1";
                zoekXEH = "XRH+1";
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("geen XET te vinden!", "SnippetXEH",
                    System.Windows.Forms.MessageBoxButtons.OK,
                    System.Windows.Forms.MessageBoxIcon.Error);
                return string.Empty;
            }

            var parts = userArea.Split(new[] { zoekXET }, StringSplitOptions.None);
            for (int i = 0; i < parts.Length; i++)
            {
                if (parts[i].IndexOf("RFF+001:" + polisNummer, StringComparison.Ordinal) >= 0)
                {
                    if (!indent)
                    {
                        // substring from "'XEH..." up to and including the zoekXET + "'"
                        int posXeh = parts[i].IndexOf("'" + zoekXEH, StringComparison.Ordinal);
                        if (posXeh < 0)
                            return string.Empty;

                        string tail = parts[i].Substring(posXeh);
                        return tail + zoekXET + "'";
                    }
                    else
                    {
                        // tb2Indent is not ported yet; just return the raw snippet.
                        return parts[i] + zoekXET + "'";
                    }
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// Decode an encoded Belgian amount string used in some Assurnet formats.
        /// Port of VB6 kt(fBedrag As String) As String.
        /// </summary>
        public static string Kt(string bedrag)
        {
            if (string.IsNullOrEmpty(bedrag))
                return string.Empty;

            char last = bedrag[bedrag.Length - 1];

            switch ((int)last)
            {
                // '0'..'9' or space => no change
                case int n when (n >= 48 && n <= 57) || n == 32:
                    return bedrag;

                // è or é (232, 233) => replace last char with '0'
                case 232:
                case 233:
                    return bedrag.Substring(0, bedrag.Length - 1) + "0";

                // 'A'..'I' => last digit 1..9 (Asc-64)
                case int n2 when (n2 >= 65 && n2 <= 73):
                    return bedrag.Substring(0, bedrag.Length - 1) + (n2 - 64).ToString("0");

                // 'J'..'R' => negative amounts; first char set to '-', last digit 1..9 (Asc-73)
                case int n3 when (n3 >= 74 && n3 <= 82):
                    {
                        char[] chars = bedrag.ToCharArray();
                        chars[0] = '-';
                        string prefix = new string(chars, 0, chars.Length - 1);
                        string digit = (n3 - 73).ToString("0");
                        return prefix + digit;
                    }

                default:
                    System.Windows.Forms.MessageBox.Show(
                        "Foutieve waarde in conversietafel voor '" + bedrag + "'" +
                        Environment.NewLine + Environment.NewLine +
                        "Kontakteer onmiddellijk de maatschappij !!",
                        "kt-conversie",
                        System.Windows.Forms.MessageBoxButtons.OK,
                        System.Windows.Forms.MessageBoxIcon.Error);
                    return new string('0', bedrag.Length);
            }
        }

        /// <summary>
        /// Remove '.', '/', '-' from a number string (VB6 Puur function).
        /// </summary>
        public static string Puur(string nummer)
        {
            if (string.IsNullOrEmpty(nummer))
                return string.Empty;

            string result = nummer;
            char[] toRemove = { '.', '/', '-' };
            foreach (char c in toRemove)
            {
                result = result.Replace(c.ToString(), string.Empty);
            }
            return result;
        }

        /// <summary>
        /// Format a numeric string to a zero-padded fixed length (VB6 Tk function).
        /// </summary>
        public static string Tk(string bedrag, int lengte)
        {
            double val;
            double.TryParse(bedrag ?? "0", out val);
            return val.ToString(new string('0', lengte));
        }
    }
}