using System;

using FileClassifier.JobManager.REST.Databases.Tables;

namespace FileClassifier.JobManager.REST.Databases.Base
{
    public interface IDatabase
    {
        bool DeleteJob(Guid id);

        bool AddJob(Jobs item);

        bool UpdateJob(Jobs item);
    }
}