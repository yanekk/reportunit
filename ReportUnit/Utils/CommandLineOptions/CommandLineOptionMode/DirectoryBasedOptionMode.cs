using System.IO;
using System.Linq;
using ReportUnit.Parsers;

namespace ReportUnit.Utils.CommandLineOptions.CommandLineOptionMode
{
    public abstract class DirectoryBasedOptionMode
    {
        private readonly ITestFileParserResolver[] _parserResolvers;

        protected DirectoryBasedOptionMode(ITestFileParserResolver[] parserResolvers)
        {
            _parserResolvers = parserResolvers;
        }

        protected FileInfo[] GetInputFilesInDirectory(string inputDirectoryPath)
        {
            var allowedExtensions = _parserResolvers
                .Select(r => r.AllowedFileExtension)
                .Distinct();

            var inputDirectory = new DirectoryInfo(inputDirectoryPath);
            return allowedExtensions.SelectMany(allowedTestFileExtension => inputDirectory
                .GetFiles("*" + allowedTestFileExtension, SearchOption.AllDirectories)
                .OrderByDescending(f => f.CreationTime))
                .ToArray();
        }
    }
}
