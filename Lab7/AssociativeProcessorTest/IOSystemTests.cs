using AssociativeProcessor;

namespace AssociativeProcessorTest;

[TestClass]
public class IOSystemTests
{
    private StringWriter _output;

    [TestCleanup]
    public void Cleanup()
    {
        Console.SetOut(new StreamWriter(Console.OpenStandardOutput()) { AutoFlush = true });
        Console.SetIn(new StreamReader(Console.OpenStandardInput()));
    }

    private void RunWithInput(string input)
    {
        _output = new StringWriter();
        Console.SetOut(_output);
        Console.SetIn(new StringReader(input));

        var processor = new TestableMatrixProcessor();
        IOSystem.Run(processor);
    }

    [TestMethod]
    public void TestPrintMatrix()
    {
        RunWithInput("1\n0\n");
        string result = _output.ToString();
        Assert.IsTrue(result.Contains("0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0"), "Matrix first line incorrect");
        Assert.IsTrue(result.Contains("1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1"), "Matrix second line incorrect");
    }

    [TestMethod]
    public void TestPrintWord()
    {
        RunWithInput("2\n1\n0\n");
        string result = _output.ToString();
        Assert.IsTrue(result.Contains("1 0 1 0 1 0 1 0 1 0 1 0 1 0 1 0"), "PrintWord output incorrect");
    }

    [TestMethod]
    public void TestConjunction()
    {
        RunWithInput("6\n1\n2\n0\n");
        string result = _output.ToString();
        Assert.IsTrue(result.Contains("Результат И: 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0"), "Conjunction output incorrect");
    }

    [TestMethod]
    public void TestShefferOperation()
    {
        RunWithInput("7\n1\n2\n0\n");
        string result = _output.ToString();
        Assert.IsTrue(result.Contains("Результат Шеффера: 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1"), "Sheffer output incorrect");
    }

    [TestMethod]
    public void TestRepeatFirstArgument()
    {
        RunWithInput("8\n1\n0\n");
        string result = _output.ToString();
        Assert.IsTrue(result.Contains("Результат повторения первого аргумента: 1 0 1 0 1 0 1 0 1 0 1 0 1 0 1 0"), "RepeatFirstArgument output incorrect");
    }

    [TestMethod]
    public void TestDenyFirstArgument()
    {
        RunWithInput("9\n1\n0\n");
        string result = _output.ToString();
        Assert.IsTrue(result.Contains("Результат отрицания первого аргумента: 0 1 0 1 0 1 0 1 0 1 0 1 0 1 0 1"), "DenyFirstArgument output incorrect");
    }

    [TestMethod]
    public void TestSumFieldsOperation()
    {
        RunWithInput("12\n1 0 1\n0\n");
        string result = _output.ToString();
        Assert.IsTrue(result.Contains("Операция выполнена!"), "SumFields operation did not confirm execution");
    }

    [TestMethod]
    public void TestHandleColumnOperation()
    {
        RunWithInput("4\n1\n0\n");
        string result = _output.ToString();
        Assert.IsTrue(result.Contains("1 0 1 0 1 0 1 0 1 0 1 0 1 0 1 0"), "HandleColumnOperation output incorrect");
    }

    [TestMethod]
    public void TestHandleWriteColumnAndReadBack()
    {
        string bits = "0 1 1 0 1 0 1 0 1 0 1 0 1 0 1 0";
        RunWithInput(
            "5\n" +
            "2\n" +
            bits + "\n" +
            "4\n" +
            "2\n" +
            "0\n"
        );

        string result = _output.ToString();
        Assert.IsTrue(result.Contains("Столбец записан!"), "WriteColumn did not confirm write");
        Assert.IsTrue(result.Contains(bits), "Written column bits do not match on readback");
    }

    [TestMethod]
    public void TestNearestUnderneath()
    {
        string arg = string.Join(" ", new string[16].Select(_ => "1"));
        RunWithInput("10\n" + arg + "\n0\n");
        string result = _output.ToString();
        Assert.IsTrue(result.Contains("Ближайшее снизу: 1010101010101010"), "NearestUnderneath output incorrect");
    }

    [TestMethod]
    public void TestNearestAbove()
    {
        string arg = string.Join(" ", new string[16].Select(_ => "0"));
        RunWithInput("11\n" + arg + "\n0\n");
        string result = _output.ToString();
        Assert.IsTrue(result.Contains("Ближайшее сверху: 0101010101010101"), "NearestAbove output incorrect");
    }

    [TestMethod]
    public void TestHandleWriteWordAndReadBack()
    {
        string bits = "1 1 0 0 1 1 0 0 1 1 0 0 1 1 0 0";
        RunWithInput(
            "3\n" +
            "2\n" +
            bits + "\n" +
            "2\n" +
            "2\n" +
            "0\n"
        );

        string result = _output.ToString();
        Assert.IsTrue(result.Contains("Слово записано!"), "WriteWord did not confirm write");
        Assert.IsTrue(result.Contains(bits), "Written word bits do not match on readback");
    }

    private class TestableMatrixProcessor : MatrixProcessor
    {
        public TestableMatrixProcessor()
        {
            for (int i = 0; i < 16; i++)
                for (int j = 0; j < 16; j++)
                    Matrix[i, j] = i % 2;
        }
    }
}