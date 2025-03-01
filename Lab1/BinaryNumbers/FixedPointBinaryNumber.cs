namespace Lab1;

public class FixedPointBinaryNumber : BinaryNumber
{
    public int[] ConvertToStraightBinary(float number)
    {
        Reset(); 
        
        _number[0] = number < 0 ? 1 : 0;
        
        int integerPart = Math.Abs((int)number);
        for (int i = BitSize/2-1; i > 0 && integerPart > 0; i--)
        {
            _number[i] = integerPart % 2;
            integerPart /= 2;
        }


        float fractionalPart = Math.Abs(number) - (int)Math.Abs(number);
        for (int i = BitSize/2; i < BitSize && fractionalPart > 0; i++)
        {
            fractionalPart *= 2;
            _number[i] = (int)fractionalPart; 
            if (fractionalPart >= 1)
            {
                fractionalPart -= 1; 
            }
        }

        _state = NumberState.Straight; 
        return _number;
    }

    
    public float GetDecimal()
    {
        switch (_state)
        {
            case NumberState.Straight:
                return GetDecimalFromStraight(_number);
            default:
                return -1f;
        }
    }

    private float GetDecimalFromStraight(int[] number)
    {
        int isNegative = number[0];

        int integerPart = 0;
        int power = 1;

        for (int i = BitSize/2-1; i > 0; i--) 
        {
            if (number[i] == 1)
            {
                integerPart += power;
            }
            power *= 2;
        }

        float fractionalPart = 0f;
        float fractionPower = 0.5f; 

        for (int i = BitSize/2; i < BitSize; i++) 
        {
            if (number[i] == 1)
            {
                fractionalPart += fractionPower;
            }
            fractionPower /= 2; 
        }
        
        float result = integerPart + fractionalPart;
        return isNegative == 1 ? -result : result;
    }

}