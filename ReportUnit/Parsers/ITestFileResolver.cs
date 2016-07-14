namespace ReportUnit.Parsers
{
    public interface ITestFileResolver
    {
        bool IsCompatibile(string filePath);
        ITestFileParser GetParser();
    }
}
