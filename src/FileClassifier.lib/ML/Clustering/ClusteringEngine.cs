using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using FileClassifier.lib.Common;
using FileClassifier.lib.Enums;
using FileClassifier.lib.ML.Base;
using FileClassifier.lib.ML.Clustering.Objects;

using Microsoft.ML;
using Microsoft.ML.Data;

namespace FileClassifier.lib.ML.Clustering
{
    public class ClusteringEngine : BasePrediction<ClusterData, ClusterDataPrediction>
    {
        protected override string MODEL_NAME => "clustering.mdl";

        protected override ClassifierResponseItem UpdateResponse(ClusterDataPrediction prediction, ClassifierResponseItem response)
        {
            response.FileGroup = (FileGroupType) prediction.PredictedClusterId;

            return response;
        }

        private static Regex StringRex;

        public override (ClusterData Data, string Output) FeatureExtraction(ClassifierResponseItem response)
        {
            var clusterData = new ClusterData
            {
                StringData = string.Empty
            };

            var stringLines = new StringBuilder();

            using (var ms = new MemoryStream(response.Data, false))
            {
                using (var streamReader = new StreamReader(ms, Encoding.GetEncoding(1252), false, 2048, false)) {
                    while (!streamReader.EndOfStream)
                    {
                        stringLines.Append(string.Join(string.Empty,
                            StringRex.Matches(streamReader.ReadLine()).OfType<Match>().Where(a => !string.IsNullOrEmpty(a.Value) && !string.IsNullOrWhiteSpace(a.Value)).ToList()));
                    }
                }                
            }

            return (clusterData, $"{(int)response.FileGroup},{string.Join(string.Empty, stringLines)}");
        }

        public ClusteringEngine()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            StringRex = new Regex(pattern: @"[ -~\t]{8,}", RegexOptions.Compiled);
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

            DataOperationsCatalog.TrainTestData trainTestData = MlContext.Data.TrainTestSplit(fullData, testFraction: 0.2);
            var trainingDataView = trainTestData.TrainSet;
            var testingDataView = trainTestData.TestSet;

            var pipeline = MlContext.Transforms.Text.NormalizeText(nameof(ClusterData.StringData))
                .Append(MlContext.Transforms.Text.TokenizeIntoWords(nameof(ClusterData.StringData)))
                .Append(MlContext.Transforms.Text.RemoveDefaultStopWords(nameof(ClusterData.StringData)))
                .Append(MlContext.Transforms.Conversion.MapValueToKey(nameof(ClusterData.StringData)))
                .Append(MlContext.Transforms.Text.ProduceNgrams(nameof(ClusterData.StringData)))
                .Append(MlContext.Transforms.NormalizeLpNorm(nameof(ClusterData.StringData)));

            var trainer = MlContext.Clustering.Trainers.KMeans(featureColumnName: nameof(ClusterData.StringData), numberOfClusters: 3);

            var trainingPipeline = pipeline.Append(trainer);
            var trainedModel = trainingPipeline.Fit(trainingDataView);

            Console.WriteLine($"Model trained in {DateTime.Now.Subtract(startDate).TotalSeconds} seconds");

            IDataView predictions = trainedModel.Transform(testingDataView);

            var metrics = MlContext.Clustering.Evaluate(predictions, scoreColumnName: "Score", featureColumnName: nameof(ClusterData.StringData));

            Console.WriteLine($"Average Distance: {metrics.AverageDistance} | Davides Bouldin Index: {metrics.DaviesBouldinIndex}");

            using (var fileStream = new FileStream(MODEL_NAME, FileMode.Create, FileAccess.Write, FileShare.Write))
            {
                MlContext.Model.Save(trainedModel, trainingDataView.Schema, fileStream);
            }

            return true;
        }
    }
}