using System.Collections.Generic;
using System.IO;

namespace ReportUnit.Utils.CommandLineOptions
{
    public class ExecutionParameters : IExecutionParameters
    {
        private readonly List<FileInfo> _inputFiles = new List<FileInfo>();
        private readonly DirectoryInfo _outputDirectory;
        private readonly string _engineName;

        public ExecutionParameters(FileInfo[] inputFiles, DirectoryInfo outputDirectory, string engineName)
        {
            _inputFiles.AddRange(inputFiles);
            _outputDirectory = outputDirectory;
            _engineName = engineName;
        }

        public FileInfo[] GetInputFiles()
        {
            return _inputFiles.ToArray();
        }

        public DirectoryInfo GetOutputDirectory()
        {
            return _outputDirectory;
        }

        public string GetEngineName()
        {
            return _engineName;
        }
    }
}
