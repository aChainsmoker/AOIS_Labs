using Lab1;

namespace AssociativeProcessor;

public class MatrixProcessor
{
    private int[,] _matrix= new int [16,16];
    
    public int[,] Matrix { get { return _matrix; } }

    public MatrixProcessor()
    {
        for(int i = 0; i < 16; i++)
            for(int j = 0; j < 16; j++)
                _matrix[i, j] = new Random().Next(0,2);
    }

    public int[] ReturnWord(int wordIndex)
    {
        int[] word = new int[16];
        int startIndex = wordIndex;
        
        for (int i = startIndex; i < startIndex + word.Length; i++)
        {
            word[i-startIndex] = _matrix[i % word.Length, wordIndex];
        }

        return word;
    }
    
    public void SetWord(int wordIndex, int[] word)
    {
        int startIndex = wordIndex;
        
        for (int i = startIndex; i < startIndex + word.Length; i++)
        {
            _matrix[i % word.Length, wordIndex] = word[i-startIndex];
        }
    }

    public int[] ReturnAdressColumn(int columnIndex)
    {
        int[] word = new int[16];
        
        for (int i = 0; i < word.Length; i++)
        {
            word[i] = _matrix[columnIndex++ % word.Length, i];
        }

        return word;
    }

    public void SetAdressColumn(int columnIndex, int[] word)
    {
        for (int i = 0; i < word.Length; i++)
        {
            _matrix[columnIndex++ % word.Length, i] = word[i];
        }
    }

    public int[] Conjunction(int[] word1, int[] word2)
    {
        int[] result = new int[16];

        for (int i = 0; i < word1.Length; i++)
        {
            result[i] = (word1[i] == 1 && word2[i] == 1) ? 1 : 0;
        }
        return result;
    }
    
    public int[] ShefferOperation(int[] word1, int[] word2)
    {
        int[] result = new int[word1.Length];
        
        for (int i = 0; i < word1.Length; i++)
        {
            result[i] = (word1[i] == 1 && word2[i] == 1) ? 0 : 1;
        }
        return result;
    }
    
    public int[] RepeatFirstArgument(int[] word1)
    {
        int[] result = new int[16];

        for (int i = 0; i < word1.Length; i++)
        {
            result[i] = word1[i];
        }
        return result;
    }
    
    public int[] DenyFirstArgument(int[] word1)
    {
        int[] result = new int[16];

        for (int i = 0; i < word1.Length; i++)
        {
            result[i] = word1[i] == 0? 1 : 0;
        }
        return result;
    }

    private int[] SumFieldsOfTheWord(int[] word)
    {
        int[] result = new int[16];
        BinaryNumber numberA = new BinaryNumber();
        BinaryNumber numberB = new BinaryNumber();
        int[] partAOfTheWord = word.Skip(3).Take(4).ToArray();
        int[] partBOfTheWord = word.Skip(7).Take(4).ToArray();
        
        for (int i = partAOfTheWord.Length - 1; i >= 0; i--)
        {
            numberA.Number[i + (numberA.Number.Length - partAOfTheWord.Length)] = partAOfTheWord[i];
            numberB.Number[i + (numberB.Number.Length - partBOfTheWord.Length)] = partBOfTheWord[i];
        }
        
        BinaryNumber partSOfTheWord = new BinaryNumber();
        partSOfTheWord = numberA + numberB;
        
        for (int i = 0; i < 3; i++)
        {
            result[i] = word[i];
        }
        for (int i = 3; i < 7; i++)
        {
            result[i] = partAOfTheWord[i-(3)];
            result[i+4] = partBOfTheWord[i-(3)];
        }
        for (int i = 11; i < 16; i++)
        {
            result[i] = partSOfTheWord.Number.TakeLast(5).ToArray()[i-11];
        }

        return result;
    }

    public void FindAndSumWord(int[] searchPattern)
    {
        for (int i = 0; i < _matrix.GetLength(1); i++)
        {
            if (ReturnWord(i).Take(3).SequenceEqual(searchPattern))
            {
                int[] resultWord = SumFieldsOfTheWord(ReturnWord(i));
                SetWord(i, resultWord);
            }
        }
    }

    private (bool g, bool l) CalculateGL(int[] word, int[] searchArgument)
    {
        bool g = false;
        bool l = false;

        for (int i = 0; i < word.Length; i++)
        {
            int a_i = searchArgument[i];
            int s_j_i = word[i];

            bool newG = g || (a_i == 0 && s_j_i == 1 && !l);
            bool newL = l || (a_i == 1 && s_j_i == 0 && !g);

            g = newG;
            l = newL;
        }

        return (g, l);
    }
    
    public int[] FindNearestUnderneath(int[] searchArgument)
    {
        List<int[]> candidates = new List<int[]>();

        for (int i = 0; i < 16; i++)
        {
            int[] word = ReturnWord(i);
            var (g, l) = CalculateGL(word, searchArgument);
            if (l) candidates.Add(word);
        }

        if (candidates.Count == 0) return null;

        int[] nearest = candidates[0];
        foreach (var candidate in candidates)
        {
            var (g, l) = CalculateGL(candidate, nearest);
            if (g) nearest = candidate;
        }
        
        return nearest;
    }
    
    public int[] FindNearestAbove(int[] searchArgument)
    {
        List<int[]> candidates = new List<int[]>();

        for (int i = 0; i < 16; i++)
        {
            int[] word = ReturnWord(i);
            var (g, l) = CalculateGL(word, searchArgument);
            if (g) candidates.Add(word);
        }

        if (candidates.Count == 0) return null;

        int[] nearest = candidates[0];
        foreach (var candidate in candidates)
        {
            var (g, l) = CalculateGL(candidate, nearest);
            if (l) nearest = candidate;
        }

        return nearest;
    }
    
}