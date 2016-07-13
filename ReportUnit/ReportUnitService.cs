using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using ReportUnit.Logging;
using ReportUnit.Model;
using ReportUnit.Parser;
using ReportUnit.Razor;
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
            	var testRunner = GetTestRunner(filePath.FullName);
        	    if (testRunner.Equals(TestRunner.Unknown))
                    continue;

        	    var report = Parse(testRunner, filePath.FullName);
                summary.AddReport(report);
        	}

            if (summary.Reports.Count > 1)
                _templateEngine.Save(summary);

			foreach (var report in summary.Reports)
            {
                report.SideNavLinks = summary.SideNavLinks;
                CopyArtifacts(report);
                _templateEngine.Save(report);
            }
            CopyAssetFiles();
        }

        private void CopyArtifacts(Report report)
        {
            var allTests = report.TestSuiteList.SelectMany(suite => suite.TestList);
            var testsWithArtifacts = allTests.Where(t => t.HasArtifacts()).ToList();
            if (!testsWithArtifacts.Any())
                return;

            var artifactBaseDirectory = Path.Combine("Artifacts\\", report.FileName);
            Directory.CreateDirectory(Path.Combine(_outputDirectory.FullName, artifactBaseDirectory));
            foreach(var test in testsWithArtifacts)
            {
                var artifactPath = Path.Combine(artifactBaseDirectory, test.ArtifactSet.DirectoryName);
                Directory.CreateDirectory(Path.Combine(_outputDirectory.FullName, artifactPath));
                foreach(var artifact in test.ArtifactSet.Artifacts)
                {
                    artifact.FilePath = Path.Combine(artifactPath, artifact.FileName);
                    File.Copy(
                        Path.Combine(test.ArtifactSet.BasePath, artifact.FileName), 
                        Path.Combine(_outputDirectory.FullName, artifact.FilePath), true);
                }
            }
        }

        private void CopyAssetFiles()
        {
            var targetDirectory = Path.Combine(_outputDirectory.FullName, "assets");
            Directory.CreateDirectory(targetDirectory);
            foreach (var sourceFile in Directory.EnumerateFiles(".\\assets"))
            {
                var fileName = Path.GetFileName(sourceFile);
                var targetFile = Path.Combine(targetDirectory, fileName);
                File.Copy(sourceFile, targetFile, true);
            }
        }

        private TestRunner GetTestRunner(string inputFile)
        {
            var testRunner = new ParserFactory(inputFile).GetTestRunnerType();
            _logger.Info("The file " + inputFile + " contains " + Enum.GetName(typeof(TestRunner), testRunner) + " test results");
            return testRunner;
        }

        private Report Parse(TestRunner testRunner, string filePath)
        {
            var parser = (IParser)Assembly.GetExecutingAssembly().CreateInstance(string.Format("ReportUnit.Parser." + testRunner));
            return parser.Parse(filePath);
        }
    }
}
