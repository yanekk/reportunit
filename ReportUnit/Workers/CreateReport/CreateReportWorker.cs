using ReportUnit.Reporting;
using ReportUnit.Utils;
using ReportUnit.Utils.CommandLineOptions;

namespace ReportUnit.Workers.CreateReport
{
    public class CreateReportWorker : ICreateReportWorker
    {
        private readonly IReportingService _reportingService;
        private readonly ICommandLineOptionsParserService _commandLineOptionsParserService;

        public CreateReportWorker(IReportingService reportingService, ICommandLineOptionsParserService commandLineOptionsParserService)
        {
            _reportingService = reportingService;
            _commandLineOptionsParserService = commandLineOptionsParserService;
        }

        public void Execute(string[] args)
        {
            var executionParameters = _commandLineOptionsParserService.Parse(args);
            _reportingService.CreateReport(
                executionParameters.GetInputFiles(), 
                executionParameters.GetOutputDirectory());
        }
    }
}
