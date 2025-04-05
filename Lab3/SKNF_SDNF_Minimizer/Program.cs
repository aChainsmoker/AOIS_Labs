namespace SKNF_SDNF_Minimizer;

class Program
{
    // CДНФ (!a/\!b/\c)\/(!a/\b/\!c)\\/(!a/\b/\c)\/(a/\b/\!c)   
    // СКНФ (a\/b\/c)/\(!a\/b\/c)/\(!a\/b\/!c)/\(!a\/!b\/!c)
    static void Main(string[] args)
    {
        while (true)
        {
            int sknfOrSdnf = IOSystem.ChooseFormulaType();
            string formula = IOSystem.TakeTheInput();
            Minimizer minimizer = new Minimizer();

            try
            {
                if (sknfOrSdnf == 1)
                {
                    IOSystem.PrintSDNFCalculatingMethod(minimizer.MinimizeSDNFByCalculating(formula));
                    IOSystem.PrintSDNFTableCalculatingMethod(minimizer.MinimizeSDNFByTableAndCalculating(formula));
                    IOSystem.PrintSDNFTableMethod(minimizer.MinimizeSDNFByKarnaugh(formula));
                }
                else
                {
                    IOSystem.PrintSKNFCalculatingMethod(minimizer.MinimizeSKNFByCalculating(formula));
                    IOSystem.PrintSKNFTableCalculatingMethod(minimizer.MinimizeSKNFByTableAndCalculating(formula));
                    IOSystem.PrintSKNFTableMethod(minimizer.MinimizeSKNFByKarnaugh(formula));
                }
        
            }
            catch 
            {
                Console.WriteLine("Incorrect input. Please try again.");
            }
        
        }

    }
}