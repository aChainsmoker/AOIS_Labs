using Lab1;

namespace BinaryNumbersTests;

[TestClass]
public sealed class FixedPointBinaryTests
{
    [TestMethod]
    public void ConvertionTest()
    {
        FixedPointBinaryNumber number = new FixedPointBinaryNumber();
        number.ConvertToStraightBinary(5.6f);
        Assert.AreEqual(5.6f, number.GetDecimal(), 0.0001f);
        
        number.ConvertToStraightBinary(-3.4f);
        Assert.AreEqual(-3.4f, number.GetDecimal(), 0.0001f);
    }
}

