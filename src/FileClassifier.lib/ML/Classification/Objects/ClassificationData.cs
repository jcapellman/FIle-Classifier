using FileClassifier.lib.ML.Base.Objects;

using Microsoft.ML.Data;

namespace FileClassifier.lib.ML.Classification.Objects
{
    public class ClassificationData : BaseData
    {
        [LoadColumn(0)]
        public string NGramText;

        [LoadColumn(1), ColumnName("Label")]
        public bool Malicious;

        [LoadColumn(2)]
        public int FileGroupType;
    }
}