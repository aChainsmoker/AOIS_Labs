namespace AssociativeProcessor;

public static class IOSystem
{
    private static bool _exitState;
    private static void PrintMatrix(MatrixProcessor processor)
    {
        for (int i = 0; i < processor.Matrix.GetLength(0); i++)
        {
            for (int j = 0; j < processor.Matrix.GetLength(1); j++)
            {
                Console.Write(processor.Matrix[i, j] + " ");
            }
            Console.WriteLine();
        }
    }

    private static void PrintWord(MatrixProcessor processor, int wordIndex)
    {
        int[] word = processor.ReturnWord(wordIndex);
        foreach (int i in word)
            Console.Write(i + " ");
    }
    
    private static void PrintAdressColumn(MatrixProcessor processor, int columnIndex)
    {
        int[] adressColumn = processor.ReturnAdressColumn(columnIndex);
        foreach (int i in adressColumn)
            Console.Write(i + " ");
    }

    private static void PrintNearestUnderneath(MatrixProcessor processor, int[] searchArg)
    {
        int[] nearestUnder = processor.FindNearestUnderneath(searchArg);
        Console.WriteLine("Ближайшее снизу: " + string.Join("", nearestUnder));
    }

    private static void PrintNearestAbove(MatrixProcessor processor, int[] searchArg)
    {
        int[] nearestAbove = processor.FindNearestAbove(searchArg);
        Console.WriteLine("Ближайшее сверху: " + string.Join("", nearestAbove));
    }

    public static void Run(MatrixProcessor processor)
    {
        _exitState = false;
        while (true)
        {
            PrintMenu();
            string? input = Console.ReadLine();
            HandleInput(input, processor);
            if (_exitState)
                break;

        }
    }

    private static void PrintMenu()
    {
        
        Console.WriteLine("1. Показать матрицу");
        Console.WriteLine("2. Показать слово");
        Console.WriteLine("3. Записать слово");
        Console.WriteLine("4. Показать адресный столбец");
        Console.WriteLine("5. Записать адресный столбец");
        Console.WriteLine("6. Выполнить операцию И");
        Console.WriteLine("7. Выполнить операцию Шеффера");
        Console.WriteLine("8. Выполнить повторение первого аргумента");
        Console.WriteLine("9. Выполнить отрицание первого аргумента");
        Console.WriteLine("10. Найти ближайшее снизу");
        Console.WriteLine("11. Найти ближайшее сверху");
        Console.WriteLine("12. Суммировать поля слова");
        Console.WriteLine("0. Выход");
        Console.Write("Выберите действие: ");
    }

    private static void HandleInput(string input, MatrixProcessor processor)
    {
        switch (input)
        {
            case "1":
                PrintMatrix(processor);
                break;
            case "2":
                HandleWordOperation(processor);
                break;
            case "4":
                HandleColumnOperation(processor);
                break;
            case "5":
                HandleWriteColumn(processor);
                break;
            case "6":
                HandleConjunction(processor);
                break;
            case "7":
                HandleSheffer(processor);
                break;
            case "8":
                HandleRepeatFirstArgument(processor);
                break;
            case "9":
                HandleDenyFirstArgument(processor);
                break;
            case "10":
                HandleNearestUnderneath(processor);
                break;
            case "11":
                HandleNearestAbove(processor);
                break;
            case "12":
                HandleSumFields(processor);
                break;
            case "3":
                HandleWriteWord(processor);
                break;
            case "0":
                _exitState = true;
                break;
            default:
                Console.WriteLine("Неверный ввод!");
                break;
        }
    }


    private static void HandleWriteColumn(MatrixProcessor processor)
    {
        try
        {
            Console.Write("Введите индекс столбца (0-15): ");
            int index = int.Parse(Console.ReadLine()!);

            Console.WriteLine("Введите 16 бит через пробел:");
            var bits = Console.ReadLine()!.Split(' ').Select(int.Parse).ToArray();

            processor.SetAdressColumn(index, bits);
            Console.WriteLine("Столбец записан!");
        }
        catch
        {
            Console.WriteLine("Ошибка ввода!");
        }
    }
    private static void HandleWordOperation(MatrixProcessor processor)
    {
        Console.Write("Введите индекс слова (0-15): ");
        if (int.TryParse(Console.ReadLine(), out int index) && index >= 0 && index < 16)
        {
            PrintWord(processor, index);
        }
        else
        {
            Console.WriteLine("Неверный индекс!");
        }
    }

