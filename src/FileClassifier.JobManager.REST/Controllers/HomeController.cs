using System;
using System.Linq;

using FileClassifier.JobManager.lib.Databases.Base;
using FileClassifier.JobManager.lib.Databases.Tables;
using FileClassifier.JobManager.REST.Models;

using FileClassifier.lib.Enums;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FileClassifier.JobManager.REST.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController(IDatabase database) : base(database) { }
    
        public IActionResult Index() => View("Index", new HomeDashboardModel {
            Jobs = Database.GetJobs(),
            Hosts = Database.GetHosts(),
            ModelTypes = Enum.GetNames(typeof(ModelType)).OrderBy(a => a).Select(a => new SelectListItem(a, a)).ToList()
        });

        [HttpGet]
        public IActionResult AddJob([FromQuery]string name, [FromQuery]string trainingDataPath, [FromQuery]string modelType)
        {
            var job = new Jobs
            {
                Name = name,
                TrainingDataPath = trainingDataPath,
                ModelType = modelType
            };

            SaveJob(job);

            return Index();
        }

        [HttpGet]
        [Route("/Download")]
        public FileResult Download([FromQuery]Guid id)
        {
            var job = Database.GetJob(id);

            return job == null ? 
                File(new byte[0], System.Net.Mime.MediaTypeNames.Text.Plain, "Model not found") : 
                File(job.Model, System.Net.Mime.MediaTypeNames.Application.Octet, $"{job.Name}.mdl");
        }
    }
}