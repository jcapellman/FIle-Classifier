using System.IO;

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

        public override (ClusterData Data, string Output) FeatureExtraction(ClassifierResponseItem response)
        {
            var clusterData = new ClusterData
            {
                Size = response.SizeInBytes,
                Grams = int.MinValue
            };
            
            return (clusterData, $"{(int)response.FileGroup},{clusterData.Size},{clusterData.Grams}");
        }

        public override bool TrainModel(TrainerCommandLineOptions options)
        {
            var fileName = FeatureExtractFolder(options);

            var dataView = MlContext.Data.LoadFromTextFile(path: fileName,
                new[]
                {
                    new TextLoader.Column("Label", DataKind.Single, 0),
                    new TextLoader.Column(nameof(ClusterData.Size), DataKind.Single, 1),
                    new TextLoader.Column(nameof(ClusterData.Grams), DataKind.Single, 2)
                },
                hasHeader: true,
                separatorChar: ',');

            var featuresColumnName = "Features";

            var pipeline = MlContext.Transforms
                .Concatenate(featuresColumnName, nameof(ClusterData.Size), nameof(ClusterData.Grams));

            var trainer = MlContext.Clustering.Trainers.KMeans(featureColumnName: featuresColumnName, numberOfClusters: 3);

            var trainingPipeline = pipeline.Append(trainer);
            var model = trainingPipeline.Fit(dataView);

            using (var fileStream = new FileStream(MODEL_NAME, FileMode.CreateNew, FileAccess.Write, FileShare.Write))
            {
                MlContext.Model.Save(model, dataView.Schema, fileStream);
            }

            return true;
        }
    }
}