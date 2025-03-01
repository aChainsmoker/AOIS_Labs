using Lab1;

namespace BinaryNumbersTests;


[TestClass]
public sealed class OperationsTests
{
    [TestMethod]
    public void TranslationToBinaryTest()
    {
        BinaryNumber number = new BinaryNumber();
        number.ConvertToStraightBinary(5);
        Assert.AreEqual(5, number.GetDecimal());
        number.ConvertToAdditionalBinary(-5);
        Assert.AreEqual(-5, number.GetDecimal());
        number.ConvertToReverseBinary(-5);
        Assert.AreEqual(-5, number.GetDecimal());
    }

    [TestMethod]
    public void SumTest()
    {
        BinaryNumber number1 = new BinaryNumber();
        number1.ConvertToAdditionalBinary(4);
        
        BinaryNumber number2 = new BinaryNumber();
        number2.ConvertToAdditionalBinary(18);
        
        BinaryNumber number3 = number1 + number2;
        
        Assert.AreEqual(22, number3.GetDecimal());
    }

    [TestMethod]
    public void SubtractionTest()
    {
        BinaryNumber number1 = new BinaryNumber();
        number1.ConvertToAdditionalBinary(4);
        
        BinaryNumber number2 = new BinaryNumber();
        number2.ConvertToAdditionalBinary(-18);
        
        BinaryNumber number3 = number1 + number2;
        
        Assert.AreEqual(-14, number3.GetDecimal());
    }
    [TestMethod]
    public void MultiplicationTest()
    {
        BinaryNumber number1 = new BinaryNumber();
        number1.ConvertToStraightBinary(4);
        
        BinaryNumber number2 = new BinaryNumber();
        number2.ConvertToStraightBinary(-18);
        
        BinaryNumber number3 = number1 * number2;
        
        Assert.AreEqual(-72, number3.GetDecimal());
    }

    [TestMethod]
    public void DivisionTest()
    {
        BinaryNumber number1 = new BinaryNumber();
        number1.ConvertToStraightBinary(4);
        
        BinaryNumber number2 = new BinaryNumber();
        number2.ConvertToStraightBinary(-16);
        
        FixedPointBinaryNumber number3 = number1 / number2;
        
        Assert.AreEqual(-0.25, number3.GetDecimal());
    }
    
}