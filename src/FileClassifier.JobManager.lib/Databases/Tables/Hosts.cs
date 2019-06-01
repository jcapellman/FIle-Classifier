using System;

namespace FileClassifier.JobManager.lib.Databases.Tables
{
    public class Hosts
    {
        public Guid ID { get; set; }

        public string Name { get; set; }

        public int NumCores { get; set; }

        public string OSVersion { get; set; }

        public DateTime LastConnected { get; set; }
        
        public string WorkerVersion { get; set; }
    }
}