using SKNF_SDNF;

namespace SKNF_SDNF_Minimizer.Tests
{
    [TestClass]
    public class MinimizerTests
    {
        private readonly Minimizer _minimizer = new Minimizer();

        [TestMethod]
        public void MinimizeSDNFByCalculatingTest()
        {
            string input = "(!a/\\!b/\\c)\\/(!a/\\b/\\!c)\\/(!a/\\b/\\c)\\/(a/\\b/\\!c)";
            string expected = "(!a/\\c)\\/(b/\\!c)";
            string result = _minimizer.MinimizeSDNFByCalculating(input);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void MinimizeSKNFByCalculatingTest()
        {
            string input = "(a\\/b\\/c)/\\(!a\\/b\\/c)/\\(!a\\/b\\/!c)/\\(!a\\/!b\\/!c)";
            string expected = "(b\\/c)/\\(!a\\/!c)";
            string result = _minimizer.MinimizeSKNFByCalculating(input);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void MinimizeSDNFByTableAndCalculatingTest()
        {
            string input = "(!a/\\!b/\\c)\\/(!a/\\b/\\!c)\\/(!a/\\b/\\c)\\/(a/\\b/\\!c)";
            string expected = "(!a/\\c)\\/(b/\\!c)";
            string result = _minimizer.MinimizeSDNFByTableAndCalculating(input);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void MinimizeSKNFByTableAndCalculatingTest()
        {
            string input = "(a\\/b\\/c)/\\(a\\/b\\/!c)/\\(a\\/!b\\/!c)/\\(!a\\/b\\/!c)/\\(!a\\/!b\\/!c)";
            string expected = "(a\\/b)/\\(!c)";
            string result = _minimizer.MinimizeSKNFByTableAndCalculating(input);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void MinimizeSDNFByKarnaughTest()
        {
            string input = "(!a/\\!b/\\c)\\/(!a/\\b/\\!c)\\/(!a/\\b/\\c)\\/(a/\\b/\\!c)";
            string expected = "(!a/\\c)\\/(b/\\!c)";
            string result = _minimizer.MinimizeSDNFByKarnaugh(input);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void MinimizeSKNFByKarnaughTest()
        {
            string input = "(a\\/b\\/c)/\\(!a\\/b\\/c)/\\(!a\\/b\\/!c)/\\(!a\\/!b\\/!c)";
            string expected = "(!a\\/!c)/\\(b\\/c)";
            string result = _minimizer.MinimizeSKNFByKarnaugh(input);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void MinimizeByKarnaughWith5VariablesTest()
        {
            InputParser parser = new InputParser();
            Minimizer minimizer = new Minimizer();
            List<List<int>> cases;
            string reformedFormula;
            string sknf;
            string sdnf;
            string input = "!a|!b|!c|!d|!e";
            string expected = "(!a)\\/(!b)\\/(!c)\\/(!d)\\/(!e)";
            reformedFormula = parser.ReformFormula(input);
            cases = CaseGenerator.GenerateCases(parser.Letters.Count);
            sdnf = SknfSdnfFinder.FindSDNF(cases, parser, reformedFormula, out _);
            string result = _minimizer.MinimizeSDNFByKarnaugh(sdnf);
            Assert.AreEqual(expected, result);
            input = "a|b|c|d|e";
            expected = "(a\\/b\\/c\\/d\\/e)";
            reformedFormula = parser.ReformFormula(input);
            cases = CaseGenerator.GenerateCases(parser.Letters.Count);
            sknf = SknfSdnfFinder.FindSKNF(cases, parser, reformedFormula, out _);
            result = _minimizer.MinimizeSKNFByKarnaugh(sknf);
            Assert.AreEqual(expected, result);
        }
    }
}