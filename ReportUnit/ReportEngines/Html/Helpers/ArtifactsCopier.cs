using System.Collections.Generic;
using System.IO;
using System.Linq;
using ReportUnit.Model;

namespace ReportUnit.ReportEngines.Html.Helpers
{
    internal class ArtifactsManager
    {
        private readonly string _outputDirectory;

        public ArtifactsManager(DirectoryInfo outputDirectory)
        {
            _outputDirectory = outputDirectory.FullName;
        }

        public void CopyReportedArtifacts(Report report)
        {
            var artifactBaseDirectory = GetArtifactsDirectoryFor(report);
            foreach (var test in GetTestsFrom(report))
            {
                var artifactPath = Path.Combine(artifactBaseDirectory, test.ArtifactSet.DirectoryName);
                Directory.CreateDirectory(Path.Combine(_outputDirectory, artifactPath));
                foreach (var artifact in test.ArtifactSet.Artifacts)
                {
                    artifact.FilePath = Path.Combine(artifactPath, artifact.FileName).Replace("\\", "/");
                    File.Copy(
                        Path.Combine(test.ArtifactSet.BasePath, artifact.FileName),
                        Path.Combine(_outputDirectory, artifact.FilePath), true);
                }
            }
        }

        public void SaveOriginalXmlContents(Report report)
        {
            var artifactBaseDirectory = GetArtifactsDirectoryFor(report);
            var inputXmlFileContents = report.XmlFileContents;
            var inputXmlFilePath = Path.Combine(_outputDirectory, artifactBaseDirectory, $"{report.FileName}.xml");
            using (var inputFile = File.CreateText(inputXmlFilePath))
            {
                inputFile.Write(inputXmlFileContents);
            }
        }

        private IEnumerable<Test> GetTestsFrom(Report report)
        {
            return report.TestSuiteList
                .SelectMany(suite => suite.TestList)
                .Where(t => t.HasArtifacts());
        }

        private string GetArtifactsDirectoryFor(Report report)
        {
            var artifactPath = Path.Combine(".\\Artifacts\\", report.FileName);
            var fullArtifactPath = Path.Combine(_outputDirectory, artifactPath);
            if (!Directory.Exists(fullArtifactPath))
                Directory.CreateDirectory(fullArtifactPath);
            return artifactPath;
        }
    }
}
