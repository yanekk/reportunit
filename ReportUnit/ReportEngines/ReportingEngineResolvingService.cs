using System.Linq;

namespace ReportUnit.ReportEngines
{
    public class ReportingEngineResolvingService : IReportingEngineResolvingService
    {
        private readonly IReportingEngine[] _reportingEngines;

        public ReportingEngineResolvingService(IReportingEngine[] reportingEngines)
        {
            _reportingEngines = reportingEngines;
        }

        public IReportingEngine ResolveByName(string engineName)
        {
            return _reportingEngines.SingleOrDefault(engine => engine.Name == engineName);
        }

        public string[] AvailableEngineNames
        {
            get { return _reportingEngines.Select(r => r.Name).ToArray(); }
        }
    }
}