    private static void HandleColumnOperation(MatrixProcessor processor)
    {
        Console.Write("Введите индекс столбца (0-15): ");
        if (int.TryParse(Console.ReadLine(), out int index) && index >= 0 && index < 16)
        {
            PrintAdressColumn(processor, index);
        }
        else
        {
            Console.WriteLine("Неверный индекс!");
        }
    }

    private static void HandleConjunction(MatrixProcessor processor)
    {
        try
        {
            Console.Write("Индекс первого слова: ");
            int w1 = int.Parse(Console.ReadLine()!);
            Console.Write("Индекс второго слова: ");
            int w2 = int.Parse(Console.ReadLine()!);
            
            var result = processor.Conjunction(
                processor.ReturnWord(w1), 
                processor.ReturnWord(w2));
            
            Console.WriteLine("Результат И: " + string.Join(" ", result));
        }
        catch
        {
            Console.WriteLine("Ошибка ввода!");
        }
    }
    
    private static void HandleRepeatFirstArgument(MatrixProcessor processor)
    {
        try
        {
            Console.Write("Индекс первого слова: ");
            int w1 = int.Parse(Console.ReadLine()!);
            
            var result = processor.RepeatFirstArgument(
                processor.ReturnWord(w1));
            
            Console.WriteLine("Результат повторения первого аргумента: " + string.Join(" ", result));
        }
        catch
        {
            Console.WriteLine("Ошибка ввода!");
        }
    }
    
    private static void HandleDenyFirstArgument(MatrixProcessor processor)
    {
        try
        {
            Console.Write("Индекс первого слова: ");
            int w1 = int.Parse(Console.ReadLine()!);
            
            var result = processor.DenyFirstArgument(
                processor.ReturnWord(w1));
            
            Console.WriteLine("Результат отрицания первого аргумента: " + string.Join(" ", result));
        }
        catch
        {
            Console.WriteLine("Ошибка ввода!");
        }
    }

    private static void HandleSheffer(MatrixProcessor processor)
    {
        try
        {
            Console.Write("Индекс первого слова: ");
            int w1 = int.Parse(Console.ReadLine()!);
            Console.Write("Индекс второго слова: ");
            int w2 = int.Parse(Console.ReadLine()!);
            
            var result = processor.ShefferOperation(
                processor.ReturnWord(w1), 
                processor.ReturnWord(w2));
            
            Console.WriteLine("Результат Шеффера: " + string.Join(" ", result));
        }
        catch
        {
            Console.WriteLine("Ошибка ввода!");
        }
    }

    private static void HandleNearestUnderneath(MatrixProcessor processor)
    {
        Console.WriteLine("Введите аргумент поиска (16 бит через пробел):");
        var input = Console.ReadLine()!.Split(' ');
        int[] arg = input.Select(int.Parse).ToArray();
        PrintNearestUnderneath(processor, arg);
    }

    private static void HandleNearestAbove(MatrixProcessor processor)
    {
        Console.WriteLine("Введите аргумент поиска (16 бит через пробел):");
        var input = Console.ReadLine()!.Split(' ');
        int[] arg = input.Select(int.Parse).ToArray();
        PrintNearestAbove(processor, arg);
    }

    private static void HandleSumFields(MatrixProcessor processor)
    {
        Console.Write("Введите шаблон поиска (3 бита через пробел): ");
        var pattern = Console.ReadLine()!.Split(' ').Select(int.Parse).ToArray();
        processor.FindAndSumWord(pattern);
        Console.WriteLine("Операция выполнена!");
    }

    private static void HandleWriteWord(MatrixProcessor processor)
    {
        try
        {
            Console.Write("Введите индекс слова (0-15): ");
            int index = int.Parse(Console.ReadLine()!);
            
            Console.WriteLine("Введите 16 бит через пробел:");
            var bits = Console.ReadLine()!.Split(' ').Select(int.Parse).ToArray();
            
            processor.SetWord(index, bits);
            Console.WriteLine("Слово записано!");
        }
        catch
        {
            Console.WriteLine("Ошибка ввода!");
        }
    }
}