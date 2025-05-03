using System.Globalization;

namespace Lab1;

public class UserInputHandler
{
    public UserInputHandler()
    {
        LaunchInputHandler();
    }

    private void LaunchInputHandler()
    {
        string? input = String.Empty;
        while (true)
        {
            Console.WriteLine("Введите выражение");
            input = Console.ReadLine();
            if(input == "exit") break;
            try
            {
                if (input != null) HandleInput(input);
            }
            catch (Exception e)
            {
                Console.WriteLine("Incorrect input. Try again.");
                if (Console.IsOutputRedirected) continue;
                Console.ReadKey();
                Console.Clear();
            }
        }
        
    }

    private void HandleInput(string input)
    {
        string[] inputParts = input.Split(' ');
        
        if(inputParts.Length != 3)
            throw new Exception("Wrong amount of parameters");
        
        string? solutionBinary;
        string solutionDecimal;
        
        switch (inputParts[1])
        {
            case "+":
                if (inputParts[0].Contains(',') || inputParts[2].Contains(','))
                {
                    FloatBinaryNumber number1 = new FloatBinaryNumber();
                    number1.ConvertToStraightBinary(Convert.ToSingle(inputParts[0]));
        
                    FloatBinaryNumber number2 = new FloatBinaryNumber();
                    number2.ConvertToStraightBinary(Convert.ToSingle(inputParts[2]));
        
                    FloatBinaryNumber number3 = number1 + number2;
                    solutionBinary = string.Join("", number3.Number);
                    solutionBinary = solutionBinary.Insert(1, " ");
                    solutionBinary = solutionBinary.Insert(9, " ");
                    solutionDecimal = number3.GetDecimal().ToString(CultureInfo.CurrentCulture);
                }
                else
                {
                    BinaryNumber number1 = new BinaryNumber();
                    number1.ConvertToAdditionalBinary(Convert.ToInt32(inputParts[0]));
        
                    BinaryNumber number2 = new BinaryNumber();
                    number2.ConvertToAdditionalBinary(Convert.ToInt32(inputParts[2]));
        
                    BinaryNumber number3 = number1 + number2;
                    solutionBinary = string.Join("", number3.Number);
                    solutionDecimal = number3.GetDecimal().ToString();
                }
                break;
            case "-":
                BinaryNumber number4 = new BinaryNumber();
                number4.ConvertToAdditionalBinary(Convert.ToInt32(inputParts[0]));
        
                BinaryNumber number5 = new BinaryNumber();
                number5.ConvertToAdditionalBinary(Convert.ToInt32(inputParts[2]));
                
                BinaryNumber number6 = number4 - number5;
                solutionBinary = string.Join("", number6.Number);
                solutionDecimal = number6.GetDecimal().ToString();
                break;
            case "*":
                BinaryNumber number7 = new BinaryNumber();
                number7.ConvertToStraightBinary(Convert.ToInt32(inputParts[0]));
                BinaryNumber number8 = new BinaryNumber();
                number8.ConvertToStraightBinary(Convert.ToInt32(inputParts[2]));
                BinaryNumber number9 = number7 * number8;
                solutionBinary = string.Join("", number9.Number);
                solutionDecimal = number9.GetDecimal().ToString();
                break;
            case "/":
                BinaryNumber number10 = new BinaryNumber();
                number10.ConvertToStraightBinary(Convert.ToInt32(inputParts[0]));
                BinaryNumber number11 = new BinaryNumber();
                number11.ConvertToStraightBinary(Convert.ToInt32(inputParts[2]));
                FixedPointBinaryNumber number12 = number10 / number11;
                solutionBinary = string.Join("", number12.Number);
                solutionBinary = solutionBinary.Insert(16, ",");
                solutionDecimal = number12.GetDecimal().ToString();
                break;
            default:
                return;
        }

        ShowResult(solutionBinary, solutionDecimal);
    }

    private void ShowResult(string resultBinary, string resultDecimal)
    {
        Console.WriteLine($"Результат в двоичной системе счисления: {resultBinary}");
        Console.WriteLine($"Результат в десятичной системе счисления: {resultDecimal}");
        if (Console.IsOutputRedirected) return;
        else
        {
            Console.ReadKey();
            Console.Clear();
        }
        
    }
}