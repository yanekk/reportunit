using System.Xml;
using ReportUnit.Parser;

namespace ReportUnit.Parsers.Gallio
{
    public class GallioTestFileResolver : ParserResolverFor<GallioTestFileParser>, ITestFileResolver
    {
        protected override string AllowedFileExtension => "xml";

        protected override bool IsHeaderCompatibile(XmlDocument document)
        {
            var nsmgr = new XmlNamespaceManager(document.NameTable);
            nsmgr.AddNamespace("ns", "http://www.gallio.org/");

            var model = document.SelectSingleNode("//ns:testModel", nsmgr);
            return model != null;
        }
    }
}
