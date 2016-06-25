using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportUnit.Model
{
    public class Test
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public Status Status;
        public string StatusMessage { get; set; }

        public string StackTrace { get; set; }

        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public double Duration { get; set; }

        /// <summary>
        /// Categories & features associated with the test
        /// </summary>
        public readonly List<string> CategoryList = new List<string>();

        public string GetCategories()
        {
            return CategoryList.Any() ? "" : string.Join(" ", CategoryList);
        }
    }
}
