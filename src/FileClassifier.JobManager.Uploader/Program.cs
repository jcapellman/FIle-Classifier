using System;
using System.Threading.Tasks;

using FileClassifier.JobManager.lib.Databases.Tables;
using FileClassifier.JobManager.lib.Handlers;
using FileClassifier.lib.Enums;

namespace FileClassifier.JobManager.Uploader
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Eventually force the use of a json file
            if (args.Length < 4)
            {
                Console.WriteLine($"Usage is:{Environment.NewLine}FileClassifier.JobManager.Uploader <name> <model type> <path to data> <server url>");

                return;
            }

            if (!Enum.TryParse<ModelType>(args[1], true, out _))
            {
                Console.WriteLine($"Invalid Model Type option ({args[1]})");

                return;
            }

            var jobHandler = new JobHandler(args[3]);

            var result = await jobHandler.AddNewJobAsync(new Jobs
            {
                ModelType = args[1],
                TrainingDataPath = args[2],
                Name = args[0]
            });

            Console.WriteLine(result ? "Job successfully uploaded" : "Failed to upload job");
        }
    }
}