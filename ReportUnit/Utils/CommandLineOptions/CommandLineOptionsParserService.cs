using System;
using System.IO;
using System.Linq;
using CommandLine;
using ReportUnit.Parsers;

namespace ReportUnit.Utils.CommandLineOptions
{
    public class CommandLineOptionsParserService : ICommandLineOptionsParserService
    {
        private readonly ITestFileParserResolver[] _parserResolvers;



        public class Error : Exception
        {
            public Error(string error) : base(error)
            {
                
            }
        }

        public CommandLineOptionsParserService(ITestFileParserResolver[] parserResolvers)
        {
            _parserResolvers = parserResolvers;
        }

        public IExecutionParameters Parse(string[] commandLineArguments)
        {
            var commandLineOptions = new CommandLineOptions();
            if (!Parser.Default.ParseArguments(commandLineArguments, commandLineOptions))
            {
                throw new Error(commandLineOptions.GetUsage());
            }

            var testFileExtensions = GetAllowedTestFileExtensions();
            var htmlReportExtensions = new [] {".htm", ".html"};
            
            if (commandLineOptions.InputOutput.Count == 2)
            {
                var input = commandLineOptions.InputOutput[0];
                var output = commandLineOptions.InputOutput[1];

                var inputFileExtension = Path.GetExtension(input).ToLower();
                var outputFileExtension = Path.GetExtension(output).ToLower();

                if (testFileExtensions.Contains(inputFileExtension) &&
                    htmlReportExtensions.Contains(outputFileExtension))
                {
                    return GetResult(commandLineOptions,
                        new FileInfo(Path.GetFullPath(input)),
                        Directory.GetParent(Path.GetFullPath(input)));
                }

                if (!Directory.Exists(input))
                    throw new Error("Input directory " + input + " not found.\n" + commandLineOptions.GetUsage());

                return GetResult(
                    commandLineOptions, 
                    new DirectoryInfo(input), 
                    new DirectoryInfo(output));
            }

            var inputFile = commandLineOptions.InputOutput[0];
            if (File.Exists(inputFile) && testFileExtensions.Contains(Path.GetExtension(inputFile)))
            {
                return GetResult(commandLineOptions, new FileInfo(inputFile), Directory.GetParent(inputFile));
            }

            var inputDirectory = commandLineOptions.InputOutput[0];
            if (!Directory.Exists(inputDirectory))
            {
                throw new Error("The path of file or directory you have specified does not exist.\n" + commandLineOptions.GetUsage());
            }
            return GetResult(commandLineOptions, new DirectoryInfo(inputDirectory), new DirectoryInfo(inputDirectory));
        }

        private string[] GetAllowedTestFileExtensions()
        {
            return _parserResolvers.Select(r => $".{r.AllowedFileExtension}").Distinct().ToArray();
        }

        private IExecutionParameters GetResult(CommandLineOptions options, FileInfo inputFile, DirectoryInfo outputDirectory)
        {
            return new ExecutionParameters(new[] {inputFile}, outputDirectory, options.Engine);
        }

        private IExecutionParameters GetResult(CommandLineOptions options, DirectoryInfo inputDirectory, DirectoryInfo outputDirectory)
        {
            var inputFiles = inputDirectory
                .GetFiles("*.xml", SearchOption.AllDirectories)
                .OrderByDescending(f => f.CreationTime)
                .ToArray();
            return GetResult(options, inputFiles, outputDirectory);
        }

        private IExecutionParameters GetResult(CommandLineOptions options, FileInfo[] inputFiles, DirectoryInfo outputDirectory)
        {
            return new ExecutionParameters(inputFiles, outputDirectory, options.Engine);
        }
    }
}
