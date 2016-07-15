using System.Collections.Generic;

namespace ReportUnit.Logging
{
    public interface ILogger
    {
        void Log(Level level, string message);
        Queue<Log> GetLogs();

        void Debug(string message);
        void Info(string message);
        void Warning(string message);
        void Error(string message);
        void Fatal(string message);
    }
}
