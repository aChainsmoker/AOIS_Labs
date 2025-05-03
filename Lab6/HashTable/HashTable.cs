namespace HashTableNS;

public class HashTable
{
    private const string russianAlphabet = "АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ";
    
    private readonly LinkedList<HashEntry>[] table;
    private int numberOfEntries = 0;
        
    public readonly int tableSize = 20;
    public LinkedList<HashEntry>[] Table { get => table;}

    public HashTable()
    {
        table = new LinkedList<HashEntry>[tableSize];
        for (int i = 0; i < tableSize; i++)
        {
            table[i] = new LinkedList<HashEntry>();
        }
    }

    public int CalculateV(string key)
    {
        if (key.Length < 2) return key.Length > 0 ? GetLetterValue(key[0]) : 0;
        int v = GetLetterValue(key[0]) * 33 + GetLetterValue(key[1]);
        return v;
    }

    private int GetLetterValue(char letter)
    {
        int index = russianAlphabet.IndexOf(char.ToUpper(letter));
        return index >= 0 ? index : 0; 
    }

    public int GetHash(int v)
    {
        return v % tableSize;
    }

    public void Insert(string key, string data)
    {
        int v = CalculateV(key);
        int hash = GetHash(v);

        foreach (var chain in table)
        {
            foreach (var entry in chain)
            {
                if (entry.ID == key && !entry.D)
                {
                    throw new ArgumentException("Ключ уже существует в таблице.");
                }
            }
        }

        var list = table[hash];
        HashEntry newEntry = new HashEntry(key, data);

        if (list.Count > 0)
        {
            newEntry.C = true;
            newEntry.T = false;
            var last = list.Last.Value;
            last.T = false;
            last.Po = newEntry;
        }

        list.AddLast(newEntry);
        numberOfEntries++;
    }

    public string Search(string key)
    {
        int v = CalculateV(key);
        int hash = GetHash(v);
        var list = table[hash];

        foreach (var entry in list)
        {
            if (entry.ID == key && !entry.D)
            {
                return entry.Data;
            }
        }
        return null;
    }

    public void Update(string key, string newData)
    {
        int v = CalculateV(key);
        int hash = GetHash(v);
        var list = table[hash];

        foreach (var entry in list)
        {
            if (entry.ID == key && !entry.D)
            {
                entry.Data = newData;
                return;
            }
        }
        throw new KeyNotFoundException("Ключ не найден.");
    }

    public void Delete(string key)
    {
        int v = CalculateV(key);
        int h = GetHash(v);
        var list = table[h];

        var node = list.First;
        while (node != null && !(node.Value.ID == key && !node.Value.D))
            node = node.Next;

        if (node == null)
            throw new KeyNotFoundException($"Ключ '{key}' не найден.");

        var entry = node.Value;
        entry.D = true;

        bool hasPrev = node.Previous != null;
        bool hasNext = node.Next != null;

        if (!hasPrev && !hasNext)
        {
            entry.U = false;
            list.Remove(node);
        }
        else if (hasPrev && !hasNext)
        {
            var prev = node.Previous.Value;
            prev.T = true;
            prev.Po = null;

            entry.U = false;
            list.Remove(node);
        }
        else
        {
            var nextNode = node.Next;
            var next = nextNode.Value;

            entry.ID = next.ID;
            entry.Data = next.Data;
            entry.C = next.C;
            entry.T = next.T;
            entry.L = next.L;
            entry.Po = next.Po;
            entry.D = next.D;
            entry.U = next.U;

            next.U = false;
            list.Remove(nextNode);
        }

        numberOfEntries--;
    }
}