using System.Collections.Generic;
using System.Linq;
using CommandLine;
using CommandLine.Text;
using ReportUnit.Utils.CommandLineOptions.CommandLineOptionMode;

namespace ReportUnit.Utils.CommandLineOptions
{
    public class CommandLineOptions
    {
        [ValueList(typeof(List<string>), MaximumElements = 2)]
        public List<string> InputOutput { get; set; }

        [Option("engine", DefaultValue = "Html")]
        public string Engine { get; set; }

        [HelpOption]
        public string GetUsage(ICommandLineOptionMode[] availableModes)
        {
            var helpText = new HelpText
            {
                AddDashesToOption = true
            };
            var usageLines = availableModes.Select(mode => $"ReportUnit {mode.Usage()} <options>");
            helpText.AddPreOptionsLines(usageLines);
            helpText.AddOptions(this);
            return helpText;
        }

        public string GetInput()
        {
            return InputOutput.Count > 0 
                ? InputOutput[0] 
                : null;
        }

        public string GetOutput()
        {
            return InputOutput.Count > 1
                ? InputOutput[1]
                : null;
        }
    }
}
