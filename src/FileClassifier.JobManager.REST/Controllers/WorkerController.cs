using System.Linq;

using FileClassifier.JobManager.lib.Common;
using FileClassifier.JobManager.lib.Databases.Base;
using FileClassifier.JobManager.lib.Databases.Tables;

using Microsoft.AspNetCore.Mvc;

using NLog;

namespace FileClassifier.JobManager.REST.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkerController : BaseController
    {
        private readonly Logger Log = NLog.LogManager.GetCurrentClassLogger();

        public WorkerController(IDatabase database) : base(database) { }
    
        [HttpGet]
        public Jobs GetWork(string hostName)
        {
            var jobs = Database.GetJobs().Where(a => !a.Completed).ToList();

            var assignedJob = jobs.FirstOrDefault(a => a.AssignedHost == hostName);

            if (assignedJob != null)
            {
                Log.Debug($"{hostName} returned {assignedJob.ID}");

                return assignedJob;
            }

            var unassignedJob = jobs.FirstOrDefault(a => a.AssignedHost == Constants.UNASSIGNED_JOB);

            // No jobs available
            if (unassignedJob == null)
            {
                Log.Debug($"No unassigned jobs for {hostName}");

                return null;
            }

            // Assign the first unassigned job to the hostName
            unassignedJob.AssignedHost = hostName;

            Log.Debug($"Assigned {unassignedJob.ID} to {hostName}");

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