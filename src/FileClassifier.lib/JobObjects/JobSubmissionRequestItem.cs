namespace FileClassifier.lib.JobObjects
{
    public class JobSubmissionRequestItem
    {
        public string TrainingSetPath { get; set; }

        public bool UseAllCores { get; set; }

        public string Name { get; set; }
    }
}