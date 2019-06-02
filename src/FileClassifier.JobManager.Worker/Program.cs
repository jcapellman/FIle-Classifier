using System.Linq;

using FileClassifier.JobManager.Worker.Common;

namespace FileClassifier.JobManager.Worker
{
    class Program
    {
        static void Main(string[] args)
        {
            var serverUrl = Constants.DEFAULT_SERVER_URL;

            if (args.Any())
            {
                serverUrl = args[0];
            }

            new Worker(serverUrl).RunAsync();
        }
    }
}