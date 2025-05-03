using SKNF_SDNF;
using SKNF_SDNF_Minimizer;

namespace SynthesisOf_DigitalMachines;

public class Synthesizer
{
    public void Synthesize()
    {
        List<List<int>> casesQ = CaseGenerator.GenerateCases(3);
        for (int i = 1; i < casesQ.Count; i++)
        {
            casesQ.Insert(i, new List<int>(casesQ[i]));
            i++;
        }
        casesQ.Add(new List<int>(casesQ[0]));
        List<List<int>> casesQR = new List<List<int>>() {new List<int>(){0,0,0}};

        for (int i = 1; i < casesQ.Count; i++)
        {
            casesQR.Add(new List<int>(casesQ[i-1]));
        }
        
        List<List<int>> casesV = CaseGenerator.GenerateCases(1);
        while (casesV.Count != casesQ.Count)
        {
            casesV.Add(new List<int>(casesV[0]));
            casesV.Add(new List<int>(casesV[1]));
        }

        List<List<int>> casesH = new List<List<int>>();

        for (int i = 0; i < casesQ.Count; i++)
        {
            List<int> aCase = new List<int>(){0,0,0};
            casesH.Add(aCase);
            for (int j = 0; j < aCase.Count; j++)
            {
                if (casesQ[i][j] != casesQR[i][j])
                    aCase[j] = 1;
            }
        }

        PrintTable(casesQ, casesQR, casesV, casesH);
        
        List<List<int>> casesQR_V = new List<List<int>>();
        for (int i = 0; i < casesV.Count; i++)
        {
            casesQR[i].AddRange(casesV[i]);
            casesQR_V.Add(casesQR[i]);
        }

        for (int j = 0; j < 3; j++)
        {
            Console.WriteLine("\n" + $"H{j+1}:");
            Minimizer minimizer = new Minimizer();
            Dictionary<List<int>, int> casesValuesPairs = new Dictionary<List<int>, int>();
            for (int i = 0; i < casesQR_V.Count; i++)
            {
                casesValuesPairs.Add(casesQR_V[i], casesH[i][j]);
            }
            int[,] kmap = minimizer.BuildKarnaughMap(casesValuesPairs, 4);
            SKNF_SDNF_Minimizer.IOSystem.PrintKarnaughMap(kmap);
            SKNF_SDNF_Minimizer.IOSystem.PrintSKNFTableMethod(minimizer.MinimizeKarnaughToDNF(kmap, 4, new List<char>(){'A', 'B', 'C', 'D'}));
        }
    }

    private void PrintTable(List<List<int>> casesQ, List<List<int>> casesQR, List<List<int>> casesV, List<List<int>> casesH)
    {
        for(int i =0; i < casesQR[0].Count; i++)
        {
            Console.Write($"Q*{3-i} ");
            for (int j = 0; j < casesQR.Count; j++)
            {
                Console.Write(casesQR[j][i] + " ");
            }
            Console.WriteLine();
        }
        for(int i =0; i < casesV[0].Count; i++)
        {
            Console.Write("V   ");
            for (int j = 0; j < casesV.Count; j++)
            {
                Console.Write(casesV[j][i] + " ");
            }
            Console.WriteLine();
        }
        for(int i =0; i < casesQ[0].Count; i++)
        {
            Console.Write($"Q{3-i}  ");
            for (int j = 0; j < casesQ.Count; j++)
            {
                Console.Write(casesQ[j][i] + " ");
            }
            Console.WriteLine();
        }
        for(int i =0; i < casesH[0].Count; i++)
        {
            Console.Write("H   ");
            for (int j = 0; j < casesH.Count; j++)
            {
                Console.Write(casesH[j][i] + " ");
            }
            Console.WriteLine();
        }
    }
}