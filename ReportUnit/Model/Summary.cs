using System.Collections.Generic;

namespace ReportUnit.Model
{
    public class Summary : IRenderable
    {
        public readonly List<Report> Reports = new List<Report>();

        public void AddReport(Report report)
        {
            Reports.Add(report);
            SideNavLinks.Add(new SideNavLink(report.FileName));
        }

        public List<SideNavLink> SideNavLinks = new List<SideNavLink>();

        public Summary()
        {
            SideNavLinks.Add(new SideNavLink("Index"));
        }

        public string TemplateName { get { return "Summary"; } }
        public string FileName { get { return "Index"; } }
    }
}
