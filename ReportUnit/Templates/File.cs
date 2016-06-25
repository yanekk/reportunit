using System.IO;
using System.Reflection;

namespace ReportUnit.Templates
{
    internal class File
    {
        public static string GetSource()
        {
            var resources = Assembly.GetExecutingAssembly().GetManifestResourceNames();
            using (var stream = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("ReportUnit.Templates.File.cshtml")))
            {
                return stream.ReadToEnd();
            }
        }
    }
}
