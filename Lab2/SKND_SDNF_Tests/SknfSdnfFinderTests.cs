using SKNF_SDNF;

namespace SKND_SDNF_Tests
{
    [TestClass]
    public class SknfSdnfFinderTests
    {
        private readonly InputParser _parser = new();

        [TestMethod]
        public void FindSKNF_ForConjunction_ReturnsCorrectSKNF()
        {
            string formula = "((a|b)&!c)";
            
            var cases = CaseGenerator.GenerateCases(3);
            string reformedFormula = _parser.ReformFormula(formula); 

            string sknf = SknfSdnfFinder.FindSKNF(cases, _parser, reformedFormula, out var numberForm);

            var expectedNumbers = new List<int> { 0, 1, 3, 5, 7 };
            string expectedSKNF = "(a\\/b\\/c)/\\(a\\/b\\/!c)/\\(a\\/!b\\/!c)/\\(!a\\/b\\/!c)/\\(!a\\/!b\\/!c)";
            Assert.AreEqual(expectedSKNF, sknf);
            CollectionAssert.AreEqual(expectedNumbers, numberForm);
        }

        [TestMethod]
        public void ExtractBinaryForm_ForTautology_ReturnsAllOnes()
        {
            _parser.ReformFormula("a|!a");
            var cases = CaseGenerator.GenerateCases(1);
            var binary = SknfSdnfFinder.ExtractBinaryForm(cases, _parser, "a!a|");
            CollectionAssert.AreEqual(new List<int> { 1, 1 }, binary);
        }

        [TestMethod]
        public void FindSDNF_SimpleDisjunction_ReturnsCorrectSDNF()
        {
            string formula = "!(!(A|B))~C";
            string reformedFormula = _parser.ReformFormula(formula);
            var cases = CaseGenerator.GenerateCases(3);

            string sdnf = SknfSdnfFinder.FindSDNF(cases, _parser, reformedFormula, out var numberForm);

            string expectedSDNF = "(!A/\\!B/\\!C)\\/(!A/\\B/\\C)\\/(A/\\!B/\\C)\\/(A/\\B/\\C)";
            List<int> expectedNumbers = new List<int> { 0,3,5,7 };

            Assert.AreEqual(expectedSDNF, sdnf);

            CollectionAssert.AreEqual(expectedNumbers, numberForm);
        }
    }
}
