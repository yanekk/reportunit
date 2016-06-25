using System.Collections.Generic;

namespace ReportUnit.Model
{
    public class TestSuite
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public Status Status;
        public string StatusMessage { get; set; }
        public string StackTrace { get; set; }

        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public double Duration { get; set; }

        public readonly List<Test> TestList = new List<Test>();
        public readonly List<string> Categories = new List<string>();
    }
}
