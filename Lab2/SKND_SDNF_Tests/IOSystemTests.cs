using SKNF_SDNF;

namespace SKNF_SDNF_Tests
{
    [TestClass]
    public class IOSystemTests
    {
        private readonly InputParser _parser = new();

        [TestMethod]
        public void Test_PrintNumberFormSknf_PrintsExpectedOutput()
        {
            var numberForm = new List<int> { 1, 2, 3 };
            string expectedOutput = "Числовая форма СКНФ: (1, 2, 3) /\\";

            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);
                IOSystem.PrintNumberFormSknf(numberForm);
                string output = sw.ToString().TrimEnd();
                Assert.IsTrue(output.Contains(expectedOutput), "Вывод PrintNumberFormSknf не соответствует ожидаемому.");
            }
        }

        [TestMethod]
        public void Test_PrintNumberFormSdnf_PrintsExpectedOutput()
        {
            var numberForm = new List<int> { 4, 5, 6 };
            string expectedOutput = "Числовая форма СДНФ: (4, 5, 6) \\/";

            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);
                IOSystem.PrintNumberFormSdnf(numberForm);
                string output = sw.ToString().TrimEnd();
                Assert.IsTrue(output.Contains(expectedOutput), "Вывод PrintNumberFormSdnf не соответствует ожидаемому.");
            }
        }

        [TestMethod]
        public void Test_PrintSknf_PrintsExpectedOutput()
        {
            string sknf = "TestSknf";
            string expectedOutput = "СКНФ: " + sknf;

            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);
                IOSystem.PrintSknf(sknf);
                string output = sw.ToString().Trim();
                Assert.IsTrue(output.StartsWith(expectedOutput), "Вывод PrintSknf не соответствует ожидаемому.");
            }
        }

        [TestMethod]
        public void Test_PrintSdnf_PrintsExpectedOutput()
        {
            string sdnf = "TestSdnf";
            string expectedOutput = "СДНФ: " + sdnf;

            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);
                IOSystem.PrintSdnf(sdnf);
                string output = sw.ToString().Trim();
                Assert.IsTrue(output.StartsWith(expectedOutput), "Вывод PrintSdnf не соответствует ожидаемому.");
            }
        }

        [TestMethod]
        public void Test_PrintIndexForm_PrintsExpectedOutput()
        {
            int indexForm = 5;
            string indexBinaryForm = "101";
            string expectedOutput = "Индексная форма: 5 = 101";

            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);
                IOSystem.PrintIndexForm(indexForm, indexBinaryForm);
                string output = sw.ToString().Trim();
                Assert.IsTrue(output.Contains(expectedOutput), "Вывод PrintIndexForm не соответствует ожидаемому.");
            }
        }

        [TestMethod]
        public void Test_TakeTheInput_ReturnsInputValue()
        {
            string inputValue = "TestInput";
            using (StringReader sr = new StringReader(inputValue + Environment.NewLine))
            using (StringWriter sw = new StringWriter())
            {
                Console.SetIn(sr);
                Console.SetOut(sw);
                string result = IOSystem.TakeTheInput();
                Assert.AreEqual(inputValue, result, "Метод TakeTheInput не вернул ожидаемое значение.");
            }
        }

        [TestMethod]
        public void Test_FormTable_PrintsExpectedTable()
        {
            string reformedFormula = _parser.ReformFormula("!(!(A|B))~C");
            List<List<int>> cases = CaseGenerator.GenerateCases(_parser.Letters.Count);

            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);
                IOSystem.FormTable(cases, _parser.FindSubformulas(reformedFormula), reformedFormula, _parser);
                string output = sw.ToString();
                Assert.IsTrue(output.Contains(" A    |    B    |    C    |    A|B    |    !(A|B)    |    !(!(A|B))    |    (!(!(A|B)))~C    |"));
                Assert.IsTrue(output.Contains(" 0    |    0    |    0    |     0     |       1      |        0        |          1          |"));
                Assert.IsTrue(output.Contains(" 1    |    1    |    0    |     1     |       0      |        1        |          0          |"));
            }
        }
    }
}