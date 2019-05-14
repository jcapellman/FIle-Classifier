using FileClassifier.lib.ML.Base.Objects;

using Microsoft.ML.Data;

namespace FileClassifier.lib.ML.Classification.Objects
{
    public class ClassificationData : BaseData
    {
        [LoadColumn(0)]
        public bool Label { get; set; }

        [LoadColumn(1)]
        public float FileGroupType { get; set; }

        [LoadColumn(2)]
        public string NGramText { get; set; }
    }
}