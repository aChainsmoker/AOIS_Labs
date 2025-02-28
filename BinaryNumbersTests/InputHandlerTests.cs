namespace BinaryNumbersTests;

using Lab1;

[TestClass]
public class InputHandlerTests
{
    [TestMethod]
    public void InputHandlerIntSum_Test()
    {
        Console.SetIn(new StringReader("2 + 2\nexit"));
        StringWriter stringWriter = new StringWriter();
        Console.SetOut(stringWriter);
        UserInputHandler inputHandler = new UserInputHandler();
        
        StringAssert.Contains(stringWriter.ToString(), "Результат в двоичной системе счисления: 00000000000000000000000000000100");
    }
    [TestMethod]
    public void InputHandlerFloatSum_Test()
    {
        Console.SetIn(new StringReader("2,3 + 2,75\nexit"));
        StringWriter stringWriter = new StringWriter();
        Console.SetOut(stringWriter);
        UserInputHandler inputHandler = new UserInputHandler();

        StringAssert.Contains(stringWriter.ToString(), "Результат в двоичной системе счисления: 0 1000000 101000011001100110011001");
    }

    [TestMethod]
    public void InputHandlerSubtract_Test()
    {
        Console.SetIn(new StringReader("5 - 3\nexit"));
        StringWriter stringWriter = new StringWriter();
        Console.SetOut(stringWriter);
        UserInputHandler inputHandler = new UserInputHandler();
        
        StringAssert.Contains(stringWriter.ToString(), "Результат в двоичной системе счисления: 00000000000000000000000000000010");
    }

    [TestMethod]
    public void InputHandlerMultiply_Test()
    {
        Console.SetIn(new StringReader("12 * -3\nexit"));
        StringWriter stringWriter = new StringWriter();
        Console.SetOut(stringWriter);
        UserInputHandler inputHandler = new UserInputHandler();
        
        StringAssert.Contains(stringWriter.ToString(), "Результат в двоичной системе счисления: 10000000000000000000000000100100");
    }

    [TestMethod]
    public void InputHandlerDivide_Test()
    {
        Console.SetIn(new StringReader("12 / -4\nexit"));
        StringWriter stringWriter = new StringWriter();
        Console.SetOut(stringWriter);
        UserInputHandler inputHandler = new UserInputHandler();
        
        StringAssert.Contains(stringWriter.ToString(), "Результат в двоичной системе счисления: 1000000000000010,1111111111111111");
    }

    [TestMethod]
    public void ErrorHandlingTest()
    {
        Console.SetIn(new StringReader("12-4\nexit"));
        StringWriter stringWriter = new StringWriter();
        Console.SetOut(stringWriter);
        UserInputHandler inputHandler = new UserInputHandler();

        StringAssert.Contains(stringWriter.ToString(), "Incorrect input. Try again.");
    }
}