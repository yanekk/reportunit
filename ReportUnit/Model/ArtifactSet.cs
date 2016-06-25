using System.Collections.Generic;
using System.IO;

namespace ReportUnit.Model
{
    public class ArtifactSet
    {
        public readonly List<Artifact> Artifacts = new List<Artifact>();
        public readonly string BasePath;

        public ArtifactSet(string basePath)
        {
            BasePath = basePath;
        }

        public string DirectoryName
        {
            get { return Path.GetFileName(BasePath); }
        }
    }
}
