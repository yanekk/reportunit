using System.IO;

namespace ReportUnit.Utils.CommandLineOptions
{
    public class ExecutionParameters : IExecutionParameters
    {
        public FileInfo[] InputFiles;
        public DirectoryInfo OutputDirectory;
        public string EngineName { get; set; }

        public FileInfo[] GetInputFiles()
        {
            return InputFiles;
        }

        public DirectoryInfo GetOutputDirectory()
        {
            return OutputDirectory;
        }

        public string GetEngineName()
        {
            return EngineName;
        }
    }
}
