using System.Threading.Tasks;

using FileClassifier.JobManager.lib.Databases.Tables;
using FileClassifier.JobManager.lib.Handlers.Base;

namespace FileClassifier.JobManager.lib.Handlers
{
    public class WorkerHandler : BaseHandler
    {
        public WorkerHandler(string rootUrl) : base(rootUrl) { }
    
        public async Task<Jobs> GetWorkAsync(string hostName) => await GetAsync<Jobs>($"Worker?hostName={hostName}");

        public async Task<bool> UpdateWorkAsync(Jobs job) => await PostAsync<Jobs>("Worker", job);
    }
}