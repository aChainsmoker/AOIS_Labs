namespace SKNF_SDNF;

public static class IOSystem
{
    public static void FormTable(List<List<int>> cases, List<string> subformulas, string reformedFormula, InputParser parser)
    {
        Console.WriteLine("\nТаблица истинности:");
        foreach (var subformula in subformulas)
            Console.Write(new string(' ', 4)+ subformula + new string(' ', 4) + "|");
        Console.WriteLine();
        for (int i = 0; i < cases.Count; i++)
        {
            parser.AssignValues(cases[i]);
            Dictionary<string, int> subformulasAndItsValue= parser.FindSubformulasAndItsValue(reformedFormula);
            foreach (var subformula in subformulas)
            {
                Console.Write(new string(' ', 4+subformula.Length/2) + subformulasAndItsValue[subformula] + new string(' ', (subformula.Length%2 == 0? 4-1 : 4)+subformula.Length/2) + "|");
            }
            Console.WriteLine();
        }
    }

    public static void PrintNumberFormSknf(List<int> numberForm)
    {
        Console.WriteLine("Числовая форма СКНФ: " + MakeNumberString(numberForm) + " /\\");
    }
    
    public static void PrintNumberFormSdnf(List<int> numberForm)
    {
        Console.WriteLine("Числовая форма СДНФ: " + MakeNumberString(numberForm) + " \\/");
    }

    private static string MakeNumberString(List<int> numberForm)
    {
        string setString = String.Empty;
        setString += "(";
        foreach (int number in numberForm)
        {
                setString += number;
                if (numberForm.Last() != number)
                    setString += ", ";
        }
        setString += ")";
        return setString;
    }

    public static void PrintSknf(string sknf)
    {
        Console.WriteLine("\nСКНФ: " + sknf);
    }

    public static void PrintSdnf(string sdnf)
    {
        Console.WriteLine("\nСДНФ: " + sdnf);
    }

    public static void PrintIndexForm(long indexForm, string indexBinaryForm)
    {
        Console.WriteLine("\nИндексная форма: " + indexForm + " = " + indexBinaryForm + "\n");
    }

    public static string? TakeTheInput()
    {
        Console.WriteLine("Введите формулу: ");
        return Console.ReadLine();
    }
}