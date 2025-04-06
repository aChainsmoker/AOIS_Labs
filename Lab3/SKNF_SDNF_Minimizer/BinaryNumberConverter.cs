namespace SKNF_SDNF;

public static  class BinaryNumberConverter
{
    
    public static int GetDecimalFromStraight(List<int> number)
    {
                    
        int magnitude = 0;
        int power = 1; 
                    
        for (int i =number.Count-1; i >= 0; i--)
        {
            if (number[i]==1) 
            {
                magnitude += power;
            }
            power *= 2; 
        }
        return magnitude;
    }
    
    public static List<int> ConvertToStraightBinary(int number, int size)
    {
        List<int> binaryNumber = new List<int>();
        for (int i = size-1; Math.Abs(number) > 0; i--)
        {
            binaryNumber.Add(((Math.Abs(number) % 2) == 1 ? 1 : 0));
            number /= 2;
        }
        
        while (binaryNumber.Count < size)
            binaryNumber.Add(0);
            
        binaryNumber.Reverse();
        return binaryNumber;
    }
}