using System.Collections.Generic;
using System.IO;

namespace ReportUnit.Workers.CreateReport
{
    public interface IReportingService
    {
        void CreateReport(string reportingEngineName, IEnumerable<FileInfo> inputFiles, DirectoryInfo outputDirectory);
    }
}
