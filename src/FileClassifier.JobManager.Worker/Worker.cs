using System;
using System.Reflection;

using FileClassifier.JobManager.lib.Databases.Tables;
using FileClassifier.JobManager.Worker.BackgroundWorkers;
using FileClassifier.JobManager.Worker.Common;

namespace FileClassifier.JobManager.Worker
{
    public class Worker
    {
        private readonly string _serverURL;

        private readonly Hosts _host;

        private readonly CheckinWorker _cWorker = new CheckinWorker();
        private readonly JobWorker _jWorker = new JobWorker();

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
            
            while (true)
            {
                var workerResult = await _jWorker.Run(_host, _serverURL);

                if (!workerResult)
                {
                    System.Threading.Thread.Sleep(Constants.LOOP_ERROR_INTERVAL_MS);
                }
                else
                {
                    System.Threading.Thread.Sleep(Constants.LOOP_INTERVAL_MS);
                }
            }
        }
    }
}