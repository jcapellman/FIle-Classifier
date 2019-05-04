using System;
using System.IO;

using FileClassifier.lib.Common;
using FileClassifier.lib.Enums;
using FileClassifier.lib.ML.Base;
using FileClassifier.lib.ML.Clustering.Objects;

using Microsoft.ML;

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

        public override ClusterData FeatureExtraction(ClassifierResponseItem response)
        {
            var clusterData = new ClusterData
            {
                Size = response.SizeInBytes,
                Grams = float.MinValue
            };

            return clusterData;
        }

        public override bool TrainModel(TrainerCommandLineOptions options)
        {
            var fileName = FeatureExtractFolder(options);

            var dataView = MlContext.Data.LoadFromTextFile<ClusterData>(fileName, hasHeader: false, separatorChar: ',');

            var featuresColumnName = "Features";

            var pipeline = MlContext.Transforms
                .Concatenate(featuresColumnName, "Size", "Grams")
                .Append(MlContext.Clustering.Trainers.KMeans(featuresColumnName, numberOfClusters: Enum.GetNames(typeof(FileGroupType)).Length));

            var model = pipeline.Fit(dataView);

            using (var fileStream = new FileStream(MODEL_NAME, FileMode.CreateNew, FileAccess.Write, FileShare.Write))
            {
                MlContext.Model.Save(model, dataView.Schema, fileStream);
            }

            return true;
        }
    }
}