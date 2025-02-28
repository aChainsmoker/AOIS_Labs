namespace Lab1;

public class BinaryNumber
{
    protected const int BitSize = 32;
    
    protected int[] _number = new int[BitSize];
    protected NumberState _state;

    public int[] Number {get => _number; set => _number = value; }
    public int Sign { get => _number[0]; set => _number[0] = value; }
    
    
    protected enum NumberState
    {
        Straight,
        Reverse,
        Additional
    }
    public int[] ConvertToStraightBinary(int number)
    {
        Reset();
        
        _number[0] = number < 0 ? 1 : 0;
        for (int i = BitSize-1; Math.Abs(number) > 0; i--)
        {
            _number[i] = (Math.Abs(number) % 2) == 1 ? 1 : 0;
            number /= 2;
        }
        
        _state = NumberState.Straight;
        
        return _number;
    }
    
    public int[] ConvertToReverseBinary(int number)
    {
        _number = ConvertToStraightBinary(number);

        if (_number[0] == 0)
            return _number;
        
        for (int i = 1; i < BitSize; i++)
        {
            if (_number[i] == 1)
                _number[i] = 0;
            else
                _number[i] = 1;
        }

        _state = NumberState.Reverse;
        
        return _number;
    }

    public int[] ConvertToAdditionalBinary(int number)
    {
        _number = ConvertToReverseBinary(number);
        
        if (_number[0] == 0)
            return _number;
        
        BinaryNumber result = new BinaryNumber();
        result._number[0] = _number[0];
        BinaryNumber oneBit = new BinaryNumber();
        oneBit._number[BitSize-1] = 1;

        result = this + oneBit;
        
        _number = result._number;
        
        _state = NumberState.Additional;
        return _number;
    }

    public  int GetDecimal()
    {
        switch (_state)
        {
            case NumberState.Straight:
                return GetDecimalFromStraight(_number);
            case NumberState.Reverse:
                return GetDecimalFromReverse(_number);
            case NumberState.Additional:
                return GetDecimalFromAdditional(_number);
            default:
                return -1;
        }
    }


        private int GetDecimalFromStraight(int[] number)
        {
            int isNegative = number[0];
                    
            int magnitude = 0;
            int power = 1; 
                    
            for (int i = BitSize-1; i > 0; i--)
            {
                if (number[i]==1) 
                {
                    magnitude += power;
                }
                power *= 2; 
            }
            return isNegative == 1 ? -magnitude : magnitude;
        }
    
    private int GetDecimalFromReverse(int[] number)
    {
        if(number[0]==0)
            return GetDecimalFromStraight(number);
        
        int[] reversedNumber = new int[32];
        reversedNumber[0] = number[0];
        
        for (int i = 1; i < BitSize; i++)
        {
            if (number[i] == 1)
                reversedNumber[i] = 0;
            else
                reversedNumber[i] = 1;
        }
        
        return GetDecimalFromStraight(reversedNumber);
    }

    private int GetDecimalFromAdditional(int[] number)
    {
        if(number[0]==0)
            return GetDecimalFromStraight(number);
        
        BinaryNumber result = new BinaryNumber();
        result.Sign = number[0];
        
        for (int i = 1; i < BitSize; i++)
        {
            if (number[i] == 1)
                result._number[i] = 0;
            else
                result._number[i] = 1;
        }
        
        BinaryNumber oneBit = new BinaryNumber();
        oneBit._number[BitSize-1] = 1;
        result += oneBit;
        
        return GetDecimalFromStraight(result._number);
    }


    public static BinaryNumber operator +(BinaryNumber number1, BinaryNumber number2)
    {
        
        BinaryNumber result = new BinaryNumber();
        result._state = NumberState.Additional;
        int remaining = 0;
        
        for (int i = BitSize-1; i >= 0; i--)
        {
            int digitSum = number1._number[i] + number2._number[i] + remaining;
            if(remaining != 0)
                remaining--;
            if (digitSum > 1)
            {
                remaining++;
                result._number[i] = digitSum == 2 ? 0 : 1;
            }
            else
            {
                result._number[i] = digitSum;
            }
        }
        return result;
    }

    public static BinaryNumber operator -(BinaryNumber number1, BinaryNumber number2)
    {
        BinaryNumber reversedNumber = new BinaryNumber();
        number2._number.CopyTo(reversedNumber._number, 0); 
        
        for(int i =0; i< BitSize; i++)
            reversedNumber._number[i] = reversedNumber._number[i] == 0 ? 1 : 0;
        
        BinaryNumber oneBit = new BinaryNumber();
        oneBit._number[BitSize-1] = 1;
        reversedNumber = reversedNumber + oneBit;
        
        return number1 + reversedNumber;
    }

