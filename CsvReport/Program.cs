using System.IO;
using System.Linq;
using ReportUnit.Parsers.NUnit;

namespace CsvReport
{
    class Program
    {
        static void Main(string[] args)
        {
            var inputFolder = @"C:\Users\jan.krolikowski\Documents\FCG\Test runs\2016-06-10";
            var outputFile = @"C:\Users\jan.krolikowski\Documents\FCG\Test runs\2016-06-10\CsvReport.csv";

            var filePathList = new DirectoryInfo(inputFolder).GetFiles("*.xml", SearchOption.AllDirectories)
                .OrderByDescending(f => f.CreationTime);
            var testParser = new NUnitTestFileParser();

            using (var csvReportFile = File.CreateText(outputFile))
            {
                foreach (var inputFile in filePathList)
                {
                    var report = testParser.Parse(inputFile.FullName);
                    foreach (var testSuite in report.TestSuiteList)
                    {
                        foreach (var test in testSuite.TestList)
                        {
                            csvReportFile.WriteLine("{0};{1};{2};{3}", testSuite.Name, test.Name, test.Status, test.StatusMessage.Replace("\n", " "));
                        }
                    }
                }
            }
        }
    }
}
