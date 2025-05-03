namespace AssociativeProcessor;

class Program
{
    static void Main(string[] args)
    {
        MatrixProcessor processor = new MatrixProcessor();
        IOSystem.Run(processor);
    }
}