namespace ReportUnit.Parsers
{
    public interface IParserResolvingService
    {
        ITestFileParser FindParserForFile(string filePath);
        string[] GetAllowedExtensions();
    }
}
