using System.Collections.Generic;
using System.IO;
using System.Linq;
using ReportUnit.Model;

namespace ReportUnit.ReportEngines.Html.Helpers
{
    internal static class ArtifactsCopier
    {
        public static void CopyTo(Report report, DirectoryInfo outputDirectory)
        {
            var artifactBaseDirectory = Path.Combine("Artifacts\\", report.FileName);
            Directory.CreateDirectory(Path.Combine(outputDirectory.FullName, artifactBaseDirectory));
            foreach (var test in GetTestsFrom(report))
            {
                var artifactPath = Path.Combine(artifactBaseDirectory, test.ArtifactSet.DirectoryName);
                Directory.CreateDirectory(Path.Combine(outputDirectory.FullName, artifactPath));
                foreach (var artifact in test.ArtifactSet.Artifacts)
                {
                    artifact.FilePath = Path.Combine(artifactPath, artifact.FileName);
                    File.Copy(
                        Path.Combine(test.ArtifactSet.BasePath, artifact.FileName),
                        Path.Combine(outputDirectory.FullName, artifact.FilePath), true);
                }
            }
        }

        private static IEnumerable<Test> GetTestsFrom(Report report)
        {
            return report.TestSuiteList
                .SelectMany(suite => suite.TestList)
                .Where(t => t.HasArtifacts());
        }
    }
}
