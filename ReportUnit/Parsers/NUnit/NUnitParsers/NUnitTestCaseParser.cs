﻿using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using ReportUnit.Model;
using ReportUnit.Utils;

namespace ReportUnit.Parsers.NUnit.NUnitParsers
{
    public class NUnitTestCaseParser
    {
        public static Test Parse(XElement testCaseNode)
        {
            var test = new Test();

            test.Name = FocusHelper.ExtractTestCaseName(testCaseNode);
            test.Status = testCaseNode.Attribute("result").Value.ToStatus();

            // TestCase Time Info
            test.StartTime = ExtractPropertyValue(testCaseNode, "StartTime");
            test.EndTime = ExtractPropertyValue(testCaseNode, "FinishTime");

            test.Description = ExtractDescription(testCaseNode);
            test.CategoryList.AddRange(ExtractCategories(testCaseNode));

            test.StatusMessage = ExtractStatusMessage(testCaseNode);
            test.StackTrace = ExtractStackTrace(testCaseNode);

            test.ArtifactSet = ExtractArtifactSet(testCaseNode);

            return test;
        }

        private static string ExtractPropertyValue(XElement testCaseNode, string propertyName)
        {
            var property = ExtractProperty(testCaseNode, propertyName);
            return property != null ? property.GetAttributeValueOrDefault("value") : null;
        }

        private static XElement ExtractProperty(XElement testCaseNode, string propertyName)
        {
            return testCaseNode
                .Descendants("property")
                .SingleOrDefault(c => c.Attribute("name").Value == propertyName);
        }

        private static string ExtractDescription(XElement testCaseNode)
        {
            var description = ExtractProperty(testCaseNode, "Description");
            if (description != null)
                return description.GetAttributeValueOrDefault("value");

            var parameterizedTestSuite = testCaseNode
                .Ancestors("test-suite")
                .SingleOrDefault(testSuite => testSuite.GetAttributeValueOrDefault("type") == "ParameterizedTest");

            if (parameterizedTestSuite != null)
                return parameterizedTestSuite.GetAttributeValueOrDefault("description");
            return null;
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
