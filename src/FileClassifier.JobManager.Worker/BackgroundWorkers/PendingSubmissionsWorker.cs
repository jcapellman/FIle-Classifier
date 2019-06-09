using System;
using System.ComponentModel;
using System.Linq;

using FileClassifier.JobManager.lib.Databases;
using FileClassifier.JobManager.lib.Databases.Tables;
using FileClassifier.JobManager.lib.Handlers;
using FileClassifier.JobManager.Worker.Common;

using Newtonsoft.Json;

namespace FileClassifier.JobManager.Worker.BackgroundWorkers
{
    public class PendingSubmissionsWorker
    {
        private BackgroundWorker _bwCheckin;

        private LiteDBDatabase _db;

        private string _serverURL;

        public PendingSubmissionsWorker()
        {
            _db = new LiteDBDatabase();

            _bwCheckin = new BackgroundWorker();
            _bwCheckin.DoWork += BwCheckin_DoWork;
            _bwCheckin.RunWorkerCompleted += BwCheckin_RunWorkerCompleted;
        }

        public void Run(string serverURL)
        {
            _serverURL = serverURL;

            _bwCheckin.RunWorkerAsync();
        }

        private void BwCheckin_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            System.Threading.Thread.Sleep(Constants.LOOP_INTERVAL_MS);

            _bwCheckin.RunWorkerAsync();
        }

        private async void BwCheckin_DoWork(object sender, DoWorkEventArgs e)
        {
            var pendingJobs = _db.GetPendingSubmissions();

            if (!pendingJobs.Any())
            {
                return;
            }

            var workerHandler = new WorkerHandler(_serverURL);


            foreach (var pJob in pendingJobs)
            {
                Jobs job = null;

                try
                {
                    job = JsonConvert.DeserializeObject<Jobs>(pJob.JobJSON);
                } catch (Exception ex)
                {
                    // LOG HERE
                }

                if (job == null)
                {
                    // LOG HERE

                    _db.RemoveOfflineSubmission(pJob.ID);

                    continue;
                }

                var result = await workerHandler.UpdateWorkAsync(job);

                if (result)
                {
                    _db.RemoveOfflineSubmission(pJob.ID);

                    continue;
                }

                // LOG HERE
            }            
        }
    }
}