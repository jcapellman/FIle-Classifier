using Microsoft.ML.Data;

namespace FileClassifier.lib.ML.Classification.Objects
{
    public class ClassificationDataPrediction : ClassificationData
    {
        [ColumnName("PredictedLabel")]
        public bool Prediction { get; set; }

        public float Probability { get; set; }

        public float Score { get; set; }
    }
}