using System.CommandLine;
using System.CommandLine.Invocation;

using FileClassifier.lib.Common;

namespace FileClassifier.Trainer
{
    public class TrainerCommandLineParser
    {
        public static TrainerCommandLineOptions Parse(string[] args)
        {
            TrainerCommandLineOptions options = null;

            Option oVerbose = new Option(
                "--verbose",
                "Enable verbose output",
                new Argument<bool>(defaultValue: false));

            Option oFolder = new Option(
                "--folderofdata",
                "Folder containing data to be parsed to build the model",
                new Argument<string>());

            var rootCommand = new RootCommand
            {
                Description = "File Trainer builds a model"
            };

            rootCommand.AddOption(oFolder);
            rootCommand.AddOption(oVerbose);
            rootCommand.TreatUnmatchedTokensAsErrors = true;

            rootCommand.Argument.AddValidator(symbolResult =>
            {
                if (symbolResult.Children["--folderofdata"] is null)
                {
                    return "Folder Path is required";
                }
                else
                {
                    return null;
                }
            });

            rootCommand.Handler = CommandHandler.Create<string, bool>((folderPath, verbose) =>
            {
                if (string.IsNullOrEmpty(folderPath))
                {
                    return;
                }

                options = new TrainerCommandLineOptions
                {
                    FolderOfData = folderPath,
                    Verbose = verbose
                };
            });

            rootCommand.InvokeAsync(args).Wait();

            return options;
        }
    }
}