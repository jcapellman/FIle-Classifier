using System;
using System.Collections.Generic;
using System.Net.Http;
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

        public async Task<List<Jobs>> GetWorkAsync(string hostName)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri(_rootURL);

                    var response = httpClient.GetAsync($"Worker?hostName={hostName}").Result;

                    var responseBody = await response.Content.ReadAsStringAsync();

                    return response.IsSuccessStatusCode ? JsonConvert.DeserializeObject<List<Jobs>>(responseBody) : null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);

                return null;
            }
        }
    }
}