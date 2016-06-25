using System.Collections.Generic;
using ReportUnit.Utils;

namespace ReportUnit.Model
{
    public class CompositeTemplate
    {
        public readonly List<Report> ReportList = new List<Report>();

        public void AddReport(Report report)
        {
            ReportList.Add(report);
            SideNavLinks += RazorHelper.CompileTemplate(report, "_Link");
        }

        public string SideNavLinks { get; internal set; }
    }
}
