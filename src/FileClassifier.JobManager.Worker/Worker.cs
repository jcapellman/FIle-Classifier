using System;
using System.ComponentModel;
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

        private BackgroundWorker _bwCheckin;

        private Hosts _host;

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

            _bwCheckin = new BackgroundWorker();
            _bwCheckin.DoWork += BwCheckin_DoWork;
            _bwCheckin.RunWorkerCompleted += BwCheckin_RunWorkerCompleted;
            _bwCheckin.RunWorkerAsync();
        }

        private void BwCheckin_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            System.Threading.Thread.Sleep(LOOP_INTERVAL_MS);

            _bwCheckin.RunWorkerAsync();
        }

        private async void BwCheckin_DoWork(object sender, DoWorkEventArgs e)
        {
            var hostHandler = new HostsHandler(_serverURL);

            // Call to checkin with the server
            var checkinResult = await hostHandler.AddUpdateHostAsync(_host);

            if (!checkinResult)
            {
                Console.WriteLine($"Failed to check in with {_serverURL}");

                System.Threading.Thread.Sleep(LOOP_ERROR_INTERVAL_MS);
            }
        }

        public async void RunAsync()
        {
            var workerHandler = new WorkerHandler(_serverURL);

            while (true)
            {
                // Check for Work
                var work = await workerHandler.GetWorkAsync(_host.Name);

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