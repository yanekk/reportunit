using System.Xml.Linq;

namespace ReportUnit.Utils
{
    public static class XmlExtensions
    {
        public static string GetChildElementValueOrDefault(this XElement parentElement, string elementName)
        {
            var childElement = parentElement.Element(elementName);
            return childElement != null ? childElement.Value.Trim() : null;
        }

        public static string GetAttributeValueOrDefault(this XElement element, string attributeName)
        {
            var attribute = element.Attribute(attributeName);
            return attribute != null ? attribute.Value : null;
        }
    }
}
