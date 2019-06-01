using System;

using FileClassifier.JobManager.lib.Databases.Base;
using FileClassifier.JobManager.lib.Databases.Tables;

using Microsoft.AspNetCore.Mvc;

namespace FileClassifier.JobManager.REST.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HostController : ControllerBase
    {
        private IDatabase _database;

        public HostController(IDatabase database)
        {
            _database = database;
        }

        [HttpPost]
        public void Post([FromBody]Hosts host)
        {
            _database.AddUpdateHost(host);       
        }

        [HttpDelete]
        public void DeleteHost(Guid id)
        {
            _database.DeleteHost(id);
        }
    }
}