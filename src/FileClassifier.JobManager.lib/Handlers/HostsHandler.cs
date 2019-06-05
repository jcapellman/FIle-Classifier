using System.Threading.Tasks;

using FileClassifier.JobManager.lib.Databases.Tables;
using FileClassifier.JobManager.lib.Handlers.Base;

namespace FileClassifier.JobManager.lib.Handlers
{
    public class HostsHandler : BaseHandler
    {
        public HostsHandler(string rootUrl) : base(rootUrl) { }
    
        public async Task<bool> AddUpdateHostAsync(Hosts host) => await PostAsync<Hosts>("Hosts", host);
    }
}