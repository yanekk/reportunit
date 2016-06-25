using System.Xml.Linq;
using ReportUnit.Model;
using ReportUnit.Utils;

namespace ReportUnit.Parser.NUnitParsers
{
    internal static class NUnitTestSuiteParser
    {
        public static TestSuite Create(XElement testSuiteNode)
        {
            var testSuite = new TestSuite();
            testSuite.Name = FocusHelper.ExtractTestMethodName(testSuiteNode);

            // Suite Time Info
            testSuite.StartTime = testSuiteNode.GetAttributeValueOrDefault("start-time") ??
                                  testSuiteNode.GetAttributeValueOrDefault("time");
            testSuite.EndTime = testSuiteNode.GetAttributeValueOrDefault("end-time");

            // any error messages and/or stack-trace
            var failure = testSuiteNode.Element("failure");
            if (failure != null)
            {
                testSuite.StatusMessage = failure.GetChildElementValueOrDefault("message");
                testSuite.StackTrace = failure.GetChildElementValueOrDefault("stack-trace");
            }

            // get test suite level categories
            testSuite.Categories.AddRange(NUnitParsingHelper.GetCategories(testSuiteNode, false));
            return testSuite;
        }
    }
}
