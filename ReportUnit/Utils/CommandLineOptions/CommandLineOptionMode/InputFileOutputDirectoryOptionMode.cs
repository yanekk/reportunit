using System.IO;
using ReportUnit.Parsers;

namespace ReportUnit.Utils.CommandLineOptions.CommandLineOptionMode
{
    public class InputFileOutputDirectoryOptionMode : FileBasedOptionMode, ICommandLineOptionMode
    {
        public InputFileOutputDirectoryOptionMode(ITestFileParserResolver[] parserResolvers) : base(parserResolvers)
        {

        }

        public bool AreCompatibile(CommandLineOptions options)
        {
            return options.GetOutput() != null && FileExistsAndHasCorrectExtension(options.GetInput());
        }

        public FileInfo[] GetInputFiles(CommandLineOptions options)
        {
            return new[] { new FileInfo(options.GetInput()) };
        }

        public DirectoryInfo GetOutputDirectory(CommandLineOptions options)
        {
            return new DirectoryInfo(options.GetOutput());
        }

        public string Usage()
        {
            return "\"path-to-test-file\" \"output-folder\"";
        }
    }
}
