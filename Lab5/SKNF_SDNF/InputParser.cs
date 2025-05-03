namespace SKNF_SDNF;

public class InputParser
{
     private readonly char[] _operations = new char[] { '&', '|', '!', '>', '~', '^' };
     private List<char> _letters = new List<char>();
     private Dictionary<string, int> _values = new Dictionary<string, int>();
     
     public List<char> Letters {get => _letters;}
     public Dictionary<string, int> Values {get => _values;}

     public string ReformFormula(string formula)
     {
          Stack<char> formulaStack = new Stack<char>();
          string reformedFormula = String.Empty;
               
          for (int k = 0; k < formula.Length; k++)
          {

               if (formula[k] == '(')
                    formulaStack.Push(formula[k]);
               if (formula[k] == ')')
               {

                    while (formulaStack.Peek() != '(')
                    {
                         reformedFormula += formulaStack.Pop();
                    }

                    formulaStack.Pop();
               }


               if ((formula[k] >= 'a' && formula[k] <= 'z') || (formula[k] >= 'A' && formula[k] <= 'Z'))
               {
                    reformedFormula += formula[k];
                    if(!_letters.Contains(formula[k]))
                        _letters.Add(formula[k]);
               }


               if (_operations.Contains(formula[k]))
               {
                    while (formulaStack.Count != 0 && PrioSet(formulaStack.Peek()) >= PrioSet(formula[k]))
                    {
                         reformedFormula += formulaStack.Pop();
                    }

                    formulaStack.Push(formula[k]);
               }
          }
          
          while (formulaStack.Count != 0)
          {
               reformedFormula += formulaStack.Pop();
          }

        _letters.Sort();
          return reformedFormula;
     }

     public Dictionary<string, int> FindSubformulasAndItsValue(string formula)
     {
          Dictionary<string, int> subformulasAndItsValues = new Dictionary<string, int>(_values);
          Stack<string> formulaStack = new Stack<string>();

          for (int i = 0; i < formula.Length; i++)
          {
               if (_operations.Contains(formula[i]))
               {
                    string rez, var1, var2;
                    if (formula[i] == '!')
                    {
                         var1 = formulaStack.Pop();
                         rez = formula[i] + WrapInBracketsIfNeeded(var1);
                         subformulasAndItsValues.Add(rez, CalculateValue(subformulasAndItsValues[var1], formula[i]));
                         formulaStack.Push(rez);
                         continue;
                    }
                    var1 = formulaStack.Pop();
                    var2 = formulaStack.Pop();
                    rez = WrapInBracketsIfNeeded(var2) + formula[i] + WrapInBracketsIfNeeded(var1);
                    subformulasAndItsValues.Add(rez, CalculateValue(subformulasAndItsValues[var2], subformulasAndItsValues[var1], formula[i]));
                    formulaStack.Push(rez);
               }
               else
               {
                    formulaStack.Push(formula[i].ToString());
               }
          }
          return subformulasAndItsValues;
     }
     
     public List<string> FindSubformulas(string reformedFormula)
     {
          List<string> subformulas = new List<string>(_letters.Select(l => l.ToString()));
          Stack<string> formulaStack = new Stack<string>();

          for (int i = 0; i < reformedFormula.Length; i++)
          {
               if (_operations.Contains(reformedFormula[i]))
               {
                    string rez, var1, var2;
                    if (reformedFormula[i] == '!')
                    {
                         var1 = formulaStack.Pop();
                         rez = reformedFormula[i] + WrapInBracketsIfNeeded(var1);
                         subformulas.Add(rez);
                         formulaStack.Push(rez);
                         continue;
                    }
                    var1 = formulaStack.Pop();
                    var2 = formulaStack.Pop();
                    rez = WrapInBracketsIfNeeded(var2) + reformedFormula[i] + WrapInBracketsIfNeeded(var1);
                    subformulas.Add(rez);
                    formulaStack.Push(rez);
               }
               else
               {
                    formulaStack.Push(reformedFormula[i].ToString());
               }
          }
          return subformulas.Distinct().ToList();
     }

     private string WrapInBracketsIfNeeded(string formula)
     {
          if(formula.Length >= 3)
               return new String("(" + formula + ")");
          return formula;
     }

     private int PrioSet(char operation)
     {
          switch (operation)
          {
               case '!': return 3;
               case '&': case '|': case '~': case '>': return 2;
               case '(': return 1;
          }
          return 0;
     }

     public void AssignValues(List<int> numberKit)
     {
          _values.Clear();
          int numberIndex = 0;
          foreach (char symbol in _letters)
          {
               if (((symbol >= 'a' && symbol <= 'z') || (symbol >= 'A' && symbol <= 'Z')) && !_values.ContainsKey(symbol.ToString()))
               {
                    _values.Add(symbol.ToString(), numberKit[numberIndex++]);
               }
          }
     }

     private int CalculateValue(int value1, int value2, char operation)
     {
          switch (operation)
          {
               case '&': return (Convert.ToBoolean(value1) && Convert.ToBoolean(value2)) ? 1 : 0;
               case '|': return (Convert.ToBoolean(value1) || Convert.ToBoolean(value2)) ? 1 : 0;
               case '>' :
                    if (value1 == 1 && value2 == 0)
                         return 0;
                    else
                         return 1;
               case '~':
                    if ((value1 == 1 && value2 == 1) || (value1 == 0 && value2 == 0))
                         return 1;
                    else 
                         return 0;
               case '^':
                    return (value1 != value2) ? 1 : 0;
               default: 
                    return -1;
          }
     }

     private int CalculateValue(int value1, char operation)
     {
          if (operation == '!')
               return Convert.ToInt32(!Convert.ToBoolean(value1));
          return -1;
     }
}