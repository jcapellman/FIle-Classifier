﻿using System.Collections.Generic;
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

        public List<Jobs> GetWork(string host)
        {
            var assignedJobs = _database.GetJobs().Where(a => a.AssignedHost == host).ToList();

            if (assignedJobs.Any())
            {
                return assignedJobs;
            }

            var unassignedJob = _database.GetJobs().FirstOrDefault(a => a.Name == Constants.UNASSIGNED_JOB);

            // No jobs available
            if (unassignedJob == null)
            {
                return new List<Jobs>();
            }

            // Assign the first unassigned job to the host
            unassignedJob.Name = host;
            _database.UpdateJob(unassignedJob);

            return new List<Jobs> { unassignedJob };
        }
    }
}