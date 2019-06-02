using System.Linq;

using FileClassifier.JobManager.lib.Common;
using FileClassifier.JobManager.lib.Databases.Base;
using FileClassifier.JobManager.lib.Databases.Tables;

using Microsoft.AspNetCore.Mvc;

namespace FileClassifier.JobManager.REST.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkerController : ControllerBase
    {
        private readonly IDatabase _database;

        public WorkerController(IDatabase database)
        {
            _database = database;
        }   

        [HttpGet]
        public Jobs GetWork(string hostName)
        {
            var jobs = _database.GetJobs().Where(a => !a.Completed).ToList();

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
            unassignedJob.Name = hostName;
            
            _database.UpdateJob(unassignedJob);

            return unassignedJob;
        }

        [HttpPost]
        public void UpdateWork([FromBody]Jobs job)
        {
            _database.UpdateJob(job);
        }
    }
}