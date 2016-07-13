using System;
using System.IO;
using System.Linq;

namespace ReportUnit.Utils
{
    public class CommandLineOptions
    {
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

        public FileInfo InputFile { get; private set; }

        public DirectoryInfo OutputDirectory { get; private set; }
        public DirectoryInfo InputDirectory { get; private set; }

        public CommandLineOptions(string[] args)
        {
            if (args.Length == 0 || args.Length > 2)
                throw new Error("Invalid number of arguments specified.\n" + USAGE);

            if (args.Any(arg => arg.Trim() == "" || arg == @"\\"))
                throw new Error("Invalid argument(s) specified.\n" + USAGE);

            var testFileExtensions = new [] { ".xml", ".trx"};
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
                    OutputDirectory = Directory.GetParent(Path.GetFullPath(input));
                    InputFile = new FileInfo(Path.GetFullPath(input));
                    return;
                }

                if (!Directory.Exists(input))
                    throw new Error("Input directory " + input + " not found.\n" + USAGE);

                if (!Directory.Exists(output))
                    throw new Error("Output directory " + input + " not found.\n" + USAGE);

                InputDirectory = new DirectoryInfo(input);
                OutputDirectory = new DirectoryInfo(output);
            }
            else
            {
                var inputFile = args[0];
                if (File.Exists(inputFile) && testFileExtensions.Contains(Path.GetExtension(inputFile)))
                {
                    InputFile = new FileInfo(inputFile);
                    OutputDirectory = Directory.GetParent(inputFile);
                    return;
                }

                var inputDirectory = args[0];
                if (!Directory.Exists(inputDirectory))
                {
                    throw new Error("The path of file or directory you have specified does not exist.\n" + USAGE);
                }
                InputDirectory = new DirectoryInfo(inputDirectory);
                OutputDirectory = new DirectoryInfo(inputDirectory);
            }
        }
    }
}
