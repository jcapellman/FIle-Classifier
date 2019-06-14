using System.Threading.Tasks;

using FileClassifier.JobManager.lib.Databases.Tables;
using FileClassifier.JobManager.lib.Handlers.Base;

namespace FileClassifier.JobManager.lib.Handlers
{
    public class JobHandler : BaseHandler
    {
        public JobHandler(string rootUrl) : base(rootUrl) { }
    
        public async Task<bool> AddNewJobAsync(Jobs job) => await PostAsync("Job", job);
    }
}