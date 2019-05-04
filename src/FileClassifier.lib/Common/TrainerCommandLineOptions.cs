namespace FileClassifier.lib.Common
{
    public class TrainerCommandLineOptions
    {
        public string FolderOfData { get; set; }

        public bool Verbose { get; set; }

        public override string ToString() => $"Folder Containing Data: {FolderOfData} | Verbose: {Verbose}";
    }
}