namespace CombinatorialSchemeSynthesizer;

class Program
{
    static void Main(string[] args)
    {
        ODS3 ods3 = new ODS3();
        ods3.MinimizeCombinatorialScheme();
        
        TetradReformer tetradReformer = new TetradReformer();
        tetradReformer.ReformTetrad();
    }
}