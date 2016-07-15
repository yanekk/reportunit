using ReportUnit.DependencyInjection;
using ReportUnit.Workers.CreateReport;

namespace ReportUnit
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            DI.Initialize();
            var worker = DI.Resolve<ICreateReportWorker>();
            worker.Execute(args);
        }
    }
}
