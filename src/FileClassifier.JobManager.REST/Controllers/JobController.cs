using System;
using System.Collections.Generic;

using FileClassifier.JobManager.lib.Databases.Base;
using FileClassifier.JobManager.lib.Databases.Tables;

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
        public List<Jobs> Get() => _database.GetJobs();

        [HttpGet("{id}")]
        public Jobs Get(Guid id) => _database.GetJob(id);

        [HttpPut]
        public bool Put(Jobs item) => _database.UpdateJob(item);

        [HttpPost]
        public Guid Post([FromBody]Jobs item)
        {
            item.ID = Guid.NewGuid();

            _database.AddJob(item);
            
            return item.ID;
        }

        [HttpDelete("{id}")]
        public void Delete(Guid id)
        {
            _database.DeleteJob(id);
        }
    }
}