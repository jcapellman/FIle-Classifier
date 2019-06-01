using FileClassifier.JobManager.lib.Databases.Base;

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
    }
}