﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

using RazorEngine;
using RazorEngine.Configuration;
using RazorEngine.Templating;
using RazorEngine.Text;

using ReportUnit.Logging;
using ReportUnit.Model;
using ReportUnit.Parser;
using ReportUnit.Utils;

namespace ReportUnit
{
    class ReportUnitService
    {
        private const string _ns = "ReportUnit.Parser";
        private Logger _logger = Logger.GetLogger();

        public ReportUnitService() { }

        public void CreateReport(string input, string outputDirectory)
        {
    		var attributes = File.GetAttributes(input);
		IEnumerable<FileInfo> filePathList;

        	 if ((FileAttributes.Directory & attributes) == FileAttributes.Directory)
        	{
				filePathList = new DirectoryInfo(input).GetFiles("*.xml", SearchOption.AllDirectories)
					.OrderByDescending(f => f.CreationTime);
	        }
	        else
	        {
				filePathList = new DirectoryInfo(Directory.GetCurrentDirectory()).GetFiles(input);
	        }

        	InitializeRazor();

        	var compositeTemplate = new CompositeTemplate();
	
        	foreach (var filePath in filePathList)
        	{
            	var testRunner = GetTestRunner(filePath.FullName);

            	if (!(testRunner.Equals(TestRunner.Unknown)))
            	{
                    IParser parser = (IParser)Assembly.GetExecutingAssembly().CreateInstance(_ns + "." + Enum.GetName(typeof(TestRunner), testRunner));
                    var report = parser.Parse(filePath.FullName);
                    compositeTemplate.AddReport(report);
                }
            }

            if (compositeTemplate.ReportList.Count > 1)
            {
                compositeTemplate.SideNavLinks = compositeTemplate.SideNavLinks.Insert(0, Templates.SideNav.IndexLink);
                SaveTemplate(compositeTemplate,
                    Path.Combine(outputDirectory, "Index.html"),
                    "ReportUnit.Templates.Summary.cshtml");
            }

			foreach (var report in compositeTemplate.ReportList)
            {
                report.SideNavLinks = compositeTemplate.SideNavLinks;
                CopyArtifacts(outputDirectory, report);

                SaveTemplate(report, 
                    Path.Combine(outputDirectory, report.FileName + ".html"), 
                    "ReportUnit.Templates.File.cshtml");          
            }
            CopyAssetFiles(outputDirectory);
        }

        private void CopyArtifacts(string outputDirectory, Report report)
        {
            var allTests = report.TestSuiteList.SelectMany(suite => suite.TestList);
            var testsWithArtifacts = allTests.Where(t => t.HasArtifacts());
            if (!testsWithArtifacts.Any())
                return;

            var artifactBaseDirectory = Path.Combine("Artifacts\\", report.FileName);
            Directory.CreateDirectory(Path.Combine(outputDirectory, artifactBaseDirectory));
            foreach(var test in testsWithArtifacts)
            {
                var artifactPath = Path.Combine(artifactBaseDirectory, test.ArtifactSet.DirectoryName);
                Directory.CreateDirectory(Path.Combine(outputDirectory, artifactPath));
                foreach(var artifact in test.ArtifactSet.Artifacts)
                {
                    artifact.FilePath = Path.Combine(artifactPath, artifact.FileName);
                    File.Copy(
                        Path.Combine(test.ArtifactSet.BasePath, artifact.FileName), 
                        Path.Combine(outputDirectory, artifact.FilePath), true);
                }
            }
        }

        private void CopyAssetFiles(string outputDirectory)
        {
            var targetDirectory = Path.Combine(outputDirectory, "assets");
            Directory.CreateDirectory(targetDirectory);
            foreach (var soruceFile in Directory.EnumerateFiles(".\\assets"))
            {
                var fileName = Path.GetFileName(soruceFile);
                var targetFile = Path.Combine(targetDirectory, fileName);
                File.Copy(soruceFile, targetFile, true);
            }
        }

        private void SaveTemplate<TModel>(TModel model, string filePath, string templateName)
        {
            var template = ResourceHelper.GetStringResource(templateName);
            var html = Engine.Razor.RunCompile(template, typeof(TModel).Name, typeof(TModel), model);
            File.WriteAllText(filePath, html);
        }

        private TestRunner GetTestRunner(string inputFile)
        {
            var testRunner = new ParserFactory(inputFile).GetTestRunnerType();

            _logger.Info("The file " + inputFile + " contains " + Enum.GetName(typeof(TestRunner), testRunner) + " test results");

            return testRunner;
        }

        private void InitializeRazor()
        {
            TemplateServiceConfiguration templateConfig = new TemplateServiceConfiguration();
            templateConfig.DisableTempFileLocking = true;
            templateConfig.EncodedStringFactory = new RawStringFactory();
            templateConfig.CachingProvider = new DefaultCachingProvider(x => { });
            var service = RazorEngineService.Create(templateConfig);
            Engine.Razor = service;
        }
    }
}
