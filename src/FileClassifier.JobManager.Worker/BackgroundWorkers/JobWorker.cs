﻿using System;
using System.IO;
using System.Threading.Tasks;

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

            return await workerHandler.UpdateWorkAsync(work);
        }
    }
}