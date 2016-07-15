using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ReportUnit.Parsers;

namespace ReportUnit.Utils.CommandLineOptions
{
    public class CommandLineOptionsParserService : ICommandLineOptionsParserService
    {
        private readonly ITestFileParserResolver[] _parserResolvers;

        /// <summary>
        /// ReportUnit usage
        /// </summary>
        private static string USAGE = "[INFO] Usage 1:  ReportUnit \"path-to-folder\"" +
                                                "\n[INFO] Usage 2:  ReportUnit \"input-folder\" \"output-folder\"" +
                                                "\n[INFO] Usage 3:  ReportUnit \"input.xml\" \"output.html\"";

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

        public IExecutionParameters Parse(string[] args)
        {
            if (args.Length == 0 || args.Length > 2)
                throw new Error("Invalid number of arguments specified.\n" + USAGE);

            if (args.Any(arg => arg.Trim() == "" || arg == @"\\"))
                throw new Error("Invalid argument(s) specified.\n" + USAGE);

            var testFileExtensions = GetAllowedTestFileExtensions();
            var htmlReportExtensions = new [] {".htm", ".html"};
            
            if (args.Length == 2)
            {
                var input = args[0];
                var output = args[1];

                var inputFileExtension = Path.GetExtension(input).ToLower();
                var outputFileExtension = Path.GetExtension(output).ToLower();

                if (testFileExtensions.Contains(inputFileExtension) &&
                    htmlReportExtensions.Contains(outputFileExtension))
                {
                    return GetResult(
                        new FileInfo(Path.GetFullPath(input)),
                        Directory.GetParent(Path.GetFullPath(input)));
                }

                if (!Directory.Exists(input))
                    throw new Error("Input directory " + input + " not found.\n" + USAGE);

                return GetResult(new DirectoryInfo(input), new DirectoryInfo(output));
            }

            var inputFile = args[0];
            if (File.Exists(inputFile) && testFileExtensions.Contains(Path.GetExtension(inputFile)))
            {
                return GetResult(new FileInfo(inputFile), Directory.GetParent(inputFile));
            }

            var inputDirectory = args[0];
            if (!Directory.Exists(inputDirectory))
            {
                throw new Error("The path of file or directory you have specified does not exist.\n" + USAGE);
            }
            return GetResult(new DirectoryInfo(inputDirectory), new DirectoryInfo(inputDirectory));
        }

        private string[] GetAllowedTestFileExtensions()
        {
            return _parserResolvers.Select(r => $".{r.AllowedFileExtension}").Distinct().ToArray();
        }

        private IExecutionParameters GetResult(FileInfo inputFile, DirectoryInfo outputDirectory)
        {
            return new ExecutionParameters(new[] {inputFile}, outputDirectory);
        }

        private IExecutionParameters GetResult(DirectoryInfo inputDirectory, DirectoryInfo outputDirectory)
        {
            var inputFiles = inputDirectory
                .GetFiles("*.xml", SearchOption.AllDirectories)
                .OrderByDescending(f => f.CreationTime)
                .ToArray();
            return GetResult(inputFiles, outputDirectory);
        }

        private IExecutionParameters GetResult(FileInfo[] inputFiles, DirectoryInfo outputDirectory)
        {
            return new ExecutionParameters(inputFiles, outputDirectory);
        }
    }
}
