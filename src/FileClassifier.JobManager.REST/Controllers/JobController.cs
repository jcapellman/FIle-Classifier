using System;
using System.Collections.Generic;

using FileClassifier.JobManager.lib.Databases.Base;
using FileClassifier.JobManager.lib.Databases.Tables;

using Microsoft.AspNetCore.Mvc;

namespace FileClassifier.JobManager.REST.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobController : BaseController
    {
        public JobController(IDatabase database) : base(database) { }

        [HttpGet]
        public List<Jobs> Get() => Database.GetJobs();

        [HttpGet("{id}")]
        public Jobs Get(Guid id) => Database.GetJob(id);

        [HttpPut]
        public bool Put(Jobs item) => Database.UpdateJob(item);

        [HttpPost]
        public Guid Post([FromBody]Jobs item) => SaveJob(item);

        [HttpDelete("{id}")]
        public void Delete(Guid id)
        {
            Database.DeleteJob(id);
        }
    }
}