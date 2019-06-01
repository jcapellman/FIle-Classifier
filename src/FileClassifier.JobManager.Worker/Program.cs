using System.Linq;

namespace FileClassifier.JobManager.Worker
{
    class Program
    {
        static void Main(string[] args)
        {
            var serverUrl = "http://localhost:5000/api/";

            if (args.Any())
            {
                serverUrl = args[0];
            }

            new Worker(serverUrl).RunAsync();
        }
    }
}