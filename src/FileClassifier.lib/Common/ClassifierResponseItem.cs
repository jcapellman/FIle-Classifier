using FileClassifier.lib.Enums;

namespace FileClassifier.lib.Common
{
    public class ClassifierResponseItem
    {
        public string SHA1Hash { get; set; }

        public double Confidence { get; set; }

        public long SizeInBytes { get; set; }

        public FileGroupType FileGroup { get; set; }

        public ClassifierResponseItem()
        {
            FileGroup = FileGroupType.UNKNOWN;

            Confidence = 0.0;
        }

        public override string ToString() => $"SHA1: {SHA1Hash} | Size (bytes): {SizeInBytes} | File Group: {FileGroup} | Confidence: {Confidence}";
    }
}