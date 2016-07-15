using System;
using System.Globalization;
using System.Xml.Linq;
using ReportUnit.Model;
using ReportUnit.Utils;

namespace ReportUnit.Parsers.NUnit.NUnitParsers
{
    internal static class NUnitTestSuiteParser
    {
        public static TestSuite Create(XElement testSuiteNode)
        {
            var testSuite = new TestSuite();
            testSuite.Name = FocusHelper.ExtractTestMethodName(testSuiteNode);

            var runningTime = testSuiteNode.GetAttributeValueOrDefault("time");
            if (runningTime != null)
                testSuite.RunningTime = TimeSpan.FromSeconds(double.Parse(runningTime, CultureInfo.InvariantCulture));

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
