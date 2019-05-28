using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FileClassifier.JobManager.REST.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            // Returns all current jobs
            return new string[] { "value1", "value2" };
        }

        [HttpGet("{id}")]
        public ActionResult<string> Get(Guid id)
        {
            // Return Job Status based on the 
            return "value";
        }

        [HttpPost]
        public void Post([FromBody] string value)
        {
            // Handle Submission of Jobs
        }

        [HttpDelete("{id}")]
        public void Delete(Guid id)
        {
            // Deletes the specified job
        }
    }
}