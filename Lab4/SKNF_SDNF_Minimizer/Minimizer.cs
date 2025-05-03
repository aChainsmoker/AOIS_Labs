using SKNF_SDNF;

namespace SKNF_SDNF_Minimizer;

public partial class Minimizer
{
    public string MinimizeSDNFByCalculating(string sdnf)
    {
        List<List<string>> parsedSDNF = ParseFormule(sdnf);
        List<List<string>> minimizedSDNF = MinimizeFormuleByMingling(parsedSDNF);
        List<List<string>> simplifiedSDNF = FindRedundantSDNFByCalculation(parsedSDNF, minimizedSDNF);
        return BuildDNFString(simplifiedSDNF);
    }

    public string MinimizeSKNFByCalculating(string sknf)
    {
        List<List<string>> parsedSKNF = ParseFormule(sknf);
        List<List<string>> minimizedSKNF = MinimizeFormuleByMingling(parsedSKNF);
        List<List<string>> simplifiedSKNF = FindRedunduntSKNFByCalculation(parsedSKNF, minimizedSKNF);
        return BuildCNFString(simplifiedSKNF);
    }

    public string MinimizeSDNFByTableAndCalculating(string sdnf)
    {
        List<List<string>> parsedSDNF = ParseFormule(sdnf);
        List<List<string>> minimizedSDNF = MinimizeFormuleByMingling(parsedSDNF);
        List<List<string>> essentialImplicants = FindRedundantByTable(parsedSDNF, minimizedSDNF);
        IOSystem.PrintSDNFTable(parsedSDNF, minimizedSDNF);
        return BuildDNFString(essentialImplicants);
    }

    public string MinimizeSKNFByTableAndCalculating(string sknf)
    {
        List<List<string>> parsedSKNF = ParseFormule(sknf);
        List<List<string>> minimizedSKNF = MinimizeFormuleByMingling(parsedSKNF);
        List<List<string>> essentialImplicants = FindRedundantByTable(parsedSKNF, minimizedSKNF);
        IOSystem.PrintSKNFTable(parsedSKNF, minimizedSKNF);
        return BuildCNFString(essentialImplicants);
    }
    
    private List<List<string>> ParseFormule(string formule)
    {
        List<List<string>> parsedFormule = new List<List<string>>();
        int index = 0;
        for (int i = 0; i < formule.Length; i++)
        {
            if (formule[i] == '(')
            {
                parsedFormule.Add(new List<string>());
                while (formule[i] != ')')
                {
                    if((formule[i] >= 'a' && formule[i] <= 'z') || (formule[i] >= 'A' && formule[i] <= 'Z'))
                        parsedFormule[index].Add(formule[i].ToString());
                    else if (formule[i] == '!')
                    {
                        parsedFormule[index].Add(formule[i].ToString() + formule[i + 1]);
                        i++;
                    }

                    i++;
                }
                index++;
            }
        }
        return parsedFormule;
    }

