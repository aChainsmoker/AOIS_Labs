using SKNF_SDNF;

namespace SKNF_SDNF_Minimizer;

public partial class Minimizer
{
    public string MinimizeSDNFByKarnaugh(string sdnf)
    {
        List<List<string>> parsedSDNF = ParseFormule(sdnf);
        List<char> variableOrder = GetVariableOrder(parsedSDNF); 
        int n = variableOrder.Count;
        if (n > 4) throw new ArgumentException("Supported up to 4 variables");

        List<List<int>> cases = CaseGenerator.GenerateCases(n);
        var casesValuesPairs = FindSDNFValues(cases, parsedSDNF, variableOrder);
        int[,] kmap = BuildKarnaughMap(casesValuesPairs, n);
        IOSystem.PrintKarnaughMap(kmap);
        return MinimizeKarnaughToDNF(kmap, n, variableOrder);
    }

    private List<char> GetVariableOrder(List<List<string>> parsedFormule)
    {
        HashSet<char> variables = new HashSet<char>();
        foreach (var constituent in parsedFormule)
        {
            foreach (var literal in constituent)
            {
                char varName = literal[0] == '!' ? literal[1] : literal[0];
                variables.Add(varName);
            }
        }
        return variables.ToList(); 
    }

    private Dictionary<List<int>, int> FindSDNFValues(List<List<int>> cases, List<List<string>> parsedSDNF, List<char> variableOrder)
    {
        var results = new Dictionary<List<int>, int>();
        foreach (var caseValues in cases)
        {
            bool formulaResult = false;
            foreach (var constituent in parsedSDNF)
            {
                bool constituentResult = true;
                foreach (var literal in constituent)
                {
                    bool isNegated = literal[0] == '!';
                    char varName = isNegated ? literal[1] : literal[0];
                    int varIndex = variableOrder.IndexOf(varName);
                    int varValue = caseValues[varIndex];
                    bool literalResult = isNegated ? (varValue == 0) : (varValue == 1);
                    constituentResult = constituentResult && literalResult;
                    if (!constituentResult)
                        break;
                }
                if (constituentResult)
                {
                    formulaResult = true;
                    break;
                }
            }
            results.Add(caseValues, formulaResult ? 1 : 0);
        }
        return results;
    }

    private int[,] BuildKarnaughMap(Dictionary<List<int>, int> results, int n)
    {
        int rows, cols;
        switch (n)
        {
            case 1: rows = 1; cols = 2; break;
            case 2: rows = 2; cols = 2; break;
            case 3: rows = 2; cols = 4; break;
            case 4: rows = 4; cols = 4; break;
            default: throw new ArgumentException("Поддерживается до 4 переменных");
        }

        int[,] kmap = new int[rows, cols];
        foreach (var entry in results)
        {
            List<int> caseValues = entry.Key;
            int value = entry.Value;
            int row = GetRowIndex(caseValues, n);
            int col = GetColIndex(caseValues, n);
            kmap[row, col] = value;
        }
        return kmap;
    }

    private int GetGrayCode(int n)
    {
        return n ^ (n >> 1);
    }

    private int GetRowIndex(List<int> values, int n)
    {
        if (n == 1) return 0;
        if (n == 2) return values[0];
        if (n == 3) return values[0];
        if (n == 4)
        {
            int a = values[0];
            int b = values[1];
            return GetGrayCode((a << 1) | b);
        }
        throw new ArgumentException("Unsupported amount of Variable");
    }

    private int GetColIndex(List<int> values, int n)
    {
        if (n == 1) return values[0];
        if (n == 2) return values[1];
        if (n == 3)
        {
            int b = values[1];
            int c = values[2];
            return GetGrayCode((b << 1) | c);
        }
        if (n == 4)
        {
            int c = values[2];
            int d = values[3];
            return GetGrayCode((c << 1) | d);
        }
        throw new ArgumentException("Unsupported amount of Variable");
    }

