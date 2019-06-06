using System.Collections.Generic;

using FileClassifier.JobManager.lib.Databases.Tables;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FileClassifier.JobManager.REST.Models
{
    public class HomeDashboardModel
    {
        public List<Hosts> Hosts { get; set; }
        
        public List<Jobs> Jobs { get; set; }

        public List<SelectListItem> ModelTypes { get; set; }
    }
}