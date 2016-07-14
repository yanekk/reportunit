using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using ReportUnit.Logging;
using ReportUnit.Model;
using ReportUnit.Parser;
using ReportUnit.Parsers;
using ReportUnit.Razor;
using ReportUnit.Reporting;
using ReportUnit.Utils;

namespace ReportUnit
{
    class ReportUnitService
    {
        private readonly DirectoryInfo _outputDirectory;
        private readonly List<FileInfo> _inputFiles = new List<FileInfo>();
        private readonly TemplateEngine _templateEngine;

        private readonly Logger _logger = Logger.GetLogger();
        
        private ReportUnitService(FileInfo inputFile, DirectoryInfo outputDirectory) : this(outputDirectory)
        {
            _inputFiles.Add(inputFile);
        }

        private ReportUnitService(DirectoryInfo inputDirectory, DirectoryInfo outputDirectory) : this(outputDirectory)
        {
            _inputFiles.AddRange(inputDirectory
                .GetFiles("*.xml", SearchOption.AllDirectories)
                .OrderByDescending(f => f.CreationTime));
        }

        private ReportUnitService(DirectoryInfo outputDirectory)
        {
            _outputDirectory = outputDirectory;
            _templateEngine = new TemplateEngine(_outputDirectory.FullName);
        }

        public static ReportUnitService GetInstanceByOptions(CommandLineOptions options)
        {
            if (options.InputDirectory != null)
                return new ReportUnitService(options.InputDirectory, options.OutputDirectory);
            if (options.InputFile != null)
                return new ReportUnitService(options.InputFile, options.OutputDirectory);
            throw new Exception("Invalid options provided");
        }

        public void CreateReport()
        {
            if (!_outputDirectory.Exists)
                Directory.CreateDirectory(_outputDirectory.FullName);

            var summary = new Summary();
	
        	foreach (var filePath in _inputFiles)
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
                _templateEngine.Save(summary);
            }

			foreach (var report in summary.Reports)
            {
                report.SideNavLinks = summary.SideNavLinks;
                ArtifactsCopier.CopyTo(report, _outputDirectory);
                _templateEngine.Save(report);
            }
            AssetsCopier.CopyTo(_outputDirectory);
        }

        private ITestFileParser GetTestFileParser(string inputFile)
        {
            var parser = ParserFactory.GetParserFor(inputFile);
            if (parser == null)
                _logger.Info("The file " + inputFile + " doesn't contain test results");
            else
                _logger.Info(string.Format("The file {0} contain {1} test results", inputFile, parser.TypeName));
            return parser;
        }
    }
}
