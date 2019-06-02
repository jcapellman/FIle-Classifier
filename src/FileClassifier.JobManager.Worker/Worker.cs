using System;
using System.Linq;
using System.Reflection;

using FileClassifier.JobManager.lib.Databases.Tables;
using FileClassifier.JobManager.lib.Handlers;
using FileClassifier.JobManager.Worker.BackgroundWorkers;
using FileClassifier.JobManager.Worker.Common;

namespace FileClassifier.JobManager.Worker
{
    public class Worker
    {
        private readonly string _serverURL;

        private Hosts _host;

        private CheckinWorker _cWorker = new CheckinWorker();

        public Worker(string serverURL)
        {
            _serverURL = serverURL;

            _host = new Hosts
            {
                Name = Environment.MachineName,
                NumCores = Environment.ProcessorCount,
                WorkerVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString(),
                OSVersion = Environment.OSVersion.VersionString
            };
        }

        public async void RunAsync()
        {
            _cWorker.Run(_host, _serverURL);

            var workerHandler = new WorkerHandler(_serverURL);

            while (true)
            {
                // Check for Work
                var work = await workerHandler.GetWorkAsync(_host.Name);

                if (work.Any())
                {
                    System.Threading.Thread.Sleep(Constants.LOOP_INTERVAL_MS);

                    continue;
                }

                // Perform Work if available
                foreach (var job in work)
                {
                    // TODO: Perform Job
                    // TODO: Update Job
                    // TODO: Upload Model
                }

                System.Threading.Thread.Sleep(Constants.LOOP_INTERVAL_MS);
            }
        }
    }
}