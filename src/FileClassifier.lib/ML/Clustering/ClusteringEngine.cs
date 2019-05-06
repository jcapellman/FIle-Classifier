﻿using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using FileClassifier.lib.Base;
using FileClassifier.lib.Common;
using FileClassifier.lib.Enums;
using FileClassifier.lib.ML.Base;
using FileClassifier.lib.ML.Clustering.Objects;
using FileClassifier.lib.Options;

using Microsoft.ML;
using Microsoft.ML.Data;

namespace FileClassifier.lib.ML.Clustering
{
    public class ClusteringEngine : BasePrediction<ClusterData, ClusterDataPrediction>
    {
        private const int STRING_BYTE_LIMIT = 65536;

        private const int BUFFER_SIZE = 2048;

        private const int FILE_ENCODING = 1252;

        protected override string MODEL_NAME => "clustering.mdl";

        protected override ClassifierResponseItem UpdateResponse(ClusterDataPrediction prediction, ClassifierResponseItem response, ClassifierCommandLineOptions options)
        {
            response.FileGroup = (FileGroupType) prediction.PredictedClusterId;

            var distances = prediction.Distances.Select((t, x) => $"{x}:{t}").ToList();

            Logger<ClassifierCommandLineOptions>.Debug($"Distances: {string.Join("|", distances)}", options);

            response.UpdateStatus(ClassifierStatus.SUCCESS);

            return response;
        }

        private static Regex _stringRex;

        public override (ClusterData Data, string Output) FeatureExtraction(ClassifierResponseItem response)
        {
            var clusterData = new ClusterData
            {
                StringData = string.Empty
            };

            var stringLines = new StringBuilder();

            var data = (response.Data.Length > STRING_BYTE_LIMIT ? response.Data.AsSpan(0, STRING_BYTE_LIMIT) : response.Data.AsSpan());

            using (var ms = new MemoryStream(data.ToArray(), false))
            {
                using (var streamReader = new StreamReader(ms, Encoding.GetEncoding(FILE_ENCODING), false, BUFFER_SIZE, false)) {
                    while (!streamReader.EndOfStream)
                    {
                        var line = streamReader.ReadLine();

                        if (string.IsNullOrEmpty(line))
                        {
                            continue;
                        }

                        stringLines.Append(string.Join(string.Empty,
                            _stringRex.Matches(line).Where(a => !string.IsNullOrEmpty(a.Value) && !string.IsNullOrWhiteSpace(a.Value)).ToList()));
                    }
                }                
            }

            return (clusterData, $"{(int)response.FileGroup},{string.Join(string.Empty, stringLines)}");
        }

        public ClusteringEngine()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            _stringRex = new Regex(@"[ -~\t]{8,}", RegexOptions.Compiled);
        }

        public override bool TrainModel(TrainerCommandLineOptions options)
        {            
            var fileName = FeatureExtractFolder(options);

            var startDate = DateTime.Now;

            var fullData = MlContext.Data.LoadFromTextFile(path: fileName,
                new[]
                {
                    new TextLoader.Column("Label", DataKind.Single, 0),
                    new TextLoader.Column(nameof(ClusterData.StringData), DataKind.String, 1)
                },
                hasHeader: false,
                separatorChar: ',');

            var trainTestData = MlContext.Data.TrainTestSplit(fullData, testFraction: 0.2);
            var trainingDataView = trainTestData.TrainSet;
            var testingDataView = trainTestData.TestSet;

            var pipeline = MlContext.Transforms.Text.NormalizeText(nameof(ClusterData.StringData))
                .Append(MlContext.Transforms.Text.TokenizeIntoWords(nameof(ClusterData.StringData)))
                .Append(MlContext.Transforms.Text.RemoveDefaultStopWords(nameof(ClusterData.StringData)))
                .Append(MlContext.Transforms.Conversion.MapValueToKey(nameof(ClusterData.StringData)))
                .Append(MlContext.Transforms.Text.ProduceNgrams(nameof(ClusterData.StringData)))
                .Append(MlContext.Transforms.NormalizeLpNorm(nameof(ClusterData.StringData)));

            var trainer = MlContext.Clustering.Trainers.KMeans(featureColumnName: nameof(ClusterData.StringData), numberOfClusters: 6);

            var trainingPipeline = pipeline.Append(trainer);
            var trainedModel = trainingPipeline.Fit(trainingDataView);

            Logger<TrainerCommandLineOptions>.Debug($"Model trained in {DateTime.Now.Subtract(startDate).TotalSeconds} seconds", options);
            
            var predictions = trainedModel.Transform(testingDataView);

            var metrics = MlContext.Clustering.Evaluate(predictions, scoreColumnName: "Score", featureColumnName: nameof(ClusterData.StringData));

            Logger<TrainerCommandLineOptions>.Debug($"Average Distance: {metrics.AverageDistance} | Davides Bouldin Index: {metrics.DaviesBouldinIndex}", options);

            SaveModel(trainedModel, trainingDataView.Schema, options);

            return true;
        }
    }
}