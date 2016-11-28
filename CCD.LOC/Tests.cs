using System.Linq;
using Xunit;


namespace CCD.LOC
{
    public class Tests
    {
        private readonly string source =
            "var x = 5;\n/*kommentar\nnoch mehr kommentar\nkommentar ende*/var y = /*12*/6;\n \nvar text = @\"lirum larum\n\n\";//ich bin ein inliner";


        [Fact]
        public void TesteCodeZeilenZählen()
        {
            Assert.Equal(5, LinesOfCode.CodeZeilenZählen(source));
        }


        [Fact]
        public void TesteKommentarEntfernen()
        {
            Assert.Equal(
                "var x = 5;\n\nvar y = 6;\n \nvar text = @\"lirum larum\n\n\";",
                LinesOfCode.KommentareEntfernen(source));
        }


        [Fact]
        public void TesteInlineKommentarEntfernen()
        {
            Assert.Equal(
                "var x = 5;\n/*kommentar\nnoch mehr kommentar\nkommentar ende*/var y = /*12*/6;\n \nvar text = @\"lirum larum\n\n\";",
                LinesOfCode.InlineKommentareEntfernen(source));

            Assert.Equal(
                "var x = 5; \n\nvar y = 7;",
                LinesOfCode.InlineKommentareEntfernen(
                    "var x = 5; //inline kommentar\n//noch einer\nvar y = 7;//und noch //einer"));
        }


        [Fact]
        public void TesteEinzeilerBlockKommentareEntfernen()
        {
            Assert.Equal(
                "var x = 5;\n/*kommentar\nnoch mehr kommentar\nkommentar ende*/var y = 6;\n \nvar text = @\"lirum larum\n\n\";//ich bin ein inliner",
                LinesOfCode.EinzeilerBlockKommentareEntfernen(source));
        }


        [Fact]
        public void TesteMehrzeilerBlockKommentareEntfernen()
        {
            Assert.Equal(
                "var x = 5;\n\nvar y = /*12*/6;\n \nvar text = @\"lirum larum\n\n\";//ich bin ein inliner",
                LinesOfCode.MehrzeilerBlockKommentareEntfernen(source));
        }


        [Fact]
        public void TesteFindeMehrzeiligeStrings()
        {
            var test =
                "var x = 5;\n\nvar y = 66;\nvar text2 = @\"mehrere\nzeilen\";\n\nalarm();\nvar text = @\"ich\nbin ein\n\n\ngedicht\n\"; ";
            var result = LinesOfCode.FindeMehrzeiligeStrings(test).ToArray();
            Assert.Equal("@\"mehrere\nzeilen\"", result.ElementAt(0));
            Assert.Equal("@\"ich\nbin ein\n\n\ngedicht\n\"", result.ElementAt(1));
        }


        [Fact]
        public void TesteLeerzeilenEntfernen()
        {
            Assert.Equal(
                "var x = 5;\n/*kommentar\nnoch mehr kommentar\nkommentar ende*/var y = /*12*/6;\nvar text = @\"lirum larum\n\";//ich bin ein inliner",
                LinesOfCode.LeerzeilenEntfernen(source));

            Assert.Equal(
                "zeile 1\nzeile 2\nvar x = @\"hallo\nwelt\"",
                LinesOfCode.LeerzeilenEntfernen("zeile 1\n\n\nzeile 2\nvar x = @\"hallo\n\nwelt\""));
        }


        [Fact]
        public void TesteZähleLeerzeilenInStrings()
        {
            Assert.Equal(3, LinesOfCode.ZähleLeerzeilenInString("ich\nbin ein\n\n \ngedicht\n"));
        }


        [Fact]
        public void TesteZeilenZählen()
        {
            Assert.Equal(6, LinesOfCode.ZeilenZählen("a\n\"ich\nbin\nmehrzeilig\"\nc\nd", 0));
        }
    }
}