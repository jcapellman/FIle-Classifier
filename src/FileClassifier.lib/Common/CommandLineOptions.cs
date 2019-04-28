namespace FileClassifier.lib.Common
{
    public class Options
    {
        public string FileName { get; set; }

        public bool Verbose { get; set; }

        public override string ToString() => $"FileName: {FileName} | Verbose: {Verbose}";
    }
}