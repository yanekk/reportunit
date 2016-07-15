using ReportUnit.DependencyInjection;
using ReportUnit.Utils.CommandLineOptions;
using ReportUnit.Workers.CreateReport;

namespace ReportUnit
{
    using System;
    using Logging;

    class Program
    {
        private static readonly Logger Logger = Logger.GetLogger();

        /// <summary>
        /// Entry point
        /// </summary>
        /// <param name="args">Accepts 3 types of input arguments
        ///     Type 1: reportunit "path-to-folder"
        ///         args.length = 1 && args[0] is a directory
        ///     Type 2: reportunit "path-to-folder" "output-folder"
        ///         args.length = 2 && both args are directories
        ///     Type 3: reportunit "input.xml" "output.html"
        ///         args.length = 2 && args[0] is xml-input && args[1] is html-output
        /// </param>
        static void Main(string[] args)
        {
            try
            {
                DI.Initialize();
                var worker = DI.Resolve<ICreateReportWorker>();
                worker.Execute(args);
            }
            catch (CommandLineOptionsParserService.Error error)
            {
                Logger.Error(error.Message);
            }
            catch (Exception e)
            {
                Logger.Error(e.Message);
                Logger.Error(e.StackTrace);
            }
        }
    }
}
