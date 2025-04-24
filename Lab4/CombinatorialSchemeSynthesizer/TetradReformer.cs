using Lab1;
using SKNF_SDNF_Minimizer;
using SKNF_SDNF;
using Lab1;

namespace CombinatorialSchemeSynthesizer;

public class TetradReformer
{
    public void ReformTetrad()
    {
        List<List<int>> cases = CaseGenerator.GenerateCases(4);
        
        List<List<int>> casesPlusNine = new List<List<int>>();

        
        
        Console.WriteLine("Д8421   Д8421+9");
        for (int i = 0; i < 10; i++)
        {
            BinaryNumber number1 = new BinaryNumber();
            number1.Number[31] = cases[i][3];
            number1.Number[30] = cases[i][2];
            number1.Number[29] = cases[i][1];
            number1.Number[28] = cases[i][0];
            
            BinaryNumber number2 = new BinaryNumber();
            number2.ConvertToAdditionalBinary(9);
            
            BinaryNumber number3 = new BinaryNumber();
            number3 = number2 + number1;
            
            number3.ConvertToAdditionalBinary(number3.GetDecimal() % 10);
            
            casesPlusNine.Add(number3.Number.TakeLast(4).ToList());
            
            Console.WriteLine(string.Join("", cases[i]) + "\t" + string.Join("", number3.Number.TakeLast(4)));
        }

        for (int j = 0; j < 4; j++)
        {
            Console.WriteLine("\n" + $"Y{j+1}:");
            Minimizer minimizer = new Minimizer();
            Dictionary<List<int>, int> casesValuesPairs = new Dictionary<List<int>, int>();
            for (int i = 0; i < cases.Count; i++)
            {
                if(i < 10)
                    casesValuesPairs.Add(cases[i], casesPlusNine[i][j]);
                else 
                    casesValuesPairs.Add(cases[i], 1);
            }
            int[,] kmap = minimizer.BuildKarnaughMap(casesValuesPairs, 4);
            SKNF_SDNF_Minimizer.IOSystem.PrintKarnaughMap(kmap);
            SKNF_SDNF_Minimizer.IOSystem.PrintSKNFTableMethod(minimizer.MinimizeKarnaughToCNF(kmap, 4, new List<char>(){'A', 'B', 'C', 'D'}));
        }
    }
    
    
}