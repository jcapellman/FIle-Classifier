using System;

using FileClassifier.JobManager.lib.Databases.Base;
using FileClassifier.JobManager.lib.Databases.Tables;

using Microsoft.AspNetCore.Mvc;

namespace FileClassifier.JobManager.REST.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HostController : BaseController
    {
        public HostController(IDatabase database) : base(database) { }
    
        [HttpPost]
        public void Post([FromBody]Hosts host)
        {
            Database.AddUpdateHost(host);       
        }

        [HttpDelete]
        public void DeleteHost(Guid id)
        {
            Database.DeleteHost(id);
        }
    }
}