using System;
using System.Collections.Generic;

namespace ReportUnit.Logging
{
    public class Logger : ILogger
    {
        private readonly Queue<Log> _queue = new Queue<Log>();

        public void Log(Level level, string message)
        {
            var log = new Log
            {
                Timestamp = DateTime.Now,
                Level = level,
                Message = message
            };

            Console.WriteLine(log.ToString());
            _queue.Enqueue(log);
        }

        public Queue<Log> GetLogs()
        {
            return _queue;
        }

        public void Debug(string message)
        {
            Log(Level.Debug, message);
        }

        public void Info(string message)
        {
            Log(Level.Info, message);
        }

        public void Warning(string message)
        {
            Log(Level.Warning, message);
        }

        public void Error(string message)
        {
            Log(Level.Error, message);
        }

        public void Fatal(string message)
        {
            Log(Level.Fatal, message);
        }
    }
}
