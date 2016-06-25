using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace ReportUnit.Parser.NUnitParsers
{
    internal static class NUnitParsingHelper
    {
        /// <summary>
        /// Returns categories for the direct children or all descendents of an XElement
        /// </summary>
        /// <param name="elem">XElement to parse</param>
        /// <param name="allDescendents">If true, return all descendent categories.  If false, only direct children</param>
        /// <returns></returns>
        public static List<string> GetCategories(XElement elem, bool allDescendents)
        {
            //Define which function to use
            var parser = allDescendents
                ? new Func<XElement, string, IEnumerable<XElement>>((e, s) => e.Descendants(s))
                : new Func<XElement, string, IEnumerable<XElement>>((e, s) => e.Elements(s));

            //Grab unique categories
            var categories = new List<string>();
            var hasCategories = parser(elem, "categories").Any();
            if (!hasCategories)
                return categories;

            var cats = parser(elem, "categories").Elements("category").ToList();

            cats.ForEach(x => categories.Add(x.Attribute("name").Value));

            return categories;
        }
    }
}
