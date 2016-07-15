using System.Collections.Generic;
using System.IO;
using ReportUnit.Logging;
using ReportUnit.Model;
using ReportUnit.Parsers;
using ReportUnit.Razor;

namespace ReportUnit.Reporting
{
    public class ReportingService : IReportingService
    {
        private readonly IParserResolvingService _parserResolvingService;
        private readonly Logger _logger = Logger.GetLogger();

        public ReportingService(IParserResolvingService parserResolvingService)
        {
            _parserResolvingService = parserResolvingService;
        }

        public void CreateReport(IEnumerable<FileInfo> inputFiles, DirectoryInfo outputDirectory)
        {
            var templateEngine = new TemplateEngine(outputDirectory.FullName);

            if (!outputDirectory.Exists)
                Directory.CreateDirectory(outputDirectory.FullName);

            var summary = new Summary();
	
        	foreach (var filePath in inputFiles)
        	{
            	var testFileParser = GetTestFileParser(filePath.FullName);
        	    if (testFileParser == null)
                    continue;

        	    var report = testFileParser.Parse(filePath.FullName);
                summary.AddReport(report);
        	}

            if (summary.Reports.Count > 1)
            {
                summary.InsertIndexSideNavLink();
                templateEngine.Save(summary);
            }

			foreach (var report in summary.Reports)
            {
                report.SideNavLinks = summary.SideNavLinks;
                ArtifactsCopier.CopyTo(report, outputDirectory);
                templateEngine.Save(report);
            }
            AssetsCopier.CopyTo(outputDirectory);
        }

        private ITestFileParser GetTestFileParser(string inputFile)
        {
            var parser = _parserResolvingService.FindParserForFile(inputFile);

            if (parser == null)
                _logger.Info("The file " + inputFile + " doesn't contain test results");
            else
                _logger.Info(string.Format("The file {0} contain {1} test results", inputFile, parser.TypeName));
            return parser;
        }
    }
}
