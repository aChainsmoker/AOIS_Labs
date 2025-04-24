using SKNF_SDNF;
using SKNF_SDNF_Minimizer;

namespace CombinatorialSchemeSynthesizer;

public class ODS3
{
    public void MinimizeCombinatorialScheme()
    {
        List<List<int>> cases = CaseGenerator.GenerateCases(3);
        string sumFormula = "A^B^C";
        string shiftFormula = "(A&B)|(A&C)|(B&C)";
        InputParser parser = new InputParser();
        Minimizer minimizer = new Minimizer();
        
        string reformedSumFormula = parser.ReformFormula(sumFormula);
        List<string> sumSubformulas = parser.FindSubformulas(reformedSumFormula);
        string sumSDNF = SknfSdnfFinder.FindSDNF(cases, parser, reformedSumFormula, out _);
        string minimizedSumSDNF = minimizer.MinimizeSDNFByCalculating(sumSDNF);
        
        string reformedShiftFormula = parser.ReformFormula(shiftFormula);
        List<string> shiftSubformulas = parser.FindSubformulas(reformedShiftFormula);
        string shiftSDNF = SknfSdnfFinder.FindSDNF(cases, parser, reformedShiftFormula, out _);
        string minimizedShiftSDNF = minimizer.MinimizeSDNFByCalculating(shiftSDNF);

        List<string> neededSubformulas = new List<string>();
        neededSubformulas.AddRange(parser.Letters.Select(c => c.ToString()));
        neededSubformulas.Add(sumSubformulas.Last());
        neededSubformulas.Add(shiftSubformulas.Last());
        
        IOSystem.FormTable(cases, sumSubformulas, reformedSumFormula, parser , neededSubformulas);
        SKNF_SDNF.IOSystem.PrintSdnf(sumSDNF);
        SKNF_SDNF_Minimizer.IOSystem.PrintSDNFCalculatingMethod(minimizedSumSDNF);
        
        IOSystem.FormTable(cases, shiftSubformulas, reformedShiftFormula, parser , neededSubformulas);
        SKNF_SDNF.IOSystem.PrintSdnf(shiftSDNF);
        SKNF_SDNF_Minimizer.IOSystem.PrintSDNFCalculatingMethod(minimizedShiftSDNF);
    }
}