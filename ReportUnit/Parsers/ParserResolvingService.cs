using System.Collections.Generic;
using System.Linq;

namespace ReportUnit.Parsers
{
    public class ParserResolvingService : IParserResolvingService
    {
        private readonly IEnumerable<ITestFileParserResolver> _parserResolvers;

        public ParserResolvingService(ITestFileParserResolver[] parserResolvers)
        {
            _parserResolvers = parserResolvers;
        }

        public ITestFileParser FindParserForFile(string filePath)
        {
            var resolver = _parserResolvers.FirstOrDefault(r => r.IsCompatibile(filePath));
            return resolver?.GetParser();
        }

        public string[] GetAllowedExtensions()
        {
            return _parserResolvers
                .Select(resolver => resolver.AllowedFileExtension)
                .Distinct()
                .ToArray();
        }
    }
}
