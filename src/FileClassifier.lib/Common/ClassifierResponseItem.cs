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

        public bool IsMalicious { get; set; }

        public ClassifierStatus Status { get; private set; }

        public long SizeInBytes { get; set; }

        public Exception Exception { get; private set; }

        public FileGroupType FileGroup { get; set; }

        public ClassifierResponseItem()
        {
            FileGroup = FileGroupType.UNKNOWN;

            Confidence = 0.0;

            Status = ClassifierStatus.INCOMPLETE;
        }

        public ClassifierResponseItem(Exception exception)
        {
            Exception = exception;

            Status = ClassifierStatus.ERROR;
        }

        public ClassifierResponseItem(byte[] data, FileGroupType fileGroup = FileGroupType.UNKNOWN)
        {
            Data = data;

            SizeInBytes = data.Length;

            SHA1Hash = data.ToSHA1();

            FileGroup = fileGroup;
        }

        public override string ToString()
        {
            var output = $"SHA1: {SHA1Hash} | Size (bytes): {SizeInBytes} | File Group: {FileGroup} | Confidence: {Confidence} | Malicious: {IsMalicious} | Status: {Status}";

            if (Status == ClassifierStatus.ERROR)
            {
                output += $" | {Exception}";
            }

            return output;
        }
    }
}