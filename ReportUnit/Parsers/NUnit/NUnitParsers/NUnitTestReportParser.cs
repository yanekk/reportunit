using System.Linq;
using System.Xml.Linq;
using ReportUnit.Model;
using ReportUnit.Utils;

namespace ReportUnit.Parsers.NUnit.NUnitParsers
{
    public static class NUnitTestReportParser
    {
        public static Report Parse(XDocument reportDoc)
        {
            var report = new Report();

            // report counts
            report.Total = reportDoc.Descendants("test-case").Count();

            report.Passed = GetCountableAttributeOrDefault(reportDoc.Root, "passed");
            if(report.Passed == 0)
            {
                report.Passed = reportDoc
                    .Descendants("test-case")
                    .Count(x => x.Attribute("executed").Value == "True" && x.Attribute("success").Value == "True");
            }

            report.Failed = GetCountableAttributeOrDefault(reportDoc.Root, "failures");
            report.Errors = GetCountableAttributeOrDefault(reportDoc.Root, "errors");

            report.Inconclusive = GetCountableAttributeOrDefault(reportDoc.Root, "inconclusive");
            report.Inconclusive += GetCountableAttributeOrDefault(reportDoc.Root, "not-run");

            report.Skipped = GetCountableAttributeOrDefault(reportDoc.Root, "skipped");
            report.Skipped += GetCountableAttributeOrDefault(reportDoc.Root, "ignored");

            // report duration
            report.StartTime = reportDoc.Root.GetAttributeValueOrDefault("start-time")
                            ?? string.Format("{0} {1}",
                                reportDoc.Root.GetAttributeValueOrDefault("date"),
                                reportDoc.Root.GetAttributeValueOrDefault("time")).Trim();

            report.EndTime = reportDoc.Root.GetAttributeValueOrDefault("end-time");

            // report status messages
            var testSuiteTypeAssembly = reportDoc.Descendants("test-suite")
                .Where(x => x.Attribute("result").Value.Equals("Failed") && x.Attribute("type").Value.Equals("Assembly"));

            report.StatusMessage = testSuiteTypeAssembly != null && testSuiteTypeAssembly.Count() > 0
                ? testSuiteTypeAssembly.First().Value
                : "";

            return report;
        }

        private static int GetCountableAttributeOrDefault(XElement element, string attribute)
        {
            var countable = element.GetAttributeValueOrDefault(attribute);
            return countable != null ? int.Parse(countable) : 0;
        }
    }
}
