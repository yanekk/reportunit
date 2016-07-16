using System;
using System.IO;
using ReportUnit.Parsers;

namespace ReportUnit.Utils.CommandLineOptions.CommandLineOptionMode
{
    public class InputDirectoryOutputDirectoryOptionMode : DirectoryBasedOptionMode, ICommandLineOptionMode
    {
        public InputDirectoryOutputDirectoryOptionMode(ITestFileParserResolver[] parserResolvers) : base(parserResolvers)
        {
        }

        public bool AreCompatibile(CommandLineOptions options)
        {
            return options.GetOutput() != null && Directory.Exists(options.GetInput());
        }

        public FileInfo[] GetInputFiles(CommandLineOptions options)
        {
            return GetInputFilesInDirectory(options.GetInput());
        }

        public DirectoryInfo GetOutputDirectory(CommandLineOptions options)
        {
            return new DirectoryInfo(options.GetOutput());
        }

        public string Usage()
        {
            return "\"input-folder\" \"output-folder\"";
        }
    }
}
