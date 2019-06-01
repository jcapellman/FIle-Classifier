using FileClassifier.JobManager.lib.Databases.Base;

using Microsoft.AspNetCore.Mvc;

namespace FileClassifier.JobManager.REST.Controllers
{
    public class HomeController : Controller
    {
        private readonly IDatabase _database;

        public HomeController(IDatabase database)
        {
            _database = database;
        }

        public IActionResult Index() => View("Index", _database.GetJobs());
    }
}