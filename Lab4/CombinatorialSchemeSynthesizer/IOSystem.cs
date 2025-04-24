using SKNF_SDNF;

namespace CombinatorialSchemeSynthesizer;

public class IOSystem
{
    public static void FormTable(List<List<int>> cases, List<string> subformulas, string reformedFormula, InputParser parser, List<string> neededFormulas)
    {
        Console.WriteLine("\nТаблица истинности:");
        foreach (var subformula in subformulas)
            if(neededFormulas.Contains(subformula))
                Console.Write(new string(' ', 4)+ subformula + new string(' ', 4) + "|");
        Console.WriteLine();
        for (int i = 0; i < cases.Count; i++)
        {
            parser.AssignValues(cases[i]);  
            Dictionary<string, int> subformulasAndItsValue= parser.FindSubformulasAndItsValue(reformedFormula);
            foreach (var subformula in subformulas)
            {
                if(neededFormulas.Contains(subformula))
                    Console.Write(new string(' ', 4+subformula.Length/2) + subformulasAndItsValue[subformula] + new string(' ', (subformula.Length%2 == 0? 4-1 : 4)+subformula.Length/2) + "|");
            }
            Console.WriteLine();
        }
    }
}