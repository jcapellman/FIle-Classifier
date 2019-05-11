using System;
using System.IO;
using System.Linq;

using FileClassifier.lib.Enums;
using FileClassifier.lib.Helpers;

namespace FileClassifier.lib.Common
{
    public class ClassifierResponseItem
    {
        public byte[] Data { get; private set; }

        public string FileName { get; private set; }

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

        private static FileGroupType GetGroupType(string fileName, bool useFileName = false)
        {
            if (string.IsNullOrEmpty(fileName) || !useFileName)
            {
                return FileGroupType.UNKNOWN;
            }

            var groupType = Enum.GetNames(typeof(FileGroupType))
                .FirstOrDefault(a => fileName.Contains(a, StringComparison.InvariantCultureIgnoreCase));

            if (!string.IsNullOrEmpty(groupType))
            {
                return Enum.Parse<FileGroupType>(groupType);
            }

            var extension = Path.GetExtension(fileName).ToLower();

            switch (extension)
            {
                case ".doc":
                case ".xlsx":
                case ".pptx":
                case ".docx":
                case ".pdf":
                    return FileGroupType.DOCUMENT;
                case ".jpg":
                case ".png":
                    return FileGroupType.IMAGE;
                case ".dll":
                case ".exe":
                    return FileGroupType.EXECUTABLE;
                case ".mp4":
                    return FileGroupType.VIDEO;
                case ".ps1":
                    return FileGroupType.SCRIPT;
            }

            return FileGroupType.UNKNOWN;
        }

        public ClassifierResponseItem(byte[] data, string fileName, bool useFileName = false)
        {
            Data = data;

            FileName = fileName;

            SizeInBytes = data.Length;

            SHA1Hash = data.ToSHA1();

            FileGroup = GetGroupType(FileName, useFileName);

            IsMalicious = fileName.Contains("malicious", StringComparison.InvariantCultureIgnoreCase);

            Status = ClassifierStatus.INCOMPLETE;
        }

        public void UpdateStatus(ClassifierStatus status)
        {
            Status = status;
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