using System;

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

        [HttpGet]
        public FileResult Download([FromQuery]Guid id)
        {
            var job = _database.GetJob(id);

            return File(job.Model, System.Net.Mime.MediaTypeNames.Application.Octet, $"{job.Name}.mdl");
        }
    }
}