    private List<List<string>> MinimizeFormuleByMingling(List<List<string>> parsedFormule)
    {
        if(parsedFormule.Any(innerList => innerList.Count == 1))
           return parsedFormule.DistinctValue();
        
        List<List<string>> minimizedSdnf = new List<List<string>>();
        List<List<string>> parsedSdnfCopy = new List<List<string>>(parsedFormule);
        for (int i = 0; i < parsedFormule.Count-1; i++)
        {
            for (int j = i + 1; j < parsedFormule.Count; j++)
            {
                List<string> commonElements = new List<string>();
                List<string> unCommonElements = new List<string>();
                for (int k = 0; k < parsedFormule[i].Count; k++)
                {
                    if(parsedFormule[i][k] == parsedFormule[j][k])
                        commonElements.Add(parsedFormule[i][k]);
                    else if (parsedFormule[i][k] == ReturnReversedElement(parsedFormule[j][k]))
                        unCommonElements.Add(parsedFormule[i][k]);
                }

                if (commonElements.Count == parsedFormule[i].Count - 1 && unCommonElements.Count == 1)
                {
                    minimizedSdnf.Add(commonElements);
                    if(parsedSdnfCopy.Contains(parsedFormule[i]))
                        parsedSdnfCopy.Remove(parsedFormule[i]);
                    if(parsedSdnfCopy.Contains(parsedFormule[j]))
                        parsedSdnfCopy.Remove(parsedFormule[j]);
                    
                }
            }
        }
        minimizedSdnf.AddRange(parsedSdnfCopy);
        
        return minimizedSdnf.SequenceEqual(parsedFormule) ?  minimizedSdnf.DistinctValue(): MinimizeFormuleByMingling(minimizedSdnf);
    }

private List<List<string>> FindRedundantSDNFByCalculation(List<List<string>> parsedSDNF, List<List<string>> minimizedSdnf)
{
    List<List<string>> result = new List<List<string>>(minimizedSdnf);
    
    for (int i = 0; i < minimizedSdnf.Count; i++)
    {
        List<string> currentImplicant = minimizedSdnf[i];
        
        Dictionary<string, int> assignment = GetAssignmentForSDNFImplicant(currentImplicant);
        if (assignment == null) continue; 

        bool isRedundant = CheckIfRedundantSDNF(currentImplicant, minimizedSdnf, assignment);

        if (isRedundant)
        {
            result.Remove(currentImplicant);
        }
    }

    return result;
}

private Dictionary<string, int> GetAssignmentForSDNFImplicant(List<string> implicant)
{
    var assignment = new Dictionary<string, int>();
    foreach (string literal in implicant)
    {
        if (literal.StartsWith("!"))
        {
            string var = literal.Substring(1);
            assignment[var] = 0;
        }
        else
        {
            assignment[literal] = 1;
        }
    }
    return assignment;
}

private bool CheckIfRedundantSDNF(List<string> implicant, List<List<string>> minimizedSdnf, 
                              Dictionary<string, int> assignment)
{
    List<List<string>> minimizedSdnfCopy = new List<List<string>>();
    for (int i = 0; i < minimizedSdnf.Count; i++)
    {
        
        minimizedSdnfCopy.Add(new List<string>());
        minimizedSdnfCopy[i].AddRange(minimizedSdnf[i]);
    }

    for(int j = 0 ; j < minimizedSdnfCopy.Count; j++)
    {
        if (minimizedSdnfCopy[j].SequenceEqual(implicant))
        {
            minimizedSdnfCopy.Remove(minimizedSdnfCopy[j]);
            j--;
            continue;
        }; 

        bool implicantValue = true;
        for(int i =0; i<minimizedSdnfCopy[j].Count; i++)
        {
            string var = minimizedSdnfCopy[j][i][0] == '!' ? minimizedSdnfCopy[j][i].Substring(1) : minimizedSdnfCopy[j][i];
            if (assignment.ContainsKey(var))
            {
                minimizedSdnfCopy[j][i] = minimizedSdnfCopy[j][i][0] == '!' ? ReturnReversedValue(assignment[var]).ToString() : Convert.ToInt32(assignment[var]).ToString();
            }
        }
    }
    List<List<string>> checkingSDNF =  MinimizeFormuleByMingling(minimizedSdnfCopy);
    if (checkingSDNF.Any(innerList => innerList.Contains("1") && innerList.Count == 1))
        return true;
    
    return false;
}
    
private List<List<string>> FindRedunduntSKNFByCalculation(List<List<string>> parsedSKNF, List<List<string>> minimizedSknf)
{
    List<List<string>> result = new List<List<string>>(minimizedSknf);
    
    for (int i = 0; i < minimizedSknf.Count; i++)
    {
        List<string> currentImplicant = minimizedSknf[i];
        
        Dictionary<string, int> assignment = GetAssignmentForSKNFImplicant(currentImplicant);
        if (assignment == null) continue; 

        bool isRedundant = CheckIfRedundantSKNF(currentImplicant, minimizedSknf, assignment);

        if (isRedundant)
        {
            result.Remove(currentImplicant);
        }
    }

    return result;
}

private Dictionary<string, int> GetAssignmentForSKNFImplicant(List<string> implicant)
{
    var assignment = new Dictionary<string, int>();
    foreach (string literal in implicant)
    {
        if (literal.StartsWith("!"))
        {
            string var = literal.Substring(1);
            assignment[var] = 1;
        }
        else
        {
            assignment[literal] = 0;
        }
    }
    return assignment;
}

private bool CheckIfRedundantSKNF(List<string> implicant, List<List<string>> minimizedSknf, 
                              Dictionary<string, int> assignment)
{
    List<List<string>> minimizedSdnfCopy = new List<List<string>>();
    for (int i = 0; i < minimizedSknf.Count; i++)
    {
        
        minimizedSdnfCopy.Add(new List<string>());
        minimizedSdnfCopy[i].AddRange(minimizedSknf[i]);
    }

    for(int j = 0 ; j < minimizedSdnfCopy.Count; j++)
    {
        if (minimizedSdnfCopy[j].SequenceEqual(implicant))
        {
            minimizedSdnfCopy.Remove(minimizedSdnfCopy[j]);
            j--;
            continue;
        }; 

        bool implicantValue = true;
        for(int i =0; i<minimizedSdnfCopy[j].Count; i++)
        {
            string var = minimizedSdnfCopy[j][i][0] == '!' ? minimizedSdnfCopy[j][i].Substring(1) : minimizedSdnfCopy[j][i];
            if (assignment.ContainsKey(var))
            {
                minimizedSdnfCopy[j][i] = minimizedSdnfCopy[j][i][0] == '!' ? ReturnReversedValue(assignment[var]).ToString() : Convert.ToInt32(assignment[var]).ToString();
            }
        }
    }
    List<List<string>> checkingSDNF =  MinimizeFormuleByMingling(minimizedSdnfCopy);
    if (checkingSDNF.Any(innerList => innerList.Contains("0") && innerList.Count == 1))
        return true;
    
    return false;
}

private List<List<string>> FindRedundantByTable(List<List<string>> parsedFormulas, List<List<string>> minimizedFormulas)
{
    List<List<string>> resultFormulas = new List<List<string>>();
    int[] amountOfInclusions = new int[parsedFormulas.Count];
    int index = 0;
    foreach (var constituent in parsedFormulas)
    {
        foreach (var implicant in minimizedFormulas)
        {
            if(implicant.All(item => constituent.Contains(item)))
                amountOfInclusions[index]++;
        }
        index++;
    }
    for(int i =0; i < parsedFormulas.Count; i++)
    {
        if(amountOfInclusions[i] == amountOfInclusions.Min())
            foreach (var implicant in minimizedFormulas)
            {
                if(implicant.All(item => parsedFormulas[i].Contains(item)))
                    resultFormulas.Add(implicant);
            }
    }
    return resultFormulas.DistinctValue();
}

    private string ReturnReversedElement(string element)
    {
        if (element[0]=='!')
            element = element.Substring(1);
        else
            element = "!" + element;
        return element;
    }

    private int ReturnReversedValue(int value)
    {
        if(value == 0)
            value = 1;
        else if (value == 1)
            value = 0;
        return value;
    }
    

    private string BuildDNFString(List<List<string>> terms)
    {
        List<string> disjunctions = new List<string>();
        foreach (var term in terms)
        {
            if (term.Count == 0) continue;
            disjunctions.Add($"({string.Join("/\\", term)})");
        }
        return disjunctions.Count > 0 ? string.Join("\\/", disjunctions) : "0";
    }

    private string BuildCNFString(List<List<string>> terms)
    {
        List<string> conjunctions = new List<string>();
        foreach (var term in terms)
        {
            if (term.Count == 0) continue;
            conjunctions.Add($"({string.Join("\\/", term)})");
        }
        return conjunctions.Count > 0 ? string.Join("/\\", conjunctions) : "1";
    }
    
}

