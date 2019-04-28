using Microsoft.ML.Data;

namespace FileClassifier.lib.ML.Classification.Objects
{
    public class ClassificationData
    {
        [LoadColumn(0)]
        public string NGramText;

        [LoadColumn(1), ColumnName("Label")]
        public bool Malicious;
    }
}