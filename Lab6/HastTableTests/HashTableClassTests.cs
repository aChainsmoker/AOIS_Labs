using Microsoft.VisualStudio.TestTools.UnitTesting;
using HashTableNS;

[TestClass]
public class HashTableClassTests
{
    [TestMethod]
    public void Insert_NewKey_ShouldInsertSuccessfully()
    {
        var hashTable = new HashTable();
        string key = "Москва";
        string data = "Столица России";
        hashTable.Insert(key, data);
        string result = hashTable.Search(key);
        Assert.AreEqual(data, result);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void Insert_DuplicateKey_ShouldThrowException()
    {
        var hashTable = new HashTable();
        string key = "Москва";
        string data1 = "Столица России";
        string data2 = "Moscow";
        hashTable.Insert(key, data1);
        hashTable.Insert(key, data2);
    }

    [TestMethod]
    public void Insert_MultipleKeysWithSameHash_ShouldHandleCollision()
    {
        var hashTable = new HashTable();
        string key1 = "Москва";
        string data1 = "Столица России";
        string key2 = "Мост";
        string data2 = "Сооружение для переправы";
        string key3 = "Море";
        string data3 = "Большой водоём";
        hashTable.Insert(key1, data1);
        hashTable.Insert(key2, data2);
        hashTable.Insert(key3, data3);
        Assert.AreEqual(data1, hashTable.Search(key1));
        Assert.AreEqual(data2, hashTable.Search(key2));
        Assert.AreEqual(data3, hashTable.Search(key3));
    }

    [TestMethod]
    public void Update_ExistingKey_ShouldUpdateData()
    {
        var hashTable = new HashTable();
        string key = "Москва";
        string initialData = "Столица России";
        string newData = "Moscow";
        hashTable.Insert(key, initialData);
        hashTable.Update(key, newData);
        string result = hashTable.Search(key);
        Assert.AreEqual(newData, result);
    }

    [TestMethod]
    [ExpectedException(typeof(KeyNotFoundException))]
    public void Update_NonExistingKey_ShouldThrowException()
    {
        var hashTable = new HashTable();
        string key = "Неизвестный";
        string newData = "Новые данные";
        hashTable.Update(key, newData);
    }

    [TestMethod]
    public void Delete_ExistingKey_ShouldRemoveEntry()
    {
        var hashTable = new HashTable();
        string key = "Москва";
        string data = "Столица России";
        hashTable.Insert(key, data);
        hashTable.Delete(key);
        string result = hashTable.Search(key);
        Assert.IsNull(result);
    }

    [TestMethod]
    [ExpectedException(typeof(KeyNotFoundException))]
    public void Delete_NonExistingKey_ShouldThrowException()
    {
        var hashTable = new HashTable();
        string key = "Неизвестный";
        hashTable.Delete(key);
    }

    [TestMethod]
    public void Delete_FirstKeyInChain_ShouldAdjustChain()
    {
        var hashTable = new HashTable();
        string key1 = "Москва";
        string data1 = "Столица России";
        string key2 = "Мост";
        string data2 = "Сооружение для переправы";
        string key3 = "Море";
        string data3 = "Большой водоём";
        hashTable.Insert(key1, data1);
        hashTable.Insert(key2, data2);
        hashTable.Insert(key3, data3);
        hashTable.Delete(key1);
        Assert.IsNull(hashTable.Search(key1));
        Assert.AreEqual(data2, hashTable.Search(key2));
        Assert.AreEqual(data3, hashTable.Search(key3));
    }

    [TestMethod]
    public void Delete_MiddleKeyInChain_ShouldAdjustChain()
    {
        var hashTable = new HashTable();
        string key1 = "Москва";
        string data1 = "Столица России";
        string key2 = "Мост";
        string data2 = "Сооружение для переправы";
        string key3 = "Море";
        string data3 = "Большой водоём";
        hashTable.Insert(key1, data1);
        hashTable.Insert(key2, data2);
        hashTable.Insert(key3, data3);
        hashTable.Delete(key2);
        Assert.AreEqual(data1, hashTable.Search(key1));
        Assert.IsNull(hashTable.Search(key2));
        Assert.AreEqual(data3, hashTable.Search(key3));
    }

    [TestMethod]
    public void Delete_LastKeyInChain_ShouldAdjustChain()
    {
        var hashTable = new HashTable();
        string key1 = "Москва";
        string data1 = "Столица России";
        string key2 = "Мост";
        string data2 = "Сооружение для переправы";
        string key3 = "Море";
        string data3 = "Большой водоём";
        hashTable.Insert(key1, data1);
        hashTable.Insert(key2, data2);
        hashTable.Insert(key3, data3);
        hashTable.Delete(key3);
        Assert.AreEqual(data1, hashTable.Search(key1));
        Assert.AreEqual(data2, hashTable.Search(key2));
        Assert.IsNull(hashTable.Search(key3));
    }


    [TestMethod]
    public void Insert_MultipleKeysHashingToZero_ShouldHandleCollision()
    {
        var hashTable = new HashTable();
        string key1 = "";
        string data1 = "Empty key";
        string key2 = "Moscow";
        string data2 = "Capital of Russia";
        string key3 = "123";
        string data3 = "Numbers";
        hashTable.Insert(key1, data1);
        hashTable.Insert(key2, data2);
        hashTable.Insert(key3, data3);
        Assert.AreEqual(data1, hashTable.Search(key1));
        Assert.AreEqual(data2, hashTable.Search(key2));
        Assert.AreEqual(data3, hashTable.Search(key3));
    }
}