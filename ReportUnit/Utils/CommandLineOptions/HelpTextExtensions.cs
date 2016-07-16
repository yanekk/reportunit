using System.Collections.Generic;
using CommandLine.Text;

namespace ReportUnit.Utils.CommandLineOptions
{
    internal static class HelpTextExtensions
    {
        public static void AddPreOptionsLines(this HelpText helpText, IEnumerable<string> preOptionsLines)
        {
            foreach (var preOptionsLine in preOptionsLines)
            {
                helpText.AddPreOptionsLine(preOptionsLine);
            }
        }
    }
}
