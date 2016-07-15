namespace ReportUnit.Workers.CreateReport
{
    public interface ICreateReportWorker
    {
        void Execute(string[] commandLineArguments);
    }
}
