using System;
using ReportUnit.Logging;
using ReportUnit.Utils.CommandLineOptions;

namespace ReportUnit.Workers.CreateReport
{
    public class CreateReportWorker : ICreateReportWorker
    {
        private readonly IReportingService _reportingService;
        private readonly ICommandLineOptionsParserService _commandLineOptionsParserService;
        private readonly ILogger _logger;

        public CreateReportWorker(
            IReportingService reportingService, 
            ICommandLineOptionsParserService commandLineOptionsParserService,
            ILogger logger)
        {
            _reportingService = reportingService;
            _commandLineOptionsParserService = commandLineOptionsParserService;
            _logger = logger;
        }

        public void Execute(string[] args)
        {
            try
            {
                var executionParameters = _commandLineOptionsParserService.Parse(args);
                _reportingService.CreateReport(
                    executionParameters.GetEngineName(),
                    executionParameters.GetInputFiles(),
                    executionParameters.GetOutputDirectory());
            }
            catch (CommandLineOptionsParserService.Error error)
            {
                _logger.Error(error.Message);
            }
            catch (Exception e)
            {
                _logger.Error(e.Message);
                _logger.Error(e.StackTrace);
            }
        }
    }
}
