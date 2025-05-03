using HashTableNS;

[TestClass]
public class IOSystemTests
{
    private HashTable _hashTable;
    private StringWriter consoleOutput;
    private StringReader consoleInput;

    [TestInitialize]
    public void Initialize()
    {
        _hashTable = new HashTable();
        consoleOutput = new StringWriter();
        consoleInput = new StringReader(String.Empty);
        Console.SetOut(consoleOutput);
    }

    [TestCleanup]
    public void Cleanup()
    {
        consoleOutput.Dispose();
        consoleInput.Dispose();
    }

    [TestMethod]
    public void Run_InsertAndSearch_ShouldInsertAndFindCorrectly()
    {
        string input = "1\nМосква\nСтолица России\n2\nМосква\n0\n";
        consoleInput = new StringReader(input);
        Console.SetIn(consoleInput);

        IOSystem.Run(_hashTable);

        string output = consoleOutput.ToString();
        Assert.IsTrue(consoleOutput.ToString().Contains("Элемент вставлен."));
        Assert.IsTrue(consoleOutput.ToString().Contains("Найдено: Столица России"));
    }

    [TestMethod]
    public void Run_UpdateExistingKey_ShouldUpdateCorrectly()
    {
        string input = "1\nМосква\nСтолица России\n3\nМосква\nMoscow\n2\nМосква\n0\n";
        consoleInput = new StringReader(input);
        Console.SetIn(consoleInput);

        IOSystem.Run(_hashTable);

        string output = consoleOutput.ToString();
        Assert.IsTrue(output.Contains("Элемент вставлен."));
        Assert.IsTrue(output.Contains("Данные обновлены."));
        Assert.IsTrue(output.Contains("Найдено: Moscow"));
    }

    [TestMethod]
    public void Run_DeleteExistingKey_ShouldDeleteCorrectly()
    {
        string input = "1\nМосква\nСтолица России\n4\nМосква\n2\nМосква\n0\n";
        consoleInput = new StringReader(input);
        Console.SetIn(consoleInput);

        IOSystem.Run(_hashTable);

        string output = consoleOutput.ToString();
        Assert.IsTrue(output.Contains("Элемент вставлен."));
        Assert.IsTrue(output.Contains("Элемент удален."));
        Assert.IsTrue(output.Contains("Ключ не найден."));
    }

    [TestMethod]
    public void Run_PrintTable_ShouldPrintCorrectly()
    {
        string input = "1\nМосква\nСтолица России\n5\n0\n";
        consoleInput = new StringReader(input);
        Console.SetIn(consoleInput);

        IOSystem.Run(_hashTable);

        string output = consoleOutput.ToString();
        Assert.IsTrue(output.Contains("Элемент вставлен."));
        Assert.IsTrue(output.Contains("[V:"));
        Assert.IsTrue(output.Contains("h(V):"));
        Assert.IsTrue(output.Contains("ID: Москва"));
        Assert.IsTrue(output.Contains("Столица России"));
    }
}