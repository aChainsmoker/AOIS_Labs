namespace BinaryNumbersTests;
using Lab1;

[TestClass]
public class FloatBinaryNumberTests
{
    [TestMethod]
    public void ConvertToStraightBinaryTest()
    {
        FloatBinaryNumber number = new FloatBinaryNumber();
        number.ConvertToStraightBinary(5.6f);
        Assert.AreEqual(5.6f, number.GetDecimal(), 0.0001f);
    }

    [TestMethod]
    public void SumTest()
    {
        FloatBinaryNumber number1 = new FloatBinaryNumber();
        number1.ConvertToStraightBinary(5.6f);
        FloatBinaryNumber number2 = new FloatBinaryNumber();
        number2.ConvertToStraightBinary(4.4f);
        FloatBinaryNumber number3 = number1 + number2;
        Assert.AreEqual(10f, number3.GetDecimal(), 0.0001f);
    }
}