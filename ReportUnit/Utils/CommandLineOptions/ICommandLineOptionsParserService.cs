namespace ReportUnit.Utils.CommandLineOptions
{
    public interface ICommandLineOptionsParserService
    {
        IExecutionParameters Parse(string[] commandLineArguments);
    }
}
