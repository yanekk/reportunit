using System.IO;
using ReportUnit.Utils;

namespace ReportUnit.ReportEngines.Html.Helpers
{
    internal static class AssetsCopier
    {
        public static void CopyTo(string assetsNamespace, DirectoryInfo outputDirectory)
        {
            var targetDirectory = Path.Combine(outputDirectory.FullName, "assets");
            Directory.CreateDirectory(targetDirectory);

            var assetNames = ResourceHelper.GetResourceNames(assetsNamespace);
            foreach (var assetName in assetNames)
            {
                var fileName = Path.GetFileName(assetName.Remove(0, assetsNamespace.Length + 1));
                var targetFile = Path.Combine(targetDirectory, fileName);
                File.WriteAllText(targetFile, ResourceHelper.GetStringResource(assetName));
            }
        }
    }
}
