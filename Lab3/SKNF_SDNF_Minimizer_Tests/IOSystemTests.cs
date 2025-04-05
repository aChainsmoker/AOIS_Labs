namespace SKNF_SDNF_Minimizer.Tests
{
    [TestClass]
    public class IOSystemTests
    {
        [TestMethod]
        public void PrintSDNFCalculatingMethodTest()
        {
            string formula = "(!a/\\c)\\/(b/\\!c)";
            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);
                IOSystem.PrintSDNFCalculatingMethod(formula);
                string output = sw.ToString();
                Assert.IsTrue(output.Contains("Минимизированная СДНФ расчётным методом:"));
                Assert.IsTrue(output.Contains(formula));
            }
        }

        [TestMethod]
        public void PrintSKNFCalculatingMethodTest()
        {
            string formula = "(!a/\\!c)\\/(b/\\c)";
            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);
                IOSystem.PrintSKNFCalculatingMethod(formula);
                string output = sw.ToString();
                Assert.IsTrue(output.Contains("Минимизированная СКНФ расчётным методом:"));
                Assert.IsTrue(output.Contains(formula));
            }
        }

        [TestMethod]
        public void PrintSDNFTableCalculatingMethodTest()
        {
            string formula = "(!a/\\c)\\/(b/\\!c)";
            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);
                IOSystem.PrintSDNFTableCalculatingMethod(formula);
                string output = sw.ToString();
                Assert.IsTrue(output.Contains("Минимизированная СДНФ расчётно-табличным методом:"));
                Assert.IsTrue(output.Contains(formula));
            }
        }

        [TestMethod]
        public void PrintSKNFTableCalculatingMethodTest()
        {
            string formula = "(!a/\\!c)\\/(b/\\c)";
            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);
                IOSystem.PrintSKNFTableCalculatingMethod(formula);
                string output = sw.ToString();
                Assert.IsTrue(output.Contains("Минимизированная СКНФ расчётно-табличным методом:"));
                Assert.IsTrue(output.Contains(formula));
            }
        }

        [TestMethod]
        public void PrintSDNFTableMethodTest()
        {
            string formula = "(!a/\\c)\\/(b/\\!c)";
            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);
                IOSystem.PrintSDNFTableMethod(formula);
                string output = sw.ToString();
                Assert.IsTrue(output.Contains("Минимизированная СДНФ табличным методом:"));
                Assert.IsTrue(output.Contains(formula));
            }
        }

        [TestMethod]
        public void PrintSKNFTableMethodTest()
        {
            string formula = "(!a/\\!c)\\/(b/\\c)";
            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);
                IOSystem.PrintSKNFTableMethod(formula);
                string output = sw.ToString();
                Assert.IsTrue(output.Contains("Минимизированная СКНФ табличным методом:"));
                Assert.IsTrue(output.Contains(formula));
            }
        }
        
        [TestMethod]
        public void TakeTheInputTest()
        {
            string expectedInput = "(!a/\\!b/\\c)\\/(!a/\\b/\\!c)";
            using (StringReader sr = new StringReader(expectedInput + "\n"))
            {
                Console.SetIn(sr);
                string result = IOSystem.TakeTheInput();
                Assert.AreEqual(expectedInput, result);
            }
        }
    }
}