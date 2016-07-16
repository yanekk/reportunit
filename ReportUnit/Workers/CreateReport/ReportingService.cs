using System;
using System.Collections.Generic;
using System.IO;
using ReportUnit.Logging;
using ReportUnit.Model;
using ReportUnit.Parsers;
using ReportUnit.ReportEngines;

namespace ReportUnit.Workers.CreateReport
{
    public class ReportingService : IReportingService
    {
        private readonly IParserResolvingService _parserResolvingService;
        private readonly IReportingEngineResolvingService _reportingEngineResolvingService;
        private readonly ILogger _logger;

        public ReportingService(
            IParserResolvingService parserResolvingService, 
            IReportingEngineResolvingService reportingEngineResolvingService,
            ILogger logger)
        {
            _parserResolvingService = parserResolvingService;
            _reportingEngineResolvingService = reportingEngineResolvingService;
            _logger = logger;
        }

        public void CreateReport(string reportingEngineName, IEnumerable<FileInfo> inputFiles, DirectoryInfo outputDirectory)
        {
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

            var reportingEngine = GetReportingEngine(reportingEngineName);
            reportingEngine.CreateReport(summary, outputDirectory);
        }

        private IReportingEngine GetReportingEngine(string reportingEngineName)
        {
            var reportingEngine = _reportingEngineResolvingService.ResolveByName(reportingEngineName);
            if (reportingEngine == null)
                throw new Exception($"Reporting engine {reportingEngineName} not found, available engines: " +
                                    string.Join(", ", _reportingEngineResolvingService.AvailableEngineNames));
            return reportingEngine;
        }

        private ITestFileParser GetTestFileParser(string inputFile)
        {
            var parser = _parserResolvingService.FindParserForFile(inputFile);
            _logger.Info(parser == null
                ? $"The file {inputFile} doesn't contain test results"
                : $"The file {inputFile} contain {parser.TypeName} test results");
            return parser;
        }
    }
}
