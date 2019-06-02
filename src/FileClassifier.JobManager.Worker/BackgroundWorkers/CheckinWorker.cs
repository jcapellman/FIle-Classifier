using System;
using System.ComponentModel;

using FileClassifier.JobManager.lib.Databases.Tables;
using FileClassifier.JobManager.lib.Handlers;
using FileClassifier.JobManager.Worker.Common;

namespace FileClassifier.JobManager.Worker.BackgroundWorkers
{
    public class CheckinWorker
    {
        private BackgroundWorker _bwCheckin;

        private Hosts _host;

        private string _serverURL;

        public CheckinWorker()
        {
            _bwCheckin = new BackgroundWorker();
            _bwCheckin.DoWork += BwCheckin_DoWork;
            _bwCheckin.RunWorkerCompleted += BwCheckin_RunWorkerCompleted;            
        }

        public void Run(Hosts host, string serverURL)
        {
            _host = host;
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
            var hostHandler = new HostsHandler(_serverURL);

            // Call to checkin with the server
            var checkinResult = await hostHandler.AddUpdateHostAsync(_host);

            if (!checkinResult)
            {
                Console.WriteLine($"Failed to check in with {_serverURL}");

                System.Threading.Thread.Sleep(Constants.LOOP_ERROR_INTERVAL_MS);
            }
        }
    }
}