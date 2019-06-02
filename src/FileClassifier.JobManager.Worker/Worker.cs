using System;
using System.Linq;
using System.Reflection;

using FileClassifier.JobManager.lib.Databases.Tables;
using FileClassifier.JobManager.lib.Handlers;

namespace FileClassifier.JobManager.Worker
{
    public class Worker
    {
        private const int LOOP_INTERVAL_MS = 60000;

        private const int LOOP_ERROR_INTERVAL_MS = LOOP_INTERVAL_MS * 5;

        private readonly string _serverURL;

        public Worker(string serverURL)
        {
            _serverURL = serverURL;
        }

        public async void RunAsync()
        {
            var hostHandler = new HostsHandler(_serverURL);

            var host = new Hosts
            {
                Name = Environment.MachineName,
                NumCores = Environment.ProcessorCount,
                WorkerVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString(),
                OSVersion = Environment.OSVersion.VersionString
            };

            var workerHandler = new WorkerHandler(_serverURL);

            while (true)
            {
                // Call to checkin with the server
                var checkinResult = await hostHandler.AddUpdateHostAsync(host);

                if (!checkinResult)
                {
                    Console.WriteLine($"Failed to check in with {_serverURL}");

                    System.Threading.Thread.Sleep(LOOP_ERROR_INTERVAL_MS);

                    continue;
                }

                // Check for Work
                var work = await workerHandler.GetWorkAsync(host.Name);

                if (work.Any())
                {
                    System.Threading.Thread.Sleep(LOOP_INTERVAL_MS);

                    continue;
                }

                // Perform Work if available
                foreach (var job in work)
                {
                    // TODO: Perform Job
                    // TODO: Update Job
                    // TODO: Upload Model
                }

                System.Threading.Thread.Sleep(LOOP_INTERVAL_MS);
            }
        }
    }
}