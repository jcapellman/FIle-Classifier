using System;
using System.Collections.Generic;

using FileClassifier.JobManager.lib.Databases.Base;
using FileClassifier.JobManager.lib.Databases.Tables;
using FileClassifier.lib.JobObjects;

using Microsoft.AspNetCore.Mvc;

namespace FileClassifier.JobManager.REST.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobController : ControllerBase
    {
        private IDatabase _database;

        public JobController(IDatabase database)
        {
            _database = database;
        }

        [HttpGet]
        public List<JobStatusResponseItem> Get()
        {
            // Returns all current jobs
            return new List<JobStatusResponseItem>();
        }

        [HttpGet("{id}")]
        public JobStatusResponseItem Get(Guid id)
        {
            return new JobStatusResponseItem();
        }

        [HttpPost]
        public Guid Post([FromBody]JobSubmissionRequestItem item)
        {
            var guid = Guid.NewGuid();

            var job = new Jobs
            {
                ID = guid,
                Completed = false,
                Name = item.Name,
                SubmissionTime = DateTime.Now
            };

            _database.AddJob(job);
            
            return guid;
        }

        [HttpDelete("{id}")]
        public void Delete(Guid id)
        {
            _database.DeleteJob(id);
        }
    }
}