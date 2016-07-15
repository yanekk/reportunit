using System.Collections.Generic;
using System.IO;

namespace ReportUnit.Utils.CommandLineOptions
{
    public class ExecutionParameters : IExecutionParameters
    {
        private readonly List<FileInfo> _inputFiles = new List<FileInfo>();
        private readonly DirectoryInfo _outputDirectory;

        public ExecutionParameters(FileInfo[] inputFiles, DirectoryInfo outputDirectory)
        {
            _inputFiles.AddRange(inputFiles);
            _outputDirectory = outputDirectory;
        }

        public FileInfo[] GetInputFiles()
        {
            return _inputFiles.ToArray();
        }

        public DirectoryInfo GetOutputDirectory()
        {
            return _outputDirectory;
        }
    }
}
