using Microsoft.ML.Data;

namespace FileClassifier.lib.ML.Clustering.Objects
{
    public class ClusterDataPrediction
    {
        [ColumnName("PredictedLabel")]
        public uint PredictedClusterId;

        [ColumnName("Score")]
        public float[] Distances;
    }
}