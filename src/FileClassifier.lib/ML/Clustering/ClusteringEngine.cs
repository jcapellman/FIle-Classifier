using FileClassifier.lib.Common;
using FileClassifier.lib.ML.Base;
using FileClassifier.lib.ML.Clustering.Objects;

namespace FileClassifier.lib.ML.Clustering
{
    public class ClusteringEngine : BasePrediction<ClusterData, ClusterDataPrediction>
    {
        protected override string MODEL_NAME => "clustering.mdl";

        protected override ClassifierResponseItem UpdateResponse(ClusterDataPrediction prediction, ClassifierResponseItem response)
        {
            // TODO: Map Id to Group

            // TODO: Set the Group in the response

            return response;
        }

        public override ClusterData FeatureExtraction(ClassifierResponseItem response)
        {
            return new ClusterData();
        }
    }
}