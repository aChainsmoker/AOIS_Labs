namespace SKNF_SDNF_Minimizer;

public static class ListListExtension
{
    public static List<List<string>> DistinctValue(this List<List<string>> list)
    {
        for (int i = 0; i < list.Count-1; i++)
        for(int j = i+1; j < list.Count; j++)
            if (list[i].SequenceEqual(list[j]))
            {
                list.Remove(list[j]);
                j--;
            }

        return list;
    }
}