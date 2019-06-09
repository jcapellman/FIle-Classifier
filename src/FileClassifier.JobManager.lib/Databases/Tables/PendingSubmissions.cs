using System;

namespace FileClassifier.JobManager.lib.Databases.Tables
{
    public class PendingSubmissions
    {
        public Guid ID { get; set; }

        public string JobJSON { get; set; }
    }
}