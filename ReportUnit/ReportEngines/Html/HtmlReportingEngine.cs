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
            var templatesNamespace = GetType().Namespace + ".Templates";
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
                templateService.Save(report);
            }
            AssetsCopier.CopyTo(outputDirectory);
        }
    }
}
