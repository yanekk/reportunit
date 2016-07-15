using System.Collections.Generic;
using System.IO;

namespace ReportUnit.Reporting
{
    public interface IReportingService
    {
        void CreateReport(IEnumerable<FileInfo> inputFiles, DirectoryInfo outputDirectory);
    }
}