    private List<List<int>> FindKarnaughGroups(int[,] kmap, int targetValue, int n)
    {
        int rows = kmap.GetLength(0);
        int cols = kmap.GetLength(1);
        List<List<int>> groups = new List<List<int>>();
        HashSet<string> uniqueGroups = new HashSet<string>();

        var dimensions = GetPossibleGroupSizes(rows, cols);
        foreach (var dim in dimensions)
        {
            int h = dim.Item1;
            int w = dim.Item2;
            List<int> startRows = h == rows ? new List<int> { 0 } : Enumerable.Range(0, rows).ToList();

            foreach (int s in startRows)
            {
                for (int c = 0; c < cols; c++)
                {
                    List<Tuple<int, int>> cells = new List<Tuple<int, int>>();
                    bool isValid = true;

                    for (int row = s; row < s + h; row++)
                    {
                        int currentRow = row % rows;
                        for (int i = 0; i < w; i++)
                        {
                            int currentCol = (c + i) % cols;
                            if (kmap[currentRow, currentCol] != targetValue)
                            {
                                isValid = false;
                                break;
                            }
                            cells.Add(Tuple.Create(currentRow, currentCol));
                        }
                        if (!isValid) break;
                    }

                    if (isValid)
                    {
                        List<int> coordinates = cells.SelectMany(cell => new[] { cell.Item1, cell.Item2 }).ToList();
                        string key = string.Join(",", coordinates.OrderBy(x => x));
                        if (!uniqueGroups.Contains(key))
                        {
                            uniqueGroups.Add(key);
                            groups.Add(coordinates);
                        }
                    }
                }
            }
        }

        return FilterGroups(groups, kmap, targetValue);
    }

    private List<Tuple<int, int>> GetPossibleGroupSizes(int rows, int cols)
    {
        var sizes = new List<Tuple<int, int>>();
        int[] rowSizes = rows == 1 ? new[] { 1 } : new[] { 1, 2, 4 }.Where(x => x <= rows).ToArray();
        int[] colSizes = new[] { 1, 2, 4 }.Where(x => x <= cols).ToArray();

        foreach (int r in rowSizes)
            foreach (int c in colSizes)
                if (r * c >= 1)
                    sizes.Add(Tuple.Create(r, c));

        return sizes;
    }

    private List<List<int>> FilterGroups(List<List<int>> groups, int[,] kmap, int targetValue)
    {
        var filteredGroups = new List<List<int>>();
        var sortedGroups = groups.OrderByDescending(g => g.Count / 2).ToList();

        foreach (var group in sortedGroups)
        {
            bool isSubset = filteredGroups.Any(existing => IsSubset(group, existing));
            if (!isSubset)
                filteredGroups.Add(group);
        }

        for (int i = 0; i < filteredGroups.Count; i++)
        {
            if (IsCoveredByUnion(filteredGroups[i], filteredGroups, kmap, targetValue))
            {
                filteredGroups.RemoveAt(i);
                i--;
            }
        }
        return filteredGroups;
    }

    private bool IsSubset(List<int> group, List<int> superGroup)
    {
        var groupCells = Enumerable.Range(0, group.Count / 2).Select(i => Tuple.Create(group[i * 2], group[i * 2 + 1])).ToHashSet();
        var superCells = Enumerable.Range(0, superGroup.Count / 2).Select(i => Tuple.Create(superGroup[i * 2], superGroup[i * 2 + 1])).ToHashSet();
        return groupCells.IsSubsetOf(superCells);
    }

    private bool IsCoveredByUnion(List<int> group, List<List<int>> allGroups, int[,] kmap, int targetValue)
    {
        var groupCells = Enumerable.Range(0, group.Count / 2).Select(i => Tuple.Create(group[i * 2], group[i * 2 + 1])).ToHashSet();
        var unionCells = new HashSet<Tuple<int, int>>();
        foreach (var other in allGroups)
        {
            if (other == group) continue;
            for (int i = 0; i < other.Count; i += 2)
                unionCells.Add(Tuple.Create(other[i], other[i + 1]));
        }

        foreach (var cell in groupCells)
            if (!unionCells.Contains(cell) && kmap[cell.Item1, cell.Item2] == targetValue)
                return false;
        return true;
    }

    private string MinimizeKarnaughToDNF(int[,] kmap, int n, List<char> variableOrder)
    {
        List<List<int>> groups = FindKarnaughGroups(kmap, 1, n);
        List<string> terms = new List<string>();

        foreach (var group in groups)
        {
            List<HashSet<int>> varValues = Enumerable.Range(0, n).Select(_ => new HashSet<int>()).ToList();
            for (int i = 0; i < group.Count; i += 2)
            {
                int row = group[i];
                int col = group[i + 1];
                List<int> values = GetVariableValues(row, col, n);
                for (int j = 0; j < n; j++)
                    varValues[j].Add(values[j]);
            }

            List<string> termParts = new List<string>();
            for (int i = 0; i < n; i++)
                if (varValues[i].Count == 1)
                    termParts.Add(varValues[i].First() == 1 ? variableOrder[i].ToString() : "!" + variableOrder[i]);

            if (termParts.Count == 0) return "1";
            terms.Add("(" + string.Join("/\\", termParts) + ")");
        }
        return terms.Count == 0 ? "0" : string.Join("\\/", terms);
    }

