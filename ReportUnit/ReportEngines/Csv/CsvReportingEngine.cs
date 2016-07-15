using System.IO;
using ReportUnit.Model;

namespace ReportUnit.ReportEngines.Csv
{
    public class CsvReportingEngine : IReportingEngine
    {
        public string Name => "Csv";

        public void CreateReport(Summary summary, DirectoryInfo outputDirectory)
        {
            var outputFile = Path.Combine(outputDirectory.FullName, "CsvReport.csv");
            using (var csvReportFile = File.CreateText(outputFile))
            {
                foreach (var report in summary.Reports)
                {
                    foreach (var testSuite in report.TestSuiteList)
                    {
                        foreach (var test in testSuite.TestList)
                        {
                            csvReportFile.WriteLine("{0};{1};{2};{3}", testSuite.Name, test.Name, test.Status,
                                test.StatusMessage?.Replace("\n", " "));
                        }
                    }
                }
            }
        }
    }
}
