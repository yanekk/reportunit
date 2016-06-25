using System.IO;
using System.Reflection;

namespace ReportUnit.Utils
{
    public static class ResourceHelper
    {
        public static string GetStringResource(string name)
        {
            var resources = Assembly.GetExecutingAssembly().GetManifestResourceNames();
            using (var stream = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream(name)))
            {
                return stream.ReadToEnd();
            }
        }
    }
}
