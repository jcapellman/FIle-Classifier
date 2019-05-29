using System;
using System.Collections.Generic;

using FileClassifier.lib.JobObjects;

using Microsoft.AspNetCore.Mvc;

namespace FileClassifier.JobManager.REST.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
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

            // Store the item

            return guid;
        }

        [HttpDelete("{id}")]
        public void Delete(Guid id)
        {
            // Deletes the specified job
        }
    }
}