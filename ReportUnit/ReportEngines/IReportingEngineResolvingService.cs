namespace ReportUnit.ReportEngines
{
    public interface IReportingEngineResolvingService
    {
        IReportingEngine ResolveByName(string engineName);
        string[] AvailableEngineNames { get; }
    }
}
