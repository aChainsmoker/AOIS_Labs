using SKNF_SDNF;

namespace SKNF_SDNF_Minimizer;

class Program
{
    // CДНФ (!a/\!b/\c)\/(!a/\b/\!c)\\/(!a/\b/\c)\/(a/\b/\!c)   
    // СКНФ (a\/b\/c)/\(!a\/b\/c)/\(!a\/b\/!c)/\(!a\/!b\/!c)
    static void Main(string[] args)
    {
        while (true)
        {
            string formula = IOSystem.TakeTheInput();
            if(formula == "exit")
                break;
            
            InputParser parser = new InputParser();
            Minimizer minimizer = new Minimizer();
            List<List<int>> cases;
            string reformedFormula;
            string sknf;
            string sdnf;

            try
            {
                reformedFormula = parser.ReformFormula(formula);
                cases = CaseGenerator.GenerateCases(parser.Letters.Count);
                sknf = SknfSdnfFinder.FindSKNF(cases, parser, reformedFormula, out _);
                sdnf = SknfSdnfFinder.FindSDNF(cases, parser, reformedFormula, out _);
                
                SKNF_SDNF.IOSystem.PrintSdnf(sdnf);
                IOSystem.PrintSDNFCalculatingMethod(minimizer.MinimizeSDNFByCalculating(sdnf));
                IOSystem.PrintSDNFTableCalculatingMethod(minimizer.MinimizeSDNFByTableAndCalculating(sdnf));
                IOSystem.PrintSDNFTableMethod(minimizer.MinimizeSDNFByKarnaugh(sdnf));
                
                
                SKNF_SDNF.IOSystem.PrintSknf(sknf);
                IOSystem.PrintSKNFCalculatingMethod(minimizer.MinimizeSKNFByCalculating(sknf));
                IOSystem.PrintSKNFTableCalculatingMethod(minimizer.MinimizeSKNFByTableAndCalculating(sknf));
                IOSystem.PrintSKNFTableMethod(minimizer.MinimizeSKNFByKarnaugh(sknf));
            }
            catch 
            {
                Console.WriteLine("Incorrect input. Please try again.");
            }
        
        }

    }
}