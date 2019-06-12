using System;
using System.Linq;

using FileClassifier.JobManager.lib.Databases.Base;
using FileClassifier.JobManager.lib.Databases.Tables;

using Microsoft.AspNetCore.Mvc;

using NLog;

namespace FileClassifier.JobManager.REST.Controllers
{
    public class BaseController : Controller
    {
        private readonly Logger Log = NLog.LogManager.GetCurrentClassLogger();

        protected readonly IDatabase Database;

        protected BaseController(IDatabase database)
        {
            Database = database;
        }

        protected Guid SaveJob(Jobs job)
        {
            job.ID = Guid.NewGuid();
            job.SubmissionTime = DateTime.Now;

            var hosts = Database.GetHosts();

            if (hosts.Any())
            {
                var jobs = Database.GetJobs().Where(a => !a.Completed).ToList();

                foreach (var host in hosts)
                {
                    if (jobs.Any(a => a.AssignedHost == host.Name))
                    {
                        continue;
                    }

                    job.AssignedHost = host.Name;

                    Log.Debug($"Assigned {job.ID} to {host.Name}");

                    break;
                }

                if (string.IsNullOrEmpty(job.AssignedHost))
                {
                    job.AssignedHost = lib.Common.Constants.UNASSIGNED_JOB;
                    Log.Debug($"No hosts found for {job.ID}");
                }
            }
            else
            {
                job.AssignedHost = lib.Common.Constants.UNASSIGNED_JOB;

                Log.Debug($"No hosts found for {job.ID}");
            }

            Database.AddJob(job);

            return job.ID;
        }
    }
}