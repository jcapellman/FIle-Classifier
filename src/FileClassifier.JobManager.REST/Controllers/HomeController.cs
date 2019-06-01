using FileClassifier.JobManager.lib.Databases.Base;
using FileClassifier.JobManager.REST.Models;

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

        public IActionResult Index() => View("Index", new HomeDashboardModel {
            Jobs = _database.GetJobs(),
            Hosts = _database.GetHosts()
        });
    }
}