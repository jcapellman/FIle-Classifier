using FileClassifier.lib.ML.Base.Objects;

using Microsoft.ML.Data;

namespace FileClassifier.lib.ML.Clustering.Objects
{
    public class ClusterDataPrediction : BasePredictionData
    {
        [ColumnName("PredictedLabel")]
        public uint PredictedClusterId;

        [ColumnName("Score")]
        public float[] Distances;
    }
}