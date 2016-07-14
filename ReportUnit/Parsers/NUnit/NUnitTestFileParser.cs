using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using ReportUnit.Model;
using ReportUnit.Parsers.NUnit.NUnitParsers;
using ReportUnit.Utils;

namespace ReportUnit.Parsers.NUnit
{
    public class NUnitTestFileParser : ITestFileParser
    {
        public string TypeName => "NUnit";

        public Report Parse(string resultsFile)
        {
            var doc = XDocument.Load(resultsFile);
            var report = NUnitTestReportParser.Parse(doc);

            report.RunInfo = CreateRunInfo(doc, report, resultsFile);
            report.FileName = Path.GetFileNameWithoutExtension(resultsFile);
            report.AssemblyName = doc.Root.GetAttributeValueOrDefault("name");
            report.TestParser = this;

            var suites = doc
                .Descendants("test-suite")
                .Where(x => x.Attribute("type").Value.Equals("TestFixture", StringComparison.CurrentCultureIgnoreCase));
            
            suites.AsParallel().ToList().ForEach(ts =>
            {
                var testSuite = NUnitTestSuiteParser.Create(ts);

                // Test Cases
                ts.Descendants("test-case").AsParallel().ToList().ForEach(tc =>
                {
                    var test = NUnitTestCaseParser.Parse(tc);
                    report.AddStatus(test.Status);
                    report.AddCategories(test.CategoryList);

                    test.CategoryList.AddRange(testSuite.Categories);

                    testSuite.TestList.Add(test);
                });
                testSuite.Status = ReportUtil.GetFixtureStatus(testSuite.TestList);
                report.TestSuiteList.Add(testSuite);
            });

            return report;
        }

        private Dictionary<string, string> CreateRunInfo(XDocument doc, Report report, string resultsFile)
        {
            var result = new Dictionary<string, string>();
            if (doc.Root.Element("environment") == null)
                return result;

            var runInfo = new RunInfo();
            runInfo.TestParser = TypeName;

            XElement env = doc.Descendants("environment").First();
            runInfo.Info.Add("Test Results File", resultsFile);
            if (env.Attribute("nunit-version") != null)
                runInfo.Info.Add("NUnit Version", env.Attribute("nunit-version").Value);
            runInfo.Info.Add("Assembly Name", report.AssemblyName);
            runInfo.Info.Add("OS Version", env.Attribute("os-version").Value);
            runInfo.Info.Add("Platform", env.Attribute("platform").Value);
            runInfo.Info.Add("CLR Version", env.Attribute("clr-version").Value);
            runInfo.Info.Add("Machine Name", env.Attribute("machine-name").Value);
            runInfo.Info.Add("User", env.Attribute("user").Value);
            runInfo.Info.Add("User Domain", env.Attribute("user-domain").Value);

            return runInfo.Info;
        }
    }
}
