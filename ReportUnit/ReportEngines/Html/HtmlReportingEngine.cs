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

            foreach (var report in summary.Reports)
            {
                report.SideNavLinks = summary.SideNavLinks;
                ArtifactsCopier.CopyTo(report, outputDirectory);
                if(report.XmlFileContents != null)
                    ArtifactsCopier.SaveOriginalXmlContents(report, outputDirectory);
                templateService.Save(report);
            }
            var assetsNamespace = engineNamespace + ".Assets";
            AssetsCopier.CopyTo(assetsNamespace, outputDirectory);
        }
    }
}
