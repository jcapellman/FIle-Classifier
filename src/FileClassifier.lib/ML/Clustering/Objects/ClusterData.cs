using FileClassifier.lib.ML.Base.Objects;

using Microsoft.ML.Data;

namespace FileClassifier.lib.ML.Clustering.Objects
{
    public class ClusterData : BaseData
    {
        [LoadColumn(0)]
        public float Size;

        [LoadColumn(1)]
        public float Grams;
    }
}