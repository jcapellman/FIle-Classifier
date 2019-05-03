using System.CommandLine;
using System.CommandLine.Invocation;

using FileClassifier.lib.Common;

namespace FileClassifier.Trainer
{
    public class CommandLineParser
    {
        public static Options Parse(string[] args)
        {
            Options options = null;

            Option oVerbose = new Option(
                "--verbose",
                "Enable verbose output",
                new Argument<bool>(defaultValue: false));

            Option oFile = new Option(
                "--trainingfile",
                "File to be parsed to build the model",
                new Argument<string>());

            var rootCommand = new RootCommand
            {
                Description = "File Classifier Trainer builds a model"
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

                options = new Options
                {
                    FileName = fileName,
                    Verbose = verbose
                };
            });

            rootCommand.InvokeAsync(args).Wait();

            return options;
        }
    }
}