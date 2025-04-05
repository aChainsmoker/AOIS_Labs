namespace SKNF_SDNF;

public static class CaseGenerator
{
    public static List<List<int>> GenerateCases(int size)
    {
        List<List<int>> cases = new List<List<int>>();
        List<int> firstCase = new List<int>();
        for (int i = 0; i < size; i++)
            firstCase.Add(1);
        int maxValue = BinaryNumberConverter.GetDecimalFromStraight(firstCase);

        for (int i = maxValue; i >= 0; i--)
        {
            cases.Add(BinaryNumberConverter.ConvertToStraightBinary(i, size));
        }

        cases.Reverse();
        return cases;
    }
}