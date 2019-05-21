using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using FileClassifier.Web.Models;

using FileClassifier.lib.ML.Clustering;
using FileClassifier.lib.ML.Classification;

namespace FileClassifier.Web.Controllers
{
    public class HomeController : Controller
    {
        private static readonly ClusteringEngine ClusteringEngine = new ClusteringEngine();

        private static readonly ClassificationEngine ClassificationEngine = new ClassificationEngine();

        public IActionResult Index() => View();

        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);

                var options = new lib.Options.ClassifierCommandLineOptions
                {
                    FileName = file.FileName,
                    LogLevel = lib.Enums.LogLevels.DEBUG
                };

                var response = ClusteringEngine.Predict(new lib.Common.ClassifierResponseItem(memoryStream.ToArray(), file.FileName), options);

                response = ClassificationEngine.Predict(response, options);

                return View("Result", response);
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}