using System.IO;
using System.Linq;
using System.Reflection;

namespace ReportUnit.Utils
{
    public static class ResourceHelper
    {
        public static string[] GetResourceNames(string namespacePrefix)
        {
            return Assembly
                .GetExecutingAssembly()
                .GetManifestResourceNames()
                .Where(name => name.StartsWith(namespacePrefix))
                .ToArray();
        }

        public static string GetStringResource(string name)
        {
            using (var stream = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream(name)))
            {
                return stream.ReadToEnd();
            }
        }
    }
}