    public static BinaryNumber operator *(BinaryNumber number1, BinaryNumber number2)
    {
        int binStartPos1 = GetFirstActiveBitPosition(number1);
        int binStartPos2 = GetFirstActiveBitPosition(number2);
        
        BinaryNumber[] intermediateMultiplication = new BinaryNumber[BitSize-binStartPos2];
        
        for(int i =0; i< intermediateMultiplication.Length; i++)
            intermediateMultiplication[i] = new BinaryNumber();

        for (int i = 0; i < BitSize - binStartPos2; i++)
        {
            for (int j = 0; j < BitSize - binStartPos1; j++)
            {
                intermediateMultiplication[i]._number[BitSize-1-j-i] = number1._number[BitSize-1-j] * number2._number[BitSize-1-i]; 
            }
        }
        
        BinaryNumber result = new BinaryNumber();

        for (int i = 0; i < intermediateMultiplication.Length; i++)
        {
            result += intermediateMultiplication[i];
        }
        
        if((number1.Sign == 1 && number2.Sign == 0) || (number2.Sign == 1 && number1.Sign == 0))
            result.Sign = 1;
        
        result._state = NumberState.Straight;
        return result;
    }

    public static FixedPointBinaryNumber operator /(BinaryNumber number1, BinaryNumber number2)
    {
        FixedPointBinaryNumber result = new FixedPointBinaryNumber();
        int binStartPos1 = GetFirstActiveBitPosition(number1);
        int binStartPos2 = GetFirstActiveBitPosition(number2);
        
        BinaryNumber tempResuced = new BinaryNumber();
        MakeAbsolute(number1)._number.CopyTo(tempResuced._number, 0);
        BinaryNumber tempSubstracted = new BinaryNumber();
        MakeAbsolute(number2)._number.CopyTo(tempSubstracted._number, 0);
        MoveBitsToRight(tempResuced, Math.Abs((BitSize - binStartPos1)-(BitSize - binStartPos2)));
        BinaryNumber tempResult = new BinaryNumber();
        tempResult = tempResuced - tempSubstracted;
        
        if(tempResult.Sign == 0)
            result._number[BitSize/2-1] = 1;
        else
        {
            MakeAbsolute(number1)._number.CopyTo(tempResult._number, 0);
        }

        for (int i =BitSize - ((BitSize - binStartPos1)-(BitSize-binStartPos2)); i < BitSize; i++)
        {
            
            MoveBitsToLeft(tempResult, 1);
            tempResult._number[BitSize - 1] = number1._number[i];

            int resultSign = (tempResult - number2).Sign;
            
            MoveBitsToLeft(result, 1);
            if (resultSign == 0)
            {
                result._number[BitSize / 2 - 1] = 1;
                tempResult = tempResult - number2;
            }
            else
                result._number[BitSize/2-1] = 0;
            
        }

        int k = 0;
        while (BitSize / 2 + k < BitSize && tempResult._number.Contains(1))
        {
            MoveBitsToLeft(tempResult, 1);
            tempResult._number[BitSize - 1] = 0;
            int resultSign = (tempResult - MakeAbsolute(number2)).Sign;
            if (resultSign == 0)
            {
                result._number[BitSize / 2 + k] = 1;
                tempResult = tempResult - number2;
            }
            else
                result._number[BitSize/2+k] = 0;
            k++;
        }
        
        if((number1.Sign == 1 && number2.Sign == 0) || (number2.Sign == 1 && number1.Sign == 0))
            result.Sign = 1;
        result._state = NumberState.Straight;
        
        return result;
    }

    public void Reset()
    {
        _number = new int[BitSize];
    }

    private static int GetFirstActiveBitPosition(BinaryNumber number)
    {
        for(int i =1; i< BitSize; i++)
            if (number._number[i] == 1)
            {
                return i;
            }
        return -1;
    }

    protected static void MoveBitsToLeft(BinaryNumber number, int offset)
    {
        BinaryNumber temp = new BinaryNumber();
        for (int i = BitSize - 1 - offset; i >= 0; i--)
        {
            temp._number[i] = number._number[i+offset];
        }
        temp._number.CopyTo(number._number, 0);
    }
    protected static void MoveBitsToRight(BinaryNumber number, int offset)
    {
        BinaryNumber temp = new BinaryNumber();
        for (int i = offset; i < BitSize; i++)
        {
            temp._number[i] = number._number[i-offset];
        }
        temp._number.CopyTo(number._number, 0);
    }

    protected static BinaryNumber MakeAbsolute(BinaryNumber number)
    {
        BinaryNumber result = new BinaryNumber();
        number._number.CopyTo(result._number, 0);
        result.Sign = 0;
        return result;
    }
}