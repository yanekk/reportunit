using System;
using System.Linq;
using System.Xml.Linq;
using ReportUnit.Model;
using ReportUnit.Utils;

namespace ReportUnit.Parser.NUnitParsers
{
    public static class NUnitTestReportParser
    {
        public static Report Parse(XDocument reportDoc)
        {
            var report = new Report();

            // report counts
            report.Total = reportDoc.Descendants("test-case").Count();
            report.Passed =
                reportDoc.Root.Attribute("passed") != null
                    ? int.Parse(reportDoc.Root.Attribute("passed").Value)
                    : reportDoc.Descendants("test-case").Count(x => x.Attribute("result").Value.Equals("success", StringComparison.CurrentCultureIgnoreCase));

            report.Failed =
                reportDoc.Root.Attribute("failed") != null
                    ? int.Parse(reportDoc.Root.Attribute("failed").Value)
                    : int.Parse(reportDoc.Root.Attribute("failures").Value);

            var errors = reportDoc.Root.GetAttributeValueOrDefault("errors");
            report.Errors = errors != null ? int.Parse(errors) : 0;

            var inconclusive = reportDoc.Root.GetAttributeValueOrDefault("inconclusive");
            report.Inconclusive = inconclusive != null ? int.Parse(inconclusive) : 0;

            var skipped = reportDoc.Root.GetAttributeValueOrDefault("skipped");
            report.Skipped = skipped != null ? int.Parse(skipped) : 0;

            var ignored = reportDoc.Root.GetAttributeValueOrDefault("ignored");
            report.Skipped += ignored != null ? int.Parse(ignored) : 0;

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
    }
}
