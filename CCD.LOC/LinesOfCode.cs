using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;


namespace CCD.LOC
{
    public class LinesOfCode
    {
        public static int CodeZeilenZählen(string quelle)
        {
            var quelleOhneKommentare = KommentareEntfernen(quelle);
            var quelleOhneKommentareUndLeerzeilen = LeerzeilenEntfernen(quelleOhneKommentare);
            var anzahlLeerzeilenInStrings = ZähleLeerzeilenInMehrzeiligenStrings(quelle);
            return ZeilenZählen(quelleOhneKommentareUndLeerzeilen, anzahlLeerzeilenInStrings);
        }


        public static int ZähleLeerzeilenInMehrzeiligenStrings(string quelle)
        {
            var mehrzeiligeStrings = FindeMehrzeiligeStrings(quelle);
            return ZähleLeerzeilenInStrings(mehrzeiligeStrings);
        }


        public static string KommentareEntfernen(string quelle)
        {
            var quelleOhneInlineKommentare = InlineKommentareEntfernen(quelle);
            var quelleMitHöchstensMehrzeilerBlockKommentaren =
                EinzeilerBlockKommentareEntfernen(quelleOhneInlineKommentare);
            return MehrzeilerBlockKommentareEntfernen(quelleMitHöchstensMehrzeilerBlockKommentaren);
        }


        public static string InlineKommentareEntfernen(string quelle)
        {
            return Regex.Replace(quelle, @"\/\/.*", "");
        }


        public static string EinzeilerBlockKommentareEntfernen(string quelle)
        {
            return Regex.Replace(quelle, @"\/\*.*\*\/", "");
        }


        public static string MehrzeilerBlockKommentareEntfernen(string quelle)
        {
            return Regex.Replace(quelle, @"\/\*(((?!\*\/).)*\n((?!\*\/).)*)+\*\/", "\n");
        }


        public static string LeerzeilenEntfernen(string quelle)
        {
            return Regex.Replace(quelle, @"(\n\s*\n)", "\n");
        }


        public static int ZähleLeerzeilenInString(string quelle)
        {
            var zeilen = quelle.Split(new[] {"\n"}, StringSplitOptions.None);
            return zeilen.Count(string.IsNullOrWhiteSpace);
        }


        public static int ZähleLeerzeilenInStrings(IEnumerable<string> quellen)
        {
            return quellen.Sum(ZähleLeerzeilenInString);
        }


        public static IEnumerable<string> FindeMehrzeiligeStrings(string quelle)
        {
            return from object match in Regex.Matches(quelle, "@\"(((?!\").)*\\n((?!\").)*)*\"") select match.ToString();
        }


        public static int ZeilenZählen(string quelle, int anzahlLeerzeilenInStrings)
        {
            return quelle.Split(new[] {"\n"}, StringSplitOptions.None).Length + anzahlLeerzeilenInStrings;
        }
    }
}