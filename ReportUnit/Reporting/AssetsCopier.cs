using System.IO;

namespace ReportUnit.Reporting
{
    internal static class AssetsCopier
    {
        public static void CopyTo(DirectoryInfo outputDirectory)
        {
            var targetDirectory = Path.Combine(outputDirectory.FullName, "assets");
            Directory.CreateDirectory(targetDirectory);

            foreach (var sourceFile in Directory.EnumerateFiles(".\\assets"))
            {
                var fileName = Path.GetFileName(sourceFile);
                var targetFile = Path.Combine(targetDirectory, fileName);
                File.Copy(sourceFile, targetFile, true);
            }
        }
    }
}
