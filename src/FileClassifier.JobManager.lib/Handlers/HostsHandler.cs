using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using FileClassifier.JobManager.lib.Databases.Tables;

using Newtonsoft.Json;

namespace FileClassifier.JobManager.lib.Handlers
{
    public class HostsHandler
    {
        private readonly string _rootURL;

        public HostsHandler(string rootURL)
        {
            _rootURL = rootURL;
        }

        public async Task<bool> AddUpdateHostAsync(Hosts host)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri(_rootURL);

                    var json = JsonConvert.SerializeObject(host);

                    Console.WriteLine(json);

                    StringContent stringContent = new StringContent(json, Encoding.UTF8, "application/json");

                    var response = httpClient.PostAsync("Host", stringContent).Result;

                    var responseBody = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        return true;
                    }

                    return false;
                }
            } catch (Exception ex)
            {
                Console.WriteLine(ex);

                return false;
            }
        }
    }
}