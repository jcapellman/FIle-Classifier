using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using FileClassifier.JobManager.lib.Databases.Tables;

using Newtonsoft.Json;

namespace FileClassifier.JobManager.lib.Handlers
{
    public class JobHandler
    {
        private readonly string _rootUrl;

        public JobHandler(string rootURL)
        {
            _rootUrl = rootURL;
        }

        public async Task<bool> AddNewJobAsync(Jobs job)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri(_rootUrl);

                    var json = JsonConvert.SerializeObject(job);

                    var stringContent = new StringContent(json, Encoding.UTF8, "application/json");

                    var response = httpClient.PostAsync("Job", stringContent).Result;

                    var responseBody = await response.Content.ReadAsStringAsync();

                    return response.IsSuccessStatusCode;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);

                return false;
            }
        }
    }
}