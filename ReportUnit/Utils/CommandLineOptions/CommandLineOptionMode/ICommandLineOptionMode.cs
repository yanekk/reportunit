using System.IO;

namespace ReportUnit.Utils.CommandLineOptions.CommandLineOptionMode
{
    public interface ICommandLineOptionMode
    {
        bool AreCompatibile(CommandLineOptions options);
        FileInfo[] GetInputFiles(CommandLineOptions options);
        DirectoryInfo GetOutputDirectory(CommandLineOptions options);
        string Usage();
    }
}
