using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace FileClassifier.JobManager.REST
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseUrls("http://0.0.0.0:5000")
                .UseKestrel(options => {
                    options.Limits.MaxRequestBodySize = null;
                })
                .Build();
    }
}