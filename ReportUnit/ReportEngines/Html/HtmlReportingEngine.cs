using System.IO;
using ReportUnit.Model;
using ReportUnit.ReportEngines.Html.Helpers;

namespace ReportUnit.ReportEngines.Html
{
    public class HtmlReportingEngine : IReportingEngine
    {
        public string Name => "Html";

        public void CreateReport(Summary summary, DirectoryInfo outputDirectory)
        {
            var engineNamespace = GetType().Namespace;
            var templatesNamespace = engineNamespace + ".Templates";
            var templateService = new RazorTemplateService(templatesNamespace, outputDirectory.FullName);
            if (summary.Reports.Count > 1)
            {
                summary.InsertIndexSideNavLink();
                templateService.Save(summary);
            }

            var artifactsManager = new ArtifactsManager(outputDirectory);
            foreach (var report in summary.Reports)
            {
                report.SideNavLinks = summary.SideNavLinks;
                artifactsManager.CopyReportedArtifacts(report);
                if(report.XmlFileContents != null)
                    artifactsManager.SaveOriginalXmlContents(report);
                templateService.Save(report);
            }
            var assetsNamespace = engineNamespace + ".Assets";
            AssetsCopier.CopyTo(assetsNamespace, outputDirectory);
        }
    }
}
