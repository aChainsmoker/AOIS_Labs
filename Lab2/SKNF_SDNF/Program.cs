namespace SKNF_SDNF;

class Program
{
    static void Main(string[] args)
    {
        //((a|b)&!c)  !(!(A|B))~C ((((P&(!Q))>R)~P)|(P&Q)) (!((S>((!R)|(P&Q)))~(P&(!(Q>R)))))

        while (true)
        {
            string input = IOSystem.TakeTheInput();
            if(input == "exit")
                break;
            InputParser parser = new InputParser();
            List<List<int>> cases;
            List<string> subformulas;
            string reformedFormula;
            string sknf;
            string sdnf;
            List<int> numberFormSknf;
            List<int> numberFormSdnf;
            long indexForm;
            string indexBinaryForm;
            try
            {
                reformedFormula = parser.ReformFormula(input);
                cases = CaseGenerator.GenerateCases(parser.Letters.Count);
                subformulas = parser.FindSubformulas(reformedFormula);
                sknf = SknfSdnfFinder.FindSKNF(cases, parser, reformedFormula, out numberFormSknf);
                sdnf = SknfSdnfFinder.FindSDNF(cases, parser, reformedFormula, out numberFormSdnf);
                indexForm = BinaryNumberConverter.GetDecimalFromBinary(
                    SknfSdnfFinder.ExtractBinaryForm(cases, parser, reformedFormula));
                indexBinaryForm = string.Join("", SknfSdnfFinder.ExtractBinaryForm(cases, parser, reformedFormula));
                IOSystem.FormTable(cases, subformulas, reformedFormula, parser);
                IOSystem.PrintSknf(sknf);
                IOSystem.PrintNumberFormSknf(numberFormSknf);
                IOSystem.PrintSdnf(sdnf);
                IOSystem.PrintNumberFormSdnf(numberFormSdnf);
                IOSystem.PrintIndexForm(indexForm, indexBinaryForm);
            }
            catch (Exception e)
            {
                Console.WriteLine("Incorrect input. Try Again.");
            }
        }
    }
}

