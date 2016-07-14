using ReportUnit.Model;

namespace ReportUnit.Parsers
{
    public interface ITestFileParser
    {
        Report Parse(string filePath);
        string TypeName { get; }
    }
}
