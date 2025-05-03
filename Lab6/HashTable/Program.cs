namespace HashTableNS;

class Program
{
    static void Main(string[] args)
    {
        HashTable hashTable = new HashTable();
        
        hashTable.Insert("Милан", "Город в Италии");
        hashTable.Insert("Минск", "Столица Беларуси");
        hashTable.Insert("Минеральные Воды", "Курортный город в России");
        hashTable.Insert("Берлин", "Столица Германии");

        hashTable.Insert("Санкт-Петербург", "Город на Неве");
        hashTable.Insert("Самара", "Город на Волге");

        hashTable.Insert("Киев", "Столица Украины");
        hashTable.Insert("Лондон", "Столица Великобритании");
        hashTable.Insert("Париж", "Столица Франции");
        hashTable.Insert("Мадрид", "Столица Испании");
        hashTable.Insert("Рим", "Столица Италии");
        hashTable.Insert("Осло", "Столица Норвегии");
        hashTable.Insert("Токио", "Столица Японии");
        
        IOSystem.Run(hashTable);
    }
}