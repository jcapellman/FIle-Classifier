using System;
using System.Collections.Generic;
using System.Linq;

using FileClassifier.JobManager.lib.Databases.Base;
using FileClassifier.JobManager.lib.Databases.Tables;

using Microsoft.AspNetCore.Mvc;

namespace FileClassifier.JobManager.REST.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobController : ControllerBase
    {
        private readonly IDatabase _database;

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

            var hosts = _database.GetHosts();

            if (hosts.Any())
            {
                var jobs = _database.GetJobs().Where(a => !a.Completed).ToList();

                foreach (var host in hosts)
                {
                    if (jobs.Any(a => a.AssignedHost == host.Name))
                    {
                        continue;
                    }

                    item.AssignedHost = host.Name;

                    break;
                }

                if (string.IsNullOrEmpty(item.AssignedHost))
                {
                    item.AssignedHost = lib.Common.Constants.UNASSIGNED_JOB;
                }
            } else
            {
                item.AssignedHost = lib.Common.Constants.UNASSIGNED_JOB;
            }

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