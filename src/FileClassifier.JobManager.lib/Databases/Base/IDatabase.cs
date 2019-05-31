using System;
using System.Collections.Generic;

using FileClassifier.JobManager.lib.Databases.Tables;

namespace FileClassifier.JobManager.lib.Databases.Base
{
    public interface IDatabase
    {
        bool DeleteJob(Guid id);

        bool AddJob(Jobs item);

        bool UpdateJob(Jobs item);

        Jobs GetJob(Guid id);

        List<Jobs> GetJobs();
    }
}