using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace FileClassifier.JobManager.lib.Handlers.Base
{
    public class BaseHandler
    {
        private readonly string _rootUrl;

        protected BaseHandler(string rootUrl)
        {
            _rootUrl = rootUrl;
        }

        protected async Task<T> GetAsync<T>(string url)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri(_rootUrl);

                    var response = httpClient.GetAsync(url).Result;

                    var responseBody = await response.Content.ReadAsStringAsync();

                    return response.IsSuccessStatusCode ? JsonConvert.DeserializeObject<T>(responseBody) : default;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);

                return default;
            }
        }

        protected async Task<bool> PostAsync<T>(string url, T data)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri(_rootUrl);

                    var json = JsonConvert.SerializeObject(data);

                    var stringContent = new StringContent(json, Encoding.UTF8, "application/json");

                    var response = await httpClient.PostAsync(url, stringContent);

                    await response.Content.ReadAsStringAsync();

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