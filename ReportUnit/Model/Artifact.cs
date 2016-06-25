namespace ReportUnit.Model
{
    public class Artifact
    {
        public string FileName { get; private set; }
        public string FilePath { get; set; }

        public Artifact(string fileName)
        {
            FileName = fileName;
        }
    }
}
