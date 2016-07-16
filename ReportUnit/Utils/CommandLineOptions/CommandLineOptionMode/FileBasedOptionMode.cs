using System.IO;
using System.Linq;
using ReportUnit.Parsers;

namespace ReportUnit.Utils.CommandLineOptions.CommandLineOptionMode
{
    public abstract class FileBasedOptionMode
    {
        private readonly ITestFileParserResolver[] _parserResolvers;

        protected FileBasedOptionMode(ITestFileParserResolver[] parserResolvers)
        {
            _parserResolvers = parserResolvers;
        }

        protected bool FileExistsAndHasCorrectExtension(string inputFile)
        {
            return File.Exists(inputFile) && FileHasCorrectExtension(inputFile);
        }

        private bool FileHasCorrectExtension(string inputFile)
        {
            var allowedExtensions = _parserResolvers.Select(r => $".{r.AllowedFileExtension}").Distinct().ToArray();
            return allowedExtensions.Contains(Path.GetExtension(inputFile));
        }
    }
}
