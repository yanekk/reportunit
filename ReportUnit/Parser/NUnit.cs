using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using ReportUnit.Model;
using ReportUnit.Utils;
using ReportUnit.Parser.NUnitParsers;

namespace ReportUnit.Parser
{
    public class NUnit : IParser
    {
        public Report Parse(string resultsFile)
        {
            var doc = XDocument.Load(resultsFile);
            var report = NUnitTestReportParser.Parse(doc);

            report.RunInfo = CreateRunInfo(doc, report, resultsFile);
            report.FileName = Path.GetFileNameWithoutExtension(resultsFile);
            report.AssemblyName = doc.Root.GetAttributeValueOrDefault("name");
            report.TestRunner = TestRunner.NUnit;

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

                    report.CategoryList.AddRange(test.CategoryList);
                    test.CategoryList.AddRange(testSuite.Categories);

                    testSuite.TestList.Add(test);
                });
                testSuite.Status = ReportUtil.GetFixtureStatus(testSuite.TestList);
                report.TestSuiteList.Add(testSuite);
            });

            //Sort category list so it's in alphabetical order
            report.CategoryList.Sort();
            return report;
        }

        private Dictionary<string, string> CreateRunInfo(XDocument doc, Report report, string resultsFile)
        {
            var result = new Dictionary<string, string>();
            if (doc.Root.Element("environment") == null)
                return result;

            var runInfo = new RunInfo();
            runInfo.TestRunner = report.TestRunner;

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
