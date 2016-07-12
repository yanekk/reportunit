using System.Collections.Generic;

using ReportUnit.Parser;

namespace ReportUnit.Model
{
    public class Report : IRenderable
    {
        private List<Status> _statusList = new List<Status>();
        public IEnumerable<Status> StatusList
        {
            get
            {
                _statusList.Sort();
                return _statusList;
            }
        }
        private List<string> _categoryList = new List<string>();
        public IEnumerable<string> CategoryList
        {
            get
            {
                _categoryList.Sort();
                return _categoryList;
            }
        }

        public List<TestSuite> TestSuiteList { get; set; }

        public string StartTime { get; set; }

        public string EndTime { get; set; }

        /// <summary>
        /// Error or other status messages
        /// </summary>
        public string StatusMessage { get; set; }

        public string TemplateName
        {
            get { return "Report"; }
        }

        /// <summary>
        /// File name generated that this data is for
        /// </summary>
        public string FileName { get; set; }

        public TestRunner TestRunner { get; set; }

        public Dictionary<string, string> RunInfo { get; set; }

        public void AddRunInfo(Dictionary<string, string> runInfo)
        {
            RunInfo = runInfo;
        }

        /// <summary>
        /// Name of the assembly that the tests are for
        /// </summary>
        public string AssemblyName { get; set; }

        /// <summary>
        /// Overall result of the test run (eg Passed, Failed)
        /// </summary>
        public Status Status { get; set; }

        /// <summary>
        /// How long the test suite took to run (in milliseconds)
        /// </summary>
        public double Duration { get; set; }

        /// <summary>
        /// Total number of tests run
        /// </summary>
        public double Total { get; set; }

        public double Passed { get; set; }

        public double Failed { get; set; }

        public double Inconclusive { get; set; }

        public double Skipped { get; set; }

        public double Errors { get; set; }

        public List<SideNavLink> SideNavLinks = new List<SideNavLink>();

        public Report()
        {
            TestSuiteList = new List<TestSuite>();
        }

        public void AddStatus(Status status)
        {
            if (!_statusList.Contains(status))
                _statusList.Add(status);
        }

        public void AddCategory(string category)
        {
            if (!_categoryList.Contains(category))
                _categoryList.Add(category);
        }

        public void AddCategories(IEnumerable<string> categories)
        {
            foreach(var category in categories)
            {
                AddCategory(category);
            }
        }
    }
}
