using System.IO;
using ReportUnit.Parsers;

namespace ReportUnit.Utils.CommandLineOptions.CommandLineOptionMode
{
    public class InputDirectoryOptionMode : DirectoryBasedOptionMode, ICommandLineOptionMode
    {
        public InputDirectoryOptionMode(ITestFileParserResolver[] parserResolvers) : base(parserResolvers)
        {
        }

        public bool AreCompatibile(CommandLineOptions options)
        {
            return options.GetOutput() == null && Directory.Exists(options.GetInput());
        }

        public FileInfo[] GetInputFiles(CommandLineOptions options)
        {
            return GetInputFilesInDirectory(options.GetInput());
        }

        public DirectoryInfo GetOutputDirectory(CommandLineOptions options)
        {
            return new DirectoryInfo(options.GetInput());
        }

        public string Usage()
        {
            return "\"path-to-folder\"";
        }
    }
}
