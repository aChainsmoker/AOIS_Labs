namespace HashTableNS;

public class HashEntry
{
    public string ID { get; set; }
    public bool C { get; set; }
    public bool U { get; set; }
    public bool T { get; set; }
    public bool L { get; set; }
    public bool D { get; set; }
    public HashEntry? Po { get; set; }
    public string Data { get; set; }

    public HashEntry(string id, string data)
    {
        ID = id;
        C = false;
        U = true;
        T = true;
        L = false;
        D = false;
        Po = null;  
        Data = data;
    }

    public override string ToString()
    {
        return $"ID: {ID} | C: {(C ? 1 : 0)} | U: {(U ? 1 : 0)} | T: {(T ? 1 : 0)} | L: {(L ? 1 : 0)} | D: {(D ? 1 : 0)} | Po: {Po?.ID} | {Data}";
    }
}