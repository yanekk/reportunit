using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace ReportUnit.Parser
{
    public static class FocusHelper
    {
        public static string ExtractTestMethodName(XElement testFixtureNode)
        {
            var result = new List<string>();
            AppendNamespacesOfParents(testFixtureNode, result);
            result.Reverse();

            var fullTestCaseName  = string.Join(".", result);
            fullTestCaseName = fullTestCaseName.Replace("Focus.Automation.Tests.", "");
            return fullTestCaseName;
        }

        private static void AppendNamespacesOfParents(XElement testSuiteNode, ICollection<string> currentFullTestCaseName)
        {
            if (testSuiteNode.Attribute("type").Value == "Assembly")
                return;
            currentFullTestCaseName.Add(testSuiteNode.Attribute("name").Value);
            AppendNamespacesOfParents(testSuiteNode.Parent.Parent, currentFullTestCaseName);
        }

        public static string ExtractTestCaseName(XElement testCaseNode)
        {
            var fullTestCaseName = testCaseNode.Attribute("name").Value;
            var testCaseNameRegex = new Regex(@"^.*\.(?<test_case_name>[\w]+?(?:\(.+\))*)$");
            return !testCaseNameRegex.IsMatch(fullTestCaseName) 
                ? fullTestCaseName 
                : testCaseNameRegex.Match(fullTestCaseName).Groups["test_case_name"].Value;
        }
    }
}
