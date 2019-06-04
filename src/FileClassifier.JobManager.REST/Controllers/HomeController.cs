using System;
using System.Linq;

using FileClassifier.JobManager.lib.Databases.Base;
using FileClassifier.JobManager.lib.Databases.Tables;
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
        public IActionResult AddJob([FromQuery]string name, [FromQuery]string trainingDataPath, [FromQuery]string modelType)
        {
            var job = new Jobs
            {
                ID = Guid.NewGuid(),
                SubmissionTime = DateTime.Now,
                Name = name,
                TrainingDataPath = trainingDataPath,
                ModelType = modelType
            };

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

                    job.AssignedHost = host.Name;

                    break;
                }

                if (string.IsNullOrEmpty(job.AssignedHost))
                {
                    job.AssignedHost = lib.Common.Constants.UNASSIGNED_JOB;
                }
            }
            else
            {
                job.AssignedHost = lib.Common.Constants.UNASSIGNED_JOB;
            }

            _database.AddJob(job);

            return Index();
        }

        [HttpGet]
        [Route("/Download")]
        public FileResult Download([FromQuery]Guid id)
        {
            var job = _database.GetJob(id);

            return File(job.Model, System.Net.Mime.MediaTypeNames.Application.Octet, $"{job.Name}.mdl");
        }
    }
}