using System.IO;
using System.Xml;

namespace ReportUnit.Parsers
{
    public abstract class ParserResolverFor<TTestFileParser>
        where TTestFileParser : ITestFileParser, new()
    {
        public abstract string AllowedFileExtension { get; }

        public bool IsCompatibile(string filePath)
        {
            var extension = Path.GetExtension(filePath).Substring(1);
            var document = new XmlDocument();
            document.Load(filePath);
            return IsExtensionMatching(extension) && IsHeaderCompatibile(document);
        }

        public ITestFileParser GetParser()
        {
            return new TTestFileParser();
        }

        protected bool IsExtensionMatching(string extension)
        {
            return extension == AllowedFileExtension;
        }

        protected abstract bool IsHeaderCompatibile(XmlDocument document);
    }
}
