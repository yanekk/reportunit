using System.Collections.Generic;
using CommandLine;
using CommandLine.Text;

namespace ReportUnit.Utils.CommandLineOptions
{
    internal class CommandLineOptions
    {
        /// <summary>
        /// ReportUnit usage
        /// </summary>
        private const string Usage = "\n[INFO] Usage 1:  ReportUnit \"path-to-folder\" <options>" 
                                   + "\n[INFO] Usage 2:  ReportUnit \"input-folder\" \"output-folder\" <options>";

        [ValueList(typeof(List<string>), MaximumElements = 2)]
        public List<string> InputOutput { get; set; }

        [Option("engine", DefaultValue = "Html")]
        public string Engine { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            var originalAutoHelpText = HelpText.AutoBuild(this, current => HelpText.DefaultParsingErrorsHandler(this, current)).ToString();
            return originalAutoHelpText + Usage;
        }
    }
}
