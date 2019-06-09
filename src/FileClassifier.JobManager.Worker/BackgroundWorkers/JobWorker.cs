using System;
using System.IO;
using System.Threading.Tasks;

using FileClassifier.JobManager.lib.Databases;
using FileClassifier.JobManager.lib.Databases.Tables;
using FileClassifier.JobManager.lib.Handlers;
using FileClassifier.JobManager.Worker.Common;

using FileClassifier.lib.Enums;
using FileClassifier.lib.ML.Classification;
using FileClassifier.lib.ML.Clustering;
using FileClassifier.lib.Options;

namespace FileClassifier.JobManager.Worker.BackgroundWorkers
{
    public class JobWorker
    {
        private Hosts _host;

        private string _serverURL;

        public async Task<bool> Run(Hosts host, string serverURL)
        {
            _host = host;

            _serverURL = serverURL;

            var workerHandler = new WorkerHandler(_serverURL);

            var work = await workerHandler.GetWorkAsync(_host.Name);

            if (work == null)
            {
                System.Threading.Thread.Sleep(Constants.LOOP_INTERVAL_MS);

                return false;
            }

            work.Started = true;
            work.StartTime = DateTime.Now;

            var result = await workerHandler.UpdateWorkAsync(work);

            if (!result)
            {
                System.Threading.Thread.Sleep(Constants.LOOP_INTERVAL_MS);

                return false;
            }

            if (!Directory.Exists(work.TrainingDataPath))
            {
                work.Completed = true;
                work.Debug = $"Path ({work.TrainingDataPath}) does not exist";
                work.CompletedTime = DateTime.Now;

                result = await workerHandler.UpdateWorkAsync(work);

                return false;
            }

            var options = new TrainerCommandLineOptions
            {
                FolderOfData = work.TrainingDataPath,
                LogLevel = LogLevels.DEBUG
            };

            var (outputFile, metrics) = (string.Empty, string.Empty);

            switch (Enum.Parse<ModelType>(work.ModelType, true))
            {
                case ModelType.CLASSIFICATION:
                    (outputFile, metrics) = new ClassificationEngine().TrainModel(options);
                    break;
                case ModelType.CLUSTERING:
                    (outputFile, metrics) = new ClusteringEngine().TrainModel(options);
                    break;
            }

            if (File.Exists(outputFile))
            {
                work.Model = File.ReadAllBytes(outputFile);
            }

            work.ModelEvaluationMetrics = metrics;
            work.Completed = true;
            work.CompletedTime = DateTime.Now;

            result = await workerHandler.UpdateWorkAsync(work);

            if (result)
            {
                Console.WriteLine($"Successfully trained model and saved to {outputFile}");
            } else
            {
                AddToPending(work);
            }

            return result;
        }

        private void AddToPending(Jobs work)
        {
            var db = new LiteDBDatabase();

            db.AddOfflineSubmission(work);

            // LOG HERE
        }
    }
}