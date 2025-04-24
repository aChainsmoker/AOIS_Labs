namespace Lab1;

public class FloatBinaryNumber : BinaryNumber
{
    private const int BinShift = 127;
    private const int MantissaSize = 23;
    private const int PowerBinShortenedSize = 8;
    private int _power;
    public int[] ConvertToStraightBinary(float number)
    {
        Reset(); 
        
        _number[0] = number < 0 ? 1 : 0;
        
        int integerPart = Math.Abs((int)number);
        for (int i = BitSize/4-1; i > 0 && integerPart > 0; i--)
        {
            _number[i] = integerPart % 2;
            integerPart /= 2;
        }
        
        float fractionalPart = Math.Abs(number) - (int)Math.Abs(number); 
        for (int i = BitSize/4; i < BitSize && fractionalPart > 0; i++)
        {
            fractionalPart *= 2; 
            _number[i] = (int)fractionalPart; 
            if (fractionalPart >= 1)
            {
                fractionalPart -= 1;
            }
        }
        
        int powerExp = 0;
        
        if(_number.Take(8).Sum() > 0)
            while (_number.Take(8).Sum() > 1 || _number[7] != 1)
            {
                MoveBitsToRight(this,1);
                powerExp++;
            }
        else
            while (_number.Take(8).Sum() <= 0 || _number[7] != 1)
            {
                MoveBitsToLeft(this,1);
                powerExp--;
            }
        
        int power = BinShift + powerExp;
        
        BinaryNumber powerBin = new BinaryNumber();
        powerBin.ConvertToStraightBinary(power);
        
        int[] powerBinShortened = new int[PowerBinShortenedSize];
        powerBinShortened[0] = powerBin.Number[0];
        for (int i = BitSize - 1; i >= BitSize - PowerBinShortenedSize; i--)
        {
            powerBinShortened[i - (BitSize-PowerBinShortenedSize)] = powerBin.Number[i];
        }
        
        int[] mantissa = new int[MantissaSize]; 

        for (int i = BitSize /4; i < BitSize-1; i++)
        {
            mantissa[i-BitSize/4] = _number[i];
        }

        if (number < 0)
            _number[0] = 1;  
        powerBinShortened.CopyTo(_number, 1);
        mantissa.CopyTo(_number, PowerBinShortenedSize+1);

        _power = power;
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
        
        int exponent = 0;
        int power = 1;
        for (int i = 8; i > 0; i--)
        {
            if (number[i] == 1)
            {
                exponent += power;
            }
            power *= 2;
        }
        exponent -= 127; 
     
        float mantissa = 1.0f; 
        float fractionPower = 0.5f;
    
        for (int i = 9; i < 32; i++)
        {
            if (number[i] == 1)
            {
                mantissa += fractionPower;
            }
            fractionPower /= 2;
        }
        
        float result = mantissa * (float)Math.Pow(2, exponent);
        return isNegative == 1 ? -result : result;
    }
    
    public static FloatBinaryNumber operator +(FloatBinaryNumber number1, FloatBinaryNumber number2)
    {
        BinaryNumber tempMantissa1 = new BinaryNumber();
        BinaryNumber tempMantissa2 = new BinaryNumber();
        BinaryNumber tempMantissaSumResult = new BinaryNumber();
        FloatBinaryNumber result = new FloatBinaryNumber();

        for (int i = 0; i < MantissaSize; i++)
        {
            tempMantissa1.Number[i] = number1.Number[BitSize - MantissaSize + i];
            tempMantissa2.Number[i] = number2.Number[BitSize - MantissaSize + i];
        }
        
        MoveBitsToRight(tempMantissa1, 1);
        MoveBitsToRight(tempMantissa2, 1);
        tempMantissa1.Number[0] = 1;
        tempMantissa2.Number[0] = 1;
        MoveBitsToRight(tempMantissa1, 1);
        MoveBitsToRight(tempMantissa2, 1);

        FloatBinaryNumber maxPowerNumber = number1;
        if(number1._power > number2._power)
            MoveBitsToRight(tempMantissa2, number1._power - number2._power);
        else if (number1._power < number2._power)
        {
            MoveBitsToRight(tempMantissa1, number2._power - number1._power);
            maxPowerNumber = number2;
        }

        tempMantissaSumResult = tempMantissa1 + tempMantissa2;
        result._number[0] = maxPowerNumber.Sign;
        
        if (tempMantissaSumResult.Number[0] == 1)
        {
            MoveBitsToLeft(tempMantissaSumResult, 1);
            BinaryNumber temp = new BinaryNumber();
            temp.ConvertToStraightBinary(maxPowerNumber._power + 1);
            for (int i = 0; i < PowerBinShortenedSize; i++)
            {
                result._number[PowerBinShortenedSize-i] = temp.Number[BitSize-1-i];
            }
            
        }
        else
        {
            MoveBitsToLeft(tempMantissaSumResult, 2);
            for (int i = 0; i < PowerBinShortenedSize; i++)
            {
                result._number[i+1] = maxPowerNumber.Number[i+1];
            }
        }
        
        for (int i = 0; i < MantissaSize; i++)
        {
            result._number[i+PowerBinShortenedSize+1] = tempMantissaSumResult.Number[i];
        }
        
        return result;
    }
    
}