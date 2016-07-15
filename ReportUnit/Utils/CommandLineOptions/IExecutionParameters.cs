using System.IO;

namespace ReportUnit.Utils.CommandLineOptions
{
    public interface IExecutionParameters
    {
        FileInfo[] GetInputFiles();
        DirectoryInfo GetOutputDirectory();
    }
}
