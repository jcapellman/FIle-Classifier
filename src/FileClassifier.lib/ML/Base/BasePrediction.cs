﻿using System;
using System.Collections.Concurrent;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

using FileClassifier.lib.Base;
using FileClassifier.lib.Common;
using FileClassifier.lib.ML.Base.Objects;
using FileClassifier.lib.Options;

using Microsoft.ML;

namespace FileClassifier.lib.ML.Base
{
    public abstract class BasePrediction<T, TK> where T : BaseData where TK : BasePredictionData, new()
    {
        protected abstract string MODEL_NAME { get; }

        protected static readonly MLContext MlContext = new MLContext(Common.Constants.ML_SEED);

        public ClassifierResponseItem Predict(ClassifierResponseItem response, ClassifierCommandLineOptions options)
        {
            if (response == null)
            {
                throw new ArgumentNullException(nameof(response));
            }

            var assembly = typeof(BasePredictionData).GetTypeInfo().Assembly;

            var resource = assembly.GetManifestResourceStream($"FileClassifier.lib.Models.{MODEL_NAME}");

            var model = MlContext.Model.Load(resource, out var schema);

            var predictor = MlContext.Model.CreatePredictionEngine<T, TK>(model);

            var (Data, Output) = FeatureExtraction(response);

            var result = predictor.Predict(Data);

            return UpdateResponse(result, response, options);
        }

        protected string FeatureExtractFolder(TrainerCommandLineOptions options)
        {
            var fileName = Path.Combine(AppContext.BaseDirectory, $"{DateTime.Now.Ticks}.txt");

            var files = Directory.GetFiles(options.FolderOfData);

            Logger<TrainerCommandLineOptions>.Debug($"{files.Length} Files found for training...", options);

            var stopWatch = DateTime.Now;

            var extractions = new ConcurrentQueue<string>();

            Parallel.ForEach(files, file =>
            {
                var (data, output) = FeatureExtraction(new ClassifierResponseItem(File.ReadAllBytes(file), file));

                extractions.Enqueue(output);
            });

            File.WriteAllText(fileName, string.Join(System.Environment.NewLine, extractions));

            Logger<TrainerCommandLineOptions>.Debug($"Feature Extraction took {DateTime.Now.Subtract(stopWatch).TotalSeconds} seconds", options);

            return fileName;
        }

        protected string OutputModelPath =>
            Path.Combine(AppContext.BaseDirectory, @"..\..\FileClassifier.lib\Models", MODEL_NAME);

        protected void SaveModel(ITransformer trainedModel, DataViewSchema schema, TrainerCommandLineOptions options)
        {
            using (var fileStream = new FileStream(OutputModelPath, FileMode.Create, FileAccess.Write, FileShare.Write))
            {
                MlContext.Model.Save(trainedModel, schema, fileStream);
            }

            Logger<TrainerCommandLineOptions>.Debug($"Model saved to {OutputModelPath}", options);
        }

        protected abstract ClassifierResponseItem UpdateResponse(TK prediction, ClassifierResponseItem response, ClassifierCommandLineOptions options);

        public abstract (T Data, string Output) FeatureExtraction(ClassifierResponseItem response);

        public abstract bool TrainModel(TrainerCommandLineOptions options);
    }
}