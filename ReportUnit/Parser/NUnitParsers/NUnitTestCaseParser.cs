using System;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using ReportUnit.Model;
using ReportUnit.Utils;
using System.Collections.Generic;

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

            test.Description = ExtractDescription(testCaseNode);
            test.CategoryList.AddRange(ExtractCategories(testCaseNode));

            test.StatusMessage = ExtractStatusMessage(testCaseNode);
            test.StackTrace = ExtractStackTrace(testCaseNode);

            test.ArtifactSet = ExtractArtifactSet(testCaseNode);

            return test;
        }

        private static string ExtractDescription(XElement testCaseNode)
        {
            var description = testCaseNode
                .Descendants("property")
                .SingleOrDefault(c => c.Attribute("name").Value == "Description");
            return description != null
                ? description.GetAttributeValueOrDefault("value")
                : "";
        }

        private static IEnumerable<string> ExtractCategories(XElement testCaseNode)
        {
            var categories = NUnitParsingHelper.GetCategories(testCaseNode, true);

            // if this is a parameterized test, get the categories from the parent test-suite
            var parameterizedTestElement = testCaseNode
                .Ancestors("test-suite").ToList()
                .Where(x => x.Attribute("type").Value == "ParameterizedTest")
                .FirstOrDefault();

            if (null != parameterizedTestElement)
            {
                var paramCategories = NUnitParsingHelper.GetCategories(parameterizedTestElement, false);
                categories.AddRange(paramCategories);
            }
            return categories;
        }

        private static string ExtractStatusMessage(XElement testCaseNode)
        {
            var failureElement = testCaseNode.Element("failure");
            if (failureElement != null)
                return HttpUtility.HtmlEncode(failureElement.GetChildElementValueOrDefault("message"));

            var reasonElement = testCaseNode.Element("reason");
            if (reasonElement != null)
                return HttpUtility.HtmlEncode(reasonElement.GetChildElementValueOrDefault("message"));

            return null;
        }

        private static string ExtractStackTrace(XElement testCaseNode)
        {
            var failureElement = testCaseNode.Element("failure");
            if (failureElement != null)
                return HttpUtility.HtmlEncode(failureElement.GetChildElementValueOrDefault("stack-trace"));

            return null;
        }

        private static ArtifactSet ExtractArtifactSet(XElement testCaseNode)
        {
            var propertiesContainer = testCaseNode.Element("properties");
            if (propertiesContainer == null)
                return null;

            var artifactSessionFolderNode = propertiesContainer
                .Elements("property")
                .SingleOrDefault(element => element.GetAttributeValueOrDefault("name") == "ArtifactSessionFolder");
            if (artifactSessionFolderNode == null)
                return null;

            var artifactSessionFolder = artifactSessionFolderNode.GetAttributeValueOrDefault("value");
            var artifactSet = new ArtifactSet(artifactSessionFolder);

            var artifactNodes = artifactSessionFolderNode
                .ElementsAfterSelf()
                .Where(property => property.GetAttributeValueOrDefault("name").StartsWith("Artifact"));

            foreach(var artifactNode in artifactNodes)
            {
                artifactSet.Artifacts.Add(new Artifact(artifactNode.GetAttributeValueOrDefault("value")));
            }
            return artifactSet;
        }
    }
}
