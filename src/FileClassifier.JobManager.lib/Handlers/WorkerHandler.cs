using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using FileClassifier.JobManager.lib.Databases.Tables;

using Newtonsoft.Json;

namespace FileClassifier.JobManager.lib.Handlers
{
    public class WorkerHandler
    {
        private readonly string _rootURL;

        public WorkerHandler(string rootURL)
        {
            _rootURL = rootURL;
        }

        public async Task<Jobs> GetWorkAsync(string hostName)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri(_rootURL);

                    var response = httpClient.GetAsync($"Worker?hostName={hostName}").Result;

                    var responseBody = await response.Content.ReadAsStringAsync();

                    return response.IsSuccessStatusCode ? JsonConvert.DeserializeObject<Jobs>(responseBody) : null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);

                return null;
            }
        }

        public async Task<bool> UpdateWorkAsync(Jobs job)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri(_rootURL);

                    var json = JsonConvert.SerializeObject(job);

                    var stringContent = new StringContent(json, Encoding.UTF8, "application/json");

                    var response = httpClient.PostAsync("Worker", stringContent).Result;

                    var responseBody = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        return true;
                    }

                    return false;
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