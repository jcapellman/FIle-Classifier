using System.Linq;

using FileClassifier.JobManager.lib.Common;
using FileClassifier.JobManager.lib.Databases.Base;
using FileClassifier.JobManager.lib.Databases.Tables;

using Microsoft.AspNetCore.Mvc;

namespace FileClassifier.JobManager.REST.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkerController : BaseController
    {
        public WorkerController(IDatabase database) : base(database) { }
    
        [HttpGet]
        public Jobs GetWork(string hostName)
        {
            var jobs = Database.GetJobs().Where(a => !a.Completed).ToList();

            var assignedJob = jobs.FirstOrDefault(a => a.AssignedHost == hostName);

            if (assignedJob != null)
            {
                return assignedJob;
            }

            var unassignedJob = jobs.FirstOrDefault(a => a.AssignedHost == Constants.UNASSIGNED_JOB);

            // No jobs available
            if (unassignedJob == null)
            {
                return null;
            }

            // Assign the first unassigned job to the hostName
            unassignedJob.AssignedHost = hostName;

            Database.UpdateJob(unassignedJob);

            return unassignedJob;
        }

        [HttpPost]
        [RequestSizeLimit(100_000_000)]
        public void UpdateWork([FromBody]Jobs job)
        {
            Database.UpdateJob(job);
        }
    }
}