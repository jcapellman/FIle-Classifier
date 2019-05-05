using System.CommandLine;
using System.CommandLine.Invocation;

using FileClassifier.lib.Enums;
using FileClassifier.lib.Options;

namespace FileClassifier.app
{
    public class CommandLineParser
    {
        public static ClassifierCommandLineOptions Parse(string[] args)
        {
            ClassifierCommandLineOptions options = null;

            var oVerbose = new Option(
                "--verbose",
                "Enable verbose output",
                new Argument<bool>(defaultValue: false));
            
            var oFile = new Option(
                "--file",
                "File to be scanned (Required)",
                new Argument<string>());

            var rootCommand = new RootCommand
            {
                Description = "File Classifier applies ML to all files to determine if it is benign or malicious"
            };

            rootCommand.AddOption(oFile);
            rootCommand.AddOption(oVerbose);
            rootCommand.TreatUnmatchedTokensAsErrors = true;

            rootCommand.Argument.AddValidator(symbolResult =>
            {
                if (symbolResult.Children["--file"] is null)
                {
                    return "Filename is required";
                }
                else
                {
                    return null;
                }
            });

            rootCommand.Handler = CommandHandler.Create<string, bool>((fileName, verbose) =>
            {
                if (string.IsNullOrEmpty(fileName))
                {
                    return;
                }

                options = new ClassifierCommandLineOptions
                {
                    FileName = fileName,
                    LogLevel = LogLevels.DEBUG
                };
            });

            rootCommand.InvokeAsync(args).Wait();
            
            return options;
        }
    }
}