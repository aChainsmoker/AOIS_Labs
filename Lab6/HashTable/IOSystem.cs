namespace HashTableNS;

public static class IOSystem
{
    private static bool _exitState;

    public static void Run(HashTable hashTable)
    {
        _exitState = false;
        while (true)
        {
            PrintMenu();
            var input = Console.ReadLine();
            HandleInput(input, hashTable);
            if (_exitState)
                return;
        }
    }

    private static void PrintMenu()
    {
        Console.WriteLine("1. Вставить новый элемент");
        Console.WriteLine("2. Поиск элемента по ключу");
        Console.WriteLine("3. Обновить данные по ключу");
        Console.WriteLine("4. Удалить элемент по ключу");
        Console.WriteLine("5. Вывести содержимое хеш-таблицы");
        Console.WriteLine("0. Выход");
        Console.Write("Выберите действие: ");
    }

    public static void HandleInput(string input, HashTable hashTable)
    {
        switch (input)
        {
            case "1":
                HandleInsert(hashTable);
                break;
            case "2":
                HandleSearch(hashTable);
                break;
            case "3":
                HandleUpdate(hashTable);
                break;
            case "4":
                HandleDelete(hashTable);
                break;
            case "5":
                PrintTable(hashTable);
                break;
            case "0":
                _exitState = true;
                break;
            default:
                Console.WriteLine("Неверный ввод!");
                break;
        }
    }

    private static void HandleInsert(HashTable hashTable)
    {
        Console.Write("Введите ключ: ");
        string key = Console.ReadLine();
        Console.Write("Введите данные: ");
        string data = Console.ReadLine();
        try { hashTable.Insert(key, data); }
        catch (ArgumentException e)
        {
            Console.WriteLine(e.Message);
            return;
        }
        Console.WriteLine("Элемент вставлен.");
    }

    private static void HandleSearch(HashTable hashTable)
    {
        Console.Write("Введите ключ для поиска: ");
        string key = Console.ReadLine();
        string result = hashTable.Search(key);
        if (result != null)
        {
            Console.WriteLine($"Найдено: {result}");
        }
        else
        {
            Console.WriteLine("Ключ не найден.");
        }
    }

    private static void HandleUpdate(HashTable hashTable)
    {
        Console.Write("Введите ключ для обновления: ");
        string key = Console.ReadLine();
        Console.Write("Введите новые данные: ");
        string newData = Console.ReadLine();
        try { hashTable.Update(key, newData); }
        catch (KeyNotFoundException e)
        {
            Console.WriteLine(e.Message);
            return;
        }
        Console.WriteLine("Данные обновлены.");
    }

    private static void HandleDelete(HashTable hashTable)
    {
        Console.Write("Введите ключ для удаления: ");
        string key = Console.ReadLine();
        try { hashTable.Delete(key); }
        catch (KeyNotFoundException e)
        {
            Console.WriteLine(e.Message);
            return;
        }
        Console.WriteLine("Элемент удален.");
    }
    
    public static void PrintTable(HashTable hashTable)
    {
        for (int i = 0; i < hashTable.tableSize; i++)
        {
            var list = hashTable.Table[i];
            if (list.Count == 0)
            {
                Console.WriteLine($"{i+1}: [                                                        ]");
            }
            else
            {
                int indentCount = 1;
                foreach (var entry in list)
                {
                    if (entry != list.First())
                    {
                        string indent = String.Empty;
                        for (int j = 0; j < indentCount; j++)
                            indent += "\t";
                        Console.Write(indent + "->");
                        indentCount++;
                    }
                    else
                        Console.Write($"{i+1}.");
                    int v = hashTable.CalculateV(entry.ID);
                    int h = hashTable.GetHash(v);
                    Console.WriteLine($"[V: {v} | h(V): {h} | {entry}]");
                }
            }
        }
    }
}