    private List<int> GetVariableValues(int row, int col, int n)
    {
        List<int> values = new List<int>();
        if (n == 1)
        {
            values.Add(col);
        }
        else if (n == 2)
        {
            values.Add(row);
            values.Add(col);
        }
        else if (n == 3)
        {
            values.Add(row);
            var bc = GetBCFromColumn(col);
            values.Add(bc.Item1);
            values.Add(bc.Item2);
        }
        else if (n == 4)
        {
            var ab = GetABFromRow(row);
            values.Add(ab.Item1);
            values.Add(ab.Item2);
            var cd = GetCDFromColumn(col);
            values.Add(cd.Item1);
            values.Add(cd.Item2);
        }
        return values;
    }

    private Tuple<int, int> GetABFromRow(int row)
    {
        switch (row)
        {
            case 0: return Tuple.Create(0, 0);
            case 1: return Tuple.Create(0, 1);
            case 2: return Tuple.Create(1, 1);
            case 3: return Tuple.Create(1, 0);
            default: throw new ArgumentException("Invalid row index");
        }
    }

    private Tuple<int, int> GetCDFromColumn(int col) => GetBCFromColumn(col);

    private Tuple<int, int> GetBCFromColumn(int col)
    {
        switch (col)
        {
            case 0: return Tuple.Create(0, 0);
            case 1: return Tuple.Create(0, 1);
            case 2: return Tuple.Create(1, 1);
            case 3: return Tuple.Create(1, 0);
            default: throw new ArgumentException("Invalid column index");
        }
    }
    
    public string MinimizeSKNFByKarnaugh(string sknf)
    {
        List<List<string>> parsedSKNF = ParseFormule(sknf);
        List<char> variableOrder = GetVariableOrder(parsedSKNF);
        int n = variableOrder.Count;
        if (n > 4) throw new ArgumentException("Поддерживается до 4 переменных");

        List<List<int>> cases = CaseGenerator.GenerateCases(n);
        var casesValuesPairs = FindSKNFValues(cases, parsedSKNF, variableOrder);
        int[,] kmap = BuildKarnaughMap(casesValuesPairs, n);
        IOSystem.PrintKarnaughMap(kmap);
        return MinimizeKarnaughToCNF(kmap, n, variableOrder);
    }

    private Dictionary<List<int>, int> FindSKNFValues(List<List<int>> cases, List<List<string>> parsedSKNF, List<char> variableOrder)
    {
        var results = new Dictionary<List<int>, int>();
        foreach (var caseValues in cases)
        {
            bool formulaResult = true;
            foreach (var disjunction in parsedSKNF)
            {
                bool disjunctionResult = false;
                foreach (var literal in disjunction)
                {
                    bool isNegated = literal[0] == '!';
                    char varName = isNegated ? literal[1] : literal[0];
                    int varIndex = variableOrder.IndexOf(varName);
                    int varValue = caseValues[varIndex];
                    bool literalResult = isNegated ? (varValue == 0) : (varValue == 1);
                    if (literalResult)
                    {
                        disjunctionResult = true;
                        break;
                    }
                }
                if (!disjunctionResult)
                {
                    formulaResult = false;
                    break;
                }
            }
            results.Add(caseValues, formulaResult ? 1 : 0);
        }
        return results;
    }

    private string MinimizeKarnaughToCNF(int[,] kmap, int n, List<char> variableOrder)
    {
        List<List<int>> groups = FindKarnaughGroups(kmap, 0, n);
        List<string> terms = new List<string>();

        foreach (var group in groups)
        {
            List<HashSet<int>> varValues = Enumerable.Range(0, n).Select(_ => new HashSet<int>()).ToList();
            for (int i = 0; i < group.Count; i += 2)
            {
                int row = group[i];
                int col = group[i + 1];
                List<int> values = GetVariableValues(row, col, n);
                for (int j = 0; j < n; j++)
                    varValues[j].Add(values[j]);
            }

            List<string> termParts = new List<string>();
            for (int i = 0; i < n; i++)
                if (varValues[i].Count == 1)
                    termParts.Add(varValues[i].First() == 1 ? "!" + variableOrder[i] : variableOrder[i].ToString());

            if (termParts.Count == 0) return "0";
            terms.Add("(" + string.Join("\\/", termParts) + ")");
        }
        return terms.Count == 0 ? "1" : string.Join("/\\", terms);
    }
}