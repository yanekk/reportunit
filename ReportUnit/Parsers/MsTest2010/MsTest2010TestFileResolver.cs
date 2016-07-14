using System.Xml;
using ReportUnit.Parser;

namespace ReportUnit.Parsers.MsTest2010
{
    public class MsTest2010TestFileResolver : ParserResolverFor<MsTest2010TestFileParser>, ITestFileResolver
    {
        protected override string AllowedFileExtension => "trx";

        protected override bool IsHeaderCompatibile(XmlDocument document)
        {
            // MSTest2010
            var nsmgr = new XmlNamespaceManager(document.NameTable);
            nsmgr.AddNamespace("ns", "http://microsoft.com/schemas/VisualStudio/TeamTest/2010");

            // check if its a mstest 2010 xml file 
            // will need to check the "//TestRun/@xmlns" attribute - value = http://microsoft.com/schemas/VisualStudio/TeamTest/2010
            XmlNode testRunNode = document.SelectSingleNode("ns:TestRun", nsmgr);
            return testRunNode != null 
                && testRunNode.Attributes != null 
                && testRunNode.Attributes["xmlns"] != null 
                && testRunNode.Attributes["xmlns"].InnerText.Contains("2010");
        }
    }
}
