using System;
using System.IO;
using System.Reflection;

using FileClassifier.JobManager.lib.Databases.Tables;
using FileClassifier.JobManager.lib.Handlers;
using FileClassifier.JobManager.Worker.BackgroundWorkers;
using FileClassifier.JobManager.Worker.Common;

using FileClassifier.lib.Enums;
using FileClassifier.lib.ML.Classification;
using FileClassifier.lib.ML.Clustering;
using FileClassifier.lib.Options;

namespace FileClassifier.JobManager.Worker
{
    public class Worker
    {
        private readonly string _serverURL;

        private readonly Hosts _host;

        private readonly CheckinWorker _cWorker = new CheckinWorker();

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
                var work = await workerHandler.GetWorkAsync(_host.Name);

                if (work == null)
                {
                    System.Threading.Thread.Sleep(Constants.LOOP_INTERVAL_MS);

                    continue;
                }

                work.Started = true;
                work.StartTime = DateTime.Now;

                var result = await workerHandler.UpdateWorkAsync(work);

                if (!result)
                {
                    System.Threading.Thread.Sleep(Constants.LOOP_INTERVAL_MS);

                    continue;
                }

                var options = new TrainerCommandLineOptions
                {
                    FolderOfData = work.TrainingDataPath,
                    LogLevel = LogLevels.DEBUG
                };

                var outputModelPath = string.Empty;

                switch (Enum.Parse(typeof(ModelType), work.ModelType))
                {
                    case ModelType.CLASSIFICATION:
                        outputModelPath = new ClassificationEngine().TrainModel(options);
                        break;
                    case ModelType.CLUSTERING:
                        outputModelPath = new ClusteringEngine().TrainModel(options);
                        break;
                }

                if (File.Exists(outputModelPath))
                {
                    work.Model = File.ReadAllBytes(outputModelPath);
                }

                work.Completed = true;
                work.CompletedTime = DateTime.Now;

                _ = await workerHandler.UpdateWorkAsync(work);

                System.Threading.Thread.Sleep(Constants.LOOP_INTERVAL_MS);
            }
        }
    }
}