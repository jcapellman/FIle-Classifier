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
            return new ClusterData();
        }

        public override bool TrainModel(string dataFile)
        {
            var dataView = MlContext.Data.LoadFromTextFile<ClusterData>(dataFile, hasHeader: false, separatorChar: ',');

            var featuresColumnName = "Features";

            var pipeline = MlContext.Transforms
                .Concatenate(featuresColumnName, "Grams")
                .Append(MlContext.Clustering.Trainers.KMeans(featuresColumnName, numberOfClusters: 3));

            var model = pipeline.Fit(dataView);

            using (var fileStream = new FileStream(MODEL_NAME, FileMode.Create, FileAccess.Write, FileShare.Write))
            {
                MlContext.Model.Save(model, dataView.Schema, fileStream);
            }

            return true;
        }
    }
}