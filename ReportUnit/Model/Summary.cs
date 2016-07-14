using System.Collections.Generic;
using System.Linq;

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

        public string TemplateName { get { return "Summary"; } }
        public string FileName { get { return "Index"; } }

        public double Total
        {
            get { return Reports.Sum(r => r.Total); }
        }

        public double Passed
        {
            get { return Reports.Sum(r => r.Passed); }
        }

        public double Failed
        {
            get { return Reports.Sum(r => r.Failed); }
        }

        public double Others
        {
            get { return Reports.Sum(r => r.Others); }
        }

        public void InsertIndexSideNavLink()
        {
            SideNavLinks.Insert(0, new SideNavLink("Index"));
        }
    }
}
