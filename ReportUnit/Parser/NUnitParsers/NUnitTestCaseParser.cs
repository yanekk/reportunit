using System;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using ReportUnit.Model;
using ReportUnit.Utils;

namespace ReportUnit.Parser.NUnitParsers
{
    public class NUnitTestCaseParser
    {
        public static Test Parse(XElement testCaseNode)
        {
            var test = new Test();

            test.Name = FocusHelper.ExtractTestCaseName(testCaseNode);
            test.Status = testCaseNode.Attribute("result").Value.ToStatus();

            // TestCase Time Info
            test.StartTime = testCaseNode.GetAttributeValueOrDefault("start-time") ??
                             testCaseNode.GetAttributeValueOrDefault("time");
            test.EndTime = testCaseNode.GetAttributeValueOrDefault("end-time");

            // description
            var description = testCaseNode
                .Descendants("property")
                .Where(c => c.Attribute("name").Value.Equals("Description", StringComparison.CurrentCultureIgnoreCase));

            test.Description =
                description.Count() > 0
                    ? description.ToArray()[0].Attribute("value").Value
                    : "";

            // get test case level categories
            var categories = NUnitParsingHelper.GetCategories(testCaseNode, true);

            // if this is a parameterized test, get the categories from the parent test-suite
            var parameterizedTestElement = testCaseNode
                .Ancestors("test-suite").ToList()
                .Where(x => x.Attribute("type").Value.Equals("ParameterizedTest", StringComparison.CurrentCultureIgnoreCase))
                .FirstOrDefault();

            if (null != parameterizedTestElement)
            {
                var paramCategories = NUnitParsingHelper.GetCategories(parameterizedTestElement, false);
                categories.AddRange(paramCategories);
            }

            test.CategoryList.AddRange(categories);

            // add NUnit console output to the status message
            var failureElement = testCaseNode.Element("failure");
            if (failureElement != null)
            {
                test.StatusMessage = HttpUtility.HtmlEncode(failureElement.GetChildElementValueOrDefault("message"));
                test.StackTrace = HttpUtility.HtmlEncode(failureElement.GetChildElementValueOrDefault("stack-trace"));
            }

            if (test.Status == Status.Inconclusive)
            {
                test.StatusMessage = testCaseNode.Element("reason").Element("message").Value;
            }
            return test;
        }
    }
}
