using System;

using FileClassifier.lib.Enums;
using FileClassifier.lib.Helpers;

namespace FileClassifier.lib.Common
{
    public class ClassifierResponseItem
    {
        public byte[] Data { get; private set; }

        public string SHA1Hash { get; set; }

        public double Confidence { get; set; }

        public long SizeInBytes { get; set; }

        public Exception Exception { get; private set; }

        public FileGroupType FileGroup { get; set; }

        public ClassifierResponseItem()
        {
            FileGroup = FileGroupType.UNKNOWN;

            Confidence = 0.0;
        }

        public ClassifierResponseItem(Exception exception)
        {
            Exception = exception;
        }

        public ClassifierResponseItem(byte[] data)
        {
            Data = data;

            SizeInBytes = data.Length;

            SHA1Hash = data.ToSHA1();
        }

        public override string ToString() => $"SHA1: {SHA1Hash} | Size (bytes): {SizeInBytes} | File Group: {FileGroup} | Confidence: {Confidence}";
    }
}