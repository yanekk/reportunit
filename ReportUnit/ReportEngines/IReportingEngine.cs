using System.IO;
using ReportUnit.Model;

namespace ReportUnit.ReportEngines
{
    public interface IReportingEngine
    {
        string Name { get; }
        void CreateReport(Summary reports, DirectoryInfo outputDirectory);
    }
}
