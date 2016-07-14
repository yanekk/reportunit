using System.Xml;
using ReportUnit.Parser;

namespace ReportUnit.Parsers.NUnit
{
    public class NUnitTestFileResolver : ParserResolverFor<NUnitTestFileParser>, ITestFileResolver
    {
        protected override string AllowedFileExtension => "xml";

        protected override bool IsHeaderCompatibile(XmlDocument document)
        {
            // NUnit
            // NOTE: not all nunit test files (ie when have nunit output format from other test runners) will contain the environment node
            //            but if it does exist - then it should have the nunit-version attribute
            var envNode = document.SelectSingleNode("//environment");
            if (envNode != null && envNode.Attributes != null && envNode.Attributes["nunit-version"] != null)
                return true;

            // check for test-suite nodes - if it has those - its probably nunit tests
            var testSuiteNodes = document.SelectNodes("//test-suite");
            return (testSuiteNodes != null && testSuiteNodes.Count > 0);
        }
    }
}
