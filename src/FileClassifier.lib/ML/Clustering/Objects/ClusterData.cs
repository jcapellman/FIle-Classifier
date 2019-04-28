using Microsoft.ML.Data;

namespace FileClassifier.lib.ML.Clustering.Objects
{
    public class ClusterData
    {
        [LoadColumn(0)]
        public string Grams;
    }
}