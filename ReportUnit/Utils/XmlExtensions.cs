using System.Xml.Linq;

namespace ReportUnit.Utils
{
    public static class XmlExtensions
    {
        public static string GetChildElementValueOrDefault(this XElement parentElement, string elementName)
        {
            return parentElement.Element(elementName) != null
                ? parentElement.Element(elementName).Value.Trim()
                : null;
        }

        public static string GetAttributeValueOrDefault(this XElement element, string attributeName)
        {
            return element.Attribute(attributeName) != null
                ? element.Attribute(attributeName).Value
                : null;
        }
    }
}
