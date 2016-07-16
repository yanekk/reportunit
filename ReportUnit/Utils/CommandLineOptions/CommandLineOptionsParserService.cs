using System;
using System.Linq;
using CommandLine;
using ReportUnit.Utils.CommandLineOptions.CommandLineOptionMode;

namespace ReportUnit.Utils.CommandLineOptions
{
    public class CommandLineOptionsParserService : ICommandLineOptionsParserService
    {
        private readonly ICommandLineOptionMode[] _modes;

        public class Error : Exception
        {
            public Error(string error) : base(error)
            {
                
            }
        }

        public CommandLineOptionsParserService(ICommandLineOptionMode[] modes)
        {
            _modes = modes;
        }

        public IExecutionParameters Parse(string[] commandLineArguments)
        {
            var commandLineOptions = new CommandLineOptions();

            if (!Parser.Default.ParseArguments(commandLineArguments, commandLineOptions))
                throw new Error(commandLineOptions.GetUsage(_modes));
            
            if(commandLineOptions.GetInput() == null)
                throw new Error("Invalid number of arguments\n" + commandLineOptions.GetUsage(_modes));

            var mode = _modes.FirstOrDefault(m => m.AreCompatibile(commandLineOptions));
            if (mode == null)
                throw new Error("The path of file or directory you have specified does not exist.\n" + commandLineOptions.GetUsage(_modes));

            var inputFiles = mode.GetInputFiles(commandLineOptions);
            var outputDirectory = mode.GetOutputDirectory(commandLineOptions);
            var executionParameters = new ExecutionParameters
            {
                OutputDirectory = outputDirectory,
                InputFiles = inputFiles,
                EngineName = commandLineOptions.Engine
            };
            return executionParameters;
        }
    }
}
