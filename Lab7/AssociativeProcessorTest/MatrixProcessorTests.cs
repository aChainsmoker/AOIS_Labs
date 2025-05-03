using Microsoft.VisualStudio.TestTools.UnitTesting;
using AssociativeProcessor;

namespace AssociativeProcessorTest;

[TestClass]
public sealed class MatrixProcessorTests
{
    [TestMethod]
    public void ReturnWord_ShouldReturnCorrectWord()
    {
        MatrixProcessor processor = new MatrixProcessor();
        int wordIndex = 0;
        int[] expectedWord = new int[16] { 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0 };
        processor.SetWord(wordIndex, expectedWord);

        int[] actualWord = processor.ReturnWord(wordIndex);

        CollectionAssert.AreEqual(expectedWord, actualWord);
    }

    [TestMethod]
    public void SetWord_ShouldSetCorrectWord()
    {
        MatrixProcessor processor = new MatrixProcessor();
        int wordIndex = 1;
        int[] wordToSet = new int[16] { 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1 };

        processor.SetWord(wordIndex, wordToSet);
        int[] retrievedWord = processor.ReturnWord(wordIndex);

        CollectionAssert.AreEqual(wordToSet, retrievedWord);
    }

    [TestMethod]
    public void ReturnAdressColumn_ShouldReturnCorrectColumn()
    {
        MatrixProcessor processor = new MatrixProcessor();
        int columnIndex = 3;
        int[] expectedColumn = new int[16] { 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0 };
        processor.SetAdressColumn(3, expectedColumn);

        int[] actualColumn = processor.ReturnAdressColumn(columnIndex);

        CollectionAssert.AreEqual(expectedColumn, actualColumn);
    }

    [TestMethod]
    public void Conjunction_ShouldReturnCorrectResult()
    {
        MatrixProcessor processor = new MatrixProcessor();
        int[] word1 = new int[16] { 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0 };
        int[] word2 = new int[16] { 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1 };
        int[] expected = new int[16] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

        int[] result = processor.Conjunction(word1, word2);

        CollectionAssert.AreEqual(expected, result);
    }

    [TestMethod]
    public void ShefferOperation_ShouldReturnCorrectResult()
    {
        MatrixProcessor processor = new MatrixProcessor();
        int[] word1 = new int[16] { 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0 };
        int[] word2 = new int[16] { 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1 };
        int[] expected = new int[16] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };

        int[] result = processor.ShefferOperation(word1, word2);

        CollectionAssert.AreEqual(expected, result);
    }

    [TestMethod]
    public void RepeatFirstArgument_ShouldReturnSameWord()
    {
        MatrixProcessor processor = new MatrixProcessor();
        int[] word1 = new int[16] { 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0 };

        int[] result = processor.RepeatFirstArgument(word1);

        CollectionAssert.AreEqual(word1, result);
    }

    [TestMethod]
    public void DenyFirstArgument_ShouldReturnNegatedWord()
    {
        MatrixProcessor processor = new MatrixProcessor();
        int[] word1 = new int[16] { 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0 };
        int[] expected = new int[16] { 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1 };

        int[] result = processor.DenyFirstArgument(word1);

        CollectionAssert.AreEqual(expected, result);
    }

    [TestMethod]
    public void FindAndSumWord_ShouldUpdateMatchingWords()
    {
        MatrixProcessor processor = new MatrixProcessor();
        int[] searchPattern = new int[] { 1, 0, 1 };
        int[] word0 = new int[16] { 1, 0, 1, 0, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0 };
        int[] word1 = new int[16] { 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        processor.SetWord(0, word0);
        processor.SetWord(1, word1);

        processor.FindAndSumWord(searchPattern);

        int[] updatedWord0 = processor.ReturnWord(0);
        int[] expectedWord0 = new int[16] { 1, 0, 1, 0, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 1, 1 };
        CollectionAssert.AreEqual(expectedWord0, updatedWord0);

        int[] updatedWord1 = processor.ReturnWord(1);
        CollectionAssert.AreEqual(word1, updatedWord1);
    }

    [TestMethod]
    public void FindNearestUnderneath_ShouldReturnCorrectWord()
    {
        MatrixProcessor processor = new MatrixProcessor();
        int[] searchArgument = new int[16] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0 };
        int[] word0 = new int[16] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 };
        int[] word1 = new int[16] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1 };
        processor.SetWord(0, word0);
        processor.SetWord(1, word1);

        int[] result = processor.FindNearestUnderneath(searchArgument);

        CollectionAssert.AreEqual(word0, result);
    }

    [TestMethod]
    public void FindNearestAbove_ShouldReturnCorrectWord()
    {
        MatrixProcessor processor = new MatrixProcessor();
        int[] searchArgument = new int[16] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0 };
        int[] word0 = new int[16] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 };
        int[] word1 = new int[16] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1 };
        processor.SetWord(0, word0);
        processor.SetWord(1, word1);

        int[] result = processor.FindNearestAbove(searchArgument);

        CollectionAssert.AreEqual(word1, result);
    }
}