using System.Collections.Generic;
using System.IO;
using System.Linq;
using ReportUnit.Model;

namespace ReportUnit.ReportEngines.Csv
{
    public class CsvReportingEngine : IReportingEngine
    {
        public string Name => "Csv";

        public void CreateReport(Summary summary, DirectoryInfo outputDirectory)
        {
            var outputFile = Path.Combine(outputDirectory.FullName, "CsvReport.csv");
            var errorTypes = new Dictionary<string, string>
            {
                {"paymenterror", "PaymentError"},
                {"StaleElementReferenceException", "Stale element" }
            };

            var rows = new List<List<string>>();
            using (var csvReportFile = File.CreateText(outputFile))
            {
                rows.Add(new List<string>
                {
                    "TestReport",
                    "TestFixture",
                    "Test",
                    "Result",
                    "Stream",
                    "IsNewBusiness",
                    "Error",
                    "ErrorType"
                });

                foreach (var report in summary.Reports)
                {
                    foreach (var testSuite in report.TestSuiteList)
                    {
                        foreach (var test in testSuite.TestList)
                        {
                            var errorType = "Unknown error";
                            if (test.StatusMessage != null && errorTypes.Any(e => test.StatusMessage.Contains(e.Key)))
                            {
                                errorType = errorTypes.First(e => test.StatusMessage.Contains(e.Key)).Value;
                            }
                            rows.Add(new List<string>
                            {
                                report.FileName,
                                testSuite.Name,
                                test.Name,
                                test.Status.ToString(),
                                testSuite.Name.Substring(0, testSuite.Name.IndexOf(".")),
                                testSuite.Name.Contains("NewBusiness").ToString(),
                                test.StatusMessage?.Replace("\n", " "),
                                errorType
                            });
                        }
                    }
                }
                foreach (var row in rows)
                {
                    csvReportFile.WriteLine(string.Join("\t", row));
                }
            }
        }
    }
}
