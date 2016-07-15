namespace ReportUnit.Parsers
{
    public interface ITestFileParserResolver
    {
        bool IsCompatibile(string filePath);
        ITestFileParser GetParser();
        string AllowedFileExtension { get; }
    }
}
