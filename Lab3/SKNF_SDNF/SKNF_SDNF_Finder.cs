namespace SKNF_SDNF;

public static class SknfSdnfFinder
{
    public static string FindSKNF(List<List<int>> cases, InputParser parser, string reformedFormula, out List<int> numberForm)
    {
        numberForm = new List<int>();
        List<string> sknfValues = new List<string>();
        List<string> subformulas = parser.FindSubformulas(reformedFormula);
        for (int i = 0; i < cases.Count; i++)
        {
            parser.AssignValues(cases[i]);
            Dictionary<string, int> subformulasAndItsValue= parser.FindSubformulasAndItsValue(reformedFormula);
            if (subformulasAndItsValue[subformulas.Last()] == 0)
            {
                numberForm.Add(i);
                foreach (char lettter in parser.Letters)
                {
                    if (parser.Values[lettter.ToString()] == 1)
                    {
                        sknfValues.Add("!" + lettter.ToString());
                    }
                    else
                        sknfValues.Add(lettter.ToString());
                }
            }
        }
        return BuildSknfString(sknfValues, cases[0].Count);
    }

    private static string BuildSknfString(List<string> sknfValues, int valuesAmount)
    {
        string sknfString = String.Empty;
        sknfString += "(";
        for (int i = 0; i < sknfValues.Count; i++)
        {
            if ((i + 1) % valuesAmount == 0)
            {
                if (i == sknfValues.Count - 1)
                {
                    sknfString += sknfValues[i] + ")";
                    continue;
                }
                sknfString += sknfValues[i] + ")/\\(";
                continue;
            }
            sknfString += sknfValues[i] + "\\/";
        }
        return sknfString;
    }
    
    public static string FindSDNF(List<List<int>> cases, InputParser parser, string reformedFormula, out List<int> numberForm)
    {
        List<string> sknfValues = new List<string>();
        numberForm = new List<int>();
        List<string> subformulas = parser.FindSubformulas(reformedFormula);
        for (int i = 0; i < cases.Count; i++)
        {
            parser.AssignValues(cases[i]);
            Dictionary<string, int> subformulasAndItsValue= parser.FindSubformulasAndItsValue(reformedFormula);
            if (subformulasAndItsValue[subformulas.Last()] == 1)
            {
                numberForm.Add(i);
                foreach (char lettter in parser.Letters)
                {
                    if (parser.Values[lettter.ToString()] == 0)
                    {
                        sknfValues.Add("!" + lettter.ToString());
                    }
                    else
                        sknfValues.Add(lettter.ToString());
                }
            }
        }
        return BuildSdnfString(sknfValues, cases[0].Count);
    }

    private static string BuildSdnfString(List<string> sknfValues, int valuesAmount)
    {
        string sknfString = String.Empty;
        sknfString += "(";
        for (int i = 0; i < sknfValues.Count; i++)
        {
            if ((i + 1) % valuesAmount == 0)
            {
                if (i == sknfValues.Count - 1)
                {
                    sknfString += sknfValues[i] + ")";
                    continue;
                }
                sknfString += sknfValues[i] + ")\\/(";
                continue;
            }
            sknfString += sknfValues[i] + "/\\";
        }
        return sknfString;
    }

    public static List<int> ExtractBinaryForm(List<List<int>> cases, InputParser parser, string reformedFormula)
    {
        List<int> binaryForm = new List<int>();
        List<string> subformulas = parser.FindSubformulas(reformedFormula);
        for (int i = 0; i < cases.Count; i++)
        {
            parser.AssignValues(cases[i]);
            Dictionary<string, int> subformulasAndItsValue= parser.FindSubformulasAndItsValue(reformedFormula);
            binaryForm.Add(subformulasAndItsValue[subformulas.Last()]);
        }

        return binaryForm;
    }
}