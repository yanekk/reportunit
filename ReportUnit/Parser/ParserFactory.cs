using System.Linq;
using ReportUnit.DependencyInjection;
using ReportUnit.Parsers;

namespace ReportUnit.Parser
{
    internal class ParserFactory
    {
        public static ITestFileParser GetParserFor(string filePath)
        {
            var resolvers = DI.ResolveAll<ITestFileResolver>();
            var resolver = resolvers.FirstOrDefault(r => r.IsCompatibile(filePath));
            return resolver?.GetParser();
        }
    }
}
