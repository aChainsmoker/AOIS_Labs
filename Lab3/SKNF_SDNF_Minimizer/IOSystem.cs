namespace SKNF_SDNF_Minimizer;

public static class IOSystem
{
    public static void PrintSDNFTable(List<List<string>> parsedFormulas, List<List<string>> minimizedFormulas)
    {
        
        List<string> longestList = (minimizedFormulas.OrderByDescending(innerList => innerList.Sum(s => s.Length)).ThenByDescending(innerList => innerList.Count).First());
        int longestListLengthWithSymbols = string.Join("/\\", longestList).Length;
        Console.Write(new string(' ', longestListLengthWithSymbols) + '|');
        foreach (var parsedFormula in parsedFormulas)
        {
            Console.Write(string.Join("/\\", parsedFormula)+'|');
        }
        Console.WriteLine();
        foreach (var minimizedFormula in minimizedFormulas)
        {
            Console.Write(string.Join("/\\", minimizedFormula)+new string(' ', longestListLengthWithSymbols - string.Join("/\\", minimizedFormula).Length) + '|');
            foreach (var parsedFormula in parsedFormulas)
            {
                Console.Write(new string(' ', string.Join("/\\", parsedFormula).Length/2) + Convert.ToInt32(minimizedFormula.All(item => parsedFormula.Contains(item))) + new string(' ', (string.Join("/\\", parsedFormula).Length%2 != 0? string.Join("/\\", parsedFormula).Length/2 : string.Join("/\\", parsedFormula).Length/2-1)) + '|');
            }
            Console.WriteLine();
        }
    }
    public static void PrintSKNFTable(List<List<string>> parsedFormulas, List<List<string>> minimizedFormulas)
    {
        List<string> longestList = (minimizedFormulas.OrderByDescending(innerList => innerList.Sum(s => s.Length)).ThenByDescending(innerList => innerList.Count).First());
        int longestListLengthWithSymbols = string.Join("/\\", longestList).Length;
        Console.Write(new string(' ', longestListLengthWithSymbols) + '|');
        foreach (var parsedFormula in parsedFormulas)
        {
            Console.Write(string.Join("\\/", parsedFormula)+'|');
        }
        Console.WriteLine();
        foreach (var minimizedFormula in minimizedFormulas)
        {
            Console.Write(string.Join("\\/", minimizedFormula)+new string(' ', longestListLengthWithSymbols - string.Join("/\\", minimizedFormula).Length) + '|');
            foreach (var parsedFormula in parsedFormulas)
            {
                Console.Write(new string(' ', string.Join("/\\", parsedFormula).Length/2) + Convert.ToInt32(minimizedFormula.All(item => parsedFormula.Contains(item))) + new string(' ', (string.Join("/\\", parsedFormula).Length%2 != 0? string.Join("/\\", parsedFormula).Length/2 : string.Join("/\\", parsedFormula).Length/2-1)) + '|');
            }
            Console.WriteLine();
        }
    }
    
    public static void PrintKarnaughMap(int[,] kMap)
    {
        // Определяем размеры массива
        int rows = kMap.GetLength(0);
        int cols = kMap.GetLength(1);

        // Проверяем, поддерживается ли размер карты
        if (!((rows == 1 && cols == 2) || (rows == 2 && cols == 2) || 
              (rows == 2 && cols == 4) || (rows == 4 && cols == 4)))
        {
            throw new ArgumentException("Неподдерживаемый размер карты Карно");
        }

        Console.WriteLine("Карта Карно:");

        // Генерируем заголовки для столбцов
        string[] colHeaders = GenerateColumnHeaders(cols);
        Console.Write("       ");
        foreach (var header in colHeaders)
        {
            Console.Write($"{header,4}");
        }
        Console.WriteLine();

        // Генерируем заголовки для строк
        string[] rowHeaders = GenerateRowHeaders(rows);

        // Выводим каждую строку карты с заголовком строки
        for (int i = 0; i < rows; i++)
        {
            Console.Write($"{rowHeaders[i],4} |");
            for (int j = 0; j < cols; j++)
            {
                Console.Write($"{kMap[i, j],4}");
            }
            Console.WriteLine();
        }
    }

// Вспомогательный метод для генерации заголовков столбцов
    private static string[] GenerateColumnHeaders(int cols)
    {
        if (cols == 2)
            return new[] { "0", "1" }; // Для 1 или 2 переменных
        else if (cols == 4)
            return new[] { "00", "01", "11", "10" }; // Для 3 или 4 переменных (Греевский код)
        else
            throw new ArgumentException("Неподдерживаемое количество столбцов");
    }

// Вспомогательный метод для генерации заголовков строк
    private static string[] GenerateRowHeaders(int rows)
    {
        if (rows == 1)
            return new[] { "" }; // Для 1 переменной заголовок строки не нужен
        else if (rows == 2)
            return new[] { "0", "1" }; // Для 2 или 3 переменных
        else if (rows == 4)
            return new[] { "00", "01", "11", "10" }; // Для 4 переменных (Греевский код)
        else
            throw new ArgumentException("Неподдерживаемое количество строк");
    }

    public static int ChooseFormulaType()
    {
        Console.WriteLine("Choose formula type:\n 1.SDNF\n 2.SKNF");
        
        int formulaType = -1;
        while (true)
        {
            try
            {
                formulaType = Convert.ToInt32(Console.ReadLine());
            }
            catch (Exception e)
            {
                Console.WriteLine("Type 1 or 2!");
            }

            if (formulaType == 1 || formulaType == 2)
                return formulaType;
            else
                Console.WriteLine("Type 1 or 2!");
        }
    }

    public static string TakeTheInput()
    {
        Console.WriteLine("Введите формулу: ");
        return Console.ReadLine();
    }

    public static void PrintSDNFCalculatingMethod(string formula)
    {
        Console.WriteLine($"Минимизированная СДНФ расчётным методом: {formula}\n");
    }
    
    public static void PrintSKNFCalculatingMethod(string formula)
    {
        Console.WriteLine($"Минимизированная СКНФ расчётным методом: {formula}\n");
    }
    
    public static void PrintSDNFTableCalculatingMethod(string formula)
    {
        Console.WriteLine($"Минимизированная СДНФ расчётно-табличным методом: {formula} \n");
    }
    
    public static void PrintSKNFTableCalculatingMethod(string formula)
    {
        Console.WriteLine($"Минимизированная СКНФ расчётно-табличным методом: {formula} \n");
    }
    
    public static void PrintSDNFTableMethod(string formula)
    {
        Console.WriteLine($"Минимизированная СДНФ табличным методом: {formula} \n");
    }
    
    public static void PrintSKNFTableMethod(string formula)
    {
        Console.WriteLine($"Минимизированная СКНФ табличным методом: {formula}  \n");
    }
}