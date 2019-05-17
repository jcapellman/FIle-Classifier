using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using FileClassifier.Web.Models;

using FileClassifier.lib.ML.Clustering;

namespace FileClassifier.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);

                var classifier = new ClusteringEngine();

                var model = new ResultViewModel();

                model.ClusterResult = classifier.Predict(new lib.Common.ClassifierResponseItem(memoryStream.ToArray(), file.FileName), new lib.Options.ClassifierCommandLineOptions
                {
                    FileName = file.FileName,
                    LogLevel = lib.Enums.LogLevels.DEBUG
                });

                return View("Result", model);
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}