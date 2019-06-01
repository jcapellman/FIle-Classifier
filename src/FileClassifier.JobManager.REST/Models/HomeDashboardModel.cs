using System.Collections.Generic;

using FileClassifier.JobManager.lib.Databases.Tables;

namespace FileClassifier.JobManager.REST.Models
{
    public class HomeDashboardModel
    {
        public List<Hosts> Hosts { get; set; }
        
        public List<Jobs> Jobs { get; set; }
    }
}